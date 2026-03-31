CREATE PROCEDURE dbo.p_GetUnsuperviseModuleList  
(  
 @OurBranchID BranchID,  
 @OperatorID  OperatorID  
)  
   
AS  
BEGIN  
 SET NOCOUNT ON  
 SET ANSI_WARNINGS OFF  
  
 SELECT DISTINCT ModuleID,Case ModuleID When '3010' Then 'Cash - Interbranch' Else dbo.f_GetModuleName(ModuleID,'en')End ModuleName  
 From t_transaction(nolock) where isnull(TrxFlagID,'')<>''   
 AND TrxBranchID=@OurBranchID    
 AND Deletedby IS NULL     
    
 SET NOCOUNT OFF  
 SET ANSI_WARNINGS ON  
END  
  
--go  
  
--exec  [p_GetUnsuperviseModuleList] '001','DENIS'