using System.Xml.Serialization;

namespace TransactionRunner.Transactions.Handlers
{
    /// <summary>
    /// Class that encapsulates parameters needed to execute staged receive transactions.
    /// </summary>
    [XmlType("Staged_Receive")]
    public class StagedReceiveParameters : SendParameters
    {
        public override string TransactionName => "Staged Receive";
    }
}