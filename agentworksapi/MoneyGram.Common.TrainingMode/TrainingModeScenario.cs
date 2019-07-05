using MoneyGram.Common.TrainingMode.Enums;

namespace MoneyGram.Common.TrainingMode
{
    /// <summary>
    /// Specification of one particular scenario for Training Mode
    /// </summary>
    public class TrainingModeScenario
    {
        /// <summary>
        /// Session Type
        /// </summary>
        public SessionType? SessionType { get; set; }

        /// <summary>
        /// Reference Number
        /// </summary>
        public string ReferenceNumber { get; set; }

        /// <summary>
        /// True when scenario is for staged transaction
        /// </summary>
        public bool IsStaged { get; set; }

        /// <summary>
        /// Response files specification
        /// </summary>
        public TrainingModeResponses Responses { get; set; }
    }
}