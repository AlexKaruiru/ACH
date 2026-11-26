SET XACT_ABORT OFF
SET NOCOUNT ON
BEGIN TRAN

BEGIN TRY
 EXEC p_AddInwardsTZ @FileName = 'MBSD1026_TEST.xml', @Data = '', @ReturnCode = '00', @VoucherCode = '10', @Amount = 1, @BankID = 'BARCTZTZ', @BranchID = '01'
  , @OurBranchID = '000', @AccountID = '000000000000', @ColumnID = 0, @TrxID = 'doc-MBSD1026-TEST_1', @ExtraDetails = '', @MsgID = 'MBSD1026TEST1', @TrxType = 'ID'
END TRY
BEGIN CATCH
 SELECT 'TEST1 ERROR' AS Result, ERROR_MESSAGE() AS Msg, ERROR_PROCEDURE() AS Proc_, ERROR_LINE() AS Line_
END CATCH

BEGIN TRY
 EXEC p_AddInwardsTZ @FileName = 'MBSD1026_TEST.xml', @Data = '', @ReturnCode = '00', @VoucherCode = '10', @Amount = 1, @BankID = 'BARCTZTZ', @BranchID = '01'
  , @OurBranchID = '000', @AccountID = '000000000000', @ColumnID = 0, @TrxID = '99999999_1', @ExtraDetails = '', @MsgID = 'MBSD1026TEST2', @TrxType = 'ID'
END TRY
BEGIN CATCH
 SELECT 'TEST2 ERROR' AS Result, ERROR_MESSAGE() AS Msg, ERROR_PROCEDURE() AS Proc_, ERROR_LINE() AS Line_
END CATCH

SELECT 'inside tran' AS Stage, 't_IncomingTransactions' AS Tbl, MsgID, TrxID,
       CASE MsgID WHEN 'MBSD1026TEST1' THEN 'expect doc-MBSD1026-TEST_1 (kept)' ELSE 'expect 999999991 (stripped, numeric)' END AS Expected
FROM t_IncomingTransactions WHERE MsgID LIKE 'MBSD1026TEST%'
UNION ALL
SELECT 'inside tran', 't_IncomingClrTrxExtraDetails', OrgnlMsgId, TrxID, ''
FROM t_IncomingClrTrxExtraDetails WHERE OrgnlMsgId LIKE 'MBSD1026TEST%'

IF @@TRANCOUNT > 0 ROLLBACK

SELECT @@TRANCOUNT AS OpenTran_must_be_0,
       (SELECT COUNT(*) FROM t_IncomingTransactions WHERE MsgID LIKE 'MBSD1026TEST%') AS LeftOver_must_be_0,
       (SELECT COUNT(*) FROM t_IncomingClrTrxExtraDetails WHERE OrgnlMsgId LIKE 'MBSD1026TEST%') AS LeftOverExtra_must_be_0
