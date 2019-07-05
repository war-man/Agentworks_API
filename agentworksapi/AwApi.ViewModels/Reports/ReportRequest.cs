using System.Collections.Generic;

namespace AwApi.ViewModels.Reports
{
    public class ReportRequest
    {
        public List<string> PosIds { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public IEnumerable<string> Locations { get; set; }

        public string DeliveryOption { get; set; }

        public string ProductVariant { get; set; }

        public string EventType { get; set; }

        public bool IsDetailed { get; set; }
    }
}