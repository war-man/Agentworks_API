using DOMAIN = MoneyGram.DLS.DomainModel;
using SERVICE = MoneyGram.DLS.Service;

namespace MoneyGram.DLS.EntityMapper.ServiceModelExtensions
{
    public static class TransactionDetailLookupResultExtensions
    {
        public static DOMAIN.TransactionDetailLookupResult ToDomain(this SERVICE.TransactionDetailLookupResult req)
        {
            var domainModel = DLSMapper.Map<SERVICE.TransactionDetailLookupResult, DOMAIN.TransactionDetailLookupResult>(req)
                .AdditionalOperations();
            return domainModel;
        }

        public static DOMAIN.TransactionDetailLookupResult AdditionalOperations(this DOMAIN.TransactionDetailLookupResult dmModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(dmModel);
            return dmModel;
        }

        public static void DefineMappings()
        {
            DLSMapper.CreateMap<SERVICE.TransactionDetailLookupResult, DOMAIN.TransactionDetailLookupResult>();
        }
    }
}