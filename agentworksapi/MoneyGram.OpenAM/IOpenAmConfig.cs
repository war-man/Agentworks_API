namespace MoneyGram.OpenAM
{
    public interface IOpenAmConfig
    {
        string OpenAmUrl { get; }
        string OpenAmDeviceUrl { get; }
        string Realm { get; }        
    }
}