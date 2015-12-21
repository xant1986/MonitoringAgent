Imports System.Management
Imports System.Math

Namespace Agent
    Public Class ComputerSystem

        Public Sub GetComputerSystem()

            Dim wmiDataList As New List(Of String)
            Dim qString As String = "SELECT * FROM Win32_ComputerSystem"
            Dim searcher As New ManagementObjectSearcher("root\CIMV2", qString)

            Try
                For Each queryObj As ManagementObject In searcher.Get()
                    wmiDataList.Add(queryObj("Domain"))
                    wmiDataList.Add(queryObj("NumberOfProcessors"))
                    wmiDataList.Add(queryObj("TotalPhysicalMemory"))
                Next
            Catch err As ManagementException
            End Try

            Dim TotalMemory As Double = Nothing


            Try
                TotalMemory = Round(wmiDataList.Item(2) / 1024 / 1024)

                Database.AgentDataList.Add(New AgentData With {.AgentName = AgentParameters.AgentName, .AgentDate = AgentParameters.AgentDate, .AgentClass = "System", .AgentProperty = "Domain", .AgentValue = wmiDataList.Item(0)})
                Database.AgentDataList.Add(New AgentData With {.AgentName = AgentParameters.AgentName, .AgentDate = AgentParameters.AgentDate, .AgentClass = "System", .AgentProperty = "Processors", .AgentValue = wmiDataList.Item(1)})
                Database.AgentDataList.Add(New AgentData With {.AgentName = AgentParameters.AgentName, .AgentDate = AgentParameters.AgentDate, .AgentClass = "System", .AgentProperty = "Total Memory (MB)", .AgentValue = TotalMemory})
            Catch ex As Exception
            End Try

        End Sub


    End Class
End Namespace

