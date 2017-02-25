using System;
using System.Linq;

namespace RipplerES.SubscriptionHandler
{
    public class SubscriptionHandler
    {
        private readonly Guid _subscriptionId;

        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IEventHandler[] _eventHandlers;
        private readonly string _name;

        public SubscriptionHandler(
            Guid subscriptionId,
            string name,
            ISubscriptionRepository subscriptionRepository,
            IEventHandler[] eventHandlers)
        {
            _subscriptionId = subscriptionId;
            _name = name;
            _subscriptionRepository = subscriptionRepository;
            _eventHandlers = eventHandlers;
        }

        public void Initialize()
        {
            _subscriptionRepository.Subscribe(_subscriptionId, _name);
        }

        public void Execute()
        {
            foreach (var @event in _subscriptionRepository.Fetch(_subscriptionId))
            {
                foreach (var handler in _eventHandlers.CanHandleEvent(@event))
                {
                    handler.Handle(@event.AggregateId, @event.Version, @event.Data, @event.MetaData);
                }
            }
        }
    }
}