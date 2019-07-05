namespace TransactionRunner.ImportExport
{
    /// <summary>
    /// Mode of file export
    /// </summary>
    public enum ExportMode
    {
        /// <summary>
        /// Overwrites existing file completely
        /// </summary>
        Overwrite,

        /// <summary>
        /// Appends transaction data to existing file
        /// </summary>
        Append
    }
}