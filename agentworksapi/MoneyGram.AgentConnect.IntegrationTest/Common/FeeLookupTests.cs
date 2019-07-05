using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Extensions;
using MoneyGram.AgentConnect.IntegrationTest.Data.Requests;
using MoneyGram.AgentConnect.IntegrationTest.Data.Send;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;
using MoneyGram.AgentConnect.IntegrationTest.Operations;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Common;
using System;
using System.Linq;

namespace MoneyGram.AgentConnect.IntegrationTest.Common
{
    [TestClass]
    public class FeeLookupTests
    {
        private FeeLookupOperations _feeLookupOperations { get; set; }

        [TestInitialize]
        public void TestSetup()
        {
            _feeLookupOperations = new FeeLookupOperations(new TestRunner());
        }

        [TestMethod]
        public void FeeLookup_SendUsSmallAmt_Test()
        {
            var sendRequest = new SendRequest
            {
                AgentState = AgentLocation.NY,
                Country = Country.Usa,
                State = State.Mn,
                SendCurr = Currency.Usd,
                AmtRange = AmountRange.NoIdsNoThirdParty,
                FeeType = ItemChoiceType1.amountExcludingFee
            };
            sendRequest.PopulateAgentData(sendRequest.AgentState);

            var sendData = new SendData(sendRequest);
            var feeLookupData = _feeLookupOperations.FeeLookupForSend(sendData);
            Assert.IsTrue(feeLookupData.FeeLookupResp.Payload.FeeInfos.Any(), $"Errors: {Environment.NewLine}{feeLookupData.FeeLookupResp.Errors?.Log()}");
        }

        [TestMethod]
        public void FeeLookup_SendUsMediumAmt_Test()
        {
            var sendRequest = new SendRequest
            {
                AgentState = AgentLocation.NY,
                Country = Country.Usa,
                State = State.Mn,
                SendCurr = Currency.Usd,
                AmtRange = AmountRange.SingleId,
                FeeType = ItemChoiceType1.amountExcludingFee
            };
            sendRequest.PopulateAgentData(sendRequest.AgentState);

            var sendData = new SendData(sendRequest);
            var feeLookupData = _feeLookupOperations.FeeLookupForSend(sendData);
            Assert.IsTrue(feeLookupData.FeeLookupResp.Payload.FeeInfos.Any(), $"Errors: {Environment.NewLine}{feeLookupData.FeeLookupResp.Errors?.Log()}");
        }

        [TestMethod]
        public void FeeLookup_SendUsLargeAmt_Test()
        {
            var sendRequest = new SendRequest
            {
                AgentState = AgentLocation.NY,
                Country = Country.Usa,
                State = State.Mn,
                SendCurr = Currency.Usd,
                AmtRange = AmountRange.TwoIds,
                FeeType = ItemChoiceType1.amountExcludingFee
            };
            sendRequest.PopulateAgentData(sendRequest.AgentState);

            var sendData = new SendData(sendRequest);
            var feeLookupData = _feeLookupOperations.FeeLookupForSend(sendData);
            Assert.IsTrue(feeLookupData.FeeLookupResp.Payload.FeeInfos.Any(), $"Errors: {Environment.NewLine}{feeLookupData.FeeLookupResp.Errors?.Log()}");
        }
    }
}