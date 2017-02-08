using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;


namespace RipplerES.CommandHandler.Tests
{
    #region Test Class

    public class MyEvent : IAggregateEvent<EventMethodsClass> { }
    public class WrongEvent : IAggregateEvent<AggregateTypeExtensions_GetAggregateEvents> { }

    public class EventMethodsClass
    {
        public string NonVoidReturnValue(MyEvent @event)
        {
            return string.Empty;
        }

        public void MissingParamter()
        {

        }

        public void TooManyParamters(MyEvent @event, string extraParameter)
        {

        }

        public void RealEventHandler(MyEvent @event)
        {

        }

        public void WrongEvent(WrongEvent @event)
        {

        }
    }

    #endregion

    [TestClass]
    public class AggregateTypeExtensions_GetAggregateEvents
    {
        [TestMethod]
        public void WhenDoesNotHaveVoidResult_Should_NotBeIncluded()
        {
            var eventHandlers =  typeof (EventMethodsClass).GetAggregateEvents();
            eventHandlers.ShouldNotContain(c=>c.Value.Name == "NonVoidReturnValue");
        }

        [TestMethod]
        public void WhenMissingParamterResult_Should_NotBeIncluded()
        {
            var eventHandlers = typeof(EventMethodsClass).GetAggregateEvents();
            eventHandlers.ShouldNotContain(c => c.Value.Name == "MissingParamter");
        }

        [TestMethod]
        public void WhenToManyParamterResult_Should_NotBeIncluded()
        {
            var eventHandlers = typeof(EventMethodsClass).GetAggregateEvents();
            eventHandlers.ShouldNotContain(c => c.Value.Name == "TooManyParamters");
        }

        [TestMethod]
        public void WhenRealEventHandlerResult_Should_BeIncluded()
        {
            var eventHandlers = typeof(EventMethodsClass).GetAggregateEvents();
            eventHandlers.ShouldContain(c => c.Value.Name == "RealEventHandler");
            eventHandlers[typeof(MyEvent)].Name.ShouldBe("RealEventHandler");
        }

        [TestMethod]
        public void WhenWrongEventResult_Should_BeIncluded()
        {
            var eventHandlers = typeof(EventMethodsClass).GetAggregateEvents();
            eventHandlers.ShouldNotContain(c => c.Value.Name == "WrongEvent");
        }
    }
}
