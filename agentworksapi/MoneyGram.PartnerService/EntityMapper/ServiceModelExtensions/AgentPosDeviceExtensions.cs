using DOMAIN = MoneyGram.PartnerService.DomainModel;
using SERVICE = MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService.EntityMapper.ServiceModelExtensions
{
    public static class AgentPosDeviceExtensions
    {
        public static DOMAIN.AgentPosDevice ToDomain(this SERVICE.AgentPosDevice req)
        {
            var domainModel = PsMapper.Map<SERVICE.AgentPosDevice, DOMAIN.AgentPosDevice>(req)
                .AdditionalOperations();
            return domainModel;
        }

        public static DOMAIN.AgentPosDevice AdditionalOperations(this DOMAIN.AgentPosDevice dmModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(dmModel);
            return dmModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<SERVICE.AgentPosDevice, DOMAIN.AgentPosDevice>()
                .ForMember(dm => dm.PosNumberFieldSpecified, opt => opt.MapFrom(svs => svs.posNumberSpecified));
        }
    }
}
