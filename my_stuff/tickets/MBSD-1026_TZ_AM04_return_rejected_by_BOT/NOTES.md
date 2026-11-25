# MBSD-1026 — TZ: unpaid cheque (AM04) return rejected by BOT, cheque settled

## Facts from the bank
- **Cheque:** 000211, TZS 5,323,100, drawer SAFE HAND COMPANY LIMITED (A/C 024244426061), payee is an ABSA customer (BARCTZTZ).
- **ACH IDs:** Tx id `doc-001-495-004342_1`, EndToEnd `AB019MK1795167518917`.
- **Unpay:** marked unpaid with AM04 at about 11:30 on 21-Nov-2026, in BR verification (manual unpay).
- **Our rows:** `t_AccountTrxClearing` has:
  - ID row 91255 / TrxRowID 80031937, code `00`, amount -5,323,100.
  - OC row 91256 / TrxRowID 80031940, code `AM04`, OrigRefCode `Cheque Unpay : 211`, amount 5,323,100.
- **Statement:** shows the inward debit and then the `Cheque Unpay : 211` credit. The unpay posted correctly in core banking.
- **BOT:** rejected the unpaid file with reason **missing OrgnTxID**. Cut-off passed, so the cheque was deemed settled. TZS 5,323,100 is not recovered.
- **Recurrence:** this is the **second** such incident in November.

## How OrgnlTxId gets into the unpaid file (pacs.002)
`TanzaniaFiles.vb` ChequeReturn → `Proc_CreateXMLFiles_Tanzania_Selected_CTS` (Session 2), which follows this chain:

```
OC row (v_Clearing: t_AccountTrxClearing ∪ t_TrxClearing).ColumnID
   └─> t_IncomingTransactions.ColumnID  ──> TrxID   ==> <OrgnlTxId>
                                      └─> MsgID
TrxID ──> t_IncomingClrTrxExtraDetails ──> OrgnlMsgId, OrgnlEndToEnd, Dbtr/Cdtr ...
```

`UnpaidCheques()` sets `OrgnlTxId = TrxID`. Before saving, it **deletes every empty XML element** (`TanzaniaFiles.vb` ~line 2069). A blank TrxID therefore makes the `<OrgnlTxId>` tag disappear completely, which matches BOT's "missing OrgnTxID".

TrxID is blank when any link in the chain breaks:
1. The OC row's `ColumnID` is NULL/0 or wrong, so it doesn't point to the inward row.
2. The inward row is no longer in `t_IncomingTransactions`. In our copy of the procedure the `...History` fallback is **commented out**, and there is no lookup anywhere else.
3. `t_IncomingTransactions.TrxID` itself is empty.

Which of these happened is answered by `02_READONLY_why_OrgnlTxId_was_blank.sql`:
- A + B show the link columns.
- D re-runs the generator for TrxRowID 80031940 inside a rolled-back transaction and shows the exact TrxID it produces.

**Caveat:** our SQL copies are OLD. Use the client's current text (Q9 in `01_...sql`) for the final RCA.

## Draft answers to the bank's RCA questions (finalise after A–D)
1. **Cause of missing OrgnTxID.** The file generator could not link the unpaid OC entry back to its original inward record, so the original transaction ID was empty. The XML clean-up step then removed the empty tag. The broken link is one of 1–3 above, to be confirmed from the data.
2. **Why it settled despite the unpaid status.**
   - The unpaid status lives only in the bank's core system. For BOT, an item is only returned when a valid return reaches TACH before cut-off. The rule quoted by the bank says that without one, the item is deemed settled.
   - Our tool marks the item "generated" when it creates the file. When BOT rejects the file, the item is **not** put back for re-generation.
3. **Category.**
   - File generation: data linkage plus missing validation.
   - Integration monitoring: the BOT rejection was not raised as an alert.
   - Not a CBS configuration issue as far as the code shows (to be confirmed with Q8 on recently changed objects).
4. **Why the rejection was not caught before cut-off.**
   - BOT's response file is read and stored (FileStatus `R` plus reason, via `p_AddClearingFileStatus`). Nothing alerts the operator, and nothing resets the item.
   - Someone has to look at the status screen and fix the item manually, all before cut-off.
5. **Corrective actions.**
   - *Current incident:* the bank needs to pursue recovery with ABSA/BOT under TACH rules. Our system cannot reverse a settled item.
   - *Done in code (branch MBSD-1026, not yet deployed or compiled):*
     - **New:** before building the file, every unpaid item is checked for OrgnlTxId, OrgnlMsgId and OrgnlEndToEndId. If one is blank, the item is held back, a warning pops up immediately with the cheque number, and it is logged. The item stays pending. This stops one bad item from getting the whole file rejected, and gives the operator time to act before cut-off.
     - The message ID could repeat (`"HHmm" + counter` was added as numbers). Now `HHmmss` + a 3-digit counter is used.
     - Items are no longer marked generated when the file was not created.
   - *Pending data:*
     - Fix the actual broken link (1–3), e.g. restore the History fallback or set ColumnID on the OC row.
     - Draft procedure fix for TrxID+MsgID matching (`DRAFT_FIX_on_OLD_copy_...sql`).
   - *Recommended next:*
     - On a BOT `RJCT` response for an unpaid file, set `IsGenerated = 0` and alert the user, so the item can be corrected and re-sent.
     - Generate unpaid files well before cut-off.

## Findings log (client DB BRNET_MB_LIVE on MNLDHQCBSDB02\SQL2019, run 25-Nov-2026)

### Step 1: unpay rows
| | ID row 80031937 | OC row 80031940 |
|---|---|---|
| ColumnID | 4344 | 4344 |
| ReturnCodeID | 00 | AM04 |
| IsGenerated | NULL | 1 |
| Reference | NULL | `DRUNPBARCTZTZ211120261260` |
| Date | 21-Nov | 21-Nov |

- The rows are in `t_AccountTrxClearing`. `t_TrxClearing` is empty for them because EOD has run, as expected.
- **The OC row's ColumnID is set (4344, same as the ID row).** Link break #1 (OC row not pointing anywhere) is ruled out.
- The file was generated and marked sent (`IsGenerated = 1`). The tool will never pick the item up again.
- The MsgId ends in `1260`, which is the HHmm+counter bug (12:59 + 1). So the file was generated at about **12:58–12:59**, roughly 1.5 h after the 11:30 unpay.
- The ColumnID value is small (4344) compared with TrxRowID (80M). Suspect the inward table is purged or re-seeded; check B5/B6.

### Step 2: inward record
- **B1 (cheque 211 + amount), B2 (ColumnID 4344) and B3 (TxId) all return nothing.** The inward for cheque 211 is **no longer in `t_IncomingTransactions`**. The TrxID lookup in the generator uses only that table.
- B4 lists the inward-like tables:

  | Table | Created | Note |
  |---|---|---|
  | `t_IncomingTransactionsHistory` | 2025-11-26 | **Exists.** In our copy of the generator, the History fallback is commented out. |
  | `t_IncomingTransactions_210…` | 2026-04-21 | Manual copy/backup? Ask who made it and why. |
  | `t_IncomingTransactions_MB…` (×2) | 2026-04-24 | Same question. |
  | `t_IncomingTransactions` | 2021-12-22 | Modified 2024-02-11 |
  | `t_IncomingClrTrxExtraDetails` | | Modified 2024-01-31 |
  | `t_TrxInwards`, `t_TrxInwardsHistory`, `t_Inwards`, `t_IncomingChequeImages`, `t_TissIncomingMessage`, `t_SwiftIncomingMessage_St…`, `t_GatewayIncomingFileInfo` | | Other inward-related tables |

- **Working theory (not proven):** inward records are moved from `t_IncomingTransactions` to `t_IncomingTransactionsHistory` by some job or EOD. If that happened before 12:59 on 21-Nov, or if the record had been moved earlier (the cheque was dated and stamped at ABSA on 20-Nov), the generator found no TrxID, so the file had no OrgnlTxId.
- **Caveat:** "empty today" does not prove it was missing at 12:59 on 21-Nov. We need to find the record in History and **when** it was moved.
- **Pending:**
  - B5/B6 (table range, ColumnID 4344 today).
  - Search History for cheque 211.
  - Find what moves the records (Step 7c jobs, procedure text).
  - Check whether the client's deployed generator has the History fallback (Step 8).

### Step 2 B5/B6: inward table range
- `t_IncomingTransactions` has **31 rows, all dated 2026-11-25 (today only)**. ColumnID runs from 201 to 4393, and IDENT_CURRENT is 4393.
- **This is a daily working table.** Earlier days are moved out, most likely to `t_IncomingTransactionsHistory`. The identity is not reseeded (4344 on 21-Nov, 4393 now, about 31 items a day).
- Anomaly: today's minimum ColumnID is **201**, far below the rest. A re-inserted or re-loaded row? Note it only.
- ColumnID 4344 is not in the table today (B6 is empty), as expected.
- **This shifts the question.** The 11:30 unpay read the inward from `t_IncomingTransactions` and succeeded, so the row was there at 11:30. Two possibilities:
  - (a) It was moved out between 11:30 and the 12:59 generation (a mid-day job or session close).
  - (b) It was still there, but its `TrxID` was blank.
- **Next:** find cheque 211 in `t_IncomingTransactionsHistory` (B7) and check its TrxID and its move/archive time. Then find what moves the rows and when (Step 6/7c).

### Step 3: original-transaction details (`t_IncomingClrTrxExtraDetails`)
| TrxID (as displayed) | TrxDate | OrgnlMsgId | OrgnlInstrId | OrgnlEndToEnd | Amount | SourceBIC |
|---|---|---|---|---|---|---|
| `doc-001-495-0043421`* | **2026-11-20** | `QNF26112012434100000002379491` | `doc-001-495-0043421`* | `AB019MK1795167518917` | 5,323,100.00 | BARCTZTZ |

\* The grid shows no underscore. The ACH screen shows `doc-001-495-004342_1`. SSMS may be clipping the `_`; confirm with `LEN(TrxID)` / `REPLACE(TrxID,'_','[_]')`.

- **Exactly one row.** The TxId does not repeat, so the TrxID-only join (suspect 1, draft procedure fix) is **not** this incident's cause.
- **The original details are complete.** If the generator had found TrxID, OrgnlMsgId and OrgnlEndToEnd would also have been filled.
- **TrxDate = 20-Nov.** The inward was **loaded on 20-Nov** (working date 20-Nov). BOT's message `QNF2611201243…` is from 20-Nov around 12:43 and matches the ACH "Sorted Batch Reference". The unpay and return were on **21-Nov** (the D+1 return window). The ID/OC rows are dated 21-Nov.
- **This makes theory (a) much more likely.** The cheque was a previous-day inward. It was still in `t_IncomingTransactions` at 11:30 on 21-Nov (the unpay posted correctly from it). By about 12:59 it was gone from there, plausibly moved to `t_IncomingTransactionsHistory` by whatever clears previous-day rows. The generator only looks in `t_IncomingTransactions`, so TrxID was blank and OrgnlTxId was missing.
- **Prediction to test:**
  - Steps 5 and 5b should show that blank-TrxID unpays are those where the inward date is earlier than the unpay date.
  - The second November incident should also be a next-day unpay.
  - B7 should show cheque 211 in History, with TrxID filled.
  - Step 6/7c should show a job or session close that moves previous-day rows during the day on 21-Nov.

### Step 4: re-run the generator (skipped)
- Failed with `Msg 916` at procedure line 312: login `MBS0108` cannot access `BRNET_IMAGEServer` (the image copy `SELECT * INTO #ChequeIMages`). The tool's own login has that access.
- Updates had already run inside the transaction ("4 rows" / "6 rows affected"). I asked the user to run `IF @@TRANCOUNT > 0 ROLLBACK` in that same window and confirm the count is 0.
- **Skipped on purpose.** Re-running today would always give a blank TrxID because the live table only holds today's rows. It can't show what the procedure saw at 12:59 on 21-Nov. The timing question goes to B7, 5c, 6 and 7c instead.

### Step 5 / 5b: pattern (not conclusive, query design)
- Every OC since Sep shows a blank TrxID (WithBlankTxId = Unpays on every day). This is expected, because `t_IncomingTransactions` holds today only. **These two queries cannot tell good unpays from bad ones. Ignore them.**
- They do confirm a few things:
  - ColumnID is set on all OC rows (3504… in Sep up to 4344 on 21-Nov).
  - All have IsGenerated = 1 and a DRUNP Reference.
  - ReturnCodeID stores ISO codes (CH11, CH05, CH20, MS02, AM04…).
- **Use S5c instead.** It takes the inward date from History and shows how many days after the inward each unpay was done. Then match it against BOT's accept/reject per DRUNP Reference (Step 9).

### S3b: exact TrxID
- Stored TrxID = `doc-001-495-0043421`, **LEN 19, no underscore**. OrgnlInstrId is the same.
- The ACH screen shows `doc-001-495-004342_1` (20 chars).
- **Secondary risk:** even a filled-in OrgnlTxId might not match BOT's. Is `_` dropped by our inward load, or absent in BOT's file? Check F3 (the inward file) and `p_AddInwardsTZ` / the inward parser.

### S5c: unpay day vs inward day (partial view)
- The rows visible on screen (12-Oct to 03-Nov) are all **DaysLater = 0** (same-day unpays), each with its inward found in History.
- November rows were not visible. A filtered query was given (only rows where DaysLater <> 0 or the History row is missing). Expect only cheque 211 and the 2nd November case.

### Step 6: branch status (current state only)
- Branches 000–006: day open, SODDate 25-Nov, EODDate 24-Nov, EOMDate 31-Oct.
- **CEDate differs:** 23-Nov for branches 000, 001 and 003; 24-Nov for 002, 004, 005 and 006. CEDate is possibly the "clearing end" date: a separate clearing close per branch that could be what moves inwards to History. Unconfirmed. Find what sets CEDate (search procedures for `CEDate`).
- This table shows today's state only. **It cannot show what ran at 11:30–12:59 on 21-Nov.** That needs SQL Agent job history (msdb) or an EOD/clearing-close log, plus the text of the routine that fills `t_IncomingTransactionsHistory`.

### Step 7: what changed (important)
**S7a, objects in the return chain WERE changed on the DB side:**

| Object | Modified | Note |
|---|---|---|
| `Proc_CreateXMLFiles_Tanzania_Selected_CTS` | **2026-08-26 10:22:24** | The file generator |
| `Proc_CreateXMLFiles_Tanzania_Selected_CTS_26082026` | created 2026-08-26 10:21:49 | **Backup taken 35 s before the change = the PRE-change version** |
| `v_Clearing` | **2026-08-26 10:46:03** | The view the generator reads (no backup copy seen) |
| `p_InsertTransactions` | 2026-08-27 15:02 | |
| `p_SuperviseTrx` (+ `_Backup25092026`) | 2026-09-25 10:36 | Supervision |

- So **"no change for 8 months" holds for the tool (exe/dll), NOT for the database objects it depends on.** The generator and the view were changed on 26-Aug-2026. The failures are seen from November on, which needs explaining. Possibly the change only bites on next-day unpays, which are rare (S5c: all Oct/early-Nov unpays were same-day).
- **Next:** diff the current procedure against `_26082026` and get `v_Clearing` text. Look for the History fallback being commented out, or a changed TrxID/ColumnID join.

**On the incident day:**
- `t_Transaction` and all 4 of its triggers (`tr_AddTransactions`, `tr_AddTransactions_ChkSum`, `tr_DeleteTransactions`, `tr_UpdateTransactions`) were modified at **2026-11-21 16:51:21**. That is the same timestamp on all of them, which looks like a table-level ALTER or a trigger disable/enable.
- It happened **after** the 12:59 generation, so it is not the cause. But someone changed the core transaction table on the incident afternoon. Ask who and why (manual correction of the unpay?).
- A daily DB audit exists: `t_DBAuditSpecificationData_DDMMYYYY` (21112026 exists). It may show who did it.

**S7b:** only the 4 `t_Transaction` triggers. There are no triggers on the clearing or inward tables.

**S7c:** no SQL Agent job step touches `IncomingTransactions` or `TrxClearing`. The move to History is done by the application or a procedure (EOD / clearing close, see CEDate), not by a scheduled job.

**Other recent changes (probably unrelated):** MobileGwy24 account procedures (Nov), `p_PostSwiftInCommingTransactions` (16-Nov), CRB TZ procedures (Aug), `p_GetTISSPendingOutgoingTrxn` (Sep).

### Live object texts, round 2 Step 1 (priorities 1+2, saved in `my_stuff/sql/11_25_2026/`)

**The 26-Aug change is NOT the cause.**
- Current generator vs `_26082026`: the only change is `LTRIM/RTRIM` of AccountID/TheirAccount in the **EFT** (`#MICREFTs`) section.
- `v_Clearing` (26-Aug): it now strips spaces from DrawerOrPayeeAccountID, and for OD rows takes AccountID from Reference. No effect on TrxID.

**The live generator confirms the mechanism.**
- TrxID (= OrgnlTxId) is looked up **only in `t_IncomingTransactions`** (lines ~330–347).
- The "fallback" block is a copy of the same lookup. Its `--History` parts are commented out, so it never looks in `t_IncomingTransactionsHistory`.
- An inward that has already left the live table therefore gives a blank TrxID.

**None of the saved chain procedures moves inwards to History.** That includes `p_AddInwardsTZ`, `p_ProcessIncomingTxns`, `p_AddIncomingTrx`, `p_AddOutgoingTrx`, `p_SuperviseTrx` and `p_InsertTransactions`. The mover is elsewhere: R3 must find it.

**Other DB changes found, tagged "bnyaswa" in the code:**

| Date (tag) | Object | Change | Relevance |
|---|---|---|---|
| **13-Jul-2026** | `p_AddInwardsTZ` ~1330 | **Auto-unpay at load removed:** "so that all incoming transactions go to verification queue and the decision be made there so we don't miss the settlement window" | **High.** Before this, AM04 (and closed/blocked/stopped…) were unpaid automatically on the **load day**. Now an operator decides, so decisions can slip to the **next day**. Next-day unpays are exactly the case where the inward has left the live table. Explains why cheque 211 was not auto-unpaid on 20-Nov. |
| **15-Jun-2026** | `p_AddInwardsTZ` line 97 | `SET @TrxID = REPLACE(@TrxID, '_', '')`: **underscore stripped from BOT's TxId at load** | **Medium.** The stored TrxID `doc-001-495-0043421` ≠ BOT's `doc-001-495-004342_1`. Even when found, the OrgnlTxId we return is **not BOT's original**. This is a separate rejection risk for any TxId containing `_`. Check F3 and BOT's view. |
| "28/26" | generator ~257 | 2nd SELECT of the UNION changed from `VoucherCode = '40'` to `<> '40'` (no ReturnCode / 58-59 filter) | **Check.** Every unpaid cheque OC row matches **both** SELECTs, so it is inserted **twice** (UNION ALL, no de-dup). The file may carry each cheque twice. DD (voucher 40) returns no longer come through this branch. Verify with F1. |
| 06-May-2026 | `p_AddIncomingTrx` ~177 | Contra GL `CUR_CLR_AC_IN` for ID on GL accounts | Low |
| 26-Aug-2026 | generator, `v_Clearing` | Trims (see above) | None for this incident |

**Revised root-cause picture (still to confirm with R2–R4 and F1/F2):**
1. Since 13-Jul, inward debits are no longer auto-unpaid at load. They wait in verification for a manual decision.
2. Cheque 211 arrived on 20-Nov and was only unpaid on 21-Nov at 11:30, a day later.
3. By the time the return file was built (~12:59), the 20-Nov inward was no longer in `t_IncomingTransactions`. The mover and its timing are not yet known.
4. The generator does not look in History, so TrxID was blank. `<OrgnlTxId>` was stripped, BOT rejected the file, and the cut-off passed.
5. Even if found, the TrxID would have lacked the `_` (15-Jun change). That is a possible second reason for a reject.

So "the tool hasn't changed" holds for the exe. **Several DB procedures in the chain were changed in May–Aug 2026**, and the 13-Jul process change makes next-day unpays (and this failure) possible.

### Next steps (revised 25-Nov after Step 7)
`00_DIAGNOSIS_RUN_IN_ORDER.sql` has been rewritten as **round 2**. The round-1 queries are removed; their results are in this log. Order:
1. **STEP 1:** `sp_helptext` the objects, saved to files (priorities 1/2/3).
2. **STEP 2:** quick checks R1–R6.
3. **STEP 3:** files F1–F6.

Questions the revised round answers:
1. Did the 26-Aug change remove or break the History lookup? (R1 + diff of current vs `_26082026`)
2. Was cheque 211 in History with its TrxID filled? (R2)
3. What moves inwards to History, and can it run mid-day? (R3 + its text)
4. Are failures exactly the next-day unpays? (R4)
5. Who altered `t_Transaction` at 16:51 on 21-Nov? (R5)
6. BOT's recorded rejection and its time (R6)

## Files in this folder
| File | Purpose |
|---|---|
| **`00_DIAGNOSIS_RUN_IN_ORDER.sql`** | **Start here.** Steps 1–9 in order, plus the files to collect. Supersedes the order of 01/02. |
| `01_READONLY_scripts_for_bank.sql` | Broad evidence: inward, extra details, DRUNP files, current procedure text, recently changed objects, plus the files to collect |
| `02_READONLY_why_OrgnlTxId_was_blank.sql` | Targeted to cheque 211, using the IDs from the bank's screenshot |
| `DRAFT_FIX_on_OLD_copy_...sql` | Do not deploy. Written against the old procedure copy. |

Originals before the fix: see git history and `my_stuff/sql/`.
