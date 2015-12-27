Imports System.Management
Imports System.Math
Namespace Agent
    Public Class CollectData

        Public Sub GetData()

            GetProcessor()
            GetMemory()
            GetLogicalDisk()
            GetServices()

        End Sub

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
                Database.AgentDataList.Add(New AgentData With {.AgentName = AgentParameters.AgentName, .AgentClass = "Processor", .AgentProperty = "Total Util (%)", .AgentValue = TotalProcessor})
            Catch ex As Exception
            End Try

        End Sub

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
                Database.AgentDataList.Add(New AgentData With {.AgentName = AgentParameters.AgentName, .AgentClass = "Memory", .AgentProperty = "Total Util (%)", .AgentValue = MemoryPercent})
            Catch ex As Exception
            End Try

        End Sub

        Public Sub GetLogicalDisk()

            Dim LDSize As Double = Nothing
            Dim LDFreeSpace As Double = 0
            Dim LDPercentFree As Double = 0
            Dim LDFreeSpaceMB As Double = 0

            Dim wmiDataList As New List(Of LogicalDisk)
            Dim qString As String = "SELECT * FROM Win32_LogicalDisk WHERE Description='Local Fixed Disk'"
            Dim searcher As New ManagementObjectSearcher("root\CIMV2", qString)

            Try
                For Each queryObj As ManagementObject In searcher.Get()
                    wmiDataList.Add(New LogicalDisk With {.Name = queryObj("Name"), .Size = queryObj("Size"), .FreeSpace = queryObj("FreeSpace")})
                Next
            Catch err As ManagementException
            End Try

            Try
                For Each i In wmiDataList
                    LDSize = i.Size
                    LDFreeSpace = i.FreeSpace
                    If LDSize <> 0 And LDFreeSpace <> 0 Then
                        LDPercentFree = Round((LDFreeSpace / LDSize) * 100)
                    End If
                    LDFreeSpaceMB = Round(LDFreeSpace / 1024 / 1024)
                    Database.AgentDataList.Add(New AgentData With {.AgentName = AgentParameters.AgentName, .AgentClass = "Local Disk (" & i.Name & ")", .AgentProperty = "Free Space (%)", .AgentValue = LDPercentFree})
                    Database.AgentDataList.Add(New AgentData With {.AgentName = AgentParameters.AgentName, .AgentClass = "Local Disk (" & i.Name & ")", .AgentProperty = "Free Space (MB)", .AgentValue = LDFreeSpaceMB})
                Next
            Catch ex As Exception
            End Try

        End Sub


        Public Sub GetServices()

            Dim Q = From T In Database.AgentConfigList
                    Where T.AgentClass = "windows" And T.AgentProperty = "service"
                    Select T.AgentValue
            For Each i In Q
                Dim qString As String = "SELECT * FROM Win32_Service where Name='" & i & "'"
                Dim Searcher As New ManagementObjectSearcher("root\CIMV2", qString)
                Dim State = Nothing
                Try
                    For Each queryObj As ManagementObject In Searcher.Get()
                        If queryObj("State") = "Running" Then
                            State = 1
                        Else
                            State = 0
                        End If
                        Database.AgentDataList.Add(New AgentData With {.AgentName = AgentParameters.AgentName, .AgentClass = "Services", .AgentProperty = queryObj("DisplayName"), .AgentValue = State})
                    Next
                Catch err As ManagementException
                End Try
            Next


        End Sub

    End Class

    Public Class LogicalDisk
        Public Property Name
        Public Property Size
        Public Property FreeSpace
    End Class

End Namespace
