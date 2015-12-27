Imports MonitoringAgent.Agent.AgentParameters
Imports System.IO

Public Class NetworkLog

    Public Sub InitializeLog()
        AgentNetLog = "Initializing Log..." & vbCrLf
        SyncLock (Lock)
            File.WriteAllText(AgentPath & "NetworkLog.log", AgentNetLog)
        End SyncLock
    End Sub

    Public Sub WriteToLog(ByVal Message As String)
        If Not AgentNetLog Is Nothing Then
            If AgentNetLog.Length > 10000 Then
                AgentNetLog = Nothing
            End If
        End If
        AgentNetLog = AgentNetLog & Date.Now & " [CLIENT] " & Message & vbCrLf
        SyncLock (Lock)
            File.WriteAllText(AgentPath & "NetworkLog.log", AgentNetLog)
        End SyncLock
    End Sub

    Private Lock As New Object

End Class
