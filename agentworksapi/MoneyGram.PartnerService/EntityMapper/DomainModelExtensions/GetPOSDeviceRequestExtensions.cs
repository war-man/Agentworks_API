using DOMAIN = MoneyGram.PartnerService.DomainModel.Request;
using SERVICE = MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService.EntityMapper.DomainModelExtensions
{
    public static class GetPOSDeviceRequestExtensions
    {
        public static SERVICE.GetPOSDeviceRequest ToService(this DOMAIN.PosDeviceRequest req)
        {
            var serviceModel = PsMapper.Map<DOMAIN.PosDeviceRequest, SERVICE.GetPOSDeviceRequest>(req)
                .AdditionalOperations();
            return serviceModel;
        }

        public static SERVICE.GetPOSDeviceRequest AdditionalOperations(this SERVICE.GetPOSDeviceRequest svcModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(svcModel);
            return svcModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<DOMAIN.PosDeviceRequest, SERVICE.GetPOSDeviceRequest>()
                .ForMember(svs => svs.header, opt => opt.MapFrom(dm => dm.header.ToService()));
        }
    }
}