CREATE PROCEDURE [dbo].[sb_EndOfDay]  
(  
 @OurBranchID dbo.BranchID,  
 @ProcessDate smalldatetime,  
 @OperatorID  dbo.OperatorID,  
 @ErrorNo  Int = 1 OUTPUT  
)  
----WITH ENCRYPTION  
AS  
BEGIN  
   
 SET NOCOUNT ON  
  
 DECLARE @SOD_Date SmallDateTime,  
   @EOD_Date SmallDateTime,  
   @CE_Date SmallDateTime,  
   @ISDayOpen Bit,  
   @Year  SmallInt,  
   @BankID  varchar(4),  
   @MobiLoanProductID nVARCHAR(6) = 'K51',  
   @ClosedBy  NVARCHAR(20) = 'SYS',  
   @CloseReasonID NVARCHAR(20) = '006',  
   @CloseReason NVARCHAR(255) = 'Loan Fully Paid',  
   @ErrorParams XML   
   --IsTrxAllow,IsReadyForClosing as of now we are not considering , we require some UI, for these  
   
 SELECT    
  @SOD_Date = SODDate,  
  @EOD_Date = EODDate,   
  @CE_Date = CEDate,  
  @ISDayOpen = IsDayOpen   
 FROM t_SystemBranchStatus(NOLOCK)   
 WHERE OurBranchID = @OurBranchID  
  
 Select @BankID = dbo.f_GetBankID(@OurBranchID)  
  
 Declare @NextWorkingDate  datetime  
  
 ----Should Only on last working date of the Month.  
 Select @NextWorkingDate = dbo.f_GetNextWorkingDate(@OurBranchID,@ProcessDate)  
   
 IF (@EOD_Date >= @ProcessDate) OR (@ProcessDate < @SOD_Date)  
 BEGIN  
  --SELECT 'EOD_DONE' AS Status -- End Of Day Done.  
  SET @ErrorNo = 600003  
  RETURN  
 END  
  
 IF (@SOD_Date <> @ProcessDate)   
 BEGIN  
  --SELECT 'BOD_NOT_DONE' AS Status  -- Begn Of Day Not Done.  
  SET @ErrorNo = 600004  
   RETURN  
 END  
  
  
   
 IF (Select Top 1 Count(AccountID) from t_GLInterface (NOLOCK) Where AccountTagID = 'CONTROL_AC' Group By AccountID Having COUNT(AccountID) > 1) > 0  
 BEGIN  
  SET @ErrorNo = 603019  
  RETURN  
 END  
  
 IF (Select Top 1 Count(AccountID) from t_GLInterface (NOLOCK) Where Module = 'CA' And AccountTagID = 'DR_CONTROL_AC' Group By AccountID Having COUNT(AccountID) > 1) > 0  
 BEGIN  
  SET @ErrorNo = 603016  
  RETURN  
 END  
/*  
 IF (Select Top 1 Count(AccountID) from t_GLInterface (NOLOCK) Where Module = 'LN' And AccountTagID = 'GEN_PROV_AC' Group By AccountID Having COUNT(AccountID) > 1) > 0  
 BEGIN  
  SET @ErrorNo = 603017  
  RETURN  
 END  
  
 IF (Select Top 1 Count(AccountID) from t_GLInterface (NOLOCK) Where Module = 'LN' And AccountTagID = 'SPC_PROV_AC' Group By AccountID Having COUNT(AccountID) > 1) > 0  
 BEGIN  
  SET @ErrorNo = 603018  
  RETURN  
 END  
*/  
  
  
  
  
 ---- to check, if Back up is taken  
 --IF NOT EXISTS (SELECT TOP 1 OurBranchID FROM t_SystemBranchStatus WHERE BackUpDate = @ProcessDate)  
 --BEGIN  
 -- SET @ErrorNo = 600031  
 -- RETURN  
 --END  
     
 ----- To check HO to do EOD at last  
 --IF @OurBranchID = dbo.f_GetHOBranchID(dbo.f_GetBankID(@OurBranchID))  
 --BEGIN  
 -- IF EXISTS (SELECT TOP 1 OurBranchID FROM t_SystemBranchStatus   
 --  WHERE OurBranchID <> @OurBranchID AND BranchStatus = 1 AND EODDate < @ProcessDate)  
 -- BEGIN  
 --  SET @ErrorNo = 600017  
 --   RETURN  
 -- END  
 --END     
     
 SET @ErrorNo = 1  
  
 EXEC p_CloseMultipleLoanSeries @OurBranchID  
  
 Update t_Transaction SET ProductID =  dbo.f_GetAccountProductID(OurBRanchID, AccountID)  
 Where OurBranchID = @OurBranchID And TrxDate = @ProcessDate  
  And TrxDate > '20 Oct 2014'   
  And ProductID <> dbo.f_GetAccountProductID(OurBRanchID, AccountID)  
  And AccountTypeID = 'C'  
  
 Update t_Transaction SET  MainGLID = dbo.f_GetGLInterfaceAccountID(@BankID,ProductID, dbo.f_GetProductTypeID(@BankID,ProductID),'CONTROL_AC')  
 Where OurBranchID = @OurBranchID And TrxDate = @ProcessDate  
  And TrxDate > '20 Oct 2014'   
  And MainGLID <> dbo.f_GetGLInterfaceAccountID(@BankID,ProductID, dbo.f_GetProductTypeID(@BankID,ProductID),'CONTROL_AC')  
  And AccountTypeID = 'C'  
  
  
 UPDATE t_SystemBranchStatus    
 SET IsDayOpen    = 0,  
  IsReadyForClosing = 0  
 WHERE OurBranchID = @OurBranchID  
  
 --Last Debit/Cedit Date Updation  
 UPDATE t_AccountCustomer  
 SET LastDebitTrxDate = @ProcessDate  
 WHERE OurBranchID = @OurBranchID  
  AND AccountID IN   
    (  
    SELECT AccountID  
    FROM t_Transaction  
    WHERE OurBranchID = @OurBranchID  
     AND TrxDate = @ProcessDate  
     AND TrxCodeID = 0  
     AND Amount < 0  
    )  
  
 --Last Debit/Cedit Date Updation  
 UPDATE t_AccountCustomer  
 SET LastCreditTrxDate = @ProcessDate  
 WHERE OurBranchID = @OurBranchID  
  AND AccountID IN   
    (  
     SELECT AccountID  
     FROM t_Transaction WITH (NOLOCK)  
     WHERE OurBranchID = @OurBranchID AND TrxDate = @ProcessDate AND TrxCodeID = 0 AND Amount > 0  
    )  
  
 DELETE FROM t_UserRole  
 WHERE isnull(ExpiryDate,'') <>'' And OurBranchID=@OurBranchID  
 AND cast (ExpiryDate as smalldatetime)<=cast (@ProcessDate as smalldatetime)  
  
 EXEC ps_ExecuteSubProcess @OurBranchID = @OurBranchID,  
        @ProcessDate = @ProcessDate,  
        @OperatorID  = @OperatorID,  
        @ProcessID  = 'EOD',  
        @ErrorNo  = @ErrorNo OUTPUT  
  
 IF @ErrorNo > 0  
  GOTO EndStatement  
  
   
  
 EXEC ps_TransferForeignToLocalBalance @OurBranchID= @OurBranchID,  
           @ProcessID='EOD',  
           @ProcessDate=@ProcessDate,  
           @ErrorNo=@ErrorNo OUTPUT,  
           @ErrorParams=@ErrorParams  
  
 IF @ErrorNo > 0  
  GOTO EndStatement  
  
 BEGIN TRANSACTION  
  
 DELETE t_AccountSpecialCondition Where OurBranchID = @OurBranchID And SpecialConditionID = '145'  
   
 --EXEC ps_UpdateGLBalance @OurBranchID = @OurBranchID,  
 --      @WorkingDate = @ProcessDate,  
 --      @ErrorNo  = @ErrorNo OUTPUT  
  
 IF @ErrorNo > 0  
  GOTO EndStatement  
  
 EXEC [dbo].ps_ODDebitControlMovement  
  @OurBranchID = @OurBranchID,  
  @ProcessID  = 'EOD',  
  @ProcessDate = @ProcessDate,  
  @ErrorNo  = @ErrorNo OUTPUT,  
  @ErrorParams = @ErrorParams OutPut  
  
  
  IF @ErrorNo > 1  
   GOTO EndStatement   
  
  
  
  
  EXEC ps_UpdateGLBalance @OurBranchID = @OurBranchID,  
        @WorkingDate = @ProcessDate,  
        @ErrorNo  = @ErrorNo OUTPUT  
  
  IF @ErrorNo > 0  
   GOTO EndStatement  
  
  EXEC ps_UpdateFDAccruedInterest @OurBranchID =@OurBranchID  
   
  UPDATE t_AccountCustomer  
  SET FreezedAmount = FreezedAmount - ( SELECT ISNULL(SUM(FreezedValue),0)  
             FROM t_AccountFreeze(NOLOCK)  
             WHERE LoanBranchID  = t_Loan.OurBranchID  
              AND LoanAccountID = t_Loan.AccountID  
              AND  FreezeCateGoryID IN ('CF','IF')  
              AND ReleasedDate IS NULL  
              AND EffectiveDate <= dbo.f_GetWorkingDate(@OurBranchID))  
  FROM t_AccountCustomer(NOLOCK)  
  INNER JOIN  v_GroupMemberScheme  
  ON t_AccountCustomer.OurBranchID   = v_GroupMemberScheme.OurBranchID  
   AND t_AccountCustomer.AccountID   = v_GroupMemberScheme.SavingsAccountID  
  INNER JOIN t_Loan(NOLOCK)  
  ON t_Loan.OurBranchID      = v_GroupMemberScheme.OurBranchID  
   AND t_Loan.AccountID     = v_GroupMemberScheme.LoanAccountID  
   AND  t_Loan.OutstandingPrincipal  =   0   
   AND  t_Loan.LoanStatusID    =   'A'   
  WHERE t_Loan.OurBranchID     = @OurBranchID   
   AND dbo.f_GetClearBalance(@OurBranchID ,t_Loan.AccountID) = 0  
  
  UPDATE t_AccountFreeze   
  SET ReleasedDate = dbo.f_GetWorkingDate(@OurBranchID),  
   ReleasedReason = dbo.f_GetSystemMessage(600023,dbo.f_GetDefaultLanguageID())  
  FROM t_AccountFreeze  
  INNER JOIN t_Loan  
  ON   t_AccountFreeze.LoanBranchID   = t_Loan.OurBranchID  
   AND  t_AccountFreeze.LoanAccountID  = t_Loan.AccountID  
   AND  t_Loan.OutstandingPrincipal  =   0   
   AND  t_Loan.LoanStatusID    =   'A'   
   AND  t_AccountFreeze.FreezeCateGoryID IN ('CF','IF')  
  INNER JOIN t_AccountCustomer  
  ON t_Loan.OurBranchID   = t_AccountCustomer.OurBranchID  
   AND t_Loan.AccountID  = t_AccountCustomer.AccountID   
   AND t_AccountCustomer.ClearBalance = 0  
  WHERE t_Loan.OurBranchID = @OurBranchID   
  
  
  EXEC p_CloseLoanSeries @OurBranchID =@OurBranchID,@ProcessDate  = @ProcessDate ,@OperatorID =@OperatorID  
          
  UPDATE t_AccountCustomer  
  SET   AccountStatusID = 'AB',  
     IsBlocked   = 1  
  FROM t_AccountCustomer  
  INNER JOIN t_Loan  
  ON  t_Loan.OurBranchID = t_AccountCustomer.OurBranchID  
   AND t_Loan.AccountID = t_AccountCustomer.AccountID  
   AND t_AccountCustomer.ClearBalance = 0  
  WHERE t_Loan.OutstandingPrincipal = 0  
   AND t_Loan.ClosedDate   IS NULL  
   AND t_Loan.OurBranchID   = @OurBranchID  
   AND t_Loan.LoanStatusID  IN ( 'A','N' )  
     
  UPDATE t_AccountCustomer  
  SET   AccountStatusID = 'AC',  
     ClosedBy   = @ClosedBy,  
     ClosedDate   = @ProcessDate,  
     CloseReasonID  = @CloseReasonID,  
     CloseReason  = @CloseReason  
  FROM t_AccountCustomer  
  INNER JOIN t_Loan  
  ON  t_Loan.OurBranchID = t_AccountCustomer.OurBranchID  
   AND t_Loan.AccountID = t_AccountCustomer.AccountID  
   AND t_AccountCustomer.ClearBalance = 0  
  WHERE --t_Loan.OutstandingPrincipal = 0  
   t_Loan.ClosedDate   IS NOT NULL  
   AND t_Loan.OurBranchID   = @OurBranchID  
   AND t_AccountCustomer.ProductID = @MobiLoanProductID  
   AND t_Loan.LoanStatusID IN ( 'F','P' )  
   AND t_AccountCustomer.ClosedDate IS NULL  
  
        EXEC p_UnfreezeGuaranteedAmount @OurBranchID =@OurBranchID  
  
  -- Added by Nimrod M. N. on 29-Aug-2018: Update t_AccountCustomer - AccountStatusID for Share Capital and Share Contribution  
  --DECLARE  
  -- @Cnt  SMALLINT = 1  
  --WHILE @Cnt <= 2  
  --BEGIN  
  -- UPDATE t_AccountCustomer  
  -- SET AccountStatusID = 'AA'  
  -- FROM t_Product P  
  -- WHERE P.ProductID = t_AccountCustomer.ProductID  
  --  AND OurBranchID = @OurBranchID  
  --  AND P.ProductID = CASE @Cnt WHEN 1 THEN 'SB06' WHEN 2 THEN 'SB07' END  
  --  AND AccountStatusID = 'AS'  
  --  AND dbo.f_GetClosingBalanceCust(OurBranchID, AccountID, @ProcessDate, 'D', 'T') >= dbo.f_GetSpecialConditionValue(BankID, CASE @Cnt WHEN 1 THEN '625' WHEN 2 THEN '921' END, P.ProductClassID, 'V')  
  -- SET @Cnt = @Cnt + 1  
  --END  
  
  --UPDATE t_Loan  
  --SET  LoanStatusID = 'F',  
  --  ClosedBy  = @OperatorID,  
  --  ClosedDate  = dbo.f_GetWorkingDate(@OurBranchID)  
  --FROM t_Loan  
  --INNER JOIN t_AccountCustomer  
  --ON  t_Loan.OurBranchID = t_AccountCustomer.OurBranchID  
  -- AND t_Loan.AccountID = t_AccountCustomer.AccountID  
  -- AND t_AccountCustomer.ClearBalance >= 0  
  --WHERE dbo.fn_GetOutstandingPrinciple (T_Loan.OurBranchID, t_loan.AccountID, T_loan.Loanseries) = 0  
  -- And ISNULL(dbo.fn_GetInterestReceivable(t_Loan.OurBranchID, t_Loan.AccountID, LoanSeries, 'LN_INT_RECV',@ProcessDate),0)   
  -- + ISNULL(dbo.fn_GetInterestReceivable(t_Loan.OurBranchID, t_Loan.AccountID, LoanSeries, 'LN_INT_RECV_SUS',@ProcessDate),0)  
  -- + ISNULL(dbo.fn_GetPenaltyReceivable(t_Loan.OurBranchID, t_Loan.AccountID, LoanSeries, 'LN_PEN_RECV',@ProcessDate),0)  
  -- + isNull(dbo.fn_GetPenaltyReceivable(t_Loan.OurBranchID,t_Loan.AccountID,LoanSeries,'LN_PEN_RECV_SUS',@ProcessDate),0) = 0  
  -- AND t_Loan.ClosedDate IS NULL  
  -- AND t_Loan.OurBranchID = @OurBranchID  
  -- AND t_Loan.LoanStatusID IN ( 'A','N')  
  
  ----- we should be considering the holidays of group change later on   
  --- this update not change the status of wrong next meeting date   
  /*  
  UPDATE t_Group   
  SET  NextMeetingDate = CASE WHEN MeetingFrequencyID ='D' THEN DATEADD(D,1,NextMeetingDate)  
         WHEN MeetingFrequencyID ='M' THEN DATEADD(M,1,NextMeetingDate)  
         WHEN MeetingFrequencyID ='W' THEN DATEADD(ww,1,NextMeetingDate)  
         WHEN MeetingFrequencyID ='B' THEN DATEADD(D,15,NextMeetingDate)  
         WHEN MeetingFrequencyID ='F' THEN DATEADD(ww,2,NextMeetingDate)  
         END     
  WHERE t_Group.OurBranchID = @OurBranchID And   
   t_Group.NextMeetingDate BETWEEN @EOD_Date AND @SOD_Date  
  */  
  
  UPDATE t_Group  
  SET NextMeetingDate = InstallmentDueDate  
  FROM t_Group,  
   (SELECT  v_GroupMemberScheme.OurBranchID,  
    v_GroupMemberScheme.GroupID,  
    MIN(t_LoanInstallment.InstallmentDueDate) InstallmentDueDate  
   FROM v_GroupMemberScheme,t_LoanInstallment,t_Loan  
   WHERE v_GroupMemberScheme.OurBranchID = t_LoanInstallment.OurBranchID  
    AND v_GroupMemberScheme.LoanAccountID = t_LoanInstallment.AccountID  
    AND t_LoanInstallment.OurBranchID = t_Loan.OurBranchID  
    AND t_LoanInstallment.AccountID = t_Loan.AccountID  
    AND t_LoanInstallment.LoanSeries = t_Loan.LoanSeries  
    AND v_GroupMemberScheme.OurBranchID = @OurBranchID  
    AND t_Loan.LoanStatusID = 'A'   
    AND t_LoanInstallment.InstallmentDueDate > @SOD_Date  
   GROUP BY v_GroupMemberScheme.OurBranchID,  
    v_GroupMemberScheme.GroupID) LoanInstallment  
  WHERE t_Group.OurBranchID = LoanInstallment.OurBranchID  
   AND t_Group.GroupID = LoanInstallment.GroupID  
   AND t_Group.OurBranchID = @OurBranchID  
   AND t_Group.NextMeetingDate BETWEEN @EOD_Date AND @SOD_Date  
  
  UPDATE t_GRTDetail  
  SET  GRTStatusID  = 'E'  
  WHERE OurBranchID  = @OurBranchID  
   AND GRTExpiryDate <= @ProcessDate  
   AND GRTStatusID  = 'A'  
  
  IF NOT EXISTS(SELECT 1   
       FROM  t_AlertReportDetail(NOLOCK)   
      WHERE  BankID = dbo.f_GetBankID(@OurBranchID)   
      AND ModuleID = (SELECT RptAlertID   
          FROM   t_AlertReport  
          WHERE 1 = CASE WHEN ReportFrequencyID = 'D' THEN 1   
                WHEN ReportFrequencyID = 'W' AND WeekDayID = DATEPART(dw,GETDATE()) THEN 1  
                WHEN ReportFrequencyID = 'B' AND (FDay1   = DATEPART(dd,GETDATE())   
                       OR FDay2   = DATEPART(dd,GETDATE())) THEN 1  
                WHEN ReportFrequencyID = 'M' AND MDay   = DATEPART(dd,GETDATE()) THEN 1  
              END)   )  
  BEGIN  
   INSERT INTO t_AlertReportDetail  
   (  
    BankID,  
    ModuleID,  
    Date,  
    EmailIDs,  
    StatusID  
   )  
   SELECT dbo.f_getBankID(@OurBranchID),  
     RptAlertID,  
     CONVERT(Varchar(10),GETDATE() ,126),  
     EmailIDs,'P'  
   FROM t_AlertReport(NOLOCK)  
   WHERE 1 = CASE WHEN ReportFrequencyID = 'D' THEN 1   
         WHEN ReportFrequencyID = 'W' AND WeekDayID = DATEPART(dw,GETDATE()) THEN 1  
         WHEN ReportFrequencyID = 'B' AND (FDay1   = DATEPART(dd,GETDATE())   
                OR FDay2   = DATEPART(dd,GETDATE())) THEN 1  
         WHEN ReportFrequencyID = 'M' AND MDay   = DATEPART(dd,GETDATE()) THEN 1  
       END  
  END  
  
  -- Reset transaction serial no.   
  IF EXISTS (SELECT 1  
   FROM t_SystemBankParameter(NOLOCK)  
   WHERE BankID = dbo.f_GetBankID(@OurBranchID)  
    AND SysParamID = 24  
    AND ParamValue = 1)  
  BEGIN  
   UPDATE t_SystemTrxSerialNo  
   SET SerialNo = 0  
   WHERE OurBranchID  = @OurBranchID  
    AND IsTrxSerialNo = 1  
  END  
  
  --CREDIT / DEBIT BALANCE TRANSFER   
  -----EXEC ch_CrDrControlAcs @OurBranchID=@OurBranchID,@TrxDate=@ProcessDate,@IsPost=1  
  
  EXEC p_UpdateMonthlyMinimumbalance @OurBranchID   
  
  /*  
 ---MAN ENjuki  
 --============================  
  
 UPDATE t_AccountCustomer SET MonthminimumBal = ISNULL(ClearBalance,0)   
 FROM t_Product(nolock) WHERE OurBranchID = @OurBranchID   
  And t_AccountCustomer.ProductID=t_Product.ProductID  
  And ProducttypeID in ('CS','SB','CA','DP')  
  And AccountStatusID<>'AC'  
  And MonthminimumBal IS NULL  
  
 UPDATE t_AccountCustomer  
  SET MonthminimumBal = CASE WHEN (ISNULL(ClearBalance,0))<=ISNULL(MonthminimumBal,0) THEN ClearBalance   
  ELSE MonthminimumBal END  
 FROM t_Product(nolock)  
 WHERE OurBranchID = @OurBranchID   
  And t_AccountCustomer.ProductID=t_Product.ProductID  
  And ProducttypeID in ('CS','SB','CA','DP')  
  And AccountStatusID<>'AC'  
  
 --For 1st Day of the Month Deposit  
 IF DATEPART(DAY,@ProcessDate)=1  
 BEGIN  
 UPDATE t_AccountCustomer  
 SET MonthminimumBal= ClearBalance   
 From t_Product(nolock)  
 Where OurBranchID = @OurBranchID   
  And t_AccountCustomer.ProductID=t_Product.ProductID  
  And ProducttypeID in ('CS','SB','CA','DP')  
  And AccountStatusID<>'AC'  
 END  
  
 Select @NextWorkingDate = dbo.f_GetNextWorkingDate(@OurBranchID,@ProcessDate)  
  
 IF Month(@ProcessDate) <> Month(@NextWorkingDate)  
 BEGIN  
  UPDATE t_AccountCustomer  
  SET MonthminimumBal = ClearBalance ,MonthlyDebits=0  
  From t_Product(nolock)  
  Where OurBranchID = @OurBranchID   
   And t_AccountCustomer.ProductID=t_Product.ProductID  
   And ProducttypeID in ('CS','SB','CA','DP')  
   And AccountStatusID <>'AC'  
 END  
 */  
  
  IF @ErrorNo > 0  
   GOTO EndStatement  
   
    
  --EXEC ps_RevaluateForeignAccountsCustom  
  --  @OurBranchID =  @OurBranchID,  
  --  @ProcessDate =  @ProcessDate,  
  --  @OperatorID  = 'Sys',  
  --  @IsPost   = 1,  
  --  @ErrorNo  = @ErrorNo OUTPUT  
      
  --IF @ErrorNo > 0  
  -- GOTO EndStatement   
    
  SET @Year = YEAR(@ProcessDate)  
  EXEC ch_GLBalanceAndUpdate  
    @OurBranchID =  @OurBranchID,  
    @Year = @Year,  
    @IsUpdate = 1,  
    @OperatorID = 'SYS'  
         
  EXEC ch_AccountBalanceAndUpdate  
    @OurBranchID =  @OurBranchID,  
    @ProductID=NULL,  
    @Year = @Year,  
    @IsUpdate = 1,  
    @OperatorID = 'SYS'   
      
  --- Post Position Account Revaluation  
  --IF @OurBranchID = dbo.f_GetHOBranchID(dbo.f_GetBankID(@OurBranchID))  
  --BEGIN  
  -- EXEC ps_RevaluateForwardContractAccounts  
  --  @OurBranchID =  @OurBranchID,  
  --  @ProcessDate =  @ProcessDate,  
  --  @IsPost   = 1,  
  --  @ErrorNo  = @ErrorNo OUTPUT  
      
  -- IF @ErrorNo > 0  
  --  GOTO EndStatement        
  --END  
  
  --IF @OurBranchID = dbo.f_GetHOBranchID(dbo.f_GetBankID(@OurBranchID))  
  --BEGIN  
  -- EXEC ps_RevaluateForeignPositionAccountsCustom  
  --  @OurBranchID =  @OurBranchID,  
  --  @ProcessDate =  @ProcessDate,  
  --  @IsPost   = 1,  
  --  @ErrorNo  = @ErrorNo OUTPUT  
      
  -- IF @ErrorNo > 0  
  --  GOTO EndStatement        
  --END    
    
    
------------------Translation Account Balance knock off-------------------------  
----Do this only after EOM: from CUR_TRANS_AC to CUR_FOREX_AC(paramount)--Danson  
  
--select @ProcessDate,@NextWorkingDate,@ProcessDate,@NextWorkingDate  
 --IF Month(@ProcessDate) = Month(@NextWorkingDate) And Year(@ProcessDate) = Year(@NextWorkingDate)   
 --BEGIN  
 -- Print 'Do Not Do Anything'  
 --END  
 --ELSE  
 --BEGIN  
  
 -- EXEC [dbo].[p_SweepTranslationBalance]  
 --  @OurBranchID = @OurBranchID,  
 --  @ProcessID  = 'EOD',  
 --  @ProcessDate = @ProcessDate,  
 --  @ErrorNo  = @ErrorNo OUTPUT  
   
  
 -- IF @ErrorNo > 1  
 --  GOTO EndStatement     
    
 --END  
   
   
--------------------------------------------------------------------------------  
--Uploaded Gateway Data Needs Archival  
 Insert Into t_GatewayLoanRecoveryHistory(BankID,trxBatchID,OurBranchID,FormatID,AccountID,Amount,WorkingDate,FilePath,RecoveryStatus,  
    RecoveryDate,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,RecoveredAmount,FosaContribution,BosaContribution  
 )  
 Select BankID,trxBatchID,OurBranchID,FormatID,AccountID,Amount,WorkingDate,FilePath,RecoveryStatus,RecoveryDate,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,  
    RecoveredAmount,FosaContribution,BosaContribution  
 From t_GatewayLoanRecovery  
 Where OurBranchID = @OurBranchID And WorkingDate <= @ProcessDate  
  
 Delete from t_GatewayLoanRecovery Where OurBranchID = @OurBranchID And WorkingDate <= @ProcessDate  
   
  --INSERT INTO t_DailyTransaction  
  -- (OurBranchID,TrxDate,TrxTypeID,ModuleID,TrxAmount,LocalAmount,Amount,CreatedOn)       
  --SELECT  
  -- OurBranchID,TrxDate,TrxTypeID,ModuleID,ISNULL(SUM(TrxAmount),0),ISNULL(SUM(LocalAmount),0),ISNULL(SUM(Amount),0),GetDate()   
  -- FROM t_AccountTrx   
  -- WHERE OurBranchID = @OurBranchID AND TrxDate = @ProcessDate  
  -- GROUP BY OurBranchID,TrxDate,ModuleID,TrxTypeID     
  ------Massai----------------------  
  --Faulu Exports  
   --Exec p_FauluReportExtracts  @OurBranchID ,@ProcessDate  
  
EndStatement:  
  
  IF @@Error > 0 OR @ErrorNo > 0  
  BEGIN   
   ROLLBACK TRANSACTION   
   --SET @ErrorNo = 51023  
     
   UPDATE t_SystemBranchStatus    
   SET IsDayOpen    = 1,  
    IsReadyForClosing = 1  
   WHERE OurBranchID = @OurBranchID  
   RETURN  
  END  
  ELSE   
   BEGIN   
   UPDATE t_SystemBranchStatus    
   SET EODDate = @ProcessDate  
   WHERE OurBranchID = @OurBranchID  
     
   --Added by mosh to update the field since there is no EOM process now  
  
   IF Month(@NextWorkingDate) = Month(@ProcessDate)+1   And Year(@ProcessDate) = Year(@NextWorkingDate)   
   BEGIN  
    --SAME YEAR  
    UPDATE t_SystemBranchStatus    
    SET EOMDate = @ProcessDate  
    WHERE OurBranchID = @OurBranchID      
   END  
     
   ELSE  
   IF Month(@NextWorkingDate) = Case Month(@ProcessDate)+1 When 13 Then 1  
                 When 14 Then 2  
                 When 15 Then 3  
                 When 16 Then 4  
                 When 17 Then 5  
                 When 18 Then 6  
                 When 19 Then 7  
                 When 20 Then 8  
                 When 21 Then 9  
                 When 22 Then 10  
                 When 23 Then 11  
                 When 24 Then 12  
            End   
   And Year(@NextWorkingDate) = Year(@ProcessDate)+1    
   BEGIN  
    --YEAR CHANGE (31 Dec 2015 - 01 Jan 2016)   
    UPDATE t_SystemBranchStatus    
    SET EOMDate = @ProcessDate  
    WHERE OurBranchID = @OurBranchID                                                                                                                                                                                      
  
   END  
     
   --finished adding  
        
      
   -- Temporaryly updating/deleting these till we get the actual cause of the issue  
   UPDATE t_AccountCustomer   
   SET UnSupervisedCredits = 0,  
    UnSupervisedDebits = 0   
   WHERE OurBranchID = @OurBranchID  
     
   DELETE FROM t_SystemRecordLocks   
   WHERE OurBranchID = @OurBranchID  
     
   -- END of Temporaryly updating/deleting these till we get the actual cause of the issue  
     
   COMMIT TRANSACTION   
    
   EXEC ps_ExecuteProcessException @OurBranchID = @OurBranchID,  
           @ProcessDate = @ProcessDate,  
           @OperatorID  = @OperatorID  
    
   SET @ErrorNo = 0  
     
   RETURN  
   END  
 SET NOCOUNT OFF  
END  
  
  
  
  
  