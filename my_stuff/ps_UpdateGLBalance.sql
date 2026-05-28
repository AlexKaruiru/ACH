CREATE PROCEDURE [dbo].[ps_UpdateGLBalance]  
(  
 @OurBranchID BranchID,  
 @WorkingDate SmallDateTime,  
 @ErrorNo  Int = 0 OUTPUT  
)  
   
AS  
BEGIN  
 SET NOCOUNT ON  
   
 DECLARE @LocalCurrencyID CurrencyID,  
  
   @BankID    BankID,  
   @TrxBatchID   Varchar(8),  
   @TrxSerialID  Int,  
     
   @CntTrx    Int,  
   @CntTrxHis   Int  
  
 SET @BankID = dbo.f_GetBankID(@OurBranchID)  
 SET @LocalCurrencyID = dbo.f_GetLocalCurrencyID(@OurBranchID)  
 BEGIN TRAN  
  
 BEGIN TRY  
 -- STEP 2: UPdate GL Balances.  
  
  
 UPDATE t_GLBranch  
 SET t_GLBranch.LocalBalance  = t_GLBranch.LocalBalance + LocalAmount,  
  t_GLBranch.ForeignBalance = t_GLBranch.ForeignBalance +   
          CASE   
           WHEN CurrencyID = @LocalCurrencyID THEN 0  
           ELSE  Amount  
          END  
 FROM t_GLBranch  
 INNER JOIN t_GeneralLedger  
 ON t_GeneralLedger.BankID   = @BankID  
  AND t_GeneralLedger.AccountID = t_GLBranch.AccountID  
  AND t_GLBranch.OurBranchID  = @OurBranchID  
 INNER JOIN (SELECT   
     OurBranchID,  
     AccountID,  
     SUM(Amount) Amount,  
     SUM(LocalAmount) LocalAmount  
    FROM v_GLTransaction  
    WHERE OurBranchID = @OurBranchID  
     AND TrxDate  = @WorkingDate  
    GROUP BY OurBranchID, AccountID  
    ) dt_GLBalance  
 ON  t_GLBranch.OurBranchID = dt_GLBalance.OurBranchID  
  AND t_GLBranch.AccountID = dt_GLBalance.AccountID  
  
  
  
  
 -- STEP 3: Copy t_Transaction to t_AccountTrx.  
 BEGIN TRY  
 INSERT INTO t_AccountTrx  
 (  
  TrxRowID,TrxBranchID,TrxBatchID,SerialID,OurBranchID,AccountTypeID,AccountID,  
  ProductID,TrxDate,TrxTypeID,TrxCurrencyID,ChequeID,ChequeDate,ValueDate,  
  TrxAmount,LocalAmount,Amount,ExchangeRate,MeanRate,TrxDescriptionID,TrxDescription,  
  TrxPrinted,Profit,MainGLID,ContraGLID,ImageID,ReferenceNo,Remarks,ModuleID,  
  TrxCodeID,BREFTTrxID,CreatedBy,CreatedOn,SupervisedBy,SupervisedOn,TraceNo, [CHECKSUM]  
 )  
 SELECT  
  TrxRowID,TrxBranchID,TrxBatchID,SerialID,OurBranchID,AccountTypeID,AccountID,  
  ProductID,TrxDate,TrxTypeID,TrxCurrencyID,ChequeID,ChequeDate,ValueDate,  
  TrxAmount,LocalAmount,Amount,ExchangeRate,MeanRate,TrxDescriptionID,TrxDescription,  
  TrxPrinted,Profit,MainGLID,ContraGLID,ImageID,ReferenceNo,Remarks,ModuleID,  
  TrxCodeID,BREFTTrxID,CreatedBy,CreatedOn,SupervisedBy,SupervisedOn,TraceNo, [CHECKSUM]  
 FROM t_Transaction(NOLOCK)  
 WHERE OurBranchID = @OurBranchID  
  AND TrxDate  <= @WorkingDate --TO INCLUDE PREV TRXS IF THEY WERE NEVER CONSIDERED FOR ONE CASE OR ANOTHER  
  AND DeletedOn IS NULL  
 ORDER BY TrxRowID  
 END TRY  
 BEGIN CATCH  
    
 --IF @@Error > 0  
 --BEGIN     
  SET @ErrorNo = 600024  
  IF @@TRANCOUNT>0  
  BEGIN  
   ROLLBACK TRAN   
  END  
  RETURN  
 --END  
 END CATCH  
 SELECT @CntTrx = ISNULL(COUNT(TrxRowID),0)   
 FROM t_Transaction(NOLOCK)   
 WHERE OurBranchID = @OurBranchID  
  AND TrxDate = @WorkingDate      
  AND DeletedOn IS NULL  
 SELECT @CntTrxHis = ISNULL(COUNT(AccountTrxID),0)   
 FROM t_AccountTrx(NOLOCK)   
  WHERE OurBranchID = @OurBranchID  
  AND TrxDate = @WorkingDate  
     
    
     
 SET @CntTrx=ISNULL(@CntTrx,0)  
 SET @CntTrxHis=ISNULL(@CntTrxHis,0)  
   
 --IF @CntTrx>0 AND (@CntTrx <> @CntTrxHis)  
 --BEGIN  
 --select @CntTrx, @CntTrxHis    
 -- SET @ErrorNo = 600024  
 -- RETURN  
 --END  
  
 -- STEP 4: Copy t_Transaction to t_AccountTrx.  
 INSERT INTO t_AccountTrxDeleted  
 (  
  TrxRowID,TrxBranchID,TrxBatchID,SerialID,OurBranchID,AccountTypeID,AccountID,  
  ProductID,TrxDate,TrxCodeID,TrxTypeID,TrxCurrencyID,InstrumentType,ChequeID,  
  ChequeDate,ValueDate,TrxAmount,LocalAmount,Amount,ExchangeRate,MeanRate,  
  TrxDescriptionID,TrxDescription,TrxPrinted,Profit,MainGLID,ContraGLID,ImageID,  
  IsSystemTrx,Remarks,ModuleID,BREFTTrxID,CreatedBy,CreatedOn,SupervisedBy,SupervisedOn,  
  DeletedBy,DeletedOn,DeletedReason,DeletionSupervisedBy,DeletionSupervisedOn,TraceNo  
 )  
 SELECT   
  TrxRowID,TrxBranchID,TrxBatchID,SerialID,OurBranchID,AccountTypeID,AccountID,  
  ProductID,TrxDate,TrxCodeID,TrxTypeID,TrxCurrencyID,InstrumentTypeID,ChequeID,  
  ChequeDate,ValueDate,TrxAmount,LocalAmount,Amount,ExchangeRate,MeanRate,  
  TrxDescriptionID,TrxDescription,TrxPrinted,Profit,MainGLID,ContraGLID,ImageID,  
  1,Remarks,ModuleID,BREFTTrxID,CreatedBy,CreatedOn,SupervisedBy,SupervisedOn,  
  DeletedBy,DeletedOn,DeletedReason,DeletionSupervisedBy,DeletionSupervisedOn,TraceNo  
 FROM t_Transaction(NOLOCK)   
 WHERE OurBranchID = @OurBranchID  
  AND TrxDate  = @WorkingDate  
  AND DeletedOn IS NOT NULL  
 ORDER BY TrxRowID  
   
 IF @@Error > 0  
 BEGIN     
  SET @ErrorNo = 600025  
  IF @@TRANCOUNT>0  
  BEGIN  
   ROLLBACK TRAN   
  END  
  RETURN  
 END  
   
 SELECT @CntTrx = ISNULL(COUNT(TrxRowID),0) FROM t_Transaction(NOLOCK)    
  WHERE OurBranchID = @OurBranchID  
  AND TrxDate  = @WorkingDate  
  AND DeletedOn IS NOT NULL  
 SELECT @CntTrxHis = ISNULL(COUNT(TrxRowID),0) FROM t_AccountTrxDeleted(NOLOCK)    
  WHERE OurBranchID = @OurBranchID  
  AND TrxDate = @WorkingDate   
  
 SET @CntTrx=ISNULL(@CntTrx,0)  
 SET @CntTrxHis=ISNULL(@CntTrxHis,0)  
          
 IF @CntTrx>0 AND (@CntTrx <> @CntTrxHis)  
 BEGIN     
  SET @ErrorNo = 600025  
  IF @@TRANCOUNT>0  
  BEGIN  
   ROLLBACK TRAN   
  END  
  RETURN  
 END  
  
 --Inserting into history table, this table is to aovid datalose  
 INSERT INTO t_TrxHistory  
 (  
  TrxRowID,TrxBatchID,TrxBatchSLNo,TrxBranchID,SerialID,OurBranchID,AccountTypeID,  
  AccountID,ProductID,ModuleID,TrxCodeID,TrxTypeID,TrxDate,ValueDate,Amount,  
  LocalAmount,TrxCurrencyID,TrxAmount,ExchangeRate,MeanRate,Profit,InstrumentTypeID,  
  ChequeID,ChequeDate,ReferenceNo,Remarks,TrxDescriptionID,TrxDescription,MainGLID,  
  ContraGLID,TrxFlagID,ImageID,TrxPrinted,UnsupervisedAmount,ForwardToUser,ForwardToGroup,BREFTTrxID,  
  CreatedBy,CreatedOn,SupervisedBy,SupervisedOn,SupervisedBy2,SupervisedOn2,DeletedBy,  
  DeletedOn,DeletedReason,DeletionSupervisedBy,DeletionSupervisedOn,TraceNo, [Checksum]  
 )  
 SELECT     
  TrxRowID,TrxBatchID,TrxBatchSLNo,TrxBranchID,SerialID,OurBranchID,AccountTypeID,  
  AccountID,ProductID,ModuleID,TrxCodeID,TrxTypeID,TrxDate,ValueDate,Amount,  
  LocalAmount,TrxCurrencyID,TrxAmount,ExchangeRate,MeanRate,Profit,InstrumentTypeID,  
  ChequeID,ChequeDate,ReferenceNo,Remarks,TrxDescriptionID,TrxDescription,MainGLID,  
  ContraGLID,TrxFlagID,ImageID,TrxPrinted,UnsupervisedAmount,ForwardToUser,ForwardToGroup,BREFTTrxID,  
  CreatedBy,CreatedOn,SupervisedBy,SupervisedOn,SupervisedBy2,SupervisedOn2,DeletedBy,  
  DeletedOn,DeletedReason,DeletionSupervisedBy,DeletionSupervisedOn,TraceNo, [Checksum]  
 FROM t_Transaction(NOLOCK)   
 WHERE OurBranchID = @OurBranchID  
  AND TrxDate  <= @WorkingDate --TO INCLUDE PREV TRXS IF THEY WERE NEVER CONSIDERED FOR ONE CASE OR ANOTHER  
 ORDER BY TrxRowID  
   
 IF @@Error > 0  
 BEGIN   
      
  SET @ErrorNo = 600026  
  IF @@TRANCOUNT>0  
  BEGIN  
   ROLLBACK TRAN   
  END  
  RETURN  
 END  
 SELECT @CntTrx = ISNULL(COUNT(TrxRowID),0) FROM t_Transaction(NOLOCK)   
  WHERE OurBranchID = @OurBranchID  
  AND TrxDate  = @WorkingDate  
 SELECT @CntTrxHis = ISNULL(COUNT(TrxRowID),0) FROM t_TrxHistory(NOLOCK)   
  WHERE OurBranchID = @OurBranchID  
  AND TrxDate = @WorkingDate   
    
     
 SET @CntTrx=ISNULL(@CntTrx,0)  
 SET @CntTrxHis=ISNULL(@CntTrxHis,0)  
  
         
 IF @CntTrx>0 AND (@CntTrx <> @CntTrxHis)  
 BEGIN   
      
  SET @ErrorNo = 600026  
  IF @@TRANCOUNT>0  
  BEGIN  
   ROLLBACK TRAN   
  END  
  RETURN  
 END  
  
 DELETE FROM t_Transaction  
 WHERE OurBranchID = @OurBranchID  
  AND TrxDate  <= @WorkingDate --TO INCLUDE PREV TRXS IF THEY WERE NEVER CONSIDERED FOR ONE CASE OR ANOTHER  
    
    
 IF EXISTS (SELECT   
  ISNULL(TrxRowID,0),TrxBranchID,TrxBatchID,TrxBatchSLNo,OurBranchID,AccountTypeID,AccountID,  
  ChequeDigit,VoucherCode,ReturnCodeID,Commission,TheirCommission,VATPINNo,VATPAYType,  
  VATPAYEMonth,VATSerialNo,BankID,BranchID,DrawerOrPayeeAccountID,DrawerOrPayee,ChequeID,  
  ChequeDate,ValueDate,Amount,Date,CurrencyID,DRN,RefNo,OrigRefCode,ProcessingNo,  
  PolicyNumber1,PolicyNumber2,VATPAYECommission,NameOfEmployee,ESlipNumber,ORIGINATORCODE,  
  FileType,PaymentDate,VatName,VatNumber,VatType,MonthOfPayment,PaymentType,PinNumber,  
  PaymentTypeID,TrxType,ImageID,IsDeleted,ColumnID,IsUnpaidItem  
 FROM t_TrxClearing(NOLOCK)   
  WHERE OurBranchID = @OurBranchID   
  AND Date = @WorkingDate)  
 BEGIN  
  INSERT INTO t_AccountTrxClearing  
  (  
   TrxRowID,TrxBranchID,TrxBatchID,TrxBatchSLNo,OurBranchID,AccountTypeID,AccountID,  
   ChequeDigit,VoucherCode,ReturnCodeID,Commission,TheirCommission,VATPINNo,VATPAYType,  
   VATPAYEMonth,VATSerialNo,BankID,BranchID,DrawerOrPayeeAccountID,DrawerOrPayee,ChequeID,  
   ChequeDate,ValueDate,Amount,Date,CurrencyID,DRN,RefNo,OrigRefCode,ProcessingNo,  
   PolicyNumber1,PolicyNumber2,VATPAYECommission,NameOfEmployee,ESlipNumber,ORIGINATORCODE,  
   FileType,PaymentDate,VatName,VatNumber,VatType,MonthOfPayment,PaymentType,PinNumber,  
   PaymentTypeID,TrxType,ImageID,IsDeleted,ColumnID,IsUnpaidItem,reference  
  )  
  SELECT  
   ISNULL(TrxRowID,0),TrxBranchID,TrxBatchID,TrxBatchSLNo,OurBranchID,AccountTypeID,AccountID,  
   ChequeDigit,VoucherCode,ReturnCodeID,Commission,TheirCommission,VATPINNo,VATPAYType,  
   VATPAYEMonth,VATSerialNo,BankID,BranchID,DrawerOrPayeeAccountID,DrawerOrPayee,ChequeID,  
   ChequeDate,ValueDate,Amount,Date,CurrencyID,DRN,RefNo,OrigRefCode,ProcessingNo,  
   PolicyNumber1,PolicyNumber2,VATPAYECommission,NameOfEmployee,ESlipNumber,ORIGINATORCODE,  
   FileType,PaymentDate,VatName,VatNumber,VatType,MonthOfPayment,PaymentType,PinNumber,  
   PaymentTypeID,TrxType,ImageID,IsDeleted,ColumnID,IsUnpaidItem,reference  
  FROM t_TrxClearing (NOLOCK)  
   WHERE OurBranchID = @OurBranchID   
   AND Date <= @WorkingDate  
  ORDER BY TrxRowID  
 END  
   
 --IF @@Error > 0  
 --BEGIN     
 -- SET @ErrorNo = 600027  
 -- RETURN  
 --END  
 SELECT @CntTrx = ISNULL(COUNT(isnull(TrxRowID,0)),0) FROM t_TrxClearing (NOLOCK)  
  WHERE OurBranchID = @OurBranchID  
  AND Date = @WorkingDate  
 SELECT @CntTrxHis = ISNULL(COUNT(AccountTrxRowID),0) FROM t_AccountTrxClearing (NOLOCK)  
  WHERE OurBranchID = @OurBranchID  
  AND Date = @WorkingDate   
    
 SET @CntTrx=ISNULL(@CntTrx,0)  
 SET @CntTrxHis=ISNULL(@CntTrxHis,0)  
    
 --IF @CntTrx>0 AND (@CntTrx <> @CntTrxHis)  
 --BEGIN    
 -- SET @ErrorNo = 600027  
 -- IF @@TRANCOUNT>0  
 -- BEGIN  
 --  ROLLBACK TRAN   
 -- END  
 -- RETURN  
 --END  
   
 DELETE FROM t_TrxClearing  
 WHERE OurBranchID = @OurBranchID  
  AND Date <= @WorkingDate  
    
 INSERT INTO t_TrxInwardsHistory  
 (  
  ColumnID,OurBankID,Date,FileName,Data,OurBranchID,AccountType,AccountID,TrxType,  
  ChequeID,Amount,BankID,BranchID,TheirAccount,Description,TrxDescriptionID,  
  ExtraDetails,ChequeDigit,VoucherCode,ReturnCode,DrawerOrPayee,OurCommission,  
  TheirCommission,TheirColumnID,DebitAmount,UnpaidAmount,CreditAmount,DebitRecords,  
  UnpaidRecords,CreditRecords,DRN,MicrLine,OriginatorCode,OriginatorRef,Policy1,  
  Policy2,Post,Status,FullName,Validity,ProductID,ModuleID,CurrencyID,InstrumentTypeID,  
  MainGLID,ContraGLID,ImageID,PostedBy,TrxBranchID,VoucherCodeDescription,IsReconsiable,  
  LocalCurrency,Reason,reference,TrxID  
 )  
 SELECT  
  ColumnID,OurBankID,Date,FileName,Data,OurBranchID,AccountType,AccountID,TrxType,  
  ChequeID,Amount,BankID,BranchID,TheirAccount,Description,TrxDescriptionID,  
  ExtraDetails,ChequeDigit,VoucherCode,ReturnCode,DrawerOrPayee,OurCommission,  
  TheirCommission,TheirColumnID,DebitAmount,UnpaidAmount,CreditAmount,DebitRecords,  
  UnpaidRecords,CreditRecords,DRN,MicrLine,OriginatorCode,OriginatorRef,Policy1,  
  Policy2,Post,Status,FullName,Validity,ProductID,ModuleID,CurrencyID,InstrumentTypeID,  
  MainGLID,ContraGLID,ImageID,PostedBy,TrxBranchID,VoucherCodeDescription,IsReconsiable,  
  LocalCurrency,Reason,reference,TrxID  
 FROM t_TrxInwards   
  WHERE OurBranchID = @OurBranchID   
  AND Date <= @WorkingDate  
 ORDER BY ColumnID  
   
 IF @@Error > 0  
 BEGIN   
      
  SET @ErrorNo = 600028  
  IF @@TRANCOUNT>0  
  BEGIN  
   ROLLBACK TRAN   
  END  
  RETURN  
 END  
   
 SELECT @CntTrx = ISNULL(COUNT(ColumnID),0) FROM t_TrxInwards(NOLOCK)   
  WHERE OurBranchID = @OurBranchID   
  AND Date = @WorkingDate  
 SELECT @CntTrxHis = ISNULL(COUNT(TrxRowID),0) FROM t_TrxInwardsHistory (NOLOCK)  
  WHERE OurBranchID = @OurBranchID   
  AND Date = @WorkingDate    
    
 SET @CntTrx=ISNULL(@CntTrx,0)  
 SET @CntTrxHis=ISNULL(@CntTrxHis,0)  
        
 IF @CntTrx>0 AND (@CntTrx <> @CntTrxHis)  
 BEGIN     
  SET @ErrorNo = 600028  
  IF @@TRANCOUNT>0  
  BEGIN  
   ROLLBACK TRAN   
  END  
  RETURN  
 END  
   
 DELETE FROM t_TrxInwards  
 WHERE OurBranchID = @OurBranchID  
  AND Date <= @WorkingDate  
   
 END TRY  
 BEGIN CATCH  
  
  SET @ErrorNo = 600026   
  IF @@TRANCOUNT>0  
  BEGIN  
   ROLLBACK TRAN   
  END    
  RETURN  
 END CATCH  
   
 IF ISNULL(@ErrorNo,'')>1  
 BEGIN  
  IF @@TRANCOUNT>0  
  BEGIN  
   ROLLBACK TRAN  
   RETURN  
  END  
 END  
  
 COMMIT TRAN  
  
 SET NOCOUNT OFF  
END  