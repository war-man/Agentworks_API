using MoneyGram.AgentConnect.DomainModel.Transaction;

namespace AwApi.EntityMapper
{
    public static class RequestExtensions
    {
        public static GetAllFieldsRequest ToGetAllFieldsRequest(this Request request)
        {
            return new GetAllFieldsRequest
            {
                UnitProfileID = request.UnitProfileID,
                AgentID = request.AgentID,
                AgentSequence = request.AgentSequence,
                Language = request.Language,
                MgiSessionID = request.MgiSessionID,
                TimeStamp = request.TimeStamp,
                ClientSoftwareVersion = request.ClientSoftwareVersion,
                PoeType = request.PoeType,
                ChannelType = request.ChannelType,
                OperatorName = request.OperatorName,
                TargetAudience = request.TargetAudience,
                PoeCapabilities = request.PoeCapabilities
            };
        }

        public static GetEnumerationsRequest ToGetEnumerationsRequest(this Request request)
        {
            return new GetEnumerationsRequest
            {
                UnitProfileID = request.UnitProfileID,
                AgentID = request.AgentID,
                AgentSequence = request.AgentSequence,
                Language = request.Language,
                MgiSessionID = request.MgiSessionID,
                TimeStamp = request.TimeStamp,
                ClientSoftwareVersion = request.ClientSoftwareVersion,
                PoeType = request.PoeType,
                ChannelType = request.ChannelType,
                OperatorName = request.OperatorName,
                TargetAudience = request.TargetAudience,
                PoeCapabilities = request.PoeCapabilities
            };
        }
    }
}