using DOMAIN = MoneyGram.DLS.DomainModel;
using SERVICE = MoneyGram.DLS.Service;

namespace MoneyGram.DLS.EntityMapper.ServiceModelExtensions
{
    public static class HeaderExtensions
    {
        public static DOMAIN.Header ToDomain(this SERVICE.Header req)
        {
            var domainModel = DLSMapper.Map<SERVICE.Header, DOMAIN.Header>(req)
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
            DLSMapper.CreateMap<SERVICE.Header, DOMAIN.Header>()
                .ForMember(dm => dm.Agent, opt => opt.MapFrom(svs => svs.agent.ToDomain()))
                .ForMember(dm => dm.ClientHeader, opt => opt.MapFrom(svs => svs.clientHeader.ToDomain()))
                .ForMember(dm => dm.ProcessingInstruction, opt => opt.MapFrom(svs => svs.processingInstruction.ToDomain()))
                .ForMember(dm => dm.RoutingContext, opt => opt.MapFrom(svs => svs.routingContext.ToDomain()))
                .ForMember(dm => dm.Security, opt => opt.MapFrom(svs => svs.security.ToDomain()));
        }
    }
}