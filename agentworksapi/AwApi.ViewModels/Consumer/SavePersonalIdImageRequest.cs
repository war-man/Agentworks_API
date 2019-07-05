using System;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace AwApi.ViewModels.Consumer
{
    [Serializable]
    public class SavePersonalIDImageRequest : Request
    {
        public string ConsumerProfileID { get; set; }
        public string ConsumerProfileIDType { get; set; }
        public PersonalIDImageContentType PersonalIDImage { get; set; }
    }
}