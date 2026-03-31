ALTER FUNCTION dbo.f_GetOriginatorDetails (@TrxBatchID VARCHAR(20), @TrxDate SMALLDATETIME)
RETURNS TABLE
AS
RETURN (
    SELECT TOP 1 
        A.AccountID as OriginatorAccount,
        dbo.f_GetAccountName(A.OurBranchID, A.AccountID) as OriginatorName
    FROM t_AccountTrx A (NOLOCK)
    INNER JOIN t_Transaction T (NOLOCK) 
        ON A.TrxBatchID = T.TrxBatchID 
        AND A.OurBranchID = T.OurBranchID 
        AND A.AccountID = T.AccountID
    WHERE A.TrxBatchID = @TrxBatchID 
      AND T.AccountTypeID <> 'G'
)
GO
