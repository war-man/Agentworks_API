using AwApi.ViewModels;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace AwApi.Business
{
    public interface ICommonBusiness
    {
        // AC API calls
        AcApiResponse<TransactionLookupResponse, ApiData> TransactionLookup(TransactionLookupRequest reqVm);
        AcApiResponse<SearchStagedTransactionsResponse, ApiData> SearchStagedTransactions(SearchStagedTransactionsRequest reqVm);
        AcApiResponse<FeeLookupResponse, ApiData> FeeLookup(FeeLookupRequest reqVm);
        AcApiResponse<CompleteSessionResponse, ReceiptsApiData> CompleteSession(CompleteSessionRequest reqVm);
        // Business processing methods
        void ExceedsTransactionalLimit(SessionType tranType, decimal tranAmount);
    }
}