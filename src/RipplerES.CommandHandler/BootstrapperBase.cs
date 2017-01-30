using System;
using Microsoft.Extensions.DependencyInjection;

namespace RipplerES.CommandHandler
{
    public abstract class BootstrapperBase
    {
        private readonly IServiceCollection _serviceCollection;

        protected BootstrapperBase() : this(new ServiceCollection())
        {
            
        }

        protected BootstrapperBase(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }

        #region Internals
        public virtual void RegisterGlobalEventRepositoryIfExists()
        {
            var globalRepository = GetGlobalEventRepositoryType();
            if (globalRepository != null)
            {
                _serviceCollection.AddTransient(typeof (IEventRepository), globalRepository);
            }
        }

        protected void RegisterGlobalEventSerializerIfExists()
        {
            var globalSerializer = GetGlobalEventSerializerType();
            if (globalSerializer != null)
            {
                _serviceCollection.AddTransient(typeof(ISerializer), globalSerializer);
            }
        }

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

        protected virtual Type GetGlobalEventSerializerType()
        {
            return null;
        }
        protected virtual Type GetGlobalEventRepositoryType()
        {
            return null;
        }

        protected virtual void RegisterServices(IServiceCollection serviceCollection)
        {

        }

        public IServiceProvider CreateServiceProvider()
        {
            RegisterGlobalEventRepositoryIfExists();
            RegisterGlobalEventSerializerIfExists();
            RegisterServices(_serviceCollection);
            RegisterHander();

            return _serviceCollection.BuildServiceProvider();
        }
    }
}
