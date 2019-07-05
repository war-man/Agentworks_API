using System;
using MoneyGram.PartnerService;
using MoneyGram.PartnerService.DomainModel;
using MoneyGram.PartnerService.DomainModel.Request;
using MoneyGram.PartnerService.DomainModel.Response;
using MoneyGram.Common.Models;

namespace AwApi.Integration.Reports
{
    public class PartnerServiceIntegration : IPartnerServiceIntegration
    {
        private readonly IPartnerServiceRepository _partnerServiceRepository;

        public PartnerServiceIntegration(IPartnerServiceRepository partnerServiceRepository)
        {
            _partnerServiceRepository = partnerServiceRepository;
        }

        public AgentPasswordResponse GetAgentPassword(AgentPasswordRequest agentPasswordRequest)
        {
            return _partnerServiceRepository.GetAgentPassword(agentPasswordRequest);
        }

        public UserReportsInfoResponseList GetUserReportsInfo(UserReportsInfoRequest getUserReportsInfoRequest)
        {
            getUserReportsInfoRequest.MainOfficeId = System.Convert.ToDecimal(AuthIntegration.GetMainOfficeId());
            return _partnerServiceRepository.GetUserReportsInfo(getUserReportsInfoRequest);
        }

        public AgentsResponse GetAgents(AgentsRequest agentsRequest)
        {
            return _partnerServiceRepository.GetAgents(agentsRequest);
        }

        public PosDeviceResponse GetPOSById(PosDeviceRequest posDeviceRequest)
        {
            return _partnerServiceRepository.GetPOSDevice(posDeviceRequest);
        }

        public AgentLocationMoResponse GetAgentLocationsForMo(AgentLocationMoRequest agentLocationMoRequest)
        {
            return _partnerServiceRepository.GetAgentLocationMo(agentLocationMoRequest);
        }

        public POEAgentLocationMoResponse GetPOEAgentLocationsForMo(POEAgentLocationMoRequest poeAgentLocationMoRequest)
        {
            return _partnerServiceRepository.GetPOEAgentLocationMo(poeAgentLocationMoRequest);
        }

        public LocationsForMoResponse GetLocationsForMo(LocationsForMoRequest locationForMoRequest)
        {
            return _partnerServiceRepository.GetLocationsForMo(locationForMoRequest);
        }

        public POELocationsForMoResponse GetPOELocationsForMo(POELocationsForMoRequest poeLocationForMoRequest)
        {
            return _partnerServiceRepository.GetPOELocationsForMo(poeLocationForMoRequest);
        }

        public AgentsDeviceNamesResponse GetAgentsDeviceNames(AgentsDeviceNamesRequest agentsDeviceNamesRequest)
        {
            return _partnerServiceRepository.GetAgentsDeviceNames(agentsDeviceNamesRequest);
        }
        public UserIdExistsResponse GetUserIdExists(UserIdExistsRequest userIdExistsRequest)
        {
            userIdExistsRequest.MainofficeId = Convert.ToDecimal(AuthIntegration.GetMainOfficeId());
            return _partnerServiceRepository.GetUserIdExists(userIdExistsRequest);
        }
        public TransactionExceedInfoResponse GetTransactionExceedInfo(TransactionExceedInfoRequest transactionExceedInfoRequest)
        {
            transactionExceedInfoRequest.AgentId = Convert.ToDecimal(AuthIntegration.GetAgent().AgentId);
            return _partnerServiceRepository.GetTransactionExceedInfo(transactionExceedInfoRequest);
        }
        
        public HealthCheckResponse HealthCheck()
        {
            return new HealthCheckResponse
            {
                ServiceName = ServiceNames.PartnerService,
                Message = "Not Implemented",
                StatusCode = StatusCode.NotImplemented
            };
        }
    }
}