using System;
using System.IO;
using System.Reflection;
using System.Web.Http;
using Swashbuckle.Application;

namespace AwApi.Documentation
{
    public static class ApiDocumentationManager
    {
        public static string ExecutingDir()
        {
            return Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath);
        }

        public static void Configure(HttpConfiguration configuration)
        {
            // Web API documentation for environments that have ApiDocumentation app key set to true
            configuration
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "AgentWorks.Nxt API")
                        .Description("MoneyGram Web API for transactional clients")
                        .TermsOfService("TERMS OF SERVICE GOES HERE")
                        .License(license => license.Name("LICENSE INFO").Url("http://moneygram.com/"));

                    c.IncludeXmlComments(GetXmlCommentsPathForControllers());
                    c.IncludeXmlComments(GetXmlCommentsPathForModels());
                })
                .EnableSwaggerUi(c => { c.DisableValidator(); });
        }

        private static string GetXmlCommentsPathForControllers()
        {
            return Path.Combine(ExecutingDir(), "AwApi.XML");
        }

        private static string GetXmlCommentsPathForModels()
        {
            return Path.Combine(ExecutingDir(), "AwApi.ViewModels.xml");
        }
    }
}