using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RipplerAccountTest.AccountAggregate;
using RipplerES.CommandHandler;
using Shouldly;

namespace RipplerAccountTest
{
    [TestClass]
    public class AccountTestsWithMissingConfig
    {
        public static readonly IConfigurationRoot Configuration =
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("missing.json", optional: true, reloadOnChange: true)
                .Build();

        private readonly Dispatcher _dispatcher;
        public AccountTestsWithMissingConfig()
        {
            _dispatcher = new Dispatcher(new Bootstrapper(configuration: Configuration)
                                            .CreateServiceProvider());
        }

        [TestMethod]
        public void Deposit()
        {
            var id = Guid.NewGuid();
            try
            {
                _dispatcher.Execute(id, -1, new Deposit(amount: 10));
            }
            catch (Exception ex)
            {
                ex.ShouldBeOfType<InvalidOperationException>();
                ex.Message.ShouldBe("The ConnectionString property has not been initialized.");
            }
        }
    }
}
