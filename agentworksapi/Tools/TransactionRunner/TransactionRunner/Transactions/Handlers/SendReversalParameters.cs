using System.Xml.Serialization;
using TransactionRunner.ImportExport;

namespace TransactionRunner.Transactions.Handlers
{
    /// <summary>
    ///     Class that encapsulates parameters needed to execute send reversal transactions.
    /// </summary>
    [XmlType("Send_Reversal")]
    public class SendReversalParameters : SendParameters
    {
        /// <summary>
        /// Returns transaction name
        /// </summary>
        public override string TransactionName => "Send Reversal";

        /// <summary>
        ///     Gets or sets the send reversal reason.
        /// </summary>
        [Exportable(ElementName = "Refund_Reason", ColumnHeader = "Refund Reason", Order = 17)]
        public string RefundReason { get; set; }

        /// <summary>
        ///     Gets or sets the refund fee.
        /// </summary>
        [Exportable(ElementName = "Refund_Fee", ColumnHeader = "Refund Fee", Order = 18)]
        public bool RefundFee { get; set; }
    }
}