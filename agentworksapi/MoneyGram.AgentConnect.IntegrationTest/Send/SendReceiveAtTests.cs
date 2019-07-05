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
    public class SendReceiveAtTests
    {
        private SendOperations _sendOperations { get; set; }

        [TestInitialize]
        public void TestSetup()
        {
            _sendOperations = new SendOperations(new TestRunner());
        }

        #region Complete Transaction

        [TestMethod]
        public void Send_ReceiveAt_Complete_NoIdsOrThirdParty_Test()
        {
            var sendRequest = new SendRequest
            {
                AgentState = AgentLocation.MN,
                Country = Country.Mexico,
                State = string.Empty,
                SendCurr = Currency.Usd,
                AmtRange = AmountRange.NoIdsNoThirdParty,
                FeeType = ItemChoiceType1.amountExcludingFee,
                ServiceOption = ServiceOptionType.ReceiveAt
            };
            sendRequest.PopulateAgentData(sendRequest.AgentState);
            var sendData = new SendData(sendRequest);  
            sendData = _sendOperations.SendCompleteForExistingCustomer(sendData);

            // ASSERT ALL THE THINGS
            Assert.IsFalse(sendData.Errors.Any(), $"Errors: {Environment.NewLine}{sendData.Errors?.Log()}");
            Assert.IsTrue(sendData.CompleteSessionResp != null);
            Assert.IsTrue(!string.IsNullOrEmpty(sendData.CompleteSessionResp.Payload.ReferenceNumber));
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////
        // The maximum receive amount for this delivery option is  500 USD . If you wish to use this option the send amount must be lowered.
        // Bancomer cannot do anything above 500, so the below tests wont be applicable until we have another bank to use.
        ////////////////////////////////////////////////////////////////////////////////////////////////

        [TestMethod]
        public void Send_ReceiveAt_Complete_WithSingleId_Test()
        {
            var sendRequest = new SendRequest
            {
                AgentState = AgentLocation.MN,
                Country = Country.Mexico,
                State = string.Empty,
                SendCurr = Currency.Usd,
                AmtRange = AmountRange.SingleId,
                FeeType = ItemChoiceType1.amountExcludingFee,
                ServiceOption = ServiceOptionType.ReceiveAt
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
        public void Send_ReceiveAt_Complete_WithTwoIds_Test()
        {
            var sendRequest = new SendRequest
            {
                AgentState = AgentLocation.MN,
                Country = Country.Mexico,
                State = string.Empty,
                SendCurr = Currency.Usd,
                AmtRange = AmountRange.TwoIds,
                FeeType = ItemChoiceType1.amountExcludingFee,
                ServiceOption = ServiceOptionType.ReceiveAt
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
        public void Send_ReceiveAt_Complete_WithThirdPartyOrg_Test()
        {
            var sendRequest = new SendRequest
            {
                AgentState = AgentLocation.MN,
                Country = Country.Mexico,
                State = string.Empty,
                SendCurr = Currency.Usd,
                AmtRange = AmountRange.ThirdParty,
                FeeType = ItemChoiceType1.amountExcludingFee,
                ServiceOption = ServiceOptionType.ReceiveAt,
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
        public void Send_ReceiveAt_NyToMnWithThirdPartyPerson_Test()
        {
            var sendRequest = new SendRequest
            {
                AgentState = AgentLocation.MN,
                Country = Country.Mexico,
                State = string.Empty,
                SendCurr = Currency.Usd,
                AmtRange = AmountRange.ThirdParty,
                FeeType = ItemChoiceType1.amountExcludingFee,
                ServiceOption = ServiceOptionType.ReceiveAt,
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
        public void Send_ReceiveAt_Complete_WithThirdPartyNONE_Test()
        {
            var sendRequest = new SendRequest
            {
                AgentState = AgentLocation.MN,
                Country = Country.Mexico,
                State = string.Empty,
                SendCurr = Currency.Usd,
                AmtRange = AmountRange.ThirdParty,
                FeeType = ItemChoiceType1.amountExcludingFee,
                ServiceOption = ServiceOptionType.ReceiveAt,
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