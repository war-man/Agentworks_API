# Prerequisite Installation
dotnet tool install --global dotnet-svcutil

# Get Latest WSDL & XSD
http://q5wsintsvcs.qacorp.moneygram.com/DataLookupService/DataLookupService_v1/META-INF/wsdl/DataLookupService_v1.wsdl
http://q5wsintsvcs.qacorp.moneygram.com/DataLookupService/DataLookupService_v1/META-INF/wsdl/DataLookupService_v1.xsd

Note...look at dependencies in WSDL & XSD, may need to include additional XSDs

# Generate Classes
dotnet-svcutil *.wsdl *.xsd -d .\ -o DataLookupService.cs -n "*,MoneyGram.DLS.Service" --wrapped --noLogo --serializer XmlSerializer