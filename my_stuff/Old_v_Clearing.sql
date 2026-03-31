  
CREATE      VIEW [dbo].[v_Clearing]  
   
AS  
 SELECT TrxRowID,  
            Trxbatchid,  
   TrxbatchSlNo,  
   OurBranchID,  
   AccountTypeID,  
   AccountID,  
   ChequeDigit,  
   VoucherCode,  
   ReturnCodeID,  
   Commission,  
   TheirCommission,  
   PinNumber,  
   PaymentType,  
   PaymentTypeID,  
   PaymentDate,  
   VATName,  
   VATNumber,  
   VATType,  
      VATSerialNo,  
   BankID,  
   BranchID,  
   DrawerOrPayeeAccountID,  
   DrawerOrPayee,  
   Amount,  
   ChequeID,  
   ChequeDate,  
   Valuedate,  
   CurrencyID,  
   Date TrxDate,  
   OriginatorCode,  
   OrigRefCode,  
   PolicyNumber1,  
   PolicyNumber2,  
   TrxBranchID,  
   Trxtype TrxTypeID,  
   ImageID,  
   IsUnpaidItem   
 FROM t_AccountTrxClearing(NOLOCK)  
 UNION   
 SELECT TrxRowID,  
   Trxbatchid,  
   TrxbatchSlNo,  
   OurBranchID,  
   AccountTypeID,  
   AccountID,  
   ChequeDigit,  
   VoucherCode,  
   ReturnCodeID,  
   Commission,  
   TheirCommission,  
   PinNumber,  
   PaymentType,  
   PaymentTypeID,  
   PaymentDate,  
   VATName,  
   VATNumber,  
   VATType,  
   VATSerialNo,  
   BankID,  
   BranchID,  
   DrawerOrPayeeAccountID,  
   DrawerOrPayee,  
   Amount,  
   ChequeID,  
   ChequeDate,  
   Valuedate,  
   CurrencyID,  
   Date TrxDate,  
   OriginatorCode,  
   OrigRefCode,  
   PolicyNumber1,  
   PolicyNumber2,  
   TrxBranchID,  
   Trxtype TrxtypeID,  
   ImageID,  
   IsUnpaidItem  
 FROM t_trxclearing(NOLOCK)  
  
  
  
   
  
  
  
  
  
  
  
  
  
  
  
  
  