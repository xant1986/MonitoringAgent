Imports MonitoringWS.Agent
Imports MonitoringWS.AgentLoad
Imports MonitoringWS.SendClass
Imports MonitoringWS.Agent.AgentParameters
Imports MonitoringWS.ReceiveClass
Imports System.Threading


Public Class Service

    Protected Overrides Sub OnStart(ByVal args() As String)
        ' Add code here to start your service. This method should set things
        ' in motion so your service can do its work.

        LoadParameters()

        Dim t As Thread
        t = New Thread(AddressOf WaitforPackets)
        t.Start()

        Dim LaunchTimer As New System.Timers.Timer
        AddHandler LaunchTimer.Elapsed, AddressOf Tick
        LaunchTimer.Interval = AgentPollPeriod * 60000
        LaunchTimer.Enabled = True
        LaunchTimer.Start()

    End Sub

    Protected Overrides Sub OnStop()
        ' Add code here to perform any tear-down necessary to stop your service.
    End Sub

    Private Sub Tick(sender As System.Object, e As System.EventArgs)
        Dim t As Thread
        t = New Thread(AddressOf WMIQuery)
        t.Start()
    End Sub

    Public Shared Sub WMIQuery()
        Try
            OperatingSystem.GetOperatingSystem()
            ComputerSystem.GetComputerSystem()
            Network.GetNetwork()
            Processor.GetProcessor()
            Memory.GetMemory()
            LogicalDisk.GetLogicalDisk()
            Services.GetServices()
            SendData()
        Catch Ex As Exception
        End Try
    End Sub


End Class
