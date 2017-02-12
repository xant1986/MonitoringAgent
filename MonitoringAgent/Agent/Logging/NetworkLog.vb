Imports MonitoringAgent.Agent.AgentParameters
Imports System.IO

Public Class NetworkLog

    Public Sub InitializeLog()
        AgentNetLog = "Monitoring Agent Version 1.0.1, Copyright 2016 Phil White" & vbCrLf & "Initializing Log..." & vbCrLf
        SyncLock (Lock)
            File.WriteAllText(AgentPath & "MonitoringAgent.log", AgentNetLog)
        End SyncLock
    End Sub

    Public Sub WriteToLog(ByVal Message As String)
        If Not AgentNetLog Is Nothing Then
            If AgentNetLog.Length > 100000 Then
                AgentNetLog = Nothing
            End If
        End If
        AgentNetLog = AgentNetLog & Date.Now & " [CLIENT] " & Message & vbCrLf
        SyncLock (Lock)
            File.WriteAllText(AgentPath & "MonitoringAgent.log", AgentNetLog)
        End SyncLock
    End Sub

    Private Lock As New Object

End Class
