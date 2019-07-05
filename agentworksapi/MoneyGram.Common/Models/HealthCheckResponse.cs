namespace MoneyGram.Common.Models
{
    public class HealthCheckResponse
    {
        public string ServiceName { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}