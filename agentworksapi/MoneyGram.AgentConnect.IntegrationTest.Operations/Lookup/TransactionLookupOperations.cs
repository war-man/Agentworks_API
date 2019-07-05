using MoneyGram.AgentConnect.IntegrationTest.Data.Amend;
using MoneyGram.AgentConnect.IntegrationTest.Data.Lookup;
using MoneyGram.AgentConnect.IntegrationTest.Data.ReceiveReversal;
using MoneyGram.AgentConnect.IntegrationTest.Data.Recv;
using MoneyGram.AgentConnect.IntegrationTest.Data.Send;
using MoneyGram.AgentConnect.IntegrationTest.Data.SendReversal;

namespace MoneyGram.AgentConnect.IntegrationTest.Operations.Lookup
{
    public class TransactionLookupOperations
    {
        private TestAgentConnectIntegration _acIntegration { get; }

        public TransactionLookupOperations(TestRunner testRunner)
        {
            _acIntegration = new TestAgentConnectIntegration(testRunner);

        }

        public TransactionLookupData TransactionLookupForAmend(AmendData amendData)
        {
            var transactionLookupData = new TransactionLookupData();
            transactionLookupData.Set(TransactionLookupRequestFactory.TransanctionLookupRequestForAmend(amendData.AmendOperationRequest.ReferenceNumber));
            var req = TransactionLookupRequestFactory.TransanctionLookupRequestForAmend(amendData.AmendOperationRequest.ReferenceNumber);
            req.AgentID = amendData.AmendOperationRequest.AgentId;
            req.AgentSequence = amendData.AmendOperationRequest.AgentPos;
            transactionLookupData.Set(req);
            transactionLookupData.Set(_acIntegration.TransactionLookup(transactionLookupData.TransactionLookupReq));
            return transactionLookupData;
        }

        public TransactionLookupData TransactionLookupForSendReversal(SendReversalData sendReversalData)
        {
            var transactionLookupData = new TransactionLookupData();
            var req = TransactionLookupRequestFactory.TransanctionLookupRequestForSendReversal(sendReversalData.SendReversalRequest.ReferenceNumber);
            req.AgentID = sendReversalData.SendReversalRequest.AgentId;
            req.AgentSequence = sendReversalData.SendReversalRequest.AgentPos;
            transactionLookupData.Set(req);
            transactionLookupData.Set(_acIntegration.TransactionLookup(transactionLookupData.TransactionLookupReq));
            return transactionLookupData;
        }

        public TransactionLookupData TransactionLookupForReceiveReversal(ReceiveReversalData receiveReversalData)
        {
            var transactionLookupData = new TransactionLookupData();
            var req = TransactionLookupRequestFactory.TransactionLookupRequestForReceiveReversal(receiveReversalData.ReceiveReversalOperationRequest.ReferenceNumber);
            req.AgentID = receiveReversalData.ReceiveReversalOperationRequest.AgentId;
            req.AgentSequence = receiveReversalData.ReceiveReversalOperationRequest.AgentPos;
            transactionLookupData.Set(req);
            transactionLookupData.Set(_acIntegration.TransactionLookup(transactionLookupData.TransactionLookupReq));
            return transactionLookupData;
        }

        public TransactionLookupData TransanctionLookupRequestForReceive(ReceiveData receiveData)
        {
            var transactionLookupData = new TransactionLookupData();
            var req = TransactionLookupRequestFactory.TransanctionLookupRequestForReceive(receiveData.CompleteSessionResponse.Payload.ReferenceNumber);
            req.AgentID = receiveData.ReceiveRequest.AgentId;
            req.AgentSequence = receiveData.ReceiveRequest.AgentPos;
            transactionLookupData.Set(req);
            transactionLookupData.Set(transactionLookupData.TransactionLookupReq);
            return transactionLookupData;
        }
        public TransactionLookupData TransanctionLookupRequestForStagedReceive(SendData sendData)
        {
            var transactionLookupData = new TransactionLookupData();
            var req = TransactionLookupRequestFactory.TransanctionLookupRequestForReceiveCompletion(sendData.CompleteSessionResp.Payload.ReferenceNumber);
            req.AgentID = sendData.SendRequest.AgentId;
            req.AgentSequence = sendData.SendRequest.AgentPos;
            transactionLookupData.Set(req);
            transactionLookupData.Set(_acIntegration.TransactionLookup(transactionLookupData.TransactionLookupReq));
            return transactionLookupData;
        }
        public TransactionLookupData TransactionLookupForStatus(string agentId, string agentPos, string refNum)
        {
            var transactionLookupData = new TransactionLookupData();
            var req = TransactionLookupRequestFactory.TransanctionLookupRequestForStatus(refNum);
            req.AgentID = agentId;
            req.AgentSequence = agentPos;
            transactionLookupData.Set(req);
            transactionLookupData.Set(_acIntegration.TransactionLookup(transactionLookupData.TransactionLookupReq));
            return transactionLookupData;
        }
    }
}