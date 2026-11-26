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

### Round 2 Steps 1–3 results (25-Nov)

**S1, objects referencing History / CEDate / DELETE of inwards.** The query was too broad: `%CEDate%` also matches "Balan**ceDate**", so dozens of balance functions came back. The relevant hits:

| Object | Why |
|---|---|
| `p_DeleteUnpostedInwardsTrx` | Name suggests it removes or moves unposted inwards. **Prime suspect.** |
| `sb_ClearEffects`, `ps_ClearEffects` | "Clear effects". **CEDate probably = Clear-Effects date** (it differs per branch). This may run per branch during the day and move inwards to History. |
| `pc_SystemBranchStatus` | Sets branch dates (CEDate?) |
| `p_DoInwardsFileTrxValidationsTZ` | Called at the end of `p_AddInwardsTZ` |
| `sb_EndOfDay` (+ `_28042026` backup), `ps_UpdateGLBalance` | EOD |
| `Proc_CreateXMLFiles_Tanzania_CTS` | The non-selected generator (it mentions History) |
| `rw_IncomingTransactions` | A report |

**S2, cheque 211 IS in `t_IncomingTransactionsHistory`** (ColumnID 4344). The columns were unlabelled; values recognised:
- `TrxID = doc-001-495-0043421` (filled) and `MsgID = QNF2611201243410000002379491`.
- FileName `MBTLTZTZ261120124341973.`, account 024244426061, ID, cheque 211, 5,323,100.
- `ReturnedTempID = 50`, operator `MB0194`, and the timestamp **2026-11-21 11:30:58.307** (the manual unpay).
- **Row `Date = 2026-11-21`.** A separate `2026-11-20` value is also in the row; the ExtraDetails TrxDate is 20-Nov.

So History had a valid TrxID. Had the generator looked in History, OrgnlTxId would have been filled (without the `_`).

**S3, the "next-day unpays" query found nothing relevant.**
- The only row is cheque 205 (5,120,000, unpaid today 25-Nov, DRUNP…CORUTZTZ…1038), still in the live table.
- Cheque 211 did not show because its History `Date` (21-Nov) equals the unpay date. The History Date is not the load date.
- **The query must compare against the ExtraDetails `TrxDate` (load date), not the History `Date`.** Rewritten for round 3.

**Still open:** when and by whom the cheque 211 row was moved from `t_IncomingTransactions` to History on 21-Nov, between 11:30:58 (unpay) and ~12:59 (generation).

### Round 3 results (25-Nov)

**A1: cheque 211 History row, columns labelled.**
- `Date` = 21-Nov, `Createdon` = **20-Nov**, `ClearedDate` = 21-Nov, `VerifiedOn` = 21-Nov.
- `SupervisedByOne` = MB0194 at **21-Nov 11:30:58.307**.
- `IsProcessed` = 1, `Paid` = 1, `Returned` = 1, `ReturnedTempID` = 50.
- `TrxID` = `doc-001-495-0043421`, `MsgID` = `QNF2611201243410000002379491`.
- There is no "moved-to-History at" column. `Date` was rolled 20 → 21-Nov, so unprocessed inwards seem to be **carried forward** in the live table to the next day, not moved.

**A3: unpays since June. Next-day unpays are NORMAL: 89 of 111.** Almost all of them are in History, so the generator presumably succeeded for them. So "next-day → inward already in History → blank TrxID" **cannot on its own** explain why cheque 211 failed. The live table must normally still hold the item when the unpaid file is generated. Something moved cheque 211 out **between 11:30 and ~12:59 on that day**.

**A2: precise movers.** Only three objects **INSERT into History**:

| Object | Notes |
|---|---|
| `ps_UpdateGLBalance` | EOD. Also DELETES-LIVE. |
| **`Proc_CreateXMLFiles_Tanzania_CTS`** | The **non-selected** file generator, 2021 version. **INSERTS-HISTORY and DELETES-LIVE.** `TanzaniaFiles.vb` calls it whenever the user generates **without ticking items** (cheques, cheque returns, EFTs, DDs). It is a **daytime** job. |
| `rw_IncomingTransactions` | A report (inserts into History?). Check later. |

**New prime suspect:** a generation run **without selection** between 11:30 and 12:59 (e.g. outward cheques or EFTs, or an unpaid run with nothing ticked) used `Proc_CreateXMLFiles_Tanzania_CTS`. That moved processed inwards, including the just-unpaid cheque 211, into History. The 12:59 unpaid generation then found no TrxID.
- This is a **race condition**. It depends on the order of operator actions that day, which fits "rare, twice in November".
- Alternatively, the 12:59 unpaid generation itself ran without selection through that procedure. It might move rows before reading TrxID.

**Ruled out (texts saved):**
- `p_DeleteUnpostedInwardsTrx` deletes only **unprocessed/unverified** inwards; cheque 211 was processed.
- `sb_ClearEffects` / `ps_ClearEffects` only set `CEDate` (Clear-Effects date) and run the CLR sub-process.
- `pc_SystemBranchStatus` only reads.
- `p_DoInwardsFileTrxValidationsTZ` only updates and deletes locks.

**A4** returned no rows. **`p_AddClearingFileStatus.sql` was saved empty (0 bytes).** Re-save it.

**A5:** CEDate is 23-Nov for branches 000/001/003 and 24-Nov for the rest. Clear-effects is behind for 3 branches. Not related.

### DEPLOYED 25-Nov-2026: generator History fix (`sql/11_25_2026_Fixes`)
- **Pre-check:** ColumnID 4344 has no live row; History has `doc-001-495-0043421`. ✔
- **Index:** `t_IncomingTransactionsHistory` has only the clustered PK on `TrxRowID`. There is **no index on ColumnID**, so each History lookup is a table scan. It is only hit for unpaid items not found live (plus 2 cursor lookups). Check the table size. If it is large, suggest to the DBA a nonclustered index on (ColumnID) INCLUDE (ChequeID, TrxID, MsgID, [Date]).
- **Applied:**
  - `Proc_CreateXMLFiles_Tanzania_Selected_CTS_25112026` (backup) created 16:38:34.
  - `Proc_CreateXMLFiles_Tanzania_Selected_CTS` altered 16:38:57.
  - FixPresent = 14489 (the tag was found). ✔
- **Pending:** a functional test (a rolled-back run for TrxRowID 80031940 with a login that can read BRNET_IMAGEServer, or watch the next unpaid item whose inward was loaded on an earlier day).

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
| `BANK_COMMUNICATION.md` | Plain-language explanation for the bank, plus the way forward for the settled cheque |
| **`00_DIAGNOSIS_RUN_IN_ORDER.sql`** | **Start here.** Steps 1–9 in order, plus the files to collect. Supersedes the order of 01/02. |
| `01_READONLY_scripts_for_bank.sql` | Broad evidence: inward, extra details, DRUNP files, current procedure text, recently changed objects, plus the files to collect |
| `02_READONLY_why_OrgnlTxId_was_blank.sql` | Targeted to cheque 211, using the IDs from the bank's screenshot |
| `DRAFT_FIX_on_OLD_copy_...sql` | Do not deploy. Written against the old procedure copy. |

Originals before the fix: see git history and `my_stuff/sql/`.

## 26-Nov-2026: new report, cheque TZS 5,120,000

### The report
- Unpaid on 25-Nov with reason "irregular account name" (CH11), not AM04. On 26-Nov BOT still showed it as "accepted, ready for settlement" (settles ~13:00).
- The item has left our verification queue, so the officer would have to debit the customer manually.
- The bank sees our .Q file but no BOT response for it.

### Production file locations (Maendeleo clearing server, E:)
| What | Path |
|---|---|
| Install / working folder | `E:\Clearing_Production` |
| Signed outgoing returns, archived by date | `E:\Clearing_Production\Prod_Files\ToTach\Archive\Out\<yyMMdd>\DRUNP….Q` |
| Gateway pick-up archive (proves the gateway took the file) | `E:\Clearing_Production\Prod_Files\ToTach\Outgoing\inputarchive\<yyyy-MM-dd>\DRUNP….Q` |
| Unsigned copies | `<OutgoingFiles>\Temp` and `\Files`. **Deleted** by `Clear_Files_Arc` after every successful signing, so none are kept. |
| Windows user on the server | `smyaka`. There is no `C:\Users\smyaka\Desktop`, so write output to `$env:TEMP`. |

- The config the user pasted on 26-Nov is **not** production. It is our dev App.config: `C:\GeneratedDemoTestFiles…`, `Sign=false`, `configFilePath` on Alex's Desktop. The real one is the `.exe.config` beside the tool's exe on the clearing server. Its `OutgoingFiles` setting is probably `E:\Clearing_Production\Prod_Files\ToTach`. Do not copy any credentials into the notes.
- File naming: `DRUNP` + payee BIC + ddMMyyyy + time. The time part is from the old `HHmm`+counter logic, so `1038` means about 10:37.

### How to read a signed .Q (steps that worked, PowerShell 5.1 on the server)
The .Q is CMS **signed** (`.signed.CMS` from BRCS), not encrypted. No key is needed.
```powershell
Add-Type -AssemblyName System.Security; $f='E:\Clearing_Production\Prod_Files\ToTach\Archive\Out\261125\DRUNPCORUTZTZ251120261038.Q'; $b=[IO.File]::ReadAllBytes($f); try { $b=[Convert]::FromBase64String(([Text.Encoding]::ASCII.GetString($b) -replace '-----[^-]+-----','' -replace '\s','')) } catch {}; $c=New-Object System.Security.Cryptography.Pkcs.SignedCms; $c.Decode($b)
[Text.Encoding]::UTF8.GetString($c.ContentInfo.Content) | Out-File "$env:TEMP\DRUNPCORUTZTZ251120261038.xml" -Encoding utf8; notepad "$env:TEMP\DRUNPCORUTZTZ251120261038.xml"
```
- Find every file for a MsgId: `Get-ChildItem 'E:\Clearing_Production\Prod_Files' -Recurse -File | Where-Object { $_.Name -like '*251120261038*' }`
- Scan the whole archive (count TxInf, OrgnlTxId, unique ids and settlement date per file): see `evidence/archive_scan_26Nov2026.txt`.

### Decoded DRUNPCORUTZTZ251120261038.Q (built 25-Nov 10:37)
- OrgnlMsgId `QNF2611241504550000002380301`: the inward came in on 24-Nov at 15:04.
- OrgnlTxId `6C3839A503BB4399B850BAA409CA15A9`, EndToEnd `970071b6…` and reason `CH11` are all **present**.
- Debtor: JERUSALEM MBEZI BEACH SCHOOL PROJECT, account 3787826027. Payee: EMANUEL WILBAD NKYA at CRDB (CORUTZTZ), account 0152612105900.
- The same TxInfAndSts appears twice.
- **`IntrBkSttlmDt` = `0001-01-01`**, an empty date.
- The gateway took it: there is a copy in `Outgoing\inputarchive\2026-11-25`. No file named after the MsgId came back from BOT.

### Archive scan, Dec-2025 to 25-Nov-2026 (`evidence/archive_scan_26Nov2026.txt`)
- **Duplicates are not the cause.** Every unpaid-return file since Dec-2025 has each transaction twice (TxInf=2, Unique=1; some have 3), and BOT has accepted them all year. Withdrawn as a cause. Clean-up only.
- **Settlement date is the anomaly.** Since 06-Mar-2026, every file has `IntrBkSttlmDt` equal to the day it was built. That matches the code `txnInf.OrgnlTxRef.IntrBkStmDt = Date.Today` (`TanzaniaFiles.vb` ~2008, present since the first git upload on 25-Feb-2026). Only these files have `0001-01-01`:
  - two files on 28-Jan-2026, which also have no BIC and no OrgnlTxId;
  - two files on 12-Feb-2026;
  - **25-Nov-2026 DRUNPCORUTZTZ251120261038**.
  The current code cannot produce `0001-01-01`. **So the 25-Nov return was built by an OLDER build of the tool** (pre-Feb-2026 logic), not the exe that built every return from March to 23-Nov. The likely BOT rejection is the invalid or empty settlement date. Confirm with BOT's response.
  - To find out: which PC and user generated it at 10:37, which exe/DLL they ran (path, file version, date), and whether any DLL was replaced or rolled back around 24/25-Nov.
- **Cheque 211 files** (21-Nov, `DRUNPBARCTZTZ211120261137` and `…1260`) both have OrgnlTxId present and SttlmDt 2026-11-21. So the "missing OrgnlTxId" fault was **not** an absent element in what we sent. The value may be wrong instead: p_AddInwardsTZ strips `_` (15-Jun), so we store `doc-001-495-0043421` while BOT has `doc-001-495-004342_1`. BOT would not match it and would report the original transaction as missing. **Print the OrgnlTxId values of those files to confirm.**
- **27-Jul-2026 09:32 `DRUNPCORUTZTZ27072026933`**: TxInf=2, OrgnlTxId=0, a truly blank OrgnlTxId. It was re-generated at 12:50 the same day with the id present. This is an earlier case of the History-move fault (fixed 25-Nov).
- The ~1260 / ~1061 MsgIds (time over 59) confirm the Option Strict string+number bug in the MsgId.

### Corrections to earlier statements
- "The duplicate makes BOT reject": **wrong**, because duplicates have always been there.
- "Cheque 211's file had no OrgnlTxId": **not supported** by the archive. Suspect the `_`-stripped TrxID value instead.

### 26-Nov: decoded key fields + tool builds on the server
| File | OrgnlMsgId | OrgnlTxId | Rsn | Amt | SttlmDt |
|---|---|---|---|---|---|
| DRUNPBARCTZTZ211120261137 (21-Nov 11:36) | QNF2611201243410000002379491 | **doc-001-495-0043421** | AM04 | 5,323,100 | 2026-11-21 |
| DRUNPBARCTZTZ211120261260 (21-Nov 12:59, resend) | same | **doc-001-495-0043421** | AM04 | 5,323,100 | 2026-11-21 |
| DRUNPCORUTZTZ23112026933 (23-Nov, OK) | QNF2611201727530000002379561 | 982A5733… (GUID) | CH01 | 750,000 | 2026-11-23 |
| DRUNPCORUTZTZ251120261038 (25-Nov) | QNF2611241504550000002380301 | 6C3839A5… (GUID) | CH11 | 5,120,000 | **0001-01-01** |

**Cheque 211: root cause revised.**
- Both returns were built 6 minutes and 1.5 hours after the unpay, and both carried an OrgnlTxId. But the value was **`doc-001-495-0043421`**, while BOT's original TxId is **`doc-001-495-004342_1`**.
- The `_` was stripped at load by `p_AddInwardsTZ` line 97 (`REPLACE(@TrxID,'_','')`, bnyaswa 15-Jun-2026). BOT could not match the original transaction, so it reported "missing OrgnTxID".
- Only banks whose TxIds contain `_` are hit (ABSA/BARCTZTZ `doc-…_n`). Banks that send GUID TxIds are not affected, which is why it is rare.
- The History-move fault (fixed 25-Nov) is real: the 27-Jul file had a blank OrgnlTxId. But it was **not** what broke cheque 211.
- **Ask Brenda why `_` is stripped.** A fix must keep the original TxId for the return, either by not stripping it or by storing the original.

**Cheque 5,120,000 (25-Nov): wrong tool build suspected.**
- Tool folders under `E:\Clearing_Production`:
  - `Debug 3_4_2026` (04-Mar-2026, newest; matches git 87cb9ac 2026-03-04);
  - `Debug 3_4_2026\Debug 2_18_2026` (18-Feb);
  - `…\Confused` and `Debug\Debug\Confused` (17-Feb).
  - Exe `BRNETUploadDownload.exe` 1.0.0.3 in all of them; the DLLs are version 1.0.0.0, so the version number does not help.
- Every return from 06-Mar to 23-Nov has SttlmDt = the build day, which is the 04-Mar build. The 25-Nov file has 0001-01-01, the same as the 28-Jan and 12-Feb files. So it was most likely generated with an **older build** (one of the Feb folders) or on another PC with an old copy.
- Next: find which exe is running or shortcut-linked and who ran it at 10:37 on 25-Nov (processes, .lnk targets, all copies of the exe on C:/E:, other PCs).

### 26-Nov: which tool is actually running (clearing PC = the server on AnyDesk, user smyaka)
```
E:\Clearing_Production\Debug 3_4_2026\BRNETUploadDownload.exe   | started 23-Nov 11:48 | smyaka
E:\Clearing_Production\Prod_FileGenTool\BRNETUploadDownload.exe | started 24-Nov 08:55 | smyaka
E:\Clearing_Production\Prod_FileGenTool\BRNETUploadDownload.exe | started 25-Nov 08:28 | smyaka
E:\Clearing_Production\Prod_FileGenTool\BRNETUploadDownload.exe | started 26-Nov 08:34 | smyaka
```
- **Four instances are open at once, from two different folders.** On 24–26 Nov the operator started the tool from **`Prod_FileGenTool`**, not from `Debug 3_4_2026`.
- `Prod_FileGenTool` did not appear in the newest-30 exe/dll listing, so its binaries are dated **before 17-Feb-2026**. That makes it the prime suspect for the 0001-01-01 settlement date in the 25-Nov return. The bank believes they run the March build. Confirm by comparing hashes of `Prod_FileGenTool` against `Debug 3_4_2026`.
- The shortcut scan failed (Access denied on `C:\Users\*`).

### Why `_` is stripped (p_AddInwardsTZ line 97, bnyaswa 15-Jun): unknown, Brenda unavailable
- Inward cheques: `chq.trxID = c.PmtId.TxId` (TanzaniaFiles.vb ~4046). The stripped value goes to `t_IncomingTransactions.TrxID` **and** `t_IncomingClrTrxExtraDetails.TrxID` (insert ~1207). The original TxId is **not stored anywhere**.
- For returns of our own cheques (TrxType ID), the procedure does `CAST(@TrxID AS BIGINT)` against `t_AccountTrxClearing.TrxRowID` (~326/346, inside TRY). A `_` would make that CAST fail, so this is a plausible reason for the strip. Not confirmed.
- Fix options that keep her change:
  - A. Strip only when the result is numeric (her case). Leave IDs like `doc-001-495-004342_1` untouched.
  - B. Keep stripping, but save the original TxId in a new nullable column and use it in the generator's OrgnlTxId.

### 26-Nov: hash check, Prod_FileGenTool vs Debug 3_4_2026: CONFIRMED old build
- In `Prod_FileGenTool`, `BrClearing.dll`, `BRClrLib.dll`, `BRClearingUtil.dll`, `BRcatDS.dll`, `BRRTGSProcessing.dll` and `BRNETUploadDownload.exe` are dated **22-Dec-2025** and differ (`same=False`) from the 04-Mar-2026 build. `ImageFileRead.dll` and `BRClearingEncryptDecrypt.dll` also differ.
- The `.exe.config` files are identical in both folders, so both copies use the same DB and folders.
- Conclusion: the 25-Nov return (settlement date 0001-01-01) was built by the Dec-2025 build in `Prod_FileGenTool`. The returns from Mar to 23-Nov came from the March build.
- Still open: why the operator switched to `Prod_FileGenTool` on 24-Nov (a new shortcut?).
- Decision (Alex): go with option A for the `_` strip. Scripts are in `my_stuff/sql/11_26_2026_Fixes/`: the backup, the CREATE OR ALTER fix, and the reopen script for the 5,120,000 cheque (ROLLBACK by default).

### 26-Nov: BOT responses FOUND
- BOT's response files land in `E:\Clearing_Production\Prod_Files\FromTach\Incoming\`. After loading they are copied to `…\Incoming\Backup\<yyyyMMdd_HHmmss>\MBTLTZTZ<yyMMddHHmmss><seq>.<V|R|RC>`.
- Prod config (`Debug 3_4_2026\BRNETUploadDownload.exe.config`):
  - `Archive` = `E:\Clearing_Production\Prod_Files\ToTach\Archive`
  - `ClearingErrorLogFilePath` = `E:\Clearing_Production\Prod_Files\Logs\`
  - `ErrorLogPath` = `C:\BRExceptionLog\`
  - `IncomingFiles` = `C:\temp\Incoming` and `IncomingSignedFiles` = `C:\GeneratedDemoTestFiles\Incoming` (look like leftover dev values)
  - there is no `Responses` key
- Responses that mention our returns:
  - `Backup\20261121_125955\MBTLTZTZ261121125914984.V` → for `DRUNPBARCTZTZ211120261260` (cheque 211 resend)
  - `Backup\20261125_103728\MBTLTZTZ261125103652048.V` → for `DRUNPCORUTZTZ251120261038` (5,120,000). It came back within seconds of sending and was loaded at 10:37:28, so it looks like an immediate validation response.
- To do: read both .V files (status and reason), and find the response for `DRUNPBARCTZTZ211120261137`.

### 26-Nov: BOT's own words, BOTH ROOT CAUSES CONFIRMED
**Cheque 211** (`MBTLTZTZ261121125914984.V`, the response to `DRUNPBARCTZTZ211120261260`):
- `GrpSts RJCT B09` "TACH: All items rejected".
- Per item: `TxSts RJCT`, `Rsn MD03` "**TACH: Missing original transaction OrgnlTxId**".
- BOT could not find any original transaction with the OrgnlTxId we sent, `doc-001-495-0043421`, because its id is `doc-001-495-004342_1`.
- Cause: the `_` stripped in p_AddInwardsTZ (bnyaswa 15-Jun). Fix: `11_26_2026_Fixes/p_AddInwardsTZ.sql` (option A).
- BOT also replied to the first send (11:36) with `Backup\20261121_113723\MBTLTZTZ261121113630983.V`, presumably the same reason.

**Cheque 5,120,000** (`MBTLTZTZ261125103652048.V`, the response to `DRUNPCORUTZTZ251120261038`):
- `GrpSts RJCT B09`.
- Per item: `Rsn DT01` "**TACH: Invalid date Item invalid because original bulk settlement date: 20261125 not between 00011226 and 00010104**". BOT built the allowed window from the `0001-01-01` we sent.
- Cause: the return was generated with the Dec-2025 build in `Prod_FileGenTool`, which does not set IntrBkSttlmDt. Fix: use only the March build (`Debug 3_4_2026`) and retire the old folder.

**Common gap:** BOT's rejection came back within seconds both times, and the tool loaded it (Incoming\Backup). Nobody was alerted, and the item stayed marked generated. Improvement: alert on `RJCT` and reopen the item. That needs an app change.

**BOT response files:** `E:\Clearing_Production\Prod_Files\FromTach\Incoming\Backup\<yyyyMMdd_HHmmss>\MBTLTZTZ….V`. They are plain XML followed by a `|<base64 signature>` tail. BOT echoes our StsId (`yyyyMMdd_<RemittanceInfo>`) in its `OrgnlTxId` field.

### 26-Nov: p_AddInwardsTZ option A DEPLOYED on BRNET_MB_LIVE
- Pre-check:
  - `p_AddInwardsTZ` was last modified 2026-07-13 11:04 (the bnyaswa auto-unpay change), so it is the same as the 25-Nov saved text. Brenda's own backup `p_AddInwardsTZ_1206` exists (created 12-Jun, modified 15-Jun).
  - Compatibility level is 150.
  - TRY_CAST test: `doc-001-495-004342_1` is kept; `80031940_1` becomes `800319401`; a GUID is unchanged.
- Backup `p_AddInwardsTZ_26112026` created 11:43:19. Fix altered 11:44:06. `FixPresent` = 3075, `BackupHasFix` = 0.
- The active shortcut on the clearing PC points to `E:\Clearing_Production\Debug 3_4_2026\BRNETUploadDownload.exe` (the March build). So the `Prod_FileGenTool` copies opened on 24–26 Nov were started some other way (directly from the folder, or a pinned or recent entry).
- Still open:
  - stop the 4 running processes and rename the old exe;
  - reopen script for the 5,120,000 cheque, only if BOT agrees;
  - app alert on BOT RJCT;
  - bank messages.
- 26-Nov: stopped all 4 tool processes (17572, 15568, 13888, 22140); none are left running. Renamed `E:\Clearing_Production\Prod_FileGenTool\BRNETUploadDownload.exe` to `BRNETUploadDownload.exe.OLD_Dec2025`. The tool must now be opened only from `Debug 3_4_2026`, which is where the shortcut points. To undo, rename it back.
- 26-Nov ~12:00: ran the reopen script dry with ROLLBACK. Before: `t_AccountTrxClearing` TrxRowID **80124011**, OC, CH11, 5,120,000.00, IsGenerated 1, SessNo 1, Reference `DRUNPCORUTZTZ251120261038`. Rows updated: t_TrxClearing 0, t_AccountTrxClearing 1. After (inside the tran): IsGenerated 0, Reference NULL. Rolled back. The next step is to run it with COMMIT and regenerate.
- 26-Nov ~12:10: the bank spoke to its customer and decided **not** to re-send the return for the 5,120,000 cheque. The reopen was **not** committed (only the ROLLBACK dry run was run), so TrxRowID 80124011 stays IsGenerated 1. The bank will handle the settled amount itself.
- To verify the fixes on real traffic: `04_VERIFY_fixes_after_26Nov.sql` (DB side) and the archive scan for new DRUNP files (file side). See the next unpaid return and the next ABSA inward.
- 26-Nov 12:24: the tool was reopened. Exactly one instance is running: `Debug 3_4_2026\BRNETUploadDownload.exe` (PID 23732), the March build. Decision: wait for the next real unpaid cheque to confirm the fix end to end (BOT .V must show ACCP).
- 26-Nov 12:31: there are two desktop shortcuts. 'BRNETUploadDownload - 3_4_2026' points to `Debug 3_4_2026` (March, correct). **'Tach Gen Prod Tool'** points to `E:\Clearing_Production\Debug\Debug\BRNETUploadDownload.exe`, another copy whose build is not yet checked. It must be repointed or disabled.
- 26-Nov 12:35: copies of the tool on E:\Clearing_Production. Only **`Debug 3_4_2026`** matches the March build (hash equal, and it contains the 87cb9ac change). Every other runnable copy is older:
  - `E:\Clearing_Production\BRNETUploadDownload.exe` (BrClearing 12-Feb-2026)
  - `Debug\Debug` (17-Feb; the "Tach Gen Prod Tool" shortcut points here) and `Debug\Debug\Confused` (17-Feb)
  - `Debug 3_4_2026\Confused` (17-Feb), `Debug 3_4_2026\Debug 2_18_2026` (18-Feb), `…\Debug 2_18_2026\Confused` (17-Feb)
  - `Maendeleo Generation Tool\…\CBT tool` and `…\CBT tool\copy` (22-Dec-2025)
  - `Prod_FileGenTool\copy` (22-Dec-2025); the `Prod_FileGenTool` exe itself is already renamed .OLD_Dec2025
  - `Utility\Utility` (03-Nov-2025)
  - `wetransfer_maendeleo-tool_2026-01-07_0638\Maendeleo Tool` (07-Jan-2026)
- Production commit check: `Debug 3_4_2026\BrClearing.dll` = git 87cb9ac (04-Mar-2026).
- 26-Nov ~12:40: desktop shortcuts for smyaka (`C:\Users\smyaka\OneDrive - maendeleo bank\Desktop`), before → after:
  - **`BackUp Utility Tool.lnk` → `Prod_FileGenTool` (Dec-2025)**. This is the most likely way the old build was opened on 24–26 Nov.
  - `Tach Gen Prod Tool.lnk` → `Debug\Debug` (17-Feb)
  - `BRNETUploadDownload - Shortcut.lnk` → `Y:\BRNETUploadDownload.exe` (a network drive, build not checked)
  - `BRNETUploadDownload - 3_4_2026.lnk` → `Debug 3_4_2026` (already correct)
  All four now point to `E:\Clearing_Production\Debug 3_4_2026\BRNETUploadDownload.exe`. Still open: check the Y:\ copy's build, which other PCs may use.
- 26-Nov: `Y:\` is not reachable from the clearing server, so that shortcut was already dead there. Still to ask the bank: does any other PC generate clearing files?
- **Status, 26-Nov ~12:45:** both DB fixes are live; only the March build can be opened from this PC; all shortcuts are repointed. Still pending:
  - end-to-end confirmation on the next real unpay (BOT .V must show ACCP);
  - the rolled-back test of p_AddInwardsTZ;
  - whether the other old copies can be disabled;
  - the app alert on RJCT;
  - a git commit.
- 26-Nov 12:51: deleted the extra desktop shortcuts (they went to the Recycle Bin). Only **'Tach Gen Prod Tool'** is left, pointing to `E:\Clearing_Production\Debug 3_4_2026\BRNETUploadDownload.exe` with Start in `Debug 3_4_2026`. Taskbar and Start-menu shortcuts were not checked.
