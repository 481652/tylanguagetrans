Imports System.IO
Imports System.IO.Compression
Imports System.Net.Http
Imports System.Security.Cryptography
Imports System.Text
Imports System.Text.Json
Imports Org.BouncyCastle.Asn1
Imports Org.BouncyCastle.Asn1.Pkcs
Imports Org.BouncyCastle.Asn1.X509
Imports Org.BouncyCastle.Crypto
Imports Org.BouncyCastle.Crypto.Generators
Imports Org.BouncyCastle.Crypto.Parameters
Imports Org.BouncyCastle.Pkcs
Imports Org.BouncyCastle.Security
Imports Org.BouncyCastle.X509
Imports tylanguagetrans.My

Public Class Form1

    Private Const MaxSessionFileBytes As Long = 4L * 1024 * 1024
    Private Const MaxUpdateDownloadBytes As Long = 512L * 1024 * 1024
    Private Const MaxUpdateExtractedBytes As Long = 1024L * 1024 * 1024
    Private Const MaxUpdateEntries As Integer = 2048

    Private Class SessionInfo
        Public Property Name As String
        Public Property PublicXml As String
        Public Property PrivateXml As String
        Public Property AlgorithmType As Integer
        Public Property DoUseCompress As Boolean
    End Class

    Private sessions As New List(Of SessionInfo)()
    Private Shared ReadOnly SessionEntropy As Byte() = Encoding.UTF8.GetBytes("tylanguagetrans-sessions-v1")
    Private Shared ReadOnly SessionDirectory As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "LCS", "tylanguagetrans")
    Private Shared ReadOnly SessionFilePath As String = Path.Combine(SessionDirectory, "sessions.dat")

    ' === 词表映射（长度必须64，每个两字） ===
    Private Shared ReadOnly wordList As String() = {
        "吴聊", "高点", "压制", "伯父", "钉钉", "车失", "咸入", "支援",
        "前线", "吻开", "一航", "悄然", "幸福", "假如", "捉胃", "陈兆",
        "变态", "行军", "全速", "动感", "红地", "儿子", "防爬", "木南",
        "打的", "炒饭", "哨兵", "远控", "天叶", "火华", "策略", "带添",
        "汗城", "欧标", "基地", "警戒", "通讯", "蟑螂", "杨台", "刺猬",
        "病毒", "顺龙", "利萍", "润冰", "找茬", "嘟嘟", "洞幺", "弹药",
        "侦察", "平头", "追击", "废了", "生艹", "抄马", "恨狗", "毛利",
        "嘴贱", "军事", "迷你", "包马", "电脑", "监听", "长官", "没屎"
    }

    Private Shared ReadOnly wordList128 As String() = {
        "吴聊", "高点", "压制", "伯父", "钉钉", "车失", "咸入", "支援",
        "前线", "吻开", "一航", "悄然", "幸福", "假如", "捉胃", "陈兆",
        "变态", "行军", "全速", "动感", "红地", "儿子", "防爬", "木南",
        "打的", "炒饭", "哨兵", "远控", "天晔", "火华", "策略", "带添",
        "汗城", "欧标", "基地", "警戒", "通讯", "蟑螂", "杨台", "刺猬",
        "病毒", "顺龙", "利萍", "润冰", "找茬", "嘟嘟", "洞幺", "弹药",
        "侦察", "平头", "追击", "废了", "生艹", "抄码", "恨狗", "毛利",
        "嘴贱", "军事", "迷你", "包马", "电脑", "监听", "长官", "没屎", '64
        "烂脸", "耳东", "掩护", "屏翻", "菊瓶", "国羊", "加豪", "春埂",
        "啵铂", "佩歪", "土匪", "莉园", "金释", "霉菌", "房御", "塔喽",
        "懒芸", "飞猪", "腥郡", "劳扩", "乒乓", "方格", "二恺", "蜂脚",
        "佰敷", "泻鲤", "首鸡", "扣币", "拿枪", "玛旧", "占术", "得可",
        "算海", "承诺", "迦浮", "核霸", "炸药", "凯鳃", "砖叶", "害人",
        "俞轩", "狐狸", "雷特", "鹄蛹", "西四", "请求", "君峥", "徕睐",
        "瘤歌", "裕霍", "慨船", "武装", "兆河", "鑫巾", "杀死", "金喽",
        "唯美", "浩文", "装甲", "护盾", "跳伞", "彩虹", "硫囚", "新服"
    }

    Private Shared ReadOnly WordList256 As String() = {
        "吴聊", "高点", "压制", "伯父", "钉钉", "车失", "咸入", "支援",
        "前线", "吻开", "一航", "悄然", "幸福", "假如", "捉胃", "陈兆",
        "变态", "行军", "全速", "动感", "红地", "儿子", "防爬", "木南",
        "打的", "炒饭", "哨兵", "远控", "天晔", "火华", "策略", "带添",
        "汗城", "欧标", "基地", "警戒", "通讯", "蟑螂", "杨台", "刺猬",
        "病毒", "顺龙", "利萍", "润冰", "找茬", "嘟嘟", "洞幺", "弹药",
        "侦察", "平头", "追击", "废了", "生艹", "抄码", "恨狗", "毛利",
        "嘴贱", "军事", "迷你", "包马", "电脑", "监听", "长官", "没屎",
        "烂脸", "耳东", "掩护", "屏翻", "菊瓶", "国羊", "加豪", "春埂",
        "啵铂", "佩歪", "土匪", "莉园", "金释", "霉菌", "房御", "塔喽",
        "懒芸", "飞猪", "腥郡", "劳扩", "乒乓", "方格", "二恺", "蜂脚",
        "佰敷", "泻鲤", "首鸡", "扣币", "拿枪", "玛旧", "占术", "得可",
        "算海", "承诺", "迦浮", "核霸", "炸药", "凯鳃", "砖叶", "害人",
        "俞轩", "狐狸", "雷特", "鹄蛹", "西四", "请求", "君峥", "徕睐",
        "瘤歌", "裕霍", "慨船", "武装", "兆河", "鑫巾", "杀死", "金喽",
        "唯美", "浩文", "装甲", "护盾", "跳伞", "彩虹", "硫囚", "新服",
        "自蛋", "机枪", "刚抢", "苟分", "舔包", "空头", "伞酱", "夜屎",
        "热橙", "香仪", "防谈", "弹夹", "蛋盒", "枪棺", "抢脱", "消因",
        "消烟", "制退", "后坐", "抛客", "进蛋", "卡蛋", "炸糖", "哑火",
        "鸭蛋", "臭蛋", "手雷", "手溜", "震爆", "烟雾", "烟幕", "催泪",
        "毒气", "生化", "核蛋", "放辐", "沾染", "洗消", "侦插", "巡锣",
        "岗少", "暗少", "明少", "潜服", "埋扶", "设伏", "突袭", "夜袭",
        "强攻", "佯功", "迂回", "穿叉", "包切", "围煎", "清缴", "肃清",
        "俘卤", "审迅", "口供", "情爆", "密电", "破译", "截获", "收听",
        "电苔", "频段", "跳频", "加密", "解码", "呼号", "代号", "密语",
        "洞铃", "洞山", "洞死", "洞舞", "洞溜", "洞漆", "洞拔", "洞酒",
        "拐零", "拐一", "拐二", "拐三", "幺洞", "两拐", "三八", "酒瓶",
        "军粮", "野餐", "水壶", "工兵", "锹镐", "地雷", "诡雷", "拌线",
        "铁丝", "拒马", "掩体", "战壕", "猫耳", "火力", "压智", "覆盖",
        "急促", "徐进", "弹幕", "校射", "观瞄", "炮兵", "榴弹", "迫击",
        "火箭", "筒子", "反坦", "飞军", "破甲", "碎甲", "装药", "引信",
        "天罡", "铁血", "利剑", "战鹰", "雷霆", "迅捷", "玄武", "苍穹"
    }

    ' 保存当前密钥
    Private PublicKeyXml As String = ""
    Private PrivateKeyXml As String = ""
    Private isLoadingSession As Boolean

    Shared Sub New()
        If WordList256.Length <> 256 OrElse WordList256.Any(Function(word) word.Length <> 2) OrElse WordList256.Distinct().Count() <> 256 Then
            Throw New InvalidOperationException("WordList256 必须包含 256 个互不重复的双字词元")
        End If
    End Sub

    Private Sub btnEncrypt_Click(sender As Object, e As EventArgs) Handles btnEncrypt.Click
        Try
            If PublicKeyXml = "" Then
                MessageBox.Show("要加密，请先导入或生成公钥")
                Return
            End If
            Dim result = EncryptAsymmetricToWords(txtMain.Text, RadioButton1.Checked OrElse chkCompress.Checked, PublicKeyXml)
            txtMain.Text = result
            '更新字数
            Label2.Text = $"字符数：{txtMain.Text.Length}"
        Catch ex As Exception
            MsgBox("加密失败：" & ex.Message, MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "错误")
        End Try
    End Sub

    Private Sub btnDecrypt_Click(sender As Object, e As EventArgs) Handles btnDecrypt.Click
        Try
            If PrivateKeyXml = "" Then
                MessageBox.Show("要解密，请先导入或生成私钥")
                Return
            End If
            Dim result = DecryptAsymmetricFromWords(txtMain.Text, RadioButton1.Checked OrElse chkCompress.Checked, PrivateKeyXml)
            txtMain.Text = result
            '更新字数
            Label2.Text = $"字符数：{txtMain.Text.Length}"
        Catch ex As Exception
            MsgBox("解密失败：" & ex.Message, MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "错误")
        End Try
    End Sub

    Private Sub btnCopy_Click(sender As Object, e As EventArgs) Handles btnCopy.Click
        Clipboard.SetText(txtMain.Text)
        Statuslbl.Text = "已复制。"
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        txtMain.Clear()
        Statuslbl.Text = "已清空。"
    End Sub

    Private Sub btnGenKeys_Click(sender As Object, e As EventArgs) Handles btnGenKeys.Click
        Try
            If RadioButton1.Checked = False Then
                Dim kp = GenerateRsaXmlPair(2048)
                PublicKeyXml = kp.PublicXml
                PrivateKeyXml = kp.PrivateXml
            Else
                Dim kp = GenerateX25519KeyPair()
                PublicKeyXml = kp.PublicBase64
                PrivateKeyXml = kp.PrivateBase64
            End If
            ListBox1.SelectedItem = Nothing
            Statuslbl.Text = "密钥对已生成。请保存为会话以安全保留密钥。"
        Catch ex As Exception
            MsgBox("生成密钥失败：" & ex.Message, MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "错误")
        End Try
    End Sub

    '导入会话
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Using ofd As New OpenFileDialog With {.Filter = "TY 会话文件|*.tysess"}
            If ofd.ShowDialog = DialogResult.OK Then
                Try
                    ' 先以字节读取文件，以支持明文文本与用密码加密的二进制格式
                    If New FileInfo(ofd.FileName).Length > MaxSessionFileBytes Then Throw New FormatException("会话文件不能超过 4 MB")
                    Dim fileBytes = File.ReadAllBytes(ofd.FileName)
                    Dim content As String = Nothing

                    ' 检查是否为加密的会话（EncryptBytesWithPassword 会在开头写入 ASCII "TYSESS1"）
                    If fileBytes.Length >= 7 Then
                        Dim hdr = Encoding.ASCII.GetString(fileBytes, 0, 7)
                        If hdr = "TYSESS1" Then
                            ' 需要密码解密
                            Dim password As String = Nothing
                            If Not TryPromptPassword("输入密码", "请输入用于加密会话文件的密码：", password) Then
                                Statuslbl.Text = "导入已取消。"
                                Return
                            End If
                            Dim plainBytes = DecryptBytesWithPassword(fileBytes, password)
                            content = Encoding.UTF8.GetString(plainBytes)
                        End If
                    End If

                    If content Is Nothing Then
                        ' 不是加密文件，按文本读取（假设 UTF8）
                        content = Encoding.UTF8.GetString(fileBytes)
                    End If

                    Dim lines = content.Replace(vbCrLf, vbLf).Replace(vbCr, vbLf).Split(ControlChars.Lf)
                    If lines.Length < 5 Then Throw New Exception("文件格式错误：行数不足")
                    Dim header = lines(0).Split("|"c)
                    If header.Length < 3 Then Throw New Exception("文件格式错误：头部字段不足")
                    Dim si As New SessionInfo With {
                    .Name = header(0).Trim(),
                    .AlgorithmType = Integer.Parse(header(1)),
                    .DoUseCompress = (header(2) = "1"),
                    .PublicXml = ExtractSection(lines, "----PUB----", "----PRIV----"),
                    .PrivateXml = ExtractSection(lines, "----PRIV----", Nothing)
                }
                    If String.IsNullOrWhiteSpace(si.Name) Then Throw New FormatException("会话名称不能为空")
                    If sessions.Any(Function(existing) String.Equals(existing.Name, si.Name, StringComparison.OrdinalIgnoreCase)) Then
                        Throw New InvalidOperationException("已存在同名会话")
                    End If
                    ValidateSession(si)
                    sessions.Add(si)
                    ListBox1.Items.Add(si.Name)
                    If SaveSessions() Then
                        ListBox1.SelectedIndex = sessions.Count - 1
                        Statuslbl.Text = "会话已导入并安全保存。"
                    Else
                        sessions.Remove(si)
                        ListBox1.Items.RemoveAt(ListBox1.Items.Count - 1)
                    End If
                Catch ex As Exception
                    MsgBox("导入失败：" & ex.Message, MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "错误")
                End Try
            End If
        End Using
    End Sub

    '从行数组中提取两标记之间的内容（不包含标记行）。v2 可为 Nothing 表示提取到末尾。
    Private Function ExtractSection(lines() As String, v1 As String, v2 As String) As String
        Dim startIdx = Array.IndexOf(lines, v1)
        If startIdx < 0 Then Throw New Exception($"文件格式错误：缺少标记 {v1}")
        Dim endIdx As Integer
        If v2 IsNot Nothing Then
            endIdx = Array.IndexOf(lines, v2, startIdx + 1)
            If endIdx < 0 Then Throw New Exception($"文件格式错误：缺少标记 {v2}")
        Else
            endIdx = lines.Length
        End If
        Return String.Join(Environment.NewLine, lines.Skip(startIdx + 1).Take(endIdx - startIdx - 1))
    End Function

    '导出会话
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If ListBox1.SelectedIndex < 0 Or ListBox1.SelectedIndex >= sessions.Count Then
            MsgBox("请先选择要导出的会话。", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, "提示")
            Return
        End If
        Dim si = sessions(ListBox1.SelectedIndex)
        Using sfd As New SaveFileDialog With {
            .Filter = "TY 会话文件|*.tysess",
            .FileName = SanitizeFileName(si.Name) & ".tysess"
        }
            If sfd.ShowDialog = DialogResult.OK Then
                ' 简单文本格式：第一行为 Name|Algorithm|DoUseCompress
                ' 然后一行----PUB----，接着公钥内容，----PRIV----，私钥内容
                Dim sb As New StringBuilder
                sb.AppendLine($"{si.Name}|{si.AlgorithmType}|{If(si.DoUseCompress, 1, 0)}")
                sb.AppendLine("----PUB----")
                sb.AppendLine(si.PublicXml)
                sb.AppendLine("----PRIV----")
                sb.AppendLine(si.PrivateXml)

                ' 询问是否使用密码加密导出的会话
                Dim password As String = Nothing
                If Not TryPromptPassword("加密选项", "请输入密码以加密导出的会话文件（留空则不加密）：", password) Then
                    Statuslbl.Text = "导出已取消。"
                    Return
                End If

                If String.IsNullOrEmpty(password) Then
                    ' 不加密，导出为明文文本
                    File.WriteAllText(sfd.FileName, sb.ToString, Encoding.UTF8)
                    Statuslbl.Text = "会话已导出（未加密）。"
                Else
                    ' 使用密码加密后导出（AES-GCM，PBKDF2 派生密钥）
                    Dim plainBytes = Encoding.UTF8.GetBytes(sb.ToString())
                    Dim enc = EncryptBytesWithPassword(plainBytes, password)
                    File.WriteAllBytes(sfd.FileName, enc)
                    Statuslbl.Text = "会话已导出并已用密码加密。"
                End If
            End If
        End Using
    End Sub

    Private Function TryPromptPassword(title As String, prompt As String, ByRef password As String) As Boolean
        Using dialog As New Form(), promptLabel As New Label(), passwordBox As New TextBox(), okButton As New Button(), cancelButton As New Button()
            dialog.Text = title
            dialog.FormBorderStyle = FormBorderStyle.FixedDialog
            dialog.StartPosition = FormStartPosition.CenterParent
            dialog.ClientSize = New Size(390, 125)
            dialog.MinimizeBox = False
            dialog.MaximizeBox = False
            dialog.ShowInTaskbar = False

            promptLabel.AutoSize = True
            promptLabel.Location = New Point(12, 15)
            promptLabel.Text = prompt
            passwordBox.Location = New Point(15, 45)
            passwordBox.Size = New Size(360, 23)
            passwordBox.UseSystemPasswordChar = True
            okButton.Location = New Point(219, 84)
            okButton.Size = New Size(75, 27)
            okButton.Text = "确定"
            okButton.DialogResult = DialogResult.OK
            cancelButton.Location = New Point(300, 84)
            cancelButton.Size = New Size(75, 27)
            cancelButton.Text = "取消"
            cancelButton.DialogResult = DialogResult.Cancel

            dialog.Controls.AddRange({promptLabel, passwordBox, okButton, cancelButton})
            dialog.AcceptButton = okButton
            dialog.CancelButton = cancelButton

            If dialog.ShowDialog(Me) <> DialogResult.OK Then Return False
            password = passwordBox.Text
            Return True
        End Using
    End Function

    ' ========= 混合加密实现 =========

    Public Function EncryptAsymmetricToWords(plainText As String, compress As Boolean, rsaPublicXml As String) As String
        Dim data As Byte() = If(compress, CompressString(plainText), Encoding.UTF8.GetBytes(plainText))
        Dim packet As Byte() = HybridEncrypt(data, rsaPublicXml)
        ' 对新算法 (X25519) 使用 256 词表（压缩时使用 256 映射，否则使用原始6位映射）
        If rsaPublicXml IsNot Nothing AndAlso rsaPublicXml.StartsWith("X25519:") Then
            If compress Then
                Return BytesToWordString128(packet)
            Else
                Return BytesToWordString(packet)
            End If
        Else
            ' 旧算法始终使用原始词表映射（不受压缩选项影响）
            Return BytesToWordString(packet)
        End If
    End Function

    ' 使用密码加密字节：PBKDF2(SHA256) -> AES-GCM。输出格式:
    ' "TYSESS1"(7 bytes) | iterations(4 big-endian) | salt(16) | nonce(12) | cipher | tag(16)
    Private Function EncryptBytesWithPassword(plain As Byte(), password As String) As Byte()
        If plain Is Nothing Then Return Nothing
        Dim rng = RandomNumberGenerator.Create()
        Dim salt(15) As Byte
        rng.GetBytes(salt)
        Dim iterations As Integer = 100000
        Dim key(31) As Byte
        Using kdf As New Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256)
            key = kdf.GetBytes(32)
        End Using

        Dim nonce(11) As Byte
        rng.GetBytes(nonce)

        Dim cipher(plain.Length - 1) As Byte
        Dim tag(15) As Byte
        ' 修复：显式指定 GCM tag 长度，避免 SYSLIB0053 警告
        Using aesg As New AesGcm(key, 16)
            aesg.Encrypt(nonce, plain, cipher, tag, Nothing)
        End Using

        Using ms As New MemoryStream()
            Dim hdr = Encoding.ASCII.GetBytes("TYSESS1")
            ms.Write(hdr, 0, hdr.Length)
            ' iterations big-endian
            Dim itb = BitConverter.GetBytes(CType(iterations, Integer))
            If BitConverter.IsLittleEndian Then Array.Reverse(itb)
            ms.Write(itb, 0, 4)
            ms.Write(salt, 0, salt.Length)
            ms.Write(nonce, 0, nonce.Length)
            ms.Write(cipher, 0, cipher.Length)
            ms.Write(tag, 0, tag.Length)
            Return ms.ToArray()
        End Using
    End Function

    ' 与 EncryptBytesWithPassword 对称的解密实现
    Private Function DecryptBytesWithPassword(enc As Byte(), password As String) As Byte()
        If enc Is Nothing OrElse enc.Length = 0 Then Return Nothing
        If enc.Length < 7 Then Throw New FormatException("数据格式错误：过短")
        Dim hdr = Encoding.ASCII.GetString(enc, 0, 7)
        If hdr <> "TYSESS1" Then Throw New FormatException("数据格式错误：不是受支持的会话文件")

        Dim pos As Integer = 7
        If enc.Length < pos + 4 + 16 + 12 + 16 Then Throw New FormatException("数据格式错误：长度不足")
        Dim itb(3) As Byte
        Array.Copy(enc, pos, itb, 0, 4) : pos += 4
        If BitConverter.IsLittleEndian Then Array.Reverse(itb)
        Dim iterations = BitConverter.ToInt32(itb, 0)
        If iterations < 100000 OrElse iterations > 2000000 Then
            Throw New FormatException("数据格式错误：不支持的密钥派生迭代次数")
        End If

        Dim salt(15) As Byte
        Array.Copy(enc, pos, salt, 0, 16) : pos += 16
        Dim nonce(11) As Byte
        Array.Copy(enc, pos, nonce, 0, 12) : pos += 12

        ' 剩余为 cipher + tag(16)
        If enc.Length < pos + 16 Then Throw New FormatException("数据格式错误：缺少认证标签")
        Dim tagLen As Integer = 16
        Dim cipherLen = enc.Length - pos - tagLen
        If cipherLen < 0 Then Throw New FormatException("数据格式错误：密文长度错误")
        Dim cipher(cipherLen - 1) As Byte
        If cipherLen > 0 Then Array.Copy(enc, pos, cipher, 0, cipherLen)
        pos += cipherLen
        Dim tag(15) As Byte
        Array.Copy(enc, pos, tag, 0, tagLen)

        ' 派生密钥
        Dim key(31) As Byte
        Using kdf As New Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256)
            key = kdf.GetBytes(32)
        End Using

        Dim plain(cipherLen - 1) As Byte
        Using aesg As New AesGcm(key, 16)
            aesg.Decrypt(nonce, cipher, tag, plain, Nothing)
        End Using
        Return plain
    End Function

    Public Function DecryptAsymmetricFromWords(wordCipher As String, compressed As Boolean, rsaPrivateXml As String) As String
        Dim packet As Byte()
        Dim data As Byte()
        ' 解码映射：新算法 (X25519) 根据 compressed 选择 256/6 位映射，旧算法始终使用原始词表映射
        If rsaPrivateXml IsNot Nothing AndAlso rsaPrivateXml.StartsWith("X25519:") Then
            ' 对 X25519 尝试基于 compressed 首选的映射，若认证/解密失败则重试另一种映射
            Dim tried As New List(Of Exception)()
            Dim tryOrder As New List(Of Func(Of Byte()))
            If compressed Then
                tryOrder.Add(Function() WordStringToBytes128(wordCipher))
                tryOrder.Add(Function() WordStringToBytes(wordCipher))
            Else
                tryOrder.Add(Function() WordStringToBytes(wordCipher))
                tryOrder.Add(Function() WordStringToBytes128(wordCipher))
            End If

            For Each conv In tryOrder
                Try
                    packet = conv()
                    data = HybridDecrypt(packet, rsaPrivateXml)
                    If compressed Then
                        Return DecompressBytes(data)
                    Else
                        Return Encoding.UTF8.GetString(data)
                    End If
                Catch ex As CryptographicException
                    ' 认证标签或解密失败，记录并尝试下一个映射
                    tried.Add(ex)
                Catch ex As Exception
                    ' 其他解析错误也记录并继续尝试
                    tried.Add(ex)
                End Try
            Next
            ' 都失败则抛出第一个错误以便上层显示
            If tried.Count > 0 Then Throw tried(0)
            Throw New CryptographicException("无法解码或解密输入")
        Else
            packet = WordStringToBytes(wordCipher)
            data = HybridDecrypt(packet, rsaPrivateXml)
            If compressed Then
                Return DecompressBytes(data)
            Else
                Return Encoding.UTF8.GetString(data)
            End If
        End If
    End Function

    '加密实现
    Private Function HybridEncrypt(plain As Byte(), rsaPublicXml As String) As Byte()
        '判断是否为新算法
        If rsaPublicXml IsNot Nothing AndAlso rsaPublicXml.StartsWith("X25519:") Then
            'X25519 + AES-GCM 分支（新算法）
            Const VERSION As Byte = 4
            Const ALG_ID As Byte = 2
            Dim pubB64 = rsaPublicXml.Substring("X25519:".Length)
            Dim recipientPub = Convert.FromBase64String(pubB64)
            '生成 ephemeral key
            Try
                Using eph As ECDiffieHellman = ECDiffieHellman.Create(ECCurve.CreateFromFriendlyName("X25519"))
                    Dim ephPub = eph.PublicKey.ExportSubjectPublicKeyInfo()
                    '取出 raw 32 字节公钥以节省包长度（SPKI 包含固定 ASN.1 前缀）
                    Dim ephRaw As Byte()
                    If ephPub IsNot Nothing AndAlso ephPub.Length >= 32 Then
                        ephRaw = ephPub.Skip(ephPub.Length - 32).ToArray()
                    Else
                        ephRaw = ephPub
                    End If
                    '导入对方公钥为 ECDiffieHellmanPublicKey（通过临时对象）
                    Using tmp As ECDiffieHellman = ECDiffieHellman.Create(ECCurve.CreateFromFriendlyName("X25519"))
                        Dim read As Integer = 0
                        tmp.ImportSubjectPublicKeyInfo(recipientPub, read)
                        Dim sharedSecret = eph.DeriveKeyMaterial(tmp.PublicKey) ' 32 bytes

                        '从 shared secret 使用 HKDF 派生 AES-256-GCM key (32) + nonce (12)
                        Dim info = Encoding.UTF8.GetBytes("tylang-x25519-aesgcm-v1")
                        Dim keyMat = HKDF_SHA256(Nothing, sharedSecret, info, 44)
                        Dim aesKey = keyMat.Take(32).ToArray()
                        Dim nonce = keyMat.Skip(32).Take(12).ToArray()

                        'AES-GCM 加密
                        Dim cipher(plain.Length - 1) As Byte
                        Dim tag(15) As Byte
                        Using aesg As New AesGcm(aesKey, 16)
                            aesg.Encrypt(nonce, plain, cipher, tag, Nothing)
                        End Using
                        '组装包
                        Dim ephLenBE = BitConverter.GetBytes(CUShort(ephPub.Length))
                        If BitConverter.IsLittleEndian Then Array.Reverse(ephLenBE)
                        Using ms As New MemoryStream()
                            ms.WriteByte(VERSION)
                            ms.WriteByte(ALG_ID)
                            ' 为了缩短包体，写入 32 字节的 raw public key（不包含 SPKI 前缀）
                            ms.Write(ephRaw, 0, ephRaw.Length)
                            ms.Write(nonce, 0, nonce.Length)
                            ms.Write(cipher, 0, cipher.Length)
                            ms.Write(tag, 0, tag.Length)
                            Return ms.ToArray()
                        End Using
                    End Using
                End Using
            Catch ex As Exception
                '平台不支持 X25519 的 System.Security 实现，使用 BouncyCastle 实现一次性密钥协商
                Dim spki = SubjectPublicKeyInfo.GetInstance(Asn1Object.FromByteArray(recipientPub))
                Dim recRaw = spki.PublicKeyData.GetBytes()
                Dim gen = New X25519KeyPairGenerator()
                gen.Init(New X25519KeyGenerationParameters(New SecureRandom()))
                Dim kp As AsymmetricCipherKeyPair = gen.GenerateKeyPair()
                Dim pubParam = CType(kp.Public, X25519PublicKeyParameters)
                Dim privParam = CType(kp.Private, X25519PrivateKeyParameters)
                ' 计算共享密钥
                Dim agreement = New Org.BouncyCastle.Crypto.Agreement.X25519Agreement()
                agreement.Init(privParam)
                Dim recipPub = New X25519PublicKeyParameters(recRaw, 0)
                Dim sharedSecret(31) As Byte
                agreement.CalculateAgreement(recipPub, sharedSecret, 0)
                '从 shared secret 使用 HKDF 派生 AES-256-GCM key (32) + nonce (12)
                Dim info = Encoding.UTF8.GetBytes("tylang-x25519-aesgcm-v1")
                Dim keyMat = HKDF_SHA256(Nothing, sharedSecret, info, 44)
                Dim aesKey = keyMat.Take(32).ToArray()
                Dim nonce = keyMat.Skip(32).Take(12).ToArray()
                'AES-GCM 加密
                Dim cipher(plain.Length - 1) As Byte
                Dim tag(15) As Byte
                Using aesg As New AesGcm(aesKey, 16)
                    aesg.Encrypt(nonce, plain, cipher, tag, Nothing)
                End Using
                '取出 ephemeral raw 公钥（32 字节）
                Dim ephRaw = pubParam.GetEncoded()
                Using ms As New MemoryStream()
                    ms.WriteByte(VERSION)
                    ms.WriteByte(ALG_ID)
                    ms.Write(ephRaw, 0, ephRaw.Length)
                    ms.Write(nonce, 0, nonce.Length)
                    ms.Write(cipher, 0, cipher.Length)
                    ms.Write(tag, 0, tag.Length)
                    Return ms.ToArray()
                End Using
            End Try
        Else
            '兼容原有 RSA + AES-CBC + HMAC 实现（version=3, alg=1）
            Const VERSION As Byte = 3
            Const ALG_ID As Byte = 1
            '会话材料（精简后）
            Dim aesKey(15) As Byte, hmacKey(15) As Byte, iv(15) As Byte ' 16字节IV
            Using rng = RandomNumberGenerator.Create()
                rng.GetBytes(aesKey)
                rng.GetBytes(hmacKey)
                rng.GetBytes(iv)
            End Using

            'AES加密正文
            Dim cipher As Byte()
            Using aes As Aes = Aes.Create()
                aes.Key = aesKey
                aes.IV = iv
                aes.Mode = CipherMode.CBC
                aes.Padding = PaddingMode.PKCS7
                Using enc = aes.CreateEncryptor()
                    cipher = enc.TransformFinalBlock(plain, 0, plain.Length)
                End Using
            End Using

            'RSA加密会话密钥
            Dim session(31) As Byte
            Buffer.BlockCopy(aesKey, 0, session, 0, 16)
            Buffer.BlockCopy(hmacKey, 0, session, 16, 16)
            Dim rsaEnc As Byte()
            Using rsa As RSA = RSA.Create()
                rsa.FromXmlString(rsaPublicXml)
                Try
                    rsaEnc = rsa.Encrypt(session, RSAEncryptionPadding.OaepSHA256)
                Catch
                    rsaEnc = rsa.Encrypt(session, RSAEncryptionPadding.OaepSHA1)
                End Try
            End Using

            '构造 rsaEnc 长度字段（2字节，大端）
            Dim rsaLenBE = BitConverter.GetBytes(CUShort(rsaEnc.Length))
            If BitConverter.IsLittleEndian Then Array.Reverse(rsaLenBE)

            '构造 AAD（version + alg + iv + rsaLenBE + rsaEnc + cipher）
            Dim aadLen = 1 + 1 + iv.Length + 2 + rsaEnc.Length + cipher.Length
            Dim aad(aadLen - 1) As Byte
            Dim off = 0
            aad(off) = VERSION : off += 1
            aad(off) = ALG_ID : off += 1
            Buffer.BlockCopy(iv, 0, aad, off, iv.Length) : off += iv.Length
            Buffer.BlockCopy(rsaLenBE, 0, aad, off, 2) : off += 2
            Buffer.BlockCopy(rsaEnc, 0, aad, off, rsaEnc.Length) : off += rsaEnc.Length
            Buffer.BlockCopy(cipher, 0, aad, off, cipher.Length)

            '计算 HMAC-SHA256 并截取前16字节
            Dim tag(15) As Byte
            Using h = New HMACSHA256(hmacKey)
                Dim fullTag = h.ComputeHash(aad)
                Buffer.BlockCopy(fullTag, 0, tag, 0, 16)
            End Using

            '组装最终数据包
            Using ms As New MemoryStream()
                ms.WriteByte(VERSION)
                ms.WriteByte(ALG_ID)
                ms.Write(iv, 0, iv.Length)
                ms.Write(rsaLenBE, 0, 2)
                ms.Write(rsaEnc, 0, rsaEnc.Length)
                ms.Write(cipher, 0, cipher.Length)
                ms.Write(tag, 0, tag.Length)
                Return ms.ToArray()
            End Using
        End If
    End Function

    '解密实现
    Private Function HybridDecrypt(packet As Byte(), rsaPrivateXml As String) As Byte()
        If packet Is Nothing OrElse packet.Length < 2 Then Throw New FormatException("密文包长度不足")
        Using ms As New MemoryStream(packet), br As New BinaryReader(ms)
            Dim version = br.ReadByte()
            Dim alg = br.ReadByte()
            If version = 4 AndAlso alg = 2 Then
                If packet.Length < 2 + 32 + 12 + 16 Then Throw New FormatException("X25519 密文包长度不足")
                'X25519 + AES-GCM 解密
                '读取 32 字节的 raw ephemeral 公钥并重建为 SubjectPublicKeyInfo
                Dim ephRaw = br.ReadBytes(32)
                Dim iv = br.ReadBytes(12)
                Dim remaining = br.ReadBytes(CInt(ms.Length - ms.Position))
                If remaining.Length < 16 Then Throw New CryptographicException("包格式错误")
                Dim cipherLen = remaining.Length - 16
                Dim cipher(cipherLen - 1) As Byte
                Buffer.BlockCopy(remaining, 0, cipher, 0, cipherLen)
                Dim tag(15) As Byte
                Buffer.BlockCopy(remaining, cipherLen, tag, 0, 16)
                '导入本地私钥
                If rsaPrivateXml Is Nothing OrElse Not rsaPrivateXml.StartsWith("X25519:") Then
                    Throw New CryptographicException("私钥格式错误：期待 X25519 私钥")
                End If
                Dim privB64 = rsaPrivateXml.Substring("X25519:".Length)
                Dim privBytes = Convert.FromBase64String(privB64)
                Try
                    Using sk As ECDiffieHellman = ECDiffieHellman.Create(ECCurve.CreateFromFriendlyName("X25519"))
                        Dim read As Integer = 0
                        sk.ImportPkcs8PrivateKey(privBytes, read)
                        Using tmp As ECDiffieHellman = ECDiffieHellman.Create(ECCurve.CreateFromFriendlyName("X25519"))
                            '重建 SPKI 前缀并导入
                            Dim spkiHeader As Byte() = {&H30, &H2A, &H30, &H5, &H6, &H3, &H2B, &H65, &H6E, &H3, &H21, &H0}
                            Dim ephSpki As Byte() = spkiHeader.Concat(ephRaw).ToArray()
                            Dim rr As Integer = 0
                            tmp.ImportSubjectPublicKeyInfo(ephSpki, rr)
                            Dim sharedSecret = sk.DeriveKeyMaterial(tmp.PublicKey)
                            Dim info = Encoding.UTF8.GetBytes("tylang-x25519-aesgcm-v1")
                            Dim keyMat = HKDF_SHA256(Nothing, sharedSecret, info, 44)
                            Dim aesKey = keyMat.Take(32).ToArray()
                            Dim nonce = keyMat.Skip(32).Take(12).ToArray()
                            '验证/解密
                            Dim plain(cipher.Length - 1) As Byte
                            Using aesg As New AesGcm(aesKey, 16)
                                aesg.Decrypt(nonce, cipher, tag, plain, Nothing)
                            End Using
                            Return plain
                        End Using
                    End Using
                Catch ex As Exception
                    ' 平台不支持 System.Security 的 X25519：使用 BouncyCastle 计算共享密钥并解密
                    Dim privInfo = PrivateKeyInfo.GetInstance(Asn1Object.FromByteArray(privBytes))
                    Dim rawPriv = Org.BouncyCastle.Asn1.Asn1OctetString.GetInstance(privInfo.ParsePrivateKey()).GetOctets()
                    Dim privParam = New X25519PrivateKeyParameters(rawPriv, 0)
                    Dim pubParam = New X25519PublicKeyParameters(ephRaw, 0)
                    Dim agreement = New Org.BouncyCastle.Crypto.Agreement.X25519Agreement()
                    agreement.Init(privParam)
                    Dim sharedSecret(31) As Byte
                    agreement.CalculateAgreement(pubParam, sharedSecret, 0)

                    Dim info = Encoding.UTF8.GetBytes("tylang-x25519-aesgcm-v1")
                    Dim keyMat = HKDF_SHA256(Nothing, sharedSecret, info, 44)
                    Dim aesKey = keyMat.Take(32).ToArray()
                    Dim nonce = keyMat.Skip(32).Take(12).ToArray()

                    Dim plain(cipher.Length - 1) As Byte
                    Using aesg As New AesGcm(aesKey, 16)
                        aesg.Decrypt(nonce, cipher, tag, plain, Nothing)
                    End Using
                    Return plain
                End Try
            ElseIf version = 3 AndAlso alg = 1 Then
                If packet.Length < 2 + 16 + 2 + 16 Then Throw New FormatException("RSA 密文包长度不足")
                '旧版 RSA + AES-CBC + HMAC
                Dim iv = br.ReadBytes(16)
                Dim rsaLenBE = br.ReadBytes(2)
                Dim rsaLen = CUShort((CUShort(rsaLenBE(0)) << 8) Or rsaLenBE(1))
                If rsaLen = 0 OrElse rsaLen > ms.Length - ms.Position - 16 Then Throw New FormatException("RSA 密钥数据长度错误")
                Dim rsaEnc = br.ReadBytes(rsaLen)
                Dim remaining = br.ReadBytes(CInt(ms.Length - ms.Position))
                Dim cipherLen = remaining.Length - 16 ' 修改为16字节
                Dim cipher(cipherLen - 1) As Byte
                Buffer.BlockCopy(remaining, 0, cipher, 0, cipherLen)
                Dim tag(15) As Byte ' 修改为16字节
                Buffer.BlockCopy(remaining, cipherLen, tag, 0, 16)
                ' AAD
                rsaLenBE = BitConverter.GetBytes(CUShort(rsaEnc.Length))
                If BitConverter.IsLittleEndian Then Array.Reverse(rsaLenBE)
                Dim aadLen = 1 + 1 + iv.Length + 2 + rsaEnc.Length + cipher.Length
                Dim aad(aadLen - 1) As Byte
                Dim off = 0
                aad(off) = 3 : off += 1 ' version
                aad(off) = 1 : off += 1 ' alg
                Buffer.BlockCopy(iv, 0, aad, off, iv.Length) : off += iv.Length
                Buffer.BlockCopy(rsaLenBE, 0, aad, off, 2) : off += 2
                Buffer.BlockCopy(rsaEnc, 0, aad, off, rsaEnc.Length) : off += rsaEnc.Length
                Buffer.BlockCopy(cipher, 0, aad, off, cipher.Length)
                ' RSA 解密会话材料
                Dim session As Byte()
                Using rsa As RSA = RSA.Create()
                    rsa.FromXmlString(rsaPrivateXml)
                    Try
                        session = rsa.Decrypt(rsaEnc, RSAEncryptionPadding.OaepSHA256)
                    Catch
                        session = rsa.Decrypt(rsaEnc, RSAEncryptionPadding.OaepSHA1)
                    End Try
                End Using
                If session.Length <> 32 Then Throw New CryptographicException("会话密钥长度错误")

                Dim aesKey(15) As Byte, hmacKey(15) As Byte
                Buffer.BlockCopy(session, 0, aesKey, 0, 16)
                Buffer.BlockCopy(session, 16, hmacKey, 0, 16)

                ' 校验 HMAC
                Using h = New HMACSHA256(hmacKey)
                    Dim calc = h.ComputeHash(aad)
                    If Not CryptographicOperations.FixedTimeEquals(calc.AsSpan(0, 16), tag) Then
                        Throw New CryptographicException("完整性校验失败")
                    End If
                End Using

                ' AES 解密正文
                Using aes As Aes = Aes.Create()
                    aes.Key = aesKey
                    aes.IV = iv
                    aes.Mode = CipherMode.CBC
                    aes.Padding = PaddingMode.PKCS7
                    Using dec = aes.CreateDecryptor()
                        Return dec.TransformFinalBlock(cipher, 0, cipher.Length)
                    End Using
                End Using
            Else
                Throw New CryptographicException("格式不支持")
            End If
        End Using
    End Function

    ' === 编码/解码 ===
    Private Function BytesToWordString(data As Byte()) As String
        Dim sb As New StringBuilder()
        For Each b In data
            Dim hi = b \ 64
            Dim lo = b And 63
            sb.Append(wordList(hi)).Append(wordList(lo)).Append("！")
        Next
        If sb.Length > 0 Then sb.Length -= 1
        Return sb.ToString()
    End Function

    Private Function WordStringToBytes(s As String) As Byte()
        Dim parts = s.Split("！"c)
        Dim outBytes As New List(Of Byte)(parts.Length)
        For i = 0 To parts.Length - 1
            Dim pair = parts(i)
            If pair.Length = 0 AndAlso i = parts.Length - 1 Then Continue For
            If pair.Length <> 4 Then Throw New FormatException("输入格式不正确：每组必须包含两个双字词元")
            Dim w1 = pair.Substring(0, 2)
            Dim w2 = pair.Substring(2, 2)
            Dim hi = Array.IndexOf(wordList, w1)
            Dim lo = Array.IndexOf(wordList, w2)
            If hi < 0 OrElse lo < 0 Then Throw New FormatException("请检查输入是否为ty语！")
            outBytes.Add(CByte(hi * 64 + lo))
        Next
        Return outBytes.ToArray()
    End Function

    Private Function BytesToWordString128(data As Byte()) As String
        ' 使用 256 词表时直接按字节映射为词元（每字节一个词）
        If WordList256 Is Nothing OrElse WordList256.Length = 0 Then
            Throw New InvalidOperationException("WordList256 未初始化")
        End If
        If WordList256.Length <> 256 Then
            Throw New InvalidOperationException($"WordList256 长度不正确：期望 256，实际 {WordList256.Length}")
        End If
        Dim sb As New StringBuilder()
        For Each b In data
            Dim idx = CInt(b)
            If idx < 0 OrElse idx >= WordList256.Length Then
                Throw New IndexOutOfRangeException($"字节值 {idx} 超出词表索引范围")
            End If
            sb.Append(WordList256(idx))
        Next
        Return sb.ToString()
    End Function

    Private Function WordStringToBytes128(s As String) As Byte()
        ' 参照 BytesToWordString128 的映射，按两个字符一词反向映射到字节
        Dim outBytes As New List(Of Byte)
        If String.IsNullOrEmpty(s) Then Return outBytes.ToArray()
        If s.Length Mod 2 <> 0 Then Throw New FormatException("输入长度不正确：词元应为两字一组")
        For i = 0 To s.Length - 2 Step 2
            Dim word = s.Substring(i, 2)
            Dim idx = Array.IndexOf(WordList256, word)
            If idx < 0 OrElse idx > 255 Then Throw New FormatException("请检查输入是否为ty语！")
            outBytes.Add(CByte(idx))
        Next
        Return outBytes.ToArray()
    End Function

    ' === 压缩/解压 ===
    Private Function CompressString(input As String) As Byte()
        Dim bytes = Encoding.UTF8.GetBytes(input)
        '智能判断长度，过短不压缩
        If bytes.Length < 32 Then
            Return bytes
        End If
        '首先尝试使用 Brotli 以获得更强的压缩（若平台支持）
        Try
            Using ms As New MemoryStream()
                Using br As New BrotliStream(ms, CompressionLevel.Optimal, True)
                    br.Write(bytes, 0, bytes.Length)
                End Using
                Dim brotli = ms.ToArray()
                '仅在 Brotli 实际减小数据时才采用，否则继续尝试 GZip
                If brotli.Length < bytes.Length Then
                    Return brotli
                End If
            End Using
        Catch ex As Exception
            '忽略 Brotli 错误，后续尝试 GZip
        End Try

        '无论 Brotli 成功与否，继续尝试使用 GZip 作为回退压缩
        Try
            Using ms2 As New MemoryStream()
                Using gz As New GZipStream(ms2, CompressionLevel.Optimal, True)
                    gz.Write(bytes, 0, bytes.Length)
                End Using
                Dim gzBytes = ms2.ToArray()
                If gzBytes.Length < bytes.Length Then
                    Return gzBytes
                End If
            End Using
        Catch ex As Exception
            '忽略 GZip 错误
        End Try

        '两种压缩方式均未能减小数据或均失败，返回原始字节
        Return bytes
    End Function

    Private Function DecompressBytes(data As Byte()) As String
        Try
            ' 先尝试 Brotli 解压（若是 Brotli 压缩）
            Try
                Using ms As New MemoryStream(data)
                    Using br As New BrotliStream(ms, CompressionMode.Decompress)
                        Using result As New MemoryStream()
                            br.CopyTo(result)
                            Return Encoding.UTF8.GetString(result.ToArray())
                        End Using
                    End Using
                End Using
            Catch exB As Exception
                ' 不是 Brotli 或解压失败，尝试 GZip
            End Try

            Using ms2 As New MemoryStream(data)
                Using gz As New GZipStream(ms2, CompressionMode.Decompress)
                    Using result As New MemoryStream()
                        gz.CopyTo(result)
                        Return Encoding.UTF8.GetString(result.ToArray())
                    End Using
                End Using
            End Using
        Catch ex As Exception
            ' 解压失败说明未压缩，直接返回原始字节对应的字符串
            Return Encoding.UTF8.GetString(data)
        End Try
    End Function

    ' === 生成密钥对 ===
    Public Function GenerateRsaXmlPair(Optional keySize As Integer = 2048) As (PublicXml As String, PrivateXml As String)
        Using rsa As RSA = RSA.Create(keySize)
            Dim pub = rsa.ToXmlString(False)
            Dim priv = rsa.ToXmlString(True)
            Return (pub, priv)
        End Using
    End Function

    ' === X25519 密钥对生成与辅助函数（使用 BouncyCastle 以兼容不支持 X25519 的平台） ===
    Public Function GenerateX25519KeyPair() As (PublicBase64 As String, PrivateBase64 As String)
        ' 使用 BouncyCastle 直接生成 X25519 密钥对并导出为 SubjectPublicKeyInfo / PKCS8 DER
        Dim gen = New X25519KeyPairGenerator()
        gen.Init(New X25519KeyGenerationParameters(New SecureRandom()))
        Dim kp As AsymmetricCipherKeyPair = gen.GenerateKeyPair()

        Dim pubParam = CType(kp.Public, X25519PublicKeyParameters)
        Dim privParam = CType(kp.Private, X25519PrivateKeyParameters)

        Dim spki = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(pubParam)
        Dim pubDer = spki.ToAsn1Object().GetDerEncoded()

        Dim privInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(privParam)
        Dim privDer = privInfo.ToAsn1Object().GetDerEncoded()

        Return ($"X25519:{Convert.ToBase64String(pubDer)}", $"X25519:{Convert.ToBase64String(privDer)}")
    End Function

    Private Function HKDF_SHA256(salt As Byte(), ikm As Byte(), info As Byte(), outLen As Integer) As Byte()
        If salt Is Nothing Then
            salt = New Byte(31) {} ' 32 zero bytes
        End If
        ' Extract
        Dim prk As Byte()
        Using hmac = New HMACSHA256(salt)
            prk = hmac.ComputeHash(ikm)
        End Using
        ' Expand
        Dim okm As New List(Of Byte)()
        Dim previous() As Byte = {}
        Dim counter As Byte = 1
        While okm.Count < outLen
            Using hmac = New HMACSHA256(prk)
                hmac.TransformBlock(previous, 0, previous.Length, Nothing, 0)
                If info IsNot Nothing AndAlso info.Length > 0 Then
                    hmac.TransformBlock(info, 0, info.Length, Nothing, 0)
                End If
                hmac.TransformFinalBlock(New Byte() {counter}, 0, 1)
                previous = hmac.Hash
            End Using
            okm.AddRange(previous)
            counter = CByte(counter + 1)
        End While
        Return okm.Take(outLen).ToArray()
    End Function


    '更新字数
    Private Sub txtMain_TextChanged(sender As Object, e As EventArgs) Handles txtMain.TextChanged
        Label2.Text = $"字符数：{txtMain.Text.Length}"
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton2.Checked Then
            chkCompress.Enabled = True
        Else
            chkCompress.Enabled = False
            chkCompress.Checked = True
        End If
        '如果当前处于会话中，更新当前会话的算法设置
        If ListBox1.SelectedIndex >= 0 Then
            If isLoadingSession Then Return
            Dim si = sessions(ListBox1.SelectedIndex)
            Dim previousAlgorithm = si.AlgorithmType
            Dim previousCompress = si.DoUseCompress
            si.AlgorithmType = If(RadioButton1.Checked, 1, 0)
            si.DoUseCompress = chkCompress.Checked
            If Not SaveSessions() Then
                si.AlgorithmType = previousAlgorithm
                si.DoUseCompress = previousCompress
            End If
        End If
    End Sub

    Private Sub chkCompress_CheckedChanged(sender As Object, e As EventArgs) Handles chkCompress.CheckedChanged
        If isLoadingSession OrElse ListBox1.SelectedIndex < 0 OrElse ListBox1.SelectedIndex >= sessions.Count Then Return
        Dim session = sessions(ListBox1.SelectedIndex)
        Dim previous = session.DoUseCompress
        session.DoUseCompress = chkCompress.Checked
        If Not SaveSessions() Then session.DoUseCompress = previous
    End Sub

    '添加会话
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim name = InputBox("请输入会话名称", "保存当前会话")
        If String.IsNullOrWhiteSpace(name) Then Return
        name = name.Trim()
        If sessions.Any(Function(existing) String.Equals(existing.Name, name, StringComparison.OrdinalIgnoreCase)) Then
            MsgBox("已存在同名会话，请使用其他名称。", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, "提示")
            Return
        End If

        '保存当前正在使用的密钥到会话（保留原有密钥）
        If String.IsNullOrEmpty(PublicKeyXml) OrElse String.IsNullOrEmpty(PrivateKeyXml) Then
            MsgBox("当前没有可保存的密钥，请先生成或导入公私钥。", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, "提示")
            Return
        End If
        Dim si As New SessionInfo() With {.Name = name, .PublicXml = PublicKeyXml, .PrivateXml = PrivateKeyXml, .AlgorithmType = If(RadioButton1.Checked, 1, 0), .DoUseCompress = chkCompress.Checked}
        sessions.Add(si)
        ListBox1.Items.Add(name)
        If Not SaveSessions() Then
            sessions.Remove(si)
            ListBox1.Items.RemoveAt(ListBox1.Items.Count - 1)
            Return
        End If
        '激活新会话
        PublicKeyXml = si.PublicXml
        PrivateKeyXml = si.PrivateXml
        '选中刚添加的会话项以同步 UI
        ListBox1.SelectedIndex = ListBox1.Items.Count - 1
        Statuslbl.Text = $"会话 {name} 已保存并激活。"
    End Sub

    '删除会话
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If ListBox1.SelectedIndex >= 0 Then
            Dim index = ListBox1.SelectedIndex
            Dim removedSession = sessions(index)
            Dim name = removedSession.Name
            sessions.RemoveAt(index)
            ListBox1.Items.RemoveAt(index)
            If Not SaveSessions() Then
                sessions.Insert(index, removedSession)
                ListBox1.Items.Insert(index, name)
                ListBox1.SelectedIndex = index
                Return
            End If
            ' 如果还有会话则激活第一个，否则清空当前密钥
            If sessions.Count > 0 Then
                ListBox1.SelectedIndex = Math.Min(index, sessions.Count - 1)
            Else
                PublicKeyXml = ""
                PrivateKeyXml = ""
            End If
            Statuslbl.Text = $"会话 {name} 已删除。"
        Else
            MsgBox("请先选择一个会话。", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, "提示")
        End If
    End Sub

    '保存预设
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Settings1.Default.AlgorithmType = If(RadioButton1.Checked, 1, 0) '1表示新版本算法，0表示旧版本算法
        Settings1.Default.DoUseCompress = chkCompress.Checked
        Settings1.Default.Save()
    End Sub

    '加载预设
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Settings1.Default.AlgorithmType = 1 Then
            RadioButton1.Checked = True
            chkCompress.Checked = True
        Else
            RadioButton2.Checked = True
            chkCompress.Enabled = True
            chkCompress.Checked = Settings1.Default.DoUseCompress
        End If
        Try
            LoadSessions()
        Catch ex As Exception
            MsgBox("加载会话失败：" & ex.Message, MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "错误")
            Statuslbl.Text = "会话加载失败，原数据未被修改。"
        End Try
        'MsgBox（"此版本为内测版。您在这个版本体验到的功能可能与正式版有出入。请勿泄露此版本，并在测试结束后通过检查更新功能更新到最新版本。测试完成后请销毁这个文件，谢谢配合。", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "测试版警告"）
    End Sub

    Private Function SaveSessions() As Boolean
        Try
            Directory.CreateDirectory(SessionDirectory)
            ValidateSessions(sessions)
            Dim protectedBytes = SerializeProtectedSessions(sessions)
            Dim tempPath = SessionFilePath & ".tmp"
            File.WriteAllBytes(tempPath, protectedBytes)
            File.Move(tempPath, SessionFilePath, True)
            Return True
        Catch ex As Exception
            MsgBox("保存会话失败：" & ex.Message, MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "错误")
            Statuslbl.Text = "会话保存失败。"
            Return False
        End Try
    End Function

    Private Function SerializeProtectedSessions(items As List(Of SessionInfo)) As Byte()
        Dim plain = JsonSerializer.SerializeToUtf8Bytes(items)
        Return ProtectedData.Protect(plain, SessionEntropy, DataProtectionScope.CurrentUser)
    End Function

    Private Function DeserializeProtectedSessions(protectedBytes As Byte()) As List(Of SessionInfo)
        If protectedBytes Is Nothing OrElse protectedBytes.Length = 0 Then Throw New FormatException("会话数据为空")
        Dim plain = ProtectedData.Unprotect(protectedBytes, SessionEntropy, DataProtectionScope.CurrentUser)
        Dim result = JsonSerializer.Deserialize(Of List(Of SessionInfo))(plain)
        If result Is Nothing Then Throw New FormatException("会话数据格式错误")
        ValidateSessions(result)
        Return result
    End Function

    Private Sub LoadSessions()
        sessions.Clear()
        ListBox1.Items.Clear()
        If File.Exists(SessionFilePath) Then
            Dim protectedBytes = File.ReadAllBytes(SessionFilePath)
            sessions = DeserializeProtectedSessions(protectedBytes)
        ElseIf File.Exists("sessions.txt") Then
            LoadLegacySessions()
            If SaveSessions() Then DeleteLegacySessionFiles()
        End If

        For Each session In sessions
            ListBox1.Items.Add(session.Name)
        Next
        If sessions.Count > 0 Then
            ListBox1.SelectedIndex = 0
        End If
    End Sub

    Private Sub ValidateSessions(items As List(Of SessionInfo))
        Dim names As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
        For Each session In items
            If session Is Nothing OrElse String.IsNullOrWhiteSpace(session.Name) Then Throw New FormatException("会话数据包含空名称")
            session.Name = session.Name.Trim()
            If Not names.Add(session.Name) Then Throw New FormatException($"会话数据包含重复名称：{session.Name}")
            ValidateSession(session)
        Next
    End Sub

    Private Sub ValidateSession(session As SessionInfo)
        If session.AlgorithmType <> 0 AndAlso session.AlgorithmType <> 1 Then Throw New FormatException($"会话 {session.Name} 的算法类型无效")
        If String.IsNullOrEmpty(session.PublicXml) OrElse String.IsNullOrEmpty(session.PrivateXml) Then Throw New FormatException($"会话 {session.Name} 的密钥不完整")
        Dim publicIsX25519 = session.PublicXml.StartsWith("X25519:", StringComparison.Ordinal)
        Dim privateIsX25519 = session.PrivateXml.StartsWith("X25519:", StringComparison.Ordinal)
        If publicIsX25519 <> privateIsX25519 OrElse publicIsX25519 <> (session.AlgorithmType = 1) Then
            Throw New FormatException($"会话 {session.Name} 的算法与密钥类型不匹配")
        End If
        If session.AlgorithmType = 1 Then session.DoUseCompress = True
    End Sub

    Private Sub LoadLegacySessions()
        Dim names As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
        For Each line In File.ReadAllLines("sessions.txt")
            If String.IsNullOrWhiteSpace(line) Then Continue For
            Dim parts = line.Split("|"c)
            If parts.Length < 1 OrElse String.IsNullOrWhiteSpace(parts(0)) Then Throw New FormatException("旧会话数据格式错误")
            Dim name = parts(0).Trim()
            If Not names.Add(name) Then Throw New FormatException($"旧会话数据包含重复名称：{name}")
            Dim alg As Integer
            Dim doComp = parts.Length >= 3 AndAlso parts(2) = "1"
            If parts.Length >= 2 AndAlso Not Integer.TryParse(parts(1), alg) Then Throw New FormatException($"会话 {name} 的算法类型无效")
            Dim safeName = SanitizeFileName(name)
            Dim pubFile = $"{safeName}_public.xml"
            Dim privFile = $"{safeName}_private.xml"
            If Not File.Exists(pubFile) OrElse Not File.Exists(privFile) Then Throw New FileNotFoundException($"会话 {name} 的密钥文件缺失")
            sessions.Add(New SessionInfo With {
                .Name = name,
                .PublicXml = File.ReadAllText(pubFile),
                .PrivateXml = File.ReadAllText(privFile),
                .AlgorithmType = alg,
                .DoUseCompress = doComp
            })
        Next
        ValidateSessions(sessions)
    End Sub

    Private Sub DeleteLegacySessionFiles()
        For Each session In sessions
            Dim safeName = SanitizeFileName(session.Name)
            Dim pubFile = $"{safeName}_public.xml"
            Dim privFile = $"{safeName}_private.xml"
            If File.Exists(pubFile) Then File.Delete(pubFile)
            If File.Exists(privFile) Then File.Delete(privFile)
        Next
        File.Delete("sessions.txt")
    End Sub

    '将会话名转换为安全的文件名
    Private Function SanitizeFileName(name As String) As String
        If String.IsNullOrWhiteSpace(name) Then Return "session"
        Dim result = name.Trim()
        For Each c In Path.GetInvalidFileNameChars()
            result = result.Replace(c, "_"c)
        Next
        '限制长度以避免过长的文件名
        If result.Length > 100 Then
            result = result.Substring(0, 100)
        End If
        If String.IsNullOrEmpty(result) Then Return "session"
        Return result
    End Function

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        If ListBox1.SelectedIndex >= 0 AndAlso ListBox1.SelectedIndex < sessions.Count Then
            Dim si = sessions(ListBox1.SelectedIndex)
            PublicKeyXml = si.PublicXml
            PrivateKeyXml = si.PrivateXml
            isLoadingSession = True
            Try
                RadioButton1.Checked = (si.AlgorithmType = 1)
                RadioButton2.Checked = (si.AlgorithmType <> 1)
                chkCompress.Enabled = RadioButton2.Checked
                chkCompress.Checked = (si.AlgorithmType = 1 OrElse si.DoUseCompress)
            Finally
                isLoadingSession = False
            End Try
            txtMain.Clear()
            Statuslbl.Text = $"已切换到会话: {si.Name}"
        End If
    End Sub

    Private Async Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Statuslbl.Text = "正在检查更新..."
        Dim url As String = "https://lcs.rth1.xyz/documents/tylanguagetrans.txt"
        Using httpClient As New HttpClient()
            httpClient.Timeout = TimeSpan.FromMinutes(10)
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36")
            httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8")
            Try
                Using response As HttpResponseMessage = Await httpClient.GetAsync(url)
                    response.EnsureSuccessStatusCode()
                    Dim content As String = Await response.Content.ReadAsStringAsync()
                    If content.Length > 4096 Then Throw New FormatException("更新信息格式错误")
                    Dim contents As String() = content.Split(",")
                    Dim CurrentVersion As Version = Application.Info.Version
                    If contents.Length < 2 Then Throw New FormatException("更新信息格式错误")
                    Dim LatestVersion As Version = Version.Parse(contents(0).Trim())
                    If LatestVersion > CurrentVersion Then
                        Dim downloadUri As New Uri(contents(1).Trim(), UriKind.Absolute)
                        If downloadUri.Scheme <> Uri.UriSchemeHttps Then Throw New FormatException("更新下载地址必须使用 HTTPS")
                        Dim expectedHash = If(contents.Length >= 3, contents(2).Trim(), "")
                        If expectedHash.Length > 0 AndAlso (expectedHash.Length <> 64 OrElse Not expectedHash.All(Function(c) Uri.IsHexDigit(c))) Then
                            Throw New FormatException("更新包 SHA-256 格式错误")
                        End If

                        Dim warning = If(expectedHash.Length = 0, vbCrLf & vbCrLf & "当前更新清单未提供 SHA-256，无法验证文件完整性。", "")
                        Dim answer = MsgBox("检测到新版本，是否立即下载并启动？" & vbCrLf & "当前版本：" & CurrentVersion.ToString() & vbCrLf & "最新版本：" & LatestVersion.ToString() & warning,
                                    MsgBoxStyle.YesNo + MsgBoxStyle.Question, "更新提示")
                        If answer <> MsgBoxResult.Yes Then
                            Statuslbl.Text = "已取消更新。"
                            Return
                        End If

                        Statuslbl.Text = "正在下载新版本..."
                        Dim updateRoot = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "LCS", "tylanguagetrans", "updates")
                        Directory.CreateDirectory(updateRoot)
                        Dim zipPath = Path.Combine(updateRoot, $"update-{Guid.NewGuid():N}.zip")
                        Dim extractPath = Path.Combine(updateRoot, $"{LatestVersion}-{Guid.NewGuid():N}")
                        Try
                            Await DownloadUpdateAsync(httpClient, downloadUri, zipPath)
                            If expectedHash.Length > 0 Then VerifyFileSha256(zipPath, expectedHash)
                            Statuslbl.Text = "正在解压新版本..."
                            ExtractUpdateSafely(zipPath, extractPath)

                            Dim executables = Directory.GetFiles(extractPath, "tylanguagetrans.exe", SearchOption.AllDirectories)
                            If executables.Length <> 1 Then Throw New InvalidDataException("更新包中必须包含且只能包含一个 tylanguagetrans.exe")
                            Process.Start(New ProcessStartInfo With {
                        .FileName = executables(0),
                        .WorkingDirectory = Path.GetDirectoryName(executables(0)),
                        .UseShellExecute = True
                    })
                            System.Windows.Forms.Application.Exit()
                        Catch
                            If Directory.Exists(extractPath) Then Directory.Delete(extractPath, True)
                            Throw
                        Finally
                            If File.Exists(zipPath) Then File.Delete(zipPath)
                        End Try
                    Else
                        Statuslbl.Text = "当前已是最新版本!"
                    End If
                End Using
            Catch ex As HttpRequestException
                MsgBox("获取更新失败：发送请求时出错，可能是无网络连接、防火墙阻止或LCS服务出现问题！", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "错误")
                Statuslbl.Text = $"检查更新失败。"
            Catch ex As TaskCanceledException
                MsgBox("获取更新失败：请求超时，可能是网络连接差或LCS服务出现问题！" & vbCrLf & "详细信息：", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "错误")
                Statuslbl.Text = $"检查更新失败。"
            Catch ex As Exception
                MsgBox("获取更新失败：发生未知错误。" & vbCrLf & "详细信息：" & ex.Message, MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "错误")
                Statuslbl.Text = $"检查更新失败。"
            End Try
        End Using
    End Sub

    Private Async Function DownloadUpdateAsync(httpClient As HttpClient, downloadUri As Uri, destinationPath As String) As Task
        Using response = Await httpClient.GetAsync(downloadUri, HttpCompletionOption.ResponseHeadersRead)
            response.EnsureSuccessStatusCode()
            Dim finalUri = response.RequestMessage?.RequestUri
            If finalUri Is Nothing OrElse finalUri.Scheme <> Uri.UriSchemeHttps Then Throw New HttpRequestException("更新下载重定向到了非 HTTPS 地址")
            If response.Content.Headers.ContentLength.HasValue AndAlso response.Content.Headers.ContentLength.Value > MaxUpdateDownloadBytes Then
                Throw New InvalidDataException("更新包超过 512 MB 限制")
            End If

            Using source = Await response.Content.ReadAsStreamAsync(), destination = New FileStream(destinationPath, FileMode.CreateNew, FileAccess.Write, FileShare.None, 81920, True)
                Dim buffer(81919) As Byte
                Dim total As Long
                Do
                    Dim read = Await source.ReadAsync(buffer.AsMemory(0, buffer.Length))
                    If read = 0 Then Exit Do
                    total += read
                    If total > MaxUpdateDownloadBytes Then Throw New InvalidDataException("更新包超过 512 MB 限制")
                    Await destination.WriteAsync(buffer.AsMemory(0, read))
                Loop
            End Using
        End Using
    End Function

    Private Sub VerifyFileSha256(filePath As String, expectedHash As String)
        Using stream = File.OpenRead(filePath)
            Dim actualHash = Convert.ToHexString(SHA256.HashData(stream))
            If Not String.Equals(actualHash, expectedHash, StringComparison.OrdinalIgnoreCase) Then
                Throw New CryptographicException("更新包 SHA-256 校验失败")
            End If
        End Using
    End Sub

    Private Sub ExtractUpdateSafely(zipPath As String, destinationDirectory As String)
        Directory.CreateDirectory(destinationDirectory)
        Dim destinationRoot = Path.GetFullPath(destinationDirectory) & Path.DirectorySeparatorChar
        Using archive = ZipFile.OpenRead(zipPath)
            If archive.Entries.Count > MaxUpdateEntries Then Throw New InvalidDataException("更新包文件数量过多")
            Dim totalExtracted As Long
            For Each entry In archive.Entries
                totalExtracted += entry.Length
                If totalExtracted > MaxUpdateExtractedBytes Then Throw New InvalidDataException("更新包解压后超过 1 GB 限制")

                Dim targetPath = Path.GetFullPath(Path.Combine(destinationDirectory, entry.FullName))
                If Not targetPath.StartsWith(destinationRoot, StringComparison.OrdinalIgnoreCase) Then Throw New InvalidDataException("更新包包含非法路径")
                If String.IsNullOrEmpty(entry.Name) Then
                    Directory.CreateDirectory(targetPath)
                Else
                    Dim parent = Path.GetDirectoryName(targetPath)
                    If Not String.IsNullOrEmpty(parent) Then Directory.CreateDirectory(parent)
                    entry.ExtractToFile(targetPath, False)
                End If
            Next
        End Using
    End Sub


End Class
