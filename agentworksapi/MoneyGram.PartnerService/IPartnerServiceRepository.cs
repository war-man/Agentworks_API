using System.Threading.Tasks;
using MoneyGram.PartnerService.DomainModel;
using MoneyGram.PartnerService.DomainModel.Request;
using MoneyGram.PartnerService.DomainModel.Response;


namespace MoneyGram.PartnerService
{
    /// <summary>
    /// Represents calls to PartnerService using the PartnerService Domain model.
    /// </summary>
    public interface IPartnerServiceRepository
    {
        Task<UserReportsInfoResponseList> GetUserReportsInfoAsync(UserReportsInfoRequest getUserReportsInfoRequest);
        Task<AgentsDeviceNamesResponse> GetAgentsDeviceNamesAsync(AgentsDeviceNamesRequest agentsDeviceNamesRequest);
        Task<UserIdExistsResponse> GetUserIdExistsAsync(UserIdExistsRequest userIdExistsRequest);
        Task<TransactionExceedInfoResponse> GetTransactionExceedInfoAsync(TransactionExceedInfoRequest transactionExceedInfoRequest);
        Task<LocationsForMoResponse> GetLocationsForMoAsync(LocationsForMoRequest locationForMoRequest);
        Task<POELocationsForMoResponse> GetPOELocationsForMoAsync(POELocationsForMoRequest poeLocationForMoRequest);
        Task<AgentLocationMoResponse> GetAgentLocationMoAsync(AgentLocationMoRequest agentLocationMoRequest);
        Task<POEAgentLocationMoResponse> GetPOEAgentLocationMoAsync(POEAgentLocationMoRequest poeAgentLocationMoRequest);
        Task<AgentsResponse> GetAgentsAsync(AgentsRequest agentsRequest);
        Task<PosDeviceResponse> GetPOSDeviceAsync(PosDeviceRequest posDeviceRequest);
        Task<AgentPasswordResponse> GetAgentPasswordAsync(AgentPasswordRequest AgentPasswordRequest);

        UserReportsInfoResponseList  GetUserReportsInfo(UserReportsInfoRequest  getUserReportsInfoRequest);
        AgentsDeviceNamesResponse GetAgentsDeviceNames(AgentsDeviceNamesRequest agentsDeviceNamesRequest);
        UserIdExistsResponse GetUserIdExists(UserIdExistsRequest userIdExistsRequest);
        TransactionExceedInfoResponse GetTransactionExceedInfo(TransactionExceedInfoRequest transactionExceedInfoRequest);
        LocationsForMoResponse GetLocationsForMo(LocationsForMoRequest locationForMoRequest);
        POELocationsForMoResponse GetPOELocationsForMo(POELocationsForMoRequest poeLocationForMoRequest);
        AgentLocationMoResponse GetAgentLocationMo(AgentLocationMoRequest agentLocationMoRequest);
        POEAgentLocationMoResponse GetPOEAgentLocationMo(POEAgentLocationMoRequest poeAgentLocationMoRequest);
        AgentsResponse GetAgents(AgentsRequest agentsRequest);
        PosDeviceResponse GetPOSDevice(PosDeviceRequest posDeviceRequest);
        AgentPasswordResponse GetAgentPassword(AgentPasswordRequest AgentPasswordRequest);
    }
}
