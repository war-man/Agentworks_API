using MoneyGram.AgentConnect.DomainModel;
using MoneyGram.AgentConnect.EntityMapper;
using MoneyGram.Common;
using MoneyGram.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DOMAIN = MoneyGram.AgentConnect.DomainModel.Transaction;
using SERVICE = MoneyGram.AgentConnect.Service;

namespace MoneyGram.AgentConnect
{
    public static class DomainTransformExtensions
    {
        public static SERVICE.Request CopyAgentData(this SERVICE.Request request, Agent agent)
        {
            if(agent == null)
            {
                return request;
            }

            request.agentID = agent.AgentId;
            request.agentSequence = agent.AgentSequence;
            // agent.OperatorName.Truncate(7); is temporary until  RCDMS matches the AC schema of 20 char.
            request.operatorName = agent.OperatorName.Truncate(7);
            //request.token = agent.AgentPassword;
            request.language = agent.Language.NullIfWhiteSpace();
            //request.apiVersion = ApiVersion;
            //request.clientSoftwareVersion = ClientSoftwareVersion;
            request.timeStamp = DateTime.Now;

            if (agent.UnitProfileId.HasValue)
            {
                request.unitProfileID = agent.UnitProfileId.Value;
                request.unitProfileIDSpecified = true;
            }

            return request;
        }
        public static void ProcessSpecifiedServiceToDomain(object svc, object dm)
        {
            var searchKey = "specified";
            if (svc == null || dm == null)
            {
                return;
            }

            var serviceModelType = svc.GetType();
            var domainModelType = dm.GetType();
            var allServiceModelProperties = serviceModelType.GetProperties();
            var allDomainModelProperties = domainModelType.GetProperties();

            // Looking for specified properties
            foreach (PropertyInfo prop in allServiceModelProperties)
            {
                object propValue = prop.GetValue(svc, null);
                if (propValue == null)
                {
                    continue;
                }
                else if (prop.Name.ToLower().EndsWith(searchKey))
                {
                    var domainMappedPropertyName = prop.Name.Remove(prop.Name.Length - searchKey.Length, searchKey.Length);

                    var flagValue = (bool)propValue;
                    var actualProperty = allServiceModelProperties.First(x => x.Name.Equals(domainMappedPropertyName, StringComparison.InvariantCultureIgnoreCase));
                    var actualPropertyValue = actualProperty.GetValue(svc, null);
                    // Assign domain object value.
                    foreach (var domainProp in allDomainModelProperties)
                    {
                        if(!domainProp.Name.Equals(domainMappedPropertyName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            continue;
                        }
                        if (flagValue)
                        {
                            domainProp.SetValue(dm, actualPropertyValue);
                        }
                        else
                        {
                            domainProp.SetValue(dm, null);
                        }
                    }
                }
            }
        }

        public static void NullifyWhiteSpaceStrings(object obj, IEnumerable<string> propertyExemptions = null)
        {
            if (obj == null)
            {
                return;
            }

            if (propertyExemptions == null)
            {
                propertyExemptions = Enumerable.Empty<string>();
            }
            var type = obj.GetType();
            var allProperties = type.GetProperties();
            foreach (PropertyInfo prop in allProperties)
            {
                object propValue = prop.GetValue(obj, null);
                if (propValue == null)
                {
                    continue;
                }
                if (prop.PropertyType.Assembly == type.Assembly && !prop.PropertyType.IsEnum)
                {
                    NullifyWhiteSpaceStrings(propValue, null);
                }
                else if (prop.PropertyType.FullName == "System.String")
                {
                    if (prop.SetMethod == null)
                    {
                        continue;
                    }
                    var stringValue = (string)propValue;
                    if (!propertyExemptions.Contains(prop.Name) && string.IsNullOrWhiteSpace(stringValue))
                    {
                        prop.SetValue(obj, null);
                    }
                }
            }
        }

        public static List<string> ToSafeArray(this string[] item)
        {
            return item != null ? new List<string>(item) : null;
        }

        public static string[] ToSafeArray(this List<string> item)
        {
            return item != null ? item.ToArray() : null;
        }
        
        public static List<DOMAIN.PromotionInfo> ToPromotionInfos(this SERVICE.PromotionInfo[] items)
        {
            if (items != null)
            {
                var results = new List<DOMAIN.PromotionInfo>();
                foreach (var item in items)
                {
                    results.Add(ServiceToDomainExtensions.ToDomain(item));
                }
                return results;
            }
            return null;
        }

        #region "ToService Request"

        public static SERVICE.PropertyType ToService(this DOMAIN.PropertyType propertyType)
        {
            return new SERVICE.PropertyType
            {
                Name = propertyType.Name,
                Value = propertyType.Value
            };
        }

        public static SERVICE.CustomFieldsType ToService(this DOMAIN.CustomFieldsType messageInfo)
        {
            return new SERVICE.CustomFieldsType
            {
                NameValue = messageInfo.NameValue.ToService()
            };
        }

        public static SERVICE.LegalIdType ToService(this DOMAIN.LegalIdType legalIdType)
        {
            return ParsingUtility.ConvertEnum<DOMAIN.LegalIdType, SERVICE.LegalIdType>(legalIdType);
        }


        #endregion


        //public static List<DOMAIN.Response.BillerInfo> ToBillerInfos(this SERVICE.BillerInfo[] items)
        //{
        //    if (items != null)
        //    {
        //        var results = new List<DOMAIN.Response.BillerInfo>();
        //        foreach (var item in items)
        //        {
        //            results.Add(item.ToDomain());
        //        }
        //        return results;
        //    }
        //    return null;
        //}
        public static DOMAIN.FeeType ToFeeType(this SERVICE.FeeType feeType)
        {
            return ParsingUtility.ConvertEnum<SERVICE.FeeType, DOMAIN.FeeType>(feeType);
        }

        public static DOMAIN.RedirectInfoRedirectType ToRedirectInfoRedirectType(this SERVICE.RedirectInfoRedirectType type)
        {
            return ParsingUtility.ConvertEnum<SERVICE.RedirectInfoRedirectType, DOMAIN.RedirectInfoRedirectType>(type);
        }

        public static List<DOMAIN.StateRegulatorInfo> ToStateRegulatorInfos(this SERVICE.StateRegulatorInfo[] items)
        {
            if (items == null)
            {
                return null;
            }

            var results = new List<DOMAIN.StateRegulatorInfo>();
            foreach (var item in items)
            {
                results.Add(ServiceToDomainExtensions.ToDomain(item));
            }
            return results;
        }

//        public static SERVICE.sendReversalReasonCode ToSendReversalReasonCode(this DOMAIN.SendReversalReasonCode type)
//        {
//            return ParsingUtility.ConvertEnum<DOMAIN.SendReversalReasonCode, SERVICE.sendReversalReasonCode>(type);
//        }

//        public static DOMAIN.SendReversalReasonCode ToSendReversalReasonCode(this SERVICE.sendReversalReasonCode type)
//        {
//            return ParsingUtility.ConvertEnum<SERVICE.sendReversalReasonCode, DOMAIN.SendReversalReasonCode>(type);
//        }

        public static SERVICE.SendReversalType ToSendReversalType(this DOMAIN.SendReversalType type)
        {
            return ParsingUtility.ConvertEnum<DOMAIN.SendReversalType, SERVICE.SendReversalType>(type);
        }

        public static DOMAIN.SendReversalType ToSendReversalType(this SERVICE.SendReversalType type)
        {
            return ParsingUtility.ConvertEnum<SERVICE.SendReversalType, DOMAIN.SendReversalType>(type);
        }

//        public static SERVICE.ReceiveReversalReasonCode ToReceiveReversalReasonCode(this DOMAIN.ReceiveReversalReasonCode type)
//        {
//            return ParsingUtility.ConvertEnum<DOMAIN.ReceiveReversalReasonCode, SERVICE.receiveReversalReasonCode>(type);
//        }

//        public static DOMAIN.ReceiveReversalReasonCode ToReceiveReversalReasonCode(this SERVICE.receiveReversalReasonCode type)
//        {
//            return ParsingUtility.ConvertEnum<SERVICE.receiveReversalReasonCode, DOMAIN.ReceiveReversalReasonCode>(type);
//        }


        public static List<DOMAIN.FeeInfo> ToFeeInfos(this SERVICE.FeeInfo[] items)
        {
            if (items != null)
            {
                var results = new List<DOMAIN.FeeInfo>();
                foreach (var item in items)
                {
                    results.Add(ServiceToDomainExtensions.ToDomain(item));
                }
                return results;
            }
            return null;
        }

        public static SERVICE.ProductVariantType ToProductVariant(this DOMAIN.ProductVariantType type)
        {
            return ParsingUtility.ConvertEnum<DOMAIN.ProductVariantType, SERVICE.ProductVariantType>(type);
        }
        
        public static SERVICE.LegalIdType ToLegalIdType(this DOMAIN.LegalIdType legalIdType)
        {
            return ParsingUtility.ConvertEnum<DOMAIN.LegalIdType, SERVICE.LegalIdType>(legalIdType);
        }

        public static DOMAIN.PhotoIdType ToPhotoIdType(this SERVICE.PhotoIdType photoIdType)
        {
            return ParsingUtility.ConvertEnum<SERVICE.PhotoIdType, DOMAIN.PhotoIdType>(photoIdType);
        }

        public static SERVICE.PhotoIdType ToPhotoIdType(this DOMAIN.PhotoIdType photoIdType)
        {
            return ParsingUtility.ConvertEnum<DOMAIN.PhotoIdType, SERVICE.PhotoIdType>(photoIdType);
        }

        public static SERVICE.ItemChoiceType ToItemChoiceType(this DOMAIN.ItemChoiceType type)
        {
            return ParsingUtility.ConvertEnum<DOMAIN.ItemChoiceType, SERVICE.ItemChoiceType>(type);
        }
        public static List<DOMAIN.IndustryInfo> ToIndustryInfoList(this SERVICE.IndustryInfo[] items)
        {
            if (items == null)
            {
                return null;
            }

            var results = new List<DOMAIN.IndustryInfo>();
            foreach (var item in items)
            {
                results.Add(ServiceToDomainExtensions.ToDomain(item));
            }
            return results;
        }

        public static List<DOMAIN.CountryTranslationType> ToCountryTranslationsList(this SERVICE.CountryTranslationType[] items)
        {
            if (items == null)
            {
                return null;
            }

            var results = new List<DOMAIN.CountryTranslationType>();
            foreach (var item in items)
            {
                results.Add(ServiceToDomainExtensions.ToDomain(item));
            }
            return results;
        }

        public static List<DOMAIN.CountrySubdivisionInfo> ToStateProvinceInfoList(this SERVICE.CountrySubdivisionInfo[] items)
        {
            if (items == null)
            {
                return null;
            }

            var results = new List<DOMAIN.CountrySubdivisionInfo>();
            foreach (var item in items)
            {
                results.Add(ServiceToDomainExtensions.ToDomain(item));
            }
            return results;
        }

        public static List<DOMAIN.DeliveryOptionTranslationType> ToDeliveryOptionTranslationList(this SERVICE.DeliveryOptionTranslationType[] items)
        {
            if (items == null)
            {
                return null;
            }

            var results = new List<DOMAIN.DeliveryOptionTranslationType>();
            foreach (var item in items)
            {
                results.Add(ServiceToDomainExtensions.ToDomain(item));
            }
            return results;
        }

        public static List<DOMAIN.CurrencyTranslationType> ToCurrencyTranslationsList(this SERVICE.CurrencyTranslationType[] items)
        {
            if (items == null)
            {
                return null;
            }

            var results = new List<DOMAIN.CurrencyTranslationType>();
            foreach (var item in items)
            {
                results.Add(ServiceToDomainExtensions.ToDomain(item));
            }
            return results;
        }

        public static List<DOMAIN.FQDOTextTranslationType> ToFqdoTextTranslationList(this SERVICE.FQDOTextTranslationType[] items)
        {
            if (items == null)
            {
                return null;
            }

            var results = new List<DOMAIN.FQDOTextTranslationType>();
            foreach (var item in items)
            {
                results.Add(ServiceToDomainExtensions.ToDomain(item));
            }
            return results;
        }

        public static List<DOMAIN.IndustryTranslationType> ToIndustryTranslationList(this SERVICE.IndustryTranslationType[] items)
        {
            if (items == null)
            {
                return null;
            }

            var results = new List<DOMAIN.IndustryTranslationType>();
            foreach (var item in items)
            {
                results.Add(ServiceToDomainExtensions.ToDomain(item));
            }
            return results;
        }

        public static List<DOMAIN.LoyaltyTranslationType> ToLoyaltyTranslationList(this SERVICE.LoyaltyTranslationType[] items)
        {
            if (items == null)
            {
                return null;
            }
            var results = new List<DOMAIN.LoyaltyTranslationType>();
            foreach (var item in items)
            {
                results.Add(ServiceToDomainExtensions.ToDomain(item));
            }
            return results;
        }

        public static List<DOMAIN.LoyaltyCardTypeTranslationType> ToLoyaltyCardTypeTranslationList(this SERVICE.LoyaltyCardTypeTranslationType[] items)
        {
            if (items == null)
            {
                return null;
            }

            var results = new List<DOMAIN.LoyaltyCardTypeTranslationType>();
            foreach (var item in items)
            {
                results.Add(ServiceToDomainExtensions.ToDomain(item));
            }
            return results;
        }

        public static List<DOMAIN.LoyaltyProgramTypeTranslationType> ToLoyaltyProgramTypeTranslationList(this SERVICE.LoyaltyProgramTypeTranslationType[] items)
        {
            if (items == null)
            {
                return null;
            }

            var results = new List<DOMAIN.LoyaltyProgramTypeTranslationType>();
            foreach (var item in items)
            {
                results.Add(ServiceToDomainExtensions.ToDomain(item));
            }
            return results;
        }

        public static List<DOMAIN.LanguageTranslationType> ToLanguageTranslationList(this SERVICE.LanguageTranslationType[] items)
        {
            if (items == null)
            {
                return null;
            }

            var results = new List<DOMAIN.LanguageTranslationType>();
            foreach (var item in items)
            {
                results.Add(ServiceToDomainExtensions.ToDomain(item));
            }
            return results;
        }

        public static List<DOMAIN.CountryInfo> ToCountryInfoList(this SERVICE.CountryInfo[] items)
        {
            if (items == null)
            {
                return null;
            }

            var results = new List<DOMAIN.CountryInfo>();
            foreach (var item in items)
            {
                results.Add(ServiceToDomainExtensions.ToDomain(item));
            }
            return results;
        }

        public static List<DOMAIN.CurrencyInfo> ToCurrencyInfoList(this SERVICE.CurrencyInfo[] items)
        {
            if (items == null)
            {
                return null;
            }

            var results = new List<DOMAIN.CurrencyInfo>();
            foreach (var item in items)
            {
                results.Add(ServiceToDomainExtensions.ToDomain(item));
            }
            return results;
        }

        public static SERVICE.KeyValuePairType[] ToKeyValuePairs(this List<DOMAIN.KeyValuePairType> items)
        {
            var result = new List<SERVICE.KeyValuePairType>();
            if (items == null)
            {
                return result.ToArray();
            }
            foreach (var item in items)
            {
                var toAdd = item.ToKeyValuePairType();
                if (toAdd == null)
                {
                    continue;
                }
                result.Add(toAdd);
            }
            return result.ToArray();
        }

        public static SERVICE.KeyValuePairType ToKeyValuePairType(this DOMAIN.KeyValuePairType keyValuePairType)
        {
            if (keyValuePairType == null)
            {
                return null;
            }
            var results = new SERVICE.KeyValuePairType
            {
                value = keyValuePairType.Value.NullIfWhiteSpace(),
                infoKey = keyValuePairType.InfoKey.NullIfWhiteSpace()
            };

            return results;
        }

        public static DOMAIN.DataType ToDataTypeCode(this SERVICE.DataType type)
        {
            return ParsingUtility.ConvertEnum<SERVICE.DataType, DOMAIN.DataType>(type);
        }

        public static List<DOMAIN.SenderLookupInfo> ToSenderLookupInfos(this SERVICE.SenderLookupInfo[] items)
        {
            if (items != null)
            {
                var results = new List<DOMAIN.SenderLookupInfo>();
                foreach (var item in items)
                {
                    results.Add(ServiceToDomainExtensions.ToDomain(item));
                }
                return results;
            }
            return null;
        }

        public static List<DOMAIN.BillerLookupInfo> ToBillerLookupInfos(this SERVICE.BillerLookupInfo[] items)
        {
            if (items != null)
            {
                var results = new List<DOMAIN.BillerLookupInfo>();
                foreach (var item in items)
                {
                    results.Add(ServiceToDomainExtensions.ToDomain(item));
                }
                return results;
            }
            return null;
        }

        public static List<DOMAIN.ReceiverLookupInfo> ToReceiverLookupInfos(this SERVICE.ReceiverLookupInfo[] items)
        {
            if (items != null)
            {
                var results = new List<DOMAIN.ReceiverLookupInfo>();
                foreach (var item in items)
                {
                    results.Add(ServiceToDomainExtensions.ToDomain(item));
                }
                return results;
            }
            return null;
        }

        public static DOMAIN.ProductVariantType ToProductVariant(this SERVICE.ProductVariantType type)
        {
            return ParsingUtility.ConvertEnum<SERVICE.ProductVariantType, DOMAIN.ProductVariantType>(type);
        }

        public static List<DOMAIN.CurrencyInfo> ToCurrencyInfos(this SERVICE.CurrencyInfo[] items)
        {
            if (items != null)
            {
                var results = new List<DOMAIN.CurrencyInfo>();
                foreach (var item in items)
                {
                    results.Add(item.ToCurrencyInfo());
                }
                return results;
            }
            return null;
        }

        public static DOMAIN.CurrencyInfo ToCurrencyInfo(this SERVICE.CurrencyInfo item)
        {
            var results = new DOMAIN.CurrencyInfo()
            {
                CurrencyCode = item.currencyCode,
                CurrencyName = item.currencyName,
                CurrencyPrecision = item.currencyPrecision
            };

            return results;
        }

        public static DOMAIN.LegalIdType ToLegalIdType(this SERVICE.LegalIdType legalIdType)
        {
            return ParsingUtility.ConvertEnum<SERVICE.LegalIdType, DOMAIN.LegalIdType>(legalIdType);
        }

        public static DOMAIN.KeyValuePairType[] ToKeyValuePairs(this List<SERVICE.KeyValuePairType> items)
        {
            var result = new List<DOMAIN.KeyValuePairType>();
            if (items == null)
                return result.ToArray();

            foreach (var item in items)
            {
                var toAdd = ServiceToDomainExtensions.ToDomain(item);
                if (toAdd == null)
                {
                    continue;
                }
                result.Add(toAdd);
            }
            return result.ToArray();
        }

        public static List<DOMAIN.HierarchyLevelInfo> ToHierarchyLevelInfos(this SERVICE.HierarchyLevelInfo[] items)
        {
            if (items != null)
            {
                var results = new List<DOMAIN.HierarchyLevelInfo>();
                foreach (var item in items)
                {
                    results.Add(ServiceToDomainExtensions.ToDomain(item));
                }
                return results;
            }
            return null;
        }

        public static List<DOMAIN.AttributeType> ToAttributes(this SERVICE.AttributeType[] items)
        {
            if (items != null)
            {
                var results = new List<DOMAIN.AttributeType>();
                foreach (var item in items)
                {
                    results.Add(ServiceToDomainExtensions.ToDomain(item));
                }
                return results;
            }
            return null;
        }

        #region Helper Methods
        public static void MapPromotionInfo(List<DOMAIN.PromotionInfo> target, SERVICE.PromotionInfo[] source)
        {
            if (source != null)
            {
                foreach (var promotionInfo in source)
                {
                    target.Add(ServiceToDomainExtensions.ToDomain(promotionInfo));
                }
            }
        }
        #endregion Helper Methods
    }
}