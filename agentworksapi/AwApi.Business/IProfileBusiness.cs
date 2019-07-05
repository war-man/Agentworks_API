using AwApi.ViewModels;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace AwApi.Business
{
    public interface IProfileBusiness
    {
        AcApiResponse<ProfileResponse, ApiData> Profile(ProfileRequest reqVm);
        AcApiResponse<SaveProfileResponse, ApiData> SaveProfile(SaveProfileRequest reqVm);
        void SaveProfilePrinter(SaveProfilePrinterRequest request);
    }
}