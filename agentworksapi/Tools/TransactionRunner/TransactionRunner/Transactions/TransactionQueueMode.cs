namespace TransactionRunner.Transactions
{
    /// <summary>
    /// Mode for running multiple transactions
    /// </summary>
    public enum TransactionQueueMode
    {
        /// <summary>
        /// Batch mode (from application main window)
        /// </summary>
        Batch,

        /// <summary>
        /// Import mode (from Import window)
        /// </summary>
        Import
    }
}