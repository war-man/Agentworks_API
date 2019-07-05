using AwApi.Business.Helpers;
using AwApi.EntityMapper;
using AwApi.Integration;
using AwApi.ViewModels;
using MoneyGram.Common.Extensions;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace AwApi.Business
{
    public class AmendBusiness : IAmendBusiness
    {
        protected readonly IAgentConnectIntegration agentConnectIntegration;
        protected readonly IReceiptIntegration receiptIntegration;
        protected readonly ILookupBusiness lookupBusiness;

        public AmendBusiness(IAgentConnectIntegration agentConnectIntegration, IReceiptIntegration receiptIntegration, ILookupBusiness lookupBusiness)
        {
            agentConnectIntegration.ThrowIfNull(nameof(agentConnectIntegration));
            receiptIntegration.ThrowIfNull(nameof(receiptIntegration));
            lookupBusiness.ThrowIfNull(nameof(lookupBusiness));

            this.agentConnectIntegration = agentConnectIntegration;
            this.receiptIntegration = receiptIntegration;
            this.lookupBusiness = lookupBusiness;
        }

        public AcApiResponse<AmendValidationResponse, ApiData> AmendValidation(AmendValidationRequest req)
        {
            var resp = agentConnectIntegration.AmendValidation(req);

            // Process Receipt
            var additionalData = ReceiptHelper.GenerateAdditionalDataReceipts(resp.Payload?.Receipts, receiptIntegration);

            var apiResp = new AcApiResponse<AmendValidationResponse, ApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(resp.Payload?.Flags, DataSource.AgentConnect),
                ResponseData = resp,
                AdditionalData = additionalData
            };
            return apiResp;
        }
    }
}