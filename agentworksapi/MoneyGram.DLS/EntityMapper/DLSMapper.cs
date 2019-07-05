using AutoMapper;
using System;
using System.Linq.Expressions;
using AutoMapper.Configuration;

namespace MoneyGram.DLS.EntityMapper
{
    public static class DLSMapper
    {
        private static readonly MapperConfigurationExpression ConfigurationExpression = new MapperConfigurationExpression();
        private static IMapper _mapper;

        public static void Configure()
        {
            DomainModelExtensions.AgentHeaderExtensions.DefineMappings();
            DomainModelExtensions.BPTransactionDetailLookupRequest1Extensions.DefineMappings();
            DomainModelExtensions.BPTransactionDetailLookupRequestExtensions.DefineMappings();
            DomainModelExtensions.ClientHeaderExtensions.DefineMappings();
            DomainModelExtensions.DailyTransactionDetailLookupRequest1Extensions.DefineMappings();
            DomainModelExtensions.DailyTransactionDetailLookupRequestExtensions.DefineMappings();
            DomainModelExtensions.HeaderExtensions.DefineMappings();
            DomainModelExtensions.MTTransactionDetailLookupRequest1Extensions.DefineMappings();
            DomainModelExtensions.MTTransactionDetailLookupRequestExtensions.DefineMappings();
            DomainModelExtensions.ProcessingInstructionExtensions.DefineMappings();
            DomainModelExtensions.RoutingContextHeaderExtensions.DefineMappings();
            DomainModelExtensions.SecurityHeaderExtensions.DefineMappings();

            ServiceModelExtensions.AgentHeaderExtensions.DefineMappings();
            ServiceModelExtensions.BPTransactionDetailLookupResponseExtensions.DefineMappings();
            ServiceModelExtensions.ClientHeaderExtensions.DefineMappings();
            ServiceModelExtensions.DailyTransactionDetailLookupResponseExtensions.DefineMappings();
            ServiceModelExtensions.HeaderExtensions.DefineMappings();
            ServiceModelExtensions.MTTransactionDetailLookupResponseExtensions.DefineMappings();
            ServiceModelExtensions.ProcessingInstructionExtensions.DefineMappings();
            ServiceModelExtensions.RoutingContextHeaderExtensions.DefineMappings();
            ServiceModelExtensions.SecurityHeaderExtensions.DefineMappings();
            ServiceModelExtensions.TransactionDetailLookupResultExtensions.DefineMappings();

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