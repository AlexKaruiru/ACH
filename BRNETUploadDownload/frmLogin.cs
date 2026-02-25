using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using BREntities;
using System.Windows.Forms;
using BrDataEncryption;
using BR.DBClient;
using System.Configuration;
using System.Security.Cryptography;
using BALProviders.BALSystemSecurityService;
using BRClearing.Util;
using BrClearing.Common;
using BRNETMFIUploadDownload;
using BROutgoingClearingStatus;

namespace BRNETUploadDownload
{
    public partial class frmLogin : Form
    {
        BRBase.UserInfo usrinfo;

        public frmLogin()
        {
            InitializeComponent();

        }
        public static string OperatorID = string.Empty;
        private void btnOk_Click(object sender, EventArgs e)
        {
            lblmessage.Text = "";
            bool StatusBased = false;
            string strPassword = txtPassword.Text;


            usrinfo.strUser = txtUserName.Text;
            Modscan.OperatorID = txtUserName.Text;
            string Mod = ConfigurationManager.AppSettings["Module"];
            StatusBased = Convert.ToBoolean(ConfigurationManager.AppSettings["ETACH"]);
            string EncryptPassword = string.Empty;
            EncryptPassword = ClearingValidations.EncryptText(ClearingValidations.CreateEncryptData(usrinfo, strPassword));
            bool ret = ClearingValidations.AuthenticateUser(usrinfo, EncryptPassword);
            if (ret != false)
            {
                if (Mod != "Proc")
                {
                    BRNetUploadDownLoadUtility BRNetUploadDownLoadUtil = new BRNetUploadDownLoadUtility();
                    BRNetUploadDownLoadUtilityMFI BRNETUpDownUTil = new BRNetUploadDownLoadUtilityMFI();
                    frmClearingStatus frmClearingStatus = new frmClearingStatus();
                    this.Hide();
                    if (StatusBased)
                    {
                        // BRNetUploadDownLoadUtil.Show();
                        frmClearingStatus.Show();
                    }
                    else
                    {
                        if (chkMFI.Checked)
                        { BRNETUpDownUTil.Show(); }
                        else { BRNetUploadDownLoadUtil.Show(); }
                    }
                }
                else
                {
                    //frmProcessUtility ProcessUtility = new frmProcessUtility();
                    //this.Hide();
                    ////frmProcessUtility.usrinfo = usrinfo;
                    //ProcessUtility.Show();
                }
            }
            else
            {
                lblmessage.Text = "Invalid credentials";
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void GetConfigConnDetails()
        {
            string configFilePath = ConfigurationManager.AppSettings["configFilePath"];
            string strSystem = ConfigurationManager.AppSettings["BRSystem"];

            try
            {

                if (string.IsNullOrEmpty(Modscan.DBServerName))
                {
                    ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
                    fileMap.ExeConfigFilename = configFilePath;

                    Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

                    Modscan.DBServerName = configuration.AppSettings.Settings[strSystem + "-DBServerName"].Value;
                    Modscan.DatabaseName = configuration.AppSettings.Settings[strSystem + "-DatabaseName"].Value;
                    Modscan.BRUserName = configuration.AppSettings.Settings[strSystem + "-BRUserName"].Value;
                    Modscan.DBPassword = configuration.AppSettings.Settings[strSystem + "-BRUserPassword"].Value;

                    //MessageBox.Show(configFilePath + ":" + strSystem + Modscan.DBServerName + ":" + Modscan.DatabaseName + ":" + Modscan.BRUserName + ":" + Modscan.DBPassword);
                }
            }
            catch (Exception ex)
            {
                //lblmessage.Text = configFilePath + ":" + strSystem;
            }
        }
        private void frmLogin_Load(object sender, EventArgs e)
        {
            usrinfo = new BRBase.UserInfo();
            usrinfo.strLanguage = "en";
            usrinfo.strBranch = ConfigurationManager.AppSettings["HeadOfficeBranchID"];
            usrinfo.strBank = ConfigurationManager.AppSettings["BankID"];
            usrinfo.MachineIP = ClearingValidations.GetRequestIP();
            string IP = ConfigurationManager.AppSettings["IPAddress"];
            string DBName = ConfigurationManager.AppSettings["TestDBName"];
            string LiveEnv = ConfigurationManager.AppSettings["IsLiveEnv"];
            string strSystem = ConfigurationManager.AppSettings["IPAddress"];


            Modscan.OurBankID = usrinfo.strBank;
            Modscan.OurBranchID = usrinfo.strBranch;

            if (LiveEnv == "1")
            {
                if (String.IsNullOrEmpty(Modscan.DBServerName))
                {
                    GetConfigConnDetails();
                }
                //Modscan.DBServerName = ConfigurationManager.AppSettings["strDBServerName"];
                //Modscan.DatabaseName = ConfigurationManager.AppSettings["strDatabaseName"]; //"FTBTEST4"; //
                //Modscan.BRUserName = ConfigurationManager.AppSettings["strBRUserName"];
                //Modscan.DBPassword = ConfigurationManager.AppSettings["strBRUserPassword"];
            }
            else
            {
                usrinfo.strSystem = "Data Source=" + IP + ";Initial Catalog=" + DBName + ";User ID=Realm;Password=friend;";
            }

            DateTime WorkingDate = ClearingValidations.SODDate(usrinfo, usrinfo.strBranch);
            lblWorkingDate.Text = WorkingDate.ToShortDateString();

            if (ConfigurationManager.AppSettings["Country"] == "KE" && ConfigurationManager.AppSettings["ClearMFI"] == "1")
            {
                chkMFI.Visible = true;
            }
            else
            {
                chkMFI.Visible = false;
            }
        }
    }
}