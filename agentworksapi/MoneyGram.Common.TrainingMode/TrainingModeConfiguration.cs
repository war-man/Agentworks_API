using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MoneyGram.Common.Json;
using MoneyGram.Common.TrainingMode.Enums;

namespace MoneyGram.Common.TrainingMode
{
    /// <summary>
    /// Class for handling Training Mode configuration read from TrainingModeConfiguration.json file
    /// </summary>
    public class TrainingModeConfiguration
    {
        /// <summary>
        /// Scenarios collection
        /// </summary>
        public List<TrainingModeScenario> Scenarios { get; set; }

        private static string ConfigurationPath => Path.Combine(JsonFileHelper.ExecutingDir(), TrainingModeFolderName, "TrainingModeConfiguration.json");

        private const string TrainingModeFolderName = "TrainingModeResources";

        /// <summary>
        /// Gets response for training mode scenario step specified by parameters
        /// </summary>
        /// <typeparam name="TResponse">Type of response to be deserialized</typeparam>
        /// <param name="sessionType">Type of session</param>
        /// <param name="referenceNumber">Reference number / MGI Session Id</param>
        /// <param name="getFileName">Expression to get required file name from TrainingModeResponses object</param>
        /// <returns>Response object if response is found, otherwise exception is thrown.</returns>
        public static TResponse GetResponse<TResponse>(SessionType? sessionType, string referenceNumber, Func<TrainingModeResponses, string> getFileName)
        {
            if ((sessionType == SessionType.SEND || sessionType == SessionType.BP) && referenceNumber != null && !IsStagedTransaction(sessionType, referenceNumber))
            {
                referenceNumber = null;
            }

            TrainingModeConfiguration instance = GetConfiguration();

            TrainingModeResponses responses = instance.Scenarios.SingleOrDefault(x =>
                x.SessionType == sessionType && x.ReferenceNumber == referenceNumber)?.Responses;

            if (responses != null)
            {
                string sessionTypePath = sessionType != null ? sessionType.ToString() : "AllTypes";

                string responseFilePath = Path.Combine(JsonFileHelper.ExecutingDir(), TrainingModeFolderName,
                    sessionTypePath, referenceNumber ?? string.Empty, getFileName(responses));

                if (File.Exists(responseFilePath))
                {
                    try
                    {
                        return JsonProcessor.DeserializeObject<TResponse>(JsonFileHelper.GetFileContents(responseFilePath), true);
                    }
                    catch (Exception exception)
                    {
                        throw new InvalidDataException($"Could not deserialize Training Mode response for sessionType:{sessionType}, referenceNumber:{referenceNumber}.", exception);
                    }
                }
            }
            else
            {
                switch (sessionType)
                {
                    case SessionType.AMD:
                        throw new TrainingModeException("STRKEY_TRAINING_ERROR_AMEND_REF_NUMBER");
                    case SessionType.SREV:
                        throw new TrainingModeException("STRKEY_TRAINING_ERROR_SREV_REF_NUMBER");
                    case SessionType.RCV:
                        throw new TrainingModeException("STRKEY_TRAINING_ERROR_RCV_REF_NUMBER");
                }
            }

            throw new FileNotFoundException($"Could not find Training Mode response file for sessionType:{sessionType}, referenceNumber:{referenceNumber}.");
        }

        private static TrainingModeConfiguration GetConfiguration()
        {
            if (!File.Exists(ConfigurationPath))
            {
                throw new FileNotFoundException($"Could not find Training Mode configuration file under path: {ConfigurationPath}.");
            }

            try
            {
                return JsonProcessor.DeserializeObject<TrainingModeConfiguration>(JsonFileHelper.GetFileContents(ConfigurationPath));
            }
            catch (Exception exception)
            {
                throw new InvalidDataException($"Could not deserialize Training Mode configuration file under path: {ConfigurationPath}.", exception);
            }
        }

        /// <summary>
        /// Checks if Training Mode transaction with provided type and mgiSessionId is staged
        /// </summary>
        /// <param name="sessionType">Session type to check</param>
        /// <param name="mgiSessionId">MgiSessionId to check</param>
        /// <returns></returns>
        public static bool IsStagedTransaction(SessionType? sessionType, string mgiSessionId)
        {
            TrainingModeConfiguration instance = GetConfiguration();
            return instance.Scenarios.Any(x => x.SessionType == sessionType && x.ReferenceNumber == mgiSessionId && x.IsStaged);
        }

        public static TResponse GetTransactionLookupStatusResponse<TResponse>(string referenceNumber, Func<TrainingModeResponses, string> getFileName)
        {
            var instance = GetConfiguration();

            var scenario = instance.Scenarios.SingleOrDefault(x => x.ReferenceNumber == referenceNumber && getFileName(x.Responses) != null);
           

            if (scenario?.Responses == null) throw new TrainingModeException( "STRKEY_TRAINING_ERROR_EDIT_TRAN_REF_NUMBER");

            var responseFilePath = Path.Combine(JsonFileHelper.ExecutingDir(), TrainingModeFolderName, scenario.SessionType.ToString(), referenceNumber ?? string.Empty, getFileName(scenario.Responses));
            if (!File.Exists(responseFilePath)) throw new FileNotFoundException($"Could not find Training Mode Transaction Lookup Status response file for referenceNumber:{referenceNumber}.");
            try
            {
                return JsonProcessor.DeserializeObject<TResponse>(JsonFileHelper.GetFileContents(responseFilePath), true);
            }
            catch (Exception exception)
            {
                throw new InvalidDataException($"Could not deserialize Training Mode Transaction Lookup Status response for sessionType:{scenario.SessionType}, referenceNumber:{referenceNumber}.", exception);
            }
        }
    }
}