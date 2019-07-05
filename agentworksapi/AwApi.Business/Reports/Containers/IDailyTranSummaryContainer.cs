using AwApi.ViewModels.Reports;

namespace AwApi.Business.Reports.Containers
{
    public interface IDailyTranSummaryContainer
    {
        ReportResponse<DailyTranSummaryReportResponsePayload> GenerateReport(ReportRequest reportRequest);
    }
}