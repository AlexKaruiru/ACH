using System;
using System.Data;
using System.Configuration;
using BR.DBClient;

using System.Net;
using System.Security.Cryptography;
using BR.ApplicationBlocks.Data;
using BRNetSecurity;
using BRCoreEntities.SystemBranchSettings;
using BRCoreEntities.SystemBranchStatus;
using BRBase;
using BREntities.BRCreateClearingFile;

namespace BRClearing.Util
{

    public class ClearingValidations
    {
        private static string[] strCon = new string[4];
        private static string strDBServerName = string.Empty;
        private static string strDatabaseName = string.Empty;
        private static string strBRUserName = string.Empty;
        private static string strBRUserPassword = string.Empty;
        private static string strSYSADMIN1UserName = string.Empty;
        private static string strSYSADMIN1Password = string.Empty;

        public static string CalculateChequeDigit(string AccountID, string BankID, string BranchID, string CountryCodeID)
        {
            string functionReturnValue = null;
            try
            {
                string Acc = "";
                string Bank = "";
                string Branch = "";
                functionReturnValue = "";
                //Put the Sort Code together
                string SortCode = "";
                long TotalValue = 0;
                int x = 0;
                int startposition = 0;
                long WeightValue = 0;
                string Modulus11 = "";

                string Bankcode = BankID;
                string BranchCode = BranchID;
                string ACCOUNTNUMBER = AccountID;
                string CountryCode = CountryCodeID;

                if (CountryCode == "UG")
                {
                    SortCode = (Bankcode).ToString().Trim() + (BranchCode).ToString().Trim() + (CountryCode).ToString().Trim() + (ACCOUNTNUMBER).ToString().Trim();
                    startposition = 16;
                    WeightValue = 2;
                    TotalValue = 0;
                    for (x = 0; x <= (SortCode).ToString().Length - 1; x++)
                    {
                        TotalValue = TotalValue + (BRBaseConvert.ConvertToInt64(Mid(SortCode, startposition, 1)) * WeightValue);
                        startposition = startposition - 1;
                        WeightValue = WeightValue + 1;
                        if (WeightValue > 9)
                            WeightValue = 2;
                    }
                    Modulus11 = BRBaseConvert.ConvertToString(TotalValue % 11);
                    if (BRBaseConvert.ConvertToInt32(Modulus11) != 0)
                    {
                        Modulus11 = BRBaseConvert.ConvertToString(11 - BRBaseConvert.ConvertToInt32(Modulus11));
                    }
                    if ((Modulus11).ToString().Length > 1)
                    {
                        functionReturnValue = Modulus11;
                    }
                    else
                    {
                        functionReturnValue = "0" + Modulus11;
                    }
                }
                else if (CountryCode == "TZ")
                {
                    Bankcode = BankID;
                    BranchCode = BranchID;
                    ACCOUNTNUMBER = AccountID;
                    CountryCode = CountryCode;
                    SortCode = CountryCode + Bankcode + BranchCode;
                    startposition = 6;
                    for (x = 2; x <= 7; x++)
                    {
                        TotalValue = TotalValue + (BRBaseConvert.ConvertToInt64(Mid(SortCode, startposition, 1)) * x);
                        startposition = startposition - 1;
                    }

                    Modulus11 = BRBaseConvert.ConvertToString(TotalValue % 11);
                    if (BRBaseConvert.ConvertToInt32(Modulus11) != 0)
                    {
                        Modulus11 = BRBaseConvert.ConvertToString(11 - BRBaseConvert.ConvertToInt32(Modulus11));
                    }
                    if (BRBaseConvert.ConvertToInt32(Modulus11) == 10)
                    {
                        Modulus11 = "0";
                    }
                    if ((Modulus11).ToString().Length > 1)
                    {
                        functionReturnValue = Modulus11;
                    }
                    else
                    {
                        functionReturnValue = "0" + Modulus11;
                    }
                }
                else if (CountryCode == "KE")
                {
                    Acc = "";
                    Bank = "";
                    Branch = "";

                    Acc = new string('0', 10 - (AccountID.ToString().Trim().Length)) + AccountID.ToString().Trim();
                    Bank = new string('0', 2 - (BankID.ToString().Trim().Length)) + BankID.ToString().Trim();
                    Branch = new string('0', 3 - (BranchID.ToString().Trim().Length)) + BranchID.ToString().Trim();
                    string pos17 = string.Empty;
                    string pos18 = string.Empty;
                    string pos19 = string.Empty;
                    string pos20 = string.Empty;
                    string pos21 = string.Empty;
                    string pos22 = string.Empty;
                    string pos23 = string.Empty;
                    string pos24 = string.Empty;
                    string pos25 = string.Empty;
                    string pos26 = string.Empty;
                    string pos32 = string.Empty;
                    string pos33 = string.Empty;
                    string pos34 = string.Empty;
                    string pos35 = string.Empty;
                    string pos36 = string.Empty;
                    pos17 = Mid(Acc, 10, 1); pos19 = Mid(Acc, 8, 1); pos21 = Mid(Acc, 6, 1); pos23 = Mid(Acc, 4, 1); pos25 = Mid(Acc, 2, 1); pos33 = Mid(Branch, 2, 1); pos35 = Mid(Bank, 2, 1);
                    pos18 = Mid(Acc, 9, 1); pos20 = Mid(Acc, 7, 1); pos22 = Mid(Acc, 5, 1); pos24 = Mid(Acc, 3, 1); pos26 = Mid(Acc, 1, 1); pos32 = Mid(Branch, 3, 1); pos34 = Mid(Branch, 1, 1); pos36 = Mid(Bank, 1, 1);
                    double newpos18 = 2 * BRBaseConvert.ConvertToInt16(pos18);
                    string nval = Left(new string('0', 2 - (newpos18.ToString().Length)) + BRBaseConvert.ConvertToString(newpos18), 1);
                    string nval1 = Right(new string('0', 2 - (newpos18.ToString().Length)) + BRBaseConvert.ConvertToString(newpos18), 1);

                    double newpos20 = 2 * BRBaseConvert.ConvertToInt16(pos20);
                    string nval2 = Left(new string('0', 2 - (newpos20.ToString().Length)) + BRBaseConvert.ConvertToString(newpos20), 1);
                    string nval3 = Right(new string('0', 2 - (newpos20.ToString().Length)) + BRBaseConvert.ConvertToString(newpos20), 1);

                    double newpos22 = 2 * BRBaseConvert.ConvertToInt16(pos22);
                    string nval4 = Left(new string('0', 2 - (newpos22.ToString().Length)) + BRBaseConvert.ConvertToString(newpos22), 1);
                    string nval5 = Right(new string('0', 2 - (newpos22.ToString().Length)) + BRBaseConvert.ConvertToString(newpos22), 1);

                    double newpos24 = 2 * BRBaseConvert.ConvertToInt16(pos24);
                    string nval6 = Left(new string('0', 2 - (newpos24.ToString().Length)) + BRBaseConvert.ConvertToString(newpos24), 1);
                    string nval7 = Right(new string('0', 2 - (newpos24.ToString().Length)) + BRBaseConvert.ConvertToString(newpos24), 1);

                    double newpos26 = 2 * BRBaseConvert.ConvertToInt16(pos26);
                    string nval8 = Left(new string('0', 2 - (newpos26.ToString().Length)) + BRBaseConvert.ConvertToString(newpos26), 1);
                    string nval9 = Right(new string('0', 2 - (newpos26.ToString().Length)) + BRBaseConvert.ConvertToString(newpos26), 1);

                    double newpos32 = 2 * BRBaseConvert.ConvertToInt16(pos32);
                    string nval10 = Left(new string('0', 2 - (newpos32.ToString().Length)) + BRBaseConvert.ConvertToString(newpos32), 1);
                    string nval11 = Right(new string('0', 2 - (newpos32.ToString().Length)) + BRBaseConvert.ConvertToString(newpos32), 1);

                    double newpos34 = 2 * BRBaseConvert.ConvertToInt16(pos34);
                    string nval12 = Left(new string('0', 2 - (newpos34.ToString().Length)) + BRBaseConvert.ConvertToString(newpos34), 1);
                    string nval13 = Right(new string('0', 2 - (newpos34.ToString().Length)) + BRBaseConvert.ConvertToString(newpos34), 1);

                    double newpos36 = 2 * BRBaseConvert.ConvertToInt16(pos36);
                    string nval14 = Left(new string('0', 2 - (newpos36.ToString().Length)) + BRBaseConvert.ConvertToString(newpos36), 1);
                    string nval15 = Right(new string('0', 2 - (newpos36.ToString().Length)) + BRBaseConvert.ConvertToString(newpos36), 1);
                    //------------ADD POS DIGITS
                    double sumnval01 = BRBaseConvert.ConvertToInt16(nval) + BRBaseConvert.ConvertToInt16(nval1);
                    double sumnval23 = BRBaseConvert.ConvertToInt16(nval2) + BRBaseConvert.ConvertToInt16(nval3);
                    double sumnval45 = BRBaseConvert.ConvertToInt16(nval4) + BRBaseConvert.ConvertToInt16(nval5);
                    double sumnval67 = BRBaseConvert.ConvertToInt16(nval6) + BRBaseConvert.ConvertToInt16(nval7);
                    double sumnval89 = BRBaseConvert.ConvertToInt16(nval8) + BRBaseConvert.ConvertToInt16(nval9);
                    double sumnval1011 = BRBaseConvert.ConvertToInt16(nval10) + BRBaseConvert.ConvertToInt16(nval11);
                    double sumnval1213 = BRBaseConvert.ConvertToInt16(nval12) + BRBaseConvert.ConvertToInt16(nval13);
                    double sumnval1415 = BRBaseConvert.ConvertToInt16(nval14) + BRBaseConvert.ConvertToInt16(nval15);

                    //----------CALC TOTAL OF LINE2
                    double sum1 = BRBaseConvert.ConvertToInt16(pos17) + BRBaseConvert.ConvertToInt16(pos19) + BRBaseConvert.ConvertToInt16(pos21) + BRBaseConvert.ConvertToInt16(pos23) + BRBaseConvert.ConvertToInt16(pos25) + BRBaseConvert.ConvertToInt16(pos33) + BRBaseConvert.ConvertToInt16(pos35);
                    double sum2 = sumnval01 + sumnval23 + sumnval45 + sumnval67 + sumnval89 + sumnval1011 + sumnval1213 + sumnval1415;
                    double sum4 = BRBaseConvert.ConvertToInt16(sum1) + BRBaseConvert.ConvertToInt16(sum2);

                    //----------GENERATE CHECKDIGIT
                    functionReturnValue = BRBaseConvert.ConvertToString(100 - BRBaseConvert.ConvertToInt16(sum4));
                    functionReturnValue = Right(functionReturnValue, 1);
                }
            }
            catch (Exception ex)
            {
                functionReturnValue = "";
            }
            return functionReturnValue;
        }
        public static string Left(string s, int len)
        {
            if (len == 0 || s.Length == 0)
                return "";
            else if (s.Length <= len)
                return s;
            else
                return s.Substring(0, len);
        }

        static string Right(string s, int count)
        {
            string newString = string.Empty;
            if (s != null && count > 0)
            {
                int startIndex = s.Length - count;
                if (startIndex > 0)
                    newString = s.Substring(startIndex, count);
                else
                    newString = s;
            }
            return newString;
        }

        public static string Mid(string s, int a, int b)
        {
            string temp = s.Substring(a - 1, b);
            return temp;
        }
        static public string CreateEncryptData(UserInfo usrInfo, string strPassword)
        {
            return string.Format("{0}{1}", usrInfo.strUser, strPassword);
        }
        static public string EncryptText(string strInputText)
        {
            byte[] data = Array.ConvertAll(strInputText.ToCharArray(), delegate (char ch) { return (byte)ch; });
            SHA256 shaM = new SHA256Managed();
            byte[] result = shaM.ComputeHash(data);
            return Convert.ToBase64String(result);
        }
        static public IDbConnection GetConnection()
        {

            IDbConnection connection = null;
            string LiveEnv = ConfigurationManager.AppSettings["IsLiveEnv"];
            if (LiveEnv == "1")
            {
                GetConfigConnDetails();
                string strSystem = ConfigurationManager.AppSettings["strSystem"].Trim().ToUpper();

                strCon[0] = strDBServerName;
                strCon[1] = strDatabaseName;
                strCon[2] = strBRUserName;
                strCon[3] = strBRUserPassword;
                connection = BRAccess.BRConnection(strBRUserName, strBRUserPassword, strDatabaseName, strDBServerName);

            }
            else
            {
                string IP = ConfigurationManager.AppSettings["IPAddress"];
                string DBName = ConfigurationManager.AppSettings["TestDBName"];
                string strSystem = ConfigurationManager.AppSettings["IPAddress"];
                string strConnectString = "";
                strConnectString = "Data Source=" + IP + ";Initial Catalog=" + DBName + ";User ID=Realm;Password=friend;";
                connection = new System.Data.SqlClient.SqlConnection(strConnectString);
            }

            if (connection.State != ConnectionState.Open)
                connection.Open();
            //}
            //else
            //{
            //    MessageBox.Show("Failed Connecting");
            //}
            //string strConnectString = strSystem;
            //strConnectString = strSystem;
            //IDbConnection connection = null;
            //if (ConfigurationManager.AppSettings["BRDBType"].ToUpper() == "SQLSERVER")
            //    connection = new System.Data.SqlClient.SqlConnection(strConnectString);
            //else if (ConfigurationManager.AppSettings["BRDBType"].ToUpper() == "ORACLE")
            //    connection = new OracleConnection(strConnectString);
            //connection.Open();
            //string user = BRAccess.BRUserName(strBRUserName);
            //string usern = BRAccess.BRUserPassword(strBRUserPassword);
            return connection;
        }
        private static void GetConfigConnDetails()
        {
            string configFilePath = ConfigurationManager.AppSettings["configFilePath"];
            string strSystem = ConfigurationManager.AppSettings["BRSystem"];

            try
            {
                if (string.IsNullOrEmpty(strDBServerName))
                {
                    ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
                    fileMap.ExeConfigFilename = configFilePath;

                    Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

                    strDBServerName = configuration.AppSettings.Settings[strSystem + "-DBServerName"].Value;
                    strDatabaseName = configuration.AppSettings.Settings[strSystem + "-DatabaseName"].Value;
                    strBRUserName = configuration.AppSettings.Settings[strSystem + "-BRUserName"].Value;
                    strBRUserPassword = configuration.AppSettings.Settings[strSystem + "-BRUserPassword"].Value;
                }
            }
            catch (Exception ex)
            {
                //lblmessage.Text = configFilePath + ":" + strSystem;
            }
        }
        static public bool AuthenticateUser(UserInfo usrInfo, string strEncryptedPassword)
        {
            bool result = false;
            try
            {
                using (IDbConnection connection = GetConnection())
                {
                    DataSet dsTemp = new DataSet();
                    IDBHelper intfDBHelper = DBClient.GetDBHelper(usrInfo);
                    IDataParameter[] arParms = intfDBHelper.CreateDBParamsArray(3);
                    arParms[0] = intfDBHelper.CreateNewDBParam("OurBranchID", SqlDbType.NVarChar, 6);
                    arParms[0].Value = usrInfo.strBranch;
                    arParms[1] = intfDBHelper.CreateNewDBParam("OperatorID", SqlDbType.NVarChar, 25);
                    arParms[1].Value = usrInfo.strUser;
                    arParms[2] = intfDBHelper.CreateNewDBParam("IPAddress", SqlDbType.NVarChar, 15);
                    arParms[2].Value = usrInfo.MachineIP;

                    intfDBHelper.FillDataset(connection, CommandType.StoredProcedure, "p_Authenticateuser", dsTemp,
                        new string[] { "DT_Temp" }, arParms);
                    if (dsTemp.Tables["DT_Temp"] == null)
                        return false;
                    if (dsTemp.Tables["DT_Temp"].Rows.Count != 0)
                    {
                        if (dsTemp.Tables["DT_Temp"].Rows[0]["Password"].ToString() == strEncryptedPassword)
                        {
                            result = true;
                        }
                        else
                        {
                            result = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public static string GetRequestIP()
        {

            string strHostName = Dns.GetHostName();
            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
            IPAddress[] addr = ipEntry.AddressList;
            return addr[0].ToString();
            //if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] == null)
            //    return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            //return UserInfo.UnknownIP;
        }
        public bool ValidateOddCents(double Value)
        {
            bool valid = false;
            try
            {
                Value = BRBaseConvert.ConvertToDouble(string.Format("{0:0.00}", Value));
                Value = Value * 100;
                //Check whether the result is old or Even Number
                if (Value % 5 == 0)
                {
                    return valid = true;
                }
                else
                {
                    return valid = false;
                }
            }
            catch (Exception ex)
            {
                return valid = false;
            }

        }
        public bool Validate4ZeroAmount(double Value)
        {
            bool valid = false;
            try
            {
                Value = BRBaseConvert.ConvertToDouble(string.Format("{0:0.00}", Value));
                Value = Value * 100;
                if (Value != 0)
                {
                    return valid = true;
                }
                else
                {
                    return valid = false;
                }
            }
            catch (Exception ex)
            {
                return valid = false;
            }
        }
        public static string CalculateEJTotalDebit(DS_trxClearing dstrxClearing, string Filler, int FileFormatValue, string ToBankID)
        {
            double SumDebit = 0;
            string StrSumDebit = string.Empty;
            try
            {
                DataRow[] drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("(Status is Null or Status<> 'R') AND RETURNCODE <> '00' and ToBank " + ToBankID + " AND TrxType='OC'");
                foreach (DataRow dvr in drHeaderFileFormatResult)
                {
                    SumDebit = SumDebit + RoundTo5Cents(BRBaseConvert.ConvertToDouble(dvr["Value"]));
                }
                StrSumDebit = BRBaseConvert.ConvertToString(SumDebit);
                StrSumDebit = string.Format("{0:0.##}", StrSumDebit);
                StrSumDebit = BRBaseConvert.ConvertToString(Convert.ToDecimal(SumDebit) * 100);
                if (StrSumDebit.ToString().Contains("."))
                {
                    StrSumDebit = StrSumDebit.Substring(0, StrSumDebit.IndexOf("."));
                }
            }
            catch (Exception ex)
            {
                StrSumDebit = "0";
                ex.ToString();
            }
            StrSumDebit = new string(BRBaseConvert.ConvertToChar(Filler), FileFormatValue - BRBaseConvert.ConvertToInt32(StrSumDebit.ToString().Length)) + StrSumDebit;
            return StrSumDebit;
        }
        public static string CalculateEJTotalCredit(DS_trxClearing dstrxClearing, string Filler, int FileFormatValue, string ToBankID)
        {
            double SumCredit = 0;
            string StrSumCredit = string.Empty;
            try
            {
                DataRow[] drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("(Status is Null or Status<> 'R') AND RETURNCODE ='00' and ToBank " + ToBankID + " AND TrxType='OC'");
                foreach (DataRow dvr in drHeaderFileFormatResult)
                {
                    SumCredit = SumCredit + RoundTo5Cents(BRBaseConvert.ConvertToDouble(dvr["Value"]));
                }
                StrSumCredit = BRBaseConvert.ConvertToString(SumCredit);
                StrSumCredit = string.Format("{0:0.##}", StrSumCredit);
                StrSumCredit = BRBaseConvert.ConvertToString(Convert.ToDecimal(SumCredit) * 100);
                if (StrSumCredit.ToString().Contains("."))
                {
                    StrSumCredit = StrSumCredit.Substring(0, StrSumCredit.IndexOf("."));
                }
            }
            catch (Exception ex)
            {
                StrSumCredit = "0";
                ex.ToString();
            }
            StrSumCredit = new string(BRBaseConvert.ConvertToChar(Filler), FileFormatValue - BRBaseConvert.ConvertToInt32(StrSumCredit.ToString().Length)) + StrSumCredit;
            return StrSumCredit;
        }
        public static string ValidateAmount(double x, string VoucherCode)
        {
            string Value = string.Empty;
            if (VoucherCode == "00")
            {
                x = RoundTo5Cents(x);
            }

            x = x * 100;
            Value = BRBaseConvert.ConvertToString(string.Format("{0:0.##}", x));
            if (Value.ToString().Contains("."))
            {
                Value = Value.Substring(0, Value.IndexOf("."));
            }

            return Value;
        }
        public static string ValidateAmount(double x)
        {
            string Value = string.Empty;
            x = RoundTo5Cents(x);
            x = x * 100;
            Value = BRBaseConvert.ConvertToString(string.Format("{0:0.##}", x));

            if (Value.ToString().Contains("."))
            {
                Value = Value.Substring(0, Value.IndexOf("."));
            }

            return Value;
        }
        public static string TotalCountEJs(DS_trxClearing dstrxClearing, string Filler, int FileFormatValue, string CurrencyType, string ToBankID)
        {
            int Count = 0;
            string StrCount = string.Empty;
            DataRow[] drHeaderFileFormatResult = null;
            BRDataSet WorkingDataEntity = new BRDataSet();
            try
            {
                switch (CurrencyType.ToString().ToUpper())
                {
                    case "FOREIGN":
                        drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("(Status is Null or Status <> 'R' or Status='') and currencyCode <> 0 and ToBank='" + ToBankID + "'  AND TrxType='OC'");
                        break;
                    default:
                        drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("(Status is Null or Status<>'R' or Status='') and currencyCode = 0 and ToBank='" + ToBankID + "' AND TrxType='OC'");
                        break;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            StrCount = new string(BRBaseConvert.ConvertToChar(Filler), FileFormatValue - BRBaseConvert.ConvertToInt32((drHeaderFileFormatResult.Length)).ToString().Length) + BRBaseConvert.ConvertToString(drHeaderFileFormatResult.Length);
            return StrCount;
        }
        public static DS_trxClearing AutoValidateData(DS_trxClearing dstrxClearingToAutoVerify, string CurrencyCode, params object[] FieldstrxClearingParamArray)
        {
            for (int y = 0; y < FieldstrxClearingParamArray.Length; y++)
            {
                switch (FieldstrxClearingParamArray[y].ToString().ToUpper())
                {
                    case "RETURNCODE":
                        for (int i = 0; i < dstrxClearingToAutoVerify.Tables[0].Rows.Count; i++)
                        {
                            if (dstrxClearingToAutoVerify.Tables[0].Rows[i]["ReturnCode"].ToString().Trim().Length != 2)
                            {
                                dstrxClearingToAutoVerify.Tables[0].Rows[i]["ReturnCode"] = new string('0', 2 - dstrxClearingToAutoVerify.Tables[0].Rows[i]["ReturnCode"].ToString().Trim().Length) + dstrxClearingToAutoVerify.Tables[0].Rows[i]["ReturnCode"].ToString();
                            }
                        }
                        break;
                    case "VOUCHERCODE":
                        for (int i = 0; i < dstrxClearingToAutoVerify.Tables[0].Rows.Count; i++)
                        {
                            if (dstrxClearingToAutoVerify.Tables[0].Rows[i]["VoucherCode"].ToString().Trim().Length != 2)
                            {
                                dstrxClearingToAutoVerify.Tables[0].Rows[i]["VoucherCode"] = new string('0', 2 - dstrxClearingToAutoVerify.Tables[0].Rows[i]["VoucherCode"].ToString().Trim().Length) + dstrxClearingToAutoVerify.Tables[0].Rows[i]["VoucherCode"].ToString();
                            }
                            if (dstrxClearingToAutoVerify.Tables[0].Rows[i]["VoucherCode"].ToString().Contains("!"))
                            {
                                dstrxClearingToAutoVerify.Tables[0].Rows[i]["VoucherCode"] = "01";
                            }
                        }
                        break;
                    case "AMOUNT":
                        for (int i = 0; i < dstrxClearingToAutoVerify.Tables[0].Rows.Count; i++)
                        {
                            if (dstrxClearingToAutoVerify.Tables[0].Rows[i]["Value"].ToString().Contains("-"))
                            {
                                dstrxClearingToAutoVerify.Tables[0].Rows[i]["Value"] = dstrxClearingToAutoVerify.Tables[0].Rows[i]["Value"].ToString().Substring(1);
                            }
                        }
                        break;
                    case "CURRENCYID":
                        string Country = ConfigurationManager.AppSettings["Country"].Trim().ToUpper();
                        switch (Country.ToUpper())
                        {
                            case "UG":
                                if (CurrencyCode == "UGX")
                                {
                                    for (int i = 0; i < dstrxClearingToAutoVerify.Tables[0].Rows.Count; i++)
                                    {
                                        dstrxClearingToAutoVerify.Tables[0].Rows[i]["CurrencyCode"] = 0;
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < dstrxClearingToAutoVerify.Tables[0].Rows.Count; i++)
                                    {
                                        dstrxClearingToAutoVerify.Tables[0].Rows[i]["CurrencyCode"] = 1;
                                    }
                                }
                                break;

                            case "KE":
                                if (CurrencyCode == "KES")
                                {
                                    for (int i = 0; i < dstrxClearingToAutoVerify.Tables[0].Rows.Count; i++)
                                    {
                                        dstrxClearingToAutoVerify.Tables[0].Rows[i]["CurrencyCode"] = 0;
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < dstrxClearingToAutoVerify.Tables[0].Rows.Count; i++)
                                    {
                                        dstrxClearingToAutoVerify.Tables[0].Rows[i]["CurrencyCode"] = 1;
                                    }
                                }
                                break;

                        }
                        break;
                }
            }
            dstrxClearingToAutoVerify.AcceptChanges();
            return dstrxClearingToAutoVerify;
        }
        public static DateTime SODDate(UserInfo usrInfo, string strOurBrachID)
        {
            DS_SystemBranchStatus dsSystemBranchStatus = GetSPSystemBranchStatus(usrInfo, strOurBrachID);
            if (dsSystemBranchStatus == null || (dsSystemBranchStatus.t_SystemBranchStatus.Rows.Count != 1))
                throw new NullReferenceException("Wrong HQ BranchID in the config file");
            return Convert.ToDateTime(dsSystemBranchStatus.t_SystemBranchStatus[0].SODDate);
        }

        static public DS_SystemBranchStatus GetSPSystemBranchStatus(UserInfo usrInfo, string strOurBranchID)
        {
            DS_SystemBranchStatus dsSystemBranchStatus = null;
            DS_SystemBranchStatus dsBranchStatuscache = new DS_SystemBranchStatus();
            try
            {
                using (IDbConnection connection = GetConnection())
                {
                    dsSystemBranchStatus = new DS_SystemBranchStatus();
                    IDBHelper intfDBHelper = DBClient.GetDBHelper(usrInfo);
                    IDataParameter[] arParms = intfDBHelper.CreateDBParamsArray(1);

                    arParms[0] = intfDBHelper.CreateNewDBParam("LanguageID", SqlDbType.VarChar, 3);
                    arParms[0].Value = usrInfo.strLanguage;

                    intfDBHelper.FillDataset(connection, CommandType.StoredProcedure, "pc_SystemBranchStatus", dsSystemBranchStatus, new string[] { "dt_SystemBranchStatus" }, arParms);
                    DataRow[] datarows = dsSystemBranchStatus.t_SystemBranchStatus.Select("OurBranchID='" + strOurBranchID + "'");
                    dsBranchStatuscache.Merge(datarows, false, MissingSchemaAction.Add);
                    return dsBranchStatuscache;
                }
            }
            catch (Exception ex)
            {
                throw DBClientUtils.GetDBErrorMessages(ex, usrInfo.strUser, usrInfo.strSystem);
            }
        }

        public static DS_SystemBranchSettings GetSystemBranchSettings(UserInfo usrInfo, string strOurBranchID, params object[] paramArray)
        {
            DS_SystemBranchSettings dsSystemBranchSettings = new DS_SystemBranchSettings();
            dsSystemBranchSettings = GetSystemBranchParameter(usrInfo, strOurBranchID, null);
            return dsSystemBranchSettings;
        }

        static public DS_SystemBranchSettings GetSystemBranchParameter(UserInfo usrInfo, string strOurBranchID, string strBankID)
        {

            DS_SystemBranchSettings dsSystemBranchSettings = new DS_SystemBranchSettings();
            DS_SystemBranchSettings dsBranchSettingCache = new DS_SystemBranchSettings();
            dsSystemBranchSettings = new DS_SystemBranchSettings();
            IDBHelper intfDBHelper = DBClient.GetDBHelper(usrInfo);
            IDataParameter[] arParms = intfDBHelper.CreateDBParamsArray(1);
            arParms[0] = intfDBHelper.CreateNewDBParam("BranchID", SqlDbType.NVarChar, 6);
            arParms[0].Value = usrInfo.strBranch;
            try
            {
                using (IDbConnection connection = GetConnection())
                {
                    intfDBHelper.FillDataset(connection, CommandType.StoredProcedure, "pc_SystemClearingBranchSettings", dsSystemBranchSettings, new string[] { "dt_SystemBranchSettings" }, arParms);
                    if (strOurBranchID != null)
                    {
                        DataRow[] datarows = dsSystemBranchSettings.t_SystemBranchSettings.Select("OurBranchID='" + strOurBranchID + "'");
                        if (datarows.Length > 0)
                        {
                            if (datarows[0]["BankID"].ToString() == usrInfo.strBank)
                                dsBranchSettingCache.Merge(datarows, false, MissingSchemaAction.Add);
                        }
                        return dsBranchSettingCache;
                    }
                    else if (strBankID != null)
                    {
                        DataRow[] datarows = dsSystemBranchSettings.t_SystemBranchSettings.Select("BankID='" + strBankID + "'");
                        if (datarows.Length > 0)
                        {
                            if (datarows[0]["BankID"].ToString() == usrInfo.strBank)
                                dsBranchSettingCache.Merge(datarows, false, MissingSchemaAction.Add);
                        }
                        return dsBranchSettingCache;
                    }
                    else

                        return dsSystemBranchSettings;
                }
            }
            catch (Exception ex)
            {
                throw DBClientUtils.GetDBErrorMessages(ex, usrInfo.strUser, usrInfo.strSystem);
            }
        }

        public static double RoundTo5Cents(double x)
        {
            double functionReturnValue = default(double);
            string CountryCode = ConfigurationManager.AppSettings["CountryCode"];
            if (CountryCode == "KE")
            {
                try
                {
                    x = BRBaseConvert.ConvertToDouble(string.Format("{0:0.##}", x));
                    x = x * 100;
                    x = BRBaseConvert.ConvertToInt32(x / 5);
                    // Get the Integer portion of the result
                    x = x * 5 / 100;
                    // Divide the result by 5 and multiply by 100
                    functionReturnValue = x;
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }
            else
            {
                functionReturnValue = x;
            }
            return functionReturnValue;
        }
        public bool ValidateChequeID(string ChqID)
        {
            bool valid = false;
            try
            {
                if (Microsoft.VisualBasic.Information.IsNumeric(ChqID))
                {
                    return valid = true;
                }
                else
                {
                    return valid = false;
                }
            }
            catch (Exception ex)
            {
                return valid = false;
            }
        }

    }

}
