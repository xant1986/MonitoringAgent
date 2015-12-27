Imports MonitoringAgent.Agent.Database
Imports MonitoringAgent.Agent
Public Class AgentTransaction

    Public Sub RunTransaction()

        'Run WMI Collection
        GetWMI()

        'Send Data when WMI Collection is complete
        Dim AgentSend As New SendTCP
        AgentSend.SendData()

    End Sub


    Private Sub GetWMI()


        'Initialize Database
        AgentSystemList.Clear()
        AgentDataList.Clear()

        'Run Collection
        Dim CollectSystem As New CollectSystem
        Dim CollectData As New CollectData
        CollectSystem.GetSystem()
        CollectData.GetData()

    End Sub
End Class
