Imports MonitoringAgent.Agent.AgentParameters

Namespace Agent

    Public Class Database
        Public Shared Property AgentSystemList As New List(Of AgentSystem)
        Public Shared Property AgentDataList As New List(Of AgentData)
        Public Shared Property AgentConfigList As New List(Of AgentConfiguration)

        Public Sub UpdateTables()
            For Each i In AgentDataList
                i.AgentDataSent = True
            Next
        End Sub

        Public Sub PurgeTables()
            Dim PurgeDate = AgentDate.AddDays(-1)
            'Purge data older than 1 day
            AgentDataList.RemoveAll(Function(x) x.AgentDate < PurgeDate)
        End Sub

    End Class

End Namespace

