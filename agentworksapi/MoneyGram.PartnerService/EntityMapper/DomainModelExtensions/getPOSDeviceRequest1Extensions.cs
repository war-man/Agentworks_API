using DOMAIN = MoneyGram.PartnerService.DomainModel.Request;
using SERVICE = MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService.EntityMapper.DomainModelExtensions
{
    public static class getPOSDeviceRequest1Extensions
    {
        public static SERVICE.getPOSDeviceRequest1 ToPOSDeviceRequest1Service(this DOMAIN.PosDeviceRequest req)
        {
            var serviceModel = PsMapper.Map<DOMAIN.PosDeviceRequest, SERVICE.getPOSDeviceRequest1>(req)
                .AdditionalOperations();
            return serviceModel;
        }

        public static SERVICE.getPOSDeviceRequest1 AdditionalOperations(this SERVICE.getPOSDeviceRequest1 svcModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(svcModel);
            return svcModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<DOMAIN.PosDeviceRequest, SERVICE.getPOSDeviceRequest1>()
                .ForMember(svs => svs.getPOSDeviceRequest, opt => opt.MapFrom(dm => dm.ToService()));
        }
    }
}