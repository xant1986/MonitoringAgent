Imports MonitoringAgent.Agent.AgentParameters
Imports MonitoringAgent.Agent
Imports System.IO
Imports System.Xml
Imports System.Text

Public Class AgentLoad

    Public NetworkLog As New NetworkLog

    Public Sub LoadParameters()
        'Configuration Parameters
        CreateXML()
        LoadXML()

        'Log Parameters
        NetworkLog.InitializeLog()

    End Sub

    Public Sub CreateXML()
        If Not File.Exists(AgentPath & "\AgentConfiguration.xml") Then
            Dim Settings As XmlWriterSettings = New XmlWriterSettings()
            Settings.Indent = True
            Settings.NewLineOnAttributes = False
            Settings.OmitXmlDeclaration = True
            Dim SB As New StringBuilder()

            Using SW As New StringWriter(SB)
                Using writer = XmlWriter.Create(SW, Settings)
                    writer.WriteStartDocument()
                    writer.WriteStartElement("agent-config")

                    writer.WriteStartElement("object")
                    writer.WriteAttributeString("class", "agent")
                    writer.WriteAttributeString("property", "version")
                    writer.WriteAttributeString("value", "2.2.0")
                    writer.WriteEndElement()

                    'For testing we are sending to the localhost
                    writer.WriteStartElement("object")
                    writer.WriteAttributeString("class", "agent")
                    writer.WriteAttributeString("property", "server")
                    writer.WriteAttributeString("value", "localhost")
                    writer.WriteEndElement()

                    writer.WriteStartElement("object")
                    writer.WriteAttributeString("class", "agent")
                    writer.WriteAttributeString("property", "tcp_send")
                    writer.WriteAttributeString("value", "10000")
                    writer.WriteEndElement()

                    writer.WriteStartElement("object")
                    writer.WriteAttributeString("class", "agent")
                    writer.WriteAttributeString("property", "poll_period")
                    writer.WriteAttributeString("value", "1")
                    writer.WriteEndElement()

                    writer.WriteEndElement()
                    writer.WriteEndDocument()
                End Using
            End Using

            File.WriteAllText(AgentPath & "\AgentConfiguration.xml", SB.ToString)

        End If

    End Sub

    Public Sub LoadXML()

        Try
            Dim xelement As XElement = XElement.Load(AgentPath & "\AgentConfiguration.xml")
            Dim xmlobjects As IEnumerable(Of XElement) = xelement.Elements("object")

            For Each i In xmlobjects
                Database.AgentConfigList.Add(New AgentConfiguration With {.AgentClass = i.Attribute("class").Value, .AgentProperty = i.Attribute("property").Value, .AgentValue = i.Attribute("value").Value})
            Next

            Dim Q = From T In Database.AgentConfigList
                    Where T.AgentClass.Contains("agent")
                    Select T

            For Each i In Q
                Select Case i.AgentProperty
                    Case "version"
                        AgentVersion = i.AgentValue
                    Case "server"
                        AgentServer = i.AgentValue
                    Case "tcp_send"
                        TCPSendPort = i.AgentValue
                    Case "poll_period"
                        AgentPollPeriod = i.AgentValue
                End Select
            Next

        Catch
        End Try

    End Sub

End Class
