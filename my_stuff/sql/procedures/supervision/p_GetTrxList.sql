CREATE PROCEDURE dbo.p_GetTrxList  
(  
 @TrxBranchID BranchID,  
 @ModuleID  SmallInt,  
 @OperatorID  OperatorID = NULL  
)  
  
   
AS  
BEGIN  
 SET NOCOUNT ON  
 DECLARE @IsVault BIT  
 SELECT @IsVault = IsVault FROM t_Till WHERE CashierID = @OperatorID And OurBranchID=@TrxBranchID  
 If @ModuleID in (3060, 3070,3061)  
 BEGIN  
    
  SELECT distinct t_TrxClearing.TrxRowID,t_TrxClearing.TrxBatchID,SerialID,  
   t_TrxClearing.OurBranchID,t_TrxClearing.AccountTypeID,t_TrxClearing.AccountID,  
   TrxTypeID,t_TrxClearing.ValueDate, --TrxDate,  
   TrxCurrencyID,t_TrxClearing.Amount TrxAmount,t_Transaction.LocalAmount,1 ExchangeRate,  
   ISNULL(t_TrxClearing.ChequeID,'') ChequeID,  
   t_TrxClearing.ChequeDate,  
   ReferenceNo, --Becuase to avoid removing column  
   TrxDescriptionID,TrxDescription,TrxFlagID,  
   CreatedBy OperatorID,  
   dbo.f_GetAccountName(t_TrxClearing.OurBranchID,t_TrxClearing.AccountID) [AccountName],  
   isNull(t_Transaction.SupervisedBy,'Not Supervised') SupervisedBy  
  FROM t_Transaction  
   Inner Join t_TrxClearing On t_TrxClearing.TrxBranchID = t_Transaction.TrxBranchID  
   LEFT OUTER Join t_TrxValueDated On t_TrxClearing.TrxBranchID = t_TrxValueDated.OurBranchID  
   And t_TrxClearing.TrxRowID = t_TrxValueDated.TrxRowID  
  WHERE t_TrxClearing.TrxBranchID = @TrxBranchID  
    AND DeletedOn IS NULL  
    And t_TrxClearing.TrxRowID = t_Transaction.TrxRowid  
    --AND ModuleID = @ModuleID  
    order by SerialID return -- requested by FASL (J.Ndetei)   
 End  
 Else  
 Begin  
  If @ModuleID = 3075  
   Select TransactionDate, ControlNumber,dbo.f_GetBranchName(SenderOurBranchID) SendingBranch,  
    Case isNull(AccountID,'') When '' Then SenderName  
          Else (RTRIM(LTRIM(dbo.f_GetAccountName(SenderOurBranchID,AccountID))))   
    End as Sender,  
    dbo.f_GetBranchName(RecieverOurBranchID) RecievingBranch ,  
    Recievername RecieverName, isNull(Paid,0) Paid, isNull(Cancelled,0) Cancelled,Amount AS TransactionAmount,RecievedAmount,  
    Commission, Tax, IsDeactivated Active  
   From t_MoneyTransfer Where SenderOurBranchID = @TrxBranchID or RecieverOurBranchID = @TrxBranchID  
    And Paid is Null or Cancelled is Null  
   Order by TransactionDate, ControlNumber  
  Else if @ModuleID In (3000,3010)  
   SELECT TrxRowID,TrxBatchID,SerialID,  
    OurBranchID,AccountTypeID,AccountID,  
    (RTRIM(LTRIM(dbo.f_GetAccountName(OurBranchID,AccountID)))) AccountName,  
    TrxTypeID,  
    cast(datepart(dd,ValueDate) as varchar(2))+'-'+cast(datepart(mm,ValueDate) as varchar(2))+'-'+cast(datepart(yyyy,ValueDate) as varchar(4)) as ValueDate, --TrxDate,  
    TrxCurrencyID,TrxAmount,LocalAmount,ExchangeRate,  
    ISNULL(ChequeID,'') ChequeID,  
    cast(datepart(dd,ChequeDate) as varchar(2))+'-'+cast(datepart(mm,ChequeDate) as varchar(2))+'-'+cast(datepart(yyyy,ChequeDate) as varchar(4)) as ChequeDate,  
    ISNULL(ReferenceNo,'') ReferenceNo, --Becuase to avoid removing column  
    TrxDescriptionID,  
    CASE WHEN ISNULL(Remarks,'')='' then ISNULL(TrxDescription,'')  ELSE  
    ISNULL(TrxDescription,'') + ' ||Narration : ' + CAST(ISNULL(Remarks,'') as Varchar(200)) END TrxDescription,     
    TrxFlagID,  
    CreatedBy OperatorID,  
    isnull(SupervisedBy,'')SupervisedBy  
   FROM t_Transaction(NOLOCK)  
   WHERE TrxBranchID = @TrxBranchID  
     AND DeletedOn IS NULL  
     AND TrxCodeID = 0  
     --AND ModuleID = @ModuleID  
     AND ModuleID IN ('3000','3010','3130')  
     AND CreatedBy = (CASE ISNULL(@IsVault,0) WHEN 0 THEN ISNULL(@OperatorID,CreatedBy) ELSE CreatedBy END)   
     --AND TrxFlagID='U'  
     order by cast(TrxBatchID as bigint), SerialID   
   ELSE         
   SELECT TrxRowID,TrxBatchID,SerialID,  
    OurBranchID,AccountTypeID,AccountID,  
    (RTRIM(LTRIM(dbo.f_GetAccountName(OurBranchID,AccountID)))) AccountName,  
    TrxTypeID,  
    cast(datepart(dd,ValueDate) as varchar(2))+'-'+cast(datepart(mm,ValueDate) as varchar(2))+'-'+cast(datepart(yyyy,ValueDate) as varchar(4)) as ValueDate, --TrxDate,  
    TrxCurrencyID,TrxAmount,LocalAmount,ExchangeRate,  
    ISNULL(ChequeID,'') ChequeID,  
    cast(datepart(dd,ChequeDate) as varchar(2))+'-'+cast(datepart(mm,ChequeDate) as varchar(2))+'-'+cast(datepart(yyyy,ChequeDate) as varchar(4)) as ChequeDate,  
    ISNULL(ReferenceNo,'') ReferenceNo, --Becuase to avoid removing column  
    TrxDescriptionID,TrxDescription,TrxFlagID,  
    CreatedBy OperatorID,  
    isnull(SupervisedBy,'')SupervisedBy  
   FROM t_Transaction(NOLOCK)  
   WHERE TrxBranchID = @TrxBranchID  
     AND DeletedOn IS NULL  
     AND TrxCodeID = 0  
     AND ModuleID IN (@ModuleID,3149)  
     --AND CreatedBy = ISNULL(@OperatorID,CreatedBy)  
     order by SerialID -- requested by FASL (J.Ndetei)  
 End  
 SET NOCOUNT OFF  
END  
  
  
--go  
--exec p_GetTrxList @TrxBranchID=N'306',@ModuleID=3000,@OperatorID=N'egikunju'  
  