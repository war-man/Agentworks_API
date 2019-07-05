using System;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace MoneyGram.AgentConnect.EntityMapper.CustomExtensions
{
    public static class SessionTypeExtensions
    {
        public static Common.TrainingMode.Enums.SessionType ToTrainingModeSessionType(
            this SessionType transactionType)
        {
            switch (transactionType)
            {
                case SessionType.AMD:
                    return Common.TrainingMode.Enums.SessionType.AMD;
                case SessionType.SEND:
                    return Common.TrainingMode.Enums.SessionType.SEND;
                case SessionType.BP:
                    return Common.TrainingMode.Enums.SessionType.BP;
                case SessionType.RCV:
                    return Common.TrainingMode.Enums.SessionType.RCV;
                case SessionType.RREV:
                    return Common.TrainingMode.Enums.SessionType.RREV;
                case SessionType.SREV:
                    return Common.TrainingMode.Enums.SessionType.SREV;
                default:
                    throw new NotImplementedException($"Not implemented mapping for type {transactionType} !");
            }
        }
    }
}
