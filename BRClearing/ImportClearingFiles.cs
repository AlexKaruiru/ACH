using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Security.Cryptography;
using ImageFileRead;
using BRBase;
using Microsoft.VisualBasic;


namespace BRClearing.Util
{
    public class ImportClearingFiles
    {
        static DataTable WorkingTable;
        static Hashtable LineItemsTable;
        static string filePath;
        static string line;
        static StreamReader file;
        static StringCollection myCol;
        static Int32 Item;
        static string My_Line;
        static StringCollection WorkedCol;
        static StringCollection WorkedColl;
        static ArrayList FileArr;
        static bool IsLCY;
        static bool IsFCY;
        static Int32 k;
        static string[] EJfilePaths;
        static string[] EFTfilePaths;
        static  string[] DISCfilePaths;
        static string[] SELTfilePaths;
        static string[] EJFCYfilePaths;
        static string[] SELTFCYfilePaths;
        static string strFileType;
        static string[] fImageBW;
        static string[] fImage;
        static string[] bImage;
        public ImportClearingFiles()
        {
            WorkingTable = new DataTable();
            LineItemsTable = new Hashtable();
            filePath = string.Empty;
            myCol = new StringCollection();
            WorkedCol = new StringCollection();
            WorkedColl = new StringCollection();
            line = string.Empty;
            My_Line = string.Empty;
            file = null;
            Item = 1;
            FileArr = new ArrayList();
            IsLCY = true;
            IsFCY = false;
            k = 0;
            EJfilePaths = null;
            EFTfilePaths = null;
            DISCfilePaths = null;
            SELTfilePaths = null;
            EJFCYfilePaths = null;
            SELTFCYfilePaths = null;
            strFileType = null;
            fImageBW = null;
            fImage = null;
            bImage = null;
        }
        private static StringCollection GetThaFlatFiles(string fileName, string FileType, string FileDate)
        {
            FileInfo fileInfo = new FileInfo(fileName);
            StringCollection ReWorkedStringCol = new StringCollection();
            StringCollection SelectedStringCol = new StringCollection();
            DateTime WorkingDate = new DateTime();
            FileDate = FileDate.Substring(0, 10);
            FileDate = FileDate.Replace("-", "");
            string Day = FileDate.Substring(6, 2);
            string Month = FileDate.Substring(4, 2);
            string FDayMonth = new string('0', 2 - (BRBaseConvert.ConvertToString(Day).Length)) + BRBaseConvert.ConvertToString(Day) + new string('0', 2 - (BRBaseConvert.ConvertToString(Month).Length)) + BRBaseConvert.ConvertToString(Month);
            string DirName = string.Empty;
            string[] EJfilePaths = null;
            if (!fileInfo.Exists)
            {
                //DirName = fileInfo.DirectoryName;
                DirName = fileName;
            }

            string Country = ConfigurationManager.AppSettings["Country"].Trim().ToUpper();

            if (Country.ToUpper() == "KE")
            {
                try
                {
                    //DirName = fileInfo.DirectoryName;
                    EJfilePaths = Directory.GetFiles(@"" + DirName + "");
                    EJfilePaths = Directory.GetFiles(DirName);
                    for (Int32 f = 0; f < EJfilePaths.Length; f++)
                    {
                        if (EJfilePaths[f].Substring(EJfilePaths[f].ToString().LastIndexOf("\\") + 1).Substring(2, 4) == FDayMonth)
                        {
                            SelectedStringCol.Add(EJfilePaths[f].ToString());
                        }
                    }

                    switch (FileType.ToString().ToUpper())
                    {
                        case "EJ":
                            for (Int32 p = 0; p < SelectedStringCol.Count; p++)
                            {
                                if (ConfigurationManager.AppSettings["sysEnc"] == "1")
                                {
                                    if (SelectedStringCol[p].Substring(SelectedStringCol[p].ToString().IndexOf(".") + 1).Substring(0, 1).ToUpper() == "J")
                                    {
                                        ReWorkedStringCol.Add(SelectedStringCol[p].ToString());
                                    }
                                }
                                else
                                {
                                    if (SelectedStringCol[p].Substring(SelectedStringCol[p].ToString().LastIndexOf(".") + 1).Substring(0, 1).ToUpper() == "J")
                                    {
                                        ReWorkedStringCol.Add(SelectedStringCol[p].ToString());
                                    }
                                }
                            }
                            break;
                        case "DD":
                            for (Int32 p = 0; p < SelectedStringCol.Count; p++)
                            {
                                if (ConfigurationManager.AppSettings["sysEnc"] == "1")
                                {
                                    if (SelectedStringCol[p].Substring(SelectedStringCol[p].ToString().IndexOf(".") + 1).Substring(0, 1).ToUpper() == "M")
                                    {
                                        ReWorkedStringCol.Add(SelectedStringCol[p].ToString());
                                    }
                                }
                                else
                                {
                                    if (SelectedStringCol[p].Substring(SelectedStringCol[p].ToString().LastIndexOf(".") + 1).Substring(0, 1).ToUpper() == "M")
                                    {
                                        ReWorkedStringCol.Add(SelectedStringCol[p].ToString());
                                    }
                                }
                            }
                            break;
                        case "EFT":
                            for (Int32 p = 0; p < SelectedStringCol.Count; p++)
                            {
                                if (ConfigurationManager.AppSettings["sysEnc"] == "1")
                                {
                                    if (SelectedStringCol[p].Substring(SelectedStringCol[p].ToString().IndexOf(".") + 1).Substring(0, 1).ToUpper() == "T")
                                    {
                                        ReWorkedStringCol.Add(SelectedStringCol[p].ToString());
                                    }
                                }
                                else
                                {
                                    if (SelectedStringCol[p].Substring(SelectedStringCol[p].ToString().LastIndexOf(".") + 1).Substring(0, 1).ToUpper() == "T")
                                    {
                                        ReWorkedStringCol.Add(SelectedStringCol[p].ToString());
                                    }
                                }
                            }
                            break;
                        case "DISC":
                            for (Int32 p = 0; p < SelectedStringCol.Count; p++)
                            {
                                if (SelectedStringCol[p].Substring(SelectedStringCol[p].ToString().LastIndexOf(".") + 1).Substring(0, 1).ToUpper() == "D")
                                {
                                    ReWorkedStringCol.Add(SelectedStringCol[p].ToString());
                                }
                            } break;
                        case "SELT":
                            for (Int32 p = 0; p < SelectedStringCol.Count; p++)
                            {
                                if (ConfigurationManager.AppSettings["sysEnc"] == "1")
                                {
                                    if (SelectedStringCol[p].Substring(SelectedStringCol[p].ToString().IndexOf(".") + 1).Substring(0, 1).ToUpper() == "P")
                                    {
                                        ReWorkedStringCol.Add(SelectedStringCol[p].ToString());
                                    }
                                }
                                else
                                {
                                    if (SelectedStringCol[p].Substring(SelectedStringCol[p].ToString().LastIndexOf(".") + 1).Substring(0, 1).ToUpper() == "P")
                                    {
                                        ReWorkedStringCol.Add(SelectedStringCol[p].ToString());
                                    }
                                }
                            } break;
                        case "FCYEJ":
                            for (Int32 p = 0; p < SelectedStringCol.Count; p++)
                            {
                                if (ConfigurationManager.AppSettings["sysEnc"] == "1")
                                {
                                    if (SelectedStringCol[p].Substring(SelectedStringCol[p].ToString().IndexOf(".") + 1).Substring(0, 1).ToUpper() == "E")
                                    {
                                        ReWorkedStringCol.Add(SelectedStringCol[p].ToString());
                                    }
                                }
                                else
                                {
                                    if (SelectedStringCol[p].Substring(SelectedStringCol[p].ToString().LastIndexOf(".") + 1).Substring(0, 1).ToUpper() == "E")
                                    {
                                        ReWorkedStringCol.Add(SelectedStringCol[p].ToString());
                                    }
                                }
                               
                            } break;
                        case "FCYSELT":
                            for (Int32 p = 0; p < SelectedStringCol.Count; p++)
                            {
                                if (ConfigurationManager.AppSettings["sysEnc"] == "1")
                                {
                                    if (SelectedStringCol[p].Substring(SelectedStringCol[p].ToString().IndexOf(".") + 1).Substring(0, 1).ToUpper() == "K")
                                    {
                                        ReWorkedStringCol.Add(SelectedStringCol[p].ToString());
                                    }
                                }
                                else
                                {
                                    if (SelectedStringCol[p].Substring(SelectedStringCol[p].ToString().LastIndexOf(".") + 1).Substring(0, 1).ToUpper() == "K")
                                    {
                                        ReWorkedStringCol.Add(SelectedStringCol[p].ToString());
                                    }
                                }

                            } break;
                    }
                }
                catch (Exception ex)
                {
                    string AppendErrorMessage = "Error Message: BREXUtility 89 :" + ex.ToString() + Environment.NewLine + "Date" + ":" + DateTime.Now + Environment.NewLine + "--------------------------" + Environment.NewLine;
                    System.IO.File.AppendAllText("C:\\ClearingFiles\\" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + ".txt", AppendErrorMessage);
                }
            }
            else if (Country.ToUpper() == "UG")
            {
                try
                {
                //DirName = fileInfo.DirectoryName;
                    EJfilePaths = Directory.GetFiles(@"" + DirName + "");
                    EJfilePaths = Directory.GetFiles(DirName);
                    for (Int32 f = 0; f < EJfilePaths.Length; f++)
                    {
                        if (EJfilePaths[f].Substring(EJfilePaths[f].ToString().LastIndexOf("\\") + 1).Substring(2, 4) == FDayMonth)
                        {
                            SelectedStringCol.Add(EJfilePaths[f].ToString());
                        }
                    }

                    switch (FileType.ToString().ToUpper())
                    {
                        case "EJ":
                            for (Int32 p = 0; p < SelectedStringCol.Count; p++)
                            {
                                if (SelectedStringCol[p].Substring(SelectedStringCol[p].ToString().LastIndexOf(".") + 1).Substring(0, 1).ToUpper() == "J")
                                {
                                    ReWorkedStringCol.Add(SelectedStringCol[p].ToString());
                                }
                            }
                            break;
                        case "EFT":
                            for (Int32 p = 0; p < SelectedStringCol.Count; p++)
                            {
                                if (SelectedStringCol[p].Substring(SelectedStringCol[p].ToString().LastIndexOf(".") + 1).Substring(0, 1).ToUpper() == "T")
                                {
                                    ReWorkedStringCol.Add(SelectedStringCol[p].ToString());
                                }
                            }
                            break;
                        case "DISC":
                            for (Int32 p = 0; p < SelectedStringCol.Count; p++)
                            {
                                if (SelectedStringCol[p].Substring(SelectedStringCol[p].ToString().LastIndexOf(".") + 1).Substring(0, 1).ToUpper() == "D")
                                {
                                    ReWorkedStringCol.Add(SelectedStringCol[p].ToString());
                                }
                            } break;
                        case "SELT":
                            for (Int32 p = 0; p < SelectedStringCol.Count; p++)
                            {
                                if (SelectedStringCol[p].Substring(SelectedStringCol[p].ToString().LastIndexOf(".") + 1).Substring(0, 1).ToUpper() == "P")
                                {
                                    ReWorkedStringCol.Add(SelectedStringCol[p].ToString());
                                }
                            } break;
                        case "FCYEJ":
                            for (Int32 p = 0; p < SelectedStringCol.Count; p++)
                            {
                                if (SelectedStringCol[p].Substring(SelectedStringCol[p].ToString().LastIndexOf(".") + 1).Substring(0, 1).ToUpper() == "E")
                                {
                                    ReWorkedStringCol.Add(SelectedStringCol[p].ToString());
                                }
                            } break;
                        case "FCYSELT":
                            for (Int32 p = 0; p < SelectedStringCol.Count; p++)
                            {
                                if (SelectedStringCol[p].Substring(SelectedStringCol[p].ToString().LastIndexOf(".") + 1).Substring(0, 1).ToUpper() == "K")
                                {
                                    ReWorkedStringCol.Add(SelectedStringCol[p].ToString());
                                }
                            } break;
                    }
                }
                catch (Exception ex)
                {
                    string AppendErrorMessage = "Error Message: BREXUtility 89 :" + ex.ToString() + Environment.NewLine + "Date" + ":" + DateTime.Now + Environment.NewLine + "--------------------------" + Environment.NewLine;
                    System.IO.File.AppendAllText("C:\\ClearingFiles\\" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + ".txt", AppendErrorMessage);
                }

            }


           return ReWorkedStringCol;
        }

        private static DataTable PGPFiles(string FileName, string DataLine)
        {
            DataTable FData = new DataTable();



            return FData;

        }
        public static DataTable EJFiles(string FileName,string FileDate,string CurrencyType)
        {
            string Country = ConfigurationManager.AppSettings["Country"].Trim().ToUpper();
            DataTable FDataTable = new DataTable();
            string StrFileName = string.Empty;
            string StrFile = string.Empty;
            WorkingTable = new DataTable();
            //UserInfo usrInfo = ASPUtils.getActiveUser();;
            try
            {
                ReadtheImagedFile MyFileData = new ReadtheImagedFile();
                string CurrCode = string.Empty;
                if (CurrencyType == "LOCAL")
                {
                    IsLCY = true;
                    IsFCY = false;
                }
                else
                {
                    IsLCY = false;
                    IsFCY = true;
                }
                strFileType = null;
                if ((IsFCY == false) && (IsLCY == true))
                {
                    strFileType = "EJ";
                    CurrCode = "0";
                }
                else if ((IsFCY == true) && (IsLCY == false))
                {
                    strFileType = "FCYEJ";
                    CurrCode = "00";
                }
                else
                {
                    strFileType = "EJ";
                    CurrCode = "0";
                    strFileType = "FCYEJ";
                }
                string FileBankID = "";
                WorkedCol = GetThaFlatFiles(FileName, strFileType, FileDate);
                for (Int32 x = 0; x < WorkedCol.Count; x++)
                {
                    StrFile = string.Empty;
                    //ImageFileRead.ReadtheImagedFile igm = new ImageFileRead.ReadtheImagedFile();
                    StrFileName = WorkedCol[x].Substring(WorkedCol[x].LastIndexOf("\\") + 1);

                    if (Country.ToUpper() == "KE")
                    {
                        FileBankID = new string('0', 2 - WorkedCol[x].Substring(WorkedCol[x].LastIndexOf(".") + 1).Substring(1, 2).Length) + WorkedCol[x].Substring(WorkedCol[x].LastIndexOf(".") + 1).Substring(1, 2);

                        if (ConfigurationManager.AppSettings["sysEnc"] == "1")
                        {
                            string sFileName = FileName + "\\" + StrFileName;
                            InPGP(sFileName);
                            string origFileName = "";
                            int FirstIndexPstofBackSlash = sFileName.LastIndexOf("\\");
                            int FristIndexPstofDot = sFileName.LastIndexOf(".");
                            int lenOfTheFileName = FristIndexPstofDot - FirstIndexPstofBackSlash;
                            origFileName = sFileName.Substring(FirstIndexPstofBackSlash + 1, lenOfTheFileName - 1);
                            WorkingTable = MyFileData.ReadImagesFromFile(FileName + "\\" + origFileName, CurrCode, FileBankID, IsFCY);
                            File.Delete(FileName + "\\" + origFileName);
                        }
                        else
                        {
                            WorkingTable = MyFileData.ReadImagesFromFile(FileName + "\\" + StrFileName, CurrCode, FileBankID, IsFCY);
                        }

                    }
                    else if (Country.ToUpper() == "UG")
                    {
                        FileBankID = new string('0', 2 - WorkedCol[x].Substring(WorkedCol[x].LastIndexOf(".") + 1).Substring(1, 2).Length) + WorkedCol[x].Substring(WorkedCol[x].LastIndexOf(".") + 1).Substring(1, 2);
                        WorkingTable = ReadImagesFromFile(FileName + "\\" + StrFileName, CurrCode, FileBankID, IsFCY,false);
                    }

                    FDataTable.Merge(WorkingTable);
                    WorkingTable.Clear();
                }

                if (Country.ToUpper() == "KE")
                {
                    for (Int32 x = 0; x < FDataTable.Rows.Count; x++)
                    {
                        if (FDataTable.Rows[x]["ReturnCode"].ToString() == "17" || FDataTable.Rows[x]["ReturnCode"].ToString() != "00")
                        {
                            string strBankID = FDataTable.Rows[x]["FromBank"].ToString();
                            string strBranchID = FDataTable.Rows[x]["FromBranch"].ToString();
                            FDataTable.Rows[x]["FromBank"] = FDataTable.Rows[x]["DestBank"].ToString();
                            FDataTable.Rows[x]["FromBranch"] = FDataTable.Rows[x]["ToBranch"].ToString();
                            FDataTable.Rows[x]["DestBank"] = strBankID;
                            FDataTable.Rows[x]["ToBranch"] = strBranchID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string AppendErrorMessage = "Error Message: BREXUtility 181 :" + ex.ToString() + Environment.NewLine + "Date" + ":" + DateTime.Now + Environment.NewLine + "--------------------------" + Environment.NewLine;
                System.IO.File.AppendAllText("C:\\ClearingFiles\\" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + ".txt", AppendErrorMessage);
            }
            return FDataTable;
        }
        public static DataTable DDFiles(string FileName, string FileDate, bool islegacyData)
        {
            string Country = ConfigurationManager.AppSettings["Country"].Trim().ToUpper();
            DataTable FDataTable = new DataTable();
            string StrFileName = string.Empty;
            string StrFile = string.Empty;
            WorkingTable = new DataTable();
            //UserInfo usrInfo = ASPUtils.getActiveUser();;
            try
            {
                ReadtheImagedFile MyFileData = new ReadtheImagedFile();
                string CurrCode = string.Empty;
                string FileBankID = "";
                    StrFile = string.Empty;
                    StrFileName = FileName.Substring(FileName.LastIndexOf("\\") + 1);

                    if (Country.ToUpper() == "KE")
                    {

                        if (ConfigurationManager.AppSettings["sysEnc"] == "1")
                        {
                            string sFileName = FileName ;
                            InPGP(sFileName);
                            string origFileName = "";
                            int FirstIndexPstofBackSlash = sFileName.LastIndexOf("\\");
                            int FristIndexPstofDot = sFileName.LastIndexOf(".");
                            int lenOfTheFileName = FristIndexPstofDot - FirstIndexPstofBackSlash;
                            origFileName = sFileName.Substring(FirstIndexPstofBackSlash + 1, lenOfTheFileName - 1);

                            if ((StrFileName.Substring(10, 2) == "02" || StrFileName.Substring(10, 2) == "04" || StrFileName.Substring(10, 2) == "05" || StrFileName.Substring(10, 2) == "06") && islegacyData == true)
                            {
                                Collection Mycol = new Collection();
                                Mycol = ReadFlatFileColl(FileName);
                                WorkingTable = MyFileData.DDColl(Mycol, StrFileName, islegacyData);
                            }
                            else
                            {
                                FileBankID = new string('0', 2 - FileName.Substring(FileName.LastIndexOf(".") + 1).Substring(1, 2).Length) + FileName.Substring(FileName.LastIndexOf(".") + 1).Substring(1, 2);
                                WorkingTable = MyFileData.ReadFile(FileName, FileBankID);
                            }

                            File.Delete(FileName + "\\" + origFileName);
                        }
                        else
                        {
                            if ((StrFileName.Substring(10, 2) == "02" || StrFileName.Substring(10, 2) == "04" || StrFileName.Substring(10, 2) == "05" || StrFileName.Substring(10, 2) == "06") && islegacyData == true)
                            {
                                Collection Mycol = new Collection();
                                Mycol = ReadFlatFileColl(FileName);
                                WorkingTable = MyFileData.DDColl(Mycol, StrFileName, islegacyData);
                            }
                            else
                            {
                                FileBankID = new string('0', 2 - FileName.Substring(FileName.LastIndexOf(".") + 1).Substring(1, 2).Length) + FileName.Substring(FileName.LastIndexOf(".") + 1).Substring(1, 2);
                                WorkingTable = MyFileData.ReadFile(FileName, FileBankID);
                            }
                        }


                       
                    }
                    FDataTable.Merge(WorkingTable);
            }
            catch (Exception ex)
            {
                string AppendErrorMessage = "Error Message: BREXUtility 181 :" + ex.ToString() + Environment.NewLine + "Date" + ":" + DateTime.Now + Environment.NewLine + "--------------------------" + Environment.NewLine;
                System.IO.File.AppendAllText("C:\\ClearingFiles\\" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + ".txt", AppendErrorMessage);
            }
            return FDataTable;
        }
        public static DataTable EFTFilesRCP(string FileName, DateTime dtFileDate, string CurrencyType, string strFileT)
        {
            string FileDate = string.Empty;
            string StrFileName = string.Empty;
            DataTable FDataTable = new DataTable();
            WorkingTable = new DataTable();
            DateTime DtVal = dtFileDate;
            string dd = new string('0', 2 - BRBaseConvert.ConvertToString(DtVal.Day).ToString().Length) + BRBaseConvert.ConvertToString(DtVal.Day);
            string mm = new string('0', 2 - BRBaseConvert.ConvertToString(DtVal.Month).Length) + BRBaseConvert.ConvertToString(DtVal.Month);
            string yyyy = BRBaseConvert.ConvertToString(DtVal.Year);
            string ddmmmyyyy = dd + mm + yyyy;
            FileDate = Convert.ToString(String.Format("{0:u}", dtFileDate));
            if (CurrencyType == "LOCAL")
            {
                try
                {
                    strFileType = null;
                    strFileType = strFileT;
                    WorkedCol = GetThaFlatFiles(FileName, strFileType, ddmmmyyyy);
                    for (Int32 x = 0; x < WorkedCol.Count; x++)
                    {
                        if (WorkedCol[x].Substring(WorkedCol[x].LastIndexOf(".") + 1).Substring(0, 3) == strFileType)
                        {
                            StrFileName = WorkedCol[x].Substring(WorkedCol[x].LastIndexOf("\\") + 1);
                            WorkingTable = ColOfFileGenerated(FileName + "\\" + StrFileName, strFileType, FileDate);
                            FDataTable.Merge(WorkingTable);
                            WorkingTable.Clear();
                        }
                    }
                }
                catch (Exception ex)
                {
                    //ex.Message = ex.Message + BRConfigurationManager.BRAppSettings["InwardFilePath"].ToString();
                    //ASPUtils.LogExceptionDetails(ex, DateTime.Now);
                }
            }
            return FDataTable;
        }

        public static DataTable EFTFiles(string FileName, string FileDate,string CurrencyType)
        {
            string StrFileName = string.Empty;
            DataTable FDataTable = new DataTable();
            WorkingTable = new DataTable();
            if (CurrencyType == "LOCAL")
            {
                try
                {
                    strFileType = null;
                    strFileType = "EFT";
                    WorkedCol = GetThaFlatFiles(FileName, strFileType, FileDate);
                    for (Int32 x = 0; x < WorkedCol.Count; x++)
                    {
                        if (ConfigurationManager.AppSettings["sysEnc"] == "1")
                        {
                            if (WorkedCol[x].Substring(WorkedCol[x].IndexOf(".") + 1).Substring(0, 1) == "T")
                            {
                                StrFileName = WorkedCol[x].Substring(WorkedCol[x].LastIndexOf("\\") + 1);
                                string sFileName = FileName + "\\" + StrFileName;
                                InPGP(sFileName);
                                string origFileName = "";
                                int FirstIndexPstofBackSlash = sFileName.LastIndexOf("\\");
                                int FristIndexPstofDot = sFileName.LastIndexOf(".");
                                int lenOfTheFileName = FristIndexPstofDot - FirstIndexPstofBackSlash;
                                origFileName = sFileName.Substring(FirstIndexPstofBackSlash + 1, lenOfTheFileName - 1);
                                WorkingTable = ColOfFileGenerated(FileName + "\\" + origFileName, strFileType, FileDate);
                                FDataTable.Merge(WorkingTable);
                                WorkingTable.Clear();
                                File.Delete(FileName + "\\" + origFileName);
                            }
                            else
                            {
                                if (WorkedCol[x].Substring(WorkedCol[x].LastIndexOf(".") + 1).Substring(0, 1) == "T")
                                {
                                    StrFileName = WorkedCol[x].Substring(WorkedCol[x].LastIndexOf("\\") + 1);
                                    WorkingTable = ColOfFileGenerated(FileName + "\\" + StrFileName, strFileType, FileDate);
                                    FDataTable.Merge(WorkingTable);
                                    WorkingTable.Clear();
                                }
                            }

                        }
                        else
                        {
                            if (WorkedCol[x].Substring(WorkedCol[x].LastIndexOf(".") + 1).Substring(0, 1) == "T")
                            {
                                StrFileName = WorkedCol[x].Substring(WorkedCol[x].LastIndexOf("\\") + 1);
                                WorkingTable = ColOfFileGenerated(FileName + "\\" + StrFileName, strFileType, FileDate);
                                FDataTable.Merge(WorkingTable);
                                WorkingTable.Clear();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string AppendErrorMessage = "Error Message: BREXUtility 240 :" + ex.ToString() + Environment.NewLine + "Date" + ":" + DateTime.Now + Environment.NewLine + "--------------------------" + Environment.NewLine;
                    System.IO.File.AppendAllText("C:\\ClearingFiles\\" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + ".txt", AppendErrorMessage);
                }
            }
            return FDataTable;
        }

        public static DataTable DiscrepanciesFiles(string FileName, string FileDate,string CurrencyType)
        {
            string StrFileName = string.Empty;
            DataTable FDataTable = new DataTable();
            WorkingTable = new DataTable();
            strFileType = "DISC";
            string StrFile = string.Empty;
            string CurrCode = string.Empty;
            CurrCode = "0";
            //ReadtheImagedFile MyFileData = new ReadtheImagedFile();
            string FileBankID = "";
            WorkedCol = GetThaFlatFiles(FileName, strFileType, FileDate);
            if(CurrencyType=="LOCAL")
             try
            {
                for (Int32 x = 0; x < WorkedCol.Count; x++)
                {
                    if (WorkedCol[x].Substring(WorkedCol[x].LastIndexOf(".") + 1).Substring(0, 1) == "D")
                    {
                        StrFile = string.Empty;
                        //ImageFileRead.ReadtheImagedFile img = new ImageFileRead.ReadtheImagedFile();
                        StrFileName = WorkedCol[x].Substring(WorkedCol[x].LastIndexOf("\\") + 1);
                        FileBankID = new string('0', 2 - WorkedCol[x].Substring(WorkedCol[x].LastIndexOf(".") + 1).Substring(1, 2).Length) + WorkedCol[x].Substring(WorkedCol[x].LastIndexOf(".") + 1).Substring(1, 2);
                        WorkingTable = ReadImagesFromFile(FileName + "\\" + StrFileName, CurrCode, FileBankID, false,false);
                        FDataTable.Merge(WorkingTable);
                        WorkingTable.Clear();
                    }
                }
             }
            catch (Exception ex)
            {
                //ex.Message = ex.Message + BRConfigurationManager.BRAppSettings["InwardFilePath"].ToString();
                //ASPUtils.LogExceptionDetails(ex, DateTime.Now);
            }
            return FDataTable;
        }

        public static DataTable SettlementFiles(string FileName, string FileDate, string CurrencyType)
        {
            strFileType = null;
            string StrFileName = string.Empty;
            WorkingTable = new DataTable();
            DataTable FDataTable = new DataTable();
            if (CurrencyType == "LOCAL")
            {
                IsLCY = true;
                IsFCY = false;
            }
            else
            {
                IsLCY = false;
                IsFCY = true;
            }
            if ((IsFCY == false) && (IsLCY == true))
            {
                strFileType = "SELT";
            }
            else if ((IsFCY == true) && (IsLCY == false))
            {
                strFileType = "FCYSELT";
            }
            else
            {
                strFileType = "SELT";
                strFileType = "FCYSELT";
            }
             try
            {
                //strFileType = "";
                WorkedCol = GetThaFlatFiles(FileName, strFileType, FileDate);
                for (Int32 x = 0; x < WorkedCol.Count; x++)
                {

                    if (ConfigurationManager.AppSettings["sysEnc"] == "1")
                    {
                        if (WorkedCol[x].Substring(WorkedCol[x].IndexOf(".") + 1).Substring(0, 1) == "K" || WorkedCol[x].Substring(WorkedCol[x].IndexOf(".") + 1).Substring(0, 1) == "P")
                        {
                            StrFileName = WorkedCol[x].Substring(WorkedCol[x].IndexOf("\\") + 1);
                            string sFileName = FileName + "\\" + StrFileName;
                            InPGP(sFileName);
                            string origFileName = "";
                            int FirstIndexPstofBackSlash = sFileName.LastIndexOf("\\");
                            int FristIndexPstofDot = sFileName.LastIndexOf(".");
                            int lenOfTheFileName = FristIndexPstofDot - FirstIndexPstofBackSlash;
                            origFileName = sFileName.Substring(FirstIndexPstofBackSlash + 1, lenOfTheFileName - 1);
                            WorkingTable = ColOfFileGenerated(FileName + "\\" + StrFileName, strFileType, FileDate);
                            FDataTable.Merge(WorkingTable);
                            WorkingTable.Clear();
                            File.Delete(FileName + "\\" + origFileName);
                        }
                        else
                        {
                            if (WorkedCol[x].Substring(WorkedCol[x].LastIndexOf(".") + 1).Substring(0, 1) == "K" || WorkedCol[x].Substring(WorkedCol[x].LastIndexOf(".") + 1).Substring(0, 1) == "P")
                            {
                                StrFileName = WorkedCol[x].Substring(WorkedCol[x].LastIndexOf("\\") + 1);
                                WorkingTable = ColOfFileGenerated(FileName + "\\" + StrFileName, strFileType, FileDate);
                                FDataTable.Merge(WorkingTable);
                                WorkingTable.Clear();
                            }
                        }

                    }



                   
                }
            }
            catch (Exception ex)
            {
                string AppendErrorMessage = "Error Message: BREXUtility 308 :" + ex.ToString() + Environment.NewLine + "Date" + ":" + DateTime.Now + Environment.NewLine + "--------------------------" + Environment.NewLine;
                System.IO.File.AppendAllText("C:\\ClearingFiles\\" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + ".txt", AppendErrorMessage);
            }
            return FDataTable;
        }
        private static DataTable ColOfFileGenerated(string FileStr, string strFileType, string FileDate)
        {
            DataTable WorkingTable = new DataTable();
            DataTable data = new DataTable();
            try
            {
                //for (k = 0; k < FileStr.Count; k++)
                if (FileStr!="")
                {
                    WorkedColl = ReadFlatFile(FileStr.ToString());

                    if (WorkingTable.Rows.Count > 0)
                    {
                        WorkingTable.Merge(DataIntoTable(WorkedColl, "LOCAL", data, strFileType, FileDate));
                    }
                    else
                    {
                        WorkingTable = DataIntoTable(WorkedColl, "LOCAL", data, strFileType, FileDate);
                    }
                }
            }
            catch (Exception ex)
            {
                return WorkingTable;
            }
            return WorkingTable;
        }
        private static DataTable DataIntoTable(StringCollection myColl, string CurrencyID, DataTable data, string FileType, string FileDate)
        {
            DataTable dt = new DataTable();
            LineItemsTable = new Hashtable();
            string Micrline= string.Empty;
            string BankID = string.Empty;
            string BranchID = string.Empty;
            //UserInfo usrInfo = ASPUtils.getActiveUser();
            DateTime WorkingDate = new DateTime();
            //WorkingDate = ASPUtils.SODDate(usrInfo, usrInfo.strBranch);
            WorkingDate = DateTime.Today;
            try
            {
            for (Item = 0; Item < myColl.Count ; Item++)
            {
                Micrline= string.Empty;
                switch (FileType.ToString())
                {
                    case "EJ":
                        switch (myColl[Item].ToString().Substring(0, 2))
                        {
                            case "16":
                            case "18":
                            case "19":
                                break;
                            default:
                               
                                LineItemsTable.Add("RETURNCODE", myColl[Item].ToString().Substring(0, 2));
                                LineItemsTable.Add("VOUCHERTYPE", myColl[Item].ToString().Substring(2, 2));
                                LineItemsTable.Add("AMOUNT", (BRBaseConvert.ConvertToInt32(myColl[Item].ToString().Substring(4, 13)) * 0.01).ToString());
                                LineItemsTable.Add("ENTRYMODE", myColl[Item].ToString().Substring(17, 1));
                                switch (CurrencyID.Trim())
                                {
                                    case "FOREIGN":
                                        if (myColl[Item].ToString().Substring(0, 2) != "00")
                                        {
                                            LineItemsTable.Add("TOBANK", myColl[Item].ToString().Substring(57, 2));
                                            LineItemsTable.Add("TOBRANCH", myColl[Item].ToString().Substring(59, 3));
                                            BankID = myColl[Item].ToString().Substring(57, 2);
                                            BranchID = myColl[Item].ToString().Substring(59, 3);
                                        }
                                        else
                                        {
                                            LineItemsTable.Add("TOBANK", myColl[Item].ToString().Substring(18, 2));
                                            LineItemsTable.Add("TOBRANCH", myColl[Item].ToString().Substring(20, 3));
                                        }
                                        break;
                                    default:
                                        if (myColl[Item].ToString().Substring(0, 2) != "00")
                                        {
                                            LineItemsTable.Add("TOBANK", myColl[Item].ToString().Substring(58, 2));
                                            LineItemsTable.Add("TOBRANCH", myColl[Item].ToString().Substring(60, 3));
                                        }
                                        else
                                        {
                                            LineItemsTable.Add("TOBANK", myColl[Item].ToString().Substring(18, 2));
                                            LineItemsTable.Add("TOBRANCH", myColl[Item].ToString().Substring(20, 3));
                                        }
                                        break;
                                }
                                LineItemsTable.Add("TOACCOUNT", myColl[Item].ToString().Substring(23, 10));
                                if (ConfigurationManager.AppSettings["Rem"] == "1")
                                {
                                    LineItemsTable.Add("REMITTER", myColl[Item].ToString().Substring(59, 35));
                                }
                                LineItemsTable.Add("CHEQUEDIGIT", myColl[Item].ToString().Substring(33, 1));
                                switch (CurrencyID.Trim())
                                {
                                    case "FOREIGN":
                                        if (myColl[Item].ToString().Substring(0, 2) != "00")
                                        {
                                            LineItemsTable.Add("FROMBANK", myColl[Item].ToString().Substring(18, 2));
                                            LineItemsTable.Add("FROMBRANCH", myColl[Item].ToString().Substring(20, 3));

                                        }
                                        else
                                        {
                                            LineItemsTable.Add("FROMBANK", myColl[Item].ToString().Substring(58, 2));
                                            LineItemsTable.Add("FROMBRANCH", myColl[Item].ToString().Substring(60, 3));
                                            BankID = myColl[Item].ToString().Substring(58, 2);
                                            BranchID = myColl[Item].ToString().Substring(60, 3);

                                        }
                                        break;
                                    default:
                                        if (myColl[Item].ToString().Substring(0, 2) != "00")
                                        {
                                            LineItemsTable.Add("FROMBANK", myColl[Item].ToString().Substring(18, 2));
                                            LineItemsTable.Add("FROMBRANCH", myColl[Item].ToString().Substring(20, 3));
                                            BankID = myColl[Item].ToString().Substring(18, 2);
                                            BranchID = myColl[Item].ToString().Substring(20, 3);
                                        }
                                        else
                                        {
                                            LineItemsTable.Add("FROMBANK", myColl[Item].ToString().Substring(58, 2));
                                            LineItemsTable.Add("FROMBRANCH", myColl[Item].ToString().Substring(60, 3));
                                        }
                                        break;
                                }
                                switch (CurrencyID.Trim())
                                {
                                    case "FOREIGN":
                                        LineItemsTable.Add("FILLER", myColl[Item].ToString().Substring(34, 4));
                                        LineItemsTable.Add("COLLECTIONACCOUNT", myColl[Item].ToString().Substring(38, 20));
                                        LineItemsTable.Add("SERIALNUMBER", myColl[Item].ToString().Substring(63, 6));
                                        LineItemsTable.Add("PROCESSINGNO", myColl[Item].ToString().Substring(69, 9));
                                        break;
                                    default:
                                        LineItemsTable.Add("FILLER", myColl[Item].ToString().Substring(34, 4));
                                        LineItemsTable.Add("COLLECTIONACCOUNT", myColl[Item].ToString().Substring(38, 20));
                                        LineItemsTable.Add("SERIALNUMBER", myColl[Item].ToString().Substring(63, 6));
                                        LineItemsTable.Add("PROCESSINGNO", myColl[Item].ToString().Substring(69, 9));
                                        break;
                                }


                                LineItemsTable.Add("FILENAME", filePath.Substring(filePath.LastIndexOf("\\") + 1).Replace("_Temp", ""));
                                LineItemsTable.Add("DATA", myColl[Item].ToString());
                                Micrline = (myColl[Item].ToString().Substring(63, 6) + BankID + BranchID + myColl[Item].ToString().Substring(33, 1) + myColl[Item].ToString().Substring(2, 2) + myColl[Item].ToString().Substring(38, 20));
                                LineItemsTable.Add("MICRLINE", Micrline);
                                LineItemsTable.Add("DATE", WorkingDate);
                                LineItemsTable.Add("TRXTYPE", "ID");
                                break;
                        }

                        break;

                    case "FCYEJ":
                        switch (myColl[Item].ToString().Substring(0, 2))
                        {
                            case "16":
                            case "18":
                            case "19":
                                break;
                            default:

                                LineItemsTable.Add("RETURNCODE", myColl[Item].ToString().Substring(0, 2));
                                LineItemsTable.Add("VOUCHERTYPE", myColl[Item].ToString().Substring(2, 2));
                                LineItemsTable.Add("AMOUNT", (BRBaseConvert.ConvertToInt32(myColl[Item].ToString().Substring(4, 13)) * 0.01).ToString());
                                LineItemsTable.Add("ENTRYMODE", myColl[Item].ToString().Substring(17, 1));
                                switch (CurrencyID.Trim())
                                {
                                    case "FOREIGN":
                                        if (myColl[Item].ToString().Substring(0, 2) != "00")
                                        {
                                            LineItemsTable.Add("TOBANK", myColl[Item].ToString().Substring(57, 2));
                                            LineItemsTable.Add("TOBRANCH", myColl[Item].ToString().Substring(59, 3));
                                            BankID = myColl[Item].ToString().Substring(57, 2);
                                            BranchID = myColl[Item].ToString().Substring(59, 3);
                                        }
                                        else
                                        {
                                            LineItemsTable.Add("TOBANK", myColl[Item].ToString().Substring(18, 2));
                                            LineItemsTable.Add("TOBRANCH", myColl[Item].ToString().Substring(20, 3));
                                        }
                                        break;
                                    default:
                                        if (myColl[Item].ToString().Substring(0, 2) != "00")
                                        {
                                            LineItemsTable.Add("TOBANK", myColl[Item].ToString().Substring(58, 2));
                                            LineItemsTable.Add("TOBRANCH", myColl[Item].ToString().Substring(60, 3));
                                        }
                                        else
                                        {
                                            LineItemsTable.Add("TOBANK", myColl[Item].ToString().Substring(18, 2));
                                            LineItemsTable.Add("TOBRANCH", myColl[Item].ToString().Substring(20, 3));
                                        }
                                        break;
                                }
                                LineItemsTable.Add("TOACCOUNT", myColl[Item].ToString().Substring(23, 10));
                                LineItemsTable.Add("CHEQUEDIGIT", myColl[Item].ToString().Substring(33, 1));
                                switch (CurrencyID.Trim())
                                {
                                    case "FOREIGN":
                                        if (myColl[Item].ToString().Substring(0, 2) != "00")
                                        {
                                            LineItemsTable.Add("FROMBANK", myColl[Item].ToString().Substring(18, 2));
                                            LineItemsTable.Add("FROMBRANCH", myColl[Item].ToString().Substring(20, 3));

                                        }
                                        else
                                        {
                                            LineItemsTable.Add("FROMBANK", myColl[Item].ToString().Substring(58, 2));
                                            LineItemsTable.Add("FROMBRANCH", myColl[Item].ToString().Substring(60, 3));
                                            BankID = myColl[Item].ToString().Substring(58, 2);
                                            BranchID = myColl[Item].ToString().Substring(60, 3);

                                        }
                                        break;
                                    default:
                                        if (myColl[Item].ToString().Substring(0, 2) != "00")
                                        {
                                            LineItemsTable.Add("FROMBANK", myColl[Item].ToString().Substring(18, 2));
                                            LineItemsTable.Add("FROMBRANCH", myColl[Item].ToString().Substring(20, 3));
                                            BankID = myColl[Item].ToString().Substring(18, 2);
                                            BranchID = myColl[Item].ToString().Substring(20, 3);
                                        }
                                        else
                                        {
                                            LineItemsTable.Add("FROMBANK", myColl[Item].ToString().Substring(58, 2));
                                            LineItemsTable.Add("FROMBRANCH", myColl[Item].ToString().Substring(60, 3));
                                        }
                                        break;
                                }
                                switch (CurrencyID.Trim())
                                {
                                    case "FOREIGN":
                                        LineItemsTable.Add("FILLER", myColl[Item].ToString().Substring(34, 4));
                                        LineItemsTable.Add("COLLECTIONACCOUNT", myColl[Item].ToString().Substring(38, 20));
                                        LineItemsTable.Add("SERIALNUMBER", myColl[Item].ToString().Substring(63, 6));
                                        LineItemsTable.Add("PROCESSINGNO", myColl[Item].ToString().Substring(69, 9));
                                        break;
                                    default:
                                        LineItemsTable.Add("FILLER", myColl[Item].ToString().Substring(34, 4));
                                        LineItemsTable.Add("COLLECTIONACCOUNT", myColl[Item].ToString().Substring(38, 20));
                                        LineItemsTable.Add("SERIALNUMBER", myColl[Item].ToString().Substring(63, 6));
                                        LineItemsTable.Add("PROCESSINGNO", myColl[Item].ToString().Substring(69, 9));
                                        break;
                                }
                                LineItemsTable.Add("FILENAME", filePath.Substring(filePath.LastIndexOf("\\") + 1).Replace("_Temp", ""));
                                LineItemsTable.Add("DATA", myColl[Item].ToString());
                                Micrline = (myColl[Item].ToString().Substring(63, 6) + BankID + BranchID + myColl[Item].ToString().Substring(33, 1) + myColl[Item].ToString().Substring(2, 2) + myColl[Item].ToString().Substring(38, 20));
                                LineItemsTable.Add("MICRLINE", Micrline);
                                LineItemsTable.Add("DATE", WorkingDate);
                                LineItemsTable.Add("TRXTYPE", "ID");

                                break;
                        }

                        break;
                        case "DD":
                            switch (myColl[Item].ToString().Substring(0, 2))
                            {
                                case "16":
                                case "18":
                                case "19":
                                    break;
                                default:
                                    LineItemsTable.Add("RETURNTYPE", myColl[Item].ToString().Substring(0, 2));
                                    LineItemsTable.Add("RETURNCODE", myColl[Item].ToString().Substring(0, 2));
                                    LineItemsTable.Add("VOUCHERTYPE", myColl[Item].ToString().Substring(2, 2));
                                    LineItemsTable.Add("FAMOUNT", (BRBaseConvert.ConvertToInt32(myColl[Item].ToString().Substring(6, 13)) * 0.01).ToString());
                                    LineItemsTable.Add("CAMOUNT", (BRBaseConvert.ConvertToInt32(myColl[Item].ToString().Substring(6, 13)) * 0.01).ToString());
                                    LineItemsTable.Add("TOBANK", myColl[Item].ToString().Substring(20, 2));
                                    LineItemsTable.Add("TOBRANCH", myColl[Item].ToString().Substring(22, 3));
                                    LineItemsTable.Add("TOACCOUNT", myColl[Item].ToString().Substring(30, 10));
                                    LineItemsTable.Add("FROMBANK", myColl[Item].ToString().Substring(40, 2));
                                    LineItemsTable.Add("FROMBRANCH", myColl[Item].ToString().Substring(42, 3));
                                    LineItemsTable.Add("COLLECTIONACCOUNT", myColl[Item].ToString().Substring(50, 10));
                                    LineItemsTable.Add("ORIGINATINGCODE", myColl[Item].ToString().Substring(65, 4));
                                    LineItemsTable.Add("ORIGINATINGREF", myColl[Item].ToString().Substring(69, 15));
                                    LineItemsTable.Add("POLICY1", myColl[Item].ToString().Substring(84, 20));
                                    LineItemsTable.Add("POLICY2", myColl[Item].ToString().Substring(104, 20));

                                    LineItemsTable.Add("DUEDATE", myColl[Item].ToString().Substring(124, 25));
                                    LineItemsTable.Add("FREQUENCY", myColl[Item].ToString().Substring(124, 25));
                                    LineItemsTable.Add("EXIPIRYDATE", myColl[Item].ToString().Substring(124, 25));
                                    LineItemsTable.Add("PAYERSNAME", myColl[Item].ToString().Substring(124, 25));
                                    

                                    LineItemsTable.Add("TRXTYPE", "ID");
                                    LineItemsTable.Add("FILENAME", filePath.Substring(filePath.LastIndexOf("\\") + 1).Replace("_Temp", ""));
                                break;
                            }
                            break;
                        case "EFT":
                        switch (myColl[Item].ToString().Substring(0, 2))
                        {
                            case "16":
                            case "18":
                            case "19":
                                break;
                            default:
                                if (myColl[Item].ToString().Substring(0, 2)=="90")
                                {
                                    string Filename = string.Empty;
                                    Filename = filePath.Substring(filePath.LastIndexOf("\\") + 1).Replace("_Temp", "");
                                    //Commissions
                                    LineItemsTable.Add("RETURNCODE", myColl[Item].ToString().Substring(0, 2));
                                    LineItemsTable.Add("VOUCHERTYPE", myColl[Item].ToString().Substring(2, 2));
                                    LineItemsTable.Add("AMOUNT", (BRBaseConvert.ConvertToInt32(myColl[Item].ToString().Substring(6, 13)) * 0.01).ToString());
                                    LineItemsTable.Add("TOBANK", myColl[Item].ToString().Substring(20, 2));
                                    LineItemsTable.Add("TOBRANCH", myColl[Item].ToString().Substring(22, 3));
                                    LineItemsTable.Add("TOACCOUNT", myColl[Item].ToString().Substring(30, 10));
                                    LineItemsTable.Add("FROMBANK", Filename.Substring(0,2));
                                    LineItemsTable.Add("FROMBRANCH", "000");
                                    LineItemsTable.Add("COLLECTIONACCOUNT", "0000000000");
                                    LineItemsTable.Add("COLLECTIONACCOUNTNAME", "");
                                    LineItemsTable.Add("DESCRIPTION", myColl[Item].ToString().Substring(65, 15));
                                    LineItemsTable.Add("DRAWERORPAYEE", myColl[Item].ToString().Substring(30, 10));
                                    LineItemsTable.Add("SERIALNUMBER", myColl[Item].ToString().Substring(40, 6));
                                    LineItemsTable.Add("TRXTYPE", "IC");
                                    LineItemsTable.Add("FILENAME", filePath.Substring(filePath.LastIndexOf("\\") + 1).Replace("_Temp", ""));
                                }
                                else if (myColl[Item].ToString().Substring(2, 2) == "40" )
                                {
                                    //DD
                                    if (ConfigurationManager.AppSettings["Country"].ToString().Trim().ToUpper() == "UG")
                                    {
                                        LineItemsTable.Add("RETURNCODE", myColl[Item].ToString().Substring(0, 2));
                                        LineItemsTable.Add("VOUCHERTYPE", myColl[Item].ToString().Substring(2, 2));
                                        LineItemsTable.Add("AMOUNT", (BRBaseConvert.ConvertToDouble(myColl[Item].ToString().Substring(4, 14)) * 0.01).ToString());
                                      


                                        LineItemsTable.Add("FROMBANK", myColl[Item].ToString().Substring(39, 2));
                                        LineItemsTable.Add("FROMBRANCH", myColl[Item].ToString().Substring(41, 2));
                                        LineItemsTable.Add("TOBANK", myColl[Item].ToString().Substring(18, 2));
                                        LineItemsTable.Add("TOBRANCH", myColl[Item].ToString().Substring(20, 2));
                                       
                                        LineItemsTable.Add("DESCRIPTION", myColl[Item].ToString().Substring(114, 30));
                                        LineItemsTable.Add("DRAWERORPAYEE", myColl[Item].ToString().Substring(79, 30));

                                        LineItemsTable.Add("COLLECTIONACCOUNT", myColl[Item].ToString().Substring(45, 15));
                                        LineItemsTable.Add("TOACCOUNT", myColl[Item].ToString().Substring(24, 15));

                                        LineItemsTable.Add("ORIGINATINGCODE", "");
                                        LineItemsTable.Add("ORIGINATINGREF","");
                                        LineItemsTable.Add("POLICY1", "");
                                        LineItemsTable.Add("POLICY2", "");
                                        LineItemsTable.Add("REMARKS", "");

                                        LineItemsTable.Add("TRXTYPE", "ID");
                                        LineItemsTable.Add("FILENAME", filePath.Substring(filePath.LastIndexOf("\\") + 1).Replace("_Temp", ""));
                                        //LineItemsTable.Add("TRXTYPE", "ID");
                                    }
                                    else
                                    {
                                        LineItemsTable.Add("RETURNCODE", myColl[Item].ToString().Substring(0, 2));
                                        LineItemsTable.Add("VOUCHERTYPE", myColl[Item].ToString().Substring(2, 2));
                                        LineItemsTable.Add("AMOUNT", (BRBaseConvert.ConvertToInt32(myColl[Item].ToString().Substring(6, 13)) * 0.01).ToString());
                                        LineItemsTable.Add("TOBANK", myColl[Item].ToString().Substring(20, 2));
                                        LineItemsTable.Add("TOBRANCH", myColl[Item].ToString().Substring(22, 3));
                                        LineItemsTable.Add("TOACCOUNT", myColl[Item].ToString().Substring(30, 10));
                                        LineItemsTable.Add("FROMBANK", myColl[Item].ToString().Substring(40, 2));
                                        LineItemsTable.Add("FROMBRANCH", myColl[Item].ToString().Substring(42, 3));
                                        LineItemsTable.Add("COLLECTIONACCOUNT", myColl[Item].ToString().Substring(50, 10));
                                        LineItemsTable.Add("ORIGINATINGCODE", myColl[Item].ToString().Substring(65, 4));
                                        LineItemsTable.Add("ORIGINATINGREF", myColl[Item].ToString().Substring(69, 15));
                                        LineItemsTable.Add("POLICY1", myColl[Item].ToString().Substring(84, 20));
                                        LineItemsTable.Add("POLICY2", myColl[Item].ToString().Substring(104, 20));
                                        LineItemsTable.Add("REMARKS", myColl[Item].ToString().Substring(124, 25));
                                        LineItemsTable.Add("REMITTER", myColl[Item].ToString().Substring(149, 35));
                                        LineItemsTable.Add("TRXTYPE", "ID");
                                        LineItemsTable.Add("FILENAME", filePath.Substring(filePath.LastIndexOf("\\") + 1).Replace("_Temp", ""));
                                    }
                                }
                                else
                                {
                                    if (myColl[Item].ToString().Substring(2, 2).ToString() == "53")
                                    {
                                        if (ConfigurationManager.AppSettings["Country"].ToString().Trim().ToUpper() == "UG")
                                        {
                                            //ATM Debits
                                            LineItemsTable.Add("RETURNCODE", myColl[Item].ToString().Substring(0, 2));
                                            LineItemsTable.Add("VOUCHERTYPE", myColl[Item].ToString().Substring(2, 2));
                                            LineItemsTable.Add("AMOUNT", (BRBaseConvert.ConvertToInt32(myColl[Item].ToString().Substring(6, 13)) * 0.01).ToString());
                                            LineItemsTable.Add("TOBANK", myColl[Item].ToString().Substring(20, 2));
                                            LineItemsTable.Add("TOBRANCH", "000");
                                            LineItemsTable.Add("TOACCOUNT", "0000000000");
                                            LineItemsTable.Add("FROMBANK", myColl[Item].ToString().Substring(40, 2));
                                            LineItemsTable.Add("FROMBRANCH", "000");
                                            LineItemsTable.Add("COLLECTIONACCOUNT", "0000000000");
                                            LineItemsTable.Add("DESCRIPTION", myColl[Item].ToString().Substring(65, 15));
                                            LineItemsTable.Add("DRAWERORPAYEE", "ATM DEBITS");
                                            LineItemsTable.Add("TRXTYPE", "ID");
                                            LineItemsTable.Add("FILENAME", filePath.Substring(filePath.LastIndexOf("\\") + 1).Replace("_Temp", ""));
                                        }
                                        else
                                        {
                                            //ATM Debits
                                            LineItemsTable.Add("RETURNCODE", myColl[Item].ToString().Substring(0, 2));
                                            LineItemsTable.Add("VOUCHERTYPE", myColl[Item].ToString().Substring(2, 2));
                                            LineItemsTable.Add("AMOUNT", (BRBaseConvert.ConvertToInt32(myColl[Item].ToString().Substring(6, 13)) * 0.01).ToString());
                                            LineItemsTable.Add("TOBANK", myColl[Item].ToString().Substring(20, 2));
                                            LineItemsTable.Add("TOBRANCH", "000");
                                            LineItemsTable.Add("TOACCOUNT", "0000000000");
                                            LineItemsTable.Add("FROMBANK", myColl[Item].ToString().Substring(40, 2));
                                            LineItemsTable.Add("FROMBRANCH", "000");
                                            LineItemsTable.Add("REMITTER", "");
                                            LineItemsTable.Add("COLLECTIONACCOUNT", "0000000000");
                                            LineItemsTable.Add("DESCRIPTION", myColl[Item].ToString().Substring(65, 15));
                                            LineItemsTable.Add("DRAWERORPAYEE", "ATM DEBITS");
                                            LineItemsTable.Add("TRXTYPE", "ID");
                                            LineItemsTable.Add("FILENAME", filePath.Substring(filePath.LastIndexOf("\\") + 1).Replace("_Temp", ""));
                                        }
                                    }
                                    else if (myColl[Item].ToString().Substring(2, 2).ToString() == "54")
                                    {
                                        if (ConfigurationManager.AppSettings["Country"].ToString().Trim().ToUpper() == "UG")
                                        {
                                            //ATM Credits
                                            LineItemsTable.Add("RETURNCODE", myColl[Item].ToString().Substring(0, 2));
                                            LineItemsTable.Add("VOUCHERTYPE", myColl[Item].ToString().Substring(2, 2));
                                            LineItemsTable.Add("AMOUNT", (BRBaseConvert.ConvertToInt32(myColl[Item].ToString().Substring(6, 13)) * 0.01).ToString());
                                            LineItemsTable.Add("TOBANK", myColl[Item].ToString().Substring(20, 2));
                                            LineItemsTable.Add("TOBRANCH", "000");
                                            LineItemsTable.Add("TOACCOUNT", "0000000000");
                                            LineItemsTable.Add("FROMBANK", myColl[Item].ToString().Substring(40, 2));
                                            LineItemsTable.Add("FROMBRANCH", "000");
                                            LineItemsTable.Add("REMITTER", "");
                                            LineItemsTable.Add("COLLECTIONACCOUNT", "0000000000");
                                            LineItemsTable.Add("DESCRIPTION", myColl[Item].ToString().Substring(65, 15));
                                            LineItemsTable.Add("DRAWERORPAYEE", "ATM CREDITS");
                                            LineItemsTable.Add("TRXTYPE", "IC");
                                            LineItemsTable.Add("FILENAME", filePath.Substring(filePath.LastIndexOf("\\") + 1).Replace("_Temp", ""));
                                        }
                                        else
                                        {
                                            //ATM Credits
                                            LineItemsTable.Add("RETURNCODE", myColl[Item].ToString().Substring(0, 2));
                                            LineItemsTable.Add("VOUCHERTYPE", myColl[Item].ToString().Substring(2, 2));
                                            LineItemsTable.Add("AMOUNT", (BRBaseConvert.ConvertToInt32(myColl[Item].ToString().Substring(6, 13)) * 0.01).ToString());
                                            LineItemsTable.Add("TOBANK", myColl[Item].ToString().Substring(40, 2));
                                            LineItemsTable.Add("TOBRANCH", myColl[Item].ToString().Substring(42, 3));
                                            LineItemsTable.Add("TOACCOUNT", myColl[Item].ToString().Substring(45, 15));
                                            LineItemsTable.Add("FROMBANK", myColl[Item].ToString().Substring(20, 2));
                                            LineItemsTable.Add("FROMBRANCH", myColl[Item].ToString().Substring(22, 3));
                                            LineItemsTable.Add("COLLECTIONACCOUNT", myColl[Item].ToString().Substring(25, 15));
                                            LineItemsTable.Add("REMITTER","");
                                            LineItemsTable.Add("DESCRIPTION", myColl[Item].ToString().Substring(68, 35));
                                            LineItemsTable.Add("DRAWERORPAYEE", "ATM CREDITS");
                                            LineItemsTable.Add("TRXTYPE", "IC");
                                            LineItemsTable.Add("FILENAME", filePath.Substring(filePath.LastIndexOf("\\") + 1).Replace("_Temp", ""));
                                        }
                                    }
                                    else
                                    {
                                        if (ConfigurationManager.AppSettings["Country"].ToString().Trim().ToUpper() == "UG")
                                        {
                                            //All other EFTs
                                            LineItemsTable.Add("RETURNCODE", myColl[Item].ToString().Substring(0, 2));
                                            LineItemsTable.Add("VOUCHERTYPE", myColl[Item].ToString().Substring(2, 2));
                                            LineItemsTable.Add("AMOUNT", (BRBaseConvert.ConvertToDouble(myColl[Item].ToString().Substring(4, 14)) * 0.01).ToString());
                                            LineItemsTable.Add("TOBANK", myColl[Item].ToString().Substring(39, 2));
                                            LineItemsTable.Add("TOBRANCH", myColl[Item].ToString().Substring(41, 4));
                                            LineItemsTable.Add("TOACCOUNT", myColl[Item].ToString().Substring(45, 15));
                                            LineItemsTable.Add("FROMBANK", myColl[Item].ToString().Substring(18, 2));
                                            LineItemsTable.Add("FROMBRANCH", myColl[Item].ToString().Substring(20, 4));
                                            LineItemsTable.Add("COLLECTIONACCOUNT", myColl[Item].ToString().Substring(24, 15));
                                            LineItemsTable.Add("DESCRIPTION", myColl[Item].ToString().Substring(114, 35));
                                            LineItemsTable.Add("DRAWERORPAYEE", myColl[Item].ToString().Substring(79, 35));
                                            LineItemsTable.Add("TRXTYPE", "IC");
                                            LineItemsTable.Add("FILENAME", filePath.Substring(filePath.LastIndexOf("\\") + 1).Replace("_Temp", ""));
                                        }
                                        else
                                        {
                                            //All other EFTs
                                            LineItemsTable.Add("RETURNCODE", myColl[Item].ToString().Substring(0, 2));
                                            LineItemsTable.Add("VOUCHERTYPE", myColl[Item].ToString().Substring(2, 2));
                                            LineItemsTable.Add("AMOUNT", (BRBaseConvert.ConvertToInt32(myColl[Item].ToString().Substring(4, 13)) * 0.01).ToString());
                                            LineItemsTable.Add("TOBANK", myColl[Item].ToString().Substring(40, 2));
                                            LineItemsTable.Add("TOBRANCH", myColl[Item].ToString().Substring(42, 3));
                                            LineItemsTable.Add("TOACCOUNT", myColl[Item].ToString().Substring(50, 10));
                                            LineItemsTable.Add("FROMBANK", myColl[Item].ToString().Substring(20, 2));
                                            LineItemsTable.Add("FROMBRANCH", myColl[Item].ToString().Substring(22, 3));
                                            LineItemsTable.Add("COLLECTIONACCOUNT", myColl[Item].ToString().Substring(30, 10));
                                            LineItemsTable.Add("DESCRIPTION", myColl[Item].ToString().Substring(115, 35));
                                            LineItemsTable.Add("REMITTER", myColl[Item].ToString().Substring(150, 35));
                                            LineItemsTable.Add("DRAWERORPAYEE", myColl[Item].ToString().Substring(80, 30));
                                            LineItemsTable.Add("TRXTYPE", "IC");
                                            LineItemsTable.Add("FILENAME", filePath.Substring(filePath.LastIndexOf("\\") + 1).Replace("_Temp", ""));
                                        }
                                    }
                                }
                                LineItemsTable.Add("DATA", myColl[Item].ToString());
                                LineItemsTable.Add("DATE",WorkingDate);
                               
                                break;
                        }

                        break;
                    case "DISC":
                        switch (myColl[Item].ToString().Substring(0, 2))
                        {
                            case "16":
                            case "18":
                            case "19":
                                break;
                            default:

                                LineItemsTable.Add("RETURNCODE", myColl[Item].ToString().Substring(0, 2));
                                LineItemsTable.Add("VOUCHERTYPE", myColl[Item].ToString().Substring(2, 2));
                                LineItemsTable.Add("AMOUNT", (BRBaseConvert.ConvertToInt32(myColl[Item].ToString().Substring(4, 13)) * 0.01).ToString());
                                LineItemsTable.Add("ENTRYMODE", myColl[Item].ToString().Substring(17, 1));
                                switch (CurrencyID.Trim())
                                {
                                    case "FOREIGN":
                                        if (myColl[Item].ToString().Substring(0, 2) != "00")
                                        {
                                            LineItemsTable.Add("TOBANK", myColl[Item].ToString().Substring(57, 2));
                                            LineItemsTable.Add("TOBRANCH", myColl[Item].ToString().Substring(59, 3));
                                            BankID = myColl[Item].ToString().Substring(57, 2);
                                            BranchID = myColl[Item].ToString().Substring(59, 3);
                                        }
                                        else
                                        {
                                            LineItemsTable.Add("TOBANK", myColl[Item].ToString().Substring(18, 2));
                                            LineItemsTable.Add("TOBRANCH", myColl[Item].ToString().Substring(20, 3));
                                        }
                                        break;
                                    default:
                                        if (myColl[Item].ToString().Substring(0, 2) != "00")
                                        {
                                            LineItemsTable.Add("TOBANK", myColl[Item].ToString().Substring(58, 2));
                                            LineItemsTable.Add("TOBRANCH", myColl[Item].ToString().Substring(60, 3));
                                        }
                                        else
                                        {
                                            LineItemsTable.Add("TOBANK", myColl[Item].ToString().Substring(18, 2));
                                            LineItemsTable.Add("TOBRANCH", myColl[Item].ToString().Substring(20, 3));
                                        }
                                        break;
                                }
                                LineItemsTable.Add("TOACCOUNT", myColl[Item].ToString().Substring(23, 10));
                                LineItemsTable.Add("CHEQUEDIGIT", myColl[Item].ToString().Substring(33, 1));
                                switch (CurrencyID.Trim())
                                {
                                    case "FOREIGN":
                                        if (myColl[Item].ToString().Substring(0, 2) != "00")
                                        {
                                            LineItemsTable.Add("FROMBANK", myColl[Item].ToString().Substring(18, 2));
                                            LineItemsTable.Add("FROMBRANCH", myColl[Item].ToString().Substring(20, 3));

                                        }
                                        else
                                        {
                                            LineItemsTable.Add("FROMBANK", myColl[Item].ToString().Substring(58, 2));
                                            LineItemsTable.Add("FROMBRANCH", myColl[Item].ToString().Substring(60, 3));
                                            BankID = myColl[Item].ToString().Substring(58, 2);
                                            BranchID = myColl[Item].ToString().Substring(60, 3);

                                        }
                                        break;
                                    default:
                                        if (myColl[Item].ToString().Substring(0, 2) != "00")
                                        {
                                            LineItemsTable.Add("FROMBANK", myColl[Item].ToString().Substring(18, 2));
                                            LineItemsTable.Add("FROMBRANCH", myColl[Item].ToString().Substring(20, 3));
                                            BankID = myColl[Item].ToString().Substring(18, 2);
                                            BranchID = myColl[Item].ToString().Substring(20, 3);
                                        }
                                        else
                                        {
                                            LineItemsTable.Add("FROMBANK", myColl[Item].ToString().Substring(58, 2));
                                            LineItemsTable.Add("FROMBRANCH", myColl[Item].ToString().Substring(60, 3));
                                        }
                                        break;
                                }
                                switch (CurrencyID.Trim())
                                {
                                    case "FOREIGN":
                                        LineItemsTable.Add("FILLER", myColl[Item].ToString().Substring(34, 4));
                                        LineItemsTable.Add("COLLECTIONACCOUNT", myColl[Item].ToString().Substring(38, 20));
                                        LineItemsTable.Add("SERIALNUMBER", myColl[Item].ToString().Substring(63, 6));
                                        LineItemsTable.Add("PROCESSINGNO", myColl[Item].ToString().Substring(69, 9));
                                        break;
                                    default:
                                        LineItemsTable.Add("FILLER", myColl[Item].ToString().Substring(34, 4));
                                        LineItemsTable.Add("COLLECTIONACCOUNT", myColl[Item].ToString().Substring(38, 20));
                                        LineItemsTable.Add("SERIALNUMBER", myColl[Item].ToString().Substring(63, 6));
                                        LineItemsTable.Add("PROCESSINGNO", myColl[Item].ToString().Substring(69, 9));
                                        break;
                                }
                                LineItemsTable.Add("FILENAME", filePath.Substring(filePath.LastIndexOf("\\") + 1).Replace("_Temp", ""));
                                LineItemsTable.Add("DATA", myColl[Item].ToString());
                                Micrline = (myColl[Item].ToString().Substring(63, 6) + BankID + BranchID + myColl[Item].ToString().Substring(33, 1) + myColl[Item].ToString().Substring(2, 2) + myColl[Item].ToString().Substring(38, 20));
                                LineItemsTable.Add("MICRLINE", Micrline);
                                LineItemsTable.Add("DATE", WorkingDate);
                                LineItemsTable.Add("TRXTYPE", "ID");
                                break;
                        }

                        break;
                    case "SELT":
                        switch (myColl[Item].ToString().Substring(0, 2))
                        {
                            case "18":
                            case "19":
                                break;
                            default:
                                if (ConfigurationManager.AppSettings["Country"].ToString().Trim().ToUpper() == "UG")
                                {
                                    //LOCAL FILE FORMAT
                                    LineItemsTable.Add("RETURNCODE", myColl[Item].ToString().Substring(0, 2));
                                    LineItemsTable.Add("BANKNO", myColl[Item].ToString().Substring(2, 2));
                                    if (myColl[Item].ToString().Substring(0, 2) == "08")
                                    {
                                        LineItemsTable.Add("CREDITCOUNT", myColl[Item].ToString().Substring(4, 6));
                                        LineItemsTable.Add("CREDITAMOUNT", (BRBaseConvert.ConvertToDouble(myColl[Item].ToString().Substring(10, 14)) * 0.01).ToString());
                                        LineItemsTable.Add("DEBITCOUNT", 0);
                                        LineItemsTable.Add("DEBITAMOUNT", 0);
                                        LineItemsTable.Add("CRUNPAIDCOUNT", myColl[Item].ToString().Substring(24, 6));
                                        LineItemsTable.Add("CRUNPAIDAMOUNT", (BRBaseConvert.ConvertToInt32(myColl[Item].ToString().Substring(30, 15)) * 0.01).ToString());
                                        LineItemsTable.Add("DRUNPAIDCOUNT", 0);
                                        LineItemsTable.Add("DRUNPAIDAMOUNT", 0);
                                        LineItemsTable.Add("UNPAIDCOUNT", 0);
                                        LineItemsTable.Add("UNPAIDVALUE", 0);
                                        LineItemsTable.Add("MDVCOUNT", 0);
                                        LineItemsTable.Add("MDVAMOUNT", 0);
                                        LineItemsTable.Add("DISCREPANCYCOUNT", 0);
                                        LineItemsTable.Add("DISCREPANCYAMOUNT", 0);
                                    }
                                    else if (myColl[Item].ToString().Substring(0, 2) == "07")
                                    {
                                        LineItemsTable.Add("CREDITCOUNT", 0);
                                        LineItemsTable.Add("CREDITAMOUNT", 0);
                                        LineItemsTable.Add("DEBITCOUNT", myColl[Item].ToString().Substring(4, 6));
                                        LineItemsTable.Add("DEBITAMOUNT", (BRBaseConvert.ConvertToDouble(myColl[Item].ToString().Substring(10, 15)) * 0.01).ToString());
                                        LineItemsTable.Add("DRUNPAIDCOUNT", myColl[Item].ToString().Substring(24, 6));
                                        LineItemsTable.Add("DRUNPAIDAMOUNT", (BRBaseConvert.ConvertToDouble(myColl[Item].ToString().Substring(30, 15)) * 0.01).ToString());
                                        LineItemsTable.Add("CRUNPAIDCOUNT", 0);
                                        LineItemsTable.Add("CRUNPAIDAMOUNT", 0);
                                        LineItemsTable.Add("UNPAIDCOUNT", 0);
                                        LineItemsTable.Add("UNPAIDVALUE", 0);
                                        LineItemsTable.Add("MDVCOUNT", 0);
                                        LineItemsTable.Add("MDVAMOUNT", 0);
                                        LineItemsTable.Add("DISCREPANCYCOUNT", 0);
                                        LineItemsTable.Add("DISCREPANCYAMOUNT", 0);
                                    }
                                    else if (myColl[Item].ToString().Substring(0, 2) == "09")
                                    {
                                        LineItemsTable.Add("CREDITCOUNT", 0);
                                        LineItemsTable.Add("CREDITAMOUNT", 0);
                                        LineItemsTable.Add("DEBITCOUNT", 0);
                                        LineItemsTable.Add("DEBITAMOUNT", 0);
                                        LineItemsTable.Add("UNPAIDCOUNT", 0);
                                        LineItemsTable.Add("UNPAIDVALUE", 0);
                                        LineItemsTable.Add("CRUNPAIDCOUNT", 0);
                                        LineItemsTable.Add("CRUNPAIDAMOUNT", 0);
                                        LineItemsTable.Add("DRUNPAIDCOUNT", 0);
                                        LineItemsTable.Add("DRUNPAIDAMOUNT", 0);
                                        LineItemsTable.Add("MDVCOUNT", myColl[Item].ToString().Substring(4, 6));
                                        LineItemsTable.Add("MDVAMOUNT", (BRBaseConvert.ConvertToDouble(myColl[Item].ToString().Substring(10, 15)) * 0.01).ToString());
                                        LineItemsTable.Add("DISCREPANCYCOUNT", 0);
                                        LineItemsTable.Add("DISCREPANCYAMOUNT", 0);
                                    }
                                    else
                                    {
                                        LineItemsTable.Add("CREDITCOUNT", 0);
                                        LineItemsTable.Add("CREDITAMOUNT", 0);
                                        LineItemsTable.Add("DEBITCOUNT", myColl[Item].ToString().Substring(4, 6));
                                        LineItemsTable.Add("DEBITAMOUNT", (BRBaseConvert.ConvertToDouble(myColl[Item].ToString().Substring(10, 15)) * 0.01).ToString());
                                        LineItemsTable.Add("UNPAIDCOUNT", myColl[Item].ToString().Substring(46, 6));
                                        LineItemsTable.Add("UNPAIDVALUE", (BRBaseConvert.ConvertToDouble(myColl[Item].ToString().Substring(52, 15)) * 0.01).ToString());
                                        LineItemsTable.Add("CRUNPAIDCOUNT", 0);
                                        LineItemsTable.Add("CRUNPAIDAMOUNT", 0);
                                        LineItemsTable.Add("DRUNPAIDCOUNT", 0);
                                        LineItemsTable.Add("DRUNPAIDAMOUNT", 0);
                                        LineItemsTable.Add("MDVCOUNT", 0);
                                        LineItemsTable.Add("MDVAMOUNT", 0);
                                        LineItemsTable.Add("DISCREPANCYCOUNT", myColl[Item].ToString().Substring(25, 6));
                                        LineItemsTable.Add("DISCREPANCYAMOUNT", (BRBaseConvert.ConvertToDouble(myColl[Item].ToString().Substring(32, 15)) * 0.01).ToString());
                                    }
                                    LineItemsTable.Add("DATA", myColl[Item].ToString());
                                    LineItemsTable.Add("DATE", WorkingDate);
                                    LineItemsTable.Add("FILENAME", filePath.Substring(filePath.LastIndexOf("\\") + 1).Replace("_Temp", ""));
                                    LineItemsTable.Add("FILEDATE", FileDate);
                                }
                                else
                                {
                                    //LOCAL FILE FORMAT
                                    LineItemsTable.Add("RETURNCODE", myColl[Item].ToString().Substring(0, 2));
                                    LineItemsTable.Add("BANKNO", myColl[Item].ToString().Substring(2, 2));
                                    if (myColl[Item].ToString().Substring(0, 2) == "08")
                                    {
                                        LineItemsTable.Add("CREDITCOUNT", myColl[Item].ToString().Substring(6, 6));
                                        LineItemsTable.Add("CREDITAMOUNT", (BRBaseConvert.ConvertToDouble(myColl[Item].ToString().Substring(12, 14)) * 0.01).ToString());
                                        LineItemsTable.Add("DEBITCOUNT", 0);
                                        LineItemsTable.Add("DEBITAMOUNT", 0);
                                        LineItemsTable.Add("CRUNPAIDCOUNT", myColl[Item].ToString().Substring(26, 6));
                                        LineItemsTable.Add("CRUNPAIDAMOUNT", (BRBaseConvert.ConvertToInt32(myColl[Item].ToString().Substring(32, 14)) * 0.01).ToString());
                                        LineItemsTable.Add("DRUNPAIDCOUNT", 0);
                                        LineItemsTable.Add("DRUNPAIDAMOUNT", 0);
                                        LineItemsTable.Add("UNPAIDCOUNT", 0);
                                        LineItemsTable.Add("UNPAIDVALUE", 0);
                                        LineItemsTable.Add("MDVCOUNT", 0);
                                        LineItemsTable.Add("MDVAMOUNT", 0);
                                        LineItemsTable.Add("DISCREPANCYCOUNT", 0);
                                        LineItemsTable.Add("DISCREPANCYAMOUNT", 0);
                                    }
                                    else if (myColl[Item].ToString().Substring(0, 2) == "07")
                                    {
                                        LineItemsTable.Add("CREDITCOUNT", 0);
                                        LineItemsTable.Add("CREDITAMOUNT", 0);
                                        LineItemsTable.Add("DEBITCOUNT", myColl[Item].ToString().Substring(6, 6));
                                        LineItemsTable.Add("DEBITAMOUNT", (BRBaseConvert.ConvertToDouble(myColl[Item].ToString().Substring(12, 14)) * 0.01).ToString());
                                        LineItemsTable.Add("DRUNPAIDCOUNT", myColl[Item].ToString().Substring(26, 6));
                                        LineItemsTable.Add("DRUNPAIDAMOUNT", (BRBaseConvert.ConvertToDouble(myColl[Item].ToString().Substring(32, 14)) * 0.01).ToString());
                                        LineItemsTable.Add("CRUNPAIDCOUNT", 0);
                                        LineItemsTable.Add("CRUNPAIDAMOUNT", 0);
                                        LineItemsTable.Add("UNPAIDCOUNT", 0);
                                        LineItemsTable.Add("UNPAIDVALUE", 0);
                                        LineItemsTable.Add("MDVCOUNT", 0);
                                        LineItemsTable.Add("MDVAMOUNT", 0);
                                        LineItemsTable.Add("DISCREPANCYCOUNT", 0);
                                        LineItemsTable.Add("DISCREPANCYAMOUNT", 0);
                                    }
                                    else if (myColl[Item].ToString().Substring(0, 2) == "09")
                                    {
                                        LineItemsTable.Add("CREDITCOUNT", 0);
                                        LineItemsTable.Add("CREDITAMOUNT", 0);
                                        LineItemsTable.Add("DEBITCOUNT", 0);
                                        LineItemsTable.Add("DEBITAMOUNT", 0);
                                        LineItemsTable.Add("UNPAIDCOUNT", 0);
                                        LineItemsTable.Add("UNPAIDVALUE", 0);
                                        LineItemsTable.Add("CRUNPAIDCOUNT", 0);
                                        LineItemsTable.Add("CRUNPAIDAMOUNT", 0);
                                        LineItemsTable.Add("DRUNPAIDCOUNT", 0);
                                        LineItemsTable.Add("DRUNPAIDAMOUNT", 0);
                                        LineItemsTable.Add("MDVCOUNT", myColl[Item].ToString().Substring(6, 6));
                                        LineItemsTable.Add("MDVAMOUNT", (BRBaseConvert.ConvertToDouble(myColl[Item].ToString().Substring(12, 14)) * 0.01).ToString());
                                        LineItemsTable.Add("DISCREPANCYCOUNT", 0);
                                        LineItemsTable.Add("DISCREPANCYAMOUNT", 0);
                                    }
                                    else
                                    {
                                        LineItemsTable.Add("CREDITCOUNT", 0);
                                        LineItemsTable.Add("CREDITAMOUNT", 0);
                                        LineItemsTable.Add("DEBITCOUNT", myColl[Item].ToString().Substring(6, 6));
                                        LineItemsTable.Add("DEBITAMOUNT", (BRBaseConvert.ConvertToDouble(myColl[Item].ToString().Substring(12, 14)) * 0.01).ToString());
                                        LineItemsTable.Add("UNPAIDCOUNT", myColl[Item].ToString().Substring(46, 6));
                                        LineItemsTable.Add("UNPAIDVALUE", (BRBaseConvert.ConvertToDouble(myColl[Item].ToString().Substring(52, 14)) * 0.01).ToString());
                                        LineItemsTable.Add("CRUNPAIDCOUNT", 0);
                                        LineItemsTable.Add("CRUNPAIDAMOUNT", 0);
                                        LineItemsTable.Add("DRUNPAIDCOUNT", 0);
                                        LineItemsTable.Add("DRUNPAIDAMOUNT", 0);
                                        LineItemsTable.Add("MDVCOUNT", 0);
                                        LineItemsTable.Add("MDVAMOUNT", 0);
                                        LineItemsTable.Add("DISCREPANCYCOUNT", myColl[Item].ToString().Substring(26, 6));
                                        LineItemsTable.Add("DISCREPANCYAMOUNT", (BRBaseConvert.ConvertToDouble(myColl[Item].ToString().Substring(32, 14)) * 0.01).ToString());
                                    }
                                    LineItemsTable.Add("DATA", myColl[Item].ToString());
                                    LineItemsTable.Add("DATE", WorkingDate);
                                    LineItemsTable.Add("FILENAME", filePath.Substring(filePath.LastIndexOf("\\") + 1).Replace("_Temp", ""));
                                    LineItemsTable.Add("FILEDATE", FileDate);

                                }
                                break;
                        }
                        break;
                    case "FCYSELT":
                        switch (myColl[Item].ToString().Substring(0, 2))
                        {
                            case "18":
                            case "19":
                                break;
                            default:
                                //FOREIGN FILE FORMAT
                                LineItemsTable.Add("RETURNCODE", myColl[Item].ToString().Substring(0, 2));
                                LineItemsTable.Add("BANKNO", myColl[Item].ToString().Substring(2, 2));
                                if (myColl[Item].ToString().Substring(0, 2) == "08")
                                {

                                    LineItemsTable.Add("CREDITCOUNT", myColl[Item].ToString().Substring(6, 6));
                                    LineItemsTable.Add("CREDITAMOUNT", (BRBaseConvert.ConvertToDouble(myColl[Item].ToString().Substring(12, 14)) * 0.01).ToString());
                                    LineItemsTable.Add("DEBITCOUNT", 0);
                                    LineItemsTable.Add("DEBITAMOUNT", 0);
                                    LineItemsTable.Add("CRUNPAIDCOUNT", myColl[Item].ToString().Substring(26, 6));
                                    LineItemsTable.Add("CRUNPAIDAMOUNT", (BRBaseConvert.ConvertToDouble(myColl[Item].ToString().Substring(32, 14)) * 0.01).ToString());
                                    LineItemsTable.Add("DRUNPAIDCOUNT", 0);
                                    LineItemsTable.Add("DRUNPAIDAMOUNT", 0);
                                    LineItemsTable.Add("UNPAIDCOUNT", 0);
                                    LineItemsTable.Add("UNPAIDVALUE", 0);
                                    LineItemsTable.Add("MDVCOUNT", 0);
                                    LineItemsTable.Add("MDVAMOUNT", 0);
                                    LineItemsTable.Add("DISCREPANCYCOUNT", 0);
                                    LineItemsTable.Add("DISCREPANCYAMOUNT", 0);

                                }
                                else if (myColl[Item].ToString().Substring(0, 2) == "07")
                                {
                                    LineItemsTable.Add("CREDITCOUNT", 0);
                                    LineItemsTable.Add("CREDITAMOUNT", 0);
                                    LineItemsTable.Add("DEBITCOUNT", myColl[Item].ToString().Substring(6, 6));
                                    LineItemsTable.Add("DEBITAMOUNT", (BRBaseConvert.ConvertToDouble(myColl[Item].ToString().Substring(12, 14)) * 0.01).ToString());
                                    LineItemsTable.Add("DRUNPAIDCOUNT", myColl[Item].ToString().Substring(26, 6));
                                    LineItemsTable.Add("DRUNPAIDAMOUNT", (BRBaseConvert.ConvertToDouble(myColl[Item].ToString().Substring(32, 14)) * 0.01).ToString());
                                    LineItemsTable.Add("CRUNPAIDCOUNT", 0);
                                    LineItemsTable.Add("CRUNPAIDAMOUNT", 0);
                                    LineItemsTable.Add("UNPAIDCOUNT", 0);
                                    LineItemsTable.Add("UNPAIDVALUE", 0);
                                    LineItemsTable.Add("MDVCOUNT", 0);
                                    LineItemsTable.Add("MDVAMOUNT", 0);
                                    LineItemsTable.Add("DISCREPANCYCOUNT",0);
                                    LineItemsTable.Add("DISCREPANCYAMOUNT", 0);

                                }
                                else if (myColl[Item].ToString().Substring(0, 2) == "09")
                                {
                                    LineItemsTable.Add("CREDITCOUNT", 0);
                                    LineItemsTable.Add("CREDITAMOUNT", 0);
                                    LineItemsTable.Add("DEBITCOUNT", 0);
                                    LineItemsTable.Add("DEBITAMOUNT", 0);
                                    LineItemsTable.Add("UNPAIDCOUNT", 0);
                                    LineItemsTable.Add("UNPAIDVALUE", 0);
                                    LineItemsTable.Add("CRUNPAIDCOUNT", 0);
                                    LineItemsTable.Add("CRUNPAIDAMOUNT", 0);
                                    LineItemsTable.Add("DRUNPAIDCOUNT", 0);
                                    LineItemsTable.Add("DRUNPAIDAMOUNT", 0);
                                    LineItemsTable.Add("MDVCOUNT", myColl[Item].ToString().Substring(6, 6));
                                    LineItemsTable.Add("MDVAMOUNT", (BRBaseConvert.ConvertToDouble(myColl[Item].ToString().Substring(12, 14)) * 0.01).ToString());
                                    LineItemsTable.Add("DISCREPANCYCOUNT", 0);
                                    LineItemsTable.Add("DISCREPANCYAMOUNT", 0);

                                }
                                else
                                {
                                    LineItemsTable.Add("CREDITCOUNT", 0);
                                    LineItemsTable.Add("CREDITAMOUNT", 0);
                                    LineItemsTable.Add("DEBITCOUNT", myColl[Item].ToString().Substring(6, 6));
                                    LineItemsTable.Add("DEBITAMOUNT", (BRBaseConvert.ConvertToDouble(myColl[Item].ToString().Substring(12, 14)) * 0.01).ToString());
                                    LineItemsTable.Add("UNPAIDCOUNT", myColl[Item].ToString().Substring(46, 6));
                                    LineItemsTable.Add("UNPAIDVALUE", (BRBaseConvert.ConvertToDouble(myColl[Item].ToString().Substring(52, 14)) * 0.01).ToString());
                                    LineItemsTable.Add("CRUNPAIDCOUNT", 0);
                                    LineItemsTable.Add("CRUNPAIDAMOUNT", 0);
                                    LineItemsTable.Add("DRUNPAIDCOUNT", 0);
                                    LineItemsTable.Add("DRUNPAIDAMOUNT", 0);
                                    LineItemsTable.Add("MDVCOUNT", 0);
                                    LineItemsTable.Add("MDVAMOUNT", 0);
                                    LineItemsTable.Add("DISCREPANCYCOUNT", myColl[Item].ToString().Substring(26, 6));
                                    LineItemsTable.Add("DISCREPANCYAMOUNT", (BRBaseConvert.ConvertToDouble(myColl[Item].ToString().Substring(32, 14)) * 0.01).ToString());

                                }
                                LineItemsTable.Add("DATA", myColl[Item].ToString());
                                LineItemsTable.Add("DATE", WorkingDate);
                                LineItemsTable.Add("FILENAME", filePath.Substring(filePath.LastIndexOf("\\") + 1).Replace("_Temp", ""));
                                LineItemsTable.Add("FILEDATE", FileDate);
                                break;
                        }
                        break;

                }
                data = CreateDataTableAndPopulate(LineItemsTable, dt, FileType,(myColl[Item].ToString().Substring(2, 2)));
                //Item = Item + 1;
                LineItemsTable.Clear();
            }
            }
            catch (Exception ex)
            {
                string AppendErrorMessage = "Error Message: BREXUtility 386 :" + ex.ToString() + Environment.NewLine + "Date" + ":" + DateTime.Now + Environment.NewLine + "--------------------------" + Environment.NewLine;
                System.IO.File.AppendAllText("C:\\ClearingFiles\\" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + ".txt", AppendErrorMessage);
            }
            return data;
        }
        private static DataTable CreateDataTableAndPopulate(Hashtable HTable,DataTable dt, string FileType,string VoucherCodeForEFT)
        {
            try
            {
            switch (FileType.ToString())
            {
                case "EJ":
                    if (dt.Columns.Count > 0)
                    {

                    }
                    else
                    {
                        dt.Columns.Add("RETURNCODE");
                        dt.Columns.Add("VOUCHERTYPE");
                        dt.Columns.Add("AMOUNT");
                        dt.Columns.Add("ENTRYMODE");
                        dt.Columns.Add("TOBANK");
                        dt.Columns.Add("TOBRANCH");
                        dt.Columns.Add("TOACCOUNT");
                        dt.Columns.Add("CHEQUEDIGIT");
                        dt.Columns.Add("FROMBANK");
                        dt.Columns.Add("FROMBRANCH");
                        dt.Columns.Add("FILLER");
                        dt.Columns.Add("COLLECTIONACCOUNT");
                        dt.Columns.Add("SERIALNUMBER");
                        dt.Columns.Add("PROCESSINGNO");
                        dt.Columns.Add("DATA");
                        dt.Columns.Add("MICRLINE");
                        dt.Columns.Add("DATE");
                        dt.Columns.Add("TRXTYPE");
                        dt.Columns.Add("FILENAME");
                        dt.Columns.Add("BIMAGESIGN");
                        dt.Columns.Add("FIMAGESIGN");
                        dt.Columns.Add("BIMAGESIGN");
                        dt.Columns.Add("BIMAGE");
                        dt.Columns.Add("FIMAGE");
                        dt.Columns.Add("BIMAGE");
                    }
                    if (HTable.Count > 0)
                    {
                        DataRow row = dt.NewRow();
                        row["RETURNCODE"] = HTable["RETURNCODE"].ToString();
                        row["VOUCHERTYPE"] = HTable["VOUCHERTYPE"].ToString();
                        row["AMOUNT"] = HTable["AMOUNT"].ToString();
                        row["ENTRYMODE"] = HTable["ENTRYMODE"].ToString();
                        row["TOBANK"] = HTable["TOBANK"].ToString();
                        row["TOBRANCH"] = HTable["TOBRANCH"].ToString();
                        row["TOACCOUNT"] = HTable["TOACCOUNT"].ToString();
                        row["CHEQUEDIGIT"] = HTable["CHEQUEDIGIT"].ToString();
                        row["FROMBANK"] = HTable["FROMBANK"].ToString();
                        row["FROMBRANCH"] = HTable["FROMBRANCH"].ToString();
                        row["FILLER"] = HTable["FILLER"].ToString();
                        row["COLLECTIONACCOUNT"] = HTable["COLLECTIONACCOUNT"].ToString();
                        row["SERIALNUMBER"] = HTable["SERIALNUMBER"].ToString();
                        row["PROCESSINGNO"] = HTable["PROCESSINGNO"].ToString();
                        row["DATA"] = HTable["DATA"].ToString();
                        row["MICRLINE"] = HTable["MICRLINE"].ToString();
                        row["DATE"] = HTable["DATE"];
                        row["TRXTYPE"] = HTable["TRXTYPE"].ToString();
                        row["FILENAME"] = HTable["FILENAME"].ToString();
                        row["BIMAGESIGN"] = HTable["BIMAGESIGN"];
                        row["FIMAGESIGN"] = HTable["FIMAGESIGN"];
                        row["BIMAGESIGN"] = HTable["BIMAGESIGN"];
                        row["BIMAGE"] = HTable["BIMAGE"];
                        row["FIMAGE"] = HTable["FIMAGE"];
                        row["BIMAGE"] = HTable["BIMAGE"];
                        dt.Rows.Add(row);
                    } 
                    break;

                case "FCYEJ":
                    if (dt.Columns.Count > 0)
                    {

                    }
                    else
                    {
                        dt.Columns.Add("RETURNCODE");
                        dt.Columns.Add("VOUCHERTYPE");
                        dt.Columns.Add("AMOUNT");
                        dt.Columns.Add("ENTRYMODE");
                        dt.Columns.Add("TOBANK");
                        dt.Columns.Add("TOBRANCH");
                        dt.Columns.Add("TOACCOUNT");
                        dt.Columns.Add("CHEQUEDIGIT");
                        dt.Columns.Add("FROMBANK");
                        dt.Columns.Add("FROMBRANCH");
                        dt.Columns.Add("FILLER");
                        dt.Columns.Add("COLLECTIONACCOUNT");
                        dt.Columns.Add("SERIALNUMBER");
                        dt.Columns.Add("PROCESSINGNO");
                        dt.Columns.Add("DATA");
                        dt.Columns.Add("MICRLINE");
                        dt.Columns.Add("DATE");
                        dt.Columns.Add("TRXTYPE");
                        dt.Columns.Add("FILENAME");
                        dt.Columns.Add("BIMAGESIGN");
                        dt.Columns.Add("FIMAGESIGN");
                        dt.Columns.Add("BIMAGESIGN");
                        dt.Columns.Add("BIMAGE");
                        dt.Columns.Add("FIMAGE");
                        dt.Columns.Add("BIMAGE");
                    }
                    if (HTable.Count > 0)
                    {
                        DataRow row = dt.NewRow();
                        row["RETURNCODE"] = HTable["RETURNCODE"].ToString();
                        row["VOUCHERTYPE"] = HTable["VOUCHERTYPE"].ToString();
                        row["AMOUNT"] = HTable["AMOUNT"].ToString();
                        row["ENTRYMODE"] = HTable["ENTRYMODE"].ToString();
                        row["TOBANK"] = HTable["TOBANK"].ToString();
                        row["TOBRANCH"] = HTable["TOBRANCH"].ToString();
                        row["TOACCOUNT"] = HTable["TOACCOUNT"].ToString();
                        row["CHEQUEDIGIT"] = HTable["CHEQUEDIGIT"].ToString();
                        row["FROMBANK"] = HTable["FROMBANK"].ToString();
                        row["FROMBRANCH"] = HTable["FROMBRANCH"].ToString();
                        row["FILLER"] = HTable["FILLER"].ToString();
                        row["COLLECTIONACCOUNT"] = HTable["COLLECTIONACCOUNT"].ToString();
                        row["SERIALNUMBER"] = HTable["SERIALNUMBER"].ToString();
                        row["PROCESSINGNO"] = HTable["PROCESSINGNO"].ToString();
                        row["DATA"] = HTable["DATA"].ToString();
                        row["MICRLINE"] = HTable["MICRLINE"].ToString();
                        row["DATE"] = HTable["DATE"];
                        row["TRXTYPE"] = HTable["TRXTYPE"].ToString();
                        row["FILENAME"] = HTable["FILENAME"].ToString();
                        row["BIMAGESIGN"] = HTable["BIMAGESIGN"];
                        row["FIMAGESIGN"] = HTable["FIMAGESIGN"];
                        row["BIMAGESIGN"] = HTable["BIMAGESIGN"];
                        row["BIMAGE"] = HTable["BIMAGE"];
                        row["FIMAGE"] = HTable["FIMAGE"];
                        row["BIMAGE"] = HTable["BIMAGE"];
                        dt.Rows.Add(row);
                    } 
                    break;
                case "EFT":
                    if (VoucherCodeForEFT == "90")
                    {
                        if (dt.Columns.Count > 0)
                        {

                        }
                        else
                        {
                            dt.Columns.Add("RETURNCODE");
                            dt.Columns.Add("VOUCHERTYPE");
                            dt.Columns.Add("AMOUNT");
                            dt.Columns.Add("TOBANK");
                            dt.Columns.Add("TOBRANCH");
                            dt.Columns.Add("TOACCOUNT");
                            dt.Columns.Add("FROMBANK");
                            dt.Columns.Add("FROMBRANCH");
                            dt.Columns.Add("COLLECTIONACCOUNT");
                            dt.Columns.Add("REMITTER");
                            dt.Columns.Add("DESCRIPTION");
                            dt.Columns.Add("DRAWERORPAYEE");
                            dt.Columns.Add("ORIGINATINGCODE");
                            dt.Columns.Add("ORIGINATINGREF");
                            dt.Columns.Add("POLICY1");
                            dt.Columns.Add("POLICY2");
                            dt.Columns.Add("REMARKS");
                            dt.Columns.Add("DATA");
                            dt.Columns.Add("DATE");
                            dt.Columns.Add("TRXTYPE");
                            dt.Columns.Add("FILENAME");
                            dt.Columns.Add("SERIALNUMBER");
                        }
                        if (HTable.Count > 0)
                        {
                            DataRow row = dt.NewRow();
                            row["RETURNCODE"] = HTable["RETURNCODE"].ToString();
                            row["VOUCHERTYPE"] = HTable["VOUCHERTYPE"].ToString();
                            row["AMOUNT"] = HTable["AMOUNT"].ToString();
                            row["TOBANK"] = HTable["TOBANK"].ToString();
                            row["TOBRANCH"] = HTable["TOBRANCH"].ToString();
                            row["TOACCOUNT"] = HTable["TOACCOUNT"].ToString();
                            row["FROMBANK"] = HTable["FROMBANK"].ToString();
                            row["FROMBRANCH"] = HTable["FROMBRANCH"].ToString();
                            row["COLLECTIONACCOUNT"] = HTable["COLLECTIONACCOUNT"].ToString();
                            row["REMITTER"] = HTable["REMITTER"].ToString();
                            row["DESCRIPTION"] = HTable["DESCRIPTION"].ToString();
                            row["DRAWERORPAYEE"] = HTable["DRAWERORPAYEE"].ToString();
                            row["DATA"] = HTable["DATA"].ToString();
                            row["DATE"] = HTable["DATE"];
                            row["TRXTYPE"] = HTable["TRXTYPE"].ToString();
                            row["FILENAME"] = HTable["FILENAME"].ToString();
                            row["SERIALNUMBER"] = HTable["SERIALNUMBER"].ToString();
                            dt.Rows.Add(row);
                        }
                    }
                    else if (VoucherCodeForEFT == "40")
                    {
                        if (dt.Columns.Count > 0)
                        {

                        }
                        else
                        {
                            dt.Columns.Add("RETURNCODE");
                            dt.Columns.Add("VOUCHERTYPE");
                            dt.Columns.Add("AMOUNT");
                            dt.Columns.Add("TOBANK");
                            dt.Columns.Add("TOBRANCH");
                            dt.Columns.Add("TOACCOUNT");
                            dt.Columns.Add("FROMBANK");
                            dt.Columns.Add("FROMBRANCH");
                            dt.Columns.Add("COLLECTIONACCOUNT");
                            dt.Columns.Add("REMITTER");
                            dt.Columns.Add("ORIGINATINGCODE");
                            dt.Columns.Add("ORIGINATINGREF");
                            dt.Columns.Add("POLICY1");
                            dt.Columns.Add("POLICY2");
                            dt.Columns.Add("REMARKS");
                            dt.Columns.Add("DESCRIPTION");
                            dt.Columns.Add("DRAWERORPAYEE");
                            dt.Columns.Add("DATA");
                            dt.Columns.Add("DATE");
                            dt.Columns.Add("TRXTYPE");
                            dt.Columns.Add("FILENAME");
                        }
                        if (HTable.Count > 0)
                        {
                            DataRow row = dt.NewRow();
                            row["RETURNCODE"] = HTable["RETURNCODE"].ToString();
                            row["VOUCHERTYPE"] = HTable["VOUCHERTYPE"].ToString();
                            row["AMOUNT"] = HTable["AMOUNT"].ToString();
                            row["TOBANK"] = HTable["TOBANK"].ToString();
                            row["TOBRANCH"] = HTable["TOBRANCH"].ToString();
                            row["TOACCOUNT"] = HTable["TOACCOUNT"].ToString();
                            row["FROMBANK"] = HTable["FROMBANK"].ToString();
                            row["FROMBRANCH"] = HTable["FROMBRANCH"].ToString();
                            row["COLLECTIONACCOUNT"] = HTable["COLLECTIONACCOUNT"].ToString();
                            row["REMITTER"] = HTable["REMITTER"].ToString();
                            row["ORIGINATINGCODE"] = HTable["ORIGINATINGCODE"].ToString();
                            row["ORIGINATINGREF"] = HTable["ORIGINATINGREF"].ToString();
                            row["POLICY1"] = HTable["POLICY1"].ToString();
                            row["POLICY2"] = HTable["POLICY2"].ToString();
                            row["REMARKS"] = HTable["REMARKS"].ToString();
                            row["DATA"] = HTable["DATA"].ToString();
                            row["DATE"] = HTable["DATE"];
                            row["TRXTYPE"] = HTable["TRXTYPE"].ToString();
                            row["FILENAME"] = HTable["FILENAME"].ToString();
                            dt.Rows.Add(row);
                        }
                    }
                    else
                    {
                        if (dt.Columns.Count > 0)
                        {

                        }
                        else
                        {
                            dt.Columns.Add("RETURNCODE");
                            dt.Columns.Add("VOUCHERTYPE");
                            dt.Columns.Add("AMOUNT");
                            dt.Columns.Add("TOBANK");
                            dt.Columns.Add("TOBRANCH");
                            dt.Columns.Add("TOACCOUNT");
                            dt.Columns.Add("FROMBANK");
                            dt.Columns.Add("FROMBRANCH");
                            dt.Columns.Add("COLLECTIONACCOUNT");
                            dt.Columns.Add("REMITTER");
                            dt.Columns.Add("DESCRIPTION");
                            dt.Columns.Add("DRAWERORPAYEE");
                            dt.Columns.Add("ORIGINATINGCODE");
                            dt.Columns.Add("ORIGINATINGREF");
                            dt.Columns.Add("POLICY1");
                            dt.Columns.Add("POLICY2");
                            dt.Columns.Add("REMARKS");
                            dt.Columns.Add("DATA");
                            dt.Columns.Add("DATE");
                            dt.Columns.Add("TRXTYPE");
                            dt.Columns.Add("FILENAME");
                        }
                        if (HTable.Count > 0)
                        {
                            DataRow row = dt.NewRow();
                            row["RETURNCODE"] = HTable["RETURNCODE"].ToString();
                            row["VOUCHERTYPE"] = HTable["VOUCHERTYPE"].ToString();
                            row["AMOUNT"] = HTable["AMOUNT"].ToString();
                            row["TOBANK"] = HTable["TOBANK"].ToString();
                            row["TOBRANCH"] = HTable["TOBRANCH"].ToString();
                            row["TOACCOUNT"] = HTable["TOACCOUNT"].ToString();
                            row["FROMBANK"] = HTable["FROMBANK"].ToString();
                            row["FROMBRANCH"] = HTable["FROMBRANCH"].ToString();
                            row["COLLECTIONACCOUNT"] = HTable["COLLECTIONACCOUNT"].ToString();
                            row["REMITTER"] = HTable["REMITTER"].ToString();
                            row["DESCRIPTION"] = HTable["DESCRIPTION"].ToString();
                            row["DRAWERORPAYEE"] = HTable["DRAWERORPAYEE"].ToString();
                            row["DATA"] = HTable["DATA"].ToString();
                            row["DATE"] = HTable["DATE"];
                            row["TRXTYPE"] = HTable["TRXTYPE"].ToString();
                            row["FILENAME"] = HTable["FILENAME"].ToString();
                            dt.Rows.Add(row);
                        }
                    }

                    break;
                case "DISC":
                    if (dt.Columns.Count > 0)
                    {

                    }
                    else
                    {
                        dt.Columns.Add("RETURNCODE");
                        dt.Columns.Add("VOUCHERTYPE");
                        dt.Columns.Add("AMOUNT");
                        dt.Columns.Add("ENTRYMODE");
                        dt.Columns.Add("TOBANK");
                        dt.Columns.Add("TOBRANCH");
                        dt.Columns.Add("TOACCOUNT");
                        dt.Columns.Add("CHEQUEDIGIT");
                        dt.Columns.Add("FROMBANK");
                        dt.Columns.Add("FROMBRANCH");
                        dt.Columns.Add("FILLER");
                        dt.Columns.Add("COLLECTIONACCOUNT");
                        dt.Columns.Add("SERIALNUMBER");
                        dt.Columns.Add("PROCESSINGNO");
                        dt.Columns.Add("DATA");
                        dt.Columns.Add("MICRLINE");
                        dt.Columns.Add("DATE");
                        dt.Columns.Add("TRXTYPE");
                        dt.Columns.Add("FILENAME");
                        dt.Columns.Add("BIMAGESIGN");
                        dt.Columns.Add("FIMAGESIGN");
                        dt.Columns.Add("BIMAGESIGN");
                        dt.Columns.Add("BIMAGE");
                        dt.Columns.Add("FIMAGE");
                        dt.Columns.Add("BIMAGE");
                    }
                    if (HTable.Count > 0)
                    {
                        DataRow row = dt.NewRow();
                        row["RETURNCODE"] = HTable["RETURNCODE"].ToString();
                        row["VOUCHERTYPE"] = HTable["VOUCHERTYPE"].ToString();
                        row["AMOUNT"] = HTable["AMOUNT"].ToString();
                        row["ENTRYMODE"] = HTable["ENTRYMODE"].ToString();
                        row["TOBANK"] = HTable["TOBANK"].ToString();
                        row["TOBRANCH"] = HTable["TOBRANCH"].ToString();
                        row["TOACCOUNT"] = HTable["TOACCOUNT"].ToString();
                        row["CHEQUEDIGIT"] = HTable["CHEQUEDIGIT"].ToString();
                        row["FROMBANK"] = HTable["FROMBANK"].ToString();
                        row["FROMBRANCH"] = HTable["FROMBRANCH"].ToString();
                        row["FILLER"] = HTable["FILLER"].ToString();
                        row["COLLECTIONACCOUNT"] = HTable["COLLECTIONACCOUNT"].ToString();
                        row["SERIALNUMBER"] = HTable["SERIALNUMBER"].ToString();
                        row["PROCESSINGNO"] = HTable["PROCESSINGNO"].ToString();
                        row["DATA"] = HTable["DATA"].ToString();
                        row["MICRLINE"] = HTable["MICRLINE"].ToString();
                        row["DATE"] = HTable["DATE"];
                        row["TRXTYPE"] = HTable["TRXTYPE"].ToString();
                        row["FILENAME"] = HTable["FILENAME"].ToString();
                        row["BIMAGESIGN"] = HTable["BIMAGESIGN"];
                        row["FIMAGESIGN"] = HTable["FIMAGESIGN"];
                        row["BIMAGESIGN"] = HTable["BIMAGESIGN"];
                        row["BIMAGE"] = HTable["BIMAGE"];
                        row["FIMAGE"] = HTable["FIMAGE"];
                        row["BIMAGE"] = HTable["BIMAGE"];
                        dt.Rows.Add(row);
                    }
                    break;
                case "SELT":
                    if (dt.Columns.Count > 0)
                    {

                    }
                    else
                    {
                        dt.Columns.Add("RETURNCODE");
                        dt.Columns.Add("BANKNO");
                        dt.Columns.Add("CREDITCOUNT");
                        dt.Columns.Add("CREDITAMOUNT");
                        dt.Columns.Add("DEBITCOUNT");
                        dt.Columns.Add("DEBITAMOUNT");
                        dt.Columns.Add("DISCREPANCYCOUNT");
                        dt.Columns.Add("DISCREPANCYAMOUNT");
                        dt.Columns.Add("UNPAIDCOUNT");
                        dt.Columns.Add("UNPAIDVALUE");
                        dt.Columns.Add("DRUNPAIDCOUNT");
                        dt.Columns.Add("CRUNPAIDAMOUNT");
                        dt.Columns.Add("CRUNPAIDCOUNT");
                        dt.Columns.Add("DRUNPAIDAMOUNT");
                        dt.Columns.Add("MDVCOUNT");
                        dt.Columns.Add("MDVAMOUNT");
                        dt.Columns.Add("DATA");
                        dt.Columns.Add("DATE");
                        dt.Columns.Add("FILENAME");
                        dt.Columns.Add("FILEDATE");
                    }
                    if (HTable.Count > 0)
                    {
                        DataRow row = dt.NewRow();
                        row["RETURNCODE"] = HTable["RETURNCODE"].ToString();
                        row["BANKNO"] = HTable["BANKNO"].ToString();
                        row["CREDITCOUNT"] = HTable["CREDITCOUNT"].ToString();
                        row["CREDITAMOUNT"] = HTable["CREDITAMOUNT"].ToString();
                        row["DEBITCOUNT"] = HTable["DEBITCOUNT"].ToString();
                        row["DEBITAMOUNT"] = HTable["DEBITAMOUNT"].ToString();
                        row["DISCREPANCYCOUNT"] = HTable["DISCREPANCYCOUNT"].ToString();
                        row["DISCREPANCYAMOUNT"] = HTable["DISCREPANCYAMOUNT"].ToString();
                        row["UNPAIDCOUNT"] = HTable["UNPAIDCOUNT"].ToString();
                        row["UNPAIDVALUE"] = HTable["UNPAIDVALUE"].ToString();
                        row["DRUNPAIDCOUNT"] = HTable["DRUNPAIDCOUNT"].ToString();
                        row["DRUNPAIDAMOUNT"] = HTable["DRUNPAIDAMOUNT"].ToString();
                        row["CRUNPAIDCOUNT"] = HTable["CRUNPAIDCOUNT"].ToString();
                        row["CRUNPAIDAMOUNT"] = HTable["CRUNPAIDAMOUNT"].ToString();
                        row["MDVCOUNT"] = HTable["MDVCOUNT"].ToString();
                        row["MDVAMOUNT"] = HTable["MDVAMOUNT"].ToString();
                        row["DATA"] = HTable["DATA"].ToString();
                        row["DATE"] = HTable["DATE"];
                        row["FILENAME"] = HTable["FILENAME"].ToString();
                        row["FILEDATE"] = HTable["FILEDATE"];
                        dt.Rows.Add(row);
                    }
                    break;
                case "FCYSELT":
                    if (dt.Columns.Count > 0)
                    {

                    }
                    else
                    {
                        dt.Columns.Add("RETURNCODE");
                        dt.Columns.Add("BANKNO");
                        //dt.Columns.Add("CURRENCYCODE");
                        dt.Columns.Add("CREDITCOUNT");
                        dt.Columns.Add("CREDITAMOUNT");
                        dt.Columns.Add("DEBITCOUNT");
                        dt.Columns.Add("DEBITAMOUNT");
                        dt.Columns.Add("DISCREPANCYCOUNT");
                        dt.Columns.Add("DISCREPANCYAMOUNT");
                        dt.Columns.Add("UNPAIDCOUNT");
                        dt.Columns.Add("UNPAIDVALUE");
                        dt.Columns.Add("DRUNPAIDCOUNT");
                        dt.Columns.Add("DRUNPAIDAMOUNT");
                        dt.Columns.Add("CRUNPAIDCOUNT");
                        dt.Columns.Add("CRUNPAIDAMOUNT");
                        dt.Columns.Add("MDVCOUNT");
                        dt.Columns.Add("MDVAMOUNT");
                        dt.Columns.Add("DATA");
                        dt.Columns.Add("DATE");
                        dt.Columns.Add("FILENAME");
                        dt.Columns.Add("FILEDATE");
                    }
                    if (HTable.Count > 0)
                    {
                        DataRow row = dt.NewRow();
                        row["RETURNCODE"] = HTable["RETURNCODE"].ToString();
                        row["BANKNO"] = HTable["BANKNO"].ToString();
                        //row["CURRENCYCODE"] = HTable["CURRENCYCODE"].ToString();
                        row["CREDITCOUNT"] = HTable["CREDITCOUNT"].ToString();
                        row["CREDITAMOUNT"] = HTable["CREDITAMOUNT"].ToString();
                        row["DEBITCOUNT"] = HTable["DEBITCOUNT"].ToString();
                        row["DEBITAMOUNT"] = HTable["DEBITAMOUNT"].ToString();
                        row["DISCREPANCYCOUNT"] = HTable["DISCREPANCYCOUNT"].ToString();
                        row["DISCREPANCYAMOUNT"] = HTable["DISCREPANCYAMOUNT"].ToString();
                        row["UNPAIDCOUNT"] = HTable["UNPAIDCOUNT"].ToString();
                        row["UNPAIDVALUE"] = HTable["UNPAIDVALUE"].ToString();
                        row["DRUNPAIDCOUNT"] = HTable["DRUNPAIDCOUNT"].ToString();
                        row["DRUNPAIDAMOUNT"] = HTable["DRUNPAIDAMOUNT"].ToString();
                        row["CRUNPAIDCOUNT"] = HTable["CRUNPAIDCOUNT"].ToString();
                        row["CRUNPAIDAMOUNT"] = HTable["CRUNPAIDAMOUNT"].ToString();
                        row["MDVCOUNT"] = HTable["MDVCOUNT"].ToString();
                        row["MDVAMOUNT"] = HTable["MDVAMOUNT"].ToString();
                        row["DATA"] = HTable["DATA"].ToString();
                        row["DATE"] = HTable["DATE"];
                        row["FILENAME"] = HTable["FILENAME"].ToString();
                        row["FILEDATE"] = HTable["FILEDATE"];
                        dt.Rows.Add(row);
                    }
                    break;
            }
           
            }
            catch (Exception ex)
            {
                string AppendErrorMessage = "Error Message: BREXUtility 1004 :" + ex.ToString() + Environment.NewLine + "Date" + ":" + DateTime.Now + Environment.NewLine + "--------------------------" + Environment.NewLine;
                System.IO.File.AppendAllText("C:\\ClearingFiles\\" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + ".txt", AppendErrorMessage);
            }
            return dt;
        }

        private static StringCollection ReadFlatFile(string AbsoluteFilePath)
        {
            myCol = new StringCollection();
            filePath = @AbsoluteFilePath;
            if (File.Exists( filePath ))
            {
                 file = null;
                 if (File.Exists(filePath + "_Temp"))
                 {
                     File.Delete(filePath + "_Temp");
                     File.Copy(filePath, filePath + "_Temp", true);
                     filePath = filePath + "_Temp";
                     try
                     {
                         file = new StreamReader(filePath);
                         while (file.Peek() > -1)
                         {
                             My_Line = file.ReadLine().ToString().Trim();
                             if (My_Line.ToString().Trim().Length > 1)
                             {
                                 myCol.Add(My_Line);
                             }
                         }
                     }
                     finally
                     {

                     }
                 }
                 else
                 {
                     File.Copy(filePath, filePath + "_Temp", true);
                     filePath = filePath + "_Temp";
                     try
                     {
                         file = new StreamReader(filePath);
                         while (file.Peek() > -1)
                         {
                             My_Line = file.ReadLine().ToString().Trim();
                             if (My_Line.ToString().Trim().Length > 1)
                             {
                                 myCol.Add(My_Line);
                             }
                         }
                     }
                     finally
                     {
                         
                     }
                 }
            }
            if (file != null)
            file.Close();
            File.Delete(filePath);
            return myCol;
        }

        private static Collection ReadFlatFileColl(string AbsoluteFilePath)
        {
            Collection myCol = new Collection();
            filePath = AbsoluteFilePath;
            if (File.Exists(filePath))
            {
                file = null;
                //if (ConfigurationManager.AppSettings["sysEnc"] == "1")
                //{
                //    InPGP(filePath);
                //}
                if (File.Exists(filePath + "_Temp"))
                {
                    File.Delete(filePath + "_Temp");
                    File.Copy(filePath, filePath + "_Temp", true);
                    filePath = filePath + "_Temp";
                    try
                    {

                        file = new StreamReader(filePath);
                        while (file.Peek() > -1)
                        {
                            My_Line = file.ReadLine().ToString().Trim();
                            if (My_Line.ToString().Trim().Length > 1)
                            {
                                myCol.Add(My_Line);
                            }
                        }
                    }
                    finally
                    {

                    }
                }
                else
                {
                    File.Copy(filePath, filePath + "_Temp", true);
                    filePath = filePath + "_Temp";
                    try
                    {
                        file = new StreamReader(filePath);
                        while (file.Peek() > -1)
                        {
                            My_Line = file.ReadLine().ToString().Trim();
                            if (My_Line.ToString().Trim().Length > 1)
                            {
                                myCol.Add(My_Line);
                            }
                        }
                    }
                    finally
                    {

                    }
                }
            }
            
            if (file != null)
                file.Close();
            File.Delete(filePath);
            return myCol;
        }
        static private bool InPGP(string Filename)
        {
            //IEncryptionService encryptionService = new EncryptionService(ConfigurationManager.AppSettings["strPGPExe"].Trim());
            string DestPath = ConfigurationManager.AppSettings["IncomingFiles"].Trim().ToUpper();
            bool RValue = false;
            FileInfo encryptedFile = new FileInfo(DestPath);
            //DirectoryInfo di = new DirectoryInfo(DestPath); FileInfo[] fi = di.GetFiles(); foreach (FileInfo inf in fi)
            //{
                try
                {
                    //string sExt = Path.GetExtension(inf.FullName);
                    //string FileName = inf.FullName;
                    string sExt = Path.GetExtension(Filename);
                    string FileName = Filename;
                    string origFileName = "";
                    int FirstIndexPstofBackSlash = FileName.LastIndexOf("\\");
                    int FristIndexPstofDot = FileName.LastIndexOf(".");
                    int lenOfTheFileName = FristIndexPstofDot - FirstIndexPstofBackSlash;
                    origFileName = FileName.Substring(FirstIndexPstofBackSlash + 1, lenOfTheFileName - 1);
                    switch (sExt.ToString().ToUpper())
                    {
                        case ".GPG":
                            //encryptedFile = encryptionService.DecryptFile(Filename, DestPath + "\\" + origFileName);
                            break;
                    }
                    RValue = true;
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Failed Decrypting file" + Filename);
                    RValue = false;
                }
            //}
            return RValue;
        }
        public static Microsoft.VisualBasic.Collection readImageFile(string fileName, bool isCheque)
        {
            Microsoft.VisualBasic.Collection myColl= new Microsoft.VisualBasic.Collection();
            try 
            {
                return myColl;
            }
	        catch (Exception ex) 
            {
		        return myColl;
	        }

        }
        public static DataTable ReadImagesFromFile(string PathOfTheFile, string Currency, string PresentingBank, bool CheckIfIsFcy, bool IsTruncationReady )
        {
            string Country = ConfigurationManager.AppSettings["Country"].Trim().ToUpper();
            switch (Country.ToUpper())
            {
                case "UG":
                    IsTruncationReady = false;
                    break;
                case "KE":
                    IsTruncationReady = true;
                    break;
            }
            
            DataTable data = new DataTable();
            DataTable dt = new DataTable();
            byte[] fImageBW = null;
            byte[] fImage = null;
            byte[] bImage = null;
            Int32 myFrontSize = default(Int32);
            Int32 myFrontSize1 = default(Int32);
            Int32 myRearSize = default(Int32);
            long signCounter = 0;
            Hashtable LineItemsTable = null;
             DataTable dtImgs  = new DataTable();
            System.IO.FileStream MyFile = null;
            System.IO.StreamReader StreamReader = null;
            Microsoft.VisualBasic.Collection myCol = new Microsoft.VisualBasic.Collection();
            int Item = 1;
            string My_Line = "";
            DateTime WorkingDate = DateTime.Today;
            bool enableTruncation = IsTruncationReady;
            try
            {
                LineItemsTable = new Hashtable();
                if (System.IO.File.Exists(PathOfTheFile) == true)
                {
                    if (System.IO.File.Exists(PathOfTheFile + "_Temp") == true)
                        System.IO.File.Delete(PathOfTheFile + "_Temp");
                    System.IO.File.Copy(PathOfTheFile, PathOfTheFile + "_Temp", true);
                    MyFile = System.IO.File.OpenRead(PathOfTheFile + "_Temp");
                    StreamReader = new System.IO.StreamReader(MyFile);
                    if (enableTruncation == false)
                    {
                        while (StreamReader.Peek() > -1)
                        {
                            My_Line = StreamReader.ReadLine().ToString().Trim();

                            if (My_Line.Trim().Length > 1)
                            {
                                myCol.Add(My_Line) ;
                            }
                        }
                    }
                    else
                    {
                        //hapa import the image file
                        myCol = readImageFile(PathOfTheFile,true);
                    }
                    if (myCol.Count > 0)
                    {
                        //break Down the Line then
                        while (Item <= myCol.Count)
                        {
                            switch (myCol[Item].ToString().Substring(0, 2))
                            {
                                case "16":
                                case "18":
                                case "19":
                                    Item += 1;
                                    continue;
                                default:
                                    LineItemsTable.Add("RETURNCODE", myCol[Item].ToString().Substring(0, 2));
                                    // RCode
                                    LineItemsTable.Add("VOUCHERTYPE", myCol[Item].ToString().Substring(2, 2));
                                    // Voucher Type
                                    LineItemsTable.Add("AMOUNT", (BRBaseConvert.ConvertToDouble((myCol[Item]).ToString().Substring(4, 12)) / 100).ToString());
                                    // Amount
                                    LineItemsTable.Add("ENTRYMODE", myCol[Item].ToString().Substring(16, 1));
                                    // Amount Entry Mode
                                    switch (Country.ToUpper())
                                    {
                                        case "UG":
                                            //switch (myCol[Item].ToString().Substring(0, 2))
                                            //{
                                            //    case "00": //Presentments
                                            //    case "17": //REPresentments
                                                        LineItemsTable.Add("TOBANK", myCol[Item].ToString().Substring(17, 2));
                                                        // Dest Bank
                                                        LineItemsTable.Add("TOBRANCH", myCol[Item].ToString().Substring(19, 4));
                                                        // Dest Branch
                                                        LineItemsTable.Add("TOACCOUNT", myCol[Item].ToString().Substring(23, 10));
                                                        // Dest Account
                                                        LineItemsTable.Add("FROMBANK", myCol[Item].ToString().Substring(57, 2));
                                                        // PBank
                                                        LineItemsTable.Add("FROMBRANCH", myCol[Item].ToString().Substring(59, 4));
                                                        // PBranch
                                                        LineItemsTable.Add("COLLECTIONACCOUNT", myCol[Item].ToString().Substring(37, 20));
                                                        //Collecting Account Details
                                                    //break;
                                                //default:
                                                //        LineItemsTable.Add("TOBANK", myCol[Item].ToString().Substring(57, 2));
                                                //        // Dest Bank
                                                //        LineItemsTable.Add("TOBRANCH", myCol[Item].ToString().Substring(59, 4));
                                                //        // Dest Branch
                                                //        LineItemsTable.Add("TOACCOUNT", myCol[Item].ToString().Substring(37, 10));
                                                //        // Dest Account
                                                //        LineItemsTable.Add("FROMBANK", myCol[Item].ToString().Substring(17, 2));
                                                //        // PBank
                                                //        LineItemsTable.Add("FROMBRANCH", myCol[Item].ToString().Substring(19, 4));
                                                //        // PBranch
                                                //        LineItemsTable.Add("COLLECTIONACCOUNT", myCol[Item].ToString().Substring(24, 20));
                                                //        //Collecting Account Details
                                                //    break;
                                            //}
                                            LineItemsTable.Add("CHEQUEDIGIT", myCol[Item].ToString().Substring(33, 2));
                                            // Check Digit
                                            LineItemsTable.Add("FILLER", myCol[Item].ToString().Substring(34, 4));
                                            // Filler
                                            LineItemsTable.Add("SERIALNUMBER", myCol[Item].ToString().Substring(63, 6));
                                            // Serial Number
                                            LineItemsTable.Add("PROCNO", myCol[Item].ToString().Substring(69, 9));
                                            // Processing Number
                                        break;
                                        
                                        case "KE": ///Add this
                                            LineItemsTable.Add("CURRENCYCODE", myCol[Item].ToString().Substring(34, 2));
                                        break;
                                       
                                    }
                                    LineItemsTable.Add("FILENAME", PathOfTheFile.Substring(PathOfTheFile.LastIndexOf("\\") + 1));
                                    LineItemsTable.Add("DATA", myCol[Item].ToString());
                                    LineItemsTable.Add("TRXTYPE", "ID");
                                    LineItemsTable.Add("DATE", WorkingDate);
                                    // The Filename
                                    switch (Country.ToUpper())
                                    {
                                        case "UG":
                                           
                                            break;
                                        case "KE":
                                                //data = CreateDataTableAndPopulate(LineItemsTable, dt, FileType, (myCol[Item].ToString().Substring(2, 2)));
                                                //SaveImagesToDB(LineItemsTable, fImageBW, fImage, bImage);
                                            break;
                                    }
                                    break;
                            }
                            Item = Item + 1;
                              if (dtImgs.Columns.Count <= 0)
                              {
                                   foreach (string name in LineItemsTable.Keys)
                                   {
                                        DataColumn  ColName = new DataColumn();
                                        ColName.ColumnName = name;
                                        ColName.DataType = System.Type.GetType(LineItemsTable[name].GetType().FullName.ToString());
                                        dtImgs.Columns.Add(ColName);
                                   }
                              }
                        DataRow  dr = dtImgs.NewRow();
                        foreach (string  name  in LineItemsTable.Keys)
                        {
                            dr[name] = LineItemsTable[name];
                        }
                        dtImgs.Rows.Add(dr);

                            LineItemsTable.Clear();
                            
                        }
                    }
                }
                MyFile.Close();
                myCol.Clear();
                System.IO.File.Delete(MyFile.Name);
                MyFile.Dispose();
            }
            catch (Exception ex)
            {
                LineItemsTable.Clear();
                myCol.Clear();
                MyFile.Close();
                System.IO.File.Delete(MyFile.Name);
                return dtImgs;
            }
            return dtImgs;

        }

        public bool SaveImagesToDB(Hashtable HashBrOutClearing, byte[] bFTFImage, byte[] bFJFImage, byte[] bRJImage)
        {
            string FTFImage = null;
            string FJFImage = null;
            string RJImage = null;
            try
            {
                if (HashBrOutClearing.Count != 0)
                {
                    FTFImage = Bytes2String(bFTFImage);
                    FJFImage = Bytes2String(bFJFImage);
                    RJImage = Bytes2String(bRJImage);
                    byte[] FIMAGESIGNBW ={ Convert.ToByte(HashBrOutClearing["FIMAGESIGNBW"]) };
                    byte[] FIMAGESIGN ={ Convert.ToByte(HashBrOutClearing["FIMAGESIGN"]) };
                    byte[] BIMAGESIGN ={ Convert.ToByte(HashBrOutClearing["BIMAGESIGN"]) };
                    string TFImageSignature = Bytes2String(FIMAGESIGNBW);
                    string JFImageSignature = Bytes2String(FIMAGESIGN);
                    string JRImageSignature = Bytes2String(BIMAGESIGN);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static byte[] HashTheImage(byte[] DataInByte)
        {
            byte[] Result = null;
            SHA384Managed sha = new SHA384Managed();
            try
            {
                Result = sha.ComputeHash(DataInByte);
            }
            catch (Exception ex)
            {
                return null;
            }
            return Result;
        }

        public static bool CompareTwoHashes(byte[] tmpNewHash, byte[] tmpHash)
        {
            bool bEqual = false;
            if (tmpNewHash.Length == tmpHash.Length)
            {
                int i = 0;
                while ((i < tmpNewHash.Length))
                {
                    if ((tmpNewHash[i] == tmpHash[i]))
                    {
                        i += 1;
                    }
                    else
                    {
                        break;
                    }
                }
                if (i == tmpNewHash.Length)
                {
                    bEqual = true;
                }
            }
            return bEqual;
        }

   public string Bytes2String(byte[] bytes)
   {
       return System.Text.Encoding.GetEncoding(1252).GetString(bytes);
   }
   public byte[] String2Bytes(string str)
   {
       return System.Text.Encoding.GetEncoding(1252).GetBytes(str);
   }

   }
}
