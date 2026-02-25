Imports System
Imports System.Windows.Forms
Imports System.Drawing
Imports System.Runtime.InteropServices
Imports System.IO
Imports LsFamily
Imports LsFamily.LsApi
Imports System.Threading
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports BrClearing.Common.Modscan
Imports System.Configuration
Imports BRCTSScanner.pubReff.pubRef
Imports BrClearing.Common
Imports System.Globalization
Imports RANGERLib

Public Module GlobalModuleCTS
    Public BROutwardForm As frmBROutwardClearingCTS
End Module



Public Class frmBROutwardClearingCTS

    Public Structure ParDocHandle

        Public ScanCard As Boolean
        Public PrintHigh As Short
        Public FrontStamp As Short
        Public CodelineMICR As CtsLs.CodeLineType
        Public CodelineOCR As CtsLs.CodeLineType
        Public Unit_measure As Short
        Public Codeline_Sw_x As Single
        'ocr
        Public Codeline_Sw_y As Single
        'per ocr
        Public Codeline_Sw_w As Single
        'ocr
        Public Codeline_Sw_h As Single
        'ocr
        Public PrintValidate As CtsLs.PrintFont
        Public PrintBold As Boolean
        Public Endorse_str As String
        Public BarcodeType As CtsLs.CodeLineType
        Public Barcode_Sw_x As Single
        'Barcode
        Public Barcode_Sw_y As Single
        'Barcode
        Public Barcode_Sw_w As Single
        'Barcode
        Public Barcode_Sw_h As Single
        'Barcode
        ' short Sorter;
        Public ScanMode As Short
        Public Side As Byte
        ' byte BadgeTrack;
        Public SaveImage As Short
        Public FileFormat As Short
        Public Qual As Short
        Public ClearAlignImage As Short
        Public NumDoc As Short
        Public BeepOnError As Short
        Public WaitTimeout As Boolean
        ' short Sorter_PrintValidate;
        ' bool Sorter_PrintBold;
        ' string Sorter_Endorse_str[200];
        ' char Sorter_Side;
        ' short Sorter_Stamp;
        Public TypeOfDecod As Byte
        ' Selected decoding
        Public DL_Type As Int32
        Public DL_Value As Short
        Public DL_MinDoc As Int32
        Public DL_MaxDoc As Int32
        'float Pdf417_Sw_x;
        'float Pdf417_Sw_y;
        'float Pdf417_Sw_w;
        'float Pdf417_Sw_h;
        Public StampPosition As Short
        'char  Digital_str[200];
        'short Digital_SidePrint;
        'char  Digital_FontName[120];
        'short Digital_FontSize;
        'short Digital_Unit;
        'float Digital_x;
        'float Digital_y;
        'short Digital_Tone;
        'bool Digital_Bold;
        'bool Digital_Italic;
        'bool Digital_Undeline;
        Public LowSpeed As Short

        Public PercentPWM_UV As Short
        Public Contrast_UV As Boolean
        Public Threshold_UV As Short

        Public PrintHighDefinition As Boolean
        'Printer HD
        'bool	PrintLine1;
        'bool	PrintLine2;
        'bool	PrintLine3;
        'bool	PrintLine4;
        Public PrintLogo As Boolean
        'char	PrintFontLine1;
        'char	PrintStringLine1[128];
        'char	PrintFontLine2;
        'char	PrintStringLine2[128];
        'char	PrintFontLine3;
        'char	PrintStringLine3[128];
        'char	PrintFontLine4;
        'char	PrintStringLine4[128];

        'bool	Setup_DropIn;
        'char	IpAddress[20];
        'ushort  Net_Port;
        'char	IpBoxNodeName[40];
        'bool	fConnectByNodeName;

        Public LightIntensity As Short

        Public fTestPrinter As Boolean

        Public BWmethod As Short
        Public BWthreshold As Short

    End Structure

    Private CurrPocket As LsFamily.LsDefines.Sorter = LsFamily.LsDefines.Sorter.SORTER_POCKET_1
    Const TITLE_POPUP As String = "Bankers Realm Clearing"
    Const TITLE_ERROR As String = "Bankers Realm Clearing"
    Const MASK_SCANNER_UV As Byte = &H4

    Public Const DECODE_NO As Byte = &H0
    Public Const DECODE_MICR As Byte = &H1
    Public Const DECODE_OCR As Byte = &H2
    Public Const DECODE_BARCODE As Byte = &H4
    Public Const DECODE_PDF417 As Byte = &H8

    <DllImport("kernel32.dll", EntryPoint:="RtlMoveMemory")> _
    Private Shared Sub CopyMemory(ByVal Destination As IntPtr, ByVal Source As IntPtr, ByVal Length As UInteger)
    End Sub
    Dim LsDefines As LsFamily.LsDefines
    Dim CtsIQA As LsFamily.CtsIQA
    Dim eee As Integer
    Public hLS As Integer
    Dim t As Thread
    Public hEvent As ManualResetEvent
    Public Declare Function SetEvent Lib "kernel32" (ByVal hEvent As Integer) As Integer
    Public Declare Function CreateEvent Lib "kernel32" Alias "CreateEventA" (ByVal lpEventAttributes As Long, ByVal bManualReset As Long, ByVal bInitialState As Long, ByVal lpName As String) As Long
    Public Declare Function WaitForSingleObject Lib "kernel32.dll" (ByVal hHandle As Long, ByVal dwMilliseconds As Long) As Long
    Public Declare Function CloseHandle Lib "kernel32" Alias "CloseHandle" (ByVal hObject As Integer) As Integer
    Public Declare Function GetLastError Lib "kernel32" () As Integer
    Public hProva As IntPtr
    Dim WAIT_OBJECT_0 As Int32 = 0
    Public Const INFINITE = &HFFFF
    Public Const WAIT_INFINITE = -1&
    Public Const ALTEZZA_CODELINE = 33
    Dim NrDocVideo As Int16
    'Public Const TITLE_POPUP = "BRClearing"
    Private fProcessDoc As Boolean
    Private PathAppl As String
    Public Shared stParAppl As New ParDocHandle()
    Private Save_FrontImage As IntPtr
    Private Save_RearImage As IntPtr
    Dim fClearImage As Boolean
    'Dim dialogOptions As New OptionsDialog
    Dim RowID As Int32
    Private FrontImg As String = ""
    Private ImgPath As String = ""
    Private BackImg As String = ""
    Private UVImg As String = ""
    Private FrontBWImg As String = ""
    Private ImageMerge As Image = Nothing
    Private picker As New DateTimePicker
    Private StampCheque As Boolean = False
    'Public m_objApi As ApiUsage = Nothing
    Protected m_objConfig As Properties = Nothing
    Protected m_dataScanStartTime As Date = Nothing
    Protected m_iTransactionNumber As Integer = 0
    Public Ls As LsFamily.LsApi
    Public Sub buttonStartFeed_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonStartFeed.Click, Button3.Click
        Try
            Select Case ReaderUsed
                Case ReaderType.Panini
                    'chqCounter = Me.txtChqCount.Text
                    'chqCaunt = 0
                    'startReading()
                Case ReaderType.CTS
                    If isMDV = True Then
                        Me.txtChqCount.Text = 1
                        chqCounter = 1
                        chqCaunt = 0
                    Else
                        chqCounter = Me.txtChqCount.Text
                        chqCaunt = 0
                        If chqCounter = 0 Then
                            Exit Sub
                        End If
                    End If
                    '' ''MultiDocHandle(True)


                    Dim Reply As Integer
                    Dim hConnect As Short
                    Dim UnitCfg As Byte() = New Byte(7) {}
                    Dim UnitStatus As CtsLs.UNITSTATUS

                    UnitStatus = New CtsLs.UNITSTATUS()
                    UnitStatus.Size = Marshal.SizeOf(UnitStatus)

                    hConnect = 0
                    fProcessDoc = True

                    Reply = TryConnect(hConnect)
                    If Reply = CtsLs.LsReply.LS_OKAY Then
                        'EnableButton(False, True)


                        Reply = CtsLs.LSUnitIdentify(hConnect, 0, UnitCfg, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, _
                         IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, _
                         IntPtr.Zero, IntPtr.Zero, IntPtr.Zero)
                        'Reserved2);
                        'Do

                        Dim strIdentify As [String]

                        Reply = CtsLs.LSUnitStatus(hConnect, 0, UnitStatus)

                        '                if (Reply == CtsLs.LsReply.LS_OKAY)
                        If True Then
                            Select Case UnitStatus.UnitStatus
                                Case 0
                                    strIdentify = "ok"
                                    Exit Select
                                Case 2
                                    strIdentify = "Peripheral busy" & vbLf
                                    Exit Select
                                Case 3
                                    strIdentify = "Paper Jam" & vbLf
                                    Exit Select
                                Case 4
                                    strIdentify = "Hardware error" & vbLf
                                    Exit Select
                                Case 5
                                    strIdentify = "Illegal request" & vbLf
                                    Exit Select
                                Case 6
                                    strIdentify = "ok"
                                    Exit Select
                                Case 7
                                    strIdentify = "Error Double Leafing" & vbLf
                                    Exit Select
                                Case 9
                                    strIdentify = "Illegal Command" & vbLf
                                    Exit Select
                                Case 11
                                    strIdentify = "Aborted command" & vbLf
                                    Exit Select
                                Case 16
                                    strIdentify = "Calibration Aborted" & vbLf
                                    Exit Select
                                Case 64
                                    strIdentify = "Jam at MICR sensor" & vbLf
                                    Exit Select
                                Case 65
                                    strIdentify = "Jam or Doc to long" & vbLf
                                    Exit Select
                                Case 66
                                    strIdentify = "Jam between Scanners" & vbLf
                                    Exit Select
                                Case Else
                                    strIdentify = "Error " & UnitStatus.UnitStatus.ToString() & " not contempled !" & vbLf
                                    Exit Select
                            End Select
                            If strIdentify <> "ok" Then
                                MessageBox.Show(strIdentify, "Peripheral status", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            End If
                        End If

                        If TestDocPresent(hConnect, Reply) Then


                            If UnitStatus.Photo_Feeder = False Then
                                CheckReply(CtsLs.LsReply.LS_FEEDER_EMPTY, "BRClearing")
                                Exit Sub
                            ElseIf Reply = CtsLs.LsReply.LS_OKAY Then
                                Button3.Enabled = False
                                Button5.Enabled = False
                            End If
                            MultiDocHandle(True)
                            'If Convert.ToInt16(txtChqCount.Text) = 0 Then
                            '    buttonFreeTrack_Click(sender, e)
                            'End If
                        Else
                            If Reply <> CtsLs.LsReply.LS_OKAY AndAlso Reply <> CtsLs.LsReply.LS_FEEDER_EMPTY Then
                                ' Visualizzo l'errore ed esco !
                                CheckReply(Reply, "LSUnitStatus")
                                fProcessDoc = False
                            Else
                                System.Threading.Thread.Sleep(400)
                            End If
                        End If

                        ' Refresh the form

                        Application.DoEvents()
                        'Loop While fProcessDoc = True


                        If UnitStatus.Photo_Feeder = True And Convert.ToInt16(txtChqCount.Text) > 0 Then
                            Button3.Enabled = True
                            Button5.Enabled = False
                        Else
                            txtChqCount.Text = 0
                            Button3.Enabled = False
                            Button5.Enabled = True
                            Application.DoEvents()
                            Reply = CtsLs.LSDisconnect(hConnect, 0)
                            buttonFreeTrack_Click(sender, e)

                        End If


                    Else
                        CheckReply(Reply, "LSConnect")
                    End If

            End Select
            If Convert.ToInt16(txtChqCount.Text) = 0 Then
                ReaderCounter = 0
            End If


        Catch ex As Exception
            Button3.Enabled = True
            Button5.Enabled = True
            'myMVX.ListEvents(ex.Message.ToString & " - " & ex.TargetSite.ToString & " - " & ex.InnerException.ToString)
        End Try
    End Sub

    Enum LsModelIcon
        MenuIco_LS150usb
        'MenuIco_Ls150eth
    End Enum

    Enum DoubleLeafingIcon
        DL_Error
        DL_Warning
    End Enum

    Private Sub buttonOptions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonOptions.Click, Button7.Click
        'Try
        '    myMVX.ChangeParameters()
        'Catch ex As Exception
        '    myMVX.ListEvents(ex.Message.ToString & " - " & ex.TargetSite.ToString & " - " & ex.InnerException.ToString)
        'End Try

        'Try
        '    myMVX.InitParams()
        '    myMVX.Online()
        'Catch ex As Exception
        '    myMVX.ListEvents(ex.Message.ToString & " - " & ex.TargetSite.ToString & " - " & ex.InnerException.ToString)
        'End Try
    End Sub


    Private Sub buttonStopFeed_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonStopFeed.Click, Button4.Click
        Select Case ReaderUsed
            Case ReaderType.Panini
                'stopReading()
            Case ReaderType.CTS


        End Select
    End Sub
    Private Function TestDocPresent(ByVal hConnect As Short, ByRef Reply As Integer) As Boolean
        Dim ret As Boolean
        Dim UnitStatus As CtsLs.UNITSTATUS

        UnitStatus = New CtsLs.UNITSTATUS()
        UnitStatus.Size = Marshal.SizeOf(UnitStatus)

        ret = False
        Reply = CtsLs.LSUnitStatus(hConnect, 0, UnitStatus)
        If Reply = CtsLs.LsReply.LS_OKAY OrElse Reply = CtsLs.LsReply.LS_FEEDER_EMPTY Then
            If UnitStatus.Photo_Feeder Then
                ret = True
            End If

            'If (Marshal.PtrToStringAnsi(Form1.LsModel).Contains("LS150") = True) AndAlso stParAppl.ScanCard AndAlso UnitStatus.Photo_Scanners Then
            ret = True
            'End If
        End If

        Return ret
    End Function
    'Free any document stuck in the track.  This will run the motor as long as
    'the device is OnLine or ChangeParameters but will return a failure if
    'Feeding.
    Private Sub buttonFreeTrack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonFreeTrack.Click, Button5.Click
        Dim Reply As Integer
        Dim hConnect As Short

        Dim UnitCfg As Byte() = New Byte(3) {}
        Dim strLsModel As IntPtr = Marshal.AllocHGlobal(20)
        Dim strFwVersion As IntPtr = Marshal.AllocHGlobal(20)
        Dim Date_Fw As IntPtr = Marshal.AllocHGlobal(20)
        Dim strUnitID As IntPtr = Marshal.AllocHGlobal(20)
        Dim strInkJetVersion As IntPtr = Marshal.AllocHGlobal(20)
        Dim DecoderExpVersion As IntPtr = Marshal.AllocHGlobal(20)


        hConnect = 0


        Reply = TryConnect(hConnect)
        If Reply = CtsLs.LsReply.LS_OKAY OrElse Reply = CtsLs.LsReply.LS_ALREADY_OPEN OrElse Reply = CtsLs.LsReply.LS_TRY_TO_RESET Then
            Reply = CtsLs.LSReset(hConnect, 0, CShort(CtsLs.Reset.RESET_PATH))

            If Reply = CtsLs.LsReply.LS_OKAY Then
                'MessageBox.Show("Reset ok !", TITLE_POPUP)
            Else
                CheckReply(Reply, "LSReset")
            End If

            Reply = CtsLs.LSDisconnect(hConnect, 0)
        Else
            CheckReply(Reply, "LSConnect")
        End If


        ' Free of local variable
        Marshal.FreeHGlobal(strLsModel)
        Marshal.FreeHGlobal(strFwVersion)
        Marshal.FreeHGlobal(Date_Fw)
        Marshal.FreeHGlobal(strUnitID)
        Marshal.FreeHGlobal(strInkJetVersion)
        Marshal.FreeHGlobal(DecoderExpVersion)
    End Sub

    'Exit the application.
    Private Sub buttonExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonExit.Click, Button8.Click
        For i As Int32 = 0 To dgvOutCreditMicr.RowCount - 1
            If dgvOutCreditMicr.Rows(i).Cells(1).Value = 0 Then
                MessageBox.Show("Some Items have zero value which is not allowed.", TITLE_POPUP, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                dgvOutCreditMicr.Rows(i).Cells(1).Selected = True
                Exit Sub
            End If
        Next
        If OurBankID = "05" Then
            'Bank of India said through Trivendi that they don't want this on 17th Oct 2011 at 08.36pm (Monday)
            'Ben cs, Jonathan cs, Chege BOI and James are the witness- by Kamunya
        Else
            For i As Int32 = 0 To dgvOutCreditMicr.RowCount - 1
                If dgvOutCreditMicr.Rows(i).Cells(2).Value = "" Then
                    MessageBox.Show("Please provide the drawer's Name in row#" & i & ".", TITLE_POPUP, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    dgvOutCreditMicr.Rows(i).Cells(2).Selected = True
                    Exit Sub
                End If
            Next
        End If
        pictPreview1.Image = Nothing
        pictPreview1.Dispose()
        pictPreview2.Image = Nothing
        pictPreview2.Dispose()
        pictPreview3.Image = Nothing
        pictPreview3.Dispose()
        pictPreview4.Image = Nothing
        pictPreview4.Dispose()
        pictMainFront.Image = Nothing
        pictMainFront.Dispose()
        pictMainRear.Image = Nothing
        pictMainRear.Dispose()
        System.Windows.Forms.Application.DoEvents()

        'If we are coming back to this screen an data has not been saved the lets show it back to the grid
        If Modscan.SysType <> Modscan.ENUM_SysType.BRNET Then
            ExecuteData(GetModify("sp_ClearingFromScantoLive", "OurBranchID", OurBranchID, "OperatorID", OperatorID), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.SelectStatement)
        Else
            ExecuteData(GetModify("p_ClearingFromScantoLive", "OurBranchID", OurBranchID, "OperatorID", OperatorID), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.SelectStatement)
        End If
        If publicDTbl.Rows.Count = 0 Then
            SaveToDB(GetDataTableContent(dgvOutCreditMicr), "OUT")
        End If
        publicDTbl.Clear()
        Me.Dispose()
        GC.SuppressFinalize(Me)
    End Sub

    'When the form is closing, shutdown the device and stop the timer.  Perform any
    'other clean up steps necessary.
    Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            Dim hConnect As Short
            Dim Reply As Integer
            textState.BackColor = Color.Red
            hConnect = 0
            Reply = TryConnect(hConnect)
            If Reply = CtsLs.LsReply.LS_OKAY Then
                Reply = CtsLs.LSDisconnect(hConnect, 0)
            End If
            RecursiveDelete(ImagePath)
        Catch ex As Exception

        Finally
            GC.SuppressFinalize(Me)
        End Try
    End Sub

    'When the form loads, get a reference to the MVX API and startup the device.
    'The MVX API object is an application level object. So it can be referenced
    'from any number of forms and modules without having to reinitialize or copy
    'settings.  We also start our timer here.
    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'BRDbType = systemDbTypes.dbTypeSql
        GlobalModuleCTS.BROutwardForm = Me
        btnOk.Enabled = False
        btnOk.Visible = False
        Button3.Enabled = True
        Button5.Enabled = True
        'GetDbConnectionStrings()
        'MessageBox.Show("Loading sasa")
        Try
            If Modscan.SysType = Modscan.ENUM_SysType.BR Then
                ExecuteData(GetModify("SP_GetSystem", "ourBranchID", OurBranchID), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
                If publicDTbl.Rows.Count > 0 Then
                    CountryCode = ConfigurationManager.AppSettings("CountryCode")
                    StampCheque = ConfigurationManager.AppSettings("Stamp")
                    Select Case CountryCode.ToUpper.Trim
                        Case "UG"
                            txtClrCenter.Text = "47"
                            CodeLineDetails.CountryClearingCenter = "47"
                        Case "SL"
                            txtClrCenter.Text = "99"
                            CodeLineDetails.CountryClearingCenter = "99"
                        Case "TZ"
                            txtClrCenter.Text = "67"
                            CodeLineDetails.CountryClearingCenter = "67"
                            RBTypeB.Visible = True
                            RBTypeC.Visible = True
                        Case "RD"
                            txtClrCenter.Text = "99"
                            CodeLineDetails.CountryClearingCenter = "99"
                        Case "KE"
                            txtClrCenter.Text = "99"
                            CodeLineDetails.CountryClearingCenter = "99"
                        Case "ET"
                            txtClrCenter.Text = "99"
                            CodeLineDetails.CountryClearingCenter = "99"
                        Case "SA"
                            txtClrCenter.Text = "99"
                            CodeLineDetails.CountryClearingCenter = "99"
                    End Select
                    strImagePath = publicDTbl.Rows(0)("ChequeImagePath").ToString.Trim & "Images"
                End If
                publicDTbl.Clear()
                txtAccountID.Text = Modscan.strAccountID
                txtAccName.Text = Modscan.strAccountName
                txtAccountID.Enabled = False
                txtAccName.Enabled = False

                'GetDbConnectionStrings()
                'OpenConnections()

                'If we are coming back to this screen and there exists any data that has not been saved, lets show it back to the grid
                ExecuteData(GetModify("sp_ClearingFromScantoLive", "OurBranchID", OurBranchID, "OperatorID", OperatorID), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.SelectStatement)
                If publicDTbl.Rows.Count > 0 Then
                    For i As Integer = 0 To publicDTbl.Rows.Count - 1
                        Dim MCLine As String = publicDTbl.Rows(i)("ChequeID").ToString & publicDTbl.Rows(i)("BankID").ToString & publicDTbl.Rows(i)("BranchID").ToString & publicDTbl.Rows(i)("ChequeDigit").ToString & publicDTbl.Rows(i)("VoucherCode").ToString & publicDTbl.Rows(i)("TheirAccount").ToString
                        dgvOutCreditMicr.Rows.Add(publicDTbl.Rows(i)("ChequeID").ToString.Trim, publicDTbl.Rows(i)("Amount").ToString, Nothing, publicDTbl.Rows(i)("BankID").ToString, publicDTbl.Rows(i)("BranchID").ToString, publicDTbl.Rows(i)("TheirAccount").ToString, publicDTbl.Rows(i)("ChequeDate").ToString, publicDTbl.Rows(i)("ClearingDays").ToString, publicDTbl.Rows(i)("ReturnCode").ToString, IIf(IsDBNull(publicDTbl.Rows(i)("HighValue")), False, True), publicDTbl.Rows(i)("ChequeDigit").ToString, publicDTbl.Rows(i)("FrontImage"), publicDTbl.Rows(i)("BackImage"), publicDTbl.Rows(i)("UVImage"), publicDTbl.Rows(i)("ClearingCenterID").ToString, publicDTbl.Rows(i)("VoucherCode").ToString, 0, 0, publicDTbl.Rows(i)("ImageUniqueID").ToString, publicDTbl.Rows(i)("TFImageSize").ToString, publicDTbl.Rows(i)("JFImageSize").ToString, publicDTbl.Rows(i)("JRImageSize").ToString, publicDTbl.Rows(i)("AccountID").ToString, publicDTbl.Rows(i)("TFImage"), Nothing, Nothing, Nothing, Nothing, MCLine, publicDTbl.Rows(i)("TransactionColumnID"))
                    Next
                    GroupBox1.Enabled = False
                    txtChqCount.Text = publicDTbl.Rows.Count
                    txtChqCount.Enabled = False
                End If
                publicDTbl.Clear()
                If dgvOutCreditMicr.Rows.Count > 0 Then
                    txtBankID.Text = dgvOutCreditMicr.Rows(0).Cells(3).Value.ToString.Substring(0, 2)
                    txtBranchID.Text = dgvOutCreditMicr.Rows(0).Cells(4).Value.ToString.Substring(0, 3)
                    txtChqDigit.Text = dgvOutCreditMicr.Rows(0).Cells(16).Value
                    txtChqNo.Text = dgvOutCreditMicr.Rows(0).Cells(0).Value
                    txtTheirAccID.Text = dgvOutCreditMicr.Rows(0).Cells(5).Value
                    txtVoucherCode.Text = dgvOutCreditMicr.Rows(0).Cells(15).Value
                    If txtBankName.Text <> "" Then
                        txtBankName.Text = dgvOutCreditMicr.Rows(0).Cells(3).Value.ToString.Substring(3)
                    End If

                    If txtBankName.Text <> "" Then
                        txtBranchName.Text = dgvOutCreditMicr.Rows(0).Cells(4).Value.ToString.Substring(4)
                    End If
                    txtClrCenter.Text = dgvOutCreditMicr.Rows(0).Cells(14).Value
                    If ReaderUsed = ReaderType.Panini Then
                        pictMainFront.Image = GetImages(dgvOutCreditMicr.Rows(0).Cells(11).Value)
                        pictPreview1.Image = GetImages(dgvOutCreditMicr.Rows(0).Cells(11).Value)
                        pictPreview2.Image = GetImages(dgvOutCreditMicr.Rows(0).Cells(12).Value)
                        pictMainRear.Image = GetImages(dgvOutCreditMicr.Rows(0).Cells(12).Value)
                        pictPreview3.Image = GetImages(dgvOutCreditMicr.Rows(0).Cells(12).Value)
                        pictPreview4.Image = GetImages(dgvOutCreditMicr.Rows(0).Cells(12).Value)
                        BackGraySPic.Image = GetImages(dgvOutCreditMicr.Rows(0).Cells(12).Value)
                        FrontBWPic.Image = GetImages(dgvOutCreditMicr.Rows(0).Cells(11).Value)
                    Else

                        pictMainFront.Image = GetImages(dgvOutCreditMicr.Rows(0).Cells(11).Value)
                        pictPreview1.Image = GetImages(dgvOutCreditMicr.Rows(0).Cells(11).Value)
                        pictPreview2.Image = GetImages(dgvOutCreditMicr.Rows(0).Cells(13).Value)
                        pictMainRear.Image = GetImages(dgvOutCreditMicr.Rows(0).Cells(12).Value)
                        BackGraySPic.Image = GetImages(dgvOutCreditMicr.Rows(0).Cells(12).Value)
                        FrontBWPic.Image = GetImages(dgvOutCreditMicr.Rows(0).Cells(11).Value)
                    End If
                End If
            ElseIf Modscan.SysType = Modscan.ENUM_SysType.BRNET Then
                Try
                    CountryCode = ConfigurationManager.AppSettings("CountryCode")
                    Select Case CountryCode.ToUpper.Trim
                        Case "UG"
                            txtClrCenter.Text = "47"
                            CodeLineDetails.CountryClearingCenter = "47"
                        Case "SL"
                            txtClrCenter.Text = "99"
                            CodeLineDetails.CountryClearingCenter = "99"
                        Case "TZ"
                            txtClrCenter.Text = "67"
                            CodeLineDetails.CountryClearingCenter = "67"
                            RBTypeB.Visible = True
                            RBTypeC.Visible = True
                        Case "RD"
                            txtClrCenter.Text = "99"
                            CodeLineDetails.CountryClearingCenter = "99"
                        Case "KE"
                            txtClrCenter.Text = "99"
                            CodeLineDetails.CountryClearingCenter = "99"
                        Case "ET"
                            txtClrCenter.Text = "99"
                            CodeLineDetails.CountryClearingCenter = "99"
                        Case "SA"
                            txtClrCenter.Text = "99"
                            CodeLineDetails.CountryClearingCenter = "99"
                    End Select
                    'strImagePath = publicDTbl.Rows(0)("ChequeImagePath").ToString.Trim & "Images"
                    txtAccountID.Text = Modscan.strAccountID
                    txtAccName.Text = Modscan.strAccountName
                    txtAccountID.Enabled = False
                    txtAccName.Enabled = False
                    txtClrCenter.Enabled = False

                    ExecuteData(GetModify("p_ClearingFromScantoLive", "OurBranchID", OurBranchID, "OperatorID", OperatorID), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.SelectStatement)
                    If publicDTbl.Rows.Count > 0 Then
                        For i As Integer = 0 To publicDTbl.Rows.Count - 1
                            Dim MCLine As String = publicDTbl.Rows(i)("ChequeID").ToString & publicDTbl.Rows(i)("BankID").ToString & publicDTbl.Rows(i)("BranchID").ToString & publicDTbl.Rows(i)("ChequeDigit").ToString & publicDTbl.Rows(i)("VoucherCode").ToString & publicDTbl.Rows(i)("TheirAccount").ToString
                            dgvOutCreditMicr.Rows.Add(publicDTbl.Rows(i)("ChequeID").ToString.Trim, publicDTbl.Rows(i)("Amount").ToString, Nothing, publicDTbl.Rows(i)("BankID").ToString, publicDTbl.Rows(i)("BranchID").ToString, publicDTbl.Rows(i)("TheirAccount").ToString, publicDTbl.Rows(i)("ChequeDate").ToString, publicDTbl.Rows(i)("ClearingDays").ToString, publicDTbl.Rows(i)("ReturnCode").ToString, IIf(IsDBNull(publicDTbl.Rows(i)("HighValue")), False, True), publicDTbl.Rows(i)("ChequeDigit").ToString, publicDTbl.Rows(i)("FrontImage"), publicDTbl.Rows(i)("BackImage"), publicDTbl.Rows(i)("UVImage"), publicDTbl.Rows(i)("ClearingCenterID").ToString, publicDTbl.Rows(i)("VoucherCode").ToString, 0, 0, publicDTbl.Rows(i)("ImageUniqueID").ToString, publicDTbl.Rows(i)("TFImageSize").ToString, publicDTbl.Rows(i)("JFImageSize").ToString, publicDTbl.Rows(i)("JRImageSize").ToString, publicDTbl.Rows(i)("AccountID").ToString, publicDTbl.Rows(i)("TFImage"), Nothing, Nothing, Nothing, Nothing, MCLine, publicDTbl.Rows(i)("TransactionColumnID"))
                        Next
                        GroupBox1.Enabled = False
                        txtChqCount.Text = publicDTbl.Rows.Count
                        txtChqCount.Enabled = False
                    End If
                    publicDTbl.Clear()
                    If dgvOutCreditMicr.Rows.Count > 0 Then
                        txtBankID.Text = dgvOutCreditMicr.Rows(0).Cells(3).Value.ToString.Substring(0, 2)
                        txtBranchID.Text = dgvOutCreditMicr.Rows(0).Cells(4).Value.ToString.Substring(0, 3)
                        txtChqDigit.Text = dgvOutCreditMicr.Rows(0).Cells(16).Value
                        txtChqNo.Text = dgvOutCreditMicr.Rows(0).Cells(0).Value
                        txtTheirAccID.Text = dgvOutCreditMicr.Rows(0).Cells(5).Value
                        txtVoucherCode.Text = dgvOutCreditMicr.Rows(0).Cells(15).Value
                        If txtBankName.Text <> "" Then
                            txtBankName.Text = dgvOutCreditMicr.Rows(0).Cells(3).Value.ToString.Substring(3)
                        End If

                        If txtBankName.Text <> "" Then
                            txtBranchName.Text = dgvOutCreditMicr.Rows(0).Cells(4).Value.ToString.Substring(4)
                        End If
                        txtClrCenter.Text = dgvOutCreditMicr.Rows(0).Cells(14).Value
                        pictMainFront.Image = GetImages(String2Bytes(dgvOutCreditMicr.Rows(0).Cells(11).Value))
                        pictPreview1.Image = GetImages(String2Bytes(dgvOutCreditMicr.Rows(0).Cells(11).Value))
                        pictMainRear.Image = GetImages(String2Bytes(dgvOutCreditMicr.Rows(0).Cells(12).Value))
                        BackGraySPic.Image = GetImages(String2Bytes(dgvOutCreditMicr.Rows(0).Cells(12).Value))
                        FrontBWPic.Image = GetImages(String2Bytes(dgvOutCreditMicr.Rows(0).Cells("TFImage").Value))
                    End If
                Catch ex As Exception
                    MessageBox.Show("imechapa " + ex.Message)
                End Try
            End If
            'MessageBox.Show("Imefika hapa " + ReaderUsed)

            Select Case ReaderUsed
                Case ReaderType.Panini
                    'textState.Text = "Offline"
                    'timerState.Start()
                    ''myMVX.InitActVisionX(Me)
                    ''myMVX.Startup()
                    'EnableDisableCtl(2)
                Case ReaderType.CTS
                    'textState.Text = "Offline"
                    'timerState.Start()
                    CTSInitialization()
                    LsUnitType = LsFamily.LsDefines.LsUnitType.LS_150_USB
                    EnableDisableCtl(1)
                Case ReaderType.Epson
                    ' InitializeComponent()
                    'm_objConfig = New Properties
                    'm_objApi = New ApiUsage
                    'm_objApi.Configure(m_objConfig)
                    'm_objApi.MainFormItem(frmBROutwardClearingEpson)
                    'textState.Text = "Epson Online"
                    'textState.BackColor = Color.Green
            End Select

            lblAmount.Text = "0.00"
            lblCount.Text = "0"
            txtChqCount.Text = 0
            txtAccountID.Enabled = False
            txtAccName.Enabled = False
            Me.txtChqCount.Focus()
        Catch ex As Exception
            'ListEvents(ex.Message.ToString & " - " & ex.TargetSite.ToString & " - " & ex.InnerException.ToString)
        End Try

    End Sub

    'This public procedure is used my our MVX class to display the images we are
    'writing to file.  We use a delegate in the MVX class to contain this reference.
    Public Sub DisplayImages(ByVal strFilename As String, ByVal location As Integer)
        Try
            'Select Case location
            '    Case 1 'pictMainFront & pictPreview1
            '        pictPreview1.ImageLocation = strFilename
            '    Case 2
            '        pictPreview2.ImageLocation = strFilename
            '        pictMainFront.ImageLocation = strFilename
            '    Case 3
            '        pictPreview3.ImageLocation = strFilename
            '    Case 4
            '        pictPreview4.ImageLocation = strFilename
            '        pictMainRear.ImageLocation = strFilename
            'End Select
        Catch e As Exception

        End Try
    End Sub

    'The next four procedures are used to display the images from the "thumbnail"
    'panes in the main images panes.
    Private Sub pictPreview1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pictPreview1.Click
        Try
            pictMainFront.Image = pictPreview1.Image
            pictMainFront.StretchImageToFit = True
        Catch ex As Exception

        End Try
    End Sub

    Private Sub pictPreview2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pictPreview2.Click
        Try
            pictMainFront.Image = pictPreview2.Image
            pictMainFront.StretchImageToFit = True
        Catch ex As Exception

        End Try
    End Sub

    Private Sub pictPreview3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pictPreview3.Click
        pictMainFront.Image = pictPreview3.Image
        pictMainFront.StretchImageToFit = True
    End Sub


    Private Sub pictPreview4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pictPreview4.Click
        pictMainFront.Image = pictPreview4.Image
        pictMainFront.StretchImageToFit = True
    End Sub

    'The timer function is used to display the state of the device in the textbox
    'on the main form.  This state is used by this proc to determine which
    'buttons we want to have enabled/disabled at any given tick (3 seconds).
    Private Sub timerState_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles timerState.Tick
        Select Case ReaderUsed
            Case 1 'Panini
                'textState.Text = myMVX.ListDeviceState()
                Dim state As String = textState.Text.ToLower()
                If state.CompareTo("offline") = 0 Then
                    buttonStartFeed.Enabled = False
                    buttonStopFeed.Enabled = False
                    buttonFreeTrack.Enabled = False
                    buttonOptions.Enabled = False
                    buttonExit.Enabled = True
                    textState.BackColor = Color.Red
                ElseIf state.CompareTo("feeding") = 0 Then
                    buttonStartFeed.Enabled = False
                    buttonStopFeed.Enabled = True
                    buttonFreeTrack.Enabled = False
                    buttonOptions.Enabled = False
                    If btnOk.Visible = True Then
                        EnableDisableCtl(4)
                    Else
                        EnableDisableCtl(2)
                    End If
                    If Val(dgvOutCreditMicr.RowCount) + Val(dgvRejectedItem.RowCount) > 0 Then
                        If Val(dgvOutCreditMicr.RowCount) + Val(dgvRejectedItem.RowCount) >= Val(txtChqCount.Text) Then
                            buttonExit.Enabled = True
                            txtChqCount.Enabled = False
                            'myMVX.StopScan()
                        End If
                    End If
                ElseIf state.CompareTo("online") = 0 Then
                    textState.BackColor = Color.Green
                    textState.Text = "Online"
                    Application.DoEvents()
                    buttonStartFeed.Enabled = True
                    buttonFreeTrack.Enabled = True
                    buttonOptions.Enabled = False
                    If btnOk.Visible = True Then
                        buttonStartFeed.Enabled = False
                        EnableDisableCtl(4)
                    End If
                    If Val(dgvOutCreditMicr.RowCount) + Val(dgvRejectedItem.RowCount) > 0 Then
                        If Val(dgvOutCreditMicr.RowCount) + Val(dgvRejectedItem.RowCount) >= Val(txtChqCount.Text) Then
                            buttonExit.Enabled = True
                            Me.stopReading()
                        Else
                            'myMVX.StartScan()
                        End If
                    End If
                End If
            Case 2 'CTS
                'textState.Text = myMVX.ListDeviceState()
                'Dim state As String = textState.Text.ToLower()
                'If state.CompareTo("offline") = 0 Then
                '    buttonStartFeed.Enabled = False
                '    buttonStopFeed.Enabled = False
                '    buttonFreeTrack.Enabled = False
                '    buttonOptions.Enabled = False
                '    buttonExit.Enabled = True
                '    textState.BackColor = Color.Red
                'ElseIf state.CompareTo("feeding") = 0 Then
                '    buttonStartFeed.Enabled = False
                '    buttonStopFeed.Enabled = True
                '    buttonFreeTrack.Enabled = False
                '    buttonOptions.Enabled = False
                '    If btnOk.Visible = True Then
                '        EnableDisableCtl(4)
                '    Else
                '        EnableDisableCtl(2)
                '    End If
                '    If Val(dgvOutCreditMicr.RowCount) + Val(dgvRejectedItem.RowCount) > 0 Then
                '        If Val(dgvOutCreditMicr.RowCount) + Val(dgvRejectedItem.RowCount) >= Val(txtChqCount.Text) Then
                '            buttonExit.Enabled = True
                '            txtChqCount.Enabled = False
                '            myMVX.StopScan()
                '        End If
                '    End If
                'ElseIf state.CompareTo("online") = 0 Then
                '    textState.BackColor = Color.Green
                '    buttonStartFeed.Enabled = True
                '    buttonFreeTrack.Enabled = True
                '    buttonOptions.Enabled = False
                '    If btnOk.Visible = True Then
                '        buttonStartFeed.Enabled = False
                '        EnableDisableCtl(4)
                '    End If
                '    If Val(dgvOutCreditMicr.RowCount) + Val(dgvRejectedItem.RowCount) > 0 Then
                '        If Val(dgvOutCreditMicr.RowCount) + Val(dgvRejectedItem.RowCount) >= Val(txtChqCount.Text) Then
                '            buttonExit.Enabled = True
                '            Me.stopReading()
                '        Else
                '            myMVX.StartScan()
                '        End If
                '    End If
                'End If
        End Select
        Dim TCount As Int32 = Val(GlobalModuleCTS.BROutwardForm.dgvOutCreditMicr.RowCount) + Val(GlobalModuleCTS.BROutwardForm.dgvRejectedItem.RowCount)
        If TCount = Val(GlobalModuleCTS.BROutwardForm.txtChqCount.Text) Then
            GlobalModuleCTS.BROutwardForm.buttonStartFeed.Enabled = False
        End If
    End Sub

    Public Function PopulateMicrInfo()
        Try
            txtTheirAccID.Text = CodeLineDetails.TheirAccountID
            txtChqNo.Text = CodeLineDetails.ChequeID
            txtBankID.Text = CodeLineDetails.BankID
            txtBranchID.Text = CodeLineDetails.BranchID
            txtChqDigit.Text = CodeLineDetails.ChequeDigit
            txtVoucherCode.Text = CodeLineDetails.VoucherCode
            txtBankName.Text = CodeLineDetails.BankName
            txtBranchName.Text = CodeLineDetails.BranchName

            If MICRFrontImgPath = "" Then
                MICRFrontImgPath = CodeLineDetails.FrontImagePathGrayScale
            End If

            If MICRBackImgPath = "" Then
                MICRBackImgPath = CodeLineDetails.BackImagePath
            End If

            If MICRUVImagePath = "" Then
                MICRUVImagePath = CodeLineDetails.UVImagePath
            End If

            pictMainFront.Image = GetImages(String2Bytes(CodeLineDetails.FrontImageGrayScale))
            pictPreview1.Image = GetImages(String2Bytes(CodeLineDetails.FrontImageGrayScale))
            FrontBWPic.Image = GetImages(String2Bytes(CodeLineDetails.FrontImageBW))
            pictMainRear.Image = GetImages(String2Bytes(CodeLineDetails.BackImageGrayScale))
            BackGraySPic.Image = GetImages(String2Bytes(CodeLineDetails.BackImageGrayScale))
            pictPreview2.Image = GetImages(String2Bytes(CodeLineDetails.UVimage))

            pictMainFront.StretchImageToFit = True
            pictMainRear.StretchImageToFit = True

            AddDataToGrid()
            'If chqCaunt <> txtChqCount.Text Then
            '    startReading()
            'End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Public Sub stopReading()
        Try
            buttonOptions.Enabled = True
            buttonStartFeed.Enabled = True
            buttonExit.Enabled = True
            buttonFreeTrack.Enabled = True
            chqCounter = 0
            ReaderCounter = 0
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Public Sub startReading()
        chqCounter = txtChqCount.Text
        'myMVX.StartScan()
        buttonOptions.Enabled = False
        buttonStartFeed.Enabled = False
        buttonExit.Enabled = False
        buttonFreeTrack.Enabled = False

    End Sub

    Private Sub EnableDisableCtl(ByVal commType As Int32)
        Select Case commType
            Case 0 'Disable All
                txtAccName.Enabled = False
                txtAccountID.Enabled = False
                txtBankID.Enabled = False
                txtBankName.Enabled = False
                txtChqCount.Enabled = False
                txtChqDigit.Enabled = False
                txtChqNo.Enabled = False
                txtClrCenter.Enabled = False
                txtTheirAccID.Enabled = False
                txtVoucherCode.Enabled = False
                buttonStartFeed.Enabled = False
                buttonFreeTrack.Enabled = False
                buttonStopFeed.Enabled = False
                txtBranchID.Enabled = False
                txtBranchName.Enabled = False


            Case 1 'Enable All
                txtAccName.Enabled = True
                txtAccountID.Enabled = True
                txtBankID.Enabled = True
                txtBankName.Enabled = True
                txtChqCount.Enabled = True
                txtChqDigit.Enabled = True
                txtChqNo.Enabled = True
                txtClrCenter.Enabled = True
                txtTheirAccID.Enabled = True
                txtVoucherCode.Enabled = True
                buttonStartFeed.Enabled = True
                buttonFreeTrack.Enabled = True
                buttonStopFeed.Enabled = True
                txtBranchID.Enabled = True
                txtBranchName.Enabled = True

            Case 2 'Disable for Read Command
                txtAccName.Enabled = False
                txtAccountID.Enabled = False
                txtBankID.Enabled = False
                txtBankName.Enabled = False
                txtChqCount.Enabled = False
                txtChqDigit.Enabled = False
                txtChqNo.Enabled = False
                txtClrCenter.Enabled = False
                txtTheirAccID.Enabled = False
                txtVoucherCode.Enabled = False
                buttonStartFeed.Enabled = False
                buttonFreeTrack.Enabled = False
                buttonStopFeed.Enabled = False
                buttonOptions.Enabled = False
                txtBranchID.Enabled = False
                txtBranchName.Enabled = False
                txtChqCount.Enabled = True
                txtChqCount.Focus()

            Case 3 'Enable for Read Command
                txtAccName.Enabled = False
                txtAccountID.Enabled = False
                txtBankID.Enabled = False
                txtBankName.Enabled = False
                txtChqCount.Enabled = True
                txtChqDigit.Enabled = False
                txtChqNo.Enabled = False
                txtClrCenter.Enabled = False
                txtTheirAccID.Enabled = False
                txtVoucherCode.Enabled = False
                buttonStartFeed.Enabled = False
                buttonFreeTrack.Enabled = True
                buttonStopFeed.Enabled = True
                buttonOptions.Enabled = False
                txtBranchID.Enabled = False
                txtBranchName.Enabled = False
                txtChqCount.Enabled = True
                txtChqCount.Focus()

            Case 4 'Enable for editing
                txtAccName.Enabled = False
                txtAccountID.Enabled = False
                txtBankID.Enabled = True
                txtBankName.Enabled = False
                txtChqCount.Enabled = False
                txtChqDigit.Enabled = True
                txtChqNo.Enabled = True
                txtClrCenter.Enabled = False
                txtTheirAccID.Enabled = True
                txtVoucherCode.Enabled = True
                buttonStartFeed.Enabled = False
                buttonFreeTrack.Enabled = False
                buttonStopFeed.Enabled = False
                txtBranchID.Enabled = True
                txtBranchName.Enabled = False
                buttonStartFeed.Enabled = False
        End Select

    End Sub
    Public Sub AddDataToGrid()
        Try
            Select Case RejectedReason
                Case "ok", ""
                    Dim CurrDate As Date = Nothing
                    Dim StrCurrDate As String = ""
                    Dim StrValueDate As String = ""
                    If cWorkingDate = Nothing Then
                        If cFromDate = Nothing Then
                            If cToDate = Nothing Then
                                CurrDate = Today.Date
                            Else
                                CurrDate = cToDate
                            End If
                        Else
                            CurrDate = cFromDate
                        End If
                    Else
                        CurrDate = cWorkingDate
                    End If
                    StrCurrDate = CurrDate.ToString("dd/MMM/yyyy")
                    StrValueDate = CodeLineDetails.ValueDate.ToString("dd/MMM/yyyy")
                    'CodeLineDetails.ClearingDays = "4"
                    If isMDV = True Then
                        If mdvReturnCode <> "" Then
                            CodeLineDetails.ReturnCode = mdvReturnCode
                        Else
                            CodeLineDetails.ReturnCode = "00"
                        End If
                    End If
                    Dim Codilain As String = CodeLineDetails.ChequeID & CodeLineDetails.BankID & CodeLineDetails.BranchID & CodeLineDetails.ChequeDigit & CodeLineDetails.VoucherCode & CodeLineDetails.TheirAccountID
                    For i As Integer = 0 To dgvOutCreditMicr.RowCount - 1
                        If String.Compare(Codilain, dgvOutCreditMicr.Rows(i).Cells("colmicrline").Value) = 0 Then
                            MessageBox.Show("Item already captured. Item will be ignored", "BrClearing", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Exit Sub
                        Else
                            ExecuteData(GetModify("sp_GetIfChequeIsPosted", "BankID", CodeLineDetails.BankID, "Branchid", CodeLineDetails.BranchID, "TheirAccountID", CodeLineDetails.TheirAccountID, "ChequeID", CodeLineDetails.ChequeID), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
                            If publicDTbl.Rows.Count > 0 Then
                                If publicDTbl.Rows(0)(0).ToString = "True" Then
                                    MessageBox.Show("Item already captured. Item will be ignored", "BrClearing", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                    publicDTbl.Clear()
                                    Exit Sub
                                End If
                            End If
                        End If
                    Next
                    'If CodeLineDetails.ReturnCode = "" Then
                    '    CodeLineDetails.ReturnCode = "00"
                    'End If
                    dgvOutCreditMicr.Rows.Add(CodeLineDetails.ChequeID,
                                              CodeLineDetails.Amount, Nothing, StrCurrDate,
                                              CodeLineDetails.BankID & "-" & CodeLineDetails.BankName,
                                              CodeLineDetails.BranchID & "-" & CodeLineDetails.BranchName, CodeLineDetails.TheirAccountID,
                                              CodeLineDetails.ClearingDays, CodeLineDetails.ReturnCode,
                                              False, CodeLineDetails.ChequeDigit, CodeLineDetails.FrontImageGrayScale,
                                              CodeLineDetails.BackImageGrayScale, CodeLineDetails.UVimage,
                                              CodeLineDetails.CountryClearingCenter, CodeLineDetails.VoucherCode,
                                              CodeLineDetails.OurCommission, CodeLineDetails.TheirCommission,
                                              CodeLineDetails.UniqueNumber, CodeLineDetails.TFSize,
                                              CodeLineDetails.JFSize, CodeLineDetails.JRSize, txtAccountID.Text,
                                              CodeLineDetails.FrontImageBW, CodeLineDetails.FrontImagePathBW,
                                              CodeLineDetails.FrontImagePathGrayScale, CodeLineDetails.BackImagePath,
                                              CodeLineDetails.UVImagePath, Codilain, Nothing, "0.00", "0.00", "0.00",
                                              CodeLineDetails.IsUpCountry, CodeLineDetails.MinCommissionRate,
                                              CodeLineDetails.CommissionRate, CodeLineDetails.OurCommissionRate,
                                              CodeLineDetails.CurrencyCode, CodeLineDetails.FrontImageBlackandWhiteSignature,
                                              CodeLineDetails.FrontImageGrayScaleSignature,
                                              CodeLineDetails.BackImageSignature, CodeLineDetails.BranchName,
                                              CodeLineDetails.BankName, StrValueDate, CodeLineDetails.JRdpi,
                                              CodeLineDetails.FTdpi, CodeLineDetails.JFdpi)

                    'dgvOutCreditMicr.Rows.Add(CodeLineDetails.ChequeID, CodeLineDetails.Amount, Nothing)
                    FormatGrids(dgvOutCreditMicr)
                    dgvOutCreditMicr.Columns(6).ValueType = GetType(DateTime)
                    dgvOutCreditMicr.Columns(0).ReadOnly = True
                    dgvOutCreditMicr.Columns(7).ReadOnly = True
                    dgvOutCreditMicr.Columns(3).ReadOnly = True
                    dgvOutCreditMicr.Columns(4).ReadOnly = True
                    dgvOutCreditMicr.Columns(5).ReadOnly = True
                    dgvOutCreditMicr.Columns(10).ReadOnly = True
                    ClearTheHolders()
                    Me.txtChqCount.Text = Val(chqCounter) - Val(dgvOutCreditMicr.RowCount)
                Case Else
                    dgvRejectedItem.Rows.Add(CodeLineDetails.ChequeID, CodeLineDetails.BankID & "-" & CodeLineDetails.BankName, CodeLineDetails.BranchID & "-" & CodeLineDetails.BranchName, RejectedReason, CodeLineDetails.VoucherCode, CodeLineDetails.ChequeDigit, CodeLineDetails.TheirAccountID, CodeLineDetails.FrontImageGrayScale, CodeLineDetails.BackImageGrayScale, CodeLineDetails.UVimage, CodeLineDetails.UniqueNumber, CodeLineDetails.FrontImageBW, CodeLineDetails.BackImagePath, CodeLineDetails.FrontImagePathBW, CodeLineDetails.FrontImagePathGrayScale, CodeLineDetails.UVImagePath)
                    ClearTheHolders()
            End Select
            RejectedReason = ""
            isMDV = False

        Catch ex As Exception
            MsgBox(ex.Message)
            isMDV = False
        End Try
        EnableDisableCtl(0)
        txtBankID.Enabled = False
        txtBankName.Enabled = False
    End Sub

    Private Sub dgvOutCreditMicr_CellBeginEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles dgvOutCreditMicr.CellBeginEdit
        Try

            If (Me.dgvOutCreditMicr.Focused AndAlso (Me.dgvOutCreditMicr.CurrentCell.ColumnIndex = 6)) Then
                picker.Location = Me.dgvOutCreditMicr.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Location
                picker.Visible = True
                If (Not (Me.dgvOutCreditMicr.CurrentCell.Value Is DBNull.Value)) Then
                    picker.Value = CType(Me.dgvOutCreditMicr.CurrentCell.Value, DateTime)
                Else
                    picker.Value = DateTime.Now
                End If
            Else
                picker.Visible = False
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub dgvOutCreditMicr_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvOutCreditMicr.CellContentClick
        'btnDelete.Visible = True

    End Sub

    Private Sub dgvOutCreditMicr_CellContentDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvOutCreditMicr.CellContentDoubleClick
        txtChqCount.Text = Val(dgvOutCreditMicr.RowCount) + Val(dgvRejectedItem.RowCount)
        txtChqCount.Enabled = False
        Dim accID As String = txtAccountID.Text
        Dim AccName As String = txtAccName.Text

        ClearCTl()
        EnableDisableCtl(4)
        txtBankName.Text = ""
        txtBranchName.Text = ""
        txtBankID.Text = dgvOutCreditMicr.Rows(e.RowIndex).Cells(3).Value.ToString.Substring(0, 2)
        txtBranchID.Text = dgvOutCreditMicr.Rows(e.RowIndex).Cells(4).Value.ToString.Substring(0, 3)
        txtChqDigit.Text = dgvOutCreditMicr.Rows(e.RowIndex).Cells(10).Value
        txtChqNo.Text = dgvOutCreditMicr.Rows(e.RowIndex).Cells(0).Value
        txtTheirAccID.Text = dgvOutCreditMicr.Rows(e.RowIndex).Cells(5).Value
        txtVoucherCode.Text = dgvOutCreditMicr.Rows(e.RowIndex).Cells(15).Value
        If txtBankName.Text = "" Then
            Try
                txtBankName.Text = dgvOutCreditMicr.Rows(e.RowIndex).Cells(3).Value.ToString.Substring(3)
            Catch ex As Exception
                txtBankName.Text = dgvOutCreditMicr.Rows(e.RowIndex).Cells(3).Value
            End Try

        End If

        If txtBranchName.Text = "" Then
            Try
                txtBranchName.Text = dgvOutCreditMicr.Rows(e.RowIndex).Cells(4).Value.ToString.Substring(4)
            Catch ex As Exception
                txtBranchName.Text = dgvOutCreditMicr.Rows(e.RowIndex).Cells(4).Value
            End Try
        End If
        txtClrCenter.Text = dgvOutCreditMicr.Rows(e.RowIndex).Cells(14).Value


        pictMainFront.Image = GetImages(String2Bytes(dgvOutCreditMicr.Rows(e.RowIndex).Cells(11).Value))
        pictPreview1.Image = GetImages(String2Bytes(dgvOutCreditMicr.Rows(e.RowIndex).Cells(11).Value))
        FrontBWPic.Image = GetImages(String2Bytes(dgvOutCreditMicr.Rows(e.RowIndex).Cells("TFImage").Value))
        pictMainRear.Image = GetImages(String2Bytes(dgvOutCreditMicr.Rows(e.RowIndex).Cells(12).Value))
        BackGraySPic.Image = GetImages(String2Bytes(dgvOutCreditMicr.Rows(e.RowIndex).Cells(12).Value))
        pictPreview2.Image = GetImages(String2Bytes(dgvOutCreditMicr.Rows(e.RowIndex).Cells("colUVIMage").Value))

        Windows.Forms.Application.DoEvents()
        pictMainFront.StretchImageToFit = True
        pictMainRear.StretchImageToFit = True
        txtChqCount.Text = Val(dgvOutCreditMicr.RowCount) + Val(dgvRejectedItem.RowCount)
        txtChqCount.Enabled = False
        txtAccountID.Text = accID
        txtAccName.Text = AccName
        txtBankID.Enabled = False
        txtBranchID.Enabled = False
        txtChqDigit.Enabled = False
        txtChqNo.Enabled = False
        txtTheirAccID.Enabled = False
        txtVoucherCode.Enabled = False
        txtBankName.Enabled = False
        txtBranchName.Enabled = False

    End Sub

    Private Sub dgvOutCreditMicr_CellContextMenuStripChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvOutCreditMicr.CellContextMenuStripChanged

    End Sub

    Private Sub dgvOutCreditMicr_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvOutCreditMicr.CellEndEdit
        Try
            Select Case e.ColumnIndex.ToString
                Case 1 'Amount
                    'TODO: Work on Value Capping for all currencies
                    If Not IsNumeric(dgvOutCreditMicr.Rows(e.RowIndex).Cells(e.ColumnIndex).Value) Then
                        MessageBox.Show("Invalid Amount entered", "Br Clearing")
                        dgvOutCreditMicr.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = 0
                        Exit Sub
                    ElseIf dgvOutCreditMicr.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString.Contains("-") Then
                        MessageBox.Show("Invalid Amount entered", "Br Clearing")
                        dgvOutCreditMicr.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = 0
                        Exit Sub
                    End If
                    Dim dgv As DataGridView = DirectCast(sender, DataGridView)
                    Dim total As Decimal

                    ExecuteData(GetModify("sp_GetRTGSValue", "CurrencyID", dgvOutCreditMicr.Rows(e.RowIndex).Cells("colCurrencyID").Value.ToString, "OurBranchID", OurBranchID), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
                    If publicDTbl.Rows.Count > 0 Then
                        If dgvOutCreditMicr.Rows(e.RowIndex).Cells(e.ColumnIndex).Value > Val(publicDTbl.Rows(0)(0)) Then
                            MessageBox.Show("The value entered exceed the value cap set, please re-enter again")
                            dgvOutCreditMicr.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = 0
                            Exit Sub
                        End If
                    End If
                    publicDTbl.Clear()
                    Select Case dgvOutCreditMicr.Rows(e.RowIndex).Cells("colVoucherCode").Value.ToString
                        Case "60", "61", "62"

                        Case Else
                            System.Convert.ToDecimal(dgvOutCreditMicr.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString.Replace(",", ""))
                            If ConfigurationManager.AppSettings("AcceptsOddCents").ToString() = "1" Then
                                dgvOutCreditMicr.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = RoundTo5Cents(Val(Replace(dgvOutCreditMicr.Rows(e.RowIndex).Cells(e.ColumnIndex).Value, ",", "")))
                            End If
                    End Select


                    If dgvOutCreditMicr.Rows(e.RowIndex).Cells("colIsUpcountry").Value = 1 Then
                        Dim TotalCommis As Double = dgvOutCreditMicr.Rows(e.RowIndex).Cells(e.ColumnIndex).Value * Val(0.25 / 100)
                        Dim OurCommis As Double = TotalCommis * Val(25 / 100)
                        Dim TheirComm As Double = TotalCommis - OurCommis

                        If Val(OurCommis + TheirComm) < 100 Then
                            'I Have hardcoded this to poesha moto first - Kamunya
                            OurCommis = 25
                            TheirComm = 75
                            TotalCommis = 100
                        End If
                        TotalCommis = RoundTo5Cents(System.Convert.ToDecimal(TotalCommis))
                        OurCommis = RoundTo5Cents(System.Convert.ToDecimal(OurCommis))
                        TheirComm = RoundTo5Cents(System.Convert.ToDecimal(TheirComm))
                        dgvOutCreditMicr.Rows(e.RowIndex).Cells("colOurCommission").Value = OurCommis
                        dgvOutCreditMicr.Rows(e.RowIndex).Cells("colTheirCommission").Value = TheirComm
                        dgvOutCreditMicr.Rows(e.RowIndex).Cells("colTotalCommission").Value = TotalCommis
                    End If

                    For Each r As DataGridViewRow In dgv.Rows
                        total += Replace(r.Cells(1).Value, ",", "")
                        r.Cells(1).Value = FormatNumber(r.Cells(1).Value, 2)
                    Next
                    lblAmount.Text = FormatNumber(total, 2)
                    lblCount.Text = dgv.RowCount

                    'Validate the clearing Days
                    If Modscan.SysType <> Modscan.ENUM_SysType.BRNET Then
                        ExecuteData(GetModify("sp_GetValueDate", "ourbranchid", OurBranchID, "AccountID", dgvOutCreditMicr.Rows(e.RowIndex).Cells("colAccountID").Value, "Date", cFromDate, "VoucherCode", dgvOutCreditMicr.Rows(e.RowIndex).Cells("colVoucherCode").Value, "BankID", dgvOutCreditMicr.Rows(e.RowIndex).Cells("colBankID").Value.ToString.Substring(0, 2), "BranchID", dgvOutCreditMicr.Rows(e.RowIndex).Cells("colBranch").Value.ToString.Substring(0, 3), "Amount", Convert.ToDouble(dgvOutCreditMicr.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.SelectStatement)
                    Else
                        ExecuteData(GetModify("p_GetValueDate", "ourbranchid", OurBranchID, "AccountID", dgvOutCreditMicr.Rows(e.RowIndex).Cells("colAccountID").Value, "ReturnCode ", "00", "CurrencyID", dgvOutCreditMicr.Rows(e.RowIndex).Cells("colCurrencyID").Value, "WorkingDate", cFromDate, "VoucherCode", dgvOutCreditMicr.Rows(e.RowIndex).Cells("colVoucherCode").Value, "AccountTypeID", "C", "BankID", dgvOutCreditMicr.Rows(e.RowIndex).Cells("colBankID").Value.ToString.Substring(0, 2), "BranchID", dgvOutCreditMicr.Rows(e.RowIndex).Cells("colBranch").Value.ToString.Substring(0, 3), "Amount", Convert.ToDouble(dgvOutCreditMicr.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.SelectStatement)
                    End If
                    If publicDTbl.Rows.Count > 0 Then
                        dgvOutCreditMicr.Rows(e.RowIndex).Cells("colclgDays").Value = publicDTbl.Rows(0)("ClearingDays").ToString
                        dgvOutCreditMicr.Rows(e.RowIndex).Cells("colValueDate").Value = publicDTbl.Rows(0)("ValueDate").ToString
                    Else
                        dgvOutCreditMicr.Rows(e.RowIndex).Cells("colclgDays").Value = "4"
                    End If
                    publicDTbl.Clear()
                Case 6 'Chequedate

                Case 7 'Clearing Days

                Case 8 'ReturnCode

                    'Validate the ReturnCode
                    If Modscan.SysType <> Modscan.ENUM_SysType.BRNET Then
                        ExecuteData("exec sp_GetReturnReasons", publicDTbl, dataExecTypes.ExecTypeQuery, queryType.SelectStatement)
                    Else
                        dgvOutCreditMicr.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = "00"
                    End If
                    If publicDTbl.Rows.Count > 0 Then
                        If publicDTbl.Select("ReturnID='" & dgvOutCreditMicr.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString & "'").Length > 0 Then

                        Else
                            MessageBox.Show("Please use a valid ReturnCode")
                            dgvOutCreditMicr.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = "00"
                            publicDTbl.Clear()
                            Exit Try
                        End If
                    End If
                    publicDTbl.Clear()

                    'Validate the clearing Days
                    If Modscan.SysType <> Modscan.ENUM_SysType.BRNET Then
                        ExecuteData(GetModify("sp_GetValueDate", "ourbranchid", OurBranchID, "AccountID", dgvOutCreditMicr.Rows(e.RowIndex).Cells("colAccountID").Value, "Date", cFromDate, "VoucherCode", "BankID", "BranchID", "Amount", "AccountID"), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.SelectStatement)
                    Else
                        ExecuteData(GetModify("p_GetValueDate", "ourbranchid", OurBranchID, "AccountID", dgvOutCreditMicr.Rows(e.RowIndex).Cells("colAccountID").Value, "Date", cFromDate, "VoucherCode", "BankID", "BranchID", "Amount", "AccountID", "AccountTypeID"), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.SelectStatement)
                    End If
                    If publicDTbl.Rows.Count > 0 Then
                        dgvOutCreditMicr.Rows(e.RowIndex).Cells("colclgDays").Value = publicDTbl.Rows(0)("ClearingDays").ToString
                    Else
                        dgvOutCreditMicr.Rows(e.RowIndex).Cells("colclgDays").Value = "4"
                    End If
                    publicDTbl.Clear()


                    If dgvOutCreditMicr.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString <> "00" Then
                        dgvOutCreditMicr.Rows(e.RowIndex).Cells(7).Value = 0
                    End If
            End Select

            chDate = dgvOutCreditMicr.Rows(e.RowIndex).Cells(e.ColumnIndex).Value
            If (Me.dgvOutCreditMicr.Focused AndAlso (Me.dgvOutCreditMicr.CurrentCell.ColumnIndex = 6)) Then
                Me.dgvOutCreditMicr.CurrentCell.Value = chDate
            End If
        Catch ex As Exception

        End Try

    End Sub
    Private Sub picker_ValueChanged(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Me.dgvOutCreditMicr.CurrentCell.Value = picker.Text
        Catch ex As Exception
            MessageBox.Show("Invalid Date")
        End Try

    End Sub
    Private Sub dgvOutCreditMicr_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles dgvOutCreditMicr.CellFormatting
        Try
            If ((e.RowIndex > -1) AndAlso (e.RowIndex <> Me.dgvOutCreditMicr.NewRowIndex)) Then
                If (e.ColumnIndex = Me.dgvOutCreditMicr.Columns("colChqDate").Index) Then
                    e.Value = CType(e.Value, DateTime).ToString("dd MMM yyyy")

                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Invalid Date")
        End Try
    End Sub
    Private Sub dgvOutCreditMicr_CellValidating(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellValidatingEventArgs) Handles dgvOutCreditMicr.CellValidating
        Try
            Select Case UCase(dgvOutCreditMicr.CurrentCell.OwningColumn.Index)
                Case "1"
                    If IsNumeric(e.FormattedValue) = False Then Exit Sub
                    If FormatNumber(dgvOutCreditMicr.CurrentCell.Value, 2) <> FormatNumber(e.FormattedValue, 2) Then
                        dgvOutCreditMicr.CurrentCell.Value = FormatNumber(e.FormattedValue, 2)
                        Me.dgvOutCreditMicr.CurrentRow.DefaultCellStyle.BackColor = Color.MediumSeaGreen
                    End If
                    dgvOutCreditMicr.CurrentCell.Value = FormatNumber(dgvOutCreditMicr.CurrentCell.Value, 2)
            End Select
            dgvOutCreditMicr.UpdateCellValue(e.ColumnIndex, e.RowIndex)
        Catch ex As Exception

        End Try
    End Sub
    Private Sub dgvOutCreditMicr_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgvOutCreditMicr.KeyDown
        Try

            If e.KeyCode = Keys.Delete Then
                If dgvOutCreditMicr.Rows.Count > 0 Then
                    If MessageBox.Show("This will delete The Selected Item completely, do you wish to continue?", Nothing, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
                        Exit Sub
                    Else
                        If dgvOutCreditMicr.Rows(dgvOutCreditMicr.CurrentRow.Index).Cells("ColumnID").Value <> "" Then
                            ExecuteData(GetModify("sp_DeleteT_BRChequeTruncation", "ourbranchid", OurBranchID, "ColumnID", dgvOutCreditMicr.Rows(dgvOutCreditMicr.CurrentRow.Index).Cells("ColumnID").Value, "OperatorID", OperatorID), publicDTbl, dataExecTypes.ExecTypeNonQuery, queryType.SelectStatement)
                        End If
                        dgvOutCreditMicr.Rows.RemoveAt(dgvOutCreditMicr.CurrentRow.Index)
                        ClearCTl()
                        Dim dgv As DataGridView = DirectCast(sender, DataGridView)
                        Dim total As Decimal
                        For Each r As DataGridViewRow In dgv.Rows
                            total += Replace(r.Cells(1).Value, ",", "")
                            r.Cells(1).Value = FormatNumber(r.Cells(1).Value, 2)
                        Next
                        lblAmount.Text = FormatNumber(total, 2)
                        lblCount.Text = dgv.RowCount
                    End If
                End If
            End If
            txtChqCount.Text = Val(dgvOutCreditMicr.RowCount) + Val(dgvRejectedItem.RowCount)
            txtChqCount.Enabled = False
            picker.Value = Format(dgvOutCreditMicr.Rows(dgvOutCreditMicr.CurrentRow.Index).Cells(6).Value, "dd MMM yyyy")
            chDate = picker.Value
        Catch ex As Exception

        End Try
    End Sub


    Private Sub ClearCTl()
        Dim con As Control
        For Each con In GroupBox1.Controls
            If TypeOf con Is TextBox Then
                con.Text = ""
            End If
        Next
        pictMainFront.Image = Nothing
        pictMainRear.Image = Nothing
        pictPreview1.Image = Nothing
        pictPreview2.Image = Nothing
        pictPreview3.Image = Nothing
        pictPreview4.Image = Nothing
        BackGraySPic.Image = Nothing
        FrontBWPic.Image = Nothing

    End Sub

    Private Sub dgvRejectedItem_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvRejectedItem.CellContentClick

    End Sub

    Private Sub dgvRejectedItem_CellContentDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvRejectedItem.CellContentDoubleClick
        Dim accID As String = txtAccountID.Text
        Dim AccName As String = txtAccName.Text
        ClearCTl()
        EnableDisableCtl(4)
        txtAccName.Text = AccName
        txtAccountID.Text = accID
        txtClrCenter.Text = CodeLineDetails.CountryClearingCenter
        txtClrCenter.Enabled = False
        RowID = dgvRejectedItem.Rows(e.RowIndex).Index
        'txtBankID.Text = dgvRejectedItem.Rows(e.RowIndex).Cells(1).Value.ToString.Substring(0, 2)
        'txtBranchID.Text = dgvRejectedItem.Rows(e.RowIndex).Cells(2).Value.ToString.Substring(0, 3)
        txtChqDigit.Text = dgvRejectedItem.Rows(e.RowIndex).Cells(4).Value
        txtChqNo.Text = dgvRejectedItem.Rows(e.RowIndex).Cells(0).Value
        txtTheirAccID.Text = dgvRejectedItem.Rows(e.RowIndex).Cells(6).Value
        txtVoucherCode.Text = dgvRejectedItem.Rows(e.RowIndex).Cells(5).Value
        txtChqNo.Text = "" : txtBankID.Text = "" : txtBranchID.Text = "" : txtChqDigit.Text = "" : txtVoucherCode.Text = "" : txtTheirAccID.Text = ""
        txtBankName.Text = "" : txtBranchName.Text = ""

        pictMainFront.Image = GetImages(String2Bytes(dgvRejectedItem.Rows(e.RowIndex).Cells(7).Value))
        pictPreview1.Image = GetImages(String2Bytes(dgvRejectedItem.Rows(e.RowIndex).Cells(7).Value))
        FrontBWPic.Image = GetImages(String2Bytes(dgvRejectedItem.Rows(e.RowIndex).Cells(11).Value))
        pictMainRear.Image = GetImages(String2Bytes(dgvRejectedItem.Rows(e.RowIndex).Cells(8).Value))
        BackGraySPic.Image = GetImages(String2Bytes(dgvRejectedItem.Rows(e.RowIndex).Cells(8).Value))
        pictPreview2.Image = GetImages(StringToByte(dgvOutCreditMicr.Rows(e.RowIndex).Cells(13).Value))

        pictMainFront.StretchImageToFit = True
        pictMainRear.StretchImageToFit = True
        'txtChqCount.Text = Val(dgvOutCreditMicr.RowCount) + Val(dgvRejectedItem.RowCount)
        txtChqCount.Enabled = False
        CodeLineDetails.ChequeID = ""
        CodeLineDetails.Amount = 0
        CodeLineDetails.BankID = ""
        CodeLineDetails.BranchID = ""
        CodeLineDetails.ChequeDigit = ""
        CodeLineDetails.VoucherCode = ""
        CodeLineDetails.TheirAccountID = ""
        CodeLineDetails.ReturnCode = ""


        btnOk.Visible = True
        btnOk.Enabled = True
        buttonFreeTrack.Visible = False
        buttonStartFeed.Enabled = False
        txtTheirAccID.Focus()
    End Sub

    Private Sub dgvRejectedItem_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgvRejectedItem.KeyDown
        Try
            If e.KeyCode = Keys.Delete Then
                If dgvOutCreditMicr.Rows.Count > 0 Then
                    If MessageBox.Show("This will delete The Selected Item completely, do you wish to continue?", Nothing, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then Exit Sub
                    dgvOutCreditMicr.Rows.RemoveAt(dgvOutCreditMicr.CurrentRow.Index)
                    ClearCTl()
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Dim MicrLine As String = ""
        Dim accID As String = txtAccountID.Text
        Dim AccName As String = txtAccName.Text

        Try
            CodeLineDetails.ChequeID = ""
            CodeLineDetails.Amount = 0
            CodeLineDetails.BankID = ""
            CodeLineDetails.BranchID = ""
            CodeLineDetails.ChequeDigit = ""
            CodeLineDetails.VoucherCode = ""
            CodeLineDetails.TheirAccountID = ""
            CodeLineDetails.CountryClearingCenter = txtClrCenter.Text
            btnOk.Visible = False
            buttonFreeTrack.Visible = True
            buttonStartFeed.Enabled = True
            txtTheirAccID.Text = New String("0", 10 - Len(txtTheirAccID.Text)) & txtTheirAccID.Text
            txtChqNo.Text = New String("0", 6 - Len(txtChqNo.Text)) & txtChqNo.Text

            MicrLine = txtChqNo.Text & txtBankID.Text & txtBranchID.Text & txtChqDigit.Text & txtVoucherCode.Text & txtTheirAccID.Text
            cTrxType = "O"
            If Modscan.SysType <> Modscan.ENUM_SysType.BRNET Then
                ValidateMicrLenght(MicrLine, True)
            Else
                ValidateMicrLenghtNet(MicrLine, True)
            End If
            CodeLineDetails.ChequeID = txtChqNo.Text
            CodeLineDetails.Amount = 0
            CodeLineDetails.ChequeDigit = txtChqDigit.Text
            CodeLineDetails.VoucherCode = txtVoucherCode.Text
            CodeLineDetails.TheirAccountID = txtTheirAccID.Text
            CodeLineDetails.CountryClearingCenter = txtClrCenter.Text
            If ReaderUsed = ReaderType.CTS Then
                CodeLineDetails.UVImagePath = dgvRejectedItem.Rows(RowID).Cells("ColUVImagePath").Value
            End If
            CodeLineDetails.FrontImagePathGrayScale = dgvRejectedItem.Rows(RowID).Cells("colJFImagePath").Value
            CodeLineDetails.BackImagePath = dgvRejectedItem.Rows(RowID).Cells("colJRImagePath").Value
            CodeLineDetails.FrontImagePathBW = dgvRejectedItem.Rows(RowID).Cells("colTFImagePath").Value

            pictMainFront.Image = GetImages(String2Bytes(dgvRejectedItem.Rows(RowID).Cells("colFrontimg").Value))
            pictPreview1.Image = GetImages(String2Bytes(dgvRejectedItem.Rows(RowID).Cells("colFrontimg").Value))
            FrontBWPic.Image = GetImages(String2Bytes(dgvRejectedItem.Rows(RowID).Cells("TFImageBW").Value))
            pictMainRear.Image = GetImages(String2Bytes(dgvRejectedItem.Rows(RowID).Cells("colBackImg").Value))
            BackGraySPic.Image = GetImages(String2Bytes(dgvRejectedItem.Rows(RowID).Cells("colBackImg").Value))
            pictPreview2.Image = GetImages(dgvRejectedItem.Rows(RowID).Cells("colUVImg").Value)
            CodeLineDetails.FrontImageGrayScale = dgvRejectedItem.Rows(RowID).Cells("colFrontimg").Value
            CodeLineDetails.FrontImageBW = dgvRejectedItem.Rows(RowID).Cells("TFImageBW").Value
            CodeLineDetails.BackImageGrayScale = dgvRejectedItem.Rows(RowID).Cells("colBackImg").Value
            CodeLineDetails.UVimage = dgvRejectedItem.Rows(RowID).Cells("colUVImg").Value


            AddDataToGrid()
            'Have added this to refresh the grid with good items
            If dgvOutCreditMicr.RowCount > 0 Then
                pictMainFront.Image = GetImages(String2Bytes(dgvOutCreditMicr.Rows(RowID).Cells(11).Value))
                pictPreview1.Image = GetImages(String2Bytes(dgvOutCreditMicr.Rows(RowID).Cells(11).Value))
                FrontBWPic.Image = GetImages(String2Bytes(dgvOutCreditMicr.Rows(RowID).Cells("TFImage").Value))
                pictMainRear.Image = GetImages(String2Bytes(dgvOutCreditMicr.Rows(RowID).Cells(12).Value))
                BackGraySPic.Image = GetImages(String2Bytes(dgvOutCreditMicr.Rows(RowID).Cells(12).Value))
                pictPreview2.Image = GetImages(dgvOutCreditMicr.Rows(RowID).Cells(13).Value)
                pictMainFront.StretchImageToFit = True
                pictMainRear.StretchImageToFit = True
            End If
            dgvOutCreditMicr.Refresh()
            ClearCTl()
            EnableDisableCtl(3)
            buttonStopFeed_Click(sender, e)
            'Remove this row from rejected grid
            dgvRejectedItem.Rows.RemoveAt(RowID)
            RowID = 0
            btnOk.Enabled = False
            btnOk.Visible = False
            txtAccountID.Text = accID
            txtAccName.Text = AccName
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub txtChqNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtChqNo.KeyDown
        If e.KeyValue = Keys.Enter Then
            txtVoucherCode.Focus()
        End If
    End Sub

    Private Sub txtChqNo_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtChqNo.LostFocus
        txtChqNo.Text = New String("0", 6 - Len(txtChqNo.Text)) & txtChqNo.Text
    End Sub

    Private Sub txtTheirAccID_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTheirAccID.KeyDown
        If e.KeyValue = Keys.Enter Then
            txtChqDigit.Focus()
        End If
    End Sub

    Private Sub txtTheirAccID_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTheirAccID.LostFocus
        txtTheirAccID.Text = New String("0", 10 - Len(txtTheirAccID.Text.TrimStart("0"))) & txtTheirAccID.Text.TrimStart("0")
    End Sub

    Private Sub txtTheirAccID_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTheirAccID.TextChanged
        'txtTheirAccID.Text = New String("0", 10 - Len(txtTheirAccID.Text.TrimStart("0"))) & txtTheirAccID.Text.TrimStart("0")
    End Sub

    Private Sub txtChqCount_Enter(sender As Object, e As EventArgs) Handles txtChqCount.Enter
        buttonStartFeed_Click(sender, e)
    End Sub
    Private Sub txtChqCount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtChqCount.KeyPress
        Dim c As Char = e.KeyChar
        Dim i As Integer = Asc(c) '--convert value into ascii
        '--0-9 first--'
        If (i >= 47 And i < 58) _
        Or (i = 43) Or (i = 45) Then '--for + and - keys
        Else
            If i = 8 Then '--for space
            Else
                e.Handled = True
            End If
        End If
    End Sub
    Private Sub txtChqCount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtChqCount.KeyDown
        If e.KeyValue = Keys.Enter Then
            buttonStartFeed.Focus()
        End If
    End Sub

    Private Sub CTSInitialization()
        Try
            'MessageBox.Show("imeingia hapa CTSInitialization()")

            Dim Model As String = New String("", 20)
            Dim Version As String = New String("", 20)
            Dim FwDate As String = New String("", 20)
            Dim UnitID As String = New String("", 20)
            Dim sPrint As String = "Br Clearing test print"
            Dim CodelineRead As String = New String(" ", 256)
            Dim fImage As Bitmap = Nothing
            Dim rImage As Bitmap = Nothing
            Dim rc As Int32
            Ls = New LsFamily.LsApi
            CtsIQA = New LsFamily.CtsIQA
            cApplFunc = New ApplClass
            Try
                If ConfigurationManager.AppSettings("TempImageDrive").ToString <> "" Then
                    ImagePath = ConfigurationManager.AppSettings("TempImageDrive").ToString() + ApplClass.SAVE_DIRECTORY_IMAGE
                Else
                    ImagePath = strImagePath + "\" + ApplClass.SAVE_DIRECTORY_IMAGE
                End If
            Catch ex As Exception
                ImagePath = "C:\" + ApplClass.SAVE_DIRECTORY_IMAGE
                If Not System.IO.Directory.Exists(ImagePath) Then
                    System.IO.Directory.CreateDirectory(ImagePath)
                End If
            End Try
            'MessageBox.Show(ImagePath)
            Try
                If Not System.IO.Directory.Exists(ImagePath) Then
                    System.IO.Directory.CreateDirectory(ImagePath)
                End If
            Catch ex As Exception
                ImagePath = "C:\Images\" + ApplClass.SAVE_DIRECTORY_IMAGE
                If Not System.IO.Directory.Exists(ImagePath) Then
                    System.IO.Directory.CreateDirectory(ImagePath)
                End If
            End Try
            'MessageBox.Show("About to start LS configs")
            'MessageBox.Show("ReadConfiguration")
            cApplFunc.ReadConfiguration(cApplFunc, LsDefines)
            'MessageBox.Show("Done ReadConfiguration")
            'cApplFunc.SaveConfiguration(cApplFunc)
            ' Set IP for LS100IP
            'Ls.LSSetIPAddress(LsFamily.LsDefines.LsUnitType.LS_100_ETH, cApplFunc.IP_Address, 4000)
            ' Set IP for LS150IP
            'MessageBox.Show("LSConnect")
            'Ls.LSSetIPAddress(LsFamily.LsDefines.LsUnitType.LS_150_ETH, cApplFunc.IP_Address, 4000)
            rc = Ls.LSConnect(0, LsUnitType, hLS, True)
            'MessageBox.Show("Done LSConnect")
            'MessageBox.Show(rc)
            If rc = -1000 Then
                textState.Text = "CTS Connected!"
                textState.BackColor = Color.GreenYellow

            Else
                textState.Text = "CTS Failed Connecting"
                textState.BackColor = Color.Red
            End If
        Catch ex As Exception
            MessageBox.Show("Imechapa huku ndani CTSInitialization()")
        End Try
    End Sub

    Private Sub MultiDocHandle(ByVal fFeederEmpty As Boolean)
        Dim rc As Int32
        Dim LsCfg As LsFamily.LsConfiguration = New LsFamily.LsConfiguration
        'Dim Ls As LsFamily.LsApi
        Dim Codeline As Int16
        Dim pos_x As Double
        Dim pos_y As Double
        Dim pos_w As Double
        Dim pos_h As Double
        Dim NrDoc As UInt32
        Dim fileFront As String = New String(" ", 256)
        Dim fileRear As String = New String(" ", 256)
        Dim fileFront2 As String = New String(" ", 256)
        Dim CodelineSw As String = Nothing
        Dim CodelineHw As String = Nothing
        Dim len_codeline As Int16
        Dim FrontImage As Bitmap = Nothing
        Dim BackImage As Bitmap = Nothing
        Dim FrontImage2 As Bitmap = Nothing
        Dim PrintFormat As Byte
        Dim PrintValidate As Int16
        Dim Stamp As Int16
        Dim DocPerMin As Int32
        Dim timeStart As Int32
        Dim timeEnd As Int32
        Dim PrintFont1 As Byte
        Dim Endorse_str As String
        Dim PrintFont2 As Byte
        Dim Endorse_str2 As String
        Dim PrintFont3 As Byte
        Dim Endorse_str3 As String
        Dim PrintFont4 As Byte
        Dim Endorse_str4 As String
        Dim cb As LsFamily.LS515OnCodelineCallBack
        Dim cb800c As LsFamily.LS800OnCodelineCallBack
        Dim cb800i As LsFamily.LS800OnImageCallBack
        Dim BarcodeToread As Int16
        Dim UniqueFilename As String = ""
        Dim ConstNum As Int16 = 0
        Dim img As Bitmap
        Dim bm As Bitmap
        UniqueFilename = GetNextString()
        Dim dpi As System.Drawing.Image
        Dim ImageDpi As Int32
        Dim Reply As Integer
        Dim EndorseChq As Boolean = False

        Try
            If Me.txtChqCount.Text = "" Then
                MessageBox.Show("Provide the number of cheques to be accepted.")
                Me.txtChqCount.Focus()
                Exit Try
            ElseIf Me.txtChqCount.Text = 0 Then
                MessageBox.Show("Provide the number of cheques to be accepted.")
                Me.txtChqCount.Focus()
                Exit Try
            End If

            'MessageBox.Show("Ndio Kuanza.")
            NrDocVideo = 0
            'Ls = New LsFamily.LsApi
            rc = Ls.LSConnect(0, LsUnitType, hLS, True)

            If (rc = LsFamily.LsReply.LS_OKAY Or rc = LsFamily.LsReply.LS_ALREADY_OPEN) Then

                rc = Ls.LSReset(hLS, 0, LsFamily.LsDefines.Reset.RESET_ERROR)

                'do an inquiry to keep the unit configuration
                rc = Ls.LSIdentify(hLS, 0, LsCfg, Nothing, Nothing, Nothing, Nothing)

                If (cApplFunc.DuobleLeafingLevel = LsFamily.LsDefines.DoubleLeafing.DOUBLE_LEAFING_DISABLE) Then
                    rc = Ls.LSDoubleLeafingSensibility(hLS, 0, cApplFunc.DuobleLeafingLevel)
                Else
                    rc = Ls.LSDoubleLeafingSensibility(hLS, 0, cApplFunc.DuobleLeafingValue)
                End If

                rc = Ls.LSDisableWaitDocument(hLS, 0, cApplFunc.WiatDoc)
                'Printing Setting done here
                ExecuteData(GetModify("P_ClearingChequeEndorsementDetails", "OurBranchID", Modscan.OurBranchID), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.SelectStatement)
                If publicDTbl.Rows.Count > 0 Then
                    PrintValidate = CtsLs.PrintValidate.PRINT_VALIDATE
                    cApplFunc.PrintValidate = formOptions.ComboFont.COMBO_FONT_BOLD
                    PrintValidate = LsFamily.LsDefines.PrintValidate.PRINT_VALIDATE_YES
                    LsCfg.InkJet_Printer_4_lines = False
                    Try
                        cApplFunc.Endorse_str = publicDTbl.Rows(0)("Endorse1").ToString + " " + FormatDateTime(publicDTbl.Rows(0)("ClearingDate").ToString(), DateFormat.GeneralDate)
                        cApplFunc.Endorse_str2 = FormatDateTime(publicDTbl.Rows(0)("ClearingDate").ToString(), DateFormat.GeneralDate)
                        cApplFunc.Endorse_str3 = publicDTbl.Rows(0)("Endorse2").ToString
                        cApplFunc.Endorse_str4 = publicDTbl.Rows(0)("Endorse3").ToString
                    Catch ex As Exception

                    End Try
                    If cApplFunc.Endorse_str <> String.Empty Then
                        cApplFunc.PrintValidate1 = 1
                    Else
                        cApplFunc.PrintValidate1 = 0
                    End If
                    If cApplFunc.Endorse_str2 <> String.Empty Then
                        cApplFunc.PrintValidate2 = 1
                    Else
                        cApplFunc.PrintValidate2 = 0
                    End If
                    If cApplFunc.Endorse_str3 <> String.Empty Then
                        cApplFunc.PrintValidate3 = 1
                    Else
                        cApplFunc.PrintValidate3 = 0
                    End If
                    If cApplFunc.Endorse_str4 <> String.Empty Then
                        cApplFunc.PrintValidate4 = 1
                    Else
                        cApplFunc.PrintValidate4 = 0
                    End If
                Else
                    LsCfg.InkJet_Printer_4_lines = False
                    cApplFunc.PrintValidate1 = 0
                    cApplFunc.PrintValidate2 = 0
                    cApplFunc.PrintValidate3 = 0
                    cApplFunc.PrintValidate4 = 0
                    Endorse_str = ""
                    Endorse_str2 = ""
                    Endorse_str3 = ""
                    Endorse_str4 = ""
                End If


                If LsCfg.InkJet_Printer_4_lines Then
                    If cApplFunc.PrintValidate1 Then
                        'PrintFont1 = cApplFunc.PrintValidate1
                        Endorse_str = cApplFunc.Endorse_str
                        'PrintValidate = LsFamily.LsDefines.PrintValidate.PRINT_VALIDATE_WITH_LOGO
                        PrintValidate = LsFamily.LsDefines.PrintValidate.PRINT_VALIDATE_YES
                    Else
                        PrintFont1 = LsFamily.LsDefines.PrintFont.PRINT_FORMAT_NORMAL
                        Endorse_str = ""
                    End If

                    If cApplFunc.PrintValidate2 Then
                        'PrintFont2 = cApplFunc.PrintValidate2
                        Endorse_str2 = cApplFunc.Endorse_str2
                        'PrintValidate = LsFamily.LsDefines.PrintValidate.PRINT_VALIDATE_WITH_LOGO
                        PrintValidate = LsFamily.LsDefines.PrintValidate.PRINT_VALIDATE_YES
                    Else
                        PrintFont2 = LsFamily.LsDefines.PrintFont.PRINT_FORMAT_NORMAL
                        Endorse_str2 = ""
                    End If

                    If cApplFunc.PrintValidate3 Then
                        'PrintFont3 = cApplFunc.PrintValidate3
                        Endorse_str3 = cApplFunc.Endorse_str3
                        'PrintValidate = LsFamily.LsDefines.PrintValidate.PRINT_VALIDATE_WITH_LOGO
                        PrintValidate = LsFamily.LsDefines.PrintValidate.PRINT_VALIDATE_YES
                    Else
                        PrintFont3 = LsFamily.LsDefines.PrintFont.PRINT_FORMAT_NORMAL
                        Endorse_str3 = ""
                    End If

                    If cApplFunc.PrintValidate4 Then
                        'PrintFont4 = cApplFunc.PrintValidate4
                        Endorse_str4 = cApplFunc.Endorse_str4
                        'PrintValidate = LsFamily.LsDefines.PrintValidate.PRINT_VALIDATE_WITH_LOGO
                        PrintValidate = LsFamily.LsDefines.PrintValidate.PRINT_VALIDATE_YES
                    Else
                        PrintFont4 = LsFamily.LsDefines.PrintFont.PRINT_FORMAT_NORMAL
                        Endorse_str4 = ""
                    End If

                    rc = Ls.LSLoadMultiStrings(hLS, 0, PrintFont1, Endorse_str, PrintFont2, Endorse_str2, PrintFont3, Endorse_str3, PrintFont4, Endorse_str4)

                ElseIf LsCfg.InkJet_Printer Then
                    Select Case cApplFunc.PrintValidate
                        Case formOptions.ComboFont.COMBO_NO_PRINT
                            PrintValidate = LsFamily.LsDefines.PrintValidate.PRINT_VALIDATE_NO
                        Case formOptions.ComboFont.COMBO_FONT_NORMAL
                            If (cApplFunc.Print_High) Then
                                PrintFormat = LsFamily.LsDefines.PrintFont.PRINT_UP_FORMAT_NORMAL
                                PrintValidate = LsFamily.LsDefines.PrintValidate.PRINT_VALIDATE_YES
                            Else
                                PrintFormat = LsFamily.LsDefines.PrintFont.PRINT_FORMAT_NORMAL
                                PrintValidate = LsFamily.LsDefines.PrintValidate.PRINT_VALIDATE_YES
                            End If
                        Case formOptions.ComboFont.COMBO_FONT_BOLD
                            If (cApplFunc.Print_High) Then
                                PrintFormat = LsFamily.LsDefines.PrintFont.PRINT_UP_FORMAT_BOLD
                                PrintValidate = LsFamily.LsDefines.PrintValidate.PRINT_VALIDATE_YES
                            Else
                                PrintFormat = LsFamily.LsDefines.PrintFont.PRINT_UP_FORMAT_BOLD
                                PrintValidate = LsFamily.LsDefines.PrintValidate.PRINT_VALIDATE_YES
                            End If
                        Case formOptions.ComboFont.COMBO_FONT_15_INCH
                            If (cApplFunc.Print_High) Then
                                PrintFormat = LsFamily.LsDefines.PrintFont.PRINT_UP_FORMAT_NORMAL_15_CHAR
                                PrintValidate = LsFamily.LsDefines.PrintValidate.PRINT_VALIDATE_YES
                            Else
                                PrintFormat = LsFamily.LsDefines.PrintFont.PRINT_FORMAT_NORMAL_15
                                PrintValidate = LsFamily.LsDefines.PrintValidate.PRINT_VALIDATE_YES
                            End If
                        Case Else
                            PrintValidate = LsFamily.LsDefines.PrintValidate.PRINT_VALIDATE_NO
                    End Select

                    If (PrintValidate <> LsFamily.LsDefines.PrintValidate.PRINT_VALIDATE_NO) Then
                        rc = Ls.LSLoadString(hLS, 0, PrintFormat, cApplFunc.Endorse_str, 1, 3)
                    End If
                End If

                ' Set type codeline to Read
                If (cApplFunc.Codeline_HW = LsFamily.LsDefines.CodelineToRead.READ_CODELINE_HW_MICR) Then
                    CodelineHw = New String(" ")
                    Codeline = cApplFunc.Codeline_HW
                    pos_x = 0
                    pos_y = 0
                    pos_w = 0
                    pos_h = 0
                End If

                If (cApplFunc.FrontStamp) Then
                    Stamp = LsFamily.LsDefines.Stamp.STAMP_FRONT
                End If

                'Sorters
                If (LsUnitType = LsFamily.LsDefines.LsUnitType.LS_100_USB Or LsUnitType = LsFamily.LsDefines.LsUnitType.LS_100_ETH Or
                    LsUnitType = LsFamily.LsDefines.LsUnitType.LS_40_USB) Then
                    cApplFunc.Sorter = cApplFunc.Sorter_Ls100
                End If
                If (LsUnitType = LsFamily.LsDefines.LsUnitType.LS_150_USB) Then
                    cApplFunc.Sorter = cApplFunc.Sorter_Ls150
                End If
                If (LsUnitType = LsFamily.LsDefines.LsUnitType.LS_515_USB Or LsUnitType = LsFamily.LsDefines.LsUnitType.LS_520_USB) Then
                    cApplFunc.Sorter = cApplFunc.Sorter_Ls520
                End If
                If (LsUnitType = LsFamily.LsDefines.LsUnitType.LS_800_USB) Then
                    cApplFunc.Sorter = cApplFunc.Sorter_Ls800
                End If

                'Uv
                If (LsUnitType = LsFamily.LsDefines.LsUnitType.LS_150_USB) Then
                    If (LsCfg.ScannerUltraViolet) Then
                        Ls.LSModifyPWMUltraViolet(hLS, 0, cApplFunc.PWMvalue, cApplFunc.HightContrast, 0)
                    End If


                End If

                DocPerMin = 0
                timeStart = Environment.TickCount()

                If (LsUnitType = LsFamily.LsDefines.LsUnitType.LS_800_USB) Then
                    If (Codeline = LsFamily.LsDefines.CodelineToRead.READ_CODELINE_HW_MICR) Then
                        cb800c = AddressOf Ls800CodelineRead
                        cb800i = Nothing
                    Else
                        cb800c = Nothing
                        cb800i = AddressOf Ls800ImageRead
                    End If

                    ' Set the initial pocket
                    NrTot_Pocket = LsCfg.Sorters_Nr * 3
                    If cApplFunc.Sorter = LsFamily.LsDefines.Sorter.SORTER_SEQUENTIAL Then
                        CurrPocket = LsFamily.LsDefines.Sorter.SORTER_POCKET_1_SELECTED
                    ElseIf cApplFunc.Sorter = LsFamily.LsDefines.Sorter.SORTER_CIRCULAR Then
                        CurrPocket = LsFamily.LsDefines.Sorter.SORTER_POCKET_1_SELECTED
                    Else
                        CurrPocket = cApplFunc.Sorter
                    End If

                    rc = Ls.LS800AutoDocHandle(hLS, 0, PrintValidate, Codeline, cApplFunc.Side, cApplFunc.ScanMode, cApplFunc.ScanMode, LsFamily.LsDefines.ClearBlack.CLEAR_BLACK_YES, NrDocToProcess, cApplFunc.SaveMode, ImagePath, UniqueFilename, LsFamily.LsDefines.Unit.UNIT_MM, pos_x, pos_y, pos_w, pos_h, cApplFunc.FileFormat, Convert.ToInt16(0), LsFamily.LsDefines.FileAttribute.SAVE_REPLACE, 0, cApplFunc.Beep, cb800c, cb800i)
                Else
                    Try
                        cb = AddressOf Ls515CodelineRead
                        CurrPocket = LsFamily.LsDefines.Sorter.SORTER_POCKET_1



                        'BufFrontFile = Marshal.AllocHGlobal(1024)
                        'BufRearFile = Marshal.AllocHGlobal(1024)
                        'BufCodelineSW = Marshal.AllocHGlobal(CInt(Ls.CodeLineType.MAX_CODE_LINE_LENGTH))
                        'BufCodelineHW = Marshal.AllocHGlobal(CInt(CtsLs.CodeLineType.MAX_CODE_LINE_LENGTH))
                        'BufBarcode = Marshal.AllocHGlobal(CInt(CtsLs.CodeLineType.MAX_CODE_LINE_LENGTH))

                        NrDoc = 1
                        ' Start Doc.
                        'Reply = Ls.LSDocHandle(hConnect, 0, stParAppl.FrontStamp, CShort(PrintValidate), CShort(stParAppl.CodelineMICR), stParAppl.Side, _
                        ' stParAppl.ScanMode, CShort(CtsLs.Feeder.FEED_AUTO), CShort(CtsLs.Sorter.SORTER_POCKET_1), CShort(If(stParAppl.WaitTimeout, CtsLs.Wait.WAIT_YES, CtsLs.Wait.WAIT_NO)), CShort(stParAppl.BeepOnError), NrDoc, _
                        ' CShort(If(stParAppl.ScanCard, CtsLs.ScanDocType.SCAN_CARD, CtsLs.ScanDocType.SCAN_PAPER_DOCUMENT)), 0)
                        'If Reply <> Ls.LsReply.LS_OKAY Then
                        '    If ApplClass.CheckReply(Reply, "LSDocHandle") Then
                        '        ' Free of local variable

                        '        Marshal.FreeHGlobal(BufRearFile)
                        '        Marshal.FreeHGlobal(BufFrontFile)

                        '        Return Reply
                        '    End If
                        'End If



                        'MsgBox(rc & "- about to begin " & UniqueFilename & ImagePath & ConstNum)
                        'MsgBox(hLS & 0 & Stamp & PrintValidate & Codeline & cApplFunc.Side & cApplFunc.ScanMode & cApplFunc.Sorter & 0 & LsFamily.LsDefines.ClearBlack.CLEAR_BLACK_YES & cApplFunc.SaveMode & ImagePath & UniqueFilename & ConstNum & pos_x & pos_y & pos_w & pos_h & cApplFunc.FileFormat & 2 & LsFamily.LsDefines.FileAttribute.SAVE_REPLACE & 0 & LsFamily.LsDefines.Wait.WAIT_NO & cApplFunc.Beep & cb)
                        If Convert.ToInt16(txtChqCount.Text) > 0 Then
                            rc = Ls.LSAutoDocHandle(hLS, 0, Stamp, PrintValidate, Codeline, cApplFunc.Side, cApplFunc.ScanMode,
                                                    cApplFunc.Sorter, 0, LsFamily.LsDefines.ClearBlack.CLEAR_AND_ALIGN_IMAGE,
                                                    cApplFunc.SaveMode, ImagePath, UniqueFilename, ConstNum, pos_x, pos_y, pos_w, pos_h,
                                                    cApplFunc.FileFormat, 60, LsFamily.LsDefines.FileAttribute.SAVE_INSERT, 0,
                                                    LsFamily.LsDefines.Wait.WAIT_NO, cApplFunc.Beep, cb)
                            ''rc = Ls.LSDocHandle(hLS, 0, Stamp, PrintValidate, Codeline, cApplFunc.Side,
                            '                    cApplFunc.ScanMode, LsFamily.LsDefines.Feeder.FEED_AUTO,
                            '                    cApplFunc.Sorter, LsFamily.LsDefines.Wait.WAIT_YES,
                            '                    cApplFunc.Beep, NrDoc, LsFamily.LsDefines.ScanDocType.SCAN_PAPER_DOCUMENT)

                            ' Save nr. of items to be processed
                        End If



                        FrontImg = ""
                        BackImg = ""
                        UVImg = ""
                        MICRFrontImgPath = ""
                        MICRBackImgPath = ""
                        MICRUVImagePath = ""






                    Catch ex As Exception
                        MessageBox.Show(" MuiltiDoc " + ex.Message)
                    End Try
                    'DisplayImages((ImagePath & UniqueFilename & ConstNum & pos_x & pos_y & pos_w & pos_h & cApplFunc.FileFormat & 2 & LsFamily.LsDefines.FileAttribute.SAVE_REPLACE), 1)
                End If
                If (rc = LsFamily.LsReply.LS_OKAY) Then
                    NrDoc = 0
                    ReaderCounter = 0
                    'MessageBox.Show("Imeingia hapa 1. " + ReaderCounter.ToString + " - ChqCounter " + chqCounter.ToString)
                    Do
                        Dim UnitStatus As CtsLs.UNITSTATUS

                        UnitStatus = New CtsLs.UNITSTATUS()
                        UnitStatus.Size = Marshal.SizeOf(UnitStatus)
                        If chqCounter = ReaderCounter Then rc = 20 : rc = LsFamily.LsReply.LS_OKAY : Exit Do

                        rc = Ls.LSGetDocData(hLS, 0, NrDoc, fileFront, fileRear, fileFront2, Nothing, FrontImage, BackImage, FrontImage2, Nothing, CodelineSw, CodelineHw)
                        If rc = 1 Then rc = 20 : rc = LsFamily.LsReply.LS_OKAY : Exit Do
                        FrontImg = (ImagePath & "\" & UniqueFilename & pos_x & pos_y & pos_w & ReaderCounter & "FF" & ".JPG")
                        BackImg = (ImagePath & "\" & UniqueFilename & pos_x & pos_y & pos_w & ReaderCounter & "BB" & ".JPG")
                        UVImg = (ImagePath & "\" & UniqueFilename & pos_x & pos_y & pos_w & ReaderCounter & "FN" & ".JPG")
                        FrontBWImg = (ImagePath & "\" & UniqueFilename & pos_x & pos_y & pos_w & ReaderCounter & "FFBW" & ".JPG")
                        pictPreview1.ImageLocation = FrontImg
                        pictPreview1.Refresh()
                        pictPreview2.ImageLocation = UVImg
                        pictPreview2.Refresh()
                        BackGraySPic.ImageLocation = BackImg
                        BackGraySPic.Refresh()
                        CodeLineDetails.FrontImagePathGrayScale = FrontImg
                        CodeLineDetails.BackImagePath = BackImg
                        CodeLineDetails.UVImagePath = UVImg
                        CodeLineDetails.FrontImagePathBW = FrontBWImg


                        'Bytes2String(BackImage
                        'Bytes2String(FrontImage)

                        BackGraySPic.Image = BackImage
                        pictMainFront.Image = FrontImage
                        pictMainRear.Image = BackImage
                        pictMainFront.Refresh()
                        img = pictMainFront.Image

                        Try
                            'TenaRudia:
                            'MessageBox.Show("Image Conversion")
                            System.Windows.Forms.Application.DoEvents()
                            If img.PixelFormat <> PixelFormat.Format32bppPArgb Then
                                Dim temp As New Bitmap(img.Width, img.Height, PixelFormat.Format32bppPArgb)
                                Dim g As Graphics = Graphics.FromImage(temp)
                                g.DrawImage(img, New Rectangle(0, 0, img.Width, img.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel)
                                img.Dispose()
                                g.Dispose()
                                img = temp
                            End If
                            'lock the bits of the original bitmap
                            Dim bmdo As BitmapData = img.LockBits(New Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadOnly, img.PixelFormat)
                            'and the new 1bpp bitmap
                            bm = New Bitmap(pictMainFront.Image.Width, pictMainFront.Image.Height, PixelFormat.Format1bppIndexed)
                            bm.SetResolution(201, 201)
                            Dim bmdn As BitmapData = bm.LockBits(New Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.ReadWrite, PixelFormat.Format1bppIndexed)
                            Dim y As Integer
                            For y = 0 To img.Height - 1
                                Dim x As Integer
                                For x = 0 To img.Width - 1
                                    'generate the address of the colour pixel
                                    Dim index As Integer = y * bmdo.Stride + x * 4
                                    'check its brightness
                                    If Color.FromArgb(Marshal.ReadByte(bmdo.Scan0, index + 2), Marshal.ReadByte(bmdo.Scan0, index + 1), Marshal.ReadByte(bmdo.Scan0, index)).GetBrightness() > 0.5F Then
                                        SetIndexedPixel(x, y, bmdn, True) 'set it if its bright.
                                    End If
                                Next x
                            Next y
                            'tidy up
                            bm.UnlockBits(bmdn)
                            img.UnlockBits(bmdo)
                            'display the 1bpp image.
                            Me.FrontBWPic.Image = bm
                            bm.Save(FrontBWImg)
                        Catch ex As Exception
                            'If IsNothing(img) = False Then
                            '    GoTo TenaRudia
                            'End If
                            'MessageBox.Show(ex.Message)
                        End Try
                        Try

                            'MessageBox.Show("Imemaliza Image Conversion")
                            Windows.Forms.Application.DoEvents()
                            CodeLineDetails.FrontImageBW = Bytes2String(ConvertImages(FrontBWImg))
                            CodeLineDetails.UniqueNumber = UniqueFilename & pos_x & pos_y & pos_w & Val(NrDoc) - 1
                            If (cApplFunc.ScanMode = LsFamily.LsDefines.ScanMode.SCAN_MODE_256GR200_AND_UV Or
                                cApplFunc.ScanMode = LsFamily.LsDefines.ScanMode.SCAN_MODE_256GR300_AND_UV Or
                                cApplFunc.ScanMode = LsFamily.LsDefines.ScanMode.SCAN_MODE_256GR100_AND_UV) Then
                                If (FrontImage2.Size.IsEmpty = False) Then
                                    If (cApplFunc.ShowUvImage = True) Then

                                    End If
                                    '' '' '' '' ''Ls.LSMergeImageGrayAndUV(0, FrontImage, FrontImage2, 0, 0, ImageMerge)
                                    '' '' '' '' ''System.Windows.Forms.Application.DoEvents()
                                    '' '' '' '' ''    SaveImage(ImageMerge)
                                    '' '' '' '' ''System.Windows.Forms.Application.DoEvents()

                                End If
                            End If
                            'MessageBox.Show("kusafe Images")
                            JRImageSize = ISize
                            JFImageSize = ISize
                            CodeLineDetails.FrontImageGrayScaleSignature = Bytes2String(HashTheImage(ConvertImages(FrontImg)))
                            CodeLineDetails.JFSize = GenarateImageSize(ConvertImages(FrontImg))
                            ' Select ReaderUsed
                            '    Case 2
                            '        pictPreview2.Image = GetImages(ConvertImages(UVImg))
                            'End Select


                            CodeLineDetails.FrontImageBlackandWhiteSignature = Bytes2String(HashTheImage(ConvertImages(FrontBWImg)))
                            CodeLineDetails.TFSize = GenarateImageSize(ConvertImages(FrontBWImg))


                            CodeLineDetails.BackImageSignature = Bytes2String(HashTheImage(ConvertImages(BackImg)))
                            CodeLineDetails.JRSize = GenarateImageSize(ConvertImages(BackImg))

                            CodeLineDetails.FrontImageGrayScale = Bytes2String(ConvertImages(FrontImg))
                            CodeLineDetails.BackImageGrayScale = Bytes2String(ConvertImages(BackImg))
                            CodeLineDetails.UVimage = Bytes2String(ConvertImages(UVImg))


                            dpi = Image.FromFile(CodeLineDetails.FrontImagePathGrayScale)
                            CodeLineDetails.JFdpi = dpi.HorizontalResolution
                            dpi = Image.FromFile(CodeLineDetails.FrontImagePathBW)
                            CodeLineDetails.FTdpi = dpi.HorizontalResolution.ToString.Substring(0, 3)
                            dpi = Image.FromFile(CodeLineDetails.BackImagePath)
                            CodeLineDetails.JRdpi = dpi.HorizontalResolution

                            'pictPreview2.Refresh()
                            Windows.Forms.Application.DoEvents()
                            'Back Image Namba yake ni 88
                            ' '' '' '' '' '' ''If (cApplFunc.Side = LsFamily.LsDefines.Side.SIDE_ALL_IMAGE Or cApplFunc.Side = LsFamily.LsDefines.Side.SIDE_BACK_IMAGE) Then

                            ' '' '' '' '' '' ''    If (BackImage.Size.IsEmpty = False) Then
                            ' '' '' '' '' '' ''        pictMainRear.Image = BackImage

                            ' '' '' '' '' '' ''    End If
                            ' '' '' '' '' '' ''End If

                            '' '' '' '' '' '' ''Front Image Namba yake ni 70
                            ' '' '' '' '' '' ''If (cApplFunc.Side = LsFamily.LsDefines.Side.SIDE_ALL_IMAGE Or cApplFunc.Side = LsFamily.LsDefines.Side.SIDE_FRONT_IMAGE) Then

                            ' '' '' '' '' '' ''    If (FrontImage.Size.IsEmpty = False) Then
                            ' '' '' '' '' '' ''        pictMainFront.Image = FrontImage

                            ' '' '' '' '' '' ''    End If
                            ' '' '' '' '' '' ''End If

                            If (FrontImage2.Size.IsEmpty = False) Then
                                'pictPreview2.Image = ImageMerge
                                'pictPreview2.Image = GetImages(ConvertImages(UVImg)) 'GetImages(StringToByte(CodeLineDetails.UVimage))
                            End If
                            System.Windows.Forms.Application.DoEvents()
                            If (rc = LsFamily.LsReply.LS_OKAY Or rc = LsFamily.LsReply.LS_DOUBLE_LEAFING_WARNING Or
                                rc = LsFamily.LsReply.LS_SORTER1_FULL Or rc = LsFamily.LsReply.LS_SORTER2_FULL Or
                                ((rc >= LsFamily.LsReply.LS_SORTER_1_POCKET_1_FULL) And (rc <= LsFamily.LsReply.LS_SORTER_7_POCKET_3_FULL))) Then
                                Dim FontToRead As String = Nothing
                                pictMainFront.StretchImageToFit = True
                                pictMainRear.StretchImageToFit = True

                                If isMDV = True Then
                                    MICRCodeline = MDVMicr
                                Else
                                    'MessageBox.Show(MICRCodeline.ToString(), "ScreenShot this message, give to Kamunya", MessageBoxButtons.OK)
                                    MICRCodeline = Replace(CodelineHw, " ", "")
                                End If
                                ProcessCodeline(MICRCodeline)
                                System.Windows.Forms.Application.DoEvents()
                                PopulateMicrInfo()
                                System.Windows.Forms.Application.DoEvents()
                                timeEnd = Environment.TickCount()
                                DocPerMin += 1

                                If (LsUnitType = LsFamily.LsDefines.LsUnitType.LS_800_USB) Then

                                    ' Check the sorter full
                                    If (rc >= LsFamily.LsReply.LS_SORTER_1_POCKET_1_FULL) And (rc <= LsFamily.LsReply.LS_SORTER_7_POCKET_3_FULL) Then

                                        If (cApplFunc.Sorter_Ls800 = LsFamily.LsDefines.Sorter.SORTER_SEQUENTIAL) Then

                                            ' Stop on last pocket
                                            If CurrPocket = (NrTot_Pocket + LsFamily.LsDefines.Sorter.SORTER_POCKET_0_SELECTED) Then
                                                rc = Ls.LSStopAutoDocHandle(hLS, 0)
                                            Else
                                                ' increase the CurrPocket ONLY if equal to sorter full !
                                                If CurrPocket = rc Then
                                                    CurrPocket += 1
                                                End If
                                            End If

                                        Else
                                            ' In the other case stop the process
                                            Ls.LSStopAutoDocHandle(hLS, 0)
                                        End If
                                    End If
                                Else
                                    If cApplFunc.CodelineType = ApplClass.DECODE_OCR Then
                                        CodelineSw = New String(" ")

                                        Select Case cApplFunc.OCR_Type
                                            Case LsFamily.LsDefines.CodelineToRead.READ_CODELINE_SW_OCRA
                                                FontToRead = "A"
                                            Case LsFamily.LsDefines.CodelineToRead.READ_CODELINE_SW_OCRB_NUM
                                                FontToRead = "B"
                                            Case LsFamily.LsDefines.CodelineToRead.READ_CODELINE_SW_OCRB_ALFANUM
                                                FontToRead = "C"
                                            Case LsFamily.LsDefines.CodelineToRead.READ_CODELINE_SW_OCRB_ITALY
                                                FontToRead = "F"
                                            Case LsFamily.LsDefines.CodelineToRead.READ_CODELINE_SW_E13B
                                                FontToRead = "E"
                                        End Select

                                        len_codeline = 256
                                        Ls.LSCodelineReadFromBitmap(0, FrontImage, FontToRead, cApplFunc.OCR_Unit, cApplFunc.OCR_x, cApplFunc.OCR_y, cApplFunc.OCR_w, cApplFunc.OCR_h, 1, CodelineSw)

                                    ElseIf cApplFunc.CodelineType = ApplClass.DECODE_BARCODE Then
                                        'Codeline = cApplFunc.Barcode_Type

                                        CodelineSw = New String(" ")

                                        If (cApplFunc.Barcode_Type) Then
                                            Select Case cApplFunc.Barcode_Type
                                                Case LsFamily.LsDefines.CodelineToRead.READ_BARCODE_2_OF_5
                                                    BarcodeToread = 50 'LSAPI_READ_BARCODE_2_OF_5
                                                Case LsFamily.LsDefines.CodelineToRead.READ_BARCODE_CODE128
                                                    BarcodeToread = 52 'LSAPI_READ_BARCODE_CODE128'
                                                Case LsFamily.LsDefines.CodelineToRead.READ_BARCODE_CODE39
                                                    BarcodeToread = 51 'LSAPI_READ_BARCODE_CODE39'
                                                Case LsFamily.LsDefines.CodelineToRead.READ_BARCODE_EAN13
                                                    BarcodeToread = 53 'LSAPI_READ_BARCODE_EAN13' 
                                            End Select

                                        End If
                                        Ls.LSReadBarcodeFromBitmap(0, FrontImage, BarcodeToread, cApplFunc.Barcode_x, cApplFunc.Barcode_y, cApplFunc.Barcode_w, cApplFunc.Barcode_h, CodelineSw)
                                        'Ls.LSReadBarcodeFromBitmap();
                                    Else
                                    End If
                                End If



                                ' '' '' '' '' '' '' '' ''If cApplFunc.SaveMode = LsFamily.LsDefines.ImageSave.IMAGE_SAVE_BOTH Then
                                ' '' '' '' '' '' '' '' ''    If cApplFunc.Side = LsFamily.LsDefines.Side.SIDE_FRONT_IMAGE Or cApplFunc.Side = LsFamily.LsDefines.Side.SIDE_ALL_IMAGE Then
                                ' '' '' '' '' '' '' '' ''        Try
                                ' '' '' '' '' '' '' '' ''            'Ls.LSSaveImage(hLS, 0, ImageMerge, ImagePath, cApplFunc.FileFormat, cApplFunc.Quality, 0, 1, LsFamily.LsDefines.Side.SIDE_FRONT_UV)

                                ' '' '' '' '' '' '' '' ''            Ls.LSSaveImage(hLS, 0, FrontImage, ImagePath, cApplFunc.FileFormat, cApplFunc.Quality, 0, 1, LsFamily.LsDefines.Side.SIDE_FRONT_IMAGE)
                                ' '' '' '' '' '' '' '' ''        Catch ex As Exception

                                ' '' '' '' '' '' '' '' ''        End Try
                                ' '' '' '' '' '' '' '' ''        If (LsUnitType = LsFamily.LsDefines.LsUnitType.LS_150_USB) Then
                                ' '' '' '' '' '' '' '' ''            If (LsCfg.ScannerUltraViolet And (cApplFunc.ScanMode = LsFamily.LsDefines.ScanMode.SCAN_MODE_256GR100_AND_UV Or
                                ' '' '' '' '' '' '' '' ''                                              cApplFunc.ScanMode = LsFamily.LsDefines.ScanMode.SCAN_MODE_256GR200_AND_UV)) Then
                                ' '' '' '' '' '' '' '' ''                Ls.LSSaveImage(hLS, 0, FrontImage2, ImagePath, cApplFunc.FileFormat, cApplFunc.Quality, 0, 1, LsFamily.LsDefines.Side.SIDE_FRONT_UV)
                                ' '' '' '' '' '' '' '' ''            End If
                                ' '' '' '' '' '' '' '' ''        End If

                                ' '' '' '' '' '' '' '' ''    End If
                                ' '' '' '' '' '' '' '' ''    If cApplFunc.Side = LsFamily.LsDefines.Side.SIDE_BACK_IMAGE Or cApplFunc.Side = LsFamily.LsDefines.Side.SIDE_ALL_IMAGE Then

                                ' '' '' '' '' '' '' '' ''        Ls.LSSaveImage(hLS, 0, BackImage, ImagePath, cApplFunc.FileFormat, cApplFunc.Quality, 0, 1, LsFamily.LsDefines.Side.SIDE_BACK_IMAGE)
                                ' '' '' '' '' '' '' '' ''    End If
                                ' '' '' '' '' '' '' '' ''End If

                                'incremento il NrDoc per visualizzarlo a video
                                'la GetDocData dovrebbe tornarlo ???
                                NrDocVideo = NrDocVideo + 1
                                ' force Ok for repeat the GetDocData()
                                rc = LsFamily.LsReply.LS_OKAY
                            Else
                                If (rc <> LsFamily.LsReply.LS_FEEDER_EMPTY Or fFeederEmpty) Then
                                    'MessageBox.Show(cApplFunc.CheckReply(rc, "LSGetDocData"), TITLE_POPUP, MessageBoxButtons.OK, MessageBoxIcon.Error)
                                End If
                            End If
                            ReaderCounter = ReaderCounter + 1
                        Catch ex As Exception

                            'MsgBox(ex.Message)

                        End Try

                        'Wait(3)
                    Loop While ((rc = LsFamily.LsReply.LS_OKAY) And (ReaderCounter <= chqCounter) And (rc <> LsFamily.LsReply.LS_FEEDER_EMPTY Or fFeederEmpty))

                Else
                    MessageBox.Show(cApplFunc.CheckReply(rc, "LSAutoDocHandle"), TITLE_POPUP, MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If

                rc = Ls.LSDisconnect(hLS, 0)
            Else
                MessageBox.Show(cApplFunc.CheckReply(rc, "LSConnect"), TITLE_POPUP, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

            If DocPerMin Then
                DocPerMin = DocPerMin / ((timeEnd - timeStart) / 60000)
            End If
            'lNrDocProc.Text = "Doc. per Minute : " + DocPerMin.ToString

        Catch ex As Exception

            'MsgBox(ex.Message)

        End Try

    End Sub
    Public Shared Function Ls515CodelineRead(ByVal CodelineReadHW As String, ByVal NrDoc As Int32, ByRef Pocket As Int32, ByRef Font As LsFamily.LsDefines.PrintFont, ByRef StringToPrint As String)

        ' Pocket selection
        Pocket = GlobalModuleCTS.BROutwardForm.CurrPocket
        If GlobalModuleCTS.BROutwardForm.CurrPocket = LsFamily.LsDefines.Sorter.SORTER_POCKET_2 Then
            GlobalModuleCTS.BROutwardForm.CurrPocket = LsFamily.LsDefines.Sorter.SORTER_POCKET_1
        Else
            GlobalModuleCTS.BROutwardForm.CurrPocket = LsFamily.LsDefines.Sorter.SORTER_POCKET_2
        End If

        ' Font and print selection
        Select Case cApplFunc.PrintValidate
            Case 0
                Font = 0
                StringToPrint = Nothing

            Case 1
                If (cApplFunc.Print_High) Then
                    Font = LsFamily.LsDefines.PrintFont.PRINT_UP_FORMAT_NORMAL
                Else
                    Font = LsFamily.LsDefines.PrintFont.PRINT_FORMAT_NORMAL
                End If
                StringToPrint = String.Copy(cApplFunc.Endorse_str)

            Case 2
                If (cApplFunc.Print_High) Then
                    Font = LsFamily.LsDefines.PrintFont.PRINT_UP_FORMAT_BOLD
                Else
                    Font = LsFamily.LsDefines.PrintFont.PRINT_UP_FORMAT_BOLD
                End If
                StringToPrint = String.Copy(cApplFunc.Endorse_str)

            Case 3
                If (cApplFunc.Print_High) Then
                    Font = LsFamily.LsDefines.PrintFont.PRINT_UP_FORMAT_NORMAL_15_CHAR
                Else
                    Font = LsFamily.LsDefines.PrintFont.PRINT_FORMAT_NORMAL_15
                End If
                StringToPrint = String.Copy(cApplFunc.Endorse_str)

            Case Else
                Font = 0
                StringToPrint = Nothing
        End Select

        Return True
    End Function
    Public Shared Function Ls800CodelineRead(ByVal CodelineReadHW As String, ByVal NrDoc As Int32, ByRef Pocket As Int32, ByRef Font As LsFamily.LsDefines.PrintFont, ByRef StringToPrint As String)

        ' Pocket selection
        Pocket = GlobalModuleCTS.BROutwardForm.CurrPocket

        ' Check the pocket
        If (cApplFunc.Sorter_Ls800 = LsFamily.LsDefines.Sorter.SORTER_CIRCULAR) Then
            If GlobalModuleCTS.BROutwardForm.CurrPocket >= (NrTot_Pocket + LsFamily.LsDefines.Sorter.SORTER_POCKET_0_SELECTED) Then
                GlobalModuleCTS.BROutwardForm.CurrPocket = LsFamily.LsDefines.Sorter.SORTER_POCKET_1_SELECTED
            Else
                GlobalModuleCTS.BROutwardForm.CurrPocket += 1
            End If
        End If


        ' Font and print selection
        Select Case cApplFunc.PrintValidate
            Case 0
                Font = 0
                StringToPrint = Nothing

            Case 1
                If (cApplFunc.Print_High) Then
                    Font = LsFamily.LsDefines.PrintFont.PRINT_UP_FORMAT_NORMAL
                Else
                    Font = LsFamily.LsDefines.PrintFont.PRINT_FORMAT_NORMAL
                End If
                StringToPrint = String.Copy(cApplFunc.Endorse_str)

            Case 2
                If (cApplFunc.Print_High) Then
                    Font = LsFamily.LsDefines.PrintFont.PRINT_UP_FORMAT_BOLD
                Else
                    Font = LsFamily.LsDefines.PrintFont.PRINT_UP_FORMAT_BOLD
                End If
                StringToPrint = String.Copy(cApplFunc.Endorse_str)

            Case 3
                If (cApplFunc.Print_High) Then
                    Font = LsFamily.LsDefines.PrintFont.PRINT_UP_FORMAT_NORMAL_15_CHAR
                Else
                    Font = LsFamily.LsDefines.PrintFont.PRINT_FORMAT_NORMAL_15
                End If
                StringToPrint = String.Copy(cApplFunc.Endorse_str)

            Case Else
                Font = 0
                StringToPrint = Nothing
        End Select

        Return True
    End Function
    Public Shared Function Ls800ImageRead(ByVal hImage As Bitmap, ByVal CodelineReadHW As String, ByVal NrDoc As Int32, ByRef Pocket As Int32, ByRef Font As LsFamily.LsDefines.PrintFont, ByRef StringToPrint As String)

        ' Pocket selection
        Pocket = GlobalModuleCTS.BROutwardForm.CurrPocket

        ' Check the pocket
        If (cApplFunc.Sorter_Ls800 = LsFamily.LsDefines.Sorter.SORTER_CIRCULAR) Then
            If GlobalModuleCTS.BROutwardForm.CurrPocket >= (NrTot_Pocket + LsFamily.LsDefines.Sorter.SORTER_POCKET_0_SELECTED) Then
                GlobalModuleCTS.BROutwardForm.CurrPocket = LsFamily.LsDefines.Sorter.SORTER_POCKET_1_SELECTED
            Else
                GlobalModuleCTS.BROutwardForm.CurrPocket += 1
            End If
        End If


        ' Font and print selection
        Select Case cApplFunc.PrintValidate
            Case 0
                Font = 0
                StringToPrint = Nothing

            Case 1
                If (cApplFunc.Print_High) Then
                    Font = LsFamily.LsDefines.PrintFont.PRINT_UP_FORMAT_NORMAL
                Else
                    Font = LsFamily.LsDefines.PrintFont.PRINT_FORMAT_NORMAL
                End If
                StringToPrint = String.Copy(cApplFunc.Endorse_str)

            Case 2
                If (cApplFunc.Print_High) Then
                    Font = LsFamily.LsDefines.PrintFont.PRINT_UP_FORMAT_BOLD
                Else
                    Font = LsFamily.LsDefines.PrintFont.PRINT_UP_FORMAT_BOLD
                End If
                StringToPrint = String.Copy(cApplFunc.Endorse_str)

            Case 3
                If (cApplFunc.Print_High) Then
                    Font = LsFamily.LsDefines.PrintFont.PRINT_UP_FORMAT_NORMAL_15_CHAR
                Else
                    Font = LsFamily.LsDefines.PrintFont.PRINT_FORMAT_NORMAL_15
                End If
                StringToPrint = String.Copy(cApplFunc.Endorse_str)

            Case Else
                Font = 0
                StringToPrint = Nothing
        End Select

        Return True
    End Function
    Private Sub ShowCodelineAndImages(ByVal Side As Int16, ByVal ScanMode As Int16, ByVal rc As Int32, ByVal NrDoc As UInt32, ByVal CodelineHW As String, ByVal CodelineSW As String, ByVal FrontImage As Bitmap, ByVal BackImage As Bitmap, ByVal FrontImage2 As Bitmap, ByVal BackImage2 As Bitmap)

        Dim image As Drawing.Image = Nothing
        'Back Image Namba yake ni 88
        If (Side = LsFamily.LsDefines.Side.SIDE_ALL_IMAGE Or Side = LsFamily.LsDefines.Side.SIDE_BACK_IMAGE) Then

            If (BackImage.Size.IsEmpty = False) Then
                pictMainRear.Image = BackImage
            End If
        End If

        'Front Image Namba yake ni 70
        If (Side = LsFamily.LsDefines.Side.SIDE_ALL_IMAGE Or Side = LsFamily.LsDefines.Side.SIDE_FRONT_IMAGE) Then

            If (FrontImage.Size.IsEmpty = False) Then
                pictMainFront.Image = FrontImage
            End If
        End If

        'UV image if present
        If (ScanMode = LsFamily.LsDefines.ScanMode.SCAN_MODE_256GR200_AND_UV Or _
            ScanMode = LsFamily.LsDefines.ScanMode.SCAN_MODE_256GR300_AND_UV Or _
            ScanMode = LsFamily.LsDefines.ScanMode.SCAN_MODE_256GR100_AND_UV) Then
            If (FrontImage2.Size.IsEmpty = False) Then
                If (cApplFunc.ShowUvImage = True) Then
                End If
                Ls.LSMergeImageGrayAndUV(0, FrontImage, FrontImage2, 0, 0, ImageMerge)
                SaveImage(ImageMerge)
            End If
        End If


        'Here lets have our Craft Silicon (No Image) Image
        If (Side = LsFamily.LsDefines.Side.SIDE_NONE_IMAGE) Then

        End If
        System.Windows.Forms.Application.DoEvents()

    End Sub


    Private Sub txtBankID_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtBankID.KeyDown
        If e.KeyValue = Keys.Enter Then
            txtChqNo.Focus()
        End If
    End Sub

    Private Sub txtBankID_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBankID.LostFocus
        ExecuteData(GetModify("SP_GETBANKS", "ourbranchid", "001", "BankID", txtBankID.Text), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
        Try
            If publicDTbl.Rows.Count > 0 Then
                CodeLineDetails.BankName = publicDTbl.Rows(0)(3).ToString.Trim
                txtBankName.Text = CodeLineDetails.BankName
                CodeLineDetails.BankID = txtBankID.Text
                RejectedReason = "ok"
            Else
                RejectedReason = "Invalid Bank ID '" & txtBankID.Text & "'"
                CodeLineDetails.BankID = txtBankID.Text
            End If
        Catch ex As Exception

        End Try
        publicDTbl.Clear()
    End Sub

    Private Sub txtBranchID_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtBranchID.KeyDown
        If e.KeyValue = Keys.Enter Then
            btnOk.Enabled = True
            btnOk.Focus()
        End If
    End Sub

    Private Sub txtBranchID_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBranchID.LostFocus
        'Validate BranchID
        ExecuteData(GetModify("SP_GETbranches", "ourbranchid", "001", "bankid", txtBankID.Text, "branchid", txtBranchID.Text), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
        If publicDTbl.Rows.Count > 0 Then
            CodeLineDetails.BranchName = publicDTbl.Rows(0)(3).ToString.Trim
            CodeLineDetails.BranchID = txtBranchID.Text
            txtBranchName.Text = CodeLineDetails.BranchName
        Else
            RejectedReason = "Invalid BranchID '" & txtBankID.Text & "' for this BankID: '" & txtBranchID.Text & "'"
            CodeLineDetails.BranchID = txtBranchID.Text
        End If
        publicDTbl.Clear()
    End Sub

    Private Sub txtChqDigit_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtChqDigit.KeyDown
        If e.KeyValue = Keys.Enter Then
            txtBankID.Focus()
        End If
    End Sub

    Private Sub txtChqNo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtChqNo.TextChanged

    End Sub

    Private Sub txtVoucherCode_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtVoucherCode.KeyDown
        If e.KeyValue = Keys.Enter Then
            txtBranchID.Focus()
        End If
    End Sub

    Private Sub txtVoucherCode_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtVoucherCode.TextChanged

    End Sub

    Private Sub txtBranchID_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBranchID.TextChanged

    End Sub
    Public Function ProcessCodeline(ByVal micrLine As String)
        RejectedReason = ""
        Try

            micrLine = Replace(micrLine, ":", "")
            micrLine = Replace(micrLine, "<", "")
            micrLine = Replace(micrLine, "=", "")
            micrLine = micrLine.Replace("o", "")
            micrLine = micrLine.Replace("t", "")
            micrLine = micrLine.Replace("-", "")
            micrLine = micrLine.Replace("!", "")
            micrLine = micrLine.Replace(" ", "")
            micrLine = Replace(micrLine, ";", "")
            micrLine = micrLine.Trim

            Select Case CountryCode.ToUpper.Trim
                Case "UG"
                    micrLine = micrLine
                Case "SL"

                Case "TZ"
                    micrLine = micrLine
                Case "RD"

                Case "KE"
                    micrLine = Mid$(micrLine, 1, 24)
                Case "ET"

                Case "SA"

            End Select
            If Modscan.SysType <> Modscan.ENUM_SysType.BRNET Then
                ValidateMicrLenght(micrLine, True)
            Else
                Select Case CountryCode.ToUpper.Trim
                    Case "UG"
                        micrLine = micrLine
                        ValidateMicrLenghtNet(micrLine, True)
                    Case "SL"

                    Case "TZ"
                        micrLine = micrLine

                        If (micrLine.Length < 26) Then
                            RBTypeB.Checked = False
                            RBTypeC.Checked = True
                        Else
                            RBTypeB.Checked = True
                            RBTypeC.Checked = False
                        End If


                        If RBTypeB.Checked Then
                            ValidateMicrLenghtNet(micrLine, "TypeB")
                        Else
                            ValidateMicrLenghtNet(micrLine, "TypeC")
                        End If
                    Case "RD"

                    Case "KE"
                        micrLine = Mid$(micrLine, 1, 24)
                        ValidateMicrLenghtNet(micrLine, True)
                    Case "ET"

                    Case "SA"

                End Select
                'MessageBox.Show(micrLine)



            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Public Function SaveImage(ByVal myUVimage As Bitmap)
        Try
            Dim bmpImage As New Bitmap(1300, 550)
            Dim grpImage As System.Drawing.Graphics
            grpImage = Graphics.FromImage(bmpImage)
            pictPreview2.Visible = False
            pictPreview2.Image = myUVimage
            grpImage.DrawImage(pictPreview2.Image, 0, 0, 1300, 550)
            bmpImage.Save(UVImg, System.Drawing.Imaging.ImageFormat.Jpeg)
            'pictPreview2.Image = Nothing
            pictPreview2.Refresh()
            Windows.Forms.Application.DoEvents()
        Catch ex As Exception

        End Try
    End Function

    Private Sub bgwImageSaver_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bgwImageSaver.DoWork
        Ls.LSMergeImageGrayAndUV(0, FrontImgx, FrontImg2x, 0, 0, ImageMerge)
        SaveImage(ImageMerge)
    End Sub

    Public Sub New()
        MyBase.New()
        InitializeComponent()
    End Sub

    Private Sub dgvOutCreditMicr_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgvOutCreditMicr.KeyPress

        Select Case UCase(dgvOutCreditMicr.CurrentCell.OwningColumn.Index)
            Case "1"
                Dim c As Char = e.KeyChar
                Dim i As Integer = Asc(c) '--convert value into ascii
                '--0-9 first--'
                If (i >= 47 And i < 58) _
                Or (i = 43) Or (i = 45) Then '--for + and - keys
                Else
                    If i = 8 Then '--for space
                    Else
                        e.Handled = True
                    End If
                End If
        End Select
        picker.Value = Format(dgvOutCreditMicr.Rows(dgvOutCreditMicr.CurrentRow.Index).Cells(6).Value, "dd MMM yyyy")
        chDate = picker.Value
    End Sub

    Private Sub FrontBWPic_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FrontBWPic.Click
        pictMainFront.Image = FrontBWPic.Image
        pictMainFront.StretchImageToFit = True
    End Sub

    Private Sub BackGraySPic_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BackGraySPic.Click
    End Sub

    Private Sub pictMainFront_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pictMainFront.MouseWheel
        pictMainFront.StretchImageToFit = False
    End Sub

    Private Sub pictMainRear_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pictMainRear.MouseWheel
        pictMainRear.StretchImageToFit = False
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click, Button2.Click
        Try
            If dgvOutCreditMicr.Rows.Count > 0 Then
                If MessageBox.Show("This will delete The Selected Item completely, do you wish to continue?", Nothing, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
                    Exit Sub
                Else
                    If dgvOutCreditMicr.Rows(dgvOutCreditMicr.CurrentRow.Index).Cells("ColumnID").Value <> 0 Then
                        ExecuteData(GetModify("sp_DeleteT_BRChequeTruncation", "ourbranchid", OurBranchID, "ColumnID", Convert.ToInt32(dgvOutCreditMicr.Rows(dgvOutCreditMicr.CurrentRow.Index).Cells("ColumnID").Value), "OperatorID", OperatorID), publicDTbl, dataExecTypes.ExecTypeNonQuery, queryType.SelectStatement)
                    End If
                    dgvOutCreditMicr.Rows.RemoveAt(dgvOutCreditMicr.CurrentRow.Index)
                    ClearCTl()
                    txtChqCount.Text = Val(dgvOutCreditMicr.RowCount) + Val(dgvRejectedItem.RowCount)
                    txtChqCount.Enabled = False
                    Application.DoEvents()
                    Dim dgv As DataGridView = DirectCast(sender, DataGridView)
                    Dim total As Decimal
                    For Each r As DataGridViewRow In dgv.Rows
                        total += Replace(r.Cells(1).Value, ",", "")
                        r.Cells(1).Value = FormatNumber(r.Cells(1).Value, 2)
                    Next
                    lblAmount.Text = FormatNumber(total, 2)
                    lblCount.Text = dgv.RowCount
                End If
            End If
            txtChqCount.Text = Val(dgvOutCreditMicr.RowCount) + Val(dgvRejectedItem.RowCount)
            txtChqCount.Enabled = False
            picker.Value = Format(dgvOutCreditMicr.Rows(dgvOutCreditMicr.CurrentRow.Index).Cells(6).Value, "dd MMM yyyy")
            chDate = picker.Value
        Catch ex As Exception

        End Try
    End Sub

    Private Sub dgvOutCreditMicr_RowEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvOutCreditMicr.RowEnter

        'txtChqCount.Text = Val(dgvOutCreditMicr.RowCount) + Val(dgvRejectedItem.RowCount)
        'txtChqCount.Enabled = False
        'Dim i As Integer = e.rowIndex
        'ClearCTl()
        'EnableDisableCtl(4)
        'txtBankName.Text = ""
        'txtBranchName.Text = ""
        'txtBankID.Text = dgvOutCreditMicr.Rows(i).Cells(3).Value.ToString.Substring(0, 2)
        'txtBranchID.Text = dgvOutCreditMicr.Rows(i).Cells(4).Value.ToString.Substring(0, 3)
        'txtChqDigit.Text = dgvOutCreditMicr.Rows(i).Cells(10).Value
        'txtChqNo.Text = dgvOutCreditMicr.Rows(i).Cells(0).Value
        'txtTheirAccID.Text = dgvOutCreditMicr.Rows(i).Cells(5).Value
        'txtVoucherCode.Text = dgvOutCreditMicr.Rows(i).Cells(15).Value
        'If txtBankName.Text = "" Then
        '    txtBankName.Text = dgvOutCreditMicr.Rows(i).Cells(3).Value.ToString.Substring(3)
        'End If

        'If txtBranchName.Text = "" Then
        '    txtBranchName.Text = dgvOutCreditMicr.Rows(i).Cells(4).Value.ToString.Substring(4)
        'End If
        'txtClrCenter.Text = dgvOutCreditMicr.Rows(i).Cells(14).Value

        'If ReaderUsed = ReaderType.Panini Then
        '    pictMainFront.Image = GetImages(dgvOutCreditMicr.Rows(i).Cells(11).Value)
        '    pictPreview1.Image = GetImages(dgvOutCreditMicr.Rows(i).Cells(11).Value)
        '    FrontBWPic.Image = GetImages(dgvOutCreditMicr.Rows(i).Cells("TFImage").Value)
        '    pictMainRear.Image = GetImages(dgvOutCreditMicr.Rows(i).Cells(12).Value)
        '    BackGraySPic.Image = GetImages(dgvOutCreditMicr.Rows(i).Cells(12).Value)
        'Else
        '    pictMainFront.Image = GetImages(dgvOutCreditMicr.Rows(i).Cells(11).Value)
        '    pictPreview1.Image = GetImages(dgvOutCreditMicr.Rows(i).Cells(11).Value)
        '    FrontBWPic.Image = GetImages(dgvOutCreditMicr.Rows(i).Cells("TFImage").Value)
        '    pictMainRear.Image = GetImages(dgvOutCreditMicr.Rows(i).Cells(12).Value)
        '    BackGraySPic.Image = GetImages(dgvOutCreditMicr.Rows(i).Cells(12).Value)
        '    pictPreview2.Image = GetImages(dgvOutCreditMicr.Rows(i).Cells(13).Value)
        'End If
        'pictMainFront.StretchImageToFit = True
        'pictMainRear.StretchImageToFit = True
    End Sub

    Private Sub dgvOutCreditMicr_RowPostPaint(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowPostPaintEventArgs) Handles dgvOutCreditMicr.RowPostPaint

    End Sub

    Private Sub dgvOutCreditMicr_RowsRemoved(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsRemovedEventArgs) Handles dgvOutCreditMicr.RowsRemoved

    End Sub

    Private Sub dgvOutCreditMicr_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvOutCreditMicr.SelectionChanged
        'txtChqCount.Text = Val(dgvOutCreditMicr.RowCount) + Val(dgvRejectedItem.RowCount)
        'txtChqCount.Enabled = False

        'ClearCTl()
        'EnableDisableCtl(4)
        'If dgvOutCreditMicr.RowCount > 0 Then
        '    txtBankName.Text = ""
        '    txtBranchName.Text = ""
        '    txtBankID.Text = dgvOutCreditMicr.Rows(0).Cells(3).Value.ToString.Substring(0, 2)
        '    txtBranchID.Text = dgvOutCreditMicr.Rows(0).Cells(4).Value.ToString.Substring(0, 3)
        '    txtChqDigit.Text = dgvOutCreditMicr.Rows(0).Cells(10).Value
        '    txtChqNo.Text = dgvOutCreditMicr.Rows(0).Cells(0).Value
        '    txtTheirAccID.Text = dgvOutCreditMicr.Rows(0).Cells(5).Value
        '    txtVoucherCode.Text = dgvOutCreditMicr.Rows(0).Cells(15).Value
        '    If txtBankName.Text = "" Then
        '        txtBankName.Text = dgvOutCreditMicr.Rows(0).Cells(3).Value.ToString.Substring(3)
        '    End If

        '    If txtBranchName.Text = "" Then
        '        txtBranchName.Text = dgvOutCreditMicr.Rows(0).Cells(4).Value.ToString.Substring(4)
        '    End If
        '    txtClrCenter.Text = dgvOutCreditMicr.Rows(0).Cells(14).Value

        '    If ReaderUsed = ReaderType.Panini Then
        '        pictMainFront.Image = GetImages(dgvOutCreditMicr.Rows(0).Cells(11).Value)
        '        pictPreview1.Image = GetImages(dgvOutCreditMicr.Rows(0).Cells(11).Value)
        '        FrontBWPic.Image = GetImages(dgvOutCreditMicr.Rows(0).Cells("TFImage").Value)
        '        pictMainRear.Image = GetImages(dgvOutCreditMicr.Rows(0).Cells(12).Value)
        '        BackGraySPic.Image = GetImages(dgvOutCreditMicr.Rows(0).Cells(12).Value)
        '    Else
        '        pictMainFront.Image = GetImages(dgvOutCreditMicr.Rows(0).Cells(11).Value)
        '        pictPreview1.Image = GetImages(dgvOutCreditMicr.Rows(0).Cells(11).Value)
        '        FrontBWPic.Image = GetImages(dgvOutCreditMicr.Rows(0).Cells("TFImage").Value)
        '        pictMainRear.Image = GetImages(dgvOutCreditMicr.Rows(0).Cells(12).Value)
        '        BackGraySPic.Image = GetImages(dgvOutCreditMicr.Rows(0).Cells(12).Value)
        '        pictPreview2.Image = GetImages(dgvOutCreditMicr.Rows(0).Cells(13).Value)
        '    End If
        '    pictMainFront.StretchImageToFit = True
        '    pictMainRear.StretchImageToFit = True
        'End If
    End Sub

    Private Sub pictMainRear_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pictMainRear.Load

    End Sub

    Private Sub textState_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles textState.TextChanged

    End Sub

    Private Sub btnMDV_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMDV.Click
        CountryCode = ConfigurationManager.AppSettings("CountryCode")
        Select Case CountryCode.ToUpper.Trim
            Case "UG"
                Dim frm As New frmMDVUG
                frm.TopMost = True
                frm.ShowDialog()
            Case "SL"
                Dim frm As New frmMDV
                frm.TopMost = True
                frm.ShowDialog()
            Case "TZ"
                Dim frm As New frmMDVTZ
                'frm.TopMost = True
                frm.ShowDialog()
            Case "RD"
                Dim frm As New frmMDV
                frm.TopMost = True
                frm.ShowDialog()
            Case "KE"
                txtChqCount.Text = "1"
                Me.TopMost = False
                Dim frm As New frmMDV
                frm.TopMost = True
                frm.ShowDialog()
            Case "ET"
                Dim frm As New frmMDV
                frm.TopMost = True
                frm.ShowDialog()
            Case "SA"
                Dim frm As New frmMDV
                frm.TopMost = True
                frm.ShowDialog()
        End Select
        'Me.TopMost = True
        Try
            Select Case ReaderUsed
                Case ReaderType.Panini
                    'chqCounter = Me.txtChqCount.Text
                    'chqCaunt = 0
                    'startReading()
                Case ReaderType.CTS
                    If isMDV = True Then
                        chqCounter = Me.txtChqCount.Text
                        chqCaunt = 0
                    Else
                        chqCounter = Me.txtChqCount.Text
                        chqCaunt = 0
                    End If
                    MultiDocHandle(True)
            End Select
            ReaderCounter = 0
        Catch ex As Exception
            'myMVX.ListEvents(ex.Message.ToString & " - " & ex.TargetSite.ToString & " - " & ex.InnerException.ToString)
        End Try
    End Sub
    Public Sub SetMicr(ByVal strMicr As String)
        Dim FrontByte() As Byte
        Dim BWByte() As Byte
        Dim BackByte() As Byte
        Try
            ProcessCodeline(strMicr)
            Dim img As Bitmap
            Dim bm As Bitmap
            FrontByte = ImageToByte(pictMainFront.Image)
            CodeLineDetails.FrontImageGrayScale = Bytes2String(FrontByte)
            CodeLineDetails.JFdpi = pictMainFront.Image.HorizontalResolution

            BackByte = ImageToByte(pictMainRear.Image)
            CodeLineDetails.BackImageGrayScale = Bytes2String(BackByte)
            CodeLineDetails.JRdpi = pictMainRear.Image.HorizontalResolution

            img = pictMainFront.Image
            System.Windows.Forms.Application.DoEvents()
            If img.PixelFormat <> PixelFormat.Format32bppPArgb Then
                Dim temp As New Bitmap(img.Width, img.Height, PixelFormat.Format32bppPArgb)
                Dim g As Graphics = Graphics.FromImage(temp)
                g.DrawImage(img, New Rectangle(0, 0, img.Width, img.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel)
                img.Dispose()
                g.Dispose()
                img = temp
            End If
            'lock the bits of the original bitmap
            Dim bmdo As BitmapData = img.LockBits(New Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadOnly, img.PixelFormat)
            bm = New Bitmap(pictMainFront.Image.Width, pictMainFront.Image.Height, PixelFormat.Format1bppIndexed)
            bm.SetResolution(201, 201)
            Dim bmdn As BitmapData = bm.LockBits(New Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.ReadWrite, PixelFormat.Format1bppIndexed)
            Dim y As Integer
            For y = 0 To img.Height - 1
                Dim x As Integer
                For x = 0 To img.Width - 1
                    Dim index As Integer = y * bmdo.Stride + x * 4
                    If Color.FromArgb(Marshal.ReadByte(bmdo.Scan0, index + 2), Marshal.ReadByte(bmdo.Scan0, index + 1), Marshal.ReadByte(bmdo.Scan0, index)).GetBrightness() > 0.5F Then
                        SetIndexedPixel(x, y, bmdn, True)
                    End If
                Next x
            Next y
            bm.UnlockBits(bmdn)
            img.UnlockBits(bmdo)
            Me.FrontBWPic.Image = bm
            Windows.Forms.Application.DoEvents()
            BWByte = ImageToByte(bm)
            CodeLineDetails.FrontImageBW = Bytes2String(BWByte)
            CodeLineDetails.FTdpi = FrontBWPic.Image.HorizontalResolution
            PopulateMicrInfo()

        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'FrontByte = Nothing
            'BWByte = Nothing
            'BackByte = Nothing
        End Try





    End Sub
    Private Function ImageToByte(ByVal img As Image) As Byte()
        Try
            Dim imgStream As MemoryStream = New MemoryStream()
            img.Save(imgStream, System.Drawing.Imaging.ImageFormat.Jpeg)
            imgStream.Close()
            Dim byteArray As Byte() = imgStream.ToArray()
            imgStream.Dispose()
            Return byteArray
        Catch ex As Exception

        End Try

    End Function
    Public Sub SetFrontImage(ByVal Image As System.Drawing.Image)
        Try
            SyncLock pictMainFront


                If Not (Me.pictMainFront.Image Is Nothing) Then
                    Me.pictMainFront.Image.Dispose()
                End If
                Me.pictMainFront.Image = Image
                Me.pictMainFront.fittoscreen()
            End SyncLock
        Catch ex As Exception

        End Try


    End Sub

    Public Sub SetBackImage(ByVal Image As System.Drawing.Image)
        Try
            SyncLock pictMainRear
                If Not (Me.pictMainRear.Image Is Nothing) Then
                    Me.pictMainRear.Image.Dispose()
                End If
                Me.pictMainRear.Image = Image

                Me.pictMainRear.fittoscreen()
            End SyncLock
        Catch ex As Exception

        End Try


    End Sub
    ' This delegate enables asynchronous calls for setting.
    Delegate Sub ScanComplateCallback(ByVal lastTransactionNumber As Integer)
    'Public Sub ScanComplete(ByVal lastTransactionNumber As Integer)
    '    Try
    '        If (Me.InvokeRequired) Then
    '            Dim c As ScanComplateCallback = New ScanComplateCallback(AddressOf ScanComplete)
    '            Me.Invoke(c, New Object() {lastTransactionNumber})
    '        Else
    '            Dim rate As Double = 0.0
    '            Dim Count As Integer = lastTransactionNumber - m_iTransactionNumber + 1
    '            Dim spanTime As TimeSpan = Date.Now.Subtract(m_dataScanStartTime)

    '            spanTime = spanTime.Subtract(m_objApi.GetStartWaitTime)
    '            rate = Count / spanTime.TotalMinutes
    '            'Me.Text = "TM-S1000SampleStep2 - Scan Complete(" + Count.ToString + " @ " + rate.ToString("G4") + " DPM )"
    '            Button3.Enabled = True
    '        End If
    '    Catch ex As Exception

    '    End Try

    'End Sub
    ' this method is called when the confirmation mode
    'Public Function Confirmation() As Boolean
    '    If m_objApi.GetErrorOccured() = "SUCCESS" Then
    '        m_objConfig(Properties.CONF_EJECT) = 0
    '        m_objConfig(Properties.CONF_STAMP) = False
    '        m_objConfig(Properties.CONF_NEXT_CHECK) = 0
    '        m_objConfig(Properties.CONF_OK) = True
    '        m_objApi.SetProc(m_objConfig)
    '    Else
    '        m_objConfig(Properties.CONF_EJECT) = 1
    '        m_objConfig(Properties.CONF_STAMP) = False
    '        m_objConfig(Properties.CONF_NEXT_CHECK) = 0
    '        m_objConfig(Properties.CONF_OK) = True
    '        m_objApi.SetProc(m_objConfig)
    '    End If
    '    Return m_objConfig(Properties.CONF_OK)
    'End Function

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        If MessageBox.Show("You will lose any unsaved data, do you wish to continue?", Modscan.MsgBoxTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
            Exit Sub
        Else
            Me.Dispose()
            GC.SuppressFinalize(Me)
        End If
    End Sub


    Private Function DoSingleDocHandle(ByVal hConnect As Short, ByVal UnitCfg As Byte()) As Integer
        Dim Reply As Integer
        '	string	FileOut;

        Dim BufFrontFile As IntPtr
        Dim BufRearFile As IntPtr
        Dim BufFrontImage As IntPtr
        Dim BufRearImage As IntPtr
        Dim BufFrontUVImage As IntPtr
        Dim BufFrontGrayUVImage As IntPtr
        Dim NoImage As IntPtr
        Dim BufCodelineSW As IntPtr
        Dim BufCodelineHW As IntPtr
        Dim BufBarcode As IntPtr

        Dim NrDoc As UInt32
        Dim PrintValidate As CtsLs.PrintValidate

        ' COORDINATE for BARCODE OR FOR OCR
        Dim lenCodeline As Short
        Dim NotUsed As Short, NotUsed2 As Short




        PrintValidate = CtsLs.PrintValidate.NO_PRINT_VALIDATE
        '-----------LoadString--------------------------------------
        If stParAppl.PrintValidate <> CtsLs.PrintFont.PRINT_NO_STRING Then
            Dim strEndorse As IntPtr = Marshal.AllocHGlobal(160)



            ' Copy the Secure string to unmanaged memory (and decrypt it).
            strEndorse = Marshal.StringToHGlobalAnsi(stParAppl.Endorse_str)

            If stParAppl.Endorse_str.Contains("%d") Then
                Reply = CtsLs.LSLoadStringWithCounterEx(hConnect, 0, CShort(stParAppl.PrintValidate), strEndorse, CShort(stParAppl.Endorse_str.Length), 8, _
                 3)
            Else
                Reply = CtsLs.LSLoadString(hConnect, 0, CShort(stParAppl.PrintValidate), CShort(stParAppl.Endorse_str.Length), strEndorse)
            End If
            If Reply <> CtsLs.LsReply.LS_OKAY Then
                If CheckReply(Reply, "LSLoadString") Then
                    Return Reply
                End If
            End If

            ' Set variable for print
            PrintValidate = CtsLs.PrintValidate.PRINT_VALIDATE
            'PrintValidate = CtsLs.PrintValidate.PRINT_VALIDATE;
            'Reply = CtsLs.LSLoadString(hConnect, 0, PRINT_FORMAT_HEAD_TEST, 1, " ");

            'if (Reply != CtsLs.LsReply.LS_OKAY)
            '{
            '    if (CheckReply(Reply, "LSLoadString"))
            '    {
            '        return Reply;
            '    }
            '}

        ElseIf stParAppl.PrintHighDefinition Then
            '-----------Print Logo--------------------------------------
        ElseIf stParAppl.PrintLogo Then
            PrintValidate = CtsLs.PrintValidate.PRINT_LOGO
        End If


        '-----------Load Digital String----------------------------------
        'if (stParAppl.PrintValidate == CtsLs.PrintValidate.PRINT_DIGITAL_VALIDATE)
        '{
        '    switch( stParAppl.Digital_SidePrint )
        '    {
        '    case 1:
        '        SidePrint = SIDE_BACK_IMAGE;
        '        break;
        '    case 2:
        '        SidePrint = SIDE_FRONT_IMAGE;
        '        break;
        '    case 3:
        '        SidePrint = SIDE_ALL_IMAGE;
        '        break;
        '    }

        '    Reply = LSLoadDigitalStringWithCounter(hConnect, hWnd,
        '                                            SidePrint,
        '                                            stParAppl.Digital_str,
        '                                            (short)strlen(stParAppl.Digital_str),
        '                                            7, //StartNumber,
        '                                            0, //Step,
        '                                            stParAppl.Digital_FontName,
        '                                            stParAppl.Digital_FontSize,
        '                                            stParAppl.Digital_Bold,
        '                                            stParAppl.Digital_Italic,
        '                                            stParAppl.Digital_Undeline,
        '                                            stParAppl.Digital_Tone,
        '                                            stParAppl.Digital_Unit,
        '                                            stParAppl.Digital_x,
        '                                            stParAppl.Digital_y);

        '    // Set variable for print
        '    PrintValidate += CtsLs.PrintValidate.PRINT_DIGITAL_VALIDATE;
        '}


        '----------- Set double leafing sensitivity -----------
        Reply = CtsLs.LSConfigDoubleLeafingAndDocLength(hConnect, 0, stParAppl.DL_Type, stParAppl.DL_Value, stParAppl.DL_MinDoc, stParAppl.DL_MaxDoc)
        If Reply <> CtsLs.LsReply.LS_OKAY Then
            If CheckReply(Reply, "LSConfigDoubleLeafingAndDocLength") Then
                Return Reply
            End If
        End If



        '----------- wait for doc insertion -----------
        Reply = CtsLs.LSDisableWaitDocument(hConnect, 0, stParAppl.WaitTimeout)
        If Reply <> CtsLs.LsReply.LS_OKAY Then
            If CheckReply(Reply, "LSDisableWaitDocument") Then
                Return Reply
            End If
        End If


        '----------- Set speed document -----------
        Reply = CtsLs.LSSetUnitSpeed(hConnect, 0, stParAppl.LowSpeed)
        If Reply <> CtsLs.LsReply.LS_OKAY Then
            If CheckReply(Reply, "LSSetSpeedUnit") Then
                Return Reply
            End If
        End If


        '----------- Set Light Intensity --------------------------
        Reply = CtsLs.LSSetLightIntensity(hConnect, 0, stParAppl.LightIntensity)
        If Reply <> CtsLs.LsReply.LS_OKAY Then
            If CheckReply(Reply, "LSSetLightIntensity") Then
                Return Reply
            End If
        End If


        '----------- Only for Ultra Violet type -----------
        If (UnitCfg(1) And MASK_SCANNER_UV) = MASK_SCANNER_UV Then
            Reply = CtsLs.LSModifyPWMUltraViolet(hConnect, 0, stParAppl.PercentPWM_UV, stParAppl.Contrast_UV, stParAppl.Threshold_UV)
            If Reply <> CtsLs.LsReply.LS_OKAY Then
                If CheckReply(Reply, "LSModifyPWMUltraViolet") Then
                    Return Reply
                End If
            End If
        End If

        '----------- Only for BW -----------
        If (stParAppl.ScanMode = CtsLs.ScanMode.SCAN_MODE_BW) Then

            Dim m As Short = stParAppl.BWmethod
            Dim t As Short = stParAppl.BWthreshold

            Reply = CtsLs.LSSetBinarizationParameters(hConnect, 0, m, t, 0)
            If (Reply <> CtsLs.LsReply.LS_OKAY) Then
                If CheckReply(Reply, "LSSetBinarizationParameters") Then
                    Return Reply
                End If
            End If
        End If


        BufFrontFile = Marshal.AllocHGlobal(1024)
        BufRearFile = Marshal.AllocHGlobal(1024)
        BufCodelineSW = Marshal.AllocHGlobal(CInt(CtsLs.CodeLineType.MAX_CODE_LINE_LENGTH))
        BufCodelineHW = Marshal.AllocHGlobal(CInt(CtsLs.CodeLineType.MAX_CODE_LINE_LENGTH))
        BufBarcode = Marshal.AllocHGlobal(CInt(CtsLs.CodeLineType.MAX_CODE_LINE_LENGTH))

        NrDoc = 0
        ' Start Doc.
        Reply = CtsLs.LSDocHandle(hConnect, 0, stParAppl.FrontStamp, CShort(PrintValidate), CShort(stParAppl.CodelineMICR), stParAppl.Side, _
         stParAppl.ScanMode, CShort(CtsLs.Feeder.FEED_AUTO), CShort(CtsLs.Sorter.SORTER_POCKET_1), CShort(If(stParAppl.WaitTimeout, CtsLs.Wait.WAIT_YES, CtsLs.Wait.WAIT_NO)), CShort(stParAppl.BeepOnError), NrDoc, _
         CShort(If(stParAppl.ScanCard, CtsLs.ScanDocType.SCAN_CARD, CtsLs.ScanDocType.SCAN_PAPER_DOCUMENT)), 0)
        If Reply <> CtsLs.LsReply.LS_OKAY Then
            If CheckReply(Reply, "LSDocHandle") Then
                ' Free of local variable
                Marshal.FreeHGlobal(BufBarcode)
                Marshal.FreeHGlobal(BufCodelineHW)
                Marshal.FreeHGlobal(BufCodelineSW)
                Marshal.FreeHGlobal(BufRearFile)
                Marshal.FreeHGlobal(BufFrontFile)

                Return Reply
            End If
        End If


        ' Zero the vars
        If Save_FrontImage <> IntPtr.Zero Then
            CtsLs.LSFreeImage(0, Save_FrontImage)
            Save_FrontImage = IntPtr.Zero
        End If
        If Save_RearImage <> IntPtr.Zero Then
            CtsLs.LSFreeImage(0, Save_RearImage)
            Save_RearImage = IntPtr.Zero
        End If

        '-----------GetDocData--------------------------------------
        Dim pBitmap As Bitmap = Nothing

        Marshal.WriteByte(BufFrontFile, 0)
        Marshal.WriteByte(BufRearFile, 0)
        BufFrontImage = IntPtr.Zero
        BufRearImage = IntPtr.Zero
        BufFrontUVImage = BufFrontGrayUVImage = IntPtr.Zero
        NoImage = IntPtr.Zero

        Marshal.WriteByte(BufCodelineSW, 0)
        Marshal.WriteByte(BufCodelineHW, 0)
        Marshal.WriteByte(BufBarcode, 0)

        lenCodeline = CShort(CtsLs.CodeLineType.MAX_CODE_LINE_LENGTH)
        NotUsed = NotUsed2 = 0

        ' Read the Codeline MICR
        Reply = CtsLs.LSReadCodeline(hConnect, 0, BufCodelineHW, lenCodeline, IntPtr.Zero, NotUsed, _
         IntPtr.Zero, NotUsed2)

        If Reply <> CtsLs.LsReply.LS_OKAY Then
            If CheckReply(Reply, "LSReadCodeline") Then
                ' Free of local variable
                Marshal.FreeHGlobal(BufBarcode)
                Marshal.FreeHGlobal(BufCodelineHW)
                Marshal.FreeHGlobal(BufCodelineSW)
                Marshal.FreeHGlobal(BufRearFile)
                Marshal.FreeHGlobal(BufFrontFile)

                Return Reply
            End If
        End If

        ' Read the Images
        Reply = CtsLs.LSReadImage(hConnect, 0, CShort(CtsLs.ClearBlack.CLEAR_AND_ALIGN_IMAGE), stParAppl.Side, 0, NrDoc, _
         BufFrontImage, BufRearImage, BufFrontUVImage, IntPtr.Zero)

        If Reply <> CtsLs.LsReply.LS_OKAY Then
            If CheckReply(Reply, "LSReadImage") Then
                ' Free of local variable
                Marshal.FreeHGlobal(BufBarcode)
                Marshal.FreeHGlobal(BufCodelineHW)
                Marshal.FreeHGlobal(BufCodelineSW)
                Marshal.FreeHGlobal(BufRearFile)
                Marshal.FreeHGlobal(BufFrontFile)

                Return Reply
            End If
        End If


        'other codelines ?
        '' '' ''If (stParAppl.TypeOfDecod And DECODE_OCR) = DECODE_OCR Then
        '' '' ''    Dim ro As CtsLs.READOPTIONS
        '' '' ''    Dim CodelineOpt As Byte() = New Byte(3) {}
        '' '' ''    Dim len_codeline As Integer

        '' '' ''    ro.PutBlanks = 1
        '' '' ''    ro.TypeRead = "N"c
        '' '' ''    If stParAppl.CodelineOCR = CtsLs.CodeLineType.READ_CODELINE_SW_E13B_X_OCRB Then
        '' '' ''        ro.TypeRead = "X"c
        '' '' ''        CodelineOpt(0) = CByte(CtsLs.CodeLineType.READ_CODELINE_SW_E13B)
        '' '' ''        CodelineOpt(1) = CByte(CtsLs.CodeLineType.READ_CODELINE_SW_OCRB_ITALY)
        '' '' ''        CodelineOpt(2) = CByte(AscW(ControlChars.NullChar))
        '' '' ''    Else
        '' '' ''        CodelineOpt(0) = CByte(stParAppl.CodelineOCR)
        '' '' ''        CodelineOpt(2) = CByte(AscW(ControlChars.NullChar))
        '' '' ''    End If

        '' '' ''    len_codeline = CInt(CtsLs.CodeLineType.MAX_CODE_LINE_LENGTH)

        '' '' ''    Reply = CtsLs.LSCodelineReadFromBitmap(0, BufFrontImage, CodelineOpt, stParAppl.Unit_measure, stParAppl.Codeline_Sw_x, stParAppl.Codeline_Sw_y, _
        '' '' ''     stParAppl.Codeline_Sw_w, stParAppl.Codeline_Sw_h, ro, BufCodelineSW, len_codeline)
        '' '' ''    If Reply <> CtsLs.LsReply.LS_OKAY Then
        '' '' ''        CheckReply(Reply, "LSCodelineReadFromBitmap")
        '' '' ''        ' Set Ok for not exit from the loop
        '' '' ''        Reply = CtsLs.LsReply.LS_OKAY
        '' '' ''    End If
        '' '' ''End If
        '' '' ''If (stParAppl.TypeOfDecod And DECODE_BARCODE) = DECODE_BARCODE Then
        '' '' ''    Dim len_barcode As Integer = CInt(CtsLs.CodeLineType.MAX_CODE_LINE_LENGTH)

        '' '' ''    Reply = CtsLs.LSReadBarcodeFromBitmap(0, BufFrontImage, CByte(stParAppl.BarcodeType), CInt(stParAppl.Barcode_Sw_x), CInt(stParAppl.Barcode_Sw_y), CInt(stParAppl.Barcode_Sw_w), _
        '' '' ''     CInt(stParAppl.Barcode_Sw_h), BufBarcode, len_barcode)
        '' '' ''    If Reply <> CtsLs.LsReply.LS_OKAY Then
        '' '' ''        CheckReply(Reply, "LSReadBarcodeFromBitmap")
        '' '' ''        ' Set Ok for not exit from the loop
        '' '' ''        Reply = CtsLs.LsReply.LS_OKAY
        '' '' ''    End If
        '' '' ''End If
        '' '' ''If (stParAppl.TypeOfDecod And DECODE_PDF417) = DECODE_PDF417 Then
        '' '' ''    Dim len_barcode As Integer = CInt(CtsLs.CodeLineType.MAX_CODE_LINE_LENGTH)

        '' '' ''    Reply = CtsLs.LSReadPdf417FromBitmap(0, BufFrontImage, BufBarcode, len_barcode, 0, 0, _
        '' '' ''     0, 0, 0)
        '' '' ''    If Reply <> CtsLs.LsReply.LS_OKAY Then
        '' '' ''        CheckReply(Reply, "LSReadPdf417FromBitmap")
        '' '' ''        ' Set Ok for not exit from the loop
        '' '' ''        Reply = CtsLs.LsReply.LS_OKAY
        '' '' ''    End If
        '' '' ''End If


        If stParAppl.ScanMode = CShort(CtsLs.ScanMode.SCAN_MODE_256GR100_AND_UV) OrElse stParAppl.ScanMode = CShort(CtsLs.ScanMode.SCAN_MODE_256GR200_AND_UV) OrElse stParAppl.ScanMode = CShort(CtsLs.ScanMode.SCAN_MODE_256GR300_AND_UV) Then
            ' Build the mergered gray UV image
            If BufFrontUVImage <> IntPtr.Zero Then
                CtsLs.LSMergeImageGrayAndUV(0, BufFrontImage, BufFrontUVImage, 0, 0, BufFrontGrayUVImage)
            End If
        End If


        ' Show Codeline
        If Marshal.ReadByte(BufCodelineHW) <> 0 Then
            CodeLineDetails.CodelineWithCharacters = Marshal.PtrToStringAnsi(BufCodelineHW)
        ElseIf Marshal.ReadByte(BufCodelineSW) <> 0 Then
            CodeLineDetails.CodelineWithCharacters = Marshal.PtrToStringAnsi(BufCodelineSW)
        Else
            CodeLineDetails.CodelineWithCharacters = ""
        End If

        ' Show the immage------------------------------------------------------------------
        'If Form.stParAppl.Side = CByte(CtsLs.Side.SIDE_ALL_IMAGE) OrElse Form1.stParAppl.Side = CByte(CtsLs.Side.SIDE_FRONT_IMAGE) Then
        Dim pBmp As CtsLs.BITMAPINFOHEADER = Marshal.PtrToStructure(BufFrontImage, GetType(CtsLs.BITMAPINFOHEADER))

        pBitmap = New Bitmap(pBmp.biWidth, pBmp.biHeight, PixelFormat.Format24bppRgb)

        'BitmapData bmpData = pBitmap.LockBits(new Rectangle(0, 0, pBitmap.Width, pBitmap.Height), ImageLockMode.WriteOnly, pBitmap.PixelFormat);
        Dim xx As Integer, yy As Integer, cxPixel As Integer
        Dim diff As Int32, WidthBytes As Int32 = pBmp.biWidth
        Dim InitBmpData As Int32
        Dim row As Int32, col As Int32

        If pBmp.biBitCount = 1 Then
            InitBmpData = 48

            diff = WidthBytes Mod 32
            If diff <> 0 Then
                WidthBytes += (32 - diff)
            End If
            WidthBytes /= 8

            row = pBmp.biHeight - 1
            col = pBmp.biWidth - 1
            For yy = 0 To pBmp.biHeight - 1
                cxPixel = 0
                For xx = 0 To pBmp.biWidth - 1
                    Dim Pixel As Byte = Marshal.ReadByte(CInt(BufFrontImage) + InitBmpData + ((yy * WidthBytes) + cxPixel))
                    diff = xx Mod 8
                    Select Case diff
                        Case 0
                            Pixel >>= 7
                        Case 1
                            Pixel = Pixel And 64
                            Pixel >>= 6
                        Case 2
                            Pixel = Pixel And 32
                            Pixel >>= 5
                        Case 3
                            Pixel = Pixel And 16
                            Pixel >>= 4
                        Case 4
                            Pixel = Pixel And 8
                            Pixel >>= 3
                        Case 5
                            Pixel = Pixel And 4
                            Pixel >>= 2
                        Case 6
                            Pixel = Pixel And 2
                            Pixel >>= 1
                        Case 7
                            Pixel = Pixel And 1
                            cxPixel += 1
                    End Select
                    Pixel *= 255
                    Dim Pixel24 As Color
                    Pixel24 = Color.FromArgb((Pixel * 256 * 256) + (Pixel * 256) + Pixel)
                    pBitmap.SetPixel(xx, (row - yy), Pixel24)
                Next
            Next
        ElseIf pBmp.biBitCount = 4 Then
            InitBmpData = 104

            diff = WidthBytes Mod 8
            If diff <> 0 Then
                WidthBytes += (8 - diff)
            End If
            WidthBytes /= 2

            row = pBmp.biHeight - 1
            col = pBmp.biWidth - 1
            For yy = 0 To pBmp.biHeight - 1
                cxPixel = 0
                For xx = 0 To pBmp.biWidth - 1
                    Dim Pixel As Byte = Marshal.ReadByte(CInt(BufFrontImage) + InitBmpData + ((yy * WidthBytes) + cxPixel))
                    diff = xx Mod 2
                    If diff = 1 Then
                        Pixel >>= 4
                        cxPixel += 1
                    Else
                        Pixel = Pixel And 15
                    End If
                    Pixel *= 16
                    Dim Pixel24 As Color
                    Pixel24 = Color.FromArgb((Pixel * 256 * 256) + (Pixel * 256) + Pixel)
                    pBitmap.SetPixel(xx, (row - yy), Pixel24)
                Next
            Next
        ElseIf pBmp.biBitCount = 8 Then
            InitBmpData = 1064

            diff = WidthBytes Mod 4
            If diff <> 0 Then
                WidthBytes += (4 - diff)
            End If

            row = pBmp.biHeight - 1
            col = pBmp.biWidth - 1
            For yy = 0 To pBmp.biHeight - 1
                For xx = 0 To pBmp.biWidth - 1
                    Dim Pixel As Byte = Marshal.ReadByte(CInt(BufFrontImage) + InitBmpData + ((yy * WidthBytes) + xx))
                    Dim Pixel24 As Color
                    Pixel24 = Color.FromArgb((Pixel * 256 * 256) + (Pixel * 256) + Pixel)
                    pBitmap.SetPixel(xx, (row - yy), Pixel24)
                Next
            Next
        ElseIf pBmp.biBitCount = 24 Then
            WidthBytes *= 3
            InitBmpData = 48

            diff = WidthBytes Mod 4
            If diff <> 0 Then
                WidthBytes += (4 - diff)
            End If

            row = pBmp.biHeight - 1
            col = pBmp.biWidth - 1
            For yy = 0 To pBmp.biHeight - 1
                cxPixel = 0
                For xx = 0 To pBmp.biWidth - 1
                    Dim Pixelb As Byte = Marshal.ReadByte(CInt(BufFrontImage) + InitBmpData + ((yy * WidthBytes) + cxPixel))
                    Dim Pixelg As Byte = Marshal.ReadByte(CInt(BufFrontImage) + InitBmpData + ((yy * WidthBytes) + cxPixel + 1))
                    Dim Pixelr As Byte = Marshal.ReadByte(CInt(BufFrontImage) + InitBmpData + ((yy * WidthBytes) + cxPixel + 2))
                    Dim Pixel24 As Color
                    Pixel24 = Color.FromArgb((Pixelb * 256 * 256) + (Pixelr * 256) + Pixelg)
                    pBitmap.SetPixel(xx, (row - yy), Pixel24)
                    cxPixel += 3
                Next
            Next
        End If

        'CopyMemory(bmpData.Scan0, (IntPtr)((int)BufFrontImage + 1064), pBmp.biSizeImage);
        'pBitmap.UnlockBits(bmpData);


        ''pbImage.Image = pBitmap ------white - Black

        'End If

        ' Refresh the form
        Application.DoEvents()




        ' Free the previous image memory and save the current
        If Save_FrontImage <> IntPtr.Zero Then
            CtsLs.LSFreeImage(0, Save_FrontImage)
        End If
        If BufFrontGrayUVImage <> IntPtr.Zero Then
            Save_FrontImage = BufFrontGrayUVImage
            CtsLs.LSFreeImage(0, BufFrontImage)
            CtsLs.LSFreeImage(0, BufFrontUVImage)
        ElseIf BufFrontImage <> IntPtr.Zero Then
            Save_FrontImage = BufFrontImage
        End If

        If Save_RearImage <> IntPtr.Zero Then
            CtsLs.LSFreeImage(0, Save_RearImage)
        End If
        Save_RearImage = BufRearImage


        ' Free of local variable
        Marshal.FreeHGlobal(BufBarcode)
        Marshal.FreeHGlobal(BufCodelineHW)
        Marshal.FreeHGlobal(BufCodelineSW)
        Marshal.FreeHGlobal(BufRearFile)
        Marshal.FreeHGlobal(BufFrontFile)

        Return Reply
    End Function

    Private Function CheckReply(ByVal ChReply As Integer, ByVal Requester As String) As Boolean
        Dim RcError As Boolean = False
        Dim szTextMsg As String

        szTextMsg = Requester & vbLf & vbLf

        Select Case ChReply
            Case CtsLs.LsReply.LS_OKAY
                '	szTextMsg += "LS_OKAY";
                Exit Select

                ' --- ERRORS ---------------------------------------------------------
            Case CtsLs.LsReply.LS_SYSTEM_ERROR
                szTextMsg += "The module was unable to execute command due to a system error." & vbLf & vbLf & "Possible reasons: memory allocation error."
                MessageBox.Show(szTextMsg, TITLE_ERROR)
                RcError = True
                Exit Select
            Case CtsLs.LsReply.LS_USB_ERROR
                szTextMsg += "The module was unable to execute command due to a USB hardware error."
                MessageBox.Show(szTextMsg, TITLE_ERROR)
                RcError = True
                Exit Select
            Case CtsLs.LsReply.LS_PERIPHERAL_NOT_FOUND
                szTextMsg += "Peripheral not found." & vbLf & vbLf & "Possible reasons: peripheral is switched off or not connected to the USB bus."
                MessageBox.Show(szTextMsg, TITLE_ERROR)
                RcError = True
                Exit Select
            Case CtsLs.LsReply.LS_HARDWARE_ERROR
                szTextMsg += "Peripheral hardware error."
                MessageBox.Show(szTextMsg, TITLE_ERROR)
                RcError = True
                Exit Select
            Case CtsLs.LsReply.LS_PERIPHERAL_OFF_ON
                szTextMsg += "Peripheral has been switched off and on again."
                MessageBox.Show(szTextMsg, TITLE_ERROR)
                RcError = True
                Exit Select
            Case CtsLs.LsReply.LS_PAPER_JAM
                szTextMsg += "Document jammed."
                MessageBox.Show(szTextMsg, TITLE_ERROR)
                RcError = True
                Exit Select
            Case CtsLs.LsReply.LS_TARGET_BUSY
                szTextMsg += "Peripheral busy"
                MessageBox.Show(szTextMsg, TITLE_ERROR)
                RcError = True
                Exit Select
            Case CtsLs.LsReply.LS_INVALID_COMMAND
                '			DebugBreak();
                szTextMsg += "Invalid command."
                MessageBox.Show(szTextMsg, TITLE_ERROR)
                RcError = True
                Exit Select
            Case CtsLs.LsReply.LS_COMMAND_IN_EXECUTION_YET
                szTextMsg += "A command is already in execution" & vbLf & "Current command aborted"
                MessageBox.Show(szTextMsg, TITLE_ERROR)
                RcError = True
                Exit Select
            Case CtsLs.LsReply.LS_JPEG_ERROR
                szTextMsg += "JPEG image not created !"
                MessageBox.Show(szTextMsg, TITLE_ERROR)
                RcError = True
                Exit Select
            Case CtsLs.LsReply.LS_COMMAND_SEQUENCE_ERROR
                szTextMsg += "There's another command in execution" & vbLf & "Impossible to execute the command"
                MessageBox.Show(szTextMsg, TITLE_ERROR)
                RcError = True
                Exit Select
            Case CtsLs.LsReply.LS_NO_LIBRARY_LOAD
                szTextMsg += "Support library not present"
                MessageBox.Show(szTextMsg, TITLE_ERROR)
                RcError = True
                Exit Select
            Case CtsLs.LsReply.LS_BMP_ERROR
                szTextMsg += "DIB image not created !"
                MessageBox.Show(szTextMsg, TITLE_ERROR)
                RcError = True
                Exit Select
            Case CtsLs.LsReply.LS_TIFF_ERROR
                szTextMsg += "TIFF image not created !"
                MessageBox.Show(szTextMsg, TITLE_ERROR)
                RcError = True
                Exit Select
            Case CtsLs.LsReply.LS_IMAGE_NOT_PRESENT
                szTextMsg += "Image not present."
                MessageBox.Show(szTextMsg, TITLE_ERROR)
                RcError = True
                Exit Select
            Case CtsLs.LsReply.LS_READ_TIMEOUT_EXPIRED
                szTextMsg += "The peripheral was disconnected"
                MessageBox.Show(szTextMsg, TITLE_ERROR)
                RcError = True
                Exit Select
            Case CtsLs.LsReply.LS_DOUBLE_LEAFING_ERROR
                szTextMsg += "Double Leafing occurred."
                MessageBox.Show(szTextMsg, TITLE_ERROR)
                RcError = True
                Exit Select
            Case CtsLs.LsReply.LS_INVALID_FORMAT
                szTextMsg += "Print format not supported."
                MessageBox.Show(szTextMsg, TITLE_ERROR)
                RcError = True
                Exit Select
            Case CtsLs.LsReply.LS_SHORT_PAPER
                szTextMsg += "Paper Short !"
                MessageBox.Show(szTextMsg, TITLE_ERROR)
                RcError = True
                Exit Select
            Case CtsLs.LsReply.LS_INVALID_DOC_LENGTH
                szTextMsg += "Invalid Length !"
                MessageBox.Show(szTextMsg, TITLE_ERROR)
                RcError = True
                Exit Select
            Case CtsLs.LsReply.LS_JAM_AT_MICR_PHOTO
                szTextMsg += "Jam at MICR photo."
                MessageBox.Show(szTextMsg, TITLE_ERROR)
                RcError = True
                Exit Select
            Case CtsLs.LsReply.LS_JAM_DOC_TOO_LONG
                szTextMsg += "Double Feeding occurred or document too long !"
                MessageBox.Show(szTextMsg, TITLE_ERROR)
                RcError = True
                Exit Select
            Case CtsLs.LsReply.LS_JAM_AT_SCANNER_PHOTO
                szTextMsg += "Jam at Scanner photo."
                MessageBox.Show(szTextMsg, TITLE_ERROR)
                RcError = True
                Exit Select
            Case CtsLs.LsReply.LS_INVALID_PWM_VALUE
                szTextMsg += "Invalid Ultra Violet PWM value."
                MessageBox.Show(szTextMsg, TITLE_ERROR)
                RcError = True
                Exit Select
            Case CtsLs.LsReply.LS_IPBOX_ADDRESS_NOT_FOUNDED
                szTextMsg += "LSConnect Box IP address not founded !"
                MessageBox.Show(szTextMsg, TITLE_ERROR)
                RcError = True
                Exit Select

                ' --- WARNINGS -------------------------------------------------------
            Case CtsLs.LsReply.LS_FEEDER_EMPTY
                szTextMsg += "No Cheque(s) available to scan"
                MessageBox.Show(szTextMsg, TITLE_ERROR)
                RcError = True
                Exit Select
            Case CtsLs.LsReply.LS_DATA_TRUNCATED
                szTextMsg += "Codeline truncated !"
                MessageBox.Show(szTextMsg, TITLE_ERROR)
                RcError = True
                Exit Select
            Case CtsLs.LsReply.LS_ALREADY_OPEN
                szTextMsg += "Peripheral already connected !" & vbLf & " Issue a reset command before to continue test."
                MessageBox.Show(szTextMsg, TITLE_ERROR)
                RcError = True
                Exit Select
            Case CtsLs.LsReply.LS_COMMAND_NOT_SUPPORTED
                szTextMsg += "CtsLs.LsReply.LS_COMMAND_NOT_SUPPORTED"
                MessageBox.Show(szTextMsg, TITLE_ERROR)
                RcError = True
                Exit Select
            Case CtsLs.LsReply.LS_TRY_TO_RESET
                szTextMsg += "Open failed retry"
                MessageBox.Show(szTextMsg, TITLE_ERROR)
                RcError = True
                Exit Select
            Case CtsLs.LsReply.LS_OPEN_NOT_DONE
                szTextMsg += "Open failed retry"
                MessageBox.Show(szTextMsg, TITLE_ERROR)
                RcError = True
                Exit Select
            Case CtsLs.LsReply.LS_PERIPHERAL_BUSY
                szTextMsg += "Peripheral busy, command NOT terminate !"
                MessageBox.Show(szTextMsg, TITLE_ERROR)
                RcError = True
                Exit Select
            Case CtsLs.LsReply.LS_DOUBLE_LEAFING_WARNING
                szTextMsg += "Warning Double Leafing occurs !"
                MessageBox.Show(szTextMsg, TITLE_ERROR)
                RcError = False
                Exit Select
            Case CtsLs.LsReply.LS_SORTER1_FULL
                szTextMsg += "Sorter FULL"
                MessageBox.Show(szTextMsg, TITLE_ERROR)
                RcError = True
                Exit Select
            Case CtsLs.LsReply.LS_NO_OTHER_DOCUMENT
                szTextMsg += "No other documents or Sorter Full"
                MessageBox.Show(szTextMsg, TITLE_ERROR)
                RcError = True
                Exit Select
            Case Else

                szTextMsg += "UNKNOWN REPLY CODE NUMBER " & ChReply.ToString()
                MessageBox.Show(szTextMsg, TITLE_ERROR)
                RcError = True
                Exit Select
        End Select

        Return RcError
    End Function
    ' end CheckReply

    Private Function TryConnect(ByRef hConnect As Short) As Integer
        Dim Reply As Integer

        Reply = CtsLs.LSConnect(0, 0, CShort(CtsLs.LsUnitType.LS_40_USB), hConnect)
        If Reply <> CtsLs.LsReply.LS_OKAY AndAlso Reply <> CtsLs.LsReply.LS_ALREADY_OPEN AndAlso Reply <> CtsLs.LsReply.LS_TRY_TO_RESET Then
            Reply = CtsLs.LSConnect(0, 0, CShort(CtsLs.LsUnitType.LS_100_USB), hConnect)
            If Reply <> CtsLs.LsReply.LS_OKAY AndAlso Reply <> CtsLs.LsReply.LS_ALREADY_OPEN AndAlso Reply <> CtsLs.LsReply.LS_TRY_TO_RESET Then
                Reply = CtsLs.LSConnect(0, 0, CShort(CtsLs.LsUnitType.LS_150_USB), hConnect)
                If Reply <> CtsLs.LsReply.LS_OKAY AndAlso Reply <> CtsLs.LsReply.LS_ALREADY_OPEN AndAlso Reply <> CtsLs.LsReply.LS_TRY_TO_RESET Then
                    Reply = CtsLs.LSConnect(0, 0, CShort(CtsLs.LsUnitType.LS_515_USB), hConnect)
                End If
            End If
        End If

        If Reply = CtsLs.LsReply.LS_OKAY OrElse Reply = CtsLs.LsReply.LS_ALREADY_OPEN OrElse Reply = CtsLs.LsReply.LS_TRY_TO_RESET Then
            Reply = CtsLs.LsReply.LS_OKAY
        End If

        Return Reply
    End Function

    Private Sub txtChqCount_TextChanged(sender As Object, e As EventArgs) Handles txtChqCount.TextChanged

    End Sub

    
End Class