Imports System.Management
Imports System.Math
Namespace Agent
    Public Class CollectData

        Public Sub GetData()

            GetProcessor()
            GetMemory()
            GetPageFile()
            GetLocalDiskSpace()
            GetNetwork()
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

        Public Sub GetPageFile()
            Dim wmiDataList As New List(Of String)

            Dim qString As String = "SELECT * FROM Win32_PerfFormattedData_PerfOS_PagingFile WHERE Name='_Total'"
            Dim searcher As New ManagementObjectSearcher("root\CIMV2", qString)
            Dim PercentUsage As String = Nothing

            Try
                For Each queryObj As ManagementObject In searcher.Get()
                    PercentUsage = queryObj("PercentUsage")
                Next
            Catch err As ManagementException
            End Try

            Try
                Database.AgentDataList.Add(New AgentData With {.AgentName = AgentParameters.AgentName, .AgentClass = "PageFile", .AgentProperty = "Total Util (%)", .AgentValue = PercentUsage})
            Catch ex As Exception
            End Try
        End Sub


        Public Sub GetLocalDiskSpace()

            Dim LDActiveTime As Double = 0
            Dim wmiDataList As New List(Of WMIData)
            Dim qString As String = "SELECT * FROM Win32_PerfFormattedData_PerfDisk_LogicalDisk"
            Dim searcher As New ManagementObjectSearcher("root\CIMV2", qString)

            Try
                For Each queryObj As ManagementObject In searcher.Get()
                    wmiDataList.Add(New WMIData With {.Name = queryObj("Name"), .Var01 = queryObj("PercentFreeSpace"), .Var02 = queryObj("PercentIdleTime")})
                Next
            Catch err As ManagementException
            End Try

            Try
                For Each i In wmiDataList
                    LDActiveTime = 100 - i.Var02
                    If i.Name.ToString.Length <= 2 Then
                        Database.AgentDataList.Add(New AgentData With {.AgentName = AgentParameters.AgentName, .AgentClass = "Local Disk (" & i.Name & ")", .AgentProperty = "Free Space (%)", .AgentValue = i.Var01})
                        Database.AgentDataList.Add(New AgentData With {.AgentName = AgentParameters.AgentName, .AgentClass = "Local Disk (" & i.Name & ")", .AgentProperty = "Active Time (%)", .AgentValue = LDActiveTime})
                    End If
                Next
            Catch ex As Exception
            End Try

        End Sub

        Public Sub GetNetwork()

            Dim wmiDataList As New List(Of WMIData)
            Dim qString As String = "SELECT * FROM Win32_PerfFormattedData_Tcpip_NetworkInterface"
            Dim searcher As New ManagementObjectSearcher("root\CIMV2", qString)

            Try
                For Each queryObj As ManagementObject In searcher.Get()
                    wmiDataList.Add(New WMIData With {.Var01 = queryObj("BytesReceivedPersec"), .Var02 = queryObj("BytesSentPersec")})
                Next
            Catch err As ManagementException
            End Try

            Try
                Dim TotalReceived As Double = 0
                Dim TotalSent As Double = 0
                For Each i In wmiDataList
                    TotalReceived = TotalReceived + i.Var01
                    TotalSent = TotalSent + i.Var02
                Next

                TotalReceived = Round(TotalReceived / 1000)
                TotalSent = Round(TotalSent / 1000)

                Database.AgentDataList.Add(New AgentData With {.AgentName = AgentParameters.AgentName, .AgentClass = "Network", .AgentProperty = "Total KB/s Received", .AgentValue = TotalReceived})
                Database.AgentDataList.Add(New AgentData With {.AgentName = AgentParameters.AgentName, .AgentClass = "Network", .AgentProperty = "Total KB/s Sent", .AgentValue = TotalSent})

            Catch ex As Exception
            End Try

        End Sub



        Public Sub GetServices()

            Dim qString As String = "SELECT * FROM Win32_Service WHERE StartMode='Auto'"
            Dim Searcher As New ManagementObjectSearcher("root\CIMV2", qString)
            Dim State = Nothing
            Try
                For Each queryObj As ManagementObject In Searcher.Get()
                    If queryObj("State") = "Running" Then
                        State = 1
                    Else
                        State = 0
                    End If
                    Database.AgentStateList.Add(New AgentState With {.AgentName = AgentParameters.AgentName, .AgentClass = "Services", .AgentProperty = queryObj("DisplayName"), .AgentValue = State})
                Next
            Catch err As ManagementException
            End Try
        End Sub

    End Class

    Public Class WMIData
        Public Property Name
        Public Property Var01
        Public Property Var02
    End Class



End Namespace
