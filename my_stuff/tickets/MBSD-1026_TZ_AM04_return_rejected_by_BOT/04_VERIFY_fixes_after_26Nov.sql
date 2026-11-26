/* MBSD-1026: verify the fixes on live, read-only. Run any day after 26-Nov-2026 11:44 (p_AddInwardsTZ fix time). */
SET NOCOUNT ON;
IF OBJECT_ID('tempdb..#Out') IS NOT NULL DROP TABLE #Out;
CREATE TABLE #Out (Seq INT IDENTITY(1,1), Line NVARCHAR(MAX));

/* V1. Inwards loaded after the fix that carry "_" in TrxID: these prove the TxId is now kept as BOT sent it.
       (Before the fix none could have "_"; all were stripped since 15-Jun.) */
INSERT INTO #Out(Line)
SELECT 'V1 Kept "_" | ' + ISNULL(TrxID, '') + ' | MsgID ' + ISNULL(MsgID, '') + ' | Bank ' + ISNULL(BankID, '') + ' | Chq ' + ISNULL(CAST(ChequeID AS VARCHAR(20)), '')
FROM (SELECT TrxID, MsgID, BankID, ChequeID FROM t_IncomingTransactions (NOLOCK)
      UNION ALL
      SELECT TrxID, MsgID, BankID, ChequeID FROM t_IncomingTransactionsHistory (NOLOCK) WHERE [Date] >= '2026-11-26') x
WHERE TrxID LIKE '%[_]%';

/* V2. Same check against the extra-details table (the return reads OrgnlMsgId/EndToEnd from here by TrxID). */
INSERT INTO #Out(Line)
SELECT 'V2 ExtraDetails with "_" | ' + TrxID + ' | OrgnlMsgId ' + ISNULL(OrgnlMsgId, '') + ' | TrxDate ' + CONVERT(VARCHAR(10), TrxDate, 120)
FROM t_IncomingClrTrxExtraDetails (NOLOCK)
WHERE TrxDate >= '2026-11-26' AND TrxID LIKE '%[_]%';

/* V3. Inward files received after the fix, by sending bank: how many items, how many with "_". */
INSERT INTO #Out(Line)
SELECT 'V3 Inward after fix | Bank ' + ISNULL(BankID, '') + ' | items ' + CAST(COUNT(*) AS VARCHAR(10))
       + ' | with "_" ' + CAST(SUM(CASE WHEN TrxID LIKE '%[_]%' THEN 1 ELSE 0 END) AS VARCHAR(10))
FROM t_IncomingTransactions (NOLOCK)
WHERE MsgID LIKE 'QNF%' AND SUBSTRING(MsgID, 4, 12) >= '261126114406'
GROUP BY BankID;

/* V4. Unpaid returns generated since the fixes, and whether each would carry a TrxID. */
INSERT INTO #Out(Line)
SELECT 'V4 Unpaid | TrxRowID ' + CAST(v.TrxRowID AS VARCHAR(20)) + ' | Chq ' + ISNULL(CAST(v.ChequeID AS VARCHAR(20)), '')
       + ' | Amt ' + CONVERT(VARCHAR(20), CAST(ABS(v.Amount) AS MONEY), 1) + ' | Ret ' + ISNULL(v.ReturnCodeID, '')
       + ' | IsGen ' + ISNULL(CAST(v.IsGenerated AS VARCHAR(1)), '0') + ' | Ref ' + ISNULL(v.Reference, '-')
       + ' | TrxID ' + COALESCE(l.TrxID, h.TrxID, '*** NOT FOUND ***')
FROM v_Clearing v (NOLOCK)
OUTER APPLY (SELECT TOP 1 NULLIF(TrxID, '') TrxID FROM t_IncomingTransactions (NOLOCK) WHERE ColumnID = v.ColumnID) l
OUTER APPLY (SELECT TOP 1 hh.TrxID FROM t_IncomingTransactionsHistory hh (NOLOCK) WHERE hh.ColumnID = v.ColumnID AND ISNULL(hh.TrxID, '') <> '' ORDER BY hh.[Date] DESC) h
WHERE v.TrxType = 'OC' AND v.ReturnCodeID <> '00' AND ISNULL(v.IsDeleted, 0) = 0 AND v.VoucherCode <> '40' AND v.[Date] >= '2026-11-26'
ORDER BY v.[Date];

IF NOT EXISTS (SELECT 1 FROM #Out) INSERT INTO #Out(Line) SELECT 'Nothing yet: no inwards with "_" and no unpaids since the fix.';
SELECT Line FROM #Out ORDER BY Seq;
