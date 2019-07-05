using MoneyGram.Common.Diagnostics.Ioc;
using Unity;
using Unity.Injection;
using Unity.Interception.ContainerIntegration;
using Unity.Interception.PolicyInjection.MatchingRules;
using Unity.Lifetime;

namespace MoneyGram.PartnerHierarchy.Ioc
{
    public static class UnityConfig
    {
        public static class PartnerHierarchyLayerNames
        {
            public const string BaseRepository = "PH.BaseRepository";
            public const string PhCache = "PH.Cache";
        }
        
        public static void RegisterTypes(IUnityContainer container)
        {
            //assumes unity interception is already enabled.
            container.RegisterType<IPartnerHierarchyConfig, PartnerHierarchyConfig>();            
            container.RegisterType<IPartnerHierarchyRepository, PartnerHierarchyRepository>(PartnerHierarchyLayerNames.BaseRepository);
            container.RegisterType<IPartnerHierarchyClient, PartnerHierarchyClient>();
        }

        public static void EnableLogging(IUnityContainer container)
        {
            container.Configure<Interception>()
                .AddPolicy("logPartnerHierarchy").AddMatchingRule<AssemblyMatchingRule>(
                    new InjectionConstructor(new InjectionParameter("MoneyGram.PartnerHierarchy.Repository"))).AddCallHandler<LogTimingCallHandler>(
                    new HierarchicalLifetimeManager(),
                    new InjectionConstructor());
        }
    }
}