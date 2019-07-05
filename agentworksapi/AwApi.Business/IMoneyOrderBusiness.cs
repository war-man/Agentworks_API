using AwApi.ViewModels;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace AwApi.Business
{
    public interface IMoneyOrderBusiness
    {
        AcApiResponse<MoneyOrderTotalResponse, ApiData> MoneyOrderTotal(MoneyOrderTotalRequest reqVm);

        AcApiResponse<ComplianceTransactionResponse, ApiData> ComplianceTransaction(ComplianceTransactionRequest reqVm);
    }
}