using MoneyGram.PartnerService.Service;
using System.Collections.Generic;
using System.ServiceModel.Description;

namespace MoneyGram.PartnerService
{
    public class PartnerServiceProxyFactory : IPartnerServiceProxyFactory
    {
        private readonly string _url;
        private readonly IList<IEndpointBehavior> _endpointBehaviors;

        public PartnerServiceProxyFactory(IPartnerServiceConfig config)
        {
            _url = config.PartnerServiceUrl;
            _endpointBehaviors = config.EndpoingBehaviors;
        }
        public WebPOEPartnerServicePortType CreateProxy()
        {
            var binding = new System.ServiceModel.BasicHttpBinding(System.ServiceModel.BasicHttpSecurityMode.None);
            var endpoint = new System.ServiceModel.EndpointAddress(this._url);

            // these are essentially theoretical max values. What's a reasonable number for the use case?
            binding.MaxReceivedMessageSize = 2147483647;
            binding.MaxBufferSize = 2147483647;

            var proxy = new WebPOEPartnerServicePortTypeClient(binding, endpoint);

            if (_endpointBehaviors != null && _endpointBehaviors.Count > 0)
            {
                foreach (var behavior in _endpointBehaviors)
                {
                    proxy.Endpoint.EndpointBehaviors.Add(behavior);
                }
            }

            return proxy;
        }
    }
}
