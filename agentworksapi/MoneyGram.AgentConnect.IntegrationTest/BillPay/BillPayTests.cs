using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.BillPay;
using MoneyGram.AgentConnect.IntegrationTest.Data.Extensions;
using MoneyGram.AgentConnect.IntegrationTest.Data.Requests;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;
using MoneyGram.AgentConnect.IntegrationTest.Operations;
using MoneyGram.AgentConnect.IntegrationTest.Operations.BillPay;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoneyGram.AgentConnect.IntegrationTest.BillPay
{
    [TestClass]
    public class BillPayTests
    {
        private BillPayOperations _billPayOperations;

        [TestInitialize]
        public void TestSetup()
        {
            _billPayOperations = new BillPayOperations(new TestRunner());
        }


        #region BillerSearch

        [TestMethod]
        public void BillPay_BillerSearchByName()
        {
            var request = new BillPayOperationRequest
            {
                AgentState = AgentLocation.MN,
                Biller = Billers.Ford,
                ThirdPartyType = TestThirdPartyType.None
            };
            request.PopulateAgentData(request.AgentState);

            var billPayData = new BillPayData(request);
            _billPayOperations.BillerSearchByName(billPayData);

            Assert.IsTrue(billPayData.BillerSearchResponse.Payload.BillerInfo.Any());
        }

        [TestMethod]
        public void BillPay_BillerSearchByReceiveCode()
        {
            var request = new BillPayOperationRequest
            {
                AgentState = AgentLocation.MN,
                Biller = Billers.Ford,
                ThirdPartyType = TestThirdPartyType.None
            };
            request.PopulateAgentData(request.AgentState);

            var billPayData = new BillPayData(request);
            _billPayOperations.BillerSearchByCode(billPayData);

            Assert.IsTrue(billPayData.BillerSearchResponse.Payload.BillerInfo.Any());
        }

        #endregion


        #region CustomerSearch

        [TestMethod]
        public void BillPay_ConsumerHistoryLookup_ConsumerExists()
        {
            var request = new BillPayOperationRequest
            {
                AgentState = AgentLocation.MN,
                ThirdPartyType = TestThirdPartyType.None
            };
            request.PopulateAgentData(request.AgentState);

            var billPayData = new BillPayData(request);
            _billPayOperations.ConsumerHistoryLookupForExistingCustomer(billPayData);

            Assert.IsTrue(billPayData.ConsumerHistoryLookupResponse.Payload.SenderInfos.SenderInfo.Any());
        }

        [TestMethod]
        public void BillPay_ConsumerHistoryLookup_ConsumerDoesNotExist()
        {
            var request = new BillPayOperationRequest
            {
                AgentState = AgentLocation.MN,
                ThirdPartyType = TestThirdPartyType.None
            };
            request.PopulateAgentData(request.AgentState);

            var billPayData = new BillPayData(request);
            _billPayOperations.ConsumerHistoryLookupForNonExistingCustomer(billPayData);

            Assert.IsNull(billPayData.ConsumerHistoryLookupResponse.Payload.SenderInfos);
        }

        #endregion

        #region FeeLookup

        [TestMethod]
        public void BillPay_FeeLookup()
        {
            var request = new BillPayOperationRequest
            {
                AgentState = AgentLocation.MN,
                Biller = Billers.HubbardAttorney,
                AmtRange = AmountRange.NoIdsNoThirdParty,
                ThirdPartyType = TestThirdPartyType.None
            };
            request.PopulateAgentData(request.AgentState);

            var billPayData = new BillPayData(request);
            _billPayOperations.FeeLookupForReceiveCode(billPayData);

            Assert.IsTrue(billPayData.FeeLookup.FeeLookupResp.Payload.FeeInfos.Any());
        }

        #endregion

        #region Validation

        [TestMethod]
        public void BillPay_ValidateShouldReturnFieldsToCollectWithNoIdsOrThirdParty()
        {
            var request = new BillPayOperationRequest
            {
                AgentState = AgentLocation.MN,
                Biller = Billers.HubbardAttorney,
                AmtRange = AmountRange.NoIdsNoThirdParty,
                ThirdPartyType = TestThirdPartyType.None
            };
            request.PopulateAgentData(request.AgentState);
            var billPayData = new BillPayData(request);
            _billPayOperations.SingleValidate(billPayData);

            var validationResponse = billPayData.BillPayValidationResponseList.First();

            Assert.IsTrue(validationResponse.Payload.FieldsToCollect.Any());
        }

        [TestMethod]
        public void BillPay_ValidateShouldReturnFieldsToCollectWithFirstId()
        {
            var request = new BillPayOperationRequest
            {
                AgentState = AgentLocation.MN,
                Biller = Billers.HubbardAttorney,
                AmtRange = AmountRange.SingleId,
                ThirdPartyType = TestThirdPartyType.None
            };
            request.PopulateAgentData(request.AgentState);
            var billPayData = new BillPayData(request);
            _billPayOperations.SingleValidate(billPayData);

            var validationResponse = billPayData.BillPayValidationResponseList.First();

            var containsFirstIdCategory = ContainsCategory(validationResponse.Payload.FieldsToCollect,
                "billPaySenderIdentificationPrimarySet");

            Assert.IsTrue(containsFirstIdCategory, "Ensure that profile editor limits for bill pay are configured properly in the current environment");
        }

        [TestMethod]
        public void BillPay_ValidateShouldReturnFieldsToCollectWithSecondId()
        {
            var request = new BillPayOperationRequest
            {
                AgentState = AgentLocation.MN,
                Biller = Billers.HubbardAttorney,
                AmtRange = AmountRange.TwoIds,
                ThirdPartyType = TestThirdPartyType.None
            };
            request.PopulateAgentData(request.AgentState);
            var billPayData = new BillPayData(request);
            _billPayOperations.SingleValidate(billPayData);

            var validationResponse = billPayData.BillPayValidationResponseList.First();

            var containsSecondIdCategory = ContainsCategory(validationResponse.Payload.FieldsToCollect,
                "billPaySenderComplianceSecondarySet");

            Assert.IsTrue(containsSecondIdCategory, "Ensure that profile editor limits for bill pay are configured properly in the current environment");
        }

        [TestMethod]
        public void BillPay_ValidateShouldReturnFieldsToCollectWithThirdPartyType()
        {
            var request = new BillPayOperationRequest
            {
                AgentState = AgentLocation.MN,
                Biller = Billers.HubbardAttorney,
                AmtRange = AmountRange.ThirdParty,
                ThirdPartyType = TestThirdPartyType.None
            };
            request.PopulateAgentData(request.AgentState);
            var billPayData = new BillPayData(request);
            _billPayOperations.SingleValidate(billPayData);

            var validationResponse = billPayData.BillPayValidationResponseList.First();

            var containsThirdPartyCategory = ContainsCategory(validationResponse.Payload.FieldsToCollect,
                "billPayTPSenderInformationSet");

            Assert.IsTrue(containsThirdPartyCategory, "Ensure that profile editor limits for bill pay are configured properly in the current environment");
        }

        [TestMethod]
        public void BillPay_ValidationUntilReadyForCommit()
        {
            var request = new BillPayOperationRequest
            {
                AgentState = AgentLocation.MN,
                Biller = Billers.HubbardAttorney,
                AmtRange = AmountRange.NoIdsNoThirdParty,
                ThirdPartyType = TestThirdPartyType.None
            };
            request.PopulateAgentData(request.AgentState);
            var billPayData = new BillPayData(request);
            _billPayOperations.ValidateUntilReadyForCommit(billPayData);

            var validationResponse = billPayData.BillPayValidationResponseList.Last();

            Assert.IsTrue(validationResponse.Payload.ReadyForCommit);
        }

        private bool ContainsCategory(List<InfoBase> infos, string categoryKey)
        {
            return infos.OfType<CategoryInfo>()
                .Any(categoryInfo => categoryInfo.InfoKey == categoryKey || ContainsCategory(categoryInfo.Infos, categoryKey));
        }

        #endregion

        #region Commit

        [TestMethod]
        public void BillPay_CompleteTransactionWithNoIdsOrThirdParty()
        {
            var request = new BillPayOperationRequest
            {
                AgentState = AgentLocation.MN,
                Biller = Billers.HubbardAttorney,
                AmtRange = AmountRange.NoIdsNoThirdParty,
                ThirdPartyType = TestThirdPartyType.None
            };
            request.PopulateAgentData(request.AgentState);
            var billPayData = new BillPayData(request);
            _billPayOperations.CompleteSession(billPayData);

            var completeSessionResponse = billPayData.CompleteSessionResponse;

            Assert.IsFalse(string.IsNullOrEmpty(completeSessionResponse.Payload.ReferenceNumber));
        }

        [TestMethod]
        public void BillPay_CompleteTransactionWithPrimaryId()
        {
            var request = new BillPayOperationRequest
            {
                AgentState = AgentLocation.MN,
                Biller = Billers.HubbardAttorney,
                AmtRange = AmountRange.SingleId,
                ThirdPartyType = TestThirdPartyType.None
            };
            request.PopulateAgentData(request.AgentState);
            var billPayData = new BillPayData(request);
            _billPayOperations.CompleteSession(billPayData);

            var completeSessionResponse = billPayData.CompleteSessionResponse;

            Assert.IsFalse(string.IsNullOrEmpty(completeSessionResponse.Payload.ReferenceNumber));
        }

        [TestMethod]
        public void BillPay_CompleteTransactionWithSecondaryId()
        {
            var request = new BillPayOperationRequest
            {
                AgentState = AgentLocation.MN,
                Biller = Billers.HubbardAttorney,
                AmtRange = AmountRange.TwoIds,
                ThirdPartyType = TestThirdPartyType.None
            };
            request.PopulateAgentData(request.AgentState);
            var billPayData = new BillPayData(request);
            _billPayOperations.CompleteSession(billPayData);

            var completeSessionResponse = billPayData.CompleteSessionResponse;

            Assert.IsFalse(string.IsNullOrEmpty(completeSessionResponse.Payload.ReferenceNumber));
        }

        [TestMethod]
        public void BillPay_CompleteTransactionWithThirdPartyOrg()
        {
            var request = new BillPayOperationRequest
            {
                AgentState = AgentLocation.MN,
                Biller = Billers.HubbardAttorney,
                AmtRange = AmountRange.ThirdParty,
                ThirdPartyType = TestThirdPartyType.Org
            };
            request.PopulateAgentData(request.AgentState);
            var billPayData = new BillPayData(request);
            _billPayOperations.CompleteSession(billPayData);

            var completeSessionResponse = billPayData.CompleteSessionResponse;

            Assert.IsFalse(string.IsNullOrEmpty(completeSessionResponse.Payload.ReferenceNumber));
        }

        [TestMethod]
        public void BillPay_CompleteTransactionWithThirdPartyPerson()
        {
            var request = new BillPayOperationRequest
            {
                AgentState = AgentLocation.MN,
                Biller = Billers.HubbardAttorney,
                AmtRange = AmountRange.ThirdParty,
                ThirdPartyType = TestThirdPartyType.Person
            };
            request.PopulateAgentData(request.AgentState);
            var billPayData = new BillPayData(request);
            _billPayOperations.CompleteSession(billPayData);

            var completeSessionResponse = billPayData.CompleteSessionResponse;

            Assert.IsFalse(string.IsNullOrEmpty(completeSessionResponse.Payload.ReferenceNumber));
        }

        /// <summary>
        /// Staged BillPay Low amount
        /// </summary>
        [TestMethod]
        public void StagingBillPay_LowAmount_MN()
        {
            var request = new BillPayOperationRequest
            {
                AgentState = AgentLocation.MN,
                Biller = Billers.HubbardAttorney,
                AmtRange = AmountRange.UnderOneHundred,
                ThirdPartyType = TestThirdPartyType.None
            };
            request.PopulateAgentData(request.AgentState);
            var billPayData = new BillPayData(request);
            _billPayOperations.CompleteStagedSession(billPayData);

            var completeSessionResponse = billPayData.CompleteSessionResponse;
            Assert.IsFalse(billPayData.BillPayValidationResponseList.First().Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{billPayData.BillPayValidationResponseList.First().Errors?.Log()}");
            Assert.IsFalse(completeSessionResponse.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{completeSessionResponse.Errors?.Log()}");
        }
        /// <summary>
        /// Staged BillPay Low amount
        /// </summary>
        [TestMethod]
        public void StagingBillPay_LowAmount_NY()
        {
            var request = new BillPayOperationRequest
            {
                AgentState = AgentLocation.MN,
                Biller = Billers.HubbardAttorney,
                AmtRange = AmountRange.UnderOneHundred,
                ThirdPartyType = TestThirdPartyType.None
            };
            request.PopulateAgentData(request.AgentState);
            var billPayData = new BillPayData(request);
            _billPayOperations.CompleteStagedSession(billPayData);

            var completeSessionResponse = billPayData.CompleteSessionResponse;
            Assert.IsFalse(billPayData.BillPayValidationResponseList.First().Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{billPayData.BillPayValidationResponseList.First().Errors?.Log()}");
            Assert.IsFalse(completeSessionResponse.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{completeSessionResponse.Errors?.Log()}");
        }

        /// <summary>
        /// Staged BillPay Low amount
        /// </summary>
        [TestMethod]
        [Ignore] //there is no Oklahoma agent to use in D2
        public void StagingBillPay_LowAmount_OK()
        {
            var request = new BillPayOperationRequest
            {
                AgentState = AgentLocation.OK,
                Biller = Billers.HubbardAttorney,
                AmtRange = AmountRange.UnderOneHundred,
                ThirdPartyType = TestThirdPartyType.None
            };
            request.PopulateAgentData(request.AgentState);
            var billPayData = new BillPayData(request);
            _billPayOperations.CompleteStagedSession(billPayData);

            var completeSessionResponse = billPayData.CompleteSessionResponse;
            Assert.IsFalse(billPayData.BillPayValidationResponseList.First().Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{billPayData.BillPayValidationResponseList.First().Errors?.Log()}");
            Assert.IsFalse(completeSessionResponse.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{completeSessionResponse.Errors?.Log()}");
        }

        /// <summary>
        /// Staged BillPay
        /// </summary>
        [TestMethod]
        public void StagingBillPay_WithNoIdsOrThirdParty()
        {
            var request = new BillPayOperationRequest
            {
                AgentState = AgentLocation.MN,
                Biller = Billers.HubbardAttorney,
                AmtRange = AmountRange.NoIdsNoThirdParty,
                ThirdPartyType = TestThirdPartyType.None
            };
            request.PopulateAgentData(request.AgentState);
            var billPayData = new BillPayData(request);
            _billPayOperations.CompleteStagedSession(billPayData);

            var completeSessionResponse = billPayData.CompleteSessionResponse;
            Assert.IsFalse(billPayData.BillPayValidationResponseList.First().Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{billPayData.BillPayValidationResponseList.First().Errors?.Log()}");
            Assert.IsFalse(completeSessionResponse.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{completeSessionResponse.Errors?.Log()}");
        }

        /// <summary>
        /// Staged BillPay Third Party
        /// </summary>
        [TestMethod]
        public void StagingBillPay_ThirdParty()
        {
            var request = new BillPayOperationRequest
            {
                AgentState = AgentLocation.MN,
                Biller = Billers.HubbardAttorney,
                AmtRange = AmountRange.ThirdParty,
                ThirdPartyType = TestThirdPartyType.Person
            };
            request.PopulateAgentData(request.AgentState);
            var billPayData = new BillPayData(request);
            _billPayOperations.CompleteStagedSession(billPayData);

            var completeSessionResponse = billPayData.CompleteSessionResponse;
            Assert.IsFalse(billPayData.BillPayValidationResponseList.First().Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{billPayData.BillPayValidationResponseList.First().Errors?.Log()}");
            Assert.IsFalse(completeSessionResponse.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{completeSessionResponse.Errors?.Log()}");
        }

        /// <summary>
        /// Staged BillPay Single ID
        /// </summary>
        [TestMethod]
        public void StagingBillPay_SingleId()
        {
            var request = new BillPayOperationRequest
            {
                AgentState = AgentLocation.MN,
                Biller = Billers.HubbardAttorney,
                AmtRange = AmountRange.SingleId,
                ThirdPartyType = TestThirdPartyType.None
            };
            request.PopulateAgentData(request.AgentState);
            var billPayData = new BillPayData(request);
            _billPayOperations.CompleteStagedSession(billPayData);

            var completeSessionResponse = billPayData.CompleteSessionResponse;
            Assert.IsFalse(billPayData.BillPayValidationResponseList.First().Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{billPayData.BillPayValidationResponseList.First().Errors?.Log()}");
            Assert.IsFalse(completeSessionResponse.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{completeSessionResponse.Errors?.Log()}");
        }

        /// <summary>
        /// Staged BillPay Two IDs
        /// </summary>
        [TestMethod]
        public void StagingBillPay_TwoId()
        {
            var request = new BillPayOperationRequest
            {
                AgentState = AgentLocation.MN,
                Biller = Billers.HubbardAttorney,
                AmtRange = AmountRange.TwoIds,
                ThirdPartyType = TestThirdPartyType.None
            };
            request.PopulateAgentData(request.AgentState);
            var billPayData = new BillPayData(request);
            _billPayOperations.CompleteStagedSession(billPayData);

            var completeSessionResponse = billPayData.CompleteSessionResponse;
            Assert.IsFalse(billPayData.BillPayValidationResponseList.First().Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{billPayData.BillPayValidationResponseList.First().Errors?.Log()}");
            Assert.IsFalse(completeSessionResponse.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{completeSessionResponse.Errors?.Log()}");
        }
        #endregion
    }
}