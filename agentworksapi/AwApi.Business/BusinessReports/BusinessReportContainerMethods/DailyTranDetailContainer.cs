using AwApi.Business.BusinessReports.BusinessReportHelperMethods;
using AwApi.Business.BusinessReports.BusinessReportsEnums;
using AwApi.Business.BusinessReports.BusinessReportVmMethods;
using AwApi.Business.BusinessReports.ReportsBusinessConstants;
using AwApi.EntityMapper.ReportsVmExtensions;
using AwApi.Integration;
using AwApi.ViewModels.ReportsViewModels;

namespace AwApi.Business.BusinessReports.BusinessReportContainerMethods
{
    public static class DailyTranDetailContainer
    {
        public static ReportContainerVm GetContainer(ReportRequestVm _reportRequestVm, IPartnerServiceIntegration _partnerIntegration, IDlsIntegration _dlsIntegration, string[] strPosIds)
        {
            var reportContainer = ReportContainerHelper.GetStagedContainer(ReportType.DailyTranDetail, _partnerIntegration, _reportRequestVm);

            //DomainTransformation
            var bpTranLookupReq = _reportRequestVm.ToBPTranDetailLookupModel();
            var mtTranLookupReq = _reportRequestVm.ToMTTranDetailLookupModel();

            //Call required Repos
            var sendReceiveReport = DailyTranDetailViewModel.GetSendReceiveDailyTranDetail(mtTranLookupReq, _dlsIntegration, _partnerIntegration, strPosIds);
            var sendTotalReport = ReportVmHelper.GetSendTotalReport(sendReceiveReport.Find(fitem => fitem.Name == BusinessReportsConstants.Common.SSR_NAME));
            var receiveTotalReport = ReportVmHelper.GetReceiveTotalReport(sendReceiveReport.Find(fitem => fitem.Name == BusinessReportsConstants.Common.RSR_NAME));
            var bpReport = DailyTranDetailViewModel.GetBPDailyTranDetail(bpTranLookupReq, _dlsIntegration, _partnerIntegration, strPosIds);
            var bpSummaryReport = DailyActivitySummaryViewModel.GetBPDailyActivitySummary(bpTranLookupReq, _reportRequestVm.IsDetailed, _dlsIntegration, strPosIds);

            //Add all the report(s)
            reportContainer.Reports.AddRange(sendReceiveReport);
            reportContainer.Reports.Add(sendTotalReport);
            reportContainer.Reports.Add(receiveTotalReport);
            reportContainer.Reports.Add(bpReport);
            reportContainer.Reports.Add(bpSummaryReport);

            return reportContainer;
        }
    }
}