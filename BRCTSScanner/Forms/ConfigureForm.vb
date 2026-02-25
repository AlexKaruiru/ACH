Public Class ConfigureForm
    Inherits System.Windows.Forms.Form

    Protected m_objConfig As Properties = Nothing

#Region " Create code by Windwos Form Designer "

    Public Sub New()
        MyBase.New()

        'Required by the Windows Form Designer
        InitializeComponent()

        ' InitializeComponent() 
        m_objConfig = New Properties
        Inittab()

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnApply As System.Windows.Forms.Button
    Friend WithEvents TabCtrl As System.Windows.Forms.TabControl
    Friend WithEvents tabImage As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents ComboBoxScanFunc As System.Windows.Forms.ComboBox
    Friend WithEvents tabEndorse As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents tabPaperMisInsert As System.Windows.Forms.TabPage
    Friend WithEvents tabNoise As System.Windows.Forms.TabPage
    Friend WithEvents tabDoubleFeed As System.Windows.Forms.TabPage
    Friend WithEvents tabBaddata As System.Windows.Forms.TabPage
    Friend WithEvents tabNodata As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents CheckBoxMisDetect As System.Windows.Forms.CheckBox
    Friend WithEvents ComboBoxMisEject As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents CheckBoxMisStamp As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxMisCancel As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents CheckBoxNoiseCancel As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxNoiseStamp As System.Windows.Forms.CheckBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ComboBoxNoiseEject As System.Windows.Forms.ComboBox
    Friend WithEvents CheckBoxNoiseDetect As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents CheckBoxDFCancel As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxDFStamp As System.Windows.Forms.CheckBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ComboBoxDFEject As System.Windows.Forms.ComboBox
    Friend WithEvents CheckBoxDFDetect As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Friend WithEvents CheckBoxBadCancel As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxBadStamp As System.Windows.Forms.CheckBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents ComboBoxBadEject As System.Windows.Forms.ComboBox
    Friend WithEvents CheckBoxBadDetect As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox7 As System.Windows.Forms.GroupBox
    Friend WithEvents CheckBoxNoCancel As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxNoStamp As System.Windows.Forms.CheckBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents ComboBoxNoEject As System.Windows.Forms.ComboBox
    Friend WithEvents CheckBoxNoDetect As System.Windows.Forms.CheckBox
    Friend WithEvents tabConfirmation As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox8 As System.Windows.Forms.GroupBox
    Friend WithEvents CheckBoxConfirmation As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox9 As System.Windows.Forms.GroupBox
    Friend WithEvents CheckBoxFrontDisplay As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxFrontSave As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox10 As System.Windows.Forms.GroupBox
    Friend WithEvents CheckBoxFrontGrayscale As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxBackScan As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxBackGrayscale As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxBackDisplay As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxBackSave As System.Windows.Forms.CheckBox
    Friend WithEvents tabMicr As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox11 As System.Windows.Forms.GroupBox
    Friend WithEvents CheckBoxMicr As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxMicrSave As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxFrontScan As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxElecEndorseText As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxElecEndorseImage As System.Windows.Forms.CheckBox
    Friend WithEvents ButtonSetValues As System.Windows.Forms.Button
    Friend WithEvents CheckBoxNoCall As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxRun As System.Windows.Forms.CheckBox
    Friend WithEvents ComboBoxMicrFont As System.Windows.Forms.ComboBox
    Friend WithEvents TabOcrAb As System.Windows.Forms.TabPage
    Friend WithEvents TabBuzzer As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox12 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox13 As System.Windows.Forms.GroupBox
    Friend WithEvents CheckBoxOcrAb As System.Windows.Forms.CheckBox
    Friend WithEvents ComboBoxOcrAbFont As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents ComboBoxSuccessHz As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBoxSuccessCount As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBoxErrorCount As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBoxErrorHz As System.Windows.Forms.ComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents ComboBoxWFeedCount As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBoxWFeedHz As System.Windows.Forms.ComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents TextBoxBadCount As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(ConfigureForm))
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnApply = New System.Windows.Forms.Button
        Me.TabCtrl = New System.Windows.Forms.TabControl
        Me.tabImage = New System.Windows.Forms.TabPage
        Me.GroupBox9 = New System.Windows.Forms.GroupBox
        Me.CheckBoxFrontScan = New System.Windows.Forms.CheckBox
        Me.CheckBoxFrontGrayscale = New System.Windows.Forms.CheckBox
        Me.CheckBoxFrontDisplay = New System.Windows.Forms.CheckBox
        Me.CheckBoxFrontSave = New System.Windows.Forms.CheckBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.ComboBoxScanFunc = New System.Windows.Forms.ComboBox
        Me.GroupBox10 = New System.Windows.Forms.GroupBox
        Me.CheckBoxBackScan = New System.Windows.Forms.CheckBox
        Me.CheckBoxBackGrayscale = New System.Windows.Forms.CheckBox
        Me.CheckBoxBackDisplay = New System.Windows.Forms.CheckBox
        Me.CheckBoxBackSave = New System.Windows.Forms.CheckBox
        Me.TabOcrAb = New System.Windows.Forms.TabPage
        Me.GroupBox12 = New System.Windows.Forms.GroupBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.ComboBoxOcrAbFont = New System.Windows.Forms.ComboBox
        Me.CheckBoxOcrAb = New System.Windows.Forms.CheckBox
        Me.tabEndorse = New System.Windows.Forms.TabPage
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.CheckBoxElecEndorseText = New System.Windows.Forms.CheckBox
        Me.CheckBoxElecEndorseImage = New System.Windows.Forms.CheckBox
        Me.tabConfirmation = New System.Windows.Forms.TabPage
        Me.GroupBox8 = New System.Windows.Forms.GroupBox
        Me.ButtonSetValues = New System.Windows.Forms.Button
        Me.CheckBoxNoCall = New System.Windows.Forms.CheckBox
        Me.CheckBoxRun = New System.Windows.Forms.CheckBox
        Me.CheckBoxConfirmation = New System.Windows.Forms.CheckBox
        Me.tabPaperMisInsert = New System.Windows.Forms.TabPage
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.CheckBoxMisCancel = New System.Windows.Forms.CheckBox
        Me.CheckBoxMisStamp = New System.Windows.Forms.CheckBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.ComboBoxMisEject = New System.Windows.Forms.ComboBox
        Me.CheckBoxMisDetect = New System.Windows.Forms.CheckBox
        Me.tabNoise = New System.Windows.Forms.TabPage
        Me.GroupBox4 = New System.Windows.Forms.GroupBox
        Me.CheckBoxNoiseCancel = New System.Windows.Forms.CheckBox
        Me.CheckBoxNoiseStamp = New System.Windows.Forms.CheckBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.ComboBoxNoiseEject = New System.Windows.Forms.ComboBox
        Me.CheckBoxNoiseDetect = New System.Windows.Forms.CheckBox
        Me.tabDoubleFeed = New System.Windows.Forms.TabPage
        Me.GroupBox5 = New System.Windows.Forms.GroupBox
        Me.CheckBoxDFCancel = New System.Windows.Forms.CheckBox
        Me.CheckBoxDFStamp = New System.Windows.Forms.CheckBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.ComboBoxDFEject = New System.Windows.Forms.ComboBox
        Me.CheckBoxDFDetect = New System.Windows.Forms.CheckBox
        Me.tabBaddata = New System.Windows.Forms.TabPage
        Me.GroupBox6 = New System.Windows.Forms.GroupBox
        Me.TextBoxBadCount = New System.Windows.Forms.TextBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.CheckBoxBadCancel = New System.Windows.Forms.CheckBox
        Me.CheckBoxBadStamp = New System.Windows.Forms.CheckBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.ComboBoxBadEject = New System.Windows.Forms.ComboBox
        Me.CheckBoxBadDetect = New System.Windows.Forms.CheckBox
        Me.tabNodata = New System.Windows.Forms.TabPage
        Me.GroupBox7 = New System.Windows.Forms.GroupBox
        Me.CheckBoxNoCancel = New System.Windows.Forms.CheckBox
        Me.CheckBoxNoStamp = New System.Windows.Forms.CheckBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.ComboBoxNoEject = New System.Windows.Forms.ComboBox
        Me.CheckBoxNoDetect = New System.Windows.Forms.CheckBox
        Me.tabMicr = New System.Windows.Forms.TabPage
        Me.GroupBox11 = New System.Windows.Forms.GroupBox
        Me.ComboBoxMicrFont = New System.Windows.Forms.ComboBox
        Me.CheckBoxMicrSave = New System.Windows.Forms.CheckBox
        Me.CheckBoxMicr = New System.Windows.Forms.CheckBox
        Me.TabBuzzer = New System.Windows.Forms.TabPage
        Me.GroupBox13 = New System.Windows.Forms.GroupBox
        Me.ComboBoxWFeedCount = New System.Windows.Forms.ComboBox
        Me.ComboBoxWFeedHz = New System.Windows.Forms.ComboBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.ComboBoxErrorCount = New System.Windows.Forms.ComboBox
        Me.ComboBoxErrorHz = New System.Windows.Forms.ComboBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.ComboBoxSuccessCount = New System.Windows.Forms.ComboBox
        Me.ComboBoxSuccessHz = New System.Windows.Forms.ComboBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.TabCtrl.SuspendLayout()
        Me.tabImage.SuspendLayout()
        Me.GroupBox9.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox10.SuspendLayout()
        Me.TabOcrAb.SuspendLayout()
        Me.GroupBox12.SuspendLayout()
        Me.tabEndorse.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.tabConfirmation.SuspendLayout()
        Me.GroupBox8.SuspendLayout()
        Me.tabPaperMisInsert.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.tabNoise.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.tabDoubleFeed.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.tabBaddata.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        Me.tabNodata.SuspendLayout()
        Me.GroupBox7.SuspendLayout()
        Me.tabMicr.SuspendLayout()
        Me.GroupBox11.SuspendLayout()
        Me.TabBuzzer.SuspendLayout()
        Me.GroupBox13.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(8, 264)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.TabIndex = 0
        Me.btnOK.Text = "OK"
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(88, 264)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.TabIndex = 0
        Me.btnCancel.Text = "Cancel"
        '
        'btnApply
        '
        Me.btnApply.Location = New System.Drawing.Point(168, 264)
        Me.btnApply.Name = "btnApply"
        Me.btnApply.TabIndex = 0
        Me.btnApply.Text = "Apply"
        '
        'TabCtrl
        '
        Me.TabCtrl.Controls.Add(Me.tabImage)
        Me.TabCtrl.Controls.Add(Me.TabOcrAb)
        Me.TabCtrl.Controls.Add(Me.tabEndorse)
        Me.TabCtrl.Controls.Add(Me.tabConfirmation)
        Me.TabCtrl.Controls.Add(Me.tabPaperMisInsert)
        Me.TabCtrl.Controls.Add(Me.tabNoise)
        Me.TabCtrl.Controls.Add(Me.tabDoubleFeed)
        Me.TabCtrl.Controls.Add(Me.tabBaddata)
        Me.TabCtrl.Controls.Add(Me.tabNodata)
        Me.TabCtrl.Controls.Add(Me.tabMicr)
        Me.TabCtrl.Controls.Add(Me.TabBuzzer)
        Me.TabCtrl.Location = New System.Drawing.Point(8, 8)
        Me.TabCtrl.Multiline = True
        Me.TabCtrl.Name = "TabCtrl"
        Me.TabCtrl.SelectedIndex = 0
        Me.TabCtrl.Size = New System.Drawing.Size(240, 240)
        Me.TabCtrl.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight
        Me.TabCtrl.TabIndex = 1
        '
        'tabImage
        '
        Me.tabImage.Controls.Add(Me.GroupBox9)
        Me.tabImage.Controls.Add(Me.GroupBox1)
        Me.tabImage.Controls.Add(Me.GroupBox10)
        Me.tabImage.Location = New System.Drawing.Point(4, 55)
        Me.tabImage.Name = "tabImage"
        Me.tabImage.Size = New System.Drawing.Size(232, 181)
        Me.tabImage.TabIndex = 0
        Me.tabImage.Text = "Image"
        '
        'GroupBox9
        '
        Me.GroupBox9.Controls.Add(Me.CheckBoxFrontScan)
        Me.GroupBox9.Controls.Add(Me.CheckBoxFrontGrayscale)
        Me.GroupBox9.Controls.Add(Me.CheckBoxFrontDisplay)
        Me.GroupBox9.Controls.Add(Me.CheckBoxFrontSave)
        Me.GroupBox9.Location = New System.Drawing.Point(0, 64)
        Me.GroupBox9.Name = "GroupBox9"
        Me.GroupBox9.Size = New System.Drawing.Size(112, 112)
        Me.GroupBox9.TabIndex = 1
        Me.GroupBox9.TabStop = False
        Me.GroupBox9.Text = "Front"
        '
        'CheckBoxFrontScan
        '
        Me.CheckBoxFrontScan.Location = New System.Drawing.Point(16, 16)
        Me.CheckBoxFrontScan.Name = "CheckBoxFrontScan"
        Me.CheckBoxFrontScan.Size = New System.Drawing.Size(64, 16)
        Me.CheckBoxFrontScan.TabIndex = 0
        Me.CheckBoxFrontScan.Text = "Scan"
        '
        'CheckBoxFrontGrayscale
        '
        Me.CheckBoxFrontGrayscale.Location = New System.Drawing.Point(16, 40)
        Me.CheckBoxFrontGrayscale.Name = "CheckBoxFrontGrayscale"
        Me.CheckBoxFrontGrayscale.Size = New System.Drawing.Size(80, 16)
        Me.CheckBoxFrontGrayscale.TabIndex = 0
        Me.CheckBoxFrontGrayscale.Text = "Grayscale"
        '
        'CheckBoxFrontDisplay
        '
        Me.CheckBoxFrontDisplay.Location = New System.Drawing.Point(16, 64)
        Me.CheckBoxFrontDisplay.Name = "CheckBoxFrontDisplay"
        Me.CheckBoxFrontDisplay.Size = New System.Drawing.Size(64, 16)
        Me.CheckBoxFrontDisplay.TabIndex = 0
        Me.CheckBoxFrontDisplay.Text = "Display"
        '
        'CheckBoxFrontSave
        '
        Me.CheckBoxFrontSave.Location = New System.Drawing.Point(16, 88)
        Me.CheckBoxFrontSave.Name = "CheckBoxFrontSave"
        Me.CheckBoxFrontSave.Size = New System.Drawing.Size(80, 16)
        Me.CheckBoxFrontSave.TabIndex = 0
        Me.CheckBoxFrontSave.Text = "Save"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.ComboBoxScanFunc)
        Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(232, 64)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Scanner"
        '
        'ComboBoxScanFunc
        '
        Me.ComboBoxScanFunc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxScanFunc.Items.AddRange(New Object() {"Multi Scan", "Single Scan"})
        Me.ComboBoxScanFunc.Location = New System.Drawing.Point(48, 24)
        Me.ComboBoxScanFunc.Name = "ComboBoxScanFunc"
        Me.ComboBoxScanFunc.Size = New System.Drawing.Size(121, 20)
        Me.ComboBoxScanFunc.TabIndex = 0
        '
        'GroupBox10
        '
        Me.GroupBox10.Controls.Add(Me.CheckBoxBackScan)
        Me.GroupBox10.Controls.Add(Me.CheckBoxBackGrayscale)
        Me.GroupBox10.Controls.Add(Me.CheckBoxBackDisplay)
        Me.GroupBox10.Controls.Add(Me.CheckBoxBackSave)
        Me.GroupBox10.Location = New System.Drawing.Point(120, 64)
        Me.GroupBox10.Name = "GroupBox10"
        Me.GroupBox10.Size = New System.Drawing.Size(112, 112)
        Me.GroupBox10.TabIndex = 1
        Me.GroupBox10.TabStop = False
        Me.GroupBox10.Text = "Back"
        '
        'CheckBoxBackScan
        '
        Me.CheckBoxBackScan.Location = New System.Drawing.Point(16, 16)
        Me.CheckBoxBackScan.Name = "CheckBoxBackScan"
        Me.CheckBoxBackScan.Size = New System.Drawing.Size(64, 16)
        Me.CheckBoxBackScan.TabIndex = 0
        Me.CheckBoxBackScan.Text = "Scan"
        '
        'CheckBoxBackGrayscale
        '
        Me.CheckBoxBackGrayscale.Location = New System.Drawing.Point(16, 40)
        Me.CheckBoxBackGrayscale.Name = "CheckBoxBackGrayscale"
        Me.CheckBoxBackGrayscale.Size = New System.Drawing.Size(80, 16)
        Me.CheckBoxBackGrayscale.TabIndex = 0
        Me.CheckBoxBackGrayscale.Text = "Grayscale"
        '
        'CheckBoxBackDisplay
        '
        Me.CheckBoxBackDisplay.Location = New System.Drawing.Point(16, 64)
        Me.CheckBoxBackDisplay.Name = "CheckBoxBackDisplay"
        Me.CheckBoxBackDisplay.Size = New System.Drawing.Size(64, 16)
        Me.CheckBoxBackDisplay.TabIndex = 0
        Me.CheckBoxBackDisplay.Text = "Display"
        '
        'CheckBoxBackSave
        '
        Me.CheckBoxBackSave.Location = New System.Drawing.Point(16, 88)
        Me.CheckBoxBackSave.Name = "CheckBoxBackSave"
        Me.CheckBoxBackSave.Size = New System.Drawing.Size(80, 16)
        Me.CheckBoxBackSave.TabIndex = 0
        Me.CheckBoxBackSave.Text = "Save"
        '
        'TabOcrAb
        '
        Me.TabOcrAb.Controls.Add(Me.GroupBox12)
        Me.TabOcrAb.Location = New System.Drawing.Point(4, 55)
        Me.TabOcrAb.Name = "TabOcrAb"
        Me.TabOcrAb.Size = New System.Drawing.Size(232, 181)
        Me.TabOcrAb.TabIndex = 9
        Me.TabOcrAb.Text = "OcrAB"
        '
        'GroupBox12
        '
        Me.GroupBox12.Controls.Add(Me.Label6)
        Me.GroupBox12.Controls.Add(Me.ComboBoxOcrAbFont)
        Me.GroupBox12.Controls.Add(Me.CheckBoxOcrAb)
        Me.GroupBox12.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox12.Name = "GroupBox12"
        Me.GroupBox12.Size = New System.Drawing.Size(232, 176)
        Me.GroupBox12.TabIndex = 0
        Me.GroupBox12.TabStop = False
        Me.GroupBox12.Text = "Settings"
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(24, 96)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(48, 16)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "Font"
        '
        'ComboBoxOcrAbFont
        '
        Me.ComboBoxOcrAbFont.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxOcrAbFont.Items.AddRange(New Object() {"OCR_A", "OCR_B"})
        Me.ComboBoxOcrAbFont.Location = New System.Drawing.Point(96, 96)
        Me.ComboBoxOcrAbFont.Name = "ComboBoxOcrAbFont"
        Me.ComboBoxOcrAbFont.Size = New System.Drawing.Size(121, 20)
        Me.ComboBoxOcrAbFont.TabIndex = 1
        '
        'CheckBoxOcrAb
        '
        Me.CheckBoxOcrAb.Location = New System.Drawing.Point(24, 32)
        Me.CheckBoxOcrAb.Name = "CheckBoxOcrAb"
        Me.CheckBoxOcrAb.TabIndex = 0
        Me.CheckBoxOcrAb.Text = "OcrAB"
        '
        'tabEndorse
        '
        Me.tabEndorse.Controls.Add(Me.GroupBox2)
        Me.tabEndorse.Location = New System.Drawing.Point(4, 55)
        Me.tabEndorse.Name = "tabEndorse"
        Me.tabEndorse.Size = New System.Drawing.Size(232, 181)
        Me.tabEndorse.TabIndex = 1
        Me.tabEndorse.Text = "Endorse"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.CheckBoxElecEndorseText)
        Me.GroupBox2.Controls.Add(Me.CheckBoxElecEndorseImage)
        Me.GroupBox2.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(232, 176)
        Me.GroupBox2.TabIndex = 0
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Settings"
        '
        'CheckBoxElecEndorseText
        '
        Me.CheckBoxElecEndorseText.Location = New System.Drawing.Point(24, 48)
        Me.CheckBoxElecEndorseText.Name = "CheckBoxElecEndorseText"
        Me.CheckBoxElecEndorseText.Size = New System.Drawing.Size(192, 32)
        Me.CheckBoxElecEndorseText.TabIndex = 0
        Me.CheckBoxElecEndorseText.Text = "Perform electronic endorsement[Text]"
        '
        'CheckBoxElecEndorseImage
        '
        Me.CheckBoxElecEndorseImage.Location = New System.Drawing.Point(24, 80)
        Me.CheckBoxElecEndorseImage.Name = "CheckBoxElecEndorseImage"
        Me.CheckBoxElecEndorseImage.Size = New System.Drawing.Size(192, 32)
        Me.CheckBoxElecEndorseImage.TabIndex = 0
        Me.CheckBoxElecEndorseImage.Text = "Perform electronic endorsement[Image]"
        '
        'tabConfirmation
        '
        Me.tabConfirmation.Controls.Add(Me.GroupBox8)
        Me.tabConfirmation.Location = New System.Drawing.Point(4, 55)
        Me.tabConfirmation.Name = "tabConfirmation"
        Me.tabConfirmation.Size = New System.Drawing.Size(232, 181)
        Me.tabConfirmation.TabIndex = 7
        Me.tabConfirmation.Text = "Confirmation"
        '
        'GroupBox8
        '
        Me.GroupBox8.Controls.Add(Me.ButtonSetValues)
        Me.GroupBox8.Controls.Add(Me.CheckBoxNoCall)
        Me.GroupBox8.Controls.Add(Me.CheckBoxRun)
        Me.GroupBox8.Controls.Add(Me.CheckBoxConfirmation)
        Me.GroupBox8.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox8.Name = "GroupBox8"
        Me.GroupBox8.Size = New System.Drawing.Size(232, 176)
        Me.GroupBox8.TabIndex = 0
        Me.GroupBox8.TabStop = False
        Me.GroupBox8.Text = "Settings"
        '
        'ButtonSetValues
        '
        Me.ButtonSetValues.Location = New System.Drawing.Point(64, 141)
        Me.ButtonSetValues.Name = "ButtonSetValues"
        Me.ButtonSetValues.Size = New System.Drawing.Size(96, 23)
        Me.ButtonSetValues.TabIndex = 13
        Me.ButtonSetValues.Text = "Settings Values"
        '
        'CheckBoxNoCall
        '
        Me.CheckBoxNoCall.Location = New System.Drawing.Point(48, 104)
        Me.CheckBoxNoCall.Name = "CheckBoxNoCall"
        Me.CheckBoxNoCall.Size = New System.Drawing.Size(168, 24)
        Me.CheckBoxNoCall.TabIndex = 12
        Me.CheckBoxNoCall.Text = "No call SetBehaviorToScnResult"
        '
        'CheckBoxRun
        '
        Me.CheckBoxRun.Location = New System.Drawing.Point(32, 64)
        Me.CheckBoxRun.Name = "CheckBoxRun"
        Me.CheckBoxRun.Size = New System.Drawing.Size(192, 16)
        Me.CheckBoxRun.TabIndex = 11
        Me.CheckBoxRun.Text = "Run SetBehaviorToScnResult"
        '
        'CheckBoxConfirmation
        '
        Me.CheckBoxConfirmation.Location = New System.Drawing.Point(16, 24)
        Me.CheckBoxConfirmation.Name = "CheckBoxConfirmation"
        Me.CheckBoxConfirmation.Size = New System.Drawing.Size(136, 16)
        Me.CheckBoxConfirmation.TabIndex = 0
        Me.CheckBoxConfirmation.Text = "Comfirmation Mode"
        '
        'tabPaperMisInsert
        '
        Me.tabPaperMisInsert.Controls.Add(Me.GroupBox3)
        Me.tabPaperMisInsert.Location = New System.Drawing.Point(4, 55)
        Me.tabPaperMisInsert.Name = "tabPaperMisInsert"
        Me.tabPaperMisInsert.Size = New System.Drawing.Size(232, 181)
        Me.tabPaperMisInsert.TabIndex = 2
        Me.tabPaperMisInsert.Text = "MisInsert"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.CheckBoxMisCancel)
        Me.GroupBox3.Controls.Add(Me.CheckBoxMisStamp)
        Me.GroupBox3.Controls.Add(Me.Label1)
        Me.GroupBox3.Controls.Add(Me.ComboBoxMisEject)
        Me.GroupBox3.Controls.Add(Me.CheckBoxMisDetect)
        Me.GroupBox3.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(232, 176)
        Me.GroupBox3.TabIndex = 0
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Settings"
        '
        'CheckBoxMisCancel
        '
        Me.CheckBoxMisCancel.Location = New System.Drawing.Point(88, 128)
        Me.CheckBoxMisCancel.Name = "CheckBoxMisCancel"
        Me.CheckBoxMisCancel.TabIndex = 4
        Me.CheckBoxMisCancel.Text = "Cancel"
        '
        'CheckBoxMisStamp
        '
        Me.CheckBoxMisStamp.Location = New System.Drawing.Point(88, 96)
        Me.CheckBoxMisStamp.Name = "CheckBoxMisStamp"
        Me.CheckBoxMisStamp.TabIndex = 3
        Me.CheckBoxMisStamp.Text = "Stamp"
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(8, 64)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 23)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Eject Type"
        '
        'ComboBoxMisEject
        '
        Me.ComboBoxMisEject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxMisEject.Items.AddRange(New Object() {"Main Eject", "Sub Eject"})
        Me.ComboBoxMisEject.Location = New System.Drawing.Point(88, 64)
        Me.ComboBoxMisEject.Name = "ComboBoxMisEject"
        Me.ComboBoxMisEject.Size = New System.Drawing.Size(121, 20)
        Me.ComboBoxMisEject.TabIndex = 1
        '
        'CheckBoxMisDetect
        '
        Me.CheckBoxMisDetect.Location = New System.Drawing.Point(48, 24)
        Me.CheckBoxMisDetect.Name = "CheckBoxMisDetect"
        Me.CheckBoxMisDetect.TabIndex = 0
        Me.CheckBoxMisDetect.Text = "Detect"
        '
        'tabNoise
        '
        Me.tabNoise.Controls.Add(Me.GroupBox4)
        Me.tabNoise.Location = New System.Drawing.Point(4, 55)
        Me.tabNoise.Name = "tabNoise"
        Me.tabNoise.Size = New System.Drawing.Size(232, 181)
        Me.tabNoise.TabIndex = 3
        Me.tabNoise.Text = "Noise"
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.CheckBoxNoiseCancel)
        Me.GroupBox4.Controls.Add(Me.CheckBoxNoiseStamp)
        Me.GroupBox4.Controls.Add(Me.Label2)
        Me.GroupBox4.Controls.Add(Me.ComboBoxNoiseEject)
        Me.GroupBox4.Controls.Add(Me.CheckBoxNoiseDetect)
        Me.GroupBox4.Location = New System.Drawing.Point(0, 3)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(232, 173)
        Me.GroupBox4.TabIndex = 1
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Settings"
        '
        'CheckBoxNoiseCancel
        '
        Me.CheckBoxNoiseCancel.Location = New System.Drawing.Point(88, 128)
        Me.CheckBoxNoiseCancel.Name = "CheckBoxNoiseCancel"
        Me.CheckBoxNoiseCancel.TabIndex = 4
        Me.CheckBoxNoiseCancel.Text = "Cancel"
        '
        'CheckBoxNoiseStamp
        '
        Me.CheckBoxNoiseStamp.Location = New System.Drawing.Point(88, 96)
        Me.CheckBoxNoiseStamp.Name = "CheckBoxNoiseStamp"
        Me.CheckBoxNoiseStamp.TabIndex = 3
        Me.CheckBoxNoiseStamp.Text = "Stamp"
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(8, 64)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(72, 23)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Eject Type"
        '
        'ComboBoxNoiseEject
        '
        Me.ComboBoxNoiseEject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxNoiseEject.Items.AddRange(New Object() {"Main Eject", "Sub Eject"})
        Me.ComboBoxNoiseEject.Location = New System.Drawing.Point(88, 64)
        Me.ComboBoxNoiseEject.Name = "ComboBoxNoiseEject"
        Me.ComboBoxNoiseEject.Size = New System.Drawing.Size(121, 20)
        Me.ComboBoxNoiseEject.TabIndex = 1
        '
        'CheckBoxNoiseDetect
        '
        Me.CheckBoxNoiseDetect.Location = New System.Drawing.Point(48, 24)
        Me.CheckBoxNoiseDetect.Name = "CheckBoxNoiseDetect"
        Me.CheckBoxNoiseDetect.TabIndex = 0
        Me.CheckBoxNoiseDetect.Text = "Detect"
        '
        'tabDoubleFeed
        '
        Me.tabDoubleFeed.Controls.Add(Me.GroupBox5)
        Me.tabDoubleFeed.Location = New System.Drawing.Point(4, 55)
        Me.tabDoubleFeed.Name = "tabDoubleFeed"
        Me.tabDoubleFeed.Size = New System.Drawing.Size(232, 181)
        Me.tabDoubleFeed.TabIndex = 4
        Me.tabDoubleFeed.Text = "DoubleFeed"
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.CheckBoxDFCancel)
        Me.GroupBox5.Controls.Add(Me.CheckBoxDFStamp)
        Me.GroupBox5.Controls.Add(Me.Label3)
        Me.GroupBox5.Controls.Add(Me.ComboBoxDFEject)
        Me.GroupBox5.Controls.Add(Me.CheckBoxDFDetect)
        Me.GroupBox5.Location = New System.Drawing.Point(0, 3)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(232, 173)
        Me.GroupBox5.TabIndex = 1
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Settings"
        '
        'CheckBoxDFCancel
        '
        Me.CheckBoxDFCancel.Location = New System.Drawing.Point(88, 128)
        Me.CheckBoxDFCancel.Name = "CheckBoxDFCancel"
        Me.CheckBoxDFCancel.TabIndex = 4
        Me.CheckBoxDFCancel.Text = "Cancel"
        '
        'CheckBoxDFStamp
        '
        Me.CheckBoxDFStamp.Location = New System.Drawing.Point(88, 96)
        Me.CheckBoxDFStamp.Name = "CheckBoxDFStamp"
        Me.CheckBoxDFStamp.TabIndex = 3
        Me.CheckBoxDFStamp.Text = "Stamp"
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(8, 64)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(72, 23)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Eject Type"
        '
        'ComboBoxDFEject
        '
        Me.ComboBoxDFEject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxDFEject.Items.AddRange(New Object() {"Main Eject", "Sub Eject"})
        Me.ComboBoxDFEject.Location = New System.Drawing.Point(88, 64)
        Me.ComboBoxDFEject.Name = "ComboBoxDFEject"
        Me.ComboBoxDFEject.Size = New System.Drawing.Size(121, 20)
        Me.ComboBoxDFEject.TabIndex = 1
        '
        'CheckBoxDFDetect
        '
        Me.CheckBoxDFDetect.Location = New System.Drawing.Point(48, 24)
        Me.CheckBoxDFDetect.Name = "CheckBoxDFDetect"
        Me.CheckBoxDFDetect.TabIndex = 0
        Me.CheckBoxDFDetect.Text = "Detect"
        '
        'tabBaddata
        '
        Me.tabBaddata.Controls.Add(Me.GroupBox6)
        Me.tabBaddata.Location = New System.Drawing.Point(4, 55)
        Me.tabBaddata.Name = "tabBaddata"
        Me.tabBaddata.Size = New System.Drawing.Size(232, 181)
        Me.tabBaddata.TabIndex = 5
        Me.tabBaddata.Text = "Baddata"
        '
        'GroupBox6
        '
        Me.GroupBox6.Controls.Add(Me.TextBoxBadCount)
        Me.GroupBox6.Controls.Add(Me.Label10)
        Me.GroupBox6.Controls.Add(Me.CheckBoxBadCancel)
        Me.GroupBox6.Controls.Add(Me.CheckBoxBadStamp)
        Me.GroupBox6.Controls.Add(Me.Label4)
        Me.GroupBox6.Controls.Add(Me.ComboBoxBadEject)
        Me.GroupBox6.Controls.Add(Me.CheckBoxBadDetect)
        Me.GroupBox6.Location = New System.Drawing.Point(0, 3)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(232, 173)
        Me.GroupBox6.TabIndex = 1
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "Settings"
        '
        'TextBoxBadCount
        '
        Me.TextBoxBadCount.Location = New System.Drawing.Point(88, 144)
        Me.TextBoxBadCount.Name = "TextBoxBadCount"
        Me.TextBoxBadCount.TabIndex = 6
        Me.TextBoxBadCount.Text = ""
        '
        'Label10
        '
        Me.Label10.Location = New System.Drawing.Point(8, 144)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(48, 23)
        Me.Label10.TabIndex = 5
        Me.Label10.Text = "Count"
        '
        'CheckBoxBadCancel
        '
        Me.CheckBoxBadCancel.Location = New System.Drawing.Point(88, 112)
        Me.CheckBoxBadCancel.Name = "CheckBoxBadCancel"
        Me.CheckBoxBadCancel.TabIndex = 4
        Me.CheckBoxBadCancel.Text = "Cancel"
        '
        'CheckBoxBadStamp
        '
        Me.CheckBoxBadStamp.Location = New System.Drawing.Point(88, 88)
        Me.CheckBoxBadStamp.Name = "CheckBoxBadStamp"
        Me.CheckBoxBadStamp.TabIndex = 3
        Me.CheckBoxBadStamp.Text = "Stamp"
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(8, 64)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(72, 23)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "Eject Type"
        '
        'ComboBoxBadEject
        '
        Me.ComboBoxBadEject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxBadEject.Items.AddRange(New Object() {"Main Eject", "Sub Eject"})
        Me.ComboBoxBadEject.Location = New System.Drawing.Point(88, 64)
        Me.ComboBoxBadEject.Name = "ComboBoxBadEject"
        Me.ComboBoxBadEject.Size = New System.Drawing.Size(121, 20)
        Me.ComboBoxBadEject.TabIndex = 1
        '
        'CheckBoxBadDetect
        '
        Me.CheckBoxBadDetect.Location = New System.Drawing.Point(48, 24)
        Me.CheckBoxBadDetect.Name = "CheckBoxBadDetect"
        Me.CheckBoxBadDetect.TabIndex = 0
        Me.CheckBoxBadDetect.Text = "Detect"
        '
        'tabNodata
        '
        Me.tabNodata.Controls.Add(Me.GroupBox7)
        Me.tabNodata.Location = New System.Drawing.Point(4, 55)
        Me.tabNodata.Name = "tabNodata"
        Me.tabNodata.Size = New System.Drawing.Size(232, 181)
        Me.tabNodata.TabIndex = 6
        Me.tabNodata.Text = "Nodata"
        '
        'GroupBox7
        '
        Me.GroupBox7.Controls.Add(Me.CheckBoxNoCancel)
        Me.GroupBox7.Controls.Add(Me.CheckBoxNoStamp)
        Me.GroupBox7.Controls.Add(Me.Label5)
        Me.GroupBox7.Controls.Add(Me.ComboBoxNoEject)
        Me.GroupBox7.Controls.Add(Me.CheckBoxNoDetect)
        Me.GroupBox7.Location = New System.Drawing.Point(0, 3)
        Me.GroupBox7.Name = "GroupBox7"
        Me.GroupBox7.Size = New System.Drawing.Size(232, 173)
        Me.GroupBox7.TabIndex = 1
        Me.GroupBox7.TabStop = False
        Me.GroupBox7.Text = "Settings"
        '
        'CheckBoxNoCancel
        '
        Me.CheckBoxNoCancel.Location = New System.Drawing.Point(88, 128)
        Me.CheckBoxNoCancel.Name = "CheckBoxNoCancel"
        Me.CheckBoxNoCancel.TabIndex = 4
        Me.CheckBoxNoCancel.Text = "Cancel"
        '
        'CheckBoxNoStamp
        '
        Me.CheckBoxNoStamp.Location = New System.Drawing.Point(88, 96)
        Me.CheckBoxNoStamp.Name = "CheckBoxNoStamp"
        Me.CheckBoxNoStamp.TabIndex = 3
        Me.CheckBoxNoStamp.Text = "Stamp"
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(8, 64)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(72, 23)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "Eject Type"
        '
        'ComboBoxNoEject
        '
        Me.ComboBoxNoEject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxNoEject.Items.AddRange(New Object() {"Main Eject", "Sub Eject"})
        Me.ComboBoxNoEject.Location = New System.Drawing.Point(88, 64)
        Me.ComboBoxNoEject.Name = "ComboBoxNoEject"
        Me.ComboBoxNoEject.Size = New System.Drawing.Size(121, 20)
        Me.ComboBoxNoEject.TabIndex = 1
        '
        'CheckBoxNoDetect
        '
        Me.CheckBoxNoDetect.Location = New System.Drawing.Point(48, 24)
        Me.CheckBoxNoDetect.Name = "CheckBoxNoDetect"
        Me.CheckBoxNoDetect.TabIndex = 0
        Me.CheckBoxNoDetect.Text = "Detect"
        '
        'tabMicr
        '
        Me.tabMicr.Controls.Add(Me.GroupBox11)
        Me.tabMicr.Location = New System.Drawing.Point(4, 55)
        Me.tabMicr.Name = "tabMicr"
        Me.tabMicr.Size = New System.Drawing.Size(232, 181)
        Me.tabMicr.TabIndex = 8
        Me.tabMicr.Text = "MICR"
        '
        'GroupBox11
        '
        Me.GroupBox11.Controls.Add(Me.ComboBoxMicrFont)
        Me.GroupBox11.Controls.Add(Me.CheckBoxMicrSave)
        Me.GroupBox11.Controls.Add(Me.CheckBoxMicr)
        Me.GroupBox11.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox11.Name = "GroupBox11"
        Me.GroupBox11.Size = New System.Drawing.Size(232, 176)
        Me.GroupBox11.TabIndex = 0
        Me.GroupBox11.TabStop = False
        Me.GroupBox11.Text = "Settings"
        '
        'ComboBoxMicrFont
        '
        Me.ComboBoxMicrFont.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxMicrFont.Items.AddRange(New Object() {"E13B", "CMC7"})
        Me.ComboBoxMicrFont.Location = New System.Drawing.Point(8, 136)
        Me.ComboBoxMicrFont.Name = "ComboBoxMicrFont"
        Me.ComboBoxMicrFont.Size = New System.Drawing.Size(121, 20)
        Me.ComboBoxMicrFont.TabIndex = 4
        '
        'CheckBoxMicrSave
        '
        Me.CheckBoxMicrSave.Location = New System.Drawing.Point(8, 72)
        Me.CheckBoxMicrSave.Name = "CheckBoxMicrSave"
        Me.CheckBoxMicrSave.Size = New System.Drawing.Size(216, 32)
        Me.CheckBoxMicrSave.TabIndex = 3
        Me.CheckBoxMicrSave.Text = "Save MICR/OcrAB information to Harddisk"
        '
        'CheckBoxMicr
        '
        Me.CheckBoxMicr.Location = New System.Drawing.Point(8, 24)
        Me.CheckBoxMicr.Name = "CheckBoxMicr"
        Me.CheckBoxMicr.Size = New System.Drawing.Size(216, 24)
        Me.CheckBoxMicr.TabIndex = 0
        Me.CheckBoxMicr.Text = "Read characters magnetically [MICR]"
        '
        'TabBuzzer
        '
        Me.TabBuzzer.Controls.Add(Me.GroupBox13)
        Me.TabBuzzer.Location = New System.Drawing.Point(4, 55)
        Me.TabBuzzer.Name = "TabBuzzer"
        Me.TabBuzzer.Size = New System.Drawing.Size(232, 181)
        Me.TabBuzzer.TabIndex = 10
        Me.TabBuzzer.Text = "Buzzer"
        '
        'GroupBox13
        '
        Me.GroupBox13.Controls.Add(Me.ComboBoxWFeedCount)
        Me.GroupBox13.Controls.Add(Me.ComboBoxWFeedHz)
        Me.GroupBox13.Controls.Add(Me.Label9)
        Me.GroupBox13.Controls.Add(Me.ComboBoxErrorCount)
        Me.GroupBox13.Controls.Add(Me.ComboBoxErrorHz)
        Me.GroupBox13.Controls.Add(Me.Label8)
        Me.GroupBox13.Controls.Add(Me.ComboBoxSuccessCount)
        Me.GroupBox13.Controls.Add(Me.ComboBoxSuccessHz)
        Me.GroupBox13.Controls.Add(Me.Label7)
        Me.GroupBox13.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox13.Name = "GroupBox13"
        Me.GroupBox13.Size = New System.Drawing.Size(232, 176)
        Me.GroupBox13.TabIndex = 0
        Me.GroupBox13.TabStop = False
        Me.GroupBox13.Text = "Settings"
        '
        'ComboBoxWFeedCount
        '
        Me.ComboBoxWFeedCount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxWFeedCount.Items.AddRange(New Object() {"DISABLE", "ONE", "TWO", "MAX"})
        Me.ComboBoxWFeedCount.Location = New System.Drawing.Point(168, 128)
        Me.ComboBoxWFeedCount.Name = "ComboBoxWFeedCount"
        Me.ComboBoxWFeedCount.Size = New System.Drawing.Size(56, 20)
        Me.ComboBoxWFeedCount.TabIndex = 8
        '
        'ComboBoxWFeedHz
        '
        Me.ComboBoxWFeedHz.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxWFeedHz.Items.AddRange(New Object() {"440Hz", "880Hz", "4000Hz"})
        Me.ComboBoxWFeedHz.Location = New System.Drawing.Point(80, 128)
        Me.ComboBoxWFeedHz.Name = "ComboBoxWFeedHz"
        Me.ComboBoxWFeedHz.Size = New System.Drawing.Size(72, 20)
        Me.ComboBoxWFeedHz.TabIndex = 7
        '
        'Label9
        '
        Me.Label9.Location = New System.Drawing.Point(8, 128)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(64, 16)
        Me.Label9.TabIndex = 6
        Me.Label9.Text = "WFEED"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'ComboBoxErrorCount
        '
        Me.ComboBoxErrorCount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxErrorCount.Items.AddRange(New Object() {"DISABLE", "ONE", "TWO", "MAX"})
        Me.ComboBoxErrorCount.Location = New System.Drawing.Point(168, 80)
        Me.ComboBoxErrorCount.Name = "ComboBoxErrorCount"
        Me.ComboBoxErrorCount.Size = New System.Drawing.Size(56, 20)
        Me.ComboBoxErrorCount.TabIndex = 5
        '
        'ComboBoxErrorHz
        '
        Me.ComboBoxErrorHz.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxErrorHz.Items.AddRange(New Object() {"440Hz", "880Hz", "4000Hz"})
        Me.ComboBoxErrorHz.Location = New System.Drawing.Point(80, 80)
        Me.ComboBoxErrorHz.Name = "ComboBoxErrorHz"
        Me.ComboBoxErrorHz.Size = New System.Drawing.Size(72, 20)
        Me.ComboBoxErrorHz.TabIndex = 4
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(8, 80)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(64, 16)
        Me.Label8.TabIndex = 3
        Me.Label8.Text = "ERROR"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'ComboBoxSuccessCount
        '
        Me.ComboBoxSuccessCount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxSuccessCount.Items.AddRange(New Object() {"DISABLE", "ONE", "TWO", "MAX"})
        Me.ComboBoxSuccessCount.Location = New System.Drawing.Point(168, 32)
        Me.ComboBoxSuccessCount.Name = "ComboBoxSuccessCount"
        Me.ComboBoxSuccessCount.Size = New System.Drawing.Size(56, 20)
        Me.ComboBoxSuccessCount.TabIndex = 2
        '
        'ComboBoxSuccessHz
        '
        Me.ComboBoxSuccessHz.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxSuccessHz.Items.AddRange(New Object() {"440Hz", "880Hz", "4000Hz"})
        Me.ComboBoxSuccessHz.Location = New System.Drawing.Point(80, 32)
        Me.ComboBoxSuccessHz.Name = "ComboBoxSuccessHz"
        Me.ComboBoxSuccessHz.Size = New System.Drawing.Size(72, 20)
        Me.ComboBoxSuccessHz.TabIndex = 1
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(8, 32)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(64, 16)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "SUCCESS"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'ConfigureForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 12)
        Me.ClientSize = New System.Drawing.Size(256, 296)
        Me.Controls.Add(Me.TabCtrl)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnApply)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "ConfigureForm"
        Me.Text = "ConfigureForm"
        Me.TabCtrl.ResumeLayout(False)
        Me.tabImage.ResumeLayout(False)
        Me.GroupBox9.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox10.ResumeLayout(False)
        Me.TabOcrAb.ResumeLayout(False)
        Me.GroupBox12.ResumeLayout(False)
        Me.tabEndorse.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.tabConfirmation.ResumeLayout(False)
        Me.GroupBox8.ResumeLayout(False)
        Me.tabPaperMisInsert.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.tabNoise.ResumeLayout(False)
        Me.GroupBox4.ResumeLayout(False)
        Me.tabDoubleFeed.ResumeLayout(False)
        Me.GroupBox5.ResumeLayout(False)
        Me.tabBaddata.ResumeLayout(False)
        Me.GroupBox6.ResumeLayout(False)
        Me.tabNodata.ResumeLayout(False)
        Me.GroupBox7.ResumeLayout(False)
        Me.tabMicr.ResumeLayout(False)
        Me.GroupBox11.ResumeLayout(False)
        Me.TabBuzzer.ResumeLayout(False)
        Me.GroupBox13.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    ' input/output of the property  
    Public Property Proc() As Properties
        Get
            Return New Properties(m_objConfig)
        End Get
        Set(ByVal Value As Properties)
            m_objConfig = New Properties(Value)
            LoadProperties()
        End Set
    End Property

    ' Initializ dialog
    Private Sub Inittab()
        LoadProperties()
        btnApply.Enabled = False
    End Sub

    ' this method is called when the user click the OK button
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        SaveProperties()
        Me.Close()
    End Sub

    ' this method is called when the user click the Cancel button
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    ' this method is called when the user click the Apply button
    Private Sub btnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApply.Click
        SaveProperties()
        btnApply.Enabled = False
    End Sub

    ' this method is Each value is bound to the property   
    Private Sub SaveProperties()
        m_objConfig(Properties.SCAN_FUNC) = ComboBoxScanFunc.SelectedIndex

        m_objConfig(Properties.SCAN_FRONT) = CheckBoxFrontScan.Checked
        m_objConfig(Properties.FRONT_GRAYSCALE) = CheckBoxFrontGrayscale.Checked
        m_objConfig(Properties.FRONT_DISPLAY) = CheckBoxFrontDisplay.Checked
        m_objConfig(Properties.FRONT_SAVE) = CheckBoxFrontSave.Checked

        m_objConfig(Properties.SCAN_BACK) = CheckBoxBackScan.Checked
        m_objConfig(Properties.BACK_GRAYSCALE) = CheckBoxBackGrayscale.Checked
        m_objConfig(Properties.BACK_DISPLAY) = CheckBoxBackDisplay.Checked
        m_objConfig(Properties.BACK_SAVE) = CheckBoxBackSave.Checked

        m_objConfig(Properties.MICR) = CheckBoxMicr.Checked
        m_objConfig(Properties.MICR_SAVE) = CheckBoxMicrSave.Checked
        m_objConfig(Properties.MICR_FONT) = ComboBoxMicrFont.SelectedIndex
        m_objConfig(Properties.MICR_SAVE_ENABLE) = CheckBoxMicrSave.Enabled

        m_objConfig(Properties.ELEC_ENDORSE_TEXT) = CheckBoxElecEndorseText.Checked
        m_objConfig(Properties.ELEC_ENDORSE_IMAGE) = CheckBoxElecEndorseImage.Checked

        m_objConfig(Properties.CONFIRMATION) = CheckBoxConfirmation.Checked
        m_objConfig(Properties.RUN_SCN_TO_RESULT) = CheckBoxRun.Checked
        m_objConfig(Properties.NO_CALL_SCN_TO_RESULT) = CheckBoxNoCall.Checked

        m_objConfig(Properties.MIS_INSERT_DETECT) = CheckBoxMisDetect.Checked
        m_objConfig(Properties.MIS_INSERT_EJECT) = ComboBoxMisEject.SelectedIndex
        m_objConfig(Properties.MIS_INSERT_STAMP) = CheckBoxMisStamp.Checked
        m_objConfig(Properties.MIS_INSERT_CANCEL) = CheckBoxMisCancel.Checked

        m_objConfig(Properties.NOISE_DETECT) = CheckBoxNoiseDetect.Checked
        m_objConfig(Properties.NOISE_EJECT) = ComboBoxNoiseEject.SelectedIndex
        m_objConfig(Properties.NOISE_STAMP) = CheckBoxNoiseStamp.Checked
        m_objConfig(Properties.NOISE_CANCEL) = CheckBoxNoiseCancel.Checked

        m_objConfig(Properties.DOUBLE_FEED_DETECT) = CheckBoxDFDetect.Checked
        m_objConfig(Properties.DOUBLE_FEED_EJECT) = ComboBoxDFEject.SelectedIndex
        m_objConfig(Properties.DOUBLE_FEED_STAMP) = CheckBoxDFStamp.Checked
        m_objConfig(Properties.DOUBLE_FEED_CANCEL) = CheckBoxDFCancel.Checked

        m_objConfig(Properties.BADDATA_COUNT) = Convert.ToInt32(TextBoxBadCount.Text)
        m_objConfig(Properties.BADDATA_DETECT) = CheckBoxBadDetect.Checked
        m_objConfig(Properties.BADDATA_EJECT) = ComboBoxBadEject.SelectedIndex
        m_objConfig(Properties.BADDATA_STAMP) = CheckBoxBadStamp.Checked
        m_objConfig(Properties.BADDATA_CANCEL) = CheckBoxBadCancel.Checked

        m_objConfig(Properties.NODATA_DETECT) = CheckBoxNoDetect.Checked
        m_objConfig(Properties.NODATA_EJECT) = ComboBoxNoEject.SelectedIndex
        m_objConfig(Properties.NODATA_STAMP) = CheckBoxNoStamp.Checked
        m_objConfig(Properties.NODATA_CANCEL) = CheckBoxNoCancel.Checked

        m_objConfig(Properties.OCR_AB) = CheckBoxOcrAb.Checked
        m_objConfig(Properties.OCR_AB_FONT) = ComboBoxOcrAbFont.SelectedIndex

        m_objConfig(Properties.BUZZER_SUCCESS_HZ) = ComboBoxSuccessHz.SelectedIndex
        m_objConfig(Properties.BUZZER_SUCCESS_COUNT) = ComboBoxSuccessCount.SelectedIndex
        m_objConfig(Properties.BUZZER_ERROR_HZ) = ComboBoxErrorHz.SelectedIndex
        m_objConfig(Properties.BUZZER_ERROR_COUNT) = ComboBoxErrorCount.SelectedIndex
        m_objConfig(Properties.BUZZER_WFEED_HZ) = ComboBoxWFeedHz.SelectedIndex
        m_objConfig(Properties.BUZZER_WFEED_COUNT) = ComboBoxWFeedCount.SelectedIndex
    End Sub

    ' this method is property is bound to the  Each value
    Private Sub LoadProperties()
        ComboBoxScanFunc.SelectedIndex = m_objConfig(Properties.SCAN_FUNC)

        CheckBoxFrontScan.Checked = m_objConfig(Properties.SCAN_FRONT)
        CheckBoxFrontGrayscale.Checked = m_objConfig(Properties.FRONT_GRAYSCALE)
        CheckBoxFrontDisplay.Checked = m_objConfig(Properties.FRONT_DISPLAY)
        CheckBoxFrontSave.Checked = m_objConfig(Properties.FRONT_SAVE)

        CheckBoxBackScan.Checked = m_objConfig(Properties.SCAN_BACK)
        CheckBoxBackGrayscale.Checked = m_objConfig(Properties.BACK_GRAYSCALE)
        CheckBoxBackDisplay.Checked = m_objConfig(Properties.BACK_DISPLAY)
        CheckBoxBackSave.Checked = m_objConfig(Properties.BACK_SAVE)

        CheckBoxMicr.Checked = m_objConfig(Properties.MICR)
        CheckBoxMicrSave.Checked = m_objConfig(Properties.MICR_SAVE)
        ComboBoxMicrFont.SelectedIndex = m_objConfig(Properties.MICR_FONT)
        CheckBoxMicrSave.Enabled = m_objConfig(Properties.MICR_SAVE_ENABLE)

        CheckBoxElecEndorseText.Checked = m_objConfig(Properties.ELEC_ENDORSE_TEXT)
        CheckBoxElecEndorseImage.Checked = m_objConfig(Properties.ELEC_ENDORSE_IMAGE)

        CheckBoxConfirmation.Checked = m_objConfig(Properties.CONFIRMATION)
        CheckBoxRun.Checked = m_objConfig(Properties.RUN_SCN_TO_RESULT)
        CheckBoxNoCall.Checked = m_objConfig(Properties.NO_CALL_SCN_TO_RESULT)

        CheckBoxMisDetect.Checked = m_objConfig(Properties.MIS_INSERT_DETECT)
        ComboBoxMisEject.SelectedIndex = m_objConfig(Properties.MIS_INSERT_EJECT)
        CheckBoxMisStamp.Checked = m_objConfig(Properties.MIS_INSERT_STAMP)
        CheckBoxMisCancel.Checked = m_objConfig(Properties.MIS_INSERT_CANCEL)

        CheckBoxNoiseDetect.Checked = m_objConfig(Properties.NOISE_DETECT)
        ComboBoxNoiseEject.SelectedIndex = m_objConfig(Properties.NOISE_EJECT)
        CheckBoxNoiseStamp.Checked = m_objConfig(Properties.NOISE_STAMP)
        CheckBoxNoiseCancel.Checked = m_objConfig(Properties.NOISE_CANCEL)

        CheckBoxDFDetect.Checked = m_objConfig(Properties.DOUBLE_FEED_DETECT)
        ComboBoxDFEject.SelectedIndex = m_objConfig(Properties.DOUBLE_FEED_EJECT)
        CheckBoxDFStamp.Checked = m_objConfig(Properties.DOUBLE_FEED_STAMP)
        CheckBoxDFCancel.Checked = m_objConfig(Properties.DOUBLE_FEED_CANCEL)

        TextBoxBadCount.Text = Convert.ToString(m_objConfig(Properties.BADDATA_COUNT))
        CheckBoxBadDetect.Checked = m_objConfig(Properties.BADDATA_DETECT)
        ComboBoxBadEject.SelectedIndex = m_objConfig(Properties.BADDATA_EJECT)
        CheckBoxBadStamp.Checked = m_objConfig(Properties.BADDATA_STAMP)
        CheckBoxBadCancel.Checked = m_objConfig(Properties.BADDATA_CANCEL)

        CheckBoxNoDetect.Checked = m_objConfig(Properties.NODATA_DETECT)
        ComboBoxNoEject.SelectedIndex = m_objConfig(Properties.NODATA_EJECT)
        CheckBoxNoStamp.Checked = m_objConfig(Properties.NODATA_STAMP)
        CheckBoxNoCancel.Checked = m_objConfig(Properties.NODATA_CANCEL)

        EnableMisInsert(m_objConfig(Properties.MIS_INSERT_DETECT))
        EnableNoise(m_objConfig(Properties.NOISE_DETECT))
        EnableDoubleFeed(m_objConfig(Properties.DOUBLE_FEED_DETECT))
        EnableNodata(m_objConfig(Properties.NODATA_DETECT))
        EnableBaddata(m_objConfig(Properties.BADDATA_DETECT))
        EnableRun(CheckBoxConfirmation.Checked)

        CheckBoxOcrAb.Checked = m_objConfig(Properties.OCR_AB)
        ComboBoxOcrAbFont.SelectedIndex = m_objConfig(Properties.OCR_AB_FONT)
        ComboBoxOcrAbFont.Enabled = m_objConfig(Properties.OCR_AB)

        ComboBoxSuccessHz.SelectedIndex = m_objConfig(Properties.BUZZER_SUCCESS_HZ)
        ComboBoxSuccessCount.SelectedIndex = m_objConfig(Properties.BUZZER_SUCCESS_COUNT)
        ComboBoxErrorHz.SelectedIndex = m_objConfig(Properties.BUZZER_ERROR_HZ)
        ComboBoxErrorCount.SelectedIndex = m_objConfig(Properties.BUZZER_ERROR_COUNT)
        ComboBoxWFeedHz.SelectedIndex = m_objConfig(Properties.BUZZER_WFEED_HZ)
        ComboBoxWFeedCount.SelectedIndex = m_objConfig(Properties.BUZZER_WFEED_COUNT)
        btnApply.Enabled = False
    End Sub

    ' Restriction of a normal scan
    Private Sub ComboBoxScanFunc_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBoxScanFunc.SelectedIndexChanged
        btnApply.Enabled = True
    End Sub

    Private Sub CheckBoxElecEndorseText_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxElecEndorseText.CheckedChanged
        btnApply.Enabled = True
    End Sub

    Private Sub CheckBoxMisDetect_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxMisDetect.CheckedChanged
        btnApply.Enabled = True
        EnableMisInsert(CheckBoxMisDetect.Checked)
    End Sub

    Private Sub ComboBoxMisEject_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBoxMisEject.SelectedIndexChanged
        btnApply.Enabled = True
    End Sub

    Private Sub CheckBoxMisStamp_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxMisStamp.CheckedChanged
        btnApply.Enabled = True
    End Sub

    Private Sub CheckBoxMisCancel_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxMisCancel.CheckedChanged
        btnApply.Enabled = True
    End Sub

    Private Sub CheckBoxNoiseDetect_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxNoiseDetect.CheckedChanged
        btnApply.Enabled = True
        EnableNoise(CheckBoxNoiseDetect.Checked)
    End Sub

    Private Sub ComboBoxNoiseEject_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBoxNoiseEject.SelectedIndexChanged
        btnApply.Enabled = True
    End Sub

    Private Sub CheckBoxNoiseStamp_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxNoiseStamp.CheckedChanged
        btnApply.Enabled = True
    End Sub

    Private Sub CheckBoxNoiseCancel_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxNoiseCancel.CheckedChanged
        btnApply.Enabled = True
    End Sub

    Private Sub CheckBoxDFDetect_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxDFDetect.CheckedChanged
        btnApply.Enabled = True
        EnableDoubleFeed(CheckBoxDFDetect.Checked)
    End Sub

    Private Sub ComboBoxDFEject_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBoxDFEject.SelectedIndexChanged
        btnApply.Enabled = True
    End Sub

    Private Sub CheckBoxDFStamp_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxDFStamp.CheckedChanged
        btnApply.Enabled = True
    End Sub

    Private Sub CheckBoxDFCancel_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxDFCancel.CheckedChanged
        btnApply.Enabled = True
    End Sub

    Private Sub CheckBoxBadDetect_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxBadDetect.CheckedChanged
        btnApply.Enabled = True
        EnableBaddata(CheckBoxBadDetect.Checked)
    End Sub

    Private Sub ComboBoxBadEject_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBoxBadEject.SelectedIndexChanged
        btnApply.Enabled = True
    End Sub

    Private Sub CheckBoxBadStamp_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxBadStamp.CheckedChanged
        btnApply.Enabled = True
    End Sub

    Private Sub CheckBoxBadCancel_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxBadCancel.CheckedChanged
        btnApply.Enabled = True
    End Sub

    Private Sub CheckBoxNoDetect_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxNoDetect.CheckedChanged
        btnApply.Enabled = True
        EnableNodata(CheckBoxNoDetect.Checked)
    End Sub

    Private Sub ComboBoxNoEject_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBoxNoEject.SelectedIndexChanged
        btnApply.Enabled = True
    End Sub

    Private Sub CheckBoxNoStamp_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxNoStamp.CheckedChanged
        btnApply.Enabled = True
    End Sub

    Private Sub CheckBoxNoCancel_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxNoCancel.CheckedChanged
        btnApply.Enabled = True
    End Sub

    Private Sub CheckBoxConfirmation_CheckedChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxConfirmation.CheckedChanged
        btnApply.Enabled = True
        EnableRun(CheckBoxConfirmation.Checked())
    End Sub

    Private Sub CheckBoxFrontScan_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxFrontScan.CheckedChanged
        btnApply.Enabled = True

        SetFrontEnable(CheckBoxFrontScan.Checked)
    End Sub

    Private Sub CheckBoxFrontGrayscale_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxFrontGrayscale.CheckedChanged
        btnApply.Enabled = True
    End Sub

    Private Sub CheckBoxFrontDisplay_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxFrontDisplay.CheckedChanged
        btnApply.Enabled = True
    End Sub

    Private Sub CheckBoxFrontSave_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxFrontSave.CheckedChanged
        btnApply.Enabled = True
    End Sub

    Private Sub CheckBoxBackScan_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxBackScan.CheckedChanged
        btnApply.Enabled = True

        SetBackEnable(CheckBoxBackScan.Checked)
    End Sub

    Private Sub CheckBoxBackGrayscale_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxBackGrayscale.CheckedChanged
        btnApply.Enabled = True
    End Sub

    Private Sub CheckBoxBackDisplay_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxBackDisplay.CheckedChanged
        btnApply.Enabled = True
    End Sub

    Private Sub CheckBoxBackSave_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxBackSave.CheckedChanged
        btnApply.Enabled = True
    End Sub

    Private Sub CheckBoxMicr_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxMicr.CheckedChanged
        btnApply.Enabled = True

        If Not (CheckBoxOcrAb.Checked) Then
            CheckBoxMicrSave.Enabled = CheckBoxMicr.Checked
        End If

        ComboBoxMicrFont.Enabled = CheckBoxMicr.Checked
    End Sub

    Private Sub CheckBoxMicrSave_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxMicrSave.CheckedChanged
        btnApply.Enabled = True
    End Sub

    ' Condition of the control of MisInsett it changes
    Private Sub EnableMisInsert(ByVal enable As Boolean)
        ComboBoxMisEject.Enabled = enable
        CheckBoxMisStamp.Enabled = enable
        CheckBoxMisCancel.Enabled = enable
    End Sub

    ' Condition of the control of Noise it changes
    Private Sub EnableNoise(ByVal enable As Boolean)
        ComboBoxNoiseEject.Enabled = enable
        CheckBoxNoiseStamp.Enabled = enable
        CheckBoxNoiseCancel.Enabled = enable
    End Sub

    ' Condition of the control of DoubleFeed it changes
    Private Sub EnableDoubleFeed(ByVal enable As Boolean)
        ComboBoxDFEject.Enabled = enable
        CheckBoxDFStamp.Enabled = enable
        CheckBoxDFCancel.Enabled = enable
    End Sub

    ' Condition of the control of Nodata it changes
    Private Sub EnableNodata(ByVal enable As Boolean)
        ComboBoxNoEject.Enabled = enable
        CheckBoxNoStamp.Enabled = enable
        CheckBoxNoCancel.Enabled = enable
    End Sub

    ' Condition of the control of Baddata it changes
    Private Sub EnableBaddata(ByVal enable As Boolean)
        ComboBoxBadEject.Enabled = enable
        CheckBoxBadStamp.Enabled = enable
        CheckBoxBadCancel.Enabled = enable
        TextBoxBadCount.Enabled = enable
    End Sub

    Private Sub CheckBoxElecEndorseImage_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxElecEndorseImage.CheckedChanged
        btnApply.Enabled = True
    End Sub

    Private Sub CheckBoxRun_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxRun.CheckedChanged
        btnApply.Enabled = True
        EnableNoCall(CheckBoxRun.Checked)
    End Sub

    Private Sub CheckBoxNoCall_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxNoCall.CheckedChanged
        btnApply.Enabled = True
        ButtonSetValues.Enabled = CheckBoxNoCall.Checked
    End Sub

    Private Sub ButtonSetValues_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSetValues.Click
        btnApply.Enabled = True
        Dim dlg As ConfirmationForm = New ConfirmationForm
        dlg.Proc = m_objConfig
        dlg.ShowDialog()
        m_objConfig = dlg.Proc()
    End Sub

    Private Sub EnableRun(ByVal bEnable As Boolean)
        CheckBoxRun.Enabled = bEnable
        EnableNoCall(CheckBoxRun.Checked)
        If Not (bEnable) Then
            EnableNoCall(bEnable)
        End If
    End Sub

    Private Sub EnableNoCall(ByVal bEnable As Boolean)
        CheckBoxNoCall.Enabled = bEnable
        ButtonSetValues.Enabled = CheckBoxNoCall.Checked
        If Not (bEnable) Then
            ButtonSetValues.Enabled = bEnable
        End If
    End Sub

    Private Sub ComboBoxMicrFont_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBoxMicrFont.SelectedIndexChanged
        btnApply.Enabled = True
    End Sub

    Private Sub CheckBoxOcrAb_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxOcrAb.CheckedChanged
        btnApply.Enabled = True

        ComboBoxOcrAbFont.Enabled = CheckBoxOcrAb.Checked

        If Not (CheckBoxMicr.Checked) Then
            CheckBoxMicrSave.Enabled = CheckBoxOcrAb.Checked
        End If
    End Sub

    Private Sub ComboBoxOcrAbFont_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBoxOcrAbFont.SelectedIndexChanged
        btnApply.Enabled = True
    End Sub

    Private Sub ComboBoxSuccessHz_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBoxSuccessHz.SelectedIndexChanged
        btnApply.Enabled = True
    End Sub

    Private Sub ComboBoxSuccessCount_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBoxSuccessCount.SelectedIndexChanged
        btnApply.Enabled = True
    End Sub

    Private Sub ComboBoxErrorHz_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBoxErrorHz.SelectedIndexChanged
        btnApply.Enabled = True
    End Sub

    Private Sub ComboBoxErrorCount_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBoxErrorCount.SelectedIndexChanged
        btnApply.Enabled = True
    End Sub

    Private Sub ComboBoxWFeedHz_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBoxWFeedHz.SelectedIndexChanged
        btnApply.Enabled = True
    End Sub

    Private Sub ComboBoxWFeedCount_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBoxWFeedCount.SelectedIndexChanged
        btnApply.Enabled = True
    End Sub

    Private Sub TextBoxBadCount_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBoxBadCount.TextChanged
        btnApply.Enabled = True
    End Sub

    Private Sub SetFrontEnable(ByVal Enable As Boolean)
        CheckBoxFrontDisplay.Enabled = Enable
        CheckBoxFrontGrayscale.Enabled = Enable
        CheckBoxFrontSave.Enabled = Enable
    End Sub

    Private Sub SetBackEnable(ByVal Enable As Boolean)
        CheckBoxBackDisplay.Enabled = Enable
        CheckBoxBackGrayscale.Enabled = Enable
        CheckBoxBackSave.Enabled = Enable
    End Sub
End Class
