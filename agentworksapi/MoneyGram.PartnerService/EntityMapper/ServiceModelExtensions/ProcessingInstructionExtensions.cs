using DOMAIN = MoneyGram.PartnerService.DomainModel;
using SERVICE = MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService.EntityMapper.ServiceModelExtensions
{
    public static class ProcessingInstructionExtensions
    {
        public static DOMAIN.ProcessingInstruction ToDomain(this SERVICE.ProcessingInstruction req)
        {
            var domainModel = PsMapper.Map<SERVICE.ProcessingInstruction, DOMAIN.ProcessingInstruction>(req)
                .AdditionalOperations();
            return domainModel;
        }

        public static DOMAIN.ProcessingInstruction AdditionalOperations(this DOMAIN.ProcessingInstruction dmModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(dmModel);
            return dmModel;
        }

        public static void DefineMappings()
        {
            PsMapper.CreateMap<SERVICE.ProcessingInstruction, DOMAIN.ProcessingInstruction>()
                .ForMember(dm => dm.EchoRequestFlagFieldSpecified, opt => opt.MapFrom(svs => svs.echoRequestFlagSpecified))
                .ForMember(dm => dm.InvocationMethodFieldSpecified, opt => opt.MapFrom(svs => svs.invocationMethodSpecified))
                .ForMember(dm => dm.ReadOnlyFlagFieldSpecified, opt => opt.MapFrom(svs => svs.readOnlyFlagSpecified))
                .ForMember(dm => dm.ReturnErrorsAsExceptionFieldSpecified, opt => opt.MapFrom(svs => svs.returnErrorsAsExceptionSpecified));
        }
    }
}
