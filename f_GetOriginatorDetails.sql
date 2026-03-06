CREATE FUNCTION dbo.f_GetOriginatorDetails (@TrxBatchID VARCHAR(20), @TrxDate SMALLDATETIME)
RETURNS TABLE
AS
RETURN (
    SELECT TOP 1 
        AccountID as OriginatorAccount,
        dbo.f_GetAccountName(NULL, AccountID) as OriginatorName
    FROM t_Transaction (NOLOCK)
    WHERE TrxBatchID = @TrxBatchID 
      AND CAST(TrxDate AS DATE) = CAST(@TrxDate AS DATE)
      AND AccountTypeID = 'C'
)
GO
