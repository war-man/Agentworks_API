using DOMAIN = MoneyGram.PartnerService.DomainModel;
using SERVICE = MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService.EntityMapper.DomainModelExtensions
{
    public static class HeaderExtensions
    {
        public static SERVICE.Header ToService(this DOMAIN.Header req)
        {
            var serviceModel = PsMapper.Map<DOMAIN.Header, SERVICE.Header>(req)
                .AdditionalOperations();
            return serviceModel;
        }

        public static SERVICE.Header AdditionalOperations(this SERVICE.Header svcModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(svcModel);
            return svcModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<DOMAIN.Header, SERVICE.Header>()
                .ForMember(svs => svs.clientHeader, opt => opt.MapFrom(dm => dm.ClientHeader.ToService()))
                .ForMember(svs => svs.processingInstruction, opt => opt.MapFrom(dm => dm.ProcessingInstruction.ToService()))
                .ForMember(svs => svs.routingContext, opt => opt.MapFrom(dm => dm.RoutingContext.ToService()))
                .ForMember(svs => svs.security, opt => opt.MapFrom(dm => dm.Security.ToService()));
        }
    }
}