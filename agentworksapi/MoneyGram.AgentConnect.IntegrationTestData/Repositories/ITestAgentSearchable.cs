namespace MoneyGram.AgentConnect.IntegrationTest.Data.Repositories
{
    public interface ITestAgentSearchable
    {
        T Search<T>(string searchTerm) where T : class;
    }
}