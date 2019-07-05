using AwApi.ViewModels.Event;

namespace AwApi.Integration.Event
{
    public interface IEventIntegration
    {
        EventResponse Process(EventRequest eventRequest);
    }
}