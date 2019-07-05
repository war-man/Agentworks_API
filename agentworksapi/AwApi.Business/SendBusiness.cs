using AwApi.Business.Helpers;
using AwApi.EntityMapper;
using AwApi.Integration;
using AwApi.ViewModels;
using MoneyGram.Common.Extensions;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace AwApi.Business
{
    public class SendBusiness : ISendBusiness
    {
        private readonly IAgentConnectIntegration _agentConnectIntegration;
        private readonly IReceiptIntegration _receiptIntegration;
        private readonly ILookupBusiness _lookupBusiness;

        public SendBusiness(IAgentConnectIntegration agentConnectIntegration, IReceiptIntegration receiptIntegration, ILookupBusiness lookupBusiness)
        {
            agentConnectIntegration.ThrowIfNull(nameof(agentConnectIntegration));
            receiptIntegration.ThrowIfNull(nameof(receiptIntegration));
            lookupBusiness.ThrowIfNull(nameof(lookupBusiness));

            _agentConnectIntegration = agentConnectIntegration;
            _receiptIntegration = receiptIntegration;
            _lookupBusiness = lookupBusiness;
        }

        public AcApiResponse<SendValidationResponse, ReceiptsApiData> SendValidation(SendValidationRequest req)
        {
            //Create request business object for agent connect service call
            req.PrimaryReceiptLanguage = AuthIntegration.GetAgentPrimaryReceiptLanguage();
            req.SecondaryReceiptLanguage = AuthIntegration.GetAgentSecondaryReceiptLanguage();

            // AgentConnect sendValidation call for SEND
            var resp = _agentConnectIntegration.SendValidation(req);

            // Process Receipt
            var additionalData = ReceiptHelper.GenerateAdditionalDataReceipts(resp.Payload?.Receipts, _receiptIntegration);
            
            // Return Response View Model
            var apiResp = new AcApiResponse<SendValidationResponse, ReceiptsApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(resp.Payload?.Flags, DataSource.AgentConnect),
                ResponseData = resp,
                AdditionalData = additionalData
            };

            return apiResp;
        }
    }
}