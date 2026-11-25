  
CREATE PROCEDURE dbo.p_GetOutwardTrx      
(      
 @TrxBranchID BranchID,      
 @OurBranchID BranchID,      
 @SerialID  Int,      
 @OperatorID  OperatorID,      
 @ModuleID  SmallInt      
)    
     
AS      
BEGIN      
 SET NOCOUNT ON      
      
 DECLARE @AccountID  AccountID,      
   @AccountTypeID SystemSubID,      
   @AccountClassID nVarChar(10),      
   @TrxBatchID  VarChar(8),      
   @PrevSol  bit,      
   @TrxDate  SmallDateTime      
      
 DECLARE @ChargeAmount  Amount,      
   @ChargeCurrencyID CurrencyID,      
   @BankID    BankID,      
   @DeletedOn   SmallDateTime,      
   @ChargeDue   dbo.Amount      
      
 DECLARE @ShowBehindSceneData Bit      
      
 DECLARE @LanguageID Varchar(3)      
      
 SET @LanguageID = dbo.f_GetUserLanguageID(@OperatorID)      
      
      
 SET @ShowBehindSceneData = 1      
      
 SELECT @BankID = dbo.f_GetBankID(@OurBranchID)      
      
 SELECT @AccountID  = AccountID,      
   @AccountTypeID = AccountTypeID,      
   @TrxBatchID  = TrxBatchID,      
   @DeletedOn  = DeletedOn,      
   @TrxDate  = dbo.f_GetWorkingDate(@OurBranchID),      
   @PrevSol  = 1      
      
 FROM t_Transaction  (NOLOCK)      
 WHERE TrxBranchID = @TrxBranchID      
  AND OurBranchID = @OurBranchID      
  AND SerialID = @SerialID      
  AND ModuleID = @ModuleID      
  AND TrxCodeID = 0      
       
    
     
 IF ISNULL(@TrxBatchID,'') = ''      
 BEGIN       
  SELECT @AccountID  = AccountID,      
   @AccountTypeID = AccountTypeID,      
   @TrxBatchID  = TrxBatchID,      
   @DeletedOn  = DeletedOn,      
   @TrxDate  = dbo.f_GetWorkingDate(@OurBranchID),      
   @PrevSol  = 0      
      
  FROM t_Transaction (NOLOCK)       
  WHERE     
  --TrxBranchID = @TrxBranchID      
   --AND OurBranchID = @OurBranchID      
   --AND     
   TrxRowID = @SerialID      
   --AND ModuleID = @ModuleID      
         
 END       
 IF ISNULL(@TrxBatchID,'') = ''      
 BEGIN      
 SELECT @AccountID  = AccountID,      
   @AccountTypeID = AccountTypeID,      
   @TrxBatchID  = TrxBatchID,      
   @TrxDate  = TrxDate,      
   @PrevSol  = 0      
 FROM t_AccountTrx (NOLOCK)       
 WHERE OurBranchID = @OurBranchID      
   AND ModuleID  IN (@ModuleID,'3030')      
   AND TrxRowID = @SerialID       
 END      
     
 SET @ChargeDue = (SELECT dbo.f_GetClientChargeDue(@OurBranchID,@AccountID))      
        
 IF ISNULL(@TrxBatchID,'') = ''      
 BEGIN      
  RAISERROR('BREXDB300001',16,1)      
  RETURN      
 END      
      
 --- Deleted Transaction       
 IF NOT @DeletedOn Is Null      
 BEGIN      
  RAISERROR('BREXDB300014',16,1)      
  RETURN      
 END      
      
 IF @AccountTypeID = 'C'      
 BEGIN      
  SELECT @AccountClassID = AccountClassID      
  FROM t_AccountCustomer (NOLOCK)       
  WHERE OurBranchID = @OurBranchID      
   AND AccountID = @AccountID      
 END      
 ELSE      
 BEGIN      
  SELECT @AccountClassID = GLClassID      
  FROM t_GeneralLedger (NOLOCK)       
  WHERE BankID  = @BankID      
   AND AccountID = @AccountID      
 END      
      
 --Acess Rights, if less then check 917 system parameters      
 IF (SELECT AccessLevel       
  FROM t_UserRole  (NOLOCK) WHERE OurBranchID = @OurBranchID      
   AND OperatorID = @OperatorID) <      
  (SELECT Value FROM t_SpecialConditionDetail (NOLOCK)       
   WHERE BankID = @BankID      
    AND ClassID = @AccountClassID      
    AND SpecialConditionID = 907)       
 BEGIN      
  --Allow Transaction When User Access Level is Less than the Account Access Level      
  IF NOT EXISTS(SELECT OurBranchID FROM t_SystemBranchParameter  (NOLOCK)      
    WHERE OurBranchID = @OurBranchID AND ParamValue = 1 AND SysParamID = 31)      
  BEGIN      
   RAISERROR(N'BREXDB051022',16,1)      
   RETURN      
  END      
  ELSE      
  BEGIN      
   --In this case not show behind scene data      
   SET @ShowBehindSceneData = 0      
  END      
 END      
 IF @PrevSol = 1      
 BEGIN    
 If @ModuleID In (3071,3061)       
 SELECT TOP 1 t_Transaction.TrxRowID,      
   t_Transaction.TrxBatchID,      
   t_Transaction.TrxTypeID,      
   t_Transaction.TrxCurrencyID,      
   dbo.f_GetCurrencyName(t_Transaction.TrxCurrencyID) CurrencyName,      
   t_Transaction.AccountTypeID,      
   t_Transaction.AccountID,      
      
   t_TrxClearing.BankID BankID,      
   dbo.f_GetClearingBankName(t_TrxClearing.BankID) BankName,      
   t_TrxClearing.BranchID BranchID,      
   dbo.f_GetClearingBranchName(t_Transaction.OurBranchID,t_TrxClearing.BankID,t_TrxClearing.BranchID) BranchName,      
         
   t_Transaction.ValueDate,      
   t_Transaction.InstrumentTypeID,    -- In Outward Credit it is always Cheque.      
   t_Transaction.ChequeID,            
   t_Transaction.ChequeDate,         
   t_TrxClearing.DrawerOrPayeeAccountID,      
   t_TrxClearing.DrawerOrPayee,      
      
   t_TrxClearing.ChequeDigit,      
   t_TrxClearing.VoucherCode,      
   t_TrxClearing.ReturnCodeID,        
   t_Transaction.ReferenceNo,      
   t_Transaction.Remarks,      
   t_Transaction.TrxDescriptionID,      
         
   --t_Transaction.TrxDescription,     -- Narration      
      --Changed to add more details in the Description BankID and chqID, but they shd not be visible when editing Requested by moses for UTB      
   CASE WHEN t_Transaction.TrxDescription like '%- BankID:%' THEN      
     Substring(t_Transaction.TrxDescription, 1,Charindex('- BankID:', t_Transaction.TrxDescription)-1) -- Narration      
    ELSE t_Transaction.TrxDescription       
   END      
   AS TrxDescription,      
      
   t_Transaction.TrxDate,      
   ABS(t_Transaction.Amount) Amount,      
   t_TrxClearing.Commission,      
   t_TrxClearing.TheirCommission,      
   t_TrxClearing.VATPAYType,      
   t_TrxClearing.VATSerialNo,      
   t_TrxClearing.VATPAYEMonth,      
   t_TrxClearing.VATPAYECommission,      
   t_Transaction.TrxPrinted,      
   t_Transaction.TrxFlagID,      
   t_Transaction.ImageID,      
   dbo.fn_GetSystemCodeDesc('TrxFlagID', t_Transaction.TrxFlagID,@LanguageID) TrxFlag      
 FROM t_Transaction(NOLOCK)      
 INNER JOIN t_TrxClearing  (NOLOCK)        
ON t_Transaction.TrxBatchID = t_TrxClearing.TrxBatchID      
  --AND t_Transaction.TrxBatchSLNo = t_TrxClearing.TrxBatchSLNo      
 WHERE t_Transaction.TrxBranchID = @TrxBranchID      
  AND t_Transaction.OurBranchID = @OurBranchID      
  AND t_Transaction.SerialID  = @SerialID      
  AND t_Transaction.ModuleID  = @ModuleID      
  AND t_Transaction.DeletedOn IS NULL      
  AND t_Transaction.TrxCodeID = 0       
 ORDER BY t_Transaction.TrxBatchSLNo      
 Else       
 SELECT t_Transaction.TrxRowID,      
   t_Transaction.TrxBatchID,      
   t_Transaction.TrxTypeID,      
   t_Transaction.TrxCurrencyID,      
   dbo.f_GetCurrencyName(t_Transaction.TrxCurrencyID) CurrencyName,      
   t_Transaction.AccountTypeID,      
   t_Transaction.AccountID,      
      
   t_TrxClearing.BankID BankID,      
   dbo.f_GetClearingBankName(t_TrxClearing.BankID) BankName,      
   t_TrxClearing.BranchID BranchID,      
   dbo.f_GetClearingBranchName(t_Transaction.OurBranchID,t_TrxClearing.BankID,t_TrxClearing.BranchID) BranchName,      
         
   t_Transaction.ValueDate,      
   t_Transaction.InstrumentTypeID,    -- In Outward Credit it is always Cheque.      
   t_Transaction.ChequeID,            
   t_Transaction.ChequeDate,         
   t_TrxClearing.DrawerOrPayeeAccountID,      
   t_TrxClearing.DrawerOrPayee,      
      
   t_TrxClearing.ChequeDigit,      
   t_TrxClearing.VoucherCode,      
   t_TrxClearing.ReturnCodeID,        
   t_Transaction.ReferenceNo,      
   t_Transaction.Remarks,      
   t_Transaction.TrxDescriptionID,      
   --t_Transaction.TrxDescription,    -- Narration      
   --Changed to add more details in the Description BankID and chqID, but they shd not be visible when editing      
   CASE WHEN t_Transaction.TrxDescription like '%- BankID:%' THEN      
     Substring(t_Transaction.TrxDescription, 1,Charindex('- BankID:', t_Transaction.TrxDescription)-1) -- Narration      
    ELSE t_Transaction.TrxDescription       
   END      
   AS TrxDescription,      
   t_Transaction.TrxDate,      
   ABS(t_Transaction.Amount) Amount,      
   t_TrxClearing.Commission,      
   t_TrxClearing.TheirCommission,      
   t_TrxClearing.VATPAYType,      
   t_TrxClearing.VATSerialNo,      
   t_TrxClearing.VATPAYEMonth,      
   t_TrxClearing.VATPAYECommission,      
   t_Transaction.TrxPrinted,      
   t_Transaction.TrxFlagID,      
   t_Transaction.ImageID,      
   dbo.fn_GetSystemCodeDesc('TrxFlagID', t_Transaction.TrxFlagID,@LanguageID) TrxFlag      
 FROM t_Transaction(NOLOCK)      
 INNER JOIN t_TrxClearing  (NOLOCK)       
 ON t_Transaction.TrxBranchID = t_TrxClearing.TrxBranchID      
  AND t_Transaction.TrxBatchID = t_TrxClearing.TrxBatchID      
  --AND t_Transaction.TrxBatchSLNo = t_TrxClearing.TrxBatchSLNo      
  AND t_Transaction.TrxRowID = t_TrxClearing.TrxRowID      
 WHERE t_Transaction.TrxBranchID = @TrxBranchID      
  AND t_Transaction.OurBranchID = @OurBranchID      
  AND t_Transaction.SerialID  = @SerialID      
  AND t_Transaction.ModuleID  = @ModuleID      
  AND t_Transaction.DeletedOn IS NULL      
  AND t_Transaction.TrxCodeID = 0       
 ORDER BY t_Transaction.TrxBatchSLNo      
 END      
 ELSE      
 BEGIN      
  IF @TrxDate = dbo.f_GetWorkingDate(@OurBranchID)      
 BEGIN   
     
  If @ModuleID In (3071,3061)  
    
  SELECT t_Transaction.TrxRowID,      
    t_Transaction.TrxBatchID,      
    t_Transaction.TrxTypeID,      
    t_Transaction.TrxCurrencyID,      
    dbo.f_GetCurrencyName(t_Transaction.TrxCurrencyID) CurrencyName,      
    t_Transaction.AccountTypeID,      
    t_Transaction.AccountID,      
      
    t_TrxClearing.BankID BankID,      
    dbo.f_GetClearingBankName(t_TrxClearing.BankID) BankName,      
    t_TrxClearing.BranchID BranchID,      
    dbo.f_GetClearingBranchName(t_Transaction.OurBranchID,t_TrxClearing.BankID,t_TrxClearing.BranchID) BranchName,      
          
    t_Transaction.ValueDate,      
    t_Transaction.InstrumentTypeID,    -- In Outward Credit it is always Cheque.      
    t_Transaction.ChequeID,            
    t_Transaction.ChequeDate,         
    t_TrxClearing.DrawerOrPayeeAccountID,      
    t_TrxClearing.DrawerOrPayee,      
      
    t_TrxClearing.ChequeDigit,      
    t_TrxClearing.VoucherCode,      
    t_TrxClearing.ReturnCodeID,        
    t_Transaction.ReferenceNo,      
    t_Transaction.Remarks,      
    t_Transaction.TrxDescription,     -- Narration      
      
    t_Transaction.TrxDate,      
    ABS(t_Transaction.Amount) Amount,      
    t_TrxClearing.Commission,      
    t_TrxClearing.TheirCommission,      
    t_TrxClearing.VATPAYType,      
    t_TrxClearing.VATSerialNo,      
    t_TrxClearing.VATPAYEMonth,      
    t_TrxClearing.VATPAYECommission,      
    t_Transaction.TrxPrinted,      
    t_Transaction.TrxFlagID,      
    t_Transaction.ImageID,      
    dbo.fn_GetSystemCodeDesc('TrxFlagID', t_Transaction.TrxFlagID,@LanguageID) TrxFlag      
  FROM t_Transaction  (NOLOCK)      
  INNER JOIN t_TrxClearing  (NOLOCK)       
  ON t_Transaction.TrxBatchID = t_TrxClearing.TrxBatchID  
  WHERE     
   t_Transaction.OurBranchID = @OurBranchID      
   AND t_Transaction.ModuleID  IN (@ModuleID ,'3030')     
   AND t_Transaction.DeletedOn IS NULL      
   AND t_Transaction.TrxCodeID = 0       
  ORDER BY t_Transaction.TrxBatchSLNo      
  Else       
  SELECT t_Transaction.TrxRowID,      
    t_Transaction.TrxBatchID,      
    t_Transaction.TrxTypeID,      
    t_Transaction.TrxCurrencyID,      
    dbo.f_GetCurrencyName(t_Transaction.TrxCurrencyID) CurrencyName,      
    t_Transaction.AccountTypeID,      
    t_Transaction.AccountID,      
      
    t_TrxClearing.BankID BankID,      
    dbo.f_GetClearingBankName(t_TrxClearing.BankID) BankName,      
    t_TrxClearing.BranchID BranchID,      
    dbo.f_GetClearingBranchName(t_Transaction.OurBranchID,t_TrxClearing.BankID,t_TrxClearing.BranchID) BranchName,      
          
    t_Transaction.ValueDate,      
    t_Transaction.InstrumentTypeID,    -- In Outward Credit it is always Cheque.      
    t_Transaction.ChequeID,            
    t_Transaction.ChequeDate,         
    t_TrxClearing.DrawerOrPayeeAccountID,      
    t_TrxClearing.DrawerOrPayee,      
      
    t_TrxClearing.ChequeDigit,      
    t_TrxClearing.VoucherCode,      
    t_TrxClearing.ReturnCodeID,        
    t_Transaction.ReferenceNo,      
    t_Transaction.Remarks,      
    t_Transaction.TrxDescription,     -- Narration      
      
    t_Transaction.TrxDate,      
    ABS(t_Transaction.Amount) Amount,      
    t_TrxClearing.Commission,      
    t_TrxClearing.TheirCommission,      
  t_TrxClearing.VATPAYType,      
    t_TrxClearing.VATSerialNo,      
    t_TrxClearing.VATPAYEMonth,      
    t_TrxClearing.VATPAYECommission,      
    t_Transaction.TrxPrinted,      
    t_Transaction.TrxFlagID,      
    t_Transaction.ImageID,      
    dbo.fn_GetSystemCodeDesc('TrxFlagID', t_Transaction.TrxFlagID,@LanguageID) TrxFlag      
  FROM t_Transaction (NOLOCK)       
  INNER JOIN t_TrxClearing  (NOLOCK)       
  ON  Abs(t_Transaction.amount) = abs(t_TrxClearing.amount)    
   AND t_Transaction.ChequeID =t_TrxClearing.ChequeID     
   AND t_Transaction.TrxBranchID =t_TrxClearing.TrxBranchID     
   AND  t_Transaction.ImageID =t_TrxClearing.ImageID     
   --t_Transaction.OurBranchID = t_TrxClearing.OurBranchID      
  WHERE     
  --t_Transaction.TrxBranchID = @TrxBranchID      
   --AND t_Transaction.OurBranchID = @OurBranchID      
   --AND     
   t_Transaction.TrxRowID  = @SerialID       
   --AND  t_Transaction.TrxBatchSLNo = (CASE WHEN @ModuleID='3040' Then 3 ELSE 1 END)      
   --AND t_Transaction.TrxBatchID = @TrxBatchID      
   --AND t_Transaction.ModuleID  = @ModuleID      
   AND t_Transaction.DeletedOn IS NULL      
   --AND t_Transaction.TrxCodeID = 0       
  ORDER BY t_Transaction.TrxBatchSLNo      
  END      
 ELSE      
 BEGIN      
  SELECT DISTINCT t_AccountTrx.TrxRowID,      
    t_AccountTrx.TrxBatchID,      
    t_AccountTrx.TrxTypeID,      
    t_AccountTrx.TrxCurrencyID,      
    dbo.f_GetCurrencyName(t_AccountTrx.TrxCurrencyID) CurrencyName,      
    t_AccountTrx.AccountTypeID,      
    t_AccountTrx.AccountID,      
      
    t_AccountTrxClearing.BankID BankID,      
    dbo.f_GetClearingBankName(t_AccountTrxClearing.BankID) BankName,      
    t_AccountTrxClearing.BranchID BranchID,      
    dbo.f_GetClearingBranchName(t_AccountTrx.OurBranchID,t_AccountTrxClearing.BankID,t_AccountTrxClearing.BranchID) BranchName,      
          
    t_AccountTrx.ValueDate,      
    'V' InstrumentTypeID,    -- In Outward Credit it is always Cheque.      
    t_AccountTrx.ChequeID,            
    t_AccountTrx.ChequeDate,         
    t_AccountTrxClearing.DrawerOrPayeeAccountID,      
    t_AccountTrxClearing.DrawerOrPayee,      
      
    t_AccountTrxClearing.ChequeDigit,      
    t_AccountTrxClearing.VoucherCode,      
    t_AccountTrxClearing.ReturnCodeID,        
    t_AccountTrx.ReferenceNo,      
    t_AccountTrx.Remarks,      
    t_AccountTrx.TrxDescription,     -- Narration      
      
    @TrxDate,      
    ABS(t_AccountTrx.Amount) Amount,      
    t_AccountTrxClearing.Commission,      
    t_AccountTrxClearing.TheirCommission,      
    t_AccountTrxClearing.VATPAYType,      
    t_AccountTrxClearing.VATSerialNo,      
    t_AccountTrxClearing.VATPAYEMonth,      
    t_AccountTrxClearing.VATPAYECommission,      
    t_AccountTrx.TrxPrinted,      
    ' ' TrxFlagID,      
    t_AccountTrx.ImageID,      
    dbo.fn_GetSystemCodeDesc('TrxFlagID', '' ,@LanguageID) TrxFlag      
  FROM t_AccountTrx  (NOLOCK)    
  INNER JOIN t_AccountTrxClearing (NOLOCK)        
  ON t_AccountTrx.ChequeID = t_AccountTrxClearing.ChequeID     
   AND t_AccountTrx.ImageID = t_AccountTrxClearing.ImageID  
   AND ABS(t_AccountTrx.Amount) = ABS(t_AccountTrxClearing.Amount)      
  WHERE t_AccountTrx.OurBranchID = @OurBranchID      
   AND t_AccountTrx.TrxBatchID = @TrxBatchID      
   AND t_AccountTrx.ModuleID  IN (@ModuleID,'3030')  
   AND t_AccountTrx.AccountTypeID = @AccountTypeID   
   AND t_AccountTrx.TrxRowID = @SerialID    
 END      
       
 END      
 --In the case of View we have to think whether we require to       
 --send if not having Access Level      
 IF @AccountTypeID ='C'      
 BEGIN      
  SELECT       
   t_AccountCustomer.Name AccountName,      
t_AccountCustomer.ClearBalance,      
   t_AccountCustomer.UnclearBalance,      
   --t_AccountCustomer.DrawingPower,      
   dbo.f_GetAccountLimit(t_AccountCustomer.OurBranchID,t_AccountCustomer.AccountID) DrawingPower,      
   t_AccountCustomer.FreezedAmount,      
--   t_ProductBranchDetail.MinimumBalance,      
   dbo.f_GetMinimumBalance(t_AccountCustomer.OurBranchID,t_AccountCustomer.AccountID) MinimumBalance,      
   dbo.f_GetAvailableBalance(t_AccountCustomer.OurBranchID,t_AccountCustomer.AccountID) AvailableBalance,   
   t_AccountCustomer.ProductID,      
   @AccountClassID AccountClassID,      
   dbo.f_GetAccountCurrencyID(t_AccountCustomer.OurBranchID,t_AccountCustomer.AccountID) CurrencyID,      
   t_AccountCustomer.UnSupervisedCredits,      
   t_AccountCustomer.UnSupervisedDebits,      
      
   @ShowBehindSceneData ShowBehindSceneData,      
   t_AccountCustomer.UpdateCount      
      
  FROM t_AccountCustomer  (NOLOCK),t_ProductBranchDetail  (NOLOCK)      
  WHERE t_AccountCustomer.OurBranchID = t_ProductBranchDetail.OurBranchID      
   AND t_AccountCustomer.ProductID = t_ProductBranchDetail.ProductID      
   AND t_AccountCustomer.OurBranchID = @OurBranchID      
   AND t_AccountCustomer.AccountID = @AccountID      
 END      
 ELSE      
 BEGIN      
  SELECT       
   t_GeneralLedger.Description AccountName,      
   LocalBalance ClearBalance,      
   0 UnclearBalance,      
   0 DrawingPower,      
   0 FreezedAmount,      
   0 MinimumBalance,      
   0 AvailableBalance,      
      
   'GL' ProductID,      
   @AccountClassID AccountClassID,      
   CurrencyID,      
   0 UnSupervisedCredits,      
   0 UnSupervisedDebits,      
   @ShowBehindSceneData ShowBehindSceneData,      
   t_GLBranch.UpdateCount      
      
  FROM t_GLBranch (NOLOCK),t_GeneralLedger (NOLOCK)      
  WHERE t_GLBranch.BankID  = t_GeneralLedger.BankID      
   AND t_GLBranch.AccountID = t_GeneralLedger.AccountID      
   AND t_GLBranch.OurBranchID = @OurBranchID      
   AND t_GLBranch.AccountID = @AccountID      
   AND t_GLBranch.GLAccountStatusID = 'A'      
 END      
      
 SET NOCOUNT OFF      
END      
      
  
  