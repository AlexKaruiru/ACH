<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBRChequesSearchClearing
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBRChequesSearchClearing))
        Me.ImgCheque = New CSI_ImageControl.ImageControl()
        Me.gfxCheques = New System.Windows.Forms.DataGridView()
        Me.Frame2 = New System.Windows.Forms.GroupBox()
        Me.ImgIconTR = New System.Windows.Forms.PictureBox()
        Me.ImgIconJR = New System.Windows.Forms.PictureBox()
        Me.ImgIconJ = New System.Windows.Forms.PictureBox()
        Me.ImgIconT = New System.Windows.Forms.PictureBox()
        Me.cmdZoom = New System.Windows.Forms.Button()
        Me.cmdFitWdth = New System.Windows.Forms.Button()
        Me.CmdFitHT = New System.Windows.Forms.Button()
        Me.cmdPrint = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.frmSearch = New System.Windows.Forms.GroupBox()
        Me.btnUnpaid = New System.Windows.Forms.Button()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtSAmount = New System.Windows.Forms.TextBox()
        Me.txtFAmount = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.cmbTrxtype = New System.Windows.Forms.ComboBox()
        Me.dtTo = New System.Windows.Forms.DateTimePicker()
        Me.dtChequeDate = New System.Windows.Forms.DateTimePicker()
        Me.cmdReset = New System.Windows.Forms.Button()
        Me.txtBranchID = New System.Windows.Forms.TextBox()
        Me.txtTheirAccID = New System.Windows.Forms.TextBox()
        Me.txtBankID = New System.Windows.Forms.TextBox()
        Me.cmdSearch = New System.Windows.Forms.Button()
        Me.txtChequeID = New System.Windows.Forms.TextBox()
        Me.txtAccount = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtAccName = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.prnDoc = New System.Drawing.Printing.PrintDocument()
        CType(Me.gfxCheques, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Frame2.SuspendLayout()
        CType(Me.ImgIconTR, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ImgIconJR, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ImgIconJ, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ImgIconT, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.frmSearch.SuspendLayout()
        Me.SuspendLayout()
        '
        'ImgCheque
        '
        Me.ImgCheque.Image = Nothing
        Me.ImgCheque.initialimage = Nothing
        Me.ImgCheque.Location = New System.Drawing.Point(4, 269)
        Me.ImgCheque.Name = "ImgCheque"
        Me.ImgCheque.Origin = New System.Drawing.Point(0, 0)
        Me.ImgCheque.PanButton = System.Windows.Forms.MouseButtons.Left
        Me.ImgCheque.PanMode = True
        Me.ImgCheque.ScrollbarsVisible = True
        Me.ImgCheque.Size = New System.Drawing.Size(537, 254)
        Me.ImgCheque.StretchImageToFit = False
        Me.ImgCheque.TabIndex = 39
        Me.ImgCheque.ZoomFactor = 1.0R
        Me.ImgCheque.ZoomOnMouseWheel = True
        '
        'gfxCheques
        '
        Me.gfxCheques.AllowUserToAddRows = False
        Me.gfxCheques.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.gfxCheques.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.gfxCheques.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(237, Byte), Integer), CType(CType(246, Byte), Integer), CType(CType(251, Byte), Integer))
        Me.gfxCheques.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        Me.gfxCheques.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gfxCheques.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.gfxCheques.EnableHeadersVisualStyles = False
        Me.gfxCheques.Location = New System.Drawing.Point(4, 141)
        Me.gfxCheques.Name = "gfxCheques"
        Me.gfxCheques.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        Me.gfxCheques.ShowCellErrors = False
        Me.gfxCheques.ShowEditingIcon = False
        Me.gfxCheques.ShowRowErrors = False
        Me.gfxCheques.Size = New System.Drawing.Size(539, 123)
        Me.gfxCheques.TabIndex = 38
        '
        'Frame2
        '
        Me.Frame2.BackColor = System.Drawing.Color.FromArgb(CType(CType(237, Byte), Integer), CType(CType(246, Byte), Integer), CType(CType(251, Byte), Integer))
        Me.Frame2.BackgroundImage = Global.BRClearing.Common.My.Resources.Resources.log_img6
        Me.Frame2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.Frame2.Controls.Add(Me.ImgIconTR)
        Me.Frame2.Controls.Add(Me.ImgIconJR)
        Me.Frame2.Controls.Add(Me.ImgIconJ)
        Me.Frame2.Controls.Add(Me.ImgIconT)
        Me.Frame2.Controls.Add(Me.cmdZoom)
        Me.Frame2.Controls.Add(Me.cmdFitWdth)
        Me.Frame2.Controls.Add(Me.CmdFitHT)
        Me.Frame2.Controls.Add(Me.cmdPrint)
        Me.Frame2.Controls.Add(Me.cmdCancel)
        Me.Frame2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame2.Location = New System.Drawing.Point(555, 6)
        Me.Frame2.Name = "Frame2"
        Me.Frame2.Padding = New System.Windows.Forms.Padding(0)
        Me.Frame2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame2.Size = New System.Drawing.Size(121, 517)
        Me.Frame2.TabIndex = 37
        Me.Frame2.TabStop = False
        '
        'ImgIconTR
        '
        Me.ImgIconTR.BackgroundImage = CType(resources.GetObject("ImgIconTR.BackgroundImage"), System.Drawing.Image)
        Me.ImgIconTR.Location = New System.Drawing.Point(10, 208)
        Me.ImgIconTR.MaximumSize = New System.Drawing.Size(100, 50)
        Me.ImgIconTR.MinimumSize = New System.Drawing.Size(100, 50)
        Me.ImgIconTR.Name = "ImgIconTR"
        Me.ImgIconTR.Size = New System.Drawing.Size(100, 50)
        Me.ImgIconTR.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.ImgIconTR.TabIndex = 25
        Me.ImgIconTR.TabStop = False
        '
        'ImgIconJR
        '
        Me.ImgIconJR.BackgroundImage = CType(resources.GetObject("ImgIconJR.BackgroundImage"), System.Drawing.Image)
        Me.ImgIconJR.Location = New System.Drawing.Point(10, 146)
        Me.ImgIconJR.MaximumSize = New System.Drawing.Size(100, 50)
        Me.ImgIconJR.MinimumSize = New System.Drawing.Size(100, 50)
        Me.ImgIconJR.Name = "ImgIconJR"
        Me.ImgIconJR.Size = New System.Drawing.Size(100, 50)
        Me.ImgIconJR.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.ImgIconJR.TabIndex = 24
        Me.ImgIconJR.TabStop = False
        '
        'ImgIconJ
        '
        Me.ImgIconJ.BackgroundImage = CType(resources.GetObject("ImgIconJ.BackgroundImage"), System.Drawing.Image)
        Me.ImgIconJ.Location = New System.Drawing.Point(10, 79)
        Me.ImgIconJ.MaximumSize = New System.Drawing.Size(100, 50)
        Me.ImgIconJ.MinimumSize = New System.Drawing.Size(100, 50)
        Me.ImgIconJ.Name = "ImgIconJ"
        Me.ImgIconJ.Size = New System.Drawing.Size(100, 50)
        Me.ImgIconJ.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.ImgIconJ.TabIndex = 23
        Me.ImgIconJ.TabStop = False
        '
        'ImgIconT
        '
        Me.ImgIconT.BackgroundImage = CType(resources.GetObject("ImgIconT.BackgroundImage"), System.Drawing.Image)
        Me.ImgIconT.Location = New System.Drawing.Point(8, 12)
        Me.ImgIconT.MaximumSize = New System.Drawing.Size(100, 50)
        Me.ImgIconT.MinimumSize = New System.Drawing.Size(100, 50)
        Me.ImgIconT.Name = "ImgIconT"
        Me.ImgIconT.Size = New System.Drawing.Size(100, 50)
        Me.ImgIconT.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.ImgIconT.TabIndex = 22
        Me.ImgIconT.TabStop = False
        '
        'cmdZoom
        '
        Me.cmdZoom.BackColor = System.Drawing.Color.WhiteSmoke
        Me.cmdZoom.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdZoom.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdZoom.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdZoom.Location = New System.Drawing.Point(14, 264)
        Me.cmdZoom.Name = "cmdZoom"
        Me.cmdZoom.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdZoom.Size = New System.Drawing.Size(97, 34)
        Me.cmdZoom.TabIndex = 0
        Me.cmdZoom.Text = "Zoom &In"
        Me.cmdZoom.UseVisualStyleBackColor = False
        '
        'cmdFitWdth
        '
        Me.cmdFitWdth.BackColor = System.Drawing.Color.WhiteSmoke
        Me.cmdFitWdth.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdFitWdth.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFitWdth.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFitWdth.Location = New System.Drawing.Point(14, 315)
        Me.cmdFitWdth.Name = "cmdFitWdth"
        Me.cmdFitWdth.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFitWdth.Size = New System.Drawing.Size(97, 34)
        Me.cmdFitWdth.TabIndex = 1
        Me.cmdFitWdth.Text = "Zoom &Out"
        Me.cmdFitWdth.UseVisualStyleBackColor = False
        '
        'CmdFitHT
        '
        Me.CmdFitHT.BackColor = System.Drawing.Color.WhiteSmoke
        Me.CmdFitHT.Cursor = System.Windows.Forms.Cursors.Default
        Me.CmdFitHT.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdFitHT.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdFitHT.Location = New System.Drawing.Point(13, 366)
        Me.CmdFitHT.Name = "CmdFitHT"
        Me.CmdFitHT.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CmdFitHT.Size = New System.Drawing.Size(97, 34)
        Me.CmdFitHT.TabIndex = 2
        Me.CmdFitHT.Text = "&Fit"
        Me.CmdFitHT.UseVisualStyleBackColor = False
        '
        'cmdPrint
        '
        Me.cmdPrint.BackColor = System.Drawing.Color.WhiteSmoke
        Me.cmdPrint.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdPrint.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPrint.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPrint.Location = New System.Drawing.Point(13, 417)
        Me.cmdPrint.Name = "cmdPrint"
        Me.cmdPrint.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPrint.Size = New System.Drawing.Size(97, 34)
        Me.cmdPrint.TabIndex = 3
        Me.cmdPrint.Text = "&Print"
        Me.cmdPrint.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.Color.WhiteSmoke
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(13, 468)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(97, 34)
        Me.cmdCancel.TabIndex = 4
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'frmSearch
        '
        Me.frmSearch.BackColor = System.Drawing.Color.FromArgb(CType(CType(237, Byte), Integer), CType(CType(246, Byte), Integer), CType(CType(251, Byte), Integer))
        Me.frmSearch.BackgroundImage = Global.BRClearing.Common.My.Resources.Resources.log_img6
        Me.frmSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.frmSearch.Controls.Add(Me.btnUnpaid)
        Me.frmSearch.Controls.Add(Me.Label10)
        Me.frmSearch.Controls.Add(Me.txtSAmount)
        Me.frmSearch.Controls.Add(Me.txtFAmount)
        Me.frmSearch.Controls.Add(Me.Label9)
        Me.frmSearch.Controls.Add(Me.cmbTrxtype)
        Me.frmSearch.Controls.Add(Me.dtTo)
        Me.frmSearch.Controls.Add(Me.dtChequeDate)
        Me.frmSearch.Controls.Add(Me.cmdReset)
        Me.frmSearch.Controls.Add(Me.txtBranchID)
        Me.frmSearch.Controls.Add(Me.txtTheirAccID)
        Me.frmSearch.Controls.Add(Me.txtBankID)
        Me.frmSearch.Controls.Add(Me.cmdSearch)
        Me.frmSearch.Controls.Add(Me.txtChequeID)
        Me.frmSearch.Controls.Add(Me.txtAccount)
        Me.frmSearch.Controls.Add(Me.Label6)
        Me.frmSearch.Controls.Add(Me.Label5)
        Me.frmSearch.Controls.Add(Me.Label4)
        Me.frmSearch.Controls.Add(Me.Label7)
        Me.frmSearch.Controls.Add(Me.Label3)
        Me.frmSearch.Controls.Add(Me.Label2)
        Me.frmSearch.Controls.Add(Me.Label1)
        Me.frmSearch.Controls.Add(Me.Label8)
        Me.frmSearch.Controls.Add(Me.txtAccName)
        Me.frmSearch.Controls.Add(Me.Label12)
        Me.frmSearch.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.frmSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.frmSearch.Location = New System.Drawing.Point(4, 6)
        Me.frmSearch.Name = "frmSearch"
        Me.frmSearch.Padding = New System.Windows.Forms.Padding(0)
        Me.frmSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.frmSearch.Size = New System.Drawing.Size(545, 129)
        Me.frmSearch.TabIndex = 36
        Me.frmSearch.TabStop = False
        Me.frmSearch.Text = "Search Cheques"
        '
        'btnUnpaid
        '
        Me.btnUnpaid.BackColor = System.Drawing.Color.WhiteSmoke
        Me.btnUnpaid.Location = New System.Drawing.Point(465, 100)
        Me.btnUnpaid.Name = "btnUnpaid"
        Me.btnUnpaid.Size = New System.Drawing.Size(72, 24)
        Me.btnUnpaid.TabIndex = 12
        Me.btnUnpaid.UseVisualStyleBackColor = False
        Me.btnUnpaid.Visible = False
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.BackColor = System.Drawing.Color.Transparent
        Me.Label10.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(223, 79)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(32, 14)
        Me.Label10.TabIndex = 38
        Me.Label10.Text = "And "
        '
        'txtSAmount
        '
        Me.txtSAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtSAmount.Location = New System.Drawing.Point(266, 74)
        Me.txtSAmount.Name = "txtSAmount"
        Me.txtSAmount.Size = New System.Drawing.Size(93, 20)
        Me.txtSAmount.TabIndex = 7
        Me.txtSAmount.Text = "999999"
        Me.txtSAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtFAmount
        '
        Me.txtFAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtFAmount.Location = New System.Drawing.Point(112, 73)
        Me.txtFAmount.Name = "txtFAmount"
        Me.txtFAmount.Size = New System.Drawing.Size(79, 20)
        Me.txtFAmount.TabIndex = 6
        Me.txtFAmount.Text = "0"
        Me.txtFAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.BackColor = System.Drawing.Color.Transparent
        Me.Label9.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(30, 77)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(71, 14)
        Me.Label9.TabIndex = 35
        Me.Label9.Text = "Amnt Btwn:"
        '
        'cmbTrxtype
        '
        Me.cmbTrxtype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTrxtype.FormattingEnabled = True
        Me.cmbTrxtype.Items.AddRange(New Object() {"Outwards Clearing", "Inwards Clearing", "InterBranch Clearing"})
        Me.cmbTrxtype.Location = New System.Drawing.Point(444, 73)
        Me.cmbTrxtype.Name = "cmbTrxtype"
        Me.cmbTrxtype.Size = New System.Drawing.Size(93, 22)
        Me.cmbTrxtype.TabIndex = 5
        '
        'dtTo
        '
        Me.dtTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtTo.Location = New System.Drawing.Point(266, 101)
        Me.dtTo.Name = "dtTo"
        Me.dtTo.Size = New System.Drawing.Size(93, 20)
        Me.dtTo.TabIndex = 10
        Me.dtTo.Value = New Date(2011, 6, 7, 0, 0, 0, 0)
        '
        'dtChequeDate
        '
        Me.dtChequeDate.Location = New System.Drawing.Point(112, 99)
        Me.dtChequeDate.Name = "dtChequeDate"
        Me.dtChequeDate.Size = New System.Drawing.Size(79, 20)
        Me.dtChequeDate.TabIndex = 9
        '
        'cmdReset
        '
        Me.cmdReset.BackColor = System.Drawing.Color.WhiteSmoke
        Me.cmdReset.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReset.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReset.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReset.Location = New System.Drawing.Point(465, 100)
        Me.cmdReset.Name = "cmdReset"
        Me.cmdReset.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReset.Size = New System.Drawing.Size(72, 25)
        Me.cmdReset.TabIndex = 30
        Me.cmdReset.Text = "&Reset"
        Me.cmdReset.UseVisualStyleBackColor = False
        '
        'txtBranchID
        '
        Me.txtBranchID.AcceptsReturn = True
        Me.txtBranchID.BackColor = System.Drawing.SystemColors.Window
        Me.txtBranchID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtBranchID.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBranchID.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBranchID.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBranchID.Location = New System.Drawing.Point(444, 44)
        Me.txtBranchID.MaxLength = 0
        Me.txtBranchID.Name = "txtBranchID"
        Me.txtBranchID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBranchID.Size = New System.Drawing.Size(91, 20)
        Me.txtBranchID.TabIndex = 4
        '
        'txtTheirAccID
        '
        Me.txtTheirAccID.AcceptsReturn = True
        Me.txtTheirAccID.BackColor = System.Drawing.SystemColors.Window
        Me.txtTheirAccID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTheirAccID.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTheirAccID.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTheirAccID.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTheirAccID.Location = New System.Drawing.Point(266, 16)
        Me.txtTheirAccID.MaxLength = 0
        Me.txtTheirAccID.Name = "txtTheirAccID"
        Me.txtTheirAccID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTheirAccID.Size = New System.Drawing.Size(98, 20)
        Me.txtTheirAccID.TabIndex = 1
        '
        'txtBankID
        '
        Me.txtBankID.AcceptsReturn = True
        Me.txtBankID.BackColor = System.Drawing.SystemColors.Window
        Me.txtBankID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtBankID.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBankID.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBankID.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBankID.Location = New System.Drawing.Point(266, 44)
        Me.txtBankID.MaxLength = 0
        Me.txtBankID.Name = "txtBankID"
        Me.txtBankID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBankID.Size = New System.Drawing.Size(98, 20)
        Me.txtBankID.TabIndex = 3
        '
        'cmdSearch
        '
        Me.cmdSearch.BackColor = System.Drawing.Color.WhiteSmoke
        Me.cmdSearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSearch.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSearch.Location = New System.Drawing.Point(387, 101)
        Me.cmdSearch.Name = "cmdSearch"
        Me.cmdSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSearch.Size = New System.Drawing.Size(72, 24)
        Me.cmdSearch.TabIndex = 11
        Me.cmdSearch.Text = "&Search"
        Me.cmdSearch.UseVisualStyleBackColor = False
        '
        'txtChequeID
        '
        Me.txtChequeID.AcceptsReturn = True
        Me.txtChequeID.BackColor = System.Drawing.SystemColors.Window
        Me.txtChequeID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtChequeID.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtChequeID.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtChequeID.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtChequeID.Location = New System.Drawing.Point(111, 46)
        Me.txtChequeID.MaxLength = 0
        Me.txtChequeID.Name = "txtChequeID"
        Me.txtChequeID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtChequeID.Size = New System.Drawing.Size(96, 20)
        Me.txtChequeID.TabIndex = 2
        '
        'txtAccount
        '
        Me.txtAccount.AcceptsReturn = True
        Me.txtAccount.BackColor = System.Drawing.SystemColors.Window
        Me.txtAccount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtAccount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAccount.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAccount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAccount.Location = New System.Drawing.Point(111, 16)
        Me.txtAccount.MaxLength = 0
        Me.txtAccount.Name = "txtAccount"
        Me.txtAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAccount.Size = New System.Drawing.Size(64, 20)
        Me.txtAccount.TabIndex = 0
        '
        'Label6
        '
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label6.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label6.Location = New System.Drawing.Point(370, 47)
        Me.Label6.Name = "Label6"
        Me.Label6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label6.Size = New System.Drawing.Size(63, 19)
        Me.Label6.TabIndex = 28
        Me.Label6.Text = "Branch ID:"
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label5.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label5.Location = New System.Drawing.Point(206, 17)
        Me.Label5.Name = "Label5"
        Me.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label5.Size = New System.Drawing.Size(62, 19)
        Me.Label5.TabIndex = 26
        Me.Label5.Text = "Their Acc:"
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label4.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Location = New System.Drawing.Point(206, 103)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(55, 19)
        Me.Label4.TabIndex = 24
        Me.Label4.Text = "To Date :"
        '
        'Label7
        '
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label7.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label7.Location = New System.Drawing.Point(217, 47)
        Me.Label7.Name = "Label7"
        Me.Label7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label7.Size = New System.Drawing.Size(51, 19)
        Me.Label7.TabIndex = 22
        Me.Label7.Text = "Bank ID:"
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(30, 100)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(71, 19)
        Me.Label3.TabIndex = 16
        Me.Label3.Text = "From Date :"
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(47, 48)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(54, 19)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Cheque:"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(33, 18)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(72, 19)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Account ID:"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(374, 77)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(56, 14)
        Me.Label8.TabIndex = 34
        Me.Label8.Text = "Trx Type:"
        '
        'txtAccName
        '
        Me.txtAccName.BackColor = System.Drawing.Color.White
        Me.txtAccName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtAccName.Enabled = False
        Me.txtAccName.Location = New System.Drawing.Point(257, 15)
        Me.txtAccName.Name = "txtAccName"
        Me.txtAccName.Size = New System.Drawing.Size(282, 20)
        Me.txtAccName.TabIndex = 42
        Me.txtAccName.Visible = False
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.BackColor = System.Drawing.Color.Transparent
        Me.Label12.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(181, 18)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(67, 14)
        Me.Label12.TabIndex = 41
        Me.Label12.Text = "Acc. Name:"
        Me.Label12.Visible = False
        '
        'prnDoc
        '
        '
        'frmBRChequesSearchClearing
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(237, Byte), Integer), CType(CType(246, Byte), Integer), CType(CType(251, Byte), Integer))
        Me.BackgroundImage = Global.BRClearing.Common.My.Resources.Resources.log_img6
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(680, 529)
        Me.Controls.Add(Me.ImgCheque)
        Me.Controls.Add(Me.gfxCheques)
        Me.Controls.Add(Me.Frame2)
        Me.Controls.Add(Me.frmSearch)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmBRChequesSearchClearing"
        CType(Me.gfxCheques, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Frame2.ResumeLayout(False)
        CType(Me.ImgIconTR, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ImgIconJR, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ImgIconJ, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ImgIconT, System.ComponentModel.ISupportInitialize).EndInit()
        Me.frmSearch.ResumeLayout(False)
        Me.frmSearch.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ImgCheque As CSI_ImageControl.ImageControl
    Public WithEvents gfxCheques As System.Windows.Forms.DataGridView
    Public WithEvents Frame2 As System.Windows.Forms.GroupBox
    Friend WithEvents ImgIconTR As System.Windows.Forms.PictureBox
    Friend WithEvents ImgIconJR As System.Windows.Forms.PictureBox
    Friend WithEvents ImgIconJ As System.Windows.Forms.PictureBox
    Friend WithEvents ImgIconT As System.Windows.Forms.PictureBox
    Public WithEvents cmdZoom As System.Windows.Forms.Button
    Public WithEvents cmdFitWdth As System.Windows.Forms.Button
    Public WithEvents CmdFitHT As System.Windows.Forms.Button
    Public WithEvents cmdPrint As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents frmSearch As System.Windows.Forms.GroupBox
    Friend WithEvents btnUnpaid As System.Windows.Forms.Button
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtSAmount As System.Windows.Forms.TextBox
    Friend WithEvents txtFAmount As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents cmbTrxtype As System.Windows.Forms.ComboBox
    Friend WithEvents dtTo As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtChequeDate As System.Windows.Forms.DateTimePicker
    Public WithEvents cmdReset As System.Windows.Forms.Button
    Public WithEvents txtBranchID As System.Windows.Forms.TextBox
    Public WithEvents txtTheirAccID As System.Windows.Forms.TextBox
    Public WithEvents txtBankID As System.Windows.Forms.TextBox
    Public WithEvents cmdSearch As System.Windows.Forms.Button
    Public WithEvents txtChequeID As System.Windows.Forms.TextBox
    Public WithEvents txtAccount As System.Windows.Forms.TextBox
    Public WithEvents Label6 As System.Windows.Forms.Label
    Public WithEvents Label5 As System.Windows.Forms.Label
    Public WithEvents Label4 As System.Windows.Forms.Label
    Public WithEvents Label7 As System.Windows.Forms.Label
    Public WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtAccName As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents prnDoc As System.Drawing.Printing.PrintDocument
End Class
