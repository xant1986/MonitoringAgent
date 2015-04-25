Imports System.Threading
Imports System.Management

Namespace Agent
    Public Class OperatingSystem

        Public Shared Sub GetOperatingSystem()

            Dim wmiDataList As New List(Of String)
            Dim qString As String = "SELECT * FROM Win32_OperatingSystem"
            Dim searcher As New ManagementObjectSearcher("root\CIMV2", qString)

            Try
                For Each queryObj As ManagementObject In searcher.Get()
                    wmiDataList.Add(queryObj("Caption"))
                    wmiDataList.Add(queryObj("BuildNumber"))
                    wmiDataList.Add(queryObj("OSArchitecture"))
                Next
            Catch err As ManagementException
            End Try

            Try
                Database.AgentDataList.Add(New AgentData With {.AgentName = AgentParameters.AgentName, .AgentDate = AgentParameters.AgentDate, .AgentClass = "System", .AgentProperty = "Version", .AgentValue = wmiDataList.Item(0), .AgentInstance = 0})
                Database.AgentDataList.Add(New AgentData With {.AgentName = AgentParameters.AgentName, .AgentDate = AgentParameters.AgentDate, .AgentClass = "System", .AgentProperty = "Build Number", .AgentValue = wmiDataList.Item(1), .AgentInstance = 0})
                Database.AgentDataList.Add(New AgentData With {.AgentName = AgentParameters.AgentName, .AgentDate = AgentParameters.AgentDate, .AgentClass = "System", .AgentProperty = "OS Architecture", .AgentValue = wmiDataList.Item(2), .AgentInstance = 0})
            Catch ex As Exception
            End Try

        End Sub





    End Class
End Namespace

