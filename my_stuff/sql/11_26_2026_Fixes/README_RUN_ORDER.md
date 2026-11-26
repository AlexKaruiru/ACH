# 11_26_2026_Fixes: MBSD-1026 follow-up

Target: **BRNET_MB_LIVE** on MNLDHQCBSDB02\SQL2019. Base text: the live `sp_helptext` of `p_AddInwardsTZ`, saved 25-Nov in `../11_25_2026/`.

## 1. p_AddInwardsTZ: keep BOT's original TxId (cheque 211 root cause)
The 15-Jun change (`--bnyaswa 15062026`, line 97) removes `_` from **every** inward TxId. ABSA sends ids such as `doc-001-495-004342_1`. We stored `doc-001-495-0043421` and sent that back in the unpaid return. BOT could not match it, so it rejected the return as "missing OrgnTxID".

The fix keeps Brenda's strip for ids that are numeric once the `_` is removed. The procedure later does `CAST(@TrxID AS BIGINT)` when matching returns of our own cheques, which is the likely reason for her change. Every other id is kept exactly as BOT sent it.
```sql
--SET @TrxID = CAST(REPLACE(@TrxID, '_', '') AS VARCHAR(200)); --bnyaswa 15062026
IF TRY_CAST(REPLACE(@TrxID, '_', '') AS BIGINT) IS NOT NULL SET @TrxID = CAST(REPLACE(@TrxID, '_', '') AS VARCHAR(200)); --alex.ndegwa 26112026 ...
```
| # | Script | What it does |
|---|---|---|
| 0 | check below | Confirm p_AddInwardsTZ has not changed since the 25-Nov save |
| 1 | `p_AddInwardsTZ_26112026.sql` | **Backup**: the live text under the name `p_AddInwardsTZ_26112026` |
| 2 | `p_AddInwardsTZ.sql` | **Fix**: `CREATE OR ALTER`, line 97 only |

```sql
SELECT name, create_date, modify_date FROM sys.objects WHERE name LIKE 'p_AddInwardsTZ%'
-- p_AddInwardsTZ modify_date must be before 25-Nov-2026; otherwise re-save the live text and rebuild the fix on it.
```
- **After deploying:** `SELECT modify_date FROM sys.objects WHERE name = 'p_AddInwardsTZ'` should show today. `OBJECT_DEFINITION(OBJECT_ID('p_AddInwardsTZ')) LIKE '%alex.ndegwa 26112026%'` should be true.
- **Scope:** new inwards only. Items already loaded keep the stripped id.
- **Rollback:** run `../11_25_2026/p_AddInwardsTZ.sql` with `CREATE` changed to `CREATE OR ALTER`.
- **Brenda to confirm the reason when she is back.** If her case was not numeric ids, adjust the condition.

## 2. Reopen the 5,120,000 cheque for a new return (ONLY if BOT agrees)
The 25-Nov return `DRUNPCORUTZTZ251120261038` was built with an old copy of the tool (`E:\Clearing_Production\Prod_FileGenTool`, Dec-2025 build). It went out with settlement date `0001-01-01`.

`Reopen_unpaid_DRUNPCORUTZTZ251120261038.sql` sets `IsGenerated = 0` and `Reference = NULL` on that one row, so the tool lists it again. It ends with **ROLLBACK**. Change it to COMMIT only after:
- BOT confirms it will accept a return for this cheque now;
- all old tool windows are closed, and the tool is opened only from `E:\Clearing_Production\Debug 3_4_2026`.

Then generate the unpaid return from the March build. Check the new file: `IntrBkSttlmDt` must be today's date, not `0001-01-01`.

Do **not** delete the old .Q files. They are already archived and were taken by the gateway, and they are our evidence.

## Run log
- 26-Nov-2026 11:43:19: backup `p_AddInwardsTZ_26112026` created. 11:44:06: fix applied. Post-check: FixPresent = 3075, BackupHasFix = 0.
- Reopen script: NOT run (waiting on BOT).
- 26-Nov ~12:00: reopen dry run (ROLLBACK) OK: 1 row, t_AccountTrxClearing TrxRowID 80124011.
- 26-Nov ~12:10: bank decided not to re-send. COMMIT script NOT run.
