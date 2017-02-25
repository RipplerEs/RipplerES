using System.Linq;

namespace RipplerES.SubscriptionHandler
{
    public static class EventDataExtensions
    {
        public static IEventHandler[] CanHandleEvent(this IEventHandler[] allEventHandlers,
                                                     EventData eventData)
        {
            var canHandleEvent = allEventHandlers.Where(c => c.CanHandle(eventData.AggregateType, eventData.EventType))
                                                 .ToArray();

            var canHandleMissingEvent = allEventHandlers.Where(c => c.CanHandle(eventData.AggregateType))
                                                 .ToArray();


            return canHandleEvent.Any() 
                        ? canHandleEvent
                        : canHandleMissingEvent;
        } 
    }
}