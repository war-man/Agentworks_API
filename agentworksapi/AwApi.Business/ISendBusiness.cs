using AwApi.ViewModels;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace AwApi.Business
{
    public interface ISendBusiness
    {
        AcApiResponse<SendValidationResponse, ReceiptsApiData> SendValidation(SendValidationRequest reqVm);
    }
}