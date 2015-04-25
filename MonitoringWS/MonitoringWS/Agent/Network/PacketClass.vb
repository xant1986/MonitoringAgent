'Imports MonitoringWS.DataClass
Imports MonitoringWS.Agent.Database
Imports MonitoringWS.Agent.AgentParameters
Imports MonitoringWS.Encryption
Imports System.Xml
Imports System.Text
Imports System.IO

Public Class PacketClass

    Public Shared Function CreatePacket()

        Dim packet As String = Nothing

        Dim settings As XmlWriterSettings = New XmlWriterSettings()
        settings.Indent = True
        settings.NewLineOnAttributes = False
        settings.OmitXmlDeclaration = True
        Dim sb As New StringBuilder()

        Dim wValueType = Nothing

        Using sw As New StringWriter(sb)
            Using writer = XmlWriter.Create(sw, settings)
                writer.WriteStartDocument()
                writer.WriteStartElement("agent-data")
                AgentDate = Date.Now

                Dim Q = From T In AgentDataList
                      Select T Where T.AgentDataSent = False

                For Each i In Q
                    writer.WriteStartElement("object")
                    writer.WriteAttributeString("name", i.AgentName)
                    writer.WriteAttributeString("date", i.AgentDate)
                    writer.WriteAttributeString("class", i.AgentClass)
                    writer.WriteAttributeString("property", i.AgentProperty)
                    writer.WriteAttributeString("instance", i.AgentInstance)
                    writer.WriteAttributeString("value", i.AgentValue.ToString)
                    writer.WriteEndElement()
                Next
                writer.WriteEndElement()
                writer.WriteEndDocument()
            End Using
        End Using

        packet = sb.ToString
        packet = EncryptData(packet)

        Return packet

    End Function

End Class