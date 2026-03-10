CREATE PROCEDURE dbo.p_ChargeTransaction        
(          
 @OurBranchID         dbo.BranchID,          
 @AccountID          dbo.AccountID,          
 @ChargeID    nVarChar(15)=NULL,            
 @EventID          dbo.SystemSubID=NULL,          
 @TrxDescriptionID       nvarchar(15)=NULL,           
 @ChargeAccountTypeID dbo.SystemSubID=NULL,          
 @ChargeAccountID  dbo.AccountID=NULL,          
 @ChargeBranchID      dbo.BranchID=NULL,          
 @TrxAmount              dbo.Amount = Null,          
 @TrxDate                smalldatetime = Null,          
 @TrxTypeID              dbo.SystemSubID=NULL,          
 @TrxCurrencyID          dbo.CurrencyID,          
 @ModuleID          Bigint,           
 @CreatedBy          dbo.OperatorID,          
 @BREFTTrxID             nvarchar(100)=NULL,          
 @TrxBatchID          VarChar(20)=NULL OUT,           
 @SerialID          Int=NULL OUTPUT,             
 @Narration       Description=NULL,          
 @IsInstallmentFee       bit=0,          
 @Multiplier             int=NULL,          
 @WaiveCharge            bit=0,        
 @ReceiptID    dbo.ReceiptID=NULL,        
 @DepositSeries   dbo.AccountSeries=NULL,        
 @IsTrxReversal   Bit = 0,          
 @ErrorNo          VarChar(12) = '0' OutPut,          
 @View     BIT = 0 ,        
 @ChargeIDWaivers nvarchar(250) = NULL,        
 @Series   int = null,  
 @IsPendingTrx  Bit   = 0,     
 @TrxFlagID  nvarchar(15)=NULL  
)          
            
AS          
SET NOCOUNT ON          
BEGIN          
        
 DECLARE           
  @ErrorCode    VarChar(15),            
  @WorkingDate   SmallDatetime,          
  @BankID     dbo.BankID,          
  @ChargeDueRowID   BigInt,          
  @ChargeAmount   dbo.Amount,          
  @TaxAmount    dbo.Amount,          
  @CessAmount    dbo.Amount,          
  @ValueDate    SmallDatetime,            
  @CreatedOn    SmallDatetime,            
  --@ChargeBranchID         dbo.BranchID=NULL,            
  @CurrencyID             dbo.CurrencyID,          
  @CessNarration          dbo.Description,          
  @CustomerNarration  dbo.Description,          
  @TaxNarration           dbo.Description,          
  @EffectiveDate   smalldatetime,          
  @ProductID    dbo.ProductID,          
  @ChargeProductID    dbo.ProductID,         
  @TotalCharge   dbo.Amount,          
  @AvailableBalance  dbo.Amount,          
  @ChargingMethodID       dbo.SystemSubID,          
  @AccountClassID         dbo.SystemSubID,          
  @FreezeAmount           dbo.Amount,          
  @RecoveryModeID         dbo.SystemSubID,             
  @ChargeReceivableGLID   dbo.AccountID,          
  @LGNumber    nVarchar(25),          
  @ProductTypeID   dbo.SystemSubID,          
  @ProductClassID   dbo.SystemSubID,        
  @ClearBalance   dbo.Amount,          
  @IntroducerAccountID  dbo.AccountID,        
  @EventID_1          dbo.SystemSubID=@EventID,        
  @SCID     NVARCHAR(20) = '180',        
  @IsTrxReversal_1  BIT = 0,        
  @SQLStatement   NVARCHAR(255),        
  @IsInterBranch   Bit,  
  @CalculatedAmount dbo.Amount,  
  @MarginAmount dbo.Amount,  
  @ChargeRate dbo.Amount,  
  @LcAmount  dbo.Amount,  
  @ExchangeRate dbo.Rate,  
  @MinChargeAmount dbo.Amount  
  --@IsTrxReversal   Bit          
 --If (@ModuleID IN (1811))          
 --BEGIN          
           
 -- SET @AccountID = SUBSTRING(@AccountID,0,charindex('-',@AccountID,0))          
 -- SET @LGNumber = SUBSTRING(@AccountID,charindex('-',@AccountID,0)+1,LEN(@AccountID))          
            
 --END          
     
  CREATE TABLE #SCLCCharge        
 (        
  ChargeID   NVARCHAR(30)        
 )        
 SET @SQLStatement = 'CREATE CLUSTERED INDEX IDX_SCLCCharge_' + CAST(DATEPART(SS, GETDATE()) AS NVARCHAR) + ' ON #SCLCCharge (ChargeID)'        
 EXEC SP_ExecuteSQL @SQLStatement        
        
 SET @IsTrxReversal = ISNULL(@IsTrxReversal, 0)        
        
 SELECT           
  @BankID        =  dbo.f_GetBankID(@OurBranchID),          
  @WorkingDate      =  dbo.f_GetWorkingDate(@OurBranchID),          
  @ErrorNo       =  '0',          
  @ValueDate       =  @WorkingDate,          
  @CreatedOn    =  GETDATE(),            
  --@ContraAccountTypeID = 'C',          
  --@ContraAccountID  = @AccountID,   
  @TrxAmount    = ISNULL(@TrxAmount,0),         
  @ProductID    = dbo.f_GetAccountProductID(dbo.f_GetAccountBranchID(@AccountID),@AccountID),          
  @ProductTypeID   = dbo.f_GetProductTypeID(@BankID, @ProductID)          
            
 SELECT @ProductClassID = ProductClassID        
 FROM t_Product (NOLOCK)        
 WHERE ProductID = @ProductID        
        
 SET @IsTrxReversal = ISNULL(@IsTrxReversal, 0)        
        
 IF @OurBranchID <> @ChargeBranchID        
SET @IsInterBranch = 1        
        
 IF @IsTrxReversal = 1        
  INSERT INTO #SCLCCharge        
  (        
   ChargeID        
  )        
  SELECT BranchID        
  FROM DBO.fn_ReturnBranchID(dbo.f_GetSpecialConditionValue(@BankID, @SCID, @ProductClassID, 'V'))          
 SELECT @WaiveCharge = ISNULL(@WaiveCharge,0)           
            
 SELECT @ChargeBranchID = CASE WHEN ISNULL(@ChargeBranchID,'')='' THEN @OurBranchID ELSE @ChargeBranchID END,          
   @ChargeAccountID = CASE WHEN ISNULL(@ChargeAccountID,'')='' THEN @AccountID ELSE @ChargeAccountID END,          
        --@ChargeAccountTypeID='C'          
     @ChargeAccountTypeID= CASE WHEN ISNULL(@ChargeAccountTypeID,'') = '' THEN 'C' ELSE @ChargeAccountTypeID END   --'C'          
           
 Select @IntroducerAccountID = IsNull(SalesOfficerID,'') From t_AccountCustomer where OurBranchID = @ChargeBranchID And AccountID = @ChargeAccountID          
           
 ----If (@IntroducerAccountID <> '')          
 ----  Begin          
 ----  IF Not Exists(Select AccountID From t_GLParameter Where GLParameterID = 'INTRODUCER_AC')          
 ----   --Begin          
 ----   -- SET @ErrorNo = 'BREXDB70581109'          
 ----   -- RAISERROR(N'BREXDB70581109', 16, 1)              
 ----   -- RETURN             
 ----   --End          
 ----  End          
          
 CREATE TABLE #ChargeDue          
 (            
  ChargeID   nVarChar(15) NULL,            
  CurrencyID   nVarChar(3) NULL,          
  EffectiveDate       smalldatetime NULL,          
  PaymentTypeID  nVarChar(25) NULL,          
  CustNarration  Varchar(200) NULL,          
  TaxNarration  Varchar(200) NULL,          
  CessNarration  Varchar(200) NULL,          
  IsChargeIncludeTax Bit NULL,          
  ApplyChargeID       nVarChar(25) NULL,          
  ChargingMethodID    nVarChar(25) NULL,          
  DeferIncome         Bit NULL,          
  ChargeAmount     money NULL,          
  TaxAmount      money NULL,          
  CessAmount         money NULL           
 )          
          
             
BEGIN TRANSACTION          
          
 --Generate the Event Charge and Amount here           
 SELECT @TrxDescriptionID =CASE WHEN ISNULL(@TrxDescriptionID,'')<>''           
   THEN @TrxDescriptionID ELSE NULL END,          
     @ChargeID  =CASE WHEN ISNULL(@ChargeID,'')<>'' THEN @ChargeID ELSE NULL END,          
        @EventID  =CASE WHEN ISNULL(@ChargeID,'')<>'' OR ISNULL(@TrxDescriptionID,'')<>''           
        THEN NULL ELSE @EventID END               
           
 DECLARE           
  @AddCharge     bit,          
  @AddExtraCharge    bit,          
  @Limit            dbo.Amount,          
  @BalWithoutLimit            dbo.Amount          
           
 SELECT  @AddCharge=1,@AddExtraCharge=0          
          
 WHILE @AddCharge=1 OR @AddExtraCharge=1          
 BEGIN          
   --------SELECT @OurBranchID,@AccountID,@ProductID,@TrxAmount,@WorkingDate,@TrxCurrencyID,@EventID,NULL,NULL,1,@ChargeBranchID,@ChargeAccountID,@Series         
   --------Select * From dbo.f_GetProductChargeDetails(@OurBranchID,@AccountID,@ProductID,@TrxAmount,@TrxDate,@TrxCurrencyID,@EventID,@ChargeID,@TrxDescriptionID,@Multiplier)          
         
 INSERT INTO #ChargeDue          
 Exec p_GetProductChargeDetails          
    @OurBranchID      = @OurBranchID,          
    @AccountID        = @AccountID,          
    @ProductID        = @ProductID,          
    @TrxAmount        = @TrxAmount,          
    @TrxDate          = @TrxDate,          
    @TrxCurrencyID    = @TrxCurrencyID,          
    @EventID    = @EventID,   
    @ChargeID      = @ChargeID,          
    @TrxDescriptionID = @TrxDescriptionID,          
    @Multiplier       = @Multiplier,          
  @ChargeBranchID      =@ChargeBranchID,          
  @ChargeAccountID  =@ChargeAccountID ,        
  @Series = @Series        
      --select * from #ChargeDue    
   --Execute p_GetProductChargeDetails          
   --@OurBranchID      = '001',          
   --@AccountID       = '0011100001',          
   --@ProductID        = 'LC01',          
   --@TrxAmount        = 0,          
   --@TrxDate          = NULL,          
   --@TrxCurrencyID    = 'KES',          
   --@EventID          = 'LC_Approval',          
   --@ChargeID         = NULL,          
   --@TrxDescriptionID = NULL,          
   --@Multiplier       = NULL          
             
   --Select @OurBranchID,@AccountID,@ProductID,@TrxAmount,@TrxDate,@TrxCurrencyID,@EventID,@ChargeID,@TrxDescriptionID,@Multiplier          
        
   If @ProductTypeID IN ('LG') AND ISNULL(@ChargeAccountID,'') = ''        
   Begin          
    Select @AccountID = DebitAccount From t_LGApplication(NOLOCK) Where OurBranchID = @OurBranchID And AccountID = @AccountID          
    Select @ChargeAccountID = @AccountID          
    --SET @ProductID = dbo.f_GetAccountProductID(@TrxBranchID, @ContraAccountID)          
 --SET @MainGLID  = dbo.f_GetGLInterfaceAccountID(@BankID, @ProductID, @ProductTypeID, 'CONTROL_AC')          
   End          
   If @ProductTypeID IN ('LC') AND ISNULL(@ChargeAccountID,'') = ''        
   Begin          
    Select @AccountID = DebitAccount From t_LCApplication(NOLOCK) Where OurBranchID = @OurBranchID And AccountID = @AccountID          
    Select @ChargeAccountID = @AccountID          
    --SET @ProductID = dbo.f_GetAccountProductID(@TrxBranchID, @ContraAccountID)          
    --SET @MainGLID  = dbo.f_GetGLInterfaceAccountID(@BankID, @ProductID, @ProductTypeID, 'CONTROL_AC')          
   End             
   If @ProductTypeID = 'LK'           
   Begin          
    Select @AccountID = ChargeAccountID From t_LockerMaintenance(NOLOCK) Where OurBranchID = @OurBranchID And AccountID = @AccountID          
    Select @ChargeAccountID = @AccountID              
    --SET @ProductID = dbo.f_GetAccountProductID(@TrxBranchID, @ContraAccountID)          
    --SET @MainGLID  = dbo.f_GetGLInterfaceAccountID(@BankID, @ProductID, @ProductTypeID, 'CONTROL_AC')          
   End          
                
 --  If @CreatedBy = 'ATM' AND ISNULL(@BREFTTrxID,'') <> ''        
 --  Begin        
 --Update #ChargeDue Set ChargeAmount = @TrxAmount Where ISNULL(@TrxAmount,0) > 0        
 --  End        
           
   IF NOT EXISTS (SELECT 1 FROM #ChargeDue)          
   BEGIN          
    GOTO EndStatement          
   END          
          
   SET @TotalCharge=0          
   IF @ChargeAccountID=@AccountID AND EXISTS(SELECT 1 FROM #ChargeDue           
   INNER JOIN t_Charge(NOLOCK) ON #ChargeDue.ChargeID=t_Charge.ChargeID           
   AND (RecoveryModeID IS NULL OR RecoveryModeID NOT IN ('G')))          
   BEGIN          
    SELECT @TotalCharge=SUM(ISNULL(ChargeAmount,0))+          
         SUM(ISNULL(TaxAmount,0))+          
         SUM(ISNULL(CessAmount,0))           
    FROM #ChargeDue INNER JOIN t_Charge(NOLOCK)            
       ON #ChargeDue.ChargeID=t_Charge.ChargeID           
       AND (RecoveryModeID IS NULL OR RecoveryModeID NOT IN ('G'))          
   END          
   SET @AvailableBalance=0          
   SELECT @Limit= dbo.f_GetAccountLimit(@OurBranchID, @AccountID),          
       @AvailableBalance =dbo.f_GetAvailableBalanceLite(@OurBranchID, @AccountID)          
                 
      SET @BalWithoutLimit=CASE WHEN @AvailableBalance>0 AND @Limit>0 THEN @AvailableBalance-@Limit ELSE          
          @AvailableBalance END          
   IF @TrxTypeID IN ('CD', 'ID', 'TD', 'OD') AND           
 dbo.f_GetProductTypeID(@BankID,@ProductID) IN ('CA') AND ISNULL(@TrxDescriptionID,'')<>''           
   AND @Limit>0 AND @BalWithoutLimit<(@TotalCharge+ABS(@TrxAmount))    
AND @AddExtraCharge=0       
   BEGIN             
      SELECT  @EventID='OD_LIM_UTIL',@AddExtraCharge=1,@AddCharge=0          
   END          
   ELSE          
   BEGIN          
      SELECT @AddCharge=0,@AddExtraCharge=0          
   END          
                
   --SELECT @ClearBalance = ClearBalance FROM t_AccountCustomer(NOLOCK)           
   --WHERE OurBranchID = @OurBranchID          
   --AND AccountID = @AccountID             
   --IF(@ClearBalance < 0)          
   --BEGIN             
   --   SELECT  @EventID='ACC_OVERDRAW',@AddExtraCharge=1,@AddCharge=0     
   --END          
   --ELSE          
   --BEGIN          
   --   SELECT @AddCharge=0,@AddExtraCharge=0          
   --END          
             
              
    END      
 ----update ma  
  
IF EXISTS(select *from #ChargeDue where chargeid='LCY206')  
   BEGIN  
   
 SELECT  @MarginAmount=AccountMarginAmount,@ExchangeRate=ExchangeRate,@LcAmount=amount     
 FROM t_LCApplication (NOLOCK) WHERE OurBranchID = @OurBranchID AND AccountID = @AccountID   
  
   
  
  
  
  
  
  
    SELECT      
        @ChargeRate=(select CASE WHEN ISNULL(t_Charge.CalculationMethodID,'')='F'     
    THEN t_ChargeRate.Formulae     
      ELSE  CAST(t_ChargeRate.Amount AS NVARCHAR(100)) END Amount),  
   @MinChargeAmount=MinimumCharge  
  
  
FROM t_ChargeRate(NOLOCK) INNER JOIN t_Charge(NOLOCK) ON    
      t_ChargeRate.BankID     = t_Charge.BankID AND     
      t_ChargeRate.ChargeID = t_Charge.ChargeID    
      INNER JOIN t_ChargeEffectiveDate(NOLOCK) ON     
      t_ChargeRate.BankID             = t_ChargeEffectiveDate.BankID AND     
      t_ChargeRate.ChargeID         = t_ChargeEffectiveDate.ChargeID AND    
      t_ChargeRate.EffectiveDateID = t_ChargeEffectiveDate.EffectiveDateID    
  WHERE t_ChargeRate.BankID         = '00'    
   AND t_ChargeRate.ChargeID     = 'LCY206'    
   and t_ChargeEffectiveDate.ChargeStatusID='AA'  
  
  
   --AND t_ChargeRate.EffectiveDateID = @EffectiveDateID    
    set @LcAmount=@LcAmount*@ExchangeRate  
 --select @LcAmount,@MarginAmount,@ExchangeRate  
 SET @MinChargeAmount=@MinChargeAmount*@ExchangeRate  
  
Select @CalculatedAmount= ChargeAmount from #ChargeDue  where  ChargeID='LCY206'   
  
  
   
  
IF(@CalculatedAmount>@MinChargeAmount)  
  
  UPDATE #ChargeDue   
SET   
    ChargeAmount = ROUND(((@LcAmount - @MarginAmount) * @ChargeRate / 100),2),  
    TaxAmount = ROUND(((@LcAmount - @MarginAmount) * @ChargeRate / 100) * 0.15,2)  
WHERE ChargeID = 'LCY206';  
  
   END  
  
   
  
--------update  marg End  
  
  
  
  
  
  
 SET @AvailableBalance=0          
          
 --select * from #ChargeDue          
 --return          
           
    IF @TrxBatchID IS NULL AND  @SerialID IS NULL          
    BEGIN          
   EXEC p_GetNextTrxID @TrxBranchID  = @OurBranchID,           
     @NextBatchID  = @TrxBatchID OutPut,           
     @NextSLNo   = @SerialID   OutPut,          
     @TrxSerialTypeID = 'TR'          
    END          
        
    IF IsNull(@TrxBatchID,'') = '' And @SerialID IS NOT NULL       EXEC p_GetNextTrxBatchID   @BranchID=@OurBranchID, @NextTrxBatchID= @TrxBatchID OutPut          
        
 IF IsNull(@SerialID,'') = ''        
    BEGIN          
   EXEC p_GetNextTrxID @TrxBranchID  = @OurBranchID,           
     @NextBatchID  = @TrxBatchID,           
     @NextSLNo   = @SerialID   OutPut,          
     @TrxSerialTypeID = 'TR'          
    END          
        
 SET @Narration=CASE WHEN ISNULL(@Narration,'')='' THEN '' ELSE ' - '+@Narration  END          
        
IF ISNULL(@ChargeIDWaivers,'') <> '' AND @EventID LIKE '%LC_AMENDMENT%'        
 DELETE FROM #ChargeDue WHERE @ChargeIDWaivers LIKE '%' + ChargeID + '%'        
  
 DECLARE CurChargeDue CURSOR FOR          
 SELECT ChargeID,CurrencyID,EffectiveDate,ChargeAmount,TaxAmount,CessAmount FROM #ChargeDue           
 OPEN CurChargeDue          
          
 FETCH NEXT FROM CurChargeDue INTO  @ChargeID,@CurrencyID,@EffectiveDate,@ChargeAmount,@TaxAmount,@CessAmount          
          
 WHILE @@FETCH_STATUS = 0          
 BEGIN          
    IF @TrxTypeID NOT IN ('CD', 'ID', 'OD', 'TD')  
  BEGIN  
  SET @TrxTypeID=''  
  END    
  SELECT  TOP 1         
    @CustomerNarration= ISNULL(CustNarration,'')+ ISNULL(@Narration,''),         
       @TaxNarration = ISNULL(CustNarration,'')+ ':  ' +TaxNarration,          
       @CessNarration = CessNarration          
  FROM #ChargeDue          
  Where ChargeID =  @ChargeID          
        
  SET @TotalCharge = ISNULL(@ChargeAmount, 0) + ISNULL(@TaxAmount, 0) + ISNULL(@CessAmount, 0)          
  SELECT @AvailableBalance = dbo.f_GetAvailableBalanceLite(@ChargeBranchID, @ChargeAccountID),          
    @TrxAmount=ISNULL(@TrxAmount,0)    
  
  
  SELECT @ChargingMethodID=ChargingMethodID           
  FROM  t_Charge(NOLOCK)           
  WHERE BankID=@BankID AND ChargeID=@ChargeID          
            
        
        
--Mercy changed wrong logic below, available balance is already updated by transaction amount          
  IF  @TotalCharge >0 AND @AvailableBalance  < @TotalCharge --( CASE WHEN @ChargeAccountID=@AccountID THEN ABS(@TrxAmount) ELSE 0 END + @TotalCharge) -- AND @TrxAmount < 0            
  --IF  @TotalCharge >0 AND @AvailableBalance < ( CASE WHEN @ChargeAccountID=@AccountID THEN ABS(@TrxAmount) ELSE 0 END + @TotalCharge) -- AND @TrxAmount < 0            
  AND ((ISNULL(@TrxTypeID,'')<>'' AND  @TrxTypeID IN ('CD', 'ID', 'OD', 'TD')) OR ISNULL(@TrxTypeID,'')='') AND @IsTrxReversal = 0 --AND ISNULL(@WaiveCharge,0)=0 --new changes by mosh          
  BEGIN          
    
   -- Modified by Nimrod M. N. on 22-Feb-2023: Added IF Statement        
   IF @IsTrxReversal = 0        
   BEGIN        
   IF @ChargingMethodID = 'EBBT' --Charge Only If Balance Is Enough,  Otherwise Block Transaction          
   BEGIN          
                 
    CLOSE CurChargeDue        
    DEALLOCATE CurChargeDue       
    ROLLBACK TRAN    
 print @TotalCharge  
 PRINT @ChargeBranchID  
  PRINT @ChargeAccountID  
PRINT @AvailableBalance           
    SET @ErrorNo = '300008'           
    RAISERROR(N'BREXDB300008', 16, 1)          
    RETURN          
   END          
          
   ELSE IF @ChargingMethodID = 'IOAB' -- Charge Irrespective Of Available Balance          
   BEGIN          
   IF NOT EXISTS (SELECT BankID FROM t_SystemBankParameter(NOLOCK)          
     WHERE BankID = @BankID AND SysParamID = 23) AND @ModuleID = 3000    -- Allow Overdraft By Cash Withdrawal?          
          
    BEGIN          
     CLOSE CurChargeDue          
     DEALLOCATE CurChargeDue           
     ROLLBACK TRAN           
     SET @ErrorNo = '300042'             
     RAISERROR(N'BREXDB300042', 16, 1)          
     RETURN          
          
    END                      
    SELECT @AccountClassID= AccountClassID FROM t_AccountCustomer(NOLOCK)          
    WHERE OurBranchID=@ChargeBranchID AND AccountID=@ChargeAccountID          
              
    IF EXISTS(SELECT BankID FROM t_SpecialConditionDetail(NOLOCK)          
    WHERE BankID = @BankID AND ClassID = @AccountClassID AND SpecialConditionID = 904) -- Account should never go in debit          
    BEGIN          
      CLOSE CurChargeDue          
      DEALLOCATE CurChargeDue           
      ROLLBACK TRAN          
      SET @ErrorNo = '300009'          
      RAISERROR(N'BREXDB300009', 16, 1)          
      RETURN          
    END           
          
   END          
          
   ELSE IF @ChargingMethodID = 'EBOF' -- Charge Only If Balance Is Enough,  Otherwise Freeze Account          
   BEGIN          
          
    INSERT INTO t_AccountChargeExemption          
     (OurBranchID, AccountID, ModuleID, ExceptionDate, ChargeAmount,          
     TaxAmount, CessAmount, ExceptionAmount, ExceptionStatusID,ChargeID,ExemptedBy,TrxBatchID,SerialID)          
    SELECT @ChargeBranchID, @ChargeAccountID, @ModuleID, @TrxDate, @ChargeAmount,          
     @TaxAmount, @CessAmount, @TotalCharge, 'A',@ChargeID,@CreatedBy,NULL,NULL --ExceptionStatusID having doubt what are the status code          
               
              
    SET @FreezeAmount = @TotalCharge          
    --SET @TotalCharge =0          
   END          
             
   /*          
   ELSE IF @ChargingMethodID = 'WAFB' --Charge Whatever Available,  Freeze Rest          
   BEGIN          
    SET  @FreezeAmount=@TotalCharge - (@AvailableBalance - @TrxAmount)          
    SET @TotalCharge  = @AvailableBalance - @TrxAmount           
   END          
   */          
          
  ELSE IF @ChargingMethodID = 'EBOW' --Charge Only If Balance Is Enough,Otherwise waive charge          
   BEGIN          
    INSERT INTO t_AccountChargeExemption           
     (OurBranchID, AccountID, ModuleID, ExceptionDate, ChargeAmount,          
     TaxAmount, CessAmount, ExceptionAmount, ExceptionStatusID,ChargeID,ExemptedBy,TrxBatchID,SerialID)          
    SELECT @ChargeBranchID, @ChargeAccountID, @ModuleID, @TrxDate, @ChargeAmount,          
     @TaxAmount, @CessAmount, @TotalCharge, 'A',@ChargeID ,@CreatedBy,NULL,NULL--ExceptionStatusID having doubt what are the status code          
    SET @TotalCharge  = 0          
          
   END          
             
   ELSE IF @ChargingMethodID = 'EBOA' --Charge Only If Balance Is Enough,Postpone charge collection until funds are Available          
   BEGIN          
    INSERT INTO t_AccountChargeExemption           
     (OurBranchID, AccountID, ModuleID, ExceptionDate, ChargeAmount,          
     TaxAmount, CessAmount, ExceptionAmount, ExceptionStatusID,ChargeID,ExemptedBy,TrxBatchID,SerialID)          
    SELECT @ChargeBranchID, @ChargeAccountID, @ModuleID, @TrxDate, @ChargeAmount,          
     @TaxAmount, @CessAmount, @TotalCharge, 'A',@ChargeID,@CreatedBy,NULL,NULL --ExceptionStatusID having doubt what are the status code          
       SET @ChargeDueRowID=NULL          
    EXEC p_GetNextChargeDueRowID @ChargeBranchID, @ChargeDueRowID OUTPUT, @ErrorNo OUTPUT          
              
 IF ISNULL(@ErrorNo, '0') <> '0'       
BEGIN         
            CLOSE CurChargeDue          
      DEALLOCATE CurChargeDue           
      ROLLBACK TRAN           
      ---SET @ErrorNo = '300008'               
      RAISERROR(@ErrorNo, 16, 1)           
      RETURN          
    END          
        
 SET @ChargeProductID = dbo.f_GetAccountProductID(@ChargeBranchID,@ChargeAccountID)        
    INSERT INTO t_ChargeDue          
    (          
     OurBranchID,ProductID,ChargeDueRowID,ClientID,AccountID,ApplicationID,ChargeID,          
     CurrencyID,ProcessDate,DueDate,ChargeColConditionID,ChargeAmount,TaxAmount,CessAmount,ModuleID,          
     ChargeDueStatusID,ExemptionReason,TrxBatchID,SerialID,TrxDate,PaymentTypeID,ReversedDate,ReversedBy,          
     RevTrxDate,RevTrxBatchID,EventID          
    )          
    SELECT  @ChargeBranchID,@ChargeProductID,@ChargeDueRowID,dbo.f_GetAccountClientID(@ChargeBranchID,@ChargeAccountID),          
  --SELECT  @ChargeBranchID,dbo.f_GetAccountProductID(@ChargeBranchID,@ChargeAccountID),@ChargeDueRowID,dbo.f_GetAccountClientID(@ChargeBranchID,@ChargeAccountID),          
            @ChargeAccountID,NULL,@ChargeID,          
      @CurrencyID,@WorkingDate,@WorkingDate,'ABD',@ChargeAmount,@TaxAmount,          
      @CessAmount,@ModuleID,'N',Null,Null,Null,Null,ISNULL(@RecoveryModeID,'C'),Null,Null,Null,Null,NULL          
        
        
                  IF ISNULL(@ChargeAccountID,'')<>'' And @AvailableBalance > 1 ----Avoid Decimal Charge Recovery          
       EXEC p_PartialChargeRecovery             
        @OurBranchID  = @ChargeBranchID,          
        @AccountID   = @ChargeAccountID,          
        @IncomingChargeID = @ChargeID,          
        @TrxBatchID  = @TrxBatchID,           
        @SerialID   = @SerialID,          
        @ProcessID   ='EOD',          
        @ProcessDate  = @WorkingDate,          
        @ErrorNo   = @ErrorNo OUTPUT          
           
                SET @TotalCharge  = 0          
  END   
  
     END          
         
  END        
   -- Modified by Nimrod M. N. on 22-Feb-2023: Added ELSE IF Statement        
  ELSE IF @IsTrxReversal = 1        
  BEGIN         
   SET @IsTrxReversal_1 = 0        
 IF (SELECT COUNT(1) FROM #SCLCCharge WHERE ChargeID = @ChargeID) >= 1        
  SET @IsTrxReversal_1 = 1        
           
 EXEC p_GetNextChargeDueRowID @ChargeBranchID, @ChargeDueRowID OUTPUT, @ErrorNo OUTPUT        
        
  INSERT INTO t_ChargeDue          
    (          
     OurBranchID,ProductID,ChargeDueRowID,ClientID,AccountID,ApplicationID,ChargeID,          
     CurrencyID,ProcessDate,DueDate,ChargeColConditionID,ChargeAmount,TaxAmount,CessAmount,ModuleID,          
     ChargeDueStatusID,ExemptionReason,TrxBatchID,SerialID,TrxDate,PaymentTypeID,ReversedDate,ReversedBy,          
     RevTrxDate,RevTrxBatchID,EventID,IsTrxReversal        
    )          
    SELECT  @ChargeBranchID,dbo.f_GetAccountProductID(@ChargeBranchID,@ChargeAccountID),@ChargeDueRowID,dbo.f_GetAccountClientID(@ChargeBranchID,@ChargeAccountID),          
            @ChargeAccountID,NULL,@ChargeID,          
      @CurrencyID,@WorkingDate,@WorkingDate,'ABD',@ChargeAmount,@TaxAmount,          
      @CessAmount,@ModuleID,'N',Null,Null,Null,Null,ISNULL(@RecoveryModeID,'C'),Null,Null,Null,Null,NULL, @IsTrxReversal_1        
                            
--select 'One', * from t_ChargeDue where AccountID = '00010651000007' order by DueDate        
--select 'One', * from t_Transaction (nolock) order by CreatedOn        
   IF (ISNULL(@ChargeAccountID,'')<>'' And @AvailableBalance > 1) Or @IsTrxReversal_1 = 1 ----Avoid Decimal Charge Recovery          
       EXEC p_PartialChargeRecovery             
        @OurBranchID  = @ChargeBranchID,          
        @AccountID   = @ChargeAccountID,          
        @IncomingChargeID = @ChargeID,          
        @TrxBatchID  = @TrxBatchID,        
        @SerialID   = @SerialID,          
        @ProcessID   ='EOD',          
        @ProcessDate  = @WorkingDate,       
    @ErrorNo  = @ErrorNo OUTPUT          
        
--select 'Two', * from t_ChargeDue where AccountID = '00010651000007' order by DueDate        
--select 'Two', * from t_Transaction (nolock) order by CreatedOn        
        SET @TotalCharge  = 0         
 END        
        
  -- Post Charge Amount if @ChargeAmount > 0          
  IF @TotalCharge > 0 --AND ISNULL(@WaiveCharge,0)=0 -- new changes by mosh          
  BEGIN      
    
   IF (@TrxTypeID IN ('CC','CD'))           
    SET @TrxTypeID = 'C'           
   Else           
    SET @TrxTypeID = 'T'          
          
   If @Moduleid In ('3100', '1571')          
   Begin          
    EXEC p_PostCharge          
      @OurBranchID    = @OurBranchID,          
      @TrxBatchID     = @TrxBatchID,          
      @SerialID     = @SerialID,          
      @ModuleID     = @ModuleID,               
      @ValueDate     = @ValueDate,          
      @CurrencyID     = @CurrencyID,          
      @ChargeID     = @ChargeID,          
      @EffectiveDate    = @EffectiveDate,          
      @ProductID     = @ProductID,              @ChargeAmount    = @ChargeAmount,          
      @CustomerNarration   = @CustomerNarration,              
      @TrxTypeID     = @TrxTypeID,--'T',          
      @ContraAccountTypeID  = @ChargeAccountTypeID,          
      @ContraAccountID   = @ChargeAccountID,          
      @ChargeBranchID    = @ChargeBranchID,          
      @ChargeAccountID   = @ChargeAccountID,          
      @TaxAmount     = @TaxAmount,          
      @TaxNarration    = @TaxNarration,          
      @CessAmount     = @CessAmount,          
      @CessNarration    = @CessNarration,          
      @IsDeferredIncomePosting = 0,          
      @BREFTTrxID              = @BREFTTrxID,          
      @CreatedBy     = @CreatedBy,          
      @CreatedOn     = @CreatedOn,                   
      @ErrorCode     = @ErrorCode OUTPUT,          
      @View      = @View,          
      @IsTrxReversal    = @WaiveCharge,  
   @Narration = @Narration,  
   @IsInterBranch = @IsInterBranch,  
   @IsPendingTrx = @IsPendingTrx  
   End     
   Else IF @WaiveCharge = 0   
   BEGIN  
  IF @Moduleid NOT IN ('3080')  
   BEGIN  
   SET @TrxFlagID = ''  
   
   END  
       
    EXEC p_PostCharge          
      @OurBranchID    = @OurBranchID,          
      @TrxBatchID     = @TrxBatchID,          
      @SerialID     = @SerialID,          
      @ModuleID     = @ModuleID,               
      @ValueDate     = @ValueDate,          
      @CurrencyID     = @CurrencyID,          
      @ChargeID     = @ChargeID,          
      @EffectiveDate    = @EffectiveDate,          
      @ProductID     = @ProductID,              @ChargeAmount    = @ChargeAmount,          
      @CustomerNarration   = @CustomerNarration,              
      @TrxTypeID     = @TrxTypeID,--'T',          
      @ContraAccountTypeID  = @ChargeAccountTypeID,          
      @ContraAccountID   = @ChargeAccountID,          
      @ChargeBranchID    = @ChargeBranchID,          
      @ChargeAccountID   = @ChargeAccountID,          
      @TaxAmount     = @TaxAmount,          
      @TaxNarration    = @TaxNarration,          
      @CessAmount     = @CessAmount,          
      @CessNarration    = @CessNarration,          
      @IsDeferredIncomePosting = 0,          
      @BREFTTrxID              = @BREFTTrxID,          
      @CreatedBy     = @CreatedBy,          
      @CreatedOn     = @CreatedOn,                   
      @ErrorCode     = @ErrorCode OUTPUT,          
      @View      = @View,          
      @IsTrxReversal    = @WaiveCharge,        
   @Narration = @Narration,        
   @IsInterBranch = @IsInterBranch,  
   @TrxflagID=@TrxflagID  
   End   
  -- Else          
  -- Begin          
  --   IF @WaiveCharge = 0          
  --   Begin          
  --    EXEC p_PostCharge          
  --      @OurBranchID    = @OurBranchID,          
  --      @TrxBatchID     = @TrxBatchID,          
  --      @SerialID     = @SerialID,          
  --    @ModuleID     = @ModuleID,             
  --  @ValueDate = @ValueDate,   
  --      @CurrencyID     = @CurrencyID,          
  --      @ChargeID     = @ChargeID,          
  --      @EffectiveDate    = @EffectiveDate,          
  --      @ProductID     = @ProductID,          
  --      @ChargeAmount    = @ChargeAmount,          
  --      @CustomerNarration   = @CustomerNarration,              
  --      @TrxTypeID     = @TrxTypeID,--'T',          
  --      @ContraAccountTypeID  = @ChargeAccountTypeID,          
  --      @ContraAccountID   = @ChargeAccountID,          
  --      @ChargeBranchID    = @ChargeBranchID,          
  --      @ChargeAccountID   = @ChargeAccountID,          
  --      @TaxAmount     = @TaxAmount,          
  --      @TaxNarration    = @TaxNarration,          
  --      @CessAmount     = @CessAmount,          
  --      @CessNarration    = @CessNarration,          
  --      @IsDeferredIncomePosting = 0,          
  --      @BREFTTrxID              = @BREFTTrxID,          
  --      @CreatedBy     = @CreatedBy,          
  --      @CreatedOn     = @CreatedOn,                   
  --      @ErrorCode     = @ErrorCode OUTPUT,          
  --      @View      = @View,          
  --      @IsTrxReversal    = @WaiveCharge,  
  --@Narration = @Narration,  
  --@IsInterBranch = @IsInterBranch,  
  --@IsPendingTrx = @IsPendingTrx  
  --   End          
  --- End            
          
   IF @ErrorCode IS NOT NULL           
   BEGIN          
CLOSE CurChargeDue          
    DEALLOCATE CurChargeDue          
    RAISERROR(@ErrorCode, 16, 1)          
    ROLLBACK TRAN         
    SET @ErrorNo = @ErrorCode          
    RETURN          
   END          
      
   IF ISNULL(@IsInstallmentFee,0)=1    
   BEGIN        
       DECLARE           
       @InstallmentNo tinyint,          
       @InstallmentDueDate smalldatetime,          
       @LoanSeries    smallint          
                 
       SET @LoanSeries=dbo.f_GetMaxLoanSeries(@OurBranchID,@AccountID)          
                 
                 
       SELECT @InstallmentNo=MIN(ISNULL(InstallmentNo,0))         
    FROM t_LoanInstallment(NOLOCK) WHERE OurBranchID= @OurBranchID           
     AND AccountID=@AccountID AND LoanSeries=@LoanSeries          
     AND InstallmentDueDate>=dbo.f_GetWorkingDate(@OurBranchID) AND PaidStatus <>1          
              
    IF @InstallmentNo IS NULL          
     SELECT @InstallmentNo=MAX(ISNULL(InstallmentNo,0))           
     FROM t_LoanInstallment(NOLOCK) WHERE OurBranchID= @OurBranchID           
     AND AccountID=@AccountID AND LoanSeries=@LoanSeries          
                 
       SELECT @InstallmentDueDate=InstallmentDueDate           
       FROM t_LoanInstallment(NOLOCK)           
       WHERE OurBranchID= @OurBranchID AND AccountID=@AccountID           
     AND LoanSeries=@LoanSeries AND InstallmentNo=@InstallmentNo          
                  
    INSERT INTO t_LoanInstallmentFees          
     (          
      OurBranchID,          
AccountID,          
      LoanSeries,          
      FeeID,          
      InstallmentNo,          
      InstallmentDueDate,          
      FeeBalance,          
      FeeDue,          
      Tax ,          
      Others,          
      PaidStatus,          
      AdhocChargeID,          
      CreatedBy,          
      CreatedOn,          
      UpdateCount          
     )          
     SELECT           
      @OurBranchID,          
      @AccountID,          
      @LoanSeries,          
      @CHARGEID,          
      @InstallmentNo,          
      @InstallmentDueDate,          
      @ChargeAmount+@TaxAmount+@CessAmount,          
      @ChargeAmount,          
      @TaxAmount,          
      @CessAmount,          
      0,          
      NULL,          
      @CreatedBy,          
      GETDATE(),          
         2              
                
   END          
          
             
  END          
            
  IF ISNULL(@WaiveCharge,0)=1          
  BEGIN          
          
          INSERT INTO t_AccountChargeExemption           
     (OurBranchID, AccountID, ModuleID, ExceptionDate, ChargeAmount,       
     TaxAmount, CessAmount, ExceptionAmount, ExceptionStatusID, ChargeID,ExemptedBy,TrxBatchID,SerialID)          
    SELECT @OurBranchID, @AccountID, @ModuleID, @TrxDate, @ChargeAmount,          
     @TaxAmount, @CessAmount, @TotalCharge, 'A', @ChargeID,@CreatedBy,@TrxBatchID,@SerialID          
            
            
   END          
          
 FETCH NEXT FROM CurChargeDue INTO @ChargeID,@CurrencyID,@EffectiveDate,@ChargeAmount,@TaxAmount,@CessAmount          
 END          
          
 CLOSE CurChargeDue          
 DEALLOCATE CurChargeDue           
           
EndStatement:          
           
 IF @@ERROR > 0 OR LEN(@ErrorCode) > 2          
 BEGIN          
  IF @@TRANCOUNT > 0          
   ROLLBACK TRAN          
 END          
 ELSE          
 BEGIN          
  IF @@TRANCOUNT > 0          
   COMMIT TRAN          
 END          
             
END   
  
  
   
  
  