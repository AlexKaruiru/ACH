Public Class ConfirmationForm
    Inherits System.Windows.Forms.Form

    Protected m_objConfig As Properties = Nothing

#Region " Create code by Windwos Form Designer "

    Public Sub New()
        MyBase.New()

        'Required by the Windows Form Designer
        InitializeComponent()

        ' InitializeComponent() 
        m_objConfig = New Properties

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents CheckBoxStamp As System.Windows.Forms.CheckBox
    Friend WithEvents ComboBoxEjectType As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ButtonOK As System.Windows.Forms.Button
    Friend WithEvents ButtonCancel As System.Windows.Forms.Button
    Friend WithEvents ComboBoxNextCheck As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(ConfirmationForm))
        Me.ButtonOK = New System.Windows.Forms.Button
        Me.ButtonCancel = New System.Windows.Forms.Button
        Me.CheckBoxStamp = New System.Windows.Forms.CheckBox
        Me.ComboBoxEjectType = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.ComboBoxNextCheck = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'ButtonOK
        '
        Me.ButtonOK.Location = New System.Drawing.Point(40, 120)
        Me.ButtonOK.Name = "ButtonOK"
        Me.ButtonOK.TabIndex = 4
        Me.ButtonOK.Text = "OK"
        '
        'ButtonCancel
        '
        Me.ButtonCancel.Location = New System.Drawing.Point(184, 120)
        Me.ButtonCancel.Name = "ButtonCancel"
        Me.ButtonCancel.TabIndex = 5
        Me.ButtonCancel.Text = "Cancel"
        '
        'CheckBoxStamp
        '
        Me.CheckBoxStamp.Location = New System.Drawing.Point(112, 48)
        Me.CheckBoxStamp.Name = "CheckBoxStamp"
        Me.CheckBoxStamp.TabIndex = 1
        Me.CheckBoxStamp.Text = "Stamp"
        '
        'ComboBoxEjectType
        '
        Me.ComboBoxEjectType.Items.AddRange(New Object() {"MAIN", "SUB"})
        Me.ComboBoxEjectType.Location = New System.Drawing.Point(112, 16)
        Me.ComboBoxEjectType.Name = "ComboBoxEjectType"
        Me.ComboBoxEjectType.Size = New System.Drawing.Size(121, 20)
        Me.ComboBoxEjectType.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(24, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(80, 23)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Eject Type"
        '
        'ComboBoxNextCheck
        '
        Me.ComboBoxNextCheck.Items.AddRange(New Object() {"OVERLAP", "NOOVERLAP", "CANCEL"})
        Me.ComboBoxNextCheck.Location = New System.Drawing.Point(112, 80)
        Me.ComboBoxNextCheck.Name = "ComboBoxNextCheck"
        Me.ComboBoxNextCheck.Size = New System.Drawing.Size(121, 20)
        Me.ComboBoxNextCheck.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(24, 80)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(80, 23)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Next Check"
        '
        'ConfirmationForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 12)
        Me.ClientSize = New System.Drawing.Size(292, 158)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.ComboBoxNextCheck)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ComboBoxEjectType)
        Me.Controls.Add(Me.ButtonOK)
        Me.Controls.Add(Me.ButtonCancel)
        Me.Controls.Add(Me.CheckBoxStamp)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "ConfirmationForm"
        Me.Text = "ConfirmationForm"
        Me.ResumeLayout(False)

    End Sub

#End Region

    ' input/output of the property  
    Public Property Proc() As Properties
        Get
            Return New Properties(m_objConfig)
        End Get
        Set(ByVal Value As Properties)
            m_objConfig = New Properties(Value)
            LoadProperties()
        End Set
    End Property

    ' this method is called when the user click the OK button
    Private Sub ButtonOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
        m_objConfig(Properties.CONF_EJECT) = ComboBoxEjectType.SelectedIndex
        m_objConfig(Properties.CONF_STAMP) = CheckBoxStamp.Checked
        m_objConfig(Properties.CONF_NEXT_CHECK) = ComboBoxNextCheck.SelectedIndex
        m_objConfig(Properties.CONF_OK) = True
        Me.Close()
    End Sub

    ' this method is called when the user click the Cancel button
    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        m_objConfig(Properties.CONF_OK) = False
        Me.Close()
    End Sub

    ' this method is property is bound to the  Each value
    Private Sub LoadProperties()
        ComboBoxEjectType.SelectedIndex = m_objConfig(Properties.CONF_EJECT)
        CheckBoxStamp.Checked = m_objConfig(Properties.CONF_STAMP)
        ComboBoxNextCheck.SelectedIndex = m_objConfig(Properties.CONF_NEXT_CHECK)
    End Sub

    ' set error code
    Public Sub SetError(ByVal strErr As String)
        Me.Text = strErr
    End Sub
End Class
