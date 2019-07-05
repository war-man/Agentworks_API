using System;
using System.Linq;
using AwApi.ViewModels.Reports;
using MoneyGram.DLS.DomainModel.Request;
using MoneyGram.PartnerService.DomainModel;

namespace AwApi.EntityMapper.ReportsVmExtensions
{
    public static class ReportReqVmExtenstions
    {
        public static MTTransactionDetailLookupRequest ToMTTranDetailLookupModel(this ReportRequest reportRequest)
        {
            var req = new MTTransactionDetailLookupRequest();
            var header = new MoneyGram.DLS.DomainModel.Header();
            var processInstruction = new MoneyGram.DLS.DomainModel.ProcessingInstruction();
            req.header = header;
            req.header.ProcessingInstruction = processInstruction;
            req.AgentId = reportRequest.Locations?.First();
            req.header.ProcessingInstruction.Action = "MTTransactionDetailLookup";
            req.PosId = null;
            req.DeliveryOption = reportRequest.DeliveryOption;
            req.EventType = reportRequest.EventType;
            req.StartDate = reportRequest.StartDate;
            return req;
        }

        public static DailyTransactionDetailLookupRequest ToDailyTranDetailLookupModel(this ReportRequest reportRequest)
        {
            var req = new DailyTransactionDetailLookupRequest();
            var header = new MoneyGram.DLS.DomainModel.Header();
            var processInstruction = new MoneyGram.DLS.DomainModel.ProcessingInstruction();
            req.header = header;
            req.header.ProcessingInstruction = processInstruction;
            req.header.ProcessingInstruction.Action = "DailyTransactionDetailLookup";
            req.PosId = null;
            req.AgentId = reportRequest.Locations?.First();
            req.StartDate = reportRequest.StartDate;
            return req;
        }

        public static BPTransactionDetailLookupRequest ToBPTranDetailLookupModel(this ReportRequest reportRequest)
        {
            var req = new BPTransactionDetailLookupRequest();
            var header = new MoneyGram.DLS.DomainModel.Header();
            var processInstruction = new MoneyGram.DLS.DomainModel.ProcessingInstruction();
            req.header = header;
            req.header.ProcessingInstruction = processInstruction;
            req.AgentId = reportRequest.Locations?.First();
            req.header.ProcessingInstruction.Action = "BPTransactionDetailLookup";
            req.PosId = null;
            req.ProductVariant = reportRequest.ProductVariant;
            req.EventType = reportRequest.EventType;
            req.StartDate = reportRequest.StartDate;
            return req;
        }
        public static UserReportsInfoRequest ToUserReportsRequestModel(this ReportRequest reportRequest)
        {
            var req = new UserReportsInfoRequest();
            var header = new MoneyGram.PartnerService.DomainModel.Header();
            var processInstruction = new MoneyGram.PartnerService.DomainModel.ProcessingInstruction();
            req.header = header;
            req.header.ProcessingInstruction = processInstruction;
            req.header.ProcessingInstruction.Action = "GetUserReportsInfo";
            //req.MainOfficeId = System.Convert.ToDecimal(mainOfficeId); will be in the integration layer
            req.Locations = reportRequest.Locations.Select(Convert.ToDecimal).ToArray();
            req.FromDate = System.DateTime.Parse(reportRequest.StartDate);
            req.ToDate = System.DateTime.Parse(reportRequest.EndDate);
            req.FromDateSpecified = true;
            req.ToDateSpecified = true;  
            return req;
        }
        public static TransactionExceedInfoRequest ToTransactionExceedRequestModel(this ReportRequest reportRequest)
        {
            var req = new TransactionExceedInfoRequest();
            var header = new MoneyGram.PartnerService.DomainModel.Header();
            var processInstruction = new MoneyGram.PartnerService.DomainModel.ProcessingInstruction();
            req.header = header;
            req.header.ProcessingInstruction = processInstruction;

            req.header.ProcessingInstruction.Action = "GetTransactionExceedInfo";
            //req.AgentId = System.Convert.ToDecimal(agentId); will be in the integration layer
            req.AgentIdFieldSpecified = true;
            req.TransactionDate = System.Convert.ToDateTime(reportRequest.StartDate);
            req.TransactionDateFieldSpecified = true;
            return req;
        }
        
    }
}