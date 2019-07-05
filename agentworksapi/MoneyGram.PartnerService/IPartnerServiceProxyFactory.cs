using MoneyGram.PartnerService.Service;

namespace MoneyGram.PartnerService
{
    public interface IPartnerServiceProxyFactory
    {
        WebPOEPartnerServicePortType CreateProxy();
    }
}
