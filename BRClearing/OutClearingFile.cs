using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using BREntities.ClearingFileFormat;
using BREntities.BRCreateClearingFile;
using System.Collections;
using System.Collections.Specialized;
using BRBase;
using System.Linq;
using System.Configuration;
using BR.ApplicationBlocks.Data;
using BR.DBClient;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using BRNetSecurity;

namespace BRClearing.Util
{

    public class OutClearingFile
    {
        private static string[] strCon = new string[4];
        private static string strDBServerName;
        private static string strDatabaseName;
        private static string strBRUserName;
        private static string strBRUserPassword;
        private static string DDFT;
        public static DataTable GenerateDirectDebitMandateNotifications(string FileType, string BankID, DS_trxClearing dstrxClearing, DateTime WorkingDate, BRBase.UserInfo usrInfo, IDbConnection conn, string[] conString, string DDMandateType = "")
        {
            DataTable FinalData = new DataTable();
            FinalData.Columns.Add("DDID", typeof(string));
            FinalData.Columns.Add("MandateType", typeof(string));
            FinalData.Columns.Add("text", typeof(string));
            FinalData.Columns.Add("FileName", typeof(string));
            return FinalData;
        }

        public static DataTable GenerateClearingFiles(string Currency, string FileType, string BankID, DS_trxClearing dstrxClearing, DS_ClearingFileFormat dsClearingFileFormat, BRBase.BRDataSet dsWithImages, DateTime WorkingDate, BRBase.UserInfo usrInfo, string Banks,string ClientName, IDbConnection conn, string[] conString, string DDMandateType = "", bool isMIPSEFTs =false)
        {
            string sFileType = FileType;
            strCon = conString;
            ReGetConnection(strCon);
            string dd = new string('0', 2 - BRBase.BRBaseConvert.ConvertToString(WorkingDate.Day).ToString().Length) + BRBase.BRBaseConvert.ConvertToString(WorkingDate.Day);
            string mm = new string('0', 2 - BRBase.BRBaseConvert.ConvertToString(WorkingDate.Month).Length) + BRBase.BRBaseConvert.ConvertToString(WorkingDate.Month);
            string yyyy = BRBase.BRBaseConvert.ConvertToString(WorkingDate.Year);
            string ddmmmyyyy = dd + mm + yyyy;

            DataTable FinalData = new DataTable();
            FinalData.Columns.Add("TrxRowID", typeof(string));
            FinalData.Columns.Add("text", typeof(string));
            FinalData.Columns.Add("FileName", typeof(string));
            FinalData.Columns.Add("ImageID", typeof(string));
            FinalData.Columns.Add("fcy", typeof(string));
            //FileType = "EJ";
            string Country = ConfigurationManager.AppSettings["Country"].Trim().ToUpper();
            //MessageBox.Show(Currency + " - " + BankID + " - " + WorkingDate + " - " + Banks );
             switch (Country.ToUpper())
             {
                
                 case "UG":
                     if (FileType == "EJ")
                     {
                         FinalData = ValidateEJDataTableUG(Currency, BankID, dstrxClearing, dsClearingFileFormat, dsWithImages, WorkingDate, usrInfo, Banks, conn);
                     }
                     else if (FileType == "EFT")
                     {
                         FinalData = GenerateEFTsUG(BankID, dstrxClearing, dsClearingFileFormat, WorkingDate, usrInfo, Banks, conn);
                     }
                     else if (FileType == "DD")
                     {
                        //MessageBox.Show  ("Now DDs - " + Banks + " -- " + BankID);
                         FinalData = GenerateDDsUG(BankID, dstrxClearing, dsClearingFileFormat, WorkingDate, usrInfo, Banks, conn);
                     }
                     else if (FileType == "DISC")
                     {
                         FinalData = GenerateDiscrepancyBodyUG(BankID, ddmmmyyyy, dstrxClearing, dsClearingFileFormat, Banks);
                     }
                     break;
                 case "KE":
                     if (sFileType != "MFI" && sFileType != "")
                     {
                         FileType = sFileType;
                     }
                     if (sFileType == "MFI")
                     {
                        BankID = sFileType;
                        FinalData = ValidateEJDataTable(Currency, BankID, dstrxClearing, dsClearingFileFormat, dsWithImages, WorkingDate, usrInfo, Banks, ClientName, conn, conString);
                     }
                     else if (FileType == "EJ")
                     {
                         FinalData = ValidateEJDataTable(Currency, BankID, dstrxClearing, dsClearingFileFormat, dsWithImages, WorkingDate, usrInfo, Banks, ClientName, conn, conString);
                     }
                     else if (FileType == "EFT" )
                     {
                         FinalData = GenerateEFTs(BankID, dstrxClearing, dsClearingFileFormat, WorkingDate, usrInfo, Banks, conn, conString, isMIPSEFTs);
                     }
                     else if (FileType == "DD")
                     {
                         FinalData = GenerateDDs(BankID, dstrxClearing, dsClearingFileFormat, WorkingDate, usrInfo, Banks, conn, DDMandateType, conString);
                     }
                     else if (FileType == "01")
                     {
                         FinalData = GenerateDDs(BankID, dstrxClearing, dsClearingFileFormat, WorkingDate, usrInfo, Banks, conn, "01", conString);
                     }
                     else if (FileType == "02")
                     {
                         FinalData = GenerateDDs(BankID, dstrxClearing, dsClearingFileFormat, WorkingDate, usrInfo, Banks, conn, "02", conString);
                     }
                     else if (FileType == "03")
                     {
                         FinalData = GenerateDDs(BankID, dstrxClearing, dsClearingFileFormat, WorkingDate, usrInfo, Banks, conn, "03", conString);
                     }
                     else if (FileType == "04")
                     {
                         FinalData = GenerateDDs(BankID, dstrxClearing, dsClearingFileFormat, WorkingDate, usrInfo, Banks, conn, "04", conString);
                     }
                     else if (FileType == "05")
                     {
                         FinalData = GenerateDDs(BankID, dstrxClearing, dsClearingFileFormat, WorkingDate, usrInfo, Banks, conn, "05", conString);
                     }
                     else if (FileType == "06")
                     {
                         FinalData = GenerateDDs(BankID, dstrxClearing, dsClearingFileFormat, WorkingDate, usrInfo, Banks, conn, "06", conString);
                     }
                     else if (FileType == "99")
                     {
                         FinalData = GenerateDDs(BankID, dstrxClearing, dsClearingFileFormat, WorkingDate, usrInfo, Banks, conn, "99", conString);
                     }
                     else if (FileType == "DISC")
                     {
                         //FinalData = GenerateDiscrepancyBody(BankID, ddmmmyyyy, dstrxClearing, dsClearingFileFormat, Banks);
                     }
                     break;
             }
            return FinalData;
        }

        public static BRBase.BRDataSet RetreveData(DateTime DateOfFilesToGenerate,IDbConnection conn)
        {
            BRBase.BRDataSet dsWithResults = new BRBase.BRDataSet();
            BRBase.UserInfo usrInfo = BRControls.ASPUtils.getActiveUser();
           BRClearing.Util.OutClearingFile.ClearingUniversalMethod(usrInfo, "p_RetreaveOutClearing", out dsWithResults, BRBase.BRModule.OutwardCredit,conn, new object[] { DateOfFilesToGenerate });
            return dsWithResults;
        }

        public static DataTable ValidateEJDataTable(string StrCurrencyID, string BankID, DS_trxClearing dstrxClearing, DS_ClearingFileFormat dsClearingFileFormat, BRBase.BRDataSet dsWithImages, DateTime WorkingDate, BRBase.UserInfo usrInfo, string Banks,string ClientName, IDbConnection conn, string[] conString)
        {
            string sBankID = BankID;
            if (sBankID == "MFI")
            {
                BankID = usrInfo.strBank;
            }
            Int16 i, j, k, m;
            Int32 t;
            string sortOrder = "Start ASC";
            string ControlVoucherType = string.Empty;
            string NewValue = string.Empty;
            string Value = string.Empty;
            string Data = string.Empty;
            string ToBank = string.Empty;
            string ImageID = string.Empty;
            Int32 IsFcy = 0;
            ToBank = BankID;
            bool Status = false;
            DataTable CompiledDataToBeWritten = new DataTable();
            DataTable FullyCompiledDataToBeWritten = new DataTable();
            DataTable SemiCompiledDataToBeWritten = new DataTable();
            CompiledDataToBeWritten.Columns.Add("TrxRowID", typeof(string));
            CompiledDataToBeWritten.Columns.Add("text", typeof(string));
            CompiledDataToBeWritten.Columns.Add("FileName", typeof(string));
            CompiledDataToBeWritten.Columns.Add("ImageID", typeof(string));
            CompiledDataToBeWritten.Columns.Add("fcy", typeof(string));

            FullyCompiledDataToBeWritten.Columns.Add("TrxRowID", typeof(string));
            FullyCompiledDataToBeWritten.Columns.Add("text", typeof(string));
            FullyCompiledDataToBeWritten.Columns.Add("FileName", typeof(string));
            FullyCompiledDataToBeWritten.Columns.Add("ImageID", typeof(string));
            FullyCompiledDataToBeWritten.Columns.Add("fcy", typeof(string));

            SemiCompiledDataToBeWritten.Columns.Add("TrxRowID", typeof(string));
            SemiCompiledDataToBeWritten.Columns.Add("text", typeof(string));
            SemiCompiledDataToBeWritten.Columns.Add("FileName", typeof(string));
            SemiCompiledDataToBeWritten.Columns.Add("ImageID", typeof(string));
            SemiCompiledDataToBeWritten.Columns.Add("fcy", typeof(string));

            DS_ClearingFileFormat WorkingDataTable = new DS_ClearingFileFormat();
            DS_trxClearing RejectDt = new DS_trxClearing();
            BRBase.BRDataSet Ds4Images = new BRBase.BRDataSet();
            DataTable EJdt = new DataTable();
            ArrayList arr = new ArrayList();
            if (StrCurrencyID!="KES")
            {
                IsFcy=1;
            }
            arr.Add("HEADER");
            arr.Add("EJ");
            arr.Add("TRAILER");
            for (i = 0; i < arr.Count; i++)
            {
                t = 0;
                DataRow[] drHeaderFileFormatResult = dsClearingFileFormat.Tables[0].Select("RecordType = '" + arr[i].ToString() + "' AND FileType='EJ'", sortOrder);
                foreach (DataRow dvr in drHeaderFileFormatResult)
                {
                    WorkingDataTable.Tables[0].ImportRow(dvr);
                }
                WorkingDataTable.AcceptChanges();
                Object[] arryRow = new Object[WorkingDataTable.Tables[0].Rows.Count];

                switch (arr[i].ToString())
                {
                    case "HEADER": // to work on Recieving Bank and Serial Nugu ber
                        foreach (DataColumn Col in WorkingDataTable.Tables[0].Columns)
                        {
                            switch (Col.ColumnName.ToUpper())
                            {
                                case "FIELDNAME":
                                    try
                                    {
                                        for (j = 0; j < WorkingDataTable.Tables[0].Rows.Count; j++)
                                        {
                                            //Get the Required Length
                                            string FieldNm = string.Empty;
                                            FieldNm = WorkingDataTable.Tables[0].Rows[j][Col.ColumnName].ToString();
                                            Int32 FileFormatValue = BRBase.BRBaseConvert.ConvertToInt32(WorkingDataTable.Tables[0].Rows[j]["Length"].ToString());
                                            bool FileFormatValueMandatoryLength = WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString() == "" ? false : BRBase.BRBaseConvert.ConvertToBoolean(WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString());
                                            string Filler = WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString() ==""? "0" : WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString();
                                            Value = BRBase.BRBaseConvert.ConvertToString(WorkingDataTable.Tables[0].Rows[j]["DEFAULTVALUES"].ToString().Trim());
                                            switch (FieldNm.ToString().ToUpper().Trim())
                                            {
                                                case "RECORDTYPE":
                                                case "FILETYPE":
                                                case "RBANK":
                                                case "PORGANISATION":
                                                case "RORGANISATION":
                                                case "FILEINDICATOR":
                                                case "PBANK":
                                                    arryRow[t] = Value;
                                                    t = t + 1;
                                                    break;
                                                case "FILLER":
                                                    Filler = new string(BRBase.BRBaseConvert.ConvertToChar(Filler), FileFormatValue);
                                                    arryRow[t] = Filler;
                                                    t = t + 1;
                                                    break;
                                                case "DATE":
                                                    string dd = new string('0', 2 - BRBase.BRBaseConvert.ConvertToString(WorkingDate.Day).ToString().Length) + BRBase.BRBaseConvert.ConvertToString(WorkingDate.Day);
                                                    string mm = new string('0', 2 - BRBase.BRBaseConvert.ConvertToString(WorkingDate.Month).Length) + BRBase.BRBaseConvert.ConvertToString(WorkingDate.Month);
                                                    string yyyy = BRBase.BRBaseConvert.ConvertToString(WorkingDate.Year);
                                                    string ddmmmyyyy = dd + mm + yyyy;
                                                    arryRow[t] = ddmmmyyyy;
                                                    t = t + 1;
                                                    break;
                                                case "PCLEARINGCENTRE":
                                                    arryRow[t] = usrInfo.strBank;
                                                    t = t + 1;
                                                    break;
                                                case "RCLEARINGCENTRE":
                                                    arryRow[t] = ToBank;
                                                    t = t + 1;
                                                    break;
                                                case "SERIALNUMBER": // To work on this
                                                    dd = new string('0', 2 - BRBase.BRBaseConvert.ConvertToString(WorkingDate.Day).ToString().Length) + BRBase.BRBaseConvert.ConvertToString(WorkingDate.Day);
                                                    mm = new string('0', 2 - BRBase.BRBaseConvert.ConvertToString(WorkingDate.Month).Length) + BRBase.BRBaseConvert.ConvertToString(WorkingDate.Month);
                                                    arryRow[t] = dd + mm + ToBank + "01";
                                                    t = t + 1;
                                                    break;
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        ex.ToString();
                                    }
                                    break;
                            }
                        }
                        Data = string.Empty;
                        for (m = 0; m < arryRow.Length; m++)
                        {
                            Data = Data + (arryRow[m].ToString());
                        }
                        CompiledDataToBeWritten.Rows.Add("0", Data, "", "0", IsFcy);
                        FullyCompiledDataToBeWritten.Merge(CompiledDataToBeWritten);
                        break;

                    case "TRAILER":
                        foreach (DataColumn Col in WorkingDataTable.Tables[0].Columns)
                        {
                            switch (Col.ColumnName.ToUpper())
                            {
                                case "FIELDNAME":
                                    for (j = 0; j < WorkingDataTable.Tables[0].Rows.Count; j++)
                                    {
                                        string FieldNm = string.Empty;
                                        FieldNm = WorkingDataTable.Tables[0].Rows[j][Col.ColumnName].ToString();
                                        Int32 FileFormatValue = BRBase.BRBaseConvert.ConvertToInt32(WorkingDataTable.Tables[0].Rows[j]["Length"].ToString());
                                        bool FileFormatValueMandatoryLength = WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString() == "" ? false : BRBase.BRBaseConvert.ConvertToBoolean(WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString());
                                        string Filler = WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString() == "" ? "0" : WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString();
                                        Value = BRBase.BRBaseConvert.ConvertToString(WorkingDataTable.Tables[0].Rows[j]["DEFAULTVALUES"].ToString().Trim());
                                        switch (FieldNm.ToString().ToUpper().Trim())
                                        {
                                            case "RECORDTYPE":
                                            case "PORGANISATION":
                                            case "PBANK":
                                                arryRow[t] = Value;
                                                t = t + 1;
                                                break;
                                            case "PTOTALVALUEDEBIT":
                                                arryRow[t] = ClearingValidations.CalculateEJTotalDebit(dstrxClearing, Filler, FileFormatValue, Banks);
                                                t = t + 1;
                                                break;
                                            case "FILLER":
                                                Filler = new string(BRBase.BRBaseConvert.ConvertToChar(Filler), FileFormatValue);
                                                arryRow[t] = Filler;
                                                t = t + 1;
                                                break;
                                            case "PCLEARINGCENTRE":
                                                arryRow[t] = usrInfo.strBank;
                                                t = t + 1;
                                                break;
                                            case "PTRANSACTIONCOUNT":
                                                arryRow[t] = ClearingValidations.TotalCountEJs(dstrxClearing, Filler, FileFormatValue, StrCurrencyID, ToBank);
                                                t = t + 1;
                                                break;
                                            case "PTOTALVALUECREDITS":
                                                arryRow[t] = ClearingValidations.CalculateEJTotalCredit(dstrxClearing, Filler, FileFormatValue, Banks);
                                                t = t + 1;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        Data = string.Empty;
                        for (m = 0; m < arryRow.Length; m++)
                        {
                            Data = Data + (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
                        }
                        FullyCompiledDataToBeWritten.Rows.Add("0", Data, "", "0", IsFcy);
                        break;

                    case "EJ":
                        if (sBankID == "MFI")
                        {
                            ToBank = sBankID;
                        }
                        if (StrCurrencyID != "KES")
                        {
                            //60
                            if ((GenerateEJs("BCV", dstrxClearing, dsClearingFileFormat, ToBank, "USD", dsWithImages, WorkingDate, usrInfo, out CompiledDataToBeWritten, Banks, ClientName, conn, conString) == true)
                                ||
                                (GenerateEJs("MDV", dstrxClearing, dsClearingFileFormat, ToBank, "USD", dsWithImages, WorkingDate, usrInfo, out CompiledDataToBeWritten, Banks, ClientName, conn, conString) == true)
                                ||
                                (GenerateEJs("UCV", dstrxClearing, dsClearingFileFormat, ToBank, "USD", dsWithImages, WorkingDate, usrInfo, out CompiledDataToBeWritten, Banks, ClientName, conn, conString) == true))
                                {
                                    Data = LeadingControlVoucher(dstrxClearing, dsClearingFileFormat, ToBank, "EJ", WorkingDate, usrInfo, "BCV", "USD",conn);
                                    FullyCompiledDataToBeWritten.Rows.Add("0", Data, "", "0", IsFcy);
                                    //Presentments
                                    Status = GenerateEJs("BCV", dstrxClearing, dsClearingFileFormat, ToBank, "USD", dsWithImages, WorkingDate, usrInfo, out CompiledDataToBeWritten, Banks, ClientName, conn, conString);
                                    if (Status == true)
                                    {
                                        Status = GenerateEJs("BCV", dstrxClearing, dsClearingFileFormat, ToBank, "USD", dsWithImages, WorkingDate, usrInfo, out SemiCompiledDataToBeWritten, Banks, ClientName, conn, conString);
                                        FullyCompiledDataToBeWritten.Merge(SemiCompiledDataToBeWritten);
                                        Data = TrailerControlVoucher(dstrxClearing, dsClearingFileFormat, "BCV", ToBank, "EJ", WorkingDate, usrInfo, "USD",conn,Banks );
                                        FullyCompiledDataToBeWritten.Rows.Add("0", Data, "", "0", IsFcy);
                                        SemiCompiledDataToBeWritten = new DataTable();
                                    }

                                    //MDV
                                    Status = GenerateEJs("MDV", dstrxClearing, dsClearingFileFormat, ToBank, "USD", dsWithImages, WorkingDate, usrInfo, out CompiledDataToBeWritten, Banks, ClientName, conn, conString);
                                    if (Status == true)
                                    {
                                        Status = GenerateEJs("MDV", dstrxClearing, dsClearingFileFormat, ToBank, "USD", dsWithImages, WorkingDate, usrInfo, out SemiCompiledDataToBeWritten, Banks, ClientName, conn, conString);
                                        FullyCompiledDataToBeWritten.Merge(SemiCompiledDataToBeWritten);
                                        Data = TrailerControlVoucher(dstrxClearing, dsClearingFileFormat, "MDV", ToBank, "EJ", WorkingDate, usrInfo, "USD",conn,Banks);
                                        FullyCompiledDataToBeWritten.Rows.Add("0", Data, "", "0", IsFcy);
                                        SemiCompiledDataToBeWritten = new DataTable();
                                    }

                                    //Unpaids
                                    Status = GenerateEJs("UCV", dstrxClearing, dsClearingFileFormat, ToBank, "USD", dsWithImages, WorkingDate, usrInfo, out CompiledDataToBeWritten, Banks, ClientName, conn, conString);
                                    if (Status == true)
                                    {
                                        Status = GenerateEJs("UCV", dstrxClearing, dsClearingFileFormat, ToBank, "USD", dsWithImages, WorkingDate, usrInfo, out SemiCompiledDataToBeWritten, Banks, ClientName, conn, conString);
                                        FullyCompiledDataToBeWritten.Merge(SemiCompiledDataToBeWritten);
                                        Data = TrailerControlVoucher(dstrxClearing, dsClearingFileFormat, "UCV", ToBank, "EJ", WorkingDate, usrInfo, "USD",conn,Banks);
                                        FullyCompiledDataToBeWritten.Rows.Add("0", Data, "", "0", IsFcy);
                                        SemiCompiledDataToBeWritten = new DataTable();
                                    }
                                }
                            //61
                            if ((GenerateEJs("BCV", dstrxClearing, dsClearingFileFormat, ToBank, "GBP", dsWithImages, WorkingDate, usrInfo, out CompiledDataToBeWritten, Banks, ClientName, conn, conString) == true)
                                ||
                                (GenerateEJs("MDV", dstrxClearing, dsClearingFileFormat, ToBank, "GBP", dsWithImages, WorkingDate, usrInfo, out CompiledDataToBeWritten, Banks, ClientName, conn, conString) == true)
                                ||
                                (GenerateEJs("UCV", dstrxClearing, dsClearingFileFormat, ToBank, "GBP", dsWithImages, WorkingDate, usrInfo, out CompiledDataToBeWritten, Banks, ClientName, conn, conString) == true))
                                {
                                    Data = LeadingControlVoucher(dstrxClearing, dsClearingFileFormat, ToBank, "EJ", WorkingDate, usrInfo, "BCV", "GBP",conn);
                                    FullyCompiledDataToBeWritten.Rows.Add("0", Data, "", "0", IsFcy);
                                    //Presentments
                                    Status = GenerateEJs("BCV", dstrxClearing, dsClearingFileFormat, ToBank, "GBP", dsWithImages, WorkingDate, usrInfo, out CompiledDataToBeWritten, Banks, ClientName, conn, conString);
                                    if (Status == true)
                                    {
                                        Status = GenerateEJs("BCV", dstrxClearing, dsClearingFileFormat, ToBank, "GBP", dsWithImages, WorkingDate, usrInfo, out SemiCompiledDataToBeWritten, Banks, ClientName, conn, conString);
                                        FullyCompiledDataToBeWritten.Merge(SemiCompiledDataToBeWritten);
                                        Data = TrailerControlVoucher(dstrxClearing, dsClearingFileFormat, "BCV", ToBank, "EJ", WorkingDate, usrInfo, "GBP",conn,Banks);
                                        FullyCompiledDataToBeWritten.Rows.Add("0", Data, "", "0", IsFcy);
                                        SemiCompiledDataToBeWritten = new DataTable();
                                    }

                                    //MDV
                                    Status = GenerateEJs("MDV", dstrxClearing, dsClearingFileFormat, ToBank, "GBP", dsWithImages, WorkingDate, usrInfo, out CompiledDataToBeWritten, Banks, ClientName, conn, conString);
                                    if (Status == true)
                                    {
                                        Status = GenerateEJs("MDV", dstrxClearing, dsClearingFileFormat, ToBank, "GBP", dsWithImages, WorkingDate, usrInfo, out SemiCompiledDataToBeWritten, Banks, ClientName, conn, conString);
                                        FullyCompiledDataToBeWritten.Merge(SemiCompiledDataToBeWritten);
                                        Data = TrailerControlVoucher(dstrxClearing, dsClearingFileFormat, "MDV", ToBank, "EJ", WorkingDate, usrInfo, "GBP",conn,Banks);
                                        FullyCompiledDataToBeWritten.Rows.Add("0", Data, "", "0", IsFcy);
                                        SemiCompiledDataToBeWritten = new DataTable();
                                    }

                                    //Unpaids
                                    Status = GenerateEJs("UCV", dstrxClearing, dsClearingFileFormat, ToBank, "GBP", dsWithImages, WorkingDate, usrInfo, out CompiledDataToBeWritten, Banks, ClientName, conn, conString);
                                    if (Status == true)
                                    {
                                        Status = GenerateEJs("UCV", dstrxClearing, dsClearingFileFormat, ToBank, "GBP", dsWithImages, WorkingDate, usrInfo, out SemiCompiledDataToBeWritten, Banks, ClientName, conn, conString);
                                        FullyCompiledDataToBeWritten.Merge(SemiCompiledDataToBeWritten);
                                        Data = TrailerControlVoucher(dstrxClearing, dsClearingFileFormat, "UCV", ToBank, "EJ", WorkingDate, usrInfo, "GBP",conn,Banks);
                                        FullyCompiledDataToBeWritten.Rows.Add("0", Data, "", "0", IsFcy);
                                        SemiCompiledDataToBeWritten = new DataTable();
                                    }
                                }
                            //62
                            if ((GenerateEJs("BCV", dstrxClearing, dsClearingFileFormat, ToBank, "EUR", dsWithImages, WorkingDate, usrInfo, out CompiledDataToBeWritten, Banks, ClientName, conn, conString) == true)
                                ||
                                (GenerateEJs("MDV", dstrxClearing, dsClearingFileFormat, ToBank, "EUR", dsWithImages, WorkingDate, usrInfo, out CompiledDataToBeWritten, Banks, ClientName, conn, conString) == true)
                                ||
                                (GenerateEJs("UCV", dstrxClearing, dsClearingFileFormat, ToBank, "EUR", dsWithImages, WorkingDate, usrInfo, out CompiledDataToBeWritten, Banks, ClientName, conn, conString) == true))
                                {
                                    Data = LeadingControlVoucher(dstrxClearing, dsClearingFileFormat, ToBank, "EJ", WorkingDate, usrInfo, "BCV", "EUR",conn);
                                    FullyCompiledDataToBeWritten.Rows.Add("0", Data, "", "0", IsFcy);
                                    //Presentments
                                    Status = GenerateEJs("BCV", dstrxClearing, dsClearingFileFormat, ToBank, "EUR", dsWithImages, WorkingDate, usrInfo, out CompiledDataToBeWritten, Banks, ClientName, conn, conString);
                                    if (Status == true)
                                    {
                                        Status = GenerateEJs("BCV", dstrxClearing, dsClearingFileFormat, ToBank, "EUR", dsWithImages, WorkingDate, usrInfo, out SemiCompiledDataToBeWritten, Banks, ClientName, conn, conString);
                                        FullyCompiledDataToBeWritten.Merge(SemiCompiledDataToBeWritten);
                                        Data = TrailerControlVoucher(dstrxClearing, dsClearingFileFormat, "BCV", ToBank, "EJ", WorkingDate, usrInfo, "EUR",conn, Banks);
                                        FullyCompiledDataToBeWritten.Rows.Add("0", Data, "", "0", IsFcy);
                                        SemiCompiledDataToBeWritten = new DataTable();
                                    }

                                    //MDV
                                    Status = GenerateEJs("MDV", dstrxClearing, dsClearingFileFormat, ToBank, "EUR", dsWithImages, WorkingDate, usrInfo, out CompiledDataToBeWritten, Banks, ClientName, conn, conString);
                                    if (Status == true)
                                    {
                                        Status = GenerateEJs("MDV", dstrxClearing, dsClearingFileFormat, ToBank, "EUR", dsWithImages, WorkingDate, usrInfo, out SemiCompiledDataToBeWritten, Banks, ClientName, conn, conString);
                                        FullyCompiledDataToBeWritten.Merge(SemiCompiledDataToBeWritten);
                                        Data = TrailerControlVoucher(dstrxClearing, dsClearingFileFormat, "MDV", ToBank, "EJ", WorkingDate, usrInfo, "EUR",conn,Banks);
                                        FullyCompiledDataToBeWritten.Rows.Add("0", Data, "", "0", IsFcy);
                                        SemiCompiledDataToBeWritten = new DataTable();
                                    }

                                    //Unpaids
                                    Status = GenerateEJs("UCV", dstrxClearing, dsClearingFileFormat, ToBank, "EUR", dsWithImages, WorkingDate, usrInfo, out CompiledDataToBeWritten, Banks, ClientName, conn, conString);
                                    if (Status == true)
                                    {
                                        Status = GenerateEJs("UCV", dstrxClearing, dsClearingFileFormat, ToBank, "EUR", dsWithImages, WorkingDate, usrInfo, out SemiCompiledDataToBeWritten, Banks, ClientName, conn, conString);
                                        FullyCompiledDataToBeWritten.Merge(SemiCompiledDataToBeWritten);
                                        Data = TrailerControlVoucher(dstrxClearing, dsClearingFileFormat, "UCV", ToBank, "EJ", WorkingDate, usrInfo, "EUR",conn, Banks);
                                        FullyCompiledDataToBeWritten.Rows.Add("0", Data, "", "0", IsFcy);
                                        SemiCompiledDataToBeWritten = new DataTable();
                                    }
                                }
                        }
                        else
                        {
                            Data = LeadingControlVoucher(dstrxClearing, dsClearingFileFormat, ToBank, "EJ", WorkingDate, usrInfo, "BCV", StrCurrencyID,conn);
                            FullyCompiledDataToBeWritten.Rows.Add("0", Data, "", "0", IsFcy);
                            //Presentments
                            Status = GenerateEJs("BCV", dstrxClearing, dsClearingFileFormat, ToBank, StrCurrencyID, dsWithImages, WorkingDate, usrInfo, out CompiledDataToBeWritten, Banks, ClientName, conn, conString);
                            if (Status == true)
                            {
                                Status = GenerateEJs("BCV", dstrxClearing, dsClearingFileFormat, ToBank, StrCurrencyID, dsWithImages, WorkingDate, usrInfo, out SemiCompiledDataToBeWritten, Banks, ClientName, conn, conString);
                                FullyCompiledDataToBeWritten.Merge(SemiCompiledDataToBeWritten);
                                Data = TrailerControlVoucher(dstrxClearing, dsClearingFileFormat, "BCV", ToBank, "EJ", WorkingDate, usrInfo, StrCurrencyID,conn, Banks);
                                FullyCompiledDataToBeWritten.Rows.Add("0", Data, "", "0", IsFcy);
                                SemiCompiledDataToBeWritten = new DataTable();
                            }

                            //MDV
                            Status = GenerateEJs("MDV", dstrxClearing, dsClearingFileFormat, ToBank, StrCurrencyID, dsWithImages, WorkingDate, usrInfo, out CompiledDataToBeWritten, Banks, ClientName, conn, conString);
                            if (Status == true)
                            {
                                Status = GenerateEJs("MDV", dstrxClearing, dsClearingFileFormat, ToBank, StrCurrencyID, dsWithImages, WorkingDate, usrInfo, out SemiCompiledDataToBeWritten, Banks, ClientName, conn, conString);
                                FullyCompiledDataToBeWritten.Merge(SemiCompiledDataToBeWritten);
                                Data = TrailerControlVoucher(dstrxClearing, dsClearingFileFormat, "MDV", ToBank, "EJ", WorkingDate, usrInfo, StrCurrencyID,conn, Banks);
                                FullyCompiledDataToBeWritten.Rows.Add("0", Data, "", "0", IsFcy);
                                SemiCompiledDataToBeWritten = new DataTable();
                            }

                            //Unpaids
                            Status = GenerateEJs("UCV", dstrxClearing, dsClearingFileFormat, ToBank, StrCurrencyID, dsWithImages, WorkingDate, usrInfo, out CompiledDataToBeWritten, Banks, ClientName, conn, conString);
                            if (Status == true)
                            {
                                Status = GenerateEJs("UCV", dstrxClearing, dsClearingFileFormat, ToBank, StrCurrencyID, dsWithImages, WorkingDate, usrInfo, out SemiCompiledDataToBeWritten, Banks, ClientName, conn, conString);
                                FullyCompiledDataToBeWritten.Merge(SemiCompiledDataToBeWritten);
                                Data = TrailerControlVoucher(dstrxClearing, dsClearingFileFormat, "UCV", ToBank, "EJ", WorkingDate, usrInfo, StrCurrencyID,conn, Banks);
                                FullyCompiledDataToBeWritten.Rows.Add("0", Data, "", "0", IsFcy);
                                SemiCompiledDataToBeWritten = new DataTable();
                            }
                        }
                        break;
                }
                WorkingDataTable.Tables[0].Rows.Clear();
            }
            return FullyCompiledDataToBeWritten;
        }
        public static DataTable ValidateEJDataTableUG(string StrCurrencyID, string BankID, DS_trxClearing dstrxClearing, DS_ClearingFileFormat dsClearingFileFormat, BRBase.BRDataSet dsWithImages, DateTime WorkingDate, BRBase.UserInfo usrInfo, string Banks, IDbConnection conn)
        {
            Int16 i, j, k, m;
            Int32 t;

            //MessageBox.Show("Imengia hapa");
            //string[] storedconn = conn;
            string ControlVoucherType = string.Empty;
            string NewValue = string.Empty;
            string Value = string.Empty;
            string Data = string.Empty;
            string ToBank = string.Empty;
            string ImageID = string.Empty;
            Int32 IsFcy = 0;
            ToBank = BankID;
            string sortOrder = "Start ASC";
            bool Status = false;
            DataTable CompiledDataToBeWritten = new DataTable();
            DataTable FullyCompiledDataToBeWritten = new DataTable();
            DataTable SemiCompiledDataToBeWritten = new DataTable();
            CompiledDataToBeWritten.Columns.Add("TrxRowID", typeof(string));
            CompiledDataToBeWritten.Columns.Add("text", typeof(string));
            CompiledDataToBeWritten.Columns.Add("FileName", typeof(string));
            CompiledDataToBeWritten.Columns.Add("ImageID", typeof(string));
            CompiledDataToBeWritten.Columns.Add("fcy", typeof(string));

            FullyCompiledDataToBeWritten.Columns.Add("TrxRowID", typeof(string));
            FullyCompiledDataToBeWritten.Columns.Add("text", typeof(string));
            FullyCompiledDataToBeWritten.Columns.Add("FileName", typeof(string));
            FullyCompiledDataToBeWritten.Columns.Add("ImageID", typeof(string));
            FullyCompiledDataToBeWritten.Columns.Add("fcy", typeof(string));

            SemiCompiledDataToBeWritten.Columns.Add("TrxRowID", typeof(string));
            SemiCompiledDataToBeWritten.Columns.Add("text", typeof(string));
            SemiCompiledDataToBeWritten.Columns.Add("FileName", typeof(string));
            SemiCompiledDataToBeWritten.Columns.Add("ImageID", typeof(string));
            SemiCompiledDataToBeWritten.Columns.Add("fcy", typeof(string));

            DS_ClearingFileFormat WorkingDataTable = new DS_ClearingFileFormat();
            DS_trxClearing RejectDt = new DS_trxClearing();
            BRBase.BRDataSet Ds4Images = new BRBase.BRDataSet();
            DataTable EJdt = new DataTable();
            ArrayList arr = new ArrayList();
            //MessageBox.Show("1");
            if (StrCurrencyID != "UGX")
            {
                IsFcy = 1;
            }
            arr.Add("HEADER");
            arr.Add("EJ");
            arr.Add("TRAILER");
            //MessageBox.Show("2");
            for (i = 0; i < arr.Count; i++)
            {
                t = 0;
                //MessageBox.Show("3");
                DataRow[] drHeaderFileFormatResult = dsClearingFileFormat.Tables[0].Select("RecordType = '" + arr[i].ToString() + "' AND FileType='EJ'", sortOrder);
                foreach (DataRow dvr in drHeaderFileFormatResult)
                {
                    WorkingDataTable.Tables[0].ImportRow(dvr);
                }
                WorkingDataTable.AcceptChanges();
                //MessageBox.Show("4");
                Object[] arryRow = new Object[WorkingDataTable.Tables[0].Rows.Count];
                //MessageBox.Show("5");
                switch (arr[i].ToString())
                {
                    case "HEADER": // to work on Recieving Bank and Serial Nugu ber
                        //MessageBox.Show("6");
                        arryRow = new Object[11];
                        //MessageBox.Show("7");
                        foreach (DataColumn Col in WorkingDataTable.Tables[0].Columns)
                        {
                            //MessageBox.Show("8");
                            switch (Col.ColumnName.ToUpper())
                            {
                                case "FIELDNAME":
                                    try
                                    {
                                        //MessageBox.Show(WorkingDataTable.Tables[0].Rows.Count.ToString());
                                        for (j = 0; j < WorkingDataTable.Tables[0].Rows.Count; j++)
                                        {
                                            //Get the Required Length
                                            if (conn.State != ConnectionState.Open)
                                            {
                                                conn = GetConnection();
                                            }
                                            //MessageBox.Show("10");
                                            string FieldNm = string.Empty;
                                            FieldNm = WorkingDataTable.Tables[0].Rows[j][Col.ColumnName].ToString();
                                            //MessageBox.Show(FieldNm);
                                            Int32 FileFormatValue = BRBase.BRBaseConvert.ConvertToInt32(WorkingDataTable.Tables[0].Rows[j]["Length"].ToString());
                                            bool FileFormatValueMandatoryLength = WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString() == "" ? false : BRBase.BRBaseConvert.ConvertToBoolean(WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString());
                                            string Filler = WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString() == "" ? "0" : WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString();
                                            Value = BRBase.BRBaseConvert.ConvertToString(WorkingDataTable.Tables[0].Rows[j]["DEFAULTVALUES"].ToString().Trim());
                                            switch (FieldNm.ToString().ToUpper().Trim())
                                            {
                                                case "RECORDTYPE":
                                                case "FILETYPE":
                                                //case "RBANK":
                                                case "PORGANISATION":
                                                case "RORGANISATION":
                                               
                                                    ////ValueLength = BRBase.BRBaseConvert.ConvertToInt32(dr[fieldNm].ToString().Length);
                                                    //Value = BRBase.BRBaseConvert.ConvertToString(dr[fieldNm].ToString().Trim());
                                                    arryRow[t] = Value;
                                                    t = t + 1;
                                                    break;
                                                case "RBANK":
                                                    arryRow[t] = "";
                                                    t = t + 1;
                                                    break;
                                                case "FILEINDICATOR":
                                                    arryRow[t] = Value;
                                                    t = t + 1;
                                                    arryRow[t] = Value;
                                                    t = t + 1;
                                                    if (StrCurrencyID != "UGX")
                                                    {
                                                        arryRow[t] = Value;
                                                        t = t + 1;
                                                    }
                                                    else
                                                    {
                                                        if (StrCurrencyID == "USD")
                                                        {
                                                            arryRow[t] = "1";
                                                            t = t + 1;
                                                        }
                                                        else if (StrCurrencyID == "EUR")
                                                        {
                                                            arryRow[t] = "2";
                                                            t = t + 1;
                                                        }
                                                        else if (StrCurrencyID == "GBP")
                                                        {
                                                            arryRow[t] = "3";
                                                            t = t + 1;
                                                        }
                                                        else if (StrCurrencyID != "KES")
                                                        {
                                                            arryRow[t] = "4";
                                                            t = t + 1;
                                                        }
                                                        
                                                    }
                                                    break;
                                                case "FILLER":
                                                    Filler = new string(BRBase.BRBaseConvert.ConvertToChar(Filler), FileFormatValue);
                                                    arryRow[t] = Filler;
                                                    t = t + 1;
                                                    break;
                                                case "DATE":
                                                    string dd = new string('0', 2 - BRBase.BRBaseConvert.ConvertToString(WorkingDate.Day).ToString().Length) + BRBase.BRBaseConvert.ConvertToString(WorkingDate.Day);
                                                    string mm = new string('0', 2 - BRBase.BRBaseConvert.ConvertToString(WorkingDate.Month).Length) + BRBase.BRBaseConvert.ConvertToString(WorkingDate.Month);
                                                    string yyyy = BRBase.BRBaseConvert.ConvertToString(WorkingDate.Year);
                                                    string ddmmmyyyy = dd + mm + yyyy;
                                                    arryRow[t] = ddmmmyyyy;
                                                    t = t + 1;
                                                    break;
                                                case "PBANK":
                                                    arryRow[t] = usrInfo.strBank;
                                                    t = t + 1;
                                                    break;
                                                case "PCLEARINGCENTRE":
                                                    arryRow[t] = usrInfo.strBank;
                                                    t = t + 1;
                                                    break;
                                                case "RCLEARINGCENTRE":
                                                    arryRow[t] = usrInfo.strBank;
                                                    //arryRow[t] = ToBank;
                                                    t = t + 1;
                                                    break;
                                                case "SERIALNUMBER": // To work on this
                                                    dd = new string('0', 2 - BRBase.BRBaseConvert.ConvertToString(WorkingDate.Day).ToString().Length) + BRBase.BRBaseConvert.ConvertToString(WorkingDate.Day);
                                                    mm = new string('0', 2 - BRBase.BRBaseConvert.ConvertToString(WorkingDate.Month).Length) + BRBase.BRBaseConvert.ConvertToString(WorkingDate.Month);
                                                    arryRow[t] = dd + mm + ToBank + "01";
                                                    t = t + 1;
                                                    break;
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        ex.ToString();
                                    }
                                    break;
                            }
                        }
                        Data = string.Empty;
                        for (m = 0; m < arryRow.Length; m++)
                        {
                            Data = Data + (arryRow[m].ToString());
                        }
                        CompiledDataToBeWritten.Rows.Add("0", Data, "", "0", IsFcy);
                        //MessageBox.Show("Imemaliza Header");
                        FullyCompiledDataToBeWritten.Merge(CompiledDataToBeWritten);
                        break;

                    case "TRAILER":
                        foreach (DataColumn Col in WorkingDataTable.Tables[0].Columns)
                        {
                            switch (Col.ColumnName.ToUpper())
                            {
                                case "FIELDNAME":
                                    for (j = 0; j < WorkingDataTable.Tables[0].Rows.Count; j++)
                                    {
                                        if (conn.State != ConnectionState.Open)
                                        {
                                            conn = GetConnection();
                                        }
                                        string FieldNm = string.Empty;
                                        FieldNm = WorkingDataTable.Tables[0].Rows[j][Col.ColumnName].ToString();
                                        Int32 FileFormatValue = BRBase.BRBaseConvert.ConvertToInt32(WorkingDataTable.Tables[0].Rows[j]["Length"].ToString());
                                        bool FileFormatValueMandatoryLength = WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString() == "" ? false : BRBase.BRBaseConvert.ConvertToBoolean(WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString());
                                        string Filler = WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString() == "" ? "0" : WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString();
                                        Value = BRBase.BRBaseConvert.ConvertToString(WorkingDataTable.Tables[0].Rows[j]["DEFAULTVALUES"].ToString().Trim());
                                        switch (FieldNm.ToString().ToUpper().Trim())
                                        {
                                            case "RECORDTYPE":
                                            case "PORGANISATION":
                                            case "PBANK":
                                                arryRow[t] = Value;
                                                t = t + 1;
                                                break;
                                            case "PTOTALVALUEDEBIT":
                                                arryRow[t] = ClearingValidations.CalculateEJTotalDebit(dstrxClearing, Filler, FileFormatValue, Banks);
                                                t = t + 1;
                                                break;
                                            case "FILLER":
                                                Filler = new string(BRBase.BRBaseConvert.ConvertToChar(Filler), FileFormatValue);
                                                arryRow[t] = Filler;
                                                t = t + 1;
                                                break;
                                            case "PCLEARINGCENTRE":
                                                arryRow[t] = usrInfo.strBank;
                                                t = t + 1;
                                                break;
                                            case "PTRANSACTIONCOUNT":
                                                arryRow[t] = ClearingValidations.TotalCountEJs(dstrxClearing, Filler, FileFormatValue, StrCurrencyID, ToBank);
                                                t = t + 1;
                                                break;
                                            case "PTOTALVALUECREDITS":
                                                arryRow[t] = ClearingValidations.CalculateEJTotalCredit(dstrxClearing, Filler, FileFormatValue, Banks);
                                                t = t + 1;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        Data = string.Empty;
                        for (m = 0; m < arryRow.Length; m++)
                        {
                            Data = Data + (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
                        }
                        FullyCompiledDataToBeWritten.Rows.Add("0", Data, "", "0", IsFcy);
                        //MessageBox.Show("Imemaliza Trailer");
                        break;

                    case "EJ":
                        //if (StrCurrencyID != "UGX")
                        //{
                        //    //60
                        //    if ((GenerateEJsUG("BCV", dstrxClearing, dsClearingFileFormat, ToBank, "USD", dsWithImages, WorkingDate, usrInfo, out CompiledDataToBeWritten, Banks,conn) == true)
                        //        ||
                        //        (GenerateEJsUG("MDV", dstrxClearing, dsClearingFileFormat, ToBank, "USD", dsWithImages, WorkingDate, usrInfo, out CompiledDataToBeWritten, Banks,conn ) == true)
                        //        ||
                        //        (GenerateEJsUG("UCV", dstrxClearing, dsClearingFileFormat, ToBank, "USD", dsWithImages, WorkingDate, usrInfo, out CompiledDataToBeWritten, Banks, conn) == true))
                        //    {
                        //        Data = LeadingControlVoucherUG(dstrxClearing, dsClearingFileFormat, ToBank, "EJ", WorkingDate, usrInfo, "BCV", "USD", conn);
                        //        FullyCompiledDataToBeWritten.Rows.Add("0", Data, "", "0", IsFcy);
                        //        //Presentments
                        //        Status = GenerateEJsUG("BCV", dstrxClearing, dsClearingFileFormat, ToBank, "USD", dsWithImages, WorkingDate, usrInfo, out CompiledDataToBeWritten, Banks, conn);
                        //        if (Status == true)
                        //        {
                        //            Status = GenerateEJsUG("BCV", dstrxClearing, dsClearingFileFormat, ToBank, "USD", dsWithImages, WorkingDate, usrInfo, out SemiCompiledDataToBeWritten, Banks, conn);
                        //            FullyCompiledDataToBeWritten.Merge(SemiCompiledDataToBeWritten);
                        //            Data = TrailerControlVoucherUG(dstrxClearing, dsClearingFileFormat, "BCV", ToBank, "EJ", WorkingDate, usrInfo, "USD", conn);
                        //            FullyCompiledDataToBeWritten.Rows.Add("0", Data, "", "0", IsFcy);
                        //            SemiCompiledDataToBeWritten = new DataTable();
                        //        }

                        //        //MDV
                        //        Status = GenerateEJsUG("MDV", dstrxClearing, dsClearingFileFormat, ToBank, "USD", dsWithImages, WorkingDate, usrInfo, out CompiledDataToBeWritten, Banks, conn);
                        //        if (Status == true)
                        //        {
                        //            Status = GenerateEJsUG("MDV", dstrxClearing, dsClearingFileFormat, ToBank, "USD", dsWithImages, WorkingDate, usrInfo, out SemiCompiledDataToBeWritten, Banks, conn);
                        //            FullyCompiledDataToBeWritten.Merge(SemiCompiledDataToBeWritten);
                        //            Data = TrailerControlVoucherUG(dstrxClearing, dsClearingFileFormat, "MDV", ToBank, "EJ", WorkingDate, usrInfo, "USD", conn);
                        //            FullyCompiledDataToBeWritten.Rows.Add("0", Data, "", "0", IsFcy);
                        //            SemiCompiledDataToBeWritten = new DataTable();
                        //        }

                        //        //Unpaids
                        //        Status = GenerateEJsUG("UCV", dstrxClearing, dsClearingFileFormat, ToBank, "USD", dsWithImages, WorkingDate, usrInfo, out CompiledDataToBeWritten, Banks, conn);
                        //        if (Status == true)
                        //        {
                        //            Status = GenerateEJsUG("UCV", dstrxClearing, dsClearingFileFormat, ToBank, "USD", dsWithImages, WorkingDate, usrInfo, out SemiCompiledDataToBeWritten, Banks, conn);
                        //            FullyCompiledDataToBeWritten.Merge(SemiCompiledDataToBeWritten);
                        //            Data = TrailerControlVoucherUG(dstrxClearing, dsClearingFileFormat, "UCV", ToBank, "EJ", WorkingDate, usrInfo, "USD", conn);
                        //            FullyCompiledDataToBeWritten.Rows.Add("0", Data, "", "0", IsFcy);
                        //            SemiCompiledDataToBeWritten = new DataTable();
                        //        }
                        //    }
                        //    //61
                        //    if ((GenerateEJsUG("BCV", dstrxClearing, dsClearingFileFormat, ToBank, "GBP", dsWithImages, WorkingDate, usrInfo, out CompiledDataToBeWritten, Banks, conn) == true)
                        //    ||
                        //    (GenerateEJsUG("MDV", dstrxClearing, dsClearingFileFormat, ToBank, "GBP", dsWithImages, WorkingDate, usrInfo, out CompiledDataToBeWritten, Banks, conn) == true)
                        //    ||
                        //    (GenerateEJsUG("UCV", dstrxClearing, dsClearingFileFormat, ToBank, "GBP", dsWithImages, WorkingDate, usrInfo, out CompiledDataToBeWritten, Banks, conn) == true))
                        //    {
                        //        Data = LeadingControlVoucherUG(dstrxClearing, dsClearingFileFormat, ToBank, "EJ", WorkingDate, usrInfo, "BCV", "GBP", conn);
                        //        FullyCompiledDataToBeWritten.Rows.Add("0", Data, "", "0", IsFcy);
                        //        //Presentments
                        //        Status = GenerateEJsUG("BCV", dstrxClearing, dsClearingFileFormat, ToBank, "GBP", dsWithImages, WorkingDate, usrInfo, out CompiledDataToBeWritten, Banks, conn);
                        //        if (Status == true)
                        //        {
                        //            Status = GenerateEJsUG("BCV", dstrxClearing, dsClearingFileFormat, ToBank, "GBP", dsWithImages, WorkingDate, usrInfo, out SemiCompiledDataToBeWritten, Banks, conn);
                        //            FullyCompiledDataToBeWritten.Merge(SemiCompiledDataToBeWritten);
                        //            Data = TrailerControlVoucherUG(dstrxClearing, dsClearingFileFormat, "BCV", ToBank, "EJ", WorkingDate, usrInfo, "GBP", conn);
                        //            FullyCompiledDataToBeWritten.Rows.Add("0", Data, "", "0", IsFcy);
                        //            SemiCompiledDataToBeWritten = new DataTable();
                        //        }

                        //        //MDV
                        //        Status = GenerateEJsUG("MDV", dstrxClearing, dsClearingFileFormat, ToBank, "GBP", dsWithImages, WorkingDate, usrInfo, out CompiledDataToBeWritten, Banks, conn);
                        //        if (Status == true)
                        //        {
                        //            Status = GenerateEJsUG("MDV", dstrxClearing, dsClearingFileFormat, ToBank, "GBP", dsWithImages, WorkingDate, usrInfo, out SemiCompiledDataToBeWritten, Banks, conn);
                        //            FullyCompiledDataToBeWritten.Merge(SemiCompiledDataToBeWritten);
                        //            Data = TrailerControlVoucherUG(dstrxClearing, dsClearingFileFormat, "MDV", ToBank, "EJ", WorkingDate, usrInfo, "GBP", conn);
                        //            FullyCompiledDataToBeWritten.Rows.Add("0", Data, "", "0", IsFcy);
                        //            SemiCompiledDataToBeWritten = new DataTable();
                        //        }

                        //        //Unpaids
                        //        Status = GenerateEJsUG("UCV", dstrxClearing, dsClearingFileFormat, ToBank, "GBP", dsWithImages, WorkingDate, usrInfo, out CompiledDataToBeWritten, Banks, conn);
                        //        if (Status == true)
                        //        {
                        //            Status = GenerateEJsUG("UCV", dstrxClearing, dsClearingFileFormat, ToBank, "GBP", dsWithImages, WorkingDate, usrInfo, out SemiCompiledDataToBeWritten, Banks, conn);
                        //            FullyCompiledDataToBeWritten.Merge(SemiCompiledDataToBeWritten);
                        //            Data = TrailerControlVoucherUG(dstrxClearing, dsClearingFileFormat, "UCV", ToBank, "EJ", WorkingDate, usrInfo, "GBP", conn);
                        //            FullyCompiledDataToBeWritten.Rows.Add("0", Data, "", "0", IsFcy);
                        //            SemiCompiledDataToBeWritten = new DataTable();
                        //        }
                        //    }
                        //    //62
                        //    if ((GenerateEJsUG("BCV", dstrxClearing, dsClearingFileFormat, ToBank, "EUR", dsWithImages, WorkingDate, usrInfo, out CompiledDataToBeWritten, Banks, conn) == true)
                        //    ||
                        //    (GenerateEJsUG("MDV", dstrxClearing, dsClearingFileFormat, ToBank, "EUR", dsWithImages, WorkingDate, usrInfo, out CompiledDataToBeWritten, Banks, conn) == true)
                        //    ||
                        //    (GenerateEJsUG("UCV", dstrxClearing, dsClearingFileFormat, ToBank, "EUR", dsWithImages, WorkingDate, usrInfo, out CompiledDataToBeWritten, Banks, conn) == true))
                        //    {
                        //        Data = LeadingControlVoucherUG(dstrxClearing, dsClearingFileFormat, ToBank, "EJ", WorkingDate, usrInfo, "BCV", "EUR", conn);
                        //        FullyCompiledDataToBeWritten.Rows.Add("0", Data, "", "0", IsFcy);
                        //        //Presentments
                        //        Status = GenerateEJsUG("BCV", dstrxClearing, dsClearingFileFormat, ToBank, "EUR", dsWithImages, WorkingDate, usrInfo, out CompiledDataToBeWritten, Banks, conn);
                        //        if (Status == true)
                        //        {
                        //            Status = GenerateEJsUG("BCV", dstrxClearing, dsClearingFileFormat, ToBank, "EUR", dsWithImages, WorkingDate, usrInfo, out SemiCompiledDataToBeWritten, Banks, conn);
                        //            FullyCompiledDataToBeWritten.Merge(SemiCompiledDataToBeWritten);
                        //            Data = TrailerControlVoucherUG(dstrxClearing, dsClearingFileFormat, "BCV", ToBank, "EJ", WorkingDate, usrInfo, "EUR", conn);
                        //            FullyCompiledDataToBeWritten.Rows.Add("0", Data, "", "0", IsFcy);
                        //            SemiCompiledDataToBeWritten = new DataTable();
                        //        }

                        //        //MDV
                        //        Status = GenerateEJsUG("MDV", dstrxClearing, dsClearingFileFormat, ToBank, "EUR", dsWithImages, WorkingDate, usrInfo, out CompiledDataToBeWritten, Banks, conn);
                        //        if (Status == true)
                        //        {
                        //            Status = GenerateEJsUG("MDV", dstrxClearing, dsClearingFileFormat, ToBank, "EUR", dsWithImages, WorkingDate, usrInfo, out SemiCompiledDataToBeWritten, Banks, conn);
                        //            FullyCompiledDataToBeWritten.Merge(SemiCompiledDataToBeWritten);
                        //            Data = TrailerControlVoucherUG(dstrxClearing, dsClearingFileFormat, "MDV", ToBank, "EJ", WorkingDate, usrInfo, "EUR", conn);
                        //            FullyCompiledDataToBeWritten.Rows.Add("0", Data, "", "0", IsFcy);
                        //            SemiCompiledDataToBeWritten = new DataTable();
                        //        }

                        //        //Unpaids
                        //        Status = GenerateEJsUG("UCV", dstrxClearing, dsClearingFileFormat, ToBank, "EUR", dsWithImages, WorkingDate, usrInfo, out CompiledDataToBeWritten, Banks, conn);
                        //        if (Status == true)
                        //        {
                        //            Status = GenerateEJsUG("UCV", dstrxClearing, dsClearingFileFormat, ToBank, "EUR", dsWithImages, WorkingDate, usrInfo, out SemiCompiledDataToBeWritten, Banks, conn);
                        //            FullyCompiledDataToBeWritten.Merge(SemiCompiledDataToBeWritten);
                        //            Data = TrailerControlVoucherUG(dstrxClearing, dsClearingFileFormat, "UCV", ToBank, "EJ", WorkingDate, usrInfo, "EUR", conn);
                        //            FullyCompiledDataToBeWritten.Rows.Add("0", Data, "", "0", IsFcy);
                        //            SemiCompiledDataToBeWritten = new DataTable();
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //MessageBox.Show("LeadingControlVoucherUG");
                            Data = LeadingControlVoucherUG(dstrxClearing, dsClearingFileFormat, ToBank, "EJ", WorkingDate, usrInfo, "BCV", StrCurrencyID, conn);
                            //MessageBox.Show("FullyCompiledDataToBeWritten");
                            FullyCompiledDataToBeWritten.Rows.Add("0", Data, "", "0", IsFcy);
                            //Presentments
                            //MessageBox.Show("GenerateEJsUG");
                            Status = GenerateEJsUG("BCV", dstrxClearing, dsClearingFileFormat, ToBank, StrCurrencyID, dsWithImages, WorkingDate, usrInfo, out CompiledDataToBeWritten, Banks, conn);
                            if (Status == true)
                            {
                                //MessageBox.Show("Status");
                                Status = GenerateEJsUG("BCV", dstrxClearing, dsClearingFileFormat, ToBank, StrCurrencyID, dsWithImages, WorkingDate, usrInfo, out SemiCompiledDataToBeWritten, Banks, conn);
                                FullyCompiledDataToBeWritten.Merge(SemiCompiledDataToBeWritten);
                                //MessageBox.Show("TrailerControlVoucherUG");
                                Data = TrailerControlVoucherUG(dstrxClearing, dsClearingFileFormat, "BCV", ToBank, "EJ", WorkingDate, usrInfo, StrCurrencyID, conn);
                                FullyCompiledDataToBeWritten.Rows.Add("0", Data, "", "0", IsFcy);
                                SemiCompiledDataToBeWritten = new DataTable();
                                //MessageBox.Show("Done");
                            }

                            //MDV
                            Status = GenerateEJsUG("MDV", dstrxClearing, dsClearingFileFormat, ToBank, StrCurrencyID, dsWithImages, WorkingDate, usrInfo, out CompiledDataToBeWritten, Banks, conn);
                            if (Status == true)
                            {
                                Status = GenerateEJsUG("MDV", dstrxClearing, dsClearingFileFormat, ToBank, StrCurrencyID, dsWithImages, WorkingDate, usrInfo, out SemiCompiledDataToBeWritten, Banks, conn);
                                FullyCompiledDataToBeWritten.Merge(SemiCompiledDataToBeWritten);
                                Data = TrailerControlVoucherUG(dstrxClearing, dsClearingFileFormat, "MDV", ToBank, "EJ", WorkingDate, usrInfo, StrCurrencyID, conn);
                                FullyCompiledDataToBeWritten.Rows.Add("0", Data, "", "0", IsFcy);
                                SemiCompiledDataToBeWritten = new DataTable();
                            }

                            //Unpaids
                            Status = GenerateEJsUG("UCV", dstrxClearing, dsClearingFileFormat, ToBank, StrCurrencyID, dsWithImages, WorkingDate, usrInfo, out CompiledDataToBeWritten, Banks, conn);
                            if (Status == true)
                            {
                                Status = GenerateEJsUG("UCV", dstrxClearing, dsClearingFileFormat, ToBank, StrCurrencyID, dsWithImages, WorkingDate, usrInfo, out SemiCompiledDataToBeWritten, Banks, conn);
                                FullyCompiledDataToBeWritten.Merge(SemiCompiledDataToBeWritten);
                                Data = TrailerControlVoucherUG(dstrxClearing, dsClearingFileFormat, "UCV", ToBank, "EJ", WorkingDate, usrInfo, StrCurrencyID, conn);
                                FullyCompiledDataToBeWritten.Rows.Add("0", Data, "", "0", IsFcy);
                                SemiCompiledDataToBeWritten = new DataTable();
                            }
                        //}
                        break;
                }
                WorkingDataTable.Tables[0].Rows.Clear();
            }
            return FullyCompiledDataToBeWritten;
        }
        private static IDbConnection ReGetConnection(string[] strConn)
        {
            string SysType = ConfigurationManager.AppSettings["sysType"];
            IDbConnection connection = null;
            if (SysType == "BR" || SysType == "BRMFO")
            {
                string IP = strConn[2];
                string DBName = strConn[0];
                string DBpass = strConn[1];
                string strSystem = ConfigurationManager.AppSettings["IPAddress"];
                string strConnectString = "";
                strConnectString = "Data Source=" + IP + ";Initial Catalog=" + DBName + ";User ID=Realm;Password=" + DBpass + ";";
                connection = new System.Data.SqlClient.SqlConnection(strConnectString);
            }
            else
            {
                string LiveEnv = ConfigurationManager.AppSettings["IsLiveEnv"];
                if (LiveEnv == "1")
                {
                    strDBServerName = strCon[0];
                    strDatabaseName = strCon[1];
                    strBRUserName = strCon[2];
                    strBRUserPassword = strCon[3];
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
            }
            

            if (connection.State != ConnectionState.Open)
                connection.Open();
          
            return connection;
        }
        private static IDbConnection GetConnection()
        {
          
            IDbConnection connection = null;
            string LiveEnv = ConfigurationManager.AppSettings["IsLiveEnv"];
            if (LiveEnv == "1")
            {
                string strDBServerName = ConfigurationManager.AppSettings["strDBServerName"];
                string strDatabaseName = ConfigurationManager.AppSettings["strDatabaseName"];
                string strBRUserName = ConfigurationManager.AppSettings["strBRUserName"];
                string strBRUserPassword = ConfigurationManager.AppSettings["strBRUserPassword"]; ;
                string strSYSADMIN1UserName = ConfigurationManager.AppSettings["strSYSADMIN1UserName"];
                string strSYSADMIN1Password = ConfigurationManager.AppSettings["strSYSADMIN1Password"]; ;
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
            return connection;
        }
        public static string TrailerControlVoucher(DS_trxClearing dstrxClearing, DS_ClearingFileFormat dsClearingFileFormat, string VoucherType, string RBank, string FileType, DateTime WorkingDate, BRBase.UserInfo usrInfo, string Currency, IDbConnection conn, string Banks)
        {
            Boolean isMFI = false;

            if (RBank == "MFI")
            {
                isMFI = true;
                RBank = usrInfo.strBank;
            }
            Int16 j, m;
            Int32 t;
            IDbConnection storedconn =conn;
            string sortOrder = "Start ASC";
            string ControlVoucherType = string.Empty;
            string NewValue = string.Empty;
            string Value = string.Empty;
            string Data = string.Empty;
            string CurrType = string.Empty;
            string Curr = string.Empty;
            DS_trxClearing EJdataTable = new DS_trxClearing();
            double SumCheques = 0;
            Int32 ChequesCount = 0;
            DS_ClearingFileFormat WorkingDataTable = new DS_ClearingFileFormat();
            BRBase.BRDataSet dsUniqueClearingID = new BRBase.BRDataSet();
            string UniqueID = string.Empty;
            string dd = new string('0', 2 - BRBase.BRBaseConvert.ConvertToString(WorkingDate.Day).ToString().Length) + BRBase.BRBaseConvert.ConvertToString(WorkingDate.Day);
            string mm = new string('0', 2 - BRBase.BRBaseConvert.ConvertToString(WorkingDate.Month).Length) + BRBase.BRBaseConvert.ConvertToString(WorkingDate.Month);
            string yyyy = BRBase.BRBaseConvert.ConvertToString(WorkingDate.Year);
            string ddmmmyyyy = dd + mm + yyyy;
            t = 0;
            DataRow[] drHeaderFileFormatResult = dsClearingFileFormat.Tables[0].Select("RecordType = 'CONTROLVOUCHER'", sortOrder);
            foreach (DataRow dvr in drHeaderFileFormatResult)
            {
                WorkingDataTable.Tables[0].ImportRow(dvr);
            }
            WorkingDataTable.AcceptChanges();
            Object[] arryRow = new Object[WorkingDataTable.Tables[0].Rows.Count];
            if (FileType.ToUpper() == "EJ")
            {
                if (Currency == "KES")
                {
                    Curr = "00";
                    CurrType = " AND currencyID ='KES' AND TrxType ='OC'"; //" AND currencyID ='" + Currency + "'";
                }
                else
                {
                    switch (Currency.ToString().ToUpper())
                    {
                        case "USD":
                            Curr = "60";
                            break;
                        case "GBP":
                            Curr = "61";
                            break;
                        case "EUR":
                            Curr = "62";
                            break;
                        case "UGX":
                            Curr = "00";
                            break;
                    }
                    CurrType = " AND currencyID = '" + Currency + "' AND TrxType ='OC'";//CurrType = " AND currencyID <> '" + Currency + "'";
                }
                switch (VoucherType.ToString().ToUpper())
                {
                    case "BCV":  //Presentments 
                        SumCheques = 0;
                        ChequesCount = 0;
                        if (isMFI == true)
                        {
                            drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("ReturnCode IN ('00','17') AND VOUCHERCODE <>'03' AND ToBank IN " + "('" + Banks + "')" + " " + CurrType + "");//

                        }
                        else
                        {
                            drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("ReturnCode IN ('00','17') AND VOUCHERCODE <>'03' AND ToBank " + Banks + " " + CurrType + "");//

                        }
                        foreach (DataRow dvr in drHeaderFileFormatResult)
                        {
                            EJdataTable.Tables[0].ImportRow(dvr);
                            SumCheques = SumCheques + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                            ChequesCount = ChequesCount + 1;
                        }
                        EJdataTable.AcceptChanges();
                        break;
                    case "UCV":  //Unpaids 
                        SumCheques = 0;
                        ChequesCount = 0;
                        drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("ReturnCode NOT IN ('00','17') AND ToBank " + Banks + " " + CurrType + "");
                        foreach (DataRow dvr in drHeaderFileFormatResult)
                        {
                            EJdataTable.Tables[0].ImportRow(dvr);
                            SumCheques = SumCheques + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                            ChequesCount = ChequesCount + 1;
                        }
                        EJdataTable.AcceptChanges();
                        break;
                    case "MDV":  //Manual Debit Voucher
                        SumCheques = 0;
                        ChequesCount = 0;
                        drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("ReturnCode IN ('00','17') AND VOUCHERCODE = '03' AND ToBank " + Banks + " " + CurrType + "");
                        foreach (DataRow dvr in drHeaderFileFormatResult)
                        {
                            EJdataTable.Tables[0].ImportRow(dvr);
                            SumCheques = SumCheques + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                            ChequesCount = ChequesCount + 1;
                        }
                        EJdataTable.AcceptChanges();
                        break;
                }
            }
            foreach (DataColumn Col in WorkingDataTable.Tables[0].Columns)
            {
                switch (Col.ColumnName.ToUpper())
                {
                    case "FIELDNAME":
                        switch (FileType.ToUpper())
                        {
                            case "EJ":
                                for (j = 0; j < WorkingDataTable.Tables[0].Rows.Count; j++)
                                {
                                    switch (VoucherType.ToString().ToUpper())
                                    {
                                        case "BCV":
                                            if (conn.State != ConnectionState.Open)
                                            {
                                                conn = GetConnection();
                                            }
                                            string FieldNm = string.Empty;
                                            FieldNm = WorkingDataTable.Tables[0].Rows[j][Col.ColumnName].ToString();
                                            Int32 FileFormatValue = BRBase.BRBaseConvert.ConvertToInt32(WorkingDataTable.Tables[0].Rows[j]["Length"].ToString());
                                            bool FileFormatValueMandatoryLength = WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString() == "" ? false : BRBase.BRBaseConvert.ConvertToBoolean(WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString());
                                            string Filler = WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString() == "" ? "0" : WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString();
                                            Value = BRBase.BRBaseConvert.ConvertToString(WorkingDataTable.Tables[0].Rows[j]["DEFAULTVALUES"].ToString().Trim());
                                            switch (FieldNm.ToString().ToUpper().Trim())
                                            {
                                                case "RECORDTYPE":
                                                    arryRow[t] = Value;
                                                    t = t + 1;
                                                    break;
                                                case "CURRENCYCODE":
                                                    arryRow[t] = Curr;
                                                    t = t + 1;
                                                    break;
                                                case "POSITIONOFAMOUNT":
                                                    arryRow[t] = 0;
                                                    t = t + 1;
                                                    break;
                                                case "PSERIALNUMBER":
                                                    dsUniqueClearingID = new BRBase.BRDataSet();
                                                    UniqueID = string.Empty;
                                                    ClearingUniversalMethod(usrInfo, "p_GetUniqueClearingID", out dsUniqueClearingID, BRBase.BRModule.InwardFileImportation, conn, new object[] { "BrDataSet" }, new object[] { FileFormatValue });
                                                    if (dsUniqueClearingID.Tables.Count > 0)
                                                    {
                                                        if (dsUniqueClearingID.Tables[0].Rows.Count > 0)
                                                        {
                                                            UniqueID = dsUniqueClearingID.Tables[0].Rows[0]["UniqueID"].ToString();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        UniqueID = RBank + dd + mm;
                                                    }
                                                    arryRow[t] = UniqueID;
                                                    t = t + 1;
                                                    break;
                                                case "FILLER":
                                                    Filler = new string(BRBase.BRBaseConvert.ConvertToChar(Filler), FileFormatValue);
                                                    arryRow[t] = Filler;
                                                    t = t + 1;
                                                    break;
                                                case "VALUE":
                                                    // Pramod
                                                    //NewValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(SumCheques)));
                                                    if (Curr == "00")
                                                    {
                                                        NewValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(SumCheques)));
                                                    }
                                                    else
                                                    {
                                                        NewValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(SumCheques),"60"));
                                                    }
                                                    NewValue = new string('0', FileFormatValue - NewValue.Length) + NewValue;
                                                    arryRow[t] = NewValue;
                                                    t = t + 1;
                                                    break;
                                                case "PCLEARINGCENTRECODE":
                                                    arryRow[t] = RBank;
                                                    t = t + 1;
                                                    break;
                                                case "RCLEARINGCENTRECODE":
                                                    arryRow[t] = usrInfo.strBank;
                                                    t = t + 1;
                                                    break;
                                                case "DRN":
                                                    dsUniqueClearingID = new BRBase.BRDataSet();
                                                    UniqueID = string.Empty;
                                                    ClearingUniversalMethod(usrInfo, "p_GetUniqueClearingID", out dsUniqueClearingID, BRBase.BRModule.InwardFileImportation, conn, new object[] { "BrDataSet" }, new object[] { FileFormatValue });
                                                    if (dsUniqueClearingID.Tables.Count > 0)
                                                    {
                                                        if (dsUniqueClearingID.Tables[0].Rows.Count > 0)
                                                        {
                                                            UniqueID = dsUniqueClearingID.Tables[0].Rows[0]["UniqueID"].ToString();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        UniqueID = RBank + dd + mm;
                                                    }
                                                    UniqueID = usrInfo.strBank + UniqueID;
                                                    NewValue = new string(BRBase.BRBaseConvert.ConvertToChar(Filler), (BRBase.BRBaseConvert.ConvertToInt32(FileFormatValue) - BRBase.BRBaseConvert.ConvertToInt32(UniqueID.Length))) + UniqueID;
                                                    arryRow[t] = NewValue;
                                                    t = t + 1;
                                                    break;
                                                case "VOUCHERCODE":
                                                    arryRow[t] = "71";
                                                    t = t + 1;
                                                    break;
                                            }
                                            break;
                                        case "UCV":
                                            if (conn.State != ConnectionState.Open)
                                            {
                                                conn = GetConnection();
                                            }
                                            FieldNm = string.Empty;
                                            FieldNm = WorkingDataTable.Tables[0].Rows[j][Col.ColumnName].ToString();
                                            FileFormatValue = BRBase.BRBaseConvert.ConvertToInt32(WorkingDataTable.Tables[0].Rows[j]["Length"].ToString());
                                            FileFormatValueMandatoryLength = WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString() == "" ? false : BRBase.BRBaseConvert.ConvertToBoolean(WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString());
                                            Filler = WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString() == "" ? "0" : WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString();
                                            Value = BRBase.BRBaseConvert.ConvertToString(WorkingDataTable.Tables[0].Rows[j]["DEFAULTVALUES"].ToString().Trim());
                                            switch (FieldNm.ToString().ToUpper().Trim())
                                            {
                                                case "RECORDTYPE":
                                                    arryRow[t] = Value;
                                                    t = t + 1;
                                                    break;
                                                case "CURRENCYCODE":
                                                    arryRow[t] = Curr;
                                                    t = t + 1;
                                                    break;
                                                case "POSITIONOFAMOUNT":
                                                    arryRow[t] = 0;
                                                    t = t + 1;
                                                    break;
                                                case "PSERIALNUMBER":
                                                    dsUniqueClearingID = new BRBase.BRDataSet();
                                                    UniqueID = string.Empty;
                                                    ClearingUniversalMethod(usrInfo, "p_GetUniqueClearingID", out dsUniqueClearingID, BRBase.BRModule.InwardFileImportation, conn, new object[] { "BrDataSet" }, new object[] { FileFormatValue });
                                                    if (dsUniqueClearingID.Tables.Count > 0)
                                                    {
                                                        if (dsUniqueClearingID.Tables[0].Rows.Count > 0)
                                                        {
                                                            UniqueID = dsUniqueClearingID.Tables[0].Rows[0]["UniqueID"].ToString();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        UniqueID = RBank + dd + mm;
                                                    }
                                                    arryRow[t] = UniqueID;
                                                    t = t + 1;
                                                    break;
                                                case "FILLER":
                                                    Filler = new string(BRBase.BRBaseConvert.ConvertToChar(Filler), FileFormatValue);
                                                    arryRow[t] = Filler;
                                                    t = t + 1;
                                                    break;
                                                case "VALUE":
                                                     // Pramod
                                                    //NewValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(SumCheques)));
                                                    if (Curr == "00")
                                                    {
                                                        NewValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(SumCheques)));
                                                    }
                                                    else
                                                    {
                                                        NewValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(SumCheques),"60"));
                                                    }
                                                    NewValue = new string('0', 13 - NewValue.Length) + NewValue;
                                                    arryRow[t] = NewValue;
                                                    t = t + 1;
                                                    break;
                                                case "PCLEARINGCENTRECODE":
                                                    arryRow[t] = RBank;
                                                    t = t + 1;
                                                    break;
                                                case "RCLEARINGCENTRECODE":
                                                    arryRow[t] = usrInfo.strBank;
                                                    t = t + 1;
                                                    break;
                                                case "DRN":
                                                    dsUniqueClearingID = new BRBase.BRDataSet();
                                                    UniqueID = string.Empty;
                                                    NewValue = string.Empty;
                                                    ClearingUniversalMethod(usrInfo, "p_GetUniqueClearingID", out dsUniqueClearingID, BRBase.BRModule.InwardFileImportation, conn, new object[] { "BrDataSet" }, new object[] { FileFormatValue });
                                                    if (dsUniqueClearingID.Tables.Count > 0)
                                                    {
                                                        if (dsUniqueClearingID.Tables[0].Rows.Count > 0)
                                                        {
                                                            UniqueID = dsUniqueClearingID.Tables[0].Rows[0]["UniqueID"].ToString();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        UniqueID = RBank + dd + mm;
                                                    }
                                                    UniqueID = usrInfo.strBank + UniqueID;
                                                    NewValue = new string(BRBase.BRBaseConvert.ConvertToChar(Filler), (BRBase.BRBaseConvert.ConvertToInt32(FileFormatValue) - BRBase.BRBaseConvert.ConvertToInt32(UniqueID.Length))) + UniqueID;
                                                    arryRow[t] = NewValue;
                                                    t = t + 1;
                                                    break;
                                                case "VOUCHERCODE":
                                                    arryRow[t] = "72";
                                                    t = t + 1;
                                                    break;
                                            }
                                            break;
                                        case "MDV":
                                            if (conn.State != ConnectionState.Open)
                                            {
                                                conn = GetConnection();
                                            }
                                            FieldNm = string.Empty;
                                            FieldNm = WorkingDataTable.Tables[0].Rows[j][Col.ColumnName].ToString();
                                            FileFormatValue = BRBase.BRBaseConvert.ConvertToInt32(WorkingDataTable.Tables[0].Rows[j]["Length"].ToString());
                                            FileFormatValueMandatoryLength = WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString() == "" ? false : BRBase.BRBaseConvert.ConvertToBoolean(WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString());
                                            Filler = WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString() == "" ? "0" : WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString();
                                            Value = BRBase.BRBaseConvert.ConvertToString(WorkingDataTable.Tables[0].Rows[j]["DEFAULTVALUES"].ToString().Trim());
                                            switch (FieldNm.ToString().ToUpper().Trim())
                                            {
                                                case "RECORDTYPE":
                                                    arryRow[t] = Value;
                                                    t = t + 1;
                                                    break;
                                                case "CURRENCYCODE":
                                                    arryRow[t] = Curr;
                                                    t = t + 1;
                                                    break;
                                                case "POSITIONOFAMOUNT":
                                                    arryRow[t] = 0;
                                                    t = t + 1;
                                                    break;
                                                case "PSERIALNUMBER":
                                                    dsUniqueClearingID = new BRBase.BRDataSet();
                                                    UniqueID = string.Empty;
                                                    ClearingUniversalMethod(usrInfo, "p_GetUniqueClearingID", out dsUniqueClearingID, BRBase.BRModule.InwardFileImportation, conn, new object[] { "BrDataSet" }, new object[] { FileFormatValue });
                                                    if (dsUniqueClearingID.Tables.Count > 0)
                                                    {
                                                        if (dsUniqueClearingID.Tables[0].Rows.Count > 0)
                                                        {
                                                            UniqueID = dsUniqueClearingID.Tables[0].Rows[0]["UniqueID"].ToString();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        UniqueID = RBank + dd + mm;
                                                    }
                                                    arryRow[t] = UniqueID;
                                                    t = t + 1;
                                                    break;
                                                case "FILLER":
                                                    Filler = new string(BRBase.BRBaseConvert.ConvertToChar(Filler), FileFormatValue);
                                                    arryRow[t] = Filler;
                                                    t = t + 1;
                                                    break;
                                                case "VALUE":
                                                    // Pramod
                                                    //NewValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(SumCheques)));
                                                    if (Curr == "00")
                                                    {
                                                        NewValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(SumCheques)));
                                                    }
                                                    else
                                                    {
                                                        NewValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(SumCheques),"60"));
                                                    }
                                                    
                                                    NewValue = new string('0', 13 - NewValue.Length) + NewValue;
                                                    arryRow[t] = NewValue;
                                                    t = t + 1;
                                                    break;
                                                case "PCLEARINGCENTRECODE":
                                                    arryRow[t] = RBank;
                                                    t = t + 1;
                                                    break;
                                                case "RCLEARINGCENTRECODE":
                                                    arryRow[t] = usrInfo.strBank;
                                                    t = t + 1;
                                                    break;
                                                case "DRN":
                                                    dsUniqueClearingID = new BRBase.BRDataSet();
                                                    UniqueID = string.Empty;
                                                    NewValue = string.Empty;
                                                    ClearingUniversalMethod(usrInfo, "p_GetUniqueClearingID", out dsUniqueClearingID, BRBase.BRModule.InwardFileImportation, conn, new object[] { "BrDataSet" }, new object[] { FileFormatValue });
                                                    if (dsUniqueClearingID.Tables.Count > 0)
                                                    {
                                                        if (dsUniqueClearingID.Tables[0].Rows.Count > 0)
                                                        {
                                                            UniqueID = dsUniqueClearingID.Tables[0].Rows[0]["UniqueID"].ToString();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        UniqueID = RBank + dd + mm;
                                                    }
                                                    UniqueID = usrInfo.strBank + UniqueID;
                                                    NewValue = new string(BRBase.BRBaseConvert.ConvertToChar(Filler), (BRBase.BRBaseConvert.ConvertToInt32(FileFormatValue) - BRBase.BRBaseConvert.ConvertToInt32(UniqueID.Length))) + UniqueID;
                                                    arryRow[t] = NewValue;
                                                    t = t + 1;
                                                    break;
                                                case "VOUCHERCODE":
                                                    arryRow[t] = "73";
                                                    t = t + 1;
                                                    break;
                                            }
                                            break;
                                    }
                                }
                                break;
                            case "EFT":
                                for (j = 0; j < WorkingDataTable.Tables[0].Rows.Count; j++)
                                {
                                    if (conn.State != ConnectionState.Open)
                                    {
                                        conn = GetConnection();
                                    }
                                    string FieldNm = string.Empty;
                                    FieldNm = WorkingDataTable.Tables[0].Rows[j][Col.ColumnName].ToString();
                                    Int32 FileFormatValue = BRBase.BRBaseConvert.ConvertToInt32(WorkingDataTable.Tables[0].Rows[j]["Length"].ToString());
                                    bool FileFormatValueMandatoryLength = WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString() == "" ? false : BRBase.BRBaseConvert.ConvertToBoolean(WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString());
                                    string Filler = WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString() == "" ? "0" : WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString();
                                    Value = BRBase.BRBaseConvert.ConvertToString(WorkingDataTable.Tables[0].Rows[j]["DEFAULTVALUES"].ToString().Trim());
                                    switch (FieldNm.ToString().ToUpper().Trim())
                                    {
                                        case "RECORDTYPE":
                                        case "PORGANIZATION":
                                            arryRow[t] = Value;
                                            t = t + 1;
                                            break;
                                        case "PBANK":
                                            arryRow[t] = usrInfo.strBank;
                                            t = t + 1;
                                            break;
                                        case "FILLER":
                                        case "FILEINDICATOR":
                                            Filler = new string(BRBase.BRBaseConvert.ConvertToChar(Filler), FileFormatValue);
                                            arryRow[t] = Filler;
                                            t = t + 1;
                                            break;
                                        case "PSERIALNUMBER":
                                            dsUniqueClearingID = new BRBase.BRDataSet();
                                            UniqueID = string.Empty;
                                            ClearingUniversalMethod(usrInfo, "p_GetUniqueClearingID", out dsUniqueClearingID, BRBase.BRModule.InwardFileImportation, conn, new object[] { "BrDataSet" }, new object[] { FileFormatValue });
                                            if (dsUniqueClearingID.Tables.Count > 0)
                                            {
                                                if (dsUniqueClearingID.Tables[0].Rows.Count > 0)
                                                {
                                                    UniqueID = dsUniqueClearingID.Tables[0].Rows[0]["UniqueID"].ToString();
                                                }
                                            }
                                            else
                                            {
                                                UniqueID = RBank + dd + mm;
                                            }
                                            arryRow[t] = UniqueID;
                                            t = t + 1;
                                            break;
                                    }
                                }
                                break;
                            case "DISC":
                                for (j = 0; j < WorkingDataTable.Tables[0].Rows.Count; j++)
                                {
                                    if (conn.State != ConnectionState.Open)
                                    {
                                        conn = GetConnection();
                                    }
                                    string FieldNm = string.Empty;
                                    FieldNm = WorkingDataTable.Tables[0].Rows[j][Col.ColumnName].ToString();
                                    Int32 FileFormatValue = BRBase.BRBaseConvert.ConvertToInt32(WorkingDataTable.Tables[0].Rows[j]["Length"].ToString());
                                    bool FileFormatValueMandatoryLength = WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString() == "" ? false : BRBase.BRBaseConvert.ConvertToBoolean(WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString());
                                    string Filler = WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString() == "" ? "0" : WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString();
                                    Value = BRBase.BRBaseConvert.ConvertToString(WorkingDataTable.Tables[0].Rows[j]["DEFAULTVALUES"].ToString().Trim());
                                    switch (FieldNm.ToString().ToUpper().Trim())
                                    {
                                        case "RECORDTYPE":
                                        case "PBANK":
                                        case "PORGANIZATION":
                                            arryRow[t] = Value;
                                            t = t + 1;
                                            break;
                                        case "FILLER":
                                        case "PTRXCOUNT":
                                        case "PVALUECREDIT":
                                        case "PVALUEDEBIT":
                                            Filler = new string(BRBase.BRBaseConvert.ConvertToChar(Filler), FileFormatValue);
                                            arryRow[t] = Filler;
                                            t = t + 1;
                                            break;
                                        case "PCLEARINGCENTRE":
                                            arryRow[t] = usrInfo.strBank;
                                            t = t + 1;
                                            break;
                                    }
                                }
                                break;
                            case "SELT":
                                for (j = 0; j < WorkingDataTable.Tables[0].Rows.Count; j++)
                                {
                                    if (conn.State != ConnectionState.Open)
                                    {
                                        conn = GetConnection();
                                    }
                                    string FieldNm = string.Empty;
                                    FieldNm = WorkingDataTable.Tables[0].Rows[j][Col.ColumnName].ToString();
                                    Int32 FileFormatValue = BRBase.BRBaseConvert.ConvertToInt32(WorkingDataTable.Tables[0].Rows[j]["Length"].ToString());
                                    bool FileFormatValueMandatoryLength = WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString() == "" ? false : BRBase.BRBaseConvert.ConvertToBoolean(WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString());
                                    string Filler = WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString() == "" ? "0" : WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString();
                                    Value = BRBase.BRBaseConvert.ConvertToString(WorkingDataTable.Tables[0].Rows[j]["DEFAULTVALUES"].ToString().Trim());
                                    switch (FieldNm.ToString().ToUpper().Trim())
                                    {
                                        case "RECORDTYPE":
                                        case "CURRENCYCODE":
                                        case "CLEARINGCENTRE":
                                        case "ORGANIZATION":
                                            arryRow[t] = Value;
                                            t = t + 1;
                                            break;
                                        case "FILLER":
                                            Filler = new string(BRBase.BRBaseConvert.ConvertToChar(Filler), FileFormatValue);
                                            arryRow[t] = Filler;
                                            t = t + 1;
                                            break;
                                        case "BANK":
                                            arryRow[t] = usrInfo.strBank;
                                            t = t + 1;
                                            break;
                                    }
                                }
                                break;
                        }

                        Data = string.Empty;
                        for (m = 0; m < arryRow.Length; m++)
                        {
                            Data = Data + (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
                        }
                        break;
                }
            }
            WorkingDataTable.Tables[0].Rows.Clear();
            return Data;
        }
        public static string TrailerControlVoucherUG(DS_trxClearing dstrxClearing, DS_ClearingFileFormat dsClearingFileFormat, string VoucherType, string RBank, string FileType, DateTime WorkingDate, BRBase.UserInfo usrInfo, string Currency, IDbConnection conn)
        {
            Int16 j, m;
            Int32 t;
            IDbConnection storedconn = conn;
            string ControlVoucherType = string.Empty;
            string NewValue = string.Empty;
            string Value = string.Empty;
            string Data = string.Empty;
            string CurrType = string.Empty;
            string sortOrder = "Start ASC";
            string Curr = string.Empty;
            DS_trxClearing EJdataTable = new DS_trxClearing();
            double SumCheques = 0;
            Int32 ChequesCount = 0;
            DS_ClearingFileFormat WorkingDataTable = new DS_ClearingFileFormat();
            BRBase.BRDataSet dsUniqueClearingID = new BRBase.BRDataSet();
            string UniqueID = string.Empty;
            string dd = new string('0', 2 - BRBase.BRBaseConvert.ConvertToString(WorkingDate.Day).ToString().Length) + BRBase.BRBaseConvert.ConvertToString(WorkingDate.Day);
            string mm = new string('0', 2 - BRBase.BRBaseConvert.ConvertToString(WorkingDate.Month).Length) + BRBase.BRBaseConvert.ConvertToString(WorkingDate.Month);
            string yyyy = BRBase.BRBaseConvert.ConvertToString(WorkingDate.Year);
            string ddmmmyyyy = dd + mm + yyyy;
            t = 0;
            DataRow[] drHeaderFileFormatResult = dsClearingFileFormat.Tables[0].Select("RecordType = 'CONTROLVOUCHER'", sortOrder);
            foreach (DataRow dvr in drHeaderFileFormatResult)
            {
                WorkingDataTable.Tables[0].ImportRow(dvr);
            }
            WorkingDataTable.AcceptChanges();
            Object[] arryRow = new Object[WorkingDataTable.Tables[0].Rows.Count];
            if (FileType.ToUpper() == "EJ")
            {
                if (Currency == "KES")
                {
                    Curr = "00";
                    CurrType = " AND currencyID ='KES' AND TrxType ='OC'"; //" AND currencyID ='" + Currency + "'";
                }
                else
                {
                    switch (Currency.ToString().ToUpper())
                    {
                        case "USD":
                            CurrType = " AND currencyID ='USD' AND TrxType ='OC'";
                            Curr = "60";
                            break;
                        case "GBP":
                            CurrType = " AND currencyID ='GBP' AND TrxType ='OC'";
                            Curr = "61";
                            break;
                        case "EUR":
                            CurrType = " AND currencyID ='EUR' AND TrxType ='OC'";
                            Curr = "62";
                            break;
                        case "KES":
                            CurrType = " AND currencyID ='KES' AND TrxType ='OC'";
                            Curr = "00";
                            break;
                    }
                    //CurrType = " AND currencyID = '" + Currency.ToString().ToUpper() + "' AND TrxType ='OC'";//CurrType = " AND currencyID <> '" + Currency + "'";
                }
                switch (VoucherType.ToString().ToUpper())
                {
                    case "BCV":  //Presentments 
                        SumCheques = 0;
                        ChequesCount = 0;
                        drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("ReturnCode IN ('00','17') AND VOUCHERCODE NOT IN ('03','40') AND ToBank='" + RBank + "' " + CurrType + "");//
                        foreach (DataRow dvr in drHeaderFileFormatResult)
                        {
                            EJdataTable.Tables[0].ImportRow(dvr);
                            SumCheques = SumCheques + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                            ChequesCount = ChequesCount + 1;
                        }
                        EJdataTable.AcceptChanges();
                        break;
                    case "UCV":  //Unpaids 
                        SumCheques = 0;
                        ChequesCount = 0;
                        drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("ReturnCode NOT IN ('00','17') AND ToBank='" + RBank + "' " + CurrType + "");
                        foreach (DataRow dvr in drHeaderFileFormatResult)
                        {
                            EJdataTable.Tables[0].ImportRow(dvr);
                            SumCheques = SumCheques + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                            ChequesCount = ChequesCount + 1;
                        }
                        EJdataTable.AcceptChanges();
                        break;
                    case "MDV":  //Manual Debit Voucher
                        SumCheques = 0;
                        ChequesCount = 0;
                        drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("ReturnCode IN ('00','17') AND VOUCHERCODE = '03' AND ToBank='" + RBank + "' " + CurrType + "");
                        foreach (DataRow dvr in drHeaderFileFormatResult)
                        {
                            EJdataTable.Tables[0].ImportRow(dvr);
                            SumCheques = SumCheques + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                            ChequesCount = ChequesCount + 1;
                        }
                        EJdataTable.AcceptChanges();
                        break;
                }
            }
            foreach (DataColumn Col in WorkingDataTable.Tables[0].Columns)
            {
                switch (Col.ColumnName.ToUpper())
                {
                    case "FIELDNAME":
                        switch (FileType.ToUpper())
                        {
                            case "EJ":
                                for (j = 0; j < WorkingDataTable.Tables[0].Rows.Count; j++)
                                {
                                    switch (VoucherType.ToString().ToUpper())
                                    {
                                        case "BCV":
                                            if (conn.State != ConnectionState.Open)
                                            {
                                                conn = GetConnection();
                                            }
                                            string FieldNm = string.Empty;
                                            FieldNm = WorkingDataTable.Tables[0].Rows[j][Col.ColumnName].ToString();
                                            Int32 FileFormatValue = BRBase.BRBaseConvert.ConvertToInt32(WorkingDataTable.Tables[0].Rows[j]["Length"].ToString());
                                            bool FileFormatValueMandatoryLength = WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString() == "" ? false : BRBase.BRBaseConvert.ConvertToBoolean(WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString());
                                            string Filler = WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString() == "" ? "0" : WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString();
                                            Value = BRBase.BRBaseConvert.ConvertToString(WorkingDataTable.Tables[0].Rows[j]["DEFAULTVALUES"].ToString().Trim());
                                            switch (FieldNm.ToString().ToUpper().Trim())
                                            {
                                                case "RECORDTYPE":
                                                    arryRow[t] = Value;
                                                    t = t + 1;
                                                    break;
                                                case "CURRENCYCODE":
                                                    arryRow[t] = Curr;
                                                    t = t + 1;
                                                    break;
                                                case "POSITIONOFAMOUNT":
                                                    arryRow[t] = 0;
                                                    t = t + 1;
                                                    break;
                                                case "PSERIALNUMBER":
                                                    dsUniqueClearingID = new BRBase.BRDataSet();
                                                    UniqueID = string.Empty;
                                                    ClearingUniversalMethod(usrInfo, "p_GetUniqueClearingID", out dsUniqueClearingID, BRBase.BRModule.InwardFileImportation, conn, new object[] { "BrDataSet" }, new object[] { FileFormatValue });
                                                    if (dsUniqueClearingID.Tables.Count > 0)
                                                    {
                                                        if (dsUniqueClearingID.Tables[0].Rows.Count > 0)
                                                        {
                                                            UniqueID = dsUniqueClearingID.Tables[0].Rows[0]["UniqueID"].ToString();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        UniqueID = RBank + dd + mm;
                                                    }

                                                    UniqueID = GetNextInt16().ToString();
                                                    arryRow[t] = new string('0', FileFormatValue - UniqueID.Length) + UniqueID;
                                                    t = t + 1;
                                                    break;
                                                case "FILLER":
                                                    Filler = new string(BRBase.BRBaseConvert.ConvertToChar(Filler), FileFormatValue);
                                                    arryRow[t] = Filler;
                                                    t = t + 1;
                                                    break;
                                                case "VALUE":
                                                    // Pramod
                                                    //NewValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(SumCheques)));
                                                    if (Curr == "00")
                                                    {
                                                        NewValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(SumCheques)));
                                                    }
                                                    else
                                                    {
                                                        NewValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(SumCheques), "60"));
                                                    }
                                                    NewValue = new string('0', FileFormatValue - NewValue.Length) + NewValue;
                                                    arryRow[t] = NewValue;
                                                    t = t + 1;
                                                    break;
                                                case "RCLEARINGCENTRECODE":
                                                    arryRow[t] = RBank;
                                                    t = t + 1;
                                                    break;
                                                case "PCLEARINGCENTRECODE":
                                                    arryRow[t] = usrInfo.strBank;
                                                    t = t + 1;
                                                    break;
                                                case "DRN":
                                                    
                                                   dsUniqueClearingID = new BRBase.BRDataSet();
                                                    UniqueID = string.Empty;
                                                    ClearingUniversalMethod(usrInfo, "p_GetUniqueClearingREFID", out dsUniqueClearingID, BRBase.BRModule.InwardFileImportation, conn, new object[] { "BrDataSet" }, new object[] { });
                                                    if (dsUniqueClearingID.Tables.Count > 0)
                                                    {
                                                        if (dsUniqueClearingID.Tables[0].Rows.Count > 0)
                                                        {
                                                            UniqueID = dsUniqueClearingID.Tables[0].Rows[0]["UniqueID"].ToString();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        UniqueID = RBank + dd + mm;
                                                    }
                                                    dsUniqueClearingID.Tables[0].Clear();
                                                    UniqueID = GetNextInt16().ToString();
                                                   //UniqueID = GenerateRandomAlphaNumericCode(FileFormatValue).ToUpper();
                                                   arryRow[t] = new string('0', FileFormatValue - UniqueID.Length) + UniqueID;
                                                    t = t + 1;
                                                    break;
                                                case "VOUCHERCODE":
                                                    arryRow[t] = "71";
                                                    t = t + 1;
                                                    break;
                                            }
                                            break;
                                        case "UCV":
                                            if (conn.State != ConnectionState.Open)
                                            {
                                                conn = GetConnection();
                                            }
                                            FieldNm = string.Empty;
                                            FieldNm = WorkingDataTable.Tables[0].Rows[j][Col.ColumnName].ToString();
                                            FileFormatValue = BRBase.BRBaseConvert.ConvertToInt32(WorkingDataTable.Tables[0].Rows[j]["Length"].ToString());
                                            FileFormatValueMandatoryLength = WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString() == "" ? false : BRBase.BRBaseConvert.ConvertToBoolean(WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString());
                                            Filler = WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString() == "" ? "0" : WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString();
                                            Value = BRBase.BRBaseConvert.ConvertToString(WorkingDataTable.Tables[0].Rows[j]["DEFAULTVALUES"].ToString().Trim());
                                            switch (FieldNm.ToString().ToUpper().Trim())
                                            {
                                                case "RECORDTYPE":
                                                    arryRow[t] = Value;
                                                    t = t + 1;
                                                    break;
                                                case "CURRENCYCODE":
                                                    arryRow[t] = Curr;
                                                    t = t + 1;
                                                    break;
                                                case "POSITIONOFAMOUNT":
                                                    arryRow[t] = 0;
                                                    t = t + 1;
                                                    break;
                                                case "PSERIALNUMBER":
                                                    dsUniqueClearingID = new BRBase.BRDataSet();
                                                    UniqueID = string.Empty;
                                                    //ClearingUniversalMethod(usrInfo, "p_GetUniqueClearingID", out dsUniqueClearingID, BRBase.BRModule.InwardFileImportation, conn, new object[] { "BrDataSet" }, new object[] { FileFormatValue });
                                                    //if (dsUniqueClearingID.Tables.Count > 0)
                                                    //{
                                                    //    if (dsUniqueClearingID.Tables[0].Rows.Count > 0)
                                                    //    {
                                                    //        UniqueID = dsUniqueClearingID.Tables[0].Rows[0]["UniqueID"].ToString();
                                                    //    }
                                                    //}
                                                    //else
                                                    //{
                                                    //    UniqueID = RBank + dd + mm;
                                                    //}
                                                    UniqueID = GetNextInt16().ToString();
                                                    arryRow[t] = new string('0', FileFormatValue - UniqueID.Length) + UniqueID;
                                                    //arryRow[t] = UniqueID;
                                                    t = t + 1;
                                                    break;
                                                case "FILLER":
                                                    Filler = new string(BRBase.BRBaseConvert.ConvertToChar(Filler), FileFormatValue);
                                                    arryRow[t] = Filler;
                                                    t = t + 1;
                                                    break;
                                                case "VALUE":
                                                    // Pramod
                                                    //NewValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(SumCheques)));
                                                    if (Curr == "00")
                                                    {
                                                        NewValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(SumCheques)));
                                                    }
                                                    else
                                                    {
                                                        NewValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(SumCheques), "60"));
                                                    }
                                                    NewValue = new string('0', 14 - NewValue.Length) + NewValue;
                                                    arryRow[t] = NewValue;
                                                    t = t + 1;
                                                    break;
                                                case "PCLEARINGCENTRECODE":
                                                    arryRow[t] = usrInfo.strBank;
                                                    t = t + 1;
                                                    break;
                                                case "RCLEARINGCENTRECODE":
                                                    arryRow[t] = RBank;
                                                    t = t + 1;
                                                    break;
                                                case "DRN":
                                                    dsUniqueClearingID = new BRBase.BRDataSet();
                                                    UniqueID = string.Empty;
                                                    //NewValue = string.Empty;
                                                    //ClearingUniversalMethod(usrInfo, "p_GetUniqueClearingID", out dsUniqueClearingID, BRBase.BRModule.InwardFileImportation, conn, new object[] { "BrDataSet" }, new object[] { FileFormatValue });
                                                    //if (dsUniqueClearingID.Tables.Count > 0)
                                                    //{
                                                    //    if (dsUniqueClearingID.Tables[0].Rows.Count > 0)
                                                    //    {
                                                    //        UniqueID = dsUniqueClearingID.Tables[0].Rows[0]["UniqueID"].ToString();
                                                    //    }
                                                    //}
                                                    //else
                                                    //{
                                                    //    UniqueID = RBank + dd + mm;
                                                    //}
                                                    UniqueID = GetNextInt16().ToString();
                                                    //UniqueID = usrInfo.strBank + UniqueID;
                                                    //NewValue = new string(BRBase.BRBaseConvert.ConvertToChar(Filler), (BRBase.BRBaseConvert.ConvertToInt32(FileFormatValue) - BRBase.BRBaseConvert.ConvertToInt32(UniqueID.Length))) + UniqueID;
                                                    arryRow[t] = new string('0', FileFormatValue - UniqueID.Length) + UniqueID;
                                                    //arryRow[t] = NewValue;
                                                    t = t + 1;
                                                    break;
                                                case "VOUCHERCODE":
                                                    arryRow[t] = "72";
                                                    t = t + 1;
                                                    break;
                                            }
                                            break;
                                        case "MDV":
                                            if (conn.State != ConnectionState.Open)
                                            {
                                                conn = GetConnection();
                                            }
                                            FieldNm = string.Empty;
                                            FieldNm = WorkingDataTable.Tables[0].Rows[j][Col.ColumnName].ToString();
                                            FileFormatValue = BRBase.BRBaseConvert.ConvertToInt32(WorkingDataTable.Tables[0].Rows[j]["Length"].ToString());
                                            FileFormatValueMandatoryLength = WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString() == "" ? false : BRBase.BRBaseConvert.ConvertToBoolean(WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString());
                                            Filler = WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString() == "" ? "0" : WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString();
                                            Value = BRBase.BRBaseConvert.ConvertToString(WorkingDataTable.Tables[0].Rows[j]["DEFAULTVALUES"].ToString().Trim());
                                            switch (FieldNm.ToString().ToUpper().Trim())
                                            {
                                                case "RECORDTYPE":
                                                    arryRow[t] = Value;
                                                    t = t + 1;
                                                    break;
                                                case "CURRENCYCODE":
                                                    arryRow[t] = Curr;
                                                    t = t + 1;
                                                    break;
                                                case "POSITIONOFAMOUNT":
                                                    arryRow[t] = 0;
                                                    t = t + 1;
                                                    break;
                                                case "PSERIALNUMBER":
                                                    dsUniqueClearingID = new BRBase.BRDataSet();
                                                    UniqueID = string.Empty;
                                                    ClearingUniversalMethod(usrInfo, "p_GetUniqueClearingID", out dsUniqueClearingID, BRBase.BRModule.InwardFileImportation, conn, new object[] { "BrDataSet" }, new object[] { FileFormatValue });
                                                    if (dsUniqueClearingID.Tables.Count > 0)
                                                    {
                                                        if (dsUniqueClearingID.Tables[0].Rows.Count > 0)
                                                        {
                                                            UniqueID = dsUniqueClearingID.Tables[0].Rows[0]["UniqueID"].ToString();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        UniqueID = RBank + dd + mm;
                                                    }
                                                    UniqueID = GetNextInt16().ToString();
                                                    arryRow[t] = new string('0', FileFormatValue - UniqueID.Length) + UniqueID;
                                                    arryRow[t] = UniqueID;
                                                    t = t + 1;
                                                    break;
                                                case "FILLER":
                                                    Filler = new string(BRBase.BRBaseConvert.ConvertToChar(Filler), FileFormatValue);
                                                    arryRow[t] = Filler;
                                                    t = t + 1;
                                                    break;
                                                case "VALUE":
                                                    // Pramod
                                                    //NewValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(SumCheques)));
                                                    if (Curr == "00")
                                                    {
                                                        NewValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(SumCheques)));
                                                    }
                                                    else
                                                    {
                                                        NewValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(SumCheques), "60"));
                                                    }

                                                    NewValue = new string('0', 14 - NewValue.Length) + NewValue;
                                                    arryRow[t] = NewValue;
                                                    t = t + 1;
                                                    break;
                                                case "PCLEARINGCENTRECODE":
                                                    arryRow[t] = RBank;
                                                    t = t + 1;
                                                    break;
                                                case "RCLEARINGCENTRECODE":
                                                    arryRow[t] = usrInfo.strBank;
                                                    t = t + 1;
                                                    break;
                                                case "DRN":
                                                    dsUniqueClearingID = new BRBase.BRDataSet();
                                                    UniqueID = string.Empty;
                                                    NewValue = string.Empty;
                                                    ClearingUniversalMethod(usrInfo, "p_GetUniqueClearingID", out dsUniqueClearingID, BRBase.BRModule.InwardFileImportation, conn, new object[] { "BrDataSet" }, new object[] { FileFormatValue });
                                                    if (dsUniqueClearingID.Tables.Count > 0)
                                                    {
                                                        if (dsUniqueClearingID.Tables[0].Rows.Count > 0)
                                                        {
                                                            UniqueID = dsUniqueClearingID.Tables[0].Rows[0]["UniqueID"].ToString();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        UniqueID = RBank + dd + mm;
                                                    }
                                                    UniqueID = GetNextInt16().ToString();
                                                    //UniqueID = usrInfo.strBank + UniqueID;
                                                    NewValue = new string(BRBase.BRBaseConvert.ConvertToChar(Filler), (BRBase.BRBaseConvert.ConvertToInt32(FileFormatValue) - BRBase.BRBaseConvert.ConvertToInt32(UniqueID.Length))) + UniqueID;
                                                    arryRow[t] = NewValue;
                                                    t = t + 1;
                                                    break;
                                                case "VOUCHERCODE":
                                                    arryRow[t] = "73";
                                                    t = t + 1;
                                                    break;
                                            }
                                            break;
                                    }
                                }
                                break;
                            case "EFT":
                                for (j = 0; j < WorkingDataTable.Tables[0].Rows.Count; j++)
                                {
                                    if (conn.State != ConnectionState.Open)
                                    {
                                        conn = GetConnection();
                                    }
                                    string FieldNm = string.Empty;
                                    FieldNm = WorkingDataTable.Tables[0].Rows[j][Col.ColumnName].ToString();
                                    Int32 FileFormatValue = BRBase.BRBaseConvert.ConvertToInt32(WorkingDataTable.Tables[0].Rows[j]["Length"].ToString());
                                    bool FileFormatValueMandatoryLength = WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString() == "" ? false : BRBase.BRBaseConvert.ConvertToBoolean(WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString());
                                    string Filler = WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString() == "" ? "0" : WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString();
                                    Value = BRBase.BRBaseConvert.ConvertToString(WorkingDataTable.Tables[0].Rows[j]["DEFAULTVALUES"].ToString().Trim());
                                    switch (FieldNm.ToString().ToUpper().Trim())
                                    {
                                        case "RECORDTYPE":
                                        case "PORGANIZATION":
                                            arryRow[t] = Value;
                                            t = t + 1;
                                            break;
                                        case "PBANK":
                                            arryRow[t] = usrInfo.strBank;
                                            t = t + 1;
                                            break;
                                        case "FILLER":
                                        case "FILEINDICATOR":
                                            Filler = new string(BRBase.BRBaseConvert.ConvertToChar(Filler), FileFormatValue);
                                            arryRow[t] = Filler;
                                            t = t + 1;
                                            break;
                                        case "PSERIALNUMBER":
                                            dsUniqueClearingID = new BRBase.BRDataSet();
                                            UniqueID = string.Empty;
                                            ClearingUniversalMethod(usrInfo, "p_GetUniqueClearingID", out dsUniqueClearingID, BRBase.BRModule.InwardFileImportation, conn, new object[] { "BrDataSet" }, new object[] { FileFormatValue });
                                            if (dsUniqueClearingID.Tables.Count > 0)
                                            {
                                                if (dsUniqueClearingID.Tables[0].Rows.Count > 0)
                                                {
                                                    UniqueID = dsUniqueClearingID.Tables[0].Rows[0]["UniqueID"].ToString();
                                                }
                                            }
                                            else
                                            {
                                                UniqueID = RBank + dd + mm;
                                            }
                                            arryRow[t] = UniqueID;
                                            t = t + 1;
                                            break;
                                    }
                                }
                                break;
                            case "DISC":
                                for (j = 0; j < WorkingDataTable.Tables[0].Rows.Count; j++)
                                {
                                    if (conn.State != ConnectionState.Open)
                                    {
                                        conn = GetConnection();
                                    }
                                    string FieldNm = string.Empty;
                                    FieldNm = WorkingDataTable.Tables[0].Rows[j][Col.ColumnName].ToString();
                                    Int32 FileFormatValue = BRBase.BRBaseConvert.ConvertToInt32(WorkingDataTable.Tables[0].Rows[j]["Length"].ToString());
                                    bool FileFormatValueMandatoryLength = WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString() == "" ? false : BRBase.BRBaseConvert.ConvertToBoolean(WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString());
                                    string Filler = WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString() == "" ? "0" : WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString();
                                    Value = BRBase.BRBaseConvert.ConvertToString(WorkingDataTable.Tables[0].Rows[j]["DEFAULTVALUES"].ToString().Trim());
                                    switch (FieldNm.ToString().ToUpper().Trim())
                                    {
                                        case "RECORDTYPE":
                                        case "PBANK":
                                        case "PORGANIZATION":
                                            arryRow[t] = Value;
                                            t = t + 1;
                                            break;
                                        case "FILLER":
                                        case "PTRXCOUNT":
                                        case "PVALUECREDIT":
                                        case "PVALUEDEBIT":
                                            Filler = new string(BRBase.BRBaseConvert.ConvertToChar(Filler), FileFormatValue);
                                            arryRow[t] = Filler;
                                            t = t + 1;
                                            break;
                                        case "PCLEARINGCENTRE":
                                            arryRow[t] = usrInfo.strBank;
                                            t = t + 1;
                                            break;
                                    }
                                }
                                break;
                            case "SELT":
                                for (j = 0; j < WorkingDataTable.Tables[0].Rows.Count; j++)
                                {
                                    if (conn.State != ConnectionState.Open)
                                    {
                                        conn = GetConnection();
                                    }
                                    string FieldNm = string.Empty;
                                    FieldNm = WorkingDataTable.Tables[0].Rows[j][Col.ColumnName].ToString();
                                    Int32 FileFormatValue = BRBase.BRBaseConvert.ConvertToInt32(WorkingDataTable.Tables[0].Rows[j]["Length"].ToString());
                                    bool FileFormatValueMandatoryLength = WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString() == "" ? false : BRBase.BRBaseConvert.ConvertToBoolean(WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString());
                                    string Filler = WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString() == "" ? "0" : WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString();
                                    Value = BRBase.BRBaseConvert.ConvertToString(WorkingDataTable.Tables[0].Rows[j]["DEFAULTVALUES"].ToString().Trim());
                                    switch (FieldNm.ToString().ToUpper().Trim())
                                    {
                                        case "RECORDTYPE":
                                        case "CURRENCYCODE":
                                        case "CLEARINGCENTRE":
                                        case "ORGANIZATION":
                                            arryRow[t] = Value;
                                            t = t + 1;
                                            break;
                                        case "FILLER":
                                            Filler = new string(BRBase.BRBaseConvert.ConvertToChar(Filler), FileFormatValue);
                                            arryRow[t] = Filler;
                                            t = t + 1;
                                            break;
                                        case "BANK":
                                            arryRow[t] = usrInfo.strBank;
                                            t = t + 1;
                                            break;
                                    }
                                }
                                break;
                        }

                        Data = string.Empty;
                        for (m = 0; m < arryRow.Length; m++)
                        {
                            Data = Data + (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
                        }
                        break;
                }
            }
            WorkingDataTable.Tables[0].Rows.Clear();
            return Data;
        }


        public static string LeadingControlVoucher(DS_trxClearing dstrxClearing, DS_ClearingFileFormat dsClearingFileFormat, string RBank, string FileType, DateTime WorkingDate, BRBase.UserInfo usrInfo, string VoucherType, string Currency, IDbConnection conn)
        {
            if (RBank == "MFI")
            {
                RBank = usrInfo.strBank;
            }
            Int16 j, m;
            Int32 t;
            IDbConnection storedconn = conn;
            string sortOrder = "Start ASC";
            string ControlVoucherType = string.Empty;
            string NewValue = string.Empty;
            string Value = string.Empty;
            string Data = string.Empty;
            string CurrType = string.Empty;
            string Curr = string.Empty;
            DS_trxClearing EJdataTable = new DS_trxClearing();
            double SumCheques = 0;
            DS_ClearingFileFormat WorkingDataTable = new DS_ClearingFileFormat();
            ArrayList arr = new ArrayList();
            string dd = new string('0', 2 - BRBase.BRBaseConvert.ConvertToString(WorkingDate.Day).ToString().Length) + BRBase.BRBaseConvert.ConvertToString(WorkingDate.Day);
            string mm = new string('0', 2 - BRBase.BRBaseConvert.ConvertToString(WorkingDate.Month).Length) + BRBase.BRBaseConvert.ConvertToString(WorkingDate.Month);
            string yyyy = BRBase.BRBaseConvert.ConvertToString(WorkingDate.Year);
            string ddmmmyyyy = dd + mm + yyyy;
            t = 0;
            DataRow[] drHeaderFileFormatResult = dsClearingFileFormat.Tables[0].Select("RecordType = 'CONTROLVOUCHER'", sortOrder);
            foreach (DataRow dvr in drHeaderFileFormatResult)
            {
                WorkingDataTable.Tables[0].ImportRow(dvr);
            }
            WorkingDataTable.AcceptChanges();
            Object[] arryRow = new Object[WorkingDataTable.Tables[0].Rows.Count];


            if (FileType.ToUpper() == "EJ")
            {
                if (Currency == "KES")
                {
                    Curr = "00";
                    CurrType = " AND CurrencyID ='KES' AND TrxType ='OC'"; //" AND currencyID ='" + Currency + "'";
                }
                else
                {
                    switch (Currency.ToString().ToUpper())
                    {
                        case "USD":
                            Curr = "60";
                            break;
                        case "GBP":
                            Curr = "61";
                            break;
                        case "EUR":
                            Curr = "62";
                            break;
                    }
                    CurrType = " AND CurrencyID = '" + Currency + "' AND TrxType ='OC'";//CurrType = " AND currencyID <> '" + Currency + "'";
                }

                drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("ToBank='" + RBank + "' " + CurrType + "");
                foreach (DataRow dvr in drHeaderFileFormatResult)
                {
                    EJdataTable.Tables[0].ImportRow(dvr);
                    SumCheques = SumCheques + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                }
                EJdataTable.AcceptChanges();
            }
            foreach (DataColumn Col in WorkingDataTable.Tables[0].Columns)
            {
                switch (Col.ColumnName.ToUpper())
                {
                    case "FIELDNAME":

                        switch (FileType.ToUpper())
                        {
                            case "EJ":
                                for (j = 0; j < WorkingDataTable.Tables[0].Rows.Count; j++)
                                {
                                    if (conn.State != ConnectionState.Open)
                                    {
                                        conn = GetConnection();
                                    }
                                    string FieldNm = string.Empty;
                                    FieldNm = WorkingDataTable.Tables[0].Rows[j][Col.ColumnName].ToString();
                                    Int32 FileFormatValue = BRBase.BRBaseConvert.ConvertToInt32(WorkingDataTable.Tables[0].Rows[j]["Length"].ToString());
                                    bool FileFormatValueMandatoryLength = WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString() == "" ? false : BRBase.BRBaseConvert.ConvertToBoolean(WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString());
                                    string Filler = WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString() == "" ? "0" : WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString();
                                    Value = BRBase.BRBaseConvert.ConvertToString(WorkingDataTable.Tables[0].Rows[j]["DEFAULTVALUES"].ToString().Trim());
                                    switch (FieldNm.ToString().ToUpper().Trim())
                                    {
                                        case "RECORDTYPE":
                                            arryRow[t] = Value;
                                            t = t + 1;
                                            break;
                                        case "CURRENCYCODE":
                                            arryRow[t] = Curr;
                                            t = t + 1;
                                            break;
                                        case "POSITIONOFAMOUNT":
                                            arryRow[t] = 0;
                                            t = t + 1;
                                            break;
                                        case "PSERIALNUMBER":
                                            BRBase.BRDataSet dsUniqueClearingID = new BRBase.BRDataSet();
                                            string UniqueID = string.Empty;
                                            ClearingUniversalMethod(usrInfo, "p_GetUniqueClearingID", out dsUniqueClearingID, BRBase.BRModule.InwardFileImportation, conn, new object[] { "BrDataSet" }, new object[] { FileFormatValue });

                                            if (dsUniqueClearingID.Tables.Count > 0)
                                            {
                                                if (dsUniqueClearingID.Tables[0].Rows.Count > 0)
                                                {
                                                    UniqueID = dsUniqueClearingID.Tables[0].Rows[0]["UniqueID"].ToString();
                                                }
                                            }
                                            else
                                            {
                                                UniqueID = RBank + dd + mm;
                                            }
                                            arryRow[t] = UniqueID;
                                            t = t + 1;
                                            break;
                                        case "FILLER":
                                            Filler = new string(BRBase.BRBaseConvert.ConvertToChar(Filler), FileFormatValue);
                                            arryRow[t] = Filler;
                                            t = t + 1;
                                            break;
                                        case "VALUE":
                                            NewValue = "0";//BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBaseConvert.ConvertToDouble(SumCheques)));
                                            NewValue = new string('0', 13 - NewValue.Length) + NewValue;
                                            arryRow[t] = NewValue;
                                            t = t + 1;
                                            break;
                                        case "PCLEARINGCENTRECODE":
                                            arryRow[t] = RBank;
                                            t = t + 1;
                                            break;
                                        case "RCLEARINGCENTRECODE":
                                            arryRow[t] = usrInfo.strBank;
                                            t = t + 1;
                                            break;
                                        case "DRN":
                                            dsUniqueClearingID = new BRBase.BRDataSet();
                                            UniqueID = string.Empty;
                                            NewValue = string.Empty;
                                            ClearingUniversalMethod(usrInfo, "p_GetUniqueClearingID", out dsUniqueClearingID, BRBase.BRModule.InwardFileImportation, conn, new object[] { "BrDataSet" }, new object[] { FileFormatValue });
                                            if (dsUniqueClearingID.Tables.Count > 0)
                                            {
                                                if (dsUniqueClearingID.Tables[0].Rows.Count > 0)
                                                {
                                                    UniqueID = dsUniqueClearingID.Tables[0].Rows[0]["UniqueID"].ToString();
                                                }
                                            }
                                            else
                                            {
                                                UniqueID = RBank + dd + mm;
                                            }
                                            UniqueID = usrInfo.strBank + UniqueID;
                                            NewValue = new string(BRBase.BRBaseConvert.ConvertToChar(Filler), (BRBase.BRBaseConvert.ConvertToInt32(FileFormatValue) - BRBase.BRBaseConvert.ConvertToInt32(UniqueID.Length))) + UniqueID;
                                            arryRow[t] = NewValue;
                                            t = t + 1;
                                            break;
                                        case "VOUCHERCODE":
                                            arryRow[t] = "70";
                                            t = t + 1;
                                            break;
                                        case "DATE":
                                            arryRow[t] = ddmmmyyyy;
                                            t = t + 1;
                                            break;
                                    }
                                }
                                break;
                            case "EFT":
                                for (j = 0; j < WorkingDataTable.Tables[0].Rows.Count; j++)
                                {
                                    if (conn.State != ConnectionState.Open)
                                    {
                                        conn = GetConnection();
                                    }
                                    string FieldNm = string.Empty;
                                    FieldNm = WorkingDataTable.Tables[0].Rows[j][Col.ColumnName].ToString();
                                    Int32 FileFormatValue = BRBase.BRBaseConvert.ConvertToInt32(WorkingDataTable.Tables[0].Rows[j]["Length"].ToString());
                                    bool FileFormatValueMandatoryLength = WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString() == "" ? false : BRBase.BRBaseConvert.ConvertToBoolean(WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString());
                                    string Filler = WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString() == "" ? "0" : WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString();
                                    Value = BRBase.BRBaseConvert.ConvertToString(WorkingDataTable.Tables[0].Rows[j]["DEFAULTVALUES"].ToString().Trim());
                                    switch (FieldNm.ToString().ToUpper().Trim())
                                    {
                                        case "RECORDTYPE":
                                        case "BANK":
                                        case "PORGANIZATION":
                                        case "LASTFILEINDICATOR":
                                            arryRow[t] = Value;
                                            t = t + 1;
                                            break;
                                        case "PSERIALNUMBER":
                                            BRBase.BRDataSet dsUniqueClearingID = new BRBase.BRDataSet();
                                            string UniqueID = string.Empty;
                                            ClearingUniversalMethod(usrInfo, "p_GetUniqueClearingID", out dsUniqueClearingID, BRBase.BRModule.InwardFileImportation, conn, new object[] { "BrDataSet" }, new object[] { FileFormatValue });
                                            if (dsUniqueClearingID.Tables.Count > 0)
                                            {
                                                if (dsUniqueClearingID.Tables[0].Rows.Count > 0)
                                                {
                                                    UniqueID = dsUniqueClearingID.Tables[0].Rows[0]["UniqueID"].ToString();
                                                }
                                            }
                                            else
                                            {
                                                UniqueID = RBank + dd + mm;
                                            }
                                            arryRow[t] = UniqueID;
                                            t = t + 1;
                                            break;
                                        case "FILLER":
                                            Filler = new string(BRBase.BRBaseConvert.ConvertToChar(Filler), FileFormatValue);
                                            arryRow[t] = Filler;
                                            t = t + 1;
                                            break;
                                        case "CLEARINGCENTRE":
                                            arryRow[t] = usrInfo.strBank;
                                            t = t + 1;
                                            break;
                                        case "DATE":
                                            arryRow[t] = ddmmmyyyy;
                                            t = t + 1;
                                            break;
                                        case "FILETYPE":
                                            arryRow[t] = "70";
                                            t = t + 1;
                                            break;
                                    }
                                }
                                break;
                            case "DISC":
                                for (j = 0; j < WorkingDataTable.Tables[0].Rows.Count; j++)
                                {
                                    if (conn.State != ConnectionState.Open)
                                    {
                                        conn = GetConnection();
                                    }
                                    string FieldNm = string.Empty;
                                    FieldNm = WorkingDataTable.Tables[0].Rows[j][Col.ColumnName].ToString();
                                    Int32 FileFormatValue = BRBase.BRBaseConvert.ConvertToInt32(WorkingDataTable.Tables[0].Rows[j]["Length"].ToString());
                                    bool FileFormatValueMandatoryLength = WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString() == "" ? false : BRBase.BRBaseConvert.ConvertToBoolean(WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString());
                                    string Filler = WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString() == "" ? "0" : WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString();
                                    Value = BRBase.BRBaseConvert.ConvertToString(WorkingDataTable.Tables[0].Rows[j]["DEFAULTVALUES"].ToString().Trim());
                                    switch (FieldNm.ToString().ToUpper().Trim())
                                    {
                                        case "RECORDTYPE":
                                        case "FILETYPE":
                                        case "PBANK":
                                        case "PORGANIZATION":
                                        case "FILEINDICATOR":
                                            arryRow[t] = Value;
                                            t = t + 1;
                                            break;
                                        case "PSERIALNUMBER":
                                            BRBase.BRDataSet dsUniqueClearingID = new BRBase.BRDataSet();
                                            string UniqueID = string.Empty;
                                            ClearingUniversalMethod(usrInfo, "p_GetUniqueClearingID", out dsUniqueClearingID, BRBase.BRModule.InwardFileImportation, conn, new object[] { "BrDataSet" }, new object[] { FileFormatValue });
                                            if (dsUniqueClearingID.Tables.Count > 0)
                                            {
                                                if (dsUniqueClearingID.Tables[0].Rows.Count > 0)
                                                {
                                                    UniqueID = dsUniqueClearingID.Tables[0].Rows[0]["UniqueID"].ToString();
                                                }
                                            }
                                            else
                                            {
                                                UniqueID = RBank + dd + mm;
                                            }
                                            arryRow[t] = UniqueID;
                                            t = t + 1;
                                            break;
                                        case "DATE":
                                            arryRow[t] = ddmmmyyyy;
                                            t = t + 1;
                                            break;
                                        case "FILLER":
                                            Filler = new string(BRBase.BRBaseConvert.ConvertToChar(Filler), FileFormatValue);
                                            arryRow[t] = Filler;
                                            t = t + 1;
                                            break;
                                        case "PCLEARINGCENTRECODE":
                                            arryRow[t] = usrInfo.strBank;
                                            t = t + 1;
                                            break;
                                    }
                                }
                                break;
                            case "SELT":
                                for (j = 0; j < WorkingDataTable.Tables[0].Rows.Count; j++)
                                {
                                    if (conn.State != ConnectionState.Open)
                                    {
                                        conn = GetConnection();
                                    }
                                    string FieldNm = string.Empty;
                                    FieldNm = WorkingDataTable.Tables[0].Rows[j][Col.ColumnName].ToString();
                                    Int32 FileFormatValue = BRBase.BRBaseConvert.ConvertToInt32(WorkingDataTable.Tables[0].Rows[j]["Length"].ToString());
                                    bool FileFormatValueMandatoryLength = WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString() == "" ? false : BRBase.BRBaseConvert.ConvertToBoolean(WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString());
                                    string Filler = WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString() == "" ? "0" : WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString();
                                    Value = BRBase.BRBaseConvert.ConvertToString(WorkingDataTable.Tables[0].Rows[j]["DEFAULTVALUES"].ToString().Trim());
                                    switch (FieldNm.ToString().ToUpper().Trim())
                                    {
                                        case "RECORDTYPE":
                                        case "CURRENCYCODE":
                                        case "FILETYPE":
                                        case "CLEARINGCENTRE":
                                        case "ORGANISATION":
                                            arryRow[t] = Value;
                                            t = t + 1;
                                            break;
                                        case "PSERIALNUMBER":
                                            BRBase.BRDataSet dsUniqueClearingID = new BRBase.BRDataSet();
                                            string UniqueID = string.Empty;
                                            ClearingUniversalMethod(usrInfo, "p_GetUniqueClearingID", out dsUniqueClearingID,BRBase.BRModule.InwardFileImportation, conn, new object[] { "BrDataSet" }, new object[] { FileFormatValue });
                                            if (dsUniqueClearingID.Tables.Count > 0)
                                            {
                                                if (dsUniqueClearingID.Tables[0].Rows.Count > 0)
                                                {
                                                    UniqueID = dsUniqueClearingID.Tables[0].Rows[0]["UniqueID"].ToString();
                                                }
                                            }
                                            else
                                            {
                                                UniqueID = RBank + dd + mm;
                                            }
                                            arryRow[t] = UniqueID;
                                            t = t + 1;
                                            break;
                                        case "FILLER":
                                        case "FILEINDICATOR":
                                            Filler = new string(BRBase.BRBaseConvert.ConvertToChar(Filler), FileFormatValue);
                                            arryRow[t] = Filler;
                                            t = t + 1;
                                            break;
                                        case "BANK":
                                            arryRow[t] = usrInfo.strBank;
                                            t = t + 1;
                                            break;
                                        case "DATE":
                                            arryRow[t] = ddmmmyyyy;
                                            t = t + 1;
                                            break;
                                    }
                                }
                                break;
                        }
                        break;
                }
            }
            Data = string.Empty;
            for (m = 0; m < arryRow.Length; m++)
            {
                Data = Data + (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
            }
            WorkingDataTable.Tables[0].Rows.Clear();
            return Data;
        }
        public static string LeadingControlVoucherUG(DS_trxClearing dstrxClearing, DS_ClearingFileFormat dsClearingFileFormat, string RBank, string FileType, DateTime WorkingDate, BRBase.UserInfo usrInfo, string VoucherType, string Currency, IDbConnection conn)
        {
            Int16 j, m;
            Int32 t;
            IDbConnection storedconn = conn;
            string ControlVoucherType = string.Empty;
            string NewValue = string.Empty;
            string Value = string.Empty;
            string Data = string.Empty;
            string sortOrder = "Start ASC";
            string CurrType = string.Empty;
            string Curr = string.Empty;
            DS_trxClearing EJdataTable = new DS_trxClearing();
            double SumCheques = 0;
            DS_ClearingFileFormat WorkingDataTable = new DS_ClearingFileFormat();
            ArrayList arr = new ArrayList();
            string dd = new string('0', 2 - BRBase.BRBaseConvert.ConvertToString(WorkingDate.Day).ToString().Length) + BRBase.BRBaseConvert.ConvertToString(WorkingDate.Day);
            string mm = new string('0', 2 - BRBase.BRBaseConvert.ConvertToString(WorkingDate.Month).Length) + BRBase.BRBaseConvert.ConvertToString(WorkingDate.Month);
            string yyyy = BRBase.BRBaseConvert.ConvertToString(WorkingDate.Year);
            string ddmmmyyyy = dd + mm + yyyy;
            t = 0;
            DataRow[] drHeaderFileFormatResult = dsClearingFileFormat.Tables[0].Select("RecordType = 'CONTROLVOUCHER'", sortOrder);
            foreach (DataRow dvr in drHeaderFileFormatResult)
            {
                WorkingDataTable.Tables[0].ImportRow(dvr);
            }
            WorkingDataTable.AcceptChanges();
            Object[] arryRow = new Object[WorkingDataTable.Tables[0].Rows.Count];


            if (FileType.ToUpper() == "EJ")
            {
                if (Currency == "UGX")
                {
                    Curr = "00";
                    CurrType = " AND CurrencyID ='UGX' AND TrxType ='OC'"; //" AND currencyID ='" + Currency + "'";
                }
                else
                {
                    switch (Currency.ToString().ToUpper())
                    {
                        case "USD":
                            Curr = "22";
                            break;
                        case "GBP":
                            Curr = "24";
                            break;
                        case "EUR":
                            Curr = "23";
                            break;
                        case "KES":
                            Curr = "25";
                            break;
                    }
                    CurrType = " AND CurrencyID = '" + Currency + "' AND TrxType ='OC'";//CurrType = " AND currencyID <> '" + Currency + "'";
                }

                drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("ToBank='" + RBank + "' " + CurrType + "");
                foreach (DataRow dvr in drHeaderFileFormatResult)
                {
                    EJdataTable.Tables[0].ImportRow(dvr);
                    SumCheques = SumCheques + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                }
                //MessageBox.Show("Imemaliza CV");
                EJdataTable.AcceptChanges();
            }
            foreach (DataColumn Col in WorkingDataTable.Tables[0].Columns)
            {
                switch (Col.ColumnName.ToUpper())
                {
                    case "FIELDNAME":

                        switch (FileType.ToUpper())
                        {
                            case "EJ":
                                for (j = 0; j < WorkingDataTable.Tables[0].Rows.Count; j++)
                                {
                                    if (conn.State != ConnectionState.Open)
                                    {
                                        conn = GetConnection();
                                    }
                                    string FieldNm = string.Empty;
                                    FieldNm = WorkingDataTable.Tables[0].Rows[j][Col.ColumnName].ToString();
                                    Int32 FileFormatValue = BRBase.BRBaseConvert.ConvertToInt32(WorkingDataTable.Tables[0].Rows[j]["Length"].ToString());
                                    bool FileFormatValueMandatoryLength = WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString() == "" ? false : BRBase.BRBaseConvert.ConvertToBoolean(WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString());
                                    string Filler = WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString() == "" ? "0" : WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString();
                                    Value = BRBase.BRBaseConvert.ConvertToString(WorkingDataTable.Tables[0].Rows[j]["DEFAULTVALUES"].ToString().Trim());
                                    switch (FieldNm.ToString().ToUpper().Trim())
                                    {
                                        case "RECORDTYPE":
                                            arryRow[t] = Value;
                                            t = t + 1;
                                            break;
                                        case "CURRENCYCODE":
                                            switch (Currency.ToString().ToUpper())
                                            {
                                                case "UGX":
                                                    Curr = "0";
                                                    break;
                                                case "USD":
                                                    Curr = "1";
                                                    break;
                                                case "GBP":
                                                    //Curr = "24";
                                                    break;
                                                case "EUR":
                                                    //Curr = "23";
                                                    break;
                                                case "KES":
                                                    Curr = "4";
                                                    break;
                                            }
                                            arryRow[t] = Curr;
                                            t = t + 1;
                                            break;
                                        case "POSITIONOFAMOUNT":
                                            arryRow[t] = 0;
                                            t = t + 1;
                                            break;
                                        case "PSERIALNUMBER":
                                            BRBase.BRDataSet dsUniqueClearingID = new BRBase.BRDataSet();
                                            string UniqueID = string.Empty;
                                            ClearingUniversalMethod(usrInfo, "p_GetUniqueClearingID", out dsUniqueClearingID, BRBase.BRModule.InwardFileImportation, conn, new object[] { "BrDataSet" }, new object[] { FileFormatValue });
                                            if (dsUniqueClearingID.Tables.Count > 0)
                                            {
                                                if (dsUniqueClearingID.Tables[0].Rows.Count > 0)
                                                {
                                                    UniqueID = dsUniqueClearingID.Tables[0].Rows[0]["UniqueID"].ToString();
                                                }
                                            }
                                            else
                                            {
                                                UniqueID = RBank + dd + mm;
                                            }
                                            UniqueID = GetNextInt16().ToString();
                                            arryRow[t] = new string('0', FileFormatValue - UniqueID.Length) + UniqueID;
                                            t = t + 1;
                                            break;
                                        case "FILLER":
                                            Filler = new string(BRBase.BRBaseConvert.ConvertToChar(Filler), FileFormatValue);
                                            arryRow[t] = Filler;
                                            t = t + 1;
                                            break;
                                        case "VALUE":
                                            NewValue = "0";//BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumCheques)));
                                            NewValue = new string('0', FileFormatValue - NewValue.Length) + NewValue;
                                            arryRow[t] = NewValue;
                                            t = t + 1;
                                            break;
                                        case "RCLEARINGCENTRECODE":
                                            arryRow[t] = RBank;
                                            t = t + 1;
                                            break;
                                        case "PCLEARINGCENTRECODE":
                                            arryRow[t] = usrInfo.strBank;
                                            t = t + 1;
                                            break;
                                        case "DRN":
                                            dsUniqueClearingID = new BRBase.BRDataSet();

                                            UniqueID = string.Empty;
                                            ClearingUniversalMethod(usrInfo, "p_GetUniqueClearingREFID", out dsUniqueClearingID, BRBase.BRModule.InwardFileImportation, conn, new object[] { "BrDataSet" }, new object[] {});
                                            if (dsUniqueClearingID.Tables.Count > 0)
                                            {
                                                if (dsUniqueClearingID.Tables[0].Rows.Count > 0)
                                                {
                                                    UniqueID = dsUniqueClearingID.Tables[0].Rows[0]["UniqueID"].ToString();
                                                }
                                            }
                                            else
                                            {
                                                UniqueID = RBank + dd + mm;
                                            }
                                            dsUniqueClearingID.Tables[0].Clear();
                                            UniqueID = GetNextInt16().ToString();
                                            arryRow[t] = new string('0', FileFormatValue - UniqueID.Length) + UniqueID;
                                            t = t + 1;
                                            break;
                                        case "VOUCHERCODE":
                                            arryRow[t] = "70";
                                            t = t + 1;
                                            break;
                                        case "DATE":
                                            arryRow[t] = ddmmmyyyy;
                                            t = t + 1;
                                            break;
                                    }
                                }
                                break;
                            case "EFT":
                                for (j = 0; j < WorkingDataTable.Tables[0].Rows.Count; j++)
                                {
                                    if (conn.State != ConnectionState.Open)
                                    {
                                        conn = GetConnection();
                                    }
                                    string FieldNm = string.Empty;
                                    FieldNm = WorkingDataTable.Tables[0].Rows[j][Col.ColumnName].ToString();
                                    Int32 FileFormatValue = BRBase.BRBaseConvert.ConvertToInt32(WorkingDataTable.Tables[0].Rows[j]["Length"].ToString());
                                    bool FileFormatValueMandatoryLength = WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString() == "" ? false : BRBase.BRBaseConvert.ConvertToBoolean(WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString());
                                    string Filler = WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString() == "" ? "0" : WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString();
                                    Value = BRBase.BRBaseConvert.ConvertToString(WorkingDataTable.Tables[0].Rows[j]["DEFAULTVALUES"].ToString().Trim());
                                    switch (FieldNm.ToString().ToUpper().Trim())
                                    {
                                        case "RECORDTYPE":
                                        case "BANK":
                                        case "PORGANIZATION":
                                        case "LASTFILEINDICATOR":
                                            arryRow[t] = Value;
                                            t = t + 1;
                                            break;
                                        case "PSERIALNUMBER":
                                            BRBase.BRDataSet dsUniqueClearingID = new BRBase.BRDataSet();
                                            string UniqueID = string.Empty;
                                            ClearingUniversalMethod(usrInfo, "p_GetUniqueClearingID", out dsUniqueClearingID, BRBase.BRModule.InwardFileImportation, conn, new object[] { "BrDataSet" }, new object[] { FileFormatValue });
                                            if (dsUniqueClearingID.Tables.Count > 0)
                                            {
                                                if (dsUniqueClearingID.Tables[0].Rows.Count > 0)
                                                {
                                                    UniqueID = dsUniqueClearingID.Tables[0].Rows[0]["UniqueID"].ToString();
                                                }
                                            }
                                            else
                                            {
                                                UniqueID = RBank + dd + mm;
                                            }
                                            arryRow[t] = UniqueID;
                                            t = t + 1;
                                            break;
                                        case "FILLER":
                                            Filler = new string(BRBase.BRBaseConvert.ConvertToChar(Filler), FileFormatValue);
                                            arryRow[t] = Filler;
                                            t = t + 1;
                                            break;
                                        case "CLEARINGCENTRE":
                                            arryRow[t] = usrInfo.strBank;
                                            t = t + 1;
                                            break;
                                        case "DATE":
                                            arryRow[t] = ddmmmyyyy;
                                            t = t + 1;
                                            break;
                                        case "FILETYPE":
                                            arryRow[t] = "70";
                                            t = t + 1;
                                            break;
                                    }
                                }
                                break;
                            case "DISC":
                                for (j = 0; j < WorkingDataTable.Tables[0].Rows.Count; j++)
                                {
                                    if (conn.State != ConnectionState.Open)
                                    {
                                        conn = GetConnection();
                                    }
                                    string FieldNm = string.Empty;
                                    FieldNm = WorkingDataTable.Tables[0].Rows[j][Col.ColumnName].ToString();
                                    Int32 FileFormatValue = BRBase.BRBaseConvert.ConvertToInt32(WorkingDataTable.Tables[0].Rows[j]["Length"].ToString());
                                    bool FileFormatValueMandatoryLength = WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString() == "" ? false : BRBase.BRBaseConvert.ConvertToBoolean(WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString());
                                    string Filler = WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString() == "" ? "0" : WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString();
                                    Value = BRBase.BRBaseConvert.ConvertToString(WorkingDataTable.Tables[0].Rows[j]["DEFAULTVALUES"].ToString().Trim());
                                    switch (FieldNm.ToString().ToUpper().Trim())
                                    {
                                        case "RECORDTYPE":
                                        case "FILETYPE":
                                        case "PBANK":
                                        case "PORGANIZATION":
                                        case "FILEINDICATOR":
                                            arryRow[t] = Value;
                                            t = t + 1;
                                            break;
                                        case "PSERIALNUMBER":
                                            BRBase.BRDataSet dsUniqueClearingID = new BRBase.BRDataSet();
                                            string UniqueID = string.Empty;
                                            ClearingUniversalMethod(usrInfo, "p_GetUniqueClearingID", out dsUniqueClearingID, BRBase.BRModule.InwardFileImportation, conn, new object[] { "BrDataSet" }, new object[] { FileFormatValue });
                                            if (dsUniqueClearingID.Tables.Count > 0)
                                            {
                                                if (dsUniqueClearingID.Tables[0].Rows.Count > 0)
                                                {
                                                    UniqueID = dsUniqueClearingID.Tables[0].Rows[0]["UniqueID"].ToString();
                                                }
                                            }
                                            else
                                            {
                                                UniqueID = RBank + dd + mm;
                                            }
                                            dsUniqueClearingID.Tables[0].Clear();
                                            arryRow[t] = UniqueID;
                                            t = t + 1;
                                            break;
                                        case "DATE":
                                            arryRow[t] = ddmmmyyyy;
                                            t = t + 1;
                                            break;
                                        case "FILLER":
                                            Filler = new string(BRBase.BRBaseConvert.ConvertToChar(Filler), FileFormatValue);
                                            arryRow[t] = Filler;
                                            t = t + 1;
                                            break;
                                        case "PCLEARINGCENTRECODE":
                                            arryRow[t] = usrInfo.strBank;
                                            t = t + 1;
                                            break;
                                    }
                                }
                                break;
                            case "SELT":
                                for (j = 0; j < WorkingDataTable.Tables[0].Rows.Count; j++)
                                {
                                    if (conn.State != ConnectionState.Open)
                                    {
                                        conn = GetConnection();
                                    }
                                    string FieldNm = string.Empty;
                                    FieldNm = WorkingDataTable.Tables[0].Rows[j][Col.ColumnName].ToString();
                                    Int32 FileFormatValue = BRBase.BRBaseConvert.ConvertToInt32(WorkingDataTable.Tables[0].Rows[j]["Length"].ToString());
                                    bool FileFormatValueMandatoryLength = WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString() == "" ? false : BRBase.BRBaseConvert.ConvertToBoolean(WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString());
                                    string Filler = WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString() == "" ? "0" : WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString();
                                    Value = BRBase.BRBaseConvert.ConvertToString(WorkingDataTable.Tables[0].Rows[j]["DEFAULTVALUES"].ToString().Trim());
                                    switch (FieldNm.ToString().ToUpper().Trim())
                                    {
                                        case "RECORDTYPE":
                                        case "CURRENCYCODE":
                                        case "FILETYPE":
                                        case "CLEARINGCENTRE":
                                        case "ORGANISATION":
                                            arryRow[t] = Value;
                                            t = t + 1;
                                            break;
                                        case "PSERIALNUMBER":
                                            BRBase.BRDataSet dsUniqueClearingID = new BRBase.BRDataSet();
                                            string UniqueID = string.Empty;
                                            ClearingUniversalMethod(usrInfo, "p_GetUniqueClearingID", out dsUniqueClearingID, BRBase.BRModule.InwardFileImportation, conn, new object[] { "BrDataSet" }, new object[] { FileFormatValue });
                                            if (dsUniqueClearingID.Tables.Count > 0)
                                            {
                                                if (dsUniqueClearingID.Tables[0].Rows.Count > 0)
                                                {
                                                    UniqueID = dsUniqueClearingID.Tables[0].Rows[0]["UniqueID"].ToString();
                                                }
                                            }
                                            else
                                            {
                                                UniqueID = RBank + dd + mm;
                                            }
                                            dsUniqueClearingID.Tables[0].Clear();
                                            arryRow[t] = UniqueID;
                                            t = t + 1;
                                            break;
                                        case "FILLER":
                                        case "FILEINDICATOR":
                                            Filler = new string(BRBase.BRBaseConvert.ConvertToChar(Filler), FileFormatValue);
                                            arryRow[t] = Filler;
                                            t = t + 1;
                                            break;
                                        case "BANK":
                                            arryRow[t] = usrInfo.strBank;
                                            t = t + 1;
                                            break;
                                        case "DATE":
                                            arryRow[t] = ddmmmyyyy;
                                            t = t + 1;
                                            break;
                                    }
                                }
                                break;
                        }
                        break;
                }
            }
            Data = string.Empty;
            for (m = 0; m < arryRow.Length; m++)
            {
                Data = Data + (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
            }
            WorkingDataTable.Tables[0].Rows.Clear();
            return Data;
        }
        static Random rnd = new Random();
        public static List<string> usedStrings = new List<string>();

        public static List<Int16> usedInt16 = new List<Int16>();
        public static Int16 GetNextInt16()
        {
            Int16 theInt16 = GetRandomInt16();
            while (usedInt16.Contains(theInt16))
            {
                theInt16 = GetRandomInt16();
            }
            usedInt16.Add(theInt16);
            return theInt16;
        }

        public static Int16 GetRandomInt16()
        {
            Int16 rndInt16 = 0;
            for (int i = 0; i <= 5; i++)
            {
                if (rnd.Next(0, 2) == 0)
                {
                    rndInt16 += (Int16) Microsoft.VisualBasic.Conversion.Int(rnd.Next(65, 91));
                }
                else
                {
                    rndInt16 += (Int16)Microsoft.VisualBasic.Conversion.Int(rnd.Next(48, 58));
                }
            }
            return rndInt16;
        }

        public static bool GenerateEJs(string VoucherType, DS_trxClearing dstrxClearing, DS_ClearingFileFormat dsClearingFileFormat, string ToBankID, string Currency, BRBase.BRDataSet dsWithImages, DateTime WorkingDate, BRBase.UserInfo usrInfo, out DataTable SemiCompiledDataToBeWritten, String Banks, string clientName, IDbConnection conn, string[] conString)
        {

            string sToBankID = ToBankID;
            if (sToBankID == "MFI")
            {
                ToBankID = usrInfo.strBank;
            }
            Int16 j, m;
            Int32 t;
            string ControlVoucherType = string.Empty;
            string NewValue = string.Empty;
            string Value = string.Empty;
            string FieldNm = string.Empty;
            string ImageID = string.Empty;
            bool Status = false;
            string curr = string.Empty;
            string sortOrder = "Start ASC";
            SemiCompiledDataToBeWritten = new DataTable();
            SemiCompiledDataToBeWritten.Columns.Add("TrxRowID", typeof(string));
            SemiCompiledDataToBeWritten.Columns.Add("text", typeof(string));
            SemiCompiledDataToBeWritten.Columns.Add("FileName", typeof(string));
            SemiCompiledDataToBeWritten.Columns.Add("ImageID", typeof(string));
            SemiCompiledDataToBeWritten.Columns.Add("fcy", typeof(string));
            string Data = string.Empty;
            DS_ClearingFileFormat WorkingDataTable = new DS_ClearingFileFormat();
            DS_trxClearing RejectDt = new DS_trxClearing();
            DS_trxClearing EJdataTable = new DS_trxClearing();
            DataTable EJdt = new DataTable();
            Int32 IsFcy = 0;
            ArrayList arr = new ArrayList();
           BRBase.BRDataSet ds4Images = new BRBase.BRDataSet();
            string dd = new string('0', 2 - BRBase.BRBaseConvert.ConvertToString(WorkingDate.Day).ToString().Length) + BRBase.BRBaseConvert.ConvertToString(WorkingDate.Day);
            string mm = new string('0', 2 - BRBase.BRBaseConvert.ConvertToString(WorkingDate.Month).Length) + BRBase.BRBaseConvert.ConvertToString(WorkingDate.Month);
            string yyyy = BRBase.BRBaseConvert.ConvertToString(WorkingDate.Year);
            string ddmmmyyyy = dd + mm + yyyy;
            ds4Images = (BRBase.BRDataSet)dsWithImages.Clone();
            EJdataTable = (DS_trxClearing)dstrxClearing.Clone();/////////////////////////////////////
            DataRow[] drHeaderFileFormatResult = null;
            string CurrType = string.Empty;
            t = 0;
            if (Currency == "KES")
            {
                if (sToBankID == "MFI")
                {
                    CurrType = " AND currencyID ='KES' AND TrxType ='ID'";
                }
                else
                {
                    CurrType = " AND currencyID ='KES' AND TrxType ='OC'";
                }
                curr = "00";
            }
            else
            {
                switch (Currency.ToUpper())
                {
                    case "USD":
                        curr = "60";
                        break;
                    case "GBP":
                        curr = "61";
                        break;
                    case "EUR":
                        curr = "62";
                        break;
                }
                if (sToBankID == "MFI")
                {
                    CurrType = " AND currencyID = '" + Currency + "' AND TrxType ='ID'";
                }
                else
                {
                    CurrType = " AND currencyID = '" + Currency + "' AND TrxType ='OC'";
                }
                IsFcy = 1;
            }
            drHeaderFileFormatResult = dsClearingFileFormat.Tables[0].Select("FileType='EJ' AND RecordType ='EJ'", sortOrder);
            foreach (DataRow dvr in drHeaderFileFormatResult)
            {
                WorkingDataTable.Tables[0].ImportRow(dvr);
            }
            WorkingDataTable.AcceptChanges();
            Object[] arryRow = new Object[WorkingDataTable.Tables[0].Rows.Count];
            switch (VoucherType.ToString().ToUpper())
            {
                case "BCV":  //Presentments 
                    if (sToBankID == "MFI")
                    {
                        drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("ReturnCode  IN ('00','17') AND VOUCHERCODE NOT IN ('03','40') " + CurrType + " AND CollectionAccount IN ('" + clientName.Substring(6) + "')");//
                    }
                    else
                    {
                        drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("ReturnCode  IN ('00','17') AND VOUCHERCODE NOT IN ('03','40') AND ToBank " + Banks + " " + CurrType + "");//
                    }
                    foreach (DataRow dvr in drHeaderFileFormatResult)
                    {
                        EJdataTable.Tables[0].ImportRow(dvr);
                    }
                    EJdataTable.AcceptChanges();
                    break;
                case "UCV":  //Unpaids 

                    if (sToBankID == "MFI")
                    {
                        drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("ReturnCode NOT IN ('00','17') AND VOUCHERCODE NOT IN ('40') " + CurrType + " AND CollectionAccount IN ('" + clientName.Substring(6) + "')");
                    }
                    else
                    {
                        drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("ReturnCode NOT IN ('00','17') AND VOUCHERCODE NOT IN ('40') AND ToBank " + Banks + " " + CurrType + "");
                    }
                    foreach (DataRow dvr in drHeaderFileFormatResult)
                    {
                        EJdataTable.Tables[0].ImportRow(dvr);
                    }
                    EJdataTable.AcceptChanges();
                    break;
                case "MDV":  //Manual Debit Voucher
                    if (sToBankID == "MFI")
                    {
                        drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("ReturnCode IN ('00','17') AND VOUCHERCODE ='03' " + CurrType + " AND CollectionAccount IN ('" + clientName.Substring(6) + "')");
                    }
                    else
                    {
                        drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("ReturnCode IN ('00','17') AND VOUCHERCODE ='03' AND ToBank " + Banks + " " + CurrType + "");
                    }
                    foreach (DataRow dvr in drHeaderFileFormatResult)
                    {
                        EJdataTable.Tables[0].ImportRow(dvr);
                    }
                    EJdataTable.AcceptChanges();
                    break;
            }
            Int32 q = 0;
            foreach (DataRow dr in EJdataTable.Tables[0].Rows)
            {
                if (ConfigurationManager.AppSettings["Rem"] == "1")
                {
                    arryRow = new Object[16]; //With Remitter details;
                }
                else
                {
                    arryRow = new Object[15];
                }
               
                //arryRow = new Object[15];
                t = 0;
                q = q + 1;
                if (q == 99)//This is meant to take care of the BCV or TCV Batching of a 100
                {
                    break;
                }
                dr["Generated"] = true;
                foreach (DataColumn Col in WorkingDataTable.Tables[0].Columns)
                {
                    switch (Col.ColumnName.ToUpper())
                    {
                        case "FIELDNAME":
                            try
                            {
                                if (sToBankID == "MFI")
                                {
                                    if (conn.State != ConnectionState.Open)
                                    {
                                        conn = GetConnection();
                                    }
                                    BRBase.BRDataSet dsOriginalTrx = new BRBase.BRDataSet();
                                     ClearingUniversalMethod(usrInfo, "p_RetreaveInClearingMFI", out dsOriginalTrx, BRBase.BRModule.GenerateClearingFile, conn, new object[] { "BrDataSet" }, new object[] { dr["TrxRowID"].ToString().Trim() });
                                        if (dsOriginalTrx.Tables.Count > 0)
                                        {
                                            if (dsOriginalTrx.Tables[0].Rows.Count > 0)
                                            {
                                                if (dsOriginalTrx.Tables[0].Columns.Contains("Data"))
                                                {
                                                    if (dsOriginalTrx.Tables[0].Rows[0]["Data"].ToString() != "NIL" && dsOriginalTrx.Tables[0].Rows[0]["Data"].ToString() != "")
                                                    {
                                                        Data = dsOriginalTrx.Tables[0].Rows[0]["Data"].ToString();
                                                    }
                                                }
                                            }
                                        }
                                }
                                else
                                {
                                    for (j = 0; j < WorkingDataTable.Tables[0].Rows.Count; j++)
                                    {
                                        if (conn.State != ConnectionState.Open)
                                        {
                                            conn = GetConnection();
                                        }
                                        //Get the Required Length
                                        FieldNm = string.Empty;
                                        FieldNm = WorkingDataTable.Tables[0].Rows[j][Col.ColumnName].ToString();
                                        Int32 FileFormatValue = BRBase.BRBaseConvert.ConvertToInt32(WorkingDataTable.Tables[0].Rows[j]["Length"].ToString());
                                        bool FileFormatValueMandatoryLength = WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString() == "" ? true : BRBase.BRBaseConvert.ConvertToBoolean(WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString());
                                        string Filler = WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString() == "" ? "0" : WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString();
                                        //Check the datatable, whether the column that we are presently in, its value meets the required length
                                        switch (FieldNm.ToString().ToUpper().Trim())
                                        {
                                            case "RETURNCODE":
                                            case "VOUCHERCODE":
                                            case "VALUE":
                                            case "CHEQUEDIGIT":
                                            case "SERIALNUMBER":
                                                Int32 ValueLength = BRBase.BRBaseConvert.ConvertToInt32((dr[FieldNm].ToString().Trim()).ToString().Length);
                                                Value = BRBase.BRBaseConvert.ConvertToString(dr[FieldNm].ToString().Trim());
                                                if (ValueLength > FileFormatValue)
                                                {
                                                    dr["Status"] = "R";
                                                    EJdataTable.AcceptChanges();
                                                    goto JustRejected;
                                                }
                                                else
                                                {
                                                    Value = BRBase.BRBaseConvert.ConvertToString(dr[FieldNm].ToString().Trim());
                                                    NewValue = Value;
                                                    if (FileFormatValueMandatoryLength == true)
                                                    {
                                                        if (ValueLength != FileFormatValue)
                                                        {
                                                            if (FieldNm == "VALUE")
                                                            {
                                                                Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(Value), BRBase.BRBaseConvert.ConvertToString(dr["VoucherCode"])));
                                                                //Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(Value)));
                                                                if (Value.ToString().Contains("."))
                                                                {
                                                                    Value = Value.Substring(0, Value.IndexOf("."));
                                                                }
                                                            }
                                                            //First Fill in the required Characters.
                                                            NewValue = new string(BRBase.BRBaseConvert.ConvertToChar(Filler), (BRBase.BRBaseConvert.ConvertToInt32(FileFormatValue) - BRBase.BRBaseConvert.ConvertToInt32(Value.Length))) + Value;
                                                            Int32 NewValueLength = NewValue.Length;
                                                            if (NewValueLength != FileFormatValue)
                                                            {//Not all do have fillers, so if the do not meet the requirement, Flag them for reject.
                                                                dr["Status"] = "R";
                                                                EJdataTable.AcceptChanges();
                                                                goto JustRejected;
                                                            }
                                                        }
                                                        arryRow[t] = NewValue;
                                                        t = t + 1;
                                                    }
                                                    else
                                                    {
                                                        arryRow[t] = "";
                                                        t = t + 1;
                                                    }
                                                }
                                                break;
                                            case "TOBANK":
                                                if (VoucherType.ToString().ToUpper() == "UCV")
                                                {
                                                    ValueLength = BRBase.BRBaseConvert.ConvertToInt32((usrInfo.strBank.ToString().Trim()).ToString().Length);
                                                    Value = BRBase.BRBaseConvert.ConvertToString(usrInfo.strBank.ToString().Trim());
                                                }
                                                else
                                                {
                                                    ValueLength = BRBase.BRBaseConvert.ConvertToInt32((dr[FieldNm].ToString().Trim()).ToString().Length);
                                                    Value = BRBase.BRBaseConvert.ConvertToString(dr[FieldNm].ToString().Trim());
                                                }
                                                if (ValueLength > FileFormatValue)
                                                {
                                                    dr["Status"] = "R";
                                                    EJdataTable.AcceptChanges();
                                                    goto JustRejected;
                                                }
                                                else
                                                {
                                                    NewValue = Value;
                                                    if (FileFormatValueMandatoryLength == true)
                                                    {
                                                        if (ValueLength != FileFormatValue)
                                                        {
                                                            if (FieldNm == "VALUE")
                                                            {
                                                                //Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(Value)));
                                                                Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(Value), BRBase.BRBaseConvert.ConvertToString(dr["VoucherCode"])));
                                                                if (Value.ToString().Contains("."))
                                                                {
                                                                    Value = Value.Substring(0, Value.IndexOf("."));
                                                                }
                                                            }
                                                            //First Fill in the required Characters.
                                                            NewValue = new string(BRBase.BRBaseConvert.ConvertToChar(Filler), (BRBase.BRBaseConvert.ConvertToInt32(FileFormatValue) - BRBase.BRBaseConvert.ConvertToInt32(Value.Length))) + Value;
                                                            Int32 NewValueLength = NewValue.Length;
                                                            if (NewValueLength != FileFormatValue)
                                                            {//Not all do have fillers, so if the do not meet the requirement, Flag them for reject.
                                                                dr["Status"] = "R";
                                                                EJdataTable.AcceptChanges();
                                                                goto JustRejected;
                                                            }
                                                        }
                                                        arryRow[t] = NewValue;
                                                        t = t + 1;
                                                    }
                                                    else
                                                    {
                                                        arryRow[t] = "";
                                                        t = t + 1;
                                                    }
                                                }
                                                break;
                                            case "TOBRANCH":
                                                if (VoucherType.ToString().ToUpper() == "UCV")
                                                {
                                                    ValueLength = BRBase.BRBaseConvert.ConvertToInt32((usrInfo.strBranch.ToString().Trim()).ToString().Length);
                                                    Value = BRBase.BRBaseConvert.ConvertToString(usrInfo.strBranch.ToString().Trim());
                                                }
                                                else
                                                {
                                                    ValueLength = BRBase.BRBaseConvert.ConvertToInt32((dr[FieldNm].ToString().Trim()).ToString().Length);
                                                    Value = BRBase.BRBaseConvert.ConvertToString(dr[FieldNm].ToString().Trim());
                                                }
                                                if (ValueLength > FileFormatValue)
                                                {
                                                    dr["Status"] = "R";
                                                    EJdataTable.AcceptChanges();
                                                    goto JustRejected;
                                                }
                                                else
                                                {
                                                    //Value = BRBase.BRBaseConvert.ConvertToString(dr[FieldNm].ToString().Trim());
                                                    NewValue = Value;
                                                    if (FileFormatValueMandatoryLength == true)
                                                    {
                                                        if (ValueLength != FileFormatValue)
                                                        {
                                                            if (FieldNm == "VALUE")
                                                            {
                                                                //Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(Value)));
                                                                Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(Value), BRBase.BRBaseConvert.ConvertToString(dr["VoucherCode"])));
                                                                if (Value.ToString().Contains("."))
                                                                {
                                                                    Value = Value.Substring(0, Value.IndexOf("."));
                                                                }
                                                            }
                                                            //First Fill in the required Characters.
                                                            NewValue = new string(BRBase.BRBaseConvert.ConvertToChar(Filler), (BRBase.BRBaseConvert.ConvertToInt32(FileFormatValue) - BRBase.BRBaseConvert.ConvertToInt32(Value.Length))) + Value;
                                                            Int32 NewValueLength = NewValue.Length;
                                                            if (NewValueLength != FileFormatValue)
                                                            {//Not all do have fillers, so if the do not meet the requirement, Flag them for reject.
                                                                dr["Status"] = "R";
                                                                EJdataTable.AcceptChanges();
                                                                goto JustRejected;
                                                            }
                                                        }
                                                        arryRow[t] = NewValue;
                                                        t = t + 1;
                                                    }
                                                    else
                                                    {
                                                        arryRow[t] = "";
                                                        t = t + 1;
                                                    }
                                                }
                                                break;
                                            case "TOACCOUNT":
                                                if (VoucherType.ToString().ToUpper() == "UCV")
                                                {
                                                    ValueLength = BRBase.BRBaseConvert.ConvertToInt32((dr["COLLECTIONACCOUNT"].ToString().Trim()).ToString().Length);
                                                    Value = BRBase.BRBaseConvert.ConvertToString(dr["COLLECTIONACCOUNT"].ToString().Trim());
                                                }
                                                else
                                                {
                                                    ValueLength = BRBase.BRBaseConvert.ConvertToInt32((dr[FieldNm].ToString().Trim()).ToString().Length);
                                                    Value = BRBase.BRBaseConvert.ConvertToString(dr[FieldNm].ToString().Trim());
                                                }
                                                if (ValueLength > 10)
                                                {
                                                    dr["Status"] = "R";
                                                    EJdataTable.AcceptChanges();
                                                    goto JustRejected;
                                                }
                                                else
                                                {
                                                    //Value = BRBase.BRBaseConvert.ConvertToString(dr[FieldNm].ToString().TrimStart('0'));
                                                    NewValue = Value;
                                                    if (FileFormatValueMandatoryLength == true)
                                                    {
                                                        if (ValueLength != 10)
                                                        {
                                                            if (FieldNm == "VALUE")
                                                            {
                                                                //Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(Value)));
                                                                Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(Value), BRBase.BRBaseConvert.ConvertToString(dr["VoucherCode"])));
                                                                if (Value.ToString().Contains("."))
                                                                {
                                                                    Value = Value.Substring(0, Value.IndexOf("."));
                                                                }
                                                            }
                                                            //First Fill in the required Characters.
                                                            NewValue = new string(BRBase.BRBaseConvert.ConvertToChar(Filler), (BRBase.BRBaseConvert.ConvertToInt32(10) - BRBase.BRBaseConvert.ConvertToInt32(Value.Length))) + Value;
                                                            Int32 NewValueLength = NewValue.Length;
                                                            if (NewValueLength != 10)
                                                            {//Not all do have fillers, so if the do not meet the requirement, Flag them for reject.
                                                                dr["Status"] = "R";
                                                                EJdataTable.AcceptChanges();
                                                                goto JustRejected;
                                                            }
                                                        }
                                                        arryRow[t] = NewValue;
                                                        t = t + 1;
                                                    }
                                                    else
                                                    {
                                                        arryRow[t] = "";
                                                        t = t + 1;
                                                    }
                                                }
                                                break;
                                            case "COLLECTIONACCOUNT":
                                                ValueLength = BRBase.BRBaseConvert.ConvertToInt32((dr[FieldNm].ToString().Trim()).ToString().Length);
                                                Value = BRBase.BRBaseConvert.ConvertToString(dr[FieldNm].ToString().Trim());
                                                if (ValueLength > FileFormatValue)
                                                {
                                                    dr["Status"] = "R";
                                                    EJdataTable.AcceptChanges();
                                                    goto JustRejected;
                                                }
                                                else
                                                {
                                                    Value = new string(BRBase.BRBaseConvert.ConvertToChar(Filler), FileFormatValue - ValueLength) + BRBase.BRBaseConvert.ConvertToString(dr[FieldNm].ToString().Trim());
                                                    NewValue = Value;
                                                }
                                                arryRow[t] = Value;
                                                t = t + 1;
                                                break;
                                            case "BENEFICIARYNAME":
                                            //case "PAYERSNAME":
                                            //case "DRAWERORPAYEE":
                                                if (sToBankID != "MFI")
                                                {
                                                    ValueLength = BRBase.BRBaseConvert.ConvertToInt32((dr[FieldNm].ToString().Trim()).ToString().Length);
                                                    Value = BRBase.BRBaseConvert.ConvertToString(dr[FieldNm].ToString().Trim());
                                                    if (ValueLength > FileFormatValue)
                                                    {
                                                        dr["Status"] = "R";
                                                        EJdataTable.AcceptChanges();
                                                        goto JustRejected;
                                                    }
                                                    else
                                                    {
                                                        Filler = " ";
                                                        Value = BRBase.BRBaseConvert.ConvertToString(Value.ToString().Trim()) + new string(BRBase.BRBaseConvert.ConvertToChar(Filler), FileFormatValue - ValueLength);
                                                        NewValue = Value;
                                                    }
                                                    arryRow[t] = Value;
                                                }
                                                t = t + 1;
                                                break;
                                            case "CURRENCYCODE":
                                                Value = curr;
                                                if (Value == "")
                                                {
                                                    Value = BRBase.BRBaseConvert.ConvertToString(WorkingDataTable.Tables[0].Rows[j]["DEFAULTVALUES"].ToString().Trim());
                                                }
                                                arryRow[t] = Value;
                                                t = t + 1;
                                                break;
                                            case "FILLER":
                                                Value = BRBase.BRBaseConvert.ConvertToString(WorkingDataTable.Tables[0].Rows[j]["DEFAULTVALUES"].ToString().Trim());
                                                arryRow[t] = Value;
                                                t = t + 1;
                                                break;
                                            case "ENTRYMODE":
                                                Value = "2"; //BRBaseConvert.ConvertToString(WorkingDataTable.Tables[0].Rows[j]["DEFAULTVALUES"].ToString().Trim());
                                                arryRow[t] = Value;
                                                t = t + 1;
                                                break;
                                            case "FROMBANK":
                                                if (VoucherType.ToString().ToUpper() != "UCV")
                                                {
                                                    ValueLength = BRBase.BRBaseConvert.ConvertToInt32((usrInfo.strBank.ToString().Trim()).ToString().Length);
                                                    Value = BRBase.BRBaseConvert.ConvertToString(usrInfo.strBank.ToString().Trim());
                                                }
                                                else
                                                {
                                                    ValueLength = BRBase.BRBaseConvert.ConvertToInt32((dr["TOBANK"].ToString().Trim()).ToString().Length);
                                                    Value = BRBase.BRBaseConvert.ConvertToString(dr["TOBANK"].ToString().Trim());
                                                }
                                                if (ValueLength > FileFormatValue)
                                                {
                                                    dr["Status"] = "R";
                                                    EJdataTable.AcceptChanges();
                                                    goto JustRejected;
                                                }
                                                else
                                                {
                                                    //Value = BRBase.BRBaseConvert.ConvertToString(dr[FieldNm].ToString().Trim());
                                                    NewValue = Value;
                                                    if (FileFormatValueMandatoryLength == true)
                                                    {
                                                        if (ValueLength != FileFormatValue)
                                                        {
                                                            if (FieldNm == "VALUE")
                                                            {
                                                                //Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(Value)));
                                                                Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(Value), BRBase.BRBaseConvert.ConvertToString(dr["VoucherCode"])));
                                                                if (Value.ToString().Contains("."))
                                                                {
                                                                    Value = Value.Substring(0, Value.IndexOf("."));
                                                                }
                                                            }
                                                            //First Fill in the required Characters.
                                                            NewValue = new string(BRBase.BRBaseConvert.ConvertToChar(Filler), (BRBase.BRBaseConvert.ConvertToInt32(FileFormatValue) - BRBase.BRBaseConvert.ConvertToInt32(Value.Length))) + Value;
                                                            Int32 NewValueLength = NewValue.Length;
                                                            if (NewValueLength != FileFormatValue)
                                                            {//Not all do have fillers, so if the do not meet the requirement, Flag them for reject.
                                                                dr["Status"] = "R";
                                                                EJdataTable.AcceptChanges();
                                                                goto JustRejected;
                                                            }
                                                        }
                                                        arryRow[t] = NewValue;
                                                        t = t + 1;
                                                    }
                                                    else
                                                    {
                                                        arryRow[t] = "";
                                                        t = t + 1;
                                                    }
                                                }
                                                break;
                                            case "FROMBRANCH":
                                                if (VoucherType.ToString().ToUpper() != "UCV")
                                                {
                                                    ValueLength = BRBase.BRBaseConvert.ConvertToInt32((dr["FROMBRANCH"].ToString().Trim()).ToString().Length);
                                                    Value = BRBase.BRBaseConvert.ConvertToString(dr["FROMBRANCH"].ToString().Trim());
                                                }
                                                else
                                                {
                                                    ValueLength = BRBase.BRBaseConvert.ConvertToInt32((dr["TOBRANCH"].ToString().Trim()).ToString().Length);
                                                    Value = BRBase.BRBaseConvert.ConvertToString(dr["TOBRANCH"].ToString().Trim());
                                                }
                                                if (ValueLength > FileFormatValue)
                                                {
                                                    dr["Status"] = "R";
                                                    EJdataTable.AcceptChanges();
                                                    goto JustRejected;
                                                }
                                                else
                                                {
                                                    //Value = BRBase.BRBaseConvert.ConvertToString(dr[FieldNm].ToString().Trim());
                                                    NewValue = Value;
                                                    if (FileFormatValueMandatoryLength == true)
                                                    {
                                                        if (ValueLength != FileFormatValue)
                                                        {
                                                            if (FieldNm == "VALUE")
                                                            {
                                                                //Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(Value)));
                                                                Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(Value), BRBase.BRBaseConvert.ConvertToString(dr["VoucherCode"])));
                                                                if (Value.ToString().Contains("."))
                                                                {
                                                                    Value = Value.Substring(0, Value.IndexOf("."));
                                                                }
                                                            }
                                                            //First Fill in the required Characters.
                                                            NewValue = new string(BRBase.BRBaseConvert.ConvertToChar(Filler), (BRBase.BRBaseConvert.ConvertToInt32(FileFormatValue) - BRBase.BRBaseConvert.ConvertToInt32(Value.Length))) + Value;
                                                            Int32 NewValueLength = NewValue.Length;
                                                            if (NewValueLength != FileFormatValue)
                                                            {//Not all do have fillers, so if the do not meet the requirement, Flag them for reject.
                                                                dr["Status"] = "R";
                                                                EJdataTable.AcceptChanges();
                                                                goto JustRejected;
                                                            }
                                                        }
                                                        arryRow[t] = NewValue;
                                                        t = t + 1;
                                                    }
                                                    else
                                                    {
                                                        arryRow[t] = "";
                                                        t = t + 1;
                                                    }
                                                }
                                                break;
                                            case "DRN":
                                                ImageID = "0";
                                                ValueLength = BRBase.BRBaseConvert.ConvertToInt32((dr["TrxRowID"].ToString().Trim()).ToString().Length);
                                                if (ValueLength != FileFormatValue)
                                                {
                                                    NewValue = dr["TrxRowID"].ToString().Trim();
                                                    ImageID = NewValue;
                                                    ImageID = ImageID.TrimStart('0');
                                                    //NewValue = usrInfo.strBank + NewValue + curr + ddmmmyyyy;
                                                    if (BRBase.BRBaseConvert.ConvertToInt32(NewValue.ToString().Length)>BRBase.BRBaseConvert.ConvertToInt32(FileFormatValue))
                                                    {
                                                        NewValue = NewValue.Substring(0, BRBase.BRBaseConvert.ConvertToInt32(FileFormatValue));
                                                    }
                                                    NewValue = new string(BRBase.BRBaseConvert.ConvertToChar(' '), (BRBase.BRBaseConvert.ConvertToInt32(FileFormatValue) - BRBase.BRBaseConvert.ConvertToInt32(NewValue.ToString().Length))) + NewValue;
                                                }
                                                arryRow[t] = NewValue;
                                                t = t + 1;
                                                break;
                                            case "FRONTIMAGESIZE1":
                                                //if (dsWithImages.Tables.Count > 0)
                                                //{
                                                //    arryRow[t] = ds4Images.Tables[0].Rows[0]["TFImageSize"];
                                                //    t = t + 1;
                                                //}
                                                //else
                                                //{
                                                //    arryRow[t] = "";
                                                //    t = t + 1;
                                                //}
                                                break;
                                            case "FRONTIMAGESIZESIGNATURE1":
                                                //if (dsWithImages.Tables.Count > 0)
                                                //{
                                                //    arryRow[t] = ds4Images.Tables[0].Rows[0]["TFImageSignature"];
                                                //    t = t + 1;
                                                //}
                                                //else
                                                //{
                                                //    arryRow[t] = "";
                                                //    t = t + 1;
                                                //}
                                                break;
                                            case "FRONTIMAGESIZE2":
                                                //if (dsWithImages.Tables.Count > 0)
                                                //{
                                                //    arryRow[t] = ds4Images.Tables[0].Rows[0]["JFImageSize"];
                                                //    t = t + 1;
                                                //}
                                                //else
                                                //{
                                                //    arryRow[t] = "";
                                                //    t = t + 1;
                                                //}
                                                break;
                                            case "FRONTIMAGESIZESIGNATURE2":
                                                //if (dsWithImages.Tables.Count > 0)
                                                //{
                                                //    arryRow[t] = ds4Images.Tables[0].Rows[0]["JFImageSignature"];
                                                //    t = t + 1;
                                                //}
                                                //else
                                                //{
                                                //    arryRow[t] = "";
                                                //    t = t + 1;
                                                //}
                                                break;
                                            case "BACKIMAGESIZE":
                                                //if (dsWithImages.Tables.Count > 0)
                                                //{
                                                //    arryRow[t] = ds4Images.Tables[0].Rows[0]["JRImageSize"];
                                                //    t = t + 1;
                                                //}
                                                //else
                                                //{
                                                //    arryRow[t] = "";
                                                //    t = t + 1;
                                                //}
                                                break;
                                            case "BACKIMAGESIZESIGNATURE":
                                                //if (dsWithImages.Tables.Count > 0)
                                                //{
                                                //    arryRow[t] = ds4Images.Tables[0].Rows[0]["JRImageSignature"];
                                                //    t = t + 1;
                                                //}
                                                //else
                                                //{
                                                //    arryRow[t] = "";
                                                //    t = t + 1;
                                                //}
                                                break;
                                            case "FRONTIMAGE1":
                                                //if (dsWithImages.Tables.Count > 0)
                                                //{
                                                //    arryRow[t] = ds4Images.Tables[0].Rows[0]["TFImage"];
                                                //    t = t + 1;
                                                //}
                                                //else
                                                //{
                                                //    arryRow[t] = "";
                                                //    t = t + 1;
                                                //}
                                                break;
                                            case "FRONTIMAGE2":
                                                //if (dsWithImages.Tables.Count > 0)
                                                //{
                                                //    arryRow[t] = ds4Images.Tables[0].Rows[0]["JFImage"];
                                                //    t = t + 1;
                                                //}
                                                //else
                                                //{
                                                //    arryRow[t] = "";
                                                //    t = t + 1;
                                                //}
                                                break;
                                            case "BACKIMAGE":
                                                //if (dsWithImages.Tables.Count > 0)
                                                //{
                                                //    arryRow[t] = ds4Images.Tables[0].Rows[0]["JRImage"];
                                                //    t = t + 1;
                                                //}
                                                //else
                                                //{
                                                //    arryRow[t] = "";
                                                //    t = t + 1;
                                                //}
                                                break;
                                        }
                                    }
                                    Data = string.Empty;

                                    for (m = 0; m < arryRow.Length; m++)
                                    {
                                        Data = Data + (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
                                    }
                                }

                                if (sToBankID == "MFI")
                                {
                                    ImageID = dr["TrxRowID"].ToString().Trim();

                                }
                                else
                                {
                                    //For Unpaid Trx, retreave the origianl Trx
                                    if (arryRow[0].ToString() != "00")
                                    {
                                        if (arryRow[0].ToString() != "17")
                                        {
                                            BRBase.BRDataSet dsOriginalUnpaidTrx = new BRBase.BRDataSet();
                                            //First I retrieve what came in, so that i just change the returncode
                                            ClearingUniversalMethod(usrInfo, "p_GetTheOriginalUnpaidTrx", out dsOriginalUnpaidTrx, BRBase.BRModule.GenerateClearingFile, conn, new object[] { "BrDataSet" }, new object[] { dr["TrxRowID"].ToString().Trim() });
                                            if (dsOriginalUnpaidTrx.Tables.Count > 0)
                                            {
                                                if (dsOriginalUnpaidTrx.Tables[0].Rows.Count > 0)
                                                {
                                                    if (dsOriginalUnpaidTrx.Tables[0].Columns.Contains("Data"))
                                                    {
                                                        if (dsOriginalUnpaidTrx.Tables[0].Rows[0]["Data"].ToString() != "NIL" && dsOriginalUnpaidTrx.Tables[0].Rows[0]["Data"].ToString() != "")
                                                        {
                                                            Data = dsOriginalUnpaidTrx.Tables[0].Rows[0]["Data"].ToString();
                                                            //System.Windows.Forms.MessageBox.Show(Data + StrTrxRowID);
                                                            Data = Data.Substring(2);
                                                            Data = arryRow[0].ToString() + Data;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                SemiCompiledDataToBeWritten.Rows.Add(dr["TrxRowID"].ToString().Trim(), Data, "", ImageID, IsFcy);
                                Status = true;
                            }
                            catch (Exception ex)
                            {
                                ex.ToString();
                                Status = false;
                            }
                        JustRejected:
                            break;
                        default: break;
                    }
                }
            }
            WorkingDataTable.Tables[0].Rows.Clear();
            return Status;
        }

        public static DataTable GenerateSettlementFile(string CurrencyID,BRBase.BRDataSet dsClearingBanks, string WorkingDate, string Type, DS_trxClearing dstrxClearing, DS_ClearingFileFormat dsClearingFileFormat, BRBase.UserInfo usrInfo,DateTime WDate, DateTime FDate,IDbConnection conn)
        {
            Int32 t, m;
            string Value = string.Empty;
            string Data = string.Empty;
            t = 0;
            string ToBank = string.Empty;
            double SumCheques = 0;
            string SumChq = string.Empty;
            double SumCredits = 0;
            string SumCr = string.Empty;
            double SumDebits = 0;
            string SumDr = string.Empty;
            string UnpaidValue = "0";
            double SumUnpaidCheques = 0;
            string SumUnChq = string.Empty;
            double SumunpaidCredits = 0;
            string SumUnCr = string.Empty;
            double SumUnpaidDebits = 0;
            string SumUnDr = string.Empty;
            double SumDiscCheques = 0;
            string MDVValue  = "0";
            string TotalCountMDV = "0";
            double SumunpaidMDV = 0;
            double SumMDV = 0;
            string TotalUnpaidCountMDV  = "0";
            string MDVUnpaidValue = "0";
            string SumDiscChq = string.Empty;
            string TotalCountCheques = "0";
            string TotalCountDebits = "0";
            string TotalCountCredits = "0";
            string TotalUnpaidCountCheques = "0";
            string TotalUnpaidCountDebits = "0";
            string TotalUnpaidCountCredits = "0";
            string TotalDiscCountCheques = "0";
            string RecordType = string.Empty;
            DS_trxClearing EJdataTable = new DS_trxClearing();
            ArrayList arr = new ArrayList();
            DataRow[] drHeaderFileFormatResult = null;
            Object[] arryRow = new Object[10];
            DataTable dt = new DataTable();
            DataSet SettlementListingDt = new DataSet();
            string Curr = string.Empty;
            dt.Columns.Add("Data", typeof(string));
            arr.Add("HEADER");
            arr.Add("06");
            arr.Add("TRAILER");
            if (CurrencyID.ToString() == "KES")
            {
                Curr = "00";
            }
            else
            {
                Curr = "60";
            }

            if (SettlementListingDt.Tables.Contains("t_SettlementListing") == false)
            {
                SettlementListingDt.Tables.Add("dt_SettlementListing");
                //SettlementListingDt.Tables[0].TableName = "t_SettlementListing";
                SettlementListingDt.Tables[0].Columns.Add("BankID",typeof(string));
                SettlementListingDt.Tables[0].Columns.Add("CRCount",typeof(Int32));
                SettlementListingDt.Tables[0].Columns.Add("CRAmount",typeof(double));
                SettlementListingDt.Tables[0].Columns.Add("DRCount",typeof(Int32));
                SettlementListingDt.Tables[0].Columns.Add("DRAmount",typeof(double));
                SettlementListingDt.Tables[0].Columns.Add("MDVCount",typeof(Int32));
                SettlementListingDt.Tables[0].Columns.Add("MDVAmount",typeof(double));
                SettlementListingDt.Tables[0].Columns.Add("DRUnpaidCount",typeof(Int32));
                SettlementListingDt.Tables[0].Columns.Add("DRUnpaidAmount",typeof(double));
                SettlementListingDt.Tables[0].Columns.Add("DiscCount",typeof(Int32));
                SettlementListingDt.Tables[0].Columns.Add("DiscAmount",typeof(double));
                SettlementListingDt.Tables[0].Columns.Add("CRUnpaidCount", typeof(Int32));
                SettlementListingDt.Tables[0].Columns.Add("CRUnpaidAmount",typeof(double));
                SettlementListingDt.Tables[0].Columns.Add("Data",typeof(string));
                SettlementListingDt.Tables[0].Columns.Add("Date",typeof(DateTime));
                SettlementListingDt.Tables[0].Columns.Add("CurrencyID",typeof(string));
                SettlementListingDt.Tables[0].Columns.Add("ClearingType",typeof(string));
                SettlementListingDt.Tables[0].Columns.Add("FileDate",typeof(DateTime));

            }

            try
            {
                for (Int32 p = 0; p < arr.Count; p++)
                {
                    switch (arr[p].ToString().ToUpper())
                    {
                        case "HEADER":
                            //                                                              Proc No.    
                            Data = "180" + Curr + WorkingDate + "00" + usrInfo.strBank + "0000" + "00000000" + "0" + new string('0', 48);
                            dt.Rows.Add(Data);
                            break;
                        case "TRAILER":
                            Data = "1900" + usrInfo.strBank + "0000" + new string('0', 68);
                            dt.Rows.Add(Data);
                            break;
                        case "06":
                            if (CurrencyID.ToString() != "KES")
                            {
                                for (Int32 n = 0; n < dsClearingBanks.Tables[0].Rows.Count; n++)
                                {
                                    ToBank = dsClearingBanks.Tables[0].Rows[n]["BankID"].ToString().Trim();
                                    string OldBankID = ToBank;
                                    BRBase.BRDataSet dsClearingThroughThisBank = new BRBase.BRDataSet();


                                    try
                                    {
                                        OutClearingFile.ClearingUniversalMethod(usrInfo, "p_OutClearingThroughBank", out dsClearingThroughThisBank, BRBase.BRModule.GenerateClearingFile, GetConnection(), new object[] { "BrDataSet" }, new object[] { ToBank.ToString() });
                                    }
                                    catch (Exception ex)
                                    {
                                        string AppendErrorMessage = "Error Message 911:" + ex.ToString() + Environment.NewLine + "Date" + ":" + DateTime.Now + Environment.NewLine + "--------------------------" + Environment.NewLine;
                                        System.IO.File.AppendAllText("C:\\ClearingFiles\\" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + ".txt", AppendErrorMessage);
                                    }
                                    if (dsClearingThroughThisBank.Tables.Count > 0)
                                    {
                                        if (dsClearingThroughThisBank.Tables[0].Rows.Count > 1)
                                        {
                                            ToBank = string.Empty;
                                            ToBank = "IN ('";
                                            for (Int32 f = 0; f < dsClearingThroughThisBank.Tables[0].Rows.Count; f++) //BankWise
                                            {

                                                ToBank = ToBank + dsClearingThroughThisBank.Tables[0].Rows[f]["BankID"].ToString() + "','";
                                            }
                                            ToBank = ToBank.Substring(0, ToBank.LastIndexOf(","));
                                            ToBank = ToBank + ")";
                                        }
                                        else
                                        {
                                            ToBank = "IN ('" + ToBank + "')";
                                        }

                                    }
                                    //dsTrxClearingBankWise.Tables[0].Clear();
                                    //ToClearingBank = dsClearingBanks.Tables[0].Rows[k]["ClearingThrough"].ToString();
                                    //ToClearingBank = ToClearingBank.ToString().Trim();




                                    //60
                                    arryRow = new Object[10];
                                            RecordType = "06";
                                            // RecordType
                                            arryRow[t] = RecordType;
                                            t = t + 1;

                                            // ToBank
                                            arryRow[t] = OldBankID;
                                            t = t + 1;

                                            // Currency
                                            arryRow[t] = "60";
                                            t = t + 1;

                                            // Presentments
                                            EJdataTable.Tables[0].Rows.Clear();
                                            drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("(SERIALNUMBER IS NOT NULL OR SERIALNUMBER <> 0) AND CHEQUEDIGIT IS NOT NULL  AND RETURNCODE = '00' AND VOUCHERCODE IN('60') AND TOBANK " + ToBank + " AND TRXTYPE='OC'");
                                            foreach (DataRow dvr in drHeaderFileFormatResult)
                                            {
                                                EJdataTable.Tables[0].ImportRow(dvr);
                                                SumCheques = SumCheques + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                            }
                                            EJdataTable.AcceptChanges();
                                            TotalCountCheques = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                            TotalCountCheques = new string('0', 6 - TotalCountCheques.Length) + TotalCountCheques;
                                            // Pramod
                                            //Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumCheques)));
                                            if (Curr == "00")
                                            {
                                                Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumCheques)));
                                            }
                                            else
                                            {
                                                Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumCheques), "60"));
                                            }
                                            Value = new string('0', 14 - Value.Length) + Value;
                                            EJdataTable.Tables[0].Rows.Clear();

                                            // Presentments Count
                                            arryRow[t] = TotalCountCheques;
                                            t = t + 1;

                                            // Presentments Sum
                                            arryRow[t] = Value;
                                            t = t + 1;

                                            // Discrepancy
                                            EJdataTable.Tables[0].Rows.Clear();
                                            drHeaderFileFormatResult = dstrxClearing.Tables[0].Select(" CHEQUEDIGIT IS NOT NULL AND RETURNCODE = '00' AND VOUCHERCODE IN('60') AND TOBANK " + ToBank + " AND TRXTYPE='OC'");
                                            foreach (DataRow dvr in drHeaderFileFormatResult)
                                            {
                                                EJdataTable.Tables[0].ImportRow(dvr);
                                                SumDiscCheques = SumDiscCheques + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                            }
                                            EJdataTable.AcceptChanges();
                                            TotalDiscCountCheques = "0"; //TotalDiscCountCheques = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                            TotalDiscCountCheques = new string('0', 6 - TotalDiscCountCheques.Length) + TotalDiscCountCheques;
                                            Value = "0"; //Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumDiscCheques)));
                                            Value = new string('0', 13 - Value.Length) + Value;
                                            EJdataTable.Tables[0].Rows.Clear();

                                            // Discrepancy Count Discrepancy
                                            arryRow[t] = TotalDiscCountCheques;
                                            t = t + 1;

                                            // Discrepancy Sum Discrepancy
                                            arryRow[t] = "+" + Value;
                                            t = t + 1;

                                            // Unpaid
                                            EJdataTable.Tables[0].Rows.Clear();
                                            drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("CHEQUEDIGIT IS NOT NULL AND RETURNCODE <> '00' AND VOUCHERCODE IN('60') AND TOBANK " + ToBank + " AND TRXTYPE='OC'");
                                            foreach (DataRow dvr in drHeaderFileFormatResult)
                                            {
                                                EJdataTable.Tables[0].ImportRow(dvr);
                                                SumUnpaidCheques = SumUnpaidCheques + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                            }
                                            EJdataTable.AcceptChanges();
                                            TotalUnpaidCountCheques = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                            TotalUnpaidCountCheques = new string('0', 6 - TotalUnpaidCountCheques.Length) + TotalUnpaidCountCheques;
                                            // Pramod
                                            //UnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumUnpaidCheques)));
                                            if (Curr == "00")
                                            {
                                                UnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumUnpaidCheques)));
                                            }
                                            else
                                            {
                                                UnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumUnpaidCheques),"60"));
                                            }
                                            UnpaidValue = new string('0', 14 - UnpaidValue.Length) + UnpaidValue;
                                            EJdataTable.Tables[0].Rows.Clear();

                                            // Unpaid Count Discrepancy
                                            arryRow[t] = TotalUnpaidCountCheques;
                                            t = t + 1;

                                            // Unpaid Sum Discrepancy
                                            arryRow[t] = UnpaidValue;
                                            t = t + 1;

                                            // Filler
                                            arryRow[t] = new string('0', 12);
                                            t = t + 1;

                                            //Reconstruct here
                                            Data = string.Empty;
                                            for (m = 0; m < arryRow.Length; m++)
                                            {
                                                Data = Data +  (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
                                            }
                                            t = 0;
                                            dt.Rows.Add(Data);
                                                                        //BankID    CRNo    CRAmount    DRNo               DrAmt    MDVNo   MDVAmt  DRUnpaidNo              DRUnpaidAmt         DiscNo  DiscAmt     CrUnpaidNo  CRUnPaidAmt     Data        Date            Currency    TrxType     FileDate
                                            SettlementListingDt.Tables[0].Rows.Add(OldBankID, 0, 0, BRBase.BRBaseConvert.ConvertToInt32(TotalCountCheques), SumCheques, 0, 0, BRBase.BRBaseConvert.ConvertToInt32(TotalUnpaidCountCheques), SumUnpaidCheques, 0, 0, 0, 0, Data, WDate, "60", "O", FDate);


                                            SumCheques = 0;
                                            SumChq = string.Empty;
                                            SumCredits = 0;
                                            SumCr = string.Empty;
                                            SumDebits = 0;
                                            SumDr = string.Empty;
                                            UnpaidValue = "0";
                                            SumUnpaidCheques = 0;
                                            SumUnChq = string.Empty;
                                            SumunpaidCredits = 0;
                                            SumUnCr = string.Empty;
                                            SumUnpaidDebits = 0;
                                            SumUnDr = string.Empty;
                                            SumDiscCheques = 0;
                                            SumDiscChq = string.Empty;
                                            TotalCountCheques = "0";
                                            TotalCountDebits = "0";
                                            TotalCountCredits = "0";
                                            TotalUnpaidCountCheques = "0";
                                            TotalUnpaidCountDebits = "0";
                                            TotalUnpaidCountCredits = "0";
                                            TotalDiscCountCheques = "0";
                                            MDVValue = "0";
                                            TotalCountMDV = "0";
                                            SumunpaidMDV = 0;
                                            SumMDV = 0;
                                            TotalUnpaidCountMDV = "0";
                                            MDVUnpaidValue = "0";
                                    
                                        //61
                                            EJdataTable.Tables[0].Rows.Clear();
                                            arryRow = new Object[10];
                                            RecordType = "06";
                                            // RecordType
                                            arryRow[t] = RecordType;
                                            t = t + 1;

                                            // ToBank
                                            arryRow[t] = OldBankID;
                                            t = t + 1;

                                            // Currency
                                            arryRow[t] = "61";
                                            t = t + 1;

                                            // Presentments
                                            drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("(SERIALNUMBER IS NOT NULL OR SERIALNUMBER <> 0) AND CHEQUEDIGIT IS NOT NULL  AND RETURNCODE = '00' AND VOUCHERCODE IN('61') AND TOBANK " + ToBank + " AND TRXTYPE='OC'");
                                            foreach (DataRow dvr in drHeaderFileFormatResult)
                                            {
                                                EJdataTable.Tables[0].ImportRow(dvr);
                                                SumCheques = SumCheques + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                            }
                                            EJdataTable.AcceptChanges();
                                            TotalCountCheques = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                            TotalCountCheques = new string('0', 6 - TotalCountCheques.Length) + TotalCountCheques;
                                            // Pramod
                                            //Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumCheques)));
                                            if (Curr == "00")
                                            {
                                                Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumCheques)));
                                            }
                                            else
                                            {
                                                Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumCheques),"60"));
                                            }
                                            Value = new string('0', 14 - Value.Length) + Value;
                                            EJdataTable.Tables[0].Rows.Clear();

                                            // Presentments Count
                                            arryRow[t] = TotalCountCheques;
                                            t = t + 1;

                                            // Presentments Sum
                                            arryRow[t] = Value;
                                            t = t + 1;

                                            // Discrepancy
                                            EJdataTable.Tables[0].Rows.Clear();
                                            drHeaderFileFormatResult = dstrxClearing.Tables[0].Select(" CHEQUEDIGIT IS NOT NULL AND RETURNCODE = '00' AND VOUCHERCODE IN('61') AND TOBANK " + ToBank + " AND TRXTYPE='OC'");
                                            foreach (DataRow dvr in drHeaderFileFormatResult)
                                            {
                                                EJdataTable.Tables[0].ImportRow(dvr);
                                                SumDiscCheques = SumDiscCheques + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                            }
                                            EJdataTable.AcceptChanges();
                                            TotalDiscCountCheques = "0"; //TotalDiscCountCheques = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                            TotalDiscCountCheques = new string('0', 6 - TotalDiscCountCheques.Length) + TotalDiscCountCheques;
                                            Value = "0"; //Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumDiscCheques)));
                                            Value = new string('0', 13 - Value.Length) + Value;
                                            EJdataTable.Tables[0].Rows.Clear();

                                            // Discrepancy Count Discrepancy
                                            arryRow[t] = TotalDiscCountCheques;
                                            t = t + 1;

                                            // Discrepancy Sum Discrepancy
                                            arryRow[t] = "+" + Value;
                                            t = t + 1;

                                            // Unpaid
                                            EJdataTable.Tables[0].Rows.Clear();
                                            drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("CHEQUEDIGIT IS NOT NULL AND RETURNCODE <> '00' AND VOUCHERCODE IN('61') AND TOBANK " + ToBank + " AND TRXTYPE='OC'");
                                            foreach (DataRow dvr in drHeaderFileFormatResult)
                                            {
                                                EJdataTable.Tables[0].ImportRow(dvr);
                                                SumUnpaidCheques = SumUnpaidCheques + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                            }
                                            EJdataTable.AcceptChanges();
                                            TotalUnpaidCountCheques = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                            TotalUnpaidCountCheques = new string('0', 6 - TotalUnpaidCountCheques.Length) + TotalUnpaidCountCheques;
                                            //UnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumUnpaidCheques)));
                                            
                                            UnpaidValue = new string('0', 14 - UnpaidValue.Length) + UnpaidValue;
                                            EJdataTable.Tables[0].Rows.Clear();

                                            // Unpaid Count Discrepancy
                                            arryRow[t] = TotalUnpaidCountCheques;
                                            t = t + 1;

                                            // Unpaid Sum Discrepancy
                                            arryRow[t] = UnpaidValue;
                                            t = t + 1;

                                            // Filler
                                            arryRow[t] = new string('0', 12);
                                            t = t + 1;

                                            //Reconstruct here
                                            Data = string.Empty;
                                            for (m = 0; m < arryRow.Length; m++)
                                            {
                                                Data = Data +  (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
                                            }
                                            t = 0;
                                            dt.Rows.Add(Data);
                                            //BankID    CRNo    CRAmount    DRNo               DrAmt    MDVNo   MDVAmt  DRUnpaidNo              DRUnpaidAmt         DiscNo  DiscAmt     CrUnpaidNo  CRUnPaidAmt     Data        Date            Currency    TrxType     FileDate
                                            SettlementListingDt.Tables[0].Rows.Add(OldBankID, 0, 0, BRBase.BRBaseConvert.ConvertToInt32(TotalCountCheques), SumCheques, 0, 0, BRBase.BRBaseConvert.ConvertToInt32(TotalUnpaidCountCheques), SumUnpaidCheques, 0, 0, 0, 0, Data, WDate, "61", "O", FDate);

                                            SumCheques = 0;
                                            SumChq = string.Empty;
                                            SumCredits = 0;
                                            SumCr = string.Empty;
                                            SumDebits = 0;
                                            SumDr = string.Empty;
                                            UnpaidValue = "0";
                                            SumUnpaidCheques = 0;
                                            SumUnChq = string.Empty;
                                            SumunpaidCredits = 0;
                                            SumUnCr = string.Empty;
                                            SumUnpaidDebits = 0;
                                            SumUnDr = string.Empty;
                                            SumDiscCheques = 0;
                                            SumDiscChq = string.Empty;
                                            TotalCountCheques = "0";
                                            TotalCountDebits = "0";
                                            TotalCountCredits = "0";
                                            TotalUnpaidCountCheques = "0";
                                            TotalUnpaidCountDebits = "0";
                                            TotalUnpaidCountCredits = "0";
                                            TotalDiscCountCheques = "0";
                                            MDVValue = "0";
                                            TotalCountMDV = "0";
                                            SumunpaidMDV = 0;
                                            SumMDV = 0;
                                            TotalUnpaidCountMDV = "0";
                                            MDVUnpaidValue = "0";




                                        //62
                                            EJdataTable.Tables[0].Rows.Clear();
                                            arryRow = new Object[10];
                                            RecordType = "06";
                                            // RecordType
                                            arryRow[t] = RecordType;
                                            t = t + 1;

                                            // ToBank
                                            arryRow[t] = OldBankID;
                                            t = t + 1;

                                            // Currency
                                            arryRow[t] = "62";
                                            t = t + 1;

                                            // Presentments
                                            EJdataTable.Tables[0].Rows.Clear();
                                            drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("(SERIALNUMBER IS NOT NULL OR SERIALNUMBER <> 0) AND CHEQUEDIGIT IS NOT NULL  AND RETURNCODE = '00' AND VOUCHERCODE IN('62') AND TOBANK " + ToBank + " AND TRXTYPE='OC'");
                                            foreach (DataRow dvr in drHeaderFileFormatResult)
                                            {
                                                EJdataTable.Tables[0].ImportRow(dvr);
                                                SumCheques = SumCheques + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                            }
                                            EJdataTable.AcceptChanges();
                                            TotalCountCheques = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                            TotalCountCheques = new string('0', 6 - TotalCountCheques.Length) + TotalCountCheques;
                                            //Pramod
                                            //Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumCheques)));
                                            if (Curr == "00")
                                            {
                                                Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumCheques)));
                                            }
                                            else
                                            {
                                                Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumCheques),"60"));
                                            }
                                            Value = new string('0', 14 - Value.Length) + Value;
                                            EJdataTable.Tables[0].Rows.Clear();

                                            // Presentments Count
                                            arryRow[t] = TotalCountCheques;
                                            t = t + 1;

                                            // Presentments Sum
                                            arryRow[t] = Value;
                                            t = t + 1;

                                            // Discrepancy
                                            EJdataTable.Tables[0].Rows.Clear();
                                            drHeaderFileFormatResult = dstrxClearing.Tables[0].Select(" CHEQUEDIGIT IS NOT NULL AND RETURNCODE = '00' AND VOUCHERCODE IN('62') AND TOBANK " + ToBank + " AND TRXTYPE='OC'");
                                            foreach (DataRow dvr in drHeaderFileFormatResult)
                                            {
                                                EJdataTable.Tables[0].ImportRow(dvr);
                                                SumDiscCheques = SumDiscCheques + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                            }
                                            EJdataTable.AcceptChanges();
                                            TotalDiscCountCheques = "0"; //TotalDiscCountCheques = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                            TotalDiscCountCheques = new string('0', 6 - TotalDiscCountCheques.Length) + TotalDiscCountCheques;
                                            Value = "0"; //Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumDiscCheques)));
                                            Value = new string('0', 13 - Value.Length) + Value;
                                            EJdataTable.Tables[0].Rows.Clear();

                                            // Discrepancy Count Discrepancy
                                            arryRow[t] = TotalDiscCountCheques;
                                            t = t + 1;

                                            // Discrepancy Sum Discrepancy
                                            arryRow[t] = "+" + Value;
                                            t = t + 1;

                                            // Unpaid
                                            EJdataTable.Tables[0].Rows.Clear();
                                            drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("CHEQUEDIGIT IS NOT NULL AND RETURNCODE <> '00' AND VOUCHERCODE IN('62') AND TOBANK " + ToBank + " AND TRXTYPE='OC'");
                                            foreach (DataRow dvr in drHeaderFileFormatResult)
                                            {
                                                EJdataTable.Tables[0].ImportRow(dvr);
                                                SumUnpaidCheques = SumUnpaidCheques + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                            }
                                            EJdataTable.AcceptChanges();
                                            TotalUnpaidCountCheques = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                            TotalUnpaidCountCheques = new string('0', 6 - TotalUnpaidCountCheques.Length) + TotalUnpaidCountCheques;
                                            // Pramod
                                            //UnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumUnpaidCheques)));
                                            if (Curr == "00")
                                            {
                                                UnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumUnpaidCheques)));
                                            }
                                            else
                                            {
                                                UnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumUnpaidCheques),"60"));
                                            }
                                            UnpaidValue = new string('0', 14 - UnpaidValue.Length) + UnpaidValue;
                                            EJdataTable.Tables[0].Rows.Clear();

                                            // Unpaid Count Discrepancy
                                            arryRow[t] = TotalUnpaidCountCheques;
                                            t = t + 1;

                                            // Unpaid Sum Discrepancy
                                            arryRow[t] = UnpaidValue;
                                            t = t + 1;

                                            // Filler
                                            arryRow[t] = new string('0', 12);
                                            t = t + 1;

                                            //Reconstruct here
                                            Data = string.Empty;
                                            for (m = 0; m < arryRow.Length; m++)
                                            {
                                                Data = Data +  (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
                                            }
                                            t = 0;
                                            dt.Rows.Add(Data);
                                            //BankID    CRNo    CRAmount    DRNo               DrAmt    MDVNo   MDVAmt  DRUnpaidNo              DRUnpaidAmt         DiscNo  DiscAmt     CrUnpaidNo  CRUnPaidAmt     Data        Date            Currency    TrxType     FileDate
                                            SettlementListingDt.Tables[0].Rows.Add(OldBankID, 0, 0, BRBase.BRBaseConvert.ConvertToInt32(TotalCountCheques), SumCheques, 0, 0, BRBase.BRBaseConvert.ConvertToInt32(TotalUnpaidCountCheques), SumUnpaidCheques, 0, 0, 0, 0, Data, WDate, "62", "O", FDate);

                                            SumCheques = 0;
                                            SumChq = string.Empty;
                                            SumCredits = 0;
                                            SumCr = string.Empty;
                                            SumDebits = 0;
                                            SumDr = string.Empty;
                                            UnpaidValue = "0";
                                            SumUnpaidCheques = 0;
                                            SumUnChq = string.Empty;
                                            SumunpaidCredits = 0;
                                            SumUnCr = string.Empty;
                                            SumUnpaidDebits = 0;
                                            SumUnDr = string.Empty;
                                            SumDiscCheques = 0;
                                            SumDiscChq = string.Empty;
                                            TotalCountCheques = "0";
                                            TotalCountDebits = "0";
                                            TotalCountCredits = "0";
                                            TotalUnpaidCountCheques = "0";
                                            TotalUnpaidCountDebits = "0";
                                            TotalUnpaidCountCredits = "0";
                                            TotalDiscCountCheques = "0";
                                            MDVValue = "0";
                                            TotalCountMDV = "0";
                                            SumunpaidMDV = 0;
                                            SumMDV = 0;
                                            TotalUnpaidCountMDV = "0";
                                            MDVUnpaidValue = "0";


                                    // 07
                                         //60
                                            EJdataTable.Tables[0].Rows.Clear();
                                            arryRow = new Object[8];
                                            t = 0;
                                            RecordType = "07";
                                            // RecordType
                                            arryRow[t] = RecordType;
                                            t = t + 1;

                                            // ToBank
                                            arryRow[t] = OldBankID;
                                            t = t + 1;

                                            // Currency
                                            arryRow[t] = "60";
                                            t = t + 1;

                                            //Presentments
                                            EJdataTable.Tables[0].Rows.Clear();
                                            drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("(SERIALNUMBER IS NOT NULL OR SERIALNUMBER = 0) AND (CHEQUEDIGIT IS NULL OR CHEQUEDIGIT='') AND VOUCHERCODE ='40' AND RETURNCODE = '00' AND TOBANK " + ToBank + " AND TRXTYPE='OC'");
                                            foreach (DataRow dvr in drHeaderFileFormatResult)
                                            {
                                                EJdataTable.Tables[0].ImportRow(dvr);
                                                SumDebits = SumDebits + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                            }
                                            EJdataTable.AcceptChanges();
                                            TotalCountDebits = "0"; //TotalCountDebits = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                            TotalCountDebits = new string('0', 6 - TotalCountDebits.Length) + TotalCountDebits;
                                            Value = "0";// Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumDebits)));
                                            Value = new string('0', 14 - Value.Length) + Value;
                                            EJdataTable.Tables[0].Rows.Clear();

                                            // Presentments Count
                                            arryRow[t] = TotalCountDebits;
                                            t = t + 1;

                                            // Presentments Sum
                                            arryRow[t] = Value;
                                            t = t + 1;

                                            //UnPaid
                                            EJdataTable.Tables[0].Rows.Clear();
                                            drHeaderFileFormatResult = null;
                                            drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("(SERIALNUMBER IS NOT NULL OR SERIALNUMBER = 0) AND (CHEQUEDIGIT IS NULL OR CHEQUEDIGIT='') AND VOUCHERCODE ='40' AND RETURNCODE <> '00' AND TOBANK " + ToBank + " AND TRXTYPE='OC'");
                                            foreach (DataRow dvr in drHeaderFileFormatResult)
                                            {
                                                EJdataTable.Tables[0].ImportRow(dvr);
                                                SumUnpaidDebits = SumUnpaidDebits + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                            }
                                            EJdataTable.AcceptChanges();
                                            TotalUnpaidCountDebits = "0"; //TotalUnpaidCountDebits = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                            TotalUnpaidCountDebits = new string('0', 6 - TotalUnpaidCountDebits.Length) + TotalUnpaidCountDebits;
                                            UnpaidValue = "0"; //UnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumUnpaidDebits)));
                                            UnpaidValue = new string('0', 14 - UnpaidValue.Length) + UnpaidValue;
                                            EJdataTable.Tables[0].Rows.Clear();

                                            // UnPaid Count
                                            arryRow[t] = TotalUnpaidCountDebits;
                                            t = t + 1;

                                            // UnPaid Sum
                                            arryRow[t] = UnpaidValue;
                                            t = t + 1;

                                            // Filler
                                            arryRow[t] = new string('0', 32);
                                            t = t + 1;

                                            //Reconstruct here
                                            Data = string.Empty;
                                            for (m = 0; m < arryRow.Length; m++)
                                            {
                                                Data = Data +  (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
                                            }
                                            t = 0;
                                            dt.Rows.Add(Data);
                                            //BankID    CRNo    CRAmount    DRNo               DrAmt    MDVNo   MDVAmt  DRUnpaidNo              DRUnpaidAmt         DiscNo  DiscAmt     CrUnpaidNo  CRUnPaidAmt     Data        Date            Currency    TrxType     FileDate
                                            SettlementListingDt.Tables[0].Rows.Add(OldBankID, 0, 0, BRBase.BRBaseConvert.ConvertToInt32(TotalCountDebits), SumDebits, 0, 0, BRBase.BRBaseConvert.ConvertToInt32(TotalUnpaidCountDebits), SumUnpaidDebits, 0, 0, 0, 0, Data, WDate, "60", "O", FDate);


                                        //61
                                            arryRow = new Object[8];
                                            t = 0;
                                            RecordType = "07";
                                            // RecordType
                                            arryRow[t] = RecordType;
                                            t = t + 1;

                                            // ToBank
                                            arryRow[t] = OldBankID;
                                            t = t + 1;

                                            // Currency
                                            arryRow[t] = "61";
                                            t = t + 1;

                                            //Presentments
                                            EJdataTable.Tables[0].Rows.Clear();
                                            drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("CHEQUEDIGIT IS NULL AND VOUCHERCODE ='40' AND RETURNCODE = '00' AND TOBANK " + ToBank + " AND TRXTYPE='OD'");
                                            foreach (DataRow dvr in drHeaderFileFormatResult)
                                            {
                                                EJdataTable.Tables[0].ImportRow(dvr);
                                                SumDebits = SumDebits + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                            }
                                            EJdataTable.AcceptChanges();
                                            TotalCountDebits = "0"; //TotalCountDebits = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                            TotalCountDebits = new string('0', 6 - TotalCountDebits.Length) + TotalCountDebits;
                                            Value = "0";// Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumDebits)));
                                            Value = new string('0', 14 - Value.Length) + Value;
                                            EJdataTable.Tables[0].Rows.Clear();

                                            // Presentments Count
                                            arryRow[t] = TotalCountDebits;
                                            t = t + 1;

                                            // Presentments Sum
                                            arryRow[t] = Value;
                                            t = t + 1;

                                            //UnPaid
                                            EJdataTable.Tables[0].Rows.Clear();
                                            drHeaderFileFormatResult = null;
                                            drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("CHEQUEDIGIT IS NULL AND VOUCHERCODE ='40' AND RETURNCODE <> '00' AND TOBANK " + ToBank + " AND TRXTYPE='OD'");
                                            foreach (DataRow dvr in drHeaderFileFormatResult)
                                            {
                                                EJdataTable.Tables[0].ImportRow(dvr);
                                                SumUnpaidDebits = SumUnpaidDebits + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                            }
                                            EJdataTable.AcceptChanges();
                                            TotalUnpaidCountDebits = "0"; //TotalUnpaidCountDebits = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                            TotalUnpaidCountDebits = new string('0', 6 - TotalUnpaidCountDebits.Length) + TotalUnpaidCountDebits;
                                            UnpaidValue = "0"; //UnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumUnpaidDebits)));
                                            UnpaidValue = new string('0', 14 - UnpaidValue.Length) + UnpaidValue;
                                            EJdataTable.Tables[0].Rows.Clear();

                                            // UnPaid Count
                                            arryRow[t] = TotalUnpaidCountDebits;
                                            t = t + 1;

                                            // UnPaid Sum
                                            arryRow[t] = UnpaidValue;
                                            t = t + 1;

                                            // Filler
                                            arryRow[t] = new string('0', 32);
                                            t = t + 1;

                                            //Reconstruct here
                                            Data = string.Empty;
                                            for (m = 0; m < arryRow.Length; m++)
                                            {
                                                Data = Data +  (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
                                            }
                                            t = 0;
                                            dt.Rows.Add(Data);
                                            //BankID    CRNo    CRAmount    DRNo               DrAmt    MDVNo   MDVAmt  DRUnpaidNo              DRUnpaidAmt         DiscNo  DiscAmt     CrUnpaidNo  CRUnPaidAmt     Data        Date            Currency    TrxType     FileDate
                                            SettlementListingDt.Tables[0].Rows.Add(OldBankID, 0, 0, BRBase.BRBaseConvert.ConvertToInt32(TotalCountDebits), SumDebits, 0, 0, BRBase.BRBaseConvert.ConvertToInt32(TotalUnpaidCountDebits), SumUnpaidDebits, 0, 0, 0, 0, Data, WDate, "61", "O", FDate);

                                        //62
                                            arryRow = new Object[8];
                                            t = 0;
                                            RecordType = "07";
                                            // RecordType
                                            arryRow[t] = RecordType;
                                            t = t + 1;

                                            // ToBank
                                            arryRow[t] = OldBankID;
                                            t = t + 1;

                                            // Currency
                                            arryRow[t] = "62";
                                            t = t + 1;

                                            //Presentments
                                            EJdataTable.Tables[0].Rows.Clear();
                                            drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("(SERIALNUMBER IS NOT NULL OR SERIALNUMBER = 0) AND (CHEQUEDIGIT IS NULL OR CHEQUEDIGIT='') AND VOUCHERCODE ='40' AND RETURNCODE = '00' AND TOBANK " + ToBank + " AND TRXTYPE='OC'");
                                            foreach (DataRow dvr in drHeaderFileFormatResult)
                                            {
                                                EJdataTable.Tables[0].ImportRow(dvr);
                                                SumDebits = SumDebits + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                            }
                                            EJdataTable.AcceptChanges();
                                            TotalCountDebits = "0"; //TotalCountDebits = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                            TotalCountDebits = new string('0', 6 - TotalCountDebits.Length) + TotalCountDebits;
                                            Value = "0";// Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumDebits)));
                                            Value = new string('0', 14 - Value.Length) + Value;
                                            EJdataTable.Tables[0].Rows.Clear();

                                            // Presentments Count
                                            arryRow[t] = TotalCountDebits;
                                            t = t + 1;

                                            // Presentments Sum
                                            arryRow[t] = Value;
                                            t = t + 1;

                                            //UnPaid
                                            EJdataTable.Tables[0].Rows.Clear();
                                            drHeaderFileFormatResult = null;
                                            drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("(SERIALNUMBER IS NOT NULL OR SERIALNUMBER = 0) AND (CHEQUEDIGIT IS NULL OR CHEQUEDIGIT='') AND VOUCHERCODE ='40' AND RETURNCODE <> '00' AND TOBANK " + ToBank + " AND TRXTYPE='OC'");
                                            foreach (DataRow dvr in drHeaderFileFormatResult)
                                            {
                                                EJdataTable.Tables[0].ImportRow(dvr);
                                                SumUnpaidDebits = SumUnpaidDebits + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                            }
                                            EJdataTable.AcceptChanges();
                                            TotalUnpaidCountDebits = "0"; //TotalUnpaidCountDebits = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                            TotalUnpaidCountDebits = new string('0', 6 - TotalUnpaidCountDebits.Length) + TotalUnpaidCountDebits;
                                            UnpaidValue = "0"; //UnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumUnpaidDebits)));
                                            UnpaidValue = new string('0', 14 - UnpaidValue.Length) + UnpaidValue;
                                            EJdataTable.Tables[0].Rows.Clear();

                                            // UnPaid Count
                                            arryRow[t] = TotalUnpaidCountDebits;
                                            t = t + 1;

                                            // UnPaid Sum
                                            arryRow[t] = UnpaidValue;
                                            t = t + 1;

                                            // Filler
                                            arryRow[t] = new string('0', 32);
                                            t = t + 1;

                                            //Reconstruct here
                                            Data = string.Empty;
                                            for (m = 0; m < arryRow.Length; m++)
                                            {
                                                Data = Data +  (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
                                            }
                                            t = 0;
                                            dt.Rows.Add(Data);
                                            //BankID    CRNo    CRAmount    DRNo               DrAmt    MDVNo   MDVAmt  DRUnpaidNo              DRUnpaidAmt         DiscNo  DiscAmt     CrUnpaidNo  CRUnPaidAmt     Data        Date            Currency    TrxType     FileDate
                                            SettlementListingDt.Tables[0].Rows.Add(OldBankID, 0, 0, BRBase.BRBaseConvert.ConvertToInt32(TotalCountDebits), SumDebits, 0, 0, BRBase.BRBaseConvert.ConvertToInt32(TotalUnpaidCountDebits), SumUnpaidDebits, 0, 0, 0, 0, Data, WDate, "62", "O", FDate);



                                    // 08
                                         //60
                                            arryRow = new Object[8];
                                            t = 0;
                                            RecordType = "08";
                                            // RecordType
                                            arryRow[t] = RecordType;
                                            t = t + 1;

                                            // ToBank
                                            arryRow[t] = OldBankID;
                                            t = t + 1;

                                            // Currency
                                            arryRow[t] = "60";
                                            t = t + 1;

                                            //Presentation
                                            EJdataTable.Tables[0].Rows.Clear();
                                            drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("VOUCHERCODE <>'40' AND RETURNCODE IN('00','90','97') AND TOBANK " + ToBank + "  AND TRXTYPE='OD'");
                                            foreach (DataRow dvr in drHeaderFileFormatResult)
                                            {
                                                EJdataTable.Tables[0].ImportRow(dvr);
                                                SumCredits = SumCredits + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                            }
                                            EJdataTable.AcceptChanges();
                                            TotalCountCredits = "0"; //TotalCountCredits = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                            TotalCountCredits = new string('0', 6 - TotalCountCredits.Length) + TotalCountCredits;
                                            Value = "0"; //Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumCredits)));
                                            Value = new string('0', 14 - Value.Length) + Value;
                                            // Presentments Count
                                            arryRow[t] = TotalCountCredits;
                                            t = t + 1;

                                            // Presentments Sum
                                            arryRow[t] = Value;
                                            t = t + 1;

                                            //UnPaids
                                            EJdataTable.Tables[0].Rows.Clear();
                                            drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("VOUCHERCODE <>'40' AND RETURNCODE NOT IN('00','90','97') AND TOBANK " + ToBank + "  AND TRXTYPE='OD'");
                                            foreach (DataRow dvr in drHeaderFileFormatResult)
                                            {
                                                EJdataTable.Tables[0].ImportRow(dvr);
                                                SumunpaidCredits = SumunpaidCredits + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                            }
                                            EJdataTable.AcceptChanges();
                                            TotalUnpaidCountCredits = "0"; //TotalUnpaidCountCredits = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                            TotalUnpaidCountCredits = new string('0', 6 - TotalUnpaidCountCredits.Length) + TotalUnpaidCountCredits;
                                            UnpaidValue = "0"; //UnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumunpaidCredits)));
                                            UnpaidValue = new string('0', 14 - UnpaidValue.Length) + UnpaidValue;
                                            // UnPaid Count
                                            arryRow[t] = TotalUnpaidCountCredits;
                                            t = t + 1;

                                            // UnPaid Sum
                                            arryRow[t] = UnpaidValue;
                                            t = t + 1;

                                            // Filler
                                            arryRow[t] = new string('0', 32);
                                            t = t + 1;

                                            //Reconstruct here
                                            Data = string.Empty;
                                            for (m = 0; m < arryRow.Length; m++)
                                            {
                                                Data = Data +  (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
                                            }
                                            t = 0;
                                            dt.Rows.Add(Data);

                                            //BankID    CRNo    CRAmount    DRNo               DrAmt    MDVNo   MDVAmt  DRUnpaidNo              DRUnpaidAmt         DiscNo  DiscAmt     CrUnpaidNo  CRUnPaidAmt     Data        Date            Currency    TrxType     FileDate
                                            SettlementListingDt.Tables[0].Rows.Add(OldBankID, BRBase.BRBaseConvert.ConvertToInt32(TotalCountCredits), SumCredits, 0, 0, 0, 0, BRBase.BRBaseConvert.ConvertToInt32(TotalUnpaidCountCredits), SumunpaidCredits, 0, 0, 0, 0, Data, WDate, "60", "O", FDate);

                                        //61
                                            //61
                                            arryRow = new Object[8];
                                            t = 0;
                                            RecordType = "08";
                                            // RecordType
                                            arryRow[t] = RecordType;
                                            t = t + 1;

                                            // ToBank
                                            arryRow[t] = OldBankID;
                                            t = t + 1;

                                            // Currency
                                            arryRow[t] = "61";
                                            t = t + 1;

                                            //Presentation
                                            EJdataTable.Tables[0].Rows.Clear();
                                            drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("VOUCHERCODE <>'40' AND RETURNCODE IN('00','90','97') AND TOBANK " + ToBank + "  AND TRXTYPE='OD'");
                                            foreach (DataRow dvr in drHeaderFileFormatResult)
                                            {
                                                EJdataTable.Tables[0].ImportRow(dvr);
                                                SumCredits = SumCredits + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                            }
                                            EJdataTable.AcceptChanges();
                                            TotalCountCredits = "0"; //TotalCountCredits = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                            TotalCountCredits = new string('0', 6 - TotalCountCredits.Length) + TotalCountCredits;
                                            Value = "0"; //Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumCredits)));
                                            Value = new string('0', 14 - Value.Length) + Value;
                                            // Presentments Count
                                            arryRow[t] = TotalCountCredits;
                                            t = t + 1;

                                            // Presentments Sum
                                            arryRow[t] = Value;
                                            t = t + 1;

                                            //UnPaids
                                            EJdataTable.Tables[0].Rows.Clear();
                                            drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("VOUCHERCODE <>'40' AND RETURNCODE NOT IN('00','90','97') AND TOBANK " + ToBank + "  AND TRXTYPE='OD'");
                                            foreach (DataRow dvr in drHeaderFileFormatResult)
                                            {
                                                EJdataTable.Tables[0].ImportRow(dvr);
                                                SumunpaidCredits = SumunpaidCredits + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                            }
                                            EJdataTable.AcceptChanges();
                                            TotalUnpaidCountCredits = "0"; //TotalUnpaidCountCredits = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                            TotalUnpaidCountCredits = new string('0', 6 - TotalUnpaidCountCredits.Length) + TotalUnpaidCountCredits;
                                            UnpaidValue = "0"; //UnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumunpaidCredits)));
                                            UnpaidValue = new string('0', 14 - UnpaidValue.Length) + UnpaidValue;
                                            // UnPaid Count
                                            arryRow[t] = TotalUnpaidCountCredits;
                                            t = t + 1;

                                            // UnPaid Sum
                                            arryRow[t] = UnpaidValue;
                                            t = t + 1;

                                            // Filler
                                            arryRow[t] = new string('0', 32);
                                            t = t + 1;

                                            //Reconstruct here
                                            Data = string.Empty;
                                            for (m = 0; m < arryRow.Length; m++)
                                            {
                                                Data = Data +  (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
                                            }
                                            t = 0;
                                            dt.Rows.Add(Data);

                                            //BankID    CRNo    CRAmount    DRNo               DrAmt    MDVNo   MDVAmt  DRUnpaidNo              DRUnpaidAmt         DiscNo  DiscAmt     CrUnpaidNo  CRUnPaidAmt     Data        Date            Currency    TrxType     FileDate
                                            SettlementListingDt.Tables[0].Rows.Add(OldBankID, BRBase.BRBaseConvert.ConvertToInt32(TotalCountCredits), SumCredits, 0, 0, 0, 0, BRBase.BRBaseConvert.ConvertToInt32(TotalUnpaidCountCredits), SumunpaidCredits, 0, 0, 0, 0, Data, WDate, "61", "O", FDate);

                                        //62
                                            //62
                                            arryRow = new Object[8];
                                            t = 0;
                                            RecordType = "08";
                                            // RecordType
                                            arryRow[t] = RecordType;
                                            t = t + 1;

                                            // ToBank
                                            arryRow[t] = OldBankID;
                                            t = t + 1;

                                            // Currency
                                            arryRow[t] = "62";
                                            t = t + 1;

                                            //Presentation
                                            EJdataTable.Tables[0].Rows.Clear();
                                            drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("VOUCHERCODE <>'40' AND RETURNCODE IN('00','90','97') AND TOBANK " + ToBank + "  AND TRXTYPE='OD'");
                                            foreach (DataRow dvr in drHeaderFileFormatResult)
                                            {
                                                EJdataTable.Tables[0].ImportRow(dvr);
                                                SumCredits = SumCredits + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                            }
                                            EJdataTable.AcceptChanges();
                                            TotalCountCredits = "0"; //TotalCountCredits = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                            TotalCountCredits = new string('0', 6 - TotalCountCredits.Length) + TotalCountCredits;
                                            Value = "0"; //Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumCredits)));
                                            Value = new string('0', 14 - Value.Length) + Value;
                                            // Presentments Count
                                            arryRow[t] = TotalCountCredits;
                                            t = t + 1;

                                            // Presentments Sum
                                            arryRow[t] = Value;
                                            t = t + 1;

                                            //UnPaids
                                            EJdataTable.Tables[0].Rows.Clear();
                                            drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("VOUCHERCODE <>'40' AND RETURNCODE NOT IN('00','90','97') AND TOBANK " + ToBank + "  AND TRXTYPE='OD'");
                                            foreach (DataRow dvr in drHeaderFileFormatResult)
                                            {
                                                EJdataTable.Tables[0].ImportRow(dvr);
                                                SumunpaidCredits = SumunpaidCredits + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                            }
                                            EJdataTable.AcceptChanges();
                                            TotalUnpaidCountCredits = "0"; //TotalUnpaidCountCredits = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                            TotalUnpaidCountCredits = new string('0', 6 - TotalUnpaidCountCredits.Length) + TotalUnpaidCountCredits;
                                            UnpaidValue = "0"; //UnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumunpaidCredits)));
                                            UnpaidValue = new string('0', 14 - UnpaidValue.Length) + UnpaidValue;
                                            // UnPaid Count
                                            arryRow[t] = TotalUnpaidCountCredits;
                                            t = t + 1;

                                            // UnPaid Sum
                                            arryRow[t] = UnpaidValue;
                                            t = t + 1;

                                            // Filler
                                            arryRow[t] = new string('0', 32);
                                            t = t + 1;

                                            //Reconstruct here
                                            Data = string.Empty;
                                            for (m = 0; m < arryRow.Length; m++)
                                            {
                                                Data = Data +  (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
                                            }
                                            t = 0;
                                            dt.Rows.Add(Data);

                                            //BankID    CRNo    CRAmount    DRNo               DrAmt    MDVNo   MDVAmt  DRUnpaidNo              DRUnpaidAmt         DiscNo  DiscAmt     CrUnpaidNo  CRUnPaidAmt     Data        Date            Currency    TrxType     FileDate
                                            SettlementListingDt.Tables[0].Rows.Add(OldBankID, BRBase.BRBaseConvert.ConvertToInt32(TotalCountCredits), SumCredits, 0, 0, 0, 0, BRBase.BRBaseConvert.ConvertToInt32(TotalUnpaidCountCredits), SumunpaidCredits, 0, 0, 0, 0, Data, WDate, "62", "O", FDate);



                                    // 09
                                        //60
                                            arryRow = new Object[8];
                                            t = 0;
                                            RecordType = "09";
                                            // RecordType
                                            arryRow[t] = RecordType;
                                            t = t + 1;

                                            // ToBank
                                            arryRow[t] = OldBankID;
                                            t = t + 1;

                                            // Currency
                                            arryRow[t] = "60";
                                            t = t + 1;

                                            //Presentation
                                            EJdataTable.Tables[0].Rows.Clear();
                                            drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("VOUCHERCODE ='60' AND RETURNCODE = '00' AND TRXTYPE='OC' AND TOBANK " + ToBank + "");
                                            foreach (DataRow dvr in drHeaderFileFormatResult)
                                            {
                                                EJdataTable.Tables[0].ImportRow(dvr);
                                                SumMDV = SumMDV + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                            }
                                            EJdataTable.AcceptChanges();
                                            TotalCountMDV = "0";// TotalCountMDV = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                            TotalCountMDV = new string('0', 6 - TotalCountMDV.Length) + TotalCountMDV;
                                            MDVValue = "0";// MDVValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumMDV)));
                                            MDVValue = new string('0', 14 - MDVValue.Length) + MDVValue;
                                            // Presentments Count
                                            arryRow[t] = TotalCountMDV;
                                            t = t + 1;

                                            // Presentments Sum
                                            arryRow[t] = MDVValue;
                                            t = t + 1;

                                            ////UnPaids
                                            EJdataTable.Tables[0].Rows.Clear();
                                            drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("VOUCHERCODE ='03' AND  RETURNCODE <> '00' AND TRXTYPE='OC' AND TOBANK " + ToBank + "");
                                            foreach (DataRow dvr in drHeaderFileFormatResult)
                                            {
                                                EJdataTable.Tables[0].ImportRow(dvr);
                                                SumunpaidMDV = SumunpaidMDV + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                            }
                                            EJdataTable.AcceptChanges();
                                            TotalUnpaidCountMDV = "0";// = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                            TotalUnpaidCountMDV = new string('0', 6 - TotalUnpaidCountMDV.Length) + TotalUnpaidCountMDV;
                                            MDVUnpaidValue = "0";// MDVUnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumunpaidMDV)));
                                            MDVUnpaidValue = new string('0', 14 - MDVUnpaidValue.Length) + MDVUnpaidValue;
                                            // UnPaid Count
                                            arryRow[t] = TotalUnpaidCountMDV;
                                            t = t + 1;

                                            // UnPaid Sum
                                            arryRow[t] = MDVUnpaidValue;
                                            t = t + 1;

                                            // Filler
                                            arryRow[t] = new string('0', 32);
                                            t = t + 1;
                                            //Reconstruct here
                                            Data = string.Empty;
                                            for (m = 0; m < arryRow.Length; m++)
                                            {
                                                Data = Data +  (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
                                            }
                                            t = 0;
                                            dt.Rows.Add(Data);
                                            //BankID    CRNo    CRAmount    DRNo               DrAmt    MDVNo   MDVAmt  DRUnpaidNo              DRUnpaidAmt         DiscNo  DiscAmt     CrUnpaidNo  CRUnPaidAmt     Data        Date            Currency    TrxType     FileDate
                                            SettlementListingDt.Tables[0].Rows.Add(OldBankID, 0, 0, 0, 0, BRBase.BRBaseConvert.ConvertToInt32(TotalCountMDV), SumMDV, BRBase.BRBaseConvert.ConvertToInt32(TotalUnpaidCountMDV), SumunpaidMDV, 0, 0, 0, 0, Data, WDate, "60", "O", FDate);

                                        //61
                                            arryRow = new Object[8];
                                            t = 0;
                                            RecordType = "09";
                                            // RecordType
                                            arryRow[t] = RecordType;
                                            t = t + 1;

                                            // ToBank
                                            arryRow[t] = OldBankID;
                                            t = t + 1;

                                            // Currency
                                            arryRow[t] = "61";
                                            t = t + 1;

                                            //Presentation
                                            EJdataTable.Tables[0].Rows.Clear();
                                            drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("VOUCHERCODE ='60' AND RETURNCODE = '00' AND TRXTYPE='OC' AND TOBANK " + ToBank + "");
                                            foreach (DataRow dvr in drHeaderFileFormatResult)
                                            {
                                                EJdataTable.Tables[0].ImportRow(dvr);
                                                SumMDV = SumMDV + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                            }
                                            EJdataTable.AcceptChanges();
                                            TotalCountMDV = "0";// TotalCountMDV = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                            TotalCountMDV = new string('0', 6 - TotalCountMDV.Length) + TotalCountMDV;
                                            MDVValue = "0";// MDVValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumMDV)));
                                            MDVValue = new string('0', 14 - MDVValue.Length) + MDVValue;
                                            // Presentments Count
                                            arryRow[t] = TotalCountMDV;
                                            t = t + 1;

                                            // Presentments Sum
                                            arryRow[t] = MDVValue;
                                            t = t + 1;

                                            ////UnPaids
                                            EJdataTable.Tables[0].Rows.Clear();
                                            drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("VOUCHERCODE ='03' AND  RETURNCODE <> '00' AND TRXTYPE='OC' AND TOBANK " + ToBank + "");
                                            foreach (DataRow dvr in drHeaderFileFormatResult)
                                            {
                                                EJdataTable.Tables[0].ImportRow(dvr);
                                                SumunpaidMDV = SumunpaidMDV + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                            }
                                            EJdataTable.AcceptChanges();
                                            TotalUnpaidCountMDV = "0";// = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                            TotalUnpaidCountMDV = new string('0', 6 - TotalUnpaidCountMDV.Length) + TotalUnpaidCountMDV;
                                            MDVUnpaidValue = "0";// MDVUnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumunpaidMDV)));
                                            MDVUnpaidValue = new string('0', 14 - MDVUnpaidValue.Length) + MDVUnpaidValue;
                                            // UnPaid Count
                                            arryRow[t] = TotalUnpaidCountMDV;
                                            t = t + 1;

                                            // UnPaid Sum
                                            arryRow[t] = MDVUnpaidValue;
                                            t = t + 1;

                                            // Filler
                                            arryRow[t] = new string('0', 32);
                                            t = t + 1;
                                            //Reconstruct here
                                            Data = string.Empty;
                                            for (m = 0; m < arryRow.Length; m++)
                                            {
                                                Data = Data +  (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
                                            }
                                            t = 0;
                                            dt.Rows.Add(Data);
                                            //BankID    CRNo    CRAmount    DRNo               DrAmt    MDVNo   MDVAmt  DRUnpaidNo              DRUnpaidAmt         DiscNo  DiscAmt     CrUnpaidNo  CRUnPaidAmt     Data        Date            Currency    TrxType     FileDate
                                            SettlementListingDt.Tables[0].Rows.Add(OldBankID, 0, 0, 0, 0, BRBase.BRBaseConvert.ConvertToInt32(TotalCountMDV), SumMDV, BRBase.BRBaseConvert.ConvertToInt32(TotalUnpaidCountMDV), SumunpaidMDV, 0, 0, 0, 0, Data, WDate, "61", "O", FDate);

                                        //62
                                            arryRow = new Object[8];
                                            t = 0;
                                            RecordType = "09";
                                            // RecordType
                                            arryRow[t] = RecordType;
                                            t = t + 1;

                                            // ToBank
                                            arryRow[t] = OldBankID;
                                            t = t + 1;

                                            // Currency
                                            arryRow[t] = "62";
                                            t = t + 1;

                                            //Presentation
                                            EJdataTable.Tables[0].Rows.Clear();
                                            drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("VOUCHERCODE ='60' AND RETURNCODE = '00' AND TRXTYPE='OC' AND TOBANK " + ToBank + "");
                                            foreach (DataRow dvr in drHeaderFileFormatResult)
                                            {
                                                EJdataTable.Tables[0].ImportRow(dvr);
                                                SumMDV = SumMDV + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                            }
                                            EJdataTable.AcceptChanges();
                                            TotalCountMDV = "0";// TotalCountMDV = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                            TotalCountMDV = new string('0', 6 - TotalCountMDV.Length) + TotalCountMDV;
                                            MDVValue = "0";// MDVValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumMDV)));
                                            MDVValue = new string('0', 14 - MDVValue.Length) + MDVValue;
                                            // Presentments Count
                                            arryRow[t] = TotalCountMDV;
                                            t = t + 1;

                                            // Presentments Sum
                                            arryRow[t] = MDVValue;
                                            t = t + 1;

                                            ////UnPaids
                                            EJdataTable.Tables[0].Rows.Clear();
                                            drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("VOUCHERCODE ='03' AND  RETURNCODE <> '00' AND TRXTYPE='OC' AND TOBANK " + ToBank + "");
                                            foreach (DataRow dvr in drHeaderFileFormatResult)
                                            {
                                                EJdataTable.Tables[0].ImportRow(dvr);
                                                SumunpaidMDV = SumunpaidMDV + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                            }
                                            EJdataTable.AcceptChanges();
                                            TotalUnpaidCountMDV = "0";// = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                            TotalUnpaidCountMDV = new string('0', 6 - TotalUnpaidCountMDV.Length) + TotalUnpaidCountMDV;
                                            MDVUnpaidValue = "0";// MDVUnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumunpaidMDV)));
                                            MDVUnpaidValue = new string('0', 14 - MDVUnpaidValue.Length) + MDVUnpaidValue;
                                            // UnPaid Count
                                            arryRow[t] = TotalUnpaidCountMDV;
                                            t = t + 1;

                                            // UnPaid Sum
                                            arryRow[t] = MDVUnpaidValue;
                                            t = t + 1;

                                            // Filler
                                            arryRow[t] = new string('0', 32);
                                            t = t + 1;
                                            //Reconstruct here
                                            Data = string.Empty;
                                            for (m = 0; m < arryRow.Length; m++)
                                            {
                                                Data = Data +  (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
                                            }
                                            t = 0;
                                            dt.Rows.Add(Data);
                                            //BankID    CRNo    CRAmount    DRNo               DrAmt    MDVNo   MDVAmt  DRUnpaidNo              DRUnpaidAmt         DiscNo  DiscAmt     CrUnpaidNo  CRUnPaidAmt     Data        Date            Currency    TrxType     FileDate
                                            SettlementListingDt.Tables[0].Rows.Add(OldBankID, 0, 0, 0, 0, BRBase.BRBaseConvert.ConvertToInt32(TotalCountMDV), SumMDV, BRBase.BRBaseConvert.ConvertToInt32(TotalUnpaidCountMDV), SumunpaidMDV, 0, 0, 0, 0, Data, WDate, "62", "O", FDate);



                                    SumCheques = 0;
                                    SumChq = string.Empty;
                                    SumCredits = 0;
                                    SumCr = string.Empty;
                                    SumDebits = 0;
                                    SumDr = string.Empty;
                                    UnpaidValue = "0";
                                    SumUnpaidCheques = 0;
                                    SumUnChq = string.Empty;
                                    SumunpaidCredits = 0;
                                    SumUnCr = string.Empty;
                                    SumUnpaidDebits = 0;
                                    SumUnDr = string.Empty;
                                    SumDiscCheques = 0;
                                    SumDiscChq = string.Empty;
                                    TotalCountCheques = "0";
                                    TotalCountDebits = "0";
                                    TotalCountCredits = "0";
                                    TotalUnpaidCountCheques = "0";
                                    TotalUnpaidCountDebits = "0";
                                    TotalUnpaidCountCredits = "0";
                                    TotalDiscCountCheques = "0";
                                    MDVValue = "0";
                                    TotalCountMDV = "0";
                                    SumunpaidMDV = 0;
                                    SumMDV = 0;
                                    TotalUnpaidCountMDV = "0";
                                    MDVUnpaidValue = "0";
                                }
                            }
                            else
                            {
                                for (Int32 n = 0; n < dsClearingBanks.Tables[0].Rows.Count; n++)
                                {
                                    
                                    ToBank = dsClearingBanks.Tables[0].Rows[n]["BankID"].ToString().Trim();
                                    string OldBankID = ToBank;
                                    BRBase.BRDataSet dsClearingThroughThisBank = new BRBase.BRDataSet();


                                    try
                                    {
                                        OutClearingFile.ClearingUniversalMethod(usrInfo, "p_OutClearingThroughBank", out dsClearingThroughThisBank, BRBase.BRModule.GenerateClearingFile, GetConnection(), new object[] { "BrDataSet" }, new object[] { ToBank.ToString() });
                                    }
                                    catch (Exception ex)
                                    {
                                        string AppendErrorMessage = "Error Message 911:" + ex.ToString() + Environment.NewLine + "Date" + ":" + DateTime.Now + Environment.NewLine + "--------------------------" + Environment.NewLine;
                                        System.IO.File.AppendAllText("C:\\ClearingFiles\\" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + ".txt", AppendErrorMessage);
                                    }
                                    if (dsClearingThroughThisBank.Tables.Count > 0)
                                    {
                                        if (dsClearingThroughThisBank.Tables[0].Rows.Count > 1)
                                        {
                                            ToBank = string.Empty;
                                            ToBank = "IN ('";
                                            for (Int32 f = 0; f < dsClearingThroughThisBank.Tables[0].Rows.Count; f++) //BankWise
                                            {

                                                ToBank = ToBank + dsClearingThroughThisBank.Tables[0].Rows[f]["BankID"].ToString() + "','";
                                            }
                                            ToBank = ToBank.Substring(0, ToBank.LastIndexOf(","));
                                            ToBank = ToBank + ")";
                                        }
                                        else
                                        {
                                            ToBank = "IN ('" + ToBank + "')";
                                        }

                                    }
                                    arryRow = new Object[10];
                                    RecordType = "06";
                                    // RecordType
                                    arryRow[t] = RecordType;
                                    t = t + 1;

                                    // ToBank
                                    arryRow[t] = OldBankID;
                                    t = t + 1;

                                    // Currency
                                    arryRow[t] = Curr;
                                    t = t + 1;

                                    // Presentments
                                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("(SERIALNUMBER IS NOT NULL OR SERIALNUMBER <> 0) AND CHEQUEDIGIT IS NOT NULL  AND RETURNCODE IN ('00','17') AND VOUCHERCODE NOT IN('02','40') AND TOBANK " + ToBank + " AND TRXTYPE='OC'");
                                    foreach (DataRow dvr in drHeaderFileFormatResult)
                                    {
                                        EJdataTable.Tables[0].ImportRow(dvr);
                                        SumCheques = SumCheques + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                    }
                                    EJdataTable.AcceptChanges();
                                    TotalCountCheques = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                    TotalCountCheques = new string('0', 6 - TotalCountCheques.Length) + TotalCountCheques;
                                    // Pramod
                                    //Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumCheques)));
                                    if (Curr == "00")
                                    {
                                        Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumCheques)));
                                    }
                                    else
                                    {
                                        Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumCheques),"60"));
                                    }
                                    Value = new string('0', 14 - Value.Length) + Value;
                                    EJdataTable.Tables[0].Rows.Clear();

                                    // Presentments Count
                                    arryRow[t] = TotalCountCheques;
                                    t = t + 1;

                                    // Presentments Sum
                                    arryRow[t] = Value;
                                    t = t + 1;

                                    // Discrepancy
                                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select(" CHEQUEDIGIT IS NOT NULL AND RETURNCODE IN ('00','17') AND VOUCHERCODE IN ('02') AND TOBANK " + ToBank + "  AND TRXTYPE='OC'");
                                    foreach (DataRow dvr in drHeaderFileFormatResult)
                                    {
                                        EJdataTable.Tables[0].ImportRow(dvr);
                                        SumDiscCheques = SumDiscCheques + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                    }
                                    EJdataTable.AcceptChanges();
                                    TotalDiscCountCheques = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                    TotalDiscCountCheques = new string('0', 6 - TotalDiscCountCheques.Length) + TotalDiscCountCheques;
                                    // Pramod
                                    //SumDiscChq = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumDiscCheques)));
                                    if (Curr == "00")
                                    {
                                        SumDiscChq = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumDiscCheques)));
                                    }
                                    else
                                    {
                                        SumDiscChq = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumDiscCheques),"60"));
                                    }
                                    SumDiscChq = new string('0', 13 - SumDiscChq.Length) + SumDiscChq;
                                    EJdataTable.Tables[0].Rows.Clear();

                                    // Discrepancy Count Discrepancy
                                    arryRow[t] = TotalDiscCountCheques;
                                    t = t + 1;

                                    // Discrepancy Sum Discrepancy
                                    arryRow[t] = "+" + SumDiscChq;
                                    t = t + 1;

                                    // Unpaid
                                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("CHEQUEDIGIT IS NOT NULL AND RETURNCODE NOT IN ('00','17') AND VOUCHERCODE NOT IN('02','04','05','40') AND TOBANK " + ToBank + " AND TRXTYPE='OC'");
                                    foreach (DataRow dvr in drHeaderFileFormatResult)
                                    {
                                        EJdataTable.Tables[0].ImportRow(dvr);
                                        SumUnpaidCheques = SumUnpaidCheques + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                    }
                                    EJdataTable.AcceptChanges();
                                    TotalUnpaidCountCheques = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                    TotalUnpaidCountCheques = new string('0', 6 - TotalUnpaidCountCheques.Length) + TotalUnpaidCountCheques;
                                    //Pramod
                                    //UnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumUnpaidCheques)));
                                    if (Curr == "00")
                                    {
                                        UnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumUnpaidCheques)));
                                    }
                                    else
                                    {
                                        UnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumUnpaidCheques),"60"));
                                    }
                                    UnpaidValue = new string('0', 14 - UnpaidValue.Length) + UnpaidValue;
                                    EJdataTable.Tables[0].Rows.Clear();

                                    // Unpaid Count Discrepancy
                                    arryRow[t] = TotalUnpaidCountCheques;
                                    t = t + 1;

                                    // Unpaid Sum Discrepancy
                                    arryRow[t] = UnpaidValue;
                                    t = t + 1;

                                    // Filler
                                    arryRow[t] = new string('0', 12);
                                    t = t + 1;

                                    //Reconstruct here
                                    Data = string.Empty;
                                    for (m = 0; m < arryRow.Length; m++)
                                    {
                                        Data = Data +  (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
                                    }
                                    t = 0;
                                    dt.Rows.Add(Data);
                                    //BankID    CRNo    CRAmount    DRNo               DrAmt    MDVNo   MDVAmt  DRUnpaidNo              DRUnpaidAmt         DiscNo  DiscAmt     CrUnpaidNo  CRUnPaidAmt     Data        Date            Currency    TrxType     FileDate
                                    SettlementListingDt.Tables[0].Rows.Add(OldBankID, 0, 0, BRBase.BRBaseConvert.ConvertToInt32(TotalCountCheques), SumCheques, 0, 0, BRBase.BRBaseConvert.ConvertToInt32(TotalUnpaidCountCheques), SumUnpaidCheques, 0, 0, 0, 0, Data, WDate, "0", "O", FDate);


                                    // 07
                                    arryRow = new Object[8];
                                    t = 0;
                                    RecordType = "07";
                                    // RecordType
                                    arryRow[t] = RecordType;
                                    t = t + 1;

                                    // ToBank
                                    arryRow[t] = OldBankID;
                                    t = t + 1;

                                    // Currency
                                    arryRow[t] = Curr;
                                    t = t + 1;

                                    //Presentments
                                    EJdataTable.Tables[0].Rows.Clear();
                                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("VOUCHERCODE ='40' AND RETURNCODE = '00' AND TOBANK " + ToBank + "  AND isNull(RECORDTYPE,'00') = '00'");
                                    foreach (DataRow dvr in drHeaderFileFormatResult)
                                    {
                                        EJdataTable.Tables[0].ImportRow(dvr);
                                        SumDebits = SumDebits + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                    }
                                    EJdataTable.AcceptChanges();
                                    TotalCountDebits = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                    TotalCountDebits = new string('0', 6 - TotalCountDebits.Length) + TotalCountDebits;
                                    //Pramod
                                    //Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumDebits)));
                                    if (Curr == "00")
                                    {
                                        Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumDebits)));
                                    }
                                    else
                                    {
                                        Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumDebits),"60"));
                                    }
                                    Value = new string('0', 14 - Value.Length) + Value;
                                    EJdataTable.Tables[0].Rows.Clear();

                                    // Presentments Count
                                    arryRow[t] = TotalCountDebits;
                                    t = t + 1;

                                    // Presentments Sum
                                    arryRow[t] = Value;
                                    t = t + 1;

                                    //UnPaid
                                    EJdataTable.Tables[0].Rows.Clear();
                                    drHeaderFileFormatResult = null;
                                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("VOUCHERCODE ='40' AND RETURNCODE NOT IN ('00','17') AND TOBANK " + ToBank + " AND isnull(RECORDTYPE,'00') = '00'");
                                    foreach (DataRow dvr in drHeaderFileFormatResult)
                                    {
                                        EJdataTable.Tables[0].ImportRow(dvr);
                                        SumUnpaidDebits = SumUnpaidDebits + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                    }
                                    EJdataTable.AcceptChanges();
                                    TotalUnpaidCountDebits = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                    TotalUnpaidCountDebits = new string('0', 6 - TotalUnpaidCountDebits.Length) + TotalUnpaidCountDebits;
                                    // Pramod
                                    //UnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumUnpaidDebits)));
                                    if (Curr == "00")
                                    {
                                        UnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumUnpaidDebits)));
                                    }
                                    else
                                    {
                                        UnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumUnpaidDebits),"60"));
                                    }
                                    UnpaidValue = new string('0', 14 - UnpaidValue.Length) + UnpaidValue;
                                    EJdataTable.Tables[0].Rows.Clear();

                                    // UnPaid Count
                                    arryRow[t] = TotalUnpaidCountDebits;
                                    t = t + 1;

                                    // UnPaid Sum
                                    arryRow[t] = UnpaidValue;
                                    t = t + 1;

                                    // Filler
                                    arryRow[t] = new string('0', 32);
                                    t = t + 1;

                                    //Reconstruct here
                                    Data = string.Empty;
                                    for (m = 0; m < arryRow.Length; m++)
                                    {
                                        Data = Data +  (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
                                    }
                                    t = 0;
                                    dt.Rows.Add(Data);
                                    //BankID    CRNo    CRAmount    DRNo               DrAmt    MDVNo   MDVAmt  DRUnpaidNo              DRUnpaidAmt         DiscNo  DiscAmt     CrUnpaidNo  CRUnPaidAmt     Data        Date            Currency    TrxType     FileDate
                                    SettlementListingDt.Tables[0].Rows.Add(OldBankID, 0, 0, BRBase.BRBaseConvert.ConvertToInt32(TotalCountDebits), SumDebits, 0, 0, BRBase.BRBaseConvert.ConvertToInt32(TotalUnpaidCountDebits), SumUnpaidDebits, 0, 0, 0, 0, Data, WDate, "0", "O", FDate);


                                    // 08
                                    arryRow = new Object[8];
                                    t = 0;
                                    RecordType = "08";
                                    // RecordType
                                    arryRow[t] = RecordType;
                                    t = t + 1;

                                    // ToBank
                                    arryRow[t] = OldBankID;
                                    t = t + 1;

                                    // Currency
                                    arryRow[t] = Curr;
                                    t = t + 1;

                                    //Presentation
                                    EJdataTable.Tables[0].Rows.Clear();
                                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select(" RETURNCODE IN('00','90','97') AND TOBANK " + ToBank + " AND VOUCHERCODE <>'40' AND TRXTYPE='OD'");
                                    foreach (DataRow dvr in drHeaderFileFormatResult)
                                    {
                                        EJdataTable.Tables[0].ImportRow(dvr);
                                        SumCredits = SumCredits + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                    }
                                    EJdataTable.AcceptChanges();
                                    TotalCountCredits = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                    TotalCountCredits = new string('0', 6 - TotalCountCredits.Length) + TotalCountCredits;
                                    //Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumCredits)));
                                    if (Curr == "00")
                                    {
                                        Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumCredits)));
                                    }
                                    else
                                    {
                                        Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumCredits),"60"));
                                    }
                                    Value = new string('0', 14 - Value.Length) + Value;
                                    // Presentments Count
                                    arryRow[t] = TotalCountCredits;
                                    t = t + 1;

                                    // Presentments Sum
                                    arryRow[t] = Value;
                                    t = t + 1;

                                    //UnPaids
                                    EJdataTable.Tables[0].Rows.Clear();
                                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("RETURNCODE NOT IN('00','90','97') AND TOBANK " + ToBank + " AND VOUCHERCODE <>'40'  AND TRXTYPE='OD'");
                                    foreach (DataRow dvr in drHeaderFileFormatResult)
                                    {
                                        EJdataTable.Tables[0].ImportRow(dvr);
                                        SumunpaidCredits = SumunpaidCredits + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                    }
                                    EJdataTable.AcceptChanges();
                                    TotalUnpaidCountCredits = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                    TotalUnpaidCountCredits = new string('0', 6 - TotalUnpaidCountCredits.Length) + TotalUnpaidCountCredits;
                                    //Pramod
                                    //UnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumunpaidCredits)));
                                    if (Curr == "00")
                                    {
                                        UnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumunpaidCredits)));
                                    }
                                    else
                                    {
                                        UnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumunpaidCredits),"60"));
                                    }
                                    UnpaidValue = new string('0', 14 - UnpaidValue.Length) + UnpaidValue;
                                    // UnPaid Count
                                    arryRow[t] = TotalUnpaidCountCredits;
                                    t = t + 1;

                                    // UnPaid Sum
                                    arryRow[t] = UnpaidValue;
                                    t = t + 1;

                                    // Filler
                                    arryRow[t] = new string('0', 32);
                                    t = t + 1;

                                    //Reconstruct here
                                    Data = string.Empty;
                                    for (m = 0; m < arryRow.Length; m++)
                                    {
                                        Data = Data +  (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
                                    }
                                    t = 0;
                                    dt.Rows.Add(Data);

                                    //BankID    CRNo    CRAmount    DRNo               DrAmt    MDVNo   MDVAmt  DRUnpaidNo              DRUnpaidAmt         DiscNo  DiscAmt     CrUnpaidNo  CRUnPaidAmt     Data        Date            Currency    TrxType     FileDate
                                    SettlementListingDt.Tables[0].Rows.Add(OldBankID, BRBase.BRBaseConvert.ConvertToInt32(TotalCountCredits), SumCredits, 0, 0, 0, 0, 0, 0, 0, 0, BRBase.BRBaseConvert.ConvertToInt32(TotalUnpaidCountCredits), SumunpaidCredits, Data, WDate, "0", "O", FDate);



                                    // 09
                                    arryRow = new Object[8];
                                    t = 0;
                                    RecordType = "09";
                                    // RecordType
                                    arryRow[t] = RecordType;
                                    t = t + 1;

                                    // ToBank
                                    arryRow[t] = OldBankID;
                                    t = t + 1;

                                    // Currency
                                    arryRow[t] = Curr;
                                    t = t + 1;

                                    //Presentation
                                    EJdataTable.Tables[0].Rows.Clear();
                                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("VOUCHERCODE ='03' AND RETURNCODE = '00' AND TRXTYPE='OC' AND TOBANK " + ToBank + " ");
                                    foreach (DataRow dvr in drHeaderFileFormatResult)
                                    {
                                        EJdataTable.Tables[0].ImportRow(dvr);
                                        SumMDV = SumMDV + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                    }
                                    EJdataTable.AcceptChanges();
                                    TotalCountMDV = "0";// TotalCountMDV = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                    TotalCountMDV = new string('0', 6 - TotalCountMDV.Length) + TotalCountMDV;
                                    MDVValue = "0";// MDVValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumMDV)));
                                    MDVValue = new string('0', 14 - MDVValue.Length) + MDVValue;
                                    // Presentments Count
                                    arryRow[t] = TotalCountMDV;
                                    t = t + 1;

                                    // Presentments Sum
                                    arryRow[t] = MDVValue;
                                    t = t + 1;

                                    ////UnPaids
                                    EJdataTable.Tables[0].Rows.Clear();
                                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("VOUCHERCODE ='03' AND  RETURNCODE <> '00' AND TRXTYPE='OC' AND TOBANK " + ToBank + " ");
                                    foreach (DataRow dvr in drHeaderFileFormatResult)
                                    {
                                        EJdataTable.Tables[0].ImportRow(dvr);
                                        SumunpaidMDV = SumunpaidMDV + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                    }
                                    EJdataTable.AcceptChanges();
                                    TotalUnpaidCountMDV = "0";// = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                    TotalUnpaidCountMDV = new string('0', 6 - TotalUnpaidCountMDV.Length) + TotalUnpaidCountMDV;
                                    MDVUnpaidValue = "0";// MDVUnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumunpaidMDV)));
                                    MDVUnpaidValue = new string('0', 14 - MDVUnpaidValue.Length) + MDVUnpaidValue;
                                    // UnPaid Count
                                    arryRow[t] = TotalUnpaidCountMDV;
                                    t = t + 1;

                                    // UnPaid Sum
                                    arryRow[t] = MDVUnpaidValue;
                                    t = t + 1;

                                    // Filler
                                    arryRow[t] = new string('0', 32);
                                    t = t + 1;

                                    //Reconstruct here
                                    Data = string.Empty;
                                    for (m = 0; m < arryRow.Length; m++)
                                    {
                                        Data = Data +  (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
                                    }
                                    t = 0;
                                    dt.Rows.Add(Data);

                                    //BankID    CRNo    CRAmount    DRNo               DrAmt    MDVNo   MDVAmt  DRUnpaidNo              DRUnpaidAmt         DiscNo  DiscAmt     CrUnpaidNo  CRUnPaidAmt     Data        Date            Currency    TrxType     FileDate
                                    SettlementListingDt.Tables[0].Rows.Add(OldBankID, 0, 0, 0, 0, BRBase.BRBaseConvert.ConvertToInt32(TotalCountMDV), SumMDV, BRBase.BRBaseConvert.ConvertToInt32(TotalUnpaidCountMDV), SumunpaidMDV, 0, 0, 0, 0, Data, WDate, "0", "O", FDate);


                                    SumCheques = 0;
                                    SumChq = string.Empty;
                                    SumCredits = 0;
                                    SumCr = string.Empty;
                                    SumDebits = 0;
                                    SumDr = string.Empty;
                                    UnpaidValue = "0";
                                    SumUnpaidCheques = 0;
                                    SumUnChq = string.Empty;
                                    SumunpaidCredits = 0;
                                    SumUnCr = string.Empty;
                                    SumUnpaidDebits = 0;
                                    SumUnDr = string.Empty;
                                    SumDiscCheques = 0;
                                    SumDiscChq = string.Empty;
                                    TotalCountCheques = "0";
                                    TotalCountDebits = "0";
                                    TotalCountCredits = "0";
                                    TotalUnpaidCountCheques = "0";
                                    TotalUnpaidCountDebits = "0";
                                    TotalUnpaidCountCredits = "0";
                                    TotalDiscCountCheques = "0";
                                    MDVValue = "0";
                                    TotalCountMDV = "0";
                                    SumunpaidMDV = 0;
                                    SumMDV = 0;
                                    TotalUnpaidCountMDV = "0";
                                    MDVUnpaidValue = "0";
                                }
                            }
                            break;
                    }
                }
            }
            catch (Exception e)
            {

            }
            if (SettlementListingDt.Tables[0].Rows.Count > 0)
            {
                //Save into the Database
                SettlementListingDt.Tables[0].Columns.Add("DetailRecords", typeof(string));
                DataSet dsTemp = new DataSet();
                dsTemp = (DataSet)SettlementListingDt.Copy();
                dsTemp.Relations.Clear();
                DataRow drSettlementListingDt = SettlementListingDt.Tables[0].Rows[0];
                drSettlementListingDt["DetailRecords"] =  GetXmlTable(dsTemp);

                try
                {
                    using (IDbConnection connection = conn)
                    {
                        using (IDbTransaction trans = connection.BeginTransaction())
                        {
                            IDBHelper intfDBHelper = DBClient.GetDBHelper(usrInfo);
                            intfDBHelper.ExecuteScalarTypedParams(trans, "p_AddOutSettlement", drSettlementListingDt);
                            trans.Commit();
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
            
            return dt;
        }
        public static bool GenerateEJsUG(string VoucherType, DS_trxClearing dstrxClearing, DS_ClearingFileFormat dsClearingFileFormat, string ToBankID, string Currency, BRBase.BRDataSet dsWithImages, DateTime WorkingDate, BRBase.UserInfo usrInfo, out DataTable SemiCompiledDataToBeWritten, String Banks,IDbConnection conn)
        {

            
            Int16 j, m;
            Int32 t;
            string ControlVoucherType = string.Empty;
            string NewValue = string.Empty;
            string Value = string.Empty;
            string FieldNm = string.Empty;
            string ImageID = string.Empty;
            bool Status = false;
            string ClearingCenter = "47";
            string curr = string.Empty;
            string sortOrder = " Start ASC";
            SemiCompiledDataToBeWritten = new DataTable();
            SemiCompiledDataToBeWritten.Columns.Add("TrxRowID", typeof(string));
            SemiCompiledDataToBeWritten.Columns.Add("text", typeof(string));
            SemiCompiledDataToBeWritten.Columns.Add("FileName", typeof(string));
            SemiCompiledDataToBeWritten.Columns.Add("ImageID", typeof(string));
            SemiCompiledDataToBeWritten.Columns.Add("fcy", typeof(string));
            string Data = string.Empty;
            DS_ClearingFileFormat WorkingDataTable = new DS_ClearingFileFormat();
            DS_trxClearing RejectDt = new DS_trxClearing();
            DS_trxClearing EJdataTable = new DS_trxClearing();
            DataTable EJdt = new DataTable();
            Int32 IsFcy = 0;
            Int64  StrTrxRowID = 0;
            ArrayList arr = new ArrayList();
            BRBase.BRDataSet ds4Images = new BRBase.BRDataSet();
            string dd = new string('0', 2 - BRBase.BRBaseConvert.ConvertToString(WorkingDate.Day).ToString().Length) + BRBase.BRBaseConvert.ConvertToString(WorkingDate.Day);
            string mm = new string('0', 2 - BRBase.BRBaseConvert.ConvertToString(WorkingDate.Month).Length) + BRBase.BRBaseConvert.ConvertToString(WorkingDate.Month);
            string yyyy = BRBase.BRBaseConvert.ConvertToString(WorkingDate.Year);
            string ddmmmyyyy = dd + mm + yyyy;
            ds4Images = (BRBase.BRDataSet)dsWithImages.Clone();
            EJdataTable = (DS_trxClearing)dstrxClearing.Clone();/////////////////////////////////////
            DataRow[] drHeaderFileFormatResult = null;
            string CurrType = string.Empty;
            t = 0;
            if (Currency == "UGX")
            {
                CurrType = " AND currencyID ='UGX' AND TrxType ='OC'";
                curr = "00";
            }
            else
            {
                switch (Currency.ToUpper())
                {
                    case "USD":
                        CurrType = " AND currencyID ='USD' AND TrxType ='OC'";
                        curr = "22";
                        IsFcy = 1;
                        break;
                    case "GBP":
                        CurrType = " AND currencyID ='GBP' AND TrxType ='OC'";
                        curr = "24";
                        IsFcy = 1;
                        break;
                    case "EUR":
                        CurrType = " AND currencyID ='EUR' AND TrxType ='OC'";
                        curr = "23";
                        IsFcy = 1;
                        break;
                    case "KES":
                        CurrType = " AND currencyID ='KES' AND TrxType ='OC'";
                        curr = "25";
                        IsFcy = 1;
                        break;
                }
                //CurrType = " AND currencyID = '" + Currency + "' AND TrxType ='OC'";
                
            }
            //MessageBox.Show(" About to..");
            drHeaderFileFormatResult = dsClearingFileFormat.Tables[0].Select("FileType='EJ' AND RecordType ='EJ'");//, sortOrder);
            foreach (DataRow dvr in drHeaderFileFormatResult)
            {
                WorkingDataTable.Tables[0].ImportRow(dvr);
            }
            WorkingDataTable.AcceptChanges();
            Object[] arryRow = new Object[WorkingDataTable.Tables[0].Rows.Count + 2];
            switch (VoucherType.ToString().ToUpper())
            {
                case "BCV":  //Presentments 
                    //MessageBox.Show(" BCV");
                    //MessageBox.Show(CurrType);
                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("ReturnCode IN ('00','17') AND VOUCHERCODE NOT IN ('03','40') AND ToBank " + Banks + " " + CurrType);// + "  AND SUBSTRING(FROMBRANCH,3,2) ='" + ClearingCenter + "' ");//
                    foreach (DataRow dvr in drHeaderFileFormatResult)
                    {
                        EJdataTable.Tables[0].ImportRow(dvr);
                    }
                    EJdataTable.AcceptChanges();
                    //MessageBox.Show(EJdataTable.Tables[0].Rows.Count.ToString());
                    break;
                case "UCV":  //Unpaids 
                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("ReturnCode NOT IN ('00','17') AND VOUCHERCODE NOT IN ('40') AND ToBank " + Banks + " " + CurrType);// + "  AND SUBSTRING(FROMBRANCH,3,2) ='" + ClearingCenter + "' ");
                    foreach (DataRow dvr in drHeaderFileFormatResult)
                    {
                        EJdataTable.Tables[0].ImportRow(dvr);
                    }
                    EJdataTable.AcceptChanges();
                    break;
                case "MDV":  //Manual Debit Voucher
                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("ReturnCode IN ('00','17') AND VOUCHERCODE ='03' AND ToBank " + Banks + " " + CurrType);// + " AND SUBSTRING(FROMBRANCH,3,2) ='" + ClearingCenter + "' ");
                    foreach (DataRow dvr in drHeaderFileFormatResult)
                    {
                        EJdataTable.Tables[0].ImportRow(dvr);
                    }
                    EJdataTable.AcceptChanges();
                    break;
            }
            Int32 q = 0;
            foreach (DataRow dr in EJdataTable.Tables[0].Rows)
            {
                arryRow = new Object[16];
                t = 0;
                q = q + 1;
                if (q == 99)//This is meant to take care of the BCV or TCV Batching of a 100
                {
                    break;
                }
                dr["Generated"] = true;
                foreach (DataColumn Col in WorkingDataTable.Tables[0].Columns)
                {
                    switch (Col.ColumnName.ToUpper())
                    {
                        case "FIELDNAME":
                            try
                            {
                                //MessageBox.Show("imeanza Kazi");
                                for (j = 0; j < WorkingDataTable.Tables[0].Rows.Count; j++)
                                {
                                    if (conn.State != ConnectionState.Open)
                                    {
                                        conn = GetConnection();
                                    }
                                    //Get the Required Length
                                    FieldNm = string.Empty;
                                    FieldNm = WorkingDataTable.Tables[0].Rows[j][Col.ColumnName].ToString();
                                    //MessageBox.Show(FieldNm);
                                    Int32 FileFormatValue = BRBase.BRBaseConvert.ConvertToInt32(WorkingDataTable.Tables[0].Rows[j]["Length"].ToString());
                                    bool FileFormatValueMandatoryLength = WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString() == "" ? true : BRBase.BRBaseConvert.ConvertToBoolean(WorkingDataTable.Tables[0].Rows[j]["IsLengthMandatoryFieldSize"].ToString());
                                    string Filler = WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString() == "" ? "0" : WorkingDataTable.Tables[0].Rows[j]["Filler"].ToString();
                                    StrTrxRowID = BRBase.BRBaseConvert.ConvertToInt64(dr["TrxRowID"].ToString().Trim());
                                    //Check the datatable, whether the column that we are presently in, its value meets the required length
                                    //MessageBox.Show(FieldNm.ToString());
                                    switch (FieldNm.ToString().ToUpper().Trim())
                                    {
                                        case "RETURNCODE":
                                        case "VOUCHERCODE":
                                        case "VALUE":
                                            //System.Windows.Forms.MessageBox.Show(FieldNm);
                                            Int32 ValueLength = BRBase.BRBaseConvert.ConvertToInt32((dr[FieldNm].ToString().Trim()).ToString().Length);
                                            Value = BRBase.BRBaseConvert.ConvertToString(dr[FieldNm].ToString().Trim());
                                            if (ValueLength > FileFormatValue)
                                            {
                                                dr["Status"] = "R";
                                                EJdataTable.AcceptChanges();
                                                goto JustRejected;
                                            }
                                            else
                                            {
                                               
                                                Value = BRBase.BRBaseConvert.ConvertToString(dr[FieldNm].ToString().Trim());
                                                NewValue = Value;
                                                if (FileFormatValueMandatoryLength == true)
                                                {
                                                    
                                                    if (ValueLength != FileFormatValue)
                                                    {
                                                        if (FieldNm == "VALUE")
                                                        {
                                                            Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(Value), BRBase.BRBaseConvert.ConvertToString(dr["VoucherCode"])));
                                                            //Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(Value)));
                                                            if (Value.ToString().Contains("."))
                                                            {
                                                                Value = Value.Substring(0, Value.IndexOf("."));
                                                            }
                                                        }
                                                        //First Fill in the required Characters.
                                                       
                                                        NewValue = new string(BRBase.BRBaseConvert.ConvertToChar(Filler), (BRBase.BRBaseConvert.ConvertToInt32(FileFormatValue) - BRBase.BRBaseConvert.ConvertToInt32(Value.Length))) + Value;
                                                        //System.Windows.Forms.MessageBox.Show(FieldNm + " : Old Value >:" + Value + " : Value length >: " + ValueLength.ToString() + " : Filler length >: " + Filler + " : FormatValue >:" + FileFormatValue + " : NewValue >: " + NewValue.ToString() );
                                                        Int32 NewValueLength = NewValue.Length;
                                                        if (NewValueLength != FileFormatValue)
                                                        {//Not all do have fillers, so if the do not meet the requirement, Flag them for reject.
                                                            dr["Status"] = "R";
                                                            EJdataTable.AcceptChanges();
                                                            goto JustRejected;
                                                        }
                                                    }
                                                    arryRow[t] = NewValue;
                                                    t = t + 1;
                                                }
                                                else
                                                {
                                                    arryRow[t] = "";
                                                    t = t + 1;
                                                }
                                            }
                                            break;
                                        case "CHEQUEDIGIT":
                                            //System.Windows.Forms.MessageBox.Show(FieldNm);
                                            ValueLength = BRBase.BRBaseConvert.ConvertToInt32((dr[FieldNm].ToString().Trim()).ToString().Length);
                                            Value = BRBase.BRBaseConvert.ConvertToString(dr[FieldNm].ToString().Trim());
                                            //System.Windows.Forms.MessageBox.Show(ValueLength + " > " + FileFormatValue.ToString());
                                            
                                            if (ValueLength > FileFormatValue)
                                            {
                                                //System.Windows.Forms.MessageBox.Show("Imechapa");
                                                dr["Status"] = "R";
                                                EJdataTable.AcceptChanges();
                                                goto JustRejected;
                                            }
                                            else
                                            {
                                                
                                                Value = BRBase.BRBaseConvert.ConvertToString(dr[FieldNm].ToString().Trim());
                                                NewValue = Value;
                                                if (FileFormatValueMandatoryLength == true)
                                                {
                                                    //System.Windows.Forms.MessageBox.Show("Imeingia hapa");
                                                    if (ValueLength != FileFormatValue)
                                                    {
                                                        NewValue = new string(BRBase.BRBaseConvert.ConvertToChar(Filler), (BRBase.BRBaseConvert.ConvertToInt32(FileFormatValue) - BRBase.BRBaseConvert.ConvertToInt32(Value.Length))) + Value;
                                                    }
                                                    NewValue = new string(BRBase.BRBaseConvert.ConvertToChar(Filler), (BRBase.BRBaseConvert.ConvertToInt32(FileFormatValue) - BRBase.BRBaseConvert.ConvertToInt32(Value.Length))) + Value;
                                                    //System.Windows.Forms.MessageBox.Show("Kuweka sasa >" + t.ToString());
                                                    arryRow[t] = NewValue;
                                                    t = t + 1;
                                                    //System.Windows.Forms.MessageBox.Show("inatoka ");
                                                }
                                                else
                                                {
                                                    //System.Windows.Forms.MessageBox.Show("Imechapa 2");
                                                    arryRow[t] = "";
                                                    t = t + 1;
                                                }
                                            }
                                            break;
                                        case "TOBANK":
                                            //System.Windows.Forms.MessageBox.Show(FieldNm);
                                            if (VoucherType.ToString().ToUpper() == "UCV")
                                            {
                                                ValueLength = BRBase.BRBaseConvert.ConvertToInt32((usrInfo.strBank.ToString().Trim()).ToString().Length);
                                                Value = BRBase.BRBaseConvert.ConvertToString(usrInfo.strBank.ToString().Trim());
                                            }
                                            else
                                            {
                                                ValueLength = BRBase.BRBaseConvert.ConvertToInt32((dr[FieldNm].ToString().Trim()).ToString().Length);
                                                Value = BRBase.BRBaseConvert.ConvertToString(dr[FieldNm].ToString().Trim());
                                            }
                                            if (ValueLength > FileFormatValue)
                                            {
                                                dr["Status"] = "R";
                                                EJdataTable.AcceptChanges();
                                                goto JustRejected;
                                            }
                                            else
                                            {
                                                NewValue = Value;
                                                if (FileFormatValueMandatoryLength == true)
                                                {
                                                    if (ValueLength != FileFormatValue)
                                                    {
                                                        if (FieldNm == "VALUE")
                                                        {
                                                            //Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(Value)));
                                                            Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(Value), BRBase.BRBaseConvert.ConvertToString(dr["VoucherCode"])));
                                                            if (Value.ToString().Contains("."))
                                                            {
                                                                Value = Value.Substring(0, Value.IndexOf("."));
                                                            }
                                                        }
                                                        //First Fill in the required Characters.
                                                        NewValue = new string(BRBase.BRBaseConvert.ConvertToChar(Filler), (BRBase.BRBaseConvert.ConvertToInt32(FileFormatValue) - BRBase.BRBaseConvert.ConvertToInt32(Value.Length))) + Value;
                                                        Int32 NewValueLength = NewValue.Length;
                                                        System.Windows.Forms.MessageBox.Show(FieldNm + " : Old Value >:" + Value + " : Value length >: " + ValueLength.ToString() + " : Filler length >: " + Filler + " : FormatValue >:" + FileFormatValue + " : NewValue >: " + NewValue.ToString());

                                                        if (NewValueLength != FileFormatValue)
                                                        {//Not all do have fillers, so if the do not meet the requirement, Flag them for reject.
                                                            dr["Status"] = "R";
                                                            EJdataTable.AcceptChanges();
                                                            goto JustRejected;
                                                        }
                                                    }
                                                    arryRow[t] = NewValue;
                                                    t = t + 1;
                                                }
                                                else
                                                {
                                                    arryRow[t] = "";
                                                    t = t + 1;
                                                }
                                            }
                                            break;
                                        case "TOBRANCH":
                                            ////System.Windows.Forms.MessageBox.Show(FieldNm);
                                            if (VoucherType.ToString().ToUpper() == "UCV")
                                            {
                                                ValueLength = BRBase.BRBaseConvert.ConvertToInt32((dr[FieldNm].ToString().Trim()).ToString().Length);
                                                Value = BRBase.BRBaseConvert.ConvertToString(dr[FieldNm].ToString().Trim());
                                                Value = Value.Substring(0, 2);
                                                ValueLength = BRBase.BRBaseConvert.ConvertToInt32((Value).ToString().Length);
                                            }
                                            else
                                            {
                                                ValueLength = BRBase.BRBaseConvert.ConvertToInt32((dr[FieldNm].ToString().Trim()).ToString().Length);
                                                Value = BRBase.BRBaseConvert.ConvertToString(dr[FieldNm].ToString().Trim());
                                                Value = Value.Substring(0, 2);
                                                ValueLength = BRBase.BRBaseConvert.ConvertToInt32((Value).ToString().Length);
                                            }
                                            if (ValueLength > FileFormatValue)
                                            {
                                                dr["Status"] = "R";
                                                EJdataTable.AcceptChanges();
                                                goto JustRejected;
                                            }
                                            else
                                            {
                                                //Value = BRBase.BRBaseConvert.ConvertToString(dr[FieldNm].ToString().Trim());
                                                NewValue = Value;
                                                if (FileFormatValueMandatoryLength == true)
                                                {
                                                    if (ValueLength != FileFormatValue)
                                                    {
                                                        if (FieldNm == "VALUE")
                                                        {
                                                            //Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(Value)));
                                                            Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(Value), BRBase.BRBaseConvert.ConvertToString(dr["VoucherCode"])));
                                                            if (Value.ToString().Contains("."))
                                                            {
                                                                Value = Value.Substring(0, Value.IndexOf("."));
                                                            }
                                                        }
                                                        //First Fill in the required Characters.
                                                        System.Windows.Forms.MessageBox.Show(FieldNm + " : Old Value >:" + Value + " : Value length >: " + ValueLength.ToString() + " : Filler length >: " + Filler + " : FormatValue >:" + FileFormatValue + " : NewValue >: " + NewValue.ToString());

                                                        NewValue = new string(BRBase.BRBaseConvert.ConvertToChar(Filler), (BRBase.BRBaseConvert.ConvertToInt32(FileFormatValue) - BRBase.BRBaseConvert.ConvertToInt32(Value.Length))) + Value;
                                                        System.Windows.Forms.MessageBox.Show(FieldNm + " : Old Value >:" + Value + " : Value length >: " + ValueLength.ToString() + " : Filler length >: " + Filler + " : FormatValue >:" + FileFormatValue + " : NewValue >: " + NewValue.ToString());

                                                        Int32 NewValueLength = NewValue.Length;
                                                        if (NewValueLength != FileFormatValue)
                                                        {//Not all do have fillers, so if the do not meet the requirement, Flag them for reject.
                                                            dr["Status"] = "R";
                                                            EJdataTable.AcceptChanges();
                                                            goto JustRejected;
                                                        }
                                                    }
                                                    arryRow[t] = NewValue;
                                                    t = t + 1;
                                                }
                                                else
                                                {
                                                    arryRow[t] = "";
                                                    t = t + 1;
                                                }
                                                Value = BRBase.BRBaseConvert.ConvertToString(dr["TOBRANCH"].ToString().Trim());
                                                arryRow[t] = Value.Substring(2, 2);
                                                t = t + 1;
                                            }
                                            break;
                                        case "TOACCOUNT":
                                            //System.Windows.Forms.MessageBox.Show(FieldNm);
                                            if (VoucherType.ToString().ToUpper() == "UCV")
                                            {
                                                ValueLength = BRBase.BRBaseConvert.ConvertToInt32((dr["COLLECTIONACCOUNT"].ToString().Trim()).ToString().Length);
                                                Value = BRBase.BRBaseConvert.ConvertToString(dr["COLLECTIONACCOUNT"].ToString().Trim());
                                            }
                                            else
                                            {
                                                ValueLength = BRBase.BRBaseConvert.ConvertToInt32((dr[FieldNm].ToString().Trim()).ToString().Length);
                                                Value = BRBase.BRBaseConvert.ConvertToString(dr[FieldNm].ToString().Trim());
                                            }
                                            if (ValueLength > 10)
                                            {
                                                dr["Status"] = "R";
                                                EJdataTable.AcceptChanges();
                                                goto JustRejected;
                                            }
                                            else
                                            {
                                                //Value = BRBase.BRBaseConvert.ConvertToString(dr[FieldNm].ToString().TrimStart('0'));
                                                NewValue = Value;
                                                if (FileFormatValueMandatoryLength == true)
                                                {
                                                    if (ValueLength != 10)
                                                    {
                                                        if (FieldNm == "VALUE")
                                                        {
                                                            //Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(Value)));
                                                            Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(Value), BRBase.BRBaseConvert.ConvertToString(dr["VoucherCode"])));
                                                            if (Value.ToString().Contains("."))
                                                            {
                                                                Value = Value.Substring(0, Value.IndexOf("."));
                                                            }
                                                        }
                                                        //First Fill in the required Characters.
                                                        NewValue = new string(BRBase.BRBaseConvert.ConvertToChar(Filler), (BRBase.BRBaseConvert.ConvertToInt32(10) - BRBase.BRBaseConvert.ConvertToInt32(Value.Length))) + Value;
                                                        Int32 NewValueLength = NewValue.Length;
                                                        if (NewValueLength != 10)
                                                        {//Not all do have fillers, so if the do not meet the requirement, Flag them for reject.
                                                            dr["Status"] = "R";
                                                            EJdataTable.AcceptChanges();
                                                            goto JustRejected;
                                                        }
                                                    }
                                                    arryRow[t] = NewValue;
                                                    t = t + 1;
                                                }
                                                else
                                                {
                                                    arryRow[t] = "";
                                                    t = t + 1;
                                                }
                                            }
                                            break;
                                        case "COLLECTIONACCOUNT":
                                            //System.Windows.Forms.MessageBox.Show(FieldNm);
                                            if (VoucherType.ToString().ToUpper() == "UCV")
                                            {
                                                ValueLength = BRBase.BRBaseConvert.ConvertToInt32((dr["ORIGINATORREFERENCECODE"].ToString().Trim()).ToString().Length);
                                                Value = BRBase.BRBaseConvert.ConvertToString(dr["ORIGINATORREFERENCECODE"].ToString().Trim());
                                            }
                                            else
                                            {
                                                ValueLength = BRBase.BRBaseConvert.ConvertToInt32((dr["ORIGINATORREFERENCECODE"].ToString().Trim()).ToString().Length);
                                                Value = BRBase.BRBaseConvert.ConvertToString(dr["ORIGINATORREFERENCECODE"].ToString().Trim());
                                            }
                                            if (ValueLength > FileFormatValue)
                                            {
                                                Value = BRBase.BRBaseConvert.ConvertToString(dr["ORIGINATORREFERENCECODE"].ToString().Substring(0,20));
                                                //dr["Status"] = "R";
                                                //EJdataTable.AcceptChanges();
                                                //goto JustRejected;
                                            }
                                            else
                                            {
                                                Value = BRBase.BRBaseConvert.ConvertToString(dr["ORIGINATORREFERENCECODE"].ToString().Trim() + new string(BRBase.BRBaseConvert.ConvertToChar(Filler), FileFormatValue - ValueLength));
                                                NewValue = Value;
                                            }
                                            arryRow[t] = Value;
                                            t = t + 1;
                                            break;
                                        case "CURRENCYCODE":
                                            //Value = curr;
                                            //if (Value == "")
                                            //{
                                            //    Value = BRBase.BRBaseConvert.ConvertToString(WorkingDataTable.Tables[0].Rows[j]["DEFAULTVALUES"].ToString().Trim());
                                            //}
                                            //arryRow[t] = Value;
                                            //t = t + 1;
                                            break;
                                        case "FILLER":
                                            //System.Windows.Forms.MessageBox.Show(FieldNm);
                                            Value = BRBase.BRBaseConvert.ConvertToString(WorkingDataTable.Tables[0].Rows[j]["DEFAULTVALUES"].ToString().Trim());
                                            ValueLength = BRBase.BRBaseConvert.ConvertToInt32((Value.ToString().Trim()).ToString().Length);
                                            if (ValueLength != FileFormatValue)
                                            {
                                                NewValue = Value;
                                                NewValue = new string(BRBase.BRBaseConvert.ConvertToChar(Filler), (BRBase.BRBaseConvert.ConvertToInt32(FileFormatValue) - BRBase.BRBaseConvert.ConvertToInt32(NewValue.ToString().Length))) + NewValue;
                                            }
                                            arryRow[t] = NewValue;
                                            t = t + 1;
                                            break;
                                        case "ENTRYMODE":
                                            Value = "2"; //BRBaseConvert.ConvertToString(WorkingDataTable.Tables[0].Rows[j]["DEFAULTVALUES"].ToString().Trim());
                                            arryRow[t] = Value;
                                            t = t + 1;
                                            break;
                                        case "PCC":
                                            //System.Windows.Forms.MessageBox.Show(FieldNm);
                                            Value = BRBase.BRBaseConvert.ConvertToString(dr["FROMBRANCH"].ToString().Trim());
                                            arryRow[t] = Value.Substring(2,2);
                                            t = t + 1;
                                            break;
                                        case "DCC":
                                            Value = BRBase.BRBaseConvert.ConvertToString(dr["TOBRANCH"].ToString().Trim());
                                            arryRow[t] = Value.Substring(2,2);
                                            t = t + 1;
                                            break;
                                        case "FROMBANK":
                                            //System.Windows.Forms.MessageBox.Show(FieldNm);
                                            if (VoucherType.ToString().ToUpper() != "UCV")
                                            {
                                                ValueLength = BRBase.BRBaseConvert.ConvertToInt32((usrInfo.strBank.ToString().Trim()).ToString().Length);
                                                Value = BRBase.BRBaseConvert.ConvertToString(usrInfo.strBank.ToString().Trim());
                                            }
                                            else
                                            {
                                                ValueLength = BRBase.BRBaseConvert.ConvertToInt32((dr["TOBANK"].ToString().Trim()).ToString().Length);
                                                Value = BRBase.BRBaseConvert.ConvertToString(dr["TOBANK"].ToString().Trim());
                                            }
                                            if (ValueLength > FileFormatValue)
                                            {
                                                dr["Status"] = "R";
                                                EJdataTable.AcceptChanges();
                                                goto JustRejected;
                                            }
                                            else
                                            {
                                                //Value = BRBase.BRBaseConvert.ConvertToString(dr[FieldNm].ToString().Trim());
                                                NewValue = Value;
                                                if (FileFormatValueMandatoryLength == true)
                                                {
                                                    if (ValueLength != FileFormatValue)
                                                    {
                                                        if (FieldNm == "VALUE")
                                                        {
                                                            //Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(Value)));
                                                            Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(Value), BRBase.BRBaseConvert.ConvertToString(dr["VoucherCode"])));
                                                            if (Value.ToString().Contains("."))
                                                            {
                                                                Value = Value.Substring(0, Value.IndexOf("."));
                                                            }
                                                        }
                                                        //First Fill in the required Characters.
                                                        NewValue = new string(BRBase.BRBaseConvert.ConvertToChar(Filler), (BRBase.BRBaseConvert.ConvertToInt32(FileFormatValue) - BRBase.BRBaseConvert.ConvertToInt32(Value.Length))) + Value;
                                                        Int32 NewValueLength = NewValue.Length;
                                                        if (NewValueLength != FileFormatValue)
                                                        {//Not all do have fillers, so if the do not meet the requirement, Flag them for reject.
                                                            dr["Status"] = "R";
                                                            EJdataTable.AcceptChanges();
                                                            goto JustRejected;
                                                        }
                                                    }
                                                    arryRow[t] = NewValue;
                                                    t = t + 1;
                                                }
                                                else
                                                {
                                                    arryRow[t] = "";
                                                    t = t + 1;
                                                }
                                            }
                                            break;
                                        case "FROMBRANCH":
                                            //System.Windows.Forms.MessageBox.Show(FieldNm);
                                            if (VoucherType.ToString().ToUpper() != "UCV")
                                            {
                                                ValueLength = BRBase.BRBaseConvert.ConvertToInt32((dr["FROMBRANCH"].ToString().Trim()).ToString().Length);
                                                Value = BRBase.BRBaseConvert.ConvertToString(dr["FROMBRANCH"].ToString().Trim());
                                                if (Value.Length > 2)
                                                {
                                                    Value = Value.Substring(0, 2);
                                                }
                                                else
                                                {
                                                    MessageBox.Show(Value);
                                                }
                                            }
                                            else
                                            {
                                                ValueLength = BRBase.BRBaseConvert.ConvertToInt32((dr["TOBRANCH"].ToString().Trim()).ToString().Length);
                                                Value = BRBase.BRBaseConvert.ConvertToString(dr["TOBRANCH"].ToString().Trim());
                                                if (Value.Length > 2)
                                                {
                                                    Value = Value.Substring(0, 2);
                                                }
                                                else
                                                {
                                                    MessageBox.Show(Value);
                                                }
                                            }
                                            
                                             ValueLength = BRBase.BRBaseConvert.ConvertToInt32((Value).ToString().Length);
                                            if (ValueLength > FileFormatValue)
                                            {
                                                dr["Status"] = "R";
                                                EJdataTable.AcceptChanges();
                                                goto JustRejected;
                                            }
                                            else
                                            {
                                                //Value = BRBase.BRBaseConvert.ConvertToString(dr[FieldNm].ToString().Trim());
                                                NewValue = Value;
                                                if (FileFormatValueMandatoryLength == true)
                                                {
                                                    if (ValueLength != FileFormatValue)
                                                    {
                                                        if (FieldNm == "VALUE")
                                                        {
                                                            //Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(Value)));
                                                            Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(Value), BRBase.BRBaseConvert.ConvertToString(dr["VoucherCode"])));
                                                            if (Value.ToString().Contains("."))
                                                            {
                                                                Value = Value.Substring(0, Value.IndexOf("."));
                                                            }
                                                        }
                                                        //First Fill in the required Characters.
                                                        NewValue = new string(BRBase.BRBaseConvert.ConvertToChar(Filler), (BRBase.BRBaseConvert.ConvertToInt32(FileFormatValue) - BRBase.BRBaseConvert.ConvertToInt32(Value.Length))) + Value;
                                                        Int32 NewValueLength = NewValue.Length;
                                                        if (NewValueLength != FileFormatValue)
                                                        {//Not all do have fillers, so if the do not meet the requirement, Flag them for reject.
                                                            dr["Status"] = "R";
                                                            EJdataTable.AcceptChanges();
                                                            goto JustRejected;
                                                        }
                                                    }
                                                    arryRow[t] = NewValue;
                                                    t = t + 1;
                                                }
                                                else
                                                {
                                                    arryRow[t] = "";
                                                    t = t + 1;
                                                }
                                                Value = BRBase.BRBaseConvert.ConvertToString(dr["FROMBRANCH"].ToString().Trim());
                                                if (Value.Length > 2)
                                                {
                                                    arryRow[t] = Value.Substring(2, 2);
                                                }
                                                else
                                                {
                                                    MessageBox.Show(Value.Substring(2));
                                                    arryRow[t] = Value.Substring(2);
                                                }
                                                t = t + 1;
                                            }
                                            break;
                                        case "DRN":
                                            //System.Windows.Forms.MessageBox.Show( "Iko DRN");
                                            if (conn.State != ConnectionState.Open)
                                            {
                                                conn = GetConnection();
                                            }

                                            

                                            BRBase.BRDataSet dsUniqueClearingID = new BRBase.BRDataSet();
                                            string UniqueID = string.Empty;
                                            ClearingUniversalMethod(usrInfo, "p_GetUniqueClearingREFID", out dsUniqueClearingID, BRBase.BRModule.InwardFileImportation, conn, new object[] { "BrDataSet" }, new object[] {});
                                            if (dsUniqueClearingID.Tables.Count > 0)
                                            {
                                                if (dsUniqueClearingID.Tables[0].Rows.Count > 0)
                                                {
                                                    UniqueID = dsUniqueClearingID.Tables[0].Rows[0]["UniqueID"].ToString();
                                                }
                                            }
                                            else
                                            {
                                                UniqueID = GetNextInt16().ToString();
                                            }
                                            dsUniqueClearingID.Tables[0].Clear();
                                            arryRow[t] = new string('0', FileFormatValue - UniqueID.Length) + UniqueID;
                                            t = t + 1;
                                            break;
                                        case "SERIALNUMBER":
                                            //System.Windows.Forms.MessageBox.Show(FieldNm);
                                            ValueLength = BRBase.BRBaseConvert.ConvertToInt32((dr[FieldNm].ToString().Trim()).ToString().Length);
                                            Value = BRBase.BRBaseConvert.ConvertToString(dr[FieldNm].ToString().Trim());
                                            if (ValueLength > 6)
                                            {
                                                dr["Status"] = "R";
                                                EJdataTable.AcceptChanges();
                                                goto JustRejected;
                                            }
                                            else
                                            {
                                                NewValue = Value;
                                                if (FileFormatValueMandatoryLength == true)
                                                {
                                                    if (ValueLength != 6)
                                                    {
                                                        //First Fill in the required Characters.
                                                        NewValue = new string(BRBase.BRBaseConvert.ConvertToChar('0'), (BRBase.BRBaseConvert.ConvertToInt32(6) - BRBase.BRBaseConvert.ConvertToInt32(Value.Length))) + Value;
                                                        Int32 NewValueLength = NewValue.Length;
                                                        if (NewValueLength != 6)
                                                        {//Not all do have fillers, so if the do not meet the requirement, Flag them for reject.
                                                            dr["Status"] = "R";
                                                            EJdataTable.AcceptChanges();
                                                            goto JustRejected;
                                                        }
                                                    }
                                                    arryRow[t] = NewValue;
                                                    t = t + 1;
                                                }
                                                else
                                                {
                                                    arryRow[t] = "";
                                                    t = t + 1;
                                                }
                                            }
                                            break;
                                        case "FRONTIMAGESIZE1":
                                            //if (dsWithImages.Tables.Count > 0)
                                            //{
                                            //    arryRow[t] = ds4Images.Tables[0].Rows[0]["TFImageSize"];
                                            //    t = t + 1;
                                            //}
                                            //else
                                            //{
                                            //    arryRow[t] = "";
                                            //    t = t + 1;
                                            //}
                                            break;
                                        case "FRONTIMAGESIZESIGNATURE1":
                                            //if (dsWithImages.Tables.Count > 0)
                                            //{
                                            //    arryRow[t] = ds4Images.Tables[0].Rows[0]["TFImageSignature"];
                                            //    t = t + 1;
                                            //}
                                            //else
                                            //{
                                            //    arryRow[t] = "";
                                            //    t = t + 1;
                                            //}
                                            break;
                                        case "FRONTIMAGESIZE2":
                                            //if (dsWithImages.Tables.Count > 0)
                                            //{
                                            //    arryRow[t] = ds4Images.Tables[0].Rows[0]["JFImageSize"];
                                            //    t = t + 1;
                                            //}
                                            //else
                                            //{
                                            //    arryRow[t] = "";
                                            //    t = t + 1;
                                            //}
                                            break;
                                        case "FRONTIMAGESIZESIGNATURE2":
                                            //if (dsWithImages.Tables.Count > 0)
                                            //{
                                            //    arryRow[t] = ds4Images.Tables[0].Rows[0]["JFImageSignature"];
                                            //    t = t + 1;
                                            //}
                                            //else
                                            //{
                                            //    arryRow[t] = "";
                                            //    t = t + 1;
                                            //}
                                            break;
                                        case "BACKIMAGESIZE":
                                            //if (dsWithImages.Tables.Count > 0)
                                            //{
                                            //    arryRow[t] = ds4Images.Tables[0].Rows[0]["JRImageSize"];
                                            //    t = t + 1;
                                            //}
                                            //else
                                            //{
                                            //    arryRow[t] = "";
                                            //    t = t + 1;
                                            //}
                                            break;
                                        case "BACKIMAGESIZESIGNATURE":
                                            //if (dsWithImages.Tables.Count > 0)
                                            //{
                                            //    arryRow[t] = ds4Images.Tables[0].Rows[0]["JRImageSignature"];
                                            //    t = t + 1;
                                            //}
                                            //else
                                            //{
                                            //    arryRow[t] = "";
                                            //    t = t + 1;
                                            //}
                                            break;
                                        case "FRONTIMAGE1":
                                            //if (dsWithImages.Tables.Count > 0)
                                            //{
                                            //    arryRow[t] = ds4Images.Tables[0].Rows[0]["TFImage"];
                                            //    t = t + 1;
                                            //}
                                            //else
                                            //{
                                            //    arryRow[t] = "";
                                            //    t = t + 1;
                                            //}
                                            break;
                                        case "FRONTIMAGE2":
                                            //if (dsWithImages.Tables.Count > 0)
                                            //{
                                            //    arryRow[t] = ds4Images.Tables[0].Rows[0]["JFImage"];
                                            //    t = t + 1;
                                            //}
                                            //else
                                            //{
                                            //    arryRow[t] = "";
                                            //    t = t + 1;
                                            //}
                                            break;
                                        case "BACKIMAGE":
                                            //if (dsWithImages.Tables.Count > 0)
                                            //{
                                            //    arryRow[t] = ds4Images.Tables[0].Rows[0]["JRImage"];
                                            //    t = t + 1;
                                            //}
                                            //else
                                            //{
                                            //    arryRow[t] = "";
                                            //    t = t + 1;
                                            //}
                                            break;
                                    }
                                }
                                Data = string.Empty;
                                //System.Windows.Forms.MessageBox.Show(Data + "Imepata Data");
                                for (m = 0; m < arryRow.Length; m++)
                                {
                                    Data = Data + (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
                                }
                                //For Unpaid Trx, retreave the origianl Trx
                                if (arryRow[0].ToString() != "00")
                                {
                                    if (arryRow[0].ToString() != "17")
                                    {
                                        if (conn.State != ConnectionState.Open)
                                        {
                                            conn = GetConnection();
                                        }
                                        BRBase.BRDataSet dsOriginalUnpaidTrx = new BRBase.BRDataSet();
                                        //First I retrieve what came in, so that i just change the returncode
                                        try
                                        {
                                            ClearingUniversalMethod(usrInfo, "p_GetTheOriginalUnpaidTrx", out dsOriginalUnpaidTrx, BRBase.BRModule.GenerateClearingFile, conn, new object[] { "BrDataSet" }, new object[] { StrTrxRowID });
                                            if (dsOriginalUnpaidTrx.Tables.Count > 0)
                                            {
                                                if (dsOriginalUnpaidTrx.Tables[0].Rows.Count > 0)
                                                {
                                                    if (dsOriginalUnpaidTrx.Tables[0].Rows[0]["Data"].ToString() != "NIL")
                                                    {
                                                        Data = dsOriginalUnpaidTrx.Tables[0].Rows[0]["Data"].ToString();
                                                        //System.Windows.Forms.MessageBox.Show(Data + StrTrxRowID);
                                                        Data = Data.Substring(2);
                                                        Data = arryRow[0].ToString() + Data;
                                                    }
                                                }
                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                            System.Windows.Forms.MessageBox.Show(ex.Message + StrTrxRowID + "Imechapa");
                                        }
                                        
                                        
                                    }
                                }
                                if (Currency == "UGX")
                                {
                                    SemiCompiledDataToBeWritten.Rows.Add("0", Data, "", BRBase.BRBaseConvert.ConvertToString(dr["TrxRowID"].ToString().Trim()), IsFcy);
                                }
                                else
                                {
                                    SemiCompiledDataToBeWritten.Rows.Add("0", Data, "", BRBase.BRBaseConvert.ConvertToString(dr["TrxRowID"].ToString().Trim()), IsFcy);
                                }
                                Status = true;
                            }
                            catch (Exception ex)
                            {
                                System.Windows.Forms.MessageBox.Show(ex.Message + Data);
                                ex.ToString();
                                Status = false;
                            }
                        JustRejected:
                            break;
                        default: break;
                    }
                }
            }
            WorkingDataTable.Tables[0].Rows.Clear();
            return Status;
        }

        public static string GenerateRandomAlphaNumericCode(int length)
        {
            string characterSet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();

            string randomCode = new string(
                Enumerable.Repeat(characterSet, length)
                    .Select(set => set[random.Next(set.Length)])
                    .ToArray());
            return randomCode;
        }
        public static DataTable GenerateSettlementFileUG(string CurrencyID, BRBase.BRDataSet dsClearingBanks, string WorkingDate, string Type, DS_trxClearing dstrxClearing, DS_ClearingFileFormat dsClearingFileFormat, BRBase.UserInfo usrInfo, DateTime WDate, DateTime FDate, IDbConnection conn, string ClearingCenter)
        {
            Int32 t, m;
            string Value = string.Empty;
            string Data = string.Empty;
            t = 0;
            string ToBank = string.Empty;
            double SumCheques = 0;
            string SumChq = string.Empty;
            double SumCredits = 0;
            string SumCr = string.Empty;
            double SumDebits = 0;
            string SumDr = string.Empty;
            string UnpaidValue = "0";
            double SumUnpaidCheques = 0;
            string SumUnChq = string.Empty;
            double SumunpaidCredits = 0;
            string SumUnCr = string.Empty;
            double SumUnpaidDebits = 0;
            string SumUnDr = string.Empty;
            double SumDiscCheques = 0;
            string MDVValue = "0";
            string TotalCountMDV = "0";
            double SumunpaidMDV = 0;
            double SumMDV = 0;
            string TotalUnpaidCountMDV = "0";
            string MDVUnpaidValue = "0";
            string SumDiscChq = string.Empty;
            string TotalCountCheques = "0";
            string TotalCountDebits = "0";
            string TotalCountCredits = "0";
            string TotalUnpaidCountCheques = "0";
            string TotalUnpaidCountDebits = "0";
            string TotalUnpaidCountCredits = "0";
            string TotalDiscCountCheques = "0";
            string RecordType = string.Empty;
            DS_trxClearing EJdataTable = new DS_trxClearing();
            ArrayList arr = new ArrayList();
            DataRow[] drHeaderFileFormatResult = null;
            Object[] arryRow = new Object[10];
            DataTable dt = new DataTable();
            DataSet SettlementListingDt = new DataSet();
            string Curr = string.Empty;
            dt.Columns.Add("Data", typeof(string));
            arr.Add("HEADER");
            arr.Add("06");
            arr.Add("TRAILER");
            if (CurrencyID.ToString() == "UGX")
            {
                Curr = "0";
            }
            else
            {
                switch (CurrencyID.ToString().ToUpper())
                {
                    case "USD":
                        Curr = "1";
                        break;
                    case "GBP":
                        Curr = "3";
                        break;
                    case "EUR":
                        Curr = "2";
                        break;
                    case "KES":
                        Curr = "4";
                        break;
                }
            }

            if (SettlementListingDt.Tables.Contains("t_SettlementListing") == false)
            {
                SettlementListingDt.Tables.Add("dt_SettlementListing");
                //SettlementListingDt.Tables[0].TableName = "t_SettlementListing";
                SettlementListingDt.Tables[0].Columns.Add("BankID", typeof(string));
                SettlementListingDt.Tables[0].Columns.Add("CRCount", typeof(Int32));
                SettlementListingDt.Tables[0].Columns.Add("CRAmount", typeof(double));
                SettlementListingDt.Tables[0].Columns.Add("DRCount", typeof(Int32));
                SettlementListingDt.Tables[0].Columns.Add("DRAmount", typeof(double));
                SettlementListingDt.Tables[0].Columns.Add("MDVCount", typeof(Int32));
                SettlementListingDt.Tables[0].Columns.Add("MDVAmount", typeof(double));
                SettlementListingDt.Tables[0].Columns.Add("DRUnpaidCount", typeof(Int32));
                SettlementListingDt.Tables[0].Columns.Add("DRUnpaidAmount", typeof(double));
                SettlementListingDt.Tables[0].Columns.Add("DiscCount", typeof(Int32));
                SettlementListingDt.Tables[0].Columns.Add("DiscAmount", typeof(double));
                SettlementListingDt.Tables[0].Columns.Add("CRUnpaidCount", typeof(Int32));
                SettlementListingDt.Tables[0].Columns.Add("CRUnpaidAmount", typeof(double));
                SettlementListingDt.Tables[0].Columns.Add("Data", typeof(string));
                SettlementListingDt.Tables[0].Columns.Add("Date", typeof(DateTime));
                SettlementListingDt.Tables[0].Columns.Add("CurrencyID", typeof(string));
                SettlementListingDt.Tables[0].Columns.Add("ClearingType", typeof(string));
                SettlementListingDt.Tables[0].Columns.Add("FileDate", typeof(DateTime));

            }

            try
            {
                for (Int32 p = 0; p < arr.Count; p++)
                {
                    switch (arr[p].ToString().ToUpper())
                    {
                        case "HEADER":
                            //                                                              Proc No.    
                            Data = "180" + WorkingDate + "00" + usrInfo.strBank + "0000" + "00000000" + "00" + Curr + new string('0', 48);
                            dt.Rows.Add(Data);
                            break;
                        case "TRAILER":
                            Data = "1900" + usrInfo.strBank + "0000" + new string('0', 68);
                            dt.Rows.Add(Data);
                            break;
                        case "06":
                            CurrencyID = "UGX";
                            if (CurrencyID.ToString() != "UGX")
                            {
                                for (Int32 n = 0; n < dsClearingBanks.Tables[0].Rows.Count; n++)
                                {
                                    ToBank = dsClearingBanks.Tables[0].Rows[n]["BankID"].ToString().Trim();
                                    string OldBankID = ToBank;
                                    //60
                                    arryRow = new Object[10];
                                    RecordType = "06";
                                    // RecordType
                                    arryRow[t] = RecordType;
                                    t = t + 1;

                                    // ToBank
                                    arryRow[t] = OldBankID;
                                    t = t + 1;

                                    // Currency
                                    arryRow[t] = "60";
                                    t = t + 1;

                                    // Presentments
                                    EJdataTable.Tables[0].Rows.Clear();
                                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("(SERIALNUMBER IS NOT NULL OR SERIALNUMBER <> 0) AND CHEQUEDIGIT IS NOT NULL  AND RETURNCODE = '00' AND VOUCHERCODE IN('60') AND TOBANK ='" + ToBank + "' AND TRXTYPE='OC'");
                                    foreach (DataRow dvr in drHeaderFileFormatResult)
                                    {
                                        EJdataTable.Tables[0].ImportRow(dvr);
                                        SumCheques = SumCheques + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                    }
                                    EJdataTable.AcceptChanges();
                                    TotalCountCheques = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                    TotalCountCheques = new string('0', 6 - TotalCountCheques.Length) + TotalCountCheques;
                                    // Pramod
                                    //Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumCheques)));
                                    if (Curr == "00")
                                    {
                                        Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(SumCheques)));
                                    }
                                    else
                                    {
                                        Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(SumCheques), "60"));
                                    }
                                    Value = new string('0', 14 - Value.Length) + Value;
                                    EJdataTable.Tables[0].Rows.Clear();

                                    // Presentments Count
                                    arryRow[t] = TotalCountCheques;
                                    t = t + 1;

                                    // Presentments Sum
                                    arryRow[t] = Value;
                                    t = t + 1;

                                    // Discrepancy
                                    EJdataTable.Tables[0].Rows.Clear();
                                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select(" CHEQUEDIGIT IS NOT NULL AND RETURNCODE = '00' AND VOUCHERCODE IN('60') AND TOBANK ='" + ToBank + "' AND TRXTYPE='OC'");
                                    foreach (DataRow dvr in drHeaderFileFormatResult)
                                    {
                                        EJdataTable.Tables[0].ImportRow(dvr);
                                        SumDiscCheques = SumDiscCheques + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                    }
                                    EJdataTable.AcceptChanges();
                                    TotalDiscCountCheques = "0"; //TotalDiscCountCheques = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                    TotalDiscCountCheques = new string('0', 6 - TotalDiscCountCheques.Length) + TotalDiscCountCheques;
                                    Value = "0"; //Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumDiscCheques)));
                                    Value = new string('0', 13 - Value.Length) + Value;
                                    EJdataTable.Tables[0].Rows.Clear();

                                    // Discrepancy Count Discrepancy
                                    arryRow[t] = TotalDiscCountCheques;
                                    t = t + 1;

                                    // Discrepancy Sum Discrepancy
                                    arryRow[t] = "+" + Value;
                                    t = t + 1;

                                    // Unpaid
                                    EJdataTable.Tables[0].Rows.Clear();
                                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("CHEQUEDIGIT IS NOT NULL AND RETURNCODE <> '00' AND VOUCHERCODE IN('60') AND TOBANK ='" + ToBank + "' AND TRXTYPE='OC'");
                                    foreach (DataRow dvr in drHeaderFileFormatResult)
                                    {
                                        EJdataTable.Tables[0].ImportRow(dvr);
                                        SumUnpaidCheques = SumUnpaidCheques + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                    }
                                    EJdataTable.AcceptChanges();
                                    TotalUnpaidCountCheques = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                    TotalUnpaidCountCheques = new string('0', 6 - TotalUnpaidCountCheques.Length) + TotalUnpaidCountCheques;
                                    // Pramod
                                    //UnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumUnpaidCheques)));
                                    if (Curr == "00")
                                    {
                                        UnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(SumUnpaidCheques)));
                                    }
                                    else
                                    {
                                        UnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(SumUnpaidCheques), "60"));
                                    }
                                    UnpaidValue = new string('0', 14 - UnpaidValue.Length) + UnpaidValue;
                                    EJdataTable.Tables[0].Rows.Clear();

                                    // Unpaid Count Discrepancy
                                    arryRow[t] = TotalUnpaidCountCheques;
                                    t = t + 1;

                                    // Unpaid Sum Discrepancy
                                    arryRow[t] = UnpaidValue;
                                    t = t + 1;

                                    // Filler
                                    arryRow[t] = new string('0', 12);
                                    t = t + 1;

                                    //Reconstruct here
                                    Data = string.Empty;
                                    for (m = 0; m < arryRow.Length; m++)
                                    {
                                        Data = Data + (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
                                    }
                                    t = 0;
                                    dt.Rows.Add(Data);
                                    //BankID    CRNo    CRAmount    DRNo               DrAmt    MDVNo   MDVAmt  DRUnpaidNo              DRUnpaidAmt         DiscNo  DiscAmt     CrUnpaidNo  CRUnPaidAmt     Data        Date            Currency    TrxType     FileDate
                                    SettlementListingDt.Tables[0].Rows.Add(ToBank, 0, 0, BRBase.BRBaseConvert.ConvertToInt32(TotalCountCheques), SumCheques, 0, 0, BRBase.BRBaseConvert.ConvertToInt32(TotalUnpaidCountCheques), SumUnpaidCheques, 0, 0, 0, 0, Data, WDate, "60", "O", FDate);


                                    SumCheques = 0;
                                    SumChq = string.Empty;
                                    SumCredits = 0;
                                    SumCr = string.Empty;
                                    SumDebits = 0;
                                    SumDr = string.Empty;
                                    UnpaidValue = "0";
                                    SumUnpaidCheques = 0;
                                    SumUnChq = string.Empty;
                                    SumunpaidCredits = 0;
                                    SumUnCr = string.Empty;
                                    SumUnpaidDebits = 0;
                                    SumUnDr = string.Empty;
                                    SumDiscCheques = 0;
                                    SumDiscChq = string.Empty;
                                    TotalCountCheques = "0";
                                    TotalCountDebits = "0";
                                    TotalCountCredits = "0";
                                    TotalUnpaidCountCheques = "0";
                                    TotalUnpaidCountDebits = "0";
                                    TotalUnpaidCountCredits = "0";
                                    TotalDiscCountCheques = "0";
                                    MDVValue = "0";
                                    TotalCountMDV = "0";
                                    SumunpaidMDV = 0;
                                    SumMDV = 0;
                                    TotalUnpaidCountMDV = "0";
                                    MDVUnpaidValue = "0";

                                    //61
                                    EJdataTable.Tables[0].Rows.Clear();
                                    arryRow = new Object[10];
                                    RecordType = "06";
                                    // RecordType
                                    arryRow[t] = RecordType;
                                    t = t + 1;

                                    // ToBank
                                    arryRow[t] = OldBankID;
                                    t = t + 1;

                                    // Currency
                                    arryRow[t] = "61";
                                    t = t + 1;

                                    // Presentments
                                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("(SERIALNUMBER IS NOT NULL OR SERIALNUMBER <> 0) AND CHEQUEDIGIT IS NOT NULL  AND RETURNCODE = '00' AND VOUCHERCODE IN('61') AND TOBANK ='" + ToBank + "' AND TRXTYPE='OC'");
                                    foreach (DataRow dvr in drHeaderFileFormatResult)
                                    {
                                        EJdataTable.Tables[0].ImportRow(dvr);
                                        SumCheques = SumCheques + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                    }
                                    EJdataTable.AcceptChanges();
                                    TotalCountCheques = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                    TotalCountCheques = new string('0', 6 - TotalCountCheques.Length) + TotalCountCheques;
                                    // Pramod
                                    //Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumCheques)));
                                    if (Curr == "00")
                                    {
                                        Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(SumCheques)));
                                    }
                                    else
                                    {
                                        Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(SumCheques), "60"));
                                    }
                                    Value = new string('0', 14 - Value.Length) + Value;
                                    EJdataTable.Tables[0].Rows.Clear();

                                    // Presentments Count
                                    arryRow[t] = TotalCountCheques;
                                    t = t + 1;

                                    // Presentments Sum
                                    arryRow[t] = Value;
                                    t = t + 1;

                                    // Discrepancy
                                    EJdataTable.Tables[0].Rows.Clear();
                                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select(" CHEQUEDIGIT IS NOT NULL AND RETURNCODE = '00' AND VOUCHERCODE IN('61') AND TOBANK ='" + ToBank + "' AND TRXTYPE='OC'");
                                    foreach (DataRow dvr in drHeaderFileFormatResult)
                                    {
                                        EJdataTable.Tables[0].ImportRow(dvr);
                                        SumDiscCheques = SumDiscCheques + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                    }
                                    EJdataTable.AcceptChanges();
                                    TotalDiscCountCheques = "0"; //TotalDiscCountCheques = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                    TotalDiscCountCheques = new string('0', 6 - TotalDiscCountCheques.Length) + TotalDiscCountCheques;
                                    Value = "0"; //Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumDiscCheques)));
                                    Value = new string('0', 13 - Value.Length) + Value;
                                    EJdataTable.Tables[0].Rows.Clear();

                                    // Discrepancy Count Discrepancy
                                    arryRow[t] = TotalDiscCountCheques;
                                    t = t + 1;

                                    // Discrepancy Sum Discrepancy
                                    arryRow[t] = "+" + Value;
                                    t = t + 1;

                                    // Unpaid
                                    EJdataTable.Tables[0].Rows.Clear();
                                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("CHEQUEDIGIT IS NOT NULL AND RETURNCODE <> '00' AND VOUCHERCODE IN('61') AND TOBANK ='" + ToBank + "' AND TRXTYPE='OC'");
                                    foreach (DataRow dvr in drHeaderFileFormatResult)
                                    {
                                        EJdataTable.Tables[0].ImportRow(dvr);
                                        SumUnpaidCheques = SumUnpaidCheques + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                    }
                                    EJdataTable.AcceptChanges();
                                    TotalUnpaidCountCheques = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                    TotalUnpaidCountCheques = new string('0', 6 - TotalUnpaidCountCheques.Length) + TotalUnpaidCountCheques;
                                    //UnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumUnpaidCheques)));

                                    UnpaidValue = new string('0', 14 - UnpaidValue.Length) + UnpaidValue;
                                    EJdataTable.Tables[0].Rows.Clear();

                                    // Unpaid Count Discrepancy
                                    arryRow[t] = TotalUnpaidCountCheques;
                                    t = t + 1;

                                    // Unpaid Sum Discrepancy
                                    arryRow[t] = UnpaidValue;
                                    t = t + 1;

                                    // Filler
                                    arryRow[t] = new string('0', 12);
                                    t = t + 1;

                                    //Reconstruct here
                                    Data = string.Empty;
                                    for (m = 0; m < arryRow.Length; m++)
                                    {
                                        Data = Data + (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
                                    }
                                    t = 0;
                                    dt.Rows.Add(Data);
                                    //BankID    CRNo    CRAmount    DRNo               DrAmt    MDVNo   MDVAmt  DRUnpaidNo              DRUnpaidAmt         DiscNo  DiscAmt     CrUnpaidNo  CRUnPaidAmt     Data        Date            Currency    TrxType     FileDate
                                    SettlementListingDt.Tables[0].Rows.Add(ToBank, 0, 0, BRBase.BRBaseConvert.ConvertToInt32(TotalCountCheques), SumCheques, 0, 0, BRBase.BRBaseConvert.ConvertToInt32(TotalUnpaidCountCheques), SumUnpaidCheques, 0, 0, 0, 0, Data, WDate, "61", "O", FDate);

                                    SumCheques = 0;
                                    SumChq = string.Empty;
                                    SumCredits = 0;
                                    SumCr = string.Empty;
                                    SumDebits = 0;
                                    SumDr = string.Empty;
                                    UnpaidValue = "0";
                                    SumUnpaidCheques = 0;
                                    SumUnChq = string.Empty;
                                    SumunpaidCredits = 0;
                                    SumUnCr = string.Empty;
                                    SumUnpaidDebits = 0;
                                    SumUnDr = string.Empty;
                                    SumDiscCheques = 0;
                                    SumDiscChq = string.Empty;
                                    TotalCountCheques = "0";
                                    TotalCountDebits = "0";
                                    TotalCountCredits = "0";
                                    TotalUnpaidCountCheques = "0";
                                    TotalUnpaidCountDebits = "0";
                                    TotalUnpaidCountCredits = "0";
                                    TotalDiscCountCheques = "0";
                                    MDVValue = "0";
                                    TotalCountMDV = "0";
                                    SumunpaidMDV = 0;
                                    SumMDV = 0;
                                    TotalUnpaidCountMDV = "0";
                                    MDVUnpaidValue = "0";




                                    //62
                                    EJdataTable.Tables[0].Rows.Clear();
                                    arryRow = new Object[10];
                                    RecordType = "06";
                                    // RecordType
                                    arryRow[t] = RecordType;
                                    t = t + 1;

                                    // ToBank
                                    arryRow[t] = OldBankID;
                                    t = t + 1;

                                    // Currency
                                    arryRow[t] = "62";
                                    t = t + 1;

                                    // Presentments
                                    EJdataTable.Tables[0].Rows.Clear();
                                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("(SERIALNUMBER IS NOT NULL OR SERIALNUMBER <> 0) AND CHEQUEDIGIT IS NOT NULL  AND RETURNCODE = '00' AND VOUCHERCODE IN('62') AND TOBANK ='" + ToBank + "' AND TRXTYPE='OC'");
                                    foreach (DataRow dvr in drHeaderFileFormatResult)
                                    {
                                        EJdataTable.Tables[0].ImportRow(dvr);
                                        SumCheques = SumCheques + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                    }
                                    EJdataTable.AcceptChanges();
                                    TotalCountCheques = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                    TotalCountCheques = new string('0', 6 - TotalCountCheques.Length) + TotalCountCheques;
                                    //Pramod
                                    //Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumCheques)));
                                    if (Curr == "00")
                                    {
                                        Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(SumCheques)));
                                    }
                                    else
                                    {
                                        Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(SumCheques), "60"));
                                    }
                                    Value = new string('0', 14 - Value.Length) + Value;
                                    EJdataTable.Tables[0].Rows.Clear();

                                    // Presentments Count
                                    arryRow[t] = TotalCountCheques;
                                    t = t + 1;

                                    // Presentments Sum
                                    arryRow[t] = Value;
                                    t = t + 1;

                                    // Discrepancy
                                    EJdataTable.Tables[0].Rows.Clear();
                                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select(" CHEQUEDIGIT IS NOT NULL AND RETURNCODE = '00' AND VOUCHERCODE IN('62') AND TOBANK ='" + ToBank + "' AND TRXTYPE='OC'");
                                    foreach (DataRow dvr in drHeaderFileFormatResult)
                                    {
                                        EJdataTable.Tables[0].ImportRow(dvr);
                                        SumDiscCheques = SumDiscCheques + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                    }
                                    EJdataTable.AcceptChanges();
                                    TotalDiscCountCheques = "0"; //TotalDiscCountCheques = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                    TotalDiscCountCheques = new string('0', 6 - TotalDiscCountCheques.Length) + TotalDiscCountCheques;
                                    Value = "0"; //Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumDiscCheques)));
                                    Value = new string('0', 13 - Value.Length) + Value;
                                    EJdataTable.Tables[0].Rows.Clear();

                                    // Discrepancy Count Discrepancy
                                    arryRow[t] = TotalDiscCountCheques;
                                    t = t + 1;

                                    // Discrepancy Sum Discrepancy
                                    arryRow[t] = "+" + Value;
                                    t = t + 1;

                                    // Unpaid
                                    EJdataTable.Tables[0].Rows.Clear();
                                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("CHEQUEDIGIT IS NOT NULL AND RETURNCODE <> '00' AND VOUCHERCODE IN('62') AND TOBANK ='" + ToBank + "' AND TRXTYPE='OC'");
                                    foreach (DataRow dvr in drHeaderFileFormatResult)
                                    {
                                        EJdataTable.Tables[0].ImportRow(dvr);
                                        SumUnpaidCheques = SumUnpaidCheques + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                    }
                                    EJdataTable.AcceptChanges();
                                    TotalUnpaidCountCheques = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                    TotalUnpaidCountCheques = new string('0', 6 - TotalUnpaidCountCheques.Length) + TotalUnpaidCountCheques;
                                    // Pramod
                                    //UnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumUnpaidCheques)));
                                    if (Curr == "00")
                                    {
                                        UnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(SumUnpaidCheques)));
                                    }
                                    else
                                    {
                                        UnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(SumUnpaidCheques), "60"));
                                    }
                                    UnpaidValue = new string('0', 14 - UnpaidValue.Length) + UnpaidValue;
                                    EJdataTable.Tables[0].Rows.Clear();

                                    // Unpaid Count Discrepancy
                                    arryRow[t] = TotalUnpaidCountCheques;
                                    t = t + 1;

                                    // Unpaid Sum Discrepancy
                                    arryRow[t] = UnpaidValue;
                                    t = t + 1;

                                    // Filler
                                    arryRow[t] = new string('0', 12);
                                    t = t + 1;

                                    //Reconstruct here
                                    Data = string.Empty;
                                    for (m = 0; m < arryRow.Length; m++)
                                    {
                                        Data = Data + (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
                                    }
                                    t = 0;
                                    dt.Rows.Add(Data);
                                    //BankID    CRNo    CRAmount    DRNo               DrAmt    MDVNo   MDVAmt  DRUnpaidNo              DRUnpaidAmt         DiscNo  DiscAmt     CrUnpaidNo  CRUnPaidAmt     Data        Date            Currency    TrxType     FileDate
                                    SettlementListingDt.Tables[0].Rows.Add(ToBank, 0, 0, BRBase.BRBaseConvert.ConvertToInt32(TotalCountCheques), SumCheques, 0, 0, BRBase.BRBaseConvert.ConvertToInt32(TotalUnpaidCountCheques), SumUnpaidCheques, 0, 0, 0, 0, Data, WDate, "62", "O", FDate);

                                    SumCheques = 0;
                                    SumChq = string.Empty;
                                    SumCredits = 0;
                                    SumCr = string.Empty;
                                    SumDebits = 0;
                                    SumDr = string.Empty;
                                    UnpaidValue = "0";
                                    SumUnpaidCheques = 0;
                                    SumUnChq = string.Empty;
                                    SumunpaidCredits = 0;
                                    SumUnCr = string.Empty;
                                    SumUnpaidDebits = 0;
                                    SumUnDr = string.Empty;
                                    SumDiscCheques = 0;
                                    SumDiscChq = string.Empty;
                                    TotalCountCheques = "0";
                                    TotalCountDebits = "0";
                                    TotalCountCredits = "0";
                                    TotalUnpaidCountCheques = "0";
                                    TotalUnpaidCountDebits = "0";
                                    TotalUnpaidCountCredits = "0";
                                    TotalDiscCountCheques = "0";
                                    MDVValue = "0";
                                    TotalCountMDV = "0";
                                    SumunpaidMDV = 0;
                                    SumMDV = 0;
                                    TotalUnpaidCountMDV = "0";
                                    MDVUnpaidValue = "0";


                                    // 07
                                    //60
                                    EJdataTable.Tables[0].Rows.Clear();
                                    arryRow = new Object[8];
                                    t = 0;
                                    RecordType = "07";
                                    // RecordType
                                    arryRow[t] = RecordType;
                                    t = t + 1;

                                    // ToBank
                                    arryRow[t] = OldBankID;
                                    t = t + 1;

                                    // Currency
                                    arryRow[t] = "60";
                                    t = t + 1;

                                    //Presentments
                                    EJdataTable.Tables[0].Rows.Clear();
                                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("(SERIALNUMBER IS NOT NULL OR SERIALNUMBER = 0) AND (CHEQUEDIGIT IS NULL OR CHEQUEDIGIT='') AND VOUCHERCODE ='40' AND RETURNCODE = '00' AND TOBANK ='" + ToBank + "' AND TRXTYPE='OC'");
                                    foreach (DataRow dvr in drHeaderFileFormatResult)
                                    {
                                        EJdataTable.Tables[0].ImportRow(dvr);
                                        SumDebits = SumDebits + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                    }
                                    EJdataTable.AcceptChanges();
                                    TotalCountDebits = "0"; //TotalCountDebits = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                    TotalCountDebits = new string('0', 6 - TotalCountDebits.Length) + TotalCountDebits;
                                    Value = "0";// Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumDebits)));
                                    Value = new string('0', 14 - Value.Length) + Value;
                                    EJdataTable.Tables[0].Rows.Clear();

                                    // Presentments Count
                                    arryRow[t] = TotalCountDebits;
                                    t = t + 1;

                                    // Presentments Sum
                                    arryRow[t] = Value;
                                    t = t + 1;

                                    //UnPaid
                                    EJdataTable.Tables[0].Rows.Clear();
                                    drHeaderFileFormatResult = null;
                                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("(SERIALNUMBER IS NOT NULL OR SERIALNUMBER = 0) AND (CHEQUEDIGIT IS NULL OR CHEQUEDIGIT='') AND VOUCHERCODE ='40' AND RETURNCODE <> '00' AND TOBANK ='" + ToBank + "' AND TRXTYPE='OC'");
                                    foreach (DataRow dvr in drHeaderFileFormatResult)
                                    {
                                        EJdataTable.Tables[0].ImportRow(dvr);
                                        SumUnpaidDebits = SumUnpaidDebits + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                    }
                                    EJdataTable.AcceptChanges();
                                    TotalUnpaidCountDebits = "0"; //TotalUnpaidCountDebits = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                    TotalUnpaidCountDebits = new string('0', 6 - TotalUnpaidCountDebits.Length) + TotalUnpaidCountDebits;
                                    UnpaidValue = "0"; //UnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumUnpaidDebits)));
                                    UnpaidValue = new string('0', 14 - UnpaidValue.Length) + UnpaidValue;
                                    EJdataTable.Tables[0].Rows.Clear();

                                    // UnPaid Count
                                    arryRow[t] = TotalUnpaidCountDebits;
                                    t = t + 1;

                                    // UnPaid Sum
                                    arryRow[t] = UnpaidValue;
                                    t = t + 1;

                                    // Filler
                                    arryRow[t] = new string('0', 32);
                                    t = t + 1;

                                    //Reconstruct here
                                    Data = string.Empty;
                                    for (m = 0; m < arryRow.Length; m++)
                                    {
                                        Data = Data + (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
                                    }
                                    t = 0;
                                    dt.Rows.Add(Data);
                                    //BankID    CRNo    CRAmount    DRNo               DrAmt    MDVNo   MDVAmt  DRUnpaidNo              DRUnpaidAmt         DiscNo  DiscAmt     CrUnpaidNo  CRUnPaidAmt     Data        Date            Currency    TrxType     FileDate
                                    SettlementListingDt.Tables[0].Rows.Add(ToBank, 0, 0, BRBase.BRBaseConvert.ConvertToInt32(TotalCountDebits), SumDebits, 0, 0, BRBase.BRBaseConvert.ConvertToInt32(TotalUnpaidCountDebits), SumUnpaidDebits, 0, 0, 0, 0, Data, WDate, "60", "O", FDate);


                                    //61
                                    arryRow = new Object[8];
                                    t = 0;
                                    RecordType = "07";
                                    // RecordType
                                    arryRow[t] = RecordType;
                                    t = t + 1;

                                    // ToBank
                                    arryRow[t] = OldBankID;
                                    t = t + 1;

                                    // Currency
                                    arryRow[t] = "61";
                                    t = t + 1;

                                    //Presentments
                                    EJdataTable.Tables[0].Rows.Clear();
                                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("CHEQUEDIGIT IS NULL AND VOUCHERCODE ='40' AND RETURNCODE = '00' AND TOBANK ='" + ToBank + "' AND TRXTYPE='OD'");
                                    foreach (DataRow dvr in drHeaderFileFormatResult)
                                    {
                                        EJdataTable.Tables[0].ImportRow(dvr);
                                        SumDebits = SumDebits + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                    }
                                    EJdataTable.AcceptChanges();
                                    TotalCountDebits = "0"; //TotalCountDebits = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                    TotalCountDebits = new string('0', 6 - TotalCountDebits.Length) + TotalCountDebits;
                                    Value = "0";// Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumDebits)));
                                    Value = new string('0', 14 - Value.Length) + Value;
                                    EJdataTable.Tables[0].Rows.Clear();

                                    // Presentments Count
                                    arryRow[t] = TotalCountDebits;
                                    t = t + 1;

                                    // Presentments Sum
                                    arryRow[t] = Value;
                                    t = t + 1;

                                    //UnPaid
                                    EJdataTable.Tables[0].Rows.Clear();
                                    drHeaderFileFormatResult = null;
                                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("CHEQUEDIGIT IS NULL AND VOUCHERCODE ='40' AND RETURNCODE <> '00' AND TOBANK ='" + ToBank + "' AND TRXTYPE='OD'");
                                    foreach (DataRow dvr in drHeaderFileFormatResult)
                                    {
                                        EJdataTable.Tables[0].ImportRow(dvr);
                                        SumUnpaidDebits = SumUnpaidDebits + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                    }
                                    EJdataTable.AcceptChanges();
                                    TotalUnpaidCountDebits = "0"; //TotalUnpaidCountDebits = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                    TotalUnpaidCountDebits = new string('0', 6 - TotalUnpaidCountDebits.Length) + TotalUnpaidCountDebits;
                                    UnpaidValue = "0"; //UnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumUnpaidDebits)));
                                    UnpaidValue = new string('0', 14 - UnpaidValue.Length) + UnpaidValue;
                                    EJdataTable.Tables[0].Rows.Clear();

                                    // UnPaid Count
                                    arryRow[t] = TotalUnpaidCountDebits;
                                    t = t + 1;

                                    // UnPaid Sum
                                    arryRow[t] = UnpaidValue;
                                    t = t + 1;

                                    // Filler
                                    arryRow[t] = new string('0', 32);
                                    t = t + 1;

                                    //Reconstruct here
                                    Data = string.Empty;
                                    for (m = 0; m < arryRow.Length; m++)
                                    {
                                        Data = Data + (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
                                    }
                                    t = 0;
                                    dt.Rows.Add(Data);
                                    //BankID    CRNo    CRAmount    DRNo               DrAmt    MDVNo   MDVAmt  DRUnpaidNo              DRUnpaidAmt         DiscNo  DiscAmt     CrUnpaidNo  CRUnPaidAmt     Data        Date            Currency    TrxType     FileDate
                                    SettlementListingDt.Tables[0].Rows.Add(ToBank, 0, 0, BRBase.BRBaseConvert.ConvertToInt32(TotalCountDebits), SumDebits, 0, 0, BRBase.BRBaseConvert.ConvertToInt32(TotalUnpaidCountDebits), SumUnpaidDebits, 0, 0, 0, 0, Data, WDate, "61", "O", FDate);

                                    //62
                                    arryRow = new Object[8];
                                    t = 0;
                                    RecordType = "07";
                                    // RecordType
                                    arryRow[t] = RecordType;
                                    t = t + 1;

                                    // ToBank
                                    arryRow[t] = OldBankID;
                                    t = t + 1;

                                    // Currency
                                    arryRow[t] = "62";
                                    t = t + 1;

                                    //Presentments
                                    EJdataTable.Tables[0].Rows.Clear();
                                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("(SERIALNUMBER IS NOT NULL OR SERIALNUMBER = 0) AND (CHEQUEDIGIT IS NULL OR CHEQUEDIGIT='') AND VOUCHERCODE ='40' AND RETURNCODE = '00' AND TOBANK ='" + ToBank + "' AND TRXTYPE='OC'");
                                    foreach (DataRow dvr in drHeaderFileFormatResult)
                                    {
                                        EJdataTable.Tables[0].ImportRow(dvr);
                                        SumDebits = SumDebits + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                    }
                                    EJdataTable.AcceptChanges();
                                    TotalCountDebits = "0"; //TotalCountDebits = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                    TotalCountDebits = new string('0', 6 - TotalCountDebits.Length) + TotalCountDebits;
                                    Value = "0";// Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumDebits)));
                                    Value = new string('0', 14 - Value.Length) + Value;
                                    EJdataTable.Tables[0].Rows.Clear();

                                    // Presentments Count
                                    arryRow[t] = TotalCountDebits;
                                    t = t + 1;

                                    // Presentments Sum
                                    arryRow[t] = Value;
                                    t = t + 1;

                                    //UnPaid
                                    EJdataTable.Tables[0].Rows.Clear();
                                    drHeaderFileFormatResult = null;
                                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("(SERIALNUMBER IS NOT NULL OR SERIALNUMBER = 0) AND (CHEQUEDIGIT IS NULL OR CHEQUEDIGIT='') AND VOUCHERCODE ='40' AND RETURNCODE <> '00' AND TOBANK ='" + ToBank + "' AND TRXTYPE='OC'");
                                    foreach (DataRow dvr in drHeaderFileFormatResult)
                                    {
                                        EJdataTable.Tables[0].ImportRow(dvr);
                                        SumUnpaidDebits = SumUnpaidDebits + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                    }
                                    EJdataTable.AcceptChanges();
                                    TotalUnpaidCountDebits = "0"; //TotalUnpaidCountDebits = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                    TotalUnpaidCountDebits = new string('0', 6 - TotalUnpaidCountDebits.Length) + TotalUnpaidCountDebits;
                                    UnpaidValue = "0"; //UnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumUnpaidDebits)));
                                    UnpaidValue = new string('0', 14 - UnpaidValue.Length) + UnpaidValue;
                                    EJdataTable.Tables[0].Rows.Clear();

                                    // UnPaid Count
                                    arryRow[t] = TotalUnpaidCountDebits;
                                    t = t + 1;

                                    // UnPaid Sum
                                    arryRow[t] = UnpaidValue;
                                    t = t + 1;

                                    // Filler
                                    arryRow[t] = new string('0', 32);
                                    t = t + 1;

                                    //Reconstruct here
                                    Data = string.Empty;
                                    for (m = 0; m < arryRow.Length; m++)
                                    {
                                        Data = Data + (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
                                    }
                                    t = 0;
                                    dt.Rows.Add(Data);
                                    //BankID    CRNo    CRAmount    DRNo               DrAmt    MDVNo   MDVAmt  DRUnpaidNo              DRUnpaidAmt         DiscNo  DiscAmt     CrUnpaidNo  CRUnPaidAmt     Data        Date            Currency    TrxType     FileDate
                                    SettlementListingDt.Tables[0].Rows.Add(ToBank, 0, 0, BRBase.BRBaseConvert.ConvertToInt32(TotalCountDebits), SumDebits, 0, 0, BRBase.BRBaseConvert.ConvertToInt32(TotalUnpaidCountDebits), SumUnpaidDebits, 0, 0, 0, 0, Data, WDate, "62", "O", FDate);



                                    // 08
                                    //60
                                    arryRow = new Object[8];
                                    t = 0;
                                    RecordType = "08";
                                    // RecordType
                                    arryRow[t] = RecordType;
                                    t = t + 1;

                                    // ToBank
                                    arryRow[t] = OldBankID;
                                    t = t + 1;

                                    // Currency
                                    arryRow[t] = "60";
                                    t = t + 1;

                                    //Presentation
                                    EJdataTable.Tables[0].Rows.Clear();
                                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("VOUCHERCODE <>'40' AND RETURNCODE IN('00','90','97') AND TOBANK ='" + ToBank + "'  AND TRXTYPE='OD'");
                                    foreach (DataRow dvr in drHeaderFileFormatResult)
                                    {
                                        EJdataTable.Tables[0].ImportRow(dvr);
                                        SumCredits = SumCredits + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                    }
                                    EJdataTable.AcceptChanges();
                                    TotalCountCredits = "0"; //TotalCountCredits = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                    TotalCountCredits = new string('0', 6 - TotalCountCredits.Length) + TotalCountCredits;
                                    Value = "0"; //Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumCredits)));
                                    Value = new string('0', 14 - Value.Length) + Value;
                                    // Presentments Count
                                    arryRow[t] = TotalCountCredits;
                                    t = t + 1;

                                    // Presentments Sum
                                    arryRow[t] = Value;
                                    t = t + 1;

                                    //UnPaids
                                    EJdataTable.Tables[0].Rows.Clear();
                                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("VOUCHERCODE <>'40' AND RETURNCODE NOT IN('00','90','97') AND TOBANK ='" + ToBank + "'  AND TRXTYPE='OD'");
                                    foreach (DataRow dvr in drHeaderFileFormatResult)
                                    {
                                        EJdataTable.Tables[0].ImportRow(dvr);
                                        SumunpaidCredits = SumunpaidCredits + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                    }
                                    EJdataTable.AcceptChanges();
                                    TotalUnpaidCountCredits = "0"; //TotalUnpaidCountCredits = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                    TotalUnpaidCountCredits = new string('0', 6 - TotalUnpaidCountCredits.Length) + TotalUnpaidCountCredits;
                                    UnpaidValue = "0"; //UnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumunpaidCredits)));
                                    UnpaidValue = new string('0', 14 - UnpaidValue.Length) + UnpaidValue;
                                    // UnPaid Count
                                    arryRow[t] = TotalUnpaidCountCredits;
                                    t = t + 1;

                                    // UnPaid Sum
                                    arryRow[t] = UnpaidValue;
                                    t = t + 1;

                                    // Filler
                                    arryRow[t] = new string('0', 32);
                                    t = t + 1;

                                    //Reconstruct here
                                    Data = string.Empty;
                                    for (m = 0; m < arryRow.Length; m++)
                                    {
                                        Data = Data + (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
                                    }
                                    t = 0;
                                    dt.Rows.Add(Data);

                                    //BankID    CRNo    CRAmount    DRNo               DrAmt    MDVNo   MDVAmt  DRUnpaidNo              DRUnpaidAmt         DiscNo  DiscAmt     CrUnpaidNo  CRUnPaidAmt     Data        Date            Currency    TrxType     FileDate
                                    SettlementListingDt.Tables[0].Rows.Add(ToBank, BRBase.BRBaseConvert.ConvertToInt32(TotalCountCredits), SumCredits, 0, 0, 0, 0, BRBase.BRBaseConvert.ConvertToInt32(TotalUnpaidCountCredits), SumunpaidCredits, 0, 0, 0, 0, Data, WDate, "60", "O", FDate);

                                    //61
                                    //61
                                    arryRow = new Object[8];
                                    t = 0;
                                    RecordType = "08";
                                    // RecordType
                                    arryRow[t] = RecordType;
                                    t = t + 1;

                                    // ToBank
                                    arryRow[t] = OldBankID;
                                    t = t + 1;

                                    // Currency
                                    arryRow[t] = "61";
                                    t = t + 1;

                                    //Presentation
                                    EJdataTable.Tables[0].Rows.Clear();
                                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("VOUCHERCODE <>'40' AND RETURNCODE IN('00','90','97') AND TOBANK ='" + ToBank + "'  AND TRXTYPE='OD'");
                                    foreach (DataRow dvr in drHeaderFileFormatResult)
                                    {
                                        EJdataTable.Tables[0].ImportRow(dvr);
                                        SumCredits = SumCredits + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                    }
                                    EJdataTable.AcceptChanges();
                                    TotalCountCredits = "0"; //TotalCountCredits = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                    TotalCountCredits = new string('0', 6 - TotalCountCredits.Length) + TotalCountCredits;
                                    Value = "0"; //Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumCredits)));
                                    Value = new string('0', 14 - Value.Length) + Value;
                                    // Presentments Count
                                    arryRow[t] = TotalCountCredits;
                                    t = t + 1;

                                    // Presentments Sum
                                    arryRow[t] = Value;
                                    t = t + 1;

                                    //UnPaids
                                    EJdataTable.Tables[0].Rows.Clear();
                                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("VOUCHERCODE <>'40' AND RETURNCODE NOT IN('00','90','97') AND TOBANK ='" + ToBank + "'  AND TRXTYPE='OD'");
                                    foreach (DataRow dvr in drHeaderFileFormatResult)
                                    {
                                        EJdataTable.Tables[0].ImportRow(dvr);
                                        SumunpaidCredits = SumunpaidCredits + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                    }
                                    EJdataTable.AcceptChanges();
                                    TotalUnpaidCountCredits = "0"; //TotalUnpaidCountCredits = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                    TotalUnpaidCountCredits = new string('0', 6 - TotalUnpaidCountCredits.Length) + TotalUnpaidCountCredits;
                                    UnpaidValue = "0"; //UnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumunpaidCredits)));
                                    UnpaidValue = new string('0', 14 - UnpaidValue.Length) + UnpaidValue;
                                    // UnPaid Count
                                    arryRow[t] = TotalUnpaidCountCredits;
                                    t = t + 1;

                                    // UnPaid Sum
                                    arryRow[t] = UnpaidValue;
                                    t = t + 1;

                                    // Filler
                                    arryRow[t] = new string('0', 32);
                                    t = t + 1;

                                    //Reconstruct here
                                    Data = string.Empty;
                                    for (m = 0; m < arryRow.Length; m++)
                                    {
                                        Data = Data + (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
                                    }
                                    t = 0;
                                    dt.Rows.Add(Data);

                                    //BankID    CRNo    CRAmount    DRNo               DrAmt    MDVNo   MDVAmt  DRUnpaidNo              DRUnpaidAmt         DiscNo  DiscAmt     CrUnpaidNo  CRUnPaidAmt     Data        Date            Currency    TrxType     FileDate
                                    SettlementListingDt.Tables[0].Rows.Add(ToBank, BRBase.BRBaseConvert.ConvertToInt32(TotalCountCredits), SumCredits, 0, 0, 0, 0, BRBase.BRBaseConvert.ConvertToInt32(TotalUnpaidCountCredits), SumunpaidCredits, 0, 0, 0, 0, Data, WDate, "61", "O", FDate);

                                    //62
                                    //62
                                    arryRow = new Object[8];
                                    t = 0;
                                    RecordType = "08";
                                    // RecordType
                                    arryRow[t] = RecordType;
                                    t = t + 1;

                                    // ToBank
                                    arryRow[t] = OldBankID;
                                    t = t + 1;

                                    // Currency
                                    arryRow[t] = "62";
                                    t = t + 1;

                                    //Presentation
                                    EJdataTable.Tables[0].Rows.Clear();
                                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("VOUCHERCODE <>'40' AND RETURNCODE IN('00','90','97') AND TOBANK ='" + ToBank + "'  AND TRXTYPE='OD'");
                                    foreach (DataRow dvr in drHeaderFileFormatResult)
                                    {
                                        EJdataTable.Tables[0].ImportRow(dvr);
                                        SumCredits = SumCredits + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                    }
                                    EJdataTable.AcceptChanges();
                                    TotalCountCredits = "0"; //TotalCountCredits = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                    TotalCountCredits = new string('0', 6 - TotalCountCredits.Length) + TotalCountCredits;
                                    Value = "0"; //Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumCredits)));
                                    Value = new string('0', 14 - Value.Length) + Value;
                                    // Presentments Count
                                    arryRow[t] = TotalCountCredits;
                                    t = t + 1;

                                    // Presentments Sum
                                    arryRow[t] = Value;
                                    t = t + 1;

                                    //UnPaids
                                    EJdataTable.Tables[0].Rows.Clear();
                                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("VOUCHERCODE <>'40' AND RETURNCODE NOT IN('00','90','97') AND TOBANK ='" + ToBank + "'  AND TRXTYPE='OD'");
                                    foreach (DataRow dvr in drHeaderFileFormatResult)
                                    {
                                        EJdataTable.Tables[0].ImportRow(dvr);
                                        SumunpaidCredits = SumunpaidCredits + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                    }
                                    EJdataTable.AcceptChanges();
                                    TotalUnpaidCountCredits = "0"; //TotalUnpaidCountCredits = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                    TotalUnpaidCountCredits = new string('0', 6 - TotalUnpaidCountCredits.Length) + TotalUnpaidCountCredits;
                                    UnpaidValue = "0"; //UnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumunpaidCredits)));
                                    UnpaidValue = new string('0', 14 - UnpaidValue.Length) + UnpaidValue;
                                    // UnPaid Count
                                    arryRow[t] = TotalUnpaidCountCredits;
                                    t = t + 1;

                                    // UnPaid Sum
                                    arryRow[t] = UnpaidValue;
                                    t = t + 1;

                                    // Filler
                                    arryRow[t] = new string('0', 32);
                                    t = t + 1;

                                    //Reconstruct here
                                    Data = string.Empty;
                                    for (m = 0; m < arryRow.Length; m++)
                                    {
                                        Data = Data + (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
                                    }
                                    t = 0;
                                    dt.Rows.Add(Data);

                                    //BankID    CRNo    CRAmount    DRNo               DrAmt    MDVNo   MDVAmt  DRUnpaidNo              DRUnpaidAmt         DiscNo  DiscAmt     CrUnpaidNo  CRUnPaidAmt     Data        Date            Currency    TrxType     FileDate
                                    SettlementListingDt.Tables[0].Rows.Add(ToBank, BRBase.BRBaseConvert.ConvertToInt32(TotalCountCredits), SumCredits, 0, 0, 0, 0, BRBase.BRBaseConvert.ConvertToInt32(TotalUnpaidCountCredits), SumunpaidCredits, 0, 0, 0, 0, Data, WDate, "62", "O", FDate);



                                    // 09
                                    //60
                                    arryRow = new Object[8];
                                    t = 0;
                                    RecordType = "09";
                                    // RecordType
                                    arryRow[t] = RecordType;
                                    t = t + 1;

                                    // ToBank
                                    arryRow[t] = OldBankID;
                                    t = t + 1;

                                    // Currency
                                    arryRow[t] = "60";
                                    t = t + 1;

                                    //Presentation
                                    EJdataTable.Tables[0].Rows.Clear();
                                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("VOUCHERCODE ='60' AND RETURNCODE = '00' AND TRXTYPE='OC' AND TOBANK ='" + ToBank + "'");
                                    foreach (DataRow dvr in drHeaderFileFormatResult)
                                    {
                                        EJdataTable.Tables[0].ImportRow(dvr);
                                        SumMDV = SumMDV + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                    }
                                    EJdataTable.AcceptChanges();
                                    TotalCountMDV = "0";// TotalCountMDV = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                    TotalCountMDV = new string('0', 6 - TotalCountMDV.Length) + TotalCountMDV;
                                    MDVValue = "0";// MDVValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumMDV)));
                                    MDVValue = new string('0', 14 - MDVValue.Length) + MDVValue;
                                    // Presentments Count
                                    arryRow[t] = TotalCountMDV;
                                    t = t + 1;

                                    // Presentments Sum
                                    arryRow[t] = MDVValue;
                                    t = t + 1;

                                    ////UnPaids
                                    EJdataTable.Tables[0].Rows.Clear();
                                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("VOUCHERCODE ='03' AND  RETURNCODE <> '00' AND TRXTYPE='OC' AND TOBANK ='" + ToBank + "'");
                                    foreach (DataRow dvr in drHeaderFileFormatResult)
                                    {
                                        EJdataTable.Tables[0].ImportRow(dvr);
                                        SumunpaidMDV = SumunpaidMDV + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                    }
                                    EJdataTable.AcceptChanges();
                                    TotalUnpaidCountMDV = "0";// = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                    TotalUnpaidCountMDV = new string('0', 6 - TotalUnpaidCountMDV.Length) + TotalUnpaidCountMDV;
                                    MDVUnpaidValue = "0";// MDVUnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumunpaidMDV)));
                                    MDVUnpaidValue = new string('0', 14 - MDVUnpaidValue.Length) + MDVUnpaidValue;
                                    // UnPaid Count
                                    arryRow[t] = TotalUnpaidCountMDV;
                                    t = t + 1;

                                    // UnPaid Sum
                                    arryRow[t] = MDVUnpaidValue;
                                    t = t + 1;

                                    // Filler
                                    arryRow[t] = new string('0', 32);
                                    t = t + 1;
                                    //Reconstruct here
                                    Data = string.Empty;
                                    for (m = 0; m < arryRow.Length; m++)
                                    {
                                        Data = Data + (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
                                    }
                                    t = 0;
                                    dt.Rows.Add(Data);
                                    //BankID    CRNo    CRAmount    DRNo               DrAmt    MDVNo   MDVAmt  DRUnpaidNo              DRUnpaidAmt         DiscNo  DiscAmt     CrUnpaidNo  CRUnPaidAmt     Data        Date            Currency    TrxType     FileDate
                                    SettlementListingDt.Tables[0].Rows.Add(ToBank, 0, 0, 0, 0, BRBase.BRBaseConvert.ConvertToInt32(TotalCountMDV), SumMDV, BRBase.BRBaseConvert.ConvertToInt32(TotalUnpaidCountMDV), SumunpaidMDV, 0, 0, 0, 0, Data, WDate, "60", "O", FDate);

                                    //61
                                    arryRow = new Object[8];
                                    t = 0;
                                    RecordType = "09";
                                    // RecordType
                                    arryRow[t] = RecordType;
                                    t = t + 1;

                                    // ToBank
                                    arryRow[t] = OldBankID;
                                    t = t + 1;

                                    // Currency
                                    arryRow[t] = "61";
                                    t = t + 1;

                                    //Presentation
                                    EJdataTable.Tables[0].Rows.Clear();
                                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("VOUCHERCODE ='60' AND RETURNCODE = '00' AND TRXTYPE='OC' AND TOBANK ='" + ToBank + "'");
                                    foreach (DataRow dvr in drHeaderFileFormatResult)
                                    {
                                        EJdataTable.Tables[0].ImportRow(dvr);
                                        SumMDV = SumMDV + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                    }
                                    EJdataTable.AcceptChanges();
                                    TotalCountMDV = "0";// TotalCountMDV = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                    TotalCountMDV = new string('0', 6 - TotalCountMDV.Length) + TotalCountMDV;
                                    MDVValue = "0";// MDVValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumMDV)));
                                    MDVValue = new string('0', 14 - MDVValue.Length) + MDVValue;
                                    // Presentments Count
                                    arryRow[t] = TotalCountMDV;
                                    t = t + 1;

                                    // Presentments Sum
                                    arryRow[t] = MDVValue;
                                    t = t + 1;

                                    ////UnPaids
                                    EJdataTable.Tables[0].Rows.Clear();
                                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("VOUCHERCODE ='03' AND  RETURNCODE <> '00' AND TRXTYPE='OC' AND TOBANK ='" + ToBank + "'");
                                    foreach (DataRow dvr in drHeaderFileFormatResult)
                                    {
                                        EJdataTable.Tables[0].ImportRow(dvr);
                                        SumunpaidMDV = SumunpaidMDV + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                    }
                                    EJdataTable.AcceptChanges();
                                    TotalUnpaidCountMDV = "0";// = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                    TotalUnpaidCountMDV = new string('0', 6 - TotalUnpaidCountMDV.Length) + TotalUnpaidCountMDV;
                                    MDVUnpaidValue = "0";// MDVUnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumunpaidMDV)));
                                    MDVUnpaidValue = new string('0', 14 - MDVUnpaidValue.Length) + MDVUnpaidValue;
                                    // UnPaid Count
                                    arryRow[t] = TotalUnpaidCountMDV;
                                    t = t + 1;

                                    // UnPaid Sum
                                    arryRow[t] = MDVUnpaidValue;
                                    t = t + 1;

                                    // Filler
                                    arryRow[t] = new string('0', 32);
                                    t = t + 1;
                                    //Reconstruct here
                                    Data = string.Empty;
                                    for (m = 0; m < arryRow.Length; m++)
                                    {
                                        Data = Data + (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
                                    }
                                    t = 0;
                                    dt.Rows.Add(Data);
                                    //BankID    CRNo    CRAmount    DRNo               DrAmt    MDVNo   MDVAmt  DRUnpaidNo              DRUnpaidAmt         DiscNo  DiscAmt     CrUnpaidNo  CRUnPaidAmt     Data        Date            Currency    TrxType     FileDate
                                    SettlementListingDt.Tables[0].Rows.Add(ToBank, 0, 0, 0, 0, BRBase.BRBaseConvert.ConvertToInt32(TotalCountMDV), SumMDV, BRBase.BRBaseConvert.ConvertToInt32(TotalUnpaidCountMDV), SumunpaidMDV, 0, 0, 0, 0, Data, WDate, "61", "O", FDate);

                                    //62
                                    arryRow = new Object[8];
                                    t = 0;
                                    RecordType = "09";
                                    // RecordType
                                    arryRow[t] = RecordType;
                                    t = t + 1;

                                    // ToBank
                                    arryRow[t] = OldBankID;
                                    t = t + 1;

                                    // Currency
                                    arryRow[t] = "62";
                                    t = t + 1;

                                    //Presentation
                                    EJdataTable.Tables[0].Rows.Clear();
                                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("VOUCHERCODE ='60' AND RETURNCODE = '00' AND TRXTYPE='OC' AND TOBANK ='" + ToBank + "'");
                                    foreach (DataRow dvr in drHeaderFileFormatResult)
                                    {
                                        EJdataTable.Tables[0].ImportRow(dvr);
                                        SumMDV = SumMDV + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                    }
                                    EJdataTable.AcceptChanges();
                                    TotalCountMDV = "0";// TotalCountMDV = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                    TotalCountMDV = new string('0', 6 - TotalCountMDV.Length) + TotalCountMDV;
                                    MDVValue = "0";// MDVValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumMDV)));
                                    MDVValue = new string('0', 14 - MDVValue.Length) + MDVValue;
                                    // Presentments Count
                                    arryRow[t] = TotalCountMDV;
                                    t = t + 1;

                                    // Presentments Sum
                                    arryRow[t] = MDVValue;
                                    t = t + 1;

                                    ////UnPaids
                                    EJdataTable.Tables[0].Rows.Clear();
                                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("VOUCHERCODE ='03' AND  RETURNCODE <> '00' AND TRXTYPE='OC' AND TOBANK ='" + ToBank + "'");
                                    foreach (DataRow dvr in drHeaderFileFormatResult)
                                    {
                                        EJdataTable.Tables[0].ImportRow(dvr);
                                        SumunpaidMDV = SumunpaidMDV + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                    }
                                    EJdataTable.AcceptChanges();
                                    TotalUnpaidCountMDV = "0";// = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                    TotalUnpaidCountMDV = new string('0', 6 - TotalUnpaidCountMDV.Length) + TotalUnpaidCountMDV;
                                    MDVUnpaidValue = "0";// MDVUnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumunpaidMDV)));
                                    MDVUnpaidValue = new string('0', 14 - MDVUnpaidValue.Length) + MDVUnpaidValue;
                                    // UnPaid Count
                                    arryRow[t] = TotalUnpaidCountMDV;
                                    t = t + 1;

                                    // UnPaid Sum
                                    arryRow[t] = MDVUnpaidValue;
                                    t = t + 1;

                                    // Filler
                                    arryRow[t] = new string('0', 32);
                                    t = t + 1;
                                    //Reconstruct here
                                    Data = string.Empty;
                                    for (m = 0; m < arryRow.Length; m++)
                                    {
                                        Data = Data + (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
                                    }
                                    t = 0;
                                    dt.Rows.Add(Data);
                                    //BankID    CRNo    CRAmount    DRNo               DrAmt    MDVNo   MDVAmt  DRUnpaidNo              DRUnpaidAmt         DiscNo  DiscAmt     CrUnpaidNo  CRUnPaidAmt     Data        Date            Currency    TrxType     FileDate
                                    SettlementListingDt.Tables[0].Rows.Add(ToBank, 0, 0, 0, 0, BRBase.BRBaseConvert.ConvertToInt32(TotalCountMDV), SumMDV, BRBase.BRBaseConvert.ConvertToInt32(TotalUnpaidCountMDV), SumunpaidMDV, 0, 0, 0, 0, Data, WDate, "62", "O", FDate);



                                    SumCheques = 0;
                                    SumChq = string.Empty;
                                    SumCredits = 0;
                                    SumCr = string.Empty;
                                    SumDebits = 0;
                                    SumDr = string.Empty;
                                    UnpaidValue = "0";
                                    SumUnpaidCheques = 0;
                                    SumUnChq = string.Empty;
                                    SumunpaidCredits = 0;
                                    SumUnCr = string.Empty;
                                    SumUnpaidDebits = 0;
                                    SumUnDr = string.Empty;
                                    SumDiscCheques = 0;
                                    SumDiscChq = string.Empty;
                                    TotalCountCheques = "0";
                                    TotalCountDebits = "0";
                                    TotalCountCredits = "0";
                                    TotalUnpaidCountCheques = "0";
                                    TotalUnpaidCountDebits = "0";
                                    TotalUnpaidCountCredits = "0";
                                    TotalDiscCountCheques = "0";
                                    MDVValue = "0";
                                    TotalCountMDV = "0";
                                    SumunpaidMDV = 0;
                                    SumMDV = 0;
                                    TotalUnpaidCountMDV = "0";
                                    MDVUnpaidValue = "0";
                                }
                            }
                            else
                            {
                                for (Int32 n = 0; n < dsClearingBanks.Tables[0].Rows.Count; n++)
                                {
                                    ToBank = dsClearingBanks.Tables[0].Rows[n]["BankID"].ToString().Trim();
                                    string OldBankID = ToBank;
                                    arryRow = new Object[9];
                                    RecordType = "06";
                                    // RecordType
                                    arryRow[t] = RecordType;
                                    t = t + 1;

                                    // ToBank
                                    arryRow[t] = OldBankID;
                                    t = t + 1;

                                    //// Currency
                                    //arryRow[t] = Curr;
                                    //t = t + 1;

                                    // Presentments
                                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("(SERIALNUMBER IS NOT NULL OR SERIALNUMBER <> 0) AND CHEQUEDIGIT IS NOT NULL  AND RETURNCODE IN ('00','17') AND VOUCHERCODE NOT IN('02','40') AND TOBANK ='" + ToBank + "' AND TRXTYPE='OC'");// AND SUBSTRING(FROMBRANCH,3,2) ='" + ClearingCenter + "' ");
                                    foreach (DataRow dvr in drHeaderFileFormatResult)
                                    {
                                        EJdataTable.Tables[0].ImportRow(dvr);
                                        SumCheques = SumCheques + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                    }
                                    EJdataTable.AcceptChanges();
                                    TotalCountCheques = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                    TotalCountCheques = new string('0', 6 - TotalCountCheques.Length) + TotalCountCheques;
                                    // Pramod
                                    //Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumCheques)));
                                    if (Curr == "0")
                                    {
                                        Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(SumCheques)));
                                    }
                                    else
                                    {
                                        Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(SumCheques), "60"));
                                    }
                                    Value = new string('0', 15 - Value.Length) + Value;
                                    EJdataTable.Tables[0].Rows.Clear();

                                    // Presentments Count
                                    arryRow[t] = TotalCountCheques;
                                    t = t + 1;

                                    // Presentments Sum
                                    arryRow[t] = Value;
                                    t = t + 1;

                                    // Discrepancy
                                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select(" CHEQUEDIGIT IS NOT NULL AND RETURNCODE = '00' AND VOUCHERCODE IN ('02') AND TOBANK ='" + ToBank + "' AND TRXTYPE='OC'");//  AND SUBSTRING(FROMBRANCH,3,2) ='" + ClearingCenter + "' ");
                                    foreach (DataRow dvr in drHeaderFileFormatResult)
                                    {
                                        EJdataTable.Tables[0].ImportRow(dvr);
                                        SumDiscCheques = SumDiscCheques + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                    }
                                    EJdataTable.AcceptChanges();
                                    TotalDiscCountCheques = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                    TotalDiscCountCheques = new string('0', 6 - TotalDiscCountCheques.Length) + TotalDiscCountCheques;
                                    // Pramod
                                    //SumDiscChq = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumDiscCheques)));
                                    if (Curr == "0")
                                    {
                                        SumDiscChq = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(SumDiscCheques)));
                                    }
                                    else
                                    {
                                        SumDiscChq = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(SumDiscCheques), "60"));
                                    }
                                    SumDiscChq = new string('0', 14 - SumDiscChq.Length) + SumDiscChq;
                                    EJdataTable.Tables[0].Rows.Clear();

                                    // Discrepancy Count Discrepancy
                                    arryRow[t] = TotalDiscCountCheques;
                                    t = t + 1;

                                    // Discrepancy Sum Discrepancy
                                    arryRow[t] = "+" + SumDiscChq;
                                    t = t + 1;

                                    // Unpaid
                                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("CHEQUEDIGIT IS NOT NULL AND RETURNCODE NOT IN ('00','17') AND VOUCHERCODE NOT IN('02','04','05','40') AND TOBANK ='" + ToBank + "' AND TRXTYPE='OC'");//  AND SUBSTRING(FROMBRANCH,3,2) ='" + ClearingCenter + "' ");
                                    foreach (DataRow dvr in drHeaderFileFormatResult)
                                    {
                                        EJdataTable.Tables[0].ImportRow(dvr);
                                        SumUnpaidCheques = SumUnpaidCheques + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                    }
                                    EJdataTable.AcceptChanges();
                                    TotalUnpaidCountCheques = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                    TotalUnpaidCountCheques = new string('0', 6 - TotalUnpaidCountCheques.Length) + TotalUnpaidCountCheques;
                                    //Pramod
                                    //UnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumUnpaidCheques)));
                                    if (Curr == "0")
                                    {
                                        UnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(SumUnpaidCheques)));
                                    }
                                    else
                                    {
                                        UnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(SumUnpaidCheques), "60"));
                                    }
                                    UnpaidValue = new string('0', 15 - UnpaidValue.Length) + UnpaidValue;
                                    EJdataTable.Tables[0].Rows.Clear();

                                    // Unpaid Count Discrepancy
                                    arryRow[t] = TotalUnpaidCountCheques;
                                    t = t + 1;

                                    // Unpaid Sum Discrepancy
                                    arryRow[t] = UnpaidValue;
                                    t = t + 1;

                                    // Filler
                                    arryRow[t] = new string('0', 11);
                                    t = t + 1;

                                    //Reconstruct here
                                    Data = string.Empty;
                                    for (m = 0; m < arryRow.Length; m++)
                                    {
                                        Data = Data + (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
                                    }
                                    t = 0;
                                    dt.Rows.Add(Data);
                                    //BankID    CRNo    CRAmount    DRNo               DrAmt    MDVNo   MDVAmt  DRUnpaidNo              DRUnpaidAmt         DiscNo  DiscAmt     CrUnpaidNo  CRUnPaidAmt     Data        Date            Currency    TrxType     FileDate
                                    SettlementListingDt.Tables[0].Rows.Add(ToBank, 0, 0, BRBase.BRBaseConvert.ConvertToInt32(TotalCountCheques), SumCheques, 0, 0, BRBase.BRBaseConvert.ConvertToInt32(TotalUnpaidCountCheques), SumUnpaidCheques, 0, 0, 0, 0, Data, WDate, "0", "O", FDate);


                                    // 07
                                    arryRow = new Object[7];
                                    t = 0;
                                    RecordType = "07";
                                    // RecordType
                                    arryRow[t] = RecordType;
                                    t = t + 1;

                                    // ToBank
                                    arryRow[t] = OldBankID;
                                    t = t + 1;

                                    //// Currency
                                    //arryRow[t] = Curr;
                                    //t = t + 1;

                                    //Presentments
                                    EJdataTable.Tables[0].Rows.Clear();
                                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("VOUCHERCODE ='40' AND RETURNCODE = '00' AND TOBANK ='" + ToBank + "' AND TRXTYPE='OD'");//  AND SUBSTRING(FROMBRANCH,3,2) ='" + ClearingCenter + "' ");
                                    foreach (DataRow dvr in drHeaderFileFormatResult)
                                    {
                                        EJdataTable.Tables[0].ImportRow(dvr);
                                        SumDebits = SumDebits + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                    }
                                    EJdataTable.AcceptChanges();
                                    TotalCountDebits = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                    TotalCountDebits = new string('0', 6 - TotalCountDebits.Length) + TotalCountDebits;
                                    //Pramod
                                    //Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumDebits)));
                                    if (Curr == "0")
                                    {
                                        Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(SumDebits)));
                                    }
                                    else
                                    {
                                        Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(SumDebits), "60"));
                                    }
                                    Value = new string('0', 15 - Value.Length) + Value;
                                    EJdataTable.Tables[0].Rows.Clear();

                                    // Presentments Count
                                    arryRow[t] = TotalCountDebits;
                                    t = t + 1;

                                    // Presentments Sum
                                    arryRow[t] = Value;
                                    t = t + 1;

                                    //UnPaid
                                    EJdataTable.Tables[0].Rows.Clear();
                                    drHeaderFileFormatResult = null;
                                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("VOUCHERCODE ='40' AND RETURNCODE <> '00' AND TOBANK ='" + ToBank + "' AND TRXTYPE='OD'");//  AND SUBSTRING(FROMBRANCH,3,2) ='" + ClearingCenter + "' ");
                                    foreach (DataRow dvr in drHeaderFileFormatResult)
                                    {
                                        EJdataTable.Tables[0].ImportRow(dvr);
                                        SumUnpaidDebits = SumUnpaidDebits + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                    }
                                    EJdataTable.AcceptChanges();
                                    TotalUnpaidCountDebits = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                    TotalUnpaidCountDebits = new string('0', 6 - TotalUnpaidCountDebits.Length) + TotalUnpaidCountDebits;
                                    // Pramod
                                    //UnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumUnpaidDebits)));
                                    if (Curr == "0")
                                    {
                                        UnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(SumUnpaidDebits)));
                                    }
                                    else
                                    {
                                        UnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(SumUnpaidDebits), "60"));
                                    }
                                    UnpaidValue = new string('0', 15 - UnpaidValue.Length) + UnpaidValue;
                                    EJdataTable.Tables[0].Rows.Clear();

                                    // UnPaid Count
                                    arryRow[t] = TotalUnpaidCountDebits;
                                    t = t + 1;

                                    // UnPaid Sum
                                    arryRow[t] = UnpaidValue;
                                    t = t + 1;

                                    // Filler
                                    arryRow[t] = new string('0', 32);
                                    t = t + 1;

                                    //Reconstruct here
                                    Data = string.Empty;
                                    for (m = 0; m < arryRow.Length; m++)
                                    {
                                        Data = Data + (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
                                    }
                                    t = 0;
                                    dt.Rows.Add(Data);
                                    //BankID    CRNo    CRAmount    DRNo               DrAmt    MDVNo   MDVAmt  DRUnpaidNo              DRUnpaidAmt         DiscNo  DiscAmt     CrUnpaidNo  CRUnPaidAmt     Data        Date            Currency    TrxType     FileDate
                                    SettlementListingDt.Tables[0].Rows.Add(ToBank, 0, 0, BRBase.BRBaseConvert.ConvertToInt32(TotalCountDebits), SumDebits, 0, 0, BRBase.BRBaseConvert.ConvertToInt32(TotalUnpaidCountDebits), SumUnpaidDebits, 0, 0, 0, 0, Data, WDate, "0", "O", FDate);


                                    // 08
                                    arryRow = new Object[7];
                                    t = 0;
                                    RecordType = "08";
                                    // RecordType
                                    arryRow[t] = RecordType;
                                    t = t + 1;

                                    // ToBank
                                    arryRow[t] = OldBankID;
                                    t = t + 1;

                                    //// Currency
                                    //arryRow[t] = Curr;
                                    //t = t + 1;

                                    //Presentation
                                    EJdataTable.Tables[0].Rows.Clear();
                                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select(" RETURNCODE IN('00','90','97') AND TOBANK ='" + ToBank + "'   AND VOUCHERCODE <>'40' AND TRXTYPE='OD'");//  AND SUBSTRING(FROMBRANCH,3,2) ='" + ClearingCenter + "' ");
                                    foreach (DataRow dvr in drHeaderFileFormatResult)
                                    {
                                        EJdataTable.Tables[0].ImportRow(dvr);
                                        SumCredits = SumCredits + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                    }
                                    EJdataTable.AcceptChanges();
                                    TotalCountCredits = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                    TotalCountCredits = new string('0', 6 - TotalCountCredits.Length) + TotalCountCredits;
                                    //Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumCredits)));
                                    if (Curr == "0")
                                    {
                                        Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(SumCredits)));
                                    }
                                    else
                                    {
                                        Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(SumCredits), "60"));
                                    }
                                    Value = new string('0', 15 - Value.Length) + Value;
                                    // Presentments Count
                                    arryRow[t] = TotalCountCredits;
                                    t = t + 1;

                                    // Presentments Sum
                                    arryRow[t] = Value;
                                    t = t + 1;

                                    //UnPaids
                                    EJdataTable.Tables[0].Rows.Clear();
                                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("RETURNCODE NOT IN('00','90','97') AND TOBANK ='" + ToBank + "'  AND VOUCHERCODE <>'40'  AND TRXTYPE='OD'");//  AND SUBSTRING(FROMBRANCH,3,2) ='" + ClearingCenter + "' ");
                                    foreach (DataRow dvr in drHeaderFileFormatResult)
                                    {
                                        EJdataTable.Tables[0].ImportRow(dvr);
                                        SumunpaidCredits = SumunpaidCredits + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                    }
                                    EJdataTable.AcceptChanges();
                                    TotalUnpaidCountCredits = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                    TotalUnpaidCountCredits = new string('0', 6 - TotalUnpaidCountCredits.Length) + TotalUnpaidCountCredits;
                                    //Pramod
                                    //UnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumunpaidCredits)));
                                    if (Curr == "0")
                                    {
                                        UnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(SumunpaidCredits)));
                                    }
                                    else
                                    {
                                        UnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(SumunpaidCredits), "60"));
                                    }
                                    UnpaidValue = new string('0', 15 - UnpaidValue.Length) + UnpaidValue;
                                    // UnPaid Count
                                    arryRow[t] = TotalUnpaidCountCredits;
                                    t = t + 1;

                                    // UnPaid Sum
                                    arryRow[t] = UnpaidValue;
                                    t = t + 1;

                                    // Filler
                                    arryRow[t] = new string('0', 32);
                                    t = t + 1;

                                    //Reconstruct here
                                    Data = string.Empty;
                                    for (m = 0; m < arryRow.Length; m++)
                                    {
                                        Data = Data + (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
                                    }
                                    t = 0;
                                    dt.Rows.Add(Data);

                                    //BankID    CRNo    CRAmount    DRNo               DrAmt    MDVNo   MDVAmt  DRUnpaidNo              DRUnpaidAmt         DiscNo  DiscAmt     CrUnpaidNo  CRUnPaidAmt     Data        Date            Currency    TrxType     FileDate
                                    SettlementListingDt.Tables[0].Rows.Add(ToBank, BRBase.BRBaseConvert.ConvertToInt32(TotalCountCredits), SumCredits, 0, 0, 0, 0, 0, 0, 0, 0, BRBase.BRBaseConvert.ConvertToInt32(TotalUnpaidCountCredits), SumunpaidCredits, Data, WDate, "0", "O", FDate);



                                    // 09
                                    arryRow = new Object[7];
                                    t = 0;
                                    RecordType = "09";
                                    // RecordType
                                    arryRow[t] = RecordType;
                                    t = t + 1;

                                    // ToBank
                                    arryRow[t] = OldBankID;
                                    t = t + 1;

                                    //// Currency
                                    //arryRow[t] = Curr;
                                    //t = t + 1;

                                    //Presentation
                                    EJdataTable.Tables[0].Rows.Clear();
                                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("VOUCHERCODE ='03' AND RETURNCODE = '00' AND TRXTYPE='OC' AND TOBANK ='" + ToBank + "'");//  AND SUBSTRING(FROMBRANCH,3,2) ='" + ClearingCenter + "' ");
                                    foreach (DataRow dvr in drHeaderFileFormatResult)
                                    {
                                        EJdataTable.Tables[0].ImportRow(dvr);
                                        SumMDV = SumMDV + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                    }
                                    EJdataTable.AcceptChanges();
                                    TotalCountMDV = "0";// TotalCountMDV = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                    TotalCountMDV = new string('0', 6 - TotalCountMDV.Length) + TotalCountMDV;
                                    MDVValue = "0";// MDVValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumMDV)));
                                    MDVValue = new string('0', 15 - MDVValue.Length) + MDVValue;
                                    // Presentments Count
                                    arryRow[t] = TotalCountMDV;
                                    t = t + 1;

                                    // Presentments Sum
                                    arryRow[t] = MDVValue;
                                    t = t + 1;

                                    ////UnPaids
                                    EJdataTable.Tables[0].Rows.Clear();
                                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("VOUCHERCODE ='03' AND  RETURNCODE <> '00' AND TRXTYPE='OC' AND TOBANK ='" + ToBank + "'");//  AND SUBSTRING(FROMBRANCH,3,2) ='" + ClearingCenter + "' ");
                                    foreach (DataRow dvr in drHeaderFileFormatResult)
                                    {
                                        EJdataTable.Tables[0].ImportRow(dvr);
                                        SumunpaidMDV = SumunpaidMDV + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                                    }
                                    EJdataTable.AcceptChanges();
                                    TotalUnpaidCountMDV = "0";// = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                                    TotalUnpaidCountMDV = new string('0', 6 - TotalUnpaidCountMDV.Length) + TotalUnpaidCountMDV;
                                    MDVUnpaidValue = "0";// MDVUnpaidValue = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumunpaidMDV)));
                                    MDVUnpaidValue = new string('0', 15 - MDVUnpaidValue.Length) + MDVUnpaidValue;
                                    // UnPaid Count
                                    arryRow[t] = TotalUnpaidCountMDV;
                                    t = t + 1;

                                    // UnPaid Sum
                                    arryRow[t] = MDVUnpaidValue;
                                    t = t + 1;

                                    // Filler
                                    arryRow[t] = new string('0', 32);
                                    t = t + 1;

                                    //Reconstruct here
                                    Data = string.Empty;
                                    for (m = 0; m < arryRow.Length; m++)
                                    {
                                        Data = Data + (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
                                    }
                                    t = 0;
                                    dt.Rows.Add(Data);

                                    //BankID    CRNo    CRAmount    DRNo               DrAmt    MDVNo   MDVAmt  DRUnpaidNo              DRUnpaidAmt         DiscNo  DiscAmt     CrUnpaidNo  CRUnPaidAmt     Data        Date            Currency    TrxType     FileDate
                                    SettlementListingDt.Tables[0].Rows.Add(ToBank, 0, 0, 0, 0, BRBase.BRBaseConvert.ConvertToInt32(TotalCountMDV), SumMDV, BRBase.BRBaseConvert.ConvertToInt32(TotalUnpaidCountMDV), SumunpaidMDV, 0, 0, 0, 0, Data, WDate, "0", "O", FDate);


                                    SumCheques = 0;
                                    SumChq = string.Empty;
                                    SumCredits = 0;
                                    SumCr = string.Empty;
                                    SumDebits = 0;
                                    SumDr = string.Empty;
                                    UnpaidValue = "0";
                                    SumUnpaidCheques = 0;
                                    SumUnChq = string.Empty;
                                    SumunpaidCredits = 0;
                                    SumUnCr = string.Empty;
                                    SumUnpaidDebits = 0;
                                    SumUnDr = string.Empty;
                                    SumDiscCheques = 0;
                                    SumDiscChq = string.Empty;
                                    TotalCountCheques = "0";
                                    TotalCountDebits = "0";
                                    TotalCountCredits = "0";
                                    TotalUnpaidCountCheques = "0";
                                    TotalUnpaidCountDebits = "0";
                                    TotalUnpaidCountCredits = "0";
                                    TotalDiscCountCheques = "0";
                                    MDVValue = "0";
                                    TotalCountMDV = "0";
                                    SumunpaidMDV = 0;
                                    SumMDV = 0;
                                    TotalUnpaidCountMDV = "0";
                                    MDVUnpaidValue = "0";
                                }
                            }
                            break;
                    }
                }
            }
            catch (Exception e)
            {

            }
            if (SettlementListingDt.Tables[0].Rows.Count > 0)
            {
                //Save into the Database
                SettlementListingDt.Tables[0].Columns.Add("DetailRecords", typeof(string));
                DataSet dsTemp = new DataSet();
                dsTemp = (DataSet)SettlementListingDt.Copy();
                dsTemp.Relations.Clear();
                DataRow drSettlementListingDt = SettlementListingDt.Tables[0].Rows[0];
                drSettlementListingDt["DetailRecords"] = GetXmlTable(dsTemp);

                try
                {
                    using (IDbConnection connection = conn)
                    {
                        using (IDbTransaction trans = connection.BeginTransaction())
                        {
                            IDBHelper intfDBHelper = DBClient.GetDBHelper(usrInfo);
                            intfDBHelper.ExecuteScalarTypedParams(trans, "p_AddOutSettlement", drSettlementListingDt);
                            trans.Commit();
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }

            return dt;
        }

        public static string GetXmlTable(DataSet ds)
        {
            string strXMLTmp = String.Empty;
            strXMLTmp = ds.GetXml();
            MatchCollection mcStrat = Regex.Matches(strXMLTmp, @"<dt_\w*");
            MatchCollection mcEnd = Regex.Matches(strXMLTmp, @"</dt_\w*>");
            StringBuilder sb = new StringBuilder();
            if (mcStrat.Count != mcEnd.Count)
                throw new Exception("Wrong Input");
            else
            {
                string[] strArr = new string[mcEnd.Count];
                for (int i = 0; i < mcEnd.Count; i++)
                {
                    string temp = strXMLTmp.Substring(strXMLTmp.IndexOf(mcStrat[i].ToString()), ((strXMLTmp.IndexOf(mcEnd[i].ToString()) + mcEnd[i].ToString().Length) - strXMLTmp.IndexOf(mcStrat[i].ToString())));
                    strXMLTmp = strXMLTmp.Replace(temp, string.Empty);
                    sb.Append(Regex.Replace(temp, @" xmlns="".*""", string.Empty));
                }
            }
            return sb.ToString();
        }

        public static DataTable GenerateDiscrepancyBody(string ToBank, string WorkingDate, DS_trxClearing dstrxClearing, DS_ClearingFileFormat dsClearingFileFormat, String Banks)
        {
            string Data = string.Empty;
            string Value = "0";
            double SumDisc = 0;
            string Count = "0";
            DataTable dt = new DataTable();
            dt.Columns.Add("Data", typeof(string));
            Object[] arryRow = new object[2];
            Int32 t = 0;
            Int32 m;
            DataRow[] drHeaderFileFormatResult = null;
            DS_trxClearing EJdataTable = new DS_trxClearing();
            drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("RETURNCODE IN('02','03') AND ToBank '" + Banks + "'");
            if (drHeaderFileFormatResult.Length > 0)
            {
                foreach (DataRow dvr in drHeaderFileFormatResult)
                {
                    EJdataTable.Tables[0].ImportRow(dvr);
                    SumDisc = SumDisc + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                    Count = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                    Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(SumDisc)));
                    arryRow[t] = Count;
                    t = t + 1;
                }
                EJdataTable.Tables[0].AcceptChanges();
            }
            Data = string.Empty;
            if (arryRow[0] != null)
            {
                for (m = 0; m < arryRow.Length; m++)
                {
                    if (arryRow[m] != null)
                        Data = Data +  (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
                }
                dt.Rows.Add(Data);
            }
            return dt;
        }

        public static DataTable GenerateDDs(String ToBank, DS_trxClearing dstrxClearing, DS_ClearingFileFormat dsClearingFileFormat, DateTime WorkingDate, BRBase.UserInfo usrInfo, String Banks, IDbConnection conn, string MandateFileType, string[] conString)
        {
            Int32 t, FillerCount, Itemcount, Header;
            Int32 i, w;
            bool isUnpaid = false;
            StringCollection UnpaidColl = new StringCollection(); ;
            StringCollection UnpaidTrxrowIDColl = new StringCollection(); ;
            string UnpaidData = string.Empty;
            string sortOrder = " Start ASC";
            ArrayList FillerLength = new ArrayList();
            string EFTType = string.Empty;
            string Data = string.Empty;
            DataRow[] drHeaderFileFormatResult = null;
            DS_trxClearing EJdataTable = new DS_trxClearing();
            string ColumnFieldName = string.Empty;
            Int32 FileFormatValue = 0;
            char Filler;
            string TrxRowID="";
            DataTable dt = new DataTable();
            Int32 ValueLength = 0;
            ArrayList arr = new ArrayList();
            ArrayList arrCounter = new ArrayList();
            ArrayList arrEFTType = new ArrayList();
            bool FileFormatValueMandatoryLength = false;
            DS_ClearingFileFormat WorkingDataTable = new DS_ClearingFileFormat();
            string Value = string.Empty;
            drHeaderFileFormatResult = dsClearingFileFormat.Tables[0].Select("RecordType = 'DR' AND FileType='EFT'", sortOrder);
            dt.Columns.Add("TrxRowID", typeof(string));
            dt.Columns.Add("Text", typeof(string));
            dt.Columns.Add("ImageID", typeof(string));
            dt.Columns.Add("OurBranchID", typeof(string));
            string BankTO = string.Empty;
            Itemcount = 0;
            Header = 0;
            string dd = new string('0', 2 - BRBase.BRBaseConvert.ConvertToString(WorkingDate.Day).ToString().Length) + BRBase.BRBaseConvert.ConvertToString(WorkingDate.Day);
            string mm = new string('0', 2 - BRBase.BRBaseConvert.ConvertToString(WorkingDate.Month).Length) + BRBase.BRBaseConvert.ConvertToString(WorkingDate.Month);
            string yyyy = BRBase.BRBaseConvert.ConvertToString(WorkingDate.Year);
            string ddmmmyyyy = dd + mm + yyyy;
            Object[] arryRow = new Object[Itemcount];
           

            foreach (DataRow dvr in drHeaderFileFormatResult)
            {
                WorkingDataTable.Tables[0].ImportRow(dvr);
            }

            switch (MandateFileType.ToString())
            {
                case "NORMAL":
                    arrEFTType.Add("NORMAL");
                    arr.Add("RETURNCODE");
                    arr.Add("VOUCHERCODE");
                    arr.Add("CURRENCYCODE");
                    arr.Add("VALUE");
                    arr.Add("FILLER");
                    arr.Add("FROMBANK");
                    arr.Add("FROMBRANCH");
                    arr.Add("COLLECTIONACCOUNT");
                    arr.Add("TOBANK");
                    arr.Add("TOBRANCH");
                    arr.Add("TOACCOUNT");
                    arr.Add("TOBANK");
                    arr.Add("TOBRANCH");
                    arr.Add("ORIGINATORCODE");
                    arr.Add("PROCESSINGNO");
                    arr.Add("POLICYNUMBER1");
                    arr.Add("POLICYNUMBER2");
                    arr.Add("REMARKS");
                    arr.Add("PAYERSNAME");
                    arr.Add("FILLER");
                    FillerLength.Add("1");
                    FillerLength.Add("5");
                    Itemcount = 19;
                    DDFT = "00";
                    arrCounter.Add(Itemcount);
                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("VoucherCode = '40' AND ToBank " + Banks + "  AND isNull(RECORDTYPE,'00') = 00");
                    foreach (DataRow dvr in drHeaderFileFormatResult)
                    {
                        EJdataTable.Tables[0].ImportRow(dvr);
                    }
                    break;
                case "01":
                    arrEFTType.Add("01");
                    arr.Add("RECORDTYPE");
                    arr.Add("RETURNCODE");
                    arr.Add("CURRENCYCODE");
                    arr.Add("FVALUE");
                    arr.Add("FILLER");
                    arr.Add("CVALUE");
                    arr.Add("FILLER");
                    arr.Add("TOBANK");
                    arr.Add("TOBRANCH");
                    arr.Add("TOACCOUNT");
                    arr.Add("FROMBANK");
                    arr.Add("FROMBRANCH");
                    arr.Add("COLLECTIONACCOUNT");
                    arr.Add("FROMBANK");
                    arr.Add("FROMBRANCH");
                    arr.Add("ORIGINATORCODE");
                    arr.Add("ORIGINATORREFERENCECODE");
                    arr.Add("POLICYNUMBER1");
                    arr.Add("POLICYNUMBER2");
                    arr.Add("DUEDATE");
                    arr.Add("FREQUENCY");
                    arr.Add("EXPIRINGDATE");
                    arr.Add("PAYERSNAME");
                    FillerLength.Add("1");
                    FillerLength.Add("1");
                    DDFT ="01";
                    Itemcount = 23;
                    arrCounter.Add(Itemcount);
                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("VoucherCode = '40' AND ToBank " + Banks + "  AND RECORDTYPE = 01");
                    foreach (DataRow dvr in drHeaderFileFormatResult)
                    {
                        EJdataTable.Tables[0].ImportRow(dvr);
                    }
                break;

                case "02":
                    arrEFTType.Add("NEWDDMANDATEACKNOWLEDGEMENT");
                    arr.Add("RECORDTYPE");
                    arr.Add("RETURNCODE");
                    arr.Add("ORIGINATORCODE");
                    arr.Add("POLICYNUMBER1");
                    arr.Add("POLICYNUMBER2");
                    arr.Add("ORIGINATORREFERENCECODE");
                    DDFT ="02";
                    Itemcount = 6;
                    arrCounter.Add(Itemcount);
                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("VoucherCode = '40' AND ToBank " + Banks + " AND RECORDTYPE = 02");
                    foreach (DataRow dvr in drHeaderFileFormatResult)
                    {
                        EJdataTable.Tables[0].ImportRow(dvr);
                    }
                break;

                case "03":
                    arrEFTType.Add("DDMANDATEAMENDMENTREQUEST");
                    arr.Add("RECORDTYPE");
                    arr.Add("RETURNCODE");
                    arr.Add("CURRENCYCODE");
                    arr.Add("FVALUE");
                    arr.Add("FILLER");
                    arr.Add("CVALUE");
                    arr.Add("FILLER");
                    arr.Add("FROMBRANCH");
                    arr.Add("COLLECTIONACCOUNT");
                    arr.Add("TOBRANCH");
                    arr.Add("TOACCOUNT");
                    arr.Add("ORIGINATORREFERENCECODE");
                    arr.Add("DUEDATE");
                    arr.Add("FREQUENCY");
                    arr.Add("EXPIRINGDATE");
                    arr.Add("PAYERSNAME");
                    arr.Add("PROCESSINGNO");
                    DDFT ="03";
                    FillerLength.Add("1");
                    FillerLength.Add("1");
                    Itemcount = 17;
                    arrCounter.Add(Itemcount);
                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("VoucherCode = '40' AND ToBank " + Banks + "  AND RECORDTYPE = 03");
                    foreach (DataRow dvr in drHeaderFileFormatResult)
                    {
                        EJdataTable.Tables[0].ImportRow(dvr);
                    }
                break;

                case "04":
                    arrEFTType.Add("DDAMENDMENTACKNOWLEDGEMENT");
                    arr.Add("RECORDTYPE");
                    arr.Add("RETURNCODE");
                    arr.Add("ORIGINATORREFERENCECODE");
                    arr.Add("PROCESSINGNO");
                    DDFT ="04";
                    Itemcount = 4;
                    arrCounter.Add(Itemcount);
                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("VoucherCode = '40' AND ToBank " + Banks + "  AND RECORDTYPE = 04");
                    foreach (DataRow dvr in drHeaderFileFormatResult)
                    {
                        EJdataTable.Tables[0].ImportRow(dvr);
                    }
                break;

                case "05":
                    arrEFTType.Add("CANCELDDMANDATEINSTRUCTION");
                    arr.Add("RECORDTYPE");
                    arr.Add("RETURNCODE");
                    arr.Add("DUEDATE");
                    arr.Add("PROCESSINGNO");
                    DDFT ="05";
                    Itemcount = 4;
                    arrCounter.Add(Itemcount);
                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("VoucherCode = '40' AND ToBank " + Banks + " AND RECORDTYPE = 05");
                    foreach (DataRow dvr in drHeaderFileFormatResult)
                    {
                        EJdataTable.Tables[0].ImportRow(dvr);
                    }
                break;
                                        
                case  "06":
                    arrEFTType.Add("CANCELDDMANDATEREQUEST");
                    arr.Add("RECORDTYPE");
                    arr.Add("RETURNCODE");
                    arr.Add("DUEDATE");
                    arr.Add("PROCESSINGNO");
                    DDFT ="06";
                    Itemcount = 4;
                    arrCounter.Add(Itemcount);
                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("VoucherCode = '40' AND ToBank " + Banks + "  AND RECORDTYPE = 06");
                    foreach (DataRow dvr in drHeaderFileFormatResult)
                    {
                        EJdataTable.Tables[0].ImportRow(dvr);
                    }
                    break;
                case "99":
                    arrEFTType.Add("LEGACYDATA");
                    arr.Add("ORIGINATORREFERENCECODE");
                    arr.Add("FILLER");
                    arr.Add("FREQUENCY");
                    DDFT = "99";
                    Itemcount = 3;
                    arrCounter.Add(Itemcount);
                    drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("VoucherCode = '40' AND ToBank " + Banks + "  AND RECORDTYPE = 99");
                    foreach (DataRow dvr in drHeaderFileFormatResult)
                    {
                        EJdataTable.Tables[0].ImportRow(dvr);
                    }
                break;

            }
           
           

            t = 0;
            FillerCount = 0;
            try
            {
                foreach (DataColumn cvr in WorkingDataTable.Tables[0].Columns)
                {
                    switch (cvr.ColumnName.ToString().ToUpper())
                    {
                        case "FIELDNAME":
                            for (Int32 p = 0; p < arrEFTType.Count; p++)
                            {
                                foreach (DataRow DataDvr in EJdataTable.Tables[0].Rows)
                                {
                                arryRow = new Object[BRBaseConvert.ConvertToInt32(arrCounter[p])];
                                t = 0;
                                FillerCount = 0;
                                    for (w = 0; w < arr.Count; w++)
                                    {

                                        if (conn.State != ConnectionState.Open)
                                        {
                                            //MessageBox.Show(conn.State.ToString() + " - > " + ColumnFieldName);
                                            conn = ReGetConnection(conString);
                                        }

                                        ColumnFieldName = arr[w].ToString();
                                        DataRow[] FieldRow = WorkingDataTable.Tables[0].Select("FIELDNAME = '" + ColumnFieldName + "' AND RecordType = 'DR' AND FileType='EFT'");
                                        try
                                        {
                                            FileFormatValue = BRBase.BRBaseConvert.ConvertToInt32(FieldRow[0]["Length"].ToString());
                                        }
                                        catch (Exception ex)
                                        {
                                            FileFormatValue = 0;
                                        }
                                        FieldRow = null;
                                        FieldRow = WorkingDataTable.Tables[0].Select("FIELDNAME = '" + ColumnFieldName + "' AND RecordType = 'DR' AND FileType='EFT'");
                                        try
                                        {
                                            FileFormatValueMandatoryLength = BRBase.BRBaseConvert.ConvertToBoolean(FieldRow[0]["IsLengthMandatoryFieldSize"].ToString());
                                        }
                                        catch (Exception ex)
                                        {
                                            FileFormatValueMandatoryLength = false;
                                        }
                                        FieldRow = null;
                                        FieldRow = WorkingDataTable.Tables[0].Select("FIELDNAME = '" + ColumnFieldName + "' AND RecordType = 'DR' AND FileType='EFT'");

                                        try
                                        {
                                            Filler = BRBase.BRBaseConvert.ConvertToChar(FieldRow[0]["Filler"].ToString());
                                        }
                                        catch (Exception ex)
                                        {
                                            Filler = BRBase.BRBaseConvert.ConvertToChar('0'); 
                                        }
                                       
                                        TrxRowID = BRBase.BRBaseConvert.ConvertToString(DataDvr["TrxRowID"]);
                                        switch (arr[w].ToString().ToUpper())
                                        {
                                            case "RECORDTYPE":
                                                arryRow[t] = DDFT;
                                                t = t + 1;
                                                break;
                                            case "CURRENCYCODE":
                                                arryRow[t] = "00";
                                                t = t + 1;
                                                break;
                                            case "RETURNCODE":
                                            case "TOBRANCH":
                                            case "BENEFICIARYNAME":
                                            case "FROMBRANCH":
                                            case "NAMEOFEMPLOYEE":
                                            case "PINNUMBER":
                                            case "VATSERIALNO":
                                            case "VATNUMBER":
                                            case "MONTHOFPAYMENT":
                                            case "REGISTEREDVATNAME":
                                            case "TYPEOFVATRETURN":
                                            case "PAYMENTTYPE":
                                            case "ESLIPNUMBER"://NewDD
                                                Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName]).Trim();
                                                ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Trim().Length);
                                                if (FileFormatValueMandatoryLength)
                                                {
                                                    Value = new string(Filler, FileFormatValue - ValueLength) + Value;
                                                }

                                                arryRow[t] = Value;
                                                t = t + 1;
                                                break;
                                            case "FREQUENCY"://NewDD
                                                if (DDFT != "99")
                                                {
                                                    Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName]).Trim();
                                                    ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Trim().Length);
                                                    if (FileFormatValueMandatoryLength)
                                                    {
                                                        Value = new string(Filler, FileFormatValue - ValueLength) + Value;
                                                    }
                                                }
                                                else
                                                {
                                                    Value = BRBase.BRBaseConvert.ConvertToString(DataDvr["RETURNCODE"]).Trim();
                                                    if (Value == "00")
                                                    {
                                                        Value = "1";
                                                    }
                                                    else
                                                    {
                                                        Value = "0";
                                                    }
                                                }
                                                arryRow[t] = Value;
                                                t = t + 1;
                                                break;
                                            case "PAYERSNAME"://NewDD
                                                Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName]);
                                                ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Length);
                                                if (FileFormatValueMandatoryLength)
                                                {
                                                    Value = new string(' ', 35 - ValueLength) + Value;
                                                }

                                                arryRow[t] = Value;
                                                t = t + 1;
                                                break;
                                            case "TOBANK":
                                                Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName].ToString().Trim());
                                                ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Trim().Length);
                                                Value = new string(Filler, FileFormatValue - ToBank.Length) + ToBank;
                                                BankTO = BRBase.BRBaseConvert.ConvertToString(Value);
                                                arryRow[t] = Value;
                                                t = t + 1;
                                                break;
                                            case "ORIGINATORREFERENCECODE":
                                                //BRBase.BRDataSet dsUniqueClearingID = new BRBase.BRDataSet();
                                                // string UniqueID = string.Empty;
                                                // ClearingUniversalMethod(usrInfo, "p_GetUniqueClearingID", out dsUniqueClearingID, BRBase.BRModule.InwardFileImportation, conn, new object[] { "BrDataSet" }, new object[] { FileFormatValue });
                                                // if (dsUniqueClearingID.Tables.Count > 0)
                                                // {
                                                //     if (dsUniqueClearingID.Tables[0].Rows.Count > 0)
                                                //     {
                                                //         UniqueID = dsUniqueClearingID.Tables[0].Rows[0]["UniqueID"].ToString();
                                                //     }
                                                // }
                                                // else
                                                // {
                                                //     UniqueID = new string('0',15);
                                                // }
                                                // dsUniqueClearingID.Tables[0].Clear();
                                                // Value = BRBase.BRBaseConvert.ConvertToString(UniqueID);
                                                // ValueLength = BRBase.BRBaseConvert.ConvertToInt32(UniqueID.ToString().Length);
                                                // if (FileFormatValueMandatoryLength)
                                                // {
                                                //     Value = new string(Filler, FileFormatValue - ValueLength) + Value;
                                                // }
                                                if (DDFT == "01")
                                                {
                                                    Value = Convert.ToString(Guid.NewGuid().ToString("N").ToUpper());
                                                }
                                                else
                                                {
                                                    Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName]).ToUpper();
                                                    ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Length);
                                                    if (DDFT != "00")
                                                    {
                                                        FileFormatValue = 32;
                                                    }
                                                    
                                                    if (FileFormatValueMandatoryLength)
                                                    {
                                                        Value = new string(Filler, FileFormatValue - ValueLength) + Value;
                                                    }
                                                }
                                                arryRow[t] = Value;
                                                t = t + 1;
                                                break;
                                            case "EXPIRINGDATE"://NewDD
                                            case "DUEDATE"://NewDD
                                            case "PAYMENTDATE":
                                                string Ddd = new string('0', 2 - BRBase.BRBaseConvert.ConvertToString(((System.DateTime)(DataDvr[ColumnFieldName])).Day).Length) + BRBase.BRBaseConvert.ConvertToString(((System.DateTime)(DataDvr[ColumnFieldName])).Day);
                                                string Dmm = new string('0', 2 - BRBase.BRBaseConvert.ConvertToString(((System.DateTime)(DataDvr[ColumnFieldName])).Month).Length) + BRBase.BRBaseConvert.ConvertToString(((System.DateTime)(DataDvr[ColumnFieldName])).Month);
                                                string Dyyyy = BRBase.BRBaseConvert.ConvertToString(((System.DateTime)(DataDvr[ColumnFieldName])).Year);
                                                string Dddmmmyyyy = Ddd + Dmm + Dyyyy;
                                                Value = Dddmmmyyyy;
                                                arryRow[t] = Value;
                                                t = t + 1;
                                                break;
                                            case "FROMBANK":
                                                arryRow[t] = usrInfo.strBank;
                                                t = t + 1;
                                                break;
                                            case "TOACCOUNT":
                                                Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName]);
                                                ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Length);
                                                if (FileFormatValueMandatoryLength)
                                                {
                                                    Value = new string(Filler, FileFormatValue - ValueLength) + Value;
                                                }
                                                arryRow[t] = Value;
                                                t = t + 1;
                                                break;

                                            case "COLLECTIONACCOUNT":
                                            case "PACCOUNT":
                                                Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName]);
                                                ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Length);
                                                if (FileFormatValueMandatoryLength)
                                                {
                                                    Value = new string(Filler, FileFormatValue - ValueLength) + Value;
                                                }
                                                arryRow[t] = Value;
                                                t = t + 1;

                                                break;
                                            case "PROCESSINGNO":

                                                if (DDFT == "03")
                                                {
                                                    Value = Convert.ToString(Guid.NewGuid().ToString("N").ToUpper());
                                                }
                                                else
                                                {
                                                    if (DDFT != "00")
                                                    {
                                                        FileFormatValue = 32;
                                                    }
                                                    Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName]).ToUpper();
                                                    ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Length);
                                                    if (FileFormatValueMandatoryLength)
                                                    {
                                                        Value = new string(Filler, FileFormatValue - ValueLength) + Value;
                                                    }
                                                    if (ColumnFieldName.ToString().ToUpper() == "VALUE")
                                                    {
                                                        Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(DataDvr[ColumnFieldName])));
                                                        Value = new string(Filler, FileFormatValue - ValueLength) + Value;
                                                    }
                                                }
                                                arryRow[t] = Value;
                                                t = t + 1;
                                                break;
                                            case "VALUE":
                                            case "CVALUE":
                                            case "FVALUE":
                                            case "COMMISSIONCHARGED":
                                                //ColumnFieldName = "VALUE";
                                                Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(DataDvr[ColumnFieldName])));
                                                ValueLength = BRBase.BRBaseConvert.ConvertToInt32(Value.ToString().Length);
                                                if (FileFormatValueMandatoryLength)
                                                {
                                                    Value = new string(Filler, FileFormatValue - ValueLength) + Value;
                                                }
                                                arryRow[t] = Value;
                                                t = t + 1;
                                                break;
                                            case "ORIGINATORCODE":
                                                Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName]);
                                                ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Length);
                                                if (FileFormatValueMandatoryLength)
                                                {
                                                    Value = new string(Filler, FileFormatValue - ValueLength) + Value;
                                                }
                                                if (ColumnFieldName.ToString().ToUpper() == "VALUE")
                                                {
                                                    Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(DataDvr[ColumnFieldName])));
                                                    Value = new string(Filler, FileFormatValue - ValueLength) + Value;
                                                }
                                                arryRow[t] = Value;
                                                t = t + 1;
                                                break;
                                            case "POLICYNUMBER1":
                                            case "POLICYNUMBER2":
                                                Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName]);
                                                ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Length);
                                                if (FileFormatValueMandatoryLength)
                                                {
                                                    Value = Value + new string(' ', FileFormatValue - ValueLength);
                                                }
                                                arryRow[t] = Value;
                                                t = t + 1;
                                                break;
                                            case "REMARKS":
                                                Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName]);
                                                ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Length);
                                                if (FileFormatValueMandatoryLength)
                                                {
                                                    Value = new string(' ', FileFormatValue - ValueLength) + Value;
                                                }
                                                arryRow[t] = Value;
                                                t = t + 1;
                                                break;
                                            case "DDIMAGESIZE"://NewDD
                                                //Image Size goes in Here
                                                arryRow[t] = "";
                                                t = t + 1;
                                                break;
                                            case "DDIMAGE"://NewDD
                                                //Image goes in Here
                                                arryRow[t] = "";
                                                t = t + 1;
                                                break;
                                            case "REFNO":

                                                    Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName]);
                                                    ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Length);
                                                    if (FileFormatValueMandatoryLength)
                                                    {
                                                        Value = new string(' ', FileFormatValue - ValueLength) + Value;
                                                    }
                                                arryRow[t] = Value;
                                                t = t + 1;
                                                break;
                                            case "FILLER":
                                                if (DDFT == "99")
                                                {
                                                    Value = "|";
                                                }
                                                else
                                                {
                                                    Value = new string('0', BRBase.BRBaseConvert.ConvertToInt32(FillerLength[FillerCount]));
                                                }
                                                FillerCount += 1;
                                                arryRow[t] = Value;
                                                t = t + 1;
                                                break;
                                        }
                                    }



                                    Data = string.Empty;
                                    if (Header == 0)
                                    {
                                        BRBase.BRDataSet dsUniqueClearingID = new BRBase.BRDataSet();
                                        string UniqueID = string.Empty;
                                        if (conn.State != ConnectionState.Open)
                                        {
                                            conn = ReGetConnection(conString);
                                        }
                                        if (DDFT != "99")
                                        {
                                            ClearingUniversalMethod(usrInfo, "p_GetUniqueClearingID", out dsUniqueClearingID, BRBase.BRModule.InwardFileImportation, conn, new object[] { "BrDataSet" }, new object[] { 9 });
                                            if (dsUniqueClearingID.Tables.Count > 0)
                                            {
                                                if (dsUniqueClearingID.Tables[0].Rows.Count > 0)
                                                {
                                                    UniqueID = dsUniqueClearingID.Tables[0].Rows[0]["UniqueID"].ToString();
                                                    string x = UniqueID.Substring(UniqueID.Length - 1, 1);
                                                    if (x == "0")
                                                    {
                                                        UniqueID = UniqueID.Substring(0, UniqueID.Length - 1) + "1";
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                UniqueID = new string('0', 9);
                                            }
                                        }
                                        if (DDFT != "99")
                                        {
                                            UniqueID = new string('0', 9 - UniqueID.Length) + UniqueID;
                                            if (DDFT != "00")
                                            {
                                                Data = "18" + DDFT + ddmmmyyyy + usrInfo.strBank + usrInfo.strBank + ToBank + ToBank;
                                            }
                                            else
                                            {
                                                Data = "185" + ddmmmyyyy + usrInfo.strBank + "00000" + UniqueID + "1" + new string('0', 161);
                                            }
                                            dt.Rows.Add('0', Data);
                                        }
                                        Header += 1;
                                    }
                                    Data = string.Empty;
                                    if (arryRow[0].ToString() != "00" && DDFT == "00")
                                    {
                                        isUnpaid = true;
                                    }
                                    if (arryRow[0] != null)
                                    {
                                        for (Int32 m = 0; m < arryRow.Length; m++)
                                        {
                                            if (isUnpaid == true)
                                            {
                                                isUnpaid = true;
                                                UnpaidData = UnpaidData + (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
                                            }
                                            else
                                            {
                                                Data = Data + (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
                                            }
                                        }
                                        DataTable ClearingFileFormat = new DataTable();
                                        dsClearingFileFormat = new DS_ClearingFileFormat();

                                        
                                        if (Data != string.Empty)
                                        {
                                            dt.Rows.Add(DataDvr["TrxRowID"].ToString().Trim(), Data, DataDvr["DRN"].ToString().Trim(), DataDvr["FromBranch"].ToString());
                                        }
                                        if (isUnpaid == true && DDFT == "00")
                                        {
                                            if (conn.State != ConnectionState.Open)
                                            {
                                                conn = ReGetConnection(conString);
                                            }
                                            BRBase.BRDataSet dsOriginalUnpaidTrx = new BRBase.BRDataSet();
                                            //First I retrieve what came in, so that i just change the returncode
                                            ClearingUniversalMethod(usrInfo, "p_GetTheOriginalUnpaidTrx", out dsOriginalUnpaidTrx, BRBase.BRModule.GenerateClearingFile, conn, new object[] { "BrDataSet" }, new object[] { TrxRowID });
                                            if (dsOriginalUnpaidTrx.Tables.Count > 0)
                                            {
                                                if (dsOriginalUnpaidTrx.Tables[0].Columns.Contains("Data"))
                                                {
                                                    if (dsOriginalUnpaidTrx.Tables[0].Rows[0]["Data"].ToString() != "NIL" && dsOriginalUnpaidTrx.Tables[0].Rows[0]["Data"].ToString() != "")
                                                    {
                                                        UnpaidData = dsOriginalUnpaidTrx.Tables[0].Rows[0]["Data"].ToString();
                                                        UnpaidData = UnpaidData.Substring(2);
                                                        UnpaidData = arryRow[0].ToString() + UnpaidData;
                                                    }
                                                }
                                            }
                                            UnpaidColl.Add(UnpaidData);
                                            UnpaidTrxrowIDColl.Add(TrxRowID);
                                        }
                                        Data = string.Empty;
                                        UnpaidData = string.Empty;
                                        isUnpaid = false;
                                    }
                                    //t = 0;
                                    //arryRow = new Object[Itemcount];
                                    //FillerCount = 0;
                                    //Value = string.Empty;
                                    //EJdataTable = new DS_trxClearing();
                                }
                                break;
                            }
                            break;
                    }
                    //FillerLength = new ArrayList();
                    //arr = new ArrayList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            if (dt.Rows.Count > 0)
            {
                if (UnpaidColl.Count > 0)
                {
                    Data = "2272" + new string('0', 185);
                    dt.Rows.Add('0', Data);
                    Data = string.Empty;
                    for (Int32 m = 0; m < UnpaidColl.Count; m++)
                    {
                        dt.Rows.Add(UnpaidTrxrowIDColl[m], UnpaidColl[m]);
                    }
                }
                if (DDFT != "99")
                {
                    if (DDFT != "00")
                    {
                        Data = "19" + usrInfo.strBank + usrInfo.strBank + new string('0', 6 - dt.Rows.Count.ToString().Length) + (dt.Rows.Count - 1).ToString() + new string('0', 8);
                    }
                    else
                    {
                        Data = "19" + usrInfo.strBank + new string('0', 185);
                    }
                    UnpaidColl.Clear();
                    dt.Rows.Add('0', Data);
                }
            }
            return dt;
        }
        public static Byte[] string2Byte(string Value)
        {
            return Convert.FromBase64String(Value);
        }
        public static DataTable GenerateDiscrepancyBodyUG(string ToBank, string WorkingDate, DS_trxClearing dstrxClearing, DS_ClearingFileFormat dsClearingFileFormat, String Banks)
        {
            string Data = string.Empty;
            string Value = "0";
            double SumDisc = 0;
            string Count = "0";
            DataTable dt = new DataTable();
            dt.Columns.Add("Data", typeof(string));
            Object[] arryRow = new object[2];
            Int32 t = 0;
            Int32 m;
            DataRow[] drHeaderFileFormatResult = null;
            DS_trxClearing EJdataTable = new DS_trxClearing();
            drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("RETURNCODE IN('02','03') AND ToBank " + Banks + "");
            if (drHeaderFileFormatResult.Length > 0)
            {
                foreach (DataRow dvr in drHeaderFileFormatResult)
                {
                    EJdataTable.Tables[0].ImportRow(dvr);
                    SumDisc = SumDisc + BRBase.BRBaseConvert.ConvertToDouble(dvr["Value"]);
                    Count = BRBase.BRBaseConvert.ConvertToString(EJdataTable.Tables[0].Rows.Count);
                    Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(SumDisc)));
                    arryRow[t] = Count;
                    t = t + 1;
                }
                EJdataTable.Tables[0].AcceptChanges();
            }
            Data = string.Empty;
            if (arryRow[0] != null)
            {
                for (m = 0; m < arryRow.Length; m++)
                {
                    if (arryRow[m] != null)
                        Data = Data + (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
                }
                dt.Rows.Add(Data);
            }
            return dt;
        }

        public static DataTable GenerateDDsUG(String ToBank, DS_trxClearing dstrxClearing, DS_ClearingFileFormat dsClearingFileFormat, DateTime WorkingDate, BRBase.UserInfo usrInfo, String Banks, IDbConnection conn)
        {
            Int32 t, FillerCount, Itemcount, Header;
            Int32 i, w;
            bool isUnpaid = false;
            StringCollection UnpaidColl = new StringCollection(); ;
            string UnpaidData = string.Empty;
            string sortOrder = " Start ASC";
            ArrayList FillerLength = new ArrayList();
            string EFTType = string.Empty;
            string Data = string.Empty;
            DataRow[] drHeaderFileFormatResult = null;
            DS_trxClearing EJdataTable = new DS_trxClearing();
            string ColumnFieldName = string.Empty;
            Int32 FileFormatValue = 0;
            char Filler;
            string TrxRowID = "";
            DataTable dt = new DataTable();
            Int32 ValueLength = 0;
            ArrayList arr = new ArrayList();
            bool FileFormatValueMandatoryLength = false;
            DS_ClearingFileFormat WorkingDataTable = new DS_ClearingFileFormat();
            string Value = string.Empty;
            drHeaderFileFormatResult = dsClearingFileFormat.Tables[0].Select("RecordType = 'DR' AND FileType='EFT'", sortOrder);
            dt.Columns.Add("Data", typeof(string));
            string BankTO = string.Empty;
            Itemcount = 0;
            Header = 0;
            string dd = new string('0', 2 - BRBase.BRBaseConvert.ConvertToString(WorkingDate.Day).ToString().Length) + BRBase.BRBaseConvert.ConvertToString(WorkingDate.Day);
            string mm = new string('0', 2 - BRBase.BRBaseConvert.ConvertToString(WorkingDate.Month).Length) + BRBase.BRBaseConvert.ConvertToString(WorkingDate.Month);
            string yyyy = BRBase.BRBaseConvert.ConvertToString(WorkingDate.Year);
            string ddmmmyyyy = dd + mm + yyyy;
            Object[] arryRow = new Object[Itemcount];

            foreach (DataRow dvr in drHeaderFileFormatResult)
            {
                WorkingDataTable.Tables[0].ImportRow(dvr);
            }
            EFTType = "NORMAL";
            arr.Add("RETURNCODE");
            arr.Add("VOUCHERCODE");
            arr.Add("VALUE");
            //arr.Add("FILLER");
            arr.Add("FROMBANK");
            arr.Add("FROMBRANCH");
            arr.Add("COLLECTIONACCOUNT");
            arr.Add("TOBANK");
            arr.Add("TOBRANCH");
            arr.Add("TOACCOUNT");
            arr.Add("TOBANK");
            arr.Add("ESLIPNUMBER");
            arr.Add("POLICYNUMBER1");
            arr.Add("BENEFICIARYNAME");
            arr.Add("ORIGINATORREFERENCECODE");
            arr.Add("FILLER");
            FillerLength.Add("9");
            Itemcount = 15;
            arryRow = new Object[Itemcount];
            drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("VoucherCode = '40' AND ToBank " + Banks + "");


            //MessageBox.Show("Imefika hapa - " + Banks);
            foreach (DataRow dvr in drHeaderFileFormatResult)
            {
                EJdataTable.Tables[0].ImportRow(dvr);
            }

            t = 0;
            int Counter = 0;
            FillerCount = 0;
            try
            {
                foreach (DataColumn cvr in WorkingDataTable.Tables[0].Columns)
                {
                    switch (cvr.ColumnName.ToString().ToUpper())
                    {
                        case "FIELDNAME":
                            foreach (DataRow DataDvr in EJdataTable.Tables[0].Rows)
                            {
                                switch (EFTType.ToString().ToUpper())
                                {
                                    case "NORMAL":
                                        for (w = 0; w < arr.Count; w++)
                                        {
                                            if (conn.State != ConnectionState.Open)
                                            {
                                                conn = GetConnection();
                                            }
                                            ColumnFieldName = arr[w].ToString();
                                            DataRow[] FieldRow = WorkingDataTable.Tables[0].Select("FIELDNAME = '" + ColumnFieldName + "' AND RecordType = 'DR' AND FileType='EFT'");
                                            FileFormatValue = BRBase.BRBaseConvert.ConvertToInt32(FieldRow[0]["Length"].ToString());

                                            FieldRow = null;
                                            FieldRow = WorkingDataTable.Tables[0].Select("FIELDNAME = '" + ColumnFieldName + "' AND RecordType = 'DR' AND FileType='EFT'");
                                            FileFormatValueMandatoryLength = BRBase.BRBaseConvert.ConvertToBoolean(FieldRow[0]["IsLengthMandatoryFieldSize"].ToString());

                                            FieldRow = null;
                                            FieldRow = WorkingDataTable.Tables[0].Select("FIELDNAME = '" + ColumnFieldName + "' AND RecordType = 'DR' AND FileType='EFT'");
                                            Filler = BRBase.BRBaseConvert.ConvertToChar(FieldRow[0]["Filler"].ToString());
                                            TrxRowID = BRBase.BRBaseConvert.ConvertToString(DataDvr["TrxRowID"]);
                                           
                                           
                                            switch (arr[w].ToString().ToUpper())
                                            {
                                                case "RETURNCODE":
                                                case "VOUCHERCODE":
                                                case "CURRENCYCODE":
                                                
                                                case "NAMEOFEMPLOYEE":
                                                case "PINNUMBER":
                                                case "VATSERIALNO":
                                                case "VATNUMBER":
                                                case "MONTHOFPAYMENT":
                                                case "REGISTEREDVATNAME":
                                                case "TYPEOFVATRETURN":
                                                case "PAYMENTTYPE":
                                                    Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName]).Trim();
                                                    ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Length);
                                                    if (FileFormatValueMandatoryLength)
                                                    {
                                                        Value = new string(Filler, FileFormatValue - ValueLength) + Value;
                                                    }

                                                    arryRow[t] = Value;
                                                    t = t + 1;
                                                    break;
                                              case "ESLIPNUMBER":
                                                    Value = BRBase.BRBaseConvert.ConvertToString(DataDvr["FROMBRANCH"]).Trim();
                                                    ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr["FROMBRANCH"].ToString().Trim().Length);
                                                    if (ValueLength > FileFormatValue)
                                                    {
                                                        Value = Value.ToString().Substring(0, FileFormatValue);
                                                    }
                                                    ValueLength = BRBase.BRBaseConvert.ConvertToInt32(Value.ToString().Trim().Length);
                                                    //if (FileFormatValueMandatoryLength)
                                                    //{
                                                    //    Value = Value.PadLeft((BRBase.BRBaseConvert.ConvertToInt32(FileFormatValue)), Filler);
                                                    //}
                                                    Value = Value.Substring(0, 2);
                                                    arryRow[t] = Value;
                                                    t = t + 1;
                                                    break;
                                                case "FROMBRANCH":
                                                    //Counter = Counter + 1;
                                                    Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName]).Trim();
                                                    ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Replace(" ", "").Length);
                                                    //if (Counter == 2)
                                                    //{
                                                    //    Value = Value.Substring(0, 2);
                                                    //}
                                                    //else
                                                    //{
                                                        if (FileFormatValueMandatoryLength)
                                                        {
                                                            Value = new string(Filler, FileFormatValue - ValueLength) + Value;
                                                        }
                                                    //}
                                                    arryRow[t] = Value;
                                                    t = t + 1;
                                                    break;
                                                case "TOBRANCH":
                                                    Counter = Counter + 1;
                                                    Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName]).Trim();
                                                    ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Replace(" ","").Length);
                                                    //if (Counter == 2)
                                                    //    {
                                                    //        Value = Value.Substring(0, 2);
                                                    //    }
                                                    //else
                                                    //    {
                                                            if (FileFormatValueMandatoryLength)
                                                            {
                                                                Value = new string(Filler, FileFormatValue - ValueLength) + Value;
                                                            }
                                                    //}
                                                    arryRow[t] = Value;
                                                    t = t + 1;
                                                    break;
                                                case "TOBANK":
                                                    Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName].ToString().Trim());
                                                    ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Trim().Length);
                                                    Value = new string(Filler, FileFormatValue - ToBank.Length) + ToBank;
                                                    BankTO = BRBase.BRBaseConvert.ConvertToString(Value);
                                                    arryRow[t] = Value;
                                                    t = t + 1;
                                                    break;
                                                case "PROCESSINGNO":
                                                    BRBase.BRDataSet dsUniqueClearingID = new BRBase.BRDataSet();
                                                    string UniqueID = string.Empty;
                                                    ClearingUniversalMethod(usrInfo, "p_GetUniqueClearingID", out dsUniqueClearingID, BRBase.BRModule.InwardFileImportation, conn, new object[] { "BrDataSet" }, new object[] { FileFormatValue });
                                                    if (dsUniqueClearingID.Tables.Count > 0)
                                                    {
                                                        if (dsUniqueClearingID.Tables[0].Rows.Count > 0)
                                                        {
                                                            UniqueID = dsUniqueClearingID.Tables[0].Rows[0]["UniqueID"].ToString();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        UniqueID = new string('0', 15);
                                                    }

                                                    Value = BRBase.BRBaseConvert.ConvertToString(UniqueID).Trim();
                                                    ValueLength = BRBase.BRBaseConvert.ConvertToInt32(UniqueID.ToString().Length);
                                                    if (FileFormatValueMandatoryLength)
                                                    {
                                                        Value = new string(Filler, FileFormatValue - ValueLength) + Value;
                                                    }
                                                    arryRow[t] = Value;
                                                    t = t + 1;
                                                    break;
                                                case "PAYMENTDATE":
                                                    Value = ddmmmyyyy;
                                                    arryRow[t] = Value;
                                                    t = t + 1;
                                                    break;
                                                case "FROMBANK":
                                                    arryRow[t] = usrInfo.strBank;
                                                    t = t + 1;
                                                    break;
                                                case "TOACCOUNT":
                                                    Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName]).Trim();
                                                    ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Length);
                                                    if (FileFormatValueMandatoryLength)
                                                    {
                                                        Value = new string(Filler, FileFormatValue - ValueLength) + Value;
                                                    }
                                                    arryRow[t] = Value;
                                                    t = t + 1;
                                                    break;

                                                case "COLLECTIONACCOUNT":
                                                case "PACCOUNT":
                                                    Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName]).Trim();
                                                    Value = Value.Trim();
                                                    ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Length);
                                                    if (FileFormatValueMandatoryLength)
                                                    {
                                                        Value = new string(Filler, FileFormatValue - ValueLength) + Value;
                                                    }
                                                    arryRow[t] = Value;
                                                    t = t + 1;

                                                    break;
                                                case "ORIGINATORREFERENCECODE":
                                                case "BENEFICIARYNAME":
                                                    Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName]);
                                                    ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Length);
                                                    if (FileFormatValueMandatoryLength)
                                                    {
                                                        //Value = new string(Filler, FileFormatValue - ValueLength) + Value;
                                                        Value = Value.PadRight(35);
                                                    }
                                                    if (ColumnFieldName.ToString().ToUpper() == "VALUE")
                                                    {
                                                        Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(DataDvr[ColumnFieldName])));
                                                        Value =  Value.PadRight(35);
                                                    }
                                                    arryRow[t] = Value;
                                                    t = t + 1;
                                                    break;
                                                case "VALUE":
                                                case "COMMISSIONCHARGED":
                                                    Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(DataDvr[ColumnFieldName])));
                                                    ValueLength = BRBase.BRBaseConvert.ConvertToInt32(Value.ToString().Length);
                                                    if (FileFormatValueMandatoryLength)
                                                    {
                                                        Value = new string(Filler, FileFormatValue - ValueLength) + Value;
                                                    }
                                                     
                                                    //MessageBox.Show(Value);
                                                    arryRow[t] = Value;
                                                    t = t + 1;
                                                    break;
                                                case "ORIGINATORCODE":
                                                    Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName]);
                                                    ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Length);
                                                    if (FileFormatValueMandatoryLength)
                                                    {
                                                        Value = new string(Filler, FileFormatValue - ValueLength) + Value;
                                                    }
                                                    if (ColumnFieldName.ToString().ToUpper() == "VALUE")
                                                    {
                                                        Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(DataDvr[ColumnFieldName])));
                                                        Value = new string(Filler, FileFormatValue - ValueLength) + Value;
                                                    }
                                                    arryRow[t] = Value;
                                                    t = t + 1;
                                                    break;
                                                case "POLICYNUMBER1":
                                                case "POLICYNUMBER2":
                                                    Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName]);
                                                    ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Length);
                                                    FileFormatValue = 15;//For now for UG
                                                    if (FileFormatValueMandatoryLength)
                                                    {
                                                        Value = Value + new string(' ', FileFormatValue - ValueLength);
                                                    }
                                                    arryRow[t] = Value;
                                                    t = t + 1;
                                                    break;
                                                case "REMARKS":
                                                    Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName]);
                                                    ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Length);
                                                    if (FileFormatValueMandatoryLength)
                                                    {
                                                        Value = new string(' ', FileFormatValue - ValueLength) + Value;
                                                    }
                                                    arryRow[t] = Value;
                                                    t = t + 1;
                                                    break;
                                                case "REFNO":
                                                    Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName]);
                                                    ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Length);
                                                    if (FileFormatValueMandatoryLength)
                                                    {
                                                        Value = new string(' ', FileFormatValue - ValueLength) + Value;
                                                    }
                                                    arryRow[t] = Value;
                                                    t = t + 1;
                                                    break;
                                                case "FILLER":
                                                    Value = new string('0', BRBase.BRBaseConvert.ConvertToInt32(FillerLength[FillerCount]));
                                                    FillerCount += 1;
                                                    arryRow[t] = Value;
                                                    t = t + 1;
                                                    break;
                                            }
                                        }
                                        break;
                                }
                                Data = string.Empty;
                                if (Header == 0)
                                {
                                    BRBase.BRDataSet dsUniqueClearingID = new BRBase.BRDataSet();
                                    string UniqueID = string.Empty;
                                    ClearingUniversalMethod(usrInfo, "p_GetUniqueClearingID", out dsUniqueClearingID, BRBase.BRModule.InwardFileImportation, conn, new object[] { "BrDataSet" }, new object[] { FileFormatValue });
                                    if (dsUniqueClearingID.Tables.Count > 0)
                                    {
                                        if (dsUniqueClearingID.Tables[0].Rows.Count > 0)
                                        {
                                            UniqueID = dsUniqueClearingID.Tables[0].Rows[0]["UniqueID"].ToString();
                                        }
                                    }
                                    else
                                    {
                                        UniqueID = new string('0', 9);
                                    }

                                    UniqueID = new string('0', 9 - UniqueID.Length) + UniqueID;
                                    Data = "185" + ddmmmyyyy + usrInfo.strBank + "00000" + UniqueID + "1" + new string('0', 130);
                                    dt.Rows.Add(Data);
                                    Header += 1;
                                }
                                Data = string.Empty;
                                if (arryRow[0].ToString() != "00")
                                {
                                    isUnpaid = true;
                                }
                                if (arryRow[0] != null)
                                {
                                    for (Int32 m = 0; m < arryRow.Length; m++)
                                    {
                                        if (isUnpaid == true)
                                        {
                                            isUnpaid = true;
                                            UnpaidData = UnpaidData + (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
                                        }
                                        else
                                        {
                                            Data = Data + (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
                                        }
                                    }
                                    if (Data != string.Empty)
                                    {
                                        dt.Rows.Add(Data);
                                    }
                                    if (isUnpaid == true)
                                    {
                                        if (conn.State != ConnectionState.Open)
                                        {
                                            conn = GetConnection();
                                        }
                                        BRBase.BRDataSet dsOriginalUnpaidTrx = new BRBase.BRDataSet();
                                        //First I retrieve what came in, so that i just change the returncode
                                        ClearingUniversalMethod(usrInfo, "p_GetTheOriginalUnpaidTrx", out dsOriginalUnpaidTrx, BRBase.BRModule.GenerateClearingFile, conn, new object[] { "BrDataSet" }, new object[] { TrxRowID });
                                        if (dsOriginalUnpaidTrx.Tables.Count > 0)
                                        {
                                            if (dsOriginalUnpaidTrx.Tables[0].Rows.Count > 0)
                                            {
                                                UnpaidData = dsOriginalUnpaidTrx.Tables[0].Rows[0]["Data"].ToString();
                                                UnpaidData = UnpaidData.Substring(2);
                                                UnpaidData = arryRow[0].ToString() + UnpaidData;
                                            }
                                        }
                                        UnpaidColl.Add(UnpaidData);
                                    }
                                    Data = string.Empty;
                                    UnpaidData = string.Empty;
                                    isUnpaid = false;
                                }
                                t = 0;
                                arryRow = new Object[Itemcount];
                                FillerCount = 0;
                                Value = string.Empty;
                                EJdataTable = new DS_trxClearing();
                            }
                            break;
                    }
                }
                FillerLength = new ArrayList();
                arr = new ArrayList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            if (dt.Rows.Count > 0)
            {
                if (UnpaidColl.Count > 0)
                {
                    Data = "2272" + new string('0', 154);
                    dt.Rows.Add(Data);
                    Data = string.Empty;
                    for (Int32 m = 0; m < UnpaidColl.Count; m++)
                    {
                        dt.Rows.Add(UnpaidColl[m]);
                    }
                }
                Data = "19" + usrInfo.strBank + new string('0', 154);
                UnpaidColl.Clear();
                dt.Rows.Add(Data);
            }
            return dt;
        }

        public static DataTable GenerateEFTs(string ToBank, DS_trxClearing dstrxClearing, DS_ClearingFileFormat dsClearingFileFormat, DateTime WorkingDate, BRBase.UserInfo usrInfo, String Banks, IDbConnection conn, string[] conString, bool isMIPSEFTs = false)
        {
            Int32 t, FillerCount, Itemcount, Header;
            Int32 i, w;
            bool isUnpaid = false;
            ArrayList FillerLength = new ArrayList();
            string EFTType = string.Empty;
            string Data = string.Empty;
            string sortOrder = " Start ASC";
            BRBase.BRDataSet dsUniqueClearingID = new BRBase.BRDataSet();
            string UniqueID = string.Empty;
            string NewValue = string.Empty;
            DataRow[] drHeaderFileFormatResult = null;
            DS_trxClearing EJdataTable = new DS_trxClearing();
            string ColumnFieldName = string.Empty;
            Int32 FileFormatValue = 0;
            StringCollection UnpaidColl = new StringCollection(); ;
            StringCollection UnPaidTrxRowIDColl = new StringCollection(); 
            char Filler;
            string TrxRowID="";
            string UnpaidData = string.Empty;
            DataTable dt = new DataTable();
            Int32 ValueLength = 0;
            ArrayList arr = new ArrayList();
            ArrayList Arr2 = new ArrayList();
            bool FileFormatValueMandatoryLength = false;
            DS_ClearingFileFormat WorkingDataTable = new DS_ClearingFileFormat();
            string Value = string.Empty;
            drHeaderFileFormatResult = dsClearingFileFormat.Tables[0].Select("RecordType = 'CR' AND FileType='EFT'", sortOrder);
            dt.Columns.Add("TrxRowID", typeof(string));
            dt.Columns.Add("Data", typeof(string));
            string BankTO = string.Empty;
            Itemcount = 0;
            Header = 0;
            string dd = new string('0', 2 - BRBase.BRBaseConvert.ConvertToString(WorkingDate.Day).ToString().Length) + BRBase.BRBaseConvert.ConvertToString(WorkingDate.Day);
            string mm = new string('0', 2 - BRBase.BRBaseConvert.ConvertToString(WorkingDate.Month).Length) + BRBase.BRBaseConvert.ConvertToString(WorkingDate.Month);
            string yyyy = BRBase.BRBaseConvert.ConvertToString(WorkingDate.Year);
            string ddmmmyyyy = dd + mm + yyyy;
            Object[] arryRow = new Object[Itemcount];
            foreach (DataRow dvr in drHeaderFileFormatResult)
            {
                WorkingDataTable.Tables[0].ImportRow(dvr);
            }
            WorkingDataTable.AcceptChanges();
            Arr2.Add("NORMAL");
            //Arr2.Add("VAT");
            //Arr2.Add("PAYE");
            //Arr2.Add("COMMISSION");
            //Arr2.Add("ATM");
            //Arr2.Add("DIRECTDEBIT");
            for (i = 0; i < Arr2.Count; i++)
            {
                arr = new ArrayList();
                switch (Arr2[i].ToString())
                {
                    case "NORMAL":
                        FillerLength = new ArrayList();
                        EFTType = "NORMAL";
                        arr.Add("RETURNCODE");
                        arr.Add("VOUCHERCODE");
                        arr.Add("CURRENCYCODE");
                        arr.Add("VALUE");
                        arr.Add("FILLER");
                        arr.Add("FROMBANK");
                        arr.Add("FROMBRANCH");
                        arr.Add("PACCOUNT");
                        arr.Add("TOBANK");
                        arr.Add("TOBRANCH");
                        arr.Add("TOACCOUNT");
                        arr.Add("FROMBANK");
                        arr.Add("FROMBRANCH");
                        arr.Add("PROCESSINGNO");
                        arr.Add("BENEFICIARYNAME");
                        arr.Add("ORIGINATORREFERENCECODE");
                        if (ConfigurationManager.AppSettings["Rem"] == "1")
                        {
                            if (isMIPSEFTs == false)
                            {
                                arr.Add("PAYERSNAME");
                                arr.Add("FILLER");
                                FillerLength.Add("1");
                                FillerLength.Add("4");
                                Itemcount = 18;
                            }
                            else
                            {
                                arr.Add("PAYERSNAME");
                                FillerLength.Add("1");
                                Itemcount = 17;
                            }
                        }
                        else
                        {
                            arr.Add("FILLER");
                            FillerLength.Add("1");
                            FillerLength.Add("8");
                            Itemcount = 17;
                        }
                        arryRow = new Object[Itemcount];
                        if (isMIPSEFTs == false)
                        {
                            drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("VoucherCode Not In('40','39','42','54','53','31','02','03') AND ReturnCode Not In('90','97') AND ToBank " + Banks + " AND TRXTYPE='OD'");
                        }
                        else
                        {
                            drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("VoucherCode Not In('40','39','42','54','53','31','02','03') AND ReturnCode Not In('90','97') AND TRXTYPE='OD'");
                        }
                        
                        foreach (DataRow dvr in drHeaderFileFormatResult)
                        {
                            EJdataTable.Tables[0].ImportRow(dvr);
                        }
                        EJdataTable.AcceptChanges();
                        break;
                    case "VAT":
                        FillerLength = new ArrayList();
                        EFTType = "NORMAL";
                        arr.Add("RETURNCODE");
                        arr.Add("VOUCHERCODE");
                        arr.Add("VALUE");
                        arr.Add("FILLER");
                        arr.Add("FROMBANK");
                        arr.Add("FROMBRANCH");
                        arr.Add("COLLECTIONACCOUNT");
                        arr.Add("TOBANK");
                        arr.Add("TOBRANCH");
                        arr.Add("TOACCOUNT");
                        arr.Add("FROMBANK");
                        arr.Add("FROMBRANCH");
                        arr.Add("PROCESSINGNO");
                        arr.Add("REGISTEREDVATNAME");
                        arr.Add("VATPINNo");
                        arr.Add("VATPAYEMonth");
                        arr.Add("PAYMENTDATE");
                        arr.Add("VATPAYTYPE");
                        arr.Add("VATSERIALNO");
                        arr.Add("COMMISSIONCHARGED");
                        arr.Add("FILLER");
                        FillerLength.Add("1");
                        FillerLength.Add("5");
                        Itemcount = 21;
                        arryRow = new Object[Itemcount];
                        drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("VoucherCode = '42' AND ToBank " + Banks + "  AND TRXTYPE='OD'");
                        foreach (DataRow dvr in drHeaderFileFormatResult)
                        {
                            EJdataTable.Tables[0].ImportRow(dvr);
                        }
                        EJdataTable.AcceptChanges();
                        break;
                    case "PAYE":
                        FillerLength = new ArrayList();
                        EFTType = "NORMAL";
                        arr.Add("RETURNCODE");
                        arr.Add("VOUCHERCODE");
                        arr.Add("VALUE");
                        arr.Add("FILLER");
                        arr.Add("FROMBANK");
                        arr.Add("FROMBRANCH");
                        arr.Add("COLLECTIONACCOUNT");
                        arr.Add("TOBANK");
                        arr.Add("TOBRANCH");
                        arr.Add("TOACCOUNT");
                        arr.Add("FROMBANK");
                        arr.Add("FROMBRANCH");
                        arr.Add("PROCESSINGNO");
                        arr.Add("NAMEOFEMPLOYEE");
                        arr.Add("VATPINNo");
                        arr.Add("VATPAYEMonth");
                        arr.Add("PAYMENTDATE");
                        arr.Add("VATPAYTYPE");
                        arr.Add("FILLER");
                        arr.Add("COMMISSIONCHARGED");
                        arr.Add("FILLER");
                        FillerLength.Add("1");
                        FillerLength.Add("6");
                        FillerLength.Add("5");
                        Itemcount = 21;
                        arryRow = new Object[Itemcount];
                        drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("VoucherCode = '39' AND ToBank " + Banks + "  AND TRXTYPE='OD'");
                        foreach (DataRow dvr in drHeaderFileFormatResult)
                        {
                            EJdataTable.Tables[0].ImportRow(dvr);
                        }
                        EJdataTable.AcceptChanges();
                        break;
                    case "COMMISSION":
                        //Commission
                        FillerLength = new ArrayList();
                        EFTType = "NORMAL";
                        arr.Add("RETURNCODE");
                        arr.Add("VOUCHERCODE");
                        arr.Add("CURRENCYCODE");
                        arr.Add("VALUE");
                        arr.Add("FILLER");
                        arr.Add("TOBANK");
                        arr.Add("TOBRANCH");
                        arr.Add("TOACCOUNT");
                        arr.Add("SERIALNUMBER");
                        arr.Add("PROCESSINGNO");
                        arr.Add("FILLER");
                        arr.Add("ORIGINATORREFERENCECODE");
                        arr.Add("FILLER");
                        FillerLength.Add("1");
                        FillerLength.Add("10");
                        FillerLength.Add("78");
                        Itemcount = 13;
                        arryRow = new Object[Itemcount];
                        drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("ReturnCode = '90' AND ToBank " + Banks + "  AND TRXTYPE='OD'");
                        foreach (DataRow dvr in drHeaderFileFormatResult)
                        {
                            EJdataTable.Tables[0].ImportRow(dvr);
                        }
                        EJdataTable.AcceptChanges();
                        break;
                       
                    case "ATM":
                        //ATM 54 & 53
                        FillerLength = new ArrayList();
                        EFTType = "NORMAL";
                        arr.Add("RETURNCODE");
                        arr.Add("VOUCHERCODE");
                        arr.Add("CURRENCYCODE");
                        arr.Add("VALUE");
                        arr.Add("FILLER");
                        arr.Add("TOBANK");
                        arr.Add("FILLER");
                        arr.Add("FROMBANK");
                        arr.Add("FILLER");
                        arr.Add("PROCESSINGNO");
                        arr.Add("FILLER");
                        FillerLength.Add("1");
                        FillerLength.Add("18");
                        FillerLength.Add("23");
                        FillerLength.Add("80");
                        Itemcount = 11;
                        arryRow = new Object[Itemcount];
                        drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("ReturnCode = '97' AND ToBank " + Banks + "  AND TRXTYPE='OD'");
                        foreach (DataRow dvr in drHeaderFileFormatResult)
                        {
                            EJdataTable.Tables[0].ImportRow(dvr);
                        }
                        EJdataTable.AcceptChanges();
                        break;
                }
                t = 0;
                FillerCount = 0;
                try
                {
                    foreach (DataColumn cvr in WorkingDataTable.Tables[0].Columns)
                    {
                        switch (cvr.ColumnName.ToString().ToUpper())
                        {
                            case "FIELDNAME":
                                foreach (DataRow DataDvr in EJdataTable.Tables[0].Rows)
                                {
                                    switch (EFTType.ToString().ToUpper())
                                    {
                                        case "NORMAL":
                                            for (w = 0; w < arr.Count; w++)
                                            {
                                                if (conn.State != ConnectionState.Open)
                                                {
                                                    conn = ReGetConnection(conString);
                                                }
                                               
                                                ColumnFieldName = arr[w].ToString();
                                                DataRow[] FieldRow = WorkingDataTable.Tables[0].Select("FIELDNAME = '" + ColumnFieldName + "' AND RecordType = 'CR' AND FileType='EFT'");
                                                FileFormatValue = BRBase.BRBaseConvert.ConvertToInt32(FieldRow[0]["Length"].ToString());

                                                FieldRow = null;
                                                FieldRow = WorkingDataTable.Tables[0].Select("FIELDNAME = '" + ColumnFieldName + "' AND RecordType = 'CR' AND FileType='EFT'");
                                                if (FieldRow[0]["IsLengthMandatoryFieldSize"].ToString() == "")
                                                {
                                                    FileFormatValueMandatoryLength = false;
                                                }
                                                else
                                                {
                                                    FileFormatValueMandatoryLength = BRBase.BRBaseConvert.ConvertToBoolean(FieldRow[0]["IsLengthMandatoryFieldSize"].ToString());
                                                }
                                                FieldRow = null;
                                                FieldRow = WorkingDataTable.Tables[0].Select("FIELDNAME = '" + ColumnFieldName + "' AND RecordType = 'CR' AND FileType='EFT'");
                                                Filler = BRBase.BRBaseConvert.ConvertToChar(FieldRow[0]["Filler"].ToString());
                                                TrxRowID = BRBase.BRBaseConvert.ConvertToString(DataDvr["TrxRowID"]).Trim();
                                                switch (arr[w].ToString().ToUpper())
                                                {
                                                    case "RETURNCODE":
                                                    case "VOUCHERCODE":
                                                    case "CURRENCYCODE": //This only Applies for Truncation, to be uncommented if trucation gone life.
                                                    case "NAMEOFEMPLOYEE":
                                                    case "VATPINNO":
                                                    case "SERIALNUMBER":
                                                    case "REGISTEREDVATNAME":
                                                        Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName]).Trim();
                                                        ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Trim().Length);
                                                        if (ValueLength > FileFormatValue)
                                                        {
                                                            Value = Value.ToString().Substring(0, FileFormatValue);
                                                        }
                                                        ValueLength = BRBase.BRBaseConvert.ConvertToInt32(Value.ToString().Trim().Length);
                                                        if (FileFormatValueMandatoryLength)
                                                        {
                                                            Value = new string(Filler, FileFormatValue - ValueLength) + Value;
                                                        }

                                                        arryRow[t] = Value;
                                                        t = t + 1;
                                                        break;

                                                    case "FROMBRANCH":
                                                        if (arryRow[0].ToString() == "00" || arryRow[0].ToString() == "17")
                                                        {
                                                            Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName]).Trim();
                                                            ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Trim().Length);
                                                            if (ValueLength > FileFormatValue)
                                                            {
                                                                Value = Value.ToString().Substring(0, FileFormatValue);
                                                            }
                                                            ValueLength = BRBase.BRBaseConvert.ConvertToInt32(Value.ToString().Trim().Length);
                                                            if (FileFormatValueMandatoryLength)
                                                            {
                                                                Value = new string(Filler, FileFormatValue - ValueLength) + Value;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            switch (Arr2[i].ToString())
                                                            {
                                                                case "NORMAL":
                                                                    Value = BRBase.BRBaseConvert.ConvertToString(DataDvr["TOBRANCH"]).Trim();
                                                                    ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr["TOBRANCH"].ToString().Trim().Length);
                                                                    Value = new string('0', FileFormatValue - ValueLength) + Value;
                                                                    break;
                                                                case "VAT":
                                                                case "COMMISSION":
                                                                case "PAYE":
                                                                    Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName]).Trim();
                                                                    ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Trim().Length);
                                                                    if (FileFormatValueMandatoryLength)
                                                                    {
                                                                        Value = new string(Filler, FileFormatValue - ValueLength) + Value;
                                                                    }
                                                                    break;
                                                            }
                                                        }
                                                        arryRow[t] = Value;
                                                        t = t + 1;
                                                        break;
                                                    case "VATSERIALNO":
                                                        Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName]).Trim();
                                                        ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Trim().Length);
                                                        if (ValueLength > FileFormatValue)
                                                        {
                                                            Value = Value.Substring(0, FileFormatValue);
                                                            ValueLength = BRBase.BRBaseConvert.ConvertToInt32(Value.ToString().Trim().Length);
                                                        }
                                                        if (FileFormatValueMandatoryLength)
                                                        {
                                                            Value = new string(Filler, FileFormatValue - ValueLength) + Value;
                                                        }

                                                        arryRow[t] = Value;
                                                        t = t + 1;
                                                        break;
                                                    case "VATPAYTYPE":
                                                        Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName]).Trim();
                                                        switch (Value)
                                                        {
                                                            case "01":
                                                            case "02":
                                                            case "03":
                                                            case "04":
                                                            case "05":
                                                            case "06":
                                                            case "07":
                                                                Value = Value;
                                                                break;
                                                            default:
                                                                Value = "01";
                                                                break;
                                                        }
                                                        ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Trim().Length);
                                                        if (ValueLength > FileFormatValue)
                                                        {
                                                            Value = Value.ToString().Substring(0, FileFormatValue);
                                                        }
                                                        if (FileFormatValueMandatoryLength)
                                                        {
                                                            Value = new string(Filler, FileFormatValue - ValueLength) + Value;
                                                        }

                                                        arryRow[t] = Value;
                                                        t = t + 1;
                                                        break;
                                                    case "TOBRANCH":
                                                        if (arryRow[0].ToString() == "00" || arryRow[0].ToString() == "17")
                                                        {
                                                            switch (Arr2[i].ToString())
                                                            {
                                                                case "NORMAL":
                                                                    Value = BRBase.BRBaseConvert.ConvertToString(DataDvr["TOBRANCH"]).Trim();
                                                                    ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr["TOBRANCH"].ToString().Trim().Length);
                                                                    Value = new string('0', FileFormatValue - ValueLength) + Value;
                                                                    break;
                                                                case "VAT":
                                                                case "COMMISSION":
                                                                case "PAYE":
                                                                    Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName]).Trim();
                                                                    ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Trim().Length);
                                                                    if (FileFormatValueMandatoryLength)
                                                                    {
                                                                        Value = new string(Filler, FileFormatValue - ValueLength) + Value;
                                                                    }
                                                                    break;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName]).Trim();
                                                            ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Trim().Length);
                                                            if (ValueLength > FileFormatValue)
                                                            {
                                                                Value = Value.ToString().Substring(0, FileFormatValue);
                                                            }
                                                            ValueLength = BRBase.BRBaseConvert.ConvertToInt32(Value.ToString().Trim().Length);
                                                            if (FileFormatValueMandatoryLength)
                                                            {
                                                                Value = new string(Filler, FileFormatValue - ValueLength) + Value;
                                                            }

                                                            arryRow[t] = Value;
                                                        }
                                                        arryRow[t] = Value;
                                                        t = t + 1;
                                                        break;
                                                    case "TOBANK":
                                                        if (arryRow[0].ToString() == "00" || arryRow[0].ToString() == "17")
                                                        {
                                                            Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName].ToString().Trim());
                                                            ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Trim().Length);
                                                            switch (Arr2[i].ToString())
                                                            {
                                                                case "COMMISSION":
                                                                case "VAT":
                                                                case "PAYE":
                                                                case "NORMAL":
                                                                    if (FileFormatValueMandatoryLength)
                                                                    {
                                                                        switch (arryRow[0].ToString())
                                                                        {
                                                                            case "00":
                                                                            case "90":
                                                                            case "97":
                                                                                Value = new string(Filler, FileFormatValue - Value.ToString().Trim().Length) + Value.ToString().Trim();
                                                                                break;
                                                                            default:
                                                                                Value = new string(Filler, FileFormatValue - Value.ToString().Trim().Length) + Value.ToString().Trim();
                                                                                break;
                                                                        }
                                                                    }
                                                                    break;
                                                            }
                                                            BankTO = BRBase.BRBaseConvert.ConvertToString(Value);
                                                            arryRow[t] = Value;
                                                        }
                                                        else
                                                        {
                                                            switch (arryRow[0].ToString())
                                                            {
                                                                case "00":
                                                                case "90":
                                                                case "97":
                                                                    arryRow[t] = usrInfo.strBank;
                                                                    break;
                                                                default:
                                                                    arryRow[t] = usrInfo.strBank;
                                                                    break;
                                                            }
                                                        }
                                                        t = t + 1;
                                                        break;
                                                    case "BENEFICIARYNAME":
                                                        Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName]).Trim();
                                                        ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Trim().Length);
                                                        if (ValueLength > FileFormatValue)
                                                        {
                                                            Value = Value.ToString().Substring(0, FileFormatValue);
                                                        }
                                                        ValueLength = BRBase.BRBaseConvert.ConvertToInt32(Value.ToString().Trim().Length);

                                                        switch (Arr2[i].ToString())
                                                        {
                                                            case "COMMISSION":
                                                                Value = new string(Filler, FileFormatValue - ValueLength) + Value;
                                                                break;
                                                            case "VAT":
                                                            case "PAYE":
                                                            case "NORMAL":
                                                                if (FileFormatValueMandatoryLength)
                                                                {
                                                                    Value = new string(' ', FileFormatValue - ValueLength) + Value.ToString().Trim();
                                                                }
                                                                break;
                                                        }
                                                        arryRow[t] = Value;
                                                        t = t + 1;
                                                        break;
                                                    case "PAYERSNAME":
                                                        Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName]).Trim();
                                                        ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Trim().Length);
                                                        if (ValueLength > FileFormatValue)
                                                        {
                                                            Value = Value.ToString().Substring(0, FileFormatValue);
                                                        }
                                                        ValueLength = BRBase.BRBaseConvert.ConvertToInt32(Value.ToString().Trim().Length);

                                                        switch (Arr2[i].ToString())
                                                        {
                                                            case "COMMISSION":
                                                                Value = new string(Filler, FileFormatValue - ValueLength) + Value;
                                                                break;
                                                            case "VAT":
                                                            case "PAYE":
                                                            case "NORMAL":
                                                                if (FileFormatValueMandatoryLength)
                                                                {
                                                                    Value = new string(' ', FileFormatValue - ValueLength) + Value.ToString().Trim();
                                                                }
                                                                break;
                                                        }
                                                        if (isMIPSEFTs ==false)
                                                        {
                                                            arryRow[t] = Value;
                                                        }
                                                        else
                                                        {
                                                            arryRow[t] = "00000000" + Value;
                                                        }
                                                        t = t + 1;
                                                        break;
                                                    case "PROCESSINGNO":
                                                        dsUniqueClearingID = new BRBase.BRDataSet();
                                                        UniqueID = string.Empty;
                                                        NewValue = string.Empty;
                                                        ClearingUniversalMethod(usrInfo, "p_GetUniqueClearingID", out dsUniqueClearingID, BRBase.BRModule.InwardFileImportation, conn, new object[] { "BrDataSet" }, new object[] { FileFormatValue });
                                                        if (dsUniqueClearingID.Tables.Count > 0)
                                                        {
                                                            if (dsUniqueClearingID.Tables[0].Rows.Count > 0)
                                                            {
                                                                UniqueID = dsUniqueClearingID.Tables[0].Rows[0]["UniqueID"].ToString();
                                                            }
                                                        }
                                                        Value = BRBase.BRBaseConvert.ConvertToString(UniqueID);
                                                        ValueLength = BRBase.BRBaseConvert.ConvertToInt32(UniqueID.ToString().Length);
                                                        switch (Arr2[i].ToString())
                                                        {
                                                            case "COMMISSION":
                                                                Value = new string(' ', 9 - ValueLength) + Value; 
                                                                break;
                                                            case "VAT":
                                                            case "PAYE":
                                                            case "NORMAL":
                                                            if (FileFormatValueMandatoryLength)
                                                            {
                                                                Value = new string(' ', FileFormatValue - ValueLength) + Value;
                                                            }
                                                                break;
                                                        }
                                                        arryRow[t] = Value;
                                                        t = t + 1;
                                                        break;
                                                    case "VATPAYEMONTH":
                                                            Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName]).Trim();
                                                            ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Trim().Length);
                                                            if (FileFormatValueMandatoryLength)
                                                            {
                                                                Value = new string(Filler, FileFormatValue - ValueLength) + Value;
                                                            }

                                                            arryRow[t] = Value;
                                                            t = t + 1;
                                                            break;
                                                    case "PAYMENTDATE":
                                                        Value = ddmmmyyyy;
                                                        arryRow[t] = Value;
                                                        t = t + 1;
                                                        break;
                                                    case "FROMBANK":
                                                        if (arryRow[0].ToString() == "00" || arryRow[0].ToString() == "17")
                                                        {
                                                            switch (arryRow[0].ToString())
                                                            {
                                                                case "00":
                                                                case "90":
                                                                case "97":
                                                                    arryRow[t] = usrInfo.strBank;
                                                                    break;
                                                                default:
                                                                    arryRow[t] = usrInfo.strBank;
                                                                    break;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName].ToString().Trim());
                                                            ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Trim().Length);
                                                            switch (Arr2[i].ToString())
                                                            {
                                                                case "COMMISSION":
                                                                case "VAT":
                                                                case "PAYE":
                                                                case "NORMAL":
                                                                    if (FileFormatValueMandatoryLength)
                                                                    {
                                                                        switch (arryRow[0].ToString())
                                                                        {
                                                                            case "00":
                                                                            case "90":
                                                                            case "97":
                                                                                Value = new string(Filler, FileFormatValue - ToBank.ToString().Trim().Length) + ToBank.ToString().Trim();
                                                                                break;
                                                                            default:
                                                                                Value = new string(Filler, FileFormatValue - ToBank.ToString().Trim().Length) + ToBank.ToString().Trim();
                                                                                break;
                                                                        }
                                                                    }
                                                                    break;
                                                            }
                                                            BankTO = BRBase.BRBaseConvert.ConvertToString(Value);
                                                            arryRow[t] = Value;
                                                        }

                                                        t = t + 1;
                                                        break;
                                                    case "TOACCOUNT":
                                                        if (arryRow[0].ToString() == "00" || arryRow[0].ToString() == "17")
                                                        {
                                                            switch (Arr2[i].ToString())
                                                            {
                                                                case "COMMISSION":
                                                                    Value = BRBase.BRBaseConvert.ConvertToString(DataDvr["TOACCOUNT"].ToString().Trim());
                                                                    ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr["TOACCOUNT"].ToString().Trim().Length);
                                                                    if (FileFormatValueMandatoryLength)
                                                                    {
                                                                        Value = new string(Filler, FileFormatValue - ValueLength) + Value;
                                                                    }
                                                                    break;
                                                                case "VAT":
                                                                case "PAYE":
                                                                case "NORMAL":
                                                                    Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName].ToString().Trim());
                                                                    ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Trim().Length);
                                                                    if (FileFormatValueMandatoryLength)
                                                                    {
                                                                        Value = new string(Filler, FileFormatValue - ValueLength) + Value;
                                                                    }
                                                                    break;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            switch (Arr2[i].ToString())
                                                            {
                                                                case "COMMISSION":
                                                                    Value = BRBase.BRBaseConvert.ConvertToString(DataDvr["COLLECTIONACCOUNT"].ToString().Trim());
                                                                    ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr["COLLECTIONACCOUNT"].ToString().Trim().Length);
                                                                    if (FileFormatValueMandatoryLength)
                                                                    {
                                                                        Value = new string(Filler, FileFormatValue - ValueLength) + Value;
                                                                    }
                                                                    break;
                                                                case "VAT":
                                                                case "PAYE":
                                                                case "NORMAL":
                                                                    Value = BRBase.BRBaseConvert.ConvertToString(DataDvr["COLLECTIONACCOUNT"].ToString().Trim());
                                                                    ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr["COLLECTIONACCOUNT"].ToString().Trim().Length);
                                                                    if (FileFormatValueMandatoryLength)
                                                                    {
                                                                        Value = new string(Filler, FileFormatValue - ValueLength) + Value;
                                                                    }
                                                                    break;
                                                            }
                                                        }
                                                        arryRow[t] = Value;
                                                        t = t + 1;
                                                        break;
                                                    case "COLLECTIONACCOUNT":
                                                    case "PACCOUNT":
                                                        if (arryRow[0].ToString() == "00" || arryRow[0].ToString() == "17")
                                                        {
                                                            switch (Arr2[i].ToString())
                                                            {
                                                                case "COMMISSION":
                                                                    Value = BRBase.BRBaseConvert.ConvertToString(DataDvr["COLLECTIONACCOUNT"].ToString().Trim());
                                                                    ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr["COLLECTIONACCOUNT"].ToString().Trim().Length);
                                                                    if (FileFormatValueMandatoryLength)
                                                                    {
                                                                        Value = new string(Filler, FileFormatValue - ValueLength) + Value;
                                                                    }
                                                                    break;
                                                                case "VAT":
                                                                case "PAYE":
                                                                case "NORMAL":
                                                                    Value = BRBase.BRBaseConvert.ConvertToString(DataDvr["COLLECTIONACCOUNT"].ToString().Trim());
                                                                    ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr["COLLECTIONACCOUNT"].ToString().Trim().Length);
                                                                    if (FileFormatValueMandatoryLength)
                                                                    {
                                                                        Value = new string(Filler, FileFormatValue - ValueLength) + Value;
                                                                    }
                                                                    break;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            switch (Arr2[i].ToString())
                                                            {
                                                                case "COMMISSION":
                                                                case "NORMAL":
                                                                    Value = BRBase.BRBaseConvert.ConvertToString(DataDvr["TOACCOUNT"].ToString().Trim());
                                                                    ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr["TOACCOUNT"].ToString().Trim().Length);
                                                                    if (FileFormatValueMandatoryLength)
                                                                    {
                                                                        Value = new string(Filler, FileFormatValue - ValueLength) + Value;
                                                                    }
                                                                    break;
                                                                case "VAT":
                                                                case "PAYE":
                                                                
                                                                    Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName].ToString().Trim());
                                                                    ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Trim().Length);
                                                                    if (FileFormatValueMandatoryLength)
                                                                    {
                                                                        Value = new string(Filler, FileFormatValue - ValueLength) + Value;
                                                                    }
                                                                    break;
                                                            }
                                                        }
                                                        arryRow[t] = Value;
                                                        t = t + 1;
                                                        break;
                                                    case "ORIGINATORREFERENCECODE":
                                                        switch (Arr2[i].ToString())
                                                        {
                                                            case "COMMISSION":
                                                                Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName]);
                                                                ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Length);
                                                                if (FileFormatValueMandatoryLength)
                                                                {
                                                                    Value = new string(Filler, 15 - ValueLength) + Value;
                                                                }
                                                                break;
                                                            case "VAT":
                                                            case "PAYE":
                                                            case "NORMAL":
                                                                if (FileFormatValueMandatoryLength)
                                                                {
                                                                    Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName]);
                                                                    ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Length);
                                                                    if (FileFormatValueMandatoryLength)
                                                                    {
                                                                        Value = new string(' ', FileFormatValue - ValueLength) + Value;
                                                                    }
                                                                    if (ColumnFieldName.ToString().ToUpper() == "VALUE")
                                                                    {
                                                                        Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(DataDvr[ColumnFieldName])));
                                                                        Value = new string(' ', FileFormatValue - ValueLength) + Value;
                                                                    }
                                                                }
                                                                break;
                                                        }
                                                        arryRow[t] = Value;
                                                        t = t + 1;
                                                        break;
                                                    case "VALUE":
                                                    case "COMMISSIONCHARGED":
                                                        Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(DataDvr[ColumnFieldName])));
                                                        ValueLength = BRBase.BRBaseConvert.ConvertToInt32(Value.ToString().Length);
                                                        if (FileFormatValueMandatoryLength)
                                                        {
                                                            Value = new string(Filler, FileFormatValue - ValueLength) + Value;
                                                        }
                                                        arryRow[t] = Value;
                                                        t = t + 1;
                                                        break;
                                                    case "ORIGINATORCODE":
                                                        Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName]);
                                                        ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Length);
                                                        if (FileFormatValueMandatoryLength)
                                                        {
                                                            Value = new string(Filler, FileFormatValue - ValueLength) + Value;
                                                        }
                                                        if (ColumnFieldName.ToString().ToUpper() == "VALUE")
                                                        {
                                                            Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount (BRBase.BRBaseConvert.ConvertToDouble(DataDvr[ColumnFieldName])));
                                                            Value = new string(Filler, FileFormatValue - ValueLength) + Value;
                                                        }
                                                        arryRow[t] = Value;
                                                        t = t + 1;
                                                        break;
                                                    case "POLICYNUMBER1":
                                                    case "POLICYNUMBER2":
                                                        Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName]);
                                                        ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Length);
                                                        if (ValueLength > FileFormatValue)
                                                        {
                                                            Value = Value.ToString().Substring(0, FileFormatValue);
                                                        }
                                                        if (FileFormatValueMandatoryLength)
                                                        {
                                                            Value = Value + new string(' ', FileFormatValue - ValueLength);
                                                        }
                                                        arryRow[t] = Value;
                                                        t = t + 1;
                                                        break;
                                                    case "REMARKS":
                                                        Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName]);
                                                        ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Length);
                                                        if (ValueLength > FileFormatValue)
                                                        {
                                                            Value = Value.ToString().Substring(0, FileFormatValue);
                                                        }
                                                        if (FileFormatValueMandatoryLength)
                                                        {
                                                            Value = new string(' ', FileFormatValue - ValueLength) + Value;
                                                        }
                                                        arryRow[t] = Value;
                                                        t = t + 1;
                                                        break;
                                                    case "REFNO":
                                                        dsUniqueClearingID = new BRBase.BRDataSet();
                                                        UniqueID = string.Empty;
                                                        NewValue = string.Empty;
                                                        ClearingUniversalMethod(usrInfo, "p_GetUniqueClearingID", out dsUniqueClearingID, BRBase.BRModule.InwardFileImportation, conn, new object[] { "BrDataSet" }, new object[] { FileFormatValue });
                                                        if (dsUniqueClearingID.Tables.Count > 0)
                                                        {
                                                            if (dsUniqueClearingID.Tables[0].Rows.Count > 0)
                                                            {
                                                                UniqueID = dsUniqueClearingID.Tables[0].Rows[0]["UniqueID"].ToString();
                                                            }
                                                        }
                                                        Value = BRBase.BRBaseConvert.ConvertToString(UniqueID);
                                                        ValueLength = BRBase.BRBaseConvert.ConvertToInt32(UniqueID.ToString().Length);
                                                        if (ValueLength > FileFormatValue)
                                                        {
                                                            Value = Value.ToString().Substring(0, FileFormatValue);
                                                        }
                                                        if (FileFormatValueMandatoryLength)
                                                        {
                                                            Value = new string(' ', FileFormatValue - ValueLength) + Value;
                                                        }
                                                        arryRow[t] = Value;
                                                        t = t + 1;
                                                        break;
                                                    case "FILLER":
                                                        //if (isMIPSEFTs == false)
                                                        //{
                                                            Value = new string('0', BRBase.BRBaseConvert.ConvertToInt32(FillerLength[FillerCount]));
                                                            FillerCount += 1;
                                                            arryRow[t] = Value;
                                                            t = t + 1;
                                                        //}
                                                        break;
                                                }
                                            }
                                            break;
                                    }
                                    Data = string.Empty;
                                    if (Header == 0)
                                    {
                                        dsUniqueClearingID = new BRBase.BRDataSet();
                                        UniqueID = string.Empty;
                                        if (conn.State != ConnectionState.Open)
                                        {
                                            conn = ReGetConnection(conString);
                                        }
                                        ClearingUniversalMethod(usrInfo, "p_GetUniqueClearingID", out dsUniqueClearingID, BRBase.BRModule.InwardFileImportation, conn, new object[] { "BrDataSet" }, new object[] { FileFormatValue });
                                        if (dsUniqueClearingID.Tables.Count > 0)
                                        {
                                            if (dsUniqueClearingID.Tables[0].Rows.Count > 0)
                                            {
                                                UniqueID = dsUniqueClearingID.Tables[0].Rows[0]["UniqueID"].ToString();
                                            }
                                        }
                                        else
                                        {
                                            UniqueID = new string('0', 9);
                                        }
                                        UniqueID = new string('0', 9 - UniqueID.Length) + UniqueID;
                                        if (ConfigurationManager.AppSettings["Rem"] == "1")
                                        {
                                            if (isMIPSEFTs == false)
                                            {
                                                Data = "186" + ddmmmyyyy + usrInfo.strBank + "00000" + UniqueID + "1" + new string('0', 161);//189
                                            }
                                            else
                                            {
                                                Data = "186" + ddmmmyyyy + usrInfo.strBank + "00000" + UniqueID + "1" + new string('0', 165);//189

                                            }
                                        }
                                        else
                                        {
                                            Data = "186" + ddmmmyyyy + usrInfo.strBank + "00000" + UniqueID + "1" + new string('0', 130);//158
                                        }
                                        //
                                       
                                        dt.Rows.Add('0', Data);
                                        Header += 1;
                                    }
                                    Data = string.Empty;
                                    if (arryRow[0].ToString() != "00" && arryRow[0].ToString() != "90" && arryRow[0].ToString() != "97")
                                    {
                                        isUnpaid = true;
                                    }
                                    if (arryRow[0] != null)
                                    {
                                        for (Int32 m = 0; m < arryRow.Length; m++)
                                        {
                                            if (isUnpaid == true)
                                            {
                                                isUnpaid = true;
                                                UnpaidData = UnpaidData +  (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
                                            }
                                            else
                                            {
                                                Data = Data +  (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
                                            }
                                        }
                                        if (Data != string.Empty)
                                        {
                                            dt.Rows.Add(TrxRowID, Data);
                                        }
                                        if (isUnpaid == true)
                                        {
                                            if (conn.State != ConnectionState.Open)
                                            {
                                                conn = ReGetConnection(conString);
                                            }
                                            BRBase.BRDataSet dsOriginalUnpaidTrx = new BRBase.BRDataSet();
                                            //First I retrieve what came in, so that i just change the returncode
                                            ClearingUniversalMethod(usrInfo, "p_GetTheOriginalUnpaidTrx", out dsOriginalUnpaidTrx, BRBase.BRModule.GenerateClearingFile, conn, new object[] { "BrDataSet" }, new object[] { TrxRowID });
                                            if (dsOriginalUnpaidTrx.Tables.Count > 0)
                                            {
                                                if (dsOriginalUnpaidTrx.Tables[0].Rows[0]["Data"].ToString() != "NIL")
                                                {
                                                    UnpaidData = dsOriginalUnpaidTrx.Tables[0].Rows[0]["Data"].ToString();
                                                    UnpaidData = UnpaidData.Substring(2);
                                                    UnpaidData = arryRow[0].ToString() + UnpaidData;
                                                }
                                            }
                                            UnpaidColl.Add(UnpaidData);
                                            UnPaidTrxRowIDColl.Add(TrxRowID);
                                            Data = string.Empty;
                                            UnpaidData = string.Empty;
                                            TrxRowID = string.Empty;
                                            isUnpaid = false;
                                        }
                                    }
                                    t = 0;
                                    arryRow = new Object[Itemcount];
                                    FillerCount = 0;
                                    Value = string.Empty;
                                    EJdataTable = new DS_trxClearing();
                                }
                                break;
                        }
                    }
                    FillerLength = new ArrayList();
                    arr = new ArrayList();
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }
            if (dt.Rows.Count > 0)
            {
                if (UnpaidColl.Count > 0)
                {
                    if (ConfigurationManager.AppSettings["Rem"] == "1")
                    {
                        if (isMIPSEFTs == false)
                        {
                            Data = "2272" + new string('0', 186);
                        }
                        else
                        {
                            Data = "2272" + new string('0', 192);
                        }
                    }
                    else
                    {
                        Data = "2272" + new string('0', 154);
                    }
                   
                    dt.Rows.Add('0', Data);
                    Data = string.Empty;
                    for (Int32 m = 0; m < UnpaidColl.Count; m++)
                    {
                        dt.Rows.Add(UnPaidTrxRowIDColl[m], UnpaidColl[m]);
                    }
                }
                if (ConfigurationManager.AppSettings["Rem"] == "1")
                {
                    if (isMIPSEFTs == false)
                    {
                        Data = "19" + usrInfo.strBank + new string('0', 185);
                    }
                    else
                    {
                        Data = "19" + usrInfo.strBank + new string('0', 189);
                    }
                }
                else
                {
                    Data = "19" + usrInfo.strBank + new string('0', 154);
                }
                dt.Rows.Add('0',Data);
            }
            UnpaidColl.Clear();
            UnPaidTrxRowIDColl.Clear();
            return dt;
        }
        public static DataTable GenerateEFTsUG(string ToBank, DS_trxClearing dstrxClearing, DS_ClearingFileFormat dsClearingFileFormat, DateTime WorkingDate, BRBase.UserInfo usrInfo, String Banks, IDbConnection conn)
        {
            Int32 t, FillerCount, Itemcount, Header;
            Int32 i, w;
            bool isUnpaid = false;
            ArrayList FillerLength = new ArrayList();
            string EFTType = string.Empty;
            string Data = string.Empty;
            string sortOrder = " Start ASC";
            string ClearingCenter = "47";
            BRBase.BRDataSet dsUniqueClearingID = new BRBase.BRDataSet();
            string UniqueID = string.Empty;
            string NewValue = string.Empty;
            DataRow[] drHeaderFileFormatResult = null;
            DS_trxClearing EJdataTable = new DS_trxClearing();
            string ColumnFieldName = string.Empty;
            Int32 FileFormatValue = 0;
            Int32 IncrNo = 0;
            string TrxRowID = "";
            StringCollection UnpaidColl = new StringCollection(); ;
            char Filler;
            string UnpaidData = string.Empty;
            DataTable dt = new DataTable();
            Int32 ValueLength = 0;
            ArrayList arr = new ArrayList();
            ArrayList Arr2 = new ArrayList();
            bool FileFormatValueMandatoryLength = false;
            DS_ClearingFileFormat WorkingDataTable = new DS_ClearingFileFormat();
            string Value = string.Empty;
            drHeaderFileFormatResult = dsClearingFileFormat.Tables[0].Select("RecordType = 'DATA' AND FileType='EFT'", sortOrder);
            dt.Columns.Add("Data", typeof(string));
            string BankTO = string.Empty;
            Itemcount = 0;
            Header = 0;
            string dd = new string('0', 2 - BRBase.BRBaseConvert.ConvertToString(WorkingDate.Day).ToString().Length) + BRBase.BRBaseConvert.ConvertToString(WorkingDate.Day);
            string mm = new string('0', 2 - BRBase.BRBaseConvert.ConvertToString(WorkingDate.Month).Length) + BRBase.BRBaseConvert.ConvertToString(WorkingDate.Month);
            string yyyy = BRBase.BRBaseConvert.ConvertToString(WorkingDate.Year);
            string ddmmmyyyy = dd + mm + yyyy;
            Object[] arryRow = new Object[Itemcount];
            foreach (DataRow dvr in drHeaderFileFormatResult)
            {
                WorkingDataTable.Tables[0].ImportRow(dvr);
            }
            WorkingDataTable.AcceptChanges();
            Arr2.Add("NORMAL");
            //Arr2.Add("VAT");
            //Arr2.Add("PAYE");
            //Arr2.Add("COMMISSION");
            //Arr2.Add("ATM");
            //Arr2.Add("DIRECTDEBIT");
            for (i = 0; i < Arr2.Count; i++)
            {
                arr = new ArrayList();
                switch (Arr2[i].ToString())
                {
                    case "NORMAL":
                        FillerLength = new ArrayList();
                        EFTType = "NORMAL";
                        arr.Add("RETURNCODE");
                        arr.Add("VOUCHERCODE");
                        arr.Add("VALUE");
                        arr.Add("FROMBANK");
                        arr.Add("FROMBRANCH");
                        arr.Add("COLLECTIONACCOUNT");
                        arr.Add("TOBANK");
                        arr.Add("TOBRANCH");
                        arr.Add("TOACCOUNT");
                        arr.Add("FROMBANK");
                        arr.Add("ESLIPNUMBER");
                        arr.Add("PROCESSINGNO");
                        arr.Add("BENEFICIARYNAME");
                        arr.Add("ORIGINATORREFERENCECODE");
                        arr.Add("FILLER");
                        FillerLength.Add("9");
                        FillerLength.Add("8");
                        Itemcount = 15;
                        arryRow = new Object[Itemcount];
                        drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("VoucherCode Not In('40','39','42','54','53','31','02','03') AND ReturnCode Not In('90','97') AND ToBank " + Banks + " AND TRXTYPE='OD'");//  AND SUBSTRING(FROMBRANCH,3,2) ='" + ClearingCenter + "' ");
                        foreach (DataRow dvr in drHeaderFileFormatResult)
                        {
                            EJdataTable.Tables[0].ImportRow(dvr);
                        }
                        EJdataTable.AcceptChanges();
                        break;
                    case "VAT":
                        FillerLength = new ArrayList();
                        EFTType = "NORMAL";
                        arr.Add("RETURNCODE");
                        arr.Add("VOUCHERCODE");
                        arr.Add("VALUE");
                        arr.Add("FILLER");
                        arr.Add("FROMBANK");
                        arr.Add("FROMBRANCH");
                        arr.Add("COLLECTIONACCOUNT");
                        arr.Add("TOBANK");
                        arr.Add("TOBRANCH");
                        arr.Add("TOACCOUNT");
                        arr.Add("FROMBANK");
                        arr.Add("FROMBRANCH");
                        arr.Add("PROCESSINGNO");
                        arr.Add("REGISTEREDVATNAME");
                        arr.Add("VATPINNo");
                        arr.Add("VATPAYEMonth");
                        arr.Add("PAYMENTDATE");
                        arr.Add("VATPAYTYPE");
                        arr.Add("VATSERIALNO");
                        arr.Add("COMMISSIONCHARGED");
                        arr.Add("FILLER");
                        FillerLength.Add("1");
                        FillerLength.Add("5");
                        Itemcount = 21;
                        arryRow = new Object[Itemcount];
                        drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("VoucherCode = '42' AND ToBank " + Banks + "  AND TRXTYPE='OD'");
                        foreach (DataRow dvr in drHeaderFileFormatResult)
                        {
                            EJdataTable.Tables[0].ImportRow(dvr);
                        }
                        EJdataTable.AcceptChanges();
                        break;
                    case "PAYE":
                        FillerLength = new ArrayList();
                        EFTType = "NORMAL";
                        arr.Add("RETURNCODE");
                        arr.Add("VOUCHERCODE");
                        arr.Add("VALUE");
                        arr.Add("FILLER");
                        arr.Add("FROMBANK");
                        arr.Add("FROMBRANCH");
                        arr.Add("COLLECTIONACCOUNT");
                        arr.Add("TOBANK");
                        arr.Add("TOBRANCH");
                        arr.Add("TOACCOUNT");
                        arr.Add("FROMBANK");
                        arr.Add("FROMBRANCH");
                        arr.Add("PROCESSINGNO");
                        arr.Add("NAMEOFEMPLOYEE");
                        arr.Add("VATPINNo");
                        arr.Add("VATPAYEMonth");
                        arr.Add("PAYMENTDATE");
                        arr.Add("VATPAYTYPE");
                        arr.Add("FILLER");
                        arr.Add("COMMISSIONCHARGED");
                        arr.Add("FILLER");
                        FillerLength.Add("1");
                        FillerLength.Add("6");
                        FillerLength.Add("5");
                        Itemcount = 21;
                        arryRow = new Object[Itemcount];
                        drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("VoucherCode = '39' AND ToBank " + Banks + "  AND TRXTYPE='OD'");
                        foreach (DataRow dvr in drHeaderFileFormatResult)
                        {
                            EJdataTable.Tables[0].ImportRow(dvr);
                        }
                        EJdataTable.AcceptChanges();
                        break;
                    case "COMMISSION":
                        //Commission
                        FillerLength = new ArrayList();
                        EFTType = "NORMAL";
                        arr.Add("RETURNCODE");
                        arr.Add("VOUCHERCODE");
                        arr.Add("CURRENCYCODE");
                        arr.Add("VALUE");
                        arr.Add("FILLER");
                        arr.Add("TOBANK");
                        arr.Add("TOBRANCH");
                        arr.Add("TOACCOUNT");
                        arr.Add("SERIALNUMBER");
                        arr.Add("PROCESSINGNO");
                        arr.Add("FILLER");
                        arr.Add("ORIGINATORREFERENCECODE");
                        arr.Add("FILLER");
                        FillerLength.Add("1");
                        FillerLength.Add("10");
                        FillerLength.Add("78");
                        Itemcount = 13;
                        arryRow = new Object[Itemcount];
                        drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("ReturnCode = '90' AND ToBank " + Banks + "  AND TRXTYPE='OD'");
                        foreach (DataRow dvr in drHeaderFileFormatResult)
                        {
                            EJdataTable.Tables[0].ImportRow(dvr);
                        }
                        EJdataTable.AcceptChanges();
                        break;

                    case "ATM":
                        //ATM 54 & 53
                        FillerLength = new ArrayList();
                        EFTType = "NORMAL";
                        arr.Add("RETURNCODE");
                        arr.Add("VOUCHERCODE");
                        arr.Add("CURRENCYCODE");
                        arr.Add("VALUE");
                        arr.Add("FILLER");
                        arr.Add("TOBANK");
                        arr.Add("FILLER");
                        arr.Add("FROMBANK");
                        arr.Add("FILLER");
                        arr.Add("PROCESSINGNO");
                        arr.Add("FILLER");
                        FillerLength.Add("1");
                        FillerLength.Add("18");
                        FillerLength.Add("23");
                        FillerLength.Add("80");
                        Itemcount = 11;
                        arryRow = new Object[Itemcount];
                        drHeaderFileFormatResult = dstrxClearing.Tables[0].Select("ReturnCode = '97' AND ToBank " + Banks + "  AND TRXTYPE='OD'");
                        foreach (DataRow dvr in drHeaderFileFormatResult)
                        {
                            EJdataTable.Tables[0].ImportRow(dvr);
                        }
                        EJdataTable.AcceptChanges();
                        break;
                }
                t = 0;
                FillerCount = 0;
                try
                {
                    foreach (DataColumn cvr in WorkingDataTable.Tables[0].Columns)
                    {
                        switch (cvr.ColumnName.ToString().ToUpper())
                        {
                            case "FIELDNAME":
                                foreach (DataRow DataDvr in EJdataTable.Tables[0].Rows)
                                {
                                    switch (EFTType.ToString().ToUpper())
                                    {
                                        case "NORMAL":
                                            for (w = 0; w < arr.Count; w++)
                                            {
                                                if (conn.State != ConnectionState.Open)
                                                {
                                                    conn = GetConnection();
                                                }
                                                ColumnFieldName = arr[w].ToString();
                                                DataRow[] FieldRow = WorkingDataTable.Tables[0].Select("FIELDNAME = '" + ColumnFieldName + "' AND RecordType = 'DATA' AND FileType='EFT'");
                                                FileFormatValue = BRBase.BRBaseConvert.ConvertToInt32(FieldRow[0]["Length"].ToString());

                                                FieldRow = null;
                                                FieldRow = WorkingDataTable.Tables[0].Select("FIELDNAME = '" + ColumnFieldName + "' AND RecordType = 'DATA' AND FileType='EFT'");
                                                if (FieldRow[0]["IsLengthMandatoryFieldSize"].ToString() == "")
                                                {
                                                    FileFormatValueMandatoryLength = false;
                                                }
                                                else
                                                {
                                                    FileFormatValueMandatoryLength = BRBase.BRBaseConvert.ConvertToBoolean(FieldRow[0]["IsLengthMandatoryFieldSize"].ToString());
                                                }
                                                FieldRow = null;
                                                FieldRow = WorkingDataTable.Tables[0].Select("FIELDNAME = '" + ColumnFieldName + "' AND RecordType = 'DATA' AND FileType='EFT'");
                                                Filler = BRBase.BRBaseConvert.ConvertToChar(FieldRow[0]["Filler"].ToString());
                                                TrxRowID = BRBase.BRBaseConvert.ConvertToString(DataDvr["TrxRowID"]).Trim();
                                                IncrNo = IncrNo + 1;
                                                switch (arr[w].ToString().ToUpper())
                                                {
                                                    case "RETURNCODE":
                                                    case "VOUCHERCODE":
                                                    case "CURRENCYCODE": //This only Applies for Truncation, to be uncommented if trucation gone life.
                                                    case "FROMBRANCH":
                                                    case "NAMEOFEMPLOYEE":
                                                    case "VATPINNO":
                                                    case "SERIALNUMBER":
                                                    case "REGISTEREDVATNAME":
                                                        Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName]).Trim();
                                                        ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Trim().Length);
                                                        if (ValueLength > FileFormatValue)
                                                        {
                                                            Value = Value.ToString().Substring(0, FileFormatValue);
                                                        }
                                                        ValueLength = BRBase.BRBaseConvert.ConvertToInt32(Value.ToString().Trim().Length);
                                                        if (FileFormatValueMandatoryLength)
                                                        {
                                                            Value = Value.PadLeft((BRBase.BRBaseConvert.ConvertToInt32(FileFormatValue)), Filler);
                                                        }

                                                        arryRow[t] = Value;
                                                        t = t + 1;
                                                        break;
                                                    case "ESLIPNUMBER":
                                                        Value = BRBase.BRBaseConvert.ConvertToString(DataDvr["FROMBRANCH"]).Trim();
                                                        ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr["FROMBRANCH"].ToString().Trim().Length);
                                                        if (ValueLength > FileFormatValue)
                                                        {
                                                            Value = Value.ToString().Substring(0, FileFormatValue);
                                                        }
                                                        ValueLength = BRBase.BRBaseConvert.ConvertToInt32(Value.ToString().Trim().Length);
                                                        if (FileFormatValueMandatoryLength)
                                                        {
                                                            Value = Value.PadLeft((BRBase.BRBaseConvert.ConvertToInt32(FileFormatValue)), Filler);
                                                        }

                                                        arryRow[t] = Value;
                                                        t = t + 1;
                                                        break;

                                                    case "VATSERIALNO":
                                                        Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName]).Trim();
                                                        ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Trim().Length);
                                                        if (ValueLength > FileFormatValue)
                                                        {
                                                            Value = Value.Substring(0, FileFormatValue);
                                                            ValueLength = BRBase.BRBaseConvert.ConvertToInt32(Value.ToString().Trim().Length);
                                                        }
                                                        if (FileFormatValueMandatoryLength)
                                                        {
                                                            Value = Value.PadLeft((BRBase.BRBaseConvert.ConvertToInt32(FileFormatValue)), Filler);
                                                        }

                                                        arryRow[t] = Value;
                                                        t = t + 1;
                                                        break;
                                                    case "VATPAYTYPE":
                                                        Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName]).Trim();
                                                        switch (Value)
                                                        {
                                                            case "01":
                                                            case "02":
                                                            case "03":
                                                            case "04":
                                                            case "05":
                                                            case "06":
                                                            case "07":
                                                                Value = Value;
                                                                break;
                                                            default:
                                                                Value = "01";
                                                                break;
                                                        }
                                                        ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Trim().Length);
                                                        if (ValueLength > FileFormatValue)
                                                        {
                                                            Value = Value.ToString().Substring(0, FileFormatValue);
                                                        }
                                                        if (FileFormatValueMandatoryLength)
                                                        {
                                                            Value = Value.PadLeft((BRBase.BRBaseConvert.ConvertToInt32(FileFormatValue)), Filler);
                                                        }

                                                        arryRow[t] = Value;
                                                        t = t + 1;
                                                        break;
                                                    case "TOBRANCH":
                                                        switch (Arr2[i].ToString())
                                                        {
                                                            case "NORMAL":
                                                                Value = BRBase.BRBaseConvert.ConvertToString(DataDvr["TOBRANCH"]).Trim();
                                                                ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr["TOBRANCH"].ToString().Trim().Length);
                                                                Value = new string('0', FileFormatValue - ValueLength) + Value;
                                                                break;
                                                            case "VAT":
                                                            case "COMMISSION":
                                                            case "PAYE":
                                                                Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName]).Trim();
                                                                ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Trim().Length);
                                                                if (FileFormatValueMandatoryLength)
                                                                {
                                                                    Value = Value.PadLeft((BRBase.BRBaseConvert.ConvertToInt32(FileFormatValue)), Filler);
                                                                }
                                                                break;
                                                        }
                                                        arryRow[t] = Value;
                                                        t = t + 1;
                                                        break;
                                                    case "TOBANK":
                                                        Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName].ToString().Trim());
                                                        ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Trim().Length);
                                                        switch (Arr2[i].ToString())
                                                        {
                                                            case "COMMISSION":
                                                            case "VAT":
                                                            case "PAYE":
                                                            case "NORMAL":
                                                                if (FileFormatValueMandatoryLength)
                                                                {
                                                                    switch (arryRow[0].ToString())
                                                                    {
                                                                        case "00":
                                                                        case "90":
                                                                        case "97":
                                                                            Value = new string(Filler, FileFormatValue - ToBank.ToString().Trim().Length) + ToBank.ToString().Trim();
                                                                            break;
                                                                        default:
                                                                            Value = new string(Filler, FileFormatValue - usrInfo.strBank.Length) + usrInfo.strBank;
                                                                            break;
                                                                    }
                                                                }
                                                                break;
                                                        }
                                                        BankTO = BRBase.BRBaseConvert.ConvertToString(Value);
                                                        arryRow[t] = Value;
                                                        t = t + 1;
                                                        break;
                                                    case "BENEFICIARYNAME":
                                                        Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName]).Trim();
                                                        ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Trim().Length);
                                                        if (ValueLength > FileFormatValue)
                                                        {
                                                            Value = Value.ToString().Substring(0, FileFormatValue);
                                                        }
                                                        ValueLength = BRBase.BRBaseConvert.ConvertToInt32(Value.ToString().Trim().Length);

                                                        switch (Arr2[i].ToString())
                                                        {
                                                            case "COMMISSION":
                                                                Value = new string(Filler, FileFormatValue - ValueLength) + Value;
                                                                break;
                                                            case "VAT":
                                                            case "PAYE":
                                                            case "NORMAL":
                                                                if (FileFormatValueMandatoryLength)
                                                                {
                                                                    //Value = new string(' ', 35 - ValueLength) + Value.ToString().Trim();
                                                                    Value = Value.PadRight(BRBase.BRBaseConvert.ConvertToInt32(FileFormatValue));
                                                                }
                                                                break;
                                                        }
                                                        arryRow[t] = Value;
                                                        t = t + 1;
                                                        break;
                                                    case "PROCESSINGNO":
                                                        dsUniqueClearingID = new BRBase.BRDataSet();
                                                        UniqueID = string.Empty;
                                                        NewValue = string.Empty;
                                                        Int32 NewFileFormatValue = 0;
                                                        if (conn.State != ConnectionState.Open)
                                                        {
                                                            conn = GetConnection();
                                                        }
                                                        ClearingUniversalMethod(usrInfo, "p_GetUniqueClearingDRNID", out dsUniqueClearingID, BRBase.BRModule.InwardFileImportation, conn, new object[] { "BrDataSet" }, new object[] { });
                                                        if (dsUniqueClearingID.Tables.Count > 0)
                                                        {
                                                            if (dsUniqueClearingID.Tables[0].Rows.Count > 0)
                                                            {
                                                                UniqueID = dsUniqueClearingID.Tables[0].Rows[0]["UniqueID"].ToString();
                                                            }
                                                        }
                                                        UniqueID = UniqueID + ToBank.ToString().Trim();
                                                        Value = BRBase.BRBaseConvert.ConvertToString(UniqueID);
                                                        ValueLength = BRBase.BRBaseConvert.ConvertToInt32(UniqueID.ToString().Length);
                                                        NewFileFormatValue = FileFormatValue - BRBase.BRBaseConvert.ConvertToInt32(IncrNo.ToString().Length);
                                                        if (ValueLength > NewFileFormatValue)
                                                        {
                                                            Value = Value.ToString().Substring(0, NewFileFormatValue);
                                                        }
                                                        if (FileFormatValueMandatoryLength)
                                                        {
                                                            Value = Value.PadLeft((BRBase.BRBaseConvert.ConvertToInt32(NewFileFormatValue)), Filler);
                                                        }
                                                        Value = Value + IncrNo.ToString();
                                                        arryRow[t] = Value;
                                                        t = t + 1;
                                                        break;
                                                    case "VATPAYEMONTH":
                                                        Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName]).Trim();
                                                        ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Trim().Length);
                                                        if (FileFormatValueMandatoryLength)
                                                        {
                                                            Value = Value.PadLeft((BRBase.BRBaseConvert.ConvertToInt32(FileFormatValue)), Filler);
                                                        }

                                                        arryRow[t] = Value;
                                                        t = t + 1;
                                                        break;
                                                    case "PAYMENTDATE":
                                                        Value = ddmmmyyyy;
                                                        arryRow[t] = Value;
                                                        t = t + 1;
                                                        break;
                                                    case "FROMBANK":
                                                        switch (arryRow[0].ToString())
                                                        {
                                                            case "00":
                                                            case "90":
                                                            case "97":
                                                                arryRow[t] = usrInfo.strBank;
                                                                break;
                                                            default:
                                                                arryRow[t] = ToBank.ToString().Trim();
                                                                break;
                                                        }
                                                        t = t + 1;
                                                        break;
                                                    case "TOACCOUNT":
                                                        switch (Arr2[i].ToString())
                                                        {
                                                            case "COMMISSION":
                                                                Value = BRBase.BRBaseConvert.ConvertToString(DataDvr["TOACCOUNT"].ToString().Trim());
                                                                ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr["TOACCOUNT"].ToString().Trim().Length);
                                                                if (FileFormatValueMandatoryLength)
                                                                {
                                                                    //Value = new string(Filler, FileFormatValue - ValueLength) + Value;
                                                                    Value = Value.PadLeft((BRBase.BRBaseConvert.ConvertToInt32(FileFormatValue)), Filler);
                                                                }
                                                                break;
                                                            case "VAT":
                                                            case "PAYE":
                                                            case "NORMAL":
                                                                Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName].ToString().Trim());
                                                                ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Trim().Length);
                                                                if (FileFormatValueMandatoryLength)
                                                                {
                                                                    //Value = new string(Filler, FileFormatValue - ValueLength) + Value;
                                                                    Value = Value.PadLeft((BRBase.BRBaseConvert.ConvertToInt32(FileFormatValue)), Filler);

                                                                }
                                                                break;
                                                        }
                                                        arryRow[t] = Value;
                                                        t = t + 1;
                                                        break;
                                                    case "COLLECTIONACCOUNT":
                                                    case "PACCOUNT":
                                                        switch (Arr2[i].ToString())
                                                        {
                                                            case "COMMISSION":
                                                                Value = BRBase.BRBaseConvert.ConvertToString(DataDvr["COLLECTIONACCOUNT"].ToString().Trim());
                                                                ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr["COLLECTIONACCOUNT"].ToString().Trim().Length);
                                                                if (FileFormatValueMandatoryLength)
                                                                {
                                                                    Value = new string(Filler, FileFormatValue - ValueLength) + Value;
                                                                }
                                                                break;
                                                            case "VAT":
                                                            case "PAYE":
                                                            case "NORMAL":
                                                                Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName].ToString().Trim());
                                                                ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Trim().Length);
                                                                if (FileFormatValueMandatoryLength)
                                                                {
                                                                    Value = Value.PadLeft((BRBase.BRBaseConvert.ConvertToInt32(FileFormatValue)), Filler);
                                                                }
                                                                break;
                                                        }
                                                        arryRow[t] = Value;
                                                        t = t + 1;
                                                        break;
                                                    case "ORIGINATORREFERENCECODE":
                                                        switch (Arr2[i].ToString())
                                                        {
                                                            case "COMMISSION":
                                                                Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName]);
                                                                ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Length);
                                                                if (FileFormatValueMandatoryLength)
                                                                {
                                                                    Value = new string(Filler, 15 - ValueLength) + Value;
                                                                }
                                                                break;
                                                            case "VAT":
                                                            case "PAYE":
                                                            case "NORMAL":
                                                                if (FileFormatValueMandatoryLength)
                                                                {
                                                                    Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName]);
                                                                    ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Length);
                                                                    if (FileFormatValueMandatoryLength)
                                                                    {
                                                                        //Value = new string(' ', FileFormatValue - ValueLength) + Value;
                                                                        Value = Value.PadRight(BRBase.BRBaseConvert.ConvertToInt32(FileFormatValue));
                                                                    }
                                                                    if (ColumnFieldName.ToString().ToUpper() == "VALUE")
                                                                    {
                                                                        Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(DataDvr[ColumnFieldName])));
                                                                        //Value = new string(' ', FileFormatValue - ValueLength) + Value;
                                                                        Value = Value.PadRight(BRBase.BRBaseConvert.ConvertToInt32(FileFormatValue));
                                                                    }
                                                                }
                                                                break;
                                                        }
                                                        arryRow[t] = Value;
                                                        t = t + 1;
                                                        break;
                                                    case "VALUE":
                                                    case "COMMISSIONCHARGED":
                                                        Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(DataDvr[ColumnFieldName])));
                                                        ValueLength = BRBase.BRBaseConvert.ConvertToInt32(Value.ToString().Length);
                                                        if (FileFormatValueMandatoryLength)
                                                        {
                                                            //Value = new string(Filler, FileFormatValue - ValueLength) + Value;
                                                            Value = Value.PadLeft((BRBase.BRBaseConvert.ConvertToInt32(FileFormatValue)), Filler);
                                                        }
                                                        arryRow[t] = Value;
                                                        t = t + 1;
                                                        break;
                                                    case "ORIGINATORCODE":
                                                        Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName]);
                                                        ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Length);
                                                        if (FileFormatValueMandatoryLength)
                                                        {
                                                            Value = Value.PadRight((BRBase.BRBaseConvert.ConvertToInt32(FileFormatValue)), Filler);
                                                        }
                                                        if (ColumnFieldName.ToString().ToUpper() == "VALUE")
                                                        {
                                                            Value = BRBase.BRBaseConvert.ConvertToString(ClearingValidations.ValidateAmount(BRBase.BRBaseConvert.ConvertToDouble(DataDvr[ColumnFieldName])));
                                                            Value = Value.PadRight((BRBase.BRBaseConvert.ConvertToInt32(FileFormatValue)), Filler);
                                                        }
                                                        arryRow[t] = Value;
                                                        t = t + 1;
                                                        break;
                                                    case "POLICYNUMBER1":
                                                    case "POLICYNUMBER2":
                                                        Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName]);
                                                        ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Length);
                                                        if (ValueLength > FileFormatValue)
                                                        {
                                                            Value = Value.ToString().Substring(0, FileFormatValue);
                                                        }
                                                        if (FileFormatValueMandatoryLength)
                                                        {
                                                            Value = Value.PadLeft((BRBase.BRBaseConvert.ConvertToInt32(FileFormatValue)), Filler);
                                                        }
                                                        arryRow[t] = Value;
                                                        t = t + 1;
                                                        break;
                                                    case "REMARKS":
                                                        Value = BRBase.BRBaseConvert.ConvertToString(DataDvr[ColumnFieldName]);
                                                        ValueLength = BRBase.BRBaseConvert.ConvertToInt32(DataDvr[ColumnFieldName].ToString().Length);
                                                        if (ValueLength > FileFormatValue)
                                                        {
                                                            Value = Value.ToString().Substring(0, FileFormatValue);
                                                        }
                                                        if (FileFormatValueMandatoryLength)
                                                        {
                                                            Value = Value.PadLeft((BRBase.BRBaseConvert.ConvertToInt32(FileFormatValue)), Filler);
                                                        }
                                                        arryRow[t] = Value;
                                                        t = t + 1;
                                                        break;
                                                    case "REFNO":
                                                        dsUniqueClearingID = new BRBase.BRDataSet();
                                                        UniqueID = string.Empty;
                                                        NewValue = string.Empty;
                                                        NewFileFormatValue = 0;
                                                        if (conn.State != ConnectionState.Open)
                                                        {
                                                            conn = GetConnection();
                                                        }
                                                        ClearingUniversalMethod(usrInfo, "p_GetUniqueClearingDRNID", out dsUniqueClearingID, BRBase.BRModule.InwardFileImportation, conn, new object[] { "BrDataSet" }, new object[] { });
                                                        if (dsUniqueClearingID.Tables.Count > 0)
                                                        {
                                                            if (dsUniqueClearingID.Tables[0].Rows.Count > 0)
                                                            {
                                                                UniqueID = dsUniqueClearingID.Tables[0].Rows[0]["UniqueID"].ToString();
                                                            }
                                                        }
                                                        UniqueID = UniqueID + ToBank.ToString().Trim();
                                                        Value = BRBase.BRBaseConvert.ConvertToString(UniqueID);
                                                        ValueLength = BRBase.BRBaseConvert.ConvertToInt32(UniqueID.ToString().Length);
                                                        NewFileFormatValue = FileFormatValue - BRBase.BRBaseConvert.ConvertToInt32(IncrNo.ToString().Length);
                                                        if (ValueLength > NewFileFormatValue)
                                                        {
                                                            Value = Value.ToString().Substring(0, NewFileFormatValue);
                                                        }
                                                        if (FileFormatValueMandatoryLength)
                                                        {
                                                            Value = Value.PadLeft((BRBase.BRBaseConvert.ConvertToInt32(NewFileFormatValue)), Filler);
                                                        }
                                                        Value = Value + IncrNo.ToString();
                                                        arryRow[t] = Value;
                                                        t = t + 1;
                                                        break;
                                                    case "FILLER":
                                                        Value = new string('0', BRBase.BRBaseConvert.ConvertToInt32(FillerLength[FillerCount]));
                                                        FillerCount += 1;
                                                        arryRow[t] = Value;
                                                        t = t + 1;
                                                        break;
                                                }
                                            }
                                            break;
                                    }
                                    Data = string.Empty;
                                    if (Header == 0)
                                    {
                                        if (conn.State != ConnectionState.Open)
                                        {
                                            conn = GetConnection();
                                        }
                                        dsUniqueClearingID = new BRBase.BRDataSet();
                                        UniqueID = string.Empty;
                                        ClearingUniversalMethod(usrInfo, "p_GetUniqueClearingID", out dsUniqueClearingID, BRBase.BRModule.InwardFileImportation, conn, new object[] { "BrDataSet" }, new object[] { FileFormatValue });
                                        if (dsUniqueClearingID.Tables.Count > 0)
                                        {
                                            if (dsUniqueClearingID.Tables[0].Rows.Count > 0)
                                            {
                                                UniqueID = dsUniqueClearingID.Tables[0].Rows[0]["UniqueID"].ToString();
                                            }
                                        }
                                        else
                                        {
                                            UniqueID = new string('0', 8);
                                        }

                                        UniqueID = new string('0', 8 - UniqueID.Length) + UniqueID;
                                        Data = "186" + ddmmmyyyy + usrInfo.strBank + "000000" + UniqueID + "100" + new string('0', 128);
                                        dt.Rows.Add(Data);
                                        Header += 1;
                                    }
                                    Data = string.Empty;
                                    if (arryRow[0].ToString() != "00" && arryRow[0].ToString() != "90" && arryRow[0].ToString() != "97")
                                    {
                                        isUnpaid = true;
                                    }
                                    if (arryRow[0] != null)
                                    {
                                        for (Int32 m = 0; m < arryRow.Length; m++)
                                        {
                                            if (isUnpaid == true)
                                            {
                                                isUnpaid = true;
                                                UnpaidData = UnpaidData + (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
                                               
                                            }
                                            else
                                            {
                                                Data = Data + (BRBase.BRBaseConvert.ConvertToString(arryRow[m]));
                                            }
                                        }
                                        if (Data != string.Empty)
                                        {
                                            dt.Rows.Add(Data);
                                        }
                                        if (isUnpaid == true)
                                        {
                                            
                                            if (conn.State != ConnectionState.Open)
                                            {
                                                conn = GetConnection();
                                            }
                                            BRBase.BRDataSet dsOriginalUnpaidTrx = new BRBase.BRDataSet();
                                            //First I retrieve what came in, so that i just change the returncode
                                            ClearingUniversalMethod(usrInfo, "p_GetTheOriginalUnpaidTrx", out dsOriginalUnpaidTrx, BRBase.BRModule.GenerateClearingFile, conn, new object[] { "BrDataSet" }, new object[] { TrxRowID });
                                            if (dsOriginalUnpaidTrx.Tables.Count > 0)
                                            {
                                                //System.Windows.Forms.MessageBox.Show(dsOriginalUnpaidTrx.Tables.Count.ToString ());
                                                if (dsOriginalUnpaidTrx.Tables[0].Rows.Count > 0)
                                                {
                                                    if (dsOriginalUnpaidTrx.Tables[0].Rows[0]["Data"].ToString() != "NIL")
                                                    {
                                                        //System.Windows.Forms.MessageBox.Show(dsOriginalUnpaidTrx.Tables[0].Rows.Count.ToString());
                                                        //System.Windows.Forms.MessageBox.Show("Imegia Hapa");
                                                        UnpaidData = dsOriginalUnpaidTrx.Tables[0].Rows[0]["Data"].ToString();
                                                        //System.Windows.Forms.MessageBox.Show(UnpaidData.ToString());
                                                        UnpaidData = UnpaidData.Substring(2);
                                                        UnpaidData = arryRow[0].ToString() + UnpaidData;
                                                    }
                                                }
                                            }
                                            //System.Windows.Forms.MessageBox.Show("Imetoka Hapa");
                                            //System.Windows.Forms.MessageBox.Show(UnpaidData.ToString());
                                            UnpaidColl.Add(UnpaidData);
                                        }
                                        Data = string.Empty;
                                        UnpaidData = string.Empty;
                                        isUnpaid = false;
                                    }
                                    t = 0;
                                    arryRow = new Object[Itemcount];
                                    FillerCount = 0;
                                    Value = string.Empty;
                                    EJdataTable = new DS_trxClearing();
                                }
                                break;
                        }
                    }
                    FillerLength = new ArrayList();
                    arr = new ArrayList();
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }
            if (dt.Rows.Count > 0)
            {
                if (UnpaidColl.Count > 0)
                {
                    Data = "2272" + new string('0', 154);
                    dt.Rows.Add(Data);
                    Data = string.Empty;
                    for (Int32 m = 0; m < UnpaidColl.Count; m++)
                    {
                        dt.Rows.Add(UnpaidColl[m]);
                    }
                }
                Data = "19" + usrInfo.strBank + new string('0', 154);
                dt.Rows.Add(Data);
            }
            UnpaidColl.Clear();
            return dt;
        }
        //static public IDbConnection GetConnection(string strSystem)
        //{

         
        //    string strConnectString = strSystem;
        //    strConnectString = strSystem;
        //    IDbConnection connection = null;
        //    if (ConfigurationManager.AppSettings["BRDBType"].ToUpper() == "SQLSERVER")
        //        connection = new System.Data.SqlClient.SqlConnection(strConnectString);
        //    else if (ConfigurationManager.AppSettings["BRDBType"].ToUpper() == "ORACLE")
        //        connection = new OracleConnection(strConnectString);
        //    connection.Open();
        //    return connection;
        //}

        public static bool ClearingUniversalMethod(BRBase.UserInfo usrinfo, string Sp_Name, out BRBase.BRDataSet dsResults,BRBase.BRModule testMod,IDbConnection conn, object[] ValuesDSNamesArray, params object[] ValuesParamarray)
        {
            Int32 Counter = 0;
           BRBase.BRDataSet dsRetreavedData = new BRBase.BRDataSet();
           IDBHelper intfDBHelper = DBClient.GetDBHelper(usrinfo);
            try
            {
                using (IDbConnection connection = conn)
                {
                    
                    IDbDataParameter[] inputParamList = (IDbDataParameter[])intfDBHelper.GetSPParameters(connection, Sp_Name);
                    dsRetreavedData = new BRBase.BRDataSet();

                    if ((inputParamList == null) || (ValuesParamarray == null))
                    {
                        dsResults = (BRBase.BRDataSet)dsRetreavedData.Copy();
                        return false;
                    }
                    if (inputParamList.Length != ValuesParamarray.Length)
                    {
                        throw new ArgumentException("Parameter count does not match Parameter Value count.");
                    }
                    for (int i = 0, j = inputParamList.Length; i < j; i++)
                    {
                        if (ValuesParamarray[i] is IDbDataParameter)
                        {
                            IDbDataParameter paramInstance = (IDbDataParameter)ValuesParamarray[i];
                            if (paramInstance.Value == null)
                            {
                                inputParamList[i].Value = DBNull.Value;
                            }
                            else
                            {
                                inputParamList[i].Value = paramInstance.Value;
                            }
                        }
                        else if (ValuesParamarray[i] == null)
                        {
                            inputParamList[i].Value = DBNull.Value;
                        }
                        else
                        {
                            inputParamList[i].Value = ValuesParamarray[i];
                        }
                    }
                    intfDBHelper.FillDataset(connection, CommandType.StoredProcedure, Sp_Name, dsRetreavedData, new string[] { "BrNames", "BRBillsOtherDetails", "BillDates", "Bills" }, inputParamList);
                    dsResults = (BRBase.BRDataSet)dsRetreavedData.Copy();
                    return true;
                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.ToString());
                //throw DBClientUtils.GetDBErrorMessages(ex, usrinfo.strUser, usrinfo.strSystem);
                dsResults = null;
                return true;
            }
            if (dsRetreavedData.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            
        }
    }
}
