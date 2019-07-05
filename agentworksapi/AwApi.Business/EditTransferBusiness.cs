using AwApi.Business.BusinessRules;
using AwApi.EntityMapper;
using AwApi.Integration;
using AwApi.ViewModels;
using MoneyGram.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using AwApi.ViewModels.EditTransfer;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace AwApi.Business
{
    public class EditTransferBusiness : IEditTransferBusiness
    {
        protected readonly ICommonBusiness _commonBusiness;

        public EditTransferBusiness(ICommonBusiness commonBusiness)
        {
            commonBusiness.ThrowIfNull(nameof(commonBusiness));

            this._commonBusiness = commonBusiness;
        }

        public AcApiResponse<EditTransferTransactionResponse, ApiData> TransactionLookup(TransactionLookupRequest transactionLookupRequest)
        {
            var respVm = new EditTransferTransactionResponse();
            var respPayload = new EditTransferTransactionResponsePayload();
            ApiResponse<TransactionLookupResponse, ApiData> respAmend = null;
            ApiResponse<TransactionLookupResponse, ApiData> respSendReversal = null;
            ApiResponse<TransactionLookupResponse, ApiData> respReceiveReversal = null;

            int? flags = null;
            var apiErrors = new Dictionary<string, string>();

            var productAuthorizations = AuthIntegration.GetProductAuthorizations();

            if (productAuthorizations.CanAmend)
            {
                transactionLookupRequest.PurposeOfLookup = PurposeOfLookup.Amend;
                respAmend = ExecuteTransactionLookup(transactionLookupRequest);
                respPayload.AmendTransactionLookupResponse = respAmend;
                if (respAmend.ApiErrors != null)
                {
                    foreach (var errItem in respAmend.ApiErrors)
                    {
                        apiErrors.Add(errItem.Key, errItem.Value);
                    }
                }
            }
            if (productAuthorizations.CanSendReversal)
            {
                transactionLookupRequest.PurposeOfLookup = PurposeOfLookup.SendReversal;
                respSendReversal = ExecuteTransactionLookup(transactionLookupRequest);
                respPayload.SendReversalTransactionLookupResponse = respSendReversal;
                if (respSendReversal.ApiErrors != null)
                {
                    foreach (var errItem in respSendReversal.ApiErrors)
                    {
                        apiErrors.Add(errItem.Key, errItem.Value);
                    }
                }
            }
            if (productAuthorizations.CanReceiveReversal)
            {
                transactionLookupRequest.PurposeOfLookup = PurposeOfLookup.ReceiveReversal;
                respReceiveReversal = ExecuteTransactionLookup(transactionLookupRequest);
                respPayload.ReceiveReversalTransactionLookupResponse = respReceiveReversal;
                if (respReceiveReversal.ApiErrors != null)
                {
                    foreach (var errItem in respReceiveReversal.ApiErrors)
                    {
                        apiErrors.Add(errItem.Key, errItem.Value);
                    }
                }                
            }

            //
            if (respAmend?.ResponseData?.Payload == null && 
                respSendReversal?.ResponseData?.Payload == null &&
                respReceiveReversal?.ResponseData?.Payload == null)
            {
                respVm.EditTransferTransactionResponsePayload = null;
                respVm.Errors = new List<BusinessError>();
                if (respAmend?.ResponseData?.Errors != null) {
                    respVm.Errors.AddRange(respAmend.ResponseData.Errors);
                }
                if (respSendReversal?.ResponseData?.Errors != null)
                {
                    respVm.Errors.AddRange(respSendReversal.ResponseData.Errors);
                }
                if (respReceiveReversal?.ResponseData?.Errors != null)
                {
                    respVm.Errors.AddRange(respReceiveReversal.ResponseData.Errors);
                }  
            }
            else
            {
                respVm.EditTransferTransactionResponsePayload = new EditTransferTransactionResponsePayload();
                respVm.EditTransferTransactionResponsePayload.AmendTransactionLookupResponse = respAmend;
                respVm.EditTransferTransactionResponsePayload.SendReversalTransactionLookupResponse = respSendReversal;
                respVm.EditTransferTransactionResponsePayload.ReceiveReversalTransactionLookupResponse = respReceiveReversal;
                respVm.EditTransferTransactionResponsePayload.Flags = flags.GetValueOrDefault();
            }

            var apiResp = new AcApiResponse<EditTransferTransactionResponse, ApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(flags, DataSource.Lookup),
                ResponseData = respVm,
                ApiErrors = apiErrors.Any() ? apiErrors : null
            };

            return apiResp;
        }

        private AcApiResponse<TransactionLookupResponse, ApiData> ExecuteTransactionLookup(TransactionLookupRequest transactionLookupRequest)
        {
            AcApiResponse<TransactionLookupResponse, ApiData> apiResp = new AcApiResponse<TransactionLookupResponse, ApiData>();
            try
            {
                apiResp = _commonBusiness.TransactionLookup(transactionLookupRequest);
            }
            catch (Exception ex)
            {
                apiResp.ApiErrors = new Dictionary<string, string>();
                apiResp.ApiErrors.Add($"{transactionLookupRequest.PurposeOfLookup} TransactionLookup", ex.Message);                
            }

            return apiResp;
        }
    }
}