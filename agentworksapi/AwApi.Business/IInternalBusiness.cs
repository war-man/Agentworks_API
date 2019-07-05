using AwApi.ViewModels;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace AwApi.Business
{
    public interface IInternalBusiness
    {
        AcApiResponse<DwInitialSetupResponse, ApiData> DwInitialSetup(DwInitialSetupRequest reqVm);
        AcApiResponse<DwProfileResponse, ApiData> DwProfile(DwProfileRequest reqVm);
        DwRegisterDeviceRespVm DwRegisterDevice(DwRegisterDeviceReqVm registerDeviceReqVm);
    }
}