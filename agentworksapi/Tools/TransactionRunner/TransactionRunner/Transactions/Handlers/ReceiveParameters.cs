using System.Xml.Serialization;

namespace TransactionRunner.Transactions.Handlers
{
    /// <summary>
    /// Class that encapsulates parameters needed to execute receive transactions.
    /// </summary>
    [XmlType("Receive")]
    public class ReceiveParameters : SendParameters
    {
        /// <summary>
        /// Returns transaction name
        /// </summary>
        public override string TransactionName => "Receive";
    }
}