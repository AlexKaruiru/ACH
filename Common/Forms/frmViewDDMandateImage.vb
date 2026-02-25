Imports System.Windows.Forms
Imports System.Drawing
Imports System.IO
Imports System.Drawing.Printing

Public Class frmViewDDMandateImage
    Private streamToPrint As StreamReader
    Private printFont As Font
    Private Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click
        Try
            Dim ImageStr As String = ""
            ImageStr = Modscan.Bytes2String(Modscan.ImageToByte(PicDDImage.Image))

            Dim Command As System.Data.SqlClient.SqlCommand
            Dim SqlConn As System.Data.SqlClient.SqlConnection

            SqlConn = Modscan.GetConnectionSQL()
            If (SqlConn.State <> ConnectionState.Open) Then
                SqlConn.Open()
            End If
            Command = New System.Data.SqlClient.SqlCommand("sp_AddDDImage", SqlConn)
            Command.CommandType = CommandType.StoredProcedure
            Command.Parameters.Add("@DDID", SqlDbType.NVarChar).Value = Modscan.cTransactionID
            Command.Parameters.Add("@trxDate", SqlDbType.NVarChar).Value = Format(Modscan.WORKING_DATE, "dd/MMM/yyyy")
            Command.Parameters.Add("@OurBranchID", SqlDbType.NVarChar).Value = Modscan.OurBranchID
            Command.Parameters.Add("@TrxType", SqlDbType.NVarChar).Value = Modscan.cTrxType
            Command.Parameters.Add("@DDImage", SqlDbType.NVarChar).Value = ImageStr
            Command.ExecuteNonQuery()
            Me.Close()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub cmdExit_Click(sender As Object, e As EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        Try

            Dim fd As OpenFileDialog = New OpenFileDialog()
            Dim strFileName As String

            fd.Title = "Open File Dialog"
            fd.InitialDirectory = "C:\"
            fd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;"
            fd.FilterIndex = 2
            fd.RestoreDirectory = True

            If fd.ShowDialog() = DialogResult.OK Then
                strFileName = fd.FileName
                txtFolderName.Text = strFileName
                PicDDImage.Image = Modscan.FromFile(strFileName)
                PicDDImage.StretchImageToFit = True
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub frmViewDDMandateImage_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Dim ImageStr As String = ""
            'ImageStr = Modscan.Bytes2String(Modscan.ImageToByte(PicDDImage.Image))
            Modscan.ExecuteData(Modscan.GetModify("sp_ViewDDImages", "DDID", Modscan.cTransactionID, "OurBranchID", Modscan.OurBranchID), Modscan.publicDTbl, Modscan.dataExecTypes.ExecTypeQuery, Modscan.queryType.StoredProcedure)
            If Modscan.publicDTbl.Rows.Count > 0 Then
                PicDDImage.Image = IIf(IsDBNull(Modscan.publicDTbl.Rows(0)("DDImage")), Nothing, Modscan.GetImages(Modscan.String2Bytes(Modscan.publicDTbl.Rows(0)("DDImage"))))
                'PicDDImage.StretchImageToFit = True

            End If

        Catch ex As Exception

        End Try
    End Sub
    Private Sub prnDoc_PrintPage(ByVal sender As System.Object, ByVal ev As System.Drawing.Printing.PrintPageEventArgs) Handles prnDoc.PrintPage
        ev.Graphics.DrawImage(PicDDImage.Image, 10, 10, 830, 1400)




    End Sub

    Private Sub cmdPrint_Click(sender As Object, e As EventArgs) Handles cmdPrint.Click
        'we need to be able to send the image to printer
        Try
            Dim pd As New Drawing.Printing.PrintDocument
            pd.PrinterSettings.PrintFileName = "x"
            pd.DocumentName = "Direct Debit Mandate"
            prnDoc.Print()
        Catch ex As Exception
        End Try
        'Try
        '    Dim img As Image = PicDDImage.Image
        '    Dim imgStream As MemoryStream = New MemoryStream()
        '    img.Save(imgStream, System.Drawing.Imaging.ImageFormat.Jpeg)


        '    streamToPrint = New StreamReader(imgStream)
        '    Try
        '        printFont = New Font("Arial", 10)
        '        Dim pd As New PrintDocument()
        '        pd.PrinterSettings.PrintFileName = "x"
        '        pd.DocumentName = "Direct Debit Mandate"
        '        prnDoc.Print()
        '    Finally
        '        streamToPrint.Close()
        '        imgStream.Close()
        '    End Try
        'Catch ex As Exception
        '    MessageBox.Show(ex.Message)
        'End Try

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        PicDDImage.ZoomIn()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        PicDDImage.ZoomOut()
    End Sub
End Class