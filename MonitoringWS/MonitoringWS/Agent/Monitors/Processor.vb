Imports System.Threading
Imports System.Management
Imports System.Math

Namespace Agent

    Public Class Processor

        Public Shared Sub GetProcessor()

            Dim wmiDataList As New List(Of String)

            Dim qString As String = "SELECT * FROM Win32_PerfRawData_PerfOS_Processor WHERE Name='_Total'"
            Dim searcher As New ManagementObjectSearcher("root\CIMV2", qString)

            For i = 0 To 1
                Try
                    For Each queryObj As ManagementObject In searcher.Get()
                        wmiDataList.Add(queryObj("PercentProcessorTime"))
                        wmiDataList.Add(queryObj("Timestamp_Sys100NS"))
                        Thread.Sleep(1000)
                    Next
                Catch err As ManagementException
                End Try
            Next

            Try
                Dim ProcessorPercent As Integer = Round(((1 - ((wmiDataList.Item(0) - wmiDataList.Item(2)) / (wmiDataList.Item(1) - wmiDataList.Item(3)))) * 100), 2)
                Database.AgentDataList.Add(New AgentData With {.AgentName = AgentParameters.AgentName, .AgentDate = AgentParameters.AgentDate, .AgentClass = "Processor", .AgentProperty = "Total Util (%)", .AgentValue = ProcessorPercent, .AgentInstance = 0})
            Catch ex As Exception
            End Try

        End Sub

    End Class

End Namespace
