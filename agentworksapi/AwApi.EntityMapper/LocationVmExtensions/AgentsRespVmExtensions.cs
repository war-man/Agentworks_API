using AwApi.ViewModels.Location;
using MoneyGram.PartnerService.DomainModel;
using MoneyGram.PartnerService.DomainModel.Response;

namespace AwApi.EntityMapper.LocationVmExtensions
{
    public static class AgentExtensions
    {
        public static void DefineMappings()
        {
            AwMapper.CreateMap<Agent, AgentVm>();
        }
    }

    public static class AgentsRespExtensions
    {
        public static AgentsRespVm ToVm(this AgentsResponse agentLocationMoResponse)
        {
            return AwMapper.Map<AgentsResponse, AgentsRespVm>(agentLocationMoResponse);
        }

        public static void DefineMappings()
        {
            AwMapper.CreateMap<AgentsResponse, AgentsRespVm>()
                .ForMember(src => src.Agents, dst => dst.MapFrom(src => src.AgentList));
        }
    }

    public static class AgentLocationMoRespExtensions
    {
        public static AgentsRespVm ToVm(this AgentLocationMoResponse agentLocationMoResponse)
        {
            return AwMapper.Map<AgentLocationMoResponse, AgentsRespVm>(agentLocationMoResponse);
        }

        public static void DefineMappings()
        {
            AwMapper.CreateMap<AgentLocationMoResponse, AgentsRespVm>()
                .ForMember(src => src.Agents, dst => dst.MapFrom(src => src.AgentList));
        }
    }

    public static class POEAgentLocationMoRespExtensions
    {
        public static AgentsRespVm ToVm(this POEAgentLocationMoResponse poeAgentLocationMoResponse)
        {
            return AwMapper.Map<POEAgentLocationMoResponse, AgentsRespVm>(poeAgentLocationMoResponse);
        }

        public static void DefineMappings()
        {
            AwMapper.CreateMap<POEAgentLocationMoResponse, AgentsRespVm>()
                .ForMember(src => src.Agents, dst => dst.MapFrom(src => src.AgentList));
        }
    }

    public static class LocationsForMoRespExtensions
    {
        public static AgentsRespVm ToVm(this LocationsForMoResponse locationsForMoResponse)
        {
            return AwMapper.Map<LocationsForMoResponse, AgentsRespVm>(locationsForMoResponse);
        }

        public static void DefineMappings()
        {
            AwMapper.CreateMap<LocationsForMoResponse, AgentsRespVm>()
                .ForMember(src => src.Agents, dst => dst.MapFrom(src => src.AgentList));
        }
    }

    public static class POELocationsForMoRespExtensions
    {
        public static AgentsRespVm ToVm(this POELocationsForMoResponse locationsForMoResponse)
        {
            return AwMapper.Map<POELocationsForMoResponse, AgentsRespVm>(locationsForMoResponse);
        }

        public static void DefineMappings()
        {
            AwMapper.CreateMap<POELocationsForMoResponse, AgentsRespVm>()
                .ForMember(src => src.Agents, dst => dst.MapFrom(src => src.AgentList));
        }
    }
}