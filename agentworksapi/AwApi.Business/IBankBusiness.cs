using AwApi.ViewModels;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace AwApi.Business
{
    public interface IBankBusiness
    {
        AcApiResponse<GetBankDetailsByLevelResponse, ApiData> GetBankDetailsByLevel(GetBankDetailsByLevelRequest reqVm);

        AcApiResponse<GetBankDetailsResponse, ApiData> GetBankDetails(GetBankDetailsRequest reqVm);
    }
}
