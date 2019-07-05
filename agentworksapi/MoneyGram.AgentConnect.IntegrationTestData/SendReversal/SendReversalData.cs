using System.Collections.Generic;
using System.Linq;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Common;
using MoneyGram.AgentConnect.IntegrationTest.Data.Lookup;
using MoneyGram.AgentConnect.IntegrationTest.Data.Requests;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.SendReversal
{
    public class SendReversalData : TransactionData
    {
        public SendReversalData()
        {
            Init();
        }

        public SendReversalData(SendReversalRequest sendReversalRequest)
        {
            Set(sendReversalRequest);
            Init();
        }

        private void Init()
        {
            Errors = new List<BusinessError>();
            ValidationRequests = new List<SendReversalValidationRequest>();
            ValidationResponses = new List<SendReversalValidationResponse>();
        }

        public CompleteSessionRequest CompleteSessionReq { get; private set; }

        public CompleteSessionResponse CompleteSessionResp { get; private set; }

        public List<BusinessError> Errors { get; private set; }

        public SendReversalRequest SendReversalRequest { get; private set; }

        public TransactionLookupData TransactionLookup { get; private set; }

        public List<SendReversalValidationRequest> ValidationRequests { get; private set; }

        public List<SendReversalValidationResponse> ValidationResponses { get; private set; }

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

        public void Set(SendReversalRequest sendReversalRequest)
        {
            SendReversalRequest = sendReversalRequest;
            SetExecutionOrder(nameof(SendReversalRequest));
        }

        public void Set(TransactionLookupData transactionLookupData)
        {
            TransactionLookup = transactionLookupData;
            SetExecutionOrder(nameof(TransactionLookup));
        }

        public void Set(SendReversalValidationRequest validationRequest)
        {
            var isFirstSet = !ValidationRequests.Any();

            ValidationRequests.Add(validationRequest);
            if (isFirstSet) SetExecutionOrder(nameof(ValidationRequests));
        }

        public void Set(List<SendReversalValidationRequest> validationRequests)
        {
            var isFirstSet = validationRequests.Any() && !ValidationRequests.Any();

            ValidationRequests.AddRange(validationRequests);
            if (isFirstSet) SetExecutionOrder(nameof(ValidationRequests));
        }

        public void Set(SendReversalValidationResponse validationResponse)
        {
            var isFirstSet = !ValidationResponses.Any();

            ValidationResponses.Add(validationResponse);
            if (isFirstSet) SetExecutionOrder(nameof(ValidationResponses));
        }

        public void Set(List<SendReversalValidationResponse> validationResponses)
        {
            var isFirstSet = validationResponses.Any() && !ValidationResponses.Any();

            ValidationResponses.AddRange(validationResponses);
            if (isFirstSet) SetExecutionOrder(nameof(ValidationResponses));
        }
    }
}