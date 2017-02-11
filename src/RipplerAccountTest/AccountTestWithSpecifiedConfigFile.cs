using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RipplerAccountTest
{
    [TestClass]
    public class AccountTestWithSpecifiedConfigFile : AccountTests
    {
        public static readonly IConfigurationRoot Configuration =
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json", optional: true, reloadOnChange: true)
                .Build();

        public AccountTestWithSpecifiedConfigFile() 
            : base(new Bootstrapper(configurationRoot: Configuration))
        {
        }
    }
}