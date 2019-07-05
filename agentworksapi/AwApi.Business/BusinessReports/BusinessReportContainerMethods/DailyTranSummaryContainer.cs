using AwApi.Business.BusinessReports.BusinessReportHelperMethods;
using AwApi.Business.BusinessReports.BusinessReportsEnums;
using AwApi.Business.BusinessReports.BusinessReportVmMethods;
using AwApi.Business.BusinessReports.ReportsBusinessConstants;
using AwApi.EntityMapper.ReportsVmExtensions;
using AwApi.Integration;
using AwApi.ViewModels.ReportsViewModels;

namespace AwApi.Business.BusinessReports.BusinessReportContainerMethods
{
    public static class DailyTranSummaryContainer
    {
        public static ReportContainerVm GetContainer(ReportRequestVm _reportRequestVm, IPartnerServiceIntegration _partnerIntegration, IDlsIntegration _dlsIntegration, string[] strPosIds)
        {
            var reportContainer = ReportContainerHelper.GetStagedContainer(ReportType.DailyTranSummary, _partnerIntegration, _reportRequestVm);

            //Domainmodel Transformation
            var bpTranLookupReq = _reportRequestVm.ToBPTranDetailLookupModel();

            //Call required Repos
            var bPSummary = DailyActivitySummaryViewModel.GetBPDailyActivitySummary(bpTranLookupReq, _reportRequestVm.IsDetailed, _dlsIntegration, strPosIds);
            var mtTranLookupReq = _reportRequestVm.ToMTTranDetailLookupModel();
            var sendReceiveReport = DailyTranDetailViewModel.GetSendReceiveDailyTranDetail(mtTranLookupReq, _dlsIntegration, _partnerIntegration, strPosIds);
            var sendTranSummary = ReportVmHelper.GetSendSummaryReport(sendReceiveReport.Find(fitem => fitem.Name == BusinessReportsConstants.Common.SSR_NAME));
            var receiveTranSummary = ReportVmHelper.GetReceiveSummaryReport(sendReceiveReport.Find(fitem => fitem.Name == BusinessReportsConstants.Common.RSR_NAME));

            //Add all the report(s)
            reportContainer.Reports.Add(sendTranSummary);
            reportContainer.Reports.Add(receiveTranSummary);
            reportContainer.Reports.Add(bPSummary);

            return reportContainer;
        }
    }
}