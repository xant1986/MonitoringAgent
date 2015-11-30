Imports MonitoringWS.Agent.AgentParameters
Imports System.Threading

Public Class Service

    Private AgentReceiveThread As Thread
    Private AgentCollectThread As Thread

    Protected Overrides Sub OnStart(ByVal args() As String)

        Dim AgentLoad As New AgentLoad
        AgentLoad.LoadParameters()

        Dim nReceive As New ReceiveTCP

        AgentReceiveThread = New Thread(AddressOf nReceive.StartListener)
        AgentReceiveThread.Start()

        Dim LaunchTimer As New Timers.Timer
        AddHandler LaunchTimer.Elapsed, AddressOf Tick
        LaunchTimer.Interval = AgentPollPeriod * 60000
        LaunchTimer.Enabled = True
        LaunchTimer.Start()

    End Sub

    Protected Overrides Sub OnStop()

        If AgentReceiveThread.IsAlive = True Then
            AgentReceiveThread.Abort()
        End If

        If AgentCollectThread.IsAlive = True Then
            AgentCollectThread.Abort()
        End If

    End Sub

    Private Sub Tick(sender As System.Object, e As System.EventArgs)
        Dim AgentWMI As New AgentWMI
        AgentCollectThread = New Thread(AddressOf AgentWMI.GetWMI)
        AgentCollectThread.Start()
    End Sub

End Class
