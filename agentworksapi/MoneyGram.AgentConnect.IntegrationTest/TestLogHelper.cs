using System;
using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace MoneyGram.AgentConnect.IntegrationTest
{
    public static class TestLogHelper
    {
        public static string BusinessErrorsMsg(Response resp)
        {
            return $"{Environment.NewLine}BUSINESS ERRORS: {Environment.NewLine}{resp.Errors?.Log()}";
        }

        public static string AssertFailedMsg(string msg)
        {
            return $"{Environment.NewLine}ASSERT FAILED: {Environment.NewLine}{msg}";
        }
    }
}