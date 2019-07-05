using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace MoneyGram.AgentConnect.IntegrationTest.Data.Common
{
    public static class CompleteSessionRequestFactory
    {
        private static CompleteSessionRequest CreateBaseRequest(string mgiSessionId)
        {
            return new CompleteSessionRequest
            {
                MgiSessionID = mgiSessionId,
                Commit = true
            };
        }

        public static CompleteSessionRequest CompleteBillPayRequest(string mgiSessionId)
        {
            var request = CreateBaseRequest(mgiSessionId);
            request.MgiSessionType = SessionType.BP;
            return request;
        }

        public static CompleteSessionRequest CompleteSendRequest(string mgiSessionId)
        {
            var request = CreateBaseRequest(mgiSessionId);
            request.MgiSessionType = SessionType.SEND;
            return request;
        }

        public static CompleteSessionRequest CompleteRecvRequest(string mgiSessionId)
        {
            var request = CreateBaseRequest(mgiSessionId);
            request.MgiSessionType = SessionType.RCV;
            return request;
        }

        public static CompleteSessionRequest CompleteReceiveReversalRequest(string mgiSessionId)
        {

            var request = CreateBaseRequest(mgiSessionId);
            request.MgiSessionType = SessionType.RREV;
            return request;
        }

        public static CompleteSessionRequest CompleteSendReversalRequest(string mgiSessionId)
        {

            var request = CreateBaseRequest(mgiSessionId);
            request.MgiSessionType = SessionType.SREV;
            return request;
        }
    }
}