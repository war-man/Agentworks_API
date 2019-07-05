using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.ReceiveReversal;
using MoneyGram.AgentConnect.IntegrationTest.Data.Requests;
using MoneyGram.AgentConnect.IntegrationTest.Data.Send;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;
using MoneyGram.AgentConnect.IntegrationTest.Operations;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Lookup;
using MoneyGram.AgentConnect.IntegrationTest.Operations.ReceiveReversal;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Recv;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Send;
using System;
using System.Linq;
using MoneyGram.AgentConnect.IntegrationTest.Data.Recv;
using MoneyGram.AgentConnect.IntegrationTest.Data.Extensions;

namespace MoneyGram.AgentConnect.IntegrationTest.ReceiveReversal
{
    [TestClass]
    public class ReceiveReversalTests
    {
        private ReceiveReversalOperations receiveReversalOperations;
        private SendOperations _sendOperations;
        private ReceiveOperations _recvOperations;
        private TransactionLookupOperations _tranLookupOperations { get; set; }

        [TestInitialize]
        public void TestSetup()
        {
            var testRunner = new TestRunner();
            receiveReversalOperations = new ReceiveReversalOperations(testRunner);
            _sendOperations = new SendOperations(testRunner);
            _recvOperations = new ReceiveOperations(testRunner);
            _tranLookupOperations = new TransactionLookupOperations(testRunner);
        }

        #region TransactionLookup Test

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void ReceiveReversal_TransactionLookup()
        {
            //PERFORM send
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
            
            //ASSERT send
            Assert.IsFalse(sendData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{sendData.Errors?.Log()}");

            //PERFORM receive
            var receiveRequest = new ReceiveRequest
            {
                AgentState = AgentLocation.NY,
                ThirdPartyType = TestThirdPartyType.None
            };
            receiveRequest.PopulateAgentData(receiveRequest.AgentState);
            var receiveData = new ReceiveData(receiveRequest);
            receiveData.Set(sendData);
            _recvOperations.ReceiveComplete(receiveData);

            //ASSERT receive
            Assert.IsFalse(receiveData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{receiveData.Errors?.Log()}");

            //PERFORM transaction lookup
            var request = new ReceiveReversalOperationRequest
            {
                AgentState = AgentLocation.NY,
                ReferenceNumber = receiveData.SendData.CompleteSessionResp.Payload.ReferenceNumber
			};
            request.PopulateAgentData(request.AgentState);
            var receiveReversalData = new ReceiveReversalData(request);
            receiveReversalOperations.TransactionLookup(receiveReversalData);
            var transLookupResponse = receiveReversalData.TransactionLookup.TransactionLookupResp;

            //ASSERT transaction lookup         
            Assert.IsTrue(transLookupResponse.Payload.TransactionStatus == TransactionStatusType.RECVD);
            Assert.IsFalse(receiveReversalData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{receiveReversalData.Errors?.Log()}");
            Assert.IsFalse(string.IsNullOrEmpty(transLookupResponse.Payload.MgiSessionID));
        }

        #endregion

        #region Validation Tests

        [TestMethod]
        public void ReceiveReversal_ShouldSucceedForReceivingAgent()
        {
            //PERFORM send
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

            //ASSERT send
            Assert.IsFalse(sendData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{sendData.Errors?.Log()}");

            //PERFORM receive
            var receiveRequest = new ReceiveRequest
            {
                AgentState = AgentLocation.NY,
                ThirdPartyType = TestThirdPartyType.None
            };
            receiveRequest.PopulateAgentData(receiveRequest.AgentState);
            var receiveData = new ReceiveData(receiveRequest);
			receiveData.Set(sendData);
            _recvOperations.ReceiveComplete(receiveData);

            //ASSERT receive
            Assert.IsFalse(receiveData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{receiveData.Errors?.Log()}");

            //Get the transaction reference number

            //ReceiveReversal with single validate
            var request = new ReceiveReversalOperationRequest
            {
                AgentState = AgentLocation.NY,
                ReferenceNumber = receiveData.SendData.CompleteSessionResp.Payload.ReferenceNumber
			};
            request.PopulateAgentData(request.AgentState);

            var receiveReversalData = receiveReversalOperations.SingleValidate(request);
            var lastValidationResponse = receiveReversalData.ValidationResponses.LastOrDefault();

            // ASSERT ALL THE THINGS
            Assert.IsFalse(receiveReversalData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{receiveReversalData.Errors?.Log()}");
            Assert.IsTrue(lastValidationResponse.Payload.ReadyForCommit || lastValidationResponse.Payload.FieldsToCollect.Any());
        }

        [TestMethod]
        public void ReceiveReversalTransaction_ShouldFailForNonReceivingAgent()
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
            var receiveRequest = new ReceiveRequest
            {
                AgentState = AgentLocation.NY,
                ThirdPartyType = TestThirdPartyType.None
            };
            receiveRequest.PopulateAgentData(receiveRequest.AgentState);

            var receiveData = new ReceiveData(receiveRequest);
	        sendData = _sendOperations.SendCompleteForExistingCustomer(sendData);
			receiveData.Set(sendData);
            _recvOperations.ReceiveComplete(receiveData);

            var request = new ReceiveReversalOperationRequest
            {
                AgentState = AgentLocation.MN,
                ReferenceNumber = receiveData.SendData.CompleteSessionResp.Payload.ReferenceNumber
			};
            request.PopulateAgentData(request.AgentState);

            var receiveReversalData = receiveReversalOperations.SingleValidate(request);
            Assert.IsFalse(receiveReversalData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{receiveReversalData.Errors?.Log()}");

            var lastValidationResponse = receiveReversalData.ValidationResponses.LastOrDefault();
            Assert.IsTrue(lastValidationResponse.Errors.Any());
        }

        #endregion

        #region Complete Transaction

        [TestMethod]
        public void ReceiveReversalTransaction_Complete()
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
            var receiveRequest = new ReceiveRequest
            {
                AgentState = AgentLocation.NY,
                ThirdPartyType = TestThirdPartyType.None
            };
            receiveRequest.PopulateAgentData(receiveRequest.AgentState);

            var receiveData = new ReceiveData(receiveRequest);
            sendData = _sendOperations.SendCompleteForExistingCustomer(sendData);
            receiveData.Set(sendData);
            _recvOperations.ReceiveComplete(receiveData);

            //Get the transaction reference number

            var request = new ReceiveReversalOperationRequest
            {
                AgentState = AgentLocation.NY,
                ReferenceNumber = receiveData.SendData.CompleteSessionResp.Payload.ReferenceNumber
			};
            request.PopulateAgentData(request.AgentState);

            var receiveReversalData = receiveReversalOperations.ReceiveReversalComplete(request);
            Assert.IsFalse(receiveReversalData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{receiveReversalData.Errors?.Log()}");
            //Commented out as CompleteSessionResponse does not contain reference number when performing Receive Reversal. The above check for errors is enough.
            //Assert.IsFalse(string.IsNullOrEmpty(receiveReversalData.CompleteSessionResp.Payload.ReferenceNumber), "there was no reference number on the receive reversal completion response");
        }

        #endregion
    }
}