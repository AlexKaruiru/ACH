CREATE  PROCEDURE [dbo].[p_AddInwardsFromTextFileTZ]    
 (    
  @FileName   varchar(100),     
  @Data    varchar(255),    
  @ReturnCode   varchar(4),    
  @VoucherCode  varchar(4),    
  @Amount    Numeric(18,2),    
  @BankID    varchar(30),    
  @BranchID   varchar(30),    
  @OurBankID   varchar(30),    
  @OurBranchID  varchar(5),    
  @ChequeID   varchar(11),    
  @ChequeDigit  varchar(2),    
  @Date    datetime,    
  @AccountID   varchar(15),    
  @ImageUniqueID  VARCHAR(20),    
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
  @TheirAccountID     varchar(15)='',    
  @Drawer    Varchar(250)='',    
  @MsgID    Varchar(250)='',    
  @EFTID    Varchar(50)='',    
  @ColumnID   Numeric(18,0),    
  @isMdv    bit =0,    
  @TrxType   Varchar(4)='',    
  @TrxID    Varchar(200)    
 )    
AS     
DECLARE @ImageID  BigInt,    
  @OurRealHQBrnID BranchID    
SELECT @Date = dbo.f_GetWorkingDate(@OurBranchID)    
SELECT @OurRealHQBrnID = OurBranchID FROM t_SystemBranchSetting WHERE IsHeadOffice = 1    
IF @OurBranchID = '099'    
BEGIN    
 SELECT @OurBranchID = @OurRealHQBrnID    
END    
IF NOT EXISTS(SELECT 1 FROM t_SystemBranchSetting WHERE OurBranchID = @OurBranchID)    
BEGIN    
 SELECT @OurBranchID = @OurRealHQBrnID    
END    
    
IF @TrxType = 'ID' AND @ReturnCode = '00' AND @AccountID != '21100100' AND @VoucherCode <> '60'    
BEGIN    
 SET @AccountID = @OurBranchID + '1' + @AccountID    
END    
    
IF @TrxType = 'ID' AND @VoucherCode = '60'    
BEGIN    
 DECLARE @StartDat2 BigInt,    
   @StartDat BigInt,    
   @Akaunti Varchar(30)    
    
    
 SELECT @StartDat = dbo.INSTR(@Data, '/', 2, 2), @StartDat2 = dbo.INSTR(@Data, '/', 2, 3)     
 SELECT @Akaunti = SUBSTRING(@Data,@StartDat+1,(@StartDat2-@StartDat)-1)    
 SELECT @Akaunti    
 SET @AccountID = @OurBranchID + '5' + @Akaunti    
END    
IF EXISTS (SELECT 1 FROM t_TrxInwards  WHERE REFERENCE = @MsgID AND TrxID = @TrxID AND isNull(Post,0) = 0)    
BEGIN    
 RETURN    
END    
   
  
    
IF NOT EXISTS( SELECT 1 FROM t_TrxInwards  WHERE [Date] = @Date AND TrxID = @TrxID)    
 BEGIN    
  IF @ReturnCode <> '00'    
  BEGIN    
   DECLARE @OutwardSerialID Numeric(18,0)    
   SELECT top 1 @ImageID = ImageID, @BankID = BankID,  @BranchID = BranchID, @ChequeID = ChequeID,    
        @VoucherCode = VoucherCode, @AccountID = AccountID, @OurBranchID = OurBranchID    
   FROM t_TrxClearing      
   WHERE Reference = @MsgID AND ChequeID = @ChequeID     
   --UNION ALL (changes done by Gabriel)    
   SELECT Top 1 @ImageID = ImageID, @BankID = BankID,  @BranchID = BranchID, @ChequeID = ChequeID,    
                 @VoucherCode = VoucherCode, @AccountID = AccountID, @OurBranchID = OurBranchID    
   FROM t_AccountTrxClearing      
   WHERE Reference = @MsgID AND ChequeID = @ChequeID     
    
  SELECT @FrontTFImage = TFImage, @FrontImage = JFImage, @BackImage = JRImage, @UVImage = UVImage     
  FROM BRNET_IMAGEServer.dbo.t_ChequeImages    
  WHERE ImageID = @ImageID    
  ORDER BY Date DESC    
    
  IF IsNull(Cast(@FrontTFImage AS VARCHAR),'') = ''    
  BEGIN    
   SELECT Top 1 @FrontTFImage = TFImage, @FrontImage = JFImage, @BackImage = JRImage, @UVImage = UVImage     
   FROM BRNET_IMAGEServer.dbo.t_ChequeImages    
   WHERE ChequeID = @ChequeID and BankID = @BankID     
   ORDER BY Date DESC    
    
   --SELECT Top 1 @AccountID = AccountID, @OurBranchID = OurBranchID     
   --FROM t_AccountTrx     
   --WHERE AccountTypeID ='C'  AND Amount = @Amount AND ChequeID = @ChequeID and BankID = @BankID AND BranchID = RIGHT(@BranchID,2)     
   --ORDER BY TrxDate DESC    
    
   IF IsNull(Cast(@FrontTFImage AS VARCHAR),'') = ''    
   BEGIN        SELECT Top 1 @FrontTFImage = TFImage, @FrontImage = JFImage, @BackImage = JRImage, @UVImage = UVImage     
    FROM BRNET_IMAGEServer.dbo.t_ChequeImages    
    WHERE ChequeID = @ChequeID and BankID = @BankID     
    ORDER BY Date DESC    
    
    --SELECT Top 1 @AccountID = AccountID, @OurBranchID = OurBranchID     
    --FROM t_AccountTrx     
    --WHERE AccountTypeID ='C'  AND Amount = @Amount AND ChequeID = @ChequeID and BankID = @BankID     
    --ORDER BY TrxDate DESC    
   END    
 END    
 END    
    
 UPDATE t_TrxInwards SET ModuleID = CASE WHEN TrxType ='ID' THEN '3050' ELSE '3040' END    
 WHERE isNull(ModuleID,0) = 0 AND TrxType IN ('ID','IC')     
    
 DELETE FROM t_systemrecordlocks WHERE LockModuleID='1300' AND ModuleID In('3050','3040','3060','3070')    
    
 UPDATE t_TrxInwards     
 SET  t_TrxInwards.TrxDescriptionID = CASE WHEN TrxType='ID' THEN '004' ELSE '003' END     
 WHERE TrxType IN ('ID', 'IC')    
    
    
 UPDATE t_TrxInwards    
 SET  t_TrxInwards.OurBankID = (SELECT BankID FROM t_SystemBankSetting)    
 IF isNull(@ImageID,'') = ''    
 BEGIN    
  EXEC p_GetUniqueClearingImageID @ImageID OUTPUT       
 END     
    
 INSERT INTO t_TrxInwards     
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
   Reference,    
   FrontBWImage,    
   FrontGrayScaleImage,    
   RearImage,    
   UVImage,    
   TrxBranchID,    
   TrxType,    
   TrxID,    
   ImageID    
  )     
 VALUES    
  (     
  --SELECT    
   @Date,     
   @FileName,     
   @Data,    
   @ReturnCode,    
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
   @TrxType,    
   @TrxID,    
   @ImageID    
  )    
  --return    
   UPDATE t_TrxInwards SET OurBankID = (SELECT BankID FROM t_SystemBranchSetting WHERE IsHeadOffice = 1)    
   UPDATE t_TrxInwards SET TrxDescriptionID = (CASE WHEN TrxType = 'ID' THEN '004' ELSE '003' END) WHERE isNull(TrxDescriptionID,'') = ''    
 END    
ELSE    
 BEGIN    
  RETURN    
 END    
    
    
    
    
    
    
    
    
    
    
    
    
    
  
  
  
  
  