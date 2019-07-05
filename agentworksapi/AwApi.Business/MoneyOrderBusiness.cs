using AwApi.EntityMapper;
using AwApi.Integration;
using AwApi.ViewModels;
using MoneyGram.Common;
using MoneyGram.Common.Extensions;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace AwApi.Business
{
    public class MoneyOrderBusiness : IMoneyOrderBusiness
    {
        protected readonly IAgentConnectIntegration agentConnectIntegration;
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public MoneyOrderBusiness(IAgentConnectIntegration agentConnectIntegration)
        {
            agentConnectIntegration.ThrowIfNull(nameof(agentConnectIntegration));

            this.agentConnectIntegration = agentConnectIntegration;
        }

        public AcApiResponse<MoneyOrderTotalResponse, ApiData> MoneyOrderTotal(MoneyOrderTotalRequest req)
        {
            var resp = agentConnectIntegration.MoneyOrderTotal(req);

            var apiResp = new AcApiResponse<MoneyOrderTotalResponse, ApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(resp.Payload?.Flags, DataSource.AgentConnect),
                ResponseData = resp
            };
            return apiResp;
        }

        public AcApiResponse<ComplianceTransactionResponse, ApiData> ComplianceTransaction(
            ComplianceTransactionRequest req)
        {
            var deviceId = AuthIntegration.GetDeviceId();
            foreach (var moneyOrder in req.MoneyOrder)
            {
                if (deviceId != moneyOrder.DeviceID)
                {
                    logger.Info(string.Format("The DeviceId {0} does not match the logged in DeviceId {1}", deviceId,
                        moneyOrder.DeviceID));
                    throw new InvalidDeviceException();
                }
            }
            var resp = agentConnectIntegration.ComplianceTransaction(req);

            var apiResp = new AcApiResponse<ComplianceTransactionResponse, ApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(resp.Payload?.Flags, DataSource.AgentConnect),
                ResponseData = resp
            };
            return apiResp;
        }
    }
}