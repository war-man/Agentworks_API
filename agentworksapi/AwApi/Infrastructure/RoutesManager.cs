using System.Web.Http;

namespace AwApi.Infrastructure
{
   public static class RoutesManager
   {
      public static void AddRoutes(HttpConfiguration configuration)
      {
         configuration.MapHttpAttributeRoutes();

         configuration.Routes.MapHttpRoute(
             "default",
             "api/{controller}/{id}",
             new { id = RouteParameter.Optional });
      }
   }
}