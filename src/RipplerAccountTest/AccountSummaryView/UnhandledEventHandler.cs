using RipplerES.SubscriptionHandler;

namespace RipplerAccountTest.AccountSummaryView
{
    public class UnhandledEventHandler : AccountSummaryViewEventHandler, IEventHandler
    {
        public UnhandledEventHandler(ViewDataContex viewRepository) : base(viewRepository)
        {
        }

        protected override void Apply(AccountSummaryView view, int version, string data)
        {
            view.Version = version;
        }

        public bool CanHandle(string aggregateType, string eventType)
        {
            return false;
        }

        public bool CanHandle(string aggregateType)
        {
            return true;
        }
    }
}