using System;
using System.Collections.Generic;
using AwApi.EntityMapper.ReportsVmExtensions;
using AwApi.Integration;
using AwApi.Integration.Reports;
using AwApi.ViewModels.Reports;
using MoneyGram.DLS.DomainModel;

namespace AwApi.Business.Reports.Containers
{
    public class DailyTranSalesContainer : IDailyTranSalesContainer
    {
        private IDlsIntegration _dlsIntegration;
        private IPartnerServiceIntegration _partnerIntegration;
        public DailyTranSalesContainer(IDlsIntegration dlsIntegration, IPartnerServiceIntegration partnerIntegration)
        {
            _dlsIntegration = dlsIntegration;
            _partnerIntegration = partnerIntegration;
        }

        public ReportResponse GetContainer(ReportRequest reportRequest)
        {
            var reportContainer = ReportResponseFactory.Create(ReportType.DailyTranSales, reportRequest);

            //Domain Tranformation
            var dailyTranLookupReq = reportRequest.ToDailyTranDetailLookupModel();
            var tranType = string.Empty;

            //Add all the report(s)
            var rpt = new Report(ReportConstants.Common.SR_NAME);

            // Map from ViewModel->Domain
            var tranDetailLookupResp = _dlsIntegration.DailyTransactionDetailLookup(dailyTranLookupReq);

            if (tranDetailLookupResp == null || tranDetailLookupResp.GetDailyTransactionDetailLookupResultList == null)
            {
                return reportContainer;
            }

            foreach (TransactionDetailLookupResult item in tranDetailLookupResp.GetDailyTransactionDetailLookupResultList)
            {
                if (!reportRequest.PosIds.Contains(item.PosId))
                {
                    continue;
                }

                var posId = string.Empty;
                if (item.PosId == null || !item.PosId.Equals("0"))
                {
                    posId = (item.PosId == null) ? "-" : item.PosId;                    
                }
                else
                {
                    posId = ReportConstants.Common.SUPER_AGENT;                    
                }

                Dictionary<string, string> DataInfo = new Dictionary<string, string>();

                DataInfo.Add(ReportConstants.DailyTranSales.AGENT_ID, dailyTranLookupReq.AgentId);
                DataInfo.Add(ReportConstants.DailyTranSales.ISO_CURRENCY_CODE, (string.IsNullOrEmpty(item.IsoCurrencyCode)) ? "-" : item.IsoCurrencyCode);
                DataInfo.Add(ReportConstants.DailyTranSales.AGENT_TIME, (string.IsNullOrEmpty(item.AgentLocalTime)) ? "" : Convert.ToDateTime(item.AgentLocalTime).ToString("HH:mm"));
                DataInfo.Add(ReportConstants.DailyTranSales.EMPLOYEE_NUMBER, string.Empty);
                DataInfo.Add(ReportConstants.DailyTranSales.RECEIVER_NAME, (string.IsNullOrEmpty(item.ReceiverName)) ? "-" : item.ReceiverName);
                DataInfo.Add(ReportConstants.DailyTranSales.REFERENCE_ID, (string.IsNullOrEmpty(item.ReferenceId)) ? "-" : item.ReferenceId);

                var serialNumber = "";
                foreach (string sItem in item.SerialNumberArray)
                {
                    serialNumber += sItem + "-";
                }
                if (!string.IsNullOrEmpty(serialNumber))
                {
                    serialNumber = serialNumber.Substring(0, serialNumber.Length - 1);
                }
                DataInfo.Add(ReportConstants.DailyTranSales.SERIAL_NUMBER, serialNumber);

                tranType = string.Empty;

                if (item.ProductId.Equals(ReportConstants.Common.UTILITY_BILL_PAYMENT_PRODUCT_ID))
                {
                    tranType = ReportConstants.Common.TRAN_TYPE_UP;
                }
                else if (item.ProductId.Equals(ReportConstants.Common.EXPRESS_PAYMENT_PRODUCT_ID))
                {
                    tranType = ReportConstants.Common.TRAN_TYPE_EP;
                }
                else if (item.ProductId.Equals(ReportConstants.Common.MT_PRODUCT_ID) && item.EventCode.Equals(DLSActivityTypeCode.SEN.ToString()))
                {
                    tranType = ReportConstants.Common.TRAN_TYPE_SEND;
                }
                else if (item.ProductId.Equals(ReportConstants.Common.MT_PRODUCT_ID) && item.EventCode.Equals(DLSActivityTypeCode.REC.ToString()))
                {
                    tranType = ReportConstants.Common.TRAN_TYPE_RECEIVE;
                }
                else if (item.EventCode.Equals(DLSActivityTypeCode.RSN.ToString()))
                {
                    tranType = ReportConstants.Common.TRAN_TYPE_CANCEL;
                }
                else if (item.EventCode.Equals(DLSActivityTypeCode.RRC.ToString()))
                {
                    tranType = ReportConstants.Common.TRAN_TYPE_REVERSAL;
                }

                DataInfo.Add(ReportConstants.DailyTranSales.TRANSACTION_TYPE, tranType);
                DataInfo.Add(ReportConstants.Common.AMOUNT, (string.IsNullOrEmpty(item.FaceAmount)) ? "-" : string.Format(ReportConstants.Common.AMT_FORMAT, double.Parse(item.FaceAmount)));
                DataInfo.Add(ReportConstants.DailyTranSales.EXCHANGE_RATE, (string.IsNullOrEmpty(item.FxConsumerRate)) ? "-" : item.FxConsumerRate);
                DataInfo.Add(ReportConstants.Common.FEE, (string.IsNullOrEmpty(item.FeeAmount)) ? "-" : string.Format(ReportConstants.Common.AMT_FORMAT, double.Parse(item.FeeAmount)));
                DataInfo.Add(ReportConstants.DailyTranSales.SENDER, (string.IsNullOrEmpty(item.SenderName)) ? "-" : item.SenderName);

                //Get user based on mainofficeid and operatorid
                var operatorId = (string.IsNullOrEmpty(item.OperatorId)) ? "" : item.OperatorId.ToLower();
                string userId = null;

                userId = string.IsNullOrEmpty(operatorId) ? "-" : operatorId;

                DataInfo.Add(ReportConstants.DailyTranSales.USERNAME, (userId == null) ? "-" : userId);

                if (item.FeeAmount != null)
                {
                    var faceAmount = double.Parse(item.FaceAmount);
                    var fee = double.Parse(item.FeeAmount);
                    var total = faceAmount + fee;
                    DataInfo.Add(ReportConstants.DailyTranSales.TOTAL_AMOUNT, string.Format(ReportConstants.Common.AMT_FORMAT, total));
                }
                else
                {
                    DataInfo.Add(ReportConstants.DailyTranSales.TOTAL_AMOUNT, string.Format(ReportConstants.Common.AMT_FORMAT, double.Parse(item.FaceAmount)));
                }

                DataInfo.Add(ReportConstants.Common.POS_ID, posId);

                rpt.Rows.Add(DataInfo);
            }

            reportContainer.Reports.Add(rpt);

            return reportContainer;
        }
    }
}