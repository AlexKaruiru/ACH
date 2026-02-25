using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.IO;
using DALProviders.DALTransactionService;
using BR;
using BR.ApplicationBlocks.Data;
using BREntities;
using BR.DBClient;
using DALProviders.DALProcessService;


using System.Configuration;
using BRCoreEntities.SystemBankSettings.Optional;
using BRDALLibrary;
using BRCoreEntities.SystemBranchSettings;
using BRCoreEntities.SystemBankSettings;
using BRCoreEntities.SystemBranchStatus;
using BALProviders.BALTransactionService;
using BREntities.InwardImportedTransactions;
using BREntities.InwardSaveFlatFileImports;
using BREntities.Transactions;
using BREntities.ClearingFileFormat;
using BREntities.BRCreateClearingFile;
using BREntities.BRSettlementClearingFile;
using System.Collections;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using BrDataEncryption;
using BRBase;


namespace BRNETUploadDownload
{
    public partial class frmProcessUtility : Form
    {
        private static DS_InwardImportedTransactions dsData = null;
        public static UserInfo usrinfo = null;
        private static StringBuilder sb = null;
        private static DS_InwardImportedTransactions dsTransact = null;
        private static Random random = new Random();
        private static BRDataSet dsProcesses = null;
        public frmProcessUtility()
        {
            InitializeComponent();
            usrinfo = new UserInfo();
            dsData = new DS_InwardImportedTransactions();
            dsTransact = new DS_InwardImportedTransactions();
            dsProcesses = new BRDataSet();
            sb = new StringBuilder();
            random = new Random((int)DateTime.Now.Ticks);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            lblMessage.Refresh();
            btnExecute.Enabled = false;
            btnExit.Enabled = false;
            cboProcess.Enabled = false;
            ArrayList ProcessesStatus = new ArrayList();
            try
            {
                lblMessage.Text = "Process started. Please wait.....";
                lblMessage.Refresh();
                string ProcessName = cboProcess.SelectedItem.ToString();
                DateTime WorkingDate = new DateTime();
                string OuBranchID = string.Empty;
                string ProcessID = string.Empty;
                string OperID = string.Empty;
                string ERRORNO = string.Empty;
                string ERRORPARAMS = string.Empty;
                WorkingDate = SODDate(usrinfo, usrinfo.strBranch);
                string FPath = ConfigurationManager.AppSettings["ImagePath"];
                //BRDataSet dsProcesses = new BRDataSet();
                BRDataSet dsProcessesOut = new BRDataSet();
                object[] Parameters;
                string ProcessProcedureID = string.Empty;
                StringCollection inputParamList = new StringCollection(); ;
                if (FPath != "")
                {
                    if (rdDelete.Checked == true)
                    {
                        System.IO.DirectoryInfo Dir = new System.IO.DirectoryInfo(FPath);
                        foreach (System.IO.FileInfo file in Dir.GetFiles()) file.Delete();
                        foreach (System.IO.DirectoryInfo SubDir in Dir.GetDirectories()) SubDir.Delete(true);
                    }
                }
                for (Int32 x = 0; x < dsProcesses.Tables[0].Rows.Count; x++)
                {
                    if (dsProcesses.Tables[0].Rows[x]["SubProcessName"].ToString() == ProcessName)
                    {
                        ProcessProcedureID = dsProcesses.Tables[0].Rows[x]["ProcedureName"].ToString();
                        break;
                    }
                }
                inputParamList = RetrieveSPParams(usrinfo, ProcessProcedureID);
                Parameters = new object[inputParamList.Count];
                for (int i = 0, j = inputParamList.Count; i < j; i++)
                {
                    string Param = inputParamList[i].ToString().ToUpper();
                    switch (Param)
                    {
                        case "@OURBRANCHID":
                            Parameters[i] = usrinfo.strBranch;
                            break;
                        case "@PROCESSID":
                            Parameters[i] = ProcessProcedureID;
                            break;
                        case "@PROCESSDATE":
                            Parameters[i] = WorkingDate;
                            break;
                        case "@OPERATORID":
                            Parameters[i] = usrinfo.strUser;
                            break;
                        case "@ERRORNO":
                            Parameters[i] = 0;
                            break;
                        case "@ERRORPARAMS":
                            Parameters[i] = "";
                            break;
                    }
                }
                //OutClearingFile.ClearingUniversalMethod(usrinfo, ProcessProcedureID, out dsProcessesOut, BRModule.OtherProcesses, new object[] { "BrDataSet" }, Parameters);
                if (dsProcessesOut.Tables[0].Rows.Count > 0)
                {
                    ProcessesStatus.Add(dsProcessesOut.Tables[0].Rows[0]["Status"].ToString());
                    ProcessesStatus.Add(dsProcessesOut.Tables[0].Rows[0]["ErrorMessage"].ToString());
                    lblMessage.Text = dsProcessesOut.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    if (lblMessage.Text == "")
                    {
                        if (dsProcessesOut.Tables[0].Rows[0]["Status"].ToString() == "Ok")
                        {
                            lblMessage.Text = "Process completed successful";
                        }
                    }
                    lblMessage.Refresh();
                }
                btnExecute.Enabled = true;
                btnExit.Enabled = true;
                cboProcess.Enabled = true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message.ToString();
                lblMessage.Refresh();
                btnExecute.Enabled = true;
                btnExit.Enabled = true;
                cboProcess.Enabled = true;
            }
        }

        private void frmProcessUtility_Load(object sender, EventArgs e)
        {

           
            //OutClearingFile.ClearingUniversalMethod(usrinfo, "p_GetUtilityProcesses", out dsProcesses, BRModule.OtherProcesses, new object[] { "BrDataSet" }, new object[] { usrinfo.strBranch, "UTL" });
            if (dsProcesses.Tables.Contains("BrNames"))
            {
                if (dsProcesses.Tables[0].Rows.Count > 0)
                {
                    ArrayList Processes = new ArrayList();
                    for (Int32 x = 0; x < dsProcesses.Tables[0].Rows.Count; x++)
                    {
                        Processes.Add(dsProcesses.Tables[0].Rows[x]["SubProcessName"]);
                    }
                    foreach (string ProcessesString in Processes)
                        cboProcess.Items.Add(ProcessesString);
                    cboProcess.SelectedText= cboProcess.Items[0].ToString();
                    cboProcess.SelectedIndex = 0;
                }
            }
        }
        public static DateTime SODDate(UserInfo usrinfo, string strOurBrachID)
        {
            DS_SystemBranchStatus dsSystemBranchStatus = GetSPSystemBranchStatus(usrinfo, strOurBrachID);
            if (dsSystemBranchStatus == null || (dsSystemBranchStatus.t_SystemBranchStatus.Rows.Count != 1))
                throw new NullReferenceException();
            return Convert.ToDateTime(dsSystemBranchStatus.t_SystemBranchStatus[0].SODDate);
        }
        static public StringCollection RetrieveSPParams(UserInfo usrinfo, string ProcedureName)
        {
            IDBHelper intfDBHelper = DBClient.GetDBHelper(usrinfo);
            object[] inputParamList;
            StringCollection ParamResults = new StringCollection();
            using (IDbConnection connection = BRNetUploadDownLoadUtility.GetConnection())
            {
                inputParamList = (object[])intfDBHelper.GetSPParameters(connection, ProcedureName);
            }
            for (int i = 0; i < inputParamList.Length; i++)
            {
                ParamResults.Add(((System.Data.SqlClient.SqlParameter)(inputParamList[i])).ParameterName.ToString());
            }
            return ParamResults;
        }
        static public DS_SystemBranchStatus GetSPSystemBranchStatus(UserInfo usrinfo, string strOurBranchID)
        {
            DS_SystemBranchStatus dsSystemBranchStatus = null;
            DS_SystemBranchStatus dsBranchStatuscache = new DS_SystemBranchStatus();
            try
            {
                using (IDbConnection connection =BRNetUploadDownLoadUtility.GetConnection())
                {
                    dsSystemBranchStatus = new DS_SystemBranchStatus();
                    IDBHelper intfDBHelper = DBClient.GetDBHelper(usrinfo);
                    IDataParameter[] arParms = intfDBHelper.CreateDBParamsArray(1);

                    arParms[0] = intfDBHelper.CreateNewDBParam("LanguageID", SqlDbType.VarChar, 3);
                    arParms[0].Value = usrinfo.strLanguage;

                    intfDBHelper.FillDataset(connection, CommandType.StoredProcedure, "pc_SystemBranchStatus", dsSystemBranchStatus, new string[] { "dt_SystemBranchStatus" }, arParms);
                    DataRow[] datarows = dsSystemBranchStatus.t_SystemBranchStatus.Select("OurBranchID='" + strOurBranchID + "'");
                    dsBranchStatuscache.Merge(datarows, false, MissingSchemaAction.Add);
                    return dsBranchStatuscache;
                }
            }
            catch (Exception ex)
            {
                throw DBClientUtils.GetDBErrorMessages(ex, usrinfo.strUser, usrinfo.strSystem);
            }
        }
    }
}