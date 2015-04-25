Imports MonitoringWS.Agent.AgentParameters
Imports MonitoringWS.Agent.Database
Imports MonitoringWS.PacketClass
Imports MonitoringWS.Encryption
Imports System.Net.Sockets
Imports System.IO

Public Class SendClass

    Public Shared Sub SendData()

        Dim Packet As String = Nothing
        Packet = CreatePacket()

        Try
            Dim Port As Int32 = TCPSendPort
            Dim Client As New TcpClient(AgentServer, Port)
            Dim PacketData As Byte() = System.Text.Encoding.ASCII.GetBytes(Packet)
            Dim NStream As NetworkStream = Client.GetStream()
            NStream.Write(PacketData, 0, PacketData.Length)
            'Console.WriteLine(packet)
        Catch ex As Exception

        Finally

            UpdateTables()
            SaveDatabase()
            'AgentDataList.Clear()
        End Try

    End Sub

End Class
