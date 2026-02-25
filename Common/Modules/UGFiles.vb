Imports System.IO
Imports Ionic.Zip
Imports dd316 = BrClearing.Common.BRISO20022DD316
Imports dd416 = BrClearing.Common.BRISO20022DD416
Imports ct816 = BrClearing.Common.BRISO20022CT816
Imports res = BrClearing.Common.ISOUG.Responses
Imports System.Text.RegularExpressions
Imports System.Windows.Forms
Imports System.Configuration
Imports System.Collections.Specialized
Imports BRBase
Imports BRCATDS

#Region "Structures"
Public Structure DDUGDetail
    Dim MsgId As String
    Dim sBIC As String
    Dim dBIC As String
    Dim DestBankID As String
    Dim SourceBankID As String
    Dim TxId As String
    Dim Curr As String
    Dim Amount As String
    Dim Retcode As String
    Dim VCode As String
    Dim Frqcy As String
    Dim OrgnlMsgId As String
    Dim OrgnTrxID As String
    Dim RetCodeDesc As String
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
    Dim OrgnlInstrID As String
    Dim UstrdColD As String
    Dim OrgnlEndToEnd As String
    Dim ReqdColltnDt As String
    Dim CCNm As String
    Dim DCNm As String
    Dim FnlColltnDt As String
End Structure
Public Structure ACHResponse
    Dim RtrId As String
    Dim OrgnlMsgId As String
    Dim OrgnTrxID As String
    Dim OrgnlInstrID As String
    Dim OrgnlEndToEnd As String
    Dim RetCode As String
    Dim RtrdIntrBkSttlmAmt As String
    Dim FileName As String
End Structure
Public Structure EFTUGDetails

    Dim Currency As String
    Dim ISOCurrency As String
    Dim Amount As String
    Dim DestBIC As String
    Dim RetCode As String
    Dim ValueDate As Date
    Dim MsgId As String
    Dim TrxId As String
    Dim TrxData As String
    Dim VCode As String
    Dim Frqcy As String
    Dim SourceBankID As String
    Dim DestBankID As String
    Dim OrgnlMsgId As String
    Dim OrgnTrxID As String
    Dim OurBankBic As String
    Dim RetCodeDesc As String
    Dim OrgnRef As String
    Dim UstrdMicr As String
    Dim DAdrLine As String
    Dim OrgnlEndToEnd As String
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
    Dim ReqdColltnDt As String
    Dim CCNm As String
    Dim DCNm As String
    Dim Ustrd As String
End Structure

Public Structure UGChequeDetails
    Dim MICRED As Boolean
    Dim TransCode As String
    Dim RetCode As String
    Dim BranchCode As String
    Dim CreationDate As String
    Dim BeneficiaryAcc As String
    Dim ChequeNumber As String
    Dim ChequeIndex As String
    Dim RemittanceInfo As String
    Dim BankCode As String
    Dim BankBIC As String
    Dim Amount As Decimal
    Dim OrgnlEndToEnd As String
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
    Dim TrxID As String
    Dim reference As String
    Dim OurBankBic As String
    Dim ImageCounter As String
    Dim Sess As String
    Dim Region As String
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
    Dim ReqdColltnDt As String
    Dim OrgnTrxID As String
    Dim CCNm As String
    Dim DCNm As String
End Structure
#End Region

Public Class BRUGClass
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
    End Enum

    Public Enum ChequeFormat
        SISPackage = 0
        XMLPackages = 1
    End Enum
#End Region

#Region "Varibles"
    Private Shared Sign As Boolean = True
    Private Shared CertName As String = "test"
    Dim dtProcessingDate As Date = Date.Now
    Private Shared strFileLocation As String = ""
    Dim sArchive As String = ""
    Dim strFileName As String = ""
    Private Shared StrDestinationFilePath As String = ""
    Public Files As FileType
    Shared TempLocation As String = ConfigurationManager.AppSettings("IncomingFiles")
#End Region

#Region "Constructors"
    Public Sub New(ByVal Location As String)
        If Not Directory.Exists(strFileLocation) Then Directory.CreateDirectory(strFileLocation)
        If Not Directory.Exists(TempLocation) Then Directory.CreateDirectory(TempLocation)
    End Sub
#End Region

#Region "Methods"
    Public Shared Function GenerateUGFiles(ByVal FileType As FileType, ByVal CurrCode As Int32, ByVal Exclude As Boolean, Optional ByVal chqFormat As ChequeFormat = ChequeFormat.XMLPackages, Optional ByVal Session As Int16 = 1, Optional ByVal x As String = "", Optional ByVal y As String = "") As Boolean
        Try


            Dim RegX As New Regex("[^A-Za-z0-9]")
            Dim strDBAction As String = ""
            Dim strAction As String = ""


            Dim DestBankBIC As String = ""
            Dim Util As New DataTable()
            Dim BankArr As ArrayList = Nothing
            strFileLocation = ConfigurationManager.AppSettings("OutgoingFiles")
            TempLocation = strFileLocation & "\Temp"
            StrDestinationFilePath = strFileLocation & "\Files"
            If Not Directory.Exists(StrDestinationFilePath) Then Directory.CreateDirectory(StrDestinationFilePath)
            If Not Directory.Exists(TempLocation) Then Directory.CreateDirectory(TempLocation)
            'Banks
            Dim publicDTblBankCopy As DataTable
            Dim publicDTblEJCopy As DataTable
            Dim publicDTblEFTCrCopy As DataTable
            Dim distinctBankID As DataTable
            Dim SystemType As String = ConfigurationManager.AppSettings("sysType")


            Modscan.cWORKING_DATE = Format(Modscan.WORKING_DATE, "dd-MMM-yyyy")
            Modscan.cFromDate = Format(Convert.ToDateTime(Modscan.cFromDate), "dd-MMM-yyyy")
            Modscan.cToDate = Format(Convert.ToDateTime(Modscan.cToDate), "dd-MMM-yyyy")
            Try
                'MessageBox.Show(Modscan.OurBankID & " - " & CurrCode & " - " & Modscan.OurBranchID & " - " & Modscan.WORKING_DATE & " - " & Modscan.cFromDate & " - " & Modscan.cToDate)
                Select Case FileType
                    Case FileType.Cheques, FileType.ChequeReturn
                        Modscan.ExecuteData(Modscan.GetModify("Proc_CreateXMLFiles_UG_CTS", "OurBankID", Modscan.OurBankID, "ourbranchid", Modscan.OurBranchID, "ReadDate", Modscan.cWORKING_DATE, "FromDate", Modscan.cFromDate, "EJDate", Modscan.cToDate, "ClearingCenters", "47", "AllCenters", 0, "Currency", CurrCode, "FileType", "J", "Sessno", Session), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.SelectStatement, Modscan.publicDset)
                        If Modscan.publicDTbl.Rows.Count > 0 Then
                            publicDTblBankCopy = Modscan.publicDset.Tables(0).Clone()
                            distinctBankID = Modscan.publicDset.Tables(0).Clone()
                            For i As Int32 = 0 To Modscan.publicDTbl.Rows.Count - 1
                                distinctBankID.ImportRow(Modscan.publicDTbl.Rows(i))
                            Next
                            publicDTblBankCopy = distinctBankID.DefaultView.ToTable(True, "BankID")
                            distinctBankID.Clear()
                            Modscan.publicDTbl.Clear()

                            BankArr = New ArrayList
                            For i As Int32 = 0 To publicDTblBankCopy.Rows.Count - 1
                                BankArr.Add(publicDTblBankCopy.Rows(i)("BankID").ToString)
                            Next
                        Else
                            'MessageBox.Show("There are no pending cheques/unpaid Cheques for generation")
                            Exit Function
                        End If

                    Case FileType.Efts, FileType.EftReturn, FileType.DD, FileType.DDReturn
                        Modscan.ExecuteData(Modscan.GetModify("Proc_CreateXMLFiles_UG_CTS", "OurBankID", Modscan.OurBankID, "ourbranchid", Modscan.OurBranchID, "ReadDate", Modscan.cWORKING_DATE, "FromDate", Modscan.cFromDate, "EJDate", Modscan.cToDate, "ClearingCenters", "47", "AllCenters", 0, "Currency", CurrCode, "FileType", "T", "Sessno", Session), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure, Modscan.publicDset)
                        If Modscan.publicDTbl.Rows.Count > 0 Then
                            publicDTblBankCopy = Modscan.publicDset.Tables(0).Clone()
                            distinctBankID = Modscan.publicDset.Tables(0).Clone()
                            For i As Int32 = 0 To Modscan.publicDset.Tables(0).Rows.Count - 1
                                distinctBankID.ImportRow(Modscan.publicDset.Tables(0).Rows(i))
                            Next
                            publicDTblBankCopy = distinctBankID.DefaultView.ToTable(True, "BankID")
                            Modscan.publicDset.Tables(1).Clear()
                            distinctBankID.Clear()

                            BankArr = New ArrayList
                            For i As Int32 = 0 To publicDTblBankCopy.Rows.Count - 1
                                BankArr.Add(publicDTblBankCopy.Rows(i)("BankID").ToString)
                            Next
                        Else
                            'MessageBox.Show("There are no pending Efts/unpaid EFTs for generation")
                            Exit Function
                        End If
                End Select


            Catch ex As Exception
                MessageBox.Show("Error Registered, Check ErrorLog")
                Modscan.ErrorLog("Swift Code not maintained for this our BankID", "Cheques Generation" & ex.Message)
            End Try
            'MessageBox.Show("Inaenda FileType.Cheques - " & CurrCode)
            Select Case FileType
                Case FileType.Cheques
                    For k As Int32 = 0 To BankArr.Count - 1
                        Try
                            Dim i As Integer = 0
                            Dim amt As Decimal = 0
                            Dim l As New List(Of UGChequeDetails)
                            Dim CreateFile As String = Session & Now.ToString(".yyyyMMdd.HHmm.") & BankArr.Item(k) & "." & Session
                            Dim BIC As String = ""
                            'Ejs
                            Modscan.ExecuteData(Modscan.GetModify("Proc_CreateXMLFiles_UG_CTS", "OurBankID", Modscan.OurBankID, "ourbranchid", Modscan.OurBranchID, "ReadDate", Modscan.cWORKING_DATE, "FromDate", Modscan.cFromDate, "EJDate", Modscan.cToDate, "ClearingCenters", "47", "AllCenters", 0, "Currency", CurrCode, "FileType", "J", "Sessno", Session), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure)
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
                                    Dim chq As New UGChequeDetails
                                    chq.Amount = CDec(r("Amount"))
                                    chq.BackImageGS = DirectCast(Modscan.String2Bytes(r("BackImageGrayScale")), Byte())
                                    chq.BankCode = r("BankID").ToString()
                                    chq.BankBIC = r("DestinationSwiftCode").ToString()
                                    'For UG testing Purpose------------------------------------------------
                                    'chq.BankBIC = chq.BankBIC.Substring(0, chq.BankBIC.Length - 1) + "0"
                                    '----------------------------------------------------------------------
                                    chq.OurBankID = Modscan.OurBankID
                                    chq.OurBranchID = r("OurBranchID").ToString()
                                    chq.BeneficiaryAcc = r("AccountID").ToString()
                                    chq.CurrencyCode = CurrCode
                                    chq.BeneficiaryName = RegX.Replace(r("BeneficiaryName").ToString(), " ")
                                    If chq.BeneficiaryName.Length > 55 Then chq.BeneficiaryName = chq.BeneficiaryName.Substring(0, 55)
                                    chq.BranchCode = r("BranchID").ToString().PadLeft(3, "0")
                                    chq.ChequeIndex = r("ChequeDigit").ToString().Trim.PadLeft(2, "0")
                                    chq.ChequeNumber = r("ChequeId").ToString()
                                    chq.Codeline = r("MicrLineDetails").ToString()
                                    chq.CreationDate = Date.Now.ToString("yyyy-MM-dd HH\\:mm\\:ss")
                                    chq.EndorsmentNo = Modscan.GetRandomInt16
                                    chq.FileName = Path.Combine(strFileLocation, CreateFile)
                                    chq.ImageCounter = r("ImageCounter").ToString().PadLeft(6, "0")
                                    chq.Sess = r("SessNo").ToString().PadLeft(2, "0")
                                    chq.TrxID = r("TransactionMicrColumnID")
                                    chq.Region = r("TheirRegion")


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


                                            chq.FrontImageGS = Modscan.String2Bytes(r("FrontImageGrayScale"))
                                            chq.FrontImageGS = Modscan.String2Bytes(r("FrontImageGrayScale"))
                                            chq.FrontImageUV = Modscan.String2Bytes(r("FrontImageUV"))
                                            chq.FrontImageBW = Modscan.String2Bytes(r("FrontImageTiff"))

                                        Case "BRMFO"
                                            chq.FrontImageGS = Modscan.ImageToByte(Modscan.Bytes2Image(DirectCast(System.Text.Encoding.GetEncoding(1252).GetBytes(r("FrontImageGrayScale")), Byte())))
                                            chq.FrontImageGS = Modscan.ImageToByte(Modscan.Bytes2Image(DirectCast(System.Text.Encoding.GetEncoding(1252).GetBytes(r("FrontImageGrayScale")), Byte())))
                                            chq.FrontImageUV = Modscan.ImageToByte(Modscan.Bytes2Image(DirectCast(System.Text.Encoding.GetEncoding(1252).GetBytes(r("FrontImageUV")), Byte())))
                                            chq.FrontImageBW = Modscan.ImageToByteTif(Modscan.Bytes2Image(DirectCast(System.Text.Encoding.GetEncoding(1252).GetBytes(r("FrontImageTiff")), Byte())))
                                        Case "BRNET"
                                            chq.FrontImageGS = Modscan.ImageToByte(Modscan.Bytes2Image(Modscan.String2Bytes(r("FrontImageGrayScale"))))
                                            chq.FrontImageGS = Modscan.ImageToByte(Modscan.Bytes2Image(Modscan.String2Bytes(r("FrontImageGrayScale"))))
                                            chq.FrontImageUV = Modscan.ImageToByte(Modscan.Bytes2Image(Modscan.String2Bytes(r("FrontImageUV"))))
                                            chq.FrontImageBW = Modscan.ImageToByteTif(Modscan.Bytes2Image(Modscan.String2Bytes(r("FrontImageTiff"))))
                                        Case "BRNETOLD"
                                            chq.FrontImageGS = Modscan.ImageToByte(Modscan.Bytes2Image(Modscan.String2Bytes(r("FrontImageGrayScale"))))
                                            chq.FrontImageGS = Modscan.ImageToByte(Modscan.Bytes2Image(Modscan.String2Bytes(r("FrontImageGrayScale"))))
                                            chq.FrontImageUV = Modscan.ImageToByte(Modscan.Bytes2Image(Modscan.String2Bytes(r("FrontImageUV"))))
                                            chq.FrontImageBW = Modscan.ImageToByteTif(Modscan.Bytes2Image(Modscan.String2Bytes(r("FrontImageTiff"))))
                                    End Select
                                    chq.MICRED = True
                                    chq.RemittanceInfo = r("TransactionMicrColumnID").ToString()
                                    chq.RemitterAcc = r("TheirAccountID").ToString()
                                    chq.RemitterName = RegX.Replace(r("RemittersName").ToString(), " ")
                                    If chq.RemitterName.Length > 55 Then chq.RemitterName = chq.RemitterName.Substring(0, 55)
                                    chq.TransCode = r("VoucherCode").ToString()
                                    chq.ValueDate = r("TrxDate")
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
                                        Dim FileCounter As String = GetScalarREC("Select isNull(FileCounter,0)+1 From t_Bank Where BankID = '" & l(0).BankCode & "'")
                                        CreateFile = DestBankBIC.Substring(0, 4) & Now.ToString("yyyyMMddHHmmss") & FileCounter & ".chk"
                                        'For UG Testing Purpose----------------------------------------
                                        'BIC = BIC.Substring(0, BIC.Length - 1) + "0"
                                        '-------------------------------------------------------
                                        Dim msgId As String = BulkCheques(l, l(0).CurrencyCode, amt, BIC, x, y)


                                        'This awaits the token
                                        'ZipContents(CreateFile, msgId, New String() {"*.J*", "*.M*"}, "", True)
                                        For Each c As UGChequeDetails In l
                                            Select Case SystemType.ToUpper.Trim
                                                Case "BR"
                                                    strAction = "UPDATE t_TransactionMICR SET IsGenerated = 1, Reference = '" & msgId & "', ExtraDetails = '" & c.Codeline & "',  MicrLine ='" & c.Codeline & "' WHERE ColumnID = '" & c.RemittanceInfo & "'"
                                                Case "BRMFO"
                                                    strAction = "UPDATE t_TransactionMICR SET IsGenerated = 1, Reference = '" & msgId & "', ExtraDetails = '" & c.Codeline & "',  MicrLine ='" & c.Codeline & "' WHERE ColumnID = '" & c.RemittanceInfo & "'"
                                                Case "BRNET"
                                                    strAction = "UPDATE t_TrxClearing SET IsGenerated = 1 ,Reference = '" & msgId & "' WHERE TrxRowID = '" & c.TrxID & "'"
                                                Case "BRNETOLD"
                                                    strAction = "UPDATE t_TrxClearing SET IsGenerated = 1 ,Reference = '" & msgId & "' WHERE TrxRowID = '" & c.RemittanceInfo & "'"
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
                            strAction = "UPDATE t_Bank SET FileCounter = isNull(FileCounter,0) + 1 WHERE BankID = '" & BankArr.Item(k) & "'"
                            Modscan.ExecuteData(strAction, Nothing, Modscan.dataExecTypes.ExecTypeNonQuery, Modscan.queryType.SelectStatement)

                        Catch ex As Exception
                            MessageBox.Show("Error Registered, Check ErrorLog")
                            Modscan.ErrorLog(ex.Message, "- Cheques Generation")
                            Continue For
                        End Try
                    Next
                    'MessageBox.Show("Inaenda FileType.ChequeReturn - " & CurrCode)
                    publicDTblEJCopy.Clear()
                Case FileType.ChequeReturn
                    For k As Int32 = 0 To BankArr.Count - 1
                        Try
                            'MessageBox.Show("Imefika Hapa 2 - " & CurrCode)
                            Dim i As Integer = 0
                            Dim amt As Decimal = 0
                            Dim l As New List(Of UGChequeDetails)
                            Dim FileCounter As String = GetScalarREC("Select isNull(FileCounter,0)+1 From t_Bank Where BankID = '" & BankArr.Item(k) & "'")
                            Dim CreateFile As String = Session & Now.ToString(".yyyyMMdd.HHmm.") & BankArr.Item(k) & "." & Session
                            Dim BIC As String = ""
                            'Ejs Rejects
                            Modscan.ExecuteData(Modscan.GetModify("Proc_CreateXMLFiles_UG_CTS", "OurBankID", Modscan.OurBankID, "ourbranchid", Modscan.OurBranchID, "ReadDate", Modscan.cWORKING_DATE, "FromDate", Modscan.cFromDate, "EJDate", Modscan.cToDate, "ClearingCenters", "47", "AllCenters", 0, "Currency", CurrCode, "FileType", "J", "Sessno", Session), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure)
                            Dim EJUNPfoundRows() As DataRow
                            Select Case SystemType.ToUpper.Trim
                                Case "BR"
                                    EJUNPfoundRows = Modscan.publicDTbl.Select("TrxType ='O' AND ReturnCode <>'00' AND BankID = '" & BankArr.Item(k) & "' And IsGenerated=false")
                                Case "BRMFO"
                                    EJUNPfoundRows = Modscan.publicDTbl.Select("TrxType ='O' AND ReturnCode <>'00' AND BankID = '" & BankArr.Item(k) & "' And IsGenerated=false")
                                Case "BRNET"
                                    EJUNPfoundRows = Modscan.publicDTbl.Select("TrxType ='OC' AND ReturnCode <>'00' AND BankID = '" & BankArr.Item(k) & "' And IsGenerated=false")
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
                                    Dim chq As New UGChequeDetails
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
                                    chq.TrxID = r("ColumnID").ToString()
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
                                    chq.DCNm = r("DCNm").ToString()
                                    chq.CCNm = r("CCNm").ToString()
                                    chq.DbtrAcct = r("DbtrAcct").ToString()
                                    chq.CdtrAcct = r("CdtrAcct").ToString()
                                    chq.OrgnlEndToEnd = r("OrgnlEndToEnd").ToString()
                                    chq.ReqdColltnDt = IIf(r("ReqdColltnDt").ToString() = "Jan  1 1900 12:00AM", "", r("ReqdColltnDt").ToString())
                                    l.Add(chq)
                                Catch ex As Exception
                                    MessageBox.Show("Error Registered, Check ErrorLog")
                                    Modscan.ErrorLog(ex.Message, "Unpaid Cheques Generation")
                                    Continue For
                                End Try
                            Next
                            If l.Count > 0 Then
                                Dim msgId As String = UnpaidCheques(l, DestBankBIC, l(0).CurrencyCode, x, y)
                                For Each s As UGChequeDetails In l
                                    Try
                                        Select Case SystemType.ToUpper.Trim
                                            Case "BR"
                                                strAction = "UPDATE t_TransactionMICR SET IsGenerated = 1, Reference = '" & msgId & "' , RejectedReason = '" & s.RetCode & "' ,  MicrLine ='" & s.Codeline & "' WHERE ColumnID = '" & s.RemittanceInfo & "'"
                                            Case "BRMFO"
                                                strAction = "UPDATE t_TransactionMICR SET IsGenerated = 1, Reference = '" & msgId & "' , RejectedReason = '" & s.RetCode & "' ,  MicrLine ='" & s.Codeline & "' WHERE ColumnID = '" & s.RemittanceInfo & "'"
                                            Case "BRNET"
                                                strAction = "UPDATE t_TrxClearing SET IsGenerated = 1, Reference = '" & msgId & "' WHERE TrxRowID = '" & s.TrxID & "'"
                                            Case "BRNETOLD"
                                                strAction = "UPDATE t_TrxClearing SET IsGenerated = 1, SessNo='1' ,Reference = '" & msgId & "'  WHERE TrxRowID = '" & s.RemittanceInfo & "'"
                                        End Select
                                        Modscan.ExecuteData(strAction, Nothing, Modscan.dataExecTypes.ExecTypeNonQuery, Modscan.queryType.SelectStatement)
                                    Catch ex As Exception
                                        MessageBox.Show("Error Registered, Check ErrorLog")
                                        Modscan.ErrorLog(ex.Message, "Unpaid Cheques Generation")
                                        Continue For
                                    End Try
                                Next
                            End If
                            strAction = "UPDATE t_Bank SET FileCounter = isNull(FileCounter,0) + 1 WHERE BankID = '" & BankArr.Item(k) & "'"
                            Modscan.ExecuteData(strAction, Nothing, Modscan.dataExecTypes.ExecTypeNonQuery, Modscan.queryType.SelectStatement)
                            System.Threading.Thread.Sleep(300)
                            Application.DoEvents()
                        Catch ex As Exception
                            MessageBox.Show("Error Registered, Check ErrorLog ")
                            Modscan.ErrorLog(ex.Message, "Unpaid Cheques Generation 2 ")
                            Continue For
                        End Try
                    Next
                    publicDTblEJCopy.Clear()
                Case FileType.RTGS

                Case FileType.Efts

                    For k As Int32 = 0 To BankArr.Count - 1
                        Try
                            Dim i As Integer = 0
                            Dim amt As Decimal = 0
                            Dim l As New List(Of TZ.ChequeDetails)
                            Dim FileCounter As String = GetScalarREC("Select isNull(FileCounter,0)+1 From t_Bank Where BankID = '" & BankArr.Item(k) & "'")
                            Dim CreateFile As String = Session & Now.ToString(".yyyyMMdd.HHmm.") & BankArr.Item(k) & "." & Session
                            Dim BIC As String = ""
                            'EFT Cr
                            Modscan.ExecuteData(Modscan.GetModify("Proc_CreateXMLFiles_UG_CTS", "OurBankID", Modscan.OurBankID, "ourbranchid", Modscan.OurBranchID, "ReadDate", Modscan.cWORKING_DATE, "FromDate", Modscan.cFromDate, "EJDate", Modscan.cToDate, "ClearingCenters", "47", "AllCenters", 0, "Currency", CurrCode, "FileType", "T", "Sessno", Session), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure, Modscan.publicDset)
                            Dim EFTCrfoundRows() As DataRow
                            Select Case SystemType.ToUpper.Trim
                                Case "BR"
                                    EFTCrfoundRows = Modscan.publicDset.Tables(1).Select("TrxType ='0' AND VoucherCode <> '40' AND BankID = '" & BankArr.Item(k).ToString.Trim() & "'  And IsGenerated=false")
                                Case "BRMFO"
                                    EFTCrfoundRows = Modscan.publicDset.Tables(1).Select("TrxType ='0' AND VoucherCode <> '40' AND BankID = '" & BankArr.Item(k).ToString.Trim() & "'  And IsGenerated=false")
                                Case "BRNET"
                                    EFTCrfoundRows = Modscan.publicDTbl.Select("TrxType ='OD' AND VoucherCode <> '40'  AND ReturnCode ='00' AND BankID = '" & BankArr.Item(k).ToString.Trim() & "'  And IsGenerated=false")
                                Case "BRNETOLD"
                                    EFTCrfoundRows = Modscan.publicDTbl.Select("TrxType ='OD' AND VoucherCode <> '40' AND BankID = '" & BankArr.Item(k).ToString.Trim() & "'  And IsGenerated=false")
                            End Select
                            publicDTblEFTCrCopy = Modscan.publicDset.Tables(0).Clone()
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
                                Continue For
                            End Try
                            Try
                                If publicDTblEFTCrCopy.Rows.Count > 0 Then
                                    DestBankBIC = ""
                                    DestBankBIC = publicDTblEFTCrCopy(0)("DestinationSwiftCode").ToString()
                                End If
                            Catch ex As Exception
                                MessageBox.Show("Error Registered, Check ErrorLog")
                                Modscan.ErrorLog("Swift Code not maintained for this BankID " & publicDTblEJCopy(0)("BankID") & " EFT generation for this Bank Abort", "- EFT Generation")
                                Continue For
                            End Try
                            Dim cr As New List(Of EFTUGDetails)
                            For Each row As DataRow In publicDTblEFTCrCopy.Rows
                                Try
                                    Dim destBIC As String = row("DestinationSwiftCode").ToString()
                                    Dim SourceBIC As String = row("SwiftCode").ToString()
                                    Dim d As New EFTUGDetails
                                    d.Amount = FormatNumber(row("Amount"), 2)
                                    d.CdtrAcct = row("TheirAccountID")
                                    d.Currency = CurrCode
                                    d.CNm = RegX.Replace(row("BeneficiaryName").Trim(), " ")
                                    If d.CNm.Length > 55 Then d.CNm = d.CNm.Substring(0, 55)
                                    d.ISOCurrency = row("CurrencyCode")
                                    d.DestBIC = destBIC
                                    d.DestBankID = row("BankID")
                                    d.SourceBankID = Modscan.OurBankID
                                    d.DbtrAcct = row("AccountID")
                                    d.OurBankBic = SourceBIC
                                    Try
                                        d.UstrdColD = row("OriginatorReference")
                                    Catch ex As Exception
                                        d.UstrdColD = ""
                                    End Try

                                    d.DNm = RegX.Replace(row("RemittersName"), " ")
                                    If d.DNm.Length > 55 Then d.DNm = d.DNm.Substring(0, 55)
                                    d.VCode = row("VoucherCode").ToString().Split("-")(0).Trim()
                                    d.VCode = row("VoucherCode").ToString()
                                    'd.ImageCounter = row("ImageCounter").ToString().PadLeft(6, "0")
                                    d.TrxId = row("TransactionMicrColumnID")
                                    'd.Region = row("TheirRegion")
                                    cr.Add(d)
                                Catch ex As Exception
                                    MessageBox.Show("Error Registered, Check ErrorLog")
                                    Modscan.ErrorLog(ex.Message, "- EFT Generation")
                                    Continue For
                                End Try
                            Next
                            If cr.Count > 0 Then
                                Dim msgId As String = BulkCredit(cr, cr(0).Currency, BIC, x, y)
                                For Each d As EFTUGDetails In cr
                                    Select Case SystemType.ToUpper.Trim
                                        Case "BR"
                                            strAction = "UPDATE t_TransactionMICR SET IsGenerated = 1, Reference = '" & msgId & "' WHERE ColumnID = '" & d.TrxId & "' AND TrxType ='0' AND VoucherCode <> '40'  AND BankID = '" & BankArr.Item(k) & "'"
                                        Case "BRMFO"
                                            strAction = "UPDATE t_TransactionMICR SET IsGenerated = 1, Reference = '" & msgId & "' WHERE ColumnID = '" & d.TrxId & "' AND TrxType ='0' AND VoucherCode <> '40'  AND BankID = '" & BankArr.Item(k) & "'"
                                        Case "BRNET"
                                            strAction = "UPDATE t_TrxClearing SET IsGenerated = 1 ,Reference = '" & msgId & "' WHERE TrxRowID = '" & d.TrxId & "'"
                                        Case "BRNETOLD"
                                            strAction = "UPDATE t_TrxClearing SET IsGenerated = 1, SessNo='1' ,Reference = '" & msgId & "' WHERE TrxRowID = '" & d.TrxId & "' AND TrxType ='OD' AND VoucherCode <> '40'  AND BankID = '" & BankArr.Item(k) & "'"
                                    End Select
                                    Modscan.ExecuteData(strAction, Nothing, Modscan.dataExecTypes.ExecTypeNonQuery, Modscan.queryType.SelectStatement)
                                Next
                            End If
                            strAction = "UPDATE t_Bank SET FileCounter = isNull(FileCounter,0) + 1 WHERE BankID = '" & BankArr.Item(k) & "'"
                            Modscan.ExecuteData(strAction, Nothing, Modscan.dataExecTypes.ExecTypeNonQuery, Modscan.queryType.SelectStatement)
                        Catch ex As Exception
                            Continue For
                        End Try
                    Next
                    publicDTblEFTCrCopy.Clear()
                Case FileType.EftReturn
                    For k As Int32 = 0 To BankArr.Count - 1
                        Try
                            Dim i As Integer = 0
                            Dim amt As Decimal = 0
                            Dim FileCounter As String = GetScalarREC("Select isNull(FileCounter,0)+1 From t_Bank Where BankID = '" & BankArr.Item(k) & "'")
                            Dim CreateFile As String = Session & Now.ToString(".yyyyMMdd.HHmm.") & BankArr.Item(k) & "." & Session
                            Dim BIC As String = ""
                            'EFT Cr Reject
                            Modscan.ExecuteData(Modscan.GetModify("Proc_CreateXMLFiles_UG_CTS", "OurBankID", Modscan.OurBankID, "ourbranchid", Modscan.OurBranchID, "ReadDate", Modscan.cWORKING_DATE, "FromDate", Modscan.cFromDate, "EJDate", Modscan.cToDate, "ClearingCenters", "47", "AllCenters", 0, "Currency", CurrCode, "FileType", "T", "Sessno", Session), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure, Modscan.publicDset)
                            Dim EFTCrfoundRows() As DataRow
                            'MessageBox.Show(SystemType.ToUpper.Trim)
                            Select Case SystemType.ToUpper.Trim
                                Case "BR"
                                    EFTCrfoundRows = Modscan.publicDset.Tables(1).Select("TrxType ='0' AND VoucherCode <> '40' AND ReturnCode <>'00' AND  BankID = '" & BankArr.Item(k) & "'  And IsGenerated=false")
                                Case "BRMFO"
                                    EFTCrfoundRows = Modscan.publicDset.Tables(1).Select("TrxType ='0' AND VoucherCode <> '40' AND ReturnCode <>'00' AND  BankID = '" & BankArr.Item(k) & "'  And IsGenerated=false")
                                Case "BRNET"
                                    EFTCrfoundRows = Modscan.publicDTbl.Select("TrxType ='OD' AND VoucherCode <> '40' AND ReturnCode <>'00' AND BankID = '" & BankArr.Item(k) & "'  And IsGenerated=false")
                                Case "BRNETOLD"
                                    EFTCrfoundRows = Modscan.publicDTbl.Select("TrxType ='OD' AND VoucherCode <> '40' AND ReturnCode <>'00' AND BankID = '" & BankArr.Item(k) & "'  And IsGenerated=false")
                            End Select
                            publicDTblEFTCrCopy = Modscan.publicDset.Tables(0).Clone()
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
                                'MessageBox.Show(ex.Message)
                                MessageBox.Show("Error Registered, Check ErrorLog")
                                Modscan.ErrorLog("Swift Code not maintained for this our BankID", "- EFT Unpaid Generation")
                                Continue For
                            End Try
                            Try
                                If publicDTblEFTCrCopy.Rows.Count > 0 Then
                                    DestBankBIC = ""
                                    DestBankBIC = publicDTblEFTCrCopy(0)("DestinationSwiftCode").ToString()
                                End If
                            Catch ex As Exception
                                MessageBox.Show("Error Registered, Check ErrorLog")
                                Modscan.ErrorLog("Swift Code not maintained for this BankID " & publicDTblEFTCrCopy(0)("BankID"), "- EFT Unpaid Generation")
                                Continue For
                            End Try
                            Dim cr As New List(Of EFTUGDetails)
                            For Each row As DataRow In publicDTblEFTCrCopy.Rows
                                Try
                                    Dim destBIC As String = row("DestinationSwiftCode").ToString()
                                    Dim SourceBIC As String = row("SwiftCode").ToString()
                                    Dim d As New EFTUGDetails
                                    d.Amount = FormatNumber(row("Amount"), 2)
                                    d.Currency = CurrCode
                                    d.ISOCurrency = row("CurrencyCode")
                                    d.DestBIC = destBIC
                                    d.DestBankID = row("BankID")
                                    d.MsgId = row("Reference")
                                    d.RetCode = row("ReturnCode")
                                    d.VCode = row("VoucherCode").ToString().Trim()
                                    d.ValueDate = Modscan.WORKING_DATE
                                    d.RetCode = row("ReturnCode").ToString()
                                    d.RetCodeDesc = row("RetCodeDesc").ToString().ToUpper
                                    d.ValueDate = row("ValueDate")
                                    d.TrxId = row("TransactionMicrColumnID").ToString()
                                    d.OrgnTrxID = row("TrxID").ToString()
                                    d.OrgnlMsgId = row("Reference").ToString()
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
                                    d.ReqdColltnDt = IIf(row("ReqdColltnDt").ToString() = "Jan  1 1900 12:00AM", "", row("ReqdColltnDt").ToString())
                                    cr.Add(d)
                                Catch ex As Exception
                                    MessageBox.Show("Error Registered, Check ErrorLog")
                                    Modscan.ErrorLog(ex.Message, "- EFT Unpaids Generation")
                                    Continue For
                                End Try
                            Next
                            If cr.Count > 0 Then
                                Dim msgId As String = CancelCredit(cr, cr(0).Currency, BIC, x, y)
                                For Each d As EFTUGDetails In cr
                                    Select Case SystemType.ToUpper.Trim
                                        Case "BR"
                                            strAction = "UPDATE t_TransactionMICR SET IsGenerated = 1, Remarks = '" & msgId & "' WHERE ColumnID = '" & d.TrxId & "' AND TrxType ='0' AND VoucherCode <> '40' AND ReturnCode <>'00' AND IsGenerated=1 AND BankID = '" & BankArr.Item(k) & "'"
                                        Case "BRMFO"
                                            strAction = "UPDATE t_TransactionMICR SET IsGenerated = 1, Remarks = '" & msgId & "' WHERE ColumnID = '" & d.TrxId & "' AND TrxType ='0' AND VoucherCode <> '40' AND ReturnCode <>'00' AND IsGenerated=1 AND BankID = '" & BankArr.Item(k) & "'"
                                        Case "BRNET"
                                            strAction = "UPDATE t_trxClearing SET IsGenerated = 1, Reference = '" & msgId & "' WHERE TrxRowID = '" & d.TrxId & "' "
                                        Case "BRNETOLD"
                                            strAction = "UPDATE t_trxClearing SET IsGenerated = 1, SessNo='1' ,'Reference = '" & msgId & "' WHERE TrxRowID = '" & d.TrxId & "' AND TrxType ='OD' AND VoucherCode <> '40' AND ReturnCodeID <>'00' AND IsGenerated=1 AND BankID = '" & BankArr.Item(k) & "'"
                                    End Select
                                    Modscan.ExecuteData(strAction, Nothing, Modscan.dataExecTypes.ExecTypeNonQuery, Modscan.queryType.SelectStatement)

                                Next
                            End If
                            strAction = "UPDATE t_Bank SET FileCounter = isNull(FileCounter,0) + 1 WHERE  BankID = '" & BankArr.Item(k) & "'"
                            Modscan.ExecuteData(strAction, Nothing, Modscan.dataExecTypes.ExecTypeNonQuery, Modscan.queryType.SelectStatement)

                        Catch ex As Exception
                            MessageBox.Show("Error Registered, Check ErrorLog")
                            Modscan.ErrorLog(ex.Message, "- EFT Unpaid Generation")
                            Continue For
                        End Try
                    Next
                    publicDTblEFTCrCopy.Clear()
                Case BRUGClass.FileType.DD
                    For k As Int32 = 0 To BankArr.Count - 1
                        Dim i As Integer = 0
                        Dim amt As Decimal = 0
                        Dim l As New List(Of TZ.ChequeDetails)
                        Dim FileCounter As String = GetScalarREC("Select isNull(FileCounter,0)+1 From t_Bank Where BankID = '" & BankArr.Item(k) & "'")
                        Dim CreateFile As String = Session & Now.ToString(".yyyyMMdd.HHmm.") & BankArr.Item(k) & "." & Session
                        Dim BIC As String = ""
                        'EFT Cr

                        Modscan.ExecuteData(Modscan.GetModify("Proc_CreateXMLFiles_UG_CTS", "OurBankID", Modscan.OurBankID, "ourbranchid", Modscan.OurBranchID, "ReadDate", Modscan.cWORKING_DATE, "FromDate", Modscan.cFromDate, "EJDate", Modscan.cToDate, "ClearingCenters", "47", "AllCenters", 0, "Currency", CurrCode, "FileType", "T", "Sessno", Session), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure, Modscan.publicDset)
                        Dim EFTCrfoundRows() As DataRow
                        Select Case SystemType.ToUpper.Trim
                            Case "BR"
                                EFTCrfoundRows = Modscan.publicDset.Tables(1).Select("TrxType ='0' AND VoucherCode = '40' AND BankID = '" & BankArr.Item(k).ToString.Trim() & "'  And IsGenerated=false")
                            Case "BRMFO"
                                EFTCrfoundRows = Modscan.publicDset.Tables(1).Select("TrxType ='0' AND VoucherCode = '40' AND BankID = '" & BankArr.Item(k).ToString.Trim() & "'  And IsGenerated=false")
                            Case "BRNET"
                                EFTCrfoundRows = Modscan.publicDTbl.Select("TrxType ='OD' AND VoucherCode = '40'  AND ReturnCode ='00' AND BankID = '" & BankArr.Item(k).ToString.Trim() & "'  And IsGenerated=false")
                            Case "BRNETOLD"
                                EFTCrfoundRows = Modscan.publicDTbl.Select("TrxType ='OD' AND VoucherCode = '40' AND BankID = '" & BankArr.Item(k).ToString.Trim() & "'  And IsGenerated=false")
                        End Select
                        publicDTblEFTCrCopy = Modscan.publicDset.Tables(0).Clone()
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
                            Modscan.ErrorLog("Swift Code not maintained for this our BankID", "- DD Generation")
                            Continue For
                        End Try
                        Try
                            If publicDTblEFTCrCopy.Rows.Count > 0 Then
                                DestBankBIC = ""
                                DestBankBIC = publicDTblEFTCrCopy(0)("DestinationSwiftCode").ToString()
                            End If
                        Catch ex As Exception
                            MessageBox.Show("Error Registered, Check ErrorLog")
                            Modscan.ErrorLog("Swift Code not maintained for this BankID " & publicDTblEJCopy(0)("BankID") & " DD generation for this Bank Abort", "- DD Generation")
                            Continue For
                        End Try
                        Dim cr As New List(Of DDUGDetail)
                        For Each row As DataRow In publicDTblEFTCrCopy.Rows
                            Try
                                Dim destBIC As String = row("DestinationSwiftCode").ToString()
                                Dim SourceBIC As String = row("SwiftCode").ToString()
                                Dim d As New DDUGDetail
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
                                Try
                                    d.FnlColltnDt = row("FnlColltnDt")
                                Catch ex As Exception
                                    d.FnlColltnDt = ""
                                End Try

                                d.UstrdColD = row("UstrdColD")
                                Try
                                    d.DNm = RegX.Replace(row("DNm"), " ")
                                    If d.DNm.Length > 55 Then d.DNm = d.DNm.Substring(0, 55)
                                Catch ex As Exception
                                    d.DNm = ""
                                End Try
                                d.VCode = row("VoucherCode").ToString()
                                d.TxId = row("ColumnID")
                                d.OrgnlEndToEnd = row("Policynumber1")
                                cr.Add(d)
                            Catch ex As Exception
                                MessageBox.Show("Error Registered, Check ErrorLog")
                                Modscan.ErrorLog(ex.Message, "- DD Generation")
                                Continue For
                            End Try

                        Next
                        If cr.Count > 0 Then
                            Dim msgId As String = BulkDebit(cr, cr(0).Curr, BIC, x, y)
                            For Each d As DDUGDetail In cr
                                Select Case SystemType.ToUpper.Trim
                                    Case "BR"
                                        strAction = "UPDATE t_TransactionMICR SET IsGenerated = 1, Reference = '" & msgId & "' WHERE ColumnID = '" & d.TxId & "' AND TrxType ='0' AND VoucherCode <> '40'  AND BankID = '" & BankArr.Item(k) & "'"
                                    Case "BRMFO"
                                        strAction = "UPDATE t_TransactionMICR SET IsGenerated = 1, Reference = '" & msgId & "' WHERE ColumnID = '" & d.TxId & "' AND TrxType ='0' AND VoucherCode <> '40'  AND BankID = '" & BankArr.Item(k) & "'"
                                    Case "BRNET"
                                        strAction = "UPDATE t_TrxClearing SET IsGenerated = 1 ,Reference = '" & msgId & "' WHERE TrxRowID = '" & d.TxId & "'"
                                    Case "BRNETOLD"
                                        strAction = "UPDATE t_TrxClearing SET IsGenerated = 1, SessNo='1' ,Reference = '" & msgId & "' WHERE TrxRowID = '" & d.TxId & "' AND TrxType ='OD' AND VoucherCode <> '40'  AND BankID = '" & BankArr.Item(k) & "'"
                                End Select
                                Modscan.ExecuteData(strAction, Nothing, Modscan.dataExecTypes.ExecTypeNonQuery, Modscan.queryType.SelectStatement)
                            Next
                        End If
                        strAction = "UPDATE t_Bank SET FileCounter = isNull(FileCounter,0) + 1 WHERE  BankID = '" & BankArr.Item(k) & "'"
                        Modscan.ExecuteData(strAction, Nothing, Modscan.dataExecTypes.ExecTypeNonQuery, Modscan.queryType.SelectStatement)

                    Next
                    publicDTblEFTCrCopy.Clear()
                Case FileType.DDReturn
                    For k As Int32 = 0 To BankArr.Count - 1
                        Try
                            Dim i As Integer = 0
                            Dim amt As Decimal = 0
                            Dim FileCounter As String = GetScalarREC("Select isNull(FileCounter,0)+1 From t_Bank Where BankID = '" & BankArr.Item(k) & "'")
                            Dim CreateFile As String = Session & Now.ToString(".yyyyMMdd.HHmm.") & BankArr.Item(k) & "." & Session
                            Dim BIC As String = ""
                            'EFT Cr Reject
                            Modscan.ExecuteData(Modscan.GetModify("Proc_CreateXMLFiles_UG_CTS", "OurBankID", Modscan.OurBankID, "ourbranchid", Modscan.OurBranchID, "ReadDate", Modscan.cWORKING_DATE, "FromDate", Modscan.cFromDate, "EJDate", Modscan.cToDate, "ClearingCenters", "47", "AllCenters", 0, "Currency", CurrCode, "FileType", "T", "Sessno", Session), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure, Modscan.publicDset)
                            Dim EFTCrfoundRows() As DataRow
                            'MessageBox.Show(SystemType.ToUpper.Trim)
                            Select Case SystemType.ToUpper.Trim
                                Case "BR"
                                    EFTCrfoundRows = Modscan.publicDset.Tables(1).Select("TrxType ='0' AND VoucherCode = '40' AND ReturnCode <>'00' AND  BankID = '" & BankArr.Item(k) & "'  And IsGenerated=false")
                                Case "BRMFO"
                                    EFTCrfoundRows = Modscan.publicDset.Tables(1).Select("TrxType ='0' AND VoucherCode = '40' AND ReturnCode <>'00' AND  BankID = '" & BankArr.Item(k) & "'  And IsGenerated=false")
                                Case "BRNET"
                                    EFTCrfoundRows = Modscan.publicDTbl.Select("TrxType ='OD' AND VoucherCode = '40' AND ReturnCode <>'00' AND BankID = '" & BankArr.Item(k) & "'  And IsGenerated=false")
                                Case "BRNETOLD"
                                    EFTCrfoundRows = Modscan.publicDTbl.Select("TrxType ='OD' AND VoucherCode = '40' AND ReturnCode <>'00' AND BankID = '" & BankArr.Item(k) & "'  And IsGenerated=false")
                            End Select
                            publicDTblEFTCrCopy = Modscan.publicDset.Tables(0).Clone()
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
                                'MessageBox.Show(ex.Message)
                                MessageBox.Show("Error Registered, Check ErrorLog")
                                Modscan.ErrorLog("Swift Code not maintained for this our BankID", "- EFT Unpaid Generation")
                                Continue For
                            End Try
                            Try
                                If publicDTblEFTCrCopy.Rows.Count > 0 Then
                                    DestBankBIC = ""
                                    DestBankBIC = publicDTblEFTCrCopy(0)("DestinationSwiftCode").ToString()
                                End If
                            Catch ex As Exception
                                MessageBox.Show("Error Registered, Check ErrorLog")
                                Modscan.ErrorLog("Swift Code not maintained for this BankID " & publicDTblEFTCrCopy(0)("BankID"), "- EFT Unpaid Generation")
                                Continue For
                            End Try
                            Dim cr As New List(Of DDUGDetail)
                            For Each row As DataRow In publicDTblEFTCrCopy.Rows
                                Try
                                    Dim destBIC As String = row("DestinationSwiftCode").ToString()
                                    Dim SourceBIC As String = row("SwiftCode").ToString()
                                    Dim d As New DDUGDetail
                                    d.Amount = FormatNumber(row("Amount"), 2)
                                    d.Curr = CurrCode
                                    d.dBIC = destBIC
                                    d.DestBankID = row("BankID")
                                    d.MsgId = row("Reference")
                                    d.Retcode = row("ReturnCode")
                                    d.VCode = row("VoucherCode").ToString()
                                    d.Retcode = row("ReturnCode").ToString()
                                    d.RetCodeDesc = row("RetCodeDesc").ToString().ToUpper
                                    d.TxId = row("TransactionMicrColumnID").ToString()
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
                                    d.MndtId = row("MndtId").ToString()
                                    d.FnlColltnDt = row("FnlColltnDt").ToString()
                                    d.DtOfSgntr = row("DtOfSgntr").ToString()
                                    d.Frqcy = row("Frqcy").ToString()
                                    d.DTwnNm = row("DTwnNm").ToString()
                                    d.DCNm = row("DCNm").ToString()
                                    d.CCNm = row("CCNm").ToString()
                                    d.DbtrAcct = row("DbtrAcct").ToString()
                                    d.CdtrAcct = row("CdtrAcct").ToString()
                                    d.UstrdColD = row("UstrdColD").ToString()
                                    d.OrgnlEndToEnd = row("OrgnlEndToEnd").ToString()
                                    d.ReqdColltnDt = IIf(row("ReqdColltnDt").ToString() = "Jan  1 1900 12:00AM", "", row("ReqdColltnDt").ToString())
                                    cr.Add(d)
                                Catch ex As Exception
                                    MessageBox.Show("Error Registered, Check ErrorLog")
                                    Modscan.ErrorLog(ex.Message, "- EFT Unpaids Generation")
                                    Continue For
                                End Try
                            Next
                            If cr.Count > 0 Then
                                Dim msgId As String = DDReject(cr, cr(0).Curr, BIC, x, y)
                                For Each d As DDUGDetail In cr
                                    Select Case SystemType.ToUpper.Trim
                                        Case "BR"
                                            strAction = "UPDATE t_TransactionMICR SET IsGenerated = 1, Remarks = '" & msgId & "' WHERE ColumnID = '" & d.TxId & "' AND TrxType ='0' AND VoucherCode <> '40' AND ReturnCode <>'00' AND IsGenerated=1 AND BankID = '" & BankArr.Item(k) & "'"
                                        Case "BRMFO"
                                            strAction = "UPDATE t_TransactionMICR SET IsGenerated = 1, Remarks = '" & msgId & "' WHERE ColumnID = '" & d.TxId & "' AND TrxType ='0' AND VoucherCode <> '40' AND ReturnCode <>'00' AND IsGenerated=1 AND BankID = '" & BankArr.Item(k) & "'"
                                        Case "BRNET"
                                            strAction = "UPDATE t_trxClearing SET IsGenerated = 1, Reference = '" & msgId & "' WHERE TrxRowID = '" & d.TxId & "' "
                                        Case "BRNETOLD"
                                            strAction = "UPDATE t_trxClearing SET IsGenerated = 1, SessNo='1' ,'Reference = '" & msgId & "' WHERE TrxRowID = '" & d.TxId & "' AND TrxType ='OD' AND VoucherCode <> '40' AND ReturnCodeID <>'00' AND IsGenerated=1 AND BankID = '" & BankArr.Item(k) & "'"
                                    End Select
                                    Modscan.ExecuteData(strAction, Nothing, Modscan.dataExecTypes.ExecTypeNonQuery, Modscan.queryType.SelectStatement)
                                Next
                            End If
                            strAction = "UPDATE t_Bank SET FileCounter = isNull(FileCounter,0) + 1 WHERE  BankID = '" & BankArr.Item(k) & "'"
                            Modscan.ExecuteData(strAction, Nothing, Modscan.dataExecTypes.ExecTypeNonQuery, Modscan.queryType.SelectStatement)

                        Catch ex As Exception
                            MessageBox.Show("Error Registered, Check ErrorLog")
                            Modscan.ErrorLog(ex.Message, "- EFT Unpaid Generation")
                            Continue For
                        End Try
                    Next
                    publicDTblEFTCrCopy.Clear()
                Case FileType.Messages
            End Select
            GenerateUGFiles = True
            publicDTblBankCopy.Clear()
        Catch ex As Exception
            GenerateUGFiles = False
        End Try
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
    Private Shared Function BulkCredit(ByVal l As List(Of EFTUGDetails), ByVal ccy As String, ByVal BIC As String, ByVal x As String, ByVal y As String) As String
        Dim dAmt As Decimal = 0
        For Each itm As EFTUGDetails In l
            dAmt += CDec(itm.Amount)
        Next
        Dim amt As String = FormatNumber(dAmt, 2, Microsoft.VisualBasic.TriState.True, Microsoft.VisualBasic.TriState.True, Microsoft.VisualBasic.TriState.False)
        Dim sDt As String = Now.ToString("dd-MMM-yyyy")
        Dim STm As String = Now.ToString("HH:mm")
        Dim TimeSec As String = Now.ToString("HHmmss")
        Dim xDt As Date = CDate(sDt & " " & STm)
        Dim stCurrCode As String = ""
        If ccy = 0 Then
            stCurrCode = "UGX"
        ElseIf ccy = 1 Then
            stCurrCode = "USD"
        ElseIf ccy = 2 Then
            stCurrCode = "GBP"
        ElseIf ccy = 3 Then
            stCurrCode = "EUR"
        ElseIf ccy = 4 Then
            stCurrCode = "JPY"
        ElseIf ccy = 5 Then
            stCurrCode = "KES"
        Else
            stCurrCode = "TZS"
        End If
        Dim FileCounter As String = GetScalarREC("Select isNull(FileCounter,0)+1 From t_Bank Where BankID = '" & l(0).DestBankID.PadLeft(2, "0") & "'")
        Dim Filename As String = l(0).DestBankID & Now.ToString("ddMMyyyy") & Modscan.Sess & stCurrCode & FileCounter.PadLeft(2, "0") & ".T" & l(0).SourceBankID
        Dim msgId As String = "CT" & l(0).DestBankID & Modscan.WORKING_DATE.ToString("ddMMyyyy") & TimeSec & Modscan.Sess & stCurrCode & GetNextString()
        Dim doc As New ct816.Document()
        Dim grpHdr As New ct816.GroupHeader70()
        grpHdr.MsgId = "OD/" & Modscan.WORKING_DATE.ToString("ddMMyy") & "/" & Modscan.Sess & TimeSec
        grpHdr.CreDtTm = xDt
        grpHdr.NbOfTxs = l.Count
        grpHdr.TtlIntrBkSttlmAmt = New ct816.ActiveCurrencyAndAmount() With {.Ccy = stCurrCode, .Value = Decimal.Round(CDec(dAmt), 2)}
        grpHdr.InterTestDt = sDt
        grpHdr.PmtTpInf = New ct816.PaymentTypeInformation21() With {.LclInstrm = New ct816.LocalInstrument2Choice() With {.ItemElementName = ct816.ItemChoiceType4.Cd, .Item = "08"}}
        grpHdr.SttlmInf = New ct816.SettlementInstruction4() _
         With {.SttlmMtd = ct816.SettlementMethod1Code.CLRG, .ClrSys = New ct816.ClearingSystemIdentification3Choice() _
              With {.Item = "47"}}
        grpHdr.InstgAgt = New ct816.BranchAndFinancialInstitutionIdentification5 With {.FinInstnId = New ct816.FinancialInstitutionIdentification8 With {.BICFI = BIC.ToUpper, .ClrSysMmbId = New ct816.ClearingSystemMemberIdentification2() With {.MmbId = l(0).SourceBankID}}}
        grpHdr.InstdAgt = New ct816.BranchAndFinancialInstitutionIdentification5 With {.FinInstnId = New ct816.FinancialInstitutionIdentification8 With {.BICFI = l(0).DestBIC.ToUpper, .ClrSysMmbId = New ct816.ClearingSystemMemberIdentification2() With {.MmbId = l(0).DestBankID}}}
        doc.FIToFICstmrCdtTrf.GrpHdr = grpHdr
        For Each itm As EFTUGDetails In l
            amt = FormatNumber(itm.Amount, 2, Microsoft.VisualBasic.TriState.True, Microsoft.VisualBasic.TriState.True, Microsoft.VisualBasic.TriState.False)
            Dim cdtTxn As New ct816.CreditTransferTransaction25()
            cdtTxn.PmtId.EndToEndId = "EFT/" & itm.TrxId
            cdtTxn.PmtId.InstrId = itm.VCode
            cdtTxn.PmtId.TxId = itm.TrxId
            cdtTxn.PmtTpInf = New ct816.PaymentTypeInformation21() _
               With {.CtgyPurp = New ct816.CategoryPurpose1Choice() With {.Item = itm.VCode}}
            cdtTxn.IntrBkSttlmAmt = New ct816.ActiveCurrencyAndAmount() With {.Ccy = stCurrCode, .Value = Decimal.Round(CDec(amt), 2)}
            'cdtTxn.IntrBkSttlmDt = xDt
            cdtTxn.ChrgBr = ct816.ChargeBearerType1Code.SHAR
            cdtTxn.Dbtr = New ct816.PartyIdentification43() _
            With {.Nm = itm.DNm.ToUpper, .PstlAdr = New ct816.PostalAddress6 With {.TwnNm = .TwnNm, .Ctry = .Ctry, .AdrLine = {""}}}
            cdtTxn.DbtrAcct = New ct816.CashAccount24() With {.Id = New ct816.AccountIdentification4Choice() With {.Item = New ct816.GenericAccountIdentification1() With {.Id = itm.DbtrAcct}}}
            cdtTxn.DbtrAgt = New ct816.BranchAndFinancialInstitutionIdentification5 With {.FinInstnId = New ct816.FinancialInstitutionIdentification8() With {.BICFI = itm.OurBankBic.ToUpper, .ClrSysMmbId = New ct816.ClearingSystemMemberIdentification2() With {.MmbId = itm.SourceBankID}}}
            cdtTxn.Cdtr = New ct816.PartyIdentification43() With {.Nm = itm.CNm.ToUpper}
            cdtTxn.CdtrAcct = New ct816.CashAccount24() With {.Id = New ct816.AccountIdentification4Choice() With {.Item = New ct816.GenericAccountIdentification1() With {.Id = itm.CdtrAcct}}}
            cdtTxn.CdtrAgt = New ct816.BranchAndFinancialInstitutionIdentification5 With {.FinInstnId = New ct816.FinancialInstitutionIdentification8() With {.BICFI = itm.DestBIC.ToUpper, .ClrSysMmbId = New ct816.ClearingSystemMemberIdentification2() With {.MmbId = itm.DestBankID}}}
            cdtTxn.RmtInf = New ct816.RemittanceInformation11() With {.Ustrd = {itm.UstrdColD.ToUpper}}
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
                'xDoc.Descendants().ToList()(4).SetValue(CDate(xCreTm.Value).ToString("dd-MM-yyyy"))
                xDoc.Descendants().ToList()(4).SetValue(CDate(xCreTm.Value).ToString("yyyy-MM-ddTHH:mm:sszzz"))
                xDoc.Descendants().ToList()(7).SetValue(CDate(xStmDt.Value).ToString("yyyy-MM-dd"))
                xDoc.Descendants().Where(Function(p) p.IsEmpty Or String.IsNullOrEmpty(p.Value)).Remove()
                xDoc.Root.Attributes().Reverse()
                xDoc.Save(fullpath, SaveOptions.None)

                Dim Filter As String() = {"*.T*"}
                Dim di As New DirectoryInfo(TempLocation)
                Dim kl As New List(Of String)
                For Each f As String In Filter
                    Dim fi As FileInfo() = di.GetFiles(f)
                    For Each inf As FileInfo In fi
                        kl.Add(inf.FullName)
                    Next
                Next

                'We need to sign authomatically these ones
                Dim MessOut As String = ""
                Dim destFileName As String = Path.Combine(StrDestinationFilePath, Filename)
                File.Move(fullpath, destFileName)
                MessOut = SignFiles_PKCS(fullpath.Trim(), StrDestinationFilePath.Trim(), x.Trim(), y.Trim())
                Dim ArchivePath As String
                If MessOut = "success" Then
                    ArchivePath = ConfigurationManager.AppSettings("Archive")
                    Dim FileDir As String = destFileName.ToString().Substring(0, destFileName.ToString().LastIndexOf("\"))
                    Clear_Files_Arc(FileDir.Trim(), ArchivePath.Trim(), FileDir.Trim(), "Out")
                Else
                    MessageBox.Show("Failed Signing. " & Path.GetFileName(destFileName) & " : " & MessOut)
                    Modscan.ErrorLog(Path.GetFileName(destFileName) & " : " & MessOut, "- Signing ")
                End If


                For Each itm As String In kl
                    File.Delete(itm)
                Next
                Return msgId
            End If
        End If
        Return Nothing
    End Function
    Private Shared Function SignFiles_PKCS(ByVal Sourcepath As String, ByVal DestPath As String, ByVal cert As String, ByVal tokenpass As String) As String
        Dim Mes As String = ""
        Sourcepath = ConfigurationManager.AppSettings("OutgoingFiles") & "\Files"
        DestPath = ConfigurationManager.AppSettings("OutgoingSignedFiles") & "\SignedFiles"
        Dim p As BRCS = New BRCS()
        p.BRCDS(BRRSACryptography.CryptographyHelper.Encrypt(Sourcepath), BRRSACryptography.CryptographyHelper.Encrypt(DestPath), BRRSACryptography.CryptographyHelper.Encrypt("h / KNJ1uE5CmUcQb4xbsfoW9ZPzk ="), 71, cert, tokenpass, Mes, "UG")
        Mes = "success"
        Return Mes
    End Function
    Private Shared Function CancelCredit(ByVal l As List(Of EFTUGDetails), ByVal ccy As String, ByVal BIC As String, ByVal x As String, ByVal y As String) As String
        Dim RegX As New Regex("[^A-Za-z0-9]")
        Dim dAmt As Decimal = 0
        For Each itm As EFTUGDetails In l
            dAmt += CDec(itm.Amount)
        Next
        Dim amt As String = FormatNumber(dAmt, 2, Microsoft.VisualBasic.TriState.True, Microsoft.VisualBasic.TriState.True, Microsoft.VisualBasic.TriState.False)
        Dim sDt As String = Modscan.FDATE.ToString("dd-MMM-yyyy")
        Dim STm As String = Now.ToString("HH:mm")
        Dim TimeSec As String = Now.ToString("HHmmss")
        Dim xDt As Date = CDate(sDt & " " & STm)
        Dim stCurrCode As String = ""
        If ccy = 0 Then
            stCurrCode = "UGX"
        ElseIf ccy = 1 Then
            stCurrCode = "USD"
        ElseIf ccy = 2 Then
            stCurrCode = "GBP"
        ElseIf ccy = 3 Then
            stCurrCode = "EUR"
        ElseIf ccy = 4 Then
            stCurrCode = "JPY"
        ElseIf ccy = 5 Then
            stCurrCode = "KES"
        Else
            stCurrCode = "TZS"
        End If
        Dim FileCounter As String = GetScalarREC("Select isNull(FileCounter,0)+1 From t_Bank Where BankID = '" & l(0).DestBankID.PadLeft(2, "0") & "'")
        Dim Filename As String = l(0).DestBankID & Modscan.FDATE.ToString("ddMMyyyy") & Modscan.Sess & stCurrCode & FileCounter.ToString().PadLeft(2, "0") & ".Y" & Modscan.OurBankID
        Dim msgId As String = "UNPOD/" & Modscan.FDATE.ToString("ddMMyyyy") & "/" & Modscan.Sess & TimeSec
        Dim doc As New dd416.Document()
        Dim grpHdr As New dd416.GroupHeader72()
        grpHdr.MsgId = msgId
        grpHdr.CreDtTm = xDt
        grpHdr.NbOfTxs = l.Count
        grpHdr.TtlRtrdIntrBkSttlmAmt = New dd416.ActiveCurrencyAndAmount() With {.Ccy = stCurrCode, .Value = Decimal.Round(CDec(amt), 2)}
        grpHdr.IntrBkStlmDt = Modscan.FDATE.ToString("dd-MMM-yyyy")
        grpHdr.SttlmInf = New dd416.SettlementInstruction4() _
        With {.SttlmMtd = dd416.SettlementMethod1Code.CLRG, .ClrSys = New dd416.ClearingSystemIdentification3Choice() _
             With {.ItemElementName = dd416.ItemChoiceType2.Cd, .Item = "47"}}
        grpHdr.InstgAgt = New dd416.BranchAndFinancialInstitutionIdentification5 With {.FinInstnId = New dd416.FinancialInstitutionIdentification8 With {.BICFI = BIC.ToUpper, .ClrSysMmbId = New dd416.ClearingSystemMemberIdentification2() With {.MmbId = Modscan.OurBankID}}}
        grpHdr.InstdAgt = New dd416.BranchAndFinancialInstitutionIdentification5 With {.FinInstnId = New dd416.FinancialInstitutionIdentification8 With {.BICFI = l(0).DestBIC.ToUpper, .ClrSysMmbId = New dd416.ClearingSystemMemberIdentification2() With {.MmbId = l(0).DestBankID}}}
        doc.PmtRtr.GrpHdr = grpHdr
        For Each d As EFTUGDetails In l
            Try
                Dim txnInf As New dd416.PaymentTransaction65()
                txnInf.RtrId = d.TrxId
                txnInf.OrgnlGrpInf.OrgnlMsgId = d.OrgnlMsgId
                txnInf.OrgnlGrpInf.OrgnlMsgNmId = "pacs.008.001.06"
                txnInf.OrgnlInstrId = d.OrgnlInstrID
                txnInf.OrgnlEndToEndId = d.OrgnlEndToEnd
                txnInf.OrgnlTxId = d.OrgnTrxID
                txnInf.OrgnlIntrBkSttlmAmt = New dd416.ActiveOrHistoricCurrencyAndAmount() With {.Ccy = stCurrCode, .Value = Decimal.Round(CDec(d.Amount), 2)}
                txnInf.RtrdIntrBkSttlmAmt = New dd416.ActiveCurrencyAndAmount() With {.Ccy = stCurrCode, .Value = Decimal.Round(CDec(d.Amount), 2)}
                txnInf.ChrgBrM = dd416.ChargeBearerType1Code.SHAR
                txnInf.RtrRsnInf = {New dd416.PaymentReturnReason1() With {.Rsn = New dd416.ReturnReason5Choice() With {.ItemElementName = dd416.ItemChoiceType7.Cd, .Item = d.RetCode}, .AddtlInf = {d.RetCodeDesc}}}
                txnInf.OrgnlTxRef.IntrBkSttlmAmt = New dd416.ActiveOrHistoricCurrencyAndAmount() With {.Ccy = stCurrCode, .Value = Decimal.Round(CDec(d.Amount), 2)}
                If d.ReqdColltnDt = "" Then
                    'txnInf.OrgnlTxRef.ReqdColltnDt = Nothing
                Else
                    Dim ReqdColltnDate As Date = Convert.ToDateTime(d.ReqdColltnDt)
                    txnInf.OrgnlTxRef.ReqdColnDt = ReqdColltnDate.ToString("yyyy-MM-dd")
                End If
                txnInf.OrgnlTxRef.PmtTpInf = New dd416.PaymentTypeInformation25() With {.CtgyPurpl = New dd416.CategoryPurpose1Choice() With {.ItemElementName = dd416.ItemChoiceType4.Cd, .Item = d.VCode}}
                txnInf.OrgnlTxRef.RmtInf = New dd416.RemittanceInformation11() With {.Ustrd = {d.UstrdColD.ToString}}
                txnInf.OrgnlTxRef.Dbtr = New dd416.PartyIdentification43() _
                                            With {.Nm = d.DNm, .PstlAdr = New dd416.PostalAddress6() With
                                                                          {.AdrLine = {d.DAdrLine}, .TwnNm = d.DTwnNm, .Ctry = d.DCtry}, .CtctDtls = New dd416.ContactDetails2() With {.Nm = d.DCNm, .PhneNb = d.DPhneNb, .MobNb = d.DMobNb, .EmailAdr = d.DEmailAdr, .Othr = d.DOthr}}
                txnInf.OrgnlTxRef.DbtrAcct = New dd416.CashAccount24() With {.Id = New dd416.AccountIdentification4Choice() With {.Item = New dd416.GenericAccountIdentification1 With {.Id = d.DbtrAcct}}}
                txnInf.OrgnlTxRef.DbtrAgt = New dd416.BranchAndFinancialInstitutionIdentification5 With {.FinInstnId = New dd416.FinancialInstitutionIdentification8 With {.BICFI = d.DestBIC.ToUpper, .ClrSysMmbId = New dd416.ClearingSystemMemberIdentification2() With {.MmbId = d.DestBankID}}}
                txnInf.OrgnlTxRef.Cdtr = New dd416.PartyIdentification43() _
                    With {.Nm = d.CNm, .PstlAdr = New dd416.PostalAddress6() With
                                                                          {.AdrLine = {d.CAdrLine}, .TwnNm = d.CTwnNm, .Ctry = d.CCtry}, .CtctDtls = New dd416.ContactDetails2() With {.Nm = d.CCNm, .PhneNb = d.CPhneNb, .MobNb = d.CMobNb, .EmailAdr = d.CEmailAdr, .Othr = d.COthr}}
                txnInf.OrgnlTxRef.CdtrAcct = New dd416.CashAccount24() With {.Id = New dd416.AccountIdentification4Choice() With {.Item = New dd416.GenericAccountIdentification1 With {.Id = d.CdtrAcct}}}
                txnInf.OrgnlTxRef.CdtrAgt = New dd416.BranchAndFinancialInstitutionIdentification5 With {.FinInstnId = New dd416.FinancialInstitutionIdentification8 With {.BICFI = BIC.ToUpper, .ClrSysMmbId = New dd416.ClearingSystemMemberIdentification2() With {.MmbId = Modscan.OurBankID}}}


                'txnInf.OrgnlTxRef.Dbtr = New dd416.PartyIdentification43() _
                '                            With {.Nm = d.BeneficiaryName.ToUpper, .CtctDtls = New dd416.ContactDetails2() With {.Nm = d.BeneficiaryName.ToUpper}}
                'txnInf.OrgnlTxRef.DbtrAgt = New dd416.BranchAndFinancialInstitutionIdentification5 With {.FinInstnId = New dd416.FinancialInstitutionIdentification8 With {.BICFI = d.OurBankBic.ToUpper, .ClrSysMmbId = New dd416.ClearingSystemMemberIdentification2() With {.MmbId = Modscan.OurBankID}}}
                'txnInf.OrgnlTxRef.Cdtr = New dd416.PartyIdentification43() _
                '    With {.Nm = d.RemitterName.ToUpper, .CtctDtls = New dd416.ContactDetails2() With {.Nm = d.RemitterName.ToUpper}}
                'txnInf.OrgnlTxRef.CdtrAcct = New dd416.CashAccount24() With {.Id = New dd416.AccountIdentification4Choice() With {.Item = New dd416.GenericAccountIdentification1 With {.Id = d.RemitterAcc}}}
                'txnInf.OrgnlTxRef.CdtrAgt = New dd416.BranchAndFinancialInstitutionIdentification5 With {.FinInstnId = New dd416.FinancialInstitutionIdentification8 With {.BICFI = d.BankBIC.ToUpper, .ClrSysMmbId = New dd416.ClearingSystemMemberIdentification2() With {.MmbId = d.BankCode}}}


                doc.PmtRtr.TxInf.Add(txnInf)
            Catch ex As Exception
                MessageBox.Show("Error Registered, Check ErrorLog")
                Modscan.ErrorLog(ex.Message, "Unpaid Cheques Generation")
                Continue For
            End Try
        Next

        Try
            If Directory.Exists(TempLocation) Then
                Dim fullpath As String = Path.Combine(TempLocation, Filename)
                Dim ex As New Exception()
                If doc.SaveToFile(fullpath, ex) Then
                    Dim xDoc As XDocument = XDocument.Load(fullpath)
                    Dim k As List(Of XAttribute) = xDoc.Root.Attributes().ToList()
                    Dim xsd As XAttribute = k(1)
                    If xDoc.Root.HasAttributes Then xDoc.Root.Attribute(xsd.Name).Remove()
                    Dim xCreTm As XElement = xDoc.Descendants().ToList()(4)
                    xDoc.Descendants().ToList()(4).SetValue(CDate(xCreTm.Value).ToString("yyyy-MM-ddTHH:mm:sszzz"))
                    xDoc.Descendants().Where(Function(p) p.IsEmpty Or String.IsNullOrEmpty(p.Value)).Remove()
                    xDoc.Descendants().Where(Function(p) p.IsEmpty Or Date.Equals("0001-01-01", p.Value)).Remove()
                    xDoc.Descendants().Where(Function(p) p.Name.LocalName = "MndtRltdInf").Remove()
                    xDoc.Root.Attributes().Reverse()
                    xDoc.Save(fullpath, SaveOptions.None)
                    'If Sign Then
                    '    Dim p As BRCS = New BRCS()
                    '    Dim Mes As String = ""
                    '    Dim TempFilesFolder As String = "C:\Images\Temp"
                    '    Dim TempPathDest As String = ""
                    '    If Directory.Exists(TempFilesFolder) Then
                    '        TempPathDest = TempFilesFolder & "\" & Filename
                    '        File.Move(fullpath, TempPathDest)
                    '    End If
                    '    p.BRCDS(TempPathDest, fullpath, "h / KNJ1uE5CmUcQb4xbsfoW9ZPzk =", 71, Mes)
                    '    If File.Exists(TempPathDest) Then
                    '        File.Delete(TempPathDest)
                    '    End If
                    'End If


                    Dim Filter As String() = {"*.Y*"}
                    Dim di As New DirectoryInfo(TempLocation)
                    Dim kl As New List(Of String)
                    For Each f As String In Filter
                        Dim fi As FileInfo() = di.GetFiles(f)
                        For Each inf As FileInfo In fi
                            kl.Add(inf.FullName)
                        Next
                    Next
                    Dim MessOut As String = ""
                    Dim destFileName As String = Path.Combine(StrDestinationFilePath, Filename)
                    File.Move(fullpath, destFileName)

                    MessOut = SignFiles_PKCS(fullpath.Trim(), StrDestinationFilePath.Trim(), x.Trim(), y.Trim())
                    Dim ArchivePath As String
                    If MessOut = "success" Then
                        ArchivePath = ConfigurationManager.AppSettings("Archive")
                        Dim FileDir As String = destFileName.ToString().Substring(0, destFileName.ToString().LastIndexOf("\"))
                        Clear_Files_Arc(FileDir.Trim(), ArchivePath.Trim(), FileDir.Trim(), "Out")
                    Else
                        MessageBox.Show("Failed Signing. " & Path.GetFileName(destFileName) & " : " & MessOut)
                        Modscan.ErrorLog(Path.GetFileName(destFileName) & " : " & MessOut, "- Signing ")
                    End If
                    'If File.Exists(destFileName) Then File.Delete(destFileName)
                    'File.Move(fullpath, destFileName)
                    For Each itm As String In kl
                        File.Delete(itm)
                    Next
                    Return msgId
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Error Registered, Check ErrorLog")
            Modscan.ErrorLog(ex.Message, "Unpaid Cheques Generation - ")
        End Try
        Return Nothing
    End Function

    Private Shared Function BulkDebit(ByVal l As List(Of DDUGDetail), ByVal ccy As String, ByVal BIC As String, ByVal x As String, ByVal y As String) As String
        Try


            Dim dAmt As Decimal = 0
            Dim ValDays As Integer = 1
            For Each itm As DDUGDetail In l
                dAmt += CDec(itm.Amount)
            Next
            Dim amt As String = FormatNumber(dAmt, 2, Microsoft.VisualBasic.TriState.True, Microsoft.VisualBasic.TriState.True, Microsoft.VisualBasic.TriState.False)
            Dim sDt As DateTime = Modscan.FDATE.ToString("dd-MM-yyyy")
            Dim STm As String = Now.ToString("HH:mm")
            Dim TimeSec As String = Now.ToString("HHmmss")
            Dim xDt As Date = CDate(sDt & " " & STm)
            Dim dtClearDate As Date = IsClearingDate(xDt.AddDays(ValDays))
            Dim stCurrCode As String = ""
            If ccy = 0 Then
                stCurrCode = "UGX"
            ElseIf ccy = 1 Then
                stCurrCode = "USD"
            ElseIf ccy = 2 Then
                stCurrCode = "GBP"
            ElseIf ccy = 3 Then
                stCurrCode = "EUR"
            ElseIf ccy = 4 Then
                stCurrCode = "JPY"
            ElseIf ccy = 5 Then
                stCurrCode = "KES"
            Else
                stCurrCode = "TZS"
            End If
            Dim FileCounter As String = GetScalarREC("Select isNull(FileCounter,0)+1 From t_Bank Where BankID = '" & l(0).DestBankID.PadLeft(2, "0") & "'")
            Dim msgId As String = "DD" & l(0).DestBankID & Modscan.FDATE.ToString("ddMMyyyy") & TimeSec & Modscan.Sess & stCurrCode & FileCounter & GetNextString()
            Dim Filename As String = l(0).DestBankID & Modscan.FDATE.ToString("ddMMyyyy") & Modscan.Sess & stCurrCode & FileCounter.PadLeft(2, "0") & ".D" & l(0).SourceBankID
            Dim doc As New dd316.Document()
            Dim grpHdr As New dd316.GroupHeader50()
            Dim fCreate As String = l(0).DestBankID & Modscan.FDATE.ToString("ddMMyyyy") & Modscan.Sess



            grpHdr.MsgId = "DD/" & Modscan.FDATE.ToString("ddMMyy") & "/" & Modscan.Sess & TimeSec
            grpHdr.CreDtTm = xDt
            grpHdr.NbOfTxs = l.Count
            grpHdr.TtlIntrBkSttlmAmt = New dd316.ActiveCurrencyAndAmount() With {.Ccy = stCurrCode, .Value = CInt(amt)}
            grpHdr.test = sDt
            grpHdr.SttlmInf = New dd316.SettlementInstruction2() _
            With {.SttlmMtd = dd316.SettlementMethod2Code.CLRG, .ClrSys = New dd316.ClearingSystemIdentification3Choice() _
                 With {.ItemElementName = dd316.ItemChoiceType2.Cd, .Item = "47"}}
            grpHdr.PmtTpInf = New dd316.PaymentTypeInformation25() With {.LclInstrm = New dd316.LocalInstrument2Choice() With {.ItemElementName = dd316.ItemChoiceType4.Cd, .Item = "07"}}
            grpHdr.InstgAgt = New dd316.BranchAndFinancialInstitutionIdentification5 With {.FinInstnId = New dd316.FinancialInstitutionIdentification8 With {.BICFI = BIC.ToUpper, .ClrSysMmbId = New dd316.ClearingSystemMemberIdentification2() With {.MmbId = Modscan.OurBankID}}}
            grpHdr.InstdAgt = New dd316.BranchAndFinancialInstitutionIdentification5 With {.FinInstnId = New dd316.FinancialInstitutionIdentification8 With {.BICFI = l(0).dBIC.ToUpper, .ClrSysMmbId = New dd316.ClearingSystemMemberIdentification2() With {.MmbId = l(0).DestBankID}}}
            doc.FIToFICstmrDrctDbt.GrpHdr = grpHdr
            For Each itm As DDUGDetail In l
                amt = FormatNumber(itm.Amount, 2, Microsoft.VisualBasic.TriState.True, Microsoft.VisualBasic.TriState.True, Microsoft.VisualBasic.TriState.False)
                Dim dbtTxn As New dd316.DirectDebitTransactionInformation20
                dbtTxn.PmtId = New dd316.PaymentIdentification3() _
                With {.InstrId = itm.TxId, .EndToEndId = itm.OrgnlEndToEnd, .TxId = itm.TxId}
                dbtTxn.PmtTpInf = New dd316.PaymentTypeInformation25() _
                    With {.CtgyPurp = New dd316.CategoryPurpose1Choice() With {.Item = itm.VCode}}
                dbtTxn.IntrBkSttlmAmt = New dd316.ActiveCurrencyAndAmount() With {.Ccy = stCurrCode, .Value = CInt(amt)}
                dbtTxn.ChrgBr = dd316.ChargeBearerType1Code.SHAR
                dbtTxn.RCDt = Modscan.WORKING_DATE
                dbtTxn.DrctDbtTx.MndtRltdInf.MndtId = itm.MndtId
                dbtTxn.DrctDbtTx.MndtRltdInf.DtOfSgn = itm.DtOfSgntr
                dbtTxn.DrctDbtTx.MndtRltdInf.FnlColtDt = itm.FnlColltnDt
                If (itm.Frqcy.ToUpper = "D") Then
                    dbtTxn.DrctDbtTx.MndtRltdInf.Frqcy = New dd316.Frequency21Choice With {.Item = dd316.Frequency6Code.MNTH}
                ElseIf (itm.Frqcy.ToUpper = "M") Then
                    dbtTxn.DrctDbtTx.MndtRltdInf.Frqcy = New dd316.Frequency21Choice With {.Item = dd316.Frequency6Code.MNTH}
                ElseIf (itm.Frqcy.ToUpper = "Y") Then
                    dbtTxn.DrctDbtTx.MndtRltdInf.Frqcy = New dd316.Frequency21Choice With {.Item = dd316.Frequency6Code.YEAR}
                ElseIf (itm.Frqcy.ToUpper = "W") Then
                    dbtTxn.DrctDbtTx.MndtRltdInf.Frqcy = New dd316.Frequency21Choice With {.Item = dd316.Frequency6Code.WEEK}
                ElseIf (itm.Frqcy.ToUpper = "Q") Then
                    dbtTxn.DrctDbtTx.MndtRltdInf.Frqcy = New dd316.Frequency21Choice With {.Item = dd316.Frequency6Code.QURT}
                ElseIf (itm.Frqcy.ToUpper = "F") Then
                    dbtTxn.DrctDbtTx.MndtRltdInf.Frqcy = New dd316.Frequency21Choice With {.Item = dd316.Frequency6Code.FRTN}
                End If
                dbtTxn.Cdtr = New dd316.PartyIdentification43 With {.Nm = itm.CNm.ToUpper} ', .PstlAdr = New dd316.PostalAddress6 With {.AdrLine = {"Katwe"}, .TwnNm = "Kampala", .Ctry = "UG"}}
                dbtTxn.CdtrAcct = New dd316.CashAccount24() With {.Id = New dd316.AccountIdentification4Choice() With {.Item = New dd316.GenericAccountIdentification1 With {.Id = itm.CdtrAcct}}}
                dbtTxn.CdtrAgt = New dd316.BranchAndFinancialInstitutionIdentification5 With {.FinInstnId = New dd316.FinancialInstitutionIdentification8 With {.BICFI = itm.sBIC.ToUpper, .ClrSysMmbId = New dd316.ClearingSystemMemberIdentification2() With {.MmbId = itm.SourceBankID}}}
                dbtTxn.Dbtr = New dd316.PartyIdentification43() _
                With {.Nm = itm.DNm.ToUpper} ', .PstlAdr = New dd316.PostalAddress6 With {.AdrLine = {"Katwe"}, .TwnNm = "Kampala", .Ctry = "UG"}}
                dbtTxn.DbtrAcct = New dd316.CashAccount24() With {.Id = New dd316.AccountIdentification4Choice() With {.Item = New dd316.GenericAccountIdentification1 With {.Id = itm.DbtrAcct}}}
                dbtTxn.DbtrAgt = New dd316.BranchAndFinancialInstitutionIdentification5 With {.FinInstnId = New dd316.FinancialInstitutionIdentification8 With {.BICFI = itm.dBIC.ToUpper, .ClrSysMmbId = New dd316.ClearingSystemMemberIdentification2() With {.MmbId = itm.DestBankID}}}
                dbtTxn.RmtInf = New dd316.RemittanceInformation11 With {.Ustrd = {itm.UstrdColD.ToUpper}}
                doc.FIToFICstmrDrctDbt.DrctDbtTxInf.Add(dbtTxn)
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
                    xDoc.Descendants().ToList()(7).SetValue(CDate(xStmDt.Value).ToString("yyyy-MM-dd"))
                    For i As Integer = 0 To m.Count - 1
                        Dim xE As XElement = m(i)
                        If xE.Name.LocalName = "ReqdColltnDt" Or xE.Name.LocalName = "DtOfSgntr" Then
                            xDoc.Descendants().ToList()(i).SetValue(CDate(xE.Value).ToString("yyyy-MM-dd"))
                        End If
                    Next
                    xDoc.Descendants().Where(Function(p) p.IsEmpty Or String.IsNullOrEmpty(p.Value)).Remove()
                    xDoc.Root.Attributes().Reverse()
                    xDoc.Save(fullpath, SaveOptions.None)
                    'If Sign Then
                    '    Dim p As BRCS = New BRCS()
                    '    Dim Mes As String = ""
                    '    Dim TempFilesFolder As String = "C:\Images\Temp"
                    '    Dim TempPathDest As String = ""
                    '    If Directory.Exists(TempFilesFolder) Then
                    '        TempPathDest = TempFilesFolder & "\" & Filename
                    '        File.Move(fullpath, TempPathDest)
                    '    End If
                    '    p.BRCDS(TempPathDest, fullpath, "h / KNJ1uE5CmUcQb4xbsfoW9ZPzk =", 71, Mes)
                    '    If File.Exists(TempPathDest) Then
                    '        File.Delete(TempPathDest)
                    '    End If
                    'End If



                    Dim Filter As String() = {"*.D*"}
                    Dim di As New DirectoryInfo(TempLocation)
                    Dim kl As New List(Of String)
                    For Each f As String In Filter
                        Dim fi As FileInfo() = di.GetFiles(f)
                        For Each inf As FileInfo In fi
                            kl.Add(inf.FullName)
                        Next
                    Next


                    Dim MessOut As String = ""
                    Dim destFileName As String = Path.Combine(StrDestinationFilePath, Filename)
                    File.Move(fullpath, destFileName)
                    MessOut = SignFiles_PKCS(fullpath.Trim(), StrDestinationFilePath.Trim(), x.Trim(), y.Trim())
                    Dim ArchivePath As String
                    If MessOut = "success" Then
                        ArchivePath = ConfigurationManager.AppSettings("Archive")
                        Dim FileDir As String = destFileName.ToString().Substring(0, destFileName.ToString().LastIndexOf("\"))
                        Clear_Files_Arc(FileDir.Trim(), ArchivePath.Trim(), FileDir.Trim(), "Out")
                    Else
                        MessageBox.Show("Failed Signing. " & Path.GetFileName(destFileName) & " : " & MessOut)
                        Modscan.ErrorLog(Path.GetFileName(destFileName) & " : " & MessOut, "- Signing ")
                    End If

                    'File.Move(fullpath, destFileName)
                    For Each itm As String In kl
                        File.Delete(itm)
                    Next
                    Return msgId
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        Return Nothing
    End Function

    Private Shared Function DDReject(ByVal l As List(Of DDUGDetail), ByVal ccy As String, ByVal BIC As String, ByVal x As String, ByVal y As String) As String
        Dim RegX As New Regex("[^A-Za-z0-9]")
        Dim dAmt As Decimal = 0
        For Each itm As DDUGDetail In l
            dAmt += CDec(itm.Amount)
        Next
        Dim amt As String = FormatNumber(dAmt, 2, Microsoft.VisualBasic.TriState.True, Microsoft.VisualBasic.TriState.True, Microsoft.VisualBasic.TriState.False)
        Dim sDt As String = Modscan.FDATE.ToString("dd-MMM-yyyy")
        Dim TimeSec As String = Now.ToString("HHmmss")
        Dim STm As String = Now.ToString("HH:mm")
        Dim xDt As Date = CDate(sDt & " " & STm)
        Dim stCurrCode As String = ""
        If ccy = 0 Then
            stCurrCode = "UGX"
        ElseIf ccy = 1 Then
            stCurrCode = "USD"
        ElseIf ccy = 2 Then
            stCurrCode = "GBP"
        ElseIf ccy = 3 Then
            stCurrCode = "EUR"
        ElseIf ccy = 4 Then
            stCurrCode = "JPY"
        ElseIf ccy = 5 Then
            stCurrCode = "KES"
        Else
            stCurrCode = "TZS"
        End If
        Dim FileCounter As String = GetScalarREC("Select isNull(FileCounter,0)+1 From t_Bank Where BankID = '" & l(0).DestBankID.PadLeft(2, "0") & "'")
        Dim Filename As String = l(0).DestBankID & Modscan.FDATE.ToString("ddMMyyyy") & Modscan.Sess & stCurrCode & FileCounter.ToString().PadLeft(2, "0") & ".W" & Modscan.OurBankID
        Dim msgId As String = "UNPDD/" & Modscan.FDATE.ToString("ddMMyyyy") & "/" & Modscan.Sess & TimeSec
        Dim doc As New dd416.Document()
        Dim grpHdr As New dd416.GroupHeader72()
        grpHdr.MsgId = msgId
        grpHdr.CreDtTm = xDt
        grpHdr.NbOfTxs = l.Count
        grpHdr.TtlRtrdIntrBkSttlmAmt = New dd416.ActiveCurrencyAndAmount() With {.Ccy = stCurrCode, .Value = CInt(amt)}
        grpHdr.IntrBkStlmDt = Modscan.FDATE.ToString("dd-MMM-yyyy")
        grpHdr.SttlmInf = New dd416.SettlementInstruction4() _
        With {.SttlmMtd = dd416.SettlementMethod1Code.CLRG, .ClrSys = New dd416.ClearingSystemIdentification3Choice() _
             With {.ItemElementName = dd416.ItemChoiceType2.Cd, .Item = "47"}}
        grpHdr.InstgAgt = New dd416.BranchAndFinancialInstitutionIdentification5 With {.FinInstnId = New dd416.FinancialInstitutionIdentification8 With {.BICFI = BIC.ToUpper, .ClrSysMmbId = New dd416.ClearingSystemMemberIdentification2() With {.MmbId = Modscan.OurBankID}}}
        grpHdr.InstdAgt = New dd416.BranchAndFinancialInstitutionIdentification5 With {.FinInstnId = New dd416.FinancialInstitutionIdentification8 With {.BICFI = l(0).dBIC.ToUpper, .ClrSysMmbId = New dd416.ClearingSystemMemberIdentification2() With {.MmbId = l(0).DestBankID}}}
        doc.PmtRtr.GrpHdr = grpHdr
        For Each d As DDUGDetail In l
            Try
                Dim txnInf As New dd416.PaymentTransaction65()
                txnInf.RtrId = d.TxId
                txnInf.OrgnlGrpInf.OrgnlMsgId = d.OrgnlMsgId
                txnInf.OrgnlGrpInf.OrgnlMsgNmId = "pacs.003.001.06"
                txnInf.OrgnlInstrId = d.OrgnlInstrID
                txnInf.OrgnlEndToEndId = d.OrgnlEndToEnd
                txnInf.OrgnlTxId = d.OrgnTrxID
                txnInf.OrgnlIntrBkSttlmAmt = New dd416.ActiveOrHistoricCurrencyAndAmount() With {.Ccy = stCurrCode, .Value = CInt(d.Amount)}
                txnInf.RtrdIntrBkSttlmAmt = New dd416.ActiveCurrencyAndAmount() With {.Ccy = stCurrCode, .Value = CInt(d.Amount)}
                txnInf.ChrgBrM = dd416.ChargeBearerType1Code.SHAR
                txnInf.RtrRsnInf = {New dd416.PaymentReturnReason1() With {.Rsn = New dd416.ReturnReason5Choice() With {.ItemElementName = dd416.ItemChoiceType7.Cd, .Item = d.Retcode}, .AddtlInf = {d.RetCodeDesc}}}
                txnInf.OrgnlTxRef.IntrBkSttlmAmt = New dd416.ActiveOrHistoricCurrencyAndAmount() With {.Ccy = stCurrCode, .Value = CInt(d.Amount)}
                If d.ReqdColltnDt = "" Then
                    'txnInf.OrgnlTxRef.ReqdColnDt = Nothing
                Else
                    Dim ReqdColltnDate As Date = Convert.ToDateTime(d.ReqdColltnDt)
                    txnInf.OrgnlTxRef.ReqdColnDt = ReqdColltnDate.ToString("yyyy-MM-dd")
                End If
                txnInf.OrgnlTxRef.PmtTpInf = New dd416.PaymentTypeInformation25() With {.CtgyPurpl = New dd416.CategoryPurpose1Choice() With {.ItemElementName = dd416.ItemChoiceType4.Cd, .Item = d.VCode.Trim}}
                Try
                    txnInf.OrgnlTxRef.MndtRltdInf.MndtId = d.MndtId
                Catch exMndtId As Exception

                End Try

                Try
                    txnInf.OrgnlTxRef.MndtRltdInf.DtOfSgnt = d.DtOfSgntr
                Catch DtOfSgntEx As Exception

                End Try

                Try
                    Dim ReqdColltnDate As Date = Convert.ToDateTime(d.ReqdColltnDt)
                    txnInf.OrgnlTxRef.MndtRltdInf.FnlColtDt = ReqdColltnDate.ToString("yyyy-MM-dd")
                Catch ex As Exception

                End Try
                If (d.Frqcy.ToUpper = "MNTH") Then
                    txnInf.OrgnlTxRef.MndtRltdInf.Frqcy = New dd416.Frequency21Choice With {.Item = dd416.Frequency6Code.MNTH}
                ElseIf (d.Frqcy.ToUpper = "YEAR") Then
                    txnInf.OrgnlTxRef.MndtRltdInf.Frqcy = New dd416.Frequency21Choice With {.Item = dd416.Frequency6Code.YEAR}
                ElseIf (d.Frqcy.ToUpper = "WEEK") Then
                    txnInf.OrgnlTxRef.MndtRltdInf.Frqcy = New dd416.Frequency21Choice With {.Item = dd416.Frequency6Code.WEEK}
                ElseIf (d.Frqcy.ToUpper = "QURT") Then
                    txnInf.OrgnlTxRef.MndtRltdInf.Frqcy = New dd416.Frequency21Choice With {.Item = dd416.Frequency6Code.QURT}
                ElseIf (d.Frqcy.ToUpper = "FRTN") Then
                    txnInf.OrgnlTxRef.MndtRltdInf.Frqcy = New dd416.Frequency21Choice With {.Item = dd416.Frequency6Code.FRTN}
                End If
                'txnInf.OrgnlTxRef.MndtRltdInf.Frqcy = New dd416.Frequency21Choice() With {.Item = d.Frqcy}
                txnInf.OrgnlTxRef.RmtInf = New dd416.RemittanceInformation11() With {.Ustrd = {d.UstrdColD.ToString}}
                txnInf.OrgnlTxRef.Dbtr = New dd416.PartyIdentification43() _
                                            With {.Nm = d.DNm, .PstlAdr = New dd416.PostalAddress6() With
                                                                          {.AdrLine = {d.DAdrLine}, .TwnNm = d.DTwnNm, .Ctry = d.DCtry}, .CtctDtls = New dd416.ContactDetails2() With {.Nm = d.DCNm, .PhneNb = d.DPhneNb, .MobNb = d.DMobNb, .EmailAdr = d.DEmailAdr, .Othr = d.DOthr}}
                txnInf.OrgnlTxRef.DbtrAcct = New dd416.CashAccount24() With {.Id = New dd416.AccountIdentification4Choice() With {.Item = New dd416.GenericAccountIdentification1 With {.Id = d.DbtrAcct}}}
                txnInf.OrgnlTxRef.DbtrAgt = New dd416.BranchAndFinancialInstitutionIdentification5 With {.FinInstnId = New dd416.FinancialInstitutionIdentification8 With {.BICFI = BIC.ToUpper, .ClrSysMmbId = New dd416.ClearingSystemMemberIdentification2() With {.MmbId = Modscan.OurBankID}}}
                txnInf.OrgnlTxRef.Cdtr = New dd416.PartyIdentification43() _
                    With {.Nm = d.CNm, .PstlAdr = New dd416.PostalAddress6() With
                                                                          {.AdrLine = {d.CAdrLine}, .TwnNm = d.CTwnNm, .Ctry = d.CCtry}, .CtctDtls = New dd416.ContactDetails2() With {.Nm = d.CCNm, .PhneNb = d.CPhneNb, .MobNb = d.CMobNb, .EmailAdr = d.CEmailAdr, .Othr = d.COthr}}
                txnInf.OrgnlTxRef.CdtrAcct = New dd416.CashAccount24() With {.Id = New dd416.AccountIdentification4Choice() With {.Item = New dd416.GenericAccountIdentification1 With {.Id = d.CdtrAcct}}}
                txnInf.OrgnlTxRef.CdtrAgt = New dd416.BranchAndFinancialInstitutionIdentification5 With {.FinInstnId = New dd416.FinancialInstitutionIdentification8 With {.BICFI = d.dBIC.ToUpper, .ClrSysMmbId = New dd416.ClearingSystemMemberIdentification2() With {.MmbId = d.DestBankID}}}


                'txnInf.OrgnlTxRef.Dbtr = New dd416.PartyIdentification43() _
                '                            With {.Nm = d.BeneficiaryName.ToUpper, .CtctDtls = New dd416.ContactDetails2() With {.Nm = d.BeneficiaryName.ToUpper}}
                'txnInf.OrgnlTxRef.DbtrAgt = New dd416.BranchAndFinancialInstitutionIdentification5 With {.FinInstnId = New dd416.FinancialInstitutionIdentification8 With {.BICFI = d.OurBankBic.ToUpper, .ClrSysMmbId = New dd416.ClearingSystemMemberIdentification2() With {.MmbId = Modscan.OurBankID}}}
                'txnInf.OrgnlTxRef.Cdtr = New dd416.PartyIdentification43() _
                '    With {.Nm = d.RemitterName.ToUpper, .CtctDtls = New dd416.ContactDetails2() With {.Nm = d.RemitterName.ToUpper}}
                'txnInf.OrgnlTxRef.CdtrAcct = New dd416.CashAccount24() With {.Id = New dd416.AccountIdentification4Choice() With {.Item = New dd416.GenericAccountIdentification1 With {.Id = d.RemitterAcc}}}
                'txnInf.OrgnlTxRef.CdtrAgt = New dd416.BranchAndFinancialInstitutionIdentification5 With {.FinInstnId = New dd416.FinancialInstitutionIdentification8 With {.BICFI = d.BankBIC.ToUpper, .ClrSysMmbId = New dd416.ClearingSystemMemberIdentification2() With {.MmbId = d.BankCode}}}
                txnInf.OrgnlTxRef.RmtInf = New dd416.RemittanceInformation11 With {.Ustrd = {d.UstrdColD.ToUpper}}

                doc.PmtRtr.TxInf.Add(txnInf)
            Catch ex As Exception
                MessageBox.Show("Error Registered, Check ErrorLog")
                Modscan.ErrorLog(ex.Message, "Unpaid Cheques Generation")
                Continue For
            End Try
        Next
        Try
            If Directory.Exists(TempLocation) Then
                Dim fullpath As String = Path.Combine(TempLocation, Filename)
                Dim ex As New Exception()
                If doc.SaveToFile(fullpath, ex) Then
                    Dim xDoc As XDocument = XDocument.Load(fullpath)
                    Dim k As List(Of XAttribute) = xDoc.Root.Attributes().ToList()
                    Dim xsd As XAttribute = k(1)
                    If xDoc.Root.HasAttributes Then xDoc.Root.Attribute(xsd.Name).Remove()
                    Dim xCreTm As XElement = xDoc.Descendants().ToList()(4)
                    xDoc.Descendants().ToList()(4).SetValue(CDate(xCreTm.Value).ToString("yyyy-MM-ddTHH:mm:sszzz"))
                    xDoc.Descendants().Where(Function(p) p.IsEmpty Or String.IsNullOrEmpty(p.Value)).Remove()
                    xDoc.Descendants().Where(Function(p) p.IsEmpty Or Date.Equals("0001-01-01", p.Value)).Remove()
                    'xDoc.Descendants().Where(Function(p) p.Name.LocalName = "MndtRltdInf").Remove()
                    xDoc.Root.Attributes().Reverse()
                    xDoc.Save(fullpath, SaveOptions.None)
                    'If Sign Then
                    '    Dim p As BRCS = New BRCS()
                    '    Dim Mes As String = ""
                    '    Dim TempFilesFolder As String = "C:\Images\Temp"
                    '    Dim TempPathDest As String = ""
                    '    If Directory.Exists(TempFilesFolder) Then
                    '        TempPathDest = TempFilesFolder & "\" & Filename
                    '        File.Move(fullpath, TempPathDest)
                    '    End If
                    '    p.BRCDS(TempPathDest, fullpath, "h / KNJ1uE5CmUcQb4xbsfoW9ZPzk =", 71, Mes)
                    '    If File.Exists(TempPathDest) Then
                    '        File.Delete(TempPathDest)
                    '    End If
                    'End If



                    Dim Filter As String() = {"*.W*"}
                    Dim di As New DirectoryInfo(TempLocation)
                    Dim kl As New List(Of String)
                    For Each f As String In Filter
                        Dim fi As FileInfo() = di.GetFiles(f)
                        For Each inf As FileInfo In fi
                            kl.Add(inf.FullName)
                        Next
                    Next

                    Dim MessOut As String = ""
                    Dim destFileName As String = Path.Combine(StrDestinationFilePath, Filename)
                    File.Move(fullpath, destFileName)
                    MessOut = SignFiles_PKCS(fullpath.Trim(), StrDestinationFilePath.Trim(), x.Trim(), y.Trim())
                    Dim ArchivePath As String
                    If MessOut = "success" Then
                        ArchivePath = ConfigurationManager.AppSettings("Archive")
                        Dim FileDir As String = destFileName.ToString().Substring(0, destFileName.ToString().LastIndexOf("\"))
                        Clear_Files_Arc(FileDir.Trim(), ArchivePath.Trim(), FileDir.Trim(), "Out")
                    Else
                        MessageBox.Show("Failed Signing. " & Path.GetFileName(destFileName) & " : " & MessOut)
                        Modscan.ErrorLog(Path.GetFileName(destFileName) & " : " & MessOut, "- Signing ")
                    End If

                    'File.Move(fullpath, destFileName)
                    For Each itm As String In kl
                        File.Delete(itm)
                    Next
                    Return msgId
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Error Registered, Check ErrorLog")
            Modscan.ErrorLog(ex.Message, "Unpaid Cheques Generation - ")
        End Try
        Return Nothing
    End Function
    Shared rnd As New Random()
    Public Shared usedStrings As New List(Of String)()
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
    Private Function InterBank(ByVal d As TZ.EFTDetails) As String
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

    Private Function SingleRTGS(ByVal d As TZ.EFTDetails) As String
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

    Private Function FreeFormat(ByVal d As TZ.EFTDetails) As String
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

    Private Function BalanceRequest(ByVal d As TZ.EFTDetails) As String
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

    Private Shared Function SISTransaction(ByVal det As UGChequeDetails) As Boolean
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
        strDetail += "Currency\ Code=" & IIf(det.CurrencyCode = "UGX", "1", "2") & vbNewLine
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

    Private Shared Function BulkCheques(ByVal det As List(Of UGChequeDetails), ByVal ccy As String, ByVal amt As Decimal, ByVal BIC As String, ByVal x As String, ByVal y As String) As String
        Dim sDt As String = Modscan.FDATE.ToString("dd-MMM-yyyy")
        Dim STm As String = Now.ToString("HH:mm")
        Dim TimeSec As String = Now.ToString("HHmmss")
        Dim MsgIdDt As String = Modscan.WORKING_DATE.ToString("ddMMyyyy")
        Dim xDt As Date = CDate(sDt & " " & STm)
        Dim insDt As Date = CDate(sDt)
        Dim ICounter As Int16 = 0
        Dim stCurrCode As String = ""
        If ccy = 0 Then
            stCurrCode = "UGX"
        ElseIf ccy = 1 Then
            stCurrCode = "USD"
        ElseIf ccy = 2 Then
            stCurrCode = "GBP"
        ElseIf ccy = 3 Then
            stCurrCode = "EUR"
        ElseIf ccy = 4 Then
            stCurrCode = "JPY"
        ElseIf ccy = 5 Then
            stCurrCode = "KES"
        Else
            stCurrCode = "TZS"
        End If
        Dim FileCounter As String = GetScalarREC("Select isNull(FileCounter,0)+1 From t_Bank Where BankID = '" & det(0).BankCode & "'")
        Dim ImageUniqueNames As String = det(0).BankCode & Modscan.FDATE.ToString("ddMMyyyy") & Modscan.Sess & stCurrCode & FileCounter.ToString().PadLeft(2, "0")
        Dim msgId As String = "OC" & det(0).BankCode & Modscan.FDATE.ToString("ddMMyyyy") & STm & Modscan.Sess & GetNextString()
        Dim sAmount As String = FormatNumber(amt, 2, Microsoft.VisualBasic.TriState.False, Microsoft.VisualBasic.TriState.False, Microsoft.VisualBasic.TriState.False).Replace(",", "")
        Dim doc As New dd316.Document()
        Dim grpHdr As New dd316.GroupHeader50()
        Dim ImageUniqueName As String = ""
        Dim fCreate As String = TempLocation & "\" & ImageUniqueNames
        If File.Exists(fCreate) Then fCreate &= "_1"
        ImageUniqueName = det(0).BankCode.Substring(0, 2) & Modscan.FDATE.ToString("ddMMyyyy") & Modscan.Sess & stCurrCode
        grpHdr.MsgId = "OC/" & Modscan.FDATE.ToString("ddMMyy") & "/" & Modscan.Sess & TimeSec
        grpHdr.CreDtTm = xDt
        grpHdr.NbOfTxs = det.Count
        grpHdr.TtlIntrBkSttlmAmt = New dd316.ActiveCurrencyAndAmount() With {.Ccy = stCurrCode, .Value = sAmount}
        grpHdr.test = Modscan.FDATE.ToString("dd-MMM-yyyy")
        grpHdr.SttlmInf = New dd316.SettlementInstruction2() _
        With {.SttlmMtd = dd316.SettlementMethod2Code.CLRG, .ClrSys = New dd316.ClearingSystemIdentification3Choice() _
             With {.ItemElementName = dd316.ItemChoiceType2.Cd, .Item = "47"}}
        grpHdr.PmtTpInf = New dd316.PaymentTypeInformation25() With {.LclInstrm = New dd316.LocalInstrument2Choice() With {.ItemElementName = dd316.ItemChoiceType4.Cd, .Item = "06"}}
        grpHdr.InstgAgt = New dd316.BranchAndFinancialInstitutionIdentification5 With {.FinInstnId = New dd316.FinancialInstitutionIdentification8 With {.BICFI = BIC.ToUpper, .ClrSysMmbId = New dd316.ClearingSystemMemberIdentification2() With {.MmbId = Modscan.OurBankID}}}
        grpHdr.InstdAgt = New dd316.BranchAndFinancialInstitutionIdentification5 With {.FinInstnId = New dd316.FinancialInstitutionIdentification8 With {.BICFI = det(0).BankBIC.ToUpper, .ClrSysMmbId = New dd316.ClearingSystemMemberIdentification2() With {.MmbId = det(0).BankCode}}}
        doc.FIToFICstmrDrctDbt.GrpHdr = grpHdr
        For Each d As UGChequeDetails In det
            ICounter = ICounter + 1
            ImageUniqueName = det(0).BankCode.Substring(0, 2) & Modscan.FDATE.ToString("ddMMyyyy") & Modscan.Sess & stCurrCode
            ImageUniqueName = ImageUniqueName & ICounter.ToString().PadLeft(6, "0")
            sAmount = FormatNumber(d.Amount, 2, Microsoft.VisualBasic.TriState.False, Microsoft.VisualBasic.TriState.False, Microsoft.VisualBasic.TriState.False).Replace(",", "")
            Dim c As New dd316.DirectDebitTransactionInformation20()
            c.PmtId = New dd316.PaymentIdentification3() _
            With {.InstrId = d.VoucherCode, .EndToEndId = d.ChequeNumber.ToString().PadLeft(6, "0"), .TxId = d.TrxID}
            c.PmtTpInf = New dd316.PaymentTypeInformation25() _
                With {.CtgyPurp = New dd316.CategoryPurpose1Choice() With {.Item = d.TransCode}}
            c.IntrBkSttlmAmt = New dd316.ActiveCurrencyAndAmount() With {.Ccy = stCurrCode, .Value = sAmount}
            'c.IntrBkSttlmDt = sDt
            c.ChrgBr = dd316.ChargeBearerType1Code.SHAR
            c.RCDt = CDate(d.ValueDate)
            c.Cdtr = New dd316.PartyIdentification43() With {.Nm = d.BeneficiaryName.ToUpper, .PstlAdr = New dd316.PostalAddress6 With {.AdrLine = {"Katwe"}, .TwnNm = "Kampala", .Ctry = "UG"}}
            '                                          , _
            '.CtctDtls = New dd316.ContactDetails2 With {.Nm = d.BeneficiaryName.ToUpper, .PhneNb = "+256-123-456789", .MobNb = "+256-123-456789", .EmailAdr = "Katwe"}}
            c.CdtrAcct = New dd316.CashAccount24() With {.Id = New dd316.AccountIdentification4Choice() With {.Item = New dd316.GenericAccountIdentification1 With {.Id = d.BeneficiaryAcc}}}
            c.CdtrAgt = New dd316.BranchAndFinancialInstitutionIdentification5 With {.FinInstnId = New dd316.FinancialInstitutionIdentification8 With {.BICFI = BIC.ToUpper, .ClrSysMmbId = New dd316.ClearingSystemMemberIdentification2() With {.MmbId = Modscan.OurBankID}}}
            c.Dbtr = New dd316.PartyIdentification43() _
            With {.Nm = d.RemitterName.ToUpper, .PstlAdr = New dd316.PostalAddress6 With {.AdrLine = {"Katwe"}, .TwnNm = "Kampala", .Ctry = "UG"}}
            ', _
            '                  .CtctDtls = New dd316.ContactDetails2 With {.Nm = "Test", .PhneNb = "+256-123-456789", .MobNb = "+256-123-456789", .EmailAdr = "Katwe@gmail.com"}}
            c.DbtrAcct = New dd316.CashAccount24() With {.Id = New dd316.AccountIdentification4Choice() With {.Item = New dd316.GenericAccountIdentification1() With {.Id = d.RemitterAcc}}}
            c.DbtrAgt = New dd316.BranchAndFinancialInstitutionIdentification5 With {.FinInstnId = New dd316.FinancialInstitutionIdentification8 With {.BICFI = d.BankBIC.ToUpper, .ClrSysMmbId = New dd316.ClearingSystemMemberIdentification2() With {.MmbId = d.BankCode}}}
            If d.BranchCode.Length > 2 Then
                d.BranchCode = d.BranchCode.Substring(1, 2)
            End If
            c.RmtInf = New dd316.RemittanceInformation11 With {.Ustrd = {d.TrxID,
                                                           (d.ChequeIndex).ToString().PadLeft(2, "0"),
                                                           (ImageUniqueName & "BWFM" & Modscan.OurBankID).ToString() & ".JPG",
                                                           (ImageUniqueName & "BWRM" & Modscan.OurBankID).ToString() & ".JPG",
                                                           (ImageUniqueName & "GSFM" & Modscan.OurBankID).ToString() & ".JPG",
                                                           (ImageUniqueName & "UVFM" & Modscan.OurBankID).ToString() & ".JPG",
                                                          d.ChequeNumber.ToString().PadLeft(6, "0") & d.ChequeIndex.ToString().PadLeft(2, "0") & d.BankCode.ToString().PadLeft(2, "0") & d.BranchCode.ToString().PadLeft(2, "0") & d.Region.ToString().PadLeft(2, "0") & d.RemitterAcc.ToString().PadLeft(10, "0") & d.TransCode.ToString().PadLeft(2, "0")}}
            doc.FIToFICstmrDrctDbt.DrctDbtTxInf.Add(c)


            fCreate = TempLocation & "\" & ImageUniqueName
            'If File.Exists(fCreate) Then fCreate &= "_1"

            'Write the Tif image
            'If File.Exists(fCreate & "BWF.M" & Modscan.OurBankID) Then fCreate &= "_1"
            Using fs As New FileStream(fCreate & "BWFM" & Modscan.OurBankID & ".JPG", FileMode.Create)
                fs.Write(d.FrontImageBW, 0, d.FrontImageBW.Length)
            End Using

            'Write the back image
            'If File.Exists(fCreate & "BWR.M" & Modscan.OurBankID) Then fCreate &= "_1"
            Using fs As New FileStream(fCreate & "BWRM" & Modscan.OurBankID & ".JPG", FileMode.Create)
                fs.Write(d.BackImageGS, 0, d.BackImageGS.Length)
            End Using

            'Write the front image
            'If File.Exists(fCreate & "GSF.M" & Modscan.OurBankID) Then fCreate &= "_1"

            Using fs As New FileStream(fCreate & "GSFM" & Modscan.OurBankID & ".JPG", FileMode.Create)
                fs.Write(d.FrontImageGS, 0, d.FrontImageGS.Length)
            End Using

            'Write the uv image
            'If File.Exists(fCreate & "UVF.M" & Modscan.OurBankID) Then fCreate &= "_1"
            Using fs As New FileStream(fCreate & "UVFM" & Modscan.OurBankID & ".JPG", FileMode.Create)
                fs.Write(d.FrontImageUV, 0, d.FrontImageUV.Length)
            End Using

        Next
        If Not Directory.Exists(StrDestinationFilePath) Then Directory.CreateDirectory(StrDestinationFilePath)
        If Directory.Exists(StrDestinationFilePath) Then
            Dim fullpath As String = Path.Combine(TempLocation, ImageUniqueNames & ".J" & det(0).OurBankID)
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
                xDoc.Descendants().ToList()(7).SetValue(CDate(xStmDt.Value).ToString("yyyy-MM-dd"))
                xDoc.Descendants().Where(Function(p) p.IsEmpty Or String.IsNullOrEmpty(p.Value)).Remove()
                xDoc.Root.Attributes().Reverse()
                xDoc.Save(fullpath, SaveOptions.None)


                ZipContents(TempLocation, ImageUniqueNames & "J" & det(0).OurBankID & ".zip", New String() {"*.J*", "*.M*"}, "", True, x, y)


                Return msgId
            End If
        End If
        Return Nothing
    End Function

    Private Shared Function UnpaidCheques(ByVal l As List(Of UGChequeDetails), ByVal BIC As String, ByVal ccy As String, ByVal x As String, ByVal y As String) As String
        Dim RegX As New Regex("[^A-Za-z0-9]")
        Dim dAmt As Decimal = 0
        For Each itm As UGChequeDetails In l
            dAmt += CDec(itm.Amount)
        Next
        Dim amt As String = FormatNumber(dAmt, 2, Microsoft.VisualBasic.TriState.True, Microsoft.VisualBasic.TriState.True, Microsoft.VisualBasic.TriState.False)
        Dim sDt As String = Modscan.FDATE.ToString("dd-MMM-yyyy")
        Dim STm As String = Now.ToString("HH:mm")
        Dim TimeSec As String = Now.ToString("HHmmss")
        Dim xDt As Date = CDate(sDt & " " & STm)
        Dim stCurrCode As String = ""
        If ccy = 0 Then
            stCurrCode = "UGX"
        ElseIf ccy = 1 Then
            stCurrCode = "USD"
        ElseIf ccy = 2 Then
            stCurrCode = "GBP"
        ElseIf ccy = 3 Then
            stCurrCode = "EUR"
        ElseIf ccy = 4 Then
            stCurrCode = "JPY"
        ElseIf ccy = 5 Then
            stCurrCode = "KES"
        Else
            stCurrCode = "TZS"
        End If
        Dim FileCounter As String = GetScalarREC("Select isNull(FileCounter,0)+1 From t_Bank Where BankID = '" & l(0).BankCode.PadLeft(2, "0") & "'")
        Dim Filename As String = l(0).BankCode & Modscan.FDATE.ToString("ddMMyyyy") & Modscan.Sess & stCurrCode & FileCounter.PadLeft(2, "0") & ".U" & l(0).OurBankID
        Dim msgId As String = "UNPOC/" & Modscan.FDATE.ToString("ddMMyyyy") & "/" & Modscan.Sess & TimeSec
        Dim doc As New dd416.Document()
        Dim grpHdr As New dd416.GroupHeader72()
        grpHdr.MsgId = msgId
        grpHdr.CreDtTm = xDt
        grpHdr.NbOfTxs = l.Count
        grpHdr.TtlRtrdIntrBkSttlmAmt = New dd416.ActiveCurrencyAndAmount() With {.Ccy = stCurrCode, .Value = Decimal.Round(CDec(amt), 2)}
        grpHdr.IntrBkStlmDt = Modscan.FDATE.ToString("dd-MMM-yyyy")
        grpHdr.SttlmInf = New dd416.SettlementInstruction4() _
        With {.SttlmMtd = dd416.SettlementMethod1Code.CLRG, .ClrSys = New dd416.ClearingSystemIdentification3Choice() _
             With {.ItemElementName = dd416.ItemChoiceType2.Cd, .Item = "47"}}
        grpHdr.InstgAgt = New dd416.BranchAndFinancialInstitutionIdentification5 With {.FinInstnId = New dd416.FinancialInstitutionIdentification8 With {.BICFI = l(0).OurBankBic.ToUpper, .ClrSysMmbId = New dd416.ClearingSystemMemberIdentification2() With {.MmbId = Modscan.OurBankID}}}
        grpHdr.InstdAgt = New dd416.BranchAndFinancialInstitutionIdentification5 With {.FinInstnId = New dd416.FinancialInstitutionIdentification8 With {.BICFI = l(0).BankBIC.ToUpper, .ClrSysMmbId = New dd416.ClearingSystemMemberIdentification2() With {.MmbId = l(0).BankCode}}}
        doc.PmtRtr.GrpHdr = grpHdr
        For Each d As UGChequeDetails In l
            Try
                Dim txnInf As New dd416.PaymentTransaction65()
                txnInf.RtrId = d.TrxID
                txnInf.OrgnlGrpInf.OrgnlMsgId = d.OrgnlMsgId
                txnInf.OrgnlGrpInf.OrgnlMsgNmId = "pacs.003.001.06"
                txnInf.OrgnlInstrId = d.OrgnlInstrID
                txnInf.OrgnlEndToEndId = d.OrgnlEndToEnd
                txnInf.OrgnlTxId = d.OrgnTrxID
                txnInf.OrgnlIntrBkSttlmAmt = New dd416.ActiveOrHistoricCurrencyAndAmount() With {.Ccy = stCurrCode, .Value = Decimal.Round(CDec(d.Amount), 2)}
                txnInf.RtrdIntrBkSttlmAmt = New dd416.ActiveCurrencyAndAmount() With {.Ccy = stCurrCode, .Value = Decimal.Round(CDec(d.Amount), 2)}
                txnInf.ChrgBrM = dd416.ChargeBearerType1Code.SHAR
                txnInf.RtrRsnInf = {New dd416.PaymentReturnReason1() With {.Rsn = New dd416.ReturnReason5Choice() With {.ItemElementName = dd416.ItemChoiceType7.Cd, .Item = d.RetCode}, .AddtlInf = {d.RetCodeDesc}}}
                txnInf.OrgnlTxRef.IntrBkSttlmAmt = New dd416.ActiveOrHistoricCurrencyAndAmount() With {.Ccy = stCurrCode, .Value = Decimal.Round(CDec(d.Amount), 2)}
                If d.ReqdColltnDt = "" Then
                    'txnInf.OrgnlTxRef.ReqdColnDt = Nothing
                Else
                    Dim ReqdColltnDate As Date = Convert.ToDateTime(d.ReqdColltnDt)
                    txnInf.OrgnlTxRef.ReqdColnDt = ReqdColltnDate.ToString("yyyy-MM-dd")
                End If
                txnInf.OrgnlTxRef.PmtTpInf = New dd416.PaymentTypeInformation25() With {.CtgyPurpl = New dd416.CategoryPurpose1Choice() With {.ItemElementName = dd416.ItemChoiceType4.Cd, .Item = d.TransCode}}
                txnInf.OrgnlTxRef.RmtInf = New dd416.RemittanceInformation11() With {.Ustrd = {d.UstrdColD.ToString, d.ChequeIndex.ToString.PadLeft(2, "0"), d.UstrdBWF.ToString, d.UstrdBWR.ToString, d.UstrdGS.ToString, d.UstrdUV.ToString, d.UstrdMicr.ToString}}
                txnInf.OrgnlTxRef.Dbtr = New dd416.PartyIdentification43() _
                                            With {.Nm = d.DNm, .PstlAdr = New dd416.PostalAddress6() With
                                                                          {.AdrLine = {d.DAdrLine}, .TwnNm = d.DTwnNm, .Ctry = d.DCtry}, .CtctDtls = New dd416.ContactDetails2() With {.Nm = d.DCNm, .PhneNb = d.DPhneNb, .MobNb = d.DMobNb, .EmailAdr = d.DEmailAdr, .Othr = d.DOthr}}
                txnInf.OrgnlTxRef.DbtrAcct = New dd416.CashAccount24() With {.Id = New dd416.AccountIdentification4Choice() With {.Item = New dd416.GenericAccountIdentification1 With {.Id = d.DbtrAcct}}}
                txnInf.OrgnlTxRef.DbtrAgt = New dd416.BranchAndFinancialInstitutionIdentification5 With {.FinInstnId = New dd416.FinancialInstitutionIdentification8 With {.BICFI = d.OurBankBic.ToUpper, .ClrSysMmbId = New dd416.ClearingSystemMemberIdentification2() With {.MmbId = Modscan.OurBankID}}}
                txnInf.OrgnlTxRef.Cdtr = New dd416.PartyIdentification43() _
                    With {.Nm = d.CNm, .PstlAdr = New dd416.PostalAddress6() With
                                                                          {.AdrLine = {d.CAdrLine}, .TwnNm = d.CTwnNm, .Ctry = d.CCtry}, .CtctDtls = New dd416.ContactDetails2() With {.Nm = d.CCNm, .PhneNb = d.CPhneNb, .MobNb = d.CMobNb, .EmailAdr = d.CEmailAdr, .Othr = d.COthr}}
                txnInf.OrgnlTxRef.CdtrAcct = New dd416.CashAccount24() With {.Id = New dd416.AccountIdentification4Choice() With {.Item = New dd416.GenericAccountIdentification1 With {.Id = d.CdtrAcct}}}
                txnInf.OrgnlTxRef.CdtrAgt = New dd416.BranchAndFinancialInstitutionIdentification5 With {.FinInstnId = New dd416.FinancialInstitutionIdentification8 With {.BICFI = d.BankBIC.ToUpper, .ClrSysMmbId = New dd416.ClearingSystemMemberIdentification2() With {.MmbId = d.BankCode}}}


                'txnInf.OrgnlTxRef.Dbtr = New dd416.PartyIdentification43() _
                '                            With {.Nm = d.BeneficiaryName.ToUpper, .CtctDtls = New dd416.ContactDetails2() With {.Nm = d.BeneficiaryName.ToUpper}}
                'txnInf.OrgnlTxRef.DbtrAgt = New dd416.BranchAndFinancialInstitutionIdentification5 With {.FinInstnId = New dd416.FinancialInstitutionIdentification8 With {.BICFI = d.OurBankBic.ToUpper, .ClrSysMmbId = New dd416.ClearingSystemMemberIdentification2() With {.MmbId = Modscan.OurBankID}}}
                'txnInf.OrgnlTxRef.Cdtr = New dd416.PartyIdentification43() _
                '    With {.Nm = d.RemitterName.ToUpper, .CtctDtls = New dd416.ContactDetails2() With {.Nm = d.RemitterName.ToUpper}}
                'txnInf.OrgnlTxRef.CdtrAcct = New dd416.CashAccount24() With {.Id = New dd416.AccountIdentification4Choice() With {.Item = New dd416.GenericAccountIdentification1 With {.Id = d.RemitterAcc}}}
                'txnInf.OrgnlTxRef.CdtrAgt = New dd416.BranchAndFinancialInstitutionIdentification5 With {.FinInstnId = New dd416.FinancialInstitutionIdentification8 With {.BICFI = d.BankBIC.ToUpper, .ClrSysMmbId = New dd416.ClearingSystemMemberIdentification2() With {.MmbId = d.BankCode}}}


                doc.PmtRtr.TxInf.Add(txnInf)
            Catch ex As Exception
                MessageBox.Show("Error Registered, Check ErrorLog")
                Modscan.ErrorLog(ex.Message, "Unpaid Cheques Generation")
                Continue For
            End Try
        Next

        Try
            If Directory.Exists(TempLocation) Then
                Dim fullpath As String = Path.Combine(TempLocation, Filename)
                Dim ex As New Exception()
                If doc.SaveToFile(fullpath, ex) Then
                    Dim xDoc As XDocument = XDocument.Load(fullpath)
                    Dim k As List(Of XAttribute) = xDoc.Root.Attributes().ToList()
                    Dim xsd As XAttribute = k(1)
                    If xDoc.Root.HasAttributes Then xDoc.Root.Attribute(xsd.Name).Remove()
                    Dim xCreTm As XElement = xDoc.Descendants().ToList()(4)
                    xDoc.Descendants().ToList()(4).SetValue(CDate(xCreTm.Value).ToString("yyyy-MM-ddTHH:mm:sszzz"))
                    xDoc.Descendants().Where(Function(p) p.IsEmpty Or String.IsNullOrEmpty(p.Value)).Remove()
                    xDoc.Descendants().Where(Function(p) p.IsEmpty Or Date.Equals("0001-01-01", p.Value)).Remove()
                    xDoc.Descendants().Where(Function(p) p.Name.LocalName = "MndtRltdInf").Remove()
                    xDoc.Root.Attributes().Reverse()
                    xDoc.Save(fullpath, SaveOptions.None)
                    'If Sign Then
                    '    Dim p As BRCS = New BRCS()
                    '    Dim Mes As String = ""
                    '    Dim TempFilesFolder As String = "C:\Images\Temp"
                    '    Dim TempPathDest As String = ""
                    '    If Directory.Exists(TempFilesFolder) Then
                    '        TempPathDest = TempFilesFolder & "\" & Filename
                    '        File.Move(fullpath, TempPathDest)
                    '    End If
                    '    p.BRCDS(TempPathDest, fullpath, "h / KNJ1uE5CmUcQb4xbsfoW9ZPzk =", 71, Mes)
                    '    If File.Exists(TempPathDest) Then
                    '        File.Delete(TempPathDest)
                    '    End If
                    'End If


                    Dim Filter As String() = {"*.U*"}
                    Dim di As New DirectoryInfo(TempLocation)
                    Dim kl As New List(Of String)
                    For Each f As String In Filter
                        Dim fi As FileInfo() = di.GetFiles(f)
                        For Each inf As FileInfo In fi
                            kl.Add(inf.FullName)
                        Next
                    Next


                    Dim MessOut As String = ""
                    Dim destFileName As String = Path.Combine(StrDestinationFilePath, Filename)
                    File.Move(fullpath, destFileName)
                    MessOut = SignFiles_PKCS(fullpath.Trim(), StrDestinationFilePath.Trim(), x.Trim(), y.Trim())
                    Dim ArchivePath As String
                    If MessOut = "success" Then
                        ArchivePath = ConfigurationManager.AppSettings("Archive")
                        Dim FileDir As String = destFileName.ToString().Substring(0, destFileName.ToString().LastIndexOf("\"))
                        Clear_Files_Arc(FileDir.Trim(), ArchivePath.Trim(), FileDir.Trim(), "Out")
                    Else
                        MessageBox.Show("Failed Signing. " & Path.GetFileName(destFileName) & " : " & MessOut)
                        Modscan.ErrorLog(Path.GetFileName(destFileName) & " : " & MessOut, "- Signing ")
                    End If

                    'If File.Exists(destFileName) Then File.Delete(destFileName)
                    'File.Move(fullpath, destFileName)
                    For Each itm As String In kl
                        File.Delete(itm)
                    Next
                    Return msgId
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Error Registered, Check ErrorLog")
            Modscan.ErrorLog(ex.Message, "Unpaid Cheques Generation - ")
        End Try
        Return Nothing
    End Function
    Public Shared Sub ReadFile(ByRef sFile As String, ByVal sTemp As String)
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
            If Sign And Log Then SignFile(fullpath)
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

    Public Shared Sub ZipContents(ByVal OutFile As String, ByVal msgId As String, ByVal Filter As String(), Optional ByVal FilesLocation As String = "", Optional ByVal Log As Boolean = False, Optional ByVal x As String = "", Optional ByVal y As String = "")
        Try
            Dim sPath As String = IIf(FilesLocation = "", OutFile, FilesLocation)
            Dim di As New DirectoryInfo(sPath)
            Dim l As New List(Of String)
            For Each f As String In Filter
                Dim fi As FileInfo() = di.GetFiles(f)
                For Each inf As FileInfo In fi
                    l.Add(inf.FullName)
                Next
            Next

            sPath = Path.Combine(StrDestinationFilePath, msgId)
            If File.Exists(sPath) Then File.Delete(sPath)
            Dim fZip As New ZipFile(sPath)
            For Each itm As String In l
                fZip.AddFile(itm, "")
            Next
            fZip.Save()
            Application.DoEvents()
            Dim MessOut As String = ""
            Dim destFileName As String = Path.Combine(StrDestinationFilePath, sPath)
            MessOut = SignFiles_PKCS(sPath.Trim(), destFileName.Trim(), x.Trim(), y.Trim())
            Dim ArchivePath As String
            If MessOut = "success" Then
                ArchivePath = ConfigurationManager.AppSettings("Archive")
                Dim FileDir As String = sPath.ToString().Substring(0, sPath.ToString().LastIndexOf("\"))
                Clear_Files_Arc(FileDir.Trim(), ArchivePath.Trim(), FileDir.Trim(), "Out")
            Else
                MessageBox.Show("Failed Signing. " & Path.GetFileName(sPath) & " : " & MessOut)
                Modscan.ErrorLog(Path.GetFileName(sPath) & " : " & MessOut, "- Signing ")
            End If
            If Log Then

            End If
            'Clean up
            For Each itm As String In l

                File.Delete(itm)
            Next
            'File.Delete(sPath)
        Catch ex As Exception
            MessageBox.Show("Error Registered, Check ErrorLog")
            Modscan.ErrorLog(ex.Message, "- Zipping")
        End Try
    End Sub
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

        For Each fileName As String In filetoclear

            If File.Exists(fileName) Then
                File.Delete(fileName)
            End If
        Next
    End Sub
    Private Shared Sub SignFile(ByVal sFile As String)
        Try
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
            MessageBox.Show("Error registerd, check error log")
            Modscan.ErrorLog("output>>" & (If([String].IsNullOrEmpty(ex.Message), "(none)", ex.Message)), "- SignFile-TZ Files")
        End Try
    End Sub
    Public Shared Sub ExecuteCommand(ByVal strBatchPath As String)
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
                Modscan.ErrorLog("output>>" & (If([String].IsNullOrEmpty(output), "(none)", output)), "ExecuteCommand-TZ Files")
                Modscan.ErrorLog("error>>" & (If([String].IsNullOrEmpty([error]), "(none)", [error])), "ExecuteCommand-TZ Files")
            End Try
            ExitCode = process__1.ExitCode
            process__1.Close()
            Kill(Modscan.strBatchPath)
            strBatchPath = ""
        Catch ex As Exception
            MessageBox.Show("Error registerd, check error log")
            Modscan.ErrorLog(ex.Message, "- Out ExecuteCommand-TZ Files")
        End Try
    End Sub
    Public Sub StripSignature(ByRef sFile As String, ByVal sTemp As String)
        'Dim fBytes() As Byte = File.ReadAllBytes(sFile)
        'Dim m As New Montran(CertName)
        'fBytes = m.ReadSigned(fBytes)
        'If Not fBytes Is Nothing Then
        '    File.WriteAllBytes(sTemp, fBytes)
        '    sFile = sTemp
        'End If
    End Sub

    Private Shared Function IsClearingDate(ByRef xDt As Date) As Date
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
    Public Shared Function SignFiles(data As Byte(), cert As String) As Byte()
        'Dim signature = New ACH_Files.MonSig(cert)
        'Return signature.SignFile(data)
    End Function
    Private Sub SplitString(ByRef sOriginal As String, ByVal Chunks As Integer, Optional ByVal sSlashes As String = "")
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

Public Class UGInwards
#Region "Enums"
    Public Enum FileType
        Cheques = 0
        Efts = 1
        DD = 2
        RTGS = 3
        ChequeRejects = 4
        ATSStatements = 5
    End Enum

    Public Enum ChequeType
        SIS = 0
        XML = 1
    End Enum
#End Region

#Region "Variables"
    Shared Sign As Boolean = False
    Shared CertName As String = ""
    Shared TempLocation As String = ""
    Shared StrDestinationFilePath As String = ""
    Shared sOurBIC As String = ""
    Shared strFileLocation As String = ""
    Shared strCurFile As String = ""
    Shared sArchivePath As String = ""
    Shared sCorruptPath As String = ""
#End Region

#Region "Constructors"
    Public Sub New(ByVal Location As String)
        strFileLocation = ConfigurationManager.AppSettings("IncomingFiles")
        TempLocation = strFileLocation & "\Temp"
        StrDestinationFilePath = strFileLocation & "\Files"
        If Not Directory.Exists(StrDestinationFilePath) Then Directory.CreateDirectory(StrDestinationFilePath)
        If Not Directory.Exists(TempLocation) Then Directory.CreateDirectory(TempLocation)
        strFileLocation = Location
        sArchivePath = Path.Combine(strFileLocation, "ARCHIVE\" & Now.ToString("yyyyMMdd"))
        sCorruptPath = Path.Combine(sArchivePath, "CORRUPT")
        If Not Directory.Exists(TempLocation) Then Directory.CreateDirectory(TempLocation)
    End Sub
#End Region

#Region "Methods"

    Public Shared Sub ReadFiles(ByVal sFiles As List(Of String), ByRef lbl As Label, ByRef prgAll As ProgressBar, ByRef prg As ProgressBar, ByVal TokenUserName As String, Optional ByVal fType As FileType = FileType.Cheques)
        Try
            prgAll.Step = 100 / sFiles.Count
            prg.Step = 50
            Dim SourcePath As String = ""
            Dim DestPath As String = ""
            Dim arcs As String = ""
            Dim TempPathDest = ""
            Dim folderName As String = "C:\Images\Temp"

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
                    dest = Path.GetDirectoryName(dest)
                    arcs = dest
                    arcs = arcs
                    arcs = Path.GetDirectoryName(dest)
                    dest = dest & "\Files"
                    DestPath = dest
                    Dim FExt As String = ""
                    Dim siExt As String = Path.GetExtension(sFile.ToString()).ToUpper()
                    If siExt.Contains("ZIP") Then
                        FExt = ".ZIP"
                    End If
                    If siExt.Contains("T") Then
                        FExt = ".T"
                    End If
                    If (siExt.Contains("D")) Then
                        FExt = ".D"
                    End If
                    If (siExt.Contains("U")) Then
                        FExt = ".U"
                    End If
                    If (siExt.Contains("Y")) Then
                        FExt = ".Y"
                    End If
                    If (siExt.Contains("W")) Then
                        FExt = ".W"
                    End If
                    Sign = True
                    'If Directory.Exists(dest) Then Directory.Delete(dest, True)
                    If Not Directory.Exists(dest) Then Directory.CreateDirectory(dest)
                    Select Case FExt.ToUpper
                        Case ".ZIP"
                            If fType = FileType.ChequeRejects Then
                                sFile = Path.Combine(strFileLocation, sFile)
                                If RejectedItems(sFile) Then bArchive = True
                            Else
                                If Sign Then
                                    Dim p As BRCS = New BRCS()
                                    Dim Mes As String = ""
                                    TempPathDest = ""
                                    If Directory.Exists(folderName) Then
                                        TempPathDest = folderName & "\" & Path.GetFileName(sFile)
                                        Application.DoEvents()
                                        File.Move(sFile, TempPathDest)
                                        Application.DoEvents()
                                    Else

                                        Directory.CreateDirectory(folderName)
                                        Application.DoEvents()
                                        TempPathDest = TempPathDest & "\" & Path.GetFileName(sFile)
                                        Application.DoEvents()
                                        File.Move(sFile, TempPathDest)
                                        Application.DoEvents()
                                    End If
                                    Try
                                        Application.DoEvents()
                                        p.BRCDS(BRRSACryptography.CryptographyHelper.Encrypt(TempPathDest), BRRSACryptography.CryptographyHelper.Encrypt(sFile), BRRSACryptography.CryptographyHelper.Encrypt("h / KNJ1uE5CmUcQb4xbsfoW9ZPzk ="), 32, TokenUserName, "", Mes)
                                        'Dim Zipdir As New DirectoryInfo(sFile)
                                        'Dim FiletoZip As New List(Of String)
                                        'Dim Filter As String() = New String() {"*.J*", "*.jpg*"}
                                        'For Each f As String In Filter
                                        '    Dim zipfi As FileInfo() = Zipdir.GetFiles(f)
                                        '    For Each inf As FileInfo In zipfi
                                        '        FiletoZip.Add(inf.FullName)
                                        '    Next
                                        'Next

                                        'Dim fZip As New ZipFile(sFile)
                                        'For Each itm As String In FiletoZip
                                        '    fZip.AddFile(itm, "")
                                        'Next
                                        'fZip.Save()
                                        Application.DoEvents()
                                    Catch ex As Exception
                                        MessageBox.Show(ex.Message)
                                    End Try
                                    Dim ArchivePath As String = ConfigurationManager.AppSettings("Archive")
                                    Clear_Files_Arc(folderName.Trim(), ArchivePath.Trim(), folderName.Trim(), "In")
                                End If
                                Dim l As List(Of String) = UnzipFiles(sFile, New String() {"*.J*", "*.JPG"})
                                If l.AsQueryable().Any(Function(p) p.Contains("J") Or p.Contains("JPG")) Then
                                    Try
                                        BulkCheque(l, origFileName)
                                    Catch ex As Exception

                                    End Try
                                Else
                                    ChequeTransaction(l, ChequeType.SIS)
                                End If
                            End If
                        Case ".T"
                            If Sign Then
                                Dim p As BRCS = New BRCS()
                                Dim Mes As String = ""
                                TempPathDest = ""
                                If Directory.Exists(folderName) Then
                                    TempPathDest = folderName & "\" & Path.GetFileName(sFile)
                                    Application.DoEvents()
                                    File.Move(sFile, TempPathDest)
                                    Application.DoEvents()
                                Else

                                    Directory.CreateDirectory(folderName)
                                    Application.DoEvents()
                                    TempPathDest = TempPathDest & "\" & Path.GetFileName(sFile)
                                    Application.DoEvents()
                                    File.Move(sFile, TempPathDest)
                                    Application.DoEvents()
                                End If
                                Try
                                    If File.Exists(sFile) Then
                                        File.Delete(sFile)
                                    End If
                                Catch ex As Exception

                                End Try
                                Try
                                    Application.DoEvents()
                                    p.BRCDS(BRRSACryptography.CryptographyHelper.Encrypt(TempPathDest), BRRSACryptography.CryptographyHelper.Encrypt(sFile), BRRSACryptography.CryptographyHelper.Encrypt("h / KNJ1uE5CmUcQb4xbsfoW9ZPzk ="), 32, TokenUserName, "", Mes)
                                    Application.DoEvents()
                                Catch ex As Exception
                                    MessageBox.Show(ex.Message)
                                End Try
                                Dim ArchivePath As String = ConfigurationManager.AppSettings("Archive")
                                Clear_Files_Arc(folderName.Trim(), ArchivePath.Trim(), folderName.Trim(), "In")
                            End If
                            Try
                                BulkCredit(sFile)
                                If File.Exists(sFile) Then
                                    File.Delete(sFile)
                                End If
                            Catch ex As Exception

                            End Try
                        Case ".U", ".Y", ".W"
                            If Sign Then
                                Dim p As BRCS = New BRCS()
                                Dim Mes As String = ""
                                TempPathDest = ""
                                If Directory.Exists(folderName) Then
                                    TempPathDest = folderName & "\" & Path.GetFileName(sFile)
                                    Application.DoEvents()
                                    File.Move(sFile, TempPathDest)
                                    Application.DoEvents()
                                Else

                                    Directory.CreateDirectory(folderName)
                                    Application.DoEvents()
                                    TempPathDest = TempPathDest & "\" & Path.GetFileName(sFile)
                                    Application.DoEvents()
                                    File.Move(sFile, TempPathDest)
                                    Application.DoEvents()
                                End If
                                Try
                                    Application.DoEvents()
                                    p.BRCDS(BRRSACryptography.CryptographyHelper.Encrypt(TempPathDest), BRRSACryptography.CryptographyHelper.Encrypt(sFile), BRRSACryptography.CryptographyHelper.Encrypt("h / KNJ1uE5CmUcQb4xbsfoW9ZPzk ="), 32, TokenUserName, "", Mes)
                                    Application.DoEvents()
                                Catch ex As Exception
                                    MessageBox.Show(ex.Message)
                                End Try
                                Dim ArchivePath As String = ConfigurationManager.AppSettings("Archive")
                                Clear_Files_Arc(folderName.Trim(), ArchivePath.Trim(), folderName.Trim(), "In")
                            End If

                            Try
                                RejectedItems(sFile)
                                If File.Exists(sFile) Then
                                    File.Delete(sFile)
                                End If
                            Catch ex As Exception

                            End Try
                        Case ".n", ".D"
                            If Sign Then
                                Dim p As BRCS = New BRCS()
                                Dim Mes As String = ""
                                TempPathDest = ""
                                If Directory.Exists(folderName) Then
                                    TempPathDest = folderName & "\" & Path.GetFileName(sFile)
                                    Application.DoEvents()
                                    File.Move(sFile, TempPathDest)
                                    Application.DoEvents()
                                Else

                                    Directory.CreateDirectory(folderName)
                                    Application.DoEvents()
                                    TempPathDest = TempPathDest & "\" & Path.GetFileName(sFile)
                                    Application.DoEvents()
                                    File.Move(sFile, TempPathDest)
                                    Application.DoEvents()
                                End If
                                Try
                                    Application.DoEvents()
                                    p.BRCDS(BRRSACryptography.CryptographyHelper.Encrypt(TempPathDest), BRRSACryptography.CryptographyHelper.Encrypt(sFile), BRRSACryptography.CryptographyHelper.Encrypt("h / KNJ1uE5CmUcQb4xbsfoW9ZPzk ="), 32, TokenUserName, "", Mes)
                                    Application.DoEvents()
                                Catch ex As Exception
                                    MessageBox.Show(ex.Message)
                                End Try
                                Dim ArchivePath As String = ConfigurationManager.AppSettings("Archive")
                                Clear_Files_Arc(folderName.Trim(), ArchivePath.Trim(), folderName.Trim(), "In")
                            End If

                            Try
                                BulkDebit(sFile)
                                If File.Exists(sFile) Then
                                    File.Delete(sFile)
                                End If
                            Catch ex As Exception

                            End Try
                    End Select
                    prg.PerformStep()
                    prg.Update()
                    prgAll.PerformStep()
                    prgAll.Update()
                    Application.DoEvents()
                    'Try
                    '    If Not Directory.Exists(Path.GetDirectoryName(sArchivePath)) Then Directory.CreateDirectory(Path.GetDirectoryName(sArchivePath))
                    '    If (File.Exists(sArchiveFile)) Then sArchiveFile = sArchiveFile & Now.ToString("yyyyMMddHHmmss")
                    '    If File.Exists(SourcePath) And bArchive Then File.Copy(sFile, sArchiveFile)
                    'Catch ex As Exception

                    'End Try
                Catch ex As Exception
                    MessageBox.Show("Error registerd, check error log")
                    Modscan.ErrorLog(ex.Message, "- Inwards File : " + Path.GetFileName(sFile))
                    Continue For
                End Try
                'Dim dix As New DirectoryInfo(TempPathDest)
                'Dim fix As FileInfo() = dix.GetFiles()
                'For Each inf As FileInfo In fix
                '    File.Delete(inf.FullName)
                'Next
                'prgAll.Value = 100
                'prgAll.Update()
                Application.DoEvents()
            Next

            'Dim di As New DirectoryInfo(DestPath)
            'Dim fi As FileInfo() = di.GetFiles()
            'For Each inf As FileInfo In fi
            '    File.Delete(inf.FullName)
            'Next
            prgAll.Value = 100
            prgAll.Update()
            Application.DoEvents()

        Catch ex As Exception
            'errorLog(ex.StackTrace, "102", "Iwards Import", "Import Files", Now, "Import Files")
        End Try
    End Sub
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

        For Each fileName As String In filetoclear

            If File.Exists(fileName) Then
                File.Delete(fileName)
            End If
        Next
    End Sub
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
                        'Dim Resp As Res.TransactionGroupStatus3Code = doc.FIToFIPmtStsRpt.OrgnlGrpInfAndSts.GrpSts
                        'Dim MsgId As String = doc.FIToFIPmtStsRpt.OrgnlGrpInfAndSts.OrgnlMsgId
                        Dim Resp As String = ""
                        Dim MsgId As String = ""

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
                            sReason = ""
                            'sReason = doc.FIToFIPmtStsRpt.OrgnlGrpInfAndSts.StsRsnInf.Rsn.Item
                            strSQL = "UPDATE t_TransactionMICR SET FileStatus = 'R', RejectedReason = '" & sReason & "' WHERE Reference ='" & MsgId & "'"
                            Modscan.ExecuteData(strSQL, Nothing, Modscan.dataExecTypes.ExecTypeNonQuery, Modscan.queryType.SelectStatement)
                            Modscan.ExecuteData(Modscan.GetModify("p_AddClearingFileStatus", "FileDate", FileDate, "MessageID", MsgId, "FileName", FileName, "FileStatus", "Rejected", "RejectedReason", sReason, "Amount", Amount, "BankID", SwiftAccount, "TrxID", TrxID, "GroupStatus", "RJCT"), Nothing, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure)
                        ElseIf Resp = ISO.Responses.TransactionGroupStatus3Code.PART Then
                            strSQL = "UPDATE t_TransactionMICR SET FileStatus = 'P', RejectedReason = '" & sReason & "' WHERE Reference ='" & MsgId & "'"
                            Modscan.ExecuteData(strSQL, Nothing, Modscan.dataExecTypes.ExecTypeNonQuery, Modscan.queryType.SelectStatement)
                            Modscan.ExecuteData(Modscan.GetModify("p_AddClearingFileStatus", "FileDate", FileDate, "MessageID", MsgId, "FileName", FileName, "FileStatus", "Partial", "RejectedReason", "", "Amount", Amount, "BankID", SwiftAccount, "TrxID", TrxID, "GroupStatus", "PART"), Nothing, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure)
                        ElseIf Resp = res.TransactionGroupStatus3Code.CLRD Then
                            strSQL = "UPDATE t_TransactionMICR SET FileStatus = 'C', RejectedReason = '" & sReason & "' WHERE Reference ='" & MsgId & "'"
                            Modscan.ExecuteData(strSQL, Nothing, Modscan.dataExecTypes.ExecTypeNonQuery, Modscan.queryType.SelectStatement)
                            Modscan.ExecuteData(Modscan.GetModify("p_AddClearingFileStatus", "FileDate", FileDate, "MessageID", MsgId, "FileName", FileName, "FileStatus", "Cleared", "RejectedReason", "", "Amount", Amount, "BankID", SwiftAccount, "TrxID", TrxID, "GroupStatus", "CLRD"), Nothing, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure)

                        End If
                    Else
                        For item As Integer = 0 To doc.FIToFIPmtStsRpt.TxInfAndSts.Count - 1
                            Dim Resp As res.TransactionGroupStatus3Code = doc.FIToFIPmtStsRpt.TxInfAndSts(item).TxSts
                            Dim MsgId As String = "" ' doc.FIToFIPmtStsRpt.OrgnlGrpInfAndSts.OrgnlMsgId
                            Dim FileDate As String = doc.FIToFIPmtStsRpt.TxInfAndSts(item).OrgnlTxRef.IntrBkSttlmDt
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
                                sReason = "" 'doc.FIToFIPmtStsRpt.OrgnlGrpInfAndSts.StsRsnInf.Rsn.Item
                                strSQL = "UPDATE t_TransactionMICR SET FileStatus = 'R', RejectedReason = '" & sReason & "' WHERE Reference ='" & MsgId & "'"
                                Modscan.ExecuteData(strSQL, Nothing, Modscan.dataExecTypes.ExecTypeNonQuery, Modscan.queryType.SelectStatement)
                                Modscan.ExecuteData(Modscan.GetModify("p_AddClearingFileStatus", "FileDate", FileDate, "MessageID", MsgId, "FileName", FileName, "FileStatus", "Rejected", "RejectedReason", sReason, "Amount", Amount, "BankID", SwiftAccount, "TrxID", TrxID, "GroupStatus", "RJCT"), Nothing, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure)
                            ElseIf Resp = ISO.Responses.TransactionGroupStatus3Code.PART Then
                                strSQL = "UPDATE t_TransactionMICR SET FileStatus = 'P', RejectedReason = '" & sReason & "' WHERE Reference ='" & MsgId & "'"
                                Modscan.ExecuteData(strSQL, Nothing, Modscan.dataExecTypes.ExecTypeNonQuery, Modscan.queryType.SelectStatement)
                                Modscan.ExecuteData(Modscan.GetModify("p_AddClearingFileStatus", "FileDate", FileDate, "MessageID", MsgId, "FileName", FileName, "FileStatus", "Partial", "RejectedReason", "", "Amount", Amount, "BankID", SwiftAccount, "TrxID", TrxID, "GroupStatus", "PART"), Nothing, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure)
                            ElseIf Resp = res.TransactionGroupStatus3Code.CLRD Then
                                strSQL = "UPDATE t_TransactionMICR SET FileStatus = 'C', RejectedReason = '" & sReason & "' WHERE Reference ='" & MsgId & "'"
                                Modscan.ExecuteData(strSQL, Nothing, Modscan.dataExecTypes.ExecTypeNonQuery, Modscan.queryType.SelectStatement)
                                Modscan.ExecuteData(Modscan.GetModify("p_AddClearingFileStatus", "FileDate", FileDate, "MessageID", MsgId, "FileName", FileName, "FileStatus", "Cleared", "RejectedReason", "", "Amount", Amount, "BankID", SwiftAccount, "TrxID", TrxID, "GroupStatus", "CLRD"), Nothing, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure)

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

            Dim doc As New ct816.Document()
            Dim ex As New Exception()
            If ct816.Document.LoadFromFile(sFile, doc, ex) Then
                For Each c As ct816.CreditTransferTransaction25 In doc.FIToFICstmrCdtTrf.CdtTrfTxInf
                    Dim d As New EFTUGDetails
                    d.MsgId = doc.FIToFICstmrCdtTrf.GrpHdr.MsgId
                    d.TrxId = c.PmtId.TxId
                    d.Amount = c.IntrBkSttlmAmt.Value
                    d.Currency = c.IntrBkSttlmAmt.Ccy.ToString()
                    d.SourceBankID = c.DbtrAgt.FinInstnId.ClrSysMmbId.MmbId
                    d.VCode = c.PmtTpInf.CtgyPurp.Item
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
                        d.DbtrAcct = DirectCast(c.DbtrAcct.Id.Item, ct816.GenericAccountIdentification1).Id.ToString()
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
                        d.CdtrAcct = DirectCast(c.CdtrAcct.Id.Item, ct816.GenericAccountIdentification1).Id.ToString()
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
            'Dim strpath = sFile.LastIndexOf(".") + 1
            'sArchivePath = Path.Combine(strFileLocation, "ARCHIVE\" & Now.ToString("yyyyMMdd"))
            'Dim sArchiveFile As String = Path.Combine(sArchivePath, sFile)
            'If Not Directory.Exists(Path.GetDirectoryName(sArchivePath)) Then Directory.CreateDirectory(Path.GetDirectoryName(sArchivePath))
            'If (File.Exists(sArchiveFile)) Then sArchiveFile = sArchivePath & "\" & sFile.Substring(sFile.LastIndexOf("\") + 1)
            'Try
            '    If File.Exists(sFile) Then File.Move(sFile, sArchiveFile)
            'Catch exi As Exception
            '    File.Delete(sArchiveFile)
            '    If File.Exists(sFile) Then File.Move(sFile, sArchiveFile)
            'End Try
            File.Delete(sFile)
        Next
    End Sub

    Private Shared Sub BulkDebit(ByVal sFile As String)
        Dim RegX As New Regex("[^A-Za-z0-9]")
        Dim sTempFile As String = Path.Combine(TempLocation, sFile)
        sFile = Path.Combine(strFileLocation, sFile)
        'If Sign Then StripSignature(sFile, sTempFile)
        Dim doc As New dd316.Document()
        Dim doc2 As New res.Document()
        Dim ex As New Exception()
        If dd316.Document.LoadFromFile(sFile, doc, ex) Then
            For Each c As dd316.DirectDebitTransactionInformation20 In doc.FIToFICstmrDrctDbt.DrctDbtTxInf
                Try
                    Dim e As New DDUGDetail
                    e.MsgId = doc.FIToFICstmrDrctDbt.GrpHdr.MsgId
                    e.OrgnlInstrID = c.PmtId.InstrId
                    e.Amount = c.IntrBkSttlmAmt.Value
                    e.OrgnlEndToEnd = c.PmtId.EndToEndId
                    Try
                        e.ReqdColltnDt = IIf(IsDBNull(c.RCDt.ToString), "", c.RCDt.ToString)
                    Catch exReqdColltnDt As Exception
                        e.ReqdColltnDt = ""
                    End Try

                    Try
                        e.UstrdMicr = IIf(IsDBNull(c.RmtInf.Ustrd(6).ToString), "", c.RmtInf.Ustrd(6).ToString)
                    Catch exUstrdMicr As Exception
                        e.UstrdMicr = ""
                    End Try
                    Try
                        e.UstrdColD = IIf(IsDBNull(c.RmtInf.Ustrd(0).ToString), "", c.RmtInf.Ustrd(0).ToString)
                    Catch exUstrdColD As Exception
                        e.UstrdColD = ""
                    End Try
                    Try
                        e.DAdrLine = IIf(IsDBNull(c.Dbtr.PstlAdr.AdrLine(0).ToString), "", c.Dbtr.PstlAdr.AdrLine(0).ToString)
                    Catch exDAdrLine As Exception
                        e.DAdrLine = ""
                    End Try
                    Try
                        e.DTwnNm = IIf(IsDBNull(c.Dbtr.PstlAdr.TwnNm.ToString), "", c.Dbtr.PstlAdr.TwnNm)
                    Catch exDTwnNm As Exception
                        e.DTwnNm = ""
                    End Try
                    Try
                        e.DCtry = IIf(IsDBNull(c.Dbtr.PstlAdr.Ctry.ToString), "", c.Dbtr.PstlAdr.Ctry)
                    Catch exDCtry As Exception
                        e.DCtry = ""
                    End Try
                    Try
                        e.DNm = IIf(IsDBNull(c.Dbtr.Nm.ToString), "", c.Dbtr.Nm)
                    Catch exDNm As Exception
                        e.DNm = ""
                    End Try
                    Try
                        e.DCNm = IIf(IsDBNull(c.Dbtr.CtctDtls.Nm.ToString), "", c.Dbtr.CtctDtls.Nm)
                    Catch exDCNm As Exception
                        e.DCNm = ""
                    End Try
                    Try
                        e.DPhneNb = IIf(IsDBNull(c.Dbtr.CtctDtls.PhneNb.ToString), "", c.Dbtr.CtctDtls.PhneNb)
                    Catch exDPhneNb As Exception
                        e.DPhneNb = ""
                    End Try
                    Try
                        e.DMobNb = IIf(IsDBNull(c.Dbtr.CtctDtls.MobNb.ToString), "", c.Dbtr.CtctDtls.MobNb)
                    Catch exDMobNb As Exception
                        e.DMobNb = ""
                    End Try
                    Try
                        e.DEmailAdr = IIf(IsDBNull(c.Dbtr.CtctDtls.EmailAdr.ToString), "", c.Dbtr.CtctDtls.EmailAdr)
                    Catch exDEmailAdr As Exception
                        e.DEmailAdr = ""
                    End Try
                    Try
                        e.DOthr = IIf(IsDBNull(c.Dbtr.CtctDtls.Othr.ToString), "", c.Dbtr.CtctDtls.Othr)
                    Catch exDOthr As Exception
                        e.DOthr = ""
                    End Try
                    Try
                        e.DbtrAcct = DirectCast(c.DbtrAcct.Id.Item, dd316.GenericAccountIdentification1).Id
                    Catch exDbtrAcct As Exception
                        e.DbtrAcct = ""
                    End Try
                    Try
                        e.CAdrLine = IIf(IsDBNull(c.Cdtr.PstlAdr.AdrLine(0).ToString), "", c.Cdtr.PstlAdr.AdrLine(0).ToString)
                    Catch exCAdrLine As Exception
                        e.CAdrLine = ""
                    End Try
                    Try
                        e.CTwnNm = IIf(IsDBNull(c.Cdtr.PstlAdr.TwnNm.ToString), "", c.Cdtr.PstlAdr.TwnNm)
                    Catch exCTwnNm As Exception
                        e.CTwnNm = ""
                    End Try
                    Try
                        e.CCtry = IIf(IsDBNull(c.Cdtr.PstlAdr.Ctry.ToString), "", c.Cdtr.PstlAdr.Ctry)
                    Catch exCCtry As Exception
                        e.CCtry = ""
                    End Try
                    Try
                        e.CNm = c.Cdtr.Nm
                    Catch exCNm As Exception
                        e.CNm = ""
                    End Try
                    Try
                        e.CCNm = IIf(IsDBNull(c.Cdtr.CtctDtls.Nm.ToString), "", c.Cdtr.CtctDtls.Nm)
                    Catch exCCNm As Exception
                        e.CCNm = ""
                    End Try
                    Try
                        e.CPhneNb = IIf(IsDBNull(c.Cdtr.CtctDtls.PhneNb.ToString), "", c.Cdtr.CtctDtls.PhneNb)
                    Catch exCPhneNb As Exception
                        e.CPhneNb = ""
                    End Try
                    Try
                        e.CMobNb = IIf(IsDBNull(c.Cdtr.CtctDtls.MobNb.ToString), "", c.Cdtr.CtctDtls.MobNb)
                    Catch exCMobNb As Exception
                        e.CMobNb = ""
                    End Try
                    Try
                        e.CEmailAdr = IIf(IsDBNull(c.Cdtr.CtctDtls.EmailAdr.ToString), "", c.Cdtr.CtctDtls.EmailAdr)
                    Catch exCEmailAdr As Exception
                        e.CEmailAdr = ""
                    End Try
                    Try
                        e.COthr = IIf(IsDBNull(c.Dbtr.CtctDtls.Othr.ToString), "", c.Dbtr.CtctDtls.Othr)
                    Catch exCOthr As Exception
                        e.COthr = ""
                    End Try
                    Try
                        e.PymType = ""
                    Catch exPymType As Exception
                        e.PymType = ""
                    End Try
                    Try
                        e.OrgnlInstrID = IIf(IsDBNull(c.PmtId.InstrId.ToString), "", c.PmtId.InstrId)
                    Catch exOrgnlInstrID As Exception
                        e.OrgnlInstrID = ""
                    End Try
                    Try
                        e.CdtrAcct = DirectCast(c.CdtrAcct.Id.Item, dd316.GenericAccountIdentification1).Id
                    Catch exCdtrAcct As Exception
                        e.CdtrAcct = ""
                    End Try
                    Try
                        e.Frqcy = c.DrctDbtTx.MndtRltdInf.Frqcy.Item.ToString
                    Catch exCdtrAcct As Exception
                        e.Frqcy = ""
                    End Try
                    Try
                        e.MndtId = c.DrctDbtTx.MndtRltdInf.MndtId
                    Catch exMndtId As Exception
                        e.MndtId = ""
                    End Try
                    Try
                        e.DtOfSgntr = c.DrctDbtTx.MndtRltdInf.DtOfSgn
                    Catch exDtOfSgntr As Exception
                        e.DtOfSgntr = ""
                    End Try
                    Try
                        e.FnlColltnDt = IIf(IsDBNull(c.DrctDbtTx.MndtRltdInf.FnlColtDt.ToString), "", c.DrctDbtTx.MndtRltdInf.FnlColtDt.ToString)
                    Catch exFnlColltnDt As Exception
                        e.FnlColltnDt = ""
                    End Try
                    Try
                        e.TxId = c.PmtId.TxId
                    Catch exTxId As Exception
                        e.TxId = ""
                    End Try
                    Try
                        e.VCode = c.PmtTpInf.CtgyPurp.Item
                        If e.VCode = "" Then
                            e.VCode = c.PmtId.InstrId
                        End If
                    Catch vEx As Exception
                        e.VCode = ""
                    End Try
                    SaveDD(e, sFile)


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
        Dim eft As TZ.EFTDetails = GetRTGSDetails(sContent)
        If eft.TranType <> 1 And eft.TranType <> 2 Then
            'SaveMessage(eft, sFile)
        End If
        If eft.TranType = 1 Or eft.TranType = 2 Or eft.TranType = 3 Or eft.TranType = 4 Then
            If Not (eft.BeneficiaryAcc Is Nothing) Then
                'SaveEFT(eft, sFile)
            End If
        End If
        File.Delete(sTempFile)
    End Sub

    Private Shared Function GetRTGSDetails(ByVal sContent As String) As TZ.EFTDetails
        Dim RegX As New Regex("[^A-Za-z0-9]")
        Dim sDetals As String() = sContent.Split(Environment.NewLine.ToCharArray())
        sContent = sContent.Replace(vbCr & vbLf, "")
        Dim sGroups As String() = sContent.Split("{"c)
        Dim sAllowed As String() = New String() {"103", "202", "900", "910", "941", "950", "999"}
        Dim sMsghdr As String = sGroups(2).Substring(3, 3)
        Dim rec As New TZ.EFTDetails()
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
                rec.Currency = "UGX"
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

    Private Shared Sub ChequeTransaction(ByVal l As List(Of String), ByVal t As ChequeType)
        'Select Case t
        '    Case ChequeType.SIS
        '        For Each f As String In l
        '            If Path.GetExtension(f) = ".chequeItem" Then
        '                Dim sContent As String = ""
        '                Using fs As New FileStream(f, FileMode.Open, FileAccess.Read)
        '                    Using sr As New StreamReader(fs)
        '                        sContent = sr.ReadToEnd()
        '                    End Using
        '                End Using
        '                If sContent.Trim() <> "" Then
        '                    Dim fIndex As String = Path.GetFileNameWithoutExtension(f)
        '                    Dim inf As New DirectoryInfo(Path.GetDirectoryName(f))
        '                    Dim fImageFile As String = inf.GetFiles(fIndex & ".chequeFrontImage")(0).FullName
        '                    Dim bImageFile As String = inf.GetFiles(fIndex & ".chequeRearImage")(0).FullName
        '                    Dim chq As ChequeDetails = GetSISDetails(sContent, File.ReadAllBytes(fImageFile), File.ReadAllBytes(bImageFile))
        '                    SaveCheque(chq)
        '                End If
        '            End If
        '        Next
        '    Case ChequeType.XML
        '        Dim di As New DirectoryInfo(Path.GetDirectoryName(l(0)))
        '        Dim fi As FileInfo() = di.GetFiles("*.xml")
        '        For Each f As FileInfo In fi
        '            Dim xDoc As XDocument = XDocument.Load(f.FullName)
        '            Dim xRoot As XElement = xDoc.Root
        '            Dim xBulk As XElement = xRoot.Nodes().First()
        '            Dim xHeader As XElement = xBulk.Nodes().First()
        '            Dim xChqs As Array = xBulk.Nodes().Skip(1).ToArray()
        '            For Each xChq As XElement In xChqs
        '                Dim chq As New ChequeDetails
        '                chq.MsgId = DirectCast(xHeader.Nodes()(0), XElement).Value
        '                chq.Amount = DirectCast(xChq.Nodes()(2), XElement).Value
        '                chq.BankBIC = DirectCast(DirectCast(DirectCast(xChq.Nodes()(10), XElement).Nodes()(0), XElement).Nodes()(0),  _
        '                                XElement).Value
        '                Dim CrAgnt As XElement = DirectCast(xChq.Nodes()(7), XElement)
        '                chq.BankCode = DirectCast(DirectCast(CrAgnt.Nodes(0), XElement).Nodes()(0), XElement).Value
        '                chq.BranchCode = GetScalarREC("Select Top 1 BranchID From t_Branches Where BankID = '" & chq.BankCode & "'")
        '                chq.BeneficiaryAcc = DirectCast(DirectCast(DirectCast(xChq.Nodes()(6), XElement).Nodes()(0), XElement).Nodes()(0),  _
        '                                XElement).Value
        '                chq.BeneficiaryName = DirectCast(DirectCast(xChq.Nodes()(5), XElement).Nodes()(0), XElement).Value
        '                chq.OurBranch = DirectCast(DirectCast(xChq.Nodes()(4), XElement).Nodes()(4), XElement).Value.PadLeft(4, "0")
        '                chq.ChequeIndex = 1
        '                chq.ChequeNumber = DirectCast(DirectCast(xChq.Nodes()(4), XElement).Nodes()(0), XElement).Value
        '                chq.CreationDate = DirectCast(xHeader.Nodes()(1), XElement).Value
        '                chq.CurrencyCode = DirectCast(xChq.Nodes()(2), XElement).Attribute("Ccy").Value
        '                chq.EndorsmentNo = DirectCast(DirectCast(xChq.Nodes()(0), XElement).Nodes()(1), XElement).Value
        '                chq.FileName = f.Name
        '                chq.MICRED = True
        '                chq.RetCode = "00"
        '                chq.RemitterAcc = DirectCast(DirectCast(DirectCast(xChq.Nodes()(9), XElement).Nodes()(0), XElement).Nodes()(0),  _
        '                                XElement).Value
        '                chq.RemitterName = DirectCast(DirectCast(xChq.Nodes()(8), XElement).Nodes()(0), XElement).Value
        '                chq.TransCode = "CLG"
        '                Dim MLine As String = DirectCast(DirectCast(xChq.Nodes()(4), XElement).Nodes()(2), XElement).Value
        '                If MLine = "NO_MICROCODE" Then
        '                    MLine = chq.ChequeNumber & "/" & chq.BankCode & "/" & chq.BranchCode & "/0/" & chq.TransCode & "/99/" & _
        '                                            chq.RemitterAcc & "/" & chq.Amount
        '                End If
        '                chq.Codeline = MLine
        '                chq.ValueDate = CDate(DirectCast(xHeader.Nodes()(4), XElement).Value)
        '                Dim sPattern As String = "*" & chq.EndorsmentNo & "_"
        '                chq.BackImageGS = File.ReadAllBytes(di.GetFiles(sPattern & "back.tif*")(0).FullName)
        '                chq.FrontImageGS = File.ReadAllBytes(di.GetFiles(sPattern & "front.tif*")(0).FullName)
        '                SaveCheque(chq)
        '            Next
        '        Next
        'End Select
    End Sub

    Private Shared Sub BulkCheque(ByVal l As List(Of String), Optional ByVal OrigFilename As String = "")
        Dim di As New DirectoryInfo(Path.GetDirectoryName(l(0)))
        Dim EJfilePaths As String() = Nothing
        Dim Day As String = Modscan.FDATE.Day.ToString().PadLeft(2, "0")
        Dim Month As String = Modscan.FDATE.Month.ToString().PadLeft(2, "0")
        Dim FDayMonth As String = (Convert.ToString(New String("0"c, 2 - (BRBaseConvert.ConvertToString(Day).Length)) & BRBaseConvert.ConvertToString(Day)) & New String("0"c, 2 - (BRBaseConvert.ConvertToString(Month).Length))) + BRBaseConvert.ConvertToString(Month)
        Dim SelectedStringCol As StringCollection = New StringCollection()
        Dim ReWorkedStringCol As StringCollection = New StringCollection()
        Dim SystemType As String = ConfigurationManager.AppSettings("sysType")
        EJfilePaths = Directory.GetFiles(Path.GetDirectoryName(l(0)))
        For f As Int32 = 0 To EJfilePaths.Length - 1
            If EJfilePaths(f).Substring(EJfilePaths(f).ToString().LastIndexOf("\") + 1).Substring(2, 4) = FDayMonth Then
                SelectedStringCol.Add(EJfilePaths(f).ToString())
            End If
        Next
        For p As Int32 = 0 To SelectedStringCol.Count - 1
            If (SelectedStringCol(p).Substring(SelectedStringCol(p).ToString().LastIndexOf(".") + 1).Substring(0, 1).ToUpper() = "J" And SelectedStringCol(p).Substring(SelectedStringCol(p).ToString().LastIndexOf(".") + 1).Substring(0, 3).ToUpper() <> "JPG") Then
                ReWorkedStringCol.Add(SelectedStringCol(p).ToString())
            End If
        Next
        For f As Int32 = 0 To ReWorkedStringCol.Count - 1
            Dim doc As New dd316.Document()
            Dim ex As New Exception()
            If dd316.Document.LoadFromFile(ReWorkedStringCol(f).ToString(), doc, ex) Then
                Dim chq As New UGChequeDetails
                chq.MsgId = doc.FIToFICstmrDrctDbt.GrpHdr.MsgId
                chq.CreationDate = doc.FIToFICstmrDrctDbt.GrpHdr.CreDtTm
                For Each c As dd316.DirectDebitTransactionInformation20 In doc.FIToFICstmrDrctDbt.DrctDbtTxInf
                    'chq.ValueDate = doc.FIToFICstmrDrctDbt.DrctDbtTxInf.
                    chq.Amount = c.IntrBkSttlmAmt.Value
                    chq.BankCode = c.CdtrAgt.FinInstnId.ClrSysMmbId.MmbId
                    'chq.BranchCode = c.ChequeTx.BranchCode
                    chq.BeneficiaryAcc = DirectCast(c.CdtrAcct.Id.Item, dd316.GenericAccountIdentification1).Id
                    chq.BeneficiaryName = c.Cdtr.Nm
                    chq.OurBranch = "" 'c.ChequeTx.BranchCode.PadLeft(3, "0")
                    chq.ChequeIndex = c.RmtInf.Ustrd(1).ToString
                    chq.ChequeNumber = c.PmtId.EndToEndId.Substring(0, 6)
                    chq.OrgnlEndToEnd = c.PmtId.EndToEndId
                    chq.CurrencyCode = c.IntrBkSttlmAmt.Ccy.ToString()
                    chq.FileName = OrigFilename
                    Try
                        chq.RemitterAcc = DirectCast(c.DbtrAcct.Id.Item, dd316.GenericAccountIdentification1).Id
                    Catch exRemitterName As Exception
                        chq.RemitterAcc = ""
                    End Try

                    Try
                        chq.RemitterName = IIf(IsDBNull(c.Dbtr.Nm.ToString), "", c.Dbtr.Nm.ToString)
                    Catch exRemitterName As Exception
                        chq.RemitterName = ""
                    End Try

                    Try
                        chq.ReqdColltnDt = IIf(IsDBNull(c.RCDt.ToString), "", c.RCDt.ToString)
                    Catch exReqdColltnDt As Exception
                        chq.ReqdColltnDt = ""
                    End Try
                    Try
                        chq.UstrdMicr = IIf(IsDBNull(c.RmtInf.Ustrd(6).ToString), "", c.RmtInf.Ustrd(6).ToString)
                    Catch exUstrdMicr As Exception
                        chq.UstrdMicr = ""
                    End Try
                    Try
                        chq.UstrdColD = IIf(IsDBNull(c.RmtInf.Ustrd(0).ToString), "", c.RmtInf.Ustrd(0).ToString)
                    Catch exUstrdColD As Exception
                        chq.UstrdColD = c.RmtInf.Ustrd(0).ToString
                    End Try
                    Try
                        chq.DAdrLine = IIf(IsDBNull(c.Dbtr.PstlAdr.AdrLine(0).ToString), "", c.Dbtr.PstlAdr.AdrLine(0).ToString)
                    Catch exDAdrLine As Exception
                        chq.DAdrLine = ""
                    End Try
                    Try
                        chq.DTwnNm = IIf(IsDBNull(c.Dbtr.PstlAdr.TwnNm.ToString), "", c.Dbtr.PstlAdr.TwnNm)
                    Catch exDTwnNm As Exception
                        chq.DTwnNm = ""
                    End Try
                    Try
                        chq.DCtry = IIf(IsDBNull(c.Dbtr.PstlAdr.Ctry.ToString), "", c.Dbtr.PstlAdr.Ctry)
                    Catch exDCtry As Exception
                        chq.DCtry = ""
                    End Try
                    Try
                        chq.DNm = IIf(IsDBNull(c.Dbtr.Nm.ToString), "", c.Dbtr.Nm)
                    Catch exDNm As Exception
                        chq.DNm = ""
                    End Try
                    Try
                        chq.DCNm = IIf(IsDBNull(c.Dbtr.CtctDtls.Nm.ToString), "", c.Dbtr.CtctDtls.Nm)
                    Catch exDCNm As Exception
                        chq.DCNm = ""
                    End Try
                    Try
                        chq.DPhneNb = IIf(IsDBNull(c.Dbtr.CtctDtls.PhneNb.ToString), "", c.Dbtr.CtctDtls.PhneNb)
                    Catch exDPhneNb As Exception
                        chq.DPhneNb = ""
                    End Try
                    Try
                        chq.DMobNb = IIf(IsDBNull(c.Dbtr.CtctDtls.MobNb.ToString), "", c.Dbtr.CtctDtls.MobNb)
                    Catch exDMobNb As Exception
                        chq.DMobNb = ""
                    End Try
                    Try
                        chq.DEmailAdr = IIf(IsDBNull(c.Dbtr.CtctDtls.EmailAdr.ToString), "", c.Dbtr.CtctDtls.EmailAdr)
                    Catch exDEmailAdr As Exception
                        chq.DEmailAdr = ""
                    End Try
                    Try
                        chq.DOthr = IIf(IsDBNull(c.Dbtr.CtctDtls.Othr.ToString), "", c.Dbtr.CtctDtls.Othr)
                    Catch exDOthr As Exception
                        chq.DOthr = ""
                    End Try
                    Try
                        chq.DbtrAcct = DirectCast(c.DbtrAcct.Id.Item, dd316.GenericAccountIdentification1).Id
                    Catch exDbtrAcct As Exception
                        chq.DbtrAcct = ""
                    End Try
                    Try
                        chq.CAdrLine = IIf(IsDBNull(c.Cdtr.PstlAdr.AdrLine(0).ToString), "", c.Cdtr.PstlAdr.AdrLine(0).ToString)
                    Catch exCAdrLine As Exception
                        chq.CAdrLine = ""
                    End Try
                    Try
                        chq.CTwnNm = IIf(IsDBNull(c.Cdtr.PstlAdr.TwnNm.ToString), "", c.Cdtr.PstlAdr.TwnNm)
                    Catch exCTwnNm As Exception
                        chq.CTwnNm = ""
                    End Try
                    Try
                        chq.CCtry = IIf(IsDBNull(c.Cdtr.PstlAdr.Ctry.ToString), "", c.Cdtr.PstlAdr.Ctry)
                    Catch exCCtry As Exception
                        chq.CCtry = ""
                    End Try
                    Try
                        chq.CNm = c.Cdtr.Nm
                    Catch exCNm As Exception
                        chq.CNm = ""
                    End Try
                    Try
                        chq.CCNm = IIf(IsDBNull(c.Cdtr.CtctDtls.Nm.ToString), "", c.Cdtr.CtctDtls.Nm)
                    Catch exCCNm As Exception
                        chq.CCNm = ""
                    End Try
                    Try
                        chq.CPhneNb = IIf(IsDBNull(c.Cdtr.CtctDtls.PhneNb.ToString), "", c.Cdtr.CtctDtls.PhneNb)
                    Catch exCPhneNb As Exception
                        chq.CPhneNb = ""
                    End Try
                    Try
                        chq.CMobNb = IIf(IsDBNull(c.Cdtr.CtctDtls.MobNb.ToString), "", c.Cdtr.CtctDtls.MobNb)
                    Catch exCMobNb As Exception
                        chq.CMobNb = ""
                    End Try
                    Try
                        chq.CEmailAdr = IIf(IsDBNull(c.Cdtr.CtctDtls.EmailAdr.ToString), "", c.Cdtr.CtctDtls.EmailAdr)
                    Catch exCEmailAdr As Exception
                        chq.CEmailAdr = ""
                    End Try
                    Try
                        chq.COthr = IIf(IsDBNull(c.Dbtr.CtctDtls.Othr.ToString), "", c.Dbtr.CtctDtls.Othr)
                    Catch exCOthr As Exception
                        chq.COthr = ""
                    End Try
                    Try
                        chq.PymType = ""
                    Catch exPymType As Exception
                        chq.PymType = ""
                    End Try
                    Try
                        chq.OrgnlInstrID = IIf(IsDBNull(c.PmtId.InstrId.ToString), "", c.PmtId.InstrId)
                    Catch exOrgnlInstrID As Exception
                        chq.OrgnlInstrID = ""
                    End Try
                    Try
                        chq.CdtrAcct = DirectCast(c.CdtrAcct.Id.Item, dd316.GenericAccountIdentification1).Id
                    Catch exCdtrAcct As Exception
                        chq.CdtrAcct = ""
                    End Try
                    Try
                        chq.RemittanceInfo = "" 'DirectCast(c.CdtrAcct.Id.Item, dd316.GenericAccountIdentification1).Id
                    Catch exCdtrAcct As Exception
                        chq.RemittanceInfo = ""
                    End Try

                    Try
                        chq.VoucherCode = c.PmtTpInf.CtgyPurp.Item
                        If chq.VoucherCode = "" Then
                            chq.VoucherCode = c.PmtId.InstrId
                        End If
                    Catch vEx As Exception
                        chq.VoucherCode = c.PmtId.InstrId
                    End Try
                    chq.TrxID = c.PmtId.TxId
                    chq.Codeline = c.RmtInf.Ustrd(0).ToString
                    Dim JImageName As String = ""
                    Dim BImageName As String = ""
                    Dim RImageName As String = ""
                    Dim UImageName As String = ""
                    For i As Int32 = 0 To c.RmtInf.Ustrd.Length - 1
                        If c.RmtInf.Ustrd(i).Contains("GSF") Then
                            JImageName = c.RmtInf.Ustrd(i).ToString
                            chq.UstrdGS = c.RmtInf.Ustrd(i).ToString
                        End If
                        If c.RmtInf.Ustrd(i).Contains("BWF") Then
                            BImageName = c.RmtInf.Ustrd(i).ToString
                            chq.UstrdBWF = c.RmtInf.Ustrd(i).ToString
                        End If
                        If c.RmtInf.Ustrd(i).Contains("BWR") Then
                            RImageName = c.RmtInf.Ustrd(i).ToString
                            chq.UstrdBWR = c.RmtInf.Ustrd(i).ToString
                        End If
                        If c.RmtInf.Ustrd(i).Contains("UVF") Then
                            UImageName = c.RmtInf.Ustrd(i).ToString
                            chq.UstrdUV = c.RmtInf.Ustrd(i).ToString
                        End If

                    Next


                    Dim arrImages As New ArrayList
                    '----front gray scale
                    arrImages.AddRange(IO.Directory.GetFiles(Path.GetDirectoryName(l(0)), JImageName))
                    If arrImages.Count = 0 Then
                        arrImages.AddRange(IO.Directory.GetFiles(Path.GetDirectoryName(l(0)), JImageName))
                        If arrImages.Count = 0 Then
                            arrImages.AddRange(IO.Directory.GetFiles(Path.GetDirectoryName(l(0)), JImageName))
                        End If
                    End If
                    If arrImages.Count > 0 Then
                        chq.FrontImageGS = File.ReadAllBytes(arrImages(0).ToString)
                    End If
                    '----front BW
                    arrImages = New ArrayList
                    arrImages.AddRange(IO.Directory.GetFiles(Path.GetDirectoryName(l(0)), BImageName))
                    If arrImages.Count = 0 Then
                        arrImages.AddRange(IO.Directory.GetFiles(Path.GetDirectoryName(l(0)), BImageName))
                        If arrImages.Count = 0 Then
                            arrImages.AddRange(IO.Directory.GetFiles(Path.GetDirectoryName(l(0)), BImageName))
                        End If
                    End If
                    If arrImages.Count > 0 Then
                        chq.FrontImageBW = File.ReadAllBytes(arrImages(0).ToString)
                    End If
                    '------Back gray scale Image
                    arrImages = New ArrayList
                    'back grayscale image
                    arrImages.AddRange(IO.Directory.GetFiles(Path.GetDirectoryName(l(0)), RImageName))
                    If arrImages.Count = 0 Then
                        arrImages.AddRange(IO.Directory.GetFiles(Path.GetDirectoryName(l(0)), RImageName))
                        If arrImages.Count = 0 Then
                            arrImages.AddRange(IO.Directory.GetFiles(Path.GetDirectoryName(l(0)), RImageName))
                        End If
                    End If
                    If arrImages.Count > 0 Then
                        chq.BackImageGS = File.ReadAllBytes(arrImages(0).ToString)
                    End If
                    'Uv image
                    arrImages = New ArrayList
                    arrImages.AddRange(IO.Directory.GetFiles(Path.GetDirectoryName(l(0)), UImageName))
                    If arrImages.Count = 0 Then
                        arrImages.AddRange(IO.Directory.GetFiles(Path.GetDirectoryName(l(0)), UImageName))
                        If arrImages.Count = 0 Then
                            arrImages.AddRange(IO.Directory.GetFiles(Path.GetDirectoryName(l(0)), UImageName))
                        End If
                    End If
                    If arrImages.Count > 0 Then
                        chq.FrontImageUV = File.ReadAllBytes(arrImages(0).ToString)
                    End If

                    SaveCheque(chq)
                Next
            End If
            If Directory.Exists(ReWorkedStringCol(f).ToString().Substring(0, ReWorkedStringCol(f).ToString().LastIndexOf("\"))) Then
                Directory.Delete(ReWorkedStringCol(f).ToString().Substring(0, ReWorkedStringCol(f).ToString().LastIndexOf("\")), True)
            End If
        Next


    End Sub

    'Private Shared Function GetSISDetails(ByVal sDetail As String, ByVal sFront As Byte(), ByVal sRear As Byte()) As ChequeDetails
    '    Dim Details As String() = sDetail.Split(vbLf)
    '    Dim chq As New TZ.ChequeDetails
    '    chq.Amount = Details(10).Split("=")(1).Trim()
    '    chq.BackImageGS = sRear
    '    chq.BankCode = Details(9).Split("=")(1).Trim()
    '    chq.BeneficiaryAcc = Details(5).Split("=")(1).Trim()
    '    chq.BeneficiaryName = Details(15).Split("=")(1).Trim().Replace("'", "")
    '    chq.BranchCode = Details(3).Split("=")(1).Trim()
    '    chq.ChequeIndex = Details(7).Split("=")(1).Trim()
    '    chq.ChequeNumber = Details(6).Split("=")(1).Trim()
    '    chq.Codeline = Details(11).Split("=")(1).Trim()
    '    chq.CreationDate = Details(4).Split("=")(1).Trim()
    '    chq.CurrencyCode = Details(14).Split("=")(1).Trim()
    '    chq.EndorsmentNo = Details(13).Split("=")(1).Trim()
    '    chq.FileName = Details(16).Split("=")(1).Trim()
    '    chq.FrontImageGS = sFront
    '    chq.RemittanceInfo = Details(8).Split("=")(1).Trim().Replace("'", "")
    '    chq.RemitterAcc = Details(17).Split("=")(1).Trim()
    '    chq.RemitterName = Details(12).Split("=")(1).Trim().Replace("'", "")
    '    chq.TransCode = Details(2).Split("=")(1).Trim()
    '    chq.EndorsmentNo = IIf(chq.EndorsmentNo = "", Modscan.GetNextInt16, chq.EndorsmentNo)
    '    chq.CurrencyCode = IIf(chq.CurrencyCode = "1", "UGX", chq.CurrencyCode)
    '    Dim MLine As String = chq.ChequeNumber & "/" & chq.BankCode & "/" & chq.BranchCode & "/0/" & chq.TransCode & "/99/" & chq.RemitterAcc &
    '    "/" & chq.Amount
    '    chq.Codeline = IIf(chq.Codeline = "NO_MICROCODE" Or chq.Codeline = "", MLine, chq.Codeline)
    '    GetSISDetails = chq
    'End Function

    Private Shared Function RejectedItems(ByVal sFile As String) As Boolean
        Dim IsReject As Boolean = False
        Dim RegX As New Regex("[^A-Za-z0-9]")
        Dim sTempFile As String = Path.Combine(TempLocation, sFile)
        Sign = True
        sFile = Path.Combine(strFileLocation, sFile)
        'If Sign Then ReadFile(sFile, sTempFile)

        Dim doc As New dd416.Document()
        Dim ex As New Exception()
        If dd416.Document.LoadFromFile(sFile, doc, ex) Then
            For Each txnInf As dd416.PaymentTransaction65 In doc.PmtRtr.TxInf()
                Try
                    Dim res As New ACHResponse
                    Try
                        res.RtrId = txnInf.RtrId
                    Catch exRtrId As Exception
                        res.RtrId = ""
                    End Try
                    Try
                        res.OrgnlMsgId = txnInf.OrgnlGrpInf.OrgnlMsgId
                    Catch exOrgnlMsgId As Exception
                        res.OrgnlMsgId = ""
                    End Try
                    Try
                        res.OrgnTrxID = txnInf.OrgnlTxId
                    Catch exOrgnTrxID As Exception
                        res.OrgnTrxID = ""
                    End Try
                    Try
                        res.OrgnlInstrID = txnInf.OrgnlInstrId
                        If IsNothing(res.OrgnlInstrID) Then
                            res.OrgnlInstrID = ""
                        End If
                    Catch exOrgnlInstrID As Exception
                        res.OrgnlInstrID = ""
                    End Try
                    Try
                        res.OrgnlEndToEnd = txnInf.OrgnlEndToEndId
                        If IsNothing(res.OrgnlEndToEnd) Then
                            res.OrgnlEndToEnd = ""
                        End If
                    Catch exOrgnlEndToEnd As Exception
                        res.OrgnlEndToEnd = ""
                    End Try
                    Try
                        res.RetCode = txnInf.RtrRsnInf(0).Rsn.Item
                    Catch exRetCode As Exception
                        res.RetCode = ""
                    End Try
                    Try
                        res.RtrdIntrBkSttlmAmt = txnInf.RtrdIntrBkSttlmAmt.Value
                    Catch exRtrdIntrBkSttlmAmt As Exception
                        res.RtrdIntrBkSttlmAmt = ""
                    End Try
                    res.FileName = Path.GetFileName(sFile)
                    'txnInf.RtrRsnInf(0).AddtlInf(0)
                    UnpayItem(res)
                Catch exResp As Exception

                End Try
            Next
        End If




        'Dim doc As New res.Document()
        'Dim ex As New Exception()
        'If res.Document.LoadFromFile(sFile, doc, ex) Then
        '    IsReject = True
        '    Dim OrgMsgId As String = "" ' doc.FIToFIPmtStsRpt.OrgnlGrpInfAndSts.OrgnlMsgNmId
        '    For Each txn As res.PaymentTransactionInformation26 In doc.FIToFIPmtStsRpt.TxInfAndSts
        '        If OrgMsgId.StartsWith("pacs.005") Or OrgMsgId.StartsWith("pacs.002") Then
        '            Dim ch As New ChequeDetails
        '            ch.Amount = CDec(txn.OrgnlTxRef.IntrBkSttlmAmt.Value)
        '            ch.BankBIC = txn.OrgnlTxRef.DbtrAgt.FinInstnId.BIC
        '            ch.BeneficiaryAcc = RegX.Replace(txn.OrgnlTxRef.CdtrAcct.Id.Item.ToString(), String.Empty) '.Replace("'", "")
        '            ch.BeneficiaryName = RegX.Replace(txn.OrgnlTxRef.Cdtr.Nm, " ") '.Replace("'", "")
        '            ch.ChequeNumber = txn.OrgnlTxId
        '            ch.Codeline = txn.OrgnlTxRef.ChequeTx.Microcode
        '            ch.CurrencyCode = txn.OrgnlTxRef.IntrBkSttlmAmt.Ccy
        '            ch.EndorsmentNo = txn.OrgnlEndToEndId
        '            ch.RemittanceInfo = txn.OrgnlInstrId
        '            'If ch.RemittanceInfo Is Nothing Then ch.ChequeNumber = txn.OrgnlEndToEndId
        '            ch.FileName = Path.GetFileName(sFile)
        '            ch.MsgId = "" ' doc.FIToFIPmtStsRpt.OrgnlGrpInfAndSts.OrgnlMsgId
        '            ch.RemitterAcc = RegX.Replace(txn.OrgnlTxRef.DbtrAcct.Id.Item.ToString(), String.Empty) '.Replace("'", "")
        '            ch.RemitterName = RegX.Replace(txn.OrgnlTxRef.Dbtr.Nm, " ") '.Replace("'", "")
        '            ch.RetCode = txn.StsRsnInf.Rsn.Item
        '            ch.trxID = "" 'txn.StsId.ToString
        '            'If (ch.RetCode Is Nothing) Or Len(ch.RetCode) > 2 Then
        '            '    ch.RetCode = "AC01"
        '            'End If
        '            ch.ValueDate = txn.OrgnlTxRef.IntrBkSttlmDt
        ' UnpayCheque(ch)
        '            'SaveCheque(ch)
        '        ElseIf OrgMsgId.StartsWith("pacs.004") Then
        '            Dim d As New TZ.EFTDetails
        '            d.Amount = CDec(txn.OrgnlTxRef.IntrBkSttlmAmt.Value)
        '            d.Currency = txn.OrgnlTxRef.IntrBkSttlmAmt.Ccy.ToString()
        '            d.EFTID = txn.OrgnlTxId
        '            'd.OrgnlMsgId = doc.FIToFIPmtStsRpt.TxInf.OrgnlMsgId
        '            d.Reference = txn.OrgnlEndToEndId
        '            d.RetCode = txn.StsRsnInf.Rsn.Item
        '            d.TranType = 0
        '            d.ValueDate = txn.OrgnlTxRef.IntrBkSttlmDt
        '            'UnpayEft(d)
        '            'SaveEFT(d, OrgMsgId)
        '        End If
        '    Next
        'End If
        If Sign Then File.Delete(sTempFile)
        Return IsReject
    End Function
    Private Shared Sub UnpayItem(ByRef ACHRes As ACHResponse)
        Dim LineItemsTable As Hashtable = New Hashtable
        LineItemsTable.Add("OrgnlInstrID", ACHRes.OrgnlInstrID)
        LineItemsTable.Add("OrgnlEndToEnd", ACHRes.OrgnlEndToEnd)
        LineItemsTable.Add("OrgnTrxID", ACHRes.OrgnTrxID)
        LineItemsTable.Add("RetCode", ACHRes.RetCode)
        LineItemsTable.Add("RtrdIntrBkSttlmAmt", ACHRes.RtrdIntrBkSttlmAmt)
        LineItemsTable.Add("RtrId", ACHRes.RtrId)
        LineItemsTable.Add("FileName", ACHRes.FileName)
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
                    MessageBox.Show("Imechapa kwa UnpaidItems" + ex.Message)
                End Try
            Next
        End If
        Dim dr As DataRow = Modscan.dt.NewRow()
        For Each name As String In LineItemsTable.Keys
            dr(name) = LineItemsTable(name)
        Next
        Modscan.dt.Rows.Add(dr)

        Modscan.ACHResponse(Modscan.dt)
    End Sub
    Private Shared Sub UnpayCheque(ByRef ch As TZ.ChequeDetails)
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

    Private Sub UnpayEft(ByRef d As TZ.EFTDetails)
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
    Private Shared Sub SaveCheque(ByVal chq As UGChequeDetails)

        Try
            Dim RegX As New Regex("[^A-Za-z0-9]")
            Dim strArr As String = ""
            Dim LineItemsTable As Hashtable = New Hashtable
            Dim MethodDt As DataTable = New DataTable
            Dim SystemType As String = ConfigurationManager.AppSettings("sysType")


            chq.RetCode = "00"


            If chq.RetCode = "00" Then chq.ValueDate = Modscan.WORKING_DATE

            Dim ProcNo As String = Modscan.GetNextInt16 & Modscan.GetNextString
            '------------------------------------------------------------------------------------------------------
            LineItemsTable.Add("RCODE", chq.RetCode) ' RCODE
            LineItemsTable.Add("VTYPE", chq.VoucherCode) ' Voucher Type
            LineItemsTable.Add("AMOUNT", (Val(chq.Amount) / 1).ToString) ' Amount
            LineItemsTable.Add("ENTRYMODE", "0") ' Amount Entry Mode
            LineItemsTable.Add("CURRENCYCODE", chq.CurrencyCode) ' Amount Entry Mode
            LineItemsTable.Add("DESTBANK", Modscan.OurBankID) ' Dest Bank
            LineItemsTable.Add("DESTACC", chq.RemitterAcc) ' Dest Account
            LineItemsTable.Add("COLLACC", chq.BeneficiaryAcc) 'Collecting Account Details
            LineItemsTable.Add("DESTBRANCH", "") ' Dest Branch


            LineItemsTable.Add("CHQDGT", chq.ChequeIndex) ' Check Digit
            LineItemsTable.Add("PBANK", chq.BankCode) ' PBank
            LineItemsTable.Add("PBRANCH", "") ' PBranch
            LineItemsTable.Add("FILLER", "0") ' Filler

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

            LineItemsTable.Add("FrontBWImage", chq.FrontImageBW)
            LineItemsTable.Add("FrontGrayScaleImage", chq.FrontImageGS)
            LineItemsTable.Add("RearImage", chq.BackImageGS)
            LineItemsTable.Add("UVImage", chq.FrontImageUV)


            LineItemsTable.Add("FILENAME", chq.FileName) ' The Filename
            LineItemsTable.Add("ValidInvalid", True) 'Validity of the image
            LineItemsTable.Add("IsFCY", False)

            LineItemsTable.Add("MsgID", chq.MsgId)
            LineItemsTable.Add("TrxID", chq.TrxID)
            LineItemsTable.Add("UstrdBWF", chq.UstrdBWF)
            LineItemsTable.Add("UstrdBWR", chq.UstrdBWR)
            LineItemsTable.Add("UstrdGS", chq.UstrdGS)
            LineItemsTable.Add("UstrdUV", chq.UstrdUV)
            LineItemsTable.Add("UstrdMicr", chq.UstrdMicr)
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
            LineItemsTable.Add("CCNm", chq.CCNm)
            LineItemsTable.Add("DCNm", chq.DCNm)
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
                        'If DirectCast(LineItemsTable.Item("RCODE"), String) <> "00" Then
                        '    Select Case name.ToString
                        '        Case "FrontBWImage", "UVImage", "FrontGrayScaleImage", "RearImage"
                        '            ColName.DataType = System.Type.GetType("System.String")
                        '        Case Else
                        '            ColName.DataType = System.Type.GetType(LineItemsTable(name).GetType().FullName.ToString)
                        '    End Select
                        'Else
                        ColName.DataType = System.Type.GetType(LineItemsTable(name).GetType().FullName.ToString)
                        'End If


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
    Private Shared Sub SaveDD(ByVal d As DDUGDetail, ByVal sFile As String)
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
            d.Retcode = "40"
            '------------------------------------------------------------------------------------------------------
            LineItemsTable.Add("RCODE", d.Retcode) ' RCODE
            LineItemsTable.Add("VTYPE", d.VCode) ' Voucher Type
            LineItemsTable.Add("AMOUNT", (Val(d.Amount)).ToString) ' Amount
            LineItemsTable.Add("ENTRYMODE", "0") ' Amount Entry Mode
            LineItemsTable.Add("CURRENCYCODE", d.Curr) ' Amount Entry Mode
            LineItemsTable.Add("DESTBANK", Modscan.OurBankID) ' Dest Bank
            LineItemsTable.Add("DESTACC", d.DbtrAcct)
            LineItemsTable.Add("CHQDGT", "00") ' Check Digit
            LineItemsTable.Add("PBANK", d.SourceBankID) ' PBank
            LineItemsTable.Add("PBRANCH", "") ' PBranch
            LineItemsTable.Add("FILLER", "0") ' Filler
            LineItemsTable.Add("COLLACCName", d.DNm)
            LineItemsTable.Add("SNO", "") ' Serial Number
            LineItemsTable.Add("PROCNO", d.MsgId) ' Processing Number
            LineItemsTable.Add("DRN", d.MsgId) ' Processing Number
            LineItemsTable.Add("DATA", "") ' The Whole String as is
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
            LineItemsTable.Add("TrxID", d.TxId)
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
            LineItemsTable.Add("OrgnlEndToEnd", d.OrgnlEndToEnd)
            LineItemsTable.Add("ReqdColltnDt", d.ReqdColltnDt)
            LineItemsTable.Add("FnlColltnDt", d.FnlColltnDt)
            LineItemsTable.Add("DtOfSgntr", d.DtOfSgntr)
            LineItemsTable.Add("MndtId", d.MndtId)
            LineItemsTable.Add("Frqcy", d.Frqcy)
            LineItemsTable.Add("CCNm", d.CCNm)
            LineItemsTable.Add("DCNm", d.DCNm)
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
            'MessageBox.Show("TZ 3049")
            'Modscan.dt.TableName = "XMLTest"
            'Modscan.dt.WriteXml("C:\\TACH\\TACHfiles\\FromTACH\\Temp\\XmKamunya.xml", True)
            Modscan.SaveToDB(Modscan.dt, "IN")

            '------------------------------------------------------------------------------------------------------
        Catch ex As Exception
            MessageBox.Show("Imechapa kwa SaveEFT 2" + ex.Message)
        End Try


    End Sub
    Public Shared Sub ReadFile(ByRef sFile As String, ByVal sTemp As String)
        Dim sline As New List(Of String)(IO.File.ReadAllLines(sFile))
        Dim p As Integer = sline.LongCount
        sline.RemoveAt(p - 1)
        IO.File.WriteAllLines(sFile, sline.ToArray())
    End Sub
    Private Shared Sub SaveEFT(ByVal d As EFTUGDetails, ByVal sFile As String)
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
        'If Sign Then StripSignature(sFile, sTempFile)
        Dim zArchive As New ZipFile()
        Try
            If sOutLocation.Trim() = "" Then sOutLocation = Path.Combine(TempLocation, Path.GetFileNameWithoutExtension(sArchive))
            zArchive = New ZipFile(sFile)
            zArchive.ExtractAll(sOutLocation, ExtractExistingFileAction.OverwriteSilently)
            Dim di As New DirectoryInfo(sOutLocation)
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
            Modscan.ErrorLog("output>>" & (If([String].IsNullOrEmpty(ex.Message), "(none)", ex.Message)), "SignFile-TZ Files")
        End Try
    End Sub
    Public Shared Sub ExecuteCommand(ByVal strBatchPath As String)
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
            Modscan.ErrorLog("output>>" & (If([String].IsNullOrEmpty(output), "(none)", output)), "ExecuteCommand-TZ Files")
            Modscan.ErrorLog("error>>" & (If([String].IsNullOrEmpty([error]), "(none)", [error])), "ExecuteCommand-TZ Files")
        End Try
        ExitCode = process__1.ExitCode
        process__1.Close()
        Kill(strBatchPath)
    End Sub
    Public Shared Sub StripSignature(ByRef sFile As String, ByVal sTemp As String)
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