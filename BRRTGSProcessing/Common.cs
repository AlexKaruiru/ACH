using System;
using System.Collections.Generic;

namespace BRRTGSProcessing
{
    public class Common
    {
        public enum CurrencyType
        {
            Etb = 1,
            Usd = 2,
            Gbp = 3,
            Eur = 4,
            Jpy = 5
        }
        public enum RtgsType
        {
            Mt103 = 1,
            Mt202 = 2,
            Mt900 = 3,
            Mt910 = 4,
            Mt920 = 5,
            Mt941 = 6,
            Mt950 = 7,
            Mt999 = 8,
            Mt198 = 9,
            Mt298 = 10,
            Mt103Cpo = 11,
            Mt103Cheque = 12,
            Mt108 = 13,
            Mt204 = 14,
            Mt205 = 15,

        }
        public enum CashMsgType
        {
            FreeFormat = 1,
            BalanceRequest = 2
        }
        public enum ResponseType
        {
            ChequeRejection = 1,
            DebitRefusal = 2,
            Validation = 3,
            Notification = 4
        }
        public class HeaderInfo
        {
            public string MsgId { get; set; }
            public DateTime CreateTime { get; set; }
            public int TxnCount { get; set; }
            public CurrencyType Currency { get; set; }
            public decimal TotalAmount { get; set; }
            public DateTime SettlementDate { get; set; }
            public string BankBic { get; set; }
        }

        public class CancelInfo : HeaderInfo
        {
            public string RemmiterBic { get; set; }
            public string OrgMsgId { get; set; }

        }
        public class ResponseHeader : HeaderInfo
        {
            public string OrgMsgId { get; set; }
            public string OrgNameSpace { get; set; }
            public string StatusType { get; set; }
            public string OrgCollectiondate { get; set; }
        }
        public class AchTransaction
        {
            public string Trans_Ref { get; set; }
            public string TxnId { get; set; }
            public string EndToEndId { get; set; }
            public CurrencyType Currency { get; set; }
            public decimal Amount { get; set; }
            public string BeneficiaryAcc { get; set; }
            public string BeneficiaryName { get; set; }
            public string BeneficiaryBic { get; set; }
            public string RemitterAcc { get; set; }
            public string RemitterName { get; set; }
            public string RemitterBic { get; set; }
            public string AdditionalInfo { get; set; }
            public string Filename { get; set; }
            public string FileId { get; set; }
            public int UserId { get; set; }
            public bool Status { get; set; }
            public string BeneficiaryBranchName { get; set; }
            public string BeneficiaryBankName { get; set; }
            public DateTime CollectionDate { get; set; }
            public int? ProcessingBranchId { get; set; }
        }
        public class Response
        {
            public ResponseHeader Header { get; set; }
            public ResponseType ResponseType { get; set; }
            public string StatusType { get; set; }
            public List<Rejection> Rejections { get; set; }
        }
        public class AchRtgs : AchTransaction
        {
            public RtgsType RtgsType { get; set; }
            public string BeneficiaryBranch { get; set; }
            public string RemitterBranch { get; set; }
        }
        public class AchCashMsg : AchTransaction
        {
            public string ReferenceNo { get; set; }
            public RtgsType RtgsType { get; set; }
            public int MsgType { get; set; }
            public decimal TransId { get; set; }
        }

        public class AchDebit : AchTransaction
        {
            public string MandateId { get; set; }
            public DateTime SignDate { get; set; }
            public bool Ammended { get; set; }
            public string SchemeId { get; set; }
            public string ReturnCode { get; set; }
        }
        public class Rejection : AchDebit
        {
            public string OrgInstructionId { get; set; }
            public string OrgTxnId { get; set; }
            public string OrgEndToEndId { get; set; }
            public string OrgMsgId { get; set; }


        }
    }
}
