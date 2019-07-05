using MoneyGram.AgentConnect.DomainModel.Transaction;
using System;

namespace AwApi.ViewModels.Lookup
{
    [Serializable]
    public class AllLookupRequest
    {
        public bool EnumerationsRequested { get; set; }
        public GetEnumerationsRequest EnumerationReq { get; set; }
        public bool ProfileRequested { get; set; }
        public ProfileRequest ProfileReq { get; set; }
        public bool CurrenciesRequested { get; set; }
        public GetCurrencyInfoRequest GetCurrencyInfoReq { get; set; }
        public bool CountriesRequested { get; set; }
        public GetCountryInfoRequest GetCountryInfoReq { get; set; }
        public bool CountrySubdivisionsRequested { get; set; }
        public GetCountrySubdivisionRequest GetCountrySubdivisionReq { get; set; }
        public bool IndustriesRequested { get; set; }
        public IndustryRequest IndustryRequestReq { get; set; }
        public bool GetAllFieldsRequested { get; set; }
        public GetAllFieldsRequest GetAllFieldsReq { get; set; }
    }
}