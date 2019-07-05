using System.Collections.Generic;
using System.Linq;
using MoneyGram.AgentConnect.DomainModel.Exceptions;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Common;
using MoneyGram.AgentConnect.IntegrationTest.Data.SendReversal;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Common;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Lookup;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Helpers;

namespace MoneyGram.AgentConnect.IntegrationTest.Operations.SendReversal
{
    public class SendReversalOperations
    {
        private TestAgentConnectIntegration _acIntegration { get; }
        private ValidationOperations _validationOperations;
        private TransactionLookupOperations _tranLookupOperations;

        public SendReversalOperations(TestRunner testRunner)
        {
            _acIntegration = new TestAgentConnectIntegration(testRunner);
            _validationOperations = new ValidationOperations(testRunner);
            _tranLookupOperations = new TransactionLookupOperations(testRunner);
        }

        public SendReversalData SingleValidate(SendReversalData sendReversalData)
        {
            try
            {
                // Transaction Lookup
                TransactionLookup(sendReversalData);
                if (DataErrorHandler.CheckForNestedErrors(sendReversalData).Any())
                {
                    return sendReversalData;
                }

                if (sendReversalData.TransactionLookup.TransactionLookupResp.Payload.TransactionStatus != TransactionStatusType.AVAIL)
                {
                    var error = new BusinessError
                    {
                        ErrorCategory = "Custom Error",
                        ErrorCode = string.Empty,
                        Message = "Transaction not available for Send Reversal."
                    };

                    sendReversalData.Set(error);
                }
                // Validate Receive
                _validationOperations.SendReversalValidate(sendReversalData);
            }
            catch (AgentConnectException acExp)
            {
                var error = new BusinessError
                {
                    ErrorCategory = "AC Exception",
                    ErrorCode = acExp.ErrorCode.ToString(),
                    Message = acExp.Message
                };

                sendReversalData.Set(error);
            }
            return sendReversalData;
        }

        public SendReversalData SendReversalComplete(SendReversalData sendReversalData)
        {
            try
            {
                // Transaction Lookup
                TransactionLookup(sendReversalData);
                if (DataErrorHandler.CheckForNestedErrors(sendReversalData).Any())
                {
                    return sendReversalData;
                }

                if (sendReversalData.TransactionLookup.TransactionLookupResp.Payload.TransactionStatus != TransactionStatusType.AVAIL)
                {
                    var error = new BusinessError
                    {
                        ErrorCategory = "Custom Error",
                        ErrorCode = string.Empty,
                        Message = "Transaction not available for SendReversal."
                    };

                    sendReversalData.Set(error);
                }

                // Validate SendReversal 
                _validationOperations.SendReversalValidate(sendReversalData);

                // Final Validate(s)
                _validationOperations.SendReversalValidateUntilReadyForCommit(sendReversalData);
                if (DataErrorHandler.CheckForNestedErrors(sendReversalData).Any())
                {
                    return sendReversalData;
                }
                if (sendReversalData.ValidationResponses.Last().Payload.ReadyForCommit)
                {
                    CompleteSession(sendReversalData);
                }
                else
                {
                    var error = new BusinessError
                    {
                        ErrorCategory = "Custom Error",
                        ErrorCode = string.Empty,
                        Message = "Not ready to commit yet"
                    };

                    sendReversalData.Set(error);
                }
            }
            catch (AgentConnectException acExp)
            {
                var error = new BusinessError
                {
                    ErrorCategory = "AC Exception",
                    ErrorCode = acExp.ErrorCode.ToString(),
                    Message = acExp.Message
                };

                sendReversalData.Set(error);
            }
            return sendReversalData;
        }
        public SendReversalData TransactionLookup(SendReversalData sendReversalData)
        {
            try
            {
                sendReversalData.Set(_tranLookupOperations.TransactionLookupForSendReversal(sendReversalData));
                var errors = sendReversalData.TransactionLookup.TransactionLookupResp.Errors ?? new List<BusinessError>();
                sendReversalData.Set(errors);
            }
            catch (AgentConnectException acExp)
            {
                var error = new BusinessError
                {
                    ErrorCategory = "AC Exception",
                    ErrorCode = acExp.ErrorCode.ToString(),
                    Message = acExp.Message
                };

                sendReversalData.Set(error);
            }
            return sendReversalData;
        }

        private SendReversalData CompleteSession(SendReversalData sendReversalData)
        {
            var mgiSessionId = sendReversalData.ValidationResponses.Last().Payload.MgiSessionID;

            var completeSessionRequest = CompleteSessionRequestFactory.CompleteSendReversalRequest(mgiSessionId);
            completeSessionRequest.AgentID = sendReversalData.SendReversalRequest.AgentId;
            completeSessionRequest.AgentSequence = sendReversalData.SendReversalRequest.AgentPos;
            sendReversalData.Set(completeSessionRequest);
            var completeSessionResponse = _acIntegration.CompleteSession(completeSessionRequest);
            var errors = completeSessionResponse.Errors ?? new List<BusinessError>();
            sendReversalData.Set(errors);
            sendReversalData.Set(completeSessionResponse);

            return sendReversalData;
        }
    }
}