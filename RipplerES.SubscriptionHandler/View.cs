using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RipplerES.SubscriptionHandler
{
    public class View 
    {
        protected Subscription[] GetSubscription()
        {
            return null;
        }

        public void Process()
        {
            foreach (var subscription in GetSubscription())
            {
                Process(subscription.Fetch());
            }

            Save();
        }

        private void Save()
        {
            
        }

        private void Process(EventData[] events)
        {
            
        }
    }

    public class EventData
    {
    }

    public class Subscription
    {
        public EventData[] Fetch()
        {
            throw new NotImplementedException();
        }
    }
}
