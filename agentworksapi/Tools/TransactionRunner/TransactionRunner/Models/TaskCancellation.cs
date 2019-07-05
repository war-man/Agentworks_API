namespace TransactionRunner.Models
{
    public class TaskCancellation
    {
        public bool CancelTasks { get; private set; }
        public TaskCancellation()
        {
            CancelTasks = false;
        }
        public void Cancel()
        {
            CancelTasks = true;
        }
        public void ResetCancellation()
        {
            CancelTasks = false;
        }
    }
}