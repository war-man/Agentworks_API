using System.Configuration;

namespace AwApi.ViewModels
{
    public static class AwVersion
    {
        /// <summary>
        /// AgentWorks Web API code version
        /// MAJOR VERSION - incremented once per release
        /// MINOR VERSION - incremented once per sprint
        /// PATCH VERSION - incremented with each commit
        /// </summary>
        public static string Api => "14.0.5";

        public static string Ac => ConfigurationManager.AppSettings["ApiVersion"];
    }
}