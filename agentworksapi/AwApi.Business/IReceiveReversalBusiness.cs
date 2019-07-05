using AwApi.ViewModels;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace AwApi.Business
{
    public interface IReceiveReversalBusiness
    {
        AcApiResponse<ReceiveReversalValidationResponse, ApiData> ReceiveReversalValidation(ReceiveReversalValidationRequest reqVm);
    }
}