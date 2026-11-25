# my_stuff

Working area for ACH clearing support. SQL under `sql/` is a copy of what is deployed (sp_helptext dumps). Do not edit it. Put fixes in the ticket folder.

| Folder | What |
|---|---|
| `docs/` | Implementation notes (currency mismatch, certificates/signing) |
| `sql/tables`, `sql/views`, `sql/functions` | Schema scripts |
| `sql/procedures/inward_and_unpay` | Loading inward items, pay/unpay (`p_AddInwardsTZ`, `p_ProcessIncomingTxns`, `p_AutoUnpayIncomingTxns`, ...) |
| `sql/procedures/outward` | Outward cheques/EFTs |
| `sql/procedures/xml_generation` | `Proc_CreateXMLFiles_*`: data for the ISO 20022 files sent to ACH |
| `sql/procedures/status` | ACH status/response handling |
| `sql/procedures/supervision` | Supervise/reject screens |
| `sql/procedures/posting_and_eod` | Posting, charges, GL balance, end of day |
| `sql/old` | Older versions kept for reference |
| `tickets/<name>` | One folder per support ticket: notes, read-only scripts for the bank, fixes |
