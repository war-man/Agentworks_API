
using System.Collections.Generic;
using System.ServiceModel.Description;

namespace MoneyGram.PartnerService
{   
    /// <summary>
    /// Defines dependency injected configuration interface for configuration driven PartnerService settings.
    /// </summary>
    public interface IPartnerServiceConfig
    {
        string PartnerServiceUrl { get; }
        IList<IEndpointBehavior> EndpoingBehaviors { get; }
    }
}
