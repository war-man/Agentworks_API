using AwApi.Business.Helpers;
using AwApi.EntityMapper;
using AwApi.Integration;
using AwApi.ViewModels;
using MoneyGram.Common.Extensions;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace AwApi.Business
{
    public class SendReversalBusiness : ISendReversalBusiness
    {
        protected readonly IAgentConnectIntegration _agentConnectIntegration;
        protected readonly IReceiptIntegration _receiptIntegration;
        protected readonly ILookupBusiness _lookupBusiness;

        public SendReversalBusiness(IAgentConnectIntegration agentConnectIntegration, IReceiptIntegration receiptIntegration, ILookupBusiness lookupBusiness)
        {
            agentConnectIntegration.ThrowIfNull(nameof(agentConnectIntegration));
            receiptIntegration.ThrowIfNull(nameof(receiptIntegration));
            receiptIntegration.ThrowIfNull(nameof(lookupBusiness));

            _agentConnectIntegration = agentConnectIntegration;
            _receiptIntegration = receiptIntegration;
            _lookupBusiness = lookupBusiness;
        }
        public AcApiResponse<SendReversalValidationResponse, ReceiptsApiData> SendReversalValidation(SendReversalValidationRequest req)
        {
            // AgentConnect SendReversal call for SEND
            var resp = _agentConnectIntegration.SendReversalValidation(req);

            // Process Receipt
            var additionalData = ReceiptHelper.GenerateAdditionalDataReceipts(resp.Payload?.Receipts, _receiptIntegration);
            
            // Return Response View Model
            var apiResp = new AcApiResponse<SendReversalValidationResponse, ReceiptsApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(resp.Payload?.Flags, DataSource.AgentConnect),
                ResponseData = resp,
                AdditionalData = additionalData
            };

            return apiResp;
        }
    }
}