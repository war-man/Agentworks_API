using System.Collections.Generic;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Setup
{
    public static class BusinessErrors
    {
        public static string CheckVerify => "CHECK_VERIFY";
        public static string Validation => "VALIDATION";
        public static string MissingFieldsOptional => "MISSING_FIELDS_OPTIONAL";
        public static string MissingFields => "MISSING_FIELDS";

        public static string ClientIntegrationError => "ClientIntegrationError";

        public static List<string> ErrorCodesToContinueValidation => new List<string>() { CheckVerify, Validation, MissingFields };
        public static List<string> ErrorCodesToExitValidation => new List<string>() { ClientIntegrationError, MissingFieldsOptional };
    } 
}