using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RipplerES.CommandHandler
{
    public abstract class BootstrapperBase
    {
        private readonly IServiceCollection _serviceCollection;
        private readonly IConfiguration _configuration;

        protected BootstrapperBase(IServiceCollection serviceCollection = null,
                                   IConfiguration configuration = null)
        {
            _serviceCollection  = serviceCollection     ?? new ServiceCollection();
            _configuration      = configuration         ?? CreateDefaultConfigurationRoot();
        }

        public IConfigurationRoot CreateDefaultConfigurationRoot()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json", optional: true, reloadOnChange: true);

            return builder.Build();
        }

        #region Internals
        protected void RegisterHandlerFor<T>()
        {
            _serviceCollection.AddTransient(typeof(T));
            _serviceCollection.AddSingleton(typeof(CommandHandler<T>), 
                                            typeof(CommandHandler<T>));
        }

        protected void RegisterHandlerFor<T>(CommandHandler<T> instance)
        {
            _serviceCollection.AddTransient(typeof(T));
            _serviceCollection.AddSingleton(typeof(CommandHandler<T>), 
                                            instance);
        }
        #endregion

        protected abstract void RegisterHander();

        protected virtual void RegisterServices(IServiceCollection serviceCollection)
        {

        }

        public IServiceProvider CreateServiceProvider()
        {
            RegisterConfiguration();
            RegisterServices(_serviceCollection);
            RegisterHander();

            return _serviceCollection.BuildServiceProvider();
        }

        private void RegisterConfiguration()
        {
            _serviceCollection.AddSingleton(_configuration);
        }
    }
}
