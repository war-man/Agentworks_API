using DOMAIN = MoneyGram.DLS.DomainModel;
using SERVICE = MoneyGram.DLS.Service;

namespace MoneyGram.DLS.EntityMapper.DomainModelExtensions
{
    public static class ProcessingInstructionExtensions
    {
        public static SERVICE.ProcessingInstruction ToService(this DOMAIN.ProcessingInstruction req)
        {
            var serviceModel = DLSMapper.Map<DOMAIN.ProcessingInstruction, SERVICE.ProcessingInstruction>(req)
                .AdditionalOperations();
            return serviceModel;
        }

        public static SERVICE.ProcessingInstruction AdditionalOperations(this SERVICE.ProcessingInstruction svcModel)
        {
            DomainTransformExtensions.NullifyWhiteSpaceStrings(svcModel);
            return svcModel;
        }

        public static void DefineMappings()
        {
            DLSMapper.CreateMap<DOMAIN.ProcessingInstruction, SERVICE.ProcessingInstruction>()
                .Ignore(src => src.invocationMethodSpecified)
                .Ignore(src => src.readOnlyFlagSpecified)
                .Ignore(src => src.echoRequestFlagSpecified)
                .Ignore(src => src.returnErrorsAsExceptionSpecified);
        }
    }
}