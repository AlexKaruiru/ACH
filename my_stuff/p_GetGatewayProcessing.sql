CREATE   PROCEDURE [dbo].[p_GetGatewayProcessing] (  
 @BankID dbo.BankID = NULL  
 ,@OurBranchID dbo.BranchID  
 ,@FormatID VARCHAR(5)  
 ,@CurrencyID dbo.CurrencyID  
 ,@OperatorID VARCHAR(25)  
 ,@FilePath VARCHAR(250)  
 ,@ErrorNo1 INT = 1 OUTPUT  
 )  
AS  
BEGIN  
 SET DEADLOCK_PRIORITY LOW  
 SET NOCOUNT ON  
  
 BEGIN  
  DECLARE @LanguageID VARCHAR(3)  
   ,@ErrorNo VARCHAR(200)  
  DECLARE @DelimiterID VARCHAR(3)  
   ,@FileFormatID VARCHAR(3)  
   ,@DelimiterType CHAR(1)  
   ,@Delimeter VARCHAR(5)  
   ,@SalFileTypeId VARCHAR(5)  
   ,@NewLine VARCHAR(8000)  
   ,@OldLine VARCHAR(8000)  
   ,@FieldCount SMALLINT  
   ,@LoopCount SMALLINT  
   ,@Incrementor SMALLINT  
   ,@Fieldlength SMALLINT  
   ,@Linelength INT  
   ,@LineCount INT  
   ,@TOTFieldlength INT  
   ,@FieldValue VARCHAR(100)  
   ,@FieldName VARCHAR(100)  
   ,@DataType VARCHAR(100)  
   ,@BatchID BIGINT  
   ,@strSQL VARCHAR(2000)  
   ,@strColumns VARCHAR(2000)  
   ,@TrxType VARCHAR(2)  
   ,@OurBankID BankID  
   ,@Count INT  
  
  SET @Incrementor = 0  
  SET @BatchID = 0  
  
  SELECT @LanguageID = LanguageID  
  FROM t_User(NOLOCK)  
  WHERE OperatorID = @OperatorID  
  
  SET @BankID = dbo.f_getBankID(@OurBranchID)  
  
  SELECT @SalFileTypeId = FileType  
   ,@Delimeter = Delimeter  
   ,@TrxType = TransactionType  
   ,@FileFormatID = FileFormat  
  FROM t_FileFormat(NOLOCK)  
  WHERE BankID = @BankID  
   AND FormatID = @FormatID  
   AND CurrencyID = @CurrencyID  
  
  IF @TrxType <> 'B'  
  BEGIN  
   IF NOT EXISTS (  
     SELECT 1  
     FROM t_GatewayContraAccountDetail(NOLOCK)  
     WHERE OurBranchID = @OurBranchID  
      AND FormatID = @FormatID  
     )  
   BEGIN  
    SET @ErrorNo = 'BREXDB860512(' + CAST(@OurBranchID AS VARCHAR(15)) + ')'  
    SET @ErrorNo1 = '860512'  
  
    RAISERROR (  
      @ErrorNo  
      ,16  
      ,1  
      )  
  
    RETURN  
   END  
  END  
  
  --IF NOT EXISTS(SELECT 1 FROM t_GatewayInvalidAccountDetail WHERE OurBranchID = @OurBranchID AND FormatID = @FormatID)  
  --BEGIN  
  -- SET @ErrorNo = 'BREXDB860513(' + CAST(@OurBranchID AS VarChar(15)) + ')'  
  -- RAISERROR(@ErrorNo,16,1)  
  -- RETURN   
  --END  
  SELECT @DelimiterID = CASE @Delimeter  
    WHEN '01'  
     THEN ','  
    WHEN '04'  
     THEN '\t'  
    WHEN '03'  
     THEN ';'  
    ELSE '\n'  
    END  
  
  DECLARE @FormatInField TABLE (  
   RowID SMALLINT IDENTITY(1, 1)  
   ,FieldName VARCHAR(30)  
   ,ColumnName VARCHAR(100)  
   ,FieldPosition SMALLINT  
   ,DataType VARCHAR(10)  
   ,[Length] SMALLINT  
   )  
  
  CREATE TABLE #MasterTable (  
   BatchID BIGINT  
   ,FieldName VARCHAR(100)  
   ,FieldValue VARCHAR(100)  
   )  
  
  CREATE TABLE #MagicTable (BatchID BIGINT)  
  
  INSERT INTO @FormatInField (  
   ColumnName  
   ,FieldName  
   ,FieldPosition  
   ,DataType  
   ,[Length]  
   )  
  SELECT dbo.f_GetSystemCodeName('DataFormatFieldID', FieldName)  
   ,FieldName  
   ,FieldPosition  
   ,FieldDataType  
   ,Length  
  FROM t_FileFormatInField(NOLOCK)  
  WHERE BankID = @BankID  
   AND FormatID = @FormatID  
   AND CurrencyID = @CurrencyID  
  ORDER BY FieldPosition  
  
  SELECT @FieldCount = COUNT(1)  
  FROM @FormatInField  
  
  --THE MAGIC TABLE IS GLOBAL, WE CREATE IT FROM HERE  
  SET @LoopCount = 1  
  SET @strColumns = ''  
  SET @TOTFieldlength = 0  
  
  WHILE @LoopCount <= @FieldCount  
  BEGIN  
   SELECT @Fieldlength = [Length]  
    ,@FieldName = ColumnName  
    ,@DataType = DataType  
   FROM @FormatInField  
   WHERE RowID = @LoopCount  
  
   SET @TOTFieldlength = @TOTFieldlength + @Fieldlength  
   SET @strColumns = @strColumns + CAST(@FieldName AS VARCHAR(100)) + ',' -- WE GET THE LIST OF COLUMNS FOR EASY FUNCTIONALITY  
   SET @strSQL = 'ALTER TABLE #MagicTable ADD ' + CAST(@FieldName AS VARCHAR(100)) + ' ' + CASE @DataType  
     WHEN 'TX'  
      THEN 'VARCHAR(' + CAST(@Fieldlength AS VARCHAR(100)) + ')'  
       --WHEN 'NM' THEN 'NUMERIC(19,4)'  
     WHEN 'NM'  
      THEN 'VARCHAR(100)'  
     ELSE 'SMALLDATETIME'  
     END  
  
   EXEC (@strSQL)  
  
   SET @LoopCount = @LoopCount + 1  
  END  
  
  --REMOVE THE LAST COMMA FROM THE STRING  
  SET @strColumns = LEFT(@strColumns, LEN(@strColumns) - 1)  
  
  IF (  
    @SalFileTypeId IN (  
     'T'  
     ,','  
     )  
    ) --THIS IS FOR TAB DELIMETED FILES. WE BRANCH HERE FOR FASTER FILE PROCESSING  
  BEGIN  
   SET @LoopCount = 1  
  
   CREATE TABLE #FileUploads (RowID BIGINT Identity(1, 1))  
  
   WHILE @LoopCount <= @FieldCount  
   BEGIN  
    SELECT @Fieldlength = [Length]  
     ,@FieldName = ColumnName  
     ,@DataType = DataType  
    FROM @FormatInField  
    WHERE RowID = @LoopCount  
  
    SET @strSQL = 'ALTER TABLE #FileUploads ADD ' + CAST(@FieldName AS VARCHAR(100)) + ' ' + CASE @DataType  
      WHEN 'TX'  
       THEN 'VARCHAR(' + CAST(@Fieldlength AS VARCHAR(100)) + ')'  
      WHEN 'NM'  
       THEN 'NUMERIC(19,2)'  
      ELSE 'SMALLDATETIME'  
      END  
  
    EXEC (@strSQL)  
  
    ---print @strSQL    
    SET @LoopCount = @LoopCount + 1  
   END  
  
   ALTER TABLE #FileUploads  
  
   DROP COLUMN RowID  
  
   DECLARE @NewStrSQL VARCHAR(Max)  
  
   IF @FileFormatID = 'E'  
   BEGIN  
    SET @NewStrSQL = 'INSERT INTO #FileUploads SELECT * FROM OPENROWSET (' + '''Microsoft.ACE.OLEDB.12.0''' + ',' + '''Excel 12.0;Database=' + @Filepath + ';HDR=YES;IMEX=1''' + ',' + '''SELECT * FROM [Sheet1$]''' + ')'  
   END  
   ELSE  
   BEGIN  
    IF @SalFileTypeId IN ('T')  
     SET @NewStrSQL = 'BULK  
     INSERT #FileUploads   
     FROM ''' + @Filepath + '''  
     WITH  
     (  
     FIELDTERMINATOR = ''' + @DelimiterID + ''',  
     FIRSTROW = 2,  
     ROWTERMINATOR = ''\n'');'  
    ELSE  
     SET @NewStrSQL = 'BULK  
     INSERT #FileUploads   
     FROM ''' + @Filepath + '''  
     WITH  
     (  
     FIELDTERMINATOR = ''' + @DelimiterID + ''',  
     FIRSTROW = 0,  
     ROWTERMINATOR = ''\n'');'  
      --BULK  
      -- INSERT #FileUploads   
      -- FROM '\\10.0.0.69\GatewayUploads\salary.txt'  
      -- WITH  
      -- (  
      -- FIELDTERMINATOR = '\t',  
      -- FIRSTROW = 2,  
      -- ROWTERMINATOR = '\n');  
      --return  
   END  
  
   BEGIN TRY  
    --PRINT 'here' + @NewStrSQL  
    EXEC (@NewStrSQL);  
   END TRY  
  
   BEGIN CATCH  
    DECLARE @ErrorMessage VARCHAR(4000)  
  
    SELECT @ErrorMessage = 'ERROR: Process: ' + IsNull(ERROR_PROCEDURE(), '') + ', Line:' + CONVERT(NVARCHAR, IsNull(ERROR_LINE(), '')) + ', ' + IsNull(ERROR_MESSAGE(), '')  
  
    PRINT @ErrorMessage  
  
    --EXECUTE usp_GetErrorInfo;  
    SET @ErrorNo = 'BREXDB860521(' + CAST(@strColumns AS VARCHAR(Max)) + ')'  
  
    RAISERROR (  
      @ErrorNo  
      ,16  
      ,1  
      )  
  
    RETURN  
   END CATCH;  
  
   --select * from #FileUploads return   
   EXEC ('INSERT INTO #MagicTable(' + @strColumns + ') SELECT * FROM #FileUploads')  
  
   DROP TABLE #FileUploads  
  END  
  ELSE IF @SalFileTypeId = 'F' --@SalFileTypeId = 'F'  
  BEGIN  
   CREATE TABLE #tempfile (line VARCHAR(8000))  
  
   EXEC ('BULK INSERT #tempfile FROM "' + @FilePath + '" WITH (ROWTERMINATOR=''' + @DelimiterID + ''')')  
  
   DECLARE @StatgingTable TABLE (  
    RowID BIGINT Identity(1, 1)  
    ,line VARCHAR(8000)  
    )  
  
   INSERT INTO @StatgingTable (line)  
   SELECT line  
   FROM #tempfile  
  
   DROP TABLE #tempfile  
  
   SET @DelimiterType = ''  
  
   SELECT @LineCount = COUNT(1)  
   FROM @StatgingTable  
  
   SET @LoopCount = 1  
   SET @BatchID = 0  
  
   WHILE @LoopCount <= @LineCount  
   BEGIN  
    SELECT @NewLine = line  
    FROM @StatgingTable  
    WHERE RowID = @LoopCount  
  
    SET @Incrementor = 1  
    SET @OldLine = @NewLine  
    SET @Linelength = LEN(@NewLine)  
  
    IF @TOTFieldlength <> @Linelength  
     AND @FormatID NOT IN (  
      '003'  
      ,'ATM'  
      )  
    BEGIN  
     SET @ErrorNo1 = '860510'  
  
     RAISERROR (  
       'BREXDB860510'  
       ,16  
       ,1  
       )  
  
     RETURN  
    END  
  
    SET @BatchID = ISNULL(@BatchID, 0) + 1  
  
    WHILE @Incrementor <= @FieldCount  
    BEGIN  
     SELECT @Fieldlength = [Length]  
      ,@FieldName = dbo.f_GetSystemCodeName('DataFormatFieldID', FieldName)  
      ,@DataType = DataType  
     FROM @FormatInField  
     WHERE RowID = @Incrementor  
  
     IF @DelimiterType = 'A'  
      SET @FieldValue = SUBSTRING(@NewLine, 1, @Fieldlength + 1)  
     ELSE  
      SET @FieldValue = SUBSTRING(@NewLine, 1, @Fieldlength)  
  
     SET @FieldValue = LTRIM(RTRIM(@FieldValue))  
  
     --DATATYPE VALIDATOR. TO BE EXPANDED FURTHER LATER  
     IF @DataType = 'NM'  
     BEGIN  
      IF ISNUMERIC(@FieldValue) = 0  
      BEGIN  
       SET @ErrorNo1 = '860511'  
       SET @ErrorNo = 'BREXDB860511(' + CAST(@FieldValue AS VARCHAR(15)) + ')(' + CAST(@OldLine AS VARCHAR(100)) + ')'  
  
       RAISERROR (  
         @ErrorNo  
         ,16  
         ,1  
         )  
  
       RETURN  
      END  
     END  
  
     IF @DelimiterType = 'A'  
      SET @NewLine = SUBSTRING(@NewLine, @Fieldlength + 1, @Linelength)  
     ELSE  
      SET @NewLine = SUBSTRING(@NewLine, @Fieldlength, @Linelength)  
  
     SET @NewLine = RTRIM(@NewLine)  
  
     --Since most sections will be tabs ('\t') we need to replace with nothing  
     --Another alternative is to replace the tabs using some other delimeter and life will be easy           
     IF @DelimiterType = ''  
      SET @DelimiterType = CASE CHARINDEX(' ', @NewLine)  
        WHEN 1  
         THEN 'A'  
        ELSE 'B'  
        END  
     SET @NewLine = SUBSTRING(@NewLine, 2, @Linelength)  
  
     IF @DelimiterType <> 'A'  
      SET @DelimiterType = CASE CHARINDEX(' ', @NewLine)  
        WHEN 1  
         THEN 'A'  
        ELSE 'B'  
        END  
  
     --do the insert to our magic table  
     INSERT INTO #MasterTable (  
      BatchID  
      ,FieldName  
      ,FieldValue  
      )  
     VALUES (  
      @BatchID  
      ,@FieldName  
      ,@FieldValue  
      )  
  
     SET @Incrementor = @Incrementor + 1  
     SET @Linelength = LEN(@NewLine)  
    END  
  
    SET @LoopCount = @LoopCount + 1  
   END  
  
   --THIS TABLE HAS OUTLIVED ITS USEFULNESS    
   --DROP TABLE #tempfile  
   --DYNAMICALLY INSERT THE VALUES INTO A TABLE    
   SET @LoopCount = 1  
   SET @strSQL = 'SELECT BatchID'  
  
   WHILE @LoopCount <= @FieldCount  
   BEGIN  
    SELECT @FieldName = ColumnName  
    FROM @FormatInField  
    WHERE RowID = @LoopCount  
  
    SET @strSQL = @strSQL + ' ,'  
    SET @strSQL = @strSQL + ' MAX(CASE WHEN FieldName=''' + @FieldName + ''' THEN FieldValue ELSE NULL END) AS ' + @FieldName + ''  
    SET @LoopCount = @LoopCount + 1  
   END  
  
   SET @strSQL = @strSQL + ' FROM #MasterTable GROUP BY BatchID'  
  
   EXEC ('INSERT INTO #MagicTable(BatchID,' + @strColumns + ') ' + @strSQL)  
  END  
  
  ---############################################################################--  
  --HARDCODED SOME ACTIVITIES FOR ATM PURPOSES. THE BRANCHID COMES IN SOME FUNNY WAY  
  IF @FormatID = 'ATM'  
  BEGIN  
   ALTER TABLE #MagicTable  
  
   DROP COLUMN OurBranchID  
  END  
  
  -- CHECK IF SOME KEY COLUMNS ARE PART OF THE UPLOAD FILES OR NOT. IF NOT THEN WE ADD THEM TO THE TABLE  
  IF NOT EXISTS (  
    SELECT *  
    FROM TempDB.INFORMATION_SCHEMA.COLUMNS  
    WHERE COLUMN_NAME = 'ProductID'  
     AND TABLE_NAME LIKE '#MagicTable%'  
    )  
  BEGIN  
   ALTER TABLE #MagicTable ADD ProductID VARCHAR(10)  
  END  
  
  IF NOT EXISTS (  
    SELECT *  
    FROM TempDB.INFORMATION_SCHEMA.COLUMNS  
    WHERE COLUMN_NAME = 'ClearBalance'  
     AND TABLE_NAME LIKE '#MagicTable%'  
    )  
  BEGIN  
   ALTER TABLE #MagicTable ADD ClearBalance NUMERIC(19, 2)  
  END  
  
  IF NOT EXISTS (  
    SELECT *  
    FROM TempDB.INFORMATION_SCHEMA.COLUMNS  
    WHERE COLUMN_NAME = 'MinimumBalance'  
     AND TABLE_NAME LIKE '#MagicTable%'  
    )  
  BEGIN  
   ALTER TABLE #MagicTable ADD MinimumBalance NUMERIC(19, 2)  
  END  
  
  IF NOT EXISTS (  
    SELECT *  
    FROM TempDB.INFORMATION_SCHEMA.COLUMNS  
    WHERE COLUMN_NAME = 'Name'  
     AND TABLE_NAME LIKE '#MagicTable%'  
    )  
  BEGIN  
   ALTER TABLE #MagicTable ADD Name VARCHAR(100)  
  END  
  
  IF NOT EXISTS (  
    SELECT *  
    FROM TempDB.INFORMATION_SCHEMA.COLUMNS  
    WHERE COLUMN_NAME = 'AccountID'  
     AND TABLE_NAME LIKE '#MagicTable%'  
    )  
  BEGIN  
   ALTER TABLE #MagicTable ADD AccountID VARCHAR(25)  
  END  
  
  IF NOT EXISTS (  
    SELECT *  
    FROM TempDB.INFORMATION_SCHEMA.COLUMNS  
    WHERE COLUMN_NAME = 'OurBranchID'  
     AND TABLE_NAME LIKE '#MagicTable%'  
    )  
  BEGIN  
   ALTER TABLE #MagicTable ADD OurBranchID VARCHAR(5)  
  END  
  
  IF NOT EXISTS (  
    SELECT *  
    FROM TempDB.INFORMATION_SCHEMA.COLUMNS  
    WHERE COLUMN_NAME = 'AccountStatusID'  
     AND TABLE_NAME LIKE '#MagicTable%'  
    )  
  BEGIN  
   ALTER TABLE #MagicTable ADD AccountStatusID VARCHAR(50)  
  END  
  
  IF NOT EXISTS (  
    SELECT *  
    FROM TempDB.INFORMATION_SCHEMA.COLUMNS  
    WHERE COLUMN_NAME = 'StatusCode'  
     AND TABLE_NAME LIKE '#MagicTable%'  
    )  
  BEGIN  
   ALTER TABLE #MagicTable ADD StatusCode VARCHAR(5)  
  END  
  
  IF NOT EXISTS (  
    SELECT *  
    FROM TempDB.INFORMATION_SCHEMA.COLUMNS  
    WHERE COLUMN_NAME = 'ExpectedBranchID'  
     AND TABLE_NAME LIKE '#MagicTable%'  
    )  
  BEGIN  
   ALTER TABLE #MagicTable ADD ExpectedBranchID VARCHAR(4)  
  END  
  
  IF NOT EXISTS (  
    SELECT *  
    FROM TempDB.INFORMATION_SCHEMA.COLUMNS  
    WHERE COLUMN_NAME = 'CurrencyID'  
     AND TABLE_NAME LIKE '#MagicTable%'  
    )  
  BEGIN  
   ALTER TABLE #MagicTable ADD CurrencyID VARCHAR(5)  
  END  
  
  IF NOT EXISTS (  
    SELECT *  
    FROM TempDB.INFORMATION_SCHEMA.COLUMNS  
    WHERE COLUMN_NAME = 'TrxtypeID'  
     AND TABLE_NAME LIKE '#MagicTable%'  
    )  
  BEGIN  
   ALTER TABLE #MagicTable ADD TrxtypeID VARCHAR(2)  
  END  
  
  IF NOT EXISTS (  
    SELECT *  
    FROM TempDB.INFORMATION_SCHEMA.COLUMNS  
    WHERE COLUMN_NAME = 'BankID'  
     AND TABLE_NAME LIKE '#MagicTable%'  
    )  
  BEGIN  
   ALTER TABLE #MagicTable ADD BankID VARCHAR(3)  
  END  
  
  IF NOT EXISTS (  
    SELECT *  
    FROM TempDB.INFORMATION_SCHEMA.COLUMNS  
    WHERE COLUMN_NAME = 'PayeeBranchCode'  
     AND TABLE_NAME LIKE '#MagicTable%'  
    )  
  BEGIN  
   ALTER TABLE #MagicTable ADD PayeeBranchCode VARCHAR(10)  
  END  
  
  IF NOT EXISTS (  
    SELECT *  
    FROM TempDB.INFORMATION_SCHEMA.COLUMNS  
    WHERE COLUMN_NAME = 'PayeeName'  
     AND TABLE_NAME LIKE '#MagicTable%'  
    )  
  BEGIN  
   ALTER TABLE #MagicTable ADD PayeeName VARCHAR(10)  
  END  
  
  IF NOT EXISTS (  
    SELECT *  
    FROM TempDB.INFORMATION_SCHEMA.COLUMNS  
    WHERE COLUMN_NAME = 'PayeeBankAccountNumber'  
     AND TABLE_NAME LIKE '#MagicTable%'  
    )  
  BEGIN  
   ALTER TABLE #MagicTable ADD PayeeBankAccountNumber VARCHAR(30)  
  END  
  
  IF NOT EXISTS (  
    SELECT *  
    FROM TempDB.INFORMATION_SCHEMA.COLUMNS  
    WHERE COLUMN_NAME = 'PayeeBankCode'  
     AND TABLE_NAME LIKE '#MagicTable%'  
    )  
  BEGIN  
   ALTER TABLE #MagicTable ADD PayeeBankCode VARCHAR(4)  
  END  
  
  IF NOT EXISTS (  
    SELECT *  
    FROM TempDB.INFORMATION_SCHEMA.COLUMNS  
    WHERE COLUMN_NAME = 'DRCRAccountID'  
     AND TABLE_NAME LIKE '#MagicTable%'  
    )  
  BEGIN  
   ALTER TABLE #MagicTable ADD DRCRAccountID VARCHAR(25)  
  END  
  
  IF NOT EXISTS (  
    SELECT *  
    FROM TempDB.INFORMATION_SCHEMA.COLUMNS  
    WHERE COLUMN_NAME = 'CustomerBankAccountNumber'  
     AND TABLE_NAME LIKE '#MagicTable%'  
    )  
  BEGIN  
   ALTER TABLE #MagicTable ADD CustomerBankAccountNumber VARCHAR(25)  
  END  
  
  IF NOT EXISTS (  
    SELECT *  
    FROM TempDB.INFORMATION_SCHEMA.COLUMNS  
    WHERE COLUMN_NAME = 'CustomerBankCode'  
     AND TABLE_NAME LIKE '#MagicTable%'  
    )  
  BEGIN  
   ALTER TABLE #MagicTable ADD CustomerBankCode VARCHAR(5)  
  END  
  
  IF NOT EXISTS (  
    SELECT *  
    FROM TempDB.INFORMATION_SCHEMA.COLUMNS  
    WHERE COLUMN_NAME = 'AccountBalance'  
     AND TABLE_NAME LIKE '#MagicTable%'  
    )  
  BEGIN  
   ALTER TABLE #MagicTable ADD AccountBalance MONEY  
  END  
  
  IF NOT EXISTS (  
    SELECT *  
    FROM TempDB.INFORMATION_SCHEMA.COLUMNS  
    WHERE COLUMN_NAME = 'TrxDescription'  
     AND TABLE_NAME LIKE '#MagicTable%'  
    )  
  BEGIN  
   ALTER TABLE #MagicTable ADD TrxDescription MONEY  
  END  
  
  IF NOT EXISTS (  
    SELECT *  
    FROM TempDB.INFORMATION_SCHEMA.COLUMNS  
    WHERE COLUMN_NAME = 'ProductID'  
     AND TABLE_NAME LIKE '#MagicTable%'  
    )  
  BEGIN  
   ALTER TABLE #MagicTable ADD ProductID VARCHAR(10)  
  END  
  
  IF NOT EXISTS (  
    SELECT *  
    FROM TempDB.INFORMATION_SCHEMA.COLUMNS  
    WHERE COLUMN_NAME = 'Description'  
     AND TABLE_NAME LIKE '#MagicTable%'  
    )  
  BEGIN  
   ALTER TABLE #MagicTable ADD Description VARCHAR(300)  
  END  
  
  IF NOT EXISTS (  
    SELECT *  
    FROM TempDB.INFORMATION_SCHEMA.COLUMNS  
    WHERE COLUMN_NAME = 'Trxtype'  
     AND TABLE_NAME LIKE '#MagicTable%'  
    )  
  BEGIN  
   ALTER TABLE #MagicTable ADD Trxtype VARCHAR(25)  
  END  
  
  IF NOT EXISTS (  
    SELECT *  
    FROM TempDB.INFORMATION_SCHEMA.COLUMNS  
    WHERE COLUMN_NAME = 'Amount'  
     AND TABLE_NAME LIKE '#MagicTable%'  
    )  
  BEGIN  
   ALTER TABLE #MagicTable ADD Amount MONEY  
  END  
  
  IF NOT EXISTS (  
    SELECT *  
    FROM TempDB.INFORMATION_SCHEMA.COLUMNS  
    WHERE COLUMN_NAME = 'ValueDate'  
     AND TABLE_NAME LIKE '#MagicTable%'  
    )  
  BEGIN  
   ALTER TABLE #MagicTable ADD ValueDate SMALLDATETIME NULL  
  END  
  ---############################################################################--  
  --HARDCODED SOME ACTIVITIES FOR ATM PURPOSES. MAYBE IN FUTURE WE MIGHT CHANGE THIS  
  IF @FormatID = 'ATM'  
  BEGIN  
   UPDATE #MagicTable  
   SET OurBranchID = ltrim(rtrim(OurBranchID))  
    ,AccountID = ltrim(rtrim(AccountID))  
    ,Amount = ltrim(rtrim(Amount))  
  
   UPDATE #MagicTable  
   SET OurBranchID = t_accountCustomer.OurBranchID  
   FROM #MagicTable  
   INNER JOIN t_accountCustomer(NOLOCK) ON #MagicTable.AccountID = t_accountCustomer.AccountID  
  
   UPDATE #MagicTable  
   SET Name = t_accountCustomer.Name  
   FROM #MagicTable  
   INNER JOIN t_accountCustomer(NOLOCK) ON #MagicTable.OurBranchID = t_accountCustomer.OurBranchID  
    AND #MagicTable.AccountID = t_accountCustomer.AccountID  
  
   UPDATE #MagicTable  
   SET Amount = LEFT(Amount, LEN(Amount) - 2) + '.' + RIGHT(Amount, 2)  
    --ALTER TABLE #MagicTable  
    --ALTER COLUMN Amount NUMERIC(19,2)  
  END  
  
  --BASIC UPDATES TO BASED ON OUR SYSTEM TABLES  
  IF EXISTS (  
    SELECT *  
    FROM TempDB.INFORMATION_SCHEMA.COLUMNS  
    WHERE COLUMN_NAME = 'EmployeeNo'  
     AND TABLE_NAME LIKE '#MagicTable%'  
    )  
  BEGIN --IF ACCOUNTID IS NOT PART OF THE FORMAT FIELDS, THEN WE UPDATE THE VALUE HERE  
   IF NOT EXISTS (  
     SELECT ColumnName  
     FROM @FormatInField  
     WHERE ColumnName = 'AccountID'  
     )  
   BEGIN  
    UPDATE #MagicTable  
    SET AccountID = t_AccountCustomer.AccountID  
    FROM #MagicTable MagicTable  
    INNER JOIN t_AccountCustomer(NOLOCK) ON MagicTable.EmployeeNo = t_AccountCustomer.EmpID  
    WHERE MagicTable.AccountID IS NULL  
   END  
  
   IF NOT EXISTS (  
     SELECT ColumnName  
     FROM @FormatInField  
     WHERE ColumnName = 'OurBranchID'  
     )  
   BEGIN  
    UPDATE #MagicTable  
    SET OurBranchID = t_AccountCustomer.OurBranchID  
    FROM #MagicTable MagicTable  
    INNER JOIN t_AccountCustomer(NOLOCK) ON MagicTable.EmployeeNo = t_AccountCustomer.EmpID  
    WHERE MagicTable.AccountID IS NULL  
   END  
  END  
  
  IF NOT EXISTS (  
    SELECT *  
    FROM TempDB.INFORMATION_SCHEMA.COLUMNS  
    WHERE COLUMN_NAME = 'EmployeeNo'  
     AND TABLE_NAME LIKE '#MagicTable%'  
    )  
  BEGIN  
   ALTER TABLE #MagicTable ADD EmployeeNo VARCHAR(100)  
  END  
  
  IF @FormatID = N'002' --Clearing Salary Uploads  
  BEGIN  
   --SELECT @BankID BankID  
   UPDATE #MagicTable  
   SET BankID = PayeeBankCode  
  
   --SELECT * FROM #MagicTable  
   UPDATE #MagicTable  
   SET AccountID = t_AccountCustomer.AccountID  
    ,OurBranchID = t_AccountCustomer.OurBranchID  
   FROM #MagicTable  
   INNER JOIN t_AccountCustomer(NOLOCK) ON isNull(#MagicTable.CustomerBankAccountNumber, #MagicTable.PayeeBankAccountNumber) = t_AccountCustomer.AccountID  
    AND #MagicTable.BankID = @BankID  
  
   UPDATE #MagicTable  
   SET PayeeBranchCode = OurBranchID  
    ,AccountID = PayeeBankAccountNumber -- = CustomerBankAccountNumber  
  END  
  
  --FOR CUSTOMER ACCOUNTS  
  UPDATE #MagicTable  
  SET ExpectedBranchID = t_AccountCustomer.OurBranchID  
  FROM #MagicTable  
  INNER JOIN t_AccountCustomer(NOLOCK) ON #MagicTable.AccountID = t_AccountCustomer.AccountID  
  
  --FOR GENERAL LEDGERS  
  UPDATE #MagicTable  
  SET ExpectedBranchID = t_glBranch.OurBranchID  
  FROM #MagicTable  
  INNER JOIN t_glBranch(NOLOCK) ON #MagicTable.AccountID = t_glBranch.AccountID  
  WHERE t_glBranch.OurBranchID = Isnull(#MagicTable.OurBranchID, @OurBranchID)  
   AND ISNULL(ExpectedBranchID, '') = ''  
  
  IF @FormatID <> '002'  
  BEGIN  
   UPDATE #MagicTable  
   SET OurBranchID = ExpectedBranchID  
   WHERE ISNULL(OurBranchID, '') <> ISNULL(ExpectedBranchID, '')  
  END  
  
  UPDATE #MagicTable  
  SET Name = t_AccountCustomer.Name  
   ,ProductID = t_AccountCustomer.ProductID  
   ,ClearBalance = t_AccountCustomer.ClearBalance  
   ,StatusCode = t_AccountCustomer.AccountStatusID  
   ,AccountStatusID = CASE @LanguageID  
    WHEN 'en'  
     THEN dbo.f_GetSystemCodeName('AccountStatusID', t_AccountCustomer.AccountStatusID)  
    ELSE dbo.f_GetSystemDescription('AccountStatusID' + t_AccountCustomer.AccountStatusID, @LanguageID)  
    END  
   ,AccountBalance = dbo.f_GetAvailableBalance(#MagicTable.OurBranchID, #MagicTable.AccountID)  
  FROM #MagicTable  
  INNER JOIN t_AccountCustomer(NOLOCK) ON #MagicTable.OurBranchID = t_AccountCustomer.OurBranchID  
   AND #MagicTable.AccountID = t_AccountCustomer.AccountID  
  WHERE #MagicTable.AccountID IS NOT NULL  
  
  --GL ACCOUNTS    
  UPDATE #MagicTable  
  SET ProductID = 'GL'  
   ,StatusCode = 'AA'  
   ,AccountStatusID = 'Active'  
   ,Name = t_GeneralLedger.Description  
   ,CurrencyID = t_GeneralLedger.CurrencyID  
  FROM #MagicTable  
  INNER JOIN t_GLBranch(NOLOCK) ON #MagicTable.OurBranchID = t_GLBranch.OurBranchID  
   AND #MagicTable.AccountID = t_GLBranch.AccountID  
  INNER JOIN t_GeneralLedger(NOLOCK) ON t_GeneralLedger.AccountID = t_GLBranch.AccountID  
  WHERE #MagicTable.AccountID IS NOT NULL  
   AND GLAccountStatusID = 'A'  
  
  UPDATE #MagicTable  
  SET OurBranchID = Right(CustomerBankCode, Len(CustomerBankCode) - Len(Left(CustomerBankCode, 2)))  
  WHERE @BankID <> RIGHT(PayeeBankCode, 2)  
  
  UPDATE #MagicTable  
  SET OurBranchID = 'V'  
  WHERE OurBranchID IS NULL  
  
  UPDATE #MagicTable  
  SET MiniMumBalance = t_ProductBranchDetail.MinimumBalance  
  FROM #MagicTable  
  INNER JOIN t_ProductBranchDetail(NOLOCK) ON #MagicTable.OurBranchID = t_ProductBranchDetail.OurBranchID  
   AND #MagicTable.ProductID = t_ProductBranchDetail.ProductID  
  
  IF @FormatID <> N'002' --Clearing Salary Uploads  
  BEGIN  
  IF EXISTS (  
    SELECT *  
    FROM TempDB.INFORMATION_SCHEMA.COLUMNS  
    WHERE COLUMN_NAME = 'PayeeBankCode'  
     AND TABLE_NAME LIKE '#MagicTable%'  
    )  
   UPDATE #MagicTable  
   SET BankID = LEFT(PayeeBankCode, 2)  
    ,PayeeBranchCode = Right(PayeeBankCode, Len(PayeeBankCode) - Len(Left(PayeeBankCode, 2)))  
   WHERE @BankID <> LEFT(PayeeBankCode, 3)  
  END  
  ELSE  
  BEGIN  
   IF EXISTS (  
     SELECT *  
     FROM TempDB.INFORMATION_SCHEMA.COLUMNS  
     WHERE COLUMN_NAME = 'PayeeBankCode'  
      AND TABLE_NAME LIKE '#MagicTable%'  
     )  
    UPDATE #MagicTable  
    SET BankID = RIGHT(PayeeBankCode, 2)  
     ,Name = PayeeName  
    WHERE @BankID <> RIGHT(PayeeBankCode, 2)  
  END  
  --Not Allow Posting to Loan Accounts  
  --UPDATE #MagicTable   
  --SET StatusCode  = 'INV',  
  --EmployeeNo = 'Loan Account'    
  --WHERE ProductID in (SELECT ProductID FROM t_Product(Nolock) WHERE productTypeID='LN')  
  IF @FormatID <> N'002' --Clearing Salary Uploads  
  BEGIN  
  UPDATE #MagicTable  
  SET StatusCode = 'INV'  
   ,EmployeeNo = 'Saving will go to Debit'  
  FROM #MagicTable  
  INNER JOIN t_Product(NOLOCK) ON #MagicTable.ProductID = t_Product.ProductID  
  WHERE (#MagicTable.ClearBalance - #MagicTable.MiniMumBalance) + Cast(RTRIM(LTRIM(Amount)) AS NUMERIC) < 0.00  
   AND t_Product.ProductTypeID = 'SB'  
   AND Cast(RTRIM(LTRIM(Amount)) AS NUMERIC) < 0.00  
  
  UPDATE #MagicTable  
  SET AccountStatusID = CASE @LanguageID  
    WHEN 'en'  
     THEN dbo.f_GetSystemCodeName('AccountTag', 'I')  
    ELSE ISNULL(dbo.f_GetSystemDescription('AccountTagI', @LanguageID), dbo.f_GetSystemCodeName('AccountTag', 'I'))  
    END  
   ,StatusCode = 'INV'  
  WHERE ISNULL(AccountStatusID, '') = ''  
  
  UPDATE #MagicTable  
  SET EmployeeNo = 'AccountID is ' + AccountStatusID  
  WHERE AccountStatusID <> 'Active'  
  
  UPDATE #MagicTable  
  SET CurrencyID = t_Product.CurrencyID  
  FROM #MagicTable  
  INNER JOIN t_Product(NOLOCK) ON #MagicTable.ProductID = t_Product.ProductID  
  
  UPDATE #MagicTable  
  SET EmployeeNo = 'InValid Account Currency(' + CurrencyID + ')'  
   ,StatusCode = 'INV'  
  WHERE CurrencyID <> @CurrencyID --AND ISNULL(EmployeeNo,'') = ''  
  END  
  ELSE IF @FormatID = N'002'  
  BEGIN  
   ALTER TABLE #MagicTable ADD BankName VARCHAR(50)  
  
   SELECT TOP 1 @OurBankID = BankID  
   FROM t_SystemBankSetting(NOLOCK)  
  
   UPDATE #MagicTable  
   SET StatusCode = 'INV'  
    ,EmployeeNo = 'Saving will go to Debit'  
   FROM #MagicTable  
   INNER JOIN t_Product(NOLOCK) ON #MagicTable.ProductID = t_Product.ProductID  
   WHERE (#MagicTable.ClearBalance - #MagicTable.MiniMumBalance) + Cast(RTRIM(LTRIM(Amount)) AS NUMERIC) < 0.00  
    AND t_Product.ProductTypeID = 'SB'  
    AND Cast(RTRIM(LTRIM(Amount)) AS NUMERIC) < 0.00  
    AND #MagicTable.BankID = @OurBankID  
  
   UPDATE #MagicTable  
   SET AccountStatusID = CASE @LanguageID  
     WHEN 'en'  
      THEN dbo.f_GetSystemCodeName('AccountTag', 'I')  
     ELSE ISNULL(dbo.f_GetSystemDescription('AccountTagI', @LanguageID), dbo.f_GetSystemCodeName('AccountTag', 'I'))  
     END  
    ,StatusCode = 'INV'  
   WHERE ISNULL(AccountStatusID, '') = ''  
    AND BankID = @OurBankID  
  
   UPDATE #MagicTable  
   SET EmployeeNo = 'AccountID is ' + AccountStatusID  
   WHERE AccountStatusID <> 'Active'  
    AND BankID = @OurBankID  
  
   UPDATE #MagicTable  
   SET CurrencyID = t_Product.CurrencyID  
   FROM #MagicTable  
   INNER JOIN t_Product(NOLOCK) ON #MagicTable.ProductID = t_Product.ProductID  
    AND #MagicTable.BankID = @OurBankID  
  
   UPDATE #MagicTable  
   SET EmployeeNo = 'InValid Account Currency(' + CurrencyID + ')'  
    ,StatusCode = 'INV'  
   WHERE CurrencyID <> @CurrencyID  
    AND BankID = @OurBankID --AND ISNULL(EmployeeNo,'') = ''  
  
   UPDATE #MagicTable  
   SET StatusCode = 'AA'  
    ,AccountStatusID = 'Active'  
   WHERE BankID <> @OurBankID  
  
  END  
  /*UPDATE #MagicTable   
  SET EmployeeNo = 'Multiple Posting Detected ' + cast(Multiples.Cnt as Varchar) + ' times',  
      StatusCode = 'INV'  
  FROM #MagicTable  
  INNER JOIN  
  (SELECT PreProcess.AccountID, PreProcess.Cnt  
  FROM(SELECT AccountID,Count(AccountID) Cnt FROM #MagicTable  
  GROUP BY AccountID HAVING Count(AccountID) >1) PreProcess  
  INNER JOIN  t_AccountCustomer(Nolock) ON PreProcess.AccountID = t_AccountCustomer.AccountID) Multiples  
  ON #MagicTable.AccountID = Multiples.AccountID*/  
  ----------------------------------------------------------------------------------------  
  IF EXISTS (  
    SELECT AccountID  
    FROM #MagicTable  
    WHERE AccountID IN (  
      SELECT AccountID  
      FROM t_AccountCustomer(NOLOCK)  
      WHERE ProductID IN (  
        SELECT ProductID  
        FROM t_Product  
        WHERE ProductTypeID IN (  
          'LN'  
          ,'FD'  
          ,'SH'  
          )  
        )  
      )  
    )  
  BEGIN  
   SET @ErrorNo = 'BREXDB300101'  
  
   RAISERROR (  
     @ErrorNo  
     ,16  
     ,1  
     )  
  
   SELECT @ErrorNo AS STATUS  
  
   RETURN  
  END  
  
  ----------------------------------------------------------------------------------------  
  --WE KEEP THE DATA TEMPORARY HERE FOR THE POSTING SP TO GET FROM IT  
  DELETE  
  FROM t_PreProcessUpload  
  WHERE CreatedBy = @OperatorID  
   AND FilePath = @FilePath  
  
  DELETE  
  FROM #MagicTable  
  WHERE ISNULL(AccountID, '') = ''  
   AND ISNULL(Amount, '') = ''  
  
  INSERT INTO t_PreProcessUpload (  
   TrxBranchID  
   ,OurBranchID  
   ,AccountID  
   ,EmployeeID  
   ,Amount  
   ,StatusCode  
   ,CreatedBy  
   ,CreatedOn  
   ,FilePath  
   ,TrxtypeID  
   ,BankID  
   ,PayeeBranchCode  
   ,PayeeBankAccountNumber  
   ,PayeeName  
   ,DRCRAccountID  
   ,TrxNarration  
   )  
  SELECT @OurBranchID  
   ,OurBranchID  
   ,AccountID  
   ,EmployeeNo  
   ,Amount  
   ,StatusCode  
   ,@OperatorID  
   ,SYSDATETIME()  
   ,@FilePath  
   ,CASE Trxtype  
    WHEN 'D'  
     THEN 'TD'  
    WHEN 'C'  
     THEN 'TC'  
    ELSE Trxtype  
    END  
   ,ISnull(BankID, dbo.f_GetBankID(@OurBranchID))  
   ,PayeeBranchCode  
   ,PayeeBankAccountNumber  
   ,CASE   
    WHEN RIGHT(@BankID,2) <> RIGHT(PayeeBankCode, 2)  
     THEN PayeeName  
    ELSE NULL  
    END  
   ,CASE   
    WHEN RIGHT(@BankID,2) <> RIGHT(PayeeBankCode, 2)  
     THEN CustomerBankAccountNumber  
    ELSE NULL  
    END  
   ,TrxDescription  
  FROM #MagicTable  
  ORDER BY OurBranchID  
   ,AccountStatusID  
  
  -- Additional Validation B4 Posting  
  --CHECK IF INVALID ACCOUNTS HAVE BEEN SET FOR THE BRANCH. IF NOT, FIRE ERROR NOTIFICATION HERE  
  --Select AccountID FROM t_GatewayInvalidAccountDetail(NOLOCK)  
  --  WHERE BankID = @BankID AND OurBranchID = @OurBranchID AND CurrencyID = @CurrencyID AND FormatID = @FormatID  
  IF @FormatID <> '002'  
  BEGIN  
  IF NOT EXISTS (  
    SELECT AccountID  
    FROM t_GatewayInvalidAccountDetail(NOLOCK)  
    WHERE BankID = @BankID  
     AND OurBranchID = @OurBranchID  
     AND CurrencyID = @CurrencyID  
     AND FormatID = @FormatID  
    )  
  BEGIN  
   SET @ErrorNo1 = '455505'  
   SET @ErrorNo = 'BREXDB455505(' + CAST(@OurBranchID AS VARCHAR(100)) + ')'  
  
   RAISERROR (  
     @ErrorNo  
     ,16  
     ,1  
     )  
  
   RETURN  
  END  
  END   
  IF ISNULL(@CurrencyID, '') = ''  
  BEGIN  
   SET @ErrorNo1 = '860512'  
   SET @ErrorNo = 'BREXDB860512(' + CAST(@OurBranchID AS VARCHAR(15)) + ')'  
  
   RAISERROR (  
     @ErrorNo  
     ,16  
     ,1  
     )  
  
   RETURN  
  END  
  IF @FormatID = N'002' --Clearing  
  BEGIN  
   DECLARE @HOBranchID dbo.BranchID  
    ,  
    --@OurBankID    varchar(4),  
    @HOBranchLocalBranchCurrency CurrencyID  
  
   SELECT TOP 1 @OurBankID = BankID  
   FROM t_SystemBankSetting(NOLOCK)  
  
   SELECT @HOBranchID = dbo.f_GetHOBranchID(@OurBankID)  
  
   SELECT @HOBranchLocalBranchCurrency = (  
     SELECT CurrencyID  
     FROM t_SystemBranchSetting NOLOCK  
     WHERE OurBranchID = @HOBranchID  
     )  
  
   UPDATE #MagicTable  
   SET BankName = dbo.f_GetClearingBankName(PayeeBankCode)  
  
  
   SELECT TOP 500 @OurBranchID AS OurBranchID  
    ,ISNULL(AccountID, CustomerBankAccountNumber) AccountID  
    ,Amount  
    ,ISNULL(dbo.f_GetAccountProductID(OurBranchID, AccountID), '') ProductID  
    ,REPLACE(REPLACE(Name, '   ', ' '), '  ', ' ') Name  
    ,ISNULL(EmployeeNo, '') EmployeeID  
    ,AccountStatusID  
    ,StatusCode  
    ,IsNull(CurrencyID, @HOBranchLocalBranchCurrency) CurrencyID  
    ,@Count NoOfRecords  
    ,BankName  
   FROM #MagicTable  
   ORDER BY OurBranchID DESC  
    ,StatusCode ASC  
  
   RETURN  
  END  
  ELSE  
  BEGIN  
  SELECT TOP 500 OurBranchID  
   ,AccountID  
   ,Amount  
   ,ProductID  
   ,REPLACE(REPLACE(Name, '   ', ' '), '  ', ' ') Name  
   ,ISNULL(EmployeeNo, '') EmployeeID  
   ,AccountStatusID  
   ,StatusCode  
   ,CurrencyID  
   ,AccountBalance  
  FROM #MagicTable  
  ORDER BY StatusCode DESC  
   ,OurBranchID  
 END  
 END  
  
  
  
 SET NOCOUNT OFF  
 SET DEADLOCK_PRIORITY NORMAL  
END  
  

