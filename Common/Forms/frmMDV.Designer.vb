<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMDV
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.chkReturnReason = New System.Windows.Forms.CheckBox()
        Me.txtChequeID = New System.Windows.Forms.TextBox()
        Me.txtBankID = New System.Windows.Forms.TextBox()
        Me.txtBranchID = New System.Windows.Forms.TextBox()
        Me.txtChequeDigit = New System.Windows.Forms.TextBox()
        Me.txtVoucherCode = New System.Windows.Forms.TextBox()
        Me.txtAccount = New System.Windows.Forms.TextBox()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnOK
        '
        Me.btnOK.BackColor = System.Drawing.Color.WhiteSmoke
        Me.btnOK.Location = New System.Drawing.Point(13, 65)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 23)
        Me.btnOK.TabIndex = 7
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = False
        '
        'btnCancel
        '
        Me.btnCancel.BackColor = System.Drawing.Color.WhiteSmoke
        Me.btnCancel.Location = New System.Drawing.Point(247, 65)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 8
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(32, 13)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "chq#"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(76, 18)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(26, 13)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "Bnk"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(102, 18)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(35, 13)
        Me.Label3.TabIndex = 13
        Me.Label3.Text = "Brnch"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(138, 18)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(28, 13)
        Me.Label4.TabIndex = 14
        Me.Label4.Text = "Digit"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(166, 18)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(35, 13)
        Me.Label5.TabIndex = 15
        Me.Label5.Text = "vchr#"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(269, 18)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(54, 13)
        Me.Label6.TabIndex = 16
        Me.Label6.Text = "Account#"
        '
        'GroupBox1
        '
        Me.GroupBox1.BackColor = System.Drawing.Color.Transparent
        Me.GroupBox1.Controls.Add(Me.txtAccount)
        Me.GroupBox1.Controls.Add(Me.txtVoucherCode)
        Me.GroupBox1.Controls.Add(Me.txtChequeDigit)
        Me.GroupBox1.Controls.Add(Me.txtBranchID)
        Me.GroupBox1.Controls.Add(Me.txtBankID)
        Me.GroupBox1.Controls.Add(Me.txtChequeID)
        Me.GroupBox1.Controls.Add(Me.chkReturnReason)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.btnCancel)
        Me.GroupBox1.Controls.Add(Me.btnOK)
        Me.GroupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.GroupBox1.Location = New System.Drawing.Point(4, 2)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(329, 92)
        Me.GroupBox1.TabIndex = 17
        Me.GroupBox1.TabStop = False
        '
        'chkReturnReason
        '
        Me.chkReturnReason.AutoSize = True
        Me.chkReturnReason.Location = New System.Drawing.Point(110, 69)
        Me.chkReturnReason.Name = "chkReturnReason"
        Me.chkReturnReason.Size = New System.Drawing.Size(119, 17)
        Me.chkReturnReason.TabIndex = 17
        Me.chkReturnReason.Text = "Edit Return Reason"
        Me.chkReturnReason.UseVisualStyleBackColor = True
        Me.chkReturnReason.Visible = False
        '
        'txtChequeID
        '
        Me.txtChequeID.Location = New System.Drawing.Point(9, 37)
        Me.txtChequeID.MaxLength = 6
        Me.txtChequeID.Name = "txtChequeID"
        Me.txtChequeID.Size = New System.Drawing.Size(62, 20)
        Me.txtChequeID.TabIndex = 0
        '
        'txtBankID
        '
        Me.txtBankID.Location = New System.Drawing.Point(71, 37)
        Me.txtBankID.MaxLength = 2
        Me.txtBankID.Name = "txtBankID"
        Me.txtBankID.Size = New System.Drawing.Size(31, 20)
        Me.txtBankID.TabIndex = 2
        '
        'txtBranchID
        '
        Me.txtBranchID.Location = New System.Drawing.Point(102, 37)
        Me.txtBranchID.MaxLength = 3
        Me.txtBranchID.Name = "txtBranchID"
        Me.txtBranchID.Size = New System.Drawing.Size(39, 20)
        Me.txtBranchID.TabIndex = 3
        '
        'txtChequeDigit
        '
        Me.txtChequeDigit.Location = New System.Drawing.Point(141, 37)
        Me.txtChequeDigit.MaxLength = 1
        Me.txtChequeDigit.Name = "txtChequeDigit"
        Me.txtChequeDigit.Size = New System.Drawing.Size(30, 20)
        Me.txtChequeDigit.TabIndex = 4
        '
        'txtVoucherCode
        '
        Me.txtVoucherCode.Location = New System.Drawing.Point(171, 37)
        Me.txtVoucherCode.MaxLength = 2
        Me.txtVoucherCode.Name = "txtVoucherCode"
        Me.txtVoucherCode.Size = New System.Drawing.Size(26, 20)
        Me.txtVoucherCode.TabIndex = 5
        '
        'txtAccount
        '
        Me.txtAccount.Location = New System.Drawing.Point(197, 37)
        Me.txtAccount.MaxLength = 10
        Me.txtAccount.Name = "txtAccount"
        Me.txtAccount.Size = New System.Drawing.Size(123, 20)
        Me.txtAccount.TabIndex = 6
        '
        'frmMDV
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(237, Byte), Integer), CType(CType(246, Byte), Integer), CType(CType(251, Byte), Integer))
        Me.BackgroundImage = Global.BRClearing.Common.My.Resources.Resources.log_img6
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(338, 98)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmMDV"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "MDV Processing ..."
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents chkReturnReason As System.Windows.Forms.CheckBox
    Friend WithEvents txtChequeID As System.Windows.Forms.TextBox
    Friend WithEvents txtAccount As System.Windows.Forms.TextBox
    Friend WithEvents txtVoucherCode As System.Windows.Forms.TextBox
    Friend WithEvents txtChequeDigit As System.Windows.Forms.TextBox
    Friend WithEvents txtBranchID As System.Windows.Forms.TextBox
    Friend WithEvents txtBankID As System.Windows.Forms.TextBox
End Class
