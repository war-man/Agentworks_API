using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Xml;

namespace MoneyGram.Common.Extensions
{
    public static class IDictionaryExtensions
    {
        public static TValue GetObjectAs<TValue>(this IDictionary<string, object> dictionary, string key, TValue defaultValue = null)
            where TValue : class
        {
            var result = dictionary[key] as TValue;
            return (result == null) ? defaultValue : result;
        }

        /// NOT REFERENCED.

        //public static TValue GetValueAs<TValue>(this IDictionary<string, object> dictionary,
        //    string key, TValue defaultValue = default(TValue))
        //    where TValue : struct
        //{
        //    return dictionary.GetNullableValueAs<TValue>(key).GetValueOrDefault(defaultValue);
        //}

        //public static TValue? GetNullableValueAs<TValue>(this IDictionary<string, object> dictionary, string key)
        //    where TValue : struct
        //{
        //    var defaultValue = default(TValue?);
        //    var obj = dictionary[key];
        //    if (obj == null)
        //        return defaultValue;

        //    try
        //    {
        //        return (TValue)obj;
        //    }
        //    catch { }

        //    return defaultValue;
        //}

        public static IDictionary<string, object> GetFilteredValues(this IDictionary<string, object> dictionary, params string[] filteredValues)
        {
            var result = new Dictionary<string, object>();
            foreach (var entry in dictionary.Where<KeyValuePair<string, object>>(kvp => !filteredValues.Contains<string>(kvp.Key)))
            {
                result.Add(entry.Key, entry.Value);
            }
            return result;
        }

        public static TResult RetrieveInitialized<TKey, TResult>(this IDictionary dictionary, TKey key)
            where TResult : class, new()
        {
            var result = dictionary[key] as TResult;
            if (result == null)
            {
                result = new TResult();
                dictionary[key] = result;
            }
            return result;
        }

        public static TValue GetSavedValue<TValue>(this IDictionary dictionary, string key, Func<TValue> loadData)
            where TValue : struct
        {
            if (dictionary.Contains(key))
            {
                return (TValue)dictionary[key];
            }
            else
            {
                TValue loaded = loadData();
                dictionary[key] = loaded;
                return loaded;
            }
        }

        public static TObject GetSavedObject<TObject>(this IDictionary dictionary, string key, Func<TObject> loadData)
            where TObject : class
        {
            if (dictionary.Contains(key))
            {
                return dictionary[key] as TObject;
            }
            else
            {
                TObject loaded = loadData();
                dictionary[key] = loaded;
                return loaded;
            }
        }

        public static Dictionary<TKey, TValue> Extend<TKey, TValue>(
            this Dictionary<TKey, TValue> dictionary, Dictionary<TKey, TValue> additionalValues, bool overwrite = false)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            var result = new Dictionary<TKey, TValue>();
            foreach (var kvp in dictionary)
            {
                result[kvp.Key] = kvp.Value;
            }
            foreach (var kvp in additionalValues ?? new Dictionary<TKey, TValue>())
            {
                if (result.ContainsKey(kvp.Key) && !overwrite)
                    continue;

                result[kvp.Key] = kvp.Value;
            }
            return result;
        }

        /// NOT REFERENCED.
        //public static XmlDocument ToXmlDocument(this IDictionary dictionary, string outerNodeName = null)
        //{
        //    if (dictionary == null)
        //        throw new ArgumentNullException("dictionary");

        //    var result = XmlExtensions.CreateNewDocument(outerNodeName ?? "Element");

        //    dictionary.AddChildElements(result, result.FirstChild);

        //    return result;
        //}

        public static XmlNode AddChildElements(this IDictionary dictionary, XmlDocument document, XmlNode nodeToAppendTo)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");
            if (document == null)
                throw new ArgumentNullException("document");
            if (nodeToAppendTo == null)
                throw new ArgumentNullException("nodeToAppendTo");

            foreach (var key in dictionary.Keys)
            {
                if (key == null)
                    continue;
                var value = dictionary[key];
                if (value == null)
                    continue;

                var valueDictionary = value as IDictionary;
                if (valueDictionary == null)
                {
                    document.AddChildElement(nodeToAppendTo, key.ToString(), value.ToString());
                }
                else
                {
                    var newElement = document.AddChildElement(nodeToAppendTo, key.ToString());
                    var newChildElements = ((IDictionary)value).AddChildElements(document, newElement);
                }
            }

            return nodeToAppendTo;
        }

        public static bool TryGetValue<TResult>(
           this Dictionary<string, TResult> dictionary, string key, out TResult result, StringComparison keyComparison)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            foreach (var kvp in dictionary)
            {
                if (kvp.Key.Equals(key, keyComparison))
                {
                    result = kvp.Value;
                    return true;
                }
            }

            result = default(TResult);
            return false;
        }

        /// NOT REFERENCED.
        //public static IDictionary<string, object> ConvertToDictionary(this object toConvert)
        //{
        //    return ObjectToDictionaryRegistry.Convert(toConvert);
        //}

        public static void AddHasToken(this Dictionary<string, object> tokens, string dependentTokenName, string tokenName = null)
        {
            tokenName = tokenName ?? "has" + dependentTokenName;
            object value;
            tokens.TryGetValue(dependentTokenName, out value);
            tokens[tokenName] = (value == null || string.IsNullOrWhiteSpace(value.ToString())) ? string.Empty : "1";
        }
    }
}
