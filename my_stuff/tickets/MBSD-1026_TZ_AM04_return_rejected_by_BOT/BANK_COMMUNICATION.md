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
