CREATE PROCEDURE [dbo].[p_AddInwardsTZ]  
 (  
  @FileName   varchar(100),   
  @Data    varchar(255),  
  @ReturnCode   varchar(4),  
  @VoucherCode  varchar(4),  
  @Amount    Numeric(18,2),  
  @BankID    varchar(20),  
  @BranchID   varchar(4),  
  @OurBankID   varchar(20),  
  @OurBranchID  varchar(5),  
  @ChequeID   varchar(11) = '',  
  @ChequeDigit  varchar(2)='',  
  @Date    datetime = '',  
  @AccountID   varchar(30),  
  @ImageUniqueID  VARCHAR(20)='',  
  @TFImageSize  varchar(15)='',  
  @JFImageSize  varchar(15)='',  
  @JRImageSize  varchar(15)='',  
  @FrontTFImage  text=null,  
  @FrontImage   text=null,  
  @BackImage   text=null,  
  @UVImage   text=null,  
  @TFImageSignature varchar(50)='',  
  @JFImageSignature varchar(50)='' ,  
  @JRImageSignature varchar(50)='',  
  @Validity   BIT =0,  
  @TFdpi    varchar(5)='',  
  @JRdpi    varchar(5)='',  
  @JFdpi    varchar(5)='',  
  @TheirAccountID     varchar(20)='',  
  @Drawer    Varchar(250)='',  
  @MsgID    Varchar(250)='',  
  @EFTID    Varchar(50)='',  
  @ColumnID   Numeric(18,0),  
  @isMdv    bit =0,  
  @TrxType   Varchar(4)='',  
  @TrxID    Varchar(200),  
  @ExtraDetails  Varchar(200),  
  @DAdrLine     varchar(100) = '',  
  @DTwnNm     varchar(100) = '',  
  @DCtry     varchar(100) = '',  
  @DNm      varchar(100) = '',  
  @DPhneNb     varchar(100) = '',  
  @DMobNb     varchar(100) = '',  
  @DEmailAdr    varchar(100) = '',  
  @DOthr     varchar(100) = '',  
  @DbtrAcct     varchar(100) = '',  
  @CAdrLine     varchar(100) = '',  
  @CTwnNm     varchar(100) = '',  
  @CCtry     varchar(100) = '',  
  @CNm      varchar(100) = '',  
  @CPhneNb     varchar(100) = '',  
  @CMobNb     varchar(100) = '',  
  @CEmailAdr    varchar(100) = '',  
  @COthr     varchar(100) = '',  
  @PymType     varchar(100) = '',  
  @CdtrAcct     varchar(100) = '',  
  @OrgnlInstrID    varchar(100) = '',  
  @UstrdColD    varchar(100) = '',  
  @UstrdBWF     varchar(100) = '',  
  @UstrdBWR     varchar(100) = '',  
  @UstrdGS     varchar(100) = '',  
  @UstrdUV     varchar(100) = '',  
  @UstrdMicr    varchar(100) = '',  
  @MndtId     varchar(100) = '',  
  @DtOfSgntr    varchar(100) = '',  
  @ReqdColltnDt    varchar(100) = '',  
  @Frqcy     varchar(100) = '',  
  @OrgnlEndToEnd  varchar(100) = '',  
  @DCNm    varchar(100) = '',  
  @CCNm    varchar(100) = '',  
  @FnlColltnDt  varchar(100) = '',  
  @IntrBkSttlmDt  varchar(100) = '',  
  @SvcLvl    varchar(100) = '',  
  @OrgnlTxId   varchar(100) = '',  
  @CurrencyID   VarChar(5)   = '',  
  @SourceBIC   Varchar(20)  = '',  
  @RemittanceInfo  Varchar(20)  = '',  
  @LclInstrm   Varchar(20)  = '',  
  @CtgyPurp   Varchar(20)  = '',  
  @OrgnlIntrBkSttlmDt  varchar(100) = ''  
 )  
AS   
SET NOCOUNT ON  
SET DATEFORMAT dmy;  
  
DECLARE @ImageID  BigInt,  
  @OurRealHQBrnID BranchID,  
  @ImportBatchID varchar(100)  
SELECT @AccountID = RIGHT(@AccountID,15)  
  
IF @ReturnCode <> '00'  
BEGIN  
 SELECT @SourceBIC = @BankID  
END  
ELSE  
BEGIN  
 SELECT @SourceBIC = @BankID  
END  
  
IF iSnull(@OrgnlTxId,'') = ''  
BEGIN   
 SELECT @OrgnlTxId = @OrgnlEndToEnd  
END  
--SELECT @OrgnlTxId  
-----Adding this to make test environemt work, if this taken to live and not commented, live will have issues  
DECLARE @SBIC varchar(20)  
SELECT  @SBIC = @SourceBIC  
SELECT  @SourceBIC = SwiftCode FROM t_Bank WHERE SwiftCode LIKE '%' + LEFT(@SourceBIC,6) + '%'  
SELECT  @BankID= dbo.f_GetClearingBankIDFromSwiftID(@BankID)  
  
SELECT @SourceBIC = isNull(@sourceBIC,@SBIC)  
------------------------------------------------------------------------------------------------------------------  
IF ISNULL(@OurBranchID,'') = '' SELECT @OurBranchID = OurBranchID FROM t_SystemBranchSetting WHERE IsHeadOffice = 1  
SELECT @Date = dbo.f_GetWorkingDate(@OurBranchID)  
SELECT @OurRealHQBrnID = OurBranchID FROM t_SystemBranchSetting WHERE IsHeadOffice = 1  
DECLARE @sub Varchar(20)  
--IF ISNUMERIC(@ReturnCode) = 1  
--BEGIN  
-- WHILE PATINDEX('%[a-z.-><]%', @ReturnCode) > 0  
-- BEGIN  
--  SET @sub = SUBSTRING(@ReturnCode, PATINDEX('%[a-z.-><]%', @ReturnCode), 1)  
--  SET @ReturnCode = REPLACE(@ReturnCode, @sub, '0')  
-- END  
--END  
--IF ISNUMERIC(@VoucherCode)  = 1  
--BEGIN  
-- WHILE PATINDEX('%[.-><]%', @VoucherCode) > 0  
-- BEGIN  
--  SET @sub = SUBSTRING(@VoucherCode, PATINDEX('%[.-><]%', @VoucherCode), 1)  
--  SET @VoucherCode = REPLACE(@VoucherCode, @sub, '0')  
-- END  
--END  
  
  
SELECT @OurBranchID = isNull(RIGHT('000'+ RTRIM(@OurBranchID),3),@OurRealHQBrnID)  
  
IF NOT EXISTS(SELECT 1 FROM t_SystemBranchSetting WHERE OurBranchID = @OurBranchID)  
BEGIN  
 SELECT @OurBranchID = @OurRealHQBrnID  
END  
  
IF @AccountID LIKE '%21100100%'  
BEGIN  
 SELECT @AccountID = '21100100'  
END  
  
IF @AccountID LIKE '%11700100%'  
BEGIN  
 SELECT @AccountID = '11700100'  
END  
  
  
SELECT @OurBranchID = isNull(dbo.f_GetNewAccBranchID(@OurBranchID,@AccountID),@OurBranchID)  
SELECT @AccountID = dbo.f_GetNewCustAccountID(@OurBranchID,@AccountID)  
SELECT @OurBranchID = isNull(dbo.f_GetNewAccBranchID(@OurBranchID,@AccountID),@OurBranchID)  
  
  
  
IF @TrxType = 'ID' AND @ReturnCode = '00' AND @AccountID != '21100100' AND @VoucherCode <> '60'    
BEGIN    
 SET @AccountID = @OurBranchID + '1' + @AccountID    
END    
  
--GLs attached to this class will pass as is and no masking will be done, in short, gls attached to this class will post without problems  
IF Len(@AccountID) <> 14 AND @AccountID NOT IN (SELECT AccountID FROM t_GeneralLedger (NOLOCK) WHERE GLClassID = 'EFT01')  
BEGIN  
 SELECT @AccountID = Right('00000000000000'+rtrim(@AccountID),14)  
END  
  
IF @AccountID IN ('14600250','14600260','14600270')  
BEGIN  
 SELECT @OurBranchID = OurBranchID FROM t_SystemBranchSetting WHERE IsHeadOffice = 1  
END   
  
IF @TrxType = 'ID' AND @VoucherCode = '00'   
BEGIN  
 SELECT @OurBranchID = LEFT(@AccountID,3)  
END  
  
IF @TrxType = 'ID' AND @VoucherCode = '60'    
BEGIN    
 DECLARE @StartDat2 BigInt,    
 @StartDat BigInt,    
 @Akaunti Varchar(30)  ,   
 @Auto_unpay bit = 0  
  
    
    
 SELECT @StartDat = dbo.INSTR(@Data, '/', 2, 2), @StartDat2 = dbo.INSTR(@Data, '/', 2, 3)     
 SELECT @Akaunti = SUBSTRING(@Data,@StartDat+1,(@StartDat2-@StartDat)-1)    
 --SELECT @Akaunti    
 SET @AccountID = @OurBranchID + '5' + @Akaunti    
END    
  
--IF @TrxType = 'IC'   
--BEGIN  
-- SELECT @OurBranchID = LEFT(@AccountID,3)  
--END --we cannot do this, some accountIDs were transferred from one branch to another  
  
IF EXISTS (SELECT 1 FROM t_IncomingClrTrxExtraDetails  WHERE OrgnlMsgID = @MsgID AND TrxID = @TrxID)  
BEGIN  
 RETURN  
END  
  
IF NOT EXISTS( SELECT 1 FROM t_IncomingTransactions  WHERE FileName = @FileName AND MsgID = @MsgID AND TrxID = @TrxID)  
 BEGIN  
    
  
 UPDATE t_IncomingTransactions SET ModuleID = CASE WHEN TrxType ='ID' THEN '3050' ELSE '3040' END  
 WHERE isNull(ModuleID,0) = 0 AND TrxType IN ('ID','IC')   
  
 DELETE FROM t_systemrecordlocks WHERE LockModuleID='1300' AND ModuleID In('3050','3040','3060','3070')  
  
 UPDATE t_IncomingTransactions   
 SET  t_IncomingTransactions.TrxDescriptionID = CASE WHEN TrxType='ID' THEN '004' ELSE '003' END   
 WHERE TrxType IN ('ID', 'IC')  AND isNull(TrxDescriptionID,'') =''  
  
  
 UPDATE t_IncomingTransactions  
 SET  t_IncomingTransactions.OurBankID = (SELECT BankID FROM t_SystemBankSetting)  
 WHERE isNull(OurBankID,'') = ''  
  
 IF isNull(@ImageID,'') = ''  
 BEGIN  
  EXEC p_GetUniqueClearingImageID @ImageID OUTPUT     
 END   
  
  
 EXEC p_GetNextTrxBatchID     
  @BranchID=@OurBranchID,    
  @NextTrxBatchID= @ImportBatchID OutPut    
  
  
 INSERT INTO t_IncomingTransactions   
 (  
   Date,   
   FileName,   
   Data,  
   ReturnCode,  
   VoucherCode,  
   Amount,  
   BankID,  
   BranchID,  
   AccountID,  
   OurBankID,  
   OurBranchID,  
   ChequeID,  
   ChequeDigit,  
   validity,  
   TheirAccount,  
   DrawerOrPayee,  
   MsgID,  
   FrontBWImage,  
   FrontGrayScaleImage,  
   RearImage,  
   UVImage,  
   TrxBranchID,  
   TrxType,  
   TrxID,  
   ImageID,  
   TrxBatchID,  
   Createdon,  
   ImportBatchID,  
   MicrLine,  
   DRN,  
   ExtraDetails,  
   CurrencyID  
  )   
 VALUES  
  (   
  --SELECT  
   @Date,   
   @FileName,   
   @Data,  
   RTRIM(LTRIM(@ReturnCode)),  
   LEFT(@VoucherCode,2),  
   @Amount,  
   @BankID,  
   RIGHT(@BranchID,2),  
   @AccountID,  
   @OurBankID,  
   @OurBranchID,  
   @ChequeID,  
   @ChequeDigit,  
   @validity,  
   @TheirAccountID,  
   @Drawer,  
   @MsgID,  
   @FrontTFImage,  
   @FrontImage,  
   @BackImage,  
   @UVImage,  
   @OurBranchID,  
   RTRIM(LTRIM(@TrxType)),  
   @TrxID,  
   @ImageID,  
   null,  
   CONVERT(date, getdate()),  
   @ImportBatchID,  
   @UstrdMicr,  
   @OrgnlTxId,  
   @ExtraDetails,  
   @CurrencyID  
  )  
  
  return  
  UPDATE t_IncomingTransactions SET AccountID = '22301600', ProductID = 'GL' , AccountType ='G'  
  WHERE FileName = @FileName AND MsgID = @MsgID AND TrxID = @TrxID AND AccountID Like '%22301600%'  
    
  UPDATE t_IncomingTransactions SET AccountID = '11700100', ProductID = 'GL' , AccountType ='G'  
  WHERE FileName = @FileName AND MsgID = @MsgID AND TrxID = @TrxID AND AccountID Like '%11700100%'  
    
  IF @ReturnCode <> '00'  
  BEGIN  
     
     
   IF @TrxType = 'ID'  
   BEGIN  
     BEGIN TRY  
      IF EXISTS(SELECT 1 FROM t_IncomingTransactions,t_AccountTrxClearing  
       WHERE FileName = @FileName AND MsgID = @MsgID AND t_IncomingTransactions.TrxID = @TrxID   
       AND t_AccountTrxClearing.TrxType = 'OC'   
       AND t_AccountTrxClearing.TrxRowID =  CAST(@TrxID  AS bigint) AND t_IncomingTransactions.ReturnCode <> '00')  
       BEGIN  
       --select 'Kamunya 1'  
       --select 'Kamunya 1', @OrgnlTxId, @FileName, @MsgID, @TrxID   
        UPDATE t_IncomingTransactions  
        SET    
          AccountID = t_AccountTrxClearing.AccountID,  
          BankID = t_AccountTrxClearing.BankID,  
          BranchID  = t_AccountTrxClearing.BranchID,  
          OurBranchID = t_AccountTrxClearing.OurBranchID,  
          DrawerOrPayee = t_AccountTrxClearing.DrawerOrPayee,  
          VoucherCode = t_AccountTrxClearing.VoucherCode,  
          ImageID = CAST(t_AccountTrxClearing.ImageID AS VarChar),  
          ChequeID = t_AccountTrxClearing.ChequeID  
        FROM t_AccountTrxClearing  
        WHERE t_IncomingTransactions.FileName = @FileName AND t_IncomingTransactions.MsgID = @MsgID   
        AND t_IncomingTransactions.TrxID = @TrxID   
        AND t_AccountTrxClearing.TrxType = 'OC'   
        AND t_AccountTrxClearing.TrxRowID = CAST(@TrxID  AS bigint) AND t_IncomingTransactions.ReturnCode <> '00'  
       END  
       ELSE IF EXISTS(SELECT 1 FROM t_IncomingTransactions,t_AccountTrxClearing  
       WHERE FileName = @FileName AND MsgID = @MsgID AND t_IncomingTransactions.TrxID = @TrxID   
       AND t_AccountTrxClearing.TrxType = 'OC'   
       AND t_AccountTrxClearing.TrxRowID =  CAST(@OrgnlTxId AS bigint)  AND t_IncomingTransactions.ReturnCode <> '00')  
       BEGIN  
       --select 'Kamunya 2',@MsgID,@FileName,@TrxID ,@OrgnlTxId  
        UPDATE t_IncomingTransactions  
        SET    
          AccountID = t_AccountTrxClearing.AccountID,  
          BankID = t_AccountTrxClearing.BankID,  
          BranchID  = t_AccountTrxClearing.BranchID,  
          OurBranchID = t_AccountTrxClearing.OurBranchID,  
          DrawerOrPayee = t_AccountTrxClearing.DrawerOrPayee,  
          VoucherCode = t_AccountTrxClearing.VoucherCode,  
          ImageID = CAST(t_AccountTrxClearing.ImageID AS VarChar),  
          ChequeID = t_AccountTrxClearing.ChequeID  
        FROM t_AccountTrxClearing  
        WHERE FileName = @FileName AND MsgID = @MsgID AND t_IncomingTransactions.TrxID = @TrxID   
        AND t_AccountTrxClearing.TrxType = 'OC'   
        AND t_AccountTrxClearing.TrxRowID = CAST(@OrgnlTxId AS bigint) AND t_IncomingTransactions.ReturnCode <> '00'  
       END   
       ELSE IF EXISTS(SELECT 1 FROM t_IncomingTransactions,t_TrxClearing  
       WHERE FileName = @FileName AND MsgID = @MsgID AND t_IncomingTransactions.TrxID = @TrxID   
       AND t_TrxClearing.TrxType = 'OC'   
       AND t_TrxClearing.TrxRowID =  CAST(@TrxID  AS bigint) AND t_IncomingTransactions.ReturnCode <> '00')  
       BEGIN  
       --select 'Kamunya 3'  
       --select 'Kamunya 1', @OrgnlTxId, @FileName, @MsgID, @TrxID   
        UPDATE t_IncomingTransactions  
        SET    
          AccountID = t_TrxClearing.AccountID,  
          BankID = t_TrxClearing.BankID,  
          BranchID  = t_TrxClearing.BranchID,  
          OurBranchID = t_TrxClearing.OurBranchID,  
          DrawerOrPayee = t_TrxClearing.DrawerOrPayee,  
          VoucherCode = t_TrxClearing.VoucherCode,  
          ImageID = CAST(t_TrxClearing.ImageID AS VarChar),  
          ChequeID = t_TrxClearing.ChequeID  
        FROM t_TrxClearing  
        WHERE t_IncomingTransactions.FileName = @FileName AND t_IncomingTransactions.MsgID = @MsgID   
        AND t_IncomingTransactions.TrxID = @TrxID   
        AND t_TrxClearing.TrxType = 'OC'   
        AND t_TrxClearing.TrxRowID = CAST(@TrxID  AS bigint) AND t_IncomingTransactions.ReturnCode <> '00'  
       END  
       ELSE IF EXISTS(SELECT 1 FROM t_IncomingTransactions,t_TrxClearing  
       WHERE FileName = @FileName AND MsgID = @MsgID AND t_IncomingTransactions.TrxID = @TrxID   
       AND t_TrxClearing.TrxType = 'OC'   
       AND t_TrxClearing.TrxRowID =  CAST(@OrgnlTxId AS bigint)  AND t_IncomingTransactions.ReturnCode <> '00')  
       BEGIN  
       --select 'Kamunya 4'  
       --select 'Kamunya 2'  
        UPDATE t_IncomingTransactions  
        SET    
          AccountID = t_TrxClearing.AccountID,  
          BankID = t_TrxClearing.BankID,  
          BranchID  = t_TrxClearing.BranchID,  
          OurBranchID = t_TrxClearing.OurBranchID,  
          DrawerOrPayee = t_TrxClearing.DrawerOrPayee,  
          VoucherCode = t_TrxClearing.VoucherCode,  
          ImageID = CAST(t_TrxClearing.ImageID AS VarChar),  
          ChequeID = t_TrxClearing.ChequeID  
        FROM t_TrxClearing  
        WHERE FileName = @FileName AND MsgID = @MsgID AND t_IncomingTransactions.TrxID = @TrxID   
        AND t_TrxClearing.TrxType = 'OC'   
        AND t_TrxClearing.TrxRowID = CAST(@OrgnlTxId AS bigint) AND t_IncomingTransactions.ReturnCode <> '00'  
       END  
       -- ELSE  
       -- BEGIN  
       -- select 'Kamunya 3'  
        -- UPDATE t_IncomingTransactions  
        -- SET    
          -- AccountID = t_AccountTrxClearing.AccountID,  
          -- BankID = t_AccountTrxClearing.BankID,  
          -- BranchID  = t_AccountTrxClearing.BranchID,  
          -- OurBranchID = t_AccountTrxClearing.OurBranchID,  
          -- DrawerOrPayee = t_AccountTrxClearing.DrawerOrPayee,  
          -- VoucherCode = t_AccountTrxClearing.VoucherCode,  
          -- ImageID = CAST(t_AccountTrxClearing.ImageID AS VarChar),  
          -- ChequeID = t_AccountTrxClearing.ChequeID  
        -- FROM t_AccountTrxClearing  
        -- WHERE t_IncomingTransactions.FileName = @FileName AND t_IncomingTransactions.MsgID = @MsgID   
        -- AND t_IncomingTransactions.TrxID = @TrxID   
        -- AND t_AccountTrxClearing.TrxType = 'OC'   
        -- AND t_AccountTrxClearing.TrxRowID = CAST(@OrgnlEndToEnd AS bigint)  AND t_IncomingTransactions.ReturnCode <> '00'  
       -- END  
       
       UPDATE t_IncomingTransactions  
       SET    
         FRONTBWIMAGE = t_ChequeImages.TFImage,  
         FRONTGRAYSCALEIMAGE = t_ChequeImages.JFImage,  
         REARIMAGE  = t_ChequeImages.JRImage,  
         UVIMAGE = t_ChequeImages.UVImage  
       FROM BRNET_IMAGEServer.dbo.t_ChequeImages  
       WHERE FileName = @FileName AND MsgID = @MsgID AND TrxID = @TrxID   
        AND t_ChequeImages.ImageID = CAST(t_IncomingTransactions.ImageID AS bigInt)  
        AND t_IncomingTransactions.ReturnCode <> '00'   
        AND t_IncomingTransactions.TrxType = 'ID'   
        AND t_IncomingTransactions.VoucherCode <> '40'  
     END TRY  
     BEGIN CATCH  
      IF EXISTS(SELECT 1 FROM t_IncomingTransactions,t_AccountTrxClearing  
       WHERE FileName = @FileName AND MsgID = @MsgID AND t_IncomingTransactions.TrxID = @TrxID   
       AND t_AccountTrxClearing.TrxType = 'OC'   
       AND t_AccountTrxClearing.TrxRowID =  CAST(@OrgnlTxId AS bigint)  AND t_IncomingTransactions.ReturnCode <> '00')  
       BEGIN  
       --select 'Kamunya 5'  
       --select 'Kamunya 2'  
        UPDATE t_IncomingTransactions  
        SET    
          AccountID = t_AccountTrxClearing.AccountID,  
          BankID = t_AccountTrxClearing.BankID,  
          BranchID  = t_AccountTrxClearing.BranchID,  
          OurBranchID = t_AccountTrxClearing.OurBranchID,  
          DrawerOrPayee = t_AccountTrxClearing.DrawerOrPayee,  
          VoucherCode = t_AccountTrxClearing.VoucherCode,  
          ImageID = CAST(t_AccountTrxClearing.ImageID AS VarChar),  
          ChequeID = t_AccountTrxClearing.ChequeID  
        FROM t_AccountTrxClearing  
        WHERE FileName = @FileName AND MsgID = @MsgID AND t_IncomingTransactions.TrxID = @TrxID   
        AND t_AccountTrxClearing.TrxType = 'OC'   
        AND t_AccountTrxClearing.TrxRowID = CAST(@OrgnlTxId AS bigint) AND t_IncomingTransactions.ReturnCode <> '00'  
       END  
      ELSE IF EXISTS(SELECT 1 FROM t_IncomingTransactions,t_AccountTrxClearing  
       WHERE FileName = @FileName AND MsgID = @MsgID AND t_IncomingTransactions.TrxID = @TrxID   
       AND t_AccountTrxClearing.TrxType = 'OC'   
       AND t_AccountTrxClearing.TrxRowID =  CAST(@TrxID  AS bigint) AND t_IncomingTransactions.ReturnCode <> '00')  
       BEGIN  
       --select 'Kamunya 6'  
       --select 'Kamunya 1', @OrgnlTxId, @FileName, @MsgID, @TrxID   
        UPDATE t_IncomingTransactions  
        SET    
          AccountID = t_AccountTrxClearing.AccountID,  
          BankID = t_AccountTrxClearing.BankID,  
          BranchID  = t_AccountTrxClearing.BranchID,  
          OurBranchID = t_AccountTrxClearing.OurBranchID,  
          DrawerOrPayee = t_AccountTrxClearing.DrawerOrPayee,  
          VoucherCode = t_AccountTrxClearing.VoucherCode,  
          ImageID = CAST(t_AccountTrxClearing.ImageID AS VarChar),  
          ChequeID = t_AccountTrxClearing.ChequeID  
        FROM t_AccountTrxClearing  
        WHERE t_IncomingTransactions.FileName = @FileName AND t_IncomingTransactions.MsgID = @MsgID   
        AND t_IncomingTransactions.TrxID = @TrxID   
        AND t_AccountTrxClearing.TrxType = 'OC'   
        AND t_AccountTrxClearing.TrxRowID = CAST(@TrxID  AS bigint) AND t_IncomingTransactions.ReturnCode <> '00'  
       END  
      ELSE  IF EXISTS(SELECT 1 FROM t_IncomingTransactions,t_TrxClearing  
       WHERE FileName = @FileName AND MsgID = @MsgID AND t_IncomingTransactions.TrxID = @TrxID   
       AND t_TrxClearing.TrxType = 'OC'   
       AND t_TrxClearing.TrxRowID =  CAST(@OrgnlTxId AS bigint)  AND t_IncomingTransactions.ReturnCode <> '00')  
       BEGIN  
       --select 'Kamunya 7'  
       --select 'Kamunya 2'  
        UPDATE t_IncomingTransactions  
        SET    
          AccountID = t_TrxClearing.AccountID,  
          BankID = t_TrxClearing.BankID,  
          BranchID  = t_TrxClearing.BranchID,  
          OurBranchID = t_TrxClearing.OurBranchID,  
          DrawerOrPayee = t_TrxClearing.DrawerOrPayee,  
          VoucherCode = t_TrxClearing.VoucherCode,  
          ImageID = CAST(t_TrxClearing.ImageID AS VarChar),  
          ChequeID = t_TrxClearing.ChequeID  
        FROM t_TrxClearing  
        WHERE FileName = @FileName AND MsgID = @MsgID AND t_IncomingTransactions.TrxID = @TrxID   
        AND t_TrxClearing.TrxType = 'OC'   
        AND t_TrxClearing.TrxRowID = CAST(@OrgnlTxId AS bigint) AND t_IncomingTransactions.ReturnCode <> '00'  
       END  
      ELSE IF EXISTS(SELECT 1 FROM t_IncomingTransactions,t_TrxClearing  
       WHERE FileName = @FileName AND MsgID = @MsgID AND t_IncomingTransactions.TrxID = @TrxID   
       AND t_TrxClearing.TrxType = 'OC'   
       AND t_TrxClearing.TrxRowID =  CAST(@TrxID  AS bigint) AND t_IncomingTransactions.ReturnCode <> '00')  
       BEGIN  
       --select 'Kamunya 8'  
       --select 'Kamunya 1', @OrgnlTxId, @FileName, @MsgID, @TrxID   
        UPDATE t_IncomingTransactions  
        SET    
          AccountID = t_TrxClearing.AccountID,  
          BankID = t_TrxClearing.BankID,  
          BranchID  = t_TrxClearing.BranchID,  
          OurBranchID = t_TrxClearing.OurBranchID,  
          DrawerOrPayee = t_TrxClearing.DrawerOrPayee,  
          VoucherCode = t_TrxClearing.VoucherCode,  
          ImageID = CAST(t_TrxClearing.ImageID AS VarChar),  
          ChequeID = t_TrxClearing.ChequeID  
        FROM t_TrxClearing  
        WHERE t_IncomingTransactions.FileName = @FileName AND t_IncomingTransactions.MsgID = @MsgID   
        AND t_IncomingTransactions.TrxID = @TrxID   
        AND t_TrxClearing.TrxType = 'OC'   
        AND t_TrxClearing.TrxRowID = CAST(@TrxID  AS bigint) AND t_IncomingTransactions.ReturnCode <> '00'  
       END  
         
       
       UPDATE t_IncomingTransactions  
       SET    
         FRONTBWIMAGE = t_ChequeImages.TFImage,  
         FRONTGRAYSCALEIMAGE = t_ChequeImages.JFImage,  
         REARIMAGE  = t_ChequeImages.JRImage,  
         UVIMAGE = t_ChequeImages.UVImage  
       FROM BRNET_IMAGEServer.dbo.t_ChequeImages  
       WHERE FileName = @FileName AND MsgID = @MsgID AND TrxID = @TrxID   
        AND t_ChequeImages.ImageID = CAST(t_IncomingTransactions.ImageID AS bigInt)  
        AND t_IncomingTransactions.ReturnCode <> '00'   
        AND t_IncomingTransactions.TrxType = 'ID'   
        AND t_IncomingTransactions.VoucherCode <> '40'  
       
       
     END CATCH  
    END  
    ELSE  
    BEGIN  
    --DECLARE @TrxIDToUse BigInt, @OrgnlTxIdToUse BigInt  
    --IF ISNumeric(@TrxID) = 1  
    --BEGIN   
    -- SELECT @TrxIDToUse = TRY_CAST(@TrxID AS bigint)  
    --END  
    --ELSE  
    --BEGIN  
    -- SELECT @TrxIDToUse = TRY_CAST(@OrgnlTxId AS bigint)  
    --END  
--return  
  
    BEGIN TRY  
     IF EXISTS(SELECT 1 FROM t_IncomingTransactions,t_AccountTrxClearing  
      WHERE FileName = @FileName AND MsgID = @MsgID --AND TrxID = @TrxID   
      AND t_AccountTrxClearing.TrxType = 'OD'   
      AND t_AccountTrxClearing.TrxRowID = CAST(@TrxID AS bigint)   AND t_IncomingTransactions.ReturnCode <> '00')  
      BEGIN  
      --select 'Kamunya 9'  
       UPDATE t_IncomingTransactions  
       SET    
         AccountID = t_AccountTrxClearing.AccountID,  
         BankID = t_AccountTrxClearing.BankID,  
         BranchID  = t_AccountTrxClearing.BranchID,  
         OurBranchID = t_AccountTrxClearing.OurBranchID,  
         DrawerOrPayee = t_AccountTrxClearing.DrawerOrPayee,  
         VoucherCode = t_AccountTrxClearing.VoucherCode,  
         ImageID = CAST(t_AccountTrxClearing.ImageID AS VarChar),  
         ChequeID = t_AccountTrxClearing.ChequeID  
       FROM t_AccountTrxClearing  
       WHERE FileName = @FileName AND MsgID = @MsgID --AND TrxID = @TrxID   
       AND t_AccountTrxClearing.TrxType = 'OD'   
       AND t_AccountTrxClearing.TrxRowID = CAST(@TrxID AS bigint) AND t_IncomingTransactions.ReturnCode <> '00'  
      END  
      ELSE IF EXISTS(SELECT 1 FROM t_IncomingTransactions,t_AccountTrxClearing  
      WHERE FileName = @FileName AND MsgID = @MsgID --AND TrxID = @TrxID   
      AND t_AccountTrxClearing.TrxType = 'OD'   
      AND t_AccountTrxClearing.TrxRowID =  CAST(@OrgnlTxId AS bigint) AND t_IncomingTransactions.ReturnCode <> '00')  
      BEGIN  
      --select 'Kamunya 10'  
       --SELECT * FROM t_AccountTrxClearing, t_IncomingTransactions  
       --WHERE   
       -- t_IncomingTransactions.TrxID = '20210308_117651' AND t_AccountTrxClearing.TrxType = 'OD'   
       --AND t_AccountTrxClearing.TrxRowID =TRY_CAST('59580528' AS bigint) AND t_IncomingTransactions.ReturnCode <> '00'  
  
       UPDATE t_IncomingTransactions  
       SET    
         AccountID = t_AccountTrxClearing.AccountID,  
         BankID = t_AccountTrxClearing.BankID,  
         BranchID  = t_AccountTrxClearing.BranchID,  
         OurBranchID = t_AccountTrxClearing.OurBranchID,  
         DrawerOrPayee = t_AccountTrxClearing.DrawerOrPayee,  
         VoucherCode = t_AccountTrxClearing.VoucherCode,  
         ImageID = CAST(t_AccountTrxClearing.ImageID AS VarChar),  
         ChequeID = t_AccountTrxClearing.ChequeID  
       FROM t_AccountTrxClearing  
       WHERE t_IncomingTransactions.FileName = @FileName AND t_IncomingTransactions.MsgID = @MsgID   
       --AND t_IncomingTransactions.TrxID = @TrxID   
       AND t_AccountTrxClearing.TrxType = 'OD'   
       AND t_AccountTrxClearing.TrxRowID = CAST(@OrgnlTxId AS bigint) AND t_IncomingTransactions.ReturnCode <> '00'  
      END  
      ELSE IF EXISTS(SELECT 1 FROM t_IncomingTransactions,t_TrxClearing  
       WHERE FileName = @FileName AND MsgID = @MsgID AND t_IncomingTransactions.TrxID = @TrxID   
       AND t_TrxClearing.TrxType = 'OD'   
       AND t_TrxClearing.TrxRowID =  CAST(@OrgnlTxId AS bigint)  AND t_IncomingTransactions.ReturnCode <> '00')  
       BEGIN  
       --select 'Kamunya 11'  
       --select 'Kamunya 2'  
        UPDATE t_IncomingTransactions  
        SET    
          AccountID = t_TrxClearing.AccountID,  
          BankID = t_TrxClearing.BankID,  
          BranchID  = t_TrxClearing.BranchID,  
          OurBranchID = t_TrxClearing.OurBranchID,  
          DrawerOrPayee = t_TrxClearing.DrawerOrPayee,  
          VoucherCode = t_TrxClearing.VoucherCode,  
          ImageID = CAST(t_TrxClearing.ImageID AS VarChar),  
          ChequeID = t_TrxClearing.ChequeID  
        FROM t_TrxClearing  
        WHERE FileName = @FileName AND MsgID = @MsgID AND t_IncomingTransactions.TrxID = @TrxID   
        AND t_TrxClearing.TrxType = 'OD'   
        AND t_TrxClearing.TrxRowID = CAST(@OrgnlTxId AS bigint) AND t_IncomingTransactions.ReturnCode <> '00'  
       END  
      ELSE IF EXISTS(SELECT 1 FROM t_IncomingTransactions,t_TrxClearing  
       WHERE FileName = @FileName AND MsgID = @MsgID AND t_IncomingTransactions.TrxID = @TrxID   
       AND t_TrxClearing.TrxType = 'OD'   
       AND t_TrxClearing.TrxRowID =  CAST(@TrxID  AS bigint) AND t_IncomingTransactions.ReturnCode <> '00')  
       BEGIN  
       --select 'Kamunya 12'  
       --select 'Kamunya 1', @OrgnlTxId, @FileName, @MsgID, @TrxID   
        UPDATE t_IncomingTransactions  
        SET    
          AccountID = t_TrxClearing.AccountID,  
          BankID = t_TrxClearing.BankID,  
          BranchID  = t_TrxClearing.BranchID,  
          OurBranchID = t_TrxClearing.OurBranchID,  
          DrawerOrPayee = t_TrxClearing.DrawerOrPayee,  
          VoucherCode = t_TrxClearing.VoucherCode,  
          ImageID = CAST(t_TrxClearing.ImageID AS VarChar),  
          ChequeID = t_TrxClearing.ChequeID  
        FROM t_TrxClearing  
        WHERE t_IncomingTransactions.FileName = @FileName AND t_IncomingTransactions.MsgID = @MsgID   
        AND t_IncomingTransactions.TrxID = @TrxID   
        AND t_TrxClearing.TrxType = 'OD'   
        AND t_TrxClearing.TrxRowID = CAST(@TrxID  AS bigint) AND t_IncomingTransactions.ReturnCode <> '00'  
       END  
    END TRY  
    BEGIN CATCH  
     IF EXISTS(SELECT 1 FROM t_IncomingTransactions,t_AccountTrxClearing  
      WHERE FileName = @FileName AND MsgID = @MsgID --AND TrxID = @TrxID   
      AND t_AccountTrxClearing.TrxType = 'OD'   
      AND t_AccountTrxClearing.TrxRowID =  CAST(@OrgnlTxId AS bigint) AND t_IncomingTransactions.ReturnCode <> '00')  
      BEGIN  
      --select 'Kamunya 13'  
       --SELECT * FROM t_AccountTrxClearing, t_IncomingTransactions  
       --WHERE   
       -- t_IncomingTransactions.TrxID = '20210308_117651' AND t_AccountTrxClearing.TrxType = 'OD'   
       --AND t_AccountTrxClearing.TrxRowID =TRY_CAST('59580528' AS bigint) AND t_IncomingTransactions.ReturnCode <> '00'  
  
       UPDATE t_IncomingTransactions  
       SET    
         AccountID = t_AccountTrxClearing.AccountID,  
         BankID = t_AccountTrxClearing.BankID,  
         BranchID  = t_AccountTrxClearing.BranchID,  
         OurBranchID = t_AccountTrxClearing.OurBranchID,  
         DrawerOrPayee = t_AccountTrxClearing.DrawerOrPayee,  
         VoucherCode = t_AccountTrxClearing.VoucherCode,  
         ImageID = CAST(t_AccountTrxClearing.ImageID AS VarChar),  
         ChequeID = t_AccountTrxClearing.ChequeID  
       FROM t_AccountTrxClearing  
       WHERE t_IncomingTransactions.FileName = @FileName AND t_IncomingTransactions.MsgID = @MsgID   
       --AND t_IncomingTransactions.TrxID = @TrxID   
       AND t_AccountTrxClearing.TrxType = 'OD'   
       AND t_AccountTrxClearing.TrxRowID = CAST(@OrgnlTxId AS bigint) AND t_IncomingTransactions.ReturnCode <> '00'  
      END  
      ELSE IF EXISTS(SELECT 1 FROM t_IncomingTransactions,t_AccountTrxClearing  
       WHERE FileName = @FileName AND MsgID = @MsgID --AND TrxID = @TrxID   
       AND t_AccountTrxClearing.TrxType = 'OD'   
       AND t_AccountTrxClearing.TrxRowID = CAST(@TrxID AS bigint)   AND t_IncomingTransactions.ReturnCode <> '00')  
       BEGIN  
       --select 'Kamunya 14'  
        UPDATE t_IncomingTransactions  
        SET    
          AccountID = t_AccountTrxClearing.AccountID,  
          BankID = t_AccountTrxClearing.BankID,  
          BranchID  = t_AccountTrxClearing.BranchID,  
          OurBranchID = t_AccountTrxClearing.OurBranchID,  
          DrawerOrPayee = t_AccountTrxClearing.DrawerOrPayee,  
          VoucherCode = t_AccountTrxClearing.VoucherCode,  
          ImageID = CAST(t_AccountTrxClearing.ImageID AS VarChar),  
          ChequeID = t_AccountTrxClearing.ChequeID  
        FROM t_AccountTrxClearing  
        WHERE FileName = @FileName AND MsgID = @MsgID --AND TrxID = @TrxID   
        AND t_AccountTrxClearing.TrxType = 'OD'   
        AND t_AccountTrxClearing.TrxRowID = CAST(@TrxID AS bigint) AND t_IncomingTransactions.ReturnCode <> '00'  
       END  
      ELSE  IF EXISTS(SELECT 1 FROM t_IncomingTransactions,t_TrxClearing  
       WHERE FileName = @FileName AND MsgID = @MsgID AND t_IncomingTransactions.TrxID = @TrxID   
       AND t_TrxClearing.TrxType = 'OD'   
       AND t_TrxClearing.TrxRowID =  CAST(@OrgnlTxId AS bigint)  AND t_IncomingTransactions.ReturnCode <> '00')  
       BEGIN  
       --select 'Kamunya 15'  
       --select 'Kamunya 2'  
        UPDATE t_IncomingTransactions  
        SET    
          AccountID = t_TrxClearing.AccountID,  
          BankID = t_TrxClearing.BankID,  
          BranchID  = t_TrxClearing.BranchID,  
          OurBranchID = t_TrxClearing.OurBranchID,  
          DrawerOrPayee = t_TrxClearing.DrawerOrPayee,  
          VoucherCode = t_TrxClearing.VoucherCode,  
          ImageID = CAST(t_TrxClearing.ImageID AS VarChar),  
          ChequeID = t_TrxClearing.ChequeID  
        FROM t_TrxClearing  
        WHERE FileName = @FileName AND MsgID = @MsgID AND t_IncomingTransactions.TrxID = @TrxID   
        AND t_TrxClearing.TrxType = 'OD'   
        AND t_TrxClearing.TrxRowID = CAST(@OrgnlTxId AS bigint) AND t_IncomingTransactions.ReturnCode <> '00'  
       END  
      ELSE IF EXISTS(SELECT 1 FROM t_IncomingTransactions,t_TrxClearing  
       WHERE FileName = @FileName AND MsgID = @MsgID AND t_IncomingTransactions.TrxID = @TrxID   
       AND t_TrxClearing.TrxType = 'OD'   
       AND t_TrxClearing.TrxRowID =  CAST(@TrxID  AS bigint) AND t_IncomingTransactions.ReturnCode <> '00')  
       BEGIN  
       --select 'Kamunya 16'  
       --select 'Kamunya 1', @OrgnlTxId, @FileName, @MsgID, @TrxID   
        UPDATE t_IncomingTransactions  
        SET    
          AccountID = t_TrxClearing.AccountID,  
          BankID = t_TrxClearing.BankID,  
          BranchID  = t_TrxClearing.BranchID,  
          OurBranchID = t_TrxClearing.OurBranchID,  
          DrawerOrPayee = t_TrxClearing.DrawerOrPayee,  
          VoucherCode = t_TrxClearing.VoucherCode,  
          ImageID = CAST(t_TrxClearing.ImageID AS VarChar),  
          ChequeID = t_TrxClearing.ChequeID  
        FROM t_TrxClearing  
        WHERE t_IncomingTransactions.FileName = @FileName AND t_IncomingTransactions.MsgID = @MsgID   
        AND t_IncomingTransactions.TrxID = @TrxID   
        AND t_TrxClearing.TrxType = 'OD'   
        AND t_TrxClearing.TrxRowID = CAST(@TrxID  AS bigint) AND t_IncomingTransactions.ReturnCode <> '00'  
       END  
         
      
      
    END CATCH  
      
       
    UPDATE t_IncomingTransactions  
    SET    
      AccountType = dbo.f_GetAllAccountType(OurBranchID,AccountID),  
      FULLNAME = RIGHT(dbo.f_GetAccountName(OurbranchID,AccountID),100),  
      ProductID  = ISNULL(dbo.f_GetAccountProductID(OurBranchID,AccountID),''),  
      OurBankID = (SELECT BankID FROM t_SystemBranchSetting WHERE IsHeadOffice = 1),  
      VoucherCodeDescription = dbo.f_GetUserCodeName('VoucherID','58'),  
      VoucherCode = '58',  
      MainGLID = dbo.f_GetGLInterfaceAccountID1(OurBankID, ISNULL(dbo.f_GetAccountProductID(OurBranchID,AccountID),''),'CONTROL_AC'),  
      ContraGLID = dbo.f_GetCurrencyBranchGLAccountID(OurBranchID, CurrencyID, 'CUR_CLR_AC'),     
      DATE =  dbo.f_GetWorkingDate( OurBranchID )    
    WHERE FileName = @FileName AND MsgID = @MsgID AND TrxID = @TrxID AND TrxType = 'IC' AND ReturnCode <> '00'  
    AND t_IncomingTransactions.MsgID = @MsgID  
  
    UPDATE t_IncomingTransactions  
    SET    
      AccountType = dbo.f_GetAllAccountType(OurBranchID,AccountID),  
      FULLNAME = RIGHT(dbo.f_GetAccountName(OurbranchID,AccountID),100),  
      ProductID  = ISNULL(dbo.f_GetAccountProductID(OurBranchID,AccountID),''),  
      OurBankID = (SELECT BankID FROM t_SystemBranchSetting WHERE IsHeadOffice = 1),  
      VoucherCodeDescription =  dbo.f_GetUserCodeName('VoucherID',VoucherCode),  
      [Description] = 'Unpaid ' + TrxType + ' Rsn ' + dbo.f_CRB_ReturnCodeDescriptionsForUnpay(ReturnCode),  
      MainGLID = dbo.f_GetGLInterfaceAccountID1(OurBankID, ISNULL(dbo.f_GetAccountProductID(OurBranchID,AccountID),''),'CONTROL_AC'),  
      ContraGLID = dbo.f_GetCurrencyBranchGLAccountID(OurBranchID, CurrencyID, 'CUR_CLR_AC'),     
      DATE =  dbo.f_GetWorkingDate( OurBranchID )    
    WHERE FileName = @FileName AND MsgID = @MsgID AND TrxID = @TrxID AND TrxType = 'ID' AND ReturnCode <> '00'  
    AND t_IncomingTransactions.MsgID = @MsgID  
  
  
  
   END  
  END  
  ------------------END of UNpaying-----------------------------------------------------------------------  
  --AND AccountID NOT IN ('21100100','22230100','11700100')  
    
  UPDATE t_IncomingTransactions SET AccountID = '11700100', OurBranchID = '000', FullName =  
  dbo.f_GetGLAccountName(dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN 'TZS'  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'END), 'ACP_CLR_SUSP_AC')),  
          ProductID = 'GL' , AccountType ='G'  
  WHERE FileName = @FileName AND MsgID = @MsgID AND TrxID = @TrxID AND AccountID Like '%11700100%'  
  AND @ReturnCode IN ('00','17')  
  
        UPDATE t_IncomingTransactions SET AccountID = '21100100' WHERE AccountID LIKE '%21100100%'  
  AND FileName = @FileName AND MsgID = @MsgID AND TrxID = @TrxID AND ReturnCode = '00'  
    
  UPDATE t_IncomingTransactions SET AccountID = '22230100' WHERE AccountID LIKE '%22230100%'  
  AND FileName = @FileName AND MsgID = @MsgID AND TrxID = @TrxID AND ReturnCode = '00'  
    
  
  UPDATE t_IncomingTransactions SET AccountType = dbo.f_GetAllAccountType(OurBranchID,AccountID)    
  WHERE FileName = @FileName AND MsgID = @MsgID AND TrxID = @TrxID AND ReturnCode = '00'  
    
  UPDATE t_IncomingTransactions  
  SET    OurBranchID = isNull(dbo.f_GetAccountBranchID(AccountID), OurBranchID)  
  WHERE FileName = @FileName AND MsgID = @MsgID AND TrxID = @TrxID  
  AND ReturnCode = '00'  
  
  UPDATE t_IncomingTransactions  
  SET    
    AccountType = dbo.f_GetAllAccountType(OurBranchID,AccountID),  
    FULLNAME = RIGHT(dbo.f_GetAccountName(OurbranchID,AccountID),100),  
    ProductID  = ISNULL(dbo.f_GetAccountProductID(OurBranchID,AccountID),''),  
    OurBankID = (SELECT BankID FROM t_SystemBranchSetting WHERE IsHeadOffice = 1),  
    VoucherCodeDescription =  dbo.f_GetUserCodeName('VoucherID','58'),  
    VoucherCode = '58',  
    MainGLID = dbo.f_GetGLInterfaceAccountID1(OurBankID, ISNULL(dbo.f_GetAccountProductID(OurBranchID,AccountID),''),'CONTROL_AC'),  
    ContraGLID = dbo.f_GetCurrencyBranchGLAccountID(OurBranchID, CurrencyID, 'CUR_CLR_AC'),     
    DATE =  dbo.f_GetWorkingDate( OurBranchID )    
  WHERE FileName = @FileName AND MsgID = @MsgID AND TrxID = @TrxID AND TrxType = 'IC'  AND ReturnCode = '00'   
    
    
  
  UPDATE t_IncomingTransactions  
  SET    
    AccountType = dbo.f_GetAllAccountType(OurBranchID,AccountID),  
    FULLNAME = RIGHT(dbo.f_GetAccountName(OurbranchID,AccountID),100),  
    ProductID  = ISNULL(dbo.f_GetAccountProductID(OurBranchID,AccountID),''),  
    OurBankID = (SELECT BankID FROM t_SystemBranchSetting WHERE IsHeadOffice = 1),  
    VoucherCodeDescription = dbo.f_GetUserCodeName('VoucherID','58'),  
    VoucherCode = '58',  
    MainGLID = dbo.f_GetGLInterfaceAccountID1(OurBankID, ISNULL(dbo.f_GetAccountProductID(OurBranchID,AccountID),''),'CONTROL_AC'),  
    ContraGLID = dbo.f_GetCurrencyBranchGLAccountID(OurBranchID, CurrencyID, 'CUR_CLR_AC'),     
    DATE =  dbo.f_GetWorkingDate( OurBranchID )    
  WHERE FileName = @FileName AND MsgID = @MsgID AND TrxID = @TrxID AND TrxType = 'IC' AND ReturnCode <> '00'  
  
  UPDATE t_IncomingTransactions  
  SET    
    AccountType = dbo.f_GetAllAccountType(OurBranchID,AccountID),  
    FULLNAME = RIGHT(dbo.f_GetAccountName(OurbranchID,AccountID),100),  
    ProductID  = ISNULL(dbo.f_GetAccountProductID(OurBranchID,AccountID),''),  
    OurBankID = (SELECT BankID FROM t_SystemBranchSetting WHERE IsHeadOffice = 1),  
    VoucherCodeDescription =  dbo.f_GetUserCodeName('VoucherID',VoucherCode),  
    MainGLID = dbo.f_GetGLInterfaceAccountID1(OurBankID, ISNULL(dbo.f_GetAccountProductID(OurBranchID,AccountID),''),'CONTROL_AC'),  
    ContraGLID = dbo.f_GetCurrencyBranchGLAccountID(OurBranchID, CurrencyID, 'CUR_CLR_AC'),     
    DATE =  dbo.f_GetWorkingDate( OurBranchID )    
  WHERE FileName = @FileName AND MsgID = @MsgID AND TrxID = @TrxID AND TrxType = 'ID' AND ReturnCode = '00'  
  
  UPDATE t_IncomingTransactions  
  SET    
    AccountType = dbo.f_GetAllAccountType(OurBranchID,AccountID),  
    FULLNAME = RIGHT(dbo.f_GetAccountName(OurbranchID,AccountID),100),  
    ProductID  = ISNULL(dbo.f_GetAccountProductID(OurBranchID,AccountID),''),  
    OurBankID = (SELECT BankID FROM t_SystemBranchSetting WHERE IsHeadOffice = 1),  
    VoucherCodeDescription =  dbo.f_GetUserCodeName('VoucherID',VoucherCode),  
    [Description] = 'Unpaid ' + TrxType + ' Rsn ' + dbo.f_CRB_ReturnCodeDescriptionsForUnpay(ReturnCode),  
    MainGLID = dbo.f_GetGLInterfaceAccountID1(OurBankID, ISNULL(dbo.f_GetAccountProductID(OurBranchID,AccountID),''),'CONTROL_AC'),  
    ContraGLID = dbo.f_GetCurrencyBranchGLAccountID(OurBranchID, CurrencyID, 'CUR_CLR_AC'),     
    DATE =  dbo.f_GetWorkingDate( OurBranchID )    
  WHERE FileName = @FileName AND MsgID = @MsgID AND TrxID = @TrxID AND TrxType = 'ID' AND ReturnCode <> '00'  
  
  
  
  UPDATE t_IncomingTransactions SET BranchID = a.BranchID FROM (SELECT BankID,BranchID FROM t_Branch ) AS a    
   WHERE t_IncomingTransactions.BankID  = a.BankID  AND FileName = @FileName AND MsgID = @MsgID   
   AND TrxID = @TrxID  AND ReturnCode = '00'  
  
  UPDATE t_IncomingTransactions SET TrxDescriptionID = (CASE WHEN TrxType = 'ID' THEN '004' ELSE '003' END)   
  WHERE isNull(TrxDescriptionID,'') = '' AND FileName = @FileName AND MsgID = @MsgID AND TrxID = @TrxID  
  
  UPDATE t_IncomingTransactions SET ModuleID = (CASE WHEN TrxType = 'ID' THEN '3050' ELSE '3040' END)   
  WHERE isNull(ModuleID,'') = '' AND FileName = @FileName AND MsgID = @MsgID AND TrxID = @TrxID  
  
  UPDATE t_IncomingTransactions SET VoucherCode = '11'   
  WHERE FileName = @FileName AND MsgID = @MsgID AND TrxID = @TrxID AND TrxType = 'ID'  
  
  UPDATE t_IncomingTransactions SET InstrumentTypeID = (CASE WHEN TrxType = 'ID' THEN 'C' ELSE 'V' END)  
  WHERE isNull(InstrumentTypeID,'') = '' AND FileName = @FileName AND MsgID = @MsgID AND TrxID = @TrxID  
  
  UPDATE t_IncomingTransactions SET TrxBranchID = OurBranchID WHERE FileName = @FileName AND MsgID = @MsgID AND TrxID = @TrxID  
  
  UPDATE t_IncomingTransactions SET TrxBranchID = @OurRealHQBrnID WHERE TrxType = 'IC' AND FileName = @FileName AND MsgID = @MsgID AND TrxID = @TrxID  
    
  UPDATE t_IncomingTransactions SET BranchID = t_Branch.BranchID  
  FROM t_Branch WHERE t_IncomingTransactions.BankID = t_Branch.BankID  
  AND FileName = @FileName AND MsgID = @MsgID AND TrxID = @TrxID AND isNull(t_IncomingTransactions.BranchID,'') = ''  
  
  UPDATE t_IncomingTransactions SET DRN ='T', Description ='IC - To : Acc('+ AccountID +') - Frm: ' + DrawerOrPayee  
  WHERE TrxType ='IC' AND ReturnCode = '00' AND FileName = @FileName AND MsgID = @MsgID AND TrxID = @TrxID  
    
  UPDATE t_IncomingTransactions SET DRN ='T', Description ='UnPaid IC From ' + DrawerOrPayee + ' rsn ' + DBO.f_CRB_ReturnCodeDescriptions('ReturnCodeID',ReturnCode,'T')    
  WHERE TrxType ='IC' AND ReturnCode <> '00' AND FileName = @FileName AND MsgID = @MsgID AND TrxID = @TrxID  
    
  UPDATE t_IncomingTransactions SET DRN ='J', Description ='ID To : Acc('+ AccountID +') ChqID : ' + CAST(ChequeID AS Varchar)    
  WHERE TrxType ='ID'  AND VoucherCode <>'40' AND VoucherCode NOT IN ('60')  AND ReturnCode = '00' AND FileName = @FileName AND MsgID = @MsgID AND TrxID = @TrxID  
  
  UPDATE t_IncomingTransactions SET DRN ='E', Description ='ID To : Acc('+ AccountID +') ChqID : ' + CAST(ChequeID AS Varchar)    
  WHERE TrxType ='ID'  AND VoucherCode <>'40' AND VoucherCode IN ('60')  AND ReturnCode = '00' AND FileName = @FileName AND MsgID = @MsgID AND TrxID = @TrxID  
    
  UPDATE t_IncomingTransactions SET DRN ='T', Description ='DD - To : Acc('+ AccountID +') - Frm: ' + DrawerOrPayee     
  WHERE TrxType ='ID'  AND VoucherCode ='40' AND ReturnCode = '00' AND FileName = @FileName AND MsgID = @MsgID AND TrxID = @TrxID  
  
  UPDATE t_IncomingTransactions SET DRN ='J', Description ='UnPaid ID To : Acc('+ AccountID +') ChqID : ' + CAST(ChequeID AS Varchar) + ' rsn ' + DBO.f_CRB_ReturnCodeDescriptions('ReturnCodeID',ReturnCode,'T')      
  WHERE TrxType ='ID'  AND VoucherCode NOT IN ('40','60') AND ReturnCode <> '00' AND FileName = @FileName AND MsgID = @MsgID AND TrxID = @TrxID  
    
  UPDATE t_IncomingTransactions SET DRN ='E', Description ='UnPaid ID To : Acc('+ AccountID +') ChqID : ' + CAST(ChequeID AS Varchar) + ' rsn ' + DBO.f_CRB_ReturnCodeDescriptions('ReturnCodeID',ReturnCode,'T')    
  WHERE TrxType ='ID'  AND VoucherCode IN ('60') AND ReturnCode <> '00' AND FileName = @FileName AND MsgID = @MsgID AND TrxID = @TrxID  
  
  UPDATE t_IncomingTransactions SET DRN ='J', Description ='UnPaid IC From ' + DrawerOrPayee + ' rsn '  + DBO.f_CRB_ReturnCodeDescriptions('ReturnCodeID',ReturnCode,'T')    
  WHERE TrxType ='ID' AND ReturnCode <> '00' AND FileName = @FileName AND MsgID = @MsgID AND TrxID = @TrxID  
    
  UPDATE t_IncomingTransactions SET FullName =  
  dbo.f_GetGLAccountName(dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN 'TZS'  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD' END), 'ACP_CLR_SUSP_AC')),  
          ProductID = 'GL' , AccountType ='G'  
  WHERE TrxType ='IC'  AND FileName = @FileName AND MsgID = @MsgID AND TrxID = @TrxID AND AccountID = '11700100'  
  DECLARE @DateDiff int, @ReqDate Date, @ReqDateStr Varchar(100), @ReqdCollDate DateTime, @StrTemp Varchar(10)  
  SELECT @ReqdColltnDt = LEFT(@ReqdColltnDt,10)  
  SELECT @StrTemp = @ReqdColltnDt  
  BEGIN TRY   
   SELECT @ReqdCollDate =  CONVERT(datetime, @ReqdColltnDt, 104)   
  END TRY   
  BEGIN CATCH  
   BEGIN TRY   
    SELECT @ReqdCollDate =  CONVERT(datetime, @ReqdColltnDt, 102)   
   END TRY   
   BEGIN CATCH  
    BEGIN TRY   
     SELECT @ReqdCollDate =  CONVERT(datetime, @ReqdColltnDt, 103)   
    END TRY   
    BEGIN CATCH  
     SELECT @ReqdCollDate = NULL  
    END CATCH  
   END CATCH  
  END CATCH  
  
    
  SELECT @ReqDate = CONVERT(DATE,@ReqdCollDate)  
    
  SELECT @DateDiff = DateDiff(day, CONVERT(DATE, GetDate()), @ReqDate)  
  
  IF @DateDiff > 10  
  BEGIN   
   BEGIN TRY   
  
    SELECT @ReqdCollDate = ''  
    SELECT @ReqdCollDate =  CONVERT(datetime, @ReqDateStr, 102)   
   END TRY   
   BEGIN CATCH  
    SELECT @ReqdCollDate = NULL  
   END CATCH  
  END   
  
  --SELECT @ReqdColltnDt, @DateDiff  
  
  BEGIN TRY   
   SELECT @DtOfSgntr =  CONVERT(datetime, @DtOfSgntr, 104)   
  END TRY   
  BEGIN CATCH  
   SELECT @DtOfSgntr = NULL  
  END CATCH  
    
    
    
  IF IsNull(@DtOfSgntr,'')=''  
  BEGIN  
   SET @DtOfSgntr = Null  
  END  
  DELETE FROM  t_IncomingClrTrxExtraDetails WHERE TrxID = @TrxID AND OrgnlMsgId = @MsgID AND OrgnlEndToEnd = @OrgnlEndToEnd  
  INSERT INTO t_IncomingClrTrxExtraDetails(TrxID, TrxDate, ColumnID, TrxRowID, OrgnlInstrId,  
   OrgnlMsgId, IntrBkSttlmAmt, OrgnlEndToEnd, MndtId, DtOfSgntr, ReqdColltnDt, Frqcy, UstrdColD,  
   UstrdChqdt, UstrdBWF, UstrdBWR, UstrdGS,UstrdUV, UstrdMicr, DAdrLine, DTwnNm, DCtry,   
   DNm, DPhneNb, DMobNb, DEmailAdr, DOthr, DbtrAcct, CAdrLine, CTwnNm, CCtry, CNm,  
   CPhneNb, CMobNb, CEmailAdr, COthr, PymType, CdtrAcct, DCNm, CCNm,FnlColltnDt,SourceBIC,Ustrd,LclInstrm,CtgyPurp,SvcLvl,ReqdColltnDate)  
  Values(@TrxID,dbo.f_GetWorkingDate(@OurBranchID) ,'','',@OrgnlInstrID,@MsgID,@Amount,@OrgnlEndToEnd,@MndtId,@DtOfSgntr,  
   @ReqdColltnDt,@Frqcy,@UstrdColD,@ChequeDigit,@UstrdBWF,@UstrdBWR,@UstrdGS,@UstrdUV,@UstrdMicr,@DAdrLine,  
   @DTwnNm,@DCtry,@DNm,@DPhneNb,@DMobNb,@DEmailAdr,@DOthr,@DbtrAcct,@CAdrLine,@CTwnNm,@CCtry,@CNm,@CPhneNb,  
   @CMobNb,@CEmailAdr,@COthr,@PymType,@CdtrAcct,@DCNm, @CCNm,@FnlColltnDt, @SourceBIC,@RemittanceInfo,@LclInstrm,@CtgyPurp,@SvcLvl,@ReqdCollDate)  
     
      
  
     
  
  
  
    DECLARE    @ReturnCodeID  CHAR(2),  
      @TrxBranchID  BranchID,  
      @OperatorID   Varchar(50),  
      @InTrxTypeID  Char(2),  
      @OutTrxDescription Description,  
      @InTrxDescription Description,  
      @OutTrxTypeID  Char(2),  
      @outModuleID  Varchar(6),  
      @InModuleID   Varchar(6)  
  
   SELECT @OurBranchID = @OurBranchID, @AccountID = AccountID, @ColumnID = ColumnID, @Amount = Amount,  
     @ChequeID = ChequeID, @ReturnCodeID = ReturnCode, @TrxType = TrxType , @TrxBranchID =TrxBranchID  
   FROM t_IncomingTransactions   
   WHERE FileName = @FileName AND MsgID = @MsgID AND TrxID = @TrxID   
   SELECT @Auto_unpay = 0  
   IF isNull(@ReturnCodeID,'00') IN ('00','17')   
   BEGIN  
    IF @TrxType = 'IC'  
     BEGIN   
      SELECT @OutTrxTypeID = 'OD'  
      SELECT @outModuleID  = '3070'  
      SELECT @InModuleID  = '3040'  
     END  
    ELSE  
     BEGIN  
      SELECT @OutTrxTypeID = 'OC'  
      SELECT @outModuleID  = '3060'  
      SELECT @InModuleID  = '3050'  
     END  
    --Stop Payment  
    IF dbo.f_GetStoppedAccChequeIDStatus(@OurBranchID,@AccountID,@ChequeID) = 'S'      
    BEGIN  
     SELECT @ReturnCodeID  = '75' --'CH23 - Payment stopped by Drawer  
     SELECT @OutTrxDescription = 'Auto-Unpaid reason Payment stopped by Drawer Acc(' + @AccountID + ')'  
     SELECT @InTrxDescription = 'Incoming Inward'  
     SELECT @OutTrxTypeID  = 'OC'  
     SELECT @InTrxTypeID   = 'ID'  
     SELECT @outModuleID   = '3060'  
     SELECT @InModuleID   = '3050'  
     SELECT @Auto_unpay = 1  
    END  
  
    --Dormat Account  
    IF dbo.f_GetAccStatus(@OurBranchID,@AccountID) = 'AD'   AND  @Auto_unpay = 0      
    BEGIN  
     SELECT @ReturnCodeID  = '48' --'AG01 - Transaction Forbidden'  
     SELECT @OutTrxDescription = 'Auto-Unpaid reason Dormant Account (' + @AccountID + ')'  
     SELECT @InTrxDescription = 'Incoming Inward'  
     SELECT @OutTrxTypeID  =  @OutTrxTypeID  
     SELECT @InTrxTypeID   =  @TrxType  
     SELECT @outModuleID   =  @outModuleID  
     SELECT @InModuleID   =  @InModuleID  
     SELECT @Auto_unpay = 1  
    END  
      
    --Blocked Account  
    IF dbo.f_GetAccStatus(@OurBranchID,@AccountID) = 'AB'  AND  @Auto_unpay = 0     
    BEGIN  
     SELECT @ReturnCodeID  = '47' --'AC06 - Blocked Account'  
     SELECT @OutTrxDescription = 'Auto-Unpaid reason Blocked Account (' + @AccountID + ')'  
     SELECT @InTrxDescription = 'Incoming Inward'  
     SELECT @OutTrxTypeID  =  @OutTrxTypeID  
     SELECT @InTrxTypeID   =  @TrxType  
     SELECT @outModuleID   =  @outModuleID  
     SELECT @InModuleID   =  @InModuleID  
     SELECT @Auto_unpay = 1  
    END  
  
    ---Closed Account  
    IF dbo.f_GetAccStatus(@OurBranchID,@AccountID) = 'AC' AND  @Auto_unpay = 0     
    BEGIN  
     SELECT @ReturnCodeID  = '46' --AC04 - Closed Account Number  
     SELECT @OutTrxDescription = 'Auto-Unpaid reason Closed Account (' + @AccountID + ')'  
     SELECT @InTrxDescription = 'Incoming Inward'  
     SELECT @OutTrxTypeID  =  @OutTrxTypeID  
     SELECT @InTrxTypeID   =  @TrxType  
     SELECT @outModuleID   =  @outModuleID  
     SELECT @InModuleID   =  @InModuleID  
     SELECT @Auto_unpay = 1  
    END  
  
  
      
    ---Invalid Account  
    IF dbo.f_GetAccStatus(@OurBranchID,@AccountID) = 'INV' AND  @Auto_unpay = 0     
    BEGIN  
     SELECT @ReturnCodeID  = '45' --AC01 - Incorrect Account Number  
     SELECT @OutTrxDescription = 'Auto-Unpaid reason Invalid Account ID (' + @AccountID + ')'  
     SELECT @InTrxDescription = 'Incoming Inward'  
     SELECT @OutTrxTypeID  =  @OutTrxTypeID  
     SELECT @InTrxTypeID   =  @TrxType  
     SELECT @outModuleID   =  @outModuleID  
     SELECT @InModuleID   =  @InModuleID  
     SELECT @Auto_unpay = 1  
    END  
  
    --Insufficient Fund  
    IF ABS(@Amount) > dbo.f_GetAvailableBalance(@OurBranchID,@AccountID)  AND  @Auto_unpay = 0  AND @TrxType ='ID'   
    BEGIN  
     SELECT @ReturnCodeID  = '50' --AM04 - Insufficient Funds (Refer to Drawer)  
     SELECT @OutTrxDescription = 'Auto-Unpaid reason Insufficient Funds (Refer to Drawer) Acc(' + @AccountID + ')'  
     SELECT @InTrxDescription = 'Incoming Inward'  
     SELECT @OutTrxTypeID  =  @OutTrxTypeID  
     SELECT @InTrxTypeID   =  @TrxType  
     SELECT @outModuleID   =  @outModuleID  
     SELECT @InModuleID   =  @InModuleID   
     SELECT @Auto_unpay = 1     
    END  
      
      
      
    IF @Auto_unpay = 1  
    BEGIN  
     EXEC p_AutoUnpayIncomingTxns  
       @ColumnID   = @ColumnID,  
       @ReturnCodeID  = @ReturnCodeID,  
       @TrxBranchID  = @TrxBranchID,  
       @OperatorID   = 'ClrSys',  
       @outModuleID  = @outModuleID,  
       @InModuleID   = @InModuleID,  
       @OutTrxTypeID  = @OutTrxTypeID,  
       @InTrxTypeID  = @InTrxTypeID,  
       @OutTrxDescription = @OutTrxDescription,  
       @InTrxDescription = @InTrxDescription  
      
     UPDATE t_IncomingTransactions       
      SET [Description] = @OutTrxDescription,  
       [Status]  = 1,      
       AccountType  = 'G',       
       ProductID  = 'GL',      
       AccountID  = dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,CurrencyID, 'ACP_CLR_SUSP_AC'),   
       FullName  = dbo.f_GetGLAccountName(dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,CurrencyID, 'ACP_CLR_SUSP_AC')),       
       MainGLID  = dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,CurrencyID, 'ACP_CLR_SUSP_AC'),   
       isProcessed  = 1,  
       AuthorizedBy = 'ClrSys',  
       AuthorizedOn = GetDate()  
     WHERE FileName   = @FileName AND MsgID = @MsgID AND TrxID = @TrxID  AND ISNULL(IsProcessed,0)=0   
    END  
   END  
   --RETURN  
   EXEC p_DoInwardsFileTrxValidationsTZ   
 END  
ELSE  
 BEGIN  
 print 'Hapa'  
  RETURN  
 END  
  
  
SET NOCOUNT OFF  
  