using DOMAIN = MoneyGram.PartnerService.DomainModel;
using SERVICE = MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService.EntityMapper.ServiceModelExtensions
{
    public static class TransactionExceedReportsInfoExtensions
    {
        public static DOMAIN.TransactionExceedReportsInfo ToDomain(this SERVICE.TransactionExceedReportsInfo req)
        {
            var domainModel = PsMapper.Map<SERVICE.TransactionExceedReportsInfo, DOMAIN.TransactionExceedReportsInfo>(req)
                .AdditionalOperations();
            return domainModel;
        }

        public static DOMAIN.TransactionExceedReportsInfo AdditionalOperations(this DOMAIN.TransactionExceedReportsInfo dmModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(dmModel);
            return dmModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<SERVICE.TransactionExceedReportsInfo, DOMAIN.TransactionExceedReportsInfo>()
                .ForMember(dm => dm.EventTranEvntDateFieldSpecified, opt => opt.MapFrom(svs => svs.eventTranEvntDateSpecified))
                .ForMember(dm => dm.EventTranEvntLclDateField, opt => opt.MapFrom(svs => svs.eventTranEvntLclDate));
        }
    }
}
