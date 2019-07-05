using AwApi.Business.Helpers;
using AwApi.EntityMapper;
using AwApi.Integration;
using AwApi.ViewModels;
using MoneyGram.Common.Extensions;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace AwApi.Business
{
    public class StagedBillPayBusiness : IStagedBillPayBusiness
    {
        protected readonly IAgentConnectIntegration agentConnectIntegration;
        protected readonly IReceiptIntegration receiptIntegration;
        protected readonly ILookupBusiness lookupBusiness;

        public StagedBillPayBusiness(IAgentConnectIntegration agentConnectIntegration, IReceiptIntegration receiptIntegration, ILookupBusiness lookupBusiness)
        {
            agentConnectIntegration.ThrowIfNull(nameof(agentConnectIntegration));
            receiptIntegration.ThrowIfNull(nameof(receiptIntegration));
            lookupBusiness.ThrowIfNull(nameof(lookupBusiness));

            this.agentConnectIntegration = agentConnectIntegration;
            this.receiptIntegration = receiptIntegration;
            this.lookupBusiness = lookupBusiness;
        }

        public AcApiResponse<BPValidationResponse, ReceiptsApiData> BPValidation(BPValidationRequest req)
        {
            //Create request business object for agent connect service call
            req.PrimaryReceiptLanguage = AuthIntegration.GetAgentPrimaryReceiptLanguage();
            req.SecondaryReceiptLanguage = AuthIntegration.GetAgentSecondaryReceiptLanguage();

            // AgentConnect sendValidation call for SEND
            var resp = agentConnectIntegration.BPValidation(req);
            
            // Process Receipt
            var additionalData = ReceiptHelper.GenerateAdditionalDataReceipts(resp.Payload?.Receipts, receiptIntegration);
            
            // Return Response View Model
            var apiResp = new AcApiResponse<BPValidationResponse, ReceiptsApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(resp.Payload?.Flags, DataSource.AgentConnect),
                ResponseData = resp,
                AdditionalData = additionalData
            };

            return apiResp;
        }
    }
}
