using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace MoneyGram.Common.Extensions
{
    public static class XmlExtensions
    {
        public static XmlDocument CreateDocument(string content)
        {
            var result = new XmlDocument();
            if (!string.IsNullOrEmpty(content))
                result.LoadXml(content);
            return result;
        }

        public static XmlDocument CreateNewDocument(string outerElementName)
        {
            var result = new XmlDocument();
            var outerElement = result.CreateElement(outerElementName);
            result.AppendChild(outerElement);
            return result;
        }

        public static XmlNode AddChildElement(this XmlDocument document, XmlNode parentNode, string elementName, string elementValue = null)
        {
            var result = document.CreateElement(XmlConvert.EncodeLocalName(elementName));
            if (!string.IsNullOrEmpty(elementValue))
                result.InnerText = elementValue;
            parentNode.AppendChild(result);
            return result;
        }

        public static string GetNodeValue(this XmlNode node, string path)
        {
            if (node == null)
                throw new ArgumentNullException("node");
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            var contentNode = node.SelectSingleNode(path);
            return (contentNode == null) ? string.Empty : contentNode.InnerText;
        }

        public static string GetAttributeValue(this XmlNode node, string attributeName, string defaultValue = "")
        {
            if (node == null)
                throw new ArgumentNullException("node");
            if (string.IsNullOrEmpty(attributeName))
                throw new ArgumentNullException("attributeName");

            var attribute = node.Attributes[attributeName];
            if (attribute == null)
                return defaultValue;

            return attribute.Value;
        }

        public static String Serialize(this object toSerialize)
        {
            Type t = toSerialize.GetType();
            if (t.IsSerializable)
            {
                var serializer = new XmlSerializer(t);
                using (StringWriter writer = new StringWriter())
                {
                    serializer.Serialize(writer, toSerialize);
                    return writer.ToString();
                }
            }
            else
            {
                throw new InvalidOperationException(string.Format("Objects of type {0} are not serializable.", t.Name));
            }
        }

        public static T ConvertTo<T>(this string data)
        {
            XmlSerializer s = new XmlSerializer(typeof(T));
            using (StringReader reader = new StringReader(data))
            {
                object obj = s.Deserialize(reader);
                return (T)obj;
            }
        }
    }
}
