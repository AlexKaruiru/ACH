Imports System.Runtime.InteropServices
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Text
Imports System.IO
Imports System.Windows.Forms
Imports System.Threading
Imports LsFamily
Imports LsFamily.LsApi

Imports BrClearing.Common.Modscan


<Serializable()> Public Class ApplClass

    Const CONFIGURATION_FILE = "LsFamily.cfg"
    Public Const SAVE_DIRECTORY_IMAGE = "Images"

    Public Const DECODE_NO = 0
    Public Const DECODE_MICR = 1
    Public Const DECODE_OCR = 2
    Public Const DECODE_BARCODE = 4
    Public Const DECODE_E13B_MICR_AND_OCR = 16      ' decodifico e13b micr + ocr

    Public Codeline_HW As Int16
    Public ScanMode As Int16
    Public Side As Byte
    Public FrontStamp As Int16
    Public BackStamp As Int16
    Public Sorter As Int16
    Public PrintValidate As Int16
    Public PrintValidate1 As Int16
    Public PrintValidate2 As Int16
    Public PrintValidate3 As Int16
    Public PrintValidate4 As Int16
    Public Endorse_str As String
    Public Endorse_str2 As String
    Public Endorse_str3 As String
    Public Endorse_str4 As String
    Public Beep As Int16
    Public WiatDoc As Int16
    Public SaveMode As Int16
    Public FileFormat As Int16
    Public DuobleLeafingLevel As Int16
    Public DuobleLeafingValue As Int16
    Public CodelineType As Int16
    Public OCR_Type As Byte
    Public OCR_x As Double
    Public OCR_y As Double
    Public OCR_w As Double
    Public OCR_h As Double
    Public OCR_Unit As Int16
    Public Barcode_Type As Int16
    Public Barcode_x As Double
    Public Barcode_y As Double
    Public Barcode_w As Double
    Public Barcode_h As Double
    Public Barcode_Unit As Int16
    Public IP_Address As String
    Public ScanMode_Card As Int16
    Public Badge_Tracks As Int16
    Public Badge_Timeout As UInt32
    Public Sorter_Ls100 As Int16
    Public Sorter_Ls150 As Int16
    Public Sorter_Ls520 As Int16
    Public Sorter_Ls800 As Int16
    Public Sorter_HoldDocument As Int16
    Public Print_High As Boolean
    Public Stamp As Int16
    'Retained Paramiter
    Public RetainedSide As Byte
    Public RetainedFrontStamp As Int16
    Public RetainedSorter As Int16
    Public RetainedPrintValidate As Int16
    Public RetainedPrintValidate1 As Int16
    Public RetainedPrintValidate2 As Int16
    Public RetainedPrintValidate3 As Int16
    Public RetainedPrintValidate4 As Int16
    Public RetainedEndorse_str As String
    Public RetainedEndorse_str2 As String
    Public RetainedEndorse_str3 As String
    Public RetainedEndorse_str4 As String
    'UV parameter 
    Public PWMvalue As Int16
    Public HightContrast As Boolean
    Public ShowUvImage As Boolean

    Public Quality As Int16

    Public Function ReadConfiguration(ByRef ParAppl As ApplClass, ByVal LsDefines As LsFamily.LsDefines) As Boolean
        Dim strHostName As String
        Dim strIPAddress As String
        Try
            'Dim fs As FileStream = New FileStream(CONFIGURATION_FILE, FileMode.Open, FileAccess.Read)
            'Dim formatter As BinaryFormatter = New BinaryFormatter

            'ParAppl = formatter.Deserialize(fs)
            'MessageBox.Show("inside ReadConfiguration ")
            'fs.Close()
            strHostName = Net.Dns.GetHostName()
            strIPAddress = "127.0.0.1"  'System.Net.Dns.GetHostByName(strHostName).AddressList(0).ToString()

            Codeline_HW = LsFamily.LsDefines.CodelineToRead.READ_CODELINE_HW_MICR
            ScanMode = LsFamily.LsDefines.ScanMode.SCAN_MODE_256GR100_AND_UV
            Side = LsFamily.LsDefines.Side.SIDE_ALL_IMAGE
            FrontStamp = LsFamily.LsDefines.Stamp.STAMP_NO
            BackStamp = LsFamily.LsDefines.Stamp.STAMP_NO
            Sorter = LsFamily.LsDefines.Sorter.SORTER_POCKET_1
            PrintValidate = LsFamily.LsDefines.PrintValidate.PRINT_VALIDATE_NO
            PrintValidate1 = LsFamily.LsDefines.PrintValidate.PRINT_VALIDATE_NO
            Endorse_str = "BR Clearing 1"
            PrintValidate2 = LsFamily.LsDefines.PrintValidate.PRINT_VALIDATE_NO
            Endorse_str2 = "BR Clearing 2"
            PrintValidate3 = LsFamily.LsDefines.PrintValidate.PRINT_VALIDATE_NO
            Endorse_str3 = "BR Clearing 3"
            PrintValidate4 = LsFamily.LsDefines.PrintValidate.PRINT_VALIDATE_NO
            Endorse_str4 = "BR Clearing 4"
            SaveMode = LsFamily.LsDefines.ImageSave.IMAGE_SAVE_BOTH
            FileFormat = LsFamily.LsDefines.FileType.FILE_JPEG
            DuobleLeafingLevel = LsFamily.LsDefines.DoubleLeafing.DOUBLE_LEAFING_LEVEL3
            DuobleLeafingValue = LsFamily.LsDefines.DoubleLeafing.DOUBLE_LEAFING_LEVEL3
            CodelineType = ApplClass.DECODE_MICR
            OCR_Type = LsFamily.LsDefines.CodelineToRead.READ_CODELINE_SW_OCRB_NUM
            OCR_x = 4
            OCR_y = 2
            OCR_w = -1
            OCR_h = 10.5
            OCR_Unit = LsFamily.LsDefines.Unit.UNIT_MM
            Barcode_Type = LsFamily.LsDefines.CodelineToRead.READ_BARCODE_2_OF_5
            Barcode_x = 4
            Barcode_y = 2
            Barcode_w = 20
            Barcode_h = 10
            Barcode_Unit = LsFamily.LsDefines.Unit.UNIT_MM
            IP_Address = strIPAddress
            Badge_Tracks = LsFamily.LsDefines.Badge.BADGE_READ_TRACKS_2_3
            Badge_Timeout = Convert.ToUInt32(7000)
            Sorter_Ls100 = LsFamily.LsDefines.Sorter.SORTER_POCKET_1
            Sorter_Ls150 = LsFamily.LsDefines.Sorter.SORTER_POCKET_1
            Sorter_Ls520 = LsFamily.LsDefines.Sorter.SORTER_SWICTH_1_TO_2
            Sorter_Ls800 = LsFamily.LsDefines.Sorter.SORTER_SEQUENTIAL
            Sorter_HoldDocument = LsFamily.LsDefines.Sorter.SORTER_POCKET_1
            Print_High = False
            Quality = 128

        Catch e As FileNotFoundException
            MessageBox.Show("error 1 " + e.Message)
            strHostName = Net.Dns.GetHostName()
            strIPAddress = "127.0.0.1"  ' System.Net.Dns.GetHostByName(strHostName).AddressList(0).ToString()
            Codeline_HW = LsFamily.LsDefines.CodelineToRead.READ_CODELINE_HW_MICR
            ScanMode = LsFamily.LsDefines.ScanMode.SCAN_MODE_256GR100_AND_UV
            Side = LsFamily.LsDefines.Side.SIDE_ALL_IMAGE
            FrontStamp = LsFamily.LsDefines.Stamp.STAMP_NO
            BackStamp = LsFamily.LsDefines.Stamp.STAMP_NO
            Sorter = LsFamily.LsDefines.Sorter.SORTER_POCKET_1
            PrintValidate = LsFamily.LsDefines.PrintValidate.PRINT_VALIDATE_NO
            PrintValidate1 = LsFamily.LsDefines.PrintValidate.PRINT_VALIDATE_NO
            Endorse_str = "BR Clearing 1"
            PrintValidate2 = LsFamily.LsDefines.PrintValidate.PRINT_VALIDATE_NO
            Endorse_str2 = "BR Clearing 2"
            PrintValidate3 = LsFamily.LsDefines.PrintValidate.PRINT_VALIDATE_NO
            Endorse_str3 = "BR Clearing 3"
            PrintValidate4 = LsFamily.LsDefines.PrintValidate.PRINT_VALIDATE_NO
            Endorse_str4 = "BR Clearing 4"
            SaveMode = LsFamily.LsDefines.ImageSave.IMAGE_SAVE_BOTH
            FileFormat = LsFamily.LsDefines.FileType.FILE_JPEG
            DuobleLeafingLevel = LsFamily.LsDefines.DoubleLeafing.DOUBLE_LEAFING_LEVEL3
            DuobleLeafingValue = LsFamily.LsDefines.DoubleLeafing.DOUBLE_LEAFING_LEVEL3
            CodelineType = ApplClass.DECODE_MICR
            OCR_Type = LsFamily.LsDefines.CodelineToRead.READ_CODELINE_SW_OCRB_NUM
            OCR_x = 4
            OCR_y = 2
            OCR_w = -1
            OCR_h = 10.5
            OCR_Unit = LsFamily.LsDefines.Unit.UNIT_MM
            Barcode_Type = LsFamily.LsDefines.CodelineToRead.READ_BARCODE_2_OF_5
            Barcode_x = 4
            Barcode_y = 2
            Barcode_w = 20
            Barcode_h = 10
            Barcode_Unit = LsFamily.LsDefines.Unit.UNIT_MM
            IP_Address = strIPAddress
            Badge_Tracks = LsFamily.LsDefines.Badge.BADGE_READ_TRACKS_2_3
            Badge_Timeout = Convert.ToUInt32(7000)
            Sorter_Ls100 = LsFamily.LsDefines.Sorter.SORTER_POCKET_1
            Sorter_Ls150 = LsFamily.LsDefines.Sorter.SORTER_POCKET_1
            Sorter_Ls520 = LsFamily.LsDefines.Sorter.SORTER_SWICTH_1_TO_2
            Sorter_Ls800 = LsFamily.LsDefines.Sorter.SORTER_SEQUENTIAL
            Sorter_HoldDocument = LsFamily.LsDefines.Sorter.SORTER_DOC_HOLDED
            Print_High = False

        Catch e As SerializationException
            MessageBox.Show(e.Message)
            strHostName = Net.Dns.GetHostName()
            strIPAddress = "127.0.0.1" 'System.Net.Dns.GetHostByName(strHostName).AddressList(0).ToString()
            Codeline_HW = LsFamily.LsDefines.CodelineToRead.READ_CODELINE_HW_MICR
            ScanMode = LsFamily.LsDefines.ScanMode.SCAN_MODE_256GR100_AND_UV
            Side = LsFamily.LsDefines.Side.SIDE_ALL_IMAGE
            FrontStamp = LsFamily.LsDefines.Stamp.STAMP_NO
            BackStamp = LsFamily.LsDefines.Stamp.STAMP_NO
            Sorter = LsFamily.LsDefines.Sorter.SORTER_POCKET_1
            PrintValidate = LsFamily.LsDefines.PrintValidate.PRINT_VALIDATE_NO
            PrintValidate1 = LsFamily.LsDefines.PrintValidate.PRINT_VALIDATE_NO
            Endorse_str = "BR Clearing 1"
            PrintValidate2 = LsFamily.LsDefines.PrintValidate.PRINT_VALIDATE_NO
            Endorse_str2 = "BR Clearing 2"
            PrintValidate3 = LsFamily.LsDefines.PrintValidate.PRINT_VALIDATE_NO
            Endorse_str3 = "BR Clearing 3"
            PrintValidate4 = LsFamily.LsDefines.PrintValidate.PRINT_VALIDATE_NO
            Endorse_str4 = "BR Clearing 4"
            SaveMode = LsFamily.LsDefines.ImageSave.IMAGE_SAVE_BOTH
            FileFormat = LsFamily.LsDefines.FileType.FILE_JPEG
            DuobleLeafingLevel = LsFamily.LsDefines.DoubleLeafing.DOUBLE_LEAFING_LEVEL3
            DuobleLeafingValue = LsFamily.LsDefines.DoubleLeafing.DOUBLE_LEAFING_LEVEL3
            CodelineType = ApplClass.DECODE_MICR
            OCR_Type = LsFamily.LsDefines.CodelineToRead.READ_CODELINE_SW_OCRB_NUM
            OCR_x = 4
            OCR_y = 2
            OCR_w = -1
            OCR_h = 10.5
            OCR_Unit = LsFamily.LsDefines.Unit.UNIT_MM
            Barcode_Type = LsFamily.LsDefines.CodelineToRead.READ_BARCODE_2_OF_5
            Barcode_x = 4
            Barcode_y = 2
            Barcode_w = 20
            Barcode_h = 10
            Barcode_Unit = LsFamily.LsDefines.Unit.UNIT_MM
            IP_Address = strIPAddress
            Badge_Tracks = LsFamily.LsDefines.Badge.BADGE_READ_TRACKS_2_3
            Badge_Timeout = Convert.ToUInt32(7000)
            Sorter_Ls100 = LsFamily.LsDefines.Sorter.SORTER_POCKET_1
            Sorter_Ls150 = LsFamily.LsDefines.Sorter.SORTER_POCKET_1
            Sorter_Ls520 = LsFamily.LsDefines.Sorter.SORTER_SWICTH_1_TO_2
            Sorter_Ls800 = LsFamily.LsDefines.Sorter.SORTER_SEQUENTIAL
            Sorter_HoldDocument = LsFamily.LsDefines.Sorter.SORTER_DOC_HOLDED
            Print_High = False
        End Try
        'MessageBox.Show("leaving ReadConfiguration ")

        Return True
    End Function

    Public Function SaveConfiguration(ByVal ParAppl As ApplClass) As Boolean

        'Dim fs As FileStream = New FileStream(CONFIGURATION_FILE, FileMode.OpenOrCreate, FileAccess.Write)
        'Dim formatter As BinaryFormatter = New BinaryFormatter

        ''        marshal.StructureToPtr()
        'formatter.Serialize(fs, ParAppl)
        ''        fs.Write(ab, 0, ab.Length)

        'fs.Close()

        Return True
    End Function

    Public Shared Function CheckReply(ByVal reply As Int16, ByVal func As String) As String
        Dim def As String

        Select Case reply
            ' Reply ok
            Case LsFamily.LsReply.LS_OKAY
                def = func + Chr(13) + Chr(13) + "Reply OK !"

                'Warnings
            Case LsFamily.LsReply.LS_FEEDER_EMPTY
                def = func + Chr(13) + Chr(13) + "Feeder Empty !"
            Case LsFamily.LsReply.LS_BADGE_TIMEOUT
                def = func + Chr(13) + Chr(13) + "Badge reader timeout expired !"
            Case LsFamily.LsReply.LS_NO_OTHER_DOCUMENT
                def = func + Chr(13) + Chr(13) + "Stop for Pocket full or no more Documents in the feeder !"
            Case LsFamily.LsReply.LS_COMMAND_NOT_SUPPORTED
                def = func + Chr(13) + Chr(13) + "Command not supported !"
            Case LsFamily.LsReply.LS_SORTER1_FULL
                def = func + Chr(13) + Chr(13) + "Pocket 1 full !"
            Case LsFamily.LsReply.LS_SORTER2_FULL
                def = func + Chr(13) + Chr(13) + "Pocket 2 full !"
            Case LsFamily.LsReply.LS_SORTERS_BOTH_FULL
                def = func + Chr(13) + Chr(13) + "All Pocket full !"
            Case LsFamily.LsReply.LS_DOUBLE_LEAFING_WARNING
                def = func + Chr(13) + Chr(13) + "Warning Double Leafing occurs !"
            Case LsFamily.LsReply.LS_LOOP_INTERRUPTED
                def = func + Chr(13) + Chr(13) + "Selected pocket full."
            Case LsFamily.LsReply.LS_INVALID_COMMAND
                def = func + Chr(13) + Chr(13) + "Invalid Command"

                ' Errors
            Case LsFamily.LsReply.LS_PERIPHERAL_NOT_FOUND
                def = func + Chr(13) + Chr(13) + "Peripheral NOT connected or power OFF !"

            Case LsFamily.LsReply.LS_NO_LIBRARY_LOAD
                def = func + Chr(13) + Chr(13) + "No library Load !"
            Case LsFamily.LsReply.LS_PAPER_JAM
                def = func + Chr(13) + Chr(13) + "Paper Jammed !"
            Case LsFamily.LsReply.LS_JAM_AT_MICR_PHOTO
                def = func + Chr(13) + Chr(13) + "Paper Jammed at micr photo !"
            Case LsFamily.LsReply.LS_JAM_AT_SCANNER_PHOTO
                def = func + Chr(13) + Chr(13) + "Paper Jammed at scanner photo!"
            Case LsFamily.LsReply.LS_JAM_DOC_TOO_LONG
                def = func + Chr(13) + Chr(13) + "Paper Jammed , doc too long!"
            Case LsFamily.LsReply.LS_DOUBLE_LEAFING_ERROR
                def = func + Chr(13) + Chr(13) + "Double leafing error"
            Case LsFamily.LsReply.LS_PC_HW_ERROR
                def = func + Chr(13) + Chr(13) + "Retry the operation !"
            Case LsFamily.LsReply.LS_INVALID_CONNECTION_HANDLE
                If (LsUnitType) Then
                    def = func + Chr(13) + Chr(13) + "Invalid Connection Handle !"
                Else
                    def = func + Chr(13) + Chr(13) + "A type of LS must be selected !"
                End If
            Case LsFamily.LsReply.LS_INVALID_SORTER
                def = func + Chr(13) + Chr(13) + "Invalid Sorter selected !"
            Case Else
                def = func + Chr(13) + Chr(13) + "Unknow Reply " + reply.ToString()
        End Select

        Return def
    End Function


    Public Function showIdentify(ByVal Cfg As LsFamily.LsConfiguration, ByVal ProductVer As String, ByVal Date_fw As String, ByVal Version As String, ByVal InkJet As String, ByVal SerialNumber As String) As Int16
        Dim MsgText As String
        Dim fFound As Boolean

        Try

            MsgText = "Peripheral Name : " + ProductVer + Chr(13)
            If (LsUnitType <> LsFamily.LsDefines.LsUnitType.LS_100_USB And _
                LsUnitType <> LsFamily.LsDefines.LsUnitType.LS_100_ETH) Then
                MsgText += "Firmware Version : " + Version + Chr(13)
            End If

            If (Date_fw <> Nothing And Date_fw.Length) Then
                MsgText += "Release Date : " + Date_fw + Chr(13) + Chr(13)
            End If

            If (InkJet <> Nothing) Then
                MsgText += "Expansion Board Printer Firmware Version : " + InkJet + Chr(13)
            End If

            'If (Lema <> Nothing) Then
            'MsgText += "Expansion Board OCR Firmware Version : " + Lema + Chr(13)
            'End If

            If (SerialNumber <> Nothing) Then
                MsgText += Chr(13) + "Peripheral Internal Number : " + SerialNumber + Chr(13)
            End If

            MsgText += Chr(13)
            fFound = False

            If (Cfg.MICR_Reader) Then
                MsgText += "MICR reader" + Chr(13)
                fFound = True
            End If

            If (Cfg.OCR_Reader) Then
                MsgText += "OCR reader" + Chr(13)
                fFound = True
            End If

            If (Cfg.ProcessCard) Then
                MsgText += "Card Processing" + Chr(13)
                fFound = True
            End If

            If (Cfg.InkJet_Printer) Then
                MsgText += "Endorsement Ink-jet printer" + Chr(13)
                fFound = True
            End If

            If (Cfg.InkJet_Printer_4_lines) Then
                MsgText += "Endorsement 4 lines Ink-jet printer" + Chr(13)
                fFound = True
            End If

            If (Cfg.Feeder) Then
                MsgText += "Feeder" + Chr(13)
                fFound = True
            End If

            If (Cfg.Stamp_Front) Then
                MsgText += "Voiding front stamp" + Chr(13)
                fFound = True
            End If

            If (Cfg.Scanner_Front) Then
                MsgText += "Scanner FRONT" + Chr(13)
                fFound = True
            End If

            If (Cfg.Scanner_Rear) Then
                MsgText += "Scanner REAR" + Chr(13)
                fFound = True
            End If

            If (Cfg.ScannerUltraViolet) Then
                MsgText += "Scanner FRONT with Ultra Violet" + Chr(13)
                fFound = True
            End If

            If (Cfg.ColorVersion) Then
                MsgText += "Color Version" + Chr(13)
                fFound = True
            End If



            If (LsUnitType = LsFamily.LsDefines.LsUnitType.LS_800_USB) Then
                If (Cfg.Sorters_Nr = 1) Then
                    MsgText += Cfg.Sorters_Nr.ToString + " Sorter connected" + Chr(13)
                Else
                    MsgText += Cfg.Sorters_Nr.ToString + " Sorters connected" + Chr(13)
                End If
                fFound = True
            End If

            If (Cfg.Badge_Track123) Then
                MsgText += "Badge reader tracks 1/2/3" + Chr(13)
                fFound = True
            ElseIf (Cfg.Badge_Track12) Then
                MsgText += "Badge reader tracks 1/2" + Chr(13)
                fFound = True
            ElseIf (Cfg.Badge_Track23) Then
                MsgText += "Badge reader tracks 2/3" + Chr(13)
                fFound = True
            End If

            If (fFound = False) Then
                MsgText += "None" + Chr(13)
            End If

            MessageBox.Show(MsgText, "Peripheral identify", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception

            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK)
        End Try

        Return 0
    End Function


    Public Function showStatus(ByVal Status As LsFamily.LsUnitStatus) As Int16
        Dim MsgText As String

        ' Convert status to string
        Select Case Status.UnitStatus
            Case 0      'No Sense
                MsgText = "Peripheral Ok !" + Chr(13) + Chr(13) + Chr(13)
            Case 2      'Peripheral busy
                MsgText = "Peripheral busy" + Chr(13) + Chr(13) + Chr(13)
            Case 3      'Paper Jam
                MsgText = "Paper Jam" + Chr(13) + Chr(13) + Chr(13)
            Case 4      'Hardware error
                MsgText = "Hardware error" + Chr(13) + Chr(13) + Chr(13)
            Case 5      'Illegal request
                MsgText = "Illegal request" + Chr(13) + Chr(13) + Chr(13)
            Case 6      'Feeder Empty
                MsgText = "Feeder Empty" + Chr(13) + Chr(13) + Chr(13)
            Case 7      'Error Double Leafing
                MsgText = "Error Double Leafing" + Chr(13) + Chr(13) + Chr(13)
            Case 9      'Illegal Command
                MsgText = "Illegal Command" + Chr(13) + Chr(13) + Chr(13)
            Case 11     'Aborted command
                MsgText = "Aborted command" + Chr(13) + Chr(13) + Chr(13)
            Case 16     'Calibration Aborted
                MsgText = "Calibration Aborted" + Chr(13) + Chr(13) + Chr(13)
            Case Else
                MsgText = "Error " + Status.UnitStatus.ToString + " not contempled !" + Chr(13) + Chr(13) + Chr(13)
        End Select


        If Status.Photo_Feeder Then
            MsgText += "Document present in the feeder" + Chr(13)
        Else
            MsgText += "Feeder Empty" + Chr(13)
        End If

        If Status.Photo_Sorter Then
            MsgText += "Document present in the sorter" + Chr(13)
        End If

        If Status.Photo_MICR Then
            MsgText += "Document present at the MICR sensor" + Chr(13)
        End If

        If Status.Photo_Path_Ls100 Then
            MsgText += "Document in the path near the scanner" + Chr(13)
        End If

        If Status.Photo_Card Then
            MsgText += "Document present at the Card sensor" + Chr(13)
        End If

        If Status.Photo_Stamp Then
            MsgText += "Document present at the stamp sensor" + Chr(13)
        End If

        If Status.Photo_Scanners Then
            MsgText += "Photo scanner covered" + Chr(13)
        End If

        If Status.Photo_Exit Then
            MsgText += "Document present at the exit sensor" + Chr(13)
        End If

        If (Status.Photo_Path_Module_Begin) Then
            MsgText += "Photo at begin path base module covered" + Chr(13)
        End If

        If (Status.Photo_Path_Binary_Rigth) Then
            MsgText += "Photo path binary right covered" + Chr(13)
        End If

        If (Status.Photo_Path_Binary_Left) Then
            MsgText += "Photo path binary left covered" + Chr(13)
        End If

        If (Status.Photo_Path_Module_End) Then
            MsgText += "Photo at end path base module covered" + Chr(13)
        End If

        If Status.Bins_All_Full Then
            MsgText += "Bin(s) full" + Chr(13)
        End If

        If Status.Bin_1_Full Then
            MsgText += "Bin 1 full" + Chr(13)
        End If

        If Status.Bin_2_Full Then
            MsgText += "Bin 2 full" + Chr(13)
        End If

        If Status.Unit_Just_ON Then
            MsgText += "Periferal just ON" + Chr(13)
        End If

        If (Status.Photo_Path_Feeder) Then
            MsgText += "Photo path Feeder covered" + Chr(13)
        End If

        If Status.Photo_Double_Leafing_Down Then
            MsgText += "Photo DOWN Double Leafing covered" + Chr(13)
        End If

        If Status.Photo_Double_Leafing_Middle Then
            MsgText += "Photo MIDDLE Double Leafing covered" + Chr(13)
        End If

        If Status.Photo_Double_Leafing_Up Then
            MsgText += "Photo UP Double Leafing covered" + Chr(13)
        End If

        If Status.Sorter_1_input_pocket_1 Then
            MsgText += "Sorter 1 - photo input pocket 1 covered" + Chr(13)
        End If
        If Status.Sorter_1_pocket_1_full Then
            MsgText += "Sorter 1 - pocket 1 full" + Chr(13)
        End If
        If Status.Sorter_1_input_pocket_2 Then
            MsgText += "Sorter 1 - photo input pocket 2 covered" + Chr(13)
        End If
        If Status.Sorter_1_pocket_2_full Then
            MsgText += "Sorter 1 - pocket 2 full" + Chr(13)
        End If
        If Status.Sorter_1_input_pocket_3 Then
            MsgText += "Sorter 1 - photo input pocket 3 covered" + Chr(13)
        End If
        If Status.Sorter_1_pocket_3_full Then
            MsgText += "Sorter 1 - pocket 3 full" + Chr(13)
        End If

        If Status.Sorter_2_input_pocket_1 Then
            MsgText += "Sorter 2 - photo input pocket 1 covered" + Chr(13)
        End If
        If Status.Sorter_2_pocket_1_full Then
            MsgText += "Sorter 2 - pocket 1 full" + Chr(13)
        End If
        If Status.Sorter_2_input_pocket_2 Then
            MsgText += "Sorter 2 - photo input pocket 2 covered" + Chr(13)
        End If
        If Status.Sorter_2_pocket_2_full Then
            MsgText += "Sorter 2 - pocket 2 full" + Chr(13)
        End If
        If Status.Sorter_2_input_pocket_3 Then
            MsgText += "Sorter 2 - photo input pocket 3 covered" + Chr(13)
        End If
        If Status.Sorter_2_pocket_3_full Then
            MsgText += "Sorter 2 - pocket 3 full" + Chr(13)
        End If

        If Status.Sorter_3_input_pocket_1 Then
            MsgText += "Sorter 3 - photo input pocket 1 covered" + Chr(13)
        End If
        If Status.Sorter_3_pocket_1_full Then
            MsgText += "Sorter 3 - pocket 1 full" + Chr(13)
        End If
        If Status.Sorter_3_input_pocket_2 Then
            MsgText += "Sorter 3 - photo input pocket 2 covered" + Chr(13)
        End If
        If Status.Sorter_3_pocket_2_full Then
            MsgText += "Sorter 3 - pocket 2 full" + Chr(13)
        End If
        If Status.Sorter_3_input_pocket_3 Then
            MsgText += "Sorter 3 - photo input pocket 3 covered" + Chr(13)
        End If
        If Status.Sorter_3_pocket_3_full Then
            MsgText += "Sorter 3 - pocket 3 full" + Chr(13)
        End If

        If Status.Sorter_4_input_pocket_1 Then
            MsgText += "Sorter 4 - photo input pocket 1 covered" + Chr(13)
        End If
        If Status.Sorter_4_pocket_1_full Then
            MsgText += "Sorter 4 - pocket 1 full" + Chr(13)
        End If
        If Status.Sorter_4_input_pocket_2 Then
            MsgText += "Sorter 4 - photo input pocket 2 covered" + Chr(13)
        End If
        If Status.Sorter_4_pocket_2_full Then
            MsgText += "Sorter 4 - pocket 2 full" + Chr(13)
        End If
        If Status.Sorter_4_input_pocket_3 Then
            MsgText += "Sorter 4 - photo input pocket 3 covered" + Chr(13)
        End If
        If Status.Sorter_4_pocket_3_full Then
            MsgText += "Sorter 4 - pocket 3 full" + Chr(13)
        End If

        If Status.Sorter_5_input_pocket_1 Then
            MsgText += "Sorter 5 - photo input pocket 1 covered" + Chr(13)
        End If
        If Status.Sorter_5_pocket_1_full Then
            MsgText += "Sorter 5 - pocket 1 full" + Chr(13)
        End If
        If Status.Sorter_5_input_pocket_2 Then
            MsgText += "Sorter 5 - photo input pocket 2 covered" + Chr(13)
        End If
        If Status.Sorter_5_pocket_2_full Then
            MsgText += "Sorter 5 - pocket 2 full" + Chr(13)
        End If
        If Status.Sorter_5_input_pocket_3 Then
            MsgText += "Sorter 5 - photo input pocket 3 covered" + Chr(13)
        End If
        If Status.Sorter_5_pocket_3_full Then
            MsgText += "Sorter 5 - pocket 3 full" + Chr(13)
        End If

        If Status.Sorter_6_input_pocket_1 Then
            MsgText += "Sorter 6 - photo input pocket 1 covered" + Chr(13)
        End If
        If Status.Sorter_6_pocket_1_full Then
            MsgText += "Sorter 6 - pocket 1 full" + Chr(13)
        End If
        If Status.Sorter_6_input_pocket_2 Then
            MsgText += "Sorter 6 - photo input pocket 2 covered" + Chr(13)
        End If
        If Status.Sorter_6_pocket_2_full Then
            MsgText += "Sorter 6 - pocket 2 full" + Chr(13)
        End If
        If Status.Sorter_6_input_pocket_3 Then
            MsgText += "Sorter 6 - photo input pocket 3 covered" + Chr(13)
        End If
        If Status.Sorter_1_pocket_3_full Then
            MsgText += "Sorter 6 - pocket 3 full" + Chr(13)
        End If

        If Status.Sorter_7_input_pocket_1 Then
            MsgText += "Sorter 7 - photo input pocket 1 covered" + Chr(13)
        End If
        If Status.Sorter_7_pocket_1_full Then
            MsgText += "Sorter 7 - pocket 1 full" + Chr(13)
        End If
        If Status.Sorter_7_input_pocket_2 Then
            MsgText += "Sorter 7 - photo input pocket 2 covered" + Chr(13)
        End If
        If Status.Sorter_7_pocket_2_full Then
            MsgText += "Sorter 7 - pocket 2 full" + Chr(13)
        End If
        If Status.Sorter_7_input_pocket_3 Then
            MsgText += "Sorter 7 - photo input pocket 3 covered" + Chr(13)
        End If
        If Status.Sorter_7_pocket_3_full Then
            MsgText += "Sorter 7 - pocket 3 full" + Chr(13)
        End If

        MessageBox.Show(MsgText, "Peripheral status", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Return 0
    End Function
End Class
