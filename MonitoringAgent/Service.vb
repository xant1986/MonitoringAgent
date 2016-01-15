'MonitoringAgent Version 2.3.5
'Copyright 2016 Phil White, wcpSoft
'This software is released under the Apache 2.0 License
'Maintained at http://github.com/philipcwhite

Imports MonitoringAgent.Agent.AgentParameters
Imports System.Threading

Public Class Service

    Private Property AgentCollectThread As Thread

    Protected Overrides Sub OnStart(ByVal args() As String)

        Dim AgentLoad As New AgentLoad
        AgentLoad.LoadParameters()

        Dim Timer As New Timers.Timer
        AddHandler Timer.Elapsed, AddressOf Tick
        Timer.Interval = 1000
        Timer.Enabled = True
        Timer.Start()

    End Sub

    Protected Overrides Sub OnStop()
        Try
            If AgentCollectThread.IsAlive = True Then
                AgentCollectThread.Abort()
            End If
        Catch
        End Try
    End Sub

    Private Sub Tick(sender As System.Object, e As System.EventArgs)

        Dim SystemTime As Date = Date.Now
        If SystemTime.ToString("mm:ss").Substring(1, 4) = "5:00" Or SystemTime.ToString("mm:ss").Substring(1, 4) = "0:00" Then
            AgentDate = SystemTime
            Dim ATransaction As New AgentTransaction
            AgentCollectThread = New Thread(AddressOf ATransaction.RunTransaction)
            AgentCollectThread.IsBackground = True
            AgentCollectThread.Start()
        End If

    End Sub

End Class
