CREATE   PROCEDURE [dbo].[p_InsertTransactions]      
(       
 @PTrxBranchID   dbo.BranchID,      
 @PTrxRowID    BigInt = 0 OUTPUT,      
 @PTrxBatchID   VarChar(8) OUTPUT,      
 @PSerialID    Int = 0 OUTPUT,      
 @POurBranchID   dbo.BranchID,      
 @PAccountTypeID   Char(1),      
 @PAccountID    dbo.AccountID,      
 @PProductID    dbo.ProductID = NULL, --GL case not require to psss this      
 @PModuleID    SmallInt,      
 @PTrxCodeID    TinyInt,      
 @PTrxTypeID    Char(2),      
 @PTrxDate    SmallDateTime,      
 @PValueDate    SmallDateTime = NULL,      
 @PAmount    dbo.Amount = 0,      
 @PLocalAmount   dbo.Amount,      
 @PTrxCurrencyID   dbo.CurrencyID,      
 @PTrxAmount    dbo.Amount,      
 @PExchangeRate   dbo.CurrencyRate = 1,      
 @PMeanRate    dbo.CurrencyRate = 1,      
 @PProfit    dbo.Amount = 0,      
 @PInstrumentTypeID  Char(1) ='V',      
 @PChequeID    Numeric = 0,      
 @PChequeDate   SmallDateTime = NULL,      
 @PReferenceNo   NVarChar(15) = '',      
 @PRemarks    dbo.Remarks = '',      
 @PTrxDescriptionID  NVarChar(10),      
 @PTrxDescription  Description = NULL,      
 @PMainGLID    dbo.AccountID = NULL, --GL case not require to psss this      
 @PContraGLID   dbo.AccountID = NULL,      
 @PTrxFlagID    Char(1),      
 @PImageID    BigInt = 0,      
 @PTrxPrinted   TinyInt = 0,      
 @PIsTrxPending          Bit=0,      
 @PForwardRemark   dbo.Remarks = '',      
 @PBREFTTrxID            nvarchar(100)=NULL,       
 @PForwardToUser   dbo.OperatorID = NULL,      
 @PForwardToGroup  dbo.OperatorID = NULL,      
 @PCreatedBy    dbo.OperatorID = NULL, -- System Posting Trx      
 @PCreatedOn    SmallDateTime = NULL,      
 @ErrorNo    Int = 0 OUTPUT,      
 @SupervisedBy   Varchar(30)=Null,      
 @SupervisedOn   dateTime=Null,    
 @TraceNo  Varchar(15)=Null    
)      
--WITH ENCRYPTION      
AS      
BEGIN      
 SET NOCOUNT ON      
    
 DECLARE @LocalCurrencyID dbo.CurrencyID,      
   @AccountCurrencyID dbo.CurrencyID,      
   @BankID    dbo.BankID,      
   @ProductID   dbo.ProductID,      
   @TrxSerialTypeID dbo.SystemSubID,      
   @IsTrxAllow   Bit,      
   @IsDayOpen   Bit,      
   @IsOurTrxAllow   Bit,      
   @IsOurDayOpen   Bit,      
   @IsHOTrxAllow   Bit,      
   @IsHODayOpen   Bit,            
   @DormantAccountID dbo.AccountID,--Changed by isaac      
   @DormantProductID dbo.ProductID,      
   @AccountClassID  NVARCHAR(10),      
   @MaximumWithdrawals Numeric,      
   @CountNoOfTrx  int      
         
 SET @ErrorNo = 0      
       
 IF CHARINDEX(@PBREFTTrxID, @PTrxDescription) = 0 AND RTRIM(LTRIM(ISNULL(@PBREFTTrxID, ''))) != ''      
  BEGIN      
   SET @PRemarks = @PBREFTTrxID + ' ~ ' + @PRemarks      
   SET @PTrxDescription = @PBREFTTrxID + ' ~ ' + @PTrxDescription      
  END      
       
 -- Retrieve Next BatchID, if it's NULL or Empty (not passed form calling Procedure)      
 IF ISNULL(@PTrxBatchID, '') = ''      
 BEGIN      
  EXEC dbo.p_GetNextTrxBatchID @BranchID = @POurBranchID,      
         @NextTrxBatchID = @PTrxBatchID OutPut      
 END      
      
 IF ISNULL(@PSerialID,0) = 0 AND (@PTrxCodeID = 0 OR @PTrxCodeID = 10 OR @PTrxCodeID = 12)      
 BEGIN      
      
  SET @TrxSerialTypeID = CASE WHEN @PTrxTypeID IN ('CC', 'CD') THEN 'CH'      
         WHEN @PTrxTypeID IN ('TC', 'TD') THEN 'TR'      
         WHEN @PTrxTypeID IN ('IC', 'ID') THEN 'IC'      
         WHEN @PTrxTypeID IN ('OC', 'OD') THEN 'OC'      
        END      
      
  -- Retrieve Next SerialNo, if it's NULL or Empty (not passed form calling Procedure)      
  EXEC dbo.p_GetNextTrxSerialNo @OurBranchID = @POurBranchID,      
         @TrxSerialTypeID = @TrxSerialTypeID,      
         @NextTrxSerialNo = @PSerialID OUTPUT      
 END      
 --check if this account is a dormant account      
       
 if exists (Select AccountID from t_AccountCustomer where OurBranchID=@POurBranchID And AccountID=@PAccountID and AccountStatusID='AD') And @PModuleID<>'1410'      
 Begin      
  /*      
   Return the dormant account if the dormant product allows Credit transaction and      
   if the transcation type is either Cash Credit Transaction Credit and      
   Moreover if the dormant product is set in the t_productdormant      
  */      
         
   Select @DormantAccountID = DormantAccountID,       
     @DormantProductID=t_ProductDormant.DormantProductID      
    FROM t_AccountDormant(NoLock) Inner JOIN t_ProductDormant (NOLOCK) on       
    t_AccountDormant.OriginalProductID= t_ProductDormant.ProductID         
    Where OurBranchID=@POurBranchID And AccountID=@PAccountID       
    And AllowCreditTrx=1      
    And isnull(t_ProductDormant.DormantProductID,'')<>''      
    And @PTrxTypeID IN ('CC', 'TC','IC','OC')      
   --Show error if account will be debited      
    if (@PTrxTypeID IN ('CD', 'TD','ID','OD'))      
     Begin      
     SET @ErrorNo = 300021      
     RAISERROR('300021', 16, 1)--Debit Transaction not Allowed in Dormant Account       
     RETURN      
    End      
           
   if (isnull(@DormantAccountID,'')='')      
    Begin      
     SET @ErrorNo = 300003      
     RAISERROR('BREXDB300003', 16, 1)--Credit Transaction not Allowed in Dormant Account       
     RETURN      
    End      
   ELSE      
    BEGIN      
     SET @PProductID = @DormantProductID      
     SET @PMainGLID = dbo.f_GetGLInterfaceAccountID(dbo.f_GetBankID(@POurBranchID), @PProductID, dbo.f_GetProductTypeID(dbo.f_GetBankID(@POurBranchID),@PProductID), 'CONTROL_AC')      
     SET @PReferenceNo = @PAccountID      
     SET @PRemarks = LEFT(@PAccountID + ' / ' + @PRemarks,500)      
     SET @PAccountID=@DormantAccountID      
     END      
         
   End      
    
 SELECT @PValueDate = ISNULL(@PValueDate,@PTrxDate),      
   @PMeanRate = ISNULL(@PMeanRate,@PExchangeRate),      
   @PTrxDescription = ISNULL(@PTrxDescription, dbo.f_GetTrxDescription(@PTrxDescriptionID))      
      
 SELECT @LocalCurrencyID = dbo.f_GetLocalCurrencyID(@POurBranchID),      
   @AccountCurrencyID = CASE @PAccountTypeID       
         WHEN 'C' THEN dbo.f_GetAccountCurrencyID(@POurBranchID, @PAccountID)      
         WHEN 'G' THEN dbo.f_GetGLBranchCurrencyID(@POurBranchID, @PAccountID)      
         END      
 --Cross Currency Trx Not Allowed      
 IF @LocalCurrencyID <> @AccountCurrencyID AND @AccountCurrencyID <> @PTrxCurrencyID AND @LocalCurrencyID <> @PTrxCurrencyID      
 BEGIN       
  SET @ErrorNo = 300018      
  RAISERROR('BREXDB300018', 16, 1)      
  RETURN      
 END      
      
      
  ------------------------CHECK NO OF ALLOWED WITHDRAWALS PER SPECIFIC PERIOD SET------------------------------------------------------------------------------------------------------------------------      
 IF @PAccountTypeID = 'C' And @PTrxTypeID IN ('CD', 'TD') And @PCreatedBy <> 'SYS' And @PModuleID in ('3000','3010','3020','3030')      
 BEGIN      
  SELECT @AccountClassID  = AccountClassID,       
    @AccountCurrencyID = t_Product.CurrencyID,       
    @LocalCurrencyID = dbo.f_GetLocalCurrencyID(@POurBranchID)      
  FROM t_AccountCustomer(NOLOCK), t_Product(NOLOCK)      
  WHERE dbo.f_GetBankID(t_AccountCustomer.OurBranchID) = t_Product.BankID      
   AND t_AccountCustomer.ProductID = t_Product.ProductID      
   AND t_AccountCustomer.OurBranchID = @POurBranchID      
   AND t_AccountCustomer.AccountID = @PAccountID      
      
  IF EXISTS (SELECT Value FROM dbo.t_SpecialConditionDetail (NOLOCK) WHERE ClassID = @AccountClassID AND SpecialConditionID = 1014)      
   BEGIN      
    SELECT @MaximumWithdrawals = ISNULL(Value,0) FROM dbo.t_SpecialConditionDetail (NOLOCK) WHERE ClassID = @AccountClassID AND SpecialConditionID = 1014      
          
    SELECT @CountNoOfTrx = IsNull(Count(AccountID),0) FROM t_AccountTrx (NOLOCK) WHERE OurBranchID= @POurBranchID AND AccountID = @PAccountID       
    AND YEAR(TrxDate) = Year(@PTrxDate) AND TrxTypeID IN ('CD','TD') And TrxCodeID = 0      
          
    SELECT @CountNoOfTrx = @CountNoOfTrx + Isnull(Count(AccountID),0) FROM t_transaction (NOLOCK) WHERE OurBranchID= @POurBranchID AND AccountID = @PAccountID       
    AND YEAR(TrxDate) = Year(@PTrxDate) AND TrxTypeID IN ('CD', 'TD') And TrxCodeID = 0       
      
    IF (@CountNoOfTrx > @MaximumWithdrawals)      
    BEGIN      
     SET @ErrorNo = 80581139      
     RAISERROR('BREXDB80581139', 16, 1)      
     RETURN      
    END      
   END      
 END      
       
 IF @PModuleID <> 6091 --- Trading P/L posting so Amount = 0 in case of FCY account      
BEGIN      
  IF @LocalCurrencyID = @AccountCurrencyID AND @AccountCurrencyID = @PTrxCurrencyID       
   SELECT @PAmount = @PLocalAmount      
 END      
       
 IF @PAmount IS NULL OR @PLocalAmount IS NULL OR @PTrxAmount IS NULL      
 BEGIN      
  IF @LocalCurrencyID = @AccountCurrencyID AND @AccountCurrencyID <> @PTrxCurrencyID      
  BEGIN      
   SELECT @PAmount = @PLocalAmount      
  END      
  ELSE IF @LocalCurrencyID <> @AccountCurrencyID AND @AccountCurrencyID = @PTrxCurrencyID      
  BEGIN      
   SELECT @PAmount = @PTrxAmount      
  END      
  ELSE IF @LocalCurrencyID <> @AccountCurrencyID AND @LocalCurrencyID = @PTrxCurrencyID      
  BEGIN      
   SELECT @PAmount = @PLocalAmount / @PExchangeRate --Rounding ?      
  END      
 END      
        
 IF @PChequeID = 0 --To avoid default date storing when Voucher No =0      
  SET @PChequeDate = NULL      
      
 IF @PTrxTypeID IN ('CC', 'IC', 'OC', 'TC')      
 BEGIN      
  SELECT @PAmount = ABS(@PAmount),      
    @PLocalAmount = ABS(@PLocalAmount),      
    @PTrxAmount = ABS(@PTrxAmount)      
 END      
 ELSE      
 BEGIN      
  --Absolute use becuase some case if already from UI send negative value      
  SELECT @PAmount = -1 * ABS(@PAmount),      
    @PLocalAmount = -1 * ABS(@PLocalAmount),      
    @PTrxAmount = -1 * ABS(@PTrxAmount)      
 END      
      
 IF @PAccountTypeID = 'G' -- GL Account      
 BEGIN      
  SELECT @PMainGLID = @PAccountID,      
    @PProductID = 'GL'      
 END      
      
 IF @PAccountTypeID = 'C' AND @PMainGLID IS NULL -- Customer Account      
 BEGIN      
  SELECT @BankID  = dbo.f_GetBankID(@POurBranchID),      
    @PProductID = dbo.f_GetAccountProductID(@POurBranchID,@PAccountID)      
      
  SELECT @PMainGLID = dbo.f_GetGLInterfaceAccountID(@BankID, @PProductID, dbo.f_GetProductTypeID(@BankID, @PProductID), 'CONTROL_AC')      
 END      
      
 SELECT TOP 1 @IsTrxAllow = IsTrxAllow,@IsDayOpen=IsDayOpen      
 FROM dbo.t_SystemBranchStatus(NOLOCK)      
 WHERE IsTrxAllow = 0 ---incase any branch is closed, let all transactions go to pending      
       
 SET @PTrxDescription = LEFT(@PTrxDescription,130)      
      
 IF (@IsDayOpen=0 OR @IsTrxAllow = 0 OR @IsOurDayOpen=0 OR @IsOurTrxAllow = 0 OR @IsHODayOpen=0 OR @IsHOTrxAllow = 0) AND @PModuleID not in  ('6020')      
 AND @PCreatedBy NOT IN (select OperatorID from t_user(NOLOCK)) AND @PCreatedBy NOT IN ('SYS','SYS_FD') --'SYS'      
 BEGIN      
       
  SET @PTrxDate=dbo.f_GetNextWorkingDate(@PTrxBranchID,@PTrxDate)      
  INSERT INTO dbo.t_PendingTransaction      
  (      
   TrxBranchID,TrxBatchID,TrxBatchSLNo,SerialID,OurBranchID,AccountTypeID,AccountID,ProductID,ModuleID,TrxCodeID,TrxTypeID,TrxDate,ValueDate,Amount,LocalAmount,      
   TrxCurrencyID,TrxAmount,ExchangeRate,MeanRate,Profit,InstrumentTypeID,ChequeID,ChequeDate,ReferenceNo,Remarks,TrxDescriptionID,TrxDescription,MainGLID,ContraGLID,      
   TrxFlagID,ImageID,TrxPrinted,IsTrxPending,ForwardToUser,ForwardToGroup,BREFTTrxID,CreatedBy,CreatedOn, TraceNo    
  )      
      
  SELECT @PTrxBranchID,@PTrxBatchID,     
  (ISNULL((SELECT MAX(TrxBatchSLNo) FROM dbo.t_PendingTransaction WHERE TrxBranchID = @PTrxBranchID AND TrxBatchID = @PTrxBatchID),0) + 1) TrxBatchSLNo,      
   @PSerialID, @POurBranchID,@PAccountTypeID,@PAccountID,@PProductID,@PModuleID,@PTrxCodeID,@PTrxTypeID,@PTrxDate, @PValueDate,@PAmount,@PLocalAmount,@PTrxCurrencyID,      
   @PTrxAmount,@PExchangeRate,@PMeanRate,@PProfit,@PInstrumentTypeID,@PChequeID,@PChequeDate,@PReferenceNo,@PRemarks, @PTrxDescriptionID,@PTrxDescription,@PMainGLID,      
   @PContraGLID,'',@PImageID,@PTrxPrinted,@PIsTrxPending,@PForwardToUser,@PForwardToGroup,@PBREFTTrxID,ISNULL(@PCreatedBy,'SYS'),IsNull(@PCreatedOn,GETDATE()) , @TraceNo     
 END      
 ELSE      
 BEGIN      
       
  INSERT INTO dbo.t_Transaction      
  (      
   TrxBranchID,      
   TrxBatchID,      
   TrxBatchSLNo,      
   SerialID,      
   OurBranchID,      
   AccountTypeID,      
   AccountID,      
   ProductID,      
   ModuleID,      
   TrxCodeID,      
   TrxTypeID,      
   TrxDate,      
   ValueDate,      
   Amount,      
   LocalAmount,      
   TrxCurrencyID,      
   TrxAmount,      
   ExchangeRate,      
   MeanRate,      
   Profit,      
   InstrumentTypeID,      
   ChequeID,      
   ChequeDate,      
   ReferenceNo,      
   Remarks,      
   TrxDescriptionID,      
   TrxDescription,      
   MainGLID,      
   ContraGLID,      
   TrxFlagID,      
   ImageID,      
   TrxPrinted,      
   IsTrxPending,      
   ForwardRemark,      
   BREFTTrxID,        
   ForwardToUser,      
   ForwardToGroup,      
   CreatedBy,      
   CreatedOn,      
   SupervisedBy,      
   SupervisedOn,    
   TraceNo    
  )       
  SELECT      
   @PTrxBranchID,      
   @PTrxBatchID,      
   (ISNULL((SELECT MAX(TrxBatchSLNo) FROM dbo.t_Transaction (NOLOCK) WHERE TrxBranchID = @PTrxBranchID AND TrxBatchID = @PTrxBatchID), 0) + 1),      
   @PSerialID,      
   @POurBranchID,      
   @PAccountTypeID,      
   @PAccountID,      
   @PProductID,      
   @PModuleID,      
   @PTrxCodeID,      
   @PTrxTypeID,      
   @PTrxDate,      
   @PValueDate,      
   @PAmount,      
   @PLocalAmount,      
   @PTrxCurrencyID,      
   @PTrxAmount,      
   @PExchangeRate,      
   @PMeanRate,      
   @PProfit,      
   @PInstrumentTypeID,      
   @PChequeID,      
   @PChequeDate,      
   @PReferenceNo,      
   @PRemarks,      
   @PTrxDescriptionID,      
   @PTrxDescription,      
   @PMainGLID,      
   @PContraGLID,      
   @PTrxFlagID,      
   @PImageID,      
   @PTrxPrinted,      
   @PIsTrxPending,      
   @PForwardRemark,      
   @PBREFTTrxID,      
   @PForwardToUser,      
   @PForwardToGroup,      
   ISNULL(@PCreatedBy,'SYS'),      
   IsNull(@PCreatedOn,GETDATE()),      
   @SupervisedBy,      
   @SupervisedOn,    
   @TraceNo    
      
 --SELECT @PTrxRowID = SCOPE_IDENTITY()      
 END      
       
 IF @PInstrumentTypeID = 'C'  --For Normal Inward      
 BEGIN      
  SELECT @PTrxRowID = TrxRowID      
  FROM t_Transaction (NOLOCK)      
  WHERE TrxBranchID  = @PTrxBranchID      
   AND TrxBatchID = @PTrxBatchID      
 END      
 IF @PInstrumentTypeID = 'V' And @PModuleID = '3040'  -- For Inward Credits      
 BEGIN      
  SELECT @PTrxRowID = TrxRowID      
  FROM t_Transaction (NOLOCK)      
  WHERE TrxBranchID  = @PTrxBranchID      
   AND TrxBatchID = @PTrxBatchID      
 END      
 IF @PChequeID = 0 And @PModuleID = '3050'  --For Direct Debits      
 BEGIN      
  SELECT @PTrxRowID = TrxRowID      
  FROM t_Transaction (NOLOCK)      
  WHERE TrxBranchID  = @PTrxBranchID      
   AND TrxBatchID = @PTrxBatchID      
 END      
      
 IF ISNULL(@PCreatedBy,'SYS') = 'SYS'      
  SET @PTrxCodeID = 10      
      
       
      
 IF @PAccountTypeID = 'C' And @PTrxCodeID < 10  AND ISNULL(@PTrxFlagID,'') = ''    
 BEGIN      
  Declare @NotificationAmountLimit  money      
      
  ------Send Notifications for Amount Equal or Greater than XXXX      
  Select @NotificationAmountLimit = Value from t_AccountSpecialCondition (NOLOCK)      
  Where OurBranchID = @POurBranchID And AccountID = @PAccountID And SpecialConditionID = '160'      
  Select @NotificationAmountLimit = isNull(@NotificationAmountLimit,0)    
  --Select @NotificationAmountLimit = isNull(@NotificationAmountLimit,2000)      
      
  ------Dont Send Notifications for any Credit Transaction      
  IF (Select Count(AccountID) from t_AccountSpecialCondition (NOLOCK)      
  Where OurBranchID = @POurBranchID And AccountID = @PAccountID And SpecialConditionID = '161') > 0      
   SET @PAmount = 0      
      
  ------Dont Send Notifications for any Debit Transaction       
  IF (Select Count(AccountID) from t_AccountSpecialCondition (NOLOCK)   
  Where OurBranchID = @POurBranchID And AccountID = @PAccountID And SpecialConditionID = '162') > 0      
   SET @PAmount = 0      
      
  IF ABS(@PAmount) >= @NotificationAmountLimit And @PAmount <> 0      
  BEGIN      
   DEclare @TrxBranchName  dbo.[Description],      
     @OperatorNames  dbo.[Description],      
     @TrxTypeNarration dbo.[Description],      
     @TrxAmount   dbo.Amount      
      
   Select @TrxBranchName = dbo.f_GetBranchName(@PTrxBranchID), @OperatorNames = dbo.f_GetOperatorName(@PCreatedBy), @TrxAmount = ABS(@PAmount),      
    @TrxTypeNarration = Case @PTrxTypeID When 'CC' Then 'imewekewa'       
              When 'CD' Then 'imetolewa'      
              When 'TC' then 'imewekewa'      
              When 'TD' Then 'imetolewa'      
              When 'OC' then 'imewekewa'      
              When 'OD' Then 'imetolewa'      
              When 'IC' then 'imewekewa'      
              When 'ID' Then 'imetolewa'         
              Else 'transacted'       
             End      
      
   EXEC p_ProcessNotification      
    @NotificationTriggerType = 'TRX',       
    @NotificationTriggerID  = 3000,      
    @BankID      = @BankID,      
    @OurBranchID    = @POurBranchID,      
    @AccountID     = @PAccountID,      
    @Amount      = @TrxAmount,      
    @TrxBranchName    = @TrxBranchName,      
    @ChequeNo     = @PChequeID,      
    @ChequeDate     = @PChequeDate,      
    @DeviceLocation    = NUll,      
    @ExpiryDate     = NUll,      
    @InstallAmount    = NUll,      
    @InstallDate    = NUll,      
    @LoanAmount     = NUll,      
    @WorkingDate    = @PTrxDate,      
    --@WorkingDate    = getdate(),      
    @OperatorID     = @PCreatedBy,      
    @OperatorNames    = @OperatorNames,      
    @ReceiptAmount    = NUll,      
    @ReceiptNo     = NUll,      
    @FromDate     = NUll,      
    @ToDate      = NUll,      
    @TrxType        = @TrxTypeNarration,      
    @BREFTTrxID     = @PBREFTTrxID      
 --Print 'herere'      
  END      
 END      
       
--For AML      
 IF @PAccountTypeID = 'C' AND ISNULL(@PCreatedBy,'SYS') <> 'SYS' -- Customer Account & Created by is not SYS (to eliminate fees & interest trx)      
 BEGIN      
      
   DECLARE @AMLTrxRowID varchar(MAX)      
   SELECT @AMLTrxRowID =  COALESCE(@AMLTrxRowID + ',', '') + CONVERT(VARCHAR(MAX),TrxRowID) FROM t_Transaction (NOLOCK)       
   WHERE TrxBranchID  = @PTrxBranchID AND TrxBatchID = @PTrxBatchID AND AccountTypeID='C' AND TrxCodeID<10      
         
   EXEC sp_AML_AddTrxJob @TrxRowID = @AMLTrxRowID      
 END      
 SET NOCOUNT OFF      
END      
    
    
    
    
    