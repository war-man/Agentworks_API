using System.Xml.Serialization;

namespace TransactionRunner.Transactions.Handlers
{
    /// <summary>
    /// Class that encapsulates parameters needed to execute staged send transactions.
    /// </summary>
    [XmlType("Staged_Send")]
    public class StagedSendParameters : SendParameters
    {
        /// <summary>
        /// Returns transaction name
        /// </summary>
        public override string TransactionName => "Staged Send";
    }
}