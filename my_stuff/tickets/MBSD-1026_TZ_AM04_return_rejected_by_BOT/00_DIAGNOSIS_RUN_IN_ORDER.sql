/* =====================================================================================
   MBSD-1026 - DIAGNOSIS, ROUND 3 (25-Nov-2026). Findings so far: NOTES.md
   Client DB: BRNET_MB_LIVE on MNLDHQCBSDB02\SQL2019. READ-ONLY.

   WHERE WE ARE
   - Cheque 211 (ColumnID 4344) was unpaid at 21-Nov 11:30:58 by MB0194. The return file
     (DRUNPBARCTZTZ211120261260) was built at ~12:59. BOT rejected it: "missing OrgnTxID".
   - The live generator reads TrxID ONLY from t_IncomingTransactions (History lookup commented out).
   - Cheque 211 is now in t_IncomingTransactionsHistory WITH its TrxID. So something moved it out
     of the live table between 11:30 and ~12:59. FIND THAT MOVER.
   - Suspects (from round 2 S1): p_DeleteUnpostedInwardsTrx, sb_ClearEffects / ps_ClearEffects
     (CEDate = Clear-Effects date?), pc_SystemBranchStatus, p_DoInwardsFileTrxValidationsTZ.

   HOW TO RUN
   - PART A: run as ONE batch in Results to Text (Ctrl+T) or Grid. Output is one column "Line"
     with labelled values. Copy all the lines back.
   - PART B: sp_helptext, Results to File (Ctrl+Shift+F), save to my_stuff/sql/11_25_2026.
   ===================================================================================== */


/* =====================================================================================
   PART A - one batch, labelled output
   ===================================================================================== */
SET NOCOUNT ON;
IF OBJECT_ID('tempdb..#Out') IS NOT NULL DROP TABLE #Out;
CREATE TABLE #Out (Seq INT IDENTITY(1,1), Line NVARCHAR(MAX));

/* ---------- A1. Cheque 211 History row, every column labelled (images and long text skipped) ---------- */
DECLARE @sql NVARCHAR(MAX) = N'';
SELECT @sql = @sql
     + N'INSERT INTO #Out(Line) SELECT N''A1 History 4344 | ' + c.name + N' = '' + ISNULL('
     + CASE WHEN TYPE_NAME(c.system_type_id) IN ('datetime', 'smalldatetime', 'datetime2', 'date')
            THEN N'CONVERT(NVARCHAR(30), ' + QUOTENAME(c.name) + N', 121)'
            ELSE N'CONVERT(NVARCHAR(400), ' + QUOTENAME(c.name) + N')' END
     + N', N''NULL'') FROM t_IncomingTransactionsHistory (NOLOCK) WHERE ColumnID = 4344;' + NCHAR(10)
FROM sys.columns c
WHERE c.object_id = OBJECT_ID('t_IncomingTransactionsHistory')
  AND TYPE_NAME(c.system_type_id) NOT IN ('image', 'varbinary', 'binary', 'text', 'ntext', 'xml', 'timestamp')
  AND c.max_length <> -1
ORDER BY c.column_id;
BEGIN TRY EXEC sp_executesql @sql; END TRY
BEGIN CATCH INSERT INTO #Out(Line) SELECT 'A1 ERROR | ' + ERROR_MESSAGE(); END CATCH

/* ---------- A2. Which objects INSERT into History / DELETE from the live inward table / use CEDate ---------- */
INSERT INTO #Out(Line)
SELECT 'A2 Mover? | ' + OBJECT_NAME(m.object_id)
       + ' | ' + o.type_desc
       + ' | modified ' + CONVERT(VARCHAR(16), o.modify_date, 120)
       + ' | ' + CASE WHEN m.definition LIKE '%INTO%t[_]IncomingTransactionsHistory%' THEN 'INSERTS-HISTORY ' ELSE '' END
               + CASE WHEN m.definition LIKE '%DELETE%FROM%t[_]IncomingTransactions[^H_]%'
                        OR m.definition LIKE '%DELETE%t[_]IncomingTransactions[^H_]%' THEN 'DELETES-LIVE ' ELSE '' END
               + CASE WHEN m.definition LIKE '%[^a-z]CEDate%' THEN 'USES-CEDATE' ELSE '' END
FROM sys.sql_modules m
JOIN sys.objects o ON o.object_id = m.object_id
WHERE m.definition LIKE '%INTO%t[_]IncomingTransactionsHistory%'
   OR m.definition LIKE '%DELETE%t[_]IncomingTransactions[^H_]%'
   OR m.definition LIKE '%[^a-z]CEDate%'
ORDER BY OBJECT_NAME(m.object_id);

/* ---------- A3. Unpays since June: day the inward was LOADED (ExtraDetails.TrxDate) vs unpay day ----------
   LoadedDaysBefore >= 1 = a next-day unpay (the case where the inward may have left the live table). */
INSERT INTO #Out(Line)
SELECT 'A3 Unpay | Chq ' + CAST(v.ChequeID AS VARCHAR(10))
       + ' | Amt ' + CONVERT(VARCHAR(20), CAST(ABS(v.Amount) AS MONEY), 1)
       + ' | Unpaid ' + CONVERT(VARCHAR(10), v.[Date], 120)
       + ' | Loaded ' + ISNULL(CONVERT(VARCHAR(10), ed.TrxDate, 120), '?')
       + ' | LoadedDaysBefore ' + ISNULL(CAST(DATEDIFF(DAY, ed.TrxDate, v.[Date]) AS VARCHAR(5)), '?')
       + ' | Where ' + CASE WHEN i.ColumnID IS NOT NULL THEN 'LIVE' WHEN h.ColumnID IS NOT NULL THEN 'HISTORY' ELSE 'NOT FOUND' END
       + ' | Ref ' + ISNULL(v.Reference, '-')
FROM v_Clearing v (NOLOCK)
LEFT JOIN t_IncomingTransactions i (NOLOCK) ON i.ColumnID = v.ColumnID AND i.ChequeID = v.ChequeID
LEFT JOIN t_IncomingTransactionsHistory h (NOLOCK) ON h.ColumnID = v.ColumnID AND h.ChequeID = v.ChequeID
OUTER APPLY (SELECT TOP 1 e.TrxDate
             FROM t_IncomingClrTrxExtraDetails e (NOLOCK)
             WHERE e.TrxID = COALESCE(i.TrxID, h.TrxID)
               AND e.OrgnlMsgId = COALESCE(i.MsgID, h.MsgID)) ed
WHERE v.TrxType = 'OC' AND v.ReturnCodeID <> '00' AND ISNULL(v.IsDeleted, 0) = 0
  AND v.VoucherCode <> '40'
  AND v.[Date] >= '2026-06-01'
ORDER BY v.[Date];

INSERT INTO #Out(Line)
SELECT 'A3 Summary | Unpays=' + CAST(COUNT(*) AS VARCHAR(10))
       + ' | NextDayOrLater=' + CAST(SUM(CASE WHEN DATEDIFF(DAY, ed.TrxDate, v.[Date]) >= 1 THEN 1 ELSE 0 END) AS VARCHAR(10))
FROM v_Clearing v (NOLOCK)
LEFT JOIN t_IncomingTransactions i (NOLOCK) ON i.ColumnID = v.ColumnID AND i.ChequeID = v.ChequeID
LEFT JOIN t_IncomingTransactionsHistory h (NOLOCK) ON h.ColumnID = v.ColumnID AND h.ChequeID = v.ChequeID
OUTER APPLY (SELECT TOP 1 e.TrxDate FROM t_IncomingClrTrxExtraDetails e (NOLOCK)
             WHERE e.TrxID = COALESCE(i.TrxID, h.TrxID) AND e.OrgnlMsgId = COALESCE(i.MsgID, h.MsgID)) ed
WHERE v.TrxType = 'OC' AND v.ReturnCodeID <> '00' AND ISNULL(v.IsDeleted, 0) = 0
  AND v.VoucherCode <> '40' AND v.[Date] >= '2026-06-01';

/* ---------- A4. Where BOT's responses are stored: tables used by p_AddClearingFileStatus ---------- */
INSERT INTO #Out(Line)
SELECT 'A4 p_AddClearingFileStatus writes to | ' + ISNULL(d.referenced_schema_name + '.', '') + d.referenced_entity_name
FROM sys.sql_expression_dependencies d
WHERE d.referencing_id = OBJECT_ID('p_AddClearingFileStatus');

/* ---------- A5. Branch clearing dates (CEDate etc.) ---------- */
INSERT INTO #Out(Line)
SELECT 'A5 Branch ' + OurBranchID
       + ' | SOD ' + ISNULL(CONVERT(VARCHAR(10), SODDate, 120), '-')
       + ' | EOD ' + ISNULL(CONVERT(VARCHAR(10), EODDate, 120), '-')
       + ' | CE ' + ISNULL(CONVERT(VARCHAR(10), CEDate, 120), '-')
FROM t_SystemBranchStatus (NOLOCK)
ORDER BY OurBranchID;

SELECT Line FROM #Out ORDER BY Seq;


/* =====================================================================================
   PART B - save texts (Results to File), one file per object, to my_stuff/sql/11_25_2026
   DONE: p_DeleteUnpostedInwardsTrx, sb_ClearEffects, ps_ClearEffects, pc_SystemBranchStatus,
         p_DoInwardsFileTrxValidationsTZ (none of them moves processed inwards; see NOTES)
   NEXT (in this order): the only 3 objects that INSERT into t_IncomingTransactionsHistory
   ===================================================================================== */
EXEC sp_helptext 'Proc_CreateXMLFiles_Tanzania_CTS'   -- PRIME SUSPECT: non-selected generator, daytime
EXEC sp_helptext 'ps_UpdateGLBalance'                 -- EOD mover
EXEC sp_helptext 'rw_IncomingTransactions'            -- report that inserts into History?
EXEC sp_helptext 'p_AddClearingFileStatus'            -- re-save: the previous file was empty (0 bytes)
EXEC sp_helptext 'sb_EndOfDay'


/* =====================================================================================
   AFTER A + B (round 4, to prepare once the mover is known)
   - Its run time on 21-Nov: audit table t_DBAuditSpecificationData_21112026 or the app log.
   - BOT's rejection rows (table from A4) for DRUNPBARCTZTZ211120261260.
   - Files: our DRUNP file (missing OrgnlTxId? cheque twice?), BOT's response, BOT's 20-Nov
     inward file (TxId with "_1"?), error log 20261121.txt, DLL version, 2nd November case.
   ===================================================================================== */
