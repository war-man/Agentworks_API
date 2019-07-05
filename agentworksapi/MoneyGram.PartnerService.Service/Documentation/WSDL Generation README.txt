# Prerequisite Installation
dotnet tool install --global dotnet-svcutil

# Get Latest WSDL & XSD
http://q5wsintsvcs.qacorp.moneygram.com/partnerService/partner_v1/WEB-INF/wsdl/WebPOEPartnerService_v1.wsdl
http://q5wsintsvcs.qacorp.moneygram.com/partnerService/partner_v1/WEB-INF/wsdl/WebPOEPartnerService_v1.xsd

Note...look at dependencies in WSDL & XSD, may need to include additional XSDs

# Generate Classes
dotnet-svcutil *.wsdl *.xsd -d .\ -o PartnerService.cs -n "*,MoneyGram.PartnerService.Service" --wrapped --noLogo --serializer XmlSerializer