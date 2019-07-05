using AwApi.EntityMapper;
using AwApi.Integration;
using AwApi.ViewModels;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.Common.Extensions;

namespace AwApi.Business
{
    public class DocumentBusiness : IDocumentBusiness
    {
        protected readonly IAgentConnectIntegration agentConnectIntegration;

        public DocumentBusiness(IAgentConnectIntegration agentConnectIntegration)
        {
            agentConnectIntegration.ThrowIfNull(nameof(agentConnectIntegration));

            this.agentConnectIntegration = agentConnectIntegration;
        }

        public AcApiResponse<SaveTransactionDocumentResponse, ApiData> SaveTransactionDocument(SaveTransactionDocumentRequest req)
        {
            //AgentConnect TransactionLookup call all flows
            var resp = agentConnectIntegration.SaveTransactionDocument(req);

            var apiResp = new AcApiResponse<SaveTransactionDocumentResponse, ApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(resp.Payload?.Flags, DataSource.AgentConnect),
                ResponseData = resp
            };

            return apiResp;
        }
    }
}
