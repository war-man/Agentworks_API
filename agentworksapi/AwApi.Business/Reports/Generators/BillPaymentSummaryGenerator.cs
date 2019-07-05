using System.Collections.Generic;
using System.Linq;
using AwApi.ViewModels.Reports;

namespace AwApi.Business.Reports.Generators
{
    public class BillPaymentSummaryGenerator : IBillPaymentSummaryGenerator
    {
        public List<BillPayTotalReportDetail> GenerateSummary(List<BillPayReportDetail> reportDetails)
        {
            if (reportDetails == null || reportDetails.Count == 0)
            {
                return new List<BillPayTotalReportDetail>();
            }

            return new List<BillPayTotalReportDetail> {
                new BillPayTotalReportDetail {
                    Count = reportDetails.Count(),
                    Currency = reportDetails.FirstOrDefault()?.Currency,
                    FaceAmount = reportDetails.Select(tran => tran.FaceAmount).Sum(),
                    FeeAmount = reportDetails.Select(tran => tran.FeeAmount).Sum(),
                    TotalAmount = reportDetails.Select(tran => tran.TotalAmount).Sum()
                }
            };
        }
    }
}