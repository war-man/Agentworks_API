using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace AwApi.ViewModels.Lookup
{
    public class AllLookupResponse : Response
    {
        public ProfileResponse ProfileResponse { get; set; }
        public GetCurrencyInfoResponse GetCurrencyInfoResponse { get; set; }
        public GetCountryInfoResponse GetCountryInfoResponse { get; set; }
        public GetCountrySubdivisionResponse GetCountrySubdivisionResponse { get; set; }
        public IndustryResponse IndustryResponse { get; set; }
        public GetAllFieldsResponse GetAllFieldsResponse { get; set; }
        public GetEnumerationsResponse GetEnumerationsResponse { get; set; }
    }
}