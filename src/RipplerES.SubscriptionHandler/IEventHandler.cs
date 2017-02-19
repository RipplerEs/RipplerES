using System;

namespace RipplerES.SubscriptionHandler
{
    public interface IEventHandler
    {
        void Handle(Guid aggregateId, int version, string data, string metaData);
    }

    public interface ITypedEventHandler : IEventHandler
    {
        bool CanHandle(string eventType);
    }
}