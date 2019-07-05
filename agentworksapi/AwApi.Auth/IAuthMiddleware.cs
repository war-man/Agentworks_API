using Microsoft.Owin;
using System;
using System.Threading.Tasks;

namespace AwApi.Auth
{
    public interface IAuthMiddleware
    {
        Task Invoke(IOwinContext context, Func<Task> next);
    }
}