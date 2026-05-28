CREATE PROCEDURE dbo.sp_GetIfChequeIsPosted    
  
 (    
  @BankID varchar(4),     
  @BranchID varchar(4),     
  @TheirAccountID varchar(15),     
  @ChequeID Varchar(15)    
 )      
AS    
 SET NOCOUNT ON    
 IF EXISTS(SELECT     
   OurbranchID, AccountID, BankID, BranchID, DrawerOrPayeeAccountID, ChequeID, Amount     
    FROM t_TrxClearing WHERE     
   BankID = RIGHT(@BankID,2) AND     
   BranchID = RIGHT(@BranchID,2) AND   
   ISNULL(IsDeleted,0) = 0 AND    
   DrawerOrPayeeAccountID = @TheirAccountID AND     
   ChequeID = @ChequeID AND TrxType IN ('OC'))     
 BEGIN    
 SELECT 'True'    
 END    
ELSE    
 BEGIN    
 SELECT 'False'    
 END   
  