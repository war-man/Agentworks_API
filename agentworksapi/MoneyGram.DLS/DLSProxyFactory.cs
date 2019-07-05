using MoneyGram.DLS.Service;
using System.Collections.Generic;
using System.ServiceModel.Description;

namespace MoneyGram.DLS
{
    public class DLSProxyFactory:IDLSProxyFactory
    {
        private readonly string _url;
        private readonly IList<IEndpointBehavior> _endpointBehaviors;

        public DLSProxyFactory(IDLSConfig config)
        {
            _url = config.DLSUrl;
            _endpointBehaviors = config.EndpoingBehaviors;
        }
        public DataLookupServicePortType CreateProxy()
        {
            // would it be better to define the endpoing in app.config? Its so simple, keep in code for now.
            var binding = new System.ServiceModel.BasicHttpBinding(System.ServiceModel.BasicHttpSecurityMode.None);
            var endpoint = new System.ServiceModel.EndpointAddress(this._url);

            // these are essentially theoretical max values. What's a reasonable number for the use case?
            binding.MaxReceivedMessageSize = 2147483647;
            binding.MaxBufferSize = 2147483647;

            var proxy = new DataLookupServicePortTypeClient(binding, endpoint);

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
