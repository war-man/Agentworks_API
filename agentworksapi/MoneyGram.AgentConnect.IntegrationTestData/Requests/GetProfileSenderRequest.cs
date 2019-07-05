namespace MoneyGram.AgentConnect.IntegrationTest.Data.Requests
{
    public class GetProfileSenderRequest : AcOperationRequest
    {
        public string GAFVersionNumber { get; set; }
        public string SenderProfileID { get; set; }
        public string SenderProfileIDType { get; set; }
    }
}