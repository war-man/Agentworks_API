using DOMAIN = MoneyGram.DLS.DomainModel;
using SERVICE = MoneyGram.DLS.Service;

namespace MoneyGram.DLS.EntityMapper.DomainModelExtensions
{
    public static class RoutingContextHeaderExtensions
    {
        public static SERVICE.RoutingContextHeader ToService(this DOMAIN.RoutingContextHeader req)
        {
            var serviceModel = DLSMapper.Map<DOMAIN.RoutingContextHeader, SERVICE.RoutingContextHeader>(req)
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
            DLSMapper.CreateMap<DOMAIN.RoutingContextHeader, SERVICE.RoutingContextHeader>();
        }
    }
}