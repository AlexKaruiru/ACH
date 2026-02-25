namespace BROutgoingClearingStatus
{
    partial class frmClearingStatus
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dtpATSBusinessDate = new System.Windows.Forms.DateTimePicker();
            this.lblClearingPortalDate = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.dtTo = new System.Windows.Forms.DateTimePicker();
            this.dtFrom = new System.Windows.Forms.DateTimePicker();
            this.btnFetch = new System.Windows.Forms.Button();
            this.Regenerate = new System.Windows.Forms.Button();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnRefreshStatus = new System.Windows.Forms.Button();
            this.lblCurrencyID = new System.Windows.Forms.Label();
            this.lblAmount = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cboCurrency = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cboSession = new System.Windows.Forms.ComboBox();
            this.cboBanks = new System.Windows.Forms.ComboBox();
            this.cboFileType = new System.Windows.Forms.ComboBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.BtnReset = new System.Windows.Forms.Button();
            this.btnView = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvClearingStatus1 = new System.Windows.Forms.DataGridView();
            this.lblmessage = new System.Windows.Forms.Label();
            this.colChkbox = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colRowNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColBranch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColAccount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColBeneficiaryAccount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColBeneficiaryName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColBeneficiaryBank = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColRemarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTrxRowID = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClearingStatus1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.dtpATSBusinessDate);
            this.groupBox1.Controls.Add(this.lblClearingPortalDate);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.dtTo);
            this.groupBox1.Controls.Add(this.dtFrom);
            this.groupBox1.Controls.Add(this.btnFetch);
            this.groupBox1.Controls.Add(this.Regenerate);
            this.groupBox1.Controls.Add(this.btnCreate);
            this.groupBox1.Controls.Add(this.btnRefreshStatus);
            this.groupBox1.Controls.Add(this.lblCurrencyID);
            this.groupBox1.Controls.Add(this.lblAmount);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cboCurrency);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cboSession);
            this.groupBox1.Controls.Add(this.cboBanks);
            this.groupBox1.Controls.Add(this.cboFileType);
            this.groupBox1.Controls.Add(this.btnClose);
            this.groupBox1.Controls.Add(this.BtnReset);
            this.groupBox1.Controls.Add(this.btnView);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1205, 108);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // dtpATSBusinessDate
            // 
            this.dtpATSBusinessDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpATSBusinessDate.Location = new System.Drawing.Point(398, 35);
            this.dtpATSBusinessDate.Name = "dtpATSBusinessDate";
            this.dtpATSBusinessDate.Size = new System.Drawing.Size(135, 20);
            this.dtpATSBusinessDate.TabIndex = 23;
            // 
            // lblClearingPortalDate
            // 
            this.lblClearingPortalDate.BackColor = System.Drawing.Color.Transparent;
            this.lblClearingPortalDate.Location = new System.Drawing.Point(227, 39);
            this.lblClearingPortalDate.Name = "lblClearingPortalDate";
            this.lblClearingPortalDate.Size = new System.Drawing.Size(134, 13);
            this.lblClearingPortalDate.TabIndex = 22;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(538, 39);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 13);
            this.label7.TabIndex = 21;
            this.label7.Text = "Date From:";
            this.label7.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(553, 75);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "Date To";
            this.label6.Visible = false;
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // dtTo
            // 
            this.dtTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtTo.Location = new System.Drawing.Point(605, 71);
            this.dtTo.Name = "dtTo";
            this.dtTo.Size = new System.Drawing.Size(135, 20);
            this.dtTo.TabIndex = 19;
            this.dtTo.Visible = false;
            // 
            // dtFrom
            // 
            this.dtFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtFrom.Location = new System.Drawing.Point(604, 35);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.Size = new System.Drawing.Size(135, 20);
            this.dtFrom.TabIndex = 18;
            this.dtFrom.Visible = false;
            // 
            // btnFetch
            // 
            this.btnFetch.Enabled = false;
            this.btnFetch.Location = new System.Drawing.Point(834, 78);
            this.btnFetch.Name = "btnFetch";
            this.btnFetch.Size = new System.Drawing.Size(152, 23);
            this.btnFetch.TabIndex = 17;
            this.btnFetch.Text = "Fetch";
            this.btnFetch.UseVisualStyleBackColor = true;
            this.btnFetch.Click += new System.EventHandler(this.btnFetch_Click);
            // 
            // Regenerate
            // 
            this.Regenerate.Enabled = false;
            this.Regenerate.Location = new System.Drawing.Point(834, 55);
            this.Regenerate.Name = "Regenerate";
            this.Regenerate.Size = new System.Drawing.Size(152, 23);
            this.Regenerate.TabIndex = 16;
            this.Regenerate.Text = "Re-Create";
            this.Regenerate.UseVisualStyleBackColor = true;
            this.Regenerate.Click += new System.EventHandler(this.Regenerate_Click);
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(834, 31);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(152, 23);
            this.btnCreate.TabIndex = 15;
            this.btnCreate.Text = "Create";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnRefreshStatus
            // 
            this.btnRefreshStatus.Location = new System.Drawing.Point(1054, 57);
            this.btnRefreshStatus.Name = "btnRefreshStatus";
            this.btnRefreshStatus.Size = new System.Drawing.Size(142, 22);
            this.btnRefreshStatus.TabIndex = 14;
            this.btnRefreshStatus.Text = "Update Status";
            this.btnRefreshStatus.UseVisualStyleBackColor = true;
            this.btnRefreshStatus.Click += new System.EventHandler(this.btnRefreshStatus_Click);
            // 
            // lblCurrencyID
            // 
            this.lblCurrencyID.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrencyID.Location = new System.Drawing.Point(86, 78);
            this.lblCurrencyID.Name = "lblCurrencyID";
            this.lblCurrencyID.Size = new System.Drawing.Size(38, 23);
            this.lblCurrencyID.TabIndex = 13;
            // 
            // lblAmount
            // 
            this.lblAmount.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAmount.Location = new System.Drawing.Point(74, 41);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new System.Drawing.Size(133, 27);
            this.lblAmount.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 41);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Total : ";
            // 
            // cboCurrency
            // 
            this.cboCurrency.FormattingEnabled = true;
            this.cboCurrency.Location = new System.Drawing.Point(834, 8);
            this.cboCurrency.Name = "cboCurrency";
            this.cboCurrency.Size = new System.Drawing.Size(152, 21);
            this.cboCurrency.TabIndex = 10;
            this.cboCurrency.SelectedIndexChanged += new System.EventHandler(this.cboCurrency_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(762, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Currency";
            // 
            // cboSession
            // 
            this.cboSession.FormattingEnabled = true;
            this.cboSession.Location = new System.Drawing.Point(605, 8);
            this.cboSession.Name = "cboSession";
            this.cboSession.Size = new System.Drawing.Size(135, 21);
            this.cboSession.TabIndex = 8;
            this.cboSession.SelectedIndexChanged += new System.EventHandler(this.cboSession_SelectedIndexChanged);
            // 
            // cboBanks
            // 
            this.cboBanks.FormattingEnabled = true;
            this.cboBanks.Location = new System.Drawing.Point(312, 8);
            this.cboBanks.Name = "cboBanks";
            this.cboBanks.Size = new System.Drawing.Size(222, 21);
            this.cboBanks.TabIndex = 7;
            this.cboBanks.SelectedIndexChanged += new System.EventHandler(this.cboBanks_SelectedIndexChanged);
            // 
            // cboFileType
            // 
            this.cboFileType.FormattingEnabled = true;
            this.cboFileType.Location = new System.Drawing.Point(72, 8);
            this.cboFileType.Name = "cboFileType";
            this.cboFileType.Size = new System.Drawing.Size(135, 21);
            this.cboFileType.TabIndex = 6;
            this.cboFileType.SelectedIndexChanged += new System.EventHandler(this.cboFileType_SelectedIndexChanged);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(1054, 81);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(142, 22);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // BtnReset
            // 
            this.BtnReset.Location = new System.Drawing.Point(1054, 33);
            this.BtnReset.Name = "BtnReset";
            this.BtnReset.Size = new System.Drawing.Size(142, 22);
            this.BtnReset.TabIndex = 4;
            this.BtnReset.Text = "Reset";
            this.BtnReset.UseVisualStyleBackColor = true;
            this.BtnReset.Click += new System.EventHandler(this.BtnReset_Click);
            // 
            // btnView
            // 
            this.btnView.Location = new System.Drawing.Point(1054, 9);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(142, 22);
            this.btnView.TabIndex = 3;
            this.btnView.Text = "View";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(555, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Session";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(223, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Receiving Bank";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "File Type";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.dgvClearingStatus1);
            this.groupBox2.Location = new System.Drawing.Point(12, 120);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1202, 443);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // dgvClearingStatus1
            // 
            this.dgvClearingStatus1.AllowUserToAddRows = false;
            this.dgvClearingStatus1.AllowUserToDeleteRows = false;
            this.dgvClearingStatus1.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightSkyBlue;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Blue;
            this.dgvClearingStatus1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvClearingStatus1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvClearingStatus1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvClearingStatus1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvClearingStatus1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvClearingStatus1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colChkbox,
            this.colRowNo,
            this.ColBranch,
            this.ColAccount,
            this.ColName,
            this.ColAmount,
            this.ColBeneficiaryAccount,
            this.ColBeneficiaryName,
            this.ColBeneficiaryBank,
            this.ColStatus,
            this.ColRemarks,
            this.colTrxRowID});
            this.dgvClearingStatus1.Location = new System.Drawing.Point(9, 15);
            this.dgvClearingStatus1.Name = "dgvClearingStatus1";
            this.dgvClearingStatus1.Size = new System.Drawing.Size(1187, 418);
            this.dgvClearingStatus1.TabIndex = 0;
            this.dgvClearingStatus1.TabStop = false;
            this.dgvClearingStatus1.Visible = false;
            this.dgvClearingStatus1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvClearingStatus_CellClick);
            this.dgvClearingStatus1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvClearingStatus_CellContentClick);
            // 
            // lblmessage
            // 
            this.lblmessage.BackColor = System.Drawing.Color.Transparent;
            this.lblmessage.Location = new System.Drawing.Point(12, 570);
            this.lblmessage.Name = "lblmessage";
            this.lblmessage.Size = new System.Drawing.Size(1202, 23);
            this.lblmessage.TabIndex = 2;
            // 
            // colChkbox
            // 
            this.colChkbox.HeaderText = "";
            this.colChkbox.Name = "colChkbox";
            this.colChkbox.ToolTipText = "Select items to regenerate";
            this.colChkbox.Width = 50;
            //
            // col RowNo
            //
            this.colRowNo.HeaderText = "No.";
            this.colRowNo.Name = "colRowNo";
            this.colRowNo.Width = 30;
            this.colRowNo.ReadOnly = true;
            // 
            // ColBranch
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.ColBranch.DefaultCellStyle = dataGridViewCellStyle3;
            this.ColBranch.HeaderText = "Branch";
            this.ColBranch.Name = "ColBranch";
            this.ColBranch.Width = 50;
            // 
            // ColAccount
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ColAccount.DefaultCellStyle = dataGridViewCellStyle4;
            this.ColAccount.HeaderText = "Account";
            this.ColAccount.Name = "ColAccount";
            // 
            // ColName
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.ColName.DefaultCellStyle = dataGridViewCellStyle5;
            this.ColName.HeaderText = "Name";
            this.ColName.Name = "ColName";
            this.ColName.Width = 200;
            // 
            // ColAmount
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Format = "N2";
            dataGridViewCellStyle6.NullValue = "0.00";
            this.ColAmount.DefaultCellStyle = dataGridViewCellStyle6;
            this.ColAmount.HeaderText = "Amount";
            this.ColAmount.Name = "ColAmount";
            // 
            // ColBeneficiaryAccount
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ColBeneficiaryAccount.DefaultCellStyle = dataGridViewCellStyle7;
            this.ColBeneficiaryAccount.HeaderText = "Beneficiary Acc.";
            this.ColBeneficiaryAccount.Name = "ColBeneficiaryAccount";
            // 
            // ColBeneficiaryName
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.ColBeneficiaryName.DefaultCellStyle = dataGridViewCellStyle8;
            this.ColBeneficiaryName.HeaderText = "Beneficiary Name";
            this.ColBeneficiaryName.Name = "ColBeneficiaryName";
            this.ColBeneficiaryName.Width = 200;
            // 
            // ColBeneficiaryBank
            // 
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.ColBeneficiaryBank.DefaultCellStyle = dataGridViewCellStyle9;
            this.ColBeneficiaryBank.HeaderText = "Beneficiary Bank";
            this.ColBeneficiaryBank.Name = "ColBeneficiaryBank";
            // 
            // ColStatus
            // 
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ColStatus.DefaultCellStyle = dataGridViewCellStyle10;
            this.ColStatus.HeaderText = "Status";
            this.ColStatus.Name = "ColStatus";
            // 
            // ColRemarks
            // 
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.ColRemarks.DefaultCellStyle = dataGridViewCellStyle11;
            this.ColRemarks.HeaderText = "Remarks";
            this.ColRemarks.Name = "ColRemarks";
            this.ColRemarks.Width = 500;
            // 
            // colTrxRowID
            // 
            this.colTrxRowID.HeaderText = "TrxRowID";
            this.colTrxRowID.Name = "colTrxRowID";
            this.colTrxRowID.Visible = false;
            // 
            // frmClearingStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::BRNETUploadDownload.Properties.Resources.log_img6;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1028, 598);
            this.Controls.Add(this.lblmessage);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmClearingStatus";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Outgoing Clearing File Generation and Status";
            this.Load += new System.EventHandler(this.frmClearingStatus_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvClearingStatus1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cboSession;
        private System.Windows.Forms.ComboBox cboBanks;
        private System.Windows.Forms.ComboBox cboFileType;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button BtnReset;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dgvClearingStatus1;
        private System.Windows.Forms.ComboBox cboCurrency;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblAmount;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblCurrencyID;
        private System.Windows.Forms.Button btnRefreshStatus;
        private System.Windows.Forms.Button btnFetch;
        private System.Windows.Forms.Button Regenerate;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Label lblmessage;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtTo;
        private System.Windows.Forms.DateTimePicker dtFrom;
        private System.Windows.Forms.DateTimePicker dtpATSBusinessDate;
        private System.Windows.Forms.Label lblClearingPortalDate;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colChkbox;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRowNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColBranch;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColAccount;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColBeneficiaryAccount;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColBeneficiaryName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColBeneficiaryBank;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColRemarks;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colTrxRowID;
    }
}

