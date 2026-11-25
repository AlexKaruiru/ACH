  
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
  
  
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
  
  
  
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
  
  
  
-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------  
--IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE NAME='ps_ClearEffects')  
--BEGIN  
  
--exec ps_ClearEffects '001','clr','16 sep 2015'  
CREATE    PROCEDURE dbo.ps_ClearEffects  
(  
 @OurBranchID BranchID,  
 @ProcessID  NVARCHAR(20),  
 @ProcessDate SMALLDATETIME,  
 @ErrorNo  INT = 0 OUTPUT,  
 @ErrorParams XML = NULL OUTPUT  
)  
   
AS  
BEGIN  
 SET NOCOUNT ON  
  
 DECLARE @SOD_Date SMALLDATETIME,  
   @EOD_Date SMALLDATETIME,  
   @CE_Date SMALLDATETIME,  
  
   @AccountID AccountID,  
   @Amount  Amount,  
   @TrxRowID BIGINT   
   
 SELECT    
  @SOD_Date = SODDate,  
  @EOD_Date = EODDate,  
  @CE_Date = CEDate  
 FROM t_SystemBranchStatus  
 WHERE OurBranchID = @OurBranchID  
   
 IF (@EOD_Date >= @ProcessDate) OR (@ProcessDate < @SOD_Date)  
 BEGIN  
  SET @ErrorNo = 600003  
  RETURN  
 END  
   
 IF (@SOD_Date <> @ProcessDate)  
 BEGIN  
  SET @ErrorNo = 600004  
   RETURN  
 END  
  
 BEGIN TRANSACTION  
  
 DECLARE Accounts CURSOR FAST_FORWARD FOR  
  SELECT  
   TrxRowID,AccountID,Amount  
  FROM t_TrxValuedated  
  WHERE OurBranchID = @OurBranchID AND ValueDate <= @ProcessDate  
   AND IsCleared = 0 AND ClearedDate IS NULL AND ClearingStatusID NOT IN ('C','D')  
  
 OPEN Accounts  
   
 FETCH NEXT FROM Accounts INTO @TrxRowID,@AccountID,@Amount  
   
 WHILE @@FETCH_STATUS = 0  
 BEGIN  
  IF NOT EXISTS(SELECT OurBranchID FROM t_AccountCustomer WHERE OurBranchID = @OurBranchID  
   AND AccountID = @AccountID AND UnClearBalance >= @Amount)  
  BEGIN  
   FETCH NEXT FROM Accounts INTO @TrxRowID,@AccountID,@Amount  
   --SELECT @OurBranchID, @AccountID, @Amount  
   CONTINUE  
  END  
    
  UPDATE t_AccountCustomer SET  
   UnClearBalance = UnClearBalance - @Amount,  
   ClearBalance = ClearBalance + @Amount  
  WHERE OurBranchID = @OurBranchID AND AccountID = @AccountID  
     
  UPDATE t_TrxValuedated SET  
   IsCleared   = 1,  
   ClearedDate   = @ProcessDate,  
   ClearingStatusID = 'C'  
  WHERE OurBranchID = @OurBranchID AND AccountID = @AccountID AND TrxRowID = @TrxRowID  
    
  FETCH NEXT FROM Accounts INTO @TrxRowID,@AccountID,@Amount  
 END  
 CLOSE Accounts  
 DEALLOCATE Accounts    
  
 IF @@Error = 0  
 BEGIN  
  COMMIT TRANSACTION  
  UPDATE t_SystemBranchStatus SET  
   CEDate = @ProcessDate  
  WHERE OurBranchID = @OurBranchID  
  
  EXEC ps_ExecuteProcessException @OurBranchID = @OurBranchID,  
          @ProcessDate = @ProcessDate,  
          @OperatorID  = 'SYS'  
  SET @ErrorNo = 0  
 END  
 ELSE   
 BEGIN   
  ROLLBACK TRANSACTION   
  SET @ErrorNo = 51023  
 END  
 SET NOCOUNT OFF  
END  
  
--END  
  