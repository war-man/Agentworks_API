namespace MoneyGram.AgentConnect.IntegrationTest.Data.Repositories
{
    public interface ITestAgentRepository
    {
        T GetAllValues<T>() where T : class;
        T GetValue<T>(string key) where T : class;
        bool ContainsKey(string key);
        void AddValue<T>(string key, T value);
        void UpdateValue<T>(string key, T value);
        void RemoveValue(string key);
    }
}