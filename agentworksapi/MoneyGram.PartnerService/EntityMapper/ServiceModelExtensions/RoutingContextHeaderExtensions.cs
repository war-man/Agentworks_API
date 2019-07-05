using DOMAIN = MoneyGram.PartnerService.DomainModel;
using SERVICE = MoneyGram.PartnerService.Service;


namespace MoneyGram.PartnerService.EntityMapper.ServiceModelExtensions
{
    public static class RoutingContextHeaderExtensions
    {
        public static DOMAIN.RoutingContextHeader ToDomain(this SERVICE.RoutingContextHeader req)
        {
            var domainModel = PsMapper.Map<SERVICE.RoutingContextHeader, DOMAIN.RoutingContextHeader>(req)
                .AdditionalOperations();
            return domainModel;
        }

        public static DOMAIN.RoutingContextHeader AdditionalOperations(this DOMAIN.RoutingContextHeader dmModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(dmModel);
            return dmModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<SERVICE.RoutingContextHeader, DOMAIN.RoutingContextHeader>();
        }
    }
}
