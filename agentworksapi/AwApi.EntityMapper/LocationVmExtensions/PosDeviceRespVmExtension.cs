using AwApi.ViewModels.Location;
using MoneyGram.PartnerService.DomainModel;
using MoneyGram.PartnerService.DomainModel.Response;

namespace AwApi.EntityMapper.LocationVmExtensions
{
    public static class PosDeviceRespVmExtension
    {
        public static PosResponse ToVm(this PosDeviceResponse locationsForMoResponse)
        {
            return AwMapper.Map<PosDeviceResponse, PosResponse>(locationsForMoResponse);
        }

        public static void DefineMappings()
        {
            AwMapper.CreateMap<PointOfSaleDevice, Pos>();

            AwMapper.CreateMap<PosDeviceResponse, PosResponse>()
                .ForMember(src => src.PosList, dst => dst.MapFrom(src => src.PosDeviceList));
        }
    }
}