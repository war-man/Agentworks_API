namespace AwLogAnalyzer
{
    public static class RegexStringConstants
    {
        public const string Brackets = @"\[(.*?)\]";
        public const string Transaction = @"Transaction_ID=.*?(.*)";
        public const string ServerIP = @"Server_Ip=.*?(.*)";
        public const string UserName = @"UserName=.*?(.*)";
        public const string Api = @"(api.*$)";
        public const string ElapsedTime = @"([\d]+)ms";
        public const string Soap = @"urn:AgentConnect([\w#]*)";
        public const string Cache = @"(Cache \w*)";
        public const string Action = @"(\[[a-zA-Z0-9-_:=\(\)\. ]+\] )+([\w\/ ]+)";
    }
}