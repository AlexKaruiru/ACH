Imports System
Imports System.Windows.Forms
Imports BrClearing.Common.Modscan
Public Class frmMDVUG
    Dim mdvChequeID As String = ""
    Dim mdvBankID As String = ""
    Dim mdvBranchID As String = ""
    Dim mdvChequeDigit As String = ""
    Dim mdvRegion As String = ""
    Dim mdvVoucherCode As String = ""
    Dim mdvAccount As String = ""

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Try
            isMDV = True
            mdvChequeID = txtChequeID.Text.Replace("/", "")
            mdvChequeID = New String("0", 6 - mdvChequeID.Length) & mdvChequeID

            mdvBankID = txtBankID.Text.Replace("/", "")
            mdvBankID = New String("0", 2 - mdvBankID.Length) & mdvBankID

            mdvBranchID = txtBranchID.Text.Replace("/", "")
            mdvBranchID = New String("0", 2 - mdvBranchID.Length) & mdvBranchID

            mdvChequeDigit = txtChequeDigit.Text.Replace("/", "")
            mdvChequeDigit = New String("0", 2 - mdvChequeDigit.Length) & mdvChequeDigit

            mdvRegion = txtRegion.Text.Replace("/", "")
            mdvRegion = New String("0", 2 - mdvRegion.Length) & mdvRegion

            mdvVoucherCode = txtVoucherCode.Text.Replace("/", "")
            mdvVoucherCode = New String("0", 2 - mdvVoucherCode.Length) & mdvVoucherCode

            mdvAccount = txtAccount.Text.Replace("/", "")
            mdvAccount = New String("0", 10 - mdvAccount.Length) & mdvAccount


            MDVMicr = mdvChequeID & mdvChequeDigit & mdvBankID & mdvBranchID & mdvRegion & mdvAccount & mdvVoucherCode
            MessageBox.Show("Please ensure the cheque is placed on the scanner before continuing from here")
            Me.Close()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        isMDV = False
        Me.Close()
    End Sub

    Private Sub frmMDV_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        isMDV = True
        mdvReturnCode = ""
        'MessageBox.Show("Please ensure the cheque is placed on the reader before clicking OK Button")
    End Sub

    Private Sub txtChequeID_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtChequeID.KeyDown
    End Sub

    Private Sub txtAccount_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAccount.LostFocus
        mdvAccount = txtAccount.Text.Replace("/", "")
        mdvAccount = mdvAccount.Trim(Space(10))
        mdvAccount = New String("0", 10 - mdvAccount.Length) & mdvAccount
        If mdvAccount = "0000000000" Then
            mdvAccount = "0000000001"
        End If
        txtAccount.Text = mdvAccount
    End Sub


    Private Sub txtChequeID_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtChequeID.LostFocus
        mdvChequeID = txtChequeID.Text.Replace("/", "")
        mdvChequeID = mdvChequeID.Trim(Space(6))
        mdvChequeID = New String("0", 6 - mdvChequeID.Length) & mdvChequeID
        If mdvChequeID = "000000" Then
            mdvChequeID = "000001"
        End If
        txtChequeID.Text = mdvChequeID
    End Sub

    Private Sub txtBankID_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBankID.LostFocus
        mdvBankID = txtBankID.Text.Replace("/", "")
        mdvBankID = mdvBankID.Trim(Space(2))
        mdvBankID = New String("0", 2 - mdvBankID.Length) & mdvBankID
        If mdvBankID = "00" Then
            mdvBankID = "01"
        End If
        txtBankID.Text = mdvBankID
    End Sub

    Private Sub txtBranchID_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBranchID.LostFocus
        mdvBranchID = txtBranchID.Text.Replace("/", "")
        mdvBranchID = mdvBranchID.Trim(Space(3))
        mdvBranchID = New String("0", 2 - mdvBranchID.Length) & mdvBranchID
        txtBranchID.Text = mdvBranchID
    End Sub

    Private Sub chkReturnReason_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkReturnReason.CheckedChanged
        If chkReturnReason.Checked = True Then
            Dim frm As New frmreturnReason
            ModuleIDT = "UnP"
            frm.ShowDialog()
            If RetReasonCancel = True Then
                Exit Sub
            Else
                mdvReturnCode = frm.cmbReturnReason.Text.Substring(0, 2)
            End If
        Else
            RetReasonCancel = True
            mdvReturnCode = "00"
            ModuleIDT = ""
        End If
    End Sub

    Private Sub txtRegion_LostFocus(sender As Object, e As EventArgs) Handles txtRegion.LostFocus
        mdvRegion = txtRegion.Text.Replace("/", "")
        mdvRegion = mdvRegion.Trim(Space(2))
        mdvRegion = New String("0", 2 - mdvRegion.Length) & mdvRegion
        If mdvRegion = "00" Then
            mdvRegion = "47"
        End If
        txtRegion.Text = mdvRegion
    End Sub
End Class