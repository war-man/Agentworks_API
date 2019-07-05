using AwApi.ViewModels;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace AwApi.Business
{
    public interface IDocumentBusiness
    {
        AcApiResponse<SaveTransactionDocumentResponse, ApiData> SaveTransactionDocument(SaveTransactionDocumentRequest req);
    }
}