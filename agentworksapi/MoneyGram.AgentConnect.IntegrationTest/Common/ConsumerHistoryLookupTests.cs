using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyGram.AgentConnect.DomainModel.Exceptions;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;
using MoneyGram.AgentConnect.IntegrationTest.Operations;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Common;
using System;
using System.Linq;

namespace MoneyGram.AgentConnect.IntegrationTest.Common
{
    [TestClass]
    public class ConsumerHistoryLookupTests
    {
        private ConsumerHistoryLookupOperations _consumerHistLookupOperations;
        private Agents _agents;

        [TestInitialize]
        public void TestSetup()
        {
            var testRunner = new TestRunner();
            _consumerHistLookupOperations = new ConsumerHistoryLookupOperations(testRunner);
            _agents = new Agents(testRunner.IsTrainingMode);
        }

        [TestMethod]
        public void ConsumerLookup_Existing_Test()
        {
            var agent = _agents.GetAgent(AgentLocation.MN);
            var consumerHistLookupData = _consumerHistLookupOperations.ConsumerHistoryLookup(agent.AgentId, agent.AgentSequence, true, SessionType.SEND);
            Assert.IsTrue(consumerHistLookupData.ConsumerHistoryLookupResp.Payload.SenderInfos.SenderInfo.Any(), $"Errors: {Environment.NewLine}{consumerHistLookupData.ConsumerHistoryLookupResp.Errors?.Log()}");
        }

        [TestMethod]
        public void ConsumerLookup_NonExisting_Test()
        {
            var exception = new AgentConnectException();            
            try
            {
                var agent = _agents.GetAgent(AgentLocation.NY);
                var consumerHistLookupData = _consumerHistLookupOperations.ConsumerHistoryLookup(agent.AgentId, agent.AgentSequence, false, SessionType.SEND);
                exception.ErrorCode =
                    int.Parse(consumerHistLookupData.ConsumerHistoryLookupResp.Errors.FirstOrDefault()?.ErrorCode);

            }
            catch (Exception exc)
            {
                Assert.AreEqual(exc.GetType(), typeof(AgentConnectException));
                exception = (AgentConnectException)exc;
            }
            finally
            {
                Assert.AreEqual(exception.ErrorCode.ToString(), "616");
            }
        }
    }
}