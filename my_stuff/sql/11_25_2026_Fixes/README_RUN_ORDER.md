# 11_25_2026_Fixes: MBSD-1026 (TZ unpaid cheque return rejected by BOT, "missing OrgnTxID")

Target: **BRNET_MB_LIVE** on MNLDHQCBSDB02\SQL2019. Base text: live `sp_helptext` saved on 25-Nov-2026 in `../11_25_2026/`.

## Run order
| # | Script | What it does |
|---|---|---|
| 0 | Check below (read-only) | Confirms the History lookup finds cheque 211's TrxID |
| 1 | `Proc_CreateXMLFiles_Tanzania_Selected_CTS_25112026.sql` | **Backup.** Creates a copy of the current live procedure named `..._25112026`. The live procedure is not changed. |
| 2 | `Proc_CreateXMLFiles_Tanzania_Selected_CTS.sql` | **Fix.** `CREATE OR ALTER` of the live procedure. |

The scripts have no header comments because they go to live. Changes are marked inline as `--alex.ndegwa 25112026`. The old line is kept commented out, in the same style as the `--bnyaswa` changes. The backup is the live text with only the name changed.

## What the fix changes (search for `alex.ndegwa 25112026`)
1. The TrxID "fallback" block now reads **`t_IncomingTransactionsHistory`**. It matches on ColumnID with a ChequeID guard and takes the newest row. The old block only re-read `t_IncomingTransactions` because its History parts were commented out.
2. The cursor that fills `Reference` (MsgID) and `TransactionMicrColumnID` (TrxID) uses `ISNULL(live, History)`.

There are no other changes. The selection logic, the "28/26" UNION, the EFT parts and the output columns are all unchanged.

## 0. Pre-check (read-only): would the fix have found cheque 211?
```sql
SELECT 'live' AS Src, TrxID FROM t_IncomingTransactions (NOLOCK) WHERE ColumnID = 4344
UNION ALL
SELECT TOP 1 'history', TrxID FROM t_IncomingTransactionsHistory (NOLOCK)
WHERE ColumnID = 4344 AND CAST(ChequeID AS VARCHAR(15)) = '211' AND ISNULL(TrxID, '') <> ''
ORDER BY 1
-- Expect: no 'live' row, 'history' = doc-001-495-0043421
```
Also check that History has an index on ColumnID, because the fix reads it per unpaid item:
```sql
EXEC sp_helpindex 't_IncomingTransactionsHistory'
```

## After deploying: test (read-only, rolled back)
Generate the unpaid file for a returned cheque whose inward is already in History, using the tool on a test session. Or run the procedure for TrxRowID 80031940 inside `BEGIN TRAN ... ROLLBACK`, with a login that can read `BRNET_IMAGEServer`. The first result set should show `TrxID = doc-001-495-0043421` and `OrgnlMsgId = QNF2611201243410000002379491`.

## Rollback
```sql
-- Put back the original text (it is saved in ../11_25_2026/Proc_CreateXMLFiles_Tanzania_Selected_CTS.sql):
-- open that file, change "CREATE           PROCEDURE" to "CREATE OR ALTER PROCEDURE", run it.
```
The `_25112026` backup procedure stays as a reference copy in the database.

## Not in this folder (other parts of MBSD-1026)
- **App (`TanzaniaFiles.vb`, branch MBSD-1026):** blocks items with a blank OrgnlTxId/OrgnlMsgId/EndToEnd, shows a warning, and leaves them pending. Also contains the MsgId fix. Not compiled or deployed yet.
- **Still under investigation:** what moved cheque 211 to History between 11:30 and ~12:59. Prime suspect: `Proc_CreateXMLFiles_Tanzania_CTS`, the non-selected generator. Its text is not read yet.
- **Not changed yet:** the `_` stripped from TrxID in `p_AddInwardsTZ` (15-Jun), and the possible duplicate rows from the "28/26" UNION.
