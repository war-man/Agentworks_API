namespace TransactionRunner.Transactions
{
    /// <summary>
    ///     Enum tracking status of current transactions.
    /// </summary>
    public enum TransactionStatus
    {
        /// <summary>
        ///     Transaction has not been run.
        /// </summary>
        Pending,
        /// <summary>
        ///     Transaction currently running
        /// </summary>
        Running,
        /// <summary>
        ///     Transaction completed successfully
        /// </summary>
        Completed,
        /// <summary>
        ///     Error while running transaction
        /// </summary>
        Error
    }
}