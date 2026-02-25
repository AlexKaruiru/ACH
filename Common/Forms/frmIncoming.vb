Imports BRClearing.Common.Modscan
Imports BRClearingEncryptDecrypt
Imports BRClearing.Common.BRTZClass
Imports System.IO
Imports System.Configuration
Imports BRClearing.Util
Imports System.Collections.Specialized

Public Class frmIncoming

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnRead_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRead.Click
        Dim path As String = String.Empty
        Dim FileBankID As String = String.Empty
        Dim OurFileBankID As String = String.Empty
        Dim FileType As String = String.Empty
        Dim Counter As Double = 0
        Dim FileDate As String = String.Empty
        Dim EncDecr As New BRClearingEncryptDecrypt.EncDec
        Dim toDay As String = String.Empty
        Dim thisMonth As String = String.Empty
        Dim thisYear As String = String.Empty
        Dim d As DateTime
        d = Convert.ToDateTime(cWorkingDate)
        toDay = d.Day
        thisMonth = d.Month
        thisYear = d.Year
        Dim WorkDy As String = New String("0", 2 - toDay.Length) & toDay & New String("0", 2 - thisMonth.Length) & thisMonth & New String("0", 4 - thisYear.Length) & thisYear

        Try
            'prgIncomingImages.Minimum = 0
            'Counter = (FLItems.Items.Count - 1) / 100
            For x As Int32 = 0 To FLItems.Items.Count - 1
                System.Windows.Forms.Application.DoEvents()
                If FLItems.Items(x).Length < 18 Then


                Else
                    '23265271220130100.J70
                    If System.Configuration.ConfigurationManager.AppSettings("BankID") = "70" Then 'Kingdom Sacco
                        FileBankID = FLItems.Items(x).Substring(19, 2)
                        OurFileBankID = FLItems.Items(x).Substring(0, 5)
                        Me.Text = "Bank " & FileBankID & ": Reading Inwards Images, Please wait ...."
                        lblBankID.Text = FileBankID
                        FileBankID = New String("0", 2 - FLItems.Items(x).Substring(19, 2).Length) & FLItems.Items(x).Substring(19, 2)
                        path = FLItems.Path & "\" & FLItems.Items(x).ToString
                        FileType = FLItems.Items(x).Substring(18, 1)
                        FileDate = FLItems.Items(x).Substring(5, 8)
                        isFcy = False
                        If FileType = "J" Then
                            If OurFileBankID <> "23265" Then
                                MsgBox("This EJ file " & FLItems.Items(x) & " is not a correct incoming file.")
                                Continue For
                            End If
                            'ElseIf FileDate <> WorkDy Then
                            '    MsgBox("There are no Inwards files for today's working date")
                            '    Exit Sub
                        End If
                        If (FileType = "J" Or FileType = "E") Then
                            If FileType = "J" Then
                                isFcy = False
                            Else
                                isFcy = True
                            End If

                            Modscan.ReadImagesFromFile(path, "0", FileBankID, FileProgress, isFcy)
                        Else

                        End If
                    Else
                        If OurBankID = "00" Then
                            FileBankID = FLItems.Items(x).Substring(16, 2)
                            OurFileBankID = FLItems.Items(x).Substring(0, 2)
                            Me.Text = "Bank " & FileBankID & ": Reading Inwards Images, Please wait ...."
                            lblBankID.Text = FileBankID
                            FileBankID = New String("0", 2 - FLItems.Items(x).Substring(16, 2).Length) & FLItems.Items(x).Substring(16, 2)
                            path = FLItems.Path & "\" & FLItems.Items(x).ToString
                            FileType = FLItems.Items(x).Substring(15, 1)
                            FileDate = FLItems.Items(x).Substring(2, 8)
                            isFcy = False
                            If FileType = "J" And OurFileBankID <> System.Configuration.ConfigurationManager.AppSettings("BankID") Then
                                'If OurFileBankID <> OurBankID Then
                                MsgBox("This EJ file " & FLItems.Items(x) & " is not a correct incoming file.")
                                Continue For
                            ElseIf FileDate <> WorkDy Then
                                MsgBox("There are no Inwards files for today's working date")
                                Exit Sub
                            End If
                            If (FileType = "J" Or FileType = "E") Then
                                If FileType = "J" Then
                                    isFcy = False
                                Else
                                    isFcy = True
                                End If

                                Modscan.ReadImagesFromFile(path, "0", FileBankID, FileProgress, isFcy)
                            Else
                                If System.Configuration.ConfigurationManager.AppSettings("sysEnc") = 1 Then
                                    Try
                                        If FileType = "T" And OurFileBankID <> "00" Then
                                            MsgBox("This EFT file " & FLItems.Items(x) & " is not a correct incoming file.")
                                            Continue For
                                        Else
                                            EncDecr.BRClearingEnc(path, path, "D9-49-A5-E6-AE-07-04-51-08-AE-35-78-7A-B8-90-0A-8A-25-86-A8", Action.Decrypt)
                                        End If
                                    Catch ex As Exception

                                    End Try
                                End If
                            End If
                        Else
                            FileBankID = FLItems.Items(x).Substring(16, 2)
                            OurFileBankID = FLItems.Items(x).Substring(0, 2)
                            Me.Text = "Bank " & FileBankID & ": Reading Inwards Images, Please wait ...."
                            lblBankID.Text = FLItems.Items(x).ToString
                            FileBankID = New String("0", 2 - FLItems.Items(x).Substring(16, 2).Length) & FLItems.Items(x).Substring(16, 2)
                            path = FLItems.Path & "\" & FLItems.Items(x).ToString
                            FileType = FLItems.Items(x).Substring(15, 1)
                            FileDate = FLItems.Items(x).Substring(2, 8)
                            isFcy = False
                            If FileType = "J" Then
                                If OurFileBankID <> OurBankID Then
                                    MsgBox("This EJ file " & FLItems.Items(x) & " is not a correct incoming file.")
                                    Continue For
                                End If
                            ElseIf FileDate <> WorkDy Then
                                MsgBox("There are no Inwards files for today's working date")
                                Exit Sub
                            End If
                            If (FileType = "J" Or FileType = "E") Then
                                If FileType = "J" Then
                                    isFcy = False
                                Else
                                    isFcy = True
                                End If

                                Modscan.ReadImagesFromFile(path, "0", FileBankID, FileProgress, isFcy)
                            Else
                                ProcessDiretDebitMandates()
                                Exit For
                                'Try
                                '    If FileType = "T" Then
                                '        MsgBox("This EFT file " & FLItems.Items(x) & " is not a correct incoming file.")
                                '        Continue For
                                '    End If
                                'Catch ex As Exception

                                'End Try
                            End If
                        End If
                    End If
                End If
            Next
            System.Windows.Forms.Application.DoEvents()
        Catch ex As Exception

        End Try

    End Sub
    Private Sub ProcessDiretDebitMandates()
        Try
            Dim Country As String = CountryCode
            Dim SelectedStringCol As New StringCollection()
            Dim DDsDt As DataTable = New DataTable()
            Dim FileDate As String = String.Empty
            FileDate = Convert.ToString([String].Format("{0:u}", Modscan.WORKING_DATE))
            Dim dd As String = New String("0"c, 2 - Convert.ToString(Modscan.WORKING_DATE.Day).ToString().Length) & Convert.ToString(Modscan.WORKING_DATE.Day)
            Dim mm As String = New String("0"c, 2 - Convert.ToString(Modscan.WORKING_DATE.Month).Length) & Convert.ToString(Modscan.WORKING_DATE.Month)
            Dim yyyy As String = Convert.ToString(Modscan.WORKING_DATE.Year)
            Dim ddmmmyyyy As String = Convert.ToString(dd & mm) & yyyy
            If Country.ToUpper() = "KE" Then
                Dim EJfilePaths As String() = Nothing
                EJfilePaths = Directory.GetFiles("" + FilePath + "")
                EJfilePaths = Directory.GetFiles(FilePath)
                For f As Int32 = 0 To EJfilePaths.Length - 1
                    If EJfilePaths(f).Substring(EJfilePaths(f).ToString().LastIndexOf(".") + 1).Substring(0, 1).ToUpper() = "M" Then
                        SelectedStringCol.Add(EJfilePaths(f).ToString())
                    End If
                Next
                Dim isLegacyData As Boolean = ConfigurationManager.AppSettings("isLegacyDDs")
                For p As Int32 = 0 To SelectedStringCol.Count - 1

                    DDsDt.Merge(ImportClearingFiles.DDFiles(SelectedStringCol(p).ToString(), FileDate, isLegacyData))
                Next
                SaveDDMandate(DDsDt, isLegacyData)
            End If

        Catch ex As Exception

        End Try
    End Sub
    Private Sub DLDrivePath_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DLDrivePath.SelectedIndexChanged
        DLDirPath.Path = DLDrivePath.Drive
    End Sub

    Private Sub DLDirPath_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles DLDirPath.DoubleClick
        FLItems.Path = DLDirPath.Path

        FLItems.SelectionMode = Windows.Forms.SelectionMode.MultiSimple
        For x As Int32 = 0 To FLItems.Items.Count - 1
            'FLItems.SelectedIndex = x
        Next
        FLItems.Enabled = False
    End Sub

    Private Sub frmIncoming_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            Select Case CountryCode.ToUpper.Trim
                Case "UG"

                Case "SL"

                Case "TZ"
                    Try
                        If Not Directory.Exists(Modscan.FilePath) Then Directory.CreateDirectory(Modscan.FilePath)
                        DLDirPath.Path = Modscan.FilePath
                        DLDirPath_DoubleClick(sender, e)
                        Dim Flist As List(Of String) = New List(Of String)
                        Dim FName As String = ""
                        For x As Int32 = 0 To FLItems.Items.Count - 1
                            Dim sExt As String = Path.GetExtension(FLItems.Items(x).ToString).ToLower()
                            Dim strNewFileName As String = ""
                            Select Case sExt.ToUpper
                                Case ".ZIP"
                                    strNewFileName = FLItems.Items(x).ToString
                                    FName = Path.ChangeExtension(Modscan.FilePath & "\" & strNewFileName, "cms.signed.CMS")
                                    Rename(Modscan.FilePath & "\" & strNewFileName, FName)
                                    Flist.Add(FName.ToString)
                                Case ".CMS"
                                    strNewFileName = Modscan.FilePath & "\" & FLItems.Items(x).ToString
                                    Flist.Add(strNewFileName.ToString)
                                Case ".CHR", ".Q", ".V", ".S", ".N", ".TXT", ".RC", ".R"
                                    strNewFileName = Modscan.FilePath & "\" & FLItems.Items(x).ToString
                                    Flist.Add(strNewFileName.ToString)
                            End Select
                        Next
                        'For x As Int32 = 0 To FLItems.Items.Count - 1
                        '    Dim sExt As String = Path.GetExtension(FLItems.Items(x).ToString).ToLower()
                        '    Select Case sExt.ToUpper
                        '        Case ".ZIP", ".CMS"
                        'TZ.Inwards.ReadFiles(Flist, lblBankID, prgIncomingImages, Prg, TZ.FileType.Cheques)
                        '        Case ".Q"
                        'Inwards.ReadFiles(Flist, lblBankID, prgIncomingImages, Prg, Inwards.FileType.ChequeRejects)
                        '        Case ".RC", ".V", ".R", ".C"
                        'ResponsesFromACH(Modscan.FilePath)
                        '        Case ".S"
                        'Inwards.ReadFiles(Flist, lblBankID, prgIncomingImages, Prg, FileType.Efts)
                        ''Inwards.ReadFiles(Flist, lblBankID, prgIncomingImages, Prg, FileType.EftReturn)
                        '        Case ".CHR", ".N", ".TXT"

                        '    End Select
                        'Next
                    Catch ex As Exception
                        System.Windows.Forms.MessageBox.Show("Error registerd, check error log")
                        Modscan.ErrorLog(ex.Message, "- Incoming Files")
                    End Try

                Case "RD"

                Case "KE"
                    DLDirPath.Path = ConfigurationManager.AppSettings("IncomingFiles")
                    DLDirPath_DoubleClick(sender, e)
                    btnRead_Click(sender, e)
                Case "ET"

                Case "SA"

            End Select
        Catch ex As Exception

        End Try
        Me.Close()
    End Sub
End Class