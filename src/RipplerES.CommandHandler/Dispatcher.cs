using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RipplerES.CommandHandler
{
    public class Dispatcher
    {
        private readonly IServiceProvider _serviceProvider;
        public Dispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ICommandResult<T> Execute<T>(Guid id, int version,  IAggregateCommand<T> command)
        {
            var commandHandler = GetCommandHandler<T>();
            return commandHandler.Handle(id, version, command);
        }

        private CommandHandler<T> GetCommandHandler<T>()
        {
            return (CommandHandler<T>)_serviceProvider.GetService(typeof(CommandHandler<T>));
        }
    }
}
