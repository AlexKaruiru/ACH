CREATE PROCEDURE dbo.p_RejectTrx  
(  
 @TrxBranchID BranchID = NULL, --We have to remove ALL default NULL  
 @TrxBatchID  NVARCHAR(8),  
 @SupervisedBy OperatorID = NULL,  
 @RejectReason Remarks = NULL  
)  
  
   
AS  
BEGIN  
 SET NOCOUNT ON  
   
 Declare @Moduleid Varchar(10),  
   @ChequeID Int,  
   @TrxRowID BigInt  
   
 DECLARE @OurBranchID BranchID,  
   @AccountID  AccountID,  
   @BankID   BankID,   
   @branchid  BranchID,   
   @vouchercode VarChar(2),   
   @ReturnCode  VarChar(2),   
   @CurrencyID  CurrencyID,  
   @Date   SmallDateTime,  
   @ReferenceNo varchar(100)  
  
 Select @TrxRowID = TrxRowID,@ModuleID = Moduleid,@ChequeID = ChequeID,@OurBranchID=OurBranchID  
 From t_Transaction(NOLOCK)  
 WHERE TrxBranchID = @TrxBranchID  
 AND TrxBatchID = @TrxBatchID  
 And (IsNull(ChequeID,0) <> 0 OR ModuleID='3034')  
  
   
 If @ModuleID = '3100'  
 BEGIN   
  Delete From t_ReconcilableItem  
  WHERE OurBranchID = @TrxBranchID  
  AND TrxBatchID = @TrxBatchID  
  And ChequeID = @ChequeID  
    
  Delete From t_ChequeTrx  
  WHERE OurBranchID = @TrxBranchID  
  And ChequeID = @ChequeID  
  And TrxRowID = @TrxRowID    
    
 END  
   
   
 If @ModuleID = '3101'  
 BEGIN  
   
  Delete From t_ReconcilableItemRealize  
  WHERE OurBranchID = @TrxBranchID  
  AND TrxBatchID = @TrxBatchID  
  And ChequeID = @ChequeID  
    
  Delete From t_ChequeTrx  
  WHERE OurBranchID = @TrxBranchID  
  And ChequeID = @ChequeID  
  And ChequeStatusID = 'P'    
  And TrxRowID = @TrxRowID    
      
 END   
 If @ModuleID = '3105'  
 BEGIN  
   
  Delete From t_ReconcilableItemCancel  
  WHERE OurBranchID = @TrxBranchID  
  AND TrxBatchID = @TrxBatchID  
  And ChequeID = @ChequeID  
    
  Delete From t_ChequeTrx  
  WHERE OurBranchID = @TrxBranchID  
  And ChequeID = @ChequeID  
  And ChequeStatusID = 'P'    
  And TrxRowID = @TrxRowID    
      
 END   
   
 IF @ModuleID IN('3060','3061','3070','3050')  
 BEGIN   
 IF EXISTS(SELECT 1 FROM t_TrxClearing   
  WHERE TrxBranchID = @TrxBranchID  
  AND TrxBatchID = @TrxBatchID)  
        BEGIN  
         UPDATE t_TrxClearing  
          SET IsDeleted = 1  
          WHERE TrxBranchID = @TrxBranchID  
          AND TrxBatchID = @TrxBatchID  
           
         SELECT @BankID  =BankID,  
          @BranchID =BranchID,  
          @ChequeID =ChequeID,  
          @VoucherCode=VoucherCode,  
          @ReturnCode =ReturnCodeID,  
          @CurrencyID =CurrencyID,  
          @Date=Date  
         FROM t_TrxClearing   
         WHERE TrxBranchID = @TrxBranchID  
          AND TrxBatchID = @TrxBatchID  
            
            
         UPDATE t_TrxInwards   
         SET POST = 0  
         WHERE BankID  =@BankID   
         AND   BranchID  =@BranchID   
         AND   ChequeID  =@ChequeID  
         AND   VoucherCode =@VoucherCode  
         AND   ReturnCode =@ReturnCode   
         AND   CurrencyID =@CurrencyID   
         AND   Date   =@Date   
         AND   TrxType  IN ('IC','ID')  
         AND   POST   = 1   
        END      
 END   
 IF @ModuleID IN('3060','3061')  
 BEGIN   
  Delete From t_TrxValueDated  
  WHERE OurBranchID = @TrxBranchID  
  AND TrxRowID = @TrxRowID  
  AND ChequeID = @ChequeID  
  AND IsCleared = 0  
  AND ClearingStatusID = 'S'  
 END   
  
 IF @ModuleID = '3034' -- SWIFT OUTGOING MODULE --two level supervision  
 BEGIN  
  select @ReferenceNo = ReferenceNo from t_Transaction (NOLOCK) where TrxBranchID = @TrxBranchID and TrxBatchID = @TrxBatchID and ModuleID = '3034' and ReferenceNo IS NOT NULL  
     
  UPDATE t_SwiftOutgoingMessages SET   
   Verified =0,   
   Posted=0,  
   Rejected=1,  
   SupervisionStatus = 1,  
   RejectedBy= @SupervisedBy,  
   RejectedOn= dbo.f_GetWorkingDate(@TrxBranchID),  
   RejectedRemarks= @RejectReason  
  WHERE Trans_Ref = @ReferenceNo  
  
  UPDATE t_Transaction SET  
   TrxFlagID    = '', -- All Deleted Transactions must go for supervision  
   IsTrxPending   = 0,  
   SupervisedBy   = @SupervisedBy,  
   SupervisedOn   = dbo.f_GetWorkingDate(@TrxBranchID),  
   DeletedBy    = @SupervisedBy,  
   DeletedOn    = dbo.f_GetWorkingDate(@TrxBranchID),  
   DeletedReason   = @RejectReason  
  WHERE ModuleID = '3034'  
   AND TrxBatchID = @TrxBatchID  
   AND TrxCodeID in (10, 12)  
   and ReferenceNo = @ReferenceNo  
   --select @ReferenceNo  
 END  
  
 UPDATE t_Transaction SET  
  TrxFlagID    = '', -- All Deleted Transactions must go for supervision  
  IsTrxPending   = 0,  
  SupervisedBy   = @SupervisedBy,  
  SupervisedOn   = dbo.f_GetWorkingDate(@TrxBranchID),  
  DeletedBy    = @SupervisedBy,  
  DeletedOn    = dbo.f_GetWorkingDate(@TrxBranchID),  
  DeletedReason   = @RejectReason  
 WHERE TrxBranchID = @TrxBranchID  
  AND TrxBatchID = @TrxBatchID  
  
 SET NOCOUNT OFF  
END  
  
  
  
  
  