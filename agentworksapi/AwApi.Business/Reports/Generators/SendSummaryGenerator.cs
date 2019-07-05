using System.Collections.Generic;
using System.Linq;
using AwApi.ViewModels.Reports;

namespace AwApi.Business.Reports.Generators
{
    public class SendSummaryGenerator : ISendSummaryGenerator
    {
        public List<SendTotalReportDetail> GenerateSummary(List<SendReportDetail> reportDetails)
        {
            if (reportDetails == null)
            {
                return null;
            }

            var transactionsByCurrency = reportDetails.GroupBy(tran => tran.Currency);

            return transactionsByCurrency.Select(currencyGroup => new SendTotalReportDetail
            {
                Count = currencyGroup.Count(),
                Currency = currencyGroup.Key,
                FaceAmount = currencyGroup.Select(tran => tran.FaceAmount).Sum(),
                FeeAmount = currencyGroup.Select(tran => tran.FeeAmount).Sum(),
                TotalAmount = currencyGroup.Select(tran => tran.TotalAmount).Sum()
            }).ToList();
        }
    }
}