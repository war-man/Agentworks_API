using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Globalization;

namespace MoneyGram.Common
{
    public static class ParsingUtility
    {
        private static readonly CultureInfo ParseCultureInfo = new CultureInfo("en-US");

        public static TResult? Parse<TResult>(string toParse, CultureInfo cultureInfo = null)
            where TResult : struct
        { 
            TResult result;
            var parsed = TryParse<TResult>(toParse, out result, cultureInfo);
            return (parsed) ? (TResult?)result : null;
        }

        public static bool TryParse<TResult>(string toParse, out TResult parsed, CultureInfo cultureInfo = null)
        {
            if (toParse == null)
                throw new ArgumentNullException("toParse");

            if (!string.IsNullOrWhiteSpace(toParse))
            {
                var converter = TypeDescriptor.GetConverter(typeof(TResult));
                if (converter.CanConvertFrom(typeof(string)) && converter.IsValid(toParse))
                {
                    parsed = (TResult)converter.ConvertFrom(null, cultureInfo ?? ParseCultureInfo, toParse);
                    return true;
                }
            }

            parsed = default(TResult);
            return false;
        }

        public static TOutput ConvertEnum<TInput, TOutput>(TInput input, bool ignoreCase = false)
            where TInput : struct
            where TOutput : struct
        {
            TOutput result;
            if (Enum.TryParse<TOutput>(input.ToString(), ignoreCase, out result))
            {
                return result;
            }
            throw new ApplicationException(
                string.Format("Unable to automatically map enum of type '{0}' to enum of type '{1}'",
                typeof(TInput).FullName, typeof(TOutput).FullName));
        }
    }
}
