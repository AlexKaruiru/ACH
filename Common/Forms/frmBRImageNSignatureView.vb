Imports BrClearing.Common.Modscan

Public Class frmBRImageNSignatureView
    Public Sub New()
        InitializeComponent()
    End Sub
    Private Counter As Int16 = 0
    Private Sub BRImageNSignatureView_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        BRDbType = systemDbTypes.dbTypeSql
        Try
            GetDbConnectionStrings()
            If cTrxType = "O" Or cTrxType = "I" Then
                ExecuteData(GetModify("sp_GetChequeImage", "TrxType", cTrxType, "ColumnID", Convert.ToInt64(cTransactionID)), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
                If publicDTbl.Rows.Count > 0 Then
                    pictMainFront.Image = IIf(IsDBNull(publicDTbl.Rows(0)("JFImage")), Nothing, GetImages(StringToByte(publicDTbl.Rows(0)("JFImage"))))
                    pictMainFront.StretchImageToFit = True
                    pictFrontBW.Image = IIf(IsDBNull(publicDTbl.Rows(0)("TFImage")), Nothing, GetImages(StringToByte(publicDTbl.Rows(0)("TFImage"))))
                    pictFrontBW.Image.PhysicalDimension.ToSize()
                    pictFrontGrayScale.Image = IIf(IsDBNull(publicDTbl.Rows(0)("JFImage")), Nothing, GetImages(StringToByte(publicDTbl.Rows(0)("JFImage"))))
                    pictFrontGrayScale.Image.PhysicalDimension.ToSize()
                    pictFrontRear.Image = IIf(IsDBNull(publicDTbl.Rows(0)("JRImage")), Nothing, GetImages(StringToByte(publicDTbl.Rows(0)("JRImage"))))
                    pictFrontRear.Image.PhysicalDimension.ToSize()
                End If
                publicDTbl.Clear()
                If cTrxType <> "O" Then
                    ExecuteData(GetModify("sp_ClearingImageSignature", "OurBranchID", OurBranchID, "AccountID", strAccountID), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
                    If publicDTbl.Rows.Count > 0 Then
                        If IsDBNull(publicDTbl.Rows(0)("Signature")) = False Then
                            pictMainFront3.Image = IIf(IsDBNull(publicDTbl.Rows(0)("Signature")), Nothing, GetImages(publicDTbl.Rows(0)("Signature")))
                        End If
                        lblMandate.Text = IIf(IsDBNull(publicDTbl.Rows(0)("Mandate")), "", publicDTbl.Rows(0)("Mandate"))
                        pictMainFront3.StretchImageToFit = True
                    End If
                    'publicDTbl.Clear()
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, My.Application.Info.ProductName)
        End Try
    End Sub

    Private Sub pictMainFront_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles pictMainFront.Click
        pictMainFront3.StretchImageToFit = True
    End Sub

    Private Sub pictMainFront_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pictMainFront.MouseWheel
        pictMainFront.StretchImageToFit = False
    End Sub

    'Private Sub PictSignature_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
    '    PictSignature.StretchImageToFit = False
    'End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub pictMainFront3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles pictMainFront3.Click
        pictMainFront.StretchImageToFit = True
    End Sub

    Private Sub pictMainFront3_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pictMainFront3.MouseClick
        pictMainFront.StretchImageToFit = True
    End Sub

    Private Sub pictMainFront3_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pictMainFront3.MouseWheel
        pictMainFront3.StretchImageToFit = False
    End Sub

    Private Sub pictFrontBW_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pictFrontBW.Click
        pictMainFront.Image = pictFrontBW.Image
        pictMainFront.StretchImageToFit = True
    End Sub

    Private Sub pictFrontGrayScale_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pictFrontGrayScale.Click
        pictMainFront.Image = pictFrontGrayScale.Image
        pictMainFront.StretchImageToFit = True
    End Sub

    Private Sub pictFrontRear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pictFrontRear.Click
        pictMainFront.Image = pictFrontRear.Image
        pictMainFront.StretchImageToFit = True
    End Sub

    Private Sub cmdZoom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdZoom.Click
        pictMainFront.StretchImageToFit = False
        pictMainFront.ZoomIn()
    End Sub

    Private Sub cmdFitWdth_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFitWdth.Click
        pictMainFront.StretchImageToFit = False
        pictMainFront.ZoomOut()
    End Sub

    Private Sub CmdFitHT_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdFitHT.Click
        pictMainFront.StretchImageToFit = True
    End Sub

    Private Sub btnNxtSign_Click(sender As Object, e As EventArgs) Handles btnNxtSign.Click
        If publicDTbl.Rows.Count > 0 Then

            Counter += 1


            If Counter <= publicDTbl.Rows.Count - 1 Then
                If IsDBNull(publicDTbl.Rows(Counter)("Signature")) = False Then
                    pictMainFront3.Image = IIf(IsDBNull(publicDTbl.Rows(Counter)("Signature")), Nothing, GetImages(publicDTbl.Rows(Counter)("Signature")))
                End If
                lblMandate.Text = IIf(IsDBNull(publicDTbl.Rows(Counter)("Mandate")), "", publicDTbl.Rows(Counter)("Mandate"))
                pictMainFront3.StretchImageToFit = True
            Else
                Counter = 0
                If IsDBNull(publicDTbl.Rows(0)("Signature")) = False Then
                    pictMainFront3.Image = IIf(IsDBNull(publicDTbl.Rows(0)("Signature")), Nothing, GetImages(publicDTbl.Rows(0)("Signature")))
                End If
                lblMandate.Text = IIf(IsDBNull(publicDTbl.Rows(0)("Mandate")), "", publicDTbl.Rows(Counter)("Mandate"))
                pictMainFront3.StretchImageToFit = True
            End If
        End If
    End Sub
End Class