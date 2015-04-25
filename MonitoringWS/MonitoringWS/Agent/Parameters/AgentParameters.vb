Namespace Agent
    Public Class AgentParameters

        Public Shared AgentName As String = System.Net.Dns.GetHostName
        Public Shared AgentPath As String = System.Reflection.Assembly.GetEntryAssembly.Location.Replace("MonitoringWS.exe", "")
        Public Shared AgentDate As Date = Date.Now
        Public Shared AgentVersion As String = Nothing
        Public Shared AgentServer As String = Nothing
        Public Shared TCPListenPort As String = Nothing
        Public Shared TCPSendPort As String = Nothing
        Public Shared AgentPollPeriod As Integer = 1
    
        'Cryptography
        Public Shared Key As Byte() = System.Text.Encoding.ASCII.GetBytes("abcdefghijklmnop")
        Public Shared IV As Byte() = System.Text.Encoding.ASCII.GetBytes("abcdefghijklmnop")

    End Class

End Namespace
