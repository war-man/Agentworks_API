using AwApi.EntityMapper;
using AwApi.Integration;
using AwApi.ViewModels;
using MoneyGram.Common.Extensions;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace AwApi.Business
{
    public class ReceiveReversalBusiness : IReceiveReversalBusiness
    {
        protected readonly IAgentConnectIntegration _agentConnectIntegration;
        protected readonly ILookupBusiness _lookupBusiness;

        public ReceiveReversalBusiness(IAgentConnectIntegration agentConnectIntegration, ILookupBusiness lookupBusiness)
        {
            agentConnectIntegration.ThrowIfNull(nameof(agentConnectIntegration));
            lookupBusiness.ThrowIfNull(nameof(lookupBusiness));

            _agentConnectIntegration = agentConnectIntegration;
            _lookupBusiness = lookupBusiness;
        }

        public AcApiResponse<ReceiveReversalValidationResponse, ApiData> ReceiveReversalValidation(ReceiveReversalValidationRequest req)
        {
            // AgentConnect Validate call for RECV Reversal
            var resp = _agentConnectIntegration.ReceiveReversalValidation(req);

            var apiResp = new AcApiResponse<ReceiveReversalValidationResponse, ApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(resp.Payload?.Flags, DataSource.AgentConnect),
                ResponseData = resp
            };

            return apiResp;
        }
    }
}