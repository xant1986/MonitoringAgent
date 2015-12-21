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


        Dim Database As New Agent.Database
        Dim Message As String = Nothing
        Dim Client As TcpClient = CType(ar.AsyncState, TcpClient)

        If Client.Connected Then
            NetworkLog.WriteToLog("Agent connected to " & AgentServer & " on port " & TCPSendPort)
            Dim NStream As NetworkStream = Client.GetStream
            Dim PacketData As Byte() = Encoding.ASCII.GetBytes(Packet)
            NStream.Write(PacketData, 0, PacketData.Length)
            NetworkLog.WriteToLog("Sending packet")

            Dim Reader As New StreamReader(NStream)
            While Reader.Peek > -1
                Message = Message + Convert.ToChar(Reader.Read)
            End While
            NStream.Close()
            Client.Close()
            NetworkLog.WriteToLog(Message)
            Database.UpdateTables()
        Else
            NetworkLog.WriteToLog("Connection failed")
        End If
        Database.PurgeTables()
        Database.SaveDatabase()

    End Sub


End Class

