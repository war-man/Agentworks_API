using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace MoneyGram.Common.Diagnostics.ServiceModel
{
    public class ProxyTimingLogger : IClientMessageInspector, IEndpointBehavior
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            var proxyTiming = (ProxyTiming) correlationState;
            proxyTiming?.Stopwatch?.Stop();

            if (proxyTiming != null)
            {
                var action = proxyTiming.Action ?? string.Empty;
                proxyTiming.Stopwatch?.Stop();

                logger.Info("{0}: {1}ms", action, proxyTiming.Stopwatch?.ElapsedMilliseconds);
            }
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            var action = request?.Headers?.Action ?? string.Empty;

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            return new ProxyTiming
            {
                Action = action,
                Stopwatch = stopwatch
            };
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.ClientMessageInspectors.Add(this);
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }

    internal class ProxyTiming
    {
        public string Action { get; set; }

        public Stopwatch Stopwatch { get; set; }
    }
}