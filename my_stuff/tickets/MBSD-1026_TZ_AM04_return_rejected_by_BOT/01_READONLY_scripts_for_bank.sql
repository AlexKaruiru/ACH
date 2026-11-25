/* =====================================================================================
   TICKET : MBSD-1026 - FAILURE TO UNPAY CHEQUE (TZ) - AM04 return rejected by BOT, cheque settled
   Cheque : 000211, TZS 5,323,100.00, drawer SAFE HAND COMPANY LIMITED (A/C 024244426061)
            presented by BARCTZTZ on 21-Nov-2026
   ACH    : Tx id (Reference) doc-001-495-004342_1, End To End AB019MK1795167518917

   ALL SCRIPTS BELOW ARE READ-ONLY (SELECT / sp_helptext). Please run in the BRNET
   clearing database and send back each result set (Excel or CSV is fine).
   ===================================================================================== */

DECLARE @TxId      VARCHAR(100) = 'doc-001-495-004342_1'
DECLARE @EndToEnd  VARCHAR(100) = 'AB019MK1795167518917'
DECLARE @ChequeID  INT          = 211
DECLARE @Amount    MONEY        = 5323100
DECLARE @FromDate  DATETIME     = '2026-11-20'
DECLARE @ToDate    DATETIME     = '2026-11-22'

/* ---------- Q1. The inward cheque as loaded from the ACH file ---------- */
SELECT 'Q1 t_IncomingTransactions' AS Src, *
FROM t_IncomingTransactions (NOLOCK)
WHERE TrxID = @TxId OR (ChequeID = @ChequeID AND ABS(Amount) = @Amount)

IF OBJECT_ID('t_IncomingTransactionsHistory') IS NOT NULL
    EXEC ('SELECT ''Q1 t_IncomingTransactionsHistory'' AS Src, * FROM t_IncomingTransactionsHistory (NOLOCK)
           WHERE TrxID = ''doc-001-495-004342_1'' OR (ChequeID = 211 AND ABS(Amount) = 5323100)')

/* ---------- Q2. KEY CHECK: original-transaction details for this TxId ----------
   More than ONE row here (different OrgnlMsgId / dates) means the return file may
   have carried another cheque's OrgnlMsgId / OrgnlEndToEndId -> BOT reject.        */
SELECT 'Q2 ExtraDetails for TxId' AS Src, TrxID, TrxDate, OrgnlMsgId, OrgnlInstrId, OrgnlEndToEnd,
       IntrBkSttlmAmt, SourceBIC, DbtrAcct, CdtrAcct, ReqdColltnDate
FROM t_IncomingClrTrxExtraDetails (NOLOCK)
WHERE TrxID = @TxId OR OrgnlEndToEnd = @EndToEnd
ORDER BY TrxDate

/* ---------- Q3. How common are repeated TxIds (last 180 days)? ---------- */
SELECT TOP 200 'Q3 Repeated TxIds' AS Src, TrxID, COUNT(*) AS Cnt,
       COUNT(DISTINCT OrgnlMsgId) AS DistinctMsgIds, MIN(TrxDate) AS FirstSeen, MAX(TrxDate) AS LastSeen
FROM t_IncomingClrTrxExtraDetails (NOLOCK)
WHERE TrxDate >= DATEADD(DAY, -180, GETDATE())
GROUP BY TrxID
HAVING COUNT(DISTINCT OrgnlMsgId) > 1
ORDER BY LastSeen DESC

/* ---------- Q4. The unpay (OC return) rows created for this cheque ---------- */
SELECT 'Q4 t_TrxClearing' AS Src, c.*
FROM t_TrxClearing c (NOLOCK)
WHERE c.ColumnID IN (SELECT CAST(ColumnID AS VARCHAR(100)) FROM t_IncomingTransactions (NOLOCK) WHERE TrxID = @TxId)
   OR (c.ChequeID = @ChequeID AND ABS(c.Amount) = @Amount)

SELECT 'Q4 t_AccountTrxClearing' AS Src, c.*
FROM t_AccountTrxClearing c (NOLOCK)
WHERE c.ColumnID IN (SELECT CAST(ColumnID AS VARCHAR(100)) FROM t_IncomingTransactions (NOLOCK) WHERE TrxID = @TxId)
   OR (c.ChequeID = @ChequeID AND ABS(c.Amount) = @Amount)

/* ---------- Q5. Every unpaid-cheque return file (DRUNP...) generated 20-22 Nov ----------
   Same Reference appearing for rows generated at different times = duplicate MsgId. */
SELECT 'Q5 DRUNP files' AS Src, Reference, BankID, COUNT(*) AS Items, SUM(ABS(Amount)) AS Total,
       MIN(TrxRowID) AS MinTrxRowID, MAX(TrxRowID) AS MaxTrxRowID
FROM t_TrxClearing (NOLOCK)
WHERE Reference LIKE 'DRUNP%' AND [Date] BETWEEN @FromDate AND @ToDate
GROUP BY Reference, BankID
ORDER BY Reference

/* ---------- Q6. Core banking postings for the cheque (statement shows debit + 'Cheque Unpay') ---------- */
SELECT 'Q6 t_Transaction' AS Src, *
FROM t_Transaction (NOLOCK)
WHERE ReferenceNo IN (SELECT CAST(ColumnID AS VARCHAR(100)) FROM t_IncomingTransactions (NOLOCK) WHERE TrxID = @TxId)

/* ---------- Q7. Return-code mapping used in the file ---------- */
SELECT 'Q7 ReturnCode map' AS Src, '50' AS Code, dbo.f_CRB_ReturnCodeDescriptionsForUnpay('50') AS SentAs
UNION ALL SELECT 'Q7 ReturnCode map', '45', dbo.f_CRB_ReturnCodeDescriptionsForUnpay('45')
UNION ALL SELECT 'Q7 ReturnCode map', '46', dbo.f_CRB_ReturnCodeDescriptionsForUnpay('46')

/* ---------- Q8. Any DB objects changed recently? ("no code changes for a year") ---------- */
SELECT 'Q8 Recently modified objects' AS Src, name, type_desc, create_date, modify_date
FROM sys.objects
WHERE modify_date >= '2026-08-01' AND type IN ('P', 'FN', 'IF', 'TF', 'V', 'TR', 'U')
ORDER BY modify_date DESC

/* ---------- Q9. CURRENT deployed text of every object in the unpay / return-file path ----------
   Our copies are old, so we need what is actually running. One result set per object.        */
IF OBJECT_ID('Proc_CreateXMLFiles_Tanzania_Selected_CTS') IS NOT NULL EXEC sp_helptext 'Proc_CreateXMLFiles_Tanzania_Selected_CTS' ELSE SELECT 'MISSING: Proc_CreateXMLFiles_Tanzania_Selected_CTS' AS Q9
IF OBJECT_ID('Proc_CreateXMLFiles_Tanzania_CTS') IS NOT NULL EXEC sp_helptext 'Proc_CreateXMLFiles_Tanzania_CTS' ELSE SELECT 'MISSING: Proc_CreateXMLFiles_Tanzania_CTS' AS Q9
IF OBJECT_ID('v_Clearing') IS NOT NULL EXEC sp_helptext 'v_Clearing' ELSE SELECT 'MISSING: v_Clearing' AS Q9
IF OBJECT_ID('p_ProcessIncomingTxns') IS NOT NULL EXEC sp_helptext 'p_ProcessIncomingTxns' ELSE SELECT 'MISSING: p_ProcessIncomingTxns' AS Q9
IF OBJECT_ID('p_AutoUnpayIncomingTxns') IS NOT NULL EXEC sp_helptext 'p_AutoUnpayIncomingTxns' ELSE SELECT 'MISSING: p_AutoUnpayIncomingTxns' AS Q9
IF OBJECT_ID('p_AddIncomingTrx') IS NOT NULL EXEC sp_helptext 'p_AddIncomingTrx' ELSE SELECT 'MISSING: p_AddIncomingTrx' AS Q9
IF OBJECT_ID('p_AddOutgoingTrx') IS NOT NULL EXEC sp_helptext 'p_AddOutgoingTrx' ELSE SELECT 'MISSING: p_AddOutgoingTrx' AS Q9
IF OBJECT_ID('p_AddInwardsTZ') IS NOT NULL EXEC sp_helptext 'p_AddInwardsTZ' ELSE SELECT 'MISSING: p_AddInwardsTZ' AS Q9
IF OBJECT_ID('p_AddInwardsFromTextFileTZ') IS NOT NULL EXEC sp_helptext 'p_AddInwardsFromTextFileTZ' ELSE SELECT 'MISSING: p_AddInwardsFromTextFileTZ' AS Q9
IF OBJECT_ID('sp_InwardsCheques') IS NOT NULL EXEC sp_helptext 'sp_InwardsCheques' ELSE SELECT 'MISSING: sp_InwardsCheques' AS Q9
IF OBJECT_ID('p_GetClearingData') IS NOT NULL EXEC sp_helptext 'p_GetClearingData' ELSE SELECT 'MISSING: p_GetClearingData' AS Q9
IF OBJECT_ID('p_AddClearingFileStatus') IS NOT NULL EXEC sp_helptext 'p_AddClearingFileStatus' ELSE SELECT 'MISSING: p_AddClearingFileStatus' AS Q9
IF OBJECT_ID('f_CRB_ReturnCodeDescriptionsForUnpay') IS NOT NULL EXEC sp_helptext 'f_CRB_ReturnCodeDescriptionsForUnpay' ELSE SELECT 'MISSING: f_CRB_ReturnCodeDescriptionsForUnpay' AS Q9
IF OBJECT_ID('f_GetClearingBankIDFromSwiftID') IS NOT NULL EXEC sp_helptext 'f_GetClearingBankIDFromSwiftID' ELSE SELECT 'MISSING: f_GetClearingBankIDFromSwiftID' AS Q9
IF OBJECT_ID('f_GetWorkingDate') IS NOT NULL EXEC sp_helptext 'f_GetWorkingDate' ELSE SELECT 'MISSING: f_GetWorkingDate' AS Q9

/* ---------- Q10. Which XML/unpay objects exist (catch renamed or extra versions in use) ---------- */
SELECT 'Q10 Related objects' AS Src, name, type_desc, create_date, modify_date
FROM sys.objects
WHERE name LIKE 'Proc_CreateXMLFiles%' OR name LIKE '%Unpa%' OR name LIKE '%Inward%'
   OR name LIKE '%Incoming%' OR name LIKE '%ClearingFileStatus%' OR name LIKE '%ReturnCode%'
ORDER BY modify_date DESC

/* ---------- Q11. Triggers on the tables the return depends on ---------- */
SELECT 'Q11 Triggers' AS Src, OBJECT_NAME(parent_id) AS TableName, name, is_disabled, create_date, modify_date,
       OBJECT_DEFINITION(object_id) AS Definition
FROM sys.triggers
WHERE OBJECT_NAME(parent_id) IN ('t_TrxClearing','t_AccountTrxClearing','t_IncomingTransactions',
                                 't_IncomingClrTrxExtraDetails','t_Transaction')

/* ---------- Q12. Structure + indexes of the extra-details table (is TrxID unique?) ---------- */
EXEC sp_help 't_IncomingClrTrxExtraDetails'

/* =====================================================================================
   FILES TO SEND (not SQL) - from the clearing application server
   F1. The return file(s) we sent: <OutgoingFiles>\Files\DRUNP*.Q and the signed copy in
       <OutgoingFiles>\SignedFiles or <Archive>, dated 21-Nov-2026 (keep file timestamps,
       e.g. send a screenshot of the folder in "Details" view too).
   F2. BOT's response to that file: the *.V / *.R / *.C / *.RC file(s) in IncomingFiles or
       ARCHIVEs\20261121\VALIDATIONS that mention our DRUNP message ID.
       --> This holds BOT's exact reject reason code. It is the single most important item.
   F3. The original inward file from BARCTZTZ that carried cheque 000211 (the file whose
       MsgId = OrgnlMsgId in Q2).
   F4. Application error log: <ClearingErrorLogFilePath>\ClearingErrorLog\20261121.txt
   F5. Times (approx.) when the operator (a) unpaid the cheque, (b) generated the unpaid
       file, (c) uploaded it to BOT; and the BOT return-window cut-off time that day.
   ===================================================================================== */
