Imports System
Imports System.Windows.Forms
Imports BrClearing.Common.Modscan
Public Class frmMDVET
    Dim mdvChequeID As String = ""
    Dim mdvBankID As String = ""
    Dim mdvBranchID As String = ""
    Dim mdvChequeDigit As String = ""
    Dim mdvVoucherCode As String = ""
    Dim mdvAccount As String = ""
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Try
            isMDV = True
            MessageBox.Show("Make sure you have the cheque ready on the scanner to be scanned")
            mdvChequeID = txtChequeID.Text.Replace("/", "")
            mdvChequeID = New String("0", 11 - mdvChequeID.Length) & mdvChequeID

            mdvBankID = txtBankID.Text.Replace("/", "")
            mdvBankID = New String("0", 2 - mdvBankID.Length) & mdvBankID

            mdvBranchID = txtBranchID.Text.Replace("/", "")
            mdvBranchID = New String("0", 4 - mdvBranchID.Length) & mdvBranchID

            mdvChequeDigit = "0" 'txtChequeDigit.Text.Replace("/", "")
            mdvChequeDigit = New String("0", 1 - mdvChequeDigit.Length) & mdvChequeDigit

            mdvVoucherCode = txtVoucherCode.Text.Replace("/", "")
            mdvVoucherCode = New String("0", 2 - mdvVoucherCode.Length) & mdvVoucherCode

            mdvAccount = txtAccount.Text.Replace("/", "")
            mdvAccount = New String("0", 13 - mdvAccount.Length) & mdvAccount
            MDVMicr = mdvChequeID & mdvBankID & mdvBranchID & mdvChequeDigit & mdvVoucherCode & mdvAccount

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

    Private Sub txtChequeID_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
    End Sub

    Private Sub txtAccount_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        'If IsNumeric(txtAccount.Text) Then
        mdvAccount = txtAccount.Text.Replace("/", "")
        mdvAccount = mdvAccount.Trim(Space(10))
        mdvAccount = New String("0", 13 - mdvAccount.Length) & mdvAccount
        If mdvAccount = "0000000000000" Then
            mdvAccount = "0000000000001"
        End If
        txtAccount.Text = mdvAccount
        'Else
        '    MessageBox.Show("Please provide correct Account ID, else confirm with clearing department")
        '    txtAccount.Text = ""
        '    txtAccount.Focus()
        'End If
    End Sub


    Private Sub txtChequeID_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        'If IsNumeric(txtChequeID.Text) Or txtChequeID.Text = "      /" Then
        mdvChequeID = txtChequeID.Text.Replace("/", "")
        mdvChequeID = mdvChequeID.Trim(Space(11))
        mdvChequeID = New String("0", 11 - mdvChequeID.Length) & mdvChequeID
        If mdvChequeID = "00000000000" Then
            mdvChequeID = "00000000001"
        End If
        txtChequeID.Text = mdvChequeID
        'Else
        '    MessageBox.Show("Please provide correct Cheque Serial ID else, confirm with clearing department")
        '    txtChequeID.Text = ""
        '    txtChequeID.Focus()
        'End If
    End Sub

    Private Sub txtBankID_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        'If IsNumeric(txtBankID.Text) Or txtBankID.Text = "  /" Then
        mdvBankID = txtBankID.Text.Replace("/", "")
        mdvBankID = mdvBankID.Trim(Space(2))
        mdvBankID = New String("0", 2 - mdvBankID.Length) & mdvBankID
        If mdvBankID = "00" Then
            mdvBankID = "01"
        End If
        txtBankID.Text = mdvBankID
        'Else
        '    MessageBox.Show("Please provide correct bank code else, confirm with clearing department")
        '    txtBankID.Text = ""
        '    txtBankID.Focus()
        'End If

    End Sub

    Private Sub txtBranchID_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        'If IsNumeric(txtBankID.Text) Or txtBankID.Text = "  /" Then
        mdvBankID = txtBankID.Text.Replace("/", "")
        mdvBankID = mdvBankID.Trim(Space(2))
        mdvBankID = New String("0", 4 - mdvBankID.Length) & mdvBankID
        If mdvBankID = "0000" Then
            mdvBankID = "0001"
        End If
        txtBankID.Text = mdvBankID
        'Else
        '    MessageBox.Show("Please provide correct bank code else, confirm with clearing department")
        '    txtBankID.Text = ""
        '    txtBankID.Focus()
        'End If


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

    Private Sub txtVoucherCode_LostFocus(sender As Object, e As EventArgs)
        'If IsNumeric(txtBranchID.Text) Or txtBranchID.Text = "   /" Or txtBranchID.Text = "000/" Then
        mdvBranchID = txtBranchID.Text.Replace("/", "")
        mdvBranchID = mdvBranchID.Trim(Space(3))
        mdvBranchID = New String("0", 3 - mdvBranchID.Length) & mdvBranchID
        txtBranchID.Text = mdvBranchID
        'Else
        '    MessageBox.Show("Please provide correct branch code else, confirm with clearing department")
        '    txtBranchID.Text = ""
        '    txtBranchID.Focus()
        'End If
        Select Case txtVoucherCode.Text.ToString()

            Case "01/", "03/", "11/", "13/", "14/", "15/", "16/", "17/", "18/", "19/", "20/", "21/", "60/", "61/", "62/"
                Exit Sub
            Case Else
                MessageBox.Show("Please provide accepted voucher code for cheques else, confirm with clearing department")
                txtVoucherCode.Text = ""
                txtVoucherCode.Focus()
        End Select
    End Sub

    Private Sub txtBranchID_TextChanged(sender As Object, e As EventArgs) Handles txtBranchID.TextChanged

    End Sub


End Class