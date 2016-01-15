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

                    'For testing we are sending to the localhost
                    writer.WriteStartElement("object")
                    writer.WriteAttributeString("class", "agent")
                    writer.WriteAttributeString("property", "server")
                    writer.WriteAttributeString("value", "localhost")
                    writer.WriteEndElement()

                    writer.WriteStartElement("object")
                    writer.WriteAttributeString("class", "agent")
                    writer.WriteAttributeString("property", "tcp_port")
                    writer.WriteAttributeString("value", "10000")
                    writer.WriteEndElement()

                    writer.WriteStartElement("object")
                    writer.WriteAttributeString("class", "agent")
                    writer.WriteAttributeString("property", "ssl_enabled")
                    writer.WriteAttributeString("value", "False")
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
