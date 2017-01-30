using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RipplerES.CommandHandler;

namespace EventStoreRepositoryTests
{
    [TestClass]
    public class AggregateRegisteryTests
    {
        public object State { get; set; }


        public static AggregateRegisteryTests Factory()
        {
            return new AggregateRegisteryTests();
        }

        void When(ASpecificEvent anEvent)
        {
            State = anEvent;
        }

        void When(AggregateEvent anEvent)
        {
            State = anEvent.Obj;
        }

        public IAggregateCommandResult<AggregateRegisteryTests> Specify(SpecificCommand command)
        {
            return new AggregateSuccessResult<AggregateRegisteryTests>(new ASpecificEvent());
        }


        public class ASpecificEvent : IAggregateEvent<AggregateRegisteryTests>
        {
            
        }

        public class SpecificCommand : IAggregateCommand<AggregateRegisteryTests>
        {

        }

        public class AggregateEvent : IAggregateEvent<AggregateRegisteryTests>
        {
            public object Obj { get; set; }

            public AggregateEvent(object obj)
            {
                Obj = obj;
            }
        }

        [TestMethod]
        public void WhenYouRegisterAnAggergateAndCallLoad_YouGetANewAggregateAfterTheEventBeingPassedToIT()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<AggregateRegisteryTests>();

            var aggregate = new AggregateRoot<AggregateRegisteryTests>(serviceCollection.BuildServiceProvider());
            var events = new List<IAggregateEvent<AggregateRegisteryTests>> { new AggregateEvent(new object()) };

            var instance = aggregate.CreateFromInitialState();
            aggregate.Apply(instance, events);

            Assert.AreEqual(instance.State.GetHashCode(), ((AggregateEvent)events.First()).Obj.GetHashCode());
        }


        [TestMethod]
        public void WhenYouRegisterAnAggergateAndCallLoad_YouGetANewAggregateAfterTheEventBeingPassedToIT2()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<AggregateRegisteryTests>();

            var aggregate = new AggregateRoot<AggregateRegisteryTests>(serviceCollection.BuildServiceProvider());
            var command = new SpecificCommand();

            var @event = aggregate.Exec(aggregate.CreateFromInitialState(), command);
            var success = @event as AggregateSuccessResult<AggregateRegisteryTests>;

            Assert.IsNotNull(success);
            Assert.IsInstanceOfType(success.Event, typeof(ASpecificEvent));
        }
    }
}
