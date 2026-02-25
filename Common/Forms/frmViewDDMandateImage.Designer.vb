<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmViewDDMandateImage
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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.PicDDImage = New CSI_ImageControl.ImageControl()
        Me.label3 = New System.Windows.Forms.Label()
        Me.txtFolderName = New System.Windows.Forms.TextBox()
        Me.btnBrowse = New System.Windows.Forms.Button()
        Me.cmdSave = New System.Windows.Forms.Button()
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.cmdPrint = New System.Windows.Forms.Button()
        Me.prnDoc = New System.Drawing.Printing.PrintDocument()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.BackgroundImage = Global.BrClearing.Common.My.Resources.Resources.log_img6
        Me.GroupBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.GroupBox1.Controls.Add(Me.cmdExit)
        Me.GroupBox1.Controls.Add(Me.cmdPrint)
        Me.GroupBox1.Controls.Add(Me.Button2)
        Me.GroupBox1.Controls.Add(Me.PicDDImage)
        Me.GroupBox1.Controls.Add(Me.Button1)
        Me.GroupBox1.Controls.Add(Me.cmdSave)
        Me.GroupBox1.Location = New System.Drawing.Point(8, 49)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(1001, 470)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'PicDDImage
        '
        Me.PicDDImage.BackColor = System.Drawing.Color.Transparent
        Me.PicDDImage.Image = Nothing
        Me.PicDDImage.initialimage = Nothing
        Me.PicDDImage.Location = New System.Drawing.Point(8, 10)
        Me.PicDDImage.Name = "PicDDImage"
        Me.PicDDImage.Origin = New System.Drawing.Point(0, 0)
        Me.PicDDImage.PanButton = System.Windows.Forms.MouseButtons.Left
        Me.PicDDImage.PanMode = True
        Me.PicDDImage.ScrollbarsVisible = True
        Me.PicDDImage.Size = New System.Drawing.Size(889, 440)
        Me.PicDDImage.StretchImageToFit = False
        Me.PicDDImage.TabIndex = 28
        Me.PicDDImage.ZoomFactor = 1.0R
        Me.PicDDImage.ZoomOnMouseWheel = True
        '
        'label3
        '
        Me.label3.AutoSize = True
        Me.label3.BackColor = System.Drawing.Color.Transparent
        Me.label3.ForeColor = System.Drawing.Color.Transparent
        Me.label3.Location = New System.Drawing.Point(27, 15)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(48, 13)
        Me.label3.TabIndex = 10
        Me.label3.Text = "File Path"
        '
        'txtFolderName
        '
        Me.txtFolderName.BackColor = System.Drawing.SystemColors.Info
        Me.txtFolderName.Location = New System.Drawing.Point(81, 11)
        Me.txtFolderName.Name = "txtFolderName"
        Me.txtFolderName.Size = New System.Drawing.Size(554, 20)
        Me.txtFolderName.TabIndex = 8
        '
        'btnBrowse
        '
        Me.btnBrowse.Location = New System.Drawing.Point(655, 11)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(92, 21)
        Me.btnBrowse.TabIndex = 9
        Me.btnBrowse.Text = "Browse Folder"
        Me.btnBrowse.UseVisualStyleBackColor = True
        '
        'cmdSave
        '
        Me.cmdSave.Location = New System.Drawing.Point(899, 296)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.Size = New System.Drawing.Size(93, 28)
        Me.cmdSave.TabIndex = 1
        Me.cmdSave.Text = "Save"
        Me.cmdSave.UseVisualStyleBackColor = True
        '
        'cmdExit
        '
        Me.cmdExit.Location = New System.Drawing.Point(899, 385)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.Size = New System.Drawing.Size(93, 27)
        Me.cmdExit.TabIndex = 2
        Me.cmdExit.Text = "Exit"
        Me.cmdExit.UseVisualStyleBackColor = True
        '
        'cmdPrint
        '
        Me.cmdPrint.Location = New System.Drawing.Point(899, 340)
        Me.cmdPrint.Name = "cmdPrint"
        Me.cmdPrint.Size = New System.Drawing.Size(93, 28)
        Me.cmdPrint.TabIndex = 3
        Me.cmdPrint.Text = "Print"
        Me.cmdPrint.UseVisualStyleBackColor = True
        '
        'prnDoc
        '
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(899, 19)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(93, 28)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "Zoom Out"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(899, 63)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(93, 28)
        Me.Button2.TabIndex = 5
        Me.Button2.Text = "Zoom In"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'frmViewDDMandateImage
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = Global.BrClearing.Common.My.Resources.Resources.log_img6
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(1015, 531)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.btnBrowse)
        Me.Controls.Add(Me.txtFolderName)
        Me.Controls.Add(Me.label3)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "frmViewDDMandateImage"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Direct Debit Mandate Image"
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents cmdSave As System.Windows.Forms.Button
    Friend WithEvents cmdExit As System.Windows.Forms.Button
    Private WithEvents label3 As System.Windows.Forms.Label
    Private WithEvents txtFolderName As System.Windows.Forms.TextBox
    Private WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents PicDDImage As CSI_ImageControl.ImageControl
    Friend WithEvents cmdPrint As System.Windows.Forms.Button
    Friend WithEvents prnDoc As System.Drawing.Printing.PrintDocument
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
End Class
