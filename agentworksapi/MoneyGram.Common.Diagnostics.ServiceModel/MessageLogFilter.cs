using System.Text.RegularExpressions;

namespace MoneyGram.Common.Diagnostics.ServiceModel
{
    public class MessageLogFilter
    {
        public Regex RegexMatch { get; set; }
        public string RegexReplacement { get; set; }
    }
}
