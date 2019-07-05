namespace MoneyGram.AgentConnect.IntegrationTest.Data.Requests
{
    public class ReceiveReversalOperationRequest : AcOperationRequest
    {
        public string ReferenceNumber { get; set; }
    }
}