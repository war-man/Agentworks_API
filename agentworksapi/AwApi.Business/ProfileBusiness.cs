using System.Collections.Generic;
using AwApi.EntityMapper;
using AwApi.Integration;
using AwApi.ViewModels;
using MoneyGram.Common.Extensions;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace AwApi.Business
{
    public class ProfileBusiness : IProfileBusiness
    {
        private readonly IAgentConnectIntegration _agentConnectIntegration;

        public ProfileBusiness(IAgentConnectIntegration agentConnectIntegration)
        {
            agentConnectIntegration.ThrowIfNull(nameof(agentConnectIntegration));

            _agentConnectIntegration = agentConnectIntegration;
        }

        public AcApiResponse<ProfileResponse, ApiData> Profile(ProfileRequest req)
        {
            var resp = _agentConnectIntegration.Profile(req);

            var apiResp = new AcApiResponse<ProfileResponse, ApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(resp.Payload?.Flags, DataSource.AgentConnect),
                ResponseData = resp
            };

            return apiResp;
        }

        public AcApiResponse<SaveProfileResponse, ApiData> SaveProfile(SaveProfileRequest req)
        {
            var resp = _agentConnectIntegration.SaveProfile(req);

            var apiResp = new AcApiResponse<SaveProfileResponse, ApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(resp.Payload?.Flags, DataSource.AgentConnect),
                ResponseData = resp
            };

            return apiResp;
        }

        public void SaveProfilePrinter(SaveProfilePrinterRequest request)
        {
            request.ThrowIfNull(nameof(request));

            SaveProfile(new SaveProfileRequest
            {
                UnitProfileID = request.UnitProfileID,
                AgentID = request.AgentID,
                AgentSequence = request.AgentSequence,
                Language = request.Language,
                MgiSessionID = request.MgiSessionID,
                TimeStamp = request.TimeStamp,
                ClientSoftwareVersion = request.ClientSoftwareVersion,
                PoeType = request.PoeType,
                ChannelType = request.ChannelType,
                OperatorName = request.OperatorName,
                TargetAudience = request.TargetAudience,
                PoeCapabilities = request.PoeCapabilities,
                ProductProfileItems = new List<ProductProfileItemType>
                {
                    new ProductProfileItemType
                    {
                        Index = 0,
                        Key = "AGENT_PRINTER_NAME",
                        Value = request.PrinterName
                    }
                }
            });
        }
    }
}