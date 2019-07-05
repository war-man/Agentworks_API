using AwApi.ViewModels;
using AwApi.ViewModels.Reports;

namespace AwApi.Business.Reports
{
    public interface IReportsBusiness
    {
        ApiResponse<ReportResponse<DailyTranDetailReportResponsePayload>, ApiData> DailyTranDetailReport(ReportRequest reportRequest);
        ApiResponse<ReportResponse<DailyTranSummaryReportResponsePayload>, ApiData> DailyTranSummaryReport(ReportRequest reportRequest);
    }
}