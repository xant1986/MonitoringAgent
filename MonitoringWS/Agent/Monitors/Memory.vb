Imports System.Management
Imports System.Math

Namespace Agent
    Public Class Memory

        Public Sub GetMemory()

            Dim wmiDataList As New List(Of String)
            Dim qString As String = "SELECT * FROM Win32_OperatingSystem"
            Dim searcher As New ManagementObjectSearcher("root\CIMV2", qString)

            Try
                For Each queryObj As ManagementObject In searcher.Get()
                    wmiDataList.Add(queryObj("FreePhysicalMemory"))
                    wmiDataList.Add(queryObj("TotalVisibleMemorySize"))
                Next
            Catch err As ManagementException
            End Try

            Try
                Dim MemoryPercent As Integer = Round(((wmiDataList.Item(1) - wmiDataList.Item(0)) / wmiDataList.Item(1)) * 100)
                Database.AgentDataList.Add(New AgentData With {.AgentName = AgentParameters.AgentName, .AgentDate = AgentParameters.AgentDate, .AgentClass = "Memory", .AgentProperty = "Total Util (%)", .AgentValue = MemoryPercent})
            Catch ex As Exception
            End Try

        End Sub

    End Class
End Namespace
