using System;

namespace MoneyGram.DLS.DomainModel
{
    [Serializable]
    public class TransactionDetailLookupResult
    {
        public string PosId { get; set; }

        public string FaceAmount { get; set; }

        public string FeeAmount { get; set; }

        public string TaxAmount { get; set; }

        public string IsoCurrencyCode { get; set; }

        public string FxConsumerRate { get; set; }

        public string AuthCode { get; set; }

        public string PayoutType { get; set; }

        public string ProductId { get; set; }

        public string ReferenceId { get; set; }

        public string AgentLocalTime { get; set; }

        public string EventCode { get; set; }

        public string EventDate { get; set; }

        public string OperatorId { get; set; }

        public string SenderName { get; set; }

        public string IsoSendCountryCode { get; set; }

        public string ReceiverName { get; set; }

        public string IsoIntendedReceiveCountryCode { get; set; }

        public bool InformationalFeeIndicator { get; set; }

        public string[] SerialNumberArray { get; set; }

        public string AlternateAgentId { get; set; }
    }
}
