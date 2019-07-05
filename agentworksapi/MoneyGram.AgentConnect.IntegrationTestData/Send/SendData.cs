using System.Collections.Generic;
using System.Linq;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Common;
using MoneyGram.AgentConnect.IntegrationTest.Data.Requests;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Send
{
    public sealed class SendData : TransactionData
    {
        public SendData()
        {
            Init();
        }

        public SendData(SendRequest sendRequest)
        {
            Set(sendRequest);
            Init();
        }

        private void Init()
        {
            SendValidationReqs = new List<SendValidationRequest>();
            SendValidationResps = new List<SendValidationResponse>();
        }

        public CompleteSessionRequest CompleteSessionReq { get; private set; }

        public CompleteSessionResponse CompleteSessionResp { get; private set; }

        public ConsumerHistoryLookupData ConsumerHistoryLookup { get; private set; }

        public FeeLookupData FeeLookup { get; private set; }

        public FeeInfo FeeInfo { get; private set; }

        public SenderLookupInfo SenderInfo { get; private set; }

        public SendRequest SendRequest { get; private set; }

        public List<SendValidationRequest> SendValidationReqs { get; private set; }

        public List<SendValidationResponse> SendValidationResps { get; private set; }

        public void Set(SendRequest sendRequest)
        {
            SendRequest = sendRequest;
            SetExecutionOrder(nameof(SendRequest));
        }

        public void Set(SendValidationRequest sendValidationRequest)
        {
            var isFirstSet = !SendValidationReqs.Any();

            SendValidationReqs.Add(sendValidationRequest);
            if (isFirstSet) SetExecutionOrder(nameof(SendValidationReqs));
        }

        public void Set(List<SendValidationRequest> sendValidationRequests)
        {
            var isFirstSet = sendValidationRequests.Any() && !SendValidationReqs.Any();

            SendValidationReqs.AddRange(sendValidationRequests);
            if (isFirstSet) SetExecutionOrder(nameof(SendValidationReqs));
        }

        public void Set(SendValidationResponse sendValidationResponse)
        {
            var isFirstSet = !SendValidationResps.Any();

            SendValidationResps.Add(sendValidationResponse);
            if (isFirstSet) SetExecutionOrder(nameof(SendValidationResps));
        }

        public void Set(List<SendValidationResponse> sendValidationResponses)
        {
            var isFirstSet = sendValidationResponses.Any() && !SendValidationResps.Any();

            SendValidationResps.AddRange(sendValidationResponses);
            if (isFirstSet) SetExecutionOrder(nameof(SendValidationResps));
        }

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

        public void Set(ConsumerHistoryLookupData consumerHistoryLookupData)
        {
            ConsumerHistoryLookup = consumerHistoryLookupData;
            SetExecutionOrder(nameof(ConsumerHistoryLookup));
        }


        public void Set(FeeLookupData feeLookupData)
        {
            FeeLookup = feeLookupData;
            SetExecutionOrder(nameof(FeeLookup));
        }

        public void Set(FeeInfo feeInfo)
        {
            FeeInfo = feeInfo;
            SetExecutionOrder(nameof(FeeInfo));
        }

        public void Set(SenderLookupInfo senderLookupInfo)
        {
            SenderInfo = senderLookupInfo;
            SetExecutionOrder(nameof(SenderInfo));
        }
    }
}