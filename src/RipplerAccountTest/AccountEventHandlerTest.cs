using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using RipplerAccountTest.AccountAggregate;
using RipplerAccountTest.AccountSummaryView;
using RipplerES.CommandHandler;
using RipplerES.Repositories.SqlServer;
using RipplerES.SubscriptionHandler;
using Shouldly;

namespace RipplerAccountTest
{
    [TestClass]
    public class AccountSummaryViewTests
    {
        private readonly Dispatcher _dispatcher;
        public AccountSummaryViewTests()
        {
            _dispatcher = new Dispatcher(new Bootstrapper().CreateServiceProvider());
        }

        public static readonly IConfigurationRoot Configuration =
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json", optional: true, reloadOnChange: true)
                .Build();

        [TestMethod]
        public void Test2()
        {
            var subscriptionId = Guid.Parse("CF5E138D-4ADC-478F-BCB9-33901DB548B7");
            var aggregateId = Guid.NewGuid();
            

            var viewRepository = new ViewDataContex();
            var initialCount = viewRepository.AccountSummaryViews.Count();

            var handler = new SubscriptionHandler(subscriptionId, "Account Summary View",
                                                  new SqlServerSubscriptionRepository(Configuration), eventHandlers: new ITypedEventHandler[]
                                                  {
                                                        new HandleAccountNameSet(viewRepository),
                                                        new HandleDeposited(viewRepository),
                                                        new HandleWitdrawn(viewRepository)
                                                  }, unhandledEventHandler: new UnhandledEventHandler(viewRepository));
            handler.Initialize();

            _dispatcher.Execute(aggregateId, -1, new SetAccountFriendlyName(name: "My Checking Account"));
            handler.Execute();

            viewRepository.AccountSummaryViews.Count().ShouldBe(initialCount + 1);
            viewRepository.AccountSummaryViews.Single(c=>c.AggregateId == aggregateId).AggregateId.ShouldBe(aggregateId);
            viewRepository.AccountSummaryViews.Single(c=>c.AggregateId == aggregateId).Version.ShouldBe(1);

            viewRepository.AccountSummaryViews.Single(c=>c.AggregateId == aggregateId).FriendlyName.ShouldBe("My Checking Account");
            viewRepository.AccountSummaryViews.Single(c=>c.AggregateId == aggregateId).Balance.ShouldBe(0);


            _dispatcher.Execute(aggregateId, viewRepository.AccountSummaryViews.Single(c=>c.AggregateId == aggregateId).Version, new Deposit(amount: 20));
            handler.Execute();

            viewRepository.AccountSummaryViews.Count().ShouldBe(initialCount + 1);
            viewRepository.AccountSummaryViews.Single(c=>c.AggregateId == aggregateId).AggregateId.ShouldBe(aggregateId);
            viewRepository.AccountSummaryViews.Single(c=>c.AggregateId == aggregateId).Version.ShouldBe(2);

            viewRepository.AccountSummaryViews.Single(c=>c.AggregateId == aggregateId).FriendlyName.ShouldBe("My Checking Account");
            viewRepository.AccountSummaryViews.Single(c=>c.AggregateId == aggregateId).Balance.ShouldBe(20);

            _dispatcher.Execute(aggregateId, viewRepository.AccountSummaryViews.Single(c=>c.AggregateId == aggregateId).Version, new Deposit(amount: 30));
            handler.Execute();

            viewRepository.AccountSummaryViews.Count().ShouldBe(initialCount + 1);
            viewRepository.AccountSummaryViews.Single(c=>c.AggregateId == aggregateId).AggregateId.ShouldBe(aggregateId);
            viewRepository.AccountSummaryViews.Single(c=>c.AggregateId == aggregateId).Version.ShouldBe(3);

            viewRepository.AccountSummaryViews.Single(c=>c.AggregateId == aggregateId).FriendlyName.ShouldBe("My Checking Account");
            viewRepository.AccountSummaryViews.Single(c=>c.AggregateId == aggregateId).Balance.ShouldBe(50);

            _dispatcher.Execute(aggregateId, viewRepository.AccountSummaryViews.Single(c=>c.AggregateId == aggregateId).Version, new Withdraw(amount: 15));
            handler.Execute();

            viewRepository.AccountSummaryViews.Count().ShouldBe(initialCount + 1);
            viewRepository.AccountSummaryViews.Single(c=>c.AggregateId == aggregateId).AggregateId.ShouldBe(aggregateId);
            viewRepository.AccountSummaryViews.Single(c=>c.AggregateId == aggregateId).Version.ShouldBe(4);

            viewRepository.AccountSummaryViews.Single(c=>c.AggregateId == aggregateId).FriendlyName.ShouldBe("My Checking Account");
            viewRepository.AccountSummaryViews.Single(c=>c.AggregateId == aggregateId).Balance.ShouldBe(35);
        }
    }
}
