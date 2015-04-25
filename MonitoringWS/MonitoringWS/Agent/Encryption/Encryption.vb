Imports MonitoringWS.Agent.AgentParameters
Imports System.Security
Imports System.Security.Cryptography

Public Class Encryption

    Public Shared AES As New AesCryptoServiceProvider

    Public Shared Function EncryptData(ByVal PlainText As String) As String

        Dim PlainTextBytes() As Byte = System.Text.Encoding.ASCII.GetBytes(PlainText)
        Dim mStream As New System.IO.MemoryStream
        Dim EncStream As New CryptoStream(mStream, AES.CreateEncryptor(Key, IV), System.Security.Cryptography.CryptoStreamMode.Write)
        EncStream.Write(PlainTextBytes, 0, PlainTextBytes.Length)
        EncStream.FlushFinalBlock()

        Return Convert.ToBase64String(mStream.ToArray)
    End Function

    Public Shared Function DecryptData(ByVal EncryptedText As String) As String

        Dim EncryptedBytes() As Byte = Convert.FromBase64String(encryptedtext)
        Dim mStream As New System.IO.MemoryStream
        Dim DecStream As New CryptoStream(mStream, AES.CreateDecryptor(Key, IV), System.Security.Cryptography.CryptoStreamMode.Write)
        DecStream.Write(encryptedBytes, 0, encryptedBytes.Length)
        DecStream.FlushFinalBlock()

        Return System.Text.Encoding.ASCII.GetString(mStream.ToArray)
    End Function


End Class

