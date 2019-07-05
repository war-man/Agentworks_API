using AwApi.ViewModels;
using MoneyGram.OpenIDM;

namespace AwApi.EntityMapper.ViewModelExtensions
{
    public static class DwRegisterDeviceReqVmExtensions
    {
        public static DwRegisterDeviceRequest ToModel(this DwRegisterDeviceReqVm dwRegisterDeviceReqVm)
        {
            var dwRegisterDeviceRequest = AwMapper.Map<DwRegisterDeviceReqVm, DwRegisterDeviceRequest>(dwRegisterDeviceReqVm);
            return dwRegisterDeviceRequest;
        }

        public static void DefineMappings()
        {
            AwMapper.CreateMap<DwRegisterDeviceReqVm, DwRegisterDeviceRequest>();
        }
    }
}