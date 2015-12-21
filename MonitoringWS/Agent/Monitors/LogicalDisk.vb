Imports System.Management
Imports System.Math

Namespace Agent
    Public Class LogicalDisk
        Public Property Name As String
        Public Property Size As Double
        Public Property FreeSpace As Double

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

                    Database.AgentDataList.Add(New AgentData With {.AgentName = AgentParameters.AgentName, .AgentDate = AgentParameters.AgentDate, .AgentClass = "Local Disk (" & i.Name & ")", .AgentProperty = "Free Space (%)", .AgentValue = LDPercentFree})
                    Database.AgentDataList.Add(New AgentData With {.AgentName = AgentParameters.AgentName, .AgentDate = AgentParameters.AgentDate, .AgentClass = "Local Disk (" & i.Name & ")", .AgentProperty = "Free Space (MB)", .AgentValue = LDFreeSpaceMB})

                Next
            Catch ex As Exception

            End Try



        End Sub


    End Class
End Namespace