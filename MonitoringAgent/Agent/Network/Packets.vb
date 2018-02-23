Imports MonitoringAgent.Agent.Database
Imports System.Xml
Imports System.Text
Imports System.IO

Public Class Packets

    Public Function CreatePacket()

        Dim Packet As String = Nothing

        Dim Q1 = From T In AgentSystemList
                 Select T

        Dim Q2 = From T In AgentDataList
                 Select T

        Dim Q3 = From T In AgentStateList
                 Select T

        Dim settings As XmlWriterSettings = New XmlWriterSettings()
        settings.Indent = True
        settings.NewLineOnAttributes = False
        settings.OmitXmlDeclaration = True
        Dim SBuilder As New StringBuilder()

        Using SWriter As New StringWriter(SBuilder)
            Using writer = XmlWriter.Create(SWriter, settings)

                writer.WriteStartDocument()
                writer.WriteStartElement("Agent")

                For Each i In Q1
                    writer.WriteStartElement("AgentSystem")
                    writer.WriteElementString("AgentName", i.AgentName)
                    writer.WriteElementString("AgentDomain", i.AgentDomain)
                    writer.WriteElementString("AgentIP", i.AgentIP)
                    writer.WriteElementString("AgentOSName", i.AgentOSName)
                    writer.WriteElementString("AgentOSBuild", i.AgentOSBuild)
                    writer.WriteElementString("AgentOSArchitecture", i.AgentOSArchitecture)
                    writer.WriteElementString("AgentProcessor", i.AgentProcessors)
                    writer.WriteElementString("AgentMemory", i.AgentMemory)
                    writer.WriteElementString("AgentUptime", i.AgentUptime)
                    writer.WriteElementString("AgentDate", i.AgentDate)
                    writer.WriteEndElement()
                Next

                For Each i In Q2
                    writer.WriteStartElement("AgentData")
                    writer.WriteElementString("AgentClass", i.AgentClass)
                    writer.WriteElementString("AgentProperty", i.AgentProperty)
                    writer.WriteElementString("AgentValue", i.AgentValue.ToString)
                    writer.WriteEndElement()
                Next

                For Each i In Q3
                    writer.WriteStartElement("AgentState")
                    writer.WriteElementString("AgentClass", i.AgentClass)
                    writer.WriteElementString("AgentProperty", i.AgentProperty)
                    writer.WriteElementString("AgentValue", i.AgentValue.ToString)
                    writer.WriteEndElement()
                Next

                writer.WriteEndElement()
                writer.WriteEndDocument()


            End Using

        End Using

        Packet = SBuilder.ToString

        Return Packet

    End Function

End Class