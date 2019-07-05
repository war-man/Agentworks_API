using System.Collections.Generic;
using System.Linq;
using MoneyGram.AgentConnect.DomainModel.Exceptions;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Common;
using MoneyGram.AgentConnect.IntegrationTest.Data.Send;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Common;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Lookup;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Helpers;

namespace MoneyGram.AgentConnect.IntegrationTest.Operations.Send
{
    public class SendOperations
    {
        private TestAgentConnectIntegration _acIntegration { get; }
        private ConsumerHistoryLookupOperations _consumerHistLookupOperations;
        private FeeLookupOperations _feeLookupOperations;
        private GetAllFieldsOperations _gafOperations;
        private ValidationOperations _validationOperations;

        public SendOperations(TestRunner testRunner)
        {
            _acIntegration = new TestAgentConnectIntegration(testRunner);
            _consumerHistLookupOperations = new ConsumerHistoryLookupOperations(testRunner);
            _feeLookupOperations = new FeeLookupOperations(testRunner);
            _gafOperations = new GetAllFieldsOperations(testRunner);
            _validationOperations = new ValidationOperations(testRunner);
        }

        public SendData SingleValidateForExistingSender(SendData sendData)
        {
            try
            {
                // Get the all fields grouping and types for generation
                sendData.Set(_gafOperations.GetInfoKeysWithGroupTypes(sendData.SendRequest.AgentId, sendData.SendRequest.AgentPos, sendData.GafInfoKeysWithGroups, GetAllFieldsTransactionType.Send));
                // Consumer History Lookup
                ConsumerHistoryLookupForExistingCustomer(sendData);
                if(DataErrorHandler.CheckForNestedErrors(sendData).Any())
                {
                    return sendData;
                }
                // Fee Lookup
                FeeLookup(sendData);
                if(DataErrorHandler.CheckForNestedErrors(sendData).Any())
                {
                    return sendData;
                }
                // Single Validate
                _validationOperations.SendValidate(sendData);
            }
            catch(AgentConnectException acExp)
            {
                sendData.Set(new BusinessError
                {
                    ErrorCategory = "AC Exception",
                    ErrorCode = acExp.ErrorCode.ToString(),
                    Message = acExp.Message
                });
            }
            return sendData;
        }

        public SendData ValidateUntilReadyForCommit(SendData sendData)
        {
            try
            {
                // Get the all fields grouping and types for generation
                sendData.Set(_gafOperations.GetInfoKeysWithGroupTypes(sendData.SendRequest.AgentId, sendData.SendRequest.AgentPos, sendData.GafInfoKeysWithGroups, GetAllFieldsTransactionType.Send));
                // Consumer History Lookup
                ConsumerHistoryLookupForExistingCustomer(sendData);
                if(DataErrorHandler.CheckForNestedErrors(sendData).Any())
                {
                    return sendData;
                }
                // Fee Lookup
                FeeLookup(sendData);
                if(DataErrorHandler.CheckForNestedErrors(sendData).Any())
                {
                    return sendData;
                }
                // Initial Validate
                _validationOperations.SendValidate(sendData);
                // Final Validate(s)
                _validationOperations.SendValidateUntilReadyForCommit(sendData);
            }
            catch(AgentConnectException acExp)
            {
                sendData.Set(new BusinessError
                {
                    ErrorCategory = "AC Exception",
                    ErrorCode = acExp.ErrorCode.ToString(),
                    Message = acExp.Message
                });
            }
            return sendData;
        }

        public SendData SendCompleteStagedForNewCustomer(SendData sendData)
        {
            try
            {
                // Get the all fields grouping and types for generation
                sendData.Set(_gafOperations.GetInfoKeysWithGroupTypes(sendData.SendRequest.AgentId, sendData.SendRequest.AgentPos, sendData.GafInfoKeysWithGroups, GetAllFieldsTransactionType.Send));
                if (DataErrorHandler.CheckForNestedErrors(sendData).Any())
                {
                    return sendData;
                }

                // Fee Lookup
                FeeLookup(sendData);
                if (DataErrorHandler.CheckForNestedErrors(sendData).Any())
                {
                    return sendData;
                }
                // Initial Validate\
                _validationOperations.SendValidate(sendData, staging: true);
                if (DataErrorHandler.CheckForNestedErrors(sendData).Any())
                {
                    return sendData;
                }
                // Final Validate(s)
                _validationOperations.SendStagedValidateUntilReadyForCommit(sendData);
                if (DataErrorHandler.CheckForNestedErrors(sendData).Any())
                {
                    return sendData;
                }

                CompleteSession(sendData, staging: true);
            }
            catch (AgentConnectException acExp)
            {
                sendData.Set(new BusinessError
                {
                    ErrorCategory = "AC Exception",
                    ErrorCode = acExp.ErrorCode.ToString(),
                    Message = acExp.Message
                });
            }
            return sendData;
        }

        public SendData SendCompleteForNewCustomer(SendData sendData)
        {
            try
            {
                // Get the all fields grouping and types for generation
                sendData.Set(_gafOperations.GetInfoKeysWithGroupTypes(sendData.SendRequest.AgentId, sendData.SendRequest.AgentPos, sendData.GafInfoKeysWithGroups, GetAllFieldsTransactionType.Send));
                if(DataErrorHandler.CheckForNestedErrors(sendData).Any())
                {
                    throw new AgentConnectException();
                }

                // Fee Lookup
                FeeLookup(sendData);
                if(DataErrorHandler.CheckForNestedErrors(sendData).Any())
                {
                    throw new AgentConnectException();
                }
                // Initial Validate
                _validationOperations.SendValidate(sendData);
                if(DataErrorHandler.CheckForNestedErrors(sendData).Any())
                {
                    throw new AgentConnectException();
                }
                // Final Validate(s)
                _validationOperations.SendValidateUntilReadyForCommit(sendData);
                if(DataErrorHandler.CheckForNestedErrors(sendData).Any())
                {
                    throw new AgentConnectException();
                }

                CompleteSession(sendData);
            }
            catch(AgentConnectException acExp)
            {
                sendData.Set(new BusinessError
                {
                    ErrorCategory = "AC Exception",
                    ErrorCode = acExp.ErrorCode.ToString(),
                    Message = acExp.Message
                });
            }
            return sendData;
        }

        public SendData SendCompleteForExistingCustomer(SendData sendData)
        {
            try
            {
                // Get the all fields grouping and types for generation
                sendData.Set(_gafOperations.GetInfoKeysWithGroupTypes(sendData.SendRequest.AgentId, sendData.SendRequest.AgentPos, sendData.GafInfoKeysWithGroups, GetAllFieldsTransactionType.Send));
                // Consumer Lookup - Existing
                ConsumerHistoryLookupForExistingCustomer(sendData);
                if(DataErrorHandler.CheckForNestedErrors(sendData).Any())
                {
                    throw new AgentConnectException();
                }

                // Fee Lookup
                FeeLookup(sendData);
                if(DataErrorHandler.CheckForNestedErrors(sendData).Any())
                {
                    throw new AgentConnectException();
                }
                // Initial Validate
                _validationOperations.SendValidate(sendData);
                if(DataErrorHandler.CheckForNestedErrors(sendData).Any())
                {
                    throw new AgentConnectException();
                }
                // Final Validate(s)
                _validationOperations.SendValidateUntilReadyForCommit(sendData);
                if(DataErrorHandler.CheckForNestedErrors(sendData).Any())
                {
                    throw new AgentConnectException();
                }

                CompleteSession(sendData);
            }
            catch(AgentConnectException acExp)
            {
                sendData.Set(new BusinessError
                {
                    ErrorCategory = "AC Exception",
                    ErrorCode = acExp.ErrorCode.ToString(),
                    Message = acExp.Message
                });
            }
            return sendData;
        }

        public SendData SendCompleteStagedForExistingCustomer(SendData sendData)
        {
            try
            {
                // Get the all fields grouping and types for generation
                sendData.Set(_gafOperations.GetInfoKeysWithGroupTypes(sendData.SendRequest.AgentId, sendData.SendRequest.AgentPos, sendData.GafInfoKeysWithGroups, GetAllFieldsTransactionType.Send));
                // Consumer Lookup - Existing
                ConsumerHistoryLookupForExistingCustomer(sendData);
                if(DataErrorHandler.CheckForNestedErrors(sendData).Any())
                {
                    throw new AgentConnectException();
                }

                // Fee Lookup
                FeeLookup(sendData);
                if(DataErrorHandler.CheckForNestedErrors(sendData).Any())
                {
                    throw new AgentConnectException();
                }
                // Initial Validate
                _validationOperations.SendValidate(sendData, staging: true);
                if(DataErrorHandler.CheckForNestedErrors(sendData).Any())
                {
                    throw new AgentConnectException();
                }
                // Final Validate(s)
                _validationOperations.SendStagedValidateUntilReadyForCommit(sendData);
                if(DataErrorHandler.CheckForNestedErrors(sendData).Any())
                {
                    throw new AgentConnectException();
                }

                CompleteSession(sendData, staging: true);
            }
            catch(AgentConnectException acExp)
            {
                sendData.Set(new BusinessError
                {
                    ErrorCategory = "AC Exception",
                    ErrorCode = acExp.ErrorCode.ToString(),
                    Message = acExp.Message
                });
            }
            return sendData;
        }

        public SendData SendCompleteStagedForExistingCustomerThirdParty(SendData sendData)
        {
            try
            {
                // Get the all fields grouping and types for generation
                sendData.Set(_gafOperations.GetInfoKeysWithGroupTypes(sendData.SendRequest.AgentId, sendData.SendRequest.AgentPos, sendData.GafInfoKeysWithGroups, GetAllFieldsTransactionType.Send));
                // Consumer Lookup - Existing
                ConsumerHistoryLookupForExistingCustomer(sendData);
                if(DataErrorHandler.CheckForNestedErrors(sendData).Any())
                {
                    throw new AgentConnectException();
                }

                // Fee Lookup
                FeeLookup(sendData);
                if(DataErrorHandler.CheckForNestedErrors(sendData).Any())
                {
                    throw new AgentConnectException();
                }
                // Initial Validate
                _validationOperations.SendValidate(sendData, staging: true);
                if(DataErrorHandler.CheckForNestedErrors(sendData).Any())
                {
                    throw new AgentConnectException();
                }
                // Final Validate(s)
                _validationOperations.SendStagedValidateUntilReadyForCommit(sendData);
                if(DataErrorHandler.CheckForNestedErrors(sendData).Any())
                {
                    throw new AgentConnectException();
                }

                CompleteSession(sendData, staging: true);
            }
            catch(AgentConnectException acExp)
            {
                sendData.Set(new BusinessError
                {
                    ErrorCategory = "AC Exception",
                    ErrorCode = acExp.ErrorCode.ToString(),
                    Message = acExp.Message
                });
            }
            return sendData;
        }

        public SendData SendCompleteForThirdParty(SendData sendData)
        {
            try
            {
                // Get the all fields grouping and types for generation
                sendData.Set(_gafOperations.GetInfoKeysWithGroupTypes(sendData.SendRequest.AgentId, sendData.SendRequest.AgentPos, sendData.GafInfoKeysWithGroups, GetAllFieldsTransactionType.Send));
                // Consumer Lookup - Existing
                ConsumerHistoryLookupForExistingCustomer(sendData);
                if(DataErrorHandler.CheckForNestedErrors(sendData).Any())
                {
                    throw new AgentConnectException();
                }

                // Fee Lookup
                FeeLookup(sendData);
                if(DataErrorHandler.CheckForNestedErrors(sendData).Any())
                {
                    throw new AgentConnectException();
                }
                // Initial Validate
                _validationOperations.SendValidate(sendData);
                if(DataErrorHandler.CheckForNestedErrors(sendData).Any())
                {
                    throw new AgentConnectException();
                }
                // Final Validate(s)
                _validationOperations.SendValidateUntilReadyForCommit(sendData);
                if(DataErrorHandler.CheckForNestedErrors(sendData).Any())
                {
                    throw new AgentConnectException();
                }

                CompleteSession(sendData);
            }
            catch(AgentConnectException acExp)
            {
                sendData.Set(new BusinessError
                {
                    ErrorCategory = "AC Exception",
                    ErrorCode = acExp.ErrorCode.ToString(),
                    Message = acExp.Message
                });
            }
            return sendData;
        }

        private SendData FeeLookup(SendData sendData)
        {
            sendData.Set(_feeLookupOperations.FeeLookupForSend(sendData));
            sendData.Set(sendData.FeeLookup.FeeLookupResp.Errors ?? new List<BusinessError>());

            var feeInfos = sendData.FeeLookup.FeeLookupResp?.Payload?.FeeInfos;
            if(feeInfos?.Count > 0)
            {
                sendData.Set(GetPreferredFeeInfo(sendData, feeInfos));
            }

            return sendData;
        }

        private SendData ConsumerHistoryLookupForExistingCustomer(SendData sendData)
        {
            sendData.Set(_consumerHistLookupOperations.ConsumerHistoryLookup(sendData.SendRequest.AgentId, sendData.SendRequest.AgentPos, true, SessionType.SEND));
            sendData.Set(sendData.ConsumerHistoryLookup.ConsumerHistoryLookupResp.Errors ?? new List<BusinessError>());
            if(sendData.ConsumerHistoryLookup.ConsumerHistoryLookupResp?.Payload?.SenderInfos?.SenderInfo.Count > 0)
            {
                sendData.Set(sendData.ConsumerHistoryLookup.ConsumerHistoryLookupResp.Payload.SenderInfos.SenderInfo[0]);
            }

            return sendData;
        }

        private FeeInfo GetPreferredFeeInfo(SendData sendData, List<FeeInfo> feeInfos)
        {
            FeeInfo feeInfo =
                !string.IsNullOrEmpty(sendData.SendRequest.ServiceOption) &&
                feeInfos.Exists(x => x.ServiceOption.Equals(sendData.SendRequest.ServiceOption))
                    ? feeInfos.Where(x => x.ErrorInfo == null)
                        .FirstOrDefault(x => x.ServiceOption.Equals(sendData.SendRequest.ServiceOption))
                    : null;

            if(feeInfo == null) //get the preferred fee quote
            {
                foreach (var preferredOption in ServiceOptionType.PreferredOrder)
                {
                    feeInfo = feeInfos.FirstOrDefault(x => x.ServiceOption.Equals(preferredOption));
                    if (feeInfo != null)
                        break;
                }
            }

            return feeInfo;
        }

        private SendData CompleteSession(SendData sendData, bool staging = false)
        {
            var mgiSessionId = sendData.SendValidationResps.Last().Payload.MgiSessionID;

            var completeSessionRequest = CompleteSessionRequestFactory.CompleteSendRequest(mgiSessionId);
            if(staging)
            {
                completeSessionRequest.Commit = false;
            }
            completeSessionRequest.AgentID = sendData.SendRequest.AgentId;
            completeSessionRequest.AgentSequence = sendData.SendRequest.AgentPos;
            sendData.Set(completeSessionRequest);
            var completeSessionResponse = _acIntegration.CompleteSession(completeSessionRequest);
            sendData.Set(completeSessionResponse);

            return sendData;
        }
    }
}