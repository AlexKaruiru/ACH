OurBranchID	BranchID	no	12	     	     	no	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
AccountID	AccountID	no	40	     	     	no	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
ProductID	ProductID	no	12	     	     	no	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
ClientID	ClientID	no	40	     	     	no	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
Name	Names	no	200	     	     	no	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
ShortName	ShortNames	no	40	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
Address1	Address	no	510	     	     	no	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
Address2	Address	no	510	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
CityID	UserSubID	no	50	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
CountryID	nvarchar	no	12	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
Phone1	Phone	no	60	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
Phone2	Phone	no	60	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
Mobile	Phone	no	60	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
Fax	Fax	no	60	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
EmailID	EmailID	no	200	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
ContactPerson	Names	no	200	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
ID1	nvarchar	no	100	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
ID2	nvarchar	no	100	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
OperatingModeID	UserSubID	no	50	     	     	no	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
OperatingInstructions	Description	no	510	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
AccountClassID	nvarchar	no	20	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
AccountOfficerID	UserSubID	no	50	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
IsBlocked	bit	no	1	     	     	no	(n/a)	(n/a)	NULL
IsDormant	bit	no	1	     	     	no	(n/a)	(n/a)	NULL
IsRepaymentAccount	bit	no	1	     	     	yes	(n/a)	(n/a)	NULL
AccountStatusID	char	no	2	     	     	no	no	no	SQL_Latin1_General_CP1_CI_AS
Comments	Remarks	no	510	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
Notes	nvarchar	no	-1	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
ClearBalance	Amount	no	8	19   	4    	no	(n/a)	(n/a)	NULL
UnClearBalance	Amount	no	8	19   	4    	no	(n/a)	(n/a)	NULL
UnSupervisedCredits	Amount	no	8	19   	4    	no	(n/a)	(n/a)	NULL
UnSupervisedDebits	Amount	no	8	19   	4    	no	(n/a)	(n/a)	NULL
DepositBalance	Amount	no	8	19   	4    	yes	(n/a)	(n/a)	NULL
DrawingPower	Amount	no	8	19   	4    	no	(n/a)	(n/a)	NULL
FreezedAmount	Amount	no	8	19   	4    	no	(n/a)	(n/a)	NULL
ProvisionDate	smalldatetime	no	4	     	     	yes	(n/a)	(n/a)	NULL
InterestAccruedUpto	smalldatetime	no	4	     	     	yes	(n/a)	(n/a)	NULL
InterestAppliedUpto	smalldatetime	no	4	     	     	yes	(n/a)	(n/a)	NULL
IsTrxPending	bit	no	1	     	     	yes	(n/a)	(n/a)	NULL
PenaltyAccruedUpto	smalldatetime	no	4	     	     	yes	(n/a)	(n/a)	NULL
PenaltyAppliedUpto	smalldatetime	no	4	     	     	yes	(n/a)	(n/a)	NULL
YearOpeningClearBalance	Amount	no	8	19   	4    	no	(n/a)	(n/a)	NULL
YearOpeningUnclearBalance	Amount	no	8	19   	4    	no	(n/a)	(n/a)	NULL
DayOpeningClearBalance	Amount	no	8	19   	4    	no	(n/a)	(n/a)	NULL
DayOpeningUnclearBalance	Amount	no	8	19   	4    	no	(n/a)	(n/a)	NULL
NoDBTrxafterDormantActivation	tinyint	no	1	3    	0    	no	(n/a)	(n/a)	NULL
LastCreditTrxDate	smalldatetime	no	4	     	     	yes	(n/a)	(n/a)	NULL
LastDebitTrxDate	smalldatetime	no	4	     	     	yes	(n/a)	(n/a)	NULL
OpenedDate	smalldatetime	no	4	     	     	no	(n/a)	(n/a)	NULL
OpenedBy	OperatorID	no	50	     	     	no	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
ApprovedBy	OperatorID	no	50	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
CloseReasonID	UserSubID	no	50	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
CloseReason	Remarks	no	510	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
ClosedDate	smalldatetime	no	4	     	     	yes	(n/a)	(n/a)	NULL
ClosedBy	OperatorID	no	50	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
CreatedBy	OperatorID	no	50	     	     	no	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
CreatedOn	smalldatetime	no	4	     	     	no	(n/a)	(n/a)	NULL
ModifiedBy	OperatorID	no	50	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
ModifiedOn	smalldatetime	no	4	     	     	yes	(n/a)	(n/a)	NULL
SupervisedBy	OperatorID	no	50	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
SupervisedOn	smalldatetime	no	4	     	     	yes	(n/a)	(n/a)	NULL
UpdateCount	tinyint	no	1	3    	0    	no	(n/a)	(n/a)	NULL
InstAmountBooked	Amount	no	8	19   	4    	yes	(n/a)	(n/a)	NULL
BadDebtDueInterestReceivable	money	no	8	19   	4    	yes	(n/a)	(n/a)	NULL
BadDebtDuePenaltyReceivable	money	no	8	19   	4    	yes	(n/a)	(n/a)	NULL
ControlGLType	varchar	no	20	     	     	yes	no	yes	SQL_Latin1_General_CP1_CI_AS
Cat3DuePenaltyReceivable	Amount	no	8	19   	4    	yes	(n/a)	(n/a)	NULL
Cat3DueInterestReceivable	Amount	no	8	19   	4    	yes	(n/a)	(n/a)	NULL
BadDebtInterestSuspended	money	no	8	19   	4    	yes	(n/a)	(n/a)	NULL
BadDebtPenaltyInterestSuspended	money	no	8	19   	4    	yes	(n/a)	(n/a)	NULL
Cat3InterestSuspended	money	no	8	19   	4    	yes	(n/a)	(n/a)	NULL
Cat3PenaltyInterestSuspended	money	no	8	19   	4    	yes	(n/a)	(n/a)	NULL
FICAAccountLevel	int	no	4	10   	0    	no	(n/a)	(n/a)	NULL
ODFeesLimitUsed	Amount	no	8	19   	4    	no	(n/a)	(n/a)	NULL
ClassificationCodeID	UserID	no	50	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
ClassificationSubCodeID	UserSubID	no	50	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
PenaltyPayable	money	no	8	19   	4    	yes	(n/a)	(n/a)	NULL
CompoundPenalty	Amount	no	8	19   	4    	yes	(n/a)	(n/a)	NULL
IsSuspendInterest	bit	no	1	     	     	yes	(n/a)	(n/a)	NULL
IsModified	bit	no	1	     	     	yes	(n/a)	(n/a)	NULL
Digit	varbinary	no	50	     	     	yes	no	yes	NULL
PreviousClearBalance	varchar	no	300	     	     	yes	no	yes	SQL_Latin1_General_CP1_CI_AS
PreviousUnClearBalance	varchar	no	300	     	     	yes	no	yes	SQL_Latin1_General_CP1_CI_AS
BiometricsEnabled	bit	no	1	     	     	no	(n/a)	(n/a)	NULL
CreditInterest	money	no	8	19   	4    	yes	(n/a)	(n/a)	NULL
DebitInterest	money	no	8	19   	4    	yes	(n/a)	(n/a)	NULL
PenaltyInterest	money	no	8	19   	4    	yes	(n/a)	(n/a)	NULL
InterestReceivable	money	no	8	19   	4    	yes	(n/a)	(n/a)	NULL
InterestPayable	money	no	8	19   	4    	yes	(n/a)	(n/a)	NULL
PenaltyReceivable	money	no	8	19   	4    	yes	(n/a)	(n/a)	NULL
InterestSuspended	money	no	8	19   	4    	yes	(n/a)	(n/a)	NULL
PenaltyInterestSuspended	money	no	8	19   	4    	yes	(n/a)	(n/a)	NULL
LossProvisionAmount	money	no	8	19   	4    	yes	(n/a)	(n/a)	NULL
ODueInterestReceivable	money	no	8	19   	4    	yes	(n/a)	(n/a)	NULL
ODuePenaltyReceivable	money	no	8	19   	4    	yes	(n/a)	(n/a)	NULL
AdvancePayment	money	no	8	19   	4    	yes	(n/a)	(n/a)	NULL
IsExported	bit	no	1	     	     	yes	(n/a)	(n/a)	NULL
ExportedOn	smalldatetime	no	4	     	     	yes	(n/a)	(n/a)	NULL
TransferReasonID	UserSubID	no	50	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
TransferReason	Remarks	no	510	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
TransferDate	smalldatetime	no	4	     	     	yes	(n/a)	(n/a)	NULL
TransferBy	OperatorID	no	50	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
LegacyProductID	nvarchar	no	400	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
LegacyAccountID	nvarchar	no	400	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
SalesOfficerID	varchar	no	25	     	     	yes	no	yes	SQL_Latin1_General_CP1_CI_AS
ExportStatusID	varchar	no	25	     	     	yes	no	yes	SQL_Latin1_General_CP1_CI_AS
ODStatusID	SubGroupID	no	40	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
ODHealthCodeID	SubGroupID	no	40	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
CheckSum	nvarchar	no	400	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
CheckSum2	nvarchar	no	400	     	     	yes	(n/a)	(n/a)	SQL_Latin1_General_CP1_CI_AS
IschecksumUpdated	bit	no	1	     	     	yes	(n/a)	(n/a)	NULL