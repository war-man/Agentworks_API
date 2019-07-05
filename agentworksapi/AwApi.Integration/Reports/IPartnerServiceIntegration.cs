using MoneyGram.Common.Models;
using MoneyGram.PartnerService.DomainModel;
using MoneyGram.PartnerService.DomainModel.Request;
using MoneyGram.PartnerService.DomainModel.Response;

namespace AwApi.Integration.Reports
{
    public interface IPartnerServiceIntegration
    {
        AgentPasswordResponse GetAgentPassword(AgentPasswordRequest agentPasswordRequest);
        HealthCheckResponse HealthCheck();
        UserReportsInfoResponseList GetUserReportsInfo(UserReportsInfoRequest getUserReportsInfoRequest);
        AgentsDeviceNamesResponse GetAgentsDeviceNames(AgentsDeviceNamesRequest agentsDeviceNamesRequest);
        UserIdExistsResponse GetUserIdExists(UserIdExistsRequest userIdExistsRequest);
        TransactionExceedInfoResponse GetTransactionExceedInfo(TransactionExceedInfoRequest transactionExceedInfoRequest);
        AgentsResponse GetAgents(AgentsRequest agentsRequest);
        PosDeviceResponse GetPOSById(PosDeviceRequest posDeviceRequest);
        AgentLocationMoResponse GetAgentLocationsForMo(AgentLocationMoRequest agentLocationMoRequest);
        POEAgentLocationMoResponse GetPOEAgentLocationsForMo(POEAgentLocationMoRequest poeAgentLocationMoRequest);
        LocationsForMoResponse GetLocationsForMo(LocationsForMoRequest locationForMoRequest);
        POELocationsForMoResponse GetPOELocationsForMo(POELocationsForMoRequest poeLocationForMoRequest);
    }
}