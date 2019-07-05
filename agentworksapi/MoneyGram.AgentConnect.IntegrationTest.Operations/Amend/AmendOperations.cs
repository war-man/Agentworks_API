using System.Collections.Generic;
using System.Linq;
using MoneyGram.AgentConnect.DomainModel.Exceptions;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Amend;
using MoneyGram.AgentConnect.IntegrationTest.Data.Common;
using MoneyGram.AgentConnect.IntegrationTest.Data.Requests;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Common;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Lookup;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Helpers;

namespace MoneyGram.AgentConnect.IntegrationTest.Operations.Amend
{
    public class AmendOperations
    {
        private TestAgentConnectIntegration _acIntegration { get; }
        private GetAllFieldsOperations _gafOperations;
        private ValidationOperations _validationOperations;
        private TransactionLookupOperations _tranLookupOperations;

        public AmendOperations(TestRunner testRunner)
        {
            _acIntegration = new TestAgentConnectIntegration(testRunner);
            _gafOperations = new GetAllFieldsOperations(testRunner);
            _validationOperations = new ValidationOperations(testRunner);
            _tranLookupOperations = new TransactionLookupOperations(testRunner);
        }

        public AmendData SingleValidate(AmendData amendData)
        {
            try
            {
                // Get the all fields grouping and types for generation
                amendData.Set(_gafOperations.GetInfoKeysWithGroupTypes(amendData.AmendOperationRequest.AgentId, amendData.AmendOperationRequest.AgentPos, amendData.GafInfoKeysWithGroups, GetAllFieldsTransactionType.Amend));

                // Transaction Lookup
                TransactionLookup(amendData);
                if (DataErrorHandler.CheckForNestedErrors(amendData).Any())
                {
                    return amendData;
                }

                if (amendData.TransactionLookup.TransactionLookupResp.Payload.TransactionStatus != TransactionStatusType.AVAIL)
                {
                    var error = new BusinessError
                    {
                        ErrorCategory = "Custom Error",
                        ErrorCode = string.Empty,
                        Message = "Transaction not available for AMEND."
                    };

                    amendData.Set(error);
                }
                // Validate Receive
                _validationOperations.AmendValidate(amendData);
            }
            catch (AgentConnectException acExp)
            {
                var error = new BusinessError
                {
                    ErrorCategory = "AC Exception",
                    ErrorCode = acExp.ErrorCode.ToString(),
                    Message = acExp.Message
                };

                amendData.Set(error);
            }
            return amendData;
        }

        public AmendData AmendComplete(AmendData amendData)
        {
            try
            {
                // Get the all fields grouping and types for generation
                _gafOperations.GetInfoKeysWithGroupTypes(amendData.AmendOperationRequest.AgentId, amendData.AmendOperationRequest.AgentPos, amendData.GafInfoKeysWithGroups, GetAllFieldsTransactionType.Amend);

                // Transaction Lookup
                TransactionLookup(amendData);
                if (DataErrorHandler.CheckForNestedErrors(amendData).Any())
                {
                    return amendData;
                }

                if (amendData.TransactionLookup.TransactionLookupResp.Payload.TransactionStatus != TransactionStatusType.AVAIL)
                {
                    var error = new BusinessError
                    {
                        ErrorCategory = "Custom Error",
                        ErrorCode = string.Empty,
                        Message = "Transaction not available for AMEND."
                    };

                    amendData.Set(error);
                }

                // Validate Amend 
                _validationOperations.AmendValidate(amendData);

                // Final Validate(s)
                _validationOperations.AmendValidateUntilReadyForCommit(amendData);
                if (DataErrorHandler.CheckForNestedErrors(amendData).Any())
                {
                    return amendData;
                }
                if (amendData.ValidationResponses.Last().Payload.ReadyForCommit)
                {
                    CompleteSession(amendData);
                }
                else
                {
                    var error = new BusinessError
                    {
                        ErrorCategory = "Custom Error",
                        ErrorCode = string.Empty,
                        Message = "Not ready to commit yet"
                    };

                    amendData.Set(error);
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

                amendData.Set(error);
            }
            return amendData;
        }
        public void TransactionLookup(AmendData amendData)
        {
            try
            {
                amendData.Set(_tranLookupOperations.TransactionLookupForAmend(amendData));
                var errors = amendData.TransactionLookup.TransactionLookupResp.Errors ?? new List<BusinessError>();
                amendData.Set(errors);
            }
            catch (AgentConnectException acExp)
            {
                var error = new BusinessError
                {
                    ErrorCategory = "AC Exception",
                    ErrorCode = acExp.ErrorCode.ToString(),
                    Message = acExp.Message
                };

                amendData.Set(error);
            }
        }

        private void CompleteSession(AmendData amendData)
        {
            var mgiSessionId = amendData.ValidationResponses.Last().Payload.MgiSessionID;

            var completeSessionRequest = CompleteSessionRequestFactory.CompleteSendRequest(mgiSessionId);
            completeSessionRequest.AgentID = amendData.AmendOperationRequest.AgentId;
            completeSessionRequest.MgiSessionType = SessionType.AMD;
            completeSessionRequest.AgentSequence = amendData.AmendOperationRequest.AgentPos;
            var completeSessionResponse = _acIntegration.CompleteSession(completeSessionRequest);
            var errors = completeSessionResponse.Errors ?? new List<BusinessError>();
            amendData.Set(errors);

            amendData.Set(completeSessionRequest);
            amendData.Set(completeSessionResponse);
        }
    }
}