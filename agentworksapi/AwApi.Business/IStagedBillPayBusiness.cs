using AwApi.ViewModels;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace AwApi.Business
{
    public interface IStagedBillPayBusiness
    {
        AcApiResponse<BPValidationResponse, ReceiptsApiData> BPValidation(BPValidationRequest reqVm);
    }
}