Imports MonitoringWS.Agent.Database
Imports MonitoringWS.Agent.AgentParameters
Imports System.Xml
Imports System.Text
Imports System.IO

Public Class Packets

    Public Function CreatePacket()

        Dim Packet As String = Nothing

        Dim settings As XmlWriterSettings = New XmlWriterSettings()
        settings.Indent = True
        settings.NewLineOnAttributes = False
        settings.OmitXmlDeclaration = True
        Dim SBuilder As New StringBuilder()

        Using SWriter As New StringWriter(SBuilder)
            Using writer = XmlWriter.Create(SWriter, settings)
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

        Packet = SBuilder.ToString
        Dim Encryption As New Encryption
        Packet = Encryption.EncryptData(Packet)

        Return Packet

    End Function

End Class