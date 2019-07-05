using AutoMapper;
using AutoMapper.Configuration;
using AwApi.EntityMapper.ViewModelExtensions;

namespace AwApi.EntityMapper
{
    public static class AwMapper
    {
        private static readonly MapperConfigurationExpression ConfigurationExpression = new MapperConfigurationExpression();
        private static IMapper _mapper;

        public static void Configure()
        {
            LocationVmExtensions.AgentExtensions.DefineMappings();
            LocationVmExtensions.AgentLocationMoRespExtensions.DefineMappings();
            LocationVmExtensions.AgentsRespExtensions.DefineMappings();
            LocationVmExtensions.LocationsForMoRespExtensions.DefineMappings();
            LocationVmExtensions.POEAgentLocationMoRespExtensions.DefineMappings();
            LocationVmExtensions.POELocationsForMoRespExtensions.DefineMappings();
            LocationVmExtensions.PosDeviceRespVmExtension.DefineMappings();
            
            DwRegisterDeviceReqVmExtensions.DefineMappings();

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
    }
}