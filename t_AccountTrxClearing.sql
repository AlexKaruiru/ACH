AccountTrxRowID	bigint	no	8	19   	0    	no	(n/a)	(n/a)	NULL
TrxRowID	bigint	no	8	19   	0    	no	(n/a)	(n/a)	NULL
TrxBranchID	BranchID	no	12	     	     	no	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
TrxBatchID	varchar	no	8	     	     	no	no	no	SQL_Latin1_General_CP1_CI_AS
TrxBatchSLNo	smallint	no	2	5    	0    	yes	(n/a)	(n/a)	NULL
OurBranchID	BranchID	no	12	     	     	no	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
AccountTypeID	SystemSubID	no	50	     	     	no	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
AccountID	AccountID	no	40	     	     	no	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
ChequeDigit	char	no	2	     	     	yes	no	yes	SQL_Latin1_General_CP1_CI_AS
VoucherCode	char	no	2	     	     	yes	no	yes	SQL_Latin1_General_CP1_CI_AS
ReturnCodeID	varchar	no	4	     	     	yes	no	yes	SQL_Latin1_General_CP1_CI_AS
Commission	Amount	no	8	19   	4    	no	(n/a)	(n/a)	NULL
TheirCommission	Amount	no	8	19   	4    	no	(n/a)	(n/a)	NULL
VATPINNo	nvarchar	no	24	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
VATPAYType	nvarchar	no	20	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
VATPAYEMonth	nvarchar	no	12	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
VATSerialNo	nvarchar	no	16	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
BankID	BankID	no	12	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
BranchID	BranchID	no	12	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
DrawerOrPayeeAccountID	AccountID	no	40	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
DrawerOrPayee	Names	no	200	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
ChequeID	int	no	4	10   	0    	yes	(n/a)	(n/a)	NULL
ChequeDate	smalldatetime	no	4	     	     	yes	(n/a)	(n/a)	NULL
ValueDate	smalldatetime	no	4	     	     	yes	(n/a)	(n/a)	NULL
Amount	Amount	no	8	19   	4    	no	(n/a)	(n/a)	NULL
Date	smalldatetime	no	4	     	     	yes	(n/a)	(n/a)	NULL
CurrencyID	CurrencyID	no	6	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
DRN	varchar	no	20	     	     	yes	no	yes	SQL_Latin1_General_CP1_CI_AS
RefNo	varchar	no	8	     	     	yes	no	yes	SQL_Latin1_General_CP1_CI_AS
OrigRefCode	nvarchar	no	50	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
ProcessingNo	nvarchar	no	18	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
PolicyNumber1	nvarchar	no	50	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
PolicyNumber2	nvarchar	no	50	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
VATPAYECommission	Amount	no	8	19   	4    	yes	(n/a)	(n/a)	NULL
NameOfEmployee	nvarchar	no	50	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
ESlipNumber	nvarchar	no	12	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
ORIGINATORCODE	varchar	no	4	     	     	yes	no	yes	SQL_Latin1_General_CP1_CI_AS
FileType	char	no	1	     	     	yes	no	yes	SQL_Latin1_General_CP1_CI_AS
PaymentDate	smalldatetime	no	4	     	     	yes	(n/a)	(n/a)	NULL
VatName	nvarchar	no	70	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
VatNumber	nvarchar	no	22	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
VatType	nvarchar	no	8	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
MonthOfPayment	nvarchar	no	12	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
PaymentType	nvarchar	no	8	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
PinNumber	nvarchar	no	22	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
PaymentTypeID	nvarchar	no	12	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
TrxType	char	no	2	     	     	yes	no	yes	SQL_Latin1_General_CP1_CI_AS
ImageID	bigint	no	8	19   	0    	yes	(n/a)	(n/a)	NULL
IsMDV	tinyint	no	1	3    	0    	yes	(n/a)	(n/a)	NULL
IsDeleted	bit	no	1	     	     	yes	(n/a)	(n/a)	NULL
OriginalAmount	Amount	no	8	19   	4    	yes	(n/a)	(n/a)	NULL
CreatedOn	datetime	no	8	     	     	yes	(n/a)	(n/a)	NULL
ColumnID	bigint	no	8	19   	0    	yes	(n/a)	(n/a)	NULL
IsUnpaidItem	bit	no	1	     	     	yes	(n/a)	(n/a)	NULL
IsGenerated	bit	no	1	     	     	yes	(n/a)	(n/a)	NULL
SessNo	int	no	4	10   	0    	yes	(n/a)	(n/a)	NULL
Reference	varchar	no	200	     	     	yes	no	yes	SQL_Latin1_General_CP1_CI_AS