SELECT 'before' AS Stage, 't_TrxClearing' AS Tbl, TrxRowID, TrxType, ReturnCodeID, Amount, IsGenerated, SessNo, Reference FROM t_TrxClearing WHERE Reference = 'DRUNPCORUTZTZ251120261038'
UNION ALL
SELECT 'before', 't_AccountTrxClearing', TrxRowID, TrxType, ReturnCodeID, Amount, IsGenerated, SessNo, Reference FROM t_AccountTrxClearing WHERE Reference = 'DRUNPCORUTZTZ251120261038'

BEGIN TRAN

UPDATE t_TrxClearing SET IsGenerated = 0, Reference = NULL  --alex.ndegwa 26112026 reopen unpaid for regeneration, BOT did not take DRUNPCORUTZTZ251120261038
WHERE Reference = 'DRUNPCORUTZTZ251120261038' AND TrxType = 'OC' AND ReturnCodeID <> '00' AND ABS(Amount) = 5120000
SELECT @@ROWCOUNT AS TrxClearingRows

UPDATE t_AccountTrxClearing SET IsGenerated = 0, Reference = NULL  --alex.ndegwa 26112026 reopen unpaid for regeneration, BOT did not take DRUNPCORUTZTZ251120261038
WHERE Reference = 'DRUNPCORUTZTZ251120261038' AND TrxType = 'OC' AND ReturnCodeID <> '00' AND ABS(Amount) = 5120000
SELECT @@ROWCOUNT AS AccountTrxClearingRows

SELECT 'after' AS Stage, TrxRowID, TrxType, ReturnCodeID, Amount, IsGenerated, Reference FROM v_Clearing WHERE TrxType = 'OC' AND ABS(Amount) = 5120000 AND [Date] >= '2026-11-24'

ROLLBACK  -- expect 1 row in total; change to COMMIT only when BOT has agreed to accept a new return
