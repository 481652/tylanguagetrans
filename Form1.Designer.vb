<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        txtMain = New TextBox()
        grpMain = New GroupBox()
        Label2 = New Label()
        Button2 = New Button()
        Button1 = New Button()
        Label1 = New Label()
        ListBox1 = New ListBox()
        grpControls = New GroupBox()
        Button3 = New Button()
        RadioButton2 = New RadioButton()
        Label3 = New Label()
        RadioButton1 = New RadioButton()
        btnEncrypt = New Button()
        btnDecrypt = New Button()
        btnCopy = New Button()
        btnClear = New Button()
        chkCompress = New CheckBox()
        grpKeys = New GroupBox()
        Button5 = New Button()
        Button4 = New Button()
        Label4 = New Label()
        btnGenKeys = New Button()
        lblWarn = New Label()
        StatusStrip1 = New StatusStrip()
        Statuslbl = New ToolStripStatusLabel()
        LinkLabel1 = New LinkLabel()
        grpMain.SuspendLayout()
        grpControls.SuspendLayout()
        grpKeys.SuspendLayout()
        StatusStrip1.SuspendLayout()
        SuspendLayout()
        ' 
        ' txtMain
        ' 
        txtMain.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        txtMain.Location = New Point(25, 31)
        txtMain.Multiline = True
        txtMain.Name = "txtMain"
        txtMain.ScrollBars = ScrollBars.Vertical
        txtMain.Size = New Size(466, 276)
        txtMain.TabIndex = 0
        ' 
        ' grpMain
        ' 
        grpMain.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left
        grpMain.Controls.Add(Label2)
        grpMain.Controls.Add(txtMain)
        grpMain.Location = New Point(205, 12)
        grpMain.Name = "grpMain"
        grpMain.Size = New Size(510, 330)
        grpMain.TabIndex = 1
        grpMain.TabStop = False
        grpMain.Text = "内容管理"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(381, 310)
        Label2.Name = "Label2"
        Label2.Size = New Size(63, 17)
        Label2.TabIndex = 5
        Label2.Text = "字符数：0"
        ' 
        ' Button2
        ' 
        Button2.Location = New Point(100, 170)
        Button2.Name = "Button2"
        Button2.Size = New Size(71, 27)
        Button2.TabIndex = 4
        Button2.Text = "删除会话"
        Button2.UseVisualStyleBackColor = True
        ' 
        ' Button1
        ' 
        Button1.Location = New Point(20, 170)
        Button1.Name = "Button1"
        Button1.Size = New Size(71, 27)
        Button1.TabIndex = 3
        Button1.Text = "保存会话"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(12, 21)
        Label1.Name = "Label1"
        Label1.Size = New Size(32, 17)
        Label1.TabIndex = 2
        Label1.Text = "会话"
        ' 
        ' ListBox1
        ' 
        ListBox1.FormattingEnabled = True
        ListBox1.ItemHeight = 17
        ListBox1.Items.AddRange(New Object() {"未命名会话"})
        ListBox1.Location = New Point(17, 41)
        ListBox1.Name = "ListBox1"
        ListBox1.Size = New Size(154, 123)
        ListBox1.TabIndex = 1
        ' 
        ' grpControls
        ' 
        grpControls.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        grpControls.Controls.Add(Button3)
        grpControls.Controls.Add(RadioButton2)
        grpControls.Controls.Add(Label3)
        grpControls.Controls.Add(RadioButton1)
        grpControls.Controls.Add(btnEncrypt)
        grpControls.Controls.Add(btnDecrypt)
        grpControls.Controls.Add(btnCopy)
        grpControls.Controls.Add(btnClear)
        grpControls.Controls.Add(chkCompress)
        grpControls.Location = New Point(721, 12)
        grpControls.Name = "grpControls"
        grpControls.Size = New Size(129, 330)
        grpControls.TabIndex = 2
        grpControls.TabStop = False
        grpControls.Text = "操作"
        ' 
        ' Button3
        ' 
        Button3.Location = New Point(6, 288)
        Button3.Name = "Button3"
        Button3.Size = New Size(118, 35)
        Button3.TabIndex = 8
        Button3.Text = "保存预设"
        Button3.UseVisualStyleBackColor = True
        ' 
        ' RadioButton2
        ' 
        RadioButton2.AutoSize = True
        RadioButton2.Location = New Point(5, 228)
        RadioButton2.Name = "RadioButton2"
        RadioButton2.Size = New Size(91, 21)
        RadioButton2.TabIndex = 7
        RadioButton2.Text = "2.2旧版算法"
        RadioButton2.UseVisualStyleBackColor = True
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(6, 183)
        Label3.Name = "Label3"
        Label3.Size = New Size(56, 17)
        Label3.TabIndex = 6
        Label3.Text = "算法选择"
        ' 
        ' RadioButton1
        ' 
        RadioButton1.AutoSize = True
        RadioButton1.Checked = True
        RadioButton1.Location = New Point(6, 203)
        RadioButton1.Name = "RadioButton1"
        RadioButton1.Size = New Size(91, 21)
        RadioButton1.TabIndex = 5
        RadioButton1.TabStop = True
        RadioButton1.Text = "3.0全新算法"
        RadioButton1.UseVisualStyleBackColor = True
        ' 
        ' btnEncrypt
        ' 
        btnEncrypt.Location = New Point(6, 25)
        btnEncrypt.Name = "btnEncrypt"
        btnEncrypt.Size = New Size(118, 35)
        btnEncrypt.TabIndex = 0
        btnEncrypt.Text = "加密"
        ' 
        ' btnDecrypt
        ' 
        btnDecrypt.Location = New Point(6, 64)
        btnDecrypt.Name = "btnDecrypt"
        btnDecrypt.Size = New Size(118, 35)
        btnDecrypt.TabIndex = 1
        btnDecrypt.Text = "解密"
        ' 
        ' btnCopy
        ' 
        btnCopy.Location = New Point(6, 104)
        btnCopy.Name = "btnCopy"
        btnCopy.Size = New Size(118, 35)
        btnCopy.TabIndex = 2
        btnCopy.Text = "复制结果"
        ' 
        ' btnClear
        ' 
        btnClear.Location = New Point(6, 145)
        btnClear.Name = "btnClear"
        btnClear.Size = New Size(118, 35)
        btnClear.TabIndex = 3
        btnClear.Text = "清空"
        ' 
        ' chkCompress
        ' 
        chkCompress.Enabled = False
        chkCompress.Location = New Point(6, 258)
        chkCompress.Name = "chkCompress"
        chkCompress.Size = New Size(104, 24)
        chkCompress.TabIndex = 4
        chkCompress.Text = "压缩密文长度"
        ' 
        ' grpKeys
        ' 
        grpKeys.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        grpKeys.Controls.Add(Button5)
        grpKeys.Controls.Add(Button4)
        grpKeys.Controls.Add(Label4)
        grpKeys.Controls.Add(btnGenKeys)
        grpKeys.Controls.Add(Button2)
        grpKeys.Controls.Add(Button1)
        grpKeys.Controls.Add(Label1)
        grpKeys.Controls.Add(ListBox1)
        grpKeys.Location = New Point(12, 12)
        grpKeys.Name = "grpKeys"
        grpKeys.Size = New Size(188, 330)
        grpKeys.TabIndex = 3
        grpKeys.TabStop = False
        grpKeys.Text = "密钥管理"
        ' 
        ' Button5
        ' 
        Button5.Location = New Point(21, 239)
        Button5.Name = "Button5"
        Button5.Size = New Size(147, 27)
        Button5.TabIndex = 7
        Button5.Text = "导出会话"
        Button5.UseVisualStyleBackColor = True
        ' 
        ' Button4
        ' 
        Button4.Location = New Point(22, 206)
        Button4.Name = "Button4"
        Button4.Size = New Size(147, 27)
        Button4.TabIndex = 6
        Button4.Text = "加入会话"
        Button4.UseVisualStyleBackColor = True
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(12, 271)
        Label4.Name = "Label4"
        Label4.Size = New Size(32, 17)
        Label4.TabIndex = 5
        Label4.Text = "密钥"
        ' 
        ' btnGenKeys
        ' 
        btnGenKeys.Location = New Point(21, 291)
        btnGenKeys.Name = "btnGenKeys"
        btnGenKeys.Size = New Size(150, 28)
        btnGenKeys.TabIndex = 0
        btnGenKeys.Text = "添加密钥对"
        ' 
        ' lblWarn
        ' 
        lblWarn.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        lblWarn.AutoSize = True
        lblWarn.ForeColor = Color.Red
        lblWarn.Location = New Point(432, 363)
        lblWarn.Name = "lblWarn"
        lblWarn.Size = New Size(421, 17)
        lblWarn.TabIndex = 4
        lblWarn.Text = "注意：本版本只兼容2.2版本。本工具严禁用于违法活动，违者后果自行承担！"
        ' 
        ' StatusStrip1
        ' 
        StatusStrip1.Items.AddRange(New ToolStripItem() {Statuslbl})
        StatusStrip1.Location = New Point(0, 389)
        StatusStrip1.Name = "StatusStrip1"
        StatusStrip1.Size = New Size(864, 22)
        StatusStrip1.TabIndex = 5
        StatusStrip1.Text = "StatusStrip1"
        ' 
        ' Statuslbl
        ' 
        Statuslbl.Name = "Statuslbl"
        Statuslbl.Size = New Size(32, 17)
        Statuslbl.Text = "就绪"
        ' 
        ' LinkLabel1
        ' 
        LinkLabel1.ActiveLinkColor = Color.Blue
        LinkLabel1.AutoSize = True
        LinkLabel1.LinkColor = Color.FromArgb(CByte(94), CByte(80), CByte(206))
        LinkLabel1.Location = New Point(12, 363)
        LinkLabel1.Name = "LinkLabel1"
        LinkLabel1.Size = New Size(208, 17)
        LinkLabel1.TabIndex = 6
        LinkLabel1.TabStop = True
        LinkLabel1.Text = "By LCS 2020-2026（点我检查更新）"
        LinkLabel1.VisitedLinkColor = Color.FromArgb(CByte(94), CByte(80), CByte(206))
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(96.0F, 96.0F)
        AutoScaleMode = AutoScaleMode.Dpi
        ClientSize = New Size(864, 411)
        Controls.Add(LinkLabel1)
        Controls.Add(StatusStrip1)
        Controls.Add(lblWarn)
        Controls.Add(grpKeys)
        Controls.Add(grpControls)
        Controls.Add(grpMain)
        Font = New Font("微软雅黑", 9.0F)
        ForeColor = SystemColors.ControlText
        FormBorderStyle = FormBorderStyle.FixedSingle
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        MaximizeBox = False
        Name = "Form1"
        Text = "TY语加密器3.0"
        grpMain.ResumeLayout(False)
        grpMain.PerformLayout()
        grpControls.ResumeLayout(False)
        grpControls.PerformLayout()
        grpKeys.ResumeLayout(False)
        grpKeys.PerformLayout()
        StatusStrip1.ResumeLayout(False)
        StatusStrip1.PerformLayout()
        ResumeLayout(False)
        PerformLayout()

    End Sub

    Friend WithEvents txtMain As TextBox
    Friend WithEvents grpMain As GroupBox
    Friend WithEvents grpControls As GroupBox
    Friend WithEvents btnEncrypt As Button
    Friend WithEvents btnDecrypt As Button
    Friend WithEvents btnCopy As Button
    Friend WithEvents btnClear As Button
    Friend WithEvents chkCompress As CheckBox
    Friend WithEvents grpKeys As GroupBox
    Friend WithEvents btnGenKeys As Button
    Friend WithEvents lblWarn As Label
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents Statuslbl As ToolStripStatusLabel
    Friend WithEvents Label2 As Label
    Friend WithEvents Button2 As Button
    Friend WithEvents Button1 As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents ListBox1 As ListBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents RadioButton1 As RadioButton
    Friend WithEvents RadioButton2 As RadioButton
    Friend WithEvents Button3 As Button
    Friend WithEvents LinkLabel1 As LinkLabel
    Friend WithEvents Button4 As Button
    Friend WithEvents Button5 As Button
End Class
