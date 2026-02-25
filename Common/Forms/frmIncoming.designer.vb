<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmIncoming
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
        Me.btnRead = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.FileProgress = New System.Windows.Forms.ProgressBar()
        Me.DLDirPath = New Microsoft.VisualBasic.Compatibility.VB6.DirListBox()
        Me.FLItems = New Microsoft.VisualBasic.Compatibility.VB6.FileListBox()
        Me.DLDrivePath = New Microsoft.VisualBasic.Compatibility.VB6.DriveListBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.PictureBox3 = New System.Windows.Forms.PictureBox()
        Me.prgIncomingImages = New System.Windows.Forms.ProgressBar()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblBankID = New System.Windows.Forms.Label()
        Me.Prg = New System.Windows.Forms.ProgressBar()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnRead
        '
        Me.btnRead.Location = New System.Drawing.Point(875, 196)
        Me.btnRead.Name = "btnRead"
        Me.btnRead.Size = New System.Drawing.Size(10, 23)
        Me.btnRead.TabIndex = 1
        Me.btnRead.Text = "Read Files"
        Me.btnRead.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(876, 244)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(10, 27)
        Me.btnExit.TabIndex = 2
        Me.btnExit.Text = "Exit"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'FileProgress
        '
        Me.FileProgress.Location = New System.Drawing.Point(7, 3)
        Me.FileProgress.Name = "FileProgress"
        Me.FileProgress.Size = New System.Drawing.Size(398, 22)
        Me.FileProgress.TabIndex = 3
        '
        'DLDirPath
        '
        Me.DLDirPath.FormattingEnabled = True
        Me.DLDirPath.IntegralHeight = False
        Me.DLDirPath.Location = New System.Drawing.Point(850, 53)
        Me.DLDirPath.Name = "DLDirPath"
        Me.DLDirPath.Size = New System.Drawing.Size(35, 29)
        Me.DLDirPath.TabIndex = 6
        '
        'FLItems
        '
        Me.FLItems.FormattingEnabled = True
        Me.FLItems.Location = New System.Drawing.Point(683, 207)
        Me.FLItems.Name = "FLItems"
        Me.FLItems.Pattern = "*.*"
        Me.FLItems.Size = New System.Drawing.Size(217, 277)
        Me.FLItems.TabIndex = 7
        '
        'DLDrivePath
        '
        Me.DLDrivePath.FormattingEnabled = True
        Me.DLDrivePath.Location = New System.Drawing.Point(876, 3)
        Me.DLDrivePath.Name = "DLDrivePath"
        Me.DLDrivePath.Size = New System.Drawing.Size(10, 21)
        Me.DLDrivePath.TabIndex = 8
        '
        'PictureBox1
        '
        Me.PictureBox1.Location = New System.Drawing.Point(12, 182)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(433, 209)
        Me.PictureBox1.TabIndex = 9
        Me.PictureBox1.TabStop = False
        '
        'PictureBox2
        '
        Me.PictureBox2.Location = New System.Drawing.Point(12, 241)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(433, 195)
        Me.PictureBox2.TabIndex = 10
        Me.PictureBox2.TabStop = False
        '
        'PictureBox3
        '
        Me.PictureBox3.Location = New System.Drawing.Point(451, 12)
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.Size = New System.Drawing.Size(377, 189)
        Me.PictureBox3.TabIndex = 11
        Me.PictureBox3.TabStop = False
        '
        'prgIncomingImages
        '
        Me.prgIncomingImages.Location = New System.Drawing.Point(514, 353)
        Me.prgIncomingImages.Name = "prgIncomingImages"
        Me.prgIncomingImages.Size = New System.Drawing.Size(350, 18)
        Me.prgIncomingImages.TabIndex = 12
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Courier New", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(4, 28)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(112, 14)
        Me.Label1.TabIndex = 13
        Me.Label1.Text = "Reading Bank : "
        '
        'lblBankID
        '
        Me.lblBankID.BackColor = System.Drawing.Color.Transparent
        Me.lblBankID.Font = New System.Drawing.Font("Courier New", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBankID.Location = New System.Drawing.Point(109, 28)
        Me.lblBankID.Name = "lblBankID"
        Me.lblBankID.Size = New System.Drawing.Size(183, 14)
        Me.lblBankID.TabIndex = 14
        '
        'Prg
        '
        Me.Prg.Location = New System.Drawing.Point(303, 143)
        Me.Prg.Name = "Prg"
        Me.Prg.Size = New System.Drawing.Size(100, 23)
        Me.Prg.TabIndex = 15
        '
        'frmIncoming
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = Global.BRClearing.Common.My.Resources.Resources.log_img6
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(418, 45)
        Me.Controls.Add(Me.Prg)
        Me.Controls.Add(Me.lblBankID)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.prgIncomingImages)
        Me.Controls.Add(Me.PictureBox3)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.DLDrivePath)
        Me.Controls.Add(Me.FLItems)
        Me.Controls.Add(Me.DLDirPath)
        Me.Controls.Add(Me.FileProgress)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.btnRead)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Name = "frmIncoming"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnRead As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents FileProgress As System.Windows.Forms.ProgressBar
    Friend WithEvents DLDirPath As Microsoft.VisualBasic.Compatibility.VB6.DirListBox
    Friend WithEvents FLItems As Microsoft.VisualBasic.Compatibility.VB6.FileListBox
    Friend WithEvents DLDrivePath As Microsoft.VisualBasic.Compatibility.VB6.DriveListBox
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox3 As System.Windows.Forms.PictureBox
    Friend WithEvents prgIncomingImages As System.Windows.Forms.ProgressBar
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblBankID As System.Windows.Forms.Label
    Friend WithEvents Prg As System.Windows.Forms.ProgressBar
End Class
