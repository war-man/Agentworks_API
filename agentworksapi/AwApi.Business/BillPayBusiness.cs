using AwApi.Integration;
using MoneyGram.Common.Extensions;
using AwApi.EntityMapper;
using AwApi.ViewModels;
using AwApi.Business.Helpers;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace AwApi.Business
{
    public class BillPayBusiness : IBillPayBusiness
    {
        protected readonly IAgentConnectIntegration agentConnectIntegration;
        protected readonly IReceiptIntegration receiptIntegration;
        protected readonly ILookupBusiness lookupBusiness;

        private const string MaxSenders = "10";
        private const string MaxReceivers = "10";

        public BillPayBusiness(IAgentConnectIntegration agentConnectIntegration, IReceiptIntegration receiptIntegration, ILookupBusiness lookupBusiness)
        {
            agentConnectIntegration.ThrowIfNull(nameof(agentConnectIntegration));
            receiptIntegration.ThrowIfNull(nameof(receiptIntegration));
            lookupBusiness.ThrowIfNull(nameof(lookupBusiness));

            this.agentConnectIntegration = agentConnectIntegration;
            this.receiptIntegration = receiptIntegration;
            this.lookupBusiness = lookupBusiness;
        }

        public AcApiResponse<BillerSearchResponse, ApiData> BillerSearch(BillerSearchRequest req)
        {
            var resp = agentConnectIntegration.BillerSearch(req);

            var apiResp = new AcApiResponse<BillerSearchResponse, ApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(resp.Payload?.Flags, DataSource.AgentConnect),
                ResponseData = resp
            };

            return apiResp;
        }

        public AcApiResponse<BPValidationResponse, ReceiptsApiData> BPValidation(BPValidationRequest req)
        {
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