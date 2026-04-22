ALTER  PROCEDURE [dbo].[p_PostUploadFileData] (  
 @OurBranchID BranchID  
 ,@Narration VARCHAR(200)  
 ,@FormatID VARCHAR(10)  
 ,@CompanyID VARCHAR(500) = NULL  
 ,@Accounts XML = NULL  
 ,@IsPosting INT = 0  
 ,@OperatorID VARCHAR(30) = ''  
 ,--  
 @PostingSerial INT = 0  
 ,@ErrorNo1 VARCHAR(50) = '0' OUTPUT  
 ,@DontRaiseError BIT = 0  
 )  
AS  
BEGIN  
 --Return  
 --Select @OurBranchID, @Narration, @FormatID, @CompanyID, @Accounts, @IsPosting, @OperatorID, @PostingSerial  
 SET NOCOUNT ON  
 SET @DontRaiseError = ISNULL(@DontRaiseError, 0)  
  
 DECLARE @CurrencyID CurrencyID  
  ,@CrContraAccountType CHAR(1)  
  ,@CrContraAccountID AccountID  
  ,@DbContraAccountType CHAR(1)  
  ,@DbContraAccountID AccountID  
  ,@DrTrxDescriptionID VARCHAR(5)  
  ,@CrTrxDescriptionID VARCHAR(5)  
  ,@ProductID ProductID  
  ,@InValidAccountType CHAR(1)  
  ,@InvalidProductID ProductID  
  ,@InvalidAccountID AccountID  
  ,@InvalidNarration Description  
  ,@OurBankID BankID  
  ,@BankID BankID  
  ,@TrxDate SMALLDATETIME  
  ,@ValueDate SMALLDATETIME  
  ,@TotalSumUpload Amount  
  ,@ErrorNo VARCHAR(200)  
  ,@ModuleID VARCHAR(5)  
  ,@TrxBatchID VARCHAR(8)  
  ,@SerialID INT  
  ,@ClearingTrxBranchID BranchID  
  ,@Company VARCHAR(10)  
  ,@ServerPath VARCHAR(250)  
  ,@LocalPath VARCHAR(250)  
  ,@TrxType VARCHAR(2)  
  ,@PostToBranchID BranchID  
  ,@IsTrxAllow BIT  
  ,@UseThisBranch BranchID  
  ,@RandomNum VARCHAR(20)  
 DECLARE @Transactions TABLE (  
  ColumnID BIGINT IDENTITY(1, 1)  
  ,TrxBranchID VARCHAR(20)  
  ,OurBranchID VARCHAR(20)  
  ,AccountTypeID CHAR(1) NULL  
  ,AccountID VARCHAR(40)  
  ,ProductID VARCHAR(20) NULL  
  ,ModuleID SMALLINT  
  ,TrxCodeID TINYINT  
  ,TrxTypeID CHAR(2)  
  ,TrxDate SMALLDATETIME  
  ,ValueDate SMALLDATETIME  
  ,Amount Amount  
  ,LocalAmount Amount  
  ,TrxCurrencyID CurrencyID  
  ,TrxAmount Amount  
  ,ExchangeRate CurrencyRate  
  ,MeanRate CurrencyRate  
  ,Profit Amount  
  ,InstrumentTypeID CHAR(1)  
  ,ChequeID NUMERIC  
  ,ChequeDate SMALLDATETIME NULL  
  ,ReferenceNo NVARCHAR(15)  
  ,Remarks Remarks  
  ,TrxDescriptionID NVARCHAR(10)  
  ,TrxDescription Description NULL  
  ,MainGLID AccountID NULL  
  ,ContraGLID AccountID NULL  
  ,TrxFlagID CHAR(1)  
  ,ImageID BIGINT  
  ,TrxPrinted TINYINT  
  ,IsTrxPending BIT  
  ,CreatedBy OperatorID  
  ,CreatedOn SMALLDATETIME  
  )  
 DECLARE @ClearingTransactions TABLE (  
  ColumnID BIGINT IDENTITY(1, 1)  
  ,TrxBranchID BranchID  
  ,OurBranchID BranchID  
  ,AccountTypeID CHAR(1) NULL  
  ,AccountID AccountID  
  ,ProductID ProductID NULL  
  ,ModuleID SMALLINT  
  ,TrxCodeID TINYINT  
  ,TrxTypeID CHAR(2)  
  ,TrxDate SMALLDATETIME  
  ,ValueDate SMALLDATETIME  
  ,Amount Amount  
  ,LocalAmount Amount  
  ,TrxCurrencyID CurrencyID  
  ,TrxAmount Amount  
  ,ExchangeRate CurrencyRate  
  ,MeanRate CurrencyRate  
  ,Profit Amount  
  ,InstrumentTypeID CHAR(1)  
  ,ChequeID NUMERIC  
  ,ChequeDate SMALLDATETIME NULL  
  ,ReferenceNo NVARCHAR(15)  
  ,Remarks Remarks  
  ,TrxDescriptionID NVARCHAR(10)  
  ,TrxDescription Description NULL  
  ,MainGLID AccountID NULL  
  ,ContraGLID AccountID NULL  
  ,TrxFlagID CHAR(1)  
  ,ImageID BIGINT  
  ,TrxPrinted TINYINT  
  ,IsTrxPending BIT  
  ,CreatedBy OperatorID  
  ,CreatedOn SMALLDATETIME  
  )  
 DECLARE @Staging TABLE (  
  OurBranchID BranchID NULL  
  ,AccountID AccountID NULL  
  ,AccountTypeID CHAR(1) NULL  
  ,EmployeeID Description NULL  
  ,Amount Amount NULL  
  ,ProductID ProductID NULL  
  ,TrxTypeID VARCHAR(5) NULL  
  ,StatusCode CHAR(3)  
  ,FeeCode VARCHAR(10)  
  ,TrxDescription VARCHAR(300)  
  )  
 DECLARE @ClearingStaging TABLE (  
  OurBranchID BranchID NULL  
  ,AccountID AccountID NULL  
  ,AccountTypeID CHAR(1) NULL  
  ,EmployeeID Description NULL  
  ,Amount Amount NULL  
  ,ProductID ProductID NULL  
  ,TrxTypeID VARCHAR(5) NULL  
  ,StatusCode CHAR(3)  
  ,FeeCode VARCHAR(10)  
  ,TrxDescription VARCHAR(300)  
  )  
 DECLARE @Contra TABLE (  
  OurBranchID BranchID NULL  
  ,Amount Amount NULL  
  ,ContraAmount Amount NULL  
  )  
 DECLARE @disBranches TABLE (  
  ColID INT IDENTITY(1, 1)  
  ,OurBranchID BranchID  
  )  
 DECLARE @BatchID TABLE (BatchId VARCHAR(10))  
  
 CREATE TABLE #ATMFeeCode (  
  ColID INT IDENTITY(1, 1)  
  ,FeeCode VARCHAR(10)  
  ,GLAccountID VARCHAR(25) NULL  
  ,Narration VARCHAR(50) NULL  
  )  
  
 DECLARE @TotalTran TABLE (  
  ColumnID BIGINT IDENTITY(1, 1)  
  ,TrxBranchID BranchID  
  ,OurBranchID BranchID  
  ,AccountTypeID CHAR(1) NULL  
  ,AccountID AccountID  
  ,ProductID ProductID NULL  
  ,ModuleID SMALLINT  
  ,TrxCodeID TINYINT  
  ,TrxTypeID CHAR(2)  
  ,TrxDate DATETIME  
  ,ValueDate DATETIME  
  ,Amount Amount  
  ,LocalAmount Amount  
  ,TrxCurrencyID CurrencyID  
  ,TrxAmount Amount  
  ,ExchangeRate CurrencyRate  
  ,MeanRate CurrencyRate  
  ,Profit Amount  
  ,InstrumentTypeID CHAR(1)  
  ,ChequeID NUMERIC  
  ,ChequeDate DATETIME NULL  
  ,ReferenceNo NVARCHAR(15)  
  ,Remarks Remarks  
  ,TrxDescriptionID NVARCHAR(10)  
  ,TrxDescription Description NULL  
  ,MainGLID AccountID NULL  
  ,ContraGLID AccountID NULL  
  ,TrxFlagID CHAR(1)  
  ,ImageID BIGINT  
  ,TrxPrinted TINYINT  
  ,IsTrxPending BIT  
  ,CreatedBy OperatorID  
  ,CreatedOn DATETIME  
  )  
 --#####################################################################     
 --CHECK IF THE SAME USER IS SAVING THE SAME FILE AGAIN. MULTIPLE OPTIONS TO BE USED FOR THE CHECK  
 --THIS SECTION MAY BE EXPANDED TO INCLUDE MANY OTHER CHECKS IN FUTURE  
 --THE LOGIC ALSO CHANGES SLIGHLTY BY INSERTING INTO t_GatewayUpload THEN UPDATING LATER. NOT A GOOD PRACTICE THOUGH  
 DECLARE @ProcedureName VARCHAR(100)  
  
 /* Select @ProcedureName  = ProcedureName  
 From t_FileFormat(Nolock)  
 Where FormatID= @FormatID  
 IF @FormatID='EOM' AND @IsPosting in (1,2)  
 BEGIN  
  
  
  EXEC p_PostJVUploadTrx  
  @OurBranchID = @OurBranchID,        
  @Narration  = @Narration,        
  @FormatID  = @FormatID,        
  @CompanyID   = @CompanyID,      
  @Accounts   = @Accounts,    
  @IsPosting   = @IsPosting,      
  @OperatorID  = @OperatorID,  
  @PostingSerial   = @PostingSerial,  
  @ErrorNo1   = @ErrorNo1 OUTPUT,  
  @DontRaiseError  = @DontRaiseError   
  RETURN  
  
 END  
 IF ISNULL(@ProcedureName,'')='NWSC'  
 BEGIN  
     
  RETURN  
 END */  
 SELECT @Company = CompanyID  
  ,@ServerPath = ServerPath  
  ,@LocalPath = LocalPath  
 FROM dbo.f_GatewayStringDisect(@CompanyID)  
  
 --SELECT @Company, @ServerPath, @LocalPath   
 SET @BankID = dbo.f_GetBankID(@OurBranchID)  
 SET @TrxType = ISNULL(@TrxType, 'N')  
  
 SELECT @ClearingTrxBranchID = @OurBranchID  
  
 --select @OurBranchID  
 --Return  
--  
  
 IF @TrxType <> 'B'  
 BEGIN   
  SELECT TOP 1 @CurrencyID = t_GatewayContraAccountDetail.CurrencyID  
   ,@CrContraAccountType = CrContraAccountType  
   ,@CrContraAccountID = CrContraAccountID  
   ,@DbContraAccountType = DbContraAccountType  
   ,@DbContraAccountID = DbContraAccountID  
   ,@DrTrxDescriptionID = DrTrxDescriptionID  
   ,@CrTrxDescriptionID = CrTrxDescriptionID  
  FROM t_FileFormat(NOLOCK)  
  INNER JOIN t_GatewayContraAccountDetail(NOLOCK) ON t_FileFormat.FormatID = t_GatewayContraAccountDetail.FormatID  
  WHERE t_FileFormat.FormatID = @FormatID  
   AND t_GatewayContraAccountDetail.OurBranchID = @OurBranchID  
  
 END  
 ELSE  
 BEGIN  
   
  SET @CurrencyID = dbo.f_GetLocalCurrencyID(@OurBranchID)  
  
  SELECT TOP 1 @DrTrxDescriptionID = DrTrxDescriptionID  
   ,@CrTrxDescriptionID = CrTrxDescriptionID  
  FROM t_FileFormat(NOLOCK)  
  INNER JOIN t_GatewayContraAccountDetail(NOLOCK) ON t_FileFormat.FormatID = t_GatewayContraAccountDetail.FormatID  
  WHERE t_FileFormat.FormatID = @FormatID --- AND t_GatewayContraAccountDetail.OurBranchID = @OurBranchID       
 END  
  
 --Update t_GatewayContraAccountDetail Set CrTrxDescriptionID='084' Where FormatID = 'NTU'  
 --SELECT * FROM t_FileFormat (Nolock)       
 -- INNER JOIN t_GatewayContraAccountDetail (NOLOCK) ON t_FileFormat.FormatID = t_GatewayContraAccountDetail.FormatID      
 -- WHERE t_FileFormat.FormatID = 'NTU'  
  
  
 IF isNULL(@CurrencyID, '') = ''  
 BEGIN  
  IF (  
    SELECT Count(CurrencyID)  
    FROM t_FileFormat(NOLOCK)  
    WHERE BankID = @BankID  
     AND FormatID = @FormatID  
    ) = 1  
   SELECT @CurrencyID = CurrencyID  
   FROM t_FileFormat(NOLOCK)  
   WHERE BankID = @BankID  
    AND FormatID = @FormatID  
 END  
  
 SELECT @RandomNum = 'GtyUpld-' + CAST(CAST(CRYPT_GEN_RANDOM(2) AS BIGINT) AS VARCHAR(20))  
  
  
 SELECT @TrxType = TransactionType  
 FROM t_FileFormat(NOLOCK)  
 WHERE BankID = @BankID  
  AND FormatID = @FormatID  
  AND CurrencyID = @CurrencyID  
    
 --SELECT @TrxType  
 IF (  
   SELECT COUNT(1)  
   FROM t_GatewayUpload(NOLOCK)  
   WHERE ServerPath = @ServerPath  
    AND LocalPath = @LocalPath  
    AND IsNull(TrxBatchID, '') <> ''  
   ) > 0  
 BEGIN  
  IF @DontRaiseError = 1  
  BEGIN  
   SET @ErrorNo1 = '860520'  
  
   RETURN  
  END  
  
  RAISERROR (  
    'BREXDB860520'  
    ,16  
    ,1  
    )  
  
  RETURN  
 END  
 ELSE  
 BEGIN --ADD THE BASIC PARTS TO ENSURE A SMART ERROR WILL NOT PENETRATE  
  DECLARE @ServerP VARCHAR(250)  
  DECLARE @ErrorDescription Description  
  DECLARE @ErrorAccountID AccountID  
  
  SET @ServerP = RIGHT(@ServerPath, LEN(@ServerPath) - 1)  
  SET @ServerP = LEFT(@ServerP, LEN(@ServerP) - 1)  
  --SET @ServerP = @ServerPath  
      
  SELECT TOP 1 @ErrorDescription = EmployeeID  
   ,@ErrorAccountID = AccountID  
  FROM t_PreProcessUpload(NOLOCK)  
  WHERE BankID = @BankID  
   AND CreatedBy = @OperatorID  
   AND FilePath = @ServerP  
   AND ISNULL(EmployeeID, '') <> ''  
     
  SET @IsTrxAllow = 1  
    
  IF ISNULL(@ErrorAccountID, '') <> ''  
  BEGIN  
   --Select @BankID, @OurBranchID, @CurrencyID, @FormatID  
   --CHECK IF INVALID ACCOUNTS HAVE BEEN SET FOR THE BRANCH. IF NOT, FIRE ERROR NOTIFICATION HERE  
   IF (  
     SELECT COUNT(AccountID)  
     FROM t_GatewayInvalidAccountDetail(NOLOCK)  
     WHERE BankID = @BankID  
      AND OurBranchID = @OurBranchID  
      AND CurrencyID = @CurrencyID  
      AND FormatID = @FormatID  
     ) = 0  
   BEGIN  
    SET @ErrorNo1 = '455505'  
    SET @ErrorNo = 'BREXDB455505(' + CAST(@OurBranchID AS VARCHAR(100)) + ')'  
  
    IF @DontRaiseError = 1  
    BEGIN  
     RETURN  
    END  
  
    RAISERROR (  
      @ErrorNo  
      ,16  
      ,1  
      )  
  
    RETURN  
   END  
  END  
    
  IF (  
    SELECT COUNT(StatusCode)  
    FROM t_PreProcessUpload(NOLOCK)  
    WHERE BankID = @BankID  
     AND CreatedBy = @OperatorID  
     AND FilePath = @ServerP  
     AND StatusCode NOT IN ('AA')  
    ) > 0  
  BEGIN  
   IF (  
     SELECT COUNT(AccountID)  
     FROM t_GatewayInvalidAccountDetail(NOLOCK)  
     WHERE BankID = @BankID  
      AND OurBranchID = @OurBranchID  
      AND CurrencyID = @CurrencyID  
      AND FormatID = @FormatID  
     ) = 0  
   BEGIN  
    SET @ErrorNo1 = '455505'  
    SET @ErrorNo = 'BREXDB455505(' + CAST(@OurBranchID AS VARCHAR(100)) + ')'  
  
    IF @DontRaiseError = 1  
    BEGIN  
     RETURN  
    END  
  
    RAISERROR (  
      @ErrorNo  
      ,16  
      ,1  
      )  
  
    RETURN  
   END  
  END  
  
  
  SELECT @InValidAccountType = AccountTagID  
   ,@InvalidAccountID = AccountID  
   ,@InvalidNarration = Narration  
  FROM t_GatewayInvalidAccountDetail(NOLOCK)  
  WHERE BankID = @BankID  
   AND OurBranchID = @OurBranchID  
   AND CurrencyID = @CurrencyID  
   AND FormatID = @FormatID  
  
  IF @InValidAccountType = 'G'  
   SET @InvalidProductID = 'GL'  
  ELSE  
  BEGIN  
   SET @InvalidProductID = dbo.f_GetAccountProductID(@OurBranchID, @InvalidAccountID)  
  END  
  
  SELECT TOP 1 @OurBranchID = TrxBranchID  
  FROM t_PreProcessUpload(NOLOCK)  
  WHERE BankID = @BankID  
   AND CreatedBy = @OperatorID  
   AND FilePath = @ServerP  
  
  SET @TrxDate = dbo.f_GetWorkingDate(@OurBranchID)  
  SET @ValueDate = @TrxDate  
  SET @ModuleID = '3150'  
  
  IF ISNULL(@CurrencyID, '') = ''  
  BEGIN  
   SET @ErrorNo1 = '860512'  
   SET @ErrorNo = 'BREXDB860512(' + CAST(@OurBranchID AS VARCHAR(15)) + ')'  
  
   IF @DontRaiseError = 1  
   BEGIN  
    RETURN  
   END  
  
   RAISERROR (@ErrorNo ,16 ,1)  
  
   RETURN  
  END  
 END  
   
 --#####################################################################   
 IF @FormatID = '002'  
 BEGIN  
  --SELECT 'kamunya',*  
  --FROM t_PreProcessUpload (NOLOCK)  
  -- WHERE RIGHT(BankID,2) = @BankID And CreatedBy = @OperatorID AND FilePath = REPLACE(REPLACE(@ServerP, '{', ''), '}', '')  
  INSERT INTO @Staging (  
   OurBranchID  
   ,AccountID  
   ,EmployeeID  
   ,Amount  
   ,StatusCode  
   ,FeeCode  
   ,TrxTypeID  
   ,TrxDescription  
   )  
  SELECT OurBranchID  
   ,AccountID  
   ,EmployeeID  
   ,Amount  
   ,StatusCode  
   ,isNull(FeeCode, '')  
   ,TrxTypeID  
   ,Description  
  FROM t_PreProcessUpload(NOLOCK)  
  WHERE RIGHT(BankID, 2) = @BankID  
   AND CreatedBy = @OperatorID  
   AND FilePath = REPLACE(REPLACE(@ServerP, '{', ''), '}', '')  
     
  INSERT INTO @ClearingStaging (  
   OurBranchID  
   ,AccountID  
   ,EmployeeID  
   ,Amount  
   ,StatusCode  
   ,FeeCode  
   ,TrxTypeID  
   ,TrxDescription  
   )  
  SELECT OurBranchID  
   ,AccountID  
   ,EmployeeID  
   ,Amount  
   ,StatusCode  
   ,isNull(FeeCode, '')  
   ,TrxTypeID  
   ,Description  
  FROM t_PreProcessUpload  
  WHERE RIGHT(BankID, 2) <> @BankID  
   AND CreatedBy = @OperatorID  
   AND FilePath = REPLACE(REPLACE(@ServerP, '{', ''), '}', '')  
     
 END  
 ELSE  
 BEGIN  
  INSERT INTO @Staging (  
   OurBranchID  
   ,AccountID  
   ,EmployeeID  
   ,Amount  
   ,StatusCode  
   ,FeeCode  
   ,TrxTypeID  
   ,TrxDescription  
   )  
  SELECT OurBranchID  
   ,AccountID  
   ,EmployeeID  
   ,Amount  
   ,StatusCode  
   ,isNull(FeeCode, '')  
   ,TrxTypeID  
   ,Description  
  FROM t_PreProcessUpload(NOLOCK)  
  WHERE BankID = @BankID  
   AND CreatedBy = @OperatorID  
   AND FilePath = REPLACE(REPLACE(@ServerP, '{', ''), '}', '')  
  
  INSERT INTO @ClearingStaging (  
   OurBranchID  
   ,AccountID  
   ,EmployeeID  
   ,Amount  
   ,StatusCode  
   ,FeeCode  
   ,TrxTypeID  
   ,TrxDescription  
   )  
  SELECT OurBranchID  
   ,AccountID  
   ,EmployeeID  
   ,Amount  
   ,StatusCode  
   ,isNull(FeeCode, '')  
   ,TrxTypeID  
   ,Description  
  FROM t_PreProcessUpload  
  WHERE BankID <> @BankID  
   AND CreatedBy = @OperatorID  
   AND FilePath = REPLACE(REPLACE(@ServerP, '{', ''), '}', '')  
 END  
  
 --SELECT @BankID, @ServerP  
 --SELECT @ServerP, OurBranchID,AccountID,EmployeeID,Amount,StatusCode,isNull(FeeCode,''),TrxTypeID,Description, *  
 --FROM t_PreProcessUpload (NOLOCK)  
 --WHERE CreatedBy = @OperatorID AND  BankID = @BankID  --AND FilePath = REPLACE(REPLACE(@ServerP, '{', ''), '}', '')  
 --   RETURN  
 --INSERT INTO @Staging(OurBranchID,AccountID,EmployeeID,Amount,StatusCode,FeeCode,TrxTypeID,TrxDescription)  
 --SELECT OurBranchID,AccountID,EmployeeID,Amount,StatusCode,isNull(FeeCode,''),TrxTypeID,Description  
 --FROM t_PreProcessUpload (NOLOCK)  
 --WHERE BankID = @BankID And CreatedBy = @OperatorID AND FilePath = REPLACE(REPLACE(@ServerP, '{', ''), '}', '')  
 --INSERT INTO @ClearingStaging(OurBranchID,AccountID,EmployeeID,Amount,StatusCode,FeeCode,TrxTypeID,TrxDescription)  
 --SELECT OurBranchID,AccountID,EmployeeID,Amount,StatusCode,isNull(FeeCode,''),TrxTypeID,Description  
 --FROM t_PreProcessUpload   
 --WHERE BankID <> @BankID And CreatedBy = @OperatorID AND FilePath =  REPLACE(REPLACE(@ServerP, '{', ''), '}', '')  
      
 UPDATE @Staging  
 SET AccountTypeID = 'C'  
  ,ProductID = t_AccountCustomer.ProductID  
 FROM @Staging Staging  
 INNER JOIN t_AccountCustomer(NOLOCK) ON Staging.OurBranchID = t_AccountCustomer.OurBranchID  
  AND Staging.AccountID = t_AccountCustomer.AccountID  
  
 UPDATE @Staging  
 SET AccountTypeID = 'G'  
  ,ProductID = 'GL'  
 FROM @Staging Staging  
 INNER JOIN t_GLBranch(NOLOCK) ON Staging.OurBranchID = t_GLBranch.OurBranchID  
  AND Staging.AccountID = t_GLBranch.AccountID  
  
  
 IF (IsNull(@IsPosting, 0) = 0)  
  OR (@IsPosting = 2)  
 BEGIN  
  INSERT INTO t_GatewayUpload (  
   trxBatchID  
   ,OurBranchID  
   ,AccountID  
   ,Amount  
   ,FormatID  
   ,OperatorID  
   ,WorkingDate  
   ,CreatedOn  
   ,EmpLoyeeID  
   ,Accounts  
   ,CompanyID  
   ,ServerPath  
   ,LocalPath  
   ,Narration  
   )  
  VALUES (  
   ''  
   ,@OurBranchID  
   ,''  
   ,''  
   ,@FormatID  
   ,@OperatorID  
   ,@TrxDate  
   ,GETDATE()  
   ,''  
   ,''  
   ,@Company  
   ,@ServerPath  
   ,@LocalPath  
   ,@Narration  
   )  
 END  
  
 INSERT INTO @disBranches (OurBranchID)  
 SELECT DISTINCT OurBranchID  
 FROM @Staging  
  
 DECLARE @ATMID INT  
  ,@ATMCount INT  
  ,@FeeCode VARCHAR(10)  
  
 SELECT TOP 1 @PostToBranchID = OurBranchID  
 FROM @disBranches --WHERE ColID = @RowID  
  
 DELETE  
 FROM @Transactions  
  
 DELETE #ATMFeeCode  
  
 SELECT TOP 1 @IsTrxAllow = IsTrxAllow  
  ,@PostToBranchID = OurBranchID  
 FROM t_SystemBranchStatus(NOLOCK)  
 WHERE OurBranchID IN (  
   SELECT DISTINCT OurBranchID  
   FROM @Staging  
   )  
  AND ISNULL(IsTrxAllow, 0) = 0  
  
 --FOR BATCH ALLOW EVEN AFTER COB IS DONE BUT NOT AFTER EOD  
 IF ISNULL(@IsTrxAllow, 0) = 0  
 BEGIN  
  IF (  
    SELECT Count(1)  
    FROM t_SystemBranchStatus(NOLOCK)  
    WHERE OurBranchID IN (  
      SELECT DISTINCT OurBranchID  
      FROM @Staging  
      )  
     AND EODDate >= SODDate  
    ) > 0  
  BEGIN  
   SET @ErrorNo1 = '300046'  
   SET @ErrorNo = 'BREXDB300046(' + @PostToBranchID + ')'  
  
   IF @DontRaiseError = 1  
   BEGIN  
    RETURN  
   END  
  
   RAISERROR (  
     @ErrorNo  
     ,16  
     ,1  
     )  
  
   RETURN  
  END  
  
  SET @IsTrxAllow = 1  
 END  
  
 SET @TrxDate = dbo.f_GetWorkingDate(@OurBranchID)  
 SET @ValueDate = @TrxDate  
 SET @BankID = dbo.f_GetBankID(@OurBranchID)  
 SET @IsTrxAllow = 1  
  
  
  
 --CAPTURE ALL THE NECCESSARY INFORMATION FOR POSTING A TRANSACTION        
 INSERT INTO @Transactions (  
  TrxBranchID  
  ,OurBranchID  
  ,AccountTypeID  
  ,AccountID  
  ,ProductID  
  ,ModuleID  
  ,TrxCodeID  
  ,TrxTypeID  
  ,TrxDate  
  ,ValueDate  
  ,Amount  
  ,LocalAmount  
  ,TrxCurrencyID  
  ,TrxAmount  
  ,ExchangeRate  
  ,MeanRate  
  ,Profit  
  ,InstrumentTypeID  
  ,ChequeID  
  ,ChequeDate  
  ,ReferenceNo  
  ,Remarks  
  ,TrxDescriptionID  
  ,TrxDescription  
  ,MainGLID  
  ,TrxFlagID  
  ,ImageID  
  ,TrxPrinted  
  ,IsTrxPending  
  ,CreatedBy  
  ,CreatedOn  
  )  
 SELECT @OurBranchID   
  ,OurBranchID  
  ,AccountTypeID  
  ,AccountID  
  ,ProductID  
  ,@ModuleID  
  ,0  
  ,TrxTypeID  
  ,@TrxDate  
  ,@ValueDate  
  ,Amount  
  ,Amount  
  ,@CurrencyID  
  ,Amount  
  ,1  
  ,1  
  ,0  
  ,'V'  
  ,0  
  ,NULL  
  ,''  
  ,FeeCode  
  ,@CrTrxDescriptionID  
  ,ISNULL(TrxDescription, '') + ' : ' + @Narration  
  ,CASE   
   WHEN ProductID = 'GL'  
    THEN AccountID  
   ELSE dbo.f_GetGLInterfaceAccountID(@BankID, ProductID, dbo.f_GetProductTypeID(@BankID, ProductID), 'CONTROL_AC')  
   END  
  ,''  
  ,0  
  ,0  
  ,0  
  ,@OperatorID  
  ,GETDATE()  
 FROM @Staging  
 WHERE StatusCode = 'AA' --Where OurBranchID = @PostToBranchID AND StatusCode IN('AA')--,'AD')    
   
 UNION ALL  
   
 SELECT @OurBranchID  
  ,@OurBranchID  
  ,@InValidAccountType  
  ,@InvalidAccountID  
  ,@InvalidProductID  
  ,@ModuleID  
  ,0  
  ,TrxTypeID  
  ,@TrxDate  
  ,@ValueDate  
  ,Amount  
  ,Amount  
  ,@CurrencyID  
  ,Amount  
  ,1  
  ,1  
  ,0  
  ,'V'  
  ,0  
  ,NULL  
  ,''  
  ,FeeCode  
  ,@CrTrxDescriptionID  
  ,ISNULL(TrxDescription, '') + ' : ' + @InvalidNarration + ' - ' + AccountID + ' : ' + EmployeeID  
  ,CASE   
   WHEN @InValidAccountType = 'G'  
    THEN @InvalidAccountID  
   ELSE dbo.f_GetGLInterfaceAccountID(@BankID, @InvalidProductID, dbo.f_GetProductTypeID(@BankID, @InvalidProductID), 'CONTROL_AC')  
   END  
  ,''  
  ,0  
  ,0  
  ,0  
  ,@OperatorID  
  ,GETDATE()  
 FROM @Staging  
 WHERE StatusCode <> 'AA'  
 ORDER BY TrxTypeID DESC  
   
  
 INSERT INTO @ClearingTransactions (  
  TrxBranchID  
  ,OurBranchID  
  ,AccountTypeID  
  ,AccountID  
  ,ProductID  
  ,ModuleID  
  ,TrxCodeID  
  ,TrxTypeID  
  ,TrxDate  
  ,ValueDate  
  ,Amount  
  ,LocalAmount  
  ,TrxCurrencyID  
  ,TrxAmount  
  ,ExchangeRate  
  ,MeanRate  
  ,Profit  
  ,InstrumentTypeID  
  ,ChequeID  
  ,ChequeDate  
  ,ReferenceNo  
  ,Remarks  
  ,TrxDescriptionID  
  ,TrxDescription    ,MainGLID  
  ,TrxFlagID  
  ,ImageID  
  ,TrxPrinted  
  ,IsTrxPending  
  ,CreatedBy  
  ,CreatedOn  
  )  
 SELECT @OurBranchID  
  ,OurBranchID  
  ,isNull(@InValidAccountType, 'G')  
  ,isnull(@InvalidAccountID, '1234567')  
  ,@InvalidProductID  
  ,@ModuleID  
  ,0  
  ,'TC'  
  ,@TrxDate  
  ,@ValueDate  
  ,Amount  
  ,Amount  
  ,@CurrencyID  
  ,Amount  
  ,1  
  ,1  
  ,0  
  ,'V'  
  ,0  
  ,NULL  
  ,''  
  ,FeeCode  
  ,@CrTrxDescriptionID  
  ,ISNULL(TrxDescription, '') + ' : ' + @InvalidNarration + ' - ' + AccountID + ' : ' + EmployeeID  
  ,CASE   
   WHEN @InValidAccountType = 'G'  
    THEN @InvalidAccountID  
   ELSE dbo.f_GetGLInterfaceAccountID(@BankID, @InvalidProductID, dbo.f_GetProductTypeID(@BankID, @InvalidProductID), 'CONTROL_AC')  
   END  
  ,''  
  ,0  
  ,0  
  ,0  
  ,@OperatorID  
  ,GETDATE()  
 FROM @ClearingStaging --Where StatusCode <> 'AA'        
 ORDER BY TrxTypeID DESC  
  
 --CHECK IF WE HAVE DEFINED CONTROL ACCOUNTS FOR ALL PRODUCTS  
 --SELECT @ProductID = ProductID  
 --FROM @Transactions  
 --WHERE ISNULL(MainGLID, '') = ''  
   
  
 IF EXISTS (SELECT 1 FROM @Transactions WHERE ISNULL(MainGLID, '') = '')-- ISNULL(@ProductID, '') <> ''  
 BEGIN  
  SET @ErrorNo1 = '250005'  
  SET @ErrorNo = 'BREXDB250005'  
  
  IF @DontRaiseError = 1  
  BEGIN  
   RETURN  
  END  
  
  RAISERROR ( @ErrorNo,16 ,1)  
  
  RETURN  
 END  
  
 IF @FormatID = '002'  
 BEGIN  
  --SELECT  @OurBranchID,'Kamunya0', * FROM @Transactions  
  DECLARE   
   @TotalCR Amount  
   ,@ClrAccID AccountID  
  
  SELECT @ClrAccID = dbo.f_GetCurrencyBranchGLAccountID(ISNULL(TrxBranchID, @OurBranchID), isNULL(TrxCurrencyID, 'TZS'), 'CEN_BANK_AC')  
  FROM @Transactions  
  WHERE TrxTypeID = 'TD'  
  
  --SELECT @ClrAccID, 'Kamunya'  
  SELECT @TotalCR = SUM(isnull(LocalAmount, 0))  
  FROM @ClearingTransactions     
  WHERE TrxTypeID = 'TC'  
  
  INSERT INTO @Transactions (  
   TrxBranchID  
   ,OurBranchID  
   ,AccountTypeID  
   ,AccountID  
   ,ProductID  
   ,ModuleID  
   ,TrxCodeID  
   ,TrxTypeID  
   ,TrxDate  
   ,ValueDate  
   ,Amount  
   ,LocalAmount  
   ,TrxCurrencyID  
   ,TrxAmount  
   ,ExchangeRate  
   ,MeanRate  
   ,Profit  
   ,InstrumentTypeID  
   ,ChequeID  
   ,ChequeDate  
   ,ReferenceNo  
   ,Remarks  
   ,TrxDescriptionID  
   ,TrxDescription  
   ,MainGLID  
   ,TrxFlagID  
   ,ImageID  
   ,TrxPrinted  
   ,IsTrxPending  
   ,CreatedBy  
   ,CreatedOn  
   )  
  SELECT TrxBranchID   
   ,'000' --OurBranchID izo  
   ,'G'  
   ,@ClrAccID  
   ,'GL'  
   ,ModuleID  
   ,TrxCodeID  
   ,'TC'  
   ,TrxDate  
   ,ValueDate  
   ,@TotalCR  
   ,@TotalCR  
   ,TrxCurrencyID  
   ,@TotalCR  
   ,ExchangeRate  
   ,MeanRate  
   ,Profit  
   ,'V'  
   ,0  
   ,ChequeDate  
   ,ReferenceNo  
   ,Remarks  
   ,TrxDescriptionID  
   ,TrxDescription  
   ,MainGLID  
   ,TrxFlagID  
   ,ImageID  
   ,TrxPrinted  
   ,IsTrxPending  
   ,CreatedBy  
   ,CreatedOn  
  FROM @Transactions  
  WHERE TrxTypeID = 'TD'  
  
   
 END  
  
 -- THROW AN ERROR MESSAGE IF THE FORMAT TYPE IS B AND THE ENTRIES IN THE FILE ARE NOT BALANCING  
 IF @FormatID = '002'  
 BEGIN  
  INSERT INTO @TotalTran (  
   TrxBranchID  
   ,OurBranchID  
   ,AccountTypeID  
   ,AccountID  
   ,ProductID  
   ,ModuleID  
   ,TrxCodeID  
   ,TrxTypeID  
   ,TrxDate  
   ,ValueDate  
   ,Amount  
   ,LocalAmount  
   ,TrxCurrencyID  
   ,TrxAmount  
   ,ExchangeRate  
   ,MeanRate  
   ,Profit  
   ,InstrumentTypeID  
   ,ChequeID  
   ,ChequeDate  
   ,ReferenceNo  
   ,Remarks  
   ,TrxDescriptionID  
   ,TrxDescription  
   ,MainGLID  
   ,ContraGLID  
   ,TrxFlagID  
   ,ImageID  
   ,TrxPrinted  
   ,IsTrxPending  
   ,CreatedBy  
   ,CreatedOn  
   )  
  SELECT TrxBranchID  
   ,OurBranchID  
   ,AccountTypeID  
   ,AccountID  
   ,ProductID  
   ,ModuleID  
   ,TrxCodeID  
   ,TrxTypeID  
   ,TrxDate  
   ,ValueDate  
   ,Amount  
   ,LocalAmount  
   ,TrxCurrencyID  
   ,TrxAmount  
   ,ExchangeRate  
   ,MeanRate  
   ,Profit  
   ,InstrumentTypeID  
   ,ChequeID  
   ,ChequeDate  
   ,ReferenceNo  
   ,Remarks  
   ,TrxDescriptionID  
   ,TrxDescription  
   ,MainGLID  
   ,ContraGLID  
   ,TrxFlagID  
   ,ImageID  
   ,TrxPrinted  
   ,IsTrxPending  
   ,CreatedBy  
   ,CreatedOn  
  FROM @Transactions  
  WHERE AccountID <> dbo.f_GetCurrencyBranchGLAccountID(OurBranchID, TrxCurrencyID, 'CEN_BANK_AC')  
  
  INSERT INTO @TotalTran (  
   TrxBranchID  
   ,OurBranchID  
   ,AccountTypeID  
   ,AccountID  
   ,ProductID  
   ,ModuleID  
   ,TrxCodeID  
   ,TrxTypeID  
   ,TrxDate  
   ,ValueDate  
   ,Amount  
   ,LocalAmount  
   ,TrxCurrencyID  
   ,TrxAmount  
   ,ExchangeRate  
   ,MeanRate  
   ,Profit  
   ,InstrumentTypeID  
   ,ChequeID  
   ,ChequeDate  
   ,ReferenceNo  
   ,Remarks  
   ,TrxDescriptionID  
   ,TrxDescription  
   ,MainGLID  
   ,ContraGLID  
   ,TrxFlagID  
   ,ImageID  
   ,TrxPrinted  
   ,IsTrxPending  
   ,CreatedBy  
   ,CreatedOn  
   )  
  SELECT TrxBranchID  
   ,OurBranchID  
   ,AccountTypeID  
   ,AccountID  
   ,ProductID  
   ,ModuleID  
   ,TrxCodeID  
   ,TrxTypeID  
   ,TrxDate  
   ,ValueDate  
   ,Amount  
   ,LocalAmount  
   ,TrxCurrencyID  
   ,TrxAmount  
   ,ExchangeRate  
   ,MeanRate  
   ,Profit  
   ,InstrumentTypeID  
   ,ChequeID  
   ,ChequeDate  
   ,ReferenceNo  
   ,Remarks  
   ,TrxDescriptionID  
   ,TrxDescription  
   ,MainGLID  
   ,ContraGLID  
   ,TrxFlagID  
   ,ImageID  
   ,TrxPrinted  
   ,IsTrxPending  
   ,CreatedBy  
   ,CreatedOn  
  FROM @Transactions  
  WHERE AccountID = dbo.f_GetCurrencyBranchGLAccountID(OurBranchID, TrxCurrencyID, 'CEN_BANK_AC')  
    
 END  
 ELSE  
 BEGIN  
  INSERT INTO @TotalTran (  
   TrxBranchID  
   ,OurBranchID  
   ,AccountTypeID  
   ,AccountID  
   ,ProductID  
   ,ModuleID  
   ,TrxCodeID  
   ,TrxTypeID  
   ,TrxDate  
   ,ValueDate  
   ,Amount  
   ,LocalAmount  
   ,TrxCurrencyID  
   ,TrxAmount  
   ,ExchangeRate  
   ,MeanRate  
   ,Profit  
   ,InstrumentTypeID  
   ,ChequeID  
   ,ChequeDate  
   ,ReferenceNo  
   ,Remarks  
   ,TrxDescriptionID  
   ,TrxDescription  
   ,MainGLID  
   ,ContraGLID  
   ,TrxFlagID  
   ,ImageID  
   ,TrxPrinted  
   ,IsTrxPending  
   ,CreatedBy  
   ,CreatedOn  
   )  
  SELECT TrxBranchID  
   ,OurBranchID  
   ,AccountTypeID  
   ,AccountID  
   ,ProductID  
   ,ModuleID  
   ,TrxCodeID  
   ,TrxTypeID  
   ,TrxDate  
   ,ValueDate  
   ,Amount  
   ,LocalAmount  
   ,TrxCurrencyID  
   ,TrxAmount  
   ,ExchangeRate  
   ,MeanRate  
   ,Profit  
   ,InstrumentTypeID  
   ,ChequeID  
   ,ChequeDate  
   ,ReferenceNo  
   ,Remarks  
   ,TrxDescriptionID  
   ,TrxDescription  
   ,MainGLID  
   ,ContraGLID  
   ,TrxFlagID  
   ,ImageID  
   ,TrxPrinted  
   ,IsTrxPending  
   ,CreatedBy  
   ,CreatedOn  
  FROM @Transactions  
 END  
  
  
  
 --INSERT INTO @TotalTran  
 -- (TrxBranchID,OurBranchID,AccountTypeID,AccountID,ProductID,ModuleID,TrxCodeID,TrxTypeID,TrxDate,ValueDate,Amount,LocalAmount,  
 -- TrxCurrencyID,TrxAmount,ExchangeRate,MeanRate,Profit,InstrumentTypeID,ChequeID,ChequeDate,ReferenceNo,Remarks,TrxDescriptionID,  
 -- TrxDescription,MainGLID,ContraGLID,TrxFlagID,ImageID,TrxPrinted,IsTrxPending,CreatedBy,CreatedOn)  
 --SELECT TrxBranchID,OurBranchID,AccountTypeID,AccountID,ProductID,ModuleID,TrxCodeID,TrxTypeID,TrxDate,ValueDate,Amount,LocalAmount,  
 -- TrxCurrencyID,TrxAmount,ExchangeRate,MeanRate,Profit,InstrumentTypeID,ChequeID,ChequeDate,ReferenceNo,Remarks,TrxDescriptionID,  
 -- TrxDescription,MainGLID,ContraGLID,TrxFlagID,ImageID,TrxPrinted,IsTrxPending,CreatedBy,CreatedOn   
 --FROM @Transactions   
 --INSERT INTO @TotalTran  
 -- (TrxBranchID,OurBranchID,AccountTypeID,AccountID,ProductID,ModuleID,TrxCodeID,TrxTypeID,TrxDate,ValueDate,Amount,LocalAmount,  
 -- TrxCurrencyID,TrxAmount,ExchangeRate,MeanRate,Profit,InstrumentTypeID,ChequeID,ChequeDate,ReferenceNo,Remarks,TrxDescriptionID,  
 -- TrxDescription,MainGLID,ContraGLID,TrxFlagID,ImageID,TrxPrinted,IsTrxPending,CreatedBy,CreatedOn)       
 --SELECT TrxBranchID,OurBranchID,AccountTypeID,AccountID,ProductID,ModuleID,TrxCodeID,TrxTypeID,TrxDate,ValueDate,Amount,LocalAmount,  
 -- TrxCurrencyID,TrxAmount,ExchangeRate,MeanRate,Profit,InstrumentTypeID,ChequeID,ChequeDate,ReferenceNo,Remarks,TrxDescriptionID,  
 -- TrxDescription,MainGLID,ContraGLID,TrxFlagID,ImageID,TrxPrinted,IsTrxPending,CreatedBy,CreatedOn   
 --FROM @ClearingTransactions   
  
 SET @DrTrxDescriptionID = ISNULL(@DrTrxDescriptionID, '008')  
 SET @CrTrxDescriptionID = ISNULL(@CrTrxDescriptionID, '007')  
  
 UPDATE @TotalTran  
 SET TrxDescriptionID = '007'  
 WHERE ISNULL(TrxDescriptionID, '') = ''  
  AND TrxTypeID = 'TC'  
  
 UPDATE @TotalTran  
 SET TrxDescriptionID = '008'  
 WHERE ISNULL(TrxDescriptionID, '') = ''  
  AND TrxTypeID = 'TD'  
  
 UPDATE @TotalTran  
 SET Amount = ABS(Amount) * - 1  
  ,LocalAmount = ABS(LocalAmount) * - 1  
  ,TrxAmount = ABS(TrxAmount) * - 1  
 WHERE TrxTypeID = 'TD'  
  
 UPDATE @TotalTran  
 SET TrxDescriptionID = @CrTrxDescriptionID  
 WHERE ISNULL(TrxDescriptionID, '') = ''  
  AND TrxTypeID = 'TC'  
  
 UPDATE @TotalTran  
 SET TrxDescriptionID = @DrTrxDescriptionID  
 WHERE ISNULL(TrxDescriptionID, '') = ''  
  AND TrxTypeID = 'TD'  
  
 UPDATE @TotalTran  
 SET Amount = ABS(Amount) * - 1  
  ,LocalAmount = ABS(LocalAmount) * - 1  
  ,TrxAmount = ABS(TrxAmount) * - 1  
 WHERE TrxTypeID = 'TD'  
  
 IF ((SELECT SUM(LocalAmount) FROM @TotalTran) <> 0 AND @TrxType = 'B')  
 BEGIN  
  --SET @ErrorNo = 'BREXDB454529'  
  DECLARE @TotalDebitsFromTotalTran Amount  
   ,@TotalCreditsFromTotalTran Amount  
  
  SELECT @TotalDebitsFromTotalTran = ISNULL(SUM(CASE   
      WHEN LocalAmount < 0  
       THEN LocalAmount  
      ELSE 0  
      END), 0)  
   ,@TotalCreditsFromTotalTran = ISNULL(SUM(CASE   
      WHEN LocalAmount > 0  
       THEN LocalAmount  
      ELSE 0  
      END), 0)  
  FROM @TotalTran  
    
  SET @ErrorNo1 = '860523'  
  
  SELECT @ErrorNo = 'BREXDB860523(' + CAST(@TotalDebitsFromTotalTran AS VARCHAR(19)) + ')(' + CAST(@TotalCreditsFromTotalTran AS VARCHAR(19)) + ')'  
  
  IF @DontRaiseError = 1  
  BEGIN  
   RETURN  
  END  
  
  RAISERROR (@ErrorNo,16,1)  
  
  RETURN  
 END  
  
    
  select 'total tran2', * from @TotalTran  
 --NO TRX TYPE SPECIFIED      
 IF (  
   SELECT Count(1)  
   FROM @Transactions  
   WHERE ISNULL(TRXtYPEID, '') = ''  
   ) > 0 --- NOT IN ('TD','TC')  
 BEGIN  
  SET @ErrorNo1 = '100105'  
  SET @ErrorNo = @ErrorNo1  
  
  IF @DontRaiseError = 1  
  BEGIN  
   RETURN  
  END  
  
  RAISERROR (  
    @ErrorNo  
    ,16  
    ,1  
    )  
  
  RETURN  
 END  
  
 --NO TRX TYPE SPECIFIED      
 IF (  
   SELECT Count(1)  
   FROM @Transactions  
   WHERE ISNULL(Amount, '') = 0  
   ) > 0 --- NOT IN ('TD','TC')  
 BEGIN  
  SET @ErrorNo1 = '300037'  
  SET @ErrorNo = @ErrorNo1  
  
  IF @DontRaiseError = 1  
  BEGIN  
   RETURN  
  END  
  
  RAISERROR (  
    @ErrorNo  
    ,16  
    ,1  
    )  
  
  RETURN  
 END  
  
  
 --INSERT INTO T_SYSTEMMESSAGE sELECT '100105','en','Some Entries dont have Transaction Type'  
 --SELECT * FROM T_SYSTEMMESSAGE where messageid=860523  
 --RETURN  
 --CHECK IF TRANTYPE IS BALANCED BUT THE TRANSACTIONS ARE BALANCED FOR ALL BRANCHES BUT NOT INDIVIDUAL BRANCHES  
 SELECT OURBRANCHID,Sum(LocalAmount) Amounts  
 INTO #TestTemp  
 FROM @Transactions  
 GROUP BY OurBranchID  
 HAVING Sum(LocalAmount) <> 0  
  
  
 /*  
 IF EXISTS(select 1 from #TestTemp) AND @TrxType = 'B' -- This is done to post to the contra so that each branch balances  
 BEGIN  
  SET @TrxType = 'N'  
  SELECT TOP 1 @CurrencyID    = t_GatewayContraAccountDetail.CurrencyID,        
   @CrContraAccountType = CrContraAccountType,        
   @CrContraAccountID  = CrContraAccountID,        
   @DbContraAccountType = DbContraAccountType,        
   @DbContraAccountID  = DbContraAccountID,        
   @DrTrxDescriptionID  = DrTrxDescriptionID,        
   @CrTrxDescriptionID  = CrTrxDescriptionID        
  FROM t_FileFormat (NOLOCK)        
  INNER JOIN t_GatewayContraAccountDetail (NOLOCK) ON   
   t_FileFormat.FormatID = t_GatewayContraAccountDetail.FormatID        
  WHERE t_FileFormat.FormatID = @FormatID AND t_GatewayContraAccountDetail.OurBranchID = @OurBranchID          
 END  
 */  
  
 IF @TrxType <> 'B'  
 BEGIN  
  ---###################THIS SECTION IS TO BE USED FOR ATM POSTING ONLY. CONTRA GL FOR ATM ARE FETCHED FROM A DIFFERENT TABLE  
  IF @FormatID = 'ATM'  
  BEGIN  
   UPDATE @Transactions  
   SET Amount = - ABS(Amount)  
    ,LocalAmount = - ABS(LocalAmount)  
    ,TrxAmount = - ABS(TrxAmount)  
    ,TrxTypeID = 'TD'  
    ,TrxDescriptionID = '008'  
  
   INSERT INTO #ATMFeeCode (FeeCode)  
   SELECT DISTINCT FeeCode  
   FROM @Staging  
   WHERE OurBranchID = @PostToBranchID  
    AND StatusCode = 'AA'  
  
   IF EXISTS (  
     SELECT 1  
     FROM #ATMFeeCode  
     WHERE ISNULL(GLAccountID, '') = ''  
     ) --SOME GL ACCOUNTS HAVE NOT BEEN DEFINED  
   BEGIN  
    SET @ErrorNo1 = '860512'  
    SET @ErrorNo = 'BREXDB860512(' + CAST(@PostToBranchID AS VARCHAR(15)) + ')'  
  
    IF @DontRaiseError = 1  
    BEGIN  
     RETURN  
    END  
  
    RAISERROR (  
      @ErrorNo  
      ,16  
      ,1  
      )  
  
    RETURN  
   END  
  
   --UPDATE THE TRXDESCRIPTION FOR THE CUSTOMER TRX POSTING   
   UPDATE @Transactions  
   SET TrxDescription = Narration  
   FROM @Transactions TRX  
   INNER JOIN #ATMFeeCode ATMC ON TRX.Remarks = ATMC.FeeCode  
  
   SELECT @ATMCount = COUNT(1)  
   FROM #ATMFeeCode  
  
   SET @ATMID = 1  
  
   WHILE @ATMID <= @ATMCount  
   BEGIN  
    SELECT @FeeCode = FeeCode  
     ,@DbContraAccountID = GLAccountID  
     ,@Narration = Narration + ' - ' + FeeCode  
   FROM #ATMFeeCode  
    WHERE ColID = @ATMID  
  
    SELECT @TotalSumUpload = SUM(Amount)  
    FROM @Staging  
    WHERE OurBranchID = @PostToBranchID  
     AND StatusCode = 'AA'  
     AND FeeCode = @FeeCode  
  
    SET @TotalSumUpload = ABS(@TotalSumUpload)  
  
    --WE SAVE THE CONTRA PART OF THE TRANSACTION HERE        
    INSERT INTO @Transactions (  
     TrxBranchID  
     ,OurBranchID  
     ,AccountTypeID  
     ,AccountID  
     ,ProductID  
     ,ModuleID  
     ,TrxCodeID  
     ,TrxTypeID  
     ,TrxDate  
     ,ValueDate  
     ,Amount  
     ,LocalAmount  
     ,TrxCurrencyID  
     ,TrxAmount  
     ,ExchangeRate  
     ,MeanRate  
     ,Profit  
     ,InstrumentTypeID  
     ,ChequeID  
     ,ChequeDate  
     ,ReferenceNo  
     ,Remarks  
     ,TrxDescriptionID  
     ,TrxDescription  
     ,MainGLID  
     ,TrxFlagID  
     ,ImageID  
     ,TrxPrinted  
     ,IsTrxPending  
     ,CreatedBy  
     ,CreatedOn  
     )  
    SELECT @PostToBranchID  
     ,@PostToBranchID  
     ,@DbContraAccountType  
     ,@DbContraAccountID  
     ,'G'  
     ,@ModuleID  
     ,0  
     ,'TC'  
     ,@TrxDate  
     ,@ValueDate  
     ,@TotalSumUpload  
     ,@TotalSumUpload  
     ,@CurrencyID  
     ,@TotalSumUpload  
     ,1  
     ,1  
     ,0  
     ,'V'  
     ,0  
     ,NULL  
     ,''  
     ,''  
     ,@DrTrxDescriptionID  
     ,@Narration  
     ,CASE @DbContraAccountType  
      WHEN 'G'  
       THEN @DbContraAccountID  
      ELSE dbo.f_GetGLInterfaceAccountID(@BankID, dbo.f_getAccountProductID(@PostToBranchID, @DbContraAccountID), dbo.f_GetProductTypeID(@BankID, dbo.f_getAccountProductID(@PostToBranchID, @DbContraAccountID)), 'CONTROL_AC')  
      END  
    ,''  
    ,0  
    ,0  
    ,0  
    ,@OperatorID  
    ,GETDATE()  
  
    SET @ATMID = @ATMID + 1  
   END  
  END  
  ELSE ---############NORMAL POSTING. SYSTEM TO BEHAVE AS USUAL  
  BEGIN  
    
   INSERT INTO @Transactions (  
    TrxBranchID  
    ,OurBranchID  
    ,AccountTypeID  
    ,AccountID  
    ,ProductID  
    ,ModuleID  
    ,TrxCodeID  
    ,TrxTypeID  
    ,TrxDate  
    ,ValueDate  
    ,Amount  
    ,LocalAmount  
    ,TrxCurrencyID  
    ,TrxAmount  
    ,ExchangeRate  
    ,MeanRate  
    ,Profit  
    ,InstrumentTypeID  
    ,ChequeID  
    ,ChequeDate  
    ,ReferenceNo  
    ,Remarks  
    ,TrxDescriptionID  
    ,TrxDescription  
    ,MainGLID  
    ,TrxFlagID  
    ,ImageID  
    ,TrxPrinted  
    ,IsTrxPending  
    ,CreatedBy  
    ,CreatedOn  
    )  
   SELECT X.OurBranchID  
    ,X.OurBranchID  
    ,@DbContraAccountType  
    ,@DbContraAccountID  
    ,CASE @DbContraAccountType  
     WHEN 'G'  
      THEN 'G'  
     ELSE dbo.f_getAccountProductID(@PostToBranchID, @DbContraAccountID)  
     END  
    ,@ModuleID  
    ,0  
    ,'TD'  
    ,@TrxDate  
    ,@ValueDate  
    ,Sum(X.Amount * - 1)  
    ,Sum(X.Amount * - 1)  
    ,@CurrencyID  
    ,Sum(X.Amount * - 1)  
    ,1  
    ,1  
    ,0  
    ,'V'  
    ,0  
    ,NULL  
    ,''  
    ,''  
    ,@DrTrxDescriptionID  
    ,@Narration  
    ,CASE @DbContraAccountType  
     WHEN 'G'  
      THEN @DbContraAccountID  
     ELSE dbo.f_GetGLInterfaceAccountID(@BankID, dbo.f_getAccountProductID(@PostToBranchID, @DbContraAccountID), dbo.f_GetProductTypeID(@BankID, dbo.f_getAccountProductID(@PostToBranchID, @DbContraAccountID)), 'CONTROL_AC')  
     END  
    ,''  
    ,0  
    ,0  
    ,0  
    ,@OperatorID  
    ,GETDATE()  
   FROM @Staging X  
   WHERE X.TrxTypeID = 'TC'  
   GROUP BY X.OurBranchID  
  
   INSERT INTO @Transactions (  
    TrxBranchID  
    ,OurBranchID  
    ,AccountTypeID  
    ,AccountID  
    ,ProductID  
    ,ModuleID  
    ,TrxCodeID  
    ,TrxTypeID  
    ,TrxDate  
    ,ValueDate  
    ,Amount  
    ,LocalAmount  
    ,TrxCurrencyID  
    ,TrxAmount  
    ,ExchangeRate  
    ,MeanRate  
    ,Profit  
    ,InstrumentTypeID  
    ,ChequeID  
    ,ChequeDate  
    ,ReferenceNo  
    ,Remarks  
    ,TrxDescriptionID  
    ,TrxDescription  
    ,MainGLID  
    ,TrxFlagID  
    ,ImageID  
    ,TrxPrinted  
    ,IsTrxPending  
    ,CreatedBy  
    ,CreatedOn  
    )  
   SELECT X.OurBranchID  
    ,X.OurBranchID  
    ,@CrContraAccountType  
    ,@CrContraAccountID  
    ,CASE @CrContraAccountType  
     WHEN 'G'  
      THEN 'G'  
     ELSE dbo.f_getAccountProductID(@PostToBranchID, @CrContraAccountID)  
     END  
    ,@ModuleID  
    ,0  
    ,'TC'  
    ,@TrxDate  
    ,@ValueDate  
    ,Sum(ABS(X.Amount))  
    ,Sum(ABS(X.Amount))  
    ,@CurrencyID  
    ,Sum(ABS(X.Amount))  
    ,1  
    ,1  
    ,0  
    ,'V'  
    ,0  
    ,NULL  
    ,''  
    ,''  
    ,@CrTrxDescriptionID  
    ,@Narration  
    ,CASE @CrContraAccountType  
     WHEN 'G'  
      THEN @CrContraAccountID  
     ELSE dbo.f_GetGLInterfaceAccountID(@BankID, dbo.f_getAccountProductID(@PostToBranchID, @CrContraAccountID), dbo.f_GetProductTypeID(@BankID, dbo.f_getAccountProductID(@PostToBranchID, @CrContraAccountID)), 'CONTROL_AC')  
     END  
    ,''  
    ,0  
    ,0  
    ,0  
    ,@OperatorID  
    ,GETDATE()  
   FROM @Staging X  
   WHERE X.TrxTypeID = 'TD'  
   GROUP BY X.OurBranchID  
  END  
 END  
  
  
 UPDATE @Transactions  
 SET TrxDescriptionID = '007'  
 WHERE ISNULL(TrxDescriptionID, '') = ''  
  AND TrxTypeID = 'TC'  
  
 UPDATE @Transactions  
 SET TrxDescriptionID = '008'  
 WHERE ISNULL(TrxDescriptionID, '') = ''  
  AND TrxTypeID = 'TD'  
  
 UPDATE @Transactions  
 SET Amount = ABS(Amount) * - 1  
  ,LocalAmount = ABS(LocalAmount) * - 1  
  ,TrxAmount = ABS(TrxAmount) * - 1  
 WHERE TrxTypeID = 'TD'  
  
 SELECT @UseThisBranch = ISNULL(@UseThisBranch, @OurBranchID)  
  
 --IF @FormatID = '002'  
 --BEGIN  
  SELECT @IsPosting = 1  
 --END  
  
 IF (ISNULL(@IsPosting, 0) = 1)  
  OR (@IsPosting = 2)  
 BEGIN  
  EXEC p_GetNextTrxBatchID @BranchID = @OurBranchID  
   ,@NextTrxBatchID = @TrxBatchID OUTPUT  
  
  EXEC p_GetNextTrxSerialNo @OurBranchID = @OurBranchID  
   ,@TrxSerialTypeID = 'TR'  
   ,@NextTrxSerialNo = @SerialID OUTPUT  
 END  
  
 IF (SELECT SUM(LocalAmount) FROM @Transactions) <> 0  
 BEGIN  
  --SET @ErrorNo = 'BREXDB454529'  
  DECLARE @TotDebits Amount  
   ,@TotCredits Amount  
  
  SELECT @TotDebits = ISNULL(SUM(CASE   
      WHEN LocalAmount < 0  
       THEN LocalAmount  
      ELSE 0  
      END), 0)  
   ,@TotCredits = ISNULL(SUM(CASE   
      WHEN LocalAmount > 0  
       THEN LocalAmount  
      ELSE 0  
      END), 0)  
  FROM @Transactions  
  
  SET @ErrorNo1 = '860523'  
  
  SELECT @ErrorNo = 'BREXDB860523(' + CAST(@TotDebits AS VARCHAR(19)) + ')(' + CAST(@TotCredits AS VARCHAR(19)) + ')'  
  
  IF @DontRaiseError = 1  
  BEGIN  
   RETURN  
  END  
  
  RAISERROR (@ErrorNo,16,1)  
  
  RETURN  
 END  
  
 IF (SELECT SUM(LocalAmount) FROM @Transactions) <> 0  
 BEGIN  
  SET @ErrorNo1 = '600007'  
  
  IF @DontRaiseError = 1  
  BEGIN  
   RETURN  
  END  
  
  RAISERROR ('BREXDB600007',16,1)  
  
  RETURN  
 END  
  
 DECLARE @SumAmt MONEY  
  
 SELECT @SumAmt = Sum(CASE   
    WHEN TrxTypeID = 'TD'  
     THEN - ABS(Amount)  
    ELSE ABS(AMOUNT)  
    END)  
 FROM @Transactions  
  
 IF @SumAmt <> 0  
 BEGIN  
  SET @ErrorNo1 = '600007'  
  
  IF @DontRaiseError = 1  
  BEGIN  
   RETURN  
  END  
  
  RAISERROR ('BREXDB600007',16,1)  
  
  RETURN  
 END  
  
 --IF @FormatID = '002'  
 --BEGIN  
  SELECT @IsPosting = 1 --Kamunya 1  
 --END  
  
  
 IF (ISNULL(@IsPosting, 0) = 1)  
  OR (@IsPosting = 2) ----And ISNULL(@IsTrxAllow,0) = 1  
  AND (  
   SELECT COUNT(1)  
   FROM t_GatewayUpload(NOLOCK)  
   WHERE OurBranchID = @OurBranchID  
    AND FormatID = @FormatID  
    AND OperatorID = @OperatorID  
    AND WorkingDate = @TrxDate  
    AND ServerPath = @ServerPath  
    AND LocalPath = @LocalPath  
   ) = 1  
 BEGIN  
  
  DECLARE @TrxSerialID SMALLINT  
  
  SELECT @TrxSerialID = ISNULL(MAX(SerialID), 0) + 1  
  FROM t_TrxTransfer  
  WHERE TrxBranchID = @OurBranchID  
   AND OperatorID = @OperatorID  
  
  INSERT INTO t_TrxTransfer (  
   TrxBranchID  
   ,OperatorID  
   ,ModuleID  
   ,SerialID  
   ,SlNo  
   ,OurBranchID  
   ,AccountTypeID  
   ,AccountID  
   ,ProductID  
   ,TrxTypeID  
   ,TrxDate  
   ,ValueDate  
   ,Amount  
   ,LocalAmount  
   ,TrxCurrencyID  
   ,TrxAmount  
   ,ExchangeRate  
   ,MeanRate  
   ,Profit  
   ,InstrumentTypeID  
   ,ChequeID  
   ,ChequeDate  
   ,ReferenceNo  
   ,Remarks  
   ,TrxDescriptionID  
   ,TrxDescription  
   ,MainGLID  
   ,TrxFlagID  
   ,ForwardRemark  
   )  
  SELECT TrxBranchID  
   ,CreatedBy  
   ,ModuleID  
   ,@TrxSerialID TrxSerialID  
   ,ROW_NUMBER() OVER (  
    ORDER BY AccountID DESC  
    )  
   ,OurBranchID  
   ,AccountTypeID  
   ,AccountID  
   ,ProductID  
   ,TrxTypeID  
   ,TrxDate  
   ,ValueDate  
   ,ABS(Amount)  
   ,ABS(LocalAmount)  
   ,TrxCurrencyID  
   ,ABS(TrxAmount)  
   ,ExchangeRate  
   ,MeanRate  
   ,Profit  
   ,InstrumentTypeID  
   ,ChequeID  
   ,ChequeDate  
   ,ReferenceNo  
   ,Remarks  
   ,TrxDescriptionID  
   ,TrxDescription  
   ,MainGLID  
   ,'' TrxFlagID  
   ,''  
  FROM @Transactions  
  ORDER BY TrxBranchID  
   ,TrxTypeID  
  
  -- Select 'ppp'  
  --SELECT * FROM @Transactions  
  
  IF EXISTS (SELECT 1 FROM @Transactions WHERE TrxBranchID <> OurBranchID )  
  BEGIN  
   UPDATE t_TrxTransfer  
   SET ModuleID = @ModuleID  
   WHERE TrxBranchID = @OurBranchID  
    AND SerialID = @TrxSerialID  
    AND OperatorID = @OperatorID  
  
   DECLARE @@BatchReturned TABLE (TrxBatchID VARCHAR(8))  
  
   --INSERT INTO @@BatchReturned  
   --BEGIN TRAN  
  
   BEGIN TRY  
    EXEC p_AddTransferTrxIB   
     @TrxBranchID = @OurBranchID  
     ,@OperatorID = @OperatorID  
     ,@SerialID = @TrxSerialID  
     ,@ModuleID = @ModuleID  
     ,@TrxPrinted = 0  
     ,@TrxBatchID = @TrxBatchID OUTPUT  
     ,  
     --@ErrorNo  = @ErrorNo OUTPUT,  
     --@DontRaiseError = 1,  
     @DoNotReturnBatchID = 1  
     --IF @ErrorNo >'1'  
     --BEGIN  
     -- --ROLLBACK TRAN  
     -- DELETE t_TrxTransfer  
     -- FROM t_TrxTransfer(Nolock)  
     -- WHERE TrxBranchID = @OurBranchID AND SerialID = @TrxSerialID AND OperatorID = @OperatorID  
     -- SET @ErrorNo1 = @ErrorNo  
     -- SET @ErrorNo = CAST(@ErrorNo AS VARCHAR) +'(' + CAST(@OurBranchID AS VarChar(100)) + ')'  
     -- IF @DontRaiseError=1  
     -- BEGIN       
     --  RETURN  
     -- END  
     -- RAISERROR(@ErrorNo,16,1)   
     -- RETURN        
     --END  
     --COMMIT TRAN  
   END TRY  
  
   BEGIN CATCH  
    SET @ErrorNo = @@Error  
  
    SELECT ERRor_message()  
  
    --ROLLBACK TRAN      
    DELETE t_TrxTransfer  
    FROM t_TrxTransfer(NOLOCK)  
    WHERE TrxBranchID = @OurBranchID  
     AND SerialID = @TrxSerialID  
     AND OperatorID = @OperatorID  
  
    SET @ErrorNo1 = @ErrorNo  
    SET @ErrorNo = CAST(@ErrorNo AS VARCHAR) + '(' + CAST(@OurBranchID AS VARCHAR(100)) + ')'  
  
    IF @DontRaiseError = 1  
    BEGIN  
     RETURN  
    END  
  
    RAISERROR (@ErrorNo ,16 ,1)  
  
    RETURN  
   END CATCH  
  END  
  ELSE  
  BEGIN  
   BEGIN TRY  
    DECLARE ALLBRANCHES CURSOR  
    FOR  
    SELECT OurBranchID  
    FROM @disBranches  
  
    OPEN ALLBRANCHES  
  
    FETCH NEXT  
    FROM ALLBRANCHES  
    INTO @UseThisBranch  
  
    WHILE @@FETCH_STATUS = 0  
    BEGIN  
     EXEC p_AddTransferTrx @TrxBranchID = @UseThisBranch  
      ,@OperatorID = @OperatorID  
      ,@SerialID = @TrxSerialID  
      ,@ModuleID = @ModuleID  
      ,@ImageID = 0  
      ,@TrxPrinted = 0  
      ,@pTrxBatchID = @TrxBatchID OUTPUT  
      ,@DoNotReturnBatchID = 1 --,  
      --@ErrorNo  = @ErrorNo OUTPUT,  
      --@DontRaiseError = 1  
      --Select * from t_transaction where TrxBatchID =@TrxBatchID  
      --SELECT * FROM t_TrxTransfer  
  
     IF @ErrorNo > '1'  
     BEGIN  
      --ROLLBACK TRAN  
      DELETE t_TrxTransfer  
      FROM t_TrxTransfer(NOLOCK)  
      WHERE TrxBranchID = @OurBranchID  
       AND SerialID = @TrxSerialID  
       AND OperatorID = @OperatorID  
  
      SET @ErrorNo1 = @ErrorNo  
      SET @ErrorNo = CAST(@ErrorNo AS VARCHAR) + '(' + CAST(@OurBranchID AS VARCHAR(100)) + ')'  
  
      ---  
      CLOSE ALLBRANCHES  
  
      DEALLOCATE ALLBRANCHES  
  
      IF @DontRaiseError = 1  
      BEGIN  
       RETURN  
      END  
  
      RAISERROR (  
        @ErrorNo  
        ,16  
        ,1  
        )  
  
      RETURN  
     END  
  
     FETCH NEXT  
     FROM ALLBRANCHES  
     INTO @UseThisBranch  
    END  
  
    CLOSE ALLBRANCHES  
  
    DEALLOCATE ALLBRANCHES  
     --COMMIT TRAN   
   END TRY  
  
   BEGIN CATCH  
    SET @ErrorNo = @@Error  
  
    --ROLLBACK TRAN     
    DELETE t_TrxTransfer  
    FROM t_TrxTransfer(NOLOCK)  
    WHERE TrxBranchID = @OurBranchID  
     AND SerialID = @TrxSerialID  
     AND OperatorID = @OperatorID  
  
    SET @ErrorNo1 = @ErrorNo  
    SET @ErrorNo = CAST(@ErrorNo AS VARCHAR) + '(' + CAST(@OurBranchID AS VARCHAR(100)) + ')'  
  
    IF @DontRaiseError = 1  
    BEGIN  
     RETURN  
    END  
  
    RAISERROR (  
      @ErrorNo  
      ,16  
      ,1  
      )  
  
    RETURN  
   END CATCH  
  
   DECLARE @@BatchReturn TABLE (  
    SerialID BIGINT  
    ,TrxBatchID VARCHAR(8)  
    )  
  END  
  
  
  IF @FormatID <> '002'  
  BEGIN  
   --TAKE CARE OF CHARGES--  
   DECLARE @EffectiveDate DATETIME  
    ,@ChargeID VARCHAR(40)  
    ,@WorkingDate DATETIME  
    ,@EffectiveDateID VARCHAR(30)  
    ,@ChargeAmount MONEY  
    ,@TaxAmount MONEY  
    ,@CessAMount MONEY  
    ,@ChargeDueRowID NUMERIC(19)  
    ,@AccountID VARCHAR(40)  
    ,@TrxBranchID_ VARCHAR(40)  
    ,@ClientID VARCHAR(50)  
    ,@DescriptionID VARCHAR(40)  
    ,@TrxTypeID_ VARCHAR(20)  
    ,@Amount_ MONEY  
    ,@HasCharges BIT  
    ,@NotificationID VARCHAR(500)  
    ,@TrxBranchName VARCHAR(200)  
  
   SET @WorkingDate = dbo.f_GetWorkingDate(@OurBranchID)  
   SET @CurrencyID = dbo.f_GetProductCurrencyID(@BankID, @ProductID)  
  
   DECLARE AllTransactions CURSOR  
   FOR  
   SELECT OurBranchID  
    ,AccountID  
    ,ProductID  
    ,TrxDescriptionID  
    ,TrxTypeID  
    ,Amount  
   FROM @Transactions  
   WHERE AccountTypeID = 'C'  
  
   OPEN AllTransactions  
  
   FETCH NEXT  
   FROM AllTransactions  
   INTO @TrxBranchID_  
    ,@AccountID  
    ,@ProductID  
    ,@DescriptionID  
    ,@TrxTypeID_  
    ,@Amount_  
  
   WHILE @@FETCH_STATUS = 0  
   BEGIN  
    SET @HasCharges = 0  
  
    DECLARE MyCharges CURSOR  
    FOR  
    SELECT t_Charge.ChargeID  
     ,t_Charge.CurrencyID  
    FROM t_Charge(NOLOCK)  
    INNER JOIN t_ProductCharge(NOLOCK) ON t_Charge.ChargeID = t_ProductCharge.ChargeID  
    INNER JOIN t_TrxCharge(NOLOCK) ON t_TrxCharge.TrxDescriptionID = @DescriptionID  
     AND t_TrxCharge.ChargeID = t_ProductCharge.ChargeID  
     AND t_TrxCharge.ChargeID = t_TrxCharge.ChargeID  
    WHERE t_ProductCharge.ProductID = @ProductID  
  
    OPEN MyCharges  
  
    FETCH NEXT  
    FROM MyCharges  
    INTO @ChargeID  
     ,@CurrencyID  
  
    WHILE @@FETCH_STATUS = 0  
    BEGIN  
     SELECT TOP 1 @EffectiveDate = EffectiveDate  
     FROM t_ChargeEffectiveDate(NOLOCK)  
     WHERE ChargeID = @ChargeID  
      AND ChargeStatusID = 'AA'  
      AND EffectiveDate <= @WorkingDate  
     ORDER BY EffectiveDate ASC  
  
     SET @EffectiveDateID = dbo.f_GetEffectiveDateID(@BankID, @ChargeID, @EffectiveDate)  
     SET @ChargeAmount = 0  
  
     IF @EffectiveDateID IS NOT NULL  
     BEGIN  
      BEGIN  
       EXEC p_GetChargeAmount @OurBranchID = @TrxBranchID_  
        ,@AccountID = @AccountID  
        ,@ChargeID = @ChargeID  
        ,@EffectiveDate = @EffectiveDate  
        ,@TrxDate = @WorkingDate  
        ,@TrxCurrencyID = @CurrencyID  
        ,@TrxAmount = 0  
        ,@ChargeAmount = @ChargeAmount OUTPUT  
        ,@TaxAmount = @TaxAmount OUTPUT  
        ,@CessAmount = @CessAmount OUTPUT  
      END  
  
      IF @ChargeAmount > 0  
       AND ISNULL(@CurrencyID, '') <> ''  
      BEGIN  
       SET @HasCharges = 1  
  
       EXEC p_GetNextChargeDueRowID @TrxBranchID_  
        ,@ChargeDueRowID OUTPUT  
        ,@ErrorNo OUTPUT  
  
       IF ISNULL(@ErrorNo, '') > '1'  
       BEGIN  
        --RAISERROR(@ErrorNo, 16, 1)  
        --RETURN  
        GOTO NextChargeID  
         --Select @ChargeDueRowID = ISNULL(Series,0) + RIGHT(RAND(100),4)  
         --From t_SystemKeyBranch(nolock)   
         --WHERE OurBranchID = @OurBranchID  
         -- AND ModuleID = 2222  
         --IF ISNULL(@ChargeDueRowID ,0) =0  
         --  Goto  NextChargeID  
       END  
  
       INSERT INTO t_ChargeDue (  
        OurBranchID  
        ,ProductID  
        ,ChargeDueRowID  
        ,ClientID  
        ,AccountID  
        ,ApplicationID  
        ,ChargeID  
        ,CurrencyID  
        ,ProcessDate  
        ,DueDate  
        ,ChargeColConditionID  
        ,ChargeAmount  
        ,TaxAmount  
        ,CessAmount  
        ,ModuleID  
        ,ChargeDueStatusID  
        ,ExemptionReason  
        ,TrxBatchID  
        ,SerialID  
        ,TrxDate  
        ,PaymentTypeID  
        ,ReversedDate  
        ,ReversedBy  
        ,RevTrxDate  
        ,RevTrxBatchID  
        ,EventID  
        )  
       SELECT @TrxBranchID_  
        ,@ProductID  
        ,@ChargeDueRowID  
        ,''  
        ,@AccountID  
        ,NULL  
        ,@ChargeID  
        ,@CurrencyID  
        ,@WorkingDate  
        ,@WorkingDate  
        ,'ABD'  
        ,@ChargeAmount  
        ,@TaxAmount  
        ,@CessAmount  
        ,@ModuleID  
        ,'N'  
        ,NULL  
        ,NULL  
        ,NULL  
        ,NULL  
        ,'C'  
        ,NULL  
        ,NULL  
        ,NULL  
        ,NULL  
        ,'GATEWAY_UPL'  
  
         
      END  
  
      NextChargeID:  
  
      FETCH NEXT  
      FROM MyCharges  
      INTO @ChargeID  
       ,@CurrencyID  
     END  
    END  
  
    CLOSE MyCharges  
  
    DEALLOCATE MyCharges  
  
    SELECT @NotificationID = NotificationID  
    FROM t_NotificationFormat(NOLOCK)  
    WHERE NotificationTriggerID = '3020'  
  
    IF @HasCharges > 0  
    BEGIN  
     --INSERT SMS NOTIFICATION  
     --Dear Customer, your account ending 0575 has been debited with TZS 2720000.00. Ref No :    
     --#TrxBatchID#.ENROLL FOR FTB SIMUYO BANKYO MOBILE BANKING SERVICE  
     ----@TrxTypeID_,@Amount_  
     BEGIN TRY  
      SET @TrxBranchName = dbo.f_GetBranchName(@TrxBranchID_)  
      SET @ClientID = dbo.f_GetAccountClientID(@TrxBranchID_, @AccountID)  
  
      --EXEC dbo.p_ProcessNotification @NotificationTriggerType = 'TRX'  
      -- ,@NotificationTriggerID = '3020'  
      -- ,@BankID = @BankID  
      -- ,@OurBranchID = @TrxBranchID_  
      -- ,@AccountID = @AccountID  
      -- ,@Amount = @Amount_  
      -- ,@TrxBranchName = @TrxBranchName  
      -- ,@ChequeNo = '0'  
      -- ,@ChequeDate = NULL  
      -- ,@DeviceLocation = NULL  
      -- ,@ExpiryDate = NULL  
      -- ,@InstallAmount = 0  
      -- ,@InstallDate = NULL  
      -- ,@LoanAmount = 0  
      -- ,@ApplicationID = NULL  
      -- ,@WorkingDate = @WorkingDate  
      -- ,@OperatorID = @OperatorID  
      -- ,@OperatorNames = @OperatorID  
      -- ,@ReceiptAmount = 0  
      -- ,@ReceiptNo = NULL  
      -- ,@FromDate = NULL  
      -- ,@ToDate = NULL  
      -- ,@TrxType = @TrxTypeID_  
      -- ,@ClientID = @ClientID  
      -- ,@ArrearsAmount = 0  
      -- ,@ArrearsDays = 0  
      -- ,@TrxBatchID = @TrxBatchID  
      -- ,@MPesaMobileNo = NULL  
      -- ,@NotificationID = NULL  
     END TRY  
  
     BEGIN CATCH  
     END CATCH  
    END  
  
    SET @HasCharges = 0  
  
    FETCH NEXT  
    FROM AllTransactions  
    INTO @TrxBranchID_  
     ,@AccountID  
     ,@ProductID  
     ,@DescriptionID  
     ,@TrxTypeID_  
     ,@Amount_  
   END  
  
   CLOSE AllTransactions  
  
   DEALLOCATE AllTransactions  
  END  
  
    
  --END OF CHARGES--  
  SET @TotalSumUpload = 0  
  
  --Post Outward Clearing-----------  
  DECLARE @TrxBranchID dbo.BranchID  
   ,@TrxBatchSLNo BIGINT  
   ,@DebitOurBranchID dbo.BranchID  
   ,@Amount dbo.Amount  
   ,@RoundingID SystemSubID  
   ,@DebitAccountType VARCHAR(2)  
   ,@DebitAccountID dbo.AccountID  
   ,@DebitProductID dbo.ProductID  
   ,@CRTrxCurrencyID CurrencyID  
   ,@DrawerAccountID dbo.AccountID  
   ,@DRAmount dbo.Amount  
   ,@DRLocalAmount dbo.Amount  
   ,@DRTrxCurrencyID dbo.CurrencyID  
   ,@TrxAmount dbo.Amount  
   ,@ExchangeRate dbo.Rate  
   ,@MeanRate dbo.Rate  
   ,@ReferenceNo VARCHAR(100)  
   ,@Remarks VARCHAR(255)  
   ,@TrxDescription VARCHAR(255)  
   ,@DrMainGLID dbo.AccountID  
   ,@DRContraGLID dbo.AccountID  
   ,@BranchID dbo.BankID  
   ,@CreditAccountID dbo.AccountID  
   ,@BeneficiaryName VARCHAR(255)  
   ,@CreditBranchID dbo.BranchID  
  
  --SELECT 'Kamunya'  
  IF (SELECT ISNULL(ClgFileGeneratedDate, EODDate)  
    FROM t_SystemBranchStatus(NOLOCK)  
    WHERE OurBranchID = @OurBranchID  
    ) >= @TrxDate  
  BEGIN  
   SELECT 'Kamunya1'    
   --Return Message  
   RETURN  
  END  
  
  SELECT @OurBankID = dbo.f_GetBankID(@ClearingTrxBranchID)  
     
  DECLARE Clearing CURSOR  
  FOR  
  SELECT OurBranchID  
   ,OurBranchID  
   ,DRCRAccountID  
   ,Amount  
   ,BankID  
   ,PayeeBranchCode  
   ,PayeeBankAccountNumber  
   ,PayeeName  
   ,[Description]  
  FROM t_PreProcessUpload  
  WHERE RIGHT(BankID, 2) <> @BankID  
   AND CreatedBy = @OperatorID  
   AND FilePath = REPLACE(REPLACE(@ServerP, '{', ''), '}', '')  
  
  OPEN Clearing  
  
  FETCH NEXT  
  FROM Clearing  
  INTO @TrxBranchID  
   ,@OurBranchID  
   ,@DebitAccountID  
   ,@Amount  
   ,@BankID  
   ,@BranchID  
   ,@CreditAccountID  
   ,@BeneficiaryName  
   ,@TrxDescription  
  
  WHILE @@FETCH_STATUS = 0  
  BEGIN  
   SELECT TOP 1 @DrawerAccountID = PayeeBankAccountNumber, @DebitOurBranchID = OurBranchID  
   FROM t_PreProcessUpload  
   WHERE RIGHT(BankID, 2) <> @BankID  
    AND CreatedBy = @OperatorID  
    AND FilePath = REPLACE(REPLACE(@ServerP, '{', ''), '}', '')  
    AND TrxTypeID = 'TD'  
  
   SELECT @TrxBranchID = @DebitOurBranchID,@OurBranchID = @DebitOurBranchID  
   --SET @DebitOurBranchID = @ClearingTrxBranchID  
   SET @DebitAccountID = dbo.f_GetCurrencyBranchGLAccountID(@DebitOurBranchID, isNULL(@DRTrxCurrencyID, 'TZS'), 'CEN_BANK_AC')  
   SET @DebitAccountType = dbo.f_GetAccountTypeID(@DebitOurBranchID, @DebitAccountID)  
   SET @DebitProductID = ISNULL(dbo.f_GetAccountProductID(@DebitOurBranchID, @DebitAccountID), '')  
   SET @DrMainGLID = dbo.f_GetGLInterfaceAccountID1(@DebitOurBranchID, @DebitProductID, 'CONTROL_AC')  
   SET @DRTrxCurrencyID = ISNULL(dbo.f_GetAccountOrGLCurrencyID(@DebitOurBranchID, @DebitAccountID, @DebitAccountType), 'TZS')  
   SET @DRContraGLID = dbo.f_GetCurrencyBranchGLAccountID(@DebitOurBranchID, @DRTrxCurrencyID, 'CUR_CLR_AC')  
   SET @TrxBatchSLNo = 1  
  
   IF @DebitProductID = 'GL'  
   BEGIN  
    SELECT @DrMainGLID = @DebitAccountID  
   END  
  
   SET @Amount = Abs(@Amount)  
   SET @DRAmount = @Amount * - 1  
   SET @DRLocalAmount = @DRAmount  
  
   SELECT @MeanRate = MeanRate  
    ,@RoundingID = RoundingID  
   FROM t_CurrencyRate(NOLOCK)  
   WHERE OurBranchID = @DebitOurBranchID  
    AND CurrencyID = @DRTrxCurrencyID  
    AND RateTypeID = 'REV'  
  
   SELECT @TrxAmount = @Amount  
  
   -- SELECT @TrxAmount TrxAmount, @MeanRate MeanRate, @RoundingID RoundingID  
   -- select dbo.f_RoundAmount(@RoundingID, (@TrxAmount * @MeanRate)) 'KKK'  
   --SET @TrxDescription = 'Gateway Salary Upload'  
   SELECT @DRLocalAmount = isNull(dbo.f_RoundAmount(@RoundingID, (@TrxAmount * @MeanRate)), (@TrxAmount * @MeanRate))  
    ,@ExchangeRate = @MeanRate  
  
   --Select @DebitAccountID,@DRContraGLID, @TrxBranchID,@OurBranchID,@Amount Amount,@DRAmount DRAmount, @TrxAmount TrxAmount, @DRLocalAmount DRLocalAmount, @ExchangeRate ExchangeRate --return-- hapa  
   IF isNull(@TrxDescription, '') = ''  
   BEGIN  
    SELECT @TrxDescription = @Narration  
   END  
  
   --SELECT  isNull(@TrxDescription,@Narration) 'DescKam'  
   EXEC p_AddOutwardTrx @TrxBranchID = '000' --@DebitOurBranchID  
    ,@TrxBatchID = @TrxBatchID  
    ,@TrxBatchSLNo = @TrxBatchSLNo  
    ,@SerialID = @SerialID  
    ,@OurBranchID = '000' --@DebitOurBranchID  
    ,@AccountTypeID = @DebitAccountType  
    ,@AccountID = @DebitAccountID  
    ,@ProductID = @DebitProductID  
    ,@ModuleID = '3070'  
    ,@TrxTypeID = 'OD'  
    ,@TrxDate = @TrxDate  
    ,@ValueDate = @TrxDate  
    ,@Amount = @DRAmount  
    ,@LocalAmount = @DRLocalAmount  
    ,@TrxCurrencyID = @DRTrxCurrencyID  
    ,@TrxAmount = @TrxAmount  
    ,@ExchangeRate = @ExchangeRate  
    ,@MeanRate = @MeanRate  
    ,@Profit = 0  
    ,@InstrumentTypeID = 'V'  
    ,@ChequeID = 0  
    ,@ChequeDate = NULL  
    ,@ReferenceNo = @RandomNum  
    ,@Remarks = @TrxDescription  
    ,@TrxDescriptionID = '512'  
    ,@TrxDescription = @TrxDescription  
    ,@MainGLID = @DrMainGLID  
    ,@ContraGLID = @DRContraGLID  
    ,@TrxFlagID = ''  
    ,@ImageID = 0  
    ,@TrxPrinted = 0  
    ,@CreatedBy = 'SYS'  
    ,@NewRecord = 1  
    ,@IsForfeit = 0  
    ,@ChequeDigit = '0'  
    ,@VoucherCode = '59'  
    ,@ReturnCodeID = '00'  
    ,@Commission = 0  
    ,@TheirCommission = 0  
    ,@BankID = @BankID  
    ,@BranchID = @BranchID  
    ,@DrawerOrPayeeAccountID = @CreditAccountID  
    ,@DrawerOrPayee = @BeneficiaryName  
    ,@VATPINNo = NULL  
    ,@VATPAYType = NULL  
    ,@VATSerialNo = NULL  
    ,@VATPAYEMonth = NULL  
    ,@VATPAYECommission = 0  
    ,@ErrorNo = @ErrorNo OUTPUT  
  
   IF ISNULL(@ErrorNo, 0) <> 0  
   BEGIN  
    SET @ErrorNo1 = @ErrorNo  
  
    RAISERROR (@ErrorNo,16,1)  
  
    RETURN  
   END  
  
   DECLARE @DRChargeDescription VARCHAR(35)  
  
   SELECT @DRChargeDescription = 'Bulk Outward EFT upload'  
  
   --Select @DebitOurBranchID, @DrawerAccountID, @DRTrxCurrencyID, @SerialID, @OperatorID, @DRChargeDescription, @TrxBatchID  
   --BEGIN TRY  
   --select @TrxBatchID aisee,  @SerialID kamunya  
   EXEC p_ChargeTransaction @OurBranchID = @DebitOurBranchID  
    ,@AccountID = @DrawerAccountID  
    ,@TrxDescriptionID = '006'  
    ,@TrxCurrencyID = @DRTrxCurrencyID  
    ,@ModuleID = 3070  
    ,@SerialID = @SerialID  
    ,@CreatedBy = @OperatorID  
    ,@Narration = @DRChargeDescription  
    ,@ChargeID = '107'  
    ,@TrxBatchID = @TrxBatchID  
    ,@Multiplier = 1  
    ,@ErrorNo = @ErrorNo OUTPUT  
  
   --select @ErrorNo   
   --END TRY  
   --BEGIN CATCH  
   --select 'K'  
   --SELECT Error_Message()  
   --END CATCH  
   FETCH NEXT  
   FROM Clearing  
   INTO @TrxBranchID  
    ,@OurBranchID  
    ,@DebitAccountID  
    ,@Amount  
    ,@BankID  
    ,@BranchID  
    ,@CreditAccountID  
    ,@BeneficiaryName  
    ,@TrxDescription  
  END  
  
  CLOSE Clearing  
  
  DEALLOCATE Clearing  
  
  INSERT INTO t_PreProcessUploadHistory (  
   TrxBranchID  
   ,OurBranchID  
   ,AccountID  
   ,EmployeeID  
   ,Amount  
   ,StatusCode  
   ,FeeCode  
   ,CreatedBy  
   ,CreatedOn  
   ,FilePath  
   ,TrxTypeID  
   ,PayeeBranchCode  
   ,PayeeBankAccountNumber  
   ,PayeeName  
   ,DRCRAccountID  
   ,BankID  
   )  
  SELECT OurBranchID  
   ,OurBranchID  
   ,AccountID  
   ,EmployeeID  
   ,Amount  
   ,StatusCode  
   ,FeeCode  
   ,CreatedBy  
   ,CreatedOn  
   ,FilePath  
   ,TrxTypeID  
   ,PayeeBranchCode  
   ,PayeeBankAccountNumber  
   ,PayeeName  
   ,DRCRAccountID  
   ,BankID  
  FROM t_PreProcessUpload(NOLOCK)  
  WHERE CreatedBy = @OperatorID  
   AND FilePath = @ServerP  
  
  DELETE  
  FROM t_PreProcessUpload  
  WHERE CreatedBy = @OperatorID  
   AND FilePath = @ServerP  
  
  UPDATE t_GatewayUpload  
  SET trxBatchID = @TrxBatchID  
   ,Accounts = @Accounts  
  WHERE OurBranchID = @OurBranchID  
   AND FormatID = @FormatID  
   AND OperatorID = @OperatorID  
   AND WorkingDate = @TrxDate  
   AND ServerPath = @ServerPath  
   AND LocalPath = @LocalPath  
   --Update t_PostingDetails   
   --SET trxBatchID = @TrxBatchID,  
   -- IsProcessed = 1   
   --Where SerialID = @PostingSerial And  
   --  FormatID = @FormatID   
 END  
 ELSE  
 BEGIN  
  UPDATE t_GatewayUpload  
  SET IsValidated = 1  
  WHERE OurBranchID = @OurBranchID  
   AND FormatID = @FormatID  
   AND OperatorID = @OperatorID  
   AND WorkingDate = @TrxDate  
   AND ServerPath = @ServerPath  
   AND LocalPath = @LocalPath  
   --UPDATE t_PostingDetails   
   --SET trxBatchID = @TrxBatchID,  
   -- IsProcessed = 1   
   --Where SerialID = @PostingSerial And  
   --  FormatID = @FormatID  
 END  
  
 SELECT @TrxBatchID TrxBatchID --THIS IS THE BATCHID RETURNED TO FRON END   
  
 SET NOCOUNT OFF  
END

