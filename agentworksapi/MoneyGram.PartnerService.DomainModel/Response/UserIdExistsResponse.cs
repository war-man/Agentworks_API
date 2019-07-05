using System;

namespace MoneyGram.PartnerService.DomainModel.Response
{
    [Serializable]
    public class UserIdExistsResponse : BaseServiceMessage
    {
        public string UserId { get; set; }
    }
}
