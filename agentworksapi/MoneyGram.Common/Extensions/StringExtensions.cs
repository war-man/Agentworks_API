using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;

namespace MoneyGram.Common.Extensions
{
    public static class StringExtensions
    {
        private const string TokenGroupName = "token";
        private const string LengthGroupName = "length";
        private const string FormatGroupName = "format";
        private const string ValueGroupName = "value";

        private readonly static Regex TokenMatchRegex = new Regex("{(?<token>\\w+)(?:,(?<length>\\d+))?(?::(?<format>\\w+))?}", RegexOptions.Compiled);
        private readonly static Regex StripTagsRegex = new Regex(@"<(.|\n)*?>", RegexOptions.Compiled);
        private readonly static Regex ExcessWhitespaceRegex = new Regex(@"\s+", RegexOptions.Compiled);
        private readonly static Regex MarkupFirstIdRegex = new Regex("^<[^>]*id=[\"|'](?<value>\\w+)[\"|']", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public const char MaskChar = '●';
        public const string MaskString = "●";

        public static string RemovePrefix(this string subject, string prefix, StringComparison stringComparisonType = StringComparison.Ordinal)
        {
            if (!subject.StartsWith(prefix, stringComparisonType))
                return subject;

            return subject.Remove(0, prefix.Length);
        }

        public static string RemoveSuffix(this string subject, string suffix, StringComparison stringComparisonType = StringComparison.Ordinal)
        {
            if (!subject.EndsWith(suffix))
                return subject;

            var startIndex = subject.Length - suffix.Length;
            return subject.Remove(startIndex);
        }

        public static bool ContainsToken(this string text, char tokenDelimiter = '{')
        {
            if (string.IsNullOrWhiteSpace(text))
                return false;

            return text.IndexOf(tokenDelimiter) >= 0;
        }

        public static string PerformTokenSubstitution(this string text, Dictionary<string, object> tokens)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException("toProcess");
            if (tokens == null)
                throw new ArgumentNullException("tokens");

            return TokenMatchRegex.Replace(text,
                match =>
                {
                    var token = match.Groups[TokenGroupName];
                    if (token == null)
                        return match.Value;

                    object tokenValue;
                    if (!tokens.TryGetValue(token.Value, out tokenValue))
                        return match.Value;

                    if (tokenValue == null) //Determine if we want to return "" or match.Value...
                        return string.Empty;

                    var length = match.Groups[LengthGroupName].Value;
                    var hasLength = !string.IsNullOrEmpty(length);
                    var format = match.Groups[FormatGroupName].Value;
                    var hasFormat = !string.IsNullOrEmpty(format);

                    if (!hasLength && !hasFormat)
                        return tokenValue.ToString();

                    var tokenFormat = string.Concat("{0",
                        (hasLength) ? "," + length : "",
                        (hasFormat) ? ":" + format : "",
                        "}");

                    return string.Format(tokenFormat, tokenValue);
                });
        }

        public static int IndexOf(this string input, int startIndex, Predicate<char> predicate)
        {
            for (int ix = startIndex; ix < input.Length; ++ix)
            {
                if (predicate(input[ix]))
                {
                    return (ix);
                }
            }
            return (input.Length);
        }

        public static int IndexOf(this string input, Predicate<char> predicate)
        {
            return (input.IndexOf(0, predicate));
        }

        public static string StripTags(this string input)
        {
            return StripTagsRegex.Replace(input, string.Empty);
        }

        public static string StripExcessWhitespace(this string input)
        {
            if (input == null)
                return input;

            return ExcessWhitespaceRegex.Replace(input, " ");
        }

        public static string ToBase64EncodedValue(this string input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            var bytes = UTF8Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(bytes);
        }

        public static string FromBase64EncodedValue(this string input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            var bytes = Convert.FromBase64String(input);
            return UTF8Encoding.UTF8.GetString(bytes);
        }

        public static string Mask(this string toMask, int lastDisplayDigits, string mask = MaskString)
        {
            if (lastDisplayDigits < 0)
                throw new ArgumentException("lastDisplayDigits must be a positive number");
            if (string.IsNullOrWhiteSpace(mask))
                throw new ArgumentNullException("mask");

            if (string.IsNullOrEmpty(toMask) || toMask.Length <= lastDisplayDigits)
                return toMask;

            return mask.Repeat(toMask.Length - lastDisplayDigits) + toMask.Substring(toMask.Length - lastDisplayDigits);
        }

        public static string Summarize(this string toSummarize, int length, string terminator = "...")
        {
            if (length <= 0)
                throw new ArgumentException("Length must be greater than zero.");
            if (string.IsNullOrWhiteSpace(terminator))
                throw new ArgumentNullException("terminator");

            if (toSummarize == null)
                return toSummarize;

            var result = toSummarize.Trim();
            var substringLength = length - terminator.Length;
            if (result.Length <= substringLength)
                return result;

            return result.Substring(0, substringLength).Trim() + terminator;
        }

        public static string Repeat(this string toRepeat, int iterations)
        {
            if (string.IsNullOrEmpty(toRepeat))
                throw new ArgumentNullException("toRepeat");
            if (iterations < 1)
                throw new ArgumentException("iterations must be a positive number > 0");

            var builder = new StringBuilder();
            for (int i = 0; i < iterations; i++)
                builder.Append(toRepeat);
            return builder.ToString();
        }

        public static string NullIfWhiteSpace(this string input)
        {
            return string.IsNullOrWhiteSpace(input) ? null : input;
        }

        public static string FirstMarkupId(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;

            var matchCollection = MarkupFirstIdRegex.Matches(input);
            if (matchCollection == null || matchCollection.Count == 0)
                return null;

            return matchCollection[0].Groups[ValueGroupName].Value;
        }

        public static Dictionary<string, string> ToDictionary(this string toProcess,
            char pairDelimeter = '|', char valueDelimeter = ':')
        {
            var result = new Dictionary<string, string>();
            if (string.IsNullOrWhiteSpace(toProcess))
                return result;

            var pairs = toProcess.Split(pairDelimeter);
            foreach (var kvp in pairs)
            {
                if (string.IsNullOrWhiteSpace(kvp))
                    continue;

                var bits = kvp.Split(valueDelimeter);
                if (bits == null || bits.Length != 2 ||
                    string.IsNullOrWhiteSpace(bits[0]) ||
                    string.IsNullOrWhiteSpace(bits[1]))
                    continue;

                result[bits[0].ToLowerInvariant()] = bits[1];
            }

            return result;
        }

        public static string Truncate(this string toTruncate, int? max, string appendString = "")
        {
            if (toTruncate == null || !max.HasValue || toTruncate.Length <= max.Value)
                return toTruncate;

            appendString = appendString ?? "";
            var maxValue = max.Value;
            return toTruncate.Substring(0, (maxValue - appendString.Length)) + appendString;
        }

        public static string BinarySerialize(this object toSerialize)
        {
            if (toSerialize == null)
                throw new ArgumentNullException("toSerialize");

            byte[] bytes;
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, toSerialize);
                stream.Seek(0, SeekOrigin.Begin);
                bytes = stream.ToArray();
            }
            return Convert.ToBase64String(bytes);
        }

        public static object BinaryDeserialize(this string toDeserialize)
        {
            if (string.IsNullOrWhiteSpace(toDeserialize))
                throw new ArgumentNullException("toDeserialize");

            using (var stream = new MemoryStream(Convert.FromBase64String(toDeserialize)))
            {
                var formatter = new BinaryFormatter();
                return formatter.Deserialize(stream);
            }
        }

        ////////////////////////////////
        /// Json serialization commented out because requires System.Web.Script.Serialization.
        ///     If these functions are needed they should be somewhere else... common.Web?
        ////////////////////////////////

        //public static string JsonSerialize(this object toSerialize)
        //{
        //    System.Web.Script.Serialization.JavaScriptSerializer oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        //    return oSerializer.Serialize(toSerialize);
        //}

        //public static object JsonDeserialize(this string toDeserialize, Type targetType)
        //{
        //    var oSerializer = new JavaScriptSerializer();
        //    return oSerializer.Deserialize(toDeserialize, targetType);
        //}
    }
}
