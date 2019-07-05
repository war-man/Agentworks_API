using System;
using System.Collections.Generic;
using AwApi.EntityMapper.ReportsVmExtensions;
using AwApi.Integration.Reports;
using AwApi.ViewModels.Reports;
using MoneyGram.PartnerService.DomainModel;

namespace AwApi.Business.Reports.Containers
{
    public class UserDetailsContainer : IUserDetailsContainer
    {
        private IPartnerServiceIntegration _partnerIntegration;

        public UserDetailsContainer(IPartnerServiceIntegration partnerIntegration)
        {
            _partnerIntegration = partnerIntegration;
        }

        public ReportResponse GetContainer(ReportRequest reportRequest)
        {
            var reportContainer = ReportResponseFactory.Create(ReportType.DailyTranDetail,  reportRequest);

            //DomainTranformation
            var userReportRequest = reportRequest.ToUserReportsRequestModel();

            var userReport = new Report(ReportConstants.Common.USER_REPORT_NAME);
            var userReportResponseList = _partnerIntegration.GetUserReportsInfo(userReportRequest);
            foreach (UserReportsInfoResponse userReportsResponse in userReportResponseList.UserReportsInfoList)
            {
                var row = new Dictionary<string, string>();
                row.Add(ReportConstants.UserDetails.ACTIVE_USER_FLAG, string.IsNullOrEmpty(userReportsResponse.ActiveUserFlag) ? string.Empty : userReportsResponse.ActiveUserFlag);
                row.Add(ReportConstants.UserDetails.ACTIVITY_TYPE, string.IsNullOrEmpty(userReportsResponse.ActivityType) ? string.Empty : userReportsResponse.ActivityType);
                row.Add(ReportConstants.Common.AGENT_ID, Convert.ToString(userReportsResponse.AgentId));
                row.Add(ReportConstants.Common.AGENT_NAME, string.IsNullOrEmpty(userReportsResponse.AgentName) ? string.Empty : userReportsResponse.AgentName);
                row.Add(ReportConstants.UserDetails.DEVICE_NAME, string.IsNullOrEmpty(userReportsResponse.DeviceName) ? string.Empty : userReportsResponse.DeviceName);
                row.Add(ReportConstants.UserDetails.EDIR_GUID, string.IsNullOrEmpty(userReportsResponse.EdirGuid) ? string.Empty : userReportsResponse.EdirGuid);
                row.Add(ReportConstants.Common.FIRST_NAME, string.IsNullOrEmpty(userReportsResponse.FirstName) ? string.Empty : userReportsResponse.FirstName);
                row.Add(ReportConstants.UserDetails.LAST_LOGON_LCL_DATE, (userReportsResponse.LastLogonLclDate == null) ? string.Empty : userReportsResponse.LastLogonLclDate.ToString("yyyy MMM dd"));
                row.Add(ReportConstants.Common.LAST_NAME, string.IsNullOrEmpty(userReportsResponse.LastName) ? string.Empty : userReportsResponse.LastName);
                row.Add(ReportConstants.UserDetails.LDAP_USER_ID, string.IsNullOrEmpty(userReportsResponse.LdapUserId) ? string.Empty : userReportsResponse.LdapUserId);
                row.Add(ReportConstants.UserDetails.POS_NUMBER, userReportsResponse.PosNumber.ToString());
                userReport.Rows.Add(row);
            }
            reportContainer.Reports.Add(userReport);

            return reportContainer;
        }
    }
}