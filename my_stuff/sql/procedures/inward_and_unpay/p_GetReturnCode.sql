CREATE PROCEDURE dbo.p_GetReturnCode    
(  
 @TrxRowID BigInt    
)  
   
AS    
DECLARE @ReturnCode Char(4),  
  @ImageID   BigInt  
SELECT @ReturnCode = ReturnCodeID FROM t_TrxClearing (NOLOCK) WHERE TrxRowID = @TrxRowID  
    
IF ISNULL(@ReturnCode,'')=''    
BEGIN    
 SELECT  @ReturnCode = ReturnCodeID FROM t_AccountTrxClearing  (NOLOCK)  WHERE TrxRowID =  @TrxRowID  
END    
IF ISNULL(@ReturnCode,'')=''    
BEGIN    
 SELECT @ReturnCode = ReturnCodeID FROM t_AccountTrxClearing  (NOLOCK) WHERE TrxRowID = @TrxRowID   
END    
  
IF ISNULL(@ReturnCode,'')=''    
BEGIN    
 SELECT @ImageID = ImageID FROM t_Transaction  (NOLOCK) WHERE TrxRowID = @TrxRowID   
 SELECT @ReturnCode = ReturnCodeID FROM t_TrxClearing  (NOLOCK) WHERE ImageID = @ImageID   
END    
--IF ISNULL(@ReturnCode,'')=''    
--BEGIN   
-- SELECT @ImageID = ImageID FROM t_TrxHistory WHERE TrxRowID = @TrxRowID   
-- SELECT @ReturnCode = ReturnCodeID FROM t_AccountTrxClearing WHERE ImageID = @ImageID   
--END   
SELECT @ReturnCode    
  