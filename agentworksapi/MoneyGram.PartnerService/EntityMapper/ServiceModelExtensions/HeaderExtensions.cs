using DOMAIN = MoneyGram.PartnerService.DomainModel;
using SERVICE = MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService.EntityMapper.ServiceModelExtensions
{
    public static class HeaderExtensions
    {
        public static DOMAIN.Header ToDomain(this SERVICE.Header req)
        {
            var domainModel = PsMapper.Map<SERVICE.Header, DOMAIN.Header>(req)
                .AdditionalOperations();
            return domainModel;
        }

        public static DOMAIN.Header AdditionalOperations(this DOMAIN.Header dmModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(dmModel);
            return dmModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<SERVICE.Header, DOMAIN.Header>()
                .ForMember(dm => dm.ClientHeader, opt => opt.MapFrom(svs => svs.clientHeader.ToDomain()))
                .ForMember(dm => dm.ProcessingInstruction, opt => opt.MapFrom(svs => svs.processingInstruction.ToDomain()))
                .ForMember(dm => dm.RoutingContext, opt => opt.MapFrom(svs => svs.routingContext.ToDomain()))
                .ForMember(dm => dm.Security, opt => opt.MapFrom(svs => svs.security.ToDomain()));
        }
    }
}
