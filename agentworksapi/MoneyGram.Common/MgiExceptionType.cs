using System;

namespace MoneyGram.Common
{
    [Serializable]
    public enum MgiExceptionType
    {
        AgentConnect,
        ServicesException,
        PrincipalException,
        TransactionalLimitsExceeded,
        InvalidDevice,
        ArgumentException,
        TraingModeException
    }
}