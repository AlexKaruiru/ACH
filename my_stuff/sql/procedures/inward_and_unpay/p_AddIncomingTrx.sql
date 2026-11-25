CREATE PROCEDURE [dbo].[p_AddIncomingTrx]  
( @TrxBranchID BranchID              
, @TrxBatchID VarChar(8)            OUTPUT  
, @SerialID Int                     = 0 OUTPUT  
, @OurBranchID BranchID              
, @AccountTypeID Char(1)             
, @AccountID AccountID               
, @ProductID ProductID               
, @ModuleID SmallInt                 
, @TrxCodeID TinyInt                 
, --Not required  
  @TrxTypeID Char(2)                 
, -- Inward Credit 'IC' and inward debit 'ID'  
  @TrxDate SmallDateTime             
, @ValueDate SmallDateTime           
, @Amount Amount                     
, @TrxCurrencyID CurrencyID          
, @InstrumentTypeID Char(1)          
, -- It will be voucher 'V'  
  @ChequeID Int                      
, -- Instrument ID  
  @ChequeDate SmallDateTime         = Null  
, -- Instrument date  
  @ReferenceNo nVarChar(15)         = Null  
, @Remarks Remarks                  = Null  
, @TrxDescriptionID nVarChar(6)      
, -- 'IC' = '004',  'ID' = '005'  
  @TrxDescription Description        
, @MainGLID AccountID                
, @ContraGLID AccountID             = Null  
, -- Validate in BAL leyer.  
  @TrxFlagID SystemSubID            = ''  
, @ImageID Int                      = Null  
, @TrxPrinted TinyInt               = 0  
,  
--Include By Kamunya  
  @ChequeDigit Char(1)             =  0  
, @VoucherCode Char(2)             =  0  
, @ReturnCodeID varChar(5)         =  0  
, @Commission Amount               =  0  
, @TheirCommission Amount          =  0  
, @VATPINNo nVarChar(12)           =  null  
, @VATPAYType nVarChar(10)         =  null  
, @VATSerialNo nVarChar(7)         =  null  
, @VATPAYEMonth nVarChar(6)        =  null  
, @VATPAYECommission Amount        =  0  
,  
---------------------------------  
  @BankID BankID                     
, @BranchID BranchID                 
, @DrawerOrPayeeAccountID AccountID  
, @DrawerOrPayee Names               
, @CreatedBy OperatorID              
, @NewRecord TinyInt                 
, @TrxRowID BigInt                   
, @ForwardRemark varchar(max)      =  null  
, @SupervisedBy OperatorID          = NULL  
)  
  
AS  
BEGIN  
 SET NOCOUNT ON  
 --select @TrxBranchID  
 ------Return  
  
 DECLARE @NewRowID                 BigInt           
 ,       @ChequeStatus             Char(1)          
 ,       @ErrorCode                VarChar(15)      
 ,       @TrxPending               Bit              
 ,       @LocalCurrencyID          CurrencyID       
 ,       @TrxAmount                Amount           
 ,       @LocalAmount              Amount           
 ,       @ExchangeRate             Rate             
 ,       @ErrorNo                  VarChar(50)      
 ,       @MeanRate                 Rate             
 ,       @Profit                   Amount           
 ,       @RoundingID               Systemsubid      
 ,       @ExtraDetails             VarChar(50)      
 ,       @OriginatorCode           VarChar(4)       
 ,       @OriginatorRef            VarChar(25)      
 ,       @Policy1                  VarChar(25)      
 ,       @Policy2                  VarChar(25)      
 ,       @BRBankID                 VarChar(25)      
 ,       @AvailableBalance         dbo.Amount       
 ,       @FreezeAmount             dbo.Amount       
 ,       @ClientID                 dbo.ClientID     
 ,       @ChargingCurrencyID       Currencyid       
 ,       @IsTaxable                Bit              
 ,       @IsTaxOnTrxAmount         Bit              
 ,       @TaxID                    NVarChar(6)      
 ,       @CessID                   NVarChar(6)      
 ,       @ChargingMethodID         Char(4)          
 ,       @ChargeTrxDescriptionID   VarChar(5)       
 ,       @ChargeTrxDescription     VarChar(300)     
 ,       @CustomerTrxDescriptionID VarChar(5)       
 ,       @CustomerTrxNarration     VarChar(300)     
 ,       @GLAccountID              Accountid        
 ,       @AccountClassID           VarChar(15)      
 ,       @OurBranchWorkingDate     SmallDateTime    
 ,       @CurrencyID               Currencyid       
 ,  @CommissionAmount   Amount           
 ,       @LCommissionAmount        Amount           
 ,       @TaxAmount                Amount           
 ,       @CessAmount               Amount           
 ,       @MainRowID                BIGINT          = 0  
 ,       @strString                VarChar(Max)     
 ,       @PValueDate               SmallDateTime    
 ,       @IsCreditTrx              Bit              
 ,       @IsHOPostingRequired      Bit              
 ,       @HOBranchID               dbo.BranchID     
 ,       @HOTrxDate                SmallDateTime    
 ,       @DestinationTrxDate       SmallDateTime    
 ,       @DestinationIBGLID        dbo.AccountID    
 ,       @SourceIBGLID             dbo.AccountID    
 ,       @HOIBCreditGLID           dbo.AccountID    
 ,       @HOIBDebitGLID            dbo.AccountID    
 ,       @DescriptionID            NVarChar(6)      
 ,       @TempAccountID            dbo.AccountID    
 ,       @TempTrxTypeID            Char(2)          
 ,       @TempTrxDescID            NVarChar(6)      
 ,       @TempTrxDesc              dbo.Description  
 ,       @UniqueImageID            BigInt           
 ,       @OriginalAmount           dbo.Amount       
 ,       @OurBankID                dbo.BankID       
 ,       @ValueDatedd              DateTime         
 ,       @OrigCode                 VarChar(4)       
 ,       @OrigRef                  VarChar(25)      
 ,       @ColumnID                 VarChar(15)      
 ,       @SetClearingDays          Int              
 ,       @DeleteTrxRowID           BigInt           
 ,       @DeleteChequeDate         DateTime         
 ,       @DeleteBranchID           VarChar(5)       
 ,       @DeleteAccountID          VarChar(25)      
 ,       @DeleteChequeID           Int              
 ,       @ValueCapingAmt           dbo.Amount       
 ,       @ChargeTrxDesc            dbo.Description  
 ,       @AccountCurrencyID        dbo.CurrencyID   
 ,       @LChargeAmount            dbo.Amount       
 , --Local Amount  
         @LTaxAmount               dbo.Amount       
 ,       @LCessAmount              dbo.Amount       
 ,       @ChargeCurrencyID         dbo.CurrencyID   
 ,       @TotalCharge              dbo.Amount       
 ,       @TaxPercent               dbo.Amount       
 ,       @TrxDesc                  dbo.Description  
 ,       @Day                      nVarchar(3)      
 ,       @Time                     nVarchar(8)      
 ,       @AutoGenerateImgID        Int             = 0  
 ,       @ActualBranchID           BranchID         
 ,       @ActualBankID             BankID           
 ,  
 --@SupervisedBy OperatorID,  
         @SupervisedOn             datetime         
  
  
 IF EXISTS (SELECT 1  
  FROM t_TrxClearing  
  WHERE ColumnID = @TrxRowID)  
 BEGIN  
  RETURN  
 END  
  
 BEGIN TRANSACTION POSTINWARDS  
 BEGIN TRY  
  
 IF @TrxTypeID  = 'IC'  
  BEGIN  
   SELECT @ContraGLID = dbo.f_GetCurrencyBranchGLAccountID(@OurBranchID, @TrxCurrencyID, 'CUR_CLR_AC')--_EFT')  
   SELECT @IsCreditTrx = 1  
  END  
 ELSE IF @TrxTypeID  = 'ID' AND @VoucherCode = '40'  
  BEGIN  
   SELECT @ContraGLID = dbo.f_GetCurrencyBranchGLAccountID(@OurBranchID, @TrxCurrencyID, 'CUR_CLR_AC')--_EFT')  
  END  
 ELSE  
  BEGIN  
   SELECT @ContraGLID = dbo.f_GetCurrencyBranchGLAccountID(@OurBranchID, @TrxCurrencyID, 'CUR_CLR_AC')--_CHQ')  
  END  
  
  
 EXEC p_GetUniqueClearingImageID @AutoGenerateImgID OUTPUT  
  
 IF @VoucherCode ='40'  
 BEGIN  
  Declare @Originator Varchar(40)  
  SELECT @Originator = SUBSTRING(Data,106,35)  
  FROM t_IncomingTransactions  
  WHERE TrxBatchID = @TrxBatchID  
  SET @ChequeID = 999999  
  SET @TrxDescription = 'Direct Debit for ' + @Originator + 'From Bnk ' + @BankID  
 END  
  
 IF isNUll(@SupervisedBy,'') = ''  
  SET @SupervisedBy = NULL  
 IF isNUll(@SupervisedBy,'') <> ''  
  SET @SupervisedOn = getdate()  
  
 IF @ReturnCodeID ='17'  
 BEGIN  
  SET @TrxDescription = 'Represented Cheque Chq No. ' + CAST(@ChequeID as VARCHAR) + ' From Bnk ' + @BankID  
 END  
 --SET @TrxBranchID = dbo.f_GetHOBranchID((Select BankID from t_SystemBankSetting))  
  
  
 DECLARE @ProductTypeID varchar(5)  
 SELECT @ProductTypeID=ProductTypeID  
 FROM t_Product (NOLOCK)  
 WHERE ProductID=@ProductID  
 SELECT @OurBankID = BankID  
 FROM t_SystemBankSetting (NOLOCK)  
  
 IF (@AccountTypeID = 'C')  
 BEGIN  
  SET @MainGLID = dbo.f_GetGLInterfaceAccountID(@OurBankID, @ProductID, @ProductTypeID, 'CONTROL_AC')  
 END  
 ELSE  
 BEGIN  
  SET @MainGLID = @AccountID  
 END  
  
 IF @ReturnCodeID NOT IN ('00','17') AND @TrxTypeID = 'ID' AND @VoucherCode <> '40'  
 BEGIN  
  
  SET @TrxDescription = 'Unpaid Chq No. : ' + cast (@ChequeID as varchar) + ' Rsn : ' + DBO.f_CRB_ReturnCodeDescriptions('ReturnCodeID',@ReturnCodeID,'T')  
 END  
 ELSE IF @ReturnCodeID NOT IN ('00','17') AND @TrxTypeID = 'ID' AND @VoucherCode ='40'  
  BEGIN  
   SET @TrxDescription = 'Unpaid DD Rsn : ' + DBO.f_CRB_ReturnCodeDescriptions('ReturnCodeID',@ReturnCodeID,'T')  
  END  
  ELSE IF @ReturnCodeID NOT IN ('00','17') AND @TrxTypeID = 'IC'  
   BEGIN  
    SET @TrxDescription = 'Unpaid IC Rsn : ' + DBO.f_CRB_ReturnCodeDescriptions('ReturnCodeID',@ReturnCodeID,'T')  
   END  
  
 SELECT @ModuleID = CASE WHEN @TrxTypeID = 'ID' THEN 3050  
                                                ELSE 3040 END  
 SELECT @LocalCurrencyID = dbo.f_GetLocalCurrencyID(@OurBranchID)  
  
 IF (@ProductID is Null Or @ProductID = '')  
 BEGIN  
  If @AccountTypeID = 'C'  
  BEGIN  
   Select @ProductID = ProductID  
   From t_AccountCustomer (NOLOCK)  
   Where OurBranchID = @OurBranchID And AccountID = @AccountID  
  END  
  IF @AccountTypeID = 'G'  
  BEGIN  
   Set @ProductID = 'GL'  
  END  
 END  
   
 IF @TrxCurrencyID = @LocalCurrencyID  
 BEGIN  
  SELECT @TrxAmount = @Amount  
  ,      @LocalAmount = @Amount  
  ,      @ExchangeRate = 1  
 END  
 ELSE  
 BEGIN  
 --SELECT @TrxCurrencyID,'Kamunya 2'  
  SELECT @MeanRate = MeanRate  
  ,      @RoundingID = RoundingID  
  FROM t_CurrencyRate(NOLOCK)  
  WHERE OurBranchID = @OurBranchID  
   AND CurrencyID = @TrxCurrencyID  
   AND RateTypeID = 'REV'  
  
  SELECT @TrxAmount = @Amount  
  ,      @LocalAmount = dbo.f_RoundAmount(@RoundingID, (@TrxAmount * @MeanRate))  
  ,      @ExchangeRate = @MeanRate  
 END  
  
  
   
 SELECT @TrxPending = dbo.f_GetAccountTrxPen(@OurBranchID,@AccountID)  
 IF EXISTS(SELECT TOP 1 AccountID  
  FROM t_TrxValueDated (NOLOCK)  
  WHERE OurBranchID = @OurBranchID AND AccountID = @AccountID AND ChequeID = @ChequeID  
   AND ValueDate >= @TrxDate AND isNull(IsCleared,0) = 0 AND ClearedDate IS NULL AND isNull(ClearingStatusID,'') NOT IN ('C', 'D') AND @ReturnCodeID NOT IN ('00','17')  
  ORDER BY ChequeDate DESC)  
 BEGIN  
  
  --forgotten select  
  --SELECT 1,* FROM t_accountcustomer where  OurBranchID = @OurBranchID AND AccountID = @AccountID  
  
  --SELECT  
  --TrxRowID,AccountID,Amount  
  --FROM t_TrxValuedated  
  --WHERE OurBranchID = @OurBranchID AND ValueDate <= @TrxDate  
  --AND IsCleared = 0 AND ClearedDate IS NULL AND ClearingStatusID NOT IN ('C','D')  
  
  UPDATE t_AccountCustomer  
  SET UnClearBalance = UnClearBalance - @Amount  
  ,   ClearBalance   = ClearBalance + @Amount  
  WHERE OurBranchID = @OurBranchID AND AccountID = @AccountID  
  --SELECT 2,* FROM t_accountcustomer where  OurBranchID = @OurBranchID AND AccountID = @AccountID  
  
  UPDATE t_TrxValueDated  
  SET IsCleared        = 1  
  ,   ClearedDate      = @TrxDate  
  ,   ClearingStatusID = 'C'  
  WHERE OurBranchID = @OurBranchID AND  
   AccountID = @AccountID AND  
   ChequeID = @ChequeID AND  
   isNull(IsCleared,0) = 0 AND  
   ClearedDate IS NULL AND  
   isNull(ClearingStatusID,'') NOT IN ('C', 'D') AND  
   @ReturnCodeID NOT IN ('00','17')  
 END  
  
 IF EXISTS(SELECT TOP 1 AccountID  
  FROM t_TrxValueDated (NOLOCK)  
  WHERE OurBranchID = @OurBranchID AND AccountID = @AccountID AND ChequeID = @ChequeID  
   AND ValueDate <= @TrxDate AND isNull(IsCleared,0) = 0 AND ClearedDate IS NULL AND isNull(ClearingStatusID,'') NOT IN ('C', 'D')  
  ORDER BY ChequeDate DESC)  
 BEGIN  
  UPDATE t_TrxValueDated  
  SET IsCleared        = 1  
  ,   ClearedDate   = @TrxDate  
  ,   ClearingStatusID = 'C'  
  WHERE OurBranchID = @OurBranchID AND  
   AccountID = @AccountID AND  
   ChequeID = @ChequeID AND  
   isNull(IsCleared,0) = 0 AND  
   ClearedDate IS NULL AND  
   isNull(ClearingStatusID,'') NOT IN ('S', 'C', 'D')  
 END  
  
 IF NOT EXISTS(SELECT t_Transaction.TrxRowID  
  FROM       t_Transaction (NOLOCK)  
  Inner Join t_TrxClearing (NOLOCK) ON t_Transaction.TrxRowID = t_TrxClearing.TrxRowID  
  WHERE t_Transaction.OurBranchID=@OurBranchID  
   AND t_Transaction.AccountID=@AccountID  
   AND t_Transaction.ChequeID=@ChequeID  
   AND ABS(t_Transaction.TrxAmount) = ABS(@Amount)  
   AND ModuleID IN ('3050','3040')  
   AND TrxTypeID In ('ID','IC')  
   And ReturnCodeID = '00'  
   And DeletedOn Is Null  
   And t_TrxClearing.ColumnID=@TrxRowID)  
  
 BEGIN  
  IF NOT EXISTS(SELECT t_Transaction.TrxRowID  
   FROM       t_Transaction (NOLOCK)  
   Inner Join t_TrxClearing (NOLOCK) ON t_Transaction.TrxRowID = t_TrxClearing.TrxRowID  
   WHERE t_Transaction.OurBranchID=@OurBranchID  
    AND t_Transaction.AccountID=@AccountID  
    AND t_Transaction.ChequeID=@ChequeID  
    AND ABS(t_Transaction.TrxAmount) = ABS(@Amount)  
    AND ModuleID IN ('3050','3040')  
    AND TrxTypeID IN ('ID','IC')  
    And ReturnCodeID <> '00'  
    And DeletedOn Is Null  
    And t_TrxClearing.ColumnID=@TrxRowID)--Kamunya  
  BEGIN  
   IF ISNULL(@ImageID,'')=''  
   BEGIN  
    SET @ImageID = 0  
    SET @UniqueImageID = @ImageID  
   END  
  
   Declare @ChqID Int  
  
   SET @ChqID = CASE WHEN @TrxTypeID='ID' THEN @ChequeID  
                                          ELSE 0 END  
   BEGIN  
    --Transaction Amount Posting  
    --InterBranch Posting  
    --select @TrxDescriptionID  
    IF Exists(SELECT 1  
     FROM t_SystemBankSetting  
     Where ShortName <> 'FTB')  
    BEGIN  
     IF @ReturnCodeID NOT IN ('00','17') AND @TrxTypeID ='ID'--AND @AccountTypeID = 'C'  
     BEGIN  
      IF @ReturnCodeID = '63'  
      BEGIN  
       SELECT @TrxDescriptionID = '083'  
      END  
      ELSE IF @ReturnCodeID = '62'  
       BEGIN  
        SELECT @TrxDescriptionID = '084'  
       END  
       ELSE IF @ReturnCodeID IN ('31','32','33','37','38','40','41','42','43','53','54','55','58','64','79')  
        BEGIN  
         SELECT @TrxDescriptionID = '085'  
        END  
        ELSE  
        BEGIN  
         SELECT @TrxDescriptionID = '004'  
        END  
     END  
     IF @TrxTypeID IN ('IC') AND @AccountTypeID = 'C' AND @ReturnCodeID NOT IN ('00','17')  
     BEGIN  
      SET @TrxDescriptionID = '089'  
     END  
    END  
    --customized for paramount  
    IF @OurBankID = '50'  
    BEGIN  
     SET @TrxBranchID = @OurBranchID  
    END  
  
    IF @TrxBranchID <> @OurBranchID  
    BEGIN  
     IF @CreatedBy ='SYS'  
     BEGIN  
      SET @TrxFlagID = ''  
     END  
     --IF @TrxTypeID = 'ID'  
     --BEGIN  
     ----SET @TrxFlagID = ''  
     ----SET @SupervisedBy = 'IDCLEARING'  
     ----SET @SupervisedOn = @TrxDate  
     --END  
     --ELSE  
     --BEGIN  
     ----SET @TrxFlagID = 'U'  
     --SET @TrxFlagID = ''  
     --END  
  
     SELECT @HOBranchID = dbo.f_GetHOBranchID(@OurBankID)  
     IF (SELECT 1  
      FROM dbo.t_SystemBankParameter (NOLOCK)  
      WHERE BankID = @OurBankID AND SysParamID = 17) = 1  
     BEGIN  
      SET @IsHOPostingRequired = 1  
     END  
     ELSE  
     BEGIN  
      SET @IsHOPostingRequired = 0  
     END  
     SELECT @HOTrxDate = dbo.f_GetWorkingDate(@HOBranchID)--dbo.f_GetTrxPostingDate(@HOBranchID)  
     SELECT @DestinationTrxDate = dbo.f_GetWorkingDate(@OurBranchID)--dbo.f_GetTrxPostingDate(@OurBranchID)  
  
     IF ISNULL(@HOTrxDate, @TrxDate) <> @TrxDate OR @DestinationTrxDate <> @TrxDate  
     BEGIN  
      IF (SELECT dbo.f_SystemBankParameterExists(@OurBankID, 34)) <> 1  
      BEGIN  
       RAISERROR('BREXDB301003', 16, 1)  
       RETURN  
      END  
     END  
     IF @IsHOPostingRequired = 1  
     BEGIN  
      IF (@HOTrxDate <> @TrxDate) OR (@DestinationTrxDate <> @TrxDate) OR (@HOTrxDate <> @DestinationTrxDate)  
      BEGIN  
       RAISERROR('BREXDB301003', 16, 1)  
       RETURN  
      END  
  
        
      IF @IsCreditTrx = 1  
      BEGIN  
  
       SELECT @SourceIBGLID = AccountID  
       FROM dbo.t_GLInterBranch (NOLOCK)  
       WHERE OurBranchID = @HOBranchID AND CurrencyID = @TrxCurrencyID AND AccountTagID = 'IB_PBLE_AC'  
       SELECT @HOIBDebitGLID = AccountID  
       FROM dbo.t_GLInterBranch (NOLOCK)  
       WHERE OurBranchID = @TrxBranchID AND CurrencyID = @TrxCurrencyID AND AccountTagID = 'IB_RBLE_AC'  
  
       SELECT @HOIBCreditGLID = AccountID  
       FROM dbo.t_GLInterBranch (NOLOCK)  
       WHERE OurBranchID = @OurBranchID AND CurrencyID = @TrxCurrencyID AND AccountTagID = 'IB_PBLE_AC'  
       SELECT @DestinationIBGLID = AccountID  
       FROM dbo.t_GLInterBranch (NOLOCK)  
       WHERE OurBranchID = @HOBranchID AND CurrencyID = @TrxCurrencyID AND AccountTagID = 'IB_RBLE_AC'  
      END  
      ELSE --- Is a debit trx  
      BEGIN  
       SELECT @SourceIBGLID = AccountID  
       FROM dbo.t_GLInterBranch (NOLOCK)  
       WHERE OurBranchID = @HOBranchID AND CurrencyID = @TrxCurrencyID AND AccountTagID = 'IB_RBLE_AC'  
       SELECT @HOIBCreditGLID = AccountID  
       FROM dbo.t_GLInterBranch (NOLOCK)  
       WHERE OurBranchID = @TrxBranchID AND CurrencyID = @TrxCurrencyID AND AccountTagID = 'IB_PBLE_AC'  
       SELECT @HOIBDebitGLID = AccountID  
       FROM dbo.t_GLInterBranch (NOLOCK)  
       WHERE OurBranchID = @OurBranchID AND CurrencyID = @TrxCurrencyID AND AccountTagID = 'IB_RBLE_AC'  
       SELECT @DestinationIBGLID = AccountID  
       FROM dbo.t_GLInterBranch (NOLOCK)  
       WHERE OurBranchID = @HOBranchID AND CurrencyID = @TrxCurrencyID AND AccountTagID = 'IB_PBLE_AC'  
      END  
      --Select @SourceIBGLID,@HOIBCreditGLID,@HOIBDebitGLID,@DestinationIBGLID  
      IF ISNULL(@SourceIBGLID, '') = '' OR ISNULL(@HOIBCreditGLID, '') = '' OR ISNULL(@HOIBDebitGLID, '') = '' OR ISNULL(@DestinationIBGLID, '') = ''  
      BEGIN  
       RAISERROR(N'BREXDB817051', 16, 1)  
       RETURN  
      END  
     END  
     ELSE -- No HO Posting  
     BEGIN  
      IF @DestinationTrxDate <> @TrxDate  
      BEGIN  
       RAISERROR('BREXDB301003', 16, 1)  
       RETURN  
      END  
      IF @IsCreditTrx = 1  
      BEGIN  
       SELECT @SourceIBGLID = AccountID  
       FROM dbo.t_GLInterBranch (NOLOCK)  
       WHERE OurBranchID = @TrxBranchID AND CurrencyID = @TrxCurrencyID AND AccountTagID = 'IB_RBLE_AC'  
       SELECT @DestinationIBGLID = AccountID  
       FROM dbo.t_GLInterBranch (NOLOCK)  
       WHERE OurBranchID = @OurBranchID AND CurrencyID = @TrxCurrencyID AND AccountTagID = 'IB_PBLE_AC'  
      END  
      ELSE  
      BEGIN  
       SELECT @SourceIBGLID = AccountID  
       FROM dbo.t_GLInterBranch (NOLOCK)  
       WHERE OurBranchID = @TrxBranchID AND CurrencyID = @TrxCurrencyID AND AccountTagID = 'IB_RBLE_AC'  
       SELECT @DestinationIBGLID = AccountID  
       FROM dbo.t_GLInterBranch (NOLOCK)  
       WHERE OurBranchID = @OurBranchID AND CurrencyID = @TrxCurrencyID AND AccountTagID = 'IB_PBLE_AC'  
      END  
      IF ISNULL(@SourceIBGLID, '') = '' OR ISNULL(@DestinationIBGLID, '') = ''  
      BEGIN  
       RAISERROR(N'BREXDB817051', 16, 1)  
       RETURN  
      END  
     END  
  
     IF @AccountID = dbo.f_GetCurrencyBranchGLAccountID(@TrxBranchID,ISNULL(@TrxCurrencyID,'KES'), 'ACP_CLR_SUSP_AC')  
     BEGIN  
      SET @TrxFlagID = ''  
     END  
     --ELSE  
     --BEGIN  
     --IF @TrxTypeID = 'ID'  
     --BEGIN  
     --SET @TrxFlagID = ''  
     ----SET @SupervisedBy = 'IDCLEARING'  
     ----SET @SupervisedOn = @TrxDate  
     --END  
     --ELSE  
     --BEGIN  
     ----SET @TrxFlagID = 'U'  
     --SET @TrxFlagID = ''  
     --END  
     --END  
  
     --1. Post to the IB GL of Trx Branch  
     EXEC dbo.p_GetNextTrxSerialNo @OurBranchID     = @TrxBranchID  
     ,                             @NextTrxSerialNo = @SerialID OUTPUT  
     ,                             @TrxSerialTypeID = 'TR'  
  
     SELECT @TempTrxDescID = @TrxDescriptionID  
     ,      @TempTrxDesc = @TrxDescription  
     ,      @TempTrxTypeID = @TrxTypeID  
  
     SET @ValueDatedd = CASE WHEN @TempTrxTypeID = 'TD' THEN @TrxDate  
                                                        ELSE @ValueDate END  
     SET @UniqueImageID = @ImageID  
  
  
     IF isNull(@TrxDate,'')=''  
     BEGIN  
      SELECT @TrxDate = dbo.f_GetWorkingDate(@TrxBranchID)  
     END  
  
  
     EXEC dbo.p_InsertTransactions @PTrxBranchID      = @TrxBranchID  
     ,  
     --@PTrxRowID = @NewRowID OUTPUT,  
                                   @PTrxBatchID       = @TrxBatchID OUTPUT  
     ,                             @PSerialID         = @SerialID  
     ,                             @POurBranchID      = @TrxBranchID  
     , --@OurBranchID,  
                                   @PAccountTypeID    = 'G'  
     ,                             @PAccountID        = @SourceIBGLID  
     ,                             @PProductID        = 'GL'  
     ,                             @PModuleID         = @ModuleID  
     ,                             @PTrxCodeID        = 16  
     ,                             @PTrxTypeID        = @TempTrxTypeID  
     ,                             @PTrxDate          = @TrxDate  
     ,                             @PValueDate        = @ValueDatedd  
     ,                             @PAmount           = @Amount  
     ,                             @PLocalAmount      = @LocalAmount  
     ,                             @PTrxCurrencyID    = @TrxCurrencyID  
     ,                             @PTrxAmount        = @TrxAmount  
     ,                             @PExchangeRate     = @ExchangeRate  
     ,                             @PMeanRate         = @MeanRate  
     ,                             @PInstrumentTypeID = @InstrumentTypeID  
     ,                             @PChequeID         = @ChequeID  
     ,                             @PChequeDate       = @ChequeDate  
     ,                             @PReferenceNo      = @ReferenceNo  
     ,                             @PRemarks          = @Remarks  
     ,                             @PTrxDescriptionID = @TempTrxDescID  
     ,                             @PTrxDescription   = @TempTrxDesc  
     ,                             @PContraGLID       = @ContraGLID  
     ,                             @PTrxFlagID        = ''  
     , --@TrxFlagID,  
                                   @PImageID          = @AutoGenerateImgID  
     ,--@UniqueImageID,  
                                   @PTrxPrinted       = 0  
     ,                             @PIsTrxPending     = 0  
     ,                             @PCreatedBy        = @CreatedBy  
     ,  @SupervisedBy      = @SupervisedBy  
     ,                             @SupervisedOn      = @SupervisedOn  
  
     IF @ErrorCode IS NOT NULL  
     BEGIN  
      RAISERROR(@ErrorCode, 16, 1)  
      RETURN  
     END  
  
     SET @UniqueImageID = @NewRowID  
  
     UPDATE dbo.t_Transaction  
     SET ImageID = @AutoGenerateImgID  
     WHERE TrxRowID = @NewRowID  
  
     --2. Destination Branch IB posting  
     SELECT @TempTrxDescID = CASE WHEN @TrxTypeID = 'IC' THEN '008'  
                                                         ELSE '007' END  
     ,      @TempTrxDesc = @TrxDescription  
     ,      @TempTrxTypeID = CASE WHEN @TrxTypeID = 'IC' THEN 'TD'  
                                                         ELSE 'TC' END  
  
     SET @TempTrxDesc = @TempTrxDesc + ' For Batch :' + @TrxBatchID  
     EXEC dbo.p_GetNextTrxSerialNo @OurBranchID     = @OurBranchID  
     ,                             @NextTrxSerialNo = @SerialID OUTPUT  
     ,                             @TrxSerialTypeID = 'TR'  
     SET @ValueDatedd = CASE WHEN @TempTrxTypeID = 'TD' THEN @TrxDate  
                                                        ELSE @ValueDate END  
     SET @UniqueImageID = @ImageID  
  
     IF isNull(@TrxDate,'')=''  
     BEGIN  
      SELECT @TrxDate = dbo.f_GetWorkingDate(@TrxBranchID)  
     END  
     ----Destination  Barnch Posting  
     EXEC dbo.p_InsertTransactions @PTrxBranchID      = @TrxBranchID  
     ,               @PTrxBatchID       = @TrxBatchID  
     ,                             @PSerialID         = @SerialID  
     ,                             @POurBranchID      = @OurBranchID  
     ,                             @PAccountTypeID    = 'G'  
     ,                             @PAccountID        = @DestinationIBGLID  
     ,                             @PModuleID         = @ModuleID  
     ,                             @PTrxCodeID        = 16  
     ,                             @PTrxTypeID        = @TempTrxTypeID  
     ,                             @PTrxDate          = @TrxDate  
     ,                             @PValueDate        = @ValueDatedd  
     ,                             @PAmount           = @Amount  
     ,                             @PLocalAmount      = @LocalAmount  
     ,                             @PTrxCurrencyID    = @TrxCurrencyID  
     ,                             @PTrxAmount        = @TrxAmount  
     ,                             @PExchangeRate     = @ExchangeRate  
     ,                             @PMeanRate         = @MeanRate  
     ,                             @PTrxDescriptionID = @TempTrxDescID  
     ,                             @PTrxDescription   = @TempTrxDesc  
     ,                             @PContraGLID       = NULL  
     ,                             @PTrxFlagID        = ''  
     , --@TrxFlagID,  
                                   @PImageID          = @AutoGenerateImgID  
     ,--@UniqueImageID,  
                                   @PTrxPrinted       = 0  
     ,                             @PIsTrxPending     = 0  
     ,                             @PCreatedBy        = 'SYS'  
     ,                             @PRemarks          = ''  
     ,                             @SupervisedBy      = @SupervisedBy  
     ,                             @SupervisedOn      = @SupervisedOn  
  
     IF @ErrorCode IS NOT NULL  
     BEGIN  
      RAISERROR(@ErrorCode, 16, 1)  
      ROLLBACK TRAN POSTINWARDS  
      RETURN  
     END  
  
     SELECT @TempTrxDescID = CASE WHEN @TrxTypeID = 'IC' THEN '007'  
                                                         ELSE '008' END  
     ,      @TempTrxDesc = @TrxDescription  
     ,      @TempTrxTypeID = CASE WHEN @TrxTypeID = 'IC' THEN 'TC'  
                                                         ELSE 'TD' END  
  
     SET @TempTrxDesc = @TempTrxDesc + ' For Batch :' + @TrxBatchID  
     SET @ValueDatedd = CASE WHEN @TempTrxTypeID = 'TD' THEN @TrxDate  
                                                        ELSE @ValueDate END  
  
     IF isNull(@DestinationTrxDate,'')=''  
     BEGIN  
      SELECT @DestinationTrxDate = dbo.f_GetWorkingDate(@TrxBranchID)  
     END  
  
  
     ----Post Customer/GL  
     EXEC dbo.p_InsertTransactions @PTrxBranchID      = @TrxBranchID  
     ,                             @PTrxRowID         = @NewRowID OUTPUT  
     ,                             @PTrxBatchID       = @TrxBatchID  
     ,                             @PSerialID         = @SerialID  
     ,                             @POurBranchID      = @OurBranchID  
     ,                             @PAccountTypeID    = @AccountTypeID  
     ,       @PAccountID   = @AccountID  
     ,                             @PProductID        = @ProductID  
     ,                             @PModuleID         = @ModuleID  
     ,                             @PTrxCodeID        = 0  
     , --this is to indicate actual transaction  
                                   @PTrxTypeID        = @TempTrxTypeID  
     ,                             @PTrxDate          = @DestinationTrxDate  
     ,                             @PValueDate        = @ValueDatedd  
     ,                             @PAmount           = @Amount  
     ,                             @PLocalAmount      = @LocalAmount  
     ,                             @PTrxCurrencyID    = @TrxCurrencyID  
     ,                             @PTrxAmount        = @TrxAmount  
     ,                             @PExchangeRate     = @ExchangeRate  
     ,             @PMeanRate     = @MeanRate  
     ,                             @PInstrumentTypeID = @InstrumentTypeID  
     ,                             @PChequeID         = @ChequeID  
     ,                             @PChequeDate       = @ChequeDate  
     ,                             @PReferenceNo      = @ReferenceNo  
     ,                             @PRemarks          = @Remarks  
     ,                             @PTrxDescriptionID = @TempTrxDescID  
     ,                             @PTrxDescription   = @TempTrxDesc  
     ,                             @PMainGLID         = @MainGLID  
     ,                             @PContraGLID       = NULL  
     ,                             @PTrxFlagID        = ''  
     ,                             @PImageID          = @AutoGenerateImgID  
     ,--@UniqueImageID,  
                                   @PTrxPrinted       = @TrxPrinted  
     ,                             @PIsTrxPending     = 0  
     ,                             @PCreatedBy        = @CreatedBy  
     ,                             @SupervisedBy      = @SupervisedBy  
     ,                             @SupervisedOn      = @SupervisedOn  
  
     IF @ErrorCode IS NOT NULL  
     BEGIN  
      RAISERROR(@ErrorCode, 16, 1)  
      ROLLBACK TRAN POSTINWARDS  
      RETURN  
     END  
  
     --SELECT @NewRowID = TrxRowID FROM dbo.T_transaction (NOLOCK)  
     --WHERE TrxBatchID = @TrxBatchID AND OurBranchID = @OurBranchID  
  
     IF @IsHOPostingRequired = 1  
     BEGIN  
      SELECT @TempTrxTypeID = CASE WHEN @IsCreditTrx = 1 THEN 'TC'  
                                                         ELSE 'TD' END  
      ,      @TempTrxDescID = CASE WHEN @IsCreditTrx = 1 THEN '007'  
                                                         ELSE '008' END  
  
      --@TempTrxDesc = dbo.f_GetTrxDescription(@TempTrxDescID) + ',' + @OurBranchID + '-' +  @AccountID + '-' + @AccountTypeID  
      SET @TempTrxDesc = @TempTrxDesc + ' For Batch :' + @TrxBatchID  
  
      EXEC dbo.p_GetNextTrxSerialNo @OurBranchID     = @HOBranchID  
      ,                             @NextTrxSerialNo = @SerialID OUTPUT  
      ,                             @TrxSerialTypeID = 'TR'  
  
      SET @ValueDatedd = CASE WHEN @TempTrxTypeID = 'TD' THEN @TrxDate  
                                                         ELSE @ValueDate END  
  
      IF isNull(@HOTrxDate,'')=''  
      BEGIN  
       SELECT @HOTrxDate = dbo.f_GetWorkingDate(@TrxBranchID)  
      END  
  
      EXEC dbo.p_InsertTransactions @PTrxBranchID      = @TrxBranchID  
      ,                             @PTrxBatchID       = @TrxBatchID  
      ,         @PSerialID         = @SerialID  
      ,         @POurBranchID      = @HOBranchID  
      ,                             @PAccountTypeID    = 'G'  
      ,                             @PAccountID        = @HOIBCreditGLID  
      ,                             @PModuleID         = @ModuleID  
      ,                             @PTrxCodeID        = 16  
      ,                             @PTrxTypeID        = @TempTrxTypeID  
      ,                             @PTrxDate   = @HOTrxDate  
      ,         @PValueDate        = @ValueDatedd  
      ,                             @PAmount           = @Amount  
      ,                             @PLocalAmount      = @LocalAmount  
      ,                             @PTrxCurrencyID    = @TrxCurrencyID  
      ,                             @PTrxAmount        = @TrxAmount  
      ,                             @PExchangeRate     = @ExchangeRate  
      ,                             @PTrxDescriptionID = @TempTrxDescID  
      ,                             @PTrxDescription   = @TempTrxDesc  
      ,                             @PContraGLID       = NULL  
      ,                             @PTrxFlagID        = ''  
      , --@TrxFlagID,  
                                    @PImageID          = @AutoGenerateImgID  
      ,--@UniqueImageID,  
                                    @PTrxPrinted       = @TrxPrinted  
      ,                             @PIsTrxPending     = 0  
      ,                           @PCreatedBy        = 'SYS'  
      ,                             @SupervisedBy      = @SupervisedBy  
      ,                             @SupervisedOn      = @SupervisedOn  
  
      IF @ErrorCode IS NOT NULL  
      BEGIN  
       RAISERROR(@ErrorCode, 16, 1)  
       ROLLBACK TRAN POSTINWARDS  
       RETURN  
      END  
      SELECT @TempTrxTypeID = CASE WHEN @IsCreditTrx = 1 THEN 'TD'  
                                                         ELSE 'TC' END  
      ,      @TempTrxDescID = CASE WHEN @IsCreditTrx = 1 THEN '008'  
                                                         ELSE '007' END  
      --@TempTrxDesc = dbo.f_GetTrxDescription(@TempTrxDescID) + ',' + @OurBranchID + '-' +  @AccountID + '-' + @AccountTypeID  
      SET @TempTrxDesc = @TempTrxDesc + ' For Batch :' + @TrxBatchID  
      SET @ValueDatedd = CASE WHEN @TempTrxTypeID = 'TD' THEN @TrxDate  
                                                         ELSE @ValueDate END  
  
      IF isNull(@HOTrxDate,'')=''  
      BEGIN  
       SELECT @HOTrxDate = dbo.f_GetWorkingDate(@TrxBranchID)  
      END  
  
      EXEC dbo.p_InsertTransactions @PTrxBranchID      = @TrxBranchID  
      ,                             @PTrxBatchID       = @TrxBatchID  
      ,                             @PSerialID         = @SerialID  
      ,                             @POurBranchID      = @HOBranchID  
      ,                             @PAccountTypeID    = 'G'  
      ,                             @PAccountID        = @HOIBDebitGLID  
      ,                             @PModuleID         = @ModuleID  
      ,                             @PTrxCodeID        = 16  
      ,                             @PTrxTypeID        = @TempTrxTypeID  
      ,                             @PTrxDate          = @HOTrxDate  
      ,                             @PValueDate        = @ValueDatedd  
      ,                             @PAmount           = @Amount  
      ,                             @PLocalAmount      = @LocalAmount  
      ,                             @PTrxCurrencyID    = @TrxCurrencyID  
      ,                             @PTrxAmount        = @TrxAmount  
      ,                             @PExchangeRate     = @ExchangeRate  
      ,                             @PTrxDescriptionID = @TempTrxDescID  
      ,                             @PTrxDescription   = @TempTrxDesc  
      ,                             @PContraGLID       = NULL  
      ,                             @PTrxFlagID        = ''  
      , --@TrxFlagID,  
                                    @PImageID          = @AutoGenerateImgID  
      ,-- @UniqueImageID,  
               @PTrxPrinted       = @TrxPrinted  
      ,                             @PIsTrxPending     = 0  
      ,                             @PCreatedBy        = 'SYS'  
      ,                             @SupervisedBy      = @SupervisedBy  
      ,                             @SupervisedOn      = @SupervisedOn  
  
      IF @ErrorCode IS NOT NULL  
      BEGIN  
       RAISERROR(@ErrorCode, 16, 1)  
       ROLLBACK TRAN POSTINWARDS  
       RETURN  
      END  
     END  
    END  
  
    -- Local Branch Posting  
    ELSE ---IF @TrxBranchID = @OurBranchID  
    BEGIN  
      
     IF @AccountID = dbo.f_GetCurrencyBranchGLAccountID(@OurBranchID,@TrxCurrencyID, 'ACP_CLR_SUSP_AC')  
     BEGIN  
      SET @TrxFlagID = ''  
      --SET @SupervisedBy = 'SYS'  
      SET @SupervisedOn = @TrxDate  
     END  
  
  
     IF @AccountTypeID = 'C' AND @TrxTypeID ='IC'  
     BEGIN  
      SELECT @TrxDescriptionID = '089'  
     END  
     IF isNull(@TrxDate,'')=''  
     BEGIN  
      SELECT @TrxDate = dbo.f_GetWorkingDate(@TrxBranchID)  
     END  
  
     EXEC p_InsertTransactions @PTrxBranchID      = @TrxBranchID  
     ,                         @PTrxRowID         = @NewRowID OUTPUT  
     ,                         @PTrxBatchID       = @TrxBatchID OUTPUT  
     ,                         @PSerialID         = @SerialID OUTPUT  
     ,                         @POurBranchID      = @OurBranchID  
     ,        @PAccountTypeID    = @AccountTypeID  
     ,                         @PAccountID        = @AccountID  
     ,                         @PProductID        = @ProductID  
     ,                         @PModuleID         = @ModuleID  
     ,                         @PTrxCodeID        = @TrxCodeID  
     ,                         @PTrxTypeID        = @TrxTypeID  
     ,                         @PTrxDate          = @TrxDate  
     ,                         @PValueDate        = @ValueDate  
     ,                         @PAmount           = @Amount  
     ,                         @PLocalAmount      = @LocalAmount  
     ,                         @PTrxCurrencyID    = @TrxCurrencyID  
     ,                         @PTrxAmount        = @TrxAmount  
     ,                         @PExchangeRate     = @ExchangeRate  
     ,                         @PMeanRate         = @ExchangeRate  
     ,                         @PInstrumentTypeID = @InstrumentTypeID  
     ,                         @PChequeID         = @ChqID  
     ,                         @PChequeDate       = @ChequeDate  
     ,                         @PReferenceNo      = @TrxRowID  
     ,                         @PRemarks          = @Remarks  
     ,                         @PTrxDescriptionID = @TrxDescriptionID  
     ,                         @PTrxDescription   = @TrxDescription  
     ,                         @PMainGLID         = @MainGLID  
     ,                         @PContraGLID       = @ContraGLID  
     ,                         @PTrxFlagID        = @TrxFlagID  
     ,                         @PIsTrxPending     = @TrxPending  
     ,                         @PImageID          = @AutoGenerateImgID  
     ,--@ImageID,  
                               @PTrxPrinted       = @TrxPrinted  
     ,                         @PCreatedBy        = @CreatedBy  
     ,                         @PForwardRemark    = @ForwardRemark  
     ,                         @SupervisedBy      = @SupervisedBy  
     ,                         @SupervisedOn      = @SupervisedOn  
  
     --select @NewRowID--sanjay  
     IF @@ERROR > 0  
     BEGIN  
      RETURN  
     END  
     --SET @ImageID = @NewRowID  
    END  
   END  
  
   IF isNull(@NewRowID,0) = 0  
   BEGIN  
    SELECT @NewRowID = TrxRowID  
    FROM dbo.T_transaction (NOLOCK)  
    WHERE TrxBatchID = @TrxBatchID AND AccountTypeID ='C'  
   END  
   --select @NewRowID, @ReturnCodeID, @TrxTypeID  
   INSERT INTO t_TrxClearing ( TrxRowID, TrxBranchID, TrxBatchID, TrxBatchSLNo, OurBranchID, AccountTypeID, AccountID,   
   Amount, ChequeDigit, VoucherCode, ReturnCodeID, Commission, TheirCommission, BankID, BranchID, DrawerOrPayeeAccountID,   
   DrawerOrPayee, CurrencyID, ValueDate, ChequeDate, ChequeID, TrxType, [date], ImageID, ColumnID )  
   SELECT @NewRowID  
   ,      @TrxBranchID  
   ,      @TrxBatchID  
   ,      1  
   ,      @OurBranchID  
   ,      @AccountTypeID  
   ,      @AccountID  
   ,      CASE WHEN @TrxTypeID ='ID' THEN -1 * @Amount  
                                     ELSE @Amount END  
   ,      @ChequeDigit  
   ,      @VoucherCode  
   ,      @ReturnCodeID  
   ,    @Commission  
   ,    @TheirCommission  
   ,      @BankID  
   ,      @BranchID  
   ,      @DrawerOrPayeeAccountID  
   ,      @DrawerOrPayee  
   ,      @TrxCurrencyID  
   ,      @ValueDate  
   ,      @TrxDate  
   ,      @ChequeID  
   ,      @TrxTypeID  
   ,      @TrxDate  
   ,      @AutoGenerateImgID  
   ,--@ImageID,  
          @TrxRowID  
   --select CONVERT(VARCHAR(15),@TrxDate,106)--sanj  
   SELECT @ActualBankID = BankID  
   ,      @ActualBranchID = BranchID  
   ,      @ExtraDetails = ExtraDetails  
   ,      @OriginatorCode = OriginatorCode  
   ,      @OriginatorRef = OriginatorRef  
   ,      @Policy1 = Policy1  
   ,      @Policy2 = Policy2  
   FROM t_IncomingTransactions (NOLOCK)  
   WHERE ColumnID = @TrxRowID  
  
  
   IF @AccountID <> dbo.f_GetCurrencyBranchGLAccountID(@OurBranchID,@TrxCurrencyID, 'CL_BANKERCQL_AC')  
   BEGIN  
    IF @AccountID <> dbo.f_GetCurrencyBranchGLAccountID(@OurBranchID, @TrxCurrencyID, 'ACP_CLR_SUSP_AC' )  
    BEGIN  
     IF @TrxTypeID <> 'IC'  
     BEGIN  
      IF NOT EXISTS(SELECT 1  
       FROM t_ChequeTrx  
       WHERE OurBranchID = @OurBranchID AND AccountID = @AccountID AND ChequeID = @ChequeID AND @AccountTypeID <> 'G')  
      BEGIN  
       INSERT INTO t_chequetrx  
       SELECT @OurBranchID  
       ,      @AccountTypeID  
       ,      @AccountID  
       ,      '60'  
       ,      @ChequeID  
       ,      @TrxDate  
       ,      'P'  
       ,      @NewRowID  
      END  
     END  
    END  
   END  
  
   --UPDATE t_TrxClearing  
   --SET BankID = @ActualBankID, BranchID = @ActualBranchID, VatName = @ExtraDetails,  
   --PolicyNumber1 = @Policy1, PolicyNumber2 = @Policy2,  
   --OrigRefCode = @OriginatorRef, ORIGINATORCODE = @OriginatorCode  
   --WHERE ColumnID = @TrxRowID AND TrxType = 'IC'  
  
   --UPDATE t_TrxClearing  
   --SET BankID = @ActualBankID, BranchID = @ActualBranchID  
   --WHERE ColumnID = @TrxRowID AND TrxType = 'ID' And ReturnCodeID NOT IN ('17','00')  
  
   SET @TrxFlagID = ISNULL(@TrxFlagID, '')  
  
   --UPDATE t_TrxClearing SET BankID = t_IncomingTransactions.BankID FROM t_IncomingTransactions  
   --WHERE  t_TrxClearing.ColumnID = @TrxRowID AND t_IncomingTransactions.TrxType='ID'  
  
   --Select @ReturnCodeID  
  
   -- IF @ReturnCodeID NOT IN ('17','00')  
   -- BEGIN  
  
   --select  
   -- @OurBranchID,  
   -- @AccountID,  
   -- @TrxDescriptionID,  
   -- @TrxCurrencyID,  
   -- @ModuleID,  
   -- @Amount,  
   -- @CreatedBy,  
   -- @TrxBatchID,  
   -- @SerialID,  
   -- @TrxDescriptionID--,  
   -- --@ErrorCode  
   ------sanjay  
   --EXEC p_ChargeTransaction  
   -- @OurBranchID = @OurBranchID,  
   -- @AccountID = @AccountID,  
   -- @TrxDescriptionID = @TrxDescriptionID,  
   -- @TrxCurrencyID = @TrxCurrencyID,  
   -- @ModuleID = @ModuleID,  
   -- @CreatedBy = @CreatedBy,  
   -- @ChargeID = @TrxDescriptionID,  
   -- @ErrorNo = @ErrorCode OUTPUT  
  
   --sp_helptext p_ChargeTransaction  
  
  
  
   --IIFEUR INWARD INSUFFICIENT FUNDS EUR  
   --IIFGBP INWARD INSUFFICIENT FUNDS GBP  
   --IIFKES INWARD INSUFFICIENT FUNDS KES  
   --IIFUSD INWARD INSUFFICIENT FUNDS USD  
   DECLARE @ChargeID Varchar(10)  
   IF @TrxDescriptionID ='083' AND @TrxCurrencyID ='USD' AND @TrxTypeID IN ('ID', 'TD')  
   BEGIN  
    SELECT @ChargeID = 'IIFUSD'  
   END  
   ELSE IF @TrxDescriptionID ='083' AND @TrxCurrencyID ='GBP' AND @TrxTypeID IN ('ID', 'TD')  
    BEGIN  
     SELECT @ChargeID = 'IIFGBP'  
    END  
    ELSE IF @TrxDescriptionID ='083' AND @TrxCurrencyID ='EUR' AND @TrxTypeID IN ('ID', 'TD')  
     BEGIN  
      SELECT @ChargeID = 'IIFEUR'  
     END  
     ELSE IF @TrxDescriptionID ='083' AND @TrxCurrencyID ='KES' AND @TrxTypeID IN ('ID', 'TD')  
      BEGIN  
       SELECT @ChargeID = 'IIFKES'  
      END  
   IF @TrxTypeID='IC'  
   BEGIN  
    SELECT @TrxDescriptionID ='089'  
   END  
   --IEFTKES INWARD EFT CHARGE KES  
   IF @TrxDescriptionID ='089' AND @TrxCurrencyID ='TZS' AND @TrxTypeID IN ('IC', 'TC')  
   BEGIN  
    SELECT @ChargeID = 'IEFTKES'  
   END  
  
   --IUEEUR INWARD UNCLEAR EFFECTS EUR  
   --IUEGBP INWARD UNCLEAR EFFECTS GBP  
   --IUEKES INWARD UNCLEAR EFFECTS KES  
   --IUEUSD INWARD UNCLEAR EFFECTS USD  
   IF @TrxDescriptionID ='084' AND @TrxCurrencyID ='USD' AND @TrxTypeID IN ('ID', 'TD')  
   BEGIN  
    SELECT @ChargeID = 'IUEUSD'  
   END  
   ELSE IF @TrxDescriptionID ='084' AND @TrxCurrencyID ='GBP' AND @TrxTypeID IN ('ID', 'TD')  
    BEGIN  
     SELECT @ChargeID = 'IUEGBP'  
    END  
    ELSE IF @TrxDescriptionID ='084' AND @TrxCurrencyID ='EUR' AND @TrxTypeID IN ('ID', 'TD')  
     BEGIN  
      SELECT @ChargeID = 'IUEEUR'  
     END  
     ELSE IF @TrxDescriptionID ='084' AND @TrxCurrencyID ='TZS' AND @TrxTypeID IN ('ID', 'TD')  
      BEGIN  
       SELECT @ChargeID = 'IUEKES'  
      END  
   --IATREUR INWARD TECHNICAL EUR  
   --IATRGBP INWARD TECHNICAL GBP  
   --IATRKES INWARD TECHNICAL KES  
   --IATREUSD INWARD TECHNICAL USD  
   IF @TrxDescriptionID ='085' AND @TrxCurrencyID ='USD' AND @TrxTypeID IN ('ID', 'TD')  
   BEGIN  
    SELECT @ChargeID = 'IATRUSD'  
   END  
   ELSE IF @TrxDescriptionID ='085' AND @TrxCurrencyID ='GBP' AND @TrxTypeID IN ('ID', 'TD')  
    BEGIN  
     SELECT @ChargeID = 'IATRGBP'  
    END  
    ELSE IF @TrxDescriptionID ='085' AND @TrxCurrencyID ='EUR' AND @TrxTypeID IN ('ID', 'TD')  
     BEGIN  
      SELECT @ChargeID = 'IATREUR'  
     END  
     ELSE IF @TrxDescriptionID ='085' AND @TrxCurrencyID ='TZS' AND @TrxTypeID IN ('ID', 'TD')  
      BEGIN  
       SELECT @ChargeID = 'IATRKES'  
      END  
  
   EXEC p_ChargeTransaction @OurBranchID      = @OurBranchID  
   ,                        @AccountID        = @AccountID  
   ,                        @TrxDescriptionID = @TrxDescriptionID  
   ,                        @TrxAmount        = @Amount  
   ,                        @TrxDate          = @TrxDate  
   ,  
   --@TrxTypeID  = 'TC',--@TrxTypeID,  
                            @TrxCurrencyID    = @TrxCurrencyID  
   ,                        @ModuleID         = @ModuleID  
   ,                        @CreatedBy        = @CreatedBy  
   ,                        @TrxBatchID       = @TrxBatchID  
   ,                        @SerialID         = @SerialID  
   ,                        @ChargeID         = @ChargeID  
   ,                        @WaiveCharge      = 0  
   ,                        @ErrorNo          = @ErrorCode OUTPUT  
  
  
   IF ISNULL(@ErrorCode ,'0') <> '0'  
   BEGIN  
    RAISERROR(N' BREXDB123456',16,1)  
   END  
   -- END  
  
   IF @ChequeID <> 0  
   BEGIN  
   --  
    INSERT INTO BRNET_ImageServer.dbo.t_IncomingChequeImages ( ImageID, OurBranchID, TrxType, TFImage, JFImage, JRImage, UVImage, TFImageSize, JFImageSize, JRImageSize, BankId, OperatorID, CreatedOn, CurrencyID, Validity, AccountID, TrxDate, ChequeID )  
    SELECT @AutoGenerateImgID  
    ,--@ImageID,  
           OurBranchID  
    ,      TrxType  
    ,      FRONTBWIMAGE  
    ,      Frontgrayscaleimage  
    ,      REARIMAGE  
    ,      null  
    ,      '0'  
    ,      '0'  
    ,      '0'  
    ,      BankID  
    ,      @CreatedBy  
    ,      getDate()  
    ,      CurrencyID  
    ,      Validity  
    ,      CASE WHEN AccountType='G' THEN RTRIM(SUBSTRING(AccountId,3,7))  
                                     ELSE AccountID END  
    ,      Date  
    ,      ChequeID  
    FROM t_IncomingTransactions  
    WHERE ColumnID=@TrxRowID  
  
    UPDATE t_IRDReport  
    SET ImageID  = @AutoGenerateImgID  
    ,   TrxDate  = @TrxDate  
    ,   ColumnID = @TrxRowID  
    ,   TrxRowID = @NewRowID  
    WHERE ImageID = @TrxRowID  
   END  
  END  
 END  
  
 IF @ReturnCodeID IN ('17')  
 BEGIN  
  DELETE FROM t_ChequeTrx  
  WHERE OurBranchID = @OurBranchID AND AccountTypeID = @AccountTypeID  
   AND AccountID = @AccountID AND ChequeID = @ChequeID AND ISNULL(@ChequeID,0) <> 0  
 END  
  
 --Remove this items from Cheque Series since they are not our cheques.  
 IF @ReturnCodeID NOT IN ('00','17') AND @TrxTypeID = 'ID'  
 BEGIN  
  DELETE FROM t_ChequeTrx  
  WHERE OurBranchID = @OurBranchID  
   AND AccountTypeID = @AccountTypeID AND AccountID = @AccountID  
   AND ChequeID = @ChequeID AND ISNULL(@ChequeID,0) <> 0  
 END  
  
  
  
 ----Create a Notification for Inward Debit  
 IF @TrxTypeID = 'ID' And @VoucherCode <> '40' AND @ReturnCodeID IN ('00','17')   
 BEGIN  
  EXEC p_ProcessNotification  @NotificationTriggerType = 'TRX', @NotificationTriggerID= '3050',  
  @BankID = '55',@OurBranchID = @OurBranchID, @AccountID = @AccountID,@Amount = @TrxAmount,  
  @TrxBranchName= null,@ChequeNo= @ChequeID,@ChequeDate= @ChequeDate,  
  @DeviceLocation = NUll,@ExpiryDate= NUll,@InstallAmount= NUll,@InstallDate= NUll,  
  @LoanAmount= NUll,@WorkingDate= @TrxDate,@OperatorID= @CreatedBy,  
  @OperatorNames = NULL, @ReceiptAmount= NUll,@ReceiptNo = NUll,  
  @FromDate= NUll,@ToDate= NUll,@TrxType = @TrxTypeID--,  
  --@ImageID = @AutoGenerateImgID---@NotificationID= 'CN0016',  
 END  
  
 --Release Lock  
 DELETE FROM t_SystemRecordLocks  
 WHERE LockModuleID = CASE WHEN @AccountTypeID = 'C' THEN 1300  
             ELSE 8020 END  
  AND PKKey = '[OurBranchID:' + @OurBranchID + '][AccountID:' + @AccountID + ']'  
  
 --Checking if the cheque belongs to us, or if its an Outward that we are unpaying.  
 DECLARE @ClrTrxFlagID varchar(5)  
 SELECT @ClrTrxFlagID = TrxFlagID  
 FROM t_transaction  
 WHERE OurBranchID = @OurBranchID AND AccountID = @AccountID AND TrxBatchID = @TrxBatchID AND ChequeID = @ChequeID  
 IF (@ReturnCodeID<>'00' AND @TrxTypeID='ID' AND @ClrTrxFlagID='I')  
 BEGIN  
  UPDATE t_transaction  
  SET TrxFlagID      = ''  
  ,   TrxDescription = CONCAT ('Unpaid Cheque ',  
  @ChequeID,  
  ' Rsn : ' , dbo.f_CRB_ReturnCodeDescriptions('ReturnCodeID',RIGHT(@ReturnCodeID,2),'T'))  
  WHERE OurBranchID = @OurBranchID  
   AND AccountID = @AccountID  
   AND TrxBatchID = @TrxBatchID  
   AND ChequeID = @ChequeID  
  --Need to appropriately update the unsupervised debits coz this situation isnt handled in the trigger--  
  UPDATE t_AccountCustomer  
  SET UnSupervisedDebits = ISNULL(UnSupervisedDebits,0) + ISNULL(@TrxAmount,0)  
  WHERE OurBranchID = @OurBranchID AND AccountID = @AccountID  
 END  
  
 --Checking if the cheque belongs to us, or if its an Outward that we are unpaying.  
  
 END TRY  
  
 BEGIN CATCH  
 --SELECT ERROR_MESSAGE()  
 Declare @ErrorMessage varchar(4000)  
 SELECT @ErrorMessage = 'ERROR: Process: ' + IsNull(ERROR_PROCEDURE(),'') + ', Line:' + CONVERT(nvarchar, IsNull(ERROR_LINE(),'')) + ', ' + IsNull(ERROR_MESSAGE(),'')  
 Print @ErrorMessage  
  
 ROLLBACK TRAN POSTINWARDS  
 --RAISERROR (@ErrorMessage, 16, 1)  
  
 IF ISNULL (@ErrorCode,'0') = '0'  
  SET @ErrorCode = '455004'  
  
 RETURN  
 END CATCH  
  
 COMMIT TRAN POSTINWARDS  
 SET NOCOUNT OFF  
  
  
END  
  