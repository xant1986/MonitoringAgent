Imports MonitoringWS.Agent.AgentParameters
Imports System.Xml
Imports System.Text
Imports System.IO
Namespace Agent

    Public Class Database
        Public Shared AgentDataList As New List(Of AgentData)
        Public Shared AgentConfigurationList As New List(Of AgentConfiguration)

        Public Shared Sub SaveDatabase()
            Dim settings As XmlWriterSettings = New XmlWriterSettings()
            settings.Indent = True
            settings.NewLineOnAttributes = False
            settings.OmitXmlDeclaration = True
            Dim SB As New StringBuilder()

            Dim wValueType = Nothing

            Using SW As New StringWriter(SB)
                Using writer = XmlWriter.Create(SW, settings)
                    writer.WriteStartDocument()
                    writer.WriteStartElement("agent-data")

                    For Each i In AgentDataList
                        writer.WriteStartElement("object")
                        writer.WriteAttributeString("name", i.AgentName)
                        writer.WriteAttributeString("date", i.AgentDate)
                        writer.WriteAttributeString("class", i.AgentClass)
                        writer.WriteAttributeString("property", i.AgentProperty)
                        writer.WriteAttributeString("instance", i.AgentInstance)
                        writer.WriteAttributeString("value", i.AgentValue.ToString)
                        writer.WriteAttributeString("sent", i.AgentDataSent.ToString)
                        writer.WriteEndElement()
                    Next
                    writer.WriteEndElement()
                    writer.WriteEndDocument()
                End Using
            End Using

            File.WriteAllText(AgentPath & "\AgentDatabase.xml", SB.ToString)

        End Sub

        Public Shared Sub UpdateTables()

            For Each i In AgentDataList
                i.AgentDataSent = True
            Next

            Dim PurgeDate = AgentDate.AddDays(-7)

            'Purge data older than 7 days
            AgentDataList.RemoveAll(Function(x) x.AgentDate < PurgeDate)

        End Sub


    End Class
    
End Namespace

