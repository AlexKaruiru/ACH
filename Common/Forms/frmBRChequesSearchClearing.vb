Imports System.Windows.Forms
Imports BrClearing.Common.Modscan

Public Class frmBRChequesSearchClearing
    Private Sub frmBRChequesSearchClearing_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        strAccountID = ""
        cSerialID = ""
        cTrxType = ""
        cFromDate = ""
        cTransactionID = ""
    End Sub
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyBase.Name = "Cheque Truncation - Cheques Search"
        BRDbType = systemDbTypes.dbTypeSql
        If cScan = ENUM_Module_Called.Unpay Then
            txtAccount.Enabled = False
            cmbTrxtype.Visible = False
            Label8.Visible = False
            btnUnpaid.Visible = True
            btnUnpaid.Enabled = False
            btnUnpaid.Text = "Unpay"
            cmdReset.Visible = False
            cTrxType = "I"
            Label12.Visible = True
            txtAccName.Visible = True
            Label5.Visible = False
            txtTheirAccID.Visible = False
            txtAccount.Text = strAccountID
            txtAccName.Text = strAccountName
            boolGetImagesDetails = False
            Me.Text = "UnPay Cheque"
            ModuleIDT = "UnP"
            'Validate the ReturnCode

            txtTheirAccID.Focus()
        ElseIf cScan = ENUM_Module_Called.Represent_Cheque Then
            txtAccount.Enabled = False
            cmbTrxtype.Visible = True
            Label8.Visible = True
            btnUnpaid.Visible = True
            txtAccount.Text = strAccountID
            btnUnpaid.Enabled = False
            btnUnpaid.Text = "Re-Present"
            cmdReset.Visible = False
            cTrxType = "I"
            Me.Text = "Re-Present Cheque"
            boolGetImagesDetails = False
            txtTheirAccID.Focus()
            Label12.Visible = True
            txtAccName.Visible = True
            Label5.Visible = False
            txtAccName.Text = strAccountName
            txtTheirAccID.Visible = False
            ModuleIDT = "Rep"
        Else
            Me.Text = "Search Cheque Item"
        End If
        If cTrxType = "" Then
            cTrxType = "O"
        End If
        Try
            GetDbConnectionStrings()
            If cTransactionID = "" Then
                If cWorkingDate = Nothing Then
                    If cFromDate = Nothing Then
                        If cToDate = Nothing Then
                            dtChequeDate.Value = Today.Date
                            dtTo.Value = Today.Date
                        Else
                            dtChequeDate.Value = cToDate
                            dtTo.Value = cToDate
                        End If
                    Else
                        dtChequeDate.Value = cFromDate
                        dtTo.Value = cFromDate
                    End If
                Else
                    dtChequeDate.Value = cWorkingDate
                    dtTo.Value = cWorkingDate
                End If
            Else
                If cWorkingDate = Nothing Then
                    If cFromDate = Nothing Then
                        If cToDate = Nothing Then
                            dtChequeDate.Value = Today.Date
                            dtTo.Value = Today.Date
                        Else
                            dtChequeDate.Value = cToDate
                            dtTo.Value = cToDate
                        End If
                    Else
                        dtChequeDate.Value = cFromDate
                        dtTo.Value = cFromDate
                    End If
                Else
                    dtChequeDate.Value = cWorkingDate
                    dtTo.Value = cWorkingDate
                End If
                If cTrxType = "O" Then
                    cmbTrxtype.SelectedIndex = 0

                ElseIf cTrxType = "A" Then
                    cmbTrxtype.SelectedIndex = 2
                Else
                    cmbTrxtype.SelectedIndex = 1
                    cTrxType = "I"
                End If
                ExecuteData(GetModify("sp_GetChequeImage", "TrxType", cTrxType, "ColumnID", Convert.ToInt64(cTransactionID), "GetImagesDetails", boolGetImagesDetails), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
                If publicDTbl.Rows.Count > 0 Then
                    txtAccount.Text = publicDTbl.Rows(0)("AccountID") & cNo_Text
                    txtChequeID.Text = publicDTbl.Rows(0)("ChequeID") & cNo_Text
                    txtTheirAccID.Text = publicDTbl.Rows(0)("TheirAccountID") & cNo_Text
                    txtBankID.Text = publicDTbl.Rows(0)("Bankid") & cNo_Text
                    txtBranchID.Text = publicDTbl.Rows(0)("BranchID") & cNo_Text
                    dtChequeDate.Value = Date.Parse(Format(publicDTbl.Rows(0)("date"), "dd MMM yyyy"))
                    dtTo.Value = Date.Parse(Format(publicDTbl.Rows(0)("date"), "dd MMM yyyy"))
                    ImgCheque.Image = IIf(IsDBNull(publicDTbl.Rows(0)("JFImage")), Nothing, GetImages(String2Bytes(publicDTbl.Rows(0)("JFImage"))))
                    ImgIconJ.Image = IIf(IsDBNull(publicDTbl.Rows(0)("JFImage")), Nothing, GetImages(String2Bytes(publicDTbl.Rows(0)("JFImage"))))
                    If IsDBNull(publicDTbl.Rows(0)("UVImage")) Then

                    ElseIf publicDTbl.Rows(0)("UVImage") = "" Then

                    Else
                        ImgIconT.Image = IIf(IsDBNull(publicDTbl.Rows(0)("UVImage")), Nothing, GetImages(String2Bytes(publicDTbl.Rows(0)("UVImage"))))

                    End If
                    ImgIconT.Image = IIf(IsDBNull(publicDTbl.Rows(0)("TFImage")), Nothing, GetImages(String2Bytes(publicDTbl.Rows(0)("TFImage"))))
                    ImgIconJR.Image = IIf(IsDBNull(publicDTbl.Rows(0)("JRImage")), Nothing, GetImages(String2Bytes(publicDTbl.Rows(0)("JRImage"))))

                    ImgCheque.StretchImageToFit = True

                    If (cScan = ENUM_Module_Called.Unpay) Or (cScan = ENUM_Module_Called.Represent_Cheque) Then
                        Dim newCol As System.Data.DataColumn
                        newCol = New System.Data.DataColumn
                        newCol.DataType = System.Type.GetType("System.Boolean")
                        newCol.ColumnName = "Select"
                        publicDTbl.Columns.Add(newCol)
                        PopulateGrid(gfxCheques, publicDTbl, MakeInvisibleColumns(0, 5, 6, 7, 8, 9, 10, 14, 17))
                    Else
                        PopulateGrid(gfxCheques, publicDTbl, MakeInvisibleColumns(0, 5, 6, 7, 8, 9, 10, 14, 17))
                    End If



                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, My.Application.Info.ProductName)
        End Try
    End Sub

    Private Sub cmdPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPrint.Click
        'we need to be able to send the image to printer
        Try
            Dim pd As New Drawing.Printing.PrintDocument
            pd.PrinterSettings.PrintFileName = "x"
            pd.DocumentName = "Name of the Report"
            prnDoc.Print()
        Catch ex As Exception
        End Try

    End Sub
    Private Sub prnDoc_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles prnDoc.PrintPage
        e.Graphics.DrawImage(ImgCheque.Image, 10, 10, 830, 400)
    End Sub

    Private Sub ImgIconT_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ImgIconT.Click
        ImgCheque.StretchImageToFit = False
        ImgCheque.Image = ImgIconT.Image
    End Sub

    Private Sub ImgIconJR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ImgIconJR.Click
        ImgCheque.StretchImageToFit = False
        ImgCheque.Image = ImgIconJR.Image
    End Sub

    Private Sub cmdZoom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdZoom.Click
        ImgCheque.StretchImageToFit = False
        ImgCheque.ZoomIn()
    End Sub

    Private Sub cmdFitWdth_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFitWdth.Click
        ImgCheque.StretchImageToFit = False
        ImgCheque.ZoomOut()
    End Sub

    Private Sub CmdFitHT_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdFitHT.Click
        ImgCheque.StretchImageToFit = True
    End Sub

    Private Sub cmdReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReset.Click
        txtAccount.Text = cNo_Text
        txtBankID.Text = cNo_Text
        txtBranchID.Text = cNo_Text
        txtChequeID.Text = cNo_Text
        txtTheirAccID.Text = cNo_Text
        ImgCheque.Image = Nothing
        ImgIconJ.Image = Nothing
        ImgIconJR.Image = Nothing
        ImgIconT.Image = Nothing
        ImgIconTR.Image = Nothing
        If cWorkingDate = Nothing Then
            If cFromDate = Nothing Then
                If cToDate = Nothing Then
                    dtChequeDate.Value = Today.Date
                    dtTo.Value = Today.Date
                Else
                    dtChequeDate.Value = cToDate
                    dtTo.Value = cToDate
                End If
            Else
                dtChequeDate.Value = cFromDate
                dtTo.Value = cFromDate
            End If
        Else
            dtChequeDate.Value = cWorkingDate
            dtTo.Value = cWorkingDate
        End If
        gfxCheques.Rows.Clear()
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        Dim UnpayRepresent As Boolean = True
        Try
            gfxCheques.Rows.Clear()
            gfxCheques.Columns.Clear()
            UnpayRepresent = False
            Dim trxTyp As String = ""

            If cScan = ENUM_Module_Called.Unpay Then
                txtAccount.Enabled = False
                cmbTrxtype.Visible = False
                Label8.Visible = False
                btnUnpaid.Visible = True
                btnUnpaid.Enabled = False
                btnUnpaid.Text = "Unpay"
                cmdReset.Visible = False
                cTrxType = "I"
                boolGetImagesDetails = False
                cmbTrxtype.Text = "Inwards Clearing"
                txtTheirAccID.Enabled = False
            ElseIf cScan = ENUM_Module_Called.Represent_Cheque Then
                txtAccount.Enabled = False
                cmbTrxtype.Visible = True
                Label8.Visible = True
                btnUnpaid.Visible = True
                btnUnpaid.Enabled = False
                btnUnpaid.Text = "RePresent"
                cmdReset.Visible = False
                'cTrxType = "I"
                boolGetImagesDetails = False
                'cmbTrxtype.Text = "Inwards Clearing"
                txtTheirAccID.Enabled = True
            Else

            End If
            If cmbTrxtype.Text = "Outwards Clearing" Then
                trxTyp = "O"
            ElseIf cmbTrxtype.Text = "InterBranch Clearing" Then
                trxTyp = "A"
            Else
                trxTyp = "I"
            End If

            Select Case CountryCode.ToUpper.Trim
                Case "UG"

                Case "SL"

                Case "TZ"
                    'If OurBranchID.Length <> 2 Then
                    '    OurBranchID = ""
                    'End If
                Case "KE"
                    If OurBranchID.Length <> 3 Then
                        OurBranchID = ""
                    End If
            End Select

            UnpayRepresent = True
            If ExecuteData(GetModify("help_GetChequeImage", "OurBranchID", OurBranchID, "BankID", txtBankID.Text, "BranchID", txtBranchID.Text, "AccountID", txtAccount.Text, "TheirAccount", txtTheirAccID.Text, "ChequeNumber", txtChequeID.Text, "TrxType", trxTyp, "FromDate", FormatDateTime(dtChequeDate.Text), "ToDate", FormatDateTime(dtTo.Text), "FirstAmount", txtFAmount.Text, "SecondAmount", txtSAmount.Text, "UnpayRepresent", UnpayRepresent), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure) Then
                If publicDTbl.Rows.Count > 0 Then
                    If (cScan = ENUM_Module_Called.Unpay) Or (cScan = ENUM_Module_Called.Represent_Cheque) Then
                        btnUnpaid.Enabled = True
                    End If
                    txtAccount.Text = publicDTbl.Rows(0)("AccountID") & cNo_Text
                    txtChequeID.Text = publicDTbl.Rows(0)("ChequeID") & cNo_Text
                    txtTheirAccID.Text = publicDTbl.Rows(0)("TheirAccountID") & cNo_Text
                    txtBankID.Text = publicDTbl.Rows(0)("BankID") & cNo_Text
                    txtBranchID.Text = publicDTbl.Rows(0)("BranchID") & cNo_Text
                    dtChequeDate.Value = Date.Parse(Format(publicDTbl.Rows(0)("date"), "dd MMM yyyy"))
                    dtTo.Value = Date.Parse(Format(publicDTbl.Rows(0)("date"), "dd MMM yyyy"))
                    ImgCheque.Image = IIf(IsDBNull(publicDTbl.Rows(0)("JFImage")), Nothing, GetImages(String2Bytes(publicDTbl.Rows(0)("JFImage"))))
                    ImgIconJ.Image = IIf(IsDBNull(publicDTbl.Rows(0)("JFImage")), Nothing, GetImages(String2Bytes(publicDTbl.Rows(0)("JFImage"))))
                    If IsDBNull(publicDTbl.Rows(0)("UVImage")) Then

                    ElseIf publicDTbl.Rows(0)("UVImage") = "" Then

                    Else
                        ImgIconT.Image = IIf(IsDBNull(publicDTbl.Rows(0)("UVImage")), Nothing, GetImages(String2Bytes(publicDTbl.Rows(0)("UVImage"))))
                    End If
                    ImgIconT.Image = IIf(IsDBNull(publicDTbl.Rows(0)("TFImage")), Nothing, GetImages(String2Bytes(publicDTbl.Rows(0)("TFImage"))))
                    ImgIconJR.Image = IIf(IsDBNull(publicDTbl.Rows(0)("JRImage")), Nothing, GetImages(String2Bytes(publicDTbl.Rows(0)("JRImage"))))


                    ImgCheque.StretchImageToFit = True

                    If (cScan = ENUM_Module_Called.Unpay) Or (cScan = ENUM_Module_Called.Represent_Cheque) Then
                        PopulateGrid(gfxCheques, publicDTbl, MakeInvisibleColumns())
                        gfxCheques.Columns("ColumnID").Visible = False
                        gfxCheques.Columns("OperatorID").Visible = False
                        gfxCheques.Columns("AccountType").Visible = False
                        gfxCheques.Columns("Trxtype").Visible = False
                        gfxCheques.Columns("TheirAccountID").Visible = True
                        gfxCheques.Columns("SerialID").Visible = False
                        gfxCheques.Columns("TFImage").Visible = False
                        gfxCheques.Columns("UVImage").Visible = False
                        gfxCheques.Columns("JFImage").Visible = False
                        gfxCheques.Columns("JRImage").Visible = False
                        gfxCheques.Columns("TFImagesize").Visible = False
                        gfxCheques.Columns("JFImagesize").Visible = False
                        gfxCheques.Columns("JRImagesize").Visible = False

                        Dim newCol As New DataGridViewCheckBoxColumn
                        Dim NewColumn As New DataGridViewTextBoxColumn   'Declare new DGV CC

                        With NewColumn 'Set Properties
                            .HeaderText = "ReturnID"
                            .ReadOnly = True
                            .Name = "ReturnID"
                        End With
                        gfxCheques.Columns.Add(NewColumn) 'Add The Column
                        publicDTbl.Clear()
                        newCol.HeaderText = "Select"
                        newCol.Name = "Select"
                        newCol.ReadOnly = False
                        newCol.Selected = False
                        gfxCheques.Columns.Insert(0, newCol)
                        gfxCheques.ReadOnly = False
                    Else
                        PopulateGrid(gfxCheques, publicDTbl, MakeInvisibleColumns(0, 5, 6, 7, 8, 9, 10, 14, 17))
                    End If
                Else
                    If (cScan = ENUM_Module_Called.Unpay) Or (cScan = ENUM_Module_Called.Represent_Cheque) Then

                    Else
                        cmdReset_Click(sender, e)
                    End If
                End If

            End If
            publicDTbl.Clear()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, My.Application.Info.ProductName)
        End Try
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub txtAccount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAccount.GotFocus
        dtChequeDate.Visible = True
        dtTo.Visible = True
    End Sub

    Private Sub txtAccount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtAccount.KeyDown
    End Sub

    Private Sub txtAccount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtAccount.KeyPress
        'If (Asc(e.KeyChar) < Asc(Keys.D0) Or (Asc(e.KeyChar) > Asc(Keys.D9))) Then
        '    e.Handled = True
        'End If
    End Sub

    Private Sub txtTheirAccID_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTheirAccID.GotFocus
        dtChequeDate.Visible = True
        dtTo.Visible = True
    End Sub

    Private Sub txtTheirAccID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTheirAccID.KeyPress
        'If (Asc(e.KeyChar) < Asc(Keys.D0) Or (Asc(e.KeyChar) > Asc(Keys.D9))) Then
        '    e.Handled = True
        'End If
    End Sub

    'Public Function IsNumericKey(ByVal e As System.Windows.Forms.KeyPressEventArgs)
    'If (Asc(e) >= Asc(Keys.D0) And (Asc(e) <= Asc(Keys.D9))) Then
    '    Return True
    'Else
    '    Return False
    'End If
    'End Function

    Private Sub txtTheirAccID_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTheirAccID.TextChanged

    End Sub

    Private Sub txtChequeID_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtChequeID.GotFocus
        dtChequeDate.Visible = True
        dtTo.Visible = True
    End Sub

    Private Sub txtChequeID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtChequeID.KeyPress
        'If (Asc(e.KeyChar) < Asc(Keys.D0) Or (Asc(e.KeyChar) > Asc(Keys.D9))) Then
        '    e.Handled = True
        'End If
    End Sub

    Private Sub txtChequeID_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtChequeID.TextChanged

    End Sub

    Private Sub txtBankID_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBankID.GotFocus
        dtChequeDate.Visible = True
        dtTo.Visible = True
    End Sub

    Private Sub txtBankID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtBankID.KeyPress
        'If (Asc(e.KeyChar) < Asc(Keys.D0) Or (Asc(e.KeyChar) > Asc(Keys.D9))) Then
        '    e.Handled = True
        'End If
    End Sub

    Private Sub txtBankID_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBankID.TextChanged

    End Sub

    Private Sub txtBranchID_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBranchID.GotFocus
        dtChequeDate.Visible = True
        dtTo.Visible = True
    End Sub

    Private Sub txtBranchID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtBranchID.KeyPress
        'If (Asc(e.KeyChar) < Asc(Keys.D0) Or (Asc(e.KeyChar) > Asc(Keys.D9))) Then
        '    e.Handled = True
        'End If
    End Sub

    Private Sub gfxCheques_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gfxCheques.CellDoubleClick
        Try
            txtAccount.Text = gfxCheques.CurrentRow.Cells("AccountID").Value & cNo_Text
            txtChequeID.Text = gfxCheques.CurrentRow.Cells("ChequeID").Value & cNo_Text
            txtTheirAccID.Text = gfxCheques.CurrentRow.Cells("TheirAccountID").Value & cNo_Text
            txtBankID.Text = gfxCheques.CurrentRow.Cells("BankID").Value & cNo_Text
            txtBranchID.Text = gfxCheques.CurrentRow.Cells("BranchID").Value & cNo_Text

            ImgCheque.Image = IIf(IsDBNull(gfxCheques.CurrentRow.Cells("JFImage").Value), Nothing, GetImages(StringToByte(gfxCheques.CurrentRow.Cells("JFImage").Value)))
            ImgIconJ.Image = IIf(IsDBNull(gfxCheques.CurrentRow.Cells("JFImage").Value), Nothing, GetImages(StringToByte(gfxCheques.CurrentRow.Cells("JFImage").Value)))
            If IsDBNull(gfxCheques.CurrentRow.Cells("UVImage").Value) Then

            ElseIf gfxCheques.CurrentRow.Cells("UVImage").Value = "" Then

            Else
                ImgIconT.Image = IIf(IsDBNull(gfxCheques.CurrentRow.Cells("UVImage").Value), Nothing, GetImages(StringToByte(gfxCheques.CurrentRow.Cells("UVImage").Value)))
            End If
            ImgIconJR.Image = IIf(IsDBNull(gfxCheques.CurrentRow.Cells("TFImage").Value), Nothing, GetImages(StringToByte(gfxCheques.CurrentRow.Cells("TFImage").Value)))
            ImgIconJR.Image = IIf(IsDBNull(gfxCheques.CurrentRow.Cells("JRImage").Value), Nothing, GetImages(StringToByte(gfxCheques.CurrentRow.Cells("JRImage").Value)))

        Catch ex As Exception

        End Try
    End Sub

    Private Sub ImgIconJ_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ImgIconJ.Click
        ImgCheque.StretchImageToFit = False
        ImgCheque.Image = ImgIconJ.Image
    End Sub

    Private Sub ImgCheque_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ImgCheque.Load

    End Sub

    Private Sub txtAccount_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAccount.TextChanged

    End Sub

    Private Sub txtBranchID_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBranchID.TextChanged

    End Sub

    Private Sub cmbTrxtype_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbTrxtype.GotFocus
        dtChequeDate.Visible = True
        dtTo.Visible = True
    End Sub

    Private Sub cmbTrxtype_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbTrxtype.SelectedIndexChanged
        If cmbTrxtype.SelectedIndex = 2 Then
            Dim returnedMsg As String = InputBox("Provide the Account's branch", "BRRealm")
            Modscan.OurBranchID = returnedMsg
        End If
    End Sub

    Private Sub dtChequeDate_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtChequeDate.ValueChanged

    End Sub

    Private Sub gfxCheques_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gfxCheques.CellContentClick
        'ModuleIDT = ""
        Try
            txtAccount.Text = gfxCheques.CurrentRow.Cells("AccountID").Value & cNo_Text
            txtChequeID.Text = gfxCheques.CurrentRow.Cells("ChequeID").Value & cNo_Text
            txtTheirAccID.Text = gfxCheques.CurrentRow.Cells("TheirAccountID").Value & cNo_Text
            txtBankID.Text = gfxCheques.CurrentRow.Cells("BankID").Value & cNo_Text
            txtBranchID.Text = gfxCheques.CurrentRow.Cells("BranchID").Value & cNo_Text

            ImgCheque.Image = IIf(IsDBNull(gfxCheques.CurrentRow.Cells("JFImage").Value), Nothing, GetImages(StringToByte(gfxCheques.CurrentRow.Cells("JFImage").Value)))
            ImgIconJ.Image = IIf(IsDBNull(gfxCheques.CurrentRow.Cells("JFImage").Value), Nothing, GetImages(StringToByte(gfxCheques.CurrentRow.Cells("JFImage").Value)))
            If IsDBNull(gfxCheques.CurrentRow.Cells("UVImage").Value) Then

            ElseIf gfxCheques.CurrentRow.Cells("UVImage").Value = "" Then

            Else
                ImgIconT.Image = IIf(IsDBNull(gfxCheques.CurrentRow.Cells("UVImage").Value), Nothing, GetImages(StringToByte(gfxCheques.CurrentRow.Cells("UVImage").Value)))
            End If
            ImgIconT.Image = IIf(IsDBNull(gfxCheques.CurrentRow.Cells("TFImage").Value), Nothing, GetImages(StringToByte(gfxCheques.CurrentRow.Cells("TFImage").Value)))
            ImgIconJR.Image = IIf(IsDBNull(gfxCheques.CurrentRow.Cells("JRImage").Value), Nothing, GetImages(StringToByte(gfxCheques.CurrentRow.Cells("JRImage").Value)))
            ImgIconT_Click(sender, e)
            Application.DoEvents()

            RowID = ""
            ReturnID = ""
            RowID = e.RowIndex
            ReturnValue = ""
            RetReasonCancel = False
            If ModuleIDT = "Rep" Then
                If gfxCheques.Rows(e.RowIndex).Cells(0).Value = True Then
                    gfxCheques.Rows(e.RowIndex).Cells(0).Value = False
                Else
                    gfxCheques.Rows(e.RowIndex).Cells(0).Value = True
                End If
            ElseIf ModuleIDT = "UnP" Then
                If gfxCheques.Rows(e.RowIndex).Cells(0).Value = True Then
                    gfxCheques.Rows(e.RowIndex).Cells(0).Value = False
                Else
                    gfxCheques.Rows(e.RowIndex).Cells(0).Value = True
                End If
            End If
            If gfxCheques.Rows(e.RowIndex).Cells(0).Value = True Then
                ModuleIDT = "UnP"
                RowID = e.RowIndex
                If cScan = ENUM_Module_Called.Unpay Then
                    Dim frm As New frmreturnReason
                    frm.ShowDialog()
                    If RetReasonCancel = True Then
                        gfxCheques.Rows(e.RowIndex).Cells(0).Value = False
                        Exit Sub
                    Else
                        gfxCheques.Rows(e.RowIndex).Cells("TheirAccountID").Value = ReturnValue2
                        gfxCheques.Rows(e.RowIndex).Cells("ReturnID").Value = ReturnValue
                        gfxCheques.Rows(e.RowIndex).DefaultCellStyle.BackColor = Drawing.Color.DarkGreen
                    End If
                ElseIf cScan = ENUM_Module_Called.Represent_Cheque Then
                    ModuleIDT = "Rep"
                    Dim frm As New frmreturnReason
                    frm.ShowDialog()
                    If RetReasonCancel = True Then
                        gfxCheques.Rows(e.RowIndex).Cells("TheirAccountID").Value = ""
                        Exit Sub
                    Else
                        gfxCheques.Rows(e.RowIndex).Cells("TheirAccountID").Value = ReturnValue
                        gfxCheques.Rows(e.RowIndex).DefaultCellStyle.BackColor = Drawing.Color.DarkGreen
                    End If
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub frmBRChequesSearchClearing_LocationChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LocationChanged

    End Sub

    Private Sub btnUnpaid_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUnpaid.Click
        Dim TotalCommis As Double
        Dim OurCommis As Double
        Dim TheirComm As Double
        Dim clearingDays As String = ""
        Dim VDate As String = ""
        If cScan = ENUM_Module_Called.Unpay Then
            txtAccount.Enabled = False
            cmbTrxtype.Visible = False
            Label8.Visible = False
            btnUnpaid.Visible = True
            btnUnpaid.Enabled = False
            btnUnpaid.Text = "Unpay"
            cmdReset.Visible = False
            cTrxType = "I"
            txtAccName.Text = strAccountName
            Label12.Visible = True
            txtAccName.Visible = True
            Label5.Visible = False
            txtTheirAccID.Visible = False
            boolGetImagesDetails = False


            For i As Int32 = 0 To gfxCheques.Rows.Count - 1
                If gfxCheques.Rows(i).Cells("select").Value = True Then


                    ExecuteData(GetModify("sp_GetBranchIDAndCommission", "ourbranchid", OurBranchID, "bankid", gfxCheques.Rows(i).Cells("bankid").Value, "branchid", gfxCheques.Rows(i).Cells("branchid").Value), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
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


                    If CodeLineDetails.IsUpCountry = 1 Then
                        TotalCommis = 0 * Val(0.25 / 100)
                        OurCommis = TotalCommis * Val(25 / 100)
                        TheirComm = TotalCommis - OurCommis

                        If Val(OurCommis + TheirComm) < 100 Then
                            OurCommis = 25
                            TheirComm = 75
                            TotalCommis = 100
                        End If
                        TotalCommis = RoundTo5Cents(System.Convert.ToDecimal(TotalCommis))
                        OurCommis = RoundTo5Cents(System.Convert.ToDecimal(OurCommis))
                        TheirComm = RoundTo5Cents(System.Convert.ToDecimal(TheirComm))
                    End If

                    ExecuteData(GetModify("SP_GETBANKS", "ourbranchid", OurBranchID, "bankid", gfxCheques.Rows(i).Cells("bankid").Value), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
                    If publicDTbl.Rows.Count > 0 Then
                        CodeLineDetails.BankID = txtBankID.Text
                        CodeLineDetails.BankName = publicDTbl.Rows(0)("FullName").ToString.Trim
                    End If

                    ExecuteData(GetModify("SP_GETbranches", "ourbranchid", OurBranchID, "bankid", gfxCheques.Rows(i).Cells("bankid").Value, "branchid", gfxCheques.Rows(i).Cells("branchid").Value), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
                    If publicDTbl.Rows.Count > 0 Then
                        CodeLineDetails.BranchName = publicDTbl.Rows(0)("Name").ToString.Trim
                        CodeLineDetails.BranchID = txtBranchID.Text
                    End If

                    'TODO: GETTING AMOUNT AND VOUCHERCODE
                    'Validate the clearing Days
                    If Configuration.ConfigurationManager.AppSettings("sysType") <> "BRMFO" Then
                        ExecuteData(GetModify("sp_GetValueDate", "ourbranchid", OurBranchID, "AccountID", gfxCheques.Rows(i).Cells("AccountID").Value, "Date", gfxCheques.Rows(i).Cells("Date").Value, "VoucherCode", gfxCheques.Rows(i).Cells("VoucherCode").Value, "BankID", gfxCheques.Rows(i).Cells("BankID").Value, "BranchID", gfxCheques.Rows(i).Cells("BranchID").Value, "Amount", gfxCheques.Rows(i).Cells("Amount").Value), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.SelectStatement)
                        If publicDTbl.Rows.Count > 0 Then
                            clearingDays = publicDTbl.Rows(0)("ClearingDays").ToString
                            VDate = publicDTbl.Rows(0)("ValueDate").ToString
                        Else
                            clearingDays = "2"
                        End If
                        publicDTbl.Clear()
                    Else
                        ExecuteData(GetModify("sp_GetValueDate", "ourbranchid", OurBranchID, "AccountID", gfxCheques.Rows(i).Cells("AccountID").Value, "AccountType", "C", "Date", gfxCheques.Rows(i).Cells("Date").Value, "IsUpcountry", False), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.SelectStatement)
                        If publicDTbl.Rows.Count > 0 Then
                            VDate = publicDTbl.Rows(0)("ValueDate").ToString
                            clearingDays = "2"
                        Else
                            clearingDays = "2"
                        End If
                        publicDTbl.Clear()

                    End If
                    Dim RetnReasn As String = (gfxCheques.Rows(i).Cells("ReturnID").Value).ToString.Substring(0, 4)
                    ExecuteData(GetModify("sp_SaveUnPayRepresentment", "OperatorID", OperatorID, "ReturnCode", RetnReasn, "ClearingDays", "0", "ClearingCenterID", "99", "Ourcommission", OurCommis, _
                                 "TheirCommission", TheirComm, "WorkingDate", cWorkingDate, "ImageUniqueID", "", "BankName", CodeLineDetails.BankName, "BranchName", CodeLineDetails.BranchName, "ValueDate", cWorkingDate, "BankID", gfxCheques.Rows(i).Cells("BankID").Value, _
                                 "BranchID", gfxCheques.Rows(i).Cells("BranchID").Value, "TheirAcc", IIf(gfxCheques.Rows(i).Cells("TheirAccountID").Value = "", "0000000001", gfxCheques.Rows(i).Cells("TheirAccountID").Value), "ChequeID", gfxCheques.Rows(i).Cells("ChequeID").Value, _
                                 "TransactionDate", gfxCheques.Rows(i).Cells("Date").Value, "AccountID", txtAccount.Text, "ourbranchid", OurBranchID), publicDTbl, dataExecTypes.ExecTypeNonQuery, queryType.SelectStatement)
                End If
            Next

        ElseIf cScan = ENUM_Module_Called.Represent_Cheque Then
            txtAccount.Enabled = False
            cmbTrxtype.Visible = False
            Label8.Visible = False
            btnUnpaid.Visible = True
            btnUnpaid.Enabled = False
            txtAccName.Text = strAccountName
            btnUnpaid.Text = "Re-Present"
            cmdReset.Visible = False
            cTrxType = "I"
            Label12.Visible = True
            txtAccName.Visible = True
            Label5.Visible = False
            txtTheirAccID.Visible = False
            boolGetImagesDetails = False

            For i As Int32 = 0 To gfxCheques.Rows.Count - 1
                If gfxCheques.Rows(i).Cells("select").Value = True Then


                    ExecuteData(GetModify("sp_GetBranchIDAndCommission", "ourbranchid", OurBranchID, "bankid", gfxCheques.Rows(i).Cells("bankid").Value, "branchid", gfxCheques.Rows(i).Cells("branchid").Value), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
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


                    If CodeLineDetails.IsUpCountry = 1 Then
                        TotalCommis = 0 * Val(0.25 / 100)
                        OurCommis = TotalCommis * Val(25 / 100)
                        TheirComm = TotalCommis - OurCommis

                        If Val(OurCommis + TheirComm) < 100 Then
                            OurCommis = 25
                            TheirComm = 75
                            TotalCommis = 100
                        End If
                        TotalCommis = RoundTo5Cents(System.Convert.ToDecimal(TotalCommis))
                        OurCommis = RoundTo5Cents(System.Convert.ToDecimal(OurCommis))
                        TheirComm = RoundTo5Cents(System.Convert.ToDecimal(TheirComm))
                    End If

                    ExecuteData(GetModify("SP_GETBANKS", "ourbranchid", OurBranchID, "bankid", gfxCheques.Rows(i).Cells("bankid").Value), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
                    If publicDTbl.Rows.Count > 0 Then
                        CodeLineDetails.BankID = txtBankID.Text
                        CodeLineDetails.BankName = publicDTbl.Rows(0)("FullName").ToString.Trim
                    End If

                    ExecuteData(GetModify("SP_GETbranches", "ourbranchid", OurBranchID, "bankid", gfxCheques.Rows(i).Cells("bankid").Value, "branchid", gfxCheques.Rows(i).Cells("branchid").Value), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.StoredProcedure)
                    If publicDTbl.Rows.Count > 0 Then
                        CodeLineDetails.BranchName = publicDTbl.Rows(0)("Name").ToString.Trim
                        CodeLineDetails.BranchID = txtBranchID.Text
                    End If

                    'TODO: GETTING AMOUNT AND VOUCHERCODE
                    'Validate the clearing Days
                    ExecuteData(GetModify("sp_GetValueDate", "ourbranchid", OurBranchID, "AccountID", gfxCheques.Rows(i).Cells("AccountID").Value, "Date", gfxCheques.Rows(i).Cells("Date").Value, "VoucherCode", gfxCheques.Rows(i).Cells("VoucherCode").Value, "BankID", gfxCheques.Rows(i).Cells("BankID").Value, "BranchID", gfxCheques.Rows(i).Cells("BranchID").Value, "Amount", gfxCheques.Rows(i).Cells("Amount").Value), publicDTbl, dataExecTypes.ExecTypeQuery, queryType.SelectStatement)
                    If publicDTbl.Rows.Count > 0 Then
                        clearingDays = publicDTbl.Rows(0)("ClearingDays").ToString
                        VDate = publicDTbl.Rows(0)("ValueDate").ToString
                    Else
                        clearingDays = "4"
                    End If
                    publicDTbl.Clear()

                    ExecuteData(GetModify("sp_SaveUnPayRepresentment", "OperatorID", OperatorID, "ReturnCode", "00", "ClearingDays", clearingDays, "ClearingCenterID", "99", "Ourcommission", OurCommis _
                     , "TheirCommission", TheirComm, "WorkingDate", cWorkingDate, "ImageUniqueID", "", "BankName", CodeLineDetails.BankName, "BranchName", CodeLineDetails.BranchName, "ValueDate", VDate, "BankID", txtBankID.Text _
                    , "TransactionDate", gfxCheques.Rows(i).Cells("date").Value, "BranchID", txtBranchID.Text, "TheirAcc", gfxCheques.Rows(i).Cells("TheirAccountID").Value, "ChequeID", txtChequeID.Text, "AccountID", txtAccount.Text, "ourbranchid", OurBranchID), publicDTbl, dataExecTypes.ExecTypeNonQuery, queryType.SelectStatement)
                End If
            Next
        End If
        If MsgBox("Do you wish to " & btnUnpaid.Text & " another item?", MsgBoxStyle.YesNo, "BRClearing") = MsgBoxResult.No Then
            Me.Close()
        Else
            cmdReset_Click(sender, e)
            txtAccount.Text = strAccountID
            txtTheirAccID.Focus()
        End If
    End Sub

    Private Sub gfxCheques_CellValidating(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellValidatingEventArgs) Handles gfxCheques.CellValidating
        ''Create a New ComboBoxColumn Object, And Cast The dataGridView's Column To That
        'Dim comboBoxColumn As DataGridViewComboBoxColumn
        ''comboBoxColumn = New DataGridViewComboBoxColumn
        'comboBoxColumn = CType(gfxCheques.Columns(1), System.Windows.Forms.DataGridViewComboBoxColumn)
        ''If In ComboBoxColumn
        'If (e.ColumnIndex = comboBoxColumn.DisplayIndex) Then
        '    If (Not comboBoxColumn.Items.Contains(e.FormattedValue)) Then
        '        'Add The Text Entered By The User
        '        comboBoxColumn.Items.Add(e.FormattedValue)
        '        'Make Sure Value Stays Displayed ( May HAve To Enter Value Twice )
        '        gfxCheques.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = comboBoxColumn.Items(comboBoxColumn.Items.Count - 1)
        '    End If
        'End If
    End Sub

    Private Sub gfxCheques_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles gfxCheques.EditingControlShowing
        'If In ComboBox Column
        If (gfxCheques.CurrentCellAddress.X = gfxCheques.Columns(23).DisplayIndex) Then
            'Cast To Normal ComboBox
            Dim cb As ComboBox = CType(e.Control, ComboBox)
            If (cb IsNot Nothing) Then
                'Change Style To DropDown, To Allow For Data Entry
                cb.DropDownStyle = ComboBoxStyle.DropDown
            End If

        End If
    End Sub
End Class