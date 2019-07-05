using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace AwApi.ViewModels
{
    public class AcApiResponse<T, U> : ApiResponse<T, U> where T : Response where U : ApiData
    {
    }
}