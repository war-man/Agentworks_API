using System.Linq;
using DOMAIN = MoneyGram.PartnerService.DomainModel;
using SERVICE = MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService.EntityMapper.ServiceModelExtensions
{
    public static class TransactionExceedInfoResponseExtensions
    {
        public static DOMAIN.TransactionExceedInfoResponse ToDomain(this SERVICE.GetTransactionExceedInfoResponse req)
        {
            var domainModel = PsMapper.Map<SERVICE.GetTransactionExceedInfoResponse, DOMAIN.TransactionExceedInfoResponse>(req)
                .AdditionalOperations();
            return domainModel;
        }

        public static DOMAIN.TransactionExceedInfoResponse AdditionalOperations(this DOMAIN.TransactionExceedInfoResponse dmModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(dmModel);
            return dmModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<SERVICE.GetTransactionExceedInfoResponse, DOMAIN.TransactionExceedInfoResponse>()
                .ForMember(dm => dm.transactionExceedReportsInfoList, opt => opt.MapFrom(svs => svs.transactionExceedReportsInfoList != null ? svs.transactionExceedReportsInfoList.ToList().ToTransactionExceedInfoResponseList() : null))
                .ForMember(dm => dm.header, opt => opt.MapFrom(svs => svs.header.ToDomain()));
        }
    }
}
