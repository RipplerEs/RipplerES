using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RipplerAccountTest
{
    [TestClass]
    public class AccountTestDatabaseOnlyConfigFile : AccountTests
    {
        public static readonly IConfigurationRoot Configuration =
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("DatabaseOnly.json", optional: true, reloadOnChange: true)
                .Build();

        public AccountTestDatabaseOnlyConfigFile() 
            : base(new Bootstrapper(configuration: Configuration))
        {
        }
    }
}