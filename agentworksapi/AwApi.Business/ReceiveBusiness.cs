using AwApi.EntityMapper;
using AwApi.Integration;
using AwApi.ViewModels;
using MoneyGram.Common.Extensions;
using AwApi.Business.Helpers;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace AwApi.Business
{
    public class ReceiveBusiness : IReceiveBusiness
    {
        protected readonly IAgentConnectIntegration _agentConnectIntegration;
        protected readonly IReceiptIntegration _receiptIntegration;
        protected readonly ILookupBusiness _lookupBusiness;

        public ReceiveBusiness(IAgentConnectIntegration agentConnectIntegration, IReceiptIntegration receiptIntegration, ILookupBusiness lookupBusiness)
        {
            agentConnectIntegration.ThrowIfNull(nameof(agentConnectIntegration));
            receiptIntegration.ThrowIfNull(nameof(receiptIntegration));
            lookupBusiness.ThrowIfNull(nameof(lookupBusiness));

            _agentConnectIntegration = agentConnectIntegration;
            _receiptIntegration = receiptIntegration;
            _lookupBusiness = lookupBusiness;
        }

        public AcApiResponse<ReceiveValidationResponse, ReceiptsApiData> ReceiveValidation(ReceiveValidationRequest req)
        {
            // AgentConnect Validate call for RECV
            var resp = _agentConnectIntegration.ReceiveValidation(req);

            var additionalData = ReceiptHelper.GenerateAdditionalDataReceipts(resp.Payload?.Receipts, _receiptIntegration);
            
            // Map: Response Domain->Response View Model
            var apiResp = new AcApiResponse<ReceiveValidationResponse, ReceiptsApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(resp.Payload?.Flags, DataSource.AgentConnect),
                ResponseData = resp,
                AdditionalData = additionalData
            };

            return apiResp;
        }
    }
}