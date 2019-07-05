using DOMAIN = MoneyGram.PartnerService.DomainModel;
using SERVICE = MoneyGram.PartnerService.Service;


namespace MoneyGram.PartnerService.EntityMapper.ServiceModelExtensions
{
    public static class ClientHeaderExtensions
    {
        public static DOMAIN.ClientHeader ToDomain(this SERVICE.ClientHeader req)
        {
            var domainModel = PsMapper.Map<SERVICE.ClientHeader, DOMAIN.ClientHeader>(req)
                .AdditionalOperations();
            return domainModel;
        }

        public static DOMAIN.ClientHeader AdditionalOperations(this DOMAIN.ClientHeader dmModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(dmModel);
            return dmModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<SERVICE.ClientHeader, DOMAIN.ClientHeader>();
        }
    }
}
