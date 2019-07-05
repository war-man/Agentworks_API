using AwApi.Business.BusinessReports.BusinessReportHelperMethods;
using AwApi.Business.BusinessReports.BusinessReportsEnums;
using AwApi.Business.BusinessReports.ReportsBusinessConstants;
using AwApi.EntityMapper.ReportsVmExtensions;
using AwApi.Integration;
using AwApi.ViewModels.ReportsViewModels;
using MoneyGram.PartnerService.DomainModel;
using System;
using System.Collections.Generic;

namespace AwApi.Business.BusinessReports.BusinessReportContainerMethods
{
    public static class UserDetailsContainer
    {
        public static ReportContainerVm GetContainer(ReportRequestVm _reportRequestVm, IPartnerServiceIntegration _partnerIntegration, IDlsIntegration _dlsIntegration)
        {
            var reportContainer = ReportContainerHelper.GetStagedContainer(ReportType.DailyTranDetail, _partnerIntegration, _reportRequestVm);

            //DomainTranformation
            var userReportRequest = _reportRequestVm.ToUserReportsRequestModel();

            var userReport = new ReportVm(BusinessReportsConstants.Common.USER_REPORT_NAME);
            var userReportResponseList = _partnerIntegration.GetUserReportsInfo(userReportRequest);
            foreach (UserReportsInfoResponse userReportsResponse in userReportResponseList.UserReportsInfoList)
            {
                var row = new Dictionary<string, string>();
                row.Add(BusinessReportsConstants.UserDetails.ACTIVE_USER_FLAG, string.IsNullOrEmpty(userReportsResponse.ActiveUserFlag) ? string.Empty : userReportsResponse.ActiveUserFlag);
                row.Add(BusinessReportsConstants.UserDetails.ACTIVITY_TYPE, string.IsNullOrEmpty(userReportsResponse.ActivityType) ? string.Empty : userReportsResponse.ActivityType);
                row.Add(BusinessReportsConstants.UserDetails.AGENT_ID, Convert.ToString(userReportsResponse.AgentId));
                row.Add(BusinessReportsConstants.UserDetails.AGENT_NAME, string.IsNullOrEmpty(userReportsResponse.AgentName) ? string.Empty : userReportsResponse.AgentName);
                row.Add(BusinessReportsConstants.UserDetails.DEVICE_NAME, string.IsNullOrEmpty(userReportsResponse.DeviceName) ? string.Empty : userReportsResponse.DeviceName);
                row.Add(BusinessReportsConstants.UserDetails.EDIR_GUID, string.IsNullOrEmpty(userReportsResponse.EdirGuid) ? string.Empty : userReportsResponse.EdirGuid);
                row.Add(BusinessReportsConstants.UserDetails.FIRST_NAME, string.IsNullOrEmpty(userReportsResponse.FirstName) ? string.Empty : userReportsResponse.FirstName);
                row.Add(BusinessReportsConstants.UserDetails.LAST_LOGON_LCL_DATE, (userReportsResponse.LastLogonLclDate == null) ? string.Empty : userReportsResponse.LastLogonLclDate.ToString("yyyy MMM dd"));
                row.Add(BusinessReportsConstants.UserDetails.LAST_NAME, string.IsNullOrEmpty(userReportsResponse.LastName) ? string.Empty : userReportsResponse.LastName);
                row.Add(BusinessReportsConstants.UserDetails.LDAP_USER_ID, string.IsNullOrEmpty(userReportsResponse.LdapUserId) ? string.Empty : userReportsResponse.LdapUserId);
                row.Add(BusinessReportsConstants.UserDetails.POS_NUMBER, userReportsResponse.PosNumber.ToString());
                userReport.Rows.Add(row);
            }
            reportContainer.Reports.Add(userReport);

            return reportContainer;
        }
    }
}