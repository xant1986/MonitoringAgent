Imports MonitoringWS.Agent.AgentParameters
Imports MonitoringWS.Agent.Database
Imports MonitoringWS.Agent
Imports System.IO
Imports System.Xml
Imports System.Text

Public Class AgentLoad

    Public NetworkLog As New NetworkLog

    Public Sub LoadParameters()
        'Configuration Parameters
        CreateXML()
        LoadXML()
        LoadDatabase()

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
                    writer.WriteAttributeString("parameter", "version")
                    writer.WriteAttributeString("value", "2.0.4")
                    writer.WriteEndElement()

                    'For testing we are sending to the localhost
                    writer.WriteStartElement("object")
                    writer.WriteAttributeString("class", "agent")
                    writer.WriteAttributeString("parameter", "server")
                    writer.WriteAttributeString("value", "localhost")
                    writer.WriteEndElement()

                    writer.WriteStartElement("object")
                    writer.WriteAttributeString("class", "agent")
                    writer.WriteAttributeString("parameter", "tcp_listen")
                    writer.WriteAttributeString("value", "10000")
                    writer.WriteEndElement()

                    'We are sending data to the agent for display.  Normally we would configure this
                    'port to go to a server Application

                    writer.WriteStartElement("object")
                    writer.WriteAttributeString("class", "agent")
                    writer.WriteAttributeString("parameter", "tcp_send")
                    writer.WriteAttributeString("value", "10001")
                    writer.WriteEndElement()

                    writer.WriteStartElement("object")
                    writer.WriteAttributeString("class", "agent")
                    writer.WriteAttributeString("parameter", "poll_period")
                    writer.WriteAttributeString("value", "1")
                    writer.WriteEndElement()

                    'Adding a sample Windows Service
                    writer.WriteStartElement("object")
                    writer.WriteAttributeString("class", "windows")
                    writer.WriteAttributeString("parameter", "service")
                    writer.WriteAttributeString("value", "LanmanServer")
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

            Dim xelement As XElement = xelement.Load(AgentPath & "\AgentConfiguration.xml")
            Dim xmlobjects As IEnumerable(Of XElement) = xelement.Elements("object")

            For Each i In xmlobjects
                AgentConfigurationList.Add(New AgentConfiguration With {.AgentClass = i.Attribute("class").Value, .AgentParameter = i.Attribute("parameter").Value, .AgentValue = i.Attribute("value").Value})
            Next

            Dim Q = From T In AgentConfigurationList
                    Where T.AgentClass.Contains("agent")
                    Select T

            For Each i In Q
                Select Case i.AgentParameter
                    Case "version"
                        AgentVersion = i.AgentValue
                    Case "server"
                        AgentServer = i.AgentValue
                    Case "tcp_listen"
                        TCPListenPort = i.AgentValue
                    Case "tcp_send"
                        TCPSendPort = i.AgentValue
                    Case "poll_period"
                        AgentPollPeriod = i.AgentValue
                    Case "encryption_key"
                        Key = System.Text.Encoding.ASCII.GetBytes(i.AgentValue)
                End Select
            Next

        Catch ex As Exception
        End Try

    End Sub

    Public Sub LoadDatabase()

        Try
            Dim xelement As XElement = xelement.Load(AgentPath & "\AgentDatabase.xml")
            Dim xmlobjects As IEnumerable(Of XElement) = xelement.Elements("object")

            For Each i In xmlobjects
                Database.AgentDataList.Add(New AgentData With {.AgentName = i.Attribute("name").Value, .AgentClass = i.Attribute("class").Value, .AgentProperty = i.Attribute("property").Value, .AgentValue = i.Attribute("value").Value, .AgentDate = i.Attribute("date").Value, .AgentInstance = i.Attribute("instance").Value, .AgentDataSent = i.Attribute("sent").Value})
            Next

        Catch ex As Exception

        End Try


    End Sub

End Class
