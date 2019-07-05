using System.Collections.Generic;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Models
{
    public class EnvironmentAgent
    {
        public string Environment { get; set; }
        public string AgentCountryIsoCode { get; set; }
        public string AgentCountry { get; set; }
        public string AgentStateCode { get; set; }
        public string AgentState { get; set; }
        public string AgentId { get; set; }
        public string AgentSequence { get; set; }
        public string AgentPassword { get; set; }
        public string Language { get; set; }
        public List<string> SendCurrencies { get; set; }
    }
}