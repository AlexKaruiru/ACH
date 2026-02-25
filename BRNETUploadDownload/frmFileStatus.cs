using BRBase;
using BrClearing.Common;
using BRClearing.Util;
using BRNetSecurity;
using BRRTGSProcessing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using static BRRTGSProcessing.Common;
using File = System.IO.File;
using rs = BRRTGSProcessing.Response;
using FileType = BrClearing.Common.ETH.FileType;
using ChequeFormat = BrClearing.Common.ETH.ChequeFormat;
using BrClearing.Common.ETH;
using BrClearing.Common.TZ;
using System.Globalization;

namespace BROutgoingClearingStatus
{
    public enum ETFileTypes : int
    {
        Out_Cheques = 0,
        Out_Rejected_Cheques = 1,
        Presented_Direct_Credit = 2,
        Rejected_Direct_Credit = 3,
        Presented_DirectDebit = 4,
        Rejected_DirectDebit = 5,
        MT103 = 6,
        MT202 = 7,
        MT920 = 8,
        MT999 = 9
    }


    public enum TZFileTypes : int
    {
        Presented_Cheques = 0,
        Rejected_Cheques = 1,
        Presented_Direct_Credit = 2,
        Rejected_Direct_Credit = 3,
        Presented_DirectDebit = 4,
        Rejected_DirectDebit = 5
    }

    public enum ETSession : int
    {
        CT_Clearing = 0,
        Afternoon = 1
    }

    public enum Session : int
    {
        Session1 = 0,
        Session2 = 1,
        Session3 = 3
    }

    public enum ETCurrency : int
    {
        ETB = 0
    }

    public enum TZCurrency : int
    {
        TZS = 0,
        USD = 1,
        GBP = 2,
        EUR = 3
    }

    public enum ETFilesNames : int
    {
        Cheques = 0,
        EFTs = 1,
        RTGS103 = 2,
        RTGS202 = 3,
        RTGS920 = 4,
        RTGS999 = 5,
        ALLRTGS = 6,
        ALLFILES = 7
    }

    public enum FilesNames : int
    {
        Cheques = 0,
        EFTs = 1,
        DD = 2,
    }

    public enum SLNames : int
    {
        SLL = 0
    }
    public enum TZNames : int
    {
        TZS = 0,
        USD = 1,
        GBP = 2,
        EUR = 3,
        KES = 4,
        All_FOREIGN_CURR = 5,
        All_CURR = 6,
    }
    public partial class frmClearingStatus : Form
    {
        private List<string> ArGen;
        private static BRBase.UserInfo usrinfo = null;
        private DataGridView dgvClearingStatus;
        private readonly string ArchivePath = ConfigurationManager.AppSettings["Archive"];
        private string ResponsesPath = ConfigurationManager.AppSettings["IncomingFiles"];
        private string RTGSResponses = ConfigurationManager.AppSettings["RTGSResponses"];
        private string Country = ConfigurationManager.AppSettings["Country"].Trim().ToUpper();

        public frmClearingStatus()
        {
            InitializeComponent();
            usrinfo = new BRBase.UserInfo();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            bool StatusBased = false;
            try
            {
                StatusBased = Convert.ToBoolean(ConfigurationManager.AppSettings["ETACH"]);
            }
            catch { }

            if (StatusBased)
            {
                GC.SuppressFinalize(this);
                this.Close();
                Application.Exit();
            }
            else
            {
                this.Close();
            }

        }
        private static void GetConfigConnDetails()
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
        private void frmClearingStatus_Load(object sender, EventArgs e)
        {
            lblAmount.Text = "0.00";
            lblCurrencyID.Text = "";
            if (String.IsNullOrEmpty(Modscan.DBServerName))
            {
                GetConfigConnDetails();
            }
            //Modscan.DBServerName = ConfigurationManager.AppSettings["strDBServerName"];
            //Modscan.DatabaseName = ConfigurationManager.AppSettings["strDatabaseName"];
            //Modscan.BRUserName = ConfigurationManager.AppSettings["strBRUserName"];
            //Modscan.DBPassword = ConfigurationManager.AppSettings["strBRUserPassword"];
            Modscan.OurBranchID = ConfigurationManager.AppSettings["HeadOfficeBranchID"];
            Modscan.OurBankID = ConfigurationManager.AppSettings["BankID"];
            if (Country.ToUpper() == "ET")
            {
                lblClearingPortalDate.Text = "ATS Business Date";

                foreach (ETFileTypes n in Enum.GetValues(typeof(ETFileTypes)))
                {
                    cboFileType.Items.Add(n);
                }
                cboFileType.SelectedIndex = 0;
                Application.DoEvents();
                cboFileType.Refresh();
                foreach (ETSession n in Enum.GetValues(typeof(ETSession)))
                {
                    cboSession.Items.Add(n);
                }
                cboSession.SelectedIndex = 0;
                Application.DoEvents();
                cboSession.Refresh();
                foreach (ETCurrency n in Enum.GetValues(typeof(ETCurrency)))
                {
                    cboCurrency.Items.Add(n);
                }
                cboCurrency.SelectedIndex = 0;
                Application.DoEvents();
                cboCurrency.Refresh();

                BRDataSet StatusData = new BRDataSet();
                try
                {
                    cboBanks.Items.Clear();
                    Application.DoEvents();
                    cboBanks.Refresh();
                    OutClearingFile.ClearingUniversalMethod(usrinfo, "p_GetPostedACPItems", out StatusData, BRBase.BRModule.GenerateClearingFile, GetConnection(), new object[] { "BrDataSet" }, new object[] { "OC", "00", null });
                    cboBanks.Items.Add("ALL");
                    foreach (DataRow dr in StatusData.Tables[0].Rows)
                    {
                        cboBanks.Items.Add(dr["BankID"].ToString());
                    }
                    cboBanks.SelectedIndex = 0;
                    Application.DoEvents();
                    cboBanks.Refresh();
                }
                catch
                {

                }
            }
            else if (Country.ToUpper() == "TZ")
            {
                lblClearingPortalDate.Text = "BOT Clearing Date";
                foreach (TZFileTypes n in Enum.GetValues(typeof(TZFileTypes)))
                {
                    cboFileType.Items.Add(n);
                }
                cboFileType.SelectedIndex = 0;
                Application.DoEvents();
                cboFileType.Refresh();
                foreach (Session n in Enum.GetValues(typeof(Session)))
                {
                    cboSession.Items.Add(n);
                }
                cboSession.SelectedIndex = 0;
                Application.DoEvents();
                cboSession.Refresh();
                foreach (TZCurrency n in Enum.GetValues(typeof(TZCurrency)))
                {
                    cboCurrency.Items.Add(n);
                }
                cboCurrency.SelectedIndex = 0;
                Application.DoEvents();
                cboCurrency.Refresh();

                BRDataSet StatusData = new BRDataSet();
                try
                {
                    cboBanks.Items.Clear();
                    Application.DoEvents();
                    cboBanks.Refresh();
                    OutClearingFile.ClearingUniversalMethod(usrinfo, "p_GetPostedACPItems", out StatusData, BRBase.BRModule.GenerateClearingFile, GetConnection(), new object[] { "BrDataSet" }, new object[] { "OC", "00", null });
                    cboBanks.Items.Add("ALL");
                    foreach (DataRow dr in StatusData.Tables[0].Rows)
                    {
                        cboBanks.Items.Add(dr["BankID"].ToString());
                    }
                    cboBanks.SelectedIndex = 0;
                    Application.DoEvents();
                    cboBanks.Refresh();
                }
                catch
                {

                }
            }


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void cboFileType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboBanks.Items.Clear();
            Application.DoEvents();
            BRDataSet StatusData = new BRDataSet();
            string fType = string.Empty;
            string RType = "00";
            string VType = "00";

            switch (cboFileType.SelectedIndex)
            {

                case 0:// "Out_Cheques":
                case 1:// "Out_Rejected_Cheques":
                case 2: // "Presented_Direct_Credit":
                case 3: // "Rejected_Direct_Credit":
                case 4:// "Presented_DirectDebit":
                case 5:// "Rejected_DirectDebit":
                    try
                    {
                        switch (cboFileType.SelectedIndex)
                        {
                            case 0:
                                fType = "OC";
                                break;
                            case 1:
                                fType = "OC";
                                RType = "99";
                                break;
                            case 2:
                                fType = "OD";
                                break;
                            case 3:
                                fType = "OD";
                                RType = "99";
                                break;
                            case 4:
                                fType = "OC";
                                VType = "40";
                                break;
                            case 5:
                                fType = "OC";
                                RType = "99";
                                VType = "40";
                                break;
                        }
                        OutClearingFile.ClearingUniversalMethod(usrinfo, "p_GetPostedACPItems", out StatusData, BRBase.BRModule.GenerateClearingFile, GetConnection(), new object[] { "BrDataSet" }, new object[] { fType, RType, VType });
                        cboBanks.Items.Clear();
                        cboBanks.Items.Add("ALL");
                        foreach (DataRow dr in StatusData.Tables[0].Rows)
                        {
                            cboBanks.Items.Add(dr["BankID"].ToString());
                        }
                        cboBanks.SelectedIndex = 0;
                        Application.DoEvents();
                        cboBanks.Refresh();
                    }
                    catch
                    {

                    }
                    break;
            }

            btnView_Click(sender, e);
            //dgvClearingStatus.Refresh();
            //Application.DoEvents();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnView_Click(object sender, EventArgs e)
        {
            lblAmount.Text = "0.00";
            lblCurrencyID.Text = "";
            dgvClearingStatus = new DataGridView();
            dgvClearingStatus.Rows.Clear();
            groupBox2.Controls.Clear();
            int x = dgvClearingStatus.Rows.Count;
            BRDataSet StatusData = new BRDataSet();
            string SelItem = string.Empty;
            int index = 0;
            DataGridView dgv = new DataGridView();
            DataGridViewCheckBoxColumn dgvChk = new DataGridViewCheckBoxColumn();
            dgvChk.ValueType = typeof(bool);
            dgvChk.Name = "ChkSelect";
            dgvChk.HeaderText = "Select";
            dgvChk.ToolTipText = "Select the ones to generate";
            dgvChk.Visible = true;
            dgvChk.Width = 50;
            dgvChk.SortMode = DataGridViewColumnSortMode.Automatic;
            dgvChk.Resizable = DataGridViewTriState.False;
            dgvChk.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
            dgvChk.HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomCenter;
            dgvChk.CellTemplate.Style.BackColor = Color.Transparent;
            dgv.Columns.Add(dgvChk);

            DataGridViewTextBoxColumn dgvNo = new DataGridViewTextBoxColumn();
            dgvNo.Name = "colRowNo";
            dgvNo.HeaderText = "No.";
            dgvNo.Width = 30;
            dgvNo.ReadOnly = true;
            dgv.Columns.Add(dgvNo);


            switch (cboFileType.SelectedIndex)
            {
                case 0:
                    SelItem = "POC";
                    break;
                case 1:
                    SelItem = "ROC";
                    break;
                case 2:
                    SelItem = "POD";
                    break;
                case 3:
                    SelItem = "RCT";
                    break;
                case 4:
                    SelItem = "PDOD";
                    break;
                case 5:
                    SelItem = "RDOD";
                    break;
                case 6:
                    SelItem = "103";
                    break;
                case 7:
                    SelItem = "202";
                    break;
                case 8:
                    SelItem = "920";
                    break;
                case 9:
                    SelItem = "999";
                    break;
            }
            try
            {
                decimal TotalAmount = 0;
                OutClearingFile.ClearingUniversalMethod(usrinfo, "p_ACHTrxStatus", out StatusData, BRBase.BRModule.GenerateClearingFile, GetConnection(), new object[] { "BrDataSet" }, new object[] { SelItem, cboCurrency.SelectedItem, cboSession.SelectedItem, cboBanks.SelectedItem });
                dgv.Rows.Clear();
                Application.DoEvents();
                if (StatusData.Tables[0].Rows.Count == 0)
                {
                    return;
                }
                foreach (DataRow dr in StatusData.Tables[0].Rows)
                {
                    TotalAmount = TotalAmount + Convert.ToDecimal(dr["Amount"]);
                    try
                    {
                        try
                        {
                            index = dgv.Rows.Add();
                        }
                        catch
                        {
                            index = 0;
                        }
                        dgv.Rows[index].Cells[dgvChk.Name].Value = false;
                        dgv.Rows[index].Cells[dgvNo.Name].Value = index + 1;

                        foreach (DataColumn col in StatusData.Tables[0].Columns)
                        {
                            if (!dgv.Columns.Contains(col.ToString().ToUpper()))
                            {
                                dgv.Columns.Add(col.ToString(), col.ToString().ToUpper());
                                switch (col.ToString())
                                {
                                    case "TrxRowID":
                                        dgv.Columns[col.ToString()].Visible = false;
                                        break;
                                }
                                dgv.Columns[col.ToString()].ReadOnly = true;

                            }
                            switch (col.ToString())
                            {
                                case "Amount":
                                    var Amount = Convert.ToDecimal(dr["Amount"]);
                                    var strAmount = String.Format("{0:n}", Amount);
                                    dgv.Rows[index].Cells[col.ToString()].Value = strAmount;
                                    break;
                                case "ValueDate":
                                    try
                                    {
                                        DateTime ValueDate = DateTime.Parse(dr["ValueDate"].ToString(), CultureInfo.InvariantCulture);
                                        string sValueDate = ValueDate.ToString("dd MMM yyyy", CultureInfo.InvariantCulture);
                                        dgv.Rows[index].Cells[col.ToString()].Value = sValueDate;
                                    }
                                    catch
                                    {
                                        dgv.Rows[index].Cells[col.ToString()].Value = dr["ValueDate"].ToString();
                                    }


                                    break;
                                case "Date":
                                    try
                                    {
                                        DateTime PostDate = DateTime.Parse(dr["Date"].ToString(), CultureInfo.InvariantCulture);
                                        string sPostDate = PostDate.ToString("dd MMM yyyy", CultureInfo.InvariantCulture);
                                        dgv.Rows[index].Cells[col.ToString()].Value = sPostDate;
                                    }
                                    catch
                                    {
                                        dgv.Rows[index].Cells[col.ToString()].Value = dr["Date"];
                                    }

                                    break;
                                default:
                                    dgv.Rows[index].Cells[col.ToString()].Value = dr[col.ToString()];
                                    break;
                            }
                            Application.DoEvents();
                        }
                    }
                    catch { }
                    lblAmount.Text = String.Format("{0:n}", TotalAmount);
                    try
                    {
                        //lblCurrencyID.Text = cboCurrency.SelectedItem.ToString();
                    }
                    catch
                    {
                        //cboCurrency.SelectedIndex = 0;
                        //lblCurrencyID.Text = cboCurrency.SelectedItem.ToString();
                    }
                }
                //dgvClearingStatus.Refresh();

                dgvClearingStatus = dgv;
                Application.DoEvents();
                int p = dgvClearingStatus.Rows.Count;
                dgvClearingStatus.AllowUserToAddRows = false;
                dgvClearingStatus.AllowUserToDeleteRows = false;
                dgvClearingStatus.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvClearingStatus_CellContentClick);
                dgvClearingStatus.Columns["Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomRight;
                dgvClearingStatus.Location = new Point(9, 15);
                dgvClearingStatus.Size = new Size(1187, 418);
                dgvClearingStatus.BackgroundColor = Color.LightSteelBlue;
                dgvClearingStatus.AlternatingRowsDefaultCellStyle.BackColor = Color.LightSkyBlue;
                dgvClearingStatus.Visible = true;
                Application.DoEvents();
                groupBox2.Controls.Add(dgvClearingStatus);
                dgvClearingStatus.Refresh();
                Application.DoEvents();
            }
            catch
            {

            }

        }
        static private IDbConnection GetConnection()
        {
            IDbConnection connection = null;
            string strSystem = ConfigurationManager.AppSettings["strSystem"].Trim().ToUpper();
            if (String.IsNullOrEmpty(Modscan.DBServerName))
            {
                GetConfigConnDetails();
            }
            //string strDBServerName = ConfigurationManager.AppSettings["strDBServerName"];
            //string strDatabaseName = ConfigurationManager.AppSettings["strDatabaseName"];
            //string strBRUserName = ConfigurationManager.AppSettings["strBRUserName"];
            //string strBRUserPassword = ConfigurationManager.AppSettings["strBRUserPassword"];
            //string strSYSADMIN1UserName = ConfigurationManager.AppSettings["strSYSADMIN1UserName"];
            //string strSYSADMIN1Password = ConfigurationManager.AppSettings["strSYSADMIN1Password"];
            try
            {
                connection = BRAccess.BRConnection(Modscan.BRUserName, Modscan.DBPassword, Modscan.DatabaseName, Modscan.DBServerName);
                if (connection.State != ConnectionState.Open)
                    connection.Open();
            }
            catch
            {

            }
            return connection;
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            cboFileType.SelectedIndex = 0;
            cboSession.SelectedIndex = 0;
            cboCurrency.SelectedIndex = 0;
            cboBanks.SelectedIndex = 0;
            dgvClearingStatus1.Rows.Clear();
            lblAmount.Text = "0.00";
            lblCurrencyID.Text = "";

        }

        private void dgvClearingStatus_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (ArGen == null)
                {
                    ArGen = new List<string>();
                }
                List<string> list = new List<string>();
                dgvClearingStatus1.Refresh();
                Application.DoEvents();
                var selectedRows = dgvClearingStatus1.SelectedRows.Cast<DataGridViewRow>();

                // Check the checkbox value in the selected rows
                //foreach (var row in selectedRows)
                //{
                //    bool isChecked = Convert.ToBoolean(row.Cells["colChkbox"].Value);
                //    int id = Convert.ToInt32(row.Cells["colTrxRowID"].Value);
                //    //Console.WriteLine("Row: ID = {0}, Selected = {1}", id, isChecked);
                //}

                //if ((bool)dgvClearingStatus.Rows[e.RowIndex].Cells["colChkbox"].Value == false )
                //{
                //    list = GetSelectedUniqueIDs(dgvClearingStatus);
                //    ArGen.AddRange(list.Where(item => !ArGen.Contains(item)));
                //}
                //else
                //{
                //    list = GetSelectedUniqueIDs(dgvClearingStatus);
                //    ArGen.RemoveAll(item => !list.Contains(item));
                //}
                //ArGen = dgvClearingStatus.Rows
                //        .Cast<DataGridViewRow>()
                //        .Where(row => (bool?)row.Cells["colChkbox"].Selected == true)
                //        .Select(row => row.Cells["colTrxRowID"].Value.ToString())
                //        .ToList<string>();
                // Get selected unique IDs from DataGridView


                // Remove items from ListA that are not present in ListB
                //ArGen.RemoveAll(item => !list.Contains(item));
                // Remove deselected unique IDs from the list
                //RemoveDeselectedUniqueIDs(dgvClearingStatus, ArGen);

                dgvClearingStatus1.Refresh();
                Application.DoEvents();

            }
            catch { }
        }
        private protected List<string> GetSelectedUniqueIDs(DataGridView dgv)
        {
            List<string> selectedUniqueIDs = dgv.Rows.Cast<DataGridViewRow>()
                .Where(row => (bool?)row.Cells["colChkbox"].Selected == true)
                .Select(row => row.Cells["colTrxRowID"].Value.ToString())
                .ToList<string>();

            return selectedUniqueIDs;
        }

        private protected void RemoveDeselectedUniqueIDs(DataGridView dgv, List<string> uniqueIDs)
        {
            uniqueIDs.RemoveAll(uniqueID => dgv.SelectedRows.Cast<DataGridViewRow>()
                .Any(row => row.Cells["colTrxRowID"].Value?.ToString() == uniqueID));
        }
        private void btnRefreshStatus_Click(object sender, EventArgs e)
        {
            RTGSResponses = Path.Combine(ResponsesPath, "RTGSREPLIES");
            string[] Filter = new string[] { "*.*" };
            DirectoryInfo di = new DirectoryInfo(RTGSResponses);
            List<string> li = new List<string>();
            li = new List<string>();
            foreach (string f in Filter)
            {
                try
                {
                    FileInfo[] fi = di.GetFiles(f);
                    foreach (FileInfo inf in fi)
                    {
                        ImportRtgsResponses(inf.FullName);
                    }
                }
                catch { }

            }
            var ClearingResponses = Path.Combine(ResponsesPath, "temp");
            string[] RtgsFilter = new string[] { "*.V", "*.R" };
            DirectoryInfo Rtgsdi = new DirectoryInfo(ClearingResponses);
            List<string> Rtgsli = new List<string>();
            Rtgsli = new List<string>();
            foreach (string f in RtgsFilter)
            {
                try
                {
                    FileInfo[] fi = Rtgsdi.GetFiles(f);
                    foreach (FileInfo inf in fi)
                    {
                        ImportRtgsResponses(inf.FullName);
                    }
                }
                catch { }
            }
        }
        private protected Common.Response ImportResponses(string sFile)
        {
            BRDataSet bRData = new BRDataSet();
            string destination = Path.Combine(ArchivePath, "ACH_reponses\\In\\" + DateTime.Now.ToString("yyMMdd"));
            var res = new Common.Response();
            var doc = new rs.Document();
            bool Response = false;

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
            if (res.Header.OrgMsgId.ToString() != "")
            {

                try
                {
                    if (res.Rejections.Count > 0)
                    {
                        foreach (var err in res.Rejections)
                        {
                            OutClearingFile.ClearingUniversalMethod(usrinfo, "p_UpdateACHstatus", out bRData, BRBase.BRModule.GenerateClearingFile, GetConnection(), new object[] { "BrDataSet" }, new object[] { res.Header.OrgMsgId.ToString(), err.OrgTxnId.ToString(), err.ReturnCode.ToString(), string.Empty });
                        }
                    }
                    else
                    {
                        OutClearingFile.ClearingUniversalMethod(usrinfo, "p_UpdateACHstatus", out bRData, BRBase.BRModule.GenerateClearingFile, GetConnection(), new object[] { "BrDataSet" }, new object[] { res.Header.OrgMsgId.ToString(), string.Empty, res.Header.StatusType.ToString(), string.Empty });
                    }

                    if (bRData.Tables.Count > 0)
                    {
                        if (bRData.Tables[0].Rows.Count > 0)
                        {
                            Response = Convert.ToBoolean(bRData.Tables[0].Rows[0]["RESPONSE"]);
                        }
                    }
                    else
                    {
                        Response = false;
                    }
                }
                catch { }
                if (Directory.Exists(destination) == false)
                {
                    Directory.CreateDirectory(destination);
                }
                var destinationfile = Path.Combine(destination, Path.GetFileName(sFile));
                if (Response == true)
                {
                    if (File.Exists(destinationfile) == false)
                    {
                        File.Copy(sFile, destinationfile);
                        File.Delete(sFile);
                    }

                }
            }

            return res;
        }
        private protected AchRtgs ImportRtgsResponses(string sFile)
        {
            var RejCode = string.Empty;
            BRDataSet bRData = new BRDataSet();
            var RejRemarks = string.Empty;
            bool Response = false;
            string destination = Path.Combine(ArchivePath, "Rtgs_reponses\\In\\" + DateTime.Now.ToString("yyMMdd"));
            var rt = new AchRtgs();
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
                    if (!content.Contains(":16R:RSN"))
                        rt.Status = true;
                    break;
                case "298":
                    rt.Trans_Ref = l.First(p => p.StartsWith(":21:")).Remove(0, 4);
                    rt.RtgsType = RtgsType.Mt298;
                    if (!content.Contains(":16R:RSN"))
                        rt.Status = true;
                    break;
            }
            if (!rt.Status)
            {
                RejCode = l.First(p => p.StartsWith(":M01:")).Remove(0, 5);
                RejRemarks = String.Concat(l.First(p => p.StartsWith(":M02:")).Remove(0, 5), l.Last(p => p.StartsWith(":M02:")).Remove(0, 5));
            }
            else
            {
                RejCode = "O";
                RejRemarks = "Successful";
            }
            try
            {
                OutClearingFile.ClearingUniversalMethod(usrinfo, "p_UpdateRtgstatus", out bRData, BRBase.BRModule.GenerateClearingFile, GetConnection(), new object[] { "BrDataSet" }, new object[] { rt.Trans_Ref, RejCode, RejRemarks });
                if (bRData.Tables.Count > 0)
                {
                    if (bRData.Tables[0].Rows.Count > 0)
                    {
                        Response = Convert.ToBoolean(bRData.Tables[0].Rows[0]["RESPONSE"]);
                    }
                }
                else
                {
                    Response = false;
                }

                if (Directory.Exists(destination) == false)
                {
                    Directory.CreateDirectory(destination);
                }
                var destinationfile = Path.Combine(destination, Path.GetFileName(sFile));
                if (Response == true)
                {
                    if (File.Exists(destinationfile) == false)
                    {
                        File.Copy(sFile, destinationfile);
                        File.Delete(sFile);
                    }

                }
            }
            catch { }


            return rt;
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

        private void btnCreate_Click(object sender, EventArgs e)
        {
            string TokenName = "";
            string TokenPassword = "";
            //ArGen = new List<string>();
            SelectedItems();
            Modscan.cToDate = dtTo.Value.ToString();
            Modscan.cFromDate = dtFrom.Value.ToString();
            Modscan.FDATE = dtpATSBusinessDate.Value;
            Modscan.cWORKING_DATE = dtFrom.Value.ToString();
            //List<DataGridViewRow> rows_with_checked_column = new List<DataGridViewRow>();
            foreach (DataGridViewRow row in dgvClearingStatus.Rows)
            {
                if (Convert.ToBoolean(row.Cells["ChkSelect"].Value) == true)
                {
                    string trxRowID = row.Cells["TrxRowID"].Value.ToString();
                    if (!ArGen.Contains(trxRowID))
                    {
                        ArGen.Add(row.Cells["TrxRowID"].Value.ToString());
                    }
                    //rows_with_checked_column.Add(row);
                }
            }
            //var ArGenk = dgvClearingStatus.Rows
            //        .Cast<DataGridViewRow>()
            //        .Where(row => (bool?)row.Cells["ChkSelect"].Selected == true)
            //        .Select(row => row.Cells["TrxRowID"].Value.ToString())
            //        .ToList<string>();
            if (ArGen.Count == 0)
            {
                lblmessage.Text = "No selected items to generate";
                return;
            }
            if (Country.ToUpper() == "ET")
            {
                switch (cboFileType.SelectedIndex)
                {
                    case 0: //Cheques
                        if (cboSession.SelectedIndex == 0)
                        {
                            lblmessage.Text = "Generating Local Cheques..";
                            lblmessage.Update();
                            BRETHClass.GenerateETH(TokenName, TokenPassword, FileType.Cheques, 0, false, ChequeFormat.XMLPackages, "01", "", "", ArGen);
                            lblmessage.Text = "Done Generating Local Cheques..";
                            lblmessage.Update();
                        }
                        else
                        {

                        }
                        break;
                    case 1: //Rejected Cheques
                        lblmessage.Text = "Generating Rejected Local Cheques..";
                        lblmessage.Update();
                        BRETHClass.GenerateETH(TokenName, TokenPassword, FileType.ChequeReturn, 0, false, ChequeFormat.XMLPackages, "01", "", "", ArGen);
                        lblmessage.Text = "Done Generating Rejected Local Cheques..";
                        lblmessage.Update();
                        break;
                    case 2://EFTs
                        if (cboSession.SelectedIndex == 0)
                        {
                            lblmessage.Text = "Generating EFTs....";
                            lblmessage.Update();
                            BRETHClass.GenerateETH(TokenName, TokenPassword, FileType.Efts, 0, false, ChequeFormat.XMLPackages, "01", "", "", ArGen);
                            lblmessage.Text = "Done Generating EFTs....";
                            lblmessage.Update();
                        }
                        else
                        {

                        }
                        break;
                    case 3://Rejected CTs
                        if (cboSession.SelectedIndex == 0)
                        {
                            lblmessage.Text = "Generating cancelled Credit Transfer....";
                            lblmessage.Update();
                            BRETHClass.GenerateETH(TokenName, TokenPassword, FileType.EftReturn, 0, false, ChequeFormat.XMLPackages, "01", "", "", ArGen);
                            lblmessage.Text = "Done Generating cancelled Credit Transfer....";
                            lblmessage.Update();
                        }

                        else
                        {

                        }
                        break;
                    case 4://DD
                        if (cboSession.SelectedIndex == 0)
                        {
                            lblmessage.Text = "Generating DDs....";
                            lblmessage.Update();
                            BRETHClass.GenerateETH(TokenName, TokenPassword, FileType.DD, 0, false, ChequeFormat.XMLPackages, "01", "", "", ArGen);
                            lblmessage.Text = "Done Generating DDs....";
                            lblmessage.Update();
                        }
                        else
                        {

                        }
                        break;
                    case 5://DD Reject
                        if (cboSession.SelectedIndex == 0)
                        {
                            lblmessage.Text = "Generating Rejected DDs....";
                            lblmessage.Update();
                            BRETHClass.GenerateETH(TokenName, TokenPassword, FileType.DDReturn, 0, false, ChequeFormat.XMLPackages, "01", "", "", ArGen);
                            lblmessage.Text = "Done Generating Rejected DDs....";
                            lblmessage.Update();
                        }
                        else
                        {
                        }
                        break;
                    case 6:
                        lblmessage.Text = "Generating RTGS MT103 Messages..";
                        lblmessage.Update();
                        BRETHClass.GenerateETH(TokenName, TokenPassword, FileType.RTGS103, 0, false, ChequeFormat.XMLPackages, "01", "", "", ArGen);
                        lblmessage.Text = "Done Generating RTGS MT103 Messages..";
                        lblmessage.Update();
                        break;
                    case 7: //RTGS202
                        lblmessage.Text = "Generating RTGS MT202 Messages..";
                        lblmessage.Update();
                        BRETHClass.GenerateETH(TokenName, TokenPassword, FileType.RTGS202, 0, false, ChequeFormat.XMLPackages, "01", "", "", ArGen);
                        lblmessage.Text = "Done Generating RTGS MT202 Messages..";
                        lblmessage.Update();
                        break;
                    case 8://RTGS920
                        lblmessage.Text = "Generating RTGS MT920 Messages..";
                        lblmessage.Update();
                        BRETHClass.GenerateETH(TokenName, TokenPassword, FileType.RTGS920, 0, false, ChequeFormat.XMLPackages, "01", "", "", ArGen);
                        lblmessage.Text = "Done Generating RTGS MT920 Messages..";
                        lblmessage.Update();
                        break;
                    case 9: //RTGS999
                        lblmessage.Text = "Generating RTGS MT999 Messages..";
                        lblmessage.Update();
                        BRETHClass.GenerateETH(TokenName, TokenPassword, FileType.RTGS999, 0, false, ChequeFormat.XMLPackages, "01", "", "", ArGen);
                        lblmessage.Text = "Done Generating RTGS MT999 Messages..";
                        lblmessage.Update();
                        break;
                }
            }
            else if (Country.ToUpper() == "TZ")
            {
                switch (cboFileType.SelectedIndex)
                {
                    case 0: //Cheques
                        if (cboSession.SelectedIndex == 0)
                        {
                            lblmessage.Text = "Generating Local Cheques..";
                            lblmessage.Update();
                            var result = BRTZClass.GenerateTZ(TokenName, TokenPassword, BrClearing.Common.TZ.FileType.Cheques, cboCurrency.SelectedIndex, false, BrClearing.Common.TZ.ChequeFormat.XMLPackages, "01", "", "", ArGen);
                            DisplayGenerationResult(result);
                        }
                        else
                        {

                        }
                        break;
                    case 1: //Rejected Cheques
                        lblmessage.Text = "Generating Rejected Local Cheques..";
                        lblmessage.Update();
                        var result1 = BRTZClass.GenerateTZ(TokenName, TokenPassword, BrClearing.Common.TZ.FileType.ChequeReturn, cboCurrency.SelectedIndex, false, BrClearing.Common.TZ.ChequeFormat.XMLPackages, "01", "", "", ArGen);
                        DisplayGenerationResult(result1);
                        break;
                    case 2://EFTs
                        if (cboSession.SelectedIndex == 0)
                        {
                            lblmessage.Text = "Generating EFTs....";
                            lblmessage.Update();
                            var result2 = BRTZClass.GenerateTZ(TokenName, TokenPassword, BrClearing.Common.TZ.FileType.Efts, cboCurrency.SelectedIndex, false, BrClearing.Common.TZ.ChequeFormat.XMLPackages, "01", "", "", ArGen);
                            DisplayGenerationResult(result2);
                        }
                        else
                        {

                        }
                        break;
                    case 3://Rejected CTs
                        if (cboSession.SelectedIndex == 0)
                        {
                            lblmessage.Text = "Generating cancelled Credit Transfer....";
                            lblmessage.Update();
                            var result3 = BRTZClass.GenerateTZ(TokenName, TokenPassword, BrClearing.Common.TZ.FileType.EftReturn, cboCurrency.SelectedIndex, false, BrClearing.Common.TZ.ChequeFormat.XMLPackages, "01", "", "", ArGen);
                            DisplayGenerationResult(result3);
                        }

                        else
                        {

                        }
                        break;
                    case 4://DD
                        if (cboSession.SelectedIndex == 0)
                        {
                            lblmessage.Text = "Generating DDs....";
                            lblmessage.Update();
                            var result4 = BRTZClass.GenerateTZ(TokenName, TokenPassword, BrClearing.Common.TZ.FileType.DD, cboCurrency.SelectedIndex, false, BrClearing.Common.TZ.ChequeFormat.XMLPackages, "01", "", "", ArGen);
                            DisplayGenerationResult(result4);
                        }
                        else
                        {

                        }
                        break;
                    case 5://DD Reject
                        if (cboSession.SelectedIndex == 0)
                        {
                            lblmessage.Text = "Generating Rejected DDs....";
                            lblmessage.Update();
                            var result5 = BRTZClass.GenerateTZ(TokenName, TokenPassword, BrClearing.Common.TZ.FileType.DDReturn, cboCurrency.SelectedIndex, false, BrClearing.Common.TZ.ChequeFormat.XMLPackages, "01", "", "", ArGen);
                            DisplayGenerationResult(result5);
                        }
                        else
                        {
                        }
                        break;
                }
            }
            ArGen.Clear();
            Application.DoEvents();
            btnView_Click(sender, e);
        }

        private void cboBanks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboBanks != null)
            {
                SelectedItems();
                btnView_Click(sender, e);
            }
        }

        private void cboSession_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (MessageBox.Show("Note items are created session wise, selected item in other session will be deselected. Are your you want to continue?") == DialogResult)
            //{
            //if (ArGen == null)
            //{
            //    ArGen = new List<string>();
            //}
            SelectedItems();
            btnView_Click(sender, e);
            //}
        }

        private void cboCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedItems();
            btnView_Click(sender, e);
        }

        private void dgvClearingStatus_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (ArGen == null)
            {
                ArGen = new List<string>();
            }
            //foreach (DataGridViewRow row in dgvClearingStatus.Rows)
            //{
            //    if (Convert.ToBoolean(row.Cells["ChkSelect"].Value) == true)
            //    {
            //        ArGen.Add(row.Cells["TrxRowID"].Value.ToString());
            //    }
            //}
            ////bool isChecked = Convert.ToBoolean(e.RowIndex.Cells["colChkbox"].Value);
            //bool isChecked = Convert.ToBoolean(dgvClearingStatus1.Rows[e.RowIndex].Cells["colChkbox"].Value);
        }

        private void SelectedItems()
        {
            if (ArGen == null)
            {
                ArGen = new List<string>();
            }
            if (dgvClearingStatus != null)
            {
                foreach (DataGridViewRow row in dgvClearingStatus.Rows)
                {
                    if (Convert.ToBoolean(row.Cells["ChkSelect"].Value) == true)
                    {
                        string trxRowID = row.Cells["TrxRowID"].Value.ToString();
                        if (!ArGen.Contains(trxRowID))
                        {
                            ArGen.Add(row.Cells["TrxRowID"].Value.ToString());
                        }
                    }
                }
            }
        }

        private void Regenerate_Click(object sender, EventArgs e)
        {

        }

        private void btnFetch_Click(object sender, EventArgs e)
        {

        }

        private void DisplayGenerationResult(GenerationResult result)
        {
            if (result == null) return;

            string summary = result.Message;
            if (result.Details != null && result.Details.Count > 0)
            {
                summary += "\n\nDetails:\n" + string.Join("\n", result.Details);
            }

            MessageBoxIcon icon = MessageBoxIcon.Information;
            switch (result.MessageType)
            {
                case MessageType.Error:
                    icon = MessageBoxIcon.Error;
                    break;
                case MessageType.Warning:
                    icon = MessageBoxIcon.Warning;
                    break;
                case MessageType.Success:
                    icon = MessageBoxIcon.Information;
                    break;
                case MessageType.Info:
                    icon = MessageBoxIcon.Information;
                    break;
            }

            MessageBox.Show(summary, "File Generation Result", MessageBoxButtons.OK, icon);
            lblmessage.Text = result.Message;
            lblmessage.Update();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
