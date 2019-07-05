using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Configuration;


namespace MoneyGram.Common.Extensions
{
    public static class ConfigurationExtensions
    {
        public static string GetValue(this NameValueCollection collection,
            string key, string defaultValue)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            return collection[key] ?? defaultValue;
        }

        public static TValue GetValueAs<TValue>(this NameValueCollection collection,
            string key, TValue defaultValue = default(TValue))
            where TValue : struct
        {
            return collection.GetNullableValueAs<TValue>(key).GetValueOrDefault(defaultValue);
        }

        public static TValue? GetNullableValueAs<TValue>(this NameValueCollection collection, string key)
            where TValue : struct
        {
            var resultStr = collection.GetValue(key, null);
            if (resultStr == null)
                return default(TValue?);

            return ParsingUtility.Parse<TValue>(resultStr);
        }

        public static string GetAppSetting(this NameValueCollection collection,
            string key, string defaultValue = null)
        {
            var result = collection.GetValue(key, defaultValue);
            if (result != null)
                return result;
                
            throw new Exception(
                    string.Format("AppSetting '{0}' is referenced but not configured and has no default.",
                    key));
        }

        public static TValue GetAppSettingAs<TValue>(this NameValueCollection collection,
            string key, TValue? defaultValue = null)
            where TValue : struct
        {
            var result = collection.GetNullableValueAs<TValue>(key) ?? defaultValue;
            if (result.HasValue)
                return result.Value;

            throw new Exception(
                string.Format("AppSetting '{0}' is referenced but not configured or is not parseable into the correct type ({1}).",
                key, typeof(TValue).Name));
        }
    }
}
