using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using AwApi.Integration;
using AwApi.ViewModels;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.Common.Attributes;

namespace AwApi.EntityMapper
{
    public static class MapperHelper
    {
        public static string GetResourceItemName(Type enumType, string value)
        {
            var memInfo = enumType.GetMember(value);
            var attributes = memInfo[0].GetCustomAttributes(typeof(EnumMetadataAttribute), false);
            var getResourceItemName = ((EnumMetadataAttribute) attributes[0]).ResourceItemName;
            return getResourceItemName;
        }

        public static BusinessMetadata SetResponseProperties(int? flags, DataSource dataSource)
        {
            var businessMetadata = new BusinessMetadata();
            if (flags != null)
            {
                // Delete cache for Countries, States, Currencies, Industries.
                // Re fetch data causing cache of new data.

                // removed usage of code tables for country, subdivision, currency, and industry.
                // we should no longer react to its changes
//                businessMetadata.CodeTablesChanged = (flags & 1) != 0 || (flags & 16) != 0;

                //Delete CacheKeys.AUTHCLAIMS, CacheKeys.AGENTPROFILECLAIMS, CacheKeys.AGENTPROFILEKEY from cache
                businessMetadata.ProfileChanged = (flags & 2) != 0;
            }

            businessMetadata.ServicesEnvironment = GetServicesEnvironment();
            businessMetadata.ApiCodeVersion = AwVersion.Api;
            businessMetadata.AcVersion = AwVersion.Ac;

            var authOperator = AuthIntegration.GetOperator();
            var agent = AuthIntegration.GetAgent();

            businessMetadata.OperatorLanguage = authOperator.Language;
            businessMetadata.AgentId = agent.AgentId;
            businessMetadata.PosNum = agent.AgentSequence;
            businessMetadata.UnitProfileId = agent.UnitProfileId.ToString();
            businessMetadata.DataSource = dataSource;

            return businessMetadata;
        }

        private static string GetServicesEnvironment()
        {
            var keyName = "ServicesEnvironment";
            return ConfigurationManager.AppSettings.AllKeys.Contains(keyName) ? ConfigurationManager.AppSettings[keyName] : string.Empty;
        }

        public static List<string> GetReceiptTextInfo(List<TextTranslationType> textTranslationtInfo)
        {
            var receiptTextInfo = new List<string>();

            if (textTranslationtInfo == null)
                return receiptTextInfo;

            foreach (var element in textTranslationtInfo)
                receiptTextInfo.Add(element.TextTranslation);

            return receiptTextInfo;
        }
    }
}