  
CREATE OR ALTER PROCEDURE [dbo].[p_AddInwardsTZ] (  
 @FileName VARCHAR(100)  
 ,@Data VARCHAR(255)  
 ,@ReturnCode VARCHAR(4)  
 ,@VoucherCode VARCHAR(4)  
 ,@Amount NUMERIC(18, 2)  
 ,@BankID VARCHAR(20)  
 ,@BranchID VARCHAR(4)  
 ,@OurBankID VARCHAR(20) = ''  
 ,@OurBranchID VARCHAR(5)  
 ,@ChequeID VARCHAR(11) = ''  
 ,@ChequeDigit VARCHAR(2) = ''  
 ,@Date DATETIME = ''  
 ,@AccountID VARCHAR(30)  
 ,@ImageUniqueID VARCHAR(20) = ''  
 ,@TFImageSize VARCHAR(15) = ''  
 ,@JFImageSize VARCHAR(15) = ''  
 ,@JRImageSize VARCHAR(15) = ''  
 ,@FrontTFImage TEXT = NULL  
 ,@FrontImage TEXT = NULL  
 ,@BackImage TEXT = NULL  
 ,@UVImage TEXT = NULL  
 ,@TFImageSignature VARCHAR(50) = ''  
 ,@JFImageSignature VARCHAR(50) = ''  
 ,@JRImageSignature VARCHAR(50) = ''  
 ,@Validity BIT = 0  
 ,@TFdpi VARCHAR(5) = ''  
 ,@JRdpi VARCHAR(5) = ''  
 ,@JFdpi VARCHAR(5) = ''  
 ,@TheirAccountID VARCHAR(20) = ''  
 ,@Drawer VARCHAR(250) = ''  
 ,@MsgID VARCHAR(250) = ''  
 ,@EFTID VARCHAR(50) = ''  
 ,@ColumnID NUMERIC(18, 0)  
 ,@isMdv BIT = 0  
 ,@TrxType VARCHAR(4) = ''  
 ,@TrxID VARCHAR(200)  
 ,@ExtraDetails VARCHAR(200)  
 ,@DAdrLine VARCHAR(100) = ''  
 ,@DTwnNm VARCHAR(100) = ''  
 ,@DCtry VARCHAR(100) = ''  
 ,@DNm VARCHAR(100) = ''  
 ,@DPhneNb VARCHAR(100) = ''  
 ,@DMobNb VARCHAR(100) = ''  
 ,@DEmailAdr VARCHAR(100) = ''  
 ,@DOthr VARCHAR(100) = ''  
 ,@DbtrAcct VARCHAR(100) = ''  
 ,@CAdrLine VARCHAR(100) = ''  
 ,@CTwnNm VARCHAR(100) = ''  
 ,@CCtry VARCHAR(100) = ''  
 ,@CNm VARCHAR(100) = ''  
 ,@CPhneNb VARCHAR(100) = ''  
 ,@CMobNb VARCHAR(100) = ''  
 ,@CEmailAdr VARCHAR(100) = ''  
 ,@COthr VARCHAR(100) = ''  
 ,@PymType VARCHAR(100) = ''  
 ,@CdtrAcct VARCHAR(100) = ''  
 ,@OrgnlInstrID VARCHAR(100) = ''  
 ,@UstrdColD VARCHAR(100) = ''  
 ,@UstrdBWF VARCHAR(100) = ''  
 ,@UstrdBWR VARCHAR(100) = ''  
 ,@UstrdGS VARCHAR(100) = ''  
 ,@UstrdUV VARCHAR(100) = ''  
 ,@UstrdMicr VARCHAR(100) = ''  
 ,@MndtId VARCHAR(100) = ''  
 ,@DtOfSgntr VARCHAR(100) = ''  
 ,@ReqdColltnDt VARCHAR(100) = ''  
 ,@Frqcy VARCHAR(100) = ''  
 ,@OrgnlEndToEnd VARCHAR(100) = ''  
 ,@DCNm VARCHAR(100) = ''  
 ,@CCNm VARCHAR(100) = ''  
 ,@FnlColltnDt VARCHAR(100) = ''  
 ,@IntrBkSttlmDt VARCHAR(100) = ''  
 ,@SvcLvl VARCHAR(100) = ''  
 ,@OrgnlTxId VARCHAR(100) = ''  
 ,@CurrencyID VARCHAR(5) = ''  
 ,@SourceBIC VARCHAR(20) = ''  
 ,@RemittanceInfo VARCHAR(20) = ''  
 ,@LclInstrm VARCHAR(20) = ''  
 ,@CtgyPurp VARCHAR(20) = ''  
 ,@OrgnlIntrBkSttlmDt VARCHAR(100) = ''  
 )  
AS  
--RETURN --bnyaswa 022026.  
SET NOCOUNT ON  
SET DATEFORMAT dmy;  
  
DECLARE @ImageID BIGINT  
 ,@OurRealHQBrnID BranchID  
 ,@ImportBatchID VARCHAR(100)  
 ,@Auto_unpay BIT = 0  
 ,@LocalCurrencyID Varchar(10)  
  
SELECT @AccountID = RIGHT(@AccountID, 12)  
  
--SET @TrxID = CAST(REPLACE(@TrxID, '_', '') AS VARCHAR(200)); --bnyaswa 15062026  
IF TRY_CAST(REPLACE(@TrxID, '_', '') AS BIGINT) IS NOT NULL SET @TrxID = CAST(REPLACE(@TrxID, '_', '') AS VARCHAR(200)); --alex.ndegwa 26112026 strip _ only on numeric ids, keep BOT TxId as sent  
  
IF @ReturnCode <> '00'  
BEGIN  
 SELECT @SourceBIC = @BankID  
END  
ELSE  
BEGIN  
 SELECT @SourceBIC = @BankID  
END  
  
IF iSnull(@OrgnlTxId, '') = ''  
BEGIN  
 SELECT @OrgnlTxId = @OrgnlEndToEnd  
END  
  
DECLARE @SBIC VARCHAR(20)  
  
SELECT @SBIC = @SourceBIC  
  
SELECT @SourceBIC = SwiftCode  
FROM t_Bank  
WHERE SwiftCode LIKE '%' + LEFT(@SourceBIC, 6) + '%'  
  
SELECT @BankID = dbo.f_GetClearingBankIDFromSwiftID(@BankID)  
  
SELECT @SourceBIC = isNull(@sourceBIC, @SBIC)  
  
------------------------------------------------------------------------------------------------------------------  
IF ISNULL(@OurBranchID, '') = ''  
 SELECT @OurBranchID = OurBranchID  
 FROM t_SystemBranchSetting  
 WHERE IsHeadOffice = 1  
  
SELECT @Date = dbo.f_GetWorkingDate(@OurBranchID)  
  
SELECT @OurRealHQBrnID = OurBranchID  
FROM t_SystemBranchSetting  
WHERE IsHeadOffice = 1  
  
DECLARE @sub VARCHAR(20)  
  
SELECT @OurBranchID = isNull(RIGHT('000' + RTRIM(@OurBranchID), 3), @OurRealHQBrnID)  
  
IF NOT EXISTS (  
  SELECT 1  
  FROM t_SystemBranchSetting  
  WHERE OurBranchID = @OurBranchID  
  )  
BEGIN  
 SELECT @OurBranchID = @OurRealHQBrnID  
END  
  
SELECT @OurBranchID = isNull(dbo.f_GetNewAccBranchID(@OurBranchID, @AccountID), @OurBranchID)  
  
SELECT @AccountID = dbo.f_GetNewCustAccountID(@OurBranchID, @AccountID)  
  
SELECT @OurBranchID = isNull(dbo.f_GetNewAccBranchID(@OurBranchID, @AccountID), @OurBranchID)  
  
SELECT @CurrencyID = isNull(dbo.f_GetAccountCurrencyID(@OurBranchID, @AccountID), @CurrencyID)  
  
SELECT @LocalCurrencyID = dbo.f_GetLocalCurrencyID(@OurBranchID)   
  
IF EXISTS (  
  SELECT 1  
  FROM t_IncomingClrTrxExtraDetails  
  WHERE OrgnlMsgID = @MsgID  
   AND TrxID = @TrxID  
  )  
BEGIN  
 RETURN  
END  
  
IF NOT EXISTS (  
  SELECT 1  
  FROM t_IncomingTransactions  
  WHERE FileName = @FileName  
   AND MsgID = @MsgID  
   AND TrxID = @TrxID  
  )  
BEGIN  
 IF isNull(@ImageID, '') = ''  
 BEGIN  
  EXEC p_GetUniqueClearingImageID @ImageID OUTPUT  
 END  
  
 EXEC p_GetNextTrxBatchID @BranchID = @OurBranchID  
  ,@NextTrxBatchID = @ImportBatchID OUTPUT  
  
 INSERT INTO t_IncomingTransactions (  
  DATE  
  ,FileName  
  ,Data  
  ,ReturnCode  
  ,VoucherCode  
  ,Amount  
  ,BankID  
  ,BranchID  
  ,AccountID  
  ,OurBankID  
  ,OurBranchID  
  ,ChequeID  
  ,ChequeDigit  
  ,validity  
  ,TheirAccount  
  ,DrawerOrPayee  
  ,MsgID  
  ,FrontBWImage  
  ,FrontGrayScaleImage  
  ,RearImage  
  ,UVImage  
  ,TrxBranchID  
  ,TrxType  
  ,TrxID  
  ,ImageID  
  ,TrxBatchID  
  ,Createdon  
  ,ImportBatchID  
  ,MicrLine  
  ,DRN  
  ,ExtraDetails  
  ,CurrencyID  
  )  
 VALUES (  
  --SELECT  
  @Date  
  ,@FileName  
  ,@Data  
  ,RTRIM(LTRIM(@ReturnCode))  
  ,LEFT(@VoucherCode, 2)  
  ,@Amount  
  ,@BankID  
  ,RIGHT(@BranchID, 2)  
  ,@AccountID  
  ,@OurBankID  
  ,@OurBranchID  
  ,@ChequeID  
  ,@ChequeDigit  
  ,@validity  
  ,@TheirAccountID  
  ,@Drawer  
  ,@MsgID  
  ,@FrontTFImage  
  ,@FrontImage  
  ,@BackImage  
  ,@UVImage  
  ,@OurBranchID  
  ,RTRIM(LTRIM(@TrxType))  
  ,@TrxID  
  ,@ImageID  
  ,NULL  
  ,CONVERT(DATE, getdate())  
  ,@ImportBatchID  
  ,@UstrdMicr  
  ,@OrgnlTxId  
  ,@ExtraDetails  
  ,@CurrencyID  
  )  
  
 UPDATE t_IncomingTransactions  
 SET ModuleID = CASE   
   WHEN TrxType = 'ID'  
    THEN '3050'  
   ELSE '3040'  
   END  
 WHERE isNull(ModuleID, 0) = 0  
  AND TrxType IN (  
   'ID'  
   ,'IC'  
   )  
  
 DELETE  
 FROM t_systemrecordlocks  
 WHERE LockModuleID = '1300'  
  AND ModuleID IN (  
   '3050'  
   ,'3040'  
   ,'3060'  
   ,'3070'  
   )  
  
 UPDATE t_IncomingTransactions  
 SET t_IncomingTransactions.TrxDescriptionID = CASE   
   WHEN TrxType = 'ID'  
    THEN '004'  
   ELSE '003'  
   END  
 WHERE TrxType IN (  
   'ID'  
   ,'IC'  
   )  
  AND isNull(TrxDescriptionID, '') = ''  
  
 UPDATE t_IncomingTransactions  
 SET t_IncomingTransactions.OurBankID = (  
   SELECT BankID  
   FROM t_SystemBankSetting  
   )  
 WHERE isNull(OurBankID, '') = ''  
  
 UPDATE t_IncomingTransactions  
 SET lrAccAsImported = @AccountID  
  ,ClrBrnAsImported = @OurBranchID  
 WHERE FileName = @FileName  
  AND MsgID = @MsgID  
  AND TrxID = @TrxID  
  
 UPDATE t_IncomingTransactions  
 SET AccountID = dbo.f_GetCurrencyBranchGLAccountID(@OurRealHQBrnID, @CurrencyID, 'CL_BANKERCQL_AC')  
  ,ProductID = 'GL'  
  ,AccountType = 'G'  
 WHERE FileName = @FileName  
  AND MsgID = @MsgID  
  AND TrxID = @TrxID  
  AND AccountID LIKE '%' + dbo.f_GetCurrencyBranchGLAccountID(@OurRealHQBrnID, @CurrencyID, 'CL_BANKERCQL_AC') + '%'  
  
 IF @ReturnCode <> '00'  
 BEGIN  
  IF @TrxType = 'ID'  
  BEGIN  
   BEGIN TRY  
    IF EXISTS (  
      SELECT 1  
      FROM t_IncomingTransactions  
       ,t_AccountTrxClearing  
      WHERE FileName = @FileName  
       AND MsgID = @MsgID  
       AND t_IncomingTransactions.TrxID = @TrxID  
       AND t_AccountTrxClearing.TrxType = 'OC'  
       AND t_AccountTrxClearing.TrxRowID = CAST(@TrxID AS BIGINT)  
       AND t_IncomingTransactions.ReturnCode <> '00'  
      )  
    BEGIN  
     --select 'Kamunya 1'  
     --select 'Kamunya 1', @OrgnlTxId, @FileName, @MsgID, @TrxID   
     UPDATE t_IncomingTransactions  
     SET AccountID = t_AccountTrxClearing.AccountID  
      ,BankID = t_AccountTrxClearing.BankID  
      ,BranchID = t_AccountTrxClearing.BranchID  
      ,OurBranchID = t_AccountTrxClearing.OurBranchID  
      ,DrawerOrPayee = t_AccountTrxClearing.DrawerOrPayee  
      ,VoucherCode = t_AccountTrxClearing.VoucherCode  
      ,ImageID = CAST(t_AccountTrxClearing.ImageID AS VARCHAR)  
      ,ChequeID = t_AccountTrxClearing.ChequeID  
     FROM t_AccountTrxClearing  
     WHERE t_IncomingTransactions.FileName = @FileName  
      AND t_IncomingTransactions.MsgID = @MsgID  
      AND t_IncomingTransactions.TrxID = @TrxID  
      AND t_AccountTrxClearing.TrxType = 'OC'  
      AND t_AccountTrxClearing.TrxRowID = CAST(@TrxID AS BIGINT)  
      AND t_IncomingTransactions.ReturnCode <> '00'  
    END  
    ELSE IF EXISTS (  
      SELECT 1  
      FROM t_IncomingTransactions  
       ,t_AccountTrxClearing  
      WHERE FileName = @FileName  
       AND MsgID = @MsgID  
       AND t_IncomingTransactions.TrxID = @TrxID  
       AND t_AccountTrxClearing.TrxType = 'OC'  
       AND t_AccountTrxClearing.TrxRowID = CAST(@OrgnlTxId AS BIGINT)  
       AND t_IncomingTransactions.ReturnCode <> '00'  
      )  
    BEGIN  
     --select 'Kamunya 2',@MsgID,@FileName,@TrxID ,@OrgnlTxId  
     UPDATE t_IncomingTransactions  
     SET AccountID = t_AccountTrxClearing.AccountID  
      ,BankID = t_AccountTrxClearing.BankID  
      ,BranchID = t_AccountTrxClearing.BranchID  
      ,OurBranchID = t_AccountTrxClearing.OurBranchID  
      ,DrawerOrPayee = t_AccountTrxClearing.DrawerOrPayee  
      ,VoucherCode = t_AccountTrxClearing.VoucherCode  
      ,ImageID = CAST(t_AccountTrxClearing.ImageID AS VARCHAR)  
      ,ChequeID = t_AccountTrxClearing.ChequeID  
     FROM t_AccountTrxClearing  
     WHERE FileName = @FileName  
      AND MsgID = @MsgID  
      AND t_IncomingTransactions.TrxID = @TrxID  
      AND t_AccountTrxClearing.TrxType = 'OC'  
      AND t_AccountTrxClearing.TrxRowID = CAST(@OrgnlTxId AS BIGINT)  
      AND t_IncomingTransactions.ReturnCode <> '00'  
    END  
    ELSE IF EXISTS (  
      SELECT 1  
      FROM t_IncomingTransactions  
       ,t_TrxClearing  
      WHERE FileName = @FileName  
       AND MsgID = @MsgID  
       AND t_IncomingTransactions.TrxID = @TrxID  
       AND t_TrxClearing.TrxType = 'OC'  
       AND t_TrxClearing.TrxRowID = CAST(@TrxID AS BIGINT)  
       AND t_IncomingTransactions.ReturnCode <> '00'  
      )  
    BEGIN  
     --select 'Kamunya 3'  
     --select 'Kamunya 1', @OrgnlTxId, @FileName, @MsgID, @TrxID   
     UPDATE t_IncomingTransactions  
     SET AccountID = t_TrxClearing.AccountID  
      ,BankID = t_TrxClearing.BankID  
      ,BranchID = t_TrxClearing.BranchID  
      ,OurBranchID = t_TrxClearing.OurBranchID  
      ,DrawerOrPayee = t_TrxClearing.DrawerOrPayee  
      ,VoucherCode = t_TrxClearing.VoucherCode  
      ,ImageID = CAST(t_TrxClearing.ImageID AS VARCHAR)  
      ,ChequeID = t_TrxClearing.ChequeID  
     FROM t_TrxClearing  
     WHERE t_IncomingTransactions.FileName = @FileName  
      AND t_IncomingTransactions.MsgID = @MsgID  
      AND t_IncomingTransactions.TrxID = @TrxID  
      AND t_TrxClearing.TrxType = 'OC'  
      AND t_TrxClearing.TrxRowID = CAST(@TrxID AS BIGINT)  
      AND t_IncomingTransactions.ReturnCode <> '00'  
    END  
    ELSE IF EXISTS (  
      SELECT 1  
      FROM t_IncomingTransactions  
       ,t_TrxClearing  
      WHERE FileName = @FileName  
       AND MsgID = @MsgID  
       AND t_IncomingTransactions.TrxID = @TrxID  
       AND t_TrxClearing.TrxType = 'OC'  
       AND t_TrxClearing.TrxRowID = CAST(@OrgnlTxId AS BIGINT)  
       AND t_IncomingTransactions.ReturnCode <> '00'  
      )  
    BEGIN  
     --select 'Kamunya 4'  
     --select 'Kamunya 2'  
     UPDATE t_IncomingTransactions  
     SET AccountID = t_TrxClearing.AccountID  
      ,BankID = t_TrxClearing.BankID  
      ,BranchID = t_TrxClearing.BranchID  
      ,OurBranchID = t_TrxClearing.OurBranchID  
      ,DrawerOrPayee = t_TrxClearing.DrawerOrPayee  
      ,VoucherCode = t_TrxClearing.VoucherCode  
      ,ImageID = CAST(t_TrxClearing.ImageID AS VARCHAR)  
      ,ChequeID = t_TrxClearing.ChequeID  
     FROM t_TrxClearing  
     WHERE FileName = @FileName  
      AND MsgID = @MsgID  
      AND t_IncomingTransactions.TrxID = @TrxID  
      AND t_TrxClearing.TrxType = 'OC'  
      AND t_TrxClearing.TrxRowID = CAST(@OrgnlTxId AS BIGINT)  
      AND t_IncomingTransactions.ReturnCode <> '00'  
    END  
  
    UPDATE t_IncomingTransactions  
    SET FRONTBWIMAGE = t_ChequeImages.TFImage  
     ,FRONTGRAYSCALEIMAGE = t_ChequeImages.JFImage  
     ,REARIMAGE = t_ChequeImages.JRImage  
     ,UVIMAGE = t_ChequeImages.UVImage  
    FROM dbo.t_ChequeImages  
    WHERE FileName = @FileName  
     AND MsgID = @MsgID  
     AND TrxID = @TrxID  
     AND t_ChequeImages.ImageID = CAST(t_IncomingTransactions.ImageID AS BIGINT)  
     AND t_IncomingTransactions.ReturnCode <> '00'  
     AND t_IncomingTransactions.TrxType = 'ID'  
     AND t_IncomingTransactions.VoucherCode <> '40'  
   END TRY  
  
   BEGIN CATCH  
    IF EXISTS (  
      SELECT 1  
      FROM t_IncomingTransactions  
       ,t_AccountTrxClearing  
      WHERE FileName = @FileName  
       AND MsgID = @MsgID  
       AND t_IncomingTransactions.TrxID = @TrxID  
       AND t_AccountTrxClearing.TrxType = 'OC'  
       AND t_AccountTrxClearing.TrxRowID = CAST(@OrgnlTxId AS BIGINT)  
       AND t_IncomingTransactions.ReturnCode <> '00'  
      )  
    BEGIN  
     --select 'Kamunya 5'  
     --select 'Kamunya 2'  
     UPDATE t_IncomingTransactions  
     SET AccountID = t_AccountTrxClearing.AccountID  
      ,BankID = t_AccountTrxClearing.BankID  
      ,BranchID = t_AccountTrxClearing.BranchID  
      ,OurBranchID = t_AccountTrxClearing.OurBranchID  
      ,DrawerOrPayee = t_AccountTrxClearing.DrawerOrPayee  
      ,VoucherCode = t_AccountTrxClearing.VoucherCode  
      ,ImageID = CAST(t_AccountTrxClearing.ImageID AS VARCHAR)  
      ,ChequeID = t_AccountTrxClearing.ChequeID  
     FROM t_AccountTrxClearing  
     WHERE FileName = @FileName  
      AND MsgID = @MsgID  
      AND t_IncomingTransactions.TrxID = @TrxID  
      AND t_AccountTrxClearing.TrxType = 'OC'  
      AND t_AccountTrxClearing.TrxRowID = CAST(@OrgnlTxId AS BIGINT)  
      AND t_IncomingTransactions.ReturnCode <> '00'  
    END  
    ELSE IF EXISTS (  
      SELECT 1  
      FROM t_IncomingTransactions  
       ,t_AccountTrxClearing  
      WHERE FileName = @FileName  
       AND MsgID = @MsgID  
       AND t_IncomingTransactions.TrxID = @TrxID  
       AND t_AccountTrxClearing.TrxType = 'OC'  
       AND t_AccountTrxClearing.TrxRowID = CAST(@TrxID AS BIGINT)  
       AND t_IncomingTransactions.ReturnCode <> '00'  
      )  
    BEGIN  
     --select 'Kamunya 6'  
     --select 'Kamunya 1', @OrgnlTxId, @FileName, @MsgID, @TrxID   
     UPDATE t_IncomingTransactions  
     SET AccountID = t_AccountTrxClearing.AccountID  
      ,BankID = t_AccountTrxClearing.BankID  
      ,BranchID = t_AccountTrxClearing.BranchID  
      ,OurBranchID = t_AccountTrxClearing.OurBranchID  
      ,DrawerOrPayee = t_AccountTrxClearing.DrawerOrPayee  
      ,VoucherCode = t_AccountTrxClearing.VoucherCode  
      ,ImageID = CAST(t_AccountTrxClearing.ImageID AS VARCHAR)  
      ,ChequeID = t_AccountTrxClearing.ChequeID  
     FROM t_AccountTrxClearing  
     WHERE t_IncomingTransactions.FileName = @FileName  
      AND t_IncomingTransactions.MsgID = @MsgID  
      AND t_IncomingTransactions.TrxID = @TrxID  
      AND t_AccountTrxClearing.TrxType = 'OC'  
      AND t_AccountTrxClearing.TrxRowID = CAST(@TrxID AS BIGINT)  
      AND t_IncomingTransactions.ReturnCode <> '00'  
    END  
    ELSE IF EXISTS (  
      SELECT 1  
      FROM t_IncomingTransactions  
       ,t_TrxClearing  
      WHERE FileName = @FileName  
       AND MsgID = @MsgID  
       AND t_IncomingTransactions.TrxID = @TrxID  
       AND t_TrxClearing.TrxType = 'OC'  
       AND t_TrxClearing.TrxRowID = CAST(@OrgnlTxId AS BIGINT)  
       AND t_IncomingTransactions.ReturnCode <> '00'  
      )  
    BEGIN  
     --select 'Kamunya 7'  
     --select 'Kamunya 2'  
     UPDATE t_IncomingTransactions  
     SET AccountID = t_TrxClearing.AccountID  
      ,BankID = t_TrxClearing.BankID  
      ,BranchID = t_TrxClearing.BranchID  
      ,OurBranchID = t_TrxClearing.OurBranchID  
      ,DrawerOrPayee = t_TrxClearing.DrawerOrPayee  
      ,VoucherCode = t_TrxClearing.VoucherCode  
      ,ImageID = CAST(t_TrxClearing.ImageID AS VARCHAR)  
      ,ChequeID = t_TrxClearing.ChequeID  
     FROM t_TrxClearing  
     WHERE FileName = @FileName  
      AND MsgID = @MsgID  
      AND t_IncomingTransactions.TrxID = @TrxID  
      AND t_TrxClearing.TrxType = 'OC'  
      AND t_TrxClearing.TrxRowID = CAST(@OrgnlTxId AS BIGINT)  
      AND t_IncomingTransactions.ReturnCode <> '00'  
    END  
    ELSE IF EXISTS (  
      SELECT 1  
      FROM t_IncomingTransactions  
       ,t_TrxClearing  
      WHERE FileName = @FileName  
       AND MsgID = @MsgID  
       AND t_IncomingTransactions.TrxID = @TrxID  
       AND t_TrxClearing.TrxType = 'OC'  
       AND t_TrxClearing.TrxRowID = CAST(@TrxID AS BIGINT)  
       AND t_IncomingTransactions.ReturnCode <> '00'  
      )  
    BEGIN  
     --select 'Kamunya 8'  
     --select 'Kamunya 1', @OrgnlTxId, @FileName, @MsgID, @TrxID   
     UPDATE t_IncomingTransactions  
     SET AccountID = t_TrxClearing.AccountID  
      ,BankID = t_TrxClearing.BankID  
      ,BranchID = t_TrxClearing.BranchID  
      ,OurBranchID = t_TrxClearing.OurBranchID  
      ,DrawerOrPayee = t_TrxClearing.DrawerOrPayee  
      ,VoucherCode = t_TrxClearing.VoucherCode  
      ,ImageID = CAST(t_TrxClearing.ImageID AS VARCHAR)  
      ,ChequeID = t_TrxClearing.ChequeID  
     FROM t_TrxClearing  
     WHERE t_IncomingTransactions.FileName = @FileName  
      AND t_IncomingTransactions.MsgID = @MsgID  
      AND t_IncomingTransactions.TrxID = @TrxID  
      AND t_TrxClearing.TrxType = 'OC'  
      AND t_TrxClearing.TrxRowID = CAST(@TrxID AS BIGINT)  
      AND t_IncomingTransactions.ReturnCode <> '00'  
    END  
  
    UPDATE t_IncomingTransactions  
    SET FRONTBWIMAGE = t_ChequeImages.TFImage  
     ,FRONTGRAYSCALEIMAGE = t_ChequeImages.JFImage  
     ,REARIMAGE = t_ChequeImages.JRImage  
     ,UVIMAGE = t_ChequeImages.UVImage  
    FROM dbo.t_ChequeImages  
    WHERE FileName = @FileName  
     AND MsgID = @MsgID  
     AND TrxID = @TrxID  
     AND t_ChequeImages.ImageID = CAST(t_IncomingTransactions.ImageID AS BIGINT)  
     AND t_IncomingTransactions.ReturnCode <> '00'  
     AND t_IncomingTransactions.TrxType = 'ID'  
     AND t_IncomingTransactions.VoucherCode <> '40'  
   END CATCH  
  END  
  ELSE  
  BEGIN  
   BEGIN TRY  
    IF EXISTS (  
      SELECT 1  
      FROM t_IncomingTransactions  
       ,t_AccountTrxClearing  
      WHERE FileName = @FileName  
       AND MsgID = @MsgID --AND TrxID = @TrxID   
       AND t_AccountTrxClearing.TrxType = 'OD'  
       AND t_AccountTrxClearing.TrxRowID = CAST(@TrxID AS BIGINT)  
       AND t_IncomingTransactions.ReturnCode <> '00'  
      )  
    BEGIN  
     --select 'Kamunya 9'  
     UPDATE t_IncomingTransactions  
     SET AccountID = t_AccountTrxClearing.AccountID  
      ,BankID = t_AccountTrxClearing.BankID  
      ,BranchID = t_AccountTrxClearing.BranchID  
      ,OurBranchID = t_AccountTrxClearing.OurBranchID  
      ,DrawerOrPayee = t_AccountTrxClearing.DrawerOrPayee  
      ,VoucherCode = t_AccountTrxClearing.VoucherCode  
      ,ImageID = CAST(t_AccountTrxClearing.ImageID AS VARCHAR)  
      ,ChequeID = t_AccountTrxClearing.ChequeID  
     FROM t_AccountTrxClearing  
     WHERE FileName = @FileName  
      AND MsgID = @MsgID --AND TrxID = @TrxID   
      AND t_AccountTrxClearing.TrxType = 'OD'  
      AND t_AccountTrxClearing.TrxRowID = CAST(@TrxID AS BIGINT)  
      AND t_IncomingTransactions.ReturnCode <> '00'  
    END  
    ELSE IF EXISTS (  
      SELECT 1  
      FROM t_IncomingTransactions  
       ,t_AccountTrxClearing  
      WHERE FileName = @FileName  
       AND MsgID = @MsgID --AND TrxID = @TrxID   
       AND t_AccountTrxClearing.TrxType = 'OD'  
       AND t_AccountTrxClearing.TrxRowID = CAST(@OrgnlTxId AS BIGINT)  
       AND t_IncomingTransactions.ReturnCode <> '00'  
      )  
    BEGIN  
     UPDATE t_IncomingTransactions  
     SET AccountID = t_AccountTrxClearing.AccountID  
      ,BankID = t_AccountTrxClearing.BankID  
      ,BranchID = t_AccountTrxClearing.BranchID  
      ,OurBranchID = t_AccountTrxClearing.OurBranchID  
      ,DrawerOrPayee = t_AccountTrxClearing.DrawerOrPayee  
      ,VoucherCode = t_AccountTrxClearing.VoucherCode  
      ,ImageID = CAST(t_AccountTrxClearing.ImageID AS VARCHAR)  
      ,ChequeID = t_AccountTrxClearing.ChequeID  
     FROM t_AccountTrxClearing  
     WHERE t_IncomingTransactions.FileName = @FileName  
      AND t_IncomingTransactions.MsgID = @MsgID  
      --AND t_IncomingTransactions.TrxID = @TrxID   
      AND t_AccountTrxClearing.TrxType = 'OD'  
      AND t_AccountTrxClearing.TrxRowID = CAST(@OrgnlTxId AS BIGINT)  
      AND t_IncomingTransactions.ReturnCode <> '00'  
    END  
    ELSE IF EXISTS (  
      SELECT 1  
      FROM t_IncomingTransactions  
       ,t_TrxClearing  
      WHERE FileName = @FileName  
       AND MsgID = @MsgID  
       AND t_IncomingTransactions.TrxID = @TrxID  
       AND t_TrxClearing.TrxType = 'OD'  
       AND t_TrxClearing.TrxRowID = CAST(@OrgnlTxId AS BIGINT)  
       AND t_IncomingTransactions.ReturnCode <> '00'  
      )  
    BEGIN  
     --select 'Kamunya 11'  
     --select 'Kamunya 2'  
     UPDATE t_IncomingTransactions  
     SET AccountID = t_TrxClearing.AccountID  
      ,BankID = t_TrxClearing.BankID  
      ,BranchID = t_TrxClearing.BranchID  
      ,OurBranchID = t_TrxClearing.OurBranchID  
      ,DrawerOrPayee = t_TrxClearing.DrawerOrPayee  
      ,VoucherCode = t_TrxClearing.VoucherCode  
      ,ImageID = CAST(t_TrxClearing.ImageID AS VARCHAR)  
      ,ChequeID = t_TrxClearing.ChequeID  
     FROM t_TrxClearing  
     WHERE FileName = @FileName  
      AND MsgID = @MsgID  
      AND t_IncomingTransactions.TrxID = @TrxID  
      AND t_TrxClearing.TrxType = 'OD'  
      AND t_TrxClearing.TrxRowID = CAST(@OrgnlTxId AS BIGINT)  
      AND t_IncomingTransactions.ReturnCode <> '00'  
    END  
    ELSE IF EXISTS (  
      SELECT 1  
      FROM t_IncomingTransactions  
       ,t_TrxClearing  
      WHERE FileName = @FileName  
       AND MsgID = @MsgID  
       AND t_IncomingTransactions.TrxID = @TrxID  
       AND t_TrxClearing.TrxType = 'OD'  
       AND t_TrxClearing.TrxRowID = CAST(@TrxID AS BIGINT)  
       AND t_IncomingTransactions.ReturnCode <> '00'  
      )  
    BEGIN  
     --select 'Kamunya 12'  
     --select 'Kamunya 1', @OrgnlTxId, @FileName, @MsgID, @TrxID   
     UPDATE t_IncomingTransactions  
     SET AccountID = t_TrxClearing.AccountID  
      ,BankID = t_TrxClearing.BankID  
      ,BranchID = t_TrxClearing.BranchID  
      ,OurBranchID = t_TrxClearing.OurBranchID  
      ,DrawerOrPayee = t_TrxClearing.DrawerOrPayee  
      ,VoucherCode = t_TrxClearing.VoucherCode  
      ,ImageID = CAST(t_TrxClearing.ImageID AS VARCHAR)  
      ,ChequeID = t_TrxClearing.ChequeID  
     FROM t_TrxClearing  
     WHERE t_IncomingTransactions.FileName = @FileName  
      AND t_IncomingTransactions.MsgID = @MsgID  
      AND t_IncomingTransactions.TrxID = @TrxID  
      AND t_TrxClearing.TrxType = 'OD'  
      AND t_TrxClearing.TrxRowID = CAST(@TrxID AS BIGINT)  
      AND t_IncomingTransactions.ReturnCode <> '00'  
    END  
   END TRY  
  
   BEGIN CATCH  
    IF EXISTS (  
      SELECT 1  
      FROM t_IncomingTransactions  
       ,t_AccountTrxClearing  
      WHERE FileName = @FileName  
       AND MsgID = @MsgID --AND TrxID = @TrxID   
       AND t_AccountTrxClearing.TrxType = 'OD'  
       AND t_AccountTrxClearing.TrxRowID = CAST(@OrgnlTxId AS BIGINT)  
       AND t_IncomingTransactions.ReturnCode <> '00'  
      )  
    BEGIN  
     UPDATE t_IncomingTransactions  
     SET AccountID = t_AccountTrxClearing.AccountID  
      ,BankID = t_AccountTrxClearing.BankID  
      ,BranchID = t_AccountTrxClearing.BranchID  
      ,OurBranchID = t_AccountTrxClearing.OurBranchID  
      ,DrawerOrPayee = t_AccountTrxClearing.DrawerOrPayee  
      ,VoucherCode = t_AccountTrxClearing.VoucherCode  
      ,ImageID = CAST(t_AccountTrxClearing.ImageID AS VARCHAR)  
      ,ChequeID = t_AccountTrxClearing.ChequeID  
     FROM t_AccountTrxClearing  
     WHERE t_IncomingTransactions.FileName = @FileName  
      AND t_IncomingTransactions.MsgID = @MsgID  
      --AND t_IncomingTransactions.TrxID = @TrxID   
      AND t_AccountTrxClearing.TrxType = 'OD'  
      AND t_AccountTrxClearing.TrxRowID = CAST(@OrgnlTxId AS BIGINT)  
      AND t_IncomingTransactions.ReturnCode <> '00'  
    END  
    ELSE IF EXISTS (  
      SELECT 1  
      FROM t_IncomingTransactions  
       ,t_AccountTrxClearing  
      WHERE FileName = @FileName  
       AND MsgID = @MsgID --AND TrxID = @TrxID   
       AND t_AccountTrxClearing.TrxType = 'OD'  
       AND t_AccountTrxClearing.TrxRowID = CAST(@TrxID AS BIGINT)  
       AND t_IncomingTransactions.ReturnCode <> '00'  
      )  
    BEGIN  
     --select 'Kamunya 14'  
     UPDATE t_IncomingTransactions  
     SET AccountID = t_AccountTrxClearing.AccountID  
      ,BankID = t_AccountTrxClearing.BankID  
      ,BranchID = t_AccountTrxClearing.BranchID  
      ,OurBranchID = t_AccountTrxClearing.OurBranchID  
      ,DrawerOrPayee = t_AccountTrxClearing.DrawerOrPayee  
      ,VoucherCode = t_AccountTrxClearing.VoucherCode  
      ,ImageID = CAST(t_AccountTrxClearing.ImageID AS VARCHAR)  
      ,ChequeID = t_AccountTrxClearing.ChequeID  
     FROM t_AccountTrxClearing  
     WHERE FileName = @FileName  
      AND MsgID = @MsgID --AND TrxID = @TrxID   
      AND t_AccountTrxClearing.TrxType = 'OD'  
      AND t_AccountTrxClearing.TrxRowID = CAST(@TrxID AS BIGINT)  
      AND t_IncomingTransactions.ReturnCode <> '00'  
    END  
    ELSE IF EXISTS (  
      SELECT 1  
      FROM t_IncomingTransactions  
       ,t_TrxClearing  
      WHERE FileName = @FileName  
       AND MsgID = @MsgID  
       AND t_IncomingTransactions.TrxID = @TrxID  
       AND t_TrxClearing.TrxType = 'OD'  
       AND t_TrxClearing.TrxRowID = CAST(@OrgnlTxId AS BIGINT)  
       AND t_IncomingTransactions.ReturnCode <> '00'  
      )  
    BEGIN  
     UPDATE t_IncomingTransactions  
     SET AccountID = t_TrxClearing.AccountID  
      ,BankID = t_TrxClearing.BankID  
      ,BranchID = t_TrxClearing.BranchID  
      ,OurBranchID = t_TrxClearing.OurBranchID  
      ,DrawerOrPayee = t_TrxClearing.DrawerOrPayee  
      ,VoucherCode = t_TrxClearing.VoucherCode  
      ,ImageID = CAST(t_TrxClearing.ImageID AS VARCHAR)  
      ,ChequeID = t_TrxClearing.ChequeID  
     FROM t_TrxClearing  
     WHERE FileName = @FileName  
      AND MsgID = @MsgID  
      AND t_IncomingTransactions.TrxID = @TrxID  
      AND t_TrxClearing.TrxType = 'OD'  
      AND t_TrxClearing.TrxRowID = CAST(@OrgnlTxId AS BIGINT)  
      AND t_IncomingTransactions.ReturnCode <> '00'  
    END  
    ELSE IF EXISTS (  
      SELECT 1  
      FROM t_IncomingTransactions  
       ,t_TrxClearing  
      WHERE FileName = @FileName  
       AND MsgID = @MsgID  
       AND t_IncomingTransactions.TrxID = @TrxID  
       AND t_TrxClearing.TrxType = 'OD'  
       AND t_TrxClearing.TrxRowID = CAST(@TrxID AS BIGINT)  
       AND t_IncomingTransactions.ReturnCode <> '00'  
      )  
    BEGIN  
     UPDATE t_IncomingTransactions  
     SET AccountID = t_TrxClearing.AccountID  
      ,BankID = t_TrxClearing.BankID  
      ,BranchID = t_TrxClearing.BranchID  
      ,OurBranchID = t_TrxClearing.OurBranchID  
      ,DrawerOrPayee = t_TrxClearing.DrawerOrPayee  
      ,VoucherCode = t_TrxClearing.VoucherCode  
      ,ImageID = CAST(t_TrxClearing.ImageID AS VARCHAR)  
      ,ChequeID = t_TrxClearing.ChequeID  
     FROM t_TrxClearing  
     WHERE t_IncomingTransactions.FileName = @FileName  
      AND t_IncomingTransactions.MsgID = @MsgID  
      AND t_IncomingTransactions.TrxID = @TrxID  
      AND t_TrxClearing.TrxType = 'OD'  
      AND t_TrxClearing.TrxRowID = CAST(@TrxID AS BIGINT)  
      AND t_IncomingTransactions.ReturnCode <> '00'  
    END  
   END CATCH  
  
   UPDATE t_IncomingTransactions  
   SET AccountType = dbo.f_GetAllAccountType(OurBranchID, AccountID)  
    ,FULLNAME = RIGHT(dbo.f_GetAccountName(OurbranchID, AccountID), 100)  
    ,ProductID = ISNULL(dbo.f_GetAccountProductID(OurBranchID, AccountID), '')  
    ,OurBankID = (  
     SELECT BankID  
     FROM t_SystemBranchSetting  
     WHERE IsHeadOffice = 1  
     )  
    ,VoucherCodeDescription = dbo.f_GetUserCodeName('VoucherID', '58')  
    ,VoucherCode = '58'  
    ,MainGLID = dbo.f_GetGLInterfaceAccountID1(OurBankID, ISNULL(dbo.f_GetAccountProductID(OurBranchID, AccountID), ''), 'CONTROL_AC')  
    ,ContraGLID = dbo.f_GetCurrencyBranchGLAccountID(OurBranchID, CurrencyID, 'CUR_CLR_AC')  
    ,DATE = dbo.f_GetWorkingDate(OurBranchID)  
   WHERE FileName = @FileName  
    AND MsgID = @MsgID  
    AND TrxID = @TrxID  
    AND TrxType = 'IC'  
    AND ReturnCode <> '00'  
    AND t_IncomingTransactions.MsgID = @MsgID  
  
   UPDATE t_IncomingTransactions  
   SET AccountType = dbo.f_GetAllAccountType(OurBranchID, AccountID)  
    ,FULLNAME = RIGHT(dbo.f_GetAccountName(OurbranchID, AccountID), 100)  
    ,ProductID = ISNULL(dbo.f_GetAccountProductID(OurBranchID, AccountID), '')  
    ,OurBankID = (  
     SELECT BankID  
     FROM t_SystemBranchSetting  
     WHERE IsHeadOffice = 1  
     )  
    ,VoucherCodeDescription = dbo.f_GetUserCodeName('VoucherID', VoucherCode)  
    ,[Description] = 'Unpaid ' + TrxType + ' Rsn:  ' + dbo.fn_ReturnCodeDescriptionsIncomingTZ(ReturnCode)  
    ,MainGLID = dbo.f_GetGLInterfaceAccountID1(OurBankID, ISNULL(dbo.f_GetAccountProductID(OurBranchID, AccountID), ''), 'CONTROL_AC')  
    ,ContraGLID = dbo.f_GetCurrencyBranchGLAccountID(OurBranchID, CurrencyID, 'CUR_CLR_AC')  
    ,DATE = dbo.f_GetWorkingDate(OurBranchID)  
   WHERE FileName = @FileName  
    AND MsgID = @MsgID  
    AND TrxID = @TrxID  
    AND TrxType = 'ID'  
    AND ReturnCode <> '00'  
    AND t_IncomingTransactions.MsgID = @MsgID  
  END  
 END  
  
 ------------------END of UNpaying-----------------------------------------------------------------------  
 UPDATE t_IncomingTransactions  
 SET AccountType = dbo.f_GetAllAccountType(OurBranchID, AccountID)  
 WHERE FileName = @FileName  
  AND MsgID = @MsgID  
  AND TrxID = @TrxID  
  AND ReturnCode = '00'  
  
 UPDATE t_IncomingTransactions  
 SET OurBranchID = isNull(dbo.f_GetAccountBranchID(AccountID), OurBranchID)  
 WHERE FileName = @FileName  
  AND MsgID = @MsgID  
  AND TrxID = @TrxID  
  AND ReturnCode = '00'  
  
 UPDATE t_IncomingTransactions  
 SET AccountType = dbo.f_GetAllAccountType(OurBranchID, AccountID)  
  ,FULLNAME = RIGHT(dbo.f_GetAccountName(OurbranchID, AccountID), 100)  
  ,ProductID = ISNULL(dbo.f_GetAccountProductID(OurBranchID, AccountID), '')  
  ,OurBankID = (  
   SELECT BankID  
   FROM t_SystemBranchSetting  
   WHERE IsHeadOffice = 1  
   )  
  ,VoucherCodeDescription = dbo.f_GetUserCodeName('VoucherID', '58')  
  ,VoucherCode = '58'  
  ,MainGLID = dbo.f_GetGLInterfaceAccountID1(OurBankID, ISNULL(dbo.f_GetAccountProductID(OurBranchID, AccountID), ''), 'CONTROL_AC')  
  ,ContraGLID = dbo.f_GetCurrencyBranchGLAccountID(OurBranchID, CurrencyID, 'CUR_CLR_AC')  
  ,DATE = dbo.f_GetWorkingDate(OurBranchID)  
 WHERE FileName = @FileName  
  AND MsgID = @MsgID  
  AND TrxID = @TrxID  
  AND TrxType = 'IC'  
  AND ReturnCode = '00'  
  
 UPDATE t_IncomingTransactions  
 SET AccountType = dbo.f_GetAllAccountType(OurBranchID, AccountID)  
  ,FULLNAME = RIGHT(dbo.f_GetAccountName(OurbranchID, AccountID), 100)  
  ,ProductID = ISNULL(dbo.f_GetAccountProductID(OurBranchID, AccountID), '')  
  ,OurBankID = (  
   SELECT BankID  
   FROM t_SystemBranchSetting  
   WHERE IsHeadOffice = 1  
   )  
  ,VoucherCodeDescription = dbo.f_GetUserCodeName('VoucherID', '58')  
  ,VoucherCode = '58'  
  ,MainGLID = dbo.f_GetGLInterfaceAccountID1(OurBankID, ISNULL(dbo.f_GetAccountProductID(OurBranchID, AccountID), ''), 'CONTROL_AC')  
  ,ContraGLID = dbo.f_GetCurrencyBranchGLAccountID(OurBranchID, CurrencyID, 'CUR_CLR_AC')  
  ,DATE = dbo.f_GetWorkingDate(OurBranchID)  
 WHERE FileName = @FileName  
  AND MsgID = @MsgID  
  AND TrxID = @TrxID  
  AND TrxType = 'IC'  
  AND ReturnCode <> '00'  
  
 UPDATE t_IncomingTransactions  
 SET AccountType = dbo.f_GetAllAccountType(OurBranchID, AccountID)  
  ,FULLNAME = RIGHT(dbo.f_GetAccountName(OurbranchID, AccountID), 100)  
  ,ProductID = ISNULL(dbo.f_GetAccountProductID(OurBranchID, AccountID), '')  
  ,OurBankID = (  
   SELECT BankID  
   FROM t_SystemBranchSetting  
   WHERE IsHeadOffice = 1  
   )  
  ,VoucherCodeDescription = dbo.f_GetUserCodeName('VoucherID', VoucherCode)  
  ,MainGLID = dbo.f_GetGLInterfaceAccountID1(OurBankID, ISNULL(dbo.f_GetAccountProductID(OurBranchID, AccountID), ''), 'CONTROL_AC')  
  ,ContraGLID = dbo.f_GetCurrencyBranchGLAccountID(OurBranchID, CurrencyID, 'CUR_CLR_AC')  
  ,DATE = dbo.f_GetWorkingDate(OurBranchID)  
 WHERE FileName = @FileName  
  AND MsgID = @MsgID  
  AND TrxID = @TrxID  
  AND TrxType = 'ID'  
  AND ReturnCode = '00'  
  
 UPDATE t_IncomingTransactions  
 SET AccountType = dbo.f_GetAllAccountType(OurBranchID, AccountID)  
  ,FULLNAME = RIGHT(dbo.f_GetAccountName(OurbranchID, AccountID), 100)  
  ,ProductID = ISNULL(dbo.f_GetAccountProductID(OurBranchID, AccountID), '')  
  ,OurBankID = (  
   SELECT BankID  
   FROM t_SystemBranchSetting  
   WHERE IsHeadOffice = 1  
   )  
  ,VoucherCodeDescription = dbo.f_GetUserCodeName('VoucherID', VoucherCode)  
  ,[Description] = 'Unpaid ' + TrxType + ' Rsn:  ' + dbo.fn_ReturnCodeDescriptionsIncomingTZ(ReturnCode)  
  ,MainGLID = dbo.f_GetGLInterfaceAccountID1(OurBankID, ISNULL(dbo.f_GetAccountProductID(OurBranchID, AccountID), ''), 'CONTROL_AC')  
  ,ContraGLID = dbo.f_GetCurrencyBranchGLAccountID(OurBranchID, CurrencyID, 'CUR_CLR_AC')  
  ,DATE = dbo.f_GetWorkingDate(OurBranchID)  
 WHERE FileName = @FileName  
  AND MsgID = @MsgID  
  AND TrxID = @TrxID  
  AND TrxType = 'ID'  
  AND ReturnCode <> '00'  
  
 UPDATE t_IncomingTransactions  
 SET BranchID = a.BranchID  
 FROM (  
  SELECT BankID  
   ,BranchID  
  FROM t_Branch  
  ) AS a  
 WHERE t_IncomingTransactions.BankID = a.BankID  
  AND FileName = @FileName  
  AND MsgID = @MsgID  
  AND TrxID = @TrxID  
  AND ReturnCode = '00'  
  
 UPDATE t_IncomingTransactions  
 SET TrxDescriptionID = (  
   CASE   
    WHEN TrxType = 'ID'  
     THEN '004'  
    ELSE '003'  
    END  
   )  
 WHERE isNull(TrxDescriptionID, '') = ''  
  AND FileName = @FileName  
  AND MsgID = @MsgID  
  AND TrxID = @TrxID  
  
 UPDATE t_IncomingTransactions  
 SET ModuleID = (  
   CASE   
    WHEN TrxType = 'ID'  
     THEN '3050'  
    ELSE '3040'  
    END  
   )  
 WHERE isNull(ModuleID, '') = ''  
  AND FileName = @FileName  
  AND MsgID = @MsgID  
  AND TrxID = @TrxID  
  
 UPDATE t_IncomingTransactions  
 SET VoucherCode = '11'  
 WHERE FileName = @FileName  
  AND MsgID = @MsgID  
  AND TrxID = @TrxID  
  AND TrxType = 'ID'  
  
 UPDATE t_IncomingTransactions  
 SET InstrumentTypeID = (  
   CASE   
    WHEN TrxType = 'ID'  
     THEN 'C'  
    ELSE 'V'  
    END  
   )  
 WHERE isNull(InstrumentTypeID, '') = ''  
  AND FileName = @FileName  
  AND MsgID = @MsgID  
  AND TrxID = @TrxID  
  
 UPDATE t_IncomingTransactions  
 SET TrxBranchID = OurBranchID  
 WHERE FileName = @FileName  
  AND MsgID = @MsgID  
  AND TrxID = @TrxID  
  
 UPDATE t_IncomingTransactions  
 SET TrxBranchID = @OurRealHQBrnID  
 WHERE TrxType = 'IC'  
  AND FileName = @FileName  
  AND MsgID = @MsgID  
  AND TrxID = @TrxID  
  
 UPDATE t_IncomingTransactions  
 SET BranchID = t_Branch.BranchID  
 FROM t_Branch  
 WHERE t_IncomingTransactions.BankID = t_Branch.BankID  
  AND FileName = @FileName  
  AND MsgID = @MsgID  
  AND TrxID = @TrxID  
  AND isNull(t_IncomingTransactions.BranchID, '') = ''  
  
 UPDATE t_IncomingTransactions  
 SET DRN = 'T'  
  ,Description = 'IC - To : Acc(' + AccountID + ') - Frm: ' + DrawerOrPayee  
 WHERE TrxType = 'IC'  
  AND ReturnCode = '00'  
  AND FileName = @FileName  
  AND MsgID = @MsgID  
  AND TrxID = @TrxID  
  
 UPDATE t_IncomingTransactions  
 SET DRN = 'T'  
  ,Description = 'UnPaid IC Rsn:  ' + DBO.fn_ReturnCodeDescriptionsIncomingTZ(ReturnCode)  
 WHERE TrxType = 'IC'  
  AND ReturnCode <> '00'  
  AND FileName = @FileName  
  AND MsgID = @MsgID  
  AND TrxID = @TrxID  
  
 UPDATE t_IncomingTransactions  
 SET DRN = 'J'  
  ,Description = 'ID To : Acc(' + AccountID + ') ChqID : ' + CAST(ChequeID AS VARCHAR)  
 WHERE TrxType = 'ID'  
  AND VoucherCode <> '40'  
  AND VoucherCode NOT IN ('60')  
  AND ReturnCode = '00'  
  AND FileName = @FileName  
  AND MsgID = @MsgID  
  AND TrxID = @TrxID  
  
 UPDATE t_IncomingTransactions  
 SET DRN = 'E'  
  ,Description = 'ID To : Acc(' + AccountID + ') ChqID : ' + CAST(ChequeID AS VARCHAR)  
 WHERE TrxType = 'ID'  
  AND VoucherCode <> '40'  
  AND VoucherCode IN ('60')  
  AND ReturnCode = '00'  
  AND FileName = @FileName  
  AND MsgID = @MsgID  
  AND TrxID = @TrxID  
  
 UPDATE t_IncomingTransactions  
 SET DRN = 'T'  
  ,Description = 'DD - To : Acc(' + AccountID + ') - Frm: ' + DrawerOrPayee  
 WHERE TrxType = 'ID'  
  AND VoucherCode = '40'  
  AND ReturnCode = '00'  
  AND FileName = @FileName  
  AND MsgID = @MsgID  
  AND TrxID = @TrxID  
  
 UPDATE t_IncomingTransactions  
 SET DRN = 'J'  
  ,Description = 'UnPaid ID To : Acc(' + AccountID + ') ChqID : ' + CAST(ChequeID AS VARCHAR) + ' Rsn:  ' + dbo.fn_ReturnCodeDescriptionsIncomingTZ(ReturnCode)  
 WHERE TrxType = 'ID'  
  AND VoucherCode NOT IN (  
   '40'  
   ,'60'  
   )  
  AND ReturnCode <> '00'  
  AND FileName = @FileName  
  AND MsgID = @MsgID  
  AND TrxID = @TrxID  
  
 UPDATE t_IncomingTransactions  
 SET DRN = 'E'  
  ,Description = 'UnPaid ID To : Acc(' + AccountID + ') ChqID : ' + CAST(ChequeID AS VARCHAR) + ' Rsn:  ' + dbo.fn_ReturnCodeDescriptionsIncomingTZ(ReturnCode)  
 WHERE TrxType = 'ID'  
  AND VoucherCode IN ('60')  
  AND ReturnCode <> '00'  
  AND FileName = @FileName  
  AND MsgID = @MsgID  
  AND TrxID = @TrxID  
  
  
 UPDATE t_IncomingTransactions  
 SET DRN = 'J'  
  ,Description = 'UnPaid ID Rsn:  ' + dbo.fn_ReturnCodeDescriptionsIncomingTZ(LTRIM(RTRIM(ReturnCode)))  
 WHERE TrxType = 'ID'  
  AND ReturnCode <> '00'  
  AND FileName = @FileName  
  AND MsgID = @MsgID  
  AND TrxID = @TrxID  
  
 DECLARE @DateDiff INT  
  ,@ReqDate DATE  
  ,@ReqDateStr VARCHAR(100)  
  ,@ReqdCollDate DATETIME  
  ,@StrTemp VARCHAR(10)  
  
 SELECT @ReqdColltnDt = LEFT(@ReqdColltnDt, 10)  
  
 SELECT @StrTemp = @ReqdColltnDt  
  
 BEGIN TRY  
  SELECT @ReqdCollDate = CONVERT(DATETIME, @ReqdColltnDt, 104)  
 END TRY  
  
 BEGIN CATCH  
  BEGIN TRY  
   SELECT @ReqdCollDate = CONVERT(DATETIME, @ReqdColltnDt, 102)  
  END TRY  
  
  BEGIN CATCH  
   BEGIN TRY  
    SELECT @ReqdCollDate = CONVERT(DATETIME, @ReqdColltnDt, 103)  
   END TRY  
  
   BEGIN CATCH  
    SELECT @ReqdCollDate = NULL  
   END CATCH  
  END CATCH  
 END CATCH  
  
 SELECT @ReqDate = CONVERT(DATE, @ReqdCollDate)  
  
 SELECT @DateDiff = DateDiff(day, CONVERT(DATE, GetDate()), @ReqDate)  
  
 IF @DateDiff > 10  
 BEGIN  
  BEGIN TRY  
   SELECT @ReqdCollDate = ''  
  
   SELECT @ReqdCollDate = CONVERT(DATETIME, @ReqDateStr, 102)  
  END TRY  
  
  BEGIN CATCH  
   SELECT @ReqdCollDate = NULL  
  END CATCH  
 END  
  
 --SELECT @ReqdColltnDt, @DateDiff  
 BEGIN TRY  
  SELECT @DtOfSgntr = CONVERT(DATETIME, @DtOfSgntr, 104)  
 END TRY  
  
 BEGIN CATCH  
  SELECT @DtOfSgntr = NULL  
 END CATCH  
  
 IF IsNull(@DtOfSgntr, '') = ''  
 BEGIN  
  SET @DtOfSgntr = NULL  
 END  
  
 DELETE  
 FROM t_IncomingClrTrxExtraDetails  
 WHERE TrxID = @TrxID  
  AND OrgnlMsgId = @MsgID  
  AND OrgnlEndToEnd = @OrgnlEndToEnd  
  
 INSERT INTO t_IncomingClrTrxExtraDetails (  
  TrxID  
  ,TrxDate  
  ,ColumnID  
  ,TrxRowID  
  ,OrgnlInstrId  
  ,OrgnlMsgId  
  ,IntrBkSttlmAmt  
  ,OrgnlEndToEnd  
  ,MndtId  
  ,DtOfSgntr  
  ,ReqdColltnDt  
  ,Frqcy  
  ,UstrdColD  
  ,UstrdChqdt  
  ,UstrdBWF  
  ,UstrdBWR  
  ,UstrdGS  
  ,UstrdUV  
  ,UstrdMicr  
  ,DAdrLine  
  ,DTwnNm  
  ,DCtry  
  ,DNm  
  ,DPhneNb  
  ,DMobNb  
  ,DEmailAdr  
  ,DOthr  
  ,DbtrAcct  
  ,CAdrLine  
  ,CTwnNm  
  ,CCtry  
  ,CNm  
  ,CPhneNb  
  ,CMobNb  
  ,CEmailAdr  
  ,COthr  
  ,PymType  
  ,CdtrAcct  
  ,DCNm  
  ,CCNm  
  ,FnlColltnDt  
  ,SourceBIC  
  ,Ustrd  
  ,LclInstrm  
  ,CtgyPurp  
  ,SvcLvl  
  ,ReqdColltnDate  
  )  
 VALUES (  
  @TrxID  
  ,dbo.f_GetWorkingDate(@OurBranchID)  
  ,''  
  ,''  
  ,@OrgnlInstrID  
  ,@MsgID  
  ,@Amount  
  ,@OrgnlEndToEnd  
  ,@MndtId  
  ,@DtOfSgntr  
  ,@ReqdColltnDt  
  ,@Frqcy  
  ,@UstrdColD  
  ,@ChequeDigit  
  ,@UstrdBWF  
  ,@UstrdBWR  
  ,@UstrdGS  
  ,@UstrdUV  
  ,@UstrdMicr  
  ,@DAdrLine  
  ,@DTwnNm  
  ,@DCtry  
  ,@DNm  
  ,@DPhneNb  
  ,@DMobNb  
  ,@DEmailAdr  
  ,@DOthr  
  ,@DbtrAcct  
  ,@CAdrLine  
  ,@CTwnNm  
  ,@CCtry  
  ,@CNm  
  ,@CPhneNb  
  ,@CMobNb  
  ,@CEmailAdr  
  ,@COthr  
  ,@PymType  
  ,@CdtrAcct  
  ,@DCNm  
  ,@CCNm  
  ,@FnlColltnDt  
  ,@SourceBIC  
  ,@RemittanceInfo  
  ,@LclInstrm  
  ,@CtgyPurp  
  ,@SvcLvl  
  ,@ReqdCollDate  
  )  
  
 DECLARE @ReturnCodeID CHAR(2)  
  ,@TrxBranchID BranchID  
  ,@OperatorID VARCHAR(50)  
  ,@InTrxTypeID CHAR(2)  
  ,@OutTrxDescription Description  
  ,@InTrxDescription Description  
  ,@OutTrxTypeID CHAR(2)  
  ,@outModuleID VARCHAR(6)  
  ,@InModuleID VARCHAR(6)  
  
 SELECT @OurBranchID = @OurBranchID  
  ,@AccountID = AccountID  
  ,@ColumnID = ColumnID  
  ,@Amount = Amount  
  ,@ChequeID = ChequeID  
  ,@ReturnCodeID = ReturnCode  
  ,@TrxType = TrxType  
  ,@TrxBranchID = TrxBranchID  
 FROM t_IncomingTransactions  
 WHERE FileName = @FileName  
  AND MsgID = @MsgID  
  AND TrxID = @TrxID  
  
--Removing this so that all incoming transactions go to verification queue and the decision   
--be made there so we don't miss the settlement window bnyaswa 13072026.  
   
 --SELECT @Auto_unpay = 0  
  
 --IF isNull(@ReturnCodeID, '00') IN (  
 --  '00'  
 --  ,'17'  
 --  )  
 --BEGIN  
 -- IF @TrxType = 'IC'  
 -- BEGIN  
 --  SELECT @OutTrxTypeID = 'OD'  
  
 --  SELECT @outModuleID = '3070'  
  
 --  SELECT @InModuleID = '3040'  
 -- END  
 -- ELSE  
 -- BEGIN  
 --  SELECT @OutTrxTypeID = 'OC'  
  
 --  SELECT @outModuleID = '3060'  
  
 --  SELECT @InModuleID = '3050'  
 -- END  
  
 -- --If the file is EFT and the account is a USD account, reject this with AG01 - Transaction Forbidden  
 -- IF @CurrencyID <> @LocalCurrencyID AND @TrxType = 'IC'  
 --  AND @Auto_unpay = 0  
 -- BEGIN  
 --  SELECT @ReturnCodeID = '48' --AG01 - Transaction Forbidden  
  
 --  SELECT @OutTrxDescription = 'Auto-Unpaid reason Transaction Forbidden Account ID (' + @AccountID + ')'  
  
 --  SELECT @InTrxDescription = 'Incoming Inward'  
  
 --  SELECT @OutTrxTypeID = @OutTrxTypeID  
  
 --  SELECT @InTrxTypeID = @TrxType  
  
 --  SELECT @outModuleID = @outModuleID  
  
 --  SELECT @InModuleID = @InModuleID  
  
 --  SELECT @Auto_unpay = 1  
 -- END  
  
 -- ---Invalid Account  
 -- IF dbo.f_GetAccStatus(@OurBranchID, @AccountID) = 'INV'    
 --  AND @Auto_unpay = 0  
 -- BEGIN  
 --  SELECT @ReturnCodeID = '45' --AC01 - Incorrect Account Number  
  
 --  SELECT @OutTrxDescription = 'Auto-Unpaid reason Invalid Account ID (' + @AccountID + ')'  
  
 --  SELECT @InTrxDescription = 'Incoming Inward'  
  
 --  SELECT @OutTrxTypeID = @OutTrxTypeID  
  
 --  SELECT @InTrxTypeID = @TrxType  
  
 --  SELECT @outModuleID = @outModuleID  
  
 --  SELECT @InModuleID = @InModuleID  
  
 --  SELECT @Auto_unpay = 1  
 -- END  
  
 -- --Stop Payment  
 -- IF dbo.f_GetStoppedAccChequeIDStatus(@OurBranchID, @AccountID, @ChequeID) = 'S'  
 -- BEGIN  
 --  SELECT @ReturnCodeID = '75' --'CH23 - Payment stopped by Drawer  
  
 --  SELECT @OutTrxDescription = 'Auto-Unpaid reason Payment stopped by Drawer Acc(' + @AccountID + ')'  
  
 --  SELECT @InTrxDescription = 'Incoming Inward'  
  
 --  SELECT @OutTrxTypeID = 'OC'  
  
 --  SELECT @InTrxTypeID = 'ID'  
  
 --  SELECT @outModuleID = '3060'  
  
 --  SELECT @InModuleID = '3050'  
  
 --  SELECT @Auto_unpay = 1  
 -- END  
  
 -- --Dormat Account  
 -- IF dbo.f_GetAccStatus(@OurBranchID, @AccountID) = 'AD'  
 --  AND @Auto_unpay = 0  
 -- BEGIN  
 --  SELECT @ReturnCodeID = '48' --'AG01 - Transaction Forbidden'  
  
 --  SELECT @OutTrxDescription = 'Auto-Unpaid reason Dormant Account (' + @AccountID + ')'  
  
 --  SELECT @InTrxDescription = 'Incoming Inward'  
  
 --  SELECT @OutTrxTypeID = @OutTrxTypeID  
  
 --  SELECT @InTrxTypeID = @TrxType  
  
 --  SELECT @outModuleID = @outModuleID  
  
 --  SELECT @InModuleID = @InModuleID  
  
 --  SELECT @Auto_unpay = 1  
 -- END  
  
 -- --Blocked Account  
 -- IF dbo.f_GetAccStatus(@OurBranchID, @AccountID) = 'AB'  
 --  AND @Auto_unpay = 0  
 -- BEGIN  
 --  SELECT @ReturnCodeID = '47' --'AC06 - Blocked Account'  
  
 --  SELECT @OutTrxDescription = 'Auto-Unpaid reason Blocked Account (' + @AccountID + ')'  
  
 --  SELECT @InTrxDescription = 'Incoming Inward'  
  
 --  SELECT @OutTrxTypeID = @OutTrxTypeID  
  
 --  SELECT @InTrxTypeID = @TrxType  
  
 --  SELECT @outModuleID = @outModuleID  
  
 --  SELECT @InModuleID = @InModuleID  
  
 --  SELECT @Auto_unpay = 1  
 -- END  
  
 -- ---Closed Account  
 -- IF dbo.f_GetAccStatus(@OurBranchID, @AccountID) = 'AC'  
 --  AND @Auto_unpay = 0  
 -- BEGIN  
 --  SELECT @ReturnCodeID = '46' --AC04 - Closed Account Number  
  
 --  SELECT @OutTrxDescription = 'Auto-Unpaid reason Closed Account (' + @AccountID + ')'  
  
 --  SELECT @InTrxDescription = 'Incoming Inward'  
  
 --  SELECT @OutTrxTypeID = @OutTrxTypeID  
  
 --  SELECT @InTrxTypeID = @TrxType  
  
 --  SELECT @outModuleID = @outModuleID  
  
 --  SELECT @InModuleID = @InModuleID  
  
 --  SELECT @Auto_unpay = 1  
 -- END  
  
 -- --Insufficient Fund  
    
 -- IF ABS(@Amount) > dbo.f_GetAvailableBalance(@OurBranchID, @AccountID)   
 --  AND @Auto_unpay = 0  
 --  AND @TrxType = 'ID'  
 -- BEGIN  
 --  SELECT @ReturnCodeID = '50' --AM04 - Insufficient Funds (Refer to Drawer)  
  
 --  SELECT @OutTrxDescription = 'Auto-Unpaid reason Insufficient Funds (Refer to Drawer) Acc(' + @AccountID + ')'  
  
 --  SELECT @InTrxDescription = 'Incoming Inward'  
  
 --  SELECT @OutTrxTypeID = @OutTrxTypeID  
  
 --  SELECT @InTrxTypeID = @TrxType  
  
 --  SELECT @outModuleID = @outModuleID  
  
 --  SELECT @InModuleID = @InModuleID  
  
 --  SELECT @Auto_unpay = 1  
 -- END  
  
 -- IF @Auto_unpay = 1  
 -- BEGIN  
 --  EXEC p_AutoUnpayIncomingTxns   
 --    @ColumnID = @ColumnID  
 --   ,@ReturnCodeID = @ReturnCodeID  
 --   ,@TrxBranchID = @TrxBranchID  
 --   ,@OperatorID = 'ClrSys'  
 --   ,@outModuleID = @outModuleID  
 --   ,@InModuleID = @InModuleID  
 --   ,@OutTrxTypeID = @OutTrxTypeID  
 --   ,@InTrxTypeID = @InTrxTypeID  
 --   ,@OutTrxDescription = @OutTrxDescription  
 --   ,@InTrxDescription = @InTrxDescription  
  
 --  UPDATE t_IncomingTransactions  
 --  SET [Description] = @OutTrxDescription  
 --   ,[Status] = 1  
 --   ,AccountType = 'G'  
 --   ,ProductID = 'GL'  
 --   ,AccountID = dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID, CurrencyID, CASE WHEN TrxType = 'IC' THEN 'ACP_CLR_SUSP_AC_EFT' ELSE 'ACP_CLR_SUSP_AC_CHQ' END)  
 --   ,FullName = dbo.f_GetGLAccountName(dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID, CurrencyID, CASE WHEN TrxType = 'IC' THEN 'ACP_CLR_SUSP_AC_EFT' ELSE 'ACP_CLR_SUSP_AC_CHQ' END))  
 --   ,MainGLID = dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID, CurrencyID, CASE WHEN TrxType = 'IC' THEN 'ACP_CLR_SUSP_AC_EFT' ELSE 'ACP_CLR_SUSP_AC_CHQ' END)  
 --   ,isProcessed = 1  
 --   ,AuthorizedBy = 'ClrSys'  
 --   ,AuthorizedOn = GetDate()  
 --  WHERE FileName = @FileName  
 --   AND MsgID = @MsgID  
 --   AND TrxID = @TrxID  
 --   AND ISNULL(IsProcessed, 0) = 0  
 -- END  
 --END  
  
 EXEC p_DoInwardsFileTrxValidationsTZ  
END  
ELSE  
BEGIN  
 PRINT 'Hapa'  
  
 RETURN  
END  
  
SET NOCOUNT OFF  
  
  
  