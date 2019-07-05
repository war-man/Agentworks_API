using System;
using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.Configuration;

namespace MoneyGram.PartnerService.EntityMapper
{
    public static class PsMapper
    {
        private static readonly MapperConfigurationExpression ConfigurationExpression = new MapperConfigurationExpression();
        private static IMapper _mapper;

        public static void Configure()
        {
            ServiceModelExtensions.AgentExtensions.DefineMappings();
            ServiceModelExtensions.AgentLocationMoResponseExtensions.DefineMappings();
            ServiceModelExtensions.POEAgentLocationMoResponseExtensions.DefineMappings();
            ServiceModelExtensions.AgentPasswordResponseExtensions.DefineMappings();
            ServiceModelExtensions.AgentPosDeviceExtensions.DefineMappings();
            ServiceModelExtensions.AgentsDeviceNamesResponseExtensions.DefineMappings();
            ServiceModelExtensions.AgentsResponseExtensions.DefineMappings();
            ServiceModelExtensions.ClientHeaderExtensions.DefineMappings();
            ServiceModelExtensions.HeaderExtensions.DefineMappings();
            ServiceModelExtensions.LocationsForMoResponseExtensions.DefineMappings();
            ServiceModelExtensions.POELocationsForMoResponseExtensions.DefineMappings();
            ServiceModelExtensions.PointOfSaleDeviceExtensions.DefineMappings();
            ServiceModelExtensions.PosDeviceResponseExtensions.DefineMappings();
            ServiceModelExtensions.ProcessingInstructionExtensions.DefineMappings();
            ServiceModelExtensions.RoutingContextHeaderExtensions.DefineMappings();
            ServiceModelExtensions.SecurityHeaderExtensions.DefineMappings();
            ServiceModelExtensions.TransactionExceedInfoResponseExtensions.DefineMappings();
            ServiceModelExtensions.TransactionExceedReportsInfoExtensions.DefineMappings();
            ServiceModelExtensions.UserIdExistsResponseExtensions.DefineMappings();
            ServiceModelExtensions.UserReportsInfoResponseExtensions.DefineMappings();
            ServiceModelExtensions.UserReportsInfoResponseListExtensions.DefineMappings();

            DomainModelExtensions.AgentPosDeviceExtensions.DefineMappings();
            DomainModelExtensions.ClientHeaderExtensions.DefineMappings();
            DomainModelExtensions.getAgentLocationMoRequest1Extensions.DefineMappings();
            DomainModelExtensions.GetAgentLocationMoRequestExtensions.DefineMappings();
            DomainModelExtensions.getPOEAgentLocationMoRequest1Extensions.DefineMappings();
            DomainModelExtensions.GetPOEAgentLocationMoRequestExtensions.DefineMappings();
            DomainModelExtensions.getAgentPasswordRequest1Extensions.DefineMappings();
            DomainModelExtensions.GetAgentPasswordRequestExtensions.DefineMappings();
            DomainModelExtensions.getAgentsDeviceNamesRequest1Extensions.DefineMappings();
            DomainModelExtensions.GetAgentsDeviceNamesRequestExtensions.DefineMappings();
            DomainModelExtensions.getAgentsRequest1Extensions.DefineMappings();
            DomainModelExtensions.GetAgentsRequestExtensions.DefineMappings();
            DomainModelExtensions.getLocationsForMoRequest1Extensions.DefineMappings();
            DomainModelExtensions.GetLocationsForMoRequestExtensions.DefineMappings();
            DomainModelExtensions.getPOELocationsForMoRequest1Extensions.DefineMappings();
            DomainModelExtensions.GetPOELocationsForMoRequestExtensions.DefineMappings();
            DomainModelExtensions.getPOSDeviceRequest1Extensions.DefineMappings();
            DomainModelExtensions.GetPOSDeviceRequestExtensions.DefineMappings();
            DomainModelExtensions.getTransactionExceedInfoRequest1Extensions.DefineMappings();
            DomainModelExtensions.GetTransactionExceedInfoRequestExtensions.DefineMappings();
            DomainModelExtensions.getUserIdExistsRequest1Extensions.DefineMappings();
            DomainModelExtensions.GetUserIdExistsRequestExtensions.DefineMappings();
            DomainModelExtensions.getUserReportsInfoRequest1Extensions.DefineMappings();
            DomainModelExtensions.GetUserReportsInfoRequestExtensions.DefineMappings();
            DomainModelExtensions.HeaderExtensions.DefineMappings();
            DomainModelExtensions.ProcessingInstructionExtensions.DefineMappings();
            DomainModelExtensions.RoutingContextHeaderExtensions.DefineMappings();
            DomainModelExtensions.SecurityHeaderExtensions.DefineMappings();

            // Initialize AutoMapper
            var mappingConfig = new MapperConfiguration(ConfigurationExpression);

            mappingConfig.AssertConfigurationIsValid();

            _mapper = mappingConfig.CreateMapper();
        }

        public static TDestination Map<TSource, TDestination>(TSource source)
        {
            return _mapper.Map<TSource, TDestination>(source);
        }

        public static TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return _mapper.Map<TSource, TDestination>(source, destination);
        }

        public static IMappingExpression<TSource, TDestination> CreateMap<TSource, TDestination>()
        {
            return ConfigurationExpression.CreateMap<TSource, TDestination>();
        }

        public static IMappingExpression<TSource, TDestination> Ignore<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> map,
            Expression<Func<TDestination, object>> selector)
        {
            map.ForMember(selector, config => config.Ignore());
            return map;
        }
    }
}