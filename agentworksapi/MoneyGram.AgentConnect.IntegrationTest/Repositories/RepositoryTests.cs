using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Models;
using MoneyGram.AgentConnect.IntegrationTest.Data.Repositories;
using MoneyGram.AgentConnect;
using FeeType = MoneyGram.AgentConnect.IntegrationTest.Data.Models.FeeType;

namespace MoneyGram.AgentConnect.IntegrationTest.Repositories
{
    [TestClass]
    public class RepositoryTests
    {
        [TestMethod]
        public void EnvironmentAgentRepository_GetValue()
        {
            var repo = new EnvironmentAgentRepository();

            var key = "Q5";
            var value = repo.GetValue<List<EnvironmentAgent>>(key);

            Assert.IsNotNull(value);
            Assert.AreEqual(4, value.Count);
        }

        [TestMethod]
        public void EnvironmentAgentRepository_GetAllValues()
        {
            var repo = new EnvironmentAgentRepository();

            var values = repo.GetAllValues<List<EnvironmentAgentInfoList>>();

            Assert.IsNotNull(values);
            Assert.IsTrue(values.Count > 0);
        }

        [TestMethod]
        public void EnvironmentAgentRepository_AddValue()
        {
            var repo = new EnvironmentAgentRepository();

            var key = "D0";
            var environmentAgents = new List<EnvironmentAgent>
            {
                new EnvironmentAgent
                {
                    AgentCountryIsoCode = "FAK",
                    AgentCountry = "FAKE COUNTRY",
                    AgentStateCode = "FA",
                    AgentState = "FAKE STATE",
                    AgentId = "44440000",
                    AgentSequence = "00",
                    AgentPassword = "000",
                    Language = "en",
                    SendCurrencies = new List<string> { "USD" }
                }
            };

            repo.AddValue(key, environmentAgents);

            var containsKey = repo.ContainsKey(key);
            var value = repo.GetValue<List<EnvironmentAgent>>(key);

            Assert.IsTrue(containsKey);
            Assert.AreEqual(1, value.Count);
            Assert.AreEqual("FAK", value.FirstOrDefault()?.AgentCountryIsoCode);
        }

        [TestMethod]
        public void EnvironmentAgentRepository_Update()
        {
            var repo = new EnvironmentAgentRepository();

            var key = "D5";
            var environmentAgents = new List<EnvironmentAgent>
            {
                new EnvironmentAgent
                {
                    AgentCountryIsoCode = "FAK",
                    AgentCountry = "FAKE COUNTRY",
                    AgentStateCode = "FA",
                    AgentState = "FAKE STATE",
                    AgentId = "44440000",
                    AgentSequence = "00",
                    AgentPassword = "000",
                    Language = "en",
                    SendCurrencies = new List<string> {"USD"}
                }
            };

            repo.UpdateValue(key, environmentAgents);

            var containsKey = repo.ContainsKey(key);
            var value = repo.GetValue<List<EnvironmentAgent>>(key);

            Assert.IsTrue(containsKey);
            Assert.AreEqual(1, value.Count);
            Assert.AreEqual("FAK", value.FirstOrDefault()?.AgentCountryIsoCode);
        }

        [TestMethod]
        public void EnvironmentAgentRepository_RemoveValue()
        {
            var repo = new EnvironmentAgentRepository();

            var key = "D5";
            
            repo.RemoveValue(key);

            var containsKey = repo.ContainsKey(key);

            Assert.IsFalse(containsKey);
        }

        [TestMethod]
        public void EnvironmentAgentRepository_Search()
        {
            var repo = new EnvironmentAgentRepository();

            var searchTerm = "44869210"; // 40698809 44869210

            var values = repo.Search<List<EnvironmentAgent>>(searchTerm);

            Assert.IsNotNull(values);
            Assert.IsTrue(values.Count > 0);
        }

        [TestMethod]
        public void EnvironmentRepository_GetAllValues()
        {
            var repo = new EnvironmentRepository();

            var values = repo.GetAllValues<List<string>>();

            Assert.IsNotNull(values);
            Assert.IsTrue(values.Count > 0);
            Assert.IsTrue(values.Contains("Q5"));
        }

        [TestMethod]
        public void EnvironmentRepository_GetValue()
        {
            var repo = new EnvironmentRepository();

            var key = "Q5";

            var value = repo.GetValue<string>(key);

            Assert.IsNotNull(value);
            Assert.AreEqual(key, value);
        }

        [TestMethod]
        public void EnvironmentRepository_AddValue()
        {
            var repo = new EnvironmentRepository();

            var key = "D0";
            var val = "D0";

            repo.AddValue(key, val);

            var value = repo.GetValue<string>(key);

            Assert.IsNotNull(value);
            Assert.IsTrue(value.Contains("D0"));
        }

        [TestMethod]
        public void EnvironmentRepository_UpdateValue()
        {
            var repo = new EnvironmentRepository();

            var key = "D5";
            var val = "D6";

            repo.UpdateValue(key, val);

            var value = repo.GetValue<string>(key);

            Assert.IsNotNull(value);
            Assert.IsTrue(value.Contains("D6"));
            Assert.IsFalse(value.Contains("D5"));
        }

        [TestMethod]
        public void EnvironmentRepository_RemoveValue()
        {
            var repo = new EnvironmentRepository();

            var key = "D3";

            repo.RemoveValue(key);

            var containsKey = repo.ContainsKey(key);

            Assert.IsFalse(containsKey);
        }

        [TestMethod]
        public void CountryRepository_GetValue()
        {
            var repo = new CountryRepository();

            var value = repo.GetValue<CountryInfo>("USA");

            Assert.IsNotNull(value);
            Assert.AreEqual("USD", value.BaseCurrency);
        }

        [TestMethod]
        public void CountryRepository_GetAllValues()
        {
            var repo = new CountryRepository();

            var values = repo.GetAllValues<List<CountryInfo>>();

            Assert.IsNotNull(values);
            Assert.IsTrue(values.Count > 0);
        }

        [TestMethod]
        public void CountryRepository_AddValue()
        {
            var repo = new CountryRepository();

            var key = "AAA";
            var countryInfo = new CountryInfo
            {
                CountryCode = "AAA",
                CountryName = "Added Fake Country",
                BaseCurrency = "AAA"
            };

            repo.AddValue(key, countryInfo);

            var value = repo.GetValue<CountryInfo>(key);
            var containsKey = repo.ContainsKey(key);

            Assert.IsNotNull(value);
            Assert.IsTrue(containsKey);
            Assert.AreEqual("AAA", value.CountryCode);
        }

        [TestMethod]
        public void CountryRepository_UpdateValue()
        {
            var repo = new CountryRepository();

            var key = "ASM";
            var countryInfo = new CountryInfo
            {
                CountryCode = "ASM",
                CountryName = "Updated American Samoa",
                BaseCurrency = "USD"
            };

            repo.UpdateValue(key, countryInfo);
            var containsKey = repo.ContainsKey(key);

            var value = repo.GetValue<CountryInfo>(key);

            Assert.IsNotNull(value);
            Assert.IsTrue(containsKey);
            Assert.AreEqual("Updated American Samoa", value.CountryName);
        }

        [TestMethod]
        public void CountryRepository_RemoveValue()
        {
            var repo = new CountryRepository();

            var key = "FAK";

            repo.RemoveValue(key);

            var containsKey = repo.ContainsKey(key);

            Assert.IsFalse(containsKey);
        }

        [TestMethod]
        public void CountryRepository_Search()
        {
            var repo = new CountryRepository();

            var searchTerm = "usd";

            var values = repo.Search<List<CountryInfo>>(searchTerm);

            Assert.IsNotNull(values);
            Assert.IsTrue(values.Count > 0);
        }

        [TestMethod]
        public void CountrySubdivisionRepository_GetValue()
        {
            var repo = new CountrySubdivisionRepository();

            var value = repo.GetValue<List<SubdivisionInfo>>("USA");

            Assert.IsNotNull(value);
            Assert.AreEqual(51, value.Count);
        }

        [TestMethod]
        public void CountrySubdivisionRepository_GetAllValues()
        {
            var repo = new CountrySubdivisionRepository();

            var values = repo.GetAllValues<List<CountrySubdivisionInfo>>();

            Assert.IsNotNull(values);
            Assert.IsTrue(values.Count > 0);
        }

        [TestMethod]
        public void CountrySubdivisionRepository_AddValue()
        {
            var repo = new CountrySubdivisionRepository();

            var key = "AAA";
            var subdivisions = new List<SubdivisionInfo>
            {
                new SubdivisionInfo
                {
                    CountrySubdivisionCode = "AAA-1",
                    CountrySubdivisionName = "AAA 1"
                },
                new SubdivisionInfo
                {
                    CountrySubdivisionCode = "AAA-2",
                    CountrySubdivisionName = "AAA 2"
                }
            };

            repo.AddValue(key, subdivisions);

            var value = repo.GetValue<List<SubdivisionInfo>>(key);
            var containsKey = repo.ContainsKey(key);

            Assert.IsNotNull(value);
            Assert.IsTrue(containsKey);
            Assert.AreEqual(2, value.Count);
        }

        [TestMethod]
        public void CountrySubdivisionRepository_UpdateValue()
        {
            var repo = new CountrySubdivisionRepository();

            var key = "MEX";
            var subdivisons = new List<SubdivisionInfo>
            {
                new SubdivisionInfo
                {
                    CountrySubdivisionCode = "MX-BCN",
                    CountrySubdivisionName = "Baja California"
                }
            };

            repo.UpdateValue(key, subdivisons);

            var value = repo.GetValue<List<SubdivisionInfo>>(key);
            var containsKey = repo.ContainsKey(key);

            Assert.IsNotNull(value);
            Assert.IsTrue(containsKey);
            Assert.AreEqual(1, value.Count);
            Assert.AreEqual("MX-BCN", value.FirstOrDefault()?.CountrySubdivisionCode);
        }
        
        [TestMethod]
        public void CountrySubdivisionRepository_RemoveValue()
        {
            var repo = new CountrySubdivisionRepository();

            var key = "CAN";

            repo.RemoveValue(key);

            var containsKey = repo.ContainsKey(key);

            Assert.IsFalse(containsKey);
        }

        [TestMethod]
        public void CountrySubdivisionRepository_Search_Country()
        {
            var repo = new CountrySubdivisionRepository();

            var searchTerm = "usa";

            var values = repo.Search<List<CountrySubdivisionInfo>>(searchTerm);

            Assert.IsNotNull(values);
            Assert.IsTrue(values.Count > 0);
        }

        [TestMethod]
        public void CountrySubdivisionRepository_Search_Subdivision()
        {
            var repo = new CountrySubdivisionRepository();

            var searchTerm = "minnesota";

            var values = repo.Search<List<CountrySubdivisionInfo>>(searchTerm);

            Assert.IsNotNull(values);
            Assert.IsTrue(values.Count > 0);
        }

        [TestMethod]
        public void CurrencyRepository_GetValue()
        {
            var repo = new CurrencyRepository();

            var value = repo.GetValue<CurrencyInfo>("USD");

            Assert.IsNotNull(value);
            Assert.AreEqual("US DOLLAR", value.CurrencyName);
        }

        [TestMethod]
        public void CurrencyRepository_GetAllValues()
        {
            var repo = new CurrencyRepository();

            var values = repo.GetAllValues<List<CurrencyInfo>>();

            Assert.IsNotNull(values);
            Assert.IsTrue(values.Count > 0);
        }

        [TestMethod]
        public void CurrencyRepository_AddValue()
        {
            var repo = new CurrencyRepository();

            var key = "FAK";
            var currency = new CurrencyInfo
            {
                CurrencyCode = "FAK",
                CurrencyName = "Fake Dollar"
            };

            repo.AddValue(key, currency);

            var value = repo.GetValue<CurrencyInfo>(key);
            var containsKey = repo.ContainsKey(key);

            Assert.IsNotNull(value);
            Assert.IsTrue(containsKey);
            Assert.AreEqual("FAK", value.CurrencyCode);
        }

        [TestMethod]
        public void CurrencyRepository_UpdateValue()
        {
            var repo = new CurrencyRepository();

            var key = "FJD";
            var currency = new CurrencyInfo
            {
                CurrencyCode = "FJD",
                CurrencyName = "UPDATED FIJI DOLLAR"
            };

            repo.UpdateValue(key, currency);
            var containsKey = repo.ContainsKey(key);

            var value = repo.GetValue<CurrencyInfo>(key);

            Assert.IsNotNull(value);
            Assert.IsTrue(containsKey);
            Assert.AreEqual("UPDATED FIJI DOLLAR", value.CurrencyName);
        }

        [TestMethod]
        public void CurrencyRepository_RemoveValue()
        {
            var repo = new CurrencyRepository();

            var key = "LVL";

            repo.RemoveValue(key);

            var containsKey = repo.ContainsKey(key);

            Assert.IsFalse(containsKey);
        }

        [TestMethod]
        public void CurrencyRepository_Search()
        {
            var repo = new CurrencyRepository();

            var searchTerm = "dollar";

            var values = repo.Search<List<CurrencyInfo>>(searchTerm);

            Assert.IsNotNull(values);
            Assert.IsTrue(values.Count > 0);
        }

        [TestMethod]
        public void AmountRangeRepository_GetValue()
        {
            var repo = new AmountRangeRepository();

            var value = repo.GetValue<AmountRange>("NoIdsNoThirdParty");

            Assert.IsNotNull(value);
            Assert.AreEqual("No IDs No Third Party", value.Display);
        }

        [TestMethod]
        public void AmountRangeRepository_GetAllValues()
        {
            var repo = new AmountRangeRepository();

            var values = repo.GetAllValues<List<AmountRange>>();

            Assert.IsNotNull(values);
            Assert.IsTrue(values.Count > 0);
        }

        [TestMethod]
        public void AmountRangeRepository_AddValue()
        {
            var repo = new AmountRangeRepository();

            var key = "Fake";
            var amountRange = new AmountRange
            {
                Code = "Fake",
                Display = "Fake"
            };

            repo.AddValue(key, amountRange);

            var value = repo.GetValue<AmountRange>(key);
            var containsKey = repo.ContainsKey(key);

            Assert.IsNotNull(value);
            Assert.IsTrue(containsKey);
            Assert.AreEqual("Fake", value.Code);
        }

        [TestMethod]
        public void AmountRangeRepository_UpdateValue()
        {
            var repo = new AmountRangeRepository();

            var key = "TwoIds";
            var amountRange = new AmountRange
            {
                Code = "TwoIds",
                Display = "Updated Two IDs"
            };

            repo.UpdateValue(key, amountRange);
            var containsKey = repo.ContainsKey(key);

            var value = repo.GetValue<AmountRange>(key);

            Assert.IsNotNull(value);
            Assert.IsTrue(containsKey);
            Assert.AreEqual("Updated Two IDs", value.Display);
        }

        [TestMethod]
        public void AmountRangeRepository_RemoveValue()
        {
            var repo = new CountryRepository();

            var key = "SingleId";

            repo.RemoveValue(key);

            var containsKey = repo.ContainsKey(key);

            Assert.IsFalse(containsKey);
        }

        [TestMethod]
        public void FeeTypeRepository_GetValue()
        {
            var repo = new FeeTypeRepository();

            var value = repo.GetValue<FeeType>("AmountExcludingFee");

            Assert.IsNotNull(value);
            Assert.AreEqual("Amount Excluding Fee", value.Display);
        }

        [TestMethod]
        public void FeeTypeRepository_GetAllValues()
        {
            var repo = new FeeTypeRepository();

            var values = repo.GetAllValues<List<FeeType>>();

            Assert.IsNotNull(values);
            Assert.IsTrue(values.Count > 0);
        }

        [TestMethod]
        public void FeeTypeRepository_AddValue()
        {
            var repo = new FeeTypeRepository();

            var key = "Fake";
            var feeType = new FeeType
            {
                Code = "Fake",
                Display = "Fake"
            };

            repo.AddValue(key, feeType);

            var value = repo.GetValue<FeeType>(key);
            var containsKey = repo.ContainsKey(key);

            Assert.IsNotNull(value);
            Assert.IsTrue(containsKey);
            Assert.AreEqual("Fake", value.Code);
        }

        [TestMethod]
        public void FeeTypeRepository_UpdateValue()
        {
            var repo = new FeeTypeRepository();

            var key = "AmountExcludingFee";
            var feeType = new FeeType
            {
                Code = "AmountExcludingFee",
                Display = "Amount Excluding Fee"
            };

            repo.UpdateValue(key, feeType);
            var containsKey = repo.ContainsKey(key);

            var value = repo.GetValue<FeeType>(key);

            Assert.IsNotNull(value);
            Assert.IsTrue(containsKey);
            Assert.AreEqual("Amount Excluding Fee", value.Display);
        }

        [TestMethod]
        public void FeeTypeRepository_RemoveValue()
        {
            var repo = new FeeTypeRepository();

            var key = "AmountExcludingFee";

            repo.RemoveValue(key);

            var containsKey = repo.ContainsKey(key);

            Assert.IsFalse(containsKey);
        }

        [TestMethod]
        public void ServiceOptionRepository_GetValue()
        {
            var repo = new ServiceOptionRepository();

            var value = repo.GetValue<ServiceOption>("WillCall");

            Assert.IsNotNull(value);
            Assert.AreEqual("Will Call", value.Display);
        }

        [TestMethod]
        public void ServiceOptionRepository_GetAllValues()
        {
            var repo = new ServiceOptionRepository();

            var values = repo.GetAllValues<List<ServiceOption>>();

            Assert.IsNotNull(values);
            Assert.IsTrue(values.Count > 0);
        }

        [TestMethod]
        public void ServiceOptionRepository_AddValue()
        {
            var repo = new ServiceOptionRepository();

            var key = "Fake";
            var serviceOption = new ServiceOption
            {
                Code = "Fake",
                Display = "Fake"
            };

            repo.AddValue(key, serviceOption);

            var value = repo.GetValue<ServiceOption>(key);
            var containsKey = repo.ContainsKey(key);

            Assert.IsNotNull(value);
            Assert.IsTrue(containsKey);
            Assert.AreEqual("Fake", value.Code);
        }

        [TestMethod]
        public void ServiceOptionRepository_UpdateValue()
        {
            var repo = new ServiceOptionRepository();

            var key = "ReceiveAt";
            var serviceOption = new ServiceOption
            {
                Code = "ReceiveAt",
                Display = "Updated Receive At"
            };

            repo.UpdateValue(key, serviceOption);
            var containsKey = repo.ContainsKey(key);

            var value = repo.GetValue<ServiceOption>(key);

            Assert.IsNotNull(value);
            Assert.IsTrue(containsKey);
            Assert.AreEqual("Updated Receive At", value.Display);
        }

        [TestMethod]
        public void ServiceOptionRepository_RemoveValue()
        {
            var repo = new ServiceOptionRepository();

            var key = "BankDeposit";

            repo.RemoveValue(key);

            var containsKey = repo.ContainsKey(key);

            Assert.IsFalse(containsKey);
        }

        [TestMethod]
        public void RefundReasonRepository_GetValue()
        {
            var repo = new RefundReasonRepository();

            var value = repo.GetValue<EnumeratedIdentifierInfo>("NO_RCV_LOC");

            Assert.IsNotNull(value);
            Assert.AreEqual("No Receive Location", value.Label);
        }

        [TestMethod]
        public void RefundReasonRepository_GetAllValues()
        {
            var repo = new RefundReasonRepository();

            var values = repo.GetAllValues<List<EnumeratedIdentifierInfo>>();

            Assert.IsNotNull(values);
            Assert.IsTrue(values.Count > 0);
        }

        [TestMethod]
        public void RefundReasonRepository_AddValue()
        {
            var repo = new RefundReasonRepository();

            var key = "Fake";
            var refundReason = new EnumeratedIdentifierInfo
            {
                Identifier = "Fake",
                Label = "Fake"
            };

            repo.AddValue(key, refundReason);

            var value = repo.GetValue<EnumeratedIdentifierInfo>(key);
            var containsKey = repo.ContainsKey(key);

            Assert.IsNotNull(value);
            Assert.IsTrue(containsKey);
            Assert.AreEqual("Fake", value.Identifier);
        }

        [TestMethod]
        public void RefundReasonRepository_UpdateValue()
        {
            var repo = new RefundReasonRepository();

            var key = "WRONG_SERVICE";
            var refundReason = new EnumeratedIdentifierInfo
            {
                Identifier = "WRONG_SERVICE",
                Label = "Updated Wrong Transfer Service"
            };

            repo.UpdateValue(key, refundReason);
            var containsKey = repo.ContainsKey(key);

            var value = repo.GetValue<EnumeratedIdentifierInfo>(key);

            Assert.IsNotNull(value);
            Assert.IsTrue(containsKey);
            Assert.AreEqual("Updated Wrong Transfer Service", value.Label);
        }

        [TestMethod]
        public void RefundReasonRepository_RemoveValue()
        {
            var repo = new RefundReasonRepository();

            var key = "NO_TQ";

            repo.RemoveValue(key);

            var containsKey = repo.ContainsKey(key);

            Assert.IsFalse(containsKey);
        }

        [TestMethod]
        public void TrainingModeRepository_CheckRepository()
        {
            var repo = new TrainingModeRepository();
            Assert.IsNotNull(repo);
        }
    }
}