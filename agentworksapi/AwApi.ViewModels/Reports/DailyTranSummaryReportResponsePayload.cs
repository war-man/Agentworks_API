using System.Collections.Generic;

namespace AwApi.ViewModels.Reports
{
    public class DailyTranSummaryReportResponsePayload
    {
        public List<SendTotalReportDetail> SendTotalReportDetails { get; set; }
        public List<ReceiveTotalReportDetail> ReceiveTotalReportDetails { get; set; }
        public List<BillPayTotalReportDetail> BillPayTotalReportDetails { get; set; }
    }
}