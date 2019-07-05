using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Requests;
using MoneyGram.AgentConnect.IntegrationTest.Data.Send;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;
using MoneyGram.AgentConnect.IntegrationTest.Operations;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Send;
using MoneyGram.AgentConnect.IntegrationTest.Data.Extensions;

namespace MoneyGram.AgentConnect.IntegrationTest.Send
{
    [TestClass]
    public class SendTests
    {
        private SendOperations _sendOperations { get; set; }

        [TestInitialize]
        public void TestSetup()
        {
            _sendOperations = new SendOperations(new TestRunner());
        }

        #region Validation

        [TestMethod]
        public void Send_ValidateShouldReturnFieldsToCollectWithNoIdsOrThirdParty()
        {
            var sendRequest = new SendRequest
            {
                AgentState = AgentLocation.MN,
                Country = Country.Usa,
                State = State.Ny,
                SendCurr = Currency.Usd,
                AmtRange = AmountRange.NoIdsNoThirdParty,
                FeeType = ItemChoiceType1.amountExcludingFee,
                ServiceOption = ServiceOptionType.WillCall,
                ThirdPartyType = TestThirdPartyType.None
            };

            sendRequest.PopulateAgentData(sendRequest.AgentState);
            var sendData = new SendData(sendRequest);
            sendData = _sendOperations.SingleValidateForExistingSender(sendData);
            var validationResponse = sendData.SendValidationResps?.FirstOrDefault();

            // ASSERT ALL THE THINGS
            Assert.IsFalse(sendData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{sendData.Errors?.Log()}");
            Assert.IsTrue(validationResponse.Payload.FieldsToCollect.Any());
        }

        [TestMethod]
        public void Send_ValidateShouldReturnFieldsToCollectWithFirstId()
        {
            var sendRequest = new SendRequest
            {
                AgentState = AgentLocation.MN,
                Country = Country.Usa,
                State = State.Ny,
                SendCurr = Currency.Usd,
                AmtRange = AmountRange.SingleId,
                FeeType = ItemChoiceType1.amountExcludingFee,
                ServiceOption = ServiceOptionType.WillCall,
                ThirdPartyType = TestThirdPartyType.None
            };
            sendRequest.PopulateAgentData(sendRequest.AgentState);

            var sendData = new SendData(sendRequest);
            sendData = _sendOperations.SingleValidateForExistingSender(sendData);
            Assert.IsFalse(sendData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{sendData.Errors?.Log()}");

            var validationResponse = sendData.SendValidationResps.FirstOrDefault();
            var containsFirstIdCategory = ContainsCategory(validationResponse?.Payload.FieldsToCollect,
                InfoKeyCategories.mtSenderIdentificationPrimarySet);
            Assert.IsTrue(containsFirstIdCategory, "Ensure that profile editor limits for send are configured properly in the current environment");
        }

        [TestMethod]
        public void Send_ValidateShouldReturnFieldsToCollectWithSecondId()
        {
            var sendRequest = new SendRequest
            {
                AgentState = AgentLocation.MN,
                Country = Country.Usa,
                State = State.Ny,
                SendCurr = Currency.Usd,
                AmtRange = AmountRange.TwoIds,
                FeeType = ItemChoiceType1.amountExcludingFee,
                ServiceOption = ServiceOptionType.WillCall,
                ThirdPartyType = TestThirdPartyType.None
            };
            sendRequest.PopulateAgentData(sendRequest.AgentState);

            var sendData = new SendData(sendRequest);
            sendData = _sendOperations.SingleValidateForExistingSender(sendData);
            Assert.IsFalse(sendData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{sendData.Errors?.Log()}");

            var validationResponse = sendData.SendValidationResps.FirstOrDefault();
            var containsSecondIdCategory = ContainsCategory(validationResponse?.Payload.FieldsToCollect,
                InfoKeyCategories.mtSenderIdentificationSecondarySet);
            Assert.IsTrue(containsSecondIdCategory, "Ensure that profile editor limits for send are configured properly in the current environment");
        }

        [TestMethod]
        public void Send_ValidateShouldReturnFieldsToCollectWithThirdPartyType()
        {
            var sendRequest = new SendRequest
            {
                AgentState = AgentLocation.MN,
                Country = Country.Usa,
                State = State.Ny,
                SendCurr = Currency.Usd,
                AmtRange = AmountRange.ThirdParty,
                FeeType = ItemChoiceType1.amountExcludingFee,
                ServiceOption = ServiceOptionType.WillCall,
                ThirdPartyType = TestThirdPartyType.Person
            };
            sendRequest.PopulateAgentData(sendRequest.AgentState);

            var sendData = new SendData(sendRequest);
            sendData = _sendOperations.SingleValidateForExistingSender(sendData);
            Assert.IsFalse(sendData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{sendData.Errors?.Log()}");

            var validationResponse = sendData.SendValidationResps.FirstOrDefault();
            //TODO: verify with the proper category name... 
            var containsThirdPartyCategory = ContainsCategory(validationResponse?.Payload.FieldsToCollect,
                InfoKeyCategories.mtTPSenderInformationSet);
            Assert.IsTrue(containsThirdPartyCategory, "Ensure that profile editor limits for send are configured properly in the current environment");
        }

        [TestMethod]
        public void Send_ValidationUntilReadyForCommit()
        {
            var sendRequest = new SendRequest
            {
                AgentState = AgentLocation.MN,
                Country = Country.Usa,
                State = State.Ny,
                SendCurr = Currency.Usd,
                AmtRange = AmountRange.NoIdsNoThirdParty,
                FeeType = ItemChoiceType1.amountExcludingFee,
                ServiceOption = ServiceOptionType.WillCall
            };
            sendRequest.PopulateAgentData(sendRequest.AgentState);

            var sendData = new SendData(sendRequest);
            sendData = _sendOperations.ValidateUntilReadyForCommit(sendData);
            var validationResponse = sendData.SendValidationResps.LastOrDefault();

            Assert.IsFalse(sendData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{sendData.Errors?.Log()}");
            Assert.IsTrue(validationResponse.Payload.ReadyForCommit);
        }

        [TestMethod]
        public void Send_Complete()
        {
            var sendRequest = new SendRequest
            {
                AgentState = AgentLocation.MN,
                Country = Country.Usa,
                State = State.Ny,
                SendCurr = Currency.Usd,
                AmtRange = AmountRange.NoIdsNoThirdParty,
                FeeType = ItemChoiceType1.amountExcludingFee,
                ServiceOption = ServiceOptionType.WillCall
            };
            sendRequest.PopulateAgentData(sendRequest.AgentState);

            var sendData = new SendData(sendRequest);
            sendData = _sendOperations.SendCompleteForExistingCustomer(sendData);

            Assert.IsFalse(sendData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{sendData.Errors?.Log()}");
            Assert.IsTrue(sendData.CompleteSessionResp.Payload != null && !string.IsNullOrWhiteSpace(sendData.CompleteSessionResp.Payload.ReferenceNumber));
        }

        [TestMethod]
        public void Send_Complete_LowAmount()
        {
            var sendRequest = new SendRequest
            {
                AgentState = AgentLocation.MN,
                Country = Country.Usa,
                State = State.Ny,
                SendCurr = Currency.Usd,
                AmtRange = AmountRange.UnderOneHundred,
                FeeType = ItemChoiceType1.amountExcludingFee,
                ServiceOption = ServiceOptionType.WillCall
            };
            sendRequest.PopulateAgentData(sendRequest.AgentState);

            var sendData = new SendData(sendRequest);
            sendData = _sendOperations.SendCompleteForExistingCustomer(sendData);

            Assert.IsFalse(sendData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{sendData.Errors?.Log()}");
            Assert.IsTrue(sendData.CompleteSessionResp.Payload != null && !string.IsNullOrWhiteSpace(sendData.CompleteSessionResp.Payload.ReferenceNumber));
        }

        [TestMethod]
        public void Send_Complete_LowAmount_NewCustomer()
        {
            var sendRequest = new SendRequest
            {
                AgentState = AgentLocation.MN,
                Country = Country.Usa,
                State = State.Ny,
                SendCurr = Currency.Usd,
                AmtRange = AmountRange.UnderOneHundred,
                FeeType = ItemChoiceType1.amountExcludingFee,
                ServiceOption = ServiceOptionType.WillCall
            };
            sendRequest.PopulateAgentData(sendRequest.AgentState);

            var sendData = new SendData(sendRequest);
            sendData = _sendOperations.SendCompleteForNewCustomer(sendData);

            Assert.IsFalse(sendData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{sendData.Errors?.Log()}");
            Assert.IsTrue(sendData.CompleteSessionResp.Payload != null && !string.IsNullOrWhiteSpace(sendData.CompleteSessionResp.Payload.ReferenceNumber));
        }

        /// <summary>
        ///     Send complete third party with a third party type of person
        /// </summary>
        [TestMethod]
        public void Send_Complete_ThirdParty_Person()
        {
            var sendRequest = new SendRequest
            {
                AgentState = AgentLocation.MN,
                Country = Country.Usa,
                State = State.Ny,
                SendCurr = Currency.Usd,
                AmtRange = AmountRange.ThirdParty,
                FeeType = ItemChoiceType1.amountExcludingFee,
                ServiceOption = ServiceOptionType.WillCall,
                ThirdPartyType = TestThirdPartyType.Person
            };
            sendRequest.PopulateAgentData(sendRequest.AgentState);

            var sendData = new SendData(sendRequest);
            sendData = _sendOperations.SendCompleteForThirdParty(sendData);

            Assert.IsFalse(sendData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{sendData.Errors?.Log()}");
        }

        private bool ContainsCategory(List<InfoBase> infos, string categoryKey)
        {
            return infos.OfType<CategoryInfo>()
                .Any(categoryInfo => categoryInfo.InfoKey.ToLower() == categoryKey.ToLower() || ContainsCategory(categoryInfo.Infos, categoryKey));
        }

        #endregion
    }
}