using System.Linq;
using DOMAIN = MoneyGram.DLS.DomainModel.Response;
using SERVICE = MoneyGram.DLS.Service;

namespace MoneyGram.DLS.EntityMapper.ServiceModelExtensions
{
    public static class BPTransactionDetailLookupResponseExtensions
    {
        public static DOMAIN.BPTransactionDetailLookupResponse ToDomain(this SERVICE.BPTransactionDetailLookupResponse req)
        {
            var domainModel = DLSMapper.Map<SERVICE.BPTransactionDetailLookupResponse, DOMAIN.BPTransactionDetailLookupResponse>(req)
                .AdditionalOperations();
            return domainModel;
        }

        public static DOMAIN.BPTransactionDetailLookupResponse AdditionalOperations(this DOMAIN.BPTransactionDetailLookupResponse dmModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(dmModel);
            return dmModel;
        }

        public static void DefineMappings()
        {
            DLSMapper.CreateMap<SERVICE.BPTransactionDetailLookupResponse, DOMAIN.BPTransactionDetailLookupResponse>()
                .ForMember(dm => dm.header, opt => opt.MapFrom(svs => svs.header.ToDomain()))
                .ForMember(dm => dm.GetDailyTransactionDetailLookupResultList, opt => opt.MapFrom(svs => svs.getBPTransactionDetailLookupResultArray != null ? svs.getBPTransactionDetailLookupResultArray.ToList().ToResponseList() : null));
        }
    }
}