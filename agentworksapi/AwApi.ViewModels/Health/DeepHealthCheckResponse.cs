using MoneyGram.Common.Models;
using System.Collections.Generic;

namespace AwApi.ViewModels.Health
{
    public class DeepHealthCheckResponse
    {
        public IEnumerable<HealthCheckResponse> HealthStatus { get; set; }
    }
}