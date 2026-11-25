CREATE PROCEDURE dbo.sb_ClearEffects  
(  
 @OurBranchID BranchID,  
 @ProcessDate SMALLDATETIME,  
 @OperatorID  OperatorID,  
 @ErrorNo  INT = 0 OUTPUT  
)  
    
  
  
AS  
BEGIN  
 SET NOCOUNT ON  
  
 EXEC ps_ExecuteSubProcess @OurBranchID = @OurBranchID,  
       @ProcessDate = @ProcessDate,  
       @OperatorID  = @OperatorID,  
       @ProcessID  = 'CLR',  
       @ErrorNo  = @ErrorNo OUTPUT  
  
 IF isnull(@ErrorNo,0) = 1  
 BEGIN  
  SET @ErrorNo = 0  
  
    
 END  
  
 IF @ErrorNo > 0  
  GOTO EndStatement   
    
 SET @ErrorNo = 0  
   
EndStatement:  
  
 IF @@Error > 0 OR @ErrorNo > 0  
 BEGIN    
  SET @ErrorNo = 51023  
  RETURN  
 END  
     
 Else  
 Begin  
 ---Massai  
  UPDATE t_SystemBranchStatus SET  
   CEDate = @ProcessDate  
  WHERE OurBranchID = @OurBranchID  
  
 END   
 SET NOCOUNT OFF  
END  
  