Imports System.Threading

Public Class AgentWMI

    Public Sub GetWMI()

        Dim ComputerSystem As New Agent.ComputerSystem
        Dim LogicalDisk As New Agent.LogicalDisk
        Dim Memory As New Agent.Memory
        Dim Network As New Agent.Network
        Dim OperatingSystem As New Agent.OperatingSystem
        Dim Processor As New Agent.Processor
        Dim Services As New Agent.Services

        ComputerSystem.GetComputerSystem()
        LogicalDisk.GetLogicalDisk()
        Memory.GetMemory()
        Network.GetNetwork()
        OperatingSystem.GetOperatingSystem()
        Processor.GetProcessor()
        Services.GetServices()

        Dim SendTCP As New SendTCP
        SendTCP.SendData()

    End Sub
End Class
