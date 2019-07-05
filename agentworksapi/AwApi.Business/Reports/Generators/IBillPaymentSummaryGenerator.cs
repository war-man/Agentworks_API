using AwApi.ViewModels.Reports;
using System.Collections.Generic;

namespace AwApi.Business.Reports.Generators
{
    public interface IBillPaymentSummaryGenerator
    {
        List<BillPayTotalReportDetail> GenerateSummary(List<BillPayReportDetail> reportDetails);
    }
}