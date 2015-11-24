Imports MonitoringWS.Agent.AgentParameters
Imports System.Net.Sockets
Imports System.Text
Imports System.IO

Public Class SendTCP

    Public NetworkLog As New NetworkLog

    Public Sub SendData()
        Try
            NetworkLog.WriteToLog("Attepting to connect to " & AgentServer & " on port " & TCPSendPort)
            Dim Client As New TcpClient
            Client.BeginConnect(AgentServer, TCPSendPort, New AsyncCallback(AddressOf ConnectCallback), Client)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub ConnectCallback(ByVal ar As IAsyncResult)
        Dim PClass As New Packets
        Dim Packet = PClass.CreatePacket()

        NetworkLog.WriteToLog("Agent connected to " & AgentServer & " on port " & TCPSendPort)

        Try
            Dim Client As TcpClient = CType(ar.AsyncState, TcpClient)
            Dim NStream As NetworkStream = Client.GetStream
            Dim PacketData As Byte() = Encoding.ASCII.GetBytes(Packet)
            NStream.Write(PacketData, 0, PacketData.Length)

            NetworkLog.WriteToLog("Sending packet")

            'Client.ReceiveBufferSize = 1024

            Dim Message As String = Nothing
            Dim Reader As New StreamReader(NStream)
            While Reader.Peek > -1
                Message = Message + Convert.ToChar(Reader.Read)
            End While
            Client.Close()
            'Console.WriteLine("[CLIENT] " & Message)

            NetworkLog.WriteToLog(Message)

        Catch ex As Exception
        Finally
            Dim Database As New Agent.Database
            Database.UpdateTables()
            Database.SaveDatabase()
        End Try

    End Sub


End Class

