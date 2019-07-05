using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Extensions;
using MoneyGram.AgentConnect.IntegrationTest.Data.Requests;
using MoneyGram.AgentConnect.IntegrationTest.Data.Send;
using MoneyGram.AgentConnect.IntegrationTest.Data.SendReversal;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;
using MoneyGram.AgentConnect.IntegrationTest.Operations;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Send;
using MoneyGram.AgentConnect.IntegrationTest.Operations.SendReversal;

namespace MoneyGram.AgentConnect.IntegrationTest.SendReversal
{
    [TestClass]
    public class SendReversalTests
    {
        private SendReversalOperations sendReversalOperations;
        private SendOperations sendOperations;

        [TestInitialize]
        public void TestSetup()
        {
            var testRunner = new TestRunner();

            sendReversalOperations = new SendReversalOperations(testRunner);
            sendOperations = new SendOperations(testRunner);
        }
        #region TransactionLookup Test

        [TestMethod]
        public void SendReversal_TransactionLookup()
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
            var sendReversalRequest = new SendReversalRequest
            {
                AgentState = AgentLocation.MN,
                ReferenceNumber = referenceNumber
            };
            sendReversalRequest.PopulateAgentData(sendReversalRequest.AgentState);

            var sendReversalData = new SendReversalData(sendReversalRequest);
            sendReversalData = sendReversalOperations.TransactionLookup(sendReversalData);
            var tranLookupResponse = sendReversalData.TransactionLookup.TransactionLookupResp;

            //ASSERT the resposne.
            Assert.IsFalse(tranLookupResponse.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{tranLookupResponse.Errors?.Log()}");
            Assert.IsTrue(tranLookupResponse.Payload.TransactionStatus == TransactionStatusType.AVAIL);
            Assert.IsFalse(string.IsNullOrEmpty(tranLookupResponse.Payload.MgiSessionID));
        }
        #endregion

        #region Validation Tests
        [TestMethod]
        public void SendReversal_ShouldSucceedForSendingAgent()
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
            string referenceNumber = sendCompleteSessionResponse.Payload.ReferenceNumber;

            //SendReversal with single validate
            var sendReversalRequest = new SendReversalRequest
            {
                AgentState = AgentLocation.MN,
                ReferenceNumber = referenceNumber
            };
            sendReversalRequest.PopulateAgentData(sendReversalRequest.AgentState);

            var sendReversalData = new SendReversalData(sendReversalRequest);
            sendReversalData = sendReversalOperations.SingleValidate(sendReversalData);
            var lastValidationResponse = sendReversalData.ValidationResponses.LastOrDefault();

            // ASSERT ALL THE THINGS
            Assert.IsFalse(sendReversalData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{sendReversalData.Errors?.Log()}");
            Assert.IsTrue(lastValidationResponse.Payload.ReadyForCommit || lastValidationResponse.Payload.FieldsToCollect.Any());
        }

        [TestMethod]
        public void SendReversalTransaction_ShouldFailForNonSendingAgent()
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

            string referenceNumber = sendCompleteSessionResponse.Payload.ReferenceNumber;

            var sendReversalRequest = new SendReversalRequest
            {
                AgentState = AgentLocation.NY,
                ReferenceNumber = referenceNumber
            };
            sendReversalRequest.PopulateAgentData(sendReversalRequest.AgentState);

            var sendReversalData = new SendReversalData(sendReversalRequest);
            sendReversalData = sendReversalOperations.SingleValidate(sendReversalData);
            Assert.IsFalse(sendReversalData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{sendReversalData.Errors?.Log()}");

            var lastValidationResponse = sendReversalData.ValidationResponses.LastOrDefault();
            Assert.IsTrue(lastValidationResponse.Errors.Any());
        }
        #endregion

        #region Complete Transaction
        [TestMethod]
        public void SendReversalTransaction_Complete()
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

            string referenceNumber = sendCompleteSessionResponse.Payload.ReferenceNumber;
            var sendReversalRequest = new SendReversalRequest
            {
                AgentState = AgentLocation.MN,
                ReferenceNumber = referenceNumber,
                RefundReason = new EnumeratedIdentifierInfo { Identifier = "NO_RCV_LOC" },
                RefundFee = true
            };
            sendReversalRequest.PopulateAgentData(sendReversalRequest.AgentState);

            var sendReversalData = new SendReversalData(sendReversalRequest);
            sendReversalData = sendReversalOperations.SendReversalComplete(sendReversalData);
            Assert.IsFalse(sendReversalData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{sendReversalData.Errors?.Log()}");
            Assert.IsFalse(string.IsNullOrEmpty(sendReversalData.CompleteSessionResp.Payload.ReferenceNumber));
        }
        #endregion
    }
}