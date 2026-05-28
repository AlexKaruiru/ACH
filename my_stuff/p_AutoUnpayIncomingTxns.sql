CREATE PROCEDURE [dbo].[p_AutoUnpayIncomingTxns]  
(  
 @ColumnID  VARCHAR(20),  
 @ReturnCodeID CHAR(2)='00',  
 @TrxBranchID BranchID,  
 @OperatorID  Varchar(50),  
 @inModuleID  Varchar(5),  
 @OutModuleID  Varchar(5),  
 @OutTrxTypeID  Char(2),  
 @InTrxTypeID  Char(2),  
 @OutTrxDescription Description,  
 @InTrxDescription Description  
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
   @TrxCodeID TINYINT,    
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
   @ReturnedTempID CHAR(2),  
   @OldFlagID SystemSubID ='',  
   @Verified Bit,  
   @FileName Varchar(50),  
   @ExcessOverLimitFlag SystemSubID,  
   @HeadOfficeBranch BranchID  
  
 SELECT TOP 1 @HeadOfficeBranch=(OurBranchID) FROM t_SystemBranchSetting WHERE IsHeadOffice=1  
  
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
   @TrxCodeID = 0,   
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
   @ContraGLID = ContraGLID,  
   @TrxFlagID = 'U',  
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
  
 EXEC p_GetUniqueClearingImageID @ImageID OUTPUT  
  
  
 IF isNull(@ReturnedTempID,'') = ''  
 BEGIN  
  IF isNull(@ReturnCodeID,'00') <> '00'  
   BEGIN  
    SELECT @ReturnedTempID = @ReturnCodeID  
   END  
 END  
  
  
 IF EXISTS(SELECT 1 FROM t_SystemBankSetting Where ShortName <> 'FTB')  
 BEGIN  
  IF @OriginalReturnCode NOT IN ('00','17') AND  @InTrxTypeID ='ID'   
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
  IF @InTrxTypeID IN ('IC') AND @AccountTypeID = 'C' AND @OriginalReturnCode NOT IN ('00','17')  
   BEGIN  
    SET @TrxDescriptionID = '089'   
   END   
  IF @InTrxTypeID IN ('IC') AND @AccountTypeID = 'C' AND @OriginalReturnCode IN ('00','17')  
   BEGIN  
    SET @TrxDescriptionID = '089'   
   END   
  ELSE IF @InTrxTypeID IN ('IC') AND @AccountTypeID = 'G'   
   BEGIN  
    SELECT @TrxDescriptionID = '089'  
   END  
 END  
    
  
 SELECT @OurBankID = BankID FROM t_SystemBankSetting (NOLOCK)  
  
 IF(@AccountTypeID = 'C')      
 BEGIN  
  SELECT @MainGLID = dbo.f_GetGLInterfaceAccountID1(@OurBankID, @ProductID,'CONTROL_AC')    
  IF isNull(@ContraGLID,'') = ''  
  BEGIN  
   SELECT @ContraGLID = dbo.f_GetCurrencyBranchGLAccountID(@OurBranchID, @TrxCurrencyID, 'CUR_CLR_AC')     
  END    
 END      
 IF (@ProductID IS NULL OR @ProductID = '')       
 BEGIN  
  IF @AccountTypeID = 'C'      
  BEGIN  
   SELECT @MainGLID = dbo.f_GetGLInterfaceAccountID1(@OurBankID, @ProductID,'CONTROL_AC')  
   IF isNull(@ContraGLID,'') = ''  
   BEGIN  
    SELECT @ContraGLID = dbo.f_GetCurrencyBranchGLAccountID(@OurBranchID, @TrxCurrencyID, 'CUR_CLR_AC')     
   END  
  END      
  IF @AccountTypeID = 'G'      
  BEGIN  
   SET @ProductID = 'GL'    
   SELECT @MainGLID = @AccountID  
   IF isNull(@ContraGLID,'') = ''  
   BEGIN  
    SELECT @ContraGLID = dbo.f_GetCurrencyBranchGLAccountID(@OurBranchID, @TrxCurrencyID, 'CUR_CLR_AC')     
   END    
  END      
 END  
  
  
 IF @ProductID = 'GL'  
 BEGIN      
  IF isNull(@ContraGLID,'') = ''  
  BEGIN  
   SELECT @ContraGLID = dbo.f_GetCurrencyBranchGLAccountID(@OurBranchID, @TrxCurrencyID, 'CUR_CLR_AC')   
   SELECT @MainGLID = @AccountID  
  END  
 END  
   
 SELECT @ChequeID = isNull(@ChequeID,0)  
 SELECT @LocalCurrencyID = dbo.f_GetLocalCurrencyID(@OurBranchID)    
  
 IF @TrxCurrencyID = @LocalCurrencyID      
  BEGIN      
   SELECT  
    @TrxAmount  = @Amount,      
    @LocalAmount = @Amount,      
    @ExchangeRate = 1       
  END      
 ELSE      
  BEGIN      
   SELECT  
     @TrxAmount  = @Amount,       
     @ExchangeRate = dbo.f_GetCurrencyRate(@OurBranchID, @TrxCurrencyID, 'REV', 'M'),     
     @MeanRate = @ExchangeRate,  
     @LocalAmount = @Amount * @MeanRate    
  END  
  
  IF isNull(@Verified,0) = 0  
  BEGIN  
   UPDATE t_IncomingTransactions   
   SET Paid = 0, Escalated = 0,IsProcessed = 0, Verified = 1, VerifiedBy = @OperatorID, VerifiedOn = @WorkingDate  
   WHERE ColumnID = @TrxRowID   
      
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
   WHERE ColumnID = @TrxRowID AND AccountID IN (dbo.f_GetCurrencyBranchGLAccountID(@TrxBranchID, @TrxCurrencyID, 'ACP_CLR_SUSP_AC_CHQ'),  
   dbo.f_GetCurrencyBranchGLAccountID(@TrxBranchID, @TrxCurrencyID, 'ACP_CLR_SUSP_AC_EFT'))  
     
  
   SET @TrxDate = dbo.f_GetWorkingDate(@OurBranchID)    
   SET @TrxCurrencyID = ISNULL( @TrxCurrencyID, 'TZS')  
   BEGIN TRY   
   BEGIN TRAN UNP  
   IF @InTrxTypeID IN ('IC')  
    BEGIN  
     SELECT @AccountID= dbo.f_GetCurrencyBranchGLAccountID(@TrxBranchID,@TrxCurrencyID, 'ACP_CLR_SUSP_AC_EFT')   
    END  
   ELSE  
    BEGIN  
     SELECT @AccountID= dbo.f_GetCurrencyBranchGLAccountID(@TrxBranchID,@TrxCurrencyID, 'ACP_CLR_SUSP_AC_CHQ')   
    END  
   SELECT @AccountTypeID = 'G'  
   SELECT @ProductID = 'GL'  
  
    EXEC p_AddIncomingTrx  
     @TrxBranchID   = @TrxBranchID,  
     @TrxBatchID    = @TrxBatchID  OUTPUT,  
     @SerialID    = @SerialID  OUTPUT,  
     @OurBranchID   = @OurBranchID,  
     @AccountTypeID   = @AccountTypeID,  
     @AccountID    = @AccountID,  
     @ProductID    = @ProductID,  
     @ModuleID    = @InModuleID,  
     @TrxCodeID    = @TrxCodeID,  
     @TrxTypeID    = @InTrxTypeID,  
     @TrxDate    = @WorkingDate,   
     @ValueDate    = @WorkingDate,   
     @Amount     = @Amount,  
     @TrxCurrencyID   = @TrxCurrencyID,  
     @InstrumentTypeID  = @InstrumentTypeID,  
     @ChequeID    = @ChequeID,  
     @ChequeDate    = @ChequeDate,  
     @ReferenceNo   = @ReferenceNo,  
     @Remarks    = @Remarks,  
     @TrxDescriptionID  = @TrxDescriptionID,  
     @TrxDescription   = @InTrxDescription,  
     @MainGLID    = @MainGLID,  
     @ContraGLID    = @ContraGLID,  
     @TrxFlagID    = '',  
     @ImageID    = @ImageID,  
     @TrxPrinted    = @TrxPrinted,  
     @ChequeDigit   = @ChequeDigit,  
     @VoucherCode   = @VoucherCode,  
     @ReturnCodeID   = @OriginalReturnCode,  
     @Commission    = @Commission,  
     @TheirCommission  = @TheirCommission,  
     @VATPINNo    = @VATPINNo,  
     @VATPAYType    = @VATPAYType,  
     @VATSerialNo   = @VATSerialNo,  
     @VATPAYEMonth   = @VATPAYEMonth,  
     @VATPAYECommission  = @VATPAYECommission,  
     @BankID     = @BankID,  
     @BranchID    = @BranchID,  
     @DrawerOrPayeeAccountID = @DrawerOrPayeeAccountID,  
     @DrawerOrPayee   = @DrawerOrPayee,  
     @CreatedBy    = @OperatorID,  
     @NewRecord    = @NewRecord,  
     @TrxRowID    = @TrxRowID,  
     @ForwardRemark   = @ForwardRemark,  
     @SupervisedBy   = 'SYS'  
  
  
     EXEC p_AddOutgoingTrx  
     @TrxBranchID = @TrxBranchID,  
     @TrxBatchID = @TrxBatchID OUTPUT,  
     @TrxBatchSLNo = 0,  
     @SerialID = @SerialID  OUTPUT,  
     @OurBranchID = @OurBranchID,  
     @AccountTypeID = @AccountTypeID,  
     @AccountID = @AccountID,  
     @ProductID = @ProductID,   
     @ModuleID = @OutModuleID,  
     @TrxTypeID = @OutTrxTypeID,  
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
     @Remarks = 'Auto Unpaid',  
     @TrxDescriptionID = '088',-- @TrxDescriptionID,  
     @TrxDescription = @OutTrxDescription,  
     @MainGLID = @MainGLID,  
     @ContraGLID = @ContraGLID,  
     @TrxFlagID = '',  
     @ImageID = 0,  
     @TrxPrinted = 0,  
     @CreatedBy = 'SYS',  
     @NewRecord = 1,  
     @ChequeDigit = 0,  
     @VoucherCode = @VoucherCode,  
     @ReturnCodeID = @ReturnCodeID,  
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
     @SupervisedBy = 'SYS'  
  
  
  
     IF @InTrxTypeID = 'ID' AND @VoucherCode <> '40'  
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
     IF @InTrxTypeID = 'IC'   
     BEGIN  
      SELECT @TrxrowID = TrxRowID FROM t_Transaction WHERE  Try_parse(ReferenceNo AS bigint) = @ColumnID AND ModuleID ='3070' AND TrxTypeID = 'OD'  
      UPDATE  t_TrxClearing SET TrxRowID = @TrxrowID WHERE TrxType IN ('OD') AND ColumnID = @ColumnID AND isNull(TrxrowID,0) = 0   
     END  
  
  
   COMMIT TRAN UNP  
   END TRY  
   BEGIN CATCH  
    ROLLBACK TRAN UNP  
    --DECLARE @ErrorCode VARCHAR(100)  
    --SELECT @ErrorCode = ERROR_MESSAGE()  
    --RAISERROR (@ErrorCode,18,1)   
    RETURN  
   END CATCH  
  
     
       
  
  END  
  RETURN  
 SET NOCOUNT OFF  
End  
  
  
  