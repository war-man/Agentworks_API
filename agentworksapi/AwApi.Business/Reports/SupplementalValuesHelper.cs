using System.Collections.Generic;
using AwApi.Integration.Reports;
using MoneyGram.PartnerService.DomainModel.Request;

namespace AwApi.Business.Reports
{
    public static class SupplementalValuesHelper
    {
        public static string GetUserId(string operatorId, IPartnerServiceIntegration _partnerIntegration)
        {
            string userId = string.Empty; ;
            if (!string.IsNullOrEmpty(operatorId))
            {
                UserIdExistsRequest localIdRequest = new UserIdExistsRequest();
                localIdRequest.OperatorId = operatorId;
                //localIdRequest.MainofficeId = decimal.Parse(mainOfficeId);//moved to integraion layer

                MoneyGram.PartnerService.DomainModel.Header header = new MoneyGram.PartnerService.DomainModel.Header();
                MoneyGram.PartnerService.DomainModel.ProcessingInstruction processingInstruction = new MoneyGram.PartnerService.DomainModel.ProcessingInstruction();
                processingInstruction.Action = "GetUserIdExists";
                processingInstruction.RollbackTransaction = false;
                header.ProcessingInstruction = processingInstruction;
                localIdRequest.header = header;

                userId = _partnerIntegration.GetUserIdExists(localIdRequest).UserId;
            }
            return string.IsNullOrEmpty(userId) ? "-" : userId;
        }

        public static string GetComputerName(string agentId, string posId, IPartnerServiceIntegration _partnerIntegration)
        {
            var request = new AgentsDeviceNamesRequest();
            decimal agent = decimal.Parse(agentId);
            decimal pos = decimal.Parse(posId);
            if (string.IsNullOrEmpty(agentId) || string.IsNullOrEmpty(posId))
            {
                return string.Empty;
            }

            MoneyGram.PartnerService.DomainModel.Header header = new MoneyGram.PartnerService.DomainModel.Header();
            MoneyGram.PartnerService.DomainModel.ProcessingInstruction processingInstruction = new MoneyGram.PartnerService.DomainModel.ProcessingInstruction();

            processingInstruction.Action = "GetAgentsDeviceNames";
            processingInstruction.RollbackTransaction = false;

            header.ProcessingInstruction = processingInstruction;
            request.header = header;

            var poslist = new List<MoneyGram.PartnerService.DomainModel.AgentPosDevice>();
            poslist.Add(new MoneyGram.PartnerService.DomainModel.AgentPosDevice() { AgentId = agent, PosNumber = pos, PosNumberFieldSpecified = true });
            request.AgentPosDeviceList = poslist;
            var resp = _partnerIntegration.GetAgentsDeviceNames(request);
            if (resp.AgentPosDeviceListResult == null)
            {
                return "-";
            }
            return resp.AgentPosDeviceListResult.Find(x => x.Name != string.Empty).Name;
        }
    }
}