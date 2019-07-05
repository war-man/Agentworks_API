using System;
using System.Collections.Generic;
using System.Linq;
using AwApi.Integration;
using AwApi.ViewModels.Reports;

namespace AwApi.Business.Reports
{
    public static class ReportResponseFactory
    {
        public static ReportResponse<T> Create<T>(ReportType reportType, ReportRequest reportRequest)
        {
            var agentId = reportRequest.Locations?.First();
            var agentName = AuthIntegration.GetAgentName();
            var reportName = reportType == ReportType.DailyTranDetail ? ReportConstants.Common.DETAIL_REPORT_NAME : ReportConstants.Common.SUMMARY_REPORT_NAME;

            return new ReportResponse<T>
            {
                Name = reportName,
                ReportDateTime = DateTime.UtcNow,
                Metadata = new ReportMetadata
                {
                    ActivityDate = DateTime.Parse(reportRequest.StartDate),
                    AgentId = agentId,
                    AgentName = agentName
                }
            };
        }
    }
}