using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;


namespace RipplerES.CommandHandler.Tests
{
    #region Test Class

    public class MyCommand : IAggregateCommand<CommandMethodsClass> { }
    public class WrongCommand : IAggregateCommand<AggregateTypeExtensions_GetAggregateCommands> { }

    public class CommandMethodsClass
    {
        public string NonIAggregateCommandResultReturnValue(MyCommand command)
        {
            return string.Empty;
        }

        public IAggregateCommandResult<CommandMethodsClass> MissingParamter()
        {
            return null;
        }

        public IAggregateCommandResult<CommandMethodsClass> TooManyParamters(MyCommand command, string extraParameter)
        {
            return null;
        }

        public IAggregateCommandResult<CommandMethodsClass> RealCommandMethod(MyCommand command)
        {
            return null;
        }

        public IAggregateCommandResult<AggregateTypeExtensions_GetAggregateCommands> WrongResult(MyCommand command)
        {
            return null;
        }
    }

    #endregion

    [TestClass]
    public class AggregateTypeExtensions_GetAggregateCommands
    {
        [TestMethod]
        public void WhenDoesNotHaveIAggregateCommandResult_Should_NotBeIncluded()
        {
            var commandMethods =  typeof (CommandMethodsClass).GetAggregateCommands();
            commandMethods.ShouldNotContain(c=>c.Value.Name == "NonIAggregateCommandResultReturnValue");
        }

        [TestMethod]
        public void WhenMissingParamterResult_Should_NotBeIncluded()
        {
            var commandMethods = typeof(CommandMethodsClass).GetAggregateCommands();
            commandMethods.ShouldNotContain(c => c.Value.Name == "MissingParamter");
        }

        [TestMethod]
        public void WhenToManyParamterResult_Should_NotBeIncluded()
        {
            var commandMethods = typeof(CommandMethodsClass).GetAggregateCommands();
            commandMethods.ShouldNotContain(c => c.Value.Name == "TooManyParamters");
        }

        [TestMethod]
        public void WhenRealCommandMethodResult_Should_BeIncluded()
        {
            var commandMethods = typeof(CommandMethodsClass).GetAggregateCommands();
            commandMethods.ShouldContain(c => c.Value.Name == "RealCommandMethod");
            commandMethods[typeof(MyEvent)].Name.ShouldBe("RealCommandMethod");
        }

        [TestMethod]
        public void WhenWrongEventResult_Should_BeIncluded()
        {
            var commandMethods = typeof(CommandMethodsClass).GetAggregateCommands();
            commandMethods.ShouldNotContain(c => c.Value.Name == "WrongResult");
        }
    }
}
