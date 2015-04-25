Imports MonitoringWS.SendClass
Imports MonitoringWS.Agent.AgentParameters
Imports MonitoringWS.AgentLoad
Imports MonitoringWS.Encryption
Imports System.Net
Imports System.Net.Sockets
Imports System.IO

Public Class ReceiveClass

    Public Shared Sub WaitforPackets()


        Dim ListenPort As Int32 = TCPListenPort
        Dim ListenAddress As IPAddress = IPAddress.Any

        Dim Listener As TcpListener = New TcpListener(ListenAddress, ListenPort)
        Listener.Start()

        While True

            SendData()

            Dim Message As String = Nothing

            Try
                Dim Reader As New System.IO.StreamReader(Listener.AcceptTcpClient.GetStream())
                While Reader.Peek > -1
                    Message = Message + Convert.ToChar(Reader.Read)
                End While
            Catch ex As Exception
            Finally
                Console.WriteLine(Message)
                Message = DecryptData(Message)
                Console.WriteLine(Message)

                TranslateXML(Message)
                Console.ReadLine()

            End Try
        End While

    End Sub


    Public Shared Sub TranslateXML(ByVal xmlMessage As String)

        Try
            If xmlMessage.Contains("agent-config") Then
                File.WriteAllText(AgentPath & "\AgentConfiguration.xml", xmlMessage)
                LoadXML()
            End If

        Catch ex As Exception

        End Try

    End Sub



End Class
