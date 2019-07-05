using MoneyGram.AgentConnect.DomainModel.Exceptions;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Amend;
using MoneyGram.AgentConnect.IntegrationTest.Data.BillPay;
using MoneyGram.AgentConnect.IntegrationTest.Data.Consumer;
using MoneyGram.AgentConnect.IntegrationTest.Data.ReceiveReversal;
using MoneyGram.AgentConnect.IntegrationTest.Data.Recv;
using MoneyGram.AgentConnect.IntegrationTest.Data.Send;
using MoneyGram.AgentConnect.IntegrationTest.Data.SendReversal;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MoneyGram.AgentConnect.IntegrationTest.Operations.Common
{
    public class ValidationOperations
    {
        private TestAgentConnectIntegration _acIntegration { get; }
        public ValidationOperations(TestRunner testRunner)
        {
            _acIntegration = new TestAgentConnectIntegration(testRunner);
        }

        public SendData SendValidateWithMockData(SendData sendData)
        {
            try
            {
                // Create base request and call validate
                var validationRequest = SendValidationRequestFactory.NewRequestWithBaseData();
                validationRequest.FromFeeInfo(sendData.FeeInfo);
                validationRequest.FromMockData();
                sendData.Set(validationRequest);
                var validationResponse = _acIntegration.SendValidation(sendData);
                sendData.Set(validationResponse.Errors ?? new List<BusinessError>());
                sendData.Set(validationResponse);
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
        public void BillPayValidate(BillPayData billPayData, TestBiller biller, string thirdPartyInfo, bool staging)
        {
            try
            {
                var monitoredListOfFieldsToCollect = new List<string>();
                // Create base request and call validate
                var validationRequest = BillPayValidationRequestFactory.CreateBaseValidationRequest();
                validationRequest.TransactionStaging = staging;
                // Make sure we set the third party type if provided.
                if (!string.IsNullOrWhiteSpace(thirdPartyInfo))
                {
                    if (!validationRequest.FieldValues.Any(x => x.InfoKey == InfoKeyNames.thirdParty_Sender_Type))
                    {
                        validationRequest.FieldValues.Add(new KeyValuePairType { InfoKey = InfoKeyNames.thirdParty_Sender_Type, Value = thirdPartyInfo });
                    }
                }
                // Map potential missing data to the request.
                var billerInfoFieldsToCollectKeys = new List<string>();
                var billerInfoFieldsToCollectInfoBase = new List<InfoBase>();
                var errorFieldsToCollect = new List<string>();
                var billPayFieldsToCollectInfo = new List<FieldToCollectInfo>();

                BPValidationRequest previousRequest = null;
                if (billPayData.BillPayValidationResponseList.Any())
                {
                    var lastValidationResponse = billPayData.BillPayValidationResponseList?.Last();
                    var allRequiredFields = FlattenFields(lastValidationResponse.Payload.FieldsToCollect).Where(y => y.Required.Value).ToList();
                    billerInfoFieldsToCollectKeys.AddRange(allRequiredFields.Select(x => x.InfoKey).ToList());
                    //Get the fields with business errors (check/verify/missing)
                    var validationErrors = lastValidationResponse.Errors?.Where(x => BusinessErrors.ErrorCodesToContinueValidation.Contains(x.ErrorCategory)).Select(x => x.OffendingField).ToList();
                    errorFieldsToCollect = validationErrors != null ? validationErrors : errorFieldsToCollect;
                    monitoredListOfFieldsToCollect.AddRange(errorFieldsToCollect);

                    billerInfoFieldsToCollectInfoBase = lastValidationResponse.Payload.FieldsToCollect;
                    billPayFieldsToCollectInfo = allRequiredFields;
                    // Add fields to collect to monitoredListOfFieldsToCollect so we can monitor what was mapped and what is left.
                    monitoredListOfFieldsToCollect.AddRange(billPayFieldsToCollectInfo.Select(x => x.InfoKey).ToList());
                    // Grab the previous request
                    if (billPayData.BillPayValidationRequestList.Any())
                    {
                        previousRequest = billPayData.BillPayValidationRequestList.Last();
                    }

                }
                //From Previous Request except those with business errors
                if (previousRequest != null)
                {
                    var prevRequestFlds = previousRequest.FieldValues.Where(x => !errorFieldsToCollect.Contains(x.InfoKey)).ToList();
                    prevRequestFlds.ForEach(x => monitoredListOfFieldsToCollect.Remove(x.InfoKey)); //update the monitoredList
                    validationRequest.FieldValues.AddRange(prevRequestFlds);
                }

                // From FeeInfo
                validationRequest.FromFeeInfo(billPayData.FeeLookup.FeeLookupResp.Payload.FeeInfos.FirstOrDefault());
                // From Biller
                validationRequest.FromBillerInfo(billPayData.BillerSearchResponse.Payload.BillerInfo.FirstOrDefault());

                //From DataGenerator
                var newFlds = PopulateFieldValues(billerInfoFieldsToCollectInfoBase, monitoredListOfFieldsToCollect, null, biller);
                newFlds.ForEach(x => monitoredListOfFieldsToCollect.Remove(x.InfoKey)); //update the monitoredList
                validationRequest.FieldValues.AddRange(newFlds);
                validationRequest.AgentID = billPayData.BillPayRequest.AgentId;
                validationRequest.AgentSequence = billPayData.BillPayRequest.AgentPos;
                billPayData.Set(validationRequest);
                var validationResponse = _acIntegration.BPValidation(validationRequest);
                billPayData.Set(validationResponse);
            }
            catch (AgentConnectException acExp)
            {
                billPayData.Errors.Add(new BusinessError
                {
                    ErrorCategory = "AC Exception",
                    ErrorCode = acExp.ErrorCode.ToString(),
                    Message = acExp.Message
                });
            }
        }

        public void BillPayValidate(BillPayData billPayData, TestBiller biller, string thirdPartyInfo)
        {
            try
            {
                var monitoredListOfFieldsToCollect = new List<string>();
                // Create base request and call validate
                var validationRequest = BillPayValidationRequestFactory.CreateBaseValidationRequest();
                // Make sure we set the third party type if provided.
                if (!string.IsNullOrWhiteSpace(thirdPartyInfo))
                {
                    if (!validationRequest.FieldValues.Any(x => x.InfoKey == InfoKeyNames.thirdParty_Sender_Type))
                    {
                        validationRequest.FieldValues.Add(new KeyValuePairType { InfoKey = InfoKeyNames.thirdParty_Sender_Type, Value = thirdPartyInfo });
                    }
                }
                // Map potential missing data to the request.
                var billerInfoFieldsToCollectKeys = new List<string>();
                var billerInfoFieldsToCollectInfoBase = new List<InfoBase>();
                var errorFieldsToCollect = new List<string>();
                var billPayFieldsToCollectInfo = new List<FieldToCollectInfo>();

                BPValidationRequest previousRequest = null;
                if (billPayData.BillPayValidationResponseList.Any())
                {
                    var lastValidationResponse = billPayData.BillPayValidationResponseList?.Last();
                    var allRequiredFields = FlattenFields(lastValidationResponse.Payload.FieldsToCollect).Where(y => y.Required.Value).ToList();
                    billerInfoFieldsToCollectKeys.AddRange(allRequiredFields.Select(x => x.InfoKey).ToList());
                    //Get the fields with business errors (check/verify/missing)
                    var validationErrors = lastValidationResponse.Errors?.Where(x => BusinessErrors.ErrorCodesToContinueValidation.Contains(x.ErrorCategory)).Select(x => x.OffendingField).ToList();
                    errorFieldsToCollect = validationErrors != null ? validationErrors : errorFieldsToCollect;
                    monitoredListOfFieldsToCollect.AddRange(errorFieldsToCollect);

                    billerInfoFieldsToCollectInfoBase = lastValidationResponse.Payload.FieldsToCollect;
                    billPayFieldsToCollectInfo = allRequiredFields;
                    // Add fields to collect to monitoredListOfFieldsToCollect so we can monitor what was mapped and what is left.
                    monitoredListOfFieldsToCollect.AddRange(billPayFieldsToCollectInfo.Select(x => x.InfoKey).ToList());
                    // Grab the previous request
                    if (billPayData.BillPayValidationRequestList.Any())
                    {
                        previousRequest = billPayData.BillPayValidationRequestList.Last();
                    }

                }
                //From Previous Request except those with business errors
                if (previousRequest != null)
                {
                    var prevRequestFlds = previousRequest.FieldValues.Where(x => !errorFieldsToCollect.Contains(x.InfoKey)).ToList();
                    prevRequestFlds.ForEach(x => monitoredListOfFieldsToCollect.Remove(x.InfoKey)); //update the monitoredList
                    validationRequest.FieldValues.AddRange(prevRequestFlds);
                }

                // From FeeInfo
                validationRequest.FromFeeInfo(billPayData.FeeLookup.FeeLookupResp.Payload.FeeInfos.FirstOrDefault());
                // From Biller
                validationRequest.FromBillerInfo(billPayData.BillerSearchResponse.Payload.BillerInfo.FirstOrDefault());

                //From DataGenerator
                var newFlds = PopulateFieldValues(billerInfoFieldsToCollectInfoBase, monitoredListOfFieldsToCollect, null, biller);
                newFlds.ForEach(x => monitoredListOfFieldsToCollect.Remove(x.InfoKey)); //update the monitoredList
                validationRequest.FieldValues.AddRange(newFlds);
                validationRequest.AgentID = billPayData.BillPayRequest.AgentId;
                validationRequest.AgentSequence = billPayData.BillPayRequest.AgentPos;
                billPayData.Set(validationRequest);
                var validationResponse = _acIntegration.BPValidation(validationRequest);
                billPayData.Set(validationResponse);
            }
            catch (AgentConnectException acExp)
            {
                billPayData.Errors.Add(new BusinessError
                {
                    ErrorCategory = "AC Exception",
                    ErrorCode = acExp.ErrorCode.ToString(),
                    Message = acExp.Message
                });
            }
        }
        public SendData SendValidate(SendData sendData, bool staging = false)
        {
            try
            {
                var monitoredListOfFieldsToCollect = new List<string>();
                // Create base request and call validate
                var validationRequest = SendValidationRequestFactory.NewRequestWithBaseData();
                validationRequest.TransactionStaging = staging;

                // Map potential missing data to the request.
                var senderInfoFieldsToCollectKeys = new List<string>();
                var senderInfoFieldsToCollectInfoBase = new List<InfoBase>();
                var errorFieldsToCollect = new List<string>();
                var sendFieldsToCollectInfo = new List<FieldToCollectInfo>();

                SendValidationRequest previousRequest = null;
                if (sendData.SendValidationResps.Any())
                {
                    var lastValidationResponse = sendData.SendValidationResps?.Last();
                    var allRequiredFields = FlattenFields(lastValidationResponse.Payload.FieldsToCollect).Where(y => y.Required.Value).ToList();
                    senderInfoFieldsToCollectKeys.AddRange(allRequiredFields.Select(x => x.InfoKey).ToList());
                    //Get the fields with business errors (check/verify/missing)
                    var validationErrors = lastValidationResponse.Errors?.Where(x => BusinessErrors.ErrorCodesToContinueValidation.Contains(x.ErrorCategory)).Select(x => x.OffendingField).ToList();
                    errorFieldsToCollect = validationErrors != null ? validationErrors : errorFieldsToCollect;

                    senderInfoFieldsToCollectInfoBase = lastValidationResponse.Payload.FieldsToCollect;
                    sendFieldsToCollectInfo = allRequiredFields;
                    // Add fields to collect to monitoredListOfFieldsToCollect so we can monitor what was mapped and what is left.
                    monitoredListOfFieldsToCollect.AddRange(sendFieldsToCollectInfo.Select(x => x.InfoKey).ToList());
                    // Grab the previous request
                    if (sendData.SendValidationReqs.Any())
                    {
                        previousRequest = sendData.SendValidationReqs.Last();
                    }
                }

                // Make sure we set the third party type if provided and not already included.
                if ((!validationRequest.FieldValues.Any(x => x.InfoKey == InfoKeyNames.thirdParty_Sender_Type)) &&
                    ((previousRequest == null) ||
                     (!previousRequest.FieldValues.Any(x => x.InfoKey == InfoKeyNames.thirdParty_Sender_Type))))
                {
                    validationRequest.FieldValues.Add(new KeyValuePairType
                    {
                        InfoKey = InfoKeyNames.thirdParty_Sender_Type,
                        Value = string.IsNullOrWhiteSpace(sendData.SendRequest.ThirdPartyType) ? TestThirdPartyType.None:sendData.SendRequest.ThirdPartyType
                    });
                }

                //From Previous Request except those with business errors
                if (previousRequest != null)
                {
                    var prevRequestFlds = previousRequest.FieldValues.Where(x => !errorFieldsToCollect.Contains(x.InfoKey)).ToList();
                    prevRequestFlds.ForEach(x => monitoredListOfFieldsToCollect.Remove(x.InfoKey)); //update the monitoredList
                    validationRequest.FieldValues.AddRange(prevRequestFlds);
                }

                // From FeeInfo
                validationRequest.FromFeeInfo(sendData.FeeInfo);
                // From Sender, will map data if performing existing customer flow, else null
                if (sendData.SenderInfo != null)
                {
                    validationRequest.FromSender(sendData.SenderInfo.CurrentValues, senderInfoFieldsToCollectKeys, previousRequest, monitoredListOfFieldsToCollect);
                }
                // From FeeLookupRequest
                validationRequest.FromFeeLookupRequest(sendData.FeeLookup.FeeLookupReq, senderInfoFieldsToCollectKeys);

                //From DataGenerator
                var newFlds = PopulateFieldValues(senderInfoFieldsToCollectInfoBase, monitoredListOfFieldsToCollect);
                newFlds.ForEach(x => monitoredListOfFieldsToCollect.Remove(x.InfoKey)); //update the monitoredList
                validationRequest.FieldValues.AddRange(newFlds);

                sendData.Set(validationRequest);
                var validationResponse = _acIntegration.SendValidation(sendData);
                sendData.Set(validationResponse);
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

        public void RecvValidate(ReceiveData recvData, string thirdPartyInfo, bool staging = false)
        {
            try
            {
                var monitoredListOfFieldsToCollect = new List<string>();
                // Create base request and call validate
                var validationRequest = ReceiveValidationRequestFactory.NewRequestWithBaseData();
                validationRequest.TransactionStaging = staging;
                // Make sure we set the third party type if provided.
                if (!string.IsNullOrWhiteSpace(thirdPartyInfo))
                {
                    if (!validationRequest.FieldValues.Any(x => x.InfoKey == InfoKeyNames.thirdParty_Receiver_Type))
                    {
                        validationRequest.FieldValues.Add(new KeyValuePairType { InfoKey = InfoKeyNames.thirdParty_Receiver_Type, Value = thirdPartyInfo });
                    }
                }
                // Map potential missing data to the request.
                var receiverInfoFieldsToCollectKeys = new List<string>();
                var receiverInfoFieldsToCollectInfoBase = new List<InfoBase>();
                var errorFieldsToCollect = new List<string>();
                var receiveFieldsToCollectInfoList = new List<FieldToCollectInfo>();

                ReceiveValidationRequest previousRequest = null;
                if (recvData.ReceiveValidationResponses.Any())
                {
                    var lastValidationResponse = recvData.ReceiveValidationResponses?.Last();
                    var allRequiredFields = FlattenFields(lastValidationResponse.Payload.FieldsToCollect).Where(x => x.Required.Value).ToList();
                    receiverInfoFieldsToCollectKeys.AddRange(allRequiredFields.Select(x => x.InfoKey).ToList());
                    //Get the fields with business errors (check/verify/missing)
                    var validationErrors = lastValidationResponse.Errors?.Where(x => BusinessErrors.ErrorCodesToContinueValidation.Contains(x.ErrorCategory)).Select(x => x.OffendingField).ToList();
                    errorFieldsToCollect = validationErrors != null ? validationErrors : errorFieldsToCollect;

                    receiverInfoFieldsToCollectInfoBase = recvData.ReceiveValidationResponses.Last().Payload.FieldsToCollect;
                    receiveFieldsToCollectInfoList = allRequiredFields;
                    // Add fields to collect to monitoredListOfFieldsToCollect so we can monitor what was mapped and what is left.
                    monitoredListOfFieldsToCollect.AddRange(receiverInfoFieldsToCollectKeys);
                    // Grab the previous request
                    if (recvData.ReceiveValidationResponses.Any())
                    {
                        previousRequest = recvData.ReceiveValidationRequests.Last();
                    }
                }
                //From Previous Request except those with business errors
                if (previousRequest != null)
                {
                    var prevRequestFlds = previousRequest.FieldValues.Where(x => !errorFieldsToCollect.Contains(x.InfoKey)).ToList();
                    prevRequestFlds.ForEach(x => monitoredListOfFieldsToCollect.Remove(x.InfoKey)); //update the monitoredList
                    validationRequest.FieldValues.AddRange(prevRequestFlds);
                }
                // From Fee Info
                if (recvData.TransactionLookupResponse.Payload != null)
                {
                    validationRequest.FromTransactionLookup(recvData.TransactionLookupResponse.Payload);
                }

                //From DataGenerator
                var newFlds = PopulateFieldValues(receiverInfoFieldsToCollectInfoBase, monitoredListOfFieldsToCollect);
                newFlds.ForEach(x => monitoredListOfFieldsToCollect.Remove(x.InfoKey)); //update the monitoredList
                validationRequest.FieldValues.AddRange(newFlds);

                recvData.ReceiveValidationRequests.Add(validationRequest);
                var validationResponse = _acIntegration.ReceiveValidation(recvData);
                recvData.ReceiveValidationResponses.Add(validationResponse);
            }
            catch (AgentConnectException acExp)
            {
                recvData.Errors.Add(new BusinessError
                {
                    ErrorCategory = "AC Exception",
                    ErrorCode = acExp.ErrorCode.ToString(),
                    Message = acExp.Message
                });
            }
        }

        public void AmendValidate(AmendData amendData)
        {
            try
            {
                var monitoredListOfFieldsToCollect = new List<string>();
                var errorFieldsToCollect = new List<string>();
                var transactionLookupResponse = amendData.TransactionLookup.TransactionLookupResp;
                // Create base request and call validate
                var validationRequest = AmendValidationRequestFactory.NewRequestWithBaseData(transactionLookupResponse.Payload.MgiSessionID);

                // Map potential missing data to the request.
                var amendInfoFieldsToCollectKeys = new List<string>();
                var amendInfoFieldsToCollectInfoBase = new List<InfoBase>();
                var amendRequiredFieldsToCollectInfo = new List<FieldToCollectInfo>();

                AmendValidationRequest previousRequest = null;
                if (amendData.ValidationResponses.Any())
                {
                    var lastValidationResponse = amendData.ValidationResponses.Last();

                    //Get the fields with business errors (check/verify/missing)
                    var validationErrors = lastValidationResponse.Errors?.Where(x => BusinessErrors.ErrorCodesToContinueValidation.Contains(x.ErrorCategory)).Select(x => x.OffendingField).ToList();
                    errorFieldsToCollect = validationErrors != null ? validationErrors : errorFieldsToCollect;

                    amendInfoFieldsToCollectInfoBase = lastValidationResponse.Payload.FieldsToCollect;
                    amendRequiredFieldsToCollectInfo = FlattenFields(amendInfoFieldsToCollectInfoBase).Where(x => x.Required.GetValueOrDefault()).ToList();
                    // Add fields to collect to monitoredListOfFieldsToCollect so we can monitor what was mapped and what is left.
                    monitoredListOfFieldsToCollect.AddRange(amendRequiredFieldsToCollectInfo.Select(x => x.InfoKey).ToList());

                    // Grab the previous request
                    if (amendData.ValidationRequests.Any())
                    {
                        previousRequest = amendData.ValidationRequests.Last();
                    }
                }

                //From Previous Request except those with business errors
                if (previousRequest != null)
                {
                    var prevRequestFlds = previousRequest.FieldValues.Where(x => !errorFieldsToCollect.Contains(x.InfoKey)).ToList();
                    prevRequestFlds.ForEach(x => monitoredListOfFieldsToCollect.Remove(x.InfoKey)); //update the monitoredList
                    validationRequest.FieldValues.AddRange(prevRequestFlds);
                }

                //From TransactionLookup
                var tranLookupFlds = transactionLookupResponse.Payload.CurrentValues.Where(x => monitoredListOfFieldsToCollect.Contains(x.InfoKey)).ToList();
                tranLookupFlds.ForEach(x => monitoredListOfFieldsToCollect.Remove(x.InfoKey)); //update the monitoredList
                validationRequest.FieldValues.AddRange(tranLookupFlds);

                //From DataGenerator
                var newFlds = PopulateFieldValues(amendInfoFieldsToCollectInfoBase, monitoredListOfFieldsToCollect);
                newFlds.ForEach(x => monitoredListOfFieldsToCollect.Remove(x.InfoKey)); //update the monitoredList
                validationRequest.FieldValues.AddRange(newFlds);

                amendData.ValidationRequests.Add(validationRequest);
                var validationResponse = _acIntegration.AmendValidation(amendData);
                amendData.ValidationResponses.Add(validationResponse);
            }
            catch (AgentConnectException acExp)
            {
                amendData.Errors.Add(new BusinessError
                {
                    ErrorCategory = "AC Exception",
                    ErrorCode = acExp.ErrorCode.ToString(),
                    Message = acExp.Message
                });
            }
        }

        public void SendReversalValidate(SendReversalData sendReversalData)
        {
            try
            {
                var monitoredListOfFieldsToCollect = new List<string>();
                var errorFieldsToCollect = new List<string>();
                var transactionLookupResponse = sendReversalData.TransactionLookup.TransactionLookupResp;
                // Create base request and call validate
                var validationRequest = SendReversalValidationRequestFactory.NewRequestWithBaseData();

                // Map potential missing data to the request.
                var sendReversalInfoFieldsToCollectInfoBase = new List<InfoBase>();
                var sendReversalRequiredFieldsToCollectInfo = new List<FieldToCollectInfo>();

                SendReversalValidationRequest previousRequest = null;
                if (sendReversalData.ValidationResponses.Any())
                {
                    var lastValidationResponse = sendReversalData.ValidationResponses.Last();

                    //Get the fields with business errors (check/verify/missing)
                    var validationErrors = lastValidationResponse.Errors?.Where(x => BusinessErrors.ErrorCodesToContinueValidation.Contains(x.ErrorCategory)).Select(x => x.OffendingField).ToList();
                    errorFieldsToCollect = validationErrors != null ? validationErrors : errorFieldsToCollect;
                    // Add ERROR fields to collect to monitoredListOfFieldsToCollect so we can monitor what was mapped and what is left.
                    monitoredListOfFieldsToCollect.AddRange(errorFieldsToCollect);

                    sendReversalInfoFieldsToCollectInfoBase = lastValidationResponse.Payload.FieldsToCollect;
                    sendReversalRequiredFieldsToCollectInfo = FlattenFields(sendReversalInfoFieldsToCollectInfoBase).Where(x => x.Required.GetValueOrDefault()).ToList();
                    // Add fields to collect to monitoredListOfFieldsToCollect so we can monitor what was mapped and what is left.
                    monitoredListOfFieldsToCollect.AddRange(sendReversalRequiredFieldsToCollectInfo.Select(x => x.InfoKey).ToList());

                    // Grab the previous request
                    if (sendReversalData.ValidationRequests.Any())
                    {
                        previousRequest = sendReversalData.ValidationRequests.Last();
                    }
                }

                //base data from Transaction Lookup.
                validationRequest.FromTransactionLookup(transactionLookupResponse.Payload);

                //From Previous Request except those with business errors
                if (previousRequest != null)
                {
                    var prevRequestFlds = previousRequest.FieldValues.Where(x => !errorFieldsToCollect.Contains(x.InfoKey)).ToList();
                    prevRequestFlds.ForEach(x => monitoredListOfFieldsToCollect.Remove(x.InfoKey)); //update the monitoredList
                    validationRequest.FieldValues.AddRange(prevRequestFlds);
                }

                //From TransactionLookup
                var tranLookupFlds = transactionLookupResponse.Payload.CurrentValues.Where(x => monitoredListOfFieldsToCollect.Contains(x.InfoKey)).ToList();
                tranLookupFlds.ForEach(x => monitoredListOfFieldsToCollect.Remove(x.InfoKey)); //update the monitoredList
                validationRequest.FieldValues.AddRange(tranLookupFlds);

                //From user values in SendReversalRequest
                var userFieldValues = GetUserFieldsList(sendReversalData);
                //From DataGenerator
                var newFlds = PopulateFieldValues(sendReversalInfoFieldsToCollectInfoBase, monitoredListOfFieldsToCollect, userFieldValues);
                newFlds.ForEach(x => monitoredListOfFieldsToCollect.Remove(x.InfoKey)); //update the monitoredList
                validationRequest.FieldValues.AddRange(newFlds);

                sendReversalData.ValidationRequests.Add(validationRequest);
                var validationResponse = _acIntegration.SendReversalValidation(sendReversalData);
                sendReversalData.ValidationResponses.Add(validationResponse);
            }
            catch (AgentConnectException acExp)
            {
                sendReversalData.Errors.Add(new BusinessError
                {
                    ErrorCategory = "AC Exception",
                    ErrorCode = acExp.ErrorCode.ToString(),
                    Message = acExp.Message
                });
            }
        }

        public void ReceiveReversalValidate(ReceiveReversalData receiveReversalData)
        {
            try
            {
                var monitoredListOfFieldsToCollect = new List<string>();
                var errorFieldsToCollect = new List<string>();
                var transactionLookupResponse = receiveReversalData.TransactionLookup.TransactionLookupResp;
                // Create base request and call validate
                var validationRequest = ReceiveReversalValidationRequestFactory.NewRequestWithBaseData();

                // Map potential missing data to the request.
                var receiveReversalInfoFieldsToCollectInfoBase = new List<InfoBase>();
                var receiveReversalRequiredFieldsToCollectInfo = new List<FieldToCollectInfo>();

                ReceiveReversalValidationRequest previousRequest = null;
                if (receiveReversalData.ValidationResponses.Any())
                {
                    var lastValidationResponse = receiveReversalData.ValidationResponses.Last();

                    //Get the fields with business errors (check/verify/missing)
                    var validationErrors = lastValidationResponse.Errors?.Where(x => BusinessErrors.ErrorCodesToContinueValidation.Contains(x.ErrorCategory)).Select(x => x.OffendingField).ToList();
                    errorFieldsToCollect = validationErrors != null ? validationErrors : errorFieldsToCollect;
                    // Add ERROR fields to collect to monitoredListOfFieldsToCollect so we can monitor what was mapped and what is left.
                    monitoredListOfFieldsToCollect.AddRange(errorFieldsToCollect);

                    receiveReversalInfoFieldsToCollectInfoBase = lastValidationResponse.Payload.FieldsToCollect;
                    receiveReversalRequiredFieldsToCollectInfo = FlattenFields(receiveReversalInfoFieldsToCollectInfoBase).Where(x => x.Required.GetValueOrDefault()).ToList();
                    // Add fields to collect to monitoredListOfFieldsToCollect so we can monitor what was mapped and what is left.
                    monitoredListOfFieldsToCollect.AddRange(receiveReversalRequiredFieldsToCollectInfo.Select(x => x.InfoKey).ToList());

                    // Grab the previous request
                    if (receiveReversalData.ValidationRequests.Any())
                    {
                        previousRequest = receiveReversalData.ValidationRequests.Last();
                    }
                }

                //base data from Transaction Lookup.
                validationRequest.FromTransactionLookup(transactionLookupResponse.Payload);

                //From Previous Request except those with business errors
                if (previousRequest != null)
                {
                    var prevRequestFlds = previousRequest.FieldValues.Where(x => !errorFieldsToCollect.Contains(x.InfoKey)).ToList();
                    prevRequestFlds.ForEach(x => monitoredListOfFieldsToCollect.Remove(x.InfoKey)); //update the monitoredList
                    validationRequest.FieldValues.AddRange(prevRequestFlds);
                }

                //From TransactionLookup
                var tranLookupFlds = transactionLookupResponse.Payload.CurrentValues.Where(x => monitoredListOfFieldsToCollect.Contains(x.InfoKey)).ToList();
                tranLookupFlds.ForEach(x => monitoredListOfFieldsToCollect.Remove(x.InfoKey)); //update the monitoredList
                validationRequest.FieldValues.AddRange(tranLookupFlds);

                //From DataGenerator
                var newFlds = PopulateFieldValues(receiveReversalInfoFieldsToCollectInfoBase, monitoredListOfFieldsToCollect);
                newFlds.ForEach(x => monitoredListOfFieldsToCollect.Remove(x.InfoKey)); //update the monitoredList
                validationRequest.FieldValues.AddRange(newFlds);

                receiveReversalData.ValidationRequests.Add(validationRequest);
                var validationResponse = _acIntegration.ReceiveReversalValidation(receiveReversalData);
                receiveReversalData.ValidationResponses.Add(validationResponse);
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
        public void BillPayValidateUntilReadyForCommit(BillPayData billPayData, TestBiller biller, bool staging, string thirdPartyType = "NONE", int maxValidationCycles = 5)
        {
            int billPayValidateCallCount = 0;
            var lastValidateResponse = billPayData.BillPayValidationResponseList?.LastOrDefault();
            while ((billPayValidateCallCount < maxValidationCycles) && ContinueValidationLoop(lastValidateResponse))
            {
                BillPayValidate(billPayData, biller, thirdPartyType, staging: staging);

                lastValidateResponse = billPayData.BillPayValidationResponseList.LastOrDefault();
                billPayValidateCallCount++;
            }

            billPayData.Errors.AddRange(lastValidateResponse.Errors ?? new List<BusinessError>());
            billPayData.Errors.AddRange(!lastValidateResponse.Payload.ReadyForCommit ? new List<BusinessError> { new BusinessError { Message = "Exited BillPayValidation loop. Not ready to commit yet." } } : new List<BusinessError>());
        }

        public void BillPayValidateUntilReadyForCommit(BillPayData billPayData, TestBiller biller, string thirdPartyType = "NONE", int maxValidationCycles = 5)
        {
            int billPayValidateCallCount = 0;
            var lastValidateResponse = billPayData.BillPayValidationResponseList?.LastOrDefault();
            while ((billPayValidateCallCount < maxValidationCycles) && ContinueValidationLoop(lastValidateResponse))
            {
                BillPayValidate(billPayData, biller, thirdPartyType);

                lastValidateResponse = billPayData.BillPayValidationResponseList.LastOrDefault();
                billPayValidateCallCount++;
            }

            billPayData.Errors.AddRange(lastValidateResponse.Errors ?? new List<BusinessError>());
            billPayData.Errors.AddRange(!lastValidateResponse.Payload.ReadyForCommit ? new List<BusinessError> { new BusinessError { Message = "Exited BillPayValidation loop. Not ready to commit yet." } } : new List<BusinessError>());
        }

        public SendData SendValidateUntilReadyForCommit(SendData sendData, int maxValidationCycles = 5)
        {
            int sendValidateCallCount = 0;
            var lastValidateResponse = sendData.SendValidationResps?.LastOrDefault();
            while (sendValidateCallCount < maxValidationCycles && ContinueValidationLoop(lastValidateResponse))
            {
                SendValidate(sendData);

                lastValidateResponse = sendData.SendValidationResps.LastOrDefault();
                sendValidateCallCount++;
            }

            sendData.Set(lastValidateResponse.Errors ?? new List<BusinessError>());
            sendData.Set(!lastValidateResponse.Payload.ReadyForCommit ? new List<BusinessError> { new BusinessError { Message = "Exited SendValidation loop. Not ready to commit yet." } } : new List<BusinessError>());

            return sendData;
        }

        public SendData SendStagedValidateUntilReadyForCommit(SendData sendData, int maxValidationCycles = 5)
        {
            int sendValidateCallCount = 0;
            var lastValidateResponse = sendData.SendValidationResps?.LastOrDefault();
            while (sendValidateCallCount < maxValidationCycles && ContinueValidationLoop(lastValidateResponse))
            {
                SendValidate(sendData, true);

                lastValidateResponse = sendData.SendValidationResps.LastOrDefault();
                sendValidateCallCount++;
            }

            sendData.Set(lastValidateResponse.Errors ?? new List<BusinessError>());
            sendData.Set(!lastValidateResponse.Payload.ReadyForCommit ? new List<BusinessError> { new BusinessError { Message = "Exited SendValidation loop. Not ready to commit yet." } } : new List<BusinessError>());

            return sendData;
        }
        public void RecvValidateUntilReadyForCommit(ReceiveData recvData, int maxValidationCycles = 5)
        {
            int recvValidateCallCount = 0;
            var lastValidateResponse = recvData.ReceiveValidationResponses?.LastOrDefault();
            while (recvValidateCallCount < maxValidationCycles && ContinueValidationLoop(lastValidateResponse))
            {
                RecvValidate(recvData, "NONE");

                lastValidateResponse = recvData.ReceiveValidationResponses.LastOrDefault();
                recvValidateCallCount++;
            }

            recvData.Errors.AddRange(lastValidateResponse.Errors ?? new List<BusinessError>());
            recvData.Errors.AddRange(!lastValidateResponse.Payload.ReadyForCommit ? new List<BusinessError> { new BusinessError { Message = "Exited RecvValidation loop. Not ready to commit yet." } } : new List<BusinessError>());
        }

        public void RecvStagedValidateUntilReadyForCommit(ReceiveData recvData, int maxValidationCycles = 5)
        {
            int recvValidateCallCount = 0;
            var lastValidateResponse = recvData.ReceiveValidationResponses?.LastOrDefault();
            while (recvValidateCallCount < maxValidationCycles && ContinueValidationLoop(lastValidateResponse))
            {
                RecvValidate(recvData, "NONE", staging: true);

                lastValidateResponse = recvData.ReceiveValidationResponses.LastOrDefault();
                recvValidateCallCount++;
            }

            recvData.Errors.AddRange(lastValidateResponse.Errors ?? new List<BusinessError>());
            recvData.Errors.AddRange(!lastValidateResponse.Payload.ReadyForCommit ? new List<BusinessError> { new BusinessError { Message = "Exited RecvValidation loop. Not ready to commit yet." } } : new List<BusinessError>());
        }

        public void AmendValidateUntilReadyForCommit(AmendData amendData, int maxValidationCycles = 5)
        {
            int amendValidateCallCount = 0;
            var lastValidateResponse = amendData.ValidationResponses?.LastOrDefault();
            while (amendValidateCallCount < maxValidationCycles && ContinueValidationLoop(lastValidateResponse))
            {
                AmendValidate(amendData);

                lastValidateResponse = amendData.ValidationResponses.LastOrDefault();
                amendValidateCallCount++;
            }
            amendData.Errors.AddRange(!(lastValidateResponse.Payload?.ReadyForCommit).GetValueOrDefault() ? new List<BusinessError> { new BusinessError { Message = "Exited AmendValidation loop. Not ready to commit yet." } } : new List<BusinessError>());
        }

        public void SendReversalValidateUntilReadyForCommit(SendReversalData sendReversalData, int maxValidationCycles = 5)
        {
            int sendReversalValidateCallCount = 0;
            var lastValidateResponse = sendReversalData.ValidationResponses?.LastOrDefault();
            while (sendReversalValidateCallCount < maxValidationCycles && ContinueValidationLoop(lastValidateResponse))
            {
                SendReversalValidate(sendReversalData);

                lastValidateResponse = sendReversalData.ValidationResponses.LastOrDefault();
                sendReversalValidateCallCount++;
            }
            sendReversalData.Errors.AddRange(!(lastValidateResponse.Payload?.ReadyForCommit).GetValueOrDefault() ? new List<BusinessError> { new BusinessError { Message = "Exited SendReversalValidation loop. Not ready to commit yet." } } : new List<BusinessError>());
        }

        public void ReceiveReversalValidateUntilReadyForCommit(ReceiveReversalData receiveReversalData, int maxValidationCycles = 5)
        {
            int receiveReversalValidateCallCount = 0;
            var lastValidateResponse = receiveReversalData.ValidationResponses?.LastOrDefault();
            while (receiveReversalValidateCallCount < maxValidationCycles && ContinueValidationLoop(lastValidateResponse))
            {
                ReceiveReversalValidate(receiveReversalData);

                lastValidateResponse = receiveReversalData.ValidationResponses.LastOrDefault();
                receiveReversalValidateCallCount++;
            }
            receiveReversalData.Errors.AddRange(!(lastValidateResponse.Payload?.ReadyForCommit).GetValueOrDefault() ? new List<BusinessError> { new BusinessError { Message = "Exited ReceiveReversalValidation loop. Not ready to commit yet." } } : new List<BusinessError>());
        }

        public  CreateOrUpdateProfileConsumerResponse CreateOrUpdateProfileConsumerValidate(
            CreateOrUpdateProfileConsumerData createOrUpdateProfileConsumerData, List<InfoBase> fieldsToCollect, int maxValidationCycles = 5)
        {
            int validateCallCount = 0;
            var continueValidation = true;
            CreateOrUpdateProfileConsumerResponse result = null;

            while (validateCallCount < maxValidationCycles && continueValidation)
            {

                var monitoredListOfFieldsToCollect =
                    FlattenFields(fieldsToCollect).Where(x => x.Required == true).Select(x => x.InfoKey).ToList();
               
                var newFlds = PopulateFieldValues(fieldsToCollect, monitoredListOfFieldsToCollect);

                createOrUpdateProfileConsumerData.CreateOrUpdateProfileConsumerRequest.FieldValues.ForEach(x =>
                {
                    var itemToDelete = newFlds.FirstOrDefault(item => item.InfoKey == x.InfoKey);
                    if (itemToDelete != null)
                        newFlds.Remove(itemToDelete);
                });

                createOrUpdateProfileConsumerData.CreateOrUpdateProfileConsumerRequest.FieldValues.AddRange(
                    newFlds.Where(x => !string.IsNullOrWhiteSpace(x.Value)));
                result = _acIntegration.CreateOrUpdateProfileConsumer(createOrUpdateProfileConsumerData);
                continueValidation = result.Payload.FieldsToCollect.Count != 0;
                if (continueValidation)
                    fieldsToCollect = result.Payload.FieldsToCollect;
                validateCallCount++;
            }
            return result;
        }

        public CreateOrUpdateProfileSenderResponse CreateOrUpdateProfileSenderValidate(
            CreateOrUpdateProfileSenderData createOrUpdateProfileSenderData, List<InfoBase> fieldsToCollect, int maxValidationCycles = 5)
        {
            int validateCallCount = 0;
            var continueValidation = true;
            CreateOrUpdateProfileSenderResponse result = null;
            while (validateCallCount < maxValidationCycles && continueValidation)
            {
                var monitoredListOfFieldsToCollect =
                    FlattenFields(fieldsToCollect).Where(x => x.Required == true).Select(x => x.InfoKey).ToList();
                var newFlds = PopulateFieldValues(fieldsToCollect, monitoredListOfFieldsToCollect);

                createOrUpdateProfileSenderData.CreateOrUpdateProfileSenderRequest.FieldValues.ForEach(x =>
                {
                    var itemToDelete = newFlds.FirstOrDefault(item => item.InfoKey == x.InfoKey);
                    if (itemToDelete != null)
                        newFlds.Remove(itemToDelete);
                });
                createOrUpdateProfileSenderData.CreateOrUpdateProfileSenderRequest.FieldValues.AddRange(newFlds);
                result = _acIntegration.CreateOrUpdateProfileSender(createOrUpdateProfileSenderData);
                continueValidation = result.Payload.FieldsToCollect.Count != 0;
                if (continueValidation)
                    fieldsToCollect = result.Payload.FieldsToCollect;
                ++validateCallCount;
            }
            return result;
        }

        public CreateOrUpdateProfileReceiverResponse CreateOrUpdateProfileReceiverValidate(
            CreateOrUpdateProfileReceiverData createOrUpdateProfileReceiverData, List<InfoBase> fieldsToCollect, int maxValidationCycles = 5)
        {
            int validateCallCount = 0;
            var continueValidation = true;
            CreateOrUpdateProfileReceiverResponse result = null;
            while (validateCallCount < maxValidationCycles && continueValidation)
            {
                var monitoredListOfFieldsToCollect =
                    FlattenFields(fieldsToCollect).Where(x => x.Required == true).Select(x => x.InfoKey).ToList();
                var newFlds = PopulateFieldValues(fieldsToCollect, monitoredListOfFieldsToCollect);
                
                createOrUpdateProfileReceiverData.CreateOrUpdateProfileReceiverRequest.FieldValues.ForEach(x =>
                {
                    var itemToDelete = newFlds.FirstOrDefault(item => item.InfoKey == x.InfoKey);
                    if (itemToDelete != null)
                        newFlds.Remove(itemToDelete);
                });
                createOrUpdateProfileReceiverData.CreateOrUpdateProfileReceiverRequest.FieldValues.AddRange(newFlds);
                result = _acIntegration.CreateOrUpdateProfileReceiver(createOrUpdateProfileReceiverData);
                continueValidation = result.Payload.FieldsToCollect.Count != 0;
                if (continueValidation)
                    fieldsToCollect = result.Payload.FieldsToCollect;
                ++validateCallCount;
            }
            return result;
        }

        public List<string> GetAllFieldsToCollect(List<InfoBase> listOfFields)
        {
            return FlattenFields(listOfFields).Select(x=>x.InfoKey).ToList();
        }

        private bool ContinueValidationLoop(dynamic lastValidateResponse)
        {
            if ((lastValidateResponse == null || lastValidateResponse.Payload == null) || lastValidateResponse.Payload.ReadyForCommit)
            {
                return false;
            }
            var lastResponseCast = (Response)lastValidateResponse;
            if(lastResponseCast.Errors != null && lastResponseCast.Errors.Any())
            {
                if (!lastResponseCast.Errors.All(x => BusinessErrors.ErrorCodesToContinueValidation.Contains(x.ErrorCategory)))
                {
                    return false;
                }
            }
            return true;
        }
        #region Required Field Values
        private static List<KeyValuePairType> PopulateFieldValues(List<InfoBase> infos, List<string> requiredInfoKeys, List<KeyValuePairType> userFieldValues = null, TestBiller biller = null)
        {
            var keyValuePairs = new List<KeyValuePairType>();
            var fieldsToCollect = FlattenFields(infos).Where(x => (requiredInfoKeys.Contains(x.InfoKey)));
            foreach (var fieldToCollect in fieldsToCollect)
            {
                var infoKey = fieldToCollect.InfoKey;

                if (userFieldValues != null && userFieldValues.Any(x => x.InfoKey == infoKey))
                {
                    var kvp = userFieldValues.First(x => x.InfoKey == infoKey);
                    keyValuePairs.Add(kvp);
                }
                else
                {
                    var kvp = InfoKeyWithValues.GetFieldValue(infoKey);
                    if (biller != null && infoKey == InfoKeyNames.biller_AccountNumber)
                    {
                        kvp.Value = biller.ValidAccountNumber;
                    }
                    keyValuePairs.Add(kvp);

                    var childField = fieldToCollect.ChildFields?.FirstOrDefault(x => x.FieldValue == kvp.Value);
                    if (childField != null)
                    {
                        keyValuePairs.AddRange(PopulateFieldValues(childField.Infos, requiredInfoKeys, userFieldValues));
                    }
                }
            }

            return keyValuePairs;
        }

        private static List<FieldToCollectInfo> FlattenFields(IEnumerable<InfoBase> fields)
        {

            var fieldsToCollect = FindAllInstances<FieldToCollectInfo>(fields);


            //var fieldsToCollect = new List<FieldToCollectInfo>();

            //foreach (var infoBase in fields)
            //{
            //    if (infoBase is CategoryInfo)
            //    {
            //        var categoryInfo = infoBase as CategoryInfo;
            //        fieldsToCollect.AddRange(FlattenFields(categoryInfo.Infos));
            //    }
            //    else if (infoBase is FieldToCollectInfo)
            //    {
            //        fieldsToCollect.Add(infoBase as FieldToCollectInfo);
            //    }
            //}

            return fieldsToCollect;
        }

        public List<KeyValuePairType> GetUserFieldsList(SendReversalData sendReversalData)
        {
            var overriddenListOfFields = new List<KeyValuePairType>();
            if (sendReversalData.SendReversalRequest?.RefundReason != null)
            {
                overriddenListOfFields.Add(new KeyValuePairType
                {
                    InfoKey = InfoKeyNames.send_ReversalReason,
                    Value = sendReversalData.SendReversalRequest?.RefundReason.Identifier
                });
            }
            return overriddenListOfFields;
        }

        private static List<T> FindAllInstances<T>(object value) where T : class
        {

            HashSet<object> exploredObjects = new HashSet<object>();
            List<T> found = new List<T>();

            FindAllInstances(value, exploredObjects, found);

            return found;
        }

        private static void FindAllInstances<T>(object value, HashSet<object> exploredObjects, List<T> found) where T : class
        {
            if (value == null)
            {
                return;
            }

            if (exploredObjects.Contains(value))
            {
                return;
            }

            exploredObjects.Add(value);

            IEnumerable enumerable = value as IEnumerable;

            if (enumerable != null)
            {
                foreach (object item in enumerable)
                {
                    FindAllInstances<T>(item, exploredObjects, found);
                }
            }
            else
            {
                T possibleMatch = value as T;

                if (possibleMatch != null)
                {
                    found.Add(possibleMatch);
                }

                Type type = value.GetType();

                PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetProperty);

                foreach (PropertyInfo property in properties)
                {
                    object propertyValue = property.GetValue(value, null);

                    FindAllInstances<T>(propertyValue, exploredObjects, found);
                }

            }

        }
        #endregion
    }
}