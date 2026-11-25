CREATE PROCEDURE [dbo].[p_AddClearingInternalTransfer]  
(   
 @DebitAccountID   AccountID,      
 @DebitOurBranchID  BranchID,      
 @Remarks    description,      
 @TrxRowID    BigInt,      
 @TrxDate    SmallDateTime,      
 @OperatorID    OperatorID      
)   
  
--WITH ENCRYPTION   
AS    
--return      
DECLARE      
 @TrxBranchID   BranchID,      
 @ModuleID    SmallInt,      
 @SerialID    SmallInt = 0,      
 @SlNo     SmallInt,      
 @OurBranchID   BranchID,      
 @AccountTypeID   Char(1),      
 @AccountID    AccountID,      
 @ProductID    ProductID,      
 @TrxTypeID    Char(2),      
 @ValueDate    SmallDateTime,      
 @Amount     Amount,      
 @LocalAmount   Amount,      
 @LocalAmountD   Amount,      
 @TrxCurrencyID   CurrencyID,      
 @TrxAmount    Amount,      
 @ExchangeRate   CurrencyRate,      
 @MeanRate    CurrencyRate,      
 @Profit     Amount,      
 @InstrumentTypeID  Char(1),      
 @ChequeID    Int,      
 @ChequeDate    SmallDateTime,      
 @ReferenceNo   nVarChar(15),      
 @TrxDescriptionID  nVarChar(6),      
 @TrxDescription   nVarChar(100),      
 @MainGLID    AccountID,      
 @TrxFlagID    Char(2),      
 @ForwardRemark   Remarks ='',      
 @DebitAccountType  CHAR(1),      
 @TrxSerialID   INT,      
 @WorkingDate   SmallDateTime,      
 @PTrxBranchID   dbo.BranchID,      
 @PTrxRowID    BigInt = 0,      
 @PTrxBatchID   VarChar(8),      
 @TrxBatchID    VarChar(8),      
 @PSerialID    Int = 0,      
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
 @TrxAmountD    dbo.Amount,      
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
 @ImageID    BigInt = 0,      
 @PTrxPrinted   TinyInt = 0,      
 @PIsTrxPending          Bit=0,      
 @PForwardRemark   dbo.Remarks = '',      
 @PBREFTTrxID            nvarchar(100)=NULL,      
 @PForwardToUser   dbo.OperatorID = NULL,      
 @PForwardToGroup  dbo.OperatorID = NULL,      
 @PCreatedBy    dbo.OperatorID = NULL, -- System Posting Trx      
 @CreatedBy    dbo.OperatorID = NULL,      
 @PCreatedOn    SmallDateTime = NULL,      
 @ErrorNo    nvarchar(20),      
 @DebitProductID   dbo.ProductID = NULL,      
 @DrMainGLID    AccountID,      
 @DRAmount    Amount,      
 @DRLocalAmount   Amount,      
 @ReturnCode    Char(2),      
 @OurBankID    BankID,      
 @AlreadyTransfered  Bit,      
 @ErrorCode    Varchar(50),      
 @VoucherCode   Varchar(4),    
 @Policy1  varchar(100),    
 @OriginatorCode varchar(10),     
 @OriginatorRef  varchar(50),  
 @OldTrxTypeID Char(2),  
 @ExtraDetails Varchar(150)     
      
SET @OurBankID = (SELECT BankID FROM t_SystemBankSetting (NOLOCK))      
SET @WorkingDate = dbo.f_GetWorkingDate(dbo.f_GetHOBranchID(@OurBankID))      
       
IF @TrxDate <> @WorkingDate      
 BEGIN      
  SELECT Top 1       
   @TrxBranchID  = TrxBranchID,      
   @ModuleID   = ModuleID,      
   @OperatorID   = @OperatorID,      
   @SerialID   = SerialID,      
   @SlNo    = 0,      
   @OurBranchID  = OurBranchID,      
   @AccountTypeID  = AccountTypeID,      
   @AccountID   = AccountID,      
   @ProductID   = ProductID,      
   @TrxTypeID   = TrxTypeID,   
   @TrxDate   = TrxDate,      
   @ValueDate   = ValueDate,      
   @Amount    = Amount,      
   @LocalAmount  = LocalAmount,      
   @TrxCurrencyID  = TrxCurrencyID,      
   @TrxAmount   = TrxAmount,      
   @ExchangeRate  = ExchangeRate,      
   @MeanRate   = MeanRate,      
   @Profit    = Profit,      
   @ImageID   = ImageID,      
   @InstrumentTypeID =  Case isNull(ChequeID,0) When 0 Then 'V' Else 'C' End ,      
   @ChequeID   = ChequeID,      
   @ChequeDate   = ChequeDate,      
   @ReferenceNo  = ReferenceNo,      
   @Remarks   = Remarks,      
   @TrxDescriptionID = TrxDescriptionID,      
   @TrxDescription  = TrxDescription,      
   @MainGLID   = MainGLID      
  FROM t_AccountTrx (NOLOCK)      
  WHERE Trxdate = @TrxDate       
   AND TrxRowID = @TrxRowID       
   --AND trxbranchID = dbo.f_GetHOBranchID(@OurBankID)      
         
  SELECT @ReturnCode = ReturnCodeID, @VoucherCode = VoucherCode, @ExtraDetails = VATName  FROM t_AccountTrxClearing WHERE TrxRowID = @TrxRowID AND Date = @TrxDate       
 END      
ELSE      
 BEGIN    
  SELECT Top 1       
   @TrxBranchID  = TrxBranchID,      
   @ModuleID   = ModuleID,      
   @OperatorID   = @OperatorID,      
   @SerialID   = SerialID,      
   @SlNo    = 0,      
   @OurBranchID  = OurBranchID,      
   @AccountTypeID  = AccountTypeID,      
   @AccountID   = AccountID,      
   @ProductID   = ProductID,      
   @TrxTypeID   = TrxTypeID,      
   @TrxDate   = TrxDate,      
   @ValueDate   = ValueDate,      
   @Amount    = Amount,      
   @LocalAmount  = LocalAmount,      
   @TrxCurrencyID  = TrxCurrencyID,      
   @TrxAmount   = TrxAmount,      
   @ExchangeRate  = ExchangeRate,      
   @MeanRate   = MeanRate,      
   @Profit    = Profit,      
   @ImageID   = ImageID,      
   @InstrumentTypeID = Case isNull(ChequeID,0) When 0 Then 'V' Else 'C' End,      
   @ChequeID   = ChequeID,      
   @ChequeDate   = ChequeDate,      
   @ReferenceNo  = ReferenceNo,      
   @Remarks   = Remarks,      
   @TrxDescriptionID = TrxDescriptionID,      
   @TrxDescription  = TrxDescription,      
   @MainGLID   = MainGLID,      
   @TrxFlagID   = TrxFlagID,      
   @ForwardRemark  = ForwardRemark      
  FROM t_Transaction (NOLOCK)      
  WHERE Trxdate = @TrxDate       
   AND TrxRowID = @TrxRowID      
   --AND trxbranchID = dbo.f_GetHOBranchID(@OurBankID)      
         
         
  SELECT @ReturnCode = ReturnCodeID, @VoucherCode = VoucherCode, @ExtraDetails = VATName  FROM t_TrxClearing WHERE TrxRowID = @TrxRowID      
 END      
   
 --SELECT @OurBranchID = OurBranchID FROM t_AccountTrxClearing WHERE TrxRowID = @TrxRowID  
  SELECT @OurBranchID = TrxBranchID FROM t_AccountTrxClearing WHERE TrxRowID = @TrxRowID  
  
 SET @OldTrxTypeID = @TrxTypeID  
   
 SELECT @AccountTypeID=dbo.f_GetAccountTypeID(@DebitOurBranchID,@DebitAccountID)  
      
SET @TrxBranchID = dbo.f_GetHOBranchID(@OurBankID)      
      
IF ISNULL(@DebitAccountID,'')=''        
 SET @DebitOurBranchID = @OurBranchID        
      
IF ISNULL(@DebitAccountID,'')<>''        
BEGIN      
 IF (@DebitAccountID = @AccountID)      
 BEGIN      
  RAISERROR(N'BREXDB999949',16,1)      
  RETURN      
 END      
     
     
   
IF @TrxDate <> @WorkingDate      
 BEGIN       
  SELECT @AlreadyTransfered = ISNULL(IsMDV,0),    
   @Policy1   = PolicyNumber1,    
   @OriginatorCode  = ORIGINATORCODE,    
   @OriginatorRef  = OrigRefCode,    
   @VoucherCode  = VoucherCode       
  FROM t_AccountTrxClearing (NOLOCK)      
  WHERE date = @TrxDate       
    AND TrxRowID = @TrxRowID      
    AND OurbranchID = dbo.f_GetHOBranchID(@OurBankID) AND TrxType IN ('IC','ID')      
 END      
ELSE      
 BEGIN      
  SELECT @AlreadyTransfered = ISNULL(IsMDV,0),    
   @Policy1   = PolicyNumber1,    
   @OriginatorCode  = ORIGINATORCODE,    
   @OriginatorRef  = OrigRefCode,    
   @VoucherCode  = VoucherCode    
  FROM t_TrxClearing (NOLOCK)      
  WHERE date = @TrxDate       
    AND TrxRowID = @TrxRowID      
    AND OurbranchID = dbo.f_GetHOBranchID(@OurBankID) AND TrxType IN ('IC','ID')      
 END     
  
EXEC p_validateClearingTransfers    
 @OurBranchID  = @DebitOurBranchID,       
 @AccountTypeID  = @AccountTypeID,      
 @AccountID   = @DebitAccountID,      
 @ChequeID   = @ChequeID,    
 @Amount    = @Amount,    
 @Policy1   = @Policy1,     
 @OriginatorCode  = @OriginatorCode,    
 @OriginatorRef  = @OriginatorRef,    
 @VoucherCode  = @VoucherCode,    
 @RetunCodeID  = @ReturnCode,   
 @TrxTypeID  = @TrxTypeID,   
 @ErrorNo   = @ErrorCode OUTPUT    
 IF ISNULL(@ErrorCode,'0') <> '0'  AND  ISNULL(@ErrorCode,'0') <> ''   
 BEGIN      
  
  RAISERROR(@ErrorNo,16,1)    
  RETURN      
 END     
   
      
IF @AlreadyTransfered = 1      
BEGIN      
 RAISERROR(N'BREXDB2346567',16,1)      
 RETURN      
END      
      
      
 IF EXISTS( SELECT AccountID       
    FROM t_AccountCustomer (NOLOCK)      
    WHERE OurBranchID = @DebitOurBranchID AND AccountID = @DebitAccountID)      
 SET @DebitAccountType = 'C'      
      
 IF EXISTS( SELECT AccountID      
    FROM t_GLBranch (NOLOCK)      
    WHERE OurBranchID = @DebitOurBranchID AND AccountID = @DebitAccountID)      
 SET @DebitAccountType = 'G'      
END      
   
 IF  @TrxTypeID IN ('ID','TD')  
  BEGIN    
   SET @Amount = Abs(@Amount)      
   SET @DRAmount = @Amount * -1      
   SET @DRLocalAmount = @DRAmount  
   SET @LocalAmount = @Amount    
  END  
 ELSE  
  BEGIN  
   SET @Amount = Abs(@Amount) * -1  
   SET @DRAmount = Abs(@Amount)      
   SET @DRLocalAmount = @DRAmount     
   SET @LocalAmount = @Amount    
  END     
      
--Get new ProductID       
SET @DebitProductID = ISNULL(dbo.f_GetAccountProductID(@DebitOurBranchID,@DebitAccountID),'')      
      
  
IF ISNULL(@ChequeID,0)=0  
BEGIN  
 SET @InstrumentTypeID ='V'  
END  
  
  
  
--Validate the Cheque Series       
IF @TrxTypeID = 'ID' AND @ReturnCode IN ('00','17') AND @VoucherCode <> '40'      
BEGIN      
 SELECT @ErrorNo = dbo.f_ChequeIDValidation(@DebitOurBranchID,'C',@DebitAccountID,@ChequeID)       
 IF ISNULL(@ErrorNo,'') <> ''       
 BEGIN        
  RAISERROR(@ErrorNo,16,1)        
  RETURN           
 END       
END     
  
IF @TrxTypeID = 'ID' AND @VoucherCode <> '40' AND @ReturnCode IN ('00','17')  
 BEGIN  
  --SET  @TrxDescription = @TrxTypeID + ' : To Account No. ' + @DebitAccountID + ' Chq No: ' + CAST (@ChequeID AS Varchar)  
  SET  @TrxDescription = @TrxTypeID + ' Chq No: ' + CAST (@ChequeID AS Varchar)  
 END  
ELSE IF @TrxTypeID = 'ID' AND @VoucherCode <> '40' AND @ReturnCode NOT IN ('00','17')  
 BEGIN  
  SET  @TrxDescription = 'Unpaid Chq No: ' + CAST (@ChequeID AS Varchar)+ ' Rsn: ' + (SELECT DBO.f_CRB_ReturnCodeDescriptions('ReturnCodeID',@ReturnCode,'T'))      
 END  
ELSE   
 BEGIN  
  SET  @TrxDescription = @TrxTypeID + ' : ' + @ExtraDetails  
 END  
  --select @OldTrxTypeID  
 --return  
 --Start Of Local Branch     
   
 If @DebitOurBranchID = @OurBranchID And IsNull(@DebitAccountID,'')<>''        
 BEGIN      
 IF @OldTrxTypeID ='TD'   
  BEGIN  
  SET @OldTrxTypeID = 'ID'  
  END  
 ELSE IF @OldTrxTypeID ='TC'  
  BEGIN  
  SET @OldTrxTypeID = 'IC'  
  END  
   
  
   SET @TrxTypeID = CASE isNull(@OldTrxTypeID,'') WHEN 'ID' THEN 'TC' ELSE 'TD' END   
  
  --Debit GL For ID and Debit GL for IC     
  EXEC p_InsertTransactions        
  @PTrxBranchID  = @DebitOurBranchID,        
  @PTrxRowID   = @TrxRowID OUTPUT,          
  @PTrxBatchID  = @TrxBatchID OUTPUT,        
  @PSerialID   = @SerialID OUTPUT,        
  @POurBranchID  = @DebitOurBranchID,        
  @PAccountTypeID  = 'G',        
  @PAccountID   = @AccountID,        
  @PModuleID   = @ModuleID,        
  @PTrxCodeID   = 0,          
  @PTrxTypeID   = @TrxTypeID,       
  @PTrxDate   = @WorkingDate,        
  @PAmount   = @Amount,        
  @PLocalAmount  = @LocalAmount,        
  @PTrxCurrencyID  = @TrxCurrencyID,        
  @PTrxAmount   = @TrxAmount,        
  @PExchangeRate  = @ExchangeRate,        
  @PMeanRate   = @MeanRate,        
  @PProfit   = @Profit,        
  @PInstrumentTypeID = @InstrumentTypeID,        
  @PChequeID   = @ChequeID,        
  @PChequeDate  = @WorkingDate,         
  @PTrxDescriptionID = @TrxDescriptionID,      
  @PTrxDescription = @TrxDescription,        
  @PTrxFlagID   = 'U',        
  @PIsTrxPending = 0,        
  @PCreatedBy   = @CreatedBy,        
  @PImageID   = @ImageID      
       
  
  
  SET @TrxTypeID = CASE isNull(@OldTrxTypeID,'') WHEN 'ID' THEN 'TD' ELSE 'TC' END   
     
  --Credit Customer A/c for ID and Credit Customer for IC      
  EXEC p_InsertTransactions        
  @PTrxBranchID  = @DebitOurBranchID,        
  @PTrxRowID   = @TrxRowID OUTPUT,          
  @PTrxBatchID  = @TrxBatchID OUTPUT,        
  @PSerialID   = @SerialID OUTPUT,        
  @POurBranchID  = @DebitOurBranchID,        
  @PAccountTypeID  = @DebitAccountType,      
  @PAccountID   = @DebitAccountID,        
  @PModuleID   = @ModuleID,      
  @PProductID   = @DebitProductID,       
  @PTrxCodeID   = 0,          
  @PTrxTypeID   =  @TrxTypeID,         
  @PTrxDate   = @WorkingDate,        
  @PAmount   = @DRAmount,        
  @PLocalAmount  = @DRLocalAmount,        
  @PTrxCurrencyID  = @TrxCurrencyID,        
  @PTrxAmount   = @TrxAmount,        
  @PExchangeRate  = @ExchangeRate,        
  @PMeanRate   = @MeanRate,        
  @PProfit   = @Profit,        
  @PInstrumentTypeID = @InstrumentTypeID,        
  @PChequeID   = @ChequeID,        
  @PChequeDate  = @WorkingDate,         
  @PTrxDescriptionID = @TrxDescriptionID,        
  @PTrxDescription = @TrxDescription,        
  @PTrxFlagID   = 'U',        
 @PIsTrxPending  = 0,        
  @PCreatedBy   = @CreatedBy,      
  @PImageID   = @ImageID      
 END      
 ELSE      
 BEGIN      
       
  SET @SerialID = ISNULL((SELECT MAX(SerialID) FROM t_TrxTransfer        
    WHERE TrxBranchID= @DebitOurBranchID AND OperatorID = @CreatedBy),0) + 1        
  SET @DrMainGLID = dbo.f_GetGLInterfaceAccountID1(dbo.f_GetBankID(@TrxBranchID),@DebitProductID,'CONTROL_AC')      
      
 IF @TrxTypeID = 'ID' AND @ReturnCode NOT IN ('00','17')      
 BEGIN      
  SET @TrxDescription = @TrxDescription +  ':Chq No. ' + Cast ( @ChequeID as Varchar)      
  SET @InstrumentTypeID ='V'      
 END      
    
 IF @OldTrxTypeID ='TD'   
  BEGIN  
  SET @OldTrxTypeID = 'ID'  
  END  
 ELSE IF @OldTrxTypeID ='TC'  
  BEGIN  
  SET @OldTrxTypeID = 'IC'  
  END  
  
   
   SET @TrxTypeID = CASE isNull(@OldTrxTypeID,'') WHEN 'ID'  THEN 'TC' ELSE 'TD' END   
  
    --Debit GL 4 ID/ Debit GL 4 IC     
     EXEC p_AddEditTransferTrx      
     @TrxBranchID  = @OurBranchID,      
     @OperatorID   = @OperatorID,      
     @ModuleID   = '3030',      
     @SerialID   = @SerialID,      
     @SlNo    = 2,      
     @OurBranchID  = @OurBranchID,      
     @AccountTypeID  = 'G',      
     @AccountID   = @AccountID,      
     @ProductID   = N'GL',      
     @TrxTypeID   = @TrxTypeID,    
     @TrxDate   = @WorkingDate,      
     @ValueDate   = @WorkingDate,      
     @Amount    = @Amount,      
     @LocalAmount  = @LocalAmount,      
     @TrxCurrencyID  = @TrxCurrencyID,      
     @TrxAmount   = @TrxAmount,      
     @ExchangeRate  = @ExchangeRate,      
     @MeanRate   = @MeanRate,      
     @Profit    = @Profit,      
     @InstrumentTypeID = 'V',      
     @ChequeID   = @ChequeID,      
     @ChequeDate   = @WorkingDate,      
     @ReferenceNo  = @ReferenceNo,      
     @Remarks   = @Remarks,      
     @TrxDescriptionID = N'007',      
     @TrxDescription  = @TrxDescription,      
     @MainGLID   = @MainGLID,      
     @TrxFlagID   = 'U',      
     @ForwardRemark  = @ForwardRemark      
           
     --Credit CustomerA/C  for ID \ Credit CustomerA/C  for IC      
   SET @TrxTypeID = CASE isNull(@OldTrxTypeID,'') WHEN 'ID' THEN 'TD' ELSE 'TC' END   
  
   IF(@DebitAccountType='G') SET @DrMainGLID=@DebitAccountID   
  
    EXEC p_AddEditTransferTrx      
     @TrxBranchID  = @OurBranchID,       
     @OperatorID   = @OperatorID,       
     @ModuleID   = '3030',      
     @SerialID   = @SerialID,       
     @SlNo    = 1,      
     @OurBranchID  = @DebitOurBranchID,      
     @AccountTypeID  = @DebitAccountType,      
     @AccountID   = @DebitAccountID,      
     @ProductID   = @DebitProductID,      
     @TrxTypeID = @TrxTypeID,        
     @TrxDate   = @WorkingDate,      
     @ValueDate   = @WorkingDate,      
     @Amount    = @DRAmount,      
     @LocalAmount  = @DRLocalAmount,      
     @TrxCurrencyID  = @TrxCurrencyID,      
     @TrxAmount   = @TrxAmount,   
     @ExchangeRate  = @ExchangeRate,      
     @MeanRate   = @MeanRate,      
     @Profit    = @Profit,      
     @InstrumentTypeID = @InstrumentTypeID,      
     @ChequeID   = @ChequeID,      
     @ChequeDate   = @WorkingDate,      
     @ReferenceNo  = @ReferenceNo,      
     @Remarks   = @Remarks,      
     @TrxDescriptionID = N'008',      
     @TrxDescription  = @TrxDescription,      
     @MainGLID   = @DrMainGLID,      
     @TrxFlagID   = 'U',      
     @ForwardRemark  = @ForwardRemark      
 END      
     
IF @DebitOurBranchID <> @OurBranchID       
 BEGIN      
  EXEC p_AddTransferTrxIB     
   @TrxBranchID   = @OurBranchID,      
   @OperatorID    = @OperatorID,      
   @SerialID    = @SerialID,       
   @TrxPrinted    = 0,      
   @TrxBatchID    = @TrxBatchID OUTPUT,      
   @DoNotReturnBatchID  = 0,      
   @PImageID    = @ImageID      
 END      
      
  
DECLARE @NewTrxRowID BigInt      
UPDATE t_Transaction SET ImageID = @ImageID WHERE TrxBatchID = @TrxBatchID AND TrxBranchID =@TrxBranchID AND ABS(TrxAmount) = ABS(@TrxAmount)      
      
SELECT @NewTrxRowID = TrxRowID       
FROM t_Transaction (NOLOCK)       
WHERE TrxBatchID = @TrxBatchID AND TrxBranchID =@TrxBranchID AND ABS(TrxAmount) = ABS(@TrxAmount) AND TrxBatchSLNo = 1      
      
UPDATE t_IRDReport       
SET  TrxRowID = @NewTrxRowID      
WHERE ImageID = @TrxRowID      
      
IF @TrxTypeID IN ('ID','TD') AND @ReturnCode NOT IN ('00','17')      
BEGIN      
 IF @ReturnCode NOT IN ('00','17') AND @DebitAccountType = 'C'      
 BEGIN      
  IF @ReturnCode = '63'      
   BEGIN      
    SET @TrxDescriptionID = '086'      
   END       
  ELSE IF @ReturnCode  = '62'      
   BEGIN      
    SET @TrxDescriptionID = '087'      
   END       
  ELSE      
   BEGIN      
    SET @TrxDescriptionID = '088'      
   END       
 END      
 EXEC p_ChargeTransaction          
   @OurBranchID  = @DebitOurBranchID,         
   @AccountID   = @DebitAccountID,         
   @TrxDescriptionID = @TrxDescriptionID,      
   @TrxCurrencyID  = @TrxCurrencyID,         
   @ModuleID   = '3030',      
   @CreatedBy   = 'SYS',       
   @ErrorNo   = @ErrorCode OUTPUT         
   IF ISNULL(@ErrorCode ,'0') <> '0'        
   BEGIN        
  RAISERROR(N' BREXDB123456',16,1)        
   END          
    
 IF @ReturnCode IN ('17')     
 BEGIN   
 DELETE   
 FROM t_ChequeTrx  
 WHERE OurBranchID = @OurBranchID  
  AND AccountTypeID = @AccountTypeID  
  AND AccountID = @AccountID  
  AND ChequeID = @ChequeID  
  AND ISNULL(@ChequeID,0) <> 0   
 END  
   
      
 IF @TrxDate = @WorkingDate      
  BEGIN      
   UPDATE t_TrxClearing       
   SET  AccountID = @DebitAccountID,       
     OurBranchID = @DebitOurBranchID,      
     IsMDV  = 1      
   WHERE TrxType = 'ID' AND ReturnCodeID NOT IN ('00','17') AND TrxRowID = @TrxRowID      
  END      
 ELSE      
  BEGIN      
   UPDATE t_AccountTrxClearing       
   SET  AccountID = @DebitAccountID,       
     OurBranchID = @DebitOurBranchID,      
     IsMDV  = 1      
   WHERE TrxType = 'ID' AND ReturnCodeID NOT IN ('00','17') AND Date = @TrxDate AND TrxRowID = @TrxRowID      
  END      
END      
ELSE      
BEGIN      
IF @TrxDate = @WorkingDate      
 BEGIN      
   UPDATE t_TrxClearing       
   SET  IsMDV  = 1      
   WHERE TrxRowID = @TrxRowID AND AccountID = dbo.f_GetCurrencyBranchGLAccountID(@TrxBranchID,@TrxCurrencyID, 'ACP_CLR_SUSP_AC')      
   AND  TrxBranchID = @TrxBranchID      
 END      
ELSE      
 BEGIN      
   UPDATE t_AccountTrxClearing       
   SET  IsMDV  = 1      
   WHERE TrxRowID = @TrxRowID AND AccountID = dbo.f_GetCurrencyBranchGLAccountID(@TrxBranchID,@TrxCurrencyID, 'ACP_CLR_SUSP_AC')      
   AND  TrxBranchID = @TrxBranchID      
 END      
END   
  
--GO  
--BEGIN TRAN  
--exec p_AddClearingInternalTransfer @DebitAccountID=N'21100100',@DebitOurBranchID=N'004',  
--@Remarks=N'B CHQ 003597 ISSUED TO TANESCO',@TrxRowID=408647,@TrxDate='2016-05-20 00:00:00',@OperatorID=N'MKAM001'  
--select * from t_transaction order by TrxRowID desc  
--ROLLBACK TRAN  
  
  
  