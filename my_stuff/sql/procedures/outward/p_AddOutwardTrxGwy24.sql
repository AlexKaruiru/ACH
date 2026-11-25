CREATE    PROCEDURE [dbo].[p_AddOutwardTrxGwy24]          
(     
 @TrxBranchID     dbo.BranchID,          
 @TrxBatchID     VarChar(8) OUTPUT,          
 @TrxBatchSLNo     SmallInt,          
 @SerialID      Int =0 OUTPUT,          
 @OurBranchID     dbo.BranchID,          
 @AccountTypeID     Char(1),          
 @AccountID     dbo.AccountID,          
 @ProductID     dbo.ProductID,    
 @ModuleID      SmallInt,          
 @TrxTypeID     Char(2),          
 @TrxDate      SmallDateTime,          
 @ValueDate     SmallDateTime,          
 @Amount      dbo.Amount,          
 @LocalAmount     dbo.Amount,          
 @TrxCurrencyID     dbo.CurrencyID,          
 @TrxAmount     dbo.Amount,          
 @ExchangeRate     dbo.CurrencyRate,          
 @MeanRate      dbo.CurrencyRate,          
 @Profit      dbo.Amount = 0,    
 @InstrumentTypeID    Char(1)='V',          
 @ChequeID      Int=0,          
 @ChequeDate     SmallDateTime = Null,          
 @ReferenceNo     nVarChar(15) = Null,          
 @Remarks        nVarChar(255),--dbo.Remarks = Null,    
 @TrxDescriptionID    nVarChar(6),          
 @TrxDescription    Description,          
 @MainGLID      dbo.AccountID,          
 @ContraGLID     dbo.AccountID = Null,          
 @TrxFlagID     dbo.SystemSubID = '',          
 @ImageID      bigInt = 0,          
 @TrxPrinted     TinyInt = 0,          
 @CreatedBy     dbo.OperatorID,          
 @NewRecord     TinyInt,          
 @IsForfeit     bit=0,          
 @ChequeDigit     Char(1),          
 @VoucherCode     Char(2),          
 @ReturnCodeID     Char(4),          
 @Commission     dbo.Amount,          
 @TheirCommission    dbo.Amount,    
 @BankID      dbo.BankID,          
 @BranchID      dbo.BranchID,          
 @DrawerOrPayeeAccountID dbo.AccountID,          
 @DrawerOrPayee     dbo.Names,          
 @VATPINNo      nVarChar(12),          
 @VATPAYType     nVarChar(10),          
 @VATSerialNo     nVarChar(20),          
 @VATPAYEMonth     nVarChar(6),          
 @VATPAYECommission    dbo.Amount,          
 @ErrorNo      VarChar(50)='' OUTPUT       
          
)    
--WITH ENCRYPTION          
AS     
BEGIN      
    
-- RETURN--EVE    
        
 SET NOCOUNT ON          
 DECLARE     
   @TrxRowID      BigInt,          
   @strString      Varchar(Max),          
   @PValueDate      SmallDateTime,          
   @IsCreditTrx     Bit,          
   @IsHOPostingRequired    Bit,          
   @HOBranchID      dbo.BranchID,          
   @HOTrxDate      SmallDateTime,          
   @DestinationTrxDate    SmallDateTime,          
   @DestinationIBGLID    dbo.AccountID,             
   @SourceIBGLID     dbo.AccountID,          
   @HOIBCreditGLID     dbo.AccountID,          
   @HOIBDebitGLID     dbo.AccountID,          
   @DescriptionID     nVarChar(6),          
   @NewRowID      BigInt,          
   @TempAccountID     dbo.AccountID,          
   @TempTrxTypeID     Char(2),          
   @TempTrxDescID     nVarChar(6),          
   @TempTrxDesc     Description,          
   @UniqueImageID     BigInt,          
   @OriginalAmount     dbo.Amount,          
   @OurBrankID      dbo.BankID,          
   @ValueDatedd     DateTime,          
   @OrigCode      varchar(4),          
   @OrigRef      varchar(25),          
   @Policy1      varchar(25),          
   @Policy2      varchar(25),          
   @ColumnID      varchar(50),    
   @OldColumnID     varchar(50),      
   @ErrorCode      VarChar(50)='',      
   @ODTrxRowID      BigInt,    
   @OldVATSerialID     Varchar(20) ,     
   @DeleteTrxRowID     BigInt,          
   @DeleteChequeDate    Datetime,          
   @DeleteBranchID     Varchar(5),          
   @DeleteAccountID     Varchar(25),          
   @DeleteChequeID     Int,          
   @ValueCapingAmt     dbo.Amount,      
   @Day       nVarchar(3),        
   @Time       nVarchar(8),        
   @AutoGenerateImgID    bIGInt=0 ,    
   @OrigBankID    dbo.BankID,    
   @OrigBranchID   dbo.BranchID       
      
    
  SELECT @OrigBankID = @BankID, @OrigBranchID = @BranchID     
  
    
   IF @TrxTypeID = 'OC'     
    BEGIN    
   SELECT @TrxDescriptionID = '006'    
 END    
   ELSE    
    BEGIN    
   SELECT @TrxDescriptionID = '104'    
    END    
            
    
 EXEC p_GetUniqueClearingImageID @AutoGenerateImgID OUTPUT       
     
 IF LTRIM(RTRIM(@ReturnCodeID)) = '0'    
 BEGIN    
 SELECT @ReturnCodeID = '0' + LTRIM(RTRIM(@ReturnCodeID))    
 END    
    
IF EXISTS (SELECT 1 FROM t_TrxClearing WHERE ChequeID = @ChequeID AND BankID = @BankID AND DrawerOrPayeeAccountID = @DrawerOrPayeeAccountID       
AND BranchID = @BranchID AND TrxType = 'OC' AND isNull(IsDeleted,0) = 0  AND Abs(Amount) = Abs(@Amount))    
BEGIN          
 RAISERROR(N'BREXDB999955',16,1)       
 RETURN          
END    
    
IF @AccountTypeID = 'G' AND @AccountID NOT IN (dbo.f_GetCurrencyBranchGLAccountID(@OurBranchID, @TrxCurrencyID, 'ACP_CLR_SUSP_AC_EFT'),  
dbo.f_GetCurrencyBranchGLAccountID(@OurBranchID, @TrxCurrencyID, 'ACP_CLR_SUSP_AC_CHQ'))    
BEGIN          
 RAISERROR(N'BREXDB300010',16,1)       
 RETURN          
END    
    
    
    
 --Validate Posting    
-- Account    
IF  IsNumeric(@AccountID)=0    
 BEGIN    
 RAISERROR(N'BREXDB543372',16,1)       
 RETURN      
 END    
    
 --SELECT @AccountID    
 --IF  RIGHT('0000000000'+ RTRIM(@AccountID),10) = '0000000000'    
 --BEGIN    
 --RAISERROR(N'BREXDB543372',16,1)       
 --RETURN      
 --END    
    
 -- BranchID    
    
 --select @BankID    
  
 SELECT @BankID = RIGHT('51'+ RTRIM(RIGHT(@BankID,2)),2)    
 IF NOT EXISTS(SELECT 1 FROM t_Branch WHERE BankID = @BankID)    
 BEGIN    
 RAISERROR(N'BREXDB543372',16,1)       
 RETURN      
 END    
    
  -- BranchID    
    
 IF @ChequeID <> 0 AND @VoucherCode NOT IN ('01','11','13','14','60','61','62','40','03','17') AND @ReturnCodeID NOT IN ('00','17')    
 BEGIN    
 IF EXISTS ( SELECT VoucherCode FROM t_IncomingTransactions WHERE ColumnID = TRY_PARSE(@ReferenceNo AS bigint))    
  BEGIN    
   SELECT @VoucherCode = VoucherCode, @BankID = BankID, @BranchID = BranchID     
   FROM t_IncomingTransactions WHERE ColumnID = TRY_PARSE(@ReferenceNo AS bigint)    
  END    
 ELSE    
  BEGIN    
   RAISERROR(N'BREXDB543372',16,1)       
   RETURN     
  END     
 END     
      
IF @TrxDate <> dbo.f_GetWorkingDate(@OurBranchID)          
 BEGIN        
  RAISERROR('BREXDB300032',16,1)          
  RETURN          
 END           
  
--Select @BankID ,dbo.f_GetBankID(@OurBranchID),   
 --IF @BankID !=  dbo.f_GetBankID(@OurBranchID)       
 --BEGIN    
 -- RAISERROR('BREXDB3337474',16,1)          
 -- RETURN     
 --END    
    
    
IF EXISTS(SELECT 1 FROM t_TrxClearing  (NOLOCK)          
  WHERE ColumnID = try_parse(@ReferenceNo as bigint) AND TrxType IN ('OC','OD')  And IsNull(IsDeleted,0)=0 AND @ReturnCodeID NOT IN ('00','17'))        
BEGIN          
 RAISERROR(N'BREXDB999955',16,1)      
 RETURN          
END      
    
    
IF @OldVATSerialID IN ('7777MFI','7778MFI')    
BEGIN    
 SET @OldColumnID = @ImageID    
END        
DECLARE @ServerTime time    
SELECT @ServerTime = CONVERT(TIME, GETDATE())    
 IF @TrxTypeID = 'OC' AND @ReturnCodeID IN ('00','17')    
 BEGIN    
 IF @ServerTime > CONVERT(TIME,'13:00:00:000')    
  BEGIN    
   SET @ValueDate = dbo.fn_GetValueDateTPlus(@OurBranchID, @TrxCurrencyID, @BankID, @BranchID,1)    
  END    
 ELSE    
  BEGIN    
   SELECT @ValueDate = dbo.fn_GetValueDate(@OurBranchID, @TrxCurrencyID, @BankID, @BranchID)    
  END    
 END    
     
 IF (SELECT Count(AccountID) from t_AccountSpecialCondition (NOLOCK) Where OurBranchID = @OurBranchID And AccountID = @AccountID And SpecialConditionID = '108') <> 0    
 BEGIN    
 SET @ValueDate = dbo.fn_GetValueDateAccount (@OurBranchID,@AccountID)    
 END    
    
 IF Len(@Remarks) > 20   
 BEGIN    
 SET @Remarks =@Remarks --LEFT(@Remarks,25)    
 --RAISERROR('BREXDB543374',16,1)          
 --RETURN      
 END    
    select 1, @Remarks  
     
 IF EXISTS(SELECT 1 FROM T_AccountCustomer  (NOLOCK) WHERE OurBranchID = @OurBranchID        
     AND AccountID = @AccountID AND AccountStatusID In ('AB'))        
     BEGIN        
      RAISERROR('BREXDB130002',16,1)          
   RETURN     
     END        
            
IF EXISTS(SELECT 1 FROM T_AccountCustomer  (NOLOCK) WHERE OurBranchID = @OurBranchID        
     AND AccountID = @AccountID AND AccountStatusID In ('AC'))        
     BEGIN        
      RAISERROR('BREXDB130003',16,1)          
   RETURN     
     END        
            
  IF EXISTS(SELECT 1 FROM T_AccountCustomer  (NOLOCK) WHERE OurBranchID = @OurBranchID        
     AND AccountID = @AccountID AND AccountStatusID In ('AD'))        
     BEGIN        
      RAISERROR('BREXDB300003',16,1)          
   RETURN     
     END        
             
 SELECT @OldVATSerialID = @VATSerialNo    
 IF @TrxTypeID = 'OC' AND @ReturnCodeID IN ('00') AND @OldVATSerialID NOT IN ('7777MFI','7778MFI')    
 BEGIN    
  IF NOT EXISTS(SELECT 1          
  FROM t_BRChequeTruncation   (NOLOCK)         
  WHERE OperatorID = @CreatedBy     
    AND AccountID = @AccountID     
    AND BranchID = @BranchID           
    AND BankID = @BankID     
    AND TheirAcc = @DrawerOrPayeeAccountID     
    AND TFImage IS NOT null     
    AND  FrontImage IS NOT null    
    AND  BackImage IS NOT null)    
    BEGIN    
      RAISERROR('BREXDB543372',16,1)          
      RETURN          
    END    
 END    
    
    
    
    
 IF @TrxBranchID = @OurBranchID AND @TrxTypeID = 'OC'          
  SET @ModuleID = '3060'          
 ELSE IF @TrxBranchID <> @OurBranchID AND @TrxTypeID = 'TC'          
  SET @ModuleID = '3061'          
       
 --SET @TrxDescriptionID = CASE WHEN @VoucherCode IN ('39','40') THEN '089' ELSE @TrxDescriptionID END    
      
 IF @VoucherCode = '40' AND @ReturnCodeID NOT IN ('00','17')      
 BEGIN      
  SET @ModuleID = '3060'       
  SET @TrxTypeID='OC'     
  SET @Amount = @Amount * -1      
  SET @LocalAmount = @LocalAmount * -1       
  SET @TrxAmount = @TrxAmount * -1      
 END     
     
 IF @ReturnCodeID NOT IN ('00','17') AND @TrxTypeID = 'OC'    
 BEGIN      
   SET @TrxDescriptionID = CASE WHEN @ReturnCodeID IN ('63') THEN '086'      
    WHEN @ReturnCodeID = '62' THEN '087'    
     WHEN @ReturnCodeID IN ('33','37','38','55','84')  THEN '091'    
    WHEN @ReturnCodeID IN ('31','32','40','53','57','66') THEN '088'    
         ELSE '005' END    
 END     
    
 IF @ReturnCodeID NOT IN ('00','17') AND @TrxTypeID = 'OD' AND @VoucherCode <>'40'    
 BEGIN      
   SET @TrxDescriptionID = '090'    
 END     
        
 --- Valuecaping          
 SELECT @ValueCapingAmt = ISNULL(ValueCappingAmount,999999) FROM t_Currency  (NOLOCK) WHERE CurrencyID = @TrxCurrencyID          
 IF @Amount >= @ValueCapingAmt          
 BEGIN          
  RAISERROR('BREXDB300051',16,1)          
  RETURN          
 END           
     
 ----- Do not allow to post outward if file is generated          
 --IF (SELECT ISNULL(ClgFileGeneratedDate,EODDate) FROM t_SystemBranchStatus (NOLOCK) WHERE OurBranchID = @OurBranchID) >= dbo.f_GetWorkingDate(@OurBranchID)          
 --BEGIN          
 -- RAISERROR('BREXDB9999115',16,1)          
 -- RETURN          
 --END     
 IF @TrxTypeID  = 'OD'    
  BEGIN    
   SELECT @ContraGLID = dbo.f_GetCurrencyBranchGLAccountID(@OurBranchID, @TrxCurrencyID, 'CUR_CLR_AC')--_EFT')    
  END    
 ELSE    
  BEGIN    
   SELECT @ContraGLID = dbo.f_GetCurrencyBranchGLAccountID(@OurBranchID, @TrxCurrencyID, 'CUR_CLR_AC')--_CHQ')    
  END    
          
   SET @UniqueImageID = 0          
   SET @TrxDescriptionID = CASE WHEN @TrxTypeID = 'OC' AND @ReturnCodeID IN ('00','17') THEN '005' ELSE @TrxDescriptionID END          
            
   IF @ReturnCodeID In ('02','03')          
   BEGIN          
    SET @ValueDate  = @TrxDate          
    SET @OriginalAmount = @Amount + @TrxAmount          
    SET @Amount   = @TrxAmount          
   END          
   SET @IsCreditTrx = CASE WHEN @TrxTypeID = 'OC' THEN 1 ELSE 0 END          
   SET @OurBrankID = dbo.f_GetBankID(@OurBranchID)          
   IF @CreatedBy <> 'SYS'      
   BEGIN      
 SET @TrxFlagID = 'U'          
   END         
       
 DECLARE @OurBankID      BankID,     
    --@HOBranchID      BranchID,    
    @HOBranchLocalBranchCurrency CurrencyID    
      
 SET @OurBankID  = (SELECT BankID FROM t_SystemBankSetting)    
 SET @HOBranchID  = dbo.f_GetHOBranchID(@OurBankID)    
SET @HOBranchLocalBranchCurrency  = (SELECT CurrencyID FROM t_SystemBranchSetting WHERE OurBranchID = @HOBranchID)     
    
 -- IF @TrxCurrencyID <> @HOBranchLocalBranchCurrency AND @VoucherCode NOT IN ('60','61','62')    
 -- BEGIN    
 --SELECT @TrxBranchID = @HOBranchID      
 -- END    
     
   --Checks for IB          
IF @TrxBranchID <> @OurBranchID       
BEGIN       
 SELECT @HOBranchID = dbo.f_GetHOBranchID(@OurBrankID)          
      
 IF (SELECT ParamValue          
  FROM t_SystemBankParameter  NOLOCK         
  WHERE BankID  = @OurBrankID          
   AND SysParamID = 17) = 1          
  SET @IsHOPostingRequired = 1          
 ELSE          
  SET @IsHOPostingRequired = 0          
      
 SELECT @HOTrxDate = dbo.f_GetWorkingDate(@HOBranchID)--dbo.f_GetTrxPostingDate(@HOBranchID)          
 SELECT @DestinationTrxDate = dbo.f_GetWorkingDate(@OurBranchID)--dbo.f_GetTrxPostingDate(@OurBranchID)          
      
      
 IF ISNULL(@HOTrxDate,@TrxDate) <> @TrxDate OR  @DestinationTrxDate <> @TrxDate          
 BEGIN          
  IF (SELECT dbo.f_SystemBankParameterExists(@OurBrankID,34)) <> 1          
  BEGIN             
   RAISERROR('BREXDB301003',16,1)          
   RETURN          
  END          
 END      
     
IF @IsHOPostingRequired = 1          
BEGIN          
 --select @HOTrxDate ,@TrxDate, @DestinationTrxDate          
 IF (@HOTrxDate <> @TrxDate) OR (@DestinationTrxDate <> @TrxDate) OR (@HOTrxDate <> @DestinationTrxDate)          
 BEGIN          
  RAISERROR('BREXDB301003',16,1)          
  RETURN          
 END             
 IF @IsCreditTrx = 1          
 BEGIN         
 SELECT @SourceIBGLID = AccountID FROM t_GLInterBranch   NOLOCK      
  WHERE OurBranchID = @HOBranchID AND CurrencyID = @TrxCurrencyID          
    AND AccountTagID = 'IB_PBLE_AC'          
      
  SELECT @HOIBDebitGLID = AccountID FROM t_GLInterBranch  NOLOCK         
  WHERE OurBranchID = @TrxBranchID AND CurrencyID = @TrxCurrencyID          
    AND AccountTagID = 'IB_RBLE_AC'          
      
  SELECT @HOIBCreditGLID = AccountID FROM t_GLInterBranch  NOLOCK         
  WHERE OurBranchID = @OurBranchID AND CurrencyID = @TrxCurrencyID          
    AND AccountTagID = 'IB_PBLE_AC'          
      
  SELECT @DestinationIBGLID = AccountID FROM t_GLInterBranch  NOLOCK         
  WHERE OurBranchID = @HOBranchID AND CurrencyID = @TrxCurrencyID          
    AND AccountTagID = 'IB_RBLE_AC'          
 END          
ELSE  --- Is a debit trx          
 BEGIN     
  SELECT @SourceIBGLID = AccountID FROM t_GLInterBranch  NOLOCK        
  WHERE OurBranchID = @HOBranchID AND CurrencyID = @TrxCurrencyID          
    AND AccountTagID = 'IB_RBLE_AC'          
      
  SELECT @HOIBCreditGLID = AccountID FROM t_GLInterBranch   NOLOCK        
  WHERE OurBranchID = @TrxBranchID AND CurrencyID = @TrxCurrencyID          
    AND AccountTagID = 'IB_PBLE_AC'          
      
SELECT @HOIBDebitGLID = AccountID FROM t_GLInterBranch    NOLOCK       
  WHERE OurBranchID = @OurBranchID AND CurrencyID = @TrxCurrencyID          
 AND AccountTagID = 'IB_RBLE_AC'          
      
  SELECT @DestinationIBGLID = AccountID FROM t_GLInterBranch  NOLOCK         
  WHERE OurBranchID = @HOBranchID AND CurrencyID = @TrxCurrencyID          
    AND AccountTagID = 'IB_PBLE_AC'          
 END           
  
 IF ISNULL(@SourceIBGLID, '') = '' OR  ISNULL(@HOIBCreditGLID, '') = ''           
  OR  ISNULL(@HOIBDebitGLID, '') = '' OR  ISNULL(@DestinationIBGLID, '') = ''          
  BEGIN         
   RAISERROR(N'BREXDB817051',16,1)          
   RETURN          
  END          
END      
ELSE -- No HO Posting          
BEGIN          
 IF @DestinationTrxDate <> @TrxDate          
 BEGIN          
  RAISERROR('BREXDB301003',16,1)          
  RETURN       
 END          
 IF @IsCreditTrx = 1          
  BEGIN    
   SELECT @SourceIBGLID = AccountID FROM t_GLInterBranch   NOLOCK        
   WHERE OurBranchID = @TrxBranchID AND CurrencyID = @TrxCurrencyID          
     AND AccountTagID = 'IB_RBLE_AC'          
      
   SELECT @DestinationIBGLID = AccountID FROM t_GLInterBranch  NOLOCK         
   WHERE OurBranchID = @OurBranchID AND CurrencyID = @TrxCurrencyID          
     AND AccountTagID = 'IB_PBLE_AC'          
  END          
 ELSE          
  BEGIN   
    
  --select 1,@TrxCurrencyID  
   SELECT @SourceIBGLID = AccountID FROM t_GLInterBranch  NOLOCK         
   WHERE OurBranchID = @TrxBranchID AND CurrencyID = @TrxCurrencyID          
     AND AccountTagID = 'IB_RBLE_AC'          
      
   SELECT @DestinationIBGLID = AccountID FROM t_GLInterBranch  NOLOCK         
   WHERE OurBranchID = @OurBranchID AND CurrencyID = @TrxCurrencyID          
   AND AccountTagID = 'IB_PBLE_AC'          
END          
  select @SourceIBGLID, @HOIBCreditGLID  
  
 IF ISNULL(@SourceIBGLID, '') = '' OR ISNULL(@DestinationIBGLID, '') = ''          
 BEGIN          
  RAISERROR(N'BREXDB817051',16,1)          
  RETURN          
 END          
END          
--1. Post to the IB GL of Trx Branch          
      
EXEC p_GetNextTrxSerialNo @OurBranchID = @TrxBranchID,           
  @NextTrxSerialNo = @SerialID OUTPUT,          
  @TrxSerialTypeID = 'TR'      
      
IF @TrxTypeID ='OC'  
BEGIN      
 SELECT @TrxDescription = 'Outward Credit Chq#: ' + CAST(@ChequeID AS VARCHAR(10)) + ' Bank :' + dbo.f_GetClearingBankName(@BankID)  
END  
    
SELECT @TempTrxDescID = @TrxDescriptionID,          
  @TempTrxDesc = @TrxDescription,          
  @TempTrxTypeID = @TrxTypeID          
SET @ValueDatedd = CASE WHEN @TempTrxTypeID = 'TD' THEN @TrxDate ELSE @ValueDate END         
    
--Print @TempTrxDescID    
--Print '1'     
EXEC p_InsertTransactions           
 @PTrxBranchID  = @TrxBranchID,          
 @PTrxRowID   = @NewRowID OUTPUT,          
 @PTrxBatchID  = @TrxBatchID OUTPUT,          
 @PSerialID   = @SerialID,          
 @POurBranchID  = @TrxBranchID,--@OurBranchID,          
 @PAccountTypeID  = 'G',          
 @PAccountID   = @SourceIBGLID,          
 @PProductID   = 'GL',          
      
 @PModuleID   = @ModuleID,          
 @PTrxCodeID   = 16,          
 @PTrxTypeID   = @TempTrxTypeID,          
 @PTrxDate   = @TrxDate,          
 @PValueDate   = @ValueDatedd,          
      
 @PAmount   = @Amount,          
 @PLocalAmount  = @LocalAmount,          
 @PTrxCurrencyID  = @TrxCurrencyID,          
 @PTrxAmount   = @TrxAmount,         
 @PExchangeRate  = @ExchangeRate,          
 @PMeanRate   = @MeanRate,          
      
 @PInstrumentTypeID = @InstrumentTypeID,          
 @PChequeID   = @ChequeID,          
 @PChequeDate  = @ChequeDate,          
 @PReferenceNo  = @ReferenceNo,          
@PRemarks   = @DrawerOrPayee,          
      
 @PTrxDescriptionID = @TempTrxDescID,          
 @PTrxDescription = @TempTrxDesc,          
 @PContraGLID  = @ContraGLID,          
 @PTrxFlagID   = '',          
 @PImageID   = @AutoGenerateImgID,--@UniqueImageID,          
 @PTrxPrinted  = 0,          
 @PIsTrxPending  = 0,          
 @PCreatedBy   = 'SYS'          
      
 SET @UniqueImageID = @NewRowID            
 --UPDATE t_Transaction SET ImageID = @NewRowID WHERE TrxRowID = @NewRowID          
      
 --2. Destination Branch IB posting           
 SELECT @TempTrxDescID = CASE WHEN @TrxTypeID = 'OC' THEN '008' ELSE '007' END,          
 @TempTrxDesc  = @TrxDescription,          
 @TempTrxTypeID  = CASE WHEN @TrxTypeID = 'OC' THEN 'TD' ELSE 'TC' END          
      
 SET @TempTrxDesc = @TempTrxDesc + ' For Batch :' + @TrxBatchID          
      
 EXEC p_GetNextTrxSerialNo @OurBranchID = @OurBranchID,                  
 @NextTrxSerialNo = @SerialID OUTPUT,          
 @TrxSerialTypeID = 'TR'          
 SET @ValueDatedd = CASE WHEN @TempTrxTypeID = 'TD' THEN @TrxDate ELSE @ValueDate END              
     
     
-- Print @TempTrxDescID    
--Print '2'     
--Destination  Barnch Posting          
EXEC p_InsertTransactions           
 @PTrxBranchID  = @TrxBranchID,          
 @PTrxRowID   = @NewRowID,          
 @PTrxBatchID  = @TrxBatchID,          
 @PSerialID   = @SerialID,          
 @POurBranchID  = @OurBranchID,      
 @PAccountTypeID  = 'G',     
 @PAccountID   = @DestinationIBGLID,          
      
 @PModuleID   = @ModuleID,          
 @PTrxCodeID   = 16,          
 @PTrxTypeID   = @TempTrxTypeID,       
 @PTrxDate   = @TrxDate,          
 @PValueDate   = @ValueDatedd,          
      
 @PAmount   = @Amount,          
 @PLocalAmount  = @LocalAmount,       
 @PTrxCurrencyID  = @TrxCurrencyID,          
 @PTrxAmount   = @TrxAmount,          
 @PExchangeRate  = @ExchangeRate,          
 @PMeanRate   = @MeanRate,          
      
 @PTrxDescriptionID = @TempTrxDescID,          
 @PTrxDescription = @TempTrxDesc,          
 @PContraGLID  = NULL,          
 @PTrxFlagID   = '',          
 @PImageID   = @AutoGenerateImgID,--@UniqueImageID,        
 @PTrxPrinted  = 0,          
 @PIsTrxPending  = 0,          
 @PCreatedBy   = 'SYS',          
 @PRemarks   = ''          
      
      
SELECT @TempTrxDescID = CASE WHEN @TrxTypeID = 'OC' THEN '007' ELSE '008' END,          
  @TempTrxDesc  = @TrxDescription,          
  @TempTrxTypeID  = CASE WHEN @TrxTypeID = 'OC' THEN 'TC' ELSE 'TD' END          
      
SET @TempTrxDesc = @TempTrxDesc + ' For Batch :' + @TrxBatchID          
      
SET @ValueDatedd = CASE WHEN @TempTrxTypeID = 'TD' THEN @TrxDate ELSE @ValueDate END              
    
    
--Print @TempTrxDescID    
--Print '3'     
--Post Customer/GL          
EXEC p_InsertTransactions           
 @PTrxBranchID  = @TrxBranchID,          
 @PTrxRowID   = @NewRowID,          
 @PTrxBatchID  = @TrxBatchID,          
 @PSerialID   = @SerialID,          
 @POurBranchID  = @OurBranchID,          
 @PAccountTypeID  = @AccountTypeID,          
 @PAccountID   = @AccountID,          
 @PProductID   = @ProductID,          
      
 @PModuleID   = @ModuleID,          
 @PTrxCodeID   = 0, --this is to indicate actual transaction          
 @PTrxTypeID   = @TempTrxTypeID,          
 @PTrxDate   = @DestinationTrxDate,          
 @PValueDate   = @ValueDatedd,          
      
 @PAmount   = @Amount,          
 @PLocalAmount  = @LocalAmount,          
 @PTrxCurrencyID  = @TrxCurrencyID,          
 @PTrxAmount   = @TrxAmount,          
 @PExchangeRate  = @ExchangeRate,          
 @PMeanRate   = @MeanRate,            
      
 @PInstrumentTypeID = @InstrumentTypeID,          
 @PChequeID   = @ChequeID,          
 @PChequeDate  = @ChequeDate,          
 @PReferenceNo  = @ReferenceNo,          
 @PRemarks   = @Remarks,          
      
 @PTrxDescriptionID = @TempTrxDescID,          
 @PTrxDescription = @TempTrxDesc,         
 @PMainGLID   = @MainGLID,          
 @PContraGLID  = NULL,          
 @PTrxFlagID   = @TrxFlagID,          
 @PImageID   =@AutoGenerateImgID,--@UniqueImageID,         
 @PTrxPrinted  = @TrxPrinted,          
 @PIsTrxPending  = 0,          
 @PCreatedBy   = @CreatedBy          
 IF @IsHOPostingRequired = 1          
 BEGIN          
  SELECT @TempTrxTypeID = CASE WHEN @IsCreditTrx = 1 THEN 'TC' ELSE 'TD' END,          
    @TempTrxDescID = CASE WHEN @IsCreditTrx = 1 THEN '007' ELSE '008' END          
  --@TempTrxDesc = dbo.f_GetTrxDescription(@TempTrxDescID) + ',' + @OurBranchID + '-' +  @AccountID + '-' + @AccountTypeID        
      
  SET  @TempTrxDesc = @TempTrxDesc + ' For Batch :' + @TrxBatchID          
  EXEC p_GetNextTrxSerialNo @OurBranchID = @HOBranchID,     
@NextTrxSerialNo = @SerialID OUTPUT,          
    @TrxSerialTypeID = 'TR'          
  SET @ValueDatedd = CASE WHEN @TempTrxTypeID = 'TD' THEN @TrxDate ELSE @ValueDate END              
     
     
--Print @TempTrxDescID    
--Print '4'      
 EXEC p_InsertTransactions           
  @PTrxBranchID  = @TrxBranchID,          
  @PTrxBatchID  = @TrxBatchID,          
  @PSerialID   = @SerialID,          
  @POurBranchID  = @HOBranchID,          
  @PAccountTypeID  = 'G',          
  @PAccountID   = @HOIBCreditGLID,          
      
  @PModuleID   = @ModuleID,          
  @PTrxCodeID   = 16,          
  @PTrxTypeID   = @TempTrxTypeID,          
  @PTrxDate   = @HOTrxDate,       
  @PValueDate   = @ValueDatedd,          
      
  @PAmount   = @Amount,          
  @PLocalAmount  = @LocalAmount,          
  @PTrxCurrencyID  = @TrxCurrencyID,          
  @PTrxAmount   = @TrxAmount,        
  @PExchangeRate  = @ExchangeRate,          
      
  @PTrxDescriptionID = @TempTrxDescID,          
  @PTrxDescription = @TempTrxDesc,          
  @PContraGLID  = NULL,          
  @PTrxFlagID   = '',          
  @PImageID   = @AutoGenerateImgID,--@UniqueImageID,          
  @PTrxPrinted  = @TrxPrinted,        
  @PIsTrxPending  = 0,        
  @PCreatedBy   = 'SYS'          
      
      
 SELECT @TempTrxTypeID = CASE WHEN @IsCreditTrx = 1 THEN 'TD' ELSE 'TC' END,          
   @TempTrxDescID = CASE WHEN @IsCreditTrx = 1 THEN '008' ELSE '007' END          
   --@TempTrxDesc = dbo.f_GetTrxDescription(@TempTrxDescID) + ',' + @OurBranchID + '-' +  @AccountID + '-' + @AccountTypeID          
 SET @TempTrxDesc = @TempTrxDesc + ' For Batch :' + @TrxBatchID          
      
 SET @ValueDatedd = CASE WHEN @TempTrxTypeID = 'TD' THEN @TrxDate ELSE @ValueDate END              
     
--Print @TempTrxDescID    
--Print '5'     
    
 EXEC p_InsertTransactions           
  @PTrxBranchID  = @TrxBranchID,        
  @PTrxBatchID  = @TrxBatchID,          
  @PSerialID   = @SerialID,          
  @POurBranchID  = @HOBranchID,          
  @PAccountTypeID  = 'G',          
  @PAccountID   = @HOIBDebitGLID,          
      
  @PModuleID   = @ModuleID,          
  @PTrxCodeID   = 16,          
  @PTrxTypeID   = @TempTrxTypeID,        
  @PTrxDate   = @HOTrxDate,          
  @PValueDate   = @ValueDatedd,          
      
  @PAmount   = @Amount,          
  @PLocalAmount  = @LocalAmount,          
  @PTrxCurrencyID  = @TrxCurrencyID,          
  @PTrxAmount   = @TrxAmount,          
  @PExchangeRate  = @ExchangeRate,          
      
  @PTrxDescriptionID = @TempTrxDescID,      
 @PTrxDescription = @TempTrxDesc,          
  @PContraGLID  = NULL,          
  @PTrxFlagID   = '',          
  @PImageID   = @AutoGenerateImgID,--@UniqueImageID,         
@PTrxPrinted  = @TrxPrinted,          
  @PIsTrxPending  = 0,          
  @PCreatedBy   = 'SYS'          
 END               
END -- Checks for IB ends          
           
IF @TrxBranchID = @OurBranchID  ---Local Branch     
BEGIN           
  SET @TrxDescriptionID = CASE  WHEN @TrxTypeID='OC' AND @ReturnCodeID IN ('00','17') THEN '005' ELSE @TrxDescriptionID END          
  IF @VoucherCode='03' AND @ReturnCodeID <>'00'          
  BEGIN          
   SET @ReturnCodeID = '00'          
  END          
      
 IF (@ReturnCodeID <>'00' OR @ReturnCodeID In('02','03'))          
 BEGIN          
  IF @CreatedBy <> 'SYS'      
  BEGIN      
   SET @TrxFlagID = 'U'          
  END           
 END          
      
 --Kamunya - This is meant to prevent posting of Unpaid again and again       
 IF EXISTS(SELECT DISTINCT t_Transaction.TrxRowID FROM t_Transaction  (NOLOCK)       
  Inner Join t_TrxClearing ( NOLOCK)         
 On t_Transaction.TrxBatchID = t_TrxClearing.TrxBatchID          
 WHERE t_Transaction.ChequeID = @ChequeID           
   AND  t_TrxClearing.ImageID = @ImageID    
   AND t_TrxClearing.AccountID <> @AccountID             
   AND ABS(t_Transaction.TrxAmount) = ABS(@Amount)          
   AND ModuleID IN ('3060','3070')          
   AND TrxTypeID IN ('OD','OC')          
   And ReturnCodeID NOT IN ('00','17')    
   And isNull(t_TrxClearing.Isdeleted,0) =0)           
 BEGIN          
 RAISERROR(N'BREXDB999955',16,1)          
  RETURN          
 END          
      
 --IF EXISTS(SELECT DISTINCT t_Transaction.TrxRowID FROM t_Transaction  (NOLOCK)          
 --  Inner Join t_TrxClearing ( NOLOCK)         
 --  On ABS(t_Transaction.TrxAmount) = ABS(t_TrxClearing.Amount)          
 --WHERE t_Transaction.ChequeID = @ChequeID           
 --  AND  t_TrxClearing.ImageID = @ImageID    
 --  AND t_TrxClearing.AccountID <> @AccountID             
 --  AND ABS(t_Transaction.TrxAmount) = ABS(@Amount)          
 --  AND ModuleID IN ('3060', '3061','3070')          
 --  AND TrxTypeID IN ('OD','OC')          
 --  And ReturnCodeID IN ('00','17')    
 --  And isNull(t_TrxClearing.Isdeleted,0) =0)        
 --BEGIN          
 -- RAISERROR(N'BREXDB999955',16,1)          
 -- RETURN          
 --END         
      
--Print @TrxDescriptionID    
--Print '6'     
    
IF @TrxTypeID = 'OC'  
BEGIN    
 SELECT @TrxDescription = 'Outward Credit Chq#: ' + CAST(@ChequeID AS VARCHAR(10)) + ' Bank ' + dbo.f_GetClearingBankName(@BankID)    
END  
  
 EXEC p_InsertTransactions           
 @PTrxBranchID  = @TrxBranchID,        
 @PTrxRowID   = @NewRowID OUTPUT,          
 @PTrxBatchID  = @TrxBatchID OUTPUT,          
 @PSerialID   = @SerialID OUTPUT,          
 @POurBranchID  = @OurBranchID,      
 @PAccountTypeID  = @AccountTypeID,          
 @PAccountID   = @AccountID,          
 @PProductID   = @ProductID,          
      
 @PModuleID   = @ModuleID,          
 @PTrxCodeID   = 0,          
 @PTrxTypeID   = @TrxTypeID,          
 @PTrxDate   = @TrxDate,          
 @PValueDate   = @ValueDate,          
      
 @PAmount   = @Amount,          
 @PLocalAmount  = @LocalAmount,         
 @PTrxCurrencyID  = @TrxCurrencyID,          
 @PTrxAmount   = @TrxAmount,          
 @PExchangeRate  = @ExchangeRate,          
 @PMeanRate   = @MeanRate,            
      
 @PInstrumentTypeID = @InstrumentTypeID,          
 @PChequeID   = @ChequeID,          
 @PChequeDate  = @ChequeDate,          
 @PReferenceNo  = @ReferenceNo,          
 @PRemarks   = @Remarks,          
      
 @PTrxDescriptionID = @TrxDescriptionID,          
 @PTrxDescription = @TrxDescription,          
 @PMainGLID = @MainGLID,          
 @PContraGLID  = @ContraGLID,          
 @PTrxFlagID   = @TrxFlagID,          
 @PImageID   = @AutoGenerateImgID,          
 @PTrxPrinted  = @TrxPrinted,          
 @PIsTrxPending  = 0,          
 @PCreatedBy   = @CreatedBy                
      
 SET @UniqueImageID = @AutoGenerateImgID--@UniqueImageID,  @NewRowID          
 --UPDATE t_Transaction SET ImageID = @NewRowID WHERE TrxRowID = @NewRowID          
      
 --UPDATE t_Transaction SET ImageID = @AutoGenerateImgID WHERE TrxRowID = @NewRowID          
END        
    
    
IF IsNull(@NewRowID,'') = ''    
BEGIN    
  SELECT @NewRowID = TrxRowID, @TrxBatchSLNo = TrxBatchSLNo     
  FROM  t_Transaction (NOLOCK)      
  WHERE ImageID = @AutoGenerateImgID     
END        
    
    
      
IF @ReturnCodeID NOT IN ('00','17')     
BEGIN    
 SELECT @OldColumnID = ColumnID    
 FROM   t_TrxClearing  NOLOCK          
 WHERE  ImageID=@ImageID AND Abs(Amount) = @Amount    
    
 IF @OldColumnID is Null    
  SELECT @OldColumnID = ColumnID    
  FROM   t_AccountTrxClearing  NOLOCK          
  WHERE  ImageID=@ImageID AND Abs(Amount) = @Amount    
    
END    
    
      
IF @CreatedBy <> 'SYS'      
BEGIN      
 SET @TrxFlagID = 'U'          
END           
IF (LEN(LTRIM(RTRIM(@ReturnCodeID)))<2)--I have included this Temporally to take care of the returncode that comes as int hence truncation one zero - Kamunya          
BEGIN          
 SET @ReturnCodeID =  '0' + @ReturnCodeID          
END          
        
IF @VoucherCode = '40'          
BEGIN          
 IF EXISTS( SELECT Top 1 *          
 FROM  t_TrxClearing    NOLOCK        
 WHERE ImageID=@ImageID AND TrxType='ID' AND VoucherCode='40')          
 BEGIN          
  SELECT @OrigCode=OriginatorCode, @OrigRef=OrigRefCode, @Policy1=PolicyNumber1, @Policy2=PolicyNumber2, @ColumnID=ColumnID           
  FROM   t_TrxClearing  NOLOCK          
  WHERE  ImageID=@ImageID AND TrxType='ID' AND VoucherCode='40'     
      
  SET  @OldColumnID = @ColumnID        
 END          
ELSE          
 BEGIN          
  SELECT @ColumnID = ColumnID FROM  t_TrxClearing  NOLOCK          
  WHERE  ImageID = @ImageID AND TrxType = 'ID' AND VoucherCode = '40'          
      
    print 'k2'    
  SELECT @OrigCode = OriginatorCode, @OrigRef = OriginatorRef, @Policy1 = Policy1, @Policy2 = Policy2          
  FROM   t_TrxInwards  NOLOCK          
  WHERE  ColumnID = @ColumnID AND TrxType = 'ID' AND VoucherCode = '40'     
      
  SET  @OldColumnID = @ColumnID           
 END          
      
 SELECT @NewRowID=TrxRowID           
 FROM t_transaction   NOLOCK         
 WHERE TrxBatchID=@TrxBatchID AND OurBranchID=@OurBranchID          
END        
       
IF @VoucherCode = '40' AND @ReturnCodeID NOT IN ('00','17')      
BEGIN      
SET @TrxTypeID='OD'       
END       
     
     
--IF @ReturnCodeID NOT IN ('00','17') AND @OldVATSerialID = '7778MFI'    
--BEGIN    
-- SET @OldColumnID = @ImageID    
--END     
     
INSERT INTO t_TrxClearing          
(          
 TrxRowID,TrxBranchID,TrxBatchID,TrxBatchSLNo,OurBranchID,AccountTypeID,          
 AccountID,ChequeDigit,VoucherCode,          
 ReturnCodeID,          
 Commission,TheirCommission,BankID,BranchID,DrawerOrPayeeAccountID,          
 DrawerOrPayee,ChequeID,ChequeDate,ValueDate,          
 Amount,          
 CurrencyID,PinNumber,VatName,VatType,          
 PaymentTypeID,          
 VatNumber,MonthOfPayment,VATPAYECommission,TrxType,[Date],OriginalAmount,ImageID,          
 OrigRefCode,OriginatorCode,PolicyNumber1,PolicyNumber2,ColumnID,IsUnpaidItem          
)          
SELECT           
 ISNULL(@NewRowID,0),@TrxBranchID,@TrxBatchID,@TrxBatchSLNo,@OurBranchID,@AccountTypeID,          
 @AccountID,@ChequeDigit,@VoucherCode,          
 CASE @VoucherCode          
 WHEN '42' THEN '00'          
 WHEN '39' THEN '00'          
 ELSE dbo.f_CRB_ReturnCodeDescriptionsForUnpay(@ReturnCodeID)  END,          
 @Commission,@TheirCommission,@BankID,@BranchID,@DrawerOrPayeeAccountID,          
 @DrawerOrPayee,    
 CASE WHEN @TrxTypeID ='OD' AND @VoucherCode <> '40' THEN 0 ELSE @ChequeID END,    
 ISNULL(@ChequeDate,@TrxDate),@ValueDate,          
 CASE WHEN @TrxTypeID ='OD' THEN -1 * ABS(@Amount) ELSE @Amount END,           
 @TrxCurrencyID,@VATPINNo,@VATPAYType,          
 CASE @VoucherCode WHEN '42' THEN LEFT(@VATPAYType,2) ELSE '' END,          
 CASE @VoucherCode WHEN '39' THEN LEFT(@VATPAYType,2) ELSE '' END,          
 @VATSerialNo,@VATPAYEMonth,@VATPAYECommission,@TrxTypeID,@TrxDate,@OriginalAmount,          
 CASE WHEN @TrxTypeID ='OC' THEN @AutoGenerateImgID ELSE @NewRowID END,    --@ImageID      
 CASE WHEN @VoucherCode ='40' THEN @OrigRef ELSE @Remarks END,        
 CASE WHEN @VoucherCode ='40' THEN @OrigCode ELSE '0' END,          
 CASE WHEN @VoucherCode ='40' THEN @Policy1 ELSE '0' END,          
 CASE WHEN @VoucherCode ='40' THEN @Policy2 ELSE '0' END,          
 ISNULL(@OldColumnID,TRY_PARSE(@ReferenceNo AS bigint)),           
 CASE WHEN @VoucherCode ='40' THEN 1 ELSE 0 END       
      
--SELECT @OldVATSerialID, @TrxTypeID, @ReturnCodeID    
IF @ReturnCodeID NOT IN ('00') AND @TrxTypeID IN ('OC')        
BEGIN       
 IF @ReturnCodeID NOT IN ('00','17') AND @TrxTypeID IN ('OC')        
 BEGIN     
  DECLARE @OldTrxrowID BigInt        
  IF EXISTS( SELECT 1 FROM t_ReconcilableItem  NOLOCK WHERE OurBranchID = @OurBranchID          
   AND AccountID = @AccountID AND ChequeID = @ChequeID AND ReconcileStatusID <> 'R')             
    BEGIN          
   UPDATE t_ChequeTrx SET  ChequeStatusID = ''      
   WHERE ChequeDate = @TrxDate         
   AND ChequeID = @ChequeID AND ChequeStatusID = 'P'      
   AND AccountID = @AccountID          
    END        
  ELSE        
    BEGIN        
    DELETE FROM t_ChequeTrx           
    WHERE ChequeDate = @TrxDate         
    AND ChequeID = @ChequeID AND ChequeStatusID = 'P'      
    AND AccountID = @AccountID         
    END        
 END           
 IF @Remarks <> 'MIPS Uploads'    
 BEGIN       
 print 'Hapa 1'    
-- BRNET_IMAGEServer.    
  INSERT INTO BRNET_IMAGEServer.dbo.t_ChequeImages           
  (          
    ImageID,OurBranchID,TrxType,TFImage,JFImage,JRImage,UVImage,          
    TFImageSize,JFImageSize,JRImageSize,BankId,OperatorID,TFImageSignature,          
    JFImageSignature,JRImageSignature,CreatedOn,CurrencyID,Validity,[Date],IsMdv          
  )          
  SELECT @AutoGenerateImgID,@TrxBranchID,@TrxTypeID,TFImage,JFImage,JRImage,UVImage,          
    TFImageSize,JFImageSize,JRImageSize,@BankID,'SYS',TFImageSignature,          
    JFImageSignature,JRImageSignature,@TrxDate,CurrencyID,Validity,@TrxDate,@IsForfeit     
  FROM BRNET_IMAGEServer.dbo.t_IncomingChequeImages  NOLOCK          
  WHERE ImageID = @ImageID        
 END        
      
UPDATE t_TrxClearing SET ReturnCodeID = a.ReturnedTempID    
FROM (SELECT ColumnID,ReturnedTempID FROM t_IncomingTransactions WHERE ColumnID IN (    
SELECT ColumnID FROM t_TrxClearing WHERE isNull(ColumnID,'') <>'' and TrxType IN ('OC','OD') )AND isNull(ReturnedTempID,'') <> '') AS A    
WHERE t_TrxClearing.ColumnID= a.ColumnID AND t_TrxClearing.TrxType IN ('OC','OD') AND isNull(ReturnCodeID,'') IN ('00','17')    
    
UPDATE t_TrxClearing     
SET TrxType = 'OD'    
WHERE VoucherCode = '40' AND ReturnCodeID NOT IN ('00','17') AND TrxType IN ('OD','OC') AND TrxType <> 'OD'    
    
UPDATE t_TrxClearing           
SET  IsUnpaidItem = 1,      
ColumnID  = @ColumnID      
WHERE TrxRowID = @NewRowID      
    
    
 IF @ReturnCodeID NOT IN ('00','17') AND @TrxTypeID IN ('OC') AND @OldVATSerialID NOT IN('7777MFI' ,'7778MFI')     
 BEGIN     
   SELECT @ColumnID = ColumnID       
   FROM t_TrxClearing NOLOCK      
   WHERE ImageID = @ImageID   AND Abs(Amount) = @Amount    
      
 END     
    
    
   --SET @TrxDescriptionID = CASE WHEN @VoucherCode IN ('39','40') THEN '089' ELSE @TrxDescriptionID END    
    
     
END     
ELSE IF @ReturnCodeID IN ('00') AND @TrxTypeID IN ('OC') AND @OldVATSerialID = '7777MFI'      
  BEGIN TRY    
   DECLARE @NewTrxBatchID VarChar(8)    
       
   --SELECT @OldColumnID, @NewRowID, @UniqueImageID, @AutoGenerateImgID    
       
   --UPDATE t_TrxClearing     
   --SET ImageID = @NewRowID     
   --WHERE TrxRowID = @NewRowID AND ColumnID = @OldColumnID    
       
   --UPDATE t_Transaction SET ImageID = @NewRowID WHERE ImageID = @AutoGenerateImgID    
       
   --INSERT INTO t_ChequeImages     
   --(    
   -- OurBranchID,AccountID,Date,TrxType,TFImage,JFImage,JRImage,UVImage,    
   -- BankId,ChequeId,OperatorID,CreatedOn,ImageID    
   --)    
   --SELECT     
   -- @OurBranchID,@AccountID,@TrxDate,@TrxTypeID,FRONTBWIMAGE,FRONTGRAYSCALEIMAGE,    
   -- REARIMAGE, NULL, @BankID, @ChequeID,'SYS',@TrxDate,@AutoGenerateImgID    
   --FROM t_TrxInwardsMFI (NOLOCK)    
   --WHERE ColumnID = @OldColumnID    
END TRY    
BEGIN CATCH    
 --EXECUTE usp_GetErrorInfo;    
 PRINT 'Shoot'    
END CATCH    
     
ELSE        
 BEGIN      
     
 IF @TrxTypeID IN ('OC') And @CreatedBy <> 'REALMIB'    
 BEGIN       
  SELECT @BankID = RIGHT('000'+ RTRIM(@BankID),3)    
  --SELECT @BankID = RIGHT('00'+ RTRIM(RIGHT(@BankID,2)),2)        
   --select @CreatedBy,@AccountID,@BranchID,@BankID,@DrawerOrPayeeAccountID,@ChequeID     
   INSERT INTO BRNET_IMAGEServer.dbo.t_ChequeImages           
   (          
     ImageID,OurBranchID,TrxType,TFImage,JFImage,JRImage,UVImage,          
     TFImageSize,JFImageSize,JRImageSize,BankId,OperatorID,TFImageSignature,          
     JFImageSignature,JRImageSignature,[Date],CreatedOn,CurrencyID,isMDV          
   )          
   SELECT      
     @AutoGenerateImgID,OurBranchID,'OC' as TrxType,TFImage,          
     FrontImage,BackImage,UVImage,TFImageSize,JFImageSize,          
     JRImageSize,BankID,OperatorID,TFImageSignature,          
     JFImageSignature,JRImageSignature,[Date],GetDate(),VoucherCode,isMDV          
   FROM t_BRChequeTruncation   NOLOCK         
   WHERE OperatorID=@CreatedBy AND AccountID=@AccountID AND BranchID=@BranchID           
     AND BankID=@BankID AND TheirAcc = @DrawerOrPayeeAccountID And ChequeID = @ChequeID          
 END      
     
  UPDATE t_TrxClearing         
  SET  IsMDV =(SELECT TOP 1 isMDV          
  FROM t_BRChequeTruncation  NOLOCK          
  WHERE OperatorID=@CreatedBy AND AccountID=@AccountID AND BranchID=@BranchID           
    AND BankID=@BankID AND TheirAcc = @DrawerOrPayeeAccountID)          
  WHERE TrxRowID=@NewRowID       
      
   ----print 'Haya hapa'    
   --select 'T', @AccountID,@OurBranchID,@ChequeID,@BankID    
   --select 'Kamunya 2'    
     
    
  DELETE       
  FROM t_BRChequeTruncation         
     WHERE OperatorID=@CreatedBy           
    AND OurBranchID=@OurBranchID          
    AND AccountID=@AccountID           
    AND RIGHT('000000'+ RTRIM(RIGHT(ChequeID,6)),6)  = @ChequeID      
    AND BankID=@OrigBankID    
     AND BranchID =@OrigBranchID           
 END           
     SELECT @BankID = RIGHT('00'+ RTRIM(RIGHT(@BankID,2)),2)   
         
 UPDATE t_TrxClearing  SET TrxRowID =  t_Transaction.TrxRowID   FROM  t_Transaction(NOLOCK)       
 WHERE t_Transaction.TrxBatchID = t_TrxClearing.TrxBatchID AND t_Transaction.OurBranchID = t_TrxClearing.OurBranchID       
 AND t_TrxClearing.TrxType = 'OD' AND ISNULL(t_TrxClearing.TrxRowID ,0)=0      
     
--UPDATE t_TrxClearing  SET ColumnID =  t_Trxinwards.ColumnID FROM t_Trxinwards    
--WHERE    
---- Abs(t_Trxclearing.amount) = Abs(t_Trxinwards.amount)    
----AND     
--t_Trxclearing.bankID = t_Trxinwards.BankID    
--AND t_Trxclearing.BranchID = t_Trxinwards.BranchID    
--AND t_Trxclearing.trxtype='OD' AND t_Trxclearing.returncodeid<>'00'    
--AND t_Trxinwards.drawerorpayee = t_Trxclearing.drawerorpayee    
 IF @Remarks <> 'EFTsBulkUploads'    
 BEGIN    
 UPDATE t_TrxClearing  SET BankID =  t_TrxInwards.BankID, BranchID  =  t_TrxInwards.BranchID  FROM  t_TrxInwards (NOLOCK)     
 WHERE t_TrxInwards.ColumnID = t_TrxClearing.ColumnID AND t_TrxClearing.TrxType IN ('ID','IC')      
 end    
--SET NOCOUNT OFF         
      
END     
    
SET @TrxDescription = CASE WHEN @TrxTypeID IN ('OC', 'TC') THEN @TrxDescription + ' : ' + cast(@ChequeID as varchar) ELSE @TrxDescription END    
    
SELECT @TrxDescription = 'For Chq No. ' + CAST(@ChequeID as VARCHAR)    
    
IF @ReturnCodeID NOT IN ('00','17') AND @TrxTypeID IN ('OC','TC')    
 BEGIN      
   SET @TrxDescriptionID = CASE WHEN @ReturnCodeID IN ('63') THEN '086'      
    WHEN @ReturnCodeID = '62' THEN '087'    
     WHEN @ReturnCodeID IN ('33','37','38','55','84')  THEN '091'    
    WHEN @ReturnCodeID IN ('31','32','40','53','57','66') THEN '088'    
         ELSE '005' END    
 END     
    
    
  --Charges for unpaids    
--IFEUR - INSUFFICIENT FUNDS EUR    
--IFGBP - INSUFFICIENT FUNDS GBP    
--IFKES - INSUFFICIENT FUNDS TZS    
--IFUSD - INSUFFICIENT FUNDS USD    
DECLARE @ChargeID Varchar(10)    
IF @TrxDescriptionID ='086' AND @TrxCurrencyID ='USD' AND @TrxTypeID IN ('OC', 'TC')    
 BEGIN    
  SELECT @ChargeID = 'IFUSD'    
 END    
ELSE IF @TrxDescriptionID ='086' AND @TrxCurrencyID ='GBP' AND @TrxTypeID IN ('OC', 'TC')    
 BEGIN    
   SELECT @ChargeID = 'IFGBP'    
 END    
ELSE IF @TrxDescriptionID ='086' AND @TrxCurrencyID ='EUR' AND @TrxTypeID IN ('OC', 'TC')    
 BEGIN    
   SELECT @ChargeID = 'IFEUR'    
 END    
ELSE IF @TrxDescriptionID ='086' AND @TrxCurrencyID ='TZS' AND @TrxTypeID IN ('OC', 'TC')    
 BEGIN    
   SELECT @ChargeID = 'DISCQ'    
 END    
    
--TREUR TECHNICAL REASONS EUR    
--TRGBP TECHNICAL REASONS GBP    
--TRKES TECHNICAL REASONS KES    
--TRUSD TECHNICAL REASONS USD    
IF @TrxDescriptionID ='088' AND @TrxCurrencyID ='USD' AND @TrxTypeID IN ('OC', 'TC')    
 BEGIN    
  SELECT @ChargeID = 'TRUSD'    
 END    
ELSE IF @TrxDescriptionID ='088' AND @TrxCurrencyID ='GBP' AND @TrxTypeID IN ('OC', 'TC')    
 BEGIN    
   SELECT @ChargeID = 'TRGBP'    
 END    
ELSE IF @TrxDescriptionID ='088' AND @TrxCurrencyID ='EUR' AND @TrxTypeID IN ('OC', 'TC')    
 BEGIN    
   SELECT @ChargeID = 'TREUR'    
 END    
ELSE IF @TrxDescriptionID ='088' AND @TrxCurrencyID ='TZS' AND @TrxTypeID IN ('OC', 'TC')    
 BEGIN    
   SELECT @ChargeID = 'DCTER'    
 END    
    
--UEEUR UNCLEARED EFFECTS EUR    
--UEGBP UNCLEARED EFFECTS GBP    
--UEKES UNCLEARED EFFECTS KES    
--UEUSD UNCLEARED EFFECTS USD    
IF @TrxDescriptionID ='087' AND @TrxCurrencyID ='USD' AND @TrxTypeID IN ('OC', 'TC')    
 BEGIN    
  SELECT @ChargeID = 'UEUSD'    
 END    
ELSE IF @TrxDescriptionID ='087' AND @TrxCurrencyID ='GBP' AND @TrxTypeID IN ('OC', 'TC')    
 BEGIN    
   SELECT @ChargeID = 'UEGBP'    
 END    
ELSE IF @TrxDescriptionID ='087' AND @TrxCurrencyID ='EUR' AND @TrxTypeID IN ('OC', 'TC')    
 BEGIN    
   SELECT @ChargeID = 'UEEUR'    
 END    
ELSE IF @TrxDescriptionID ='087' AND @TrxCurrencyID ='TZS' AND @TrxTypeID IN ('OC', 'TC')    
 BEGIN    
   SELECT @ChargeID = 'DISCQ'    
 END    
--Charges for unpaids    
    
IF @TrxCurrencyID ='TZS' AND @TrxTypeID IN ('OD')    
BEGIN    
  SELECT @ChargeID = 'SIOB'    
  SELECT @TrxDescriptionID ='090'     
END    
    
IF @Remarks <> 'GatewayUpload'    
BEGIN    
 EXEC  p_ChargeTransaction          
    @OurBranchID     = @OurBranchID,         
    @AccountID   = @AccountID,        
@TrxDescriptionID = @TrxDescriptionID,      
    @TrxCurrencyID  = @TrxCurrencyID,     
    @ModuleID   = @ModuleID,      
    @CreatedBy   = @CreatedBy,    
    @Narration   = @TrxDescription,    
    @ChargeID   = @ChargeID,    
    @ErrorNo   = @ErrorCode OUTPUT    
    --,    
    --@TrxFlagID   = @TrxFlagID      
 IF ISNULL(@ErrorCode ,'0') <> '0'        
 BEGIN        
  RAISERROR(N' BREXDB123456',16,1)        
 END     
END    
    
  