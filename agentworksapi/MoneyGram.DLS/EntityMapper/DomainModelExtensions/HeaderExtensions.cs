using DOMAIN = MoneyGram.DLS.DomainModel;
using SERVICE = MoneyGram.DLS.Service;

namespace MoneyGram.DLS.EntityMapper.DomainModelExtensions
{
    public static class HeaderExtensions
    {
        public static SERVICE.Header ToService(this DOMAIN.Header req)
        {
            var serviceModel = DLSMapper.Map<DOMAIN.Header, SERVICE.Header>(req)
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
            DLSMapper.CreateMap<DOMAIN.Header, SERVICE.Header>()
                .ForMember(svc => svc.agent, opt => opt.MapFrom(dm => dm.Agent.ToService()))
                .ForMember(svc => svc.clientHeader, opt => opt.MapFrom(dm => dm.ClientHeader.ToService()))
                .ForMember(svc => svc.processingInstruction, opt => opt.MapFrom(dm => dm.ProcessingInstruction.ToService()))
                .ForMember(svc => svc.routingContext, opt => opt.MapFrom(dm => dm.RoutingContext.ToService()))
                .ForMember(svc => svc.security, opt => opt.MapFrom(dm => dm.Security.ToService()));
        }
    }
}