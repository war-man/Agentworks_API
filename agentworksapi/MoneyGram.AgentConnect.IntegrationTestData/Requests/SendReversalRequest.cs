using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Requests
{
    public class SendReversalRequest : AcOperationRequest
    {
        public string ReferenceNumber { get; set; }

        public EnumeratedIdentifierInfo RefundReason { get; set; }

        public bool? RefundFee { get; set; }
    }
}