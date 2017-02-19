using System;


namespace RipplerES.SubscriptionHandler
{
    public interface ISubscriptionRepository
    {
        void Subscribe(Guid channelId, string name);
        EventData[] Fetch(Guid channelId);
    }
}
