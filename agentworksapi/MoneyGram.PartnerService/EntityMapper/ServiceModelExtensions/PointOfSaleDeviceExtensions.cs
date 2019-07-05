using DOMAIN = MoneyGram.PartnerService.DomainModel;
using SERVICE = MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService.EntityMapper.ServiceModelExtensions
{
    public static class PointOfSaleDeviceExtensions
    {
        public static DOMAIN.PointOfSaleDevice ToDomain(this SERVICE.PointOfSaleDevice req)
        {
            var domainModel = PsMapper.Map<SERVICE.PointOfSaleDevice, DOMAIN.PointOfSaleDevice>(req)
                .AdditionalOperations();
            return domainModel;
        }

        public static DOMAIN.PointOfSaleDevice AdditionalOperations(this DOMAIN.PointOfSaleDevice dmModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(dmModel);
            return dmModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<SERVICE.PointOfSaleDevice, DOMAIN.PointOfSaleDevice>();
        }
    }
}
