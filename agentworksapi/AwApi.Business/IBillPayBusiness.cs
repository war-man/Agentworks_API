using AwApi.ViewModels;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace AwApi.Business
{
    public interface IBillPayBusiness
    {
        AcApiResponse<BillerSearchResponse, ApiData> BillerSearch(BillerSearchRequest reqVm);
        AcApiResponse<BPValidationResponse, ReceiptsApiData> BPValidation(BPValidationRequest reqVm);
        
        //BillPayProductsRespVm FindPrePaidCardProducts(PrepayCardLoadProductsReqVm prepayCardLoadProductsReqVm);
    }
}