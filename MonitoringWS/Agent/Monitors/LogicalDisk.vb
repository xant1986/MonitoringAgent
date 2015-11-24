Imports System.Threading
Imports System.Management
Imports System.Math

Namespace Agent
    Public Class LogicalDisk

        Public Sub GetLogicalDisk()

            Dim LDSize As Double = Nothing
            Dim LDFreeSpace As Double = 0
            Dim LDPercentFree As Double = 0
            Dim LDFreeSpaceMB As Double = 0

            Dim wmiDataList As New List(Of WMIData)
            Dim qString As String = "SELECT * FROM Win32_LogicalDisk WHERE Description='Local Fixed Disk'"
            Dim searcher As New ManagementObjectSearcher("root\CIMV2", qString)

            Dim instance As Integer = 0
            Try
                For Each queryObj As ManagementObject In searcher.Get()
                    wmiDataList.Add(New WMIData With {.wmiValue0 = queryObj("Name"), .wmiValue1 = queryObj("Size"), .wmiValue2 = queryObj("FreeSpace"), .wmiInstance = instance})
                    instance += 1
                Next
            Catch err As ManagementException
            End Try

            Try
                For Each i In wmiDataList
                    LDSize = i.wmiValue1
                    LDFreeSpace = i.wmiValue2
                    If LDSize <> 0 And LDFreeSpace <> 0 Then
                        LDPercentFree = Round((LDFreeSpace / LDSize) * 100)
                    End If
                    LDFreeSpaceMB = Round(LDFreeSpace / 1024 / 1024)

                    Database.AgentDataList.Add(New AgentData With {.AgentName = AgentParameters.AgentName, .AgentDate = AgentParameters.AgentDate, .AgentClass = "Logical Disk (" & i.wmiValue0 & ")", .AgentProperty = "Free Space (%)", .AgentValue = LDPercentFree, .AgentInstance = i.wmiInstance})
                    Database.AgentDataList.Add(New AgentData With {.AgentName = AgentParameters.AgentName, .AgentDate = AgentParameters.AgentDate, .AgentClass = "Logical Disk (" & i.wmiValue0 & ")", .AgentProperty = "Free Space (MB)", .AgentValue = LDFreeSpaceMB, .AgentInstance = i.wmiInstance})

                Next
            Catch ex As Exception

            End Try



        End Sub


    End Class
End Namespace