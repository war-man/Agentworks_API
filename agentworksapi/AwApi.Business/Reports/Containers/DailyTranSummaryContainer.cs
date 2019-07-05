using AwApi.Business.Reports.Generators;
using AwApi.EntityMapper.ReportsVmExtensions;
using AwApi.ViewModels.Reports;

namespace AwApi.Business.Reports.Containers
{
    public class DailyTranSummaryContainer : IDailyTranSummaryContainer
    {
        private readonly IDailyTranDetailReportGenerator _dailyTranDetailReportGenerator;
        private readonly IBillPaymentSummaryGenerator _billPaymentSummaryGenerator;
        private readonly IReceiveSummaryGenerator _receiveSummaryGenerator;
        private readonly ISendSummaryGenerator _sendSummaryGenerator;

        public DailyTranSummaryContainer(IDailyTranDetailReportGenerator dailyTranDetailReportGenerator, IBillPaymentSummaryGenerator billPaymentSummaryGenerator, IReceiveSummaryGenerator receiveSummaryGenerator, ISendSummaryGenerator sendSummaryGenerator)
        {
            _dailyTranDetailReportGenerator = dailyTranDetailReportGenerator;
            _billPaymentSummaryGenerator = billPaymentSummaryGenerator;
            _receiveSummaryGenerator = receiveSummaryGenerator;
            _sendSummaryGenerator = sendSummaryGenerator;
        }
        public ReportResponse<DailyTranSummaryReportResponsePayload> GenerateReport(ReportRequest reportRequest)
        {
            var reportContainer = ReportResponseFactory.Create<DailyTranSummaryReportResponsePayload>(ReportType.DailyTranSummary, reportRequest);

            //Domainmodel Transformation
            var mtTranLookupReq = reportRequest.ToMTTranDetailLookupModel();
            var bpTranLookupReq = reportRequest.ToBPTranDetailLookupModel();

            //Call required Repos
            var sendReport = _dailyTranDetailReportGenerator.SendReport(mtTranLookupReq, reportRequest.PosIds);
            var receiveReport = _dailyTranDetailReportGenerator.ReceiveReport(mtTranLookupReq, reportRequest.PosIds);
            var billPayReport = _dailyTranDetailReportGenerator.BillPayReport(bpTranLookupReq, reportRequest.PosIds);

            var sendSummaryReport = _sendSummaryGenerator.GenerateSummary(sendReport);
            var receiveSummaryReport = _receiveSummaryGenerator.GenerateSummary(receiveReport);
            var billPaySummaryReport = _billPaymentSummaryGenerator.GenerateSummary(billPayReport);

            reportContainer.Payload = new DailyTranSummaryReportResponsePayload
            {
                BillPayTotalReportDetails = billPaySummaryReport,
                ReceiveTotalReportDetails = receiveSummaryReport,
                SendTotalReportDetails = sendSummaryReport
            };

            return reportContainer;
        }
    }
}