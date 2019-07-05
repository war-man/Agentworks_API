using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Lookup
{
    public class GetCountryInfoData
    {
        public GetCountryInfoRequest SendActiveCountriesReq { get; set; }
        public GetCountryInfoResponse SendActiveCountriesResp { get; set; }
    }
}