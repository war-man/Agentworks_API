using AwApi.ViewModels;

namespace AwApi.Business
{
    public interface IOperatorBusiness
    {
        ApiResponse<Operator, ApiData> GetOperator();
        ApiResponse<Device, ApiData> GetDevice();
    }
}