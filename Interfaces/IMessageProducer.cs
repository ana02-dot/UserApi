namespace UserProfileAPI.Interfaces
{
    public interface IMessageProducer
    {
        public void SendingMessage<T>(T message);

    }
}
