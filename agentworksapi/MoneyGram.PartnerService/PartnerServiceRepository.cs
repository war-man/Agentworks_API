using System.Threading.Tasks;
using MoneyGram.Common.Extensions;
using MoneyGram.PartnerService.DomainModel;
using MoneyGram.PartnerService.DomainModel.Request;
using MoneyGram.PartnerService.DomainModel.Response;
using MoneyGram.PartnerService.EntityMapper.DomainModelExtensions;
using MoneyGram.PartnerService.EntityMapper.ServiceModelExtensions;

namespace MoneyGram.PartnerService
{
    public class PartnerServiceRepository : IPartnerServiceRepository
    {
        protected IPartnerServiceProxyFactory PartnerServiceProxyFactory { get { return _proxyFactory; } }

        private readonly IPartnerServiceProxyFactory _proxyFactory = null;

        public PartnerServiceRepository(IPartnerServiceProxyFactory proxyFactory)
        {
            _proxyFactory = proxyFactory;
        }

        #region Public Methods

        /// <summary>
        /// Returns UserReportsInfo for the PartnerService Request...
        /// </summary>
        /// <param name="getUserReportsInfoRequest">getUserReportsInfoRequest</param>
        /// <returns>GetUserReportsInfoResponse</returns>
        public async Task<UserReportsInfoResponseList> GetUserReportsInfoAsync(UserReportsInfoRequest getUserReportsInfoRequest)
        {
            getUserReportsInfoRequest.ThrowIfNull(nameof(getUserReportsInfoRequest));

            var contentResponse = await PartnerServiceProxyFactory.UseService(service => service.getUserReportsInfoAsync(getUserReportsInfoRequest.ToUserReportsInfoRequest1Service()));

            return contentResponse.getUserReportsInfoResponse.ToDomain();
        }

        /// <summary>
        /// Returns agentsDeviceNamesResponse for the PartnerService Request...
        /// </summary>
        /// <param name="agentsDeviceNamesRequest">agentsDeviceNamesRequest</param>
        /// <returns>agentsDeviceNamesResponse</returns>
        public async Task<AgentsDeviceNamesResponse> GetAgentsDeviceNamesAsync(AgentsDeviceNamesRequest agentsDeviceNamesRequest)
        {
            agentsDeviceNamesRequest.ThrowIfNull(nameof(agentsDeviceNamesRequest));

            var deviceNamesResponse = await PartnerServiceProxyFactory.UseService(service => service.getAgentsDeviceNamesAsync(agentsDeviceNamesRequest.ToAgentsDeviceNamesRequest1Service()));

            return deviceNamesResponse.getAgentsDeviceNamesResponse.ToDomain();
        }

        /// <summary>
        /// Returns userIdExistsResponse for the PartnerService Request...
        /// </summary>
        /// <param name="userIdExistsRequest">userIdExistsRequest</param>
        /// <returns>userIdExistsResponse</returns>
        public async Task<UserIdExistsResponse> GetUserIdExistsAsync(UserIdExistsRequest userIdExistsRequest)
        {
            userIdExistsRequest.ThrowIfNull(nameof(userIdExistsRequest));

            var userIdExistsResponse = await PartnerServiceProxyFactory.UseService(service => service.getUserIdExistsAsync(userIdExistsRequest.ToUserIdExistsRequest1Service()));

            return userIdExistsResponse.getUserIdExistsResponse.ToDomain();
        }

        /// <summary>
        /// Returns TransactionExceedInfoResponse for the PartnerService Request...
        /// </summary>
        /// <param name="transactionExceedInfoRequest">transactionExceedInfoRequest</param>
        /// <returns>GetTransactionExceedInfoResponse</returns>
        public async Task<TransactionExceedInfoResponse> GetTransactionExceedInfoAsync(TransactionExceedInfoRequest transactionExceedInfoRequest)
        {
            transactionExceedInfoRequest.ThrowIfNull(nameof(transactionExceedInfoRequest));

            var transactionExceedInfoResponse = await PartnerServiceProxyFactory.UseService(service => service.getTransactionExceedInfoAsync(transactionExceedInfoRequest.ToTransactionExceedInfoRequest1Service()));

            return transactionExceedInfoResponse.getTransactionExceedInfoResponse.ToDomain();
        }

        public async Task<LocationsForMoResponse> GetLocationsForMoAsync(LocationsForMoRequest locationForMoRequest)
        {
            locationForMoRequest.ThrowIfNull(nameof(locationForMoRequest));

            var locationForMoResponse = await PartnerServiceProxyFactory.UseService(service => service.getLocationsForMoAsync(locationForMoRequest.ToLocationsForMoRequest1Service()));

            return locationForMoResponse.getLocationsForMoResponse.ToDomain();
        }

        public async Task<POELocationsForMoResponse> GetPOELocationsForMoAsync(POELocationsForMoRequest poeLocationForMoRequest)
        {
            poeLocationForMoRequest.ThrowIfNull(nameof(poeLocationForMoRequest));

            var poeLocationForMoResponse = await PartnerServiceProxyFactory.UseService(service => service.getPOELocationsForMoAsync(poeLocationForMoRequest.ToPOELocationsForMoRequest1Service()));

            return poeLocationForMoResponse.getPOELocationsForMoResponse.ToDomain();
        }

        public async Task<AgentLocationMoResponse> GetAgentLocationMoAsync(AgentLocationMoRequest agentLocationMoRequest)
        {
            agentLocationMoRequest.ThrowIfNull(nameof(agentLocationMoRequest));

            var agentLocationMoResponse = await PartnerServiceProxyFactory.UseService(service => service.getAgentLocationMoAsync(agentLocationMoRequest.ToService()));

            return agentLocationMoResponse.getAgentLocationMoResponse.ToDomain();
        }
        public async Task<POEAgentLocationMoResponse> GetPOEAgentLocationMoAsync(POEAgentLocationMoRequest poeAgentLocationMoRequest)
        {
            poeAgentLocationMoRequest.ThrowIfNull(nameof(poeAgentLocationMoRequest));
            var poeAgentLocationMoResponse = await PartnerServiceProxyFactory.UseService(service => service.getPOEAgentLocationMoAsync(poeAgentLocationMoRequest.ToService()));
            return poeAgentLocationMoResponse.getPOEAgentLocationMoResponse.ToDomain();
        }

        /// <summary>
        /// Returns AgentsResponse for the PartnerService Request...
        /// </summary>
        /// <param name="agentsRequest">agentsRequest</param>
        /// <returns>AgentsResponse</returns>
        public async Task<AgentsResponse> GetAgentsAsync(AgentsRequest agentsRequest)
        {
            agentsRequest.ThrowIfNull(nameof(agentsRequest));

            var agentsResponse = await PartnerServiceProxyFactory.UseService(service => service.getAgentsAsync(agentsRequest.ToAgentsRequest1Service()));

            return agentsResponse.getAgentsResponse.ToDomain();
        }

        /// <summary>
        /// Returns PosDeviceResponse for the PartnerService Request...
        /// </summary>
        /// <param name="posDeviceRequest">posDeviceRequest</param>
        /// <returns>PosDeviceResponse</returns>
        public async Task<PosDeviceResponse> GetPOSDeviceAsync(PosDeviceRequest posDeviceRequest)
        {
            posDeviceRequest.ThrowIfNull(nameof(posDeviceRequest));

            var posDeviceResponse = await PartnerServiceProxyFactory.UseService(service => service.getPOSDeviceAsync(posDeviceRequest.ToPOSDeviceRequest1Service()));

            return posDeviceResponse.getPOSDeviceResponse.ToDomain();
        }

        /// <summary>
        /// Returns AgentPasswordResponse for the PartnerService Request...
        /// </summary>
        /// <param name="agentPasswordRequest">agentPasswordRequest</param>
        /// <returns>AgentPasswordResponse</returns>
        public async Task<AgentPasswordResponse> GetAgentPasswordAsync(AgentPasswordRequest agentPasswordRequest)
        {
            agentPasswordRequest.ThrowIfNull(nameof(agentPasswordRequest));

            var agentPasswordResponse = await PartnerServiceProxyFactory.UseService(service => service.getAgentPasswordAsync(agentPasswordRequest.ToService()));

            return agentPasswordResponse.getAgentPasswordResponse.ToDomain();
        }

        #endregion Public Methods

        public UserReportsInfoResponseList GetUserReportsInfo(UserReportsInfoRequest getUserReportsInfoRequest)
        {
            Task<UserReportsInfoResponseList> task = Task.Run(() => this.GetUserReportsInfoAsync(getUserReportsInfoRequest));

            task.Wait();

            return task.Result;
        }

        public AgentsDeviceNamesResponse GetAgentsDeviceNames(AgentsDeviceNamesRequest agentsDeviceNamesRequest)
        {
            Task<AgentsDeviceNamesResponse> task = Task.Run(() => this.GetAgentsDeviceNamesAsync(agentsDeviceNamesRequest));

            task.Wait();

            return task.Result;
        }

        public UserIdExistsResponse GetUserIdExists(UserIdExistsRequest userIdExistsRequest)
        {
            Task<UserIdExistsResponse> task = Task.Run(() => this.GetUserIdExistsAsync(userIdExistsRequest));

            task.Wait();

            return task.Result;
        }

        public TransactionExceedInfoResponse GetTransactionExceedInfo(TransactionExceedInfoRequest transactionExceedInfoRequest)
        {
            Task<TransactionExceedInfoResponse> task = Task.Run(() => this.GetTransactionExceedInfoAsync(transactionExceedInfoRequest));

            task.Wait();

            return task.Result;
        }

        public LocationsForMoResponse GetLocationsForMo(LocationsForMoRequest locationForMoRequest)
        {
            Task<LocationsForMoResponse> task = Task.Run(() => this.GetLocationsForMoAsync(locationForMoRequest));

            task.Wait();

            return task.Result;
        }

        public POELocationsForMoResponse GetPOELocationsForMo(POELocationsForMoRequest poeLocationForMoRequest)
        {
            Task<POELocationsForMoResponse> task = Task.Run(() => this.GetPOELocationsForMoAsync(poeLocationForMoRequest));

            task.Wait();

            return task.Result;
        }

        public AgentLocationMoResponse GetAgentLocationMo(AgentLocationMoRequest agentLocationMoRequest)
        {
            Task<AgentLocationMoResponse> task = Task.Run(() => this.GetAgentLocationMoAsync(agentLocationMoRequest));

            task.Wait();

            return task.Result;
        }

        public POEAgentLocationMoResponse GetPOEAgentLocationMo(POEAgentLocationMoRequest poeAgentLocationMoRequest)
        {
            Task<POEAgentLocationMoResponse> task = Task.Run(() => this.GetPOEAgentLocationMoAsync(poeAgentLocationMoRequest));

            task.Wait();

            return task.Result;
        }

        public AgentsResponse GetAgents(AgentsRequest agentsRequest)
        {
            Task<AgentsResponse> task = Task.Run(() => this.GetAgentsAsync(agentsRequest));

            task.Wait();

            return task.Result;
        }

        public PosDeviceResponse GetPOSDevice(PosDeviceRequest posDeviceRequest)
        {
            Task<PosDeviceResponse> task = Task.Run(() => this.GetPOSDeviceAsync(posDeviceRequest));

            task.Wait();

            return task.Result;
        }

        public AgentPasswordResponse GetAgentPassword(AgentPasswordRequest AgentPasswordRequest)
        {
            Task<AgentPasswordResponse> task = Task.Run(() => this.GetAgentPasswordAsync(AgentPasswordRequest));

            task.Wait();

            return task.Result;
        }
    }
}