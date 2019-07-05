using System.Threading.Tasks;
using MoneyGram.PartnerService.DomainModel;
using MoneyGram.PartnerService.DomainModel.Request;
using MoneyGram.PartnerService.DomainModel.Response;
using MoneyGram.Common.Extensions;

namespace MoneyGram.PartnerService
{
    public abstract class PartnerServiceDecorator : IPartnerServiceRepository
    {
        private readonly IPartnerServiceRepository _partnerService;

        public PartnerServiceDecorator(IPartnerServiceRepository partnerService)
        {
            partnerService.ThrowIfNull(nameof(partnerService));

            _partnerService = partnerService;
        }

        protected IPartnerServiceRepository PartnerService => _partnerService;


        public Task<UserReportsInfoResponseList> GetUserReportsInfoAsync(UserReportsInfoRequest getUserReportsInfoRequest)
        {
            return _partnerService.GetUserReportsInfoAsync(getUserReportsInfoRequest);
        }

        public Task<AgentsDeviceNamesResponse> GetAgentsDeviceNamesAsync(AgentsDeviceNamesRequest agentsDeviceNamesRequest)
        {
            return _partnerService.GetAgentsDeviceNamesAsync(agentsDeviceNamesRequest);
        }

        public Task<UserIdExistsResponse> GetUserIdExistsAsync(UserIdExistsRequest userIdExistsRequest)
        {
            return _partnerService.GetUserIdExistsAsync(userIdExistsRequest);
        }

        public Task<TransactionExceedInfoResponse> GetTransactionExceedInfoAsync(TransactionExceedInfoRequest transactionExceedInfoRequest)
        {
            return _partnerService.GetTransactionExceedInfoAsync(transactionExceedInfoRequest);
        }

        public Task<LocationsForMoResponse> GetLocationsForMoAsync(LocationsForMoRequest locationForMoRequest)
        {
            return _partnerService.GetLocationsForMoAsync(locationForMoRequest);
        }

        public Task<POELocationsForMoResponse> GetPOELocationsForMoAsync(POELocationsForMoRequest poeLocationForMoRequest)
        {
            return _partnerService.GetPOELocationsForMoAsync(poeLocationForMoRequest);
        }

        public Task<AgentLocationMoResponse> GetAgentLocationMoAsync(AgentLocationMoRequest agentLocationMoRequest)
        {
            return _partnerService.GetAgentLocationMoAsync(agentLocationMoRequest);
        }

        public Task<POEAgentLocationMoResponse> GetPOEAgentLocationMoAsync(POEAgentLocationMoRequest poeAgentLocationMoRequest)
        {
            return _partnerService.GetPOEAgentLocationMoAsync(poeAgentLocationMoRequest);
        }

        public Task<AgentsResponse> GetAgentsAsync(AgentsRequest agentsRequest)
        {
            return _partnerService.GetAgentsAsync(agentsRequest);
        }

        public Task<PosDeviceResponse> GetPOSDeviceAsync(PosDeviceRequest posDeviceRequest)
        {
            return _partnerService.GetPOSDeviceAsync(posDeviceRequest);
        }

        public Task<AgentPasswordResponse> GetAgentPasswordAsync(AgentPasswordRequest AgentPasswordRequest)
        {
            return _partnerService.GetAgentPasswordAsync(AgentPasswordRequest);
        }

        public virtual AgentLocationMoResponse GetAgentLocationMo(AgentLocationMoRequest agentLocationMoRequest)
        {
            return _partnerService.GetAgentLocationMo(agentLocationMoRequest);
        }
                
        public virtual POEAgentLocationMoResponse GetPOEAgentLocationMo(POEAgentLocationMoRequest poeAgentLocationMoRequest)
        {
            return _partnerService.GetPOEAgentLocationMo(poeAgentLocationMoRequest);
        }

        public virtual AgentPasswordResponse GetAgentPassword(AgentPasswordRequest AgentPasswordRequest)
        {
            return _partnerService.GetAgentPassword(AgentPasswordRequest);
        }

        public virtual AgentsResponse GetAgents(AgentsRequest agentsRequest)
        {
            return _partnerService.GetAgents(agentsRequest);
        }

        public virtual AgentsDeviceNamesResponse GetAgentsDeviceNames(AgentsDeviceNamesRequest agentsDeviceNamesRequest)
        {
            return _partnerService.GetAgentsDeviceNames(agentsDeviceNamesRequest);
        }

        public virtual LocationsForMoResponse GetLocationsForMo(LocationsForMoRequest locationForMoRequest)
        {
            return _partnerService.GetLocationsForMo(locationForMoRequest);
        }

        public virtual POELocationsForMoResponse GetPOELocationsForMo(POELocationsForMoRequest poeLocationForMoRequest)
        {
            return _partnerService.GetPOELocationsForMo(poeLocationForMoRequest);
        }

        public virtual PosDeviceResponse GetPOSDevice(PosDeviceRequest posDeviceRequest)
        {
            return _partnerService.GetPOSDevice(posDeviceRequest);
        }

        public virtual TransactionExceedInfoResponse GetTransactionExceedInfo(TransactionExceedInfoRequest transactionExceedInfoRequest)
        {
            return _partnerService.GetTransactionExceedInfo(transactionExceedInfoRequest);
        }

        public virtual UserIdExistsResponse GetUserIdExists(UserIdExistsRequest userIdExistsRequest)
        {
            return _partnerService.GetUserIdExists(userIdExistsRequest);
        }

        public virtual UserReportsInfoResponseList GetUserReportsInfo(UserReportsInfoRequest getUserReportsInfoRequest)
        {
            return _partnerService.GetUserReportsInfo(getUserReportsInfoRequest);
        }
    }
}