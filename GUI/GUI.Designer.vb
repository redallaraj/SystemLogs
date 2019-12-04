<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class GUI
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
        Me.btn_save = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txt_username = New System.Windows.Forms.TextBox()
        Me.txt_fullname = New System.Windows.Forms.TextBox()
        Me.txt_machineid = New System.Windows.Forms.TextBox()
        Me.txt_ip = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'btn_save
        '
        Me.btn_save.BackColor = System.Drawing.Color.Lime
        Me.btn_save.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_save.ForeColor = System.Drawing.Color.DarkGreen
        Me.btn_save.Location = New System.Drawing.Point(199, 327)
        Me.btn_save.Name = "btn_save"
        Me.btn_save.Size = New System.Drawing.Size(153, 43)
        Me.btn_save.TabIndex = 17
        Me.btn_save.Text = "SAVE"
        Me.btn_save.UseVisualStyleBackColor = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(96, 245)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(87, 16)
        Me.Label4.TabIndex = 16
        Me.Label4.Text = "Username :"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(96, 173)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(86, 16)
        Me.Label3.TabIndex = 15
        Me.Label3.Text = "Full Name :"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(96, 100)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(91, 16)
        Me.Label2.TabIndex = 14
        Me.Label2.Text = "Machine Id :"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(96, 31)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(30, 16)
        Me.Label1.TabIndex = 13
        Me.Label1.Text = "IP :"
        '
        'txt_username
        '
        Me.txt_username.Location = New System.Drawing.Point(282, 245)
        Me.txt_username.Name = "txt_username"
        Me.txt_username.Size = New System.Drawing.Size(203, 20)
        Me.txt_username.TabIndex = 12
        '
        'txt_fullname
        '
        Me.txt_fullname.Location = New System.Drawing.Point(282, 172)
        Me.txt_fullname.Name = "txt_fullname"
        Me.txt_fullname.Size = New System.Drawing.Size(203, 20)
        Me.txt_fullname.TabIndex = 11
        '
        'txt_machineid
        '
        Me.txt_machineid.Location = New System.Drawing.Point(282, 99)
        Me.txt_machineid.Name = "txt_machineid"
        Me.txt_machineid.ReadOnly = True
        Me.txt_machineid.Size = New System.Drawing.Size(203, 20)
        Me.txt_machineid.TabIndex = 10
        '
        'txt_ip
        '
        Me.txt_ip.Location = New System.Drawing.Point(282, 30)
        Me.txt_ip.Name = "txt_ip"
        Me.txt_ip.ReadOnly = True
        Me.txt_ip.Size = New System.Drawing.Size(203, 20)
        Me.txt_ip.TabIndex = 9
        '
        'GUI
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(581, 400)
        Me.Controls.Add(Me.btn_save)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txt_username)
        Me.Controls.Add(Me.txt_fullname)
        Me.Controls.Add(Me.txt_machineid)
        Me.Controls.Add(Me.txt_ip)
        Me.Name = "GUI"
        Me.Text = "GUI"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btn_save As Button
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents txt_username As TextBox
    Friend WithEvents txt_fullname As TextBox
    Friend WithEvents txt_machineid As TextBox
    Friend WithEvents txt_ip As TextBox
End Class
