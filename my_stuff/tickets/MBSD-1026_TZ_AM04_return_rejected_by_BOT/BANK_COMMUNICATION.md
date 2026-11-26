# MBSD-1026: communication to the bank (plain language)

## What happened and what we fixed
When a cheque is unpaid, the system sends a return message to BOT that must include the cheque's original reference number. To find that number, the system only looked in its list of cheques still being worked on. If the cheque had already been moved from that list into the archive, the reference could not be found, so the return went to BOT without it. BOT rejected the return, and once the cut-off passed the cheque was treated as paid. This only happened when the cheque was archived before the return was created, which is why it happened only occasionally.

We have fixed this so the system now also looks in the archive. The original reference is always found and included in the return to BOT. We have also tested the fix against the affected cheque, and its reference is now found correctly.

## Technical summary of the change
The procedure `Proc_CreateXMLFiles_Tanzania_Selected_CTS`, which builds the unpaid cheque return file sent to BOT, has been changed. It used to look up the cheque's original transaction reference (`TrxID`, sent as `OrgnlTxId`) only in `t_IncomingTransactions`. Now, if the reference isn't in that table, it also looks it up in `t_IncomingTransactionsHistory`. The return file therefore always carries the original reference, even when the inward cheque has already been archived.

## Way forward for the cheque that already settled
The system cannot reverse a cheque once BOT has settled it, so recovering the funds is a bank-to-bank and customer matter. In practice:

1. **The current position.** The drawer's account was debited and then credited back when the cheque was unpaid. The settlement to the presenting bank still went through. The amount is therefore sitting as an open item in the bank's clearing/suspense account, not with the customer. Finance should confirm and reconcile this.
2. **Recovery from the presenting bank (ABSA).** The bank can raise a claim or recall with ABSA through the BOT TACH dispute or bilateral process, asking for the funds back from their customer because the cheque was unpaid for insufficient funds.
3. **Alternatively, recovery from the drawer.** The cheque was in effect honoured, so the bank may decide to re-debit the drawer's account (Safe Hand Company Limited) under its own policy. This will overdraw the account. It is a credit, legal and customer-relationship decision for the bank.
4. **Our support.** We will provide the evidence for any claim: the unpaid time, the return file sent, BOT's rejection response, and an explanation of the fault and fix.

Which route to take is the bank's decision (operations, finance and legal). We recommend starting the claim with ABSA/BOT as early as possible.

---

## Update: confirmed causes from BOT's responses (replaces the explanation above)
We have now read BOT's rejection messages for both cheques. There were two different causes.

**Cheque of TZS 5,323,100 (Safe Hand Company, ABSA).** BOT rejected the return with "Missing original transaction OrgnlTxId". The return did carry the cheque's reference, but in a slightly altered form: one character (an underscore) had been removed when the cheque was first received. Because of this, BOT could not match the return to the original cheque. This only affects banks whose references contain that character, such as ABSA, which is why it happens rarely.
*Fix:* the system now keeps the reference exactly as BOT sent it.

**Cheque of TZS 5,120,000 (25-Nov, "irregular account name").** BOT rejected the return with "Invalid date". The return was created from an older copy of the clearing tool that is still on the clearing PC, and that copy left the settlement date empty.
*Fix:* the older copy has been retired. Only the current version will be used.

**In both cases** BOT's rejection came back within seconds and was received by the system, but nobody was alerted. We will add a warning so that a rejected return is flagged straight away and can be corrected and re-sent before the cut-off.

### Technical summary
- `p_AddInwardsTZ`: the `_` is now removed from the inward TxId only when the result is numeric, so other ids are kept as sent (`--alex.ndegwa 26112026`). The 25-Nov change to `Proc_CreateXMLFiles_Tanzania_Selected_CTS` (History lookup) stays as an extra safeguard.
- The tool is run only from `E:\Clearing_Production\Debug 3_4_2026` (the March-2026 build). `E:\Clearing_Production\Prod_FileGenTool` holds the Dec-2025 build and is retired.

---

## Chat messages for the 5,120,000 cheque (separate messages)

**1. Why BOT rejected the file**
> Yes, we have found BOT's response. BOT rejected the unpaid return within a few seconds of receiving it, with the reason "Invalid date". The return was created from an older copy of the clearing tool that was still open on the clearing PC. That copy left the settlement date empty, so BOT could not accept it. Your unpay reason was correct and was not the problem.

**2. What we have done**
> We have closed the older copy and disabled it, so the tool can now only be opened in the current version. We have also fixed a separate issue that caused the earlier ABSA cheque return to be rejected. Future unpaid returns will go out correctly.

**3. What happens now / next steps before 1 PM**
> To stop this cheque from settling, a corrected return has to reach BOT before the cut-off. Please:
> 1. Call BOT now and ask if they can still accept an unpaid return for this cheque today.
> 2. If they agree, let us know right away. We will put the cheque back in the unpaid list, and you can generate the return again from the tool.
> 3. After sending, check that BOT's response is accepted, not rejected. We can check this with you.
>
> If BOT says it is too late, the cheque will settle, and the amount will have to be recovered from the presenting bank (CRDB) or the customer through the bank's normal process.
