CREATE   PROCEDURE [dbo].[p_ProcessIncomingTxns]  
(  
 @TxnType VARCHAR(25),  
 @ColumnID VARCHAR(Max),  
 @ReturnCodeID CHAR(4)='00',  
 @TrxBranchID BranchID,  
 @EscalationNotes Description,  
 @OperatorID OperatorID,  
 @SupervisionFlag VARCHAR(5)  
)   
AS  
BEGIN  
 --RETURN  
 SET NOCOUNT ON  
 DECLARE  
   @ClearingTrxBranchID BranchID,       
   @OurBranchID BranchID,      
   @AccountTypeID CHAR(1),      
   @AccountID AccountID,      
   @ProductID ProductID,      
   @ModuleID SMALLINT,      
   @TrxCodeID TINYINT,    
   @TrxTypeID CHAR(2),       
   @NewTrxTypeID CHAR(2),      
   @TrxDate SMALLDATETIME,      
   @ValueDate SMALLDATETIME,      
   @Amount Amount,      
   @TrxCurrencyID CurrencyID,      
   @InstrumentTypeID CHAR(1),           
   @ChequeID INT,           
   @ChequeDate SMALLDATETIME = Null,        
   @ReferenceNo NVARCHAR(15) = Null,      
   @Remarks Remarks = Null,      
   @TrxDescriptionID NVARCHAR(6),         
   @TrxDescription DESCRIPTION,      
   @MainGLID AccountID,      
   @ContraGLID AccountID = Null,        
   @TrxFlagID SystemSubID = '',   
   @OriginalReturnCode     CHAR(2),    
   @ImageID VARCHAR(30),      
   @TrxPrinted TINYINT = 0,      
   @ChequeDigit CHAR(1)=0,      
   @VoucherCode CHAR(2)=0,         
   @Commission Amount=0,      
   @TheirCommission Amount=0,      
   @VATPINNo NVARCHAR(12)=null,      
   @VATPAYType NVARCHAR(10)=null,      
   @VATSerialNo NVARCHAR(7)=null,      
   @VATPAYEMonth NVARCHAR(6)=null,      
   @VATPAYECommission Amount=0,      
   @BankID BankID,      
   @BranchID BranchID,      
   @DrawerOrPayeeAccountID AccountID,      
   @DrawerOrPayee Names,      
   @CreatedBy OperatorID,      
   @NewRecord TINYINT,      
   @TrxRowID BIGINT,      
   @ForwardRemark VARCHAR(MAX)=null ,  
   @WorkingDate datetime ,  
   @ClearedBy varchar(30),  
   @ClearedOn datetime,  
   @SupervisedBy varchar(30) = NULL,  
   @Status   varchar(50),  
   @ReturnedTempID CHAR(4),  
   @OldFlagID SystemSubID ='',  
   @Verified Bit,  
   @FileName Varchar(50),  
   @ExcessOverLimitFlag SystemSubID,  
   @HeadOfficeBranch BranchID  
  
 SELECT TOP 1 @HeadOfficeBranch=(OurBranchID) FROM t_SystemBranchSetting WHERE IsHeadOffice=1  
  
  
   
 --To work on this later - Noted on Kamunya 30 Dec  
 IF @TxnType = 'Supervise'  
 BEGIN  
  SELECT @TxnType='Authorize'  
 END  
   
 IF @SupervisionFlag='null' OR @SupervisionFlag IS NULL  
 BEGIN  
  SET @SupervisionFlag=''  
 END  
  
 SELECT @WorkingDate = dbo.f_GetWorkingDate(@TrxBranchID)  
  
 DECLARE @OurBankID dbo.BankID,  
   @ProductTypeID VARCHAR(5) ,  
   @LocalCurrencyID CurrencyID,  
   @LocalAmount Amount,      
   @ExchangeRate Rate,  
   @MeanRate Rate,      
   @Profit Amount,      
   @RoundingID SystemSubID,  
   @TrxAmount Amount      
  
  
 DECLARE @TrxBatchID VARCHAR(10),  
   @SerialID VARCHAR(10),  
   @ErrorNo VARCHAR(50)  
   
  
  
  
 IF EXISTS(SELECT 1 FROM t_Transaction WHERE Try_parse(ReferenceNo AS bigint) = @ColumnID AND TrxTypeID IN ('IC','ID'))  
 BEGIN  
  IF EXISTS (SELECT 1 FROM t_IncomingTransactions WHERE ColumnID = @ColumnID AND isNull(Verified,0) = 0)  
   BEGIN  
    UPDATE t_IncomingTransactions   
    SET  Returned = 1, Paid = 1, isProcessed = 1, Verified = 1, VerifiedBy = @OperatorID,   
      VerifiedOn = @WorkingDate, ClearedDate = @WorkingDate, TrxBatchID = @TrxBatchID,  
      SupervisedByOne = @OperatorID, SupervisedOnOne = getDate(),SupervisionFlag = ''  
    WHERE ColumnID=@ColumnID   
   END  
  ELSE  
   BEGIN  
    UPDATE t_IncomingTransactions   
    SET  Returned = 1, Paid = 1, isProcessed = 1, ClearedBy = @OperatorID,   
      ClearedOn = @WorkingDate, ClearedDate = @WorkingDate, TrxBatchID = @TrxBatchID,  
      SupervisedByOne = @OperatorID, SupervisedOnOne = getDate(),SupervisionFlag = ''  
    WHERE ColumnID=@ColumnID   
   END  
 END  
  
 IF isNull(@ReturnCodeID,'00') <> '00'  
 BEGIN  
  UPDATE t_IncomingTransactions SET ReturnedTempID = @ReturnCodeID WHERE ColumnID = @ColumnID  
 END  
 ELSE IF isNull(@ReturnCodeID,'') = ''  
 BEGIN  
  UPDATE t_IncomingTransactions SET ReturnedTempID = '00' WHERE ColumnID = @ColumnID  
 END  
    
   
 SELECT @ClearingTrxBranchID = TrxBranchID,  
   @OurBranchID = OurBranchID,  
   @AccountTypeID = AccountType,  
   @AccountID = AccountID,  
   @ProductID = ProductID,   
   @ModuleID = ModuleID,   
   @TrxCodeID = 0,   
   @TrxTypeID = TrxType,   
   @TrxDate = [Date],   
   @ValueDate = [Date] ,   
   @Amount = Amount,   
   @TrxCurrencyID = CurrencyID,   
   @InstrumentTypeID = InstrumentTypeID,  
   @ChequeID = ChequeID,  
   @ChequeDate = [Date],  
   @ReferenceNo = '' ,  
   @Remarks = '',  
   @TrxDescriptionID = TrxDescriptionID,   
   @TrxDescription = Description,  
   @MainGLID = MainGLID,  
   --@ContraGLID = ContraGLID,  
   @TrxFlagID = @SupervisionFlag,  
   --@ImageID = 'Assign'--Ask ,  
   @TrxPrinted = 0,  
   @ChequeDigit = ChequeDigit,  
   @VoucherCode = VoucherCode,  
   @OriginalReturnCode = ReturnCode,  
   @Commission = 0,  
   @TheirCommission = 0,  
   @VATPINNo = 0,  
   @VATPAYType = 0,  
   @VATSerialNo = 0,  
   @VATPAYEMonth = 0,  
   @VATPAYECommission = 0,  
   @BankID = BankID,  
   @BranchID = BranchID,  
   @DrawerOrPayeeAccountID = TheirAccount,  
   @DrawerOrPayee = DrawerOrPayee,  
   @CreatedBy = VerifiedBy,  
   @NewRecord = 1,  
   @TrxRowID = @ColumnID,  
   @ForwardRemark = '' ,  
   @ClearedBy =   ClearedBy,  
   @ClearedOn = ClearedOn,  
   @Status  = [Status],  
   @ReturnedTempID = isNull(ReturnedTempID,'00'),  
   @OldFlagID = SupervisionFlag,  
   @Verified = isNull(Verified,0),  
   @FileName = FileName,  
   @ExcessOverLimitFlag = isNull(Policy2,'')  
 FROM t_IncomingTransactions   
 WHERE ColumnID = @ColumnID  
  
   
  
 SELECT @ReferenceNo = @ColumnID  
  
 --Added to ensure that an unpaid inwards do not get unpaid again--SBMKe - 22 Jan 2018  
 --select @OriginalReturnCode  
 IF @TxnType ='Unpay' AND @OriginalReturnCode NOT IN ('00','17')  
 BEGIN  
  RETURN  
 END  
  
  
 IF isNull(@ReturnedTempID,'') = ''  
 BEGIN  
  SELECT @ReturnedTempID = '00'  
 END  
  
 EXEC p_GetUniqueClearingImageID @ImageID OUTPUT  
  
  
 IF isNull(@ReturnedTempID,'') = ''  
 BEGIN  
  IF isNull(@ReturnCodeID,'00') <> '00'  
   BEGIN  
    SELECT @ReturnedTempID = @ReturnCodeID  
   END  
  ELSE  
   BEGIN  
    SELECT @ReturnedTempID = '00'  
   END  
 END  
  
  
 IF EXISTS(SELECT 1 FROM t_SystemBankSetting Where ShortName <> 'FTB')  
 BEGIN  
  IF @OriginalReturnCode NOT IN ('00','17') AND  @TrxTypeID ='ID' --AND @AccountTypeID = 'C'    
  BEGIN    
   IF @OriginalReturnCode = '63'    
    BEGIN    
     SELECT @TrxDescriptionID = '083'    
    END     
   ELSE IF @OriginalReturnCode  = '62'    
    BEGIN    
     SELECT @TrxDescriptionID = '084'    
    END    
   ELSE IF @OriginalReturnCode  = '55'    
    BEGIN    
     SELECT @TrxDescriptionID = '085'    
    END     
   ELSE    
    BEGIN    
     SELECT @TrxDescriptionID = '004'    
    END   
  END  
  IF @TrxTypeID IN ('IC') AND @AccountTypeID = 'C' AND @OriginalReturnCode NOT IN ('00','17')  
   BEGIN  
    SET @TrxDescriptionID = '089'   
   END   
  IF @TrxTypeID IN ('IC') AND @AccountTypeID = 'C' AND @OriginalReturnCode IN ('00','17')  
   BEGIN  
    SET @TrxDescriptionID = '089'   
   END   
  --ELSE IF @TrxTypeID IN ('ID') AND @AccountTypeID = 'G'   
  -- BEGIN  
  --  SELECT @TrxDescriptionID = '004'  
  -- END   
  ELSE IF @TrxTypeID IN ('IC') AND @AccountTypeID = 'G'   
   BEGIN  
    SELECT @TrxDescriptionID = '089'  
   END  
 END  
  
 DECLARE @ChequeStatusID VARCHAR(5)   
 IF @AccountID <> dbo.f_GetCurrencyBranchGLAccountID(@TrxBranchID, ISNULL(@TrxCurrencyID, 'TZS'), 'ACP_CLR_SUSP_AC_CHQ') AND @OriginalReturnCode IN ('00','17') AND @TrxTypeID = 'ID' AND @ReturnedTempID<>'00'  
 BEGIN  
  EXEC p_ValidChequeSeries  
   @OurBranchID  = @OurBranchID,  
   @AccountTypeID  = @AccountTypeID,  
   @AccountID   = @AccountID,  
   @ChequeID   = @ChequeID,  
   @ChequeStatusID  = @ChequeStatusID Output,  
   @ErrorNo   = @ErrorNo OutPut,  
   @IsTransfer   = 0  
  
  IF @ErrorNo <> ''  
  BEGIN  
     
   RETURN  
  END  
 END  
 IF @VoucherCode ='40'       
 BEGIN  
  SET @ChequeID = 999999  
 END  
   
 IF @OriginalReturnCode ='17'      
 BEGIN      
  SET @TrxDescription = 'Represented Cheque Chq No. ' + CAST(@ChequeID as VARCHAR) + ' From Bnk ' + @BankID      
 END  
 SELECT @OurBankID = BankID FROM t_SystemBankSetting (NOLOCK)  
 --SELECT @TrxBranchID = @ClearingTrxBranchID  
 --SELECT @ProductID,@AccountTypeID, '1', @AccountID  
   
 IF(@AccountTypeID = 'C')      
 BEGIN  
  SELECT @MainGLID = dbo.f_GetGLInterfaceAccountID1(@OurBankID, @ProductID,'CONTROL_AC')    
  IF isNull(@ContraGLID,'') = ''  
  BEGIN  
   IF @ReturnCodeID IN ('00','17')  
    BEGIN  
     SELECT @ContraGLID = dbo.f_GetCurrencyBranchGLAccountID(@OurBranchID, @TrxCurrencyID, 'CUR_CLR_AC_IN')    
    END  
   ELSE  
    BEGIN  
     IF @TrxTypeID ='ID'  
      BEGIN  
       SELECT @ContraGLID = dbo.f_GetCurrencyBranchGLAccountID(@OurBranchID, @TrxCurrencyID, 'CUR_CLR_AC_IN')   
      END  
     ELSE  
      BEGIN  
       SELECT @ContraGLID = dbo.f_GetCurrencyBranchGLAccountID(@OurBranchID, @TrxCurrencyID, 'CUR_CLR_AC_IN')  
      END  
    END  
  END    
 END    
   
 --SELECT @ProductID,@AccountTypeID  
 IF (@ProductID IS NULL OR @ProductID = '')       
 BEGIN  
  IF @AccountTypeID = 'C'      
  BEGIN  
   SELECT @MainGLID = dbo.f_GetGLInterfaceAccountID1(@OurBankID, @ProductID,'CONTROL_AC')  
   IF isNull(@ContraGLID,'') = ''  
   BEGIN  
    IF @ReturnCodeID IN ('00','17')  
     BEGIN  
      SELECT @ContraGLID = dbo.f_GetCurrencyBranchGLAccountID(@OurBranchID, @TrxCurrencyID, 'CUR_CLR_AC_IN')    
     END  
    ELSE  
     BEGIN  
      IF @TrxTypeID ='ID'  
       BEGIN  
        SELECT @ContraGLID = dbo.f_GetCurrencyBranchGLAccountID(@OurBranchID, @TrxCurrencyID, 'CUR_CLR_AC')   
       END  
      ELSE  
       BEGIN  
        SELECT @ContraGLID = dbo.f_GetCurrencyBranchGLAccountID(@OurBranchID, @TrxCurrencyID, 'CUR_CLR_AC_IN')  
       END  
     END    
   END  
  END      
  IF @AccountTypeID = 'G'      
  BEGIN  
   SET @ProductID = 'GL'    
   SELECT @MainGLID = @AccountID  
   IF isNull(@ContraGLID,'') = ''  
   BEGIN  
    IF @ReturnCodeID IN ('00','17')  
     BEGIN  
      SELECT @ContraGLID = dbo.f_GetCurrencyBranchGLAccountID(@OurBranchID, @TrxCurrencyID, 'CUR_CLR_AC_IN')    
     END  
    ELSE  
     BEGIN  
      IF @TrxTypeID ='ID'  
       BEGIN  
        SELECT @ContraGLID = dbo.f_GetCurrencyBranchGLAccountID(@OurBranchID, @TrxCurrencyID, 'CUR_CLR_AC')   
       END  
      ELSE  
       BEGIN  
        --SELECT @ContraGLID = dbo.f_GetCurrencyBranchGLAccountID(@OurBranchID, @TrxCurrencyID, 'CUR_CLR_AC_IN')  
        SELECT @ContraGLID = dbo.f_GetCurrencyBranchGLAccountID(@OurBranchID, @TrxCurrencyID, 'CUR_CLR_AC')  
       END  
     END      
   END    
  END      
 END  
  
   
 IF @ProductID = 'GL'  
 BEGIN      
  IF isNull(@ContraGLID,'') = ''  
  BEGIN  
   IF @ReturnCodeID IN ('00','17')  
     BEGIN  
      SELECT @ContraGLID = dbo.f_GetCurrencyBranchGLAccountID(@OurBranchID, @TrxCurrencyID, 'CUR_CLR_AC_IN')    
     END  
    ELSE  
     BEGIN  
      IF @TrxTypeID ='ID'  
       BEGIN  
        SELECT @ContraGLID = dbo.f_GetCurrencyBranchGLAccountID(@OurBranchID, @TrxCurrencyID, 'CUR_CLR_AC')   
       END  
      ELSE  
       BEGIN  
        --SELECT @ContraGLID = dbo.f_GetCurrencyBranchGLAccountID(@OurBranchID, @TrxCurrencyID, 'CUR_CLR_AC_IN')  
        SELECT @ContraGLID = dbo.f_GetCurrencyBranchGLAccountID(@OurBranchID, @TrxCurrencyID, 'CUR_CLR_AC')  
       END  
     END   
   SELECT @MainGLID = @AccountID  
  END  
 END  
   
 IF @OriginalReturnCode NOT IN ('00','17') AND @TrxTypeID = 'ID' AND @VoucherCode <> '40'  
 BEGIN      
  SELECT @TrxDescription = 'Unpaid Chq No. : ' + cast (@ChequeID as varchar) + ' Rsn : ' +  DBO.f_CRB_ReturnCodeDescriptions('ReturnCodeID',@ReturnCodeID,'T')   
 END  
 ELSE IF @OriginalReturnCode NOT IN ('00','17') AND @TrxTypeID = 'ID' AND @VoucherCode ='40'  
 BEGIN  
  SELECT @TrxDescription = 'Unpaid DD Rsn : ' +  DBO.f_CRB_ReturnCodeDescriptions('ReturnCodeID',@ReturnCodeID,'T')   
 END  
 ELSE IF @OriginalReturnCode NOT IN ('00','17') AND @TrxTypeID = 'IC'   
 BEGIN  
  SELECT @TrxDescription = 'Unpaid IC Rsn : ' +  DBO.f_CRB_ReturnCodeDescriptions('ReturnCodeID',@ReturnCodeID,'T')   
 END  
  
  
  
 SELECT @ChequeID = isNull(@ChequeID,0)  
 SELECT @ModuleID = CASE WHEN @TrxTypeID = 'ID' THEN 3050 ELSE 3040 END  
 SELECT @LocalCurrencyID = dbo.f_GetLocalCurrencyID(@OurBranchID)    
  
 --SELECT  @MainGLID   
 --RETURN  
  
   
 IF @TrxCurrencyID = @LocalCurrencyID      
  BEGIN      
   SELECT  
    @TrxAmount  = @Amount,      
    @LocalAmount = @Amount,      
    @ExchangeRate = 1       
  END      
 ELSE      
  BEGIN      
   --SELECT @MeanRate = MeanRate, @RoundingID = RoundingID       
   --FROM t_CurrencyRate (NOLOCK)     
   --WHERE OurBranchID = @OurBranchID      
   --  AND CurrencyID = @TrxCurrencyID      
   --  AND RateTypeID = 'REV'  
   SELECT  
     @TrxAmount  = @Amount,       
     @ExchangeRate = dbo.f_GetCurrencyRate(@OurBranchID, @TrxCurrencyID, 'REV', 'M'),     
     @MeanRate = @ExchangeRate,  
     @LocalAmount = @Amount * @MeanRate    
  END  
  
    
      
  
  
  
 IF @TxnType='Pay'  
 BEGIN  
 IF isNull(@Verified,0) = 0  
  BEGIN  
   UPDATE t_IncomingTransactions   
   SET Paid = 0, Escalated = 0,IsProcessed = 0, Verified = 1, VerifiedBy = @OperatorID, VerifiedOn = @WorkingDate  
   WHERE ColumnID = @TrxRowID   
  END  
 ELSE  
  BEGIN  
   UPDATE t_IncomingTransactions   
    SET  Paid = 0, Escalated = 0, isProcessed = 0, SupervisedByOne = @OperatorID,   
      SupervisedOnOne = getDate(), ClearedDate = @WorkingDate, TrxBatchID = @TrxBatchID,   
      SupervisionFlag = 'X'   
    WHERE ColumnID=@TrxRowID   
      
   UPDATE t_IncomingTransactions   
   SET TrxBranchID = @HeadOfficeBranch  
   WHERE ColumnID = @TrxRowID AND TrxType = 'IC'  
  
   UPDATE t_IncomingTransactions   
   SET TrxBranchID = @OurBranchID  
   WHERE ColumnID = @TrxRowID AND TrxType = 'ID'  
  
   UPDATE t_IncomingTransactions   
   SET TrxBranchID = @OurBranchID  
   WHERE ColumnID = @TrxRowID AND AccountID IN (dbo.f_GetCurrencyBranchGLAccountID(@TrxBranchID, @TrxCurrencyID, 'ACP_CLR_SUSP_AC_EFT'),  
   dbo.f_GetCurrencyBranchGLAccountID(@TrxBranchID, @TrxCurrencyID, 'ACP_CLR_SUSP_AC_CHQ'))  
    
  Select @ExcessOverLimitFlag,@OurBranchID,@TrxRowID  
   IF (@ExcessOverLimitFlag = 'P' OR @ExcessOverLimitFlag = 'S')  
   BEGIN  
    IF isNull(@Verified,0) = 0  
    BEGIN  
     SET @SupervisedBy = @OperatorID  
     IF NOT EXISTS(SELECT 1 FROM t_Transaction WHERE Try_parse(ReferenceNo AS bigint) = @TrxRowID AND TrxTypeID IN ('IC','ID'))  
     BEGIN  
      IF (@ExcessOverLimitFlag = 'P')  
       BEGIN  
        SELECT @ReturnCodeID = '51'  
       END  
      ELSE  
       BEGIN  
        SELECT @ReturnCodeID = '75'  
       END  
        
      SET @TrxDate = dbo.f_GetWorkingDate(@OurBranchID)    
      SET @TrxCurrencyID = ISNULL( @TrxCurrencyID, 'TZS')  
      BEGIN TRY   
        
       --BEGIN TRAN UNPSTP  
       EXEC p_AddIncomingTrx  
        @TrxBranchID = @TrxBranchID,--@ClearingTrxBranchID,   
        @TrxBatchID = @TrxBatchID  OUTPUT,  
        @SerialID = @SerialID  OUTPUT,  
        @OurBranchID = @OurBranchID,  
        @AccountTypeID = @AccountTypeID,  
        @AccountID = @AccountID,  
        @ProductID = @ProductID,  
        @ModuleID = @ModuleID,  
        @TrxCodeID = @TrxCodeID,  
        @TrxTypeID = @TrxTypeID,  
        @TrxDate = @WorkingDate,   
        @ValueDate = @WorkingDate,   
        @Amount = @Amount,  
        @TrxCurrencyID = @TrxCurrencyID,  
        @InstrumentTypeID = @InstrumentTypeID,  
        @ChequeID = @ChequeID,  
        @ChequeDate = @ChequeDate,  
        @ReferenceNo = @ReferenceNo,  
        @Remarks = @Remarks,  
        @TrxDescriptionID = @TrxDescriptionID,  
        @TrxDescription = @TrxDescription,  
        @MainGLID = @MainGLID,  
        @ContraGLID = @ContraGLID,  
        @TrxFlagID = '',-- @TrxFlagID,  
        @ImageID = @ImageID,  
        @TrxPrinted = @TrxPrinted,  
        @ChequeDigit = @ChequeDigit,  
        @VoucherCode = @VoucherCode,  
        @ReturnCodeID = @OriginalReturnCode,  
        @Commission = @Commission,  
        @TheirCommission = @TheirCommission,  
        @VATPINNo = @VATPINNo,  
        @VATPAYType = @VATPAYType,  
        @VATSerialNo = @VATSerialNo,  
        @VATPAYEMonth = @VATPAYEMonth,  
        @VATPAYECommission = @VATPAYECommission,  
        @BankID = @BankID,  
        @BranchID = @BranchID,  
        @DrawerOrPayeeAccountID = @DrawerOrPayeeAccountID,  
        @DrawerOrPayee = @DrawerOrPayee,  
        @CreatedBy = @OperatorID,  
        @NewRecord = @NewRecord,  
        @TrxRowID = @TrxRowID,  
        @ForwardRemark = @ForwardRemark,  
        @SupervisedBy =  @SupervisedBy  
        SELECT @TrxDescription = 'Unpaid chq no. ' + CAST(@ChequeID AS VARCHAR) + ' - ' + DBO.f_CRB_ReturnCodeDescriptions ('ReturnCodeID', @ReturnedTempID ,'U')   
        
        
      EXEC p_AddOutwardTrx    
        @TrxBranchID  = @TrxBranchID,  
        @TrxBatchID   = @TrxBatchID,  
        @TrxBatchSLNo  = 0,  
        @OurBranchID  = @OurBranchID,  
        @AccountTypeID  = @AccountTypeID,  
        @AccountID   = @AccountID,  
        @ProductID   = @ProductID,  
        @ModuleID   = '3060',  
        @TrxTypeID   = 'OC',  
        @TrxDate   = @TrxDate,  
        @ValueDate   = @TrxDate,  
        @Amount    = @Amount,  
        @LocalAmount  = @Amount,  
        @TrxCurrencyID  = @TrxCurrencyID,  
        @TrxAmount   = @Amount,  
        @ExchangeRate  = 1,  
        @MeanRate   = 1,  
        @TrxDescriptionID = '088',  
        @TrxDescription  = @TrxDescription,  
        @MainGLID   = @MainGLID,  
        @CreatedBy   = 'CLRSys',  
        @NewRecord   = 1,  
        @ChequeDate   = @TrxDate,  
        @ChequeDigit  = @ChequeDigit,  
        @VoucherCode  = @VoucherCode,  
        @ReturnCodeID  = @ReturnCodeID,  
        @BankID    = @BankID,  
        @ContraGLID   = @ContraGLID,  
        @BranchID   = @BranchID,  
        @TrxFlagID   = 'U',  
        @DrawerOrPayeeAccountID = @DrawerOrPayeeAccountID,  
        @DrawerOrPayee  = @DrawerOrPayee,  
        @ErrorNo   = 0,  
        @ImageID   = @ColumnID,  
        @Commission   = 0,  
        @TheirCommission = 0,  
        @VATPINNo   = 0,  
        @VATPAYType   = '',  
        @VATSerialNo  = 0,  
        @VATPAYEMonth  = '',  
        @VATPAYECommission = '',  
        @ReferenceNo = @ReferenceNo,  
        @ChequeID   = @ChequeID  
  
        UPDATE t_TrxClearing SET ColumnID = @ColumnID WHERE TrxType = 'OC' AND TrxBatchID = @TrxBatchID  
          
        IF @TrxTypeID = 'ID' AND @VoucherCode <> '40'  
        BEGIN  
         DECLARE @OutImageID BigInt  
         SELECT @OutImageID = Imageid FROM t_TrxClearing WHERE TrxType IN ('ID') AND ColumnID = @ColumnID  AND VoucherCode <> '40'  
         EXEC p_GetUniqueClearingImageID @OutImageID OUTPUT    
  
         UPDATE t_TrxClearing   
          SET ImageID = @OutImageID  
          WHERE ColumnID = @ColumnID   
         AND TrxType = 'OC'  AND VoucherCode <> '40'  
  
          UPDATE t_Transaction Set ImageID =  @OutImageID WHERE Try_parse(ReferenceNo AS bigint) = @ColumnID AND ModuleID ='3050'  
          SELECT @TrxrowID = TrxRowID FROM t_Transaction WHERE  Try_parse(ReferenceNo AS bigint) = @ColumnID AND ModuleID ='3050' AND TrxTypeID = 'ID'   
          UPDATE  t_TrxClearing SET TrxRowID = @TrxrowID WHERE TrxType IN ('ID') AND ColumnID = @ColumnID AND isNull(TrxrowID,0) = 0  AND ReturnCodeID IN ('00','17')  
            
          SELECT @TrxrowID = TrxRowID FROM t_Transaction WHERE  Try_parse(ReferenceNo AS bigint) = @ColumnID --AND ModuleID ='3060' AND TrxTypeID = 'OC'   
          UPDATE  t_TrxClearing SET TrxRowID = @TrxrowID WHERE TrxType IN ('OC') AND ColumnID = @ColumnID AND isNull(TrxrowID,0) = 0  AND ReturnCodeID NOT IN ('00','17')   
  
  
         BEGIN TRY    
         --  
          INSERT INTO BRNET_ImageServer.dbo.t_ChequeImages         
          (      
           ImageID,OurBranchID,TrxType,TFImage,JFImage,JRImage,UVImage,      
           TFImageSize,JFImageSize,JRImageSize,BankId,OperatorID,TFImageSignature,        
           JFImageSignature,JRImageSignature,CreatedOn,CurrencyID,Validity,[Date],IsMdv        
          )        
          SELECT @OutImageID,OurBranchID,'OC',FRONTBWIMAGE,FRONTGRAYSCALEIMAGE,REARIMAGE,UVImage,      
           NULL,NULL,NULL,BankID,'SYS',NULL,        
           NULL,NULL,Date,CurrencyID,Validity,Date,0        
          FROM t_IncomingTransactions  NOLOCK        
          WHERE ColumnID = @ColumnID   
         END TRY    
         BEGIN CATCH   
          --EXECUTE usp_GetErrorInfo;   
          --PRINT 'Shoot'  
         END CATCH;   
              
  
  
        END  
          --COMMIT TRAN UNPSTP  
       --COMMIT TRAN POSTPAYSTP  
       END TRY  
       BEGIN CATCH  
        --ROLLBACK TRAN UNPSTP  
        DECLARE @ErrorCode VARCHAR(100)  
        SELECT @ErrorCode = ERROR_MESSAGE()  
        RAISERROR (@ErrorCode,18,1)   
        RETURN  
       END CATCH  
     END  
    END   
   END  
  END  
    
  RETURN  
 END  
 ELSE IF @TxnType='UnPay'  
  BEGIN  
     
   --IF @ReturnedTempID NOT IN ('62','63')  
   --BEGIN  
   -- SELECT @TrxBranchID = @OurBranchID  
   -- SELECT @AccountID = dbo.f_GetCurrencyBranchGLAccountID(@TrxBranchID, ISNULL(@TrxCurrencyID, 'KES'), 'ACP_CLR_SUSP_AC')  
   -- SELECT @OurBranchID = @OurBranchID  
   -- SELECT @AccountTypeID = 'G'  
   -- SELECT @ProductID = 'GL'  
   -- SELECT @MainGLID = dbo.f_GetCurrencyBranchGLAccountID(@TrxBranchID, ISNULL(@TrxCurrencyID, 'KES'), 'ACP_CLR_SUSP_AC')  
   -- SELECT @ContraGLID = dbo.f_GetCurrencyBranchGLAccountID(@TrxBranchID, ISNULL(@TrxCurrencyID, 'KES'), 'CUR_CLR_AC' )   
   --END  
     
     --select 'Kamunya unpay'  
  
   IF @TrxTypeID = 'ID'  
    BEGIN  
     SET @NewTrxTypeID = 'OC'  
    END  
   ELSE   
    BEGIN  
     SET @NewTrxTypeID = 'OD'  
    END  
  
   --IF @BankID <> RIGHT(@FileName,2)  
   --BEGIN  
   -- SELECT @BankID = RIGHT(@FileName,2)  
   -- UPDATE t_IncomingTransactions SET BankID = RIGHT(@FileName,2) WHERE ColumnID = @ColumnID  
   --END  
   --print  
  
     
   ------1. Post Inward  
   IF NOT EXISTS(SELECT 1 FROM t_Transaction WHERE Try_parse(ReferenceNo AS bigint) = @TrxRowID AND TrxTypeID IN ('IC','ID'))  
   BEGIN TRY  
   BEGIN TRAN POSTUNPAY  
  
    EXEC p_AddIncomingTrx  
     @TrxBranchID = @TrxBranchID, --@ClearingTrxBranchID,   
     @TrxBatchID = @TrxBatchID OUTPUT,  
     @SerialID = @SerialID OUTPUT,  
     @OurBranchID = @OurBranchID,  
     @AccountTypeID = @AccountTypeID,  
     @AccountID = @AccountID,  
     @ProductID = @ProductID,  
     @ModuleID = @ModuleID,  
     @TrxCodeID = @TrxCodeID,  
     @TrxTypeID = @TrxTypeID,  
     @TrxDate = @WorkingDate,   
     @ValueDate = @WorkingDate,   
     @Amount = @Amount,  
     @TrxCurrencyID = @TrxCurrencyID,  
     @InstrumentTypeID = @InstrumentTypeID,  
     @ChequeID = @ChequeID,  
     @ChequeDate = @ChequeDate,  
     @ReferenceNo = @TrxRowID,  
     @Remarks = @Remarks,  
     @TrxDescriptionID = @TrxDescriptionID,  
     @TrxDescription = @TrxDescription,  
     @MainGLID = @MainGLID,  
     @ContraGLID = @ContraGLID,  
     @TrxFlagID = '',  
     @ImageID = @ImageID,  
     @TrxPrinted = @TrxPrinted,  
     @ChequeDigit = @ChequeDigit,  
     @VoucherCode = @VoucherCode,  
     @ReturnCodeID = @OriginalReturnCode,  
     @Commission = @Commission,  
     @TheirCommission = @TheirCommission,  
     @VATPINNo = @VATPINNo,  
     @VATPAYType = @VATPAYType,  
     @VATSerialNo = @VATSerialNo,  
     @VATPAYEMonth = @VATPAYEMonth,  
     @VATPAYECommission = @VATPAYECommission,  
     @BankID = @BankID,  
     @BranchID = @BranchID,  
     @DrawerOrPayeeAccountID = @DrawerOrPayeeAccountID,  
     @DrawerOrPayee = @DrawerOrPayee,  
     @CreatedBy = 'SYS',  
     @NewRecord = @NewRecord,  
     @TrxRowID = @TrxRowID,  
     @ForwardRemark = @ForwardRemark  
  
       
    --2. Post Outward/Unpay  
    SELECT @ModuleID = CASE WHEN @TrxTypeID = 'ID' AND @VoucherCode <> '40'  THEN 3060 ELSE 3070 END  
  
    SELECT @Remarks = CASE WHEN @NewTrxTypeID = 'OC' AND @VoucherCode <> '40' THEN CONCAT('Cheque Unpay :', CAST(@ChequeID as VARCHAR)) ELSE '' END   
     
    SELECT @TrxDescription =   
    CASE WHEN @NewTrxTypeID = 'OC' AND @VoucherCode <> '40'   
      THEN CONCAT('Cheque Unpaid ',CAST(@ChequeID as VARCHAR), ': Rsn : ' +  DBO.f_CRB_ReturnCodeDescriptions('ReturnCodeID',@ReturnedTempID,'T'))  
      WHEN @NewTrxTypeID = 'OD' AND @VoucherCode <> '40'  AND @ReturnedTempID <> '00'  
      THEN 'Rejected EFT Credit Rsn : ' +  DBO.f_CRB_ReturnCodeDescriptions('ReturnCodeID',@ReturnedTempID,'T')  
      ELSE @TrxDescription END   
  
     IF @ReturnedTempID NOT IN ('00','17') AND @NewTrxTypeID = 'OC'   
     BEGIN    
     SELECT @TrxDescriptionID =   
      CASE WHEN @ReturnedTempID = '63' THEN '086'   -- AND @AccountTypeID ='C'   
        WHEN @ReturnedTempID = '62' THEN '087'  --AND @AccountTypeID ='C'   
        WHEN @ReturnCodeID IN ('31','32','33','37','38','40','41','42','43','53','54','55','58','64','79') THEN '088'  
      ELSE '005' END  
     END   
  
    IF @ReturnedTempID NOT IN ('00','17') AND @NewTrxTypeID = 'OC'   
    BEGIN  
     SELECT @TrxDescription = 'Unpaid OC Rsn : ' +  DBO.f_CRB_ReturnCodeDescriptions('ReturnCodeID',@ReturnedTempID,'T')   
    END  
  
    --SELECT @TrxDescription  
     --print 'Hapa'  
     --Print @TrxDescriptionID  
     --print @ReturnedTempID  
     --ROLLBACK TRAN POSTUNPAY  
     --return  
       
    --IF isNull(@Verified,0) = 0  
    --BEGIN  
    -- @CreatedBy  
    --END  
      
     EXEC p_AddOutgoingTrx  
     @TrxBranchID = @TrxBranchID,  
     @TrxBatchID = @TrxBatchID OUTPUT,  
     @TrxBatchSLNo = 0,  
     @SerialID = @SerialID  OUTPUT,  
     @OurBranchID = @OurBranchID,  
     @AccountTypeID = @AccountTypeID,  
     @AccountID = @AccountID,  
     @ProductID = @ProductID,   
     @ModuleID = @ModuleID,  
     @TrxTypeID = @NewTrxTypeID,  
     @TrxDate = @WorkingDate,  
     @ValueDate = @WorkingDate,   
     @Amount = @Amount,  
     @LocalAmount = @Amount,  
     @TrxCurrencyID = @TrxCurrencyID,  
     @TrxAmount = @Amount,  
     @ExchangeRate = @ExchangeRate,  
     @MeanRate = @MeanRate,  
     @Profit = 0,   
     @InstrumentTypeID = 'V',  
     @ChequeID = @ChequeID,  
     @ChequeDate = @ChequeDate,  
     @ReferenceNo = @ReferenceNo,  
     @Remarks = @Remarks,  
     @TrxDescriptionID = @TrxDescriptionID,  
     @TrxDescription = @TrxDescription,  
     @MainGLID = @MainGLID,  
     @ContraGLID = @ContraGLID,  
     @TrxFlagID = 'U',  
     @ImageID = 0,  
     @TrxPrinted = 0,  
     @CreatedBy = @OperatorID,  
     @NewRecord = 1,  
     @ChequeDigit = 0,  
     @VoucherCode = @VoucherCode,  
     @ReturnCodeID = @ReturnedTempID,  
     @Commission = 0,  
     @TrxRowID = @TrxRowID,  
     @TheirCommission = 0,   
     @BankID = @BankID,   
     @BranchID = @BranchID,   
     @DrawerOrPayeeAccountID = @DrawerOrPayeeAccountID,   
     @DrawerOrPayee = @DrawerOrPayee,   
     @VATPINNo = NULL,  
     @VATPAYType = NULL,  
     @VATSerialNo = NULL,  
     @VATPAYEMonth = NULL,  
     @VATPAYECommission = 0,  
     @ErrorNo = @ErrorNo,  
     @SupervisedBy = @SupervisedBy  
  
     IF @TrxTypeID = 'ID' AND @VoucherCode <> '40'  
     BEGIN  
      --DECLARE @OutImageID BigInt  
      SELECT @OutImageID = Imageid FROM t_TrxClearing WHERE TrxType IN ('ID') AND ColumnID = @ColumnID  AND VoucherCode <> '40'  
      EXEC p_GetUniqueClearingImageID @OutImageID OUTPUT    
  
      UPDATE t_TrxClearing   
       SET ImageID = @OutImageID  
       WHERE ColumnID = @ColumnID   
      AND TrxType = 'OC'  AND VoucherCode <> '40'  
        
      UPDATE t_Transaction Set ImageID =  @OutImageID WHERE Try_parse(ReferenceNo AS bigint) = @ColumnID AND ModuleID ='3050'  
      SELECT @TrxrowID = TrxRowID FROM t_Transaction WHERE  Try_parse(ReferenceNo AS bigint) = @ColumnID AND ModuleID ='3050' AND TrxTypeID = 'ID'   
      UPDATE  t_TrxClearing SET TrxRowID = @TrxrowID WHERE TrxType IN ('ID') AND ColumnID = @ColumnID AND isNull(TrxrowID,0) = 0  AND ReturnCodeID IN ('00','17')  
      SELECT @TrxrowID = TrxRowID FROM t_Transaction WHERE  Try_parse(ReferenceNo AS bigint) = @ColumnID AND ModuleID ='3060' AND TrxTypeID = 'OC'   
      UPDATE  t_TrxClearing SET TrxRowID = @TrxrowID WHERE TrxType IN ('OC') AND ColumnID = @ColumnID AND isNull(TrxrowID,0) = 0  AND ReturnCodeID NOT IN ('00','17')   
  
  
  
      BEGIN TRY    
      --  
       INSERT INTO BRNET_ImageServer.dbo.t_ChequeImages         
       (      
        ImageID,OurBranchID,TrxType,TFImage,JFImage,JRImage,UVImage,      
        TFImageSize,JFImageSize,JRImageSize,BankId,OperatorID,TFImageSignature,        
        JFImageSignature,JRImageSignature,CreatedOn,CurrencyID,Validity,[Date],IsMdv        
       )        
       SELECT @OutImageID,OurBranchID,'OC',FRONTBWIMAGE,FRONTGRAYSCALEIMAGE,REARIMAGE,UVImage,      
        NULL,NULL,NULL,BankID,'SYS',NULL,        
        NULL,NULL,Date,CurrencyID,Validity,Date,0        
       FROM t_IncomingTransactions  NOLOCK        
       WHERE ColumnID = @ColumnID   
      END TRY    
      BEGIN CATCH   
       --EXECUTE usp_GetErrorInfo;   
       --PRINT 'Shoot'  
      END CATCH;   
              
  
     END  
     IF @TrxTypeID = 'IC'   
     BEGIN  
      SELECT @TrxrowID = TrxRowID FROM t_Transaction WHERE  Try_parse(ReferenceNo AS bigint) = @ColumnID AND ModuleID ='3070' AND TrxTypeID = 'OD'  
      UPDATE  t_TrxClearing SET TrxRowID = @TrxrowID WHERE TrxType IN ('OD') AND ColumnID = @ColumnID AND isNull(TrxrowID,0) = 0   
     END  
       
  
    COMMIT TRAN POSTUNPAY  
    END TRY  
    BEGIN CATCH  
     Print  ERROR_MESSAGE() --AS ErrorMessage  
     ROLLBACK TRAN POSTUNPAY  
     RETURN  
    END CATCH  
  
    IF EXISTS (SELECT 1 FROM t_IncomingTransactions WHERE ColumnID = @ColumnID AND isNull(Verified,0) = 0)  
     BEGIN  
      UPDATE t_IncomingTransactions   
      SET  Returned = 1, Paid = 1, isProcessed = 1, Verified = 1, VerifiedBy = @OperatorID,   
        VerifiedOn = @WorkingDate, ClearedDate = @WorkingDate, TrxBatchID = @TrxBatchID,  
        SupervisedByOne = @OperatorID, SupervisedOnOne = getDate(),SupervisionFlag = ''  
      WHERE ColumnID=@ColumnID   
     END  
    ELSE  
     BEGIN  
      UPDATE t_IncomingTransactions   
      SET  Returned = 1, Paid = 1, isProcessed = 1, ClearedBy = @OperatorID,   
        ClearedOn = @WorkingDate, ClearedDate = @WorkingDate, TrxBatchID = @TrxBatchID,  
        SupervisedByOne = @OperatorID, SupervisedOnOne = getDate(),SupervisionFlag = ''  
      WHERE ColumnID=@ColumnID   
     END  
  
    UPDATE t_TrxClearing Set TrxRowID = t_transaction.TrxRowID  
    FROM t_transaction   
    WHERE t_TrxClearing.TrxBatchID = t_transaction.TrxBatchID  
    AND Try_parse(t_transaction.ReferenceNo AS bigint) = ColumnID  
    AND t_TrxClearing.TrxRowID IS Null  
    AND t_TrxClearing.ColumnID  = @TrxRowID  
  
    IF @NewTrxTypeID = 'OC' AND @ReturnedTempID NOT IN ('00','17') AND @ChequeID = 999999  
    BEGIN  
     UPDATE t_TrxClearing Set TrxRowID = t_transaction.TrxRowID, ColumnID = @TrxRowID  
     FROM t_transaction   
     WHERE t_TrxClearing.TrxBatchID = t_transaction.TrxBatchID  
     AND Try_parse(t_transaction.ReferenceNo AS bigint) = @TrxRowID  
     AND isNull(t_TrxClearing.TrxRowID,0) = 0  
     AND t_TrxClearing.ChequeID = 999999  
     AND t_TrxClearing.TrxType = 'OD'  
    END  
  
    UPDATE t_TrxClearing Set ColumnID = Try_parse(t_transaction.ReferenceNo AS bigint)  
    FROM t_transaction   
    WHERE t_TrxClearing.TrxBatchID = t_transaction.TrxBatchID  
    AND t_TrxClearing.TrxRowID = t_transaction.TrxRowID   
    AND t_TrxClearing.ColumnID  IS NULL   
  END  
 ELSE IF @TxnType='Escalate'  
  BEGIN  
   --IF isNull(@Verified,0) = 1  
   --BEGIN  
    UPDATE t_IncomingTransactions   
    SET Paid = 0, Escalated = 1,IsProcessed = 0, SupervisionFlag = 'X',EscalatedBy = @OperatorID,  
     EscalatedOn = GETDATE(), EscalationReason = @EscalationNotes, Verified=1,VerifiedBy=@OperatorID,VerifiedOn=getdate()   
    WHERE ColumnID = @TrxRowID   
    RETURN  
   --END  
  END  
 ELSE IF @TxnType='Supervise'  
  BEGIN  
   IF isNull(@Verified,0) = 1  
   BEGIN  
    IF isNull(@AccountTypeID,'') = ''  
    BEGIN   
     RETURN  
    END  
    --IF @AccountID IN (SELECT AccountID FROM (  
    --      SELECT OurBranchID, AccountID, Date, Amount, AvailAmount, TrxTypeID   
    --      FROM (SELECT OurBranchID, AccountID, Date, SUM(amount) Amount, dbo.f_GetAvailableBalance(OurBranchID,AccountID) AvailAmount,TrxType TrxTypeID  
    --       FROM t_IncomingTransactions (NOLOCK)  
    --       WHERE AccountType = 'C' AND ReturnCode in ('00','17') AND TrxType = 'ID'   
    --       GROUP BY OurBranchID, AccountID, date,TrxType) AS A  
    --     WHERE Amount > AvailAmount AND TrxTypeID = 'ID') AS B)   
    -- AND @TrxTypeID = 'ID' AND @VoucherCode <> '40'  
    -- BEGIN  
    --  UPDATE t_IncomingTransactions   
    --   SET  Escalated = 1, Paid = 0, isProcessed = 0, SupervisedByOne = @OperatorID, AutoUnPay=1,   
    --     SupervisedOnOne = getDate(), ClearedDate = @WorkingDate, TrxBatchID = @TrxBatchID,   
    --     SupervisionFlag = 'X', EscalationReason = 'This account will be overdraw'   
    --  WHERE ColumnID = @ColumnID  
    --  RETURN  
    -- END  
    --ELSE   
    --IF (@TrxTypeID = 'IC')  
    -- BEGIN  
    --  SELECT @TrxDescriptionID = '089'  
    --  --RETURN  
    --  IF NOT EXISTS(SELECT 1 FROM t_Transaction WHERE Try_parse(ReferenceNo AS bigint) = @TrxRowID AND TrxTypeID IN ('IC'))  
    --  BEGIN TRY  
    --  BEGIN TRAN POSTPAYICSUPERVISE  
    --  EXEC p_AddIncomingTrx  
    --    @TrxBranchID = @TrxBranchID,--@ClearingTrxBranchID,   
    --    @TrxBatchID = @TrxBatchID  OUTPUT,  
    --    @SerialID = @SerialID  OUTPUT,  
    --    @OurBranchID = @OurBranchID,  
    --    @AccountTypeID = @AccountTypeID,  
    --    @AccountID = @AccountID,  
    --    @ProductID = @ProductID,  
    --    @ModuleID = @ModuleID,  
    --    @TrxCodeID = @TrxCodeID,  
    --    @TrxTypeID = @TrxTypeID,  
    --    @TrxDate = @WorkingDate,   
    --    @ValueDate = @WorkingDate,   
    --    @Amount = @Amount,  
    --    @TrxCurrencyID = @TrxCurrencyID,  
    --    @InstrumentTypeID = @InstrumentTypeID,  
    --    @ChequeID = @ChequeID,  
    --    @ChequeDate = @ChequeDate,  
    --    @ReferenceNo = @ReferenceNo,  
    --    @Remarks = @Remarks,  
    --    @TrxDescriptionID = @TrxDescriptionID,  
    --    @TrxDescription = @TrxDescription,  
    --    @MainGLID = @MainGLID,  
    --    @ContraGLID = @ContraGLID,  
    --    @TrxFlagID = 'U',-- @TrxFlagID,  
    --    @ImageID = @ImageID,  
    --    @TrxPrinted = @TrxPrinted,  
    --    @ChequeDigit = @ChequeDigit,  
    --    @VoucherCode = @VoucherCode,  
    --    @ReturnCodeID = @OriginalReturnCode,  
    --    @Commission = @Commission,  
    --    @TheirCommission = @TheirCommission,  
    --    @VATPINNo = @VATPINNo,  
    --    @VATPAYType = @VATPAYType,  
    --    @VATSerialNo = @VATSerialNo,  
    --    @VATPAYEMonth = @VATPAYEMonth,  
    --    @VATPAYECommission = @VATPAYECommission,  
    --    @BankID = @BankID,  
    --    @BranchID = @BranchID,  
    --    @DrawerOrPayeeAccountID = @DrawerOrPayeeAccountID,  
    --    @DrawerOrPayee = @DrawerOrPayee,  
    --    @CreatedBy = @CreatedBy,  
    --    @NewRecord = @NewRecord,  
    --    @TrxRowID = @TrxRowID,  
    --    @ForwardRemark = @ForwardRemark,  
    --    @SupervisedBy =  @OperatorID  
    --   COMMIT TRAN POSTPAYICSUPERVISE  
    --   END TRY  
    --   BEGIN CATCH  
    --    ROLLBACK TRAN POSTPAYICSUPERVISE  
    --    RETURN  
    --   END CATCH  
    --   UPDATE t_IncomingTransactions   
    --   SET  Paid = 1, Escalated = 0,isProcessed = 1, AuthorizedBy = @OperatorID,   
    --     AuthorizedOn = getDate(), ClearedDate = @WorkingDate, TrxBatchID = @TrxBatchID, SupervisionFlag = ''    
    --   WHERE ColumnID=@TrxRowID   
  
    --   UPDATE t_TrxClearing Set TrxRowID = t_transaction.TrxRowID  
    --   FROM t_transaction   
    --   WHERE t_TrxClearing.TrxBatchID = t_transaction.TrxBatchID  
    --   AND Try_parse(ReferenceNo AS bigint) = ColumnID  
    --   AND t_TrxClearing.TrxRowID IS Null  
    --   AND t_TrxClearing.ColumnID  = @TrxRowID  
  
    --   UPDATE t_TrxClearing Set ColumnID = Try_parse(t_transaction.ReferenceNo AS bigint)  
    --   FROM t_transaction   
    --   WHERE t_TrxClearing.TrxBatchID = t_transaction.TrxBatchID  
    --   AND t_TrxClearing.TrxRowID = t_transaction.TrxRowID   
    --   AND t_TrxClearing.ColumnID  IS NULL   
    --   RETURN  
    --  END  
    --ELSE   
    IF (@OriginalReturnCode NOT IN ('00','17') AND @TrxTypeID = 'ID')  
    BEGIN  
      IF  @TrxTypeID ='ID' --@AccountTypeID = 'C'  AND  
      BEGIN    
       IF @OriginalReturnCode = '63'    
        BEGIN    
         SELECT @TrxDescriptionID = '083'    
        END     
       ELSE IF @OriginalReturnCode  = '62'    
        BEGIN    
         SELECT @TrxDescriptionID = '084'    
        END    
       ELSE IF @OriginalReturnCode  = '55'    
        BEGIN    
         SELECT @TrxDescriptionID = '085'    
        END      
       ELSE    
        BEGIN    
         SELECT @TrxDescriptionID = '004'    
        END   
      END  
  
     IF @ReturnedTempID IN ('17') AND @TrxTypeID ='ID'  
     BEGIN  
      --SELECT @TrxBranchID = @OurBranchID  
      SELECT @AccountID = dbo.f_GetCurrencyBranchGLAccountID(@TrxBranchID, ISNULL(@TrxCurrencyID, 'TZS'), 'ACP_CLR_SUSP_AC_CHQ')  
      SELECT @OurBranchID = @OurBranchID  
      SELECT @AccountTypeID = 'G'  
      SELECT @ProductID = 'GL'  
      SELECT @MainGLID = dbo.f_GetCurrencyBranchGLAccountID(@TrxBranchID, ISNULL(@TrxCurrencyID, 'TZS'), 'ACP_CLR_SUSP_AC_CHQ')  
      SELECT @ContraGLID = dbo.f_GetCurrencyBranchGLAccountID(@TrxBranchID, ISNULL(@TrxCurrencyID, 'TZS'), 'CUR_CLR_AC' )   
     END  
  
  
     IF NOT EXISTS(SELECT 1 FROM t_Transaction WHERE Try_parse(ReferenceNo AS bigint) = @TrxRowID AND TrxTypeID IN ('ID'))  
     BEGIN TRY  
     BEGIN TRAN POSTPAYIDSUPERVISE       
     EXEC p_AddIncomingTrx  
       @TrxBranchID = @TrxBranchID,--@ClearingTrxBranchID,   
       @TrxBatchID = @TrxBatchID  OUTPUT,  
       @SerialID = @SerialID  OUTPUT,  
       @OurBranchID = @OurBranchID,  
       @AccountTypeID = @AccountTypeID,  
       @AccountID = @AccountID,  
       @ProductID = @ProductID,  
       @ModuleID = @ModuleID,  
       @TrxCodeID = @TrxCodeID,  
       @TrxTypeID = @TrxTypeID,  
       @TrxDate = @WorkingDate,   
       @ValueDate = @WorkingDate,   
       @Amount = @Amount,  
       @TrxCurrencyID = @TrxCurrencyID,  
       @InstrumentTypeID = @InstrumentTypeID,  
       @ChequeID = @ChequeID,  
       @ChequeDate = @ChequeDate,  
       @ReferenceNo = @ReferenceNo,  
       @Remarks = @Remarks,  
       @TrxDescriptionID = @TrxDescriptionID,  
       @TrxDescription = @TrxDescription,  
       @MainGLID = @MainGLID,  
       @ContraGLID = @ContraGLID,  
       @TrxFlagID = '',-- @TrxFlagID,  
       @ImageID = @ImageID,  
       @TrxPrinted = @TrxPrinted,  
       @ChequeDigit = @ChequeDigit,  
       @VoucherCode = @VoucherCode,  
       @ReturnCodeID = @OriginalReturnCode,  
       @Commission = @Commission,  
       @TheirCommission = @TheirCommission,  
       @VATPINNo = @VATPINNo,  
       @VATPAYType = @VATPAYType,  
       @VATSerialNo = @VATSerialNo,  
       @VATPAYEMonth = @VATPAYEMonth,  
       @VATPAYECommission = @VATPAYECommission,  
       @BankID = @BankID,  
       @BranchID = @BranchID,  
       @DrawerOrPayeeAccountID = @DrawerOrPayeeAccountID,  
       @DrawerOrPayee = @DrawerOrPayee,  
       @CreatedBy = @CreatedBy,  
       @NewRecord = @NewRecord,  
       @TrxRowID = @TrxRowID,  
       @ForwardRemark = @ForwardRemark,  
       @SupervisedBy =  @OperatorID  
      COMMIT TRAN POSTPAYIDSUPERVISE  
      END TRY  
      BEGIN CATCH  
       ROLLBACK TRAN POSTPAYIDSUPERVISE  
       RETURN  
      END CATCH  
      UPDATE t_IncomingTransactions   
      SET  Paid = 1, Escalated = 0,isProcessed = 1, AuthorizedBy = @OperatorID,   
        AuthorizedOn = getDate(), ClearedDate = @WorkingDate, TrxBatchID = @TrxBatchID, SupervisionFlag = ''    
      WHERE ColumnID=@TrxRowID   
  
      UPDATE t_TrxClearing Set TrxRowID = t_transaction.TrxRowID  
      FROM t_transaction   
      WHERE t_TrxClearing.TrxBatchID = t_transaction.TrxBatchID  
      AND Try_parse(t_transaction.ReferenceNo AS bigint) = ColumnID  
      AND t_TrxClearing.TrxRowID IS Null  
      AND t_TrxClearing.ColumnID  = @TrxRowID  
  
      UPDATE t_TrxClearing Set ColumnID = Try_parse(t_transaction.ReferenceNo AS bigint)  
      FROM t_transaction   
      WHERE t_TrxClearing.TrxBatchID = t_transaction.TrxBatchID  
      AND t_TrxClearing.TrxRowID = t_transaction.TrxRowID   
      AND t_TrxClearing.ColumnID  IS NULL   
      RETURN  
    END  
    ELSE  
    BEGIN  
     UPDATE t_IncomingTransactions   
     SET  Paid = 0, Escalated = 0, isProcessed = 0, SupervisedByOne = @OperatorID,   
       SupervisedOnOne = getDate(), ClearedDate = @WorkingDate, TrxBatchID = @TrxBatchID,   
       SupervisionFlag = 'X'   
     WHERE ColumnID=@TrxRowID   
     RETURN  
    END  
  END  
 END  
 ELSE IF @TxnType='Authorize'  
  BEGIN  
    
   --print @TrxTypeID  
   --print @TrxDescriptionID  
   --return  
  
   --IF @ReturnedTempID IN ('17') AND @TrxTypeID ='ID'  
   --BEGIN  
   -- --SELECT @TrxBranchID = @OurBranchID  
   -- SELECT @AccountID = dbo.f_GetCurrencyBranchGLAccountID(@TrxBranchID, ISNULL(@TrxCurrencyID, 'KES'), 'ACP_CLR_SUSP_AC')  
   -- SELECT @OurBranchID = @OurBranchID  
   -- SELECT @AccountTypeID = 'G'  
   -- SELECT @ProductID = 'GL'  
   -- SELECT @MainGLID = dbo.f_GetCurrencyBranchGLAccountID(@TrxBranchID, ISNULL(@TrxCurrencyID, 'KES'), 'ACP_CLR_SUSP_AC')  
   -- SELECT @ContraGLID = dbo.f_GetCurrencyBranchGLAccountID(@TrxBranchID, ISNULL(@TrxCurrencyID, 'KES'), 'CUR_CLR_AC' )   
   --END  
  
  
   IF @OriginalReturnCode = '00'  AND @TrxTypeID = 'ID'  
   BEGIN  
    SELECT @TrxDescriptionID = '004'  
   END  
  
   --RETURN---------------------------------------------------------------------------------------------------------  
     
  
   IF isNull(@Verified,0) = 1  
   BEGIN  
    SET @SupervisedBy = @OperatorID  
   IF NOT EXISTS(SELECT 1 FROM t_Transaction WHERE Try_parse(ReferenceNo AS bigint) = @TrxRowID AND TrxTypeID IN ('IC','ID'))  
   BEGIN TRY  
   BEGIN TRAN POSTPAY  
   IF dbo.f_GetStoppedAccChequeIDStatus(@OurBranchID,@AccountID,@ChequeID) = 'S' AND @ReturnCodeID IN ('00','17') AND @TrxTypeID = 'ID'    
     BEGIN  
      SET @TrxDate = dbo.f_GetWorkingDate(@OurBranchID)    
      SET @TrxCurrencyID = ISNULL( @TrxCurrencyID, 'TZS')  
      BEGIN TRY   
      BEGIN TRAN UNP  
       EXEC p_AddIncomingTrx  
        @TrxBranchID = @HeadOfficeBranch,  
        @TrxBatchID = @TrxBatchID  OUTPUT,  
        @SerialID = @SerialID  OUTPUT,  
        @OurBranchID = @OurBranchID,  
        @AccountTypeID = @AccountTypeID,  
        @AccountID = @AccountID,  
        @ProductID = @ProductID,  
        @ModuleID = @ModuleID,  
        @TrxCodeID = @TrxCodeID,  
        @TrxTypeID = @TrxTypeID,  
        @TrxDate = @WorkingDate,   
        @ValueDate = @WorkingDate,   
        @Amount = @Amount,  
        @TrxCurrencyID = @TrxCurrencyID,  
        @InstrumentTypeID = @InstrumentTypeID,  
        @ChequeID = @ChequeID,  
        @ChequeDate = @ChequeDate,  
        @ReferenceNo = @ReferenceNo,  
        @Remarks = @Remarks,  
        @TrxDescriptionID = @TrxDescriptionID,  
        @TrxDescription = @TrxDescription,  
        @MainGLID = @MainGLID,  
        @ContraGLID = @ContraGLID,  
        @TrxFlagID = '',-- @TrxFlagID,  
        @ImageID = @ImageID,  
        @TrxPrinted = @TrxPrinted,  
        @ChequeDigit = @ChequeDigit,  
        @VoucherCode = @VoucherCode,  
        @ReturnCodeID = @OriginalReturnCode,  
        @Commission = @Commission,  
        @TheirCommission = @TheirCommission,  
        @VATPINNo = @VATPINNo,  
        @VATPAYType = @VATPAYType,  
        @VATSerialNo = @VATSerialNo,  
        @VATPAYEMonth = @VATPAYEMonth,  
        @VATPAYECommission = @VATPAYECommission,  
        @BankID = @BankID,  
        @BranchID = @BranchID,  
        @DrawerOrPayeeAccountID = @DrawerOrPayeeAccountID,  
        @DrawerOrPayee = @DrawerOrPayee,  
        @CreatedBy = @CreatedBy,  
        @NewRecord = @NewRecord,  
        @TrxRowID = @TrxRowID,  
        @ForwardRemark = @ForwardRemark,  
        @SupervisedBy =  @SupervisedBy  
  
  
       SELECT @TrxDescription = 'Unpaid chq no. ' + CAST(@ChequeID AS VARCHAR) + ' - ' + DBO.f_CRB_ReturnCodeDescriptions ('ReturnCodeID', '79' ,'U')   
       EXEC p_AddOutwardTrx    
        @TrxBranchID  = @HeadOfficeBranch,  
        @TrxBatchID   = @TrxBatchID,  
        @TrxBatchSLNo  = 0,  
        @OurBranchID  = @OurBranchID,  
        @AccountTypeID  = @AccountTypeID,  
        @AccountID   = @AccountID,  
        @ProductID   = @ProductID,  
        @ModuleID   = '3060',  
        @TrxTypeID   = 'OC',  
        @TrxDate   = @TrxDate,  
        @ValueDate   = @TrxDate,  
        @Amount    = @Amount,  
        @LocalAmount  = @Amount,  
        @TrxCurrencyID  = @TrxCurrencyID,  
        @TrxAmount   = @Amount,  
        @ExchangeRate  = 1,  
        @MeanRate   = 1,  
        @TrxDescriptionID = '088',  
        @TrxDescription  = @TrxDescription,  
        @MainGLID   = @MainGLID,  
        @CreatedBy   = 'CLRSys',  
        @NewRecord   = 1,  
        @ChequeDate   = @TrxDate,  
        @ChequeDigit  = @ChequeDigit,  
        @VoucherCode  = @VoucherCode,  
        @ReturnCodeID  = @ReturnCodeID,  
        @BankID    = @BankID,  
        @ContraGLID   = @ContraGLID,  
        @BranchID   = @BranchID,  
        @TrxFlagID   = 'U',  
        @DrawerOrPayeeAccountID = @DrawerOrPayeeAccountID,  
        @DrawerOrPayee  = @DrawerOrPayee,  
        @ErrorNo   = 0,  
        @ImageID   = @ColumnID  
      COMMIT TRAN UNP  
      COMMIT TRAN POSTPAY  
      END TRY  
      BEGIN CATCH  
       ROLLBACK TRAN UNP  
       --DECLARE @ErrorCode VARCHAR(100)  
       --SELECT @ErrorCode = ERROR_MESSAGE()  
       --RAISERROR (@ErrorCode,18,1)   
       RETURN  
      END CATCH  
     END  
    ELSE IF (@TrxTypeID = 'IC')  
     BEGIN  
      --print @TrxBranchID  
      SELECT @TrxDescriptionID = '089'  
      --RETURN  
      IF NOT EXISTS(SELECT 1 FROM t_Transaction WHERE Try_parse(ReferenceNo AS bigint) = @TrxRowID AND TrxTypeID IN ('IC'))  
      BEGIN TRY  
      BEGIN TRAN POSTPAYICSUPERVISE  
        
      EXEC p_AddIncomingTrx  
        @TrxBranchID = @HeadOfficeBranch,  
        @TrxBatchID = @TrxBatchID  OUTPUT,  
        @SerialID = @SerialID  OUTPUT,  
        @OurBranchID = @OurBranchID,  
        @AccountTypeID = @AccountTypeID,  
        @AccountID = @AccountID,  
        @ProductID = @ProductID,  
        @ModuleID = @ModuleID,  
        @TrxCodeID = @TrxCodeID,  
        @TrxTypeID = @TrxTypeID,  
        @TrxDate = @WorkingDate,   
        @ValueDate = @WorkingDate,   
        @Amount = @Amount,  
        @TrxCurrencyID = @TrxCurrencyID,  
        @InstrumentTypeID = @InstrumentTypeID,  
        @ChequeID = @ChequeID,  
        @ChequeDate = @ChequeDate,  
        @ReferenceNo = @ReferenceNo,  
        @Remarks = @Remarks,  
        @TrxDescriptionID = @TrxDescriptionID,  
        @TrxDescription = @TrxDescription,  
        @MainGLID = @MainGLID,  
        @ContraGLID = @ContraGLID,  
        @TrxFlagID = '',-- @TrxFlagID,  
        @ImageID = @ImageID,  
        @TrxPrinted = @TrxPrinted,  
        @ChequeDigit = @ChequeDigit,  
        @VoucherCode = @VoucherCode,  
        @ReturnCodeID = @OriginalReturnCode,  
        @Commission = @Commission,  
        @TheirCommission = @TheirCommission,  
        @VATPINNo = @VATPINNo,  
        @VATPAYType = @VATPAYType,  
        @VATSerialNo = @VATSerialNo,  
        @VATPAYEMonth = @VATPAYEMonth,  
        @VATPAYECommission = @VATPAYECommission,  
        @BankID = @BankID,  
        @BranchID = @BranchID,  
        @DrawerOrPayeeAccountID = @DrawerOrPayeeAccountID,  
        @DrawerOrPayee = @DrawerOrPayee,  
        @CreatedBy = @CreatedBy,  
        @NewRecord = @NewRecord,  
        @TrxRowID = @TrxRowID,  
        @ForwardRemark = @ForwardRemark,  
        @SupervisedBy =  @OperatorID  
       COMMIT TRAN POSTPAYICSUPERVISE  
       COMMIT TRAN POSTPAY  
       END TRY  
       BEGIN CATCH  
        ROLLBACK TRAN POSTPAYICSUPERVISE  
        RETURN  
       END CATCH  
  
         
       UPDATE t_IncomingTransactions   
       SET  Paid = 1, Escalated = 0,isProcessed = 1, AuthorizedBy = @OperatorID,   
         AuthorizedOn = getDate(), ClearedDate = @WorkingDate, TrxBatchID = @TrxBatchID, SupervisionFlag = ''    
       WHERE ColumnID=@TrxRowID   
  
       UPDATE t_TrxClearing Set TrxRowID = t_transaction.TrxRowID  
       FROM t_transaction   
       WHERE t_TrxClearing.TrxBatchID = t_transaction.TrxBatchID  
       AND Try_parse(t_transaction.ReferenceNo AS bigint) = ColumnID  
       AND t_TrxClearing.TrxRowID IS Null  
       AND t_TrxClearing.ColumnID  = @TrxRowID  
  
       UPDATE t_TrxClearing Set ColumnID = Try_parse(t_transaction.ReferenceNo AS bigint)  
       FROM t_transaction   
       WHERE t_TrxClearing.TrxBatchID = t_transaction.TrxBatchID  
       AND t_TrxClearing.TrxRowID = t_transaction.TrxRowID   
       AND t_TrxClearing.ColumnID  IS NULL   
       RETURN  
      END   
       ELSE  
     BEGIN  
  
  
     IF @ExcessOverLimitFlag = 'B' AND @TrxTypeID = 'ID'  
      BEGIN  
       SELECT @TrxFlagID = 'B'  
      END  
     ELSE  
      BEGIN  
       SELECT @TrxFlagID = ' '  
      END  
        
      EXEC p_AddIncomingTrx  
       @TrxBranchID = @HeadOfficeBranch,  
       @TrxBatchID = @TrxBatchID  OUTPUT,  
       @SerialID = @SerialID  OUTPUT,  
       @OurBranchID = @OurBranchID,  
       @AccountTypeID = @AccountTypeID,  
       @AccountID = @AccountID,  
       @ProductID = @ProductID,  
       @ModuleID = @ModuleID,  
       @TrxCodeID = @TrxCodeID,  
       @TrxTypeID = @TrxTypeID,  
       @TrxDate = @WorkingDate,   
       @ValueDate = @WorkingDate,   
       @Amount = @Amount,  
       @TrxCurrencyID = @TrxCurrencyID,  
       @InstrumentTypeID = @InstrumentTypeID,  
       @ChequeID = @ChequeID,  
       @ChequeDate = @ChequeDate,  
       @ReferenceNo = @ReferenceNo,  
       @Remarks = @Remarks,  
       @TrxDescriptionID = @TrxDescriptionID,  
       @TrxDescription = @TrxDescription,  
       @MainGLID = @MainGLID,  
       @ContraGLID = @ContraGLID,  
       @TrxFlagID = @TrxFlagID,  
       @ImageID = @ImageID,  
       @TrxPrinted = @TrxPrinted,  
       @ChequeDigit = @ChequeDigit,  
       @VoucherCode = @VoucherCode,  
       @ReturnCodeID = @OriginalReturnCode,  
       @Commission = @Commission,  
       @TheirCommission = @TheirCommission,  
       @VATPINNo = @VATPINNo,  
       @VATPAYType = @VATPAYType,  
       @VATSerialNo = @VATSerialNo,  
       @VATPAYEMonth = @VATPAYEMonth,  
       @VATPAYECommission = @VATPAYECommission,  
       @BankID = @BankID,  
       @BranchID = @BranchID,  
       @DrawerOrPayeeAccountID = @DrawerOrPayeeAccountID,  
       @DrawerOrPayee = @DrawerOrPayee,  
       @CreatedBy = @CreatedBy,  
       @NewRecord = @NewRecord,  
       @TrxRowID = @TrxRowID,  
       @ForwardRemark = @ForwardRemark,  
       @SupervisedBy =  @SupervisedBy  
      END  
     COMMIT TRAN POSTPAY  
     END TRY  
     BEGIN CATCH  
  
      ROLLBACK TRAN POSTPAY  
      RETURN  
     END CATCH  
    UPDATE t_IncomingTransactions   
    SET  Paid = 1, Escalated = 0,isProcessed = 1, AuthorizedBy = @SupervisedBy,   
      AuthorizedOn = getDate(), ClearedDate = @WorkingDate, TrxBatchID = @TrxBatchID, SupervisionFlag = ''    
    WHERE ColumnID=@TrxRowID   
  
    UPDATE t_TrxClearing Set TrxRowID = t_transaction.TrxRowID  
    FROM t_transaction   
    WHERE t_TrxClearing.TrxBatchID = t_transaction.TrxBatchID  
    AND Try_parse(t_transaction.ReferenceNo AS bigint) = ColumnID  
    AND t_TrxClearing.TrxRowID IS Null  
    AND t_TrxClearing.ColumnID  = @TrxRowID  
  
    UPDATE t_TrxClearing Set ColumnID = Try_parse(t_transaction.ReferenceNo AS bigint)  
    FROM t_transaction   
    WHERE t_TrxClearing.TrxBatchID = t_transaction.TrxBatchID  
    AND t_TrxClearing.TrxRowID = t_transaction.TrxRowID   
    AND t_TrxClearing.ColumnID  IS NULL   
  
    DECLARE @TrxNewRowID BigInt  
    SELECT @TrxNewRowID = TrxRowID FROM t_TrxClearing  WHERE  ColumnID  = @TrxRowID  
  
    IF NOT EXISTS(SELECT 1 FROM t_ChequeTrx(NOLOCK) WHERE OurBranchID = @OurBranchID  
    AND AccountTypeID = 'G' AND AccountID = @AccountID   
    AND ChequeID = @ChequeID AND ChequeStatusID IN ('P'))  
    BEGIN  
     UPDATE t_ChequeTrx SET ChequeStatusID = 'P',  
       TrxRowID = @TrxNewRowID, ChequeDate = @TrxDate  
     WHERE OurBranchID = @OurBranchID  
     AND AccountTypeID = 'G'   
     AND AccountID = @AccountID  
     AND ChequeID = @ChequeID  
  
     SELECT @ReturnCodeID=ReturnCodeID FROM t_TrxClearing   
     WHERE ChequeID=@ChequeID   
     AND OurBranchID = @OurBranchID  
     AND AccountID = @AccountID   
     AND TrxType = 'ID'  
  
     IF @TrxTypeID IN('ID')  AND @ReturnCodeID IN ('00','17') AND @VoucherCode IN ('13','14')  
     BEGIN  
      INSERT INTO t_ReconcilableItemRealize  
      (  
       OurBranchID,AccountID,ChequeID,ChequeDate,  
       TrxDate,TrxAmount,ExchangeRate,Remarks,TrxRowID,  
       CreatedBy,CreatedOn,UpdateCount  
      )  
      VALUES  
      (  
       @OurBranchID,@AccountID,@ChequeID,@ChequeDate,  
       @TrxDate,ABS(@Amount),@ExchangeRate,'',@TrxNewRowID,  
       @CreatedBy,GETDATE(),2  
      )  
  
      UPDATE t_ReconcilableItem  
      SET ReconcileStatusID = 'R',ReconcileDate = @TrxDate  
      WHERE OurBranchID = @OurBranchID  
      AND AccountID = @AccountID  
      AND ChequeID = @ChequeID  
     END   
    END  
   END  
  END  
 SET NOCOUNT OFF  
End  
  