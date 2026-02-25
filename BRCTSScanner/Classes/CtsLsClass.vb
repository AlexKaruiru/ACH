Imports System.Runtime.InteropServices


Public Class CtsLs
    <DllImport("LsApi.dll")> _
    Public Shared Function LSConnect(ByVal hWnd As Integer, ByVal hInst As Integer, ByVal Peripheral As Short, ByRef hConnect As Short) As Integer
    End Function
    <DllImport("LsApi.dll")> _
    Public Shared Function LSDisconnect(ByVal hConnect As Short, ByVal hWnd As Integer) As Integer
    End Function
    <DllImport("LsApi.dll")> _
    Public Shared Function LSUnitIdentify(ByVal hConnect As Short, ByVal hWnd As Integer, ByVal pLsCfg As Byte(), ByVal LsModel As IntPtr, ByVal FwVersion As IntPtr, ByVal FwDate As IntPtr, _
     ByVal PeripheralID As IntPtr, ByVal BoardVersion As IntPtr, ByVal DecoderExpVersion As IntPtr, ByVal InkJetVersion As IntPtr, ByVal FeederVersion As IntPtr, ByVal SorterVersion As IntPtr, _
     ByVal MotorVersion As IntPtr, ByVal Reserved1 As IntPtr, ByVal Reserved2 As IntPtr) As Integer
    End Function
    <DllImport("LsApi.dll")> _
    Public Shared Function LSUnitStatus(ByVal hConnect As Short, ByVal hWnd As Integer, ByRef lpStatus As UNITSTATUS) As Integer
    End Function
    <DllImport("LsApi.dll")> _
    Public Shared Function LSReset(ByVal hConnect As Short, ByVal hWnd As Integer, ByVal ResetType As Short) As Integer
    End Function
    <DllImport("LsApi.dll")> _
    Public Shared Function LSLoadStringWithCounterEx(ByVal hConnect As Short, ByVal hWnd As Integer, ByVal PrintType As Short, ByVal strEndorse As IntPtr, ByVal LenEndorse As Short, ByVal StartNumber As UInt32, _
     ByVal [Step] As Short) As Integer
    End Function
    <DllImport("LsApi.dll")> _
    Public Shared Function LSLoadString(ByVal hConnect As Short, ByVal hWnd As Integer, ByVal PrintType As Short, ByVal LenEndorse As Short, ByVal strEndorse As IntPtr) As Integer
    End Function
    <DllImport("LsApi.dll")> _
    Public Shared Function LSConfigDoubleLeafingAndDocLength(ByVal hConnect As Short, ByVal hWnd As Integer, ByVal Type As Int32, ByVal Value As Short, ByVal DocMin As Int32, ByVal DocMax As Int32) As Integer
    End Function
    <DllImport("LsApi.dll")> _
    Public Shared Function LSChangeStampPosition(ByVal hConnect As Short, ByVal hWnd As Integer, ByVal [Step] As Short, ByVal Reserved As Byte) As Integer
    End Function
    <DllImport("LsApi.dll")> _
    Public Shared Function LSDisableWaitDocument(ByVal hConnect As Short, ByVal hWnd As Integer, ByVal fWait As Boolean) As Integer
    End Function
    <DllImport("LsApi.dll")> _
    Public Shared Function LSSetUnitSpeed(ByVal hConnect As Short, ByVal hWnd As Integer, ByVal UnitSpeed As Short) As Integer
    End Function
    <DllImport("LsApi.dll")> _
    Public Shared Function LSSetLightIntensity(ByVal hConnect As Short, ByVal hWnd As Integer, ByVal UnitSpeed As Short) As Integer
    End Function
    <DllImport("LsApi.dll")> _
    Public Shared Function LSModifyPWMUltraViolet(ByVal hConnect As Short, ByVal hWnd As Integer, ByVal UnitSpeed As Short, ByVal HighContrast As Boolean, ByVal Reserved As Short) As Integer
    End Function
    <DllImport("LsApi.dll")> _
    Public Shared Function LSAutoDocHandle(ByVal hConnect As Short, ByVal hWnd As Integer, ByVal Stamp As Short, ByVal Validate As Short, ByVal CodeLine As Short, ByVal ScanMode As Short, _
     ByVal Feeder As Short, ByVal Sorter As Short, ByVal NumDocument As Short, ByVal ClearBlack As Short, ByVal Side As Byte, ByVal ReadMode As Short, _
     ByVal SaveImage As Short, ByVal DirectoryFile As IntPtr, ByVal BaseFilename As IntPtr, ByVal pos_x As [Single], ByVal pos_y As [Single], ByVal sizeW As [Single], _
     ByVal sizeH As [Single], ByVal OriginMeasureDoc As Short, ByVal OcrImageSide As Short, ByVal FileFormat As Short, ByVal Quality As Integer, ByVal SaveMode As Integer, _
     ByVal PageNumber As Integer, ByVal WaitTimeout As Short, ByVal Beep As Short, ByVal Reserved1 As Integer, ByVal Reserved2 As IntPtr, ByVal Reserved3 As IntPtr) As Integer
    End Function
    <DllImport("LsApi.dll")> _
    Public Shared Function LSGetDocData(ByVal hConnect As Short, ByVal hWnd As Integer, ByRef NrDoc As UInt32, ByVal FilenameFront As IntPtr, ByVal FilenameRear As IntPtr, ByVal Reserved1 As IntPtr, _
     ByVal Reserved2 As IntPtr, ByRef FrontImage As IntPtr, ByRef RearImage As IntPtr, ByRef Reserved3 As IntPtr, ByRef Reserved4 As IntPtr, ByVal CodelineSW As IntPtr, _
     ByVal CodelineHW As IntPtr, ByVal Barcode As IntPtr, ByVal CodelinesOptical As IntPtr, ByRef DocToRead As Short, ByRef NrPrinted As Int32, ByVal Reserved5 As IntPtr, _
     ByVal Reserved6 As IntPtr) As Integer
    End Function
    <DllImport("LsApi.dll")> _
    Public Shared Function LSDocHandle(ByVal hConnect As Short, ByVal hWnd As Integer, ByVal Stamp As Short, ByVal Validate As Short, ByVal CodeLine As Short, ByVal Side As Byte, _
     ByVal ScanMode As Short, ByVal Feeder As Short, ByVal Sorter As Short, ByVal WaitTimeout As Short, ByVal Beep As Short, ByRef NrDoc As UInt32, _
     ByVal ScanDocType As Int16, ByVal Reserved As Int32) As Integer
    End Function
    <DllImport("LsApi.dll")> _
    Public Shared Function LSReadCodeline(ByVal hConnect As Short, ByVal hWnd As Integer, ByVal CodelineHW As IntPtr, ByRef LenCodelineHW As Short, ByVal Barcode As IntPtr, ByRef LenBarcode As Short, _
     ByVal CodelinesOptical As IntPtr, ByRef LenOptic As Short) As Integer
    End Function
    <DllImport("LsApi.dll")> _
    Public Shared Function LSReadImage(ByVal hConnect As Short, ByVal hWnd As Integer, ByVal ClearBlack As Short, ByVal Side As Byte, ByVal ReadMode As Short, ByVal NrDoc As UInt32, _
     ByRef FrontImage As IntPtr, ByRef RearImage As IntPtr, ByRef Reserved1 As IntPtr, ByVal Reserved2 As IntPtr) As Integer
    End Function
    <DllImport("LsApi.dll")> _
    Public Shared Function LSCodelineReadFromBitmap(ByVal hWnd As Integer, ByVal hImage As IntPtr, ByVal CodelineType As Byte(), ByVal UintMeasure As Short, ByVal Pos_x As Single, ByVal Pos_y As Single, _
     ByVal Width As Single, ByVal Height As Single, ByRef ro As READOPTIONS, ByVal Codeline As IntPtr, ByRef Length_Codeline As Integer) As Integer
    End Function
    <DllImport("LsApi.dll")> _
    Public Shared Function LSReadBarcodeFromBitmap(ByVal hWnd As Integer, ByVal hImage As IntPtr, ByVal BarcodeType As Byte, ByVal Pos_x As Single, ByVal Pos_y As Single, ByVal Width As Single, _
     ByVal Height As Single, ByVal Codeline As IntPtr, ByRef Length_Codeline As Integer) As Integer
    End Function
    <DllImport("LsApi.dll")> _
    Public Shared Function LSReadPdf417FromBitmap(ByVal hWnd As Integer, ByVal hImage As IntPtr, ByVal Codeline As IntPtr, ByRef Length_Codeline As Integer, ByVal Reserved As Byte, ByVal Pos_x As Single, _
     ByVal Pos_y As Single, ByVal Width As Single, ByVal Height As Single) As Integer
    End Function
    <DllImport("LsApi.dll")> _
    Public Shared Function LSMergeImageGrayAndUV(ByVal hWnd As Integer, ByVal hFrontGrayImage As IntPtr, ByVal hFrontUVImage As IntPtr, ByVal Reserved As Single, ByVal Reserved2 As Single, ByRef hGrayUVImage As IntPtr) As Integer
    End Function
    <DllImport("LsApi.dll")> _
    Public Shared Function LSFreeImage(ByVal hWnd As Integer, ByRef hImage As IntPtr) As Integer
    End Function
    <DllImport("LsApi.dll")> _
    Public Shared Function LSUnitHistory(ByVal hConnect As Short, ByVal hWnd As Integer, ByRef UnitHistory As UNITHISTORY) As Integer
    End Function
    <DllImport("LsApi.dll")>
    Public Shared Function LSUnitConfiguration(ByVal hConnect As Short, ByVal hWnd As Integer, ByVal lpReserved As IntPtr, ByRef DeviceFeatures As UNITCONFIGURATION, ByVal LsModel As IntPtr, _
     ByVal Fw_Version As IntPtr, ByVal Fw_Date As IntPtr, ByVal PeripheralID As IntPtr, ByVal BoardAndFPGANr As IntPtr, ByVal DecoderExpVersion As IntPtr, ByVal InkJetVersion As IntPtr, ByVal FeederVersion As IntPtr, _
     ByVal SorterVersion As IntPtr, ByVal MotorVersion As IntPtr, ByVal Reserved1 As IntPtr, ByVal Reserved2 As IntPtr) As Integer
    End Function
    <DllImport("LsApi.dll")>
    Public Shared Function LSSetBinarizationParameters(ByVal hConnect As Short, ByVal hWnd As Integer, ByVal Method As Short, ByVal Threshold As Short, ByVal Reserved As [Single]) As Integer
    End Function
    Public Structure BITMAPINFOHEADER
        Public biSize As UInt32
        Public biWidth As Int32
        Public biHeight As Int32
        Public biPlanes As Int16
        Public biBitCount As Int16
        Public biCompression As UInt32
        Public biSizeImage As UInt32
        Public biXPelsPerMeter As Int32
        Public biYPelsPerMeter As Int32
        Public biClrUsed As UInt32
        Public biClrImportant As UInt32
    End Structure

    Public Structure UNITCONFIGURATION

        Public Size As Integer                      ' Size of the structure

        Public MICR_Reader As Boolean               '		 Ls100 Ls150 Ls515 Ls800
        Public CMC7_Reader_only As Boolean          '		 Ls100 Ls150 Ls515 Ls800
        Public E13B_Reader_only As Boolean          '		 Ls100 Ls150 Ls515 Ls800
        Public Scanner_Front As Boolean             '		 Ls100 Ls150 Ls515 Ls800
        Public Scanner_Rear As Boolean              '		 Ls100 Ls150 Ls515 Ls800
        Public InkJet_Printer As Boolean             '		 Ls100 Ls150 Ls515 Ls800
        Public InkJet_HD_Printer_4_lines As Boolean  '			   Ls150 Ls515 Ls800
        Public Feeder As Boolean                    '		 Ls100
        Public Double_Leafing_sensor As Boolean     '					 Ls515
        Public Voiding_Front_Stamp As Boolean       '		 Ls100       Ls515
        Public Voiding_Rear_Stamp As Boolean        '					 Ls515
        Public No_Blanks As Boolean                 '					 Ls515
        Public Badge_Track123 As Boolean            '		 Ls100 Ls150
        Public Badge_Track12 As Boolean             '		 Ls100 Ls150 Ls515
        Public Badge_Track23 As Boolean             '		 Ls100 Ls150 Ls515
        Public OCR_Reader As Boolean                '		 Ls100
        Public Sorters_Nr As Integer                '						   Ls800
        Public Module_Encoder As Boolean            '						   Ls800
        Public Process_Card As Boolean              '	Ls40
        Public Capacitor As Boolean                 '	Ls40
        Public Scanner_UltraViolet As Boolean       '			   Ls150 Ls515
        Public Scanner_Color As Boolean             '			   Ls150
        Public Hight_Speed As Boolean               '			   Ls150
        Public Feeder_Motorized As Boolean          '			   Ls150
        Public Feeder_Electromagnet As Boolean      '			   Ls150
        Public ID_Card_Color As Boolean             '			   Ls150
        Public License_ClearPIX As Boolean          '	Ls40 Ls100 Ls150 Ls515
        Public License_2D_Barcode As Boolean        '	Ls40 Ls100 Ls150 Ls515
        Public License_IQA As Boolean               '	Ls40 Ls100 Ls150 Ls515
        Public License_Micro_Hole As Boolean        '	Ls40 Ls100 Ls150 Ls515
        Public Ghost_Reactor As Boolean             '		 Ls100
    End Structure

    Public Structure UNITSTATUS
        Public Size As Integer
        ' Size of the structure
        Public UnitStatus As Integer
        ' Ls40 Ls100 Ls150 Ls5xx Ls800
        Public Photo_Feeder As Boolean
        ' Ls40 Ls100 Ls150 Ls5xx Ls800
        Public Photo_Sorter As Boolean
        '      Ls100
        Public Photo_MICR As Boolean
        '      Ls100 Ls150 Ls5xx
        Public Photo_Path_Ls100 As Boolean
        '      Ls100
        Public Photo_Scanners As Boolean
        '      Ls100
        Public Unit_Just_ON As Boolean
        ' Ls40 Ls100 Ls150
        Public Photo_Double_Leafing_Down As Boolean
        '      Ls100 Ls150
        Public Photo_Double_Leafing_Middle As Boolean
        '            Ls150
        Public Photo_Double_Leafing_Up As Boolean
        '      Ls100 Ls150
        Public Photo_Card As Boolean
        '            Ls150
        Public Pockets_All_Full As Boolean
        '            Ls150 Ls5xx
        Public Photo_Stamp As Boolean
        '                  Ls5xx
        Public Photo_Exit As Boolean
        '                  Ls5xx
        Public Pocket_1_Full As Boolean
        '                  Ls5xx
        Public Pocket_2_Full As Boolean
        '                  Ls5xx
        Public Photo_Path_Feeder As Boolean
        '                        Ls800
        Public Photo_Path_Module_Begin As Boolean
        '                        Ls800
        Public Photo_Path_Binary_Rigth As Boolean
        '                        Ls800
        Public Photo_Path_Binary_Left As Boolean
        '                        Ls800
        Public Photo_Path_Module_End As Boolean
        '                        Ls800
        Public Sorter_1_input_pocket_1 As Boolean
        '                        Ls800
        Public Sorter_1_pocket_1_full As Boolean
        '                        Ls800
        Public Sorter_1_input_pocket_2 As Boolean
        '                        Ls800
        Public Sorter_1_pocket_2_full As Boolean
        '                        Ls800
        Public Sorter_1_input_pocket_3 As Boolean
        '                        Ls800
        Public Sorter_1_pocket_3_full As Boolean
        '                        Ls800
        Public Sorter_2_input_pocket_1 As Boolean
        '                        Ls800
        Public Sorter_2_pocket_1_full As Boolean
        '                        Ls800
        Public Sorter_2_input_pocket_2 As Boolean
        '                        Ls800
        Public Sorter_2_pocket_2_full As Boolean
        '                        Ls800
        Public Sorter_2_input_pocket_3 As Boolean
        '                        Ls800
        Public Sorter_2_pocket_3_full As Boolean
        '                        Ls800
        Public Sorter_3_input_pocket_1 As Boolean
        '                        Ls800
        Public Sorter_3_pocket_1_full As Boolean
        '                        Ls800
        Public Sorter_3_input_pocket_2 As Boolean
        '                        Ls800
        Public Sorter_3_pocket_2_full As Boolean
        '                        Ls800
        Public Sorter_3_input_pocket_3 As Boolean
        '                        Ls800
        Public Sorter_3_pocket_3_full As Boolean
        '                        Ls800
        Public Sorter_4_input_pocket_1 As Boolean
        '                        Ls800
        Public Sorter_4_pocket_1_full As Boolean
        '                        Ls800
        Public Sorter_4_input_pocket_2 As Boolean
        '                        Ls800
        Public Sorter_4_pocket_2_full As Boolean
        '                        Ls800
        Public Sorter_4_input_pocket_3 As Boolean
        '                        Ls800
        Public Sorter_4_pocket_3_full As Boolean
        '                        Ls800
        Public Sorter_5_input_pocket_1 As Boolean
        '                        Ls800
        Public Sorter_5_pocket_1_full As Boolean
        '                        Ls800
        Public Sorter_5_input_pocket_2 As Boolean
        '                        Ls800
        Public Sorter_5_pocket_2_full As Boolean
        '                        Ls800
        Public Sorter_5_input_pocket_3 As Boolean
        '                        Ls800
        Public Sorter_5_pocket_3_full As Boolean
        '                        Ls800
        Public Sorter_6_input_pocket_1 As Boolean
        '                        Ls800
        Public Sorter_6_pocket_1_full As Boolean
        '                        Ls800
        Public Sorter_6_input_pocket_2 As Boolean
        '                        Ls800
        Public Sorter_6_pocket_2_full As Boolean
        '                        Ls800
        Public Sorter_6_input_pocket_3 As Boolean
        '                        Ls800
        Public Sorter_6_pocket_3_full As Boolean
        '                        Ls800
        Public Sorter_7_input_pocket_1 As Boolean
        '                        Ls800
        Public Sorter_7_pocket_1_full As Boolean
        '                        Ls800
        Public Sorter_7_input_pocket_2 As Boolean
        '                        Ls800
        Public Sorter_7_pocket_2_full As Boolean
        '                        Ls800
        Public Sorter_7_input_pocket_3 As Boolean
        '                        Ls800
        Public Sorter_7_pocket_3_full As Boolean
        '                        Ls800
        Public Photo_Trigger As Boolean
        ' Ls40
        Public Document_Retained As Boolean
        ' Ls40
    End Structure

    Public Structure UNITHISTORY
        Public Size As Int32
        ' Size of the structure
        Public doc_sorted As UInt32
        ' Document sortered
        Public doc_retained As UInt32
        ' Nr. of document retained
        Public doc_retained_micr As UInt32
        ' Nr. documents retained after MICR header
        Public doc_retained_scan As UInt32
        ' Nr. documents retained after front scanning
        Public doc_ink_jet As UInt32
        ' Nr. of document printed
        Public doc_stamped As UInt32
        ' Nr. of document stamped
        Public tot_paper_jams As UInt32
        ' Totally of Paper jam
        Public jams_in_feeder As UInt32
        ' Nr. jam in the feeder
        Public jams_in_micr As UInt32
        ' Nr. jam during the MICR reading
        Public jams_scanner As UInt32
        ' Nr. jam between scanners
        Public jams_stamp As UInt32
        ' Nr. jam at stamp document
        Public jams_on_exit As UInt32
        ' Nr. jam after the film
        Public jams_card As UInt32
        ' Nr. jam in the card entry
        Public nr_double_leafing As UInt32
        ' Nr. double leafing occurs Ls800 only
        Public tot_doc_MICR_err As UInt32
        ' Totally MICR document, read with error
        Public doc_cmc7_err As UInt32
        ' Nr. of document CMC7, read with error
        Public doc_e13b_err As UInt32
        ' Nr. of document E13B, read with error
        Public doc_hw_barcode_err As UInt32
        ' Nr. of document Barcode, read from LS with error
        Public doc_hw_optic_err As UInt32
        ' Nr. of document OCR, read from LS with error
        Public num_turn_on As UInt32
        ' Nr. of power ON
        Public time_peripheral_on As UInt32
        ' Minutes peripheral time life
        ' Section specific Ls800 unit
        Public jam_front_scanner As UInt32
        ' Jam in scanner front
        Public jam_track_left As UInt32
        ' Jam in the left track
        Public jam_track_right As UInt32
        ' Jam in the right track
        Public jam_back_scanner As UInt32
        ' Jam in scanner back
        Public jam_in_the_sorters As UInt32
        ' Jam in sorters track
        ' Section compiled only from Ls800 unit
        Public nr_drops_printed As UInt32
        ' Nr. drops printed
    End Structure

    Public Structure READOPTIONS
        Public PutBlanks As Integer
        ' 0 = CodeLIne whitout blans, 1 = CodeLine with 1 blanks
        Public TypeRead As Char
        ' 'N' for 1 type of CodeLine, 'X' for CodeLine E13B switch OCRB
    End Structure


    ' Parameter Peripheral Type
    Public Enum LsUnitType As Short
        LS_40_LSCONNECT = 39
        LS_40_USB = 40
        LS_100_LSCONNECT = 109
        LS_100_USB = 100
        LS_100_RS232 = 101
        LS_100_ETH = 110
        LS_150_LSCONNECT = 149
        LS_150_USB = 150
        LS_200_USB = 201
        LS_5xx_SCSI = 500
        LS_515_LSCONNECT = 501
        LS_515_USB = 502
        LS_520_USB = 520
        LS_800_USB = 801
    End Enum

    Public Enum Stamp As Short
        STAMP_NO = 0
        ' No stamp is done
        STAMP_FRONT = 1
        ' Stamp on front document
        STAMP_BACK = 2
        ' Stamp on rear document
        STAMP_FRONT_AND_BACK = 3
        ' Stamp front and rear document
    End Enum

    Public Enum PrintValidate As Short
        NO_PRINT_VALIDATE = 0
        ' No print is done
        PRINT_VALIDATE = 1
        ' Print done
        PRINT_LOGO = 4
        ' Print a logo only
        PRINT_VALIDATE_WITH_LOGO = 5
        ' Print logo and lines
    End Enum

    Public Enum Feeder As Short
        FEED_AUTO = 0
        ' Start Document from Feeder
        FEED_FROM_PATH = 1
        ' Start Document from Unit Path
    End Enum

    Public Enum Sorter As Short
        SORTER_DOC_HOLDED = 0
        SORTER_POCKET_1 = 1
        SORTER_POCKET_2 = 2
        SORTER_AUTOMATIC = 3
        SORTER_SWICTH_1_TO_2 = 4
        SORTER_DOC_EJECTED = 5
        SORTER_ON_CODELINE_CALLBACK = 6

        ' For Ls800 unit
        SORTER_CIRCULAR = 48
        SORTER_SEQUENTIAL = 49
        SORTER_POCKET_0_SELECTED = 50
        SORTER_POCKET_1_SELECTED = 51
        SORTER_POCKET_2_SELECTED = 52
        SORTER_POCKET_3_SELECTED = 53
        SORTER_POCKET_4_SELECTED = 54
        SORTER_POCKET_5_SELECTED = 55
        SORTER_POCKET_6_SELECTED = 56
        SORTER_POCKET_7_SELECTED = 57
        SORTER_POCKET_8_SELECTED = 58
        SORTER_POCKET_9_SELECTED = 59
        SORTER_POCKET_10_SELECTED = 60
        SORTER_POCKET_11_SELECTED = 61
        SORTER_POCKET_12_SELECTED = 62
        SORTER_POCKET_13_SELECTED = 63
        SORTER_POCKET_14_SELECTED = 64
        SORTER_POCKET_15_SELECTED = 65
        SORTER_POCKET_16_SELECTED = 66
        SORTER_POCKET_17_SELECTED = 67
        SORTER_POCKET_18_SELECTED = 68
        SORTER_POCKET_19_SELECTED = 69
        SORTER_POCKET_20_SELECTED = 70
        SORTER_POCKET_21_SELECTED = 71
    End Enum

    Public Enum CodeLineType As Byte
        NO_READ_CODELINE = 0
        READ_CODELINE_HW_MICR = 1
        READ_CODELINE_E13B_MICR_WITH_OCR = 15

        READ_CODELINE_SW_OCRA = 65
        ''A',
        READ_CODELINE_SW_OCRB_NUM = 66
        ''B',
        READ_CODELINE_SW_OCRB_ALFANUM = 67
        ''C',
        READ_CODELINE_SW_OCRB_ITALY = 70
        ''F',
        READ_CODELINE_SW_E13B = 69
        ''E',
        READ_CODELINE_SW_E13B_X_OCRB = 88
        ''X',
        READ_BARCODE_2_OF_5 = 50
        READ_BARCODE_CODE39 = 51
        READ_BARCODE_CODE128 = 52
        '			READ_BARCODE_EAN13 = 53,

        MAX_CODE_LINE_LENGTH = 254
    End Enum

    Public Enum UnitMeasure As Short
        UNIT_MM = 0
        UNIT_INCH = 1
    End Enum

    Public Class OcrHeight
        Public Const OCR_MAX_HEIGHT_IN_MM As Double = 10.5
        Public Const OCR_MAX_HEIGHT_IN_INCH As Double = 0.41
    End Class

    Public Enum BlankInCodeline As Short
        BLANK_IN_CODELINE_NO = 0
        BLANK_IN_CODELINE_YES = 1
    End Enum

    Public Enum OriginOCR As Short
        ORIGIN_BOTTOM_RIGHT_MM = 10
        ORIGIN_BOTTOM_RIGHT_INCH = 20
    End Enum

    Public Enum ScanMode As Short
        SCAN_MODE_BW = 1
        SCAN_MODE_16_GRAY_100 = 2
        SCAN_MODE_16_GRAY_200 = 3
        SCAN_MODE_256_GRAY_100 = 4
        SCAN_MODE_256_GRAY_200 = 5
        SCAN_MODE_COLOR_100 = 10
        SCAN_MODE_COLOR_200 = 11
        SCAN_MODE_16_GRAY_300 = 20
        SCAN_MODE_256_GRAY_300 = 21
        SCAN_MODE_COLOR_300 = 22
        SCAN_MODE_256GR100_AND_UV = 40
        SCAN_MODE_256GR200_AND_UV = 41
        SCAN_MODE_256GR300_AND_UV = 42
    End Enum

    Public Enum ScanDocType As Short
        SCAN_PAPER_DOCUMENT = 0
        SCAN_CARD = 1
    End Enum

    Public Enum BWMethodType As Short

        ALGORITHM_CTS = 4
        ALGORITHM_CTS_CLEAR_PIX = 9
    End Enum

    Public Enum Side As Short
        SIDE_NONE_IMAGE = 78
        ''N',
        SIDE_FRONT_IMAGE = 70
        ''F',
        SIDE_BACK_IMAGE = 66
        ''B',
        SIDE_ALL_IMAGE = 88
        ''X',
        SIDE_FRONT_UV = 85
        ''U',
        SIDE_FRONT_MERGED = 77
        ''M',
    End Enum

    Public Enum Wait As Short
        WAIT_NO = 71
        ''G',
        WAIT_YES = 87
        ''W',
    End Enum

    Public Enum Beep As Short
        BEEP_NO = 0
        BEEP_YES = 1
    End Enum

    Public Enum ClearBlack As Short
        CLEAR_BLACK_NO = 0
        CLEAR_BLACK_YES = 1
        CLEAR_AND_ALIGN_IMAGE = 2
    End Enum

    Public Enum PrintFont As Byte
        PRINT_NO_STRING = 0
        PRINT_FONT_NORMAL = 78
        ''N',
        PRINT_FONT_BOLD = 66
        ''B',
        PRINT_FONT_NORMAL_15 = 65
        ''A',
        PRINT_UP_FONT_NORMAL = 110
        ''n',
        PRINT_UP_FONT_BOLD = 98
        ''b',
        PRINT_UP_FONT_NORMAL_15_CHAR = 97
        ''a',
    End Enum

    Public Enum DoubleLeafing As Short

        DOUBLE_LEAFING_WARNING = 0
        DOUBLE_LEAFING_ERROR = 1
        'DOUBLE_LEAFING_LEVEL1 = 1,non lo uso piu'
        DOUBLE_LEAFING_LEVEL2 = 2
        DOUBLE_LEAFING_LEVEL3 = 3
        DOUBLE_LEAFING_DEFAULT = 4
        DOUBLE_LEAFING_LEVEL4 = 5
        DOUBLE_LEAFING_LEVEL5 = 6
        DOUBLE_LEAFING_DISABLE = 7
    End Enum

    Public Enum Reset As Short
        RESET_ERROR = 48
        ''0',
        RESET_PATH = 49
        ''1',
        RESET_BELT_CLEANING = 50
        ''2',
    End Enum

    Public Enum ImageSave As Short
        IMAGE_SAVE_ON_FILE = 4
        IMAGE_SAVE_HANDLE = 5
        IMAGE_SAVE_BOTH = 6
        IMAGE_SAVE_NONE = 7
    End Enum

    Public Enum FileType As Short
        FILE_JPEG = 10
        FILE_BMP = 11
        FILE_TIF = 3
        FILE_CCITT = 25
        FILE_CCITT_GROUP3_1DIM = 27
        FILE_CCITT_GROUP3_2DIM = 28
        FILE_CCITT_GROUP4 = 29
    End Enum

    Public Enum FileAttribute As Short
        SAVE_OVERWRITE = 0
        SAVE_APPEND = 1
        SAVE_REPLACE = 2
        SAVE_INSERT = 3
    End Enum

    Public Enum LsSpeed As Short
        SPEED_DEFAULT = 0
        SPEED_STAMP = 1
    End Enum

    Public Enum Badge As Short
        BADGE_READ_TRACK_1 = &H20
        BADGE_READ_TRACK_2 = &H40
        BADGE_READ_TRACK_3 = &H80
        BADGE_READ_TRACKS_1_2 = &H60
        BADGE_READ_TRACKS_2_3 = &HC0
        BADGE_READ_TRACKS_1_2_3 = &HE0
    End Enum

    Public Class LsReply
        ' ------------------------------------------------------------------------
        '                          REPLY-CODE
        ' ------------------------------------------------------------------------
        Public Const LS_OKAY As Integer = 0


        ' ------------------------------------------------------------------------
        '                  ERRORS
        ' ------------------------------------------------------------------------
        Public Const LS_SYSTEM_ERROR As Integer = -1
        Public Const LS_USB_ERROR As Integer = -2
        Public Const LS_PERIPHERAL_NOT_FOUND As Integer = -3
        Public Const LS_HARDWARE_ERROR As Integer = -4
        Public Const LS_PERIPHERAL_OFF_ON As Integer = -5
        Public Const LS_RESERVED_ERROR As Integer = -6
        Public Const LS_PAPER_JAM As Integer = -7
        Public Const LS_TARGET_BUSY As Integer = -8
        Public Const LS_INVALID_COMMAND As Integer = -9
        Public Const LS_DATA_LOST As Integer = -10
        Public Const LS_COMMAND_IN_EXECUTION_YET As Integer = -11
        Public Const LS_JPEG_ERROR As Integer = -12
        Public Const LS_COMMAND_SEQUENCE_ERROR As Integer = -13
        Public Const LS_PC_HW_ERROR As Integer = -14
        Public Const LS_IMAGE_OVERWRITE As Integer = -15
        Public Const LS_INVALID_HANDLE As Integer = -16
        Public Const LS_NO_LIBRARY_LOAD As Integer = -17
        Public Const LS_BMP_ERROR As Integer = -18
        Public Const LS_TIFF_ERROR As Integer = -19
        Public Const LS_IMAGE_NO_MORE_AVAILABLE As Integer = -20
        Public Const LS_IMAGE_NO_FILMED As Integer = -21
        Public Const LS_IMAGE_NOT_PRESENT As Integer = -22
        Public Const LS_FUNCTION_NOT_AVAILABLE As Integer = -23
        Public Const LS_DOCUMENT_NOT_SUPPORTED As Integer = -24
        Public Const LS_BARCODE_ERROR As Integer = -25
        Public Const LS_INVALID_LIBRARY As Integer = -26
        Public Const LS_INVALID_IMAGE As Integer = -27
        Public Const LS_INVALID_IMAGE_FORMAT As Integer = -28
        Public Const LS_INVALID_BARCODE_TYPE As Integer = -29
        Public Const LS_OPEN_NOT_DONE As Integer = -30
        Public Const LS_INVALID_TYPE_COMMAND As Integer = -31
        Public Const LS_INVALID_CLEARBLACK As Integer = -32
        Public Const LS_INVALID_SIDE As Integer = -33
        Public Const LS_MISSING_IMAGE As Integer = -34
        Public Const LS_INVALID_TYPE As Integer = -35
        Public Const LS_INVALID_SAVEMODE As Integer = -36
        Public Const LS_INVALID_PAGE_NUMBER As Integer = -37
        Public Const LS_INVALID_NRIMAGE As Integer = -38
        Public Const LS_INVALID_STAMP As Integer = -39
        Public Const LS_INVALID_WAITTIMEOUT As Integer = -40
        Public Const LS_INVALID_VALIDATE As Integer = -41
        Public Const LS_INVALID_CODELINE_TYPE As Integer = -42
        Public Const LS_MISSING_NRIMAGE As Integer = -43
        Public Const LS_INVALID_SCANMODE As Integer = -44
        Public Const LS_INVALID_BEEP As Integer = -45
        Public Const LS_INVALID_FEEDER As Integer = -46
        Public Const LS_INVALID_SORTER As Integer = -47
        Public Const LS_INVALID_BADGE_TRACK As Integer = -48
        Public Const LS_MISSING_FILENAME As Integer = -49
        Public Const LS_INVALID_QUALITY As Integer = -50
        Public Const LS_INVALID_FILEFORMAT As Integer = -51
        Public Const LS_INVALID_COORDINATE As Integer = -52
        Public Const LS_MISSING_HANDLE_VARIABLE As Integer = -53
        Public Const LS_INVALID_POLO_FILTER As Integer = -54
        Public Const LS_INVALID_ORIGIN_MEASURES As Integer = -55
        Public Const LS_INVALID_SIZEH_VALUE As Integer = -56
        Public Const LS_INVALID_FORMAT As Integer = -57
        Public Const LS_STRINGS_TOO_LONGS As Integer = -58
        Public Const LS_READ_IMAGE_FAILED As Integer = -59
        Public Const LS_INVALID_CMD_HISTORY As Integer = -60
        Public Const LS_MISSING_BUFFER_HISTORY As Integer = -61
        Public Const LS_INVALID_ANSWER As Integer = -62
        Public Const LS_OPEN_FILE_ERROR_OR_NOT_FOUND As Integer = -63
        Public Const LS_READ_TIMEOUT_EXPIRED As Integer = -64
        Public Const LS_INVALID_METHOD As Integer = -65
        Public Const LS_CALIBRATION_FAILED As Integer = -66
        Public Const LS_INVALID_SAVEIMAGE As Integer = -67
        Public Const LS_INVALID_UNIT As Integer = -68
        Public Const LS_INVALID_NRWINDOWS As Integer = -71
        Public Const LS_INVALID_VALUE As Integer = -72
        Public Const LS_ILLEGAL_REQUEST As Integer = -73
        Public Const LS_INVALID_NR_CRITERIA As Integer = -74
        Public Const LS_MISSING_CRITERIA_STRUCTURE As Integer = -75
        Public Const LS_INVALID_MOVEMENT As Integer = -76
        Public Const LS_INVALID_DEGREE As Integer = -77
        Public Const LS_R0TATE_ERROR As Integer = -78
        Public Const LS_MICR_VALUE_OUT_OF_RANGE As Integer = -79
        Public Const LS_PERIPHERAL_RESERVED As Integer = -80
        Public Const LS_INVALID_NCHANGE As Integer = -81
        Public Const LS_BRIGHTNESS_ERROR As Integer = -82
        Public Const LS_CONTRAST_ERROR As Integer = -83
        Public Const LS_INVALID_SIDETOPRINT As Integer = -84
        Public Const LS_DOUBLE_LEAFING_ERROR As Integer = -85
        Public Const LS_INVALID_BADGE_TIMEOUT As Integer = -86
        Public Const LS_INVALID_RESET_TYPE As Integer = -87
        Public Const LS_MISSING_SET_CALLBACK As Integer = -88
        Public Const LS_IMAGE_NOT_200_DPI As Integer = -89
        Public Const LS_DOWNLOAD_ERROR As Integer = -90
        Public Const LS_INVALID_SORT_ON_CHOICE As Integer = -91
        Public Const LS_INVALID_FONT As Integer = -92
        Public Const LS_INVALID_UNIT_SPEED As Integer = -93
        Public Const LS_INVALID_LENGTH As Integer = -94
        Public Const LS_SHORT_PAPER As Integer = -95
        Public Const LS_INVALID_DOC_LENGTH As Integer = -96
        Public Const LS_INVALID_DOCSLONG As Integer = -97
        Public Const LS_IMAGE_NOT_256_COLOR As Integer = -98
        Public Const LS_BATTERY_NOT_CHARGED As Integer = -99
        Public Const LS_INVALID_SCAN_DOC_TYPE As Integer = -100
        Public Const LS_ILLEGAL_SCAN_CARD_SPEED As Integer = -101
        Public Const LS_INVALID_PWM_VALUE As Integer = -102
        Public Const LS_INVALID_KEY_LENGTH As Integer = -103
        Public Const LS_INVALID_PASSWORD As Integer = -104
        Public Const LS_UNIT_LOCKED As Integer = -105
        Public Const LS_INVALID_IMAGEFORMAT As Integer = -106
        Public Const LS_INVALID_THRESHOLD As Integer = -107
        Public Const LS_NO_START_FOR_SORTER_FULL As Integer = -108
        Public Const LS_IPBOX_ADDRESS_NOT_FOUNDED As Integer = -109
        Public Const LS_INVALID_LED_COMMAND As Integer = -110
        Public Const LS_INVALID_COLOR_PARAMETER As Integer = -111

        Public Const LS_JAM_AT_MICR_PHOTO As Integer = -201
        Public Const LS_JAM_DOC_TOO_LONG As Integer = -202
        Public Const LS_JAM_AT_SCANNER_PHOTO As Integer = -203

        Public Const LS_SCAN_NETTO_IMAGE_NOT_SUPPORTED As Integer = -521
        Public Const LS_256_GRAY_NOT_SUPPORTED As Integer = -522
        Public Const LS_INVALID_PATH As Integer = -523
        Public Const LS_MISSING_CALLBACK_FUNCTION As Integer = -526
        Public Const LS_INVALID_OCR_IMAGE_SIDE As Integer = -558
        Public Const LS_PERIPHERAL_NOT_ANSWER As Integer = -599

        Public Const LS_INVALID_CONNECTION_HANDLE As Integer = -1000
        Public Const LS_INVALID_CONNECT_PERIPHERAL As Integer = -1001
        Public Const LS_PERIPHERAL_NOT_YET_INTEGRATE As Integer = -1002
        Public Const LS_UNKNOW_PERIPHERAL_REPLY As Integer = -1003
        Public Const LS_CODELINE_ALREADY_DEFINED As Integer = -1004
        Public Const LS_INVALID_NUMBER_OF_DOC As Integer = -1005

        Public Const LS_DECODE_FONT_NOT_PRESENT As Integer = -1101
        Public Const LS_DECODE_INVALID_COORDINATE As Integer = -1102
        Public Const LS_DECODE_INVALID_OPTION As Integer = -1103
        Public Const LS_DECODE_INVALID_CODELINE_TYPE As Integer = -1104
        Public Const LS_DECODE_SYSTEM_ERROR As Integer = -1105
        Public Const LS_DECODE_DATA_TRUNC As Integer = -1106
        Public Const LS_DECODE_INVALID_BITMAP As Integer = -1107
        Public Const LS_DECODE_ILLEGAL_USE As Integer = -1108

        Public Const LS_BARCODE_GENERIC_ERROR As Integer = -1201
        Public Const LS_BARCODE_NOT_DECODABLE As Integer = -1202
        Public Const LS_BARCODE_OPENFILE_ERROR As Integer = -1203
        Public Const LS_BARCODE_READBMP_ERROR As Integer = -1204
        Public Const LS_BARCODE_MEMORY_ERROR As Integer = -1205
        Public Const LS_BARCODE_START_NOTFOUND As Integer = -1206
        Public Const LS_BARCODE_STOP_NOTFOUND As Integer = -1207

        Public Const LS_PDF_NOT_DECODABLE As Integer = -1301
        Public Const LS_PDF_READBMP_ERROR As Integer = -1302
        Public Const LS_PDF_BITMAP_FORMAT_ERROR As Integer = -1303
        Public Const LS_PDF_MEMORY_ERROR As Integer = -1304
        Public Const LS_PDF_START_NOTFOUND As Integer = -1305
        Public Const LS_PDF_STOP_NOTFOUND As Integer = -1306
        Public Const LS_PDF_LEFTIND_ERROR As Integer = -1307
        Public Const LS_PDF_RIGHTIND_ERROR As Integer = -1308
        Public Const LS_PDF_OPENFILE_ERROR As Integer = -1309


        ' ------------------------------------------------------------------------
        '                  WARNINGS
        ' ------------------------------------------------------------------------
        Public Const LS_FEEDER_EMPTY As Integer = 1
        Public Const LS_DATA_TRUNCATED As Integer = 2
        Public Const LS_DOC_PRESENT As Integer = 3
        Public Const LS_BADGE_TIMEOUT As Integer = 4
        Public Const LS_ALREADY_OPEN As Integer = 5
        Public Const LS_PERIPHERAL_BUSY As Integer = 6
        Public Const LS_DOUBLE_LEAFING_WARNING As Integer = 7
        Public Const LS_COMMAND_NOT_ENDED As Integer = 8
        Public Const LS_RETRY As Integer = 9
        Public Const LS_NO_OTHER_DOCUMENT As Integer = 10
        Public Const LS_QUEUE_FULL As Integer = 11
        Public Const LS_NO_SENSE As Integer = 12
        Public Const LS_TRY_TO_RESET As Integer = 14
        Public Const LS_STRING_TRUNCATED As Integer = 15
        Public Const LS_COMMAND_NOT_SUPPORTED As Integer = 19
        Public Const LS_SORTER1_FULL As Integer = 35
        Public Const LS_SORTER2_FULL As Integer = 36
        Public Const LS_SORTERS_BOTH_FULL As Integer = 37
        Public Const LS_KEEP_DOC_ON_CODELINE_ERROR As Integer = 39
        Public Const LS_LOOP_INTERRUPTED As Integer = 40

        Public Const LS_SORTER_1_POCKET_1_FULL As Integer = 51
        Public Const LS_SORTER_1_POCKET_2_FULL As Integer = 52
        Public Const LS_SORTER_1_POCKET_3_FULL As Integer = 53
        Public Const LS_SORTER_2_POCKET_1_FULL As Integer = 54
        Public Const LS_SORTER_2_POCKET_2_FULL As Integer = 55
        Public Const LS_SORTER_2_POCKET_3_FULL As Integer = 56
        Public Const LS_SORTER_3_POCKET_1_FULL As Integer = 57
        Public Const LS_SORTER_3_POCKET_2_FULL As Integer = 58
        Public Const LS_SORTER_3_POCKET_3_FULL As Integer = 59
        Public Const LS_SORTER_4_POCKET_1_FULL As Integer = 60
        Public Const LS_SORTER_4_POCKET_2_FULL As Integer = 61
        Public Const LS_SORTER_4_POCKET_3_FULL As Integer = 62
        Public Const LS_SORTER_5_POCKET_1_FULL As Integer = 63
        Public Const LS_SORTER_5_POCKET_2_FULL As Integer = 64
        Public Const LS_SORTER_5_POCKET_3_FULL As Integer = 65
        Public Const LS_SORTER_6_POCKET_1_FULL As Integer = 66
        Public Const LS_SORTER_6_POCKET_2_FULL As Integer = 67
        Public Const LS_SORTER_6_POCKET_3_FULL As Integer = 68
        Public Const LS_SORTER_7_POCKET_1_FULL As Integer = 69
        Public Const LS_SORTER_7_POCKET_2_FULL As Integer = 70
        Public Const LS_SORTER_7_POCKET_3_FULL As Integer = 71
    End Class
End Class

