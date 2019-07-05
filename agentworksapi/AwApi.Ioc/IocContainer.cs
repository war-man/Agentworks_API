using System;
using AwApi.Business;
using AwApi.Business.LocationBusiness;
using AwApi.Business.Reports;
using AwApi.Business.Reports.Containers;
using AwApi.Business.Reports.Generators;
using AwApi.Integration;
using AwApi.Integration.Event;
using AwApi.Integration.Reports;
using MoneyGram.AgentConnect;
using MoneyGram.Common.Cache;
using MoneyGram.Common.Cache.Couchbase;
using MoneyGram.Common.Cache.InMemory;
using MoneyGram.Common.Cache.NoCache;
using MoneyGram.PartnerService;
using MoneyGram.PartnerHierarchy;
using AwApi.Cache.CouchBase;
using System.Configuration;
using MoneyGram.OpenAM;
using MoneyGram.OpenIDM;
using MoneyGram.DLS;
using MoneyGram.DLS.EntityMapper;
using MoneyGram.PartnerService.EntityMapper;
using MoneyGram.Common.Diagnostics.ServiceModel;
using System.Collections.Generic;
using System.ServiceModel.Description;
using System.Text.RegularExpressions;
using AwApi.Auth;
using AwApi.Auth.ApiKey;
using AwApi.Auth.Device;
using AwApi.Auth.Fake;
using AwApi.Auth.Oidc;
using AwApi.Auth.SupportKey;
using AwApi.Auth.UnregisteredDevice;
using Microsoft.Extensions.DependencyInjection;

namespace AwApi.Ioc
{
    public sealed class IocContainer : IDisposable
    {
        private readonly IocConfiguration _iocConfig;

        public IocContainer(IocConfiguration configuration)
        {
            _iocConfig = configuration;
            Services = new ServiceCollection();

            RegisterTypes();
        }

        public IServiceCollection Services { get; }

        public void Dispose()
        {
            Services.Clear();
        }

        public IServiceProvider BuildServiceProvider()
        {
            return this.Services.BuildServiceProvider();
        }

        private void RegisterTypes()
        {
            // Business
            Services.AddSingleton<ICommonBusiness, CommonBusiness>();
            Services.AddSingleton<ISendBusiness, SendBusiness>();
            Services.AddSingleton<IStagedSendBusiness, StagedSendBusiness>();
            Services.AddSingleton<IStagedBillPayBusiness, StagedBillPayBusiness>();
            Services.AddSingleton<IAmendBusiness, AmendBusiness>();
            Services.AddSingleton<ISendReversalBusiness, SendReversalBusiness>();
            Services.AddSingleton<IReceiveBusiness, ReceiveBusiness>();
            Services.AddSingleton<IReceiveReversalBusiness, ReceiveReversalBusiness>();
            Services.AddSingleton<IBillPayBusiness, BillPayBusiness>();
            Services.AddSingleton<IDepositBusiness, DepositBusiness>();
            Services.AddSingleton<IDocumentBusiness, DocumentBusiness>();

            Services.AddSingleton<ILanguageBusiness, LanguageBusiness>();
            Services.AddSingleton<ILocationBusiness, LocationBusiness>();
            Services.AddSingleton<IOperatorBusiness, OperatorBusiness>();
            Services.AddSingleton<IBankBusiness, BankBusiness>();
            Services.AddSingleton<IInternalBusiness, InternalBusiness>();
            Services.AddSingleton<IMoneyOrderBusiness, MoneyOrderBusiness>();

            Services.AddSingleton<IConsumerBusiness, ConsumerBusiness>();
            Services.AddSingleton<ILookupBusiness, LookupBusiness>();
            Services.AddSingleton<IProfileBusiness, ProfileBusiness>();
            Services.AddSingleton<IEditTransferBusiness, EditTransferBusiness>();
            Services.AddSingleton<ISupportBusiness, SupportBusiness>();
            Services.AddSingleton<IEventBusiness, EventBusiness>();

            // REPORTS
            Services.AddSingleton<IReportsBusiness, ReportsBusiness>();
            Services.AddSingleton<IDailyTranDetailContainer, DailyTranDetailContainer>();
            Services.AddSingleton<IDailyTranSummaryContainer, DailyTranSummaryContainer>();
            Services.AddSingleton<IDailyTranDetailReportGenerator, DailyTranDetailReportGenerator>();
            Services.AddSingleton<IBillPaymentSummaryGenerator, BillPaymentSummaryGenerator>();
            Services.AddSingleton<IReceiveSummaryGenerator, ReceiveSummaryGenerator>();
            Services.AddSingleton<ISendSummaryGenerator, SendSummaryGenerator>();

            //INTEGRATION FOR Events
            Services.AddSingleton<IEventIntegration, EventIntegration>();
            Services.AddSingleton<IDlsIntegration, DlsIntegration>();
            Services.AddSingleton<IPartnerServiceIntegration, PartnerServiceIntegration>();
            Services.AddSingleton<IPartnerHierarchyIntegration, PartnerHierarchyIntegration>();
            Services.AddSingleton<IAgentConnectIntegration, AgentConnectIntegration>();
            Services.AddSingleton<IOpenAmIntegration, OpenAmIntegration>();
            Services.AddSingleton<IOpenIdmIntegration, OpenIdmIntegration>();
            Services.AddSingleton<ILanguageIntegration, LanguageIntegration>();
            Services.AddSingleton<IReceiptIntegration, ReceiptIntegration>();
            Services.AddSingleton<ICacheIntegration, CacheIntegration>();

            // CACHING
            switch (_iocConfig.CacheType)
            {
                case CacheTypeEnum.Couchbase:
                    Services.AddSingleton<ICacheManager, CouchbaseCacheManager>();
                    Services.AddSingleton<ICouchbaseCacheConfig>(cf =>
                    {
                        return CouchbaseCacheConfigHelper.GetConfigModel();
                    });
                    break;
                case CacheTypeEnum.InMemory:
                    Services.AddSingleton<ICacheManager, InMemoryCacheManager>();
                    break;
                case CacheTypeEnum.NoCache:
                    Services.AddSingleton<ICacheManager, NoCacheManager>();
                    break;
                default:
                    Services.AddSingleton<ICacheManager, InMemoryCacheManager>();
                    break;
            }

            // IOC FOR AUTH
            Services.AddSingleton<IAuthMiddleware, AuthMiddleware>();
            Services.AddSingleton<IAgentProfilePrincipalService, AgentProfilePrincipalService>();
            Services.AddSingleton<IAgentPasswordPrincipalService, AgentPasswordPrincipalService>();
            Services.AddSingleton<IOpenAmPrincipalService, OpenAmPrincipalService>();
            Services.AddSingleton<IFakeAuthPrincipalCreator, FakeAuthPrincipalCreator>();
            Services.AddSingleton<IApiKeyPrincipalCreator, ApiKeyPrincipalCreator>();
            Services.AddSingleton<ISupportAuthPrincipalCreator, SupportAuthPrincipalCreator>();
            Services.AddSingleton<IOidcAuthPrincipalCreator, OidcAuthPrincipalCreator>();
            Services.AddSingleton<IDeviceAuthPrincipalCreator, DeviceAuthPrincipalCreator>();
            Services.AddSingleton<IUnregisteredDeviceAuthPrincipalCreator, UnregisteredDeviceAuthPrincipalCreator>();

            // IOC FOR OpenIDM
            ConfigureOpenIdm(Services);

            // IOC FOR OpenAM
            ConfigureOpenAm(Services);

            // IOC FOR AgentConnect
            ConfigureAgentConnect(Services);

            // IOC FOR DLS(for daily transaction reports)
            ConfigureDls(Services);

            // IOC FOR User Activity Report
            ConfigurePartnerService(Services);

            // IOC For Partner Hierarchy
            ConfigurePartnerHierarchy(Services);

            // Registyer PartnerHierarchyIntegration -> inject Ph Cache
            
            Services.AddSingleton<IAgentConnect, AgentConnectRepository>();
            Services.Decorate<IAgentConnect, AgentConnectTraining>();
            Services.Decorate<IAgentConnect, AgentConnectCache>();
        }

        private void ConfigurePartnerHierarchy(IServiceCollection services)
        {
            services.AddSingleton<IPartnerHierarchyConfig>( cfg => {
                return new PartnerHierarchyConfig
                {
                    PartnerHierarchyUrl = ConfigurationManager.AppSettings["PartnerHierarchyUrl"]
                };
            });

            services.AddSingleton<IPartnerHierarchyRepository, PartnerHierarchyRepository>();
            services.AddSingleton<IPartnerHierarchyClient, PartnerHierarchyClient>();
        }

        private void ConfigureAgentConnect(IServiceCollection services)
        {
            var messageLogger = new MessageLogger
            {
                MessageLogFilters = new List<MessageLogFilter>
                {
                    new MessageLogFilter
                    {
                        RegexMatch = ConstructRegex(_iocConfig.RegexStringList),
                        RegexReplacement = "<aw:filteredText>...</aw:filteredText>"
                    }
                }
            };

            services.AddSingleton<IAgentConnectConfig>( cfg => {
                return new AgentConnectConfig
                {
                    AgentConnectUrl = ConfigurationManager.AppSettings["AgentConnectUrl"],
                    ApiVersion = ConfigurationManager.AppSettings["ApiVersion"],
                    ChannelType = ConfigurationManager.AppSettings["ChannelType"],
                    ClientSoftwareVersion = ConfigurationManager.AppSettings["ClientSoftwareVersion"],
                    PoeType = ConfigurationManager.AppSettings["PoeType"],
                    TargetAudience = ConfigurationManager.AppSettings["TargetAudience"],
                    EndpoingBehaviors = new List<IEndpointBehavior> { messageLogger }
                };
            });

            services.AddSingleton<IAgentConnectProxyFactory, AgentConnectProxyFactory>();
            Services.AddSingleton<MoneyGram.AgentConnect.ITrainingModeRepository,
                    MoneyGram.AgentConnect.TrainingModeRepository>();
        }

        private Regex ConstructRegex(List<string> tags)
        {
            var regexString = "<(?:" + string.Join("|", tags) + ").*>.*</(?:" + string.Join("|", tags) + ")>";
            return new Regex(regexString, RegexOptions.Singleline);
        }

        private void ConfigureDls(IServiceCollection services)
        {
            DLSMapper.Configure();

            services.AddSingleton<IDLSConfig>( dlscfg => {
                return new DLSConfig
                {
                    DLSUrl = ConfigurationManager.AppSettings["DLSUrl"],
                    EndpoingBehaviors = new List<IEndpointBehavior> { new MessageLogger() }
                };
            });

            services.AddSingleton<IDLSProxyFactory, DLSProxyFactory>();
            services.AddSingleton<MoneyGram.DLS.ITrainingModeRepository, MoneyGram.DLS.TrainingModeRepository>();

            services.AddSingleton<IDLSRepository, DLSRepository>();
            services.Decorate<IDLSRepository, DLSRepositoryTraining>();
        }

        private void ConfigureOpenAm(IServiceCollection services)
        {
            services.AddSingleton<IOpenAmConfig>(ocfg => {
                return new OpenAmConfig
                {
                    OpenAmDeviceUrl = ConfigurationManager.AppSettings["OpenAmDeviceUrl"],
                    OpenAmUrl = ConfigurationManager.AppSettings["OpenAmUrl"],
                    Realm = ConfigurationManager.AppSettings["OpenAmRealm"],
                };
            });

            services.AddSingleton<IOpenAmClient, OpenAmClient>();
            services.AddSingleton<IOpenAmRepository, OpenAmRepository>();
        }

        private void ConfigureOpenIdm(IServiceCollection services)
        {
            services.AddSingleton<IOpenIdmConfig>( cf => {
                return new OpenIdmConfig
                {
                    OpenIdmUrl = ConfigurationManager.AppSettings["OpenIdmUrl"]
                };
            });

            services.AddSingleton<IOpenIdmClient, OpenIdmClient>();
            services.AddSingleton<IOpenIdmRepository, OpenIdmRepository>();
        }

        private void ConfigurePartnerService(IServiceCollection services)
        {

            services.AddSingleton<IPartnerServiceConfig>( ps => {
                return new PartnerServiceConfig
                {
                    PartnerServiceUrl = ConfigurationManager.AppSettings["PartnerServiceUrl"],
                    EndpoingBehaviors = new List<IEndpointBehavior> { new MessageLogger() }
                };
            });

            services.AddSingleton<IPartnerServiceProxyFactory, PartnerServiceProxyFactory>();

            services.AddSingleton<IPartnerServiceRepository, PartnerServiceRepository>();
            services.Decorate<IPartnerServiceRepository, PartnerServiceCache>();

            PsMapper.Configure();
        }
    }
}