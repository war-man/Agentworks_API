using System.Collections.Generic;
using System.Linq;
using MoneyGram.AgentConnect.DomainModel.Transaction;
using MoneyGram.AgentConnect.IntegrationTest.Data.Setup;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.BillPay
{
    public static class BillPayValidationRequestFactory
    {
        public static BPValidationRequest InitialValidationForEp(FeeInfo feeInfo, BillerInfo billerInfo)
        {
            var validationRequest = CreateBaseValidationRequest()
                .FromFeeInfo(feeInfo)
                .FromBillerInfo(billerInfo);

            return validationRequest;
        }

        public static BPValidationRequest SubsequentValidationForEp(FeeInfo feeInfo, BillerInfo billerInfo, BPValidationResponse validationResponse, TestBiller biller, string thirdPartyType)
        {
            var validationRequest = CreateBaseValidationRequest()
                .FromFeeInfo(feeInfo)
                .FromBillerInfo(billerInfo)
                .FromValidationResponse(validationResponse, biller, thirdPartyType);

            return validationRequest;
        }

        public static BPValidationRequest CreateBaseValidationRequest()
        {
            return new BPValidationRequest
            {
                TransactionStaging = false,
                SendCurrency = Currency.Usd,
                ReceiveCurrency = Currency.Usd,
                DestinationCountry = Country.Usa,
                ProductVariant = ProductVariantType.EP,
                PrimaryReceiptLanguage = Language.Eng,
                SecondaryReceiptLanguage = Language.Spa,
                FieldValues = new List<KeyValuePairType>()
            };
        }

        public static BPValidationRequest FromFeeInfo(this BPValidationRequest validationRequest, FeeInfo feeInfo)
        {
            validationRequest.MgiSessionID = feeInfo.MgiSessionID;
            validationRequest.SendAmount = feeInfo.SendAmounts.SendAmount ?? 0m;
            validationRequest.FeeAmount = feeInfo.SendAmounts.TotalSendFees;

            return validationRequest;
        }

        public static BPValidationRequest FromBillerInfo(this BPValidationRequest validationRequest, BillerInfo billerInfo)
        {
            validationRequest.ReceiveCode = billerInfo.ReceiveCode;
            validationRequest.ReceiveAgentID = billerInfo.ReceiveAgentID;

            return validationRequest;
        }

        public static BPValidationRequest FromValidationResponse(this BPValidationRequest validationRequest, BPValidationResponse validationResponse, TestBiller biller, string thirdPartyType)
        {
            var senderCityStateZip = DataGenerator.CityStateZipInfo();
            var thirdPartyCityStateZip = DataGenerator.CityStateZipInfo();

            var fieldValues = new Dictionary<string, string>
            {
                // Sender Info
                { InfoKeyNames.sender_FirstName, DataGenerator.FirstName() },
                { InfoKeyNames.sender_LastName, DataGenerator.LastName() },
                { InfoKeyNames.sender_Country, Country.Usa },
                { InfoKeyNames.sender_Address,  DataGenerator.Address() },
                { InfoKeyNames.sender_City,  senderCityStateZip.City },
                { InfoKeyNames.sender_Country_SubdivisionCode,  senderCityStateZip.CountrySubdivisionCode },
                { InfoKeyNames.sender_PostalCode,  senderCityStateZip.PostalCode },

                // Sender Ids
                { InfoKeyNames.sender_PersonalId1_Type, IdType.DriversLicense },
                { InfoKeyNames.sender_PersonalId1_Issue_Country, Country.Usa },
                { InfoKeyNames.sender_PersonalId1_Issue_Country_SubdivisionCode, State.Mn },
                { InfoKeyNames.sender_PersonalId1_Number, DataGenerator.DriversLicense() },
                { InfoKeyNames.sender_PersonalId2_Type, IdType.Ssn },
                { InfoKeyNames.sender_PersonalId2_Number, DataGenerator.Ssn() },
                { InfoKeyNames.sender_DOB, DataGenerator.DOB() },

                // Biller info
                { InfoKeyNames.biller_AccountNumber, biller.ValidAccountNumber },

                // Third party info
                { InfoKeyNames.thirdParty_Sender_Type, thirdPartyType },
                { InfoKeyNames.thirdParty_Sender_Address, DataGenerator.Address() },
                { InfoKeyNames.thirdParty_Sender_City, thirdPartyCityStateZip.City },
                { InfoKeyNames.thirdParty_Sender_Country, Country.Usa },
                { InfoKeyNames.thirdParty_Sender_Country_SubdivisionCode, thirdPartyCityStateZip.CountrySubdivisionCode },
                { InfoKeyNames.thirdParty_Sender_DOB, DataGenerator.DOB() },
                { InfoKeyNames.thirdParty_Sender_FirstName, DataGenerator.FirstName() },
                { InfoKeyNames.thirdParty_Sender_LastName, DataGenerator.LastName() },
                { InfoKeyNames.thirdParty_Sender_Occupation, Occupation.Computer},
                { InfoKeyNames.thirdParty_Sender_Organization, DataGenerator.Organization()},
                { InfoKeyNames.thirdParty_Sender_PersonalId2_Type, IdType.Ssn },
                { InfoKeyNames.thirdParty_Sender_PersonalId2_Number, DataGenerator.Ssn() },
                { InfoKeyNames.thirdParty_Sender_PostalCode, thirdPartyCityStateZip.PostalCode }
            };

            validationRequest.FieldValues = PopulateFieldValues(validationResponse.Payload.FieldsToCollect, fieldValues);
            
            return validationRequest;
        }

        public static List<KeyValuePairType> PopulateFieldValues(List<InfoBase> infos, Dictionary<string, string> fieldValues)
        {
            var keyValuePairs = new List<KeyValuePairType>();
            var fieldsToCollect = FlattenFields(infos).Where(x => x.Required.GetValueOrDefault());

            foreach (var fieldToCollect in fieldsToCollect)
            {
                var infoKey = fieldToCollect.InfoKey;
                var value = fieldValues[infoKey];

                keyValuePairs.Add(new KeyValuePairType
                {
                    InfoKey = infoKey,
                    Value = value
                });
                
                var childField = fieldToCollect.ChildFields?.FirstOrDefault(x => x.FieldValue == value);
                if (childField != null)
                {
                    keyValuePairs.AddRange(PopulateFieldValues(childField.Infos, fieldValues));
                }
            }
            
            return keyValuePairs;
        }

        public static List<FieldToCollectInfo> FlattenFields(IEnumerable<InfoBase> fields)
        {
            var fieldsToCollect = new List<FieldToCollectInfo>();

            foreach (var infoBase in fields)
            {
                if (infoBase is CategoryInfo)
                {
                    var categoryInfo = infoBase as CategoryInfo;
                    fieldsToCollect.AddRange(FlattenFields(categoryInfo.Infos));   
                }
                else if (infoBase is FieldToCollectInfo)
                {
                    fieldsToCollect.Add(infoBase as FieldToCollectInfo);
                }
            }

            return fieldsToCollect;
        }
    }
}