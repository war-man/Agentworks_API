
namespace MoneyGram.PartnerService.DomainModel
{
    public class PointOfSaleDevice
    {
        public decimal Id { get; set; }

        public decimal PosNumber { get; set; }

        public string Status { get; set; }

        public string StatusDescription { get; set; }
    }
}
