using System.Collections.Generic;
using System.Linq;
using MoneyGram.AgentConnect.DomainModel.Exceptions;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Common;
using MoneyGram.AgentConnect.IntegrationTest.Data.ReceiveReversal;
using MoneyGram.AgentConnect.IntegrationTest.Data.Requests;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Common;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Lookup;

namespace MoneyGram.AgentConnect.IntegrationTest.Operations.ReceiveReversal
{
    public class ReceiveReversalOperations
    {
        private TestAgentConnectIntegration _acIntegration { get; }
        private ValidationOperations _validationOperations;
        private TransactionLookupOperations _tranLookupOperations;

        public ReceiveReversalOperations(TestRunner testRunner)
        {
            _acIntegration = new TestAgentConnectIntegration(testRunner);
            _validationOperations = new ValidationOperations(testRunner);
            _tranLookupOperations = new TransactionLookupOperations(testRunner);
        }

        public ReceiveReversalData SingleValidate(ReceiveReversalOperationRequest request)
        {
            var receiveReversalData = new ReceiveReversalData(request);

            try
            {
                // Transaction Lookup
                TransactionLookup(receiveReversalData);
                if (receiveReversalData.Errors.Count > 0)
                {
                    return receiveReversalData;
                }

                if (receiveReversalData.TransactionLookup.TransactionLookupResp.Payload.TransactionStatus != TransactionStatusType.RECVD)
                {
                    receiveReversalData.Errors.Add(new BusinessError
                    {
                        ErrorCategory = "Custom Error",
                        ErrorCode = string.Empty,
                        Message = "Transaction not available for Receive Reversal."
                    });
                    return receiveReversalData;
                }
                // Validate Receive
                _validationOperations.ReceiveReversalValidate(receiveReversalData);
            }
            catch (AgentConnectException acExp)
            {
                receiveReversalData.Errors.Add(new BusinessError
                {
                    ErrorCategory = "AC Exception",
                    ErrorCode = acExp.ErrorCode.ToString(),
                    Message = acExp.Message
                });
            }
            return receiveReversalData;
        }

        public ReceiveReversalData ReceiveReversalComplete(ReceiveReversalOperationRequest request)
        {
            var receiveReversalData = new ReceiveReversalData(request);
            try
            {
                //Transaction Lookup
                TransactionLookup(receiveReversalData);
                if (receiveReversalData.Errors.Count > 0)
                {
                    return receiveReversalData;
                }

                if (receiveReversalData.TransactionLookup.TransactionLookupResp.Payload.TransactionStatus != TransactionStatusType.RECVD)
                {
                    receiveReversalData.Errors.Add(new BusinessError
                    {
                        ErrorCategory = "Custokm Error",
                        ErrorCode = string.Empty,
                        Message = "Transaction not available for ReceiveRevarsal."
                    });
                    return receiveReversalData;
                }

                //Validate ReceiveReversal
                _validationOperations.ReceiveReversalValidate(receiveReversalData);

                //Final Validate(s)
                _validationOperations.ReceiveReversalValidateUntilReadyForCommit(receiveReversalData);
                if (receiveReversalData.Errors.Count > 0)
                {
                    return receiveReversalData;
                }
                if (receiveReversalData.ValidationResponses.Last().Payload.ReadyForCommit)
                {
                    CompleteSession(request, receiveReversalData);
                }
                else
                {
                    receiveReversalData.Errors.Add(new BusinessError
                    {
                        ErrorCategory = "Custom Error",
                        ErrorCode = string.Empty,
                        Message = "Not ready to commit yet"
                    });
                }
            }
            catch (AgentConnectException acExp)
            {
                receiveReversalData.Errors.Add(new BusinessError
                {
                    ErrorCategory = "AC Exception",
                    ErrorCode = acExp.ErrorCode.ToString(),
                    Message = acExp.Message
                });
            }
            return receiveReversalData;
        }
        public void TransactionLookup(ReceiveReversalData receiveReversalData)
        {
            try
            {
                receiveReversalData.TransactionLookup = _tranLookupOperations.TransactionLookupForReceiveReversal(receiveReversalData);
                receiveReversalData.Errors.AddRange(receiveReversalData.TransactionLookup.TransactionLookupResp.Errors ?? new List<BusinessError>());
            }
            catch (AgentConnectException acExp)
            {
                receiveReversalData.Errors.Add(new BusinessError
                {
                    ErrorCategory = "AC Exception",
                    ErrorCode = acExp.ErrorCode.ToString(),
                    Message = acExp.Message
                });
            }
        }

        private void CompleteSession(ReceiveReversalOperationRequest request, ReceiveReversalData receiveReversalData)
        {
            var mgiSessionId = receiveReversalData.ValidationResponses.Last().Payload.MgiSessionID;
            var completeSessionRequest = CompleteSessionRequestFactory.CompleteReceiveReversalRequest(mgiSessionId);
            completeSessionRequest.AgentID = request.AgentId;
            completeSessionRequest.AgentSequence = request.AgentPos;

            var completeSessionResponse = _acIntegration.CompleteSession(completeSessionRequest);
            receiveReversalData.Errors.AddRange(completeSessionResponse.Errors ?? new List<BusinessError>());

            receiveReversalData.CompleteSessionReq = completeSessionRequest;
            receiveReversalData.CompleteSessionResp = completeSessionResponse;
        }
    }
}