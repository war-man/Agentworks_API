namespace MoneyGram.AgentConnect.IntegrationTest.Data.Requests
{
    public class GetProfileReceiverRequest : AcOperationRequest
    {
        public string GAFVersionNumber { get; set; }
        public string ReceiverProfileID { get; set; }
        public string ReceiverProfileIDType { get; set; }
    }
}