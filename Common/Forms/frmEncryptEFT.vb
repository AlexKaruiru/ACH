Imports BrClearing.Common.Modscan
Imports BRClearingEncryptDecrypt
Imports System.IO
Imports System.Configuration

Public Class frmEncryptEFT
    Private Sub frmEncryptEFT_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Modscan.FilePath = ConfigurationManager.AppSettings("OutgoingFiles")
        Dim File As New FileInfo(Modscan.FilePath + "\")
        Dim fileName As String = ""
        Dim NewFileName As String = ""
        Dim EncDecr As New BRClearingEncryptDecrypt.EncDec
        Dim strSourceNewPath As String = File.DirectoryName
        Try
            Wait(5)
            'If System.IO.Directory.Exists(strSourceNewPath) Then
            '    Dim files As String() = System.IO.Directory.GetFiles(strSourceNewPath)
            '    For Each s As String In files
            '        fileName = System.IO.Path.GetFileName(s)
            '        NewFileName = System.IO.Path.Combine(strSourceNewPath, fileName)
            '        EncDecr.BRClearingEnc(NewFileName, NewFileName, "D9-49-A5-E6-AE-07-04-51-08-AE-35-78-7A-B8-90-0A-8A-25-86-A8", Action.Encrpt)
            '        System.IO.File.Delete(NewFileName)
            '    Next
            'End If
            Me.Close()
        Catch ex As Exception

        End Try

    End Sub
End Class