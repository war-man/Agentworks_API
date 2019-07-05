using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyGram.DLS.Service;

namespace MoneyGram.DLS
{
    public interface IDLSProxyFactory
    {
        DataLookupServicePortType CreateProxy();
    }
}
