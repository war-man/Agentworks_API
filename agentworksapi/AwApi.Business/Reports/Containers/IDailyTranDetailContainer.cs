using AwApi.ViewModels.Reports;

namespace AwApi.Business.Reports.Containers
{
    public interface IDailyTranDetailContainer
    {
        ReportResponse<DailyTranDetailReportResponsePayload> GenerateReport(ReportRequest reportRequest);
    }
}