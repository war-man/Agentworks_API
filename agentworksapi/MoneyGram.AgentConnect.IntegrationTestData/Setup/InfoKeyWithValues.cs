using MoneyGram.AgentConnect.DomainModel.Transaction;
using System.Collections.Generic;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Setup
{
    public static class InfoKeyWithValues
    {
        private static CityStateZipInfo senderCityStateZip = DataGenerator.CityStateZipInfo();
        private static CityStateZipInfo thirdPartyCityStateZip = DataGenerator.CityStateZipInfo();

        private static Dictionary<string, string> GetFieldValues()
        {
            return new Dictionary<string, string>()
            {
                // Sender Info
                {InfoKeyNames.sender_FirstName, DataGenerator.FirstName()},
                {InfoKeyNames.sender_LastName, DataGenerator.LastName()},
                {InfoKeyNames.sender_Country, Country.Usa},
                {InfoKeyNames.sender_Address, DataGenerator.Address()},
                {InfoKeyNames.sender_City, senderCityStateZip.City},
                {InfoKeyNames.sender_Country_SubdivisionCode, senderCityStateZip.CountrySubdivisionCode},
                {InfoKeyNames.sender_PostalCode, senderCityStateZip.PostalCode},
                {InfoKeyNames.sender_PrimaryPhoneCountryCode, DataGenerator.PhoneCountryCode()},
                {InfoKeyNames.sender_PrimaryPhone, DataGenerator.Phone()},
                {InfoKeyNames.sender_NameSuffixOther, "Y"},
                {InfoKeyNames.sender_Birth_Country, Country.Usa},
                {InfoKeyNames.sender_Occupation, Occupation.Computer},
                {InfoKeyNames.sender_LastName2, DataGenerator.LastName()},
                {InfoKeyNames.sender_NameSuffix, "Jr."},

                // Sender Ids
                {InfoKeyNames.sender_PersonalId1_Type, IdType.DriversLicense},
                {InfoKeyNames.sender_PersonalId1_Issue_Country, Country.Usa},
                {InfoKeyNames.sender_PersonalId1_Issue_Country_SubdivisionCode, State.Mn},
                {InfoKeyNames.sender_PersonalId1_Number, DataGenerator.DriversLicense()},
                {InfoKeyNames.sender_PersonalId2_Type, IdType.Ssn},
                {InfoKeyNames.sender_PersonalId2_Number, DataGenerator.Ssn()},
                {InfoKeyNames.sender_DOB, DataGenerator.DOB()},
                {InfoKeyNames.sender_PersonalId1_Expiration_Year, DataGenerator.Expiration_Year()},
                {InfoKeyNames.sender_PersonalId1_Expiration_Month, DataGenerator.Month()},
                {InfoKeyNames.sender_PersonalId1_Expiration_Day, DataGenerator.Day()},

                //Receiver Info   
                {InfoKeyNames.receiver_FirstName, DataGenerator.FirstName()},
                {InfoKeyNames.receiver_MiddleName, DataGenerator.MiddleName()},
                {InfoKeyNames.receiver_LastName, DataGenerator.LastName()},
                {InfoKeyNames.receiver_LastName2, DataGenerator.LastName()},
                {InfoKeyNames.receiver_PrimaryPhoneCountryCode, DataGenerator.PhoneCountryCode()},
                {InfoKeyNames.receiver_PrimaryPhone, DataGenerator.Phone()},
                {InfoKeyNames.receiver_NameSuffix, "Jr."},
                {InfoKeyNames.receiver_NameSuffixOther, "Y"},
                {InfoKeyNames.receiver_City, DataGenerator.City()},
                {InfoKeyNames.relationshipToReceiver, "FRIEND"},
                {InfoKeyNames.receiver_Gender, "M"},
                {InfoKeyNames.relationshipToSender, "FRIEND"},
                {InfoKeyNames.receiver_DOB, DataGenerator.DOB()},
                {InfoKeyNames.receiver_Birth_Country, DataGenerator.Country()},
                {InfoKeyNames.receiver_PersonalId2_Type, IdType.Ssn},
                {InfoKeyNames.receiver_PersonalId2_Number, DataGenerator.Ssn()},
                {InfoKeyNames.receiver_Country, DataGenerator.Country()},
                {InfoKeyNames.receiver_Country_SubdivisionCode, DataGenerator.SubdivisionCode()},
                {InfoKeyNames.receiver_Address, DataGenerator.Address()},
                {InfoKeyNames.receiver_PostalCode, DataGenerator.PostalCode()},
                {InfoKeyNames.receiver_Occupation, DataGenerator.Occupation()},
                {InfoKeyNames.receiver_PersonalId1_Type, IdType.DriversLicense},
                {InfoKeyNames.receiver_PersonalId1_Number, DataGenerator.DriversLicense()},
                {InfoKeyNames.receiver_PersonalId1_Expiration_Year, DataGenerator.Expiration_Year()},
                {InfoKeyNames.receiver_PersonalId1_Expiration_Month, DataGenerator.Month()},
                {InfoKeyNames.receiver_PersonalId1_Expiration_Day, DataGenerator.Day()},
                {InfoKeyNames.receiver_PersonalId1_Issue_Country, DataGenerator.Country()},
                {InfoKeyNames.receiver_PersonalId1_Issue_Country_SubdivisionCode, DataGenerator.SubdivisionCode()},
                {InfoKeyNames.receiverAccountIssueStatePartnerField, "049"},

                // Biller info
                {InfoKeyNames.biller_AccountNumber, "replace"},
                {InfoKeyNames.accountNumberRetryCount, "2"},

                // Third party info
                {InfoKeyNames.thirdParty_Sender_Address, DataGenerator.Address()},
                {InfoKeyNames.thirdParty_Sender_City, thirdPartyCityStateZip.City},
                {InfoKeyNames.thirdParty_Sender_Country, Country.Usa},
                {InfoKeyNames.thirdParty_Sender_Country_SubdivisionCode, thirdPartyCityStateZip.CountrySubdivisionCode},
                {InfoKeyNames.thirdParty_Sender_DOB, DataGenerator.DOB()},
                {InfoKeyNames.thirdParty_Sender_FirstName, DataGenerator.FirstName()},
                {InfoKeyNames.thirdParty_Sender_LastName, DataGenerator.LastName()},
                {InfoKeyNames.thirdParty_Sender_Occupation, Occupation.Computer},
                {InfoKeyNames.thirdParty_Sender_Organization, DataGenerator.Organization()},

                {InfoKeyNames.thirdParty_Sender_PersonalId2_Type, IdType.Ssn},
                {InfoKeyNames.thirdParty_Sender_PersonalId2_Number, DataGenerator.Ssn()},
                {InfoKeyNames.thirdParty_Sender_PostalCode, thirdPartyCityStateZip.PostalCode},
                {InfoKeyNames.primaryReceiptLanguage, Language.Eng},

                {InfoKeyNames.operatorName, DataGenerator.OperatorName()},
                {InfoKeyNames.messageField1, DataGenerator.MessageField()},
                {InfoKeyNames.messageField2, DataGenerator.MessageField()},
                {InfoKeyNames.testQuestion, DataGenerator.Question()},
                {InfoKeyNames.testAnswer, DataGenerator.Answer()},
                {InfoKeyNames.direction1, DataGenerator.Direction()},
                {InfoKeyNames.direction2, DataGenerator.Direction()},
                {InfoKeyNames.receive_ReversalReason, ReceiveReversalReasons.WantsCash},
                {InfoKeyNames.send_ReversalReason, SendReversalReasons.WrongService},
                {InfoKeyNames.payout1_Type, "CASH"},
                {InfoKeyNames.payout2_Type, "CHK"},
                {InfoKeyNames.accountNumber, DataGenerator.AccountNumber()},
                {InfoKeyNames.mgiRewardsNumber, DataGenerator.MgiRewardsNumber()},

                //Consumer
                {InfoKeyNames.consumer_FirstName, DataGenerator.FirstName()},
                {InfoKeyNames.consumer_LastName, DataGenerator.LastName()},
                {InfoKeyNames.consumer_Address, DataGenerator.Address()},
                {InfoKeyNames.consumer_City, DataGenerator.City()},
                {InfoKeyNames.consumer_Country, DataGenerator.Country()},
                {InfoKeyNames.consumer_PrimaryPhone, DataGenerator.Phone()},
                {InfoKeyNames.consumer_PrimaryPhoneCountryCode, DataGenerator.PhoneCountryCode()},
                {InfoKeyNames.consumer_DOB, DataGenerator.DOB()},
                {InfoKeyNames.consumer_AccountNumber, DataGenerator.AccountNumber()}
            };
        }

        public static KeyValuePairType GetFieldValue(string infoKey)
        {
            // Generate new keys..
            var fieldValues = GetFieldValues();
            return (new KeyValuePairType() { InfoKey = infoKey, Value = (fieldValues.ContainsKey(infoKey) ? fieldValues[infoKey] : string.Empty) });
        }
    }
}