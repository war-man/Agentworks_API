using AwApi.ViewModels;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace AwApi.Business
{
    public interface ISendReversalBusiness
    {
        AcApiResponse<SendReversalValidationResponse, ReceiptsApiData> SendReversalValidation(SendReversalValidationRequest reqVm);
    }
}