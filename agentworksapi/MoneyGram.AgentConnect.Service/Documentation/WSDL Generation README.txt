# Prerequisite Installation
dotnet tool install --global dotnet-svcutil


dotnet-svcutil .\AgentConnect.wsdl -d .\ -o AgentConnect.cs -n "*,MoneyGram.AgentConnect.Service" --wrapped --noLogo --serializer XmlSerializer