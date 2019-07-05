using System.Collections.Generic;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Lookup;
using MoneyGram.AgentConnect.IntegrationTest.Data.Requests;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.ReceiveReversal
{
    public class ReceiveReversalData
    {
        public ReceiveReversalData(ReceiveReversalOperationRequest request)
        {
            ReceiveReversalOperationRequest = request;
            ValidationRequests = new List<ReceiveReversalValidationRequest>();
            ValidationResponses = new List<ReceiveReversalValidationResponse>();
            Errors = new List<BusinessError>();
        }
        public ReceiveReversalOperationRequest ReceiveReversalOperationRequest { get; set; }
        public TransactionLookupData TransactionLookup { get; set; }
        public List<ReceiveReversalValidationRequest> ValidationRequests { get; set; }
        public List<ReceiveReversalValidationResponse> ValidationResponses { get; set; }

        public CompleteSessionRequest CompleteSessionReq { get; set; }
        public CompleteSessionResponse CompleteSessionResp { get; set; }

        public List<BusinessError> Errors { get; set; }
    }
}