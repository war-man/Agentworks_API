using System.Web.Http.Filters;

namespace AwApi.Infrastructure
{
    public class HeaderFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionContext)
        {
            if (!actionContext.ActionContext.RequestContext.Principal.Identity.IsAuthenticated)
            {
                //actionContext.Response.Headers.Add("Access-Control-Expose-Headers", "authRequired");
                //actionContext.Response.Headers.Add("authRequired", "CAMS url");
            }
        }

        //public override Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        //{
        //}
    }
}