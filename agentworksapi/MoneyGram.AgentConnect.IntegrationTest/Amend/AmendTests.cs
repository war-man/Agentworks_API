using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Amend;
using MoneyGram.AgentConnect.IntegrationTest.Data.Extensions;
using MoneyGram.AgentConnect.IntegrationTest.Data.Requests;
using MoneyGram.AgentConnect.IntegrationTest.Data.Send;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;
using MoneyGram.AgentConnect.IntegrationTest.Operations;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Amend;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Lookup;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Send;
using System;
using System.Linq;

namespace MoneyGram.AgentConnect.IntegrationTest.Amend
{
    [TestClass]
    public class AmendTests
    {
        private AmendOperations amendOperations;
        private SendOperations sendOperations;
        private TransactionLookupOperations tranLookupOperations;

        [TestInitialize]
        public void TestSetup()
        {
            var testRunner = new TestRunner();

            amendOperations = new AmendOperations(testRunner);
            sendOperations = new SendOperations(testRunner);
        }
        #region TransactionLookup Test

        [TestMethod]
        public void Amend_TransactionLookup()
        {

            var sendRequest = new SendRequest
            {
                AgentState = AgentLocation.MN,
                Country = Country.Usa,
                State = State.Ny,
                SendCurr = Currency.Usd,
                AmtRange = AmountRange.NoIdsNoThirdParty,
                FeeType = ItemChoiceType1.amountIncludingFee
            };
            sendRequest.PopulateAgentData(sendRequest.AgentState);
            var sendData = new SendData(sendRequest);
            sendData = sendOperations.SendCompleteForExistingCustomer(sendData);
            var sendCompleteSessionResponse = sendData.CompleteSessionResp;
            Assert.IsFalse(sendData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{sendData.Errors?.Log()}");
            Assert.IsFalse(sendCompleteSessionResponse.Errors.Any(), "Send Failed.");

            //Get the transaction reference number
            var referenceNumber = sendCompleteSessionResponse.Payload.ReferenceNumber;
            //Transaction lookup
            var request = new AmendOperationRequest
            {
                AgentState = AgentLocation.MN,
                ReferenceNumber = referenceNumber
            };
            request.PopulateAgentData(request.AgentState);
            var amendData = new AmendData(request);
            amendData.Set(sendData);
            amendOperations.TransactionLookup(amendData);
            var tranLookupResponse = amendData.TransactionLookup.TransactionLookupResp;

            //ASSERT the resposne.
            Assert.IsFalse(tranLookupResponse.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{tranLookupResponse.Errors?.Log()}");
            Assert.IsTrue(tranLookupResponse.Payload.TransactionStatus == TransactionStatusType.AVAIL);
            Assert.IsFalse(string.IsNullOrEmpty(tranLookupResponse.Payload.MgiSessionID));
        }
        #endregion

        #region Validation Tests
        [TestMethod]
        public void Amend_ShouldSucceedForSendingAgent()
        {
            var sendRequest = new SendRequest
            {
                AgentState = AgentLocation.MN,
                Country = Country.Usa,
                State = State.Ny,
                SendCurr = Currency.Usd,
                AmtRange = AmountRange.NoIdsNoThirdParty,
                FeeType = ItemChoiceType1.amountIncludingFee
            };
            sendRequest.PopulateAgentData(sendRequest.AgentState);
            var sendData = new SendData(sendRequest);
            sendData = sendOperations.SendCompleteForExistingCustomer(sendData);
            var sendCompleteSessionResponse = sendData.CompleteSessionResp;
            Assert.IsFalse(sendData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{sendData.Errors?.Log()}");
            Assert.IsFalse(sendCompleteSessionResponse.Errors.Any(), "Send Failed.");

            //Get the transaction reference number
            string referenceNumber = sendCompleteSessionResponse.Payload?.ReferenceNumber;
            //Amend with single validate
            var request = new AmendOperationRequest
            {
                AgentState = AgentLocation.MN,
                ReferenceNumber = referenceNumber
            };
            request.PopulateAgentData(request.AgentState);

            var amendData = new AmendData(request);
            amendData.Set(sendData);
            amendOperations.SingleValidate(amendData);
            Assert.IsFalse(amendData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{amendData.Errors?.Log()}");
            amendOperations.AmendComplete(amendData);
            var lastAmendValidationResponse = amendData.ValidationResponses.LastOrDefault();

            // ASSERT ALL THE THINGS
            Assert.IsFalse(amendData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{amendData.Errors?.Log()}");
            Assert.IsTrue(lastAmendValidationResponse.Payload.FieldsToCollect.Any());

        }

        [TestMethod]
        public void AmendTransaction_ShouldFailForNonSendingAgent()
        {
            var sendRequest = new SendRequest
            {
                AgentState = AgentLocation.MN,
                Country = Country.Usa,
                State = State.Ny,
                SendCurr = Currency.Usd,
                AmtRange = AmountRange.NoIdsNoThirdParty,
                FeeType = ItemChoiceType1.amountIncludingFee
            };
            sendRequest.PopulateAgentData(sendRequest.AgentState);
            var sendData = new SendData(sendRequest);
            sendData = sendOperations.SendCompleteForExistingCustomer(sendData);
            var sendCompleteSessionResponse = sendData.CompleteSessionResp;
            Assert.IsFalse(sendData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{sendData.Errors?.Log()}");
            Assert.IsFalse(sendCompleteSessionResponse.Errors.Any(), "Send Failed.");

            string referenceNumber = sendCompleteSessionResponse.Payload?.ReferenceNumber;
            var request = new AmendOperationRequest
            {
                AgentState = AgentLocation.NY,
                ReferenceNumber = referenceNumber
            };
            request.PopulateAgentData(request.AgentState);

            var amendData = new AmendData(request);
            amendData.Set(sendData);
            amendOperations.SingleValidate(amendData);
            Assert.IsFalse(amendData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{amendData.Errors?.Log()}");

            amendOperations.AmendComplete(amendData);
            var lastAmendValidationResponse = amendData.ValidationResponses.LastOrDefault();            
            Assert.IsTrue(lastAmendValidationResponse.Errors.Any());
        }
        #endregion

        #region Complete Transaction
        [TestMethod]
        public void AmendTransaction_Complete()
        {
            var sendRequest = new SendRequest
            {
                AgentState = AgentLocation.MN,
                Country = Country.Usa,
                State = State.Ny,
                SendCurr = Currency.Usd,
                AmtRange = AmountRange.NoIdsNoThirdParty,
                FeeType = ItemChoiceType1.amountIncludingFee
            };
            sendRequest.PopulateAgentData(sendRequest.AgentState);

            var sendData = new SendData(sendRequest);
            sendData = sendOperations.SendCompleteForExistingCustomer(sendData);
            var sendCompleteSessionResponse = sendData.CompleteSessionResp;
            Assert.IsFalse(sendData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{sendData.Errors?.Log()}");
            Assert.IsFalse(sendCompleteSessionResponse.Errors.Any(), "Send Failed.");

            string referenceNumber = sendCompleteSessionResponse.Payload?.ReferenceNumber;

            var request = new AmendOperationRequest
            {
                AgentState = AgentLocation.MN,
                ReferenceNumber = referenceNumber
            };
            request.PopulateAgentData(request.AgentState);

            var amendData = new AmendData(request);
            amendData.Set(sendData);
            amendOperations.AmendComplete(amendData);
            Assert.IsFalse(amendData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{amendData.Errors?.Log()}");
            Assert.IsFalse(string.IsNullOrEmpty(amendData.CompleteSessionResp.Payload.ReferenceNumber));
        }
        #endregion
    }
}