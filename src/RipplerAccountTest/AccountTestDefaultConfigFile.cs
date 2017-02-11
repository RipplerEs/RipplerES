using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RipplerAccountTest
{
    [TestClass]
    public class AccountTestDefaultConfigFile : AccountTests
    {
        public AccountTestDefaultConfigFile()
            : base(new Bootstrapper())
        {
        }
    }
}