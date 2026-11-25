CREATE FUNCTION dbo.f_GetAccountName  
(  
 @OurBranchID BranchID,  
 @AccountID AccountID  
)  
RETURNS Names  
   
AS  
BEGIN  
 DECLARE @Name nVarChar(200)  
   
 SELECT @Name = IsNull(Name,'')  
 FROM t_AccountCustomer(NOLOCK)  
 WHERE OurBranchID = @OurBranchID  
   AND AccountID = @AccountID  
     
 If ISNULL(@Name,'')=''  
 Begin  
  SELECT @Name = IsNull(Description,'')  
  FROM t_GeneralLedger(NOLOCK)  
  WHERE AccountID = @AccountID  
 End  
     
RETURN @Name  
END  
  
  