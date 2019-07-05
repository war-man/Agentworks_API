using AwApi.ViewModels;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace AwApi.Business
{
    public interface IAmendBusiness
    {
        AcApiResponse<AmendValidationResponse, ApiData> AmendValidation(AmendValidationRequest reqVm);
    }
}