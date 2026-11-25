CREATE PROCEDURE p_DeleteUnpostedInwardsTrx  
AS  
DECLARE @ItemCount Int = 0  
  
SELECT @ItemCount = Count(ColumnID) FROM t_IncomingTransactions WHERE  isNull(IsProcessed,0) = 0 AND isNull(Verified,0) = 0  
SELECT @ItemCount ItemCount  
  
DELETE FROM t_IncomingClrTrxExtraDetails   
WHERE TrxID IN (SELECT TrxID FROM t_IncomingTransactions WHERE  isNull(IsProcessed,0) = 0 AND isNull(Verified,0) = 0)   
AND TrxDate = dbo.f_GetWorkingDate('001')  
  
DELETE FROM t_IncomingTransactions WHERE  isNull(IsProcessed,0) = 0 AND isNull(Verified,0) = 0  
  
  
  
  