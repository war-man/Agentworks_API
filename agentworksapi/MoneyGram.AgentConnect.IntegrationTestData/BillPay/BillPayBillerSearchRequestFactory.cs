using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.BillPay
{
    public static class BillPayBillerSearchRequestFactory
    {
        public static BillerSearchRequest BillerSearchRequestByNameForEP(string billerName)
        {
            return new BillerSearchRequest
            {
                BillerName = billerName,
                SearchType = BillerSearchType.NAME,
                ProductVariant = ProductVariantType.EP
            };
        }
        public static BillerSearchRequest BillerSearchRequestByCodeForEP(string receiveCode)
        {
            return new BillerSearchRequest
            {
                ReceiveCode = receiveCode,
                SearchType = BillerSearchType.CODE,
                ProductVariant = ProductVariantType.EP
            };
        }
    }
}