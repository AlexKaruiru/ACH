Imports System.Drawing
Imports LsFamily
Imports BRCTSScanner.pubReff.pubRef
Imports BrClearing.Common.Modscan
Public Class formOptions
    Inherits System.Windows.Forms.Form
    Const MODEL_LS105 = "LS100/5"
    Const MODEL_LS520 = "C.T.S.  LS520"

    Enum ComboFont
        COMBO_NO_PRINT
        COMBO_FONT_NORMAL
        COMBO_FONT_BOLD
        COMBO_FONT_15_INCH
        COMBO_FONT_DOUBLE_HIGH
    End Enum

    Dim LsReply As LsFamily.LsReply
    Dim bgColor As Color
    Friend WithEvents cbPrint4 As System.Windows.Forms.ComboBox
    Friend WithEvents tbPrintString4 As System.Windows.Forms.TextBox
    Friend WithEvents cbPrint3 As System.Windows.Forms.ComboBox
    Friend WithEvents tbPrintString3 As System.Windows.Forms.TextBox
    Friend WithEvents cbPrint2 As System.Windows.Forms.ComboBox
    Friend WithEvents tbPrintString2 As System.Windows.Forms.TextBox
    Friend WithEvents cbPrint1 As System.Windows.Forms.ComboBox
    Friend WithEvents rbColor300dpi As System.Windows.Forms.RadioButton
    Friend WithEvents cbE13bMicr As System.Windows.Forms.CheckBox
    Friend WithEvents rbUV100dpi As System.Windows.Forms.RadioButton
    Friend WithEvents rbUV200dpi As System.Windows.Forms.RadioButton
    Friend WithEvents gbUV As System.Windows.Forms.GroupBox
    Friend WithEvents tbPWMvalue As System.Windows.Forms.TextBox
    Friend WithEvents cbHightContrast As System.Windows.Forms.CheckBox
    Friend WithEvents cbShowUVImage As System.Windows.Forms.CheckBox
    Dim setColor As Color

#Region " Codice generato da Progettazione Windows Form "

    Public Sub New()
        MyBase.New()

        'Chiamata richiesta da Progettazione Windows Form.
        InitializeComponent()

        'Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent()

    End Sub

    'Form esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Richiesto da Progettazione Windows Form
    Private components As System.ComponentModel.IContainer

    'NOTA: la procedura che segue è richiesta da Progettazione Windows Form.
    'Può essere modificata in Progettazione Windows Form.  
    'Non modificarla nell'editor del codice.
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents rbBWImage As System.Windows.Forms.RadioButton
    Friend WithEvents rb16g100dpi As System.Windows.Forms.RadioButton
    Friend WithEvents rb16g200dpi As System.Windows.Forms.RadioButton
    Friend WithEvents rb256g100dpi As System.Windows.Forms.RadioButton
    Friend WithEvents rbSideAll As System.Windows.Forms.RadioButton
    Friend WithEvents rbSideFront As System.Windows.Forms.RadioButton
    Friend WithEvents rbSideRear As System.Windows.Forms.RadioButton
    Friend WithEvents rbSideNone As System.Windows.Forms.RadioButton
    Friend WithEvents cbMICR As System.Windows.Forms.CheckBox
    Friend WithEvents cbOCRsw As System.Windows.Forms.CheckBox
    Friend WithEvents cbPrint As System.Windows.Forms.ComboBox
    Friend WithEvents cbFrontStamp As System.Windows.Forms.CheckBox
    Friend WithEvents cbBeepOnError As System.Windows.Forms.CheckBox
    Friend WithEvents cbWaitDoc As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Friend WithEvents bOptionOK As System.Windows.Forms.Button
    Friend WithEvents bOptionCancel As System.Windows.Forms.Button
    Friend WithEvents rb256g200dpi As System.Windows.Forms.RadioButton
    Friend WithEvents rbColor200dpi As System.Windows.Forms.RadioButton
    Friend WithEvents rbColor100dpi As System.Windows.Forms.RadioButton
    Friend WithEvents tbPrintString As System.Windows.Forms.TextBox
    Friend WithEvents cbBarcode As System.Windows.Forms.CheckBox
    Friend WithEvents bBarcodexy As System.Windows.Forms.Button
    Friend WithEvents bOCRxy As System.Windows.Forms.Button
    Friend WithEvents rbFileBMP As System.Windows.Forms.RadioButton
    Friend WithEvents tbJPEGq As System.Windows.Forms.TextBox
    Friend WithEvents rbTIFF As System.Windows.Forms.RadioButton
    Friend WithEvents rbTiffCCITT As System.Windows.Forms.RadioButton
    Friend WithEvents rbTiffGr4 As System.Windows.Forms.RadioButton
    Friend WithEvents rbFileNone As System.Windows.Forms.RadioButton
    Friend WithEvents rbFileJPEG As System.Windows.Forms.RadioButton
    Friend WithEvents grbPrint As System.Windows.Forms.GroupBox
    Friend WithEvents gbIPAddress As System.Windows.Forms.GroupBox
    Friend WithEvents tbIPAddress As System.Windows.Forms.TextBox
    Friend WithEvents rb256g300dpi As System.Windows.Forms.RadioButton
    Friend WithEvents rb16g300dpi As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents gbDoubleLeafing As System.Windows.Forms.GroupBox
    Friend WithEvents lbDoubleLeafing As System.Windows.Forms.Label
    Friend WithEvents tbDoubleLeafing As System.Windows.Forms.TrackBar
    Friend WithEvents cbSorter As System.Windows.Forms.ComboBox
    Friend WithEvents cbPrintHigh As System.Windows.Forms.CheckBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.cbBeepOnError = New System.Windows.Forms.CheckBox
        Me.grbPrint = New System.Windows.Forms.GroupBox
        Me.cbPrint1 = New System.Windows.Forms.ComboBox
        Me.cbPrint4 = New System.Windows.Forms.ComboBox
        Me.tbPrintString4 = New System.Windows.Forms.TextBox
        Me.cbPrint3 = New System.Windows.Forms.ComboBox
        Me.tbPrintString3 = New System.Windows.Forms.TextBox
        Me.cbPrint2 = New System.Windows.Forms.ComboBox
        Me.tbPrintString2 = New System.Windows.Forms.TextBox
        Me.cbPrintHigh = New System.Windows.Forms.CheckBox
        Me.cbPrint = New System.Windows.Forms.ComboBox
        Me.tbPrintString = New System.Windows.Forms.TextBox
        Me.GroupBox4 = New System.Windows.Forms.GroupBox
        Me.cbE13bMicr = New System.Windows.Forms.CheckBox
        Me.bBarcodexy = New System.Windows.Forms.Button
        Me.cbBarcode = New System.Windows.Forms.CheckBox
        Me.bOCRxy = New System.Windows.Forms.Button
        Me.cbOCRsw = New System.Windows.Forms.CheckBox
        Me.cbMICR = New System.Windows.Forms.CheckBox
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.rbSideNone = New System.Windows.Forms.RadioButton
        Me.rbSideRear = New System.Windows.Forms.RadioButton
        Me.rbSideFront = New System.Windows.Forms.RadioButton
        Me.rbSideAll = New System.Windows.Forms.RadioButton
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.rbUV100dpi = New System.Windows.Forms.RadioButton
        Me.rbColor300dpi = New System.Windows.Forms.RadioButton
        Me.rb16g300dpi = New System.Windows.Forms.RadioButton
        Me.rbUV200dpi = New System.Windows.Forms.RadioButton
        Me.rb256g300dpi = New System.Windows.Forms.RadioButton
        Me.rbColor200dpi = New System.Windows.Forms.RadioButton
        Me.rb256g200dpi = New System.Windows.Forms.RadioButton
        Me.rb256g100dpi = New System.Windows.Forms.RadioButton
        Me.rb16g200dpi = New System.Windows.Forms.RadioButton
        Me.rb16g100dpi = New System.Windows.Forms.RadioButton
        Me.rbColor100dpi = New System.Windows.Forms.RadioButton
        Me.rbBWImage = New System.Windows.Forms.RadioButton
        Me.cbWaitDoc = New System.Windows.Forms.CheckBox
        Me.cbFrontStamp = New System.Windows.Forms.CheckBox
        Me.GroupBox6 = New System.Windows.Forms.GroupBox
        Me.rbFileNone = New System.Windows.Forms.RadioButton
        Me.rbTiffGr4 = New System.Windows.Forms.RadioButton
        Me.rbTiffCCITT = New System.Windows.Forms.RadioButton
        Me.rbTIFF = New System.Windows.Forms.RadioButton
        Me.tbJPEGq = New System.Windows.Forms.TextBox
        Me.rbFileJPEG = New System.Windows.Forms.RadioButton
        Me.rbFileBMP = New System.Windows.Forms.RadioButton
        Me.bOptionOK = New System.Windows.Forms.Button
        Me.bOptionCancel = New System.Windows.Forms.Button
        Me.gbIPAddress = New System.Windows.Forms.GroupBox
        Me.tbIPAddress = New System.Windows.Forms.TextBox
        Me.GroupBox5 = New System.Windows.Forms.GroupBox
        Me.cbSorter = New System.Windows.Forms.ComboBox
        Me.gbDoubleLeafing = New System.Windows.Forms.GroupBox
        Me.lbDoubleLeafing = New System.Windows.Forms.Label
        Me.tbDoubleLeafing = New System.Windows.Forms.TrackBar
        Me.gbUV = New System.Windows.Forms.GroupBox
        Me.cbShowUVImage = New System.Windows.Forms.CheckBox
        Me.cbHightContrast = New System.Windows.Forms.CheckBox
        Me.tbPWMvalue = New System.Windows.Forms.TextBox
        Me.GroupBox1.SuspendLayout()
        Me.grbPrint.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        Me.gbIPAddress.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.gbDoubleLeafing.SuspendLayout()
        CType(Me.tbDoubleLeafing, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbUV.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.cbBeepOnError)
        Me.GroupBox1.Controls.Add(Me.grbPrint)
        Me.GroupBox1.Controls.Add(Me.GroupBox4)
        Me.GroupBox1.Controls.Add(Me.GroupBox3)
        Me.GroupBox1.Controls.Add(Me.GroupBox2)
        Me.GroupBox1.Location = New System.Drawing.Point(13, 14)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(460, 314)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Docoments Handle"
        '
        'cbBeepOnError
        '
        Me.cbBeepOnError.Location = New System.Drawing.Point(172, 141)
        Me.cbBeepOnError.Name = "cbBeepOnError"
        Me.cbBeepOnError.Size = New System.Drawing.Size(75, 20)
        Me.cbBeepOnError.TabIndex = 5
        Me.cbBeepOnError.Text = "Beep On Error"
        '
        'grbPrint
        '
        Me.grbPrint.Controls.Add(Me.cbPrint1)
        Me.grbPrint.Controls.Add(Me.cbPrint4)
        Me.grbPrint.Controls.Add(Me.tbPrintString4)
        Me.grbPrint.Controls.Add(Me.cbPrint3)
        Me.grbPrint.Controls.Add(Me.tbPrintString3)
        Me.grbPrint.Controls.Add(Me.cbPrint2)
        Me.grbPrint.Controls.Add(Me.tbPrintString2)
        Me.grbPrint.Controls.Add(Me.cbPrintHigh)
        Me.grbPrint.Controls.Add(Me.cbPrint)
        Me.grbPrint.Controls.Add(Me.tbPrintString)
        Me.grbPrint.Enabled = False
        Me.grbPrint.Location = New System.Drawing.Point(167, 167)
        Me.grbPrint.Name = "grbPrint"
        Me.grbPrint.Size = New System.Drawing.Size(280, 129)
        Me.grbPrint.TabIndex = 3
        Me.grbPrint.TabStop = False
        Me.grbPrint.Text = "String to Print"
        '
        'cbPrint1
        '
        Me.cbPrint1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbPrint1.Items.AddRange(New Object() {"No Print", "Print 10", "Bold", "Print 15", "Double High"})
        Me.cbPrint1.Location = New System.Drawing.Point(7, 37)
        Me.cbPrint1.Name = "cbPrint1"
        Me.cbPrint1.Size = New System.Drawing.Size(80, 21)
        Me.cbPrint1.TabIndex = 11
        Me.cbPrint1.Visible = False
        '
        'cbPrint4
        '
        Me.cbPrint4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbPrint4.Enabled = False
        Me.cbPrint4.Items.AddRange(New Object() {"No Print", "Print 10", "Bold", "Print 15"})
        Me.cbPrint4.Location = New System.Drawing.Point(7, 108)
        Me.cbPrint4.Name = "cbPrint4"
        Me.cbPrint4.Size = New System.Drawing.Size(80, 21)
        Me.cbPrint4.TabIndex = 9
        '
        'tbPrintString4
        '
        Me.tbPrintString4.Enabled = False
        Me.tbPrintString4.Location = New System.Drawing.Point(93, 108)
        Me.tbPrintString4.Name = "tbPrintString4"
        Me.tbPrintString4.Size = New System.Drawing.Size(180, 20)
        Me.tbPrintString4.TabIndex = 8
        Me.tbPrintString4.Text = "CTS Electronics Line 4"
        '
        'cbPrint3
        '
        Me.cbPrint3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbPrint3.Enabled = False
        Me.cbPrint3.Items.AddRange(New Object() {"No Print", "Print 10", "Bold", "Print 15", "Double High"})
        Me.cbPrint3.Location = New System.Drawing.Point(7, 84)
        Me.cbPrint3.Name = "cbPrint3"
        Me.cbPrint3.Size = New System.Drawing.Size(80, 21)
        Me.cbPrint3.TabIndex = 7
        '
        'tbPrintString3
        '
        Me.tbPrintString3.Enabled = False
        Me.tbPrintString3.Location = New System.Drawing.Point(93, 84)
        Me.tbPrintString3.Name = "tbPrintString3"
        Me.tbPrintString3.Size = New System.Drawing.Size(180, 20)
        Me.tbPrintString3.TabIndex = 6
        Me.tbPrintString3.Text = "CTS Electronics Line 3"
        '
        'cbPrint2
        '
        Me.cbPrint2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbPrint2.Enabled = False
        Me.cbPrint2.Items.AddRange(New Object() {"No Print", "Print 10", "Bold", "Print 15", "Double High"})
        Me.cbPrint2.Location = New System.Drawing.Point(7, 61)
        Me.cbPrint2.Name = "cbPrint2"
        Me.cbPrint2.Size = New System.Drawing.Size(80, 21)
        Me.cbPrint2.TabIndex = 5
        '
        'tbPrintString2
        '
        Me.tbPrintString2.Enabled = False
        Me.tbPrintString2.Location = New System.Drawing.Point(93, 61)
        Me.tbPrintString2.Name = "tbPrintString2"
        Me.tbPrintString2.Size = New System.Drawing.Size(180, 20)
        Me.tbPrintString2.TabIndex = 4
        Me.tbPrintString2.Text = "CTS Electronics Line 2"
        '
        'cbPrintHigh
        '
        Me.cbPrintHigh.Enabled = False
        Me.cbPrintHigh.Location = New System.Drawing.Point(192, 10)
        Me.cbPrintHigh.Name = "cbPrintHigh"
        Me.cbPrintHigh.Size = New System.Drawing.Size(80, 20)
        Me.cbPrintHigh.TabIndex = 3
        Me.cbPrintHigh.Text = "Print High"
        '
        'cbPrint
        '
        Me.cbPrint.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbPrint.Items.AddRange(New Object() {"No Print", "Print 10", "Bold", "Print 15"})
        Me.cbPrint.Location = New System.Drawing.Point(7, 37)
        Me.cbPrint.Name = "cbPrint"
        Me.cbPrint.Size = New System.Drawing.Size(80, 21)
        Me.cbPrint.TabIndex = 2
        '
        'tbPrintString
        '
        Me.tbPrintString.Location = New System.Drawing.Point(93, 38)
        Me.tbPrintString.Name = "tbPrintString"
        Me.tbPrintString.Size = New System.Drawing.Size(180, 20)
        Me.tbPrintString.TabIndex = 0
        Me.tbPrintString.Text = "CTS Electronics"
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.cbE13bMicr)
        Me.GroupBox4.Controls.Add(Me.bBarcodexy)
        Me.GroupBox4.Controls.Add(Me.cbBarcode)
        Me.GroupBox4.Controls.Add(Me.bOCRxy)
        Me.GroupBox4.Controls.Add(Me.cbOCRsw)
        Me.GroupBox4.Controls.Add(Me.cbMICR)
        Me.GroupBox4.Location = New System.Drawing.Point(253, 21)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(187, 140)
        Me.GroupBox4.TabIndex = 2
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Codeline to Read"
        '
        'cbE13bMicr
        '
        Me.cbE13bMicr.Enabled = False
        Me.cbE13bMicr.Location = New System.Drawing.Point(12, 48)
        Me.cbE13bMicr.Name = "cbE13bMicr"
        Me.cbE13bMicr.Size = New System.Drawing.Size(128, 21)
        Me.cbE13bMicr.TabIndex = 6
        Me.cbE13bMicr.Text = "E13B MICR + OCR"
        '
        'bBarcodexy
        '
        Me.bBarcodexy.Enabled = False
        Me.bBarcodexy.Location = New System.Drawing.Point(106, 107)
        Me.bBarcodexy.Name = "bBarcodexy"
        Me.bBarcodexy.Size = New System.Drawing.Size(73, 20)
        Me.bBarcodexy.TabIndex = 5
        Me.bBarcodexy.Text = "Coordinate"
        '
        'cbBarcode
        '
        Me.cbBarcode.Location = New System.Drawing.Point(12, 107)
        Me.cbBarcode.Name = "cbBarcode"
        Me.cbBarcode.Size = New System.Drawing.Size(96, 21)
        Me.cbBarcode.TabIndex = 4
        Me.cbBarcode.Text = "Barcode Sw."
        '
        'bOCRxy
        '
        Me.bOCRxy.Enabled = False
        Me.bOCRxy.Location = New System.Drawing.Point(106, 80)
        Me.bOCRxy.Name = "bOCRxy"
        Me.bOCRxy.Size = New System.Drawing.Size(73, 19)
        Me.bOCRxy.TabIndex = 2
        Me.bOCRxy.Text = "Coordinate"
        '
        'cbOCRsw
        '
        Me.cbOCRsw.Location = New System.Drawing.Point(12, 80)
        Me.cbOCRsw.Name = "cbOCRsw"
        Me.cbOCRsw.Size = New System.Drawing.Size(76, 20)
        Me.cbOCRsw.TabIndex = 1
        Me.cbOCRsw.Text = "OCR Sw."
        '
        'cbMICR
        '
        Me.cbMICR.Location = New System.Drawing.Point(13, 21)
        Me.cbMICR.Name = "cbMICR"
        Me.cbMICR.Size = New System.Drawing.Size(67, 21)
        Me.cbMICR.TabIndex = 0
        Me.cbMICR.Text = "MICR"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.rbSideNone)
        Me.GroupBox3.Controls.Add(Me.rbSideRear)
        Me.GroupBox3.Controls.Add(Me.rbSideFront)
        Me.GroupBox3.Controls.Add(Me.rbSideAll)
        Me.GroupBox3.Location = New System.Drawing.Point(171, 21)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(73, 111)
        Me.GroupBox3.TabIndex = 1
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Side"
        '
        'rbSideNone
        '
        Me.rbSideNone.Location = New System.Drawing.Point(13, 83)
        Me.rbSideNone.Name = "rbSideNone"
        Me.rbSideNone.Size = New System.Drawing.Size(54, 21)
        Me.rbSideNone.TabIndex = 3
        Me.rbSideNone.Text = "None"
        '
        'rbSideRear
        '
        Me.rbSideRear.Location = New System.Drawing.Point(13, 62)
        Me.rbSideRear.Name = "rbSideRear"
        Me.rbSideRear.Size = New System.Drawing.Size(54, 21)
        Me.rbSideRear.TabIndex = 2
        Me.rbSideRear.Text = "Rear"
        '
        'rbSideFront
        '
        Me.rbSideFront.Location = New System.Drawing.Point(13, 42)
        Me.rbSideFront.Name = "rbSideFront"
        Me.rbSideFront.Size = New System.Drawing.Size(54, 20)
        Me.rbSideFront.TabIndex = 1
        Me.rbSideFront.Text = "Front"
        '
        'rbSideAll
        '
        Me.rbSideAll.Location = New System.Drawing.Point(13, 21)
        Me.rbSideAll.Name = "rbSideAll"
        Me.rbSideAll.Size = New System.Drawing.Size(54, 21)
        Me.rbSideAll.TabIndex = 0
        Me.rbSideAll.Text = "All"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.rbUV100dpi)
        Me.GroupBox2.Controls.Add(Me.rbColor300dpi)
        Me.GroupBox2.Controls.Add(Me.rb16g300dpi)
        Me.GroupBox2.Controls.Add(Me.rbUV200dpi)
        Me.GroupBox2.Controls.Add(Me.rb256g300dpi)
        Me.GroupBox2.Controls.Add(Me.rbColor200dpi)
        Me.GroupBox2.Controls.Add(Me.rb256g200dpi)
        Me.GroupBox2.Controls.Add(Me.rb256g100dpi)
        Me.GroupBox2.Controls.Add(Me.rb16g200dpi)
        Me.GroupBox2.Controls.Add(Me.rb16g100dpi)
        Me.GroupBox2.Controls.Add(Me.rbColor100dpi)
        Me.GroupBox2.Controls.Add(Me.rbBWImage)
        Me.GroupBox2.Location = New System.Drawing.Point(20, 21)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(141, 287)
        Me.GroupBox2.TabIndex = 0
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Scan Mode"
        '
        'rbUV100dpi
        '
        Me.rbUV100dpi.Appearance = System.Windows.Forms.Appearance.Button
        Me.rbUV100dpi.Enabled = False
        Me.rbUV100dpi.Location = New System.Drawing.Point(7, 233)
        Me.rbUV100dpi.Name = "rbUV100dpi"
        Me.rbUV100dpi.Size = New System.Drawing.Size(119, 21)
        Me.rbUV100dpi.TabIndex = 10
        Me.rbUV100dpi.Text = "256 gray - UV 100 dpi "
        Me.rbUV100dpi.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'rbColor300dpi
        '
        Me.rbColor300dpi.Appearance = System.Windows.Forms.Appearance.Button
        Me.rbColor300dpi.Enabled = False
        Me.rbColor300dpi.Location = New System.Drawing.Point(7, 212)
        Me.rbColor300dpi.Name = "rbColor300dpi"
        Me.rbColor300dpi.Size = New System.Drawing.Size(119, 21)
        Me.rbColor300dpi.TabIndex = 9
        Me.rbColor300dpi.Text = "Color 300 dpi"
        Me.rbColor300dpi.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'rb16g300dpi
        '
        Me.rb16g300dpi.Appearance = System.Windows.Forms.Appearance.Button
        Me.rb16g300dpi.Enabled = False
        Me.rb16g300dpi.Location = New System.Drawing.Point(7, 84)
        Me.rb16g300dpi.Name = "rb16g300dpi"
        Me.rb16g300dpi.Size = New System.Drawing.Size(119, 21)
        Me.rb16g300dpi.TabIndex = 3
        Me.rb16g300dpi.Text = "16 gray 300 dpi"
        Me.rb16g300dpi.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'rbUV200dpi
        '
        Me.rbUV200dpi.Appearance = System.Windows.Forms.Appearance.Button
        Me.rbUV200dpi.Enabled = False
        Me.rbUV200dpi.Location = New System.Drawing.Point(7, 257)
        Me.rbUV200dpi.Name = "rbUV200dpi"
        Me.rbUV200dpi.Size = New System.Drawing.Size(119, 21)
        Me.rbUV200dpi.TabIndex = 11
        Me.rbUV200dpi.Text = "256 gray - UV 200 dpi"
        Me.rbUV200dpi.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'rb256g300dpi
        '
        Me.rb256g300dpi.Appearance = System.Windows.Forms.Appearance.Button
        Me.rb256g300dpi.Enabled = False
        Me.rb256g300dpi.Location = New System.Drawing.Point(7, 146)
        Me.rb256g300dpi.Name = "rb256g300dpi"
        Me.rb256g300dpi.Size = New System.Drawing.Size(119, 21)
        Me.rb256g300dpi.TabIndex = 6
        Me.rb256g300dpi.Text = "256 gray 300 dpi"
        Me.rb256g300dpi.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'rbColor200dpi
        '
        Me.rbColor200dpi.Appearance = System.Windows.Forms.Appearance.Button
        Me.rbColor200dpi.Enabled = False
        Me.rbColor200dpi.Location = New System.Drawing.Point(7, 190)
        Me.rbColor200dpi.Name = "rbColor200dpi"
        Me.rbColor200dpi.Size = New System.Drawing.Size(119, 21)
        Me.rbColor200dpi.TabIndex = 8
        Me.rbColor200dpi.Text = "Color 200 dpi"
        Me.rbColor200dpi.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'rb256g200dpi
        '
        Me.rb256g200dpi.Appearance = System.Windows.Forms.Appearance.Button
        Me.rb256g200dpi.Location = New System.Drawing.Point(7, 126)
        Me.rb256g200dpi.Name = "rb256g200dpi"
        Me.rb256g200dpi.Size = New System.Drawing.Size(119, 20)
        Me.rb256g200dpi.TabIndex = 5
        Me.rb256g200dpi.Text = "256 gray 200 dpi"
        Me.rb256g200dpi.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'rb256g100dpi
        '
        Me.rb256g100dpi.Appearance = System.Windows.Forms.Appearance.Button
        Me.rb256g100dpi.Location = New System.Drawing.Point(7, 105)
        Me.rb256g100dpi.Name = "rb256g100dpi"
        Me.rb256g100dpi.Size = New System.Drawing.Size(119, 21)
        Me.rb256g100dpi.TabIndex = 4
        Me.rb256g100dpi.Text = "256 gray 100 dpi"
        Me.rb256g100dpi.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'rb16g200dpi
        '
        Me.rb16g200dpi.Appearance = System.Windows.Forms.Appearance.Button
        Me.rb16g200dpi.Location = New System.Drawing.Point(7, 63)
        Me.rb16g200dpi.Name = "rb16g200dpi"
        Me.rb16g200dpi.Size = New System.Drawing.Size(119, 21)
        Me.rb16g200dpi.TabIndex = 2
        Me.rb16g200dpi.Text = "16 gray 200 dpi"
        Me.rb16g200dpi.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'rb16g100dpi
        '
        Me.rb16g100dpi.Appearance = System.Windows.Forms.Appearance.Button
        Me.rb16g100dpi.Location = New System.Drawing.Point(7, 42)
        Me.rb16g100dpi.Name = "rb16g100dpi"
        Me.rb16g100dpi.Size = New System.Drawing.Size(119, 21)
        Me.rb16g100dpi.TabIndex = 1
        Me.rb16g100dpi.Text = "16 gray 100 dpi"
        Me.rb16g100dpi.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'rbColor100dpi
        '
        Me.rbColor100dpi.Appearance = System.Windows.Forms.Appearance.Button
        Me.rbColor100dpi.Enabled = False
        Me.rbColor100dpi.Location = New System.Drawing.Point(7, 168)
        Me.rbColor100dpi.Name = "rbColor100dpi"
        Me.rbColor100dpi.Size = New System.Drawing.Size(119, 21)
        Me.rbColor100dpi.TabIndex = 7
        Me.rbColor100dpi.Text = "Color 100 dpi"
        Me.rbColor100dpi.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'rbBWImage
        '
        Me.rbBWImage.Appearance = System.Windows.Forms.Appearance.Button
        Me.rbBWImage.Location = New System.Drawing.Point(7, 22)
        Me.rbBWImage.Name = "rbBWImage"
        Me.rbBWImage.Size = New System.Drawing.Size(119, 20)
        Me.rbBWImage.TabIndex = 0
        Me.rbBWImage.Text = "B/W Image"
        Me.rbBWImage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cbWaitDoc
        '
        Me.cbWaitDoc.Location = New System.Drawing.Point(173, 340)
        Me.cbWaitDoc.Name = "cbWaitDoc"
        Me.cbWaitDoc.Size = New System.Drawing.Size(82, 20)
        Me.cbWaitDoc.TabIndex = 6
        Me.cbWaitDoc.Text = "Wait Doc."
        '
        'cbFrontStamp
        '
        Me.cbFrontStamp.Enabled = False
        Me.cbFrontStamp.Location = New System.Drawing.Point(55, 340)
        Me.cbFrontStamp.Name = "cbFrontStamp"
        Me.cbFrontStamp.Size = New System.Drawing.Size(92, 20)
        Me.cbFrontStamp.TabIndex = 4
        Me.cbFrontStamp.Text = "Front Stamp"
        '
        'GroupBox6
        '
        Me.GroupBox6.Controls.Add(Me.rbFileNone)
        Me.GroupBox6.Controls.Add(Me.rbTiffGr4)
        Me.GroupBox6.Controls.Add(Me.rbTiffCCITT)
        Me.GroupBox6.Controls.Add(Me.rbTIFF)
        Me.GroupBox6.Controls.Add(Me.tbJPEGq)
        Me.GroupBox6.Controls.Add(Me.rbFileJPEG)
        Me.GroupBox6.Controls.Add(Me.rbFileBMP)
        Me.GroupBox6.Location = New System.Drawing.Point(27, 366)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(287, 107)
        Me.GroupBox6.TabIndex = 1
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "Save Images On File type"
        '
        'rbFileNone
        '
        Me.rbFileNone.Location = New System.Drawing.Point(27, 22)
        Me.rbFileNone.Name = "rbFileNone"
        Me.rbFileNone.Size = New System.Drawing.Size(53, 21)
        Me.rbFileNone.TabIndex = 6
        Me.rbFileNone.Text = "None"
        '
        'rbTiffGr4
        '
        Me.rbTiffGr4.Location = New System.Drawing.Point(167, 76)
        Me.rbTiffGr4.Name = "rbTiffGr4"
        Me.rbTiffGr4.Size = New System.Drawing.Size(114, 21)
        Me.rbTiffGr4.TabIndex = 5
        Me.rbTiffGr4.Text = "CCITT Gr. 4"
        '
        'rbTiffCCITT
        '
        Me.rbTiffCCITT.Location = New System.Drawing.Point(167, 48)
        Me.rbTiffCCITT.Name = "rbTiffCCITT"
        Me.rbTiffCCITT.Size = New System.Drawing.Size(100, 21)
        Me.rbTiffCCITT.TabIndex = 4
        Me.rbTiffCCITT.Text = "CCITT"
        '
        'rbTIFF
        '
        Me.rbTIFF.Location = New System.Drawing.Point(167, 20)
        Me.rbTIFF.Name = "rbTIFF"
        Me.rbTIFF.Size = New System.Drawing.Size(80, 21)
        Me.rbTIFF.TabIndex = 3
        Me.rbTIFF.Text = "TIFF"
        '
        'tbJPEGq
        '
        Me.tbJPEGq.Enabled = False
        Me.tbJPEGq.Location = New System.Drawing.Point(87, 76)
        Me.tbJPEGq.MaxLength = 3
        Me.tbJPEGq.Name = "tbJPEGq"
        Me.tbJPEGq.Size = New System.Drawing.Size(33, 20)
        Me.tbJPEGq.TabIndex = 2
        Me.tbJPEGq.Text = "128"
        Me.tbJPEGq.WordWrap = False
        '
        'rbFileJPEG
        '
        Me.rbFileJPEG.Location = New System.Drawing.Point(27, 76)
        Me.rbFileJPEG.Name = "rbFileJPEG"
        Me.rbFileJPEG.Size = New System.Drawing.Size(63, 21)
        Me.rbFileJPEG.TabIndex = 1
        Me.rbFileJPEG.Text = "JPEG"
        '
        'rbFileBMP
        '
        Me.rbFileBMP.Location = New System.Drawing.Point(27, 49)
        Me.rbFileBMP.Name = "rbFileBMP"
        Me.rbFileBMP.Size = New System.Drawing.Size(53, 21)
        Me.rbFileBMP.TabIndex = 0
        Me.rbFileBMP.Text = "BMP"
        '
        'bOptionOK
        '
        Me.bOptionOK.Location = New System.Drawing.Point(500, 49)
        Me.bOptionOK.Name = "bOptionOK"
        Me.bOptionOK.Size = New System.Drawing.Size(67, 27)
        Me.bOptionOK.TabIndex = 3
        Me.bOptionOK.Text = "Ok"
        '
        'bOptionCancel
        '
        Me.bOptionCancel.Location = New System.Drawing.Point(500, 104)
        Me.bOptionCancel.Name = "bOptionCancel"
        Me.bOptionCancel.Size = New System.Drawing.Size(67, 28)
        Me.bOptionCancel.TabIndex = 4
        Me.bOptionCancel.Text = "Cancel"
        '
        'gbIPAddress
        '
        Me.gbIPAddress.Controls.Add(Me.tbIPAddress)
        Me.gbIPAddress.Enabled = False
        Me.gbIPAddress.Location = New System.Drawing.Point(480, 153)
        Me.gbIPAddress.Name = "gbIPAddress"
        Me.gbIPAddress.Size = New System.Drawing.Size(114, 62)
        Me.gbIPAddress.TabIndex = 5
        Me.gbIPAddress.TabStop = False
        Me.gbIPAddress.Text = "Addr. LS  IP"
        '
        'tbIPAddress
        '
        Me.tbIPAddress.Location = New System.Drawing.Point(7, 28)
        Me.tbIPAddress.Name = "tbIPAddress"
        Me.tbIPAddress.Size = New System.Drawing.Size(86, 20)
        Me.tbIPAddress.TabIndex = 0
        Me.tbIPAddress.Text = "IP Address"
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.cbSorter)
        Me.GroupBox5.Location = New System.Drawing.Point(320, 431)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(153, 48)
        Me.GroupBox5.TabIndex = 6
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Sorter"
        '
        'cbSorter
        '
        Me.cbSorter.Location = New System.Drawing.Point(13, 21)
        Me.cbSorter.Name = "cbSorter"
        Me.cbSorter.Size = New System.Drawing.Size(127, 21)
        Me.cbSorter.TabIndex = 0
        Me.cbSorter.Text = "Sorter"
        '
        'gbDoubleLeafing
        '
        Me.gbDoubleLeafing.Controls.Add(Me.lbDoubleLeafing)
        Me.gbDoubleLeafing.Controls.Add(Me.tbDoubleLeafing)
        Me.gbDoubleLeafing.Location = New System.Drawing.Point(320, 334)
        Me.gbDoubleLeafing.Name = "gbDoubleLeafing"
        Me.gbDoubleLeafing.Size = New System.Drawing.Size(153, 90)
        Me.gbDoubleLeafing.TabIndex = 2
        Me.gbDoubleLeafing.TabStop = False
        Me.gbDoubleLeafing.Text = "Double Leafing level"
        '
        'lbDoubleLeafing
        '
        Me.lbDoubleLeafing.Location = New System.Drawing.Point(27, 62)
        Me.lbDoubleLeafing.Name = "lbDoubleLeafing"
        Me.lbDoubleLeafing.Size = New System.Drawing.Size(113, 14)
        Me.lbDoubleLeafing.TabIndex = 2
        Me.lbDoubleLeafing.Text = " 2          3        4(d)"
        '
        'tbDoubleLeafing
        '
        Me.tbDoubleLeafing.LargeChange = 1
        Me.tbDoubleLeafing.Location = New System.Drawing.Point(20, 28)
        Me.tbDoubleLeafing.Maximum = 4
        Me.tbDoubleLeafing.Minimum = 2
        Me.tbDoubleLeafing.Name = "tbDoubleLeafing"
        Me.tbDoubleLeafing.Size = New System.Drawing.Size(107, 45)
        Me.tbDoubleLeafing.TabIndex = 0
        Me.tbDoubleLeafing.Value = 2
        '
        'gbUV
        '
        Me.gbUV.Controls.Add(Me.cbShowUVImage)
        Me.gbUV.Controls.Add(Me.cbHightContrast)
        Me.gbUV.Controls.Add(Me.tbPWMvalue)
        Me.gbUV.Enabled = False
        Me.gbUV.Location = New System.Drawing.Point(480, 248)
        Me.gbUV.Name = "gbUV"
        Me.gbUV.Size = New System.Drawing.Size(114, 112)
        Me.gbUV.TabIndex = 12
        Me.gbUV.TabStop = False
        Me.gbUV.Text = "PWM UV % value"
        '
        'cbShowUVImage
        '
        Me.cbShowUVImage.Location = New System.Drawing.Point(8, 79)
        Me.cbShowUVImage.Name = "cbShowUVImage"
        Me.cbShowUVImage.Size = New System.Drawing.Size(102, 21)
        Me.cbShowUVImage.TabIndex = 2
        Me.cbShowUVImage.Text = "Show UV image"
        '
        'cbHightContrast
        '
        Me.cbHightContrast.Location = New System.Drawing.Point(9, 57)
        Me.cbHightContrast.Name = "cbHightContrast"
        Me.cbHightContrast.Size = New System.Drawing.Size(96, 21)
        Me.cbHightContrast.TabIndex = 1
        Me.cbHightContrast.Text = "Hight Contrast"
        '
        'tbPWMvalue
        '
        Me.tbPWMvalue.Location = New System.Drawing.Point(7, 28)
        Me.tbPWMvalue.MaxLength = 3
        Me.tbPWMvalue.Name = "tbPWMvalue"
        Me.tbPWMvalue.Size = New System.Drawing.Size(86, 20)
        Me.tbPWMvalue.TabIndex = 0
        '
        'formOptions
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(600, 490)
        Me.Controls.Add(Me.gbUV)
        Me.Controls.Add(Me.cbWaitDoc)
        Me.Controls.Add(Me.cbFrontStamp)
        Me.Controls.Add(Me.gbIPAddress)
        Me.Controls.Add(Me.bOptionCancel)
        Me.Controls.Add(Me.bOptionOK)
        Me.Controls.Add(Me.gbDoubleLeafing)
        Me.Controls.Add(Me.GroupBox6)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.GroupBox5)
        Me.Name = "formOptions"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Ls scan Options"
        Me.GroupBox1.ResumeLayout(False)
        Me.grbPrint.ResumeLayout(False)
        Me.grbPrint.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        Me.gbIPAddress.ResumeLayout(False)
        Me.gbIPAddress.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        Me.gbDoubleLeafing.ResumeLayout(False)
        Me.gbDoubleLeafing.PerformLayout()
        CType(Me.tbDoubleLeafing, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbUV.ResumeLayout(False)
        Me.gbUV.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Function UnitConfig(ByVal Lsunit As Int16, ByRef LsCfg As LsFamily.LsConfiguration, ByRef Model As String, ByRef Version As String) As Int32
        Dim Ls As LsFamily.LsApi = New LsFamily.LsApi
        Dim hLs As Int16
        Dim rc As Int32

        Ls.LSDebug("----- formOptions_UnitConfig Inizio")
        rc = Ls.LSConnect(0, Lsunit, hLs, True)
        Ls.LSDebug("----- formOptions_UnitConfig 1")
        If (rc = LsFamily.LsReply.LS_OKAY Or rc = LsFamily.LsReply.LS_ALREADY_OPEN) Then

            Ls.LSDebug("----- formOptions_UnitConfig 2")
            rc = Ls.LSIdentify(hLs, 0, LsCfg, 0, 0, 0, 0)
            Ls.LSDebug("----- formOptions_UnitConfig 3")
            rc = Ls.LSDisconnect(hLs, 0)
            Ls.LSDebug("----- formOptions_UnitConfig 4")
        End If
        Ls.LSDebug("----- formOptions_UnitConfig Fine")

        Return rc
    End Function

    Private Sub formOptions_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim LsCfg As LsFamily.LsConfiguration
        Dim Ls As LsFamily.LsApi
        Dim Model As String = New String(" ", 20)
        Dim Version As String = New String(" ", 20)
        Dim rc As Int32
        Dim fColorHandled As Boolean = False
        Dim f300dpi As Boolean = False
        Dim fUV As Boolean = False

        bgColor = Me.BackColor
        setColor = Color.LightSalmon

        'Inizializzo i controlli della dialog
        LsCfg = New LsFamily.LsConfiguration
        Ls = New LsFamily.LsApi

        rc = UnitConfig(LsUnitType, LsCfg, Model, Version)

        If (LsUnitType <> LsFamily.LsDefines.LsUnitType.LS_100_ETH) Then

            If (LsUnitType = LsFamily.LsDefines.LsUnitType.LS_150_USB Or _
                LsUnitType = LsFamily.LsDefines.LsUnitType.LS_40_USB) Then
                f300dpi = True
                If (LsCfg.ScannerUltraViolet) Then
                    fUV = True
                End If
            End If
            If LsUnitType = LsFamily.LsDefines.LsUnitType.LS_515_USB Or _
               LsUnitType = LsFamily.LsDefines.LsUnitType.LS_800_USB Then
                fColorHandled = True
            End If


            'Ls.LSDebug(Model)

            'Try

            '    If (rc = LsReply.LS_OKAY) Then

            '        If Model.Substring(0, 7) = MODEL_LS105 Or _
            '           Model.Substring(0, 13) = MODEL_LS520 Then
            '            fColorHandled = True
            '        End If
            '    End If

            'Catch ex As Exception

            '    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK)
            'End Try
        End If

        Ls.LSDebug("----- formOptions_Load 2")
        ' ScanMode
        If fColorHandled Then
            rbColor100dpi.Enabled = True
            rbColor200dpi.Enabled = True
        End If

        If (LsCfg.ColorVersion) Then
            fColorHandled = True
            rbColor100dpi.Enabled = True
            rbColor200dpi.Enabled = True
            rbColor300dpi.Enabled = True

        End If

        If (fUV) Then
            rbUV100dpi.Enabled = True
            rbUV200dpi.Enabled = True
        End If


        If (LsUnitType = LsFamily.LsDefines.LsUnitType.LS_40_USB) Then
            cbE13bMicr.Enabled = True
        End If


        If (LsUnitType = LsFamily.LsDefines.LsUnitType.LS_150_USB Or _
            LsUnitType = LsFamily.LsDefines.LsUnitType.LS_40_USB) Then

            rb16g300dpi.Enabled = True
            rb256g300dpi.Enabled = True
        End If

        If LsUnitType = LsFamily.LsDefines.LsUnitType.LS_520_USB Then
            rb16g300dpi.Enabled = True
            rb256g300dpi.Enabled = True
            rbColor100dpi.Enabled = True
            rbColor200dpi.Enabled = True
            rbColor300dpi.Enabled = True
        End If

        If LsUnitType = LsFamily.LsDefines.LsUnitType.LS_150_ETH Then
            rbBWImage.Enabled = False
        End If

        Select Case cApplFunc.ScanMode
            Case LsFamily.LsDefines.ScanMode.SCAN_MODE_BW
                rbBWImage.Checked = True
            Case LsFamily.LsDefines.ScanMode.SCAN_MODE_16_GRAY_100
                rb16g100dpi.Checked = True
            Case LsFamily.LsDefines.ScanMode.SCAN_MODE_16_GRAY_200
                rb16g200dpi.Checked = True
            Case LsFamily.LsDefines.ScanMode.SCAN_MODE_16_GRAY_300
                If (f300dpi) Then
                    rb16g300dpi.Checked = True
                Else
                    rb16g200dpi.Checked = True
                End If
            Case LsFamily.LsDefines.ScanMode.SCAN_MODE_256_GRAY_100
                rb256g100dpi.Checked = True
            Case LsFamily.LsDefines.ScanMode.SCAN_MODE_256_GRAY_200
                rb256g200dpi.Checked = True
            Case LsFamily.LsDefines.ScanMode.SCAN_MODE_256_GRAY_300
                If (f300dpi) Then
                    rb256g300dpi.Checked = True
                Else
                    rb256g200dpi.Checked = True
                End If
            Case LsFamily.LsDefines.ScanMode.SCAN_MODE_COLOR_100
                If fColorHandled Then
                    rbColor100dpi.Checked = True
                Else
                    rb256g100dpi.Checked = True
                End If
            Case LsFamily.LsDefines.ScanMode.SCAN_MODE_COLOR_200
                If fColorHandled Then
                    rbColor200dpi.Checked = True
                Else
                    rb256g200dpi.Checked = True
                End If
            Case LsFamily.LsDefines.ScanMode.SCAN_MODE_COLOR_300
                If fColorHandled Then
                    rbColor300dpi.Checked = True
                Else
                    rb256g200dpi.Checked = True
                End If

            Case LsFamily.LsDefines.ScanMode.SCAN_MODE_256GR100_AND_UV
                If (fUV) Then
                    rbUV100dpi.Checked = True
                Else
                    rb256g100dpi.Checked = True
                End If

            Case LsFamily.LsDefines.ScanMode.SCAN_MODE_256GR200_AND_UV
                If (fUV) Then
                    rbUV200dpi.Checked = True
                Else
                    rb256g200dpi.Checked = True
                End If

            Case Else
                rb256g200dpi.Checked = True
        End Select

        ' Side
        Select Case cApplFunc.Side
            Case LsFamily.LsDefines.Side.SIDE_ALL_IMAGE
                rbSideAll.Checked = True
            Case LsFamily.LsDefines.Side.SIDE_FRONT_IMAGE
                rbSideFront.Checked = True
            Case LsFamily.LsDefines.Side.SIDE_BACK_IMAGE
                rbSideRear.Checked = True
            Case LsFamily.LsDefines.Side.SIDE_NONE_IMAGE
                rbSideNone.Checked = True
            Case Else
                rbSideFront.Checked = True
        End Select

        ' Codeline to Read
        If cApplFunc.Codeline_HW = LsFamily.LsDefines.CodelineToRead.READ_CODELINE_HW_MICR Then
            cbMICR.Checked = True
        Else
            cbMICR.Checked = False
        End If

        If (LsUnitType = LsFamily.LsDefines.LsUnitType.LS_800_USB) Then
            ' For Ls800 disable the ocr recognization
            cbOCRsw.Enabled = False
            cbBarcode.Enabled = False
        Else
            Select Case cApplFunc.CodelineType
                Case ApplClass.DECODE_MICR
                    cbMICR.Checked = True
                Case ApplClass.DECODE_E13B_MICR_AND_OCR
                    cbE13bMicr.Checked = True
                Case ApplClass.DECODE_OCR
                    cbOCRsw.Checked = True
                Case ApplClass.DECODE_BARCODE
                    cbBarcode.Checked = True
            End Select
        End If

        ' String to Print
        ' per LS520 4 linee sostituisco cbPrint con cbPrint1
        If LsCfg.InkJet_Printer_4_lines Then
            grbPrint.Enabled = True
            cbPrint.Visible = False
            cbPrint1.Visible = True
            cbPrint2.Enabled = True
            tbPrintString2.Enabled = True
            cbPrint3.Enabled = True
            tbPrintString3.Enabled = True
            cbPrint4.Enabled = True
            tbPrintString4.Enabled = True
        ElseIf LsCfg.InkJet_Printer Then
            grbPrint.Enabled = True
        End If
        cbPrint.SelectedIndex = cApplFunc.PrintValidate

        Select Case cApplFunc.PrintValidate1
            Case LsFamily.LsDefines.PrintFont.PRINT_FORMAT_NORMAL
                cbPrint1.SelectedIndex = ComboFont.COMBO_FONT_NORMAL
            Case LsFamily.LsDefines.PrintFont.PRINT_FORMAT_BOLD
                cbPrint1.SelectedIndex = ComboFont.COMBO_FONT_BOLD
            Case LsFamily.LsDefines.PrintFont.PRINT_FORMAT_NORMAL_15
                cbPrint1.SelectedIndex = ComboFont.COMBO_FONT_15_INCH
            Case LsFamily.LsDefines.PrintFont.PRINT_FORMAT_DOUBLE_HIGH
                cbPrint1.SelectedIndex = ComboFont.COMBO_FONT_DOUBLE_HIGH
            Case Else
                cbPrint1.SelectedIndex = ComboFont.COMBO_NO_PRINT
        End Select
        tbPrintString.Text = cApplFunc.Endorse_str

        Select Case cApplFunc.PrintValidate2
            Case LsFamily.LsDefines.PrintFont.PRINT_FORMAT_NORMAL
                cbPrint2.SelectedIndex = ComboFont.COMBO_FONT_NORMAL
            Case LsFamily.LsDefines.PrintFont.PRINT_FORMAT_BOLD
                cbPrint2.SelectedIndex = ComboFont.COMBO_FONT_BOLD
            Case LsFamily.LsDefines.PrintFont.PRINT_FORMAT_NORMAL_15
                cbPrint2.SelectedIndex = ComboFont.COMBO_FONT_15_INCH
            Case LsFamily.LsDefines.PrintFont.PRINT_FORMAT_DOUBLE_HIGH
                cbPrint2.SelectedIndex = ComboFont.COMBO_FONT_DOUBLE_HIGH
            Case Else
                cbPrint2.SelectedIndex = ComboFont.COMBO_NO_PRINT
        End Select
        tbPrintString2.Text = cApplFunc.Endorse_str2

        Select Case cApplFunc.PrintValidate3
            Case LsFamily.LsDefines.PrintFont.PRINT_FORMAT_NORMAL
                cbPrint3.SelectedIndex = ComboFont.COMBO_FONT_NORMAL
            Case LsFamily.LsDefines.PrintFont.PRINT_FORMAT_BOLD
                cbPrint3.SelectedIndex = ComboFont.COMBO_FONT_BOLD
            Case LsFamily.LsDefines.PrintFont.PRINT_FORMAT_NORMAL_15
                cbPrint3.SelectedIndex = ComboFont.COMBO_FONT_15_INCH
            Case LsFamily.LsDefines.PrintFont.PRINT_FORMAT_DOUBLE_HIGH
                cbPrint3.SelectedIndex = ComboFont.COMBO_FONT_DOUBLE_HIGH
            Case Else
                cbPrint3.SelectedIndex = ComboFont.COMBO_NO_PRINT
        End Select
        tbPrintString3.Text = cApplFunc.Endorse_str3

        Select Case cApplFunc.PrintValidate4
            Case LsFamily.LsDefines.PrintFont.PRINT_FORMAT_NORMAL
                cbPrint4.SelectedIndex = ComboFont.COMBO_FONT_NORMAL
            Case LsFamily.LsDefines.PrintFont.PRINT_FORMAT_BOLD
                cbPrint4.SelectedIndex = ComboFont.COMBO_FONT_BOLD
            Case LsFamily.LsDefines.PrintFont.PRINT_FORMAT_NORMAL_15
                cbPrint4.SelectedIndex = ComboFont.COMBO_FONT_15_INCH
            Case LsFamily.LsDefines.PrintFont.PRINT_FORMAT_DOUBLE_HIGH
                cbPrint4.SelectedIndex = ComboFont.COMBO_FONT_DOUBLE_HIGH
            Case Else
                cbPrint4.SelectedIndex = ComboFont.COMBO_NO_PRINT
        End Select
        tbPrintString4.Text = cApplFunc.Endorse_str4

        If LsUnitType = LsFamily.LsDefines.LsUnitType.LS_150_USB Then
            cbPrintHigh.Enabled = True
            If (cApplFunc.Print_High) Then
                cbPrintHigh.Checked = True
            End If
        End If

        ' Front Stamp
        If (LsCfg.Stamp_Front = True) Then
            cbFrontStamp.Enabled = True
            If (cApplFunc.FrontStamp = LsFamily.LsDefines.Stamp.STAMP_FRONT) Then
                cbFrontStamp.Checked = True
                'cbFrontStamp.Visible = True

            End If
        End If


        ' Beep on Error
        If (cApplFunc.Beep = LsFamily.LsDefines.Beep.BEEP_YES) Then
            cbBeepOnError.Checked = True
        End If

        ' Wait Documment
        If (cApplFunc.WiatDoc = False) Then
            cbWaitDoc.Checked = True
        End If

        If (cApplFunc.HightContrast = True) Then
            cbHightContrast.Checked = True
        End If

        If (cApplFunc.ShowUvImage = True) Then
            cbShowUVImage.Checked = True
        End If

        'Save On File
        If cApplFunc.SaveMode = LsFamily.LsDefines.ImageSave.IMAGE_SAVE_ON_FILE Or _
           cApplFunc.SaveMode = LsFamily.LsDefines.ImageSave.IMAGE_SAVE_BOTH Then

            Select Case cApplFunc.FileFormat
                Case LsFamily.LsDefines.FileType.FILE_BMP
                    rbFileBMP.Checked = True
                Case LsFamily.LsDefines.FileType.FILE_JPEG
                    rbFileJPEG.Checked = True
                Case LsFamily.LsDefines.FileType.FILE_TIF
                    rbTIFF.Checked = True
                Case LsFamily.LsDefines.FileType.FILE_CCITT
                    rbTiffCCITT.Checked = True
                Case LsFamily.LsDefines.FileType.FILE_CCITT_GROUP4
                    rbTiffGr4.Checked = True
                Case Else
                    rbFileNone.Checked = True
            End Select
        Else
            rbFileNone.Checked = True
        End If

        ' Double Leafing Level
        If (LsUnitType = LsFamily.LsDefines.LsUnitType.LS_40_USB Or _
            LsUnitType = LsFamily.LsDefines.LsUnitType.LS_150_ETH) Then
            gbDoubleLeafing.Enabled = False
        End If
        tbDoubleLeafing.Value = cApplFunc.DuobleLeafingValue

        'Sorters
        If (LsUnitType = LsFamily.LsDefines.LsUnitType.LS_100_USB Or LsUnitType = LsFamily.LsDefines.LsUnitType.LS_100_ETH Or _
            LsUnitType = LsFamily.LsDefines.LsUnitType.LS_40_USB) Then
            cbSorter.Items.AddRange(New Object() {"Pocket 1"})
            cbSorter.SelectedIndex = 0
        ElseIf (LsUnitType = LsFamily.LsDefines.LsUnitType.LS_150_USB Or LsUnitType = LsFamily.LsDefines.LsUnitType.LS_150_ETH) Then
            cbSorter.Items.AddRange(New Object() {"Pocket 1"})
            cbSorter.SelectedIndex = 0
        ElseIf (LsUnitType = LsFamily.LsDefines.LsUnitType.LS_515_USB Or LsUnitType = LsFamily.LsDefines.LsUnitType.LS_520_USB) Then
            cbSorter.Items.AddRange(New Object() {"Pocket 1", "Pocket 2", "Pocket 1 switch to 2", "Sort on Call-back"})
            Select Case cApplFunc.Sorter_Ls520
                Case LsFamily.LsDefines.Sorter.SORTER_POCKET_1
                    cbSorter.SelectedIndex = 0
                Case LsFamily.LsDefines.Sorter.SORTER_POCKET_2
                    cbSorter.SelectedIndex = 1
                Case LsFamily.LsDefines.Sorter.SORTER_SWICTH_1_TO_2
                    cbSorter.SelectedIndex = 2
                Case LsFamily.LsDefines.Sorter.SORTER_ON_CODELINE_CALLBACK
                    cbSorter.SelectedIndex = 3
                Case Else
                    cbSorter.SelectedIndex = 0
            End Select
        ElseIf (LsUnitType = LsFamily.LsDefines.LsUnitType.LS_800_USB) Then
            'cbSorter.Items.AddRange(New Object() {"Circular", "Sequential", "All docs Pocket 0", "All docs Pocket 1", "All docs Pocket 2", "All docs Pocket 3", "All docs Pocket 4", "All docs Pocket 5", "All docs Pocket 6", "All docs Pocket 7", "All docs Pocket 8", "All docs Pocket 9", "All docs Pocket 10", "All docs Pocket 11", "All docs Pocket 12", "All docs Pocket 13", "All docs Pocket 14", "All docs Pocket 15"})
            cbSorter.Items.AddRange(New Object() {"Circular", "Sequential", "All docs Pocket 0"})
            If (LsCfg.Sorters_Nr >= 1) Then
                cbSorter.Items.AddRange(New Object() {"All docs Pocket 1", "All docs Pocket 2", "All docs Pocket 3"})
            End If
            If (LsCfg.Sorters_Nr >= 2) Then
                cbSorter.Items.AddRange(New Object() {"All docs Pocket 4", "All docs Pocket 5", "All docs Pocket 6"})
            End If
            If (LsCfg.Sorters_Nr >= 3) Then
                cbSorter.Items.AddRange(New Object() {"All docs Pocket 7", "All docs Pocket 8", "All docs Pocket 9"})
            End If
            If (LsCfg.Sorters_Nr >= 4) Then
                cbSorter.Items.AddRange(New Object() {"All docs Pocket 10", "All docs Pocket 11", "All docs Pocket 12"})
            End If
            If (LsCfg.Sorters_Nr >= 5) Then
                cbSorter.Items.AddRange(New Object() {"All docs Pocket 13", "All docs Pocket 14", "All docs Pocket 15"})
            End If
            If (LsCfg.Sorters_Nr >= 6) Then
                cbSorter.Items.AddRange(New Object() {"All docs Pocket 16", "All docs Pocket 17", "All docs Pocket 18"})
            End If
            If (LsCfg.Sorters_Nr >= 7) Then
                cbSorter.Items.AddRange(New Object() {"All docs Pocket 19", "All docs Pocket 20", "All docs Pocket 21"})
            End If
            Select Case cApplFunc.Sorter_Ls800
                Case LsFamily.LsDefines.Sorter.SORTER_CIRCULAR
                    cbSorter.SelectedIndex = 0
                Case LsFamily.LsDefines.Sorter.SORTER_SEQUENTIAL
                    cbSorter.SelectedIndex = 1
                Case LsFamily.LsDefines.Sorter.SORTER_POCKET_0_SELECTED
                    cbSorter.SelectedIndex = 2
                Case LsFamily.LsDefines.Sorter.SORTER_POCKET_1_SELECTED
                    cbSorter.SelectedIndex = 3
                Case LsFamily.LsDefines.Sorter.SORTER_POCKET_2_SELECTED
                    cbSorter.SelectedIndex = 4
                Case LsFamily.LsDefines.Sorter.SORTER_POCKET_3_SELECTED
                    cbSorter.SelectedIndex = 5
                Case LsFamily.LsDefines.Sorter.SORTER_POCKET_4_SELECTED
                    cbSorter.SelectedIndex = 6
                Case LsFamily.LsDefines.Sorter.SORTER_POCKET_5_SELECTED
                    cbSorter.SelectedIndex = 7
                Case LsFamily.LsDefines.Sorter.SORTER_POCKET_6_SELECTED
                    cbSorter.SelectedIndex = 8
                Case LsFamily.LsDefines.Sorter.SORTER_POCKET_7_SELECTED
                    cbSorter.SelectedIndex = 9
                Case LsFamily.LsDefines.Sorter.SORTER_POCKET_8_SELECTED
                    cbSorter.SelectedIndex = 10
                Case LsFamily.LsDefines.Sorter.SORTER_POCKET_9_SELECTED
                    cbSorter.SelectedIndex = 11
                Case LsFamily.LsDefines.Sorter.SORTER_POCKET_10_SELECTED
                    cbSorter.SelectedIndex = 12
                Case LsFamily.LsDefines.Sorter.SORTER_POCKET_11_SELECTED
                    cbSorter.SelectedIndex = 13
                Case LsFamily.LsDefines.Sorter.SORTER_POCKET_12_SELECTED
                    cbSorter.SelectedIndex = 14
                Case LsFamily.LsDefines.Sorter.SORTER_POCKET_13_SELECTED
                    cbSorter.SelectedIndex = 15
                Case LsFamily.LsDefines.Sorter.SORTER_POCKET_14_SELECTED
                    cbSorter.SelectedIndex = 16
                Case LsFamily.LsDefines.Sorter.SORTER_POCKET_15_SELECTED
                    cbSorter.SelectedIndex = 17
                Case Else
                    cbSorter.SelectedIndex = 1
            End Select
        End If

        ' IP address
        If (LsUnitType = LsFamily.LsDefines.LsUnitType.LS_100_ETH Or _
            LsUnitType = LsFamily.LsDefines.LsUnitType.LS_150_ETH) Then
            gbIPAddress.Enabled = True
        End If

        'UV parameter
        If (LsUnitType = LsFamily.LsDefines.LsUnitType.LS_150_USB) Then
            If (LsCfg.ScannerUltraViolet) Then
                gbUV.Enabled = True
            End If

        End If

        tbPWMvalue.Text = cApplFunc.PWMvalue
        tbIPAddress.Text = cApplFunc.IP_Address
        Ls.LSDebug("----- formOptions_Load Fine")

    End Sub

    Private Sub bOptionOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bOptionOK.Click
        Dim Ls As LsFamily.LsApi = New LsFamily.LsApi


        ' ScanMode
        If rbBWImage.Checked = True Then
            cApplFunc.ScanMode = LsFamily.LsDefines.ScanMode.SCAN_MODE_BW
        ElseIf rb16g100dpi.Checked = True Then
            cApplFunc.ScanMode = LsFamily.LsDefines.ScanMode.SCAN_MODE_16_GRAY_100
        ElseIf rb16g200dpi.Checked = True Then
            cApplFunc.ScanMode = LsFamily.LsDefines.ScanMode.SCAN_MODE_16_GRAY_200
        ElseIf rb16g300dpi.Checked = True Then
            cApplFunc.ScanMode = LsFamily.LsDefines.ScanMode.SCAN_MODE_16_GRAY_300
        ElseIf rb256g100dpi.Checked = True Then
            cApplFunc.ScanMode = LsFamily.LsDefines.ScanMode.SCAN_MODE_256_GRAY_100
        ElseIf rb256g200dpi.Checked = True Then
            cApplFunc.ScanMode = LsFamily.LsDefines.ScanMode.SCAN_MODE_256_GRAY_200
        ElseIf rb256g300dpi.Checked = True Then
            cApplFunc.ScanMode = LsFamily.LsDefines.ScanMode.SCAN_MODE_256_GRAY_300
        ElseIf rbColor100dpi.Checked = True Then
            cApplFunc.ScanMode = LsFamily.LsDefines.ScanMode.SCAN_MODE_COLOR_100
        ElseIf rbColor200dpi.Checked = True Then
            cApplFunc.ScanMode = LsFamily.LsDefines.ScanMode.SCAN_MODE_COLOR_200
        ElseIf rbColor300dpi.Checked = True Then
            cApplFunc.ScanMode = LsFamily.LsDefines.ScanMode.SCAN_MODE_COLOR_300
        ElseIf rbUV100dpi.Checked = True Then
            cApplFunc.ScanMode = LsFamily.LsDefines.ScanMode.SCAN_MODE_256GR100_AND_UV
        ElseIf rbUV200dpi.Checked = True Then
            cApplFunc.ScanMode = LsFamily.LsDefines.ScanMode.SCAN_MODE_256GR200_AND_UV
        End If

        ' Side
        If rbSideAll.Checked = True Then
            cApplFunc.Side = LsFamily.LsDefines.Side.SIDE_ALL_IMAGE
        ElseIf rbSideFront.Checked = True Then
            cApplFunc.Side = LsFamily.LsDefines.Side.SIDE_FRONT_IMAGE
        ElseIf rbSideRear.Checked = True Then
            cApplFunc.Side = LsFamily.LsDefines.Side.SIDE_BACK_IMAGE
        ElseIf rbSideNone.Checked = True Then
            cApplFunc.Side = LsFamily.LsDefines.Side.SIDE_NONE_IMAGE
        End If

        ' Codeline
        If cbMICR.Checked = True Then
            cApplFunc.CodelineType = ApplClass.DECODE_MICR
            cApplFunc.Codeline_HW = LsFamily.LsDefines.CodelineToRead.READ_CODELINE_HW_MICR
            'Else
            'cApplFunc.Codeline_HW = LsFamily.LsDefines.CodelineToRead.READ_CODELINE_HW_NO
            'End If

        ElseIf cbE13bMicr.Checked = True Then
            cApplFunc.CodelineType = ApplClass.DECODE_E13B_MICR_AND_OCR
            cApplFunc.Codeline_HW = LsFamily.LsDefines.CodelineToRead.READ_CODELINE_E13B_MICR_WITH_OCR
        Else
            cApplFunc.Codeline_HW = LsFamily.LsDefines.CodelineToRead.READ_CODELINE_HW_NO
            cApplFunc.CodelineType = ApplClass.DECODE_NO ' se no rimaneva sempre MICR selezionato
        End If

        If cbOCRsw.Checked = True Then
            cApplFunc.CodelineType = ApplClass.DECODE_OCR
        ElseIf cbBarcode.Checked = True Then
            cApplFunc.CodelineType = ApplClass.DECODE_BARCODE
            'Else
            '   cApplFunc.CodelineType = ApplClass.DECODE_NO
        End If

        ' String to Print
        cApplFunc.PrintValidate = cbPrint.SelectedIndex
        cApplFunc.Endorse_str = tbPrintString.Text
        If (cbPrintHigh.Checked = True) Then
            cApplFunc.Print_High = True
        Else
            cApplFunc.Print_High = False
        End If

        Select Case cbPrint1.SelectedIndex
            Case ComboFont.COMBO_NO_PRINT
                cApplFunc.PrintValidate1 = ComboFont.COMBO_NO_PRINT
            Case ComboFont.COMBO_FONT_NORMAL
                cApplFunc.PrintValidate1 = LsFamily.LsDefines.PrintFont.PRINT_FORMAT_NORMAL
            Case formOptions.ComboFont.COMBO_FONT_BOLD
                cApplFunc.PrintValidate1 = LsFamily.LsDefines.PrintFont.PRINT_FORMAT_BOLD
            Case formOptions.ComboFont.COMBO_FONT_15_INCH
                cApplFunc.PrintValidate1 = LsFamily.LsDefines.PrintFont.PRINT_FORMAT_NORMAL_15
            Case formOptions.ComboFont.COMBO_FONT_DOUBLE_HIGH
                cApplFunc.PrintValidate1 = LsFamily.LsDefines.PrintFont.PRINT_FORMAT_DOUBLE_HIGH
        End Select

        Select Case cbPrint2.SelectedIndex
            Case ComboFont.COMBO_NO_PRINT
                cApplFunc.PrintValidate2 = ComboFont.COMBO_NO_PRINT
            Case ComboFont.COMBO_FONT_NORMAL
                cApplFunc.PrintValidate2 = LsFamily.LsDefines.PrintFont.PRINT_FORMAT_NORMAL
            Case formOptions.ComboFont.COMBO_FONT_BOLD
                cApplFunc.PrintValidate2 = LsFamily.LsDefines.PrintFont.PRINT_FORMAT_BOLD
            Case formOptions.ComboFont.COMBO_FONT_15_INCH
                cApplFunc.PrintValidate2 = LsFamily.LsDefines.PrintFont.PRINT_FORMAT_NORMAL_15
            Case formOptions.ComboFont.COMBO_FONT_DOUBLE_HIGH
                cApplFunc.PrintValidate2 = LsFamily.LsDefines.PrintFont.PRINT_FORMAT_DOUBLE_HIGH
        End Select
        cApplFunc.Endorse_str2 = tbPrintString2.Text

        If (cbPrint3.Enabled = True) Then
            Select Case cbPrint3.SelectedIndex
                Case ComboFont.COMBO_NO_PRINT
                    cApplFunc.PrintValidate3 = ComboFont.COMBO_NO_PRINT
                Case ComboFont.COMBO_FONT_NORMAL
                    cApplFunc.PrintValidate3 = LsFamily.LsDefines.PrintFont.PRINT_FORMAT_NORMAL
                Case formOptions.ComboFont.COMBO_FONT_BOLD
                    cApplFunc.PrintValidate3 = LsFamily.LsDefines.PrintFont.PRINT_FORMAT_BOLD
                Case formOptions.ComboFont.COMBO_FONT_15_INCH
                    cApplFunc.PrintValidate3 = LsFamily.LsDefines.PrintFont.PRINT_FORMAT_NORMAL_15
                Case formOptions.ComboFont.COMBO_FONT_DOUBLE_HIGH
                    cApplFunc.PrintValidate3 = LsFamily.LsDefines.PrintFont.PRINT_FORMAT_DOUBLE_HIGH
            End Select
            cApplFunc.Endorse_str3 = tbPrintString3.Text
        Else
            cApplFunc.PrintValidate3 = 0
        End If
        If (cbPrint4.Enabled = True) Then
            Select Case cbPrint4.SelectedIndex
                Case ComboFont.COMBO_NO_PRINT
                    cApplFunc.PrintValidate4 = ComboFont.COMBO_NO_PRINT
                Case ComboFont.COMBO_FONT_NORMAL
                    cApplFunc.PrintValidate4 = LsFamily.LsDefines.PrintFont.PRINT_FORMAT_NORMAL
                Case formOptions.ComboFont.COMBO_FONT_BOLD
                    cApplFunc.PrintValidate4 = LsFamily.LsDefines.PrintFont.PRINT_FORMAT_BOLD
                Case formOptions.ComboFont.COMBO_FONT_15_INCH
                    cApplFunc.PrintValidate4 = LsFamily.LsDefines.PrintFont.PRINT_FORMAT_NORMAL_15
                Case formOptions.ComboFont.COMBO_FONT_DOUBLE_HIGH
                    cApplFunc.PrintValidate4 = LsFamily.LsDefines.PrintFont.PRINT_FORMAT_DOUBLE_HIGH
            End Select
            cApplFunc.Endorse_str4 = tbPrintString4.Text
        Else
            cApplFunc.PrintValidate4 = 0
        End If

        ' Front Stamp
        If (cbFrontStamp.Checked = True) Then
            cApplFunc.FrontStamp = LsFamily.LsDefines.Stamp.STAMP_FRONT
        Else
            cApplFunc.FrontStamp = LsFamily.LsDefines.Stamp.STAMP_NO
        End If

        ' Beep on Error
        If (cbBeepOnError.Checked = True) Then
            cApplFunc.Beep = LsFamily.LsDefines.Beep.BEEP_YES
        Else
            cApplFunc.Beep = LsFamily.LsDefines.Beep.BEEP_NO
        End If

        ' Wait Documment
        If (cbWaitDoc.Checked = True) Then
            cApplFunc.WiatDoc = False
        Else
            cApplFunc.WiatDoc = True
        End If

        If (cbHightContrast.Checked = True) Then
            cApplFunc.HightContrast = True
        Else
            cApplFunc.HightContrast = False
        End If

        If (cbShowUVImage.Checked = True) Then
            cApplFunc.ShowUvImage = True
        Else
            cApplFunc.ShowUvImage = False
        End If

        'Save On File
        If rbFileNone.Checked = True Then

            cApplFunc.SaveMode = LsFamily.LsDefines.ImageSave.IMAGE_SAVE_HANDLE
        Else
            cApplFunc.SaveMode = LsFamily.LsDefines.ImageSave.IMAGE_SAVE_BOTH
            If rbFileBMP.Checked = True Then
                cApplFunc.FileFormat = LsFamily.LsDefines.FileType.FILE_BMP
            ElseIf rbFileJPEG.Checked = True Then
                cApplFunc.FileFormat = LsFamily.LsDefines.FileType.FILE_JPEG
            ElseIf rbTIFF.Checked = True Then
                cApplFunc.FileFormat = LsFamily.LsDefines.FileType.FILE_TIF
            ElseIf rbTiffCCITT.Checked = True Then
                cApplFunc.FileFormat = LsFamily.LsDefines.FileType.FILE_CCITT
            ElseIf rbTiffGr4.Checked = True Then
                cApplFunc.FileFormat = LsFamily.LsDefines.FileType.FILE_CCITT_GROUP4
            End If
        End If

        cApplFunc.Quality = tbJPEGq.Text

        ' Double Leafing Level
        cApplFunc.DuobleLeafingValue = tbDoubleLeafing.Value


        'Sorters
        If (LsUnitType = LsFamily.LsDefines.LsUnitType.LS_520_USB Or LsUnitType = LsFamily.LsDefines.LsUnitType.LS_515_USB) Then
            Select Case cbSorter.SelectedIndex
                Case 0
                    cApplFunc.Sorter_Ls520 = LsFamily.LsDefines.Sorter.SORTER_POCKET_1
                Case 1
                    cApplFunc.Sorter_Ls520 = LsFamily.LsDefines.Sorter.SORTER_POCKET_2
                Case 2
                    cApplFunc.Sorter_Ls520 = LsFamily.LsDefines.Sorter.SORTER_SWICTH_1_TO_2
                Case 3
                    cApplFunc.Sorter_Ls520 = LsFamily.LsDefines.Sorter.SORTER_ON_CODELINE_CALLBACK
                Case Else
                    cApplFunc.Sorter_Ls520 = LsFamily.LsDefines.Sorter.SORTER_POCKET_1
            End Select
        ElseIf (LsUnitType = LsFamily.LsDefines.LsUnitType.LS_800_USB) Then
            Select Case cbSorter.SelectedIndex
                Case 0
                    cApplFunc.Sorter_Ls800 = LsFamily.LsDefines.Sorter.SORTER_CIRCULAR
                Case 1
                    cApplFunc.Sorter_Ls800 = LsFamily.LsDefines.Sorter.SORTER_SEQUENTIAL
                Case 2
                    cApplFunc.Sorter_Ls800 = LsFamily.LsDefines.Sorter.SORTER_POCKET_0_SELECTED
                Case 3
                    cApplFunc.Sorter_Ls800 = LsFamily.LsDefines.Sorter.SORTER_POCKET_1_SELECTED
                Case 4
                    cApplFunc.Sorter_Ls800 = LsFamily.LsDefines.Sorter.SORTER_POCKET_2_SELECTED
                Case 5
                    cApplFunc.Sorter_Ls800 = LsFamily.LsDefines.Sorter.SORTER_POCKET_3_SELECTED
                Case 6
                    cApplFunc.Sorter_Ls800 = LsFamily.LsDefines.Sorter.SORTER_POCKET_4_SELECTED
                Case 7
                    cApplFunc.Sorter_Ls800 = LsFamily.LsDefines.Sorter.SORTER_POCKET_5_SELECTED
                Case 8
                    cApplFunc.Sorter_Ls800 = LsFamily.LsDefines.Sorter.SORTER_POCKET_6_SELECTED
                Case 9
                    cApplFunc.Sorter_Ls800 = LsFamily.LsDefines.Sorter.SORTER_POCKET_7_SELECTED
                Case 10
                    cApplFunc.Sorter_Ls800 = LsFamily.LsDefines.Sorter.SORTER_POCKET_8_SELECTED
                Case 11
                    cApplFunc.Sorter_Ls800 = LsFamily.LsDefines.Sorter.SORTER_POCKET_9_SELECTED
                Case 12
                    cApplFunc.Sorter_Ls800 = LsFamily.LsDefines.Sorter.SORTER_POCKET_10_SELECTED
                Case 13
                    cApplFunc.Sorter_Ls800 = LsFamily.LsDefines.Sorter.SORTER_POCKET_11_SELECTED
                Case 14
                    cApplFunc.Sorter_Ls800 = LsFamily.LsDefines.Sorter.SORTER_POCKET_12_SELECTED
                Case 15
                    cApplFunc.Sorter_Ls800 = LsFamily.LsDefines.Sorter.SORTER_POCKET_13_SELECTED
                Case 16
                    cApplFunc.Sorter_Ls800 = LsFamily.LsDefines.Sorter.SORTER_POCKET_14_SELECTED
                Case 17
                    cApplFunc.Sorter_Ls800 = LsFamily.LsDefines.Sorter.SORTER_POCKET_15_SELECTED
                Case Else
                    cApplFunc.Sorter_Ls800 = LsFamily.LsDefines.Sorter.SORTER_SEQUENTIAL
            End Select

        End If

        ' IP address
        cApplFunc.IP_Address = tbIPAddress.Text
        If (LsUnitType = LsFamily.LsDefines.LsUnitType.LS_100_ETH Or LsUnitType = LsFamily.LsDefines.LsUnitType.LS_150_ETH) Then
            Ls.LSSetIPAddress(LsUnitType, cApplFunc.IP_Address, 4000)
        End If

        cApplFunc.PWMvalue = tbPWMvalue.Text
        
        Me.Close()
    End Sub

    Private Sub bOptionCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bOptionCancel.Click
        Me.Close()
    End Sub

    Private Sub cbOCRsw_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbOCRsw.CheckedChanged
        If cbOCRsw.Checked = True Then
            bOCRxy.Enabled = True
            cbBarcode.Checked = False
        Else
            bOCRxy.Enabled = False
        End If
    End Sub

    Private Sub cbBarcode_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbBarcode.CheckedChanged
        If cbBarcode.Checked = True Then
            bBarcodexy.Enabled = True
            cbOCRsw.Checked = False
        Else
            bBarcodexy.Enabled = False
        End If
    End Sub

    Private Sub rbFileJPEG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbFileJPEG.CheckedChanged
        If rbFileJPEG.Checked = True Then
            tbJPEGq.Enabled = True
        Else
            tbJPEGq.Enabled = False
        End If
    End Sub

    Private Sub rbBWImage_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbBWImage.CheckedChanged

        If rbBWImage.Checked = True Then
            'bgColor = me.BackColor
            rbBWImage.BackColor = setColor
        Else
            rbBWImage.BackColor = bgColor
        End If
    End Sub

    Private Sub rb16g100dpi_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rb16g100dpi.CheckedChanged

        If rb16g100dpi.Checked = True Then
            'bgColor = me.BackColor
            rb16g100dpi.BackColor = setColor
        Else
            rb16g100dpi.BackColor = bgColor
        End If
    End Sub

    Private Sub rb16g200dpi_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rb16g200dpi.CheckedChanged

        If rb16g200dpi.Checked = True Then
            'bgColor = me.BackColor
            rb16g200dpi.BackColor = setColor
        Else
            rb16g200dpi.BackColor = bgColor
        End If
    End Sub

    Private Sub rb16g300dpi_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rb16g300dpi.CheckedChanged

        If rb16g300dpi.Checked = True Then
            'bgColor = me.BackColor
            rb16g300dpi.BackColor = setColor
        Else
            rb16g300dpi.BackColor = bgColor
        End If
    End Sub

    Private Sub rb256g100dpi_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rb256g100dpi.CheckedChanged

        If rb256g100dpi.Checked = True Then
            'bgColor = me.BackColor
            rb256g100dpi.BackColor = setColor
        Else
            rb256g100dpi.BackColor = bgColor
        End If
    End Sub

    Private Sub rb256g200dpi_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rb256g200dpi.CheckedChanged

        If rb256g200dpi.Checked = True Then
            'bgColor = me.BackColor
            rb256g200dpi.BackColor = setColor
        Else
            rb256g200dpi.BackColor = bgColor
        End If
    End Sub

    Private Sub rb256g300dpi_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rb256g300dpi.CheckedChanged

        If rb256g300dpi.Checked = True Then
            'bgColor = me.BackColor
            rb256g300dpi.BackColor = setColor
        Else
            rb256g300dpi.BackColor = bgColor
        End If
    End Sub

    Private Sub rbColor100dpi_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbColor100dpi.CheckedChanged

        If rbColor100dpi.Checked = True Then
            'bgColor = me.BackColor
            rbColor100dpi.BackColor = setColor
        Else
            rbColor100dpi.BackColor = bgColor
        End If
    End Sub

    Private Sub rbColor200dpi_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbColor200dpi.CheckedChanged

        If rbColor200dpi.Checked = True Then
            'bgColor = me.BackColor
            rbColor200dpi.BackColor = setColor
        Else
            rbColor200dpi.BackColor = bgColor
        End If
    End Sub

    Private Sub rbColor300dpi_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbColor300dpi.CheckedChanged

        If rbColor300dpi.Checked = True Then
            'bgColor = me.BackColor
            rbColor300dpi.BackColor = setColor
        Else
            rbColor300dpi.BackColor = bgColor
        End If
    End Sub

    'Private Sub bOCRxy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bOCRxy.Click
    '    Dim aa As formOcr = New formOcr

    '    aa.ShowDialog(Me)
    'End Sub

    'Private Sub bBarcodexy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bBarcodexy.Click
    '    Dim aa As formBarcode = New formBarcode

    '    aa.ShowDialog(Me)
    'End Sub

    Private Sub cbPrint1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbPrint1.SelectedIndexChanged

        If (cbPrint1.Visible = True) Then
            If (cbPrint1.SelectedIndex = ComboFont.COMBO_FONT_DOUBLE_HIGH) Then
                cbPrint4.SelectedIndex = 0
                cbPrint4.Enabled = False
                tbPrintString4.Enabled = False
                If (cbPrint2.SelectedIndex = ComboFont.COMBO_FONT_DOUBLE_HIGH Or cbPrint3.SelectedIndex = ComboFont.COMBO_FONT_DOUBLE_HIGH) Then
                    cbPrint3.SelectedIndex = 0
                    cbPrint3.Enabled = False
                    tbPrintString3.Enabled = False
                Else
                    cbPrint3.Enabled = True
                    tbPrintString3.Enabled = True
                End If
            ElseIf (cbPrint2.SelectedIndex = ComboFont.COMBO_FONT_DOUBLE_HIGH Or cbPrint3.SelectedIndex = ComboFont.COMBO_FONT_DOUBLE_HIGH) Then
                cbPrint3.Enabled = True
                tbPrintString3.Enabled = True
            Else
                cbPrint4.Enabled = True
                tbPrintString4.Enabled = True
            End If
        End If
    End Sub

    Private Sub cbPrint2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbPrint2.SelectedIndexChanged

        If (cbPrint2.Enabled = True) Then
            If (cbPrint2.SelectedIndex = ComboFont.COMBO_FONT_DOUBLE_HIGH) Then
                cbPrint4.SelectedIndex = 0
                cbPrint4.Enabled = False
                tbPrintString4.Enabled = False
                If (cbPrint1.SelectedIndex = ComboFont.COMBO_FONT_DOUBLE_HIGH Or cbPrint3.SelectedIndex = ComboFont.COMBO_FONT_DOUBLE_HIGH) Then
                    cbPrint3.SelectedIndex = 0
                    cbPrint3.Enabled = False
                    tbPrintString3.Enabled = False
                Else
                    cbPrint3.Enabled = True
                    tbPrintString3.Enabled = True
                End If
            ElseIf (cbPrint1.SelectedIndex = ComboFont.COMBO_FONT_DOUBLE_HIGH Or cbPrint3.SelectedIndex = ComboFont.COMBO_FONT_DOUBLE_HIGH) Then
                cbPrint3.Enabled = True
                tbPrintString3.Enabled = True
            Else
                cbPrint4.Enabled = True
                tbPrintString4.Enabled = True
            End If
        End If
    End Sub

    Private Sub cbPrint3_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbPrint3.SelectedIndexChanged

        If (cbPrint3.Enabled = True) Then
            If (cbPrint3.SelectedIndex = ComboFont.COMBO_FONT_DOUBLE_HIGH) Then
                cbPrint4.SelectedIndex = 0
                cbPrint4.Enabled = False
                tbPrintString4.Enabled = False
                If (cbPrint1.SelectedIndex = ComboFont.COMBO_FONT_DOUBLE_HIGH Or cbPrint2.SelectedIndex = ComboFont.COMBO_FONT_DOUBLE_HIGH) Then
                    cbPrint3.SelectedIndex = 0
                    cbPrint3.Enabled = False
                    tbPrintString3.Enabled = False
                Else
                    cbPrint3.Enabled = True
                    tbPrintString3.Enabled = True
                End If
            ElseIf (cbPrint1.SelectedIndex = ComboFont.COMBO_FONT_DOUBLE_HIGH Or cbPrint2.SelectedIndex = ComboFont.COMBO_FONT_DOUBLE_HIGH) Then
                cbPrint3.Enabled = True
                tbPrintString3.Enabled = True
            Else
                cbPrint4.Enabled = True
                tbPrintString4.Enabled = True
            End If
        End If
    End Sub



    Private Sub cbMICR_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbMICR.CheckedChanged
        If cbMICR.Checked = True Then
            cbE13bMicr.Checked = False
        End If
    End Sub

    Private Sub cbE13bMicr_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbE13bMicr.CheckedChanged
        If cbE13bMicr.Checked = True Then
            cbMICR.Checked = False
        End If
    End Sub



    Private Sub rbUV100dpi_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbUV100dpi.CheckedChanged
        If rbUV100dpi.Checked = True Then
            'bgColor = me.BackColor
            rbUV100dpi.BackColor = setColor
        Else
            rbUV100dpi.BackColor = bgColor
        End If


    End Sub

    Private Sub rbUV200dpi_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbUV200dpi.CheckedChanged
        If rbUV200dpi.Checked = True Then
            'bgColor = me.BackColor
            rbUV200dpi.BackColor = setColor
        Else
            rbUV200dpi.BackColor = bgColor
        End If
    End Sub


    Private Sub tbPWMvalue_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbPWMvalue.KeyPress
        If e.KeyChar <> Chr(8) Then
            If (e.KeyChar > Chr(57) Or e.KeyChar < Chr(48)) Then
                e.KeyChar = ChrW(0)
            End If

        End If

    End Sub

    
End Class
