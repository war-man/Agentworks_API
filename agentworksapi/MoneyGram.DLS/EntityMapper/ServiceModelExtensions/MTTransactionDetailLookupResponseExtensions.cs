using System.Linq;
using DOMAIN = MoneyGram.DLS.DomainModel.Response;
using SERVICE = MoneyGram.DLS.Service;

namespace MoneyGram.DLS.EntityMapper.ServiceModelExtensions
{
    public static class MTTransactionDetailLookupResponseExtensions
    {
        public static DOMAIN.MTTransactionDetailLookupResponse ToDomain(this SERVICE.MTTransactionDetailLookupResponse req)
        {
            var domainModel = DLSMapper.Map<SERVICE.MTTransactionDetailLookupResponse, DOMAIN.MTTransactionDetailLookupResponse>(req)
                .AdditionalOperations();
            return domainModel;
        }

        public static DOMAIN.MTTransactionDetailLookupResponse AdditionalOperations(this DOMAIN.MTTransactionDetailLookupResponse dmModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(dmModel);
            return dmModel;
        }

        public static void DefineMappings()
        {
            DLSMapper.CreateMap<SERVICE.MTTransactionDetailLookupResponse, DOMAIN.MTTransactionDetailLookupResponse>()
                .ForMember(dm => dm.header, opt => opt.MapFrom(svs => svs.header.ToDomain()))
                .ForMember(dm => dm.GetMTTransactionDetailLookupResultList, opt => opt.MapFrom(svs => svs.getMTTransactionDetailLookupResultArray != null ? svs.getMTTransactionDetailLookupResultArray.ToList().ToResponseList() : null));
        }
    }
}