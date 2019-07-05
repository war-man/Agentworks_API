using System.Collections.Generic;
using System.ServiceModel.Description;
using System.Text.RegularExpressions;
using SERVICE = MoneyGram.AgentConnect.Service;

namespace MoneyGram.AgentConnect
{
    /// <summary>
    /// Implements IAgentConnectProxyFactory to return a WCF proxy pointing to the AgentConnet URL defined by IAgentConnectConfig.
    /// </summary>
    public class AgentConnectProxyFactory : IAgentConnectProxyFactory
    {
        private readonly string _url;
        private readonly IList<IEndpointBehavior> _endpointBehaviors;

        public AgentConnectProxyFactory(IAgentConnectConfig config)
        {
            _url = config.AgentConnectUrl;
            _endpointBehaviors = config.EndpoingBehaviors;
        }

        public SERVICE.AgentConnect CreateProxy()
        {
            // would it be better to define the endpoing in app.config? Its so simple, keep in code for now.
            // Setting BasicHttpSecurityMode to none will allow HTTP access to agent connect for debugging
            //            var binding = new System.ServiceModel.BasicHttpBinding(System.ServiceModel.BasicHttpSecurityMode.None);
            var binding = new System.ServiceModel.BasicHttpBinding(System.ServiceModel.BasicHttpSecurityMode.Transport);
            var endpoint = new System.ServiceModel.EndpointAddress(this._url);

            // these are essentially theoretical max values. What's a reasonable number for the use case?
            binding.MaxReceivedMessageSize = 2147483647;
            binding.MaxBufferSize = 2147483647;

            var proxy = new SERVICE.AgentConnectClient(binding, endpoint);

            if (_endpointBehaviors != null && _endpointBehaviors.Count > 0)
            {
                foreach(var behavior in _endpointBehaviors)
                {
                    proxy.Endpoint.EndpointBehaviors.Add(behavior);
                }
            }

            return proxy;
        }

    }
}