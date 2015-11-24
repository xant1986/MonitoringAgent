Imports MonitoringWS.Agent.AgentParameters
Imports System.Threading

Public Class Service

    Protected Overrides Sub OnStart(ByVal args() As String)
        ' Add code here to start your service. This method should set things
        ' in motion so your service can do its work.

        Dim AgentLoad As New AgentLoad
        AgentLoad.LoadParameters()

        Dim nReceive As New ReceiveTCP
        Dim t As Thread
        t = New Thread(AddressOf nReceive.StartListener)
        t.Start()

        Dim LaunchTimer As New Timers.Timer
        AddHandler LaunchTimer.Elapsed, AddressOf Tick
        LaunchTimer.Interval = AgentPollPeriod * 60000
        LaunchTimer.Enabled = True
        LaunchTimer.Start()

    End Sub

    Protected Overrides Sub OnStop()
        ' Add code here to perform any tear-down necessary to stop your service.
    End Sub

    Private Sub Tick(sender As System.Object, e As System.EventArgs)
        Dim AgentWMI As New AgentWMI
        Dim t As Thread
        t = New Thread(AddressOf AgentWMI.GetWMI)
        t.Start()
    End Sub

End Class
