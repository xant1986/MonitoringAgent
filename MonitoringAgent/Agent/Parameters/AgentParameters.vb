Namespace Agent
    Public Class AgentParameters

        'Agent Parameters
        Public Shared AgentDate As Date = Nothing
        Public Shared AgentName As String = Net.Dns.GetHostName
        Public Shared AgentPath As String = Reflection.Assembly.GetEntryAssembly.Location.Replace("MonitoringAgent.exe", "")
        Public Shared AgentServer As String = Nothing
        Public Shared TCPSendPort As String = Nothing
        Public Shared SSLEnabled As Boolean = False
        'Logging
        Public Shared AgentNetLog As String = Nothing

    End Class

End Namespace
