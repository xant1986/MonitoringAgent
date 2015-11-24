Namespace Agent
    Public Class AgentParameters

        'Agent Parameters
        Public Shared AgentDate As Date = Date.Now
        Public Shared AgentName As String = Net.Dns.GetHostName
        Public Shared AgentPath As String = Reflection.Assembly.GetEntryAssembly.Location.Replace("MonitoringWS.exe", "")
        Public Shared AgentPollPeriod As Integer = 1
        Public Shared AgentServer As String = Nothing
        Public Shared AgentVersion As String = Nothing
        Public Shared TCPListenPort As String = Nothing
        Public Shared TCPSendPort As String = Nothing

        'Logging
        Public Shared AgentNetLog As String = Nothing

        'Cryptography
        Public Shared Key As Byte() = Text.Encoding.ASCII.GetBytes("abcdefghijklmnop")
        Public Shared IV As Byte() = Text.Encoding.ASCII.GetBytes("abcdefghijklmnop")

    End Class

End Namespace
