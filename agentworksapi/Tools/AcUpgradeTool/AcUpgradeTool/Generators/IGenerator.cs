using System;
using System.Collections.Generic;

namespace AcUpgradeTool.Generators
{
    public interface IGenerator
    {
        void Generate(List<Type> types);

        void LogStart();

        void LogComplete();
    }
}