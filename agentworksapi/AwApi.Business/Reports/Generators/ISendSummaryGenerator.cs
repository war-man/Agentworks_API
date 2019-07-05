using AwApi.ViewModels.Reports;
using System.Collections.Generic;

namespace AwApi.Business.Reports.Generators
{
    public interface ISendSummaryGenerator
    {
        List<SendTotalReportDetail> GenerateSummary(List<SendReportDetail> reportDetails);
    }
}