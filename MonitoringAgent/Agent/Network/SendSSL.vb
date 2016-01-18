Imports MonitoringAgent.Agent.AgentParameters
Imports System.Net.Sockets
Imports System.Text
Imports System.IO
Imports System.Net.Security
Imports System.Security.Authentication
Imports System.Security.Cryptography.X509Certificates

Public Class SendSSL

    Public NetworkLog As New NetworkLog

    Public Sub SendData()
        Try
            NetworkLog.WriteToLog("Atempting to connect to " & AgentServer & " on port " & TCPSendPort)
            Dim Client As New TcpClient
            Client.BeginConnect(AgentServer, TCPSendPort, New AsyncCallback(AddressOf ConnectCallback), Client)
            Threading.Thread.Sleep(5000)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub ConnectCallback(ByVal ar As IAsyncResult)
        Dim PClass As New Packets
        Dim Packet = PClass.CreatePacket()
        Dim Database As New Agent.Database
        Dim Message As String = Nothing

        Try
            Dim Client As TcpClient = CType(ar.AsyncState, TcpClient)
            If Client.Connected Then
                NetworkLog.WriteToLog("Agent connected to " & AgentServer & " on port " & TCPSendPort)
                Dim sslStream = New SslStream(Client.GetStream, False, New RemoteCertificateValidationCallback(AddressOf TrustAllCertificatesCallback))
                sslStream.AuthenticateAsClient(AgentServer, Nothing, SslProtocols.Tls12, False)

                'Dim PacketData As Byte() = Encoding.UTF8.GetBytes(Packet)
                'sslstream.Write(PacketData, 0, PacketData.Length)

                Dim CompressedPacket As String = Nothing
                Dim Compression As New Compression
                CompressedPacket = Compression.CompressedData(Packet)
                Dim CompressedPacketData As Byte() = Encoding.UTF8.GetBytes(CompressedPacket)
                sslStream.Write(CompressedPacketData, 0, CompressedPacketData.Length)

                NetworkLog.WriteToLog("Sending packet")
                Dim Reader As New StreamReader(sslstream)
                While Reader.Peek > -1
                    Message = Message + Convert.ToChar(Reader.Read)
                End While
                sslStream.Flush()
                sslStream.Close()
                Client.Close()
                NetworkLog.WriteToLog(Message)
            Else
                NetworkLog.WriteToLog("Connection failed")
            End If

        Catch ex As Exception
            NetworkLog.WriteToLog(ex.ToString)
        End Try

    End Sub

    Public Shared Function TrustAllCertificatesCallback(sender As Object, cert As X509Certificate, chain As X509Chain, errors As System.Net.Security.SslPolicyErrors) As Boolean
        Return True
    End Function

End Class
