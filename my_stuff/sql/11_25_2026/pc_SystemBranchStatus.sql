/*  
 01-Jan-2012  
 BRMFS Author  
 Retrieve Branch Status details  
*/  
  
--pc_SystemBranchStatus  
CREATE PROCEDURE dbo.pc_SystemBranchStatus  
(  
 @LanguageID Varchar(3) = 'en'  
)  
   
AS  
BEGIN  
 SET NOCOUNT ON  
  
 SELECT OurBranchID,  
  IsTrxAllow,  
  CASE   
   WHEN BranchProcessStatusID <> 'N' THEN 'P'  
   WHEN IsDayOpen = 0 THEN 'C'  
   WHEN IsDayOpen = 1  THEN 'O'  
  END BranchStatusID,  
  dbo.fn_GetSystemCodeDesc('BranchStatusID',  
    CASE  
     WHEN BranchProcessStatusID <> 'N' THEN 'P'   
     WHEN IsDayOpen = 0 THEN 'C'  
     WHEN IsDayOpen = 1 THEN 'O'  
    END, @LanguageID) BranchStatus,  
  SODDate,  
  EODDate,  
  CEDate,  
  EOMDate,  
  EOYDate,  
  StartYear,  
  EndYear  
 FROM t_SystemBranchStatus(NOLOCK)  
 WHERE BranchStatus = 1  
  
 SET NOCOUNT OFF  
END  
  