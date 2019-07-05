using AwApi.ViewModels;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace AwApi.Business
{
    public interface IReceiveBusiness
    {
        AcApiResponse<ReceiveValidationResponse, ReceiptsApiData> ReceiveValidation(ReceiveValidationRequest reqVm);
    }
}