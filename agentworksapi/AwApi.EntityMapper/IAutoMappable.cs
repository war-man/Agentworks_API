using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwApi.EntityMapper
{
    public interface IAutoMappable
    {
        static void DefineMappings();
    }
}
