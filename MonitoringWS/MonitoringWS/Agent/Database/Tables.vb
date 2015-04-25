Namespace Agent

    Public Class AgentConfiguration
        Public Property AgentClass As String
        Public Property AgentParameter As String
        Public Property AgentValue As String
    End Class

    Public Class AgentData
        Public Property AgentDate As Date
        Public Property AgentName As String
        Public Property AgentClass As String
        Public Property AgentProperty As String
        Public Property AgentInstance As Int32?
        Public Property AgentValue As String
        Public Property AgentDataSent As Boolean = False
    End Class

    Public Class WMIData
        Public Property wmiValue0 As String
        Public Property wmiValue1 As String
        Public Property wmiValue2 As String
        Public Property wmiValue3 As String
        Public Property wmiValue4 As String
        Public Property wmiValue5 As String
        Public Property wmiValue6 As String
        Public Property wmiValue7 As String
        Public Property wmiValue8 As String
        Public Property wmiValue9 As String
        Public Property wmiInstance As String
    End Class

End Namespace
