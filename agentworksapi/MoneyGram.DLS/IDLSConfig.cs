
using System.Collections.Generic;
using System.ServiceModel.Description;

namespace MoneyGram.DLS
{
    public interface IDLSConfig
    {
        string DLSUrl { get; }

        IList<IEndpointBehavior> EndpoingBehaviors { get; }
    }
}
