using System.Linq;
using MoneyGram.AgentConnect.DomainModel.Exceptions;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.BillPay;
using MoneyGram.AgentConnect.IntegrationTest.Data.Common;
using MoneyGram.AgentConnect.IntegrationTest.Data.Requests;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Common;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Helpers;

namespace MoneyGram.AgentConnect.IntegrationTest.Operations.BillPay
{
    public class BillPayOperations
    {
        private TestAgentConnectIntegration _acIntegration { get; }
        private ValidationOperations _validationOperations;
        public BillPayOperations(TestRunner testRunner)
        {
            _acIntegration = new TestAgentConnectIntegration(testRunner);
            _validationOperations = new ValidationOperations(testRunner);
        }

        #region BillerSearch
        public BillPayData BillerSearchByName(BillPayData billPayData)
        {
            billPayData.Set(BillPayBillerSearchRequestFactory.BillerSearchRequestByNameForEP(billPayData.BillPayRequest.Biller.Name));
            billPayData.Set(_acIntegration.BillerSearch(billPayData.BillPayRequest.AgentId, billPayData.BillPayRequest.AgentPos, billPayData.BillerSearchRequest));

            return billPayData;
        }

        public BillPayData BillerSearchByCode(BillPayData billPayData)
        {
            billPayData.Set(BillPayBillerSearchRequestFactory.BillerSearchRequestByCodeForEP(billPayData.BillPayRequest.Biller.Code));
	        billPayData.Set(_acIntegration.BillerSearch(billPayData.BillPayRequest.AgentId, billPayData.BillPayRequest.AgentPos, billPayData.BillerSearchRequest));

            return billPayData;
        }
        #endregion

        #region ConsumerLookup
        public BillPayData ConsumerHistoryLookupForExistingCustomer(BillPayData billPayData)
        {
            billPayData.Set(ConsumerHistoryLookupRequestFactory.ConsumerHistoryLookupRequestExisting);
            billPayData.ConsumerHistoryLookupRequest.MgiSessionType = SessionType.BP;
            billPayData.Set(_acIntegration.ConsumerHistoryLookup(billPayData.BillPayRequest.AgentId, billPayData.BillPayRequest.AgentPos, true));

            return billPayData;
        }

        public BillPayData ConsumerHistoryLookupForNonExistingCustomer(BillPayData billPayData)
        {
            var consumerHistoryLookupRequest = ConsumerHistoryLookupRequestFactory.ConsumerHistoryLookupRequestNonExisting;
            consumerHistoryLookupRequest.AgentID = billPayData.BillPayRequest.AgentId;
            consumerHistoryLookupRequest.AgentSequence = billPayData.BillPayRequest.AgentPos;
            billPayData.Set(consumerHistoryLookupRequest);
            billPayData.ConsumerHistoryLookupRequest.MgiSessionType = SessionType.BP;
            billPayData.Set(_acIntegration.ConsumerHistoryLookup(billPayData.ConsumerHistoryLookupRequest));

            return billPayData;
        }
        #endregion

        #region FeeLookup

        public BillPayData FeeLookupForBillerName(BillPayData billPayData)
        {
            BillerSearchByName(billPayData);

            return FeeLookup(billPayData);
        }

        public BillPayData FeeLookupForReceiveCode(BillPayData billPayData)
        {
            BillerSearchByCode(billPayData);

            return FeeLookup(billPayData);
        }

        private BillPayData FeeLookup(BillPayData billPayData)
        {
            var selectedBiller = billPayData.BillerSearchResponse.Payload.BillerInfo.First();
            var feeLookupReq = BillPayFeeLookupRequestFactory.FeeLookupForEp(selectedBiller.ReceiveCode, selectedBiller.ReceiveAgentID, billPayData.BillPayRequest.AmtRange, billPayData.BillPayRequest.Amount);
            feeLookupReq.AgentID = billPayData.BillPayRequest.AgentId;
            feeLookupReq.AgentSequence = billPayData.BillPayRequest.AgentPos;
            billPayData.FeeLookup.Set(feeLookupReq);
            billPayData.FeeLookup.Set(_acIntegration.FeeLookup(billPayData.FeeLookup.FeeLookupReq));

            return billPayData;
        }

        #endregion

        #region Validation

        public BillPayData SingleValidate(BillPayData billPayData)
        {
            BillerSearchByCode(billPayData);

            FeeLookup(billPayData);

            InitialValidation(billPayData);

            return billPayData;
        }

        private BillPayData InitialValidation(BillPayData billPayData)
        {
            var feeInfo = billPayData.FeeLookup.FeeLookupResp.Payload.FeeInfos.FirstOrDefault();
            var billerInfo = billPayData.BillerSearchResponse.Payload.BillerInfo.FirstOrDefault();

            var validationRequest = BillPayValidationRequestFactory.InitialValidationForEp(feeInfo, billerInfo);
            if (billPayData.BillPayRequest.Staging)
            {
                validationRequest.TransactionStaging = true;
            }
            validationRequest.AgentID = billPayData.BillPayRequest.AgentId;
            validationRequest.AgentSequence = billPayData.BillPayRequest.AgentPos;
            var validationResponse = _acIntegration.BPValidation(validationRequest);

            billPayData.Set(validationRequest);
            billPayData.Set(validationResponse);

            return billPayData;
        }

        public BillPayData ValidateUntilReadyForCommit(BillPayData billPayData)
        {
            try
            {
                BillerSearchByCode(billPayData);

                FeeLookup(billPayData);

                if (DataErrorHandler.CheckForNestedErrors(billPayData).Any())
                {
                    return billPayData;
                }
                // Initial Validate
                _validationOperations.BillPayValidate(billPayData, billPayData.BillPayRequest.Biller, TestThirdPartyType.None, staging: billPayData.BillPayRequest.Staging);
                // Final Validate(s)
                _validationOperations.BillPayValidateUntilReadyForCommit(billPayData, billPayData.BillPayRequest.Biller, billPayData.BillPayRequest.Staging, TestThirdPartyType.None);
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
            return billPayData;
        }

        #endregion

        #region
        public BillPayData CompleteStagedSession(BillPayData billPayData)
        {
            try
            {
                BillerSearchByCode(billPayData);

                FeeLookup(billPayData);

                if (DataErrorHandler.CheckForNestedErrors(billPayData).Any())
                {
                    return billPayData;
                }
                // Initial Validate
                _validationOperations.BillPayValidate(billPayData, billPayData.BillPayRequest.Biller, billPayData.BillPayRequest.ThirdPartyType, staging: true);
                // Final Validate(s)
                _validationOperations.BillPayValidateUntilReadyForCommit(billPayData, billPayData.BillPayRequest.Biller, true, billPayData.BillPayRequest.ThirdPartyType);

                CompleteStaged(billPayData);
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
            return billPayData;
        }
        public BillPayData CompleteSession(BillPayData billPayData)
        {
            try
            {
                BillerSearchByCode(billPayData);

                FeeLookup(billPayData);

                if (DataErrorHandler.CheckForNestedErrors(billPayData).Any())
                {
                    return billPayData;
                }
                // Initial Validate
                _validationOperations.BillPayValidate(billPayData, billPayData.BillPayRequest.Biller, billPayData.BillPayRequest.ThirdPartyType);
                // Final Validate(s)
                _validationOperations.BillPayValidateUntilReadyForCommit(billPayData, billPayData.BillPayRequest.Biller, billPayData.BillPayRequest.ThirdPartyType);

                var mgiSessionId = billPayData.BillPayValidationResponseList.Last().Payload.MgiSessionID;
                var completeSessionRequest = CompleteSessionRequestFactory.CompleteBillPayRequest(mgiSessionId);
                completeSessionRequest.AgentID = billPayData.BillPayRequest.AgentId;
                completeSessionRequest.AgentSequence = billPayData.BillPayRequest.AgentPos;
                var completeSessionResponse = _acIntegration.CompleteSession(completeSessionRequest);

                billPayData.Set(completeSessionRequest);
                billPayData.Set(completeSessionResponse);
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
            return billPayData;
        }
        private BillPayData CompleteStaged(BillPayData billPayData)
        {
            var mgiSessionId = billPayData.BillPayValidationResponseList.Last().Payload.MgiSessionID;
            var completeSessionRequest = CompleteSessionRequestFactory.CompleteBillPayRequest(mgiSessionId);
            completeSessionRequest.AgentID = billPayData.BillPayRequest.AgentId;
            completeSessionRequest.AgentSequence = billPayData.BillPayRequest.AgentPos;
            completeSessionRequest.Commit = false;
            var completeSessionResponse = _acIntegration.CompleteSession(completeSessionRequest);

            billPayData.Set(completeSessionRequest);
            billPayData.Set(completeSessionResponse);

            return billPayData;
        }

        #endregion
    }
}