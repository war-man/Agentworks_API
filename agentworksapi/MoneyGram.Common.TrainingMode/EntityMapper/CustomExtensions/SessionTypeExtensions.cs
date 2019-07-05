using System;
using MoneyGram.Common.TrainingMode.Enums;

namespace MoneyGram.Common.TrainingMode.EntityMapper.CustomExtensions
{
    public static class SessionTypeExtensions
    {
        public static string ToCode(this SessionType sessionType)
        {
            switch (sessionType)
            {
                case SessionType.RCV:
                    return "REC";
                case SessionType.SEND:
                    return "SEN";
                default:
                    throw new NotImplementedException($"Not implemented mapping for {sessionType} to code !");

            }
        }
    }
}