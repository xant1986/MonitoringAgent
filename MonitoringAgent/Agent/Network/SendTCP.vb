Imports MonitoringAgent.Agent.AgentParameters
Imports System.Net.Sockets
Imports System.Text
Imports System.IO

Public Class SendTCP

    Public NetworkLog As New NetworkLog

    Public Sub SendData()
        Try
            NetworkLog.WriteToLog("Atempting to connect to " & AgentServer & " on port " & TCPSendPort)
            Dim Client As New TcpClient
            Client.BeginConnect(AgentServer, TCPSendPort, New AsyncCallback(AddressOf ConnectCallback), Client)
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
                Dim NStream As NetworkStream = Client.GetStream

                'For sending non-compressed data.
                'Dim PacketData As Byte() = Encoding.ASCII.GetBytes(Packet)
                'NStream.Write(PacketData, 0, PacketData.Length)

                Dim CompressedPacket As String = Nothing
                Dim Compression As New Compression
                CompressedPacket = Compression.CompressedData(Packet)
                Dim CompressedPacketData As Byte() = Encoding.UTF8.GetBytes(CompressedPacket)
                NStream.Write(CompressedPacketData, 0, CompressedPacketData.Length)

                NetworkLog.WriteToLog("Sending packet")

                'View Packet data debug
                'NetworkLog.WriteToLog(Packet)

                Dim Reader As New StreamReader(NStream)
                While Reader.Peek > -1
                    Message = Message + Convert.ToChar(Reader.Read)
                End While
                NStream.Close()
                Client.Close()
                NetworkLog.WriteToLog(Message)

            Else
                NetworkLog.WriteToLog("Connection failed")
            End If

        Catch ex As Exception
            NetworkLog.WriteToLog(ex.ToString)
        End Try

    End Sub


End Class

