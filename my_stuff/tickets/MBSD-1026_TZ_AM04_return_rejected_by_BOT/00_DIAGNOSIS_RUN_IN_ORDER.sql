/* =====================================================================================
   MBSD-1026 - DIAGNOSIS, ROUND 2 (updated 25-Nov-2026 after reading the live objects)
   Client DB: BRNET_MB_LIVE on MNLDHQCBSDB02\SQL2019. Full findings: NOTES.md

   WHERE WE ARE
   - Cheque 211 was loaded 20-Nov and unpaid manually 21-Nov 11:30. The file was generated ~12:59.
     BOT rejected it: "missing OrgnTxID".
   - The live generator looks up TrxID (= OrgnlTxId) ONLY in t_IncomingTransactions. The History
     lookup is commented out. That table holds today's rows only.
   - The 26-Aug change is NOT the cause (EFT trims only).
   - 13-Jul-2026 change in p_AddInwardsTZ: auto-unpay at load REMOVED. All items now wait for
     a manual decision, so next-day unpays happen, and a next-day unpay = inward no longer live.
   - 15-Jun-2026 change in p_AddInwardsTZ: "_" stripped from BOT's TxId at load. The stored TrxID
     differs from BOT's.
   - Generator "28/26" change: unpaid cheques may be selected TWICE (UNION ALL).

   DONE: live texts saved in my_stuff/sql/11_25_2026 (generator + 26-Aug backup, v_Clearing,
         p_ProcessIncomingTxns, p_AddOutgoingTrx, p_AddIncomingTrx, p_AddInwardsTZ,
         p_SuperviseTrx + backup, p_InsertTransactions).
   DROPPED: old R1 (History check), answered by reading the texts.

   Everything below is READ-ONLY. Run in this order.
   ===================================================================================== */


/* ---------- STEP 1. What moves inwards out of t_IncomingTransactions, and when? ----------
   This is the missing piece: who moves the row, and can it run during the day?
   sp_helptext every object returned and save it to my_stuff/sql/11_25_2026.                */
SELECT 'S1' AS Step, OBJECT_NAME(object_id) AS Obj, OBJECTPROPERTY(object_id, 'IsProcedure') AS IsProc
FROM sys.sql_modules
WHERE definition LIKE '%t_IncomingTransactionsHistory%'
   OR definition LIKE '%CEDate%'
   OR definition LIKE '%DELETE%FROM%t[_]IncomingTransactions%'
   OR definition LIKE '%TRUNCATE%t[_]IncomingTransactions%'
ORDER BY 2


/* ---------- STEP 2. Cheque 211 in History: TrxID filled? Any moved-at / archive time? ---------- */
SELECT 'S2' AS Step, *
FROM t_IncomingTransactionsHistory (NOLOCK)
WHERE ColumnID = 4344 OR (ChequeID = 211 AND ABS(Amount) = 5323100)


/* ---------- STEP 3. Next-day unpays since July, before vs after the 13-Jul change ----------
   Expect: few or none before 13-Jul; after it, cheque 211 + the 2nd November case.             */
SELECT 'S3' AS Step, v.TrxRowID, v.ChequeID, v.Amount, CONVERT(DATE, v.[Date]) AS UnpayDate,
       CONVERT(DATE, h.[Date]) AS InwardDate, DATEDIFF(DAY, h.[Date], v.[Date]) AS DaysLater, v.Reference
FROM v_Clearing v (NOLOCK)
LEFT JOIN t_IncomingTransactionsHistory h (NOLOCK) ON h.ColumnID = v.ColumnID
WHERE v.TrxType = 'OC' AND v.ReturnCodeID <> '00' AND ISNULL(v.IsDeleted, 0) = 0
  AND v.[Date] >= '2026-06-01'
  AND (h.ColumnID IS NULL OR DATEDIFF(DAY, h.[Date], v.[Date]) <> 0)
ORDER BY v.[Date]


/* ---------- STEP 4. Files from the application server (prove it from what was sent) ----------
   F1  Our file DRUNPBARCTZTZ211120261260 (.Q in <OutgoingFiles>\Files, and the signed copy):
       - is <OrgnlTxId> missing?            (confirms the mechanism)
       - does cheque 211 appear TWICE?      (confirms the "28/26" UNION duplication)
   F2  BOT's response to it (*.V/*.R/*.C/*.RC, ARCHIVEs\20261121\VALIDATIONS): exact reason and time.
   F3  BOT's inward file of 20-Nov (msg QNF2611201243...): TxId 'doc-001-495-004342_1' WITH "_"?
       (confirms the 15-Jun underscore strip changes the ID we return)
   F4  <ClearingErrorLogFilePath>\ClearingErrorLog\20261121.txt
   F5  CommonMods.dll / EXE version and date in use.
   F6  The other November incident: cheque no., amount, dates, account. Run STEP 2-3 for it.  */


/* ---------- STEP 5. Priority 3 texts (context), save to my_stuff/sql/11_25_2026 ---------- */
EXEC sp_helptext 'p_AddClearingFileStatus'              -- needed for STEP 6 (where BOT's reject is stored)
EXEC sp_helptext 'ps_UpdateGLBalance'                   -- EOD: t_TrxClearing -> t_AccountTrxClearing
EXEC sp_helptext 'sb_EndOfDay'
EXEC sp_helptext 'p_AutoUnpayIncomingTxns'              -- no longer called from p_AddInwardsTZ since 13-Jul?
EXEC sp_helptext 'Proc_CreateXMLFiles_Tanzania_CTS'     -- generator used when nothing is ticked
EXEC sp_helptext 'f_CRB_ReturnCodeDescriptionsForUnpay'


/* ---------- STEP 6. BOT's rejection as stored (table name from p_AddClearingFileStatus) ----------
   SELECT * FROM <that table> WHERE MessageID = 'DRUNPBARCTZTZ211120261260'                     */


/* ---------- STEP 7 (low priority). Who altered t_Transaction + triggers on 21-Nov 16:51? ---------- */
SELECT TOP 5 'S7' AS Step, * FROM t_DBAuditSpecificationData_21112026
-- then filter on time ~16:51 and object t_Transaction / tr_% using those column names
