Imports MonitoringWS.Agent.AgentParameters
Imports System.Security.Cryptography

Public Class Encryption

    Public AES As New AesCryptoServiceProvider

    Public Function EncryptData(ByVal PlainText As String) As String

        Dim PlainTextBytes() As Byte = Text.Encoding.ASCII.GetBytes(PlainText)
        Dim mStream As New IO.MemoryStream
        Dim EncStream As New CryptoStream(mStream, AES.CreateEncryptor(Key, IV), CryptoStreamMode.Write)
        EncStream.Write(PlainTextBytes, 0, PlainTextBytes.Length)
        EncStream.FlushFinalBlock()

        Return Convert.ToBase64String(mStream.ToArray)
    End Function

    Public Function DecryptData(ByVal EncryptedText As String) As String

        Dim EncryptedBytes() As Byte = Convert.FromBase64String(EncryptedText)
        Dim mStream As New IO.MemoryStream
        Dim DecStream As New CryptoStream(mStream, AES.CreateDecryptor(Key, IV), CryptoStreamMode.Write)
        DecStream.Write(EncryptedBytes, 0, EncryptedBytes.Length)
        DecStream.FlushFinalBlock()

        Return Text.Encoding.ASCII.GetString(mStream.ToArray)
    End Function


End Class

