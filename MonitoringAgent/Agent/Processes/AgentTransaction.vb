Imports MonitoringAgent.Agent.AgentParameters
Imports MonitoringAgent.Agent.Database
Imports MonitoringAgent.Agent
Public Class AgentTransaction

    Public Sub RunTransaction()

        'Run WMI Collection
        GetWMI()

        'Send Data when WMI Collection is complete
        If SSLEnabled = True Then
            Dim AgentSend As New SendSSL
            AgentSend.SendData()
        ElseIf SSLEnabled = False Then
            Dim AgentSend As New SendTCP
            AgentSend.SendData()
        End If

    End Sub


    Private Sub GetWMI()

        'Initialize Database
        AgentSystemList.Clear()
        AgentDataList.Clear()
        AgentStateList.Clear()

        'Run Collection
        Dim CollectSystem As New CollectSystem
        Dim CollectData As New CollectData
        CollectSystem.GetSystem()
        CollectData.GetData()

    End Sub
End Class
