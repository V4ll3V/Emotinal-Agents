using System;

namespace MessageBus
{
    public interface IMessageBus
    {
        void PublishEvent<TEventType>(TEventType evt);
        void Subscribe(Object subscriber);
    }
}