Imports BrClearing.Common.Modscan
Imports BrClearing.Common.BRTZClass
Imports System.Configuration.ConfigurationManager
Imports System.IO
Imports System.Configuration
Imports BrClearing.Util
Imports BREntities.ClearingFileFormat
Imports BRBase
Imports BREntities.BRCreateClearingFile
Imports BR.ApplicationBlocks.Data
Imports BR.DBClient
Imports System.IO.Compression
Imports System.Xml
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters
Imports System.Drawing

Public Class FrmBROutFile

    Private Enum CurrencyID
        TZS = 0
        USD = 1
        EUR = 3
        GBP = 2
        JPY = 4
    End Enum
    Private Sub FrmBROutFile_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim arr3 As ArrayList = New ArrayList()
        Dim ToClearingBank As String = String.Empty
        'System.Windows.Forms.MessageBox.Show("kugenerate sasa")
        Select Case CountryCode.ToUpper.Trim
            Case "UG"

            Case "SL"

            Case "TZ"
                Dim sPath As String = AppSettings("OutgoingFiles")
                Dim di As New DirectoryInfo(sPath)
                Dim fi As FileInfo() = di.GetFiles()
                For Each inf As FileInfo In fi
                    Kill(inf.FullName)
                Next
                FrmBROutFile.prgOutFile.Value = 0
                FrmBROutFile.prgOutFile.Update()
                Label1.Text = "Generation started please..."
                Label1.Update()
                Me.Visible = True
                lblBankID.Visible = False
                Wait(1.5)
                Label1.Text = "Generating Cheques."
                Label1.Update()
                FrmBROutFile.prgOutFile.Value = 15
                FrmBROutFile.prgOutFile.Update()
                ''Local
                'GenerateTZFiles(FileType.Cheques, 0, False, ChequeFormat.XMLPackages, "01")
                'FrmBROutFile.prgOutFile.Value = 25
                'FrmBROutFile.prgOutFile.Update()
                'Label1.Text = "Generating Local Cheques.."
                'Label1.Update()
                'Wait(1)
                ''Foreign
                'GenerateTZFiles(FileType.Cheques, 2, False, ChequeFormat.XMLPackages, "01")
                'FrmBROutFile.prgOutFile.Value = 25
                'FrmBROutFile.prgOutFile.Update()
                'Label1.Text = "Generating GBP Foreign Cheques.."
                'Label1.Update()
                'Wait(1)

                'GenerateTZFiles(FileType.Cheques, 1, False, ChequeFormat.XMLPackages, "01")
                'FrmBROutFile.prgOutFile.Value = 25
                'FrmBROutFile.prgOutFile.Update()
                'Label1.Text = "Generating USD Foreign Cheques.."
                'Label1.Update()
                'Wait(1)

                'GenerateTZFiles(FileType.Cheques, 3, False, ChequeFormat.XMLPackages, "01")
                'FrmBROutFile.prgOutFile.Value = 25
                'FrmBROutFile.prgOutFile.Update()
                'Label1.Text = "Generating EUR Foreign Cheques.."
                'Label1.Update()
                'Wait(1)

                'GenerateTZFiles(FileType.Cheques, 4, False, ChequeFormat.XMLPackages, "01")
                'FrmBROutFile.prgOutFile.Value = 25
                'FrmBROutFile.prgOutFile.Update()
                'Label1.Text = "Generating JYP Foreign Cheques.."
                'Label1.Update()
                'Wait(1)

                'GenerateTZFiles(FileType.Cheques, 5, False, ChequeFormat.XMLPackages, "01")
                'FrmBROutFile.prgOutFile.Value = 25
                'FrmBROutFile.prgOutFile.Update()
                'Label1.Text = "Generating KES Foreign Cheques.."
                'Label1.Update()
                'Wait(1)

                'GenerateTZFiles(FileType.ChequeReturn, 0, False, ChequeFormat.XMLPackages, "01")
                'FrmBROutFile.prgOutFile.Value = 50
                'FrmBROutFile.prgOutFile.Update()
                'Label1.Text = "Generating Unpaid Local Cheques.."
                'Label1.Update()
                'Wait(1)
                'Dim x As New GenerateTZFile
                'x(FileType.Efts, 0, False, ChequeFormat.XMLPackages, "01")
                'FrmBROutFile.prgOutFile.Value = 75
                'FrmBROutFile.prgOutFile.Update()
                'Label1.Text = "Generating EFTs...."
                'Label1.Update()
                'Wait(1)
                'GenerateTZFiles(FileType.EftReturn, 0, False, ChequeFormat.XMLPackages, "01")
                'FrmBROutFile.prgOutFile.Value = 100
                'FrmBROutFile.prgOutFile.Update()
                'Label1.Text = "Generating Unpaid EFTs...."
                'Wait(1)
                'Label1.Update()

                Dim destPath As String = Modscan.ArchivesPath & "\ToTach\" & DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "")
                If Not Directory.Exists(destPath) Then Directory.CreateDirectory(destPath)
                sPath = AppSettings("OutgoingFiles")
                di = New DirectoryInfo(sPath)
                fi = di.GetFiles()
                For Each inf As FileInfo In fi
                    File.Copy(inf.FullName, destPath & "\" & Path.GetFileName(inf.FullName), True)
                Next
                Label1.Text = "Done File Generation"
            Case "RD"

            Case "KE"
                ReadDataIntoADataTable()
                If AppSettings("isDDMan") = 0 Then
                    Exit Select
                End If

                arr3.Add("01")
                arr3.Add("02")
                arr3.Add("03")
                arr3.Add("04")
                arr3.Add("05")
                arr3.Add("06")
                arr3.Add("99")
                Dim FinalData As DataTable = New DataTable
                Dim FileExt As String = ""
                Dim CurrenctTrxTypeID As String = ""
                Dim MandateFT As String = ""
                Dim Country As String = CountryCode
                Dim TrxType As String = "FILEFORMATS"
                Dim usrInfo As BRBase.UserInfo = New BRBase.UserInfo
                usrInfo.strSystem = "BR"
                usrInfo.strUser = "SYS"
                usrInfo.strBranch = OurBranchID
                usrInfo.strLanguage = "en"
                usrInfo.strBank = OurBankID
                Dim fileName As String = String.Empty
                Dim dstrxClearing As DS_trxClearing = New DS_trxClearing()
                Dim drIsTrxGenerated As DataRow() = Nothing
                Dim dsClearingBanks As DataTable = New DataTable()
                Dim strCon As String() = New String(3) {}
                Dim dsClearingThroughThisBank As DataTable = New DataTable()
                Dim dsImages As DataTable = New DataTable()
                Dim dsClearingFileFormat As DS_ClearingFileFormat = New DS_ClearingFileFormat()
                Dim dsTrxClearingBankWise As DS_trxClearing = New DS_trxClearing()
                Dim dsClearing As DS_trxClearing = New DS_trxClearing()
                FinalData.Columns.Add("TrxRowID", GetType(String))
                FinalData.Columns.Add("text", GetType(String))
                FinalData.Columns.Add("FileName", GetType(String))
                FinalData.Columns.Add("ImageID", GetType(String))
                FinalData.Columns.Add("fcy", GetType(String))
                Dim sPath As String = AppSettings("OutgoingFiles")
                fileName = sPath
                Dim ToBank As String = ""
                ClearingFilesDataManuplation(usrInfo, TrxType, BRModule.GenerateClearingFile, "LOCAL", dsClearingFileFormat)
                ExecuteData(GetModify("p_RetreaveOutClearing", "FromDate", Modscan.cFromDate, "ToDate", Modscan.cToDate), dsImages, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
                ExecuteData(GetModify("p_OutClearingBank", "IsLocal0IsForeign1", "0", "Dir", "IN"), dsClearingBanks, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
                OutClearingData(usrInfo, BRModule.GenerateClearingFile, dsClearing, New Object() {BRBaseConvert.ConvertToDateTime(cFromDate), BRBaseConvert.ConvertToDateTime(Modscan.cToDate), "KES", 1})

                Dim dd As String = New String("0"c, 2 - BRBaseConvert.ConvertToString(WORKING_DATE.Day).ToString().Length) & BRBaseConvert.ConvertToString(WORKING_DATE.Day)
                Dim mm As String = New String("0"c, 2 - BRBaseConvert.ConvertToString(WORKING_DATE.Month).Length) & BRBaseConvert.ConvertToString(WORKING_DATE.Month)
                Dim yyyy As String = BRBaseConvert.ConvertToString(WORKING_DATE.Year)
                Dim ddmmmyyyy As String = dd & mm & yyyy
                Dim ddmmm As String = dd & mm
                For q As Int32 = 0 To arr3.Count - 1
                    If dsClearingBanks.Rows.Count > 0 Then
                        If dsClearingBanks.Rows.Count > 0 Then
                            For k = 0 To dsClearingBanks.Rows.Count - 1
                                'BankWise
                                ToBank = dsClearingBanks.Rows(k)("BankID").ToString().Trim()
                                ToBank = ToBank.ToString().Trim()
                                Try
                                    ExecuteData(GetModify("p_OutClearingThroughBank", "BankID", ToBank.ToString()), dsClearingThroughThisBank, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
                                Catch ex As Exception
                                    Dim AppendErrorMessage As String = "Error Message 911:" + ex.ToString() + Environment.NewLine + "Date" + ":" + DateTime.Now + Environment.NewLine + "--------------------------" + Environment.NewLine
                                    System.IO.File.AppendAllText("C:\ClearingFiles\" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + ".txt", AppendErrorMessage)
                                End Try
                                If dsClearingThroughThisBank.Rows.Count > 0 Then
                                    If dsClearingThroughThisBank.Rows.Count > 1 Then
                                        ToBank = String.Empty
                                        ToBank = "IN ('"
                                        For f As Int32 = 0 To dsClearingThroughThisBank.Rows.Count - 1
                                            'BankWise
                                            ToBank = ToBank + dsClearingThroughThisBank.Rows(f)("BankID").ToString() + "','"
                                        Next
                                        ToBank = ToBank.Substring(0, ToBank.LastIndexOf(","))
                                        ToBank = ToBank + ")"
                                    Else
                                        ToBank = "IN ('" + ToBank + "')"

                                    End If
                                End If
                                dsTrxClearingBankWise.Tables(0).Clear()
                                ToClearingBank = dsClearingBanks.Rows(k)("ClearingThrough").ToString()
                                ToClearingBank = ToClearingBank.ToString().Trim()
                                BankName = dsClearingBanks.Rows(k)("FullName").ToString()

                                If dsTrxClearingBankWise.Tables(0).Columns.Contains("CurrencyID") = False Then
                                    dsTrxClearingBankWise.Tables(0).Columns.Add("CurrencyID", GetType(String))
                                End If
                                If dsTrxClearingBankWise.Tables(0).Columns.Contains("TrxType") = False Then
                                    dsTrxClearingBankWise.Tables(0).Columns.Add("TrxType", GetType(String))
                                End If

                                If arr3(q).ToString() = "01" And Country.ToUpper() = "KE" Then
                                    Select Case Country.ToUpper()
                                        Case "KE"
                                            FileExt = "M"
                                            CurrenctTrxTypeID = "OD"
                                            MandateFT = "01"
                                            Exit Select
                                    End Select
                                ElseIf arr3(q).ToString() = "02" AndAlso Country.ToUpper() = "KE" Then
                                    Select Case Country.ToUpper()
                                        Case "KE"
                                            FileExt = "M"
                                            CurrenctTrxTypeID = "OD"
                                            MandateFT = "02"
                                            Exit Select
                                    End Select
                                ElseIf arr3(q).ToString() = "03" AndAlso Country.ToUpper() = "KE" Then
                                    Select Case Country.ToUpper()
                                        Case "KE"
                                            FileExt = "M"
                                            CurrenctTrxTypeID = "OD"
                                            MandateFT = "03"
                                            Exit Select
                                    End Select
                                ElseIf arr3(q).ToString() = "04" AndAlso Country.ToUpper() = "KE" Then
                                    Select Case Country.ToUpper()
                                        Case "KE"
                                            FileExt = "M"
                                            CurrenctTrxTypeID = "OD"
                                            MandateFT = "04"
                                            Exit Select
                                    End Select
                                ElseIf arr3(q).ToString() = "05" AndAlso Country.ToUpper() = "KE" Then
                                    Select Case Country.ToUpper()
                                        Case "KE"
                                            FileExt = "M"
                                            CurrenctTrxTypeID = "OD"
                                            MandateFT = "05"
                                            Exit Select
                                    End Select
                                ElseIf arr3(q).ToString() = "06" AndAlso Country.ToUpper() = "KE" Then
                                    Select Case Country.ToUpper()
                                        Case "KE"
                                            FileExt = "M"
                                            CurrenctTrxTypeID = "OD"
                                            MandateFT = "06"
                                            Exit Select
                                    End Select
                                ElseIf arr3(q).ToString() = "99" AndAlso Country.ToUpper() = "KE" Then
                                    Select Case Country.ToUpper()
                                        Case "KE"
                                            FileExt = "M"
                                            CurrenctTrxTypeID = "OD"
                                            MandateFT = "99"
                                            Exit Select
                                    End Select
                                End If

                                Dim drGeneratedRows As DataRow() = dsClearing.Tables(0).[Select]("ToBank  " + ToBank + "")
                                If drGeneratedRows.Length = 0 Then
                                    Continue For
                                End If
                                For Each dvr As DataRow In drGeneratedRows
                                    dsTrxClearingBankWise.Tables(0).ImportRow(dvr)
                                Next
                                dsTrxClearingBankWise.AcceptChanges()

                                If dsTrxClearingBankWise.Tables(0).Columns.Contains("Generated") = False Then
                                    dsTrxClearingBankWise.Tables(0).Columns.Add("Generated", GetType(Boolean))
                                End If
                                For Each row As DataRow In dsTrxClearingBankWise.Tables(0).Rows
                                    row("Generated") = 0
                                Next
                                dsTrxClearingBankWise.AcceptChanges()
                                'Dim ds As DataSet = New DataSet
                                'ds.Tables.Add(dsImages)
                                Dim dsDDImagex As New BRDataSet


                                'dsDDImagex.

                                'dsDDImagex = dsImages.Clone
                                strCon(0) = DatabaseName
                                strCon(1) = DBPassword
                                strCon(2) = DBServerName
                                GetConnectionSQL()

                                'FinalData = OutClearingFile.GenerateClearingFiles("KES", arr3(q).ToString(), ToClearingBank, dsTrxClearingBankWise, dsClearingFileFormat, dsDDImagex, Modscan.cToDate, usrInfo, ToBank, "", GetConnectionSQL(), strCon, MandateFT)
                                If FinalData.Rows.Count = 0 Then
                                    Continue For
                                End If

                                If FileExt = "M" AndAlso (MandateFT = "01" OrElse MandateFT = "03") Then

                                    'Get Images
                                    Dim dsDDImages As New DataTable()
                                    ExecuteData(GetModify("p_GetDDImages"), dsDDImages, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
                                    Try
                                        'Generate
                                        fileName = sPath + "\" + ToClearingBank.ToString().Trim() + ddmmmyyyy + MandateFT + "00" + "." + FileExt + usrInfo.strBank
                                        WriteDDKenya(FinalData, fileName, dsDDImages)
                                        drIsTrxGenerated = FinalData.[Select]("ISNULL(ImageID,0)<>'0'")
                                        For Each dvr As DataRow In drIsTrxGenerated
                                            If (MandateFT = "01" Or MandateFT = "03") Then
                                                ExecuteData(GetModify("p_UpdateDDStatus", "DDID", dvr("TrxRowID"), "FileType", MandateFT, "OurBranchID", dvr("OurBranchID"), "TData", dvr("Text")), dsClearingThroughThisBank, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
                                            Else
                                                ExecuteData(GetModify("p_UpdateDDStatus", "DDID", dvr("TrxRowID"), "FileType", MandateFT, "OurBranchID", dvr("OurBranchID"), "TData", dvr("Text")), dsClearingThroughThisBank, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
                                            End If
                                        Next
                                    Catch ex As Exception
                                    End Try
                                ElseIf FileExt = "M" AndAlso (MandateFT <> "01" OrElse MandateFT <> "03") Then
                                    Try
                                        fileName = sPath + "\"
                                        If System.IO.Directory.Exists(fileName) = False Then
                                            System.IO.Directory.CreateDirectory(fileName)
                                            'System.Windows.Forms.MessageBox.Show("Imeingia hapa")
                                        End If
                                        'System.Windows.Forms.MessageBox.Show(fileName)
                                        fileName = fileName + ToClearingBank.ToString().Trim() + ddmmmyyyy + MandateFT + "00" + "." + FileExt + usrInfo.strBank
                                        Dim x As Integer
                                        Dim sw As StreamWriter = Nothing
                                        sw = New StreamWriter(fileName, False)
                                        For Each row As DataRow In FinalData.Rows
                                            Dim array As Object() = row.ItemArray
                                            For x = 0 To array.Length - 2
                                                If x = 0 Then
                                                ElseIf x = 2 Then
                                                Else
                                                    sw.Write(array(x).ToString())
                                                    sw.WriteLine()
                                                End If

                                            Next
                                        Next
                                        sw.Close()
                                    Catch ex As Exception
                                        System.Windows.Forms.MessageBox.Show(fileName + ":" + ex.Message)
                                    End Try

                                    drIsTrxGenerated = FinalData.[Select]("ISNULL(ImageID,0)<>'0'")
                                    For Each dvr As DataRow In drIsTrxGenerated
                                        If (MandateFT = "01" Or MandateFT = "03") Then
                                            ExecuteData(GetModify("p_UpdateDDStatus", "DDID", dvr("TrxRowID"), "FileType", MandateFT, "OurBranchID", dvr("OurBranchID"), "TData", dvr("Text")), dsClearingThroughThisBank, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
                                        Else
                                            ExecuteData(GetModify("p_UpdateDDStatus", "DDID", dvr("TrxRowID"), "FileType", MandateFT, "OurBranchID", dvr("OurBranchID"), "TData", dvr("Text")), dsClearingThroughThisBank, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
                                        End If
                                    Next
                                End If

                            Next
                        End If
                    End If

                Next
            Case "ET"

            Case "SA"

        End Select
        Me.Close()
    End Sub

    Private Shared Function OutClearingData(usrInfo As UserInfo, brModule As BRModule, ByRef dstrxClearing As DS_trxClearing, ParamArray ValuesParamarray As Object()) As Boolean
        Dim auditInfo As New AuditInfo()
        auditInfo.ModuleID = brModule
        auditInfo.EventID = BROperation.View
        auditInfo.Status = BRStatus.Successful
        Dim IsFirstSession As Boolean = False
        dstrxClearing = New DS_trxClearing()
        If ValuesParamarray(3).ToString() = "1" Then
            IsFirstSession = True
        Else
            IsFirstSession = False
        End If
        Try
            Using connection As IDbConnection = GetConnection()
                Dim Country As String = CountryCode
                If Country.ToUpper() = "KE" Then
                    Dim intfDBHelper As IDBHelper = DBClient.GetDBHelper(usrInfo)
                    Dim arParms As IDataParameter() = intfDBHelper.CreateDBParamsArray(2)
                    arParms(0) = intfDBHelper.CreateNewDBParam("FromDate", SqlDbType.DateTime)
                    arParms(0).Value = ValuesParamarray(0)
                    arParms(1) = intfDBHelper.CreateNewDBParam("ToDate", SqlDbType.DateTime)
                    arParms(1).Value = ValuesParamarray(1)
                    intfDBHelper.FillDataset(connection, CommandType.StoredProcedure, "p_OutClearingDDMandateKE", dstrxClearing, New String() {"dt_trxClearing"}, arParms)
                End If
                Dim strNewInfo As String = GetXmlTables(dstrxClearing)
                Using trans As IDbTransaction = connection.BeginTransaction()
                End Using
            End Using
        Catch ex As Exception
            auditInfo.Status = BRStatus.Failed
            auditInfo.Message = ex.Message
            Dim AppendErrorMessage As String = "Error Message OutClearingData:" + ex.ToString() + Environment.NewLine + "Date" + ":" + DateTime.Now + auditInfo.ToString() + Environment.NewLine + "--------------------------" + Environment.NewLine
            System.IO.File.AppendAllText("C:\ClearingFiles\" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + ".txt", AppendErrorMessage)
        End Try
        If dstrxClearing.t_trxClearing.Rows.Count = 0 Then
            Return False
        End If
        Return True
    End Function

    Private Shared Function WriteDDKenya(dtDDTextContent As DataTable, FileName As String, dsDDImages As DataTable) As Boolean
        Dim DDImageSize As Byte() = Nothing
        Dim DDImage As Byte() = Nothing
        Dim DDLineContent As String = String.Empty
        Dim FilePath As String = String.Empty

        Dim dsCopy4Images As New BRDataSet()
        dsCopy4Images.Tables.Add("Images")
        For Each col As DataColumn In dsDDImages.Columns
            dsCopy4Images.Tables(0).Columns.Add(col.ColumnName)
        Next
        FilePath = FileName
        Dim EJfoundRows As DataRow() = Nothing
        Dim CheckIfItsData As Boolean = False

        For x As Int32 = 0 To dtDDTextContent.Rows.Count - 1
            CheckIfItsData = False
            DDLineContent = dtDDTextContent.Rows(x)("text").ToString()
            Select Case DDLineContent.Substring(0, 2).ToString()
                Case "18", "19"
                    OtherContents(FilePath, DDLineContent)
                    Exit Select
                Case Else
                    'Images
                    Dim ImageID As String = dtDDTextContent.Rows(x)("TrxRowID").ToString()
                    Dim OurBranchID As String = dtDDTextContent.Rows(x)("OurBranchID").ToString()
                    EJfoundRows = dsDDImages.[Select]((Convert.ToString("ImageID = '") & ImageID) + "' AND OurBranchID = '" & OurBranchID & "'")
                    dsCopy4Images.Clear()
                    If EJfoundRows.Length > 0 Then
                        CheckIfItsData = True
                        For i As Int32 = 0 To EJfoundRows.Length - 1
                            dsCopy4Images.Tables(0).ImportRow(EJfoundRows(i))
                        Next
                        dsCopy4Images.AcceptChanges()
                        Dim Str As String() = New String(0) {}


                        'For i As Int32 = 0 To dsCopy4Images.Tables(0).Rows.Count - 1
                        '    Dim ms As MemoryStream = Nothing
                        '    Dim imgAbsolutePath As String = "C:\Images\" + Convert.ToString(Guid.NewGuid().ToString("N")) + ".tif"
                        '    ms = New MemoryStream(DirectCast(String2Bytes(DirectCast(dsCopy4Images.Tables(0).Rows(0)("DDImage"), String)), Byte()))
                        '    Dim bmpStoredImage As System.Drawing.Bitmap = New Bitmap(ms)
                        '    Dim rgbBitmap As Bitmap = GrayScale(bmpStoredImage)
                        '    dsCopy4Images.Tables(0).Rows(i)("DDImage") = Bytes2String(ImageToBytes(rgbBitmap))
                        '    dsCopy4Images.Tables(0).Rows(i)("DDImage") = DirectCast(String2Bytes(dsCopy4Images.Tables(0).Rows(0)("DDImage")), Byte())
                        '    dsCopy4Images.AcceptChanges()
                        'Next


                        DDImage = DirectCast(String2Bytes(DirectCast(dsCopy4Images.Tables(0).Rows(0)("DDImage"), String)), Byte())
                        If BitConverter.IsLittleEndian = True Then
                            DDImageSize = BitConverter.GetBytes(DDImage.Length)
                        End If
                        DDWriter(DDImageSize, DDImage, FilePath, DDLineContent, CheckIfItsData)
                    End If
                    dsCopy4Images.Clear()
                    Exit Select
            End Select
        Next
        Return True
    End Function
    Private Shared Function GrayScale(Bmp As Bitmap) As Bitmap
        Dim rgb As Integer
        Dim c As Color

        For y As Integer = 0 To Bmp.Height - 1
            For x As Integer = 0 To Bmp.Width - 1
                c = Bmp.GetPixel(x, y)
                rgb = CInt((c.R + c.G + c.B) / 3)
                Bmp.SetPixel(x, y, Color.FromArgb(rgb, rgb, rgb))
            Next
        Next
        Return Bmp
    End Function
    Private Shared Function OtherContents(FilePath As String, EJOtherContent As String) As Boolean
        Dim myEJContentStreamWriter As StreamWriter = Nothing
        Try
            myEJContentStreamWriter = New StreamWriter(FilePath, True)
            myEJContentStreamWriter.WriteLine(EJOtherContent)
        Finally
            If (myEJContentStreamWriter IsNot Nothing) Then
                myEJContentStreamWriter.Close()
            End If
        End Try
        Return True
    End Function
    Private Shared Function ImageToBytes(TheImage As Image) As Byte()
        Dim ms As New MemoryStream()
        Try
            Dim imgFile As String = String.Empty
            imgFile = String.Empty
            Dim imgAbsolutePath As String = String.Empty
            ms = New MemoryStream()
            TheImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg)
            imgFile = "../Download/" + Guid.NewGuid().ToString() + ".jpg"
            imgAbsolutePath = System.Web.HttpContext.Current.Request.MapPath(imgFile)

            DirectCast(System.Drawing.Image.FromStream(ms), System.Drawing.Bitmap).Save(imgAbsolutePath, System.Drawing.Imaging.ImageFormat.Jpeg)

        Catch x As Exception
        End Try
        Return ms.ToArray()
    End Function
    Private Shared Function DDWriter(DDImageSize As Byte(), DDImage As Byte(), FilePath As String, DDLineContent As String, CheckIfItsData As Boolean) As Boolean
        Dim myFileStream As FileStream = Nothing
        Dim myDDContentStreamWriter As StreamWriter = Nothing
        If DDLineContent.ToString().Trim() = "" Then
            myDDContentStreamWriter = New StreamWriter(FilePath, True)
            If (myDDContentStreamWriter IsNot Nothing) Then
                myDDContentStreamWriter.Close()
            End If
            System.IO.File.Delete(FilePath)
            Return False
        End If
        Try
            myDDContentStreamWriter = New StreamWriter(FilePath, True)
            myDDContentStreamWriter.Write(DDLineContent)
        Finally
            If (myDDContentStreamWriter IsNot Nothing) Then
                myDDContentStreamWriter.Close()
            End If
        End Try
        Try
            myFileStream = New FileStream(FilePath, FileMode.Append)
            myFileStream.Write(DDImageSize, 0, DDImageSize.Length)
            myFileStream.Write(DDImage, 0, DDImage.Length)
        Finally
            If (myFileStream IsNot Nothing) Then
                myFileStream.Close()
            End If
        End Try
        If CheckIfItsData = True Then
            myDDContentStreamWriter = Nothing
            Try
                myDDContentStreamWriter = New StreamWriter(FilePath, True)
                myDDContentStreamWriter.WriteLine()
            Finally
                If (myDDContentStreamWriter IsNot Nothing) Then
                    myDDContentStreamWriter.Close()
                End If
            End Try
        End If
        Return True
    End Function
    Private Shared Function DeSerializeXmlBinary(bytes As Byte()) As Object
        Using rdr As XmlDictionaryReader = XmlDictionaryReader.CreateBinaryReader(bytes, XmlDictionaryReaderQuotas.Max)
            Dim serializer As NetDataContractSerializer
            serializer = New NetDataContractSerializer()
            serializer.AssemblyFormat = FormatterAssemblyStyle.Simple
            Return serializer.ReadObject(rdr)
        End Using
    End Function
    Private Shared Function DecompressData(inb As Byte()) As Object
        Dim outb As Byte()
        'Using istream As New MemoryStream(inb)
        '    Using ostream As New MemoryStream()
        '        Using sr As New System.IO.Compression.DeflateStream(istream, System.IO.Compression.CompressionMode.Decompress)
        '            sr.CopyTo(ostream)
        '        End Using
        '        outb = ostream.ToArray()
        '    End Using
        'End Using
        Return DeSerializeXmlBinary(outb)
    End Function

End Class