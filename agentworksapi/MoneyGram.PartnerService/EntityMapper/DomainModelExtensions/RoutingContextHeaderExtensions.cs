using DOMAIN = MoneyGram.PartnerService.DomainModel;
using SERVICE = MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService.EntityMapper.DomainModelExtensions
{
    public static class RoutingContextHeaderExtensions
    {
        public static SERVICE.RoutingContextHeader ToService(this DOMAIN.RoutingContextHeader req)
        {
            var serviceModel = PsMapper.Map<DOMAIN.RoutingContextHeader, SERVICE.RoutingContextHeader>(req)
                .AdditionalOperations();
            return serviceModel;
        }

        public static SERVICE.RoutingContextHeader AdditionalOperations(this SERVICE.RoutingContextHeader svcModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(svcModel);
            return svcModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<DOMAIN.RoutingContextHeader, SERVICE.RoutingContextHeader>();
        }
    }
}