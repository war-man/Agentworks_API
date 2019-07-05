using AwApi.ViewModels;
using AwApi.ViewModels.Lookup;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace AwApi.Business
{
    public interface ILookupBusiness
    {
        AcApiResponse<GetCurrencyInfoResponse, ApiData> GetCurrencyInfo(GetCurrencyInfoRequest reqVm);
        AcApiResponse<GetCountryInfoResponse, ApiData> GetCountryInfo(GetCountryInfoRequest reqVm);
        AcApiResponse<GetCountrySubdivisionResponse, ApiData> GetCountrySubdivision(GetCountrySubdivisionRequest reqVm);
        AcApiResponse<IndustryResponse, ApiData> Industry(IndustryRequest reqVm);

        AcApiResponse<AllLookupResponse, ApiData> All(AllLookupRequest req);
        AcApiResponse<GetAllFieldsResponse, ApiData> GetAllFields(GetAllFieldsRequest reqVm);
        AcApiResponse<GetEnumerationsResponse, ApiData> GetEnumerations(GetEnumerationsRequest reqVm);
        GetEnumerationsResponse TryGetEnumerations(Request reqVm);
        GetAllFieldsResponse TryGetAllFields(Request reqVm, string cachedVersion, string tranType);
    }
}