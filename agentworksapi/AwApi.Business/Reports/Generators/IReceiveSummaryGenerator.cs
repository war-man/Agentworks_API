using AwApi.ViewModels.Reports;
using System.Collections.Generic;

namespace AwApi.Business.Reports.Generators
{
    public interface IReceiveSummaryGenerator
    {
        List<ReceiveTotalReportDetail> GenerateSummary(List<ReceiveReportDetail> reportDetails);
    }
}