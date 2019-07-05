using System.Diagnostics;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace MoneyGram.Common.Diagnostics.ServiceModel
{
    /// <summary>
    ///     WCF Behavior that logs the raw SOAP message being sent and recieved by the client.
    /// </summary>
    public class TimingLogger : IClientMessageInspector, IEndpointBehavior
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            TimingLogRequest logRequest = null;
            if (correlationState != null)
            {
                logRequest = (TimingLogRequest) correlationState;
                logRequest.Stopwatch.Stop();
            }

            logger.Info(
                $"Received from {logRequest?.Action} -- Elapsed time: {logRequest?.Stopwatch.ElapsedMilliseconds}");
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            var action = string.Empty;
            var stopwatch = new Stopwatch();

            if (request?.Headers != null)
            {
                action = request.Headers.Action;
            }

            logger.Info($"Sending to {action}");

            stopwatch.Start();
            return new TimingLogRequest
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

    internal class TimingLogRequest
    {
        public string Action { get; set; }

        public Stopwatch Stopwatch { get; set; }
    }
}