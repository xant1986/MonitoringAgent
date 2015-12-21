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
                    writer.WriteAttributeString("property", "version")
                    writer.WriteAttributeString("value", "2.0.7")
                    writer.WriteEndElement()

                    'For testing we are sending to the localhost
                    writer.WriteStartElement("object")
                    writer.WriteAttributeString("class", "agent")
                    writer.WriteAttributeString("property", "server")
                    writer.WriteAttributeString("value", "localhost")
                    writer.WriteEndElement()

                    writer.WriteStartElement("object")
                    writer.WriteAttributeString("class", "agent")
                    writer.WriteAttributeString("property", "tcp_listen")
                    writer.WriteAttributeString("value", "10000")
                    writer.WriteEndElement()

                    writer.WriteStartElement("object")
                    writer.WriteAttributeString("class", "agent")
                    writer.WriteAttributeString("property", "tcp_send")
                    writer.WriteAttributeString("value", "10001")
                    writer.WriteEndElement()

                    writer.WriteStartElement("object")
                    writer.WriteAttributeString("class", "agent")
                    writer.WriteAttributeString("property", "poll_period")
                    writer.WriteAttributeString("value", "1")
                    writer.WriteEndElement()

                    'Adding default Windows Services

                    Dim ServiceList As New List(Of String)
                    ServiceList.Add("AppHostSvc")
                    ServiceList.Add("BrokerInfrastructure")
                    ServiceList.Add("BFE")
                    ServiceList.Add("EventSystem")
                    ServiceList.Add("CryptSvc")
                    ServiceList.Add("DcomLaunch")
                    ServiceList.Add("Dhcp")
                    ServiceList.Add("DPS")
                    ServiceList.Add("DiagTrack")
                    ServiceList.Add("TrkWks")
                    ServiceList.Add("iphlpsvc")
                    ServiceList.Add("LSM")
                    ServiceList.Add("NlaSvc")
                    ServiceList.Add("nsi")
                    ServiceList.Add("Power")
                    ServiceList.Add("Spooler")
                    ServiceList.Add("PcaSvc")
                    ServiceList.Add("RpcSs")
                    ServiceList.Add("RpcEptMapper")
                    ServiceList.Add("SamSs")
                    ServiceList.Add("LanmanServer")
                    ServiceList.Add("ShellHWDetection")
                    ServiceList.Add("SysMain")
                    ServiceList.Add("SENS")
                    ServiceList.Add("Schedule")
                    ServiceList.Add("Themes")
                    ServiceList.Add("ProfSvc")
                    ServiceList.Add("Audiosrv")
                    ServiceList.Add("WinDefend")
                    ServiceList.Add("EventLog")
                    ServiceList.Add("MpsSvc")
                    ServiceList.Add("FontCache")
                    ServiceList.Add("Winmgmt")
                    ServiceList.Add("LanmanWorkstation")

                    For Each i In ServiceList
                        writer.WriteStartElement("object")
                        writer.WriteAttributeString("class", "windows")
                        writer.WriteAttributeString("property", "service")
                        writer.WriteAttributeString("value", i)
                        writer.WriteEndElement()
                    Next

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
                AgentConfigurationList.Add(New AgentConfiguration With {.AgentClass = i.Attribute("class").Value, .AgentProperty = i.Attribute("property").Value, .AgentValue = i.Attribute("value").Value})
            Next

            Dim Q = From T In AgentConfigurationList
                    Where T.AgentClass.Contains("agent")
                    Select T

            For Each i In Q
                Select Case i.AgentProperty
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
                Database.AgentDataList.Add(New AgentData With {.AgentName = i.Attribute("name").Value, .AgentClass = i.Attribute("class").Value, .AgentProperty = i.Attribute("property").Value, .AgentValue = i.Attribute("value").Value, .AgentDate = i.Attribute("date").Value, .AgentDataSent = i.Attribute("sent").Value})
            Next

        Catch ex As Exception

        End Try


    End Sub

End Class
