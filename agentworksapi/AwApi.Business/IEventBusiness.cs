using AwApi.ViewModels;
using AwApi.ViewModels.Event;

namespace AwApi.Business
{
    public interface IEventBusiness
    {
        ApiResponse<EventResponse, ApiData> Process(EventRequest eventRequest);
    }
}