ALTER PROCEDURE p_ACHTrxStatus
(  
 @FileType  Varchar(10),  
 @CurrencyID Varchar(5),  
 @SessionID Int,  
 @Bic   Varchar(20)  
)  
AS  
DECLARE @File VarChar(10)  
  
IF @Bic ='ALL'  
BEGIN  
 SELECT @Bic = NULL  
END  

SELECT @File =   
 CASE WHEN @FileType = 'POC' THEN 'OC'  
  WHEN @FileType = 'ROC' THEN 'OC'  
  WHEN @FileType = 'POD' THEN 'OD'  
  WHEN @FileType = 'ROD' THEN 'OD'  
  WHEN @FileType = 'PDOD' THEN 'OC'  
  WHEN @FileType = 'RDOD' THEN 'OC'  
  WHEN @FileType = 'RCT' THEN 'OD'  
  ELSE @FileType END  
  
UPDATE t_TrxClearing   
SET ReturnCodeID = '00'  
WHERE ReturnCodeID = '00 -'  
  
-- For POC and POD (Outward Credits and Debits)
IF @FileType IN ('POC', 'POD')  
BEGIN  
 SELECT 
    OurBranchID, 
    AccountID, 
    dbo.f_GetAccountName(OurBranchID, AccountID) as CName,   
    ABS(Amount) as Amount,  -- Use ABS for positive amounts
    DrawerOrPayeeAccountID, 
    DrawerOrPayee, 
    TrxRowID,
    dbo.f_GetClearingSwiftIDFromBankID(BankId) as BankID,   
    CASE WHEN IsNull(IsGenerated, 0) = 0 THEN '' ELSE IsNull(TrxStatus, 'Awaiting response') END as TrxStatus,    
    CASE WHEN IsNull(IsGenerated, 0) = 0 THEN 'File Not Created'  
    ELSE IsNull(Remarks, 'File Created') END as Remarks   
 FROM v_Clearing WITH (NOLOCK)  -- CHANGED: Using view instead of table
 WHERE TrxTypeID = @File         -- CHANGED: Using TrxTypeID instead of TrxType
    AND ReturnCodeID = '00' 
    AND IsNull(IsDeleted, 0) = 0  
    AND IsNull(IsGenerated, 0) = 0  
    AND VoucherCode <> '40'  
    AND BankID = IsNull(dbo.f_GetClearingSwiftIDFromBankID(@Bic), BankID)
END  

-- For Paid Demand Drafts (PDOD)
IF @FileType IN ('PDOD')  
BEGIN  
 SELECT 
    OurBranchID, 
    AccountID, 
    dbo.f_GetAccountName(OurBranchID, AccountID) as CName,   
    ABS(Amount) as Amount,
    DrawerOrPayeeAccountID, 
    DrawerOrPayee, 
    TrxRowID,
    dbo.f_GetClearingSwiftIDFromBankID(BankId) as BankID,   
    CASE WHEN IsNull(IsGenerated, 0) = 0 THEN '' ELSE IsNull(TrxStatus, 'Awaiting response') END as TrxStatus,    
    CASE WHEN IsNull(IsGenerated, 0) = 0 THEN 'File Not Created'  
    ELSE IsNull(Remarks, 'File Created') END as Remarks   
 FROM v_Clearing WITH (NOLOCK)  -- CHANGED: Using view
 WHERE TrxTypeID = @File         -- CHANGED: Using TrxTypeID
    AND ReturnCodeID = '00' 
    AND IsNull(IsDeleted, 0) = 0  
    AND IsNull(IsGenerated, 0) = 0  
    AND VoucherCode = '40'  
    AND BankID = IsNull(dbo.f_GetClearingSwiftIDFromBankID(@Bic), BankID)
END  
  
-- For Returned Outward Debits (ROD)
IF @FileType IN ('ROD')  
BEGIN  
 SELECT 
    OurBranchID, 
    AccountID, 
    dbo.f_GetAccountName(OurBranchID, AccountID) as CName,   
    ABS(Amount) as Amount,
    DrawerOrPayeeAccountID, 
    DrawerOrPayee, 
    TrxRowID,
    dbo.f_GetClearingSwiftIDFromBankID(BankId) as BankID,   
    CASE WHEN IsNull(IsGenerated, 0) = 0 THEN '' ELSE IsNull(TrxStatus, 'Awaiting response') END as TrxStatus,    
    CASE WHEN IsNull(IsGenerated, 0) = 0 THEN 'File Not Created'  
    ELSE IsNull(Remarks, 'File Created') END as Remarks  
 FROM v_Clearing WITH (NOLOCK)  -- CHANGED: Using view
 WHERE TrxTypeID = @File         -- CHANGED: Using TrxTypeID
    AND ReturnCodeID <> '00' 
    AND IsNull(IsDeleted, 0) = 0  
    AND IsNull(IsGenerated, 0) = 0  
    AND IsNull(ChequeID, '') <> ''  
    AND BankID = IsNull(@Bic, BankID) 
    AND TrxTypeID IN ('OC')  
    AND VoucherCode NOT IN ('40', '58', '59')  
END  

-- For Returned Demand Drafts (RDOD)
IF @FileType IN ('RDOD')  
BEGIN  
 SELECT 
    OurBranchID, 
    AccountID, 
    dbo.f_GetAccountName(OurBranchID, AccountID) as CName,   
    ABS(Amount) as Amount,
    DrawerOrPayeeAccountID, 
    DrawerOrPayee, 
    TrxRowID,
    dbo.f_GetClearingSwiftIDFromBankID(BankId) as BankID,   
    CASE WHEN IsNull(IsGenerated, 0) = 0 THEN '' ELSE IsNull(TrxStatus, 'Awaiting response') END as TrxStatus,    
    CASE WHEN IsNull(IsGenerated, 0) = 0 THEN 'File Not Created'  
    ELSE IsNull(Remarks, 'File Created') END as Remarks  
 FROM v_Clearing WITH (NOLOCK)  -- CHANGED: Using view
 WHERE TrxTypeID = @File         -- CHANGED: Using TrxTypeID
    AND ReturnCodeID <> '00' 
    AND IsNull(IsDeleted, 0) = 0  
    AND IsNull(IsGenerated, 0) = 0   
    AND VoucherCode = '40'  
    AND BankID = IsNull(@Bic, BankID) 
    AND TrxTypeID IN ('OC')  
END  
  
-- For Returned Outward Credits (ROC)
IF @FileType IN ('ROC')  
BEGIN  
 SELECT 
    OurBranchID, 
    AccountID, 
    dbo.f_GetAccountName(OurBranchID, AccountID) as CName,   
    ABS(Amount) as Amount,
    DrawerOrPayeeAccountID, 
    DrawerOrPayee, 
    TrxRowID,
    dbo.f_GetClearingSwiftIDFromBankID(BankId) as BankID,   
    CASE WHEN IsNull(IsGenerated, 0) = 0 THEN '' ELSE IsNull(TrxStatus, 'Awaiting response') END as TrxStatus,    
    CASE WHEN IsNull(IsGenerated, 0) = 0 THEN 'File Not Created'  
    ELSE IsNull(Remarks, 'File Created') END as Remarks  
 FROM v_Clearing WITH (NOLOCK)  -- CHANGED: Using view
 WHERE TrxTypeID = @File         -- CHANGED: Using TrxTypeID
    AND ReturnCodeID <> '00' 
    AND IsNull(IsDeleted, 0) = 0  
    AND IsNull(IsGenerated, 0) = 0   
    AND BankID = IsNull(@Bic, BankID) 
    AND TrxTypeID IN ('OC')  
    AND IsNull(chequeid, 0) <> 0   
    AND VoucherCode <> '40'  
END  

-- For Returned Credits/EFTs (RCT)
IF @FileType IN ('RCT')  
BEGIN  
 SELECT 
    OurBranchID, 
    AccountID, 
    dbo.f_GetAccountName(OurBranchID, AccountID) as CName,   
    ABS(Amount) as Amount,
    DrawerOrPayeeAccountID, 
    DrawerOrPayee, 
    TrxRowID,
    dbo.f_GetClearingSwiftIDFromBankID(BankId) as BankID,   
    CASE WHEN IsNull(IsGenerated, 0) = 0 THEN '' ELSE IsNull(TrxStatus, 'Awaiting response') END as TrxStatus,    
    CASE WHEN IsNull(IsGenerated, 0) = 0 THEN 'File Not Created'  
    ELSE IsNull(Remarks, 'File Created') END as Remarks  
 FROM v_Clearing WITH (NOLOCK)  -- CHANGED: Using view
 WHERE TrxTypeID = @File         -- CHANGED: Using TrxTypeID
    AND ReturnCodeID <> '00' 
    AND IsNull(IsDeleted, 0) = 0  
    AND IsNull(IsGenerated, 0) = 0   
    AND IsNull(chequeid, 0) = 0   
    AND BankID = IsNull(@Bic, BankID) 
    AND TrxTypeID IN ('OD')  
    AND VoucherCode <> '40'  
END