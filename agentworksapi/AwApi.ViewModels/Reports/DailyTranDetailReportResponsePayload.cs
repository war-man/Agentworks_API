using System.Collections.Generic;

namespace AwApi.ViewModels.Reports
{
    public class DailyTranDetailReportResponsePayload : DailyTranSummaryReportResponsePayload
    {
        public List<SendReportDetail> SendReportDetails { get; set; }
        public List<ReceiveReportDetail> ReceiveReportDetails { get; set; }
        public List<BillPayReportDetail> BillPayReportDetails { get; set; }
    }
}