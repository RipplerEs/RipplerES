using System;
using System.Linq;
using RipplerES.SubscriptionHandler;

namespace RipplerAccountTest.AccountSummaryView
{
    public abstract class AccountSummaryViewEventHandler
    {
        private readonly ViewDataContex _viewRepository;
        protected AccountSummaryViewEventHandler(ViewDataContex viewRepository)
        {
            _viewRepository = viewRepository;
        }

        public void Handle(Guid aggregateId, int version, string data, string metaData)
        {
            AccountSummaryView accountSummary;
            if (version == 1)
            {
                accountSummary = new AccountSummaryView
                {
                    AggregateId = aggregateId,
                    Version = version - 1
                };
                _viewRepository.AccountSummaryViews.Add(accountSummary);
            }
            else
            {
                accountSummary = _viewRepository.AccountSummaryViews
                    .Single(c => c.AggregateId == aggregateId);
            }

            if (accountSummary.Version != version - 1)
                throw new Exception("Unexpected version");

            Apply(accountSummary, version, data);
            _viewRepository.SaveChanges();
        }

        protected abstract void Apply(AccountSummaryView view, int version, string data);
    }
}