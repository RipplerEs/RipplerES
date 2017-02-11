using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RippleES.Serialization.Json;
using RipplerAccountTest.AccountAggregate;
using RipplerES.CommandHandler;
using RipplerES.Repositories.SqlServer;

namespace RipplerAccountTest
{
    public class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper(IServiceCollection serviceCollection = null,
                               IConfigurationRoot configurationRoot = null) 
            : base(serviceCollection, configurationRoot)
        {
        }

        protected override void RegisterHander()
        {
            RegisterHandlerFor<Account>();
        }

        protected override void RegisterServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IEventRepository, SqlServerEventRepository>();
            serviceCollection.AddTransient<ISerializer, JsonSerializer>();
        }
    }
}
