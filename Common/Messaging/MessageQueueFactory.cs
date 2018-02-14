namespace Common.Messaging
{
    public class MessageQueueFactory
    {
        public IMessageQueue GetReadOnlyQueue(string endpoint)
        {
            return new ZmqPull(endpoint);
        }

        public IMessageQueue GetWriteOnlyQueue(string endpoint)
        {
            return new ZmqPush(endpoint);
        }
    }
}
