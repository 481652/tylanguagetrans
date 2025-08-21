Imports System.Text
Imports System.Security.Cryptography
Imports System.IO
Imports System.IO.Compression

Public Class Form1

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

    ' 保存当前密钥
    Private PublicKeyXml As String = ""
    Private PrivateKeyXml As String = ""

    ' ========= 按钮事件 =========
    Private Sub btnEncrypt_Click(sender As Object, e As EventArgs) Handles btnEncrypt.Click
        Try
            If PublicKeyXml = "" Then
                MessageBox.Show("要加密，请先导入或生成公钥")
                Return
            End If
            Dim result = EncryptAsymmetricToWords(txtMain.Text, chkCompress.Checked, PublicKeyXml)
            txtMain.Text = result
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
            Dim result = DecryptAsymmetricFromWords(txtMain.Text, chkCompress.Checked, PrivateKeyXml)
            txtMain.Text = result
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
        Dim kp = GenerateRsaXmlPair(3072)
        PublicKeyXml = kp.PublicXml
        PrivateKeyXml = kp.PrivateXml
        File.WriteAllText("public.xml", PublicKeyXml)
        File.WriteAllText("private.xml", PrivateKeyXml)
        MsgBox("密钥对已生成并保存在与本程序同路径的 public.xml / private.xml", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, "信息")
    End Sub

    Private Sub btnImportPub_Click(sender As Object, e As EventArgs) Handles btnImportPub.Click
        Dim ofd As New OpenFileDialog()
        ofd.Filter = "XML 文件|*.xml"
        If ofd.ShowDialog() = DialogResult.OK Then
            PublicKeyXml = File.ReadAllText(ofd.FileName)
            Statuslbl.Text = "公钥已导入。"
        End If
    End Sub

    Private Sub btnImportPriv_Click(sender As Object, e As EventArgs) Handles btnImportPriv.Click
        Dim ofd As New OpenFileDialog()
        ofd.Filter = "XML 文件|*.xml"
        If ofd.ShowDialog() = DialogResult.OK Then
            PrivateKeyXml = File.ReadAllText(ofd.FileName)
            Statuslbl.Text = "私钥已导入。"
        End If
    End Sub

    ' ========= 混合加密实现 =========

    Public Function EncryptAsymmetricToWords(plainText As String, compress As Boolean, rsaPublicXml As String) As String
        Dim data As Byte() = If(compress, CompressString(plainText), Encoding.UTF8.GetBytes(plainText))
        Dim packet As Byte() = HybridEncrypt(data, rsaPublicXml)
        If compress Then
            ' 压缩模式下使用7位编码
            Return BytesToWordString128(packet)
        Else
            ' 非压缩模式下使用6位编码
            Return BytesToWordString(packet)
        End If
    End Function

    Public Function DecryptAsymmetricFromWords(wordCipher As String, compressed As Boolean, rsaPrivateXml As String) As String
        Dim packet As Byte()
        Dim data As Byte()
        If compressed Then
            ' 压缩模式下使用7位编码
            packet = WordStringToBytes128(wordCipher)
            data = HybridDecrypt(packet, rsaPrivateXml)
            Return DecompressBytes(data)
        Else
            packet = WordStringToBytes(wordCipher)
            data = HybridDecrypt(packet, rsaPrivateXml)
            Return Encoding.UTF8.GetString(data)

        End If
    End Function

    Private Function HybridEncrypt(plain As Byte(), rsaPublicXml As String) As Byte()
        Const VERSION As Byte = 2
        Const ALG_ID As Byte = 1

        ' 会话材料
        Dim aesKey(31) As Byte, hmacKey(31) As Byte, iv(15) As Byte ' 16字节IV
        Using rng = RandomNumberGenerator.Create()
            rng.GetBytes(aesKey)
            rng.GetBytes(hmacKey)
            rng.GetBytes(iv)
        End Using

        ' AES加密正文
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

        ' RSA加密会话密钥
        Dim session(63) As Byte
        Buffer.BlockCopy(aesKey, 0, session, 0, 32)
        Buffer.BlockCopy(hmacKey, 0, session, 32, 32)
        Dim rsaEnc As Byte()
        Using rsa As RSA = RSA.Create()
            rsa.FromXmlString(rsaPublicXml)
            Try
                rsaEnc = rsa.Encrypt(session, RSAEncryptionPadding.OaepSHA256)
            Catch
                rsaEnc = rsa.Encrypt(session, RSAEncryptionPadding.OaepSHA1)
            End Try
        End Using

        ' 构造 rsaEnc 长度字段（2字节，大端）
        Dim rsaLenBE = BitConverter.GetBytes(CUShort(rsaEnc.Length))
        If BitConverter.IsLittleEndian Then Array.Reverse(rsaLenBE)

        ' 构造 AAD（version + alg + iv + rsaLenBE + rsaEnc + cipher）
        Dim aadLen = 1 + 1 + iv.Length + 2 + rsaEnc.Length + cipher.Length
        Dim aad(aadLen - 1) As Byte
        Dim off = 0
        aad(off) = VERSION : off += 1
        aad(off) = ALG_ID : off += 1
        Buffer.BlockCopy(iv, 0, aad, off, iv.Length) : off += iv.Length
        Buffer.BlockCopy(rsaLenBE, 0, aad, off, 2) : off += 2
        Buffer.BlockCopy(rsaEnc, 0, aad, off, rsaEnc.Length) : off += rsaEnc.Length
        Buffer.BlockCopy(cipher, 0, aad, off, cipher.Length)

        ' 计算 HMAC-SHA256 并截取前16字节
        Dim tag(15) As Byte
        Using h = New HMACSHA256(hmacKey)
            Dim fullTag = h.ComputeHash(aad)
            Buffer.BlockCopy(fullTag, 0, tag, 0, 16)
        End Using

        ' 组装最终数据包
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
    End Function


    Private Function HybridDecrypt(packet As Byte(), rsaPrivateXml As String) As Byte()
        Using ms As New MemoryStream(packet), br As New BinaryReader(ms)
            Dim version = br.ReadByte()
            Dim alg = br.ReadByte()
            If version <> 2 OrElse alg <> 1 Then Throw New CryptographicException("格式不支持")
            Dim iv = br.ReadBytes(16)
            Dim rsaLenBE = br.ReadBytes(2)
            Dim rsaLen = CUShort((CUShort(rsaLenBE(0)) << 8) Or rsaLenBE(1))
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
            aad(off) = 2 : off += 1 ' version
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
            If session.Length <> 64 Then Throw New CryptographicException("会话密钥长度错误")

            Dim aesKey(31) As Byte, hmacKey(31) As Byte
            Buffer.BlockCopy(session, 0, aesKey, 0, 32)
            Buffer.BlockCopy(session, 32, hmacKey, 0, 32)

            ' 校验 HMAC
            Using h = New HMACSHA256(hmacKey)
                Dim calc = h.ComputeHash(aad)
                ' 只比对前16字节
                For i = 0 To 15
                    If calc(i) <> tag(i) Then
                        Throw New CryptographicException("完整性校验失败")
                    End If
                Next
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
        End Using
    End Function

    Private Function ConstantTimeEqual(a As Byte(), b As Byte()) As Boolean
        If a.Length <> b.Length Then Return False
        Dim diff As Integer = 0
        For i = 0 To a.Length - 1
            diff = diff Or (a(i) Xor b(i))
        Next
        Return diff = 0
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
        For Each pair In parts
            If pair.Length < 4 Then Continue For
            Dim w1 = pair.Substring(0, 2)
            Dim w2 = pair.Substring(2, 2)
            Dim hi = Array.IndexOf(wordList, w1)
            Dim lo = Array.IndexOf(wordList, w2)
            If hi < 0 OrElse lo < 0 Then Throw New FormatException("未知词元，请检查输入是否为ty语！")
            outBytes.Add(CByte(hi * 64 + lo))
        Next
        Return outBytes.ToArray()
    End Function

    Private Function BytesToWordString128(data As Byte()) As String
        Dim sb As New StringBuilder()
        Dim bitBuffer As Integer = 0
        Dim bitCount As Integer = 0
        For Each b In data
            bitBuffer = (bitBuffer << 8) Or b
            bitCount += 8
            While bitCount >= 7
                Dim idx = (bitBuffer >> (bitCount - 7)) And &H7F
                sb.Append(wordList128(idx))
                bitCount -= 7
            End While
        Next
        If bitCount > 0 Then
            Dim idx = (bitBuffer << (7 - bitCount)) And &H7F
            sb.Append(wordList128(idx))
        End If
        Return sb.ToString()
    End Function

    Private Function WordStringToBytes128(s As String) As Byte()
        Dim outBytes As New List(Of Byte)
        Dim bitBuffer As Integer = 0
        Dim bitCount As Integer = 0
        For i = 0 To s.Length - 2 Step 2
            Dim word = s.Substring(i, 2)
            Dim idx = Array.IndexOf(wordList128, word)
            If idx < 0 Then Throw New FormatException("未知词元")
            bitBuffer = (bitBuffer << 7) Or idx
            bitCount += 7
            If bitCount >= 8 Then
                outBytes.Add((bitBuffer >> (bitCount - 8)) And &HFF)
                bitCount -= 8
            End If
        Next
        Return outBytes.ToArray()
    End Function

    ' === 压缩/解压 ===
    Private Function CompressString(input As String) As Byte()
        Dim bytes = Encoding.UTF8.GetBytes(input)
        Using ms As New MemoryStream()
            Using gz As New GZipStream(ms, CompressionLevel.Optimal)
                gz.Write(bytes, 0, bytes.Length)
            End Using
            Return ms.ToArray()
        End Using
    End Function

    Private Function DecompressBytes(data As Byte()) As String
        Using ms As New MemoryStream(data)
            Using gz As New GZipStream(ms, CompressionMode.Decompress)
                Using result As New MemoryStream()
                    gz.CopyTo(result)
                    Return Encoding.UTF8.GetString(result.ToArray())
                End Using
            End Using
        End Using
    End Function

    ' === 生成密钥对 ===
    Public Function GenerateRsaXmlPair(Optional keySize As Integer = 3072) As (PublicXml As String, PrivateXml As String)
        Using rsa As RSA = RSA.Create(keySize)
            Dim pub = rsa.ToXmlString(False)
            Dim priv = rsa.ToXmlString(True)
            Return (pub, priv)
        End Using
    End Function

    ' === 导入密钥对 ===
    Private Sub btnDetectKeys_Click(sender As Object, e As EventArgs) Handles btnDetectKeys.Click
        Dim appPath As String = Directory.GetCurrentDirectory()
        Dim pubExists As Boolean = File.Exists(Path.Combine(appPath, "public.xml"))
        Dim privExists As Boolean = File.Exists(Path.Combine(appPath, "private.xml"))
        If Not pubExists And privExists Then
            MsgBox("导入失败：密钥文件不存在。", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "错误")
            Return
        End If
        PublicKeyXml = File.ReadAllText(Path.Combine(appPath, "public.xml"))
        PrivateKeyXml = File.ReadAllText(Path.Combine(appPath, "private.xml"))
        Statuslbl.Text = "密钥已导入。"
    End Sub


End Class
