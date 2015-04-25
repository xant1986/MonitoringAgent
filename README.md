# MonitoringAgent
This is a VB.Net Windows Service WMI Monitoring Agent

This is a basic Windows monitoring agent that collects system, processor, memory, disk, and service data, logs it to a database and has the capability to send the data to a central server.  It is built as a Windows service and runs every minute by default.  Since this is a Windows service it is somewhat difficult to test so I have also created a second repository for a Console Application version of the Agent.  Please use that if you are looking to learn the inner workings of this agent.


Application Installer:

This Agent uses the Visual Studio installer.  It will install the Agent by default to C:\Program Files\Cythraul\MonitoringAgent.  This can be changed during setup.  The agent is also by default set to Manual startup and runs under the local system account.


Configuration and Data files:

AgentDatabase.xml is the Agent database.  It is created when the agent is started.

AgentConfiguration.xml is the Agent configuration.  It is also created when the agent is started.

To configure services add a new line with the following values.

object class="windows" parameter="service" value="ServiceName"


Network Information:

This application sends data over TCP port 10001 by default and receives traffic on port 10000.  This values can be changed in the AgentConfiguration.xml file.  The Agent is also configured to accept new encrypted xml configuration files over port 10000.


Security/Encryption:

This aplication uses a weak version of AES packet encryption over TCP.  Please change the Key and IV if you plan on using this code for production.


About the Author:

My name is Phil White and I have been working with enterprise system monitoring solutions for over 7 years.  I enjoy learning new technology, writing code, and playing guitar in my free time.  I hope you find my applications useful.  