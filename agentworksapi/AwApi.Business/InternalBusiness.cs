using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using AwApi.EntityMapper;
using AwApi.EntityMapper.ViewModelExtensions;
using AwApi.Integration;
using AwApi.ViewModels;
using MoneyGram.Common.Extensions;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace AwApi.Business
{
    public class InternalBusiness : IInternalBusiness
    {
        protected readonly IAgentConnectIntegration agentConnectIntegration;
        protected readonly IOpenIdmIntegration openIdmIntegration;

        public InternalBusiness(IAgentConnectIntegration agentConnectIntegration, IOpenIdmIntegration openIdmIntegration)
        {
            agentConnectIntegration.ThrowIfNull(nameof(agentConnectIntegration));
            agentConnectIntegration.ThrowIfNull(nameof(openIdmIntegration));

            this.agentConnectIntegration = agentConnectIntegration;
            this.openIdmIntegration = openIdmIntegration;
        }

        public AcApiResponse<DwInitialSetupResponse, ApiData> DwInitialSetup(DwInitialSetupRequest req)
        {
            var resp = agentConnectIntegration.DwInitialSetup(req);

            var apiResp = new AcApiResponse<DwInitialSetupResponse, ApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(resp.Payload?.Flags, DataSource.AgentConnect),
                ResponseData = resp
            };

            return apiResp;
        }

        public AcApiResponse<DwProfileResponse, ApiData> DwProfile(DwProfileRequest req)
        {
            var resp = agentConnectIntegration.DwProfile(req);

            var apiResp = new AcApiResponse<DwProfileResponse, ApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(resp.Payload?.Flags, DataSource.AgentConnect),
                ResponseData = resp
            };

            return apiResp;
        }

        public DwRegisterDeviceRespVm DwRegisterDevice(DwRegisterDeviceReqVm registerDeviceReqVm)
        {
            var dwInitialreq = new DwInitialSetupRequest
            {
                DeviceID = registerDeviceReqVm.DeviceId,
                Password = registerDeviceReqVm.SetupPin,
                ClientSoftwareVersion = registerDeviceReqVm.ClientSoftwareVersion,
                PoeType = registerDeviceReqVm.PoeType,
                ChannelType = registerDeviceReqVm.ChannelType,
                TargetAudience = registerDeviceReqVm.TargetAudience
            };

            var dwSetup = DwInitialSetup(dwInitialreq);
            var xDoc = XDocument.Parse(dwSetup.ResponseData.Payload.Profile);
            registerDeviceReqVm.MgiDeviceSession = registerDeviceReqVm.MgiDeviceSession;
            registerDeviceReqVm.PosUnitProfileId = int.Parse(GetValue(xDoc, "PROFILE_ID").FirstOrDefault().Value);

            var dwregisterDeviceReqVm = registerDeviceReqVm.ToModel();
            var resp = openIdmIntegration.RegisterDevice(dwregisterDeviceReqVm);
            return new DwRegisterDeviceRespVm
            {
                AgentLocationId = AuthIntegration.GetAgent().AgentId,
                MainOfficeId = AuthIntegration.GetMainOfficeId(),
                AgentName = resp == true ? GetValue(xDoc, "AGENT_NAME").FirstOrDefault().Value : string.Empty,
                AgentAddress1 = resp == true ? GetValue(xDoc, "AGENT_ADDRESS_1").FirstOrDefault().Value : string.Empty,
                AgentAddress2 = resp == true ? GetValue(xDoc, "AGENT_ADDRESS_2").FirstOrDefault().Value : string.Empty,
                AgentAddress3 = resp == true ? GetValue(xDoc, "AGENT_ADDRESS_3").FirstOrDefault().Value : string.Empty,
                AgentCity = resp == true ? GetValue(xDoc, "AGENT_CITY").FirstOrDefault().Value : string.Empty,
                AgentState = resp == true ? GetValue(xDoc, "AGENT_STATE").FirstOrDefault().Value : string.Empty,
                AgentZip = resp == true ? GetValue(xDoc, "AGENT_ZIP").FirstOrDefault().Value : string.Empty,
                AgentPhoneNumber = resp == true ? GetValue(xDoc, "AGENT_PHONE").FirstOrDefault().Value : string.Empty,
                AgentCountry = resp == true ? GetValue(xDoc, "AGENT_COUNTRY").FirstOrDefault().Value : string.Empty,
                AgentTimeZone = resp == true ? GetValue(xDoc, "AGENT_TIME_ZONE").FirstOrDefault().Value : string.Empty,
                Success = resp
            };

        }

        private IEnumerable<XElement> GetValue(XDocument xDoc, string attributeName)
        {
            return from item in xDoc.Root.Elements("ITEM")
                   where (string)item.Attribute("tag") == attributeName
                   select item;
        }
    }
}