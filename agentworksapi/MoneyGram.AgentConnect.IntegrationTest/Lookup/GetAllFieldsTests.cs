using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;
using MoneyGram.AgentConnect.IntegrationTest.Operations;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Lookup;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace MoneyGram.AgentConnect.IntegrationTest.Lookup
{
    [TestClass]
    public class GetAllFieldsTests
    {
        private GetAllFieldsOperations _gafOperations { get; set; }
        private string _logDir { get; set; }
        private Agents _agents { get; set; }

        [TestInitialize]
        public void TestSetup()
        {
            var _testRunner = new TestRunner();
            _gafOperations = new GetAllFieldsOperations(_testRunner);
            _logDir = ConfigurationManager.AppSettings["LoggingDirectory"];
            _agents = new Agents(_testRunner.IsTrainingMode);
        }

        [TestMethod]
        public void GetAllFieldsForSendTest()
        {
            var agent = _agents.GetAgent(AgentLocation.NY);
            var gafSendData = _gafOperations.GetAllFields(agent.AgentId, agent.AgentSequence, GetAllFieldsTransactionType.Send, string.Empty);
            //TODO: ASSERT the data
            Assert.IsTrue((gafSendData.InfoKeys.Count > 0), $"Errors: {Environment.NewLine}{gafSendData.GetAllFieldsResp.Errors?.Log()}");
            System.Diagnostics.Trace.WriteLine(string.Join(Environment.NewLine, gafSendData.InfoKeys));
        }

        [TestMethod]
        public void GetAllFieldsForReceiveTest()
        {
            var agent = _agents.GetAgent(AgentLocation.NY);
            var gafReceiveData = _gafOperations.GetAllFields(agent.AgentId, agent.AgentSequence, GetAllFieldsTransactionType.Receive, string.Empty);
            //TODO: ASSERT the data
            Assert.IsTrue((gafReceiveData.InfoKeys.Count > 0), $"Errors: {Environment.NewLine}{gafReceiveData.GetAllFieldsResp.Errors?.Log()}");
            System.Diagnostics.Trace.WriteLine(string.Join(Environment.NewLine, gafReceiveData.InfoKeys));
        }

        [TestMethod]
        public void GetAllFieldsForAmendTest()
        {
            var agent = _agents.GetAgent(AgentLocation.NY);
            var gafAmendData = _gafOperations.GetAllFields(agent.AgentId, agent.AgentSequence, GetAllFieldsTransactionType.Amend, string.Empty);
            //TODO: ASSERT the data
            Assert.IsTrue((gafAmendData.InfoKeys.Count > 0), $"Errors: {Environment.NewLine}{gafAmendData.GetAllFieldsResp.Errors?.Log()}");
            System.Diagnostics.Trace.WriteLine(string.Join(Environment.NewLine, gafAmendData.InfoKeys));
        }

        [TestMethod]

        public void GetAllFieldsTest()
        {
            var agent = _agents.GetAgent(AgentLocation.MN);
            var gafSendData = _gafOperations.GetAllFields(agent.AgentId, agent.AgentSequence, GetAllFieldsTransactionType.Send, string.Empty);
            Assert.IsTrue((gafSendData.InfoKeys.Count > 0), $"Errors: {Environment.NewLine}{gafSendData.GetAllFieldsResp.Errors?.Log()}");

            var gafReceiveData = _gafOperations.GetAllFields(agent.AgentId, agent.AgentSequence, GetAllFieldsTransactionType.Receive, string.Empty);
            Assert.IsTrue((gafReceiveData.InfoKeys.Count > 0), $"Errors: {Environment.NewLine}{gafReceiveData.GetAllFieldsResp.Errors?.Log()}");

            var gafAmendData = _gafOperations.GetAllFields(agent.AgentId, agent.AgentSequence, GetAllFieldsTransactionType.Amend, string.Empty);
            Assert.IsTrue((gafAmendData.InfoKeys.Count > 0), $"Errors: {Environment.NewLine}{gafAmendData.GetAllFieldsResp.Errors?.Log()}");

            List<string> gafInfoKeys = new List<string>();
            gafInfoKeys.AddRange(gafSendData.InfoKeys);
            gafInfoKeys.AddRange(gafReceiveData.InfoKeys);
            gafInfoKeys.AddRange(gafAmendData.InfoKeys);

            LogExtractedInfoKeysData(gafInfoKeys);
        }

        private void LogExtractedInfoKeysData(List<string> gafInfoKeys)
        {
            var keywordList = InfoKeyGroups.AllGroups;
            Dictionary<string, string> infoKeyGroupDict = new Dictionary<string, string>();

            System.Diagnostics.Trace.WriteLine($"====================== Total: {gafInfoKeys.Count} InfoKeys====================");
            System.Diagnostics.Trace.WriteLine(string.Join(Environment.NewLine, gafInfoKeys));
            System.IO.File.WriteAllLines($"{_logDir}GetAllFieldsData_{DateTime.Now.ToString("yyyyMMdd")}.txt", gafInfoKeys);

            List<string> uniqueCategories = GetGAFUniqueCategories(gafInfoKeys);
            System.Diagnostics.Trace.WriteLine($"====================== Categories: {uniqueCategories.Count} InfoKeys====================");
            System.Diagnostics.Trace.WriteLine(string.Join(Environment.NewLine, uniqueCategories));
            System.IO.File.WriteAllLines($"{_logDir}GetAllFieldsData_Categories_{DateTime.Now.ToString("yyyyMMdd")}.txt", uniqueCategories);

            //Get the unique InfoKeys from the FieldInfo list.
            List<string> uniqueInfoKeys = GetGAFUniqueInfoKeys(gafInfoKeys);
            System.Diagnostics.Trace.WriteLine($"====================== Distinct: {uniqueInfoKeys.Count} InfoKeys====================");
            System.Diagnostics.Trace.WriteLine(string.Join(Environment.NewLine, uniqueInfoKeys));
            System.IO.File.WriteAllLines($"{_logDir}GetAllFields_Distinct_{DateTime.Now.ToString("yyyyMMdd")}.txt", uniqueInfoKeys);

            infoKeyGroupDict = _gafOperations.GetInfoKeyWithGroupTypes(gafInfoKeys);
            System.Diagnostics.Trace.WriteLine($"====================== InfoKeysWithGroups: {infoKeyGroupDict.Count} InfoKeys====================");
            System.Diagnostics.Trace.WriteLine(string.Join(Environment.NewLine, infoKeyGroupDict));
            System.IO.File.WriteAllText($"{_logDir}GetAllFields_WithGroupTypes_{DateTime.Now.ToString("yyyyMMdd")}.txt", string.Join(Environment.NewLine, infoKeyGroupDict));
        }

        private static List<string> GetGAFUniqueInfoKeys(IEnumerable<string> gafInfoKeys)
        {
            //Ignore the 'Category' fields.
            var gafFieldInfoOnly = gafInfoKeys.Where(x => !x.Split('\t')[1].ToLower().Contains("category"));
            return gafFieldInfoOnly.Select(x => x.Split('\t')[2]).Distinct().ToList();
        }
        private static List<string> GetGAFUniqueCategories(IEnumerable<string> gafInfoKeys)
        {
            //Get the 'Category' fields.
            var gafFieldInfoOnly = gafInfoKeys.Where(x => x.Split('\t')[1].ToLower().Contains("category"));
            return gafFieldInfoOnly.Select(x => x.Split('\t')[2]).Distinct().ToList();
        }
    }
}