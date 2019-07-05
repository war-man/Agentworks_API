using AwApi.ViewModels;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace AwApi.Business
{
    public interface IStagedSendBusiness
    {
        AcApiResponse<SendValidationResponse, ReceiptsApiData> SendValidation(SendValidationRequest reqVm);
    }
}