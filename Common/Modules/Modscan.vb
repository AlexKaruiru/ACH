Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.OracleClient
Imports System.Windows.Forms
Imports System.Drawing
Imports System.IO
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Collections
Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports System.Runtime.InteropServices.COMException
Imports System.Security.Cryptography
Imports prjDataAccess
Imports BRSecurity
Imports System.Windows.Media
Imports System.Configuration
Imports BRClearingEncryptDecrypt
Imports BRNetSecurity
Imports BR.Configuration
Imports BrDataEncryption
Imports BRCoreEntities.SystemBranchStatus
Imports BR.ApplicationBlocks.Data
Imports BREntities.SystemBranchStatus
Imports BREntities.SystemBranchSettings
Imports BR.DBClient
Imports BREntities
Imports BRCheckSum
Imports BR
Imports BREntities.ClearingFileFormat
Imports System.Text.RegularExpressions
Imports BRBase
Imports BRCoreEntities.SystemBranchSettings
Imports System.Threading

Public Class Modscan
    Public Shared TImagePath As String = ""
    Private bm As Bitmap
    Private Shared insertMutex As Mutex = New Mutex()
    Private Shared ClrHSID As String = "99"
    Public Shared ImgTest As Bitmap
    Public Shared ImgTest1 As Bitmap
    Public Shared ImgTest2 As Bitmap
    Public Const cNo_Text As String = ""
    Public Shared MsgBoxTitle As String = "BRClearing"
    Public Shared OurBankID As String
    Public Shared MTSServer As String
    Public Shared BankName As String
    Public Shared CodeLineDetails As MICRLINE
    Public Shared OurBranchID As String ' Here it is logged in branch ID
    Public Shared BranchName As String ' Logged in branch name
    Public Shared MicrEditted As Boolean = False
    Public Shared theForm As String = ""
    Public Shared isMDV As Boolean = False
    Public Shared MDVMicr As String = ""
    Public Shared IsTypeCCheque As Boolean = False
    Public Shared BankIDSize As Int16 = 2
    Dim img As Bitmap
    Public Shared FILE_NAME As String = App_Path()
    Public Shared FrontImgx As Bitmap
    Public Shared FrontImg2x As Bitmap
    Public Shared mdvReturnCode As String = ""
    Dim imgFormat As System.Drawing.Imaging.ImageFormat
    Private strFileExt() As String = {".jpg", ".tif", ".JPG", ".TIF"}
    Public Shared chqCaunt As Int32 = 0
    Public Shared FilePath As String = ""
    Public Shared myColSigns As New Collection
    Public Shared ReturnID As String
    Public Shared RowID As String
    Public Shared ReturnValue As String = ""
    Public Shared ReturnValue2 As String = ""
    Public Shared RetReasonCancel As Boolean = False
    Public Shared ModuleIDT As String = ""
    Public Shared Dir_Path As String = ""
    Public Shared isFcy As Boolean = False
    Public Shared CouID As Int16 = 0
    Public Shared strBatchPath As String = ""
    Public Shared strJavaExeInstallation As String = ""
    Public Shared strDSkeyFile As String = ""
    Public Shared keyPass As String = ""
    Public Shared LoginDate As String
    Public Shared WORKING_DATE As Date
    Public Shared FDATE As Date
    Public Shared ChequeIDLength As Int16 = 6
    Public Shared cWORKING_DATE As String = ""
    Public Shared CustomerDetails As udtCustomerDetails 'this is the variable to be used in customer searches
    Public Map As String
    Public Shared ModuleId As Short 'Gives the ModueId for the Report
    Public Shared strErrorLogPath As String = ""
    Public Shared LocalCurrency As String 'Local Currency /base currency name
    Public Shared ArchivesPath As String = ""
    Public Shared OldImageFormat As String = 1
    Public Shared FromSerialID As Integer 'Printing the one voucher / printing all selected voucher ,
    Public Shared ToSerialID As Integer 'if it is transfer print the whole voucher that includes all cedit and debit vouchers

    Public Shared strColumnIDs As String ' Used for comma seperated rowids used in printing Internal Transfer vouchers etc
    Public Shared FromAccountID As String
    Public Shared ToAccountID As String

    Public Shared FromReceiptID As String
    Public Shared ToReceiptID As String
    Public Shared ISize As Int32
    Public Shared FromProductID As String
    Public Shared ToProductID As String
    Shared FromGridRowCounter As Int32 = 0
    Shared ToGridRowCounter As Int32 = 0
    Public Shared AccountType As String
    Public Shared JRImageSize, TFImageSize, JFImageSize As String
    Public Shared FromDate As String
    Public Shared chDate As String
    Public Shared cSerialID As String
    Public Shared DBPassword As String '= '"friend" ' Server password
    Public Shared DatabaseName As String '= "CBL22092010" ' Database Name  ,In some cases we were using the diff diff database name in same server
    Public Shared DBServerName As String '= "CSWKS24" ' Name of the server that hosts the database
    Public Shared ReaderUsed As String '= 1 ' 0 - Panini, 1 - UV Reader(CTS)
    Public Shared MyBranchID As String '= "001" ' My Currenct BranchID
    Public Shared OperatorID As String '= ""
    Public Shared BRUserName As String
    Public Shared cMICRFCY As Int32
    Public Shared cAccountBranch As String
    Public Shared MICRChequeID As String
    Public Shared MICRBankID As String
    Public Shared MICRBranchID As String
    Public Shared MICRChequeDigit As String
    Public Shared MICRVoucherCode As String
    Public Shared MICRAccountID As String
    Public Shared MICRClearingCenter As String
    Public Shared MIRCAmount As Double
    'Private Shared strSystemD As BrWebDataEcryption
    Private Shared strPathEncpt As String
    Public Shared MICRRejectedReason As String
    Public Shared MICRFrontImgPath As String = ""
    Public Shared MICRFrontBWImgPath As String = ""
    Public Shared MICRBackImgPath As String = ""
    Public Shared MICRUVImagePath As String = ""
    Public Shared MICRHighValue As String
    Public Shared MICRClearingDays As String
    Public Shared MICRReturnCode As String
    Public Shared MICRChequeDate As DateTime
    Public Shared MICRDrawer As String
    Public Shared MICROurComm As String
    Public Shared MICRTheirComm As String
    Public Shared CountryCode As String = ""
    'Public rs As New ADODB.Recordset
    Public Shared dt As System.Data.DataTable
    ' ''Public cChqImageApplication As frmBROutwardClearing
    Public Shared strAccountID As String
    Public Shared strAccountName As String
    Public Shared chequeimagepath As String
    Public Shared bytMICRReaderConnectedOn As Byte
    Public Shared RejectedReason As String
    Public Shared ReaderCounter As Int32
    Public Shared chqCounter As Int32
    Public Shared pTrxType As String
    Public Shared cTrxType As String
    Public Shared pScan As String
    Public Shared cScan As String
    Public Shared pWorkingDate As String
    Public Shared cWorkingDate As String
    Public Shared pLocalCurrency As String
    Public Shared cLocalCurrency As String
    Public Shared pFromDate As String
    Public Shared cFromDate As String
    Public Shared pToDate As String
    Public Shared cToDate As String
    Public Shared BrReaderType As Int32
    Public Shared ReaderStatus As String
    Public Shared cMICRChequeID As String
    Public Shared cMICRBankID As String
    Public Shared cMICRBranchID As String
    Public Shared cMICRTheirAccountID As String
    Public Shared cTransactionID As String
    'Public cApplFunc As ApplClass
    ' Peripheral choose
    Public Shared LsUnitType As Integer = 0
    Public Shared ImagePath As String
    Public Shared ScanMode_Card As Int16
    Public Shared fAutoFeed As Boolean
    Public Shared NrDocToProcess As Short
    Public Shared NrTot_Pocket As Short
    Public Shared strImagePath As String = ""
    Public Shared MICRCodeline As String = ""
    Public Shared cDBType As String
    Public Shared pubSettFileName As String
    Public Shared tblAllData As New DataTable
    Public tblDataErrors As New DataTable
    Public Shared LastErrorMessage As String = ""
    Shared pubDataSqlCommand As New SqlClient.SqlCommand
    Shared pubDataSqlAdapter As New SqlClient.SqlDataAdapter
    Shared pubDbSqlConn As New SqlClient.SqlConnection
    Shared pubDataOraCommand As New OracleClient.OracleCommand
    Shared pubDataOraAdapter As New OracleClient.OracleDataAdapter
    Shared pubDbOraConn As New OracleClient.OracleConnection
    Shared pubDataSet As New DataSet
    Shared StrSql As String = ""
    Public Shared OracleConn As OracleConnection
    Public Shared SqlConn As SqlConnection
    Public Shared CnString As String = ""
    Public Shared publicDTbl As DataTable = Nothing
    Public Shared publicDset As DataSet = Nothing
    Public Shared myTempDataTbl As DataTable = Nothing
    Public Shared BRDbType As String = ""
    Public Shared dr As DataRow
    Public Shared strFrontFilename As String = ""
    Public Shared strBackFilename As String = ""
    Public Shared strFilename As String = ""
    Private height As Integer, width As Integer
    Public Shared boolGetImagesDetails As Boolean
    Public Shared SysType As Int16 = 3
    Public Shared intRow As Int16 = 0
    Public Shared PreAmt As Double = 0
    Public Shared PreCount As Int16 = 0
    Public Shared DiscAmt As Double = 0
    Public Shared DiscCnt As Int16 = 0
    Public Shared UnpAmt As Double = 0
    Public Shared UnpCnt As Int16 = 0
    Public Shared PreDebitAmt As Double = 0
    Public Shared PreDebitCount As Int16 = 0
    Public Shared SettlementDt As New DataTable
    Public Shared ErrorMsgsDt As New DataTable
    Public Shared pubDSet As New DataSet
    Public Shared Sess As Int16 = 1


    'Basics CountryCode, BranchCode, RegionalClearingCode, BankIDLength, BranchIDLenth
    Public Enum ENUM_Module_Called
        Outward_scan = 0 'For Scanning Outwards
        Inward_scan = 1 'For Scanning Inwards
        Search_Module = 2   'For Searching the Images
        Display_Signature = 3 'For Displaying Signature and Images
        Generate_OutFile = 4
        Read_IncomingFiles = 5
        Unpay = 6
        Represent_Cheque = 7
        Sign_The_File = 8
        View_Mandate_Images = 9
    End Enum
    Public Enum MandateFT
        NewDD = 1
        NewDDAcknowledgment = 2
        DDAmendmentRequest = 3
        DDAcknoledgement = 4
        CancelledDD = 5
    End Enum
    Public Enum ENUM_CountryCode
        Kenya = 1
        Uganda = 2
        Tanzania = 3
        Ethiopia = 4
        Malawi = 5
        Zambia = 6
        Swaziland = 7
        South_Sudan = 8
        South_Africa = 9
        Mozambique = 10
    End Enum
    Public Enum ENUM_SysType
        BR = 1
        BRMFO = 2
        BRNET = 3
        BRNETOLD = 4
    End Enum
    Public Enum ENUM_BankIDLenth
        Kenya = 2
        Uganda = 2
        Tanzania = 3
        Ethiopia = 2
    End Enum
    Public Enum ENUM_BranchIDLenth
        Kenya = 3
        Uganda = 2
        Tanzania = 3
        Ethiopia = 4
    End Enum
    Enum BWMode
        By_Lightness
        By_RGB_Value
    End Enum

    Public Enum dataExecTypes
        ExecTypeQuery = 0
        ExecTypeNonQuery = 1
    End Enum

    Public Enum systemDbTypes
        dbTypeOracle = 0
        dbTypeSql = 1
        dbTypeAccess = 2
        dbTypeMySql = 3
    End Enum

    Public Enum dbConnectionTypes
        dbConnTypeLive = 0
    End Enum

    Public Enum queryType
        SelectStatement = 0
        StoredProcedure = 1
    End Enum

    Public Enum ReaderType
        Panini = 1
        CTS = 2
        Epson = 3
        NewPanini = 4
        MagTekExcella = 5
        FB20 = 6
        FB08 = 7
        FB10 = 8
        SmartSource = 9
        Cannon = 10
    End Enum
    Private Structure UUID
        Dim Data1 As Long
        Dim Data2 As Integer
        Dim Data3 As Integer
        Dim Data4 As Byte()
    End Structure
    '--FOR SPLITTING THE CODELINE--'
    Public Structure MICRLINE
        Dim ChequeID As String
        Dim ChequeDigit As String
        Dim CountryClearingCenter As String
        Dim CountryID As String
        Dim BankID As String
        Dim BankName As String
        Dim BranchID As String
        Dim BranchName As String
        Dim TheirAccountID As String
        Dim VoucherCode As String
        Dim Amount As Double
        Dim ChequeDate As Date
        Dim CurrencyCode As String
        Dim CodelineWithCharacters As String
        Dim CodelineWithoutCharacters As String
        Dim UniqueNumber As String
        Dim ExtraDetails As String
        Dim [Date] As Date
        Dim ClearingDays As Integer
        Dim FrontImagePathGrayScale As String
        Dim BackImagePath As String
        Dim UVImagePath As String
        Dim FrontImageGrayScale As String
        Dim BackImageGrayScale As String
        Dim FrontImageBW As String
        Dim FrontImagePathBW As String
        Dim UVimage As String
        Dim AccountID As String
        Dim ReturnCode As String
        Dim TFSize As String
        Dim JRSize As String
        Dim JFSize As String
        Dim FullMicrline As String
        Dim IsUpCountry As Int16
        Dim OurCommission As Double
        Dim TheirCommission As Double
        Dim OurCommissionRate As Double
        Dim MinCommissionRate As Double
        Dim CommissionRate As Double
        Dim columnID As String
        Dim FrontImageBlackandWhiteSignature As String
        Dim BackImageSignature As String
        Dim FrontImageGrayScaleSignature As String
        Dim FTdpi As String
        Dim JFdpi As String
        Dim JRdpi As String
        Dim ValueDate As Date
    End Structure
    '-For Customer Details-'
    Public Structure udtCustomerDetails
        Dim CustName As String
        Dim CustAccount As String
        Dim CustBranch As String
        Dim CustExemptCommission As Boolean
        Dim CustExemptUnpaids As Boolean
        Dim CustExemptAutoDelete As Boolean
        Dim CustChequeValueDays As String
        Dim CustAddress As String
        Dim CustTelephone As String
        Dim CustStatus As Integer
        Dim CustRemoteComm As Double
        Dim CustUpcountryComm As Double
        Dim CustUnpaidsInsufficient As Double
        Dim CustUnpaidsOther As Double
        Dim CustMinCom As Double
        Dim CustMaxCom As Double
    End Structure
    '--for commissions processing--'
    Public Structure CommissionItem
        Dim strMICR As MICRLINE
        Dim currChqAmount As Double
        Dim currCommissionRetained As Double
        Dim currCommissionRemitted As Double
        Dim strAccountCr As String
        Dim strAccountDr As String
        Dim strPbranch As String
        Dim blnUpcountryTrue_RemoteFalse As Boolean
    End Structure
    Public Shared Function ValidateMicrLenghtNet(ByVal micrLine As String, ByVal ValidationType As String)
        Dim ValToValidate As String = ""
        ClearTheHolders()
        Dim BkID As Int16 = 0
        Dim BrnID As Int16 = 0
        Dim ClrHSID As String = "99"
        Dim OldBnk As String = ""
        If ValidationType = "TypeB" Then
            IsTypeCCheque = False
        Else
            IsTypeCCheque = True
        End If

        'MessageBox.Show("Mirc:-" + micrLine + " Type: " + IsTypeCCheque)
        If isMDV = True Then
            micrLine = MDVMicr
        End If
        cTrxType = "OC"
        If micrLine.ToString.Contains("?") Then
            RejectedReason = "Invalid Micr Line"
        End If
        Select Case CouID
            Case ENUM_CountryCode.Kenya
                BkID = ENUM_BankIDLenth.Kenya
                BrnID = ENUM_BranchIDLenth.Kenya
            Case ENUM_CountryCode.Uganda
                BkID = ENUM_BankIDLenth.Uganda
                BrnID = ENUM_BranchIDLenth.Uganda
            Case ENUM_CountryCode.Ethiopia
                BkID = ENUM_BankIDLenth.Ethiopia
                BrnID = ENUM_BranchIDLenth.Ethiopia
            Case ENUM_CountryCode.Tanzania
                BkID = ENUM_BankIDLenth.Tanzania
                BrnID = ENUM_BranchIDLenth.Tanzania
                BankIDSize = BkID
        End Select
        'MsgBox(micrLine)
        micrLine = micrLine.Replace(" ", "!")
        Try
            If isMDV = True Then
                micrLine = MDVMicr
                micrLine = micrLine.Replace(" ", "!")
                CodeLineDetails.FullMicrline = micrLine
            End If
            If cTrxType = "" Then cTrxType = "OC"
            CodeLineDetails.FullMicrline = micrLine
            'Check ChequeID
            Select Case CouID
                Case ENUM_CountryCode.Kenya
                    ValToValidate = micrLine.Substring(0, 6)
                Case ENUM_CountryCode.Uganda
                    ValToValidate = micrLine.Substring(0, 10)
                Case ENUM_CountryCode.Ethiopia
                    ValToValidate = micrLine.Substring(0, 11)
                    ValToValidate = ValToValidate.TrimStart("0")
                Case ENUM_CountryCode.Tanzania
                    If IsTypeCCheque Then
                        ValToValidate = micrLine.Substring(0, 6)
                    Else
                        ValToValidate = micrLine.Substring(0, 6)
                    End If

            End Select

            Select Case CouID
                Case ENUM_CountryCode.Ethiopia
                    If ValToValidate.ToString.Contains(" ") Then
                        RejectedReason = "Invalid Cheque Number"
                        CodeLineDetails.ChequeID = ValToValidate
                    ElseIf ValToValidate.ToString.Contains("!") Then
                        RejectedReason = "Invalid Cheque Number"
                        CodeLineDetails.ChequeID = ValToValidate
                    ElseIf ValToValidate.ToString.Contains("?") Then
                        RejectedReason = "Invalid Cheque Number"
                        CodeLineDetails.ChequeID = ValToValidate
                    Else
                        CodeLineDetails.ChequeID = ValToValidate
                        If RejectedReason = "" Then
                            RejectedReason = "ok"
                        End If
                    End If
                Case Else
                    If Not IsNumeric(ValToValidate) Then
                        RejectedReason = "Invalid Cheque Number"
                        CodeLineDetails.ChequeID = ValToValidate
                    ElseIf ValToValidate.ToString.Contains(" ") Then
                        RejectedReason = "Invalid Cheque Number"
                        CodeLineDetails.ChequeID = ValToValidate
                    ElseIf ValToValidate.ToString.Contains("E") Then
                        RejectedReason = "Invalid Cheque Number"
                        CodeLineDetails.ChequeID = ValToValidate
                    ElseIf ValToValidate.ToString.Contains("!") Then
                        RejectedReason = "Invalid Cheque Number"
                        CodeLineDetails.ChequeID = ValToValidate
                    ElseIf ValToValidate.ToString.Contains("?") Then
                        RejectedReason = "Invalid Cheque Number"
                        CodeLineDetails.ChequeID = ValToValidate
                    Else
                        CodeLineDetails.ChequeID = ValToValidate
                        If RejectedReason = "" Then
                            RejectedReason = "ok"
                        End If
                    End If
            End Select

            'Validate Bank Id
            Select Case CouID
                Case ENUM_CountryCode.Kenya
                    CodeLineDetails.BankID = micrLine.Substring(6, 2)
                    ClrHSID = "99"
                Case ENUM_CountryCode.Uganda
                    CodeLineDetails.BankID = micrLine.Substring(8, 2)
                    ClrHSID = micrLine.Substring(12, 2)
                Case ENUM_CountryCode.Ethiopia
                    CodeLineDetails.BankID = micrLine.Substring(11, 2)
                    ClrHSID = "00"
                Case ENUM_CountryCode.Tanzania
                    If IsTypeCCheque Then
                        CodeLineDetails.BankID = micrLine.Substring(6, 3)
                    Else
                        CodeLineDetails.BankID = micrLine.Substring(8, 2)
                    End If
                    CodeLineDetails.BankID = New String("0", BankIDSize - CodeLineDetails.BankID.Length) & CodeLineDetails.BankID
            End Select

            ValToValidate = CodeLineDetails.BankID
            If Not IsNumeric(ValToValidate) Then
                RejectedReason = "Invalid BankID Number"
                CodeLineDetails.BankID = ValToValidate
            ElseIf ValToValidate.ToString.Contains(" ") Then
                RejectedReason = "Invalid BankID Number"
                CodeLineDetails.BankID = ValToValidate
            ElseIf ValToValidate.ToString.Contains("E") Then
                RejectedReason = "Invalid BankID Number"
                CodeLineDetails.BankID = ValToValidate
            ElseIf ValToValidate.ToString.Contains("!") Then
                RejectedReason = "Invalid BankID Number"
                CodeLineDetails.BankID = ValToValidate
            ElseIf ValToValidate.ToString.Contains("?") Then
                RejectedReason = "Invalid BankID Number"
                CodeLineDetails.BankID = ValToValidate
            Else
                CodeLineDetails.BankID = ValToValidate
                If RejectedReason = "" Then
                    RejectedReason = "ok"
                End If
            End If



            If cTrxType = "OC" Then
                Try
                    Select Case CouID
                        Case ENUM_CountryCode.Kenya

                        Case ENUM_CountryCode.Uganda
                            OldBnk = CodeLineDetails.BankID
                            If CodeLineDetails.BankID = "37" Then
                                CodeLineDetails.BankID = "98"
                            End If
                        Case ENUM_CountryCode.Tanzania

                    End Select
                    'MessageBox.Show("<<<<>>>> 10")
                    ExecuteData(GetModify("p_GetMDVBanks", "BankID", CodeLineDetails.BankID, "ourbranchid", OurBranchID, "OperatorID", OperatorID), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
                Catch ex As Exception
                    'MessageBox.Show("<<<<>>>> 10 - 1")
                    MessageBox.Show(ex.Message)
                End Try
                Try
                    Select Case CouID
                        Case ENUM_CountryCode.Kenya

                        Case ENUM_CountryCode.Uganda
                            CodeLineDetails.BankID = OldBnk
                        Case ENUM_CountryCode.Tanzania

                    End Select
                    If publicDTbl.Rows.Count > 0 Then
                        CodeLineDetails.BankName = publicDTbl.Rows(0)("BankName").ToString.Trim
                        If RejectedReason = "" Then
                            RejectedReason = "ok"
                        End If
                        'Only for Finance Trust Bank coz of poor Migration done

                        If CodeLineDetails.BankID = OurBankID Then
                            Dim s As String
repeat:
                            s = InputBox("Provide the receiving bank for this cheque", "BRClearing")
                            If s = cNo_Text Or IsNumeric(s) = False Then
                                MsgBox("Invalid Receiving bank ID")
                                GoTo repeat
                            ElseIf Len(s) <> BkID Then
                                MsgBox("Invalid Receiving bank ID length")
                                GoTo repeat
                            ElseIf s = OurBankID Then
                                MsgBox("Invalid ID Same as Our Bank ID")
                                GoTo repeat
                            Else
                                MICRBankID = s
                                publicDTbl.Clear()
                                Try
                                    'MessageBox.Show("<<<<>>>> 11")
                                    ExecuteData(GetModify("p_GetMDVBanks", "BankID", s, "ourbranchid", OurBranchID, "OperatorID", OperatorID), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
                                Catch ex As Exception
                                    'MessageBox.Show("<<<<>>>> 11 - 2")
                                    MessageBox.Show(ex.Message)
                                End Try
                                CodeLineDetails.BankName = publicDTbl.Rows(0)("BankName").ToString.Trim
                                CodeLineDetails.BankID = s
RepeatBranch:
                                MICRBranchID = ""
                                s = InputBox("Provide the receiving branch for this cheque", "BRClearing")
                                If s = cNo_Text Or IsNumeric(s) = False Then
                                    MsgBox("Invalid Receiving Branch ID")
                                    GoTo RepeatBranch
                                ElseIf Len(s) <> BrnID Then
                                    MsgBox("Invalid Receiving Branch ID length")
                                    GoTo RepeatBranch
                                Else
                                    MICRBranchID = s
                                    CodeLineDetails.BranchID = MICRBranchID
                                    If RejectedReason = "" Then
                                        RejectedReason = "ok"
                                    End If
                                End If
                            End If
returnCode:
                            s = InputBox("Provide the return Code for this cheque", "BRClearing")
                            If s = cNo_Text Or IsNumeric(s) = False Then
                                MsgBox("Invalid Return code ID")
                                GoTo repeat
                            End If
                            Try
                                ExecuteData("exec p_GetChequeReturnReasons", publicDTbl, dataExecTypes.ExecTypeQuery, queryType.SelectStatement)
                            Catch ex As Exception
                                MessageBox.Show("<<<<>>>> 11 - 3")
                                MessageBox.Show(ex.Message)
                            End Try
                            If publicDTbl.Rows.Count > 0 Then
                                If publicDTbl.Select("ReturnID='" & s.ToString & "'").Length > 0 Then
                                    CodeLineDetails.ReturnCode = s
                                Else
                                    MessageBox.Show("Please use a valid ReturnCode")
                                    publicDTbl.Clear()
                                    GoTo returnCode
                                End If
                            End If
                            publicDTbl.Clear()
                            If CodeLineDetails.ReturnCode <> "" Then
                                CodeLineDetails.ClearingDays = "0"
                            End If
                        Else
                            Select Case CouID
                                Case ENUM_CountryCode.Kenya
                                    CodeLineDetails.BankID = micrLine.Substring(6, 2)
                                Case ENUM_CountryCode.Uganda
                                    CodeLineDetails.BankID = micrLine.Substring(8, 2)
                                Case ENUM_CountryCode.Ethiopia
                                    CodeLineDetails.BankID = micrLine.Substring(11, 2)
                                Case ENUM_CountryCode.Tanzania
                                    If IsTypeCCheque Then
                                        CodeLineDetails.BankID = micrLine.Substring(6, 3)
                                    Else
                                        CodeLineDetails.BankID = micrLine.Substring(8, 2)
                                    End If
                                    CodeLineDetails.BankID = New String("0", BankIDSize - CodeLineDetails.BankID.Length) & CodeLineDetails.BankID
                            End Select
                        End If

                    End If
                Catch ex As Exception

                End Try

                publicDTbl.Clear()
            Else
                Select Case CouID
                    Case ENUM_CountryCode.Kenya
                        CodeLineDetails.BankID = micrLine.Substring(6, 2)
                    Case ENUM_CountryCode.Uganda
                        CodeLineDetails.BankID = micrLine.Substring(8, 2)
                    Case ENUM_CountryCode.Ethiopia
                        CodeLineDetails.BankID = micrLine.Substring(11, 2)
                    Case ENUM_CountryCode.Tanzania
                        If IsTypeCCheque Then
                            CodeLineDetails.BankID = micrLine.Substring(6, 3)
                        Else
                            CodeLineDetails.BankID = micrLine.Substring(8, 2)
                        End If
                        CodeLineDetails.BankID = New String("0", BankIDSize - CodeLineDetails.BankID.Length) & CodeLineDetails.BankID
                End Select
            End If



            Dim NewBranch As String = ""
            Select Case CouID
                Case ENUM_CountryCode.Kenya
                    NewBranch = micrLine.Substring(8, 3)
                Case ENUM_CountryCode.Ethiopia
                    NewBranch = micrLine.Substring(13, 4)
                Case ENUM_CountryCode.Uganda
                    NewBranch = micrLine.Substring(10, 2)
                Case ENUM_CountryCode.Tanzania
                    If IsTypeCCheque = True Then
                        NewBranch = micrLine.Substring(9, 3)
                        CodeLineDetails.BranchID = NewBranch
                    Else
                        NewBranch = micrLine.Substring(10, 2)
                        CodeLineDetails.BranchID = NewBranch
                    End If
                    CodeLineDetails.BranchID = New String("0", BankIDSize - CodeLineDetails.BranchID.Length) & CodeLineDetails.BranchID
                    NewBranch = New String("0", BankIDSize - NewBranch.Length) & NewBranch
            End Select

            'If IsTypeCCheque = True Then
            '    NewBranch = micrLine.Substring(9, 3)
            '    CodeLineDetails.BranchID = micrLine.Substring(9, 3)
            '    NewBranch = CodeLineDetails.BranchID
            'Else
            '    NewBranch = Right(CodeLineDetails.BranchID, 2)
            'End If

            If NewBranch = "" Then
                Select Case CouID
                    Case ENUM_CountryCode.Kenya
                        NewBranch = micrLine.Substring(8, 3)
                    Case ENUM_CountryCode.Uganda
                        NewBranch = micrLine.Substring(10, 2)
                    Case ENUM_CountryCode.Ethiopia
                        NewBranch = micrLine.Substring(13, 4)
                    Case ENUM_CountryCode.Tanzania
                        If IsTypeCCheque = True Then
                            NewBranch = micrLine.Substring(9, 3)
                        Else
                            NewBranch = Right(micrLine.Substring(10, 2), 2)
                        End If
                        NewBranch = New String("0", BankIDSize - NewBranch.Length) & NewBranch

                End Select

            End If
            ValToValidate = NewBranch
            If Not IsNumeric(ValToValidate) Then
                RejectedReason = "Invalid BranchID Number"
                CodeLineDetails.BranchID = ValToValidate
            ElseIf ValToValidate.ToString.Contains(" ") Then
                RejectedReason = "Invalid BranchID Number"
                CodeLineDetails.BranchID = ValToValidate
            ElseIf ValToValidate.ToString.Contains("E") Then
                RejectedReason = "Invalid BranchID Number"
                CodeLineDetails.BranchID = ValToValidate
            ElseIf ValToValidate.ToString.Contains("!") Then
                RejectedReason = "Invalid BranchID Number"
                CodeLineDetails.BranchID = ValToValidate
            ElseIf ValToValidate.ToString.Contains("?") Then
                RejectedReason = "Invalid BranchID Number"
                CodeLineDetails.BranchID = ValToValidate
            Else
                CodeLineDetails.BranchID = ValToValidate
                If RejectedReason = "" Then
                    RejectedReason = "ok"
                End If
            End If
            'Validate BranchID
            If cTrxType = "OC" Then
                Try


                    'MessageBox.Show("Kamunya BankID:" + CodeLineDetails.BankID + " - OurBranchID:" + OurBranchID + "- BranchID:" + NewBranch)
                    Select Case CouID
                        Case ENUM_CountryCode.Kenya
                            ExecuteData("SELECT dbo.f_GetClearingBranchName('" & OurBranchID & "','" & CodeLineDetails.BankID & "', '" & NewBranch & "')", publicDTbl, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
                        Case ENUM_CountryCode.Ethiopia
                            ExecuteData("SELECT dbo.f_GetClearingBranchName('" & OurBranchID & "','" & CodeLineDetails.BankID & "', '" & NewBranch & "')", publicDTbl, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
                        Case ENUM_CountryCode.Uganda
                            ExecuteData("SELECT dbo.f_GetClearingBranchName('" & OurBranchID & "','" & CodeLineDetails.BankID & "', '" & NewBranch & ClrHSID & "')", publicDTbl, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
                        Case ENUM_CountryCode.Tanzania
                            Dim SystemType As String = ConfigurationManager.AppSettings("sysType")
                            Select Case SystemType.ToUpper.Trim
                                Case "BR"
                                    ExecuteData("SELECT dbo.f_GetClearingBranchName('" & OurBranchID & "','" & CodeLineDetails.BankID & "', '" & NewBranch & ClrHSID & "')", publicDTbl, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
                                Case "BRMFO"
                                    ExecuteData("SELECT dbo.f_GetClearingBranchName('" & OurBranchID & "','" & CodeLineDetails.BankID & "', '" & NewBranch & ClrHSID & "')", publicDTbl, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
                                Case "BRNET"
                                    ExecuteData("SELECT dbo.f_GetClearingBranchName('" & OurBranchID & "','" & CodeLineDetails.BankID & "', '" & NewBranch & "')", publicDTbl, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
                                Case "BRNETOLD"
                                    ExecuteData("SELECT dbo.f_GetClearingBranchName('" & OurBranchID & "','" & CodeLineDetails.BankID & "', '" & NewBranch & "')", publicDTbl, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
                            End Select
                    End Select
                Catch ex As Exception
                    'MessageBox.Show("<<<<>>>> 11 - 4")
                    'MessageBox.Show(ex.Message)
                End Try
                If publicDTbl.Rows.Count > 0 Then
                    CodeLineDetails.BranchName = publicDTbl.Rows(0)(0).ToString.Trim
                    If RejectedReason = "" Then
                        RejectedReason = "ok"
                    End If
                    CodeLineDetails.BranchID = NewBranch
                Else
                    RejectedReason = "Invalid BranchID '" & NewBranch & "' for this BankID: '" & CodeLineDetails.BankID & "'"
                    CodeLineDetails.BranchID = NewBranch
                    Return RejectedReason
                End If
                publicDTbl.Clear()
            Else
                Select Case CouID
                    Case ENUM_CountryCode.Kenya
                        CodeLineDetails.BranchID = micrLine.Substring(8, 3)
                    Case ENUM_CountryCode.Ethiopia
                        NewBranch = micrLine.Substring(13, 4)
                    Case ENUM_CountryCode.Uganda
                        CodeLineDetails.BranchID = micrLine.Substring(10, 2)
                    Case ENUM_CountryCode.Tanzania
                        If IsTypeCCheque = True Then
                            CodeLineDetails.BranchID = micrLine.Substring(8, 3)
                        Else
                            CodeLineDetails.BranchID = Right(micrLine.Substring(10, 2), 2)
                        End If
                        CodeLineDetails.BranchID = New String("0", BankIDSize - CodeLineDetails.BranchID.Length) & CodeLineDetails.BranchID
                End Select

            End If
            publicDTbl.Clear()
            NewBranch = CodeLineDetails.BranchID



            If isMDV = True Then
                CodeLineDetails.IsUpCountry = 0
                CodeLineDetails.CommissionRate = 0
                CodeLineDetails.OurCommissionRate = 0
                CodeLineDetails.MinCommissionRate = 0
            Else
                'Check Whether its Upcountry or Not
                If cTrxType = "OC" Then
                    Try
                        'ExecuteData(GetModify("sp_GetBranchIDAndCommission", "ourbranchid", OurBranchID, "bankid", CodeLineDetails.BankID, "branchid", NewBranch), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
                    Catch ex As Exception
                        MessageBox.Show(ex.Message)
                    End Try
                    If publicDTbl.Rows.Count > 0 Then
                        If publicDTbl.Rows(0)(4).ToString.Trim = True Then
                            CodeLineDetails.IsUpCountry = 1
                            CodeLineDetails.CommissionRate = publicDTbl.Rows(0)(2).ToString.Trim
                            CodeLineDetails.OurCommissionRate = publicDTbl.Rows(0)(3).ToString.Trim
                            CodeLineDetails.MinCommissionRate = publicDTbl.Rows(0)(1).ToString.Trim
                        Else
                            CodeLineDetails.IsUpCountry = 0
                        End If
                    Else
                        CodeLineDetails.IsUpCountry = 0
                    End If
                    publicDTbl.Clear()
                End If
            End If
            'Check Voucher Code
            Select Case CouID
                Case ENUM_CountryCode.Kenya
                    ValToValidate = micrLine.Substring(12, 2)
                Case ENUM_CountryCode.Ethiopia
                    ValToValidate = micrLine.Substring(18, 2)
                Case ENUM_CountryCode.Uganda
                    ValToValidate = micrLine.Substring(24, 2)
                Case ENUM_CountryCode.Tanzania
                    If IsTypeCCheque Then
                        ValToValidate = micrLine.Substring(22, 2)
                    Else
                        ValToValidate = micrLine.Substring(24, 2)
                    End If
            End Select

            If Not IsNumeric(ValToValidate) Then
                RejectedReason = "Invalid Voucher Code"
                CodeLineDetails.VoucherCode = ValToValidate
            ElseIf ValToValidate.ToString.Contains(" ") Then
                RejectedReason = "Invalid Voucher Code"
                CodeLineDetails.VoucherCode = ValToValidate
            ElseIf ValToValidate.ToString.Contains("E") Then
                RejectedReason = "Invalid Voucher Code"
                CodeLineDetails.VoucherCode = ValToValidate
            ElseIf ValToValidate.ToString.Contains("!") Then
                RejectedReason = "Invalid Voucher Code"
                CodeLineDetails.VoucherCode = ValToValidate
            ElseIf ValToValidate.ToString.Contains("?") Then
                RejectedReason = "Invalid Voucher Code"
                CodeLineDetails.VoucherCode = ValToValidate
            Else
                If RejectedReason = "" Then
                    RejectedReason = "ok"
                End If
                CodeLineDetails.VoucherCode = ValToValidate
            End If
            publicDTbl.Clear()
            If cTrxType = "OC" Or cTrxType = "A" Then
                Try
                    If AccountType = "C" Then
                        ExecuteData(GetModify("p_GetTrxAcDetails", "AccountID", strAccountID, "OurBranchID", OurBranchID, "TrxBranchID", OurBranchID, "OperatorID", OperatorID, "ModuleID", "3060", "TrxTypeID", "CR", "CurrencyID", cLocalCurrency), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
                    Else
                        ExecuteData(GetModify("p_GetTrxGLDetails", "TrxBranchID", OurBranchID, "OurBranchID", OurBranchID, "AccountID", strAccountID, "TrxTypeID", "CR", "OperatorID", OperatorID, "ModuleID", 3060, "CurrencyID", cLocalCurrency), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
                    End If
                Catch ex As Exception
                    publicDTbl.Clear()
                    ExecuteData("Select dbo.f_GetSystemMessage('" & ex.Message.Substring(6) & "','en')", publicDTbl, dataExecTypes.ExecTypeQuery, queryType.SelectStatement)
                    If (publicDTbl.Rows.Count > 0) Then
                        RejectedReason = publicDTbl.Rows(0)(0).ToString.Trim
                    End If
                    RejectedReason = RejectedReason
                End Try
            End If

            'If the voucher code is ok, now validate if this account accepts this voucher code
            If cTrxType = "OC" Then
                If publicDTbl.Rows.Count > 0 Then
                    Select Case CouID
                        Case ENUM_CountryCode.Kenya
                            Select Case publicDTbl.Rows(0)("CurrencyID").ToString.Trim.ToUpper
                                Case "USD"
                                    If CodeLineDetails.VoucherCode <> 60 Then
                                        RejectedReason = "Cheque posted to a wrong currency account. This is a " & publicDTbl.Rows(0)(0) & " Account."
                                    End If
                                Case "GBP"
                                    If CodeLineDetails.VoucherCode <> 61 Then
                                        RejectedReason = "Cheque posted to a wrong currency account. This is a " & publicDTbl.Rows(0)(0) & " Account."
                                    End If
                                Case "EUR"
                                    If CodeLineDetails.VoucherCode <> 62 Then
                                        RejectedReason = "Cheque posted to a wrong currency account. This is a " & publicDTbl.Rows(0)(0) & " Account."
                                    End If
                                Case "EURO"
                                    If CodeLineDetails.VoucherCode <> 62 Then
                                        RejectedReason = "Cheque posted to a wrong currency account. This is a " & publicDTbl.Rows(0)(0) & " Account."
                                    End If
                                Case "STG"
                                    If CodeLineDetails.VoucherCode <> 61 Then
                                        RejectedReason = "Cheque posted to a wrong currency account. This is a " & publicDTbl.Rows(0)(0) & " Account."
                                    End If
                                Case Else 'Local
                                    Select Case CodeLineDetails.VoucherCode
                                        Case 60, 61, 62
                                            RejectedReason = "Cheque posted to a wrong currency account. This is a " & publicDTbl.Rows(0)(0) & " Account."
                                        Case Else
                                            If RejectedReason = "" Then
                                                RejectedReason = "ok"
                                            End If
                                    End Select
                                    CodeLineDetails.CurrencyCode = publicDTbl.Rows(0)(10).ToString.Trim.ToUpper
                            End Select
                        Case ENUM_CountryCode.Uganda
                            Select Case publicDTbl.Rows(0)("CurrencyID").ToString.Trim.ToUpper
                                Case "USD"
                                    If CodeLineDetails.VoucherCode <> 22 Then
                                        RejectedReason = "Cheque posted to a wrong currency account. This is a " & publicDTbl.Rows(0)(0) & " Account."
                                    End If
                                Case "GBP"
                                    If CodeLineDetails.VoucherCode <> 24 Then
                                        RejectedReason = "Cheque posted to a wrong currency account. This is a " & publicDTbl.Rows(0)(0) & " Account."
                                    End If
                                Case "EUR"
                                    If CodeLineDetails.VoucherCode <> 23 Then
                                        RejectedReason = "Cheque posted to a wrong currency account. This is a " & publicDTbl.Rows(0)(0) & " Account."
                                    End If
                                Case "EURO"
                                    If CodeLineDetails.VoucherCode <> 23 Then
                                        RejectedReason = "Cheque posted to a wrong currency account. This is a " & publicDTbl.Rows(0)(0) & " Account."
                                    End If
                                Case "STG"
                                    If CodeLineDetails.VoucherCode <> 24 Then
                                        RejectedReason = "Cheque posted to a wrong currency account. This is a " & publicDTbl.Rows(0)(0) & " Account."
                                    End If
                                Case Else 'Local
                                    Select Case CodeLineDetails.VoucherCode
                                        Case 22, 23, 24
                                            RejectedReason = "Cheque posted to a wrong currency account. This is a " & publicDTbl.Rows(0)(0) & " Account."
                                        Case Else
                                            If RejectedReason = "" Then
                                                RejectedReason = "ok"
                                            End If
                                    End Select
                                    CodeLineDetails.CurrencyCode = publicDTbl.Rows(0)(10).ToString.Trim.ToUpper
                            End Select
                        Case ENUM_CountryCode.Tanzania

                    End Select


                End If
            Else
                Try
                    If AccountType = "C" Then
                        ExecuteData(GetModify("p_GetTrxAcDetails", "AccountID", strAccountID, "OurBranchID", OurBranchID, "TrxBranchID", OurBranchID, "OperatorID", OperatorID, "ModuleID", "3060", "TrxTypeID", "OC", "CurrencyID", ""), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
                    Else
                        ExecuteData(GetModify("p_GetTrxGLDetails", "TrxBranchID", OurBranchID, "OurBranchID", OurBranchID, "AccountID", strAccountID, "TrxTypeID", "OC", "OperatorID", OperatorID, "ModuleID", 3060, "CurrencyID", ""), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
                    End If

                Catch ex As Exception
                    publicDTbl.Clear()
                    ExecuteData("Select dbo.f_GetSystemMessage('" & ex.Message.Substring(6) & "','en')", publicDTbl, dataExecTypes.ExecTypeQuery, queryType.SelectStatement)
                    If (publicDTbl.Rows.Count > 0) Then
                        RejectedReason = publicDTbl.Rows(0)(0).ToString.Trim
                    End If
                    Return RejectedReason
                End Try
                If publicDTbl.Rows.Count > 0 Then
                    Select Case publicDTbl.Rows(0)(0).ToString.Trim.ToUpper
                        Case "USD"
                            If CodeLineDetails.VoucherCode <> 60 Then
                                RejectedReason = "Cheque posted to a wrong currency account. This is a " & publicDTbl.Rows(0)(0) & " Account."

                            End If
                        Case "GBP"
                            If CodeLineDetails.VoucherCode <> 61 Then
                                RejectedReason = "Cheque posted to a wrong currency account. This is a " & publicDTbl.Rows(0)(0) & " Account."
                            End If
                        Case "EUR"
                            If CodeLineDetails.VoucherCode <> 62 Then
                                RejectedReason = "Cheque posted to a wrong currency account. This is a " & publicDTbl.Rows(0)(0) & " Account."
                            End If
                        Case "EURO"
                            If CodeLineDetails.VoucherCode <> 62 Then
                                RejectedReason = "Cheque posted to a wrong currency account. This is a " & publicDTbl.Rows(0)(0) & " Account."
                            End If
                        Case "STG"
                            If CodeLineDetails.VoucherCode <> 61 Then
                                RejectedReason = "Cheque posted to a wrong currency account. This is a " & publicDTbl.Rows(0)(0) & " Account."
                            End If
                        Case Else 'Local
                            Select Case CodeLineDetails.VoucherCode
                                Case 60, 61, 62
                                    RejectedReason = "Cheque posted to a wrong currency account. This is a " & publicDTbl.Rows(0)(0) & " Account."
                                Case Else
                                    RejectedReason = "ok"
                            End Select
                            CodeLineDetails.CurrencyCode = publicDTbl.Rows(0)(10).ToString.Trim.ToUpper
                    End Select
                End If
            End If
            publicDTbl.Clear()

            If CodeLineDetails.CurrencyCode = "" Then
                CodeLineDetails.CurrencyCode = cLocalCurrency
            End If
            'Check Whether branch is maintained
            If cTrxType = "OC" Or cTrxType = "A" Then
                Dim WrkDt As DateTime = Convert.ToDateTime(WORKING_DATE)
                Dim VluDt As DateTime
                Try
                    Select Case CouID
                        Case ENUM_CountryCode.Kenya
                            ExecuteData(GetModify("p_GetClearingBranch", "ourbranchid", OurBranchID, "bankid", CodeLineDetails.BankID, "branchid", NewBranch, "CurrencyID", CodeLineDetails.CurrencyCode), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
                            VluDt = Convert.ToDateTime(publicDTbl.Rows(0)("ClearingDays").ToString.Trim)
                            CodeLineDetails.ClearingDays = DateDiff(DateInterval.Day, WrkDt, VluDt)
                        Case ENUM_CountryCode.Uganda
                            ExecuteData(GetModify("p_GetClearingBranch", "ourbranchid", OurBranchID, "bankid", CodeLineDetails.BankID, "branchid", NewBranch & ClrHSID, "CurrencyID", CodeLineDetails.CurrencyCode), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
                            VluDt = Convert.ToDateTime(publicDTbl.Rows(0)("ClearingDays").ToString.Trim)
                            CodeLineDetails.ClearingDays = DateDiff(DateInterval.Day, WrkDt, VluDt)
                        Case ENUM_CountryCode.Tanzania
                            Dim SystemType As String = ConfigurationManager.AppSettings("sysType")
                            Select Case SystemType.ToUpper.Trim
                                Case "BR"
                                    ExecuteData(GetModify("p_GetClearingBranch", "ourbranchid", OurBranchID, "bankid", CodeLineDetails.BankID, "branchid", NewBranch & ClrHSID, "CurrencyID", CodeLineDetails.CurrencyCode), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
                                    VluDt = Convert.ToDateTime(publicDTbl.Rows(0)("ClearingDays").ToString.Trim)
                                    CodeLineDetails.ClearingDays = DateDiff(DateInterval.Day, WrkDt, VluDt)
                                Case "BRMFO"
                                    ExecuteData(GetModify("p_GetClearingBranch", "ourbranchid", OurBranchID, "bankid", CodeLineDetails.BankID, "branchid", NewBranch & ClrHSID, "CurrencyID", CodeLineDetails.CurrencyCode), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
                                    VluDt = Convert.ToDateTime(publicDTbl.Rows(0)("ClearingDays").ToString.Trim)
                                    CodeLineDetails.ClearingDays = DateDiff(DateInterval.Day, WrkDt, VluDt)
                                Case "BRNET"
                                    ExecuteData(GetModify("p_GetClearingBranch", "ourbranchid", OurBranchID, "bankid", CodeLineDetails.BankID, "branchid", NewBranch, "CurrencyID", CodeLineDetails.CurrencyCode), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
                                    VluDt = Convert.ToDateTime(publicDTbl.Rows(0)("ClearingDays").ToString.Trim)
                                    CodeLineDetails.ClearingDays = DateDiff(DateInterval.Day, WrkDt, VluDt)
                                Case "BRNETOLD"
                                    ExecuteData(GetModify("p_GetClearingBranch", "ourbranchid", OurBranchID, "bankid", CodeLineDetails.BankID, "branchid", NewBranch, "CurrencyID", CodeLineDetails.CurrencyCode), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
                                    VluDt = Convert.ToDateTime(publicDTbl.Rows(0)("ClearingDays").ToString.Trim)
                                    CodeLineDetails.ClearingDays = DateDiff(DateInterval.Day, WrkDt, VluDt)
                            End Select

                    End Select
                Catch ex As Exception
                    publicDTbl.Clear()
                    If publicDTbl.Rows.Count > 0 Then

                    Else
                        publicDTbl.Clear()
                        'ExecuteData("Select dbo.f_GetSystemMessage('" & ex.Message.Substring(6) & "','en')", publicDTbl, dataExecTypes.ExecTypeQuery, queryType.SelectStatement)
                        'If (publicDTbl.Rows.Count > 0) Then
                        '    RejectedReason = publicDTbl.Rows(0)(0).ToString.Trim
                        'End If
                        RejectedReason = RejectedReason
                    End If
                End Try
            End If
            publicDTbl.Clear()
            NewBranch = CodeLineDetails.BranchID
            If isMDV = True Then
                CodeLineDetails.IsUpCountry = 0
                CodeLineDetails.CommissionRate = 0
                CodeLineDetails.OurCommissionRate = 0
                CodeLineDetails.MinCommissionRate = 0
            Else
                'Check Whether its Upcountry or Not
                If cTrxType = "OC" Or cTrxType = "A" Then
                    Try
                        ExecuteData(GetModify("p_GetClearingBranch", "ourbranchid", OurBranchID, "bankid", CodeLineDetails.BankID, "branchid", NewBranch & ClrHSID, "CurrencyID", CodeLineDetails.CurrencyCode), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
                    Catch ex As Exception
                        MessageBox.Show(ex.Message)
                    End Try
                    If publicDTbl.Rows.Count > 0 Then
                        If publicDTbl.Rows(0)("IsUpcountry").ToString.Trim = True Then
                            CodeLineDetails.IsUpCountry = 1
                            CodeLineDetails.CommissionRate = CodeLineDetails.ClearingDays
                            CodeLineDetails.OurCommissionRate = publicDTbl.Rows(0)("OurCommissionRate").ToString.Trim
                            CodeLineDetails.MinCommissionRate = publicDTbl.Rows(0)("MinCommission").ToString.Trim
                        Else
                            CodeLineDetails.IsUpCountry = 0
                        End If
                    Else
                        CodeLineDetails.IsUpCountry = 0
                    End If
                    publicDTbl.Clear()
                End If
            End If

            Dim AcctLength As String = ""
            Dim AcctVal As String = ""
            Select Case CouID
                Case ENUM_CountryCode.Kenya
                    AcctLength = micrLine.Substring(14).Length
                    AcctVal = micrLine.Substring(14)
                Case ENUM_CountryCode.Ethiopia
                    AcctLength = micrLine.Substring(21).Length
                    AcctVal = micrLine.Substring(21)
                Case ENUM_CountryCode.Uganda
                    AcctLength = micrLine.Substring(14, 10).Length
                    AcctVal = micrLine.Substring(14, 10)
                Case ENUM_CountryCode.Tanzania
                    If IsTypeCCheque Then
                        AcctLength = micrLine.Substring(12, 10).Length
                        AcctVal = micrLine.Substring(12, 10)
                    Else
                        AcctLength = micrLine.Substring(14, 10).Length
                        AcctVal = micrLine.Substring(14, 10)
                    End If

            End Select

            'Validate AccountID
            If CodeLineDetails.AccountID = "" And cTrxType = "ID" Then
                If AcctLength = 10 Then
                    ValToValidate = New String("0", 10 - AcctLength) & AcctVal
                    If Not IsNumeric(ValToValidate) Then
                        RejectedReason = "Invalid Account Number"
                        CodeLineDetails.TheirAccountID = ValToValidate
                    ElseIf ValToValidate.ToString.Contains(" ") Then
                        RejectedReason = "Invalid Account Number"
                        CodeLineDetails.TheirAccountID = ValToValidate
                    ElseIf ValToValidate.ToString.Contains("E") Then
                        RejectedReason = "Invalid Account Number"
                        CodeLineDetails.TheirAccountID = ValToValidate
                    ElseIf ValToValidate.ToString.Contains("!") Then
                        RejectedReason = "Invalid Account Number"
                        CodeLineDetails.TheirAccountID = ValToValidate
                    ElseIf ValToValidate.ToString.Contains("?") Then
                        RejectedReason = "Invalid Account Number"
                        CodeLineDetails.TheirAccountID = ValToValidate
                    Else
                        CodeLineDetails.TheirAccountID = ValToValidate
                        If RejectedReason = "" Then
                            RejectedReason = "ok"
                        End If
                    End If
                Else
                    RejectedReason = "Invalid Account Number"
                    CodeLineDetails.TheirAccountID = ValToValidate
                End If
            ElseIf CodeLineDetails.AccountID = "" And (cTrxType = "OC" Or cTrxType = "A") Then
                If CouID = "4" Then
                    ValToValidate = AcctVal
                    If ValToValidate.ToString.Contains(" ") Then
                        RejectedReason = "Invalid Account Number"
                        CodeLineDetails.TheirAccountID = ValToValidate
                    ElseIf ValToValidate.ToString.Contains("E") Then
                        RejectedReason = "Invalid Account Number"
                        CodeLineDetails.TheirAccountID = ValToValidate
                    ElseIf ValToValidate.ToString.Contains("!") Then
                        RejectedReason = "Invalid Account Number"
                        CodeLineDetails.TheirAccountID = ValToValidate
                    ElseIf ValToValidate.ToString.Contains("?") Then
                        RejectedReason = "Invalid Account Number"
                        CodeLineDetails.TheirAccountID = ValToValidate
                    Else
                        CodeLineDetails.TheirAccountID = ValToValidate
                        If RejectedReason = "" Then
                            RejectedReason = "ok"
                        End If
                    End If
                ElseIf AcctLength = 10 And CouID <> "4" Then
                    ValToValidate = New String("0", 10 - AcctLength) & AcctVal
                    If Not IsNumeric(ValToValidate) Then
                        RejectedReason = "Invalid Account Number"
                        CodeLineDetails.TheirAccountID = ValToValidate
                    ElseIf ValToValidate.ToString.Contains(" ") Then
                        RejectedReason = "Invalid Account Number"
                        CodeLineDetails.TheirAccountID = ValToValidate
                    ElseIf ValToValidate.ToString.Contains("E") Then
                        RejectedReason = "Invalid Account Number"
                        CodeLineDetails.TheirAccountID = ValToValidate
                    ElseIf ValToValidate.ToString.Contains("!") Then
                        RejectedReason = "Invalid Account Number"
                        CodeLineDetails.TheirAccountID = ValToValidate
                    ElseIf ValToValidate.ToString.Contains("?") Then
                        RejectedReason = "Invalid Account Number"
                        CodeLineDetails.TheirAccountID = ValToValidate
                    Else
                        CodeLineDetails.TheirAccountID = ValToValidate
                        If RejectedReason = "" Then
                            RejectedReason = "ok"
                        End If
                    End If
                Else
                    RejectedReason = "Invalid Account Number"
                    CodeLineDetails.TheirAccountID = ValToValidate
                End If

                'Validate ChequeDigit

                If isMDV = False Then

                    Select Case CouID
                        Case ENUM_CountryCode.Kenya
                            CodeLineDetails.ChequeDigit = micrLine.Substring(11, 1)
                            ClrHSID = "99"
                            CodeLineDetails.CountryID = ClrHSID
                        Case ENUM_CountryCode.Uganda
                            CodeLineDetails.ChequeDigit = micrLine.Substring(6, 2)
                            ClrHSID = micrLine.Substring(12, 2)
                        Case ENUM_CountryCode.Ethiopia
                            CodeLineDetails.ChequeDigit = "0"
                            ClrHSID = "00"
                        Case ENUM_CountryCode.Tanzania
                            If IsTypeCCheque Then
                                CodeLineDetails.ChequeDigit = "00"
                                ClrHSID = "67"
                            Else
                                CodeLineDetails.ChequeDigit = micrLine.Substring(12, 2)
                                ClrHSID = micrLine.Substring(6, 2)
                            End If
                    End Select


                    ValToValidate = CodeLineDetails.ChequeDigit
                    If Not IsNumeric(ValToValidate) Then
                        RejectedReason = "Invalid Cheque Digit"
                        CodeLineDetails.ChequeDigit = ValToValidate
                    ElseIf ValToValidate.ToString.Contains(" ") Then
                        RejectedReason = "Invalid Cheque Digit"
                        CodeLineDetails.ChequeDigit = ValToValidate
                    ElseIf ValToValidate.ToString.Contains("E") Then
                        RejectedReason = "Invalid Cheque Digit"
                        CodeLineDetails.ChequeDigit = ValToValidate
                    ElseIf ValToValidate.ToString.Contains("!") Then
                        RejectedReason = "Invalid Cheque Digit"
                        CodeLineDetails.ChequeDigit = ValToValidate
                    ElseIf ValToValidate.ToString.Contains("?") Then
                        RejectedReason = "Invalid Cheque Digit"
                        CodeLineDetails.ChequeDigit = ValToValidate
                    Else
                        CodeLineDetails.ChequeDigit = ValToValidate
                        If RejectedReason = "" Then
                            RejectedReason = "ok"
                        End If
                    End If
                    CodeLineDetails.CountryClearingCenter = ClrHSID
                    If RejectedReason = "ok" Then
                        'If OurBankID <> "50" And ClrHSID = "99" Then
                        If OurBankID <> "50" Then
                            Select Case CouID
                                Case ENUM_CountryCode.Kenya
                                    If GetChequeDigit(CodeLineDetails.TheirAccountID, CodeLineDetails.BankID, CodeLineDetails.BranchID, CodeLineDetails.CountryClearingCenter, CountryCode) <> CodeLineDetails.ChequeDigit Then
                                        If MsgBox("Invalid Cheque Digit. The correct one is : " & GetChequeDigit(CodeLineDetails.TheirAccountID, CodeLineDetails.BankID, CodeLineDetails.BranchID, CodeLineDetails.CountryClearingCenter, CountryCode) & vbCrLf & "Do you wish to adopt invalid one? This will pick the value captured.", vbYesNo) = vbYes Then
                                            If RejectedReason = "" Then
                                                RejectedReason = "ok"
                                            End If
                                            MicrEditted = True
                                            'CodeLineDetails.ChequeDigit = GetChequeDigit(CodeLineDetails.TheirAccountID, CodeLineDetails.BankID, CodeLineDetails.BranchID, CodeLineDetails.CountryClearingCenter, CountryCode)
                                        Else
                                            RejectedReason = "Invalid Cheque Digit. With this bankID " & CodeLineDetails.BankID & " and BranchID " & CodeLineDetails.BranchID & " It should be - " & GetChequeDigit(CodeLineDetails.TheirAccountID, CodeLineDetails.BankID, CodeLineDetails.BranchID, CodeLineDetails.CountryClearingCenter, CountryCode) & ""
                                        End If
                                    End If
                                Case ENUM_CountryCode.Uganda
                                    If GetChequeDigit(CodeLineDetails.TheirAccountID, CodeLineDetails.BankID, CodeLineDetails.BranchID, CodeLineDetails.CountryClearingCenter, CountryCode) <> CodeLineDetails.ChequeDigit Then
                                        'If MsgBox("Invalid Cheque Digit. The correct one is : " & GetChequeDigit(CodeLineDetails.TheirAccountID, CodeLineDetails.BankID, CodeLineDetails.BranchID, CodeLineDetails.CountryClearingCenter, CountryCode) & vbCrLf & "Do you wish to adopt it? This will correct cheque digit to its correct value.", vbYesNo) = vbYes Then
                                        '    If RejectedReason = "" Then
                                        '        RejectedReason = "ok"
                                        '    End If
                                        '    MicrEditted = True
                                        '    CodeLineDetails.ChequeDigit = GetChequeDigit(CodeLineDetails.TheirAccountID, CodeLineDetails.BankID, CodeLineDetails.BranchID, CodeLineDetails.CountryClearingCenter, CountryCode)
                                        'Else
                                        RejectedReason = "Invalid Cheque Digit. With this bankID " & CodeLineDetails.BankID & " and BranchID " & CodeLineDetails.BranchID & " It should be - " & GetChequeDigit(CodeLineDetails.TheirAccountID, CodeLineDetails.BankID, CodeLineDetails.BranchID, CodeLineDetails.CountryClearingCenter, CountryCode) & ""
                                        'End If
                                    End If
                                Case ENUM_CountryCode.Tanzania

                            End Select

                        Else
                            Select Case CouID
                                Case ENUM_CountryCode.Kenya
                                    CodeLineDetails.ChequeDigit = micrLine.Substring(11, 1)
                                Case ENUM_CountryCode.Uganda
                                    CodeLineDetails.ChequeDigit = micrLine.Substring(6, 2)
                                Case ENUM_CountryCode.Ethiopia
                                    CodeLineDetails.ChequeDigit = "0"
                                Case ENUM_CountryCode.Tanzania
                                    If IsTypeCCheque Then
                                        CodeLineDetails.ChequeDigit = "00"
                                    Else
                                        CodeLineDetails.ChequeDigit = micrLine.Substring(12, 2)
                                    End If
                                    RejectedReason = "ok"
                            End Select
                        End If
                    Else
                        Select Case CouID
                            Case ENUM_CountryCode.Kenya
                                CodeLineDetails.ChequeDigit = micrLine.Substring(11, 1)
                            Case ENUM_CountryCode.Ethiopia
                                CodeLineDetails.ChequeDigit = "0"
                            Case ENUM_CountryCode.Uganda
                                CodeLineDetails.ChequeDigit = micrLine.Substring(6, 2)
                            Case ENUM_CountryCode.Tanzania
                                If IsTypeCCheque Then
                                    CodeLineDetails.ChequeDigit = "00"
                                Else
                                    CodeLineDetails.ChequeDigit = micrLine.Substring(12, 2)
                                End If
                        End Select
                    End If
                Else
                    Select Case CouID
                        Case ENUM_CountryCode.Kenya
                            CodeLineDetails.ChequeDigit = micrLine.Substring(11, 1)
                            ClrHSID = "99"
                            CodeLineDetails.CountryID = ClrHSID
                        Case ENUM_CountryCode.Ethiopia
                            CodeLineDetails.ChequeDigit = "0"
                            ClrHSID = "00"
                            CodeLineDetails.CountryID = ClrHSID
                        Case ENUM_CountryCode.Uganda
                            CodeLineDetails.ChequeDigit = micrLine.Substring(6, 2)
                            ClrHSID = micrLine.Substring(12, 2)
                        Case ENUM_CountryCode.Tanzania
                            If IsTypeCCheque Then
                                CodeLineDetails.ChequeDigit = "00"
                                ClrHSID = "67"
                            Else
                                CodeLineDetails.ChequeDigit = micrLine.Substring(12, 2)
                                ClrHSID = micrLine.Substring(6, 2)
                            End If
                    End Select
                    ValToValidate = CodeLineDetails.ChequeDigit
                    If Not IsNumeric(ValToValidate) Then
                        RejectedReason = "Invalid Cheque Digit"
                        CodeLineDetails.ChequeDigit = ValToValidate
                    ElseIf ValToValidate.ToString.Contains(" ") Then
                        RejectedReason = "Invalid Cheque Digit"
                        CodeLineDetails.ChequeDigit = ValToValidate
                    ElseIf ValToValidate.ToString.Contains("E") Then
                        RejectedReason = "Invalid Cheque Digit"
                        CodeLineDetails.ChequeDigit = ValToValidate
                    ElseIf ValToValidate.ToString.Contains("!") Then
                        RejectedReason = "Invalid Cheque Digit"
                        CodeLineDetails.ChequeDigit = ValToValidate
                    ElseIf ValToValidate.ToString.Contains("?") Then
                        RejectedReason = "Invalid Cheque Digit"
                        CodeLineDetails.ChequeDigit = ValToValidate
                    Else
                        CodeLineDetails.CountryClearingCenter = ClrHSID
                        Select Case CouID
                            Case ENUM_CountryCode.Kenya
                                If OurBankID <> "50" Then

                                    If GetChequeDigit(CodeLineDetails.TheirAccountID, CodeLineDetails.BankID, CodeLineDetails.BranchID, CodeLineDetails.CountryClearingCenter, CountryCode) <> CodeLineDetails.ChequeDigit Then
                                        If MsgBox("Invalid Cheque Digit. The correct one is : " & GetChequeDigit(CodeLineDetails.TheirAccountID, CodeLineDetails.BankID, CodeLineDetails.BranchID, CodeLineDetails.CountryClearingCenter, CountryCode) & vbCrLf & "Do you wish to adopt invalid one? This will pick the value captured.", vbYesNo) = vbYes Then
                                            If RejectedReason = "" Then
                                                RejectedReason = "ok"
                                            End If
                                            MicrEditted = True
                                            'CodeLineDetails.ChequeDigit = GetChequeDigit(CodeLineDetails.TheirAccountID, CodeLineDetails.BankID, CodeLineDetails.BranchID, CodeLineDetails.CountryClearingCenter, CountryCode)
                                        Else
                                            RejectedReason = "Invalid Cheque Digit. With this bankID " & CodeLineDetails.BankID & " and BranchID " & CodeLineDetails.BranchID & " It should be - " & GetChequeDigit(CodeLineDetails.TheirAccountID, CodeLineDetails.BankID, CodeLineDetails.BranchID, CodeLineDetails.CountryClearingCenter, CountryCode) & ""
                                        End If
                                    End If
                                End If
                            Case ENUM_CountryCode.Uganda
                                If GetChequeDigit(CodeLineDetails.TheirAccountID, CodeLineDetails.BankID, CodeLineDetails.BranchID, CodeLineDetails.CountryClearingCenter, CountryCode) <> CodeLineDetails.ChequeDigit Then
                                    RejectedReason = "Invalid Cheque Digit. With this bankID " & CodeLineDetails.BankID & " and BranchID " & CodeLineDetails.BranchID & " It should be - " & GetChequeDigit(CodeLineDetails.TheirAccountID, CodeLineDetails.BankID, CodeLineDetails.BranchID, CodeLineDetails.CountryClearingCenter, CountryCode) & ""
                                End If
                            Case ENUM_CountryCode.Tanzania

                        End Select

                        CodeLineDetails.ChequeDigit = ValToValidate
                        If RejectedReason = "" Then
                            RejectedReason = "ok"
                        End If
                    End If
                End If


            Else


            End If
            'Validate branch again if its maintained
            Try
                If CodeLineDetails.AccountID = "" Then
                    CodeLineDetails.AccountID = strAccountID
                End If
                ExecuteData(GetModify("p_GetValueDate", "ourbranchid", OurBranchID, "CurrencyID", CodeLineDetails.CurrencyCode, "AccountID", CodeLineDetails.AccountID, "AccountTypeID", AccountType, "WorkingDate", cWorkingDate, "VoucherCode", CodeLineDetails.VoucherCode, "BankID", CodeLineDetails.BankID, "BranchID", CodeLineDetails.BranchID, "Amount", 0, "ReturnCode", CodeLineDetails.ReturnCode), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.SelectStatement)
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
            If publicDTbl.Rows.Count > 0 Then
                If Convert.ToDateTime(publicDTbl.Rows(0)("ValueDate")) >= Convert.ToDateTime(cWorkingDate) Then
                    Dim diff2 As String = (Convert.ToDateTime(publicDTbl.Rows(0)("ValueDate")) - Convert.ToDateTime(cWorkingDate)).TotalDays.ToString()
                    CodeLineDetails.ClearingDays = diff2
                    CodeLineDetails.ValueDate = publicDTbl.Rows(0)("ValueDate")
                Else
                    Select Case CouID
                        Case ENUM_CountryCode.Kenya
                            RejectedReason = "Invalid ValueDated " & publicDTbl.Rows(0)("ValueDate") & ". Please Confirm Their BankID and TheirBranchID are Maintained"
                            CodeLineDetails.BranchID = CodeLineDetails.BranchID
                            RejectedReason = RejectedReason
                        Case ENUM_CountryCode.Uganda
                            RejectedReason = "Invalid ValueDated " & publicDTbl.Rows(0)("ValueDate") & ". Please Confirm Their BankID and TheirBranchID are Maintained"
                            CodeLineDetails.BranchID = CodeLineDetails.BranchID
                            RejectedReason = RejectedReason
                        Case ENUM_CountryCode.Tanzania

                    End Select
                End If
            Else

            End If
            publicDTbl.Clear()
            ExecuteData(GetModify("sp_GetIfChequeIsPosted", "BankID", CodeLineDetails.BankID, "Branchid", CodeLineDetails.BranchID, "TheirAccountID", CodeLineDetails.TheirAccountID, "ChequeID", CodeLineDetails.ChequeID), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
            If publicDTbl.Rows.Count > 0 Then
                If publicDTbl.Rows(0)(0) <> "False" Then
                    RejectedReason = "Item already captured. Item will be ignored"
                    publicDTbl.Clear()
                End If
            End If
            If RejectedReason = "" Then
                RejectedReason = "ok"
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        micrLine = ""
        Return RejectedReason
    End Function

    Public Shared Function ValidateMicrLenght(ByVal micrLine As String, ByVal ValidationType As String)
        Dim ValToValidate As String = ""
        ClearTheHolders()
        Dim BnkID As String = ""
        Dim BrnchID As String = ""
        'MsgBox("imeanza validations")
        If CouID = ENUM_CountryCode.Uganda Then


        End If
        'MsgBox(micrLine)
        Select Case CountryCode.ToUpper.Trim
            Case "UG"
                micrLine = micrLine.Replace(" ", "!")
                ClrHSID = micrLine.Substring(6, 2)
            Case "SL"

            Case "TZ"
                If IsTypeCCheque = False Then
                    micrLine = micrLine.Replace(" ", "")
                    ClrHSID = micrLine.Substring(6, 2)
                Else
                    ClrHSID = "67"
                End If

            Case "RD"

            Case "KE"
                micrLine = micrLine.Replace(" ", "!")
                ClrHSID = "99"
            Case "ET"

            Case "SA"

        End Select

        Try
            If isMDV = True Then
                micrLine = MDVMicr
                micrLine = micrLine.Replace(" ", "!")
                CodeLineDetails.FullMicrline = micrLine
            End If
            If cTrxType = "" Then cTrxType = "O"
            CodeLineDetails.FullMicrline = micrLine
            'Check ChequeID
            ValToValidate = micrLine.Substring(0, 6)
            If Not IsNumeric(ValToValidate) Then
                RejectedReason = "Invalid Cheque Number"
                CodeLineDetails.ChequeID = ValToValidate
            ElseIf ValToValidate.ToString.Contains(" ") Then
                RejectedReason = "Invalid Cheque Number"
                CodeLineDetails.ChequeID = ValToValidate
            ElseIf ValToValidate.ToString.Contains("!") Then
                RejectedReason = "Invalid Cheque Number"
                CodeLineDetails.ChequeID = ValToValidate
            ElseIf ValToValidate.ToString.Contains("?") Then
                RejectedReason = "Invalid Cheque Number"
                CodeLineDetails.ChequeID = ValToValidate
            Else
                CodeLineDetails.ChequeID = ValToValidate
                If RejectedReason = "" Then
                    RejectedReason = "ok"
                End If
            End If

            'Branch Details and Cheque Digit Calculation

            Select Case CountryCode.ToUpper.Trim
                Case "UG"
                    ValToValidate = micrLine.Substring(6, 6)
                Case "SL"

                Case "TZ"
                    ValToValidate = micrLine.Substring(8, 6)
                Case "RD"

                Case "KE"
                    ValToValidate = micrLine.Substring(6, 6)
                Case "ET"

                Case "SA"

            End Select
            If Not IsNumeric(ValToValidate) Then
                RejectedReason = "Invalid Micr Line"
            ElseIf ValToValidate.ToString.Contains(" ") Then
                RejectedReason = "Invalid Micr Line"
            ElseIf ValToValidate.ToString.Contains("!") Then
                RejectedReason = "Invalid Micr Line"
            ElseIf ValToValidate.ToString.Contains("?") Then
                RejectedReason = "Invalid Micr Line"
            Else

                Select Case CountryCode.ToUpper.Trim
                    Case "UG"
                        BnkID = micrLine.Substring(8, 2)
                        BrnchID = micrLine.Substring(10, 2)
                        ClrHSID = micrLine.Substring(12, 2)
                    Case "SL"

                    Case "TZ"
                        If IsTypeCCheque = False Then
                            BnkID = micrLine.Substring(8, 2)
                            BrnchID = micrLine.Substring(10, 2)
                        Else
                            BnkID = micrLine.Substring(8, 2)
                            BrnchID = micrLine.Substring(10, 2)
                        End If

                    Case "RD"

                    Case "KE"
                        ValToValidate = micrLine.Substring(6, 6)
                        BnkID = micrLine.Substring(6, 2)
                        BrnchID = micrLine.Substring(8, 3)
                    Case "ET"

                    Case "SA"

                End Select
                'Validate Bank Id
                If cTrxType = "O" Or cTrxType = "A" Then
                    If isMDV = True Then
                        ExecuteData(GetModify("SP_GETBANKS", "ourbranchid", OurBranchID, "BankID", BnkID), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
                    Else
                        ExecuteData(GetModify("SP_GETBANKS", "ourbranchid", OurBranchID, "BankID", BnkID), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
                    End If
                    Try
                        If publicDTbl.Rows.Count > 0 Then
                            CodeLineDetails.BankID = BnkID

                            CodeLineDetails.BankName = publicDTbl.Rows(0)("FullName").ToString.Trim
                            If RejectedReason = "" Then
                                RejectedReason = "ok"
                            End If
                            If CodeLineDetails.BankID = OurBankID Then
                                Dim s As String
repeat:
                                s = InputBox("Provide the receiving bank for this cheque", "BRClearing")
                                If s = cNo_Text Or IsNumeric(s) = False Then
                                    MsgBox("Invalid Receiving bank ID")
                                    GoTo repeat
                                ElseIf Len(s) <> 2 Then
                                    MsgBox("Invalid Receiving bank ID length")
                                    GoTo repeat
                                ElseIf s = OurBankID Then
                                    MsgBox("Invalid ID Same as Our Bank ID")
                                    GoTo repeat
                                Else
                                    MICRBankID = s
                                    publicDTbl.Clear()
                                    ExecuteData(GetModify("SP_GETBANKS", "ourbranchid", OurBranchID, "BankID", s), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
                                    CodeLineDetails.BankName = publicDTbl.Rows(0)("FullName").ToString.Trim
                                    CodeLineDetails.BankID = s
RepeatBranch:
                                    MICRBranchID = ""
                                    s = InputBox("Provide the receiving branch for this cheque", "BRClearing")
                                    If s = cNo_Text Or IsNumeric(s) = False Then
                                        MsgBox("Invalid Receiving Branch ID")
                                        GoTo RepeatBranch
                                    ElseIf Len(s) <> 3 Then
                                        MsgBox("Invalid Receiving Branch ID length")
                                        GoTo RepeatBranch
                                    Else
                                        MICRBranchID = s
                                        CodeLineDetails.BranchID = MICRBranchID
                                        If RejectedReason = "" Then
                                            RejectedReason = "ok"
                                        End If
                                    End If
                                End If
returnCode:
                                s = InputBox("Provide the return Code for this cheque", "BRClearing")
                                If s = cNo_Text Or IsNumeric(s) = False Then
                                    MsgBox("Invalid Return code ID")
                                    GoTo repeat
                                End If
                                ExecuteData("exec sp_GetReturnReasons", publicDTbl, dataExecTypes.ExecTypeQuery, queryType.SelectStatement)
                                If publicDTbl.Rows.Count > 0 Then
                                    If publicDTbl.Select("ReturnID='" & s.ToString & "'").Length > 0 Then
                                        CodeLineDetails.ReturnCode = s
                                    Else
                                        MessageBox.Show("Please use a valid ReturnCode")
                                        publicDTbl.Clear()
                                        GoTo returnCode
                                    End If
                                End If
                                publicDTbl.Clear()
                                If CodeLineDetails.ReturnCode <> "" Then
                                    CodeLineDetails.ClearingDays = "0"
                                End If
                            Else
                                CodeLineDetails.BankID = BnkID
                            End If

                        End If
                    Catch ex As Exception

                    End Try

                    publicDTbl.Clear()
                Else
                    CodeLineDetails.BankID = BnkID
                End If



                Dim NewBranch As String = CodeLineDetails.BranchID
                If NewBranch = "" Then
                    NewBranch = BrnchID
                End If

                'Validate BranchID
                If cTrxType = "O" Or cTrxType = "A" Then
                    ExecuteData(GetModify("SP_GETbranches", "ourbranchid", OurBranchID, "bankid", CodeLineDetails.BankID, "branchid", NewBranch), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
                    If publicDTbl.Rows.Count > 0 Then
                        CodeLineDetails.BranchName = publicDTbl.Rows(0)("Name").ToString.Trim
                        If RejectedReason = "" Then
                            RejectedReason = "ok"
                        End If
                        CodeLineDetails.BranchID = NewBranch
                    Else
                        RejectedReason = "Invalid BranchID '" & NewBranch & "' for this BankID: '" & BnkID & "'"
                        CodeLineDetails.BranchID = NewBranch
                        Return RejectedReason
                    End If
                    publicDTbl.Clear()
                Else
                    CodeLineDetails.BranchID = BrnchID
                End If
                publicDTbl.Clear()
                NewBranch = CodeLineDetails.BranchID
                If isMDV = True Then
                    CodeLineDetails.IsUpCountry = 0
                    CodeLineDetails.CommissionRate = 0
                    CodeLineDetails.OurCommissionRate = 0
                    CodeLineDetails.MinCommissionRate = 0
                Else
                    'Check Whether its Upcountry or Not
                    If cTrxType = "O" Or cTrxType = "A" Then
                        ExecuteData(GetModify("sp_GetBranchIDAndCommission", "ourbranchid", OurBranchID, "bankid", CodeLineDetails.BankID, "branchid", NewBranch), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
                        If publicDTbl.Rows.Count > 0 Then
                            If publicDTbl.Rows(0)(4).ToString.Trim = True Then
                                CodeLineDetails.IsUpCountry = 1
                                CodeLineDetails.CommissionRate = publicDTbl.Rows(0)(2).ToString.Trim
                                CodeLineDetails.OurCommissionRate = publicDTbl.Rows(0)(3).ToString.Trim
                                CodeLineDetails.MinCommissionRate = publicDTbl.Rows(0)(1).ToString.Trim
                            Else
                                CodeLineDetails.IsUpCountry = 0
                            End If
                        Else
                            CodeLineDetails.IsUpCountry = 0
                        End If
                        publicDTbl.Clear()
                    End If
                End If
                'Check Voucher Code
                Select Case CountryCode.ToUpper.Trim
                    Case "UG"
                        ValToValidate = micrLine.Substring(24)
                    Case "SL"

                    Case "TZ"
                        If IsTypeCCheque = False Then
                            ValToValidate = micrLine.Substring(24)
                        Else
                            ValToValidate = micrLine.Substring(24)
                        End If
                    Case "RD"

                    Case "KE"
                        ValToValidate = micrLine.Substring(12, 2)
                    Case "ET"

                    Case "SA"

                End Select
                'MsgBox("imefika Voucher codes")

                If Not IsNumeric(ValToValidate) Then
                    RejectedReason = "Invalid Voucher Code"
                    CodeLineDetails.VoucherCode = ValToValidate
                ElseIf ValToValidate.ToString.Contains(" ") Then
                    RejectedReason = "Invalid Voucher Code"
                    CodeLineDetails.VoucherCode = ValToValidate
                ElseIf ValToValidate.ToString.Contains("!") Then
                    RejectedReason = "Invalid Voucher Code"
                    CodeLineDetails.VoucherCode = ValToValidate
                ElseIf ValToValidate.ToString.Contains("?") Then
                    RejectedReason = "Invalid Voucher Code"
                    CodeLineDetails.VoucherCode = ValToValidate
                Else
                    If RejectedReason = "" Then
                        RejectedReason = "ok"
                    End If
                    CodeLineDetails.VoucherCode = ValToValidate
                End If
                publicDTbl.Clear()
                'For Paramount get the correct Account
                Dim IsGL As Boolean = False
                If cTrxType = "I" Then
                    ExecuteData(GetModify("Proc_EJ" & OurBankID & "", "ourbranchid", CodeLineDetails.BranchID, "bankid", CodeLineDetails.BankID, "branchid", CodeLineDetails.BranchID, "AccountID", micrLine.Substring(14), "FileType", "J", "RecordType", "00"), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
                    If publicDTbl.Rows.Count > 0 Then
                        If publicDTbl.Rows(0)(1).ToString = "" Then RejectedReason = "Invalid Account" : GoTo q
                        CodeLineDetails.TheirAccountID = publicDTbl.Rows(0)(1).ToString
                        If CodeLineDetails.TheirAccountID.Substring(0, 1).Contains("G") Then
                            CodeLineDetails.TheirAccountID = CodeLineDetails.TheirAccountID.Substring(2)
                            IsGL = True
                        End If
                        CodeLineDetails.AccountID = CodeLineDetails.TheirAccountID
                    End If
                    publicDTbl.Clear()
                End If
                'If the voucher code is ok, now validate if this account accepts this voucher code
                If cTrxType = "O" Or cTrxType = "A" Then
                    ExecuteData(GetModify("sp_ClearingVoucherCodesCurrency", "AccountID", strAccountID, "OurBranchID", OurBranchID, "IsGLAccount", IsGL), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
                    If publicDTbl.Rows.Count > 0 Then
                        Select Case publicDTbl.Rows(0)(0).ToString.Trim.ToUpper
                            Case "USD"
                                If CodeLineDetails.VoucherCode <> 60 Then
                                    RejectedReason = "Cheque posted to a wrong currency account. This is a " & publicDTbl.Rows(0)(0) & " Account."
                                End If
                            Case "GBP"
                                If CodeLineDetails.VoucherCode <> 61 Then
                                    RejectedReason = "Cheque posted to a wrong currency account. This is a " & publicDTbl.Rows(0)(0) & " Account."
                                End If
                            Case "EUR"
                                If CodeLineDetails.VoucherCode <> 62 Then
                                    RejectedReason = "Cheque posted to a wrong currency account. This is a " & publicDTbl.Rows(0)(0) & " Account."
                                End If
                            Case "EURO"
                                If CodeLineDetails.VoucherCode <> 62 Then
                                    RejectedReason = "Cheque posted to a wrong currency account. This is a " & publicDTbl.Rows(0)(0) & " Account."
                                End If
                            Case "STG"
                                If CodeLineDetails.VoucherCode <> 61 Then
                                    RejectedReason = "Cheque posted to a wrong currency account. This is a " & publicDTbl.Rows(0)(0) & " Account."
                                End If
                            Case Else 'Local
                                Select Case CodeLineDetails.VoucherCode
                                    Case 60, 61, 62
                                        RejectedReason = "Cheque posted to a wrong currency account. This is a " & publicDTbl.Rows(0)(0) & " Account."
                                    Case Else
                                        RejectedReason = "ok"
                                End Select
                                CodeLineDetails.CurrencyCode = publicDTbl.Rows(0)(0).ToString.Trim.ToUpper
                        End Select
                    End If
                Else
                    ExecuteData(GetModify("sp_ClearingVoucherCodesCurrency", "AccountID", CodeLineDetails.TheirAccountID, "OurBranchID", CodeLineDetails.BranchID, "IsGLAccount", IsGL), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.SelectStatement)
                    If publicDTbl.Rows.Count > 0 Then
                        Select Case publicDTbl.Rows(0)(0).ToString.Trim.ToUpper
                            Case "USD"
                                If CodeLineDetails.VoucherCode <> 60 Then
                                    RejectedReason = "Cheque posted to a wrong currency account. This is a " & publicDTbl.Rows(0)(0) & " Account."
                                End If
                            Case "GBP"
                                If CodeLineDetails.VoucherCode <> 61 Then
                                    RejectedReason = "Cheque posted to a wrong currency account. This is a " & publicDTbl.Rows(0)(0) & " Account."
                                End If
                            Case "EUR"
                                If CodeLineDetails.VoucherCode <> 62 Then
                                    RejectedReason = "Cheque posted to a wrong currency account. This is a " & publicDTbl.Rows(0)(0) & " Account."
                                End If
                            Case "EURO"
                                If CodeLineDetails.VoucherCode <> 62 Then
                                    RejectedReason = "Cheque posted to a wrong currency account. This is a " & publicDTbl.Rows(0)(0) & " Account."
                                End If
                            Case "STG"
                                If CodeLineDetails.VoucherCode <> 61 Then
                                    RejectedReason = "Cheque posted to a wrong currency account. This is a " & publicDTbl.Rows(0)(0) & " Account."
                                End If
                            Case Else 'Local
                                Select Case CodeLineDetails.VoucherCode
                                    Case 60, 61, 62
                                        RejectedReason = "Cheque posted to a wrong currency account. This is a " & publicDTbl.Rows(0)(0) & " Account."
                                    Case Else
                                        RejectedReason = "ok"
                                End Select
                                CodeLineDetails.CurrencyCode = publicDTbl.Rows(0)(0).ToString.Trim.ToUpper
                        End Select
                    End If
                End If
                publicDTbl.Clear()
                'Validate AccountID
                Dim AccIDLnth As Int16 = 0
                Dim AccID As String = ""
                Select Case CountryCode.ToUpper.Trim
                    Case "UG"
                        AccID = micrLine.Substring(14, 10)
                        AccIDLnth = AccID.Length
                    Case "SL"

                    Case "TZ"
                        If IsTypeCCheque = False Then
                            AccID = micrLine.Substring(14, 10)
                            AccIDLnth = AccID.Length
                        Else
                            AccID = micrLine.Substring(14, 10)
                            AccIDLnth = AccID.Length
                        End If
                    Case "RD"

                    Case "KE"
                        AccID = micrLine.Substring(14)
                        AccIDLnth = AccID.Length

                    Case "ET"

                    Case "SA"

                End Select
                If CodeLineDetails.AccountID = "" And cTrxType = "I" Then
                    If AccIDLnth = 10 Then
                        ValToValidate = New String("0", 10 - AccIDLnth) & AccID
                        If Not IsNumeric(ValToValidate) Then
                            RejectedReason = "Invalid Account Number"
                            CodeLineDetails.TheirAccountID = ValToValidate
                        ElseIf ValToValidate.ToString.Contains(" ") Then
                            RejectedReason = "Invalid Account Number"
                            CodeLineDetails.TheirAccountID = ValToValidate
                        ElseIf ValToValidate.ToString.Contains("!") Then
                            RejectedReason = "Invalid Account Number"
                            CodeLineDetails.TheirAccountID = ValToValidate
                        ElseIf ValToValidate.ToString.Contains("?") Then
                            RejectedReason = "Invalid Account Number"
                            CodeLineDetails.TheirAccountID = ValToValidate
                        Else
                            CodeLineDetails.TheirAccountID = ValToValidate
                            If RejectedReason = "" Then
                                RejectedReason = "ok"
                            End If
                        End If
                    Else
                        RejectedReason = "Invalid Account Number"
                        CodeLineDetails.TheirAccountID = ValToValidate
                    End If
                ElseIf CodeLineDetails.AccountID = "" And (cTrxType = "O" Or cTrxType = "A") Then
                    If AccIDLnth = 10 Then
                        ValToValidate = New String("0", 10 - AccIDLnth) & AccID
                        If Not IsNumeric(ValToValidate) Then
                            RejectedReason = "Invalid Account Number"
                            CodeLineDetails.TheirAccountID = ValToValidate
                        ElseIf ValToValidate.ToString.Contains(" ") Then
                            RejectedReason = "Invalid Account Number"
                            CodeLineDetails.TheirAccountID = ValToValidate
                        ElseIf ValToValidate.ToString.Contains("!") Then
                            RejectedReason = "Invalid Account Number"
                            CodeLineDetails.TheirAccountID = ValToValidate
                        ElseIf ValToValidate.ToString.Contains("?") Then
                            RejectedReason = "Invalid Account Number"
                            CodeLineDetails.TheirAccountID = ValToValidate
                        Else
                            CodeLineDetails.TheirAccountID = ValToValidate
                            If RejectedReason = "" Then
                                RejectedReason = "ok"
                            End If
                        End If
                    Else
                        RejectedReason = "Invalid Account Number"
                        CodeLineDetails.TheirAccountID = ValToValidate
                    End If
                Else
                    If IsGL = True Then

                    Else
                        CodeLineDetails.TheirAccountID = New String("0", 10 - AccIDLnth) & AccID
                    End If
                End If
q:
                If IsGL = True Then
                    CodeLineDetails.TheirAccountID = "G-" & CodeLineDetails.TheirAccountID
                    CodeLineDetails.AccountID = CodeLineDetails.TheirAccountID
                    IsGL = False
                End If
                'Validate ChequeDigit
                Select Case CountryCode.ToUpper.Trim
                    Case "UG"
                        CodeLineDetails.ChequeDigit = micrLine.Substring(6, 2)
                    Case "SL"

                    Case "TZ"
                        If IsTypeCCheque = False Then
                            CodeLineDetails.ChequeDigit = micrLine.Substring(12, 2)
                        Else
                            CodeLineDetails.ChequeDigit = "00"
                        End If
                    Case "RD"

                    Case "KE"
                        CodeLineDetails.ChequeDigit = micrLine.Substring(11, 1)
                    Case "ET"

                    Case "SA"

                End Select
                If isMDV = False Then

                    If GetChequeDigit(CodeLineDetails.TheirAccountID, CodeLineDetails.BankID, CodeLineDetails.BranchID, CodeLineDetails.CountryClearingCenter, CountryCode) <> CodeLineDetails.ChequeDigit Then
                        If MsgBox("Invalid Cheque Digit. The correct one is : " & GetChequeDigit(CodeLineDetails.TheirAccountID, CodeLineDetails.BankID, CodeLineDetails.BranchID, CodeLineDetails.CountryClearingCenter, CountryCode) & vbCrLf & "Do you wish to adopt invalid one? This will pick the value captured.", vbYesNo) = vbYes Then
                            If RejectedReason = "" Then
                                RejectedReason = "ok"
                            End If
                            MicrEditted = True
                            'CodeLineDetails.ChequeDigit = GetChequeDigit(CodeLineDetails.TheirAccountID, CodeLineDetails.BankID, CodeLineDetails.BranchID, CodeLineDetails.CountryClearingCenter, CountryCode)
                        Else
                            RejectedReason = "Invalid Cheque Digit. With this bankID " & CodeLineDetails.BankID & " and BranchID " & CodeLineDetails.BranchID & " It should be - " & GetChequeDigit(CodeLineDetails.TheirAccountID, CodeLineDetails.BankID, CodeLineDetails.BranchID, CodeLineDetails.CountryClearingCenter, CountryCode) & ""
                        End If
                    End If
                Else
                    'CodeLineDetails.ChequeDigit = micrLine.Substring(11, 1)
                End If
            End If
            CodeLineDetails.CountryClearingCenter = ClrHSID
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        micrLine = ""
        'MsgBox("imefika mwisho")
        Return RejectedReason
    End Function

    Public Shared Sub Wait(ByVal Seconds As Double)
        Dim timeOut As DateTime = Now.AddSeconds(Seconds)
        Do
            System.Windows.Forms.Application.DoEvents()
        Loop Until Now > timeOut
    End Sub
    Public Shared Function ExecuteData(ByVal pubSqlString As String, ByRef pubDataTable As DataTable, ByVal dtExecType As dataExecTypes, ByVal qType As queryType, Optional ByRef pubDataSet As DataSet = Nothing) As Boolean
        LastErrorMessage = ""
        Try
            BRDbType = systemDbTypes.dbTypeSql
            Dim strSystem As String = ConfigurationManager.AppSettings("sysType")
            If ConfigurationManager.AppSettings("sysType") = "BRNETOLD" Then
                'strSystem = strSystemD.DecyptKey(strPathEncpt).ToString()
            End If
            GetDbConnectionStrings(ConfigurationManager.AppSettings("SystemUser"))
            If pubSqlString.Trim = "" Then
                MsgBox("Query String Can Not Be Blank!", MsgBoxStyle.Exclamation)
                Return False
            End If
            pubDataTable = New DataTable
            pubDataSet = New DataSet
            Select Case BRDbType
                Case systemDbTypes.dbTypeSql
                    If ConfigurationManager.AppSettings("sysType") = "BRNET" Then
                        If pubDbSqlConn.State = ConnectionState.Open = False Then
                            'MessageBox.Show(DBServerName + ">>>>>" + DatabaseName + "------" + "4")
                            Try
                                Dim strConn As String()
                                'Dim strSystem As String = ConfigurationManager.AppSettings("strSystem").Trim().ToUpper()
                                Dim strDBServerName As String = DBServerName.Replace("'", "")
                                Dim strDatabaseName As String = DatabaseName.Replace("'", "")
                                Dim strBRUserName As String = BRUserName.Replace(" ", "+").Replace("'", "")
                                Dim strBRUserPassword As String = DBPassword.Replace(" ", "+").Replace("'", "")
                                Dim strConnectString = ""
                                Dim Conn As SqlConnection = New SqlConnection()
                                Conn = BRAccess.BRConnection(strBRUserName, strBRUserPassword, strDatabaseName, strDBServerName)
                                If Conn.State <> ConnectionState.Open Then
                                    Conn.Open()
                                End If
                                pubDbSqlConn = Conn
                            Catch ex As Exception
                                MessageBox.Show(DBServerName + ">>>>>" + DatabaseName + "------" + "Connection")
                            End Try



                        End If
                    ElseIf ConfigurationManager.AppSettings("sysType") = "BRNETOLD" Then
                        If pubDbSqlConn.State = ConnectionState.Open = False Then
                            pubDbSqlConn = New System.Data.SqlClient.SqlConnection(strSystem)
                            If pubDbSqlConn.State <> ConnectionState.Open Then
                                pubDbSqlConn.Open()
                            End If
                        End If
                    Else
                        If pubDbSqlConn.State = ConnectionState.Open = False Then
                            pubDbSqlConn.ConnectionString = CnString
                            If pubDbSqlConn.State <> ConnectionState.Open Then
                                pubDbSqlConn = GetConnectionSQL(CnString)
                            End If
                        End If
                    End If
                    Select Case qType
                        Case queryType.SelectStatement
                            pubDataSqlCommand = New SqlClient.SqlCommand(pubSqlString, pubDbSqlConn)
                            If pubDataSqlCommand.Connection.State = ConnectionState.Closed Then
                                pubDataSqlCommand.Connection.Open()
                            End If
                            If dtExecType = dataExecTypes.ExecTypeNonQuery Then
                                pubDataSqlCommand.ExecuteNonQuery()
                            Else
                                pubDataSqlAdapter = New SqlClient.SqlDataAdapter(pubSqlString, pubDbSqlConn)
                                pubDataSqlAdapter.Fill(pubDataSet)
                                pubDataTable = pubDataSet.Tables(0)
                            End If
                        Case queryType.StoredProcedure

                            pubDataSqlCommand = New SqlClient.SqlCommand(pubSqlString, pubDbSqlConn)
                            pubDataSqlCommand.CommandType = CommandType.StoredProcedure
                            If pubDataSqlCommand.Connection.State = ConnectionState.Closed Then
                                pubDataSqlCommand.Connection.Open()
                            End If
                            If dtExecType = dataExecTypes.ExecTypeNonQuery Then
                                pubDataSqlCommand.ExecuteNonQuery()
                            Else
                                pubDataSqlAdapter = New SqlClient.SqlDataAdapter(pubSqlString, pubDbSqlConn)
                                pubDataSqlAdapter.Fill(pubDataSet)
                                pubDataTable = pubDataSet.Tables(0)
                            End If
                    End Select
                Case systemDbTypes.dbTypeOracle
                    pubDataOraCommand = New OracleClient.OracleCommand(pubSqlString, pubDbOraConn)
                    If pubDataOraCommand.Connection.State = ConnectionState.Closed Then
                        pubDataOraCommand.Connection.Open()
                    End If
                    If dtExecType = dataExecTypes.ExecTypeNonQuery Then
                        pubDataOraCommand.ExecuteNonQuery()
                    Else
                        '------------ Concantinating in oracle ---------
                        If pubSqlString.IndexOf("+") > 0 Then pubSqlString = pubSqlString.Replace("+", "||")
                        pubDataOraAdapter = New OracleClient.OracleDataAdapter(pubSqlString, pubDbOraConn)
                        pubDataOraAdapter.Fill(pubDataSet)
                        pubDataTable = pubDataSet.Tables(0)
                    End If
            End Select
            Return True
        Catch ex As Exception
            'MessageBox.Show("Error Occoured, check file C:\ClearingFilesErrorLog, The proceess though will proceed for other transactions")
            Dim AppendErrorMessage As String = "Error Message ClearingFilesDataManuplation: Clearing Files"
            AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "Date" + ":" + DateTime.Now
            AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "--------------------------"
            AppendErrorMessage = AppendErrorMessage + Environment.NewLine + ex.Message.ToString()
            AppendErrorMessage = AppendErrorMessage + Environment.NewLine + pubSqlString
            AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "--------------------------"
            AppendErrorMessage = AppendErrorMessage + Environment.NewLine
            If Not Directory.Exists("C:\ClearingFilesErrorLog") Then
                Directory.CreateDirectory("C:\ClearingFilesErrorLog")
            End If
            LastErrorMessage = "Database Error: " & ex.Message
            System.IO.File.AppendAllText("C:\ClearingFilesErrorLog\" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + ".txt", AppendErrorMessage)
            Return False
        Finally
            pubDataSqlCommand.Dispose()
        End Try
    End Function
    Private Shared Function GetCheckSum(strData As String) As Long
        Dim tmpKey(0 To 7) As Byte
        Dim crcUser As Long, crcKey As Long

        strData = Trim(strData)
        crcUser = CRC16(strData)

        GetCheckSum = crcUser
    End Function
    Private Shared Function CRC16(B As String) As Long
        Dim Power(0 To 7) As Byte
        Dim i As Integer, j As Integer
        Dim ByteVal As Byte
        Dim TestBit As Boolean
        Dim CRC As Long

        For i = 0 To 7
            Power(i) = 2 ^ i
        Next i
        CRC = 0
        For i = 1 To Len(B)
            ByteVal = Asc(Mid$(B, i, 1))
            For j = 7 To 0 Step -1
                TestBit = ((CRC And 32768) = 32768) Xor ((ByteVal And Power(j)) = Power(j))
                CRC = ((CRC And 32767&) * 2&)
                If TestBit Then CRC = CRC Xor &H1021
            Next j
        Next i
        CRC16 = CRC
    End Function
    Public Shared Function GetConnection(Optional ByVal strSystem As String = "") As IDbConnection
        Dim connection As IDbConnection = Nothing
        'MessageBox.Show("Iko huku")

        'MessageBox.Show(DBServerName + "<<<<>>>>" + DatabaseName + ">>>" + "Three")
        Try
            If Modscan.SysType = Modscan.ENUM_SysType.BRNETOLD Then
                'MessageBox.Show("Iko 1")
                Dim strConnectString As String = strSystem
                strConnectString = strSystem
                connection = New System.Data.SqlClient.SqlConnection(strConnectString)
                If connection.State <> ConnectionState.Open Then
                    connection.Open()
                End If
            ElseIf Modscan.SysType = Modscan.ENUM_SysType.BR Then
                'MessageBox.Show("Iko 2")
                BRDbType = systemDbTypes.dbTypeSql
                Select Case UCase(BRDbType)
                    Case systemDbTypes.dbTypeOracle
                        If Debugger.IsAttached = False Then
                            CnString = "Data Source=" & DBServerName & ";Initial Catalog=" & DatabaseName & " ;User ID=realm;Password=" & DBPassword & ";MultipleActiveResultSets=true"
                        Else
                            CnString = "Data Source=" & DBServerName & ";Initial Catalog=" & DatabaseName & " ;User ID=realm;Password=" & DBPassword & ";MultipleActiveResultSets=true"
                        End If
                    Case systemDbTypes.dbTypeSql
                        If Debugger.IsAttached = False Then
                            CnString = "Data Source=" & DBServerName & ";Initial Catalog=" & DatabaseName & " ;User ID=realm;Password=" & DBPassword & ";MultipleActiveResultSets=true"
                        Else
                            CnString = "Data Source=" & DBServerName & ";Initial Catalog=" & DatabaseName & " ;User ID=realm;Password=" & DBPassword & ";MultipleActiveResultSets=true"
                        End If
                End Select
                SqlConnection.ClearAllPools()
                connection = New System.Data.SqlClient.SqlConnection(CnString)
                'MessageBox.Show(CnString)
                If connection.State <> ConnectionState.Open Then
                    connection.Open()
                End If
            Else
                'MessageBox.Show(connection.State + " Imetoka kwa connection poa")
                'Modscan.DBServerName = ConfigurationManager.AppSettings("strDBServerName")
                'Modscan.DatabaseName = ConfigurationManager.AppSettings("strDatabaseName")
                'Modscan.BRUserName = ConfigurationManager.AppSettings("strBRUserName")
                'Modscan.DBPassword = ConfigurationManager.AppSettings("strBRUserPassword")
                'Modscan.OurBranchID = ConfigurationManager.AppSettings("HeadOfficeBranchID")
                'Modscan.OurBankID = ConfigurationManager.AppSettings("BankID")
                Dim strDBServerName As String = DBServerName.Replace("'", "")
                Dim strDatabaseName As String = DatabaseName.Replace("'", "")
                Dim strBRUserName As String = BRUserName.Replace("'", "").Replace(" ", "+")
                Dim strBRUserPassword As String = DBPassword.Replace(" ", "+").Replace("'", "")
                connection = BRAccess.BRConnection(strBRUserName, strBRUserPassword, strDatabaseName, strDBServerName)
                If connection.State <> ConnectionState.Open Then
                    connection.Open()
                End If
            End If
            'MessageBox.Show(connection.State + " Imetoka kwa connection poa")
        Catch ex As Exception
            MessageBox.Show(connection.State + " Imechapa")
        End Try
        'MessageBox.Show(connection.State)
        Return connection
    End Function
    Private Protected Shared Function GetConfigConnDetails()
        Modscan.SysType = Modscan.ENUM_SysType.BRNET
        Try
            Dim configFilePath As String = ConfigurationManager.AppSettings("configFilePath")
            Dim strSystem As String = ConfigurationManager.AppSettings("BRSystem")

            If String.IsNullOrEmpty(DBServerName) Then
                Dim fileMap As New ExeConfigurationFileMap()
                fileMap.ExeConfigFilename = configFilePath

                Dim configuration As Configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None)

                DBServerName = configuration.AppSettings.Settings(strSystem + "-DBServerName").Value
                DatabaseName = configuration.AppSettings.Settings(strSystem + "-DatabaseName").Value
                BRUserName = configuration.AppSettings.Settings(strSystem + "-BRUserName").Value
                DBPassword = configuration.AppSettings.Settings(strSystem + "-BRUserPassword").Value

            End If

        Catch ex As Exception

        End Try

    End Function
    Public Shared Function GetConnectionSQL(Optional ByVal strSystem As String = "") As SqlConnection
        Dim connection As SqlConnection = Nothing
        If strSystem = "" Then
            GetConfigConnDetails()
        End If
        'MessageBox.Show(DBServerName + "<<<<>>>>" + DatabaseName + ">>>" + "One")
        If Modscan.SysType = Modscan.ENUM_SysType.BRNETOLD Then
            Dim strConnectString As String = strSystem
            strConnectString = strSystem
            connection = New System.Data.SqlClient.SqlConnection(strConnectString)
            If connection.State <> ConnectionState.Open Then
                connection.Open()
            End If
        ElseIf Modscan.SysType = Modscan.ENUM_SysType.BR Or Modscan.SysType = Modscan.ENUM_SysType.BRMFO Then
            Try
                If Modscan.SysType = Modscan.ENUM_SysType.BRNET Or Modscan.SysType = Modscan.ENUM_SysType.BRNETOLD Then
                    OpenConnections(strSystem)
                Else

                    BRDbType = systemDbTypes.dbTypeSql
                    Select Case UCase(BRDbType)
                        Case systemDbTypes.dbTypeOracle
                            If Debugger.IsAttached = False Then
                                CnString = "Data Source=" & DBServerName & ";Initial Catalog=" & DatabaseName & " ;User ID=realm;Password=" & DBPassword & ";MultipleActiveResultSets=true"
                            Else
                                CnString = "Data Source=" & DBServerName & ";Initial Catalog=" & DatabaseName & " ;User ID=realm;Password=" & DBPassword & ";MultipleActiveResultSets=true"
                            End If
                        Case systemDbTypes.dbTypeSql
                            If Debugger.IsAttached = False Then
                                CnString = "Data Source=" & DBServerName & ";Initial Catalog=" & DatabaseName & " ;User ID=realm;Password=" & DBPassword & ";MultipleActiveResultSets=true"
                            Else
                                CnString = "Data Source=" & DBServerName & ";Initial Catalog=" & DatabaseName & " ;User ID=realm;Password=" & DBPassword & ";MultipleActiveResultSets=true"
                            End If
                    End Select
                End If
                connection = New System.Data.SqlClient.SqlConnection(CnString)
                SqlConnection.ClearAllPools()
                If connection.State <> ConnectionState.Open Then
                    connection.Open()
                End If
            Catch ex As Exception
                MsgBox(ex.Message)
                MsgBox("Invalid connection settings", MsgBoxStyle.Information, Nothing)
            End Try
        Else
            'Modscan.DBServerName = ConfigurationManager.AppSettings("strDBServerName")
            'Modscan.DatabaseName = ConfigurationManager.AppSettings("strDatabaseName")
            'Modscan.BRUserName = ConfigurationManager.AppSettings("strBRUserName")
            'Modscan.DBPassword = ConfigurationManager.AppSettings("strBRUserPassword")
            Dim strDBServerName As String = DBServerName.Replace("'", "")
            Dim strDatabaseName As String = DatabaseName.Replace("'", "")
            Dim strBRUserName As String = BRUserName.Replace("'", "").Replace(" ", "+")
            Dim strBRUserPassword As String = DBPassword.Replace(" ", "+").Replace("'", "")
            connection = BRAccess.BRConnection(strBRUserName, strBRUserPassword, strDatabaseName, strDBServerName)
            If IsNothing(connection) Then
                CnString = "Data Source=" & DBServerName & ";Initial Catalog=" & DatabaseName & " ;User ID=" & BRAccess.BRUserName(strBRUserName) & " ;Password=" & BRAccess.BRUserPassword(strBRUserPassword) & ";MultipleActiveResultSets=true"
                connection = New System.Data.SqlClient.SqlConnection(CnString)
                SqlConnection.ClearAllPools()
            End If

            If connection.State <> ConnectionState.Open Then
                connection.Open()
            End If
        End If

        Return connection
    End Function
    Public Shared Sub FormatGrids(ByRef GridName As DataGridView)
        Try
            Dim row As DataGridViewRow
            Dim x As Integer = 0
            For x = 0 To GridName.Columns.Count - 1
                GridName.Columns(x).DefaultCellStyle.BackColor = System.Drawing.Color.WhiteSmoke
                If x < GridName.ColumnCount - 1 Then
                    x = x + 1
                    GridName.Columns(x).DefaultCellStyle.BackColor = System.Drawing.Color.PapayaWhip
                    If x < GridName.ColumnCount - 1 Then
                        x = x + 1
                        GridName.Columns(x).DefaultCellStyle.BackColor = System.Drawing.Color.LightYellow
                    End If
                End If
            Next
        Catch ex As Exception
            MessageBox.Show(ex.Message & "Formating Grids")
        End Try
    End Sub
    Public Shared Sub GetDbConnectionStrings(Optional ByVal strSystem As String = "")
        Try
            'MessageBox.Show(DBServerName + "<<<<>>>>" + DatabaseName + ">>>" + "two")


            If Modscan.SysType = Modscan.ENUM_SysType.BRNET Or Modscan.SysType = Modscan.ENUM_SysType.BRNETOLD Then
                OpenConnections(strSystem)
            Else

                BRDbType = systemDbTypes.dbTypeSql
                Select Case UCase(BRDbType)
                    Case systemDbTypes.dbTypeOracle
                        If Debugger.IsAttached = False Then
                            CnString = "Data Source=" & DBServerName & ";Initial Catalog=" & DatabaseName & " ;User ID=realm;Password=" & DBPassword & ";MultipleActiveResultSets=true"
                        Else
                            CnString = "Data Source=" & DBServerName & ";Initial Catalog=" & DatabaseName & " ;User ID=realm;Password=" & DBPassword & ";MultipleActiveResultSets=true"
                        End If
                    Case systemDbTypes.dbTypeSql
                        If Debugger.IsAttached = False Then
                            CnString = "Data Source=" & DBServerName & ";Initial Catalog=" & DatabaseName & " ;User ID=realm;Password=" & DBPassword & ";MultipleActiveResultSets=true"
                        Else
                            CnString = "Data Source=" & DBServerName & ";Initial Catalog=" & DatabaseName & " ;User ID=realm;Password=" & DBPassword & ";MultipleActiveResultSets=true"
                        End If
                End Select
                OpenConnections(CnString)
                'MessageBox.Show(DBServerName + "------" + "2")
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
            'MessageBox.Show(DBServerName + "------" + "3")
            MsgBox("Invalid connection settings", MsgBoxStyle.Information, Nothing)
        End Try
    End Sub


    Public Sub SearchCombo(ByVal CMB As ComboBox, ByVal SearchItem As String)
        Try
            CMB.Text = ""
            Dim x As Integer = 0
            For x = 0 To CMB.Items.Count - 1
                If Trim(CMB.Items(x).ToString) = Trim(SearchItem) Then
                    CMB.SelectedIndex = x
                    Exit For
                End If
            Next x
        Catch ex As Exception
            MessageBox.Show("Error Loading Items to the Selected Combo Box.", Nothing, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Public Shared Function RoundTo5Cents(ByVal x As Decimal) As Decimal
        Try
            x = FormatNumber(x, 2)
            x = x * 100
            x = Int(x / 5)  ' Get the Integer portion of the result
            x = x * 5 / 100 ' Divide the result by 5 and multiply by 100
            RoundTo5Cents = x
        Catch ex As Exception
            MessageBox.Show("Round off error trying to round off amounts to the nearest five cent", Nothing, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Function
    Public Shared Sub OpenConnections(Optional ByVal strSystem As String = "")
        Try
            Dim connection As SqlConnection = New SqlConnection()
            Try
                If ConfigurationManager.AppSettings("sysType") = "BRNET" Then
                    Dim strConn(4) As String
                    If String.IsNullOrEmpty(DBServerName) Then
                        GetConfigConnDetails()
                    End If
                    'Modscan.DBServerName = ConfigurationManager.AppSettings("strDBServerName")
                    'Modscan.DatabaseName = ConfigurationManager.AppSettings("strDatabaseName")
                    'Modscan.BRUserName = ConfigurationManager.AppSettings("strBRUserName")
                    'Modscan.DBPassword = ConfigurationManager.AppSettings("strBRUserPassword")
                    ''Dim strSystem As String = ConfigurationManager.AppSettings("strSystem").Trim().ToUpper()
                    Dim strDBServerName As String = DBServerName.Replace("'", "")
                    Dim strDatabaseName As String = DatabaseName.Replace("'", "")
                    Dim strBRUserName As String = BRUserName.Replace("'", "")
                    'MessageBox.Show("Hapa 1 " + DBServerName + DatabaseName + BRUserName)
                    Dim strBRUserPassword As String = DBPassword.Replace(" ", "+").Replace("'", "")
                    Dim strConnectString = ""
                    connection = BRAccess.BRConnection(strBRUserName, DBPassword, DatabaseName, DBServerName)
                ElseIf ConfigurationManager.AppSettings("sysType") = "BRNETOLD" Or ConfigurationManager.AppSettings("sysType") = "BR" Or ConfigurationManager.AppSettings("sysType") = "BRMFO" Then
                    'MessageBox.Show("Hapa 2 ")
                    Dim strConnectString As String = strSystem
                    strConnectString = strSystem
                    SqlConnection.ClearAllPools()
                    connection = New System.Data.SqlClient.SqlConnection(strConnectString)
                End If
                If connection.State <> ConnectionState.Open Then
                    connection.Open()
                End If
            Catch ex As Exception
                'MessageBox.Show("Wrong configurations. Please Consult Your System Administrator. " & ex.Message, Modscan.MsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End Try
            'Select Case UCase(BRDbType)
            '    Case 0
            '        Try
            '            OracleConn = New OracleConnection(CnString)
            '            Try
            '                If SqlConn.State <> ConnectionState.Open Then
            '                    SqlConn.Open()
            '                End If
            '            Catch ex As Exception

            '            End Try

            '        Catch ex As OracleException
            '            MessageBox.Show("Unable To Open Connections To The Oracle Database. Please Consult Your System Administrator. " & ex.Message, Modscan.MsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
            '            Exit Sub
            '        End Try
            '    Case 1
            '        Try
            '            If Modscan.SysType <> Modscan.ENUM_SysType.BRNET Then
            '                SqlConn = New SqlConnection(CnString)
            '                If SqlConn.State <> ConnectionState.Open Then
            '                    SqlConn.Open()
            '                End If
            '            End If
            '        Catch ex As Exception
            '            MessageBox.Show("Unable To Open Connections To The SQL Database. Please Consult Your System Administrator. " & ex.Message, Modscan.MsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
            '            Exit Sub
            '        End Try
            'End Select
        Catch ex As Exception
            MsgBox("Unable To Open Connections To The SQL Database. Please Consult Your System Administrator. ", MsgBoxStyle.Exclamation, Nothing)
            Exit Sub
        End Try
    End Sub
    Public Shared Function GenerateParameters(ByVal m_Array As Object) As String
        Dim StrSql As String = ""
        Dim IntI As Integer
        Dim intNoOFElementPassed As Integer
        Const cEQUALTO = "="
        Const cCOMMA = ","
        Const cCOLON = "'"
        Const cATE = "@"
        intNoOFElementPassed = UBound(m_Array, 1)
        If intNoOFElementPassed <= 0 Then
            Exit Function
        End If
        For IntI = 0 To intNoOFElementPassed Step 2
            If Trim(m_Array(IntI)) <> cNo_Text Then
                If VarType(m_Array(IntI + 1)) = vbCurrency Then
                    StrSql = StrSql & cATE & m_Array(IntI) & cEQUALTO & m_Array(IntI + 1) & cCOMMA
                ElseIf VarType(m_Array(IntI + 1)) = vbDouble Then
                    StrSql = StrSql & cATE & m_Array(IntI) & cEQUALTO & m_Array(IntI + 1) & cCOMMA
                ElseIf VarType(m_Array(IntI + 1)) = VariantType.Integer Then
                    StrSql = StrSql & cATE & m_Array(IntI) & cEQUALTO & m_Array(IntI + 1) & cCOMMA
                Else
                    StrSql = StrSql & cATE & m_Array(IntI) & cEQUALTO & cCOLON & m_Array(IntI + 1) & cCOLON & cCOMMA
                End If
            End If
        Next IntI
        GenerateParameters = Left(StrSql, Len(StrSql) - 1)
    End Function
    Public Shared Function IsDivisbleByTwo(ByVal m_value As Long, Optional ByVal m_base1 As Boolean = True) As Boolean
        If m_base1 Then
            m_value = m_value + 1
        End If
        If ((m_value Mod 2) = 0) Then
            IsDivisbleByTwo = True
        Else
            IsDivisbleByTwo = False
        End If
    End Function
    Public Shared Function GetModify(ByVal m_sp_Name As String, ByVal ParamArray m_ParamName_ParamValue() As Object) As String
        Dim StrSql As String = ""
        If Trim(m_sp_Name) = cNo_Text Then
            MsgBox("No stored procedure name specified")
            Exit Function
        End If
        GetModify = False
        If IsDivisbleByTwo(UBound(m_ParamName_ParamValue, 1)) Then
            StrSql = "exec " & m_sp_Name & " " & GenerateParameters(m_ParamName_ParamValue) & ""
        End If
        'MessageBox.Show(StrSql)
        Return StrSql
    End Function
    Shared rnd As New Random()
    Public Shared usedStrings As New List(Of String)()
    Public Shared usedInt16 As New List(Of Int16)()

    Public Shared Function GetNextInt16() As String
        Dim theInt16 As Int16 = GetRandomInt16()
        While usedInt16.Contains(theInt16)
            theInt16 = GetRandomInt16()
        End While
        usedInt16.Add(theInt16)
        Return theInt16
    End Function

    Public Shared Function GetRandomInt16() As Int16
        Dim rndInt16 As Int16 = 0
        For i As Integer = 0 To 5
            If rnd.Next(0, 2) = 0 Then
                rndInt16 += Microsoft.VisualBasic.Int(rnd.Next(65, 91))
            Else
                rndInt16 += Microsoft.VisualBasic.Int(rnd.Next(48, 58))
            End If
        Next
        Return rndInt16
    End Function
    Public Shared Function GetNextString() As String
        Dim theString As String = GetRandomString()
        While usedStrings.Contains(theString)
            theString = GetRandomString()
        End While
        usedStrings.Add(theString)
        Return theString
    End Function

    Public Shared Function GetRandomString() As String
        Dim rndString As String = ""
        For i As Integer = 0 To 5
            If rnd.Next(0, 2) = 0 Then
                rndString += Microsoft.VisualBasic.Chr(rnd.Next(65, 91))
            Else
                rndString += Microsoft.VisualBasic.Chr(rnd.Next(48, 58))
            End If
        Next
        Return rndString
    End Function
    Shared Function GetDataTableContent(ByVal dgvOutCredit As DataGridView) As System.Data.DataTable
        Dim dr As System.Data.DataRow = Nothing
        Dim col As System.Windows.Forms.DataGridViewColumn
        Dim gridRow As System.Windows.Forms.DataGridViewRow = Nothing
        Dim cola As DataColumn = New DataColumn

        Try

            dt = New DataTable()
            dt.Rows.Clear()
            dt.TableName = "BrOutwardCredits"
            For Each col In dgvOutCredit.Columns
                Select Case col.Name
                    Case "ColJRImageSignature", "ColTFImageSignature", "ColJFImageSignature"
                        dt.Columns.Add(col.Name)
                    Case Else
                        dt.Columns.Add(col.Name)
                End Select
            Next
            For Each gridRow In dgvOutCredit.Rows
                If (gridRow.IsNewRow) Then
                    Continue For
                Else
                    dr = dt.NewRow()
                    For i As Int32 = 0 To dgvOutCredit.Columns.Count - 1
                        dr(i) = IIf(IsDBNull(gridRow.Cells(i).Value), DBNull.Value, gridRow.Cells(i).Value)
                    Next
                    dt.Rows.Add(dr)
                End If
            Next
            Return dt
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Function

    Public Function GetScalarREC(ByVal strSCALARsql As String) As String
        Dim result As String = ""
        Dim cmdSaveSQL As SqlClient.SqlCommand
        Dim cmdSaveOracle As OracleClient.OracleCommand
        Try
            Select Case BRDbType
                Case systemDbTypes.dbTypeSql
                    cmdSaveSQL = New SqlClient.SqlCommand(strSCALARsql, SqlConn)
                    If cmdSaveSQL.Connection.State = ConnectionState.Closed Then
                        cmdSaveSQL.Connection.Open()
                    End If
                    Try
                        result = IIf(IsDBNull(cmdSaveSQL.ExecuteScalar), "", cmdSaveSQL.ExecuteScalar)
                    Catch ex As Exception

                    End Try

                    If result = Nothing Then
                        result = ""
                    End If
                Case systemDbTypes.dbTypeOracle
                    cmdSaveOracle = New OracleClient.OracleCommand(strSCALARsql, OracleConn)
                    If cmdSaveOracle.Connection.State = ConnectionState.Closed Then
                        cmdSaveOracle.Connection.Open()
                    End If
                    result = IIf(IsDBNull(cmdSaveOracle.ExecuteScalar), "", cmdSaveOracle.ExecuteScalar)
                    If result = Nothing Then
                        result = ""
                    End If
            End Select
        Catch ex As Exception
            MessageBox.Show("Error encountered during retrieving a requested record. " & ex.Message, Nothing, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return ""
        End Try
        Return result
    End Function

    Public Sub WriteDataToXML(ByVal dtBrOutClearing As Data.DataTable)
        Try
            'these two are for encryption purposes. they help me to kill two birds with one stone ie , the data is encrypted and compressed
            'Dim ds As System.IO.Compression.DeflateStream
            Dim fs As System.IO.FileStream
            Dim MyXMLFile As String = ""
            'Now hapa na write the XML file
            ExecuteData(GetModify("SP_GetSystem", "ourBranchID", MyBranchID), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
            If publicDTbl.Rows.Count > 0 Then
                MyXMLFile = publicDTbl.Rows(0)("ChequeImagePath").ToString.Trim
                MyXMLFile = MyXMLFile & "BrClearing.xml"
            End If
            publicDTbl.Clear()
            If dtBrOutClearing.Rows.Count <> 0 Then
                If dtBrOutClearing.Rows.Count <> 0 Then
                    'Then na compression kiasi. this takes care of encryption as well
                    fs = New System.IO.FileStream(MyXMLFile, FileMode.OpenOrCreate, FileAccess.Write)
                    dtBrOutClearing.WriteXml(fs, XmlWriteMode.WriteSchema)
                    fs.Close()
                    fs.Dispose()
                End If
            Else
                MessageBox.Show("Failed Exporting to xml", Nothing, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End If
            dtBrOutClearing.Dispose()
        Catch ex As Exception
            MessageBox.Show("Error Source: " & ex.Source & Chr(13) &
            "Specific Description: " & ex.Message & Chr(13) &
            "Error Message: " & "Error Creating xml File Please Consult Your System Administrator.", Nothing, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1)
        End Try

    End Sub
    Public Shared Function ACHResponse(ByVal dtBrOutClearing As DataTable)
        Try
            Dim dr As Data.DataRow
            Dim Command As SqlCommand
            If dtBrOutClearing.Rows.Count > 0 Then
                For Each dr In dtBrOutClearing.Rows
                    Try
                        If Modscan.SysType = Modscan.ENUM_SysType.BRNET Then
                            SqlConn = GetConnectionSQL()
                            Command = New SqlCommand("p_AddACHResponse", SqlConn)
                        Else

                        End If
                        Command.CommandType = CommandType.StoredProcedure
                        Command.Parameters.Add("@OrgnlInstrID", SqlDbType.NVarChar).Value = dr("OrgnlInstrID")
                        Command.Parameters.Add("@OrgnlEndToEnd", SqlDbType.NVarChar).Value = dr("OrgnlEndToEnd")
                        Command.Parameters.Add("@OrgnTrxID", SqlDbType.NVarChar).Value = dr("OrgnTrxID")
                        Command.Parameters.Add("@RetCode", SqlDbType.NVarChar).Value = dr("RetCode")
                        Command.Parameters.Add("@RtrdIntrBkSttlmAmt", SqlDbType.NVarChar).Value = dr("RtrdIntrBkSttlmAmt")
                        Command.Parameters.Add("@RtrId", SqlDbType.NVarChar).Value = dr("RtrId")
                        Command.Parameters.Add("@FileName", SqlDbType.NVarChar).Value = dr("FileName")
                        SqlConn = GetConnectionSQL()
                        Command.ExecuteNonQuery()
                    Catch ex As Exception
                        MessageBox.Show("Failed uploading file " + dr("FileName") + "Error message " + ex.Message)
                    End Try

                Next
            End If
        Catch ex As Exception

        End Try
    End Function
    Public Shared Function SaveToDB(ByVal dtBrOutClearing As DataTable, Optional ByVal strModule As String = "") As Boolean
        Dim Command As SqlCommand
        Dim dr As Data.DataRow
        Dim TFImage As String
        Dim JFImage As String
        Dim JRImage As String
        Dim TFImageSignature As String
        Dim JFImageSignature As String
        Dim JRImageSignature As String
        Dim UVImage As String
        Dim ValueD As DateTime
        Dim ClearingD As String
        Dim DestAccount As String = ""
        Dim x As New System.Random

        If String.IsNullOrEmpty(cWorkingDate) Then
            cWorkingDate = DateTime.Now.Date.ToString()
        End If
        Dim CFDate As Date = Convert.ToDateTime(Modscan.cFromDate)
        Dim cWorkgDate As Date = Convert.ToDateTime(cWorkingDate)
        Dim CTDate As DateTime = Convert.ToDateTime(Modscan.cToDate)

        Modscan.cFromDate = CFDate.ToString("dd-MMM-yyyy")
        Modscan.cToDate = CTDate.ToString("dd-MMM-yyyy")
        cWorkingDate = cWorkgDate.ToString("dd-MMM-yyyy")

        'MessageBox.Show(cWorkingDate + " : " + Modscan.cToDate + " : " + Modscan.cFromDate + " : " + WORKING_DATE.ToString())
        Try
            'MessageBox.Show("Imefika hapa 1100")
            If dtBrOutClearing.Rows.Count > 0 Then
                For Each dr In dtBrOutClearing.Rows
                    'MessageBox.Show("Imefika hapa 1111 poa")
                    GetConnectionSQL()
                    'MessageBox.Show("Imefika hapa 1112")
                    If Modscan.SysType = Modscan.ENUM_SysType.BRNET Then
                        Try
                            ExecuteData(GetModify("p_GetValueDate", "ourbranchid", OurBranchID, "CurrencyID", CodeLineDetails.CurrencyCode, "AccountID", CodeLineDetails.AccountID, "AccountTypeID", AccountType, "WorkingDate", cWorkingDate, "VoucherCode", CodeLineDetails.VoucherCode, "BankID", CodeLineDetails.BankID, "BranchID", CodeLineDetails.BranchID, "Amount", 0, "ReturnCode", CodeLineDetails.ReturnCode), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.SelectStatement)
                        Catch ex As Exception
                            MessageBox.Show("Imechapia hapa hapa 1113")
                        End Try
                    Else
                        Try
                            DestAccount = dr("DestAcc")
                        Catch ex As Exception
                            'MessageBox.Show("Imefika hapa 1101")
                            DestAccount = ""
                        End Try
                        Try
                            Select Case CountryCode.ToUpper.Trim
                                Case "UG"
                                    Try
                                        If strModule = "IN" Then
                                            ExecuteData(GetModify("sp_GetValueDate", "ourbranchid", OurBranchID, "AccountID", dr("DestAcc").ToString(), "Date", Modscan.cFromDate, "VoucherCode", dr("VType").ToString(), "BankID", dr("PBank").ToString.Substring(0, 2), "BranchID", dr("PBranch").ToString, "Amount", IIf(IsDBNull(dr("Amount").ToString), 0, Convert.ToDouble(dr("Amount")))), publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.SelectStatement)
                                        Else
                                            ExecuteData(GetModify("sp_GetValueDate", "ourbranchid", OurBranchID, "AccountID", dr("DestAcc").ToString(), "Date", Modscan.cFromDate, "VoucherCode", dr("VType").ToString(), "BankID", dr("PBank").ToString.Substring(0, 2), "BranchID", dr("PBranch").ToString.Substring(0, 3), "Amount", IIf(IsDBNull(dr("Amount").ToString), 0, Convert.ToDouble(dr("Amount")))), publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.SelectStatement)
                                        End If

                                    Catch ex As Exception
                                        MessageBox.Show("Imefika hapa 1102")
                                        If strModule = "IN" Then
                                            ExecuteData(GetModify("sp_GetValueDate", "ourbranchid", OurBranchID, "AccountID", dr(22).ToString(), "Date", Modscan.cFromDate, "VoucherCode", dr(15).ToString(), "BankID", dr(3).ToString.Substring(0, 2), "BranchID", dr(4).ToString, "Amount", IIf(IsDBNull(dr(1).ToString), 0, Convert.ToDouble(dr(1)))), publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.SelectStatement)
                                        Else
                                            ExecuteData(GetModify("sp_GetValueDate", "ourbranchid", OurBranchID, "AccountID", dr(22).ToString(), "Date", Modscan.cFromDate, "VoucherCode", dr(15).ToString(), "BankID", dr(3).ToString.Substring(0, 2), "BranchID", dr(4).ToString.Substring(0, 3), "Amount", IIf(IsDBNull(dr(1).ToString), 0, Convert.ToDouble(dr(1)))), publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.SelectStatement)
                                        End If

                                    End Try
                                Case "SL"

                                Case "TZ"
                                    Try
                                        If strModule = "IN" Then
                                            ExecuteData(GetModify("sp_GetValueDate", "ourbranchid", OurBranchID, "AccountID", dr("DestAcc").ToString(), "Date", Modscan.cFromDate, "VoucherCode", dr("VType").ToString(), "BankID", dr("PBank").ToString.Substring(0, 2), "BranchID", dr("PBranch").ToString, "Amount", IIf(IsDBNull(dr("Amount").ToString), 0, Convert.ToDouble(dr("Amount")))), publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.SelectStatement)
                                        Else
                                            ExecuteData(GetModify("sp_GetValueDate", "ourbranchid", OurBranchID, "AccountID", dr("DestAcc").ToString(), "Date", Modscan.cFromDate, "VoucherCode", dr("VType").ToString(), "BankID", dr("PBank").ToString.Substring(0, 2), "BranchID", dr("PBranch").ToString.Substring(0, 3), "Amount", IIf(IsDBNull(dr("Amount").ToString), 0, Convert.ToDouble(dr("Amount")))), publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.SelectStatement)
                                        End If

                                    Catch ex As Exception
                                        MessageBox.Show("Imefika hapa 1102")
                                        If strModule = "IN" Then
                                            ExecuteData(GetModify("sp_GetValueDate", "ourbranchid", OurBranchID, "AccountID", dr(22).ToString(), "Date", Modscan.cFromDate, "VoucherCode", dr(15).ToString(), "BankID", dr(3).ToString.Substring(0, 2), "BranchID", dr(4).ToString, "Amount", IIf(IsDBNull(dr(1).ToString), 0, Convert.ToDouble(dr(1)))), publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.SelectStatement)
                                        Else
                                            ExecuteData(GetModify("sp_GetValueDate", "ourbranchid", OurBranchID, "AccountID", dr(22).ToString(), "Date", Modscan.cFromDate, "VoucherCode", dr(15).ToString(), "BankID", dr(3).ToString.Substring(0, 2), "BranchID", dr(4).ToString.Substring(0, 3), "Amount", IIf(IsDBNull(dr(1).ToString), 0, Convert.ToDouble(dr(1)))), publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.SelectStatement)
                                        End If

                                    End Try
                                Case "RD"

                                Case "KE"
                                    'ExecuteData(GetModify("sp_GetValueDate", "ourbranchid", OurBranchID, "AccountID", dr("DestAcc").ToString(), "Date", Modscan.cFromDate, "VoucherCode", dr("VType").ToString(), "BankID", dr("PBank").ToString.Substring(0, 2), "BranchID", dr("PBranch").ToString.Substring(0, 3), "Amount", IIf(IsDBNull(dr("Amount").ToString), 0, Convert.ToDouble(dr("Amount")))), publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.SelectStatement)
                                    Try
                                        If strModule = "IN" Then
                                            ExecuteData(GetModify("sp_GetValueDate", "ourbranchid", OurBranchID, "AccountID", dr("DestAcc").ToString(), "Date", Modscan.cFromDate, "VoucherCode", dr("VType").ToString(), "BankID", dr("PBank").ToString.Substring(0, 2), "BranchID", dr("PBranch").ToString, "Amount", IIf(IsDBNull(dr("Amount").ToString), 0, Convert.ToDouble(dr("Amount")))), publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.SelectStatement)
                                        Else
                                            ExecuteData(GetModify("sp_GetValueDate", "ourbranchid", OurBranchID, "AccountID", dr("DestAcc").ToString(), "Date", Modscan.cFromDate, "VoucherCode", dr("VType").ToString(), "BankID", dr("PBank").ToString.Substring(0, 2), "BranchID", dr("PBranch").ToString.Substring(0, 3), "Amount", IIf(IsDBNull(dr("Amount").ToString), 0, Convert.ToDouble(dr("Amount")))), publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.SelectStatement)
                                        End If

                                    Catch ex As Exception
                                        If strModule = "IN" Then
                                            ExecuteData(GetModify("sp_GetValueDate", "ourbranchid", OurBranchID, "AccountID", dr(22).ToString(), "Date", Modscan.cFromDate, "VoucherCode", dr(15).ToString(), "BankID", dr(3).ToString.Substring(0, 2), "BranchID", dr(4).ToString, "Amount", IIf(IsDBNull(dr(1).ToString), 0, Convert.ToDouble(dr(1)))), publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.SelectStatement)
                                        Else
                                            ExecuteData(GetModify("sp_GetValueDate", "ourbranchid", OurBranchID, "AccountID", dr(22).ToString(), "Date", Modscan.cFromDate, "VoucherCode", dr(15).ToString(), "BankID", dr(3).ToString.Substring(0, 2), "BranchID", dr(4).ToString.Substring(0, 3), "Amount", IIf(IsDBNull(dr(1).ToString), 0, Convert.ToDouble(dr(1)))), publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.SelectStatement)
                                        End If

                                    End Try
                                Case "ET"

                                Case "SA"

                            End Select
                        Catch ex As Exception
                            Dim AppendErrorMessage As String = "Error Message ClearingFilesDataManuplation: Clearing Files"
                            AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "Date" + ":" + DateTime.Now
                            AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "--------------------------"
                            AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "Mod2669" + ex.Message.ToString()
                            AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "--------------------------"
                            AppendErrorMessage = AppendErrorMessage + Environment.NewLine
                            If Not Directory.Exists("C:\ClearingFilesErrorLog") Then
                                Directory.CreateDirectory("C:\ClearingFilesErrorLog")
                            End If
                            System.IO.File.AppendAllText("C:\ClearingFilesErrorLog\" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + ".txt", AppendErrorMessage)

                            'MessageBox.Show("1103" & ex.Message)
                        End Try
                    End If

                    If (Modscan.publicDTbl.Rows.Count > 0) Then
                        'MessageBox.Show("Imefika hapa 1106")
                        If Modscan.SysType = Modscan.ENUM_SysType.BRNET Then
                            Dim diff2 As String = (Convert.ToDateTime(publicDTbl.Rows(0)("ValueDate")) - Convert.ToDateTime(cWorkingDate)).TotalDays.ToString()
                            If CodeLineDetails.ClearingDays = 0 Then
                                ClearingD = diff2
                            Else
                                ClearingD = CodeLineDetails.ClearingDays
                            End If
                        Else
                            ClearingD = publicDTbl.Rows(0)("ClearingDays").ToString()
                        End If
                        ValueD = IIf(IsDBNull(publicDTbl.Rows(0)("ValueDate")).ToString, Convert.ToDateTime(WORKING_DATE), Convert.ToDateTime(publicDTbl.Rows(0)("ValueDate").ToString()))
                    Else
                        'MessageBox.Show("Imefika hapa 1105")
                        ClearingD = "0"
                        ValueD = (Convert.ToDateTime(WORKING_DATE).AddDays(0))
                    End If

                    'Modscan.CountryCode = ConfigurationManager.AppSettings("CountryCode")
                    'MessageBox.Show("Imefika hapa 1104 " & CountryCode.ToUpper.Trim.ToString & " - " & strModule)
                    Try
                        'MessageBox.Show("About to save sasa 2")
                        'MessageBox.Show(CountryCode.ToUpper.Trim.ToString())
                        Select Case CountryCode.ToUpper.Trim.ToString
                            Case "TZ", "UG", "ET"
                                'Case 
                                'MessageBox.Show(strModule, "Hapa kazi tu")
                                If strModule = "OUT" Then
                                    Modscan.publicDTbl.Clear()
                                    TFImage = Nothing
                                    If (dr(23).ToString = "") Then
                                        TFImage = Nothing
                                    Else
                                        TFImage = dr(23)
                                    End If
                                    JFImage = Nothing
                                    If (dr(11).ToString = "") Then
                                        JFImage = Nothing
                                    Else
                                        JFImage = dr(11)
                                    End If
                                    JRImage = Nothing
                                    If (dr(12).ToString = "") Then
                                        JRImage = Nothing
                                    Else
                                        JRImage = dr(12)
                                    End If
                                    UVImage = Nothing
                                    If (dr(13).ToString = "") Then
                                        UVImage = Nothing
                                    Else
                                        UVImage = dr(13)
                                    End If
                                    TFImageSignature = Nothing
                                    If (dr(38).ToString = "") Then
                                        TFImageSignature = Nothing
                                    Else
                                        TFImageSignature = dr(38)
                                    End If
                                    JFImageSignature = Nothing
                                    If (dr(39).ToString = "") Then
                                        JFImageSignature = Nothing
                                    Else
                                        JFImageSignature = dr(39)
                                    End If
                                    JRImageSignature = Nothing
                                    If (dr(40).ToString = "") Then
                                        JRImageSignature = Nothing
                                    Else
                                        JRImageSignature = dr(40)
                                    End If
                                    If Modscan.SysType = Modscan.ENUM_SysType.BRNET Then
                                        SqlConn = GetConnectionSQL()
                                        Command = New SqlCommand("[p_AddChequeTrunc]", SqlConn)
                                    Else
                                        Command = New SqlCommand("[sp_AddChequeTrunc]", SqlConn)
                                    End If
                                    Command.CommandType = CommandType.StoredProcedure
                                    If Modscan.SysType = Modscan.ENUM_SysType.BRNET Then
                                        Command.Parameters.Add("@ColumnID", SqlDbType.SmallInt).Value = 0
                                        Command.Parameters.Add("@IsMDV", SqlDbType.Bit).Value = False
                                    End If
                                    Command.Parameters.Add("@OurBranchID", SqlDbType.NVarChar).Value = OurBranchID
                                    Command.Parameters.Add("@ChequeID", SqlDbType.NVarChar).Value = dr(0)
                                    If CountryCode.ToUpper() = "KE" Then
                                        Select Case dr(15).ToString
                                            Case "60", "61", "62"
                                                Command.Parameters.Add("@Amount", SqlDbType.Money).Value = IIf(IsDBNull(dr(1).ToString), 0, dr(1))
                                            Case Else
                                                Command.Parameters.Add("@Amount", SqlDbType.Money).Value = RoundTo5Cents(IIf(IsDBNull(dr(1).ToString), 0, dr(1)))
                                        End Select
                                    Else
                                        Command.Parameters.Add("@Amount", SqlDbType.Money).Value = IIf(IsDBNull(dr(1).ToString), 0, dr(1))
                                    End If
                                    Try
                                        'MessageBox.Show(" AccountID " + dr(22) + "3: " + dr(6).ToString + " cWorkingDate " + cWorkingDate + " ValueD " + ValueD + "7 " + dr(7).ToString() + " ChequeDate " + dr(3).ToString())
                                        Command.Parameters.Add("@Drawer", SqlDbType.NVarChar).Value = dr(2)
                                        If CountryCode.ToUpper() = "TZ" Then
                                            Command.Parameters.Add("@BankID", SqlDbType.NVarChar).Value = dr(4).ToString.Substring(0, 3)
                                            Command.Parameters.Add("@BranchID", SqlDbType.NVarChar).Value = dr(5).ToString.Substring(0, 3).Replace("-", "")
                                            Command.Parameters.Add("@TheirAcc", SqlDbType.NVarChar).Value = dr(6)

                                        ElseIf CountryCode.ToUpper() = "UG" Then
                                            Command.Parameters.Add("@BankID", SqlDbType.NVarChar).Value = dr(3).ToString.Substring(0, 3)
                                            Command.Parameters.Add("@BranchID", SqlDbType.NVarChar).Value = dr(4).ToString.Substring(0, 3).Replace("-", "")
                                            Command.Parameters.Add("@TheirAcc", SqlDbType.NVarChar).Value = dr(5)
                                        ElseIf CountryCode.ToUpper() = "ET" Then
                                            Command.Parameters.Add("@BankID", SqlDbType.NVarChar).Value = dr(4).ToString.Substring(0, 2)
                                            Command.Parameters.Add("@BranchID", SqlDbType.NVarChar).Value = dr(5).ToString.Substring(0, 4).Replace("-", "")
                                            Command.Parameters.Add("@TheirAcc", SqlDbType.NVarChar).Value = dr(6)
                                        Else
                                            Command.Parameters.Add("@BankID", SqlDbType.NVarChar).Value = dr(3).ToString.Substring(0, 3)
                                            Command.Parameters.Add("@BranchID", SqlDbType.NVarChar).Value = dr(4).ToString.Substring(0, 3).Replace("-", "")
                                            Command.Parameters.Add("@TheirAcc", SqlDbType.NVarChar).Value = dr(5)
                                        End If

                                        Command.Parameters.Add("@ChequeDate", SqlDbType.DateTime).Value = IIf(cWorkingDate = "", WORKING_DATE, Convert.ToDateTime(cWorkingDate))
                                        Command.Parameters.Add("@Date", SqlDbType.DateTime).Value = IIf(cWorkingDate = "", WORKING_DATE, Convert.ToDateTime(cWorkingDate))
                                        Command.Parameters.Add("@ClearingDays", SqlDbType.SmallInt).Value = IIf(IsDBNull(dr(7).ToString), 0, dr(7))
                                        Command.Parameters.Add("@ReturnCode", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr(8).ToString), "00", dr(8))
                                        Command.Parameters.Add("@OperatorID", SqlDbType.NVarChar).Value = OperatorID
                                        Command.Parameters.Add("@ChequeDigit", SqlDbType.SmallInt).Value = dr(10)
                                        Command.Parameters.Add("@HighValue", SqlDbType.Bit).Value = dr(9)
                                        Command.Parameters.Add("@FrontImage", SqlDbType.NVarChar).Value = IIf(JFImage Is Nothing, Convert.DBNull, JFImage)
                                        Command.Parameters.Add("@FrontTFImage", SqlDbType.NVarChar).Value = IIf(TFImage Is Nothing, Convert.DBNull, TFImage)
                                        Command.Parameters.Add("@BackImage", SqlDbType.NVarChar).Value = IIf(JRImage Is Nothing, Convert.DBNull, JRImage)
                                        Command.Parameters.Add("@UVImage", SqlDbType.NVarChar).Value = IIf(UVImage Is Nothing, Convert.DBNull, UVImage)
                                        Command.Parameters.Add("@ClearingCenterID", SqlDbType.NVarChar).Value = dr(14)
                                        Command.Parameters.Add("@VoucherCode", SqlDbType.NVarChar).Value = dr(15)
                                        Command.Parameters.Add("@Ourcommission", SqlDbType.Money).Value = IIf(IsDBNull(dr(30).ToString), 0, dr(30))
                                        Command.Parameters.Add("@TheirCommission", SqlDbType.Money).Value = IIf(IsDBNull(dr(31).ToString), 0, dr(31))
                                        Command.Parameters.Add("@ImageUniqueID", SqlDbType.NVarChar).Value = dr(18).ToString
                                        Command.Parameters.Add("@TFImageSize", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr(19).ToString), 0, dr(19))
                                        Command.Parameters.Add("@JFImageSize", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr(20).ToString), 0, dr(20))
                                        Command.Parameters.Add("@JRImageSize", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr(21).ToString), 0, dr(21))
                                        Command.Parameters.Add("@TransactionColumnID", SqlDbType.SmallInt).Value = x.Next(9999)
                                        Command.Parameters.Add("@AccountID", SqlDbType.NVarChar).Value = dr(22)
                                        Command.Parameters.Add("@TFImageSignature", SqlDbType.NVarChar).Value = TFImageSignature
                                        Command.Parameters.Add("@JFImageSignature", SqlDbType.NVarChar).Value = JFImageSignature
                                        Command.Parameters.Add("@JRImageSignature", SqlDbType.NVarChar).Value = JRImageSignature
                                        Command.Parameters.Add("@BankName", SqlDbType.NVarChar).Value = dr(42)
                                        Command.Parameters.Add("@ValueDate", SqlDbType.DateTime).Value = Date.Parse(Format(ValueD, "dd MMM yyyy"))
                                        Command.Parameters.Add("@BranchName", SqlDbType.NVarChar).Value = dr(41)
                                        If strModule <> "OUT" Then
                                            Command.Parameters.Add("@FileName", SqlDbType.NVarChar).Value = dr("FileName").ToString.Substring(dr("FileName").ToString().LastIndexOf("\") + 1)
                                        End If
                                        If Modscan.SysType <> Modscan.ENUM_SysType.BRNET Then
                                            Command.Parameters.Add("@JFdpi", SqlDbType.NVarChar).Value = dr(46)
                                            Command.Parameters.Add("@TFdpi", SqlDbType.NVarChar).Value = dr(45)
                                            Command.Parameters.Add("@JRdpi", SqlDbType.NVarChar).Value = dr(44)
                                        End If
                                    Catch ex As Exception
                                        Dim AppendErrorMessage As String = "Error Message ClearingFilesDataManuplation: Clearing Files"
                                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "Date" + ":" + DateTime.Now
                                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "--------------------------"
                                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "Mod2822" + ex.Message.ToString()
                                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "--------------------------"
                                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine
                                        If Not Directory.Exists("C:\ClearingFilesErrorLog") Then
                                            Directory.CreateDirectory("C:\ClearingFilesErrorLog")
                                        End If
                                        System.IO.File.AppendAllText("C:\ClearingFilesErrorLog\" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + ".txt", AppendErrorMessage)

                                        'MessageBox.Show("Mod2745: " + ex.Message)
                                    End Try
                                Else
                                    'MessageBox.Show("About to save sasa kabisa")
                                    Try


                                        SqlConn = GetConnectionSQL()
                                        If CountryCode.ToUpper() = "UG" Then
                                            Command = New SqlCommand("p_AddInwardsUG", SqlConn)
                                        ElseIf CountryCode.ToUpper() = "TZ" Then
                                            Command = New SqlCommand("p_AddInwardsTZ", SqlConn)
                                        ElseIf CountryCode.ToUpper() = "ET" Then
                                            Command = New SqlCommand("p_AddInwardsET", SqlConn)
                                        End If
                                        Command.CommandType = CommandType.StoredProcedure
                                        Command.Parameters.Add("@ColumnID", SqlDbType.SmallInt).Value = 0
                                        Command.Parameters.Add("@IsMDV", SqlDbType.Bit).Value = False

                                        Command.Parameters.Add("@TrxID", SqlDbType.NVarChar).Value = dr("TrxID").ToString()
                                        Command.Parameters.Add("@ExtraDetails", SqlDbType.NVarChar).Value = dr("ExtraDetails").ToString()
                                        Command.Parameters.Add("@OurBranchID", SqlDbType.NVarChar).Value = "01"
                                        Command.Parameters.Add("@TheirAccountID", SqlDbType.NVarChar).Value = dr("TheirACC")
                                        Command.Parameters.Add("@ChequeID", SqlDbType.NVarChar).Value = ""
                                        Command.Parameters.Add("@Amount", SqlDbType.Money).Value = IIf(IsDBNull(dr("AMOUNT").ToString), 0, dr("AMOUNT"))
                                        Command.Parameters.Add("@Data", SqlDbType.NVarChar).Value = dr("DATA")
                                        Command.Parameters.Add("@BankID", SqlDbType.NVarChar).Value = dr("PBANK")
                                        Command.Parameters.Add("@BranchID", SqlDbType.NVarChar).Value = ""
                                        Command.Parameters.Add("@Date", SqlDbType.DateTime).Value = cWorkingDate
                                        Command.Parameters.Add("@CurrencyID", SqlDbType.NVarChar).Value = dr("CURRENCYCODE")
                                        Command.Parameters.Add("@ChequeDigit", SqlDbType.SmallInt).Value = dr("CHQDGT")
                                        Command.Parameters.Add("@Drawer", SqlDbType.NVarChar).Value = dr("COLLACCName")
                                        Command.Parameters.Add("@VoucherCode", SqlDbType.NVarChar).Value = dr("VTYPE")
                                        Command.Parameters.Add("@ImageUniqueID", SqlDbType.NVarChar).Value = ""
                                        Command.Parameters.Add("@TFImageSize", SqlDbType.NVarChar).Value = "0"
                                        Command.Parameters.Add("@JFImageSize", SqlDbType.NVarChar).Value = "0"
                                        Command.Parameters.Add("@JRImageSize", SqlDbType.NVarChar).Value = "0"
                                        Command.Parameters.Add("@AccountID", SqlDbType.NVarChar).Value = dr("DESTACC")
                                        Command.Parameters.Add("@Filename", SqlDbType.NVarChar).Value = dr("FILENAME")
                                        Command.Parameters.Add("@Validity", SqlDbType.NVarChar).Value = dr("ValidInvalid")
                                        Command.Parameters.Add("@OurBankID", SqlDbType.NVarChar).Value = Modscan.OurBankID
                                        Command.Parameters.Add("@JFdpi", SqlDbType.NVarChar).Value = ""
                                        Command.Parameters.Add("@TFdpi", SqlDbType.NVarChar).Value = ""
                                        Command.Parameters.Add("@JRdpi", SqlDbType.NVarChar).Value = ""
                                        Command.Parameters.Add("@TFImageSignature", SqlDbType.NVarChar).Value = ""
                                        Command.Parameters.Add("@JFImageSignature", SqlDbType.NVarChar).Value = ""
                                        Command.Parameters.Add("@JRImageSignature", SqlDbType.NVarChar).Value = ""
                                        Command.Parameters.Add("@DAdrLine", SqlDbType.NVarChar).Value = dr("DAdrLine").ToString()
                                        Command.Parameters.Add("@DTwnNm", SqlDbType.NVarChar).Value = dr("DTwnNm").ToString()
                                        Command.Parameters.Add("@DCtry", SqlDbType.NVarChar).Value = dr("DCtry").ToString()
                                        Command.Parameters.Add("@DNm", SqlDbType.NVarChar).Value = dr("DNm").ToString()
                                        Command.Parameters.Add("@DPhneNb", SqlDbType.NVarChar).Value = dr("DPhneNb").ToString()
                                        Command.Parameters.Add("@DMobNb", SqlDbType.NVarChar).Value = dr("DMobNb").ToString()
                                        Command.Parameters.Add("@DEmailAdr", SqlDbType.NVarChar).Value = dr("DEmailAdr").ToString()
                                        Command.Parameters.Add("@DOthr", SqlDbType.NVarChar).Value = dr("DOthr").ToString()
                                        Command.Parameters.Add("@DbtrAcct", SqlDbType.NVarChar).Value = dr("DbtrAcct").ToString()
                                        Command.Parameters.Add("@CAdrLine", SqlDbType.NVarChar).Value = dr("CAdrLine").ToString()
                                        Command.Parameters.Add("@CTwnNm", SqlDbType.NVarChar).Value = dr("CTwnNm").ToString()
                                        Command.Parameters.Add("@CCtry", SqlDbType.NVarChar).Value = dr("CCtry").ToString()
                                        Command.Parameters.Add("@CNm", SqlDbType.NVarChar).Value = dr("CNm").ToString()
                                        Command.Parameters.Add("@CPhneNb", SqlDbType.NVarChar).Value = dr("CPhneNb").ToString()
                                        Command.Parameters.Add("@CMobNb", SqlDbType.NVarChar).Value = dr("CMobNb").ToString()
                                        Command.Parameters.Add("@CEmailAdr", SqlDbType.NVarChar).Value = dr("CEmailAdr").ToString()
                                        Command.Parameters.Add("@COthr", SqlDbType.NVarChar).Value = dr("COthr").ToString()
                                        Command.Parameters.Add("@PymType", SqlDbType.NVarChar).Value = dr("PymType").ToString()
                                        Command.Parameters.Add("@CdtrAcct", SqlDbType.NVarChar).Value = dr("CdtrAcct").ToString()
                                        Command.Parameters.Add("@OrgnlInstrID", SqlDbType.NVarChar).Value = dr("OrgnlInstrID").ToString()
                                        Command.Parameters.Add("@UstrdColD", SqlDbType.NVarChar).Value = dr("UstrdColD").ToString()
                                        Command.Parameters.Add("@OrgnlEndToEnd", SqlDbType.NVarChar).Value = dr("OrgnlEndToEnd").ToString()
                                        Command.Parameters.Add("@CCNm", SqlDbType.NVarChar).Value = dr("CCNm").ToString()
                                        Command.Parameters.Add("@DCNm", SqlDbType.NVarChar).Value = dr("DCNm").ToString()
                                        Command.Parameters.Add("@ReqdColltnDt", SqlDbType.NVarChar).Value = dr("ReqdColltnDt").ToString()
                                        Command.Parameters.Add("@SourceBIC", SqlDbType.NVarChar).Value = dr("SourceBIC").ToString()
                                        If CountryCode.ToUpper() = "TZ" Then
                                            Command.Parameters.Add("@RemittanceInfo", SqlDbType.NVarChar).Value = dr("RemittanceInfo").ToString()
                                            Command.Parameters.Add("@IntrBkSttlmDt", SqlDbType.NVarChar).Value = dr("IntrBkSttlmDt").ToString()
                                            Command.Parameters.Add("@SvcLvl", SqlDbType.NVarChar).Value = dr("SvcLvl").ToString()
                                            Command.Parameters.Add("@LclInstrm", SqlDbType.NVarChar).Value = dr("LclInstrm").ToString()
                                            Command.Parameters.Add("@CtgyPurp", SqlDbType.NVarChar).Value = dr("CtgyPurp").ToString()
                                            Command.Parameters.Add("@OrgnlTxId", SqlDbType.NVarChar).Value = dr("OrgnlTxId").ToString()
                                            Command.Parameters.Add("OrgnlIntrBkSttlmDt", SqlDbType.NVarChar).Value = dr("OrgnlIntrBkSttlmDt").ToString()
                                        Else
                                            'Command.Parameters.Add("IntrBkSttlmDt", SqlDbType.NVarChar).Value = ""RemittanceInfo
                                            'Command.Parameters.Add("SvcLvl", SqlDbType.NVarChar).Value = ""
                                            'Command.Parameters.Add("OrgnlTxId", SqlDbType.NVarChar).Value = ""
                                        End If

                                        If dr("VTYPE") = "40" Then
                                            Command.Parameters.Add("@DtOfSgntr", SqlDbType.NVarChar).Value = dr("DtOfSgntr").ToString()
                                            Command.Parameters.Add("@MndtId", SqlDbType.NVarChar).Value = dr("MndtId").ToString()
                                            Command.Parameters.Add("@FnlColltnDt", SqlDbType.NVarChar).Value = dr("FnlColltnDt").ToString()
                                            Command.Parameters.Add("@Frqcy", SqlDbType.NVarChar).Value = dr("Frqcy").ToString()
                                            Command.Parameters.Add("@CdtrSchmeId", SqlDbType.NVarChar).Value = dr("CdtrSchmeId").ToString()
                                            dr("RCODE") = "00"
                                            Try
                                                Command.Parameters.Add("@RemittanceInfo", SqlDbType.NVarChar).Value = dr("RemittanceInfo").ToString()
                                            Catch ex As Exception

                                            End Try
                                            Try
                                                Command.Parameters.Add("@IntrBkSttlmDt", SqlDbType.NVarChar).Value = dr("IntrBkSttlmDt").ToString()
                                            Catch ex As Exception

                                            End Try

                                            Command.Parameters.Add("@TrxType", SqlDbType.NVarChar).Value = "ID"
                                        Else
                                            Command.Parameters.Add("@TrxType", SqlDbType.NVarChar).Value = "IC"
                                        End If
                                        Command.Parameters.Add("@ReturnCode", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("RCODE").ToString), "00", dr("RCODE"))

                                        If IIf(IsDBNull(dr("TrxTypeID").ToString), "ID", dr("TrxTypeID")) = "ID" Then
                                            Command.Parameters.Add("@FrontImage", SqlDbType.NVarChar).Value = Nothing ' FJFImage
                                            Command.Parameters.Add("@FrontTFImage", SqlDbType.NVarChar).Value = Nothing 'FTFImage
                                            Command.Parameters.Add("@BackImage", SqlDbType.NVarChar).Value = Nothing 'RJImage
                                            Command.Parameters.Add("@UVImage", SqlDbType.NVarChar).Value = Nothing 'FUVImage
                                        Else
                                            Command.Parameters.Add("@FrontImage", SqlDbType.NVarChar).Value = Nothing
                                            Command.Parameters.Add("@FrontTFImage", SqlDbType.NVarChar).Value = Nothing
                                            Command.Parameters.Add("@BackImage", SqlDbType.NVarChar).Value = Nothing
                                            Command.Parameters.Add("@UVImage", SqlDbType.NVarChar).Value = Nothing
                                        End If
                                        Command.Parameters.Add("@MsgID", SqlDbType.NVarChar).Value = dr("DRN")
                                        'Command.ExecuteNonQuery()
                                    Catch ex As Exception
                                        Dim AppendErrorMessage As String = "Error Message ClearingFilesDataManuplation: Clearing Files"
                                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "Date" + ":" + DateTime.Now
                                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "--------------------------"
                                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "Mod2932" + ex.Message.ToString()
                                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "--------------------------"
                                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine
                                        If Not Directory.Exists("C:\ClearingFilesErrorLog") Then
                                            Directory.CreateDirectory("C:\ClearingFilesErrorLog")
                                        End If
                                        System.IO.File.AppendAllText("C:\ClearingFilesErrorLog\" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + ".txt", AppendErrorMessage)

                                        ' MessageBox.Show("Mod2855: " + ex.Message)
                                    End Try
                                End If

                                SqlConn = GetConnectionSQL()
                                Command.ExecuteNonQuery()

                            Case "RD"

                            Case "KE"
                                Try
                                    'MessageBox.Show("Imeingia  UG Case 1242" & " - " & strModule)
                                    Modscan.publicDTbl.Clear()
                                    TFImage = Nothing
                                    If (dr(23).ToString = "") Then
                                        TFImage = Nothing
                                    Else
                                        TFImage = dr(23)
                                    End If
                                    JFImage = Nothing
                                    If (dr(11).ToString = "") Then
                                        JFImage = Nothing
                                    Else
                                        JFImage = dr(11)
                                    End If
                                    JRImage = Nothing
                                    If (dr(12).ToString = "") Then
                                        JRImage = Nothing
                                    Else
                                        JRImage = dr(12)
                                    End If
                                    UVImage = Nothing
                                    If (dr(13).ToString = "") Then
                                        UVImage = Nothing
                                    Else
                                        UVImage = dr(13)
                                    End If
                                    TFImageSignature = Nothing
                                    If (dr(38).ToString = "") Then
                                        TFImageSignature = Nothing
                                    Else
                                        TFImageSignature = dr(38)
                                    End If
                                    JFImageSignature = Nothing
                                    If (dr(39).ToString = "") Then
                                        JFImageSignature = Nothing
                                    Else
                                        JFImageSignature = dr(39)
                                    End If
                                    JRImageSignature = Nothing
                                    If (dr(40).ToString = "") Then
                                        JRImageSignature = Nothing
                                    Else
                                        JRImageSignature = dr(40)
                                    End If
                                    If Modscan.SysType = Modscan.ENUM_SysType.BRNET Then
                                        SqlConn = GetConnectionSQL()
                                        Command = New SqlCommand("[p_AddChequeTrunc]", SqlConn)
                                    Else
                                        SqlConn = GetConnectionSQL()
                                        Command = New SqlCommand("[sp_AddChequeTrunc]", SqlConn)
                                    End If
                                    Command.CommandType = CommandType.StoredProcedure
                                    If Modscan.SysType = Modscan.ENUM_SysType.BRNET Then
                                        Command.Parameters.Add("@ColumnID", SqlDbType.SmallInt).Value = 0
                                        Command.Parameters.Add("@IsMDV", SqlDbType.Bit).Value = False
                                    End If
                                    Command.Parameters.Add("@OurBranchID", SqlDbType.NVarChar).Value = OurBranchID
                                    Command.Parameters.Add("@ChequeID", SqlDbType.NVarChar).Value = dr(0)
                                    If CountryCode.ToUpper() = "KE" Then
                                        Select Case dr(15).ToString
                                            Case "60", "61", "62"
                                                Command.Parameters.Add("@Amount", SqlDbType.Money).Value = IIf(IsDBNull(dr(1).ToString), 0, dr(1))
                                            Case Else
                                                Command.Parameters.Add("@Amount", SqlDbType.Money).Value = RoundTo5Cents(IIf(IsDBNull(dr(1).ToString), 0, dr(1)))
                                        End Select
                                    Else
                                        Command.Parameters.Add("@Amount", SqlDbType.Money).Value = IIf(IsDBNull(dr(1).ToString), 0, dr(1))
                                    End If
                                    If CountryCode.ToUpper() = "KE" Then
                                        Command.Parameters.Add("@Drawer", SqlDbType.NVarChar).Value = dr(2)
                                        Command.Parameters.Add("@BankID", SqlDbType.NVarChar).Value = dr(4).ToString.Substring(0, 2)
                                        Command.Parameters.Add("@BranchID", SqlDbType.NVarChar).Value = dr(5).ToString.Substring(0, 3).Replace("-", "") 'dr(4).ToString.Substring(0, 3).Replace("-", "")
                                        Command.Parameters.Add("@TheirAcc", SqlDbType.NVarChar).Value = dr(6)
                                        Command.Parameters.Add("@ChequeDate", SqlDbType.DateTime).Value = IIf(IsDBNull(dr(3)).ToString, WORKING_DATE, Convert.ToDateTime(dr(3).ToString))
                                    Else
                                        Command.Parameters.Add("@Drawer", SqlDbType.NVarChar).Value = dr(2)
                                        Command.Parameters.Add("@BankID", SqlDbType.NVarChar).Value = dr(4).ToString.Substring(0, 2)
                                        Command.Parameters.Add("@BranchID", SqlDbType.NVarChar).Value = dr(5).ToString.Substring(0, 3).Replace("-", "")
                                        Command.Parameters.Add("@TheirAcc", SqlDbType.NVarChar).Value = dr(6)
                                        Command.Parameters.Add("@ChequeDate", SqlDbType.DateTime).Value = IIf(IsDBNull(dr(3)).ToString, WORKING_DATE, Convert.ToDateTime(dr(3).ToString))

                                    End If
                                    Command.Parameters.Add("@Date", SqlDbType.DateTime).Value = IIf(cWorkingDate = "", WORKING_DATE, Convert.ToDateTime(cWorkingDate))
                                    Command.Parameters.Add("@ClearingDays", SqlDbType.SmallInt).Value = IIf(IsDBNull(dr(7).ToString), 0, dr(7))
                                    Command.Parameters.Add("@ReturnCode", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr(8).ToString), "00", dr(8))
                                    Command.Parameters.Add("@OperatorID", SqlDbType.NVarChar).Value = OperatorID
                                    Command.Parameters.Add("@ChequeDigit", SqlDbType.SmallInt).Value = dr(10)
                                    Command.Parameters.Add("@HighValue", SqlDbType.Bit).Value = dr(9)
                                    Command.Parameters.Add("@FrontImage", SqlDbType.NVarChar).Value = IIf(JFImage Is Nothing, Convert.DBNull, JFImage)
                                    Command.Parameters.Add("@FrontTFImage", SqlDbType.NVarChar).Value = IIf(TFImage Is Nothing, Convert.DBNull, TFImage)
                                    Command.Parameters.Add("@BackImage", SqlDbType.NVarChar).Value = IIf(JRImage Is Nothing, Convert.DBNull, JRImage)
                                    Command.Parameters.Add("@UVImage", SqlDbType.NVarChar).Value = IIf(UVImage Is Nothing, Convert.DBNull, UVImage)
                                    Command.Parameters.Add("@ClearingCenterID", SqlDbType.NVarChar).Value = dr(14)
                                    Command.Parameters.Add("@VoucherCode", SqlDbType.NVarChar).Value = dr(15)
                                    Command.Parameters.Add("@Ourcommission", SqlDbType.Money).Value = IIf(IsDBNull(dr(30).ToString), 0, dr(30))
                                    Command.Parameters.Add("@TheirCommission", SqlDbType.Money).Value = IIf(IsDBNull(dr(31).ToString), 0, dr(31))
                                    Command.Parameters.Add("@ImageUniqueID", SqlDbType.NVarChar).Value = dr(18).ToString
                                    Command.Parameters.Add("@TFImageSize", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr(19).ToString), 0, dr(19))
                                    Command.Parameters.Add("@JFImageSize", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr(20).ToString), 0, dr(20))
                                    Command.Parameters.Add("@JRImageSize", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr(21).ToString), 0, dr(21))
                                    Command.Parameters.Add("@TransactionColumnID", SqlDbType.SmallInt).Value = x.Next(9999)
                                    Command.Parameters.Add("@AccountID", SqlDbType.NVarChar).Value = dr(22)
                                    Command.Parameters.Add("@TFImageSignature", SqlDbType.NVarChar).Value = TFImageSignature
                                    Command.Parameters.Add("@JFImageSignature", SqlDbType.NVarChar).Value = JFImageSignature
                                    Command.Parameters.Add("@JRImageSignature", SqlDbType.NVarChar).Value = JRImageSignature
                                    Command.Parameters.Add("@BankName", SqlDbType.NVarChar).Value = dr(42)
                                    Command.Parameters.Add("@ValueDate", SqlDbType.DateTime).Value = Date.Parse(Format(ValueD, "dd MMM yyyy"))
                                    Command.Parameters.Add("@BranchName", SqlDbType.NVarChar).Value = dr(41)
                                    If strModule <> "OUT" Then
                                        Command.Parameters.Add("@FileName", SqlDbType.NVarChar).Value = dr("FileName").ToString.Substring(dr("FileName").ToString().LastIndexOf("\") + 1)
                                    End If
                                    If Modscan.SysType <> Modscan.ENUM_SysType.BRNET Then
                                        Command.Parameters.Add("@JFdpi", SqlDbType.NVarChar).Value = dr(46)
                                        Command.Parameters.Add("@TFdpi", SqlDbType.NVarChar).Value = dr(45)
                                        Command.Parameters.Add("@JRdpi", SqlDbType.NVarChar).Value = dr(44)
                                    End If
                                    SqlConn = GetConnectionSQL()
                                    Command.ExecuteNonQuery()
                                Catch ex As Exception
                                    Dim AppendErrorMessage As String = "Error Message ClearingFilesDataManuplation: Clearing Files"
                                    AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "Date" + ":" + DateTime.Now
                                    AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "--------------------------"
                                    AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "Mod3061" + ex.Message.ToString()
                                    AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "--------------------------"
                                    AppendErrorMessage = AppendErrorMessage + Environment.NewLine
                                    If Not Directory.Exists("C:\ClearingFilesErrorLog") Then
                                        Directory.CreateDirectory("C:\ClearingFilesErrorLog")
                                    End If
                                    System.IO.File.AppendAllText("C:\ClearingFilesErrorLog\" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + ".txt", AppendErrorMessage)



                                    'MessageBox.Show("2975 " & ex.Message & " - " & strModule)
                                End Try
                            Case "ET"

                            Case "SA"

                        End Select
                    Catch ex As Exception
                        Dim AppendErrorMessage As String = "Error Message ClearingFilesDataManuplation: Clearing Files"
                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "Date" + ":" + DateTime.Now
                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "--------------------------"
                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "Mod3069" + ex.Message.ToString()
                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "--------------------------"
                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine
                        If Not Directory.Exists("C:\ClearingFilesErrorLog") Then
                            Directory.CreateDirectory("C:\ClearingFilesErrorLog")
                        End If
                        System.IO.File.AppendAllText("C:\ClearingFilesErrorLog\" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + ".txt", AppendErrorMessage)

                        ' MessageBox.Show("Mod2985: " + ex.Message)
                        'Return False
                    End Try

                Next

            End If
            'Return True
        Catch ex As Exception
            Dim AppendErrorMessage As String = "Error Message ClearingFilesDataManuplation: Clearing Files"
            AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "Date" + ":" + DateTime.Now
            AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "--------------------------"
            AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "Mod3089" + ex.Message.ToString()
            AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "--------------------------"
            AppendErrorMessage = AppendErrorMessage + Environment.NewLine
            If Not Directory.Exists("C:\ClearingFilesErrorLog") Then
                Directory.CreateDirectory("C:\ClearingFilesErrorLog")
            End If
            System.IO.File.AppendAllText("C:\ClearingFilesErrorLog\" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + ".txt", AppendErrorMessage)


            'MessageBox.Show("1245" + ex.Message)
            'Return False
        End Try
        'MessageBox.Show("Inatoka 1246" & " - " & strModule)
    End Function
    Public Shared Function SaveMandate(ByVal dtMandate As DataTable) As Boolean
        Dim Command As SqlCommand
        Dim dr As Data.DataRow
        Try
            If dtMandate.Rows.Count > 0 Then
                SqlConn = GetConnectionSQL()
                For Each dr In dtMandate.Rows
                    Try
                        Dim ExpiryDate As Date
                        Dim DueDate As Date
                        If dr("FileType").ToString() <> "CMF" Then
                            If Convert.ToDateTime(dr("ToDt")).Year > "2078" Then
                                ExpiryDate = New Date(2078, Convert.ToDateTime(dr("ToDt")).Month, Convert.ToDateTime(dr("ToDt")).Day)
                            Else
                                ExpiryDate = Convert.ToDateTime(dr("ToDt"))
                            End If

                            If Convert.ToDateTime(dr("FrDt")).Year > "2078" Then
                                DueDate = New Date(2078, Convert.ToDateTime(dr("FrDt")).Month, Convert.ToDateTime(dr("FrDt")).Day)
                            Else
                                DueDate = Convert.ToDateTime(dr("FrDt"))
                            End If
                        Else
                            ExpiryDate = DateTime.Now.Date
                            DueDate = DateTime.Now.Date
                        End If

                        'IIf(IsDBNull(dr("AMOUNT").ToString), 0, dr("AMOUNT"))
                        Command = New SqlCommand("P_AddIncomingMandate", SqlConn)
                        Command.CommandType = CommandType.StoredProcedure
                        Command.Parameters.Add("@FileType", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("FileType").ToString()), "", dr("FileType").ToString())
                        Command.Parameters.Add("@DDID", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("DDID").ToString()), "", dr("DDID").ToString())
                        Command.Parameters.Add("@RequestID", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("RequestID").ToString()), "", dr("RequestID").ToString())
                        Command.Parameters.Add("@FrqCy", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("FrqCy").ToString()), "", dr("FrqCy").ToString())
                        Command.Parameters.Add("@FrDt", SqlDbType.SmallDateTime).Value = IIf(IsDBNull(dr("FrDt")), Nothing, Convert.ToDateTime(dr("FrDt")))
                        Command.Parameters.Add("@ToDt", SqlDbType.DateTime2).Value = IIf(IsDBNull(dr("ToDt")), Nothing, dr("ToDt").ToString())
                        Command.Parameters.Add("@FrstColltnDt", SqlDbType.SmallDateTime).Value = IIf(IsDBNull(dr("FrstColltnDt")), Convert.ToDateTime(dr("FrDt")), Convert.ToDateTime(dr("FrstColltnDt")))
                        Command.Parameters.Add("@FinalCollDate", SqlDbType.DateTime2).Value = IIf(IsDBNull(dr("FinalCollDate")), dr("ToDt").ToString(), Convert.ToDateTime(dr("FinalCollDate")))
                        Try
                            If IsDBNull(dr("FrstColltnAmt")) Then
                                Command.Parameters.Add("@FrstColltnAmt", SqlDbType.Money).Value = 0
                            Else
                                Command.Parameters.Add("@FrstColltnAmt", SqlDbType.Money).Value = IIf(IsDBNull(dr("FrstColltnAmt")), 0, CDec(dr("FrstColltnAmt")))
                            End If
                        Catch

                        End Try
                        Command.Parameters.Add("@FrstColltnCurr", SqlDbType.NVarChar).Value = dr("FrstColltnCurr").ToString()
                        Try
                            If IsDBNull(dr("ColltnAm")) Then
                                Command.Parameters.Add("@ColltnAmt", SqlDbType.Money).Value = 0
                            Else
                                Command.Parameters.Add("@ColltnAmt", SqlDbType.Money).Value = IIf(IsDBNull(dr("ColltnAm")), 0, CDec(dr("ColltnAm")))
                            End If
                        Catch
                            Command.Parameters.Add("@ColltnAmt", SqlDbType.Money).Value = 0
                        End Try
                        Command.Parameters.Add("@ColltnCurr", SqlDbType.NVarChar).Value = dr("ColltnCurr").ToString()
                        Try
                            If IsDBNull(dr("MaxAmt")) Then
                                Command.Parameters.Add("@MaxAmt", SqlDbType.Money).Value = 0
                            Else
                                Command.Parameters.Add("@MaxAmt", SqlDbType.Money).Value = IIf(IsDBNull(dr("MaxAmt")), 0, CDec(dr("MaxAmt")))
                            End If
                        Catch
                            Command.Parameters.Add("@MaxAmt", SqlDbType.Money).Value = 0
                        End Try
                        Command.Parameters.Add("@MaxCurr", SqlDbType.NVarChar).Value = dr("MaxCurr").ToString()
                        Try
                            If IsDBNull(dr("Adjstmnt")) Then
                                Command.Parameters.Add("@Adjstmnt", SqlDbType.Money).Value = 0
                            Else
                                Command.Parameters.Add("@Adjstmnt", SqlDbType.Money).Value = IIf(IsDBNull(dr("Adjstmnt")), 0, CDec(dr("Adjstmnt")))
                            End If
                        Catch
                            Command.Parameters.Add("@Adjstmnt", SqlDbType.Money).Value = 0
                        End Try
                        Command.Parameters.Add("@DtAdjstmntRuleInd", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("DtAdjstmntRuleInd").ToString()), "", dr("DtAdjstmntRuleInd").ToString())
                        Command.Parameters.Add("@AdjstCurr", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("AdjstCurr").ToString()), "", dr("AdjstCurr").ToString())
                        Command.Parameters.Add("@AdjstRate", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("AdjstRate").ToString()), "", dr("AdjstRate").ToString())
                        Command.Parameters.Add("@CdtrName", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("CdtrName").ToString()), "", dr("CdtrName").ToString())
                        Command.Parameters.Add("@OrigCode", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("OrigCode").ToString()), "", dr("OrigCode").ToString())
                        Command.Parameters.Add("@CdtrAcctID", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("CdtrAcctID").ToString()), "", dr("CdtrAcctID").ToString())
                        Command.Parameters.Add("@CdtrAcctCurrID", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("CdtrAcctCurrID").ToString()), "", dr("CdtrAcctCurrID").ToString())
                        Command.Parameters.Add("@CdtrAgt", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("CdtrAgt").ToString()), "", dr("CdtrAgt").ToString())
                        Command.Parameters.Add("@CdtrBranchID", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("CdtrBranchID").ToString()), "", dr("CdtrBranchID").ToString())
                        Command.Parameters.Add("@CDtrBranchName", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("CDtrBranchName").ToString()), "", dr("CDtrBranchName").ToString())
                        Command.Parameters.Add("@DbtrName", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("DbtrName").ToString()), "", dr("DbtrName").ToString())
                        Command.Parameters.Add("@DbtrAcctID", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("DbtrAcctID").ToString()), "", dr("DbtrAcctID").ToString())
                        Command.Parameters.Add("@DbtrAcctCurrID", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("DbtrAcctCurrID").ToString()), "", dr("DbtrAcctCurrID").ToString())
                        Command.Parameters.Add("@DbtrAgt", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("DbtrAgt").ToString()), "", dr("DbtrAgt").ToString())
                        Command.Parameters.Add("@DbtrBranchID", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("DbtrBranchID").ToString()), "", dr("DbtrBranchID").ToString())
                        Command.Parameters.Add("@DbtrBranchName", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("DbtrBranchName").ToString()), "", dr("DbtrBranchName").ToString())
                        Command.Parameters.Add("@OrigRef", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("OrigRef").ToString()), "", dr("OrigRef").ToString())
                        Command.Parameters.Add("@Policy1", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("Policy1").ToString()), "", dr("Policy1").ToString())
                        Command.Parameters.Add("@Policy2", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("Policy2").ToString()), "", dr("Policy2").ToString())
                        Command.Parameters.Add("@DDImage", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("DDImage").ToString()), Nothing, dr("DDImage").ToString())
                        Command.Parameters.Add("@IQA", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("IQA").ToString()), "", dr("IQA").ToString())
                        Command.Parameters.Add("@InstdAgt", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("InstdAgt").ToString()), "", dr("InstdAgt").ToString())
                        Command.Parameters.Add("@InstgAgt", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("InstgAgt").ToString()), "", dr("InstgAgt").ToString())
                        Command.Parameters.Add("@OrgnlMndtId", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("OrgnlMndtId").ToString()), "", dr("OrgnlMndtId").ToString())
                        Command.Parameters.Add("@MndtId", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("MndtId").ToString()), "", dr("MndtId").ToString())
                        Command.Parameters.Add("@MndtReqID", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("MndtReqID").ToString()), "", dr("MndtReqID").ToString())
                        Command.Parameters.Add("@MndtRef", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("MndtRef").ToString()), "", dr("MndtRef").ToString())
                        Command.Parameters.Add("@MsgId", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("MsgId").ToString()), "", dr("MsgId").ToString())
                        Command.Parameters.Add("@MsgNmId", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("MsgNmId").ToString()), "", dr("MsgNmId").ToString())
                        Command.Parameters.Add("@CreDtTm", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("CreDtTm").ToString()), "", dr("CreDtTm").ToString())
                        Command.Parameters.Add("@AddtlInf", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("AddtlInf").ToString()), "", dr("AddtlInf").ToString())
                        Command.Parameters.Add("@Rsn", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("Rsn").ToString()), "", dr("Rsn").ToString())
                        Try
                            If IsDBNull(dr("SspnsnStartDate")) Then
                                Command.Parameters.Add("@SspnsnStartDate", SqlDbType.SmallDateTime).Value = Nothing
                            Else
                                Command.Parameters.Add("@SspnsnStartDate", SqlDbType.SmallDateTime).Value = IIf(IsDBNull(dr("SspnsnStartDate")), 0, CDate(dr("SspnsnStartDate")))
                            End If
                        Catch
                            Command.Parameters.Add("@SspnsnStartDate", SqlDbType.SmallDateTime).Value = Nothing
                        End Try
                        Try
                            If IsDBNull(dr("SspnsnEndDate")) Then
                                Command.Parameters.Add("@SspnsnEndDate", SqlDbType.SmallDateTime).Value = Nothing
                            Else
                                Command.Parameters.Add("@SspnsnEndDate", SqlDbType.SmallDateTime).Value = IIf(IsDBNull(dr("SspnsnEndDate")), 0, CDate(dr("SspnsnEndDate")))
                            End If
                        Catch
                            Command.Parameters.Add("@SspnsnEndDate", SqlDbType.SmallDateTime).Value = Nothing
                        End Try
                        Try
                            If IsDBNull(dr("TrckgInd")) Then
                                Command.Parameters.Add("@TrckgInd", SqlDbType.Bit).Value = 0
                            Else
                                Command.Parameters.Add("@TrckgInd", SqlDbType.Bit).Value = IIf(IsDBNull(dr("TrckgInd")), 0, CBool(dr("TrckgInd")))
                            End If
                        Catch
                            Command.Parameters.Add("@TrckgInd", SqlDbType.Bit).Value = False
                        End Try
                        Command.Parameters.Add("@SeqTp", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("SeqTp").ToString()), "", dr("SeqTp").ToString())
                        Command.Parameters.Add("@DRStrtNm", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("DRStrtNm").ToString()), "", dr("DRStrtNm").ToString())
                        Command.Parameters.Add("@DRBldgNb", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("DRBldgNb").ToString()), "", dr("DRBldgNb").ToString())
                        Command.Parameters.Add("@DRPstBx", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("DRPstBx").ToString()), "", dr("DRPstBx").ToString())
                        Command.Parameters.Add("@DRPstCd", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("DRPstCd").ToString()), "", dr("DRPstCd").ToString())
                        Command.Parameters.Add("@DRTwnNm", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("DRTwnNm").ToString()), "", dr("DRTwnNm").ToString())
                        Command.Parameters.Add("@DRCtry", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("DRCtry").ToString()), "", dr("DRCtry").ToString())
                        Command.Parameters.Add("@DRPhneNb", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("DRPhneNb").ToString()), "", dr("DRPhneNb").ToString())
                        Command.Parameters.Add("@DRMobNb", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("DRMobNb").ToString()), "", dr("DRMobNb").ToString())
                        Command.Parameters.Add("@DREmailAdr", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("DREmailAdr").ToString()), "", dr("DREmailAdr").ToString())
                        Command.Parameters.Add("@CRStrtNm", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("CRStrtNm").ToString()), "", dr("CRStrtNm").ToString())
                        Command.Parameters.Add("@CRBldgNb", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("CRBldgNb").ToString()), "", dr("CRBldgNb").ToString())
                        Command.Parameters.Add("@CRPstBx", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("CRPstBx").ToString()), "", dr("CRPstBx").ToString())
                        Command.Parameters.Add("@CRPstCd", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("CRPstCd").ToString()), "", dr("CRPstCd").ToString())
                        Command.Parameters.Add("@CRTwnNm", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("CRTwnNm").ToString()), "", dr("CRTwnNm").ToString())
                        Command.Parameters.Add("@CRCtry", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("CRCtry").ToString()), "", dr("CRCtry").ToString())
                        Command.Parameters.Add("@CRPhneNb", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("CRPhneNb").ToString()), "", dr("CRPhneNb").ToString())
                        Command.Parameters.Add("@CRMobNb", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("CRMobNb").ToString()), "", dr("CRMobNb").ToString())
                        Command.Parameters.Add("@CREmailAdr", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("CREmailAdr").ToString()), "", dr("CREmailAdr").ToString())
                        Command.Parameters.Add("@OrgnlMsgInfMsgId", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("OrgnlMsgInfMsgId").ToString()), "", dr("OrgnlMsgInfMsgId").ToString())
                        Command.Parameters.Add("@OrgnlMsgInfMsgNmId", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("OrgnlMsgInfMsgNmId").ToString()), "", dr("OrgnlMsgInfMsgNmId").ToString())
                        Command.Parameters.Add("@OrgnlMsgInCreDtTm", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("OrgnlMsgInCreDtTm").ToString()), "", dr("OrgnlMsgInCreDtTm").ToString())
                        Command.Parameters.Add("@AmdmntRsn", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("AmdmntRsn").ToString()), "", dr("AmdmntRsn").ToString())
                        Command.Parameters.Add("@AmdmntRsnAddtlInf", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("AmdmntRsnAddtlInf").ToString()), "", dr("AmdmntRsnAddtlInf").ToString())
                        Command.Parameters.Add("@OrgnlMndtMndtReqId", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("OrgnlMndtMndtReqId").ToString()), "", dr("OrgnlMndtMndtReqId").ToString())
                        Command.Parameters.Add("@FileName", SqlDbType.NVarChar).Value = IIf(IsDBNull(dr("FileName").ToString()), "", dr("FileName").ToString())
                        Command.Parameters.Add("@ExpiryDate", SqlDbType.SmallDateTime).Value = ExpiryDate.ToString()
                        Command.Parameters.Add("@DueDate", SqlDbType.SmallDateTime).Value = DueDate.ToString()
                        SqlConn = GetConnectionSQL()
                        Try
                            Command.ExecuteNonQuery()
                        Catch ex As Exception

                        End Try
                    Catch ex As DataException

                    End Try
                Next
            End If
        Catch ex As Exception

        End Try

    End Function
    Public Shared Function SaveUnPaidETDDs(ByVal dtBrOutClearing As DataTable) As Boolean
        Dim Command As SqlCommand
        Dim dr As Data.DataRow
        Try
            If dtBrOutClearing.Rows.Count > 0 Then
                For Each dr In dtBrOutClearing.Rows
                    GetConnectionSQL()
                    Try
                        SqlConn = GetConnectionSQL()
                        Command = New SqlCommand("P_AddUnpaidETDDs", SqlConn)
                        Command.CommandType = CommandType.StoredProcedure
                        Command.Parameters.Add("@RCode", SqlDbType.NVarChar).Value = dr("RCode").ToString()
                        Command.Parameters.Add("@Amount", SqlDbType.NVarChar).Value = dr("Amount").ToString()
                        Command.Parameters.Add("@CurrencyCode", SqlDbType.NVarChar).Value = dr("CurrencyCode").ToString()
                        Command.Parameters.Add("@OrgnlInstrID", SqlDbType.NVarChar).Value = dr("OrgnlInstrID").ToString()
                        Command.Parameters.Add("@OrgnlEndToEnd", SqlDbType.NVarChar).Value = dr("OrgnlEndToEnd").ToString()
                        Command.Parameters.Add("@OrgnlTxId", SqlDbType.NVarChar).Value = dr("OrgnlTxId").ToString()
                        Command.Parameters.Add("@OrgnlIntrBkSttlmDt", SqlDbType.NVarChar).Value = dr("OrgnlIntrBkSttlmDt").ToString()
                        Command.Parameters.Add("@FileName", SqlDbType.NVarChar).Value = dr("FileName").ToString()
                        Command.Parameters.Add("@MsgID", SqlDbType.NVarChar).Value = dr("MsgID").ToString()
                        SqlConn = GetConnectionSQL()
                        Command.ExecuteNonQuery()
                    Catch ex As Exception
                        Dim AppendErrorMessage As String = "Error Message ClearingFilesDataManuplation: Clearing Files"
                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "Date" + ":" + DateTime.Now
                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "--------------------------"
                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "Mod3291" + ex.Message.ToString()
                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "--------------------------"
                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine
                        If Not Directory.Exists("C:\ClearingFilesErrorLog") Then
                            Directory.CreateDirectory("C:\ClearingFilesErrorLog")
                        End If
                        System.IO.File.AppendAllText("C:\ClearingFilesErrorLog\" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + ".txt", AppendErrorMessage)

                        'MessageBox.Show("Mod2855: " + ex.Message)
                    End Try
                Next
            End If
        Catch ex As Exception
            Dim AppendErrorMessage As String = "Error Message ClearingFilesDataManuplation: RTGS Files"
            AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "Date" + ":" + DateTime.Now
            AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "--------------------------"
            AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "Mod3296" + ex.Message.ToString()
            AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "--------------------------"
            AppendErrorMessage = AppendErrorMessage + Environment.NewLine
            If Not Directory.Exists("C:\ClearingFilesErrorLog") Then
                Directory.CreateDirectory("C:\ClearingFilesErrorLog")
            End If
            System.IO.File.AppendAllText("C:\ClearingFilesErrorLog\" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + ".txt", AppendErrorMessage)


            'MessageBox.Show("1245" + ex.Message)
        End Try
    End Function
    Public Shared Function SaveRTGS(ByVal dtBrOutClearing As DataTable) As Boolean
        Dim Command As SqlCommand
        Dim dr As Data.DataRow
        Try
            If dtBrOutClearing.Rows.Count > 0 Then
                For Each dr In dtBrOutClearing.Rows
                    GetConnectionSQL()
                    Try
                        SqlConn = GetConnectionSQL()
                        Command = New SqlCommand("P_AddRTGSIncomingMessages", SqlConn)
                        Command.CommandType = CommandType.StoredProcedure
                        Command.Parameters.Add("@TxFileName", SqlDbType.NVarChar).Value = dr("TxFileName").ToString()
                        Command.Parameters.Add("@Trans_Ref", SqlDbType.NVarChar).Value = dr("Trans_Ref").ToString()
                        Command.Parameters.Add("@MessageType", SqlDbType.NVarChar).Value = dr("MessageType").ToString()
                        Command.Parameters.Add("@TrxCurrencyID", SqlDbType.NVarChar).Value = dr("TrxCurrencyID").ToString()
                        Command.Parameters.Add("@TrxAmount", SqlDbType.NVarChar).Value = dr("TrxAmount").ToString()
                        Command.Parameters.Add("@BeneficiaryAcc", SqlDbType.NVarChar).Value = dr("BeneficiaryAcc").ToString()
                        Command.Parameters.Add("@BeneficiaryName", SqlDbType.NVarChar).Value = dr("BeneficiaryName").ToString()
                        Command.Parameters.Add("@BeneficiaryBic", SqlDbType.NVarChar).Value = dr("BeneficiaryBic").ToString()
                        Command.Parameters.Add("@BeneficiaryBranch", SqlDbType.NVarChar).Value = dr("BeneficiaryBranch").ToString()
                        Command.Parameters.Add("@RemitterAcc", SqlDbType.NVarChar).Value = dr("RemitterAcc").ToString()
                        Command.Parameters.Add("@RemitterName", SqlDbType.NVarChar).Value = dr("RemitterName").ToString()
                        Command.Parameters.Add("@RemitterBic", SqlDbType.NVarChar).Value = dr("RemitterBic").ToString()
                        Command.Parameters.Add("@RemitterBranch", SqlDbType.NVarChar).Value = dr("RemitterBranch").ToString()
                        Command.Parameters.Add("@AdditionalInfo", SqlDbType.NVarChar).Value = dr("AdditionalInfo").ToString()
                        Command.Parameters.Add("@CreatedBy", SqlDbType.NVarChar).Value = dr("CreatedBy").ToString()
                        SqlConn = GetConnectionSQL()
                        Command.ExecuteNonQuery()
                    Catch ex As Exception
                        Dim AppendErrorMessage As String = "Error Message ClearingFilesDataManuplation: Clearing Files"
                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "Date" + ":" + DateTime.Now
                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "--------------------------"
                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "Mod3291" + ex.Message.ToString()
                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "--------------------------"
                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine
                        If Not Directory.Exists("C:\ClearingFilesErrorLog") Then
                            Directory.CreateDirectory("C:\ClearingFilesErrorLog")
                        End If
                        System.IO.File.AppendAllText("C:\ClearingFilesErrorLog\" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + ".txt", AppendErrorMessage)

                        'MessageBox.Show("Mod2855: " + ex.Message)
                    End Try
                Next
            End If
        Catch ex As Exception
            Dim AppendErrorMessage As String = "Error Message ClearingFilesDataManuplation: RTGS Files"
            AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "Date" + ":" + DateTime.Now
            AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "--------------------------"
            AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "Mod3296" + ex.Message.ToString()
            AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "--------------------------"
            AppendErrorMessage = AppendErrorMessage + Environment.NewLine
            If Not Directory.Exists("C:\ClearingFilesErrorLog") Then
                Directory.CreateDirectory("C:\ClearingFilesErrorLog")
            End If
            System.IO.File.AppendAllText("C:\ClearingFilesErrorLog\" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + ".txt", AppendErrorMessage)


            'MessageBox.Show("1245" + ex.Message)
        End Try
    End Function

    Public Shared Function SaveKEINUnpaid(ByVal dtBrOutClearing As DataTable) As Boolean
        Dim Command As SqlCommand
        Dim dr As Data.DataRow
        Dim DestAccount As String = ""
        Dim x As New System.Random
        Try
            If dtBrOutClearing.Rows.Count > 0 Then
                For Each dr In dtBrOutClearing.Rows
                    GetConnectionSQL()
                    Try
                        SqlConn = GetConnectionSQL()
                        Command = New SqlCommand("p_Unpaids", SqlConn)
                        Command.CommandType = CommandType.StoredProcedure
                        Command.Parameters.Add("@TxFileName", SqlDbType.NVarChar).Value = dr("TxFileName").ToString()
                        Command.Parameters.Add("@FType", SqlDbType.NVarChar).Value = dr("FType").ToString()
                        Command.Parameters.Add("@RtrId", SqlDbType.NVarChar).Value = dr("RtrId").ToString()
                        Command.Parameters.Add("@RetCode", SqlDbType.NVarChar).Value = dr("RetCode").ToString()
                        Command.Parameters.Add("@MsgID", SqlDbType.NVarChar).Value = dr("MsgID").ToString()
                        Command.Parameters.Add("@OrgnlEndToEnd", SqlDbType.NVarChar).Value = dr("OrgnlEndToEnd").ToString()
                        Command.Parameters.Add("@TrxId", SqlDbType.NVarChar).Value = dr("TrxId").ToString()
                        Command.Parameters.Add("@OrgnlTxId", SqlDbType.NVarChar).Value = dr("OrgnlTxId").ToString()
                        Command.Parameters.Add("@Amount", SqlDbType.NVarChar).Value = dr("Amount").ToString()
                        Command.Parameters.Add("@ChqID", SqlDbType.NVarChar).Value = dr("ChqID").ToString()
                        Command.Parameters.Add("@AccountID", SqlDbType.NVarChar).Value = dr("AccountID").ToString()
                        Command.Parameters.Add("@PresentingSortCode", SqlDbType.NVarChar).Value = dr("PresentingSortCode").ToString()
                        Command.Parameters.Add("@CurrencyCode", SqlDbType.NVarChar).Value = dr("CurrencyCode").ToString()
                        Command.Parameters.Add("@SortCode", SqlDbType.NVarChar).Value = dr("SortCode").ToString()
                        Command.Parameters.Add("@CheckDigit", SqlDbType.NVarChar).Value = dr("CheckDigit").ToString()
                        Command.Parameters.Add("@DepositDate", SqlDbType.NVarChar).Value = dr("DepositDate").ToString()
                        Command.Parameters.Add("@FImage", SqlDbType.NVarChar).Value = dr("FrontGS").ToString()
                        Command.Parameters.Add("@UVImage", SqlDbType.NVarChar).Value = dr("FrontUV").ToString()
                        Command.Parameters.Add("@BImage", SqlDbType.NVarChar).Value = dr("FrontBW").ToString()
                        Command.Parameters.Add("@RImage", SqlDbType.NVarChar).Value = dr("BackGS").ToString()
                        Command.Parameters.Add("@PayeeName", SqlDbType.NVarChar).Value = dr("PayeeName").ToString()
                        Command.Parameters.Add("@ColAccount", SqlDbType.NVarChar).Value = dr("ColAccount").ToString()
                        Command.Parameters.Add("@TransactionCode", SqlDbType.NVarChar).Value = dr("TransactionCode").ToString()
                        Command.Parameters.Add("@OperatorID", SqlDbType.NVarChar).Value = Modscan.OperatorID.ToString()
                        SqlConn = GetConnectionSQL()
                        Command.ExecuteNonQuery()
                    Catch ex As Exception
                        Dim AppendErrorMessage As String = "Error Message ClearingFilesDataManuplation: Clearing Files"
                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "Date" + ":" + DateTime.Now
                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "--------------------------"
                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "Mod3291" + ex.Message.ToString()
                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "--------------------------"
                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine
                        If Not Directory.Exists("C:\ClearingFilesErrorLog") Then
                            Directory.CreateDirectory("C:\ClearingFilesErrorLog")
                        End If
                        System.IO.File.AppendAllText("C:\ClearingFilesErrorLog\" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + ".txt", AppendErrorMessage)

                        'MessageBox.Show("Mod2855: " + ex.Message)
                    End Try
                Next
            End If
        Catch ex As Exception
            Dim AppendErrorMessage As String = "Error Message ClearingFilesDataManuplation: Clearing Files"
            AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "Date" + ":" + DateTime.Now
            AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "--------------------------"
            AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "Mod3296" + ex.Message.ToString()
            AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "--------------------------"
            AppendErrorMessage = AppendErrorMessage + Environment.NewLine
            If Not Directory.Exists("C:\ClearingFilesErrorLog") Then
                Directory.CreateDirectory("C:\ClearingFilesErrorLog")
            End If
            System.IO.File.AppendAllText("C:\ClearingFilesErrorLog\" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + ".txt", AppendErrorMessage)


            'MessageBox.Show("1245" + ex.Message)
        End Try
    End Function
    Public Shared Function SaveKEUnpaidDD(ByVal dtBrOutClearing As DataTable) As Boolean
        Dim Command As SqlCommand
        Dim dr As Data.DataRow
        Dim DestAccount As String = ""
        Dim x As New System.Random
        Try
            If dtBrOutClearing.Rows.Count > 0 Then
                For Each dr In dtBrOutClearing.Rows
                    GetConnectionSQL()
                    Try
                        SqlConn = GetConnectionSQL()
                        Command = New SqlCommand("p_UnpaidEFTS", SqlConn)
                        Command.CommandType = CommandType.StoredProcedure
                        Command.Parameters.Add("@TxFileName", SqlDbType.NVarChar).Value = dr("TxFileName").ToString()
                        Command.Parameters.Add("@FType", SqlDbType.NVarChar).Value = dr("FType").ToString()
                        Command.Parameters.Add("@RtrId", SqlDbType.NVarChar).Value = dr("RtrId").ToString()
                        Command.Parameters.Add("@RetCode", SqlDbType.NVarChar).Value = dr("RetCode").ToString()
                        Command.Parameters.Add("@MsgID", SqlDbType.NVarChar).Value = dr("MsgID").ToString()
                        Command.Parameters.Add("@OrgnlEndToEnd", SqlDbType.NVarChar).Value = dr("OrgnlEndToEnd").ToString()
                        Command.Parameters.Add("@TrxId", SqlDbType.NVarChar).Value = dr("TrxId").ToString()
                        Command.Parameters.Add("@OrgnlTxId", SqlDbType.NVarChar).Value = dr("OrgnlTxId").ToString()

                        SqlConn = GetConnectionSQL()
                        Command.ExecuteNonQuery()
                    Catch ex As Exception
                        Dim AppendErrorMessage As String = "Error Message ClearingFilesDataManuplation: Clearing Files"
                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "Date" + ":" + DateTime.Now
                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "--------------------------"
                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "Mod3323" + ex.Message.ToString()
                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "--------------------------"
                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine
                        If Not Directory.Exists("C:\ClearingFilesErrorLog") Then
                            Directory.CreateDirectory("C:\ClearingFilesErrorLog")
                        End If
                        System.IO.File.AppendAllText("C:\ClearingFilesErrorLog\" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + ".txt", AppendErrorMessage)

                        'MessageBox.Show("Mod2855: " + ex.Message)
                    End Try
                Next
            End If
        Catch ex As Exception
            Dim AppendErrorMessage As String = "Error Message ClearingFilesDataManuplation: Clearing Files"
            AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "Date" + ":" + DateTime.Now
            AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "--------------------------"
            AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "Mod3339" + ex.Message.ToString()
            AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "--------------------------"
            AppendErrorMessage = AppendErrorMessage + Environment.NewLine
            If Not Directory.Exists("C:\ClearingFilesErrorLog") Then
                Directory.CreateDirectory("C:\ClearingFilesErrorLog")
            End If
            System.IO.File.AppendAllText("C:\ClearingFilesErrorLog\" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + ".txt", AppendErrorMessage)


            'MessageBox.Show("1245" + ex.Message)
        End Try
    End Function

    Public Shared Function SaveToDDKEDB(ByVal dtBrOutClearing As DataTable) As Boolean
        Dim Command As SqlCommand
        Dim dr As Data.DataRow
        Dim DestAccount As String = ""
        Dim x As New System.Random
        Dim CFDate As Date = Convert.ToDateTime(Modscan.cFromDate)
        Dim cWorkgDate As Date = Convert.ToDateTime(cWorkingDate)
        Dim CTDate As DateTime = Convert.ToDateTime(Modscan.cToDate)
        Modscan.cFromDate = CFDate.ToString("dd-MMM-yyyy")
        Modscan.cToDate = CTDate.ToString("dd-MMM-yyyy")
        cWorkingDate = cWorkgDate.ToString("dd-MMM-yyyy")
        If Modscan.OperatorID IsNot Nothing Then
            If String.IsNullOrEmpty(Modscan.OperatorID) Then
                Modscan.OperatorID = "ClearingService"
            End If
        Else
            Modscan.OperatorID = "ClearingService"
        End If
        Dim DtOfSgntr As String = String.Empty
        Try
            If dtBrOutClearing.Rows.Count > 0 Then
                For Each dr In dtBrOutClearing.Rows
                    GetConnectionSQL()
                    Try

                        If Convert.ToDateTime(dr("DtOfSgntr")) = Convert.ToDateTime("1/1/0001 12:00:00 AM") Then
                            DtOfSgntr = Modscan.cToDate

                        Else
                            DtOfSgntr = dr("DtOfSgntr").ToString()
                        End If


                        SqlConn = GetConnectionSQL()
                        Command = New SqlCommand("p_AddKEDDs", SqlConn)
                        Command.CommandType = CommandType.StoredProcedure
                        Command.Parameters.Add("@RCODE", SqlDbType.NVarChar).Value = dr("RCODE").ToString()
                        Command.Parameters.Add("@VTYPE", SqlDbType.NVarChar).Value = dr("VTYPE").ToString()
                        Command.Parameters.Add("@FileName", SqlDbType.NVarChar).Value = dr("FileName").ToString()
                        Command.Parameters.Add("@SourceBIC", SqlDbType.NVarChar).Value = dr("SourceBIC").ToString()
                        Command.Parameters.Add("@MsgID", SqlDbType.NVarChar).Value = dr("MsgID").ToString()
                        Command.Parameters.Add("@CreDtTm", SqlDbType.NVarChar).Value = dr("CreDtTm").ToString()
                        Command.Parameters.Add("@EndToEndId", SqlDbType.NVarChar).Value = dr("EndToEndId").ToString()
                        Command.Parameters.Add("@InstrId", SqlDbType.NVarChar).Value = dr("InstrId").ToString()
                        Command.Parameters.Add("@TrxId", SqlDbType.NVarChar).Value = dr("TrxId").ToString()
                        Command.Parameters.Add("@LclInstrm", SqlDbType.NVarChar).Value = dr("LclInstrm").ToString()
                        Command.Parameters.Add("@IntrBkSttlmDt", SqlDbType.NVarChar).Value = "" 'dr("IntrBkSttlmDt")
                        Command.Parameters.Add("@DNm", SqlDbType.NVarChar).Value = dr("DNm").ToString()
                        Command.Parameters.Add("@DbtrAcct", SqlDbType.NVarChar).Value = dr("DbtrAcct").ToString()
                        Command.Parameters.Add("@CNm", SqlDbType.NVarChar).Value = dr("CNm").ToString()
                        Command.Parameters.Add("@CdtrAcct", SqlDbType.NVarChar).Value = dr("CdtrAcct").ToString()
                        Command.Parameters.Add("@RmtInf", SqlDbType.NVarChar).Value = "" 'dr("RmtInf").ToString()
                        Command.Parameters.Add("@IntrBkSttlmAmt", SqlDbType.NVarChar).Value = dr("IntrBkSttlmAmt").ToString()
                        Command.Parameters.Add("@CurrencyID", SqlDbType.NVarChar).Value = dr("CurrencyID").ToString()
                        Command.Parameters.Add("@ReqdColltnDt", SqlDbType.DateTime).Value = Convert.ToDateTime(dr("ReqdColltnDt"))
                        Command.Parameters.Add("@MndtId", SqlDbType.NVarChar).Value = dr("MndtId").ToString()
                        Command.Parameters.Add("@DtOfSgntr", SqlDbType.DateTime).Value = DtOfSgntr 'Convert.ToDateTime(dr("DtOfSgntr"))
                        Command.Parameters.Add("@SeqTp", SqlDbType.NVarChar).Value = dr("SeqTp").ToString()
                        Command.Parameters.Add("@PrvtId", SqlDbType.NVarChar).Value = dr("PrvtId").ToString()

                        Command.Parameters.Add("@OrigCode", SqlDbType.NVarChar).Value = dr("OrigCode").ToString()
                        Command.Parameters.Add("@Policy1", SqlDbType.NVarChar).Value = dr("Policy1")
                        Command.Parameters.Add("@Policy2", SqlDbType.NVarChar).Value = dr("Policy2").ToString()
                        Command.Parameters.Add("@DestBranchId", SqlDbType.NVarChar).Value = dr("DestBranchId")
                        Command.Parameters.Add("@DestBranchName", SqlDbType.NVarChar).Value = dr("DestBranchName").ToString()
                        Command.Parameters.Add("@SourceBranchId", SqlDbType.NVarChar).Value = dr("SourceBranchId").ToString()
                        Command.Parameters.Add("@SourceBranchName", SqlDbType.NVarChar).Value = dr("SourceBranchName").ToString()
                        Command.Parameters.Add("@Amount", SqlDbType.NVarChar).Value = dr("Amount").ToString()
                        Command.Parameters.Add("@OperatorID", SqlDbType.NVarChar).Value = Modscan.OperatorID.ToString()
                        SqlConn = GetConnectionSQL()
                        Command.ExecuteNonQuery()
                    Catch ex As Exception
                        Dim AppendErrorMessage As String = "Error Message ClearingFilesDataManuplation: Clearing Files"
                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "Date" + ":" + DateTime.Now
                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "--------------------------"
                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "Mod3395" + ex.Message.ToString()
                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "--------------------------"
                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine
                        If Not Directory.Exists("C:\ClearingFilesErrorLog") Then
                            Directory.CreateDirectory("C:\ClearingFilesErrorLog")
                        End If
                        System.IO.File.AppendAllText("C:\ClearingFilesErrorLog\" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + ".txt", AppendErrorMessage)

                        MessageBox.Show("Mod2855: " + ex.Message)
                    End Try
                Next
            End If
        Catch ex As Exception
            Dim AppendErrorMessage As String = "Error Message ClearingFilesDataManuplation: Clearing Files"
            AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "Date" + ":" + DateTime.Now
            AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "--------------------------"
            AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "Mod3412" + ex.Message.ToString()
            AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "--------------------------"
            AppendErrorMessage = AppendErrorMessage + Environment.NewLine
            If Not Directory.Exists("C:\ClearingFilesErrorLog") Then
                Directory.CreateDirectory("C:\ClearingFilesErrorLog")
            End If
            System.IO.File.AppendAllText("C:\ClearingFilesErrorLog\" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + ".txt", AppendErrorMessage)

            ' MessageBox.Show("1245" + ex.Message)
        End Try
    End Function
    Public Shared Function SaveToEFTKEDB(ByVal dtBrOutClearing As DataTable) As Boolean
        Dim Command As SqlCommand
        Dim dr As Data.DataRow
        Dim DestAccount As String = ""
        Dim x As New System.Random
        Dim CFDate As Date = Convert.ToDateTime(Modscan.cFromDate)
        Dim cWorkgDate As Date = Convert.ToDateTime(cWorkingDate)
        Dim CTDate As DateTime = Convert.ToDateTime(Modscan.cToDate)
        Modscan.cFromDate = CFDate.ToString("dd-MMM-yyyy")
        Modscan.cToDate = CTDate.ToString("dd-MMM-yyyy")
        If Modscan.OperatorID IsNot Nothing Then
            If String.IsNullOrEmpty(Modscan.OperatorID) Then
                Modscan.OperatorID = "ClearingService"
            End If
        Else
            Modscan.OperatorID = "ClearingService"
        End If
        cWorkingDate = cWorkgDate.ToString("dd-MMM-yyyy")
        Try
            If dtBrOutClearing.Rows.Count > 0 Then
                For Each dr In dtBrOutClearing.Rows
                    GetConnectionSQL()
                    Try
                        SqlConn = GetConnectionSQL()
                        Command = New SqlCommand("p_AddKEEFTs", SqlConn)
                        Command.CommandType = CommandType.StoredProcedure
                        Command.Parameters.Add("@RCODE", SqlDbType.NVarChar).Value = dr("RCODE").ToString()
                        Command.Parameters.Add("@VTYPE", SqlDbType.NVarChar).Value = dr("VTYPE").ToString()
                        Command.Parameters.Add("@FileName", SqlDbType.NVarChar).Value = dr("FileName").ToString()
                        Command.Parameters.Add("@TrxTypeID", SqlDbType.NVarChar).Value = dr("TrxTypeID").ToString()
                        Command.Parameters.Add("@SourceBIC", SqlDbType.NVarChar).Value = dr("SourceBIC").ToString()
                        Command.Parameters.Add("@MsgID", SqlDbType.NVarChar).Value = dr("MsgID").ToString()
                        Command.Parameters.Add("@CreDtTm", SqlDbType.NVarChar).Value = dr("CreDtTm").ToString()
                        Command.Parameters.Add("@EndToEndId", SqlDbType.NVarChar).Value = dr("EndToEndId").ToString()
                        Command.Parameters.Add("@TrxId", SqlDbType.NVarChar).Value = dr("TrxId").ToString()
                        Command.Parameters.Add("@LclInstrm", SqlDbType.NVarChar).Value = dr("LclInstrm").ToString()
                        Command.Parameters.Add("@IntrBkSttlmDt", SqlDbType.NVarChar).Value = dr("IntrBkSttlmDt").ToString()
                        Command.Parameters.Add("@DInstgAgtBrnchId", SqlDbType.NVarChar).Value = dr("DInstgAgtBrnchId").ToString()
                        Command.Parameters.Add("@DInstgAgtBrnchNM", SqlDbType.NVarChar).Value = dr("DInstgAgtBrnchNM").ToString()
                        Command.Parameters.Add("@DNm", SqlDbType.NVarChar).Value = dr("DNm").ToString()
                        Command.Parameters.Add("@DStrtNm", SqlDbType.NVarChar).Value = dr("DStrtNm").ToString()
                        Command.Parameters.Add("@DBldgNb", SqlDbType.NVarChar).Value = dr("DBldgNb").ToString()
                        Command.Parameters.Add("@DPstBx", SqlDbType.NVarChar).Value = dr("DPstBx").ToString()
                        Command.Parameters.Add("@DPstCd", SqlDbType.NVarChar).Value = dr("DPstCd").ToString()
                        Command.Parameters.Add("@DTwnNm", SqlDbType.NVarChar).Value = dr("DTwnNm").ToString()
                        Command.Parameters.Add("@DCtry", SqlDbType.NVarChar).Value = dr("DCtry").ToString()
                        Command.Parameters.Add("@DbtrAcct", SqlDbType.NVarChar).Value = dr("DbtrAcct").ToString()
                        Command.Parameters.Add("@Dtp", SqlDbType.NVarChar).Value = dr("Dtp").ToString()
                        Command.Parameters.Add("@CInstgAgtBrnchId", SqlDbType.NVarChar).Value = dr("CInstgAgtBrnchId").ToString()
                        Command.Parameters.Add("@CInstgAgtBrnchNM", SqlDbType.NVarChar).Value = dr("CInstgAgtBrnchNM").ToString()
                        Command.Parameters.Add("@CNm", SqlDbType.NVarChar).Value = dr("CNm").ToString()
                        Command.Parameters.Add("@CStrtNm", SqlDbType.NVarChar).Value = dr("CStrtNm").ToString()
                        Command.Parameters.Add("@CBldgNb", SqlDbType.NVarChar).Value = dr("CBldgNb").ToString()
                        Command.Parameters.Add("@CPstBx", SqlDbType.NVarChar).Value = dr("CPstBx").ToString()
                        Command.Parameters.Add("@CPstCd", SqlDbType.NVarChar).Value = dr("CPstCd").ToString()
                        Command.Parameters.Add("@CTwnNm", SqlDbType.NVarChar).Value = dr("CTwnNm").ToString()
                        Command.Parameters.Add("@CCtry", SqlDbType.NVarChar).Value = dr("CCtry").ToString()
                        Command.Parameters.Add("@CdtrAcct", SqlDbType.NVarChar).Value = dr("CdtrAcct").ToString()
                        Command.Parameters.Add("@Ctp", SqlDbType.NVarChar).Value = dr("Ctp").ToString()
                        Command.Parameters.Add("@RmtInf", SqlDbType.NVarChar).Value = dr("RmtInf").ToString()
                        Command.Parameters.Add("@Amount", SqlDbType.NVarChar).Value = dr("Amount").ToString()
                        Command.Parameters.Add("@CurrencyID", SqlDbType.NVarChar).Value = dr("CurrencyID").ToString()
                        Command.Parameters.Add("@OperatorID", SqlDbType.NVarChar).Value = Modscan.OperatorID.ToString()
                        SqlConn = GetConnectionSQL()
                        Command.ExecuteNonQuery()
                    Catch ex As Exception
                        Dim AppendErrorMessage As String = "Error Message ClearingFilesDataManuplation: Clearing Files"
                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "Date" + ":" + DateTime.Now
                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "--------------------------"
                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "Mod2855" + ex.Message.ToString()
                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "--------------------------"
                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine
                        If Not Directory.Exists("C:\ClearingFilesErrorLog") Then
                            Directory.CreateDirectory("C:\ClearingFilesErrorLog")
                        End If
                        System.IO.File.AppendAllText("C:\ClearingFilesErrorLog\" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + ".txt", AppendErrorMessage)

                        'MessageBox.Show("Mod2855: " + ex.Message)
                    End Try
                Next
            End If
        Catch ex As Exception
            Dim AppendErrorMessage As String = "Error Message ClearingFilesDataManuplation: Clearing Files"
            AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "Date" + ":" + DateTime.Now
            AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "--------------------------"
            AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "1245" + ex.Message.ToString()
            AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "--------------------------"
            AppendErrorMessage = AppendErrorMessage + Environment.NewLine
            If Not Directory.Exists("C:\ClearingFilesErrorLog") Then
                Directory.CreateDirectory("C:\ClearingFilesErrorLog")
            End If
            System.IO.File.AppendAllText("C:\ClearingFilesErrorLog\" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + ".txt", AppendErrorMessage)
            'MessageBox.Show("1245" + ex.Message)
        End Try
    End Function
    Public Shared Function SaveToCHQKEDB(ByVal dtBrOutClearing As DataTable) As Boolean
        Dim Command As SqlCommand
        Dim dr As Data.DataRow
        Dim DestAccount As String = ""
        Dim x As New System.Random
        Dim CFDate As Date = Convert.ToDateTime(Modscan.cFromDate)
        Dim cWorkgDate As Date = Convert.ToDateTime(cWorkingDate)
        Dim CTDate As DateTime = Convert.ToDateTime(Modscan.cToDate)
        Modscan.cFromDate = CFDate.ToString("dd-MMM-yyyy")
        Modscan.cToDate = CTDate.ToString("dd-MMM-yyyy")
        cWorkingDate = cWorkgDate.ToString("dd-MMM-yyyy")



        If Modscan.OperatorID IsNot Nothing Then
            If String.IsNullOrEmpty(Modscan.OperatorID) Then
                Modscan.OperatorID = "ClearingService"
            End If
        Else
            Modscan.OperatorID = "ClearingService"
        End If

        Try
            If dtBrOutClearing.Rows.Count > 0 Then
                For Each dr In dtBrOutClearing.Rows
                    GetConnectionSQL()
                    Try
                        SqlConn = GetConnectionSQL()
                        Command = New SqlCommand("p_AddCHQISOKE", SqlConn)
                        Command.CommandType = CommandType.StoredProcedure
                        Command.Parameters.Add("@MsgID", SqlDbType.NVarChar).Value = dr("MsgID").ToString()
                        Command.Parameters.Add("@CreDtTm", SqlDbType.NVarChar).Value = dr("CreDtTm").ToString()
                        Command.Parameters.Add("@RecordType", SqlDbType.NVarChar).Value = dr("RecordType").ToString()
                        Command.Parameters.Add("@Value", SqlDbType.NVarChar).Value = dr("Value").ToString()
                        Command.Parameters.Add("@SortCode", SqlDbType.NVarChar).Value = dr("SortCode").ToString()
                        Command.Parameters.Add("@BankBIC", SqlDbType.NVarChar).Value = dr("BankBIC").ToString()
                        Command.Parameters.Add("@PresentingSortCode", SqlDbType.NVarChar).Value = dr("PresentingSortCode").ToString()
                        Command.Parameters.Add("@ReasonForReturn", SqlDbType.NVarChar).Value = dr("ReasonForReturn").ToString()
                        Command.Parameters.Add("@TransactionCode", SqlDbType.NVarChar).Value = dr("TransactionCode").ToString()
                        Command.Parameters.Add("@CurrencyCode", SqlDbType.NVarChar).Value = dr("CurrencyCode").ToString()
                        Command.Parameters.Add("@AccountNo", SqlDbType.NVarChar).Value = dr("AccountNo").ToString()
                        Command.Parameters.Add("@SerialNo", SqlDbType.NVarChar).Value = dr("SerialNo").ToString()
                        Command.Parameters.Add("@CheckDigit", SqlDbType.NVarChar).Value = dr("CheckDigit").ToString()
                        Command.Parameters.Add("@DepositDate", SqlDbType.DateTime).Value = dr("DepositDate")
                        Command.Parameters.Add("@UniqueReferenceID", SqlDbType.NVarChar).Value = dr("UniqueReferenceID").ToString()
                        Command.Parameters.Add("@ColAccount", SqlDbType.NVarChar).Value = dr("ColAccount").ToString()
                        Command.Parameters.Add("@PayeeName", SqlDbType.NVarChar).Value = dr("PayeeName").ToString()
                        Command.Parameters.Add("@PurposeOfPayment", SqlDbType.NVarChar).Value = dr("PurposeOfPayment").ToString()
                        Command.Parameters.Add("@RelatedReferenceID", SqlDbType.NVarChar).Value = dr("RelatedReferenceID").ToString()
                        Command.Parameters.Add("@FrontBW", SqlDbType.NVarChar).Value = dr("FrontBW").ToString()
                        Command.Parameters.Add("@FrontGS", SqlDbType.NVarChar).Value = dr("FrontGS").ToString()
                        Command.Parameters.Add("@BackGS", SqlDbType.NVarChar).Value = dr("BackGS").ToString()
                        Command.Parameters.Add("@FrontUV", SqlDbType.NVarChar).Value = dr("FrontUV").ToString()
                        Command.Parameters.Add("@FileName", SqlDbType.NVarChar).Value = dr("FileName").ToString()
                        Command.Parameters.Add("@OperatorID", SqlDbType.NVarChar).Value = Modscan.OperatorID.ToString()
                        SqlConn = GetConnectionSQL()
                        Command.ExecuteNonQuery()
                    Catch ex As Exception
                        Dim AppendErrorMessage As String = "Error Message ClearingFilesDataManuplation: Clearing Files"
                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "Date" + ":" + DateTime.Now
                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "--------------------------"
                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "Mod2855: " + ex.Message.ToString()
                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "--------------------------"
                        AppendErrorMessage = AppendErrorMessage + Environment.NewLine
                        If Not Directory.Exists("C:\ClearingFilesErrorLog") Then
                            Directory.CreateDirectory("C:\ClearingFilesErrorLog")
                        End If
                        System.IO.File.AppendAllText("C:\ClearingFilesErrorLog\" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + ".txt", AppendErrorMessage)
                        'Return False
                        'MessageBox.Show("Mod2855: " + ex.Message)
                    End Try
                Next
            End If
        Catch ex As Exception
            Dim AppendErrorMessage As String = "Error Message ClearingFilesDataManuplation: Clearing Files"
            AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "Date" + ":" + DateTime.Now
            AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "--------------------------"
            AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "1245" + ex.Message.ToString()
            AppendErrorMessage = AppendErrorMessage + Environment.NewLine + "--------------------------"
            AppendErrorMessage = AppendErrorMessage + Environment.NewLine
            If Not Directory.Exists("C:\ClearingFilesErrorLog") Then
                Directory.CreateDirectory("C:\ClearingFilesErrorLog")
            End If
            System.IO.File.AppendAllText("C:\ClearingFilesErrorLog\" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + ".txt", AppendErrorMessage)

            'MessageBox.Show("1245" + ex.Message)
        End Try
    End Function
    Public Shared Function SaveToResponse(ByVal dtBrOutClearing As DataTable) As Boolean
        Dim Command As SqlCommand
        Dim dr As Data.DataRow
        Try
            If dtBrOutClearing.Rows.Count > 0 Then
                For Each dr In dtBrOutClearing.Rows
                    GetConnectionSQL()
                    Try
                        SqlConn = GetConnectionSQL()
                        Command = New SqlCommand("p_AddISOClearingResponses", SqlConn)
                        Command.CommandType = CommandType.StoredProcedure
                        Command.Parameters.Add("@OrgnlTxId", SqlDbType.NVarChar).Value = dr("OrgnlTxId").ToString()
                        Command.Parameters.Add("@OrgnlEndToEndId", SqlDbType.NVarChar).Value = dr("OrgnlEndToEndId").ToString()
                        Command.Parameters.Add("@TxSts", SqlDbType.NVarChar).Value = dr("TxSts").ToString()
                        Command.Parameters.Add("@Status", SqlDbType.NVarChar).Value = dr("Status").ToString()
                        Command.Parameters.Add("@StsRsnInf", SqlDbType.NVarChar).Value = dr("StsRsnInf").ToString()
                        Command.Parameters.Add("@OrgnlMsgId", SqlDbType.NVarChar).Value = dr("OrgnlMsgId").ToString()
                        Command.Parameters.Add("@OrgnlMsgNmId", SqlDbType.NVarChar).Value = dr("OrgnlMsgNmId").ToString()
                        SqlConn = GetConnectionSQL()
                        Command.ExecuteNonQuery()
                    Catch ex As Exception
                        MessageBox.Show("Mod2855: " + ex.Message)
                    End Try
                Next
            End If
        Catch ex As Exception
            MessageBox.Show("1245" + ex.Message)
        End Try
    End Function

    Public Shared Function SaveMandateResponse(ByVal dtBrOutClearing As DataTable) As Boolean
        Dim Command As SqlCommand
        Dim dr As Data.DataRow
        Try
            If dtBrOutClearing.Rows.Count > 0 Then
                For Each dr In dtBrOutClearing.Rows
                    GetConnectionSQL()
                    Try
                        SqlConn = GetConnectionSQL()
                        Command = New SqlCommand("p_AddISOMandateClearingResponses", SqlConn)
                        Command.CommandType = CommandType.StoredProcedure
                        Command.Parameters.Add("@MsgId", SqlDbType.NVarChar).Value = dr("MsgId").ToString()
                        Command.Parameters.Add("@GrpHdrMsgId", SqlDbType.NVarChar).Value = dr("GrpHdrMsgId").ToString()
                        Command.Parameters.Add("@MsgNmId", SqlDbType.NVarChar).Value = dr("MsgNmId").ToString()
                        Command.Parameters.Add("@CreDtTm", SqlDbType.NVarChar).Value = dr("CreDtTm").ToString()
                        Command.Parameters.Add("@Accptd", SqlDbType.NVarChar).Value = dr("Accptd").ToString()
                        Command.Parameters.Add("@RjctRsn", SqlDbType.NVarChar).Value = dr("RjctRsn").ToString()
                        Command.Parameters.Add("@AddtlRjctRsnInf", SqlDbType.NVarChar).Value = dr("AddtlRjctRsnInf").ToString()
                        Command.Parameters.Add("@OrgnlMndtId", SqlDbType.NVarChar).Value = dr("OrgnlMndtId").ToString()
                        Command.Parameters.Add("@MndtReqId", SqlDbType.NVarChar).Value = dr("MndtReqId").ToString()
                        Command.Parameters.Add("@InstgAgt", SqlDbType.NVarChar).Value = dr("InstgAgt").ToString()
                        Command.Parameters.Add("@FileType", SqlDbType.NVarChar).Value = dr("FileType").ToString()
                        SqlConn = GetConnectionSQL()
                        Command.ExecuteNonQuery()
                    Catch ex As Exception
                        MessageBox.Show("Mod2855: " + ex.Message)
                    End Try
                Next
            End If
        Catch ex As Exception
            MessageBox.Show("1245" + ex.Message)
        End Try
    End Function

    Public Shared Function SaveSettlement(ByVal dtBrOutClearing As DataTable) As Boolean
        Dim Command As SqlCommand
        Dim dr As Data.DataRow
        Try
            If dtBrOutClearing.Rows.Count > 0 Then
                For Each dr In dtBrOutClearing.Rows
                    GetConnectionSQL()
                    Try
                        SqlConn = GetConnectionSQL()
                        Command = New SqlCommand("p_AddISOSettlement", SqlConn)
                        Command.CommandType = CommandType.StoredProcedure
                        Command.Parameters.Add("@SettlementDate", SqlDbType.DateTime).Value = dr("SettlementDate").ToString()
                        Command.Parameters.Add("@Bank", SqlDbType.NVarChar).Value = dr("BankID").ToString()
                        Command.Parameters.Add("@RecordCount", SqlDbType.Int).Value = dr("RecordCount").ToString()
                        Command.Parameters.Add("@statusID", SqlDbType.NVarChar).Value = dr("statusID").ToString()
                        Command.Parameters.Add("@CurrencyID", SqlDbType.NVarChar).Value = dr("TrxCurrencyID").ToString()
                        Command.Parameters.Add("@Type", SqlDbType.NVarChar).Value = dr("TrxType").ToString()
                        Command.Parameters.Add("@Amount", SqlDbType.Decimal).Value = dr("Total").ToString()
                        Command.Parameters.Add("@FileName", SqlDbType.NVarChar).Value = dr("FileName").ToString()
                        SqlConn = GetConnectionSQL()
                        Command.ExecuteNonQuery()
                    Catch ex As Exception
                        MessageBox.Show("Mod2855: " + ex.Message)
                    End Try
                Next
            End If
        Catch ex As Exception
            MessageBox.Show("1245" + ex.Message)
        End Try
    End Function

    Public Shared Function ConvertImages(ByVal ImgLocation As String) As Byte()
        Dim Fs As New System.IO.FileStream(ImgLocation, IO.FileMode.Open)
        Try
            ISize = 0
            Dim Fi As System.IO.FileInfo = New System.IO.FileInfo(ImgLocation)
            If Not Fi.Exists Then
                ConvertImages = Nothing
                Exit Function
            End If
            Dim FileLen_Tmp As Long = Fi.Length
            Dim FileLen As Integer = Convert.ToInt32(FileLen_Tmp)
            ISize = FileLen
            Dim picture As Byte() = New Byte(FileLen - 1) {}
            Fs.Read(picture, 0, FileLen)
            Fs.Close()
            ConvertImages = picture
            Fs = Nothing
        Catch ex As Exception
            ConvertImages = Nothing
            Fs = Nothing
        End Try
    End Function
    Private Shared Sub EJContentPlusImage(ByVal FrontBWSize() As Byte, ByVal FrontGSSize() As Byte, ByVal RearGSSize() As Byte,
                                      ByVal FrontBWSignature() As Byte, ByVal FrontGSSignature() As Byte, ByVal RearGSSignature() As Byte,
                                      ByVal FrontBW() As Byte, ByVal FrontGS() As Byte, ByVal RearGS() As Byte, ByVal FilePath As String, ByVal EJLineContent As String, Optional ByVal CheckIfItsData As Boolean = False)
        Dim myFileStream As FileStream = Nothing
        Dim myEJContentStreamWriter As StreamWriter = Nothing
        Try
            myEJContentStreamWriter = New StreamWriter(FilePath, True)
            myEJContentStreamWriter.Write(EJLineContent)
        Finally
            If Not (myEJContentStreamWriter Is Nothing) Then myEJContentStreamWriter.Close()
        End Try

        Try
            myFileStream = New FileStream(FilePath, FileMode.Append)
            myFileStream.Write(FrontBWSize, 0, FrontBWSize.Length)
            myFileStream.Write(FrontBWSignature, 0, FrontBWSignature.Length)
            myFileStream.Write(FrontGSSize, 0, FrontGSSize.Length)
            myFileStream.Write(FrontGSSignature, 0, FrontGSSignature.Length)
            myFileStream.Write(RearGSSize, 0, RearGSSize.Length)
            myFileStream.Write(RearGSSignature, 0, RearGSSignature.Length)
            '--------------Images------------------------
            myFileStream.Write(FrontBW, 0, FrontBW.Length)
            myFileStream.Write(FrontGS, 0, FrontGS.Length)
            myFileStream.Write(RearGS, 0, RearGS.Length)
        Finally
            If Not (myFileStream Is Nothing) Then myFileStream.Close()
        End Try
        If CheckIfItsData = True Then
            myEJContentStreamWriter = Nothing
            Try
                myEJContentStreamWriter = New StreamWriter(FilePath, True)
                myEJContentStreamWriter.WriteLine()
            Finally
                If Not (myEJContentStreamWriter Is Nothing) Then myEJContentStreamWriter.Close()
            End Try
        End If
    End Sub
    Private Shared Sub OtherContents(ByVal FilePath As String, ByVal EJOtherContent As String)
        Dim myFileStream As FileStream = Nothing
        Dim myEJContentStreamWriter As StreamWriter = Nothing
        Try
            myEJContentStreamWriter = New StreamWriter(FilePath, True)
            myEJContentStreamWriter.WriteLine(EJOtherContent)
        Finally
            If Not (myEJContentStreamWriter Is Nothing) Then myEJContentStreamWriter.Close()
        End Try
    End Sub
    Private Shared Sub TrailerContents(ByVal FilePath As String, ByVal EJOtherContent As String)
        Dim myFileStream As FileStream = Nothing
        Dim myEJContentStreamWriter As StreamWriter = Nothing
        Try
            myEJContentStreamWriter = New StreamWriter(FilePath, True)
            myEJContentStreamWriter.WriteLine()
            myEJContentStreamWriter.WriteLine(EJOtherContent)
        Finally
            If Not (myEJContentStreamWriter Is Nothing) Then myEJContentStreamWriter.Close()
        End Try
    End Sub
    Public Shared Sub WriterContents(ByVal FilePath As String, ByVal Content As String)
        Dim myFileStream As FileStream = Nothing
        Dim myEJContentStreamWriter As StreamWriter = Nothing
        Try
            myEJContentStreamWriter = New StreamWriter(FilePath, True)
            'myEJContentStreamWriter.WriteLine()
            myEJContentStreamWriter.WriteLine(Content)
        Finally
            If Not (myEJContentStreamWriter Is Nothing) Then myEJContentStreamWriter.Close()
        End Try
    End Sub
    Public Shared Sub ReadDataIntoADataTable()
        Dim FileName As String = ""
        Dim ColumnID As String = ""
        Dim FrontBW() As Byte = Nothing
        Dim FrontGS() As Byte = Nothing
        Dim RearGS() As Byte = Nothing
        Dim EJLineContent As String = ""
        Dim EJClonefoundRows() As DataRow
        Dim FrontBWSize() As Byte = Nothing
        Dim FrontGSSize() As Byte = Nothing
        Dim RearGSSize() As Byte = Nothing
        Dim FrontBWSignature() As Byte = Nothing
        Dim FrontGSSignature() As Byte = Nothing
        Dim RearGSSignature() As Byte = Nothing
        Dim FPath As String = ""
        FPath = FilePath
        FPath = FPath & "\"
        Dim BankArr As ArrayList = Nothing

        'Banks
        ExecuteData(GetModify("sp_OutClearingBanks"), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.SelectStatement)
        Dim publicDTblBankCopy As DataTable
        publicDTblBankCopy = publicDTbl.Clone()
        EJClonefoundRows = Nothing
        EJClonefoundRows = publicDTbl.Select()
        For i As Int32 = 0 To publicDTbl.Rows.Count - 1
            publicDTblBankCopy.ImportRow(publicDTbl.Rows(i))
        Next
        publicDTbl.Clear()
        BankArr = New ArrayList
        For i As Int32 = 0 To publicDTblBankCopy.Rows.Count - 1
            BankArr.Add(publicDTblBankCopy.Rows(i)(0).ToString)
        Next
        publicDTblBankCopy.Clear()

        'Ejs
        ExecuteData(GetModify("sp_OutEjs"), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.SelectStatement)
        Dim publicDTblEJCopy As DataTable = publicDTbl.Clone()
        EJClonefoundRows = Nothing
        EJClonefoundRows = publicDTbl.Select()
        For i As Int32 = 0 To publicDTbl.Rows.Count - 1
            publicDTblEJCopy.ImportRow(publicDTbl.Rows(i))
        Next
        publicDTbl.Clear()

        'Images
        ExecuteData(GetModify("sp_ChequeItems"), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.SelectStatement)
        'ExecuteData(GetModify("sp_ChequeItems", "FromDate", cFromDate, "ToDate", cToDate), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.SelectStatement)
        Dim publicDTblImagesCopy As DataTable = publicDTbl.Clone()
        EJClonefoundRows = Nothing
        EJClonefoundRows = publicDTbl.Select()
        For i As Int32 = 0 To publicDTbl.Rows.Count - 1
            publicDTblImagesCopy.ImportRow(publicDTbl.Rows(i))
        Next
        publicDTbl.Clear()
        If BankArr.Count > 0 Then
            '' '' '' '' ''FrmBROutFile.prgOutFile.Minimum = 0
            Dim Counter As Double = BankArr.Count - 1 / 100
            For k As Int32 = 0 To BankArr.Count - 1
                'FrmBROutFile.lblBankID.Text = k
                'FrmBROutFile.lblBankID.Text = "Generating Outward EJs for: " & BankArr(k).ToString() & ", Please wait ...."
                System.Windows.Forms.Application.DoEvents()
                If publicDTblEJCopy.Rows.Count > 0 Then
                    FilePath = FPath ' This would be a public variable passed from the BR
                    Dim publicDTblCopy As DataTable = publicDTblEJCopy.Clone
                    Dim publicDTblImageCopy As DataTable = publicDTblImagesCopy.Clone
                    Dim EJfoundRows() As DataRow
                    EJfoundRows = publicDTblEJCopy.Select("(SUBSTRING(FILENAME,16,1)='J' OR SUBSTRING(FILENAME,16,1)='E') AND SUBSTRING(FILENAME,1,2)= '" & BankArr.Item(k) & "'")
                    For i As Int32 = 0 To EJfoundRows.Length - 1
                        publicDTblCopy.ImportRow(EJfoundRows(i))
                    Next
                    Dim CheckIfItsData As Boolean = False
                    'Am looping through the ej items for that Bank
                    For x As Int32 = 0 To publicDTblCopy.Rows.Count - 1
                        CheckIfItsData = False
                        FilePath = FPath & publicDTblCopy.Rows(x)("FileName").ToString
                        Dim ReturnType As String = publicDTblCopy.Rows(x)("text").ToString
                        Dim VoucherType As String = publicDTblCopy.Rows(x)("text").ToString
                        Select Case ReturnType.Substring(0, 2).ToString
                            Case "18", "16", "19"
                                'Control Voucher, Header, Trailer
                                Select Case VoucherType.Substring(0, 4)
                                    Case "1671", "1672", "1673"
                                        EJLineContent = publicDTblCopy.Rows(x)("Text").ToString
                                        TrailerContents(FilePath, EJLineContent)
                                    Case Else
                                        EJLineContent = publicDTblCopy.Rows(x)("Text").ToString
                                        OtherContents(FilePath, EJLineContent)
                                End Select
                            Case Else
                                'Images
                                Dim NextLineData As String = publicDTblCopy.Rows(x + 1)("text").ToString
                                Select Case NextLineData.Substring(0, 2)
                                    Case "16", "18", "19"
                                        CheckIfItsData = False
                                    Case Else
                                        CheckIfItsData = True
                                End Select
                                Dim SerialId As String = publicDTblCopy.Rows(x)("serialid").ToString
                                EJfoundRows = publicDTblImagesCopy.Select("transactionid = '" & SerialId & "'")
                                If EJfoundRows.Length > 0 Then
                                    For i As Int32 = 0 To EJfoundRows.Length - 1
                                        publicDTblImageCopy.ImportRow(EJfoundRows(i))
                                    Next
                                    FrontBW = String2Bytes(publicDTblImageCopy.Rows(0)("TFImage"))
                                    FrontGS = String2Bytes(publicDTblImageCopy.Rows(0)("JFImage"))
                                    RearGS = String2Bytes(publicDTblImageCopy.Rows(0)("JRImage"))
                                    If BitConverter.IsLittleEndian = True Then
                                        FrontBWSize = BitConverter.GetBytes(FrontBW.Length)
                                        FrontGSSize = BitConverter.GetBytes(FrontGS.Length)
                                        RearGSSize = BitConverter.GetBytes(RearGS.Length)
                                    End If
                                    FrontBWSignature = HashTheImage(FrontBW)
                                    FrontGSSignature = HashTheImage(FrontGS)
                                    RearGSSignature = HashTheImage(RearGS)
                                    'Ensure that we are looping through the images
                                    EJLineContent = publicDTblCopy.Rows(x)("Text").ToString
                                    EJContentPlusImage(FrontBWSize, FrontGSSize, RearGSSize, FrontBWSignature, FrontGSSignature, RearGSSignature, FrontBW, FrontGS, RearGS, FilePath, EJLineContent, CheckIfItsData)
                                End If
                                publicDTblImageCopy.Clear()
                        End Select
                    Next
                End If
                'FrmBROutFile.prgOutFile.Increment(Counter)
                System.Windows.Forms.Application.DoEvents()
            Next
            'FrmBROutFile.prgOutFile.Maximum = 100
            System.Windows.Forms.Application.DoEvents()

        End If
    End Sub
    Private Shared Function GetEncoder(ByVal format As ImageFormat) As ImageCodecInfo

        Dim codecs As ImageCodecInfo() = ImageCodecInfo.GetImageDecoders()

        For Each codec As ImageCodecInfo In codecs
            If codec.FormatID = format.Guid Then
                Return codec
            End If
        Next
        Return Nothing
    End Function
    Private Shared Function bytes(ByVal source As Integer) As Byte()
        Return System.Text.Encoding.ASCII.GetBytes(source)
    End Function
    Public Shared Function ImageToByte(ByVal img As Image) As Byte()

        Try
            'img = ValidateFileSize(img)
            Dim imgStream As MemoryStream = New MemoryStream()
            img.Save(imgStream, System.Drawing.Imaging.ImageFormat.Jpeg)
            imgStream.Close()
            Dim byteArray As Byte() = imgStream.ToArray()
            imgStream.Dispose()
            Return byteArray
        Catch ex As Exception

        End Try

    End Function
    Public Shared Function ClearingFilesDataManuplation(usrInfo As UserInfo, TrxType As String, brModule As BRModule, TrxCurrency As String, ByRef dsClearingFileFormat As DS_ClearingFileFormat) As Boolean
        Dim auditInfo As New AuditInfo()
        auditInfo.ModuleID = brModule
        auditInfo.EventID = BROperation.View
        auditInfo.Status = BRStatus.Successful
        Dim ClearingFileFormat As DataTable = New DataTable

        dsClearingFileFormat = New DS_ClearingFileFormat()

        Dim intfDBHelper As IDBHelper = DBClient.GetDBHelper(usrInfo)
        Dim arParms As IDataParameter() = intfDBHelper.CreateDBParamsArray(1)
        arParms(0) = intfDBHelper.CreateNewDBParam("CurrencyID", SqlDbType.NVarChar, 10)
        arParms(0).Value = TrxCurrency
        Try
            Using connection As IDbConnection = GetConnection()
                'ExecuteData(GetModify("p_ClearingFileFormat", "CurrencyID", TrxCurrency), ClearingFileFormat, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)

                intfDBHelper.FillDataset(connection, CommandType.StoredProcedure, "p_ClearingFileFormat", dsClearingFileFormat, New String() {"dt_ClearingFileFormat"}, arParms)
                'dsClearingFileFormat.Tables.Item("") = ClearingFileFormat.Rows(0)(0)


                Dim strNewInfo As String = GetXmlTables(dsClearingFileFormat)
                Using trans As IDbTransaction = connection.BeginTransaction()
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.ToString())
            auditInfo.Status = BRStatus.Failed
            auditInfo.Message = ex.Message
            Dim AppendErrorMessage As String = "Error Message ClearingFilesDataManuplation:" + ex.ToString() + Environment.NewLine + "Date" + ":" + DateTime.Now + auditInfo.ToString() + Environment.NewLine + "--------------------------" + Environment.NewLine
            System.IO.File.AppendAllText("C:\ClearingFiles\" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + ".txt", AppendErrorMessage)
        End Try
        If dsClearingFileFormat.t_ClearingFileFormat.Rows.Count = 0 Then
            Return False
        End If
        Return True
    End Function
    Public Shared Function ImageToByteTif(ByVal img As Image) As Byte()
        Try
            img = ValidateFileSize(img)
            Dim imgStream As MemoryStream = New MemoryStream()
            img.Save(imgStream, System.Drawing.Imaging.ImageFormat.Jpeg)
            imgStream.Close()
            Dim byteArray As Byte() = imgStream.ToArray()
            imgStream.Dispose()
            Return byteArray
        Catch ex As Exception

        End Try

    End Function
    Public Shared Function GetXmlTables(ds As BRBaseDataSet) As String
        Dim strXMLTmp As String = [String].Empty
        strXMLTmp = ds.GetXml()
        Dim mcStrat As MatchCollection = Regex.Matches(strXMLTmp, "<dt_\w*")
        Dim mcEnd As MatchCollection = Regex.Matches(strXMLTmp, "</dt_\w*>")
        Dim sb As New System.Text.StringBuilder()
        If mcStrat.Count <> mcEnd.Count Then
            Throw New Exception("Wrong Input")
        Else
            Dim strArr As String() = New String(mcEnd.Count - 1) {}
            For i As Integer = 0 To mcEnd.Count - 1
                Dim temp As String = strXMLTmp.Substring(strXMLTmp.IndexOf(mcStrat(i).ToString()), ((strXMLTmp.IndexOf(mcEnd(i).ToString()) + mcEnd(i).ToString().Length) - strXMLTmp.IndexOf(mcStrat(i).ToString())))
                strXMLTmp = strXMLTmp.Replace(temp, String.Empty)
                sb.Append(Regex.Replace(temp, " xmlns="".*""", String.Empty))
            Next
        End If
        Return sb.ToString()
    End Function
    Public Shared Function ReadImagesFromFile(
    ByVal PathOfTheFile As String,
    ByVal Currency As String,
    ByVal PresentingBank As String,
    ByVal FileProgress As Windows.Forms.ProgressBar,
    ByVal CheckIfIsFcy As Boolean
    ) As Boolean
        Dim fImageBW() As Byte = Nothing
        Dim EncDecr As New BRClearingEncryptDecrypt.EncDec
        Dim fImage() As Byte = Nothing
        Dim bImage() As Byte = Nothing
        Dim myFrontSize As Int32
        Dim myFrontSize1 As Int32
        Dim myRearSize As Int32
        Dim signCounter As Long
        Dim LineItemsTable As Hashtable
        Dim ht As Hashtable
        Dim MyFile As System.IO.FileStream = Nothing
        Dim StreamReader As System.IO.StreamReader
        Dim myCol As New Collection
        Dim Item As Integer = 1
        Dim My_Line As String = ""
        Dim enableTruncation As Boolean = True
        dt = New DataTable
        Dim OldPathOfTheFile As String = ""
        Try
            LineItemsTable = New Hashtable
            If System.IO.File.Exists(PathOfTheFile) = True Then
                'Read and Decrypt as u safe in a diffent location for rafiki
                If ConfigurationManager.AppSettings("sysEnc") = 1 Then
                    'remove this
                    'EncDec.BRClearingEnc(PathOfTheFile, PathOfTheFile, "D9-49-A5-E6-AE-07-04-51-08-AE-35-78-7A-B8-90-0A-8A-25-86-A8")
                    EncDecr.BRClearingEnc(PathOfTheFile, PathOfTheFile, "D9-49-A5-E6-AE-07-04-51-08-AE-35-78-7A-B8-90-0A-8A-25-86-A8", BRClearingEncryptDecrypt.Action.Decrypt)
                    PathOfTheFile = PathOfTheFile.Substring(0, PathOfTheFile.LastIndexOf("."))
                End If
                'Exit Function
                If System.IO.File.Exists(PathOfTheFile & "_Temp") = True Then System.IO.File.Delete(PathOfTheFile & "_Temp")
                System.IO.File.Copy(PathOfTheFile, PathOfTheFile & "_Temp", True)
                MyFile = System.IO.File.OpenRead(PathOfTheFile & "_Temp")
                StreamReader = New System.IO.StreamReader(MyFile)
                If enableTruncation = False Then
                    While StreamReader.Peek() > -1
                        My_Line = StreamReader.ReadLine().ToString.Trim
                        If My_Line.Trim.Length > 1 Then

                        End If
                        System.Windows.Forms.Application.DoEvents()
                    End While
                Else
                    'hapa import the image file
                    myCol = readImageFile(PathOfTheFile)
                End If
                If myCol.Count > 0 Then
                    FileProgress.Maximum = myCol.Count
                    'break Down the Line then
                    While Item <= myCol.Count
                        FileProgress.Value = Item
                        Select Case myCol.Item(Item).ToString.Substring(0, 2)
                            Case "16", "18", "19"
                                Item += 1
                                Continue While
                            Case Else
                                LineItemsTable.Add("RETURNCODE", myCol.Item(Item).ToString.Substring(0, 2)) ' RCODE
                                LineItemsTable.Add("VOUCHERTYPE", myCol.Item(Item).ToString.Substring(2, 2)) ' Voucher Type
                                LineItemsTable.Add("AMOUNT", (Val(myCol.Item(Item).ToString.Substring(4, 13)) / 100).ToString) ' Amount
                                LineItemsTable.Add("ENTRYMODE", myCol.Item(Item).ToString.Substring(17, 1)) ' Amount Entry Mode
                                LineItemsTable.Add("CURRENCYCODE", myCol.Item(Item).ToString.Substring(34, 2)) ' Amount Entry Mode
                                LineItemsTable.Add("DATA", myCol.Item(Item))
                                Select Case Currency.Trim
                                    Case "00" 'Foreign
                                        'If myCol.Item(Item).ToString.Substring(0, 2) <> "00" Then
                                        '    LineItemsTable.Add("DESTBANK", myCol.Item(Item).ToString.Substring(57, 2)) ' Dest Bank
                                        '    LineItemsTable.Add("TOBRANCH", myCol.Item(Item).ToString.Substring(59, 3)) ' Dest Branch
                                        'Else
                                        LineItemsTable.Add("DESTBANK", myCol.Item(Item).ToString.Substring(18, 2)) ' Dest Bank
                                        LineItemsTable.Add("TOBRANCH", myCol.Item(Item).ToString.Substring(20, 3)) ' Dest Branch
                                        'End If
                                    Case Else
                                        If myCol.Item(Item).ToString.Substring(0, 2) <> "00" Then
                                            LineItemsTable.Add("DESTBANK", myCol.Item(Item).ToString.Substring(18, 2)) ' Dest Bank
                                            LineItemsTable.Add("TOBRANCH", myCol.Item(Item).ToString.Substring(20, 3)) ' Dest Branch
                                        Else
                                            LineItemsTable.Add("DESTBANK", myCol.Item(Item).ToString.Substring(18, 2)) ' Dest Bank
                                            LineItemsTable.Add("TOBRANCH", myCol.Item(Item).ToString.Substring(20, 3)) ' Dest Branch
                                        End If
                                End Select
                                LineItemsTable.Add("TOACCOUNT", myCol.Item(Item).ToString.Substring(23, 10)) ' Dest Account
                                LineItemsTable.Add("CHEQUEDIGIT", myCol.Item(Item).ToString.Substring(33, 1)) ' Check Digit
                                ' theres redundancy in the case statement below but i'll leave it as it
                                Select Case Currency.Trim
                                    Case "00" 'Foreign Clearing
                                        If (ConfigurationManager.AppSettings("Rem") = "1") Then
                                            LineItemsTable.Add("FROMBANK", myCol.Item(Item).ToString.Substring(93, 2)) ' PBank
                                            LineItemsTable.Add("FROMBRANCH", myCol.Item(Item).ToString.Substring(95, 3)) ' PBranch

                                        Else
                                            LineItemsTable.Add("FROMBANK", myCol.Item(Item).ToString.Substring(58, 2)) ' PBank
                                            LineItemsTable.Add("FROMBRANCH", myCol.Item(Item).ToString.Substring(60, 3)) ' PBranch
                                        End If
                                    Case Else
                                        If (ConfigurationManager.AppSettings("Rem") = "1") Then
                                            If myCol.Item(Item).ToString.Substring(0, 2) <> "00" Then
                                                LineItemsTable.Add("FROMBANK", myCol.Item(Item).ToString.Substring(93, 2)) ' PBank
                                                LineItemsTable.Add("FROMBRANCH", myCol.Item(Item).ToString.Substring(95, 3)) ' PBranch

                                            Else
                                                LineItemsTable.Add("FROMBANK", myCol.Item(Item).ToString.Substring(93, 2)) ' PBank
                                                LineItemsTable.Add("FROMBRANCH", myCol.Item(Item).ToString.Substring(95, 3)) ' PBranch
                                            End If
                                        Else
                                            If myCol.Item(Item).ToString.Substring(0, 2) <> "00" Then
                                                LineItemsTable.Add("FROMBANK", myCol.Item(Item).ToString.Substring(58, 2)) ' PBank
                                                LineItemsTable.Add("FROMBRANCH", myCol.Item(Item).ToString.Substring(60, 3)) ' PBranch

                                            Else
                                                LineItemsTable.Add("FROMBANK", myCol.Item(Item).ToString.Substring(58, 2)) ' PBank
                                                LineItemsTable.Add("FROMBRANCH", myCol.Item(Item).ToString.Substring(60, 3)) ' PBranch
                                            End If
                                        End If

                                End Select
                                Select Case Currency.Trim
                                    Case "000"
                                        If (ConfigurationManager.AppSettings("Rem") = "1") Then
                                            LineItemsTable.Add("FILLER", myCol.Item(Item).ToString.Substring(34, 4)) ' Filler
                                            LineItemsTable.Add("COLLECTIONACCOUNT", myCol.Item(Item).ToString.Substring(38, 20)) 'Collecting Account Details
                                            LineItemsTable.Add("COLLECTIONACCOUNTNAME", myCol.Item(Item).ToString.Substring(58, 20)) 'Collecting Account Details
                                            LineItemsTable.Add("SERIALNUMBER", myCol.Item(Item).ToString.Substring(98, 6)) ' Serial Number
                                            LineItemsTable.Add("PROCNO", myCol.Item(Item).ToString.Substring(104, 9)) ' Processing Number
                                        Else
                                            LineItemsTable.Add("FILLER", myCol.Item(Item).ToString.Substring(34, 4)) ' Filler
                                            LineItemsTable.Add("COLLECTIONACCOUNT", myCol.Item(Item).ToString.Substring(38, 20)) 'Collecting Account Details
                                            LineItemsTable.Add("SERIALNUMBER", myCol.Item(Item).ToString.Substring(63, 6)) ' Serial Number
                                            LineItemsTable.Add("PROCNO", myCol.Item(Item).ToString.Substring(69, 9)) ' Processing Number
                                        End If

                                    Case Else
                                        If (ConfigurationManager.AppSettings("Rem") = "1") Then
                                            LineItemsTable.Add("FILLER", myCol.Item(Item).ToString.Substring(34, 4)) ' Filler
                                            LineItemsTable.Add("COLLECTIONACCOUNT", myCol.Item(Item).ToString.Substring(38, 20)) 'Collecting Account Details
                                            LineItemsTable.Add("COLLECTIONACCOUNTNAME", myCol.Item(Item).ToString.Substring(58, 20)) 'Collecting Account Details
                                            LineItemsTable.Add("SERIALNUMBER", myCol.Item(Item).ToString.Substring(98, 6)) ' Serial Number
                                        Else
                                            LineItemsTable.Add("FILLER", myCol.Item(Item).ToString.Substring(34, 4)) ' Filler
                                            LineItemsTable.Add("COLLECTIONACCOUNT", myCol.Item(Item).ToString.Substring(38, 20)) 'Collecting Account Details
                                            LineItemsTable.Add("SERIALNUMBER", myCol.Item(Item).ToString.Substring(63, 6)) ' Serial Number
                                        End If

                                        If enableTruncation = False Then
                                            LineItemsTable.Add("PROCNO", myCol.Item(Item).ToString.Substring(69, 9)) ' Processing Number
                                        Else
                                            'truncation details
                                            Try
                                                If (ConfigurationManager.AppSettings("Rem") = "1") Then
                                                    LineItemsTable.Add("DRN", myCol.Item(Item).ToString.Substring(104, 20)) ' Processing Number
                                                    LineItemsTable.Add("PROCNO", myCol.Item(Item).ToString.Substring(104, 20)) ' Processing Number
                                                Else
                                                    LineItemsTable.Add("DRN", myCol.Item(Item).ToString.Substring(69, 20)) ' Processing Number
                                                    LineItemsTable.Add("PROCNO", myCol.Item(Item).ToString.Substring(69, 20)) ' Processing Number
                                                End If

                                            Catch ex As Exception
                                                'LineItemsTable.Add("DRN", myCol.Item(Item).ToString.Substring(69, 17)) ' Processing Number
                                                'LineItemsTable.Add("PROCNO", myCol.Item(Item).ToString.Substring(69, 17)) ' Processing Number
                                            End Try
                                            'MsgBox("Iko DATA") '----------------
                                            'LineItemsTable.Add("DATA", myCol.Item(Item).ToString()) ' The Whole String as is
                                            Item += 1
                                            signCounter = 1
                                            myFrontSize = myCol.Item(Item)
                                            'MsgBox("Iko FIMAGESIZEBW") '----------------
                                            LineItemsTable.Add("FIMAGESIZEBW", myFrontSize)
                                            Item += 1
                                            ' front black/white image size
                                            LineItemsTable.Add("FIMAGESIGNBW", myCol.Item(Item))
                                            Item += 1
                                            myFrontSize1 = myCol.Item(Item)
                                            LineItemsTable.Add("FIMAGESIZE", myFrontSize1)
                                            Item += 1
                                            LineItemsTable.Add("FIMAGESIGN", myCol.Item(Item))
                                            Item += 1
                                            myRearSize = myCol.Item(Item)
                                            LineItemsTable.Add("BIMAGESIZE", myRearSize)
                                            Item += 1
                                            ' back tiff image size
                                            LineItemsTable.Add("BIMAGESIGN", myCol.Item(Item)) 'myCol.Item(Item).ToString.Substring(197, 48)) ' back tiff image signature

                                            'MsgBox("Iko IMAGES sasa") '----------------
                                            'THE IMAGES ARE STILL IN STRING FORM.
                                            Item += 1
                                            Dim img() As Byte = myCol.Item(Item)
                                            Dim j As Integer = 0
                                            Dim str As String = ""
                                            '-------------- Black And White Front Image --------------
                                            ReDim fImageBW(Val(myFrontSize) - 1)
                                            For i As Integer = 0 To Val(myFrontSize) - 1
                                                fImageBW(i) = img(j)
                                                j = j + 1
                                            Next
                                            str = ""
                                            '-------------- Grayscale Front Image --------------
                                            ReDim fImage(Val(myFrontSize1) - 1)
                                            For i As Integer = 0 To Val(myFrontSize1) - 1
                                                fImage(i) = img(j)
                                                j = j + 1
                                            Next
                                            '-------------- Grayscale Back Image --------------
                                            ReDim bImage(Val(myRearSize) - 1)
                                            For i As Integer = 0 To Val(myRearSize) - 1
                                                bImage(i) = img(j)
                                                j = j + 1
                                            Next
                                        End If
                                End Select
                                'MsgBox("Iko FrontBWImage sasa") '----------------
                                LineItemsTable.Add("FrontBWImage", fImageBW)
                                LineItemsTable.Add("FrontGrayScaleImage", fImage)
                                LineItemsTable.Add("RearImage", bImage)
                                'MsgBox("inataka kuanzaFILENAME sasa" & PathOfTheFile.ToString) '----------------
                                LineItemsTable.Add("FILENAME", PathOfTheFile.Substring(PathOfTheFile.LastIndexOf("\") + 1)) ' The Filename

                                Dim ValidInvalid As Boolean = False
                                Dim ComputedHashFromIncome As Byte() = Nothing
                                'FrontBW
                                ComputedHashFromIncome = HashTheImage(fImageBW)
                                'MsgBox("Iko FIMAGESIGNBW sasa") '----------------
                                ValidInvalid = CompareTwoHashes(LineItemsTable("FIMAGESIGNBW"), ComputedHashFromIncome)
                                If ValidInvalid = True Then ' GrayScale
                                    ComputedHashFromIncome = Nothing
                                    ComputedHashFromIncome = HashTheImage(fImage)
                                    ValidInvalid = CompareTwoHashes(LineItemsTable("FIMAGESIGN"), ComputedHashFromIncome)
                                End If
                                If ValidInvalid = True Then ' GrayScale
                                    ComputedHashFromIncome = Nothing
                                    ComputedHashFromIncome = HashTheImage(bImage)
                                    ValidInvalid = CompareTwoHashes(LineItemsTable("BIMAGESIGN"), ComputedHashFromIncome)
                                End If
                                LineItemsTable.Add("ValidInvalid", ValidInvalid) 'Validity of the image
                                ComputedHashFromIncome = Nothing
                                ValidInvalid = False
                                'MsgBox("Iko IsFCY sasa") '----------------
                                LineItemsTable.Add("IsFCY", CheckIfIsFcy)

                                If dt.Columns.Count <= 0 Then
                                    For Each name As String In LineItemsTable.Keys
                                        Dim ColName As DataColumn = New DataColumn()
                                        ColName.ColumnName = name
                                        ColName.DataType = System.Type.GetType(LineItemsTable(name).GetType().FullName.ToString)
                                        dt.Columns.Add(ColName)
                                    Next
                                End If
                                Dim dr As DataRow = dt.NewRow()
                                For Each name As String In LineItemsTable.Keys
                                    'Dim x As Object = Nothing
                                    'x = LineItemsTable(name).GetType().FullName
                                    'If x = "System.Byte[]" Then
                                    '    Dim value As Byte() = LineItemsTable(name)
                                    '    dr(name) = value
                                    'Else
                                    dr(name) = LineItemsTable(name)
                                    'End If

                                Next
                                dt.Rows.Add(dr)
                                'MsgBox("Iko kusave sasa ") '----------------
                                Modscan.SaveImagesToDB(LineItemsTable, fImageBW, fImage, bImage)
                                'MsgBox("Imaliza kusave sasa ") '----------------
                        End Select
                        System.Windows.Forms.Application.DoEvents()
                        Item = Item + 1
                        LineItemsTable.Clear()
                    End While
                End If
            End If
            MyFile.Close()
            myCol.Clear()
            System.IO.File.Delete(MyFile.Name)
            MyFile.Dispose()
            FileProgress.Value = myCol.Count
        Catch ex As Exception
            LineItemsTable.Clear()
            myCol.Clear()
            MyFile.Close()
            'MsgBox(ex.ToString)
            System.IO.File.Delete(MyFile.Name)
            Return False
        End Try
        Return True
    End Function

    Private Function readImageFile(ByVal fileName As String, ByVal FileToName As String) As Collection
        Dim myColl As New Collection
        'myColSigns.Clear()
        Dim strData As String = ""
        Dim b As BinaryReader = New BinaryReader(File.Open(fileName, FileMode.Open))
        Dim Pos As Int64 = 0
        Dim strStartData As String = ""
        Dim lngFileLength As Long
        Dim strTrailerData As String = ""

        lngFileLength = b.BaseStream.Length
        Try
            Dim intCounter As Long = 0
            Dim strTwoChar As String = ""
            Dim myFrontSize1 As Int64 = 0
            Dim myFrontSize2 As Int64 = 0
            Dim myRearSize As Int64 = 0
            Dim strImageWholeSize As String = ""
            Dim imageSize As Int64 = 0
            Dim myCnt As Int64 = 0
            'Header
            strData = ""
            strData = b.ReadChars(119)
            intCounter = intCounter + 119
            OtherContent(FileToName, strData) '''''''''''''''''''''''''''''''''''''''''''
            'Voucher
            strData = ""
            strImageWholeSize = ""
            strData = b.ReadChars(91)
            intCounter = intCounter + 91
            OtherContent(FileToName, strData) ''''''''''''''''''''''''''''''''''''''''''''''
            'myColl.Add(strData.Trim)
            strTwoChar = b.ReadChars(2)
            OtherContent(FileToName, strTwoChar) ''''''''''''''''''''''''''''''''''''''''''''''''''''''
            intCounter = intCounter + 2
            '---------- Loop Through The file
            Do While intCounter < lngFileLength
                Select Case lngFileLength - intCounter
                    Case Is = 211
                        Exit Do
                    Case Else
                        If (lngFileLength - intCounter) < 211 Then
                            Exit Do
                        End If
                        strData = ""
                        strData = b.ReadChars(2)
                        If strData = "16" Then
                            strImageWholeSize = ""
                            strData = strData & b.ReadChars(89)
                            intCounter = intCounter + 89
                            'myColl.Add(strData.Trim)
                            OtherContent(FileToName, strData) '''''''''''''''''''''''''''''''
                            'strTwoChar = b.ReadChars(2)
                            'intCounter = intCounter + 2
                            Exit Select
                        Else
                            If strData = "19" Then
                                strTrailerData = ""
                                strTrailerData = b.ReadChars(89)
                                Exit Select
                            End If
                            strStartData = strData & b.ReadChars(87)
                            intCounter = intCounter + 87
                            'myColl.Add(strData)
                            'OtherContents(FileToName, strData) ''''''''''''''''''''''''''''''''''''''''''''''''''

                            Dim byteFrontSize1(3) As Byte
                            byteFrontSize1 = b.ReadBytes(4)
                            intCounter = intCounter + 4
                            myFrontSize1 = System.BitConverter.ToInt32(byteFrontSize1, 0)
                            'myColl.Add(myFrontSize1)
                            EJContentPlusImages(byteFrontSize1, FileToName, False, True, strStartData) '''''''''''''''''''''''''''''''''


                            Dim byteSign1(47) As Byte
                            byteSign1 = b.ReadBytes(48)
                            intCounter = intCounter + 48
                            'myColl.Add(byteSign1)
                            EJContentPlusImages(byteSign1, FileToName) '''''''''''''''''''''''''''''''''

                            Dim byteFrontSize2(3) As Byte
                            byteFrontSize2 = b.ReadBytes(4)
                            intCounter = intCounter + 4
                            myFrontSize2 = System.BitConverter.ToInt32(byteFrontSize2, 0)
                            'myColl.Add(myFrontSize2)
                            EJContentPlusImages(byteFrontSize2, FileToName) '''''''''''''''''''''''''''''''''

                            Dim byteSign2(47) As Byte
                            byteSign2 = b.ReadBytes(48)
                            intCounter = intCounter + 48
                            'myColl.Add(byteSign2)
                            EJContentPlusImages(byteSign2, FileToName) '''''''''''''''''''''''''''''''''

                            Dim byteBackSize(3) As Byte
                            byteBackSize = b.ReadBytes(4)
                            intCounter = intCounter + 4
                            myRearSize = System.BitConverter.ToInt32(byteBackSize, 0)
                            EJContentPlusImages(byteBackSize, FileToName) '''''''''''''''''''''''''''''''''

                            'myColl.Add(myRearSize)
                            Dim byteSign3(47) As Byte
                            byteSign3 = b.ReadBytes(48)
                            intCounter = intCounter + 48
                            'myColl.Add(byteSign3)
                            EJContentPlusImages(byteSign3, FileToName) '''''''''''''''''''''''''''''''''

                            Dim myImages(imageSize) As Byte
                            imageSize = myFrontSize1 + myFrontSize2 + myRearSize
                            myImages = b.ReadBytes(imageSize)
                            'myColl.Add(myImages)
                            intCounter = intCounter + imageSize
                            strTwoChar = b.ReadChars(2)
                            intCounter = intCounter + 2
                            EJContentPlusImages(myImages, FileToName, True) '''''''''''''''''''''''''''''''''
                        End If
                End Select
            Loop
            Return myColl
        Catch ex As Exception
            Return myColl
        End Try
    End Function
    Private Shared Function readImageF(ByVal fileName As String, ByVal FileToName As String) As Collection
        Dim myColl As New Collection
        'myColSigns.Clear()
        Dim strData As String = ""
        Dim b As BinaryReader = New BinaryReader(File.Open(fileName, FileMode.Open))
        Dim Pos As Int64 = 0
        Dim strStartData As String = ""
        Dim lngFileLength As Long
        Dim strTrailerData As String = ""
        Dim tempDirectory As String = b.ReadInt32()
        lngFileLength = b.BaseStream.Length
        Try
            Dim intCounter As Long = 0
            Dim strTwoChar As String = ""
            Dim myFrontSize1 As Int64 = 0
            Dim myFrontSize2 As Int64 = 0
            Dim myRearSize As Int64 = 0
            Dim strImageWholeSize As String = ""
            Dim imageSize As Int64 = 0
            Dim myCnt As Int64 = 0
            'Header
            strData = ""
            strData = b.ReadChars(119)
            intCounter = intCounter + 119
            EOtherContent(FileToName, strData) '''''''''''''''''''''''''''''''''''''''''''
            'Voucher
            strData = ""
            strImageWholeSize = ""
            strData = b.ReadChars(91)
            intCounter = intCounter + 91
            EOtherContent(FileToName, strData) ''''''''''''''''''''''''''''''''''''''''''''''
            'myColl.Add(strData.Trim)
            strTwoChar = b.ReadChars(2)
            EOtherContent(FileToName, strTwoChar) ''''''''''''''''''''''''''''''''''''''''''''''''''''''
            intCounter = intCounter + 2
            '---------- Loop Through The file
            Do While intCounter < lngFileLength
                Select Case lngFileLength - intCounter
                    Case Is = 211
                        Exit Do
                    Case Else
                        If (lngFileLength - intCounter) < 211 Then
                            Exit Do
                        End If
                        strData = ""
                        strData = b.ReadChars(2)
                        If strData = "16" Then
                            strImageWholeSize = ""
                            strData = strData & b.ReadChars(89)
                            intCounter = intCounter + 89
                            'myColl.Add(strData.Trim)
                            EOtherContent(FileToName, strData) '''''''''''''''''''''''''''''''
                            'strTwoChar = b.ReadChars(2)
                            'intCounter = intCounter + 2
                            Exit Select
                        Else
                            If strData = "19" Then
                                strTrailerData = ""
                                strTrailerData = b.ReadChars(89)
                                Exit Select
                            End If
                            strStartData = strData & b.ReadChars(87)
                            intCounter = intCounter + 87
                            'myColl.Add(strData)
                            'OtherContents(FileToName, strData) ''''''''''''''''''''''''''''''''''''''''''''''''''

                            Dim byteFrontSize1(3) As Byte
                            byteFrontSize1 = b.ReadBytes(4)
                            intCounter = intCounter + 4
                            myFrontSize1 = System.BitConverter.ToInt32(byteFrontSize1, 0)
                            'myColl.Add(myFrontSize1)
                            ContentPlusImages(byteFrontSize1, FileToName, False, True, strStartData) '''''''''''''''''''''''''''''''''


                            Dim byteSign1(47) As Byte
                            byteSign1 = b.ReadBytes(48)
                            intCounter = intCounter + 48
                            'myColl.Add(byteSign1)
                            ContentPlusImages(byteSign1, FileToName) '''''''''''''''''''''''''''''''''

                            Dim byteFrontSize2(3) As Byte
                            byteFrontSize2 = b.ReadBytes(4)
                            intCounter = intCounter + 4
                            myFrontSize2 = System.BitConverter.ToInt32(byteFrontSize2, 0)
                            'myColl.Add(myFrontSize2)
                            ContentPlusImages(byteFrontSize2, FileToName) '''''''''''''''''''''''''''''''''

                            Dim byteSign2(47) As Byte
                            byteSign2 = b.ReadBytes(48)
                            intCounter = intCounter + 48
                            'myColl.Add(byteSign2)
                            ContentPlusImages(byteSign2, FileToName) '''''''''''''''''''''''''''''''''

                            Dim byteBackSize(3) As Byte
                            byteBackSize = b.ReadBytes(4)
                            intCounter = intCounter + 4
                            myRearSize = System.BitConverter.ToInt32(byteBackSize, 0)
                            ContentPlusImages(byteBackSize, FileToName) '''''''''''''''''''''''''''''''''

                            'myColl.Add(myRearSize)
                            Dim byteSign3(47) As Byte
                            byteSign3 = b.ReadBytes(48)
                            intCounter = intCounter + 48
                            'myColl.Add(byteSign3)
                            ContentPlusImages(byteSign3, FileToName) '''''''''''''''''''''''''''''''''

                            Dim myImages(imageSize) As Byte
                            imageSize = myFrontSize1 + myFrontSize2 + myRearSize
                            myImages = b.ReadBytes(imageSize)
                            'myColl.Add(myImages)
                            intCounter = intCounter + imageSize
                            strTwoChar = b.ReadChars(2)
                            intCounter = intCounter + 2
                            ContentPlusImages(myImages, FileToName, True) '''''''''''''''''''''''''''''''''
                        End If
                End Select
            Loop
            Return myColl
        Catch ex As Exception
            Return myColl
        End Try
    End Function
    Public Shared Function EncryptString(ByVal Message As String, ByVal Passphrase As String) As String
        Dim Results As Byte()
        Dim UTF8 As New System.Text.UTF8Encoding()
        Dim HashProvider As New MD5CryptoServiceProvider()
        Dim TDESKey As Byte() = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase))
        Dim TDESAlgorithm As New TripleDESCryptoServiceProvider()
        TDESAlgorithm.Key = TDESKey
        TDESAlgorithm.Mode = CipherMode.ECB
        TDESAlgorithm.Padding = PaddingMode.PKCS7
        Dim DataToEncrypt As Byte() = UTF8.GetBytes(Message)
        Try
            Dim Encryptor As ICryptoTransform = TDESAlgorithm.CreateEncryptor()
            Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length)
        Finally
            TDESAlgorithm.Clear()
            HashProvider.Clear()
        End Try
        Return Convert.ToBase64String(Results)
    End Function
    Public Shared Function DecryptString(ByVal Message As String, ByVal Passphrase As String) As String
        Dim Results As Byte()
        Dim UTF8 As New System.Text.UTF8Encoding()
        Dim HashProvider As New MD5CryptoServiceProvider()
        Dim TDESKey As Byte() = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase))
        Dim TDESAlgorithm As New TripleDESCryptoServiceProvider()
        TDESAlgorithm.Key = TDESKey
        TDESAlgorithm.Mode = CipherMode.ECB
        TDESAlgorithm.Padding = PaddingMode.PKCS7
        Dim DataToDecrypt As Byte() = Convert.FromBase64String(Message)
        Try
            Dim Decryptor As ICryptoTransform = TDESAlgorithm.CreateDecryptor()
            Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length)
        Finally
            TDESAlgorithm.Clear()
            HashProvider.Clear()
        End Try
        Return UTF8.GetString(Results)
    End Function
    Private Shared Sub EJContentPlusImages(ByVal ByteFile() As Byte, ByVal FilePath As String, Optional ByVal CheckIfItsData As Boolean = False, Optional ByVal HasStartData As Boolean = False, Optional ByVal StartData As String = "")
        Dim myFileStream As FileStream = Nothing
        Dim myEJContentStreamWriter As StreamWriter = Nothing
        If HasStartData = True Then
            Try
                myEJContentStreamWriter = New StreamWriter(FilePath, True)
                myEJContentStreamWriter.Write(EncryptString(StartData, "test1"))
                myEJContentStreamWriter.Write("<~^>")
            Finally
                If Not (myEJContentStreamWriter Is Nothing) Then myEJContentStreamWriter.Close()
            End Try
        End If
        Try
            myFileStream = New FileStream(FilePath, FileMode.Append)
            myFileStream.Write(ByteFile, 0, ByteFile.Length)
        Finally
            If Not (myFileStream Is Nothing) Then myFileStream.Close()
        End Try
        If CheckIfItsData = True Then
            myEJContentStreamWriter = Nothing
            Try
                myEJContentStreamWriter = New StreamWriter(FilePath, True)
                myEJContentStreamWriter.Write("<~^>")
                myEJContentStreamWriter.WriteLine()
            Finally
                If Not (myEJContentStreamWriter Is Nothing) Then myEJContentStreamWriter.Close()
            End Try
        End If
    End Sub
    Private Shared Sub OtherContent(ByVal FilePath As String, ByVal EJOtherContent As String)
        Dim myFileStream As FileStream = Nothing
        Dim myEJContentStreamWriter As StreamWriter = Nothing
        Try
            myEJContentStreamWriter = New StreamWriter(FilePath, True)
            'myEJContentStreamWriter.Write(EncryptString(EJOtherContent, "test1"))
            myEJContentStreamWriter.WriteLine(EJOtherContent)
            myEJContentStreamWriter.Write("<~^>")
        Finally
            If Not (myEJContentStreamWriter Is Nothing) Then myEJContentStreamWriter.Close()
        End Try
    End Sub
    Private Shared Sub ContentPlusImages(ByVal ByteFile() As Byte, ByVal FilePath As String, Optional ByVal CheckIfItsData As Boolean = False, Optional ByVal HasStartData As Boolean = False, Optional ByVal StartData As String = "")
        Dim myFileStream As FileStream = Nothing
        Dim myEJContentStreamWriter As StreamWriter = Nothing
        If HasStartData = True Then
            Try
                myEJContentStreamWriter = New StreamWriter(FilePath, True)
                myEJContentStreamWriter.Write(DecryptString(StartData, "test1"))
            Finally
                If Not (myEJContentStreamWriter Is Nothing) Then myEJContentStreamWriter.Close()
            End Try
        End If
        Try
            myFileStream = New FileStream(FilePath, FileMode.Append)
            myFileStream.Write(ByteFile, 0, ByteFile.Length)
        Finally
            If Not (myFileStream Is Nothing) Then myFileStream.Close()
        End Try
        If CheckIfItsData = True Then
            myEJContentStreamWriter = Nothing
            Try
                myEJContentStreamWriter = New StreamWriter(FilePath, True)
                myEJContentStreamWriter.WriteLine()
            Finally
                If Not (myEJContentStreamWriter Is Nothing) Then myEJContentStreamWriter.Close()
            End Try
        End If
    End Sub
    Private Shared Sub EOtherContent(ByVal FilePath As String, ByVal EJOtherContent As String)
        Dim myFileStream As FileStream = Nothing
        Dim myEJContentStreamWriter As StreamWriter = Nothing
        Try
            myEJContentStreamWriter = New StreamWriter(FilePath, True)
            myEJContentStreamWriter.Write(DecryptString(EJOtherContent, "test1"))
        Finally
            If Not (myEJContentStreamWriter Is Nothing) Then myEJContentStreamWriter.Close()
        End Try
    End Sub
    Public Shared Function SaveImagesToDB(ByVal HashBrOutClearing As Hashtable, ByVal bFTFImage As Byte(), ByVal bFJFImage As Byte(), ByVal bRJImage As Byte(), Optional ByVal UVImage As Byte() = Nothing) As Boolean
        Dim Command As SqlCommand
        Dim FTFImage As String = ""
        Dim FJFImage As String = ""
        Dim RJImage As String = ""
        Dim FUVImage As String = ""
        Dim JRdpi As String
        Dim TFdpi As String
        Dim JFdpi As String
        Dim SystemType As String = ConfigurationManager.AppSettings("sysType")
        Try

            Dim TFImageSignature As String = ""
            Dim JFImageSignature As String = ""
            Dim JRImageSignature As String = ""
            JRdpi = ""
            TFdpi = ""
            JFdpi = ""


            If HashBrOutClearing.Count <> 0 Then
                If bFTFImage Is Nothing Then
                Else
                    Try
                        Dim JFimg As Bitmap = New Bitmap(GetImages(bFJFImage))
                        TFdpi = JFimg.VerticalResolution.ToString
                        TFImageSignature = "" 'IIf(IsDBNull(HashBrOutClearing("FRONTBWIMAGESIGNATURE").ToString), Bytes2String(HashTheImage(bRJImage)), HashBrOutClearing("FRONTBWIMAGESIGNATURE").ToString)
                    Catch ex As Exception
                        'MsgBox("Invalid B\W Image from bank ID: " & HashBrOutClearing("PBANK") & vbCrLf & "Cheque Number: " & HashBrOutClearing("SNO"))
                        JFdpi = "0"
                    End Try

                End If
                If bFJFImage Is Nothing Then
                Else
                    Try
                        Dim TFimg As Bitmap = New Bitmap(GetImages(bFTFImage))
                        TFdpi = TFimg.VerticalResolution.ToString
                        JFImageSignature = "" 'IIf(IsDBNull(HashBrOutClearing("FRONTGRAYSCALEIMAGESIGNATURE").ToString), Bytes2String(HashTheImage(bFTFImage)), HashBrOutClearing("FRONTGRAYSCALEIMAGESIGNATURE").ToString)
                    Catch ex As Exception
                        'MsgBox("Invalid Front Gray Scale Image from bank ID: " & HashBrOutClearing("PBANK") & vbCrLf & "Cheque Number: " & HashBrOutClearing("SNO"))
                        TFdpi = "0"
                    End Try

                End If
                If bRJImage Is Nothing Then
                Else
                    Try
                        Dim JRimg As Bitmap = New Bitmap(GetImages(bRJImage))
                        JRdpi = JRimg.VerticalResolution.ToString
                        JRImageSignature = "" ' IIf(IsDBNull(HashBrOutClearing("REARIMAGESIGNATURE").ToString), Bytes2String(HashTheImage(bRJImage)), HashBrOutClearing("REARIMAGESIGNATURE").ToString)

                    Catch ex As Exception
                        'MsgBox("Invalid Rear Gray Scale Image from bank ID: " & HashBrOutClearing("PBANK") & vbCrLf & "Cheque Number: " & HashBrOutClearing("SNO"))
                        JRdpi = "0"
                    End Try
                End If
                If UVImage Is Nothing Then
                Else
                    FUVImage = Bytes2String(UVImage)
                End If
                If bFTFImage Is Nothing Then
                Else
                    FTFImage = Bytes2String(bFTFImage)
                End If
                If bFJFImage Is Nothing Then
                Else
                    FJFImage = Bytes2String(bFJFImage)
                End If
                If bRJImage Is Nothing Then
                Else
                    RJImage = Bytes2String(bRJImage)
                End If

                Select Case SystemType.ToUpper.Trim
                    Case "BR", "BRMFO"
                        Dim Country As String = ConfigurationManager.AppSettings("CountryCode").Trim().ToUpper()
                        SqlConn = GetConnectionSQL()
                        Command = New SqlCommand("sp_InwardsCheques", SqlConn)
                        Command.CommandType = CommandType.StoredProcedure
                        Command.Parameters.Add("@OurBranchID", SqlDbType.NVarChar).Value = IIf(IsDBNull(HashBrOutClearing("TOBRANCH").ToString), OurBranchID.ToString(), HashBrOutClearing("TOBRANCH"))
                        Command.Parameters.Add("@TheirAccountID", SqlDbType.NVarChar).Value = HashBrOutClearing("COLLECTIONACCOUNT")
                        Command.Parameters.Add("@ChequeID", SqlDbType.NVarChar).Value = HashBrOutClearing("SERIALNUMBER")
                        Command.Parameters.Add("@Amount", SqlDbType.Money).Value = IIf(IsDBNull(HashBrOutClearing("AMOUNT").ToString), 0, HashBrOutClearing("AMOUNT"))
                        'HashBrOutClearing("RCODE") & HashBrOutClearing("VTYPE") & HashBrOutClearing("AMOUNT") & "1" & _
                        '                                                            HashBrOutClearing("FILENAME").ToString.Substring(0, 2) & _
                        '                                                            HashBrOutClearing("DESTBRANCH") & HashBrOutClearing("DESTACC") & HashBrOutClearing("CHQDGT") & _
                        '                                                            HashBrOutClearing("CURRENCYCODE") & "00" & HashBrOutClearing("COLLACC") & _
                        '                                                            HashBrOutClearing("PBANK") & HashBrOutClearing("PBRANCH") & HashBrOutClearing("SNO")
                        Command.Parameters.Add("@Data", SqlDbType.NVarChar).Value = HashBrOutClearing("DATA") ' The Data
                        Command.Parameters.Add("@BankID", SqlDbType.NVarChar).Value = HashBrOutClearing("FROMBANK")
                        Command.Parameters.Add("@BranchID", SqlDbType.NVarChar).Value = HashBrOutClearing("FROMBRANCH")
                        Command.Parameters.Add("@Date", SqlDbType.DateTime).Value = cWorkingDate
                        Command.Parameters.Add("@ReturnCode", SqlDbType.NVarChar).Value = IIf(IsDBNull(HashBrOutClearing("RETURNCODE").ToString), "00", HashBrOutClearing("RETURNCODE"))
                        Command.Parameters.Add("@ChequeDigit", SqlDbType.SmallInt).Value = HashBrOutClearing("CHEQUEDIGIT")
                        Command.Parameters.Add("@CollAccName", SqlDbType.NVarChar).Value = HashBrOutClearing("COLLECTIONACCOUNTNAME")
                        Command.Parameters.Add("@VoucherCode", SqlDbType.NVarChar).Value = HashBrOutClearing("VOUCHERTYPE")
                        Command.Parameters.Add("@ImageUniqueID", SqlDbType.NVarChar).Value = HashBrOutClearing("DRN")
                        Command.Parameters.Add("@TFImageSize", SqlDbType.NVarChar).Value = 0 'IIf(IsDBNull(HashBrOutClearing("FRONTBWIMAGESIZE").ToString), 0, HashBrOutClearing("FIMAGESIZEBW").ToString)
                        Command.Parameters.Add("@JFImageSize", SqlDbType.NVarChar).Value = 0 'IIf(IsDBNull(HashBrOutClearing("FRONTGRAYSCALEIMAGESIZE").ToString), 0, HashBrOutClearing("FIMAGESIZE").ToString)
                        Command.Parameters.Add("@JRImageSize", SqlDbType.NVarChar).Value = 0 'IIf(IsDBNull(HashBrOutClearing("REARIMAGESIZE").ToString), 0, HashBrOutClearing("BIMAGESIZE").ToString)
                        Command.Parameters.Add("@AccountID", SqlDbType.NVarChar).Value = HashBrOutClearing("TOACCOUNT")
                        Command.Parameters.Add("@Filename", SqlDbType.NVarChar).Value = HashBrOutClearing("FILENAME")
                        Command.Parameters.Add("@Validity", SqlDbType.NVarChar).Value = HashBrOutClearing("ValidInvalid")
                        Command.Parameters.Add("@OurBankID", SqlDbType.NVarChar).Value = Modscan.OurBankID
                        Command.Parameters.Add("@JFdpi", SqlDbType.NVarChar).Value = 96 'JFdpi
                        Command.Parameters.Add("@TFdpi", SqlDbType.NVarChar).Value = 96 'TFdpi
                        Command.Parameters.Add("@JRdpi", SqlDbType.NVarChar).Value = 96 'JRdpi
                        Command.Parameters.Add("@TFImageSignature", SqlDbType.NVarChar).Value = TFImageSignature
                        Command.Parameters.Add("@JFImageSignature", SqlDbType.NVarChar).Value = JFImageSignature
                        Command.Parameters.Add("@JRImageSignature", SqlDbType.NVarChar).Value = JRImageSignature
                        If IIf(IsDBNull(HashBrOutClearing("RETURNCODE").ToString), "00", HashBrOutClearing("RETURNCODE")) <> "00" And Country.ToUpper() = "TZ" Then
                            Command.Parameters.Add("@FrontImage", SqlDbType.NVarChar).Value = Nothing ' FJFImage
                            Command.Parameters.Add("@FrontTFImage", SqlDbType.NVarChar).Value = Nothing 'FTFImage
                            Command.Parameters.Add("@BackImage", SqlDbType.NVarChar).Value = Nothing 'RJImage
                            If Not UVImage Is Nothing Then
                                Command.Parameters.Add("@UVImage", SqlDbType.NVarChar).Value = Nothing 'FUVImage
                            End If
                        Else
                            Command.Parameters.Add("@FrontImage", SqlDbType.NVarChar).Value = FJFImage
                            Command.Parameters.Add("@FrontTFImage", SqlDbType.NVarChar).Value = FTFImage
                            Command.Parameters.Add("@BackImage", SqlDbType.NVarChar).Value = RJImage
                            If Not UVImage Is Nothing Then
                                Command.Parameters.Add("@UVImage", SqlDbType.NVarChar).Value = FUVImage
                            End If
                        End If
                        If (Country.ToUpper() = "UG") Then
                            Command.Parameters.Add("@ExtraDetails", SqlDbType.NVarChar).Value = HashBrOutClearing("DRN").ToString()
                            Command.Parameters.Add("@DAdrLine", SqlDbType.NVarChar).Value = HashBrOutClearing("DAdrLine").ToString()
                            Command.Parameters.Add("@DTwnNm", SqlDbType.NVarChar).Value = HashBrOutClearing("DTwnNm").ToString()
                            Command.Parameters.Add("@DCtry", SqlDbType.NVarChar).Value = HashBrOutClearing("DCtry").ToString()
                            Command.Parameters.Add("@DNm", SqlDbType.NVarChar).Value = HashBrOutClearing("DNm").ToString()
                            Command.Parameters.Add("@DPhneNb", SqlDbType.NVarChar).Value = HashBrOutClearing("DPhneNb").ToString()
                            Command.Parameters.Add("@DMobNb", SqlDbType.NVarChar).Value = HashBrOutClearing("DMobNb").ToString()
                            Command.Parameters.Add("@DEmailAdr", SqlDbType.NVarChar).Value = HashBrOutClearing("DEmailAdr").ToString()
                            Command.Parameters.Add("@DOthr", SqlDbType.NVarChar).Value = HashBrOutClearing("DOthr").ToString()
                            Command.Parameters.Add("@DbtrAcct", SqlDbType.NVarChar).Value = HashBrOutClearing("DbtrAcct").ToString()
                            Command.Parameters.Add("@CAdrLine", SqlDbType.NVarChar).Value = HashBrOutClearing("CAdrLine").ToString()
                            Command.Parameters.Add("@CTwnNm", SqlDbType.NVarChar).Value = HashBrOutClearing("CTwnNm").ToString()
                            Command.Parameters.Add("@CCtry", SqlDbType.NVarChar).Value = HashBrOutClearing("CCtry").ToString()
                            Command.Parameters.Add("@CNm", SqlDbType.NVarChar).Value = HashBrOutClearing("CNm").ToString()
                            Command.Parameters.Add("@CPhneNb", SqlDbType.NVarChar).Value = HashBrOutClearing("CPhneNb").ToString()
                            Command.Parameters.Add("@CMobNb", SqlDbType.NVarChar).Value = HashBrOutClearing("CMobNb").ToString()
                            Command.Parameters.Add("@CEmailAdr", SqlDbType.NVarChar).Value = HashBrOutClearing("CEmailAdr").ToString()
                            Command.Parameters.Add("@COthr", SqlDbType.NVarChar).Value = HashBrOutClearing("COthr").ToString()
                            Command.Parameters.Add("@PymType", SqlDbType.NVarChar).Value = HashBrOutClearing("PymType").ToString()
                            Command.Parameters.Add("@CdtrAcct", SqlDbType.NVarChar).Value = HashBrOutClearing("CdtrAcct").ToString()
                            Command.Parameters.Add("@OrgnlInstrID", SqlDbType.NVarChar).Value = HashBrOutClearing("OrgnlInstrID").ToString()
                            Command.Parameters.Add("@UstrdColD", SqlDbType.NVarChar).Value = HashBrOutClearing("UstrdColD").ToString()
                            Command.Parameters.Add("@UstrdBWF", SqlDbType.NVarChar).Value = HashBrOutClearing("UstrdBWF").ToString()
                            Command.Parameters.Add("@UstrdBWR", SqlDbType.NVarChar).Value = HashBrOutClearing("UstrdBWR").ToString()
                            Command.Parameters.Add("@UstrdGS", SqlDbType.NVarChar).Value = HashBrOutClearing("UstrdGS").ToString()
                            Command.Parameters.Add("@UstrdUV", SqlDbType.NVarChar).Value = HashBrOutClearing("UstrdUV").ToString()
                            Command.Parameters.Add("@UstrdMicr", SqlDbType.NVarChar).Value = HashBrOutClearing("UstrdMicr").ToString()
                            Command.Parameters.Add("@OrgnlEndToEnd", SqlDbType.NVarChar).Value = HashBrOutClearing("OrgnlEndToEnd").ToString()
                        End If
                        If (Country.ToUpper() = "TZ" Or Country.ToUpper() = "UG") Then
                            Command.Parameters.Add("@TrxID", SqlDbType.NVarChar).Value = HashBrOutClearing("TrxID").ToString()
                            Command.Parameters.Add("@MsgID", SqlDbType.NVarChar).Value = HashBrOutClearing("MsgID")
                            Command.Parameters.Add("@Drawer", SqlDbType.NVarChar).Value = HashBrOutClearing("DRAWERORPAYEE")
                        End If
                        Command.ExecuteNonQuery()
                    Case "BRNET"
                        Dim Country As String = ConfigurationManager.AppSettings("CountryCode").Trim().ToUpper()
                        SqlConn = GetConnectionSQL()
                        If Country.ToUpper() = "TZ" Then
                            Command = New SqlCommand("p_AddInwardsTZ", SqlConn)
                        ElseIf Country.ToUpper() = "UG" Then
                            Command = New SqlCommand("p_AddInwardsUG", SqlConn)
                        ElseIf Country.ToUpper() = "ET" Then
                            Command = New SqlCommand("p_AddInwardsET", SqlConn)
                        Else
                            Command = New SqlCommand("sp_InwardsCheques", SqlConn)
                        End If

                        Command.CommandType = CommandType.StoredProcedure
                        If (Country.ToUpper() = "TZ" Or Country.ToUpper() = "UG") Or Country.ToUpper() = "ET" Then
                            Command.Parameters.Add("@ColumnID", SqlDbType.SmallInt).Value = 0
                            Command.Parameters.Add("@IsMDV", SqlDbType.Bit).Value = False
                            Command.Parameters.Add("@TrxType", SqlDbType.NVarChar).Value = "ID" 'dr("TrxTypeID").ToString()
                            Command.Parameters.Add("@TrxID", SqlDbType.NVarChar).Value = HashBrOutClearing("TrxID").ToString()
                        End If
                        If (Country.ToUpper() = "UG") Or (Country.ToUpper() = "TZ") Or (Country.ToUpper() = "ET") Then
                            If (Country.ToUpper() <> "TZ") AndAlso (Country.ToUpper() <> "ET") Then
                                Command.Parameters.Add("@ExtraDetails", SqlDbType.NVarChar).Value = HashBrOutClearing("DRN").ToString()
                            Else
                                Command.Parameters.Add("@ExtraDetails", SqlDbType.NVarChar).Value = ""
                                If HashBrOutClearing("RCODE").ToString = "00" Then
                                    Try
                                        If Not (IsDBNull(HashBrOutClearing("LclInstrm").ToString)) Then
                                            Command.Parameters.Add("@LclInstrm", SqlDbType.NVarChar).Value = ""
                                        Else
                                            Command.Parameters.Add("@LclInstrm", SqlDbType.NVarChar).Value = IIf(IsDBNull(HashBrOutClearing("LclInstrm").ToString), "", HashBrOutClearing("LclInstrm"))
                                        End If
                                    Catch ex As Exception
                                        Command.Parameters.Add("@LclInstrm", SqlDbType.NVarChar).Value = ""
                                    End Try
                                    Try
                                        If Not (IsDBNull(HashBrOutClearing("SvcLvl").ToString)) Then
                                            If Not (IsDBNull(HashBrOutClearing("PymType").ToString)) Then
                                                Command.Parameters.Add("@SvcLvl", SqlDbType.NVarChar).Value = HashBrOutClearing("PymType")
                                            Else
                                                Command.Parameters.Add("@SvcLvl", SqlDbType.NVarChar).Value = ""
                                            End If
                                        Else
                                            Command.Parameters.Add("@SvcLvl", SqlDbType.NVarChar).Value = IIf(IsDBNull(HashBrOutClearing("SvcLvl").ToString), HashBrOutClearing("PymType"), HashBrOutClearing("SvcLvl"))
                                        End If
                                    Catch ex As Exception
                                        Command.Parameters.Add("@SvcLvl", SqlDbType.NVarChar).Value = ""
                                    End Try
                                    Try
                                        If Not (IsDBNull(HashBrOutClearing("CtgyPurp").ToString)) Then
                                            Command.Parameters.Add("@CtgyPurp", SqlDbType.NVarChar).Value = ""
                                        Else
                                            Command.Parameters.Add("@CtgyPurp", SqlDbType.NVarChar).Value = IIf(IsDBNull(HashBrOutClearing("LclInstrm").ToString), "", HashBrOutClearing("LclInstrm"))
                                        End If
                                    Catch ex As Exception
                                        Command.Parameters.Add("@CtgyPurp", SqlDbType.NVarChar).Value = ""
                                    End Try
                                End If
                            End If
                            Command.Parameters.Add("@DAdrLine", SqlDbType.NVarChar).Value = HashBrOutClearing("DAdrLine").ToString()
                            Command.Parameters.Add("@DTwnNm", SqlDbType.NVarChar).Value = HashBrOutClearing("DTwnNm").ToString()
                            Command.Parameters.Add("@DCtry", SqlDbType.NVarChar).Value = HashBrOutClearing("DCtry").ToString()
                            Command.Parameters.Add("@DNm", SqlDbType.NVarChar).Value = HashBrOutClearing("DNm").ToString()
                            Command.Parameters.Add("@DPhneNb", SqlDbType.NVarChar).Value = HashBrOutClearing("DPhneNb").ToString()
                            Command.Parameters.Add("@DMobNb", SqlDbType.NVarChar).Value = HashBrOutClearing("DMobNb").ToString()
                            Command.Parameters.Add("@DEmailAdr", SqlDbType.NVarChar).Value = HashBrOutClearing("DEmailAdr").ToString()
                            Command.Parameters.Add("@DOthr", SqlDbType.NVarChar).Value = HashBrOutClearing("DOthr").ToString()
                            Command.Parameters.Add("@DbtrAcct", SqlDbType.NVarChar).Value = HashBrOutClearing("DbtrAcct").ToString()
                            Command.Parameters.Add("@CAdrLine", SqlDbType.NVarChar).Value = HashBrOutClearing("CAdrLine").ToString()
                            Command.Parameters.Add("@CTwnNm", SqlDbType.NVarChar).Value = HashBrOutClearing("CTwnNm").ToString()
                            Command.Parameters.Add("@CCtry", SqlDbType.NVarChar).Value = HashBrOutClearing("CCtry").ToString()
                            Command.Parameters.Add("@CNm", SqlDbType.NVarChar).Value = HashBrOutClearing("CNm").ToString()
                            Command.Parameters.Add("@CPhneNb", SqlDbType.NVarChar).Value = HashBrOutClearing("CPhneNb").ToString()
                            Command.Parameters.Add("@CMobNb", SqlDbType.NVarChar).Value = HashBrOutClearing("CMobNb").ToString()
                            Command.Parameters.Add("@CEmailAdr", SqlDbType.NVarChar).Value = HashBrOutClearing("CEmailAdr").ToString()
                            Command.Parameters.Add("@COthr", SqlDbType.NVarChar).Value = HashBrOutClearing("COthr").ToString()
                            Command.Parameters.Add("@PymType", SqlDbType.NVarChar).Value = HashBrOutClearing("PymType").ToString()
                            Command.Parameters.Add("@CdtrAcct", SqlDbType.NVarChar).Value = HashBrOutClearing("CdtrAcct").ToString()
                            Command.Parameters.Add("@OrgnlInstrID", SqlDbType.NVarChar).Value = HashBrOutClearing("OrgnlInstrID").ToString()
                            Command.Parameters.Add("@UstrdColD", SqlDbType.NVarChar).Value = HashBrOutClearing("UstrdColD").ToString()
                            Command.Parameters.Add("@OrgnlEndToEnd", SqlDbType.NVarChar).Value = HashBrOutClearing("OrgnlEndToEnd").ToString()
                            Command.Parameters.Add("@CurrencyID", SqlDbType.NVarChar).Value = HashBrOutClearing("CURRENCYCODE").ToString()
                            Command.Parameters.Add("@CCNm", SqlDbType.NVarChar).Value = HashBrOutClearing("CCNm").ToString()
                            Command.Parameters.Add("@DCNm", SqlDbType.NVarChar).Value = HashBrOutClearing("DCNm").ToString()
                            Try
                                If Not (IsDBNull(HashBrOutClearing("ReqdColltnDt").ToString)) Then
                                    Command.Parameters.Add("@ReqdColltnDt", SqlDbType.NVarChar).Value = ""
                                Else
                                    Command.Parameters.Add("@ReqdColltnDt", SqlDbType.NVarChar).Value = HashBrOutClearing("ReqdColltnDt").ToString()
                                End If
                            Catch ex As Exception
                                Command.Parameters.Add("@ReqdColltnDt", SqlDbType.NVarChar).Value = ""
                            End Try
                        End If

                        If (Country.ToUpper() <> "TZ" AndAlso Country.ToUpper() <> "ET") Then
                            Command.Parameters.Add("@OurBranchID", SqlDbType.NVarChar).Value = IIf(IsDBNull(HashBrOutClearing("DESTBRANCH").ToString), OurBranchID.ToString(), HashBrOutClearing("DESTBRANCH"))
                            Command.Parameters.Add("@BankID", SqlDbType.NVarChar).Value = HashBrOutClearing("PBANK")
                            Command.Parameters.Add("@BranchID", SqlDbType.NVarChar).Value = HashBrOutClearing("PBRANCH")
                            Command.Parameters.Add("@ImageUniqueID", SqlDbType.NVarChar).Value = HashBrOutClearing("DRN")
                            Command.Parameters.Add("@Drawer", SqlDbType.NVarChar).Value = HashBrOutClearing("DRAWERORPAYEE")
                            Command.Parameters.Add("@RemittanceInfo", SqlDbType.NVarChar).Value = HashBrOutClearing("RemittanceInfo").ToString()
                        Else
                            If HashBrOutClearing("RCODE").ToString = "00" Then
                                Command.Parameters.Add("@OurBranchID", SqlDbType.NVarChar).Value = IIf(IsDBNull(HashBrOutClearing("DESTBRANCH").ToString), OurBranchID.ToString(), HashBrOutClearing("DESTBRANCH"))
                                Command.Parameters.Add("@BankID", SqlDbType.NVarChar).Value = HashBrOutClearing("PBANK")
                                Command.Parameters.Add("@BranchID", SqlDbType.NVarChar).Value = HashBrOutClearing("PBRANCH")
                                Command.Parameters.Add("@ImageUniqueID", SqlDbType.NVarChar).Value = HashBrOutClearing("DRN")
                                Command.Parameters.Add("@Drawer", SqlDbType.NVarChar).Value = HashBrOutClearing("DRAWERORPAYEE")
                            Else
                                Command.Parameters.Add("@OurBranchID", SqlDbType.NVarChar).Value = ""
                                Command.Parameters.Add("@BankID", SqlDbType.NVarChar).Value = HashBrOutClearing("PBANK")
                                Command.Parameters.Add("@BranchID", SqlDbType.NVarChar).Value = ""
                            End If

                        End If
                        Command.Parameters.Add("@TheirAccountID", SqlDbType.NVarChar).Value = HashBrOutClearing("COLLACC")
                        Command.Parameters.Add("@ChequeID", SqlDbType.NVarChar).Value = HashBrOutClearing("SNO")
                        Command.Parameters.Add("@Amount", SqlDbType.Money).Value = IIf(IsDBNull(HashBrOutClearing("AMOUNT").ToString), 0, HashBrOutClearing("AMOUNT"))
                        Command.Parameters.Add("@Data", SqlDbType.NVarChar).Value = HashBrOutClearing("DATA") ' The Data

                        Command.Parameters.Add("@Date", SqlDbType.DateTime).Value = cWorkingDate
                        Command.Parameters.Add("@ReturnCode", SqlDbType.NVarChar).Value = IIf(IsDBNull(HashBrOutClearing("RCODE").ToString), "00", HashBrOutClearing("RCODE"))
                        Command.Parameters.Add("@ChequeDigit", SqlDbType.SmallInt).Value = HashBrOutClearing("CHQDGT")

                        Command.Parameters.Add("@VoucherCode", SqlDbType.NVarChar).Value = HashBrOutClearing("VTYPE")

                        Command.Parameters.Add("@TFImageSize", SqlDbType.NVarChar).Value = IIf(IsDBNull(HashBrOutClearing("FIMAGESIZEBW").ToString), 0, HashBrOutClearing("FIMAGESIZEBW").ToString)
                        Command.Parameters.Add("@JFImageSize", SqlDbType.NVarChar).Value = IIf(IsDBNull(HashBrOutClearing("FIMAGESIZE").ToString), 0, HashBrOutClearing("FIMAGESIZE").ToString)
                        Command.Parameters.Add("@JRImageSize", SqlDbType.NVarChar).Value = IIf(IsDBNull(HashBrOutClearing("BIMAGESIZE").ToString), 0, HashBrOutClearing("BIMAGESIZE").ToString)
                        Command.Parameters.Add("@AccountID", SqlDbType.NVarChar).Value = HashBrOutClearing("DESTACC")
                        Command.Parameters.Add("@Filename", SqlDbType.NVarChar).Value = HashBrOutClearing("FILENAME")
                        Command.Parameters.Add("@Validity", SqlDbType.NVarChar).Value = HashBrOutClearing("ValidInvalid")
                        Command.Parameters.Add("@OurBankID", SqlDbType.NVarChar).Value = HashBrOutClearing("DESTBANK")
                        Command.Parameters.Add("@JFdpi", SqlDbType.NVarChar).Value = JFdpi
                        Command.Parameters.Add("@TFdpi", SqlDbType.NVarChar).Value = TFdpi
                        Command.Parameters.Add("@JRdpi", SqlDbType.NVarChar).Value = JRdpi
                        Command.Parameters.Add("@TFImageSignature", SqlDbType.NVarChar).Value = TFImageSignature
                        Command.Parameters.Add("@JFImageSignature", SqlDbType.NVarChar).Value = JFImageSignature
                        Command.Parameters.Add("@JRImageSignature", SqlDbType.NVarChar).Value = JRImageSignature
                        If IIf(IsDBNull(HashBrOutClearing("RCODE").ToString), "00", HashBrOutClearing("RCODE")) <> "00" Then
                            Command.Parameters.Add("@FrontImage", SqlDbType.NVarChar).Value = Nothing ' FJFImage
                            Command.Parameters.Add("@FrontTFImage", SqlDbType.NVarChar).Value = Nothing 'FTFImage
                            Command.Parameters.Add("@BackImage", SqlDbType.NVarChar).Value = Nothing 'RJImage
                            If Not UVImage Is Nothing Then
                                Command.Parameters.Add("@UVImage", SqlDbType.NVarChar).Value = Nothing 'FUVImage
                            End If
                        Else
                            Command.Parameters.Add("@FrontImage", SqlDbType.NVarChar).Value = FJFImage
                            Command.Parameters.Add("@FrontTFImage", SqlDbType.NVarChar).Value = FTFImage
                            Command.Parameters.Add("@BackImage", SqlDbType.NVarChar).Value = RJImage
                            If Not UVImage Is Nothing Then
                                Command.Parameters.Add("@UVImage", SqlDbType.NVarChar).Value = FUVImage
                            End If
                        End If
                        If IIf(IsDBNull(HashBrOutClearing("RCODE").ToString), "00", HashBrOutClearing("RCODE")) = "00" Then
                            Command.Parameters.Add("@UstrdBWF", SqlDbType.NVarChar).Value = HashBrOutClearing("UstrdBWF").ToString()
                            Command.Parameters.Add("@UstrdBWR", SqlDbType.NVarChar).Value = HashBrOutClearing("UstrdBWR").ToString()
                            Command.Parameters.Add("@UstrdGS", SqlDbType.NVarChar).Value = HashBrOutClearing("UstrdGS").ToString()
                            Command.Parameters.Add("@UstrdUV", SqlDbType.NVarChar).Value = HashBrOutClearing("UstrdUV").ToString()
                            Command.Parameters.Add("@UstrdMicr", SqlDbType.NVarChar).Value = HashBrOutClearing("UstrdMicr").ToString()
                        Else
                            Command.Parameters.Add("@UstrdBWF", SqlDbType.NVarChar).Value = ""
                            Command.Parameters.Add("@UstrdBWR", SqlDbType.NVarChar).Value = ""
                            Command.Parameters.Add("@UstrdGS", SqlDbType.NVarChar).Value = ""
                            Command.Parameters.Add("@UstrdUV", SqlDbType.NVarChar).Value = ""
                            Command.Parameters.Add("@UstrdMicr", SqlDbType.NVarChar).Value = ""
                        End If

                        Command.Parameters.Add("@MsgID", SqlDbType.NVarChar).Value = HashBrOutClearing("MsgID")
                        Command.ExecuteNonQuery()
                End Select
                Modscan.Wait(1)
            End If
            Return True
        Catch ex As Exception
            'MsgBox(ex.Message)
            Return False
        End Try
    End Function
    Public Function SaveDetailsToDB(ByVal HashBrOutClearing As Hashtable) As Boolean
        Dim Command As SqlCommand
        Dim x As New System.Random
        Try
            If HashBrOutClearing.Count <> 0 Then
                Command = New SqlCommand("sp_InwardsCheques", SqlConn)
                Command.CommandType = CommandType.StoredProcedure
                Command.Parameters.Add("@ColumnID", SqlDbType.Int).Value = x.Next(99999)
                Command.Parameters.Add("@OurBranchID", SqlDbType.NVarChar).Value = HashBrOutClearing("DESTBRANCH")
                Command.Parameters.Add("@OurBankID", SqlDbType.NVarChar).Value = HashBrOutClearing("FILENAME").ToString.Substring(0, 2)
                Command.Parameters.Add("@Account", SqlDbType.NVarChar).Value = HashBrOutClearing("DESTACC")
                Command.Parameters.Add("@ChequeID", SqlDbType.NVarChar).Value = HashBrOutClearing("SNO")
                Command.Parameters.Add("@ChequeDate", SqlDbType.DateTime).Value = Today.Date
                Command.Parameters.Add("@BranchID", SqlDbType.NVarChar).Value = HashBrOutClearing("PBRANCH")
                Command.Parameters.Add("@Amount", SqlDbType.Money).Value = IIf(IsDBNull(HashBrOutClearing("AMOUNT").ToString), 0, HashBrOutClearing("AMOUNT"))
                Command.Parameters.Add("@BankID", SqlDbType.NVarChar).Value = HashBrOutClearing("PBANK")
                Command.Parameters.Add("@RecordType", SqlDbType.NVarChar).Value = HashBrOutClearing("RCODE")
                Command.Parameters.Add("@Data", SqlDbType.NVarChar).Value = ""
                Command.Parameters.Add("@ChequeDigit", SqlDbType.SmallInt).Value = HashBrOutClearing("CHQDGT")
                Command.Parameters.Add("@VoucherCode", SqlDbType.NVarChar).Value = HashBrOutClearing("VTYPE")
                Command.Parameters.Add("@Filename", SqlDbType.NVarChar).Value = HashBrOutClearing("FILENAME")
                Command.Parameters.Add("@Validity", SqlDbType.NVarChar).Value = HashBrOutClearing("ValidInvalid")
                Command.Parameters.Add("@TransactionColumnID", SqlDbType.NVarChar).Value = 0
                Command.Parameters.Add("@ImageUniqueID", SqlDbType.NVarChar).Value = HashBrOutClearing("DRN")
                Command.Parameters.Add("@TFImageSize", SqlDbType.NVarChar).Value = IIf(IsDBNull(HashBrOutClearing("FIMAGESIZEBW").ToString), 0, HashBrOutClearing("FIMAGESIZEBW").ToString)
                Command.Parameters.Add("@JFImageSize", SqlDbType.NVarChar).Value = IIf(IsDBNull(HashBrOutClearing("FIMAGESIZE").ToString), 0, HashBrOutClearing("FIMAGESIZE").ToString)
                Command.Parameters.Add("@JRImageSize", SqlDbType.NVarChar).Value = IIf(IsDBNull(HashBrOutClearing("BIMAGESIZE").ToString), 0, HashBrOutClearing("BIMAGESIZE").ToString)
                OpenConnections()
                Command.ExecuteNonQuery()
            End If
            Return True
        Catch ex As Exception
            MsgBox(ex.Message)
            Return False
        End Try
    End Function
    Private Shared Function readImageFile(ByVal fileName As String, Optional ByVal isCheque As Boolean = True) As Collection
        Dim myColl As New Collection
        myColSigns.Clear()
        Dim strData As String = ""
        Dim strTrailerData As String = ""
        Dim b As BinaryReader = New BinaryReader(File.Open(fileName, FileMode.Open))
        Dim Pos As Int64 = 0
        Dim lngFileLength As Long

        lngFileLength = b.BaseStream.Length
        Try
            Dim intCounter As Long = 0
            Dim strTwoChar As String = ""
            Dim myFrontSize1 As Int64 = 0
            Dim myFrontSize2 As Int64 = 0
            Dim myRearSize As Int64 = 0
            Dim strImageWholeSize As String = ""
            Dim imageSize As Int64 = 0
            Dim myCnt As Int64 = 0
            'Header
            strData = ""
            If (ConfigurationManager.AppSettings("Rem") = "1") Then
                strData = b.ReadChars(119)
                intCounter = intCounter + 119
            Else
                strData = b.ReadChars(119)
                intCounter = intCounter + 119
            End If


            'Voucher
            strData = ""
            strImageWholeSize = ""

            If (ConfigurationManager.AppSettings("Rem") = "1") Then
                strData = b.ReadChars(91)
                intCounter = intCounter + 91
            Else
                strData = b.ReadChars(91)
                intCounter = intCounter + 91
            End If

            myColl.Add(strData.Trim)
            strTwoChar = b.ReadChars(2)
            intCounter = intCounter + 2
            '---------- Loop Through The file
            Do While intCounter < lngFileLength
                Select Case lngFileLength - intCounter
                    Case Is = 211
                        Exit Do
                    Case Else
                        If (lngFileLength - intCounter) < 211 Then
                            Exit Do
                        End If
                        strData = ""
                        strData = b.ReadChars(2)
                        If strData = "16" Then
                            strImageWholeSize = ""
                            strData = strData & b.ReadChars(89)
                            intCounter = intCounter + 89
                            myColl.Add(strData.Trim)
                            'strTwoChar = b.ReadChars(2)
                            'intCounter = intCounter + 2
                            Exit Select
                        Else
                            If strData = "19" Then
                                Exit Do
                            End If
                            If (ConfigurationManager.AppSettings("Rem") = "1") Then
                                strData = strData & b.ReadChars(122)
                                intCounter = intCounter + 122
                            Else
                                strData = strData & b.ReadChars(87)
                                intCounter = intCounter + 87
                            End If
                            myColl.Add(strData)

                            Dim byteFrontSize1(3) As Byte
                            byteFrontSize1 = b.ReadBytes(4)
                            intCounter = intCounter + 4
                            myFrontSize1 = System.BitConverter.ToInt32(byteFrontSize1, 0)
                            myColl.Add(myFrontSize1)
                            Dim byteSign1(47) As Byte
                            byteSign1 = b.ReadBytes(48)
                            intCounter = intCounter + 48
                            myColl.Add(byteSign1)

                            Dim byteFrontSize2(3) As Byte
                            byteFrontSize2 = b.ReadBytes(4)
                            intCounter = intCounter + 4
                            myFrontSize2 = System.BitConverter.ToInt32(byteFrontSize2, 0)
                            myColl.Add(myFrontSize2)
                            Dim byteSign2(47) As Byte
                            byteSign2 = b.ReadBytes(48)
                            intCounter = intCounter + 48
                            myColl.Add(byteSign2)

                            Dim byteBackSize(3) As Byte
                            byteBackSize = b.ReadBytes(4)
                            intCounter = intCounter + 4
                            myRearSize = System.BitConverter.ToInt32(byteBackSize, 0)
                            myColl.Add(myRearSize)
                            Dim byteSign3(47) As Byte
                            byteSign3 = b.ReadBytes(48)
                            intCounter = intCounter + 48
                            myColl.Add(byteSign3)

                            Dim myImages(imageSize) As Byte
                            imageSize = myFrontSize1 + myFrontSize2 + myRearSize
                            myImages = b.ReadBytes(imageSize)
                            myColl.Add(myImages)
                            intCounter = intCounter + imageSize
                            strTwoChar = b.ReadChars(2)
                            intCounter = intCounter + 2
                        End If
                End Select
            Loop
            Return myColl
        Catch ex As Exception
            Return myColl
        End Try
    End Function
    Public Shared Function GetImages(ByVal PicBytes As Byte()) As Image
        Dim ChequeImage As Image = Nothing
        Dim FrontImage As Image = Nothing
        Dim BackImage As Image = Nothing
        Dim ImgStream As IO.MemoryStream
        '--Get The Image--'
        If PicBytes.Length > 0 Then
            Try
                ImgStream = New IO.MemoryStream(PicBytes, True) 'Get Reference to a solid memory Location
                ImgStream.Write(PicBytes, 0, PicBytes.Length)   'Consolidate all bytes into one solid memory Location
                ChequeImage = System.Drawing.Image.FromStream(ImgStream, True, True)
                ImgStream.Close()
            Catch ex As Exception
                'MessageBox.Show(ex.Message)
                ChequeImage = Nothing
            End Try
        End If
        GetImages = ChequeImage
    End Function

    'This Function populate grid with columns and row data
    Public Shared Sub PopulateGrid(ByVal GridName As DataGridView, ByVal DataTable As DataTable, Optional ByVal ArrayOfColumnsToMakeInvisble As Object = cNo_Text, Optional ByVal ArrayOfColumnsToAllowEditting As Object = cNo_Text, Optional ByVal IsAddingColumn As Boolean = False)
        Dim dc As DataColumn
        GridName.Rows.Clear()
        GridName.Columns.Clear()
        Try
            If DataTable.Columns.Count > 0 Then
                For Each dc In DataTable.Columns
                    GridName.Columns.Add(dc.ColumnName, dc.ColumnName)
                Next
            End If
            If IsAddingColumn = False Then
                If DataTable.Rows.Count = 0 Then
                    GridName.Columns.Clear()
                    Exit Sub
                End If
                For k As Int32 = 0 To DataTable.Rows.Count - 1
                    GridName.Rows.Add()
                    For i As Integer = 0 To DataTable.Columns.Count - 1
                        GridName.Rows(k).Cells(i).Value = (DataTable.Rows(k)(i))
                    Next i
                Next
            End If
            For i As Integer = 0 To DataTable.Columns.Count - 1
                If DataTable.Rows.Count = 0 Then
                    GridName.Columns.Clear()
                    Exit Sub
                End If
                If ArrayOfColumnsToMakeInvisble = "" Then GoTo Q
                If ArrayOfColumnsToMakeInvisble.Contains(i).ToString Then
                    If GridName.Name.ToString = "dgvMatched" And i = 9 Then
                        GridName.Columns(i).Visible = True
                    Else
                        GridName.Columns(i).Visible = False
                    End If
                Else
                    GridName.Columns(i).Visible = True
                End If
            Next
Q:
            If ArrayOfColumnsToAllowEditting = "" Then GoTo N
            For i As Integer = 0 To DataTable.Columns.Count - 1
                If ArrayOfColumnsToAllowEditting.Contains(i).ToString Then
                    GridName.Columns(i).ReadOnly = False
                Else
                    GridName.Columns(i).ReadOnly = True
                End If
            Next
        Catch ex As Exception

        End Try
N:
        FormatGrids(GridName)
    End Sub

    Public Shared Function MakeInvisibleColumns(ByVal ParamArray Arr() As Object) As String
        Dim ColumnsPassed As Integer
        Dim strArray As String = ""
        Const cCOMMA = ","
        ColumnsPassed = UBound(Arr)
        If ColumnsPassed < 0 Then
            Return cNo_Text
            Exit Function
        End If
        For IntI = 0 To ColumnsPassed
            If Trim(Arr(IntI)) <> cNo_Text Then
                If IntI = ColumnsPassed Then
                    strArray = strArray & Arr(IntI)
                Else
                    strArray = strArray & Arr(IntI) & cCOMMA
                End If
            End If
        Next IntI
        Return strArray
    End Function

    Public Shared Function CloneDataGridRow(ByVal FromGridName As DataGridView, ByVal ToGridName As DataGridView, ByVal CurrRowIndex As Integer)
        Try
            Dim MatchedCurrRowIndex As Int16
            FromGridRowCounter = 0
            ToGridRowCounter = 0
            If ToGridName.Columns.Count > 0 Then
                ToGridName.Rows.Add()
                MatchedCurrRowIndex = ToGridName.Rows.Count - 1
                For i As Integer = 0 To FromGridName.Columns.Count - 1
                    ToGridName.Rows(MatchedCurrRowIndex).Cells(i).Value = (FromGridName.Rows(CurrRowIndex).Cells(i).Value)
                Next i
                FromGridName.Rows.RemoveAt(CurrRowIndex)
            Else
                For Each col In FromGridName.Columns
                    Dim dgvNewCol As DataGridViewColumn = New DataGridViewColumn()
                    dgvNewCol = col.Clone()
                    ToGridName.Columns.Add(dgvNewCol)
                Next
                FromGridName.Columns.Add("JFImage", "JFImage")
                FromGridName.Columns.Add("JRImage", "JRImage")
                FromGridName.Columns.Add("TFImage", "TFImage")
                FromGridName.Columns.Add("UVImage", "UVImage")

                ToGridName.Columns.Add("JFImage", "JFImage")
                ToGridName.Columns.Add("JRImage", "JRImage")
                ToGridName.Columns.Add("TFImage", "TFImage")
                ToGridName.Columns.Add("UVImage", "UVImage")


                ToGridName.Rows.Add()
                MatchedCurrRowIndex = ToGridName.Rows.Count - 1
                For i As Integer = 0 To FromGridName.Columns.Count - 1
                    ToGridName.Rows(MatchedCurrRowIndex).Cells(i).Value = (FromGridName.Rows(CurrRowIndex).Cells(i).Value)
                Next i

                ToGridName.Rows(MatchedCurrRowIndex).Cells("JFImage").Value = CodeLineDetails.FrontImageGrayScale
                ToGridName.Rows(MatchedCurrRowIndex).Cells("JRImage").Value = CodeLineDetails.BackImageGrayScale
                ToGridName.Rows(MatchedCurrRowIndex).Cells("TFImage").Value = CodeLineDetails.FrontImageBW
                ToGridName.Rows(MatchedCurrRowIndex).Cells("UVImage").Value = CodeLineDetails.UVimage

                FromGridName.Rows.RemoveAt(CurrRowIndex)
            End If
            ToGridName.Columns(10).Visible = True
            Format(ToGridName.Rows(MatchedCurrRowIndex).Cells(10).Value, "#,###.#0")
            FromGridRowCounter = FromGridName.Rows.Count
            ToGridRowCounter = ToGridName.Rows.Count
            ' '' '' '' ''GlobalModule2.BRInwardsForm.lblMatchedCount.Text = ToGridRowCounter
            ' '' '' '' ''GlobalModule2.BRInwardsForm.lblUnmatchedCount.Text = FromGridRowCounter
            Application.DoEvents()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        theForm = "Inwards"
    End Function

    Public Shared Function CheckWhetherFileMatchesThePhysicalItem(ByVal MicrLine As String, ByVal FromGridName As DataGridView, ByVal ToGridName As DataGridView) As Boolean
        Try
            MicrLine = CodeLineDetails.ChequeID & CodeLineDetails.BankID & CodeLineDetails.BranchID & CodeLineDetails.ChequeDigit & CodeLineDetails.VoucherCode & CodeLineDetails.AccountID
            For i As Integer = 0 To FromGridName.Rows.Count - 1
                If String.Compare(MicrLine, FromGridName.Rows(i).Cells("micrLine").Value) = 0 Then

                    CodeLineDetails.CountryID = "99" 'TODO the country Codes
                    '' '' '' '' '' ''CodeLineDetails.ReturnCode = GlobalModule2.BRInwardsForm.dgvUnmatched.Rows(i).Cells("RecordType").Value
                    '' '' '' '' '' ''CodeLineDetails.Amount = GlobalModule2.BRInwardsForm.dgvUnmatched.Rows(i).Cells("Amount").Value
                    '' '' '' '' '' ''CodeLineDetails.AccountID = GlobalModule2.BRInwardsForm.dgvUnmatched.Rows(i).Cells("Account").Value
                    '' '' '' '' '' ''CodeLineDetails.columnID = GlobalModule2.BRInwardsForm.dgvUnmatched.Rows(i).Cells("ColumnID").Value
                    ' '' '' ''ExecuteData("UPDATE t_PendingInwardTransactions SET MATCHED=1 WHERE COLUMNID='" & GlobalModule2.BRInwardsForm.dgvUnmatched.Rows(i).Cells("ColumnID").Value & "'", Nothing, dataExecTypes.ExecTypeNonQuery, queryType.SelectStatement)
                    CodeLineDetails.Amount = Format(CodeLineDetails.Amount, "#,###.#0")

                    CloneDataGridRow(FromGridName, ToGridName, i)

                    'GlobalModule2.BRInwardsForm.SaveImagesToDB()
                    Return True
                Else
                    'MsgBox("No Match for this item. " & vbCrLf & "Codeline provided by the reader: " & vbCrLf & "" & MicrLine & ". Note the account may change.")
                End If
            Next
            theForm = "Inwards"
        Catch ex As Exception
            Return False
            MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function
    Public Shared Sub FillCombo(ByVal dt As DataTable, ByVal ColumnName1 As String, ByVal cmbName As ComboBox, Optional ByVal ColumnName2 As String = "")
        cmbName.Items.Clear()
        If dt.Rows.Count > 0 Then
            cmbName.Items.Add("All")
            If ColumnName2 = cNo_Text Then
                For k As Integer = 0 To dt.Rows.Count - 1
                    cmbName.Items.Add(dt.Rows(k).Item("" & ColumnName1 & ""))
                Next
            Else
                For k As Integer = 0 To dt.Rows.Count - 1
                    cmbName.Items.Add(dt.Rows(k).Item("" & ColumnName1 & "") & " - " & dt.Rows(k).Item("" & ColumnName2 & ""))
                Next
            End If
            cmbName.SelectedIndex = 0
        End If
    End Sub
    Public Shared Function SetResolution(ByVal sourceImage As Image, ByVal resolution As Integer) As Image
        Dim reduction As Double = resolution / CInt(sourceImage.HorizontalResolution)
        Using newImage As New Bitmap(sourceImage.Width, sourceImage.Height, sourceImage.PixelFormat)
            newImage.SetResolution(resolution, resolution)
            Dim outImage As New Bitmap(sourceImage, CInt(sourceImage.Width * reduction), CInt(sourceImage.Height * reduction))
            Using g As Graphics = Graphics.FromImage(newImage)
                g.InterpolationMode = InterpolationMode.HighQualityBicubic
                g.DrawImage(outImage, 0, 0)
            End Using
            Return outImage
        End Using
    End Function
    Public Shared Function DoAllowEdittingOfColumns(ByVal ParamArray Arr() As Object) As String
        Dim ColumnsPassed As Integer
        Dim strArray As String = ""
        Const cCOMMA = ","
        ColumnsPassed = UBound(Arr)
        If ColumnsPassed < 0 Then
            Return cNo_Text
            Exit Function
        End If
        For IntI = 0 To ColumnsPassed
            If Trim(Arr(IntI)) <> cNo_Text Then
                If IntI = ColumnsPassed Then
                    strArray = strArray & Arr(IntI)
                Else
                    strArray = strArray & Arr(IntI) & cCOMMA
                End If
            End If
        Next IntI
        Return strArray
    End Function
    Public Sub ResizeImage(ByVal Pict As PictureBox)
        'following code resizes picture to fit
        Dim bm As New Bitmap(Pict.Image)
        Dim x As Int32 'variable for new width size
        Dim y As Int32 'variable for new height size
        Dim width As Integer = Val(x) 'image width. 
        Dim height As Integer = Val(y) 'image height
        Dim thumb As New Bitmap(width, height)
        Dim g As Graphics = Graphics.FromImage(thumb)

        g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
        g.DrawImage(bm, New Rectangle(0, 0, width, height), New Rectangle(0, 0, bm.Width, bm.Height), GraphicsUnit.Pixel)
        g.Dispose()
    End Sub
    Public Function PureBW(ByVal image As System.Drawing.Bitmap, Optional ByVal Mode As BWMode = BWMode.By_Lightness, Optional ByVal tolerance As Single = 0) As System.Drawing.Bitmap
        Dim x As Integer
        Dim y As Integer
        If tolerance > 1 Or tolerance < -1 Then
            Throw New ArgumentOutOfRangeException
            Exit Function
        End If
        For x = 0 To image.Width - 1 Step 1
            For y = 0 To image.Height - 1 Step 1
                Dim clr As System.Drawing.Color = image.GetPixel(x, y)
                If Mode = BWMode.By_RGB_Value Then
                    If (CInt(clr.R) + CInt(clr.G) + CInt(clr.B)) > 383 - (tolerance * 383) Then
                        image.SetPixel(x, y, System.Drawing.Color.White)
                    Else
                        image.SetPixel(x, y, System.Drawing.Color.Black)
                    End If
                Else
                    If clr.GetBrightness > 0.5 - (tolerance / 2) Then
                        image.SetPixel(x, y, System.Drawing.Color.White)
                    Else
                        image.SetPixel(x, y, System.Drawing.Color.Black)
                    End If
                End If
            Next
        Next
        Return image
    End Function
    Public Shared Sub SetIndexedPixel(ByVal x As Integer, ByVal y As Integer, ByVal bmd As BitmapData, ByVal pixel As Boolean)
        'Ensure that it's a 32 bit per pixel file
        Dim index As Integer = y * bmd.Stride + (x >> 3)
        Dim p As Byte = Marshal.ReadByte(bmd.Scan0, index)
        Dim mask As Byte = &H80 >> (x And &H7)
        If pixel Then
            p = p Or mask
        Else
            p = p And CByte(mask ^ &HFF)
        End If
        Marshal.WriteByte(bmd.Scan0, index, p)
    End Sub
    Public Shared Function GetChequeDigit(ByVal Account As String, ByVal BnkCode As String, ByVal BrnCode As String, ByVal strClearingCode As String, ByVal chkCountryCode As String) As String
        Try
            'MessageBox.Show(Account & ":" & BnkCode & ":" & BrnCode & ":" & strClearingCode & ":" & chkCountryCode)
            Dim Acc As String = "" : Dim Bank As String = "" : Dim Branch As String = ""
            GetChequeDigit = ""
            Dim SortCode As String = ""
            Dim sCode As Long = 0
            Dim TotalValue As Long = 0
            Dim x As Integer = 0
            Dim startposition As Integer = 0
            Dim WeightValue As Long = 0
            Dim Modulus11 As String = ""

            Dim Bankcode As String = BnkCode
            Dim BranchCode As String = BrnCode
            Dim ACCOUNTNUMBER As String = Account
            Dim CountryCode As String = chkCountryCode

            If CountryCode = "UG" Then
                SortCode = Trim(Bankcode) & Trim(BranchCode) & Trim(strClearingCode) & Trim(ACCOUNTNUMBER)
                sCode = Trim(Bankcode) & Trim(BranchCode) & Trim(strClearingCode) & Trim(ACCOUNTNUMBER)
                startposition = 16
                WeightValue = 2
                TotalValue = 0
                For x = 0 To Len(SortCode) - 1
                    TotalValue = TotalValue + (Mid(SortCode, startposition, 1) * WeightValue)
                    startposition = startposition - 1
                    WeightValue = WeightValue + 1
                    If WeightValue > 9 Then WeightValue = 2
                Next x
                Modulus11 = TotalValue Mod 11
                If Modulus11 <> 0 Then
                    Modulus11 = 11 - Modulus11
                End If
                If Len(Modulus11) > 1 Then
                    GetChequeDigit = Modulus11
                Else
                    GetChequeDigit = "0" & Modulus11
                End If
            ElseIf CountryCode = "TZ" Then
                Bankcode = BnkCode
                BranchCode = BrnCode
                ACCOUNTNUMBER = Account
                CountryCode = chkCountryCode
                SortCode = strClearingCode & Bankcode & BranchCode
                startposition = 6
                For x = 2 To 7
                    TotalValue = TotalValue + (Mid(SortCode, startposition, 1) * x)
                    startposition = startposition - 1
                Next x

                Modulus11 = TotalValue Mod 11
                If Modulus11 <> 0 Then
                    Modulus11 = 11 - Modulus11
                End If
                If Modulus11 = 10 Then
                    Modulus11 = 0
                End If
                If Len(Modulus11) > 1 Then
                    GetChequeDigit = Modulus11
                Else
                    GetChequeDigit = "0" & Modulus11
                End If
            ElseIf CountryCode = "KE" Then
                Acc = "" : Bank = "" : Branch = ""
                Acc = New String("0", 10 - Len(Trim(Account))) & Trim(Account)
                Bank = New String("0", 2 - Len(Trim(BnkCode))) & Trim(BnkCode)
                Branch = New String("0", 3 - Len(Trim(BrnCode))) & Trim(BrnCode)
                Dim pos17, pos18, pos19, pos20, pos21, pos22, pos23, pos24, pos25, pos26, pos32, pos33, pos34, pos35, pos36 As String
                pos17 = Mid(Acc, 10, 1) : pos19 = Mid(Acc, 8, 1) : pos21 = Mid(Acc, 6, 1) : pos23 = Mid(Acc, 4, 1) : pos25 = Mid(Acc, 2, 1) : pos33 = Mid(Branch, 2, 1) : pos35 = Mid(Bank, 2, 1)
                pos18 = Mid(Acc, 9, 1) : pos20 = Mid(Acc, 7, 1) : pos22 = Mid(Acc, 5, 1) : pos24 = Mid(Acc, 3, 1) : pos26 = Mid(Acc, 1, 1) : pos32 = Mid(Branch, 3, 1) : pos34 = Mid(Branch, 1, 1) : pos36 = Mid(Bank, 1, 1)

                Dim newpos18 As Double = 2 * Val(pos18)
                Dim nval As String = Left(New String("0", 2 - Len(newpos18.ToString)) & newpos18.ToString, 1)
                Dim nval1 As String = Right(New String("0", 2 - Len(newpos18.ToString)) & newpos18.ToString, 1)

                Dim newpos20 As Double = 2 * Val(pos20)
                Dim nval2 As String = Left(New String("0", 2 - Len(newpos20.ToString)) & newpos20.ToString, 1)
                Dim nval3 As String = Right(New String("0", 2 - Len(newpos20.ToString)) & newpos20.ToString, 1)

                Dim newpos22 As Double = 2 * Val(pos22)
                Dim nval4 As String = Left(New String("0", 2 - Len(newpos22.ToString)) & newpos22.ToString, 1)
                Dim nval5 As String = Right(New String("0", 2 - Len(newpos22.ToString)) & newpos22.ToString, 1)

                Dim newpos24 As Double = 2 * Val(pos24)
                Dim nval6 As String = Left(New String("0", 2 - Len(newpos24.ToString)) & newpos24.ToString, 1)
                Dim nval7 As String = Right(New String("0", 2 - Len(newpos24.ToString)) & newpos24.ToString, 1)

                Dim newpos26 As Double = 2 * Val(pos26)
                Dim nval8 As String = Left(New String("0", 2 - Len(newpos26.ToString)) & newpos26.ToString, 1)
                Dim nval9 As String = Right(New String("0", 2 - Len(newpos26.ToString)) & newpos26.ToString, 1)

                Dim newpos32 As Double = 2 * Val(pos32)
                Dim nval10 As String = Left(New String("0", 2 - Len(newpos32.ToString)) & newpos32.ToString, 1)
                Dim nval11 As String = Right(New String("0", 2 - Len(newpos32.ToString)) & newpos32.ToString, 1)

                Dim newpos34 As Double = 2 * Val(pos34)
                Dim nval12 As String = Left(New String("0", 2 - Len(newpos34.ToString)) & newpos34.ToString, 1)
                Dim nval13 As String = Right(New String("0", 2 - Len(newpos34.ToString)) & newpos34.ToString, 1)

                Dim newpos36 As Double = 2 * Val(pos36)
                Dim nval14 As String = Left(New String("0", 2 - Len(newpos36.ToString)) & newpos36.ToString, 1)
                Dim nval15 As String = Right(New String("0", 2 - Len(newpos36.ToString)) & newpos36.ToString, 1)
                Dim sumnval01 As Double = Val(nval) + Val(nval1)
                Dim sumnval23 As Double = Val(nval2) + Val(nval3)
                Dim sumnval45 As Double = Val(nval4) + Val(nval5)
                Dim sumnval67 As Double = Val(nval6) + Val(nval7)
                Dim sumnval89 As Double = Val(nval8) + Val(nval9)
                Dim sumnval1011 As Double = Val(nval10) + Val(nval11)
                Dim sumnval1213 As Double = Val(nval12) + Val(nval13)
                Dim sumnval1415 As Double = Val(nval14) + Val(nval15)

                Dim sum1 As Double = Val(pos17) + Val(pos19) + Val(pos21) + Val(pos23) + Val(pos25) + Val(pos33) + Val(pos35)
                Dim sum2 As Double = sumnval01 + sumnval23 + sumnval45 + sumnval67 + sumnval89 + sumnval1011 + sumnval1213 + sumnval1415
                Dim sum4 As Double = Val(sum1) + Val(sum2)
                GetChequeDigit = 100 - Val(sum4)
                GetChequeDigit = Right(GetChequeDigit, 1)
            End If
            'MessageBox.Show(GetChequeDigit)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            GetChequeDigit = ""
        End Try
    End Function
    Public Function CNumeric(ByVal Txt As Object) As Double
        Try
            If IsDBNull(Txt) Then Txt = 0.0# 'if The Value Is NULL
            If Txt = "" Then Txt = 0.0# 'if The Value Is cNo_Text
            If Not IsNumeric(Txt) Then Txt = 0.0# 'If The Value Is Other Than Number
            CNumeric = CDbl(Txt)
            Exit Function
        Catch ex As Exception

        End Try
    End Function

    Public Sub ImageResizer(ByVal Ffull As String)
        Dim str As String = ""
        strImagePath = strImagePath
        If Not Directory.Exists(strImagePath) Or Not Directory.Exists(strImagePath) Then
            MessageBox.Show("The folder you specified as input and/or output path does not exist. Please, check it and retry.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If
        Dim Fshort As String
        FromFile(Ffull)
        Application.DoEvents()
        Fshort = Ffull.Substring(Ffull.LastIndexOf("\") + 1)
        Application.DoEvents()
        Dim dr As DialogResult
        dr = DialogResult.Yes
        Reduce(Double.Parse(0.32, New System.Globalization.CultureInfo("EN-us")))
        Application.DoEvents()
        ToFile(strImagePath & "\" & Fshort)
    End Sub
    Public Shared Function ResizeImage(ByVal image As Image,
  ByVal size As Size, Optional ByVal preserveAspectRatio As Boolean = True) As Image
        Dim newWidth As Integer
        Dim newHeight As Integer
        If preserveAspectRatio Then
            Dim originalWidth As Integer = image.Width
            Dim originalHeight As Integer = image.Height
            Dim percentWidth As Single = CSng(size.Width) / CSng(originalWidth)
            Dim percentHeight As Single = CSng(size.Height) / CSng(originalHeight)
            Dim percent As Single = If(percentHeight < percentWidth, percentHeight, percentWidth)
            newWidth = CInt(originalWidth * percent)
            newHeight = CInt(originalHeight * percent)
        Else
            newWidth = size.Width
            newHeight = size.Height
        End If
        Dim newImage As Image = New Bitmap(newWidth, newHeight)
        Using graphicsHandle As Graphics = Graphics.FromImage(newImage)
            graphicsHandle.InterpolationMode = InterpolationMode.HighQualityBicubic
            graphicsHandle.DrawImage(image, 0, 0, newWidth, newHeight)
        End Using
        Return newImage
    End Function
    Private Sub Reduce(ByVal factor As Double)
        img = New Bitmap(img, New Size(img.Size.Width * factor, img.Size.Height * factor))
        'picPhoto.Image = img
        Dim thumb As System.Drawing.Image
        Dim Quality As Integer = 100
        thumb = New Bitmap(img.Size.Width, img.Size.Height)
        Dim objGraphics As System.Drawing.Graphics
        objGraphics = System.Drawing.Graphics.FromImage(thumb)
        objGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic
        objGraphics.SmoothingMode = SmoothingMode.HighQuality
        objGraphics.PixelOffsetMode = PixelOffsetMode.HighQuality
        objGraphics.CompositingQuality = CompositingQuality.HighQuality
        Dim codecEncoder As ImageCodecInfo = GetEncoder("image/tiff")
        Dim encodeParams As New EncoderParameters(1)
        Dim qualityParam As New EncoderParameter(System.Drawing.Imaging.Encoder.Quality, Quality)
        encodeParams.Param(0) = qualityParam
        objGraphics.DrawImage(img, 0, 0, width, height)
        Dim SizeKb As String
        ' To compute: size in Kb
        Dim ms As New MemoryStream()
        img.Save(ms, codecEncoder, encodeParams)
        SizeKb = (ms.Length \ 1024).ToString() & "Kb "
    End Sub
    Private Sub ToFile(ByVal filename As String)
        Dim ms As New MemoryStream()
        img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg)
        Dim imgData(ms.Length - 1) As Byte
        ms.Position = 0
        ms.Read(imgData, 0, ms.Length)
        Dim fs As New FileStream(filename, FileMode.Create, FileAccess.Write)
        fs.Write(imgData, 0, UBound(imgData))
        fs.Close()
    End Sub
    Public Shared Function MakeGrayscale(ByVal original As Bitmap, ByVal FileNam As String) As Bitmap

        ''create a blank bitmap the same size as original
        Dim newBitmap As Bitmap = New Bitmap(original.Width, original.Height)

        ''get a graphics object from the new image
        Dim g As Graphics = Graphics.FromImage(newBitmap)

        'create the grayscale ColorMatrix
        Dim colorMatrix As ColorMatrix = New ColorMatrix(New Single()() {
          New Single() {0.3F, 0.3F, 0.3F, 0, 0},
          New Single() {0.59F, 0.59F, 0.59F, 0, 0},
          New Single() {0.11F, 0.11F, 0.11F, 0, 0},
          New Single() {0, 0, 0, 1, 0},
          New Single() {0, 0, 0, 0, 1}})

        'create some image attributes
        Dim attributes As ImageAttributes = New ImageAttributes()

        'set the color matrix attribute
        attributes.SetColorMatrix(colorMatrix)

        'draw the original image on the new image using the grayscale color matrix
        g.DrawImage(original, New Rectangle(0, 0, original.Width, original.Height), 0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes)

        'dispose the Graphics object
        g.Dispose()
        '------------------------------------------------------------------------------------
        'Convert to Grayscale
        Dim image2 As Bitmap = newBitmap

        'Save as PNG, 8bbp
        Dim fcb As Windows.Media.Imaging.FormatConvertedBitmap = New Windows.Media.Imaging.FormatConvertedBitmap(System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(image2.GetHbitmap(System.Drawing.Color.Transparent), IntPtr.Zero, System.Windows.Int32Rect.Empty,
          Windows.Media.Imaging.BitmapSizeOptions.FromWidthAndHeight(image2.Width, image2.Height)), System.Windows.Media.PixelFormats.Gray8, Windows.Media.Imaging.BitmapPalettes.Gray256, 0.5)
        Dim pngBitmapEncoder As Windows.Media.Imaging.GifBitmapEncoder = New Windows.Media.Imaging.GifBitmapEncoder()
        'pngBitmapEncoder.Interlac = Windows.Media.Imaging.PngInterlaceOption.Off
        'pngBitmapEncoder.Frames.Add(Windows.Media.Imaging.BitmapFrame.Create(fcb))

        Dim fileStream As Stream = File.Open(FileNam, FileMode.Create)
        pngBitmapEncoder.Save(fileStream)
        fileStream.Close()
        newBitmap = FromFile(FileNam)
        newBitmap.SetResolution(100, 100)
        '-------------------------------------------------------------------------------------------
        Return newBitmap

    End Function

    Public Shared Function FromFile(ByVal filename As String) As Bitmap
        Dim NewImg As Bitmap
        Dim fs As New FileStream(filename, FileMode.Open, FileAccess.Read)
        Dim imgData(fs.Length) As Byte
        fs.Read(imgData, 0, fs.Length)
        fs.Close()
        Try
            NewImg = Image.FromStream(New MemoryStream(imgData))
            'imgFormat = NewImg.RawFormat
        Catch

        End Try
        Return NewImg
    End Function
    Function GetEncoder(ByVal mimeType As String) As ImageCodecInfo
        Try
            Dim codecs() As ImageCodecInfo = ImageCodecInfo.GetImageEncoders()
            For Each codec As ImageCodecInfo In codecs
                If codec.MimeType = mimeType Then
                    Return codec
                End If
            Next
        Catch ex As Exception
            Throw New Exception("Error processing GetEncoder (MimeType)", ex)
        End Try
    End Function

    Public Shared Sub ClearTheHolders()
        CodeLineDetails.AccountID = ""
        CodeLineDetails.ReturnCode = "00"
        CodeLineDetails.Amount = "0"
        CodeLineDetails.BankID = ""
        CodeLineDetails.BranchID = ""
        CodeLineDetails.ChequeDigit = ""
        CodeLineDetails.ChequeID = ""
        CodeLineDetails.ClearingDays = "0"
        CodeLineDetails.FullMicrline = ""
        CodeLineDetails.CommissionRate = "0"
        CodeLineDetails.OurCommission = "0"
        CodeLineDetails.MinCommissionRate = "0"
        CodeLineDetails.TheirAccountID = ""
    End Sub

    Public Shared Function HashTheImage(ByVal DataInByte As Byte())
        Dim Result() As Byte
        Dim sha As New SHA384Managed()
        Try
            Result = sha.ComputeHash(DataInByte)
        Catch ex As Exception
            Return Nothing
        End Try
        Return Result
    End Function
    Public Function GetImageSizeOneWord(ByVal size As Int32) As Byte
        Dim bytes() As Byte = BitConverter.GetBytes(size)   '----- Convert an inter to byte array
    End Function
    Public Function GetImageSizeOneWord(ByVal size() As Byte) As Int32
        Dim intbytes As Integer = BitConverter.ToInt32(size, 0) '--------Convert Byte array to an integer
    End Function

    Public Shared Function GenarateImageSize(ByVal ByteImage() As Byte) As String
        Dim i As Integer
        Dim StrChr As String = ""
        Dim myFileName As String = App_Path() & "FileSizeGetter.txt"
        Dim myFileStream As FileStream = Nothing
        Try
            Dim byteFntSize() As Byte = ByteImage
            Dim m() As Byte = System.BitConverter.GetBytes(byteFntSize.Length)
            myFileStream = New FileStream(myFileName, FileMode.Append)
            myFileStream.Write(m, 0, m.Length)
            If Not (myFileStream Is Nothing) Then myFileStream.Close()
            Dim f As System.IO.FileStream
            Dim mybuffer(3) As Byte
            f = New System.IO.FileStream(App_Path() & "FileSizeGetter.txt", IO.FileMode.Open, IO.FileAccess.Read)
            f.Read(mybuffer, 0, 3)
            For i = 0 To 3
                StrChr = StrChr & Chr(mybuffer(i))
            Next
            f.Close()
            Kill(App_Path() & "FileSizeGetter.txt")
        Catch e As Exception
            Throw e
        Finally
            If Not (myFileStream Is Nothing) Then myFileStream.Close()
        End Try
        GenarateImageSize = StrChr
    End Function
    Public Function ReadImagesize(ByVal ImageSizeInString As String) As Int32
        Dim myFileStream As FileStream = Nothing
        Dim intValue As Integer = 0
        Dim MByte() As Byte = StringToByteArray(ImageSizeInString)
        Try
            Dim myFileName As String = "c:\FileSizeGetter.txt"
            myFileStream = New FileStream(myFileName, FileMode.Append)
            myFileStream.Write(MByte, 0, MByte.Length)
            If Not (myFileStream Is Nothing) Then myFileStream.Close()
            Dim byteFntSizerr() As Byte = ConvertImages("C:\FileSizeGetter.txt")
            intValue = BitConverter.ToInt32(byteFntSizerr, 0)
        Catch e As Exception
            Throw e
        Finally
            Kill("C:\FileSizeGetter.txt")
            If Not (myFileStream Is Nothing) Then myFileStream.Close()
        End Try
        ReadImagesize = intValue
    End Function
    Public Function StringToByteArray(ByVal s As String) As Byte()
        Dim b(s.Length - 1) As Byte
        Dim i As Integer
        For i = 0 To s.Length - 1
            b(i) = Convert.ToByte(s(i))
        Next
        Return b
    End Function
    Public Function byteArrayToString(ByVal b() As Byte) As String
        Dim i As Integer
        Dim s As New System.Text.StringBuilder()
        For i = 0 To b.Length - 1
            Console.WriteLine(b(i))
            If i <> b.Length - 1 Then
                s.Append(b(i) & " ")
            Else
                s.Append(b(i))
            End If
        Next
        Return s.ToString
    End Function

    Public Function ReadImageFromTextFile(ByVal StringedImage As String) As Byte

    End Function


    Public Shared Function Bytes2String(ByVal bytes As Byte()) As String
        Dim SystemType As String = ConfigurationManager.AppSettings("sysType")
        If (SystemType = "BR" Or SystemType = "BRMFO") And OldImageFormat = "0" Then
            Return System.Text.Encoding.GetEncoding(1252).GetString(bytes)
        ElseIf (SystemType = "BR" Or SystemType = "BRMFO") And OldImageFormat = "1" Then
            Return Convert.ToBase64String(bytes)
        ElseIf SystemType = "BROLD" Or SystemType = "BRNET" Then
            Return Convert.ToBase64String(bytes)
        Else
            Return System.Text.Encoding.GetEncoding(1252).GetString(bytes)
        End If
    End Function
    Public Shared Function Bytes2Image(ByVal ImageData As Byte()) As Image
        Dim RImage As Image
        Dim ms As New IO.MemoryStream
        RImage = Image.FromStream(New MemoryStream(ImageData))
        'MessageBox.Show(RImage.Size.ToString())
        Return RImage
    End Function
    Public Shared Function ValidateFileSize(ByVal bImage As Image) As Image
        Dim img As System.Drawing.Image = bImage
        Dim height As Integer = img.Height
        Dim width As Integer = img.Width
        Dim Newimg As System.Drawing.Image

        If height > 800 Or width > 600 Then
            Newimg = ResizeImage(img, New Size(800, 600))
        Else
            Newimg = img
        End If
        Return Newimg
    End Function

    Public Shared Function String2Bytes(ByVal str As String) As Byte()
        Dim SystemType As String = ConfigurationManager.AppSettings("sysType")
        If (SystemType = "BR" Or SystemType = "BRMFO") And OldImageFormat = "0" Then
            Return System.Text.Encoding.GetEncoding(1252).GetBytes(str)
        ElseIf (SystemType = "BR" Or SystemType = "BRMFO") And OldImageFormat = "1" Then
            Return Convert.FromBase64String(str)
        ElseIf SystemType = "BROLD" Or SystemType = "BRNET" Then
            Return Convert.FromBase64String(str)
        Else
            Return System.Text.Encoding.GetEncoding(1252).GetBytes(str)
        End If
    End Function
    Public Shared Function ByteToString(ByVal Value As Byte()) As String
        'Return Convert.ToBase64String(Value)
        Dim sRet As String = ""
        For i As Integer = 0 To Value.Length - 1
            sRet = String.Concat(sRet, Chr(Value(i)))
        Next
        Return sRet
    End Function
    Public Shared Function StringToByte(ByVal Value As String) As Byte()
        Dim bRet(Value.Length - 1) As Byte
        For i As Integer = 0 To Value.Length - 1
            bRet(i) = Asc(Value.Substring(i, 1))
        Next
        Return bRet
        'Return Convert.FromBase64String(Value)
    End Function
    Public Shared Function App_Path() As String
        Return System.AppDomain.CurrentDomain.BaseDirectory()
    End Function
    Private Function CheckInstanceOfApp() As Boolean
        Dim appProc() As Process
        Dim strModName, strProcName As String
        strModName = Process.GetCurrentProcess.MainModule.ModuleName
        strProcName = System.IO.Path.GetFileNameWithoutExtension(strModName)
        appProc = Process.GetProcessesByName(strProcName)
        If appProc.Length > 1 Then
            MessageBox.Show("There is an instance of this application running.", "BrClearing", MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Return True
        Else
            Return False
        End If
    End Function
    Sub Main()
        Try
            If CheckInstanceOfApp() = True Then
                Application.Exit()
                Exit Sub
            End If
            ReadTxtFile()
            ShowScan()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub
    Public Shared Sub ReadTxtFile()
        Dim MTS As Object
        Dim MTSConn As String = ""
        Dim stOurBranchID As String = ""
        Dim isActive As String = 1
        Dim ConnType As String = ""
        Dim readerType As String = ""
        Dim SystemType As String = ""
        Dim arr As New ArrayList
        'If File.Exists(App_Path() & "BrCtsClearing.ini") = False Then
        '    File.Create(App_Path() & "BrCtsClearing.ini").Dispose()
        'Else
        '    File.Delete(App_Path() & "BrCtsClearing.ini")
        '    File.Create(App_Path() & "BrCtsClearing.ini").Dispose()
        'End If
        Dim objReader
        Try
            objReader = New System.IO.StreamReader(App_Path() + "\BRScanner.exe")
        Catch ex As Exception

        End Try
        Try
            ' MessageBox.Show("Imengia hapa")


            Dim IsCheckSum As Boolean = ConfigurationManager.AppSettings("sysCheckSum")
            Try

                ConnType = ConfigurationManager.AppSettings("ConnectionType")
                readerType = ConfigurationManager.AppSettings("ReaderType")
                SystemType = ConfigurationManager.AppSettings("sysType")
                CountryCode = ConfigurationManager.AppSettings("CountryCode")
                strBatchPath = ConfigurationManager.AppSettings("strBatchPath")
                strJavaExeInstallation = ConfigurationManager.AppSettings("strJavaExeInstallation")
                strDSkeyFile = ConfigurationManager.AppSettings("strDSkeyFile")
                keyPass = ConfigurationManager.AppSettings("keyPass")
                Modscan.FilePath = ConfigurationManager.AppSettings("IncomingFiles")
                Modscan.ArchivesPath = ConfigurationManager.AppSettings("ClearingArchives")
                Modscan.OldImageFormat = ConfigurationManager.AppSettings("ImageFormat")
            Catch ex As Exception
                'MsgBox(ex.Message)
            End Try

            Try
                BankIDSize = ConfigurationManager.AppSettings("BankIDSize")
            Catch ex As Exception
                'MsgBox(ex.Message)
            End Try
            'MessageBox.Show("so far so good")

            Select Case CountryCode.ToUpper.Trim
                Case "UG"
                    CouID = 2
                Case "SL"
                    CouID = 11
                Case "TZ"
                    CouID = 3
                Case "RD"
                    CouID = 12
                Case "KE"
                    CouID = 1
                Case "ET"
                    CouID = 4
                Case "SA"
                    CouID = 9
            End Select

            Select Case SystemType.ToUpper.Trim
                Case "BR"
                    SysType = 1
                Case "BRMFO"
                    SysType = 2
                Case "BRNET"
                    SysType = 3
                Case "BRNETOLD"
                    SysType = 3
            End Select

            'MessageBox.Show("inasoma Config")
            Dim TextLine As String = ""

            If Modscan.SysType <> Modscan.ENUM_SysType.BRNET Then
                FILE_NAME = App_Path().Substring(0, App_Path().LastIndexOf("\"))
                FILE_NAME = App_Path().Substring(0, FILE_NAME.LastIndexOf("\"))
                FILE_NAME = FILE_NAME & "\user.ini"
                objReader = New System.IO.StreamReader(FILE_NAME)
                If System.IO.File.Exists(FILE_NAME) = True Then
                    Do While objReader.Peek() <> -1
                        arr.Add(objReader.ReadLine())
                    Loop
                End If
            End If
            Modscan.ReaderUsed = readerType
            'MessageBox.Show("done Config")
            'MessageBox.Show(SysType)
            If SysType = ENUM_SysType.BRMFO Then
                For i As Int32 = 0 To arr.Count
                    If arr(i).Contains("DBSERVERNAME") = True Then
                        DBServerName = arr(i).ToString.Substring((arr(i).ToString.IndexOf("=") + 1))
                        Exit For
                    End If
                Next
                For i As Int32 = 0 To arr.Count
                    If arr(i).Contains("DBNAME") = True Then
                        DatabaseName = arr(i).ToString.Substring((arr(i).ToString.IndexOf("=") + 1))
                        Exit For
                    End If
                Next
                For i As Int32 = 0 To arr.Count
                    If arr(i).Contains("PWD=") = True Then
                        If IsCheckSum = True Then
                            'Dim BRchecks = New BRCheckSum.clsSecurity
                            'Dim BRchecks = New BRCheckSum.clsSecurity
                            'DBPassword = BRchecks.myDecryptionHex(arr(i).ToString.Substring((arr(i).ToString.IndexOf("=") + 1)))
                            DBPassword = GetCheckSum(arr(i).ToString.Substring((arr(i).ToString.IndexOf("=") + 1)))
                        Else
                            Dim checks = New BRSecurity.clsSecurity
                            '    DBPassword = checks.DecryptPassword(arr(i).ToString.Substring((arr(i).ToString.IndexOf("=") + 1)))
                            'DBPassword = GetCheckSum(arr(i).ToString.Substring((arr(i).ToString.IndexOf("=") + 1)))
                            DBPassword = "friend"

                        End If

                        Exit For
                    End If
                Next
                'End If

                'MsgBox("Imefika Hapa")
                For i As Int32 = 0 To arr.Count
                    If arr(i).Contains("BANKID") = True Then
                        OurBankID = arr(i).ToString.Substring((arr(i).ToString.IndexOf("=") + 1))
                        Exit For
                    End If
                Next
                Try
                    For i As Int32 = 0 To arr.Count
                        If arr(i).Contains("BRANCHID") = True Then
                            stOurBranchID = arr(i).ToString.Substring((arr(i).ToString.IndexOf("=") + 1))
                            Exit For
                        End If
                    Next
                Catch ex As Exception
                    MsgBox(ex.Message)
                    For i As Int32 = 0 To arr.Count
                        If arr(i).Contains("OURBRANCHID") = True Then
                            stOurBranchID = arr(i).ToString.Substring((arr(i).ToString.IndexOf("=") + 1))
                            Exit For
                        End If
                    Next
                End Try

                objReader.Close()


            ElseIf Modscan.SysType = Modscan.ENUM_SysType.BRNETOLD Then
                'Dim strSystemD As BrWebDataEcryption = New BrWebDataEcryption()
                Dim Serv As WebService.IEService = New WebService.IEService()
                Dim usrinfo As New UserInfo()
                Dim myUserID As String = "SYS"
                usrinfo.strSystem = "BRNET"
                strPathEncpt = Serv.GetConnectionString()
                'strSystemD = New BrWebDataEcryption()
                'usrinfo.strSystem = strSystemD.DecyptKey(strPathEncpt).ToString
                '//usrinfo.strSystem = "Data Source=SAMKAMUNYA;Initial Catalog=BRNETDB;User ID=realm;Password=friend;";
                usrinfo.strUser = myUserID
                usrinfo.strBranch = ConfigurationManager.AppSettings("OurBranchID")
                usrinfo.strLanguage = "en"
                usrinfo.strBank = ConfigurationManager.AppSettings("BankID")
                Dim WorkingDate As DateTime = SODDate(usrinfo, usrinfo.strBranch)
            ElseIf Modscan.SysType <> Modscan.ENUM_SysType.BRNET And SysType <> ENUM_SysType.BRMFO Then

                'If ConnType.ToUpper = "MTS" Then
                '    For i As Int32 = 0 To arr.Count
                '        If arr(i).Contains("MTSSERVERNAME") = True Then
                '            MTSServer = arr(i).ToString.Substring((arr(i).ToString.IndexOf("=") + 1))
                '            Exit For
                '        End If
                '    Next
                '    MTS = CreateObject("prjDataAccess.clsDataAccess", MTSServer)
                '    MTSConn = MTS.GetConnectionstring
                '    'MsgBox(MTSConn)
                '    DBPassword = MTSConn.Substring(MTSConn.LastIndexOf(";") + 1)
                '    DBServerName = MTSConn.Substring(0, MTSConn.IndexOf(";"))
                '    DatabaseName = MTSConn.Substring(MTSConn.IndexOf(";") + 1, MTSConn.Substring(MTSConn.IndexOf(";") + 1, _
                '                   MTSConn.LastIndexOf(";")).Length - MTSConn.Substring(MTSConn.LastIndexOf(";")).Length)
                '    GetDbConnectionStrings()
                '    MTS = Nothing
                'Else
                'MsgBox("inaanza kazi Hapa")
                'Try
                For i As Int32 = 0 To arr.Count
                    If arr(i).Contains("DBSERVER") = True Then
                        DBServerName = arr(i).ToString.Substring((arr(i).ToString.IndexOf("=") + 1))
                        Exit For
                    End If
                Next
                For i As Int32 = 0 To arr.Count
                    If arr(i).Contains("DBNAME") = True Then
                        DatabaseName = arr(i).ToString.Substring((arr(i).ToString.IndexOf("=") + 1))
                        Exit For
                    End If
                Next
                For i As Int32 = 0 To arr.Count
                    If arr(i).Contains("PWD") = True Then

                        Dim checks = New clsSecurity
                        DBPassword = checks.DecryptPassword(arr(i).ToString.Substring((arr(i).ToString.IndexOf("=") + 1))) 'GetCheckSum(arr(i).ToString.Substring((arr(i).ToString.IndexOf("=") + 1))) 'decrypt(arr(i).ToString.Substring((arr(i).ToString.IndexOf("=") + 1)))
                        Exit For
                    End If
                Next
                'MsgBox(DatabaseName)
                'MsgBox(DBServerName)
                For i As Int32 = 0 To arr.Count
                    If arr(i).Contains("BANKID") = True Then
                        OurBankID = arr(i).ToString.Substring((arr(i).ToString.IndexOf("=") + 1))
                        Exit For
                    End If
                Next
                For i As Int32 = 0 To arr.Count
                    If arr(i).Contains("BRANCHID") = True Then
                        stOurBranchID = arr(i).ToString.Substring((arr(i).ToString.IndexOf("=") + 1))
                        Exit For
                    End If
                Next
                objReader.Close()

            End If

        Catch ex As Exception
            MsgBox("2:- " + ex.Message)
        End Try

        'End If
        objReader.Close()
        objReader = New System.IO.StreamReader(App_Path() & "BrCtsClearing.ini")
        Try
            '    'Modscan.DBServerName = ConfigurationManager.AppSettings("strDBServerName")
            '    'Modscan.DatabaseName = ConfigurationManager.AppSettings("strDatabaseName")
            '    'Modscan.BRUserName = ConfigurationManager.AppSettings("strBRUserName")
            '    'Modscan.DBPassword = ConfigurationManager.AppSettings("strBRUserPassword")
            '    'Dim strBranch As String = ConfigurationManager.AppSettings("OurBranchID")
            '    'ExecuteData(GetModify("p_ReadScan", "TrxBranchID", strBranch), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
            'Catch ex As Exception
            '    'MessageBox.Show(ex.Message)
            'End Try
            'Dim Curr As String = ""
            'Dim Opr As String = ""
            'Dim AccID As String = ""
            'Dim WorkinDt As String = ""
            'Dim AccountName As String = ""
            'Dim OurB As String = ""
            'Dim AccType As String = ""
            'Dim User As String = ""
            'Dim Pass As String = ""
            'Dim db As String = ""
            'Dim Server As String = ""
            'Dim UserSys As String = ""
            'Dim SysPass As String = ""
            'If publicDTbl.Rows.Count > 0 Then
            '    Curr = publicDTbl.Rows(0)("Curr").ToString.Trim
            '    Opr = publicDTbl.Rows(0)("OperatorID").ToString.Trim
            '    AccID = publicDTbl.Rows(0)("AccountID").ToString.Trim
            '    WorkinDt = publicDTbl.Rows(0)("WorkingDate").ToString.Trim
            '    AccountName = publicDTbl.Rows(0)("AccountName").ToString.Trim
            '    OurB = publicDTbl.Rows(0)("OurBranchID").ToString.Trim
            '    AccType = publicDTbl.Rows(0)("AccountTypeID").ToString.Trim
            '    User = publicDTbl.Rows(0)("U").ToString.Trim
            '    Pass = publicDTbl.Rows(0)("P").ToString.Trim
            '    db = publicDTbl.Rows(0)("D").ToString.Trim
            '    Server = publicDTbl.Rows(0)("S").ToString.Trim
            '    UserSys = publicDTbl.Rows(0)("UA").ToString.Trim
            '    SysPass = publicDTbl.Rows(0)("PA").ToString.Trim
            'End If
            'objReader.Close()
            'FILE_NAME = App_Path() & "BrCtsClearing.ini"
            'Dim Content As String = ""
            'Content = ""
            'Content = "LocalCurrency=" + Curr
            'WriterContents(FILE_NAME, Content)
            'Content = ""
            'Content = "OperatorID=" + Opr
            'WriterContents(FILE_NAME, Content)
            'Content = ""
            'Content = "AccountID=" + AccID
            'WriterContents(FILE_NAME, Content)
            'Content = ""
            'Content = "WORKING_DATE=" + WorkinDt
            'WriterContents(FILE_NAME, Content)
            'Content = ""
            'Content = "TrxType=OC"
            'WriterContents(FILE_NAME, Content)
            'Content = ""
            'Content = "ReaderUsed=3"
            'WriterContents(FILE_NAME, Content)
            'Content = ""
            'Content = "FromDate=" + WorkinDt
            'WriterContents(FILE_NAME, Content)
            'Content = ""
            'Content = "AccountName=" + AccountName
            'WriterContents(FILE_NAME, Content)
            'Content = ""
            'Content = "pScan=0"
            'WriterContents(FILE_NAME, Content)
            'Content = ""
            'Content = "OurBranchID=" + OurB
            'WriterContents(FILE_NAME, Content)
            'Content = ""
            'Content = "ToDate=" + WorkinDt
            'WriterContents(FILE_NAME, Content)
            'Content = ""
            'Content = "AccountType=" + AccType
            'WriterContents(FILE_NAME, Content)
            'Content = ""
            'Content = "SerialID="
            'WriterContents(FILE_NAME, Content)
            'Content = ""
            'Content = "ChqID="
            'WriterContents(FILE_NAME, Content)
            'Content = ""
            'Content = "TheirBankID="
            'WriterContents(FILE_NAME, Content)
            'Content = "TheirAccID="
            'WriterContents(FILE_NAME, Content)
            'Content = ""
            'Content = "FilePath="
            'WriterContents(FILE_NAME, Content)
            'Content = ""
            'Content = "boolGetImagesDetails=False"
            'WriterContents(FILE_NAME, Content)
            'Content = ""
            'Content = "pAccountBranch="
            'WriterContents(FILE_NAME, Content)
            'Content = ""
            'Content = "DBType="
            'WriterContents(FILE_NAME, Content)
            'Content = ""
            'Content = "ProductID="
            'WriterContents(FILE_NAME, Content)
            'Content = ""
            'Content = "ColumnID="
            'WriterContents(FILE_NAME, Content)
            'Content = ""
            'Content = "CurrencyID="
            'WriterContents(FILE_NAME, Content)
            'Content = ""
            'Content = "DBServerName=" + Server
            'WriterContents(FILE_NAME, Content)
            'Content = ""
            'Content = "DatabaseName=" + db
            'WriterContents(FILE_NAME, Content)
            'Content = ""
            'Content = "BRUserName=" + User
            'WriterContents(FILE_NAME, Content)
            'Content = ""
            'Content = "BRUserPassword=" + Pass
            'WriterContents(FILE_NAME, Content)
            'Content = ""
            'Content = "SYSADMIN1UserName=" + UserSys
            'WriterContents(FILE_NAME, Content)
            'Content = ""
            'Content = "SYSADMIN1Password=" + SysPass
            'WriterContents(FILE_NAME, Content)
            arr = New ArrayList
            FILE_NAME = App_Path() & "BrCtsClearing.ini"
            objReader = New System.IO.StreamReader(FILE_NAME)

            If System.IO.File.Exists(FILE_NAME) = True Then
                Do While objReader.Peek() <> -1
                    arr.Add(objReader.ReadLine())
                Loop
            End If
            For i As Int32 = 0 To arr.Count
                If arr(i).Contains("OperatorID") = True Then
                    Modscan.OperatorID = arr(i).ToString.Substring((arr(i).ToString.IndexOf("=") + 1))
                    Exit For
                End If
            Next
            For i As Int32 = 0 To arr.Count
                If arr(i).Contains("AccountID") = True Then
                    Modscan.strAccountID = arr(i).ToString.Substring((arr(i).ToString.IndexOf("=") + 1))
                    Exit For
                End If
            Next
            For i As Int32 = 0 To arr.Count
                If arr(i).Contains("AccountName") = True Then
                    Modscan.strAccountName = arr(i).ToString.Substring((arr(i).ToString.IndexOf("=") + 1))
                    Exit For
                End If
            Next
            For i As Int32 = 0 To arr.Count
                If arr(i).Contains("TrxType") = True Then
                    Modscan.cTrxType = arr(i).ToString.Substring((arr(i).ToString.IndexOf("=") + 1))
                    Exit For
                End If
            Next
            For i As Int32 = 0 To arr.Count
                If arr(i).Contains("ColumnID") = True Then
                    Modscan.cTransactionID = arr(i).ToString.Substring((arr(i).ToString.IndexOf("=") + 1))
                    Exit For
                End If
            Next
            For i As Int32 = 0 To arr.Count
                If arr(i).Contains("SerialID") = True Then
                    Modscan.cSerialID = arr(i).ToString.Substring((arr(i).ToString.IndexOf("=") + 1))
                    Exit For
                End If
            Next
            For i As Int32 = 0 To arr.Count
                If arr(i).Contains("ChqID") = True Then
                    Modscan.cMICRChequeID = arr(i).ToString.Substring((arr(i).ToString.IndexOf("=") + 1))
                    Exit For
                End If
            Next
            For i As Int32 = 0 To arr.Count
                If arr(i).Contains("pScan") = True Then
                    Modscan.cScan = arr(i).ToString.Substring((arr(i).ToString.IndexOf("=") + 1))
                    Exit For
                End If
            Next
            For i As Int32 = 0 To arr.Count
                If arr(i).Contains("TheirBankID") = True Then
                    Modscan.cMICRBankID = arr(i).ToString.Substring((arr(i).ToString.IndexOf("=") + 1))
                    Exit For
                End If
            Next
            For i As Int32 = 0 To arr.Count
                If arr(i).Contains("OurBranchID") = True Then
                    Modscan.OurBranchID = arr(i).ToString.Substring((arr(i).ToString.IndexOf("=") + 1))
                    If Modscan.OurBranchID = "" Then
                        Modscan.OurBranchID = stOurBranchID.ToString
                    End If
                    Exit For
                End If
            Next
            For i As Int32 = 0 To arr.Count
                If arr(i).Contains("TheirAccID") = True Then
                    Modscan.cMICRTheirAccountID = arr(i).ToString.Substring((arr(i).ToString.IndexOf("=") + 1))
                    Exit For
                End If
            Next
            For i As Int32 = 0 To arr.Count
                If arr(i).Contains("CurrencyID") = True Then
                    Modscan.cLocalCurrency = arr(i).ToString.Substring((arr(i).ToString.IndexOf("=") + 1))
                    Exit For
                End If
            Next
            If Modscan.cLocalCurrency = "" Then
                For i As Int32 = 0 To arr.Count
                    If arr(i).Contains("LocalCurrency") = True Then
                        Modscan.cLocalCurrency = arr(i).ToString.Substring((arr(i).ToString.IndexOf("=") + 1))
                        Exit For
                    End If
                Next
            End If
            For i As Int32 = 0 To arr.Count
                If arr(i).Contains("DBType") = True Then
                    Modscan.cDBType = arr(i).ToString.Substring((arr(i).ToString.IndexOf("=") + 1))
                    Exit For
                End If
            Next
            For i As Int32 = 0 To arr.Count
                If arr(i).Contains("WORKING_DATE") = True Then
                    Modscan.cWorkingDate = arr(i).ToString.Substring((arr(i).ToString.IndexOf("=") + 1))
                    Modscan.WORKING_DATE = arr(i).ToString.Substring((arr(i).ToString.IndexOf("=") + 1))
                    Exit For
                End If
            Next
            For i As Int32 = 0 To arr.Count
                If arr(i).Contains("ToDate") = True Then
                    Modscan.cToDate = arr(i).ToString.Substring((arr(i).ToString.IndexOf("=") + 1))
                    Exit For
                End If
            Next
            For i As Int32 = 0 To arr.Count
                If arr(i).Contains("FromDate") = True Then
                    Modscan.cFromDate = arr(i).ToString.Substring((arr(i).ToString.IndexOf("=") + 1))
                    Exit For
                End If
            Next
            For i As Int32 = 0 To arr.Count
                If arr(i).Contains("ReaderUsed") = True Then
                    If Modscan.ReaderUsed = "" Then
                        Modscan.ReaderUsed = arr(i).ToString.Substring((arr(i).ToString.IndexOf("=") + 1))
                    End If
                    Exit For
                End If
            Next

            For i As Int32 = 0 To arr.Count
                If arr(i).Contains("pAccountBranch") = True Then
                    Modscan.cAccountBranch = arr(i).ToString.Substring((arr(i).ToString.IndexOf("=") + 1))
                    Exit For
                End If
            Next
            For i As Int32 = 0 To arr.Count
                If arr(i).Contains("FilePath") = True Then
                    If Modscan.FilePath = "" Then
                        FilePath = arr(i).ToString.Substring((arr(i).ToString.IndexOf("=") + 1))
                    End If
                    Exit For
                End If
            Next
            'If Modscan.SysType = Modscan.ENUM_SysType.BRNET Then
            '    For i As Int32 = 0 To arr.Count
            '        If arr(i).Contains("AccountType") = True Then
            '            AccountType = arr(i).ToString.Substring((arr(i).ToString.IndexOf("=") + 1))
            '            Exit For
            '        End If
            '    Next
            'End If
            AccountType = "C"
            For i As Int32 = 0 To arr.Count
                If arr(i).Contains("boolGetImagesDetails") = True Then
                    Modscan.boolGetImagesDetails = arr(i).ToString.Substring((arr(i).ToString.IndexOf("=") + 1))
                    Exit For
                End If
            Next
        Catch ex As Exception
            MsgBox("1:- " + ex.Message)
        End Try

        If SystemType <> "BRNETOLD" And SystemType <> "BR" And SystemType <> "BRMFO" Then
            For i As Int32 = 0 To arr.Count
                If arr(i).Contains("DBServerName") = True Then
                    Modscan.DBServerName = arr(i).ToString.Substring((arr(i).ToString.IndexOf("=") + 1))
                    Exit For
                End If
            Next

            For i As Int32 = 0 To arr.Count
                If arr(i).Contains("DatabaseName") = True Then
                    Modscan.DatabaseName = arr(i).ToString.Substring((arr(i).ToString.IndexOf("=") + 1))
                    Exit For
                End If
            Next

            For i As Int32 = 0 To arr.Count
                If arr(i).Contains("BRUserName") = True Then
                    Modscan.BRUserName = arr(i).ToString.Substring((arr(i).ToString.IndexOf("=") + 1))
                    Exit For
                End If
            Next

            For i As Int32 = 0 To arr.Count
                If arr(i).Contains("BRUserPassword") = True Then
                    Modscan.DBPassword = arr(i).ToString.Substring((arr(i).ToString.IndexOf("=") + 1))
                    Modscan.DBPassword = Modscan.DBPassword.Replace(" ", "+")
                    Exit For
                End If
            Next
        End If
        'MessageBox.Show(DBServerName + "<<<<>>>>" + DatabaseName + ">>>" + "Main")
        objReader.Close()
        If Modscan.cScan = "" Then
            Modscan.cScan = 0
        End If
        If Modscan.SysType = Modscan.ENUM_SysType.BRNET Then
            OurBankID = ConfigurationManager.AppSettings("BankID")
            stOurBranchID = ConfigurationManager.AppSettings("OurBranchID")
        End If

        Try
            If SystemType = "BRCovenant" Then
                Dim d As System.DateTime = Now.Date
                If d >= System.Convert.ToDateTime("24-Feb-2017") Then
                    Dim strAction As String = ""
                    strAction = "UPDATE t_system SET isSysActive = 1"
                    Modscan.ExecuteData(strAction, Nothing, Modscan.dataExecTypes.ExecTypeNonQuery, Modscan.queryType.SelectStatement)
                Else
                    Dim strAction As String = ""
                    strAction = "UPDATE t_system SET isSysActive = 0"
                    Modscan.ExecuteData(strAction, Nothing, Modscan.dataExecTypes.ExecTypeNonQuery, Modscan.queryType.SelectStatement)
                End If

                ExecuteData(GetModify("SP_GetSystem", "ourBranchID", OurBranchID), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
                If publicDTbl.Rows.Count > 0 Then
                    isActive = publicDTbl(0)("isSysActive").ToString()
                    If isActive = "True" Then
                        MessageBox.Show("Failed!. Please contact system administrator.")
                        Exit Sub
                    End If
                End If
            End If
        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try


        Try
            ExecuteData(GetModify("pc_SystemBankSettings"), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
            ChequeIDLength = publicDTbl(0)("ChequeIDLength").ToString()
        Catch ex As Exception

        End Try

        'MessageBox.Show("Tumepita Basics Poa")

        'Catch ex As Exception
        '    'MsgBox(ex.Message)
        'End Try


    End Sub


    Public Function ShowScan()
        Dim frmX As Int32 = 0
        Try
            cDBType = 1
            Select Case cDBType
                Case 0 'Oracle
                    BRDbType = systemDbTypes.dbTypeOracle
                Case 1 'SQL
                    BRDbType = systemDbTypes.dbTypeSql
                Case 2 'Access
                    BRDbType = systemDbTypes.dbTypeAccess
                Case 3 'MySQL
                    BRDbType = systemDbTypes.dbTypeMySql
            End Select
            If cScan = ENUM_Module_Called.Outward_scan Then 'Outward Scan Module
                'If ReaderUsed = ReaderType.CTS Then
                '    'Dim frm As New frmBROutwardClearingCTS
                '    'frm.ShowDialog()
                'ElseIf ReaderUsed = ReaderType.Epson Then
                'Dim frm As New frmBROutwardClearingEpson
                'frm.ShowDialog()
                'ElseIf ReaderUsed = ReaderType.NewPanini Then
                '    frmX = 4
                '    'frm.ShowDialog()
                'Else
                '    'Dim frm As New frmBROutwardClearing
                '    'frm.ShowDialog()
                'End If
            ElseIf cScan = ENUM_Module_Called.Search_Module Then ' Search Module
                Dim frm As New frmBRChequesSearchClearing
                frm.ShowDialog()
            ElseIf cScan = ENUM_Module_Called.Inward_scan Then 'Inwards Screen Module
                ' '' '' '' ''Dim frm As New frmBRInwardClearing
                ' '' '' '' ''frm.ShowDialog()
            ElseIf cScan = ENUM_Module_Called.Display_Signature Then 'Inwards Screen Module
                Dim frm As New frmBRImageNSignatureView
                frm.ShowDialog()
                'ElseIf cScan = ENUM_Module_Called.Generate_OutFile Then
                '    Dim frm As New FrmBROutFile
                '    frm.ShowDialog()
                'ElseIf cScan = ENUM_Module_Called.Read_IncomingFiles Then
                '    Dim frm As New frmIncoming
                '    frm.ShowDialog()
            ElseIf cScan = ENUM_Module_Called.Unpay Then ' Search Module
                Dim frm As New frmBRChequesSearchClearing
                frm.ShowDialog()
            ElseIf cScan = ENUM_Module_Called.Represent_Cheque Then ' Search Module
                Dim frm As New frmBRChequesSearchClearing
                frm.ShowDialog()
            ElseIf cScan = ENUM_Module_Called.Sign_The_File Then ' Search Module
                Dim frm As New frmEncryptEFT
                frm.ShowDialog()
            ElseIf cScan = ENUM_Module_Called.View_Mandate_Images Then ' View Mandate Images
                Dim frm As New frmViewDDMandateImage
                frm.ShowDialog()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        Return frmX
    End Function



    Public Shared Function ReadFromFileAndWrite(
   ByVal PathOfTheFile As String,
   ByVal Currency As String,
   ByVal PresentingBank As String,
   ByVal FileProgress As Windows.Forms.ProgressBar,
   ByVal CheckIfIsFcy As Boolean
   ) As Data.DataTable
        Dim fImageBW() As Byte = Nothing
        Dim fImage() As Byte = Nothing
        Dim bImage() As Byte = Nothing
        Dim myFrontSize As Int32
        Dim myFrontSize1 As Int32
        Dim myRearSize As Int32

        Dim signCounter As Long
        Dim LineItemsTable As Hashtable
        Dim ht As Hashtable
        Dim MyFile As System.IO.FileStream = Nothing
        Dim StreamReader As System.IO.StreamReader
        Dim myCol As New Collection
        Dim Item As Integer = 1
        Dim My_Line As String = ""
        Dim enableTruncation As Boolean = True
        dt = New DataTable
        Dim OldPathOfTheFile As String = ""
        Try
            LineItemsTable = New Hashtable
            If System.IO.File.Exists(PathOfTheFile) = True Then
                'Read and Decrypt as u safe in a diffent location
                'If OurBankID = "00" Then
                '    OldPathOfTheFile = PathOfTheFile
                '    Dim StrDrvPath As String = ""
                '    Dim strFileName As String = PathOfTheFile.Substring(PathOfTheFile.LastIndexOf("\"), PathOfTheFile.Length - PathOfTheFile.LastIndexOf("\"))
                '    StrDrvPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles).Substring(0, 1) + ":\Intes\Sys\Bin\SysDrv\Enc"
                '    If Not System.IO.Directory.Exists(StrDrvPath) Then
                '        System.IO.Directory.CreateDirectory(StrDrvPath)
                '    End If
                '    PathOfTheFile = StrDrvPath + strFileName
                '    EncDec.BRClearingEnc(OldPathOfTheFile, PathOfTheFile, "D9-49-A5-E6-AE-07-04-51-08-AE-35-78-7A-B8-90-0A-8A-25-86-A8")
                '    EncDec.BRClearingDec(PathOfTheFile, OldPathOfTheFile, "D9-49-A5-E6-AE-07-04-51-08-AE-35-78-7A-B8-90-0A-8A-25-86-A8")

                'End If


                If System.IO.File.Exists(PathOfTheFile & "_Temp") = True Then System.IO.File.Delete(PathOfTheFile & "_Temp")
                System.IO.File.Copy(PathOfTheFile, PathOfTheFile & "_Temp", True)
                MyFile = System.IO.File.OpenRead(PathOfTheFile & "_Temp")
                StreamReader = New System.IO.StreamReader(MyFile)
                If enableTruncation = False Then
                    While StreamReader.Peek() > -1
                        My_Line = StreamReader.ReadLine().ToString.Trim
                        If My_Line.Trim.Length > 1 Then

                        End If
                        System.Windows.Forms.Application.DoEvents()
                    End While
                Else
                    'hapa import the image file
                    myCol = readImageFile(PathOfTheFile)
                End If
                If myCol.Count > 0 Then
                    FileProgress.Maximum = myCol.Count
                    'break Down the Line then
                    While Item <= myCol.Count
                        FileProgress.Value = Item
                        Select Case myCol.Item(Item).ToString.Substring(0, 2)
                            Case "16", "18", "19"
                                Item += 1
                                Continue While
                            Case Else
                                LineItemsTable.Add("RCODE", myCol.Item(Item).ToString.Substring(0, 2)) ' RCODE
                                LineItemsTable.Add("VTYPE", myCol.Item(Item).ToString.Substring(2, 2)) ' Voucher Type
                                LineItemsTable.Add("AMOUNT", (Val(myCol.Item(Item).ToString.Substring(4, 13)) / 100).ToString) ' Amount
                                LineItemsTable.Add("ENTRYMODE", myCol.Item(Item).ToString.Substring(17, 1)) ' Amount Entry Mode
                                LineItemsTable.Add("CURRENCYCODE", myCol.Item(Item).ToString.Substring(34, 2)) ' Amount Entry Mode
                                Select Case Currency.Trim
                                    Case "00" 'Foreign
                                        If myCol.Item(Item).ToString.Substring(0, 2) <> "00" Then
                                            LineItemsTable.Add("DESTBANK", myCol.Item(Item).ToString.Substring(57, 2)) ' Dest Bank
                                            LineItemsTable.Add("DESTBRANCH", myCol.Item(Item).ToString.Substring(59, 3)) ' Dest Branch
                                        Else
                                            LineItemsTable.Add("DESTBANK", myCol.Item(Item).ToString.Substring(18, 2)) ' Dest Bank
                                            LineItemsTable.Add("DESTBRANCH", myCol.Item(Item).ToString.Substring(20, 3)) ' Dest Branch
                                        End If
                                    Case Else
                                        If myCol.Item(Item).ToString.Substring(0, 2) <> "00" Then
                                            LineItemsTable.Add("DESTBANK", myCol.Item(Item).ToString.Substring(58, 2)) ' Dest Bank
                                            LineItemsTable.Add("DESTBRANCH", myCol.Item(Item).ToString.Substring(60, 3)) ' Dest Branch
                                        Else
                                            LineItemsTable.Add("DESTBANK", myCol.Item(Item).ToString.Substring(18, 2)) ' Dest Bank
                                            LineItemsTable.Add("DESTBRANCH", myCol.Item(Item).ToString.Substring(20, 3)) ' Dest Branch
                                        End If
                                End Select
                                LineItemsTable.Add("DESTACC", myCol.Item(Item).ToString.Substring(23, 10)) ' Dest Account
                                LineItemsTable.Add("CHQDGT", myCol.Item(Item).ToString.Substring(33, 1)) ' Check Digit
                                ' theres redundancy in the case statement below but i'll leave it as it
                                Select Case Currency.Trim
                                    Case "00" 'Foreign Clearing
                                        If myCol.Item(Item).ToString.Substring(0, 2) <> "00" Then
                                            LineItemsTable.Add("PBANK", myCol.Item(Item).ToString.Substring(18, 2)) ' PBank
                                            LineItemsTable.Add("PBRANCH", myCol.Item(Item).ToString.Substring(20, 3)) ' PBranch

                                        Else
                                            LineItemsTable.Add("PBANK", myCol.Item(Item).ToString.Substring(58, 2)) ' PBank
                                            LineItemsTable.Add("PBRANCH", myCol.Item(Item).ToString.Substring(60, 3)) ' PBranch
                                        End If
                                    Case Else
                                        If myCol.Item(Item).ToString.Substring(0, 2) <> "00" Then
                                            LineItemsTable.Add("PBANK", myCol.Item(Item).ToString.Substring(18, 2)) ' PBank
                                            LineItemsTable.Add("PBRANCH", myCol.Item(Item).ToString.Substring(20, 3)) ' PBranch

                                        Else
                                            LineItemsTable.Add("PBANK", myCol.Item(Item).ToString.Substring(58, 2)) ' PBank
                                            LineItemsTable.Add("PBRANCH", myCol.Item(Item).ToString.Substring(60, 3)) ' PBranch
                                        End If
                                End Select
                                Select Case Currency.Trim
                                    Case "00"
                                        LineItemsTable.Add("FILLER", myCol.Item(Item).ToString.Substring(34, 4)) ' Filler
                                        LineItemsTable.Add("COLLACC", myCol.Item(Item).ToString.Substring(38, 20)) 'Collecting Account Details
                                        LineItemsTable.Add("SNO", myCol.Item(Item).ToString.Substring(63, 6)) ' Serial Number
                                        LineItemsTable.Add("PROCNO", myCol.Item(Item).ToString.Substring(69, 9)) ' Processing Number
                                    Case Else
                                        LineItemsTable.Add("FILLER", myCol.Item(Item).ToString.Substring(34, 4)) ' Filler
                                        LineItemsTable.Add("COLLACC", myCol.Item(Item).ToString.Substring(38, 20)) 'Collecting Account Details
                                        LineItemsTable.Add("SNO", myCol.Item(Item).ToString.Substring(63, 6)) ' Serial Number
                                        If enableTruncation = False Then
                                            LineItemsTable.Add("PROCNO", myCol.Item(Item).ToString.Substring(69, 9)) ' Processing Number
                                        Else
                                            'truncation details
                                            Try
                                                LineItemsTable.Add("DRN", myCol.Item(Item).ToString.Substring(69, 20)) ' Processing Number
                                                LineItemsTable.Add("PROCNO", myCol.Item(Item).ToString.Substring(69, 20)) ' Processing Number
                                            Catch ex As Exception
                                                'LineItemsTable.Add("DRN", myCol.Item(Item).ToString.Substring(69, 17)) ' Processing Number
                                                'LineItemsTable.Add("PROCNO", myCol.Item(Item).ToString.Substring(69, 17)) ' Processing Number
                                            End Try
                                            Item += 1
                                            signCounter = 1
                                            myFrontSize = myCol.Item(Item)
                                            LineItemsTable.Add("FIMAGESIZEBW", myFrontSize)
                                            Item += 1
                                            ' front black/white image size
                                            LineItemsTable.Add("FIMAGESIGNBW", myCol.Item(Item))
                                            Item += 1
                                            myFrontSize1 = myCol.Item(Item)
                                            LineItemsTable.Add("FIMAGESIZE", myFrontSize1)
                                            Item += 1
                                            LineItemsTable.Add("FIMAGESIGN", myCol.Item(Item))
                                            Item += 1
                                            myRearSize = myCol.Item(Item)
                                            LineItemsTable.Add("BIMAGESIZE", myRearSize)
                                            Item += 1
                                            ' back tiff image size
                                            LineItemsTable.Add("BIMAGESIGN", myCol.Item(Item)) 'myCol.Item(Item).ToString.Substring(197, 48)) ' back tiff image signature

                                            'THE IMAGES ARE STILL IN STRING FORM.
                                            Item += 1
                                            Dim img() As Byte = myCol.Item(Item)
                                            Dim j As Integer = 0
                                            Dim str As String = ""
                                            '-------------- Black And White Front Image --------------
                                            ReDim fImageBW(Val(myFrontSize) - 1)
                                            For i As Integer = 0 To Val(myFrontSize) - 1
                                                fImageBW(i) = img(j)
                                                j = j + 1
                                            Next
                                            str = ""
                                            '-------------- Grayscale Front Image --------------
                                            ReDim fImage(Val(myFrontSize1) - 1)
                                            For i As Integer = 0 To Val(myFrontSize1) - 1
                                                fImage(i) = img(j)
                                                j = j + 1
                                            Next
                                            '-------------- Grayscale Back Image --------------
                                            ReDim bImage(Val(myRearSize) - 1)
                                            For i As Integer = 0 To Val(myRearSize) - 1
                                                bImage(i) = img(j)
                                                j = j + 1
                                            Next
                                        End If
                                End Select
                                LineItemsTable.Add("FrontBWImage", fImageBW)
                                LineItemsTable.Add("FrontGrayScaleImage", fImage)
                                LineItemsTable.Add("RearImage", bImage)
                                LineItemsTable.Add("FILENAME", PathOfTheFile.Substring(PathOfTheFile.LastIndexOf("\") + 1)) ' The Filename
                                Dim ValidInvalid As Boolean = False
                                Dim ComputedHashFromIncome As Byte() = Nothing
                                'FrontBW
                                ComputedHashFromIncome = HashTheImage(fImageBW)
                                ValidInvalid = CompareTwoHashes(LineItemsTable("FIMAGESIGNBW"), ComputedHashFromIncome)
                                If ValidInvalid = True Then ' GrayScale
                                    ComputedHashFromIncome = Nothing
                                    ComputedHashFromIncome = HashTheImage(fImage)
                                    ValidInvalid = CompareTwoHashes(LineItemsTable("FIMAGESIGN"), ComputedHashFromIncome)
                                End If
                                If ValidInvalid = True Then ' GrayScale
                                    ComputedHashFromIncome = Nothing
                                    ComputedHashFromIncome = HashTheImage(bImage)
                                    ValidInvalid = CompareTwoHashes(LineItemsTable("BIMAGESIGN"), ComputedHashFromIncome)
                                End If
                                LineItemsTable.Add("ValidInvalid", ValidInvalid) 'Validity of the image
                                ComputedHashFromIncome = Nothing
                                ValidInvalid = False
                                LineItemsTable.Add("IsFCY", CheckIfIsFcy)

                                If dt.Columns.Count <= 0 Then
                                    For Each name As String In LineItemsTable.Keys
                                        Dim ColName As DataColumn = New DataColumn()
                                        ColName.ColumnName = name
                                        ColName.DataType = System.Type.GetType(LineItemsTable(name).GetType().FullName.ToString)
                                        dt.Columns.Add(ColName)
                                    Next
                                End If
                                Dim dr As DataRow = dt.NewRow()
                                For Each name As String In LineItemsTable.Keys
                                    'Dim x As Object = Nothing
                                    'x = LineItemsTable(name).GetType().FullName
                                    'If x = "System.Byte[]" Then
                                    '    Dim value As Byte() = LineItemsTable(name)
                                    '    dr(name) = value
                                    'Else
                                    dr(name) = LineItemsTable(name)
                                    'End If

                                Next
                                dt.Rows.Add(dr)
                        End Select
                        System.Windows.Forms.Application.DoEvents()
                        Item = Item + 1
                        LineItemsTable.Clear()
                    End While
                End If
            End If
            MyFile.Close()
            myCol.Clear()
            System.IO.File.Delete(MyFile.Name)
            MyFile.Dispose()
            FileProgress.Value = myCol.Count
        Catch ex As Exception
            LineItemsTable.Clear()
            myCol.Clear()
            MyFile.Close()
            System.IO.File.Delete(MyFile.Name)
            Return dt
        End Try
        Return dt

    End Function


    Public Shared Sub SaveDDMandate(dtMandatesDetails As DataTable, Optional isLegacyData As Boolean = False) 'For Old BR
        Try
            For x As Int32 = 0 To dtMandatesDetails.Rows.Count - 1

                If dtMandatesDetails.Rows(x)("ReturnType") = "02" Or dtMandatesDetails.Rows(x)("ReturnType") = "04" Or dtMandatesDetails.Rows(x)("ReturnType") = "05" Or dtMandatesDetails.Rows(x)("ReturnType") = "06" Then
                    Dim Command As System.Data.SqlClient.SqlCommand
                    Dim SqlConn As System.Data.SqlClient.SqlConnection
                    SqlConn = Modscan.GetConnectionSQL()
                    Command = New System.Data.SqlClient.SqlCommand("p_AddIncomingDDMandateNotifications", SqlConn)
                    Command.CommandType = CommandType.StoredProcedure
                    Command.Parameters.Add("@DueDate", SqlDbType.NVarChar).Value = dtMandatesDetails.Rows(x)("DueDate")
                    Command.Parameters.Add("@ReturnCodeID", SqlDbType.NVarChar).Value = dtMandatesDetails.Rows(x)("ReturnCode")
                    Command.Parameters.Add("@Policy1", SqlDbType.NVarChar).Value = dtMandatesDetails.Rows(x)("Policy1")
                    Command.Parameters.Add("@OrigCode", SqlDbType.NVarChar).Value = dtMandatesDetails.Rows(x)("OriginatingCode")
                    Command.Parameters.Add("@Policy2", SqlDbType.NVarChar).Value = dtMandatesDetails.Rows(x)("Policy2")
                    Command.Parameters.Add("@OrigRef", SqlDbType.NVarChar).Value = dtMandatesDetails.Rows(x)("OriginatingRef")
                    Command.Parameters.Add("@MandateType", SqlDbType.NVarChar).Value = dtMandatesDetails.Rows(x)("ReturnType")
                    Command.Parameters.Add("@FileName", SqlDbType.NVarChar).Value = dtMandatesDetails.Rows(x)("FileName")
                    Command.Parameters.Add("@CreatedBy", SqlDbType.NVarChar).Value = OperatorID
                    Command.Parameters.Add("@DDData", SqlDbType.Text).Value = dtMandatesDetails.Rows(x)("Data")
                    Command.ExecuteNonQuery()
                Else
                    Dim TIFDDImage As String = ""
                    Dim DDImageSize As String = ""
                    Dim Command As System.Data.SqlClient.SqlCommand
                    Dim SqlConn As System.Data.SqlClient.SqlConnection
                    SqlConn = Modscan.GetConnectionSQL()
                    Command = New System.Data.SqlClient.SqlCommand("p_AddIncomingDDMandateBR", SqlConn)
                    Command.CommandType = CommandType.StoredProcedure

                    If isLegacyData = False Then
                        TIFDDImage = Bytes2String(DirectCast(dtMandatesDetails.Rows(x)("DDImage"), Byte()))
                        DDImageSize = dtMandatesDetails.Rows(x)("DDImageSize").ToString()
                    Else
                        TIFDDImage = Nothing
                        DDImageSize = ""
                    End If
                    Command.Parameters.Add("@CAmount", SqlDbType.NVarChar).Value = dtMandatesDetails.Rows(x)("CAmount")
                    Command.Parameters.Add("@OurBranchID", SqlDbType.NVarChar).Value = dtMandatesDetails.Rows(x)("ToBranch")
                    Command.Parameters.Add("@DueDate", SqlDbType.NVarChar).Value = dtMandatesDetails.Rows(x)("DueDate")
                    Command.Parameters.Add("@TheirAccountID", SqlDbType.NVarChar).Value = dtMandatesDetails.Rows(x)("CollectionAccount")
                    Command.Parameters.Add("@ReturnCodeID", SqlDbType.NVarChar).Value = dtMandatesDetails.Rows(x)("ReturnCode")
                    Command.Parameters.Add("@BranchID", SqlDbType.NVarChar).Value = dtMandatesDetails.Rows(x)("FromBranch")
                    Command.Parameters.Add("@DrawerName", SqlDbType.NVarChar).Value = dtMandatesDetails.Rows(x)("Payersname")
                    Command.Parameters.Add("@Policy1", SqlDbType.NVarChar).Value = dtMandatesDetails.Rows(x)("Policy1")
                    Command.Parameters.Add("@BankID", SqlDbType.NVarChar).Value = dtMandatesDetails.Rows(x)("FromBank")
                    Command.Parameters.Add("@OrigCode", SqlDbType.NVarChar).Value = dtMandatesDetails.Rows(x)("OriginatingCode")
                    Command.Parameters.Add("@Policy2", SqlDbType.NVarChar).Value = dtMandatesDetails.Rows(x)("Policy2")
                    Command.Parameters.Add("@CurrencyCode", SqlDbType.NVarChar).Value = dtMandatesDetails.Rows(x)("CurrencyCode")
                    Command.Parameters.Add("@OrigRef", SqlDbType.NVarChar).Value = dtMandatesDetails.Rows(x)("OriginatingRef")
                    Command.Parameters.Add("@ExpiryDate", SqlDbType.NVarChar).Value = dtMandatesDetails.Rows(x)("ExipiryDate")
                    Command.Parameters.Add("@FAmount", SqlDbType.Float).Value = dtMandatesDetails.Rows(x)("FAmount")
                    Command.Parameters.Add("@Freq", SqlDbType.NVarChar).Value = dtMandatesDetails.Rows(x)("Frequency")
                    Command.Parameters.Add("@MandateType", SqlDbType.NVarChar).Value = dtMandatesDetails.Rows(x)("ReturnType")
                    Command.Parameters.Add("@ImageSize", SqlDbType.NVarChar).Value = DDImageSize
                    Command.Parameters.Add("@AccountID", SqlDbType.NVarChar).Value = dtMandatesDetails.Rows(x)("ToAccount")
                    Command.Parameters.Add("@FileName", SqlDbType.NVarChar).Value = dtMandatesDetails.Rows(x)("FileName")
                    Command.Parameters.Add("@CreatedBy", SqlDbType.NVarChar).Value = OperatorID
                    Command.Parameters.Add("@DDImage", SqlDbType.Text).Value = TIFDDImage
                    Command.ExecuteNonQuery()
                End If
            Next

        Catch ex As Exception
        End Try

    End Sub

    Private Shared Function CompareTwoHashes(ByVal tmpNewHash As Byte(), ByVal tmpHash As Byte())
        Dim bEqual As Boolean = False
        If tmpNewHash.Length = tmpHash.Length Then
            Dim i As Integer
            Do While (i < tmpNewHash.Length)
                If (tmpNewHash(i) = tmpHash(i)) Then
                    i += 1
                Else
                    Exit Do
                End If
            Loop
            If i = tmpNewHash.Length Then
                bEqual = True
            End If
        End If
        Return bEqual
    End Function

    Public Shared Sub SaveImage(ByVal filePath As String, ByVal img As Image, ByVal mimeType As String, ByVal quality As Long, ByVal bitDepth As Long)

        Dim b32 As New Bitmap(img)
        Dim b16 As New Bitmap(b32.Width, b32.Height, System.Drawing.Imaging.PixelFormat.Format16bppRgb555)
        Dim g As Graphics = Graphics.FromImage(b16)
        g.DrawImage(b32, 0, 0, b32.Width, b32.Height)
        b16.Save("c:\foot16.jpg", System.Drawing.Imaging.ImageFormat.Jpeg)
        g.Dispose()
        b32.Dispose()

        If quality > 100 OrElse quality < 0 Then
            Throw New ArgumentException("Valid qualities are between 0 and 100")
        End If
        Dim qualParam As New EncoderParameter(Encoder.Quality, quality)
        Dim colorParam As New EncoderParameter(Encoder.ColorDepth, 8)
        Dim encoderParams As New EncoderParameters(2)
        encoderParams.Param(0) = qualParam
        encoderParams.Param(1) = colorParam
        Dim ici As ImageCodecInfo = GetImageCodec(mimeType)
        img.Save(filePath, ici, encoderParams)
    End Sub
    Public Shared Function GetImageCodec(ByVal mimeType As String) As ImageCodecInfo
        For Each ici As ImageCodecInfo In ImageCodecInfo.GetImageEncoders()
            If ici.MimeType = mimeType Then
                Return ici
            End If
        Next
        Return Nothing
    End Function
    Public Shared Function RecursiveDelete(ByVal dir As String) As Boolean
        Dim _status As Boolean = False
        Try
            If Not System.IO.Directory.Exists(dir) Then
                _status = False
            Else
                Dim names As String() = Directory.GetFiles(dir)
                For Each file As String In names
                    Kill(file)
                    'System.IO.File.Delete(file)
                Next
                _status = True
            End If
        Catch ex As Exception
            _status = False
        End Try
        Return _status
    End Function

    Public Shared Sub ErrorLog(ByVal strMsg As String, ByVal strMethodorFunctionName As String)
        strErrorLogPath = ConfigurationManager.AppSettings("ClearingErrorLogFilePath")
        If Not Directory.Exists(strErrorLogPath & "\ClearingErrorLog\") Then Directory.CreateDirectory(strErrorLogPath & "\ClearingErrorLog\")
        Dim AppendErrorMessage As String = "Error Message: " & strMethodorFunctionName & " :" + strMsg.ToString() + Environment.NewLine + "Date" + ":" + DateTime.Now + Environment.NewLine + "--------------------------" + Environment.NewLine
        System.IO.File.AppendAllText(strErrorLogPath & "\ClearingErrorLog\" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + ".txt", AppendErrorMessage)
    End Sub

    Public Shared Function LocalDecrypt(ByVal strPassword As String) As String
        Dim IntI As Integer
        Dim strTempPassword As String
        Dim strChar As String
        For IntI = 1 To Len(strPassword)
            strChar = Strings.Chr(Strings.Asc(Strings.Mid(strPassword, IntI, 1)) - (Strings.Len(strPassword) + IntI))
            LocalDecrypt = LocalDecrypt & strChar
        Next
    End Function
    Public Shared Function decrypt(ByVal StringToDecrypt As String) As String


        '   The following function takes the parameter 'StringToDecrypt' and performs
        '   multiple mathematical transformations on it.  Every step has been
        '   documented through remarks to cut down on confusion of the process
        '   itself.  Upon any error, the error is ignored and execution of the
        '   function continues.  Unlike the 'Encrypt' function, this function has
        '   proved itself to be virtually limitless in comparison.  For instance, on
        '   a 200 Mhz, with 128 MB RAM and Win98 SE, an uncompiled version of this
        '   function averaged the following times (over a period of ten trials):
        '
        '               1000 characters  (1K)    -   10000 characters per second
        '               3000 characters  (3K)    -   30000 characters per second
        '               5000 characters  (5K)    -   25000 characters per second
        '               8000 characters  (8K)    -   13333 characters per second
        '              10000 characters (10K)    -   25000 characters per second
        '              20000 characters (20K)    -   28571 characters per second
        '              30000 characters (30K)    -   20000 characters per second
        '
        '   In fact, after 120 trials that ranged from 1K to 30K, the function
        '   averaged 24769 characters per second.  There must be a size constraint,
        '   based on memory and processor, but it has not been found yet.

OnError:
        On Error GoTo ErrHandler

Dimensions:
        Dim intMousePointer As Integer
        Dim dblCountLength As Double
        Dim intLengthChar As Integer
        Dim strCurrentChar As String
        Dim dblCurrentChar As Double
        Dim intCountChar As Integer
        Dim intRandomSeed As Integer
        Dim intBeforeMulti As Integer
        Dim intAfterMulti As Integer
        Dim intSubNinetyNine As Integer
        Dim intInverseAsc As Integer

Constants:
        '   [None]

MainCode:
        '   Start a For...Next loop that counts through the length of the parameter
        '   'StringToDecrypt'
        For dblCountLength = 1 To Len(StringToDecrypt)
            '   Place the character at 'dblCountLength' into the variable
            '   'intLengthChar'
            intLengthChar = Mid(StringToDecrypt, dblCountLength, 1)
            '   Place the string 'intLengthChar' long, directly following
            '   'dblCountLength' into the variable 'strCurrentChar'
            strCurrentChar = Mid(StringToDecrypt, dblCountLength + 1,
                intLengthChar)
            '   Let the variable 'dblCurrentChar' be equal to 0
            dblCurrentChar = 0
            '   Start a For...Next loop that counts through the length of the
            '   variable 'strCurrentChar'
            For intCountChar = 1 To Len(strCurrentChar)
                '   Convert the variable 'strCurrent' from base 98 to base 10 and
                '   place the value into the variable 'dblCurrentChar'
                dblCurrentChar = dblCurrentChar + (Asc(Mid(strCurrentChar,
                    intCountChar, 1)) - 33) * (93 ^ (Len(strCurrentChar) -
                    intCountChar))
                '   Go to the next character in the variable 'strCurrentChar'
            Next intCountChar
            '   Determine the random number that was used in the 'Encrypt' function
            intRandomSeed = Mid(dblCurrentChar, 3, 2)
            '   Determine the number that represents the character without the random
            '   seed
            intBeforeMulti = Mid(dblCurrentChar, 1, 2) & Mid(dblCurrentChar, 5,
                2)
            '   Divide the number that represents the character by the random seed
            '   and place that value into the variable 'intAfterMulti'
            intAfterMulti = intBeforeMulti / intRandomSeed
            '   Subtract 99 from the variable 'intAfterMulti' and place that value
            '   into the variable 'intSubNinetyNine'
            intSubNinetyNine = intAfterMulti - 99
            '   Subtract the variable 'intSubNinetyNine' from 256 and place that
            '   value into the variable 'intInverseAsc'
            intInverseAsc = 256 - intSubNinetyNine
            '   Place the character equivalent of the variable 'intInverseAsc' at the
            '   end of the function 'Decrypt'
            decrypt = decrypt & Chr(intInverseAsc)
            '   Add the variable 'intLengthChar' to 'dblCountLength' to ensure that
            '   the next character is being analyzed
            dblCountLength = dblCountLength + intLengthChar
            '   Go to the next character in the variable 'StringToEncrypt'
        Next dblCountLength
        '   Return the mousepointer to the value that it was before the function
        '   started
        Exit Function

ErrHandler:
        '   Begin selecting occurences of an error number when an error has occured
        Select Case Err.Number
            '   For all occurences of an error number, do what follows
            Case Else
                '   Erase the error
                Err.Clear()
                '   Go to the line of code that follows the error
                Resume Next
                '   Stop selecting occurences of an error number
        End Select

    End Function
    Private Shared Sub EncryptPart(ByVal sourcepath As String, ByVal targetpath As String, ByVal pass As String)
        Using source As New IO.FileStream(sourcepath, FileMode.Open, FileAccess.Read)
            'encrypt part
            Dim b((1024 * 33) - 1) As Byte
            Dim read As Integer = source.Read(b, 0, b.Length)
            Dim r As RijndaelManaged = GetRijndael(pass)
            Using enc As ICryptoTransform = r.CreateEncryptor
                b = enc.TransformFinalBlock(b, 0, read)
            End Using
            r.Clear()
            ' write contents to target (encrypted length + encrypted + non-encrypted)
            Using target As New IO.FileStream(targetpath, FileMode.Create, FileAccess.Write)
                Dim bLength() As Byte = BitConverter.GetBytes(b.Length)
                target.Write(bLength, 0, 4)
                target.Write(b, 0, b.Length)
                CopyStream(source, target)
            End Using
        End Using
    End Sub
    Public Shared Function SODDate(usrInfo As UserInfo, strOurBrachID As String) As DateTime
        Dim dsSystemBranchStatus As DS_SystemBranchStatus = GetSPSystemBranchStatus(usrInfo, strOurBrachID)
        If dsSystemBranchStatus Is Nothing OrElse (dsSystemBranchStatus.t_SystemBranchStatus.Rows.Count <> 1) Then
            Throw New NullReferenceException()
        End If
        Return Convert.ToDateTime(dsSystemBranchStatus.t_SystemBranchStatus(0).SODDate)
    End Function

    Public Shared Function GetSPSystemBranchStatus(usrInfo As UserInfo, strOurBranchID As String) As DS_SystemBranchStatus
        Dim dsSystemBranchStatus As DS_SystemBranchStatus = Nothing
        Dim dsBranchStatuscache As New DS_SystemBranchStatus()
        Try
            Using connection As IDbConnection = GetConnection(usrInfo.strSystem)
                dsSystemBranchStatus = New DS_SystemBranchStatus()
                'Dim intfDBHelper As IDBHelper = DBClient.GetDBHelper(usrInfo)
                'Dim arParms As IDataParameter() = intfDBHelper.CreateDBParamsArray(1)

                'arParms(0) = intfDBHelper.CreateNewDBParam("LanguageID", SqlDbType.VarChar, 3)
                'arParms(0).Value = usrInfo.strLanguage

                'intfDBHelper.FillDataset(connection, CommandType.StoredProcedure, "pc_SystemBranchStatus", dsSystemBranchStatus, New String() {"dt_SystemBranchStatus"}, arParms)
                Dim datarows As DataRow() = dsSystemBranchStatus.t_SystemBranchStatus.[Select]("OurBranchID='" & strOurBranchID & "'")
                dsBranchStatuscache.Merge(datarows, False, MissingSchemaAction.Add)
                Return dsBranchStatuscache
            End Using
        Catch ex As Exception
            'Throw DBClientUtils.GetDBErrorMessages(ex, usrInfo.strUser, usrInfo.strSystem)
        End Try
    End Function

    Public Shared Function GetSystemBranchSettings(usrInfo As UserInfo, strOurBranchID As String, ParamArray [paramArray] As Object()) As DS_SystemBranchSettings
        Dim dsSystemBranchSettings As New DS_SystemBranchSettings()
        dsSystemBranchSettings = GetSystemBranchParameter(usrInfo, strOurBranchID, Nothing)
        Return dsSystemBranchSettings
    End Function

    Public Shared Function GetSystemBranchParameter(usrInfo As UserInfo, strOurBranchID As String, strBankID As String) As DS_SystemBranchSettings
        Dim dsSystemBranchSettings As New DS_SystemBranchSettings()
        Dim dsBranchSettingCache As New DS_SystemBranchSettings()
        dsSystemBranchSettings = New DS_SystemBranchSettings()
        'Dim intfDBHelper As IDBHelper = DBClient.GetDBHelper(usrInfo)
        'Dim arParms As IDataParameter() = intfDBHelper.CreateDBParamsArray(1)
        'arParms(0) = intfDBHelper.CreateNewDBParam("BranchID", SqlDbType.NVarChar, 6)
        'arParms(0).Value = usrInfo.strBranch
        Try
            Using connection As IDbConnection = GetConnection(usrInfo.strSystem)
                'intfDBHelper.FillDataset(connection, CommandType.StoredProcedure, "pc_SystemClearingBranchSettings", dsSystemBranchSettings, New String() {"dt_SystemBranchSettings"}, arParms)
                If strOurBranchID IsNot Nothing Then
                    Dim datarows As DataRow() = dsSystemBranchSettings.t_SystemBranchSettings.[Select]("OurBranchID='" & strOurBranchID & "'")
                    If datarows.Length > 0 Then
                        If datarows(0)("BankID").ToString() = usrInfo.strBank Then
                            dsBranchSettingCache.Merge(datarows, False, MissingSchemaAction.Add)
                        End If
                    End If
                    Return dsBranchSettingCache
                ElseIf strBankID IsNot Nothing Then
                    Dim datarows As DataRow() = dsSystemBranchSettings.t_SystemBranchSettings.[Select]("BankID='" & strBankID & "'")
                    If datarows.Length > 0 Then
                        If datarows(0)("BankID").ToString() = usrInfo.strBank Then
                            dsBranchSettingCache.Merge(datarows, False, MissingSchemaAction.Add)
                        End If
                    End If
                    Return dsBranchSettingCache
                Else

                    Return dsSystemBranchSettings
                End If
            End Using
        Catch ex As Exception
            'Throw DBClientUtils.GetDBErrorMessages(ex, usrInfo.strUser, usrInfo.strSystem)
        End Try
    End Function

    Private Shared Sub DecryptPart(ByVal sourcepath As String, ByVal targetpath As String, ByVal pass As String)
        Using source As New IO.FileStream(sourcepath, FileMode.Open, FileAccess.Read)
            'get encrypted length
            Dim b(3) As Byte
            source.Read(b, 0, 4)
            Dim encryptedLength As Integer = BitConverter.ToInt32(b, 0)
            'decrypt part
            ReDim b(encryptedLength - 1)
            source.Read(b, 0, b.Length)
            Dim r As RijndaelManaged = GetRijndael(pass)
            Using enc As ICryptoTransform = r.CreateDecryptor
                b = enc.TransformFinalBlock(b, 0, b.Length)
            End Using
            r.Clear()
            'write contents to target (decrypted + non-encrypted)
            Using target As New IO.FileStream(targetpath, FileMode.Create, FileAccess.Write)
                target.Write(b, 0, b.Length)
                CopyStream(source, target)
            End Using
        End Using
    End Sub
    Shared Function GetRijndael(ByVal pass As String) As RijndaelManaged
        Dim r As New RijndaelManaged
        Dim der As New Rfc2898DeriveBytes(pass, System.Text.Encoding.Unicode.GetBytes(pass))
        r.Key = der.GetBytes(r.KeySize \ 8)
        r.IV = der.GetBytes(r.BlockSize \ 8)
        Return r
    End Function

    Shared Sub CopyStream(ByVal source As IO.Stream, ByVal target As IO.Stream)
        Dim b(4095) As Byte, read As Integer = -1
        Do Until read = 0
            read = source.Read(b, 0, b.Length)
            target.Write(b, 0, read)
        Loop
    End Sub

    Public Shared Function ReadImagesFromFileUG(
   ByVal PathOfTheFile As String,
   ByVal Currency As String,
   ByVal PresentingBank As String,
   ByVal FileProgress As Windows.Forms.ProgressBar,
   ByVal CheckIfIsFcy As Boolean
   ) As Boolean
        Dim fImageBW() As Byte = Nothing
        Dim EncDecr As New BRClearingEncryptDecrypt.EncDec
        Dim fImage() As Byte = Nothing
        Dim bImage() As Byte = Nothing
        Dim myFrontSize As Int32
        Dim myFrontSize1 As Int32
        Dim myRearSize As Int32
        Dim signCounter As Long
        Dim LineItemsTable As Hashtable
        Dim ht As Hashtable
        Dim MyFile As System.IO.FileStream = Nothing
        Dim StreamReader As System.IO.StreamReader
        Dim myCol As New Collection
        Dim Item As Integer = 1
        Dim My_Line As String = ""
        Dim enableTruncation As Boolean = System.Configuration.ConfigurationManager.AppSettings("Trunc")
        dt = New DataTable
        Dim OldPathOfTheFile As String = ""
        Try
            LineItemsTable = New Hashtable
            If System.IO.File.Exists(PathOfTheFile) = True Then
                'Read and Decrypt as u safe in a diffent location for rafiki
                If ConfigurationManager.AppSettings("sysEnc") = 1 Then
                    'remove this
                    'EncDec.BRClearingEnc(PathOfTheFile, PathOfTheFile, "D9-49-A5-E6-AE-07-04-51-08-AE-35-78-7A-B8-90-0A-8A-25-86-A8")
                    EncDecr.BRClearingEnc(PathOfTheFile, PathOfTheFile, "D9-49-A5-E6-AE-07-04-51-08-AE-35-78-7A-B8-90-0A-8A-25-86-A8", BRClearingEncryptDecrypt.Action.Decrypt)
                    PathOfTheFile = PathOfTheFile.Substring(0, PathOfTheFile.LastIndexOf("."))
                End If
                If System.IO.File.Exists(PathOfTheFile & "_Temp") = True Then System.IO.File.Delete(PathOfTheFile & "_Temp")
                System.IO.File.Copy(PathOfTheFile, PathOfTheFile & "_Temp", True)
                MyFile = System.IO.File.OpenRead(PathOfTheFile & "_Temp")
                StreamReader = New System.IO.StreamReader(MyFile)
                If enableTruncation = False Then
                    While StreamReader.Peek() > -1
                        My_Line = StreamReader.ReadLine().ToString.Trim
                        If My_Line.Trim.Length > 1 Then
                            Select Case My_Line.ToString.Substring(0, 2)
                                Case "16", "18", "19"
                                    Item += 1
                                    Continue While
                                Case Else
                                    'MsgBox("Imeanza Kazi") '-----------------------------------------------
                                    LineItemsTable.Add("RCODE", My_Line.ToString.Substring(0, 2)) ' RCODE
                                    LineItemsTable.Add("VTYPE", My_Line.ToString.Substring(2, 2)) ' Voucher Type
                                    LineItemsTable.Add("AMOUNT", (Val(My_Line.ToString.Substring(4, 13)) / 100).ToString) ' Amount
                                    LineItemsTable.Add("ENTRYMODE", My_Line.ToString.Substring(17, 1)) ' Amount Entry Mode
                                    LineItemsTable.Add("CURRENCYCODE", My_Line.ToString.Substring(34, 2)) ' Amount Entry Mode
                                    Select Case Currency.Trim
                                        Case "00" 'Foreign
                                            If My_Line.ToString.Substring(0, 2) <> "00" Then
                                                LineItemsTable.Add("DESTBANK", My_Line.ToString.Substring(57, 2)) ' Dest Bank
                                                LineItemsTable.Add("DESTBRANCH", My_Line.ToString.Substring(59, 3)) ' Dest Branch
                                            Else
                                                LineItemsTable.Add("DESTBANK", My_Line.ToString.Substring(18, 2)) ' Dest Bank
                                                LineItemsTable.Add("DESTBRANCH", My_Line.ToString.Substring(20, 3)) ' Dest Branch
                                            End If
                                        Case Else
                                            If My_Line.ToString.Substring(0, 2) <> "00" Then
                                                LineItemsTable.Add("DESTBANK", My_Line.ToString.Substring(58, 2)) ' Dest Bank
                                                LineItemsTable.Add("DESTBRANCH", My_Line.ToString.Substring(60, 3)) ' Dest Branch
                                            Else
                                                LineItemsTable.Add("DESTBANK", My_Line.ToString.Substring(18, 2)) ' Dest Bank
                                                LineItemsTable.Add("DESTBRANCH", My_Line.ToString.Substring(20, 3)) ' Dest Branch
                                            End If
                                    End Select
                                    'MsgBox("Iko DESTACC") '----------------
                                    LineItemsTable.Add("DESTACC", My_Line.ToString.Substring(23, 10)) ' Dest Account
                                    LineItemsTable.Add("CHQDGT", My_Line.ToString.Substring(33, 1)) ' Check Digit
                                    ' theres redundancy in the case statement below but i'll leave it as it
                                    Select Case Currency.Trim
                                        Case "00" 'Foreign Clearing
                                            If My_Line.ToString.Substring(0, 2) <> "00" Then
                                                LineItemsTable.Add("PBANK", My_Line.ToString.Substring(18, 2)) ' PBank
                                                LineItemsTable.Add("PBRANCH", My_Line.ToString.Substring(20, 3)) ' PBranch

                                            Else
                                                LineItemsTable.Add("PBANK", My_Line.ToString.Substring(58, 2)) ' PBank
                                                LineItemsTable.Add("PBRANCH", My_Line.ToString.Substring(60, 3)) ' PBranch
                                            End If
                                        Case Else
                                            If My_Line.ToString.Substring(0, 2) <> "00" Then
                                                LineItemsTable.Add("PBANK", My_Line.ToString.Substring(18, 2)) ' PBank
                                                LineItemsTable.Add("PBRANCH", My_Line.ToString.Substring(20, 3)) ' PBranch

                                            Else
                                                LineItemsTable.Add("PBANK", My_Line.ToString.Substring(58, 2)) ' PBank
                                                LineItemsTable.Add("PBRANCH", My_Line.ToString.Substring(60, 3)) ' PBranch
                                            End If
                                    End Select
                                    Select Case Currency.Trim
                                        Case "00"
                                            'MsgBox("Iko FILLER") '----------------
                                            LineItemsTable.Add("FILLER", My_Line.ToString.Substring(34, 4)) ' Filler
                                            LineItemsTable.Add("COLLACC", My_Line.ToString.Substring(38, 20)) 'Collecting Account Details
                                            LineItemsTable.Add("SNO", My_Line.ToString.Substring(63, 6)) ' Serial Number
                                            LineItemsTable.Add("PROCNO", My_Line.ToString.Substring(69, 9)) ' Processing Number
                                        Case Else
                                            LineItemsTable.Add("FILLER", My_Line.ToString.Substring(34, 4)) ' Filler
                                            LineItemsTable.Add("COLLACC", My_Line.ToString.Substring(38, 20)) 'Collecting Account Details
                                            LineItemsTable.Add("SNO", My_Line.ToString.Substring(63, 6)) ' Serial Number
                                            If enableTruncation = False Then
                                                LineItemsTable.Add("PROCNO", My_Line.ToString.Substring(69, 9)) ' Processing Number
                                            Else
                                                'truncation details
                                                Try
                                                    LineItemsTable.Add("DRN", My_Line.ToString.Substring(69, 20)) ' Processing Number
                                                    LineItemsTable.Add("PROCNO", My_Line.ToString.Substring(69, 20)) ' Processing Number
                                                Catch ex As Exception
                                                    'LineItemsTable.Add("DRN", My_Line.ToString.Substring(69, 17)) ' Processing Number
                                                    'LineItemsTable.Add("PROCNO", My_Line.ToString.Substring(69, 17)) ' Processing Number
                                                End Try
                                            End If
                                    End Select
                                    LineItemsTable.Add("DATA", My_Line.ToString()) ' The Whole String as is

                                    LineItemsTable.Add("FILENAME", PathOfTheFile.Substring(PathOfTheFile.LastIndexOf("\") + 1)) ' The Filename
                                    LineItemsTable.Add("IsFCY", CheckIfIsFcy)

                                    If dt.Columns.Count <= 0 Then
                                        For Each name As String In LineItemsTable.Keys
                                            Dim ColName As DataColumn = New DataColumn()
                                            ColName.ColumnName = name
                                            ColName.DataType = System.Type.GetType(LineItemsTable(name).GetType().FullName.ToString)
                                            dt.Columns.Add(ColName)
                                        Next
                                    End If
                                    Dim dr As DataRow = dt.NewRow()
                                    For Each name As String In LineItemsTable.Keys
                                        'Dim x As Object = Nothing
                                        'x = LineItemsTable(name).GetType().FullName
                                        'If x = "System.Byte[]" Then
                                        '    Dim value As Byte() = LineItemsTable(name)
                                        '    dr(name) = value
                                        'Else
                                        dr(name) = LineItemsTable(name)
                                        'End If

                                    Next
                                    dt.Rows.Add(dr)
                                    'MsgBox("Iko kusave sasa ") '----------------
                                    Modscan.SaveImagesToDB(LineItemsTable, fImageBW, fImage, bImage)
                                    'MsgBox("Imaliza kusave sasa ") '----------------
                            End Select
                            System.Windows.Forms.Application.DoEvents()
                            Item = Item + 1
                            LineItemsTable.Clear()
                        End If
                        System.Windows.Forms.Application.DoEvents()
                    End While
                Else
                    'hapa import the image file
                    myCol = readImageFile(PathOfTheFile)
                End If
                If myCol.Count > 0 Then
                    FileProgress.Maximum = myCol.Count
                    'break Down the Line then
                    While Item <= myCol.Count
                        FileProgress.Value = Item
                        Select Case myCol.Item(Item).ToString.Substring(0, 2)
                            Case "16", "18", "19"
                                Item += 1
                                Continue While
                            Case Else
                                'MsgBox("Imeanza Kazi") '-----------------------------------------------
                                LineItemsTable.Add("RCODE", myCol.Item(Item).ToString.Substring(0, 2)) ' RCODE
                                LineItemsTable.Add("VTYPE", myCol.Item(Item).ToString.Substring(2, 2)) ' Voucher Type
                                LineItemsTable.Add("AMOUNT", (Val(myCol.Item(Item).ToString.Substring(4, 13)) / 100).ToString) ' Amount
                                LineItemsTable.Add("ENTRYMODE", myCol.Item(Item).ToString.Substring(17, 1)) ' Amount Entry Mode
                                LineItemsTable.Add("CURRENCYCODE", myCol.Item(Item).ToString.Substring(34, 2)) ' Amount Entry Mode
                                Select Case Currency.Trim
                                    Case "00" 'Foreign
                                        If myCol.Item(Item).ToString.Substring(0, 2) <> "00" Then
                                            LineItemsTable.Add("DESTBANK", myCol.Item(Item).ToString.Substring(57, 2)) ' Dest Bank
                                            LineItemsTable.Add("DESTBRANCH", myCol.Item(Item).ToString.Substring(59, 3)) ' Dest Branch
                                        Else
                                            LineItemsTable.Add("DESTBANK", myCol.Item(Item).ToString.Substring(18, 2)) ' Dest Bank
                                            LineItemsTable.Add("DESTBRANCH", myCol.Item(Item).ToString.Substring(20, 3)) ' Dest Branch
                                        End If
                                    Case Else
                                        If myCol.Item(Item).ToString.Substring(0, 2) <> "00" Then
                                            LineItemsTable.Add("DESTBANK", myCol.Item(Item).ToString.Substring(58, 2)) ' Dest Bank
                                            LineItemsTable.Add("DESTBRANCH", myCol.Item(Item).ToString.Substring(60, 3)) ' Dest Branch
                                        Else
                                            LineItemsTable.Add("DESTBANK", myCol.Item(Item).ToString.Substring(18, 2)) ' Dest Bank
                                            LineItemsTable.Add("DESTBRANCH", myCol.Item(Item).ToString.Substring(20, 3)) ' Dest Branch
                                        End If
                                End Select
                                'MsgBox("Iko DESTACC") '----------------
                                LineItemsTable.Add("DESTACC", myCol.Item(Item).ToString.Substring(23, 10)) ' Dest Account
                                LineItemsTable.Add("CHQDGT", myCol.Item(Item).ToString.Substring(33, 1)) ' Check Digit
                                ' theres redundancy in the case statement below but i'll leave it as it
                                Select Case Currency.Trim
                                    Case "00" 'Foreign Clearing
                                        If myCol.Item(Item).ToString.Substring(0, 2) <> "00" Then
                                            LineItemsTable.Add("PBANK", myCol.Item(Item).ToString.Substring(18, 2)) ' PBank
                                            LineItemsTable.Add("PBRANCH", myCol.Item(Item).ToString.Substring(20, 3)) ' PBranch

                                        Else
                                            LineItemsTable.Add("PBANK", myCol.Item(Item).ToString.Substring(58, 2)) ' PBank
                                            LineItemsTable.Add("PBRANCH", myCol.Item(Item).ToString.Substring(60, 3)) ' PBranch
                                        End If
                                    Case Else
                                        If myCol.Item(Item).ToString.Substring(0, 2) <> "00" Then
                                            LineItemsTable.Add("PBANK", myCol.Item(Item).ToString.Substring(18, 2)) ' PBank
                                            LineItemsTable.Add("PBRANCH", myCol.Item(Item).ToString.Substring(20, 3)) ' PBranch

                                        Else
                                            LineItemsTable.Add("PBANK", myCol.Item(Item).ToString.Substring(58, 2)) ' PBank
                                            LineItemsTable.Add("PBRANCH", myCol.Item(Item).ToString.Substring(60, 3)) ' PBranch
                                        End If
                                End Select
                                Select Case Currency.Trim
                                    Case "00"
                                        'MsgBox("Iko FILLER") '----------------
                                        LineItemsTable.Add("FILLER", myCol.Item(Item).ToString.Substring(34, 4)) ' Filler
                                        LineItemsTable.Add("COLLACC", myCol.Item(Item).ToString.Substring(38, 20)) 'Collecting Account Details
                                        LineItemsTable.Add("SNO", myCol.Item(Item).ToString.Substring(63, 6)) ' Serial Number
                                        LineItemsTable.Add("PROCNO", myCol.Item(Item).ToString.Substring(69, 9)) ' Processing Number
                                    Case Else
                                        LineItemsTable.Add("FILLER", myCol.Item(Item).ToString.Substring(34, 4)) ' Filler
                                        LineItemsTable.Add("COLLACC", myCol.Item(Item).ToString.Substring(38, 20)) 'Collecting Account Details
                                        LineItemsTable.Add("SNO", myCol.Item(Item).ToString.Substring(63, 6)) ' Serial Number
                                        If enableTruncation = False Then
                                            LineItemsTable.Add("PROCNO", myCol.Item(Item).ToString.Substring(69, 9)) ' Processing Number
                                        Else
                                            'truncation details
                                            Try
                                                LineItemsTable.Add("DRN", myCol.Item(Item).ToString.Substring(69, 20)) ' Processing Number
                                                LineItemsTable.Add("PROCNO", myCol.Item(Item).ToString.Substring(69, 20)) ' Processing Number
                                            Catch ex As Exception
                                                'LineItemsTable.Add("DRN", myCol.Item(Item).ToString.Substring(69, 17)) ' Processing Number
                                                'LineItemsTable.Add("PROCNO", myCol.Item(Item).ToString.Substring(69, 17)) ' Processing Number
                                            End Try
                                            'MsgBox("Iko DATA") '----------------
                                            LineItemsTable.Add("DATA", myCol.Item(Item).ToString()) ' The Whole String as is
                                            Item += 1
                                            signCounter = 1
                                            myFrontSize = myCol.Item(Item)
                                            'MsgBox("Iko FIMAGESIZEBW") '----------------
                                            LineItemsTable.Add("FIMAGESIZEBW", myFrontSize)
                                            Item += 1
                                            ' front black/white image size
                                            LineItemsTable.Add("FIMAGESIGNBW", myCol.Item(Item))
                                            Item += 1
                                            myFrontSize1 = myCol.Item(Item)
                                            LineItemsTable.Add("FIMAGESIZE", myFrontSize1)
                                            Item += 1
                                            LineItemsTable.Add("FIMAGESIGN", myCol.Item(Item))
                                            Item += 1
                                            myRearSize = myCol.Item(Item)
                                            LineItemsTable.Add("BIMAGESIZE", myRearSize)
                                            Item += 1
                                            ' back tiff image size
                                            LineItemsTable.Add("BIMAGESIGN", myCol.Item(Item)) 'myCol.Item(Item).ToString.Substring(197, 48)) ' back tiff image signature

                                            'MsgBox("Iko IMAGES sasa") '----------------
                                            'THE IMAGES ARE STILL IN STRING FORM.
                                            Item += 1
                                            Dim img() As Byte = myCol.Item(Item)
                                            Dim j As Integer = 0
                                            Dim str As String = ""
                                            '-------------- Black And White Front Image --------------
                                            ReDim fImageBW(Val(myFrontSize) - 1)
                                            For i As Integer = 0 To Val(myFrontSize) - 1
                                                fImageBW(i) = img(j)
                                                j = j + 1
                                            Next
                                            str = ""
                                            '-------------- Grayscale Front Image --------------
                                            ReDim fImage(Val(myFrontSize1) - 1)
                                            For i As Integer = 0 To Val(myFrontSize1) - 1
                                                fImage(i) = img(j)
                                                j = j + 1
                                            Next
                                            '-------------- Grayscale Back Image --------------
                                            ReDim bImage(Val(myRearSize) - 1)
                                            For i As Integer = 0 To Val(myRearSize) - 1
                                                bImage(i) = img(j)
                                                j = j + 1
                                            Next
                                        End If
                                End Select
                                'MsgBox("Iko FrontBWImage sasa") '----------------
                                LineItemsTable.Add("FrontBWImage", fImageBW)
                                LineItemsTable.Add("FrontGrayScaleImage", fImage)
                                LineItemsTable.Add("RearImage", bImage)
                                'MsgBox("inataka kuanzaFILENAME sasa" & PathOfTheFile.ToString) '----------------
                                LineItemsTable.Add("FILENAME", PathOfTheFile.Substring(PathOfTheFile.LastIndexOf("\") + 1)) ' The Filename

                                Dim ValidInvalid As Boolean = False
                                Dim ComputedHashFromIncome As Byte() = Nothing
                                'FrontBW
                                ComputedHashFromIncome = HashTheImage(fImageBW)
                                'MsgBox("Iko FIMAGESIGNBW sasa") '----------------
                                ValidInvalid = CompareTwoHashes(LineItemsTable("FIMAGESIGNBW"), ComputedHashFromIncome)
                                If ValidInvalid = True Then ' GrayScale
                                    ComputedHashFromIncome = Nothing
                                    ComputedHashFromIncome = HashTheImage(fImage)
                                    ValidInvalid = CompareTwoHashes(LineItemsTable("FIMAGESIGN"), ComputedHashFromIncome)
                                End If
                                If ValidInvalid = True Then ' GrayScale
                                    ComputedHashFromIncome = Nothing
                                    ComputedHashFromIncome = HashTheImage(bImage)
                                    ValidInvalid = CompareTwoHashes(LineItemsTable("BIMAGESIGN"), ComputedHashFromIncome)
                                End If
                                LineItemsTable.Add("ValidInvalid", ValidInvalid) 'Validity of the image
                                ComputedHashFromIncome = Nothing
                                ValidInvalid = False
                                'MsgBox("Iko IsFCY sasa") '----------------
                                LineItemsTable.Add("IsFCY", CheckIfIsFcy)

                                If dt.Columns.Count <= 0 Then
                                    For Each name As String In LineItemsTable.Keys
                                        Dim ColName As DataColumn = New DataColumn()
                                        ColName.ColumnName = name
                                        ColName.DataType = System.Type.GetType(LineItemsTable(name).GetType().FullName.ToString)
                                        dt.Columns.Add(ColName)
                                    Next
                                End If
                                Dim dr As DataRow = dt.NewRow()
                                For Each name As String In LineItemsTable.Keys
                                    'Dim x As Object = Nothing
                                    'x = LineItemsTable(name).GetType().FullName
                                    'If x = "System.Byte[]" Then
                                    '    Dim value As Byte() = LineItemsTable(name)
                                    '    dr(name) = value
                                    'Else
                                    dr(name) = LineItemsTable(name)
                                    'End If

                                Next
                                dt.Rows.Add(dr)
                                'MsgBox("Iko kusave sasa ") '----------------
                                Modscan.SaveImagesToDB(LineItemsTable, fImageBW, fImage, bImage)
                                'MsgBox("Imaliza kusave sasa ") '----------------
                        End Select
                        System.Windows.Forms.Application.DoEvents()
                        Item = Item + 1
                        LineItemsTable.Clear()
                    End While
                End If
            End If
            MyFile.Close()
            myCol.Clear()
            System.IO.File.Delete(MyFile.Name)
            MyFile.Dispose()
            FileProgress.Value = myCol.Count
        Catch ex As Exception
            LineItemsTable.Clear()
            myCol.Clear()
            MyFile.Close()
            'MsgBox(ex.ToString)
            System.IO.File.Delete(MyFile.Name)
            Return False
        End Try
        Return True
    End Function
End Class
