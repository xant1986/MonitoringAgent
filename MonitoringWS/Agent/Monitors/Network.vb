Imports System.Management


Namespace Agent
    Public Class Network

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

            Try
                Dim IPAddressList As New List(Of String)
                IPAddressList.AddRange(wmiDataList.Item(0))
                Database.AgentDataList.Add(New AgentData With {.AgentName = AgentParameters.AgentName, .AgentDate = AgentParameters.AgentDate, .AgentClass = "System", .AgentProperty = "IP Address", .AgentValue = IPAddressList.Item(0), .AgentInstance = 0})
            Catch ex As Exception
            End Try

        End Sub

    End Class
End Namespace
