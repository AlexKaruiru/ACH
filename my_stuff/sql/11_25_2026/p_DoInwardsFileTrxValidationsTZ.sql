CREATE PROCEDURE dbo.p_DoInwardsFileTrxValidationsTZ    
--WITH ENCRYPTION    
AS      
SET NOCOUNT ON      
SET DEADLOCK_PRIORITY LOW      
--RETURN  
DECLARE   
 @BankersChequeAccount   AccountID,  
 @LegacyBankersChequeAccount   AccountID,      
 @OurBankID      BankID,       
 @HOBranchID      BranchID,      
 @HOBranchLocalBranchCurrency CurrencyID ,  
 @OurBranchID BranchID,  
 @AccountID  AccountID,  
 @ProductID  ProductID,  
 @ColumnID  Bigint,  
 @ChequeID  Int,  
 @AccountTypeID Char(2),  
 @TagClearingAcc AccountID,  
    @TagSuspAcc_EFT AccountID,  
 @TagSuspAcc_CHQ AccountID,  
    @TagControlAcc AccountID,  
 @BankersKESChequeAccount     AccountID  
  ,  @BankersUSDChequeAccount     AccountID  
  ,  @BankersEURChequeAccount     AccountID  
  ,  @BankersGBPChequeAccount     AccountID     
  
  
--TRUNCATE TABLE BRNET_IMAGEServer.dbo.t_TempInChequeImages   
--TRUNCATE TABLE dbo.t_TempClrData   
  
  
SELECT @OurBankID  = (SELECT BankID FROM t_SystemBankSetting  (NOLOCK) )      
SELECT @HOBranchID  = dbo.f_GetHOBranchID(@OurBankID)      
SELECT @HOBranchLocalBranchCurrency  = (SELECT CurrencyID FROM t_SystemBranchSetting (NOLOCK) WHERE OurBranchID = @HOBranchID)  
DELETE FROM t_SystemRecordLocks FROM t_IncomingTransactions (NOLOCK) WHERE PKKey  LIKE '%' + AccountID + '%' AND Reason='Record Locked By Another User'      
DELETE FROM t_systemrecordlocks  WHERE LockModuleID='1300' AND ModuleID In('3050','3040')    
  
UPDATE t_IncomingTransactions SET lrAccAsImported = AccountID  
UPDATE t_IncomingTransactions SET ClrBrnAsImported = OurBranchID  
-------------------------------------------------------------------------------------------------------------------------------------------------  
  
             
SELECT  @BankersChequeAccount  = dbo.f_GetCurrencyBranchGLAccountID(@HOBranchID,@HOBranchLocalBranchCurrency, 'CL_BANKERCQL_AC')  
   
  
SELECT @BankersKESChequeAccount = dbo.f_GetCurrencyBranchGLAccountID(@HOBranchID,'TZS', 'CL_BANKERCQL_AC')  
SELECT @BankersUSDChequeAccount = dbo.f_GetCurrencyBranchGLAccountID(@HOBranchID,'USD', 'CL_BANKERCQL_AC')  
SELECT @BankersGBPChequeAccount = dbo.f_GetCurrencyBranchGLAccountID(@HOBranchID,'GBP', 'CL_BANKERCQL_AC')  
SELECT @BankersEURChequeAccount = dbo.f_GetCurrencyBranchGLAccountID(@HOBranchID,'EUR', 'CL_BANKERCQL_AC')  
SELECT @TagClearingAcc = 'CUR_CLR_AC'  
SELECT @TagSuspAcc_CHQ = 'ACP_CLR_SUSP_AC_CHQ'  
SELECT @TagSuspAcc_EFT = 'ACP_CLR_SUSP_AC_EFT'  
SELECT @TagControlAcc = 'CONTROL_AC'  
  
  
--Banker's Cheques---  
UPDATE t_IncomingTransactions   
 SET [Status] = 1,  
  FullName = dbo.f_GetGLAccountName(dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagSuspAcc_CHQ)),   
  AccountType = 'G',  
  ProductID   = 'GL',  
  MainGLID = dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagControlAcc),  
  AccountID   = dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagSuspAcc_CHQ),  
  [Description]  = 'Banker''s Cheque #: ' + CAST( ChequeID as varchar) + ' Date Not Reconcilable'  
FROM t_IncomingTransactions (NOLOCK)   
WHERE dbo.f_GetBankersChequeIDStatus(OurBranchID,'G',AccountID,ChequeID,amount) = 'D' AND ISNULL(IsProcessed,0)=0   
  AND t_IncomingTransactions.AccountID  = dbo.f_GetCurrencyBranchGLAccountID(OurBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), 'CL_BANKERCQL_AC')  
  AND ISNULL(FullName,'') <> dbo.f_GetGLAccountName(dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagSuspAcc_CHQ))   
  
UPDATE t_IncomingTransactions   
 SET [Status] = 1,  
  FullName = dbo.f_GetGLAccountName(dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagSuspAcc_CHQ)),   
  AccountType = 'G',  
  ProductID   = 'GL',  
  MainGLID = dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagControlAcc),  
  AccountID   = dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagSuspAcc_CHQ),  
  [Description]  = 'Banker''s Cheque #: ' + CAST( ChequeID as varchar) + ' Amount Not Reconcilable'  
FROM t_IncomingTransactions (NOLOCK)   
WHERE dbo.f_GetBankersChequeIDStatus(OurBranchID,'G',AccountID,ChequeID,amount) = 'A' AND ISNULL(IsProcessed,0)=0   
  AND t_IncomingTransactions.AccountID  = dbo.f_GetCurrencyBranchGLAccountID(OurBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), 'CL_BANKERCQL_AC')  
  AND ISNULL(FullName,'') <> dbo.f_GetGLAccountName(dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagSuspAcc_CHQ))   
  
UPDATE t_IncomingTransactions   
 SET [Status] = 1,  
  FullName = dbo.f_GetGLAccountName(dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagSuspAcc_CHQ)),   
  AccountType = 'G',  
  ProductID   = 'GL',  
  MainGLID = dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagControlAcc),  
  AccountID   = dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagSuspAcc_CHQ),  
  [Description]  = 'Invalid Banker''s Cheque Series #: ' + CAST( ChequeID as varchar)  
FROM t_IncomingTransactions (NOLOCK)   
WHERE dbo.f_GetBankersChequeIDStatus(OurBranchID,'G',AccountID,ChequeID,amount) = 'I' AND ISNULL(IsProcessed,0)=0   
  AND t_IncomingTransactions.AccountID  = dbo.f_GetCurrencyBranchGLAccountID(OurBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), 'CL_BANKERCQL_AC')  
  AND ISNULL(FullName,'') <> dbo.f_GetGLAccountName(dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagSuspAcc_CHQ))   
  
UPDATE t_IncomingTransactions   
 SET [Status] = 1,  
  FullName = dbo.f_GetGLAccountName(dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagSuspAcc_CHQ)),   
  AccountType = 'G',  
  ProductID   = 'GL',  
  MainGLID = dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagControlAcc),  
  AccountID   = dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagSuspAcc_CHQ),  
  [Description]  = 'Banker''s Cheque #: ' + CAST( ChequeID as varchar) + ' Previously Paid once'  
FROM t_IncomingTransactions (NOLOCK)   
WHERE dbo.f_GetBankersChequeIDStatus(OurBranchID,'G',AccountID,ChequeID,amount) = 'P' AND ISNULL(IsProcessed,0)=0   
  AND t_IncomingTransactions.AccountID  = dbo.f_GetCurrencyBranchGLAccountID(OurBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), 'CL_BANKERCQL_AC')  
  AND ISNULL(FullName,'') <> dbo.f_GetGLAccountName(dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagSuspAcc_CHQ))   
  
UPDATE t_IncomingTransactions   
 SET [Status] = 1,  
  FullName = dbo.f_GetGLAccountName(dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagSuspAcc_CHQ)),   
  AccountType = 'G',  
  ProductID   = 'GL',  
  MainGLID = dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagControlAcc),  
  AccountID   = dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagSuspAcc_CHQ),  
  [Description]  = 'Banker''s Cheque #: ' + CAST( ChequeID as varchar) + ' Payment Stopped'  
FROM t_IncomingTransactions (NOLOCK)   
WHERE dbo.f_GetBankersChequeIDStatus(OurBranchID,'G',AccountID,ChequeID,amount) = 'S' AND ISNULL(IsProcessed,0)=0   
  AND t_IncomingTransactions.AccountID  = dbo.f_GetCurrencyBranchGLAccountID(OurBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), 'CL_BANKERCQL_AC')  
  AND ISNULL(FullName,'') <> dbo.f_GetGLAccountName(dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagSuspAcc_CHQ))   
--------------------------------------------------------------------------------------------------------------------------------------------------------  
  
----Account Status  Dormant, Closed, Blocked --------------------------------------------------------   
UPDATE  t_IncomingTransactions  
 SET FullName  = dbo.f_GetGLAccountName(dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagSuspAcc_CHQ)),   
  AccountType  = 'G',  
  ProductID  = 'GL',  
  MainGLID  = dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagControlAcc),  
  AccountID  = dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagSuspAcc_CHQ),  
  [Description] = CASE AccountStatusID   
     WHEN 'AB' THEN  'Account # ' + t_IncomingTransactions.AccountID + ' Blocked'  
     WHEN 'AC' THEN  'Account # ' + t_IncomingTransactions.AccountID + ' Closed'  
     WHEN 'AD' THEN  'Account # ' + t_IncomingTransactions.AccountID + ' Dormant' END,  
  [Status]  = 1  
FROM t_AccountCustomer (NOLOCK) ,t_IncomingTransactions (NOLOCK)  
WHERE t_AccountCustomer.OurBranchID  = t_IncomingTransactions.OurBranchID  
  AND t_AccountCustomer.AccountID = t_IncomingTransactions.AccountID  
  AND AccountStatusID In ('AB','AC','AD')  
  AND isNull(t_IncomingTransactions.AccountType,'') <> 'G'  
  AND TrxType IN ('ID') AND ISNULL(IsProcessed,0)=0    
  
  
UPDATE  t_IncomingTransactions  
 SET FullName  = dbo.f_GetGLAccountName(dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagSuspAcc_EFT)),   
  AccountType  = 'G',  
  ProductID  = 'GL',  
  MainGLID  = dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagControlAcc),  
  AccountID  = dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagSuspAcc_EFT),  
  [Description] = CASE AccountStatusID   
     WHEN 'AB' THEN  'Account # ' + t_IncomingTransactions.AccountID + ' Blocked'  
     WHEN 'AC' THEN  'Account # ' + t_IncomingTransactions.AccountID + ' Closed'  
     WHEN 'AD' THEN  'Account # ' + t_IncomingTransactions.AccountID + ' Dormant' END,  
  [Status]  = 1  
FROM t_AccountCustomer (NOLOCK) ,t_IncomingTransactions (NOLOCK)  
WHERE t_AccountCustomer.OurBranchID  = t_IncomingTransactions.OurBranchID  
  AND t_AccountCustomer.AccountID = t_IncomingTransactions.AccountID  
  AND AccountStatusID In ('AB','AC','AD')  
  AND isNull(t_IncomingTransactions.AccountType,'') <> 'G'  
  AND TrxType IN ('IC') AND ISNULL(IsProcessed,0)=0    
------------------------------------------------------------------------------------------------------------------------------------------   
----Update Cheque Paid Once       
UPDATE t_IncomingTransactions   
 SET [Status] = 1,  
  ProductID = 'GL',  
  AccountType = 'G',  
  Policy2 = 'P',  
  AccountID  = dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,CurrencyID, @TagSuspAcc_CHQ),         
  FullName  = dbo.f_GetGLAccountName(dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,CurrencyID, @TagSuspAcc_CHQ)),       
  MainGLID  = dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,CurrencyID, @TagSuspAcc_CHQ),        
  Description = 'Account # '+ ACCOUNTID + ' has Cheque already Paid. Cheque #. '+ CAST(ChequeID AS varchar)  
WHERE ColumnID IN (SELECT ColumnID FROM t_IncomingTransactions (NoLock)  
WHERE EXISTS (SELECT ChequeID FROM t_ChequeTrx (NoLock)  
WHERE t_IncomingTransactions.OurbranchID  = t_ChequeTrx.OurbranchID   
  AND t_IncomingTransactions.AccountID  = t_ChequeTrx.AccountID  
  AND t_IncomingTransactions.AccountType = t_ChequeTrx.AccountTypeID   
  AND t_IncomingTransactions.ChequeID  = t_ChequeTrx.ChequeID   
  AND ChequeStatusID = 'P')   
  AND t_IncomingTransactions.TrxType = 'ID' AND ISNULL(IsProcessed,0)=0    
  AND isNull(t_IncomingTransactions.AccountType,'') = 'C'  
  AND t_IncomingTransactions.ReturnCode IN ('00','17'))   
  AND ISNULL(t_IncomingTransactions.FullName, '') <> dbo.f_GetGLAccountName(dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,CurrencyID, @TagSuspAcc_CHQ))  
----------------------------------------------------------------------------------------------------------------------------------------------  
----Update Stop Payment   
UPDATE t_IncomingTransactions   
 SET [Status] = 1,   
  ProductID = 'GL',  
  AccountType = 'G',  
  Policy2 = 'S',  
  AccountID  =dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,CurrencyID, @TagSuspAcc_CHQ),         
  FullName  = dbo.f_GetGLAccountName(dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,CurrencyID, @TagSuspAcc_CHQ)),       
  MainGLID  = dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,CurrencyID, @TagSuspAcc_CHQ),        
  Description = 'Account # '+ AccountID +' has Stopped Payment Cheque #. '+CAST(ChequeID AS varchar)  
WHERE ColumnID IN ( SELECT ColumnID FROM t_IncomingTransactions (NoLock)  
WHERE dbo.f_GetStoppedAccChequeIDStatus(OurBranchID,AccountID,ChequeID)='S'   
  AND t_IncomingTransactions.TrxType = 'ID' AND ISNULL(IsProcessed,0)=0    
  AND isNull(t_IncomingTransactions.AccountType,'') = 'C'  
  AND t_IncomingTransactions.ReturnCode = '00')   
  AND ISNULL(t_IncomingTransactions.FullName, '') <> dbo.f_GetGLAccountName(dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,CurrencyID, @TagSuspAcc_CHQ))  
-----------------------------------------------------------------------------------------------------------------------------------------------  
----Update Invalid Cheques----------------------------------------------------------------------------------------------------------------------      
UPDATE t_IncomingTransactions   
 SET [Status] = 1,    
  ProductID = 'GL',  
  AccountType = 'G',  
  AccountID  =dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,CurrencyID, @TagSuspAcc_CHQ),     
  FullName  = dbo.f_GetGLAccountName(dbo.f_GetCurrencyBranchGLAccountID(t_IncomingTransactions.TrxBranchID,t_IncomingTransactions.CurrencyID, @TagSuspAcc_CHQ)),       
  MainGLID  = dbo.f_GetCurrencyBranchGLAccountID(t_IncomingTransactions.TrxBranchID,t_IncomingTransactions.CurrencyID, @TagSuspAcc_CHQ),        
  Description = 'Account # '+ AccountID  +' has Invalid Cheque Series #. '+ISNULL(CAST(ChequeID AS varchar), '')  
WHERE ColumnID IN(SELECT Columnid FROM t_IncomingTransactions  (NoLock)  
WHERE NOT EXISTS( SELECT t_IncomingTransactions.chequeid FROM t_ChequeBook (NoLock)  
WHERE t_IncomingTransactions.OurBranchID = t_ChequeBook.OurBranchID   
  AND t_IncomingTransactions.AccountID = t_ChequeBook.AccountID   
  AND t_IncomingTransactions.chequeid BETWEEN t_ChequeBook.ChequeStart AND t_ChequeBook.ChequeEnd)   
  AND t_IncomingTransactions.TrxType = 'ID'  AND ISNULL(IsProcessed,0)=0   
  AND isNull(t_IncomingTransactions.AccountType,'') = 'C'  
  AND t_IncomingTransactions.ReturnCode = '00'   
  AND t_IncomingTransactions.vouchercode NOT IN( '40', '16' )  
  AND ISNULL(t_IncomingTransactions.FullName, '') <> dbo.f_GetGLAccountName(dbo.f_GetCurrencyBranchGLAccountID(t_IncomingTransactions.TrxBranchID,t_IncomingTransactions.CurrencyID, @TagSuspAcc_CHQ)))  
  AND TrxType ='ID'  
-------------------------------------------------------------------------------------------------------------------------------------------------------  
-----Loan Accounts-------------------------------------------------------------------------------------------------------------------------------------  
UPDATE t_IncomingTransactions       
    SET [Description] = 'Posting to loan Acc ('  + lrAccAsImported + ') not allowed',  
  [Status] = 1,      
  AccountType  = 'G',       
  ProductID  = 'GL',      
  CurrencyID  = (@HOBranchLocalBranchCurrency),      
  AccountID  = dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,CurrencyID, @TagSuspAcc_CHQ),        
  FullName  = dbo.f_GetGLAccountName(dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,CurrencyID, @TagSuspAcc_CHQ)),       
  MainGLID  = dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,CurrencyID, @TagSuspAcc_CHQ)        
WHERE dbo.f_GetProductTypeID(ourBankID,ProductID) In ('LN','LG') AND ISNULL(IsProcessed,0)=0 AND VoucherCode NOT IN ('13','14','40')  
  AND ISNULL(t_IncomingTransactions.FullName, '') <> dbo.f_GetGLAccountName(dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,CurrencyID, @TagSuspAcc_CHQ))  
  AND t_IncomingTransactions.ReturnCode IN ('00','17') AND t_IncomingTransactions.TrxType = 'ID'     
  
UPDATE t_IncomingTransactions       
    SET [Description] = 'Posting to loan Acc ('  + lrAccAsImported + ') not allowed',  
  [Status] = 1,      
  AccountType  = 'G',       
  ProductID  = 'GL',      
  CurrencyID  = (@HOBranchLocalBranchCurrency),      
  AccountID  = dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,CurrencyID, 'ACP_CLR_SUSP_AC'),   
  FullName  = dbo.f_GetGLAccountName(dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,CurrencyID, 'ACP_CLR_SUSP_AC')),       
  MainGLID  = dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,CurrencyID, 'ACP_CLR_SUSP_AC')        
WHERE dbo.f_GetProductTypeID(ourBankID,ProductID) In ('LN','LG','LA') AND ISNULL(IsProcessed,0)=0   
  AND ISNULL(t_IncomingTransactions.FullName, '') <> dbo.f_GetGLAccountName(dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,CurrencyID, 'ACP_CLR_SUSP_AC'))  
  AND t_IncomingTransactions.ReturnCode IN ('00','17') AND t_IncomingTransactions.TrxType = 'IC'     
----------------------------------------------------------------------------------------------------------------------------------------------------------  
----- Cross - Curreny Accounts ---------------------------------------------------------------------------------------------------------------------------  
UPDATE t_IncomingTransactions       
    SET [Description] = 'Cross Curr Posting Not allowed: ' + ' Acc ('  + lrAccAsImported + ') -  to Curr. ' +   
        CASE WHEN DRN IN ('J','T') THEN 'TZS'  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END,  
  AccountType  = 'G',       
  ProductID  = 'GL',   
  AccountID  = dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,CurrencyID, 'ACP_CLR_SUSP_AC'),       
  FullName  = dbo.f_GetGLAccountName(dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,CurrencyID, 'ACP_CLR_SUSP_AC')),       
  MainGLID  = dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,CurrencyID, 'ACP_CLR_SUSP_AC')       
WHERE ISNULL(IsProcessed,0)=0   
  AND ISNULL(t_IncomingTransactions.FullName, '') <> dbo.f_GetGLAccountName(dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,CurrencyID, 'ACP_CLR_SUSP_AC'))  
  AND LTRIM(RTRIM(ReturnCode)) = '00'   
  AND CurrencyID <> dbo.f_GetAccountCurrencyID(OurBranchID,AccountID)  
----------------------------------------------------------------------------------------------------------------------------------------------------------  
-------------------------Insufficient Balance-------------------------------------------------------------------------------------------------------------  
UPDATE t_IncomingTransactions       
SET    
--[Description] = TrxType + ' - Insufficient Available bal: Acc #. ('  + AccountID + ')' ,      
  [Status]  = 1 ,  
  Policy2 = 'B'  
WHERE Amount > dbo.f_GetAvailableBalance(OurBranchID,AccountID)   
  AND FullName <> dbo.f_GetGLAccountName(dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagSuspAcc_CHQ))   
          AND ISNULL(IsProcessed,0)= 0 AND TrxType = 'ID' AND ReturnCode IN ('00','17')  
  AND isNull(t_IncomingTransactions.AccountType,'') <> 'G'     
----------------------------------------------------------------------------------------------------------------------------------------------------------  
-------------------------Invalid AccountIDs---------------------------------------------------------------------------------------------------------------  
--UPDATE t_IncomingTransactions SET AccountID = '1234' WHERE isNull(IsProcessed,0) = 0 AND FullName = 'SALARY CONSOLIDATION ACCOUNT' AND TrxType = 'IC'  
  
  
UPDATE t_IncomingTransactions       
 SET [Description] = TrxType + ' - Invalid Account ID: ' + RIGHT(lrAccAsImported,10)  + ' For Brn ('  + ClrBrnAsImported + ') - Frm: Bank' + BankID,      
  AccountType  = 'G',       
  ProductID  = 'GL',  
  [Status]  = 1,       
  AccountID  = dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagSuspAcc_CHQ),       
  FullName  = dbo.f_GetGLAccountName(dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,CurrencyID, @TagSuspAcc_CHQ)),  
  MainGLID  = dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagControlAcc)       
WHERE dbo.f_IsAccountIDExist(AccountID) = 0 AND ISNULL(IsProcessed,0)=0 AND VoucherCode NOT IN ('13','14','40')  
  AND isNull(AccountType,'') <> 'G'  
  --AND  AccountID NOT IN (@BankersKESChequeAccount, @BankersGBPChequeAccount, @BankersEURChequeAccount, @BankersUSDChequeAccount)   
  AND ISNULL(t_IncomingTransactions.FullName, '') <> dbo.f_GetGLAccountName(dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagSuspAcc_CHQ))  
  AND TrxType ='ID'  
  
  
  
UPDATE t_IncomingTransactions       
 SET [Description] = TrxType + ' - Invalid Account ID: ' + RIGHT(lrAccAsImported,10)  + ' For Brn ('  + ClrBrnAsImported + ') - Frm: Bank' + BankID,      
  AccountType  = 'G',       
  ProductID  = 'GL',  
  [Status]  = 1,       
  AccountID  = dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagSuspAcc_EFT),       
  FullName  = dbo.f_GetGLAccountName(dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,CurrencyID, @TagSuspAcc_EFT)),  
  MainGLID  = dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagControlAcc)       
WHERE dbo.f_IsAccountIDExist(AccountID) = 0 AND ISNULL(IsProcessed,0)=0 --AND VoucherCode NOT IN ('13','14','40')  
  AND isNull(AccountType,'') <> 'G'  
  --AND  AccountID NOT IN (@BankersKESChequeAccount, @BankersGBPChequeAccount, @BankersEURChequeAccount, @BankersUSDChequeAccount)   
  AND ISNULL(t_IncomingTransactions.FullName, '') <> dbo.f_GetGLAccountName(dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagSuspAcc_EFT))  
  AND TrxType ='IC'  
  
-------------------------Direct Debits Mandates ----------------------------------------------------------------------      
  
IF NOT EXISTS (SELECT 1 FROM t_DirectDebitMaintenance (NOLOCK))      
BEGIN      
 UPDATE t_IncomingTransactions        
  SET FullName  = dbo.f_GetGLAccountName(dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagSuspAcc_EFT)),  
   AccountType    = 'G',  
   [Status]  = 1,       
   ProductID  = 'GL',      
   MainGLID  = dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagControlAcc),       
   AccountID  = dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagSuspAcc_EFT),       
   [Description]   = 'No Mandates Exists for this Account # ' + AccountID      
 WHERE VoucherCode     = '40'   
  
  
END       
ELSE      
BEGIN       
 UPDATE t_IncomingTransactions        
 SET  [Description]   = [Description] + ' InstructionID #.: ' + a.DDID       
 FROM t_DirectDebitMaintenance a (NOLOCK) INNER JOIN t_IncomingTransactions b (NOLOCK)      
 ON  a.DebitAccountID  = b.AccountID AND      
   a.DDStatusID = 'A'      
 WHERE a.OurBranchID = b.OurBranchID AND      
   a.DebitAccountID  = b.AccountID  AND      
   a.DDStatusID = 'A' AND       
   a.DDTypeID = 'Incoming Direct Debit' AND      
   RIGHT(LEFT(b.Data,4),2) = '40' AND ISNULL(b.IsProcessed,0) <> 1        
  
 UPDATE t_IncomingTransactions       
 SET  FullName  = dbo.f_GetGLAccountName(dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagSuspAcc_EFT)),       
   AccountType  = 'G',  
   [Status]  = 1,       
   ProductID  = 'GL',      
   MainGLID  = dbo.f_GetCurrencyBranchGLAccountID(b.TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagControlAcc),       
   AccountID  = dbo.f_GetCurrencyBranchGLAccountID(b.TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagSuspAcc_EFT),       
   [Description]   = 'Mandate Expired on ' + cast(a.lastexecutiondate as varchar) + ' for this Account # ' + b.AccountID      
 FROM t_DirectDebitMaintenance a  (NOLOCK) INNER JOIN t_IncomingTransactions b (NOLOCK)      
 ON  a.DebitAccountID  = b.AccountID   
 WHERE a.OurBranchID = b.OurBranchID AND      
   a.DebitAccountID  = b.AccountID AND      
   a.LastExecutionDate <  dbo.f_getworkingdate(b.OurBranchID) AND       
   a.Amount = b.Amount AND      
   a.Policy1 = b.Policy1 AND      
   a.OrigCode =  b.OriginatorCode  AND      
   a.OrigRef = b.OriginatorRef  AND      
   a.DDStatusID = 'A' AND      
   a.DDTypeID = 'Incoming Direct Debit' AND      
   RIGHT(LEFT(b.Data,4),2)='40'      
   AND ISNULL(b.IsProcessed,0) <> 1       
   AND ISNULL(b.FullName,'')<>'ACP CLEARING SUSPENSE ACC'      
  
 UPDATE t_IncomingTransactions        
 SET  FullName  = dbo.f_GetGLAccountName(dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagSuspAcc_EFT)),   
   AccountType  = 'G',  
   [Status]  = 1,       
   ProductID  = 'GL',      
   MainGLID  = dbo.f_GetCurrencyBranchGLAccountID(b.TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagControlAcc),       
   AccountID  = dbo.f_GetCurrencyBranchGLAccountID(b.TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagSuspAcc_EFT),       
   [Description]   = 'Mandate Disabled for this Account # ' + b.AccountID      
 FROM t_DirectDebitMaintenance a (NOLOCK)INNER JOIN t_IncomingTransactions b (NOLOCK)      
 ON  a.DebitAccountID  = b.AccountID      
 WHERE a.OurBranchID = b.OurBranchID AND      
   a.DebitAccountID  = b.AccountID AND      
   a.Amount = b.Amount AND      
   a.LastExecutionDate > dbo.f_getworkingdate(b.OurBranchID) AND      
   a.Amount = b.Amount AND       
   a.Policy1 = b.Policy1 AND      
   a.OrigCode =  b.OriginatorCode  AND      
   a.OrigRef = b.OriginatorRef  AND      
   a.DDStatusID = 'D' AND      
   a.DDTypeID = 'Incoming Direct Debit' AND      
   RIGHT(LEFT(b.Data,4),2)='40'       
   AND ISNULL(b.IsProcessed,0) <> 1       
   AND ISNULL(b.FullName,'')<>'ACP CLEARING SUSPENSE ACC'      
  
 UPDATE t_IncomingTransactions        
 SET  FullName  = dbo.f_GetGLAccountName(dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagSuspAcc_EFT)),   
   AccountType  = 'G',  
   [Status]  = 1,       
   ProductID  = 'GL',      
   MainGLID  = dbo.f_GetCurrencyBranchGLAccountID(b.TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagControlAcc),       
   AccountID  = dbo.f_GetCurrencyBranchGLAccountID(b.TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagSuspAcc_EFT),       
   [Description]   = 'Invalid Mandate Amount for this Account # ' + b.AccountID      
 FROM t_DirectDebitMaintenance a (NOLOCK) INNER JOIN t_IncomingTransactions b (NOLOCK)      
 ON  a.DebitAccountID  = b.AccountID      
 WHERE a.OurBranchID = b.OurBranchID And      
   a.DebitAccountID  = b.AccountID And      
   a.Amount <> b.Amount And      
   a.LastExecutionDate > dbo.f_getworkingdate(b.OurBranchID) And      
   a.Policy1 = b.Policy1 And    
   a.OrigCode = b.OriginatorCode  And      
   a.OrigRef = b.OriginatorRef  And      
   a.DDStatusID = 'A' And      
   a.DDTypeID = 'Incoming Direct Debit' And      
   RIGHT(LEFT(b.Data,4),2)='40'      
   AND a.Amount <> 0      
   AND ISNULL(b.IsProcessed,0) <> 1       
   AND ISNULL(b.FullName,'')<>dbo.f_GetGLAccountName(dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagSuspAcc_EFT))    
  
 UPDATE t_IncomingTransactions        
 SET  FullName  = dbo.f_GetGLAccountName(dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagSuspAcc_EFT)),       
   AccountType  = 'G',   
   [Status]  = 1,      
   ProductID  = 'GL',   
   MainGLID  = dbo.f_GetCurrencyBranchGLAccountID(b.TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagControlAcc),       
   AccountID  = dbo.f_GetCurrencyBranchGLAccountID(b.TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagSuspAcc_EFT),       
   [Description]    = 'Invalid Mandate Policy Numbers for this Account # ' + b.AccountID       
 FROM t_DirectDebitMaintenance a  (NOLOCK) INNER JOIN t_IncomingTransactions b (NOLOCK)      
 ON  a.DebitAccountID  = b.AccountID      
 WHERE a.OurBranchID = b.OurBranchID And      
   a.DebitAccountID  = b.AccountID And      
   a.Amount = b.Amount And      
   a.LastExecutionDate > dbo.f_getworkingdate(b.OurBranchID) And      
   a.Policy1 <> b.Policy1 AND      
   a.OrigCode =  b.OriginatorCode  And      
   a.OrigRef = b.OriginatorRef  And      
   DDStatusID = 'A' And       
   DDTypeID = 'Incoming Direct Debit' And    
   RIGHT(LEFT(b.Data,4),2)='40'       
   AND ISNULL(b.IsProcessed,0) <> 1      
   AND ISNULL(b.FullName,'')<> dbo.f_GetGLAccountName(dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagSuspAcc_EFT))    
  
 UPDATE t_IncomingTransactions        
 SET  FullName  = dbo.f_GetGLAccountName(dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagSuspAcc_EFT)),    
   AccountType  = 'G',  
   [Status]  = 1,       
   ProductID  = 'GL',      
   MainGLID  = dbo.f_GetCurrencyBranchGLAccountID(b.TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagControlAcc),       
   AccountID  = dbo.f_GetCurrencyBranchGLAccountID(b.TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagSuspAcc_EFT),       
   [Description]   = 'Invalid Mandate Originators Code for this Account # ' + b.AccountID       
 FROM t_DirectDebitMaintenance a (NOLOCK) INNER JOIN t_IncomingTransactions b (NOLOCK)      
 ON  a.DebitAccountID  = b.AccountID      
 WHERE a.OurBranchID = b.OurBranchID And      
   a.DebitAccountID  = b.AccountID And      
   a.Amount = b.Amount And      
   a.LastExecutionDate > dbo.f_getworkingdate(b.OurBranchID) And      
   a.Policy1 = b.Policy1 And      
   a.OrigCode <>  b.OriginatorCode  And      
   a.OrigRef = b.OriginatorRef  And      
   a.DDStatusID = 'A' And       
   a.DDTypeID = 'Incoming Direct Debit' And      
   RIGHT(LEFT(b.Data,4),2)='40'      
   AND ISNULL(b.IsProcessed,0) <> 1      
   AND ISNULL(b.FullName,'')<> dbo.f_GetGLAccountName(dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagSuspAcc_EFT))      
  
 UPDATE t_IncomingTransactions        
 SET  FullName  = dbo.f_GetGLAccountName(dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagSuspAcc_EFT)),      
   AccountType  = 'G',  
   [Status]  = 1,       
   ProductID  = 'GL',      
   MainGLID  = dbo.f_GetCurrencyBranchGLAccountID(b.TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagControlAcc),       
   AccountID  = dbo.f_GetCurrencyBranchGLAccountID(b.TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagSuspAcc_EFT),       
   [Description]   = 'Invalid Mandate OriginatorRef Referecen NO. for this Account # ' + b.AccountID      
 FROM t_DirectDebitMaintenance a (NOLOCK) INNER JOIN t_IncomingTransactions b (NOLOCK)      
 ON  a.DebitAccountID  = b.AccountID      
 WHERE a.OurBranchID = b.OurBranchID And      
   a.DebitAccountID  = b.AccountID And      
   a.Amount = b.Amount And      
   a.LastExecutionDate > dbo.f_getworkingdate(b.OurBranchID) And      
   a.Policy1 = b.Policy1 And      
   a.OrigCode =  b.OriginatorCode  And      
   a.OrigRef <> b.OriginatorRef  And      
   a.DDStatusID = 'A' And       
   a.DDTypeID = 'Incoming Direct Debit' And      
   RIGHT(LEFT(b.Data,4),2)='40'       
   AND ISNULL(b.IsProcessed,0) <> 1       
   AND ISNULL(b.FullName,'')<> dbo.f_GetGLAccountName(dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagSuspAcc_EFT))    
  
 UPDATE t_IncomingTransactions        
 SET  FullName  = dbo.f_GetGLAccountName(dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagSuspAcc_EFT)),       
   AccountType     = 'G',   
   [Status]  = 1,      
   ProductID  = 'GL',      
   MainGLID  = dbo.f_GetCurrencyBranchGLAccountID(b.TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagControlAcc),       
   AccountID  = dbo.f_GetCurrencyBranchGLAccountID(b.TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagSuspAcc_EFT),       
   [Description]   = 'Mandate Does Not Exists for this Account # ' + b.AccountID      
 FROM t_DirectDebitMaintenance a (NOLOCK) RIGHT OUTER JOIN t_IncomingTransactions b (NOLOCK)      
 ON  a.OurBranchID = b.OurBranchID AND      
   a.DebitAccountID  = b.AccountID      
 WHERE a.OurBranchID is null AND      
   a.DebitAccountID  is null AND      
   RIGHT(LEFT(b.Data,4),2) = '40' AND ISNULL(b.IsProcessed,0) <> 1       
   AND ISNULL(b.FullName,'')<> dbo.f_GetGLAccountName(dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagSuspAcc_EFT))    
END    
-------------------------------------------------------------------------------------------------------------------------------------------------------------  
DECLARE Trx CURSOR FOR      
SELECT ColumnID,OurBranchID,AccountID,ChequeID,AccountType      
 FROM t_IncomingTransactions    (NOLOCK)     
 WHERE RIGHT(LEFT(t_IncomingTransactions.Data,4),2) NOT IN ('13','14','40') AND isNull(FullName,'') <> dbo.f_GetGLAccountName(dbo.f_GetCurrencyBranchGLAccountID(TrxBranchID,(  
        CASE WHEN DRN IN ('J','T') THEN @HOBranchLocalBranchCurrency  
          WHEN DRN IN ('E') AND VoucherCode = '60' THEN 'USD'  
          WHEN DRN IN ('E') AND VoucherCode = '61' THEN 'GBP'  
          WHEN DRN IN ('E') AND VoucherCode = '62' THEN 'EUR' END), @TagSuspAcc_CHQ))    
 AND AccountID NOT IN (dbo.f_GetCurrencyBranchGLAccountID(OurBranchID,CurrencyID, 'CL_BANKERCQL_AC')) AND isNull(IsProcessed,0) =0       
OPEN Trx      
FETCH NEXT FROM Trx INTO @ColumnID,@OurBranchID,@AccountID,@ChequeID,@AccountTypeID   
WHILE @@FETCH_STATUS = 0      
BEGIN        
DECLARE @OldUnpColumnID BigInt, @OldPdTrxRowID BigInt, @UnpDate SmallDatetime, @PChequeID Int, @PAccountID AccountID  
--Verify if Unpaid before  
SELECT TOP 1 @OldUnpColumnID = a.ColumnID, @UnpDate = a.Date   
FROM t_AccountTrxClearing a (NOLOCK), t_IncomingTransactions b  (NOLOCK)  
WHERE A.AccountID =b.AccountID AND A.Date < b.Date  
 AND Abs(A.Amount) = Abs(b.Amount) AND a.OurBranchID = b.OurBranchID  
 AND A.TrxType = 'OC' AND a.ReturnCodeID NOT IN ('00','17')  
 AND a.BankID = B.BankID AND A.BranchID = B.BranchID  
 AND a.DrawerOrPayeeAccountID = B.TheirAccount  
 AND a.ChequeID = b.ChequeID AND a.ChequeDigit = b.ChequeDigit  
 AND b.ColumnID = @ColumnID      
ORDER BY A.ColumnID DESC  
  
IF isNull(@OldUnpColumnID,0) = 0  
 BEGIN  
  --Do nothing  
  SELECT 'Do nothing'  
 END  
ELSE  
 BEGIN  
  --Verify if it was represented again after been unpaid before  
  IF (SELECT 1 FROM t_AccountTrxClearing a  (NOLOCK) , t_IncomingTransactions b  (NOLOCK)   
   WHERE a.Date > @UnpDate AND  A.AccountID = b.AccountID AND A.Date < b.Date  
    AND Abs(A.Amount) = Abs(b.Amount) AND a.OurBranchID = b.OurBranchID  
    AND A.TrxType = 'ID' AND a.ReturnCodeID IN ('00','17')  
    AND a.BankID = B.BankID AND A.BranchID = B.BranchID  
    AND a.DrawerOrPayeeAccountID = B.TheirAccount  
    AND a.ChequeID = b.ChequeID AND a.ChequeDigit = b.ChequeDigit  
    AND b.ColumnID = @ColumnID ) = 1  
   BEGIN  
    --Do nothing  
    SELECT 'Do nothing'  
   END  
  ELSE  
   BEGIN  
    SELECT @OldPdTrxRowID = TrxRowID , @PAccountID = AccountID, @PChequeID = ChequeID  
    FROM t_AccountTrxClearing (NOLOCK) WHERE ColumnID = @OldUnpColumnID AND TrxType = 'ID'  
    IF isNull(@OldPdTrxRowID,'') <> ''  
     BEGIN  
      DELETE FROM t_ChequeTrx WHERE TrxRowID = @OldPdTrxRowID  
     END  
    ELSE  
     BEGIN  
      DELETE FROM t_ChequeTrx WHERE AccountID = @PAccountID AND ChequeID = @PChequeID  
     END  
   END  
 END  
FETCH NEXT FROM Trx INTO @ColumnID,@OurBranchID,@AccountID,@ChequeID,@AccountTypeID      
END      
CLOSE Trx      
DEALLOCATE Trx    
    
UPDATE t_IncomingTransactions SET InstrumentTypeID = '00', Validity = 1, ImageID = ColumnID, Reason = lrAccAsImported  
WHERE ISNULL(IsProcessed,0) = 0 AND TrxType ='ID'  
  
--UPDATE t_IncomingTransactions SET OurBranchID = '000'  
--WHERE ISNULL(IsProcessed,0) = 0 AND TrxType ='IC' AND OurBranchID ='001'  
  
  
  
  
  
  
  
SET NOCOUNT OFF      
SET DEADLOCK_PRIORITY NORMAL   
  
  
  
  