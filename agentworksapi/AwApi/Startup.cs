using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dependencies;
using AwApi;
using AwApi.Auth;
using AwApi.Documentation;
using AwApi.EntityMapper;
using AwApi.Infrastructure;
using AwApi.Ioc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;
using MoneyGram.Common.Cache;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace AwApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var configuration = new HttpConfiguration();

            // enable logging
            configuration.Filters.Add(new DiagnosticsFilterAttribute());

            // Web API configuration and services
            // CORS Attribute
            configuration.EnableCors(CorsManager.CorsAttribute());

            // Ensure HTTPS over TLS 1.2 for External and Prod
            SecurityProtocolManager.SetSecurityProtocol();

            // Set application specific Json Serialization settings
            SerializerManager.ApplyCustomSettings(configuration.Formatters);

            // Web API routes and application routes
            RoutesManager.AddRoutes(configuration);

            // Owin level exception handling
            app.Use<GlobalExceptionMiddleware>();

            // IoC and DI configuration
            configuration.DependencyResolver = ConfigureIocResolver();

            // Middleware for properties stored on the LogicalThreadContext for log4net and other purposes
            app.Use<RequestMetadataMiddleware>();

            var authMiddleware = (AuthMiddleware)configuration.DependencyResolver.GetService(typeof(IAuthMiddleware));
            app.Use(async (context, next) =>
            {
                await authMiddleware.Invoke(context, next);
            });

            // Global exception handler for AC errors
            configuration.Filters.Add(new MgiExceptionFilter());
            // Filter to add custom headers to the response
            configuration.Filters.Add(new HeaderFilter());
            AwMapper.Configure();

            // Web API documentation configuration
            // Web API documentation for environments that have ApiDocumentation app key set to true
            var keyName = "ApiDocumentation";
            var apiDocKeyExists = ConfigurationManager.AppSettings.AllKeys.Contains(keyName);
            if(apiDocKeyExists && bool.Parse(ConfigurationManager.AppSettings[keyName]))
            {
                ApiDocumentationManager.Configure(configuration);
            }

            app.UseWebApi(configuration);
        }

        private IDependencyResolver ConfigureIocResolver()
        {
            // Setup configuration for IOC container here
            var config = new IocConfiguration();
            config.CacheType = GetCacheManagerType();
            config.RegexStringList = GetLoggingRegularExpressionList();
            var iocContainer = new IocContainer(config);

            var controllerTypes = Assembly.GetExecutingAssembly().GetExportedTypes()
              .Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition)
              .Where(t => typeof(ApiController).IsAssignableFrom(t) || t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase));

            foreach (var type in controllerTypes)
            {
                iocContainer.Services.AddTransient(type);
            }

            var iocResolver = new DefaultDependencyResolver(iocContainer.BuildServiceProvider());

            return iocResolver;
        }
        private CacheTypeEnum GetCacheManagerType()
        {
            try
            {
                var configValue = ConfigurationManager.AppSettings["CacheTypeEnum"];
                return (CacheTypeEnum)Enum.Parse(typeof(CacheTypeEnum), configValue);
            }
            catch (Exception ex)
            {
                // We will default to InMemory.
                return CacheTypeEnum.InMemory;
            }
        }
        private List<string> GetLoggingRegularExpressionList()
        {
            var configValue = ConfigurationManager.AppSettings["NlogRegularExpressions"];
            if(configValue != null)
            {
                var splitExpressions = configValue.Split(',').ToList();
                if (splitExpressions.Any())
                {
                    return splitExpressions;
                }
            }
            return new List<string>();
        }
    }
}