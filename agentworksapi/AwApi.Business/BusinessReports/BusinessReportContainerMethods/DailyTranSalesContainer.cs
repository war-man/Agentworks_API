using AwApi.Business.BusinessReports.BusinessReportHelperMethods;
using AwApi.Business.BusinessReports.BusinessReportsEnums;
using AwApi.Business.BusinessReports.ReportsBusinessConstants;
using AwApi.EntityMapper.ReportsVmExtensions;
using AwApi.Integration;
using AwApi.ViewModels.ReportsViewModels;
using MoneyGram.DLS.DomainModel;
using System;
using System.Collections.Generic;

namespace AwApi.Business.BusinessReports.BusinessReportContainerMethods
{
    public static class DailyTranSalesContainer
    {
        public static ReportContainerVm GetContainer(ReportRequestVm _reportRequestVm, IPartnerServiceIntegration _partnerIntegration, IDlsIntegration _dlsIntegration, string[] strPosIdList)
        {
            var reportContainer = ReportContainerHelper.GetStagedContainer(ReportType.DailyTranSales, _partnerIntegration, _reportRequestVm);

            //Domain Tranformation
            var dailyTranLookupReq = _reportRequestVm.ToDailyTranDetailLookupModel();
            var tranType = string.Empty;

            //Add all the report(s)
            ReportVm rpt = new ReportVm(BusinessReportsConstants.Common.SR_NAME);

            // Map from ViewModel->Domain
            var tranDetailLookupResp = _dlsIntegration.DailyTransactionDetailLookup(dailyTranLookupReq);

            if (tranDetailLookupResp == null || tranDetailLookupResp.GetDailyTransactionDetailLookupResultList == null)
            {
                return reportContainer;
            }

            foreach (TransactionDetailLookupResult item in tranDetailLookupResp.GetDailyTransactionDetailLookupResultList)
            {
                Dictionary<string, string> DataInfo = new Dictionary<string, string>();

                DataInfo.Add(BusinessReportsConstants.DailyTranSales.AGENT_ID, dailyTranLookupReq.AgentId);
                DataInfo.Add(BusinessReportsConstants.DailyTranSales.ISO_CURRENCY_CODE, (string.IsNullOrEmpty(item.IsoCurrencyCode)) ? "-" : item.IsoCurrencyCode);
                DataInfo.Add(BusinessReportsConstants.DailyTranSales.AGENT_LOCAL_TIME, (string.IsNullOrEmpty(item.AgentLocalTime)) ? "" : Convert.ToDateTime(item.AgentLocalTime).ToString("HH:mm"));
                DataInfo.Add(BusinessReportsConstants.DailyTranSales.EMPLOYEE_NUMBER, string.Empty);
                DataInfo.Add(BusinessReportsConstants.DailyTranSales.RECEIVER_NAME, (string.IsNullOrEmpty(item.ReceiverName)) ? "-" : item.ReceiverName);
                DataInfo.Add(BusinessReportsConstants.DailyTranSales.REFERENCE_ID, (string.IsNullOrEmpty(item.ReferenceId)) ? "-" : item.ReferenceId);

                var serialNumber = "";
                foreach (string sItem in item.SerialNumberArray)
                {
                    serialNumber += sItem + "-";
                }
                if (!string.IsNullOrEmpty(serialNumber))
                {
                    serialNumber = serialNumber.Substring(0, serialNumber.Length - 1);
                }
                DataInfo.Add(BusinessReportsConstants.DailyTranSales.SERIAL_NUMBER, serialNumber);

                tranType = string.Empty;

                if (item.ProductId.Equals(BusinessReportsConstants.Common.UTILITY_BILL_PAYMENT_PRODUCT_ID))
                {
                    tranType = BusinessReportsConstants.Common.TRAN_TYPE_UP;
                }
                else if (item.ProductId.Equals(BusinessReportsConstants.Common.EXPRESS_PAYMENT_PRODUCT_ID))
                {
                    tranType = BusinessReportsConstants.Common.TRAN_TYPE_EP;
                }
                else if (item.ProductId.Equals(BusinessReportsConstants.Common.MT_PRODUCT_ID) && item.EventCode.Equals(DLSActivityTypeCode.SEN.ToString()))
                {
                    tranType = BusinessReportsConstants.Common.TRAN_TYPE_SEND;
                }
                else if (item.ProductId.Equals(BusinessReportsConstants.Common.MT_PRODUCT_ID) && item.EventCode.Equals(DLSActivityTypeCode.REC.ToString()))
                {
                    tranType = BusinessReportsConstants.Common.TRAN_TYPE_RECEIVE;
                }
                else if (item.EventCode.Equals(DLSActivityTypeCode.RSN.ToString()))
                {
                    tranType = BusinessReportsConstants.Common.TRAN_TYPE_CANCEL;
                }
                else if (item.EventCode.Equals(DLSActivityTypeCode.RRC.ToString()))
                {
                    tranType = BusinessReportsConstants.Common.TRAN_TYPE_REVERSAL;
                }

                DataInfo.Add(BusinessReportsConstants.DailyTranSales.TRANSACTION_TYPE, tranType);
                DataInfo.Add(BusinessReportsConstants.DailyTranSales.AMOUNT, (string.IsNullOrEmpty(item.FaceAmount)) ? "-" : string.Format(BusinessReportsConstants.Common.AMT_FORMAT, double.Parse(item.FaceAmount)));
                DataInfo.Add(BusinessReportsConstants.DailyTranSales.EXCHANGE_RATE, (string.IsNullOrEmpty(item.FxConsumerRate)) ? "-" : item.FxConsumerRate);
                DataInfo.Add(BusinessReportsConstants.DailyTranSales.FEE, (string.IsNullOrEmpty(item.FeeAmount)) ? "-" : string.Format(BusinessReportsConstants.Common.AMT_FORMAT, double.Parse(item.FeeAmount)));
                DataInfo.Add(BusinessReportsConstants.DailyTranSales.SENDER, (string.IsNullOrEmpty(item.SenderName)) ? "-" : item.SenderName);

                //Get user based on mainofficeid and operatorid
                var operatorId = (string.IsNullOrEmpty(item.OperatorId)) ? "" : item.OperatorId.ToLower();
                string userId = null;

                userId = SupplementalValuesHelper.GetUserId(operatorId, _partnerIntegration);

                DataInfo.Add(BusinessReportsConstants.DailyTranSales.USERNAME, (userId == null) ? "-" : userId);

                if (item.FeeAmount != null)
                {
                    var faceAmount = double.Parse(item.FaceAmount);
                    var fee = double.Parse(item.FeeAmount);
                    var total = faceAmount + fee;
                    DataInfo.Add(BusinessReportsConstants.DailyTranSales.TOTAL, string.Format(BusinessReportsConstants.Common.AMT_FORMAT, total));
                }
                else
                {
                    DataInfo.Add(BusinessReportsConstants.DailyTranSales.TOTAL, string.Format(BusinessReportsConstants.Common.AMT_FORMAT, double.Parse(item.FaceAmount)));
                }

                if (item.PosId == null || !item.PosId.Equals("0"))
                {
                    DataInfo.Add(BusinessReportsConstants.DailyTranSales.POS_ID, (item.PosId == null) ? "-" : item.PosId);
                }
                else
                {
                    DataInfo.Add(BusinessReportsConstants.DailyTranSales.POS_ID, BusinessReportsConstants.Common.SUPER_AGENT);
                }
              
                //Filter by Pos ID
                foreach (string posid in strPosIdList)
                {
                    if (item.PosId == posid)
                    {
                        rpt.Rows.Add(DataInfo);
                    }
                }
            }

            reportContainer.Reports.Add(rpt);

            return reportContainer;
        }
    }
}