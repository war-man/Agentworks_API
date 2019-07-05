using AwApi.ViewModels;
using AwApi.ViewModels.EditTransfer;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace AwApi.Business
{
    public interface IEditTransferBusiness
    {
        //API calls
        AcApiResponse<EditTransferTransactionResponse, ApiData> TransactionLookup(TransactionLookupRequest transactionLookupRequest);
    }
}