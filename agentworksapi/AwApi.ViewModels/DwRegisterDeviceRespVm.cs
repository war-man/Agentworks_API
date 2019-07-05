using System;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace AwApi.ViewModels
{
    [Serializable]
    public class DwRegisterDeviceRespVm : Response
    {
        public string AgentLocationId { get; set; }
        public string AgentName { get; set; }
        public string AgentPhoneNumber { get; set; }
        public string AgentAddress1 { get; set; }
        public string AgentAddress2 { get; set; }
        public string AgentAddress3 { get; set; }
        public string AgentCity { get; set; }
        public string AgentState { get; set; }
        public string AgentZip { get; set; }
        public string AgentCountry { get; set; }
        public string AgentTimeZone { get; set; }
        public string MainOfficeId { get; set; }
        public bool Success { get; set; }
    }
}