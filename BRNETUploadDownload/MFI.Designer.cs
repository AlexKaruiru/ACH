namespace BRNETMFIUploadDownload
{
    partial class BRNetUploadDownLoadUtilityMFI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BRNetUploadDownLoadUtilityMFI));
            this.btnGenarate = new System.Windows.Forms.Button();
            this.tcrBRFileDownloadUpload = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.rbNonParticipating = new System.Windows.Forms.RadioButton();
            this.rbParticipating = new System.Windows.Forms.RadioButton();
            this.btnImport = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtFolderName = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.dtFileDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.cboCurrencyType = new System.Windows.Forms.ComboBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.rbNormalClearing = new System.Windows.Forms.RadioButton();
            this.rbSession2 = new System.Windows.Forms.RadioButton();
            this.rbfirstsession = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cboClient = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rbAllowrgBoth = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.rbAllowrg2ndsession = new System.Windows.Forms.RadioButton();
            this.rbAllowrg1st = new System.Windows.Forms.RadioButton();
            this.btnAllowPosting = new System.Windows.Forms.Button();
            this.btnUnpaidLockPosting = new System.Windows.Forms.Button();
            this.btnUnlockUnpaidPosting = new System.Windows.Forms.Button();
            this.dtToDate = new System.Windows.Forms.DateTimePicker();
            this.dtFromDate = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtFileGenerationPath = new System.Windows.Forms.TextBox();
            this.btnGenerationBrowse = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.cboCurrType = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dtGenerationFileDate = new System.Windows.Forms.DateTimePicker();
            this.btnExit = new System.Windows.Forms.Button();
            this.prbFileProgress = new System.Windows.Forms.ProgressBar();
            this.lblmessage = new System.Windows.Forms.Label();
            this.colTrxDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colChequeID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAccount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSlno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOurBranchID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvUnsupervised = new System.Windows.Forms.DataGridView();
            this.tcrBRFileDownloadUpload.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUnsupervised)).BeginInit();
            this.SuspendLayout();
            // 
            // btnGenarate
            // 
            this.btnGenarate.Location = new System.Drawing.Point(327, 124);
            this.btnGenarate.Name = "btnGenarate";
            this.btnGenarate.Size = new System.Drawing.Size(100, 29);
            this.btnGenarate.TabIndex = 0;
            this.btnGenarate.Text = "Generate";
            this.btnGenarate.UseVisualStyleBackColor = true;
            this.btnGenarate.Click += new System.EventHandler(this.btnGenarate_Click);
            // 
            // tcrBRFileDownloadUpload
            // 
            this.tcrBRFileDownloadUpload.Controls.Add(this.tabPage1);
            this.tcrBRFileDownloadUpload.Controls.Add(this.tabPage2);
            this.tcrBRFileDownloadUpload.Location = new System.Drawing.Point(4, 1);
            this.tcrBRFileDownloadUpload.Name = "tcrBRFileDownloadUpload";
            this.tcrBRFileDownloadUpload.SelectedIndex = 0;
            this.tcrBRFileDownloadUpload.Size = new System.Drawing.Size(443, 185);
            this.tcrBRFileDownloadUpload.TabIndex = 0;
            this.tcrBRFileDownloadUpload.SelectedIndexChanged += new System.EventHandler(this.tcrBRFileDownloadUpload_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.BackgroundImage = global::BRNETUploadDownload.Properties.Resources.log_img6;
            this.tabPage1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tabPage1.Controls.Add(this.rbNonParticipating);
            this.tabPage1.Controls.Add(this.rbParticipating);
            this.tabPage1.Controls.Add(this.btnImport);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(435, 159);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "File Importation";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // rbNonParticipating
            // 
            this.rbNonParticipating.AutoSize = true;
            this.rbNonParticipating.Location = new System.Drawing.Point(7, 156);
            this.rbNonParticipating.Name = "rbNonParticipating";
            this.rbNonParticipating.Size = new System.Drawing.Size(137, 17);
            this.rbNonParticipating.TabIndex = 9;
            this.rbNonParticipating.Text = "Non Particapting Banks";
            this.rbNonParticipating.UseVisualStyleBackColor = true;
            this.rbNonParticipating.Visible = false;
            // 
            // rbParticipating
            // 
            this.rbParticipating.AutoSize = true;
            this.rbParticipating.Checked = true;
            this.rbParticipating.Location = new System.Drawing.Point(7, 122);
            this.rbParticipating.Name = "rbParticipating";
            this.rbParticipating.Size = new System.Drawing.Size(116, 17);
            this.rbParticipating.TabIndex = 8;
            this.rbParticipating.TabStop = true;
            this.rbParticipating.Text = "Participating Banks\r\n";
            this.rbParticipating.UseVisualStyleBackColor = true;
            this.rbParticipating.Visible = false;
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(325, 125);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(101, 30);
            this.btnImport.TabIndex = 0;
            this.btnImport.Text = "Import ";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click_1);
            this.btnImport.LostFocus += new System.EventHandler(this.btnImport_LostFocus);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtFolderName);
            this.groupBox1.Controls.Add(this.btnBrowse);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.dtFileDate);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cboCurrencyType);
            this.groupBox1.Location = new System.Drawing.Point(6, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(420, 104);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "File Path";
            // 
            // txtFolderName
            // 
            this.txtFolderName.BackColor = System.Drawing.SystemColors.Info;
            this.txtFolderName.Location = new System.Drawing.Point(87, 19);
            this.txtFolderName.Name = "txtFolderName";
            this.txtFolderName.Size = new System.Drawing.Size(229, 20);
            this.txtFolderName.TabIndex = 0;
            this.txtFolderName.TextChanged += new System.EventHandler(this.txtFolderName_TextChanged);
            this.txtFolderName.LostFocus += new System.EventHandler(this.txtFolderName_LostFocus);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(322, 19);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(92, 21);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "Browse Folder";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "File Date";
            // 
            // dtFileDate
            // 
            this.dtFileDate.Location = new System.Drawing.Point(88, 72);
            this.dtFileDate.Name = "dtFileDate";
            this.dtFileDate.Size = new System.Drawing.Size(228, 20);
            this.dtFileDate.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Currency Type";
            // 
            // cboCurrencyType
            // 
            this.cboCurrencyType.FormattingEnabled = true;
            this.cboCurrencyType.Items.AddRange(new object[] {
            "Local",
            "Foreign"});
            this.cboCurrencyType.Location = new System.Drawing.Point(88, 45);
            this.cboCurrencyType.Name = "cboCurrencyType";
            this.cboCurrencyType.Size = new System.Drawing.Size(228, 21);
            this.cboCurrencyType.TabIndex = 2;
            this.cboCurrencyType.SelectedIndexChanged += new System.EventHandler(this.cboCurrencyType_MouseClick);
            this.cboCurrencyType.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtFileGenerationPath_LostFocus);
            // 
            // tabPage2
            // 
            this.tabPage2.BackgroundImage = global::BRNETUploadDownload.Properties.Resources.log_img6;
            this.tabPage2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tabPage2.Controls.Add(this.rbNormalClearing);
            this.tabPage2.Controls.Add(this.rbSession2);
            this.tabPage2.Controls.Add(this.rbfirstsession);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.btnGenarate);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.dtGenerationFileDate);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(435, 159);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "File Generation";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // rbNormalClearing
            // 
            this.rbNormalClearing.AutoSize = true;
            this.rbNormalClearing.Location = new System.Drawing.Point(174, 180);
            this.rbNormalClearing.Name = "rbNormalClearing";
            this.rbNormalClearing.Size = new System.Drawing.Size(99, 17);
            this.rbNormalClearing.TabIndex = 14;
            this.rbNormalClearing.Text = "Normal Clearing";
            this.rbNormalClearing.UseVisualStyleBackColor = true;
            this.rbNormalClearing.Visible = false;
            // 
            // rbSession2
            // 
            this.rbSession2.AutoSize = true;
            this.rbSession2.Location = new System.Drawing.Point(174, 235);
            this.rbSession2.Name = "rbSession2";
            this.rbSession2.Size = new System.Drawing.Size(83, 17);
            this.rbSession2.TabIndex = 11;
            this.rbSession2.Text = "2nd Session";
            this.rbSession2.UseVisualStyleBackColor = true;
            this.rbSession2.Visible = false;
            this.rbSession2.CheckedChanged += new System.EventHandler(this.rbSession2_CheckedChanged);
            // 
            // rbfirstsession
            // 
            this.rbfirstsession.AutoSize = true;
            this.rbfirstsession.Checked = true;
            this.rbfirstsession.Enabled = false;
            this.rbfirstsession.Location = new System.Drawing.Point(153, 212);
            this.rbfirstsession.Name = "rbfirstsession";
            this.rbfirstsession.Size = new System.Drawing.Size(79, 17);
            this.rbfirstsession.TabIndex = 10;
            this.rbfirstsession.TabStop = true;
            this.rbfirstsession.Text = "1st Session";
            this.rbfirstsession.UseVisualStyleBackColor = true;
            this.rbfirstsession.Visible = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.cboClient);
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Controls.Add(this.dtToDate);
            this.groupBox2.Controls.Add(this.dtFromDate);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtFileGenerationPath);
            this.groupBox2.Controls.Add(this.btnGenerationBrowse);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.cboCurrType);
            this.groupBox2.Location = new System.Drawing.Point(7, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(420, 105);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(218, 45);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(33, 13);
            this.label9.TabIndex = 18;
            this.label9.Text = "Client";
            // 
            // cboClient
            // 
            this.cboClient.FormattingEnabled = true;
            this.cboClient.Location = new System.Drawing.Point(257, 46);
            this.cboClient.Name = "cboClient";
            this.cboClient.Size = new System.Drawing.Size(157, 21);
            this.cboClient.TabIndex = 17;
            this.cboClient.SelectedIndexChanged += new System.EventHandler(this.cboClient_SelectedIndexChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.groupBox3);
            this.groupBox4.Controls.Add(this.btnAllowPosting);
            this.groupBox4.Controls.Add(this.btnUnpaidLockPosting);
            this.groupBox4.Controls.Add(this.btnUnlockUnpaidPosting);
            this.groupBox4.Location = new System.Drawing.Point(426, 97);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(420, 116);
            this.groupBox4.TabIndex = 16;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Lock / Unlock Posting ";
            this.groupBox4.Visible = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rbAllowrgBoth);
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Controls.Add(this.rbAllowrg2ndsession);
            this.groupBox3.Controls.Add(this.rbAllowrg1st);
            this.groupBox3.Location = new System.Drawing.Point(7, 52);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(405, 60);
            this.groupBox3.TabIndex = 16;
            this.groupBox3.TabStop = false;
            // 
            // rbAllowrgBoth
            // 
            this.rbAllowrgBoth.AutoSize = true;
            this.rbAllowrgBoth.Location = new System.Drawing.Point(3, 40);
            this.rbAllowrgBoth.Name = "rbAllowrgBoth";
            this.rbAllowrgBoth.Size = new System.Drawing.Size(92, 17);
            this.rbAllowrgBoth.TabIndex = 3;
            this.rbAllowrgBoth.Text = "Both Sessions";
            this.rbAllowrgBoth.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(284, 11);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(115, 46);
            this.button1.TabIndex = 2;
            this.button1.Text = "Allow Regeneration";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // rbAllowrg2ndsession
            // 
            this.rbAllowrg2ndsession.AutoSize = true;
            this.rbAllowrg2ndsession.Location = new System.Drawing.Point(3, 24);
            this.rbAllowrg2ndsession.Name = "rbAllowrg2ndsession";
            this.rbAllowrg2ndsession.Size = new System.Drawing.Size(166, 17);
            this.rbAllowrg2ndsession.TabIndex = 1;
            this.rbAllowrg2ndsession.Text = "2nd Session (Unpaid Session)";
            this.rbAllowrg2ndsession.UseVisualStyleBackColor = true;
            this.rbAllowrg2ndsession.CheckedChanged += new System.EventHandler(this.rbAllowrg2ndsession_CheckedChanged);
            // 
            // rbAllowrg1st
            // 
            this.rbAllowrg1st.AutoSize = true;
            this.rbAllowrg1st.Checked = true;
            this.rbAllowrg1st.Location = new System.Drawing.Point(3, 8);
            this.rbAllowrg1st.Name = "rbAllowrg1st";
            this.rbAllowrg1st.Size = new System.Drawing.Size(167, 17);
            this.rbAllowrg1st.TabIndex = 0;
            this.rbAllowrg1st.TabStop = true;
            this.rbAllowrg1st.Text = "1st Session (Evening Session)";
            this.rbAllowrg1st.UseVisualStyleBackColor = true;
            // 
            // btnAllowPosting
            // 
            this.btnAllowPosting.Location = new System.Drawing.Point(6, 11);
            this.btnAllowPosting.Name = "btnAllowPosting";
            this.btnAllowPosting.Size = new System.Drawing.Size(100, 40);
            this.btnAllowPosting.TabIndex = 9;
            this.btnAllowPosting.Text = "Allow Posting";
            this.btnAllowPosting.UseVisualStyleBackColor = true;
            this.btnAllowPosting.Click += new System.EventHandler(this.btnAllowPosting_Click);
            // 
            // btnUnpaidLockPosting
            // 
            this.btnUnpaidLockPosting.Location = new System.Drawing.Point(157, 11);
            this.btnUnpaidLockPosting.Name = "btnUnpaidLockPosting";
            this.btnUnpaidLockPosting.Size = new System.Drawing.Size(100, 40);
            this.btnUnpaidLockPosting.TabIndex = 12;
            this.btnUnpaidLockPosting.Text = "Lock Unpaid Posting";
            this.btnUnpaidLockPosting.UseVisualStyleBackColor = true;
            this.btnUnpaidLockPosting.Click += new System.EventHandler(this.btnUnpaidLockPosting_Click);
            // 
            // btnUnlockUnpaidPosting
            // 
            this.btnUnlockUnpaidPosting.Location = new System.Drawing.Point(308, 11);
            this.btnUnlockUnpaidPosting.Name = "btnUnlockUnpaidPosting";
            this.btnUnlockUnpaidPosting.Size = new System.Drawing.Size(100, 40);
            this.btnUnlockUnpaidPosting.TabIndex = 13;
            this.btnUnlockUnpaidPosting.Text = "Unlock Unpaid Posting";
            this.btnUnlockUnpaidPosting.UseVisualStyleBackColor = true;
            this.btnUnlockUnpaidPosting.Click += new System.EventHandler(this.btnUnlockUnpaidPosting_Click);
            // 
            // dtToDate
            // 
            this.dtToDate.CustomFormat = "dd-MMM-yyyy";
            this.dtToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtToDate.Location = new System.Drawing.Point(290, 75);
            this.dtToDate.Name = "dtToDate";
            this.dtToDate.Size = new System.Drawing.Size(124, 20);
            this.dtToDate.TabIndex = 5;
            // 
            // dtFromDate
            // 
            this.dtFromDate.CustomFormat = "dd-MMM-yyyy";
            this.dtFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtFromDate.Location = new System.Drawing.Point(88, 71);
            this.dtFromDate.Name = "dtFromDate";
            this.dtFromDate.Size = new System.Drawing.Size(124, 20);
            this.dtFromDate.TabIndex = 4;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(235, 75);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(46, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "To Date";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 71);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "From Date:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "File Path";
            // 
            // txtFileGenerationPath
            // 
            this.txtFileGenerationPath.BackColor = System.Drawing.SystemColors.Info;
            this.txtFileGenerationPath.Location = new System.Drawing.Point(87, 19);
            this.txtFileGenerationPath.Name = "txtFileGenerationPath";
            this.txtFileGenerationPath.Size = new System.Drawing.Size(229, 20);
            this.txtFileGenerationPath.TabIndex = 0;
            this.txtFileGenerationPath.TextChanged += new System.EventHandler(this.txtFileGenerationPath_TextChanged);
            this.txtFileGenerationPath.LostFocus += new System.EventHandler(this.txtFileGenerationPath_LostFocus);
            // 
            // btnGenerationBrowse
            // 
            this.btnGenerationBrowse.Location = new System.Drawing.Point(322, 19);
            this.btnGenerationBrowse.Name = "btnGenerationBrowse";
            this.btnGenerationBrowse.Size = new System.Drawing.Size(92, 21);
            this.btnGenerationBrowse.TabIndex = 1;
            this.btnGenerationBrowse.Text = "Browse Folder";
            this.btnGenerationBrowse.UseVisualStyleBackColor = true;
            this.btnGenerationBrowse.Click += new System.EventHandler(this.btnGenerationBrowse_Click);
            this.btnGenerationBrowse.LostFocus += new System.EventHandler(this.btnGenerationBrowse_LostFocus);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 45);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Currency Type";
            // 
            // cboCurrType
            // 
            this.cboCurrType.FormattingEnabled = true;
            this.cboCurrType.Location = new System.Drawing.Point(88, 45);
            this.cboCurrType.Name = "cboCurrType";
            this.cboCurrType.Size = new System.Drawing.Size(124, 21);
            this.cboCurrType.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 128);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "File Date";
            this.label5.Visible = false;
            // 
            // dtGenerationFileDate
            // 
            this.dtGenerationFileDate.CustomFormat = "dd-MMM-yyyy";
            this.dtGenerationFileDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtGenerationFileDate.Location = new System.Drawing.Point(69, 124);
            this.dtGenerationFileDate.Name = "dtGenerationFileDate";
            this.dtGenerationFileDate.Size = new System.Drawing.Size(124, 20);
            this.dtGenerationFileDate.TabIndex = 3;
            this.dtGenerationFileDate.Visible = false;
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(365, 192);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(70, 25);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // prbFileProgress
            // 
            this.prbFileProgress.Location = new System.Drawing.Point(4, 192);
            this.prbFileProgress.Name = "prbFileProgress";
            this.prbFileProgress.Size = new System.Drawing.Size(323, 18);
            this.prbFileProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.prbFileProgress.TabIndex = 5;
            this.prbFileProgress.Visible = false;
            // 
            // lblmessage
            // 
            this.lblmessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblmessage.Location = new System.Drawing.Point(1, 189);
            this.lblmessage.Name = "lblmessage";
            this.lblmessage.Size = new System.Drawing.Size(362, 31);
            this.lblmessage.TabIndex = 8;
            // 
            // colTrxDescription
            // 
            this.colTrxDescription.HeaderText = "TrxDescription";
            this.colTrxDescription.Name = "colTrxDescription";
            this.colTrxDescription.ReadOnly = true;
            // 
            // colChequeID
            // 
            this.colChequeID.HeaderText = "ChequeID";
            this.colChequeID.Name = "colChequeID";
            this.colChequeID.ReadOnly = true;
            // 
            // colAmount
            // 
            this.colAmount.HeaderText = "Amount";
            this.colAmount.Name = "colAmount";
            this.colAmount.ReadOnly = true;
            // 
            // colAccount
            // 
            this.colAccount.HeaderText = "Account";
            this.colAccount.Name = "colAccount";
            this.colAccount.ReadOnly = true;
            // 
            // colSlno
            // 
            this.colSlno.HeaderText = "SLNo";
            this.colSlno.Name = "colSlno";
            this.colSlno.ReadOnly = true;
            // 
            // colOurBranchID
            // 
            this.colOurBranchID.HeaderText = "OurBranchID";
            this.colOurBranchID.Name = "colOurBranchID";
            this.colOurBranchID.ReadOnly = true;
            // 
            // dgvUnsupervised
            // 
            this.dgvUnsupervised.AllowUserToAddRows = false;
            this.dgvUnsupervised.AllowUserToDeleteRows = false;
            this.dgvUnsupervised.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUnsupervised.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colOurBranchID,
            this.colSlno,
            this.colAccount,
            this.colAmount,
            this.colChequeID,
            this.colTrxDescription});
            this.dgvUnsupervised.Location = new System.Drawing.Point(3, 226);
            this.dgvUnsupervised.Name = "dgvUnsupervised";
            this.dgvUnsupervised.ReadOnly = true;
            this.dgvUnsupervised.Size = new System.Drawing.Size(439, 204);
            this.dgvUnsupervised.TabIndex = 9;
            // 
            // BRNetUploadDownLoadUtilityMFI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::BRNETUploadDownload.Properties.Resources.log_img6;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(443, 226);
            this.ControlBox = false;
            this.Controls.Add(this.dgvUnsupervised);
            this.Controls.Add(this.lblmessage);
            this.Controls.Add(this.prbFileProgress);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.tcrBRFileDownloadUpload);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "BRNetUploadDownLoadUtilityMFI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BrNet Importation / Upload Utility";
            this.Load += new System.EventHandler(this.BRNetUploadDownLoadUtilityMFI_Load);
            this.tcrBRFileDownloadUpload.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUnsupervised)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnGenarate;
        private System.Windows.Forms.TabControl tcrBRFileDownloadUpload;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtFileGenerationPath;
        private System.Windows.Forms.Button btnGenerationBrowse;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtGenerationFileDate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cboCurrType;
        private System.Windows.Forms.DateTimePicker dtToDate;
        private System.Windows.Forms.DateTimePicker dtFromDate;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.ProgressBar prbFileProgress;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.RadioButton rbNonParticipating;
        private System.Windows.Forms.RadioButton rbParticipating;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtFolderName;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtFileDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboCurrencyType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrxDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn colChequeID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAccount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSlno;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOurBranchID;
        private System.Windows.Forms.DataGridView dgvUnsupervised;
        private System.Windows.Forms.Button btnAllowPosting;
        private System.Windows.Forms.RadioButton rbfirstsession;
        private System.Windows.Forms.RadioButton rbSession2;
        private System.Windows.Forms.Button btnUnlockUnpaidPosting;
        private System.Windows.Forms.Button btnUnpaidLockPosting;
        private System.Windows.Forms.RadioButton rbNormalClearing;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rbAllowrgBoth;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RadioButton rbAllowrg2ndsession;
        private System.Windows.Forms.RadioButton rbAllowrg1st;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cboClient;
        private System.Windows.Forms.Label lblmessage;
    }
}

