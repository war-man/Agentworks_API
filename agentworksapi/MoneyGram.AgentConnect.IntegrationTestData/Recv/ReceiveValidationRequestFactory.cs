using System.Collections.Generic;
using System.Linq;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Recv
{
    public static class ReceiveValidationRequestFactory
    {
        public static ReceiveValidationRequest NewRequestWithBaseData()
        {
            var recvValidationRequest = PopulateBaseData();

            return recvValidationRequest;
        }
        public static ReceiveValidationRequest PopulateBaseData()
        {
            return new ReceiveValidationRequest
            {
                MgiSessionID = string.Empty,
                ReceiveCurrency = string.Empty,
                ReceiveAmount = 0.0m,
                FieldValues = new List<KeyValuePairType>(),
                PrimaryReceiptLanguage = Language.Eng,
                SecondaryReceiptLanguage = Language.Spa,
                TransactionStaging = false
            };
        }
        private static KeyValuePairType GetFieldValueItem(string infoKey, string val)
        {
            return new KeyValuePairType() { InfoKey = infoKey, Value = val };
        }
        public static List<KeyValuePairType> GetRequestedFieldValuesFromErrors(IEnumerable<BusinessError> checkVerifiedErrors, TransactionLookupResponsePayload transactionLookupResponsePayload, Dictionary<string, string> infoKeysWithGroupTypes, string thirdPartyType)
        {
            List<KeyValuePairType> fldValues = new List<KeyValuePairType>();
            List<string> reqFlds = checkVerifiedErrors.Where(x => x.ErrorCategory.ToUpper() == BusinessErrors.CheckVerify || x.ErrorCategory.ToUpper() == BusinessErrors.Validation).Select(x => x.OffendingField).ToList();
            PopulateFields(transactionLookupResponsePayload, infoKeysWithGroupTypes, thirdPartyType, fldValues, reqFlds);
            return fldValues;
        }
        public static List<KeyValuePairType> GetFieldValues(IEnumerable<FieldToCollectInfo> categoryFieldsToCollect, TransactionLookupResponsePayload transactionLookupResponsePayload, Dictionary<string, string> infoKeysWithGroupTypes, string thirdPartyType)
        {
            List<KeyValuePairType> fldValues = new List<KeyValuePairType>();
            #region PopulateEnumFieldValues
            //Get keyvalues for enum fields
            foreach (var enumItem in categoryFieldsToCollect.Where(x => x.DataType == DataType.@enum).ToList())
            {
                var enumeration = enumItem.Enumeration;
                var enumeratedItems = enumeration.EnumeratedItems.Select(x => x.Identifier).ToList();
                var fldValue = GetFieldValueItem(enumItem.InfoKey, DataGenerator.GetRandomItemFromList(enumeratedItems));
                fldValues.Add(fldValue);
                if (enumItem.ChildFields != null)
                {
                    var matchingChildField = enumItem.ChildFields.FirstOrDefault(x => x.FieldValue == fldValue.Value);
                    var requiredChildFieldInfos = matchingChildField?.Infos.OfType<FieldToCollectInfo>().Where(x => x.Required == true).ToList() ?? new List<FieldToCollectInfo>();
                    if (requiredChildFieldInfos?.Count() > 0)
                    {
                        PopulateFields(transactionLookupResponsePayload, infoKeysWithGroupTypes, thirdPartyType, fldValues, requiredChildFieldInfos.Select(x => x.InfoKey).ToList());
                    }
                }
            }
            #endregion

            #region PopulateFieldValues
            if (!categoryFieldsToCollect.Any())
            {
                return fldValues;
            }
            List<string> reqFld = categoryFieldsToCollect.Where(x => (x.DataType != DataType.@enum)).Select(x => x.InfoKey).ToList();
            PopulateFields(transactionLookupResponsePayload, infoKeysWithGroupTypes, thirdPartyType, fldValues, reqFld);
            #endregion

            return fldValues;
        }

        private static void PopulateFields(TransactionLookupResponsePayload transactionLookupResponsePayload, Dictionary<string, string> infoKeysWithGroupTypes, string thirdPartyType, List<KeyValuePairType> fldValues, List<string> reqFld)
        {
            foreach (var key in reqFld)
            {
                KeyValuePairType newFldVal = FromTransactionLookupResponsePayload(transactionLookupResponsePayload, key);
                if (newFldVal == null)
                {
                    if (key == InfoKeyNames.thirdParty_Sender_Type) //TODO: Verify whether ThirdPartyType.NONE/null works.
                    {
                        newFldVal = GetFieldValueItem(key, thirdPartyType ?? TestThirdPartyType.None);
                    }
                    else if (infoKeysWithGroupTypes.Keys.Contains(key) && (infoKeysWithGroupTypes[key] == InfoKeyGroups.PersonalIdNumber))
                    {
                        var correspondingIdTypeInfoKey = key.Substring(0, key.LastIndexOf("_") + 1) + "Type";
                        var idType = fldValues.FirstOrDefault(x => x.InfoKey == correspondingIdTypeInfoKey).Value;
                        newFldVal = GetFieldValueItem(key, DataGenerator.PersonalIdNumber(idType));
                    }
                    else
                    {
                        newFldVal = GetFieldValueItem(key, infoKeysWithGroupTypes.Keys.Contains(key) ? DataGenerator.GetRandomData(infoKeysWithGroupTypes[key]) : string.Empty);
                    }
                }
                fldValues.Add(newFldVal);
            }
        }
        private static KeyValuePairType FromTransactionLookupResponsePayload(TransactionLookupResponsePayload transactionLookupResponsePayload, string reqInfoKey)
        {
            KeyValuePairType currVal = transactionLookupResponsePayload.CurrentValues.Exists(x => x.InfoKey == reqInfoKey) ?
                        transactionLookupResponsePayload.CurrentValues.First(x => x.InfoKey == reqInfoKey) : null;
            return currVal;
        }
        public static ReceiveValidationRequest FromTransactionLookup(this ReceiveValidationRequest recvValReq, TransactionLookupResponsePayload transactionResponsePayload)
        {
            var sessionId = transactionResponsePayload.MgiSessionID;
            var currency = transactionResponsePayload.ReceiveAmounts;
            recvValReq.ReceiveAmount = currency.ReceiveAmount.Value;
            recvValReq.ReceiveCurrency = currency.ReceiveCurrency;
            recvValReq.MgiSessionID = sessionId;
            return recvValReq;
        }
        private static ReceiveValidationRequest MapFieldValues(ReceiveValidationRequest recvValReq, List<KeyValuePairType> populatedFields, List<string> requestedInfoKeys, ReceiveValidationRequest previousRequest, List<string> monitoredListOfFieldsToCollect)
        {
            if (populatedFields == null)
            {
                return recvValReq;
            }
            var availableKeys = populatedFields.Select(x => x.InfoKey).ToList();
            var keysToMap = availableKeys.Union(requestedInfoKeys);
            foreach (var key in keysToMap)
            {
                if (availableKeys.Contains(key))
                {
                    var pairToAdd = populatedFields.Where(k => k.InfoKey == key).FirstOrDefault();
                    if (!recvValReq.FieldValues.Any(x => x.InfoKey == pairToAdd.InfoKey))
                    {
                        if (!string.IsNullOrWhiteSpace(pairToAdd.Value))
                        {
                            recvValReq.FieldValues.Add(pairToAdd);
                            if (monitoredListOfFieldsToCollect != null && monitoredListOfFieldsToCollect.Contains(pairToAdd.InfoKey))
                            {
                                monitoredListOfFieldsToCollect.Remove(pairToAdd.InfoKey);
                            }
                        }
                    }
                }
            }
            if (previousRequest != null && previousRequest.FieldValues.Any())
            {
                foreach (var keyPair in previousRequest.FieldValues)
                {
                    if (!recvValReq.FieldValues.Any(x => x.InfoKey == keyPair.InfoKey))
                    {
                        recvValReq.FieldValues.Add(keyPair);
                        if (monitoredListOfFieldsToCollect != null && monitoredListOfFieldsToCollect.Contains(keyPair.InfoKey))
                        {
                            monitoredListOfFieldsToCollect.Remove(keyPair.InfoKey);
                        }
                    }
                }
            }
            return recvValReq;
        }
    }
}