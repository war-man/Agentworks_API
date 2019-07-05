using System.Linq;
using AwApi.EntityMapper;
using AwApi.Integration;
using AwApi.ViewModels;
using MoneyGram.PartnerHierarchy.DomainModel.Request;

namespace AwApi.Business
{
    public class OperatorBusiness : IOperatorBusiness
    {
        private readonly IPartnerHierarchyIntegration _partnerHierarchyIntegration;

        public OperatorBusiness(IPartnerHierarchyIntegration partnerHierarchyIntegration)
        {
            _partnerHierarchyIntegration = partnerHierarchyIntegration;
        }

        //Following is placeholder code
        public ApiResponse<Operator, ApiData> GetOperator()
        {
            var respVm = AuthIntegration.GetOperator();

            var apiResp = new ApiResponse<Operator, ApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(null, DataSource.Operator),
                ResponseData = respVm
            };

            return apiResp;
        }

        public ApiResponse<Device, ApiData> GetDevice()
        {
            var respVm = AuthIntegration.GetDevice();
            
            var partnerHierarchyResponse =  _partnerHierarchyIntegration.GetPartnerHierarchyAgent(new PartnerHierarchyAgentRequest
            {
                LocationId = AuthIntegration.GetDeviceAgentLocation(),
                MainofficeId = AuthIntegration.GetMainOfficeId()
            });
            
            var agentDetails = partnerHierarchyResponse.GetAgent();
            if (agentDetails != null)
            {
                respVm.AgentStatus = agentDetails.Status;
                respVm.OracleAccountNumber = agentDetails.OracleAccountNumber;
                respVm.IsRetailCredit = agentDetails.IsRetailCredit == "Y";
            }

            var apiResp = new ApiResponse<Device, ApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(null, DataSource.Operator),
                ResponseData = respVm
            };

            return apiResp;
        }
    }
}