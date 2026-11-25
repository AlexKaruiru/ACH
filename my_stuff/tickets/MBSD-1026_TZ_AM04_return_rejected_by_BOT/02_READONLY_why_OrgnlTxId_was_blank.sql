/* =====================================================================================
   MBSD-1026 - Why was OrgnlTxId blank in the unpaid file for cheque 211?
   BOT reason given by the bank: "missing OrgnTxID".

   How the file gets OrgnlTxId:
     OC row (t_AccountTrxClearing / t_TrxClearing, via v_Clearing).ColumnID
        -> t_IncomingTransactions.ColumnID  -> t_IncomingTransactions.TrxID  = OrgnlTxId
   If any link breaks, TrxID is blank. The empty tag is then stripped from the XML
   and BOT rejects the whole file.

   From the bank's screenshot (21-Nov-2026, A/C 024244426061):
     ID  row: AccountTrxRowID 91255, TrxRowID 80031937, ReturnCodeID 00
     OC  row: AccountTrxRowID 91256, TrxRowID 80031940, ReturnCodeID AM04, OrigRefCode 'Cheque Unpay : 211'

   Everything here is READ-ONLY, except block D, which runs inside a transaction that is
   always rolled back. Please send back every result set.
   ===================================================================================== */

/* ---------- A. The two clearing rows WITH the link columns (not shown in the screenshot) ---------- */
SELECT 'A1 AccountTrxClearing' AS Src, AccountTrxRowID, TrxRowID, TrxType, ReturnCodeID, ChequeID, Amount,
       ColumnID, IsGenerated, IsDeleted, Reference, SessNo, TrxBatchID, BankID, [Date], OrigRefCode
FROM t_AccountTrxClearing (NOLOCK)
WHERE AccountTrxRowID IN (91255, 91256) OR TrxRowID IN (80031937, 80031940)

SELECT 'A2 TrxClearing' AS Src, TrxRowID, TrxType, ReturnCodeID, ChequeID, Amount,
       ColumnID, IsGenerated, IsDeleted, Reference, SessNo, TrxBatchID, BankID, [Date]
FROM t_TrxClearing (NOLOCK)
WHERE TrxRowID IN (80031937, 80031940) OR (ChequeID = 211 AND ABS(Amount) = 5323100 AND [Date] >= '2026-11-19')

SELECT 'A3 v_Clearing (what the file generator reads)' AS Src, TrxRowID, TrxType, ReturnCodeID, VoucherCode,
       ColumnID, IsGenerated, IsDeleted, Reference
FROM v_Clearing (NOLOCK)
WHERE TrxRowID IN (80031937, 80031940)

/* ---------- B. The inward record the OC row should point to ---------- */
SELECT 'B1 Inward by cheque/amount' AS Src, ColumnID, TrxID, MsgID, FileName, ChequeID, Amount, AccountID,
       ReturnCode, ReturnedTempID, Paid, Returned, isProcessed, Verified, [Date]
FROM t_IncomingTransactions (NOLOCK)
WHERE ChequeID = 211 AND ABS(Amount) = 5323100

SELECT 'B2 Inward by OC.ColumnID' AS Src, i.ColumnID, i.TrxID, i.MsgID, i.FileName, i.ChequeID, i.Amount, i.[Date]
FROM t_IncomingTransactions i (NOLOCK)
WHERE i.ColumnID IN (SELECT ColumnID FROM v_Clearing (NOLOCK) WHERE TrxRowID = 80031940)

SELECT 'B3 Inward by ACH TxId' AS Src, ColumnID, TrxID, MsgID, FileName, ChequeID, Amount, [Date]
FROM t_IncomingTransactions (NOLOCK)
WHERE TrxID = 'doc-001-495-004342_1'

/* Is there a history/archive table the inward may have been moved to? */
SELECT 'B4 Inward-like tables' AS Src, name, create_date, modify_date
FROM sys.tables WHERE name LIKE '%Incoming%'

/* ---------- C. The original-transaction details ---------- */
SELECT 'C1 ExtraDetails' AS Src, TrxID, TrxDate, OrgnlMsgId, OrgnlInstrId, OrgnlEndToEnd, IntrBkSttlmAmt, SourceBIC
FROM t_IncomingClrTrxExtraDetails (NOLOCK)
WHERE TrxID = 'doc-001-495-004342_1' OR OrgnlEndToEnd = 'AB019MK1795167518917'

/* ---------- D. Re-run the file generator for this cheque and ROLL BACK ----------
   This produces exactly what the tool received, including TrxID (= OrgnlTxId),
   OrgnlMsgId and OrgnlEndToEnd. The procedure does small housekeeping updates,
   so it is wrapped in a transaction that is always rolled back.                 */
DECLARE @wd DATETIME = '2026-11-21', @bank VARCHAR(5)
SELECT TOP 1 @bank = BankID FROM t_SystemBankSetting   -- or the BankID value from the app.config key "BankID"
BEGIN TRAN
    EXEC Proc_CreateXMLFiles_Tanzania_Selected_CTS
         @OurBankID = @bank, @OurBranchID = '000',          -- app.config key "OurBranchID"
         @ReadDate = @wd, @FromDate = @wd, @EJDate = @wd,
         @ClearingCenters = '67', @AllCenters = 0, @Currency = 0, @Session = 2,
         @xmlList = '<ArrayOfLsItems><lsItems><TrxRowID>80031940</TrxRowID></lsItems></ArrayOfLsItems>'
ROLLBACK TRAN
/* Columns of interest in the first result set: TrxID (= OrgnlTxId), OrgnlMsgId,
   OrgnlEndToEnd, OriginalColumnID, ColumnID, ReturnCode.                         */

/* ---------- E. The other November incident the bank mentioned ---------- */
/* Please share its cheque number, amount, date and account so we can run A-D for it as well. */
