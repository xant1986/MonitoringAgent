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
        If Not File.Exists(AgentPath & "\MonitoringAgent.xml") Then
            Dim Settings As XmlWriterSettings = New XmlWriterSettings()
            Settings.Indent = True
            Settings.NewLineOnAttributes = False
            Settings.OmitXmlDeclaration = True
            Dim SB As New StringBuilder()

            Using SW As New StringWriter(SB)
                Using writer = XmlWriter.Create(SW, Settings)
                    writer.WriteStartDocument()
                    writer.WriteStartElement("Agent")

                    'Default Values
                    writer.WriteStartElement("Configuration")
                    writer.WriteAttributeString("Property", "server")
                    writer.WriteAttributeString("Value", "localhost")
                    writer.WriteEndElement()

                    writer.WriteStartElement("Configuration")
                    writer.WriteAttributeString("Property", "tcp_port")
                    writer.WriteAttributeString("Value", "10000")
                    writer.WriteEndElement()

                    writer.WriteStartElement("Configuration")
                    writer.WriteAttributeString("Property", "ssl_enabled")
                    writer.WriteAttributeString("Value", "False")
                    writer.WriteEndElement()

                    writer.WriteEndElement()
                    writer.WriteEndDocument()
                End Using
            End Using

            File.WriteAllText(AgentPath & "\MonitoringAgent.xml", SB.ToString)

        End If

    End Sub

    Public Sub LoadXML()

        Try
            Dim xelement As XElement = XElement.Load(AgentPath & "\MonitoringAgent.xml")
            Dim xmlobjects As IEnumerable(Of XElement) = xelement.Elements("Configuration")

            For Each i In xmlobjects
                Database.AgentConfigList.Add(New AgentConfiguration With {.AgentProperty = i.Attribute("Property").Value, .AgentValue = i.Attribute("Value").Value})
            Next

            Dim Q = From T In Database.AgentConfigList
                    Select T

            For Each i In Q
                If i.AgentProperty = "server" Then
                    AgentServer = i.AgentValue
                End If

                If i.AgentProperty = "tcp_port" Then
                    TCPSendPort = i.AgentValue
                End If
                If i.AgentProperty = "ssl_enabled" Then
                    SSLEnabled = i.AgentValue
                End If
            Next
        Catch
        End Try

    End Sub

End Class
