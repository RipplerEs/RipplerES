using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RipplerES.SubscriptionHandler
{
    public class SubscriptionHandler
    {
    }

    public class Subscription
    {
        private readonly Guid _subscriptionId;
        private readonly string _aggregateName;

        public Subscription(Guid subscriptionId, string aggregateName)
        {
            _subscriptionId = subscriptionId;
            _aggregateName = aggregateName;
        }
    }
}
