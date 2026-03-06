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
    ISNULL(Orig.OriginatorAccount, AccountID) as AccountID, 
    ISNULL(Orig.OriginatorName, dbo.f_GetAccountName(OurBranchID, AccountID)) as CName,   
    ABS(Amount) as Amount,  
    DrawerOrPayeeAccountID, 
    DrawerOrPayee, 
    TrxRowID,
    dbo.f_GetClearingSwiftIDFromBankID(BankId) as BankID,   
    CASE WHEN IsNull(IsGenerated, 0) = 0 THEN '' ELSE IsNull(TrxStatus, 'Awaiting response') END as TrxStatus,    
    CASE WHEN IsNull(IsGenerated, 0) = 0 THEN 'File Not Created'  
    ELSE IsNull(Remarks, 'File Created') END as Remarks   
 FROM t_TrxClearing WITH (NOLOCK)
 OUTER APPLY dbo.f_GetOriginatorDetails(TrxBatchID, [Date]) as Orig
 WHERE TrxType = @File
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
    ISNULL(Orig.OriginatorAccount, AccountID) as AccountID, 
    ISNULL(Orig.OriginatorName, dbo.f_GetAccountName(OurBranchID, AccountID)) as CName,   
    ABS(Amount) as Amount,
    DrawerOrPayeeAccountID, 
    DrawerOrPayee, 
    TrxRowID,
    dbo.f_GetClearingSwiftIDFromBankID(BankId) as BankID,   
    CASE WHEN IsNull(IsGenerated, 0) = 0 THEN '' ELSE IsNull(TrxStatus, 'Awaiting response') END as TrxStatus,    
    CASE WHEN IsNull(IsGenerated, 0) = 0 THEN 'File Not Created'  
    ELSE IsNull(Remarks, 'File Created') END as Remarks   
 FROM t_TrxClearing WITH (NOLOCK)
 OUTER APPLY dbo.f_GetOriginatorDetails(TrxBatchID, [Date]) as Orig
 WHERE TrxType = @File 
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
    ISNULL(Orig.OriginatorAccount, AccountID) as AccountID, 
    ISNULL(Orig.OriginatorName, dbo.f_GetAccountName(OurBranchID, AccountID)) as CName,   
    ABS(Amount) as Amount,
    DrawerOrPayeeAccountID, 
    DrawerOrPayee, 
    TrxRowID,
    dbo.f_GetClearingSwiftIDFromBankID(BankId) as BankID,   
    CASE WHEN IsNull(IsGenerated, 0) = 0 THEN '' ELSE IsNull(TrxStatus, 'Awaiting response') END as TrxStatus,    
    CASE WHEN IsNull(IsGenerated, 0) = 0 THEN 'File Not Created'  
    ELSE IsNull(Remarks, 'File Created') END as Remarks  
 FROM t_TrxClearing WITH (NOLOCK)
 OUTER APPLY dbo.f_GetOriginatorDetails(TrxBatchID, [Date]) as Orig
 WHERE TrxType = @File 
    AND ReturnCodeID <> '00' 
    AND IsNull(IsDeleted, 0) = 0  
    AND IsNull(IsGenerated, 0) = 0  
    AND IsNull(ChequeID, '') <> ''  
    AND BankID = IsNull(@Bic, BankID) 
    AND TrxType IN ('OC')  
    AND VoucherCode NOT IN ('40', '58', '59')  
END  

-- For Returned Demand Drafts (RDOD)
IF @FileType IN ('RDOD')  
BEGIN  
 SELECT 
    OurBranchID, 
    ISNULL(Orig.OriginatorAccount, AccountID) as AccountID, 
    ISNULL(Orig.OriginatorName, dbo.f_GetAccountName(OurBranchID, AccountID)) as CName,   
    ABS(Amount) as Amount,
    DrawerOrPayeeAccountID, 
    DrawerOrPayee, 
    TrxRowID,
    dbo.f_GetClearingSwiftIDFromBankID(BankId) as BankID,   
    CASE WHEN IsNull(IsGenerated, 0) = 0 THEN '' ELSE IsNull(TrxStatus, 'Awaiting response') END as TrxStatus,    
    CASE WHEN IsNull(IsGenerated, 0) = 0 THEN 'File Not Created'  
    ELSE IsNull(Remarks, 'File Created') END as Remarks  
 FROM t_TrxClearing WITH (NOLOCK)
 OUTER APPLY dbo.f_GetOriginatorDetails(TrxBatchID, [Date]) as Orig
 WHERE TrxType = @File 
    AND ReturnCodeID <> '00' 
    AND IsNull(IsDeleted, 0) = 0  
    AND IsNull(IsGenerated, 0) = 0   
    AND VoucherCode = '40'  
    AND BankID = IsNull(@Bic, BankID) 
    AND TrxType IN ('OC')  
END  
  
-- For Returned Outward Credits (ROC)
IF @FileType IN ('ROC')  
BEGIN  
 SELECT 
    OurBranchID, 
    ISNULL(Orig.OriginatorAccount, AccountID) as AccountID, 
    ISNULL(Orig.OriginatorName, dbo.f_GetAccountName(OurBranchID, AccountID)) as CName,   
    ABS(Amount) as Amount,
    DrawerOrPayeeAccountID, 
    DrawerOrPayee, 
    TrxRowID,
    dbo.f_GetClearingSwiftIDFromBankID(BankId) as BankID,   
    CASE WHEN IsNull(IsGenerated, 0) = 0 THEN '' ELSE IsNull(TrxStatus, 'Awaiting response') END as TrxStatus,    
    CASE WHEN IsNull(IsGenerated, 0) = 0 THEN 'File Not Created'  
    ELSE IsNull(Remarks, 'File Created') END as Remarks  
 FROM t_TrxClearing WITH (NOLOCK)
 OUTER APPLY dbo.f_GetOriginatorDetails(TrxBatchID, [Date]) as Orig
 WHERE TrxType = @File 
    AND ReturnCodeID <> '00' 
    AND IsNull(IsDeleted, 0) = 0  
    AND IsNull(IsGenerated, 0) = 0   
    AND BankID = IsNull(@Bic, BankID) 
    AND TrxType IN ('OC')  
    AND IsNull(chequeid, 0) <> 0   
    AND VoucherCode <> '40'  
END  

-- For Returned Credits/EFTs (RCT)
IF @FileType IN ('RCT')  
BEGIN  
 SELECT 
    OurBranchID, 
    ISNULL(Orig.OriginatorAccount, AccountID) as AccountID, 
    ISNULL(Orig.OriginatorName, dbo.f_GetAccountName(OurBranchID, AccountID)) as CName,   
    ABS(Amount) as Amount,
    DrawerOrPayeeAccountID, 
    DrawerOrPayee, 
    TrxRowID,
    dbo.f_GetClearingSwiftIDFromBankID(BankId) as BankID,   
    CASE WHEN IsNull(IsGenerated, 0) = 0 THEN '' ELSE IsNull(TrxStatus, 'Awaiting response') END as TrxStatus,    
    CASE WHEN IsNull(IsGenerated, 0) = 0 THEN 'File Not Created'  
    ELSE IsNull(Remarks, 'File Created') END as Remarks  
 FROM t_TrxClearing WITH (NOLOCK)
 OUTER APPLY dbo.f_GetOriginatorDetails(TrxBatchID, [Date]) as Orig
 WHERE TrxType = @File 
    AND ReturnCodeID <> '00' 
    AND IsNull(IsDeleted, 0) = 0  
    AND IsNull(IsGenerated, 0) = 0   
    AND IsNull(chequeid, 0) = 0   
    AND BankID = IsNull(@Bic, BankID) 
    AND TrxType IN ('OD')  
    AND VoucherCode <> '40'  
END
GO