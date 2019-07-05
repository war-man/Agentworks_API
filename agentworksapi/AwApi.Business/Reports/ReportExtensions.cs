using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace AwApi.Business.Reports
{
    public static class ReportExtensions
    {
        public static string ComputeSumForCurrencyGroup(
            this IGrouping<string, List<string>> currencyGroup,
            int propertyIndex)
        {
            return currencyGroup.Select(
                    transaction =>
                        decimal.Parse(transaction[propertyIndex],
                            CultureInfo.InvariantCulture))
                .Sum()
                .ToString();
        }
    }
}