using System.Configuration;
using System.Web.Http.Cors;

namespace AwApi.Infrastructure
{
    public static class CorsManager
    {
        public static EnableCorsAttribute CorsAttribute()
        {
            var corsAttribute = new EnableCorsAttribute(
                  ConfigurationManager.AppSettings["CorsOrigins"],
                  "*",      // Headers
                  "*");     // Methods

            // OPTIONS response is cached for 1800 seconds
            // This reduces the number of preflight roundtrips
            // but limits caching to 30 minutes (idle session timeout)
            corsAttribute.PreflightMaxAge = 1800;

            return corsAttribute;            
        }
    }
}