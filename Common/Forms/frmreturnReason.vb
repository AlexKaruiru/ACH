Imports BrClearing.Common.Modscan
Public Class frmreturnReason

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        RetReasonCancel = True
        Me.Close()
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        RetReasonCancel = False

        If ModuleIDT = "Rep" Then
            If txtTheirAcc.Text = "" Then
                MsgBox("Provide receiving Account", MsgBoxStyle.Information, "BRClearing")
                txtTheirAcc.Focus()
                Exit Sub
            Else
                ReturnValue = txtTheirAcc.Text
            End If
        Else
            If txtTheirAcc.Text = "" Then
                ReturnValue2 = "0000000001"
            Else
                ReturnValue2 = txtTheirAcc.Text
            End If
            If cmbReturnReason.SelectedIndex = 0 Then
                MsgBox(" Invalid return Reason Code", MsgBoxStyle.Information, "BRClearing")
                cmbReturnReason.Focus()
                Exit Sub
            ElseIf cmbReturnReason.SelectedIndex = 1 Then
                MsgBox("This return Reason Code is not allowed", MsgBoxStyle.Information, "BRClearing")
                cmbReturnReason.Focus()
                Exit Sub
            End If
            If RowID <> "" Then
                ReturnID = cmbReturnReason.SelectedItem
                ReturnValue = ReturnID
            End If
        End If
        Me.Close()
    End Sub

    Private Sub frmreturnReason_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ExecuteData("exec sp_GetReturnReasons", publicDTbl, dataExecTypes.ExecTypeQuery, queryType.SelectStatement)
        FillCombo(publicDTbl, "ReturnID", cmbReturnReason, "Description")
        If ModuleIDT = "Rep" Then
            cmbReturnReason.Enabled = False
            txtTheirAcc.Enabled = True
        Else
            txtTheirAcc.Enabled = True
            cmbReturnReason.Enabled = True
        End If
    End Sub

    Private Sub cmbReturnReason_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbReturnReason.SelectedIndexChanged

    End Sub
End Class