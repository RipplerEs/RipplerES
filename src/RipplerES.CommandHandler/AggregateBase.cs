using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RipplerES.CommandHandler
{
    public abstract class AggregateBase<T>
    {
        public IAggregateCommandResult<T> Success(IAggregateEvent<T> @event)
        {
            return new AggregateCommandSuccessResult<T>(@event);
        }

        public IAggregateCommandResult<T> Error(IAggregateError<T> error)
        {
            return new AggregateCommandErrorResult<T>(error);
        }
    }
}
