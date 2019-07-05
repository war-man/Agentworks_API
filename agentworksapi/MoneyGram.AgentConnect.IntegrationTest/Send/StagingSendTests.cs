using System;
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
    public class StagingSendTests
    {
        private SendOperations _sendOperations { get; set; }

        [TestInitialize]
        public void TestSetup()
        {
            _sendOperations = new SendOperations(new TestRunner());
        }

        /// <summary>
        ///     Staging small amount Send
        /// </summary>
        [TestMethod]
        public void StagingSend_SmallAmount()
        {
            var sendRequest = new SendRequest
            {
                AgentState = AgentLocation.NY,
                Country = Country.Usa,
                State = State.Mn,
                SendCurr = Currency.Usd,
                AmtRange = AmountRange.NoIdsNoThirdParty,
                FeeType = ItemChoiceType1.amountExcludingFee,
                ServiceOption = ServiceOptionType.WillCall
            };
            sendRequest.PopulateAgentData(sendRequest.AgentState);

            var sendData = new SendData(sendRequest);
            sendData = _sendOperations.SendCompleteStagedForExistingCustomer(sendData);

            Assert.IsFalse(sendData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{sendData.Errors?.Log()}");
        }
        /// <summary>
        ///     Staging small amount Send
        /// </summary>
        [TestMethod]
        public void StagingSend_SmallAmountNewCustomer()
        {
            var sendRequest = new SendRequest
            {
                AgentState = AgentLocation.NY,
                Country = Country.Usa,
                State = State.Mn,
                SendCurr = Currency.Usd,
                AmtRange = AmountRange.NoIdsNoThirdParty,
                FeeType = ItemChoiceType1.amountExcludingFee,
                ServiceOption = ServiceOptionType.WillCall
            };
            sendRequest.PopulateAgentData(sendRequest.AgentState);

            var sendData = new SendData(sendRequest);
            sendData = _sendOperations.SendCompleteStagedForNewCustomer(sendData);

            Assert.IsFalse(sendData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{sendData.Errors?.Log()}");
        }
        /// <summary>
        ///     Staging high amount Send with third pssetarty set to NONE
        /// </summary>
        [TestMethod]
        public void StagingSend_HighAmount_ThirdParty_None()
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
                ThirdPartyType = TestThirdPartyType.None
            };
            sendRequest.PopulateAgentData(sendRequest.AgentState);

            var sendData = new SendData(sendRequest);
            sendData = _sendOperations.SendCompleteStagedForExistingCustomer(sendData);

            Assert.IsFalse(sendData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{sendData.Errors?.Log()}");
        }

        /// <summary>
        ///     Staging high amount Send with third party set to PERSON
        /// </summary>
        [TestMethod]
        public void StagingSend_HighAmount_ThirdParty_Person()
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
            sendData = _sendOperations.SendCompleteStagedForExistingCustomerThirdParty(sendData);

            Assert.IsFalse(sendData.Errors.Any(), $" {Environment.NewLine}Errors: {Environment.NewLine}{sendData.Errors?.Log()}");
        }
    }
}