using System;
using System.Collections.Generic;
using AwApi.EntityMapper.ReportsVmExtensions;
using AwApi.Integration.Reports;
using AwApi.ViewModels.Reports;
using MoneyGram.PartnerService.DomainModel;

namespace AwApi.Business.Reports.Containers
{
    public class TransactionExceedDetailsContainer : ITransactionExceedDetailsContainer
    {
        private IPartnerServiceIntegration _partnerIntegration;

        public TransactionExceedDetailsContainer(IPartnerServiceIntegration partnerIntegration)
        {
            _partnerIntegration = partnerIntegration;
        }

        public ReportResponse GetContainer(ReportRequest reportRequest)
        {
            var reportContainer = ReportResponseFactory.Create(ReportType.DailyTranDetail, reportRequest);

            //DomainTranformation
            var tranExceedRequest = reportRequest.ToTransactionExceedRequestModel();

            var transactionExceedReport = new Report(ReportConstants.Common.TRANEXCEED_REPORT_NAME);

            //Call repo
            var transactionExceedInfoResponse = _partnerIntegration.GetTransactionExceedInfo(tranExceedRequest);

            foreach (TransactionExceedReportsInfo item in transactionExceedInfoResponse.transactionExceedReportsInfoList)
            {
                var row = new Dictionary<string, string>();
                row.Add(ReportConstants.Common.AGENT_ID, Convert.ToString(item.AgentId));
                row.Add(ReportConstants.Common.AGENT_NAME, string.IsNullOrEmpty(item.AgentName) ? string.Empty : item.AgentName);

                row.Add(ReportConstants.TransactionExceedDetails.CITY, string.IsNullOrEmpty(item.City) ? string.Empty : item.City);
                row.Add(ReportConstants.Common.POS_ID, string.IsNullOrEmpty(item.PosId) ? string.Empty : item.PosId);
                row.Add(ReportConstants.Common.FIRST_NAME, string.IsNullOrEmpty(item.FirstName) ? string.Empty : item.FirstName);
                row.Add(ReportConstants.Common.LAST_NAME, string.IsNullOrEmpty(item.LastName) ? string.Empty : item.LastName);
                row.Add(ReportConstants.TransactionExceedDetails.LDAP_USER_ID, string.IsNullOrEmpty(item.LdapUserId) ? string.Empty : item.LdapUserId);
                row.Add(ReportConstants.TransactionExceedDetails.EVENT_TRAN_CODE, string.IsNullOrEmpty(item.EventTranCode) ? string.Empty : item.EventTranCode);
                row.Add(ReportConstants.TransactionExceedDetails.EVENT_TRAN_NAME, string.IsNullOrEmpty(item.EventTranName) ? string.Empty : item.EventTranName);
                row.Add(ReportConstants.TransactionExceedDetails.TRAN_REF_ID, string.IsNullOrEmpty(item.TranRefId) ? string.Empty : item.TranRefId);
                row.Add(ReportConstants.TransactionExceedDetails.TRAN_LIM_CODE, Convert.ToString(item.TranLimCode));
                row.Add(ReportConstants.TransactionExceedDetails.TRAN_LIM_BSNS_DESC, string.IsNullOrEmpty(item.TranLimBsnsDesc) ? string.Empty : item.TranLimBsnsDesc);
                row.Add(ReportConstants.TransactionExceedDetails.EVENT_FACE_TRAN_AMT, Convert.ToString(item.EventFaceTranAmt));
                row.Add(ReportConstants.TransactionExceedDetails.TRAN_LIM_USD_AMT, Convert.ToString(item.TranLimUsdAmt));
                row.Add(ReportConstants.TransactionExceedDetails.MGR_FIRST_NAME, string.IsNullOrEmpty(item.MgrFirstName) ? string.Empty : item.MgrFirstName);
                row.Add(ReportConstants.TransactionExceedDetails.MGR_LAST_NAME, string.IsNullOrEmpty(item.MgrLastName) ? string.Empty : item.MgrLastName);
                row.Add(ReportConstants.TransactionExceedDetails.MGR_LDAP_USER_ID, string.IsNullOrEmpty(item.MgrLdapUserId) ? string.Empty : item.MgrLdapUserId);
                row.Add(ReportConstants.TransactionExceedDetails.EVENT_TRAN_EVENT_DATE, (item.EventTranEvntDate == null) ? string.Empty : item.EventTranEvntDate.ToString("yyyy MMM dd"));
                row.Add(ReportConstants.TransactionExceedDetails.EVENT_TRAN_EVENT_LCL_DATE_FIELD, (item.EventTranEvntLclDateField == null) ? string.Empty : item.EventTranEvntLclDateField.ToString("yyyy MMM dd"));

                transactionExceedReport.Rows.Add(row);
            }
            reportContainer.Reports.Add(transactionExceedReport);
            return reportContainer;
        }
    }
}