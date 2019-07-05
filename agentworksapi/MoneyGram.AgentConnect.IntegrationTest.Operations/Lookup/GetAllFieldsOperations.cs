using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Lookup;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;

namespace MoneyGram.AgentConnect.IntegrationTest.Operations.Lookup
{
    public class GetAllFieldsOperations
    {
        private TestAgentConnectIntegration _acIntegration { get; }
        private string tabDelimiter = "\t";

        public GetAllFieldsOperations(TestRunner testRunner)
        {
            _acIntegration = new TestAgentConnectIntegration(testRunner);
        }

        public GetAllFieldsData GetAllFields(string agentId, string agentPos, string transactionType, string cachedVersion)
        {
            var getAllFieldsData = new GetAllFieldsData();

            getAllFieldsData.GetAllFieldsReq = GetAllFieldsRequestFactory.GetAllFieldsRequest(transactionType, cachedVersion);
            getAllFieldsData.GetAllFieldsReq.AgentID = agentId;
            getAllFieldsData.GetAllFieldsReq.AgentSequence = agentPos;
            getAllFieldsData.GetAllFieldsResp = _acIntegration.GetAllFields(getAllFieldsData.GetAllFieldsReq);
            getAllFieldsData.InfoKeys = new List<string>();

            if (getAllFieldsData.GetAllFieldsResp.Payload != null)
            {
                getAllFieldsData.InfoKeys.AddRange(ExtractInfoKeys(getAllFieldsData.GetAllFieldsResp.Payload.Infos, transactionType));
            }

            return getAllFieldsData;
        }
        public Dictionary<string, string> GetInfoKeysWithGroupTypes(string agentId, string agentPos, Dictionary<string, string> gafInfoKeysWithGroups, string transactionType)
        {
            try
            {
                // We only want to fetch if we have no data, prevent multiple calls from going out.
                if (gafInfoKeysWithGroups.Count == 0)
                {
                    var gafSendData = GetAllFields(agentId, agentPos, transactionType, string.Empty);
                    gafInfoKeysWithGroups = GetInfoKeyWithGroupTypes(gafSendData.InfoKeys);
                }
            }
            catch //AC call failed, so populate with setup data
            {
                gafInfoKeysWithGroups = InfoKeysWithGroupType.GetInfoKeyGroupTypeDictionary;
            }
            return gafInfoKeysWithGroups;
        }
        private List<string> ExtractInfoKeys(List<InfoBase> infos, string transactionType)
        {
            List<string> infoKeys = new List<string>();

            var categoryInfoKeys = ExtractCategoryInfoKeys(infos.OfType<CategoryInfo>());
            var fieldInfoKeys = ExtractFieldInfoKeys(infos.OfType<FieldToCollectInfo>());

            infoKeys.AddRange(categoryInfoKeys.Select(x => $"{transactionType}{tabDelimiter}{x}"));
            infoKeys.AddRange(fieldInfoKeys.Select(x => $"{transactionType}{tabDelimiter}{x}"));

            return infoKeys;
        }

        private List<string> ExtractFieldInfoKeys(IEnumerable<FieldToCollectInfo> fieldToCollectInfos)
        {
            List<string> infoKeys = new List<string>();

            if (fieldToCollectInfos != null)
            {
                infoKeys.AddRange(fieldToCollectInfos.Select(x => $"Field{tabDelimiter}{x.InfoKey}{tabDelimiter}{x.DataType}{tabDelimiter}{x.Enumeration?.Name}"));

                var fieldsToCollectWithChildFields = fieldToCollectInfos.Where(x => x.ChildFields != null);
                foreach (var item in fieldsToCollectWithChildFields)
                {
                    foreach (var childField in item.ChildFields)
                    {
                        infoKeys.AddRange(ExtractCategoryInfoKeys(childField.Infos.OfType<CategoryInfo>()));
                        infoKeys.AddRange(ExtractFieldInfoKeys(childField.Infos.OfType<FieldToCollectInfo>()));
                    }
                }
            }

            return infoKeys;
        }

        private List<string> ExtractCategoryInfoKeys(IEnumerable<CategoryInfo> categoryInfos)
        {
            List<string> infoKeys = new List<string>();

            if (categoryInfos != null)
            {
                infoKeys.AddRange(categoryInfos.Select(x => $"Category {tabDelimiter}{x.InfoKey}"));
                foreach (var categoryItem in categoryInfos)
                {
                    infoKeys.AddRange(ExtractCategoryInfoKeys(categoryItem.Infos.OfType<CategoryInfo>()));
                    infoKeys.AddRange(ExtractFieldInfoKeys(categoryItem.Infos.OfType<FieldToCollectInfo>()));
                }
            }

            return infoKeys;
        }

        public Dictionary<string, string> GetInfoKeyWithGroupTypes(List<InfoBase> infos, string transactionType)
        {
            return GetInfoKeyWithGroupTypes(ExtractInfoKeys(infos, transactionType));
        }

        public Dictionary<string, string> GetInfoKeyWithGroupTypes(IEnumerable<string> gafInfoKeys)
        {
            var keywordList = InfoKeyGroups.AllGroups;

            Dictionary<string, string> infoKeyGroupDict = new Dictionary<string, string>();
            //Ignore the 'Category' fields.
            var gafFieldInfoOnly = gafInfoKeys.Where(x => !x.Split('\t')[1].Contains("Category"));

            //Poplate the dictionary with <InfoKey> & the corresponding group (eg. FirstName, LastName, enumName).
            foreach (var item in gafFieldInfoOnly)
            {
                var parsedItem = item.Split('\t');
                var infoKeyName = parsedItem[2];
                var beginningExpression = "(?i)";
                var endOfLineExpression = "(?-i)$";
                var matchingKeyword = keywordList.FirstOrDefault(x => Regex.IsMatch(infoKeyName, $"{beginningExpression}{x}{endOfLineExpression}")) ??
                                        (item.Contains("\tenum\t") ? $"enum_{parsedItem[4]}" : "GroupNotIdentified");

                if (!infoKeyGroupDict.Keys.Contains(infoKeyName))
                {
                    infoKeyGroupDict.Add(infoKeyName, matchingKeyword);
                }
            }

            return infoKeyGroupDict;
        }

    }
}