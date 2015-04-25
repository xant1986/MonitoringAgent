Imports System.Threading
Imports System.Management

Namespace Agent
    Public Class Services

        Public Shared Sub GetServices()

            Dim Q = From T In Database.AgentConfigurationList
                    Where T.AgentClass = "windows" And T.AgentParameter = "service"
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
                        Database.AgentDataList.Add(New AgentData With {.AgentName = AgentParameters.AgentName, .AgentDate = AgentParameters.AgentDate, .AgentClass = "Services", .AgentProperty = queryObj("DisplayName"), .AgentValue = State, .AgentInstance = 0})
                    Next
                Catch err As ManagementException
                End Try
            Next


        End Sub


    End Class
End Namespace

