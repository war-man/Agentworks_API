using AwApi.Business.Reports.Generators;
using AwApi.EntityMapper.ReportsVmExtensions;
using AwApi.ViewModels.Reports;

namespace AwApi.Business.Reports.Containers
{
    public class DailyTranDetailContainer : IDailyTranDetailContainer
    {
        private readonly IDailyTranDetailReportGenerator _dailyTranDetailReportGenerator;
        private readonly IBillPaymentSummaryGenerator _billPaymentSummaryGenerator;
        private readonly IReceiveSummaryGenerator _receiveSummaryGenerator;
        private readonly ISendSummaryGenerator _sendSummaryGenerator;

        public DailyTranDetailContainer(IDailyTranDetailReportGenerator dailyTranDetailReportGenerator, IBillPaymentSummaryGenerator billPaymentSummaryGenerator, IReceiveSummaryGenerator receiveSummaryGenerator, ISendSummaryGenerator sendSummaryGenerator)
        {
            _dailyTranDetailReportGenerator = dailyTranDetailReportGenerator;
            _billPaymentSummaryGenerator = billPaymentSummaryGenerator;
            _receiveSummaryGenerator = receiveSummaryGenerator;
            _sendSummaryGenerator = sendSummaryGenerator;
        }

        public ReportResponse<DailyTranDetailReportResponsePayload> GenerateReport(ReportRequest reportRequest)
        {
            var reportContainer = ReportResponseFactory.Create<DailyTranDetailReportResponsePayload>(ReportType.DailyTranDetail, reportRequest);

            //DomainTransformation
            var bpTranLookupReq = reportRequest.ToBPTranDetailLookupModel();
            var mtTranLookupReq = reportRequest.ToMTTranDetailLookupModel();

            //Call required Repos
            var sendReport = _dailyTranDetailReportGenerator.SendReport(mtTranLookupReq, reportRequest.PosIds);
            var receiveReport = _dailyTranDetailReportGenerator.ReceiveReport(mtTranLookupReq, reportRequest.PosIds);
            var billPayReport = _dailyTranDetailReportGenerator.BillPayReport(bpTranLookupReq, reportRequest.PosIds);
            var sendTotalReport = _sendSummaryGenerator.GenerateSummary(sendReport);
            var receiveTotalReport = _receiveSummaryGenerator.GenerateSummary(receiveReport);
            var billPayTotalReport = _billPaymentSummaryGenerator.GenerateSummary(billPayReport);

            reportContainer.Payload = new DailyTranDetailReportResponsePayload
            {
                BillPayReportDetails = billPayReport,
                BillPayTotalReportDetails = billPayTotalReport,
                ReceiveReportDetails = receiveReport,
                ReceiveTotalReportDetails = receiveTotalReport,
                SendReportDetails = sendReport,
                SendTotalReportDetails = sendTotalReport
            };

            return reportContainer;
        }
    }
}