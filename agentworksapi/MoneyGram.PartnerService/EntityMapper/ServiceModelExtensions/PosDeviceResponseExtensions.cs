using System.Linq;
using DOMAIN = MoneyGram.PartnerService.DomainModel.Response;
using SERVICE = MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService.EntityMapper.ServiceModelExtensions
{
    public static class PosDeviceResponseExtensions
    {
        public static DOMAIN.PosDeviceResponse ToDomain(this SERVICE.GetPOSDeviceResponse req)
        {
            var domainModel = PsMapper.Map<SERVICE.GetPOSDeviceResponse, DOMAIN.PosDeviceResponse>(req)
                .AdditionalOperations();
            return domainModel;
        }

        public static DOMAIN.PosDeviceResponse AdditionalOperations(this DOMAIN.PosDeviceResponse dmModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(dmModel);
            return dmModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<SERVICE.GetPOSDeviceResponse, DOMAIN.PosDeviceResponse>()
                .ForMember(dm => dm.header, opt => opt.MapFrom(svs => svs.header.ToDomain()))
                .ForMember(dm => dm.PosDeviceList, opt => opt.MapFrom(svs => svs.posDeviceList != null ? svs.posDeviceList.ToList().ToPosDeviceList() : null));
        }
    }
}
