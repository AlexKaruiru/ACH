<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBROutwardClearingCTS
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBROutwardClearingCTS))
        Me.textState = New System.Windows.Forms.TextBox()
        Me.pictPreview4 = New System.Windows.Forms.PictureBox()
        Me.pictPreview3 = New System.Windows.Forms.PictureBox()
        Me.pictPreview2 = New System.Windows.Forms.PictureBox()
        Me.pictPreview1 = New System.Windows.Forms.PictureBox()
        Me.buttonOptions = New System.Windows.Forms.Button()
        Me.buttonStopFeed = New System.Windows.Forms.Button()
        Me.buttonStartFeed = New System.Windows.Forms.Button()
        Me.buttonFreeTrack = New System.Windows.Forms.Button()
        Me.buttonExit = New System.Windows.Forms.Button()
        Me.lstMessages = New System.Windows.Forms.ListBox()
        Me.timerState = New System.Windows.Forms.Timer(Me.components)
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.txtClrCenter = New System.Windows.Forms.TextBox()
        Me.txtVoucherCode = New System.Windows.Forms.TextBox()
        Me.txtChqDigit = New System.Windows.Forms.TextBox()
        Me.txtChqNo = New System.Windows.Forms.TextBox()
        Me.txtTheirAccID = New System.Windows.Forms.TextBox()
        Me.txtBranchName = New System.Windows.Forms.TextBox()
        Me.txtBranchID = New System.Windows.Forms.TextBox()
        Me.txtBankName = New System.Windows.Forms.TextBox()
        Me.txtBankID = New System.Windows.Forms.TextBox()
        Me.txtAccName = New System.Windows.Forms.TextBox()
        Me.txtAccountID = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtChqCount = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.BackGraySPic = New System.Windows.Forms.PictureBox()
        Me.FrontBWPic = New System.Windows.Forms.PictureBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.pictMainRear = New CSI_ImageControl.ImageControl()
        Me.pictMainFront = New CSI_ImageControl.ImageControl()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.dgvOutCreditMicr = New System.Windows.Forms.DataGridView()
        Me.dgvRejectedItem = New System.Windows.Forms.DataGridView()
        Me.colchqID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colBnkID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colBrnID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colRejReason = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colRejVoucherCode = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colRejChqDigit = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colRejTheirAccID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colFrontImg = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colBackImg = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colUVImg = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colImageunqID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TFImageBW = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colJRImagePath = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colTFImagePath = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colJFImagePath = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colUVImagePath = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.lblCount = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.lblAmount = New System.Windows.Forms.Label()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.btnOption = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.bgwImageSaver = New System.ComponentModel.BackgroundWorker()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnMDV = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.Button7 = New System.Windows.Forms.Button()
        Me.Button8 = New System.Windows.Forms.Button()
        Me.RBTypeC = New System.Windows.Forms.RadioButton()
        Me.RBTypeB = New System.Windows.Forms.RadioButton()
        Me.colChequeNo = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colAmount = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colDrawer = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colChqDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colBankID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colBranch = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colTheirAcc = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colclgDays = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colRetCode = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colHighValue = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.colchqdigit = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colFIMGImage = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colBIMaGe = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colUVIMage = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colClrCenter = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colVoucherCode = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colOurComm = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colTheirComm = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colUniqueID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colTFImageSize = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colJFImageSize = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colJRImageSize = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colAccountID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TFImage = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TFImagePath = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.JFImagePath = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.JRImagePath = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.UVImagePath = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colmicrline = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColumnID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colOurCommission = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colTheirCommission = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colTotalCommission = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colIsUpcountry = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.colMinCommRate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colCommRate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColOurCommRate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colCurrencyID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColJFImageSignature = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColTFImageSignature = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColJRImageSignature = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colBranchName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colBankName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colValueDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.coJRdpi = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colTFdpi = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colJFdpi = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.pictPreview4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pictPreview3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pictPreview2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pictPreview1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.BackGraySPic, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.FrontBWPic, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox3.SuspendLayout()
        CType(Me.dgvOutCreditMicr, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvRejectedItem, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.SuspendLayout()
        '
        'textState
        '
        Me.textState.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.textState.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.textState.Enabled = False
        Me.textState.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.textState.ForeColor = System.Drawing.Color.Black
        Me.textState.Location = New System.Drawing.Point(7, 327)
        Me.textState.Name = "textState"
        Me.textState.Size = New System.Drawing.Size(93, 21)
        Me.textState.TabIndex = 0
        '
        'pictPreview4
        '
        Me.pictPreview4.Location = New System.Drawing.Point(1086, 535)
        Me.pictPreview4.MaximumSize = New System.Drawing.Size(100, 50)
        Me.pictPreview4.MinimumSize = New System.Drawing.Size(100, 50)
        Me.pictPreview4.Name = "pictPreview4"
        Me.pictPreview4.Size = New System.Drawing.Size(100, 50)
        Me.pictPreview4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pictPreview4.TabIndex = 24
        Me.pictPreview4.TabStop = False
        '
        'pictPreview3
        '
        Me.pictPreview3.Location = New System.Drawing.Point(1167, 411)
        Me.pictPreview3.MaximumSize = New System.Drawing.Size(100, 50)
        Me.pictPreview3.MinimumSize = New System.Drawing.Size(100, 50)
        Me.pictPreview3.Name = "pictPreview3"
        Me.pictPreview3.Size = New System.Drawing.Size(100, 50)
        Me.pictPreview3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pictPreview3.TabIndex = 23
        Me.pictPreview3.TabStop = False
        '
        'pictPreview2
        '
        Me.pictPreview2.BackColor = System.Drawing.Color.Transparent
        Me.pictPreview2.Location = New System.Drawing.Point(125, 62)
        Me.pictPreview2.MaximumSize = New System.Drawing.Size(100, 50)
        Me.pictPreview2.MinimumSize = New System.Drawing.Size(100, 50)
        Me.pictPreview2.Name = "pictPreview2"
        Me.pictPreview2.Size = New System.Drawing.Size(100, 50)
        Me.pictPreview2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pictPreview2.TabIndex = 22
        Me.pictPreview2.TabStop = False
        '
        'pictPreview1
        '
        Me.pictPreview1.BackColor = System.Drawing.Color.Transparent
        Me.pictPreview1.Location = New System.Drawing.Point(8, 7)
        Me.pictPreview1.MaximumSize = New System.Drawing.Size(100, 50)
        Me.pictPreview1.MinimumSize = New System.Drawing.Size(100, 50)
        Me.pictPreview1.Name = "pictPreview1"
        Me.pictPreview1.Size = New System.Drawing.Size(100, 50)
        Me.pictPreview1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pictPreview1.TabIndex = 21
        Me.pictPreview1.TabStop = False
        '
        'buttonOptions
        '
        Me.buttonOptions.Location = New System.Drawing.Point(6, 222)
        Me.buttonOptions.Name = "buttonOptions"
        Me.buttonOptions.Size = New System.Drawing.Size(92, 26)
        Me.buttonOptions.TabIndex = 20
        Me.buttonOptions.Text = "&Options"
        Me.buttonOptions.UseVisualStyleBackColor = True
        Me.buttonOptions.Visible = False
        '
        'buttonStopFeed
        '
        Me.buttonStopFeed.Location = New System.Drawing.Point(7, 70)
        Me.buttonStopFeed.Name = "buttonStopFeed"
        Me.buttonStopFeed.Size = New System.Drawing.Size(90, 24)
        Me.buttonStopFeed.TabIndex = 8
        Me.buttonStopFeed.Text = "Stop Feeding"
        Me.buttonStopFeed.UseVisualStyleBackColor = True
        '
        'buttonStartFeed
        '
        Me.buttonStartFeed.Location = New System.Drawing.Point(7, 19)
        Me.buttonStartFeed.Name = "buttonStartFeed"
        Me.buttonStartFeed.Size = New System.Drawing.Size(90, 24)
        Me.buttonStartFeed.TabIndex = 7
        Me.buttonStartFeed.Text = "Start Feeding"
        Me.buttonStartFeed.UseVisualStyleBackColor = True
        '
        'buttonFreeTrack
        '
        Me.buttonFreeTrack.Location = New System.Drawing.Point(8, 121)
        Me.buttonFreeTrack.Name = "buttonFreeTrack"
        Me.buttonFreeTrack.Size = New System.Drawing.Size(89, 24)
        Me.buttonFreeTrack.TabIndex = 9
        Me.buttonFreeTrack.Text = "&FreeTrack"
        Me.buttonFreeTrack.UseVisualStyleBackColor = True
        '
        'buttonExit
        '
        Me.buttonExit.Location = New System.Drawing.Point(6, 274)
        Me.buttonExit.Name = "buttonExit"
        Me.buttonExit.Size = New System.Drawing.Size(91, 24)
        Me.buttonExit.TabIndex = 10
        Me.buttonExit.Text = "E&xit"
        Me.buttonExit.UseVisualStyleBackColor = True
        '
        'lstMessages
        '
        Me.lstMessages.FormattingEnabled = True
        Me.lstMessages.Location = New System.Drawing.Point(618, 749)
        Me.lstMessages.Name = "lstMessages"
        Me.lstMessages.Size = New System.Drawing.Size(43, 4)
        Me.lstMessages.TabIndex = 15
        '
        'timerState
        '
        Me.timerState.Interval = 1500
        '
        'GroupBox1
        '
        Me.GroupBox1.BackgroundImage = Global.BRCTSScanner.My.Resources.Resources.log_img6
        Me.GroupBox1.Controls.Add(Me.txtClrCenter)
        Me.GroupBox1.Controls.Add(Me.txtVoucherCode)
        Me.GroupBox1.Controls.Add(Me.txtChqDigit)
        Me.GroupBox1.Controls.Add(Me.txtChqNo)
        Me.GroupBox1.Controls.Add(Me.txtTheirAccID)
        Me.GroupBox1.Controls.Add(Me.txtBranchName)
        Me.GroupBox1.Controls.Add(Me.txtBranchID)
        Me.GroupBox1.Controls.Add(Me.txtBankName)
        Me.GroupBox1.Controls.Add(Me.txtBankID)
        Me.GroupBox1.Controls.Add(Me.txtAccName)
        Me.GroupBox1.Controls.Add(Me.txtAccountID)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(295, 274)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(611, 94)
        Me.GroupBox1.TabIndex = 28
        Me.GroupBox1.TabStop = False
        '
        'txtClrCenter
        '
        Me.txtClrCenter.BackColor = System.Drawing.SystemColors.Window
        Me.txtClrCenter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtClrCenter.Location = New System.Drawing.Point(448, 67)
        Me.txtClrCenter.Name = "txtClrCenter"
        Me.txtClrCenter.Size = New System.Drawing.Size(44, 20)
        Me.txtClrCenter.TabIndex = 20
        '
        'txtVoucherCode
        '
        Me.txtVoucherCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtVoucherCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtVoucherCode.Location = New System.Drawing.Point(582, 43)
        Me.txtVoucherCode.MaxLength = 2
        Me.txtVoucherCode.Name = "txtVoucherCode"
        Me.txtVoucherCode.Size = New System.Drawing.Size(20, 20)
        Me.txtVoucherCode.TabIndex = 6
        '
        'txtChqDigit
        '
        Me.txtChqDigit.BackColor = System.Drawing.SystemColors.Window
        Me.txtChqDigit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtChqDigit.Location = New System.Drawing.Point(582, 19)
        Me.txtChqDigit.MaxLength = 1
        Me.txtChqDigit.Name = "txtChqDigit"
        Me.txtChqDigit.Size = New System.Drawing.Size(20, 20)
        Me.txtChqDigit.TabIndex = 2
        '
        'txtChqNo
        '
        Me.txtChqNo.BackColor = System.Drawing.SystemColors.Window
        Me.txtChqNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtChqNo.Location = New System.Drawing.Point(426, 43)
        Me.txtChqNo.MaxLength = 6
        Me.txtChqNo.Name = "txtChqNo"
        Me.txtChqNo.Size = New System.Drawing.Size(66, 20)
        Me.txtChqNo.TabIndex = 5
        '
        'txtTheirAccID
        '
        Me.txtTheirAccID.BackColor = System.Drawing.SystemColors.Window
        Me.txtTheirAccID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTheirAccID.Location = New System.Drawing.Point(377, 17)
        Me.txtTheirAccID.MaxLength = 15
        Me.txtTheirAccID.Name = "txtTheirAccID"
        Me.txtTheirAccID.Size = New System.Drawing.Size(122, 20)
        Me.txtTheirAccID.TabIndex = 1
        '
        'txtBranchName
        '
        Me.txtBranchName.BackColor = System.Drawing.SystemColors.Window
        Me.txtBranchName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtBranchName.Location = New System.Drawing.Point(124, 67)
        Me.txtBranchName.Name = "txtBranchName"
        Me.txtBranchName.Size = New System.Drawing.Size(169, 20)
        Me.txtBranchName.TabIndex = 14
        '
        'txtBranchID
        '
        Me.txtBranchID.BackColor = System.Drawing.SystemColors.Window
        Me.txtBranchID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtBranchID.Location = New System.Drawing.Point(77, 67)
        Me.txtBranchID.MaxLength = 3
        Me.txtBranchID.Name = "txtBranchID"
        Me.txtBranchID.Size = New System.Drawing.Size(41, 20)
        Me.txtBranchID.TabIndex = 4
        '
        'txtBankName
        '
        Me.txtBankName.BackColor = System.Drawing.SystemColors.Window
        Me.txtBankName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtBankName.Location = New System.Drawing.Point(124, 43)
        Me.txtBankName.Name = "txtBankName"
        Me.txtBankName.Size = New System.Drawing.Size(169, 20)
        Me.txtBankName.TabIndex = 12
        '
        'txtBankID
        '
        Me.txtBankID.BackColor = System.Drawing.SystemColors.Window
        Me.txtBankID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtBankID.Location = New System.Drawing.Point(77, 43)
        Me.txtBankID.MaxLength = 2
        Me.txtBankID.Name = "txtBankID"
        Me.txtBankID.Size = New System.Drawing.Size(41, 20)
        Me.txtBankID.TabIndex = 3
        '
        'txtAccName
        '
        Me.txtAccName.BackColor = System.Drawing.SystemColors.Window
        Me.txtAccName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtAccName.Location = New System.Drawing.Point(164, 19)
        Me.txtAccName.Name = "txtAccName"
        Me.txtAccName.Size = New System.Drawing.Size(129, 20)
        Me.txtAccName.TabIndex = 10
        Me.txtAccName.TabStop = False
        '
        'txtAccountID
        '
        Me.txtAccountID.BackColor = System.Drawing.SystemColors.Window
        Me.txtAccountID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtAccountID.Location = New System.Drawing.Point(77, 19)
        Me.txtAccountID.Name = "txtAccountID"
        Me.txtAccountID.Size = New System.Drawing.Size(81, 20)
        Me.txtAccountID.TabIndex = 9
        Me.txtAccountID.TabStop = False
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Location = New System.Drawing.Point(498, 47)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(78, 13)
        Me.Label8.TabIndex = 7
        Me.Label8.Text = "Voucher Code:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.Location = New System.Drawing.Point(505, 24)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(71, 13)
        Me.Label7.TabIndex = 6
        Me.Label7.Text = "Cheque Digit:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Location = New System.Drawing.Point(312, 74)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(59, 13)
        Me.Label6.TabIndex = 5
        Me.Label6.Text = "Clr. Center:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Location = New System.Drawing.Point(317, 50)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(54, 13)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Cheque#:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Location = New System.Drawing.Point(298, 24)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(73, 13)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Their Acc. ID:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Location = New System.Drawing.Point(27, 74)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(44, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Branch:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Location = New System.Drawing.Point(36, 52)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(35, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Bank:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Location = New System.Drawing.Point(10, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(61, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "AccountID:"
        '
        'txtChqCount
        '
        Me.txtChqCount.BackColor = System.Drawing.SystemColors.Window
        Me.txtChqCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtChqCount.Location = New System.Drawing.Point(853, 373)
        Me.txtChqCount.Name = "txtChqCount"
        Me.txtChqCount.Size = New System.Drawing.Size(20, 20)
        Me.txtChqCount.TabIndex = 0
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.BackColor = System.Drawing.Color.Transparent
        Me.Label9.Location = New System.Drawing.Point(715, 374)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(138, 13)
        Me.Label9.TabIndex = 8
        Me.Label9.Text = "No. Of Cheques to Capture:"
        '
        'GroupBox2
        '
        Me.GroupBox2.BackgroundImage = Global.BRCTSScanner.My.Resources.Resources.log_img6
        Me.GroupBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.GroupBox2.Controls.Add(Me.BackGraySPic)
        Me.GroupBox2.Controls.Add(Me.FrontBWPic)
        Me.GroupBox2.Controls.Add(Me.pictPreview1)
        Me.GroupBox2.Controls.Add(Me.pictPreview2)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 274)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(231, 118)
        Me.GroupBox2.TabIndex = 29
        Me.GroupBox2.TabStop = False
        '
        'BackGraySPic
        '
        Me.BackGraySPic.BackColor = System.Drawing.Color.Transparent
        Me.BackGraySPic.Location = New System.Drawing.Point(8, 62)
        Me.BackGraySPic.MaximumSize = New System.Drawing.Size(100, 50)
        Me.BackGraySPic.MinimumSize = New System.Drawing.Size(100, 50)
        Me.BackGraySPic.Name = "BackGraySPic"
        Me.BackGraySPic.Size = New System.Drawing.Size(100, 50)
        Me.BackGraySPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.BackGraySPic.TabIndex = 24
        Me.BackGraySPic.TabStop = False
        '
        'FrontBWPic
        '
        Me.FrontBWPic.BackColor = System.Drawing.Color.Transparent
        Me.FrontBWPic.Location = New System.Drawing.Point(125, 7)
        Me.FrontBWPic.MaximumSize = New System.Drawing.Size(100, 50)
        Me.FrontBWPic.MinimumSize = New System.Drawing.Size(100, 50)
        Me.FrontBWPic.Name = "FrontBWPic"
        Me.FrontBWPic.Size = New System.Drawing.Size(100, 50)
        Me.FrontBWPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.FrontBWPic.TabIndex = 23
        Me.FrontBWPic.TabStop = False
        '
        'GroupBox3
        '
        Me.GroupBox3.BackgroundImage = Global.BRCTSScanner.My.Resources.Resources.log_img6
        Me.GroupBox3.Controls.Add(Me.pictMainRear)
        Me.GroupBox3.Controls.Add(Me.pictMainFront)
        Me.GroupBox3.Location = New System.Drawing.Point(12, 9)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(894, 259)
        Me.GroupBox3.TabIndex = 30
        Me.GroupBox3.TabStop = False
        '
        'pictMainRear
        '
        Me.pictMainRear.BackColor = System.Drawing.Color.Transparent
        Me.pictMainRear.Image = Nothing
        Me.pictMainRear.initialimage = Nothing
        Me.pictMainRear.Location = New System.Drawing.Point(447, 13)
        Me.pictMainRear.Name = "pictMainRear"
        Me.pictMainRear.Origin = New System.Drawing.Point(0, 0)
        Me.pictMainRear.PanButton = System.Windows.Forms.MouseButtons.Left
        Me.pictMainRear.PanMode = True
        Me.pictMainRear.ScrollbarsVisible = True
        Me.pictMainRear.Size = New System.Drawing.Size(438, 246)
        Me.pictMainRear.StretchImageToFit = False
        Me.pictMainRear.TabIndex = 28
        Me.pictMainRear.ZoomFactor = 1.0R
        Me.pictMainRear.ZoomOnMouseWheel = True
        '
        'pictMainFront
        '
        Me.pictMainFront.BackColor = System.Drawing.Color.Transparent
        Me.pictMainFront.Image = Nothing
        Me.pictMainFront.initialimage = Nothing
        Me.pictMainFront.Location = New System.Drawing.Point(6, 13)
        Me.pictMainFront.Name = "pictMainFront"
        Me.pictMainFront.Origin = New System.Drawing.Point(0, 0)
        Me.pictMainFront.PanButton = System.Windows.Forms.MouseButtons.Left
        Me.pictMainFront.PanMode = True
        Me.pictMainFront.ScrollbarsVisible = True
        Me.pictMainFront.Size = New System.Drawing.Size(438, 246)
        Me.pictMainFront.StretchImageToFit = False
        Me.pictMainFront.TabIndex = 27
        Me.pictMainFront.ZoomFactor = 1.0R
        Me.pictMainFront.ZoomOnMouseWheel = True
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.BackColor = System.Drawing.Color.Transparent
        Me.Label11.Location = New System.Drawing.Point(249, 372)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(152, 13)
        Me.Label11.TabIndex = 32
        Me.Label11.Text = "Accepted Automated Cheques"
        '
        'dgvOutCreditMicr
        '
        Me.dgvOutCreditMicr.AllowUserToAddRows = False
        Me.dgvOutCreditMicr.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.dgvOutCreditMicr.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvOutCreditMicr.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvOutCreditMicr.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.dgvOutCreditMicr.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvOutCreditMicr.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colChequeNo, Me.colAmount, Me.colDrawer, Me.colChqDate, Me.colBankID, Me.colBranch, Me.colTheirAcc, Me.colclgDays, Me.colRetCode, Me.colHighValue, Me.colchqdigit, Me.colFIMGImage, Me.colBIMaGe, Me.colUVIMage, Me.colClrCenter, Me.colVoucherCode, Me.colOurComm, Me.colTheirComm, Me.colUniqueID, Me.colTFImageSize, Me.colJFImageSize, Me.colJRImageSize, Me.colAccountID, Me.TFImage, Me.TFImagePath, Me.JFImagePath, Me.JRImagePath, Me.UVImagePath, Me.colmicrline, Me.ColumnID, Me.colOurCommission, Me.colTheirCommission, Me.colTotalCommission, Me.colIsUpcountry, Me.colMinCommRate, Me.colCommRate, Me.ColOurCommRate, Me.colCurrencyID, Me.ColJFImageSignature, Me.ColTFImageSignature, Me.ColJRImageSignature, Me.colBranchName, Me.colBankName, Me.colValueDate, Me.coJRdpi, Me.colTFdpi, Me.colJFdpi})
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvOutCreditMicr.DefaultCellStyle = DataGridViewCellStyle5
        Me.dgvOutCreditMicr.EnableHeadersVisualStyles = False
        Me.dgvOutCreditMicr.Location = New System.Drawing.Point(12, 398)
        Me.dgvOutCreditMicr.Name = "dgvOutCreditMicr"
        Me.dgvOutCreditMicr.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvOutCreditMicr.RowHeadersDefaultCellStyle = DataGridViewCellStyle6
        Me.dgvOutCreditMicr.ShowCellErrors = False
        Me.dgvOutCreditMicr.ShowEditingIcon = False
        Me.dgvOutCreditMicr.ShowRowErrors = False
        Me.dgvOutCreditMicr.Size = New System.Drawing.Size(998, 150)
        Me.dgvOutCreditMicr.TabIndex = 31
        '
        'dgvRejectedItem
        '
        Me.dgvRejectedItem.AllowUserToAddRows = False
        Me.dgvRejectedItem.AllowUserToDeleteRows = False
        Me.dgvRejectedItem.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvRejectedItem.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle7
        Me.dgvRejectedItem.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvRejectedItem.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colchqID, Me.colBnkID, Me.colBrnID, Me.colRejReason, Me.colRejVoucherCode, Me.colRejChqDigit, Me.colRejTheirAccID, Me.colFrontImg, Me.colBackImg, Me.colUVImg, Me.colImageunqID, Me.TFImageBW, Me.colJRImagePath, Me.colTFImagePath, Me.colJFImagePath, Me.colUVImagePath})
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvRejectedItem.DefaultCellStyle = DataGridViewCellStyle8
        Me.dgvRejectedItem.Location = New System.Drawing.Point(12, 554)
        Me.dgvRejectedItem.Name = "dgvRejectedItem"
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvRejectedItem.RowHeadersDefaultCellStyle = DataGridViewCellStyle9
        Me.dgvRejectedItem.Size = New System.Drawing.Size(999, 165)
        Me.dgvRejectedItem.TabIndex = 33
        '
        'colchqID
        '
        Me.colchqID.HeaderText = "Cheque ID"
        Me.colchqID.Name = "colchqID"
        '
        'colBnkID
        '
        Me.colBnkID.HeaderText = "Bank ID"
        Me.colBnkID.Name = "colBnkID"
        '
        'colBrnID
        '
        Me.colBrnID.HeaderText = "Branch ID"
        Me.colBrnID.Name = "colBrnID"
        '
        'colRejReason
        '
        Me.colRejReason.HeaderText = "Rejected Reason"
        Me.colRejReason.Name = "colRejReason"
        Me.colRejReason.Width = 400
        '
        'colRejVoucherCode
        '
        Me.colRejVoucherCode.HeaderText = "VoucherCode"
        Me.colRejVoucherCode.Name = "colRejVoucherCode"
        Me.colRejVoucherCode.Visible = False
        '
        'colRejChqDigit
        '
        Me.colRejChqDigit.HeaderText = "Cheque Digit"
        Me.colRejChqDigit.Name = "colRejChqDigit"
        Me.colRejChqDigit.Visible = False
        '
        'colRejTheirAccID
        '
        Me.colRejTheirAccID.HeaderText = "TheirAccountID"
        Me.colRejTheirAccID.Name = "colRejTheirAccID"
        Me.colRejTheirAccID.Visible = False
        '
        'colFrontImg
        '
        Me.colFrontImg.HeaderText = "FrontImg"
        Me.colFrontImg.Name = "colFrontImg"
        Me.colFrontImg.Visible = False
        '
        'colBackImg
        '
        Me.colBackImg.HeaderText = "BackImg"
        Me.colBackImg.Name = "colBackImg"
        Me.colBackImg.Visible = False
        '
        'colUVImg
        '
        Me.colUVImg.HeaderText = "UVImg"
        Me.colUVImg.Name = "colUVImg"
        Me.colUVImg.Visible = False
        '
        'colImageunqID
        '
        Me.colImageunqID.HeaderText = "ImageUniqueID"
        Me.colImageunqID.Name = "colImageunqID"
        Me.colImageunqID.Visible = False
        '
        'TFImageBW
        '
        Me.TFImageBW.HeaderText = "TFImage"
        Me.TFImageBW.Name = "TFImageBW"
        Me.TFImageBW.Visible = False
        '
        'colJRImagePath
        '
        Me.colJRImagePath.HeaderText = "JRImagePath"
        Me.colJRImagePath.Name = "colJRImagePath"
        Me.colJRImagePath.Visible = False
        '
        'colTFImagePath
        '
        Me.colTFImagePath.HeaderText = "TFImagePath"
        Me.colTFImagePath.Name = "colTFImagePath"
        Me.colTFImagePath.Visible = False
        '
        'colJFImagePath
        '
        Me.colJFImagePath.HeaderText = "JFImagePathy"
        Me.colJFImagePath.Name = "colJFImagePath"
        Me.colJFImagePath.Visible = False
        '
        'colUVImagePath
        '
        Me.colUVImagePath.HeaderText = "UVImagePath"
        Me.colUVImagePath.Name = "colUVImagePath"
        Me.colUVImagePath.Visible = False
        '
        'lblCount
        '
        Me.lblCount.Font = New System.Drawing.Font("Courier New", 9.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCount.Location = New System.Drawing.Point(402, 373)
        Me.lblCount.Name = "lblCount"
        Me.lblCount.Size = New System.Drawing.Size(60, 19)
        Me.lblCount.TabIndex = 35
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.BackColor = System.Drawing.Color.Transparent
        Me.Label12.Location = New System.Drawing.Point(469, 372)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(73, 13)
        Me.Label12.TabIndex = 36
        Me.Label12.Text = "Total Amount:"
        '
        'lblAmount
        '
        Me.lblAmount.Font = New System.Drawing.Font("Courier New", 9.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAmount.Location = New System.Drawing.Point(544, 370)
        Me.lblAmount.Name = "lblAmount"
        Me.lblAmount.Size = New System.Drawing.Size(163, 19)
        Me.lblAmount.TabIndex = 37
        '
        'GroupBox4
        '
        Me.GroupBox4.BackgroundImage = Global.BRCTSScanner.My.Resources.Resources.log_img6
        Me.GroupBox4.Controls.Add(Me.btnOption)
        Me.GroupBox4.Controls.Add(Me.btnDelete)
        Me.GroupBox4.Controls.Add(Me.buttonStartFeed)
        Me.GroupBox4.Controls.Add(Me.buttonStopFeed)
        Me.GroupBox4.Controls.Add(Me.buttonFreeTrack)
        Me.GroupBox4.Controls.Add(Me.textState)
        Me.GroupBox4.Controls.Add(Me.buttonOptions)
        Me.GroupBox4.Controls.Add(Me.buttonExit)
        Me.GroupBox4.Location = New System.Drawing.Point(912, 9)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(103, 359)
        Me.GroupBox4.TabIndex = 38
        Me.GroupBox4.TabStop = False
        '
        'btnOption
        '
        Me.btnOption.Location = New System.Drawing.Point(15, 201)
        Me.btnOption.Name = "btnOption"
        Me.btnOption.Size = New System.Drawing.Size(89, 24)
        Me.btnOption.TabIndex = 36
        Me.btnOption.Text = "MDV"
        Me.btnOption.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(8, 172)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(89, 24)
        Me.btnDelete.TabIndex = 35
        Me.btnDelete.Text = "Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'bgwImageSaver
        '
        '
        'GroupBox5
        '
        Me.GroupBox5.BackgroundImage = Global.BRCTSScanner.My.Resources.Resources.log_img6
        Me.GroupBox5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.GroupBox5.Controls.Add(Me.btnCancel)
        Me.GroupBox5.Controls.Add(Me.btnMDV)
        Me.GroupBox5.Controls.Add(Me.Button2)
        Me.GroupBox5.Controls.Add(Me.Button3)
        Me.GroupBox5.Controls.Add(Me.Button4)
        Me.GroupBox5.Controls.Add(Me.Button5)
        Me.GroupBox5.Controls.Add(Me.btnOk)
        Me.GroupBox5.Controls.Add(Me.Button7)
        Me.GroupBox5.Controls.Add(Me.Button8)
        Me.GroupBox5.Location = New System.Drawing.Point(912, 9)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(103, 321)
        Me.GroupBox5.TabIndex = 38
        Me.GroupBox5.TabStop = False
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(6, 293)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(89, 23)
        Me.btnCancel.TabIndex = 38
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnMDV
        '
        Me.btnMDV.Location = New System.Drawing.Point(8, 192)
        Me.btnMDV.Name = "btnMDV"
        Me.btnMDV.Size = New System.Drawing.Size(89, 24)
        Me.btnMDV.TabIndex = 36
        Me.btnMDV.Text = "MDV"
        Me.btnMDV.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(8, 148)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(89, 24)
        Me.Button2.TabIndex = 35
        Me.Button2.Text = "Delete"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(7, 19)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(90, 24)
        Me.Button3.TabIndex = 7
        Me.Button3.Text = "Start Feeding"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Location = New System.Drawing.Point(7, 62)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(90, 24)
        Me.Button4.TabIndex = 8
        Me.Button4.Text = "Stop Feeding"
        Me.Button4.UseVisualStyleBackColor = True
        Me.Button4.Visible = False
        '
        'Button5
        '
        Me.Button5.Location = New System.Drawing.Point(8, 105)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(89, 24)
        Me.Button5.TabIndex = 9
        Me.Button5.Text = "&FreeTrack"
        Me.Button5.UseVisualStyleBackColor = True
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(6, 234)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(91, 25)
        Me.btnOk.TabIndex = 34
        Me.btnOk.Text = "Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'Button7
        '
        Me.Button7.Location = New System.Drawing.Point(6, 233)
        Me.Button7.Name = "Button7"
        Me.Button7.Size = New System.Drawing.Size(92, 26)
        Me.Button7.TabIndex = 20
        Me.Button7.Text = "&Options"
        Me.Button7.UseVisualStyleBackColor = True
        Me.Button7.Visible = False
        '
        'Button8
        '
        Me.Button8.Location = New System.Drawing.Point(6, 266)
        Me.Button8.Name = "Button8"
        Me.Button8.Size = New System.Drawing.Size(91, 24)
        Me.Button8.TabIndex = 10
        Me.Button8.Text = "Save"
        Me.Button8.UseVisualStyleBackColor = True
        '
        'RBTypeC
        '
        Me.RBTypeC.AutoSize = True
        Me.RBTypeC.BackColor = System.Drawing.Color.Transparent
        Me.RBTypeC.Checked = True
        Me.RBTypeC.Location = New System.Drawing.Point(880, 374)
        Me.RBTypeC.Name = "RBTypeC"
        Me.RBTypeC.Size = New System.Drawing.Size(59, 17)
        Me.RBTypeC.TabIndex = 39
        Me.RBTypeC.TabStop = True
        Me.RBTypeC.Text = "Type C"
        Me.RBTypeC.UseVisualStyleBackColor = False
        Me.RBTypeC.Visible = False
        '
        'RBTypeB
        '
        Me.RBTypeB.AutoSize = True
        Me.RBTypeB.BackColor = System.Drawing.Color.Transparent
        Me.RBTypeB.Location = New System.Drawing.Point(949, 374)
        Me.RBTypeB.Name = "RBTypeB"
        Me.RBTypeB.Size = New System.Drawing.Size(59, 17)
        Me.RBTypeB.TabIndex = 40
        Me.RBTypeB.Text = "Type B"
        Me.RBTypeB.UseVisualStyleBackColor = False
        Me.RBTypeB.Visible = False
        '
        'colChequeNo
        '
        DataGridViewCellStyle3.Format = "d"
        DataGridViewCellStyle3.NullValue = Nothing
        Me.colChequeNo.DefaultCellStyle = DataGridViewCellStyle3
        Me.colChequeNo.HeaderText = "ChequeNo"
        Me.colChequeNo.Name = "colChequeNo"
        Me.colChequeNo.Width = 80
        '
        'colAmount
        '
        Me.colAmount.HeaderText = "Amount"
        Me.colAmount.Name = "colAmount"
        Me.colAmount.Width = 80
        '
        'colDrawer
        '
        Me.colDrawer.HeaderText = "Drawer"
        Me.colDrawer.MaxInputLength = 35
        Me.colDrawer.Name = "colDrawer"
        '
        'colChqDate
        '
        DataGridViewCellStyle4.Format = "d"
        DataGridViewCellStyle4.NullValue = Nothing
        Me.colChqDate.DefaultCellStyle = DataGridViewCellStyle4
        Me.colChqDate.HeaderText = "Cheque Date"
        Me.colChqDate.Name = "colChqDate"
        Me.colChqDate.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.colChqDate.Width = 90
        '
        'colBankID
        '
        Me.colBankID.HeaderText = "Bank"
        Me.colBankID.Name = "colBankID"
        Me.colBankID.Width = 200
        '
        'colBranch
        '
        Me.colBranch.HeaderText = "Branch"
        Me.colBranch.Name = "colBranch"
        Me.colBranch.Width = 200
        '
        'colTheirAcc
        '
        Me.colTheirAcc.HeaderText = "Their Acc."
        Me.colTheirAcc.Name = "colTheirAcc"
        Me.colTheirAcc.Width = 80
        '
        'colclgDays
        '
        Me.colclgDays.HeaderText = "Clearing Days"
        Me.colclgDays.Name = "colclgDays"
        Me.colclgDays.Width = 40
        '
        'colRetCode
        '
        Me.colRetCode.HeaderText = "Return Code"
        Me.colRetCode.Name = "colRetCode"
        Me.colRetCode.ReadOnly = True
        Me.colRetCode.Width = 40
        '
        'colHighValue
        '
        Me.colHighValue.HeaderText = "High Value"
        Me.colHighValue.Name = "colHighValue"
        Me.colHighValue.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.colHighValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.colHighValue.Visible = False
        Me.colHighValue.Width = 40
        '
        'colchqdigit
        '
        Me.colchqdigit.HeaderText = "Cheque Digit"
        Me.colchqdigit.Name = "colchqdigit"
        Me.colchqdigit.Width = 40
        '
        'colFIMGImage
        '
        Me.colFIMGImage.HeaderText = "Front Image "
        Me.colFIMGImage.Name = "colFIMGImage"
        Me.colFIMGImage.Visible = False
        '
        'colBIMaGe
        '
        Me.colBIMaGe.HeaderText = "Back Image"
        Me.colBIMaGe.Name = "colBIMaGe"
        Me.colBIMaGe.Visible = False
        '
        'colUVIMage
        '
        Me.colUVIMage.HeaderText = "UV Image "
        Me.colUVIMage.Name = "colUVIMage"
        Me.colUVIMage.Visible = False
        '
        'colClrCenter
        '
        Me.colClrCenter.HeaderText = "Clr Centre"
        Me.colClrCenter.Name = "colClrCenter"
        Me.colClrCenter.Visible = False
        '
        'colVoucherCode
        '
        Me.colVoucherCode.HeaderText = "Voucher Code"
        Me.colVoucherCode.Name = "colVoucherCode"
        Me.colVoucherCode.Visible = False
        '
        'colOurComm
        '
        Me.colOurComm.HeaderText = "Our Commission"
        Me.colOurComm.Name = "colOurComm"
        Me.colOurComm.Visible = False
        '
        'colTheirComm
        '
        Me.colTheirComm.HeaderText = "Their Commission"
        Me.colTheirComm.Name = "colTheirComm"
        Me.colTheirComm.Visible = False
        '
        'colUniqueID
        '
        Me.colUniqueID.HeaderText = "UniqueID"
        Me.colUniqueID.Name = "colUniqueID"
        Me.colUniqueID.Visible = False
        '
        'colTFImageSize
        '
        Me.colTFImageSize.HeaderText = "TFImageSize"
        Me.colTFImageSize.Name = "colTFImageSize"
        Me.colTFImageSize.Visible = False
        '
        'colJFImageSize
        '
        Me.colJFImageSize.HeaderText = "JFImageSize"
        Me.colJFImageSize.Name = "colJFImageSize"
        Me.colJFImageSize.Visible = False
        '
        'colJRImageSize
        '
        Me.colJRImageSize.HeaderText = "JRImageSize"
        Me.colJRImageSize.Name = "colJRImageSize"
        Me.colJRImageSize.Visible = False
        '
        'colAccountID
        '
        Me.colAccountID.HeaderText = "AccountID"
        Me.colAccountID.Name = "colAccountID"
        '
        'TFImage
        '
        Me.TFImage.HeaderText = "TFImage"
        Me.TFImage.Name = "TFImage"
        Me.TFImage.Visible = False
        '
        'TFImagePath
        '
        Me.TFImagePath.HeaderText = "TFImagePath"
        Me.TFImagePath.Name = "TFImagePath"
        Me.TFImagePath.Visible = False
        '
        'JFImagePath
        '
        Me.JFImagePath.HeaderText = "JFImagePath"
        Me.JFImagePath.Name = "JFImagePath"
        Me.JFImagePath.Visible = False
        '
        'JRImagePath
        '
        Me.JRImagePath.HeaderText = "JRImagePath"
        Me.JRImagePath.Name = "JRImagePath"
        Me.JRImagePath.Visible = False
        '
        'UVImagePath
        '
        Me.UVImagePath.HeaderText = "UVImagePath"
        Me.UVImagePath.Name = "UVImagePath"
        Me.UVImagePath.Visible = False
        '
        'colmicrline
        '
        Me.colmicrline.HeaderText = "micrline"
        Me.colmicrline.Name = "colmicrline"
        Me.colmicrline.Visible = False
        '
        'ColumnID
        '
        Me.ColumnID.HeaderText = "ColumnID"
        Me.ColumnID.Name = "ColumnID"
        Me.ColumnID.Visible = False
        '
        'colOurCommission
        '
        Me.colOurCommission.HeaderText = "OurCommission"
        Me.colOurCommission.Name = "colOurCommission"
        Me.colOurCommission.ReadOnly = True
        '
        'colTheirCommission
        '
        Me.colTheirCommission.HeaderText = "TheirCommission"
        Me.colTheirCommission.Name = "colTheirCommission"
        Me.colTheirCommission.ReadOnly = True
        '
        'colTotalCommission
        '
        Me.colTotalCommission.HeaderText = "Total Commission"
        Me.colTotalCommission.Name = "colTotalCommission"
        Me.colTotalCommission.ReadOnly = True
        '
        'colIsUpcountry
        '
        Me.colIsUpcountry.HeaderText = "Is Upcountry"
        Me.colIsUpcountry.Name = "colIsUpcountry"
        Me.colIsUpcountry.ReadOnly = True
        '
        'colMinCommRate
        '
        Me.colMinCommRate.HeaderText = "MinCommissionRate"
        Me.colMinCommRate.Name = "colMinCommRate"
        Me.colMinCommRate.ReadOnly = True
        Me.colMinCommRate.Visible = False
        '
        'colCommRate
        '
        Me.colCommRate.HeaderText = "CommissionRate"
        Me.colCommRate.Name = "colCommRate"
        Me.colCommRate.ReadOnly = True
        Me.colCommRate.Visible = False
        '
        'ColOurCommRate
        '
        Me.ColOurCommRate.HeaderText = "OurCommRate"
        Me.ColOurCommRate.Name = "ColOurCommRate"
        Me.ColOurCommRate.ReadOnly = True
        Me.ColOurCommRate.Visible = False
        '
        'colCurrencyID
        '
        Me.colCurrencyID.HeaderText = "CurrencyID"
        Me.colCurrencyID.Name = "colCurrencyID"
        Me.colCurrencyID.Visible = False
        '
        'ColJFImageSignature
        '
        Me.ColJFImageSignature.HeaderText = "JFImageSignature"
        Me.ColJFImageSignature.Name = "ColJFImageSignature"
        Me.ColJFImageSignature.ReadOnly = True
        Me.ColJFImageSignature.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.ColJFImageSignature.Visible = False
        '
        'ColTFImageSignature
        '
        Me.ColTFImageSignature.HeaderText = "TFImageSignature"
        Me.ColTFImageSignature.Name = "ColTFImageSignature"
        Me.ColTFImageSignature.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.ColTFImageSignature.Visible = False
        '
        'ColJRImageSignature
        '
        Me.ColJRImageSignature.HeaderText = "JRImageSignature"
        Me.ColJRImageSignature.Name = "ColJRImageSignature"
        Me.ColJRImageSignature.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.ColJRImageSignature.Visible = False
        '
        'colBranchName
        '
        Me.colBranchName.HeaderText = "BranchName"
        Me.colBranchName.Name = "colBranchName"
        Me.colBranchName.Visible = False
        '
        'colBankName
        '
        Me.colBankName.HeaderText = "BankName"
        Me.colBankName.Name = "colBankName"
        Me.colBankName.Visible = False
        '
        'colValueDate
        '
        Me.colValueDate.HeaderText = "ValueDate"
        Me.colValueDate.Name = "colValueDate"
        Me.colValueDate.Visible = False
        '
        'coJRdpi
        '
        Me.coJRdpi.HeaderText = "JRdpi"
        Me.coJRdpi.Name = "coJRdpi"
        '
        'colTFdpi
        '
        Me.colTFdpi.HeaderText = "TFdpi"
        Me.colTFdpi.Name = "colTFdpi"
        Me.colTFdpi.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        '
        'colJFdpi
        '
        Me.colJFdpi.HeaderText = "JFdpi"
        Me.colJFdpi.Name = "colJFdpi"
        Me.colJFdpi.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        '
        'frmBROutwardClearingCTS
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = Global.BRCTSScanner.My.Resources.Resources.log_img6
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(1024, 700)
        Me.Controls.Add(Me.RBTypeB)
        Me.Controls.Add(Me.RBTypeC)
        Me.Controls.Add(Me.GroupBox5)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.txtChqCount)
        Me.Controls.Add(Me.lblAmount)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.lblCount)
        Me.Controls.Add(Me.dgvRejectedItem)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.dgvOutCreditMicr)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.pictPreview4)
        Me.Controls.Add(Me.pictPreview3)
        Me.Controls.Add(Me.lstMessages)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox3)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmBROutwardClearingCTS"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "BR Outward Clearing"
        CType(Me.pictPreview4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pictPreview3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pictPreview2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pictPreview1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        CType(Me.BackGraySPic, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.FrontBWPic, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox3.ResumeLayout(False)
        CType(Me.dgvOutCreditMicr, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvRejectedItem, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents textState As System.Windows.Forms.TextBox
    Friend WithEvents pictPreview4 As System.Windows.Forms.PictureBox
    Friend WithEvents pictPreview3 As System.Windows.Forms.PictureBox
    Friend WithEvents pictPreview2 As System.Windows.Forms.PictureBox
    Friend WithEvents pictPreview1 As System.Windows.Forms.PictureBox
    Friend WithEvents buttonOptions As System.Windows.Forms.Button
    Friend WithEvents buttonStopFeed As System.Windows.Forms.Button
    Friend WithEvents buttonStartFeed As System.Windows.Forms.Button
    Friend WithEvents buttonFreeTrack As System.Windows.Forms.Button
    Friend WithEvents buttonExit As System.Windows.Forms.Button
    Friend WithEvents lstMessages As System.Windows.Forms.ListBox
    Friend WithEvents timerState As System.Windows.Forms.Timer
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents txtClrCenter As System.Windows.Forms.TextBox
    Friend WithEvents txtChqCount As System.Windows.Forms.TextBox
    Public WithEvents txtVoucherCode As System.Windows.Forms.TextBox
    Public WithEvents txtChqDigit As System.Windows.Forms.TextBox
    Public WithEvents txtChqNo As System.Windows.Forms.TextBox
    Public WithEvents txtTheirAccID As System.Windows.Forms.TextBox
    Friend WithEvents txtBranchName As System.Windows.Forms.TextBox
    Public WithEvents txtBranchID As System.Windows.Forms.TextBox
    Friend WithEvents txtBankName As System.Windows.Forms.TextBox
    Public WithEvents txtBankID As System.Windows.Forms.TextBox
    Friend WithEvents txtAccName As System.Windows.Forms.TextBox
    Friend WithEvents txtAccountID As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Public WithEvents dgvOutCreditMicr As System.Windows.Forms.DataGridView
    Friend WithEvents dgvRejectedItem As System.Windows.Forms.DataGridView
    Friend WithEvents lblCount As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents lblAmount As System.Windows.Forms.Label
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents bgwImageSaver As System.ComponentModel.BackgroundWorker
    Friend WithEvents BackGraySPic As System.Windows.Forms.PictureBox
    Friend WithEvents FrontBWPic As System.Windows.Forms.PictureBox
    Friend WithEvents pictMainFront As CSI_ImageControl.ImageControl
    Friend WithEvents pictMainRear As CSI_ImageControl.ImageControl
    Friend WithEvents colchqID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colBnkID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colBrnID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colRejReason As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colRejVoucherCode As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colRejChqDigit As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colRejTheirAccID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colFrontImg As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colBackImg As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colUVImg As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colImageunqID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TFImageBW As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colJRImagePath As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colTFImagePath As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colJFImagePath As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colUVImagePath As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents btnOption As System.Windows.Forms.Button
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents btnMDV As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents Button7 As System.Windows.Forms.Button
    Friend WithEvents Button8 As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents RBTypeC As Windows.Forms.RadioButton
    Friend WithEvents RBTypeB As Windows.Forms.RadioButton
    Friend WithEvents colChequeNo As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colAmount As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colDrawer As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colChqDate As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colBankID As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colBranch As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colTheirAcc As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colclgDays As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colRetCode As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colHighValue As Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents colchqdigit As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colFIMGImage As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colBIMaGe As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colUVIMage As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colClrCenter As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colVoucherCode As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colOurComm As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colTheirComm As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colUniqueID As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colTFImageSize As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colJFImageSize As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colJRImageSize As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colAccountID As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TFImage As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TFImagePath As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents JFImagePath As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents JRImagePath As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents UVImagePath As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colmicrline As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ColumnID As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colOurCommission As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colTheirCommission As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colTotalCommission As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colIsUpcountry As Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents colMinCommRate As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colCommRate As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ColOurCommRate As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colCurrencyID As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ColJFImageSignature As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ColTFImageSignature As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ColJRImageSignature As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colBranchName As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colBankName As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colValueDate As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents coJRdpi As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colTFdpi As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colJFdpi As Windows.Forms.DataGridViewTextBoxColumn
End Class
