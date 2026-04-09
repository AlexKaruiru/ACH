CREATE PROCEDURE [dbo].[p_AddInwardTrx]     
(    
 @TrxBranchID   BranchID,    
 @TrxBatchID    VarChar(8) OUTPUT,    
 @SerialID    Int =0    OUTPUT,    
 @OurBranchID   BranchID,    
 @AccountTypeID   Char(1),    
 @AccountID    AccountID,    
 @ProductID    ProductID,    
     
 @ModuleID    SmallInt,    
 @TrxCodeID    TinyInt, --Not required    
 @TrxTypeID    Char(2),    -- Inward Credit 'IC' and inward debit 'ID'    
 @TrxDate    SmallDateTime,    
 @ValueDate    SmallDateTime,    
    
 @Amount     Amount,    
 @TrxCurrencyID   CurrencyID,    
     
 @InstrumentTypeID  Char(1),     -- It will be voucher 'V'    
 @ChequeID    Int,      -- Instrument ID    
 @ChequeDate    SmallDateTime = Null,  -- Instrument date    
 @ReferenceNo   nVarChar(15) = Null,    
 @Remarks    Remarks = Null,    
    
 @TrxDescriptionID  nVarChar(6),    -- 'IC' = '004',  'ID' = '005'    
 @TrxDescription   Description,    
 @MainGLID    AccountID,    
 @ContraGLID    AccountID = Null,   -- Validate in BAL leyer.     
 @TrxFlagID    SystemSubID = '',    
 @ImageID    Int = Null,    
 @TrxPrinted    TinyInt = 0,    
    
 --Include By Kamunya    
 @ChequeDigit   Char(1)=0,    
 @VoucherCode   Char(2)=0,    
 @ReturnCodeID   Char(4)=0,    
 @Commission    Amount=0,    
 @TheirCommission  Amount=0,    
 @VATPINNo    nVarChar(12)=null,    
 @VATPAYType    nVarChar(10)=null,    
 @VATSerialNo   nVarChar(7)=null,    
 @VATPAYEMonth   nVarChar(6)=null,    
 @VATPAYECommission  Amount=0,    
 ---------------------------------    
 @BankID     BankID,    
 @BranchID    BranchID,    
 @DrawerOrPayeeAccountID AccountID,    
 @DrawerOrPayee   Names,    
 @CreatedBy    OperatorID,    
 @NewRecord     TinyInt,    
 @TrxRowID    BigInt,    
 @ForwardRemark varchar(max)=null     
)
     
AS    
BEGIN    
 SET NOCOUNT ON    
    
 DECLARE @NewRowID  BigInt,    
   @ChequeStatus Char(1),    
   @ErrorCode  VarChar(15),    
   @TrxPending   Bit,    
   @LocalCurrencyID CurrencyID,    
   @TrxAmount   Amount,    
   @LocalAmount  Amount,    
   @ExchangeRate  Rate,    
   @ErrorNo   VarChar(50),    
   @MeanRate   Rate,    
   @Profit    Amount,    
   @RoundingID   Systemsubid,    
   @ExtraDetails  VarChar(50),    
   @OriginatorCode  VarChar(4),    
   @OriginatorRef  VarChar(25),    
   @Policy1   VarChar(25),    
   @Policy2   VarChar(25),    
   @BRBankID  VarChar(25),    
   @AvailableBalance  dbo.Amount,    
   @FreezeAmount  dbo.Amount,    
   @ClientID            dbo.ClientID,    
   @ChargingCurrencyID Currencyid,    
   @IsTaxable   Bit,    
   @IsTaxOnTrxAmount Bit,    
   @TaxID    NVarChar(6),    
   @CessID    NVarChar(6),    
   @ChargingMethodID Char(4),    
   @ChargeTrxDescriptionID VarChar(5),    
   @ChargeTrxDescription VarChar(300),    
   @CustomerTrxDescriptionID VarChar(5),    
   @CustomerTrxNarration VarChar(300),    
   @GLAccountID Accountid,    
   @AccountClassID VarChar(15) ,    
   @OurBranchWorkingDate SmallDateTime,    
   @CurrencyID   Currencyid,    
   @CommissionAmount Amount,    
   @LCommissionAmount Amount,    
   @TaxAmount   Amount,    
   @CessAmount   Amount,    
   @MainRowID    BIGINT = 0,    
   @strString    VarChar(Max),    
   @PValueDate    SmallDateTime,    
   @IsCreditTrx   Bit,    
   @IsHOPostingRequired Bit,    
   @HOBranchID    dbo.BranchID,    
   @HOTrxDate    SmallDateTime,    
   @DestinationTrxDate  SmallDateTime,    
   @DestinationIBGLID  dbo.AccountID,    
   @SourceIBGLID   dbo.AccountID,    
   @HOIBCreditGLID   dbo.AccountID,    
   @HOIBDebitGLID   dbo.AccountID,    
   @DescriptionID   NVarChar(6),    
   @TempAccountID   dbo.AccountID,    
   @TempTrxTypeID   Char(2),    
   @TempTrxDescID   NVarChar(6),    
   @TempTrxDesc   dbo.Description,    
   @UniqueImageID   BigInt,    
   @OriginalAmount   dbo.Amount,    
   @OurBankID    dbo.BankID,    
   @ValueDatedd   DateTime, 
   @OrigCode    VarChar(4),    
   @OrigRef    VarChar(25),    
   @ColumnID    VarChar(15),    
   @SetClearingDays  Int,    
   @DeleteTrxRowID   BigInt,    
   @DeleteChequeDate  DateTime,    
   @DeleteBranchID   VarChar(5),    
   @DeleteAccountID  VarChar(25),    
   @DeleteChequeID   Int,    
   @ValueCapingAmt   dbo.Amount,    
   @ChargeTrxDesc   dbo.Description,    
   @AccountCurrencyID  dbo.CurrencyID,    
   @LChargeAmount   dbo.Amount, --Local Amount      
   @LTaxAmount    dbo.Amount,    
   @LCessAmount   dbo.Amount,    
   @ChargeCurrencyID  dbo.CurrencyID,    
   @TotalCharge   dbo.Amount,    
   @TaxPercent    dbo.Amount,    
   @TrxDesc    dbo.Description,    
   @Day  nVarchar(3),    
   @Time nVarchar(8),    
   @AutoGenerateImgID  Int=0,    
   @ActualBranchID  BranchID,    
   @ActualBankID  BankID,
   @SupervisedBy OperatorID,
   @SupervisedOn  datetime
           
  SET @Day = DAY  ( GETDATE() )    
  SET @Time = REPLACE((SELECT CONVERT(VARCHAR(8), GETDATE(), 108) 'hh:mi:ss'), ':', '')    
        --SELECT @Day + @Time    
  SET @AutoGenerateImgID = @Day + @Time      
  IF @TrxDate <> dbo.f_GetWorkingDate(@OurBranchID)    
  BEGIN    
   RAISERROR('BREXDB300032',16,1)    
   RETURN    
  END 
  
  IF @VoucherCode ='40'     
  BEGIN
	SET @ChequeID = 999999
  END
  
  SET @SupervisedBy = NULL
  SET @SupervisedOn = NULL

  IF @ReturnCodeID ='17'    
  BEGIN    
   SET @TrxDescription = 'Represented Cheque Chq No. ' + CAST(@ChequeID as VARCHAR) + ' From Bnk ' + @BankID    
  END    
  SET @TrxBranchID = dbo.f_GetHOBranchID((Select BankID from t_SystemBankSetting))   
   
  --Validate Cheque    
  --Validate Invalid Cheques    
  IF (@TrxTypeID ='ID' and @InstrumentTypeID <>'V' and @AccountID <>  dbo.f_GetCurrencyBranchGLAccountID(@TrxBranchID,@TrxCurrencyID, 'CUR_CLR_AC_IN'))    
  BEGIN   
   IF NOT EXISTS (SELECT Top 1 * FROM  t_ChequeBook     
       WHERE OurbranchID =  @OurBranchID    
       AND AccountID = @AccountID    
       AND @ChequeID   BETWEEN ChequeStart AND ChequeEnd    
       AND @TrxTypeID ='ID' AND (@ReturnCodeID ='00' OR @ReturnCodeID IS NULL)    
       AND (@VoucherCode <> '40' OR @VoucherCode IS NULL))    
   BEGIN    
    RAISERROR('BREXDB8670066',16,1)    
    RETURN    
   END    
  END    
    
  ----Validate Stop Payment    
  IF EXISTS ( SELECT ChequeID FROM  t_ChequeTrx     
     WHERE OurbranchID = @OurBranchID     
     AND AccountID = @AccountID  And (@VoucherCode <> '40' OR @VoucherCode IS NULL)    
     AND @ChequeID = ChequeID AND ChequeStatusID = 'S'    
     AND @TrxTypeID ='ID' AND (@ReturnCodeID ='00' OR @ReturnCodeID IS NULL))    
  BEGIN    
   RAISERROR('BREXDB8670067',16,1)    
   RETURN    
  END    
       
  ----Validate Cheque Paid Once    
  IF EXISTS (SELECT ChequeID FROM  t_ChequeTrx     
  WHERE OurbranchID = @OurBranchID    
  AND AccountID = @AccountID And (@VoucherCode <> '40' OR @VoucherCode IS NULL)    
  AND ChequeID = @ChequeID AND ChequeStatusID = 'P'    
  AND @TrxTypeID ='ID' AND (@ReturnCodeID ='00' OR @ReturnCodeID IS NULL))    
  BEGIN    
   RAISERROR('BREXDB8670068',16,1)    
   RETURN    
  END    
     
 DECLARE @ProductTypeID varchar(5)    
 SELECT @ProductTypeID=ProductTypeID FROM t_Product WHERE ProductID=@ProductID    
 SELECT @OurBankID = BankID FROM t_SystemBankSetting    
 IF(@AccountTypeID = 'C')    
   BEGIN    
    SET @MainGLID = dbo.f_GetGLInterfaceAccountID(@OurBankID, @ProductID, @ProductTypeID, 'CONTROL_AC')    
   END    
  ELSE    
   BEGIN    
    SET @MainGLID = @AccountID    
   END    
     
    IF @ReturnCodeID NOT IN ('00','17') AND @TrxTypeID = 'ID' AND @VoucherCode <> '40'
	BEGIN    
		SET @TrxDescription = 'Unpaid Chq No. : ' + cast (@ChequeID as varchar) + ' Rsn : ' +  DBO.f_CRB_ReturnCodeDescriptions('ReturnCodeID',@ReturnCodeID,'T') 
	END
	ELSE IF @ReturnCodeID NOT IN ('00','17') AND @TrxTypeID = 'ID' AND @VoucherCode ='40'
	BEGIN
		SET @TrxDescription = 'Unpaid DD Rsn : ' +  DBO.f_CRB_ReturnCodeDescriptions('ReturnCodeID',@ReturnCodeID,'T') 
	END
    ELSE IF @ReturnCodeID NOT IN ('00','17') AND @TrxTypeID = 'IC' 
	BEGIN
		SET @TrxDescription = 'Unpaid IC Rsn : ' +  DBO.f_CRB_ReturnCodeDescriptions('ReturnCodeID',@ReturnCodeID,'T') 
	END

 SELECT @ModuleID = CASE WHEN @TrxTypeID = 'ID' THEN 3050 ELSE 3040 END    
 SELECT @LocalCurrencyID = dbo.f_GetLocalCurrencyID(@OurBranchID)    
     
 --Is Data already edited by another user (using UpdateCount)    
 IF (SELECT dbo.f_IsAccountEdited(@OurBranchID,@AccountTypeID,@AccountID,@NewRecord)) =1    
 BEGIN    
  RAISERROR(N'BREXDB005305',16,1)    
  RETURN    
 END    
 IF (@ProductID is Null Or @ProductID = '')     
 Begin    
  If @AccountTypeID = 'C'    
  Begin    
   Select @ProductID = ProductID     
   From t_AccountCustomer    
   Where OurBranchID = @OurBranchID    
   And AccountID = @AccountID    
  End    
  If @AccountTypeID = 'G'    
  Begin    
   Set @ProductID = 'GL'    
  End    
  End    
    
 IF @TrxCurrencyID = @LocalCurrencyID    
 BEGIN    
  SELECT    
   @TrxAmount  = @Amount,    
   @LocalAmount = @Amount,    
   @ExchangeRate = 1     
 END    
 ELSE    
 BEGIN    
  SELECT @MeanRate   = MeanRate,     
    @RoundingID = RoundingID     
  FROM t_CurrencyRate    
  WHERE OurBranchID = @OurBranchID    
   AND CurrencyID = @TrxCurrencyID    
   AND RateTypeID = 'REV'    
      
  SELECT    
   @TrxAmount  = @Amount,    
   @LocalAmount = dbo.f_RoundAmount(@RoundingID, (@TrxAmount * @MeanRate)),    
   @ExchangeRate = @MeanRate     
 END    
    
 SELECT @TrxPending = dbo.f_GetAccountTrxPen(@OurBranchID,@AccountID)    
     
     
 IF EXISTS(SELECT TOP 1 AccountID    
       FROM t_TrxValueDated    
       WHERE Ourbranchid   = @OurBranchID AND    
       AccountID   = @AccountID AND    
       ChequeID   = @ChequeID AND    
       ValueDate   <= @TrxDate AND    
       IsCleared   = 0 AND    
       ClearedDate   IS NULL AND    
       ClearingStatusID NOT IN ('C', 'D')    
       ORDER BY ChequeDate DESC)    
    BEGIN    
     UPDATE t_TrxValuedated    
     SET    
     IsCleared   = 1,    
     ClearedDate   = @TrxDate,    
     ClearingStatusID = 'C'    
     WHERE OurBranchID = @OurBranchID AND    
        AccountID  = @AccountID AND    
        ChequeID  = @ChequeID AND    
        IsCleared  = 0 AND    
        ClearedDate IS NULL AND    
        ClearingStatusID NOT IN ('S', 'C', 'D')    
    END    
     
 IF NOT EXISTS(SELECT t_Transaction.TrxRowID FROM t_Transaction     
      Inner Join t_TrxClearing    
      On t_Transaction.TrxRowID = t_TrxClearing.TrxRowID    
      WHERE t_Transaction.OurBranchID=@OurBranchID     
      AND t_Transaction.AccountID=@AccountID      
      AND  t_Transaction.ChequeID=@ChequeID    
      AND ABS(t_Transaction.TrxAmount) = ABS(@Amount)    
      AND ModuleID IN ('3050','3040')    
      AND TrxTypeID In ('ID','IC')    
      And ReturnCodeID = '00'    
      And DeletedOn Is Null    
      And t_TrxClearing.ColumnID=@TrxRowID)--Kamunya    
          
 BEGIN        
  IF NOT EXISTS(SELECT t_Transaction.TrxRowID FROM t_Transaction     
      Inner Join t_TrxClearing    
      On t_Transaction.TrxRowID = t_TrxClearing.TrxRowID    
      WHERE t_Transaction.OurBranchID=@OurBranchID     
      AND t_Transaction.AccountID=@AccountID      
      AND  t_Transaction.ChequeID=@ChequeID     
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
       
   SET @ChqID = CASE WHEN @TrxTypeID='ID' THEN @ChequeID ELSE 0 END    
   BEGIN    
   --Transaction Amount Posting    
   --InterBranch Posting    
     
     
   IF @ReturnCodeID NOT IN ('00','17') AND @AccountTypeID = 'C'  
   BEGIN  
  IF @ReturnCodeID = '63'  
   BEGIN  
    SET @TrxDescriptionID = '083'  
   END   
  ELSE IF @ReturnCodeID  = '62'  
   BEGIN  
    SET @TrxDescriptionID = '084'  
   END   
  ELSE  
   BEGIN  
    SET @TrxDescriptionID = '085'  
   END   
    
   END  

   --customized for Maendeleo
   IF @OurBankID = '51' AND @ModuleID = '3050'
   BEGIN
		select @ContraGLID = dbo.f_GetCurrencyBranchGLAccountID(@TrxBranchID,@TrxCurrencyID, 'CUR_CLR_AC_IN')
   END
   
   
   IF @TrxBranchID <> @OurBranchID    
   BEGIN    
    IF @CreatedBy ='SYS'  
    BEGIN    
		SET @TrxFlagID = ''    
    END
   IF @TrxTypeID = 'ID' 
		 BEGIN
			SET @TrxFlagID = ''
			SET @SupervisedBy = 'IDCLEARING'
		    SET @SupervisedOn = @TrxDate
		 END
	ELSE
		BEGIN
			SET @TrxFlagID = 'U'
		END
         
    SELECT @HOBranchID = dbo.f_GetHOBranchID(@OurBankID)    
    IF (SELECT 0    
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
      --select @HOTrxDate ,@TrxDate, @DestinationTrxDate      
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
      ELSE  --- Is a debit trx      
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
      IF ISNULL(@SourceIBGLID, '') = '' OR    
         ISNULL(@HOIBCreditGLID, '') = '' OR    
         ISNULL(@HOIBDebitGLID, '') = '' OR    
         ISNULL(@DestinationIBGLID, '') = ''    
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
    IF @AccountID = dbo.f_GetCurrencyBranchGLAccountID(@TrxBranchID,ISNULL(@TrxCurrencyID,'UGX'), 'ACP_CLR_SUSP_AC')     
		BEGIN     
			SET @TrxFlagID = ''     
		END    
    ELSE    
		BEGIN    
		 IF @TrxTypeID = 'ID' 
			 BEGIN
				SET @TrxFlagID = ''
				SET @SupervisedBy = 'IDCLEARING'
			    SET @SupervisedOn = @TrxDate
			 END
		ELSE
			BEGIN
				SET @TrxFlagID = 'U'
			END  
		END    
    --1. Post to the IB GL of Trx Branch      
    EXEC dbo.p_GetNextTrxSerialNo @OurBranchID = @TrxBranchID,    
    @NextTrxSerialNo = @SerialID OUTPUT,    
    @TrxSerialTypeID = 'TR'  
      
    IF ISNULL(@DrawerOrPayee, '') <> '' 
    BEGIN
        IF @TrxTypeID = 'IC'
            SET @TrxDescription = ISNULL(@TrxDescription, '') + ' | From: ' + LTRIM(RTRIM(@DrawerOrPayee))
        ELSE IF @TrxTypeID = 'ID'
            SET @TrxDescription = ISNULL(@TrxDescription, '') + ' | To: ' + LTRIM(RTRIM(@DrawerOrPayee))
    END
    
    SELECT @TempTrxDescID = @TrxDescriptionID,      
    @TempTrxDesc = @TrxDescription,      
    @TempTrxTypeID = @TrxTypeID    
    SET @ValueDatedd = CASE    
            WHEN @TempTrxTypeID = 'TD' THEN @TrxDate    
            ELSE @ValueDate    
           END    
               
               
    SET @UniqueImageID = @ImageID    
        
    EXEC dbo.p_InsertTransactions    
    @PTrxBranchID = @TrxBranchID,    
    @PTrxRowID = @NewRowID OUTPUT,    
    @PTrxBatchID = @TrxBatchID OUTPUT,    
    @PSerialID = @SerialID,    
    @POurBranchID = @TrxBranchID, --@OurBranchID,      
    @PAccountTypeID = 'G',    
    @PAccountID = @SourceIBGLID,    
    @PProductID = 'GL',    
    @PModuleID = @ModuleID,    
    @PTrxCodeID = @TrxCodeID,    
    @PTrxTypeID = @TempTrxTypeID,    
    @PTrxDate = @TrxDate,    
    @PValueDate = @ValueDatedd,    
    @PAmount = @Amount,    
    @PLocalAmount = @LocalAmount,    
    @PTrxCurrencyID = @TrxCurrencyID,    
    @PTrxAmount = @TrxAmount,    
    @PExchangeRate = @ExchangeRate,    
    @PMeanRate = @MeanRate,    
    @PInstrumentTypeID = @InstrumentTypeID,    
    @PChequeID = @ChequeID,    
    @PChequeDate = @ChequeDate,    
    @PReferenceNo = @ReferenceNo,    
    @PRemarks = @Remarks,    
    @PTrxDescriptionID = @TempTrxDescID,    
    @PTrxDescription = @TempTrxDesc,    
    @PContraGLID = @ContraGLID,    
    @PTrxFlagID = @TrxFlagID,    
    @PImageID = @AutoGenerateImgID,--@UniqueImageID,    
    @PTrxPrinted = 0,    
    @PIsTrxPending = 0,    
    @PCreatedBy = @CreatedBy,
    @SupervisedBy = @SupervisedBy,
    @SupervisedOn = @SupervisedOn

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
    SELECT @TempTrxDescID = CASE    
           WHEN @TrxTypeID = 'IC' THEN '008'    
           ELSE '007'    
          END,    
    @TempTrxDesc = @TrxDescription,    
    @TempTrxTypeID = CASE    
          WHEN @TrxTypeID = 'IC' THEN 'TD'    
          ELSE 'TC'    
         END    
    SET @TempTrxDesc = @TempTrxDesc + ' For Batch :' + @TrxBatchID    
    EXEC dbo.p_GetNextTrxSerialNo @OurBranchID = @OurBranchID,    
    @NextTrxSerialNo = @SerialID OUTPUT,    
    @TrxSerialTypeID = 'TR'    
    SET @ValueDatedd = CASE    
            WHEN @TempTrxTypeID = 'TD' THEN @TrxDate    
            ELSE @ValueDate    
           END    
    SET @UniqueImageID = @ImageID    
    ----Destination  Barnch Posting      
    EXEC dbo.p_InsertTransactions    
    @PTrxBranchID = @TrxBranchID,    
    @PTrxRowID = @NewRowID,    
    @PTrxBatchID = @TrxBatchID,    
    @PSerialID = @SerialID,    
    @POurBranchID = @OurBranchID,    
    @PAccountTypeID = 'G',    
    @PAccountID = @DestinationIBGLID,    
    @PModuleID = @ModuleID,    
    @PTrxCodeID = 16,    
    @PTrxTypeID = @TempTrxTypeID,    
    @PTrxDate = @TrxDate,    
    @PValueDate = @ValueDatedd,    
    @PAmount = @Amount,    
    @PLocalAmount = @LocalAmount,    
    @PTrxCurrencyID = @TrxCurrencyID,    
    @PTrxAmount = @TrxAmount,    
    @PExchangeRate = @ExchangeRate,    
    @PMeanRate = @MeanRate,    
    @PTrxDescriptionID = @TempTrxDescID,    
    @PTrxDescription = @TempTrxDesc,    
    @PContraGLID = NULL,    
    @PTrxFlagID = @TrxFlagID,    
    @PImageID = @AutoGenerateImgID,--@UniqueImageID,    
    @PTrxPrinted = 0,    
    @PIsTrxPending = 0,    
    @PCreatedBy = 'SYS',    
    @PRemarks = '',
    @SupervisedBy = @SupervisedBy,
    @SupervisedOn = @SupervisedOn
    
    IF @ErrorCode IS NOT NULL    
    BEGIN    
     RAISERROR(@ErrorCode, 16, 1)    
     --ROLLBACK TRAN    
     RETURN    
    END  
	
	
	  
    SELECT @TempTrxDescID = CASE    
           WHEN @TrxTypeID = 'IC' THEN '007'    
           ELSE '008'    
          END,    
    @TempTrxDesc = @TrxDescription,    
    @TempTrxTypeID = CASE    
          WHEN @TrxTypeID = 'IC' THEN 'TC'    
          ELSE 'TD'    
         END    
    SET @TempTrxDesc = @TempTrxDesc + ' For Batch :' + @TrxBatchID    
    SET @ValueDatedd = CASE    
            WHEN @TempTrxTypeID = 'TD' THEN @TrxDate    
            ELSE @ValueDate    
           END    
    ----Post Customer/GL      
    EXEC dbo.p_InsertTransactions    
    @PTrxBranchID = @TrxBranchID,    
    @PTrxRowID = @NewRowID,    
    @PTrxBatchID = @TrxBatchID,    
    @PSerialID = @SerialID,    
    @POurBranchID = @OurBranchID,    
    @PAccountTypeID = @AccountTypeID,    
    @PAccountID = @AccountID,    
    @PProductID = @ProductID,    
    @PModuleID = @ModuleID,    
    @PTrxCodeID = 0, --this is to indicate actual transaction      
    @PTrxTypeID = @TempTrxTypeID,    
    @PTrxDate = @DestinationTrxDate,    
    @PValueDate = @ValueDatedd,    
    @PAmount = @Amount,    
    @PLocalAmount = @LocalAmount,    
    @PTrxCurrencyID = @TrxCurrencyID,    
    @PTrxAmount = @TrxAmount,    
    @PExchangeRate = @ExchangeRate,    
    @PMeanRate = @MeanRate,    
    @PInstrumentTypeID = @InstrumentTypeID,    
    @PChequeID = @ChequeID,    
    @PChequeDate = @ChequeDate,    
    @PReferenceNo = @ReferenceNo,    
    @PRemarks = @Remarks,    
    @PTrxDescriptionID = @TempTrxDescID,    
    @PTrxDescription = @TempTrxDesc,    
    @PMainGLID = @MainGLID,    
    @PContraGLID = NULL,    
    @PTrxFlagID = @TrxFlagID,    
    @PImageID = @AutoGenerateImgID,--@UniqueImageID,    
    @PTrxPrinted = @TrxPrinted,    
    @PIsTrxPending = 0,    
    @PCreatedBy = @CreatedBy,
    @SupervisedBy = @SupervisedBy,
    @SupervisedOn = @SupervisedOn
    
    IF @ErrorCode IS NOT NULL    
    BEGIN    
     RAISERROR(@ErrorCode, 16, 1)    
     --ROLLBACK TRAN    
     RETURN    
    END    

	

    SELECT @NewRowID = TrxRowID    
    FROM dbo.t_transaction (NOLOCK)    
    WHERE TrxBatchID = @TrxBatchID AND OurBranchID = @OurBranchID    
    IF @IsHOPostingRequired = 1    
     BEGIN    
      SELECT @TempTrxTypeID = CASE    
             WHEN @IsCreditTrx = 1 THEN 'TC'    
             ELSE 'TD'    
            END,    
      @TempTrxDescID = CASE    
            WHEN @IsCreditTrx = 1 THEN '007'    
            ELSE '008'    
           END    
      --@TempTrxDesc = dbo.f_GetTrxDescription(@TempTrxDescID) + ',' + @OurBranchID + '-' +  @AccountID + '-' + @AccountTypeID      
      SET @TempTrxDesc = @TempTrxDesc + ' For Batch :' + @TrxBatchID    
      EXEC dbo.p_GetNextTrxSerialNo @OurBranchID = @HOBranchID,    
      @NextTrxSerialNo = @SerialID OUTPUT,    
      @TrxSerialTypeID = 'TR'    
      SET @ValueDatedd = CASE    
              WHEN @TempTrxTypeID = 'TD' THEN @TrxDate    
              ELSE @ValueDate    
             END    
      EXEC dbo.p_InsertTransactions    
      @PTrxBranchID = @TrxBranchID,    
      @PTrxBatchID = @TrxBatchID,    
      @PSerialID = @SerialID,    
      @POurBranchID = @HOBranchID,    
      @PAccountTypeID = 'G',    
      @PAccountID = @HOIBCreditGLID,    
      @PModuleID = @ModuleID,    
      @PTrxCodeID = 16,    
      @PTrxTypeID = @TempTrxTypeID,    
      @PTrxDate = @HOTrxDate,    
      @PValueDate = @ValueDatedd,    
      @PAmount = @Amount,    
      @PLocalAmount = @LocalAmount,    
      @PTrxCurrencyID = @TrxCurrencyID,    
      @PTrxAmount = @TrxAmount,    
      @PExchangeRate = @ExchangeRate,    
      @PTrxDescriptionID = @TempTrxDescID,    
      @PTrxDescription = @TempTrxDesc,    
      @PContraGLID = NULL,    
      @PTrxFlagID = @TrxFlagID,    
      @PImageID = @AutoGenerateImgID,--@UniqueImageID,    
      @PTrxPrinted = @TrxPrinted,    
      @PIsTrxPending = 0,    
      @PCreatedBy = 'SYS',
      @SupervisedBy = @SupervisedBy,
	  @SupervisedOn = @SupervisedOn
   
      IF @ErrorCode IS NOT NULL    
      BEGIN    
       RAISERROR(@ErrorCode, 16, 1)    
       --ROLLBACK TRAN    
       RETURN    
      END    

      SELECT @TempTrxTypeID = CASE    
             WHEN @IsCreditTrx = 1 THEN 'TD'    
             ELSE 'TC'    
            END,    
      @TempTrxDescID = CASE    
            WHEN @IsCreditTrx = 1 THEN '008'    
            ELSE '007'    
           END    
      --@TempTrxDesc = dbo.f_GetTrxDescription(@TempTrxDescID) + ',' + @OurBranchID + '-' +  @AccountID + '-' + @AccountTypeID      
SET @TempTrxDesc = @TempTrxDesc + ' For Batch :' + @TrxBatchID    
      SET @ValueDatedd = CASE    
              WHEN @TempTrxTypeID = 'TD' THEN @TrxDate    
              ELSE @ValueDate    
             END    
      EXEC dbo.p_InsertTransactions    
      @PTrxBranchID = @TrxBranchID,    
      @PTrxBatchID = @TrxBatchID,    
      @PSerialID = @SerialID,    
      @POurBranchID = @HOBranchID,    
      @PAccountTypeID = 'G',    
      @PAccountID = @HOIBDebitGLID,    
      @PModuleID = @ModuleID,    
      @PTrxCodeID = 16,    
      @PTrxTypeID = @TempTrxTypeID,    
      @PTrxDate = @HOTrxDate,    
      @PValueDate = @ValueDatedd,    
      @PAmount = @Amount,    
      @PLocalAmount = @LocalAmount,    
      @PTrxCurrencyID = @TrxCurrencyID,    
      @PTrxAmount = @TrxAmount,    
      @PExchangeRate = @ExchangeRate,    
      @PTrxDescriptionID = @TempTrxDescID,    
      @PTrxDescription = @TempTrxDesc,    
      @PContraGLID = NULL,    
      @PTrxFlagID = @TrxFlagID,    
      @PImageID =@AutoGenerateImgID,-- @UniqueImageID,    
      @PTrxPrinted = @TrxPrinted,    
      @PIsTrxPending = 0,    
      @PCreatedBy = 'SYS',
      @SupervisedBy = @SupervisedBy,
      @SupervisedOn = @SupervisedOn
    
      IF @ErrorCode IS NOT NULL    
      BEGIN    
       RAISERROR(@ErrorCode, 16, 1)    
       --ROLLBACK TRAN    
       RETURN    
      END    
     END    
       
       
   END    
      
   -- Local Branch Posting    
       
   IF @TrxBranchID = @OurBranchID    
    BEGIN 
    IF @AccountID = dbo.f_GetCurrencyBranchGLAccountID(@OurBranchID,@TrxCurrencyID, 'CUR_RET_AC')
    BEGIN
		SET @TrxFlagID = ''
		SET @SupervisedBy = 'SYS'
		SET @SupervisedOn = @TrxDate
    END   
    EXEC p_InsertTransactions     
     @PTrxBranchID = @TrxBranchID,    
     @PTrxRowID  = @NewRowID OUTPUT,    
     @PTrxBatchID = @TrxBatchID OUTPUT,    
     @PSerialID  = @SerialID OUTPUT,    
     @POurBranchID = @OurBranchID,    
     @PAccountTypeID = @AccountTypeID,    
     @PAccountID  = @AccountID,    
    @PProductID  = @ProductID,    
    
     @PModuleID  = @ModuleID,    
     @PTrxCodeID  = @TrxCodeID,    
     @PTrxTypeID  = @TrxTypeID,    
     @PTrxDate  = @TrxDate,    
     @PValueDate  = @ValueDate,    
    
     @PAmount  = @Amount,    
     @PLocalAmount = @LocalAmount,    
     @PTrxCurrencyID = @TrxCurrencyID,    
     @PTrxAmount  = @TrxAmount,    
     @PExchangeRate = @ExchangeRate,    
     @PMeanRate  = @ExchangeRate,    
    
     @PInstrumentTypeID = @InstrumentTypeID,    
     @PChequeID  = @ChqID,    
     @PChequeDate = @ChequeDate,    
     @PReferenceNo = @ReferenceNo,    
     @PRemarks  = @Remarks,    
    
     @PTrxDescriptionID = @TrxDescriptionID,    
     @PTrxDescription = @TrxDescription,    
     @PMainGLID  = @MainGLID,    
     @PContraGLID = @ContraGLID,    
     @PTrxFlagID  = @TrxFlagID,    
     @PIsTrxPending = @TrxPending,    
    
     @PImageID  = @AutoGenerateImgID,--@ImageID,    
     @PTrxPrinted = @TrxPrinted,    
     @PCreatedBy  = @CreatedBy,    
     @PForwardRemark = @ForwardRemark,
     @SupervisedBy = @SupervisedBy,
     @SupervisedOn = @SupervisedOn
    
    
     IF @@ERROR > 0    
     BEGIN    
      RETURN     
     END    
         
     SET @ImageID = @NewRowID    
    END        
   END    
       
     
    INSERT INTO t_TrxClearing    
    (    
     TrxRowID,    
     TrxBranchID,    
     TrxBatchID,    
     TrxBatchSLNo,    
     OurBranchID,    
     AccountTypeID,    
     AccountID,    
     Amount,    
     ChequeDigit,    
     VoucherCode,    
     ReturnCodeID,    
     Commission,    
     TheirCommission,    
    
     BankID,    
     BranchID,    
     DrawerOrPayeeAccountID,    
     DrawerOrPayee,    
     CurrencyID,    
     ValueDate,    
     ChequeDate,    
     ChequeID,    
     TrxType,    
     [date],    
     ImageID,    
     ColumnID    
    )    
    SELECT     
     @NewRowID,    
     @TrxBranchID,    
     @TrxBatchID,    
     1,    
     @OurBranchID,    
     @AccountTypeID,    
     @AccountID,    
     CASE WHEN @TrxTypeID ='ID' THEN -1 * @Amount ELSE @Amount END,    
     @ChequeDigit,    
     @VoucherCode,    
     @ReturnCodeID,    
     @Commission,    
     @TheirCommission,    
     @BankID,    
     @BranchID,    
     @DrawerOrPayeeAccountID,    
     @DrawerOrPayee,    
     @TrxCurrencyID,    
     @ValueDate,    
     @TrxDate,    
     @ChequeID,    
     @TrxTypeID,    
     @TrxDate,    
     @AutoGenerateImgID,--@ImageID,    
     @TrxRowID    
         
    

	
    SELECT @ActualBankID = BankID , @ActualBranchID = BranchID, @ExtraDetails = ExtraDetails, @OriginatorCode = OriginatorCode,    
        @OriginatorRef = OriginatorRef, @Policy1 = Policy1, @Policy2 = Policy2    
    FROM t_Trxinwards WHERE ColumnID = @TrxRowID    
 
    
  --  UPDATE t_TrxClearing   
	 --SET BankID = @ActualBankID, BranchID = @ActualBranchID, VatName = @ExtraDetails, PolicyNumber1 = @Policy1, PolicyNumber2 = @Policy2,     
  --   OrigRefCode = @OriginatorRef, ORIGINATORCODE = @OriginatorCode    
  --  WHERE ColumnID = @TrxRowID AND TrxType = 'IC'    
    
 


    UPDATE t_TrxClearing     
    SET BankID = @ActualBankID, BranchID = @ActualBranchID    
    WHERE ColumnID = @TrxRowID AND TrxType = 'ID' And ReturnCodeID NOT IN ('17','00')    
        
    SET @TrxFlagID = ISNULL(@TrxFlagID, '')     
        
        
    UPDATE  t_TrxClearing SET BankID = t_TrxInwards.BankID FROM t_Trxinwards     
    WHERE  t_TrxClearing.ColumnID = @TrxRowID AND t_Trxinwards.TrxType='ID'    
   
 IF @ReturnCodeID NOT IN ('17','00')   
 BEGIN  
 EXEC p_ChargeTransaction      
   @OurBranchID  = @OurBranchID,     
   @AccountID   = @AccountID,     
   @TrxDescriptionID = @TrxDescriptionID,  
   @TrxCurrencyID  = @TrxCurrencyID,     
   @ModuleID   = @ModuleID,  
   @CreatedBy   = @CreatedBy, 
   @ChargeID = @TrxDescriptionID,   
   @ErrorNo   = @ErrorCode OUTPUT     
   IF ISNULL(@ErrorCode ,'0') <> '0'    
   BEGIN    
   RAISERROR(N' BREXDB123456',16,1)    
   END  
 END   
    --INSERT INTO t_ChequeTrx    
    -- (    
    --  OurBranchID,    
    --  AccountTypeID,    
    --  AccountID,           
    --  ChequeID,    
    --  BankID,    
    --  ChequeDate,    
    --  ChequeStatusID,    
    --  TrxRowID    
    -- )   
   
      --BankID,    
      --@ChequeDate,    
      --'P',    
      --@TrxRowID    
          
    IF  @ChequeID <> 0    
    BEGIN    
     INSERT INTO t_IncomingChequeImages    
      (ImageID,    
      OurBranchID,    
      TrxType,    
      TFImage,    
      JFImage,    
      JRImage,    
      UVImage,    
      TFImageSize,    
      JFImageSize,    
      JRImageSize,    
      BankId,    
      OperatorID,    
      TFImageSignature,    
      JFImageSignature,    
      JRImageSignature,    
      CreatedOn,    
      CurrencyID,    
      Validity,    
      AccountID,    
      TrxDate,    
      ChequeID)    
     SELECT    
      @AutoGenerateImgID,--@ImageID,    
      OurBranchID,    
      TrxType,    
      FRONTBWIMAGE,     
      Frontgrayscaleimage,    
      REARIMAGE,    
      null,    
      '0','0','0',    
      BankID,    
      @CreatedBy,    
      FRONTBWIMAGESIGNATURE,    
      FRONTGRAYSCALEIMAGESIGNATURE,    
      REARIMAGESIGNATURE,    
      getDate(),    
      CurrencyID,    
      Validity,    
      CASE WHEN AccountType='G' THEN RTRIM(SUBSTRING(AccountId,3,7))ELSE AccountID END,    
      Date,    
      ChequeID    
     FROM t_TrxInwards     
     WHERE ColumnID=@TrxRowID     
         
     UPDATE t_IRDReport     
     SET  ImageID = @AutoGenerateImgID,    
       TrxDate = @TrxDate    
     WHERE ImageID = @TrxRowID    
         
         
         
    END    
        
  END    
 END    
 
IF @ReturnCodeID IN ('17')   
 BEGIN 
	DELETE 
	FROM t_ChequeTrx
	WHERE OurBranchID = @OurBranchID
		AND AccountTypeID = @AccountTypeID
		AND AccountID = @AccountID
		AND ChequeID = @ChequeID
		AND ISNULL(@ChequeID,0) <> 0 
 END
 
 --Remove this items from Cheque Series since they are not our cheques.
 IF @ReturnCodeID NOT IN ('00','17') AND @TrxTypeID = 'ID'   
 BEGIN 
	DELETE 
	FROM t_ChequeTrx
	WHERE OurBranchID = @OurBranchID
		AND AccountTypeID = @AccountTypeID
		AND AccountID = @AccountID
		AND ChequeID = @ChequeID
		AND ISNULL(@ChequeID,0) <> 0 
 END
 
 UPDATE t_TrxInwards     
 SET POST = 1     
 WHERE ColumnID=@TrxRowID 
 
 UPDATE  t_transaction SET  TrxFlagID = 'B' WHERE TrxTypeID = 'TD' 
		AND Amount < dbo.f_GetAvailableBalance(OurBranchID,AccountID) AND ChequeID = 999999
		AND AccountID <> dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,TrxCurrencyID, 'CLR_SUSP_AC')    
		
--IF EXISTS(SELECT 1 FROM t_Transaction NOLOCK WHERE ModuleID = 3050 AND TrxTypeID IN ('TD','TC') AND TrxFlagID  IN ('I','P','S','B') AND TrxBatchID = @TrxBatchID  )
--	BEGIN
--		UPDATE  t_transaction 
--		SET		ModuleID = 3030, 
--				supervisedby = null,
--				supervisedOn = Null,
--				supervisedby2 = 'IDCLEARING', 
--				supervisedon2 = @TrxDate
--		WHERE ModuleID = 3050 AND TrxTypeID IN ('TD','TC') AND TrxBatchID = @TrxBatchID 
		
--	END
    
 --Release Lock    
 DELETE FROM t_SystemRecordLocks    
 WHERE LockModuleID = CASE WHEN @AccountTypeID = 'C' THEN 1300 ELSE 8020 END    
  AND PKKey  = '[OurBranchID:' + @OurBranchID + '][AccountID:' + @AccountID + ']'    
 SET NOCOUNT OFF    
    
END    


