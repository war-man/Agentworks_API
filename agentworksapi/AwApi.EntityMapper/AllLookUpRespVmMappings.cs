using AwApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwApi.EntityMapper
{
    public class AllLookUpRespVmMappings
    {
        public static void DefineMappings()
        {
            AwMapper.CreateMap<CountryLookupRespVm, AllLookupRespVm>()
                .ForMember(dst => dst.Countries, opt => opt.MapFrom(src => src.Countries))
                .ForMember(dst => dst.CurrencyInfo, opt => opt.Ignore())
                .ForMember(dst => dst.States, opt => opt.Ignore());

            AwMapper.CreateMap<CurrencyLookupRespVm, AllLookupRespVm>()
                .ForMember(dst => dst.Countries, opt => opt.Ignore())
                .ForMember(dst => dst.CurrencyInfo, opt => opt.MapFrom(src => src.CurrencyInfo))
                .ForMember(dst => dst.States, opt => opt.Ignore());

            AwMapper.CreateMap<StatesLookupRespVm, AllLookupRespVm>()
                .ForMember(dst => dst.Countries, opt => opt.Ignore())
                .ForMember(dst => dst.CurrencyInfo, opt => opt.Ignore())
                .ForMember(dst => dst.States, opt => opt.MapFrom(src => src.States));
        }
    }
}
