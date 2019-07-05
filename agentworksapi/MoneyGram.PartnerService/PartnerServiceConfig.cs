using System.Collections.Generic;
using System.ServiceModel.Description;

namespace MoneyGram.PartnerService
{
    public class PartnerServiceConfig : IPartnerServiceConfig
    {
        public string PartnerServiceUrl { get; set; }

        public IList<IEndpointBehavior> EndpoingBehaviors { get; set; }
    }
}