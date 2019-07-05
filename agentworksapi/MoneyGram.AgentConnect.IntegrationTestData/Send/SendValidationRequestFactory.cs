using System.Collections.Generic;
using System.Linq;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Send
{
    public static class SendValidationRequestFactory
    {
        public static SendValidationRequest NewRequestWithBaseData()
        {
            var sendValidationReq = PopulateBaseData();

            return sendValidationReq;
        }
        public static SendValidationRequest NewRequestWithRequiredFieldValues(SenderLookupInfo senderInfo, FeeInfo feeInfo, Dictionary<string, string> infoKeysWithGroupTypes, List<InfoBase> fieldsToCollect, string thirdPartyType)
        {
            var newReq = NewRequestWithBaseData();

            var reqFldValues = GetFieldValues(fieldsToCollect.OfType<FieldToCollectInfo>(), senderInfo, infoKeysWithGroupTypes, thirdPartyType);
            newReq.FieldValues.AddRange(reqFldValues);

            if (newReq.FieldValues.Exists(x => x.InfoKey == InfoKeyNames.sender_Occupation))
            {
                var senderOccupationFld = newReq.FieldValues.First(x => x.InfoKey == InfoKeyNames.sender_Occupation);
                if ((senderOccupationFld.Value.ToLower() != "other") && (newReq.FieldValues.Exists(x => x.InfoKey == InfoKeyNames.sender_OccupationOther)))
                {
                    var senderOtherOccupationFld = newReq.FieldValues.First(x => x.InfoKey == InfoKeyNames.sender_OccupationOther);
                    newReq.FieldValues.Remove(senderOtherOccupationFld);
                }
            }
            return newReq;
        }

        private static SendValidationRequest PopulateBaseData()
        {
            return new SendValidationRequest
            {
                GAFVersionNumber = string.Empty,
                TransactionStaging = false,
                FieldValues = new List<KeyValuePairType>(),
                VerifiedFields = new List<string>(),
                PrimaryReceiptLanguage = Language.Eng,
                SecondaryReceiptLanguage = Language.Spa,
                ReceiptImages = new List<ReceiptImageContentType>()
            };
        }

        private static KeyValuePairType GetFieldValueItem(string infoKey, string val)
        {
            return new KeyValuePairType() { InfoKey = infoKey, Value = val };
        }
        public static List<KeyValuePairType> GetFieldValues(IEnumerable<FieldToCollectInfo> categoryFieldsToCollect, SenderLookupInfo senderInfo, Dictionary<string, string> infoKeysWithGroupTypes, string thirdPartyType)
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
                        PopulateRequiredFields(senderInfo, infoKeysWithGroupTypes, thirdPartyType, fldValues, requiredChildFieldInfos.Select(x => x.InfoKey).ToList());
                    }
                }
            }
            #endregion

            #region PopulateFieldValues
            if(!categoryFieldsToCollect.Any())
            {
                return fldValues;
            }
            List<string> reqFld = categoryFieldsToCollect.Where(x => (x.DataType != DataType.@enum)).Select(x=>x.InfoKey).ToList();
            PopulateRequiredFields(senderInfo, infoKeysWithGroupTypes, thirdPartyType, fldValues, reqFld);
            #endregion

            return fldValues;
        }

        private static void PopulateRequiredFields(SenderLookupInfo senderInfo, Dictionary<string, string> infoKeysWithGroupTypes, string thirdPartyType, List<KeyValuePairType> fldValues, List<string> reqFld)
        {
            foreach (var key in reqFld)
            {
                KeyValuePairType newFldVal = FromSenderInfo(senderInfo, key);
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

        private static KeyValuePairType FromSenderInfo(SenderLookupInfo senderInfo, string reqInfoKey)
        {
            KeyValuePairType currVal = senderInfo.CurrentValues.Exists(x => x.InfoKey == reqInfoKey) ?
                        senderInfo.CurrentValues.First(x => x.InfoKey == reqInfoKey) : null;
            return currVal;
        }

        public static SendValidationRequest FromFeeInfo(this SendValidationRequest sendValReq, FeeInfo feeInfo)
        {
            sendValReq.MgiSessionID = feeInfo.MgiSessionID;
            sendValReq.SendAmount = feeInfo.SendAmounts.SendAmount.GetValueOrDefault();
            sendValReq.SendCurrency = feeInfo.SendAmounts.SendCurrency;
            sendValReq.PromoCodes = feeInfo.PromotionInfos?.Select(x => x.PromotionCode).ToList();
            sendValReq.DestinationCountry = feeInfo.DestinationCountry;
            sendValReq.ServiceOption = feeInfo.ServiceOption;
            sendValReq.ReceiveCurrency = feeInfo.ReceiveAmounts.ReceiveCurrency;
            if(!string.IsNullOrWhiteSpace(feeInfo.ReceiveAgentID))
            {
                sendValReq.ReceiveAgentID = feeInfo.ReceiveAgentID;
            }
            return sendValReq;
        }
        public static SendValidationRequest FromFeeLookupRequest(this SendValidationRequest sendValReq, FeeLookupRequest feeLookupRequest, List<string> requestedInfoKeys)
        {
            if(!requestedInfoKeys.Any() || !requestedInfoKeys.Contains("destination_Country_SubdivisionCode"))
            {
                return sendValReq;
            }
            // Check if it exists, dont want to keep re-adding on each subsequent request.
            if (sendValReq.FieldValues.Any(x => x.InfoKey == "destination_Country_SubdivisionCode"))
            {
                return sendValReq;
            }
            if(string.IsNullOrWhiteSpace(feeLookupRequest.DestinationCountrySubdivisionCode))
            {
                return sendValReq;
            }
            // Explicitly added
            sendValReq.FieldValues.Add(new KeyValuePairType { InfoKey = "destination_Country_SubdivisionCode", Value = feeLookupRequest.DestinationCountrySubdivisionCode });
            return sendValReq;
        }
        public static SendValidationRequest FromSender(this SendValidationRequest sendValReq, List<KeyValuePairType> senderFields, List<string> requestedInfoKeys, SendValidationRequest previousRequest = null, List<string> monitoredListOfFieldsToCollect = null)
        {
            return MapFieldValues(sendValReq, senderFields, requestedInfoKeys, previousRequest, monitoredListOfFieldsToCollect);
        }
        public static SendValidationRequest FromReceiver(this SendValidationRequest sendValReq, List<KeyValuePairType> receiverFields, List<string> requestedInfoKeys, SendValidationRequest previousRequest = null, List<string> monitoredListOfFieldsToCollect = null)
        {
            return MapFieldValues(sendValReq, receiverFields, requestedInfoKeys, previousRequest, monitoredListOfFieldsToCollect);
        }
        private static SendValidationRequest MapFieldValues(SendValidationRequest sendValReq, List<KeyValuePairType> populatedFields, List<string> requestedInfoKeys, SendValidationRequest previousRequest, List<string> monitoredListOfFieldsToCollect)
        {
            if (populatedFields == null)
            {
                return sendValReq;
            }
            // Dont map anything if there is nothing requested.
            if(!requestedInfoKeys.Any())
            {
                return sendValReq;
            }
            var availableKeys = populatedFields.Select(x => x.InfoKey).ToList();
            var keysToMap = availableKeys.Union(requestedInfoKeys);
            foreach (var key in keysToMap)
            {
                if (availableKeys.Contains(key))
                {
                    var pairToAdd = populatedFields.Where(k => k.InfoKey == key).FirstOrDefault();
                    if (!sendValReq.FieldValues.Any(x => x.InfoKey == pairToAdd.InfoKey))
                    {
                        if (!string.IsNullOrWhiteSpace(pairToAdd.Value))
                        {
                            sendValReq.FieldValues.Add(pairToAdd);
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
                    if (!sendValReq.FieldValues.Any(x => x.InfoKey == keyPair.InfoKey))
                    {
                        sendValReq.FieldValues.Add(keyPair);
                        if (monitoredListOfFieldsToCollect != null && monitoredListOfFieldsToCollect.Contains(keyPair.InfoKey))
                        {
                            monitoredListOfFieldsToCollect.Remove(keyPair.InfoKey);
                        }
                    }
                }
            }
            return sendValReq;
        }

        public static SendValidationRequest FromMockData(this SendValidationRequest sendValReq)
        {
            sendValReq.FieldValues = sendValReq.FieldValues ?? new List<KeyValuePairType>();
            sendValReq.FieldValues.Clear();

            //add all required fields
            sendValReq.FieldValues.Add(InfoKeyWithValues.GetFieldValue(InfoKeyNames.sender_LastName));
            sendValReq.FieldValues.Add(InfoKeyWithValues.GetFieldValue(InfoKeyNames.sender_Address));
            sendValReq.FieldValues.Add(InfoKeyWithValues.GetFieldValue(InfoKeyNames.sender_Birth_Country));
            sendValReq.FieldValues.Add(InfoKeyWithValues.GetFieldValue(InfoKeyNames.sender_City));
            sendValReq.FieldValues.Add(InfoKeyWithValues.GetFieldValue(InfoKeyNames.sender_DOB));
            sendValReq.FieldValues.Add(InfoKeyWithValues.GetFieldValue(InfoKeyNames.sender_Country));
            sendValReq.FieldValues.Add(InfoKeyWithValues.GetFieldValue(InfoKeyNames.sender_Occupation));
            sendValReq.FieldValues.Add(InfoKeyWithValues.GetFieldValue(InfoKeyNames.receiver_LastName));
            sendValReq.FieldValues.Add(InfoKeyWithValues.GetFieldValue(InfoKeyNames.primaryReceiptLanguage));
            sendValReq.FieldValues.Add(InfoKeyWithValues.GetFieldValue(InfoKeyNames.sender_PrimaryPhoneCountryCode));
            sendValReq.FieldValues.Add(InfoKeyWithValues.GetFieldValue(InfoKeyNames.sender_PrimaryPhone));
            sendValReq.FieldValues.Add(InfoKeyWithValues.GetFieldValue(InfoKeyNames.receiver_PrimaryPhoneCountryCode));
            sendValReq.FieldValues.Add(InfoKeyWithValues.GetFieldValue(InfoKeyNames.receiver_PrimaryPhone));
            sendValReq.FieldValues.Add(InfoKeyWithValues.GetFieldValue(InfoKeyNames.sender_FirstName));
            sendValReq.FieldValues.Add(InfoKeyWithValues.GetFieldValue(InfoKeyNames.receiver_FirstName));

            return sendValReq;
        }
    }
}