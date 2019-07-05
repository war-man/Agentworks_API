using AwApi.ViewModels.Reports;

namespace AwApi.Business.Reports.Containers
{
    public interface ITransactionExceedDetailsContainer
    {
        ReportResponse GetContainer(ReportRequest reportRequest);
    }
}