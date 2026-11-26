BEGIN TRAN

UPDATE t_AccountTrxClearing SET IsGenerated = 0, Reference = NULL  --alex.ndegwa 26112026 reopen unpaid for regeneration, BOT rejected DRUNPCORUTZTZ251120261038 (DT01)
WHERE TrxRowID = 80124011 AND Reference = 'DRUNPCORUTZTZ251120261038' AND TrxType = 'OC' AND ReturnCodeID <> '00' AND ABS(Amount) = 5120000

IF @@ROWCOUNT = 1
BEGIN
 COMMIT
 SELECT 'COMMITTED' AS Result
END
ELSE
BEGIN
 ROLLBACK
 SELECT 'ROLLED BACK - row count was not 1' AS Result
END

SELECT TrxRowID, TrxType, ReturnCodeID, Amount, IsGenerated, Reference FROM t_AccountTrxClearing WHERE TrxRowID = 80124011
