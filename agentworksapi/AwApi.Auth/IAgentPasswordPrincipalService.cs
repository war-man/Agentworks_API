namespace AwApi.Auth
{
    public interface IAgentPasswordPrincipalService
    {
        string GetAgentPassword(string token, string agentId, string posNumber);
    }
}