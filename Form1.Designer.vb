<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.txtMain = New System.Windows.Forms.TextBox()
        Me.grpMain = New System.Windows.Forms.GroupBox()
        Me.grpControls = New System.Windows.Forms.GroupBox()
        Me.btnEncrypt = New System.Windows.Forms.Button()
        Me.btnDecrypt = New System.Windows.Forms.Button()
        Me.btnCopy = New System.Windows.Forms.Button()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.chkCompress = New System.Windows.Forms.CheckBox()
        Me.grpKeys = New System.Windows.Forms.GroupBox()
        Me.btnDetectKeys = New System.Windows.Forms.Button()
        Me.btnGenKeys = New System.Windows.Forms.Button()
        Me.btnImportPub = New System.Windows.Forms.Button()
        Me.btnImportPriv = New System.Windows.Forms.Button()
        Me.lblFooter = New System.Windows.Forms.Label()
        Me.lblWarn = New System.Windows.Forms.Label()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.Statuslbl = New System.Windows.Forms.ToolStripStatusLabel()
        Me.grpMain.SuspendLayout()
        Me.grpControls.SuspendLayout()
        Me.grpKeys.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtMain
        '
        Me.txtMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMain.Location = New System.Drawing.Point(10, 22)
        Me.txtMain.Multiline = True
        Me.txtMain.Name = "txtMain"
        Me.txtMain.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtMain.Size = New System.Drawing.Size(600, 300)
        Me.txtMain.TabIndex = 0
        '
        'grpMain
        '
        Me.grpMain.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.grpMain.Controls.Add(Me.txtMain)
        Me.grpMain.Location = New System.Drawing.Point(12, 12)
        Me.grpMain.Name = "grpMain"
        Me.grpMain.Size = New System.Drawing.Size(620, 330)
        Me.grpMain.TabIndex = 1
        Me.grpMain.TabStop = False
        Me.grpMain.Text = "明文 / 密文"
        '
        'grpControls
        '
        Me.grpControls.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpControls.Controls.Add(Me.btnEncrypt)
        Me.grpControls.Controls.Add(Me.btnDecrypt)
        Me.grpControls.Controls.Add(Me.btnCopy)
        Me.grpControls.Controls.Add(Me.btnClear)
        Me.grpControls.Controls.Add(Me.chkCompress)
        Me.grpControls.Location = New System.Drawing.Point(650, 12)
        Me.grpControls.Name = "grpControls"
        Me.grpControls.Size = New System.Drawing.Size(200, 200)
        Me.grpControls.TabIndex = 2
        Me.grpControls.TabStop = False
        Me.grpControls.Text = "操作"
        '
        'btnEncrypt
        '
        Me.btnEncrypt.Location = New System.Drawing.Point(20, 25)
        Me.btnEncrypt.Name = "btnEncrypt"
        Me.btnEncrypt.Size = New System.Drawing.Size(150, 35)
        Me.btnEncrypt.TabIndex = 0
        Me.btnEncrypt.Text = "加密"
        '
        'btnDecrypt
        '
        Me.btnDecrypt.Location = New System.Drawing.Point(20, 65)
        Me.btnDecrypt.Name = "btnDecrypt"
        Me.btnDecrypt.Size = New System.Drawing.Size(150, 35)
        Me.btnDecrypt.TabIndex = 1
        Me.btnDecrypt.Text = "解密"
        '
        'btnCopy
        '
        Me.btnCopy.Location = New System.Drawing.Point(20, 105)
        Me.btnCopy.Name = "btnCopy"
        Me.btnCopy.Size = New System.Drawing.Size(150, 35)
        Me.btnCopy.TabIndex = 2
        Me.btnCopy.Text = "复制结果"
        '
        'btnClear
        '
        Me.btnClear.Location = New System.Drawing.Point(20, 145)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(150, 35)
        Me.btnClear.TabIndex = 3
        Me.btnClear.Text = "清空"
        '
        'chkCompress
        '
        Me.chkCompress.Location = New System.Drawing.Point(20, 180)
        Me.chkCompress.Name = "chkCompress"
        Me.chkCompress.Size = New System.Drawing.Size(104, 24)
        Me.chkCompress.TabIndex = 4
        Me.chkCompress.Text = "压缩密文长度"
        '
        'grpKeys
        '
        Me.grpKeys.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpKeys.Controls.Add(Me.btnDetectKeys)
        Me.grpKeys.Controls.Add(Me.btnGenKeys)
        Me.grpKeys.Controls.Add(Me.btnImportPub)
        Me.grpKeys.Controls.Add(Me.btnImportPriv)
        Me.grpKeys.Location = New System.Drawing.Point(650, 220)
        Me.grpKeys.Name = "grpKeys"
        Me.grpKeys.Size = New System.Drawing.Size(200, 122)
        Me.grpKeys.TabIndex = 3
        Me.grpKeys.TabStop = False
        Me.grpKeys.Text = "密钥管理"
        '
        'btnDetectKeys
        '
        Me.btnDetectKeys.Location = New System.Drawing.Point(20, 86)
        Me.btnDetectKeys.Name = "btnDetectKeys"
        Me.btnDetectKeys.Size = New System.Drawing.Size(150, 28)
        Me.btnDetectKeys.TabIndex = 3
        Me.btnDetectKeys.Text = "导入现有密钥"
        '
        'btnGenKeys
        '
        Me.btnGenKeys.Location = New System.Drawing.Point(20, 25)
        Me.btnGenKeys.Name = "btnGenKeys"
        Me.btnGenKeys.Size = New System.Drawing.Size(150, 28)
        Me.btnGenKeys.TabIndex = 0
        Me.btnGenKeys.Text = "生成密钥对"
        '
        'btnImportPub
        '
        Me.btnImportPub.Location = New System.Drawing.Point(20, 55)
        Me.btnImportPub.Name = "btnImportPub"
        Me.btnImportPub.Size = New System.Drawing.Size(73, 28)
        Me.btnImportPub.TabIndex = 1
        Me.btnImportPub.Text = "导入公钥"
        '
        'btnImportPriv
        '
        Me.btnImportPriv.Location = New System.Drawing.Point(100, 55)
        Me.btnImportPriv.Name = "btnImportPriv"
        Me.btnImportPriv.Size = New System.Drawing.Size(70, 28)
        Me.btnImportPriv.TabIndex = 2
        Me.btnImportPriv.Text = "导入私钥"
        '
        'lblFooter
        '
        Me.lblFooter.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblFooter.AutoSize = True
        Me.lblFooter.Location = New System.Drawing.Point(12, 345)
        Me.lblFooter.Name = "lblFooter"
        Me.lblFooter.Size = New System.Drawing.Size(112, 17)
        Me.lblFooter.TabIndex = 0
        Me.lblFooter.Text = "By LCS 2020-2025"
        '
        'lblWarn
        '
        Me.lblWarn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblWarn.AutoSize = True
        Me.lblWarn.ForeColor = System.Drawing.Color.Red
        Me.lblWarn.Location = New System.Drawing.Point(388, 345)
        Me.lblWarn.Name = "lblWarn"
        Me.lblWarn.Size = New System.Drawing.Size(464, 17)
        Me.lblWarn.TabIndex = 4
        Me.lblWarn.Text = "注意：本版本的加密和压缩算法与老版不同，不兼容老版！本工具严禁用于违法活动！"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.Statuslbl})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 389)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(864, 22)
        Me.StatusStrip1.TabIndex = 5
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'Statuslbl
        '
        Me.Statuslbl.Name = "Statuslbl"
        Me.Statuslbl.Size = New System.Drawing.Size(32, 17)
        Me.Statuslbl.Text = "就绪"
        '
        'Form1
        '
        Me.ClientSize = New System.Drawing.Size(864, 411)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.lblWarn)
        Me.Controls.Add(Me.lblFooter)
        Me.Controls.Add(Me.grpKeys)
        Me.Controls.Add(Me.grpControls)
        Me.Controls.Add(Me.grpMain)
        Me.Font = New System.Drawing.Font("微软雅黑", 9.0!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "Form1"
        Me.Text = "TY语加密器2.2"
        Me.grpMain.ResumeLayout(False)
        Me.grpMain.PerformLayout()
        Me.grpControls.ResumeLayout(False)
        Me.grpKeys.ResumeLayout(False)
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

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
    Friend WithEvents btnImportPub As Button
    Friend WithEvents btnImportPriv As Button
    Friend WithEvents lblFooter As Label
    Friend WithEvents lblWarn As Label
    Friend WithEvents btnDetectKeys As Button
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents Statuslbl As ToolStripStatusLabel
End Class
