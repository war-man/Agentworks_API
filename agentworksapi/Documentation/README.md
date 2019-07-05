# AgentWorks API Setup

Once the BitBucket team admin adds you to the AgentWorks team, clone the repo:
1. Pull latest from https://bitbucket.org/AgentWorks/agentworksapi.git
2. Open solution in Visual Studio
3. Run Debug build

### Build Solution
To build AgentWorks, use either the Debug or Release build configurations.

### Register .NET with IIS
Open cmd as administrator, execute:

```
C:\Windows\Microsoft.NET\Framework\v4.0.30319\aspnet_regiis.exe -i
```

### Create Application Pool in IIS
1.  Right-click on Application Pools and click 'Add Application Pool'
    * BASIC SETTINGS
        * Name: .NET 4.0
        * .NET Framework version: .NET Framework v4.0.30319
        * Managed pipeline mode: Integrated
        * Start application pool immediately: checked
        * Click OK
    * ADVANCED SETTINGS
        * Under Process Model, set Identity to  NetworkService

### Create Site in IIS
1. Right-click on Sites. Select 'Add Web Site' (do not add this site under default web site)
    * BASIC SETTINGS
        * Name: AgentWorksApi DEV
        * Application Pool: .NET 4.0
        * Physical path: path to AwApi directory example: `[%path%]\AgentWorksApi\AwApi`
    * BINDINGS
        * Type: http
        * IP address: All Unassigned
        * Port: 8080
        * Host name: <leave blank for now>

### Open Regedit
1. Change `HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Windows NT\CurrentVersion`
2. Double-click on RegisteredOrganization and change the value to MoneyGram International
3. Double-click on  RegisteredOwner and change the value to MoneyGram International

### Aw.Nxt Web API documentation
`API_URL:API_PORT/swagger/ui/index#/`
e.g. [http://localdev.moneygram.com:8080/swagger/ui/index#/](http://localdev.moneygram.com:8080/swagger/ui/index#/)
Swagger uses API Keys for authentication. Valid keys can be found in `[%path%]\MoneyGram.Common.Auth\ApiKeyAuth\ApiKeyAuthenticationRepository.cs`
