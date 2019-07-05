namespace MoneyGram.AgentConnect.IntegrationTest.Data.Setup
{
    public static class Billers
    {
        public static TestBiller Ford => new TestBiller
        {
            Name = "FORD",
            Code = "2074"
        };

        public static TestBiller HubbardAttorney => new TestBiller
        {
            Name = "ATTORNEY RICHARD R HUBBARD",
            Code = "4356",
            ValidAccountNumber = "7418529630"
        };
        public static TestBiller ComcastCableXfinity => new TestBiller
        {
            Name = "COMCAST CABLE - XFINITY",
            Code = "7823",
            ValidAccountNumber = "8798101234567896"
        };
    }

    public class TestBiller
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string ValidAccountNumber { get; set; }
    }
}