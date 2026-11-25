AccountTrxID	bigint	no	8	19   	0    	no	(n/a)	(n/a)	NULL
TrxRowID	bigint	no	8	19   	0    	no	(n/a)	(n/a)	NULL
TrxBranchID	BranchID	no	12	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
TrxBatchID	varchar	no	8	     	     	yes	no	yes	SQL_Latin1_General_CP1_CI_AS
SerialID	int	no	4	10   	0    	no	(n/a)	(n/a)	NULL
OurBranchID	BranchID	no	12	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
AccountTypeID	SystemSubID	no	50	     	     	no	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
AccountID	AccountID	no	40	     	     	no	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
ProductID	ProductID	no	12	     	     	no	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
TrxDate	smalldatetime	no	4	     	     	no	(n/a)	(n/a)	NULL
TrxTypeID	SystemSubID	no	50	     	     	no	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
TrxCurrencyID	CurrencyID	no	6	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
ChequeID	numeric	no	9	18   	0    	no	(n/a)	(n/a)	NULL
ChequeDate	smalldatetime	no	4	     	     	yes	(n/a)	(n/a)	NULL
ValueDate	smalldatetime	no	4	     	     	no	(n/a)	(n/a)	NULL
TrxAmount	Amount	no	8	19   	4    	yes	(n/a)	(n/a)	NULL
LocalAmount	Amount	no	8	19   	4    	no	(n/a)	(n/a)	NULL
Amount	Amount	no	8	19   	4    	no	(n/a)	(n/a)	NULL
ExchangeRate	numeric	no	9	18   	4    	yes	(n/a)	(n/a)	NULL
MeanRate	numeric	no	9	18   	4    	yes	(n/a)	(n/a)	NULL
TrxDescriptionID	nvarchar	no	20	     	     	no	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
TrxDescription	Description	no	510	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
TrxPrinted	tinyint	no	1	3    	0    	no	(n/a)	(n/a)	NULL
Profit	Amount	no	8	19   	4    	no	(n/a)	(n/a)	NULL
MainGLID	AccountID	no	40	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
ContraGLID	AccountID	no	40	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
ImageID	numeric	no	9	10   	0    	yes	(n/a)	(n/a)	NULL
ReferenceNo	nvarchar	no	30	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
Remarks	varchar	no	-1	     	     	yes	no	yes	SQL_Latin1_General_CP1_CI_AS
ModuleID	smallint	no	2	5    	0    	yes	(n/a)	(n/a)	NULL
TrxCodeID	tinyint	no	1	3    	0    	yes	(n/a)	(n/a)	NULL
CreatedBy	OperatorID	no	50	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
CreatedOn	smalldatetime	no	4	     	     	yes	(n/a)	(n/a)	NULL
SupervisedBy	OperatorID	no	50	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
SupervisedOn	smalldatetime	no	4	     	     	yes	(n/a)	(n/a)	NULL
BREFTTrxID	nvarchar	no	200	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
SupervisedBy2	varchar	no	20	     	     	yes	no	yes	SQL_Latin1_General_CP1_CI_AS
SupervisedOn2	varchar	no	20	     	     	yes	no	yes	SQL_Latin1_General_CP1_CI_AS
OldAccountID	varchar	no	20	     	     	yes	no	yes	SQL_Latin1_General_CP1_CI_AS
TraceNo	varchar	no	12	     	     	yes	no	yes	SQL_Latin1_General_CP1_CI_AS
CheckSum	nvarchar	no	400	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS