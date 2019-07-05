using System;

namespace MoneyGram.PartnerService.DomainModel.Request
{
    [Serializable]
    public class UserIdExistsRequest : BaseServiceMessage
    {
        public decimal MainofficeId { get; set; }

        public string OperatorId { get; set; }
    }
}
