namespace MoneyGram.AgentConnect.IntegrationTest.Data.Requests
{
    public class AcOperationRequest
    {
        public string Environment { get; set; }
        public string AgentId { get; set; }
        public string AgentPos { get; set; }
        public string AgentCountryIsoCode { get; set; }
        public string AgentState { get; set; }
    }
}