namespace RipplerAccountTest.AccountSummaryView
{
    public class UnhandledEventHandler : AccountSummaryViewEventHandler
    {
        public UnhandledEventHandler(ViewDataContex viewRepository) : base(viewRepository)
        {
        }

        protected override void Apply(AccountSummaryView view, int version, string data)
        {
            view.Version = version;
        }
    }
}