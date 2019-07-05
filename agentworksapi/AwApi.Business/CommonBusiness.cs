using AwApi.Business.Helpers;
using AwApi.EntityMapper;
using AwApi.Integration;
using AwApi.ViewModels;
using MoneyGram.Common;
using MoneyGram.Common.Extensions;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace AwApi.Business
{
    public class CommonBusiness : ICommonBusiness
    {
        protected readonly IAgentConnectIntegration agentConnectIntegration;
        protected readonly IReceiptIntegration receiptIntegration;

        public CommonBusiness(IAgentConnectIntegration agentConnectIntegration, IReceiptIntegration receiptIntegration)
        {
            agentConnectIntegration.ThrowIfNull(nameof(agentConnectIntegration));
            receiptIntegration.ThrowIfNull(nameof(receiptIntegration));

            this.agentConnectIntegration = agentConnectIntegration;
            this.receiptIntegration = receiptIntegration;
        }

        public AcApiResponse<TransactionLookupResponse, ApiData> TransactionLookup(TransactionLookupRequest req)
        {
            //AgentConnect TransactionLookup call all flows
            var resp = agentConnectIntegration.TransactionLookup(req);

            var apiResp = new AcApiResponse<TransactionLookupResponse, ApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(resp.Payload?.Flags, DataSource.AgentConnect),
                ResponseData = resp
            };

            return apiResp;
        }

        public AcApiResponse<SearchStagedTransactionsResponse, ApiData> SearchStagedTransactions(SearchStagedTransactionsRequest req)
        {
            var resp = agentConnectIntegration.SearchStagedTransactions(req);

            var apiResp = new AcApiResponse<SearchStagedTransactionsResponse, ApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(resp.Payload?.Flags, DataSource.AgentConnect),
                ResponseData = resp
            };

            return apiResp;
        }

        public AcApiResponse<FeeLookupResponse, ApiData> FeeLookup(FeeLookupRequest req)
        {
            // If Transactional limit is exceeded, throw an exception
            var transactionalLimits = AuthIntegration.GetTransactionalLimits();
            decimal? limit = null;
            switch (req.MgiSessionType)
            {
                case SessionType.SEND:
                    limit = transactionalLimits.MaxSendAmount;
                    break;
                case SessionType.BP:
                    limit = transactionalLimits.MaxBillPayAmount;
                    break;
            }
            if (limit != null && req.Item > limit)
            {
                throw new TransactionalLimitsException();
            }

            // AgentConnect FeeLookup call for SEND
            var resp = agentConnectIntegration.FeeLookup(req);

            var apiResp = new AcApiResponse<FeeLookupResponse, ApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(resp.Payload?.Flags, DataSource.AgentConnect),
                ResponseData = resp
            };

            return apiResp;
        }

        public AcApiResponse<CompleteSessionResponse, ReceiptsApiData> CompleteSession(CompleteSessionRequest req)
        {
            var resp = agentConnectIntegration.CompleteSession(req);

            var additionalData = ReceiptHelper.GenerateAdditionalDataReceipts(resp.Payload?.Receipts, receiptIntegration);

            var apiResp = new AcApiResponse<CompleteSessionResponse, ReceiptsApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(resp.Payload?.Flags, DataSource.AgentConnect),
                ResponseData = resp,
                AdditionalData = additionalData
            };

            return apiResp;
        }

        public void ExceedsTransactionalLimit(SessionType tranType, decimal tranAmount)
        {
            if (tranAmount == 0m)
            {
                return;
            }

            var transactionalLimits = AuthIntegration.GetTransactionalLimits();
            bool exceedsTransactionalLimit;
            switch (tranType)
            {
                case SessionType.SEND:
                    exceedsTransactionalLimit = transactionalLimits.MaxSendAmount != null && tranAmount > transactionalLimits.MaxSendAmount;
                    break;
                case SessionType.RCV:
                    exceedsTransactionalLimit = transactionalLimits.MaxReceiveAmount != null && tranAmount > transactionalLimits.MaxReceiveAmount;
                    break;
                case SessionType.BP:
                    exceedsTransactionalLimit = transactionalLimits.MaxBillPayAmount != null && tranAmount > transactionalLimits.MaxBillPayAmount;
                    break;
                case SessionType.SREV:
                    exceedsTransactionalLimit = transactionalLimits.MaxSendCancellationAmount != null && tranAmount > transactionalLimits.MaxSendCancellationAmount;
                    break;
                case SessionType.RREV:
                    exceedsTransactionalLimit = transactionalLimits.MaxReceiveCancellationAmount != null && tranAmount > transactionalLimits.MaxReceiveCancellationAmount;
                    break;
                case SessionType.AMD:
                    exceedsTransactionalLimit = transactionalLimits.MaxAmendAmount != null && tranAmount > transactionalLimits.MaxAmendAmount;
                    break;
                default:
                    exceedsTransactionalLimit = false;
                    break;
            }

            if (exceedsTransactionalLimit)
            {
                throw new TransactionalLimitsException();
            }
        }
    }
}