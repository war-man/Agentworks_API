namespace TransactionRunner.Interfaces
{
    public interface IResultsPaneViewModel
    {
        void Initialize();
        string ReferenceNumber { get; set; }
        string TimeTaken { get; set; }
    }
}