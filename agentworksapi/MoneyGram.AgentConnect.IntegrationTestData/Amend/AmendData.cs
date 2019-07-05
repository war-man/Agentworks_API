using System.Collections.Generic;
using System.Linq;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Common;
using MoneyGram.AgentConnect.IntegrationTest.Data.Lookup;
using MoneyGram.AgentConnect.IntegrationTest.Data.Requests;
using MoneyGram.AgentConnect.IntegrationTest.Data.Send;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Amend
{
    public class AmendData : TransactionData
    {
        public AmendData(AmendOperationRequest request)
        {
            AmendOperationRequest = request;
            Errors = new List<BusinessError>();
            GafInfoKeysWithGroups = new Dictionary<string, string>();
            ValidationRequests = new List<AmendValidationRequest>();
            ValidationResponses = new List<AmendValidationResponse>();
        }

        public AmendOperationRequest AmendOperationRequest { get; private set; }
        public CompleteSessionRequest CompleteSessionReq { get; private set; }

        public CompleteSessionResponse CompleteSessionResp { get; private set; }

        public List<BusinessError> Errors { get; private set; }

        public Dictionary<string, string> GafInfoKeysWithGroups { get; private set; }

        public TransactionLookupData TransactionLookup { get; private set; }
        public SendData SendData { get; private set; }

        public List<AmendValidationRequest> ValidationRequests { get; private set; }

        public List<AmendValidationResponse> ValidationResponses { get; private set; }

        public void Set(CompleteSessionRequest completeSessionRequest)
        {
            CompleteSessionReq = completeSessionRequest;
            SetExecutionOrder(nameof(CompleteSessionReq));
        }

        public void Set(CompleteSessionResponse completeSessionResponse)
        {
            CompleteSessionResp = completeSessionResponse;
            SetExecutionOrder(nameof(CompleteSessionResp));
        }

        public void Set(BusinessError error)
        {
            var isFirstSet = !Errors.Any();

            Errors.Add(error);
            if (isFirstSet) SetExecutionOrder(nameof(Errors));
        }

        public void Set(List<BusinessError> errors)
        {
            var isFirstSet = errors.Any() && !Errors.Any();

            Errors.AddRange(errors);
            if (isFirstSet) SetExecutionOrder(nameof(Errors));
        }

        public void Set(Dictionary<string, string> gafInfoKeysWithGroups)
        {
            var isFirstSet = gafInfoKeysWithGroups.Any() && !GafInfoKeysWithGroups.Any();

            GafInfoKeysWithGroups = gafInfoKeysWithGroups;
            if (isFirstSet) SetExecutionOrder(nameof(GafInfoKeysWithGroups));
        }

        public void Set(TransactionLookupData transactionLookupData)
        {
            TransactionLookup = transactionLookupData;
            SetExecutionOrder(nameof(TransactionLookup));
        }

        public void Set(AmendValidationRequest validationRequest)
        {
            var isFirstSet = !ValidationRequests.Any();

            ValidationRequests.Add(validationRequest);
            if (isFirstSet) SetExecutionOrder(nameof(ValidationRequests));
        }

        public void Set(List<AmendValidationRequest> validationRequests)
        {
            var isFirstSet = validationRequests.Any() && !ValidationRequests.Any();

            ValidationRequests.AddRange(validationRequests);
            if (isFirstSet) SetExecutionOrder(nameof(ValidationRequests));
        }

        public void Set(AmendValidationResponse validationResponse)
        {
            var isFirstSet = !ValidationResponses.Any();

            ValidationResponses.Add(validationResponse);
            if (isFirstSet) SetExecutionOrder(nameof(ValidationResponses));
        }

        public void Set(List<AmendValidationResponse> validationResponses)
        {
            var isFirstSet = validationResponses.Any() && !ValidationResponses.Any();

            ValidationResponses.AddRange(validationResponses);
            if (isFirstSet) SetExecutionOrder(nameof(ValidationResponses));
        }
        public void Set(SendData sendData)
        {
            SendData = sendData;
            SetExecutionOrder(nameof(ValidationResponses));
        }
    }
}