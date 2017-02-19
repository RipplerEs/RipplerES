using System.Linq;

namespace RipplerES.SubscriptionHandler
{
    public static class EventDataExtensions
    {
        public static IEventHandler[] CanHandle(this ITypedEventHandler[] allEventHandlers,
                                                     IEventHandler unhandledEventHandlers, 
                                                     EventData eventData)
        {
            var canHandleEvent = allEventHandlers.Where(c => c.CanHandle(eventData.EventType))
                                                 .Cast<IEventHandler>()
                                                 .ToArray();
            return canHandleEvent.Any() 
                        ? canHandleEvent.ToArray()
                        : new[] { unhandledEventHandlers };
        } 
    }
}