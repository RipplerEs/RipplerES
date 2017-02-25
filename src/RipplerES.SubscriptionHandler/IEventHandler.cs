using System;

namespace RipplerES.SubscriptionHandler
{
    public interface IEventHandler
    {
        void Handle(Guid aggregateId, int version, string data, string metaData);
        bool CanHandle(string aggregateType, string eventType);
        bool CanHandle(string aggregateType);
    }
}