using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Lookup;
using MoneyGram.AgentConnect.IntegrationTest.Data.Requests;
using MoneyGram.AgentConnect.IntegrationTest.Data.Send;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;
using MoneyGram.AgentConnect.IntegrationTest.Operations;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Lookup;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Send;
using MoneyGram.AgentConnect.IntegrationTest.Data.Extensions;
using System;
using System.Linq;
using MoneyGram.AgentConnect.IntegrationTest.Data.Recv;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Recv;

namespace MoneyGram.AgentConnect.IntegrationTest.Lookup
{
    [TestClass]
    public class TransactionLookupTests
    {
        private TransactionLookupOperations _tranLookupOperations { get; set; }
        private SendOperations _sendOperations { get; set; }

        private ReceiveOperations _receiveOperations { get; set; }
        private Agents _agents { get; set; }

        [TestInitialize]
        public void TestSetup()
        {
            var testRunner = new TestRunner();
            _tranLookupOperations = new TransactionLookupOperations(testRunner);
            _sendOperations = new SendOperations(testRunner);
            _receiveOperations = new ReceiveOperations(testRunner);
            _agents = new Agents(testRunner.IsTrainingMode);
        }

        [TestMethod]
        public void TransactionLookup_StatusForSendingAgent()
        {
            TransactionLookupData transactionLookupData = new TransactionLookupData();

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
            sendData = _sendOperations.SendCompleteForExistingCustomer(sendData);
            var sendCompleteSessionResponse = sendData.CompleteSessionResp;
            Assert.IsFalse(sendData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{sendData.Errors?.Log()}");
            Assert.IsFalse(sendCompleteSessionResponse.Errors.Any(), "Send Failed.");

            //Get the transaction reference number
            var referenceNumber = sendCompleteSessionResponse.Payload.ReferenceNumber;
            //Transaction lookup
            transactionLookupData = _tranLookupOperations.TransactionLookupForStatus(sendRequest.AgentId, sendRequest.AgentPos, referenceNumber);
            var tranLookupResponse = transactionLookupData.TransactionLookupResp;

            //ASSERT the resposne.
            Assert.IsFalse(tranLookupResponse.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{tranLookupResponse.Errors?.Log()}");
            Assert.IsTrue(tranLookupResponse.Payload.TransactionStatus == TransactionStatusType.AVAIL);
        }

        [TestMethod]
        public void TransactionLookup_StatusForNonSendingAgent()
        {
            TransactionLookupData transactionLookupData = new TransactionLookupData();

            var sendRequest = new SendRequest
            {
                AgentState = AgentLocation.NY,
                Country = Country.Usa,
                State = State.Ny,
                SendCurr = Currency.Usd,
                AmtRange = AmountRange.NoIdsNoThirdParty,
                FeeType = ItemChoiceType1.amountIncludingFee
            };
            sendRequest.PopulateAgentData(sendRequest.AgentState);

            var sendData = new SendData(sendRequest);
            sendData = _sendOperations.SendCompleteForExistingCustomer(sendData);
            var sendCompleteSessionResponse = sendData.CompleteSessionResp;
            Assert.IsFalse(sendData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{sendData.Errors?.Log()}");
            Assert.IsFalse(sendCompleteSessionResponse.Errors.Any(), "Send Failed.");

            //Get the transaction reference number
            var referenceNumber = sendCompleteSessionResponse.Payload.ReferenceNumber;

            //Transaction lookup
            var agent = _agents.GetAgent(AgentLocation.MN);
            transactionLookupData = _tranLookupOperations.TransactionLookupForStatus(agent.AgentId, agent.AgentSequence, referenceNumber);
            var tranLookupResponse = transactionLookupData.TransactionLookupResp;

            //ASSERT the resposne.
            Assert.IsFalse(tranLookupResponse.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{tranLookupResponse.Errors?.Log()}");
            Assert.IsTrue(tranLookupResponse.Payload.TransactionStatus == TransactionStatusType.AVAIL);
        }

        [TestMethod]
        public void TransactionLookup_ReceiveReversalForReceivingAgent()
        {
            TransactionLookupData transactionLookupData = new TransactionLookupData();

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
            sendData = _sendOperations.SendCompleteForExistingCustomer(sendData);
            var sendCompleteSessionResponse = sendData.CompleteSessionResp;
            Assert.IsFalse(sendData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{sendData.Errors?.Log()}");
            Assert.IsFalse(sendCompleteSessionResponse.Errors.Any(), "Send Failed.");

            //Get the transaction reference number
            var referenceNumber = sendCompleteSessionResponse.Payload.ReferenceNumber;

            //Transaction lookup
            var agent = _agents.GetAgent(AgentLocation.NY);
            transactionLookupData = _tranLookupOperations.TransactionLookupForStatus(agent.AgentId, agent.AgentSequence, referenceNumber);
            var tranLookupResponse = transactionLookupData.TransactionLookupResp;

            //ASSERT the resposne.
            Assert.IsFalse(tranLookupResponse.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{tranLookupResponse.Errors?.Log()}");
            Assert.IsTrue(tranLookupResponse.Payload.TransactionStatus == TransactionStatusType.AVAIL);
        }
        //[TestMethod]
        //public void TransactionLookup_Receive()
        //{
        //    TransactionLookupData transactionLookupData = new TransactionLookupData();

        //    var sendRequest = new SendRequest
        //    {
        //        AgentState = AgentLocation.MN,
        //        Country = Country.Usa,
        //        State = State.Ny,
        //        SendCurr = Currency.Usd,
        //        AmtRange = AmountRange.NoIdsNoThirdParty,
        //        FeeType = ItemChoiceType.amountIncludingFee
        //    };
        //    sendRequest.PopulateAgentData(sendRequest.AgentState);

        //    var sendData = new SendData(sendRequest);
        //    sendData = _sendOperations.SendCompleteForExistingCustomer(sendData);
        //    var sendCompleteSessionResponse = sendData.CompleteSessionResp;
        //    Assert.IsFalse(sendData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{sendData.Errors?.Log()}");
        //    Assert.IsFalse(sendCompleteSessionResponse.Errors.Any(), "Send Failed.");

        //    //Get the transaction reference number
        //    var referenceNumber = sendCompleteSessionResponse.Payload.ReferenceNumber;

        //    //Transaction lookup
        //    transactionLookupData = _tranLookupOperations.TransanctionLookupRequestForReceive(AgentLocation.NY, referenceNumber);
        //    var tranLookupResponse = transactionLookupData.TransactionLookupResp;

        //    //ASSERT the resposne.
        //    Assert.IsFalse(tranLookupResponse.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{tranLookupResponse.Errors?.Log()}");
        //    Assert.IsTrue(tranLookupResponse.Payload.TransactionStatus == TransactionStatusType.AVAIL);
        //}

        [TestMethod]
        public void TransactionLookup_StagedReceive()
        {
            TransactionLookupData transactionLookupData = new TransactionLookupData();
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
            sendData = _sendOperations.SendCompleteForNewCustomer(sendData);
            var sendCompleteSessionResponse = sendData.CompleteSessionResp;
            Assert.IsFalse(sendData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{sendData.Errors?.Log()}");
            Assert.IsFalse(sendCompleteSessionResponse.Errors.Any(), "Send Failed.");

            //Get the transaction reference number
            var referenceNumber = sendCompleteSessionResponse.Payload.ReferenceNumber;

            var receiveRequest = new ReceiveRequest
            {
                AgentCountryIsoCode = Country.Usa,
                AgentState = AgentLocation.NY,
                ThirdPartyType = TestThirdPartyType.None
            };
            var receiveData = new ReceiveData(receiveRequest);
            receiveData.Set(sendData);
            receiveData = _receiveOperations.ReceiveCompleteStaged(receiveData);

            //Transaction lookup
            transactionLookupData = _tranLookupOperations.TransactionLookupForStatus(
                receiveData.TransactionLookupRequest.AgentID, receiveData.TransactionLookupRequest.AgentSequence,
                receiveData.CompleteSessionResponse.Payload.ReferenceNumber);
            var tranLookupResponse = transactionLookupData.TransactionLookupResp;
            receiveData.Set(tranLookupResponse);

            //ASSERT the resposne.
            Assert.IsFalse(tranLookupResponse.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{tranLookupResponse.Errors?.Log()}");
            Assert.IsTrue(tranLookupResponse.Payload.TransactionStatus == TransactionStatusType.AVAIL);
        }
    }
}