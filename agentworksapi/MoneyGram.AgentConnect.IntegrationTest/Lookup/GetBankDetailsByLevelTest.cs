using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;
using MoneyGram.AgentConnect.IntegrationTest.Operations;
using MoneyGram.AgentConnect.IntegrationTest.Operations.Lookup;
using System;

namespace MoneyGram.AgentConnect.IntegrationTest.Lookup
{
    [TestClass]
    public class GetBankDetailsByLevelTest
    {
        private GetBankDetailsByLevelOperations _getBankDetailsByLevelOperations { get; set; }

        [TestInitialize]
        public void TestSetup()
        {
            _getBankDetailsByLevelOperations = new GetBankDetailsByLevelOperations(new TestRunner());
        }

        [TestMethod]
        public void GetBankDetailsByLevel_Bank()
        {
            var bankSetupData = Banks.GetBank(Country.India, BankIdentifier.ABHY);
            var bankNumber = bankSetupData.BankInfo.HierarchyLevelElementNumber;

            var response = _getBankDetailsByLevelOperations.GetBanks(AgentLocation.MN, Country.India);
            //TODO: ASSERT the data
            Assert.IsFalse(response.Errors?.Count > 0, $" {Environment.NewLine}Errors: {Environment.NewLine}{response.Errors?.Log()}");
            Assert.IsNotNull(response.Payload, $"{Environment.NewLine}Errors: {Environment.NewLine}Payload is null.");

            var bankFound = (response.Payload?.HierarchyLevelNumber == BankLevel.Bank) && (response.Payload.HierarchyLevelInfos.Exists(x => x.HierarchyLevelElementNumber == bankNumber));
            Assert.IsTrue(bankFound, $"{Environment.NewLine}Errors: {Environment.NewLine}Bank not found.");
        }

        [TestMethod]
        public void GetBankDetailsByLevel_State()
        {
            var bankSetupData = Banks.GetBank(Country.India, BankIdentifier.ABHY);
            var bankStateNumber = bankSetupData.BankStates[0].StateInfo.HierarchyLevelElementNumber;

            var response = _getBankDetailsByLevelOperations.GetStateDetails(AgentLocation.MN, Country.India, bankSetupData.BankInfo.HierarchyLevelElementNumber);

            // ASSERT ALL THE THINGS
            Assert.IsFalse(response.Errors?.Count > 0, $" {Environment.NewLine}Errors: {Environment.NewLine}{response.Errors?.Log()}");
            Assert.IsNotNull(response.Payload, $"{Environment.NewLine}Errors: {Environment.NewLine}Payload is null.");

            var stateFound = (response.Payload.HierarchyLevelNumber == BankLevel.State) && (response.Payload.HierarchyLevelInfos.Exists(x => x.HierarchyLevelElementNumber == bankStateNumber));
            Assert.IsTrue(stateFound, $"{Environment.NewLine}Errors: {Environment.NewLine}State for bank not found.");
        }

        [TestMethod]
        public void GetBankDetailsByLevel_City()
        {
            var bankSetupData = Banks.GetBank(Country.India, BankIdentifier.ABHY);
            var bankCityNumber = bankSetupData.BankStates[0].BankCities[0].CityInfo.HierarchyLevelElementNumber;

            var response = _getBankDetailsByLevelOperations.GetCityDetails(AgentLocation.MN, Country.India, bankSetupData.BankStates[0].StateInfo.HierarchyLevelElementNumber);

            // ASSERT ALL THE THINGS
            Assert.IsFalse(response.Errors?.Count > 0, $" {Environment.NewLine}Errors: {Environment.NewLine}{response.Errors?.Log()}");
            Assert.IsNotNull(response.Payload, $"{Environment.NewLine}Errors: {Environment.NewLine}Payload is null.");

            var cityFound = (response.Payload.HierarchyLevelNumber == BankLevel.City) && (response.Payload.HierarchyLevelInfos.Exists(x => x.HierarchyLevelElementNumber == bankCityNumber));
            Assert.IsTrue(cityFound, $"{Environment.NewLine}Errors: {Environment.NewLine}City for bank not found.");
        }

        [TestMethod]
        public void GetBankDetailsByLevel_Branch()
        {
            var bankSetupData = Banks.GetBank(Country.India, BankIdentifier.ABHY);
            var bankBranchNumber = bankSetupData.BankStates[0].BankCities[0].BankBranches[0].HierarchyLevelElementNumber;

            var response = _getBankDetailsByLevelOperations.GetBranchDetails(AgentLocation.MN, Country.India, bankSetupData.BankStates[0].BankCities[0].CityInfo.HierarchyLevelElementNumber);

            // ASSERT ALL THE THINGS
            Assert.IsFalse(response.Errors?.Count > 0, $" {Environment.NewLine}Errors: {Environment.NewLine}{response.Errors?.Log()}");
            Assert.IsNotNull(response.Payload, $"{Environment.NewLine}Errors: {Environment.NewLine}Payload is null.");

            var branchFound = (response.Payload.HierarchyLevelNumber == BankLevel.Branch) && (response.Payload.HierarchyLevelInfos.Exists(x => x.HierarchyLevelElementNumber == bankBranchNumber));
            Assert.IsTrue(branchFound, $"{Environment.NewLine}Errors: {Environment.NewLine}Branch for bank not found.");
        }
        [TestMethod]
        public void GetBankDetailsByLevel_IFSC()
        {
            var bankSetupData = Banks.GetBank(Country.India, BankIdentifier.ABHY);
            var bankFirstBranch = bankSetupData.BankStates[0].BankCities[0].BankBranches[0];
            var ifscFromSetup = bankFirstBranch.Attributes.Find(x => x.AttributeLabel == "IFSC").AttributeValue;

            var ifscFromAC = _getBankDetailsByLevelOperations.GetIFSC(AgentLocation.MN, Country.India, bankSetupData.BankStates[0].BankCities[0].CityInfo.HierarchyLevelElementNumber);
            Assert.AreEqual<string>(ifscFromSetup, ifscFromAC, $"{Environment.NewLine}Errors: {Environment.NewLine}IFSC for bank not found.");
        }
    }
}