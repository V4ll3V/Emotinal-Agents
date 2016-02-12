namespace MessageBus
{
    public interface ISubscriber<TEventType>
    {
        void OnEvent(TEventType evt);
    }
}