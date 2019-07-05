using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Collections.Generic;
using AwApi.ViewModels;

namespace AwApi.Auth
{
    public class AwAuthorization : AuthorizeAttribute
    {
        public AwAuthorization(params string[] roles) : base()
        {
            var rolesList = new List<string>(roles);

            if(rolesList.Contains(MgiAwRole.TellerManagerAdmin))
            {
                Roles = MgiAwRole.GetApplicationDefinedRoles(MgiAwRole.TellerManagerAdmin);
                rolesList.Remove(MgiAwRole.TellerManagerAdmin);
            }

            if(rolesList.Any())
            {
                if(!string.IsNullOrEmpty(Roles))
                {
                    Roles += ",";
                }
                Roles += string.Join(",", rolesList);
            }
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var currentPrincipal = (GenericPrincipal) Thread.CurrentPrincipal;

            var authenticated = currentPrincipal.Identity.IsAuthenticated;
            var inRole = (string.IsNullOrEmpty(Roles) || Roles.Split(',').Any(currentPrincipal.IsInRole));

            if(!authenticated)
            {
                AuthHelper.SetUnauthenticatedResponse();
            }
            if(!inRole)
            {
                AuthHelper.SetNotInRoleResponse();
            }
            return (authenticated && inRole);
        }
    }
}