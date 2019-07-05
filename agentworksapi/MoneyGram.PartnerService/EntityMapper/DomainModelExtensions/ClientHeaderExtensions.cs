using DOMAIN = MoneyGram.PartnerService.DomainModel;
using SERVICE = MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService.EntityMapper.DomainModelExtensions
{
    public static class ClientHeaderExtensions
    {
        public static SERVICE.ClientHeader ToService(this DOMAIN.ClientHeader req)
        {
            var serviceModel = PsMapper.Map<DOMAIN.ClientHeader, SERVICE.ClientHeader>(req)
                .AdditionalOperations();
            return serviceModel;
        }

        public static SERVICE.ClientHeader AdditionalOperations(this SERVICE.ClientHeader svcModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(svcModel);
            return svcModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<DOMAIN.ClientHeader, SERVICE.ClientHeader>();
        }
    }
}