Imports System.Collections.Specialized
Imports System.Configuration
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Windows.Forms
Imports System.Xml
Imports System.Xml.Serialization
Imports BRBase
Imports BRCATDS
Imports BRRTGSProcessing
Imports BRRTGSProcessing.Common
Imports Ionic.Zip
Imports ch = BrClearing.Common.ISO.Cheques
Imports cr = BrClearing.Common.BRISO20022CT812
Imports cx = BrClearing.Common.BRISO20022Cancellation
Imports dr = BrClearing.Common.ISO.Debits
Imports File = System.IO.File
Imports pc412 = BrClearing.Common.BRISO20022PC412
Imports pcr = BrClearing.Common.ISO.Cancellations
Imports res = BrClearing.Common.BRISO20022Response

Namespace ETH
#Region "Structures"
    Public Structure ETDDDetail
        Dim MsgId As String
        Dim Creation As String
        Dim Settlement As String
        Dim sBIC As String
        Dim dBIC As String
        Dim InstrId As String
        Dim EndToEndId As String
        Dim TrxId As String
        Dim Curr As String
        Dim Amount As String
        Dim Collection As String
        Dim Mandate As DDMandate
        Dim Scheme As String
        Dim Remittance As String
        Dim Retcode As String
        Dim InstrumentType As String
        Dim VCode As String
        Dim Frqcy As String
        Dim OrgnlMsgId As String
        Dim RemitterName As String
        Dim OrgnTrxID As String
        Dim OurBankBic As String
        Dim RetCodeDesc As String
        Dim BeneficiaryAcc As String
        Dim BeneficiaryName As String
        Dim DestBankID As String
        Dim OrgnRef As String
        Dim SourceBankID As String
        Dim UstrdMicr As String
        Dim DAdrLine As String
        Dim DTwnNm As String
        Dim DCtry As String
        Dim DNm As String
        Dim DPhneNb As String
        Dim DMobNb As String
        Dim DEmailAdr As String
        Dim DOthr As String
        Dim DbtrAcct As String
        Dim CAdrLine As String
        Dim CTwnNm As String
        Dim CCtry As String
        Dim CNm As String
        Dim CPhneNb As String
        Dim CMobNb As String
        Dim CEmailAdr As String
        Dim COthr As String
        Dim PymType As String
        Dim CdtrAcct As String
        Dim MndtId As String
        Dim DtOfSgntr As String
        Dim ReqdColltnDt As String
        Dim OrgnlInstrID As String
        Dim UstrdColD As String
        Dim TrxData As String
        Dim SvcLvl As String
        Dim IntrBkSttlmDt As String
        Dim LclInstrm As String
        Dim CtgyPurp As String
        Dim OrgnlTxId As String
        Dim OrgnlIntrBkSttlmDt As String
        Dim SeqTp As String
    End Structure
    Public Structure DDMandate
        Dim MndtId As String
        Dim DtOfSgntr As String
        Dim AmdmntInd As String
        Dim Frqcy As String
    End Structure
    Public Structure ETEFTDetails
        Dim EFTID As String
        Dim Reference As String
        Dim Currency As String
        Dim ISOCurrency As String
        Dim Amount As String
        Dim RemitterAcc As String
        Dim RemitterName As String
        Dim RemBankAcc As String
        Dim SourceAcc As String
        Dim SourceBIC As String
        Dim DestAcc As String
        Dim DestBIC As String
        Dim BeneficiaryAcc As String
        Dim BeneficiaryName As String
        Dim TranType As Integer
        Dim RetCode As String
        Dim RemittanceInfo As String
        Dim ValueDate As Date
        Dim IsDebit As Boolean
        Dim MsgId As String
        Dim TrxId As String
        Dim TrxData As String
        Dim VCode As String
        Dim Frqcy As String
        Dim SourceBankID As String
        Dim DestBankID As String
        Dim InstrumentType As String
        Dim ImageCounter As String
        Dim OrgnlMsgId As String
        Dim OrgnTrxID As String
        Dim OurBankBic As String
        Dim RetCodeDesc As String
        Dim OrgnRef As String
        Dim UstrdMicr As String
        Dim DAdrLine As String
        Dim DTwnNm As String
        Dim DCtry As String
        Dim DNm As String
        Dim DPhneNb As String
        Dim DMobNb As String
        Dim DEmailAdr As String
        Dim DOthr As String
        Dim DbtrAcct As String
        Dim CAdrLine As String
        Dim CTwnNm As String
        Dim CCtry As String
        Dim CNm As String
        Dim CPhneNb As String
        Dim CMobNb As String
        Dim CEmailAdr As String
        Dim COthr As String
        Dim PymType As String
        Dim CdtrAcct As String
        Dim OrgnlInstrID As String
        Dim UstrdColD As String
        Dim OrgnlEndToEnd As String
        Dim ReqdColltnDt As String
        Dim OrgnlIntrBkSttlmDt As String
        Dim OrgnlTxId As String
        Dim CCNm As String
        Dim DCNm As String
        Dim Rsn As String
        Dim SvcLvl As String
        Dim IntrBkSttlmDt As String
        Dim LclInstrm As String
        Dim CtgyPurp As String
    End Structure
    Public Structure ETChequeDetails
        Dim MICRED As Boolean
        Dim TransCode As String
        Dim RetCode As String
        Dim SourceBIC As String
        Dim BranchCode As String
        Dim CreationDate As String
        Dim BeneficiaryAcc As String
        Dim ChequeNumber As String
        Dim ChequeIndex As String
        Dim RemittanceInfo As String
        Dim SvcLvl As String
        Dim IntrBkSttlmDt As String
        Dim ReqdColltnDt As String
        Dim BankCode As String
        Dim BankBIC As String
        Dim Amount As Decimal
        Dim Codeline As String
        Dim OurBankID As String
        Dim OurBranchID As String
        Dim RemitterName As String
        Dim EndorsmentNo As String
        Dim CurrencyCode As String
        Dim BeneficiaryName As String
        Dim FileName As String
        Dim RemitterAcc As String
        Dim ValueDate As Date
        Dim MsgId As String
        Dim OurBranch As String
        Dim FrontImageGS As Byte()
        Dim FrontImageBW As Byte()
        Dim BackImageGS As Byte()
        Dim FrontImageUV As Byte()
        Dim Orgid As String
        Dim VoucherCode As String
        Dim trxID As String
        Dim OrgnTrxID As String
        Dim reference As String
        Dim OurBankBic As String
        Dim OrgnlMsgId As String
        Dim RetCodeDesc As String
        Dim OrgnRef As String
        Dim UstrdMicr As String
        Dim DAdrLine As String
        Dim DTwnNm As String
        Dim DCtry As String
        Dim DNm As String
        Dim DPhneNb As String
        Dim DMobNb As String
        Dim DEmailAdr As String
        Dim DOthr As String
        Dim DbtrAcct As String
        Dim CAdrLine As String
        Dim CTwnNm As String
        Dim CCtry As String
        Dim CNm As String
        Dim CPhneNb As String
        Dim CMobNb As String
        Dim CEmailAdr As String
        Dim COthr As String
        Dim PymType As String
        Dim CdtrAcct As String
        Dim UstrdBWF As String
        Dim UstrdBWR As String
        Dim UstrdGS As String
        Dim UstrdUV As String
        Dim OrgnlInstrID As String
        Dim UstrdColD As String
        Dim OrgnlTxId As String
        Dim OrgnlEndToEnd As String
        Dim TrxData As String
        Dim EndToEndId As String
        Dim LclInstrm As String
        Dim CtgyPurp As String
    End Structure
#End Region
#Region "Enums"
    Public Enum FileType
        Messages = 0
        Cheques = 1
        ChequeReturn = 2
        Efts = 3
        EftReturn = 4
        DD = 5
        DDReturn = 6
        RTGS = 7
        ChequeRejects = 8
        ATSStatements = 9
        RTGS103 = 10
        RTGS202 = 11
        RTGS920 = 12
        RTGS999 = 13
        ALLRTGS = 14
        ALLFILES = 15
    End Enum
    Public Enum ChequeFormat
        SISPackage = 0
        XMLPackages = 1
    End Enum
#End Region
    Public Class lsItems
        Public Property TrxRowID As String
    End Class
#Region "Outwards Generation Class"
    Public Class BRETHClass
        'mm = FileType, cc = CurrCode, = xyz = Session, x = CertName, y = Token/Keystore pass/Cert Password, (TPss and Tusr are both decoys, both are not in use) 
        Public Shared Sub GenerateETH(ByVal x As String, ByVal y As String, ByVal mm As FileType, ByVal cc As Int32, ByVal Exclude As Boolean, Optional ByVal chqFormat As ChequeFormat = ChequeFormat.XMLPackages, Optional ByVal xyz As String = "", Optional ByVal TPss As String = "", Optional ByVal TUsr As String = "", Optional ByVal ls As List(Of String) = Nothing)
            GenerateETHFiles(mm, cc, Exclude, chqFormat, xyz, x, y, TPss, TUsr, ls)
        End Sub
#Region "Varibles"
        Private Shared Sign As Boolean = Convert.ToBoolean(ConfigurationManager.AppSettings("Sign"))
        Private Shared CertName As String = ConfigurationManager.AppSettings("CertUser")
        Private Shared CountryID As String = ConfigurationManager.AppSettings("Country")
        Private Shared TkBased As Boolean
        Private Shared TokenPass As String
        Private Shared dtProcessingDate As Date = Date.Now
        Private Shared strFileLocation As String = ""
        Private Shared sArchive As String = ""
        Private Shared WorkingDt As String = ""
        Private Shared strFileName As String = ""
        Private Shared StrDestinationFilePath As String = ""
        Private Shared Files As FileType
        Private Shared TempLocation As String = ConfigurationManager.AppSettings("IncomingFiles")
#End Region

#Region "Constructors"
        Private Sub New(ByVal Location As String)
            If Not Directory.Exists(strFileLocation) Then Directory.CreateDirectory(strFileLocation)
            If Not Directory.Exists(TempLocation) Then Directory.CreateDirectory(TempLocation)
            Try
                Sign = Convert.ToBoolean(ConfigurationManager.AppSettings("Sign"))
            Catch ex As Exception
                Sign = True
            End Try

            Try
                TkBased = Convert.ToBoolean(ConfigurationManager.AppSettings("TokenBase"))
                If IsDBNull(TkBased) Then
                    TkBased = False
                End If
            Catch ex As Exception
                TkBased = False
            End Try
        End Sub
#End Region
#Region "Methods"
        Private Shared Function GenerateETHFiles(ByVal FileType As FileType, ByVal CurrCode As Int32, ByVal Exclude As Boolean, Optional ByVal chqFormat As ChequeFormat = ChequeFormat.XMLPackages, Optional ByVal Session As String = "01", Optional ByVal x As String = "", Optional ByVal y As String = "", Optional ByVal TokenPass As String = "", Optional ByVal TokenUser As String = "", Optional ByVal ls As List(Of String) = Nothing) As Boolean
            Try

                Dim RegX As New Regex("[^A-Za-z0-9]")
                Dim strDBAction As String = ""
                Dim strAction As String = ""

                Dim DestBankBIC As String = ""
                Dim Util As New DataTable()
                Dim BankArr As ArrayList = Nothing
                Dim MsgIdArr As ArrayList = Nothing
                CertName = x
                TokenPass = y
                strFileLocation = ConfigurationManager.AppSettings("OutgoingFiles")
                TempLocation = strFileLocation & "\Temp"
                StrDestinationFilePath = strFileLocation & "\Files"
                If Not Directory.Exists(StrDestinationFilePath) Then Directory.CreateDirectory(StrDestinationFilePath)
                If Not Directory.Exists(TempLocation) Then Directory.CreateDirectory(TempLocation)
                Modscan.OurBankID = ConfigurationManager.AppSettings("BankID")
                Dim modifiedXml = String.Empty
                Modscan.OurBranchID = ConfigurationManager.AppSettings("OurBranchID")
                'Banks
                Dim publicDTblBankCopy As DataTable
                Dim publicDTblmsgIDCopy As DataTable
                Dim publicDTblEJCopy As DataTable
                Dim publicDTblEFTCrCopy As DataTable
                Dim distinctBankID As DataTable
                Dim Counter As Int16
                Dim SystemType As String = ConfigurationManager.AppSettings("sysType")


                'MessageBox.Show("1")
                Try
                    'Modscan.WORKING_DATE = Format(Convert.ToDateTime(Modscan.WORKING_DATE), "dd-MMM-yyyy")
                    'Modscan.cWORKING_DATE = Format(Convert.ToDateTime(Modscan.FDATE), "yyyy-MM-dd")


                    Modscan.WORKING_DATE = Format(Convert.ToDateTime(Modscan.cFromDate), "dd-MMM-yyyy")
                    Modscan.cWORKING_DATE = Format(Convert.ToDateTime(Modscan.cToDate), "yyyy-MM-dd")

                    'MessageBox.Show("WORKING_DATE " + Modscan.WORKING_DATE)

                    'MessageBox.Show("cWORKING_DATE " + Modscan.cWORKING_DATE)
                    Modscan.cFromDate = Format(Convert.ToDateTime(Modscan.cFromDate), "dd-MMM-yyyy")
                    'MessageBox.Show("cFromDate " + Modscan.cFromDate)
                    Modscan.cToDate = Format(Convert.ToDateTime(Modscan.cToDate), "dd-MMM-yyyy")
                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                End Try

                Dim DestSignedFolderLoc As String = ConfigurationManager.AppSettings("OutgoingFiles") & "\SignedFiles"

                Try
                    TkBased = Convert.ToBoolean(ConfigurationManager.AppSettings("TokenBase"))
                    If IsDBNull(TkBased) Then
                        TkBased = False
                    End If
                Catch ex As Exception
                    TkBased = False
                End Try
                'MessageBox.Show("2")

                Dim serializer As New XmlSerializer(GetType(List(Of lsItems)))
                Dim newls As New List(Of lsItems)()
                Dim xmlls As String = String.Empty
                If ls IsNot Nothing Then
                    If ls.Count > 0 Then
                        For Each trxRowID As String In ls
                            newls.Add(New lsItems() With {.TrxRowID = trxRowID})
                        Next


                        'items.Add(New lsItems() With {.TrxRowID = "19"})
                        'Using writer As New StringWriter()
                        '    serializer.Serialize(writer, newls)
                        '    xmlls = writer.ToString()
                        'End Using
                        modifiedXml = ConvertListToXmlString(newls)
                        modifiedXml = RemoveXmlHeaderAndRootElement(modifiedXml)

                    End If
                End If

                Try
                    Select Case FileType
                        Case FileType.Cheques, FileType.ChequeReturn
                            If ls IsNot Nothing Then
                                If ls.Count > 0 Then
                                    Modscan.ExecuteData(Modscan.GetModify("[Proc_CreateXMLFiles_Ethiopia_Selected_CTS]", "OurBankID", Modscan.OurBankID, "ourbranchid", Modscan.OurBranchID, "ReadDate", Modscan.cToDate, "FromDate", Modscan.cFromDate, "EJDate", Modscan.cWORKING_DATE, "ClearingCenters", "67", "AllCenters", 0, "Currency", CurrCode, "Session", 0, "xmlList", modifiedXml), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.SelectStatement, Modscan.publicDset)
                                Else
                                    Modscan.ExecuteData(Modscan.GetModify("[Proc_CreateXMLFiles_Ethiopia_CTS]", "OurBankID", Modscan.OurBankID, "ourbranchid", Modscan.OurBranchID, "ReadDate", Modscan.cToDate, "FromDate", Modscan.cFromDate, "EJDate", Modscan.cWORKING_DATE, "ClearingCenters", "67", "AllCenters", 0, "Currency", CurrCode, "Session", 0), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.SelectStatement, Modscan.publicDset)
                                End If
                            Else
                                Modscan.ExecuteData(Modscan.GetModify("[Proc_CreateXMLFiles_Ethiopia_CTS]", "OurBankID", Modscan.OurBankID, "ourbranchid", Modscan.OurBranchID, "ReadDate", Modscan.cToDate, "FromDate", Modscan.cFromDate, "EJDate", Modscan.cWORKING_DATE, "ClearingCenters", "67", "AllCenters", 0, "Currency", CurrCode, "Session", 0), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.SelectStatement, Modscan.publicDset)
                            End If

                            If Modscan.publicDTbl.Rows.Count > 0 Then
                                publicDTblBankCopy = Modscan.publicDset.Tables(0).Clone()
                                distinctBankID = Modscan.publicDset.Tables(0).Clone()
                                publicDTblmsgIDCopy = Modscan.publicDset.Tables(0).Clone()
                                For i As Int32 = 0 To Modscan.publicDTbl.Rows.Count - 1
                                    distinctBankID.ImportRow(Modscan.publicDTbl.Rows(i))
                                Next
                                publicDTblBankCopy = distinctBankID.DefaultView.ToTable(True, "BankID")
                                publicDTblmsgIDCopy = distinctBankID.DefaultView.ToTable(True, "OrgnlMsgId")
                                distinctBankID.Clear()
                                'Modscan.publicDTbl.Clear()

                                BankArr = New ArrayList
                                For i As Int32 = 0 To publicDTblBankCopy.Rows.Count - 1
                                    BankArr.Add(publicDTblBankCopy.Rows(i)("BankID").ToString)
                                Next

                                MsgIdArr = New ArrayList()
                                For i As Int32 = 0 To publicDTblmsgIDCopy.Rows.Count - 1
                                    MsgIdArr.Add(publicDTblmsgIDCopy.Rows(i)("OrgnlMsgId").ToString)
                                Next
                            Else
                                MessageBox.Show("There are no pending cheques/unpaid Cheques for generation")
                                Exit Function
                            End If
                        Case FileType.RTGS, FileType.RTGS103, FileType.RTGS202, FileType.RTGS999, FileType.RTGS920, FileType.ALLRTGS
                            If ls IsNot Nothing Then
                                If ls.Count > 0 Then
                                    Modscan.ExecuteData(Modscan.GetModify("p_getETRTGSMessagesToSend_specific", "FromDate", Modscan.cFromDate, "ToDate", Modscan.cWORKING_DATE, "xmlList", modifiedXml), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure, Modscan.publicDset)
                                Else
                                    Modscan.ExecuteData(Modscan.GetModify("p_getETRTGSMessagesToSend", "FromDate", Modscan.cFromDate, "ToDate", Modscan.cWORKING_DATE), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure, Modscan.publicDset)
                                End If
                            Else
                                Modscan.ExecuteData(Modscan.GetModify("p_getETRTGSMessagesToSend", "FromDate", Modscan.cFromDate, "ToDate", Modscan.cWORKING_DATE, "FileType", FileType), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure, Modscan.publicDset)
                            End If
                            If Modscan.publicDset.Tables(0).Rows.Count > 0 Then
                                publicDTblBankCopy = Modscan.publicDset.Tables(0).Clone()
                                distinctBankID = Modscan.publicDset.Tables(0).Clone()
                                For i As Int32 = 0 To Modscan.publicDset.Tables(0).Rows.Count - 1
                                    distinctBankID.ImportRow(Modscan.publicDset.Tables(0).Rows(i))
                                Next
                                publicDTblBankCopy = distinctBankID.DefaultView.ToTable(True, "BeneficiaryBic")
                                'Modscan.publicDset.Tables(0).Clear()
                                distinctBankID.Clear()

                                BankArr = New ArrayList
                                For i As Int32 = 0 To publicDTblBankCopy.Rows.Count - 1
                                    BankArr.Add(publicDTblBankCopy.Rows(i)("BeneficiaryBic").ToString)
                                Next
                            Else
                                MessageBox.Show("There are no RTGS for generation")
                                Exit Function
                            End If
                        Case FileType.Efts, FileType.EftReturn, FileType.DD, FileType.DDReturn
                            If ls IsNot Nothing Then
                                If ls.Count > 0 Then
                                    Modscan.ExecuteData(Modscan.GetModify("[Proc_CreateXMLFiles_Ethiopia_Selected_CTS]", "OurBankID", Modscan.OurBankID, "ourbranchid", Modscan.OurBranchID, "ReadDate", Modscan.cToDate, "FromDate", Modscan.cFromDate, "EJDate", Modscan.cWORKING_DATE, "ClearingCenters", "67", "AllCenters", 0, "Currency", CurrCode, "Session", 0, "xmlList", modifiedXml), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.SelectStatement, Modscan.publicDset)
                                Else
                                    Modscan.ExecuteData(Modscan.GetModify("[Proc_CreateXMLFiles_Ethiopia_CTS]", "OurBankID", Modscan.OurBankID, "ourbranchid", Modscan.OurBranchID, "ReadDate", Modscan.cToDate, "FromDate", Modscan.cFromDate, "EJDate", Modscan.cWORKING_DATE, "ClearingCenters", "67", "AllCenters", 0, "Currency", CurrCode, "Session", 0), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure, Modscan.publicDset)
                                End If
                            Else
                                Modscan.ExecuteData(Modscan.GetModify("[Proc_CreateXMLFiles_Ethiopia_CTS]", "OurBankID", Modscan.OurBankID, "ourbranchid", Modscan.OurBranchID, "ReadDate", Modscan.cToDate, "FromDate", Modscan.cFromDate, "EJDate", Modscan.cWORKING_DATE, "ClearingCenters", "67", "AllCenters", 0, "Currency", CurrCode, "Session", 0), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure, Modscan.publicDset)
                            End If

                            If Modscan.publicDset.Tables(1).Rows.Count > 0 Then
                                publicDTblBankCopy = Modscan.publicDset.Tables(1).Clone()
                                distinctBankID = Modscan.publicDset.Tables(0).Clone()
                                For i As Int32 = 0 To Modscan.publicDset.Tables(1).Rows.Count - 1
                                    distinctBankID.ImportRow(Modscan.publicDset.Tables(1).Rows(i))
                                Next
                                publicDTblBankCopy = distinctBankID.DefaultView.ToTable(True, "BankID")
                                'Modscan.publicDset.Tables(1).Clear()
                                distinctBankID.Clear()

                                BankArr = New ArrayList
                                For i As Int32 = 0 To publicDTblBankCopy.Rows.Count - 1
                                    BankArr.Add(publicDTblBankCopy.Rows(i)("BankID").ToString)
                                Next
                            Else
                                MessageBox.Show("There are no pending Efts/unpaid EFTs for generation")
                                Exit Function
                            End If
                    End Select
                Catch ex As Exception
                    'MessageBox.Show("Error Registered, Check ErrorLog")
                    Modscan.ErrorLog("Swift Code not maintained for this our BankID", "Cheques Generation")
                End Try
                'MessageBox.Show("Inaenda FileType.Cheques - " & CurrCode)
                Select Case FileType
                    Case FileType.Cheques

                        For k As Int32 = 0 To BankArr.Count - 1
                            Try
                                Dim i As Integer = 0
                                Dim amt As Decimal = 0
                                Dim l As New List(Of ETChequeDetails)
                                Dim CreateFile As String = Session & Now.ToString(".yyyyMMdd.HHmm.") & BankArr.Item(k) & "." & Session.Substring(1)
                                Dim BIC As String = ""
                                'Ejs
                                'Modscan.ExecuteData(Modscan.GetModify("[Proc_CreateXMLFiles_Ethiopia_CTS]", "OurBankID", Modscan.OurBankID, "ourbranchid", Modscan.OurBranchID, "ReadDate", Modscan.cToDate, "FromDate", Modscan.cFromDate, "EJDate", Modscan.cWORKING_DATE, "ClearingCenters", "67", "AllCenters", 0, "Currency", CurrCode, "Session", 1), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure)
                                If ls IsNot Nothing Then
                                    If ls.Count > 0 Then
                                        Modscan.ExecuteData(Modscan.GetModify("[Proc_CreateXMLFiles_Ethiopia_Selected_CTS]", "OurBankID", Modscan.OurBankID, "ourbranchid", Modscan.OurBranchID, "ReadDate", Modscan.cToDate, "FromDate", Modscan.cFromDate, "EJDate", Modscan.cWORKING_DATE, "ClearingCenters", "67", "AllCenters", 0, "Currency", CurrCode, "Session", 0, "xmlList", modifiedXml), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.SelectStatement, Modscan.publicDset)
                                    Else
                                        Modscan.ExecuteData(Modscan.GetModify("[Proc_CreateXMLFiles_Ethiopia_CTS]", "OurBankID", Modscan.OurBankID, "ourbranchid", Modscan.OurBranchID, "ReadDate", Modscan.cToDate, "FromDate", Modscan.cFromDate, "EJDate", Modscan.cWORKING_DATE, "ClearingCenters", "67", "AllCenters", 0, "Currency", CurrCode, "Session", 0), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.SelectStatement, Modscan.publicDset)
                                    End If
                                Else
                                    Modscan.ExecuteData(Modscan.GetModify("[Proc_CreateXMLFiles_Ethiopia_CTS]", "OurBankID", Modscan.OurBankID, "ourbranchid", Modscan.OurBranchID, "ReadDate", Modscan.cToDate, "FromDate", Modscan.cFromDate, "EJDate", Modscan.cWORKING_DATE, "ClearingCenters", "67", "AllCenters", 0, "Currency", CurrCode, "Session", 0), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.SelectStatement, Modscan.publicDset)
                                End If

                                Dim EJfoundRows() As DataRow
                                Select Case SystemType.ToUpper.Trim
                                    Case "BR"
                                        EJfoundRows = Modscan.publicDTbl.Select("TrxType ='O' AND ReturnCode ='00' AND BankID = '" & BankArr.Item(k) & "' And IsGenerated=false")
                                    Case "BRMFO"
                                        EJfoundRows = Modscan.publicDTbl.Select("TrxType ='O' AND ReturnCode ='00' AND BankID = '" & BankArr.Item(k) & "' And IsGenerated=false")
                                    Case "BRNET"
                                        EJfoundRows = Modscan.publicDTbl.Select("TrxType ='OC' AND ReturnCode ='00' AND BankID = '" & BankArr.Item(k) & "' And IsGenerated=false")
                                    Case "BRNETOLD"
                                        EJfoundRows = Modscan.publicDTbl.Select("TrxType ='OC' AND ReturnCode ='00' AND BankID = '" & BankArr.Item(k) & "' And IsGenerated=false")
                                End Select
                                If EJfoundRows.Length = 0 Then
                                    Continue For
                                End If
                                'MessageBox.Show("Inaenda FileType.Cheques - " & EJfoundRows.Length)
                                publicDTblEJCopy = Modscan.publicDTbl.Clone()
                                For j As Int32 = 0 To EJfoundRows.Length - 1
                                    publicDTblEJCopy.ImportRow(EJfoundRows(j))
                                Next
                                Modscan.publicDTbl.Clear()
                                Try
                                    If publicDTblEJCopy.Rows.Count > 0 Then
                                        BIC = publicDTblEJCopy(0)("SwiftCode")
                                    End If
                                Catch ex As Exception
                                    MessageBox.Show("Error Registered, Check ErrorLog")
                                    Modscan.ErrorLog("Swift Code not maintained for this our BankID", "Cheques Generation")
                                    Continue For
                                End Try
                                Try
                                    If publicDTblEJCopy.Rows.Count > 0 Then
                                        DestBankBIC = ""
                                        DestBankBIC = publicDTblEJCopy(0)("DestinationSwiftCode").ToString()
                                    End If
                                Catch ex As Exception
                                    MessageBox.Show("Error Registered, Check ErrorLog")
                                    Modscan.ErrorLog("Swift Code not maintained for this BankID " & publicDTblEJCopy(0)("BankID"), "Cheque generation for this bank aborted- Cheques Generation")
                                    Continue For
                                End Try

                                For Each r As DataRow In publicDTblEJCopy.Rows
                                    Try
                                        i += 1
                                        Dim chq As New ETChequeDetails
                                        chq.Amount = CDec(r("Amount"))
                                        'chq.BackImageGS = DirectCast(Modscan.String2Bytes(r("BackImageGrayScale")), Byte())
                                        chq.BankCode = r("BankID").ToString().PadLeft(2, "0")
                                        chq.BankBIC = r("DestinationSwiftCode").ToString()
                                        chq.BeneficiaryAcc = r("AccountID").ToString()
                                        chq.CurrencyCode = CurrCode
                                        chq.BeneficiaryName = RegX.Replace(r("BeneficiaryName").ToString(), " ")
                                        If chq.BeneficiaryName.Length > 55 Then chq.BeneficiaryName = chq.BeneficiaryName.Substring(0, 55)
                                        chq.BranchCode = r("BranchID").ToString().PadLeft(4, "0")
                                        chq.ChequeIndex = i.ToString()
                                        chq.ChequeNumber = r("ChequeId").ToString()
                                        chq.Codeline = r("MicrLineDetails").ToString()
                                        chq.CreationDate = Date.Now.ToString("yyyy-MM-dd HH\\:mm\\:ss")
                                        chq.EndorsmentNo = Modscan.GetRandomInt16
                                        chq.FileName = Path.Combine(strFileLocation, CreateFile)
                                        chq.trxID = r("TransactionMicrColumnID").ToString()


                                        Select Case SystemType.ToUpper.Trim
                                            Case "BR"
                                            'chq.FrontImageGS = Modscan.ImageToByte(Modscan.Bytes2Image(DirectCast(System.Text.Encoding.GetEncoding(1252).GetBytes(r("FrontImageGrayScale")), Byte())))
                                            'chq.FrontImageGS = Modscan.ImageToByte(Modscan.Bytes2Image(DirectCast(System.Text.Encoding.GetEncoding(1252).GetBytes(r("FrontImageGrayScale")), Byte())))
                                            'chq.FrontImageUV = Modscan.ImageToByte(Modscan.Bytes2Image(DirectCast(System.Text.Encoding.GetEncoding(1252).GetBytes(r("FrontImageUV")), Byte())))
                                            'chq.FrontImageBW = Modscan.ImageToByteTif(Modscan.Bytes2Image(DirectCast(System.Text.Encoding.GetEncoding(1252).GetBytes(r("FrontImageTiff")), Byte())))

                                            'Old format
                                            'chq.FrontImageGS = Modscan.ImageToByte(Modscan.Bytes2Image(Modscan.String2Bytes(r("FrontImageGrayScale"))))
                                            'chq.FrontImageGS = Modscan.ImageToByte(Modscan.Bytes2Image(Modscan.String2Bytes(r("FrontImageGrayScale"))))
                                            'chq.FrontImageUV = Modscan.ImageToByte(Modscan.Bytes2Image(Modscan.String2Bytes(r("FrontImageUV"))))
                                            'chq.FrontImageBW = Modscan.ImageToByteTif(Modscan.Bytes2Image(Modscan.String2Bytes(r("FrontImageTiff"))))


                                            'chq.FrontImageGS = Modscan.String2Bytes(r("FrontImageGrayScale"))
                                            'chq.FrontImageGS = Modscan.String2Bytes(r("FrontImageGrayScale"))
                                            'chq.FrontImageUV = Modscan.String2Bytes(r("FrontImageUV"))
                                            'chq.FrontImageBW = Modscan.String2Bytes(r("FrontImageTiff"))

                                            Case "BRMFO"
                                            'chq.FrontImageGS = Modscan.ImageToByte(Modscan.Bytes2Image(DirectCast(System.Text.Encoding.GetEncoding(1252).GetBytes(r("FrontImageGrayScale")), Byte())))
                                            'chq.FrontImageGS = Modscan.ImageToByte(Modscan.Bytes2Image(DirectCast(System.Text.Encoding.GetEncoding(1252).GetBytes(r("FrontImageGrayScale")), Byte())))
                                            'chq.FrontImageUV = Modscan.ImageToByte(Modscan.Bytes2Image(DirectCast(System.Text.Encoding.GetEncoding(1252).GetBytes(r("FrontImageUV")), Byte())))
                                            'chq.FrontImageBW = Modscan.ImageToByteTif(Modscan.Bytes2Image(DirectCast(System.Text.Encoding.GetEncoding(1252).GetBytes(r("FrontImageTiff")), Byte())))
                                            Case "BRNET"
                                                chq.FrontImageGS = Modscan.ImageToByte(Modscan.Bytes2Image(Modscan.String2Bytes(r("FrontImageGrayScale"))))
                                                chq.BackImageGS = Modscan.ImageToByte(Modscan.Bytes2Image(Modscan.String2Bytes(r("BackImageGrayScale"))))
                                                chq.FrontImageUV = Modscan.ImageToByte(Modscan.Bytes2Image(Modscan.String2Bytes(r("FrontImageUV"))))
                                                chq.FrontImageBW = Modscan.ImageToByteTif(Modscan.Bytes2Image(Modscan.String2Bytes(r("FrontImageTiff"))))
                                            Case "BRNETOLD"
                                                'chq.FrontImageGS = Modscan.ImageToByte(Modscan.Bytes2Image(Modscan.String2Bytes(r("FrontImageGrayScale"))))
                                                'chq.FrontImageGS = Modscan.ImageToByte(Modscan.Bytes2Image(Modscan.String2Bytes(r("FrontImageGrayScale"))))
                                                'chq.FrontImageUV = Modscan.ImageToByte(Modscan.Bytes2Image(Modscan.String2Bytes(r("FrontImageUV"))))
                                                'chq.FrontImageBW = Modscan.ImageToByteTif(Modscan.Bytes2Image(Modscan.String2Bytes(r("FrontImageTiff"))))
                                        End Select
                                        chq.MICRED = True
                                        chq.RemittanceInfo = r("TransactionMicrColumnID").ToString()
                                        chq.RemitterAcc = r("TheirAccountID").ToString()
                                        chq.RemitterName = RegX.Replace(r("RemittersName").ToString(), " ")
                                        If chq.RemitterName.Length > 55 Then chq.RemitterName = chq.RemitterName.Substring(0, 55)
                                        chq.TransCode = r("VoucherCode").ToString()
                                        chq.ValueDate = r("ValueDate")
                                        amt += chq.Amount
                                        If chqFormat = ChequeFormat.SISPackage Then SISTransaction(chq) Else l.Add(chq)
                                    Catch ex As Exception
                                        MessageBox.Show("Error Registered, Check ErrorLog")
                                        Modscan.ErrorLog(ex.Message, "- Cheques Generation")
                                        Continue For
                                    End Try
                                Next
                                If chqFormat = ChequeFormat.SISPackage Then
                                    ZipContents(CreateFile & ".zip", Path.GetFileNameWithoutExtension(CreateFile), New String() {"*.cheque*"}, "", True)
                                Else
                                    If l.Count > 0 Then
                                        Try
                                            CreateFile = DestBankBIC.Substring(0, 4) & Now.ToString("yyyyMMddHHmmss") & Session & ".chk"
                                            Dim msgId As String = BulkCheques(l, l(0).CurrencyCode, amt, BIC)

                                            'Dim RTGS = New BRRTGSProcessing.ETRTGSProcessing(DestSignedFolderLoc, Sign, CertName, TempLocation, Modscan.WORKING_DATE, Modscan.OurBankID, Modscan.OurBranchID)
                                            'RTGS.BRRSFiles(Modscan.publicDset.Tables(0), "AchCheques")

                                            'ZipContents(CreateFile, msgId, New String() {"*.xml", "*.tiff"}, "", True)
                                            For Each c As ETChequeDetails In l
                                                Select Case SystemType.ToUpper.Trim
                                                    Case "BR"
                                                        strAction = "UPDATE t_TransactionMICR SET IsGenerated = 1, Reference = '" & msgId & "', ExtraDetails = '" & c.Codeline & "',  MicrLine ='" & c.Codeline & "' WHERE ColumnID = '" & c.RemittanceInfo & "'"
                                                    Case "BRMFO"
                                                        strAction = "UPDATE t_TransactionMICR SET IsGenerated = 1, Reference = '" & msgId & "', ExtraDetails = '" & c.Codeline & "',  MicrLine ='" & c.Codeline & "' WHERE ColumnID = '" & c.RemittanceInfo & "'"
                                                    Case "BRNET"
                                                        strAction = "UPDATE t_TrxClearing SET IsGenerated = 1, SessNo='1' ,Reference = '" & msgId & "' WHERE TrxRowID = '" & c.trxID & "' AND ReturnCodeID = '00'"
                                                    Case "BRNETOLD"
                                                        strAction = "UPDATE t_TrxClearing SET IsGenerated = 1, SessNo='1' ,Reference = '" & msgId & "' WHERE TrxRowID = '" & c.RemittanceInfo & "'"
                                                End Select
                                                Modscan.ExecuteData(strAction, Nothing, Modscan.dataExecTypes.ExecTypeNonQuery, Modscan.queryType.SelectStatement)
                                            Next
                                        Catch ex As Exception
                                            MessageBox.Show("Error Registered, Check ErrorLog")
                                            Modscan.ErrorLog(ex.Message, "- Cheques Generation")
                                            Continue For
                                        End Try
                                    End If
                                End If
                            Catch ex As Exception
                                MessageBox.Show("Error Registered, Check ErrorLog")
                                Modscan.ErrorLog(ex.Message, "- Cheques Generation")
                                Continue For
                            End Try
                        Next
                        'MessageBox.Show("Inaenda FileType.ChequeReturn - " & CurrCode)
                        publicDTblEJCopy.Clear()

                    Case FileType.RTGS, FileType.RTGS103, FileType.RTGS202, FileType.RTGS999, FileType.RTGS920, FileType.ALLRTGS
                        'M = Location, N = SignFiles, O = CertName, P = Temp, Q = ProcDate, R = OurBankBic, S = HQBranch, T = details, U = Action
                        Try
                            Dim Response As Boolean = False
                            CertName = ConfigurationManager.AppSettings("CertUser")
                            Dim RTGS = New BRRTGSProcessing.ETRTGSProcessing(DestSignedFolderLoc, Sign, CertName, TempLocation, Modscan.FDATE, Modscan.OurBankID, Modscan.OurBranchID)

                            'Select Case FileType
                            '    Case FileType.RTGS103
                            '        Modscan.ExecuteData(Modscan.GetModify("[p_getETRTGSMessagesToSend]", "FromDate", Modscan.cFromDate, "ToDate", Modscan.cWORKING_DATE, "FileType", "103"), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure, Modscan.publicDset)
                            '    Case FileType.RTGS202
                            '        Modscan.ExecuteData(Modscan.GetModify("[p_getETRTGSMessagesToSend]", "FromDate", Modscan.cFromDate, "ToDate", Modscan.cWORKING_DATE, "FileType", "202"), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure, Modscan.publicDset)
                            '    Case FileType.RTGS999
                            '        Modscan.ExecuteData(Modscan.GetModify("[p_getETRTGSMessagesToSend]", "FromDate", Modscan.cFromDate, "ToDate", Modscan.cWORKING_DATE, "FileType", "999"), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure, Modscan.publicDset)
                            '    Case FileType.RTGS920
                            '        Modscan.ExecuteData(Modscan.GetModify("[p_getETRTGSMessagesToSend]", "FromDate", Modscan.cFromDate, "ToDate", Modscan.cWORKING_DATE, "FileType", "920"), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure, Modscan.publicDset)
                            '    Case FileType.ALLRTGS
                            '        Modscan.ExecuteData(Modscan.GetModify("[p_getETRTGSMessagesToSend]", "FromDate", Modscan.cFromDate, "ToDate", Modscan.cWORKING_DATE, "FileType", Nothing), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure, Modscan.publicDset)
                            'End Select
                            If Modscan.publicDset.Tables(0).Rows.Count > 0 Then
                                For Each datar In Modscan.publicDset.Tables(0).Rows
                                    Try
                                        Response = RTGS.BRRSFiles(datar, datar("MessageType").ToString())
                                        If (Response) Then
                                            strAction = "UPDATE t_swiftOutGoingMessages SET SentToSwift = 1, Processed = 1 WHERE Trans_Ref = '" & datar("OriginalTrans_Ref").ToString() & "' AND MessageType = '" & datar("MessageType").ToString() & "'"
                                            Modscan.ExecuteData(strAction, Nothing, Modscan.dataExecTypes.ExecTypeNonQuery, Modscan.queryType.SelectStatement)
                                        End If
                                    Catch ex As Exception

                                    End Try
                                Next
                            End If

                        Catch ex As Exception
                            MessageBox.Show("Error Registered, Check ErrorLog")
                            Modscan.ErrorLog(ex.Message, "- RTGS Generation")
                        End Try


                    Case FileType.ChequeReturn
                        Counter = 0
                        For k As Int32 = 0 To BankArr.Count - 1
                            For MsgIdK As Int32 = 0 To MsgIdArr.Count - 1
                                Try
                                    Counter = Counter + 1
                                    'MessageBox.Show("Imefika Hapa 2 - " & CurrCode)
                                    Dim i As Integer = 0
                                    Dim amt As Decimal = 0
                                    Dim l As New List(Of ETChequeDetails)
                                    Dim CreateFile As String = Session & Now.ToString(".yyyyMMdd.HHmm.") & BankArr.Item(k) & "" & Session.Substring(1)
                                    Dim BIC As String = ""
                                    'Ejs Rejects
                                    'Modscan.ExecuteData(Modscan.GetModify("[Proc_CreateXMLFiles_Ethiopia_CTS]", "OurBankID", Modscan.OurBankID, "ourbranchid", Modscan.OurBranchID, "ReadDate", Modscan.cToDate, "FromDate", Modscan.cFromDate, "EJDate", Modscan.cWORKING_DATE, "ClearingCenters", "67", "AllCenters", 0, "Currency", CurrCode, "Session", 2), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure)
                                    Dim EJUNPfoundRows() As DataRow
                                    Select Case SystemType.ToUpper.Trim
                                        Case "BR"
                                            EJUNPfoundRows = Modscan.publicDTbl.Select("TrxType ='O' AND ReturnCode <>'00' AND BankID = '" & BankArr.Item(k) & "' And IsGenerated=false")
                                        Case "BRMFO"
                                            EJUNPfoundRows = Modscan.publicDTbl.Select("TrxType ='O' AND ReturnCode <>'00' AND BankID = '" & BankArr.Item(k) & "' And IsGenerated=false")
                                        Case "BRNET"
                                            EJUNPfoundRows = Modscan.publicDTbl.Select("TrxType ='OC' AND ReturnCode <>'00' AND BankID = '" & BankArr.Item(k) & "' And OrgnlMsgId =  '" & MsgIdArr.Item(MsgIdK) & "'   AND isNull(IsGenerated,0) = 0 ")
                                        Case "BRNETOLD"
                                            EJUNPfoundRows = Modscan.publicDTbl.Select("TrxType ='OC' AND ReturnCode <>'00' AND BankID = '" & BankArr.Item(k) & "' And IsGenerated=false")
                                    End Select
                                    If EJUNPfoundRows.Length = 0 Then
                                        Continue For
                                    End If
                                    'MessageBox.Show(EJUNPfoundRows.Length)
                                    publicDTblEJCopy = Modscan.publicDTbl.Clone()
                                    For j As Int32 = 0 To EJUNPfoundRows.Length - 1

                                        publicDTblEJCopy.ImportRow(EJUNPfoundRows(j))

                                    Next
                                    Try
                                        If publicDTblEJCopy.Rows.Count > 0 Then
                                            BIC = publicDTblEJCopy(0)("SwiftCode")
                                        End If
                                    Catch ex As Exception
                                        MessageBox.Show("Error Registered, Check ErrorLog")
                                        Modscan.ErrorLog("Swift Code not maintained for this our BankID", " - Unpaid Cheques Generation")
                                        Continue For
                                    End Try
                                    Try
                                        If publicDTblEJCopy.Rows.Count > 0 Then
                                            DestBankBIC = ""
                                            DestBankBIC = publicDTblEJCopy(0)("DestinationSwiftCode").ToString()
                                        End If
                                    Catch ex As Exception
                                        MessageBox.Show("Error Registered, Check ErrorLog")
                                        Modscan.ErrorLog("Swift Code not maintained for this BankID " & publicDTblEJCopy(0)("BankID"), "- Unpaid Cheques Generation")
                                        Continue For
                                    End Try
                                    Modscan.publicDTbl.Clear()
                                    For Each r As DataRow In publicDTblEJCopy.Rows
                                        Try
                                            i += 1
                                            Dim chq As New ETChequeDetails
                                            chq.Amount = CDec(r("Amount"))
                                            chq.BankCode = r("BankID").ToString()
                                            chq.BankBIC = r("DestinationSwiftCode").ToString()
                                            chq.BeneficiaryAcc = r("TheirAccountID").ToString()
                                            chq.BeneficiaryName = r("BeneficiaryName").ToString() 'RegX.Replace(r("BeneficiaryName").ToString(), " ")
                                            If chq.BeneficiaryName.Length > 55 Then chq.BeneficiaryName = chq.BeneficiaryName.Substring(0, 55)
                                            chq.BranchCode = r("BranchID").ToString()
                                            chq.OurBankBic = publicDTblEJCopy(0)("SwiftCode").ToString()
                                            chq.OurBankID = Modscan.OurBankID
                                            chq.ChequeIndex = r("UstrdChqdt").ToString()
                                            chq.ChequeNumber = r("ChequeId").ToString()
                                            chq.Codeline = r("MicrLineDetails").ToString()
                                            chq.CreationDate = Date.Now.ToString("yyyy-MM-dd HH\\:mm\\:ss")
                                            chq.CurrencyCode = CurrCode
                                            chq.RemittanceInfo = r("TransactionID").ToString()
                                            chq.EndorsmentNo = Modscan.GetRandomInt16
                                            chq.RemitterAcc = r("AccountID").ToString()
                                            chq.RemitterName = r("RemittersName").ToString() 'RegX.Replace(r("RemittersName").ToString(), " ")
                                            chq.FileName = r("TransactionMicrColumnID").ToString()
                                            chq.MICRED = True
                                            If chq.RemitterName.Length > 55 Then chq.RemitterName = chq.RemitterName.Substring(0, 55)
                                            chq.RetCode = r("ReturnCode").ToString()
                                            chq.RetCodeDesc = r("RetCodeDesc").ToString().ToUpper
                                            chq.TransCode = r("VoucherCode").ToString()
                                            chq.ValueDate = r("ValueDate")
                                            chq.trxID = r("ColumnID").ToString()
                                            chq.OrgnTrxID = r("TrxID").ToString()
                                            chq.OrgnlMsgId = r("OrgnlMsgId").ToString()
                                            chq.OrgnlInstrID = r("OrgnlInstrID").ToString()
                                            chq.CAdrLine = r("CAdrLine").ToString()
                                            chq.CNm = r("CNm").ToString()
                                            chq.CEmailAdr = r("CEmailAdr").ToString()
                                            chq.CMobNb = r("CMobNb").ToString()
                                            chq.CPhneNb = r("CPhneNb").ToString()
                                            chq.COthr = r("COthr").ToString()
                                            chq.CCtry = r("CCtry").ToString()
                                            chq.CTwnNm = r("CTwnNm").ToString()
                                            chq.UstrdBWF = r("UstrdBWF").ToString()
                                            chq.UstrdBWR = r("UstrdBWR").ToString()
                                            chq.UstrdColD = r("UstrdColD").ToString()
                                            chq.UstrdGS = r("UstrdGS").ToString()
                                            chq.UstrdMicr = r("UstrdMicr").ToString()
                                            chq.UstrdUV = r("UstrdUV").ToString()
                                            chq.DAdrLine = r("DAdrLine").ToString()
                                            chq.DNm = r("DNm").ToString()
                                            chq.DEmailAdr = r("DEmailAdr").ToString()
                                            chq.DMobNb = r("DMobNb").ToString()
                                            chq.DPhneNb = r("DPhneNb").ToString()
                                            chq.DOthr = r("DOthr").ToString()
                                            chq.DCtry = r("DCtry").ToString()
                                            chq.DTwnNm = r("DTwnNm").ToString()
                                            chq.DbtrAcct = r("DbtrAcct").ToString()
                                            chq.CdtrAcct = r("CdtrAcct").ToString()
                                            chq.OrgnlEndToEnd = r("OrgnlEndToEnd").ToString()
                                            chq.SvcLvl = r("SvcLvl").ToString()
                                            chq.LclInstrm = r("LclInstrm").ToString()
                                            chq.CtgyPurp = r("CtgyPurp").ToString()
                                            chq.IntrBkSttlmDt = IIf(r("ReqdColltnDt").ToString() = "Jan  1 1900 12:00AM", "", r("ReqdColltnDt").ToString())
                                            chq.ReqdColltnDt = IIf(r("ReqdColltnDt").ToString() = "Jan  1 1900 12:00AM", "", r("ReqdColltnDt").ToString())
                                            l.Add(chq)
                                        Catch ex As Exception
                                            MessageBox.Show("Error Registered, Check ErrorLog")
                                            Modscan.ErrorLog(ex.Message, "Unpaid Cheques Generation")
                                            Continue For
                                        End Try
                                    Next
                                    If l.Count > 0 Then
                                        Dim msgId As String = UnpaidCheques(l, DestBankBIC, l(0).CurrencyCode, Modscan.cWORKING_DATE, Counter)
                                        For Each s As ETChequeDetails In l
                                            Try
                                                Select Case SystemType.ToUpper.Trim
                                                    Case "BR"
                                                        strAction = "UPDATE t_TransactionMICR SET IsGenerated = 1, Reference = '" & msgId & "' , RejectedReason = '" & s.RetCode & "' ,  MicrLine ='" & s.Codeline & "' AND ReturnCode <>'00' WHERE ColumnID = '" & s.RemittanceInfo & "'"
                                                    Case "BRMFO"
                                                        strAction = "UPDATE t_TransactionMICR SET IsGenerated = 1, Reference = '" & msgId & "' , RejectedReason = '" & s.RetCode & "' ,  MicrLine ='" & s.Codeline & "' AND ReturnCode <>'00' WHERE ColumnID = '" & s.RemittanceInfo & "'"
                                                    Case "BRNET"
                                                        strAction = "UPDATE t_TrxClearing SET IsGenerated = 1, SessNo='1' ,Reference = '" & msgId & "'  WHERE TrxRowID = '" & s.trxID & "' AND  ReturnCodeID <>'00'"
                                                    Case "BRNETOLD"
                                                        strAction = "UPDATE t_TrxClearing SET IsGenerated = 1, SessNo='1' ,Reference = '" & msgId & "' ReturnCodeID <>'00'  WHERE TrxRowID = '" & s.MsgId & "'  AND  ReturnCodeID <>'00'"
                                                End Select
                                                Modscan.ExecuteData(strAction, Nothing, Modscan.dataExecTypes.ExecTypeNonQuery, Modscan.queryType.SelectStatement)
                                            Catch ex As Exception
                                                MessageBox.Show("Error Registered, Check ErrorLog")
                                                Modscan.ErrorLog(ex.Message, "Unpaid Cheques Generation")
                                                Continue For
                                            End Try
                                        Next
                                    End If
                                    System.Threading.Thread.Sleep(300)
                                    Application.DoEvents()
                                Catch ex As Exception
                                    MessageBox.Show("Error Registered, Check ErrorLog ")
                                    Modscan.ErrorLog(ex.Message, "Unpaid Cheques Generation 2 ")
                                    Continue For
                                End Try
                            Next
                        Next
                        publicDTblEJCopy.Clear()
                    Case FileType.RTGS

                    Case FileType.Efts
                        'For k As Int32 = 0 To BankArr.Count - 1
                        Dim i As Integer = 0
                            Dim amt As Decimal = 0
                            Dim l As New List(Of ETChequeDetails)
                        Dim CreateFile As String = Session & Now.ToString(".yyyyMMdd.HHmm.") & Session.Substring(1)
                        Dim BIC As String = ""
                            'EFT Cr
                            'Truncation  Modscan.ExecuteData(Modscan.GetModify("[Proc_CreateXMLFiles_Ethiopia_CTS]", "OurBankID", Modscan.OurBankID, "ourbranchid", Modscan.OurBranchID, "ReadDate", Modscan.WORKING_DATE, "FromDate", Modscan.cFromDate, "EJDate", Modscan.cToDate, "ClearingCenters", "67", "AllCenters", 0, "Currency", CurrCode), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure, Modscan.publicDset)
                            'Modscan.ExecuteData(Modscan.GetModify("[Proc_CreateXMLFiles_Ethiopia_CTS]", "OurBankID", Modscan.OurBankID, "ourbranchid", Modscan.OurBranchID, "ReadDate", Modscan.cToDate, "FromDate", Modscan.cFromDate, "EJDate", Modscan.cWORKING_DATE, "ClearingCenters", "47", "AllCenters", 0, "Currency", CurrCode, "Session", 1), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure, Modscan.publicDset)

                            Dim EFTCrfoundRows() As DataRow
                            Select Case SystemType.ToUpper.Trim
                                Case "BR"
                                EFTCrfoundRows = Modscan.publicDset.Tables(1).Select("TrxType ='0' AND VoucherCode <> '40'  AND ReturnCode ='00'   And IsGenerated=false") 'AND BankID = '" & BankArr.Item(k).ToString.Trim() & "'
                            Case "BRMFO"
                                EFTCrfoundRows = Modscan.publicDset.Tables(1).Select("TrxType ='0' AND VoucherCode <> '40'  AND ReturnCode ='00'  And IsGenerated=false") 'AND BankID = '" & BankArr.Item(k).ToString.Trim() & "' 
                            Case "BRNET"
                                EFTCrfoundRows = Modscan.publicDset.Tables(1).Select("TrxType ='OD' AND VoucherCode <> '40'  AND ReturnCode ='00'     AND isNull(IsGenerated,0) = 0 ") 'AND BankID = '" & BankArr.Item(k).ToString.Trim() & "'
                            Case "BRNETOLD"
                                EFTCrfoundRows = Modscan.publicDset.Tables(1).Select("TrxType ='OD' AND VoucherCode <> '40'  AND ReturnCode ='00'   And IsGenerated=false") 'AND BankID = '" & BankArr.Item(k).ToString.Trim() & "'
                        End Select
                            publicDTblEFTCrCopy = Modscan.publicDset.Tables(1).Clone()
                            For j As Int32 = 0 To EFTCrfoundRows.Length - 1
                                publicDTblEFTCrCopy.ImportRow(EFTCrfoundRows(j))
                            Next
                            Modscan.publicDset.Tables(0).Clear()
                            Try
                                If publicDTblEFTCrCopy.Rows.Count > 0 Then
                                    BIC = publicDTblEFTCrCopy(0)("SwiftCode")
                                End If
                            Catch ex As Exception
                                MessageBox.Show("Error Registered, Check ErrorLog")
                                Modscan.ErrorLog("Swift Code not maintained for this our BankID", "- EFT Generation")
                                'Continue For
                            End Try
                            Try
                                If publicDTblEFTCrCopy.Rows.Count > 0 Then
                                    DestBankBIC = ""
                                    DestBankBIC = publicDTblEFTCrCopy(0)("DestinationSwiftCode").ToString()
                                End If
                            Catch ex As Exception
                                MessageBox.Show("Error Registered, Check ErrorLog")
                                Modscan.ErrorLog("Swift Code not maintained for this BankID " & publicDTblEJCopy(0)("BankID") & " EFT generation for this Bank Abort", "- EFT Generation")
                                'Continue For
                            End Try
                            Dim cr As New List(Of ETEFTDetails)
                            For Each row As DataRow In publicDTblEFTCrCopy.Rows
                                Try
                                    Dim destBIC As String = row("DestinationSwiftCode").ToString()
                                    Dim SourceBIC As String = row("SwiftCode").ToString()
                                    Dim d As New ETEFTDetails
                                    d.IsDebit = row("ISDebitORISCredit")
                                    d.Amount = FormatNumber(row("Amount"), 2)
                                    d.BeneficiaryAcc = row("TheirAccountID")
                                    d.Currency = CurrCode
                                    d.BeneficiaryName = row("BeneficiaryName")
                                    d.BeneficiaryName = d.BeneficiaryName
                                    d.ISOCurrency = row("CurrencyCode")
                                    d.DestBIC = destBIC
                                    d.DestAcc = row("BranchID")
                                    d.EFTID = row("TransactionMicrColumnID")
                                    d.SourceAcc = row("OurBranchID")
                                    d.SourceBIC = SourceBIC
                                    d.Reference = Modscan.GetRandomString
                                    d.RemBankAcc = row("Amount")
                                    d.RemittanceInfo = row("OriginatorReference")
                                    d.RemitterAcc = row("AccountID")
                                    d.RemitterName = row("RemittersName")
                                    d.RemitterName = d.RemitterName
                                    d.TranType = row("VoucherCode").ToString().Split("-")(0).Trim()
                                    d.VCode = row("VoucherCode").ToString()
                                    d.TrxId = row("TransactionMicrColumnID")
                                    cr.Add(d)
                                Catch ex As Exception
                                    MessageBox.Show("Error Registered, Check ErrorLog")
                                    Modscan.ErrorLog(ex.Message, "- EFT Generation")
                                    Continue For
                                End Try
                            Next
                            If cr.Count > 0 Then
                                Dim msgId As String = BulkCredit(cr, cr(0).Currency, BIC)
                                For Each d As ETEFTDetails In cr
                                    Select Case SystemType.ToUpper.Trim
                                        Case "BR"
                                        strAction = "UPDATE t_TransactionMICR SET IsGenerated = 1, Reference = '" & msgId & "' WHERE ColumnID = '" & d.EFTID & "' AND TrxType ='0' AND VoucherCode <> '40' "' AND BankID = '" & BankArr.Item(k) & "'
                                    Case "BRMFO"
                                        strAction = "UPDATE t_TransactionMICR SET IsGenerated = 1, Reference = '" & msgId & "' WHERE ColumnID = '" & d.EFTID & "' AND TrxType ='0' AND VoucherCode <> '40'  " 'AND BankID = '" & BankArr.Item(k) & "'
                                    Case "BRNET"
                                            strAction = "UPDATE t_TrxClearing SET IsGenerated = 1, SessNo='1' ,Reference = '" & msgId & "' WHERE TrxRowID = '" & d.EFTID & "' AND TrxType ='OD' AND ReturnCodeID = '00' AND  VoucherCode <> '40'"  'AND BankID = '" & BankArr.Item(k) & "'"
                                        Case "BRNETOLD"
                                        strAction = "UPDATE t_TrxClearing SET IsGenerated = 1, SessNo='1' ,Reference = '" & msgId & "' WHERE TrxRowID = '" & d.EFTID & "' AND TrxType ='OD' AND VoucherCode <> '40'  " 'AND BankID = '" & BankArr.Item(k) & "'
                                End Select
                                    Modscan.ExecuteData(strAction, Nothing, Modscan.dataExecTypes.ExecTypeNonQuery, Modscan.queryType.SelectStatement)
                                Next
                            End If
                        'Next
                        publicDTblEFTCrCopy.Clear()
                    Case FileType.EftReturn
                        Counter = 0
                        'MessageBox.Show("imeingia")
                        'For k As Int32 = 0 To BankArr.Count - 1
                        'For MsgIdK As Int32 = 0 To MsgIdArr.Count - 1
                        Try
                                Counter = Counter + 1
                                Dim i As Integer = 0
                                Dim amt As Decimal = 0
                            Dim CreateFile As String = Session & Now.ToString(".yyyyMMdd.HHmm.") & "." & Session.Substring(1)
                            Dim BIC As String = ""
                                'EFT Cr Reject
                                'Modscan.ExecuteData(Modscan.GetModify("[Proc_CreateXMLFiles_Ethiopia_CTS]", "OurBankID", Modscan.OurBankID, "ourbranchid", Modscan.OurBranchID, "ReadDate", Modscan.cToDate, "FromDate", Modscan.cFromDate, "EJDate", Modscan.cWORKING_DATE, "ClearingCenters", "67", "AllCenters", 0, "Currency", CurrCode, "Session", 2), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure, Modscan.publicDset)
                                Dim EFTCrfoundRows() As DataRow
                                'MessageBox.Show(MsgIdArr.Item(MsgIdK))
                                Select Case SystemType.ToUpper.Trim
                                    Case "BR"
                                    EFTCrfoundRows = Modscan.publicDset.Tables(1).Select("TrxType ='0' AND VoucherCode <> '40' AND ReturnCode <>'00'   And IsGenerated=0")
                                Case "BRMFO"
                                    EFTCrfoundRows = Modscan.publicDset.Tables(1).Select("TrxType ='0' AND VoucherCode <> '40' AND ReturnCode <>'00'   And IsGenerated=0")
                                Case "BRNET"
                                    'EFTCrfoundRows = Modscan.publicDset.Tables(1).Select("TrxType ='OD' AND VoucherCode <> '40' AND ReturnCode <>'00' AND BankID = '" & BankArr.Item(k) & "'  And  OrgnlMsgId =  '" & MsgIdArr.Item(MsgIdK) & "' And  IsGenerated=0")
                                    EFTCrfoundRows = Modscan.publicDset.Tables(1).Select("TrxType IN ('OC') AND VoucherCode IN ('58','59') AND ReturnCode <>'00'  AND isNull(IsGenerated,0) = 0 ")
                                Case "BRNETOLD"
                                    EFTCrfoundRows = Modscan.publicDset.Tables(1).Select("TrxType ='OC' AND VoucherCode <> '40' AND ReturnCode <>'00'  And IsGenerated=0")
                            End Select
                                publicDTblEFTCrCopy = Modscan.publicDset.Tables(1).Clone()
                                'MessageBox.Show("Imepata unpaid EFTs rows : " & EFTCrfoundRows.Length)
                                For j As Int32 = 0 To EFTCrfoundRows.Length - 1
                                    publicDTblEFTCrCopy.ImportRow(EFTCrfoundRows(j))
                                Next
                                'Modscan.publicDset.Tables(1).Clear()
                                Try
                                    If publicDTblEFTCrCopy.Rows.Count > 0 Then
                                        BIC = publicDTblEFTCrCopy(0)("SwiftCode")
                                    End If
                                Catch ex As Exception
                                    'MessageBox.Show(ex.Message)
                                    MessageBox.Show("Error Registered, Check ErrorLog")
                                    Modscan.ErrorLog("Swift Code not maintained for this our BankID", "- EFT Unpaid Generation")
                                'Continue For
                            End Try
                                Try
                                    If publicDTblEFTCrCopy.Rows.Count > 0 Then
                                        DestBankBIC = ""
                                        DestBankBIC = publicDTblEFTCrCopy(0)("DestinationSwiftCode").ToString()
                                    End If
                                Catch ex As Exception
                                    MessageBox.Show("Error Registered, Check ErrorLog")
                                    Modscan.ErrorLog("Swift Code not maintained for this BankID " & publicDTblEJCopy(0)("BankID"), "- EFT Unpaid Generation")
                                'Continue For
                            End Try
                                Dim cr As New List(Of ETEFTDetails)
                                For Each row As DataRow In publicDTblEFTCrCopy.Rows
                                    Try
                                        Dim destBIC As String = row("DestinationSwiftCode").ToString()
                                        Dim SourceBIC As String = row("SwiftCode").ToString()
                                        Dim d As New ETEFTDetails
                                        d.Amount = FormatNumber(row("Amount"), 2)
                                        d.Currency = row("CurrencyCode")
                                        d.ISOCurrency = row("CurrencyCode")
                                        d.DestBIC = destBIC
                                        d.DestBankID = row("BankID")
                                        d.MsgId = row("Reference")
                                        d.VCode = row("VoucherCode").ToString().Trim()
                                        d.ValueDate = Modscan.WORKING_DATE
                                        d.RetCode = row("ReturnCode").ToString()
                                        d.RetCodeDesc = row("RetCodeDesc").ToString().ToUpper
                                        d.ValueDate = row("ValueDate")
                                        d.TrxId = row("TransactionMicrColumnID").ToString()
                                        d.OrgnTrxID = row("OrgnlInstrID").ToString()
                                        d.OrgnlMsgId = row("OrgnlMsgId").ToString()

                                        If String.IsNullOrEmpty(d.OrgnlMsgId) Then
                                            d.OrgnlMsgId = row("OriginatorReference").ToString()
                                        End If
                                        d.OrgnlInstrID = row("OrgnlInstrID").ToString()
                                        d.CAdrLine = row("CAdrLine").ToString()
                                        d.CNm = row("CNm").ToString()
                                        d.CEmailAdr = row("CEmailAdr").ToString()
                                        d.CMobNb = row("CMobNb").ToString()
                                        d.CPhneNb = row("CPhneNb").ToString()
                                        d.COthr = row("COthr").ToString()
                                        d.CCtry = row("CCtry").ToString()
                                        d.CTwnNm = row("CTwnNm").ToString()
                                        d.DAdrLine = row("DAdrLine").ToString()
                                        d.DNm = row("DNm").ToString()
                                        d.DEmailAdr = row("DEmailAdr").ToString()
                                        d.DMobNb = row("DMobNb").ToString()
                                        d.DPhneNb = row("DPhneNb").ToString()
                                        d.DOthr = row("DOthr").ToString()
                                        d.DCtry = row("DCtry").ToString()
                                        d.DTwnNm = row("DTwnNm").ToString()
                                        d.DCNm = row("DCNm").ToString()
                                        d.CCNm = row("CCNm").ToString()
                                        d.DbtrAcct = row("DbtrAcct").ToString()
                                        d.CdtrAcct = row("CdtrAcct").ToString()
                                        d.UstrdColD = row("UstrdColD").ToString()
                                        d.OrgnlEndToEnd = row("OrgnlInstrID").ToString()
                                        d.SvcLvl = row("SvcLvl").ToString()
                                        d.LclInstrm = row("LclInstrm").ToString()
                                        d.CtgyPurp = row("CtgyPurp").ToString()
                                        d.RemittanceInfo = row("UstrdColD").ToString()
                                        d.OrgnlIntrBkSttlmDt = Modscan.FDATE 'IIf(row("ReqdColltnDt").ToString() = "Jan  1 1900 12:00AM", "", row("ReqdColltnDt").ToString())
                                        d.ReqdColltnDt = Modscan.FDATE 'IIf(row("ReqdColltnDt").ToString() = "Jan  1 1900 12:00AM", "", row("ReqdColltnDt").ToString())
                                        cr.Add(d)
                                    Catch ex As Exception
                                        MessageBox.Show("Error Registered, Check ErrorLog")
                                        Modscan.ErrorLog(ex.Message, "- EFT Unpaids Generation")
                                        Continue For
                                    End Try
                                Next
                            If cr.Count > 0 Then
                                Dim msgId As String = CancelCreditq(cr, cr(0).Currency, BIC, Modscan.cWORKING_DATE)
                                For Each d As ETEFTDetails In cr
                                    Select Case SystemType.ToUpper.Trim
                                        Case "BR"
                                            strAction = "UPDATE t_TransactionMICR SET IsGenerated = 1, Remarks = '" & msgId & "' WHERE ColumnID = '" & d.TrxId & "' AND TrxType ='0' AND VoucherCode <> '40' "
                                        Case "BRMFO"
                                            strAction = "UPDATE t_TransactionMICR SET IsGenerated = 1, Remarks = '" & msgId & "' WHERE ColumnID = '" & d.TrxId & "' AND TrxType ='0' AND VoucherCode <> '40' AND IsGenerated=1 "
                                        Case "BRNET"
                                            strAction = "UPDATE t_trxClearing SET IsGenerated = 1, SessNo='2' ,Reference = '" & msgId & "' WHERE TrxRowID = '" & d.TrxId & "'  AND isNull(IsGenerated,0) = 0 " 'AND BankID = '" & BankArr.Item(k) & "'"
                                        Case "BRNETOLD"
                                            strAction = "UPDATE t_trxClearing SET IsGenerated = 1, SessNo='1' ,'Reference = '" & msgId & "' WHERE TrxRowID = '" & d.TrxId & "' AND TrxType ='OD' AND VoucherCode <> '40'  AND IsGenerated=1 "
                                    End Select
                                    Modscan.ExecuteData(strAction, Nothing, Modscan.dataExecTypes.ExecTypeNonQuery, Modscan.queryType.SelectStatement)
                                Next
                            End If
                        Catch ex As Exception
                                MessageBox.Show("Error Registered, Check ErrorLog")
                                Modscan.ErrorLog(ex.Message, "- EFT Unpaid Generation")
                            'Continue For
                        End Try
                        'Next
                        'Next
                        publicDTblEFTCrCopy.Clear()
                    Case FileType.DD
                        'For k As Int32 = 0 To BankArr.Count - 1
                        Dim i As Integer = 0
                            Dim amt As Decimal = 0
                            'Dim l As New List(Of TZ.ChequeDetails)
                            'Dim FileCounter As String = GetScalarREC("Select isNull(FileCounter,0)+1 From t_Bank Where BankID = '" & BankArr.Item(k) & "'")
                            'Dim CreateFile As String = Session & Now.ToString(".yyyyMMdd.HHmm.") & BankArr.Item(k) & "." & Session
                            Dim BIC As String = ""
                            'EFT Cr
                            'Modscan.ExecuteData(Modscan.GetModify("[Proc_CreateXMLFiles_Ethiopia_CTS]", "OurBankID", Modscan.OurBankID, "ourbranchid", Modscan.OurBranchID, "ReadDate", Modscan.cToDate, "FromDate", Modscan.cFromDate, "EJDate", Modscan.cWORKING_DATE, "ClearingCenters", "67", "AllCenters", 0, "Currency", CurrCode, "Session", 0), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure, Modscan.publicDset)
                            'Modscan.ExecuteData(Modscan.GetModify("Proc_CreateXMLFiles_UG_CTS", "OurBankID", Modscan.OurBankID, "ourbranchid", Modscan.OurBranchID, "ReadDate", Modscan.cWORKING_DATE, "FromDate", Modscan.cFromDate, "EJDate", Modscan.cToDate, "ClearingCenters", "47", "AllCenters", 0, "Currency", CurrCode, "FileType", "T", "Sessno", Session), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure, Modscan.publicDset)
                            Dim EFTCrfoundRows() As DataRow
                            Select Case SystemType.ToUpper.Trim
                                Case "BR"
                                EFTCrfoundRows = Modscan.publicDset.Tables(1).Select("TrxType ='0' AND VoucherCode = '40''  And IsGenerated=false") '  AND BankID = '" & BankArr.Item(k).ToString.Trim() & "
                            Case "BRMFO"
                                EFTCrfoundRows = Modscan.publicDset.Tables(1).Select("TrxType ='0' AND VoucherCode = '40'  And IsGenerated=false") 'AND BankID = '" & BankArr.Item(k).ToString.Trim() & "' 
                            Case "BRNET"
                                EFTCrfoundRows = Modscan.publicDset.Tables(1).Select("TrxType ='OC' AND VoucherCode = '40'  AND ReturnCode ='00'   And IsGenerated=false") 'AND BankID = '" & BankArr.Item(k).ToString.Trim() & "'
                            Case "BRNETOLD"
                                EFTCrfoundRows = Modscan.publicDTbl.Select("TrxType ='OD' AND VoucherCode = '40'   And IsGenerated=false") 'AND BankID = '" & BankArr.Item(k).ToString.Trim() & "'
                        End Select
                            publicDTblEFTCrCopy = Modscan.publicDset.Tables(0).Clone()
                            For j As Int32 = 0 To EFTCrfoundRows.Length - 1
                                publicDTblEFTCrCopy.ImportRow(EFTCrfoundRows(j))
                            Next
                            'Modscan.publicDset.Tables(1).Clear()
                            Try
                                If publicDTblEFTCrCopy.Rows.Count > 0 Then
                                    BIC = publicDTblEFTCrCopy(0)("SwiftCode")
                                End If
                            Catch ex As Exception
                                MessageBox.Show("Error Registered, Check ErrorLog")
                                Modscan.ErrorLog("Swift Code not maintained for this our BankID", "- DD Generation")
                            'Continue For
                        End Try
                            Try
                                If publicDTblEFTCrCopy.Rows.Count > 0 Then
                                    DestBankBIC = ""
                                    DestBankBIC = publicDTblEFTCrCopy(0)("DestinationSwiftCode").ToString()
                                End If
                            Catch ex As Exception
                                MessageBox.Show("Error Registered, Check ErrorLog")
                                Modscan.ErrorLog("Swift Code not maintained for this BankID " & publicDTblEJCopy(0)("BankID") & " DD generation for this Bank Abort", "- DD Generation")
                            'Continue For
                        End Try
                            Dim cr As New List(Of ETDDDetail)
                            For Each row As DataRow In publicDTblEFTCrCopy.Rows
                                Try
                                    Dim destBIC As String = row("DestinationSwiftCode").ToString()
                                    Dim SourceBIC As String = row("SwiftCode").ToString()
                                    Dim d As New ETDDDetail
                                    d.Amount = FormatNumber(row("Amount"), 2)
                                    d.CdtrAcct = row("CdtrAcct")
                                    d.Curr = CurrCode
                                    Try
                                        d.CNm = RegX.Replace(row("CNm").Trim(), " ")
                                        If d.CNm.Length > 55 Then d.CNm = d.CNm.Substring(0, 55)
                                    Catch ex As Exception
                                        d.CNm = ""
                                    End Try
                                    d.dBIC = destBIC
                                    d.DestBankID = row("BankID")
                                    d.SourceBankID = Modscan.OurBankID
                                    d.DbtrAcct = row("DbtrAcct")
                                    d.sBIC = SourceBIC
                                    d.DtOfSgntr = row("DtOfSgntr")
                                    d.Frqcy = row("Frqcy")
                                    d.MndtId = row("MndtId")
                                    d.IntrBkSttlmDt = Modscan.FDATE.ToString("dd-MMM-yyyy")
                                    Try
                                        d.ReqdColltnDt = Modscan.WORKING_DATE.ToString("dd-MMM-yyyy")
                                    Catch ex As Exception
                                        d.ReqdColltnDt = ""
                                    End Try


                                    Try
                                        d.DNm = RegX.Replace(row("DNm"), " ")
                                        If d.DNm.Length > 55 Then d.DNm = d.DNm.Substring(0, 55)
                                    Catch ex As Exception
                                        d.DNm = ""
                                    End Try
                                    d.VCode = row("VoucherCode").ToString()
                                    d.TrxId = row("ColumnID")
                                    d.InstrId = row("ColumnID")
                                    d.EndToEndId = row("ColumnID")
                                    cr.Add(d)
                                Catch ex As Exception
                                    MessageBox.Show("Error Registered, Check ErrorLog")
                                    Modscan.ErrorLog(ex.Message, "- DD Generation")
                                    Continue For
                                End Try

                            Next
                            If cr.Count > 0 Then
                                Dim msgId As String = BulkDebit(cr, cr(0).Curr, BIC)
                                For Each d As ETDDDetail In cr
                                    Select Case SystemType.ToUpper.Trim
                                        Case "BR"
                                        strAction = "UPDATE t_TransactionMICR SET IsGenerated = 1, Reference = '" & msgId & "' WHERE ColumnID = '" & d.TrxId & "' AND TrxType ='0' AND VoucherCode <> '40' " ' AND BankID = '" & BankArr.Item(k) & "'
                                    Case "BRMFO"
                                        strAction = "UPDATE t_TransactionMICR SET IsGenerated = 1, Reference = '" & msgId & "' WHERE ColumnID = '" & d.TrxId & "' AND TrxType ='0' AND VoucherCode <> '40'  " 'AND BankID = '" & BankArr.Item(k) & "'
                                    Case "BRNET"
                                            strAction = "UPDATE t_TrxClearing SET IsGenerated = 1 ,Reference = '" & msgId & "' WHERE TrxRowID = '" & d.TrxId & "'"
                                        Case "BRNETOLD"
                                        strAction = "UPDATE t_TrxClearing SET IsGenerated = 1, SessNo='1' ,Reference = '" & msgId & "' WHERE TrxRowID = '" & d.TrxId & "' AND TrxType ='OD' AND VoucherCode <> '40'  " 'AND BankID = '" & BankArr.Item(k) & "'
                                End Select
                                    Modscan.ExecuteData(strAction, Nothing, Modscan.dataExecTypes.ExecTypeNonQuery, Modscan.queryType.SelectStatement)
                                Next
                            End If
                        'strAction = "UPDATE t_Bank SET FileCounter = isNull(FileCounter,0) + 1 WHERE  BankID = '" & BankArr.Item(k) & "'"
                        'Modscan.ExecuteData(strAction, Nothing, Modscan.dataExecTypes.ExecTypeNonQuery, Modscan.queryType.SelectStatement)

                        'Next
                        publicDTblEFTCrCopy.Clear()
                    Case FileType.DDReturn
                        'For k As Int32 = 0 To BankArr.Count - 1
                        Try
                                Dim i As Integer = 0
                                Dim amt As Decimal = 0
                                'Dim FileCounter As String = GetScalarREC("Select isNull(FileCounter,0)+1 From t_Bank Where BankID = '" & BankArr.Item(k) & "'")
                                'Dim CreateFile As String = Session & Now.ToString(".yyyyMMdd.HHmm.") & BankArr.Item(k) & "." & Session
                                Dim BIC As String = ""
                                'EFT Cr Reject
                                'Modscan.ExecuteData(Modscan.GetModify("[Proc_CreateXMLFiles_Ethiopia_CTS]", "OurBankID", Modscan.OurBankID, "ourbranchid", Modscan.OurBranchID, "ReadDate", Modscan.cToDate, "FromDate", Modscan.cFromDate, "EJDate", Modscan.cWORKING_DATE, "ClearingCenters", "67", "AllCenters", 0, "Currency", CurrCode, "Session", 2), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure, Modscan.publicDset)
                                'Modscan.ExecuteData(Modscan.GetModify("Proc_CreateXMLFiles_UG_CTS", "OurBankID", Modscan.OurBankID, "ourbranchid", Modscan.OurBranchID, "ReadDate", Modscan.cWORKING_DATE, "FromDate", Modscan.cFromDate, "EJDate", Modscan.cToDate, "ClearingCenters", "47", "AllCenters", 0, "Currency", CurrCode, "FileType", "T", "Sessno", Session), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure, Modscan.publicDset)
                                Dim EFTCrfoundRows() As DataRow
                                'MessageBox.Show(SystemType.ToUpper.Trim)
                                Select Case SystemType.ToUpper.Trim
                                    Case "BR"
                                        EFTCrfoundRows = Modscan.publicDset.Tables(1).Select("TrxType ='0' AND VoucherCode = '40' AND ReturnCode <>'00'   And IsGenerated=false") 'AND  BankID = '" & BankArr.Item(k) & "'
                                    Case "BRMFO"
                                        EFTCrfoundRows = Modscan.publicDset.Tables(1).Select("TrxType ='0' AND VoucherCode = '40' AND ReturnCode <>'00'   And IsGenerated=false") ' AND  BankID = '" & BankArr.Item(k) & "'
                                    Case "BRNET"
                                        EFTCrfoundRows = Modscan.publicDset.Tables(1).Select("TrxType ='OC' AND VoucherCode = '40' AND ReturnCode <>'00'  And IsGenerated=false") 'AND BankID = '" & BankArr.Item(k) & "' 
                                    Case "BRNETOLD"
                                        EFTCrfoundRows = Modscan.publicDTbl.Select("TrxType ='OD' AND VoucherCode = '40' AND ReturnCode <>'00'   And IsGenerated=false") 'AND BankID = '" & BankArr.Item(k) & "'
                                End Select
                                publicDTblEFTCrCopy = Modscan.publicDset.Tables(1).Clone()
                                'MessageBox.Show("Imepata unpaid EFTs rows : " & EFTCrfoundRows.Length)
                                For j As Int32 = 0 To EFTCrfoundRows.Length - 1
                                    publicDTblEFTCrCopy.ImportRow(EFTCrfoundRows(j))
                                Next
                                'Modscan.publicDset.Tables(1).Clear()
                                Try
                                    If publicDTblEFTCrCopy.Rows.Count > 0 Then
                                        BIC = publicDTblEFTCrCopy(0)("SwiftCode")
                                    End If
                                Catch ex As Exception
                                    'MessageBox.Show(ex.Message)
                                    MessageBox.Show("Error Registered, Check ErrorLog")
                                    Modscan.ErrorLog("Swift Code not maintained for this our BankID", "- EFT Unpaid Generation")
                                'Continue For
                            End Try
                                Try
                                    If publicDTblEFTCrCopy.Rows.Count > 0 Then
                                        DestBankBIC = ""
                                        DestBankBIC = publicDTblEFTCrCopy(0)("DestinationSwiftCode").ToString()
                                    End If
                                Catch ex As Exception
                                    MessageBox.Show("Error Registered, Check ErrorLog")
                                    Modscan.ErrorLog("Swift Code not maintained for this BankID " & publicDTblEFTCrCopy(0)("BankID"), "- EFT Unpaid Generation")
                                'Continue For
                            End Try
                                Dim cr As New List(Of ETDDDetail)
                                For Each row As DataRow In publicDTblEFTCrCopy.Rows
                                    Try
                                        Dim destBIC As String = row("DestinationSwiftCode").ToString()
                                        Dim SourceBIC As String = row("SwiftCode").ToString()
                                        Dim d As New ETDDDetail
                                        d.Amount = FormatNumber(row("Amount"), 2)
                                        d.Curr = CurrCode
                                        d.dBIC = destBIC
                                        d.sBIC = SourceBIC
                                        d.DestBankID = row("BankID")
                                        d.MsgId = row("Reference")
                                        d.VCode = row("VoucherCode").ToString()
                                        d.Retcode = row("ReturnCode").ToString()
                                        d.RetCodeDesc = row("RetCodeDesc").ToString().ToUpper
                                        d.TrxId = row("TransactionMicrColumnID").ToString()
                                        d.OrgnTrxID = row("OrgnTrxID").ToString()
                                        d.OrgnlMsgId = row("OrgnlMsgId").ToString()
                                        d.OrgnlInstrID = row("OrgnlInstrID").ToString()
                                        d.CAdrLine = row("CAdrLine").ToString()
                                        d.CNm = row("CNm").ToString()
                                        d.CEmailAdr = row("CEmailAdr").ToString()
                                        d.CMobNb = row("CMobNb").ToString()
                                        d.CPhneNb = row("CPhneNb").ToString()
                                        d.COthr = row("COthr").ToString()
                                        d.CCtry = row("CCtry").ToString()
                                        d.CTwnNm = row("CTwnNm").ToString()
                                        d.DAdrLine = row("DAdrLine").ToString()
                                        d.DNm = row("DNm").ToString()
                                        d.DEmailAdr = row("DEmailAdr").ToString()
                                        d.DMobNb = row("DMobNb").ToString()
                                        d.DPhneNb = row("DPhneNb").ToString()
                                        d.DOthr = row("DOthr").ToString()
                                        d.DCtry = row("DCtry").ToString()
                                        d.MndtId = row("MndtId").ToString()
                                        'd.ReqdColltnDt = row("FnlColltnDt").ToString()
                                        d.DtOfSgntr = row("DtOfSgntr").ToString()
                                        d.Frqcy = row("Frqcy").ToString()
                                        d.DTwnNm = row("DTwnNm").ToString()
                                        d.DbtrAcct = row("DbtrAcct").ToString()
                                        d.CdtrAcct = row("CdtrAcct").ToString()
                                        d.UstrdColD = row("UstrdColD").ToString()
                                        d.EndToEndId = row("OrgnlEndToEnd").ToString()
                                        d.ReqdColltnDt = IIf(row("ReqdColltnDt").ToString() = "Jan  1 1900 12:00AM", "", row("ReqdColltnDt").ToString())
                                        cr.Add(d)
                                    Catch ex As Exception
                                        MessageBox.Show("Error Registered, Check ErrorLog")
                                        Modscan.ErrorLog(ex.Message, "- EFT Unpaids Generation")
                                        Continue For
                                    End Try
                                Next
                                If cr.Count > 0 Then
                                    Dim msgId As String = DDRejections(cr, cr(0).Curr, BIC)
                                    For Each d As ETDDDetail In cr
                                        Select Case SystemType.ToUpper.Trim
                                            Case "BR"
                                                strAction = "UPDATE t_TransactionMICR SET IsGenerated = 1, Remarks = '" & msgId & "' WHERE ColumnID = '" & d.TrxId & "' AND TrxType ='0' AND VoucherCode <> '40' AND ReturnCode <>'00' AND IsGenerated=1 " 'AND BankID = '" & BankArr.Item(k) & "'
                                            Case "BRMFO"
                                                strAction = "UPDATE t_TransactionMICR SET IsGenerated = 1, Remarks = '" & msgId & "' WHERE ColumnID = '" & d.TrxId & "' AND TrxType ='0' AND VoucherCode <> '40' AND ReturnCode <>'00' AND IsGenerated=1 " 'AND BankID = '" & BankArr.Item(k) & "'
                                            Case "BRNET"
                                                strAction = "UPDATE t_trxClearing SET IsGenerated = 1, Reference = '" & msgId & "' WHERE TrxRowID = '" & d.TrxId & "' "
                                            Case "BRNETOLD"
                                                strAction = "UPDATE t_trxClearing SET IsGenerated = 1, SessNo='1' ,'Reference = '" & msgId & "' WHERE TrxRowID = '" & d.TrxId & "' AND TrxType ='OD' AND VoucherCode <> '40' AND ReturnCodeID <>'00' AND IsGenerated=1 " 'AND BankID = '" & BankArr.Item(k) & "'
                                        End Select
                                        Modscan.ExecuteData(strAction, Nothing, Modscan.dataExecTypes.ExecTypeNonQuery, Modscan.queryType.SelectStatement)
                                    Next
                                End If
                                'strAction = "UPDATE t_Bank SET FileCounter = isNull(FileCounter,0) + 1 WHERE  BankID = '" & BankArr.Item(k) & "'"
                                'Modscan.ExecuteData(strAction, Nothing, Modscan.dataExecTypes.ExecTypeNonQuery, Modscan.queryType.SelectStatement)

                            Catch ex As Exception
                                MessageBox.Show("Error Registered, Check ErrorLog")
                                Modscan.ErrorLog(ex.Message, "- EFT Unpaid Generation")
                            'Continue For
                        End Try
                        'Next
                        publicDTblEFTCrCopy.Clear()

                    Case FileType.Messages
                End Select
                GenerateETHFiles = True
                publicDTblBankCopy.Clear()
            Catch ex As Exception
                GenerateETHFiles = False
            End Try
        End Function
        Private Shared Function ConvertListToXmlString(items As List(Of lsItems)) As String
            Dim serializer As New XmlSerializer(GetType(List(Of lsItems)))

            ' Remove XML declaration and root element
            Dim settings As New XmlWriterSettings()
            settings.OmitXmlDeclaration = True

            ' Write XML to a StringWriter
            Using writer As New StringWriter()
                Using xmlWriter As XmlWriter = XmlWriter.Create(writer, settings)
                    serializer.Serialize(xmlWriter, items)
                End Using

                ' Get the XML string from the StringWriter
                Dim xmlString As String = writer.ToString()

                Return xmlString
            End Using
        End Function
        Private Shared Function RemoveXmlHeaderAndRootElement(xml As String) As String
            Dim xdoc As XDocument = XDocument.Parse(xml)

            Dim k As List(Of XAttribute) = xdoc.Root.Attributes().ToList()
            Dim xsd As XAttribute = k(1)
            If xdoc.Root.HasAttributes Then xdoc.Root.Attribute(xsd.Name).Remove()
            Dim m As List(Of XElement) = xdoc.Descendants().ToList()
            xdoc.Descendants().Where(Function(p) p.IsEmpty Or String.IsNullOrEmpty(p.Value)).Remove()
            xdoc.Root.Attributes().Reverse()
            'xsd = k(1)
            'If xdoc.Root.HasAttributes Then xdoc.Root.Attribute(xsd.Name).Remove()
            'Dim root As XElement = xdoc.Root
            'root.Remove()
            'xdoc.Root.Remove()
            Return xdoc.ToString()
        End Function
        Private Shared Function BulkCredit(ByVal l As List(Of ETEFTDetails), ByVal ccy As String, ByVal BIC As String) As String
            Dim dAmt As Decimal = 0
            For Each itm As ETEFTDetails In l
                dAmt += CDec(itm.Amount)
            Next
            Dim stCurrCode As String = ""
            If ccy = "0" Then
                stCurrCode = "ETB"
            ElseIf ccy = "1" Then
                stCurrCode = "USD"
            ElseIf ccy = "2" Then
                stCurrCode = "GBP"
            ElseIf ccy = "3" Then
                stCurrCode = "EUR"
            ElseIf ccy = "4" Then
                stCurrCode = "JPY"
            ElseIf ccy = "5" Then
                stCurrCode = "KES"
            Else
                stCurrCode = "UGX"
            End If
            Dim amt As String = FormatNumber(dAmt, 2, Microsoft.VisualBasic.TriState.True, Microsoft.VisualBasic.TriState.True, Microsoft.VisualBasic.TriState.False)
            Dim sDt As String = Now.ToString("dd-MMM-yyyy")
            Dim STm As String = Now.ToString("HH:mm")
            Dim TimeSec As String = Now.ToString("HHmmss")
            Dim xDt As Date = CDate(sDt & " " & STm)
            Dim msgId As String = "OD/" & BIC & Modscan.WORKING_DATE.ToString("ddMMyy") & "/" & stCurrCode & Modscan.Sess & TimeSec
            Dim Filename As String = BIC & Now.ToString("ddMMyyyy") & Modscan.Sess & stCurrCode & l(0).SourceBankID & ".i"
            Dim doc As New cr.Document()
            Dim grpHdr As New BrClearing.Common.BRISO20022CT812.GroupHeader33()
            grpHdr.MsgId = msgId
            grpHdr.CreDtTm = xDt
            grpHdr.NbOfTxs = l.Count
            grpHdr.TtlIntrBkSttlmAmt = New cr.ActiveCurrencyAndAmount() With {.Ccy = stCurrCode, .Value = Decimal.Round(CDec(amt), 2)} 'Truncation cr.ActiveCurrencyCode.ETB
            grpHdr.IntrBkStDate = CDate(sDt)
            grpHdr.SttlmInf = New cr.SettlementInformation13() With {
            .SttlmMtd = cr.SettlementMethod1Code.CLRG
        }
            '.ClrSys = New cr.ClearingSystemIdentification3Choice() With {.Item = cr.ItemChoiceType1.ACH}, _
            grpHdr.InstgAgt = New cr.BranchAndFinancialInstitutionIdentification4 With {.FinInstnId = New cr.FinancialInstitutionIdentification7() With {.BIC = BIC}}
            doc.FIToFICstmrCdtTrf.GrpHdr = grpHdr
            For Each itm As ETEFTDetails In l
                Dim sBIC As String = itm.SourceBIC
                Dim dBIC As String = itm.DestBIC
                amt = FormatNumber(itm.Amount, 2, Microsoft.VisualBasic.TriState.True, Microsoft.VisualBasic.TriState.True, Microsoft.VisualBasic.TriState.False)
                Dim cdtTxn As New cr.CreditTransferTransactionInformation11()
                cdtTxn.PmtId = New cr.PaymentIdentification3() _
           With {.EndToEndId = itm.EFTID, .InstrId = itm.EFTID, .TxId = itm.EFTID}
                cdtTxn.PmtTpInf = New cr.PaymentTypeInformation21() With {.SvcLvl = New cr.ServiceLevel8Choice() With {.Item = "SEPA"}}
                cdtTxn.IntrBkSttlmAmt = New cr.ActiveCurrencyAndAmount() With {.Ccy = stCurrCode, .Value = Decimal.Round(CDec(amt), 2)}
                cdtTxn.ChrgBr = cr.ChargeBearerType1Code.SLEV
                cdtTxn.Dbtr = New cr.PartyIdentification32 With {.Nm = itm.RemitterName, .Id = New cr.Party6Choice() With {.Item = New cr.OrganisationIdentification4() With {.BICOrBEI = sBIC}}}
                cdtTxn.DbtrAcct = New cr.CashAccount16() With {.Id = New cr.AccountIdentification4Choice() With {.Item = itm.RemitterAcc}}
                cdtTxn.DbtrAgt = New cr.BranchAndFinancialInstitutionIdentification4 With {.FinInstnId = New cr.FinancialInstitutionIdentification7() With {.BIC = sBIC}}
                cdtTxn.CdtrAgt = New cr.BranchAndFinancialInstitutionIdentification4 With {.FinInstnId = New cr.FinancialInstitutionIdentification7() With {.BIC = dBIC}}
                cdtTxn.Cdtr = New cr.PartyIdentification32() With {.Nm = itm.BeneficiaryName, .Id = New cr.Party6Choice() With {.Item = New cr.OrganisationIdentification4() With {.BICOrBEI = dBIC}}}
                cdtTxn.CdtrAcct = New cr.CashAccount16() With {.Id = New cr.AccountIdentification4Choice() With {.Item = itm.BeneficiaryAcc}}
                cdtTxn.RmtInf = New cr.RemittanceInformation5() With {.Ustrd = {itm.RemittanceInfo}}
                doc.FIToFICstmrCdtTrf.CdtTrfTxInf.Add(cdtTxn)
            Next
            If Directory.Exists(TempLocation) Then
                Dim fullpath As String = Path.Combine(TempLocation, Filename)
                Dim ex As New Exception()
                If doc.SaveToFile(fullpath, ex) Then
                    Dim xDoc As XDocument = XDocument.Load(fullpath)
                    Dim k As List(Of XAttribute) = xDoc.Root.Attributes().ToList()
                    Dim xsd As XAttribute = k(1)
                    If xDoc.Root.HasAttributes Then xDoc.Root.Attribute(xsd.Name).Remove()
                    Dim m As List(Of XElement) = xDoc.Descendants().ToList()
                    Dim xCreTm As XElement = m(4)
                    Dim xStmDt As XElement = m(7)
                    xDoc.Descendants().ToList()(4).SetValue(CDate(xCreTm.Value).ToString("yyyy-MM-ddTHH:mm:sszzz"))
                    'xDoc.Descendants().ToList()(7).SetValue(CDate(xStmDt.Value).ToString("yyyy-MM-dd"))
                    xDoc.Descendants().Where(Function(p) p.IsEmpty Or String.IsNullOrEmpty(p.Value)).Remove()
                    xDoc.Root.Attributes().Reverse()
                    xDoc.Save(fullpath, SaveOptions.None)



                    'Dim Filter As String() = {"*.T*"}
                    'Dim di As New DirectoryInfo(TempLocation)
                    'Dim kl As New List(Of String)
                    'For Each f As String In Filter
                    '    Dim fi As FileInfo() = di.GetFiles(f)
                    '    For Each inf As FileInfo In fi
                    '        kl.Add(inf.FullName)
                    '    Next
                    'Next



                    'MessageBox.Show("Step 1")
                    Dim DestZippedFolderLoc As String = ConfigurationManager.AppSettings("OutgoingFiles") & "\Files"
                    If Directory.Exists(DestZippedFolderLoc) = False Then
                        Directory.CreateDirectory(DestZippedFolderLoc)
                    End If
                    'MessageBox.Show("Step 2")
                    Dim DestSignedFolderLoc As String = ConfigurationManager.AppSettings("OutgoingFiles") & "\SignedFiles"
                    If Directory.Exists(DestSignedFolderLoc) = False Then
                        Directory.CreateDirectory(DestSignedFolderLoc)
                    End If
                    'MessageBox.Show("Step 3")


                    Dim Destinationfile As String = String.Empty
                    If Sign Then
                        Destinationfile = DestZippedFolderLoc & "\" & Path.GetFileName(fullpath)
                    Else
                        Destinationfile = DestSignedFolderLoc & "\" & Path.GetFileName(fullpath)
                    End If

                    If File.Exists(Destinationfile) = False Then
                        File.Copy(fullpath, Destinationfile, True)
                    Else
                        File.Delete(Destinationfile)
                        File.Copy(fullpath, Destinationfile, True)
                    End If

                    'MessageBox.Show("Step 4")
                    Dim MessOut As String = ""
                    Dim CertPass As String = Encoding.ASCII.GetString(Convert.FromBase64String(ConfigurationManager.AppSettings("keypass")))
                    Try
                        'MessageBox.Show("fullpath " & fullpath)
                        'MessageBox.Show("DestZippedFolderLoc " & DestZippedFolderLoc)
                        'MessageBox.Show("DestSignedFolderLoc " & DestSignedFolderLoc)
                        Sign = Convert.ToBoolean(ConfigurationManager.AppSettings("Sign"))
                        If Sign Then
                            MessOut = SignFiles_PKCS(fullpath.Trim(), DestSignedFolderLoc.Trim(), "ET", CertPass.Trim(), "i")
                        End If

                        'MessageBox.Show("Step 6 ")
                        Dim ArchivePath As String
                        If MessOut = "success" Then
                            'MessageBox.Show("Step 7 ")
                            ArchivePath = ConfigurationManager.AppSettings("Archive")
                            If Directory.Exists(ArchivePath) = False Then
                                Directory.CreateDirectory(ArchivePath)
                            End If
                            'MessageBox.Show("Step 8 ")
                            'MessageBox.Show("ArchivePath " & ArchivePath)
                            'MessageBox.Show("DestSignedFolderLoc " & DestSignedFolderLoc)
                            Clear_Files_Arc(DestSignedFolderLoc.Trim(), ArchivePath.Trim(), DestSignedFolderLoc.Trim(), "Out")
                            'MessageBox.Show("Step 9 ")
                        Else
                            If Sign Then
                                MessageBox.Show("Failed Signing. " & Path.GetFileName(fullpath) & " : " & MessOut)
                                Modscan.ErrorLog(Path.GetFileName(fullpath) & " : " & MessOut, "- Signing ")
                            End If
                        End If
                        'MessageBox.Show("Step 10 ")
                    Catch exi As Exception
                        MessageBox.Show("Failed Signing. " & Path.GetFileName(fullpath) & " : " & MessOut)
                        Modscan.ErrorLog(Path.GetFileName(fullpath) & " : " & MessOut & " : " & exi.Message, "- Signing ")
                    End Try

                    Dim Filter As String() = New String() {"*.i"}
                    Dim SourceTempFolderLoc As String = ConfigurationManager.AppSettings("OutgoingFiles") & "\Temp"
                    'MessageBox.Show("Step 11 ")
                    Dim di As New DirectoryInfo(SourceTempFolderLoc)
                    Dim li As New List(Of String)
                    li = New List(Of String)()
                    For Each f As String In Filter
                        Dim fi As FileInfo() = di.GetFiles(f)
                        For Each inf As FileInfo In fi
                            li.Add(inf.FullName)
                        Next
                    Next

                    'MessageBox.Show("Step 12 ")
                    For Each itm As String In li
                        File.Delete(itm)
                    Next

                    SourceTempFolderLoc = ConfigurationManager.AppSettings("OutgoingFiles") & "\Files"
                    'MessageBox.Show("Step 11 ")
                    di = New DirectoryInfo(SourceTempFolderLoc)
                    li = New List(Of String)()
                    For Each f As String In Filter
                        Dim fi As FileInfo() = di.GetFiles(f)
                        For Each inf As FileInfo In fi
                            li.Add(inf.FullName)
                        Next
                    Next

                    'MessageBox.Show("Step 12 ")
                    For Each itm As String In li
                        File.Delete(itm)
                    Next
                    Return msgId
                End If
            End If
            Return Nothing
        End Function

        'Private Shared Function CancelCredit(ByVal l As List(Of ETEFTDetails), ByVal ccy As String, ByVal BIC As String, ByVal SttlDate As String) As String
        '    Try

        '        'MessageBox.Show("imefika kuwrite the file sasa")

        '        Dim sDt As String = Modscan.WORKING_DATE.AddDays(1).ToString("dd-MMM-yyyy")
        '        Dim sDt1 As String = Modscan.WORKING_DATE.AddDays(1).ToString("yyyy-MM-dd")
        '        Dim STm As String = Now.ToString("HH:mm")
        '        Dim STm1 As String = STm.Replace(":", "")
        '        Dim xDt As Date = CDate(sDt & " " & STm)
        '        Dim msgId As String = "CTUNP" & l(0).DestBIC & Modscan.FDATE.ToString("ddMMyyyy") & STm1
        '        Dim Filename As String = "CTUNP" & l(0).DestBIC & Modscan.FDATE.ToString("ddMMyyyy") & STm1 & l(0).SourceBankID & ".i"
        '        'MessageBox.Show("Filename" & Filename)
        '        Dim doc As New pcr.Document()
        '        Dim dAmt As Decimal = 0
        '        For Each itm As ETEFTDetails In l
        '            dAmt += CDec(itm.Amount)
        '        Next

        '        ' BIC = BIC.Substring(0, BIC.Length - 3)
        '        Dim amt As String = FormatNumber(dAmt, 2, Microsoft.VisualBasic.TriState.True, Microsoft.VisualBasic.TriState.True, Microsoft.VisualBasic.TriState.False)
        '        doc.PmtRtr.GrpHdr.MsgId = msgId
        '        doc.PmtRtr.GrpHdr.CreDtTm = xDt '.Item.FinInstnId.BIC = Left(l(0).DestBIC, 8)
        '        doc.PmtRtr.GrpHdr.NbOfTxs = l.Count '.Item.FinInstnId.BIC = Left(BIC, 8)
        '        'doc.PmtRtr.GrpHdr.TtlBkSttlmAmt = xDt
        '        doc.PmtRtr.GrpHdr.TtlRtrdIntrBkSttlmAmt = New pcr.ActiveCurrencyAndAmount() With {.Ccy = pcr.ActiveCurrencyCode.ETB, .Value = CDec(dAmt)}
        '        doc.PmtRtr.GrpHdr.IntrBkSttlmDt = SttlDate
        '        doc.PmtRtr.GrpHdr.SttlmInf = New pcr.SettlementInformation13() With {.ClrSys = New pcr.ClearingSystemIdentification3Choice() With {.Item = pcr.ClearingSystemIdentification.ACH}, .SttlmMtd = pcr.SettlementMethod1Code.CLRG}
        '        doc.PmtRtr.GrpHdr.InstgAgt = New pcr.FinancialInstitution4() With {.FinInstnId = New pcr.FinancialInstitutionIdentification7() With {.BIC = BIC}}
        '        For Each txn As ETEFTDetails In l
        '            Dim sAmount As String = FormatNumber(txn.Amount, 2, Microsoft.VisualBasic.TriState.False, Microsoft.VisualBasic.TriState.False, Microsoft.VisualBasic.TriState.False).Replace(",", "")
        '            Dim t As New pcr.PaymentTransactionInformation31()
        '            t.RtrId = "TRID:" + txn.TrxId
        '            t.RtrRsnInf.Orgtr.Item = New pcr.Party6Choice() With {.Item = New pcr.OrganisationIdentification4() With {.Item = Left(txn.DestBIC, 8)}}
        '            ' Dim rCode As New pcr.CancellationReason4Code
        '            t.RtrRsnInf.Rsn.Item = txn.RetCode
        '            t.OrgnlGrpInf.OrgnlMsgId = txn.OrgnlMsgId
        '            t.OrgnlGrpInf.OrgnlMsgNmId = "pacs.008.001.02"
        '            t.OrgnlEndToEndId = txn.OrgnlEndToEnd
        '            t.OrgnlInstrId = txn.OrgnlInstrID
        '            t.OrgnlTxId = txn.OrgnTrxID
        '            t.OrgnlIntrBkSttlmAmt.Ccy = pcr.ActiveCurrencyCode.ETB
        '            t.OrgnlIntrBkSttlmAmt.Value = sAmount
        '            t.RtrdIntrBkSttlmAmt.Value = sAmount 'txn.ValueDate
        '            Try
        '                t.OrgnlTxRef.IntrBkSttlmDt = txn.OrgnlIntrBkSttlmDt
        '            Catch ex As Exception
        '                Dim ReqdColltnDate As Date = Convert.ToDateTime(txn.ReqdColltnDt)
        '                t.OrgnlTxRef.IntrBkSttlmDt = ReqdColltnDate.ToString("yyyy-MM-dd")
        '            End Try
        '            If txn.ReqdColltnDt = "" Then
        '                'txnInf.OrgnlTxRef.ReqdColltnDt = Nothing
        '            Else
        '                Dim ReqdColltnDate As Date = Convert.ToDateTime(txn.ReqdColltnDt)
        '                t.OrgnlTxRef.IntrBkSttlmDt = ReqdColltnDate.ToString("yyyy-MM-dd")
        '            End If
        '            t.OrgnlTxRef.Cdtr.Id = New pcr.Party6Choice() With {.Item = New pcr.OrganisationIdentification4() With {.Item = Left(BIC, 8)}}
        '            t.OrgnlTxRef.Cdtr.Nm = txn.CNm
        '            t.OrgnlTxRef.CdtrAcct.Id.Item = txn.CdtrAcct
        '            t.OrgnlTxRef.CdtrAgt.FinInstnId.BIC = Left(BIC, 8)
        '            t.OrgnlTxRef.Dbtr.Nm = txn.DNm
        '            t.OrgnlTxRef.Dbtr.Id = New pcr.Party6Choice() With {.Item = New pcr.OrganisationIdentification4() With {.Item = Left(txn.DestBIC, 8)}}
        '            t.OrgnlTxRef.DbtrAcct.Id.Item = txn.DbtrAcct
        '            t.OrgnlTxRef.DbtrAgt.FinInstnId.BIC = Left(txn.DestBIC, 8)
        '            t.OrgnlTxRef.PmtTpInf.SvcLvl.Item = pcr.ServiceLevel3Code.SEPA
        '            t.OrgnlTxRef.PmtTpInf.LclInstrm = New pcr.LocalInstrument2Choice() With {.ItemElementName = pcr.ItemChoiceType2.Prtry}
        '            t.OrgnlTxRef.PmtTpInf.LclInstrm.Item = txn.LclInstrm
        '            t.OrgnlTxRef.PmtTpInf.CtgyPurp = New pcr.CategoryPurpose1Choice() With {.ItemElementName = pcr.ItemChoiceType3.Cd}
        '            t.OrgnlTxRef.PmtTpInf.CtgyPurp.Item = txn.CtgyPurp
        '            't.OrgnlTxRef.SttlmInf.ClrSys.Item = pcr.ClearingSystemIdentification.ACH--Prtry
        '            't.OrgnlTxRef.SttlmInf.SttlmMtd = pcr.SettlementMethod1Code.CLRG ' New pcr.SettlementInformation131() With {.ClrSys = New pcr.ClearingSystemIdentification3Choice() With {.Item = pcr.ClearingSystemIdentification.ACH}, .SttlmMtd = pcr.SettlementMethod1Code.CLRG}
        '            '  t.OrgnlTxRef.SttlmInf.SttlmMtd. ' = New pcr.SettlementInformation13() With {.ClrSys = New pcr.ClearingSystemIdentification3Choice() With {.Item = pcr.ClearingSystemIdentification.ACH}, .SttlmMtd = pcr.SettlementMethod1Code.CLRG}
        '            t.OrgnlTxRef.RmtInf.Item = txn.RemittanceInfo
        '            doc.PmtRtr.TxInf.Add(t)
        '        Next
        '        If Directory.Exists(TempLocation) Then
        '            Dim fullpath As String = Path.Combine(TempLocation, Filename)
        '            Dim ex As New Exception()
        '            If doc.SaveToFile(fullpath, ex) Then
        '                Dim xDoc As XDocument = XDocument.Load(fullpath)
        '                Dim k As List(Of XAttribute) = xDoc.Root.Attributes().ToList()
        '                Dim xsd As XAttribute = k(1)
        '                If xDoc.Root.HasAttributes Then
        '                    xDoc.Root.Attribute(xsd.Name).Remove()
        '                End If
        '                Dim m As List(Of XElement) = xDoc.Descendants().ToList()
        '                Dim xCreTm As XElement = m(12)
        '                'xDoc.Descendants().ToList()(12).SetValue(Convert.ToDateTime(xCreTm.Value).ToString("yyyy-MM-ddTHH:mm:sszzz"))
        '                xDoc.Descendants().Where(Function(p) p.IsEmpty Or String.IsNullOrEmpty(p.Value)).Remove()
        '                xDoc.Root.Attributes().Reverse()
        '                xDoc.Save(fullpath, SaveOptions.None)

        '                'MessageBox.Show("Step 1 Unpaid ")
        '                'MessageBox.Show("FullPath: " & fullpath)
        '                Dim DestZippedFolderLoc As String = ConfigurationManager.AppSettings("OutgoingFiles") & "\Files"
        '                If Directory.Exists(DestZippedFolderLoc) = False Then
        '                    Directory.CreateDirectory(DestZippedFolderLoc)
        '                End If
        '                'MessageBox.Show("Step 2 Unpaid ")
        '                Dim DestSignedFolderLoc As String = ConfigurationManager.AppSettings("OutgoingFiles") & "\SignedFiles"
        '                If Directory.Exists(DestSignedFolderLoc) = False Then
        '                    Directory.CreateDirectory(DestSignedFolderLoc)
        '                End If
        '                'MessageBox.Show("Step 3 Unpaid ")
        '                Dim Destinationfile As String = String.Empty
        '                If Sign Then
        '                    Destinationfile = DestZippedFolderLoc & "\" & Path.GetFileName(fullpath)
        '                Else
        '                    Destinationfile = DestSignedFolderLoc & "\" & Path.GetFileName(fullpath)
        '                End If
        '                'MessageBox.Show("Destinationfile :" & Destinationfile)
        '                Try

        '                    If File.Exists(Destinationfile) = False Then
        '                        File.Copy(fullpath, Destinationfile, True)
        '                    Else
        '                        File.Delete(Destinationfile)
        '                        File.Copy(fullpath, Destinationfile, True)
        '                    End If
        '                Catch exDestinationfile As Exception
        '                    MessageBox.Show(Destinationfile)
        '                End Try


        '                'MessageBox.Show(fullpath)
        '                Dim MessOut As String = ""
        '                Dim CertPass As String = Encoding.ASCII.GetString(Convert.FromBase64String(ConfigurationManager.AppSettings("keypass")))
        '                Try
        '                    'MessageBox.Show("Step 5 Unpaid ")
        '                    Try
        '                        Sign = Convert.ToBoolean(ConfigurationManager.AppSettings("Sign"))
        '                        If Sign Then
        '                            MessOut = SignFiles_PKCS(fullpath.Trim(), DestSignedFolderLoc.Trim(), CertName, CertPass.Trim(), "i")
        '                        End If
        '                    Catch exi As Exception
        '                        MessageBox.Show("error Step 5 Unpaid " & ex.Message)
        '                    End Try

        '                    'MessageBox.Show("Step 6 ")
        '                    Dim ArchivePath As String
        '                    If MessOut = "success" Then
        '                        'MessageBox.Show("Step 7 ")
        '                        ArchivePath = ConfigurationManager.AppSettings("Archive")
        '                        If Directory.Exists(ArchivePath) = False Then
        '                            Directory.CreateDirectory(ArchivePath)
        '                        End If
        '                        'MessageBox.Show("Step 8 ")
        '                        Clear_Files_Arc(DestSignedFolderLoc.Trim(), ArchivePath.Trim(), DestSignedFolderLoc.Trim(), "Out")
        '                        'MessageBox.Show("Step 9 ")
        '                    Else
        '                        If Sign Then
        '                            MessageBox.Show("Failed Signing Unpaid. " & Path.GetFileName(fullpath) & " : " & MessOut)
        '                            Modscan.ErrorLog(Path.GetFileName(fullpath) & " : " & MessOut, "- Signing ")
        '                        End If
        '                    End If
        '                    'MessageBox.Show("Step 10 ")
        '                Catch exp As Exception
        '                    MessageBox.Show("Failed Signing Unpaid. " & Path.GetFileName(fullpath) & " : " & MessOut)
        '                    Modscan.ErrorLog(Path.GetFileName(fullpath) & " : " & MessOut & " : " & exp.Message, "- Signing ")
        '                End Try

        '                Dim Filter As String() = New String() {"*.i"}
        '                Dim SourceTempFolderLoc As String = ConfigurationManager.AppSettings("OutgoingFiles") & "\Temp"
        '                'MessageBox.Show("Step 11 ")
        '                Dim di As New DirectoryInfo(SourceTempFolderLoc)
        '                Dim li As New List(Of String)
        '                li = New List(Of String)()
        '                For Each f As String In Filter
        '                    Dim fi As FileInfo() = di.GetFiles(f)
        '                    For Each inf As FileInfo In fi
        '                        li.Add(inf.FullName)
        '                    Next
        '                Next

        '                'MessageBox.Show("Step 12 ")
        '                For Each itm As String In li
        '                    File.Delete(itm)
        '                Next

        '                SourceTempFolderLoc = ConfigurationManager.AppSettings("OutgoingFiles") & "\Files"
        '                'MessageBox.Show("Step 11 ")
        '                di = New DirectoryInfo(SourceTempFolderLoc)
        '                li = New List(Of String)()
        '                For Each f As String In Filter
        '                    Dim fi As FileInfo() = di.GetFiles(f)
        '                    For Each inf As FileInfo In fi
        '                        li.Add(inf.FullName)
        '                    Next
        '                Next

        '                'MessageBox.Show("Step 12 ")
        '                For Each itm As String In li
        '                    File.Delete(itm)
        '                Next

        '                Return msgId
        '            End If
        '        End If
        '    Catch ex As Exception
        '        'MessageBox.Show("Imechapa when writing the file -: " & ex.Message)
        '    End Try
        '    Return Nothing
        'End Function
        Private Shared Function CancelCredit(ByVal l As List(Of ETEFTDetails), ByVal ccy As String, ByVal BIC As String, ByVal SttlDate As String) As String
            Try

                'MessageBox.Show("imefika kuwrite the file sasa")

                Dim sDt As String = Modscan.WORKING_DATE.AddDays(1).ToString("dd-MMM-yyyy")
                Dim sDt1 As String = Modscan.WORKING_DATE.AddDays(1).ToString("yyyy-MM-dd")
                Dim STm As String = Now.ToString("HH:mm")
                Dim STm1 As String = STm.Replace(":", "")
                Dim xDt As Date = CDate(sDt & " " & STm)
                Dim msgId As String = "CTUNP" & l(0).DestBIC & Modscan.FDATE.ToString("ddMMyyyy") & STm1
                Dim Filename As String = "CTUNP" & l(0).DestBIC & Modscan.FDATE.ToString("ddMMyyyy") & STm1 & l(0).SourceBankID & ".i"
                'MessageBox.Show("Filename" & Filename)
                Dim doc As New pcr.Document()
                Dim dAmt As Decimal = 0
                For Each itm As ETEFTDetails In l
                    dAmt += CDec(itm.Amount)
                Next

                ' BIC = BIC.Substring(0, BIC.Length - 3)
                Dim amt As String = FormatNumber(dAmt, 2, TriState.True, TriState.True, TriState.False)
                doc.PmtRtr.GrpHdr.MsgId = msgId
                doc.PmtRtr.GrpHdr.CreDtTm = xDt '.Item.FinInstnId.BIC = Left(l(0).DestBIC, 8)
                doc.PmtRtr.GrpHdr.NbOfTxs = l.Count '.Item.FinInstnId.BIC = Left(BIC, 8)
                'doc.PmtRtr.GrpHdr.TtlBkSttlmAmt = xDt
                doc.PmtRtr.GrpHdr.TtlRtrdIntrBkSttlmAmt = New pcr.ActiveCurrencyAndAmount() With {.Ccy = pcr.ActiveCurrencyCode.TZS, .Value = CDec(dAmt)}
                doc.PmtRtr.GrpHdr.IntrBkSttlmDt = SttlDate
                doc.PmtRtr.GrpHdr.SttlmInf = New pcr.SettlementInformation13() With {.ClrSys = New pcr.ClearingSystemIdentification3Choice() With {.Item = pcr.ClearingSystemIdentification.ACH}, .SttlmMtd = pcr.SettlementMethod1Code.CLRG}
                doc.PmtRtr.GrpHdr.InstgAgt = New pcr.FinancialInstitution4() With {.FinInstnId = New pcr.FinancialInstitutionIdentification7() With {.BIC = BIC}}
                For Each txn As ETEFTDetails In l
                    Dim sAmount As String = FormatNumber(txn.Amount, 2, TriState.False, TriState.False, TriState.False).Replace(",", "")
                    Dim t As New pcr.PaymentTransactionInformation31()
                    t.RtrId = "TRID:" + txn.TrxId
                    t.RtrRsnInf.Orgtr.Item = New pcr.Party6Choice() With {.Item = New pcr.OrganisationIdentification4() With {.Item = Left(txn.DestBIC, 8)}}
                    ' Dim rCode As New pcr.CancellationReason4Code
                    t.RtrRsnInf.Rsn.Item = txn.RetCode
                    t.OrgnlGrpInf.OrgnlMsgId = txn.OrgnlMsgId
                    t.OrgnlGrpInf.OrgnlMsgNmId = "pacs.008.001.02"
                    t.OrgnlEndToEndId = txn.OrgnlEndToEnd
                    t.OrgnlInstrId = txn.OrgnlInstrID
                    t.OrgnlTxId = txn.OrgnTrxID
                    t.OrgnlIntrBkSttlmAmt.Ccy = pcr.ActiveCurrencyCode.TZS
                    t.OrgnlIntrBkSttlmAmt.Value = sAmount
                    t.RtrdIntrBkSttlmAmt.Value = sAmount 'txn.ValueDate
                    t.OrgnlTxRef.IntrBkSttlmDt = txn.OrgnlIntrBkSttlmDt
                    If txn.ReqdColltnDt = "" Then
                        'txnInf.OrgnlTxRef.ReqdColltnDt = Nothing
                    Else
                        Dim ReqdColltnDate As Date = Convert.ToDateTime(txn.ReqdColltnDt)
                        t.OrgnlTxRef.IntrBkSttlmDt = ReqdColltnDate.ToString("yyyy-MM-dd")
                    End If
                    t.OrgnlTxRef.Cdtr.Id = New pcr.Party6Choice() With {.Item = New pcr.OrganisationIdentification4() With {.Item = Left(BIC, 8)}}
                    t.OrgnlTxRef.Cdtr.Nm = txn.CNm
                    t.OrgnlTxRef.CdtrAcct.Id.Item = txn.CdtrAcct
                    t.OrgnlTxRef.CdtrAgt.FinInstnId.BIC = Left(BIC, 8)
                    t.OrgnlTxRef.Dbtr.Nm = txn.DNm
                    t.OrgnlTxRef.Dbtr.Id = New pcr.Party6Choice() With {.Item = New pcr.OrganisationIdentification4() With {.Item = Left(txn.DestBIC, 8)}}
                    t.OrgnlTxRef.DbtrAcct.Id.Item = txn.DbtrAcct
                    t.OrgnlTxRef.DbtrAgt.FinInstnId.BIC = Left(txn.DestBIC, 8)
                    t.OrgnlTxRef.PmtTpInf.SvcLvl.Item = pcr.ServiceLevel3Code.SEPA
                    t.OrgnlTxRef.PmtTpInf.LclInstrm = New pcr.LocalInstrument2Choice() With {.ItemElementName = pcr.ItemChoiceType2.Prtry}
                    t.OrgnlTxRef.PmtTpInf.LclInstrm.Item = txn.LclInstrm
                    t.OrgnlTxRef.PmtTpInf.CtgyPurp = New pcr.CategoryPurpose1Choice() With {.ItemElementName = pcr.ItemChoiceType3.Cd}
                    t.OrgnlTxRef.PmtTpInf.CtgyPurp.Item = txn.CtgyPurp
                    't.OrgnlTxRef.SttlmInf.ClrSys.Item = pcr.ClearingSystemIdentification.ACH--Prtry
                    't.OrgnlTxRef.SttlmInf.SttlmMtd = pcr.SettlementMethod1Code.CLRG ' New pcr.SettlementInformation131() With {.ClrSys = New pcr.ClearingSystemIdentification3Choice() With {.Item = pcr.ClearingSystemIdentification.ACH}, .SttlmMtd = pcr.SettlementMethod1Code.CLRG}
                    '  t.OrgnlTxRef.SttlmInf.SttlmMtd. ' = New pcr.SettlementInformation13() With {.ClrSys = New pcr.ClearingSystemIdentification3Choice() With {.Item = pcr.ClearingSystemIdentification.ACH}, .SttlmMtd = pcr.SettlementMethod1Code.CLRG}
                    t.OrgnlTxRef.RmtInf.Item = txn.RemittanceInfo
                    doc.PmtRtr.TxInf.Add(t)
                Next
                If Directory.Exists(TempLocation) Then
                    Dim fullpath As String = Path.Combine(TempLocation, Filename)
                    Dim ex As New Exception()
                    If doc.SaveToFile(fullpath, ex) Then
                        Dim xDoc As XDocument = XDocument.Load(fullpath)
                        Dim k As List(Of XAttribute) = xDoc.Root.Attributes().ToList()
                        Dim xsd As XAttribute = k(1)
                        If xDoc.Root.HasAttributes Then
                            xDoc.Root.Attribute(xsd.Name).Remove()
                        End If
                        Dim m As List(Of XElement) = xDoc.Descendants().ToList()
                        Dim xCreTm As XElement = m(12)
                        'xDoc.Descendants().ToList()(12).SetValue(Convert.ToDateTime(xCreTm.Value).ToString("yyyy-MM-ddTHH:mm:sszzz"))
                        xDoc.Descendants().Where(Function(p) p.IsEmpty Or String.IsNullOrEmpty(p.Value)).Remove()
                        xDoc.Root.Attributes().Reverse()
                        xDoc.Save(fullpath, SaveOptions.None)

                        'MessageBox.Show("Step 1 Unpaid ")
                        'MessageBox.Show("FullPath: " & fullpath)
                        Dim DestZippedFolderLoc As String = ConfigurationManager.AppSettings("OutgoingFiles") & "\Files"
                        If Directory.Exists(DestZippedFolderLoc) = False Then
                            Directory.CreateDirectory(DestZippedFolderLoc)
                        End If
                        'MessageBox.Show("Step 2 Unpaid ")
                        Dim DestSignedFolderLoc As String = ConfigurationManager.AppSettings("OutgoingFiles") & "\SignedFiles"
                        If Directory.Exists(DestSignedFolderLoc) = False Then
                            Directory.CreateDirectory(DestSignedFolderLoc)
                        End If
                        'MessageBox.Show("Step 3 Unpaid ")
                        Dim Destinationfile As String = DestZippedFolderLoc & "\" & Path.GetFileName(fullpath)
                        'MessageBox.Show("Destinationfile :" & Destinationfile)
                        Try
                            If File.Exists(Destinationfile) = False Then
                                File.Copy(fullpath, Destinationfile, True)
                            Else
                                File.Delete(Destinationfile)
                                File.Copy(fullpath, Destinationfile, True)
                            End If
                        Catch exDestinationfile As Exception
                            MessageBox.Show(Destinationfile)
                        End Try


                        'MessageBox.Show(fullpath)
                        Dim MessOut As String = ""
                        Dim CertPass As String = Encoding.ASCII.GetString(Convert.FromBase64String(ConfigurationManager.AppSettings("keypass")))
                        Try
                            'MessageBox.Show("Step 5 Unpaid ")
                            Try
                                Sign = Convert.ToBoolean(ConfigurationManager.AppSettings("Sign"))
                                If Sign Then
                                    MessOut = SignFiles_PKCS(fullpath.Trim(), DestSignedFolderLoc.Trim(), CertName, CertPass.Trim(), "i")
                                End If
                            Catch exi As Exception
                                MessageBox.Show("error Step 5 Unpaid " & ex.Message)
                            End Try

                            'MessageBox.Show("Step 6 ")
                            Dim ArchivePath As String
                            If MessOut = "success" Then
                                'MessageBox.Show("Step 7 ")
                                ArchivePath = ConfigurationManager.AppSettings("Archive")
                                If Directory.Exists(ArchivePath) = False Then
                                    Directory.CreateDirectory(ArchivePath)
                                End If
                                'MessageBox.Show("Step 8 ")
                                Clear_Files_Arc(DestSignedFolderLoc.Trim(), ArchivePath.Trim(), DestSignedFolderLoc.Trim(), "Out")
                                'MessageBox.Show("Step 9 ")
                            Else
                                MessageBox.Show("Failed Signing Unpaid. " & Path.GetFileName(fullpath) & " : " & MessOut)
                                Modscan.ErrorLog(Path.GetFileName(fullpath) & " : " & MessOut, "- Signing ")
                            End If
                            'MessageBox.Show("Step 10 ")
                        Catch exp As Exception
                            MessageBox.Show("Failed Signing Unpaid. " & Path.GetFileName(fullpath) & " : " & MessOut)
                            Modscan.ErrorLog(Path.GetFileName(fullpath) & " : " & MessOut & " : " & exp.Message, "- Signing ")
                        End Try

                        Dim Filter As String() = New String() {"*.i"}
                        Dim SourceTempFolderLoc As String = ConfigurationManager.AppSettings("OutgoingFiles") & "\Temp"
                        'MessageBox.Show("Step 11 ")
                        Dim di As New DirectoryInfo(SourceTempFolderLoc)
                        Dim li As New List(Of String)
                        li = New List(Of String)()
                        For Each f As String In Filter
                            Dim fi As FileInfo() = di.GetFiles(f)
                            For Each inf As FileInfo In fi
                                li.Add(inf.FullName)
                            Next
                        Next

                        'MessageBox.Show("Step 12 ")
                        For Each itm As String In li
                            File.Delete(itm)
                        Next

                        SourceTempFolderLoc = ConfigurationManager.AppSettings("OutgoingFiles") & "\Files"
                        'MessageBox.Show("Step 11 ")
                        di = New DirectoryInfo(SourceTempFolderLoc)
                        li = New List(Of String)()
                        For Each f As String In Filter
                            Dim fi As FileInfo() = di.GetFiles(f)
                            For Each inf As FileInfo In fi
                                li.Add(inf.FullName)
                            Next
                        Next

                        'MessageBox.Show("Step 12 ")
                        For Each itm As String In li
                            File.Delete(itm)
                        Next

                        Return msgId
                    End If
                End If
            Catch ex As Exception
                'MessageBox.Show("Imechapa when writing the file -: " & ex.Message)
            End Try
            Return Nothing
        End Function

        Private Shared Function CancelCreditq(ByVal credits As List(Of ETEFTDetails), ByVal ccy As String, ByVal BIC As String, ByVal SttlDate As String) As String
            Dim Rx As Regex = New Regex("[^A-Za-z0-9 ]")
            Dim sDt As String = Modscan.WORKING_DATE.ToString("dd-MMM-yyyy")
            Dim STm As String = Now.ToString("HH:mm")
            Dim xDt As Date = CDate(sDt & " " & STm)
            Dim msgId As String = "CTREJ" & BIC & xDt.ToString("yyyyMMddHHmmss")
            Dim filName As String = "CTREJ" & BIC & Now.ToString("ddMMyyyy") & STm.Replace(":", "")
            Dim stCurrCode As String = ""
            If ccy = "0" Then
                stCurrCode = "ETB"
            ElseIf ccy = "1" Then
                stCurrCode = "USD"
            ElseIf ccy = "2" Then
                stCurrCode = "GBP"
            ElseIf ccy = "3" Then
                stCurrCode = "EUR"
            ElseIf ccy = "4" Then
                stCurrCode = "JPY"
            ElseIf ccy = "5" Then
                stCurrCode = "KES"
            Else
                stCurrCode = "ETB" 'ccy
            End If
            Dim final = Path.Combine(strFileLocation & "\Files\", filName & ".i")
            Dim temp = Path.Combine(strFileLocation & "\Temp\", filName & ".i")
            Dim currencyCode As res.ActiveCurrencyCode
            [Enum].TryParse(ccy.ToString(), True, currencyCode)
            Dim doc = New cx.Document With {
        .FIToFIPmtCxlReq = New cx.FIToFIPaymentCancellationRequestV01 With {
            .Assgnmt = New cx.CaseAssignment2 With {
                .Assgnr = New cx.Party7Choice With {
                    .Item = New cx.FinancialInstitution4 With {
                        .FinInstnId = New cx.FinancialInstitutionIdentification7 With {
                            .BIC = BIC
                        }
                    }
                },
                .Assgne = New cx.Party7Choice With {
                    .Item = New cx.FinancialInstitution4 With {
                        .FinInstnId = New cx.FinancialInstitutionIdentification7 With {
                            .BIC = "NBETETAB"
                        }
                    }
                },
                .CreDtTm = xDt,
                .Id = msgId
            },
            .CtrlData = New cx.ControlData1 With {
                .NbOfTxs = credits.Count.ToString()
            },
            .Undrlyg = (From p In credits Select New cx.PaymentTransactionInformation31 With {
                .CxlId = "TRID" & p.TrxId,
                .CxlRsnInf = New cx.CancellationReasonInformation3 With {
                    .Orgtr = New cx.PartyIdentification35 With {
                        .Id = New cx.Party6Choice With {
                            .Item = New cx.OrganisationIdentification4 With {
                                .Item = BIC
                            }
                        }
                    },
                    .Rsn = New cx.CancellationReason2Choice With {
                        .Item = p.RetCode
                    }
                },
                .OrgnlEndToEndId = p.OrgnlEndToEnd,
                .OrgnlGrpInf = New cx.OriginalGroupInformation3 With {
                    .OrgnlMsgId = p.OrgnlMsgId,
                    .OrgnlMsgNmId = "pacs.008.001.02"
                },
                .OrgnlInstrId = p.OrgnlInstrID,
                .OrgnlIntrBkSttlmAmt = New cx.ActiveCurrencyAndAmount With {
                    .Ccy = currencyCode,
                    .Value = FormatNumber(p.Amount, 2, TriState.True, TriState.True, TriState.False)
                },
                .OrgnlIntrBkSttlmDt = p.OrgnlIntrBkSttlmDt,
                .OrgnlTxId = p.OrgnTrxID,
                .OrgnlTxRef = New cx.OriginalTransactionReference13 With {
                    .Cdtr = New cx.PartyIdentification33 With {
                        .Id = New cx.Party6Choice With {
                            .Item = New cx.OrganisationIdentification4 With {
                                .Item = p.DestBIC
                            }
                        },
                        .Nm = Rx.Replace(p.CNm, "")
                    },
                    .CdtrAcct = New cx.CashAccount17 With {
                        .Id = New cx.AccountIdentification4Choice With {
                            .Item = p.CdtrAcct
                        }
                    },
                    .CdtrAgt = New cx.FinancialInstitution4 With {
                        .FinInstnId = New cx.FinancialInstitutionIdentification7 With {
                            .BIC = p.DestBIC
                        }
                    },
                    .Dbtr = New cx.PartyIdentification33 With {
                        .Id = New cx.Party6Choice With {
                            .Item = New cx.OrganisationIdentification4 With {
                                .Item = BIC
                            }
                        },
                        .Nm = Rx.Replace(p.DNm, "")
                    },
                    .DbtrAcct = New cx.CashAccount17 With {
                        .Id = New cx.AccountIdentification4Choice With {
                            .Item = p.DbtrAcct
                        }
                    },
                    .DbtrAgt = New cx.FinancialInstitution4 With {
                        .FinInstnId = New cx.FinancialInstitutionIdentification7 With {
                            .BIC = BIC
                        }
                    },
                    .PmtTpInf = New cx.PaymentTypeInformation22 With {
                        .SvcLvl = New cx.ServiceLevel9Choice With {
                            .Item = cx.ServiceLevel3Code.SEPA
                        }
                    },
                    .SttlmInf = New cx.SettlementInformation13 With {
                        .ClrSys = New cx.ClearingSystemIdentification3Choice With {
                            .Item = cx.ClearingSystemIdentification.ACH
                        },
                        .SttlmMtd = cx.SettlementMethod1Code.CLRG
                    },
                    .RmtInf = New cx.RemittanceInformation5 With {
                        .Item = Rx.Replace(p.RemittanceInfo, "")
                    }
                }
            }).ToList()
        }
    }
            Dim ex As Exception

            If doc.SaveToFile(temp, ex) Then
                Dim xDoc As XDocument = XDocument.Load(temp)

                If xDoc.Root IsNot Nothing Then
                    Dim k As List(Of XAttribute) = xDoc.Root.Attributes().ToList()
                    Dim xsd As XAttribute = k(1)

                    If xDoc.Root.HasAttributes Then
                        Dim xAttribute = xDoc.Root.Attribute(xsd.Name)
                        If xAttribute IsNot Nothing Then xAttribute.Remove()
                    End If
                End If

                xDoc.Descendants().Where(Function(p) p.IsEmpty Or String.IsNullOrEmpty(p.Value)).Remove()
                If xDoc.Root IsNot Nothing Then xDoc.Root.Attributes().Reverse()
                xDoc.Save(temp, SaveOptions.None)
            End If

            Dim fileName As String = temp
            'MessageBox.Show("Step 1 Unpaid ")
            Dim DestZippedFolderLoc As String = ConfigurationManager.AppSettings("OutgoingFiles") & "\Files"
            If Directory.Exists(DestZippedFolderLoc) = False Then
                Directory.CreateDirectory(DestZippedFolderLoc)
            End If
            'MessageBox.Show("Step 2 Unpaid ")
            Dim DestSignedFolderLoc As String = ConfigurationManager.AppSettings("OutgoingFiles") & "\SignedFiles"
            If Directory.Exists(DestSignedFolderLoc) = False Then
                Directory.CreateDirectory(DestSignedFolderLoc)
            End If


            Dim Destinationfile As String = String.Empty
            If Sign Then
                Destinationfile = DestZippedFolderLoc & "\" & Path.GetFileName(fileName)
            Else
                Destinationfile = DestSignedFolderLoc & "\" & Path.GetFileName(fileName)
            End If

            'MessageBox.Show("Step 3 Unpaid ")
            If File.Exists(Destinationfile) = False Then
                File.Copy(fileName, Destinationfile, True)
            Else
                File.Delete(Destinationfile)
                File.Copy(fileName, Destinationfile, True)
            End If


            'MessageBox.Show(fileName)
            Dim MessOut As String = ""
            Dim CertPass As String = Encoding.ASCII.GetString(Convert.FromBase64String(ConfigurationManager.AppSettings("keypass")))
            Try
                'MessageBox.Show("Step 5 Unpaid ")
                Try
                    Sign = Convert.ToBoolean(ConfigurationManager.AppSettings("Sign"))
                    If Sign Then
                        MessOut = SignFiles_PKCS(fileName.Trim(), DestSignedFolderLoc.Trim(), CertName, CertPass.Trim(), "i")
                    End If
                Catch exi As Exception
                    MessageBox.Show("error Step 5 Unpaid " & ex.Message)
                End Try

                'MessageBox.Show("Step 6 ")
                Dim ArchivePath As String
                If MessOut = "success" Then
                    'MessageBox.Show("Step 7 ")
                    ArchivePath = ConfigurationManager.AppSettings("Archive")
                    If Directory.Exists(ArchivePath) = False Then
                        Directory.CreateDirectory(ArchivePath)
                    End If
                    'MessageBox.Show("Step 8 ")
                    Clear_Files_Arc(DestSignedFolderLoc.Trim(), ArchivePath.Trim(), DestSignedFolderLoc.Trim(), "Out")
                    'MessageBox.Show("Step 9 ")
                Else
                    If Sign Then
                        MessageBox.Show("Failed Signing Unpaid. " & Path.GetFileName(fileName) & " : " & MessOut)
                        Modscan.ErrorLog(Path.GetFileName(fileName) & " : " & MessOut, "- Signing ")
                    End If
                End If
                'MessageBox.Show("Step 10 ")
            Catch exp As Exception
                MessageBox.Show("Failed Signing Unpaid. " & Path.GetFileName(fileName) & " : " & MessOut)
                Modscan.ErrorLog(Path.GetFileName(fileName) & " : " & MessOut & " : " & exp.Message, "- Signing ")
            End Try
            Return msgId
        End Function


        Private Shared Function BulkDebit(ByVal l As List(Of ETDDDetail), ByVal ccy As String, ByVal BIC As String) As String
            Dim dAmt As Decimal = 0
            For Each itm As ETDDDetail In l
                dAmt += CDec(itm.Amount)
            Next
            Dim amt As String = FormatNumber(dAmt, 2, Microsoft.VisualBasic.TriState.True, Microsoft.VisualBasic.TriState.True, Microsoft.VisualBasic.TriState.False)
            Dim sDt As String = Now.AddDays(1).ToString("dd-MMM-yyyy")
            Dim cDt As String = Now.ToString("dd-MMM-yyyy")
            Dim STm As String = Now.ToString("HH:mm")
            Dim TimeSec As String = Now.ToString("HHmmss")
            Dim xDt As Date = CDate(cDt & " " & STm)
            Dim msgId As String = "DD" & BIC & Modscan.WORKING_DATE.ToString("ddMMyyyy") & TimeSec
            Dim Filename As String = "DD" & BIC & Now.ToString("ddMMyyyy") & ".i"
            Dim doc As New dr.Document()
            Dim grpHdr As New dr.GroupHeader34()
            grpHdr.MsgId = msgId
            grpHdr.CreDtTm = xDt
            grpHdr.NbOfTxs = l.Count
            grpHdr.TtlIntrBkSttlmAmt = New dr.ActiveCurrencyAndAmount() With {.Ccy = dr.ActiveCurrencyCode.ETB, .Value = CDec(amt)}
            grpHdr.IntrBkSttlmDt = CDate(l(0).IntrBkSttlmDt)
            grpHdr.SttlmInf = New dr.SettlementInformation14() With {
                .ClrSys = New dr.ClearingSystemIdentification3Choice() With {.Item = dr.ClearingSystemIdentification.ACH},
                .SttlmMtd = dr.SettlementMethod1Code.CLRG
            }
            grpHdr.InstgAgt = New dr.FinancialInstitution4() With {.FinInstnId = New dr.FinancialInstitutionIdentification7() With {.BIC = BIC}}
            doc.FIToFICstmrDrctDbt.GrpHdr = grpHdr
            For Each itm As ETDDDetail In l
                Dim sBIC As String = itm.sBIC
                Dim dBIC As String = itm.dBIC
                amt = FormatNumber(itm.Amount, 2, Microsoft.VisualBasic.TriState.True, Microsoft.VisualBasic.TriState.True, Microsoft.VisualBasic.TriState.False)
                Dim dbtTxn As New dr.DirectDebitTransactionInformation10()
                dbtTxn.PmtId.EndToEndId = itm.EndToEndId
                dbtTxn.PmtId.InstrId = itm.InstrId
                dbtTxn.PmtId.TxId = itm.TrxId
                dbtTxn.PmtTpInf.SvcLvl.Item = dr.ServiceLevel3Code.SEPA
                dbtTxn.PmtTpInf.LclInstrm.Item = "B2B"
                dbtTxn.PmtTpInf.SeqTp = dr.SequenceType1Code.FRST
                dbtTxn.IntrBkSttlmAmt = New dr.ActiveCurrencyAndAmount() With {.Ccy = dr.ActiveCurrencyCode.ETB, .Value = CDec(amt)}
                dbtTxn.ChrgBr = dr.ChargeBearerType1Code.SLEV
                dbtTxn.ReqdColltnDt = CDate(l(0).IntrBkSttlmDt)
                dbtTxn.DrctDbtTx.MndtRltdInf.DtOfSgntr = (CDate(itm.DtOfSgntr)).ToString("yyyy-MM-dd")
                dbtTxn.DrctDbtTx.MndtRltdInf.MndtId = itm.MndtId
                dbtTxn.DrctDbtTx.MndtRltdInf.AmdmntInd = False
                dbtTxn.DrctDbtTx.MndtRltdInf.AmdmntIndSpecified = True
                dbtTxn.DrctDbtTx.CdtrSchmeId.Id.Item.Othr.Id = "Debit"
                dbtTxn.Dbtr = New dr.PartyIdentification33() With {.Nm = itm.DNm, .Id = New dr.Party6Choice() With {.Item = New dr.OrganisationIdentification4() With {.Item = dBIC}}}
                dbtTxn.DbtrAcct = New dr.CashAccount17() With {.Id = New dr.AccountIdentification4Choice() With {.Item = itm.DbtrAcct}}
                dbtTxn.DbtrAgt = New dr.FinancialInstitution4() With {.FinInstnId = New dr.FinancialInstitutionIdentification7() With {.BIC = dBIC}}
                dbtTxn.CdtrAgt = New dr.FinancialInstitution4() With {.FinInstnId = New dr.FinancialInstitutionIdentification7() With {.BIC = sBIC}}
                dbtTxn.Cdtr = New dr.PartyIdentification33() With {.Nm = itm.CNm} '.Id = New dr.Party6Choice() With {.Item = New dr.OrganisationIdentification4() With {.Item = sBIC}}
                dbtTxn.CdtrAcct = New dr.CashAccount17() With {.Id = New dr.AccountIdentification4Choice() With {.Item = itm.CdtrAcct}}
                dbtTxn.RmtInf.Item = "Test" 'itm.Remittance
                doc.FIToFICstmrDrctDbt.DrctDbtTxInf.Add(dbtTxn)
            Next
            If Directory.Exists(TempLocation) Then
                Dim fullpath As String = Path.Combine(TempLocation, msgId & ".i")
                Dim ex As New Exception()
                If doc.SaveToFile(fullpath, ex) Then
                    Dim xDoc As XDocument = XDocument.Load(fullpath)
                    Dim k As List(Of XAttribute) = xDoc.Root.Attributes().ToList()
                    Dim xsd As XAttribute = k(1)
                    If xDoc.Root.HasAttributes Then xDoc.Root.Attribute(xsd.Name).Remove()
                    Dim m As List(Of XElement) = xDoc.Descendants().ToList()
                    Dim xCreTm As XElement = m(4)
                    Dim xStmDt As XElement = m(7)
                    xDoc.Descendants().ToList()(4).SetValue(CDate(xCreTm.Value).ToString("yyyy-MM-ddTHH:mm:sszzz"))
                    xDoc.Descendants().ToList()(7).SetValue(CDate(xStmDt.Value).ToString("yyyy-MM-dd"))
                    For i As Integer = 0 To m.Count - 1
                        Dim xE As XElement = m(i)
                        If xE.Name.LocalName = "ReqdColltnDt" Or xE.Name.LocalName = "DtOfSgntr" Then
                            xDoc.Descendants().ToList()(i).SetValue(CDate(xE.Value).ToString("yyyy-MM-ddzzz"))
                        End If
                    Next
                    xDoc.Descendants().Where(Function(p) p.IsEmpty Or String.IsNullOrEmpty(p.Value)).Remove()
                    xDoc.Root.Attributes().Reverse()
                    xDoc.Save(fullpath, SaveOptions.None)

                    Dim DestZippedFolderLoc As String = ConfigurationManager.AppSettings("OutgoingFiles") & "\Files"
                    If Directory.Exists(DestZippedFolderLoc) = False Then
                        Directory.CreateDirectory(DestZippedFolderLoc)
                    End If
                    'MessageBox.Show("Step 2")
                    Dim DestSignedFolderLoc As String = ConfigurationManager.AppSettings("OutgoingFiles") & "\SignedFiles"
                    If Directory.Exists(DestSignedFolderLoc) = False Then
                        Directory.CreateDirectory(DestSignedFolderLoc)
                    End If
                    'MessageBox.Show("Step 3")


                    Dim Destinationfile As String = String.Empty
                    If Sign Then
                        Destinationfile = DestZippedFolderLoc & "\" & Path.GetFileName(fullpath)
                    Else
                        Destinationfile = DestSignedFolderLoc & "\" & Path.GetFileName(fullpath)
                    End If

                    If File.Exists(Destinationfile) = False Then
                        File.Copy(fullpath, Destinationfile, True)
                    Else
                        File.Delete(Destinationfile)
                        File.Copy(fullpath, Destinationfile, True)
                    End If

                    'MessageBox.Show("Step 4")
                    Dim MessOut As String = ""
                    Dim CertPass As String = Encoding.ASCII.GetString(Convert.FromBase64String(ConfigurationManager.AppSettings("keypass")))
                    Try
                        'MessageBox.Show("fullpath " & fullpath)
                        'MessageBox.Show("DestZippedFolderLoc " & DestZippedFolderLoc)
                        'MessageBox.Show("DestSignedFolderLoc " & DestSignedFolderLoc)
                        Sign = Convert.ToBoolean(ConfigurationManager.AppSettings("Sign"))
                        If Sign Then
                            MessOut = SignFiles_PKCS(fullpath.Trim(), DestSignedFolderLoc.Trim(), "ET", CertPass.Trim(), "i")
                        End If

                        'MessageBox.Show("Step 6 ")
                        Dim ArchivePath As String
                        If MessOut = "success" Then
                            'MessageBox.Show("Step 7 ")
                            ArchivePath = ConfigurationManager.AppSettings("Archive")
                            If Directory.Exists(ArchivePath) = False Then
                                Directory.CreateDirectory(ArchivePath)
                            End If
                            'MessageBox.Show("Step 8 ")
                            'MessageBox.Show("ArchivePath " & ArchivePath)
                            'MessageBox.Show("DestSignedFolderLoc " & DestSignedFolderLoc)
                            Clear_Files_Arc(DestSignedFolderLoc.Trim(), ArchivePath.Trim(), DestSignedFolderLoc.Trim(), "Out")
                            'MessageBox.Show("Step 9 ")
                        Else
                            If Sign Then
                                MessageBox.Show("Failed Signing. " & Path.GetFileName(fullpath) & " : " & MessOut)
                                Modscan.ErrorLog(Path.GetFileName(fullpath) & " : " & MessOut, "- Signing ")
                            End If
                        End If
                        'MessageBox.Show("Step 10 ")
                    Catch exi As Exception
                        MessageBox.Show("Failed Signing. " & Path.GetFileName(fullpath) & " : " & MessOut)
                        Modscan.ErrorLog(Path.GetFileName(fullpath) & " : " & MessOut & " : " & exi.Message, "- Signing ")
                    End Try

                    Dim Filter As String() = New String() {"*.i"}
                    Dim SourceTempFolderLoc As String = ConfigurationManager.AppSettings("OutgoingFiles") & "\Temp"
                    'MessageBox.Show("Step 11 ")
                    'Dim di As New DirectoryInfo(SourceTempFolderLoc)
                    'Dim li As New List(Of String)
                    'li = New List(Of String)()
                    'For Each f As String In Filter
                    '    Dim fi As FileInfo() = di.GetFiles(f)
                    '    For Each inf As FileInfo In fi
                    '        li.Add(inf.FullName)
                    '    Next
                    'Next

                    'MessageBox.Show("Step 12 ")
                    'For Each itm As String In li
                    File.Delete(fullpath)
                    'Next

                    SourceTempFolderLoc = ConfigurationManager.AppSettings("OutgoingFiles") & "\Files"
                    'MessageBox.Show("Step 11 ")
                    'di = New DirectoryInfo(SourceTempFolderLoc)
                    'li = New List(Of String)()
                    'For Each f As String In Filter
                    '    Dim fi As FileInfo() = di.GetFiles(f)
                    '    For Each inf As FileInfo In fi
                    '        li.Add(inf.FullName)
                    '    Next
                    'Next

                    ''MessageBox.Show("Step 12 ")
                    'For Each itm As String In li
                    '    File.Delete(itm)
                    'Next
                    Return msgId
                End If
            End If
            Return Nothing
        End Function

        Private Shared Function DDRejections(ByVal debits As List(Of ETDDDetail), ByVal ccy As String, ByVal BIC As String) As String
            Dim sDt As String = Modscan.FDATE.ToString("dd-MMM-yyyy")
            Dim STm As String = Now.ToString("HH:mm")
            Dim xDt As Date = CDate(sDt & " " & STm)
            Dim msgId As String = "DDREJ" & BIC & xDt.ToString("yyyyMMddHHmmss")
            Dim stCurrCode As String = ""
            If ccy = "0" Then
                stCurrCode = "ETB"
            ElseIf ccy = "1" Then
                stCurrCode = "USD"
            ElseIf ccy = "2" Then
                stCurrCode = "GBP"
            ElseIf ccy = "3" Then
                stCurrCode = "EUR"
            ElseIf ccy = "4" Then
                stCurrCode = "JPY"
            ElseIf ccy = "5" Then
                stCurrCode = "KES"
            Else
                stCurrCode = "UGX"
            End If
            Dim final = Path.Combine(strFileLocation & "\Files\", msgId & ".i")
            Dim temp = Path.Combine(strFileLocation & "\Temp\", msgId & ".i")
            Dim currencyCode As res.ActiveCurrencyCode
            [Enum].TryParse(ccy.ToString(), True, currencyCode)
            Dim doc = New res.Document With {
                .FIToFIPmtStsRpt = New res.FIToFIPaymentStatusReportV03 With {
                    .GrpHdr = New res.GroupHeader37 With {
                        .CreDtTm = xDt,
                        .InstgAgt = New res.FinancialInstitution4 With {
                            .FinInstnId = New res.FinancialInstitutionIdentification7 With {
                                .BIC = BIC
                            }
                        },
                        .MsgId = msgId
                    },
                    .OrgnlGrpInfAndSts = New res.OriginalGroupInformation20 With {
                        .OrgnlMsgId = debits(0).OrgnlMsgId,
                        .OrgnlMsgNmId = "pacs.003.001.02"
                    },
                    .TxInfAndSts = (From p In debits Select New res.PaymentTransactionInformation26 With {
                        .StsId = p.TrxId,
                        .OrgnlEndToEndId = p.EndToEndId.Trim(),
                        .OrgnlTxId = p.OrgnTrxID.Trim(),
                        .OrgnlInstrId = p.OrgnlInstrID.Trim(),
                        .StsRsnInf = New res.StatusReasonInformation8 With {
                            .Orgtr = New res.PartyIdentification34 With {
                                .Item = New res.Party6Choice With {
                                    .Item = New res.OrganisationIdentification4 With {
                                        .Item = p.dBIC
                                    }
                                }
                            },
                            .Rsn = New res.StatusReason6Choice With {
                                .Item = p.Retcode
                            }
                        },
                        .TxSts = res.TransactionIndividualStatus3Code.RJCT,
                        .TxStsSpecified = True,
                        .OrgnlTxRef = New res.OriginalTransactionReference13 With {
                            .IntrBkSttlmAmt = New res.ActiveCurrencyAndAmount With {
                                .Ccy = currencyCode,
                                .Value = FormatNumber(p.Amount, 2, Microsoft.VisualBasic.TriState.False, Microsoft.VisualBasic.TriState.False, Microsoft.VisualBasic.TriState.False).Replace(",", "")
                            },
                            .IntrBkSttlmDtSpecified = True,
                            .IntrBkSttlmDtt = p.ReqdColltnDt,
                            .ReqdColltnDt = p.ReqdColltnDt,
                            .ReqdColltnDtSpecified = True,
                            .SttlmInf = New res.SettlementInformation13 With {
                                .SttlmMtd = res.SettlementMethod1Code.CLRG,
                                .ClrSys = New res.ClearingSystemIdentification3Choice With {
                                    .Item = res.ClearingSystemIdentification.ACH
                                }
                            },
                            .PmtTpInf = New res.PaymentTypeInformation22 With {
                                .SvcLvl = New res.ServiceLevel9Choice With {
                                    .Item = res.ServiceLevel3Code.SEPA
                                },
                                .LclInstrm = New res.LocalInstrument2Choice With {
                                    .ItemElementName = res.ItemChoiceType2.Cd,
                                    .Item = "B2B"
                                },
                                .SeqTp = res.SequenceType1Code.FRST,
                                .SeqTpSpecified = True
                            },
                            .MndtRltdInf = New res.MandateRelatedInformation6 With {
                                .AmdmntInd = False,
                                .AmdmntIndSpecified = True,
                                .DtOfSgntr = p.DtOfSgntr,
                                .MndtId = p.MndtId
                            },
                            .Dbtr = New res.PartyIdentification33 With {
                                .Id = New res.Party6Choice With {
                                    .Item = New res.OrganisationIdentification4 With {
                                        .Item = p.sBIC
                                    }
                                },
                                .Nm = p.DNm
                            },
                            .DbtrAcct = New res.CashAccount17 With {
                                .Id = New res.AccountIdentification4Choice With {
                                    .Item = p.DbtrAcct
                                }
                            },
                            .DbtrAgt = New res.FinancialInstitution4 With {
                                .FinInstnId = New res.FinancialInstitutionIdentification7 With {
                                    .BIC = p.sBIC
                                }
                            },
                            .CdtrAgt = New res.FinancialInstitution4 With {
                                .FinInstnId = New res.FinancialInstitutionIdentification7 With {
                                    .BIC = p.dBIC
                                }
                            },
                            .CdtrAcct = New res.CashAccount17 With {
                                .Id = New res.AccountIdentification4Choice With {
                                    .Item = p.CdtrAcct
                                }
                            },
                            .Cdtr = New res.PartyIdentification33 With {
                                .Id = New res.Party6Choice With {
                                    .Item = New res.OrganisationIdentification4 With {
                                        .Item = p.dBIC
                                    }
                                },
                                .Nm = p.CNm
                            },
                            .RmtInf = New res.RemittanceInformation5 With {
                                .Item = p.Remittance
                            }
                        }
                    }).ToList()
                }
            }
            Dim ex As Exception

            If doc.SaveToFile(temp, ex) Then
                Dim xDoc As XDocument = XDocument.Load(temp)

                If xDoc.Root IsNot Nothing Then
                    Dim k As List(Of XAttribute) = xDoc.Root.Attributes().ToList()
                    Dim xsd As XAttribute = k(1)

                    If xDoc.Root.HasAttributes Then
                        Dim xAttribute = xDoc.Root.Attribute(xsd.Name)
                        If xAttribute IsNot Nothing Then xAttribute.Remove()
                    End If
                End If

                Dim m As List(Of XElement) = xDoc.Descendants().ToList()
                Dim xCreTm As XElement = m(4)
                xDoc.Descendants().ToList()(4).SetValue(Convert.ToDateTime(xCreTm.Value).ToString("yyyy-MM-ddTHH:mm:sszzz"))
                xDoc.Descendants().Where(Function(p) p.IsEmpty Or String.IsNullOrEmpty(p.Value)).Remove()
                If xDoc.Root IsNot Nothing Then xDoc.Root.Attributes().Reverse()
                xDoc.Save(temp, SaveOptions.None)
            End If

            Dim fileName As String = temp
            'MessageBox.Show("Step 1 Unpaid ")
            Dim DestZippedFolderLoc As String = ConfigurationManager.AppSettings("OutgoingFiles") & "\Files"
            If Directory.Exists(DestZippedFolderLoc) = False Then
                Directory.CreateDirectory(DestZippedFolderLoc)
            End If
            'MessageBox.Show("Step 2 Unpaid ")
            Dim DestSignedFolderLoc As String = ConfigurationManager.AppSettings("OutgoingFiles") & "\SignedFiles"
            If Directory.Exists(DestSignedFolderLoc) = False Then
                Directory.CreateDirectory(DestSignedFolderLoc)
            End If


            Dim Destinationfile As String = String.Empty
            If Sign Then
                Destinationfile = DestZippedFolderLoc & "\" & Path.GetFileName(fileName)
            Else
                Destinationfile = DestSignedFolderLoc & "\" & Path.GetFileName(fileName)
            End If

            'MessageBox.Show("Step 3 Unpaid ")
            If File.Exists(Destinationfile) = False Then
                File.Copy(fileName, Destinationfile, True)
            Else
                File.Delete(Destinationfile)
                File.Copy(fileName, Destinationfile, True)
            End If


            'MessageBox.Show(fileName)
            Dim MessOut As String = ""
            Dim CertPass As String = Encoding.ASCII.GetString(Convert.FromBase64String(ConfigurationManager.AppSettings("keypass")))
            Try
                'MessageBox.Show("Step 5 Unpaid ")
                Try
                    Sign = Convert.ToBoolean(ConfigurationManager.AppSettings("Sign"))
                    If Sign Then
                        MessOut = SignFiles_PKCS(fileName.Trim(), DestSignedFolderLoc.Trim(), CertName, CertPass.Trim(), "Q")
                    End If
                Catch exi As Exception
                    MessageBox.Show("error Step 5 Unpaid " & ex.Message)
                End Try

                'MessageBox.Show("Step 6 ")
                Dim ArchivePath As String
                If MessOut = "success" Then
                    'MessageBox.Show("Step 7 ")
                    ArchivePath = ConfigurationManager.AppSettings("Archive")
                    If Directory.Exists(ArchivePath) = False Then
                        Directory.CreateDirectory(ArchivePath)
                    End If
                    'MessageBox.Show("Step 8 ")
                    Clear_Files_Arc(DestSignedFolderLoc.Trim(), ArchivePath.Trim(), DestSignedFolderLoc.Trim(), "Out")
                    'MessageBox.Show("Step 9 ")
                Else
                    If Sign Then
                        MessageBox.Show("Failed Signing Unpaid. " & Path.GetFileName(fileName) & " : " & MessOut)
                        Modscan.ErrorLog(Path.GetFileName(fileName) & " : " & MessOut, "- Signing ")
                    End If
                End If
                'MessageBox.Show("Step 10 ")
            Catch exp As Exception
                MessageBox.Show("Failed Signing Unpaid. " & Path.GetFileName(fileName) & " : " & MessOut)
                Modscan.ErrorLog(Path.GetFileName(fileName) & " : " & MessOut & " : " & exp.Message, "- Signing ")
            End Try
            Return msgId
        End Function

        Private Shared Function GetScalarREC(ByVal strStatementORstrProcedure As String, Optional ByVal strArr As String = "") As String
            Dim strResults As String = ""
            Try
                If Modscan.GetModify(strStatementORstrProcedure, strArr) = "" Then
                    Modscan.ExecuteData(strStatementORstrProcedure, Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.SelectStatement, Nothing)
                Else
                    Modscan.ExecuteData(Modscan.GetModify(strStatementORstrProcedure, strArr), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.SelectStatement, Nothing)
                End If
                If Modscan.publicDTbl.Rows.Count >= 1 Then
                    strResults = Modscan.publicDTbl(0)(0)
                Else
                    strResults = Nothing
                End If
            Catch ex As Exception
                strResults = Nothing
            End Try
            Modscan.publicDTbl.Clear()
            Return strResults
        End Function


        Private Shared Function SISTransaction(ByVal det As ETChequeDetails) As Boolean
            Dim strDetail As String = ""
            'CREATE CHEQUE DETAIL
            strDetail += "#Cheque Properties" & vbNewLine
            strDetail += "#" & Now.ToString("ddd MMM dd HH:mm:ss EAT yyyy") & vbNewLine
            strDetail += "Transaction\ Code=1" & vbNewLine 'det.TransCode & vbNewLine
            strDetail += "Branch\ Code=" & det.BranchCode & vbNewLine
            strDetail += "Creation\ Date=" & det.CreationDate & vbNewLine
            strDetail += "Beneficiary\ Account=" & det.BeneficiaryAcc & vbNewLine
            strDetail += "Cheque\ Number=" & det.ChequeNumber & vbNewLine
            strDetail += "Cheque\ Index=" & det.ChequeIndex & vbNewLine
            strDetail += "Remittance\ Information=" & det.RemittanceInfo & vbNewLine
            strDetail += "Bank\ Code=" & det.BankCode & vbNewLine
            strDetail += "Amount = " & det.Amount * 100 & vbNewLine
            strDetail += "Microcode = " & IIf(det.MICRED, det.Codeline, "NO_MICROCODE") & vbNewLine
            strDetail += "Payer\ Name=" & det.RemitterName & vbNewLine
            strDetail += "Endorsement\ Number=" & det.EndorsmentNo & vbNewLine
            strDetail += "Currency\ Code=" & IIf(det.CurrencyCode = "ETB", "1", "2") & vbNewLine
            strDetail += "Beneficiary\ Name=" & det.BeneficiaryName & vbNewLine
            strDetail += "File\ Name=" & det.FileName & vbNewLine
            strDetail += "Payer\ Account=" & det.RemitterAcc & vbNewLine
            WriteFile(strDetail, det.EndorsmentNo, det.ChequeIndex & ".chequeItem")

            Dim fCreate As String = Path.Combine(TempLocation, det.ChequeIndex)
            'Write the front image
            Using fs As New FileStream(fCreate & ".chequeFrontImage", FileMode.Create)
                fs.Write(det.FrontImageGS, 0, det.FrontImageGS.Length)
            End Using
            'Write the back image
            Using fs As New FileStream(fCreate & ".chequeRearImage", FileMode.Create)
                fs.Write(det.BackImageGS, 0, det.BackImageGS.Length)
            End Using
        End Function

        Private Shared Function BulkCheques(ByVal det As List(Of ETChequeDetails), ByVal ccy As String, ByVal amt As Decimal, ByVal BIC As String) As String
            Dim sDt As String = Modscan.FDATE.ToString("dd-MMM-yyyy")
            Dim STm As String = Now.ToString("HH:mm")
            Dim TimeSec As String = Now.ToString("HHmmss")
            Dim MsgIdDt As String = Modscan.WORKING_DATE.ToString("ddMMyyyy")
            Dim xDt As Date = CDate(sDt & " " & STm)
            Dim insDt As Date = CDate(sDt)
            Dim ReceivingBank As String
            Dim ICounter As Int16 = 0
            Dim stCurrCode As String = ""
            If ccy = "0" Then
                stCurrCode = "ETB"
            ElseIf ccy = "1" Then
                stCurrCode = "USD"
            ElseIf ccy = "2" Then
                stCurrCode = "GBP"
            ElseIf ccy = "3" Then
                stCurrCode = "EUR"
            ElseIf ccy = "4" Then
                stCurrCode = "JPY"
            ElseIf ccy = "5" Then
                stCurrCode = "KES"
            Else
                stCurrCode = "UGX"
            End If
            'Dim FileCounter As String = GetScalarREC("Select isNull(FileCounter,0)+1 From t_Bank Where BankID = '" & det(0).BankCode & "'")
            Dim ImageUniqueNames As String = det(0).BankCode & Modscan.FDATE.ToString("ddMMyyyy") & ccy '& FileCounter.ToString().PadLeft(2, "0")
            'Dim msgId As String = "OC" & det(0).BankCode & Modscan.FDATE.ToString("ddMMyyyy") & STm & Modscan.Sess & GetNextString()

            Dim msgId As String = det(0).BankBIC & Modscan.FDATE.ToString("ddMMyy") & ccy & TimeSec
            Dim sAmount As String = FormatNumber(amt, 2, Microsoft.VisualBasic.TriState.False, Microsoft.VisualBasic.TriState.False, Microsoft.VisualBasic.TriState.False).Replace(",", "")
            Dim doc As New ch.Document()
            Dim grpHdr As New ch.GroupHeader34()
            Dim ImageUniqueName As String = ""
            Dim fCreate As String = TempLocation & "\"
            If File.Exists(fCreate) Then fCreate &= "_1"
            ImageUniqueName = "CH" + det(0).BankCode & Modscan.FDATE.ToString("ddMMyyyy") & DateTime.Now.ToString("HHmm") & "1"
            ReceivingBank = det(0).BankCode & Modscan.FDATE.ToString("ddMMyyyy") & DateTime.Now.ToString("HHmm")
            'grpHdr.MsgId = "OC/" & Modscan.FDATE.ToString("ddMMyy") & "/" & ccy & det(0).BankCode.Substring(0, 2)
            grpHdr.MsgId = det(0).BankBIC & Modscan.FDATE.ToString("ddMMyy") & Modscan.Sess & TimeSec
            grpHdr.CreDtTm = xDt
            grpHdr.NbOfTxs = det.Count
            grpHdr.TtlIntrBkSttlmAmt = New ch.ActiveCurrencyAndAmount() With {.Ccy = ch.ActiveCurrencyCode.ETB, .Value = Decimal.Round(CDec(sAmount), 2)}
            grpHdr.IntrBkSttlmDt = Modscan.FDATE.ToString("dd-MMM-yyyy")
            grpHdr.SttlmInf = New ch.SettlementInformation14() _
        With {.SttlmMtd = ch.SettlementMethod1Code.CLRG, .ClrSys = New ch.ClearingSystemIdentification3Choice() _
             With {.Item = ch.ClearingSystemIdentification.ACH}}
            grpHdr.InstgAgt = New ch.FinancialInstitution4() _
        With {.FinInstnId = New ch.FinancialInstitutionIdentification7() With {.BIC = BIC}}
            doc.BlkChq.GrpHdr = grpHdr
            For Each d As ETChequeDetails In det
                sAmount = FormatNumber(d.Amount, 2, Microsoft.VisualBasic.TriState.False, Microsoft.VisualBasic.TriState.False, Microsoft.VisualBasic.TriState.False).Replace(",", "")
                'Temporarly before Ethiopia gets real cheque 
                d.MICRED = False
                '------------------------------------------
                If Not d.MICRED Then d.Codeline = "NO_MICROCODE"
                Dim c As New ch.ChequeType()
                c.PmtId = New ch.PaymentIdentification3() _
            With {.EndToEndId = d.ChequeNumber, .TxId = d.trxID & d.ChequeNumber}
                c.PmtTpInf = New ch.PaymentTypeInformation22() _
                With
                {
                .SvcLvl = New ch.ServiceLevel9Choice() With {.Item = ch.ServiceLevel3Code.SEPA},
                .LclInstrm = New ch.LocalInstrument3Choice() With {.Item = "CORE"}
                }
                c.IntrBkSttlmAmt = New ch.ActiveCurrencyAndAmount() With {.Ccy = ch.ActiveCurrencyCode.ETB, .Value = sAmount}
                c.ChrgBr = ch.ChargeBearerType1Code.SLEV
                c.ChequeTx = New ch.ChequeDetails() _
            With {.ChkNmbr = d.ChequeNumber.ToString(), .AccNo = d.RemitterAcc, .Microcode = d.Codeline, .BankCode = d.BankCode, .BranchCode = d.BranchCode}
                c.Cdtr = New ch.PartyIdentification33() With {.Nm = d.BeneficiaryName}
                c.CdtrAcct = New ch.CashAccount17() With {.Id = New ch.AccountIdentification4Choice() With {.Item = d.BeneficiaryAcc}}
                c.CdtrAgt = New ch.FinancialInstitution4() With {.FinInstnId = New ch.FinancialInstitutionIdentification7() With {.BIC = BIC}}
                c.Dbtr = New ch.PartyIdentification33() _
            With {.Nm = d.RemitterName, .Id = New ch.Party6Choice() With {.Item = New ch.OrganisationIdentification4() With {.Item = d.BankBIC}}}
                c.DbtrAcct = New ch.CashAccount17() With {.Id = New ch.AccountIdentification4Choice() With {.Item = d.RemitterAcc}}
                c.DbtrAgt = New ch.FinancialInstitution4() With {.FinInstnId = New ch.FinancialInstitutionIdentification7() With {.BIC = d.BankBIC}}
                doc.BlkChq.Chq.Add(c)
                'Dim fCreate As String = Path.Combine(TempLocation, d.ChequeNumber)
                'Write the front image
                If File.Exists(fCreate & d.trxID & d.ChequeNumber & "_front.tiff") Then fCreate &= "_1"
                Using fs As New FileStream(fCreate & d.trxID & d.ChequeNumber & "_front.tiff", FileMode.Create)
                    fs.Write(d.FrontImageGS, 0, d.FrontImageGS.Length)
                End Using
                'Write the back image
                If File.Exists(fCreate & d.trxID & d.ChequeNumber & "_back.tiff") Then fCreate &= "_1"
                Using fs As New FileStream(fCreate & d.trxID & d.ChequeNumber & "_back.tiff", FileMode.Create)
                    fs.Write(d.BackImageGS, 0, d.BackImageGS.Length)
                End Using

                ''Write the uv image
                'If File.Exists(fCreate & d.trxID & d.trxID & "_UV.tiff") Then fCreate &= "_1"
                'Using fs As New FileStream(fCreate & d.trxID & "_UV.tiff", FileMode.Create)
                '    fs.Write(d.FrontImageUV, 0, d.FrontImageUV.Length)
                'End Using

                ''Write the Tif image
                'If File.Exists(fCreate & d.trxID & d.trxID & "_BW.tiff") Then fCreate &= "_1"
                'Using fs As New FileStream(fCreate & d.trxID & "_BW.tiff", FileMode.Create)
                '    fs.Write(d.FrontImageBW, 0, d.FrontImageBW.Length)
                'End Using
            Next
            If Not Directory.Exists(StrDestinationFilePath) Then Directory.CreateDirectory(StrDestinationFilePath)
            'If Not Directory.Exists(TempLocation) Then Directory.CreateDirectory(TempLocation)
            If Directory.Exists(StrDestinationFilePath) Then
                Dim fullpath As String = Path.Combine(TempLocation, ImageUniqueName + ".xml")

                'MessageBox.Show("fullpath writing chq file " & fullpath)
                If File.Exists(fullpath) Then fullpath &= "_1"
                Dim ex As New Exception()
                If doc.SaveToFile(fullpath, ex) Then
                    Dim xDoc As XDocument = XDocument.Load(fullpath)
                    Dim k As List(Of XAttribute) = xDoc.Root.Attributes().ToList()
                    Dim xsd As XAttribute = k(1)
                    If xDoc.Root.HasAttributes Then xDoc.Root.Attribute(xsd.Name).Remove()
                    Dim m As List(Of XElement) = xDoc.Descendants().ToList()
                    Dim xCreTm As XElement = m(4)
                    Dim xStmDt As XElement = m(7)
                    xDoc.Descendants().ToList()(4).SetValue(CDate(xCreTm.Value).ToString("yyyy-MM-ddTHH:mm:sszzz"))
                    xDoc.Descendants().ToList()(7).SetValue(CDate(xStmDt.Value).ToString("yyyy-MM-ddzzz"))
                    xDoc.Descendants().Where(Function(p) p.IsEmpty Or String.IsNullOrEmpty(p.Value)).Remove()
                    xDoc.Root.Attributes().Reverse()
                    xDoc.Save(fullpath, SaveOptions.None)
                    'MessageBox.Show(TempLocation)

                End If
                ZipContents(TempLocation, ImageUniqueName, New String() {"*.xml", "*.tiff"}, "", True)
                'ZipContents(TempLocation, ImageUniqueNames & "J" & det(0).OurBankID & ".zip", New String() {"*.J*", "*.M*"}, "", True)
                Return msgId
            End If


            Return Nothing
        End Function
        Private Shared Function UnpaidCheques(ByVal l As List(Of ETChequeDetails), ByVal BIC As String, ByVal ccy As String, ByVal SttlDate As String, ByVal Cntr As Int16) As String
            Dim RegX As New Regex("[^A-Za-z0-9]")
            Dim sDt As String = Modscan.WORKING_DATE.ToString("dd-MMM-yyyy")
            Dim STm As String = Now.ToString("HH:mm")
            Dim xDt As Date = CDate(sDt & " " & STm)
            Dim sDt1 As String = Modscan.WORKING_DATE.ToString("yyyy-MM-dd")
            Dim STm1 As String = STm.Replace(":", "")
            Dim msgId As String = "DRUNP" & BIC & Modscan.FDATE.ToString("ddMMyyyy") & STm1 + Cntr
            Dim stCurrCode As String = ""
            If ccy = "0" Then
                stCurrCode = "0"
            ElseIf ccy = "1" Then
                stCurrCode = "USD"
            ElseIf ccy = "2" Then
                stCurrCode = "GBP"
            ElseIf ccy = "3" Then
                stCurrCode = "EUR"
            ElseIf ccy = "4" Then
                stCurrCode = "JPY"
            ElseIf ccy = "5" Then
                stCurrCode = "KES"
            Else
                stCurrCode = "UGX"
            End If
            Dim doc As New res.Document()
            Dim grpHdr As New res.GroupHeader37()
            grpHdr.MsgId = msgId
            grpHdr.CreDtTm = xDt
            grpHdr.InstgAgt = New res.FinancialInstitution4 With {.FinInstnId = New res.FinancialInstitutionIdentification7 With {.BIC = l(0).OurBankBic.ToUpper}}
            'grpHdr.InstdAgt = New res.BranchAndFinancialInstitutionIdentification4 With {.FinInstnId = New res.FinancialInstitutionIdentification7 With {.BIC = l(0).BankBIC.ToUpper}}
            doc.FIToFIPmtStsRpt.GrpHdr = grpHdr
            Dim grpInf As New res.OriginalGroupInformation20()
            grpInf.GrpSts = res.TransactionGroupStatus3Code.RJCT
            grpInf.GrpStsSpecified = True
            grpInf.OrgnlMsgId = l(0).OrgnlMsgId
            grpInf.OrgnlMsgNmId = "pacs.005.001.02"
            doc.FIToFIPmtStsRpt.OrgnlGrpInfAndSts = grpInf
            For Each d As ETChequeDetails In l
                Try
                    Dim txnInf As New res.PaymentTransactionInformation26()
                    txnInf.StsId = xDt.ToString("yyyyMMdd") & "_" & d.RemittanceInfo 'd.ChequeNumber 'RegX.Replace(d.BankBIC & d.EndorsmentNo, String.Empty)
                    txnInf.OrgnlEndToEndId = d.OrgnlEndToEnd
                    txnInf.OrgnlTxId = d.OrgnTrxID
                    txnInf.TxSts = res.TransactionIndividualStatus3Code.RJCT
                    txnInf.StsRsnInf = New res.StatusReasonInformation8() With
                                      {.Orgtr = New res.PartyIdentification34() With
                                                {.Item = New res.Party6Choice() With
                                                       {.Item = New res.OrganisationIdentification4() With
                                                                {.Item = l(0).OurBankBic.ToUpper}}}, .Rsn = New res.StatusReason6Choice() With
                                                                                                                {.Item = d.RetCode.ToUpper}}

                    'MessageBox.Show("Currency : " + d.CurrencyCode + " Value : " + FormatNumber(d.Amount, 2, Microsoft.VisualBasic.TriState.False, Microsoft.VisualBasic.TriState.False, Microsoft.VisualBasic.TriState.False).Replace(",", ""))
                    txnInf.OrgnlTxRef.IntrBkSttlmAmt = New res.ActiveCurrencyAndAmount With {.Ccy = stCurrCode, .Value = Decimal.Round(CDec(d.Amount), 2)}
                    If d.ReqdColltnDt = "" Then

                    Else
                        Dim ReqdColltnDate As Date = Convert.ToDateTime(d.ReqdColltnDt)
                        txnInf.OrgnlTxRef.IntrBkSttlmDtt = ReqdColltnDate.ToString("yyyy-MM-dd")
                    End If
                    txnInf.OrgnlTxRef.SttlmInf = New res.SettlementInformation13() With {.SttlmMtd = res.SettlementMethod1Code.CLRG, .ClrSys = New res.ClearingSystemIdentification3Choice With {.Item = res.ClearingSystemIdentification.ACH}}
                    txnInf.OrgnlTxRef.PmtTpInf = New res.PaymentTypeInformation22() With {.SvcLvl = New res.ServiceLevel9Choice With {.Item = res.ServiceLevel3Code.SEPA}}
                    txnInf.OrgnlTxRef.Dbtr = New res.PartyIdentification33() With {.Id = New res.Party6Choice() With {.Item = New res.OrganisationIdentification4() With {.Item = l(0).OurBankBic}}, .Nm = d.DNm}
                    txnInf.OrgnlTxRef.DbtrAcct = New res.CashAccount17() With {.Id = New res.AccountIdentification4Choice() With {.Item = d.DbtrAcct}}
                    txnInf.OrgnlTxRef.DbtrAgt = New res.FinancialInstitution4 With {.FinInstnId = New res.FinancialInstitutionIdentification7 With {.BIC = d.OurBankBic.ToUpper}}
                    txnInf.OrgnlTxRef.CdtrAgt = New res.FinancialInstitution4 With {.FinInstnId = New res.FinancialInstitutionIdentification7 With {.BIC = d.BankBIC.ToUpper}}
                    txnInf.OrgnlTxRef.Cdtr = New res.PartyIdentification33() _
                    With {.Nm = d.CNm}

                    txnInf.OrgnlTxRef.CdtrAcct = New res.CashAccount17() With {.Id = New res.AccountIdentification4Choice() With {.Item = d.CdtrAcct}}
                    txnInf.OrgnlTxRef.PmtTpInf.LclInstrm = New res.LocalInstrument2Choice() With {.ItemElementName = res.ItemChoiceType2.Prtry}
                    txnInf.OrgnlTxRef.PmtTpInf.LclInstrm.Item = d.LclInstrm
                    txnInf.OrgnlTxRef.PmtTpInf.CtgyPurp = New res.CategoryPurpose1Choice() With {.ItemElementName = res.ItemChoiceType3.Cd}
                    txnInf.OrgnlTxRef.PmtTpInf.CtgyPurp.Item = d.CtgyPurp

                    doc.FIToFIPmtStsRpt.TxInfAndSts.Add(txnInf)
                Catch ex As Exception
                    MessageBox.Show("Error Registered, Check ErrorLog")
                    Modscan.ErrorLog(ex.Message, "Unpaid Cheques Generation")
                    Continue For
                End Try
            Next

            Try
                If Not Directory.Exists(strFileLocation) Then Directory.CreateDirectory(strFileLocation)
                If Directory.Exists(strFileLocation) Then
                    Dim fullpath As String = Path.Combine(strFileLocation & "\Temp\", msgId & ".chr")
                    Dim dest As String = Path.Combine(strFileLocation & "\Files\", msgId & ".chr")
                    Dim ex As New Exception()
                    If doc.SaveToFile(fullpath, ex) Then
                        Dim xDoc As XDocument = XDocument.Load(fullpath)
                        Dim k As List(Of XAttribute) = xDoc.Root.Attributes().ToList()
                        Dim xsd As XAttribute = k(1)
                        If xDoc.Root.HasAttributes Then xDoc.Root.Attribute(xsd.Name).Remove()
                        Dim xCreTm As XElement = xDoc.Descendants().ToList()(4)
                        xDoc.Descendants().ToList()(4).SetValue(CDate(xCreTm.Value).ToString("yyyy-MM-ddTHH:mm:sszzz"))
                        xDoc.Descendants().Where(Function(p) p.IsEmpty Or String.IsNullOrEmpty(p.Value)).Remove()
                        xDoc.Descendants().Where(Function(p) p.Name.LocalName = "MndtRltdInf").Remove()
                        xDoc.Root.Attributes().Reverse()
                        xDoc.Save(fullpath, SaveOptions.None)
                        'MessageBox.Show("Sign")

                        Dim fileName As String = fullpath
                        'MessageBox.Show("Step 1 Unpaid ")
                        Dim DestZippedFolderLoc As String = ConfigurationManager.AppSettings("OutgoingFiles") & "\Files"
                        If Directory.Exists(DestZippedFolderLoc) = False Then
                            Directory.CreateDirectory(DestZippedFolderLoc)
                        End If
                        'MessageBox.Show("Step 2 Unpaid ")
                        Dim DestSignedFolderLoc As String = ConfigurationManager.AppSettings("OutgoingFiles") & "\SignedFiles"
                        If Directory.Exists(DestSignedFolderLoc) = False Then
                            Directory.CreateDirectory(DestSignedFolderLoc)
                        End If
                        'MessageBox.Show("Step 3 Unpaid ")
                        Dim Destinationfile As String = DestSignedFolderLoc & "\" & Path.GetFileName(fileName)
                        If File.Exists(Destinationfile) = False Then
                            File.Copy(fileName, Destinationfile, True)
                        Else
                            File.Delete(Destinationfile)
                            File.Copy(fileName, Destinationfile, True)
                        End If


                        'MessageBox.Show(fileName)
                        Dim MessOut As String = ""
                        Dim CertPass As String = Encoding.ASCII.GetString(Convert.FromBase64String(ConfigurationManager.AppSettings("keypass")))
                        Try
                            'MessageBox.Show("Step 5 Unpaid ")
                            Try
                                Sign = Convert.ToBoolean(ConfigurationManager.AppSettings("Sign"))
                                If Sign Then
                                    MessOut = SignFiles_PKCS(fileName.Trim(), DestSignedFolderLoc.Trim(), CertName, CertPass.Trim(), "chr")
                                End If
                            Catch exi As Exception
                                MessageBox.Show("error Step 5 Unpaid " & ex.Message)
                            End Try

                            'MessageBox.Show("Step 6 ")
                            Dim ArchivePath As String
                            If MessOut = "success" Then
                                'MessageBox.Show("Step 7 ")
                                ArchivePath = ConfigurationManager.AppSettings("Archive")
                                If Directory.Exists(ArchivePath) = False Then
                                    Directory.CreateDirectory(ArchivePath)
                                End If
                                'MessageBox.Show("Step 8 ")
                                Clear_Files_Arc(DestSignedFolderLoc.Trim(), ArchivePath.Trim(), DestSignedFolderLoc.Trim(), "Out")
                                'MessageBox.Show("Step 9 ")
                            Else
                                If Sign Then
                                    MessageBox.Show("Failed Signing Unpaid. " & Path.GetFileName(fileName) & " : " & MessOut)
                                    Modscan.ErrorLog(Path.GetFileName(fileName) & " : " & MessOut, "- Signing ")
                                End If
                            End If
                            'MessageBox.Show("Step 10 ")
                        Catch exp As Exception
                            MessageBox.Show("Failed Signing Unpaid. " & Path.GetFileName(fileName) & " : " & MessOut)
                            Modscan.ErrorLog(Path.GetFileName(fileName) & " : " & MessOut & " : " & exp.Message, "- Signing ")
                        End Try
                        Return msgId
                    End If
                End If
            Catch ex As Exception
                MessageBox.Show("Error Registered, Check ErrorLog")
                Modscan.ErrorLog(ex.Message, "Unpaid Cheques Generation - ")
            End Try
            Return Nothing
        End Function

        '    Private Shared Function UnpaidCheques(ByVal l As List(Of ETChequeDetails), ByVal BIC As String, ByVal ccy As String, ByVal SttlDate As String, ByVal Cntr As Int16) As String
        '        Dim sDt As String = Modscan.WORKING_DATE.ToString("dd-MMM-yyyy")
        '        Dim STm As String = Now.ToString("HH:mm")
        '        Dim xDt As Date = CDate(sDt & " " & STm)
        '        Dim sDt1 As String = Modscan.WORKING_DATE.ToString("yyyy-MM-dd")
        '        Dim STm1 As String = STm.Replace(":", "")
        '        Dim msgId As String = "DRREJ" & BIC & Modscan.FDATE.ToString("ddMMyyyy") & STm1
        '        Dim stCurrCode As String = ""
        '        If ccy = "0" Then
        '            stCurrCode = "ETB"
        '        ElseIf ccy = "1" Then
        '            stCurrCode = "USD"
        '        ElseIf ccy = "2" Then
        '            stCurrCode = "GBP"
        '        ElseIf ccy = "3" Then
        '            stCurrCode = "EUR"
        '        ElseIf ccy = "4" Then
        '            stCurrCode = "JPY"
        '        ElseIf ccy = "5" Then
        '            stCurrCode = "KES"
        '        Else
        '            stCurrCode = "UGX"
        '        End If
        '        Dim final = Path.Combine(strFileLocation & "\Files\", msgId & ".chr")
        '        Dim temp = Path.Combine(strFileLocation & "\Temp\", msgId & ".chr")
        '        Dim currencyCode As res.ActiveCurrencyCode


        '        [Enum].TryParse(ccy.ToString, True, currencyCode)
        '        Dim doc = New res.Document With {
        '    .FIToFIPmtStsRpt = New res.FIToFIPaymentStatusReportV03 With {
        '        .GrpHdr = New res.GroupHeader37 With {
        '            .CreDtTm = xDt,
        '            .InstgAgt = New res.FinancialInstitution4 With {
        '                .FinInstnId = New res.FinancialInstitutionIdentification7 With {
        '                    .BIC = l(0).OurBankBic.ToUpper
        '                }
        '            },
        '            .MsgId = msgId
        '        },
        '        .OrgnlGrpInfAndSts = New res.OriginalGroupInformation20 With {
        '            .GrpSts = res.TransactionGroupStatus3Code.RJCT,
        '            .GrpStsSpecified = True,
        '            .OrgnlMsgId = l(0).OrgnlMsgId,
        '            .OrgnlMsgNmId = "pacs.005.001.02"
        '        },
        '        .TxInfAndSts = (From p In l Select New res.PaymentTransactionInformation26 With {
        '            .StsId = p.trxID,
        '            .OrgnlEndToEndId = p.OrgnlEndToEnd,
        '            .OrgnlInstrId = p.OrgnlInstrID,
        '            .OrgnlTxId = p.OrgnlTxId,
        '            .StsRsnInf = New res.StatusReasonInformation8 With {
        '                .Orgtr = New res.PartyIdentification34 With {
        '                    .Item = New res.Party6Choice With {
        '                        .Item = New res.OrganisationIdentification4 With {
        '                            .Item = p.BankBIC
        '                        }
        '                    }
        '                },
        '                .Rsn = New res.StatusReason6Choice With {
        '                    .Item = p.RetCode
        '                }
        '            },
        '            .OrgnlTxRef = New res.OriginalTransactionReference13 With {
        '                .IntrBkSttlmAmt = New res.ActiveCurrencyAndAmount With {
        '                    .Ccy = currencyCode,
        '                    .Value = FormatNumber(p.Amount, 2, Microsoft.VisualBasic.TriState.False, Microsoft.VisualBasic.TriState.False, Microsoft.VisualBasic.TriState.False).Replace(",", "")
        '                },
        '                .IntrBkSttlmDtSpecified = True,
        '                .IntrBkSttlmDt = p.IntrBkSttlmDt,
        '                .SttlmInf = New res.SettlementInformation13 With {
        '                    .SttlmMtd = res.SettlementMethod1Code.CLRG,
        '                    .ClrSys = New res.ClearingSystemIdentification3Choice With {
        '                        .Item = res.ClearingSystemIdentification.ACH
        '                    }
        '                },
        '                .PmtTpInf = New res.PaymentTypeInformation22 With {
        '                    .SvcLvl = New res.ServiceLevel9Choice With {
        '                        .Item = res.ServiceLevel3Code.SEPA
        '                    },
        '                    .LclInstrm = New res.LocalInstrument2Choice With {
        '                        .ItemElementName = res.ItemChoiceType2.Cd,
        '                        .Item = "CORE"
        '                    }
        '                },
        '                .Dbtr = New res.PartyIdentification33 With {
        '                    .Id = New res.Party6Choice With {
        '                        .Item = New res.OrganisationIdentification4 With {
        '                            .Item = p.BankBIC
        '                        }
        '                    },
        '                    .Nm = p.DNm
        '                },
        '                .DbtrAcct = New res.CashAccount17 With {
        '                    .Id = New res.AccountIdentification4Choice With {
        '                        .Item = p.DbtrAcct
        '                    }
        '                },
        '                .DbtrAgt = New res.FinancialInstitution4 With {
        '                    .FinInstnId = New res.FinancialInstitutionIdentification7 With {
        '                        .BIC = p.OurBankBic.ToUpper
        '                    }
        '                },
        '                .CdtrAgt = New res.FinancialInstitution4 With {
        '                    .FinInstnId = New res.FinancialInstitutionIdentification7 With {
        '                        .BIC = p.BankBIC.ToUpper
        '                    }
        '                },
        '                .CdtrAcct = New res.CashAccount17 With {
        '                    .Id = New res.AccountIdentification4Choice With {
        '                        .Item = p.CdtrAcct
        '                    }
        '                },
        '                .Cdtr = New res.PartyIdentification33 With {
        '                    .Nm = p.CNm
        '                }
        '            }
        '        }).ToList()
        '    }
        '}
        '        Dim ex As Exception

        '        If doc.SaveToFile(temp, ex) Then
        '            Dim xDoc As XDocument = XDocument.Load(temp)

        '            If xDoc.Root IsNot Nothing Then
        '                Dim k As List(Of XAttribute) = xDoc.Root.Attributes().ToList()
        '                Dim xsd As XAttribute = k(1)

        '                If xDoc.Root.HasAttributes Then
        '                    Dim xAttribute = xDoc.Root.Attribute(xsd.Name)
        '                    If xAttribute IsNot Nothing Then xAttribute.Remove()
        '                End If
        '            End If

        '            Dim m As List(Of XElement) = xDoc.Descendants().ToList()
        '            Dim xCreTm As XElement = m(4)
        '            xDoc.Descendants().ToList()(4).SetValue(Convert.ToDateTime(xCreTm.Value).ToString("yyyy-MM-ddTHH:mm:sszzz"))
        '            xDoc.Descendants().Where(Function(p) p.IsEmpty Or String.IsNullOrEmpty(p.Value)).Remove()
        '            If xDoc.Root IsNot Nothing Then xDoc.Root.Attributes().Reverse()
        '            xDoc.Save(temp, SaveOptions.None)
        '        End If

        '        Dim fileName As String = temp
        '        'MessageBox.Show("Step 1 Unpaid ")
        '        Dim DestZippedFolderLoc As String = ConfigurationManager.AppSettings("OutgoingFiles") & "\Files"
        '        If Directory.Exists(DestZippedFolderLoc) = False Then
        '            Directory.CreateDirectory(DestZippedFolderLoc)
        '        End If
        '        'MessageBox.Show("Step 2 Unpaid ")
        '        Dim DestSignedFolderLoc As String = ConfigurationManager.AppSettings("OutgoingFiles") & "\SignedFiles"
        '        If Directory.Exists(DestSignedFolderLoc) = False Then
        '            Directory.CreateDirectory(DestSignedFolderLoc)
        '        End If


        '        Dim Destinationfile As String = String.Empty
        '        If Sign Then
        '            Destinationfile = DestZippedFolderLoc & "\" & Path.GetFileName(fileName)
        '        Else
        '            Destinationfile = DestSignedFolderLoc & "\" & Path.GetFileName(fileName)
        '        End If

        '        'MessageBox.Show("Step 3 Unpaid ")
        '        If File.Exists(Destinationfile) = False Then
        '            File.Copy(fileName, Destinationfile, True)
        '        Else
        '            File.Delete(Destinationfile)
        '            File.Copy(fileName, Destinationfile, True)
        '        End If


        '        'MessageBox.Show(fileName)
        '        Dim MessOut As String = ""
        '        Dim CertPass As String = Encoding.ASCII.GetString(Convert.FromBase64String(ConfigurationManager.AppSettings("keypass")))
        '        Try
        '            'MessageBox.Show("Step 5 Unpaid ")
        '            Try
        '                Sign = Convert.ToBoolean(ConfigurationManager.AppSettings("Sign"))
        '                If Sign Then
        '                    MessOut = SignFiles_PKCS(fileName.Trim(), DestSignedFolderLoc.Trim(), CertName, CertPass.Trim(), "Q")
        '                End If
        '            Catch exi As Exception
        '                MessageBox.Show("error Step 5 Unpaid " & ex.Message)
        '            End Try

        '            'MessageBox.Show("Step 6 ")
        '            Dim ArchivePath As String
        '            If MessOut = "success" Then
        '                'MessageBox.Show("Step 7 ")
        '                ArchivePath = ConfigurationManager.AppSettings("Archive")
        '                If Directory.Exists(ArchivePath) = False Then
        '                    Directory.CreateDirectory(ArchivePath)
        '                End If
        '                'MessageBox.Show("Step 8 ")
        '                Clear_Files_Arc(DestSignedFolderLoc.Trim(), ArchivePath.Trim(), DestSignedFolderLoc.Trim(), "Out")
        '                'MessageBox.Show("Step 9 ")
        '            Else
        '                If Sign Then
        '                    MessageBox.Show("Failed Signing Unpaid. " & Path.GetFileName(fileName) & " : " & MessOut)
        '                    Modscan.ErrorLog(Path.GetFileName(fileName) & " : " & MessOut, "- Signing ")
        '                End If
        '            End If
        '            'MessageBox.Show("Step 10 ")
        '        Catch exp As Exception
        '            MessageBox.Show("Failed Signing Unpaid. " & Path.GetFileName(fileName) & " : " & MessOut)
        '            Modscan.ErrorLog(Path.GetFileName(fileName) & " : " & MessOut & " : " & exp.Message, "- Signing ")
        '        End Try
        '        Return msgId
        '    End Function

        Private Sub ReadFile(ByRef sFile As String, ByVal sTemp As String)
            Dim sline As New List(Of String)(IO.File.ReadAllLines(sFile))
            Dim p As Integer = sline.LongCount
            sline.RemoveAt(p - 1)
            IO.File.WriteAllLines(sFile, sline.ToArray())
        End Sub

#End Region

#Region "Utility"
        Private Shared Sub WriteFile(ByVal Content As String, ByVal msgId As String, ByVal FileName As String, Optional ByVal Log As Boolean = False)
            If Directory.Exists(TempLocation) Then
                Dim fullpath As String = Path.Combine(TempLocation, FileName)
                If File.Exists(fullpath) Then File.Delete(fullpath)
                Using fs As New FileStream(fullpath, FileMode.CreateNew)
                    Using sw As New StreamWriter(fs)
                        sw.Write(Content)
                    End Using
                End Using
                If Sign And Log Then SignFile(fullpath, FileName)
                Dim dest As String = Path.Combine(strFileLocation, FileName)
                If Not File.Exists(dest) Then
                    File.Move(fullpath, dest)
                Else
                    If MessageBox.Show("The File [" & dest & "] Has Already Been Created. Overwrite?", Modscan.MsgBoxTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                        Try
                            File.Delete(dest)
                            File.Move(fullpath, dest)
                        Catch ex As Exception
                            MessageBox.Show("The File [" & dest & "] Could Not Be Replaced", Modscan.MsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                            Exit Sub
                        End Try
                    End If
                End If
                If Log Then

                End If
            End If
        End Sub

        Private Shared Sub ZipContents(ByVal OutFile As String, ByVal msgId As String, ByVal Filter As String(), Optional ByVal FilesLocation As String = "", Optional ByVal Log As Boolean = False)
            Try
                'MessageBox.Show("Step 1 - OutFile: " & OutFile)
                Dim sPath As String = IIf(FilesLocation = "", OutFile, FilesLocation)

                'MessageBox.Show("Step 1 - sPath: " & sPath)
                Dim di As New DirectoryInfo(sPath)
                Dim l As New List(Of String)
                For Each f As String In Filter
                    Dim fi As FileInfo() = di.GetFiles(f)
                    For Each inf As FileInfo In fi
                        l.Add(inf.FullName)
                    Next
                Next
                sPath = Path.Combine(OutFile, msgId & ".xml")
                'MessageBox.Show("Step 1 - sPath: " & sPath)
                Dim fileName As String = String.Empty
                Dim Destinationfile As String = String.Empty
                Dim DestSignedFolderLoc As String = ConfigurationManager.AppSettings("OutgoingFiles") & "\SignedFiles"
                If Directory.Exists(DestSignedFolderLoc) = False Then
                    Directory.CreateDirectory(DestSignedFolderLoc)
                End If

                Dim DestZippedFolderLoc As String = ConfigurationManager.AppSettings("OutgoingFiles") & "\Files"
                If Directory.Exists(DestZippedFolderLoc) = False Then
                    Directory.CreateDirectory(DestZippedFolderLoc)
                End If


                If Not Sign Then
                    fileName = Path.Combine(OutFile, msgId & ".chk")
                    Destinationfile = DestSignedFolderLoc & "\" & Path.GetFileName(fileName)
                Else
                    fileName = Path.Combine(OutFile, msgId & ".zip")
                    Destinationfile = DestZippedFolderLoc & "\" & Path.GetFileName(fileName)
                End If



                'MessageBox.Show("Step 1")

                'MessageBox.Show("Step 2")

                'MessageBox.Show("Step 3")


                'MessageBox.Show("Step 1 - fileName: " & fileName)
                Dim fZip As New ZipFile(fileName)
                For Each itm As String In l
                    fZip.AddFile(itm, "")
                Next
                fZip.Save()
                Application.DoEvents()


                If File.Exists(Destinationfile) = False Then
                    File.Copy(fileName, Destinationfile, True)
                Else
                    File.Delete(Destinationfile)
                    File.Copy(fileName, Destinationfile, True)
                End If


                'MessageBox.Show(fileName)
                Dim MessOut As String = ""
                Dim CertPass As String = Encoding.ASCII.GetString(Convert.FromBase64String(ConfigurationManager.AppSettings("keypass")))
                Try
                    'MessageBox.Show("Step 5 ")
                    Try
                        Sign = Convert.ToBoolean(ConfigurationManager.AppSettings("Sign"))
                        If Sign Then
                            MessOut = SignFiles_PKCS(fileName.Trim(), DestSignedFolderLoc.Trim(), CertName, CertPass.Trim(), "chk")
                        End If
                    Catch ex As Exception
                        MessageBox.Show("error Step 5 " & ex.Message)
                    End Try

                    'MessageBox.Show("Step 6 ")
                    Dim ArchivePath As String
                    If MessOut = "success" Then
                        'MessageBox.Show("Step 7 ")
                        ArchivePath = ConfigurationManager.AppSettings("Archive")
                        If Directory.Exists(ArchivePath) = False Then
                            Directory.CreateDirectory(ArchivePath)
                        End If
                        'MessageBox.Show("Step 8 ")
                        Clear_Files_Arc(DestSignedFolderLoc.Trim(), ArchivePath.Trim(), DestSignedFolderLoc.Trim(), "Out")
                        'MessageBox.Show("Step 9 ")
                    Else
                        If Sign Then
                            MessageBox.Show("Failed Signing. " & Path.GetFileName(fileName) & " : " & MessOut)
                            Modscan.ErrorLog(Path.GetFileName(fileName) & " : " & MessOut, "- Signing ")
                        End If
                    End If
                    'MessageBox.Show("Step 10 ")
                Catch ex As Exception
                    MessageBox.Show("Failed Signing. " & Path.GetFileName(fileName) & " : " & MessOut)
                    Modscan.ErrorLog(Path.GetFileName(fileName) & " : " & MessOut & " : " & ex.Message, "- Signing ")
                End Try



                'MessageBox.Show("NewFilename: " + NewFilename)

                'sPath = Path.Combine(OutFile, msgId & ".chk.signed.CMS")
                'MessageBox.Show("sPath: " + sPath)
                'If Sign Then SignFile(NewFilename, sPath)

                'MessageBox.Show("imesign about to move: ")
                'Dim dest As String = Path.Combine(StrDestinationFilePath, msgId & ".chk")
                'MessageBox.Show("dest: " + dest)
                'If File.Exists(dest) Then File.Delete(dest)
                'File.Move(sPath, dest)



                If Log Then

                End If

                Filter = New String() {"*.chk", "*.xml", "*.tiff"}
                Dim SourceTempFolderLoc As String = ConfigurationManager.AppSettings("OutgoingFiles") & "\Temp"
                'MessageBox.Show("Step 11 ")
                di = New DirectoryInfo(SourceTempFolderLoc)
                l = New List(Of String)()
                For Each f As String In Filter
                    Dim fi As FileInfo() = di.GetFiles(f)
                    For Each inf As FileInfo In fi
                        l.Add(inf.FullName)
                    Next
                Next

                'MessageBox.Show("Step 12 ")
                For Each itm As String In l
                    File.Delete(itm)
                Next

                SourceTempFolderLoc = ConfigurationManager.AppSettings("OutgoingFiles") & "\Files"
                'MessageBox.Show("Step 11 ")
                di = New DirectoryInfo(SourceTempFolderLoc)
                l = New List(Of String)()
                For Each f As String In Filter
                    Dim fi As FileInfo() = di.GetFiles(f)
                    For Each inf As FileInfo In fi
                        l.Add(inf.FullName)
                    Next
                Next

                'MessageBox.Show("Step 12 ")
                If Sign Then
                    For Each itm As String In l
                        File.Delete(itm)
                    Next
                End If
                'MessageBox.Show("Step 13 ")
                'MessageBox.Show("Deleted all files: ")
            Catch ex As Exception
                MessageBox.Show("Error Registered, Check ErrorLog")
                Modscan.ErrorLog(ex.Message, "- Zipping")
            End Try
        End Sub


        Private Shared Function SignFiles_PKCS(ByVal Sourcepath As String, ByVal DestPath As String, ByVal cert As String, ByVal tokenpass As String, ByVal ext As String) As String
            Dim Mes As String = ""
            Try
                Dim filepath As String = Sourcepath
                Dim strBatchPath As String = ""
                Dim strDSkeyFile As String = ""
                Dim strJavaExeInstallation As String = ""
                Sourcepath = ConfigurationManager.AppSettings("OutgoingFiles") & "\Files"
                DestPath = ConfigurationManager.AppSettings("OutgoingFiles") & "\SignedFiles"
                Dim p As BRCS = New BRCS()
                'MessageBox.Show("Step 5.1")

                'MessageBox.Show("Step 5.2 " & TkBased.ToString)

                Try
                    If TkBased = False Then
                        tokenpass = BRRSACryptography.CryptographyHelper.Encrypt(tokenpass)
                        cert = BRRSACryptography.CryptographyHelper.Encrypt(cert)
                        strBatchPath = BRRSACryptography.CryptographyHelper.Encrypt(ConfigurationManager.AppSettings("strBatchPath")) '"E:\Source\Mwanga\Cert\SignatureClient\"
                        strDSkeyFile = BRRSACryptography.CryptographyHelper.Encrypt(ConfigurationManager.AppSettings("strDSkeyFile")) '"E:\Source\Mwanga\Cert\MWCBTZT0.jks"
                        strJavaExeInstallation = BRRSACryptography.CryptographyHelper.Encrypt(ConfigurationManager.AppSettings("strJavaExeInstallation")) '"C:\Program Files\Java\jre1.8.0_181\bin\java.exe"
                    End If
                Catch ex As Exception
                    strBatchPath = BRRSACryptography.CryptographyHelper.Encrypt(ConfigurationManager.AppSettings("strBatchPath")) '"E:\Source\Mwanga\Cert\SignatureClient\"
                    strDSkeyFile = BRRSACryptography.CryptographyHelper.Encrypt(ConfigurationManager.AppSettings("strDSkeyFile")) '"E:\Source\Mwanga\Cert\MWCBTZT0.jks"
                    strJavaExeInstallation = BRRSACryptography.CryptographyHelper.Encrypt(ConfigurationManager.AppSettings("strJavaExeInstallation")) '"C:\Program Files\Java\jre1.8.0_181\bin\java.exe"
                End Try


                'MessageBox.Show("Step 5.3")
                Try
                    p.BRCDS(BRRSACryptography.CryptographyHelper.Encrypt(Sourcepath), BRRSACryptography.CryptographyHelper.Encrypt(DestPath), BRRSACryptography.CryptographyHelper.Encrypt("h / KNJ1uE5CmUcQb4xbsfoW9ZPzk ="), 71, cert, tokenpass, Mes, "ET", strBatchPath, strDSkeyFile, strJavaExeInstallation, TkBased)
                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                End Try
                'MessageBox.Show("Step 5.4" & " : " & Mes)

                'MessageBox.Show("filepath " + filepath)
                'File.Delete(filepath)
                Dim fileName = Sourcepath & "\" & Path.GetFileName(filepath)
                'MessageBox.Show("fileName " + fileName)
                Dim destinationfile As String = DestPath & "\" & Path.GetFileName(filepath).Substring(0, Path.GetFileName(filepath).IndexOf(".")) & "." & ext
                'MessageBox.Show("destinationfile " + destinationfile)

                If File.Exists(destinationfile) = False Then
                    File.Copy(fileName, destinationfile, True)
                    File.Delete(fileName)
                Else
                    File.Delete(destinationfile)
                    File.Copy(fileName, destinationfile, True)
                    File.Delete(fileName)
                End If
            Catch ex As Exception
                MessageBox.Show("SignFiles_PKCS Chapa -" & ex.Message)
            End Try
            Return Mes
        End Function

        Private Shared Sub Clear_Files_Arc(ByVal Sourcepath As String, ByVal destpath As String, ByVal temppath As String, ByVal Direction As String)
            Dim dir As String = ""


            If Direction = "Out" Then
                dir = destpath & "\Out\" & DateTime.Now.ToString("yyMMdd")
            Else
                dir = destpath & "\In\" & DateTime.Now.ToString("yyMMdd")
            End If

            If Directory.Exists(dir) = False Then
                Directory.CreateDirectory(dir)
            End If

            Dim fileEntries As String() = Directory.GetFiles(Sourcepath)
            Dim filetoclear As String() = Directory.GetFiles(temppath)

            For Each fileName As String In fileEntries
                Dim destinationfile As String = dir & "\" & Path.GetFileName(fileName)

                If File.Exists(destinationfile) = False Then
                    File.Copy(fileName, destinationfile, True)
                End If
            Next
            Dim TempFilesPath As String = ConfigurationManager.AppSettings("OutgoingFiles") & "\Temp"
            Dim FilesPath As String = ConfigurationManager.AppSettings("OutgoingFiles") & "\Files"
            filetoclear = Directory.GetFiles(TempFilesPath)
            For Each fileName As String In filetoclear
                If File.Exists(fileName) Then
                    File.Delete(fileName)
                End If
            Next

            filetoclear = Directory.GetFiles(FilesPath)
            For Each fileName As String In filetoclear
                If File.Exists(fileName) Then
                    File.Delete(fileName)
                End If
            Next
        End Sub

        Private Shared Sub SignFile(ByVal sFile As String, ByVal dFile As String)
            Try
                'Dim SignF As ACH_Files.MonSig
                'SignF = New ACH_Files.MonSig(CertName)
                'MessageBox.Show("CertName: " + CertName)
                'Dim b As Byte() = SignF.SignFile(File.ReadAllBytes(sFile))
                'File.WriteAllBytes(dFile, b)

                'Dim Mes As String = ""
                'Dim cert As String = BRRSACryptography.CryptographyHelper.Encrypt(CertName)
                'Dim tokenpass As String = BRRSACryptography.CryptographyHelper.Encrypt("passpass")
                'Dim p As BRCS = New BRCS()
                'p.BRCDS(BRRSACryptography.CryptographyHelper.Encrypt(sFile), BRRSACryptography.CryptographyHelper.Encrypt(dFile), BRRSACryptography.CryptographyHelper.Encrypt("h / KNJ1uE5CmUcQb4xbsfoW9ZPzk ="), 71, cert, tokenpass, Mes, CountryID)



                sFile = sFile.Replace("\", "/")
                If Path.GetExtension(Modscan.strBatchPath) = ".bat" Then
                    Modscan.strBatchPath = Path.GetDirectoryName(Modscan.strBatchPath)
                    Modscan.strBatchPath = Modscan.strBatchPath & "\"
                End If
                Modscan.strBatchPath = Modscan.strBatchPath & "Execute.bat"
                Dim Dskkey As String = Modscan.strDSkeyFile.Substring(Modscan.strDSkeyFile.ToString().LastIndexOf("\") + 1, Modscan.strDSkeyFile.ToString().LastIndexOf(".") - Modscan.strDSkeyFile.ToString().LastIndexOf("\") - 1)
                Dim strCmd As String = """" & Modscan.strJavaExeInstallation.Trim() & """ -cp .;com.springsource.org.bouncycastle.jce-1.46.0.jar;com.springsource.org.bouncycastle.mail-1.46.0.jar SignatureClient DSkeyFile=" & Modscan.strDSkeyFile.Trim().Replace("\", "/") &
                        " fileName=" & sFile & " function=sign mode=CMS keyAlias=" & Dskkey & " certificateAlias=" & Dskkey & " keyPass=" & Modscan.keyPass & ""

                Dim myFileStream As FileStream = Nothing
                Dim myEJContentStreamWriter As StreamWriter = Nothing
                Try
                    myEJContentStreamWriter = New StreamWriter(Modscan.strBatchPath, True)
                    myEJContentStreamWriter.WriteLine(strCmd)
                Finally
                    If Not (myEJContentStreamWriter Is Nothing) Then myEJContentStreamWriter.Close()
                End Try
                ExecuteCommand(Modscan.strBatchPath)
            Catch ex As Exception
                MessageBox.Show(ex.Message)
                MessageBox.Show("Error registerd, check error log")
                Modscan.ErrorLog("output>>" & (If([String].IsNullOrEmpty(ex.Message), "(none)", ex.Message)), "- SignFile-ET Files")
            End Try
        End Sub

        Private Shared Sub ExecuteCommand(ByVal strBatchPath As String)
            Try
                Dim ExitCode As Integer
                Dim ProcessInfo As ProcessStartInfo
                Dim process__1 As Process = Nothing
                Dim output As String = ""
                Dim [error] As String = ""
                Dim strWorkingDir As String = Path.GetDirectoryName(strBatchPath)
                Try
                    ProcessInfo = New ProcessStartInfo(strBatchPath)
                    ProcessInfo.CreateNoWindow = True
                    ProcessInfo.UseShellExecute = False
                    ProcessInfo.WorkingDirectory = strWorkingDir

                    ProcessInfo.RedirectStandardError = True
                    ProcessInfo.RedirectStandardOutput = True

                    process__1 = Process.Start(ProcessInfo)
                    process__1.WaitForExit()

                    output = process__1.StandardOutput.ReadToEnd()
                    [error] = process__1.StandardError.ReadToEnd()
                    If [error] <> "" Then
                        Modscan.ErrorLog("output>>" & (If([String].IsNullOrEmpty(output), "(none)", output)), "ExecuteCommand-ET Files")
                        Modscan.ErrorLog("error>>" & (If([String].IsNullOrEmpty([error]), "(none)", [error])), "ExecuteCommand-ET Files")
                    End If
                Catch ex As Exception
                    MessageBox.Show("E2: " & ex.Message)
                    Modscan.ErrorLog("output>>" & (If([String].IsNullOrEmpty(output), "(none)", output)), "ExecuteCommand-ET Files")
                    Modscan.ErrorLog("error>>" & (If([String].IsNullOrEmpty([error]), "(none)", [error])), "ExecuteCommand-ET Files")
                End Try
                ExitCode = process__1.ExitCode
                process__1.Close()
                Kill(Modscan.strBatchPath)
                strBatchPath = ""
            Catch ex As Exception
                MessageBox.Show("E3: " & ex.Message)
                MessageBox.Show("Error registerd, check error log")
                Modscan.ErrorLog(ex.Message, "- Out ExecuteCommand-ET Files")
            End Try
        End Sub

        Private Sub StripSignature(ByRef sFile As String, ByVal sTemp As String)
            'Dim fBytes() As Byte = File.ReadAllBytes(sFile)
            'Dim m As New Montran(CertName)
            'fBytes = m.ReadSigned(fBytes)
            'If Not fBytes Is Nothing Then
            '    File.WriteAllBytes(sTemp, fBytes)
            '    sFile = sTemp
            'End If
        End Sub

        Private Function IsClearingDate(ByRef xDt As Date) As Date
            If xDt.DayOfWeek = DayOfWeek.Saturday Then
                xDt = xDt.AddDays(2)
                IsClearingDate(xDt)
            End If
            If xDt.DayOfWeek = DayOfWeek.Sunday Then
                xDt = xDt.AddDays(1)
                IsClearingDate(xDt)
            End If
            'If IsPublicHoliday(xDt) Then
            '    xDt = xDt.AddDays(1)
            '    IsClearingDate(xDt)
            'End If
            Return xDt
        End Function

        Private Shared Sub SplitString(ByRef sOriginal As String, ByVal Chunks As Integer, Optional ByVal sSlashes As String = "")
            Dim split As String = ""
            Dim i As Integer = 0
            While i < sOriginal.Length
                If sOriginal.Length - i > Chunks Then
                    split += (sSlashes & sOriginal.Substring(i, Chunks)) + Environment.NewLine
                Else
                    split += sSlashes & sOriginal.Substring(i, sOriginal.Length - i)
                End If
                i += Chunks
            End While
            sOriginal = split
        End Sub

#End Region
    End Class
#End Region
#Region "Inward File Importation Class"
    Public Class ETInwards

        Public Sub New()

        End Sub
        'mm = FileType, cc = CurrCode, = xyz = Session, x = CertName, y = Token/Keystore pass/Cert Password, (TPss and Tusr are both decoys, both are not in use) 
        Public Shared Sub ImportETH(ByVal x As String, ByVal y As String, ByVal sFiles As List(Of String), ByRef lbl As Label, ByRef prgAll As ProgressBar, ByRef prg As ProgressBar, Optional ByVal fType As FileType = FileType.Cheques, Optional ByVal xyz As String = "", Optional ByVal TPss As String = "", Optional ByVal TUsr As String = "")
            'MessageBox.Show("3")
            ReadFiles(x, y, sFiles, lbl, prgAll, prg, fType, xyz, TPss, TUsr)
        End Sub
        '#Region "Enums"
        '        Public Enum FileType
        '            Cheques = 0
        '            Efts = 1
        '            DD = 2
        '            RTGS = 3
        '            ChequeRejects = 4
        '            ATSStatements = 5
        '        End Enum

        '        Public Enum ChequeType
        '            SIS = 0
        '            XML = 1
        '        End Enum
        '#End Region

#Region "Variables"
        Private Shared Sign As Boolean = False
        Private Shared CertName As String = ""
        Private Shared TempLocation As String = ""
        Shared StrDestinationFilePath As String = ""
        Private Shared sOurBIC As String = ""
        Private Shared strFileLocation As String = ""
        Private Shared strCurFile As String = ""
        Private Shared sArchivePath As String = ""
        Private Shared sCorruptPath As String = ""
        'Private Shared rtgsProcessor As ETRTGSProcessing
#End Region

#Region "Constructors"
        Private Sub New(ByVal Location As String)
            strFileLocation = Location
            sArchivePath = Path.Combine(strFileLocation, "ARCHIVE\" & Now.ToString("yyyyMMdd"))
            sCorruptPath = Path.Combine(sArchivePath, "CORRUPT")
            If Not Directory.Exists(TempLocation) Then Directory.CreateDirectory(TempLocation)
        End Sub


#End Region

#Region "Methods"

        Private Shared Sub ReadFiles(ByVal x As String, ByVal y As String, ByVal sFiles As List(Of String), ByRef lbl As Label, ByRef prgAll As ProgressBar, ByRef prg As ProgressBar, Optional ByVal fType As FileType = FileType.Cheques, Optional ByVal xyz As String = "", Optional ByVal TPss As String = "", Optional ByVal TUsr As String = "")
            Try

                'MessageBox.Show("4")
                prgAll.Step = 100 / sFiles.Count
                prg.Step = 50
                Dim SourcePath As String = ""
                Dim DestPath As String = ""
                Dim arcs As String = ""
                Sign = Convert.ToBoolean(ConfigurationManager.AppSettings("Sign"))

                For Each sFile As String In sFiles
                    Try
                        SourcePath = sFile
                        DestPath = ""
                        Dim bArchive As Boolean = False
                        'MessageBox.Show(sFile)
                        Dim origFileName As String = ""
                        Dim FirstIndexPstofBackSlash As Int16 = sFile.LastIndexOf("\")
                        Dim FristIndexPstofDot As Int16 = sFile.IndexOf(".")
                        Dim lenOfTheFileName As Int16 = FristIndexPstofDot - FirstIndexPstofBackSlash
                        origFileName = sFile.Substring(FirstIndexPstofBackSlash + 1, lenOfTheFileName)
                        'MessageBox.Show(origFileName)
                        sArchivePath = ConfigurationManager.AppSettings("Archive")
                        Dim sArchiveFile As String = Path.Combine(sArchivePath, sFile)

                        strFileLocation = ConfigurationManager.AppSettings("IncomingFiles")

                        TempLocation = strFileLocation & "\Temp"
                        StrDestinationFilePath = strFileLocation & "\Files"
                        If Not Directory.Exists(StrDestinationFilePath) Then Directory.CreateDirectory(StrDestinationFilePath)
                        If Not Directory.Exists(TempLocation) Then Directory.CreateDirectory(TempLocation)
                        sArchivePath = Path.Combine(strFileLocation, "ARCHIVE\" & Now.ToString("yyyyMMdd"))
                        sCorruptPath = Path.Combine(sArchivePath, "CORRUPT")
                        If Not Directory.Exists(sArchivePath) Then Directory.CreateDirectory(sArchivePath)

                        'Threading.Thread.Sleep(100)
                        lbl.Text = Path.GetFileName(sFile)
                        lbl.Update()
                        prg.Value = 0
                        prg.PerformStep()
                        prg.Update()
                        Application.DoEvents()
                        Dim sExt As String = Path.GetExtension(sFile).ToLower()
                        Dim sDir As String = Path.Combine(strFileLocation, Path.GetFileNameWithoutExtension(sFile))
                        Dim dest As String = Path.GetDirectoryName(sFile)
                        Dim isZip As Boolean = False
                        dest = Path.GetDirectoryName(dest)
                        arcs = dest
                        arcs = arcs
                        arcs = Path.GetDirectoryName(dest)
                        dest = dest & "\Files"
                        DestPath = dest
                        If Not Directory.Exists(dest) Then Directory.CreateDirectory(dest)
                        Select Case sExt.ToUpper()
                            Case ".ZIP"
                                If fType = FileType.ChequeRejects Then
                                    sFile = Path.Combine(strFileLocation, sFile)
                                    If RejectedItems(sFile) Then bArchive = True
                                Else
                                    Try
                                        If Sign Then UnSignFile(sFile)
                                    Catch ex As Exception
                                        MessageBox.Show(ex.Message)
                                    End Try

                                    If Directory.Exists(strFileLocation + "\Unsign") = False Then
                                        Directory.CreateDirectory(strFileLocation + "\Unsign")
                                    End If

                                    'Rename the unsigned file and move to unsigned folder
                                    'MessageBox.Show(" From: " + strFileLocation + "\Unsign\" + Path.GetFileName(sFile) + " To: " + strFileLocation + "\Temp\" + Path.GetFileName(sFile) + ".Unsigned.CMS")
                                    If File.Exists(strFileLocation + "\Unsign\" + Path.GetFileName(sFile)) Then
                                        File.Delete(strFileLocation + "\Unsign\" + Path.GetFileName(sFile))
                                        Try
                                            File.Move(strFileLocation + "\Temp\" + Path.GetFileName(sFile) + ".unsigned.CMS", strFileLocation + "\Unsign\" + Path.GetFileName(sFile))
                                        Catch ex As Exception
                                            If Sign Then
                                                MessageBox.Show(ex.Message)
                                            End If
                                        End Try
                                        File.Delete(strFileLocation + "\Temp\" + Path.GetFileName(sFile) + ".Unsigned.CMS")
                                    Else
                                        Try
                                            File.Move(strFileLocation + "\Temp\" + Path.GetFileName(sFile), strFileLocation + "\Unsign\" + Path.GetFileName(sFile))
                                        Catch ex As Exception
                                            If Sign Then
                                                MessageBox.Show(ex.Message)
                                            End If
                                        End Try
                                        File.Delete(strFileLocation + "\Temp\" + Path.GetFileName(sFile))
                                    End If

                                    'Move from unsigned folder to temp folder
                                    'MessageBox.Show(" From: " + strFileLocation + "\Unsign\" + Path.GetFileName(sFile) + " To: " + strFileLocation + "\Temp\" + Path.GetFileName(sFile) + ".Unsigned.CMS")
                                    If File.Exists(strFileLocation + "\Temp\" + Path.GetFileName(sFile)) Then
                                        File.Delete(strFileLocation + "\Temp\" + Path.GetFileName(sFile))
                                        File.Move(strFileLocation + "\Unsign\" + Path.GetFileName(sFile), strFileLocation + "\Temp\" + Path.GetFileName(sFile))
                                        File.Delete(strFileLocation + "\Unsign\" + Path.GetFileName(sFile))
                                    Else
                                        File.Move(strFileLocation + "\Unsign\" + Path.GetFileName(sFile), strFileLocation + "\Temp\" + Path.GetFileName(sFile))
                                        File.Delete(strFileLocation + "\Unsign\" + Path.GetFileName(sFile))
                                    End If


                                    'Archive Unsigned
                                    sArchiveFile = ""
                                    If Not Directory.Exists(Path.GetDirectoryName(sArchivePath)) Then Directory.CreateDirectory(Path.GetDirectoryName(sArchivePath))
                                    If (File.Exists(sFile)) Then
                                        sArchiveFile = sArchivePath + "\" + Path.GetFileName(sFile)
                                    End If

                                    bArchive = True

                                    If Not File.Exists(sArchiveFile) And bArchive Then
                                        File.Copy(sFile, sArchiveFile, True)
                                    End If

                                    'Now proceed with the normal exercise.
                                    Dim l As List(Of String) = UnzipFiles(sFile, New String() {"*.cheque*", "*.xml", "*.tif*"})
                                    If l.AsQueryable().Any(Function(p) p.EndsWith("xml") Or p.Contains("tif")) Then
                                        BulkCheque(l, origFileName)
                                    Else
                                        ChequeTransaction(l, ChequeFormat.SISPackage)
                                    End If
                                    'If Directory.Exists(sDir) Then Directory.Delete(sDir, True)
                                    'sFile = Path.Combine(strFileLocation, sFile)
                                    'bArchive = True

                                    Dim DirToremove As DirectoryInfo = New DirectoryInfo(strFileLocation + "\Temp\" + Path.GetFileNameWithoutExtension(sFile))

                                    For Each FiToDel As FileInfo In DirToremove.GetFiles()
                                        FiToDel.Delete()
                                    Next

                                    DirToremove.Delete(True)
                                    sFile = ""
                                    isZip = True
                                End If
                            Case ".CHK"
                                'MessageBox.Show("5")
                                Dim l As List(Of String) = UnzipFiles(sFile, New String() {"*.xml", "*.tif*"})
                                'MessageBox.Show("6")
                                BulkCheque(l, origFileName)
                                'MessageBox.Show("7")
                                If Directory.Exists(sDir) Then Directory.Delete(sDir, True)
                                sFile = Path.Combine(strFileLocation, sFile)
                                bArchive = True
                                sArchiveFile = Path.Combine(sArchivePath, Path.GetFileName(sFile))
                            Case ".CMS"
                                UnSignFile(sFile)
                                Modscan.Wait(1)
                                dest = dest & "\" & Path.GetFileName(sFile & ".unsigned.CMS")
                                File.Move(sFile & ".unsigned.CMS", dest)
                                Dim l As List(Of String) = UnzipFiles(dest, New String() {"*.xml", "*.tif*"}, Path.GetDirectoryName(dest))
                                Modscan.Wait(1)
                                BulkCheque(l, origFileName)
                                Modscan.Wait(1)
                                sArchivePath = Modscan.ArchivesPath & Now.ToString("yyyyMMdd") & "\"
                                sArchiveFile = sArchivePath & Path.GetFileName(sFile)
                                bArchive = True
                            Case ".CHR", ".Q"
                                'If Sign = True Then
                                '    'MessageBox.Show("Imefika Mbili")
                                '    ReadFile(sFile, strFileLocation)
                                '    'MessageBox.Show("Imefika tatu")
                                'End If
                                RejectedItems(sFile)
                                bArchive = True
                                sArchiveFile = Path.Combine(sArchivePath, Path.GetFileName(sFile))
                            Case ".V", ".R", ".RC"
                                ReadFile(sFile, strFileLocation)
                                sFile = Path.Combine(strFileLocation, sFile)
                                'ResponsesFromACH(sFile)
                                bArchive = True
                                sArchiveFile = Path.Combine(sArchivePath, Path.GetFileName(sFile))
                            Case ".S"

                                'RejectedEFTs(sFile)
                                'MessageBox.Show("Imefika moja")
                                If Sign = True Then
                                    'MessageBox.Show("Imefika Mbili")
                                    ReadFile(sFile, strFileLocation)
                                    'MessageBox.Show("Imefika tatu")
                                End If
                                'MessageBox.Show("Imefika nne")
                                BulkCredit(sFile)
                                'MessageBox.Show("Imefika tano")
                                '    sFile = Path.Combine(strFileLocation, sFile)
                                '    bArchive = True
                                '    sArchivePath = Modscan.ArchivesPath & Now.ToString("yyyyMMdd") & "\"
                                'sArchiveFile = sArchivePath & Path.GetFileName(sFile)
                                ''Archive Unsigned
                                'sArchiveFile = ""
                                'If Not Directory.Exists(Path.GetDirectoryName(sArchivePath)) Then Directory.CreateDirectory(Path.GetDirectoryName(sArchivePath))
                                'If (File.Exists(sFile)) Then
                                '    sArchiveFile = sArchivePath + "\" + Path.GetFileName(sFile)
                                'End If

                                'bArchive = True

                                'If Not File.Exists(sArchiveFile) And bArchive Then
                                '    File.Copy(sFile, sArchiveFile, True)
                                'End If
                            Case ".N"
                                If Sign = True Then
                                    ReadFile(strFileLocation, sFile)
                                End If
                                BulkDebit(sFile)
                                bArchive = True
                                sArchiveFile = Path.Combine(sArchivePath, Path.GetFileName(sFile))
                            Case ".TXT"
                                If Sign = True Then
                                    ReadFile(strFileLocation, sFile)
                                End If
                                SingleRTGS(sFile)
                                sArchiveFile = Path.Combine(Path.Combine(sArchivePath, "RTGSPAYMENTS"), sFile)
                                sFile = Path.Combine(Path.Combine(strFileLocation, "RTGSPAYMENTS"), sFile)
                                bArchive = True
                            Case Else

                                bArchive = True
                        End Select
                        prg.PerformStep()
                        prg.Update()
                        prgAll.PerformStep()
                        prgAll.Update()
                        Application.DoEvents()

                        If Not File.Exists(sArchiveFile) And bArchive Then
                            File.Copy(sFile, sArchiveFile, True)
                        End If
                        isZip = False
                    Catch ex As Exception
                        MessageBox.Show("Error registerd, check error log")
                        Modscan.ErrorLog(ex.Message, "- Inwards File : " + Path.GetFileName(sFile))
                        Continue For
                    End Try
                Next

                Dim di As New DirectoryInfo(DestPath)
                Dim fi As FileInfo() = di.GetFiles()
                For Each inf As FileInfo In fi
                    File.Delete(inf.FullName)
                Next
                prgAll.Value = 100
                prgAll.Update()
                Application.DoEvents()
            Catch ex As Exception
                'errorLog(ex.StackTrace, "102", "Iwards Import", "Import Files", Now, "Import Files")
            End Try
        End Sub
        Private Shared Sub BulkCredit(ByVal sFile As String)

            Dim RegX As New Regex("[^A-Za-z0-9]")
            Dim sTempFile As String = Path.Combine(TempLocation, sFile)
            sFile = Path.Combine(StrDestinationFilePath, sFile)

            Dim EJfilePaths As String() = Nothing
            Dim Day As String = Modscan.FDATE.Day.ToString().PadLeft(2, "0")
            Dim Month As String = Modscan.FDATE.Month.ToString().PadLeft(2, "0")
            Dim FDayMonth As String = (Convert.ToString(New String("0"c, 2 - (BRBaseConvert.ConvertToString(Day).Length)) & BRBaseConvert.ConvertToString(Day)) & New String("0"c, 2 - (BRBaseConvert.ConvertToString(Month).Length))) + BRBaseConvert.ConvertToString(Month)
            Dim SelectedStringCol As StringCollection = New StringCollection()
            Dim ReWorkedStringCol As StringCollection = New StringCollection()
            Dim SystemType As String = ConfigurationManager.AppSettings("sysType")
            'EJfilePaths = Directory.GetFiles(Path.GetDirectoryName(sFile))
            'For f As Int32 = 0 To EJfilePaths.Length - 1
            '    If EJfilePaths(f).Substring(EJfilePaths(f).ToString().LastIndexOf("\") + 1).Substring(2, 4) = FDayMonth Then
            '        SelectedStringCol.Add(EJfilePaths(f).ToString())
            '    End If
            'Next
            'For p As Int32 = 0 To SelectedStringCol.Count - 1
            '    If (SelectedStringCol(p).Substring(SelectedStringCol(p).ToString().LastIndexOf(".") + 1).Substring(0, 1).ToUpper() = "T") Then
            ReWorkedStringCol.Add(sFile.ToString())
            '    End If
            'Next
            For f As Int32 = 0 To ReWorkedStringCol.Count - 1
                sFile = ReWorkedStringCol(f).ToString

                Dim doc As New cr.Document()
                Dim ex As New Exception()
                If cr.Document.LoadFromFile(sFile, doc, ex) Then
                    For Each c As cr.CreditTransferTransactionInformation11 In doc.FIToFICstmrCdtTrf.CdtTrfTxInf
                        Dim d As New ETEFTDetails
                        d.MsgId = doc.FIToFICstmrCdtTrf.GrpHdr.MsgId
                        d.TrxId = c.PmtId.TxId
                        d.Amount = c.IntrBkSttlmAmt.Value
                        d.Currency = c.IntrBkSttlmAmt.Ccy.ToString()
                        d.SourceBankID = c.DbtrAgt.FinInstnId.BIC
                        d.VCode = "58" ' ' c.PmtTpInf.CtgyPurp.Item
                        d.RetCode = "00"
                        Try
                            d.UstrdColD = IIf(IsDBNull(c.RmtInf.Ustrd(0).ToString), "", c.RmtInf.Ustrd(0).ToString)
                        Catch exUstrdColD As Exception
                            d.UstrdColD = ""
                        End Try
                        Try
                            d.DAdrLine = IIf(c.Dbtr.PstlAdr.AdrLine(0).ToString = "", "", c.Dbtr.PstlAdr.AdrLine(0).ToString)
                        Catch exDAdrLine As Exception
                            d.DAdrLine = ""
                        End Try
                        Try
                            d.DTwnNm = IIf(c.Dbtr.PstlAdr.TwnNm.ToString = "", "", c.Dbtr.PstlAdr.TwnNm)
                        Catch exDTwnNm As Exception
                            d.DTwnNm = ""
                        End Try
                        Try
                            d.DCtry = IIf(c.Dbtr.PstlAdr.Ctry.ToString = "", "", c.Dbtr.PstlAdr.Ctry)
                        Catch exDCtry As Exception
                            d.DCtry = ""
                        End Try
                        Try
                            d.DNm = c.Dbtr.Nm
                        Catch exDNm As Exception
                            d.DNm = ""
                        End Try
                        Try
                            d.DPhneNb = IIf(c.Dbtr.CtctDtls.PhneNb.ToString = "", "", c.Dbtr.CtctDtls.PhneNb)
                        Catch exDPhneNb As Exception
                            d.DPhneNb = ""
                        End Try
                        Try
                            d.DMobNb = IIf(c.Dbtr.CtctDtls.MobNb.ToString = "", "", c.Dbtr.CtctDtls.MobNb)
                        Catch exDMobNb As Exception
                            d.DMobNb = ""
                        End Try
                        Try
                            d.DEmailAdr = IIf(c.Dbtr.CtctDtls.EmailAdr.ToString = "", "", c.Dbtr.CtctDtls.EmailAdr)
                        Catch exDEmailAdr As Exception
                            d.DEmailAdr = ""
                        End Try
                        Try
                            d.DOthr = IIf(c.Dbtr.CtctDtls.Othr.ToString = "", "", c.Dbtr.CtctDtls.Othr)
                        Catch exDOthr As Exception
                            d.DOthr = ""
                        End Try
                        Try
                            d.DbtrAcct = c.DbtrAcct.Id.Item.ToString()
                        Catch exDbtrAcct As Exception
                            d.DbtrAcct = ""
                        End Try
                        Try
                            d.CAdrLine = IIf(c.Cdtr.PstlAdr.AdrLine(0).ToString = "", "", c.Cdtr.PstlAdr.AdrLine(0).ToString)
                        Catch exCAdrLine As Exception
                            d.CAdrLine = ""
                        End Try
                        Try
                            d.CTwnNm = IIf(c.Cdtr.PstlAdr.TwnNm.ToString = "", "", c.Cdtr.PstlAdr.TwnNm)
                        Catch exCTwnNm As Exception
                            d.CTwnNm = ""
                        End Try
                        Try
                            d.CCtry = IIf(c.Cdtr.PstlAdr.Ctry.ToString = "", "", c.Cdtr.PstlAdr.Ctry)
                        Catch exCCtry As Exception
                            d.CCtry = ""
                        End Try
                        Try
                            d.CNm = IIf(c.Cdtr.Nm.ToString = "", "", c.Cdtr.Nm)
                        Catch exCNm As Exception
                            d.CNm = ""
                        End Try
                        Try
                            d.CPhneNb = IIf(c.Cdtr.CtctDtls.PhneNb.ToString = "", "", c.Cdtr.CtctDtls.PhneNb)
                        Catch exCPhneNb As Exception
                            d.CPhneNb = ""
                        End Try
                        Try
                            d.CMobNb = IIf(c.Cdtr.CtctDtls.MobNb.ToString = "", "", c.Cdtr.CtctDtls.MobNb)
                        Catch exCMobNb As Exception
                            d.CMobNb = ""
                        End Try
                        Try
                            d.CEmailAdr = IIf(c.Cdtr.CtctDtls.EmailAdr.ToString = "", "", c.Cdtr.CtctDtls.EmailAdr)
                        Catch exCEmailAdr As Exception
                            d.CEmailAdr = ""
                        End Try
                        Try
                            d.COthr = IIf(c.Dbtr.CtctDtls.Othr.ToString = "", "", c.Dbtr.CtctDtls.Othr)
                        Catch exCOthr As Exception
                            d.COthr = ""
                        End Try
                        Try
                            d.PymType = ""
                        Catch exPymType As Exception
                            d.PymType = ""
                        End Try
                        Try
                            d.CdtrAcct = c.CdtrAcct.Id.Item.ToString()
                        Catch exCdtrAcct As Exception
                            d.CdtrAcct = ""
                        End Try
                        Try
                            d.OrgnlInstrID = IIf(c.PmtId.InstrId.ToString = "", "", c.PmtId.InstrId)
                        Catch exOrgnlInstrID As Exception
                            d.OrgnlInstrID = ""
                        End Try
                        Try
                            d.OrgnlEndToEnd = c.PmtId.EndToEndId
                        Catch exOrgnlEndToEnd As Exception
                            d.OrgnlEndToEnd = ""
                        End Try

                        'Try
                        '    d.Ustrd = IIf(IsDBNull(c.RmtInf.Ustrd(0).ToString), "", c.RmtInf.Ustrd(0).ToString)
                        'Catch exUstrd As Exception
                        '    d.Ustrd = ""
                        'End Try
                        d.TrxData = d.MsgId & ":" & d.TrxId

                        SaveEFT(d, sFile)
                    Next
                Else
                    Throw ex
                End If
                Dim strpath = sFile.LastIndexOf(".") + 1
                sArchivePath = Path.Combine(strFileLocation, "ARCHIVE\" & Now.ToString("yyyyMMdd"))
                Dim sArchiveFile As String = Path.Combine(sArchivePath, sFile)
                If Not Directory.Exists(Path.GetDirectoryName(sArchivePath)) Then Directory.CreateDirectory(Path.GetDirectoryName(sArchivePath))
                If (File.Exists(sArchiveFile)) Then sArchiveFile = sArchivePath & "\" & sFile.Substring(sFile.LastIndexOf("\") + 1)
                Try
                    If File.Exists(sFile) Then File.Move(sFile, sArchiveFile)
                Catch exi As Exception
                    File.Delete(sArchiveFile)
                    If File.Exists(sFile) Then File.Move(sFile, sArchiveFile)
                End Try
                File.Delete(sFile)
            Next
        End Sub
        Private Shared Sub BulkDebit(ByVal sFile As String)
            Dim RegX As New Regex("[^A-Za-z0-9]")
            Dim sTempFile As String = Path.Combine(TempLocation, sFile)
            sFile = Path.Combine(strFileLocation, sFile)
            If Sign Then StripSignature(sFile, sTempFile)
            Dim doc As New dr.Document()
            Dim doc2 As New res.Document()
            Dim ex As New Exception()
            If dr.Document.LoadFromFile(sFile, doc, ex) Then
                For Each c As dr.DirectDebitTransactionInformation10 In doc.FIToFICstmrDrctDbt.DrctDbtTxInf
                    Dim d As New ETDDDetail
                    Try
                        Dim e As New ETDDDetail
                        e.MsgId = doc.FIToFICstmrDrctDbt.GrpHdr.MsgId
                        e.IntrBkSttlmDt = doc.FIToFICstmrDrctDbt.GrpHdr.IntrBkSttlmDt.ToString("dd-MMM-yy")
                        e.Curr = c.IntrBkSttlmAmt.Ccy.ToString()
                        e.dBIC = c.DbtrAgt.FinInstnId.BIC
                        e.Collection = c.ReqdColltnDt
                        e.VCode = "40"
                        e.Retcode = "00"
                        e.InstrId = c.PmtId.InstrId
                        e.EndToEndId = c.PmtId.EndToEndId
                        e.TrxId = c.PmtId.TxId
                        e.SvcLvl = "SEPA"
                        e.LclInstrm = "B2B"
                        e.Amount = c.IntrBkSttlmAmt.Value
                        e.ReqdColltnDt = c.ReqdColltnDt.ToString("dd-MMM-yy")
                        e.Mandate.MndtId = c.DrctDbtTx.MndtRltdInf.MndtId
                        e.Mandate.DtOfSgntr = c.DrctDbtTx.MndtRltdInf.DtOfSgntr.ToString("dd-MMM-yy")
                        e.Mandate.AmdmntInd = c.DrctDbtTx.MndtRltdInf.AmdmntInd
                        e.Mandate.Frqcy = "M"
                        e.Scheme = c.DrctDbtTx.CdtrSchmeId.Id.Item.Othr.Id
                        e.CNm = RegX.Replace(c.Cdtr.Nm, " ")
                        e.sBIC = c.CdtrAgt.FinInstnId.BIC
                        e.SourceBankID = c.CdtrAgt.FinInstnId.BIC
                        e.CdtrAcct = RegX.Replace(c.CdtrAcct.Id.Item, String.Empty)
                        e.DNm = RegX.Replace(c.Dbtr.Nm, " ")
                        e.DbtrAcct = RegX.Replace(c.DbtrAcct.Id.Item, String.Empty)
                        e.DestBankID = c.DbtrAgt.FinInstnId.BIC
                        e.Remittance = c.RmtInf.Item
                        e.Creation = doc.FIToFICstmrDrctDbt.GrpHdr.CreDtTm
                        e.UstrdColD = String.Empty
                        e.TrxData = String.Empty
                        e.DAdrLine = String.Empty
                        e.DTwnNm = String.Empty
                        e.DCtry = String.Empty
                        e.DPhneNb = String.Empty
                        e.DMobNb = String.Empty
                        e.DEmailAdr = String.Empty
                        e.DOthr = String.Empty
                        e.CAdrLine = String.Empty
                        e.CTwnNm = String.Empty
                        e.CCtry = String.Empty
                        e.CPhneNb = String.Empty
                        e.CMobNb = String.Empty
                        e.CEmailAdr = String.Empty
                        e.COthr = String.Empty
                        e.PymType = String.Empty
                        SaveDD(e, sFile)
                    Catch
                    End Try
                Next
            Else
                If res.Document.LoadFromFile(sFile, doc2, ex) Then
                    For Each c As res.PaymentTransactionInformation26 In doc2.FIToFIPmtStsRpt.TxInfAndSts
                        Try
                            Dim e As New ETDDDetail
                            e.Amount = Decimal.Parse(c.OrgnlTxRef.IntrBkSttlmAmt.Value)
                            e.EndToEndId = c.OrgnlEndToEndId
                            e.OrgnlInstrID = c.OrgnlInstrId
                            e.OrgnlTxId = c.OrgnlEndToEndId
                            e.OrgnlIntrBkSttlmDt = c.OrgnlTxRef.IntrBkSttlmDtt.ToString("dd-MMM-yy")
                            e.Retcode = c.StsRsnInf.Rsn.Item
                            e.Curr = c.OrgnlTxRef.IntrBkSttlmAmt.Ccy.ToString()
                            e.MsgId = doc2.FIToFIPmtStsRpt.GrpHdr.MsgId
                            SaveRejectedDD(e, sFile)
                        Catch
                        End Try
                    Next
                End If
            End If
            Dim strpath = sFile.LastIndexOf(".") + 1
            sArchivePath = Path.Combine(strFileLocation, "ARCHIVE\" & Now.ToString("yyyyMMdd"))
            Dim sArchiveFile As String = Path.Combine(sArchivePath, sFile)
            If Not Directory.Exists(Path.GetDirectoryName(sArchivePath)) Then Directory.CreateDirectory(Path.GetDirectoryName(sArchivePath))
            If (File.Exists(sArchiveFile)) Then sArchiveFile = sArchivePath & "\" & sFile.Substring(sFile.LastIndexOf("\") + 1)
            Try
                If File.Exists(sFile) Then File.Move(sFile, sArchiveFile)
            Catch exi As Exception
                File.Delete(sArchiveFile)
                If File.Exists(sFile) Then File.Move(sFile, sArchiveFile)
            End Try
            File.Delete(sTempFile)
        End Sub
        Private Shared Sub SingleRTGS(ByVal sFile As String)



            Dim sTempFile As String = Path.Combine(TempLocation, sFile)
            Dim DirPath As String = Path.GetDirectoryName(sFile)
            Dim FileName As String = Path.GetFileName(sFile)
            Dim sContent As String = ""
            If File.Exists(Path.Combine(DirPath, "RTGSPAYMENTS\" & FileName)) Then
                sFile = Path.Combine(DirPath, "RTGSPAYMENTS\" & FileName)
            ElseIf File.Exists(Path.Combine(DirPath, "RTGSSTATEMENTS\" & FileName)) Then
                sFile = Path.Combine(DirPath, "RTGSSTATEMENTS\" & FileName)
            ElseIf File.Exists(Path.Combine(DirPath, "RTGSREPLIES\" & FileName)) Then
                sFile = Path.Combine(DirPath, "RTGSREPLIES\" & FileName)
            ElseIf File.Exists(Path.Combine(DirPath, "RTGSADVICES\" & FileName)) Then
                sFile = Path.Combine(DirPath, "RTGSADVICES\" & FileName)
            ElseIf File.Exists(FileName) Then
                sFile = sFile
            ElseIf File.Exists(Path.Combine(DirPath, "RTGSPAYMENTS\\" + Path.GetFileNameWithoutExtension(FileName))) Then
                sFile = Path.Combine(DirPath, "RTGSPAYMENTS\\" + Path.GetFileNameWithoutExtension(FileName))
            End If
            If Sign Then
                StripSignature(sFile, sTempFile)
            End If

            Dim rtgsProcessor = New ETRTGSProcessing(sFile, False, "", "", Nothing, "", "")
            Dim rtgs As AchRtgs = rtgsProcessor.BRRTGSFiles(sFile)

            SaveRTGS(rtgs)

            Dim strpath = sFile.LastIndexOf(".") + 1
            sArchivePath = Path.Combine(strFileLocation, "ARCHIVE\" & Now.ToString("yyyyMMdd") & "\RTGS\")
            Dim sArchiveFile As String = Path.Combine(sArchivePath, sFile)
            If Not Directory.Exists(Path.GetDirectoryName(sArchivePath)) Then Directory.CreateDirectory(Path.GetDirectoryName(sArchivePath))
            If (File.Exists(sArchiveFile)) Then sArchiveFile = sArchivePath & "\" & sFile.Substring(sFile.LastIndexOf("\") + 1)
            Try
                If File.Exists(sFile) Then File.Move(sFile, sArchiveFile)
            Catch exi As Exception
                File.Delete(sArchiveFile)
                If File.Exists(sFile) Then File.Move(sFile, sArchiveFile)
            End Try
            File.Delete(sFile)
        End Sub
        Private Shared Function GetRTGSDetails(ByVal sContent As String) As ETEFTDetails
            Dim RegX As New Regex("[^A-Za-z0-9]")
            Dim sDetals As String() = sContent.Split(Environment.NewLine.ToCharArray())
            sContent = sContent.Replace(vbCr & vbLf, "")
            Dim sGroups As String() = sContent.Split("{"c)
            Dim sAllowed As String() = New String() {"103", "202", "900", "910", "941", "950", "999"}
            Dim sMsghdr As String = sGroups(2).Substring(3, 3)
            Dim rec As New ETEFTDetails()
            rec.RetCode = "0"
            rec.BeneficiaryName = ""
            rec.RemitterName = ""
            rec.BeneficiaryAcc = ""
            rec.RemitterAcc = ""
            rec.Amount = 0
            For i As Integer = 0 To sDetals.Length - 1
                Dim s As String = sDetals(i)
                If sMsghdr = sAllowed(0) Then
                    s = s.Trim()
                    If s.StartsWith(":20:") Then
                        rec.Reference = s.Remove(0, 4)
                    ElseIf s.StartsWith(":32A:") Then
                        rec.Amount = s.Substring(14).Replace(",", ".")
                        rec.Currency = s.Substring(11, 3)
                        Dim sDate As String = s.Substring(5, 6)
                        rec.ValueDate = DateSerial("20" & sDate.Substring(0, 2), sDate.Substring(2, 2), sDate.Substring(4))
                    ElseIf s.StartsWith(":59:") Then
                        rec.BeneficiaryAcc = RegX.Replace(s.Substring(4), " ").Trim()
                        If rec.BeneficiaryAcc.Length > 20 Then rec.BeneficiaryAcc = rec.BeneficiaryAcc.Substring(0, 20)
                        rec.BeneficiaryName = IIf(sDetals(i + 1).Trim().StartsWith(":"), s.Substring(4), sDetals(i + 1).Trim).Replace("\", "").Replace("'", "")
                    ElseIf s.StartsWith(":52A:") Then
                        rec.SourceBIC = s.Substring(5)
                    ElseIf s.StartsWith(":57A:") Then
                        rec.DestBIC = s.Substring(5)
                    ElseIf s.StartsWith(":50K:") Then
                        rec.RemitterAcc = RegX.Replace(s.Substring(5), " ").Trim()
                        If rec.RemitterAcc.Length > 20 Then rec.RemitterAcc = rec.RemitterAcc.Substring(0, 20)
                        rec.RemitterName = IIf(sDetals(i + 1).StartsWith(":"), s.Substring(5), sDetals(i + 1).Trim).Replace("\", "").Replace("'", "")
                    ElseIf s.StartsWith(":70:") Then
                        rec.Reference = s.Substring(4)
                    ElseIf s.StartsWith(":72/:") Then
                        rec.RemittanceInfo = s.Substring(5)
                    End If
                    rec.TranType = 1
                    rec.EFTID = Modscan.GetNextInt16
                    rec.IsDebit = False
                    If rec.Reference = Nothing Then rec.Reference = rec.EFTID
                ElseIf sMsghdr = sAllowed(1) Then
                    s = s.Trim()
                    If s.StartsWith(":20:") Then
                        rec.Reference = s.Remove(0, 4)
                    ElseIf s.StartsWith(":21:") Then
                        rec.RemittanceInfo = s.Remove(0, 4)
                    ElseIf s.StartsWith(":32A:") Then
                        rec.Amount = s.Substring(14).Replace(",", ".")
                        rec.Currency = s.Substring(11, 3)
                        Dim sDate As String = s.Substring(5, 6)
                        rec.ValueDate = DateSerial("20" & sDate.Substring(0, 2), sDate.Substring(2, 2), sDate.Substring(4))
                    ElseIf s.StartsWith(":52A:") Then
                        rec.SourceBIC = IIf(sDetals(i + 1).Trim().StartsWith(":"), s.Substring(5), sDetals(i + 1).Trim())
                    ElseIf s.StartsWith(":58A:") Then
                        rec.DestBIC = s.Substring(5)
                        rec.BeneficiaryName = IIf(sDetals(i + 1).StartsWith(":"), rec.DestBIC, sDetals(i + 1).Trim()).Replace("\", "").Replace("'", "")
                    End If
                    'Dim Curr As String = FindRecord("CURRENCYTYPES", "CURRENCYCODE", "BANKSYSCODE = '" & rec.Currency & "'")
                    'rec.BeneficiaryAcc = FindRecord("BANKS", "SWIFTACCOUNT", "CODE = '" & strBankCode & "'")
                    'rec.RemitterAcc = FindRecord("LCUPSETTINGS", "INRTGSGL", "BRANCHCODE = '" & strBranchCode & "' AND CURRENCYCODE = '" & Curr & "'")
                    rec.RemitterName = rec.SourceBIC
                    rec.TranType = 2
                    rec.EFTID = Modscan.GetNextInt16
                    rec.IsDebit = False
                    If rec.Reference = Nothing Then rec.Reference = rec.EFTID
                ElseIf sMsghdr = sAllowed(2) Then
                    s = s.Trim()
                    If s.Contains(":20:") Then
                        rec.Reference = s.Remove(0, 4)
                    ElseIf s.Contains(":21:") Then
                        rec.RetCode = s.Remove(0, 4)
                    ElseIf s.Contains(":25:") Then
                        rec.RemittanceInfo = s.Remove(0, 4)
                    ElseIf s.Contains(":32A:") Then
                        rec.Amount = Convert.ToDecimal(s.Substring(14).Replace(",", "."))
                        rec.Currency = s.Substring(11, 3)
                        Dim sDate As String = s.Substring(5, 6)
                        rec.ValueDate = DateSerial("20" & sDate.Substring(0, 2), sDate.Substring(2, 2), sDate.Substring(4))
                    ElseIf s.Contains(":52A:") Then
                        rec.SourceBIC = (If(sDetals(i + 2).Trim().StartsWith(":") OrElse sDetals(i + 2).Trim() = "", s.Substring(5), sDetals(i + 2).Trim()))
                    ElseIf s.StartsWith(":72:/") Then
                        rec.RemittanceInfo = s.Substring(6)
                        If i + 2 < sDetals.Length Then
                            rec.RemittanceInfo += (If(sDetals(i + 2).Trim().StartsWith(":") OrElse sDetals(i + 2).Trim() = "" OrElse sDetals(i + 2).Trim() = "-}", "", sDetals(i + 2).Trim()))
                        End If
                        If i + 4 < sDetals.Length Then
                            rec.RemittanceInfo += (If(sDetals(i + 4).Trim().StartsWith(":") OrElse sDetals(i + 4).Trim() = "" OrElse sDetals(i + 4).Trim() = "-}", "", sDetals(i + 4).Trim()))
                        End If
                    End If
                    rec.BeneficiaryName = rec.DestBIC
                    rec.RemitterName = rec.SourceBIC
                    rec.RemitterAcc = rec.SourceBIC
                    rec.DestBIC = sOurBIC
                    rec.BeneficiaryAcc = rec.SourceBIC
                    rec.TranType = 3
                    rec.EFTID = Modscan.GetNextInt16
                    rec.IsDebit = True
                ElseIf sMsghdr = sAllowed(3) Then
                    s = s.Trim()
                    If s.Contains(":20:") Then
                        rec.Reference = s.Remove(0, 4)
                    ElseIf s.Contains(":21:") Then
                        rec.RetCode = s.Remove(0, 4)
                    ElseIf s.Contains(":25:") Then
                        rec.RemittanceInfo = s.Remove(0, 4)
                    ElseIf s.Contains(":32A:") Then
                        rec.Amount = Convert.ToDecimal(s.Substring(14).Replace(",", "."))
                        rec.Currency = s.Substring(11, 3)
                        Dim sDate As String = s.Substring(5, 6)
                        rec.ValueDate = DateSerial("20" & sDate.Substring(0, 2), sDate.Substring(2, 2), sDate.Substring(4))
                    ElseIf s.Contains(":52A:") Then
                        rec.SourceBIC = (If(sDetals(i + 1).Trim().StartsWith(":") OrElse sDetals(i + 1).Trim() = "", s.Substring(5), sDetals(i + 1).Trim()))
                    ElseIf s.StartsWith(":72:/") Then
                        rec.RemittanceInfo = s.Substring(6)
                        If i + 2 < sDetals.Length Then
                            rec.RemittanceInfo += (If(sDetals(i + 2).Trim().StartsWith(":") OrElse sDetals(i + 2).Trim() = "" OrElse sDetals(i + 2).Trim() = "-}", "", sDetals(i + 2).Trim()))
                        End If
                        If i + 4 < sDetals.Length Then
                            rec.RemittanceInfo += (If(sDetals(i + 4).Trim().StartsWith(":") OrElse sDetals(i + 4).Trim() = "" OrElse sDetals(i + 4).Trim() = "-}", "", sDetals(i + 4).Trim()))
                        End If
                    End If
                    rec.DestBIC = sOurBIC
                    rec.BeneficiaryName = rec.DestBIC
                    rec.BeneficiaryAcc = rec.SourceBIC
                    rec.RemitterAcc = rec.SourceBIC
                    rec.RemitterName = rec.SourceBIC
                    rec.TranType = 4
                    rec.EFTID = Modscan.GetNextInt16
                    rec.IsDebit = False
                ElseIf sMsghdr = sAllowed(4) Then
                    s = s.Trim()
                    If s.Contains(":20:") Then
                        rec.Reference = s.Remove(0, 4)
                    ElseIf s.Contains(":21:") Then
                        rec.RetCode = s.Remove(0, 4)
                    ElseIf s.Contains(":25:") Then
                        rec.BeneficiaryAcc = s.Remove(0, 4)
                    ElseIf s.Contains(":62F:") Then
                        rec.Amount = Convert.ToDecimal(s.Substring(15).Replace(",", "."))
                        rec.Currency = s.Substring(12, 3)
                        Dim sDate As String = s.Substring(5, 6)
                    End If
                    rec.SourceBIC = "NBETETAB"
                    rec.DestBIC = sOurBIC
                    rec.RemitterAcc = rec.SourceBIC
                    rec.RemitterName = rec.SourceBIC
                    rec.TranType = 5
                    rec.IsDebit = False
                ElseIf sMsghdr = sAllowed(5) Then
                    s = s.Trim()
                    If s.Contains(":20:") Then
                        rec.Reference = s.Remove(0, 4)
                    ElseIf s.Contains(":21:") Then
                        rec.RetCode = s.Remove(0, 4)
                    ElseIf s.Contains(":25:") Then
                        rec.BeneficiaryAcc = s.Remove(0, 4)
                    ElseIf s.Contains(":28C:") Then
                        rec.BeneficiaryAcc = s.Remove(0, 4)
                    ElseIf s.Contains(":62F:") Then
                        rec.Amount = Convert.ToDecimal(s.Substring(15).Replace(",", "."))
                        rec.Currency = s.Substring(12, 3)
                        Dim sDate As String = s.Substring(5, 6)
                    ElseIf s.StartsWith(":61:") Then
                        rec.RemittanceInfo = s.Substring(5)
                        If i + 2 < sDetals.Length Then
                            rec.RemittanceInfo += (If(sDetals(i + 2).Trim().StartsWith(":") OrElse sDetals(i + 2).Trim() = "" OrElse sDetals(i + 2).Trim() = "-}", "", sDetals(i + 2).Trim()))
                        End If
                        If i + 4 < sDetals.Length Then
                            rec.RemittanceInfo += (If(sDetals(i + 4).Trim().StartsWith(":") OrElse sDetals(i + 4).Trim() = "" OrElse sDetals(i + 4).Trim() = "-}", "", sDetals(i + 4).Trim()))
                        End If
                    End If
                    rec.SourceBIC = "NBETETAB"
                    rec.DestBIC = sOurBIC
                    rec.BeneficiaryAcc = rec.SourceBIC
                    rec.RemitterAcc = rec.SourceBIC
                    rec.RemitterName = rec.SourceBIC
                    rec.TranType = 6
                    rec.IsDebit = False
                ElseIf sMsghdr = sAllowed(6) Then
                    s = s.Trim()
                    If s.Contains(":20:") Then
                        rec.Reference = s.Remove(0, 4)
                    ElseIf s.Contains(":21:") Then
                        rec.RetCode = s.Remove(0, 4)
                    ElseIf s.Contains(":25:") Then
                        rec.BeneficiaryAcc = s.Remove(0, 4)
                    ElseIf s.StartsWith(":79:") Then
                        rec.RemittanceInfo = s.Substring(6)
                        If i + 2 < sDetals.Length Then
                            rec.RemittanceInfo += (If(sDetals(i + 2).Trim().StartsWith(":") OrElse sDetals(i + 2).Trim() = "" OrElse sDetals(i + 2).Trim() = "-}", "", sDetals(i + 2).Trim()))
                        End If
                        If i + 4 < sDetals.Length Then
                            rec.RemittanceInfo += (If(sDetals(i + 4).Trim().StartsWith(":") OrElse sDetals(i + 4).Trim() = "" OrElse sDetals(i + 4).Trim() = "-}", "", sDetals(i + 4).Trim()))
                        End If
                    End If
                    rec.Currency = "ETB"
                    rec.SourceBIC = "NBETETAB"
                    rec.DestBIC = sOurBIC
                    rec.RemitterAcc = rec.SourceBIC
                    rec.RemitterName = rec.SourceBIC
                    rec.TranType = 7
                    rec.IsDebit = True
                End If
            Next
            rec.BeneficiaryName = RegX.Replace(rec.BeneficiaryName, " ")
            rec.RemitterName = RegX.Replace(rec.RemitterName, " ")
            rec.BeneficiaryAcc = RegX.Replace(rec.BeneficiaryAcc, "")
            rec.RemitterAcc = RegX.Replace(rec.RemitterAcc, "")
            Return rec
        End Function
        Private Shared Sub ChequeTransaction(ByVal l As List(Of String), ByVal t As ChequeFormat)
            Select Case t
                Case ChequeFormat.SISPackage
                    For Each f As String In l
                        If Path.GetExtension(f) = ".chequeItem" Then
                            Dim sContent As String = ""
                            Using fs As New FileStream(f, FileMode.Open, FileAccess.Read)
                                Using sr As New StreamReader(fs)
                                    sContent = sr.ReadToEnd()
                                End Using
                            End Using
                            If sContent.Trim() <> "" Then
                                Dim fIndex As String = Path.GetFileNameWithoutExtension(f)
                                Dim inf As New DirectoryInfo(Path.GetDirectoryName(f))
                                Dim fImageFile As String = inf.GetFiles(fIndex & ".chequeFrontImage")(0).FullName
                                Dim bImageFile As String = inf.GetFiles(fIndex & ".chequeRearImage")(0).FullName
                                Dim chq As ETChequeDetails = GetSISDetails(sContent, File.ReadAllBytes(fImageFile), File.ReadAllBytes(bImageFile))
                                SaveCheque(chq)
                            End If
                        End If
                    Next
                Case ChequeFormat.XMLPackages
                    Dim di As New DirectoryInfo(Path.GetDirectoryName(l(0)))
                    Dim fi As FileInfo() = di.GetFiles("*.xml")
                    For Each f As FileInfo In fi
                        Dim xDoc As XDocument = XDocument.Load(f.FullName)
                        Dim xRoot As XElement = xDoc.Root
                        Dim xBulk As XElement = xRoot.Nodes().First()
                        Dim xHeader As XElement = xBulk.Nodes().First()
                        Dim xChqs As Array = xBulk.Nodes().Skip(1).ToArray()
                        For Each xChq As XElement In xChqs
                            Dim chq As New ETChequeDetails
                            chq.MsgId = DirectCast(xHeader.Nodes()(0), XElement).Value
                            chq.Amount = DirectCast(xChq.Nodes()(2), XElement).Value
                            chq.BankBIC = DirectCast(DirectCast(DirectCast(xChq.Nodes()(10), XElement).Nodes()(0), XElement).Nodes()(0),
                                        XElement).Value
                            Dim CrAgnt As XElement = DirectCast(xChq.Nodes()(7), XElement)
                            chq.BankCode = DirectCast(DirectCast(CrAgnt.Nodes(0), XElement).Nodes()(0), XElement).Value
                            chq.BranchCode = GetScalarREC("Select Top 1 BranchID From t_Branches Where BankID = '" & chq.BankCode & "'")
                            chq.BeneficiaryAcc = DirectCast(DirectCast(DirectCast(xChq.Nodes()(6), XElement).Nodes()(0), XElement).Nodes()(0),
                                        XElement).Value
                            chq.BeneficiaryName = DirectCast(DirectCast(xChq.Nodes()(5), XElement).Nodes()(0), XElement).Value
                            chq.OurBranch = DirectCast(DirectCast(xChq.Nodes()(4), XElement).Nodes()(4), XElement).Value.PadLeft(4, "0")
                            chq.ChequeIndex = 1
                            chq.ChequeNumber = DirectCast(DirectCast(xChq.Nodes()(4), XElement).Nodes()(0), XElement).Value
                            chq.CreationDate = DirectCast(xHeader.Nodes()(1), XElement).Value
                            chq.CurrencyCode = DirectCast(xChq.Nodes()(2), XElement).Attribute("Ccy").Value
                            chq.EndorsmentNo = DirectCast(DirectCast(xChq.Nodes()(0), XElement).Nodes()(1), XElement).Value
                            chq.FileName = f.Name
                            chq.MICRED = True
                            chq.RetCode = "00"
                            chq.RemitterAcc = DirectCast(DirectCast(DirectCast(xChq.Nodes()(9), XElement).Nodes()(0), XElement).Nodes()(0),
                                        XElement).Value
                            chq.RemitterName = DirectCast(DirectCast(xChq.Nodes()(8), XElement).Nodes()(0), XElement).Value
                            chq.TransCode = "CLG"
                            Dim MLine As String = DirectCast(DirectCast(xChq.Nodes()(4), XElement).Nodes()(2), XElement).Value
                            If MLine = "NO_MICROCODE" Then
                                MLine = chq.ChequeNumber & "/" & chq.BankCode & "/" & chq.BranchCode & "/0/" & chq.TransCode & "/99/" &
                                                    chq.RemitterAcc & "/" & chq.Amount
                            End If
                            chq.Codeline = MLine
                            chq.ValueDate = CDate(DirectCast(xHeader.Nodes()(4), XElement).Value)
                            Dim sPattern As String = "*" & chq.EndorsmentNo & "_"
                            chq.BackImageGS = File.ReadAllBytes(di.GetFiles(sPattern & "back.tif*")(0).FullName)
                            chq.FrontImageGS = File.ReadAllBytes(di.GetFiles(sPattern & "front.tif*")(0).FullName)
                            SaveCheque(chq)
                        Next
                    Next
            End Select
        End Sub
        Private Shared Sub BulkCheque(ByVal l As List(Of String), Optional ByVal OrigFilename As String = "")
            Dim di As New DirectoryInfo(Path.GetDirectoryName(l(0)))
            Dim fi As FileInfo() = di.GetFiles("*.xml")
            Dim ChqLen As Int16 = 0
            Dim ChequePrefix As String = ""
            Dim SystemType As String = ConfigurationManager.AppSettings("sysType")
            For Each f As FileInfo In fi
                Dim doc As New ch.Document()
                Dim ex As New Exception()
                If ch.Document.LoadFromFile(f.FullName, doc, ex) Then
                    Dim chq As New ETChequeDetails
                    chq.MsgId = doc.BlkChq.GrpHdr.MsgId
                    chq.ValueDate = doc.BlkChq.GrpHdr.IntrBkSttlmDt
                    chq.CreationDate = doc.BlkChq.GrpHdr.CreDtTm
                    For Each c As ch.ChequeType In doc.BlkChq.Chq
                        chq.Amount = c.IntrBkSttlmAmt.Value
                        chq.BankBIC = c.CdtrAgt.FinInstnId.BIC
                        If chq.BankCode = "" And Len(c.ChequeTx.BranchCode.Trim()) = 3 Then
                            Select Case SystemType.ToUpper.Trim
                                Case "BR"
                                    chq.BankCode = GetScalarREC("Select TOP 1 BankID From t_Banks Where SWIFTCODE LIKE '" & "%" & c.CdtrAgt.FinInstnId.BIC.Replace("0", "").Trim() & "%' ")
                                Case "BRNET"
                                    chq.BankCode = GetScalarREC("Select TOP 1 BankID From t_Bank Where SWIFTCODE LIKE '" & "%" & c.CdtrAgt.FinInstnId.BIC.Replace("0", "").Trim() & "%' ")
                            End Select
                        End If
                        chq.BranchCode = c.ChequeTx.BranchCode
                        chq.BeneficiaryAcc = c.CdtrAcct.Id.Item
                        chq.BeneficiaryName = c.Cdtr.Nm '.Replace("'", "").Trim()
                        chq.OurBranch = c.ChequeTx.BranchCode
                        chq.ChequeIndex = 1
                        ChqLen = GetScalarREC("Select ChequeIDLength From t_SystemBankSetting")
                        If IsNumeric(c.ChequeTx.ChkNmbr) = False Then
                            chq.ChequeNumber = Int64.Parse(Regex.Replace(c.ChequeTx.ChkNmbr, "[^\d]", ""))
                            ChequePrefix = String.Concat(c.ChequeTx.ChkNmbr.Where(AddressOf Char.IsLetter))
                        Else
                            chq.ChequeNumber = Int64.Parse(Regex.Replace(c.ChequeTx.ChkNmbr, "[^\d]", ""))
                            ChequePrefix = ""
                        End If

                        If chq.ChequeNumber.Length > ChqLen Then
                            chq.ChequeNumber = chq.ChequeNumber.Substring(0, ChqLen)
                        End If

                        chq.CurrencyCode = c.IntrBkSttlmAmt.Ccy.ToString()
                        chq.EndorsmentNo = c.PmtId.TxId
                        chq.FileName = OrigFilename 'f.Name
                        chq.MICRED = True
                        chq.RemitterAcc = c.DbtrAcct.Id.Item
                        chq.RemitterName = c.Dbtr.Nm '.Replace("'", "").Trim()
                        chq.TransCode = "CLG"
                        chq.trxID = c.PmtId.TxId
                        Dim MLine As String = c.ChequeTx.Microcode.Trim()

                        Dim splitMicr As String() = c.ChequeTx.Microcode.ToString.Trim().Split("/", 7, StringSplitOptions.RemoveEmptyEntries)

                        Dim splitLen As Int16 = splitMicr.Length

                        Try
                            chq.BankBIC = c.DbtrAgt.FinInstnId.BIC
                        Catch exUstrdColD As Exception
                            chq.BankBIC = ""
                        End Try

                        Try
                            If chq.CurrencyCode = "ETB" Then
                                chq.VoucherCode = "01" 'splitMicr(splitLen - 1)
                            ElseIf chq.CurrencyCode = "USD" Then
                                chq.VoucherCode = "60"
                            ElseIf chq.CurrencyCode = "GBP" Then
                                chq.VoucherCode = "61"
                            ElseIf chq.CurrencyCode = "EUR" Then
                                chq.VoucherCode = "62"
                            End If
                        Catch exUstrdColD As Exception
                            chq.VoucherCode = ""
                        End Try

                        Try
                            chq.ReqdColltnDt = IIf(IsDBNull(doc.BlkChq.GrpHdr.IntrBkSttlmDt.ToString("dd-MMM-yy")), "", doc.BlkChq.GrpHdr.IntrBkSttlmDt.ToString("dd-MMM-yy"))
                        Catch exReqdColltnDt As Exception
                            chq.ReqdColltnDt = ""
                        End Try
                        Try
                            chq.OrgnlMsgId = ""
                        Catch exUstrdColD As Exception
                            chq.OrgnlMsgId = ""
                        End Try
                        Try
                            chq.RetCode = "00"
                        Catch exUstrdColD As Exception
                            chq.RetCode = ""
                        End Try
                        Try
                            chq.UstrdColD = c.ChequeTx.Microcode
                        Catch exUstrdColD As Exception
                            chq.UstrdColD = ""
                        End Try
                        Try
                            chq.DAdrLine = ""
                        Catch exDAdrLine As Exception
                            chq.DAdrLine = ""
                        End Try
                        Try
                            chq.DTwnNm = ""
                        Catch exDTwnNm As Exception
                            chq.DTwnNm = ""
                        End Try
                        Try
                            chq.DCtry = ""
                        Catch exDCtry As Exception
                            chq.DCtry = ""
                        End Try
                        Try
                            chq.DNm = c.Dbtr.Nm
                        Catch exDNm As Exception
                            chq.DNm = ""
                        End Try
                        Try
                            chq.DPhneNb = ""
                        Catch exDPhneNb As Exception
                            chq.DPhneNb = ""
                        End Try
                        Try
                            chq.DMobNb = ""
                        Catch exDMobNb As Exception
                            chq.DMobNb = ""
                        End Try
                        Try
                            chq.DEmailAdr = ""
                        Catch exDEmailAdr As Exception
                            chq.DEmailAdr = ""
                        End Try
                        Try
                            chq.DOthr = ""
                        Catch exDOthr As Exception
                            chq.DOthr = ""
                        End Try
                        Try
                            chq.DbtrAcct = c.DbtrAcct.Id.Item
                        Catch exDbtrAcct As Exception
                            chq.DbtrAcct = ""
                        End Try
                        Try
                            chq.CAdrLine = ""
                        Catch exCAdrLine As Exception
                            chq.CAdrLine = ""
                        End Try
                        Try
                            chq.CTwnNm = ""
                        Catch exCTwnNm As Exception
                            chq.CTwnNm = ""
                        End Try
                        Try
                            chq.CCtry = ""
                        Catch exCCtry As Exception
                            chq.CCtry = ""
                        End Try
                        Try
                            chq.CNm = c.Cdtr.Nm
                        Catch exCNm As Exception
                            chq.CNm = ""
                        End Try
                        Try
                            chq.CPhneNb = ""
                        Catch exCPhneNb As Exception
                            chq.CPhneNb = ""
                        End Try
                        Try
                            chq.CMobNb = ""
                        Catch exCMobNb As Exception
                            chq.CMobNb = ""
                        End Try
                        Try
                            chq.CEmailAdr = ""
                        Catch exCEmailAdr As Exception
                            chq.CEmailAdr = ""
                        End Try
                        Try
                            chq.COthr = ""
                        Catch exCOthr As Exception
                            chq.COthr = ""
                        End Try
                        Try
                            chq.PymType = c.PmtTpInf.SvcLvl.Item.ToString()
                        Catch exPymType As Exception
                            chq.PymType = ""
                        End Try
                        Try
                            chq.CtgyPurp = c.PmtTpInf.CtgyPurp.Item.ToString()
                        Catch exCtgyPurp As Exception
                            chq.CtgyPurp = ""
                        End Try
                        Try
                            chq.LclInstrm = c.PmtTpInf.LclInstrm.Item.ToString()
                        Catch exLclInstrm As Exception
                            chq.LclInstrm = ""
                        End Try
                        Try
                            chq.CdtrAcct = c.CdtrAcct.Id.Item.ToString()
                        Catch exCdtrAcct As Exception
                            chq.CdtrAcct = ""
                        End Try
                        Try
                            chq.OrgnlInstrID = ""
                        Catch exOrgnlInstrID As Exception
                            chq.OrgnlInstrID = ""
                        End Try
                        Try
                            chq.OrgnlTxId = ""
                        Catch exOrgnlInstrID As Exception
                            chq.OrgnlTxId = ""
                        End Try
                        Try
                            chq.EndToEndId = c.PmtId.EndToEndId
                        Catch exEndToEndId As Exception
                            chq.EndToEndId = ""
                        End Try
                        Try
                            chq.OrgnlEndToEnd = c.PmtId.EndToEndId
                        Catch exOrgnlEndToEnd As Exception
                            chq.OrgnlEndToEnd = ""
                        End Try
                        Try
                            chq.SourceBIC = c.CdtrAgt.FinInstnId.BIC.ToString()
                        Catch exSourceBIC As Exception
                            chq.SourceBIC = ""
                        End Try
                        Try
                            chq.IntrBkSttlmDt = IIf(IsDBNull(doc.BlkChq.GrpHdr.IntrBkSttlmDt.ToString("dd-MMM-yy")), "", doc.BlkChq.GrpHdr.IntrBkSttlmDt.ToString("dd-MMM-yy"))
                        Catch exIntrBkSttlmDt As Exception
                            chq.IntrBkSttlmDt = ""
                        End Try
                        Try
                            chq.RemittanceInfo = ""
                        Catch exRemittanceInfo As Exception
                            chq.RemittanceInfo = ""
                        End Try
                        chq.TrxData = chq.MsgId & ":" & chq.trxID







                        'If chq.VoucherCode Then
                        If MLine = "NO_MICROCODE" Then
                            MLine = c.ChequeTx.ChkNmbr & "/" & chq.BankCode & "/" & chq.BranchCode & "/0/" & c.PmtId.EndToEndId & "/99/" &
                        chq.RemitterAcc & "/" & chq.Amount & "/" & Format(chq.ValueDate, "dd-MMM-yyyy") & "/" & c.PmtId.TxId

                        End If
                        chq.Codeline = MLine
                        Dim sPattern As String = "*" & chq.EndorsmentNo & "*."
                        Dim arrImages As New ArrayList
                        '----front gray scale

                        sPattern = Replace(sPattern, " ", "")
                        'chq.FrontImageGS = File.ReadAllBytes(f.First(Function(p) (p.Name.Contains(sPattern) Or p.Name.Contains(sPattern2)) &
                        'AndAlso (p.Name.Contains("front.jpg") Or p.Name.Contains("front.tif") Or p.Name.Contains("front.jpeg") &
                        'Or p.Name.Contains("front.tiff"))).FullName)

                        'arrImages.AddRange(From p In fi
                        '                   Where (p.Name.Contains(sPattern) AndAlso (p.Name.Contains("front.jpg") Or p.Name.Contains("front.tif") Or p.Name.Contains("front.jpeg") Or p.Name.Contains("front.tiff")))
                        '                   Select p.FullName)


                        'If arrImages.Count > 0 Then
                        '    chq.FrontImageGS = File.ReadAllBytes(arrImages(0).ToString)
                        'End If

                        arrImages.AddRange(IO.Directory.GetFiles(Path.GetDirectoryName(l(0)), sPattern & ".front.tif*"))
                        If arrImages.Count = 0 Then
                            arrImages.AddRange(IO.Directory.GetFiles(Path.GetDirectoryName(l(0)), chq.EndorsmentNo & "front.tif*"))
                            If arrImages.Count = 0 Then
                                arrImages.AddRange(IO.Directory.GetFiles(Path.GetDirectoryName(l(0)), chq.EndorsmentNo & "_front.tiff*"))
                                If arrImages.Count = 0 Then
                                    arrImages.AddRange(IO.Directory.GetFiles(Path.GetDirectoryName(l(0)), chq.EndorsmentNo & "front.jpg"))
                                    If arrImages.Count = 0 Then
                                        arrImages.AddRange(IO.Directory.GetFiles(Path.GetDirectoryName(l(0)), chq.EndorsmentNo & "front.jpeg"))
                                        If arrImages.Count = 0 Then
                                            arrImages.AddRange(IO.Directory.GetFiles(Path.GetDirectoryName(l(0)), chq.EndorsmentNo & "_front.jpg"))
                                            If arrImages.Count = 0 Then
                                                arrImages.AddRange(IO.Directory.GetFiles(Path.GetDirectoryName(l(0)), chq.EndorsmentNo & "_front.jpeg"))
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                        If arrImages.Count > 0 Then
                            chq.FrontImageGS = File.ReadAllBytes(arrImages(0).ToString)
                        End If
                        '----front BW
                        'arrImages = New ArrayList
                        'arrImages.AddRange(IO.Directory.GetFiles(Path.GetDirectoryName(l(0)), sPattern & ".BW.tif*"))
                        'If arrImages.Count = 0 Then
                        '    arrImages.AddRange(IO.Directory.GetFiles(Path.GetDirectoryName(l(0)), chq.EndorsmentNo & ".BW.tif*"))
                        '    If arrImages.Count = 0 Then
                        '        arrImages.AddRange(IO.Directory.GetFiles(Path.GetDirectoryName(l(0)), chq.EndorsmentNo & ".BW.tiff*"))
                        '    End If
                        'End If
                        'If arrImages.Count > 0 Then
                        '    chq.FrontImageBW = File.ReadAllBytes(arrImages(0).ToString)
                        'End If

                        chq.FrontImageBW = Nothing
                        '------Back gray scale Image
                        arrImages = New ArrayList
                        'back grayscale image
                        arrImages.AddRange(IO.Directory.GetFiles(Path.GetDirectoryName(l(0)), sPattern & ".back.tif*"))
                        If arrImages.Count = 0 Then
                            arrImages.AddRange(IO.Directory.GetFiles(Path.GetDirectoryName(l(0)), chq.EndorsmentNo & "back.tif*"))
                            If arrImages.Count = 0 Then
                                arrImages.AddRange(IO.Directory.GetFiles(Path.GetDirectoryName(l(0)), chq.EndorsmentNo & "_back.tiff*"))
                                If arrImages.Count = 0 Then
                                    arrImages.AddRange(IO.Directory.GetFiles(Path.GetDirectoryName(l(0)), chq.EndorsmentNo & "back.jpg"))
                                    If arrImages.Count = 0 Then
                                        arrImages.AddRange(IO.Directory.GetFiles(Path.GetDirectoryName(l(0)), chq.EndorsmentNo & "back.jpeg"))
                                        If arrImages.Count = 0 Then
                                            arrImages.AddRange(IO.Directory.GetFiles(Path.GetDirectoryName(l(0)), chq.EndorsmentNo & "_back.jpg"))
                                            If arrImages.Count = 0 Then
                                                arrImages.AddRange(IO.Directory.GetFiles(Path.GetDirectoryName(l(0)), chq.EndorsmentNo & "_back.jpeg"))
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                        If arrImages.Count > 0 Then
                            chq.BackImageGS = File.ReadAllBytes(arrImages(0).ToString)
                        End If
                        'Uv image
                        'arrImages = New ArrayList
                        'arrImages.AddRange(IO.Directory.GetFiles(Path.GetDirectoryName(l(0)), sPattern & ".UV.tif*"))
                        'If arrImages.Count = 0 Then
                        '    arrImages.AddRange(IO.Directory.GetFiles(Path.GetDirectoryName(l(0)), chq.EndorsmentNo & ".UV.tif*"))
                        '    If arrImages.Count = 0 Then
                        '        arrImages.AddRange(IO.Directory.GetFiles(Path.GetDirectoryName(l(0)), chq.EndorsmentNo & ".UV.tiff*"))
                        '    End If
                        'End If
                        'If arrImages.Count > 0 Then
                        '    chq.FrontImageUV = File.ReadAllBytes(arrImages(0).ToString)
                        'End If
                        chq.FrontImageUV = Nothing
                        SaveCheque(chq)
                    Next
                End If
            Next
        End Sub
        Private Shared Function GetSISDetails(ByVal sDetail As String, ByVal sFront As Byte(), ByVal sRear As Byte()) As ETChequeDetails
            Dim Details As String() = sDetail.Split(vbLf)
            Dim chq As New ETChequeDetails
            chq.Amount = Details(10).Split("=")(1).Trim()
            chq.BackImageGS = sRear
            chq.BankCode = Details(9).Split("=")(1).Trim()
            chq.BeneficiaryAcc = Details(5).Split("=")(1).Trim()
            chq.BeneficiaryName = Details(15).Split("=")(1).Trim().Replace("'", "")
            chq.BranchCode = Details(3).Split("=")(1).Trim()
            chq.ChequeIndex = Details(7).Split("=")(1).Trim()
            chq.ChequeNumber = Details(6).Split("=")(1).Trim()
            chq.Codeline = Details(11).Split("=")(1).Trim()
            chq.CreationDate = Details(4).Split("=")(1).Trim()
            chq.CurrencyCode = Details(14).Split("=")(1).Trim()
            chq.EndorsmentNo = Details(13).Split("=")(1).Trim()
            chq.FileName = Details(16).Split("=")(1).Trim()
            chq.FrontImageGS = sFront
            chq.RemittanceInfo = Details(8).Split("=")(1).Trim().Replace("'", "")
            chq.RemitterAcc = Details(17).Split("=")(1).Trim()
            chq.RemitterName = Details(12).Split("=")(1).Trim().Replace("'", "")
            chq.TransCode = Details(2).Split("=")(1).Trim()
            chq.EndorsmentNo = IIf(chq.EndorsmentNo = "", Modscan.GetNextInt16, chq.EndorsmentNo)
            chq.CurrencyCode = IIf(chq.CurrencyCode = "1", "ETB", chq.CurrencyCode)
            Dim MLine As String = chq.ChequeNumber & "/" & chq.BankCode & "/" & chq.BranchCode & "/0/" & chq.TransCode & "/99/" & chq.RemitterAcc &
        "/" & chq.Amount
            chq.Codeline = IIf(chq.Codeline = "NO_MICROCODE" Or chq.Codeline = "", MLine, chq.Codeline)
            GetSISDetails = chq
        End Function
        Private Shared Function RejectedItems(ByVal sFile As String) As Boolean
            Dim IsReject As Boolean = False
            Dim RegX As New Regex("[^A-Za-z0-9]")
            Dim sTempFile As String = Path.Combine(TempLocation, sFile)
            Sign = True
            sFile = Path.Combine(strFileLocation, sFile)
            If Sign Then ReadFile(sFile, sTempFile)
            Dim doc As New res.Document()
            Dim ex As New Exception()
            If res.Document.LoadFromFile(sFile, doc, ex) Then
                IsReject = True
                Dim OrgMsgId As String = doc.FIToFIPmtStsRpt.OrgnlGrpInfAndSts.OrgnlMsgNmId
                For Each txn As res.PaymentTransactionInformation26 In doc.FIToFIPmtStsRpt.TxInfAndSts
                    Dim ch As New ETChequeDetails
                    ch.BeneficiaryAcc = RegX.Replace(txn.OrgnlTxRef.CdtrAcct.Id.Item, String.Empty)
                    ch.BeneficiaryName = RegX.Replace(txn.OrgnlTxRef.Cdtr.Nm, " ")
                    ch.ChequeNumber = txn.OrgnlEndToEndId
                    ch.FileName = Path.GetFileName(sFile)
                    ch.RemitterAcc = RegX.Replace(txn.OrgnlTxRef.DbtrAcct.Id.Item, String.Empty)
                    ch.RemitterName = RegX.Replace(txn.OrgnlTxRef.Dbtr.Nm, " ")
                    ch.trxID = txn.OrgnlTxId.ToString
                    ch.MsgId = doc.FIToFIPmtStsRpt.GrpHdr.MsgId
                    ch.Amount = CDec(txn.OrgnlTxRef.IntrBkSttlmAmt.Value)
                    ch.CurrencyCode = txn.OrgnlTxRef.IntrBkSttlmAmt.Ccy
                    Try
                        ch.BankBIC = txn.OrgnlTxRef.DbtrAgt.FinInstnId.BIC
                    Catch exUstrdColD As Exception
                        ch.BankBIC = ""
                    End Try

                    Try
                        ch.VoucherCode = ""
                    Catch exUstrdColD As Exception
                        ch.VoucherCode = ""
                    End Try
                    Try
                        ch.OrgnlMsgId = doc.FIToFIPmtStsRpt.OrgnlGrpInfAndSts.OrgnlMsgId
                    Catch exUstrdColD As Exception
                        ch.OrgnlMsgId = ""
                    End Try
                    Try
                        ch.RetCode = txn.StsRsnInf.Rsn.Item(0).ToString()
                    Catch exUstrdColD As Exception
                        ch.RetCode = ""
                    End Try
                    Try
                        ch.UstrdColD = ""
                    Catch exUstrdColD As Exception
                        ch.UstrdColD = ""
                    End Try
                    Try
                        ch.DAdrLine = ""
                    Catch exDAdrLine As Exception
                        ch.DAdrLine = ""
                    End Try
                    Try
                        ch.DTwnNm = ""
                    Catch exDTwnNm As Exception
                        ch.DTwnNm = ""
                    End Try
                    Try
                        ch.DCtry = ""
                    Catch exDCtry As Exception
                        ch.DCtry = ""
                    End Try
                    Try
                        ch.DNm = RegX.Replace(txn.OrgnlTxRef.Dbtr.Nm, " ")
                    Catch exDNm As Exception
                        ch.DNm = ""
                    End Try
                    Try
                        ch.DPhneNb = ""
                    Catch exDPhneNb As Exception
                        ch.DPhneNb = ""
                    End Try
                    Try
                        ch.DMobNb = ""
                    Catch exDMobNb As Exception
                        ch.DMobNb = ""
                    End Try
                    Try
                        ch.DEmailAdr = ""
                    Catch exDEmailAdr As Exception
                        ch.DEmailAdr = ""
                    End Try
                    Try
                        ch.DOthr = ""
                    Catch exDOthr As Exception
                        ch.DOthr = ""
                    End Try
                    Try
                        ch.DbtrAcct = RegX.Replace(txn.OrgnlTxRef.DbtrAcct.Id.Item, String.Empty)
                    Catch exDbtrAcct As Exception
                        ch.DbtrAcct = ""
                    End Try
                    Try
                        ch.CAdrLine = ""
                    Catch exCAdrLine As Exception
                        ch.CAdrLine = ""
                    End Try
                    Try
                        ch.CTwnNm = ""
                    Catch exCTwnNm As Exception
                        ch.CTwnNm = ""
                    End Try
                    Try
                        ch.CCtry = ""
                    Catch exCCtry As Exception
                        ch.CCtry = ""
                    End Try
                    Try
                        ch.CNm = RegX.Replace(txn.OrgnlTxRef.Cdtr.Nm, " ")
                    Catch exCNm As Exception
                        ch.CNm = ""
                    End Try
                    Try
                        ch.CPhneNb = ""
                    Catch exCPhneNb As Exception
                        ch.CPhneNb = ""
                    End Try
                    Try
                        ch.CMobNb = ""
                    Catch exCMobNb As Exception
                        ch.CMobNb = ""
                    End Try
                    Try
                        ch.CEmailAdr = ""
                    Catch exCEmailAdr As Exception
                        ch.CEmailAdr = ""
                    End Try
                    Try
                        ch.COthr = ""
                    Catch exCOthr As Exception
                        ch.COthr = ""
                    End Try
                    Try
                        ch.PymType = ""
                    Catch exPymType As Exception
                        ch.PymType = ""
                    End Try
                    Try
                        ch.CdtrAcct = RegX.Replace(txn.OrgnlTxRef.CdtrAcct.Id.Item, String.Empty) '.Replace("'", "")
                    Catch exCdtrAcct As Exception
                        ch.CdtrAcct = ""
                    End Try
                    Try
                        ch.OrgnlInstrID = IIf(txn.OrgnlInstrId.ToString = "", "", txn.OrgnlInstrId)
                    Catch exOrgnlInstrID As Exception
                        ch.OrgnlInstrID = ""
                    End Try
                    Try
                        ch.OrgnlTxId = txn.OrgnlTxId.ToString
                    Catch exOrgnlInstrID As Exception
                        ch.OrgnlTxId = ""
                    End Try

                    Try
                        ch.OrgnlEndToEnd = txn.OrgnlEndToEndId
                    Catch exOrgnlEndToEnd As Exception
                        ch.OrgnlEndToEnd = ""
                    End Try
                    Try
                        ch.IntrBkSttlmDt = txn.OrgnlTxRef.IntrBkSttlmDtt
                    Catch exIntrBkSttlmDt As Exception
                        ch.IntrBkSttlmDt = ""
                    End Try
                    ch.TrxData = ch.OrgnlTxId & ":" & ch.MsgId
                    SaveCheque(ch)
                Next
            End If
            'If Sign Then File.Delete(sTempFile)
            'If (File.Exists(arch)) Then sArchiveFile = sArchiveFile & Now.ToString("yyyyMMddHHmmss")
            'If File.Exists(sFile) Then File.Move(sFile, sArchiveFile)

            Return IsReject
        End Function
        Private Shared Function RejectedEFTs(ByVal sFile As String) As Boolean
            Dim IsReject As Boolean = False
            Dim RegX As New Regex("[^A-Za-z0-9]")
            Dim sTempFile As String = Path.Combine(TempLocation, sFile)
            Sign = True
            sFile = Path.Combine(strFileLocation, sFile)
            If Sign Then ReadFile(sFile, sTempFile)
            Dim doc As New pc412.Document ' pcr.Document()
            Dim ex As New Exception()
            If pc412.Document.LoadFromFile(sFile, doc, ex) Then
                IsReject = True
                Dim OrgMsgId As String = doc.PmtRtr.GrpHdr.MsgId
                For Each txn As pc412.PaymentTransactionInformation27 In doc.PmtRtr.TxInf
                    If OrgMsgId.StartsWith("pacs.005") Or OrgMsgId.StartsWith("pacs.002") Then
                        Dim ch As New ETChequeDetails
                        'ch.Amount = CDec(txn.OrgnlTxRef.IntrBkSttlmAmt.Value)
                        'ch.BankBIC = txn.OrgnlTxRef.DbtrAgt.FinInstnId.BIC
                        'ch.BeneficiaryAcc = RegX.Replace(txn.OrgnlTxRef.CdtrAcct.Id.Item, String.Empty) '.Replace("'", "")
                        'ch.BeneficiaryName = RegX.Replace(txn.OrgnlTxRef.Cdtr.Nm, " ") '.Replace("'", "")
                        'ch.ChequeNumber = txn.OrgnlTxId
                        'ch.Codeline = txn.OrgnlTxRef.ChequeTx.Microcode
                        'ch.CurrencyCode = txn.OrgnlTxRef.IntrBkSttlmAmt.Ccy
                        'ch.EndorsmentNo = txn.OrgnlEndToEndId
                        'ch.RemittanceInfo = txn.OrgnlInstrId
                        ''If ch.RemittanceInfo Is Nothing Then ch.ChequeNumber = txn.OrgnlEndToEndId
                        'ch.FileName = Path.GetFileName(sFile)
                        'ch.MsgId = doc.FIToFIPmtStsRpt.OrgnlGrpInfAndSts.OrgnlMsgId
                        'ch.RemitterAcc = RegX.Replace(txn.OrgnlTxRef.DbtrAcct.Id.Item, String.Empty) '.Replace("'", "")
                        'ch.RemitterName = RegX.Replace(txn.OrgnlTxRef.Dbtr.Nm, " ") '.Replace("'", "")
                        'ch.RetCode = txn.StsRsnInf.Rsn.Item
                        'ch.trxID = txn.StsId.ToString
                        'If (ch.RetCode Is Nothing) Or Len(ch.RetCode) > 2 Then
                        '    ch.RetCode = "AC01"
                        'End If
                        ch.ValueDate = txn.OrgnlTxRef.IntrBkSttlmDt
                        UnpayCheque(ch)
                        SaveCheque(ch)
                    ElseIf OrgMsgId.StartsWith("pacs.003") Or OrgMsgId.StartsWith("pacs.004") Then
                        Dim d As New ETEFTDetails
                        d.IsDebit = False
                        d.Amount = CDec(txn.RtrdIntrBkSttlmAmt.Value)
                        d.Currency = txn.OrgnlIntrBkSttlmAmt.Ccy.ToString()
                        d.EFTID = txn.OrgnlTxId
                        d.MsgId = txn.OrgnlGrpInf.OrgnlMsgId
                        d.Reference = txn.OrgnlEndToEndId
                        d.RetCode = txn.RtrRsnInf(0).Rsn.Item
                        d.TranType = 0
                        d.ValueDate = txn.OrgnlTxRef.IntrBkSttlmDt
                        'UnpayEft(d)
                        'SaveEFT(d, OrgMsgId)
                    End If
                Next
            End If
            If Sign Then File.Delete(sTempFile)
            Return IsReject
        End Function
        Private Shared Sub UnpayCheque(ByRef ch As ETChequeDetails)
            Try
                Dim dtUnpaid As New DataTable()
                Modscan.ExecuteData(Modscan.GetModify("p_GetClearingData", "RefID", ch.MsgId, "ChequeID", ch.EndorsmentNo), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.SelectStatement, Modscan.publicDset)
                If Modscan.publicDTbl.Rows.Count > 0 Then
                    If Modscan.publicDTbl.Rows(0)("ColumnID").ToString() = "77777777" Then
                        ch.OurBranch = Modscan.publicDTbl.Rows(0)("OurBranchID").ToString()
                        ch.BankCode = Modscan.publicDTbl.Rows(0)("BankID").ToString()
                        ch.BranchCode = Modscan.publicDTbl.Rows(0)("BranchID").ToString()
                        ch.Codeline = Modscan.publicDTbl.Rows(0)("MicrLine").ToString()
                        ch.VoucherCode = "11"
                        ch.CurrencyCode = Modscan.publicDTbl.Rows(0)("CurrencyCode").ToString()
                    Else
                        ch.OurBranch = Modscan.publicDTbl.Rows(0)("OurBranchID").ToString()
                        ch.BackImageGS = If(IsDBNull(Modscan.publicDTbl.Rows(0)("JRImage").ToString), Modscan.publicDTbl.Rows(0)("JRImage"), Nothing)
                        ch.FrontImageGS = If(IsDBNull(Modscan.publicDTbl.Rows(0)("JFImage").ToString), Modscan.publicDTbl.Rows(0)("JFImage"), Nothing)
                        ch.FrontImageBW = If(IsDBNull(Modscan.publicDTbl.Rows(0)("TFImage").ToString), Modscan.publicDTbl.Rows(0)("TFImage"), Nothing)
                        ch.FrontImageUV = If(IsDBNull(Modscan.publicDTbl.Rows(0)("TFImage").ToString), Modscan.publicDTbl.Rows(0)("UVImage"), Nothing)
                        ch.BankCode = Modscan.publicDTbl.Rows(0)("BankID").ToString()
                        ch.BranchCode = Modscan.publicDTbl.Rows(0)("BranchID").ToString()
                        ch.RemitterAcc = Modscan.publicDTbl.Rows(0)("TheirAccountID").ToString()
                        ch.BeneficiaryAcc = Modscan.publicDTbl.Rows(0)("AccountID").ToString()
                        ch.Codeline = Modscan.publicDTbl.Rows(0)("MicrLine").ToString()
                        ch.CurrencyCode = Modscan.publicDTbl.Rows(0)("CurrencyCode").ToString()
                        ch.EndorsmentNo = Modscan.publicDTbl.Rows(0)("ColumnID").ToString()
                        ch.ValueDate = CDate(Modscan.publicDTbl.Rows(0)("ChequeDate"))
                        ch.ChequeNumber = Modscan.publicDTbl.Rows(0)("ChequeID").ToString()
                        ch.Orgid = Modscan.publicDTbl.Rows(0)("OrigOurBranchID").ToString()
                        ch.VoucherCode = Modscan.publicDTbl.Rows(0)("VoucherCode").ToString()

                    End If
                Else
                    Modscan.ExecuteData(Modscan.GetModify("p_GetClearingData", "RefID", ch.EndorsmentNo, "ChequeID", ch.Codeline), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.SelectStatement, Modscan.publicDset)
                    For Each r As DataRow In dtUnpaid.Rows
                        Dim MLine As String = r("MicrLine").ToString()
                        If ch.ChequeNumber.Contains(MLine.Trim().Split("/")(0)) Then
                            ch.OurBranch = r("OurBranchID").ToString()
                            ch.BackImageGS = r("JRImage")
                            ch.FrontImageGS = r("JFImage")
                            ch.FrontImageBW = r("TFImage")
                            ch.FrontImageUV = r("UVImage")
                            ch.BankCode = r("BankID").ToString()
                            ch.BranchCode = r("BranchID").ToString()
                            ch.RemitterAcc = r("TheirAccount").ToString()
                            ch.BeneficiaryAcc = r("AccountID").ToString()
                            ch.Codeline = r("MicrLine").ToString()
                            ch.CurrencyCode = r("CurrencyCode").ToString()
                            ch.EndorsmentNo = dtUnpaid.Rows(0)("ColumnID").ToString()
                            ch.ValueDate = CDate(r("ChequeDate"))
                            ch.ChequeNumber = r("ChequeID").ToString()
                            Exit For
                        End If
                    Next
                End If
            Catch ex As Exception

            End Try
        End Sub
        Private Sub UnpayEft(ByRef d As ETEFTDetails)
            'Try
            '    Dim dtUnpaid As New DataTable()
            '    strAction = "SELECT AMOUNT, BANK, BRANCH, TOBRANCH, ACC_DR, ACC_CR, BFNAME, ORGREF, REMITTERNAME, TOBANK, CURRENCYCODE, REF_NO, " & _
            '    "ORIGINATORCODE, EFTID, TRANTYPE FROM EFTSI WHERE EFTID = '" & d.EFTID & "'"
            '    ExecuteData(strAction, dtUnpaid, dataExecTypes.ExecTypeQuery, dbConnectionTypes.dbConnTypeArchive)
            '    If dtUnpaid.Rows.Count > 0 Then
            '        d.Reference = dtUnpaid.Rows(0)("ORGREF").ToString()
            '        d.BeneficiaryAcc = dtUnpaid.Rows(0)("ACC_CR").ToString()
            '        d.RemitterAcc = dtUnpaid.Rows(0)("ACC_DR").ToString()
            '        d.BeneficiaryName = dtUnpaid.Rows(0)("BFNAME").ToString()
            '        d.RemitterName = dtUnpaid.Rows(0)("REMITTERNAME").ToString()
            '        d.Amount = dtUnpaid.Rows(0)("AMOUNT").ToString()
            '        d.DestBIC = dtUnpaid.Rows(0)("TOBANK").ToString()
            '        d.DestBIC = Left(FindRecord("BANKS", "SWIFTCODE", "CODE = '" & d.DestBIC & "'"), 8)
            '        d.SourceBIC = dtUnpaid.Rows(0)("BANK").ToString()
            '        d.SourceBIC = Left(FindRecord("BANKS", "SWIFTCODE", "CODE = '" & d.SourceBIC & "'"), 8)
            '    Else
            '        ExecuteData(strAction, dtUnpaid, dataExecTypes.ExecTypeQuery, dbConnectionTypes.dbConnTypeLive)
            '        If dtUnpaid.Rows.Count > 0 Then
            '            d.Reference = dtUnpaid.Rows(0)("ORGREF").ToString()
            '            d.BeneficiaryAcc = dtUnpaid.Rows(0)("ACC_CR").ToString()
            '            d.RemitterAcc = dtUnpaid.Rows(0)("ACC_DR").ToString()
            '            d.BeneficiaryName = dtUnpaid.Rows(0)("BFNAME").ToString()
            '            d.RemitterName = dtUnpaid.Rows(0)("REMITTERNAME").ToString()
            '            d.Amount = dtUnpaid.Rows(0)("AMOUNT").ToString()
            '            d.DestBIC = dtUnpaid.Rows(0)("TOBANK").ToString()
            '            d.DestBIC = Left(FindRecord("BANKS", "SWIFTCODE", "CODE = '" & d.DestBIC & "'"), 8)
            '            d.SourceBIC = dtUnpaid.Rows(0)("BANK").ToString()
            '            d.SourceBIC = Left(FindRecord("BANKS", "SWIFTCODE", "CODE = '" & d.SourceBIC & "'"), 8)
            '        End If
            '    End If
            'Catch ex As Exception

            'End Try
        End Sub
        Private Shared Function GetScalarREC(ByVal strStatementORstrProcedure As String, Optional ByVal strArr As String = "") As String
            Dim strResults As String = ""
            Try
                If Modscan.GetModify(strStatementORstrProcedure, strArr) = "" Then
                    Modscan.ExecuteData(strStatementORstrProcedure, Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.SelectStatement, Nothing)
                Else
                    Modscan.ExecuteData(Modscan.GetModify(strStatementORstrProcedure, strArr), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.SelectStatement, Nothing)
                End If
                If Modscan.publicDTbl.Rows.Count >= 1 Then
                    strResults = Modscan.publicDTbl(0)(0)
                Else
                    strResults = Nothing
                End If
            Catch ex As Exception
                strResults = Nothing
            End Try
            Modscan.publicDTbl.Clear()
            Return strResults
        End Function
        Private Shared Function Getdata(ByVal strStatementORstrProcedure As String, ByVal ProcTrueORStstFalse As Boolean, ByVal ParamArray strArr() As Object) As DataTable
            Dim dt As DataTable = Nothing
            Try
                If ProcTrueORStstFalse = True Then
                    Modscan.ExecuteData(Modscan.GetModify(strStatementORstrProcedure, strArr), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure, Nothing)
                Else
                    Modscan.ExecuteData(Modscan.GetModify(strStatementORstrProcedure, strArr), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.SelectStatement, Nothing)
                End If
                If Modscan.publicDTbl.Rows.Count >= 1 Then
                    dt = Modscan.publicDTbl
                Else
                    dt = Nothing
                End If
            Catch ex As Exception
                dt = Nothing
            End Try
            Return dt
        End Function
        Private Shared Function RecExists(ByVal strStatement As String) As Boolean
            Dim ItExist As Boolean = False
            Try
                Modscan.ExecuteData(strStatement, Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.SelectStatement, Nothing)
                If Modscan.publicDTbl.Rows.Count >= 1 Then
                    ItExist = True
                Else
                    ItExist = False
                End If
            Catch ex As Exception
                ItExist = False
            End Try
            Return ItExist
        End Function
        Private Shared Sub SaveCheque(ByVal chq As ETChequeDetails)

            Try
                Dim RegX As New Regex("[^A-Za-z0-9]")
                Dim strArr As String = ""
                Dim LineItemsTable As Hashtable = New Hashtable
                Dim MethodDt As DataTable = New DataTable
                Dim SystemType As String = ConfigurationManager.AppSettings("sysType")


                'chq.RetCode = "00"


                If chq.RetCode = "00" Then chq.ValueDate = Modscan.WORKING_DATE

                Dim ProcNo As String = Modscan.GetNextInt16 & Modscan.GetNextString
                '------------------------------------------------------------------------------------------------------
                LineItemsTable.Add("RCODE", chq.RetCode) ' RCODE
                LineItemsTable.Add("VTYPE", chq.VoucherCode) ' Voucher Type
                LineItemsTable.Add("AMOUNT", (Val(chq.Amount) / 1).ToString) ' Amount
                LineItemsTable.Add("ENTRYMODE", "0") ' Amount Entry Mode
                LineItemsTable.Add("CURRENCYCODE", chq.CurrencyCode) ' Amount Entry Mode
                'LineItemsTable.Add("DESTBANK", Modscan.OurBankID) ' Dest Bank
                LineItemsTable.Add("DESTACC", chq.RemitterAcc) ' Dest Account
                LineItemsTable.Add("COLLACC", chq.BeneficiaryAcc) 'Collecting Account Details
                LineItemsTable.Add("DESTBRANCH", "") ' Dest Branch


                'LineItemsTable.Add("CHQDGT", chq.ChequeIndex) ' Check Digit
                ' LineItemsTable.Add("PBANK", chq.BankCode) ' PBank
                LineItemsTable.Add("PBRANCH", "") ' PBranch
                LineItemsTable.Add("FILLER", "0") ' Filler
                LineItemsTable.Add("DESTBANK", chq.BankBIC) ' Dest Bank
                LineItemsTable.Add("PBANK", chq.SourceBIC) ' PBank
                LineItemsTable.Add("DRAWERORPAYEE", chq.RemitterName) 'Collecting Account Details
                LineItemsTable.Add("SNO", chq.ChequeNumber) ' Serial Number
                LineItemsTable.Add("PROCNO", ProcNo) ' Processing Number
                LineItemsTable.Add("DRN", chq.BeneficiaryName) ' Processing Number
                LineItemsTable.Add("DATA", "/" + chq.ChequeNumber + "/" + Modscan.OurBankID + chq.BranchCode + "/" + chq.RemitterAcc + "/" + chq.MsgId + "/") 'chq.Codeline & "-" & chq.MsgId) ' The Whole String as is
                LineItemsTable.Add("FIMAGESIZEBW", 0)
                LineItemsTable.Add("FIMAGESIGNBW", 0)
                LineItemsTable.Add("FIMAGESIZE", 0)
                LineItemsTable.Add("FIMAGESIGN", 0)
                LineItemsTable.Add("BIMAGESIZE", 0)
                LineItemsTable.Add("BIMAGESIGN", 0) 'myCol.Item(Item).ToString.Substring(197, 48)) ' back tiff image signature

                LineItemsTable.Add("FrontBWImage", "") 'chq.FrontImageBW)
                LineItemsTable.Add("FrontGrayScaleImage", chq.FrontImageGS)
                LineItemsTable.Add("RearImage", chq.BackImageGS)
                LineItemsTable.Add("UVImage", "") ' chq.FrontImageUV)


                LineItemsTable.Add("FILENAME", chq.FileName) ' The Filename
                LineItemsTable.Add("ValidInvalid", True) 'Validity of the image
                LineItemsTable.Add("IsFCY", False)

                LineItemsTable.Add("MsgID", chq.MsgId)
                LineItemsTable.Add("TrxID", chq.trxID)
                LineItemsTable.Add("UstrdBWF", "") 'chq.UstrdBWF)
                LineItemsTable.Add("UstrdBWR", "") ' chq.UstrdBWR)
                LineItemsTable.Add("UstrdGS", "") 'chq.UstrdGS)
                LineItemsTable.Add("UstrdUV", "") 'chq.UstrdUV)
                LineItemsTable.Add("UstrdMicr", "") 'chq.UstrdMicr)
                LineItemsTable.Add("DAdrLine", chq.DAdrLine)
                LineItemsTable.Add("DTwnNm", chq.DTwnNm)
                LineItemsTable.Add("DCtry", chq.DCtry)
                LineItemsTable.Add("DNm", chq.DNm)
                LineItemsTable.Add("DPhneNb", chq.DPhneNb)
                LineItemsTable.Add("DMobNb", chq.DMobNb)
                LineItemsTable.Add("DEmailAdr", chq.DEmailAdr)
                LineItemsTable.Add("DOthr", chq.DOthr)
                LineItemsTable.Add("DbtrAcct", chq.DbtrAcct)
                LineItemsTable.Add("CAdrLine", chq.CAdrLine)
                LineItemsTable.Add("CTwnNm", chq.CTwnNm)
                LineItemsTable.Add("CCtry", chq.CCtry)
                LineItemsTable.Add("CNm", chq.CNm)
                LineItemsTable.Add("CPhneNb", chq.CPhneNb)
                LineItemsTable.Add("CMobNb", chq.CMobNb)
                LineItemsTable.Add("CEmailAdr", chq.CEmailAdr)
                LineItemsTable.Add("COthr", chq.COthr)
                LineItemsTable.Add("PymType", chq.PymType)
                LineItemsTable.Add("CdtrAcct", chq.CdtrAcct)
                LineItemsTable.Add("OrgnlInstrID", chq.OrgnlInstrID)
                LineItemsTable.Add("UstrdColD", chq.UstrdColD)
                LineItemsTable.Add("OrgnlEndToEnd", chq.OrgnlEndToEnd)
                LineItemsTable.Add("ReqdColltnDt", chq.ReqdColltnDt)
                LineItemsTable.Add("CCNm", chq.CNm)
                LineItemsTable.Add("DCNm", chq.DNm)
                LineItemsTable.Add("LclInstrm", chq.LclInstrm)
                LineItemsTable.Add("RemittanceInfo", chq.RemittanceInfo)
                If Modscan.SysType = Modscan.ENUM_SysType.BRNET Then
                    LineItemsTable.Add("Reference", "/" + chq.ChequeNumber + "/" + Modscan.OurBankID + chq.BranchCode + "/" + chq.RemitterAcc + "/" + chq.MsgId + "/") 'chq.reference
                    LineItemsTable.Add("TrxTypeID", "ID")
                End If
                Modscan.dt = New DataTable
                If Modscan.dt.Columns.Count <= 0 Then
                    For Each name As String In LineItemsTable.Keys
                        Dim ColName As DataColumn = New DataColumn()
                        Try
                            ColName.ColumnName = name
                            If DirectCast(LineItemsTable.Item("RCODE"), String) <> "00" Then
                                Select Case name.ToString
                                    Case "FrontBWImage", "UVImage", "FrontGrayScaleImage", "RearImage"
                                        ColName.DataType = System.Type.GetType("System.String")
                                    Case Else
                                        Try
                                            ColName.DataType = System.Type.GetType(LineItemsTable(name).GetType().FullName.ToString)
                                        Catch ex As Exception

                                        End Try
                                End Select
                            Else
                                Try
                                    ColName.DataType = System.Type.GetType(LineItemsTable(name).GetType().FullName.ToString)
                                Catch ex As Exception

                                End Try
                            End If


                            Modscan.dt.Columns.Add(ColName)
                        Catch ex As Exception
                            MsgBox(ex.Message & " - " & ColName.ToString) ' & ": " & LineItemsTable.Item(ColName.ToString).ToString)
                        End Try
                    Next
                End If
                Dim dr As DataRow = Modscan.dt.NewRow()
                For Each name As String In LineItemsTable.Keys
                    dr(name) = LineItemsTable(name)
                Next
                Modscan.dt.Rows.Add(dr)
                Modscan.SaveImagesToDB(LineItemsTable, chq.FrontImageBW, chq.FrontImageGS, chq.BackImageGS, chq.FrontImageUV)
                '------------------------------------------------------------------------------------------------------

            Catch ex As Exception
                MsgBox(ex.Message)
            End Try

        End Sub
        Private Shared Sub ReadFile(ByRef sFile As String, ByVal sTemp As String)
            Dim sline As New List(Of String)(IO.File.ReadAllLines(sFile))
            'Dim p As Integer = sline.LongCount
            'sline.RemoveAt(p - 1)
            IO.File.WriteAllLines(sFile, sline.ToArray())
        End Sub
        'Private  Sub SaveEFT(ByVal d As ETEFTDetails, ByVal sFile As String)
        '    Dim LineItemsTable As Hashtable = New Hashtable
        '    Dim TraCode As String = ""
        '    Dim sPattern As String = ""
        '    Dim PBranch As String = ""
        '    Dim Curr As String = ""
        '    Dim PBank As String = ""
        '    Dim AccountIDLength As Int16 = 13
        '    Dim SystemType As String = ConfigurationManager.AppSettings("sysType")
        '    'MessageBox.Show(SystemType.ToUpper.Trim)
        '    Select Case SystemType.ToUpper.Trim
        '        Case "BR"
        '            PBank = GetScalarREC("Select BankID From t_Banks Where SWIFTCODE Like '" & Mid(d.SourceBIC, 1, 7) & "%'")
        '            If PBank = "" Then
        '                If d.SourceBIC.Contains("ECXAETAA") Then
        '                    d.SourceBIC = "NBETETAA"
        '                    PBank = GetScalarREC("Select BankID From t_Banks Where SWIFTCODE Like '" & d.SourceBIC & "%'")
        '                End If
        '            End If
        '            If PBank <> Modscan.OurBankID Then
        '                If d.RetCode = "00" Then d.ValueDate = Modscan.WORKING_DATE
        '                PBranch = GetScalarREC("Select Top 1 BranchID From t_branches Where BankID = '" & PBank & "'")
        '                sPattern = Modscan.GetNextInt16
        '                TraCode = IIf(d.IsDebit, "40", "59")
        '                If d.TranType <> 0 Then TraCode = IIf(d.TranType = 1, "58", "57")
        '                Curr = d.Currency
        '                Dim RejectReason As String = ""
        '                d.BeneficiaryAcc = d.BeneficiaryAcc
        '                d.RemitterAcc = d.RemitterAcc
        '            ElseIf d.RetCode <> "00" Then
        '                Try
        '                    d.RetCode = "21"
        '                    d.ValueDate = Modscan.WORKING_DATE
        '                    PBranch = GetScalarREC("Select top 1 BranchID from t_Branches where BankID = '" & PBank & "'")
        '                    Dim sBatch As String = PBank.PadLeft(4, "3")
        '                    sPattern = Modscan.GetNextInt16
        '                    TraCode = IIf(d.IsDebit, "40", "59")
        '                    If d.TranType <> 0 Then TraCode = IIf(d.TranType = 1, "58", "57")
        '                    Curr = d.Currency
        '                    Dim RejectReason As String = ""
        '                    d.BeneficiaryAcc = d.BeneficiaryAcc
        '                    d.RemitterAcc = d.RemitterAcc

        '                Catch ex As Exception
        '                    MessageBox.Show(ex.Message, Modscan.MsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop)
        '                End Try
        '            End If
        '        Case "BRMFO"
        '            PBank = GetScalarREC("Select BankID From t_Banks Where SWIFTCODE Like '" & Mid(d.SourceBIC, 1, 7) & "%'")
        '            If PBank = "" Then
        '                If d.SourceBIC.Contains("ECXAETAA") Then
        '                    d.SourceBIC = "NBETETAA"
        '                    PBank = GetScalarREC("Select BankID From t_Banks Where SWIFTCODE Like '" & d.SourceBIC & "%'")
        '                End If
        '            End If
        '            If PBank <> Modscan.OurBankID Then
        '                If d.RetCode = "00" Then d.ValueDate = Modscan.GetNextInt16
        '                PBranch = GetScalarREC("Select Top 1 BranchID From t_branches Where BankID = '" & PBank & "'")
        '                sPattern = Modscan.GetNextInt16
        '                TraCode = IIf(d.IsDebit, "40", "59")
        '                If d.TranType <> 0 Then TraCode = IIf(d.TranType = 1, "58", "57")
        '                Curr = d.Currency
        '                Dim RejectReason As String = ""
        '                d.BeneficiaryAcc = d.BeneficiaryAcc
        '                d.RemitterAcc = d.RemitterAcc
        '            ElseIf d.RetCode <> "00" Then
        '                Try
        '                    d.RetCode = "21"
        '                    d.ValueDate = Modscan.WORKING_DATE
        '                    PBranch = GetScalarREC("Select top 1 BranchID from t_Branches where BankID = '" & PBank & "'")
        '                    Dim sBatch As String = PBank.PadLeft(4, "3")
        '                    sPattern = Modscan.GetNextInt16
        '                    TraCode = IIf(d.IsDebit, "40", "59")
        '                    If d.TranType <> 0 Then TraCode = IIf(d.TranType = 1, "58", "57")
        '                    Curr = d.Currency
        '                    Dim RejectReason As String = ""
        '                    d.BeneficiaryAcc = d.BeneficiaryAcc
        '                    d.RemitterAcc = d.RemitterAcc

        '                Catch ex As Exception
        '                    MessageBox.Show(ex.Message, Modscan.MsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop)
        '                End Try
        '            End If
        '        Case "BRNET"
        '            Try
        '                PBank = GetScalarREC("Select BankID From t_Bank Where SWIFTCODE Like '" & Mid(d.SourceBIC, 1, 7) & "%'")
        '                AccountIDLength = GetScalarREC("SELECT AccountIDLength FROM t_SystemBankSetting")
        '                If PBank = "" Then
        '                    If d.SourceBIC.Contains("ECXAETAA") Then
        '                        d.SourceBIC = "NBETETAA"
        '                        PBank = GetScalarREC("Select BankID From t_Bank Where SWIFTCODE Like '" & d.SourceBIC & "%'")
        '                    End If
        '                End If
        '                If PBank <> Modscan.OurBankID Then
        '                    If d.RetCode = "00" Then d.ValueDate = Modscan.WORKING_DATE
        '                    PBranch = GetScalarREC("Select Top 1 BranchID From t_branch Where BankID = '" & PBank & "'")
        '                    sPattern = Modscan.GetNextInt16
        '                    TraCode = IIf(d.IsDebit, "40", "59")
        '                    If d.TranType <> 0 Then TraCode = IIf(d.TranType = 1, "58", "57")
        '                    Curr = d.Currency
        '                    Dim RejectReason As String = ""
        '                    d.BeneficiaryAcc = d.BeneficiaryAcc
        '                    d.RemitterAcc = d.RemitterAcc
        '                End If


        '                If d.RetCode <> "00" Then

        '                    d.RetCode = "00"
        '                    d.ValueDate = Modscan.WORKING_DATE
        '                    PBranch = GetScalarREC("Select top 1 BranchID from t_Branch where BankID = '" & PBank & "'")
        '                    Dim sBatch As String = PBank.PadLeft(4, "3")
        '                    sPattern = Modscan.GetNextInt16
        '                    TraCode = IIf(d.IsDebit, "40", "59")
        '                    If d.TranType <> 0 Then TraCode = IIf(d.TranType = 1, "58", "57")
        '                    Curr = d.Currency
        '                    Dim RejectReason As String = ""
        '                    d.BeneficiaryAcc = d.BeneficiaryAcc
        '                    d.RemitterAcc = d.RemitterAcc


        '                End If


        '                If IsNothing(d.RetCode) = True Then

        '                    d.RetCode = "00"

        '                End If
        '            Catch ex As Exception
        '                MessageBox.Show(ex.Message, Modscan.MsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop)
        '            End Try
        '        Case "BRNETOLD"
        '            PBank = GetScalarREC("Select BankID From t_Bank Where SWIFTCODE Like '" & Mid(d.SourceBIC, 1, 7) & "%'")
        '            If PBank = "" Then
        '                If d.SourceBIC.Contains("ECXAETAA") Then
        '                    d.SourceBIC = "NBETETAA"
        '                    PBank = GetScalarREC("Select BankID From t_Bank Where SWIFTCODE Like '" & d.SourceBIC & "%'")
        '                End If
        '            End If
        '            If PBank <> Modscan.OurBankID Then
        '                If d.RetCode = "00" Then d.ValueDate = Modscan.GetNextInt16
        '                PBranch = GetScalarREC("Select Top 1 BranchID From t_branch Where BankID = '" & PBank & "'")
        '                sPattern = Modscan.GetNextInt16
        '                TraCode = IIf(d.IsDebit, "40", "59")
        '                If d.TranType <> 0 Then TraCode = IIf(d.TranType = 1, "58", "57")
        '                Curr = d.Currency
        '                Dim RejectReason As String = ""
        '                d.BeneficiaryAcc = d.BeneficiaryAcc
        '                d.RemitterAcc = d.RemitterAcc
        '            ElseIf d.RetCode <> "00" Then
        '                Try
        '                    d.RetCode = "21"
        '                    d.ValueDate = Modscan.WORKING_DATE
        '                    PBranch = GetScalarREC("Select top 1 BranchID from t_Branch where BankID = '" & PBank & "'")
        '                    Dim sBatch As String = PBank.PadLeft(4, "3")
        '                    sPattern = Modscan.GetNextInt16
        '                    TraCode = IIf(d.IsDebit, "40", "59")
        '                    If d.TranType <> 0 Then TraCode = IIf(d.TranType = 1, "58", "57")
        '                    Curr = d.Currency
        '                    Dim RejectReason As String = ""
        '                    d.BeneficiaryAcc = d.BeneficiaryAcc
        '                    d.RemitterAcc = d.RemitterAcc

        '                Catch ex As Exception
        '                    MessageBox.Show(ex.Message, Modscan.MsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop)
        '                End Try
        '            End If
        '    End Select

        '    Try
        '        '------------------------------------------------------------------------------------------------------
        '        LineItemsTable.Add("RCODE", d.RetCode) ' RCODE
        '        LineItemsTable.Add("VTYPE", TraCode) ' Voucher Type
        '        LineItemsTable.Add("AMOUNT", (Val(d.Amount)).ToString) ' Amount
        '        LineItemsTable.Add("ENTRYMODE", "0") ' Amount Entry Mode
        '        LineItemsTable.Add("CURRENCYCODE", Curr) ' Amount Entry Mode
        '        LineItemsTable.Add("DESTBANK", Modscan.OurBankID) ' Dest Bank
        '        If Modscan.SysType = Modscan.ENUM_SysType.BRNET Then
        '            LineItemsTable.Add("DESTBRANCH", IIf(d.IsDebit, Left(Right(d.RemitterAcc.Replace("'", "").Trim, AccountIDLength), 3), Left(Right(d.BeneficiaryAcc.Replace("'", "").Trim, AccountIDLength), 3))) ' Dest Branch
        '            LineItemsTable.Add("DESTACC", IIf(d.IsDebit, Right(d.RemitterAcc.Replace("'", "").Trim, AccountIDLength), Right(d.BeneficiaryAcc.Replace("'", "").Trim, AccountIDLength))) ' Dest Account
        '        Else
        '            LineItemsTable.Add("DESTBRANCH", Modscan.OurBranchID) ' Dest Branch
        '            LineItemsTable.Add("DESTACC", IIf(d.IsDebit, d.RemitterAcc.Replace("'", "").Trim, d.BeneficiaryAcc.Replace("'", "").Trim)) ' Dest Account
        '        End If
        '        LineItemsTable.Add("CHQDGT", "00") ' Check Digit
        '        LineItemsTable.Add("PBANK", PBank) ' PBank
        '        LineItemsTable.Add("PBRANCH", PBranch) ' PBranch
        '        LineItemsTable.Add("FILLER", "0") ' Filler
        '        LineItemsTable.Add("COLLACCName", IIf(d.IsDebit, d.BeneficiaryName.Replace("'", "").Trim, d.RemitterName.Replace("'", "").Trim)) 'Collecting Account Details
        '        LineItemsTable.Add("SNO", "") ' Serial Number
        '        LineItemsTable.Add("PROCNO", d.MsgId) ' Processing Number
        '        LineItemsTable.Add("DRN", d.EFTID) ' Processing Number
        '        LineItemsTable.Add("DATA", d.TrxData) ' The Whole String as is
        '        LineItemsTable.Add("FIMAGESIZEBW", 0)
        '        LineItemsTable.Add("FIMAGESIGNBW", 0)
        '        LineItemsTable.Add("FIMAGESIZE", 0)
        '        LineItemsTable.Add("FIMAGESIGN", 0)
        '        LineItemsTable.Add("BIMAGESIZE", 0)
        '        LineItemsTable.Add("BIMAGESIGN", 0) 'myCol.Item(Item).ToString.Substring(197, 48)) ' back tiff image signature
        '        LineItemsTable.Add("FrontBWImage", Nothing)
        '        LineItemsTable.Add("FrontGrayScaleImage", Nothing)
        '        LineItemsTable.Add("RearImage", Nothing)
        '        LineItemsTable.Add("UVImage", Nothing)
        '        LineItemsTable.Add("FILENAME", sFile) ' The Filename
        '        LineItemsTable.Add("ValidInvalid", True) 'Validity of the image
        '        LineItemsTable.Add("IsFCY", False)
        '        LineItemsTable.Add("ExtraDetails", d.RemittanceInfo.Replace("'", ""))
        '        LineItemsTable.Add("TheirACC", IIf(d.IsDebit, d.BeneficiaryAcc.Replace("'", "").Trim, d.RemitterAcc.Replace("'", "").Trim)) ' Dest Account
        '        LineItemsTable.Add("TrxID", d.TrxId)
        '        LineItemsTable.Add("Reference", d.Reference)
        '        If Modscan.SysType = Modscan.ENUM_SysType.BRNET Then
        '            LineItemsTable.Add("TrxTypeID", "IC")
        '        End If


        '        Modscan.dt = New DataTable
        '        If Modscan.dt.Columns.Count <= 0 Then
        '            For Each name As String In LineItemsTable.Keys
        '                Try
        '                    Dim ColName As DataColumn = New DataColumn()
        '                    ColName.ColumnName = name
        '                    If LineItemsTable(name) = Nothing Then
        '                        ColName.DataType = GetType(String)
        '                    Else
        '                        ColName.DataType = System.Type.GetType(LineItemsTable(name).GetType().FullName.ToString)
        '                    End If
        '                    Modscan.dt.Columns.Add(ColName)
        '                Catch ex As Exception
        '                    MessageBox.Show("Imechapa kwa SaveEFT 1" + ex.Message)
        '                End Try
        '            Next
        '        End If
        '        Dim dr As DataRow = Modscan.dt.NewRow()
        '        For Each name As String In LineItemsTable.Keys
        '            dr(name) = LineItemsTable(name)
        '        Next
        '        Modscan.dt.Rows.Add(dr)
        '        'MessageBox.Show("ET 3049")
        '        'Modscan.dt.TableName = "XMLTest"
        '        'Modscan.dt.WriteXml("C:\\TACH\\TACHfiles\\FromTACH\\Temp\\XmKamunya.xml", True)
        '        Modscan.SaveToDB(Modscan.dt, "IN")

        '        '------------------------------------------------------------------------------------------------------
        '    Catch ex As Exception
        '        MessageBox.Show("Imechapa kwa SaveEFT 2" + ex.Message)
        '    End Try


        'End Sub

        Private Shared Sub SaveRTGS(ByVal d As AchRtgs)
            Dim LineItemsTable As Hashtable = New Hashtable
            Dim SystemType As String = ConfigurationManager.AppSettings("sysType")
            Try
                If (d.RemitterBic.ToString().Split("|").Count > 1) Then
                    d.RemitterBic = d.RemitterBic.ToString().Split("|")(1)
                End If

                If (d.BeneficiaryBic.ToString().Split("|").Count > 1) Then
                    d.BeneficiaryBic = d.BeneficiaryBic.ToString().Split("|")(1)
                End If
                LineItemsTable.Add("Trans_Ref", d.Trans_Ref)
                LineItemsTable.Add("MessageType", d.RtgsType.ToString().ToUpper())
                LineItemsTable.Add("TrxCurrencyID", d.Currency.ToString().ToUpper())
                LineItemsTable.Add("TrxAmount", (Val(d.Amount)).ToString)
                LineItemsTable.Add("BeneficiaryAcc", d.BeneficiaryAcc)
                LineItemsTable.Add("BeneficiaryName", d.BeneficiaryName)
                LineItemsTable.Add("BeneficiaryBic", d.BeneficiaryBic)
                LineItemsTable.Add("BeneficiaryBranch", d.BeneficiaryBranch)
                LineItemsTable.Add("RemitterAcc", d.RemitterAcc)
                LineItemsTable.Add("RemitterName", d.RemitterName)
                LineItemsTable.Add("RemitterBic", d.RemitterBic)
                LineItemsTable.Add("RemitterBranch", d.RemitterBranch)
                LineItemsTable.Add("AdditionalInfo", d.AdditionalInfo)
                LineItemsTable.Add("TxFilename", d.Filename)
                LineItemsTable.Add("Field21", d.EndToEndId)
                LineItemsTable.Add("CreatedBy", Modscan.OperatorID)

                Modscan.dt = New DataTable
                If Modscan.dt.Columns.Count <= 0 Then
                    For Each name As String In LineItemsTable.Keys
                        Try
                            Dim ColName As DataColumn = New DataColumn()
                            ColName.ColumnName = name
                            If LineItemsTable(name) = Nothing Then
                                ColName.DataType = GetType(String)
                            Else
                                ColName.DataType = System.Type.GetType(LineItemsTable(name).GetType().FullName.ToString)
                            End If
                            Modscan.dt.Columns.Add(ColName)
                        Catch ex As Exception
                            MessageBox.Show("Imechapa kwa SaveRTGS 1" + ex.Message)
                        End Try
                    Next
                End If
                Dim dr As DataRow = Modscan.dt.NewRow()
                For Each name As String In LineItemsTable.Keys
                    dr(name) = LineItemsTable(name)
                Next
                Modscan.dt.Rows.Add(dr)
                Modscan.SaveRTGS(Modscan.dt)

                '------------------------------------------------------------------------------------------------------
            Catch ex As Exception
                MessageBox.Show("Imechapa kwa SaveEFT 2" + ex.Message)
            End Try


        End Sub
        Private Shared Sub SaveEFT(ByVal d As ETEFTDetails, ByVal sFile As String)
            Dim LineItemsTable As Hashtable = New Hashtable
            Dim TraCode As String = ""
            Dim sPattern As String = ""
            Dim PBranch As String = ""
            Dim Curr As String = ""
            Dim PBank As String = ""
            Dim AccountIDLength As Int16 = 13
            Dim SystemType As String = ConfigurationManager.AppSettings("sysType")
            'MessageBox.Show(SystemType.ToUpper.Trim)

            Try
                '------------------------------------------------------------------------------------------------------
                LineItemsTable.Add("RCODE", d.RetCode) ' RCODE
                LineItemsTable.Add("VTYPE", d.VCode) ' Voucher Type
                LineItemsTable.Add("AMOUNT", (Val(d.Amount)).ToString) ' Amount
                LineItemsTable.Add("ENTRYMODE", "0") ' Amount Entry Mode
                LineItemsTable.Add("CURRENCYCODE", d.Currency) ' Amount Entry Mode
                LineItemsTable.Add("DESTBANK", Modscan.OurBankID) ' Dest Bank
                LineItemsTable.Add("DESTACC", d.CdtrAcct)
                LineItemsTable.Add("CHQDGT", "00") ' Check Digit
                LineItemsTable.Add("PBANK", d.SourceBankID) ' PBank
                LineItemsTable.Add("PBRANCH", "") ' PBranch
                LineItemsTable.Add("FILLER", "0") ' Filler
                LineItemsTable.Add("COLLACCName", d.DNm)
                LineItemsTable.Add("SNO", "") ' Serial Number
                LineItemsTable.Add("PROCNO", d.MsgId) ' Processing Number
                LineItemsTable.Add("DRN", d.MsgId) ' Processing Number
                LineItemsTable.Add("DATA", d.TrxData) ' The Whole String as is
                LineItemsTable.Add("FIMAGESIZEBW", 0)
                LineItemsTable.Add("FIMAGESIGNBW", 0)
                LineItemsTable.Add("FIMAGESIZE", 0)
                LineItemsTable.Add("FIMAGESIGN", 0)
                LineItemsTable.Add("BIMAGESIZE", 0)
                LineItemsTable.Add("BIMAGESIGN", 0) 'myCol.Item(Item).ToString.Substring(197, 48)) ' back tiff image signature
                LineItemsTable.Add("FrontBWImage", Nothing)
                LineItemsTable.Add("FrontGrayScaleImage", Nothing)
                LineItemsTable.Add("RearImage", Nothing)
                LineItemsTable.Add("UVImage", Nothing)
                LineItemsTable.Add("FILENAME", Path.GetFileName(sFile)) ' The Filename
                LineItemsTable.Add("ValidInvalid", True) 'Validity of the image
                LineItemsTable.Add("IsFCY", False)
                LineItemsTable.Add("ExtraDetails", d.CNm)
                LineItemsTable.Add("TheirACC", d.CdtrAcct)
                LineItemsTable.Add("TrxID", d.TrxId)
                LineItemsTable.Add("Reference", d.UstrdColD)
                If Modscan.SysType = Modscan.ENUM_SysType.BRNET Then
                    LineItemsTable.Add("TrxTypeID", "IC")
                End If
                LineItemsTable.Add("MsgID", d.MsgId)
                LineItemsTable.Add("UstrdMicr", "")
                LineItemsTable.Add("DAdrLine", d.DAdrLine)
                LineItemsTable.Add("DTwnNm", d.DTwnNm)
                LineItemsTable.Add("DCtry", d.DCtry)
                LineItemsTable.Add("DNm", d.DNm)
                LineItemsTable.Add("DPhneNb", d.DPhneNb)
                LineItemsTable.Add("DMobNb", d.DMobNb)
                LineItemsTable.Add("DEmailAdr", d.DEmailAdr)
                LineItemsTable.Add("DOthr", d.DOthr)
                LineItemsTable.Add("DbtrAcct", d.DbtrAcct)
                LineItemsTable.Add("CAdrLine", d.CAdrLine)
                LineItemsTable.Add("CTwnNm", d.CTwnNm)
                LineItemsTable.Add("CCtry", d.CCtry)
                LineItemsTable.Add("CNm", d.CNm)
                LineItemsTable.Add("CPhneNb", d.CPhneNb)
                LineItemsTable.Add("CMobNb", d.CMobNb)
                LineItemsTable.Add("CEmailAdr", d.CEmailAdr)
                LineItemsTable.Add("COthr", d.COthr)
                LineItemsTable.Add("PymType", d.PymType)
                LineItemsTable.Add("CdtrAcct", d.CdtrAcct)
                LineItemsTable.Add("OrgnlInstrID", d.OrgnlInstrID)
                LineItemsTable.Add("UstrdColD", d.UstrdColD)
                LineItemsTable.Add("OrgnlEndToEnd", d.OrgnlEndToEnd)
                LineItemsTable.Add("ReqdColltnDt", d.ReqdColltnDt)
                LineItemsTable.Add("CCNm", d.CCNm)
                LineItemsTable.Add("DCNm", d.DCNm)
                LineItemsTable.Add("SourceBIC", d.SourceBankID)

                LineItemsTable.Add("IntrBkSttlmDt", d.IntrBkSttlmDt)
                LineItemsTable.Add("SvcLvl", d.SvcLvl)
                LineItemsTable.Add("OrgnlTxId", d.OrgnlTxId)
                LineItemsTable.Add("LclInstrm", d.LclInstrm)
                LineItemsTable.Add("CtgyPurp", d.CtgyPurp)
                LineItemsTable.Add("OrgnlIntrBkSttlmDt", d.OrgnlIntrBkSttlmDt)

                Modscan.dt = New DataTable
                If Modscan.dt.Columns.Count <= 0 Then
                    For Each name As String In LineItemsTable.Keys
                        Try
                            Dim ColName As DataColumn = New DataColumn()
                            ColName.ColumnName = name
                            If LineItemsTable(name) = Nothing Then
                                ColName.DataType = GetType(String)
                            Else
                                ColName.DataType = System.Type.GetType(LineItemsTable(name).GetType().FullName.ToString)
                            End If
                            Modscan.dt.Columns.Add(ColName)
                        Catch ex As Exception
                            MessageBox.Show("Imechapa kwa SaveEFT 1" + ex.Message)
                        End Try
                    Next
                End If
                Dim dr As DataRow = Modscan.dt.NewRow()
                For Each name As String In LineItemsTable.Keys
                    dr(name) = LineItemsTable(name)
                Next
                Modscan.dt.Rows.Add(dr)
                'MessageBox.Show("TZ 3049")
                'Modscan.dt.TableName = "XMLTest"
                'Modscan.dt.WriteXml("C:\\TACH\\TACHfiles\\FromTACH\\Temp\\XmKamunya.xml", True)
                Modscan.SaveToDB(Modscan.dt, "IN")

                '------------------------------------------------------------------------------------------------------
            Catch ex As Exception
                MessageBox.Show("Imechapa kwa SaveEFT 2" + ex.Message)
            End Try


        End Sub
        Private Shared Sub SaveDD(ByVal d As ETDDDetail, ByVal sFile As String)
            Dim LineItemsTable As Hashtable = New Hashtable
            Dim TraCode As String = ""
            Dim sPattern As String = ""
            Dim PBranch As String = ""
            Dim Curr As String = ""
            Dim PBank As String = ""
            Dim AccountIDLength As Int16 = 13
            Dim SystemType As String = ConfigurationManager.AppSettings("sysType")
            'MessageBox.Show(SystemType.ToUpper.Trim)

            Try
                '------------------------------------------------------------------------------------------------------
                LineItemsTable.Add("RCODE", d.Retcode) ' RCODE
                LineItemsTable.Add("VTYPE", d.VCode) ' Voucher Type
                LineItemsTable.Add("AMOUNT", (Val(d.Amount)).ToString) ' Amount
                LineItemsTable.Add("ENTRYMODE", "0") ' Amount Entry Mode
                LineItemsTable.Add("CURRENCYCODE", d.Curr) ' Amount Entry Mode
                LineItemsTable.Add("DESTBANK", d.DestBankID) ' Dest Bank
                LineItemsTable.Add("DESTACC", d.CdtrAcct)
                LineItemsTable.Add("CHQDGT", "00") ' Check Digit
                LineItemsTable.Add("PBANK", d.SourceBankID) ' PBank
                LineItemsTable.Add("PBRANCH", "") ' PBranch
                LineItemsTable.Add("FILLER", "0") ' Filler
                LineItemsTable.Add("COLLACCName", d.DNm)
                LineItemsTable.Add("SNO", "") ' Serial Number
                LineItemsTable.Add("PROCNO", d.MsgId) ' Processing Number
                LineItemsTable.Add("DRN", d.MsgId) ' Processing Number
                LineItemsTable.Add("DATA", d.TrxData) ' The Whole String as is
                LineItemsTable.Add("FIMAGESIZEBW", 0)
                LineItemsTable.Add("FIMAGESIGNBW", 0)
                LineItemsTable.Add("FIMAGESIZE", 0)
                LineItemsTable.Add("FIMAGESIGN", 0)
                LineItemsTable.Add("BIMAGESIZE", 0)
                LineItemsTable.Add("BIMAGESIGN", 0) 'myCol.Item(Item).ToString.Substring(197, 48)) ' back tiff image signature
                LineItemsTable.Add("FrontBWImage", Nothing)
                LineItemsTable.Add("FrontGrayScaleImage", Nothing)
                LineItemsTable.Add("RearImage", Nothing)
                LineItemsTable.Add("UVImage", Nothing)
                LineItemsTable.Add("FILENAME", Path.GetFileName(sFile)) ' The Filename
                LineItemsTable.Add("ValidInvalid", True) 'Validity of the image
                LineItemsTable.Add("IsFCY", False)
                LineItemsTable.Add("ExtraDetails", d.CNm)
                LineItemsTable.Add("TheirACC", d.CdtrAcct)
                LineItemsTable.Add("TrxID", d.TrxId)
                LineItemsTable.Add("Reference", d.UstrdColD)
                If Modscan.SysType = Modscan.ENUM_SysType.BRNET Then
                    LineItemsTable.Add("TrxTypeID", "ID")
                End If
                LineItemsTable.Add("MsgID", d.MsgId)
                LineItemsTable.Add("UstrdMicr", "")
                LineItemsTable.Add("DAdrLine", d.DAdrLine)
                LineItemsTable.Add("DTwnNm", d.DTwnNm)
                LineItemsTable.Add("DCtry", d.DCtry)
                LineItemsTable.Add("DNm", d.DNm)
                LineItemsTable.Add("DPhneNb", d.DPhneNb)
                LineItemsTable.Add("DMobNb", d.DMobNb)
                LineItemsTable.Add("DEmailAdr", d.DEmailAdr)
                LineItemsTable.Add("DOthr", d.DOthr)
                LineItemsTable.Add("DbtrAcct", d.DbtrAcct)
                LineItemsTable.Add("CAdrLine", d.CAdrLine)
                LineItemsTable.Add("CTwnNm", d.CTwnNm)
                LineItemsTable.Add("CCtry", d.CCtry)
                LineItemsTable.Add("CNm", d.CNm)
                LineItemsTable.Add("CPhneNb", d.CPhneNb)
                LineItemsTable.Add("CMobNb", d.CMobNb)
                LineItemsTable.Add("CEmailAdr", d.CEmailAdr)
                LineItemsTable.Add("COthr", d.COthr)
                LineItemsTable.Add("PymType", d.PymType)
                LineItemsTable.Add("CdtrAcct", d.CdtrAcct)
                LineItemsTable.Add("OrgnlInstrID", d.OrgnlInstrID)
                LineItemsTable.Add("UstrdColD", d.UstrdColD)
                LineItemsTable.Add("OrgnlEndToEnd", d.EndToEndId)
                LineItemsTable.Add("ReqdColltnDt", d.ReqdColltnDt)
                LineItemsTable.Add("CCNm", d.CNm)
                LineItemsTable.Add("DCNm", d.DNm)
                LineItemsTable.Add("SourceBIC", d.SourceBankID)
                LineItemsTable.Add("IntrBkSttlmDt", d.IntrBkSttlmDt)
                LineItemsTable.Add("SvcLvl", d.SvcLvl)
                LineItemsTable.Add("OrgnlTxId", d.OrgnlTxId)
                LineItemsTable.Add("LclInstrm", d.LclInstrm)
                LineItemsTable.Add("CtgyPurp", d.CtgyPurp)
                LineItemsTable.Add("OrgnlIntrBkSttlmDt", d.IntrBkSttlmDt)
                LineItemsTable.Add("DtOfSgntr", d.Mandate.DtOfSgntr)
                LineItemsTable.Add("MndtId", d.Mandate.MndtId)
                LineItemsTable.Add("FnlColltnDt", d.ReqdColltnDt)
                LineItemsTable.Add("Frqcy", d.Mandate.Frqcy)
                LineItemsTable.Add("CdtrSchmeId", d.Scheme)
                Modscan.dt = New DataTable
                If Modscan.dt.Columns.Count <= 0 Then
                    For Each name As String In LineItemsTable.Keys
                        Try
                            Dim ColName As DataColumn = New DataColumn()
                            ColName.ColumnName = name
                            If LineItemsTable(name) = Nothing Then
                                ColName.DataType = GetType(String)
                            Else
                                ColName.DataType = System.Type.GetType(LineItemsTable(name).GetType().FullName.ToString)
                            End If
                            Modscan.dt.Columns.Add(ColName)
                        Catch ex As Exception
                            MessageBox.Show("Imechapa kwa SaveDD 1" + ex.Message)
                        End Try
                    Next
                End If
                Dim dr As DataRow = Modscan.dt.NewRow()
                For Each name As String In LineItemsTable.Keys
                    dr(name) = LineItemsTable(name)
                Next
                Modscan.dt.Rows.Add(dr)
                'MessageBox.Show("ET 3049")
                'Modscan.dt.TableName = "XMLTest"
                'Modscan.dt.WriteXml("C:\\..Temp\\XmKamunya.xml", True)
                Modscan.SaveToDB(Modscan.dt, "IN")

                '------------------------------------------------------------------------------------------------------
            Catch ex As Exception
                MessageBox.Show("Imechapa kwa SaveDD 2" + ex.Message)
            End Try


        End Sub

        Private Shared Sub SaveRejectedDD(ByVal d As ETDDDetail, ByVal sFile As String)
            Dim LineItemsTable As Hashtable = New Hashtable
            Dim SystemType As String = ConfigurationManager.AppSettings("sysType")
            Try
                '------------------------------------------------------------------------------------------------------
                LineItemsTable.Add("RCODE", d.Retcode) ' RCODE
                LineItemsTable.Add("AMOUNT", (Val(d.Amount)).ToString) ' Amount
                LineItemsTable.Add("CURRENCYCODE", d.Curr) ' Amount Entry Mode
                LineItemsTable.Add("OrgnlInstrID", d.OrgnlInstrID)
                LineItemsTable.Add("OrgnlEndToEnd", d.EndToEndId)
                LineItemsTable.Add("OrgnlTxId", d.OrgnlTxId)
                LineItemsTable.Add("OrgnlIntrBkSttlmDt", d.OrgnlIntrBkSttlmDt)
                LineItemsTable.Add("FileName", Path.GetFileName(sFile))
                LineItemsTable.Add("MsgID", d.MsgId)
                Modscan.dt = New DataTable
                If Modscan.dt.Columns.Count <= 0 Then
                    For Each name As String In LineItemsTable.Keys
                        Try
                            Dim ColName As DataColumn = New DataColumn()
                            ColName.ColumnName = name
                            If LineItemsTable(name) = Nothing Then
                                ColName.DataType = GetType(String)
                            Else
                                ColName.DataType = System.Type.GetType(LineItemsTable(name).GetType().FullName.ToString)
                            End If
                            Modscan.dt.Columns.Add(ColName)
                        Catch ex As Exception
                            MessageBox.Show("Imechapa kwa SaveDD 1" + ex.Message)
                        End Try
                    Next
                End If
                Dim dr As DataRow = Modscan.dt.NewRow()
                For Each name As String In LineItemsTable.Keys
                    dr(name) = LineItemsTable(name)
                Next
                Modscan.dt.Rows.Add(dr)
                'MessageBox.Show("ET 3049")
                'Modscan.dt.TableName = "XMLTest"
                'Modscan.dt.WriteXml("C:\\..Temp\\XmKamunya.xml", True)
                Modscan.SaveUnPaidETDDs(Modscan.dt)

                '------------------------------------------------------------------------------------------------------
            Catch ex As Exception
                MessageBox.Show("Imechapa kwa SaveDD 2" + ex.Message)
            End Try


        End Sub

#End Region

#Region "Utility"
        Private Shared Function UnzipFiles(ByVal sArchive As String, ByVal Filter() As String, Optional ByVal sOutLocation As String = "") As List(Of String)
            Dim l As New List(Of String)
            Dim sTempFile As String = Path.Combine(TempLocation, sArchive)
            Dim sFile As String = Path.Combine(strFileLocation, sArchive)

            Dim zArchive As New ZipFile()
            Try
                If sOutLocation.Trim() = "" Then sOutLocation = Path.Combine(TempLocation, Path.GetFileNameWithoutExtension(sArchive))
                zArchive = New ZipFile(sFile)
                zArchive.ExtractAll(sOutLocation, ExtractExistingFileAction.OverwriteSilently)
                Dim di = New DirectoryInfo(sOutLocation)
                For Each f As String In Filter
                    Dim fi As FileInfo() = di.GetFiles(f)
                    For Each inf As FileInfo In fi
                        l.Add(inf.FullName)
                    Next
                Next
                zArchive.Dispose()
            Catch ex As Ionic.Zip.ZipException
                RejectedItems(sArchive)
            Catch ex As Exception
                MsgBox("Could Not Process The File [" & sArchive & "] Due To:" & vbNewLine & ex.Message, MsgBoxStyle.Critical, Modscan.MsgBoxTitle)
                zArchive.Dispose()
                If Not Directory.Exists(sCorruptPath) Then Directory.CreateDirectory(sCorruptPath)
                File.Move(sFile, Path.Combine(sCorruptPath, sArchive))
            End Try
            File.Delete(sTempFile)
            Return l
        End Function
        Private Shared Sub UnSignFile(ByVal sFile As String)
            Try
                sFile = sFile.Replace("\", "/")
                Modscan.strBatchPath = Modscan.strBatchPath & "Execute.bat"
                Modscan.strDSkeyFile = Encoding.ASCII.GetString(Convert.FromBase64String(ConfigurationManager.AppSettings("keypass")))

                Dim strCmd As String = """" & Modscan.strJavaExeInstallation.Trim() & """ -cp .;com.springsource.org.bouncycastle.jce-1.46.0.jar;com.springsource.org.bouncycastle.mail-1.46.0.jar SignatureClient DSkeyFile=" _
                                   & Modscan.strDSkeyFile.Trim().Replace("\", "/") & " fileName=" & sFile & " function=unsign mode=CMS"
                Dim myFileStream As FileStream = Nothing
                Dim myEJContentStreamWriter As StreamWriter = Nothing
                Try
                    myEJContentStreamWriter = New StreamWriter(Modscan.strBatchPath, True)
                    myEJContentStreamWriter.WriteLine(strCmd)
                Finally
                    If Not (myEJContentStreamWriter Is Nothing) Then myEJContentStreamWriter.Close()
                End Try
                ExecuteCommand(Modscan.strBatchPath)


            Catch ex As Exception
                Modscan.ErrorLog("output>>" & (If([String].IsNullOrEmpty(ex.Message), "(none)", ex.Message)), "SignFile-ET Files")
            End Try
        End Sub
        Private Shared Sub ExecuteCommand(ByVal strBatchPath As String)
            Dim ExitCode As Integer
            Dim ProcessInfo As ProcessStartInfo
            Dim process__1 As Process = Nothing
            Dim output As String = ""
            Dim [error] As String = ""
            Dim strWorkingDir As String = Path.GetDirectoryName(strBatchPath)
            Try
                ProcessInfo = New ProcessStartInfo(strBatchPath)
                ProcessInfo.CreateNoWindow = True
                ProcessInfo.UseShellExecute = False
                ProcessInfo.WorkingDirectory = strWorkingDir

                ProcessInfo.RedirectStandardError = True
                ProcessInfo.RedirectStandardOutput = True

                process__1 = Process.Start(ProcessInfo)
                process__1.WaitForExit()

                output = process__1.StandardOutput.ReadToEnd()
                [error] = process__1.StandardError.ReadToEnd()
                If [error] <> "" Then
                    Modscan.ErrorLog("output>>" & (If([String].IsNullOrEmpty(output), "(none)", output)), "ExecuteCommand-ET Files")
                    Modscan.ErrorLog("error>>" & (If([String].IsNullOrEmpty([error]), "(none)", [error])), "ExecuteCommand-ET Files")
                End If
            Catch ex As Exception
                Modscan.ErrorLog("output>>" & (If([String].IsNullOrEmpty(output), "(none)", output)), "ExecuteCommand-ET Files")
                Modscan.ErrorLog("error>>" & (If([String].IsNullOrEmpty([error]), "(none)", [error])), "ExecuteCommand-ET Files")
            End Try
            ExitCode = process__1.ExitCode
            process__1.Close()
            Kill(strBatchPath)
        End Sub
        Private Shared Sub StripSignature(ByRef sFile As String, ByVal sTemp As String)
            'Dim fBytes() As Byte = File.ReadAllBytes(sFile)
            'Dim m As New Montran(CertName)
            'fBytes = m.ReadSigned(fBytes)
            'If Not fBytes Is Nothing Then
            '    File.WriteAllBytes(sTemp, fBytes)
            '    sFile = sTemp
            'End If
        End Sub

#End Region
    End Class
#End Region
End Namespace
