using BRClrLib;
using BRNetSecurity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static BRRTGSProcessing.Common;
using static System.Net.Mime.MediaTypeNames;
using rs = BRRTGSProcessing.Response;
namespace BRRTGSProcessing
{
    public class ETRTGSProcessing: ETSys
    {
        private readonly string _location;
        private DateTime _procDate;
        private readonly bool _signFiles;
        private readonly string _certName = "";
        private readonly string _temp;
        private static readonly Regex Rx = new Regex("[^A-Za-z0-9 ]");
        private DataSet pubDataSet;
        private SqlConnection conn;
        private DataSet publicDset;
        private SqlCommand pubDataSqlCommand;
        private SqlDataAdapter pubDataSqlAdapter;
        private SqlConnection pubDbSqlConn;
        private DataTable publicDTbl;
        private readonly string OurBankBic = "";
        private readonly string HQBranch = "";
        private BRClr brclr = new BRClr();
        private protected string DBServerName = ConfigurationManager.AppSettings["DBServerName"];
        private protected string DatabaseName = ConfigurationManager.AppSettings["DatabaseName"];
        private protected string BRUserName = ConfigurationManager.AppSettings["BRUserName"];
        private protected string BRUserPassword = ConfigurationManager.AppSettings["BRUserPassword"];


        private enum dataExecTypes
        {

            ExecTypeQuery = 0,

            ExecTypeNonQuery = 1,
        }
        private enum queryType
        {

            SelectStatement = 0,

            StoredProcedure = 1,
        }
        // M = Location, N = SignFiles, O = CertName, P = Temp, Q = ProcDate, R = OurBankBic, S = HQBranch, T = details, U = Action
        public ETRTGSProcessing(string M, bool N, string O, string P, DateTime Q, string R, string S)
        {
            _location = M;
            _procDate = Q;
            _signFiles = N;
            _certName = O;
            _temp = P;
            OurBankBic = R;
            HQBranch = S;
        }

        /// <summary>
        /// Generate BR RTGS Files
        /// </summary>
        public bool BRRSFiles(DataRow T, string U)
        {
            bool ProcessingResponse = false;
            ProcessingResponse = BRRTGSFileProcessing(T, U);
            return ProcessingResponse;
        }
        public bool BRRSFiles(string U)
        {
            bool Response = false;
            string strAction = string.Empty;
            DataTable dataTable = null;
            DataSet dataSet = null;
            string HQBranchID = ConfigurationManager.AppSettings["HQBranchID"];
            DateTime Procdate = Convert.ToDateTime(GetScalarREC("SELECT dbo.f_GetWorkingDate('" + HQBranchID + "')",true));

            ExecuteData(brclr.GetModify("[p_getETRTGSMessagesToSend]", "FromDate", _procDate, "ToDate", _procDate, "FileType", U), ref publicDTbl,  queryType.StoredProcedure, dataExecTypes.ExecTypeQuery);
            if (publicDTbl.Rows.Count > 0)
            {
                foreach (DataRow datar in publicDTbl.Rows)
                {
                    try
                    {
                        Response = BRRSFiles(datar, datar["MessageType"].ToString());
                        if (Response)
                        {
                            strAction = "UPDATE t_swiftOutGoingMessages SET SentToSwift = 1, Processed = 1 WHERE Trans_Ref = '" + datar["OriginalTrans_Ref"].ToString() + "' AND MessageType = '" + datar["MessageType"].ToString() + "'";
                            ExecuteData(strAction, ref dataTable, queryType.SelectStatement, dataExecTypes.ExecTypeNonQuery);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    continue;
                }
            }
            return Response;
        }
        public AchRtgs BRRTGSFiles(string M)
        {
            AchRtgs rtgs = new AchRtgs();
            rtgs = ReadRtgs(M, "", "", "");
            return rtgs;
        }
        private protected bool BRRTGSFileProcessing(DataRow RtgsDt, string U)
        {
            bool ProcessingResponse = false;
            AchRtgs rtgs = new AchRtgs();
            var day = _procDate.ToString("yyMMdd");
            var tim = DateTime.Now.ToString("HHmmss");
            var FileName = RtgsDt["RemitterBic"].ToString().Trim().ToUpper() + "MT" + U.ToString().ToUpper() + day + RtgsDt["EndToEndId"].ToString().Trim();
            switch (U)
            {
                case "103":
                case "202":
                    if (RtgsDt["MessageType"].ToString() == "103")
                    {
                        rtgs.RtgsType = RtgsType.Mt103;
                    }
                    else if (RtgsDt["MessageType"].ToString() == "202")
                    {
                        rtgs.RtgsType = RtgsType.Mt202;
                    }
                    rtgs.Trans_Ref = RtgsDt["Trans_Ref"].ToString().Trim();
                    rtgs.TxnId = RtgsDt["OriginatorRef"].ToString().Trim();
                    rtgs.EndToEndId = RtgsDt["EndToEndId"].ToString().Trim();
                    switch (RtgsDt["CurrencyID"].ToString().Trim())
                    {
                        case "ETB":
                            rtgs.Currency = CurrencyType.Etb;
                            break;
                        case "USD":
                            rtgs.Currency = CurrencyType.Usd;
                            break;
                        case "GBP":
                            rtgs.Currency = CurrencyType.Gbp;
                            break;
                        case "EUR":
                            rtgs.Currency = CurrencyType.Eur;
                            break;
                        case "JPY":
                            rtgs.Currency = CurrencyType.Jpy;
                            break;
                    }
                    rtgs.Amount = Convert.ToDecimal(RtgsDt["Amount"]);
                    rtgs.RemitterAcc = RtgsDt["RemitterAcc"].ToString().Trim();
                    rtgs.RemitterName = RtgsDt["RemitterName"].ToString().Trim();
                    rtgs.RemitterBic = RtgsDt["RemitterBic"].ToString().Trim();
                    rtgs.RemitterBranch = RtgsDt["RemitterBranch"].ToString().Trim();
                    rtgs.BeneficiaryAcc = RtgsDt["BeneficiaryAcc"].ToString().Trim();
                    rtgs.BeneficiaryBic = RtgsDt["BeneficiaryBic"].ToString().Trim();
                    rtgs.BeneficiaryBranch = RtgsDt["BeneficiaryBranch"].ToString().Trim();
                    rtgs.BeneficiaryName = RtgsDt["BeneficiaryName"].ToString().Trim();
                    rtgs.BeneficiaryBranchName = RtgsDt["BeneficiaryBranchName"].ToString().Trim();
                    rtgs.AdditionalInfo = RtgsDt["AdditionalInfo"].ToString().Trim();
                    ProcessingResponse = CreateRtgs(rtgs,FileName);
                    break;
                case "920":
                case "999":
                    if (RtgsDt["MessageType"].ToString() == "920")
                    {
                        rtgs.RtgsType = RtgsType.Mt920;
                        rtgs.AdditionalInfo = RtgsDt["AdditionalInfo"].ToString().Trim();
                        if (rtgs.AdditionalInfo == "")
                        {
                            rtgs.AdditionalInfo = RtgsDt["NostroAccountId"].ToString().Trim();
                        }
                    }
                    else if (RtgsDt["MessageType"].ToString() == "999")
                    {
                        rtgs.RtgsType = RtgsType.Mt999;
                        rtgs.AdditionalInfo = RtgsDt["AdditionalInfo"].ToString().Trim();
                    }
                    rtgs.Trans_Ref = RtgsDt["Trans_Ref"].ToString().Trim();
                    rtgs.EndToEndId = RtgsDt["EndToEndId"].ToString().Trim();
                    rtgs.RemitterAcc = RtgsDt["RemitterAcc"].ToString().Trim();
                    rtgs.RemitterBic = RtgsDt["RemitterBic"].ToString().Trim();
                    rtgs.BeneficiaryAcc = RtgsDt["BeneficiaryAcc"].ToString().Trim();
                    rtgs.BeneficiaryBic = RtgsDt["BeneficiaryBic"].ToString().Trim();

                    ProcessingResponse = CreateRtgs(rtgs, FileName);

                    break;
                case "ImportResponses":


                    break;
                case "ImportRtgs":


                    break;
            }
            return ProcessingResponse;

        }
        private protected string GetScalarREC(string strStatementORstrProcedure, bool isFunction)
        {
            string strResults = string.Empty;
            DataTable publicDTbl = new DataTable();
            DataSet dataSet = new DataSet();
            try
            {
                if (isFunction)
                {
                    ExecuteData(strStatementORstrProcedure, ref publicDTbl,  queryType.SelectStatement, dataExecTypes.ExecTypeQuery);
                }
                else
                {
                    ExecuteData(brclr.GetModify(strStatementORstrProcedure), ref publicDTbl, queryType.SelectStatement, dataExecTypes.ExecTypeQuery);
                }
               
                if (publicDTbl.Rows.Count >= 1)
                {
                    strResults = publicDTbl.Rows[0][0].ToString();
                }
                else
                {
                    strResults = null;
                }
            }
            catch (Exception ex)
            {
                strResults = null;
            }

            return strResults;
        }

        /// <summary>
        /// Creates an RTGS transaction file using the MT formats
        /// </summary>
        /// <param name="rtgs">The RTGS object to be written</param>
        /// <param name="destFile">The file to which the transactions will be written</param>
        /// <returns>The Message Id assigned to the transaction</returns>
        private protected bool CreateRtgs(AchRtgs rtgs, string destFile)
        {
            try
            {
                var final = Path.Combine(_location, Path.GetFileName(destFile) + ".txt");
                var temp = Path.Combine(_temp, Path.GetFileName(final));
                string s;

                if (!Directory.Exists(_location))
                {
                    Directory.CreateDirectory(_location);
                }

                if (!Directory.Exists(_temp))
                {
                    Directory.CreateDirectory(_temp);
                }

                rtgs.Amount = decimal.Round(rtgs.Amount, 2);
                switch (rtgs.RtgsType)
                {
                    case RtgsType.Mt103:
                        s = CreateMt103(rtgs);
                        break;
                    case RtgsType.Mt103Cpo:
                        s = CreateMt103(rtgs);
                        break;
                    case RtgsType.Mt103Cheque:
                        s = CreateMt103(rtgs);
                        break;
                    case RtgsType.Mt202:
                        s = CreateMt202(rtgs);
                        break;
                    case RtgsType.Mt920:
                        s = CreateMt920(rtgs);
                        break;
                    case RtgsType.Mt999:
                        s = CreateMt999(rtgs);
                        break;
                    default:
                        s = "";
                        break;
                }
                Utility.WriteFile(temp, s);
                if (_signFiles)
                {
                    byte[] b = Utility.SignFiles(File.ReadAllBytes(temp), _certName);
                    File.WriteAllBytes(temp, b);
                }
                File.Move(temp, final);
                return true;

            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Creates Cash Messages
        /// </summary>
        /// <param name="CMsg">A list of Cash Items to be created</param>
        /// <param name="destFile">The file to which the transactions will be written</param>
        /// <returns>The Message Id assigned to the transaction</returns>
        private protected string CreateCashMessages(AchRtgs CMsg, string destFile)
        {
            var final = Path.Combine(_location, CMsg.RtgsType.ToString().Substring(0, 5).ToUpper() + Path.GetFileName(destFile) + ".txt");
            var temp = Path.Combine(_temp, Path.GetFileName(final));
            
            if (!Directory.Exists(_location))
            {
                Directory.CreateDirectory(_location);
            }

            if (!Directory.Exists(_temp))
            {
                Directory.CreateDirectory(_temp);
            }

            string s;
            CMsg.Amount = decimal.Round(CMsg.Amount, 2);
            switch (CMsg.RtgsType)
            {
                case RtgsType.Mt999:
                    s = CreateMt999(CMsg);
                    break;
                case RtgsType.Mt920://Balance                    
                    s = CreateMt920(CMsg);
                    break;
                default:
                    s = "";
                    break;
            }
            Utility.WriteFile(temp, s);
            if (_signFiles)
            {
                byte[] b = Utility.SignFiles(File.ReadAllBytes(temp), _certName);
                File.WriteAllBytes(temp, b);
            }
            File.Move(temp, final);
            return CMsg.Trans_Ref;
        }

        /// <summary>
        /// Imports RTGS transactions from the specified file
        /// </summary>
        /// <param name="sFile">The file containing the transactions to be imported</param>
        /// <param name="mt202Acc">The default ledger account to be used for MT202 transactions</param>
        /// <param name="mt900Acc">The default ledger account to be used for MT900 transactions</param>
        /// <param name="mt910Acc">The default ledger account to be used for MT910 transactions</param>
        /// <returns>An RTGS transaction imported</returns>
        private protected AchRtgs ImportRtgs(string sFile, string mt202Acc, string mt900Acc, string mt910Acc)
        {
            var rtgs = ReadRtgs(sFile, mt202Acc, mt900Acc, mt910Acc);
            return rtgs;
        }
        private DateTime DateConvertion(string strDate)
        {
            int year = 2000 + int.Parse(strDate.Substring(0, 2)); // Assuming "01" represents the year 2001
            int month = int.Parse(strDate.Substring(2, 2));
            int day = int.Parse(strDate.Substring(4, 2));

            DateTime dateTime = new DateTime(year, month, day);
            return dateTime;
        }
        /// <summary>
        /// Imports response transactions from the specified file
        /// </summary>
        /// <param name="sFile">The file containing the transactions to be imported</param>
        /// <returns>A list of response transactions imported</returns>
        private protected Common.Response ImportResponses(string sFile)
        {
            var res = new Common.Response();
            var doc = new rs.Document();
            Exception e;
            if (rs.Document.LoadFromFile(sFile, out doc, out e))
            {
                res.Header = new ResponseHeader
                {
                    MsgId = doc.FIToFIPmtStsRpt.GrpHdr.MsgId,
                    OrgMsgId = doc.FIToFIPmtStsRpt.OrgnlGrpInfAndSts.OrgnlMsgId,
                    OrgNameSpace = doc.FIToFIPmtStsRpt.OrgnlGrpInfAndSts.OrgnlMsgNmId,
                    StatusType = doc.FIToFIPmtStsRpt.OrgnlGrpInfAndSts.GrpSts.ToString(),
                    TxnCount = doc.FIToFIPmtStsRpt.TxInfAndSts.Count,
                };
                res.Rejections = doc.FIToFIPmtStsRpt.TxInfAndSts.Select(dr => new Rejection
                {
                    AdditionalInfo = dr.OrgnlTxRef.RmtInf == null ? "" : dr.OrgnlTxRef.RmtInf.Item.ToString(),
                    Ammended = dr.OrgnlTxRef.MndtRltdInf != null && dr.OrgnlTxRef.MndtRltdInf.AmdmntInd,
                    Amount = Convert.ToDecimal(dr.OrgnlTxRef.IntrBkSttlmAmt.Value),
                    BeneficiaryAcc = dr.OrgnlTxRef.CdtrAcct == null ? "" : dr.OrgnlTxRef.CdtrAcct.Id.Item,
                    BeneficiaryBic = dr.OrgnlTxRef.CdtrAgt == null ? "" : dr.OrgnlTxRef.CdtrAgt.FinInstnId.BIC,
                    BeneficiaryName = dr.OrgnlTxRef.Cdtr == null ? "" : dr.OrgnlTxRef.Cdtr.Nm,
                    CollectionDate = dr.OrgnlTxRef.ReqdColltnDt,
                    Currency = (CurrencyType)dr.OrgnlTxRef.IntrBkSttlmAmt.Ccy,
                    EndToEndId = dr.OrgnlEndToEndId,
                    FileId = doc.FIToFIPmtStsRpt.OrgnlGrpInfAndSts.OrgnlMsgId,
                    Filename = Path.GetFileName(sFile),
                    Trans_Ref = dr.OrgnlInstrId,
                    MandateId = dr.OrgnlTxRef.MndtRltdInf != null ? dr.OrgnlTxRef.MndtRltdInf.MndtId : "",
                    OrgEndToEndId = dr.OrgnlEndToEndId,
                    OrgInstructionId = dr.OrgnlInstrId,
                    OrgTxnId = dr.OrgnlTxId,
                    RemitterAcc = dr.OrgnlTxRef.DbtrAcct == null ? "" : dr.OrgnlTxRef.DbtrAcct.Id.Item,
                    RemitterBic = dr.OrgnlTxRef.DbtrAgt == null ? "" : dr.OrgnlTxRef.DbtrAgt.FinInstnId.BIC,
                    RemitterName = dr.OrgnlTxRef.Dbtr == null ? "" : dr.OrgnlTxRef.Dbtr.Nm,
                    ReturnCode = dr.StsRsnInf == null ? "0" : dr.StsRsnInf.Rsn.Item,
                    SchemeId = dr.OrgnlTxRef.CdtrSchmeId != null ? dr.OrgnlTxRef.CdtrSchmeId.Item.ToString() : "",
                    SignDate = dr.OrgnlTxRef.MndtRltdInf != null ? dr.OrgnlTxRef.MndtRltdInf.DtOfSgntr : DateTime.Now,
                    TxnId = dr.OrgnlTxId
                }).ToList();
            }
            return res;
        }

        private protected AchRtgs ImportRtgsResponses(string sFile, out bool archive)
        {
            var rt = new AchRtgs();
            archive = false;
            string[] sDet = File.ReadAllLines(sFile);
            var content = File.ReadAllText(sFile);
            var sGrp = content.Split('{');
            var sType = sGrp[2].Substring(3, 3);
            List<string> l = GetRtgsDetails(sDet);
            switch (sType)
            {
                case "198":
                    rt.Trans_Ref = l.First(p => p.StartsWith(":21:")).Remove(0, 4);
                    rt.RtgsType = RtgsType.Mt198;
                    archive = true;
                    if (!content.Contains(":16R:RSN"))
                        rt.Status = true;
                    break;
                case "298":
                    rt.Trans_Ref = l.First(p => p.StartsWith(":21:")).Remove(0, 4);
                    rt.RtgsType = RtgsType.Mt298;
                    archive = true;
                    if (!content.Contains(":16R:RSN"))
                        rt.Status = true;
                    break;
            }
            return rt;
        }

        private protected string CreateMt103(AchRtgs r)
        {
            var tm = DateTime.Now.ToString("HHmm");
            var dt = _procDate.ToString("yyMMdd");
            var id = r.EndToEndId;
            id = id.PadLeft(10, '0');
            id = id.Substring(id.Length - 10);
            string s = "{1:F01NBETETABAXXX" + id + "}{2:O103" + tm + dt + r.RemitterBic + "XXXX" + id + dt + tm + "N}{4:" + Environment.NewLine;
            s += ":20:" + r.Trans_Ref + Environment.NewLine;
            s += ":23B:CRED" + Environment.NewLine;
            s += ":23E:SDVA" + Environment.NewLine;
            s += ":32A:" + dt + r.Currency.ToString().ToUpper() + r.Amount.ToString("0.#0").Replace(".", ",") + Environment.NewLine;
            s += ":33B:" + r.Currency.ToString().ToUpper() + r.Amount.ToString("0.#0").Replace(".", ",") + Environment.NewLine;
            s += ":50K:/" + r.RemitterAcc + Environment.NewLine + TransformText(r.RemitterName, 35) + Environment.NewLine + r.RemitterBranch + Environment.NewLine;
            s += ":52A:" + r.RemitterBic + Environment.NewLine;
            s += ":57A:" + r.BeneficiaryBic + Environment.NewLine;
            s += ":59:/" + r.BeneficiaryAcc + Environment.NewLine + TransformText(r.BeneficiaryName, 35) + Environment.NewLine + r.BeneficiaryBranch + r.BeneficiaryBranchName + Environment.NewLine;
            s += ":71A:SHA" + Environment.NewLine;
            s += ":72:/" + TransformText(r.EndToEndId + " " + r.AdditionalInfo, 35) + Environment.NewLine + "-}";
            return s;
        }

        private protected string CreateMt202(AchRtgs r)
        {
            var tm = DateTime.Now.ToString("HHmm");
            var dt = _procDate.ToString("yyMMdd");
            var id = r.EndToEndId;
            id = id.PadLeft(10, '0');
            id = id.Substring(id.Length - 10);
            var s = "{1:F01NBETETABAXXX" + id + "}{2:O202" + tm + dt + r.RemitterBic + "XXXX" + id + dt + tm + "N}{4:" + Environment.NewLine;
            s += ":20:" + r.Trans_Ref + Environment.NewLine;
            s += ":21:" + r.TxnId + Environment.NewLine;
            s += ":32A:" + dt + r.Currency.ToString().ToUpper() + r.Amount.ToString("0.#0").Replace(".", ",") + Environment.NewLine;
            s += ":52A:" + r.RemitterBic + Environment.NewLine;
            s += ":58A:/" + r.BeneficiaryAcc + Environment.NewLine + r.BeneficiaryBic + Environment.NewLine;
            //s += ":59:/" + r.BeneficiaryAcc + Environment.NewLine;// + TransformText(r.BeneficiaryName, 35) + Environment.NewLine;
            s += ":72:/" + id + Environment.NewLine + TransformText(r.AdditionalInfo, 35) + Environment.NewLine + "-}";
            return s;
        }

        private protected string CreateMt920(AchRtgs r)
        {
            //It is used to request the account servicing institution to transmit one or more MT 940 Customer Statement(s),
            //MT 941 Balance Report(s), MT 942 Interim Transaction Report(s), or MT 950 Statement Message(s) 
            //containing the latest information available for the account(s) identified in the message.

            //Used mostly to request for balance 941`

            var tm = DateTime.Now.ToString("HHmm");
            var dt = _procDate.ToString("yyMMdd");
            var id = r.EndToEndId;
            id = id.PadLeft(10, '0');
            id = id.Substring(id.Length - 10);
            var s = "{1:F01NBETETABAXXX" + id + "}{2:O920" + tm + dt + r.RemitterBic + "XXXX" + id + dt + tm + "N}{4:" + Environment.NewLine;
            s += ":20:" + r.Trans_Ref + Environment.NewLine;
            s += ":12:941" + Environment.NewLine;
            s += ":25:" + r.AdditionalInfo + Environment.NewLine + "-}";
            return s;
        }

        private protected string CreateMt999(AchRtgs r)
        {
            //used by financial institutions to send or receive information for which another message type is not applicable.
            var tm = DateTime.Now.ToString("HHmm");
            var dt = _procDate.ToString("yyMMdd");
            var id = r.EndToEndId;
            id = id.PadLeft(10, '0');
            id = id.Substring(id.Length - 10);
            var s = "{1:F01" + r.BeneficiaryBic + "XXXX" + id + "}{2:O999" + tm + dt + r.RemitterBic + "XXXX" + id + dt + tm + "N}{4:" + Environment.NewLine;
            s += ":20:" + r.Trans_Ref + Environment.NewLine;
            s += ":21:999" + Environment.NewLine;
            s += ":79:" + TransformText(r.AdditionalInfo, 35) + Environment.NewLine + "-}";
            return s;
        }

        private protected AchRtgs ReadRtgs(string sFile, string mt202Acc, string mt900Acc, string mt910Acc)
        {
            var rx = new Regex("[^A-Za-z0-9]");
            AchRtgs rtgs = new AchRtgs();
            string[] sDet = File.ReadAllLines(sFile);
            string content = File.ReadAllText(sFile);
            string[] sGrp = content.Split('{');
            string sType = sGrp[2].Substring(3, 3);
            var tm = DateTime.Now.ToString("HH:mm:ss");
            var currencyType = CurrencyType.Etb;
            List<string> l = GetRtgsDetails(sDet);
            if (sDet.Any(p => p.StartsWith(":32A:")))
                Enum.TryParse(l.First(p => p.StartsWith(":32A:")).Substring(11, 3), true, out currencyType);
            else if (sDet.Any(p => p.StartsWith(":62F:")))
                Enum.TryParse(l.First(p => p.StartsWith(":62F:")).Substring(12, 3), true, out currencyType);
            string[] sBeneficiary;
            string[] sRemitter;
            switch (sType)
            {
                case "103":
                    sBeneficiary = l.First(p => p.StartsWith(":59")).Substring(5).Replace("/", "").Split('|');
                    sRemitter = l.First(p => p.StartsWith(":50")).Substring(5).Replace("/", "").Split('|');
                    rtgs = new AchRtgs
                    {
                        AdditionalInfo = l.Any(p => p.StartsWith(":72:")) ? l.First(p => p.StartsWith(":72:")).Substring(4).Replace("|", "") : "",
                        Amount = Convert.ToDecimal(l.First(p => p.StartsWith(":32")).Substring(14).Replace(",", ".")),
                        CollectionDate = Convert.ToDateTime(DateConvertion(l.First(p => p.StartsWith(":32A:")).Substring(5, 6))),
                        BeneficiaryAcc = rx.Replace(sBeneficiary[0].Trim(), string.Empty).PadRight(25, ' ').Substring(0, 24).Trim(),
                        BeneficiaryBic = l.First(p => p.StartsWith(":57A:")).Substring(5),
                        BeneficiaryBranch = sBeneficiary.Length > 1 ? sBeneficiary[1].Trim() : sBeneficiary[0].Trim(),
                        BeneficiaryName = sBeneficiary.Length > 1 ? sBeneficiary[1].Trim() : sBeneficiary[0].Trim(),
                        Currency = currencyType,
                        EndToEndId = l.First(p => p.StartsWith(":20:")).Remove(0, 4),
                        FileId = l.Any(p => p.StartsWith(":70:")) ? l.First(p => p.StartsWith(":70:")).Substring(4).Replace("|", "") : l.First(p => p.StartsWith(":20:")).Remove(0, 4),
                        Filename = Path.GetFileName(sFile),
                        Trans_Ref = l.Any(p => p.StartsWith(":20:")) ? l.First(p => p.StartsWith(":20:")).Remove(0,4) : l.First(p => p.StartsWith(":70:")).Substring(4),
                        RemitterAcc = sRemitter[0].Trim(),
                        RemitterBic = l.First(p => p.StartsWith(":52A:")).Substring(5),
                        RemitterBranch = sBeneficiary.Length > 1 ? sBeneficiary[1].Trim() : sBeneficiary[0].Trim(),
                        RemitterName = sRemitter.Length > 1 ? sRemitter[1].Trim() : sRemitter[0].Trim(),
                        RtgsType = RtgsType.Mt103,
                        TxnId = l.First(p => p.StartsWith(":20:")).Substring(4)


                    };
                    break;
                case "202":
                    sBeneficiary = l.First(p => p.StartsWith(":58")).Substring(5).Split('|');
                    sRemitter = l.First(p => p.StartsWith(":52")).Substring(5).Split('|');
                    rtgs = new AchRtgs
                    {
                        AdditionalInfo = l.Any(p => p.StartsWith(":72:")) ? l.First(p => p.StartsWith(":72:")).Substring(4).Replace("|", "") : "",
                        Amount = Convert.ToDecimal(l.First(p => p.StartsWith(":32A:")).Substring(14).Replace(",", ".")),
                        CollectionDate = Convert.ToDateTime(DateConvertion(l.First(p => p.StartsWith(":32A:")).Substring(5, 6))),
                        BeneficiaryAcc = mt202Acc,
                        BeneficiaryBic = sBeneficiary[0].Trim().Length == 8 ? sBeneficiary[0].Trim() : sBeneficiary[1].Trim(),
                        BeneficiaryBranch = sBeneficiary.Length > 1 ? sBeneficiary[1].Trim() : sBeneficiary[0].Trim(),
                        BeneficiaryName = mt202Acc,
                        Currency = currencyType,
                        EndToEndId = l.First(p => p.StartsWith(":21:")).Remove(0, 4),
                        FileId = l.First(p => p.StartsWith(":21:")).Substring(4),
                        Filename = Path.GetFileName(sFile),
                        Trans_Ref = l.First(p => p.StartsWith(":20:")).Substring(4),
                        RemitterAcc = sRemitter[0].Trim(),
                        RemitterBic = sRemitter[0].Trim().Length == 8 ? sRemitter[0].Trim() : sRemitter[1].Trim(),
                        RemitterBranch = sRemitter.Length > 1 ? sRemitter[1].Trim() : sRemitter[0].Trim(),
                        RemitterName = sRemitter.Length > 1 ? sRemitter[1].Trim() : sRemitter[0].Trim(),
                        RtgsType = RtgsType.Mt202,
                        TxnId = l.First(p => p.StartsWith(":20:")).Remove(0, 4)
                    };
                    break;
                case "900":
                    sBeneficiary = l.First(p => p.StartsWith(":52A:")).Substring(5).Split('|');
                    rtgs = new AchRtgs
                    {
                        AdditionalInfo = l.Any(p => p.StartsWith(":72:/")) ? l.First(p => p.StartsWith(":72:")).Substring(4).Replace("|", "") : "",
                        Amount = Convert.ToDecimal(l.First(p => p.StartsWith(":32A:")).Substring(14).Replace(",", ".")),
                        CollectionDate = Convert.ToDateTime(DateConvertion(l.First(p => p.StartsWith(":32A:")).Substring(5,6))),
                        BeneficiaryAcc = mt900Acc,
                        BeneficiaryBic = OurBankBic,
                        BeneficiaryBranch = HQBranch,
                        BeneficiaryName = mt202Acc,
                        Currency = currencyType,
                        EndToEndId = l.First(p => p.StartsWith(":20:")).Remove(0, 4),
                        FileId = l.First(p => p.StartsWith(":21:")).Substring(4),
                        Filename = Path.GetFileName(sFile),
                        Trans_Ref = l.First(p => p.StartsWith(":21:")).Substring(4),
                        RemitterAcc = sBeneficiary[0].Trim(),
                        RemitterBic = l.First(p => p.StartsWith(":52A:")).Substring(5),
                        RemitterBranch = sBeneficiary.Length > 1 ? sBeneficiary[1].Trim() : sBeneficiary[0].Trim(),
                        RemitterName = sBeneficiary.Length > 1 ? sBeneficiary[1].Trim() : sBeneficiary[0].Trim(),
                        RtgsType = RtgsType.Mt900,
                        TxnId = l.First(p => p.StartsWith(":20:")).Remove(0, 4)
                    };
                    break;
                case "910":
                    sRemitter = l.First(p => p.StartsWith(":52A:")).Substring(5).Split('|');
                    rtgs = new AchRtgs
                    {
                        AdditionalInfo = l.Any(p => p.StartsWith(":72:/")) ? l.First(p => p.StartsWith(":72:")).Substring(4).Replace("|", "") : "",
                        Amount = Convert.ToDecimal(l.First(p => p.StartsWith(":32A:")).Substring(14).Replace(",", ".")),
                        CollectionDate = Convert.ToDateTime(DateConvertion(l.First(p => p.StartsWith(":32A:")).Substring(5, 6))),
                        BeneficiaryAcc = mt910Acc,
                        BeneficiaryBic = OurBankBic,
                        BeneficiaryBranch = HQBranch,
                        BeneficiaryName = mt202Acc,
                        Currency = currencyType,
                        EndToEndId = l.First(p => p.StartsWith(":20:")).Remove(0, 4),
                        FileId = l.First(p => p.StartsWith(":21:")).Substring(4),
                        Filename = Path.GetFileName(sFile),
                        Trans_Ref = l.First(p => p.StartsWith(":21:")).Substring(4),
                        RemitterAcc = sRemitter[0].Trim(),
                        RemitterBic = l.First(p => p.StartsWith(":52A:")).Substring(5),
                        RemitterBranch = sRemitter.Length > 1 ? sRemitter[1].Trim() : sRemitter[0].Trim(),
                        RemitterName = sRemitter.Length > 1 ? sRemitter[1].Trim() : sRemitter[0].Trim(),
                        RtgsType = RtgsType.Mt910,
                        TxnId = l.First(p => p.StartsWith(":20:")).Remove(0, 4)
                    };
                    break;
                case "941":
                    rtgs = new AchRtgs
                    {
                        AdditionalInfo = l.Any(p => p.StartsWith(":61:")) ? l.First(p => p.StartsWith(":61:")).Substring(4) : "ACCOUNT BALANCE RESPONSE AS AT " + tm,
                        Amount = Convert.ToDecimal(l.First(p => p.StartsWith(":62F:")).Substring(15).Replace(",", ".")),
                        CollectionDate = Convert.ToDateTime(DateConvertion(l.First(p => p.StartsWith(":60M:") | p.StartsWith(":61M:") | p.StartsWith(":60F:") | p.StartsWith(":62F:")).Substring(6, 6))),
                        BeneficiaryAcc = l.First(p => p.StartsWith(":25:")).Substring(4),
                        BeneficiaryBic = OurBankBic,
                        BeneficiaryBranch = HQBranch,
                        BeneficiaryName = "BANKS P&S ACCOUNT",
                        Currency = currencyType,
                        EndToEndId = l.First(p => p.StartsWith(":20:")).Remove(0, 4),
                        FileId = l.First(p => p.StartsWith(":21:")).Substring(4),
                        Filename = Path.GetFileName(sFile),
                        Trans_Ref = l.First(p => p.StartsWith(":21:")).Substring(4),
                        RemitterAcc = l.First(p => p.StartsWith(":25:")).Substring(4),
                        RemitterBic = "NBETETAB",
                        RemitterBranch = HQBranch,
                        RemitterName = "ATS P&S ACCOUNT",
                        RtgsType = RtgsType.Mt941,
                        TxnId = l.First(p => p.StartsWith(":20:")).Remove(0, 4)
                    };
                    break;
                case "204":
                    rtgs = new AchRtgs
                    {
                        AdditionalInfo = l.Any(p => p.StartsWith(":72:/")) ? l.First(p => p.StartsWith(":72:")).Substring(4).Replace("|", "") : "",
                        Amount = Convert.ToDecimal(l.First(p => p.StartsWith(":19:")).Substring(4).Replace(",", ".")),
                        BeneficiaryAcc = l.First(p => p.StartsWith(":53A:")).Remove(0, 6),
                        CollectionDate = Convert.ToDateTime(DateConvertion(l.First(p => p.StartsWith(":30:")).Substring(4,6))),
                        BeneficiaryBic = OurBankBic,
                        BeneficiaryBranch = HQBranch,
                        BeneficiaryName = "BANKS P&S ACCOUNT",
                        Currency = currencyType,
                        EndToEndId = l.First(p => p.StartsWith(":20:")).Remove(0, 4),
                        FileId = l.First(p => p.StartsWith(":21:")).Substring(4),
                        Filename = Path.GetFileName(sFile),
                        Trans_Ref = l.First(p => p.StartsWith(":20:")).Substring(4),
                        RemitterAcc = l.First(p => p.StartsWith(":58A:")).Substring(6),
                        RemitterBic = "NBETETAB",
                        RemitterBranch = HQBranch,
                        RemitterName = "ATS P&S ACCOUNT",
                        RtgsType = RtgsType.Mt204,
                        TxnId = l.First(p => p.StartsWith(":20:")).Remove(0, 4)
                    };
                    break;
                case "205":
                    rtgs = new AchRtgs
                    {
                        AdditionalInfo = l.Any(p => p.StartsWith(":72:/")) ? l.First(p => p.StartsWith(":72:")).Substring(4).Replace("|", "") : "",
                        Amount = Convert.ToDecimal(l.First(p => p.StartsWith(":32A:")).Substring(14).Replace(",", ".")),
                        BeneficiaryAcc = l.First(p => p.StartsWith(":58A:")).Remove(0, 6),
                        CollectionDate = Convert.ToDateTime(DateConvertion(l.First(p => p.StartsWith(":32A:")).Substring(5, 6))),
                        BeneficiaryBic = OurBankBic,
                        BeneficiaryBranch = HQBranch,
                        BeneficiaryName = "BANKS P&S ACCOUNT",
                        Currency = currencyType,
                        EndToEndId = l.First(p => p.StartsWith(":20:")).Remove(0, 4),
                        FileId = l.First(p => p.StartsWith(":21:")).Substring(4),
                        Filename = Path.GetFileName(sFile),
                        Trans_Ref = l.First(p => p.StartsWith(":21:")).Substring(4),
                        RemitterAcc = l.First(p => p.StartsWith(":52A:")).Substring(6),
                        RemitterBic = "NBETETAB",
                        RemitterBranch = HQBranch,
                        RemitterName = "ATS P&S ACCOUNT",
                        RtgsType = RtgsType.Mt205,
                        TxnId = l.First(p => p.StartsWith(":20:")).Remove(0, 4)
                    };
                    break;
                case "950":
                    rtgs = new AchRtgs
                    {
                        AdditionalInfo = l.Any(p => p.StartsWith(":61:")) ? l.First(p => p.StartsWith(":61:")).Substring(4) : "EOD BALANCE STATEMENT AS AT " + tm,
                        Amount = Convert.ToDecimal(l.First(p => p.StartsWith(":60M:") | p.StartsWith(":61M:") | p.StartsWith(":60F:") | p.StartsWith(":62F:")).Substring(15).Replace(",", ".")),
                        CollectionDate = Convert.ToDateTime(DateConvertion(l.First(p => p.StartsWith(":60M:") | p.StartsWith(":61M:") | p.StartsWith(":60F:") | p.StartsWith(":62F:")).Substring(6,6))),
                        BeneficiaryAcc = l.First(p => p.StartsWith(":25:")).Substring(4),
                        BeneficiaryBic = OurBankBic,
                        BeneficiaryBranch = HQBranch,
                        BeneficiaryName = "BANKS P&S ACCOUNT",
                        Currency = currencyType,
                        EndToEndId = l.First(p => p.StartsWith(":20:")).Remove(0, 4),
                        FileId = l.First(p => p.StartsWith(":25:")).Substring(4),
                        Filename = Path.GetFileName(sFile),
                        Trans_Ref = l.First(p => p.StartsWith(":25:")).Substring(4),
                        RemitterAcc = l.First(p => p.StartsWith(":28C:")).Substring(4),
                        RemitterBic = "NBETETAB",
                        RemitterBranch = HQBranch,
                        RemitterName = "ATS P&S ACCOUNT",
                        RtgsType = RtgsType.Mt950,
                        TxnId = l.First(p => p.StartsWith(":20:")).Remove(0, 4)
                    };
                    break;
                case "999":
                    var sRemitterBic = sGrp[2].Substring(16, 8);
                    var sTxnId2 = sGrp[2].Substring(28, 20);
                    var sEndToEndId = l.Any(p => p.StartsWith(":20:")) ? l.First(p => p.StartsWith(":20:")).Remove(0, 4) : sTxnId2;
                    var sFileId = l.Any(p => p.StartsWith(":21:")) ? l.First(p => p.StartsWith(":21:")).Substring(4) : sTxnId2;
                    var sInstructionId = l.Any(p => p.StartsWith(":21:")) ? l.First(p => p.StartsWith(":21:")).Substring(4) : sTxnId2;
                    var sTxnId = l.Any(p => p.StartsWith(":20:")) ? l.First(p => p.StartsWith(":20:")).Remove(0, 4) : sTxnId2;

                    rtgs = new AchRtgs
                    {
                        AdditionalInfo = l.First(p => p.StartsWith(":79:")).Substring(4).Replace("/", " "),
                        Amount = 0,
                        BeneficiaryAcc = mt910Acc,
                        BeneficiaryBic = OurBankBic,
                        BeneficiaryBranch = HQBranch,
                        BeneficiaryName = "FREE FORMAT MESSAGE" + tm,
                        Currency = currencyType,
                        EndToEndId = sEndToEndId,
                        FileId = sFileId,
                        Filename = Path.GetFileName(sFile),
                        Trans_Ref = sInstructionId,
                        RemitterAcc = "000000000001",
                        RemitterBic = sRemitterBic,
                        RemitterBranch = HQBranch,
                        RemitterName = "ATS P&S ACCOUNT",
                        RtgsType = RtgsType.Mt999,
                        TxnId = sTxnId,
                        CollectionDate = DateTime.Now.Date,
                    };
                    break;
                case "198":
                    rtgs = new AchRtgs
                    {
                        RtgsType = RtgsType.Mt108,
                    };

                    break;
            }
            return rtgs;
        }

        private protected List<string> GetRtgsDetails(string[] d)
        {
            var l = new List<string>();
            for (var i = 1; i < d.Length - 1; i++)
            {
                if (!d[i].StartsWith(":"))
                {
                    if (!d[i - 1].StartsWith(":"))
                    {
                        if (!d[i - 2].StartsWith(":"))
                        {
                            if (!d[i - 3].StartsWith(":"))
                            {
                                d[i - 4] += "|" + d[i];
                                d[i] = "";
                            }
                            else
                            {
                                d[i - 3] += "|" + d[i];
                                d[i] = "";
                            }
                        }
                        else
                        {
                            d[i - 2] += "|" + d[i];
                            d[i] = "";
                        }
                    }
                    else
                    {
                        d[i - 1] += "|" + d[i];
                        d[i] = "";
                    }
                }
            }
            for (int i = 1; i < d.Length - 1; i++)
            {
                if (d[i].Trim() != "")
                    l.Add(d[i]);
            }
            return l;
        }

        private protected string TransformText(string s, int i)
        {
            var f = s;
            if (s.Length > 1)
            {
                f = "";
                for (int x = 0; x < s.Length;)
                {
                    if (x + i < s.Length)
                    {
                        string y = s.Substring(x, i);
                        int l = y.LastIndexOf(' ');
                        f += y.Substring(0, l) + Environment.NewLine;
                        x = x + l;
                    }
                    else
                    {
                        f += s.Substring(x);
                        x = s.Length;
                    }
                }
            }
            return f;
        }
        private void ExecuteData(string pubSqlString, ref DataTable pubDataTable, queryType qType, dataExecTypes dtExecType)
        {
            try
            {
                if (pubDataSet == null)
                {
                    pubDataSet = new DataSet();
                }
                pubDataSet.Tables.Clear();
                string strConnectString = "";
                using (conn = new SqlConnection())
                {
                    //conn.ConnectionTimeout = 0;
                    SqlConnection.ClearAllPools();
                    DBServerName = DBServerName.Replace("'", "");
                    DatabaseName = DatabaseName.Replace("'", "");
                    BRUserName = BRUserName.Replace("'", "");
                    BRUserPassword = BRUserPassword.Replace(" ", "+").Replace("'", "");
                    conn = BRAccess.BRConnection(BRUserName, BRUserPassword, DatabaseName, DBServerName);

                    if (conn.State != ConnectionState.Open)
                    {
                        try
                        {
                            conn.Open();
                        }
                        catch (Exception exception)
                        {
                            WriteLogFile("Database Connection Error", exception);
                        }
                    }
                }

                switch (qType)
                {
                    case queryType.SelectStatement:
                        pubDataSqlCommand = new SqlCommand(pubSqlString, conn);
                        if ((pubDataSqlCommand.Connection.State == ConnectionState.Closed))
                        {
                            pubDataSqlCommand.Connection.Open();
                        }

                        if ((dtExecType == dataExecTypes.ExecTypeNonQuery))
                        {
                            pubDataSqlCommand.ExecuteNonQuery();
                        }
                        else
                        {
                            pubDataSqlAdapter = new SqlDataAdapter(pubSqlString, conn);
                            pubDataSqlAdapter.Fill(pubDataSet);
                            pubDataTable = pubDataSet.Tables[0];
                        }

                        break;
                    case queryType.StoredProcedure:
                        pubDataSqlCommand = new SqlCommand(pubSqlString, conn);
                        pubDataSqlCommand.CommandType = CommandType.StoredProcedure;
                        if ((pubDataSqlCommand.Connection.State == ConnectionState.Closed))
                        {
                            pubDataSqlCommand.Connection.Open();
                        }

                        if ((dtExecType == dataExecTypes.ExecTypeNonQuery))
                        {
                            pubDataSqlCommand.ExecuteNonQuery();
                        }
                        else
                        {
                            pubDataSqlAdapter = new SqlDataAdapter(pubSqlString, conn);
                            pubDataSqlAdapter.Fill(pubDataSet);
                            pubDataTable = pubDataSet.Tables[0];
                        }

                        break;
                }
            }
            catch (Exception ex)
            {
                WriteLogFile(ex.Message, ex);
            }
        }

        [Obsolete]
        private string GetConnectionDetails()
        {
            string DBServerName = ConfigurationSettings.AppSettings["DBServerName"].ToString();
            string DatabaseName = ConfigurationSettings.AppSettings["DatabaseName"].ToString();
            string UserName = ConfigurationSettings.AppSettings["BRUserName"].ToString();
            string DBPassword = ConfigurationSettings.AppSettings["BRUserPassword"];
            bool Integrated = Convert.ToBoolean(ConfigurationSettings.AppSettings["Integrated"].ToString());
            string strConnectString = string.Empty;
            if (!Integrated)
            {
                strConnectString = "Data Source=" + DBServerName + ";Initial Catalog=" + DatabaseName + " ;User ID=" + UserName + ";Password=" + DBPassword + ";Connect Timeout=5;MultipleActiveResultSets=true";
            }
            else
            {
                strConnectString = "Data Source=" + DBServerName + ";Initial Catalog=" + DatabaseName + " ;MultipleActiveResultSets=true;integrated security=SSPI;Trusted_Connection = yes;";
            }
            return strConnectString;
        }
    }

    public class ETRTGSIN : ETSys
    {
        private readonly string _location = ConfigurationManager.AppSettings["Archive"];
        private DateTime _procDate;
        private readonly bool _signFiles;
        private readonly string _certName = "";
        private readonly string _temp;
        private static readonly Regex Rx = new Regex("[^A-Za-z0-9 ]");
        private readonly string OperatorID = "Sys";
        private readonly string OurBankBic = "";
        private readonly string HQBranch = "";
        private DataSet pubDataSet;
        private SqlConnection conn;
        private DataSet publicDset;
        private SqlCommand pubDataSqlCommand;
        private SqlDataAdapter pubDataSqlAdapter;
        private SqlConnection pubDbSqlConn;

        private DataTable dt = new DataTable();
        private readonly string DBServerName = ConfigurationManager.AppSettings["DBServerName"];
        private readonly string DatabaseName = ConfigurationManager.AppSettings["DatabaseName"];
        private readonly string BRUserName = ConfigurationManager.AppSettings["BRUserName"];
        private readonly string DBPassword = ConfigurationManager.AppSettings["BRUserPassword"];
        private enum dataExecTypes
        {

            ExecTypeQuery = 0,

            ExecTypeNonQuery = 1,
        }
        private enum queryType
        {

            SelectStatement = 0,

            StoredProcedure = 1,
        }
        // M = Location, N = SignFiles, O = CertName, P = Temp, Q = ProcDate, R = OurBankBic, S = HQBranch, T = details, U = Action
        public ETRTGSIN(string M, bool N, string O, string P, DateTime Q, string R, string S)
        {
            _location = M;
            _procDate = Q;
            _signFiles = N;
            _certName = O;
            _temp = P;
            OurBankBic = R;
            HQBranch = S;
        }

        public ETRTGSIN(List<string> Flist)
        {
            foreach (string file in Flist)
            {
                SingleRTGS(file);
            }
        }
        private void SingleRTGS(string sFile)
        {
            string sArchivePath = string.Empty;
            string DirPath = Path.GetDirectoryName(sFile);
            string FileName = Path.GetFileName(sFile);
            bool response = false;
            try
            {
                if (File.Exists(Path.Combine(DirPath, @"RTGSPAYMENTS\" + FileName)))
                    sFile = Path.Combine(DirPath, @"RTGSPAYMENTS\" + FileName);
                else if (File.Exists(Path.Combine(DirPath, @"RTGSSTATEMENTS\" + FileName)))
                    sFile = Path.Combine(DirPath, @"RTGSSTATEMENTS\" + FileName);
                else if (File.Exists(Path.Combine(DirPath, @"RTGSREPLIES\" + FileName)))
                    sFile = Path.Combine(DirPath, @"RTGSREPLIES\" + FileName);
                else if (File.Exists(Path.Combine(DirPath, @"RTGSADVICES\" + FileName)))
                    sFile = Path.Combine(DirPath, @"RTGSADVICES\" + FileName);
                else if (File.Exists(Path.Combine(DirPath,  Path.GetFileNameWithoutExtension(FileName))))
                    sFile = Path.Combine(DirPath,  Path.GetFileNameWithoutExtension(FileName));
                else if (File.Exists(Path.Combine(DirPath,  FileName)))
                    sFile = Path.Combine(DirPath,  FileName);
                else if (File.Exists(Path.Combine(DirPath,  FileName)))
                    sFile = Path.Combine(DirPath,FileName);
                else if (File.Exists(Path.Combine(DirPath,  FileName)))
                    sFile = Path.Combine(DirPath,  FileName);
                else if (File.Exists(Path.Combine(DirPath,  FileName)))
                    sFile = Path.Combine(DirPath,  FileName);
                else if (File.Exists(Path.Combine(DirPath,  Path.GetFileNameWithoutExtension(FileName))))
                    sFile = Path.Combine(DirPath,  Path.GetFileNameWithoutExtension(FileName));
            }
            catch { }

              var rtgsProcessor = new ETRTGSProcessing(sFile, false, "", "", DateTime.Now, "", "");
            AchRtgs rtgs = rtgsProcessor.BRRTGSFiles(sFile);

            response = SaveRTGS(rtgs);

            if (response)
            {
                var strpath = sFile.LastIndexOf(".") + 1;
                sArchivePath = Path.Combine(_location, @"ARCHIVE\" + DateTime.Now.ToString("yyyyMMdd") + @"\RTGS\");
                string sArchiveFile = Path.Combine(sArchivePath, sFile);
                if (!Directory.Exists(Path.GetDirectoryName(sArchivePath)))
                    Directory.CreateDirectory(Path.GetDirectoryName(sArchivePath));
                if ((File.Exists(sArchiveFile)))
                    sArchiveFile = sArchivePath + @"\" + sFile.Substring(sFile.LastIndexOf(@"\") + 1);
                try
                {
                    if (File.Exists(sFile))
                        File.Move(sFile, sArchiveFile);

                    File.Delete(sFile);
                }
                catch (Exception exi)
                {
                    WriteLogFile(exi.Message, exi);
                    File.Delete(sArchiveFile);
                    if (File.Exists(sFile))
                        File.Move(sFile, sArchiveFile);

                }
            }
        }
        public bool logAudit(AchRtgs d, string OperatorID, string AuditKey) //this a decoy method Name  for SaveRTGS
        {
            bool IsSaved = false;
            if (AuditKey == "PXdetsrfDST")
            {
                IsSaved = SaveRTGS(d);
            }
            return IsSaved;
        }

            private protected bool SaveRTGS(AchRtgs d)
        {
            bool response = false;
            Hashtable LineItemsTable = new Hashtable();
            try
            {
                if (d.RemitterBic.ToString().Split('|').Length > 1)
                    d.RemitterBic = d.RemitterBic.ToString().Split('|')[1];

                if ((d.BeneficiaryBic.ToString().Split('|').Length > 1))
                    d.BeneficiaryBic = d.BeneficiaryBic.ToString().Split('|')[1];
                // ------------------------------------------------------------------------------------------------------
                LineItemsTable.Add("Trans_Ref", d.TxnId);
                LineItemsTable.Add("MessageType", d.RtgsType.ToString().ToUpper());
                LineItemsTable.Add("TrxCurrencyID", d.Currency.ToString().ToUpper());
                LineItemsTable.Add("TrxAmount", Convert.ToDecimal(d.Amount).ToString());
                LineItemsTable.Add("BeneficiaryAcc", d.BeneficiaryAcc);
                LineItemsTable.Add("BeneficiaryName", d.BeneficiaryName);
                LineItemsTable.Add("BeneficiaryBic", d.BeneficiaryBic);
                LineItemsTable.Add("BeneficiaryBranch", d.BeneficiaryBranch);
                LineItemsTable.Add("RemitterAcc", d.RemitterAcc);
                LineItemsTable.Add("RemitterName", d.RemitterName);
                LineItemsTable.Add("RemitterBic", d.RemitterBic);
                LineItemsTable.Add("RemitterBranch", d.RemitterBranch);
                LineItemsTable.Add("AdditionalInfo", d.AdditionalInfo);
                LineItemsTable.Add("TxFilename", d.Filename);
                LineItemsTable.Add("Field21", d.EndToEndId);
                LineItemsTable.Add("CreatedBy", OperatorID);
                LineItemsTable.Add("MessageDate", d.CollectionDate);

                dt = new DataTable();
                if (dt.Columns.Count <= 0)
                {
                    foreach (string name in LineItemsTable.Keys)
                    {
                        try
                        {
                            DataColumn ColName = new DataColumn();
                            ColName.ColumnName = name;
                            if (LineItemsTable[name] == null)
                                ColName.DataType = typeof(string);
                            else
                                ColName.DataType = Type.GetType(LineItemsTable[name].GetType().FullName.ToString());
                            dt.Columns.Add(ColName);
                        }
                        catch (Exception ex)
                        {
                            WriteLogFile(ex.Message, ex);
                            return response;
                        }
                    }
                }
                DataRow dr = dt.NewRow();
                foreach (string name in LineItemsTable.Keys)
                    dr[name] = LineItemsTable[name];
                dt.Rows.Add(dr);
                response = SaveRTGSToDB(dt);
                return response;
            }

            // ------------------------------------------------------------------------------------------------------
            catch (Exception ex)
            {
                WriteLogFile(ex.Message, ex);
                return response;
            }
        }
        private protected bool SaveRTGSToDB(DataTable dtBrOutClearing)
        {
            SqlCommand Command;
            SqlConnection SqlConn;

            try
            {
                if (dtBrOutClearing.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtBrOutClearing.Rows)
                    {
                        //GetConnectionSQL();
                        try
                        {
                            SqlConn = GetConnectionSQL();
                            Command = new SqlCommand("P_AddRTGSIncomingMessages", SqlConn);
                            Command.CommandType = CommandType.StoredProcedure;
                            Command.Parameters.Add("@TxFileName", SqlDbType.NVarChar).Value = dr["TxFileName"].ToString();
                            Command.Parameters.Add("@Trans_Ref", SqlDbType.NVarChar).Value = dr["Trans_Ref"].ToString();
                            Command.Parameters.Add("@MessageType", SqlDbType.NVarChar).Value = dr["MessageType"].ToString();
                            Command.Parameters.Add("@TrxCurrencyID", SqlDbType.NVarChar).Value = dr["TrxCurrencyID"].ToString();
                            Command.Parameters.Add("@TrxAmount", SqlDbType.NVarChar).Value = dr["TrxAmount"].ToString();
                            Command.Parameters.Add("@BeneficiaryAcc", SqlDbType.NVarChar).Value = dr["BeneficiaryAcc"].ToString();
                            Command.Parameters.Add("@BeneficiaryName", SqlDbType.NVarChar).Value = dr["BeneficiaryName"].ToString();
                            Command.Parameters.Add("@BeneficiaryBic", SqlDbType.NVarChar).Value = dr["BeneficiaryBic"].ToString();
                            Command.Parameters.Add("@BeneficiaryBranch", SqlDbType.NVarChar).Value = dr["BeneficiaryBranch"].ToString();
                            Command.Parameters.Add("@RemitterAcc", SqlDbType.NVarChar).Value = dr["RemitterAcc"].ToString();
                            Command.Parameters.Add("@RemitterName", SqlDbType.NVarChar).Value = dr["RemitterName"].ToString();
                            Command.Parameters.Add("@RemitterBic", SqlDbType.NVarChar).Value = dr["RemitterBic"].ToString();
                            Command.Parameters.Add("@RemitterBranch", SqlDbType.NVarChar).Value = dr["RemitterBranch"].ToString();
                            Command.Parameters.Add("@AdditionalInfo", SqlDbType.NVarChar).Value = dr["AdditionalInfo"].ToString();
                            Command.Parameters.Add("@CreatedBy", SqlDbType.NVarChar).Value = dr["CreatedBy"].ToString();
                            Command.Parameters.Add("@TrxDate", SqlDbType.SmallDateTime).Value = dr["MessageDate"];
                            SqlConn = GetConnectionSQL();
                            Command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            WriteLogFile(ex.Message, ex);
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                WriteLogFile(ex.Message, ex);
                return false;

            }
        }
        private protected SqlConnection GetConnectionSQL(string strSystem = "")
        {
            SqlConnection connection = null;
            string CnString = string.Empty;


            string strDBServerName = DBServerName.Replace("'", "");
            string strDatabaseName = DatabaseName.Replace("'", "");
            string strBRUserName = BRUserName.Replace("'", "");
            string strBRUserPassword = DBPassword.Replace(" ", "+").Replace("'", "");
            connection = BRAccess.BRConnection(strBRUserName, strBRUserPassword, strDatabaseName, strDBServerName);
            if (connection == null)
            {
                CnString = "Data Source=" + DBServerName + ";Initial Catalog=" + DatabaseName + " ;User ID=" + BRAccess.BRUserName(strBRUserName) + " ;Password=" + BRAccess.BRUserPassword(strBRUserPassword) + ";MultipleActiveResultSets=true";
                connection = new SqlConnection(CnString);
                SqlConnection.ClearAllPools();
            }

            if (connection.State != ConnectionState.Open)
                connection.Open();

            return connection;
        }
    }
    public class ETSys
    {
        protected internal void WriteLogFile(string Message, Exception ex = null)
        {
            StreamWriter sw = null;
            try
            {
                string sYear = DateTime.Now.Year.ToString();
                string sMonth = DateTime.Now.Month.ToString();
                string sDay = DateTime.Now.Day.ToString();
                StackTrace st;
                StackFrame sf;
                Int32 LineNo = 0;
                string errorLogPath = ConfigurationManager.AppSettings["LogsFolder"];
                string sPathName = Path.Combine(errorLogPath + @"\RTGS\" + sYear + @"\" + sMonth + @"\" + sDay + @"\");
                sPathName = Path.Combine(sPathName + "BR-ETRTGSServiceLogs");
                if (!Directory.Exists(sPathName))
                    Directory.CreateDirectory(sPathName);

                try
                {
                    st = new StackTrace(ex);
                    sf = st.GetFrame(st.FrameCount - 1);
                    LineNo = sf.GetFileLineNumber();
                }
                catch
                {
                }

                sw = new StreamWriter(sPathName + @"\Log.txt", true);
                string AppendErrorMessage = Environment.NewLine
                + "=============================================================================================="
                + Environment.NewLine + "Target Site : RTGS"
                + Environment.NewLine + "Date : " + DateTime.Now
                + Environment.NewLine + "Log : " + Message
                + Environment.NewLine + "Line " + LineNo.ToString()
                + Environment.NewLine
                + "===============================================================================================";
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ": " + AppendErrorMessage);
                sw.Flush();
                sw.Close();
            }
            catch (Exception exi)
            {
            }
        }
    }
}
