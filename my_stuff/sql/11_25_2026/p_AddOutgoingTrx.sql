CREATE PROCEDURE [dbo].[p_AddOutgoingTrx]  
(   
@TrxBranchID   BranchID,        
@TrxBatchID    VarChar(8) OUTPUT,        
@TrxBatchSLNo   SmallInt,        
@SerialID    Int =0 OUTPUT,        
@OurBranchID   BranchID,        
@AccountTypeID   Char(1),        
@AccountID    AccountID,        
@ProductID    ProductID,  
@ModuleID    SmallInt,        
@TrxTypeID    Char(2),        
@TrxDate    SmallDateTime,        
@ValueDate    SmallDateTime,        
@Amount     Amount,        
@LocalAmount   Amount,        
@TrxCurrencyID   CurrencyID,        
@TrxAmount    Amount,        
@ExchangeRate   CurrencyRate,        
@MeanRate    CurrencyRate,        
@Profit     Amount = 0,  
@InstrumentTypeID  Char(1)='V',        
@ChequeID    Int=0,        
@ChequeDate    SmallDateTime = Null,        
@ReferenceNo   nVarChar(15) = Null,        
@Remarks    Remarks = Null,  
@TrxDescriptionID  nVarChar(6),        
@TrxDescription   Description,        
@MainGLID    AccountID,        
@ContraGLID    AccountID = Null,        
@TrxFlagID    SystemSubID = '',        
@ImageID    Int = 0,        
@TrxPrinted    TinyInt = 0,        
@CreatedBy    OperatorID,        
@NewRecord     TinyInt,        
@IsForfeit    bit=0,        
@ChequeDigit   Char(1),        
@VoucherCode   Char(2),        
@ReturnCodeID   Char(4),        
@Commission    Amount,        
@TheirCommission  Amount,  
@BankID     BankID,        
@BranchID    BranchID,        
@DrawerOrPayeeAccountID AccountID,        
@DrawerOrPayee   Names,        
@VATPINNo    nVarChar(12),        
@VATPAYType    nVarChar(10),        
@VATSerialNo   nVarChar(7),        
@VATPAYEMonth   nVarChar(6),        
@VATPAYECommission  Amount,        
@ErrorNo    VarChar(50)='' OUTPUT,  
@SupervisedBy OperatorID = NULL,  
@TrxRowID    BigInt = 0       
)  
--WITH ENCRYPTION        
AS   
  
BEGIN TRANSACTION  
BEGIN TRY   
  
BEGIN    
  
  
SET NOCOUNT ON        
DECLARE         
@strString Varchar(Max),        
@PValueDate SmallDateTime,        
@IsCreditTrx   Bit,        
@IsHOPostingRequired Bit,        
@HOBranchID    dbo.BranchID,        
@HOTrxDate    SmallDateTime,        
@DestinationTrxDate  SmallDateTime,        
@DestinationIBGLID  dbo.AccountID,           
@SourceIBGLID   dbo.AccountID,        
@HOIBCreditGLID   dbo.AccountID,        
@HOIBDebitGLID   dbo.AccountID,        
@DescriptionID   nVarChar(6),        
@NewRowID    BigInt,        
@TempAccountID   dbo.AccountID,        
@TempTrxTypeID   Char(2),        
@TempTrxDescID   nVarChar(6),        
@TempTrxDesc   Description,        
@UniqueImageID   BigInt,        
@OriginalAmount   Amount,        
@OurBrankID    BankID,        
@ValueDatedd   DateTime,        
@OrigCode    varchar(4),        
@OrigRef    varchar(25),        
@Policy1    varchar(25),        
@Policy2    varchar(25),        
@ColumnID    varchar(50),  
@OldColumnID varchar(50),    
@ErrorCode    VarChar(50)='',    
@ODTrxRowID BigInt,  
@OldVATSerialID Varchar(20) ,  
@SupervisedOn datetime    
           
Declare @DeleteTrxRowID BigInt,        
@DeleteChequeDate Datetime,        
@DeleteBranchID Varchar(5),        
@DeleteAccountID Varchar(25),        
@DeleteChequeID Int,        
@ValueCapingAmt Amount,    
@Day  nVarchar(3),      
@Time nVarchar(8),      
@AutoGenerateImgID  Int=0      
              
EXEC p_GetUniqueClearingImageID @AutoGenerateImgID OUTPUT     
 --print '3'  
IF isNUll(@SupervisedBy,'') <> ''  
SET @SupervisedOn = getdate()  
IF EXISTS(SELECT ImageID FROM t_Transaction NOLOCK WHERE ImageID = @AutoGenerateImgID AND TrxTypeID = 'OC')  
BEGIN  
SET @AutoGenerateImgID = @AutoGenerateImgID + 1  
END     
  
  
  
IF @ReturnCodeID NOT IN ('00','17')   
BEGIN  
 SELECT @OldColumnID = @TrxRowID  
 SELECT @ReferenceNo = @TrxRowID  
END  
       
IF @TrxDate <> dbo.f_GetWorkingDate(@OurBranchID)        
BEGIN        
RAISERROR('BREXDB300032',16,1)        
RETURN        
END         
  
 IF @VoucherCode = '40' AND @ReturnCodeID IN ('00','17')   
 BEGIN  
 RAISERROR('BREXDB300004',16,1)      --Direct Debit in Kenya has been aoutomated with images, so manual posting is prohibited from Outward Debit Module.  RETURN   
END   
  
IF @BankID =  dbo.f_GetBankID(@OurBranchID)     
 BEGIN  
  RAISERROR('BREXDB3337474',16,1)        
  RETURN   
 END  
  
  
 --print '1'  
--Kamunya - This is meant to prevent posting of Unpaid again and again        
 IF EXISTS(SELECT 1 FROM t_Transaction  (NOLOCK)        
 WHERE Try_parse(ReferenceNo AS bigint) = @OldColumnID    
   AND ModuleID IN ('3060','3070','3061')        
   AND TrxTypeID IN ('OD','OC')        
   And isNull(DeletedReason,'') ='')         
 BEGIN        
  RAISERROR(N'BREXDB999955',16,1)        
  RETURN        
 END        
  --print '2'  
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
  
 IF isNull(@TrxDescriptionID,'') = ''  
 BEGIN  
 SELECT @TrxDescriptionID = '005'  
 END   
  
--------Kamunya - Am including this to take care for the automation of ougoing Unpaid Items for UFT----------------  
  
SET @TrxDescriptionID = CASE  
WHEN @TrxTypeID = 'OC' AND @TrxDescriptionID NOT IN ('043', '116', '117', '118','086','087','088') AND  @VoucherCode <> '40' THEN '005'  
WHEN @TrxTypeID = 'OC' AND @ReturnCodeID IN ('63') THEN '086'  
WHEN @TrxTypeID = 'OC' AND @TrxDescriptionID IN ('043') THEN '086'  
WHEN @TrxTypeID = 'OC' AND @ReturnCodeID IN ('62') THEN '087'  
-- WHEN @TrxTypeID = 'OC' AND @ReturnCodeID NOT IN ('04','05','10','11','12','13','14','15','16','17','20','21','22','62','63',  
        -- '23','25','26','27','28','29','30','34','35','36','39','47','48','49','50','51','65','57','66','69','72','71','73','74'  
        -- ,'75','76','77','78','81','82','83','84','85','86','87','88','89','90') THEN '088'  
WHEN @TrxTypeID = 'OC' AND @ReturnCodeID IN ('31','32','33','37','38','40','41','42','43','53','54','55','58','64','79') THEN '088'  
WHEN @TrxTypeID = 'OC' AND @ReturnCodeID IN ('33','37','38','55','84')  THEN '091'  
ELSE @TrxDescriptionID  
END  
  
--Select @ReturnCodeID,@TrxDescriptionID  
--------Kamunya - Am including this to take care for the automation of EFT Items for FTB ----------------  
  
IF @TrxTypeID IN ('OC','OD') AND @ReturnCodeID NOT IN ('00','17')  
BEGIN  
SELECT @ColumnID = ColumnID  
FROM   t_TrxClearing  NOLOCK        
WHERE  ImageID = @ImageID AND TrxType IN ('ID','IC')  
IF isNull(@ColumnID,'') = ''  
BEGIN  
SELECT @ColumnID = ColumnID  
FROM   t_AccountTrxClearing  NOLOCK        
WHERE  ImageID = @ImageID   
AND OurBranchID = @OurBranchID  
AND ABS(Amount) = ABS(@Amount)  
AND TrxType IN ('ID','IC')  
END  
  
  
IF EXISTS(SELECT 1 FROM t_TrxClearing  (NOLOCK)        
WHERE ColumnID = @ColumnID AND TrxType IN ('OC','OD'))      
BEGIN        
RAISERROR(N'BREXDB999955',16,1)    
RETURN        
END    
END   
--IF @TrxTypeID = 'OC' AND @ReturnCodeID IN ('00','17')  
--BEGIN  
--SET @ValueDate = dbo.fn_GetValueDate(@OurBranchID, @TrxCurrencyID, @BankID, @BranchID)  
--EXEC p_GetValueDate @OurBranchID=@OurBranchID,@AccountTypeID=@AccountTypeID, @ShowValueDate = 0,  
--@CurrencyID=@TrxCurrencyID,@AccountID=@AccountID,@BankID=@BankID,@BranchID=@BranchID,@WorkingDate=@TrxDate,  
--@VoucherCode=@VoucherCode,@ReturnCode=@ReturnCodeID,@Amount=@Amount, @ErrorNo=@ErrorNo OUTPUT,@ValueDate=@ValueDate OUTPUT  
--END  
   
  
IF @TrxTypeID = 'OC' AND @ReturnCodeID IN ('00','17')  
 BEGIN  
 SET @ValueDate = dbo.fn_GetValueDate(@OurBranchID, @TrxCurrencyID, @BankID, @BranchID)  
 END  
  
IF Len(@Remarks) > 20  
BEGIN  
SET @Remarks = LEFT(@Remarks,25)  
--RAISERROR('BREXDB543374',16,1)        
--RETURN    
END  
   
  
  
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
  
SET @OldVATSerialID = @VATSerialNo  
IF @TrxTypeID = 'OC' AND @ReturnCodeID IN ('00') AND @OldVATSerialID <> '7777MFI'  
BEGIN  
IF NOT EXISTS(SELECT 1 FROM t_BRChequeTruncation   (NOLOCK)       
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
--SET @TrxDescriptionID = CASE WHEN @ReturnCodeID = '63' THEN '086'    
--WHEN @ReturnCodeID = '62' THEN '087'    
--WHEN @ReturnCodeID IN ('40','53','32','31') THEN '005' ELSE '088' END    
SET @TrxDescription = 'Unpaid Direct Debit with Return rsn: ' + DBO.f_CRB_ReturnCodeDescriptions('ReturnCodeID',@ReturnCodeID,'T')  
SET @Amount = @Amount * -1    
SET @LocalAmount = @LocalAmount * -1     
SET @TrxAmount = @TrxAmount * -1    
END     
--print @TrxDescriptionID  
--- Valuecaping    
    
SELECT @ValueCapingAmt = ISNULL(ValueCappingAmount,99999999) FROM t_Currency  NOLOCK WHERE CurrencyID = @TrxCurrencyID        
IF @Amount > @ValueCapingAmt        
BEGIN        
RAISERROR('BREXDB300051',16,1)        
RETURN        
END         
    
    
--IF (SELECT ISNULL(ClgFileGeneratedDateUnpaids,EODDate) FROM t_SystemBranchStatus  NOLOCK   
--WHERE OurBranchID = @OurBranchID AND  @ReturnCodeID NOT IN ('00','17')) >= dbo.f_GetWorkingDate(@OurBranchID)        
--BEGIN        
--RAISERROR('BREXDB300050',16,1)        
--RETURN        
--END    
        
----- Do not allow to post outward if file is generated        
--IF (SELECT ISNULL(ClgFileGeneratedDate,EODDate) FROM t_SystemBranchStatus  NOLOCK   
--WHERE OurBranchID = @OurBranchID AND  @ReturnCodeID IN ('00','17')) >= dbo.f_GetWorkingDate(@OurBranchID)        
--BEGIN        
--RAISERROR('BREXDB300050',16,1)        
--RETURN        
--END  
DECLARE @ServerTime time  
SELECT @ServerTime = CONVERT(TIME, GETDATE())  
IF @TrxTypeID = 'OC' AND @ReturnCodeID IN ('00','17')  
 BEGIN  
 IF @ServerTime > '13:00:00:0000000'  
  BEGIN  
   SET @ValueDate = dbo.fn_GetValueDateTPlus(@OurBranchID, @TrxCurrencyID, @BankID, @BranchID,1)  
  END  
 ELSE  
  BEGIN  
   SELECT @ValueDate = dbo.fn_GetValueDate(@OurBranchID, @TrxCurrencyID, @BankID, @BranchID)  
  END  
 END  
   
   
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
BEGIN        SET @ValueDate  = @TrxDate        
SET @OriginalAmount = @Amount + @TrxAmount        
SET @Amount   = @TrxAmount        
END        
SET @IsCreditTrx = CASE WHEN @TrxTypeID = 'OC' THEN 1 ELSE 0 END        
SET @OurBrankID = dbo.f_GetBankID(@OurBranchID)        
IF @CreatedBy <> 'SYS'    
BEGIN    
SET @TrxFlagID = 'U'      
--SET @TrxFlagID = ''   --sanj-lets have it go for supervision  
END       
         
   --Checks for IB        
IF @TrxBranchID <> @OurBranchID        
BEGIN        
SELECT @HOBranchID = dbo.f_GetHOBranchID(@OurBrankID)        
    
IF (SELECT ParamValue FROM t_SystemBankParameter  NOLOCK WHERE BankID  = @OurBrankID AND SysParamID = 17) = 1        
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
SELECT @SourceIBGLID = AccountID FROM t_GLInterBranch   NOLOCK      
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
SELECT @SourceIBGLID = AccountID FROM t_GLInterBranch  NOLOCK       
WHERE OurBranchID = @TrxBranchID AND CurrencyID = @TrxCurrencyID        
AND AccountTagID = 'IB_RBLE_AC'        
    
SELECT @DestinationIBGLID = AccountID FROM t_GLInterBranch  NOLOCK       
WHERE OurBranchID = @OurBranchID AND CurrencyID = @TrxCurrencyID        
AND AccountTagID = 'IB_PBLE_AC'        
END        
IF ISNULL(@SourceIBGLID, '') = '' OR ISNULL(@DestinationIBGLID, '') = ''        
BEGIN        
RAISERROR(N'BREXDB817051',16,1)        
RETURN        
END        
END        
--1. Post to the IB GL of Trx Branch        
    
EXEC p_GetNextTrxSerialNo   
@OurBranchID = @TrxBranchID,         
@NextTrxSerialNo = @SerialID OUTPUT,        
@TrxSerialTypeID = 'TR'        
    
SELECT @TempTrxDescID = @TrxDescriptionID,        
@TempTrxDesc = @TrxDescription,        
@TempTrxTypeID = @TrxTypeID        
  
SET @ValueDatedd = CASE WHEN @TempTrxTypeID = 'TD' THEN @TrxDate ELSE @ValueDate END   
  
IF isNull(@TrxDate,'')=''  
BEGIN  
 SELECT @TrxDate = dbo.f_GetWorkingDate(@TrxBranchID)  
END  
  
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
@PRemarks   = @Remarks,  
@PTrxDescriptionID = @TempTrxDescID,        
@PTrxDescription = @TempTrxDesc,        
@PContraGLID  = @ContraGLID,        
@PTrxFlagID   = @TrxFlagID,        
@PImageID   = @AutoGenerateImgID,--@UniqueImageID,        
@PTrxPrinted  = 0,        
@PIsTrxPending  = 0,        
@PCreatedBy   = @CreatedBy,  
@SupervisedBy   = @SupervisedBy,  
@SupervisedOn = @SupervisedOn        
    
SET @UniqueImageID = @NewRowID          
UPDATE t_Transaction SET ImageID = @NewRowID WHERE TrxRowID = @NewRowID        
    
--2. Destination Branch IB posting         
SELECT @TempTrxDescID = CASE WHEN @TrxTypeID = 'OC' THEN '008' ELSE '007' END,        
@TempTrxDesc  = @TrxDescription,        
@TempTrxTypeID  = CASE WHEN @TrxTypeID = 'OC' THEN 'TD' ELSE 'TC' END        
    
SET @TempTrxDesc = @TempTrxDesc + ' For Batch :' + @TrxBatchID        
    
EXEC p_GetNextTrxSerialNo @OurBranchID = @OurBranchID,                
@NextTrxSerialNo = @SerialID OUTPUT,        
@TrxSerialTypeID = 'TR'        
SET @ValueDatedd = CASE WHEN @TempTrxTypeID = 'TD' THEN @TrxDate ELSE @ValueDate END            
   
   
 IF isNull(@TrxDate,'')=''  
BEGIN  
 SELECT @TrxDate = dbo.f_GetWorkingDate(@TrxBranchID)  
END  
  
--Print @TempTrxDescID  
--Print '2'    
  
--Destination  Barnch Posting        
EXEC p_InsertTransactions         
@PTrxBranchID  = @TrxBranchID,        
@PTrxRowID = @NewRowID,        
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
@PTrxFlagID   = 'U',        
@PImageID   = @AutoGenerateImgID,--@UniqueImageID,      
@PTrxPrinted  = 0,        
@PIsTrxPending  = 0,        
@PCreatedBy   = 'SYS',        
@PRemarks   = '',  
@SupervisedBy   = @SupervisedBy,  
@SupervisedOn = @SupervisedOn         
    
SELECT @TempTrxDescID = CASE WHEN @TrxTypeID = 'OC' THEN '007' ELSE '008' END,        
@TempTrxDesc  = @TrxDescription,        
@TempTrxTypeID  = CASE WHEN @TrxTypeID = 'OC' THEN 'TC' ELSE 'TD' END        
    
SET @TempTrxDesc = @TempTrxDesc + ' For Batch :' + @TrxBatchID   
SET @ValueDatedd = CASE WHEN @TempTrxTypeID = 'TD' THEN @TrxDate ELSE @ValueDate END            
  
IF isNull(@DestinationTrxDate,'')=''  
BEGIN  
 SELECT @DestinationTrxDate = dbo.f_GetWorkingDate(@TrxBranchID)  
END  
  
--Print @TempTrxDescID  
--Print '3'   
   
--Post Customer/GL        
EXEC p_InsertTransactions       
@PTrxBranchID  = @TrxBranchID,        
@PTrxRowID = @NewRowID,        
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
@PTrxFlagID   = 'U',     
@PImageID   =@AutoGenerateImgID,--@UniqueImageID,       
@PTrxPrinted  = @TrxPrinted,        
@PIsTrxPending  = 0,        
@PCreatedBy   = @CreatedBy,  
@SupervisedBy   = @SupervisedBy,  
@SupervisedOn = @SupervisedOn       
   
IF @IsHOPostingRequired = 1        
BEGIN        
SELECT @TempTrxTypeID = CASE WHEN @IsCreditTrx = 1 THEN 'TC' ELSE 'TD' END,        
@TempTrxDescID = CASE WHEN @IsCreditTrx = 1 THEN '007' ELSE '008' END        
--@TempTrxDesc = dbo.f_GetTrxDescription(@TempTrxDescID) + ',' + @OurBranchID + '-' +  @AccountID + '-' + @AccountTypeID        
    
SET @TempTrxDesc = @TempTrxDesc + ' For Batch :' + @TrxBatchID        
EXEC p_GetNextTrxSerialNo @OurBranchID = @HOBranchID,                 
@NextTrxSerialNo = @SerialID OUTPUT,        
@TrxSerialTypeID = 'TR'        
SET @ValueDatedd = CASE WHEN @TempTrxTypeID = 'TD' THEN @TrxDate ELSE @ValueDate END            
  
IF isNull(@HOTrxDate,'')=''  
BEGIN  
 SELECT @HOTrxDate = dbo.f_GetWorkingDate(@TrxBranchID)  
END  
  
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
@PTrxFlagID   = 'U',        
@PImageID   = @AutoGenerateImgID,--@UniqueImageID,        
@PTrxPrinted  = @TrxPrinted,        
@PIsTrxPending  = 0,        
@PCreatedBy   = 'SYS',  
@SupervisedBy   = @SupervisedBy,  
@SupervisedOn = @SupervisedOn         
    
    
SELECT @TempTrxTypeID = CASE WHEN @IsCreditTrx = 1 THEN 'TD' ELSE 'TC' END,        
@TempTrxDescID = CASE WHEN @IsCreditTrx = 1 THEN '008' ELSE '007' END        
--@TempTrxDesc = dbo.f_GetTrxDescription(@TempTrxDescID) + ',' + @OurBranchID + '-' +  @AccountID + '-' + @AccountTypeID        
SET @TempTrxDesc = @TempTrxDesc + ' For Batch :' + @TrxBatchID        
    
SET @ValueDatedd = CASE WHEN @TempTrxTypeID = 'TD' THEN @TrxDate ELSE @ValueDate END            
  
  
IF isNull(@HOTrxDate,'')=''  
BEGIN  
 SELECT @HOTrxDate = dbo.f_GetWorkingDate(@TrxBranchID)  
END  
  
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
@PTrxDate = @HOTrxDate,        
@PValueDate   = @ValueDatedd,      
    
@PAmount   = @Amount,        
@PLocalAmount  = @LocalAmount,        
@PTrxCurrencyID  = @TrxCurrencyID,        
@PTrxAmount   = @TrxAmount,    
@PExchangeRate  = @ExchangeRate,        
    
@PTrxDescriptionID = @TempTrxDescID,        
@PTrxDescription = @TempTrxDesc,        
@PContraGLID  = NULL,        
@PTrxFlagID   = 'U',        
@PImageID   = @AutoGenerateImgID,--@UniqueImageID,       
@PTrxPrinted  = @TrxPrinted,        
@PIsTrxPending  = 0,        
@PCreatedBy   = 'SYS' ,  
@SupervisedBy   = @SupervisedBy,  
@SupervisedOn = @SupervisedOn        
END              
END -- Checks for IB ends        
  
  
          
IF @TrxBranchID = @OurBranchID  ---Local Branch      
BEGIN      
  
--SELECT 'Imeingia hapa'     
--SET @TrxDescriptionID = CASE  WHEN @TrxTypeID='OC' AND @ReturnCodeID IN ('00','17') THEN '005' ELSE @TrxDescriptionID END        
IF @VoucherCode='03' AND @ReturnCodeID <>'00'        
BEGIN        
SET @ReturnCodeID = '00'        
END        
    
IF (@ReturnCodeID <>'00' OR @ReturnCodeID In('02','03'))        
BEGIN        
IF @CreatedBy <> 'SYS'    
BEGIN    
SET @TrxFlagID = 'U'  
--SET @TrxFlagID = ''  --sanj-lets have it go for supervision      
END         
END        
  
IF isNull(@TrxDate,'')=''  
BEGIN  
 SELECT @TrxDate = dbo.f_GetWorkingDate(@TrxBranchID)  
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
@PMainGLID   = @MainGLID,        
@PContraGLID  = @ContraGLID,        
@PTrxFlagID   = @TrxFlagID,        
@PImageID   = 0,        
@PTrxPrinted  = @TrxPrinted,        
@PIsTrxPending  = 0,        
@PCreatedBy   = @CreatedBy ,  
@SupervisedBy   = @SupervisedBy,  
@SupervisedOn = @SupervisedOn              
    
SET @UniqueImageID = @AutoGenerateImgID--@UniqueImageID,  @NewRowID        
--UPDATE t_Transaction SET ImageID = @NewRowID WHERE TrxRowID = @NewRowID   
    
UPDATE t_Transaction SET ImageID = @AutoGenerateImgID WHERE TrxRowID = @NewRowID        
END      
    
--IF @ReturnCodeID NOT IN ('00','17')   
--BEGIN  
-- SELECT @OldColumnID = ColumnID  
-- FROM   t_TrxClearing  NOLOCK     
-- WHERE  ImageID = @ImageID  
-- IF isNull(@OldColumnID,'') = ''  
-- BEGIN  
--  SELECT @OldColumnID = ColumnID  
--  FROM   t_AccountTrxClearing  NOLOCK        
--  WHERE  ImageID = @ImageID   
--  AND OurBranchID = @OurBranchID  
--  AND ABS(Amount) = ABS(@Amount)  
-- END  
--END  
  
  
   
IF @ReturnCodeID IN ('00','17') AND @OldVATSerialID = '7777MFI'  
BEGIN  
SET @OldColumnID = @ImageID  
END        
IF @CreatedBy <> 'SYS'    
BEGIN    
SET @TrxFlagID = 'U'   
--SET @TrxFlagID = ''     --sanj-lets have it go for supervision   
END         
IF (LEN(LTRIM(RTRIM(@ReturnCodeID)))<2)--I have included this Temporally to take care of the returncode that comes as int hence truncation one zero - Kamunya        
BEGIN        
SET @ReturnCodeID =  '0' + @ReturnCodeID        
END   
      
IF @VoucherCode = '40'        
BEGIN   
IF EXISTS( SELECT Top 1 TrxType FROM  t_TrxClearing    NOLOCK      
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
         
--New Module Changes Gichuhi  
SELECT @OrigCode = OriginatorCode, @OrigRef = OriginatorRef, @Policy1 = Policy1, @Policy2 = Policy2        
FROM   t_IncomingTransactions  NOLOCK    
WHERE ColumnID =  @ColumnID AND TrxType = 'ID' AND VoucherCode = '40'     
    
SET  @OldColumnID = @ColumnID         
END        
    
      
END      
       
IF @VoucherCode = '40' AND @ReturnCodeID NOT IN ('00','17')    
BEGIN    
SET @TrxTypeID='OD'     
END     
  
SELECT  @BankID = BankID, @BranchID = BranchID FROM t_IncomingTransactions WHERE ColumnID = @OldColumnID  
  
IF isNull(@NewRowID,0) = 0  
BEGIN  
 SELECT Top 1 @NewRowID = TrxRowID  FROM t_Transaction   NOLOCK       
 WHERE TrxBatchID=@TrxBatchID AND OurBranchID=@OurBranchID AND AccountTypeID ='C'   
END   
  
IF @VoucherCode = '40' AND @ReturnCodeID NOT IN ('00','17')    
BEGIN    
 SET @TrxTypeID='OD'   
END  
  
  
  
  
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
@DrawerOrPayee,@ChequeID,ISNULL(@ChequeDate,@TrxDate),@ValueDate,        
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
ISNULL(@OldColumnID,0),         
CASE WHEN @VoucherCode ='40' THEN 1 ELSE 0 END     
    
    
--print 'Imefika hapa sasa'  
  
--SELECT @OldVATSerialID, @TrxTypeID, @ReturnCodeID  
  
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
    
  
UPDATE t_TrxClearing SET ReturnCodeID = a.ReturnedTempID  
FROM (SELECT ColumnID,ReturnedTempID FROM t_IncomingTransactions WHERE ColumnID IN (  
SELECT ColumnID FROM t_TrxClearing WHERE isNull(ColumnID,'') <>'' and TrxType IN ('OC','OD') )AND isNull(ReturnedTempID,'') <> '') AS A  
WHERE t_TrxClearing.ColumnID= a.ColumnID AND t_TrxClearing.TrxType IN ('OC','OD') AND isNull(ReturnCodeID,'') IN ('00','17')  
    
SELECT @ColumnID = ColumnID     
FROM t_TrxClearing NOLOCK    
WHERE ImageID = @ImageID    
    
UPDATE t_TrxClearing SET  IsUnpaidItem = 1, ColumnID  = @ColumnID    
WHERE TrxRowID = @NewRowID  
END   
ELSE IF @ReturnCodeID IN ('00','17') AND @TrxTypeID IN ('OC') AND @OldVATSerialID = '7777MFI'    
BEGIN  
BEGIN TRY  
DECLARE @NewTrxBatchID VarChar(8)  
---SELECT @OldColumnID, @NewRowID, @UniqueImageID, @AutoGenerateImgID  
UPDATE t_TrxClearing   
SET ImageID = @NewRowID   
WHERE TrxRowID = @NewRowID AND ColumnID = @OldColumnID  
UPDATE t_Transaction SET ImageID = @NewRowID WHERE ImageID = @AutoGenerateImgID  
--INSERT INTO t_ChequeImages   
--(  
--OurBranchID,AccountID,Date,TrxType,TFImage,JFImage,JRImage,UVImage,  
--BankId,ChequeId,OperatorID,CreatedOn,ImageID  
--)  
--SELECT   
--@OurBranchID,@AccountID,@TrxDate,@TrxTypeID,FRONTBWIMAGE,FRONTGRAYSCALEIMAGE,  
--REARIMAGE, NULL, @BankID, @ChequeID,'SYS',@TrxDate,@NewRowID  
--FROM t_TrxInwardsMFI (NOLOCK)  
--WHERE ColumnID = @OldColumnID  
END TRY  
BEGIN CATCH  
--EXECUTE usp_GetErrorInfo;  
PRINT 'Shoot'  
END CATCH  
END  
ELSE  
BEGIN   
    
   
UPDATE t_TrxClearing       
SET  IsMDV =(SELECT isMDV        
FROM t_BRChequeTruncation  NOLOCK        
WHERE OperatorID=@CreatedBy AND AccountID=@AccountID AND BranchID=@BranchID         
AND BankID=@BankID AND TheirAcc = @DrawerOrPayeeAccountID)        
WHERE TrxRowID=@NewRowID  
   
DELETE     
FROM t_BRChequeTruncation         
WHERE OperatorID=@CreatedBy       
AND OurBranchID=@OurBranchID        
AND AccountID=@AccountID       
AND ChequeID=@ChequeID     
AND BankID=@BankID        
END         
   
       
UPDATE t_TrxClearing  SET TrxRowID =  t_Transaction.TrxRowID   FROM  t_Transaction     
WHERE t_Transaction.TrxBatchID = t_TrxClearing.TrxBatchID AND t_Transaction.OurBranchID = t_TrxClearing.OurBranchID     
AND t_TrxClearing.TrxType = t_Transaction.TrxTypeID AND ISNULL(t_TrxClearing.TrxRowID ,0)=0    
UPDATE t_TrxClearing  SET ColumnID =  t_IncomingTransactions.ColumnID   FROM  t_IncomingTransactions     
WHERE t_IncomingTransactions.TrxBatchID = t_TrxClearing.TrxBatchID AND t_TrxClearing.TrxType = 'OD' AND ISNULL(t_TrxClearing.ColumnID ,0)=0  
AND t_TrxClearing.ReturnCodeID NOT IN ('00','17')   
   
UPDATE t_TrxClearing  SET BankID =  t_IncomingTransactions.BankID, BranchID  =  t_IncomingTransactions.BranchID    
FROM  t_IncomingTransactions     
WHERE t_IncomingTransactions.ColumnID = t_TrxClearing.ColumnID AND t_TrxClearing.TrxType IN ('ID','IC')    
  
SET NOCOUNT OFF       
    
END   
  
SET @TrxDescription = CASE WHEN @TrxTypeID IN ('OC', 'TC') THEN @TrxDescription ELSE @TrxDescription END  
  
IF @ReturnCodeID NOT IN ('00','17') AND @TrxTypeID IN ('OC','TC')  
 BEGIN    
   SET @TrxDescriptionID = CASE WHEN @ReturnCodeID IN ('63') THEN '086'    
    WHEN @ReturnCodeID = '62' THEN '087'  
    WHEN @ReturnCodeID IN ('33','37','38','55','84')  THEN '091'  
    WHEN @TrxTypeID = 'OC' AND @ReturnCodeID IN ('31','32','33','37','38','40','41','42','43','53','54','55','58','64','79') THEN '088'  
         ELSE '005' END  
 END    
  
--IFEUR - INSUFFICIENT FUNDS EUR  
--IFGBP - INSUFFICIENT FUNDS GBP  
--IFKES - INSUFFICIENT FUNDS KES  
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
ELSE IF @TrxDescriptionID ='086' AND @TrxCurrencyID ='KES' AND @TrxTypeID IN ('OC', 'TC')  
 BEGIN  
   SELECT @ChargeID = 'IFKES'  
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
ELSE IF @TrxDescriptionID ='088' AND @TrxCurrencyID ='KES' AND @TrxTypeID IN ('OC', 'TC')  
 BEGIN  
   SELECT @ChargeID = 'OATRKES'  
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
ELSE IF @TrxDescriptionID ='087' AND @TrxCurrencyID ='KES' AND @TrxTypeID IN ('OC', 'TC')  
 BEGIN  
   SELECT @ChargeID = 'UEKES'  
 END  
  
  
IF @TrxCurrencyID ='KES' AND @TrxTypeID IN ('OD') AND @VoucherCode <>'40'  
BEGIN  
  SELECT @ChargeID = 'OEFTKES'  
  SELECT @TrxDescriptionID ='090'   
END  
  
EXEC p_ChargeTransaction    
@OurBranchID  = @OurBranchID,   
@AccountID  = @AccountID,   
@TrxDescriptionID = @TrxDescriptionID,  
@TrxAmount  = @Amount,   
@TrxDate  = @TrxDate,   
@TrxCurrencyID  = @TrxCurrencyID,   
@ModuleID  = @ModuleID,   
@CreatedBy  = @CreatedBy,  
@TrxBatchID  = @TrxBatchID,   
@SerialID  = @SerialID,  
@ChargeID  = @ChargeID,   
@WaiveCharge  = 0,  
@ErrorNo  = @ErrorCode OUTPUT  
--,  
--@TrxFlagID = @TrxFlagID  
     
  
SET @TrxDescription = CASE WHEN @TrxTypeID IN ('OC', 'TC') THEN @TrxDescription + ' : ' + cast(@ChequeID as varchar) ELSE @TrxDescription END  
  
SELECT @TrxDescription = 'For Chq No. ' + CAST(@ChequeID as VARCHAR)  
  
IF ISNULL(@ErrorCode ,'0') <> '0'      
BEGIN     
print 'Kuna error'   
RAISERROR(N' BREXDB123456',16,1)      
END     
  
END TRY  
  
BEGIN CATCH     
    --SELECT ERROR_MESSAGE()     
Declare @ErrorMessage varchar(4000)  
SELECT  @ErrorMessage = 'ERROR: Process: ' + IsNull(ERROR_PROCEDURE(),'') + ', Line:' + CONVERT(nvarchar, IsNull(ERROR_LINE(),'')) + ', ' + IsNull(ERROR_MESSAGE(),'')       
Print @ErrorMessage  
  
ROLLBACK TRANSACTION  
--RAISERROR (@ErrorMessage, 16, 1)   
IF ISNULL (@ErrorCode,'0') = '0'  
SET @ErrorCode = '455004'    
    
RETURN        
END CATCH     
  
COMMIT TRANSACTION       
SET NOCOUNT OFF   
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  