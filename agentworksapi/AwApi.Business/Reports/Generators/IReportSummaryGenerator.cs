using AwApi.ViewModels.Reports;

namespace AwApi.Business.Reports.Generators
{
    public interface IReportSummaryGenerator
    {
        Report TotalReport(Report detailedReport);
        Report SummaryReport(Report detailedReport);
    }
}