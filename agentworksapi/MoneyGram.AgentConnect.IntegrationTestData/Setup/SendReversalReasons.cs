namespace MoneyGram.AgentConnect.IntegrationTest.Data.Setup
{
    public static class SendReversalReasons
    {
        public static string NoReceiveLocation => "NO_RCV_LOC";
        public static string WrongService => "WRONG_SERVICE";
        public static string MissingTestQuestion => "NO_TQ";
        public static string AgentTechnicalIssue => "TECH_PROB";
    }
}