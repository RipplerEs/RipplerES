using Newtonsoft.Json.Linq;
using RipplerAccountTest.AccountAggregate;
using RipplerES.SubscriptionHandler;

namespace RipplerAccountTest.AccountSummaryView
{
    public class HandleAccountNameSet : AccountSummaryViewEventHandler, IEventHandler
    {
        public HandleAccountNameSet(ViewDataContex viewRepository) : base(viewRepository)
        {
        }

        protected override void Apply(AccountSummaryView view, int version, string data)
        {
            dynamic json = JObject.Parse(data);
            view.Version = version;
            view.FriendlyName = json.Name;
        }

        public bool CanHandle(string aggregateType, string eventType)
        {
            return aggregateType == typeof(Account).AssemblyQualifiedName &&
                eventType == typeof(AccountFriendlyNameSet).AssemblyQualifiedName;
        }

        public bool CanHandle(string aggregateType)
        {
            return false;
        }
    }
}