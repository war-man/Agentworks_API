using System.Collections.Generic;
using System.ServiceModel.Description;

namespace MoneyGram.DLS
{
    public class DLSConfig : IDLSConfig
    {
        public string DLSUrl { get; set; }

        public IList<IEndpointBehavior> EndpoingBehaviors { get; set; }
    }
}
