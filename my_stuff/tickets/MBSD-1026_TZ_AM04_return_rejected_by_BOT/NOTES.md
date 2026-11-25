# MBSD-1026 — TZ: unpaid cheque (AM04) return rejected by BOT, cheque settled

**Case:** cheque 000211, TZS 5,323,100, drawer SAFE HAND COMPANY LIMITED, presented by BARCTZTZ on 21-Nov-2026.
- ACH Tx id: `doc-001-495-004342_1`
- ACH EndToEnd: `AB019MK1795167518917`
- The core banking statement shows `INWARD DEBIT ChqID 211` and then `Cheque Unpay : 211` on the same day. Our side did unpay it.
- ACH shows the original item as Accepted/Settled, because BOT rejected our return file.

## How the return is built (BRNET)
1. The unpay creates an `OC` row in `t_TrxClearing`/`t_AccountTrxClearing`, with `ReturnCodeID = 50` and `IsGenerated = 0`.
2. The operator ticks the row. `TanzaniaFiles.vb` (FileType.ChequeReturn) calls `Proc_CreateXMLFiles_Tanzania_Selected_CTS` with `Session = 2`.
3. The procedure links `OC.ColumnID` → `t_IncomingTransactions.ColumnID` → `TrxID`, then `t_IncomingClrTrxExtraDetails` (joined on TrxID only) → OrgnlMsgId, OrgnlEndToEnd, etc.
4. `UnpaidCheques()` writes a pacs.002 `RJCT` file with MsgId `DRUNP<destBIC><ddMMyyyy>...`, then signs it.
5. The rows are flagged `IsGenerated = 1`, `Reference = MsgId`.

## Suspects found in code (fixed)
| # | Where | Problem | Fix |
|---|---|---|---|
| 1 | Procedure, extra-details join | Joins on `TrxID` alone. TxId is the presenting bank's number and can repeat, while `p_AddInwardsTZ` keys on TrxID + MsgID. A repeat makes the return carry another cheque's OrgnlMsgId/OrgnlEndToEndId, and BOT rejects it. This fits "worked for a year": the sequence numbers start repeating. | **Draft only**: `DRAFT_FIX_on_OLD_copy_...sql`. It was written against an OLD copy. Re-apply it to the client's current text (Q9) if Q2 confirms. |
| 2 | `TanzaniaFiles.vb` `UnpaidCheques` MsgId | `STm1 + Cntr` added numbers, e.g. "1430"+2 = 1432. A file at 14:30 #2 and one at 14:31 #1 got the same MsgId, and BOT rejects a duplicate. | MsgId now uses `HHmmss` + a 3-digit counter |
| 3 | `TanzaniaFiles.vb` ChequeReturn caller | If the XML failed to build, rows were still marked `IsGenerated = 1` with an empty Reference, so the return was never sent. | Rows stay pending and an error is shown |

**Not changed (flag only):**
- If signing fails, the file is still treated as generated. The unsigned copy is left in `\Files`.
- `IntrBkSttlmDt` in the return is the PC date (`Date.Today`), not the original settlement date.

## Confirming it
`01_READONLY_scripts_for_bank.sql`:
- **Q2** with more than one row confirms suspect 1.
- **Q5** plus the file timestamps confirm suspect 2.
- **F2** (BOT's response file) gives the actual reject code. Get it first.

Originals before the fix: see git history (commit before the MBSD-1026 commit) and `my_stuff/sql/`.

**Important:** the SQL in `my_stuff/sql` is OLD and not what the client runs. Every SQL conclusion needs re-checking against the Q9 output. The VB fixes are in the source repo (`Common/Modules/TanzaniaFiles.vb`). Confirm the deployed build matches the source (F6).
