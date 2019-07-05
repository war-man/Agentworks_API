using AwApi.ViewModels.Reports;

namespace AwApi.Business.Reports.Containers
{
    public interface IUserDetailsContainer
    {
        ReportResponse GetContainer(ReportRequest reportRequest);
    }
}