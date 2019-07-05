using DOMAIN = MoneyGram.PartnerService.DomainModel;
using SERVICE = MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService.EntityMapper.DomainModelExtensions
{
    public static class SecurityHeaderExtensions
    {
        public static SERVICE.SecurityHeader ToService(this DOMAIN.SecurityHeader req)
        {
            var serviceModel = PsMapper.Map<DOMAIN.SecurityHeader, SERVICE.SecurityHeader>(req)
                .AdditionalOperations();
            return serviceModel;
        }

        public static SERVICE.SecurityHeader AdditionalOperations(this SERVICE.SecurityHeader svcModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(svcModel);
            return svcModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<DOMAIN.SecurityHeader, SERVICE.SecurityHeader>();
        }
    }
}