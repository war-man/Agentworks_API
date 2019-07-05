namespace MoneyGram.OpenAM
{
    public class OpenAmConfig : IOpenAmConfig
    {
        public string OpenAmUrl { get; set; }
        public string OpenAmDeviceUrl { get; set; }

        public string Realm { get; set; }
    }
}