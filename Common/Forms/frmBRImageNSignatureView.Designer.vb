<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBRImageNSignatureView
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBRImageNSignatureView))
        Me.pictMainFront = New CSI_ImageControl.ImageControl()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblMandate = New System.Windows.Forms.Label()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.pictMainFront3 = New CSI_ImageControl.ImageControl()
        Me.pictFrontBW = New System.Windows.Forms.PictureBox()
        Me.pictFrontGrayScale = New System.Windows.Forms.PictureBox()
        Me.pictFrontRear = New System.Windows.Forms.PictureBox()
        Me.cmdZoom = New System.Windows.Forms.Button()
        Me.cmdFitWdth = New System.Windows.Forms.Button()
        Me.CmdFitHT = New System.Windows.Forms.Button()
        Me.btnNxtSign = New System.Windows.Forms.Button()
        CType(Me.pictFrontBW, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pictFrontGrayScale, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pictFrontRear, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pictMainFront
        '
        Me.pictMainFront.BackColor = System.Drawing.Color.FromArgb(CType(CType(237, Byte), Integer), CType(CType(246, Byte), Integer), CType(CType(251, Byte), Integer))
        Me.pictMainFront.Image = Nothing
        Me.pictMainFront.initialimage = Nothing
        Me.pictMainFront.Location = New System.Drawing.Point(11, 53)
        Me.pictMainFront.Name = "pictMainFront"
        Me.pictMainFront.Origin = New System.Drawing.Point(0, 0)
        Me.pictMainFront.PanButton = System.Windows.Forms.MouseButtons.Left
        Me.pictMainFront.PanMode = True
        Me.pictMainFront.ScrollbarsVisible = True
        Me.pictMainFront.Size = New System.Drawing.Size(547, 227)
        Me.pictMainFront.StretchImageToFit = False
        Me.pictMainFront.TabIndex = 28
        Me.pictMainFront.ZoomFactor = 1.0R
        Me.pictMainFront.ZoomOnMouseWheel = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(23, 14)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(68, 16)
        Me.Label1.TabIndex = 30
        Me.Label1.Text = "Mandate"
        '
        'lblMandate
        '
        Me.lblMandate.BackColor = System.Drawing.Color.Transparent
        Me.lblMandate.Location = New System.Drawing.Point(120, 15)
        Me.lblMandate.Name = "lblMandate"
        Me.lblMandate.Size = New System.Drawing.Size(352, 25)
        Me.lblMandate.TabIndex = 31
        '
        'btnExit
        '
        Me.btnExit.BackColor = System.Drawing.Color.WhiteSmoke
        Me.btnExit.Location = New System.Drawing.Point(564, 477)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(95, 33)
        Me.btnExit.TabIndex = 32
        Me.btnExit.Text = "Exit"
        Me.btnExit.UseVisualStyleBackColor = False
        '
        'pictMainFront3
        '
        Me.pictMainFront3.BackColor = System.Drawing.Color.FromArgb(CType(CType(237, Byte), Integer), CType(CType(246, Byte), Integer), CType(CType(251, Byte), Integer))
        Me.pictMainFront3.Image = Nothing
        Me.pictMainFront3.initialimage = Nothing
        Me.pictMainFront3.Location = New System.Drawing.Point(11, 291)
        Me.pictMainFront3.Name = "pictMainFront3"
        Me.pictMainFront3.Origin = New System.Drawing.Point(0, 0)
        Me.pictMainFront3.PanButton = System.Windows.Forms.MouseButtons.Left
        Me.pictMainFront3.PanMode = True
        Me.pictMainFront3.ScrollbarsVisible = True
        Me.pictMainFront3.Size = New System.Drawing.Size(547, 242)
        Me.pictMainFront3.StretchImageToFit = False
        Me.pictMainFront3.TabIndex = 34
        Me.pictMainFront3.ZoomFactor = 1.0R
        Me.pictMainFront3.ZoomOnMouseWheel = True
        '
        'pictFrontBW
        '
        Me.pictFrontBW.BackColor = System.Drawing.Color.Transparent
        Me.pictFrontBW.Location = New System.Drawing.Point(564, 53)
        Me.pictFrontBW.Name = "pictFrontBW"
        Me.pictFrontBW.Size = New System.Drawing.Size(100, 50)
        Me.pictFrontBW.TabIndex = 35
        Me.pictFrontBW.TabStop = False
        '
        'pictFrontGrayScale
        '
        Me.pictFrontGrayScale.BackColor = System.Drawing.Color.Transparent
        Me.pictFrontGrayScale.Location = New System.Drawing.Point(564, 118)
        Me.pictFrontGrayScale.Name = "pictFrontGrayScale"
        Me.pictFrontGrayScale.Size = New System.Drawing.Size(100, 50)
        Me.pictFrontGrayScale.TabIndex = 36
        Me.pictFrontGrayScale.TabStop = False
        '
        'pictFrontRear
        '
        Me.pictFrontRear.BackColor = System.Drawing.Color.Transparent
        Me.pictFrontRear.Location = New System.Drawing.Point(564, 183)
        Me.pictFrontRear.Name = "pictFrontRear"
        Me.pictFrontRear.Size = New System.Drawing.Size(100, 50)
        Me.pictFrontRear.TabIndex = 37
        Me.pictFrontRear.TabStop = False
        '
        'cmdZoom
        '
        Me.cmdZoom.BackColor = System.Drawing.Color.WhiteSmoke
        Me.cmdZoom.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdZoom.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdZoom.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdZoom.Location = New System.Drawing.Point(564, 343)
        Me.cmdZoom.Name = "cmdZoom"
        Me.cmdZoom.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdZoom.Size = New System.Drawing.Size(97, 34)
        Me.cmdZoom.TabIndex = 38
        Me.cmdZoom.Text = "Zoom &In"
        Me.cmdZoom.UseVisualStyleBackColor = False
        '
        'cmdFitWdth
        '
        Me.cmdFitWdth.BackColor = System.Drawing.Color.WhiteSmoke
        Me.cmdFitWdth.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdFitWdth.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFitWdth.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFitWdth.Location = New System.Drawing.Point(564, 383)
        Me.cmdFitWdth.Name = "cmdFitWdth"
        Me.cmdFitWdth.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFitWdth.Size = New System.Drawing.Size(97, 34)
        Me.cmdFitWdth.TabIndex = 39
        Me.cmdFitWdth.Text = "Zoom &Out"
        Me.cmdFitWdth.UseVisualStyleBackColor = False
        '
        'CmdFitHT
        '
        Me.CmdFitHT.BackColor = System.Drawing.Color.WhiteSmoke
        Me.CmdFitHT.Cursor = System.Windows.Forms.Cursors.Default
        Me.CmdFitHT.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdFitHT.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdFitHT.Location = New System.Drawing.Point(564, 423)
        Me.CmdFitHT.Name = "CmdFitHT"
        Me.CmdFitHT.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CmdFitHT.Size = New System.Drawing.Size(97, 34)
        Me.CmdFitHT.TabIndex = 40
        Me.CmdFitHT.Text = "&Fit"
        Me.CmdFitHT.UseVisualStyleBackColor = False
        '
        'btnNxtSign
        '
        Me.btnNxtSign.Location = New System.Drawing.Point(564, 291)
        Me.btnNxtSign.Name = "btnNxtSign"
        Me.btnNxtSign.Size = New System.Drawing.Size(95, 46)
        Me.btnNxtSign.TabIndex = 41
        Me.btnNxtSign.Text = "Next Signature"
        Me.btnNxtSign.UseVisualStyleBackColor = True
        '
        'frmBRImageNSignatureView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(237, Byte), Integer), CType(CType(246, Byte), Integer), CType(CType(251, Byte), Integer))
        Me.BackgroundImage = Global.BrClearing.Common.My.Resources.Resources.log_img6
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(672, 538)
        Me.Controls.Add(Me.btnNxtSign)
        Me.Controls.Add(Me.cmdZoom)
        Me.Controls.Add(Me.cmdFitWdth)
        Me.Controls.Add(Me.CmdFitHT)
        Me.Controls.Add(Me.pictFrontRear)
        Me.Controls.Add(Me.pictFrontGrayScale)
        Me.Controls.Add(Me.pictFrontBW)
        Me.Controls.Add(Me.pictMainFront3)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.lblMandate)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.pictMainFront)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "frmBRImageNSignatureView"
        Me.Text = "BR Image and Signature Viewer"
        CType(Me.pictFrontBW, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pictFrontGrayScale, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pictFrontRear, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pictMainFront As CSI_ImageControl.ImageControl
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblMandate As System.Windows.Forms.Label
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents pictMainFront3 As CSI_ImageControl.ImageControl
    Friend WithEvents pictFrontBW As System.Windows.Forms.PictureBox
    Friend WithEvents pictFrontGrayScale As System.Windows.Forms.PictureBox
    Friend WithEvents pictFrontRear As System.Windows.Forms.PictureBox
    Public WithEvents cmdZoom As System.Windows.Forms.Button
    Public WithEvents cmdFitWdth As System.Windows.Forms.Button
    Public WithEvents CmdFitHT As System.Windows.Forms.Button
    Friend WithEvents btnNxtSign As System.Windows.Forms.Button
End Class
