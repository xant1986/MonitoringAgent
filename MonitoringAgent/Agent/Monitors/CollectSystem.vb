Imports System.Management
Imports System.Math

Namespace Agent
    Public Class CollectSystem

        Public Property AgentName As String = AgentParameters.AgentName
        Public Property AgentDomain As String
        Public Property AgentIP As String
        Public Property AgentOSName As String
        Public Property AgentOSBuild As String
        Public Property AgentOSArchitecture As String
        Public Property AgentProcessors As String
        Public Property AgentMemory As String

        Public Sub GetSystem()

            GetComputerSystem()
            GetOperatingSystem()
            GetNetwork()
            Database.AgentSystemList.Add(New AgentSystem With {.AgentName = AgentName, .AgentDomain = AgentDomain, .AgentIP = AgentIP, .AgentOSName = AgentOSName, .AgentOSBuild = AgentOSBuild, .AgentOSArchitecture = AgentOSArchitecture, .AgentProcessors = AgentProcessors, .AgentMemory = AgentMemory, .AgentDate = Date.Now})

        End Sub

        Public Sub GetComputerSystem()

            Dim wmiDataList As New List(Of String)
            Dim qString As String = "SELECT * FROM Win32_ComputerSystem"
            Dim searcher As New ManagementObjectSearcher("root\CIMV2", qString)

            Try
                For Each queryObj As ManagementObject In searcher.Get()
                    AgentDomain = queryObj("Domain")
                    AgentProcessors = queryObj("NumberOfProcessors")
                    AgentMemory = Round(queryObj("TotalPhysicalMemory") / 1024 / 1024)
                Next
            Catch err As ManagementException
            End Try

        End Sub

        Public Sub GetOperatingSystem()

            Dim wmiDataList As New List(Of String)
            Dim qString As String = "SELECT * FROM Win32_OperatingSystem"
            Dim searcher As New ManagementObjectSearcher("root\CIMV2", qString)

            Try
                For Each queryObj As ManagementObject In searcher.Get()
                    AgentOSName = queryObj("Caption")
                    AgentOSBuild = queryObj("BuildNumber")
                    AgentOSArchitecture = queryObj("OSArchitecture")
                Next
            Catch err As ManagementException
            End Try

        End Sub

        Public Sub GetNetwork()

            Dim wmiDataList As New List(Of Object)
            Dim qString As String = "SELECT * FROM Win32_NetworkAdapterConfiguration"
            Dim Searcher As New ManagementObjectSearcher("root\CIMV2", qString)

            Try
                For Each queryObj As ManagementObject In Searcher.Get()
                    wmiDataList.Add(queryObj("IPAddress"))
                Next
            Catch err As ManagementException
            End Try

            Dim IPAddressList As New List(Of String)
            IPAddressList.AddRange(wmiDataList.Item(0))

            AgentIP = IPAddressList.Item(0)

        End Sub

    End Class
End Namespace

