Text
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE    PROCEDURE [dbo].Proc_CreateXMLFiles_Tanzania_Selected_CTS (
	@OurBankID VARCHAR(5), -- OurBankID is Kept 3 Here - Do Not Change                
	@OurBranchID VARCHAR(5), -- OurBranchID is Kept 3 Here - Do Not Change                
	@ReadDate DATETIME, @FromDate DATETIME, @EJDate DATETIME, -- Date On The EJ Files                
	@ClearingCenters VARCHAR(2), @AllCenters BIT, @Currency INT, @Session INT = 0, @xmlList XML
	)
AS
--RETURN
SET DATEFORMAT dmy;

DECLARE @Curr VARCHAR(4), @WorkingDate SMALLDATETIME, @TrxRowID BIGINT, @ImageID BIGINT

SELECT @WorkingDate = dbo.f_GetWorkingDate(@OurBranchID)

SET @ReadDate = convert(DATETIME, convert(VARCHAR(10), @ReadDate, 103), 103)
--cast(cast(datepart(dd, @ReadDate) as varchar) + '-' 
--+ cast(datepart(mm, @ReadDate) as varchar)  + '-' 
--+ cast(datepart(yyyy, @ReadDate) as varchar) as datetime)	
SET @FromDate = convert(DATETIME, convert(VARCHAR(10), @FromDate, 103), 103)

--cast(cast(datepart(dd, @FromDate) as varchar) + '-' 
--+ cast(datepart(mm, @FromDate) as varchar)  + '-' 
--+ cast(datepart(yyyy, @FromDate) as varchar) as datetime)
--SET @EJDate =  convert(datetime,convert(varchar(10),@EJDate,104),104)

UPDATE t_TrxClearing
SET ReturnCodeID = '00'
Where ReturnCodeID ='00 -'

UPDATE t_AccountTrxClearing
SET ReturnCodeID = '00'
Where ReturnCodeID ='00 -'

BEGIN TRY
	SELECT @EJDate = TRY_CONVERT(DATETIME, TRY_CONVERT(VARCHAR(10), @EJDate, 103), 103)
END TRY

BEGIN CATCH
	BEGIN TRY
		SELECT @EJDate = TRY_CONVERT(DATETIME, TRY_CONVERT(VARCHAR(10), @EJDate, 104), 104)
	END TRY

	BEGIN CATCH
		SELECT @EJDate = TRY_CONVERT(DATETIME, TRY_CONVERT(VARCHAR(10), @EJDate, 102), 102)
	END CATCH
END CATCH

BEGIN TRY
	UPDATE t_IncomingClrTrxExtraDetails
	SET ReqdColltnDate = Convert(DATE, ReqdColltnDt)
	WHERE Month(GetDate()) < Month(CONVERT(VARCHAR, CONVERT(DATETIME, CONVERT(DATETIME, CONVERT(VARCHAR(10), dbo.ChangeDateFormatFromUK(
								ReqdColltnDt), 101), 101)), 106))
		AND TrxDate = dbo.f_GetWorkingDate(@OurBranchID)
END TRY

BEGIN CATCH
END CATCH

CREATE TABLE #MICRCheque (
	OurBranchID VARCHAR(6), AccountID VARCHAR(50), ProductID VARCHAR(8), TrxDate DATETIME, ValueDate DATETIME, SerialID INT, AccountType CHAR(2), 
	TrxType CHAR(2), VoucherCode VARCHAR(4), ReturnCode VARCHAR(8), Amount MONEY, BankID VARCHAR(4), BranchID VARCHAR(6), ChequeId VARCHAR(15), 
	ChequeDigit VARCHAR(2), TheirAccountID VARCHAR(50), SwiftCode VARCHAR(15), DestinationSwiftCode VARCHAR(15), CurrencyCode VARCHAR(4), 
	ISOCurrencyID VARCHAR(4), RemittersName VARCHAR(100), BeneficiaryName VARCHAR(100), MicrLineDetails VARCHAR(50), ProcessingNumber NUMERIC(18
		, 0), DRN NUMERIC(18, 0), FrontImageGrayScale TEXT, FrontImageUV TEXT, BackImageGrayScale TEXT, FrontImageTiff TEXT, CreationDate DATETIME, 
	TransactionID NUMERIC(18, 0), TransactionMicrColumnID VARCHAR(100), IsGenerated BIT, Reference VARCHAR(50), OutwardSerialID NUMERIC(18, 0), 
	ColumnID VARCHAR(100), OriginalColumnID NUMERIC(18, 0), TrxID VARCHAR(200), RetCodeDesc VARCHAR(250), OrgnlInstrID VARCHAR(100), OrgnlEndToEnd 
	VARCHAR(100), MndtId VARCHAR(100), DtOfSgntr VARCHAR(100), ReqdColltnDt VARCHAR(100), Frqcy VARCHAR(100), UstrdColD VARCHAR(100), UstrdChqdt 
	VARCHAR(100), UstrdBWF VARCHAR(100), UstrdBWR VARCHAR(100), UstrdGS VARCHAR(100), UstrdUV VARCHAR(100), UstrdMicr VARCHAR(100), DAdrLine 
	VARCHAR(100), DTwnNm VARCHAR(100), DCtry VARCHAR(100), DNm VARCHAR(100), DPhneNb VARCHAR(100), DMobNb VARCHAR(100), DEmailAdr VARCHAR(100), 
	DOthr VARCHAR(100), CAdrLine VARCHAR(100), CTwnNm VARCHAR(100), CCtry VARCHAR(100), CNm VARCHAR(100), CPhneNb VARCHAR(100), CMobNb VARCHAR(100), 
	CEmailAdr VARCHAR(100), COthr VARCHAR(100), DCNm VARCHAR(100), CCNm VARCHAR(100), DbtrAcct VARCHAR(50), CdtrAcct VARCHAR(50), OrgnlMsgId VARCHAR
	(100), SwftCd VARCHAR(30), SvcLvl VARCHAR(30), LclInstrm VARCHAR(30), CtgyPurp VARCHAR(30), OrgnTrxID VARCHAR(70)
	)

CREATE TABLE #MICREFTs (
	OurBranchID VARCHAR(6), AccountID VARCHAR(50), ProductID VARCHAR(8), DATE DATETIME, ValueDate DATETIME, AccountType CHAR(4), TrxType CHAR(4), 
	VoucherCode VARCHAR(4), ReturnCode VARCHAR(8), Amount MONEY, BankID VARCHAR(4), BranchID VARCHAR(6), TheirAccountID VARCHAR(50), SwiftCode 
	VARCHAR(20), DestinationSwiftCode VARCHAR(20), CurrencyCode VARCHAR(4), ISOCurrencyID VARCHAR(4), BeneficiaryName VARCHAR(100), 
	RemittersName VARCHAR(100), -- our customer's name                
	ProcessingNumber VARCHAR(20), EFTID VARCHAR(30), OriginatorCode VARCHAR(8), OriginatorReference VARCHAR(30), PolicyNumber1 VARCHAR(20), 
	PolicyNumber2 VARCHAR(20), Remarks VARCHAR(150), ISDebitORISCredit BIT, CreationDate DATETIME, TransactionMicrColumnID VARCHAR(100), 
	IsGenerated BIT, Reference VARCHAR(50), OutwardSerialID NUMERIC(18, 0), ColumnID VARCHAR(100), OriginalColumnID NUMERIC(18, 0), TrxID VARCHAR(
		200), RetCodeDesc VARCHAR(250), OrgnlInstrID VARCHAR(100), OrgnlEndToEnd VARCHAR(100), MndtId VARCHAR(100), DtOfSgntr VARCHAR(100), 
	ReqdColltnDt VARCHAR(100), FnlColltnDt VARCHAR(100), Frqcy VARCHAR(100), UstrdColD VARCHAR(100), UstrdChqdt VARCHAR(100), UstrdBWF VARCHAR(100
	), UstrdBWR VARCHAR(100), UstrdGS VARCHAR(100), UstrdUV VARCHAR(100), UstrdMicr VARCHAR(100), DAdrLine VARCHAR(100), DTwnNm VARCHAR(100), DCtry 
	VARCHAR(100), DNm VARCHAR(100), DPhneNb VARCHAR(100), DMobNb VARCHAR(100), DEmailAdr VARCHAR(100), DOthr VARCHAR(100), CAdrLine VARCHAR(100), 
	CTwnNm VARCHAR(100), CCtry VARCHAR(100), CNm VARCHAR(100), CPhneNb VARCHAR(100), CMobNb VARCHAR(100), CEmailAdr VARCHAR(100), COthr VARCHAR(100)
	, DCNm VARCHAR(100), CCNm VARCHAR(100), DbtrAcct VARCHAR(50), CdtrAcct VARCHAR(50), OrgnlMsgId VARCHAR(100), SwftCd VARCHAR(30), SvcLvl VARCHAR(
		30), LclInstrm VARCHAR(30), CtgyPurp VARCHAR(30), OrgnTrxID VARCHAR(70), OrgnlIntrBkSttlmDt VARCHAR(30)
	)

SET @Curr = CASE 
		WHEN @Currency = 0
			THEN 'TZS'
		WHEN @Currency = 1
			THEN 'USD'
		WHEN @Currency = 2
			THEN 'GBP'
		WHEN @Currency = 3
			THEN 'EUR'
		WHEN @Currency = 4
			THEN 'JPY'
		END

CREATE TABLE #TempCurrency (CurrencyID VARCHAR(8))

CREATE TABLE #TempOutClearing (ReturncodeID VARCHAR(4), TrxBatchID BIGINT, ColumnID VARCHAR(100), TrxTypeID VARCHAR(4), DATE SMALLDATETIME, Amount NUMERIC(18, 2)
	)




IF isNull(@OurBankID, '') = ''
BEGIN
	SELECT @OurBankID = (
			SELECT BankID
			FROM t_SystemBankSetting
			)
END

INSERT INTO #TempOutClearing (ReturncodeID, TrxBatchID, ColumnID, TrxTypeID)
SELECT ReturncodeID, TrxBatchID, ColumnID, TrxType
FROM v_Clearing
WHERE TrxType IN (
		'ID'
		,'IC'
		)
	AND ReturncodeID = '00'
	AND DATE BETWEEN @FromDate
		AND @ReadDate

UNION ALL

SELECT ReturncodeID, TrxBatchID, ColumnID, TrxType
FROM v_Clearing
WHERE TrxType IN (
		'ID'
		,'IC'
		)
	AND ReturncodeID = '00'
	AND DATE BETWEEN @FromDate
		AND @ReadDate

UPDATE t_TrxClearing
SET ColumnID = #TempOutClearing.ColumnID
FROM #TempOutClearing
WHERE t_TrxClearing.TrxBatchID = #TempOutClearing.TrxBatchID
	AND t_TrxClearing.ReturncodeID <> '00'
	AND isNull(t_TrxClearing.ColumnID, '') = ''
	AND TrxType IN (
		'OC'
		,'OD'
		)
	AND t_TrxClearing.DATE BETWEEN @FromDate
		AND @ReadDate

UPDATE t_AccountTrxClearing
SET ColumnID = #TempOutClearing.ColumnID
FROM #TempOutClearing
WHERE t_AccountTrxClearing.TrxBatchID = #TempOutClearing.TrxBatchID
	AND t_AccountTrxClearing.ReturncodeID <> '00'
	AND isNull(t_AccountTrxClearing.ColumnID, '') = ''
	AND TrxType IN (
		'OC'
		,'OD'
		)
	AND t_AccountTrxClearing.DATE BETWEEN @FromDate
		AND @ReadDate

UPDATE t_TrxClearing
SET BANKID = t_IncomingTransactions.BankID
FROM t_IncomingTransactions
WHERE t_TrxClearing.ColumnID = t_IncomingTransactions.ColumnID

------------------------------------------------------------------------------------------------------------------
IF @Curr = 'TZS'
BEGIN
	INSERT INTO #TempCurrency
	SELECT 'TZS'
END

IF @Curr = 'TSH'
BEGIN
	INSERT INTO #TempCurrency
	SELECT 'TSH'
END

IF @Curr IN ('USD')
BEGIN
	INSERT INTO #TempCurrency
	SELECT 'USD'
END
ELSE IF @Curr IN ('GBP')
BEGIN
	INSERT INTO #TempCurrency
	SELECT 'GBP'
END
ELSE IF @Curr IN ('EUR')
BEGIN
	INSERT INTO #TempCurrency
	SELECT 'EUR'
END
ELSE IF @Curr IN ('JPY')
BEGIN
	INSERT INTO #TempCurrency
	SELECT 'JPY'
END
ELSE IF @Curr IN ('KES')
BEGIN
	INSERT INTO #TempCurrency
	SELECT 'KES'
END

DECLARE MICRChequeTrx CURSOR
FOR
SELECT MICRChequeTx.x.value('TrxRowID[1]', 'nVARCHAR(60)')
FROM @xmlList.nodes('/ArrayOfLsItems/lsItems') AS MICRChequeTx(x)

OPEN MICRChequeTrx

FETCH NEXT
FROM MICRChequeTrx
INTO @TrxRowID

WHILE @@FETCH_STATUS = 0
BEGIN
	INSERT INTO #MICRCheque (
		OurBranchID, ProductID, AccountID, TrxDate, ValueDate, SerialID, AccountType, TrxType, VoucherCode, ReturnCode, Amount, BankID, BranchID, 
		ChequeId, ChequeDigit, TheirAccountID, SwiftCode, DestinationSwiftCode, CurrencyCode, ISOCurrencyID, RemittersName, BeneficiaryName, 
		MicrLineDetails, ProcessingNumber, DRN, FrontImageGrayScale, FrontImageUV, BackImageGrayScale, FrontImageTiff, CreationDate, 
		TransactionID, TransactionMicrColumnID, IsGenerated, Reference, OutwardSerialID, OriginalColumnID, TrxID, RetCodeDesc, OrgnlInstrID, 
		OrgnlEndToEnd, MndtId, DtOfSgntr, ReqdColltnDt, Frqcy, UstrdColD, UstrdChqdt, UstrdBWF, UstrdBWR, UstrdGS, UstrdUV, UstrdMicr, DAdrLine, 
		DTwnNm, DCtry, DNm, DPhneNb, DMobNb, DEmailAdr, DOthr, CAdrLine, CTwnNm, CCtry, CNm, CPhneNb, CMobNb, CEmailAdr, COthr, DCNm, CCNm, DbtrAcct, 
		CdtrAcct, OrgnlMsgId, SwftCd, SvcLvl, LclInstrm, CtgyPurp
		)
	SELECT RIGHT(OurBranchID, 3), dbo.f_GetAccountProductID(OurBranchID, AccountID) ProductID, AccountID, DATE, ValueDate, ImageID, AccountTypeID 
		AccountType, TrxType, VoucherCode, ReturnCodeID ReturnCode, ABS(Amount), BankID, RIGHT(BranchID, 3), ChequeId, ChequeDigit, CASE 
			WHEN ReturnCodeID = '00'
				THEN DrawerOrPayeeAccountID
			ELSE DrawerOrPayeeAccountID
			END TheirAccount, '', '', '', '', DrawerOrPayee, '', '', 0, 0, '', '', '', '', GETDATE(), ImageID OutwardSerialID, TrxRowID ColumnID, IsNull(
			IsGenerated, 0), Reference, ImageID OutwardSerialID, ColumnID, ValueDate, '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', 
		'', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', ''
	FROM v_Clearing(NOLOCK)
	WHERE TrxType = 'OC'
		AND IsNull(IsGenerated, 0) = 0
		AND isNull(IsDeleted, 0) = 0
		AND VoucherCode <> '40'
		AND ReturnCodeID <> '00'
		AND VoucherCode NOT IN (
			'58'
			,'59'
			)
		AND TrxRowID = @TrxRowID
	
	UNION ALL
	
	SELECT RIGHT(OurBranchID, 3), dbo.f_GetAccountProductID(OurBranchID, AccountID) ProductID, AccountID, DATE, ValueDate, ImageID, AccountTypeID 
		AccountType, TrxType, VoucherCode, ReturnCodeID ReturnCode, ABS(Amount), BankID, RIGHT(BranchID, 3), ChequeId, ChequeDigit, CASE 
			WHEN ReturnCodeID = '00'
				THEN DrawerOrPayeeAccountID
			ELSE DrawerOrPayeeAccountID
			END TheirAccount, '', '', '', '', DrawerOrPayee, '', '', 0, 0, '', '', '', '', GETDATE(), ImageID OutwardSerialID, TrxRowID ColumnID, IsNull(
			IsGenerated, 0), Reference, ImageID OutwardSerialID, ColumnID, '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', 
		'', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', ''
	FROM v_Clearing(NOLOCK)
	WHERE TrxType = 'OC'
		AND IsNull(IsGenerated, 0) = 0
		AND isNull(IsDeleted, 0) = 0
		AND VoucherCode = '40'
		AND TrxRowID = @TrxRowID
	
	--UNION ALL
	
	--SELECT RIGHT(OurBranchID, 3), dbo.f_GetAccountProductID(OurBranchID, AccountID) ProductID, AccountID, DATE, ValueDate, ImageID, AccountTypeID 
	--	AccountType, TrxType, VoucherCode, ReturnCodeID ReturnCode, ABS(Amount), BankID, RIGHT(BranchID, 3), DRN ChequeId, ChequeDigit, CASE 
	--		WHEN ReturnCodeID = '00'
	--			THEN DrawerOrPayeeAccountID
	--		ELSE DrawerOrPayeeAccountID
	--		END TheirAccount, '', '', '', '', DrawerOrPayee, '', '', 0, 0, '', '', '', '', GETDATE(), ImageID OutwardSerialID, TrxRowID ColumnID, IsNull(
	--		IsGenerated, 0), Reference, ImageID OutwardSerialID, ColumnID, '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', 
	--	'', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', ''
	--FROM v_Clearing(NOLOCK)
	--WHERE DATE BETWEEN @FromDate
	--		AND @ReadDate
	--	AND TrxType = 'OC'
	--	AND IsNull(IsGenerated, 0) = 0
	--	AND isNull(IsDeleted, 0) = 0
	--	AND VoucherCode <> '40'
	--	AND dbo.f_GetAccountOrGLCurrencyID(OurBranchID, AccountID, AccountTypeID) IN (
	--		SELECT DISTINCT CurrencyID
	--		FROM #TempCurrency
	--		)
	--	AND VoucherCode NOT IN (
	--		'58'
	--		,'59'
	--		)
	--	AND TrxRowID = @TrxRowID

	FETCH NEXT
	FROM MICRChequeTrx
	INTO @TrxRowID
END

CLOSE MICRChequeTrx

DEALLOCATE MICRChequeTrx

--SELECT * FROm #MICRCheque
--return-----Brenda
SELECT *
INTO #ChequeIMages
FROM BrNet_ImageServer.dbo.t_ChequeImages(NOLOCK)

--WHERE DATE BETWEEN @FromDate
--AND @ReadDate
--		select * from #ChequeIMages
--return
DELETE
FROM #ChequeIMages
WHERE ImageID NOT IN (
		SELECT TransactionID
		FROM #MICRCheque
		)

UPDATE #MICRCheque
SET TrxID = t_IncomingTransactions.TrxID
FROM t_IncomingTransactions
WHERE OriginalColumnID = t_IncomingTransactions.ColumnID
	AND #MICRCheque.ReturnCode <> '00'

IF EXISTS (
		SELECT 1
		FROM #MICRCheque
		WHERE isNull(TrxID, '') = ''
		)
BEGIN
	UPDATE #MICRCheque
	SET TrxID = t_IncomingTransactions.TrxID --History.TrxID
	FROM t_IncomingTransactions --History
	WHERE OriginalColumnID = t_IncomingTransactions.ColumnID --History.ColumnID
		AND #MICRCheque.ReturnCode <> '00'
		AND isNull(#MICRCheque.TrxID, '') = ''
END

UPDATE #MICRCheque
SET SwftCd = SourceBIC
FROM t_IncomingClrTrxExtraDetails
WHERE #MICRCheque.TrxID COLLATE DATABASE_DEFAULT = t_IncomingClrTrxExtraDetails.TrxID COLLATE DATABASE_DEFAULT
	AND #MICRCheque.ReturnCode <> '00'

UPDATE #MICRCheque
SET BankID = dbo.f_GetClearingBankIDFromSwiftID(SwftCd)
WHERE #MICRCheque.ReturnCode <> '00'
	AND BankID IS NULL

UPDATE b
SET CurrencyCode = CurrencyID
FROM t_Product a, #MICRCheque b
WHERE a.ProductID COLLATE DATABASE_DEFAULT = b.ProductID COLLATE DATABASE_DEFAULT
	AND b.AccountType = 'C'

UPDATE b
SET CurrencyCode = CurrencyID
FROM t_GeneralLedger a, #MICRCheque b
WHERE a.AccountID COLLATE DATABASE_DEFAULT = b.AccountID COLLATE DATABASE_DEFAULT

UPDATE b
SET ISOCurrencyID = SWIFTCurrencyID
FROM t_Currency a, #MICRCheque b
WHERE a.CurrencyID COLLATE DATABASE_DEFAULT = b.CurrencyCode COLLATE DATABASE_DEFAULT

UPDATE b
SET b.SwiftCode = a.SwiftCode
FROM t_Bank a, #MICRCheque b
WHERE a.BankID COLLATE DATABASE_DEFAULT = RIGHT(@OurBankID, 2) COLLATE DATABASE_DEFAULT

UPDATE b
SET b.DestinationSwiftCode = a.SwiftCode
FROM t_Bank a, #MICRCheque b
WHERE a.BankID COLLATE DATABASE_DEFAULT = b.BankID COLLATE DATABASE_DEFAULT

UPDATE b
SET b.BeneficiaryName = a.Name
FROM t_AccountCustomer a, #MICRCheque b
WHERE a.AccountID COLLATE DATABASE_DEFAULT LIKE '%' + b.AccountID + '%' COLLATE DATABASE_DEFAULT
	AND a.ProductID COLLATE DATABASE_DEFAULT = b.ProductID COLLATE DATABASE_DEFAULT

UPDATE b
SET b.BeneficiaryName = a.Description
FROM t_GeneralLedger a, #MICRCheque b
WHERE a.AccountID COLLATE DATABASE_DEFAULT = b.AccountID COLLATE DATABASE_DEFAULT
	AND b.ProductID = 'GL'

UPDATE #MICRCheque
SET BranchID = replace(BranchID, '-', '0')

UPDATE #MICRCheque
SET BankID = Right('000' + rtrim(BankID), 3), BranchID = Right('000' + rtrim(BranchID), 3)

UPDATE #MICRCheque
SET BeneficiaryName = 'NOT PROVIDED'
WHERE (
		BeneficiaryName = ''
		OR BeneficiaryName IS NULL
		)

UPDATE #MICRCheque
SET RemittersName = 'NOT PROVIDED'
WHERE (
		RemittersName = ''
		OR RemittersName IS NULL
		)

UPDATE #MICRCheque
SET MicrLineDetails = '/' + Right('000000' + rtrim(ChequeId), 6) + '/' + '67' + '/' + BankID + BranchID + Right('00' + rtrim(ChequeDigit), 2) + '/' + 
	TheirAccountID + '/' + VoucherCode + '/'

UPDATE #MICRCheque
SET ReturnCode = dbo.f_CRB_ReturnCodeDescriptionsForUnpay(ReturnCode)
WHERE ReturnCode <> '00'

DECLARE @OutwardSerialID VARCHAR(100), @AccountType VARCHAR(3), @AccountID VARCHAR(20)

DECLARE Trx CURSOR
FOR
SELECT OutwardSerialID
FROM #MICRCheque
WHERE ProductID = 'GL'
	AND ACCOUNTID NOT IN (
		'10410001'
		,'11600500'
		,'22301600'
		,'22230100'
		,'22301601'
		,'11600501'
		,'60100111'
		)

OPEN Trx

FETCH NEXT
FROM Trx
INTO @OutwardSerialID

WHILE @@FETCH_STATUS = 0
BEGIN
	SELECT @AccountID = AccountID, @OurBranchID = OurBranchID --, @AccountType = AccountType  
	FROM #ChequeIMages
	WHERE ImageID = @OutwardSerialID

	UPDATE #MICRCheque
	SET AccountID = @AccountID
	WHERE TransactionMicrColumnID = @OutwardSerialID

	UPDATE #MICRCheque
	SET BeneficiaryName = LEFT(ISNULL(dbo.f_GetAccountName(@OurBranchID, @AccountID), ''), 50)
	WHERE TransactionMicrColumnID = @OutwardSerialID

	FETCH NEXT
	FROM Trx
	INTO @OutwardSerialID
END

CLOSE Trx

DEALLOCATE Trx

UPDATE #MICRCheque
SET ColumnID = TransactionMicrColumnID

DECLARE @ColumnID VARCHAR(100), @RemittersName VARCHAR(100), @BeneficiaryName VARCHAR(100), @Jina VARCHAR(100), @JinaR VARCHAR(100), 
	@TransactionMicrColumnIDR VARCHAR(100), @Branchi VARCHAR(3), @Akaunti VARCHAR(30)

IF EXISTS (
		SELECT 1
		FROM #MICRCheque
		WHERE isNull(Reference, '') = ''
			AND ReturnCode <> '00'
		)
BEGIN
	DECLARE @ColumnIDC VARCHAR(100)

	DECLARE Trxx CURSOR
	FOR
	SELECT OriginalColumnID
	FROM #MICRCheque
	WHERE ReturnCode <> '00'
		AND isNull(Reference, '') = ''

	OPEN Trxx

	FETCH NEXT
	FROM Trxx
	INTO @ColumnIDC

	WHILE @@FETCH_STATUS = 0
	BEGIN TRY
		UPDATE #MICRCheque
		SET Reference = (
				SELECT MsgID
				FROM t_IncomingTransactions --History
				WHERE ColumnID = @ColumnIDC
				)
		WHERE OriginalColumnID = @ColumnIDC

		UPDATE #MICRCheque
		SET TransactionMicrColumnID = (
				SELECT TrxID
				FROM t_IncomingTransactions --History
				WHERE ColumnID = @ColumnIDC
				)
		WHERE OriginalColumnID = @ColumnIDC

		FETCH NEXT
		FROM Trxx
		INTO @ColumnIDC
	END TRY

	BEGIN CATCH
		SELECT Error_Message(), Error_line()
	END CATCH

	CLOSE Trxx

	DEALLOCATE Trxx
END

--A Temporary solution for test till BOT gives a go ahead----------------------------------------------------------                
--Update #MICRCheque Set SwiftCode= Left(SwiftCode,8) ,DestinationSwiftCode= Left(DestinationSwiftCode,8)                 
-------------------------------------------------------------------------------------------------------------------                    
--select * from #ChequeIMages  
--return
UPDATE #MICRCheque
SET FrontImageGrayScale = b.JFImage, FrontImageUV = b.UVImage, BackImageGrayScale = b.JRImage, FrontImageTiff = b.TFImage
FROM #MICRCheque a, #ChequeIMages b
WHERE a.TransactionID = CAST(B.ImageID AS NUMERIC)

BEGIN TRY
	UPDATE #MICRCheque
	SET OrgnlInstrID = b.OrgnlInstrID, OrgnlEndToEnd = b.OrgnlEndToEnd, OrgnlMsgId = b.OrgnlMsgId, MndtId = b.MndtId, DtOfSgntr = b.DtOfSgntr, 
		ReqdColltnDt = isNUll(TRY_CONVERT(DATETIME, TRY_CONVERT(VARCHAR(10), dbo.ChangeDateFormatFromUK(b.ReqdColltnDate), 101), 101), CONVERT
			(VARCHAR, CONVERT(DATETIME, b.ReqdColltnDate), 106)), Frqcy = b.Frqcy, UstrdColD = b.UstrdColD, UstrdChqdt = b.UstrdChqdt, UstrdBWF = b.
		UstrdBWF, UstrdBWR = b.UstrdBWR, UstrdGS = b.UstrdGS, UstrdUV = b.UstrdUV, UstrdMicr = b.UstrdMicr, DAdrLine = b.DAdrLine, DTwnNm = b.DTwnNm, 
		DCtry = b.DCtry, DNm = b.DNm, DCNm = b.DCNm, DPhneNb = b.DPhneNb, DMobNb = b.DMobNb, DEmailAdr = b.DEmailAdr, DOthr = b.DOthr, CAdrLine = b.CAdrLine, 
		CTwnNm = b.CTwnNm, CCtry = b.CCtry, CNm = b.CNm, CCNm = b.CCNm, CPhneNb = b.CPhneNb, CMobNb = b.CMobNb, CEmailAdr = b.CEmailAdr, COthr = b.COthr, 
		DbtrAcct = b.DbtrAcct, CdtrAcct = b.CdtrAcct, SvcLvl = b.SvcLvl, LclInstrm = b.LclInstrm, CtgyPurp = b.CtgyPurp
	FROM #MICRCheque a, t_IncomingClrTrxExtraDetails b
	WHERE a.TrxID COLLATE DATABASE_DEFAULT = B.TrxID COLLATE DATABASE_DEFAULT
		AND a.ReturnCode <> '00'
		AND VoucherCode NOT IN (
			'58'
			,'59'
			)
END TRY

BEGIN CATCH
	UPDATE #MICRCheque
	SET OrgnlInstrID = b.OrgnlInstrID, OrgnlEndToEnd = b.OrgnlEndToEnd, OrgnlMsgId = b.OrgnlMsgId, MndtId = b.MndtId, DtOfSgntr = b.DtOfSgntr, 
		ReqdColltnDt = CONVERT(VARCHAR, CONVERT(DATETIME, b.ReqdColltnDate), 106), Frqcy = b.Frqcy, UstrdColD = b.UstrdColD, UstrdChqdt = b.
		UstrdChqdt, UstrdBWF = b.UstrdBWF, UstrdBWR = b.UstrdBWR, UstrdGS = b.UstrdGS, UstrdUV = b.UstrdUV, UstrdMicr = b.UstrdMicr, DAdrLine = b.
		DAdrLine, DTwnNm = b.DTwnNm, DCtry = b.DCtry, DNm = b.DNm, DCNm = b.DCNm, DPhneNb = b.DPhneNb, DMobNb = b.DMobNb, DEmailAdr = b.DEmailAdr, DOthr = b.
		DOthr, CAdrLine = b.CAdrLine, CTwnNm = b.CTwnNm, CCtry = b.CCtry, CNm = b.CNm, CCNm = b.CCNm, CPhneNb = b.CPhneNb, CMobNb = b.CMobNb, CEmailAdr = b.
		CEmailAdr, COthr = b.COthr, DbtrAcct = b.DbtrAcct, CdtrAcct = b.CdtrAcct, SvcLvl = b.SvcLvl, LclInstrm = b.LclInstrm, CtgyPurp = b.CtgyPurp
	FROM #MICRCheque a, t_IncomingClrTrxExtraDetails b
	WHERE a.TrxID COLLATE DATABASE_DEFAULT = B.TrxID COLLATE DATABASE_DEFAULT
		AND a.ReturnCode <> '00'
		AND VoucherCode NOT IN (
			'58'
			,'59'
			)
END CATCH

DECLARE MICREFTsTrx CURSOR
FOR
SELECT MICREFTsTx.x.value('TrxRowID[1]', 'nVARCHAR(60)')
FROM @xmlList.nodes('/ArrayOfLsItems/lsItems') AS MICREFTsTx(x)

OPEN MICREFTsTrx

FETCH NEXT
FROM MICREFTsTrx
INTO @TrxRowID

WHILE @@FETCH_STATUS = 0
BEGIN
	INSERT INTO #MICREFTs (
		OurBranchID, ProductID, AccountID, DATE, ValueDate, AccountType, TrxType, VoucherCode, ReturnCode, Amount, BankID, BranchID, TheirAccountID
		, SwiftCode, DestinationSwiftCode, CurrencyCode, ISOCurrencyID, BeneficiaryName, RemittersName,
		-- our customer's name                 
		ProcessingNumber, EFTID, OriginatorCode, OriginatorReference, PolicyNumber1, PolicyNumber2, Remarks, ISDebitORISCredit, CreationDate, 
		TransactionMicrColumnID, IsGenerated, Reference, OutwardSerialID, ColumnID, OriginalColumnID, TrxID, RetCodeDesc, OrgnlInstrID, 
		OrgnlEndToEnd, MndtId, DtOfSgntr, ReqdColltnDt, Frqcy, UstrdColD, UstrdChqdt, UstrdBWF, UstrdBWR, UstrdGS, UstrdUV, UstrdMicr, DAdrLine, 
		DTwnNm, DCtry, DNm, DPhneNb, DMobNb, DEmailAdr, DOthr, CAdrLine, CTwnNm, CCtry, CNm, CPhneNb, CMobNb, CEmailAdr, COthr, DCNm, CCNm, CdtrAcct, 
		DbtrAcct, OrgnlMsgId, SwftCd, SvcLvl, LclInstrm, CtgyPurp, OrgnTrxID
		)
	SELECT RIGHT(OurBranchID, 3), dbo.f_GetAccountProductID(OurBranchID, AccountID) ProductID, LTRIM(RTRIM(AccountID)) AS AccountID , DATE, DATE, AccountTypeID AccountType, 
		TrxType, VoucherCode, ReturnCodeID ReturnCode, Abs(Amount), BankID, RIGHT(BranchID, 3),LTRIM(RTRIM(DrawerOrPayeeAccountID)) AS TheirAccount, '', '', '', '', 
		DrawerORPayee ExtraDetails, dbo.f_GetAccountName(OurBranchID, AccountID) Name,
		-- our customer's name               
		0, 0, OriginatorCode, OrigRefCode OriginatorReference, PolicyNumber1, PolicyNumber2, DrawerORPayee Remarks, 1, GETDATE(), TrxRowID ColumnID
		, IsNull(IsGenerated, 0), isNull(Reference, TrxRowID), TrxRowID OutwardSerialID, TrxRowID ColumnID, ColumnID, DRN, '', ColumnID, '', '', '', ''
		, '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', ''
	FROM v_Clearing(NOLOCK)
	WHERE  DATE BETWEEN @FromDate
			AND @ReadDate
			AND TrxType = 'OD'
		AND isNull(isDeleted, 0) = 0
		AND IsNull(IsGenerated, 0) = 0
		AND ReturnCodeID <> '00'
		AND VoucherCode <> '40'
		AND TrxRowID = @TrxRowID --pp
	
	UNION ALL
	
	SELECT RIGHT(OurBranchID, 3), dbo.f_GetAccountProductID(OurBranchID, AccountID) ProductID, AccountID, DATE, DATE, AccountTypeID AccountType, 
		TrxType, VoucherCode, ReturnCodeID ReturnCode, Abs(Amount), BankID, RIGHT(BranchID, 3), DrawerOrPayeeAccountID TheirAccount, '', '', '', '', 
		DrawerORPayee ExtraDetails, dbo.f_GetAccountName(OurBranchID, AccountID) Name, -- our customer's name                 
		0, 0, OriginatorCode, OrigRefCode OriginatorReference, PolicyNumber1, PolicyNumber2, DrawerORPayee Remarks, 1, GETDATE(), TrxRowID ColumnID
		, IsNull(IsGenerated, 0), isNull(Reference, TrxRowID), TrxRowID OutwardSerialID, TrxRowID ColumnID, ColumnID, '', '', ColumnID, ColumnID, DRN
		, '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', ''
	FROM v_Clearing(NOLOCK)
	WHERE DATE BETWEEN @FromDate
			AND @ReadDate
		AND TrxType = 'OD'
		AND isNull(isDeleted, 0) = 0
		AND IsNull(IsGenerated, 0) = 0
		AND ReturnCodeID = '00'
		AND VoucherCode <> '40'
		AND dbo.f_GetAccountOrGLCurrencyID(OurBranchID, AccountID, AccountTypeID) IN (
			SELECT DISTINCT CurrencyID
			FROM #TempCurrency
			)
		AND TrxRowID = @TrxRowID--pp
	
	
	

	

	
	--UNION ALL
	
	--SELECT RIGHT(OurBranchID, 3), dbo.f_GetAccountProductID(OurBranchID, AccountID) ProductID, AccountID, DATE, DATE, AccountTypeID AccountType, 
	--	TrxType, VoucherCode, ReturnCodeID ReturnCode, Abs(Amount), BankID, RIGHT(BranchID, 3), DrawerOrPayeeAccountID TheirAccount, '', '', '', '', 
	--	DrawerORPayee ExtraDetails, dbo.f_GetAccountName(OurBranchID, AccountID) Name, -- our customer's name                 
	--	0, 0, OriginatorCode, OrigRefCode OriginatorReference, PolicyNumber1, PolicyNumber2, DrawerORPayee Remarks, 1, GETDATE(), TrxRowID ColumnID
	--	, IsNull(IsGenerated, 0), isNull(Reference, TrxRowID), TrxRowID OutwardSerialID, TrxRowID ColumnID, ColumnID, '', '', ColumnID, ColumnID, DRN
	--	, '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', ''
	--FROM v_Clearing(NOLOCK)
	--WHERE TrxType = 'OD'
	--	AND isNull(isDeleted, 0) = 0
	--	AND IsNull(IsGenerated, 0) = 0
	--	AND dbo.f_GetAccountOrGLCurrencyID(OurBranchID, AccountID, AccountTypeID) IN (
	--		SELECT DISTINCT CurrencyID
	--		FROM #TempCurrency
	--		)
	--	AND ReturnCodeID <> '00'
	--	AND VoucherCode <> '40'
	--	AND TrxRowID = @TrxRowID

	FETCH NEXT
	FROM MICREFTsTrx
	INTO @TrxRowID
END

CLOSE MICREFTsTrx

DEALLOCATE MICREFTsTrx

UPDATE #MICREFTs
SET TrxID = t_IncomingTransactions.TrxID
FROM t_IncomingTransactions
WHERE OriginalColumnID = CAST(t_IncomingTransactions.ColumnID AS NUMERIC)
	AND #MICREFTs.ReturnCode <> '00'

UPDATE #MICREFTs
SET SwftCd = SourceBIC
FROM t_IncomingClrTrxExtraDetails
WHERE #MICREFTs.TrxID COLLATE DATABASE_DEFAULT = t_IncomingClrTrxExtraDetails.TrxID COLLATE DATABASE_DEFAULT
	AND #MICREFTs.ReturnCode <> '00'

UPDATE #MICREFTs
SET BankID = dbo.f_GetClearingBankIDFromSwiftID(LEFT(SwftCd, 6))
WHERE #MICREFTs.ReturnCode <> '00'
	AND BankID IS NULL

UPDATE b
SET CurrencyCode = CurrencyID
FROM t_Product a, #MICREFTs b
WHERE a.ProductID COLLATE DATABASE_DEFAULT = b.ProductID COLLATE DATABASE_DEFAULT
	AND b.AccountType = 'C'

UPDATE b
SET CurrencyCode = CurrencyID
FROM t_GeneralLedger a, #MICREFTs b
WHERE a.AccountID COLLATE DATABASE_DEFAULT = b.AccountID COLLATE DATABASE_DEFAULT
	AND b.AccountType = 'G'

UPDATE b
SET ISOCurrencyID = SWIFTCurrencyID
FROM t_Currency a, #MICREFTs b
WHERE a.CurrencyID COLLATE DATABASE_DEFAULT = b.CurrencyCode COLLATE DATABASE_DEFAULT

--select * from #MICREFTs
UPDATE b
SET b.SwiftCode = a.SwiftCode
FROM t_Bank a, #MICREFTs b
WHERE a.BankID = RIGHT(@OurBankID, 2)

--select * from #MICREFTs	  
UPDATE b
SET b.DestinationSwiftCode = a.SwiftCode
FROM t_Bank a, #MICREFTs b
WHERE a.BankID COLLATE DATABASE_DEFAULT = b.BankID COLLATE DATABASE_DEFAULT

UPDATE #MICREFTs
SET BeneficiaryName = 'NOT PROVIDED'
WHERE (
		BeneficiaryName = ''
		OR BeneficiaryName IS NULL
		)

UPDATE #MICREFTs
SET RemittersName = 'NOT PROVIDED'
WHERE (
		RemittersName = ''
		OR RemittersName IS NULL
		)

UPDATE #MICREFTs
SET OriginatorReference = transactionmicrcolumnid
WHERE isnull(OriginatorReference, '') = ''

UPDATE #MICREFTs
SET ReturnCode = IsNull(dbo.f_CRB_ReturnCodeDescriptionsForUnpay(ReturnCode), ReturnCode)
WHERE ReturnCode <> '00'

UPDATE #MICREFTs
SET TrxID = b.TrxID
FROM #MICREFTs a, t_IncomingTransactions b
WHERE a.OriginalColumnID = b.ColumnID
	AND isNull(a.TrxID, '') = ''

UPDATE #MICREFTs
SET MndtId = b.DDID
FROM #MICREFTs a, t_DirectDebitMaintenance b
WHERE a.MndtId COLLATE DATABASE_DEFAULT = b.DDID COLLATE DATABASE_DEFAULT
	AND isNull(a.MndtId, '') = ''
	AND a.VoucherCode = '40'

UPDATE #MICREFTs
SET MndtId = b.TrxRowID
FROM #MICREFTs a, v_Clearing b
WHERE isNull(a.MndtId, '') = ''
	AND a.VoucherCode = '40'
	AND a.TransactionMicrColumnID = b.TrxRowID
	AND a.ReturnCode = '00'

UPDATE #MICREFTs
SET TrxID = b.TrxID
FROM #MICREFTs a, t_IncomingTransactions b --History b
WHERE a.OriginalColumnID = b.ColumnID
	AND isNull(a.TrxID, '') = ''

UPDATE #MICREFTs
SET OrgnlInstrID = MndtId
FROM #MICREFTs a, v_Clearing b
WHERE a.ColumnID = b.TrxRowID
	AND a.VoucherCode = '40'
	AND a.TransactionMicrColumnID = b.TrxRowID
	AND a.ReturnCode = '00'

UPDATE #MICREFTs
SET DtOfSgntr = b.DATE
FROM #MICREFTs a, v_Clearing b
WHERE isNull(a.DtOfSgntr, '') = ''
	AND a.VoucherCode = '40'
	AND a.TransactionMicrColumnID = b.TrxRowID
	AND a.ReturnCode = '00'

UPDATE #MICREFTs
SET OrgnlInstrID = a.TransactionMicrColumnID
FROM #MICREFTs a, v_Clearing b
WHERE isNull(a.OrgnlInstrID, '') = ''
	AND a.VoucherCode = '40'
	AND a.TransactionMicrColumnID = b.TrxRowID
	AND a.ReturnCode = '00'

BEGIN TRY
	UPDATE #MICREFTs
	SET OrgnlInstrID = b.OrgnlInstrID, OrgnlEndToEnd = b.OrgnlEndToEnd, MndtId = b.MndtId, DtOfSgntr = b.DtOfSgntr, OrgnlMsgId = b.OrgnlMsgId, 
		ReqdColltnDt = isNULL(CONVERT(DATETIME, CONVERT(VARCHAR(10), dbo.ChangeDateFormatFromUK(b.ReqdColltnDate), 101), 101), CONVERT(VARCHAR
				, CONVERT(DATETIME, b.ReqdColltnDate), 106)), Frqcy = b.Frqcy, UstrdColD = b.UstrdColD, UstrdChqdt = b.UstrdChqdt, UstrdBWF = b.
		UstrdBWF, UstrdBWR = b.UstrdBWR, UstrdGS = b.UstrdGS, UstrdUV = b.UstrdUV, UstrdMicr = b.UstrdMicr, DAdrLine = b.DAdrLine, DTwnNm = b.DTwnNm, 
		DCtry = b.DCtry, DNm = b.DNm, DCNm = b.DCNm, DPhneNb = b.DPhneNb, DMobNb = b.DMobNb, DEmailAdr = b.DEmailAdr, DOthr = b.DOthr, CAdrLine = b.CAdrLine, 
		CTwnNm = b.CTwnNm, CCtry = b.CCtry, CNm = b.CNm, CCNm = b.CCNm, CPhneNb = b.CPhneNb, CMobNb = b.CMobNb, CEmailAdr = b.CEmailAdr, COthr = b.COthr, 
		DbtrAcct = b.DbtrAcct, CdtrAcct = b.CdtrAcct, SvcLvl = b.SvcLvl, LclInstrm = b.LclInstrm, CtgyPurp = b.CtgyPurp, OrgnTrxID = b.TrxID,
		OrgnlIntrBkSttlmDt = b.ReqdColltnDate
	FROM #MICREFTs a, t_IncomingClrTrxExtraDetails b
	WHERE a.TrxID COLLATE DATABASE_DEFAULT = B.TrxID COLLATE DATABASE_DEFAULT
		AND a.ReturnCode <> '00'
END TRY

BEGIN CATCH
	--select 'll'
	UPDATE #MICREFTs
	SET OrgnlInstrID = b.OrgnlInstrID, OrgnlEndToEnd = b.OrgnlEndToEnd, MndtId = b.MndtId, DtOfSgntr = b.DtOfSgntr, OrgnlMsgId = b.OrgnlMsgId, 
		ReqdColltnDt = CONVERT(VARCHAR, CONVERT(DATETIME, b.ReqdColltnDate), 106), Frqcy = b.Frqcy, UstrdColD = b.UstrdColD, UstrdChqdt = b.
		UstrdChqdt, UstrdBWF = b.UstrdBWF, UstrdBWR = b.UstrdBWR, UstrdGS = b.UstrdGS, UstrdUV = b.UstrdUV, UstrdMicr = b.UstrdMicr, DAdrLine = b.
		DAdrLine, DTwnNm = b.DTwnNm, DCtry = b.DCtry, DNm = b.DNm, DCNm = b.DCNm, DPhneNb = b.DPhneNb, DMobNb = b.DMobNb, DEmailAdr = b.DEmailAdr, DOthr = b.
		DOthr, CAdrLine = b.CAdrLine, CTwnNm = b.CTwnNm, CCtry = b.CCtry, CNm = b.CNm, CCNm = b.CCNm, CPhneNb = b.CPhneNb, CMobNb = b.CMobNb, CEmailAdr = b.
		CEmailAdr, COthr = b.COthr, DbtrAcct = b.DbtrAcct, CdtrAcct = b.CdtrAcct, SvcLvl = b.SvcLvl, LclInstrm = b.LclInstrm, CtgyPurp = b.CtgyPurp, 
		OrgnTrxID = b.TrxID, OrgnlIntrBkSttlmDt = b.ReqdColltnDate
	FROM #MICREFTs a, t_IncomingClrTrxExtraDetails b
	WHERE a.TrxID COLLATE DATABASE_DEFAULT = B.TrxID COLLATE DATABASE_DEFAULT
		AND a.ReturnCode <> '00'
END CATCH

--IF EXISTS (SELECT 1 From  #MICREFTs WHERE isNull(ReqdColltnDt,'') = '' WHERE  
UPDATE #MICREFTs
SET DtOfSgntr = b.FirstExecutionDate,
	--ReqdColltnDt = CAST AS  @WorkingDate,
	FnlColltnDt = b.LastExecutionDate, Frqcy = b.TrfFrequencyID, CNm = LEFT(rtrim(dbo.f_GetAccountName(b.OurBranchID, b.CreditAccountID)), 35), 
	DNm = b.Reference, DbtrAcct = b.DebitAccountID, CdtrAcct = b.CreditAccountID
FROM #MICREFTs a, t_DirectDebitMaintenance b
WHERE a.MndtId COLLATE DATABASE_DEFAULT = b.DDID COLLATE DATABASE_DEFAULT
	AND a.ReturnCode = '00'
	AND a.VoucherCode = '40'
	AND b.DDTypeID = 'DDT'

UPDATE #MICREFTs
SET DtOfSgntr = a.DATE,
	--ReqdColltnDt = CAST AS  @WorkingDate,
	FnlColltnDt = a.DATE, Frqcy = 'D', CNm = LEFT(rtrim(dbo.f_GetAccountName(a.OurBranchID, a.AccountID)), 35), DNm = b.DrawerOrPayee, DbtrAcct = b.
	DrawerOrPayeeAccountID, CdtrAcct = a.AccountID
FROM #MICREFTs a, v_Clearing b(NOLOCK)
WHERE a.TransactionMicrColumnID = CAST(b.TrxRowID AS VARCHAR)
	AND a.ReturnCode = '00'
	AND a.VoucherCode = '40'
	AND IsNull(a.CNm, '') = ''

--Cancelled EFTs
UPDATE #MICREFTs
SET OrgnlInstrID = b.TrxRowID, OrgnlEndToEnd = b.TrxRowID, OrgnlMsgId = b.Reference, ReqdColltnDt = b.DATE, DNm = dbo.f_GetAccountName(b.OurBranchID, b
		.AccountID), CNm = b.DrawerOrPayee, DbtrAcct = b.AccountID, CdtrAcct = b.DrawerOrPayeeAccountID
FROM #MICREFTs a, v_Clearing b
WHERE a.OrgnlInstrID = CAST(b.TrxRowID AS VARCHAR(50))
	AND a.ReturnCode <> '00'
	AND b.VoucherCode IN (
		'58'
		,'59'
		)

DELETE
FROM #MICRCheque
WHERE (
		CAST(FrontImageGrayScale AS VARCHAR) = ''
		OR CAST(BackImageGrayScale AS VARCHAR) = ''
		OR CAST(FrontImageTiff AS VARCHAR) = ''
		)
	AND ReturnCode = '00'

IF isNull(@session, 0) = 0
BEGIN
	SELECT *
	FROM #MICRCheque

	SELECT *
	FROM #MICREFTs
END
ELSE IF isNull(@session, 0) = 1
BEGIN
	SELECT *
	FROM #MICRCheque
	WHERE ReturnCode = '00'

	SELECT *
	FROM #MICREFTs
	WHERE ReturnCode = '00'
END
ELSE
BEGIN
	SELECT *
	FROM #MICRCheque
	WHERE ReturnCode <> '00'

	SELECT *
	FROM #MICREFTs
	WHERE ReturnCode <> '00'
END

SELECT *
FROM #ChequeIMages



Completion time: 2026-03-31T21:41:34.6301126+08:00
