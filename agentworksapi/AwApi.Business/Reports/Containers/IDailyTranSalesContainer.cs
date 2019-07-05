using AwApi.ViewModels.Reports;

namespace AwApi.Business.Reports.Containers
{
    public interface IDailyTranSalesContainer
    {
        ReportResponse GetContainer(ReportRequest reportRequest);
    }
}