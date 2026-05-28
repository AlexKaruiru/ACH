CREATE PROCEDURE [dbo].[p_SuperviseTrx]  
(  
 @TrxBranchID  BranchID,  
 @TrxBatchID   nVarChar(8),  
 @CategoryID   Char(1), --System Code SupervisionCategoryID  
 @SupervisedBy  OperatorID,  
 @IsJointSupervision Bit = 0, --Use for Joint supervision  
 @IsUnpaidItem  Bit = 0 --Use for unpay  
)  
  
   
AS  
BEGIN  
 SET NOCOUNT ON  
--return  
 DECLARE @ModuleID varchar(50), @ReferenceNo varchar(100)  
  
 SELECT TOP 1 @ModuleID = ModuleID   
 FROM t_Transaction(NOLOCK)   
 WHERE TrxBranchID = @TrxBranchID and TrxBatchID = @TrxBatchID AND ModuleID IS NOT NULL ORDER BY TrxBatchSLNo DESC  
  
  
 IF @IsUnpaidItem = 1  
 BEGIN  
  IF EXISTS(SELECT 1 FROM t_TrxClearing  (NOLOCK)  
   WHERE TrxBranchID = @TrxBranchID AND TrxBatchID = @TrxBatchID AND (IsUnpaidItem = 0 OR IsUnpaidItem IS NULL))  
  BEGIN  
   UPDATE t_TrxClearing SET IsUnpaidItem = 1 WHERE TrxBranchID = @TrxBranchID AND TrxBatchID = @TrxBatchID  
  END  
 END  
   
  
 IF NOT EXISTS(SELECT 1 From t_Transaction(NOLOCK) WHERE TrxBatchID = @TrxBatchID AND ModuleID IN ('3040','3050') AND ModuleID = @ModuleID )--Added By Kamunya   
 BEGIN  
  IF @IsJointSupervision = 1  
  BEGIN  
   IF EXISTS (SELECT 1 FROM t_Transaction(Nolock) WHERE TrxBranchID = @TrxBranchID  
    AND TrxBatchID = @TrxBatchID AND ISNULL(SupervisedBy,'') = '')  
   BEGIN  
    --First person supervising  
    UPDATE t_Transaction SET   
     UnsupervisedAmount = TrxAmount / 2,  
     SupervisedBy  = @SupervisedBy,  
     --SupervisedOn  = dbo.f_GetWorkingDate(@TrxBranchID)  
     SupervisedOn  = GETDATE()  
    WHERE TrxBranchID  = @TrxBranchID  
     AND TrxBatchID = @TrxBatchID  
   END  
   ELSE  
   BEGIN  
    UPDATE t_Transaction SET   
     UnsupervisedAmount = 0,  
     TrxFlagID = '',  
     SupervisedBy2  = @SupervisedBy,  
     --SupervisedOn2  = dbo.f_GetWorkingDate(@TrxBranchID)  
     SupervisedOn2  = GETDATE()  
    WHERE TrxBranchID = @TrxBranchID  
     AND TrxBatchID = @TrxBatchID  
   END  
  END  
  ELSE   
  BEGIN  
   IF @CategoryID = 'D'  
   BEGIN  
    UPDATE t_Transaction SET   
     TrxFlagID = '',  
     UnsupervisedAmount= 0,      
     DeletionSupervisedBy = @SupervisedBy,  
     DeletionSupervisedOn = dbo.f_GetWorkingDate(@TrxBranchID),  
     SupervisedBy = @SupervisedBy,  
     --SupervisedOn = dbo.f_GetWorkingDate(@TrxBranchID)  
     SupervisedOn  = GETDATE()  
    WHERE TrxBranchID = @TrxBranchID  
     AND TrxBatchID = @TrxBatchID  
   END  
   ELSE  
   BEGIN  
    UPDATE t_Transaction  
     SET   
     TrxFlagID = '',  
     UnsupervisedAmount= 0,  
     SupervisedBy = @SupervisedBy,  
     --SupervisedOn = dbo.f_GetWorkingDate(@TrxBranchID)  
     SupervisedOn  = GETDATE()  
    WHERE TrxBranchID = @TrxBranchID  
     AND TrxBatchID = @TrxBatchID  
  
  
    UPDATE t_UtilityPAyment   
    SET   
     SupervisionFlag = '',  
     SupervisedBy = @SupervisedBy,  
     --SupervisedOn = dbo.f_GetWorkingDate(@TrxBranchID)  
     SupervisedOn  = GETDATE()  
    WHERE TrxBranchID = @TrxBranchID  
     AND TrxBatchID = @TrxBatchID  
     AND SupervisionFlag='U'  
   END  
  END  
 END  
 ELSE --Added By Kamunya   
 BEGIN  
  IF @IsJointSupervision = 1  
  BEGIN  
   IF EXISTS (SELECT 1 FROM t_Transaction(Nolock) WHERE OurBranchID = @TrxBranchID  
    AND TrxBatchID = @TrxBatchID AND ISNULL(SupervisedBy,'') = '')  
   BEGIN  
    --First person supervising  
    UPDATE t_Transaction SET   
     UnsupervisedAmount = TrxAmount / 2,  
     SupervisedBy  = @SupervisedBy,  
     --SupervisedOn  = dbo.f_GetWorkingDate(@TrxBranchID)  
     SupervisedOn  = GETDATE()  
    WHERE OurBranchID  = @TrxBranchID  
     AND TrxBatchID = @TrxBatchID  
   END  
   ELSE  
   BEGIN  
    UPDATE t_Transaction SET   
     UnsupervisedAmount = 0,  
     TrxFlagID = '',  
     SupervisedBy2  = @SupervisedBy,  
     --SupervisedOn2  = dbo.f_GetWorkingDate(@TrxBranchID)  
     SupervisedOn2  = GETDATE()  
    WHERE OurBranchID = @TrxBranchID  
     AND TrxBatchID = @TrxBatchID  
   END  
  END  
  ELSE   
  BEGIN  
   IF @CategoryID = 'D'  
    UPDATE t_Transaction SET   
     TrxFlagID = '',  
     UnsupervisedAmount= 0,      
     DeletionSupervisedBy = @SupervisedBy,  
     DeletionSupervisedOn = dbo.f_GetWorkingDate(@TrxBranchID),  
     SupervisedBy = @SupervisedBy,  
     --SupervisedOn = dbo.f_GetWorkingDate(@TrxBranchID)  
     SupervisedOn  = GETDATE()  
    WHERE OurBranchID = @TrxBranchID  
     AND TrxBatchID = @TrxBatchID  
   ELSE  
    UPDATE t_Transaction SET   
     TrxFlagID = '',  
     UnsupervisedAmount= 0,  
     SupervisedBy = @SupervisedBy,  
     --SupervisedOn = dbo.f_GetWorkingDate(@TrxBranchID)  
     SupervisedOn  = GETDATE()  
    WHERE OurBranchID = @TrxBranchID  
     AND TrxBatchID = @TrxBatchID  
  END  
 END  
  
 IF @ModuleID IN ('3034') AND @CategoryID <> 'D'  -- SWIFT OUTGOING MODULE --two level supervision  
 BEGIN  
  SELECT TOP 1 @ReferenceNo = ReferenceNo FROM t_Transaction (NOLOCK)   
  WHERE TrxBatchID = @TrxBatchID AND ModuleID IN ('3034') AND ReferenceNo IS NOT NULL  
     
    
  UPDATE t_SwiftOutgoingMessages  
   SET SupervisedBy = @SupervisedBy,  
    SupervisedOn = getdate(),  
    SupervisionStatus = 1,  
    Verified = 1,   
    Posted = 1,  
    Processed = 1  
   WHERE Trans_Ref = @ReferenceNo  
  --select @ReferenceNo  
 END  
 ELSE IF @ModuleID IN ('3034') AND @CategoryID = 'D'  -- SWIFT OUTGOING MODULE --two level supervision  
 BEGIN  
  SELECT TOP 1 @ReferenceNo = ReferenceNo FROM t_Transaction (NOLOCK)   
  WHERE TrxBatchID = @TrxBatchID AND ModuleID IN ('3034') AND ReferenceNo IS NOT NULL  
     
    
  UPDATE t_SwiftOutgoingMessages  
   SET SupervisedBy = @SupervisedBy,  
    SupervisedOn = getdate(),  
    SupervisionStatus = 1,  
    Verified = 1,   
    Posted = 1,  
    Rejected = 1,  
    RejectedOn = getdate(),  
    RejectedBy = @SupervisedBy  
   WHERE Trans_Ref = @ReferenceNo  
  --select @ReferenceNo  
 END  
      
 SET NOCOUNT OFF  
END  
  