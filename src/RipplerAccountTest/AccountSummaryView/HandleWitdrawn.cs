using Newtonsoft.Json.Linq;
using RipplerAccountTest.AccountAggregate;
using RipplerES.SubscriptionHandler;

namespace RipplerAccountTest.AccountSummaryView
{
    public class HandleWitdrawn : AccountSummaryViewEventHandler, IEventHandler
    {
        public HandleWitdrawn(ViewDataContex viewRepository) : base(viewRepository)
        {
        }

        protected override void Apply(AccountSummaryView view, int version, string data)
        {
            dynamic json = JObject.Parse(data);
            view.Version = version;
            view.Balance -= (double)json.Amount;
        }

        public bool CanHandle(string aggregateType, string eventType)
        {
            return aggregateType == typeof(Account).AssemblyQualifiedName &&
                eventType == typeof(Withdrawn).AssemblyQualifiedName;
        }

        public bool CanHandle(string eventType)
        {
            return false;
        }
    }
}