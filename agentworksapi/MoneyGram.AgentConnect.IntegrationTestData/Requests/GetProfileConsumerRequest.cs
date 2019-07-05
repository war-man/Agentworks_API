namespace MoneyGram.AgentConnect.IntegrationTest.Data.Requests
{
    public class GetProfileConsumerRequest: AcOperationRequest
    {
        public string GAFVersionNumber { get; set; }
        public string ConsumerProfileID { get; set; }
        public string ConsumerProfileIDType { get; set; }
    }
}
