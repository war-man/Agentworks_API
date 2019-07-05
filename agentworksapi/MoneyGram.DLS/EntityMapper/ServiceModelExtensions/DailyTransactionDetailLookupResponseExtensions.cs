using System.Linq;
using DOMAIN = MoneyGram.DLS.DomainModel.Response;
using SERVICE = MoneyGram.DLS.Service;

namespace MoneyGram.DLS.EntityMapper.ServiceModelExtensions
{
    public static class DailyTransactionDetailLookupResponseExtensions
    {
        public static DOMAIN.DailyTransactionDetailLookupResponse ToDomain(this SERVICE.DailyTransactionDetailLookupResponse req)
        {
            var domainModel = DLSMapper.Map<SERVICE.DailyTransactionDetailLookupResponse, DOMAIN.DailyTransactionDetailLookupResponse>(req)
                .AdditionalOperations();
            return domainModel;
        }

        public static DOMAIN.DailyTransactionDetailLookupResponse AdditionalOperations(this DOMAIN.DailyTransactionDetailLookupResponse dmModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(dmModel);
            return dmModel;
        }

        public static void DefineMappings()
        {
            DLSMapper.CreateMap<SERVICE.DailyTransactionDetailLookupResponse, DOMAIN.DailyTransactionDetailLookupResponse>()
                .ForMember(dm => dm.header, opt => opt.MapFrom(svs => svs.header.ToDomain()))
                .ForMember(dm => dm.GetDailyTransactionDetailLookupResultList, opt => opt.MapFrom(svs => svs.getDailyTransactionDetailLookupResultArray != null ? svs.getDailyTransactionDetailLookupResultArray.ToList().ToResponseList() : null));
        }
    }
}