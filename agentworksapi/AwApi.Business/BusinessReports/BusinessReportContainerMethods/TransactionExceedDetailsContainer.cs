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
    public static class TransactionExceedDetailsContainer
    {
        public static ReportContainerVm GetContainer(ReportRequestVm _reportRequestVm, IPartnerServiceIntegration _partnerIntegration, IDlsIntegration _dlsIntegration)
        {
            var reportContainer = ReportContainerHelper.GetStagedContainer(ReportType.DailyTranDetail, _partnerIntegration, _reportRequestVm);

            //DomainTranformation
            var tranExceedRequest = _reportRequestVm.ToTransactionExceedRequestModel();

            var transactionExceedReport = new ReportVm(BusinessReportsConstants.Common.TRANEXCEED_REPORT_NAME);

            //Call repo
            var transactionExceedInfoResponse = _partnerIntegration.GetTransactionExceedInfo(tranExceedRequest);

            foreach (TransactionExceedReportsInfo item in transactionExceedInfoResponse.transactionExceedReportsInfoList)
            {
                var row = new Dictionary<string, string>();
                row.Add(BusinessReportsConstants.TransactionExceedDetails.AGENT_ID, Convert.ToString(item.AgentId));
                row.Add(BusinessReportsConstants.TransactionExceedDetails.AGENT_NAME, string.IsNullOrEmpty(item.AgentName) ? string.Empty : item.AgentName);

                row.Add(BusinessReportsConstants.TransactionExceedDetails.CITY, string.IsNullOrEmpty(item.City) ? string.Empty : item.City);
                row.Add(BusinessReportsConstants.TransactionExceedDetails.POS_ID, string.IsNullOrEmpty(item.PosId) ? string.Empty : item.PosId);
                row.Add(BusinessReportsConstants.TransactionExceedDetails.FIRST_NAME, string.IsNullOrEmpty(item.FirstName) ? string.Empty : item.FirstName);
                row.Add(BusinessReportsConstants.TransactionExceedDetails.LAST_NAME, string.IsNullOrEmpty(item.LastName) ? string.Empty : item.LastName);
                row.Add(BusinessReportsConstants.TransactionExceedDetails.LDAP_USER_ID, string.IsNullOrEmpty(item.LdapUserId) ? string.Empty : item.LdapUserId);
                row.Add(BusinessReportsConstants.TransactionExceedDetails.EVENT_TRAN_CODE, string.IsNullOrEmpty(item.EventTranCode) ? string.Empty : item.EventTranCode);
                row.Add(BusinessReportsConstants.TransactionExceedDetails.EVENT_TRAN_NAME, string.IsNullOrEmpty(item.EventTranName) ? string.Empty : item.EventTranName);
                row.Add(BusinessReportsConstants.TransactionExceedDetails.TRAN_REF_ID, string.IsNullOrEmpty(item.TranRefId) ? string.Empty : item.TranRefId);
                row.Add(BusinessReportsConstants.TransactionExceedDetails.TRAN_LIM_CODE, Convert.ToString(item.TranLimCode));
                row.Add(BusinessReportsConstants.TransactionExceedDetails.TRAN_LIM_BSNS_DESC, string.IsNullOrEmpty(item.TranLimBsnsDesc) ? string.Empty : item.TranLimBsnsDesc);
                row.Add(BusinessReportsConstants.TransactionExceedDetails.EVENT_FACE_TRAN_AMT, Convert.ToString(item.EventFaceTranAmt));
                row.Add(BusinessReportsConstants.TransactionExceedDetails.TRAN_LIM_USD_AMT, Convert.ToString(item.TranLimUsdAmt));
                row.Add(BusinessReportsConstants.TransactionExceedDetails.MGR_FIRST_NAME, string.IsNullOrEmpty(item.MgrFirstName) ? string.Empty : item.MgrFirstName);
                row.Add(BusinessReportsConstants.TransactionExceedDetails.MGR_LAST_NAME, string.IsNullOrEmpty(item.MgrLastName) ? string.Empty : item.MgrLastName);
                row.Add(BusinessReportsConstants.TransactionExceedDetails.MGR_LDAP_USER_ID, string.IsNullOrEmpty(item.MgrLdapUserId) ? string.Empty : item.MgrLdapUserId);
                row.Add(BusinessReportsConstants.TransactionExceedDetails.EVENT_TRAN_EVENT_DATE, (item.EventTranEvntDate == null) ? string.Empty : item.EventTranEvntDate.ToString("yyyy MMM dd"));
                row.Add(BusinessReportsConstants.TransactionExceedDetails.EVENT_TRAN_EVENT_LCL_DATE_FIELD, (item.EventTranEvntLclDateField == null) ? string.Empty : item.EventTranEvntLclDateField.ToString("yyyy MMM dd"));

                transactionExceedReport.Rows.Add(row);
            }
            reportContainer.Reports.Add(transactionExceedReport);
            return reportContainer;
        }
    }
}