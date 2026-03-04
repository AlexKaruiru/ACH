Imports System.Configuration
Imports System.Data.OracleClient
Imports System.Data.SqlClient
Imports System.IO
Imports System.Linq
Imports System.Linq.Expressions
Imports System.Security.Cryptography
Imports System.Security.Cryptography.Pkcs
Imports System.Security.Cryptography.X509Certificates
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading.Tasks
Imports System.Windows.Forms
Imports System.Xml
Imports System.Xml.Linq
Imports System.Xml.Serialization
Imports BRCATDS
Imports BrClearing.Common
Imports BrClearing.Common.ETH
Imports Ionic.Zip
Imports ch = BrClearing.Common.ISO.Cheques
Imports cr = BrClearing.Common.BRISO20022CT812
Imports dr = BrClearing.Common.ISO.Debits
Imports pc412 = BrClearing.Common.BRISO20022PC412
Imports pcr = BrClearing.Common.ISO.Cancellations
Imports res = BrClearing.Common.BRISO20022PS213

Namespace TZ
#Region "Structures"
    Public Structure DDDetail
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
        Dim CrAcc As String
        Dim CrName As String
        Dim DrAcc As String
        Dim DrName As String
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
    End Structure
    Public Structure DDMandate
        Dim MndtId As String
        Dim DtOfSgntr As String
        Dim AmdmntInd As String
    End Structure
    Public Structure EFTDetails
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
    Public Structure ChequeDetails
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
    End Enum
    Public Enum ChequeFormat
        SISPackage = 0
        XMLPackages = 1
    End Enum
#End Region
#Region "Outwards Generation Class"
    Public Class BRTZClass
        'mm = FileType, cc = CurrCode, = xyz = Session, x = CertName, y = Token/Keystore pass/Cert Password, (TPss and Tusr are both decoys, both are not in use) 
        Public Shared Function GenerateTZ(ByVal x As String, ByVal y As String, ByVal mm As FileType, ByVal cc As Int32, ByVal Exclude As Boolean, Optional ByVal chqFormat As ChequeFormat = ChequeFormat.XMLPackages, Optional ByVal xyz As String = "", Optional ByVal TPss As String = "", Optional ByVal TUsr As String = "", Optional ByVal ls As List(Of String) = Nothing) As GenerationResult
            Return GenerateTZFiles(mm, cc, Exclude, chqFormat, xyz, x, y, TPss, TUsr, ls)
        End Function
#Region "Varibles"
        Private Shared Sign As Boolean = True
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
        Private Shared Function GenerateTZFiles(ByVal FileType As FileType, ByVal CurrCode As Int32, ByVal Exclude As Boolean, Optional ByVal chqFormat As ChequeFormat = ChequeFormat.XMLPackages, Optional ByVal Session As String = "01", Optional ByVal x As String = "", Optional ByVal y As String = "", Optional ByVal TokenPass As String = "", Optional ByVal TokenUser As String = "", Optional ByVal ls As List(Of String) = Nothing) As GenerationResult
            Dim result As New GenerationResult()
            Try

                Dim RegX As New Regex("[^A-Za-z0-9]")
                Dim strDBAction As String = ""
                Dim strAction As String = ""
                Dim modifiedXml = String.Empty
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
                    Modscan.WORKING_DATE = Format(Convert.ToDateTime(Modscan.cFromDate), "yyyy-MM-dd")
                    Modscan.cWORKING_DATE = Format(Convert.ToDateTime(Modscan.cToDate), "yyyy-MM-dd")
                    Modscan.cFromDate = Format(Convert.ToDateTime(Modscan.cFromDate), "yyyy-MM-dd")
                    Modscan.cToDate = Format(Convert.ToDateTime(Modscan.cToDate), "yyyy-MM-dd")
                Catch ex As Exception
                    result.AddWarning("Date conversion failed: " & ex.Message)
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
                                    If Not Modscan.ExecuteData(Modscan.GetModify("[Proc_CreateXMLFiles_Tanzania_Selected_CTS]", "OurBankID", Modscan.OurBankID, "ourbranchid", Modscan.OurBranchID, "ReadDate", Modscan.cToDate, "FromDate", Modscan.cFromDate, "EJDate", Modscan.cWORKING_DATE, "ClearingCenters", "67", "AllCenters", 0, "Currency", CurrCode, "Session", 0, "xmlList", modifiedXml), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.SelectStatement, Modscan.publicDset) Then
                                        result.AddError("Database execution failed for Cheques: " & Modscan.LastErrorMessage)
                                        Return result
                                    End If
                                Else
                                    If Not Modscan.ExecuteData(Modscan.GetModify("[Proc_CreateXMLFiles_Tanzania_CTS]", "OurBankID", Modscan.OurBankID, "ourbranchid", Modscan.OurBranchID, "ReadDate", Modscan.cToDate, "FromDate", Modscan.cFromDate, "EJDate", Modscan.cWORKING_DATE, "ClearingCenters", "67", "AllCenters", 0, "Currency", CurrCode, "Session", 0), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.SelectStatement, Modscan.publicDset) Then
                                        result.AddError("Database execution failed for Cheques: " & Modscan.LastErrorMessage)
                                        Return result
                                    End If
                                End If
                            Else
                                If Not Modscan.ExecuteData(Modscan.GetModify("[Proc_CreateXMLFiles_Tanzania_CTS]", "OurBankID", Modscan.OurBankID, "ourbranchid", Modscan.OurBranchID, "ReadDate", Modscan.cToDate, "FromDate", Modscan.cFromDate, "EJDate", Modscan.cWORKING_DATE, "ClearingCenters", "67", "AllCenters", 0, "Currency", CurrCode, "Session", 0), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.SelectStatement, Modscan.publicDset) Then
                                    result.AddError("Database execution failed for Cheques: " & Modscan.LastErrorMessage)
                                    Return result
                                End If
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
                                Modscan.publicDTbl.Clear()

                                BankArr = New ArrayList
                                For i As Int32 = 0 To publicDTblBankCopy.Rows.Count - 1
                                    BankArr.Add(publicDTblBankCopy.Rows(i)("BankID").ToString)
                                Next

                                MsgIdArr = New ArrayList()
                                For i As Int32 = 0 To publicDTblmsgIDCopy.Rows.Count - 1
                                    MsgIdArr.Add(publicDTblmsgIDCopy.Rows(i)("OrgnlMsgId").ToString)
                                Next
                            Else
                                result.AddInfo("There are no pending cheques/unpaid Cheques for generation")
                                Return result
                            End If
                        Case FileType.Efts, FileType.EftReturn, FileType.DD, FileType.DDReturn
                            ' Modscan.ExecuteData(Modscan.GetModify("Proc_CreateXMLFiles_Tanzania_CTS", "OurBankID", Modscan.OurBankID, "ourbranchid", Modscan.OurBranchID, "ReadDate", Modscan.cToDate, "FromDate", Modscan.cFromDate, "EJDate", Modscan.cWORKING_DATE, "ClearingCenters", "67", "AllCenters", 0, "Currency", CurrCode, "Session", 0), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure, Modscan.publicDset)

                            If ls IsNot Nothing Then
                                If ls.Count > 0 Then
                                    If Not Modscan.ExecuteData(Modscan.GetModify("[Proc_CreateXMLFiles_Tanzania_Selected_CTS]", "OurBankID", Modscan.OurBankID, "ourbranchid", Modscan.OurBranchID, "ReadDate", Modscan.cToDate, "FromDate", Modscan.cFromDate, "EJDate", Modscan.cWORKING_DATE, "ClearingCenters", "67", "AllCenters", 0, "Currency", CurrCode, "Session", 0, "xmlList", modifiedXml), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.SelectStatement, Modscan.publicDset) Then
                                        result.AddError("Database execution failed for EFTs/DDs: " & Modscan.LastErrorMessage)
                                        Return result
                                    End If
                                Else
                                    If Not Modscan.ExecuteData(Modscan.GetModify("[Proc_CreateXMLFiles_Tanzania_CTS]", "OurBankID", Modscan.OurBankID, "ourbranchid", Modscan.OurBranchID, "ReadDate", Modscan.cToDate, "FromDate", Modscan.cFromDate, "EJDate", Modscan.cWORKING_DATE, "ClearingCenters", "67", "AllCenters", 0, "Currency", CurrCode, "Session", 0), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.SelectStatement, Modscan.publicDset) Then
                                        result.AddError("Database execution failed for EFTs/DDs: " & Modscan.LastErrorMessage)
                                        Return result
                                    End If
                                End If
                            Else
                                If Not Modscan.ExecuteData(Modscan.GetModify("[Proc_CreateXMLFiles_Tanzania_CTS]", "OurBankID", Modscan.OurBankID, "ourbranchid", Modscan.OurBranchID, "ReadDate", Modscan.cToDate, "FromDate", Modscan.cFromDate, "EJDate", Modscan.cWORKING_DATE, "ClearingCenters", "67", "AllCenters", 0, "Currency", CurrCode, "Session", 0), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.SelectStatement, Modscan.publicDset) Then
                                    result.AddError("Database execution failed for EFTs/DDs: " & Modscan.LastErrorMessage)
                                    Return result
                                End If
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
                                result.AddInfo("There are no pending Efts/unpaid EFTs for generation")
                                Return result
                            End If
                    End Select
                Catch ex As Exception
                    result.AddError("Critical error during data retrieval: " & ex.Message)
                    Modscan.ErrorLog(ex.Message, "GenerateTZFiles - Data Retrieval")
                    Return result
                End Try
                'MessageBox.Show("Inaenda FileType.Cheques - " & CurrCode)
                Select Case FileType
                    Case FileType.Cheques
                        For k As Int32 = 0 To BankArr.Count - 1
                            Try
                                Dim i As Integer = 0
                                Dim amt As Decimal = 0
                                Dim l As New List(Of ChequeDetails)
                                Dim CreateFile As String = Session & Now.ToString(".yyyyMMdd.HHmm.") & BankArr.Item(k) & "." & Session.Substring(1)
                                Dim BIC As String = ""
                                'Ejs
                                'Modscan.ExecuteData(Modscan.GetModify("Proc_CreateXMLFiles_Tanzania_CTS", "OurBankID", Modscan.OurBankID, "ourbranchid", Modscan.OurBranchID, "ReadDate", Modscan.cToDate, "FromDate", Modscan.cFromDate, "EJDate", Modscan.cWORKING_DATE, "ClearingCenters", "67", "AllCenters", 0, "Currency", CurrCode, "Session", 1), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure)
                                If ls IsNot Nothing Then
                                    If ls.Count > 0 Then
                                        If Not Modscan.ExecuteData(Modscan.GetModify("[Proc_CreateXMLFiles_Tanzania_Selected_CTS]", "OurBankID", Modscan.OurBankID, "ourbranchid", Modscan.OurBranchID, "ReadDate", Modscan.cToDate, "FromDate", Modscan.cFromDate, "EJDate", Modscan.cWORKING_DATE, "ClearingCenters", "67", "AllCenters", 0, "Currency", CurrCode, "Session", 0, "xmlList", modifiedXml), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.SelectStatement, Modscan.publicDset) Then
                                            result.AddError("Database execution failed for Cheques during loop: " & Modscan.LastErrorMessage)
                                            Continue For
                                        End If
                                    Else
                                        If Not Modscan.ExecuteData(Modscan.GetModify("[Proc_CreateXMLFiles_Tanzania_CTS]", "OurBankID", Modscan.OurBankID, "ourbranchid", Modscan.OurBranchID, "ReadDate", Modscan.cToDate, "FromDate", Modscan.cFromDate, "EJDate", Modscan.cWORKING_DATE, "ClearingCenters", "67", "AllCenters", 0, "Currency", CurrCode, "Session", 0), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.SelectStatement, Modscan.publicDset) Then
                                            result.AddError("Database execution failed for Cheques during loop: " & Modscan.LastErrorMessage)
                                            Continue For
                                        End If
                                    End If
                                Else
                                    If Not Modscan.ExecuteData(Modscan.GetModify("[Proc_CreateXMLFiles_Tanzania_CTS]", "OurBankID", Modscan.OurBankID, "ourbranchid", Modscan.OurBranchID, "ReadDate", Modscan.cToDate, "FromDate", Modscan.cFromDate, "EJDate", Modscan.cWORKING_DATE, "ClearingCenters", "67", "AllCenters", 0, "Currency", CurrCode, "Session", 0), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.SelectStatement, Modscan.publicDset) Then
                                        result.AddError("Database execution failed for Cheques during loop: " & Modscan.LastErrorMessage)
                                        Continue For
                                    End If
                                End If
                                Dim EJfoundRows() As DataRow
                                Select Case SystemType.ToUpper.Trim
                                    Case "BR"
                                        EJfoundRows = Modscan.publicDTbl.Select("TrxType ='O' AND ReturnCode ='00' AND BankID = '" & BankArr.Item(k) & "' And isNull(IsGenerated, false)=false")
                                    Case "BRMFO"
                                        EJfoundRows = Modscan.publicDTbl.Select("TrxType ='O' AND ReturnCode ='00' AND BankID = '" & BankArr.Item(k) & "' And isNull(IsGenerated, false)=false")
                                    Case "BRNET"
                                        EJfoundRows = Modscan.publicDTbl.Select("TrxType ='OC' AND ReturnCode ='00' AND BankID = '" & BankArr.Item(k) & "' And isNull(IsGenerated, false)=false")
                                    Case "BRNETOLD"
                                        EJfoundRows = Modscan.publicDTbl.Select("TrxType ='OC' AND ReturnCode ='00' AND BankID = '" & BankArr.Item(k) & "' And isNull(IsGenerated, false)=false")
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
                                    result.AddError("Swift Code not maintained for our BankID: " & ex.Message)
                                    Modscan.ErrorLog("Swift Code not maintained for this our BankID", "Cheques Generation")
                                    Continue For
                                End Try
                                Try
                                    If publicDTblEJCopy.Rows.Count > 0 Then
                                        DestBankBIC = ""
                                        DestBankBIC = publicDTblEJCopy(0)("DestinationSwiftCode").ToString()
                                    End If
                                Catch ex As Exception
                                    result.AddError("Swift Code not maintained for BankID " & publicDTblEJCopy(0)("BankID") & ": " & ex.Message)
                                    Modscan.ErrorLog("Swift Code not maintained for this BankID " & publicDTblEJCopy(0)("BankID"), "Cheque generation for this bank aborted - Cheques Generation")
                                    Continue For
                                End Try

                                For Each r As DataRow In publicDTblEJCopy.Rows
                                    Try
                                        i += 1
                                        Dim chq As New ChequeDetails
                                        chq.Amount = CDec(r("Amount"))
                                        'chq.BackImageGS = DirectCast(Modscan.String2Bytes(r("BackImageGrayScale")), Byte())
                                        chq.BankCode = r("BankID").ToString().PadLeft(3, "0")
                                        chq.BankBIC = r("DestinationSwiftCode").ToString()
                                        chq.BeneficiaryAcc = r("AccountID").ToString()
                                        chq.CurrencyCode = r("CurrencyCode").ToString()
                                        'chq.CurrencyCode = CurrCode
                                        chq.BeneficiaryName = RegX.Replace(r("BeneficiaryName").ToString(), " ")
                                        If chq.BeneficiaryName.Length > 55 Then chq.BeneficiaryName = chq.BeneficiaryName.Substring(0, 55)
                                        chq.BranchCode = r("BranchID").ToString().PadLeft(3, "0")
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
                                        result.AddError("Failed to process cheque " & r("ChequeId").ToString() & ": " & ex.Message)
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
                                            'ZipContents(CreateFile, msgId, New String() {"*.xml", "*.tiff"}, "", True)
                                            For Each c As ChequeDetails In l
                                                Select Case SystemType.ToUpper.Trim
                                                    Case "BR"
                                                        strAction = "UPDATE t_TransactionMICR SET IsGenerated = 1, Reference = '" & msgId & "', ExtraDetails = '" & c.Codeline & "',  MicrLine ='" & c.Codeline & "' WHERE ColumnID = '" & c.RemittanceInfo & "'"
                                                    Case "BRMFO"
                                                        strAction = "UPDATE t_TransactionMICR SET IsGenerated = 1, Reference = '" & msgId & "', ExtraDetails = '" & c.Codeline & "',  MicrLine ='" & c.Codeline & "' WHERE ColumnID = '" & c.RemittanceInfo & "'"
                                                    Case "BRNET"
                                                        strAction = "UPDATE t_TrxClearing SET IsGenerated = 1, SessNo='1' ,Reference = '" & msgId & "' WHERE TrxRowID = '" & c.trxID & "' AND ReturnCodeID = '00'; " &
                                                                    "UPDATE t_AccountTrxClearing SET IsGenerated = 1, SessNo='1' ,Reference = '" & msgId & "' WHERE TrxRowID = '" & c.trxID & "' AND ReturnCodeID = '00'"
                                                        'Modscan.ExecuteData(Modscan.GetModify("p_UpdateGeneratedClearingTrx", "TrxRowID", c.trxID, "SessNo", "2", "Reference", msgId, "ReadDate", Modscan.cToDate, "FromDate", Modscan.cFromDate), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.SelectStatement, Modscan.publicDset)
                                                    Case "BRNETOLD"
                                                        strAction = "UPDATE t_TrxClearing SET IsGenerated = 1, SessNo='1' ,Reference = '" & msgId & "' WHERE TrxRowID = '" & c.RemittanceInfo & "'"
                                                End Select
                                                If Not String.IsNullOrEmpty(strAction) Then
                                                    Modscan.ExecuteData(strAction, Nothing, Modscan.dataExecTypes.ExecTypeNonQuery, Modscan.queryType.SelectStatement)
                                                End If
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
                    Case FileType.ChequeReturn
                        Counter = 0
                        For k As Int32 = 0 To BankArr.Count - 1
                            For MsgIdK As Int32 = 0 To MsgIdArr.Count - 1
                                Try
                                    Counter = Counter + 1
                                    'MessageBox.Show("Imefika Hapa 2 - " & CurrCode)
                                    Dim i As Integer = 0
                                    Dim amt As Decimal = 0
                                    Dim l As New List(Of ChequeDetails)
                                    Dim CreateFile As String = Session & Now.ToString(".yyyyMMdd.HHmm.") & BankArr.Item(k) & "" & Session.Substring(1)
                                    Dim BIC As String = ""
                                    'Ejs Rejects

                                    If ls IsNot Nothing Then
                                        If ls.Count > 0 Then
                                            Modscan.ExecuteData(Modscan.GetModify("[Proc_CreateXMLFiles_Tanzania_Selected_CTS]", "OurBankID", Modscan.OurBankID, "ourbranchid", Modscan.OurBranchID, "ReadDate", Modscan.cToDate, "FromDate", Modscan.cFromDate, "EJDate", Modscan.cWORKING_DATE, "ClearingCenters", "67", "AllCenters", 0, "Currency", CurrCode, "Session", 2, "xmlList", modifiedXml), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.SelectStatement, Modscan.publicDset)
                                        Else
                                            Modscan.ExecuteData(Modscan.GetModify("Proc_CreateXMLFiles_Tanzania_CTS", "OurBankID", Modscan.OurBankID, "ourbranchid", Modscan.OurBranchID, "ReadDate", Modscan.cToDate, "FromDate", Modscan.cFromDate, "EJDate", Modscan.cWORKING_DATE, "ClearingCenters", "67", "AllCenters", 0, "Currency", CurrCode, "Session", 2), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure)

                                        End If
                                    Else
                                        Modscan.ExecuteData(Modscan.GetModify("Proc_CreateXMLFiles_Tanzania_CTS", "OurBankID", Modscan.OurBankID, "ourbranchid", Modscan.OurBranchID, "ReadDate", Modscan.cToDate, "FromDate", Modscan.cFromDate, "EJDate", Modscan.cWORKING_DATE, "ClearingCenters", "67", "AllCenters", 0, "Currency", CurrCode, "Session", 2), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure)
                                    End If
                                    Dim EJUNPfoundRows() As DataRow
                                    Select Case SystemType.ToUpper.Trim
                                        Case "BR"
                                            EJUNPfoundRows = Modscan.publicDTbl.Select("TrxType ='O' AND ReturnCode <>'00' AND BankID = '" & BankArr.Item(k) & "' And isNull(IsGenerated, false)=false")
                                        Case "BRMFO"
                                            EJUNPfoundRows = Modscan.publicDTbl.Select("TrxType ='O' AND ReturnCode <>'00' AND BankID = '" & BankArr.Item(k) & "' And isNull(IsGenerated, false)=false")
                                        Case "BRNET"
                                            EJUNPfoundRows = Modscan.publicDTbl.Select("TrxType ='OC' AND ReturnCode <>'00' AND BankID = '" & BankArr.Item(k) & "' And OrgnlMsgId =  '" & MsgIdArr.Item(MsgIdK) & "'   AND isNull(IsGenerated, 0) = 0 ")
                                        Case "BRNETOLD"
                                            EJUNPfoundRows = Modscan.publicDTbl.Select("TrxType ='OC' AND ReturnCode <>'00' AND BankID = '" & BankArr.Item(k) & "' And isNull(IsGenerated, false)=false")
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
                                        result.AddError("Swift Code not maintained for our BankID: " & ex.Message)
                                        Modscan.ErrorLog("Swift Code not maintained for this our BankID", " - Unpaid Cheques Generation")
                                        Continue For
                                    End Try
                                    Try
                                        If publicDTblEJCopy.Rows.Count > 0 Then
                                            DestBankBIC = ""
                                            DestBankBIC = publicDTblEJCopy(0)("DestinationSwiftCode").ToString()
                                        End If
                                    Catch ex As Exception
                                        result.AddError("Swift Code not maintained for BankID " & publicDTblEJCopy(0)("BankID") & ": " & ex.Message)
                                        Modscan.ErrorLog("Swift Code not maintained for this BankID " & publicDTblEJCopy(0)("BankID"), "- Unpaid Cheques Generation")
                                        Continue For
                                    End Try
                                    Modscan.publicDTbl.Clear()
                                    For Each r As DataRow In publicDTblEJCopy.Rows
                                        Try
                                            i += 1
                                            Dim chq As New ChequeDetails
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
                                            chq.CurrencyCode = r("CurrencyCode").ToString()
                                            'chq.CurrencyCode = CurrCode
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
                                            chq.ReqdColltnDt = IIf(r("ReqdColltnDt").ToString() = "Jan  1 1900 12:00AM", "", r("ReqdColltnDt").ToString())
                                            l.Add(chq)
                                        Catch ex As Exception
                                            result.AddError("Failed to process unpaid cheque " & r("ChequeId").ToString() & ": " & ex.Message)
                                            Modscan.ErrorLog(ex.Message, "Unpaid Cheques Generation")
                                            Continue For
                                        End Try
                                    Next
                                    If l.Count > 0 Then
                                        Dim msgId As String = UnpaidCheques(l, DestBankBIC, l(0).CurrencyCode, Modscan.cWORKING_DATE, Counter, l(0).ValueDate)
                                        For Each s As ChequeDetails In l
                                            Try
                                                Select Case SystemType.ToUpper.Trim
                                                    Case "BR"
                                                        strAction = "UPDATE t_TransactionMICR SET IsGenerated = 1, Reference = '" & msgId & "' , RejectedReason = '" & s.RetCode & "' ,  MicrLine ='" & s.Codeline & "' AND ReturnCode <>'00' WHERE ColumnID = '" & s.RemittanceInfo & "'"
                                                    Case "BRMFO"
                                                        strAction = "UPDATE t_TransactionMICR SET IsGenerated = 1, Reference = '" & msgId & "' , RejectedReason = '" & s.RetCode & "' ,  MicrLine ='" & s.Codeline & "' AND ReturnCode <>'00' WHERE ColumnID = '" & s.RemittanceInfo & "'"
                                                    Case "BRNET"
                                                        strAction = "UPDATE t_TrxClearing SET IsGenerated = 1, SessNo='1' ,Reference = '" & msgId & "'  WHERE TrxRowID = '" & s.trxID & "' AND  ReturnCodeID <>'00'; " &
                                                                    "UPDATE t_AccountTrxClearing SET IsGenerated = 1, SessNo='1' ,Reference = '" & msgId & "'  WHERE TrxRowID = '" & s.trxID & "' AND  ReturnCodeID <>'00'"
                                                        'Modscan.ExecuteData(Modscan.GetModify("p_UpdateGeneratedClearingTrx", "TrxRowID", s.trxID, "SessNo", "2", "Reference", msgId, "ReadDate", Modscan.cToDate, "FromDate", Modscan.cFromDate), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.SelectStatement, Modscan.publicDset)
                                                    Case "BRNETOLD"
                                                        strAction = "UPDATE t_TrxClearing SET IsGenerated = 1, SessNo='1' ,Reference = '" & msgId & "' ReturnCodeID <>'00'  WHERE TrxRowID = '" & s.MsgId & "'  AND  ReturnCodeID <>'00'"
                                                End Select
                                                If Not String.IsNullOrEmpty(strAction) Then
                                                    Modscan.ExecuteData(strAction, Nothing, Modscan.dataExecTypes.ExecTypeNonQuery, Modscan.queryType.SelectStatement)
                                                End If
                                            Catch ex As Exception
                                                result.AddError("Failed to update status for unpaid cheque " & s.trxID & ": " & ex.Message)
                                                Modscan.ErrorLog(ex.Message, "Unpaid Cheques Generation")
                                                Continue For
                                            End Try
                                        Next
                                    End If
                                    System.Threading.Thread.Sleep(300)
                                    Application.DoEvents()
                                Catch ex As Exception
                                    result.AddError("Unexpected error in Unpaid Cheques Generation loop: " & ex.Message)
                                    Modscan.ErrorLog(ex.Message, "Unpaid Cheques Generation 2 ")
                                    Continue For
                                End Try
                            Next
                        Next
                        publicDTblEJCopy.Clear()
                    Case FileType.RTGS

                    Case FileType.Efts
                        For k As Int32 = 0 To BankArr.Count - 1
                            Dim i As Integer = 0
                            Dim amt As Decimal = 0
                            Dim l As New List(Of ChequeDetails)
                            Dim CreateFile As String = Session & Now.ToString(".yyyyMMdd.HHmm.") & BankArr.Item(k) & "." & Session.Substring(1)
                            Dim BIC As String = ""
                            'EFT Cr
                            'Truncation  Modscan.ExecuteData(Modscan.GetModify("Proc_CreateXMLFiles_Tanzania_CTS", "OurBankID", Modscan.OurBankID, "ourbranchid", Modscan.OurBranchID, "ReadDate", Modscan.WORKING_DATE, "FromDate", Modscan.cFromDate, "EJDate", Modscan.cToDate, "ClearingCenters", "67", "AllCenters", 0, "Currency", CurrCode), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure, Modscan.publicDset)
                            'Modscan.ExecuteData(Modscan.GetModify("Proc_CreateXMLFiles_Tanzania_CTS", "OurBankID", Modscan.OurBankID, "ourbranchid", Modscan.OurBranchID, "ReadDate", Modscan.cToDate, "FromDate", Modscan.cFromDate, "EJDate", Modscan.cWORKING_DATE, "ClearingCenters", "47", "AllCenters", 0, "Currency", CurrCode, "Session", 1), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure, Modscan.publicDset)
                            If ls IsNot Nothing Then
                                If ls.Count > 0 Then
                                    Modscan.ExecuteData(Modscan.GetModify("[Proc_CreateXMLFiles_Tanzania_Selected_CTS]", "OurBankID", Modscan.OurBankID, "ourbranchid", Modscan.OurBranchID, "ReadDate", Modscan.cToDate, "FromDate", Modscan.cFromDate, "EJDate", Modscan.cWORKING_DATE, "ClearingCenters", "67", "AllCenters", 0, "Currency", CurrCode, "Session", 0, "xmlList", modifiedXml), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.SelectStatement, Modscan.publicDset)
                                Else
                                    Modscan.ExecuteData(Modscan.GetModify("[Proc_CreateXMLFiles_Tanzania_CTS]", "OurBankID", Modscan.OurBankID, "ourbranchid", Modscan.OurBranchID, "ReadDate", Modscan.cToDate, "FromDate", Modscan.cFromDate, "EJDate", Modscan.cWORKING_DATE, "ClearingCenters", "67", "AllCenters", 0, "Currency", CurrCode, "Session", 0), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.SelectStatement, Modscan.publicDset)
                                End If
                            Else
                                Modscan.ExecuteData(Modscan.GetModify("[Proc_CreateXMLFiles_Tanzania_CTS]", "OurBankID", Modscan.OurBankID, "ourbranchid", Modscan.OurBranchID, "ReadDate", Modscan.cToDate, "FromDate", Modscan.cFromDate, "EJDate", Modscan.cWORKING_DATE, "ClearingCenters", "67", "AllCenters", 0, "Currency", CurrCode, "Session", 0), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.SelectStatement, Modscan.publicDset)
                            End If

                            Dim EFTCrfoundRows() As DataRow
                            Select Case SystemType.ToUpper.Trim
                                Case "BR"
                                    EFTCrfoundRows = Modscan.publicDset.Tables(1).Select("TrxType ='0' AND VoucherCode <> '40'  AND ReturnCode ='00' AND BankID = '" & BankArr.Item(k).ToString.Trim() & "'  And isNull(IsGenerated, false)=false")
                                Case "BRMFO"
                                    EFTCrfoundRows = Modscan.publicDset.Tables(1).Select("TrxType ='0' AND VoucherCode <> '40'  AND ReturnCode ='00' AND BankID = '" & BankArr.Item(k).ToString.Trim() & "'  And isNull(IsGenerated, false)=false")
                                Case "BRNET"
                                    EFTCrfoundRows = Modscan.publicDset.Tables(1).Select("TrxType ='OD' AND VoucherCode <> '40' AND ReturnCode ='00' AND BankID = '" & BankArr.Item(k).ToString.Trim() & "'    AND isNull(IsGenerated, 0) = 0 ")
                                Case "BRNETOLD"
                                    EFTCrfoundRows = Modscan.publicDset.Tables(1).Select("TrxType ='OD' AND VoucherCode <> '40' AND ReturnCode ='00' AND BankID = '" & BankArr.Item(k).ToString.Trim() & "'    AND isNull(IsGenerated, 0) = 0 ")
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
                                result.AddError("Swift Code not maintained for our BankID: " & ex.Message)
                                Modscan.ErrorLog("Swift Code not maintained for this our BankID", "- EFT Generation")
                                Continue For
                            End Try
                            Try
                                If publicDTblEFTCrCopy.Rows.Count > 0 Then
                                    DestBankBIC = ""
                                    DestBankBIC = publicDTblEFTCrCopy(0)("DestinationSwiftCode").ToString()
                                End If
                            Catch ex As Exception
                                result.AddError("Swift Code not maintained for BankID: " & ex.Message)
                                Modscan.ErrorLog("Swift Code not maintained for this BankID " & publicDTblEJCopy(0)("BankID") & " EFT generation for this Bank Abort", "- EFT Generation")
                                Continue For
                            End Try
                            Dim cr As New List(Of EFTDetails)
                            For Each row As DataRow In publicDTblEFTCrCopy.Rows
                                Try
                                    Dim destBIC As String = row("DestinationSwiftCode").ToString()
                                    Dim SourceBIC As String = row("SwiftCode").ToString()
                                    Dim d As New EFTDetails
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
                                    result.AddError("Failed to process EFT " & row("TransactionMicrColumnID").ToString() & ": " & ex.Message)
                                    Modscan.ErrorLog(ex.Message, "- EFT Generation")
                                    Continue For
                                End Try
                            Next
                            If cr.Count > 0 Then
                                Dim msgId As String = BulkCredit(cr, cr(0).Currency, BIC)
                                For Each d As EFTDetails In cr
                                    Select Case SystemType.ToUpper.Trim
                                        Case "BR"
                                            strAction = "UPDATE t_TransactionMICR SET IsGenerated = 1, Reference = '" & msgId & "' WHERE ColumnID = '" & d.EFTID & "' AND TrxType ='0' AND VoucherCode <> '40'  AND BankID = '" & BankArr.Item(k) & "'"
                                        Case "BRMFO"
                                            strAction = "UPDATE t_TransactionMICR SET IsGenerated = 1, Reference = '" & msgId & "' WHERE ColumnID = '" & d.EFTID & "' AND TrxType ='0' AND VoucherCode <> '40'  AND BankID = '" & BankArr.Item(k) & "'"
                                        Case "BRNET"
                                            strAction = "UPDATE t_TrxClearing SET IsGenerated = 1, SessNo='1' ,Reference = '" & msgId & "' WHERE TrxRowID = '" & d.EFTID & "' AND TrxType ='OD' AND ReturnCodeID = '00' AND  VoucherCode <> '40'; " &
                                                        "UPDATE t_AccountTrxClearing SET IsGenerated = 1, SessNo='1' ,Reference = '" & msgId & "' WHERE TrxRowID = '" & d.EFTID & "' AND TrxType ='OD' AND ReturnCodeID = '00' AND  VoucherCode <> '40'"  'AND BankID = '" & BankArr.Item(k) & "'"
                                            'Modscan.ExecuteData(Modscan.GetModify("p_UpdateGeneratedClearingTrx", "TrxRowID", d.TrxId, "SessNo", "2", "Reference", msgId, "ReadDate", Modscan.cToDate, "FromDate", Modscan.cFromDate), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.SelectStatement, Modscan.publicDset)
                                        Case "BRNETOLD"
                                            strAction = "UPDATE t_TrxClearing SET IsGenerated = 1, SessNo='1' ,Reference = '" & msgId & "' WHERE TrxRowID = '" & d.EFTID & "' AND TrxType ='OD' AND VoucherCode <> '40'  AND BankID = '" & BankArr.Item(k) & "'"
                                    End Select
                                    If Not String.IsNullOrEmpty(strAction) Then
                                        Modscan.ExecuteData(strAction, Nothing, Modscan.dataExecTypes.ExecTypeNonQuery, Modscan.queryType.SelectStatement)
                                    End If
                                Next
                            End If
                        Next
                        publicDTblEFTCrCopy.Clear()
                    Case FileType.EftReturn
                        Counter = 0
                        'MessageBox.Show("imeingia")
                        For k As Int32 = 0 To BankArr.Count - 1
                            'For MsgIdK As Int32 = 0 To MsgIdArr.Count - 1
                            Try
                                Counter = Counter + 1
                                Dim i As Integer = 0
                                Dim amt As Decimal = 0
                                Dim CreateFile As String = Session & Now.ToString(".yyyyMMdd.HHmm.") & BankArr.Item(k) & "." & Session.Substring(1)
                                Dim BIC As String = ""
                                'EFT Cr Reject

                                'If ls IsNot Nothing Then
                                '    If ls.Count > 0 Then
                                '        Modscan.ExecuteData(Modscan.GetModify("[Proc_CreateXMLFiles_Tanzania_Selected_CTS]", "OurBankID", Modscan.OurBankID, "ourbranchid", Modscan.OurBranchID, "ReadDate", Modscan.cToDate, "FromDate", Modscan.cFromDate, "EJDate", Modscan.cWORKING_DATE, "ClearingCenters", "67", "AllCenters", 0, "Currency", CurrCode, "Session", 2, "xmlList", modifiedXml), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.SelectStatement, Modscan.publicDset)
                                '    Else
                                '        Modscan.ExecuteData(Modscan.GetModify("Proc_CreateXMLFiles_Tanzania_CTS", "OurBankID", Modscan.OurBankID, "ourbranchid", Modscan.OurBranchID, "ReadDate", Modscan.cToDate, "FromDate", Modscan.cFromDate, "EJDate", Modscan.cWORKING_DATE, "ClearingCenters", "67", "AllCenters", 0, "Currency", CurrCode, "Session", 2), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure, Modscan.publicDset)

                                '    End If
                                'Else
                                '    Modscan.ExecuteData(Modscan.GetModify("Proc_CreateXMLFiles_Tanzania_CTS", "OurBankID", Modscan.OurBankID, "ourbranchid", Modscan.OurBranchID, "ReadDate", Modscan.cToDate, "FromDate", Modscan.cFromDate, "EJDate", Modscan.cWORKING_DATE, "ClearingCenters", "67", "AllCenters", 0, "Currency", CurrCode, "Session", 2), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure, Modscan.publicDset)
                                'End If

                                Dim EFTCrfoundRows() As DataRow
                                'MessageBox.Show(MsgIdArr.Item(MsgIdK))
                                Select Case SystemType.ToUpper.Trim
                                    Case "BR"
                                        EFTCrfoundRows = Modscan.publicDset.Tables(1).Select("TrxType ='0' AND VoucherCode <> '40' AND ReturnCode <>'00' AND  BankID = '" & BankArr.Item(k) & "'  And isNull(IsGenerated, 0)=0")
                                    Case "BRMFO"
                                        EFTCrfoundRows = Modscan.publicDset.Tables(1).Select("TrxType ='0' AND VoucherCode <> '40' AND ReturnCode <>'00' AND  BankID = '" & BankArr.Item(k) & "'  And isNull(IsGenerated, 0)=0")
                                    Case "BRNET"
                                        'EFTCrfoundRows = Modscan.publicDset.Tables(1).Select("TrxType ='OD' AND VoucherCode <> '40' AND ReturnCode <>'00' AND BankID = '" & BankArr.Item(k) & "'  And  OrgnlMsgId =  '" & MsgIdArr.Item(MsgIdK) & "' And  IsGenerated=0")
                                        EFTCrfoundRows = Modscan.publicDset.Tables(1).Select("TrxType ='OD' AND VoucherCode <> '40' AND ReturnCode <>'00' AND BankID = '" & BankArr.Item(k) & "'   AND isNull(IsGenerated, 0) = 0 ")
                                    Case "BRNETOLD"
                                        EFTCrfoundRows = Modscan.publicDset.Tables(1).Select("TrxType ='OD' AND VoucherCode <> '40' AND ReturnCode <>'00' AND BankID = '" & BankArr.Item(k) & "'  And isNull(IsGenerated, 0)=0")
                                End Select
                                publicDTblEFTCrCopy = Modscan.publicDset.Tables(1).Clone()
                                'MessageBox.Show("Imepata unpaid EFTs rows : " & EFTCrfoundRows.Length)
                                For j As Int32 = 0 To EFTCrfoundRows.Length - 1
                                    publicDTblEFTCrCopy.ImportRow(EFTCrfoundRows(j))
                                Next
                                Modscan.publicDset.Tables(1).Clear()
                                Try
                                    If publicDTblEFTCrCopy.Rows.Count > 0 Then
                                        BIC = publicDTblEFTCrCopy(0)("SwiftCode")
                                    End If
                                Catch ex As Exception
                                    result.AddError("Swift Code not maintained for our BankID: " & ex.Message)
                                    Modscan.ErrorLog("Swift Code not maintained for this our BankID", "- EFT Unpaid Generation")
                                    Continue For
                                End Try
                                Try
                                    If publicDTblEFTCrCopy.Rows.Count > 0 Then
                                        DestBankBIC = ""
                                        DestBankBIC = publicDTblEFTCrCopy(0)("DestinationSwiftCode").ToString()
                                    End If
                                Catch ex As Exception
                                    result.AddError("Swift Code not maintained for BankID: " & ex.Message)
                                    Modscan.ErrorLog("Swift Code not maintained for this BankID " & publicDTblEJCopy(0)("BankID"), "- EFT Unpaid Generation")
                                    Continue For
                                End Try
                                Dim cr As New List(Of EFTDetails)
                                For Each row As DataRow In publicDTblEFTCrCopy.Rows
                                    Try
                                        Dim destBIC As String = row("DestinationSwiftCode").ToString()
                                        Dim SourceBIC As String = row("SwiftCode").ToString()
                                        Dim d As New EFTDetails
                                        d.Amount = FormatNumber(row("Amount"), 2)
                                        d.Currency = CurrCode
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
                                        d.OrgnTrxID = row("TrxID").ToString()
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
                                        d.DTwnNm = row("DTwnNm").ToString()
                                        d.DCNm = row("DCNm").ToString()
                                        d.CCNm = row("CCNm").ToString()
                                        d.DbtrAcct = row("DbtrAcct").ToString()
                                        d.CdtrAcct = row("CdtrAcct").ToString()
                                        d.UstrdColD = row("UstrdColD").ToString()
                                        d.OrgnlEndToEnd = row("OrgnlEndToEnd").ToString()
                                        d.SvcLvl = row("SvcLvl").ToString()
                                        d.LclInstrm = row("LclInstrm").ToString()
                                        d.CtgyPurp = row("CtgyPurp").ToString()
                                        d.OrgnlIntrBkSttlmDt = row("OrgnlIntrBkSttlmDt").ToString()
                                        d.ReqdColltnDt = IIf(row("ReqdColltnDt").ToString() = "Jan  1 1900 12:00AM", "", row("ReqdColltnDt").ToString())
                                        cr.Add(d)
                                    Catch ex As Exception
                                        result.AddError("Failed to process EFT Unpaid " & row("TransactionMicrColumnID").ToString() & ": " & ex.Message)
                                        Modscan.ErrorLog(ex.Message, "- EFT Unpaids Generation")
                                        Continue For
                                    End Try
                                Next
                                If cr.Count > 0 Then
                                    Dim msgId As String = CancelCredit(cr, cr(0).Currency, BIC, Modscan.cWORKING_DATE)
                                    For Each d As EFTDetails In cr
                                        Select Case SystemType.ToUpper.Trim
                                            Case "BR"
                                                strAction = "UPDATE t_TransactionMICR SET IsGenerated = 1, Remarks = '" & msgId & "' WHERE ColumnID = '" & d.TrxId & "' AND TrxType ='0' AND VoucherCode <> '40'  AND BankID = '" & BankArr.Item(k) & "'"
                                            Case "BRMFO"
                                                strAction = "UPDATE t_TransactionMICR SET IsGenerated = 1, Remarks = '" & msgId & "' WHERE ColumnID = '" & d.TrxId & "' AND TrxType ='0' AND VoucherCode <> '40' AND BankID = '" & BankArr.Item(k) & "'"
                                            Case "BRNET"
                                                strAction = "UPDATE t_trxClearing SET IsGenerated = 1, SessNo='2' ,Reference = '" & msgId & "' WHERE TrxRowID = '" & d.TrxId & "' AND TrxType ='OD' AND ReturnCodeID <> '00' AND VoucherCode <> '40'  AND isNull(IsGenerated,0) = 0; " &
                                                            "UPDATE t_AccountTrxClearing SET IsGenerated = 1, SessNo='2' ,Reference = '" & msgId & "' WHERE TrxRowID = '" & d.TrxId & "' AND TrxType ='OD' AND ReturnCodeID <> '00' AND VoucherCode <> '40'  AND isNull(IsGenerated,0) = 0"
                                                'If Not Modscan.ExecuteData(Modscan.GetModify("p_UpdateGeneratedClearingTrx", "TrxRowID", d.TrxId, "SessNo", "2", "Reference", msgId, "ReadDate", Modscan.cToDate, "FromDate", Modscan.cFromDate), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.SelectStatement, Modscan.publicDset) Then
                                                '    result.AddError("Failed to update status for TrxID " & d.TrxId & ": " & Modscan.LastErrorMessage)
                                                'End If
                                            Case "BRNETOLD"
                                                strAction = "UPDATE t_trxClearing SET IsGenerated = 1, SessNo='1' ,Reference = '" & msgId & "' WHERE TrxRowID = '" & d.TrxId & "' AND TrxType ='OD' AND VoucherCode <> '40'  AND BankID = '" & BankArr.Item(k) & "'"
                                        End Select
                                        If Not String.IsNullOrEmpty(strAction) Then
                                            Modscan.ExecuteData(strAction, Nothing, Modscan.dataExecTypes.ExecTypeNonQuery, Modscan.queryType.SelectStatement)
                                        End If
                                    Next
                                End If
                            Catch ex As Exception
                                result.AddError("Unexpected error in EFT Unpaid Generation loop: " & ex.Message)
                                Modscan.ErrorLog(ex.Message, "- EFT Unpaid Generation")
                                Continue For
                            End Try
                            'Next
                        Next
                        publicDTblEFTCrCopy.Clear()
                    Case FileType.DDReturn

                    Case FileType.Messages
                End Select

                result.Success = (result.MessageType <> MessageType.Error)
                If result.Success Then
                    result.Message = "File generation completed successfully."
                    result.MessageType = MessageType.Success
                Else
                    result.Message = "File generation completed with errors."
                End If

                publicDTblBankCopy.Clear()
                Return result
            Catch ex As Exception
                result.AddError("Critical error in GenerateTZFiles: " & ex.Message)
                Return result
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
        Private Shared Function BulkCredit(ByVal l As List(Of EFTDetails), ByVal ccy As String, ByVal BIC As String) As String
            Dim dAmt As Decimal = 0
            For Each itm As EFTDetails In l
                dAmt += CDec(itm.Amount)
            Next
            Dim stCurrCode As String = ""
            If ccy = "0" Or ccy = "TZS" Then
                stCurrCode = "TZS"
            ElseIf ccy = "1" Or ccy = "USD" Then
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
            Dim msgId As String = "CT" & l(0).DestBIC & Modscan.WORKING_DATE.ToString("ddMMyyyy") & TimeSec & stCurrCode & Modscan.Sess
            Dim Filename As String = l(0).DestBIC & Now.ToString("ddMMyyyy") & Modscan.Sess & stCurrCode & l(0).SourceBankID & ".i"
            Dim doc As New cr.Document()
            Dim grpHdr As New BrClearing.Common.BRISO20022CT812.GroupHeader33()
            grpHdr.MsgId = "OD/" & l(0).DestBIC & Modscan.WORKING_DATE.ToString("ddMMyy") & "/" & stCurrCode & Modscan.Sess & TimeSec
            grpHdr.CreDtTm = xDt
            grpHdr.NbOfTxs = l.Count
            grpHdr.TtlIntrBkSttlmAmt = New cr.ActiveCurrencyAndAmount() With {.Ccy = stCurrCode, .Value = Decimal.Round(CDec(amt), 2)} 'Truncation cr.ActiveCurrencyCode.TZS
            grpHdr.IntrBkStDate = CDate(sDt)
            grpHdr.SttlmInf = New cr.SettlementInformation13() With {
            .SttlmMtd = cr.SettlementMethod1Code.CLRG
        }
            '.ClrSys = New cr.ClearingSystemIdentification3Choice() With {.Item = cr.ItemChoiceType1.ACH}, _
            grpHdr.InstgAgt = New cr.BranchAndFinancialInstitutionIdentification4 With {.FinInstnId = New cr.FinancialInstitutionIdentification7() With {.BIC = BIC}}
            doc.FIToFICstmrCdtTrf.GrpHdr = grpHdr
            For Each itm As EFTDetails In l
                Dim sBIC As String = itm.SourceBIC
                Dim dBIC As String = itm.DestBIC
                amt = FormatNumber(itm.Amount, 2, Microsoft.VisualBasic.TriState.True, Microsoft.VisualBasic.TriState.True, Microsoft.VisualBasic.TriState.False)
                Dim cdtTxn As New cr.CreditTransferTransactionInformation11()
                cdtTxn.PmtId = New cr.PaymentIdentification3() _
           With {.EndToEndId = itm.Reference, .InstrId = itm.Reference, .TxId = itm.EFTID}
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
                    Dim Destinationfile As String = DestZippedFolderLoc & "\" & Path.GetFileName(fullpath)
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
                            MessOut = SignFiles_PKCS(fullpath.Trim(), DestSignedFolderLoc.Trim(), "TZ", CertPass.Trim(), "i")
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
                            MessageBox.Show("Failed Signing. " & Path.GetFileName(fullpath) & " : " & MessOut)
                            Modscan.ErrorLog(Path.GetFileName(fullpath) & " : " & MessOut, "- Signing ")
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

        Private Shared Function CancelCredit(ByVal l As List(Of EFTDetails), ByVal ccy As String, ByVal BIC As String, ByVal SttlDate As String) As String
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
                For Each itm As EFTDetails In l
                    dAmt += CDec(itm.Amount)
                Next

                ' BIC = BIC.Substring(0, BIC.Length - 3)
                Dim amt As String = FormatNumber(dAmt, 2, Microsoft.VisualBasic.TriState.True, Microsoft.VisualBasic.TriState.True, Microsoft.VisualBasic.TriState.False)
                doc.PmtRtr.GrpHdr.MsgId = msgId
                doc.PmtRtr.GrpHdr.CreDtTm = SttlDate  '.Item.FinInstnId.BIC = Left(l(0).DestBIC, 8)
                doc.PmtRtr.GrpHdr.NbOfTxs = l.Count '.Item.FinInstnId.BIC = Left(BIC, 8)
                'doc.PmtRtr.GrpHdr.TtlBkSttlmAmt = xDt
                doc.PmtRtr.GrpHdr.TtlRtrdIntrBkSttlmAmt = New pcr.ActiveCurrencyAndAmount() With {.Ccy = pcr.ActiveCurrencyCode.TZS, .Value = CDec(dAmt)}
                doc.PmtRtr.GrpHdr.IntrBkSttlmDt = SttlDate
                doc.PmtRtr.GrpHdr.SttlmInf = New pcr.SettlementInformation13() With {.ClrSys = New pcr.ClearingSystemIdentification3Choice() With {.Item = pcr.ClearingSystemIdentification.ACH}, .SttlmMtd = pcr.SettlementMethod1Code.CLRG}
                doc.PmtRtr.GrpHdr.InstgAgt = New pcr.FinancialInstitution4() With {.FinInstnId = New pcr.FinancialInstitutionIdentification7() With {.BIC = BIC}}
                For Each txn As EFTDetails In l
                    Dim sAmount As String = FormatNumber(txn.Amount, 2, Microsoft.VisualBasic.TriState.False, Microsoft.VisualBasic.TriState.False, Microsoft.VisualBasic.TriState.False).Replace(",", "")
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

                        Dim ReqdColltnDate As Date = IIf(String.IsNullOrEmpty(Convert.ToDateTime(txn.ReqdColltnDt)), Convert.ToDateTime(txn.OrgnlIntrBkSttlmDt), Convert.ToDateTime(txn.ReqdColltnDt))
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

        Private Shared Function BulkDebit(ByVal l As List(Of EFTDetails), ByVal ccy As String, ByVal BIC As String) As String
            'Dim dAmt As Decimal = 0
            'Dim ValDays As Integer = Val(GetChqPntParam("DD_VALUE_DAYS", "GENERALPARAMS"))
            'For Each itm As EFTDetails In l
            '    dAmt += CDec(itm.Amount)
            'Next
            'BIC = BIC.Substring(0, BIC.Length - 3)
            'Dim amt As String = FormatNumber(dAmt, 2, Microsoft.VisualBasic.TriState.True, Microsoft.VisualBasic.TriState.True, Microsoft.VisualBasic.TriState.False)
            'Dim sDt As DateTime = Modscan.WORKING_DATE.ToString("dd-MMM-yyyy")
            'Dim STm As String = Now.ToString("HH:mm")
            'Dim xDt As Date = CDate(sDt & " " & STm)
            'Dim dtClearDate As Date = IsClearingDate(xDt.AddDays(ValDays))
            'Dim msgId As String = "DR" & Left(l(0).DestBIC, 8) & xDt.ToString("yyyyMMddHHmmss")
            'Dim doc As New dr.Document()
            'Dim grpHdr As New dr.GroupHeader34()
            'grpHdr.MsgId = msgId
            'grpHdr.CreDtTm = xDt
            'grpHdr.NbOfTxs = l.Count
            'grpHdr.TtlIntrBkSttlmAmt = New dr.ActiveCurrencyAndAmount() With {.Ccy = dr.ActiveCurrencyCode.TZS, .Value = CDec(amt)}
            'grpHdr.IntrBkSttlmDt = dtClearDate
            'grpHdr.SttlmInf = New dr.SettlementInformation14() With { _
            '    .ClrSys = New dr.ClearingSystemIdentification3Choice() With {.Item = dr.ClearingSystemIdentification.ACH}, _
            '    .SttlmMtd = dr.SettlementMethod1Code.CLRG _
            '}
            'grpHdr.InstgAgt = New dr.FinancialInstitution4() With {.FinInstnId = New dr.FinancialInstitutionIdentification7() With {.BIC = BIC}}
            'doc.FIToFICstmrDrctDbt.GrpHdr = grpHdr
            'For Each itm As EFTDetails In l
            '    Dim sBIC As String = itm.SourceBIC.Substring(0, itm.SourceBIC.Length - 3)
            '    Dim dBIC As String = itm.DestBIC.Substring(0, itm.DestBIC.Length - 3)
            '    amt = FormatNumber(itm.Amount, 2, Microsoft.VisualBasic.TriState.True, Microsoft.VisualBasic.TriState.True, Microsoft.VisualBasic.TriState.False)
            '    Dim dbtTxn As New dr.DirectDebitTransactionInformation10()
            '    dbtTxn.PmtId.EndToEndId = itm.Reference
            '    dbtTxn.PmtId.InstrId = itm.Reference
            '    dbtTxn.PmtId.TxId = itm.EFTID
            '    dbtTxn.PmtTpInf.SvcLvl.Item = dr.ServiceLevel3Code.SEPA
            '    dbtTxn.PmtTpInf.LclInstrm.Item = "B2B"
            '    dbtTxn.IntrBkSttlmAmt = New dr.ActiveCurrencyAndAmount() With {.Ccy = dr.ActiveCurrencyCode.TZS, .Value = CDec(amt)}
            '    dbtTxn.ChrgBr = dr.ChargeBearerType1Code.SLEV
            '    dbtTxn.ReqdColltnDt = dtClearDate
            '    dbtTxn.DrctDbtTx.MndtRltdInf.DtOfSgntr = Modscan.WORKING_DATE
            '    dbtTxn.DrctDbtTx.MndtRltdInf.MndtId = itm.RemittanceInfo
            '    dbtTxn.DrctDbtTx.MndtRltdInf.AmdmntInd = False
            '    dbtTxn.DrctDbtTx.MndtRltdInf.AmdmntIndSpecified = True
            '    dbtTxn.DrctDbtTx.CdtrSchmeId.Id.Item.Othr.Id = itm.RemittanceInfo
            '    dbtTxn.Dbtr = New dr.PartyIdentification33() With {.Nm = itm.RemitterName, .Id = New dr.Party6Choice() With {.Item = New dr.OrganisationIdentification4() With {.Item = sBIC}}}
            '    dbtTxn.DbtrAcct = New dr.CashAccount17() With {.Id = New dr.AccountIdentification4Choice() With {.Item = itm.RemitterAcc}}
            '    dbtTxn.DbtrAgt = New dr.FinancialInstitution4() With {.FinInstnId = New dr.FinancialInstitutionIdentification7() With {.BIC = dBIC}}
            '    dbtTxn.CdtrAgt = New dr.FinancialInstitution4() With {.FinInstnId = New dr.FinancialInstitutionIdentification7() With {.BIC = sBIC}}
            '    dbtTxn.Cdtr = New dr.PartyIdentification33() With {.Nm = itm.BeneficiaryName}
            '    dbtTxn.CdtrAcct = New dr.CashAccount17() With {.Id = New dr.AccountIdentification4Choice() With {.Item = itm.BeneficiaryAcc}}
            '    dbtTxn.RmtInf.Item = itm.RemittanceInfo
            '    doc.FIToFICstmrDrctDbt.DrctDbtTxInf.Add(dbtTxn)
            'Next
            'If Directory.Exists(strFileLocation) Then
            '    Dim fullpath As String = Path.Combine(strFileLocation, msgId & ".i")
            '    Dim ex As New Exception()
            '    If doc.SaveToFile(fullpath, ex) Then
            '        Dim xDoc As XDocument = XDocument.Load(fullpath)
            '        Dim k As List(Of XAttribute) = xDoc.Root.Attributes().ToList()
            '        Dim xsd As XAttribute = k(1)
            '        If xDoc.Root.HasAttributes Then xDoc.Root.Attribute(xsd.Name).Remove()
            '        Dim m As List(Of XElement) = xDoc.Descendants().ToList()
            '        Dim xCreTm As XElement = m(4)
            '        Dim xStmDt As XElement = m(7)
            '        xDoc.Descendants().ToList()(4).SetValue(CDate(xCreTm.Value).ToString("yyyy-MM-ddTHH:mm:sszzz"))
            '        xDoc.Descendants().ToList()(7).SetValue(CDate(xStmDt.Value).ToString("yyyy-MM-dd"))
            '        For i As Integer = 0 To m.Count - 1
            '            Dim xE As XElement = m(i)
            '            If xE.Name.LocalName = "ReqdColltnDt" Or xE.Name.LocalName = "DtOfSgntr" Then
            '                xDoc.Descendants().ToList()(i).SetValue(CDate(xE.Value).ToString("yyyy-MM-ddzzz"))
            '            End If
            '        Next
            '        xDoc.Descendants().Where(Function(p) p.IsEmpty Or String.IsNullOrEmpty(p.Value)).Remove()
            '        xDoc.Root.Attributes().Reverse()
            '        xDoc.Save(fullpath, SaveOptions.None)
            '        If Sign Then SignFile(fullpath)
            '        If Not RecExists("SELECT MSGID FROM CLEARINGFILES WHERE MSGID = '" & msgId & "'") Then
            '            strAction = "INSERT INTO CLEARINGFILES(FILEPATH, MSGID) VALUES('" & fullpath & "', '" & msgId & "')"
            '            ExecuteData(strAction, Nothing, dataExecTypes.ExecTypeNonQuery)
            '        End If
            '        AuditAction = "Created Clearing File [" & msgId & "] At: " & Now.ToString("dd/MM/yyyy HH:mm:ss")
            '        AuditTrail(Functions.StrLoginId.ToUpper.Trim, Date.Now, AuditAction, Now, "ClearingFiles", audAuditTypes.audOperations)
            '        Return msgId
            '    End If
            'End If
            'Return Nothing
        End Function

        Private Shared Function DDReject(ByVal l As List(Of DDDetail), ByVal ccy As String, ByVal BIC As String) As String
            'Dim RegX As New Regex("[^A-Za-z0-9]")
            'Dim sDt As String = Modscan.WORKING_DATE.ToString("dd-MMM-yyyy")
            'Dim STm As String = Now.ToString("HH:mm")
            'Dim xDt As Date = CDate(sDt & " " & STm)
            'Dim msgId As String = "DRX" & Left(l(0).sBIC, 8) & xDt.ToString("yyyyMMddHHmmss")
            'Dim doc As New res.Document()
            'doc.FIToFIPmtStsRpt.GrpHdr.MsgId = msgId
            'doc.FIToFIPmtStsRpt.GrpHdr.CreDtTm = xDt
            'doc.FIToFIPmtStsRpt.GrpHdr.InstgAgt.FinInstnId.BIC = Left(BIC, 8)
            'doc.FIToFIPmtStsRpt.OrgnlGrpInfAndSts.OrgnlMsgId = l(0).MsgId
            'doc.FIToFIPmtStsRpt.OrgnlGrpInfAndSts.OrgnlMsgNmId = "pacs.003.001.02"
            'For Each d As DDDetail In l
            '    Dim sAmount As String = FormatNumber(d.Amount, 2, Microsoft.VisualBasic.TriState.False, Microsoft.VisualBasic.TriState.False, Microsoft.VisualBasic.TriState.False).Replace(",", "")
            '    Dim txnInf As New res.PaymentTransactionInformation26()
            '    txnInf.StsId = "CX" & d.InstrId.Trim()
            '    txnInf.OrgnlInstrId = d.InstrId.Trim()
            '    txnInf.OrgnlEndToEndId = d.EndToEndId.Trim()
            '    txnInf.OrgnlTxId = d.TxId.Trim()
            '    txnInf.OrgnlTxRef.IntrBkSttlmAmt.Ccy = res.ActiveCurrencyCode.TZS
            '    txnInf.OrgnlTxRef.IntrBkSttlmAmt.Value = d.Amount
            '    txnInf.OrgnlTxRef.IntrBkSttlmDtSpecified = True
            '    txnInf.OrgnlTxRef.ReqdColltnDtSpecified = True
            '    txnInf.OrgnlTxRef.ReqdColltnDt = d.Collection
            '    txnInf.OrgnlTxRef.IntrBkSttlmDt = d.Settlement
            '    txnInf.StsRsnInf.Orgtr.Item = New res.Party6Choice() With {.Item = New res.OrganisationIdentification4() With {.Item = d.dBIC}}
            '    txnInf.StsRsnInf.Rsn.Item = d.Retcode
            '    txnInf.TxSts = res.TransactionIndividualStatus3Code.RJCT
            '    txnInf.TxStsSpecified = True
            '    txnInf.OrgnlTxRef.SttlmInf.SttlmMtd = res.SettlementMethod1Code.CLRG
            '    txnInf.OrgnlTxRef.SttlmInf.ClrSys.Item = res.ClearingSystemIdentification.ACH
            '    txnInf.OrgnlTxRef.PmtTpInf.SvcLvl.Item = res.ServiceLevel3Code.SEPA
            '    txnInf.OrgnlTxRef.PmtTpInf.LclInstrm.Item = "B2B"
            '    txnInf.OrgnlTxRef.PmtTpInf.SeqTp = res.SequenceType1Code.FRST
            '    txnInf.OrgnlTxRef.PmtTpInf.SeqTpSpecified = True
            '    txnInf.OrgnlTxRef.Dbtr.Nm = d.DrName
            '    txnInf.OrgnlTxRef.Dbtr.Id.Item = New res.OrganisationIdentification4() With {.Item = d.sBIC}
            '    txnInf.OrgnlTxRef.DbtrAcct.Id.Item = d.DrAcc
            '    txnInf.OrgnlTxRef.DbtrAgt.FinInstnId.BIC = d.dBIC
            '    txnInf.OrgnlTxRef.CdtrAgt.FinInstnId.BIC = d.sBIC
            '    txnInf.OrgnlTxRef.Cdtr.Nm = d.CrName
            '    txnInf.OrgnlTxRef.CdtrAcct.Id.Item = d.CrAcc
            '    txnInf.OrgnlTxRef.RmtInf.Item = d.Remittance
            '    txnInf.OrgnlTxRef.MndtRltdInf.AmdmntInd = d.Mandate.AmdmntInd
            '    txnInf.OrgnlTxRef.MndtRltdInf.AmdmntIndSpecified = True
            '    txnInf.OrgnlTxRef.MndtRltdInf.DtOfSgntr = d.Mandate.DtOfSgntr
            '    txnInf.OrgnlTxRef.MndtRltdInf.MndtId = d.Mandate.MndtId
            '    doc.FIToFIPmtStsRpt.TxInfAndSts.Add(txnInf)
            'Next
            'If Directory.Exists(strFileLocation) Then
            '    Dim fullpath As String = Path.Combine(strFileLocation, msgId & ".i")
            '    Dim ex As New Exception()
            '    If doc.SaveToFile(fullpath, ex) Then
            '        Dim xDoc As XDocument = XDocument.Load(fullpath)
            '        Dim k As List(Of XAttribute) = xDoc.Root.Attributes().ToList()
            '        Dim xsd As XAttribute = k(1)
            '        If xDoc.Root.HasAttributes Then xDoc.Root.Attribute(xsd.Name).Remove()
            '        Dim xCreTm As XElement = xDoc.Descendants().ToList()(4)
            '        xDoc.Descendants().ToList()(4).SetValue(CDate(xCreTm.Value).ToString("yyyy-MM-ddTHH:mm:sszzz"))
            '        xDoc.Descendants().Where(Function(p) p.IsEmpty Or String.IsNullOrEmpty(p.Value)).Remove()
            '        xDoc.Root.Attributes().Reverse()
            '        xDoc.Save(fullpath, SaveOptions.None)
            '        If Sign Then SignFile(fullpath)
            '        If Not RecExists("SELECT MSGID FROM CLEARINGFILES WHERE MSGID = '" & msgId & "'") Then
            '            strAction = "INSERT INTO CLEARINGFILES(FILEPATH, MSGID) VALUES('" & fullpath & "', '" & msgId & "')"
            '            ExecuteData(strAction, Nothing, dataExecTypes.ExecTypeNonQuery)
            '        End If
            '        AuditAction = "Created Clearing File [" & msgId & "] At: " & Now.ToString("dd/MM/yyyy HH:mm:ss")
            '        AuditTrail(Functions.StrLoginId.ToUpper.Trim, Date.Now, AuditAction, Now, "ClearingFiles", audAuditTypes.audOperations)
            '        Return msgId
            '    End If
            'End If
            Return Nothing
        End Function

        Private Shared Function InterBank(ByVal d As EFTDetails) As String
            Dim Id As String = d.EFTID.Substring(d.EFTID.Length - 3).PadLeft(10, "0")
            Dim dt As String = Modscan.WORKING_DATE.ToString("yyMMdd")
            Dim tm As String = Now.ToString("HHmm")
            Dim sBIC As String = d.SourceBIC.Substring(0, d.SourceBIC.Length - 3)
            Dim dBIC As String = d.DestBIC.Substring(0, d.DestBIC.Length - 3)
            Dim strDetail As String = "{1:F01NBETETABAXXX" & Id & "}{2:O202" & tm & dt & sBIC & "XXXX" & Id & dt & tm & "N}{4:" & vbCrLf
            strDetail += ":20:" & d.EFTID & vbCrLf
            'strDetail += ":21:" & IIf(d.RemittanceInfo.Trim() = "", "NOREF", d.RemittanceInfo) & vbCrLf
            strDetail += ":21:" & d.EFTID & vbCrLf
            strDetail += ":32A:" & dt & d.ISOCurrency & d.Amount.Replace(",", "").Replace(".", ",") & vbCrLf
            strDetail += ":52A:" & sBIC & vbCrLf
            strDetail += ":58A:/" & d.BeneficiaryAcc & vbCrLf & dBIC & vbCrLf
            Dim sRef As String = IIf(d.RemittanceInfo.Trim() = "", "NOREF", d.RemittanceInfo)
            SplitString(sRef, 35)
            strDetail += ":72:" & sRef & vbCrLf & "-}"
            InterBank = strDetail
        End Function

        Private Shared Function SingleRTGS(ByVal d As EFTDetails) As String
            Dim Id As String = d.EFTID.Substring(d.EFTID.Length - 3).PadLeft(10, "0")
            Dim dt As String = Modscan.WORKING_DATE.ToString("yyMMdd")
            Dim tm As String = Now.ToString("HHmm")
            Dim sBIC As String = d.SourceBIC.Substring(0, d.SourceBIC.Length - 3)
            Dim dBIC As String = d.DestBIC.Substring(0, d.DestBIC.Length - 3)
            If d.BeneficiaryName.Length > 34 Then d.BeneficiaryName = d.BeneficiaryName.Substring(0, 34)
            If d.RemitterName.Length > 34 Then d.RemitterName = d.RemitterName.Substring(0, 34)
            Dim strDetail As String = "{1:F01NBETETABAXXX" & Id & "}{2:O103" & tm & dt & sBIC & "XXXX" & Id & dt & tm & "N}{4:" & vbCrLf
            strDetail += ":20:" & d.EFTID & vbCrLf
            strDetail += ":23B:CRED" & vbCrLf
            strDetail += ":23E:SDVA" & vbCrLf
            strDetail += ":32A:" & dt & d.ISOCurrency & d.Amount.Replace(",", "").Replace(".", ",") & vbCrLf
            strDetail += ":33B:" & d.ISOCurrency & d.Amount.Replace(",", "").Replace(".", ",") & vbCrLf
            strDetail += ":50K:/" & d.RemitterAcc & vbCrLf & d.RemitterName & vbCrLf & d.DestAcc & vbCrLf
            strDetail += ":52A:" & sBIC & vbCrLf
            strDetail += ":57A:" & dBIC & vbCrLf
            strDetail += ":59:/" & d.BeneficiaryAcc & vbCrLf & d.BeneficiaryName & vbCrLf & d.SourceAcc & vbCrLf
            strDetail += ":71A:SHA" & vbCrLf
            Dim sRef As String = d.RemittanceInfo
            SplitString(sRef, 34)
            strDetail += ":72:/" & sRef & vbCrLf & "-}"
            SingleRTGS = strDetail
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

        Private Shared Function FreeFormat(ByVal d As EFTDetails) As String
            Dim Id As String = d.EFTID
            Id = Id.Substring(Id.Length - 3).PadLeft(10, "0"c)
            Dim dt As String = d.ValueDate.ToString("yyMMdd")
            Dim tm As String = Now.ToString("HHmm")
            Dim sBIC As String = d.SourceBIC.Substring(0, d.SourceBIC.Length - 3)
            Dim strDetail As String = ("{1:F01" & d.DestBIC & Id & "}{2:O999" & tm & dt & sBIC & "XXXX" & Id & dt & tm & "N}{4:") + Environment.NewLine
            strDetail += ":20:" + d.EFTID + Environment.NewLine
            strDetail += ":21:999" + Environment.NewLine
            Dim sRef As String = d.Reference + " " + d.RemittanceInfo
            SplitString(sRef, 34)
            strDetail += (":79:" & sRef) + Environment.NewLine & "-}"
            Return strDetail
        End Function

        Private Shared Function BalanceRequest(ByVal d As EFTDetails) As String
            Dim Id As String = d.EFTID
            Id = Id.Substring(Id.Length - 3).PadLeft(10, "0"c)
            Dim dt As String = d.ValueDate.ToString("yyMMdd")
            Dim tm As String = Now.ToString("HHmm")
            Dim sBIC As String = d.SourceBIC.Substring(0, d.SourceBIC.Length - 3)
            Dim strDetail As String = ("{1:F01NBETETABAXXX" & Id & "}{2:O920" & tm & dt & sBIC & "XXXX" & Id & dt & tm & "N}{4:") + Environment.NewLine
            strDetail += ":20:" + d.EFTID + Environment.NewLine
            strDetail += ":12:941" + Environment.NewLine
            strDetail += ":25:" + d.Reference + Environment.NewLine & "-}"
            Return strDetail
        End Function

        Private Shared Function SISTransaction(ByVal det As ChequeDetails) As Boolean
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
            strDetail += "Currency\ Code=" & IIf(det.CurrencyCode = "TZS" Or det.CurrencyCode = "0", "0", "1") & vbNewLine
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

        Private Shared Function BulkCheques(ByVal det As List(Of ChequeDetails), ByVal ccy As String, ByVal amt As Decimal, ByVal BIC As String) As String
            Dim sDt As String = Modscan.FDATE.ToString("dd-MMM-yyyy")
            Dim STm As String = Now.ToString("HH:mm")
            Dim TimeSec As String = Now.ToString("HHmmss")
            Dim MsgIdDt As String = Modscan.WORKING_DATE.ToString("ddMMyyyy")
            Dim xDt As Date = CDate(sDt & " " & STm)
            Dim insDt As Date = CDate(sDt)
            Dim ICounter As Int16 = 0
            Dim stCurrCode As String = ""
            If ccy = "0" Or ccy = "TZS" Then
                stCurrCode = "TZS"
                ccy = 0
            ElseIf ccy = "1" Or ccy = "USD" Then
                stCurrCode = "USD"
                ccy = 1
            ElseIf ccy = "2" Or ccy = "GBP" Then
                stCurrCode = "GBP"
                ccy = 3
            ElseIf ccy = "3" Or ccy = "EUR" Then
                stCurrCode = "EUR"
                ccy = 3
            ElseIf ccy = "5" Or ccy = "KES" Then
                stCurrCode = "KES"
                ccy = 5
            Else
                stCurrCode = "UGX"
                ccy = 6
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
            ImageUniqueName = det(0).BankBIC & Modscan.FDATE.ToString("ddMMyyyy") & stCurrCode
            'grpHdr.MsgId = "OC/" & Modscan.FDATE.ToString("ddMMyy") & "/" & ccy & det(0).BankCode.Substring(0, 2)
            grpHdr.MsgId = "OC" & det(0).BankBIC & Modscan.FDATE.ToString("ddMMyy") & Modscan.Sess & TimeSec
            grpHdr.CreDtTm = xDt
            grpHdr.NbOfTxs = det.Count
            grpHdr.TtlIntrBkSttlmAmt = New ch.ActiveCurrencyAndAmount() With {.Ccy = ccy, .Value = Decimal.Round(CDec(sAmount), 2)}
            grpHdr.IntrBkSttlmDt = Modscan.FDATE.ToString("dd-MMM-yyyy")
            grpHdr.SttlmInf = New ch.SettlementInformation14() _
        With {.SttlmMtd = ch.SettlementMethod1Code.CLRG, .ClrSys = New ch.ClearingSystemIdentification3Choice() _
             With {.Item = ch.ClearingSystemIdentification.ACH}}
            grpHdr.InstgAgt = New ch.FinancialInstitution4() _
        With {.FinInstnId = New ch.FinancialInstitutionIdentification7() With {.BIC = BIC}}
            doc.BlkChq.GrpHdr = grpHdr
            For Each d As ChequeDetails In det
                sAmount = FormatNumber(d.Amount, 2, Microsoft.VisualBasic.TriState.False, Microsoft.VisualBasic.TriState.False, Microsoft.VisualBasic.TriState.False).Replace(",", "")
                If Not d.MICRED Then d.Codeline = "NO_MICROCODE"
                Dim c As New ch.ChequeType()
                c.PmtId = New ch.PaymentIdentification3() _
            With {.EndToEndId = d.trxID, .TxId = d.trxID}
                c.PmtTpInf = New ch.PaymentTypeInformation22() _
                With {.SvcLvl = New ch.ServiceLevel9Choice() With {.Item = ch.ServiceLevel3Code.SEPA}}
                c.IntrBkSttlmAmt = New ch.ActiveCurrencyAndAmount() With {.Ccy = ccy, .Value = sAmount}
                c.ChrgBr = ch.ChargeBearerType1Code.SLEV
                c.ChequeTx = New ch.ChequeDetails() _
            With {.ChkNmbr = d.ChequeNumber.ToString().PadLeft(6, "0"), .AccNo = d.RemitterAcc, .Microcode = d.Codeline, .BankCode = d.BankCode, .BranchCode = d.BranchCode}
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
                If File.Exists(fCreate & d.trxID & ".front.tiff") Then fCreate &= "_1"
                Using fs As New FileStream(fCreate & d.trxID & ".front.tiff", FileMode.Create)
                    fs.Write(d.FrontImageGS, 0, d.FrontImageGS.Length)
                End Using
                'Write the back image
                If File.Exists(fCreate & d.trxID & ".back.tiff") Then fCreate &= "_1"
                Using fs As New FileStream(fCreate & d.trxID & ".back.tiff", FileMode.Create)
                    fs.Write(d.BackImageGS, 0, d.BackImageGS.Length)
                End Using
                'Write the uv image
                If File.Exists(fCreate & d.trxID & ".UV.tiff") Then fCreate &= "_1"
                Using fs As New FileStream(fCreate & d.trxID & ".UV.tiff", FileMode.Create)
                    fs.Write(d.FrontImageUV, 0, d.FrontImageUV.Length)
                End Using
                'Write the Tif image
                If File.Exists(fCreate & d.trxID & ".BW.tiff") Then fCreate &= "_1"
                Using fs As New FileStream(fCreate & d.trxID & ".BW.tiff", FileMode.Create)
                    fs.Write(d.FrontImageBW, 0, d.FrontImageBW.Length)
                End Using
            Next
            If Not Directory.Exists(StrDestinationFilePath) Then Directory.CreateDirectory(StrDestinationFilePath)
            'If Not Directory.Exists(TempLocation) Then Directory.CreateDirectory(TempLocation)
            If Directory.Exists(StrDestinationFilePath) Then
                Dim fullpath As String = Path.Combine(TempLocation, msgId & ".xml")

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
                ZipContents(TempLocation, msgId, New String() {"*.xml", "*.tiff"}, "", True)
                'ZipContents(TempLocation, ImageUniqueNames & "J" & det(0).OurBankID & ".zip", New String() {"*.J*", "*.M*"}, "", True)
                Return msgId
            End If


            Return Nothing
        End Function

        Private Shared Function UnpaidCheques(ByVal l As List(Of ChequeDetails), ByVal BIC As String, ByVal ccy As String, ByVal SttlDate As String, ByVal Cntr As Int16, ByVal InterBkDate As String) As String
            Dim RegX As New Regex("[^A-Za-z0-9]")
            Dim sDt As String = Modscan.WORKING_DATE.ToString("dd-MMM-yyyy")
            Dim STm As String = Now.ToString("HH:mm")
            Dim xDt As Date = CDate(sDt & " " & STm)
            Dim sDt1 As String = Modscan.WORKING_DATE.ToString("yyyy-MM-dd")
            Dim STm1 As String = STm.Replace(":", "")
            Dim msgId As String = "DRUNP" & BIC & Modscan.FDATE.ToString("ddMMyyyy") & STm1 + Cntr
            Dim stCurrCode As String = ""
            If ccy = "0" Or ccy = "TZS" Then
                stCurrCode = "TZS"
                ccy = 0
            ElseIf ccy = "1" Or ccy = "USD" Then
                stCurrCode = "USD"
                ccy = 1
            ElseIf ccy = "2" Or ccy = "GBP" Then
                stCurrCode = "GBP"
                ccy = 3
            ElseIf ccy = "3" Or ccy = "EUR" Then
                stCurrCode = "EUR"
                ccy = 3
            ElseIf ccy = "5" Or ccy = "KES" Then
                stCurrCode = "KES"
                ccy = 5
            Else
                stCurrCode = "UGX"
                ccy = 6
            End If

            Dim doc As New res.Document()
            Dim grpHdr As New res.GroupHeader37()
            grpHdr.MsgId = msgId
            grpHdr.CreDtTm = xDt
            grpHdr.InstgAgt = New BrClearing.Common.BRISO20022PS213.BranchAndFinancialInstitutionIdentification4 With {.FinInstnId = New BrClearing.Common.BRISO20022PS213.FinancialInstitutionIdentification7 With {.BIC = l(0).OurBankBic.ToUpper}}
            'grpHdr.InstdAgt = New BrClearing.Common.BRISO20022PS213.BranchAndFinancialInstitutionIdentification4 With {.FinInstnId = New BrClearing.Common.BRISO20022PS213.FinancialInstitutionIdentification7 With {.BIC = l(0).BankBIC.ToUpper}}
            doc.FIToFIPmtStsRpt.GrpHdr = grpHdr
            Dim grpInf As New res.OriginalGroupInformation20()
            grpInf.OrgnlMsgId = l(0).OrgnlMsgId
            grpInf.OrgnlMsgNmId = "pacs.005.001.02"
            doc.FIToFIPmtStsRpt.OrgnlGrpInfAndSts = grpInf
            For Each d As ChequeDetails In l
                Try
                    Dim txnInf As New res.PaymentTransactionInformation26()
                    txnInf.StsId = xDt.ToString("yyyyMMdd") & "_" & d.RemittanceInfo 'd.ChequeNumber 'RegX.Replace(d.BankBIC & d.EndorsmentNo, String.Empty)
                    txnInf.OrgnlEndToEndId = d.OrgnlEndToEnd
                    txnInf.OrgnlTxId = d.OrgnTrxID
                    txnInf.TxSts = res.TransactionIndividualStatus3Code.RJCT
                    txnInf.StsRsnInf = {New res.StatusReasonInformation8() With
                                      {.Orgtr = New res.PartyIdentification32() With
                                                {.Id = New res.Party6Choice() With
                                                       {.Item = New res.OrganisationIdentification4() With
                                                                {.BICOrBEI = l(0).OurBankBic.ToUpper}}}, .Rsn = New res.StatusReason6Choice() With
                                                                                                                {.Item = d.RetCode.ToUpper}}}

                    'MessageBox.Show("Currency : " + d.CurrencyCode + " Value : " + FormatNumber(d.Amount, 2, Microsoft.VisualBasic.TriState.False, Microsoft.VisualBasic.TriState.False, Microsoft.VisualBasic.TriState.False).Replace(",", ""))
                    txnInf.OrgnlTxRef.IntrBkSttlmAmt = New BRISO20022PS213.ActiveOrHistoricCurrencyAndAmount With {.Ccy = stCurrCode, .Value = Decimal.Round(CDec(d.Amount), 2)}
                    'If d.ReqdColltnDt = "" Then

                    'Else
                    '    Dim ReqdColltnDate As Date = Convert.ToDateTime(d.ReqdColltnDt)
                    '    txnInf.OrgnlTxRef.IntrBkStmDt = ReqdColltnDate.ToString("yyyy-MM-dd")
                    'End If

                    'Dim ReqdColltnDate1 As Date = Convert.ToDateTime(d.ValueDate)
                    'txnInf.OrgnlTxRef.IntrBkStmDt = ReqdColltnDate1.ToString("yyyy-MM-dd")
                    'txnInf.OrgnlTxRef.IntrBkStmDt = CDate(InterBkDate).ToString("yyyy-MM-dd")
                    txnInf.OrgnlTxRef.IntrBkStmDt = Date.Today
                    txnInf.OrgnlTxRef.IntrBkSttlmDtSpecified = True

                    'Dim dt As DateTime

                    'If DateTime.TryParse(InterBkDate, dt) Then
                    '    txnInf.OrgnlTxRef.IntrBkStmDt = dt.ToString("yyyy-MM-dd")
                    'Else
                    '    Throw New Exception("Invalid InterBkDate format")
                    'End If

                    '-=========================================================
                    ' 1. Define the input string and its exact format for parsing
                    'Dim inputString As String = InterBkDate
                    ' MM must be uppercase for Month, mm is for minutes
                    'Dim inputFormat As String = "dd/MM/yyyy"

                    ' 2. Parse the string into a DateTime object
                    ' Use CultureInfo.InvariantCulture for a consistent format across different computer regional settings
                    'Dim dateTimeObject As DateTime = DateTime.ParseExact(inputString, inputFormat, System.Globalization.CultureInfo.InvariantCulture)

                    ' 3. Format the DateTime object into the desired output string
                    'Dim outputFormat As String = "yyyy-MM-dd"
                    'Dim formattedString As String = dateTimeObject.ToString(outputFormat)

                    '-=========================================================

                    'txnInf.OrgnlTxRef.IntrBkStmDt = InterBkDate.ToString("yyyy-MM-dd")

                    txnInf.OrgnlTxRef.SttlmInf = New BRISO20022PS213.SettlementInformation13() With {.SttlmMtd = BRISO20022PS213.SettlementMethod1Code.CLRG, .ClrSys = New BRISO20022PS213.ClearingSystemIdentification3Choice With {.ItemElementName = BRISO20022PS213.ItemChoiceType6.Prtry}}
                    txnInf.OrgnlTxRef.PmtTpInf = New BRISO20022PS213.PaymentTypeInformation22() With {.SvcLvl = New res.ServiceLevel8Choice With {.ItemElementName = BRISO20022PS213.ItemChoiceType7.Cd}}
                    txnInf.OrgnlTxRef.Dbtr = New BRISO20022PS213.PartyIdentification32() With {.Id = New res.Party6Choice() With {.Item = New res.OrganisationIdentification4() With {.BICOrBEI = l(0).OurBankBic}}, .Nm = d.DNm}
                    txnInf.OrgnlTxRef.DbtrAcct = New BRISO20022PS213.CashAccount16() With {.Id = New BRISO20022PS213.AccountIdentification4Choice() With {.Item = d.DbtrAcct}}
                    txnInf.OrgnlTxRef.DbtrAgt = New BRISO20022PS213.BranchAndFinancialInstitutionIdentification4 With {.FinInstnId = New BRISO20022PS213.FinancialInstitutionIdentification7 With {.BIC = d.OurBankBic.ToUpper}}
                    txnInf.OrgnlTxRef.CdtrAgt = New BRISO20022PS213.BranchAndFinancialInstitutionIdentification4 With {.FinInstnId = New BRISO20022PS213.FinancialInstitutionIdentification7 With {.BIC = d.BankBIC.ToUpper}}
                    txnInf.OrgnlTxRef.Cdtr = New BRISO20022PS213.PartyIdentification32() _
                    With {.Nm = d.CNm}

                    txnInf.OrgnlTxRef.CdtrAcct = New BRISO20022PS213.CashAccount16() With {.Id = New BRISO20022PS213.AccountIdentification4Choice() With {.Item = d.CdtrAcct}}
                    If d.SvcLvl.Trim = "SEPA" Then
                        txnInf.OrgnlTxRef.PmtTpInf.SvcLvl.Item = "SEPA"
                    ElseIf d.SvcLvl.Trim = "ACH" Then
                        txnInf.OrgnlTxRef.PmtTpInf.SvcLvl.Item = "SEPA"
                    End If
                    txnInf.OrgnlTxRef.PmtTpInf.LclInstrm = New BRISO20022PS213.LocalInstrument2Choice() With {.ItemElementName = BRISO20022PS213.ItemChoiceType2.Prtry}
                    txnInf.OrgnlTxRef.PmtTpInf.LclInstrm.Item = d.LclInstrm
                    txnInf.OrgnlTxRef.PmtTpInf.CtgyPurp = New BRISO20022PS213.CategoryPurpose1Choice() With {.ItemElementName = BRISO20022PS213.ItemChoiceType3.Cd}
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
                    Dim fullpath As String = Path.Combine(strFileLocation & "\Temp\", msgId & ".Q")
                    Dim dest As String = Path.Combine(strFileLocation & "\Files\", msgId & ".Q")
                    Dim ex As New Exception()
                    If doc.SaveToFile(fullpath, ex) Then
                        Dim xDoc As XDocument = XDocument.Load(fullpath)
                        Dim k As List(Of XAttribute) = xDoc.Root.Attributes().ToList()
                        Dim xsd As XAttribute = k(1)
                        If xDoc.Root.HasAttributes Then xDoc.Root.Attribute(xsd.Name).Remove()
                        'Dim xCreTm As XElement = xDoc.Descendants().ToList()(4)
                        'xDoc.Descendants().ToList()(4).SetValue(CDate(xCreTm.Value).ToString("yyyy-MM-ddTHH:mm:sszzz"))
                        Dim xCreTm = xDoc.Descendants().FirstOrDefault(Function(p) p.Name.LocalName = "CreDtTm")
                        If xCreTm IsNot Nothing Then
                            xCreTm.SetValue(CDate(xCreTm.Value).ToString("yyyy-MM-ddTHH:mm:sszzz"))
                        End If

                        For Each el In xDoc.Descendants().Where(Function(p) p.Name.LocalName = "IntrBkStmDt" Or p.Name.LocalName = "IntrBkSttlmDt")
                            el.SetValue(CDate(el.Value).ToString("yyyy-MM-dd"))
                        Next
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
                        Dim Destinationfile As String = DestZippedFolderLoc & "\" & Path.GetFileName(fileName)
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
                                MessageBox.Show("Failed Signing Unpaid. " & Path.GetFileName(fileName) & " : " & MessOut)
                                Modscan.ErrorLog(Path.GetFileName(fileName) & " : " & MessOut, "- Signing ")
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
                Dim fileName As String = Path.Combine(OutFile, msgId & ".chk")


                'MessageBox.Show("Step 1 - fileName: " & fileName)
                Dim fZip As New ZipFile(fileName)
                For Each itm As String In l
                    fZip.AddFile(itm, "")
                Next
                fZip.Save()
                Application.DoEvents()

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
                Dim Destinationfile As String = DestZippedFolderLoc & "\" & Path.GetFileName(fileName)
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
                        MessageBox.Show("Failed Signing. " & Path.GetFileName(fileName) & " : " & MessOut)
                        Modscan.ErrorLog(Path.GetFileName(fileName) & " : " & MessOut, "- Signing ")
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
                For Each itm As String In l
                    File.Delete(itm)
                Next
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
                    p.BRCDS(BRRSACryptography.CryptographyHelper.Encrypt(Sourcepath), BRRSACryptography.CryptographyHelper.Encrypt(DestPath), BRRSACryptography.CryptographyHelper.Encrypt("h / KNJ1uE5CmUcQb4xbsfoW9ZPzk ="), 71, cert, tokenpass, Mes, "TZ", strBatchPath, strDSkeyFile, strJavaExeInstallation, TkBased)
                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                End Try
                'MessageBox.Show("Step 5.4" & " : " & Mes)

                'MessageBox.Show("filepath " + filepath)
                'File.Delete(filepath)
                Dim fileName = Sourcepath & "\" & Path.GetFileName(filepath) & ".signed.CMS"
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

        Private Shared Function SignWithCms(ByVal sourcePath As String, ByVal destPath As String,
                                     ByVal encryptedCert As String, ByVal encryptedPassword As String,
                                     ByVal fileToSign As String) As String
            Try
                ' Decrypt certificate path and password
                'Dim certificatePath As String = BRRSACryptography.CryptographyHelper.Decrypt(encryptedCert)
                Dim certificatePassword As String = BRRSACryptography.CryptographyHelper.Decrypt(encryptedPassword)

                ' Validate certificate file exists
                If Not File.Exists(encryptedCert) Then
                    Return "Error: Certificate file not found: " & encryptedCert
                End If

                ' Load the certificate
                Dim certificate As New X509Certificate2(
                    encryptedCert,
                    certificatePassword,
                    X509KeyStorageFlags.Exportable Or X509KeyStorageFlags.PersistKeySet
                )

                If Not certificate.HasPrivateKey Then
                    Return "Error: Certificate does not contain a private key"
                End If

                ' Read the file to sign
                Dim fullSourcePath As String = Path.Combine(sourcePath, Path.GetFileName(fileToSign))
                If Not File.Exists(fullSourcePath) Then
                    Return "Error: File to sign not found: " & fullSourcePath
                End If

                Dim fileData As Byte() = File.ReadAllBytes(fullSourcePath)

                ' Create CMS signature
                Dim contentInfo As New ContentInfo(fileData)
                Dim signedCms As New SignedCms(contentInfo, True)

                Dim signer As New CmsSigner(certificate)
                signer.IncludeOption = X509IncludeOption.EndCertOnly
                signer.DigestAlgorithm = New Oid("2.16.840.1.101.3.4.2.1") ' SHA256

                ' Compute signature
                signedCms.ComputeSignature(signer)
                Dim signature As Byte() = signedCms.Encode()

                ' Save signed file
                Dim signedFileName As String = Path.Combine(sourcePath, Path.GetFileName(fileToSign) & ".signed.CMS")
                File.WriteAllBytes(signedFileName, signature)

                Return "Success: File signed with .NET CMS"
            Catch cex As CryptographicException
                Return "Cryptographic Error: " & cex.Message
            Catch ex As Exception
                Return "Signing Error: " & ex.Message
            End Try
        End Function

        ' Optional: Signature verification method
        Public Shared Function VerifySignature(ByVal signedFilePath As String, Optional ByVal originalFilePath As String = Nothing) As Boolean
            Try
                Dim signedData As Byte() = File.ReadAllBytes(signedFilePath)
                Dim signedCms As New SignedCms()
                signedCms.Decode(signedData)

                ' Verify the signature
                signedCms.CheckSignature(True)

                ' Verify content if original file provided
                If Not String.IsNullOrEmpty(originalFilePath) AndAlso File.Exists(originalFilePath) Then
                    Dim originalData As Byte() = File.ReadAllBytes(originalFilePath)
                    Dim extractedData As Byte() = signedCms.ContentInfo.Content

                    Return CompareByteArrays(originalData, extractedData)
                End If

                Return True
            Catch ex As Exception
                MessageBox.Show("Verification failed: " & ex.Message)
                Return False
            End Try
        End Function

        Private Shared Function CompareByteArrays(ByVal array1 As Byte(), ByVal array2 As Byte()) As Boolean
            If array1.Length <> array2.Length Then
                Return False
            End If

            For i As Integer = 0 To array1.Length - 1
                If array1(i) <> array2(i) Then
                    Return False
                End If
            Next

            Return True
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
                Modscan.ErrorLog("output>>" & (If([String].IsNullOrEmpty(ex.Message), "(none)", ex.Message)), "- SignFile-TZ Files")
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
                        Modscan.ErrorLog("output>>" & (If([String].IsNullOrEmpty(output), "(none)", output)), "ExecuteCommand-TZ Files")
                        Modscan.ErrorLog("error>>" & (If([String].IsNullOrEmpty([error]), "(none)", [error])), "ExecuteCommand-TZ Files")
                    End If
                Catch ex As Exception
                    MessageBox.Show("E2: " & ex.Message)
                    Modscan.ErrorLog("output>>" & (If([String].IsNullOrEmpty(output), "(none)", output)), "ExecuteCommand-TZ Files")
                    Modscan.ErrorLog("error>>" & (If([String].IsNullOrEmpty([error]), "(none)", [error])), "ExecuteCommand-TZ Files")
                End Try
                ExitCode = process__1.ExitCode
                process__1.Close()
                Kill(Modscan.strBatchPath)
                strBatchPath = ""
            Catch ex As Exception
                MessageBox.Show("E3: " & ex.Message)
                MessageBox.Show("Error registerd, check error log")
                Modscan.ErrorLog(ex.Message, "- Out ExecuteCommand-TZ Files")
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
    Public Class Inwards
        'mm = FileType, cc = CurrCode, = xyz = Session, x = CertName, y = Token/Keystore pass/Cert Password, (TPss and Tusr are both decoys, both are not in use) 
        Public Shared Sub ImportTZ(ByVal x As String, ByVal y As String, ByVal sFiles As List(Of String), ByRef lbl As Label, ByRef prgAll As ProgressBar, ByRef prg As ProgressBar, Optional ByVal fType As FileType = FileType.Cheques, Optional ByVal xyz As String = "", Optional ByVal TPss As String = "", Optional ByVal TUsr As String = "")
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
        Private Shared strOriginalFile As String = " "
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
                        'Modscan.ErrorLog("Received file " & sFile, "file upload")

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


                        StrDestinationFilePath = strFileLocation & "\Unsign"
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
                        Select Case sExt
                            Case ".zip"
                                If fType = FileType.ChequeRejects Then
                                    sFile = Path.Combine(strFileLocation, sFile)
                                    If RejectedItems(sFile) Then bArchive = True
                                Else
                                    ' Define paths clearly
                                    Dim unsignedFilePath As String = Path.Combine(strFileLocation, "File", Path.GetFileName(sFile) & ".unsigned.CMS")
                                    Dim workingFilePath As String = Path.Combine(strFileLocation, "WorkingFolder", Path.GetFileName(sFile))
                                    Dim tempExtractPath As String = Path.Combine(strFileLocation, "Temp", Path.GetFileNameWithoutExtension(sFile))

                                    ' Ensure required directories exist
                                    If Not Directory.Exists(Path.Combine(strFileLocation, "Unsign")) Then
                                        Directory.CreateDirectory(Path.Combine(strFileLocation, "Unsign"))
                                    End If

                                    If Not Directory.Exists(Path.Combine(strFileLocation, "WorkingFolder")) Then
                                        Directory.CreateDirectory(Path.Combine(strFileLocation, "WorkingFolder"))
                                    End If

                                    If Not Directory.Exists(Path.Combine(strFileLocation, "Temp")) Then
                                        Directory.CreateDirectory(Path.Combine(strFileLocation, "Temp"))
                                    End If

                                    ' Step 1: Unsign the file if signing is enabled
                                    If Sign Then
                                        Try
                                            UnSignFile(sFile)
                                        Catch ex As Exception
                                            MessageBox.Show("Error unsigning file: " & ex.Message)
                                            Modscan.ErrorLog(ex.Message, "- Error  unsigning  File : " + Path.GetFileName(sFile))
                                            Continue For ' Skip this file if unsigning fails
                                        End Try
                                    End If

                                    ' Step 2: Check if unsigned file exists
                                    If Not File.Exists(unsignedFilePath) Then
                                        ' If unsigned file doesn't exist, try to use the original file
                                        MessageBox.Show("Unsigned file not found: " & unsignedFilePath & ". Using original file.")
                                        Modscan.ErrorLog("Unsigned File not found ", "- Inwards File : " + Path.GetFileName(sFile))
                                        workingFilePath = Path.Combine(strFileLocation, "WorkingFolder", Path.GetFileName(sFile))

                                        ' Copy original to working folder
                                        If File.Exists(sFile) Then
                                            File.Copy(sFile, workingFilePath, True)
                                        Else
                                            MessageBox.Show("Original file not found: " & sFile)
                                            Modscan.ErrorLog("Original  File not found ", "- Inwards File : " + Path.GetFileName(sFile))

                                            Continue For
                                        End If
                                    Else
                                        ' Step 3: Move unsigned file to working folder
                                        Try
                                            If File.Exists(workingFilePath) Then
                                                File.Delete(workingFilePath)
                                            End If
                                            File.Move(unsignedFilePath, workingFilePath)
                                        Catch ex As Exception
                                            MessageBox.Show("Error moving unsigned file: " & ex.Message)
                                            Modscan.ErrorLog(ex.Message, "Error moving unsigned  File : " + Path.GetFileName(sFile))
                                            Continue For
                                        End Try
                                    End If

                                    ' Update sFile to the working file path
                                    sFile = workingFilePath

                                    ' Step 4: Archive the working file
                                    If Not Directory.Exists(sArchivePath) Then
                                        Directory.CreateDirectory(sArchivePath)
                                    End If

                                    Dim archiveFile As String = Path.Combine(sArchivePath, Path.GetFileName(sFile))
                                    If File.Exists(sFile) And Not File.Exists(archiveFile) Then
                                        File.Copy(sFile, archiveFile, True)
                                    End If

                                    bArchive = True

                                    ' Step 5: Extract and process the ZIP file
                                    Try
                                        Dim l As List(Of String) = UnzipFiles(sFile, New String() {"*.cheque*", "*.xml", "*.tif*"}, tempExtractPath)

                                        If l IsNot Nothing AndAlso l.Count > 0 Then
                                            If l.AsQueryable().Any(Function(p) p.EndsWith("xml") Or p.Contains("tif")) Then
                                                BulkCheque(l, origFileName)

                                            Else
                                                ChequeTransaction(l, ChequeFormat.SISPackage)
                                            End If
                                        Else
                                            MessageBox.Show("No valid files found in ZIP archive: " & Path.GetFileName(sFile))
                                        End If
                                    Catch ex As Exception
                                        MessageBox.Show("Error processing ZIP file: " & ex.Message)
                                        Modscan.ErrorLog(ex.Message, "- ZIP Processing: " + Path.GetFileName(sFile))

                                    End Try

                                    ' Step 6: Clean up temporary files
                                    Try

                                        ' Clean the unsigned file if it still exists
                                        If File.Exists(unsignedFilePath) Then
                                            File.Delete(unsignedFilePath)
                                        End If


                                        ' Clean working file after processing
                                        If File.Exists(workingFilePath) Then
                                            File.Delete(workingFilePath)
                                        End If

                                        ' Clean temp folder after processing
                                        If File.Exists(tempExtractPath) Then
                                            File.Delete(tempExtractPath)
                                        End If
                                    Catch cleanupEx As Exception
                                        ' Log cleanup errors but don't stop execution
                                        Modscan.ErrorLog(cleanupEx.Message, "- Cleanup: " + Path.GetFileName(sFile))
                                    End Try

                                    isZip = True
                                End If
                            Case ".chk"
                                'MessageBox.Show("5")
                                Dim l As List(Of String) = UnzipFiles(sFile, New String() {"*.xml", "*.tif*"})
                                'MessageBox.Show("6")
                                BulkCheque(l, origFileName)
                                'MessageBox.Show("7")
                                If Directory.Exists(sDir) Then Directory.Delete(sDir, True)
                                sFile = Path.Combine(strFileLocation, sFile)
                                bArchive = True
                            Case ".cms"
                                UnSignFile(sFile)
                                Modscan.Wait(1)
                                dest = dest & "\" & Path.GetFileName(sFile & ".unsigned.CMS")
                                File.Move(sFile & ".unsigned.CMS", dest)
                                Dim l As List(Of String) = UnzipFiles(dest, New String() {"*.xml", "*.tif*"}, Path.GetDirectoryName(dest))
                                Modscan.Wait(1)
                                BulkCheque(l, origFileName)
                                Modscan.Wait(1)
                                sArchivePath = Modscan.ArchivesPath & "\FromTach\" & Now.ToString("yyyyMMdd") & "\"
                                sArchiveFile = sArchivePath & Path.GetFileName(sFile)
                                bArchive = True
                            Case ".chr", ".q"
                                'If Sign = True Then
                                '    'MessageBox.Show("Imefika Mbili")
                                '    ReadFile(sFile, strFileLocation)
                                '    'MessageBox.Show("Imefika tatu")
                                'End If
                                RejectedItems(sFile)
                                bArchive = True
                                sArchivePath = Modscan.ArchivesPath & "\FromTach\" & Now.ToString("yyyyMMdd") & "\"
                                sArchiveFile = sArchivePath & Path.GetFileName(sFile)
                            Case ".v", ".r", ".rc"
                                ReadFile(sFile, strFileLocation)
                                sFile = Path.Combine(strFileLocation, sFile)
                                ResponsesFromACH(sFile)
                                bArchive = True
                                sArchivePath = Modscan.ArchivesPath & "\FromTach\" & Now.ToString("yyyyMMdd") & "\"
                                sArchiveFile = sArchivePath & Path.GetFileName(sFile)
                            Case ".s"

                                'RejectedEFTs(sFile)
                                'MessageBox.Show("Imefika moja")
                                If Sign = True Then
                                    'MessageBox.Show("Imefika Mbili")
                                    'Modscan.ErrorLog("undressing this file " & sFile, "file upload")
                                    ReadFile(sFile, strFileLocation)
                                    'MessageBox.Show("Imefika tatu")
                                End If
                                'MessageBox.Show("Imefika nne")
                                'Modscan.ErrorLog("uploading " & sFile, "file upload")

                                BulkCredit(sFile)
                                'MessageBox.Show("Imefika tano")
                                '    sFile = Path.Combine(strFileLocation, sFile)
                                '    bArchive = True
                                '    sArchivePath = Modscan.ArchivesPath & "\FromTach\" & Now.ToString("yyyyMMdd") & "\"
                                'sArchiveFile = sArchivePath & Path.GetFileName(sFile)
                                ''Archive Unsigned
                                'sArchiveFile = ""
                                If Not Directory.Exists(Path.GetDirectoryName(sArchivePath)) Then Directory.CreateDirectory(Path.GetDirectoryName(sArchivePath))
                                If (File.Exists(sFile)) Then
                                    sArchiveFile = sArchivePath + "\" + Path.GetFileName(sFile)
                                End If

                                bArchive = True

                                If Not File.Exists(sArchiveFile) And bArchive Then
                                    File.Copy(sFile, sArchiveFile, True)
                                End If
                            Case ".n"
                                If Sign = False Then
                                    ReadFile(strFileLocation, sFile)
                                End If
                                BulkDebit(sFile)
                                sFile = Path.Combine(strFileLocation, sFile)
                                bArchive = True
                            Case ".txt"
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
                        If Not Directory.Exists(Path.GetDirectoryName(sArchivePath)) Then Directory.CreateDirectory(Path.GetDirectoryName(sArchivePath))
                        If (File.Exists(sArchiveFile)) And isZip = False Then sArchiveFile = sArchiveFile & Now.ToString("yyyyMMddHHmmss")
                        If File.Exists(SourcePath) And bArchive And isZip = False Then File.Move(sFile, sArchiveFile)
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

        'to look at this further 07/02/2026
        'Private Shared Sub ReadFiles(ByVal x As String, ByVal y As String, ByVal sFiles As List(Of String), ByRef lbl As Label, ByRef prgAll As ProgressBar, ByRef prg As ProgressBar, Optional ByVal fType As FileType = FileType.Cheques, Optional ByVal xyz As String = "", Optional ByVal TPss As String = "", Optional ByVal TUsr As String = "")

        '    Try
        '        prgAll.Step = 100 / Math.Max(1, sFiles.Count)
        '        prg.Step = 50

        '        Sign = Convert.ToBoolean(ConfigurationManager.AppSettings("Sign"))
        '        strFileLocation = ConfigurationManager.AppSettings("IncomingFiles")

        '        For Each sFile As String In sFiles

        '            Dim originalFile As String = sFile
        '            Dim workingFile As String = ""
        '            Dim unsignedFile As String = ""
        '            Dim tempExtractPath As String = ""
        '            Dim sArchiveFile As String = ""
        '            Dim bArchive As Boolean = False
        '            Dim isZip As Boolean = False
        '            Dim processedOk As Boolean = False

        '            Try
        '                lbl.Text = Path.GetFileName(originalFile)
        '                lbl.Update()

        '                Dim sExt As String = Path.GetExtension(originalFile).ToLower()
        '                Dim origFileName As String = Path.GetFileNameWithoutExtension(originalFile)
        '                Dim tempLocation = Path.Combine(strFileLocation, "Temp")
        '                Dim workingFolder = Path.Combine(strFileLocation, "WorkingFolder")
        '                Dim unsignFolder = Path.Combine(strFileLocation, "Unsign")
        '                sArchivePath = Path.Combine(strFileLocation, "ARCHIVE", Now.ToString("yyyyMMdd"))

        '                If Not Directory.Exists(tempLocation) Then Directory.CreateDirectory(tempLocation)
        '                If Not Directory.Exists(workingFolder) Then Directory.CreateDirectory(workingFolder)
        '                If Not Directory.Exists(unsignFolder) Then Directory.CreateDirectory(unsignFolder)
        '                If Not Directory.Exists(sArchivePath) Then Directory.CreateDirectory(sArchivePath)

        '                ' ===============================
        '                ' PHASE 2: WORKING COPY
        '                ' ===============================
        '                workingFile = Path.Combine(workingFolder, Path.GetFileName(originalFile))
        '                If File.Exists(workingFile) Then File.Delete(workingFile)
        '                File.Copy(originalFile, workingFile, True)

        '                ' ===============================
        '                ' PHASE 3: PROCESS BY TYPE
        '                ' ===============================
        '                Select Case sExt

        '                    Case ".zip"

        '                        If fType = FileType.ChequeRejects Then
        '                            RejectedItems(workingFile)
        '                            bArchive = True

        '                        Else
        '                            ' --- UNSIGN ---
        '                            If Sign Then
        '                                UnSignFile(workingFile)
        '                            End If

        '                            unsignedFile = Path.Combine(
        '                        strFileLocation,
        '                        "File",
        '                        Path.GetFileName(workingFile) & ".unsigned.CMS"
        '                    )

        '                            If File.Exists(unsignedFile) Then
        '                                File.Delete(workingFile)
        '                                File.Move(unsignedFile, workingFile)
        '                            End If

        '                            ' --- EXTRACT ---
        '                            tempExtractPath = Path.Combine(
        '                        tempLocation,
        '                        Path.GetFileNameWithoutExtension(workingFile)
        '                    )

        '                            Dim extracted = UnzipFiles(
        '                        workingFile,
        '                        New String() {"*.cheque*", "*.xml", "*.tif*"},
        '                        tempExtractPath
        '                    )

        '                            If extracted Is Nothing OrElse extracted.Count = 0 Then
        '                                Throw New Exception("ZIP contains no valid files")
        '                            End If

        '                            If extracted.Any(Function(p) p.EndsWith("xml") Or p.Contains("tif")) Then
        '                                BulkCheque(extracted, origFileName)
        '                            Else
        '                                ChequeTransaction(extracted, ChequeFormat.SISPackage)
        '                            End If

        '                            bArchive = True
        '                            isZip = True
        '                        End If

        '                    Case ".chk"
        '                        BulkCheque(UnzipFiles(workingFile, {"*.xml", "*.tif*"}), origFileName)
        '                        bArchive = True

        '                    Case ".cms"
        '                        UnSignFile(workingFile)
        '                        BulkCheque(UnzipFiles(workingFile & ".unsigned.CMS", {"*.xml", "*.tif*"}), origFileName)
        '                        bArchive = True

        '                    Case ".chr", ".q"
        '                        RejectedItems(workingFile)
        '                        bArchive = True

        '                    Case ".v", ".r", ".rc"
        '                        ReadFile(workingFile, strFileLocation)
        '                        ResponsesFromACH(workingFile)
        '                        bArchive = True

        '                    Case ".s"
        '                        If Sign Then ReadFile(workingFile, strFileLocation)
        '                        BulkCredit(workingFile)
        '                        bArchive = True

        '                    Case ".n"
        '                        If Not Sign Then ReadFile(strFileLocation, workingFile)
        '                        BulkDebit(workingFile)
        '                        bArchive = True

        '                    Case ".txt"
        '                        If Sign Then ReadFile(strFileLocation, workingFile)
        '                        SingleRTGS(workingFile)
        '                        bArchive = True

        '                    Case Else
        '                        bArchive = True
        '                End Select

        '                processedOk = True

        '                ' ===============================
        '                ' PHASE 4: ARCHIVE (SUCCESS ONLY)
        '                ' ===============================
        '                If processedOk AndAlso bArchive Then
        '                    sArchiveFile = Path.Combine(sArchivePath, Path.GetFileName(originalFile))
        '                    If File.Exists(sArchiveFile) Then
        '                        sArchiveFile &= "_" & Now.ToString("HHmmss")
        '                    End If
        '                    File.Move(originalFile, sArchiveFile)
        '                End If

        '                ' ===============================
        '                ' PHASE 5: CLEANUP
        '                ' ===============================
        '                If processedOk Then
        '                    If File.Exists(workingFile) Then File.Delete(workingFile)
        '                    If Directory.Exists(tempExtractPath) Then Directory.Delete(tempExtractPath, True)
        '                End If

        '            Catch ex As Exception
        '                Modscan.ErrorLog(ex.Message, "- Inwards File : " & Path.GetFileName(originalFile))
        '                ' DO NOT DELETE original file on failure
        '            End Try

        '            prg.PerformStep()
        '            prgAll.PerformStep()
        '            Application.DoEvents()
        '        Next

        '        prgAll.Value = 100
        '        prgAll.Update()

        '    Catch ex As Exception
        '        ' Global safety net
        '    End Try
        'End Sub

        Private Shared Sub ResponsesFromACH(ByVal Location As String)
            '1
            Dim sArchive As String = ""
            Location = Path.GetDirectoryName(Location)
            strFileLocation = ConfigurationManager.AppSettings("IncomingFiles")
            If Not Directory.Exists(strFileLocation) Then Directory.CreateDirectory(strFileLocation)
            sArchive = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(strFileLocation)), "ARCHIVEs\" & Now.ToString("yyyyMMdd"))
            Dim sReason = ""
            Dim Filters As String() = {"*.V", "*.C", "*.R", "*.RC"}
            For Each f As String In Filters
                Dim di As New DirectoryInfo(Location)
                For Each fi As FileInfo In di.GetFiles(f)
                    If Not Directory.Exists(sArchive & "\VALIDATIONS") Then Directory.CreateDirectory(sArchive & "\VALIDATIONS")
                    Dim temp As String = Path.Combine(sArchive & "\VALIDATIONS", Path.GetFileName(fi.FullName))
                    Dim sTempFile As String = Path.Combine(TempLocation, Path.GetFileName(fi.FullName))
                    Dim sFile As String = fi.FullName
                    ' ReadFile(sFile, sTempFile)
                    Dim doc As New res.Document()
                    Dim ex As New Exception()
                    sReason = "A"
                    If res.Document.LoadFromFile(sFile, doc, ex) Then
                        If f = "*.V" Then
                            Dim Resp As res.TransactionGroupStatus3Code = doc.FIToFIPmtStsRpt.OrgnlGrpInfAndSts.GrpSts
                            Dim MsgId As String = doc.FIToFIPmtStsRpt.OrgnlGrpInfAndSts.OrgnlMsgId

                            Dim FileDate As String = doc.FIToFIPmtStsRpt.GrpHdr.CreDtTm
                            Dim Amount As String = "0"
                            Dim SwiftAccount As String = ""
                            Dim TrxID As String = ""
                            Dim FileName As String = Path.GetFileName(sFile)
                            Dim strSQL As String = ""
                            If Resp = res.TransactionGroupStatus3Code.ACCP Then
                                strSQL = "UPDATE t_TransactionMICR SET FileStatus = 'A', RejectedReason = '" & sReason & "' WHERE Reference ='" & MsgId & "'"
                                Modscan.ExecuteData(strSQL, Nothing, Modscan.dataExecTypes.ExecTypeNonQuery, Modscan.queryType.SelectStatement)
                                Modscan.ExecuteData(Modscan.GetModify("p_AddClearingFileStatus", "FileDate", FileDate, "MessageID", MsgId, "FileName", FileName, "FileStatus", "Accepted", "RejectedReason", "", "Amount", Amount, "BankID", SwiftAccount, "TrxID", TrxID, "GroupStatus", "ACCP"), Nothing, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure)
                            ElseIf Resp = res.TransactionGroupStatus3Code.ACSC Then
                                strSQL = "UPDATE t_TransactionMICR SET FileStatus = 'S', RejectedReason = '" & sReason & "' WHERE Reference ='" & MsgId & "'"
                                Modscan.ExecuteData(strSQL, Nothing, Modscan.dataExecTypes.ExecTypeNonQuery, Modscan.queryType.SelectStatement)
                                Modscan.ExecuteData(Modscan.GetModify("p_AddClearingFileStatus", "FileDate", FileDate, "MessageID", MsgId, "FileName", FileName, "FileStatus", "Settled", "RejectedReason", "", "Amount", Amount, "BankID", SwiftAccount, "TrxID", TrxID, "GroupStatus", "ACSC"), Nothing, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure)
                            ElseIf Resp = res.TransactionGroupStatus3Code.RJCT Then
                                sReason = doc.FIToFIPmtStsRpt.OrgnlGrpInfAndSts.StsRsnInf(0).Rsn.Item
                                strSQL = "UPDATE t_TransactionMICR SET FileStatus = 'R', RejectedReason = '" & sReason & "' WHERE Reference ='" & MsgId & "'"
                                Modscan.ExecuteData(strSQL, Nothing, Modscan.dataExecTypes.ExecTypeNonQuery, Modscan.queryType.SelectStatement)
                                Modscan.ExecuteData(Modscan.GetModify("p_AddClearingFileStatus", "FileDate", FileDate, "MessageID", MsgId, "FileName", FileName, "FileStatus", "Rejected", "RejectedReason", sReason, "Amount", Amount, "BankID", SwiftAccount, "TrxID", TrxID, "GroupStatus", "RJCT"), Nothing, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure)
                            ElseIf Resp = ISO.Responses.TransactionGroupStatus3Code.PART Then
                                strSQL = "UPDATE t_TransactionMICR SET FileStatus = 'P', RejectedReason = '" & sReason & "' WHERE Reference ='" & MsgId & "'"
                                Modscan.ExecuteData(strSQL, Nothing, Modscan.dataExecTypes.ExecTypeNonQuery, Modscan.queryType.SelectStatement)
                                Modscan.ExecuteData(Modscan.GetModify("p_AddClearingFileStatus", "FileDate", FileDate, "MessageID", MsgId, "FileName", FileName, "FileStatus", "Partial", "RejectedReason", "", "Amount", Amount, "BankID", SwiftAccount, "TrxID", TrxID, "GroupStatus", "PART"), Nothing, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure)
                                'ElseIf Resp = res.TransactionGroupStatus3Code.CLRD Then
                                '    strSQL = "UPDATE t_TransactionMICR SET FileStatus = 'C', RejectedReason = '" & sReason & "' WHERE Reference ='" & MsgId & "'"
                                '    Modscan.ExecuteData(strSQL, Nothing, Modscan.dataExecTypes.ExecTypeNonQuery, Modscan.queryType.SelectStatement)
                                '    Modscan.ExecuteData(Modscan.GetModify("p_AddClearingFileStatus", "FileDate", FileDate, "MessageID", MsgId, "FileName", FileName, "FileStatus", "Cleared", "RejectedReason", "", "Amount", Amount, "BankID", SwiftAccount, "TrxID", TrxID, "GroupStatus", "CLRD"), Nothing, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure)

                            End If
                        Else
                            For item As Integer = 0 To doc.FIToFIPmtStsRpt.TxInfAndSts.Count - 1
                                Dim Resp As res.TransactionGroupStatus3Code = doc.FIToFIPmtStsRpt.TxInfAndSts(item).TxSts
                                Dim MsgId As String = doc.FIToFIPmtStsRpt.OrgnlGrpInfAndSts.OrgnlMsgId
                                Dim FileDate As String = doc.FIToFIPmtStsRpt.TxInfAndSts(item).OrgnlTxRef.IntrBkStmDt
                                Dim Amount As String = doc.FIToFIPmtStsRpt.TxInfAndSts(item).OrgnlTxRef.IntrBkSttlmAmt.Value
                                Dim SwiftAccount As String = doc.FIToFIPmtStsRpt.TxInfAndSts(item).OrgnlTxRef.DbtrAgt.FinInstnId.BIC
                                Dim TrxID As String = doc.FIToFIPmtStsRpt.TxInfAndSts(item).OrgnlEndToEndId
                                Dim FileName As String = Path.GetFileName(sFile)
                                Dim strSQL As String = ""
                                If Resp = res.TransactionGroupStatus3Code.ACCP Then
                                    strSQL = "UPDATE t_TransactionMICR SET FileStatus = 'A', RejectedReason = '" & sReason & "' WHERE Reference ='" & MsgId & "'"
                                    Modscan.ExecuteData(strSQL, Nothing, Modscan.dataExecTypes.ExecTypeNonQuery, Modscan.queryType.SelectStatement)
                                    Modscan.ExecuteData(Modscan.GetModify("p_AddClearingFileStatus", "FileDate", FileDate, "MessageID", MsgId, "FileName", FileName, "FileStatus", "Accepted", "RejectedReason", "", "Amount", Amount, "BankID", SwiftAccount, "TrxID", TrxID, "GroupStatus", "ACCP"), Nothing, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure)
                                ElseIf Resp = res.TransactionGroupStatus3Code.ACSC Then
                                    strSQL = "UPDATE t_TransactionMICR SET FileStatus = 'S', RejectedReason = '" & sReason & "' WHERE Reference ='" & MsgId & "'"
                                    Modscan.ExecuteData(strSQL, Nothing, Modscan.dataExecTypes.ExecTypeNonQuery, Modscan.queryType.SelectStatement)
                                    Modscan.ExecuteData(Modscan.GetModify("p_AddClearingFileStatus", "FileDate", FileDate, "MessageID", MsgId, "FileName", FileName, "FileStatus", "Settled", "RejectedReason", "", "Amount", Amount, "BankID", SwiftAccount, "TrxID", TrxID, "GroupStatus", "ACSC"), Nothing, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure)
                                ElseIf Resp = res.TransactionGroupStatus3Code.RJCT Then
                                    sReason = doc.FIToFIPmtStsRpt.OrgnlGrpInfAndSts.StsRsnInf(0).Rsn.Item
                                    strSQL = "UPDATE t_TransactionMICR SET FileStatus = 'R', RejectedReason = '" & sReason & "' WHERE Reference ='" & MsgId & "'"
                                    Modscan.ExecuteData(strSQL, Nothing, Modscan.dataExecTypes.ExecTypeNonQuery, Modscan.queryType.SelectStatement)
                                    Modscan.ExecuteData(Modscan.GetModify("p_AddClearingFileStatus", "FileDate", FileDate, "MessageID", MsgId, "FileName", FileName, "FileStatus", "Rejected", "RejectedReason", sReason, "Amount", Amount, "BankID", SwiftAccount, "TrxID", TrxID, "GroupStatus", "RJCT"), Nothing, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure)
                                ElseIf Resp = ISO.Responses.TransactionGroupStatus3Code.PART Then
                                    strSQL = "UPDATE t_TransactionMICR SET FileStatus = 'P', RejectedReason = '" & sReason & "' WHERE Reference ='" & MsgId & "'"
                                    Modscan.ExecuteData(strSQL, Nothing, Modscan.dataExecTypes.ExecTypeNonQuery, Modscan.queryType.SelectStatement)
                                    Modscan.ExecuteData(Modscan.GetModify("p_AddClearingFileStatus", "FileDate", FileDate, "MessageID", MsgId, "FileName", FileName, "FileStatus", "Partial", "RejectedReason", "", "Amount", Amount, "BankID", SwiftAccount, "TrxID", TrxID, "GroupStatus", "PART"), Nothing, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure)
                                    'ElseIf Resp = res.TransactionGroupStatus3Code.CLRD Then
                                    '    strSQL = "UPDATE t_TransactionMICR SET FileStatus = 'C', RejectedReason = '" & sReason & "' WHERE Reference ='" & MsgId & "'"
                                    '    Modscan.ExecuteData(strSQL, Nothing, Modscan.dataExecTypes.ExecTypeNonQuery, Modscan.queryType.SelectStatement)
                                    '    Modscan.ExecuteData(Modscan.GetModify("p_AddClearingFileStatus", "FileDate", FileDate, "MessageID", MsgId, "FileName", FileName, "FileStatus", "Cleared", "RejectedReason", "", "Amount", Amount, "BankID", SwiftAccount, "TrxID", TrxID, "GroupStatus", "CLRD"), Nothing, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure)

                                End If
                                'File.Move(fi.FullName, temp)
                            Next
                        End If
                    End If
                Next
            Next

        End Sub
        Private Shared Sub BulkCredit(ByVal sFile As String)

            Dim RegX As New Regex("[^A-Za-z0-9]")
            'Dim sTempFile As String = Path.Combine(TempLocation, sFile)
            'sFile = Path.Combine(strFileLocation, sFile)
            Dim doc As New cr.Document()
            Dim ex As New Exception()
            'MessageBox.Show("Imefika BulkCredit")
            'MessageBox.Show(sFile)
            If cr.Document.LoadFromFile(sFile, doc, ex) Then
                For Each c As cr.CreditTransferTransactionInformation11 In doc.FIToFICstmrCdtTrf.CdtTrfTxInf
                    Dim d As New EFTDetails
                    d.MsgId = doc.FIToFICstmrCdtTrf.GrpHdr.MsgId
                    d.TrxId = c.PmtId.TxId
                    d.Amount = c.IntrBkSttlmAmt.Value
                    d.Currency = c.IntrBkSttlmAmt.Ccy.ToString()
                    Try
                        d.OrgnlIntrBkSttlmDt = doc.FIToFICstmrCdtTrf.GrpHdr.IntrBkStDate.ToString()
                    Catch exOrgnlIntrBkSttlmDt As Exception
                        d.OrgnlIntrBkSttlmDt = ""
                    End Try

                    Try
                        d.SourceBankID = c.DbtrAgt.FinInstnId.BIC
                    Catch exUstrdColD As Exception
                        d.SourceBankID = ""
                    End Try

                    Try
                        d.VCode = IIf(IsDBNull(c.PmtTpInf.CtgyPurp.Item), "59", c.PmtTpInf.CtgyPurp.Item.ToString)
                    Catch exUstrdColD As Exception
                        d.VCode = "59"
                    End Try

                    Try
                        d.ReqdColltnDt = doc.FIToFICstmrCdtTrf.GrpHdr.IntrBkStDate.ToShortDateString
                    Catch exUstrdColD As Exception
                        d.VCode = ""
                    End Try

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
                        d.PymType = c.PmtTpInf.SvcLvl.Item.ToString
                    Catch exPymType As Exception
                        d.PymType = ""
                    End Try
                    Try
                        d.LclInstrm = c.PmtTpInf.LclInstrm.Item.ToString
                    Catch exLclInstrm As Exception
                        d.LclInstrm = ""
                    End Try
                    Try
                        d.CtgyPurp = c.PmtTpInf.CtgyPurp.Item.ToString
                    Catch exCtgyPurp As Exception
                        d.CtgyPurp = ""
                    End Try
                    Try
                        d.SvcLvl = c.PmtTpInf.SvcLvl.Item.ToString
                    Catch exSvcLvl As Exception
                        d.SvcLvl = ""
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

                    Try
                        d.OrgnlTxId = ""
                    Catch exOrgnlInstrID As Exception
                        d.OrgnlTxId = ""
                    End Try
                    Try
                        d.IntrBkSttlmDt = doc.FIToFICstmrCdtTrf.GrpHdr.IntrBkStDate.ToShortDateString
                    Catch exIntrBkSttlmDt As Exception
                        d.IntrBkSttlmDt = ""
                    End Try

                    Try
                        d.RemittanceInfo = c.RmtInf.Ustrd(0).ToString()
                    Catch exIntrBkSttlmDt As Exception
                        d.RemittanceInfo = ""
                    End Try
                    d.TrxData = d.MsgId & ":" & d.TrxId
                    SaveEFT(d, sFile)
                Next
            Else
                Dim Undoc As New pc412.Document()
                If pc412.Document.LoadFromFile(sFile, Undoc, ex) Then
                    For Each c As pc412.PaymentTransactionInformation27 In Undoc.PmtRtr.TxInf
                        Dim d As New EFTDetails
                        d.MsgId = Undoc.PmtRtr.GrpHdr.MsgId
                        d.TrxId = c.RtrId
                        d.Amount = c.RtrdIntrBkSttlmAmt.Value
                        d.Currency = c.RtrdIntrBkSttlmAmt.Ccy.ToString()
                        Try
                            d.SourceBankID = DirectCast(c.RtrRsnInf(0).Orgtr.Id.Item, pc412.OrganisationIdentification4).BICOrBEI
                        Catch exUstrdColD As Exception
                            d.SourceBankID = ""
                        End Try

                        Try
                            d.VCode = "59"
                        Catch exUstrdColD As Exception
                            d.VCode = ""
                        End Try
                        Try
                            d.OrgnlMsgId = c.OrgnlGrpInf.OrgnlMsgId
                        Catch exUstrdColD As Exception
                            d.OrgnlMsgId = ""
                        End Try
                        Try
                            d.RetCode = IIf(IsDBNull(c.RtrRsnInf(0).Rsn.Item.ToString), "", c.RtrRsnInf(0).Rsn.Item.ToString)
                        Catch exUstrdColD As Exception
                            d.RetCode = ""
                        End Try
                        Try
                            d.UstrdColD = ""
                        Catch exUstrdColD As Exception
                            d.UstrdColD = ""
                        End Try
                        Try
                            d.DAdrLine = IIf(c.OrgnlTxRef.Dbtr.PstlAdr.AdrLine.ToString = "", "", c.OrgnlTxRef.Dbtr.PstlAdr.AdrLine.ToString)
                        Catch exDAdrLine As Exception
                            d.DAdrLine = ""
                        End Try
                        Try
                            d.DTwnNm = IIf(c.OrgnlTxRef.Dbtr.PstlAdr.TwnNm.ToString = "", "", c.OrgnlTxRef.Dbtr.PstlAdr.TwnNm.ToString)
                        Catch exDTwnNm As Exception
                            d.DTwnNm = ""
                        End Try
                        Try
                            d.DCtry = IIf(c.OrgnlTxRef.Dbtr.PstlAdr.Ctry.ToString = "", "", c.OrgnlTxRef.Dbtr.PstlAdr.Ctry.ToString)
                        Catch exDCtry As Exception
                            d.DCtry = ""
                        End Try
                        Try
                            d.DNm = c.OrgnlTxRef.Dbtr.Nm
                        Catch exDNm As Exception
                            d.DNm = ""
                        End Try
                        Try
                            d.DPhneNb = IIf(c.OrgnlTxRef.Dbtr.CtctDtls.PhneNb.ToString = "", "", c.OrgnlTxRef.Dbtr.CtctDtls.PhneNb)
                        Catch exDPhneNb As Exception
                            d.DPhneNb = ""
                        End Try
                        Try
                            d.DMobNb = IIf(c.OrgnlTxRef.Dbtr.CtctDtls.MobNb.ToString = "", "", c.OrgnlTxRef.Dbtr.CtctDtls.MobNb = "")
                        Catch exDMobNb As Exception
                            d.DMobNb = ""
                        End Try
                        Try
                            d.DEmailAdr = IIf(c.OrgnlTxRef.Dbtr.CtctDtls.EmailAdr.ToString = "", "", c.OrgnlTxRef.Dbtr.CtctDtls.EmailAdr.ToString)
                        Catch exDEmailAdr As Exception
                            d.DEmailAdr = ""
                        End Try
                        Try
                            d.DOthr = IIf(c.OrgnlTxRef.Dbtr.CtctDtls.Othr.ToString = "", "", c.OrgnlTxRef.Dbtr.CtctDtls.Othr)
                        Catch exDOthr As Exception
                            d.DOthr = ""
                        End Try
                        Try
                            d.DbtrAcct = c.OrgnlTxRef.CdtrAcct.Id.Item.ToString()
                        Catch exDbtrAcct As Exception
                            d.DbtrAcct = ""
                        End Try
                        Try
                            d.CAdrLine = IIf(c.OrgnlTxRef.Cdtr.PstlAdr.AdrLine(0).ToString = "", "", c.OrgnlTxRef.Cdtr.PstlAdr.AdrLine(0).ToString)
                        Catch exCAdrLine As Exception
                            d.CAdrLine = ""
                        End Try
                        Try
                            d.CTwnNm = IIf(c.OrgnlTxRef.Cdtr.PstlAdr.TwnNm.ToString = "", "", c.OrgnlTxRef.Cdtr.PstlAdr.TwnNm)
                        Catch exCTwnNm As Exception
                            d.CTwnNm = ""
                        End Try
                        Try
                            d.CCtry = IIf(c.OrgnlTxRef.Cdtr.PstlAdr.Ctry.ToString = "", "", c.OrgnlTxRef.Cdtr.PstlAdr.Ctry)
                        Catch exCCtry As Exception
                            d.CCtry = ""
                        End Try
                        Try
                            d.CNm = IIf(c.OrgnlTxRef.Cdtr.Nm.ToString = "", "", c.OrgnlTxRef.Cdtr.Nm)
                        Catch exCNm As Exception
                            d.CNm = ""
                        End Try
                        Try
                            d.CPhneNb = IIf(c.OrgnlTxRef.Cdtr.CtctDtls.PhneNb.ToString = "", "", c.OrgnlTxRef.Cdtr.CtctDtls.PhneNb)
                        Catch exCPhneNb As Exception
                            d.CPhneNb = ""
                        End Try
                        Try
                            d.CMobNb = IIf(c.OrgnlTxRef.Cdtr.CtctDtls.MobNb.ToString = "", "", c.OrgnlTxRef.Cdtr.CtctDtls.MobNb)
                        Catch exCMobNb As Exception
                            d.CMobNb = ""
                        End Try
                        Try
                            d.CEmailAdr = IIf(c.RtrRsnInf(0).Orgtr.CtctDtls.EmailAdr.ToString = "", "", c.RtrRsnInf(0).Orgtr.CtctDtls.EmailAdr)
                        Catch exCEmailAdr As Exception
                            d.CEmailAdr = ""
                        End Try
                        Try
                            d.COthr = IIf(c.RtrRsnInf(0).Orgtr.CtctDtls.Othr.ToString = "", "", c.RtrRsnInf(0).Orgtr.CtctDtls.Othr)
                        Catch exCOthr As Exception
                            d.COthr = ""
                        End Try
                        Try
                            d.PymType = c.OrgnlTxRef.PmtTpInf.SvcLvl.Item.ToString
                        Catch exPymType As Exception
                            d.PymType = ""
                        End Try
                        Try
                            d.CdtrAcct = c.OrgnlTxRef.DbtrAcct.Id.Item.ToString()
                        Catch exCdtrAcct As Exception
                            d.CdtrAcct = ""
                        End Try
                        Try
                            d.OrgnlInstrID = IIf(c.OrgnlInstrId.ToString = "", "", c.OrgnlInstrId)
                        Catch exOrgnlInstrID As Exception
                            d.OrgnlInstrID = ""
                        End Try
                        Try
                            d.OrgnlTxId = IIf(c.OrgnlTxId.ToString = "", "", c.OrgnlTxId)
                        Catch exOrgnlInstrID As Exception
                            d.OrgnlTxId = ""
                        End Try

                        Try
                            d.OrgnlEndToEnd = c.OrgnlEndToEndId
                        Catch exOrgnlEndToEnd As Exception
                            d.OrgnlEndToEnd = ""
                        End Try
                        Try
                            d.IntrBkSttlmDt = c.OrgnlTxRef.IntrBkSttlmDt.ToShortDateString
                        Catch exIntrBkSttlmDt As Exception
                            d.IntrBkSttlmDt = ""
                        End Try
                        d.TrxData = d.OrgnlTxId & ":" & d.MsgId & ":" & d.TrxId
                        SaveEFT(d, sFile)
                    Next
                End If
            End If
            Dim strpath = sFile.LastIndexOf(".") + 1

            'File.Delete(sTempFile)
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
                    Dim d As New EFTDetails
                    d.MsgId = doc.FIToFICstmrDrctDbt.GrpHdr.MsgId
                    d.Amount = c.IntrBkSttlmAmt.Value
                    d.VCode = "40"
                    d.BeneficiaryAcc = RegX.Replace(c.DbtrAcct.Id.Item, String.Empty) '.Replace("'", "")
                    d.BeneficiaryName = RegX.Replace(c.Dbtr.Nm, " ") '.Replace("'", "")
                    d.Currency = c.IntrBkSttlmAmt.Ccy.ToString()
                    d.DestBIC = c.DbtrAgt.FinInstnId.BIC
                    d.EFTID = Modscan.GetNextInt16
                    d.IsDebit = True
                    d.Reference = c.PmtId.EndToEndId
                    d.RemitterAcc = RegX.Replace(c.CdtrAcct.Id.Item, String.Empty) '.Replace("'", "")
                    d.RemitterName = RegX.Replace(c.Cdtr.Nm, " ") '.Replace("'", "")
                    d.RemittanceInfo = c.RmtInf.Item
                    d.SourceBIC = c.CdtrAgt.FinInstnId.BIC
                    d.TranType = 0
                    d.ValueDate = doc.FIToFICstmrDrctDbt.GrpHdr.IntrBkSttlmDt
                    SaveEFT(d, sFile)

                    Try
                        Dim e As New DDDetail
                        e.MsgId = doc.FIToFICstmrDrctDbt.GrpHdr.MsgId
                        e.Amount = c.IntrBkSttlmAmt.Value
                        e.DrAcc = RegX.Replace(c.DbtrAcct.Id.Item, String.Empty)
                        e.DrName = RegX.Replace(c.Dbtr.Nm, " ")
                        e.Curr = c.IntrBkSttlmAmt.Ccy.ToString()
                        e.dBIC = c.DbtrAgt.FinInstnId.BIC
                        e.Collection = c.ReqdColltnDt
                        d.VCode = "40"
                        e.CrAcc = RegX.Replace(c.CdtrAcct.Id.Item, String.Empty)
                        e.CrName = RegX.Replace(c.Cdtr.Nm, " ")
                        e.EndToEndId = c.PmtId.EndToEndId
                        e.InstrId = c.PmtId.InstrId
                        e.Mandate.AmdmntInd = c.DrctDbtTx.MndtRltdInf.AmdmntInd
                        e.Mandate.DtOfSgntr = c.DrctDbtTx.MndtRltdInf.DtOfSgntr
                        e.Mandate.MndtId = c.DrctDbtTx.MndtRltdInf.MndtId
                        e.Remittance = c.RmtInf.Item
                        e.sBIC = c.CdtrAgt.FinInstnId.BIC
                        e.Scheme = c.DrctDbtTx.CdtrSchmeId.Id.Item.Othr.Id
                        e.Settlement = doc.FIToFICstmrDrctDbt.GrpHdr.IntrBkSttlmDt
                        e.TrxId = c.PmtId.TxId
                        e.Creation = doc.FIToFICstmrDrctDbt.GrpHdr.CreDtTm
                        'SaveDDInfo(e)
                    Catch
                    End Try
                Next
            ElseIf res.Document.LoadFromFile(sFile, doc2, ex) Then
                RejectedItems(sFile)
            Else
                Throw ex
            End If
            File.Delete(sTempFile)
        End Sub
        Private Shared Sub SingleRTGS(ByVal sFile As String)
            Dim sTempFile As String = Path.Combine(TempLocation, sFile)
            Dim sContent As String = ""
            If File.Exists(Path.Combine(strFileLocation, "RTGSPAYMENTS\" & sFile)) Then
                sFile = Path.Combine(strFileLocation, "RTGSPAYMENTS\" & sFile)
            ElseIf File.Exists(Path.Combine(strFileLocation, "RTGSSTATEMENTS\" & sFile)) Then
                sFile = Path.Combine(strFileLocation, "RTGSSTATEMENTS\" & sFile)
            ElseIf File.Exists(Path.Combine(strFileLocation, "RTGSREPLIES\" & sFile)) Then
                sFile = Path.Combine(strFileLocation, "RTGSREPLIES\" & sFile)
            ElseIf File.Exists(Path.Combine(strFileLocation, "RTGSADVICES\" & sFile)) Then
                sFile = Path.Combine(strFileLocation, "RTGSADVICES\" & sFile)
            End If
            If Sign Then
                StripSignature(sFile, sTempFile)
            End If
            Using fs As New FileStream(sFile, FileMode.Open, FileAccess.Read)
                Using sr As New StreamReader(fs)
                    sContent = sr.ReadToEnd()
                End Using
            End Using
            Dim eft As EFTDetails = GetRTGSDetails(sContent)
            If eft.TranType <> 1 And eft.TranType <> 2 Then
                'SaveMessage(eft, sFile)
            End If
            If eft.TranType = 1 Or eft.TranType = 2 Or eft.TranType = 3 Or eft.TranType = 4 Then
                If Not (eft.BeneficiaryAcc Is Nothing) Then
                    SaveEFT(eft, sFile)
                End If
            End If
            File.Delete(sTempFile)
        End Sub
        Private Shared Function GetRTGSDetails(ByVal sContent As String) As EFTDetails
            Dim RegX As New Regex("[^A-Za-z0-9]")
            Dim sDetals As String() = sContent.Split(Environment.NewLine.ToCharArray())
            sContent = sContent.Replace(vbCr & vbLf, "")
            Dim sGroups As String() = sContent.Split("{"c)
            Dim sAllowed As String() = New String() {"103", "202", "900", "910", "941", "950", "999"}
            Dim sMsghdr As String = sGroups(2).Substring(3, 3)
            Dim rec As New EFTDetails()
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
                    rec.Currency = "TZS"
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
                                Dim chq As ChequeDetails = GetSISDetails(sContent, File.ReadAllBytes(fImageFile), File.ReadAllBytes(bImageFile))
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
                            Dim chq As New ChequeDetails
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
            Dim SystemType As String = ConfigurationManager.AppSettings("sysType")
            For Each f As FileInfo In fi
                Dim doc As New ch.Document()
                Dim ex As New Exception()
                If ch.Document.LoadFromFile(f.FullName, doc, ex) Then
                    Dim chq As New ChequeDetails
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
                        chq.BeneficiaryAcc = c.CdtrAcct.Id.Item.Replace("'", "")
                        chq.BeneficiaryName = c.Cdtr.Nm '.Replace("'", "").Trim()
                        chq.OurBranch = c.ChequeTx.BranchCode.PadLeft(3, "0")
                        chq.ChequeIndex = 1
                        chq.ChequeNumber = c.ChequeTx.ChkNmbr
                        chq.CurrencyCode = c.IntrBkSttlmAmt.Ccy.ToString()
                        chq.EndorsmentNo = c.PmtId.TxId
                        chq.FileName = OrigFilename 'f.Name
                        chq.MICRED = True
                        chq.RemitterAcc = c.DbtrAcct.Id.Item.Replace("'", "")
                        chq.RemitterName = c.Dbtr.Nm '.Replace("'", "").Trim()
                        chq.TransCode = "CLG"
                        chq.trxID = c.PmtId.TxId
                        Dim MLine As String = c.ChequeTx.Microcode.Trim()

                        Dim splitMicr As String() = c.ChequeTx.Microcode.ToString.Trim().Split("/", 7, StringSplitOptions.RemoveEmptyEntries)

                        Dim splitLen As Int16 = splitMicr.Length

                        Try
                            chq.BankBIC = c.CdtrAgt.FinInstnId.BIC
                        Catch exUstrdColD As Exception
                            chq.BankBIC = ""
                        End Try

                        Try
                            If chq.CurrencyCode = "TZS" Then
                                chq.VoucherCode = splitMicr(splitLen - 1)
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
                            chq.ReqdColltnDt = IIf(IsDBNull(doc.BlkChq.GrpHdr.IntrBkSttlmDt.ToString("dd'/'MM'/'yyyy")), "", doc.BlkChq.GrpHdr.IntrBkSttlmDt.ToString("dd'/'MM'/'yyyy"))
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
                            chq.IntrBkSttlmDt = IIf(IsDBNull(doc.BlkChq.GrpHdr.IntrBkSttlmDt.ToString("dd'/'MM'/'yyyy")), "", doc.BlkChq.GrpHdr.IntrBkSttlmDt.ToString("dd'/'MM'/'yyyy"))
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

                        arrImages.AddRange(IO.Directory.GetFiles(Path.GetDirectoryName(l(0)), sPattern & ".front.tif*"))
                        If arrImages.Count = 0 Then
                            arrImages.AddRange(IO.Directory.GetFiles(Path.GetDirectoryName(l(0)), chq.EndorsmentNo & ".front.tif*"))
                            If arrImages.Count = 0 Then
                                arrImages.AddRange(IO.Directory.GetFiles(Path.GetDirectoryName(l(0)), chq.EndorsmentNo & ".front.tiff*"))
                            End If
                        End If
                        If arrImages.Count > 0 Then
                            chq.FrontImageGS = File.ReadAllBytes(arrImages(0).ToString)
                        End If
                        '----front BW
                        arrImages = New ArrayList
                        arrImages.AddRange(IO.Directory.GetFiles(Path.GetDirectoryName(l(0)), sPattern & ".BW.tif*"))
                        If arrImages.Count = 0 Then
                            arrImages.AddRange(IO.Directory.GetFiles(Path.GetDirectoryName(l(0)), chq.EndorsmentNo & ".BW.tif*"))
                            If arrImages.Count = 0 Then
                                arrImages.AddRange(IO.Directory.GetFiles(Path.GetDirectoryName(l(0)), chq.EndorsmentNo & ".BW.tiff*"))
                            End If
                        End If
                        If arrImages.Count > 0 Then
                            chq.FrontImageBW = File.ReadAllBytes(arrImages(0).ToString)
                        End If
                        '------Back gray scale Image
                        arrImages = New ArrayList
                        'back grayscale image
                        arrImages.AddRange(IO.Directory.GetFiles(Path.GetDirectoryName(l(0)), sPattern & ".back.tif*"))
                        If arrImages.Count = 0 Then
                            arrImages.AddRange(IO.Directory.GetFiles(Path.GetDirectoryName(l(0)), chq.EndorsmentNo & ".back.tif*"))
                            If arrImages.Count = 0 Then
                                arrImages.AddRange(IO.Directory.GetFiles(Path.GetDirectoryName(l(0)), chq.EndorsmentNo & ".back.tiff*"))
                            End If
                        End If
                        If arrImages.Count > 0 Then
                            chq.BackImageGS = File.ReadAllBytes(arrImages(0).ToString)
                        End If
                        'Uv image
                        arrImages = New ArrayList
                        arrImages.AddRange(IO.Directory.GetFiles(Path.GetDirectoryName(l(0)), sPattern & ".UV.tif*"))
                        If arrImages.Count = 0 Then
                            arrImages.AddRange(IO.Directory.GetFiles(Path.GetDirectoryName(l(0)), chq.EndorsmentNo & ".UV.tif*"))
                            If arrImages.Count = 0 Then
                                arrImages.AddRange(IO.Directory.GetFiles(Path.GetDirectoryName(l(0)), chq.EndorsmentNo & ".UV.tiff*"))
                            End If
                        End If
                        If arrImages.Count > 0 Then
                            chq.FrontImageUV = File.ReadAllBytes(arrImages(0).ToString)
                        End If
                        SaveCheque(chq)
                    Next
                End If
            Next
        End Sub
        Private Shared Function GetSISDetails(ByVal sDetail As String, ByVal sFront As Byte(), ByVal sRear As Byte()) As ChequeDetails
            Dim Details As String() = sDetail.Split(vbLf)
            Dim chq As New ChequeDetails
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
            chq.CurrencyCode = IIf(chq.CurrencyCode = "1", "TZS", chq.CurrencyCode)
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
                    Dim ch As New ChequeDetails
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
                        ch.RetCode = txn.StsRsnInf(0).Rsn.Item
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
                        ch.IntrBkSttlmDt = txn.OrgnlTxRef.IntrBkStmDt
                    Catch exIntrBkSttlmDt As Exception
                        ch.IntrBkSttlmDt = ""
                    End Try
                    ch.TrxData = ch.OrgnlTxId & ":" & ch.MsgId
                    SaveCheque(ch)
                Next
            End If
            If Sign Then File.Delete(sTempFile)
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
                        Dim ch As New ChequeDetails
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
                        Dim d As New EFTDetails
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
        Private Shared Sub UnpayCheque(ByRef ch As ChequeDetails)
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
        Private Sub UnpayEft(ByRef d As EFTDetails)
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
        Private Shared Sub SaveCheque(ByVal chq As ChequeDetails)

            Try
                Dim RegX As New Regex("[^A-Za-z0-9]")
                Dim strArr As String = ""
                Dim LineItemsTable As Hashtable = New Hashtable
                Dim MethodDt As DataTable = New DataTable
                Dim SystemType As String = ConfigurationManager.AppSettings("sysType")

                Select Case SystemType.ToUpper.Trim
                    Case "BR"
                        If chq.RetCode = "00" Then
                            MethodDt = Getdata("sp_GetSystem", True, "OurBranchID", chq.OurBranch)
                        Else
                            MethodDt = Getdata("sp_GetSystem", True, "OurBranchID", chq.Orgid)
                        End If
                    Case "BRMFO"
                        If chq.RetCode = "00" Then
                            MethodDt = Getdata("sp_GetSystem", True, "OurBranchID", chq.OurBranch)
                        Else
                            MethodDt = Getdata("sp_GetSystem", True, "OurBranchID", chq.Orgid)
                        End If
                    Case "BRNET"
                        If chq.RetCode = "00" Then
                            If chq.OurBranch.Length < 2 Then
                                chq.OurBranch = "0" + chq.OurBranch
                            End If
                            chq.Orgid = chq.OurBranch
                            'MethodDt = Getdata("sp_GetSystemBranchDetail", True, "OurBranchID", chq.OurBranch)
                        End If
                    Case "BRNETOLD"
                        If chq.RetCode = "00" Then
                            MethodDt = Getdata("sp_GetSystemBranchDetail", True, "OurBranchID", chq.OurBranch)
                        Else
                            MethodDt = Getdata("sp_GetSystemBranchDetail", True, "OurBranchID", chq.Orgid)
                        End If
                End Select

                If IsNothing(MethodDt) = True Then

                    If chq.RetCode = "00" Then
                        chq.OurBranch = "0" & chq.Codeline.Substring(13, 2)
                    End If
                    Select Case SystemType.ToUpper.Trim
                        Case "BR"
                            MethodDt = Getdata("sp_GetSystem", True, "OurBranchID", chq.OurBranch)
                        Case "BRNET"
                            MethodDt = Getdata("sp_GetSystemBranchDetail", True, "OurBranchID", chq.OurBranch)
                    End Select
                End If

                Dim sCurr As String = ""
                If MethodDt.Rows.Count <= 1 Then
                    Select Case SystemType.ToUpper.Trim
                        Case "BR"
                            sCurr = MethodDt(0)("Currency")
                        Case "BRNET"
                            If chq.RetCode = "00" Then
                                sCurr = chq.CurrencyCode 'MethodDt(0)("CurrencyID")
                            End If
                    End Select
                End If
                Dim strAction As String = ""
                If chq.RetCode = "00" Then chq.ValueDate = Modscan.WORKING_DATE
                Select Case SystemType.ToUpper.Trim
                    Case "BR"
                        If Not RecExists("SELECT ReferenceNo FROM t_TransactionIncomingEJ WHERE ReferenceNo = '" & chq.EndorsmentNo & "'") Then
                            If chq.RetCode = "00" Then
                                chq.ChequeNumber = RegX.Replace(chq.ChequeNumber, String.Empty)
                                chq.OurBranch = chq.OurBranch
                                chq.RemitterAcc = chq.RemitterAcc
                                'chq.RemitterAcc = ReMappedAccount(chq.RemitterAcc, chq.OurBranch)
                                chq.ChequeNumber = chq.ChequeNumber.Replace("CPO", "CP").Trim()
                                chq.ChequeNumber = chq.ChequeNumber.Replace("NOC", "NC").Trim()
                                If IsNumeric(chq.ChequeNumber) Then
                                    If chq.ChequeNumber.Length = 7 Then
                                        chq.ChequeNumber = chq.ChequeNumber
                                    ElseIf chq.ChequeNumber.Length = 6 Then
                                        chq.ChequeNumber = chq.ChequeNumber
                                    End If
                                End If
                                Dim sAcc As String = RegX.Replace(chq.RemitterAcc, String.Empty)
                                If chq.VoucherCode <> "12" Then
                                    If chq.OurBranch.Trim = "" Then
                                        If RecExists("SELECT OurBranchID FROM t_Account WHERE AccountID='" & chq.RemitterAcc & "'") Then
                                            Dim sBranch As String = GetScalarREC("SELECT OurBranchID FROM t_Account WHERE AccountID='" & chq.RemitterAcc & "'")
                                            chq.OurBranch = sBranch
                                        End If
                                    End If
                                Else
                                    Dim GlIDLength As String = GetScalarREC("SELECT GLIDLength FROM t_System")
                                    If chq.RemitterAcc.Length > GlIDLength Then
                                        chq.RemitterAcc = chq.RemitterAcc.Substring((chq.RemitterAcc.Length - Val(GlIDLength)))
                                    End If

                                End If
                                If chq.OurBranch.Trim = "" Then chq.OurBranch = Modscan.OurBranchID
                            Else
                                If chq.VoucherCode <> "12" Then
                                    If chq.OurBranch.Trim = "" Then
                                        If RecExists("SELECT OurBranchID FROM t_Account WHERE AccountID='" & chq.BeneficiaryAcc & "'") Then
                                            Dim sBranch As String = GetScalarREC("SELECT OurBranchID FROM t_Account WHERE AccountID='" & chq.BeneficiaryAcc & "'")
                                            chq.OurBranch = sBranch
                                        End If
                                    End If
                                Else
                                    Dim GlIDLength As String = GetScalarREC("SELECT GLIDLength FROM t_System")
                                    If chq.RemitterAcc.Length > GlIDLength Then
                                        chq.RemitterAcc = chq.RemitterAcc.Substring((chq.RemitterAcc.Length - Val(GlIDLength)))
                                    End If

                                End If

                                If chq.BankCode = "" Then
                                    If RecExists("SELECT BankID FROM t_Banks WHERE SwiftCode = '" & chq.BankBIC & "'") Then
                                        chq.BankCode = GetScalarREC("SELECT BankID FROM t_Banks WHERE SwiftCode = '" & chq.BankBIC & "'")
                                    End If
                                    If chq.BankCode <> "" And chq.BranchCode = "" Then
                                        chq.BranchCode = GetScalarREC("SELECT Top 1 BranchID FROM t_Branches WHERE BankID = '" & chq.BankCode & "'")
                                    End If
                                End If

                                If chq.BeneficiaryName = "NOT PROVIDED" Then
                                    If RecExists("SELECT 1 FROM t_Account WHERE AccountID='" & chq.BeneficiaryAcc & "'") Then
                                        Dim sName As String = GetScalarREC("SELECT Name FROM t_Account WHERE AccountID='" & chq.BeneficiaryAcc & "'")
                                        chq.BeneficiaryName = sName
                                    End If
                                End If
                            End If
                        End If
                    Case "BRNET"
                        If chq.RetCode = "00" Then
                            chq.ChequeNumber = RegX.Replace(chq.ChequeNumber, String.Empty)
                            chq.OurBranch = chq.OurBranch
                            chq.RemitterAcc = chq.RemitterAcc
                            'chq.RemitterAcc = ReMappedAccount(chq.RemitterAcc, chq.OurBranch)
                            chq.ChequeNumber = chq.ChequeNumber.Replace("CPO", "CP").Trim()
                            chq.ChequeNumber = chq.ChequeNumber.Replace("NOC", "NC").Trim()
                            If IsNumeric(chq.ChequeNumber) Then
                                If chq.ChequeNumber.Length = 7 Then
                                    chq.ChequeNumber = chq.ChequeNumber
                                ElseIf chq.ChequeNumber.Length = 6 Then
                                    chq.ChequeNumber = chq.ChequeNumber
                                End If
                            End If
                            Dim sAcc As String = RegX.Replace(chq.RemitterAcc, String.Empty)
                            If chq.VoucherCode <> "12" Then
                                If chq.OurBranch.Trim = "" Then
                                    If RecExists("SELECT OurBranchID FROM t_AccountCustomer WHERE AccountID='" & chq.RemitterAcc & "'") Then
                                        Dim sBranch As String = GetScalarREC("SELECT OurBranchID FROM t_AccountCustomer WHERE AccountID='" & chq.RemitterAcc & "'")
                                        chq.OurBranch = sBranch
                                    End If
                                End If
                            Else
                                Dim GlIDLength As String = GetScalarREC("SELECT GLIDLength FROM t_systembanksetting")
                                If chq.RemitterAcc.Length > GlIDLength Then
                                    chq.RemitterAcc = chq.RemitterAcc.Substring((chq.RemitterAcc.Length - Val(GlIDLength)))
                                End If

                            End If
                            If chq.OurBranch.Trim = "" Then chq.OurBranch = Modscan.OurBranchID
                        End If
                End Select


                Dim ProcNo As String = Modscan.GetNextInt16 & Modscan.GetNextString
                '------------------------------------------------------------------------------------------------------
                LineItemsTable.Add("RCODE", chq.RetCode) ' RCODE
                LineItemsTable.Add("VTYPE", chq.VoucherCode) ' Voucher Type
                LineItemsTable.Add("AMOUNT", (Val(chq.Amount) / 1).ToString) ' Amount
                LineItemsTable.Add("ENTRYMODE", "0") ' Amount Entry Mode
                LineItemsTable.Add("CURRENCYCODE", chq.CurrencyCode) ' Amount Entry Mode
                If chq.RetCode = "00" Then
                    LineItemsTable.Add("DESTBANK", If(IsDBNull(Modscan.OurBankID).ToString() = "", "", Modscan.OurBankID)) ' Dest Bank
                End If
                If chq.RetCode <> "00" Then
                    LineItemsTable.Add("COLLACC", chq.RemitterAcc) ' Dest Account
                    LineItemsTable.Add("DESTACC", chq.BeneficiaryAcc) 'Collecting Account Details
                    'LineItemsTable.Add("DESTBRANCH", If(IsDBNull(chq.Orgid).ToString() = "", "", chq.Orgid)) ' Dest Branch
                Else
                    LineItemsTable.Add("DESTACC", chq.RemitterAcc) ' Dest Account
                    LineItemsTable.Add("COLLACC", chq.BeneficiaryAcc) 'Collecting Account Details
                    If chq.RetCode = "00" Then
                        LineItemsTable.Add("DESTBRANCH", If(IsDBNull(chq.OurBranch).ToString() = "", "", chq.OurBranch)) ' Dest Branch
                    End If
                End If

                LineItemsTable.Add("CHQDGT", "00") ' Check Digit
                If chq.RetCode = "00" Then
                    LineItemsTable.Add("PBANK", If(IsDBNull(chq.BankCode).ToString() = "", "", chq.BankBIC)) ' PBank
                    LineItemsTable.Add("PBRANCH", If(IsDBNull(chq.BranchCode).ToString() = "", "", chq.BranchCode)) ' PBranch
                Else
                    LineItemsTable.Add("PBANK", chq.BankBIC)
                End If
                LineItemsTable.Add("FILLER", "0") ' Filler
                If chq.FileName.Contains(".Q") Then
                    LineItemsTable.Add("DRAWERORPAYEE", chq.BeneficiaryName) 'Collecting Account Details
                Else
                    LineItemsTable.Add("DRAWERORPAYEE", chq.BeneficiaryName & "*" & chq.RemitterName) 'Collecting Account Details
                End If

                LineItemsTable.Add("SNO", chq.ChequeNumber) ' Serial Number
                LineItemsTable.Add("PROCNO", ProcNo) ' Processing Number
                If chq.RetCode = "00" Then
                    LineItemsTable.Add("DRN", chq.EndorsmentNo) ' Processing Number
                End If
                LineItemsTable.Add("DATA", "/" + chq.ChequeNumber + "/" + Modscan.OurBankID + chq.BranchCode + "/" + chq.RemitterAcc + "/" + chq.MsgId + "/") 'chq.Codeline & "-" & chq.MsgId) ' The Whole String as is
                LineItemsTable.Add("FIMAGESIZEBW", 0)
                LineItemsTable.Add("FIMAGESIGNBW", 0)
                LineItemsTable.Add("FIMAGESIZE", 0)
                LineItemsTable.Add("FIMAGESIGN", 0)
                LineItemsTable.Add("BIMAGESIZE", 0)
                LineItemsTable.Add("BIMAGESIGN", 0) 'myCol.Item(Item).ToString.Substring(197, 48)) ' back tiff image signature
                If chq.RetCode <> "00" Then
                    LineItemsTable.Add("FrontBWImage", "")
                    LineItemsTable.Add("FrontGrayScaleImage", "")
                    LineItemsTable.Add("RearImage", "")
                    LineItemsTable.Add("UVImage", "")
                Else
                    LineItemsTable.Add("FrontBWImage", chq.FrontImageBW)
                    LineItemsTable.Add("FrontGrayScaleImage", chq.FrontImageGS)
                    LineItemsTable.Add("RearImage", chq.BackImageGS)
                    LineItemsTable.Add("UVImage", chq.FrontImageUV)
                    LineItemsTable.Add("UstrdBWF", chq.FrontImageBW)
                    LineItemsTable.Add("UstrdBWR", chq.BackImageGS)
                    LineItemsTable.Add("UstrdGS", chq.FrontImageGS)
                    LineItemsTable.Add("UstrdUV", chq.FrontImageUV)
                    'LineItemsTable.Add("UstrdMicr", chq.UstrdColD)
                End If

                LineItemsTable.Add("FILENAME", chq.FileName) ' The Filename
                LineItemsTable.Add("ValidInvalid", True) 'Validity of the image
                LineItemsTable.Add("IsFCY", False)
                LineItemsTable.Add("MsgID", chq.MsgId)
                LineItemsTable.Add("TrxID", chq.trxID)
                If Modscan.SysType = Modscan.ENUM_SysType.BRNET Then
                    LineItemsTable.Add("Reference", "/" + chq.ChequeNumber + "/" + Modscan.OurBankID + chq.BranchCode + "/" + chq.RemitterAcc + "/" + chq.MsgId + "/") 'chq.reference
                    LineItemsTable.Add("TrxTypeID", "ID")
                End If
                LineItemsTable.Add("UstrdMicr", chq.Codeline)
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
                If chq.RetCode = "00" Then
                    LineItemsTable.Add("UstrdColD", chq.UstrdColD)
                Else
                    LineItemsTable.Add("UstrdColD", "")
                End If
                LineItemsTable.Add("OrgnlEndToEnd", chq.OrgnlEndToEnd)
                LineItemsTable.Add("ReqdColltnDt", chq.IntrBkSttlmDt)
                LineItemsTable.Add("CCNm", "")
                LineItemsTable.Add("DCNm", "")
                LineItemsTable.Add("OrgnlMsgId", chq.OrgnlMsgId)
                If chq.RetCode = "00" Then
                    LineItemsTable.Add("SvcLvl", chq.PymType)
                    LineItemsTable.Add("LclInstrm", chq.LclInstrm)
                    LineItemsTable.Add("CtgyPurp", chq.CtgyPurp)
                End If
                LineItemsTable.Add("RemittanceInfo", chq.RemittanceInfo)

                LineItemsTable.Add("IntrBkSttlmDt", chq.IntrBkSttlmDt)
                LineItemsTable.Add("OrgnlTxId", chq.OrgnlTxId)
                LineItemsTable.Add("SourceBIC", chq.SourceBIC)
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
                                        ColName.DataType = System.Type.GetType(LineItemsTable(name).GetType().FullName.ToString)
                                End Select
                            Else
                                ColName.DataType = System.Type.GetType(LineItemsTable(name).GetType().FullName.ToString)
                            End If


                            Modscan.dt.Columns.Add(ColName)
                        Catch ex As Exception
                            'MsgBox(ex.Message & " - " & ColName.ToString) ' & ": " & LineItemsTable.Item(ColName.ToString).ToString)
                        End Try
                    Next
                End If
                Dim dr As DataRow = Modscan.dt.NewRow()
                For Each name As String In LineItemsTable.Keys
                    Try
                        dr(name) = LineItemsTable(name)
                    Catch ex As Exception

                    End Try
                Next
                Modscan.dt.Rows.Add(dr)
                Modscan.SaveImagesToDB(LineItemsTable, chq.FrontImageBW, chq.FrontImageGS, chq.BackImageGS, chq.FrontImageUV)
                '------------------------------------------------------------------------------------------------------

            Catch ex As Exception
                MsgBox(ex.Message)
            End Try

        End Sub
        Private Shared Sub ReadFile(ByRef sFile As String, ByVal sTemp As String)
            Try
                If CheckLineBelowDocument(sFile) = True Then
                    Dim sline As New List(Of String)(IO.File.ReadAllLines(sFile))
                    Dim p As Integer = sline.LongCount
                    sline.RemoveAt(p - 1)
                    IO.File.WriteAllLines(sFile, sline.ToArray())
                End If
            Catch ex As Exception
                Modscan.ErrorLog("error undressing : " & ex.Message & " : " & sFile, "file upload")
            End Try

        End Sub

        Private Shared Function CheckLineBelowDocument(filePath As String) As Boolean
            Dim foundDocumentClosingTag As Boolean = False

            Using reader As New StreamReader(filePath)
                Dim line As String = ""
                While (InlineAssignHelper(line, reader.ReadLine())) IsNot Nothing
                    If line.Contains("</Document>") Then
                        foundDocumentClosingTag = True
                        ' Check if there is another line after </Document>
                        Return reader.ReadLine() IsNot Nothing
                    End If
                End While
            End Using

            Return False ' </Document> not found
        End Function

        Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
            target = value
            Return value
        End Function
        'Private  Sub SaveEFT(ByVal d As EFTDetails, ByVal sFile As String)
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
        '        'MessageBox.Show("TZ 3049")
        '        'Modscan.dt.TableName = "XMLTest"
        '        'Modscan.dt.WriteXml("C:\\TACH\\TACHfiles\\FromTACH\\Temp\\XmKamunya.xml", True)
        '        Modscan.SaveToDB(Modscan.dt, "IN")

        '        '------------------------------------------------------------------------------------------------------
        '    Catch ex As Exception
        '        MessageBox.Show("Imechapa kwa SaveEFT 2" + ex.Message)
        '    End Try


        'End Sub
        Private Shared Sub SaveEFT(ByVal d As EFTDetails, ByVal sFile As String)
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
                LineItemsTable.Add("TheirACC", d.DbtrAcct)
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
                LineItemsTable.Add("ReqdColltnDt", d.IntrBkSttlmDt)
                LineItemsTable.Add("CCNm", d.CCNm)
                LineItemsTable.Add("DCNm", d.DCNm)
                LineItemsTable.Add("OrgnlMsgId", d.OrgnlMsgId)
                LineItemsTable.Add("SvcLvl", d.SvcLvl)
                LineItemsTable.Add("IntrBkSttlmDt", d.IntrBkSttlmDt)
                LineItemsTable.Add("TrxId", d.TrxId)
                LineItemsTable.Add("OrgnlTxId", d.OrgnlTxId)
                LineItemsTable.Add("RemittanceInfo", d.RemittanceInfo)
                LineItemsTable.Add("SourceBIC", d.SourceBankID)
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

        Private Shared Sub ExecuteCommand(ByVal strBatchPath As String)
            Dim ExitCode As Integer = -1
            Dim output As String = ""
            Dim [error] As String = ""

            Try
                Using process__1 As New Process()
                    With process__1.StartInfo
                        .FileName = "cmd.exe"
                        .Arguments = "/c """ & strBatchPath & """"
                        .CreateNoWindow = True
                        .UseShellExecute = False
                        .WorkingDirectory = Path.GetDirectoryName(strBatchPath)
                        .RedirectStandardError = True
                        .RedirectStandardOutput = True
                        .RedirectStandardInput = True
                    End With

                    process__1.Start()

                    ' Read output and error asynchronously to avoid deadlocks
                    output = process__1.StandardOutput.ReadToEnd()
                    [error] = process__1.StandardError.ReadToEnd()

                    process__1.WaitForExit()
                    ExitCode = process__1.ExitCode
                End Using

                ' Log based on exit code or error content
                If ExitCode <> 0 OrElse [error] <> "" Then
                    Modscan.ErrorLog("ExitCode: " & ExitCode, "ExecuteCommand-TZ Files")
                    Modscan.ErrorLog("Output: " & (If(String.IsNullOrEmpty(output), "(none)", output)), "ExecuteCommand-TZ Files")
                    Modscan.ErrorLog("Error: " & (If(String.IsNullOrEmpty([error]), "(none)", [error])), "ExecuteCommand-TZ Files")
                End If

            Catch ex As Exception
                Modscan.ErrorLog("Exception in ExecuteCommand: " & ex.Message, "ExecuteCommand-TZ Files")
            Finally
                ' Clean up batch file
                Try
                    If File.Exists(strBatchPath) Then
                        File.Delete(strBatchPath)
                    End If
                Catch ex As Exception
                    Modscan.ErrorLog("Failed to delete batch file: " & ex.Message, "ExecuteCommand-TZ Files")
                End Try
            End Try
        End Sub




        Private Shared Function ExecuteCommandService(ByVal batchPath As String, ByVal workingDirectory As String) As Integer
            Dim exitCode As Integer = -1
            Dim output As New StringBuilder()
            Dim errors As New StringBuilder()

            Try
                Using process As New Process()
                    process.StartInfo.FileName = "cmd.exe"
                    process.StartInfo.Arguments = $"/c ""{batchPath}"""
                    process.StartInfo.CreateNoWindow = True
                    process.StartInfo.UseShellExecute = False
                    process.StartInfo.RedirectStandardOutput = True
                    process.StartInfo.RedirectStandardError = True
                    process.StartInfo.WorkingDirectory = workingDirectory

                    ' Services need these settings:
                    process.StartInfo.LoadUserProfile = True
                    process.StartInfo.Verb = "runas"  ' Run with elevated privileges if needed

                    process.Start()

                    ' Read output asynchronously to avoid deadlocks
                    output.Append(process.StandardOutput.ReadToEnd())
                    errors.Append(process.StandardError.ReadToEnd())

                    process.WaitForExit(60000) ' 60 second timeout
                    exitCode = process.ExitCode

                    ' Log everything for debugging
                    If output.Length > 0 Then
                        Modscan.ErrorLog($"Process Output:{Environment.NewLine}{output}", "ExecuteCommand-Output")
                    End If

                    If errors.Length > 0 Then
                        Modscan.ErrorLog($"Process Errors:{Environment.NewLine}{errors}", "ExecuteCommand-Errors")
                    End If

                End Using

            Catch ex As Exception
                Modscan.ErrorLog($"ExecuteCommandService Exception: {ex.Message}", "ExecuteCommand-Error")
            End Try

            Return exitCode
        End Function




        'working locally 
        Private Shared Sub UnSignFile(ByVal sFile As String)
            Try
                ' Validate file exists
                If Not File.Exists(sFile) Then
                    Modscan.ErrorLog("File does not exist: " & sFile, "UnSignFile-TZ Files")
                    Return
                End If

                ' Get configuration values
                Dim strWorkingDir As String = Path.GetDirectoryName(Modscan.strBatchPath)
                Dim strDSkeyFile As String = Encoding.ASCII.GetString(Convert.FromBase64String(ConfigurationManager.AppSettings("keypass")))

                ' Create command directly without batch file
                Dim javaExePath As String = Modscan.strJavaExeInstallation.Trim()
                If Not File.Exists(javaExePath) Then
                    Modscan.ErrorLog("Java executable not found: " & javaExePath, "UnSignFile-TZ Files")
                    Return
                End If

                ' Build the Java command
                Dim arguments As String = String.Format(
            "-cp .;com.springsource.org.bouncycastle.jce-1.46.0.jar;com.springsource.org.bouncycastle.mail-1.46.0.jar SignatureClient " &
            "DSkeyFile=""{0}"" fileName=""{1}"" function=unsign mode=CMS",
            strDSkeyFile.Replace("\", "/"),
            sFile.Replace("\", "/"))

                ' Execute directly without batch file
                Using process As New Process()
                    With process.StartInfo
                        .FileName = javaExePath
                        .Arguments = arguments
                        .CreateNoWindow = True
                        .UseShellExecute = False
                        .WorkingDirectory = strWorkingDir
                        .RedirectStandardError = True
                        .RedirectStandardOutput = True
                    End With

                    process.Start()

                    Dim output As String = process.StandardOutput.ReadToEnd()
                    Dim [error] As String = process.StandardError.ReadToEnd()

                    process.WaitForExit()

                    ' Log results
                    If process.ExitCode <> 0 OrElse [error] <> "" Then
                        Modscan.ErrorLog("Unsigned file: " & sFile, "UnSignFile-TZ Files")
                        Modscan.ErrorLog("Java Exit Code: " & process.ExitCode, "UnSignFile-TZ Files")
                        Modscan.ErrorLog("Output: " & (If(String.IsNullOrEmpty(output), "(none)", output)), "UnSignFile-TZ Files")
                        Modscan.ErrorLog("Error: " & (If(String.IsNullOrEmpty([error]), "(none)", [error])), "UnSignFile-TZ Files")
                    Else
                        Modscan.ErrorLog("Successfully unsigned file: " & sFile, "UnSignFile-TZ Files")
                    End If
                End Using

            Catch ex As Exception
                Modscan.ErrorLog("Exception in UnSignFile: " & ex.Message & " StackTrace: " & ex.StackTrace, "UnSignFile-TZ Files")
            End Try
        End Sub

        'Private Shared Sub UnSignFile(ByVal sFile As String)
        '    Try
        '        sFile = sFile.Replace("\", "/")
        '        Modscan.strBatchPath = Modscan.strBatchPath
        '        'Modscan.strBatchPath = Modscan.strBatchPath & "Execute.bat"
        '        Modscan.strDSkeyFile = Encoding.ASCII.GetString(Convert.FromBase64String(ConfigurationManager.AppSettings("keypass")))

        '        Dim strCmd As String = """" & Modscan.strJavaExeInstallation.Trim() & """ -cp .;com.springsource.org.bouncycastle.jce-1.46.0.jar;com.springsource.org.bouncycastle.mail-1.46.0.jar SignatureClient DSkeyFile=" _
        '                           & Modscan.strDSkeyFile.Trim().Replace("\", "/") & " fileName=" & sFile & " function=unsign mode=CMS"
        '        Dim myFileStream As FileStream = Nothing
        '        Dim myEJContentStreamWriter As StreamWriter = Nothing
        '        Try
        '            myEJContentStreamWriter = New StreamWriter(Modscan.strBatchPath, True)
        '            myEJContentStreamWriter.WriteLine(strCmd)
        '        Finally
        '            If Not (myEJContentStreamWriter Is Nothing) Then myEJContentStreamWriter.Close()
        '        End Try
        '        ExecuteCommand(Modscan.strBatchPath)


        '    Catch ex As Exception
        '        Modscan.ErrorLog("output>>" & (If([String].IsNullOrEmpty(ex.Message), "(none)", ex.Message)), "SignFile-TZ Files")
        '    End Try
        'End Sub
        'Private Shared Sub ExecuteCommand(ByVal strBatchPath As String)
        '    Dim ExitCode As Integer
        '    Dim ProcessInfo As ProcessStartInfo
        '    Dim process__1 As Process = Nothing
        '    Dim output As String = ""
        '    Dim [error] As String = ""
        '    Dim strWorkingDir As String = Path.GetDirectoryName(strBatchPath)
        '    Try
        '        ProcessInfo = New ProcessStartInfo(strBatchPath)
        '        ProcessInfo.CreateNoWindow = True
        '        ProcessInfo.UseShellExecute = False
        '        ProcessInfo.WorkingDirectory = strWorkingDir

        '        ProcessInfo.RedirectStandardError = True
        '        ProcessInfo.RedirectStandardOutput = True

        '        process__1 = Process.Start(ProcessInfo)
        '        process__1.WaitForExit()

        '        output = process__1.StandardOutput.ReadToEnd()
        '        [error] = process__1.StandardError.ReadToEnd()
        '        If [error] <> "" Then
        '            Modscan.ErrorLog("output>>" & (If([String].IsNullOrEmpty(output), "(none)", output)), "ExecuteCommand-TZ Files")
        '            Modscan.ErrorLog("error>>" & (If([String].IsNullOrEmpty([error]), "(none)", [error])), "ExecuteCommand-TZ Files")
        '        End If
        '    Catch ex As Exception
        '        Modscan.ErrorLog("output>>" & (If([String].IsNullOrEmpty(output), "(none)", output)), "ExecuteCommand-TZ Files")
        '        Modscan.ErrorLog("error>>" & (If([String].IsNullOrEmpty([error]), "(none)", [error])), "ExecuteCommand-TZ Files")
        '    End Try
        '    ExitCode = process__1.ExitCode
        '    process__1.Close()
        '    Kill(strBatchPath)
        'End Sub
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
