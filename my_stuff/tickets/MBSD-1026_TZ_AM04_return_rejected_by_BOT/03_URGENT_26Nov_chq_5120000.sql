/* =====================================================================================
   MBSD-1026 follow-up, 26-Nov-2026: cheque TZS 5,120,000 unpaid 25-Nov ("irregular account name"),
   BOT still shows it "accepted, ready for settlement" (settles ~13:00 today).
   READ-ONLY. Run as ONE batch, copy all "Line" rows back.
   ===================================================================================== */
SET NOCOUNT ON;
IF OBJECT_ID('tempdb..#Out') IS NOT NULL DROP TABLE #Out;
IF OBJECT_ID('tempdb..#C') IS NOT NULL DROP TABLE #C;
CREATE TABLE #Out (Seq INT IDENTITY(1,1), Line NVARCHAR(MAX));

/* ---------- U1. The cheque (any OC row of 5,120,000 since 24-Nov) ---------- */
SELECT TrxRowID, ColumnID, ChequeID, ReturnCodeID, Reference, IsGenerated, [Date]
INTO #C
FROM v_Clearing (NOLOCK)
WHERE TrxType = 'OC' AND ABS(Amount) = 5120000 AND [Date] >= '2026-11-24';

INSERT INTO #Out(Line)
SELECT 'U1 Cheque | TrxRowID ' + CAST(TrxRowID AS VARCHAR(20))
       + ' | ColumnID ' + ISNULL(CAST(ColumnID AS VARCHAR(20)), 'NULL')
       + ' | Chq [' + ISNULL(CAST(ChequeID AS VARCHAR(20)), 'NULL') + ']'
       + ' | Date ' + CONVERT(VARCHAR(19), [Date], 120)
       + ' | Ret ' + ISNULL(ReturnCodeID, 'NULL')
       + ' | RetForBOT ' + ISNULL(dbo.f_CRB_ReturnCodeDescriptionsForUnpay(ReturnCodeID), '*** NULL ***')
       + ' | IsGen ' + ISNULL(CAST(IsGenerated AS VARCHAR(1)), 'NULL')
       + ' | Ref ' + ISNULL(Reference, '-')
FROM #C;
IF NOT EXISTS (SELECT 1 FROM #C) INSERT INTO #Out(Line) SELECT 'U1 *** no OC row of 5,120,000 since 24-Nov ***';

/* ---------- U2. Every column of that row (operator, time, SessNo, ...) ---------- */
DECLARE @sql NVARCHAR(MAX) = N'';
SELECT @sql = @sql
     + N'INSERT INTO #Out(Line) SELECT N''U2 v_Clearing '' + CAST(TrxRowID AS NVARCHAR(20)) + N'' | ' + c.name + N' = '' + ISNULL('
     + CASE WHEN TYPE_NAME(c.system_type_id) IN ('datetime', 'smalldatetime', 'datetime2', 'date')
            THEN N'CONVERT(NVARCHAR(30), ' + QUOTENAME(c.name) + N', 121)'
            ELSE N'CONVERT(NVARCHAR(400), ' + QUOTENAME(c.name) + N')' END
     + N', N''NULL'') FROM v_Clearing (NOLOCK) WHERE TrxRowID IN (SELECT TrxRowID FROM #C);' + NCHAR(10)
FROM sys.columns c
WHERE c.object_id = OBJECT_ID('v_Clearing')
  AND TYPE_NAME(c.system_type_id) NOT IN ('image', 'varbinary', 'binary', 'text', 'ntext', 'xml', 'timestamp')
  AND c.max_length <> -1
ORDER BY c.column_id;
BEGIN TRY EXEC sp_executesql @sql; END TRY
BEGIN CATCH INSERT INTO #Out(Line) SELECT 'U2 ERROR | ' + ERROR_MESSAGE(); END CATCH

/* ---------- U3. The inward: still in the live table, or in History? ---------- */
INSERT INTO #Out(Line)
SELECT 'U3 Inward ' + x.Src + ' | ColumnID ' + CAST(c.ColumnID AS VARCHAR(20))
       + ' | Chq [' + ISNULL(CAST(x.ChequeID AS VARCHAR(20)), 'NULL') + ']'
       + ' | TrxID ' + ISNULL(NULLIF(x.TrxID, ''), '*** BLANK ***')
       + ' | MsgID ' + ISNULL(x.MsgID, '-')
FROM #C c
CROSS APPLY (SELECT 'LIVE' Src, TrxID, MsgID, ChequeID FROM t_IncomingTransactions (NOLOCK) WHERE ColumnID = c.ColumnID
             UNION ALL
             SELECT 'HISTORY', TrxID, MsgID, ChequeID FROM t_IncomingTransactionsHistory (NOLOCK) WHERE ColumnID = c.ColumnID) x;

/* ---------- U4. Would the deployed fix find the TrxID? (exact same match as the procedure) ---------- */
INSERT INTO #Out(Line)
SELECT 'U4 Fix lookup | TrxRowID ' + CAST(c.TrxRowID AS VARCHAR(20))
       + ' | live ' + ISNULL(l.TrxID, '-')
       + ' | history ' + ISNULL(h.TrxID, '*** NOT FOUND ***')
FROM #C c
OUTER APPLY (SELECT TOP 1 TrxID FROM t_IncomingTransactions (NOLOCK) WHERE ColumnID = c.ColumnID) l
OUTER APPLY (SELECT TOP 1 hh.TrxID FROM t_IncomingTransactionsHistory hh (NOLOCK)
             WHERE hh.ColumnID = c.ColumnID
               AND CAST(hh.ChequeID AS VARCHAR(15)) = LTRIM(RTRIM(CAST(c.ChequeID AS VARCHAR(15))))
               AND ISNULL(hh.TrxID, '') <> '' ORDER BY hh.[Date] DESC) h;

/* ---------- U5. Original BOT details (needed in the return) ---------- */
SET @sql = N'';
SELECT @sql = @sql
     + N'INSERT INTO #Out(Line) SELECT N''U5 ExtraDetails | ' + c.name + N' = '' + ISNULL(CONVERT(NVARCHAR(400), e.' + QUOTENAME(c.name)
     + N'), N''NULL'') FROM t_IncomingClrTrxExtraDetails e (NOLOCK) WHERE e.TrxID IN ('
     + N'SELECT TrxID FROM t_IncomingTransactions (NOLOCK) WHERE ColumnID IN (SELECT ColumnID FROM #C) UNION '
     + N'SELECT TrxID FROM t_IncomingTransactionsHistory (NOLOCK) WHERE ColumnID IN (SELECT ColumnID FROM #C));' + NCHAR(10)
FROM sys.columns c
WHERE c.object_id = OBJECT_ID('t_IncomingClrTrxExtraDetails')
  AND TYPE_NAME(c.system_type_id) NOT IN ('image', 'varbinary', 'binary', 'text', 'ntext', 'xml', 'timestamp')
  AND c.max_length <> -1
ORDER BY c.column_id;
BEGIN TRY EXEC sp_executesql @sql; END TRY
BEGIN CATCH INSERT INTO #Out(Line) SELECT 'U5 ERROR | ' + ERROR_MESSAGE(); END CATCH

/* ---------- U6. When the fix went live (compare with the time in the .Q name / Ref) ---------- */
INSERT INTO #Out(Line)
SELECT 'U6 Object | ' + name + ' | modified ' + CONVERT(VARCHAR(19), modify_date, 120)
FROM sys.objects WHERE name LIKE 'Proc_CreateXMLFiles_Tanzania%' ORDER BY name;

/* ---------- U7. BOT responses stored for unpaid files of 21, 25, 26 Nov ---------- */
SET @sql = N'';
SELECT @sql = @sql
     + N'BEGIN TRY INSERT INTO #Out(Line) SELECT N''U7 ' + t.name + N' | MsgID '' + ISNULL(CONVERT(NVARCHAR(100), MessageID), ''-'')'
     + N' + N'' | Status '' + ISNULL(CONVERT(NVARCHAR(50), FileStatus), ''-'')'
     + N' + N'' | Reason '' + ISNULL(CONVERT(NVARCHAR(400), RejectedReason), ''-'')'
     + N' + N'' | File '' + ISNULL(CONVERT(NVARCHAR(200), FileName), ''-'')'
     + N' FROM ' + QUOTENAME(t.name) + N' (NOLOCK) WHERE MessageID LIKE ''DRUNP%25112026%'' OR MessageID LIKE ''DRUNP%26112026%'''
     + N' OR MessageID LIKE ''DRUNP%21112026%'' OR MessageID IN (SELECT Reference FROM #C)'
     + N' OR FileName LIKE ''%DRUNP%25112026%'' OR FileName LIKE ''%DRUNP%26112026%'';'
     + N' END TRY BEGIN CATCH INSERT INTO #Out(Line) SELECT ''U7 ' + t.name + N' ERROR | '' + ERROR_MESSAGE(); END CATCH' + NCHAR(10)
FROM sys.tables t
WHERE EXISTS (SELECT 1 FROM sys.columns WHERE object_id = t.object_id AND name = 'MessageID')
  AND EXISTS (SELECT 1 FROM sys.columns WHERE object_id = t.object_id AND name = 'RejectedReason')
  AND EXISTS (SELECT 1 FROM sys.columns WHERE object_id = t.object_id AND name = 'FileStatus')
  AND EXISTS (SELECT 1 FROM sys.columns WHERE object_id = t.object_id AND name = 'FileName');
IF @sql = N'' INSERT INTO #Out(Line) SELECT 'U7 *** no table with MessageID/RejectedReason/FileStatus/FileName ***';
ELSE EXEC sp_executesql @sql;

/* ---------- U8. ALL unpaid cheques since 24-Nov: generated? would the return carry a TrxID? ----------
   Any row with IsGen 0, or TrxID NOT FOUND, or RetForBOT NULL is at risk before today's cut-off. */
INSERT INTO #Out(Line)
SELECT 'U8 Unpaid | TrxRowID ' + CAST(v.TrxRowID AS VARCHAR(20))
       + ' | Chq ' + ISNULL(CAST(v.ChequeID AS VARCHAR(20)), 'NULL')
       + ' | Amt ' + CONVERT(VARCHAR(20), CAST(ABS(v.Amount) AS MONEY), 1)
       + ' | Date ' + CONVERT(VARCHAR(19), v.[Date], 120)
       + ' | Ret ' + ISNULL(v.ReturnCodeID, 'NULL')
       + ' | RetForBOT ' + ISNULL(dbo.f_CRB_ReturnCodeDescriptionsForUnpay(v.ReturnCodeID), '*** NULL ***')
       + ' | IsGen ' + ISNULL(CAST(v.IsGenerated AS VARCHAR(1)), '0')
       + ' | Ref ' + ISNULL(v.Reference, '-')
       + ' | TrxID ' + COALESCE(l.TrxID, h.TrxID, '*** NOT FOUND ***')
FROM v_Clearing v (NOLOCK)
OUTER APPLY (SELECT TOP 1 NULLIF(TrxID, '') TrxID FROM t_IncomingTransactions (NOLOCK) WHERE ColumnID = v.ColumnID) l
OUTER APPLY (SELECT TOP 1 hh.TrxID FROM t_IncomingTransactionsHistory hh (NOLOCK)
             WHERE hh.ColumnID = v.ColumnID
               AND CAST(hh.ChequeID AS VARCHAR(15)) = LTRIM(RTRIM(CAST(v.ChequeID AS VARCHAR(15))))
               AND ISNULL(hh.TrxID, '') <> '' ORDER BY hh.[Date] DESC) h
WHERE v.TrxType = 'OC' AND v.ReturnCodeID <> '00' AND ISNULL(v.IsDeleted, 0) = 0
  AND v.VoucherCode <> '40' AND v.[Date] >= '2026-11-24'
ORDER BY v.[Date];

SELECT Line FROM #Out ORDER BY Seq;
