using System.Collections.Generic;

namespace MoneyGram.PartnerService.DomainModel.Response
{
    public class PosDeviceResponse : BaseServiceMessage
    {
        public List<PointOfSaleDevice> PosDeviceList;
    }
}
