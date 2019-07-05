using AwApi.EntityMapper;
using AwApi.Integration;
using AwApi.ViewModels;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace AwApi.Business
{
    public class DepositBusiness : IDepositBusiness
    {
        private IAgentConnectIntegration agentConnectIntegration { get; set; }

        public DepositBusiness(IAgentConnectIntegration agentConnectIntegration)
        {
            this.agentConnectIntegration = agentConnectIntegration;
        }

        public AcApiResponse<GetDepositInformationResponse, ApiData> GetDepositInformation(GetDepositInformationRequest req)
        {
            var resp = agentConnectIntegration.GetDepositInformation(req);

            var apiResp = new AcApiResponse<GetDepositInformationResponse, ApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(resp.Payload?.Flags, DataSource.AgentConnect),
                ResponseData = resp
            };

            return apiResp;
        }

        public AcApiResponse<GetDepositBankListResponse, ApiData> GetDepositBankList(GetDepositBankListRequest req)
        {
            var resp = agentConnectIntegration.GetDepositBankList(req);

            var apiResp = new AcApiResponse<GetDepositBankListResponse, ApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(resp.Payload?.Flags, DataSource.AgentConnect),
                ResponseData = resp
            };

            return apiResp;
        }

        public AcApiResponse<DepositAnnouncementResponse, ApiData> DepositAnnouncement(DepositAnnouncementRequest req)
        {
            var resp = agentConnectIntegration.DepositAnnouncement(req);

            var apiResp = new AcApiResponse<DepositAnnouncementResponse, ApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(resp.Payload?.Flags, DataSource.AgentConnect),
                ResponseData = resp
            };

            return apiResp;
        }
    }
}