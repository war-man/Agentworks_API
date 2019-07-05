using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Extensions;
using MoneyGram.AgentConnect.IntegrationTest.Data.Requests;
using MoneyGram.AgentConnect.IntegrationTest.Data.Send;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;
using MoneyGram.AgentConnect.IntegrationTest.Operations;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Send;
using System;
using System.Linq;

namespace MoneyGram.AgentConnect.IntegrationTest.Send
{
    [TestClass]
    public class SendWillCallTests
    {
        private SendOperations _sendOperations { get; set; }

        [TestInitialize]
        public void TestSetup()
        {
            _sendOperations = new SendOperations(new TestRunner());
        }

        #region Complete Transaction

        [TestMethod]
        public void Send_WillCall_Complete_NoIdsOrThirdParty_Test()
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

            // ASSERT ALL THE THINGS
            Assert.IsFalse(sendData.Errors.Any(), $"Errors: {Environment.NewLine}{sendData.Errors?.Log()}");
            Assert.IsTrue(sendData.CompleteSessionResp != null);
            Assert.IsTrue(!string.IsNullOrEmpty(sendData.CompleteSessionResp.Payload.ReferenceNumber));
        }

        [TestMethod]
        public void Send_WillCall_Complete_WithSingleId_Test()
        {
            var sendRequest = new SendRequest
            {
                AgentState = AgentLocation.MN,
                Country = Country.Usa,
                State = State.Ny,
                SendCurr = Currency.Usd,
                AmtRange = AmountRange.SingleId,
                FeeType = ItemChoiceType1.amountExcludingFee,
                ServiceOption = ServiceOptionType.WillCall
            };
            sendRequest.PopulateAgentData(sendRequest.AgentState);

            var sendData = new SendData(sendRequest);
            sendData = _sendOperations.SendCompleteForExistingCustomer(sendData);

            // ASSERT ALL THE THINGS
            Assert.IsFalse(sendData.Errors.Any(), $"Errors: {Environment.NewLine}{sendData.Errors?.Log()}");
            Assert.IsTrue(sendData.CompleteSessionResp != null);
            Assert.IsTrue(!string.IsNullOrEmpty(sendData.CompleteSessionResp.Payload.ReferenceNumber));
        }

        [TestMethod]
        public void Send_WillCall_Complete_WithTwoIds_Test()
        {
            var sendRequest = new SendRequest
            {
                AgentState = AgentLocation.MN,
                Country = Country.Usa,
                State = State.Ny,
                SendCurr = Currency.Usd,
                AmtRange = AmountRange.TwoIds,
                FeeType = ItemChoiceType1.amountExcludingFee,
                ServiceOption = ServiceOptionType.WillCall
            };
            sendRequest.PopulateAgentData(sendRequest.AgentState);

            var sendData = new SendData(sendRequest);
            sendData = _sendOperations.SendCompleteForExistingCustomer(sendData);

            // ASSERT ALL THE THINGS
            Assert.IsFalse(sendData.Errors.Any(), $"Errors: {Environment.NewLine}{sendData.Errors?.Log()}");
            Assert.IsTrue(sendData.CompleteSessionResp != null);
            Assert.IsTrue(!string.IsNullOrEmpty(sendData.CompleteSessionResp.Payload.ReferenceNumber));
        }
        [TestMethod]
        public void Send_WillCall_Complete_WithThirdPartyOrg_Test()
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
                ThirdPartyType = TestThirdPartyType.Org
            };
            sendRequest.PopulateAgentData(sendRequest.AgentState);

            var sendData = new SendData(sendRequest);
            sendData = _sendOperations.SendCompleteForThirdParty(sendData);

            // ASSERT ALL THE THINGS
            Assert.IsFalse(sendData.Errors.Any(), $"Errors: {Environment.NewLine}{sendData.Errors?.Log()}");
            Assert.IsTrue(sendData.CompleteSessionResp != null);
            Assert.IsTrue(!string.IsNullOrEmpty(sendData.CompleteSessionResp.Payload.ReferenceNumber));
        }
        [TestMethod]
        public void Send_WillCall_NyToMnWithThirdPartyPerson_Test()
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

            // ASSERT ALL THE THINGS
            Assert.IsFalse(sendData.Errors.Any(), $"Errors: {Environment.NewLine}{sendData.Errors?.Log()}");
            Assert.IsTrue(sendData.CompleteSessionResp != null);
            Assert.IsTrue(!string.IsNullOrEmpty(sendData.CompleteSessionResp.Payload.ReferenceNumber));
        }

        [TestMethod]
        public void Send_WillCall_Complete_WithThirdPartyNONE_Test()
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
                ThirdPartyType = null
            };
            sendRequest.PopulateAgentData(sendRequest.AgentState);

            var sendData = new SendData(sendRequest);
            sendData = _sendOperations.SendCompleteForThirdParty(sendData);

            // ASSERT ALL THE THINGS
            Assert.IsFalse(sendData.Errors.Any(), $"Errors: {Environment.NewLine}{sendData.Errors?.Log()}");
            Assert.IsTrue(sendData.CompleteSessionResp != null);
            Assert.IsTrue(!string.IsNullOrEmpty(sendData.CompleteSessionResp.Payload.ReferenceNumber));
        }

        #endregion
    }
}