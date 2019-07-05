using System;

namespace AwApi.ViewModels.Reports
{
    public class TransactionDetail
    {
        public DateTime LocalTime { get; set; }
        public string ReferenceNumber { get; set; }
        public string UserId { get; set; }
        public string PosNumber { get; set; }
        public string Currency { get; set; }
        public double TotalAmount { get; set; }
    }
}