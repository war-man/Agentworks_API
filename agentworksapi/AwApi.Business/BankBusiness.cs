using AwApi.EntityMapper;
using AwApi.Integration;
using AwApi.ViewModels;
using MoneyGram.Common.Extensions;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace AwApi.Business
{
    public class BankBusiness : IBankBusiness
    {
        protected readonly IAgentConnectIntegration agentConnectIntegration;

        public BankBusiness(IAgentConnectIntegration agentConnectIntegration)
        {
            agentConnectIntegration.ThrowIfNull(nameof(agentConnectIntegration));

            this.agentConnectIntegration = agentConnectIntegration;
        }

        public AcApiResponse<GetBankDetailsByLevelResponse, ApiData> GetBankDetailsByLevel(GetBankDetailsByLevelRequest req)
        {
            var resp = agentConnectIntegration.GetBankDetailsByLevel(req);

            var apiResp = new AcApiResponse<GetBankDetailsByLevelResponse, ApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(resp.Payload?.Flags, DataSource.AgentConnect),
                ResponseData = resp
            };

            return apiResp;
        }

        public AcApiResponse<GetBankDetailsResponse, ApiData> GetBankDetails(GetBankDetailsRequest req)
        {
            var resp = agentConnectIntegration.GetBankDetails(req);

            var apiResp = new AcApiResponse<GetBankDetailsResponse, ApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(resp.Payload?.Flags, DataSource.AgentConnect),
                ResponseData = resp
            };

            return apiResp;
        }
    }
}