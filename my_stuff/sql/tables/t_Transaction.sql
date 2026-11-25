TrxRowID	bigint	no	8	19   	0    	no	(n/a)	(n/a)	NULL
TrxBatchID	varchar	no	8	     	     	no	no	no	SQL_Latin1_General_CP1_CI_AS
TrxBatchSLNo	smallint	no	2	5    	0    	yes	(n/a)	(n/a)	NULL
TrxBranchID	BranchID	no	12	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
SerialID	int	no	4	10   	0    	no	(n/a)	(n/a)	NULL
OurBranchID	BranchID	no	12	     	     	no	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
AccountTypeID	char	no	1	     	     	no	no	no	SQL_Latin1_General_CP1_CI_AS
AccountID	AccountID	no	40	     	     	no	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
ProductID	ProductID	no	12	     	     	no	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
ModuleID	smallint	no	2	5    	0    	yes	(n/a)	(n/a)	NULL
TrxCodeID	tinyint	no	1	3    	0    	yes	(n/a)	(n/a)	NULL
TrxTypeID	char	no	2	     	     	no	no	no	SQL_Latin1_General_CP1_CI_AS
TrxDate	smalldatetime	no	4	     	     	no	(n/a)	(n/a)	NULL
ValueDate	smalldatetime	no	4	     	     	no	(n/a)	(n/a)	NULL
Amount	Amount	no	8	19   	4    	no	(n/a)	(n/a)	NULL
LocalAmount	Amount	no	8	19   	4    	no	(n/a)	(n/a)	NULL
TrxCurrencyID	CurrencyID	no	6	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
TrxAmount	Amount	no	8	19   	4    	yes	(n/a)	(n/a)	NULL
ExchangeRate	CurrencyRate	no	9	11   	6    	yes	(n/a)	(n/a)	NULL
MeanRate	CurrencyRate	no	9	11   	6    	yes	(n/a)	(n/a)	NULL
Profit	Amount	no	8	19   	4    	no	(n/a)	(n/a)	NULL
InstrumentTypeID	char	no	1	     	     	yes	no	yes	SQL_Latin1_General_CP1_CI_AS
ChequeID	int	no	4	10   	0    	no	(n/a)	(n/a)	NULL
ChequeDate	smalldatetime	no	4	     	     	yes	(n/a)	(n/a)	NULL
ReferenceNo	nvarchar	no	30	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
Remarks	Remarks	no	510	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
TrxDescriptionID	nvarchar	no	12	     	     	no	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
TrxDescription	nvarchar	no	300	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
MainGLID	AccountID	no	40	     	     	no	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
ContraGLID	AccountID	no	40	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
TrxFlagID	char	no	2	     	     	no	no	no	SQL_Latin1_General_CP1_CI_AS
ImageID	bigint	no	8	19   	0    	no	(n/a)	(n/a)	NULL
TrxPrinted	tinyint	no	1	3    	0    	no	(n/a)	(n/a)	NULL
IsTrxPending	bit	no	1	     	     	no	(n/a)	(n/a)	NULL
UnsupervisedAmount	Amount	no	8	19   	4    	yes	(n/a)	(n/a)	NULL
ForwardToUser	OperatorID	no	50	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
ForwardToGroup	OperatorID	no	50	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
CreatedBy	OperatorID	no	50	     	     	no	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
CreatedOn	smalldatetime	no	4	     	     	yes	(n/a)	(n/a)	NULL
SupervisedBy	OperatorID	no	50	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
SupervisedOn	smalldatetime	no	4	     	     	yes	(n/a)	(n/a)	NULL
SupervisedBy2	OperatorID	no	50	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
SupervisedOn2	smalldatetime	no	4	     	     	yes	(n/a)	(n/a)	NULL
DeletedBy	OperatorID	no	50	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
DeletedOn	smalldatetime	no	4	     	     	yes	(n/a)	(n/a)	NULL
DeletedReason	Remarks	no	510	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
DeletionSupervisedBy	OperatorID	no	50	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
DeletionSupervisedOn	smalldatetime	no	4	     	     	yes	(n/a)	(n/a)	NULL
TraceNo	varchar	no	12	     	     	yes	no	yes	SQL_Latin1_General_CP1_CI_AS
ForwardRemark	Remarks	no	510	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
TheirColumnID	nvarchar	no	510	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
BREFTTrxID	nvarchar	no	200	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
GLBudgetExcessAmount	money	no	8	19   	4    	yes	(n/a)	(n/a)	NULL
SupervisedByTemp	varchar	no	15	     	     	yes	no	yes	SQL_Latin1_General_CP1_CI_AS
SupervisedOnTemp	datetime	no	8	     	     	yes	(n/a)	(n/a)	NULL
OldAccountID	varchar	no	15	     	     	yes	no	yes	SQL_Latin1_General_CP1_CI_AS
AMLFlag	varchar	no	1	     	     	yes	no	yes	SQL_Latin1_General_CP1_CI_AS
DisableReason	varchar	no	100	     	     	yes	no	yes	SQL_Latin1_General_CP1_CI_AS
DisabledBy	varchar	no	200	     	     	yes	no	yes	SQL_Latin1_General_CP1_CI_AS
DisabledOn	datetime	no	8	     	     	yes	(n/a)	(n/a)	NULL
ReversedBy	nvarchar	no	80	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
ReversedOn	datetime	no	8	     	     	yes	(n/a)	(n/a)	NULL
CheckSum	nvarchar	no	400	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS