using TransactionRunner.Interfaces;

namespace TransactionRunner.Messaging
{
    public static class StaticMessageBus
    {
        private static IMessageBus _messageBus;

        public static IMessageBus MessageBus
        {
            get { return _messageBus ?? (_messageBus = new MessageBus()); }
            set { _messageBus = value; }
        }
    }
}