Imports System.Management

Namespace Agent

    Public Class Processor

        Public Sub GetProcessor()

            Dim wmiDataList As New List(Of String)

            Dim qString As String = "SELECT * FROM Win32_PerfFormattedData_PerfOS_Processor WHERE Name='_Total'"
            Dim searcher As New ManagementObjectSearcher("root\CIMV2", qString)
            Dim TotalProcessor As String = Nothing

            Try
                For Each queryObj As ManagementObject In searcher.Get()
                    TotalProcessor = queryObj("PercentProcessorTime")
                Next
            Catch err As ManagementException
            End Try

            Try
                Database.AgentDataList.Add(New AgentData With {.AgentName = AgentParameters.AgentName, .AgentDate = AgentParameters.AgentDate, .AgentClass = "Processor", .AgentProperty = "Total Util (%)", .AgentValue = TotalProcessor})
            Catch ex As Exception
            End Try

        End Sub

    End Class

End Namespace
