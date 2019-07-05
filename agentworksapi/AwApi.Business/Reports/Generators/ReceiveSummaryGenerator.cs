using System.Collections.Generic;
using System.Linq;
using AwApi.ViewModels.Reports;

namespace AwApi.Business.Reports.Generators
{
    public class ReceiveSummaryGenerator : IReceiveSummaryGenerator
    {
        public List<ReceiveTotalReportDetail> GenerateSummary(List<ReceiveReportDetail> reportDetails)
        {
            var transactionsByCurrency = reportDetails?.GroupBy(tran => new { tran.Currency, tran.TransactionType });

            return transactionsByCurrency?.Select(currencyGroup => new ReceiveTotalReportDetail
            {
                TransactionType = currencyGroup.Key.TransactionType,
                Count = currencyGroup.Count(),
                Currency = currencyGroup.Key.Currency,
                TotalAmount = currencyGroup.Select(tran => tran.TotalAmount).Sum()
            }).ToList();
        }


    }
}