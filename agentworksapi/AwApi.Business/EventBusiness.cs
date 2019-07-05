using AwApi.EntityMapper;
using AwApi.Integration.Event;
using AwApi.ViewModels;
using AwApi.ViewModels.Event;
using MoneyGram.Common.Extensions;

namespace AwApi.Business
{
    public class EventBusiness : IEventBusiness
    {
        private readonly IEventIntegration _eventIntegration;

        public EventBusiness(IEventIntegration eventIntegration)
        {
            eventIntegration.ThrowIfNull(nameof(eventIntegration));

            _eventIntegration = eventIntegration;

        }

        public ApiResponse<EventResponse, ApiData> Process(EventRequest eventRequest)
        {
            var respVm = _eventIntegration.Process(eventRequest);

            var apiResp = new ApiResponse<EventResponse, ApiData>
            {
                BusinessMetadata = MapperHelper.SetResponseProperties(null, DataSource.Event),
                ResponseData = respVm
            };

            return apiResp;
        }
    }
}