Public Class Properties
    Protected m_Values As Hashtable

    Public Const SCAN_FUNC As String = "Scan Func"
    Public Const SCAN_FRONT As String = "Scan Front"
    Public Const FRONT_GRAYSCALE As String = "Front Grayscale"
    Public Const FRONT_DISPLAY As String = "Front Display"
    Public Const FRONT_SAVE As String = "Front Save"
    Public Const SCAN_BACK As String = "Scan Back"
    Public Const BACK_GRAYSCALE As String = "Back Grayscale"
    Public Const BACK_DISPLAY As String = "Back Display"
    Public Const BACK_SAVE As String = "Back Save"
    Public Const MICR As String = "MICR"
    Public Const MICR_SAVE As String = "Micr Save"
    Public Const MICR_SAVE_ENABLE As String = "Micr Save Enable"
    Public Const MICR_FONT As String = "Micr Font"
    Public Const ELEC_ENDORSE_TEXT As String = "Electronic Endorse Text"
    Public Const ELEC_ENDORSE_IMAGE As String = "Electronic Endorse Image"
    Public Const MIS_INSERT_DETECT As String = "MisInsertionErrorDetect"
    Public Const MIS_INSERT_EJECT As String = "MisInsertionErrorEject"
    Public Const MIS_INSERT_STAMP As String = "MisInsertionStamp"
    Public Const MIS_INSERT_CANCEL As String = "MisInsertionCancel"
    Public Const NOISE_DETECT As String = "NoiseErrorDetect"
    Public Const NOISE_EJECT As String = "NoiseErrorEject"
    Public Const NOISE_STAMP As String = "NoiseStamp"
    Public Const NOISE_CANCEL As String = "NoiseCancel"
    Public Const DOUBLE_FEED_DETECT As String = "DoubleFeedErrorDetect"
    Public Const DOUBLE_FEED_EJECT As String = "DoubleFeedErrorEject"
    Public Const DOUBLE_FEED_STAMP As String = "DoubleFeedStamp"
    Public Const DOUBLE_FEED_CANCEL As String = "DoubleFeedCancel"
    Public Const BADDATA_COUNT As String = "BaddataCount"
    Public Const BADDATA_DETECT As String = "BaddataErrorDetect"
    Public Const BADDATA_EJECT As String = "BaddataErrorEject"
    Public Const BADDATA_STAMP As String = "BaddataStamp"
    Public Const BADDATA_CANCEL As String = "BaddataCancel"
    Public Const NODATA_DETECT As String = "NodataErrorDetect"
    Public Const NODATA_EJECT As String = "NodataErrorEject"
    Public Const NODATA_STAMP As String = "NodataStamp"
    Public Const NODATA_CANCEL As String = "NodataCancel"
    Public Const CONFIRMATION As String = "Confirmation Mode"
    Public Const CONF_EJECT As String = "Confirmation Eject"
    Public Const CONF_STAMP As String = "Confirmation Stamp"
    Public Const CONF_NEXT_CHECK As String = "Confirmation NextCheck"
    Public Const CONF_OK As String = "Confirmation OK"
    Public Const RUN_SCN_TO_RESULT As String = "Run ScanToResult"
    Public Const NO_CALL_SCN_TO_RESULT As String = "No Call ScnToResult"
    Public Const OCR_AB As String = "Ocr Ab"
    Public Const OCR_AB_FONT As String = "Ocr Ab Font"
    Public Const BUZZER_SUCCESS_HZ As String = "Buzzer Success Hz"
    Public Const BUZZER_SUCCESS_COUNT As String = "Buzzer Success Count"
    Public Const BUZZER_ERROR_HZ As String = "Buzzer Error Hz"
    Public Const BUZZER_ERROR_COUNT As String = "Buzzer Error Count"
    Public Const BUZZER_WFEED_HZ As String = "Buzzer WFeed Hz"
    Public Const BUZZER_WFEED_COUNT As String = "Buzzer WFeed Count"

    Public Sub New()
        ' this is where we define our default properties
        ' except for this there is no real advantage to having this class defined
        m_Values = New Hashtable
        m_Values.Add(SCAN_FUNC, 0)

        m_Values.Add(SCAN_FRONT, True)
        m_Values.Add(FRONT_GRAYSCALE, True)
        m_Values.Add(FRONT_DISPLAY, True)
        m_Values.Add(FRONT_SAVE, False)

        m_Values.Add(SCAN_BACK, True)
        m_Values.Add(BACK_GRAYSCALE, True)
        m_Values.Add(BACK_DISPLAY, True)
        m_Values.Add(BACK_SAVE, False)

        m_Values.Add(MICR, True)
        m_Values.Add(MICR_SAVE, False)
        m_Values.Add(MICR_FONT, 0)
        m_Values.Add(MICR_SAVE_ENABLE, True)

        m_Values.Add(ELEC_ENDORSE_TEXT, False)
        m_Values.Add(ELEC_ENDORSE_IMAGE, False)

        m_Values.Add(CONFIRMATION, True)
        m_Values.Add(RUN_SCN_TO_RESULT, True)

        m_Values.Add(MIS_INSERT_DETECT, True)
        m_Values.Add(MIS_INSERT_EJECT, 1)
        m_Values.Add(MIS_INSERT_STAMP, False)
        m_Values.Add(MIS_INSERT_CANCEL, False)

        m_Values.Add(NOISE_DETECT, True)
        m_Values.Add(NOISE_EJECT, 1)
        m_Values.Add(NOISE_STAMP, False)
        m_Values.Add(NOISE_CANCEL, True)

        m_Values.Add(DOUBLE_FEED_DETECT, True)
        m_Values.Add(DOUBLE_FEED_EJECT, 1)
        m_Values.Add(DOUBLE_FEED_STAMP, False)
        m_Values.Add(DOUBLE_FEED_CANCEL, False)

        m_Values.Add(BADDATA_COUNT, 255)
        m_Values.Add(BADDATA_DETECT, True)
        m_Values.Add(BADDATA_EJECT, 1)
        m_Values.Add(BADDATA_STAMP, False)
        m_Values.Add(BADDATA_CANCEL, False)

        m_Values.Add(NODATA_DETECT, True)
        m_Values.Add(NODATA_EJECT, 1)
        m_Values.Add(NODATA_STAMP, False)
        m_Values.Add(NODATA_CANCEL, False)

        m_Values.Add(CONF_EJECT, 1)
        m_Values.Add(CONF_STAMP, False)
        m_Values.Add(CONF_NEXT_CHECK, 0)
        m_Values.Add(CONF_OK, True)

        m_Values.Add(OCR_AB, False)
        m_Values.Add(OCR_AB_FONT, 0)

        m_Values.Add(BUZZER_SUCCESS_HZ, 0)
        m_Values.Add(BUZZER_SUCCESS_COUNT, 0)
        m_Values.Add(BUZZER_ERROR_HZ, 0)
        m_Values.Add(BUZZER_ERROR_COUNT, 0)
        m_Values.Add(BUZZER_WFEED_HZ, 0)
        m_Values.Add(BUZZER_WFEED_COUNT, 0)
    End Sub

    Public Sub New(ByVal source As Properties)
        m_Values = source.m_Values.Clone
    End Sub

    ' Binding acquisition of each value
    Default Property Value(ByVal name As String) As Integer
        Get
            ' If name agrees return the value
            If (m_Values.Contains(name)) Then
                Return m_Values.Item(name)
            End If
            Return False
        End Get
        Set(ByVal Value As Integer)
            ' If name agrees I renew the value
            If (m_Values.Contains(name)) Then
                m_Values.Item(name) = Value
            ElseIf Value Then
                ' It adds it with the value, if name is not discovered
                m_Values.Add(name, Value)
            End If
        End Set
    End Property

End Class
