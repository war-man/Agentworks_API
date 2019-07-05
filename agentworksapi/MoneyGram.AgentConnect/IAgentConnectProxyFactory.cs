using SERVICE = MoneyGram.AgentConnect.Service;

namespace MoneyGram.AgentConnect
{
    /// NOTE: The AgentConnectService.AgentConnect interface is in the .Repository assembly 
    ///       and  returns the current AC WSDL interface.  

    /// <summary>
    /// Defines dependency injected factory that creates AgentConnect proxes that are IDisposable.
    /// </summary>
    public interface IAgentConnectProxyFactory
    {
        SERVICE.AgentConnect CreateProxy();
    }
}
