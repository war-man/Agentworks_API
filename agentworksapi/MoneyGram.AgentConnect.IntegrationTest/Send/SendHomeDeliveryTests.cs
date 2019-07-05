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
    public class SendHomeDeliveryTests
    {
        private SendOperations _sendOperations { get; set; }

        [TestInitialize]
        public void TestSetup()
        {
            _sendOperations = new SendOperations(new TestRunner());
        }
        [TestMethod]
        public void Send_Simple_HomeDelivery_Vietnam()
        {
            var sendRequest = new SendRequest
            {
                AgentState = AgentLocation.NY,
                Country = Country.Vietnam,
                State = string.Empty,
                SendCurr = Currency.Usd,
                AmtRange = AmountRange.NoIdsNoThirdParty,
                FeeType = ItemChoiceType1.amountExcludingFee,
                ServiceOption = ServiceOptionType.HomeDelivery
            };
            sendRequest.PopulateAgentData(sendRequest.AgentState);
            var sendData = new SendData(sendRequest);  
            sendData = _sendOperations.SendCompleteForExistingCustomer(sendData);
            // ASSERT ALL THE THINGS
            var errorMsg = string.Join(Environment.NewLine, sendData.Errors.Select(x => $"{x.ErrorCode}: {x.OffendingField} - {x.Message}"));
            Assert.IsFalse(sendData.Errors.Any(), $"{Environment.NewLine}Errors:{Environment.NewLine}{errorMsg}");
            //todo: display all errors.
            Assert.IsFalse((sendData.CompleteSessionResp.Errors.Count > 0), $"{sendData.CompleteSessionResp.Errors.Count} errors");
        }
        [TestMethod]
        public void Send_PhotoId_HomeDelivery_Vietnam()
        {
            var sendRequest = new SendRequest
            {
                AgentState = AgentLocation.NY,
                Country = Country.Vietnam,
                State = string.Empty,
                SendCurr = Currency.Usd,
                AmtRange = AmountRange.SingleId,
                FeeType = ItemChoiceType1.amountExcludingFee,
                ServiceOption = ServiceOptionType.HomeDelivery
            };
            sendRequest.PopulateAgentData(sendRequest.AgentState);
            var sendData = new SendData(sendRequest);
            sendData = _sendOperations.SendCompleteForExistingCustomer(sendData);
            // ASSERT ALL THE THINGS
            var errorMsg = string.Join(Environment.NewLine, sendData.Errors.Select(x => $"{x.ErrorCode}: {x.OffendingField} - {x.Message}"));
            Assert.IsFalse(sendData.Errors.Any(), $"{Environment.NewLine}Errors:{Environment.NewLine}{errorMsg}");
            //todo: display all errors.
            Assert.IsFalse((sendData.CompleteSessionResp.Errors.Count > 0), $"{sendData.CompleteSessionResp.Errors.Count} errors");
        }
        [TestMethod]
        public void Send_PhotoId_LegalId_HomeDelivery_Vietnam()
        {
            var sendRequest = new SendRequest
            {
                AgentState = AgentLocation.NY,
                Country = Country.Vietnam,
                State = string.Empty,
                SendCurr = Currency.Usd,
                AmtRange = AmountRange.TwoIds,
                FeeType = ItemChoiceType1.amountExcludingFee,
                ServiceOption = ServiceOptionType.HomeDelivery
            };
            sendRequest.PopulateAgentData(sendRequest.AgentState);
            var sendData = new SendData(sendRequest);
            sendData = _sendOperations.SendCompleteForExistingCustomer(sendData);
            // ASSERT ALL THE THINGS
            var errorMsg = string.Join(Environment.NewLine, sendData.Errors.Select(x => $"{x.ErrorCode}: {x.OffendingField} - {x.Message}"));
            Assert.IsFalse(sendData.Errors.Any(), $"{Environment.NewLine}Errors:{Environment.NewLine}{errorMsg}");
            //todo: display all errors.
            Assert.IsFalse((sendData.CompleteSessionResp.Errors.Count > 0), $"{sendData.CompleteSessionResp.Errors.Count} errors");
        }
        [TestMethod]
        public void Send_ThirdParty_Person_HomeDelivery_Vietnam()
        {
            var sendRequest = new SendRequest
            {
                AgentState = AgentLocation.MN,
                Country = Country.Vietnam,
                State = string.Empty,
                SendCurr = Currency.Usd,
                AmtRange = AmountRange.ThirdParty,
                FeeType = ItemChoiceType1.amountExcludingFee,
                ServiceOption = ServiceOptionType.HomeDelivery,
                ThirdPartyType = TestThirdPartyType.Person
            };
            sendRequest.PopulateAgentData(sendRequest.AgentState);
            var sendData = new SendData(sendRequest);
            sendData = _sendOperations.SendCompleteForThirdParty(sendData);
            // ASSERT ALL THE THINGS
            var errorMsg = string.Join(Environment.NewLine, sendData.Errors.Select(x => $"{x.ErrorCode}: {x.OffendingField} - {x.Message}"));
            Assert.IsFalse(sendData.Errors.Any(), $"{Environment.NewLine}Errors:{Environment.NewLine}{errorMsg}");
            //todo: display all errors.
            Assert.IsFalse((sendData.CompleteSessionResp.Errors.Count > 0), $"{sendData.CompleteSessionResp.Errors.Count} errors");
        }
        [TestMethod]
        public void Send_ThirdParty_Org_HomeDelivery_Vietnam()
        {
            var sendRequest = new SendRequest
            {
                AgentState = AgentLocation.MN,
                Country = Country.Vietnam,
                State = string.Empty,
                SendCurr = Currency.Usd,
                AmtRange = AmountRange.ThirdParty,
                FeeType = ItemChoiceType1.amountExcludingFee,
                ServiceOption = ServiceOptionType.HomeDelivery,
                ThirdPartyType = TestThirdPartyType.Org
            };
            sendRequest.PopulateAgentData(sendRequest.AgentState);
            var sendData = new SendData(sendRequest);
            sendData = _sendOperations.SendCompleteForThirdParty(sendData);
            // ASSERT ALL THE THINGS
            var errorMsg = string.Join(Environment.NewLine, sendData.Errors.Select(x => $"{x.ErrorCode}: {x.OffendingField} - {x.Message}"));
            Assert.IsFalse(sendData.Errors.Any(), $"{Environment.NewLine}Errors:{Environment.NewLine}{errorMsg}");
            //todo: display all errors.
            Assert.IsFalse((sendData.CompleteSessionResp.Errors.Count > 0), $"{sendData.CompleteSessionResp.Errors.Count} errors");
        }
    }
}