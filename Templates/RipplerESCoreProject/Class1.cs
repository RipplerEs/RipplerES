using RipplerES.CommandHandler;

#region Aliases
using Result = RipplerES.CommandHandler.IAggregateCommandResult<$safeprojectname$.Aggregate1>;
#endregion

namespace $safeprojectname$
{
    public class Aggregate1 : AggregateBase<Aggregate1>
    {
        private bool _someState = true;
        public Result Execute(DoSomething command)
        {
            return !_someState 
                        ? Error(new SomeError()) 
                        : Success(new SomethingDone());
        }

        public void Apply(SomethingDone @event)
        {
            _someState = false;
        }
    }

    public class DoSomething : IAggregateCommand<Aggregate1>
    {
        
    }

    public class SomethingDone : IAggregateEvent<Aggregate1>
    {
        
    }

    public class SomeError : IAggregateError<Aggregate1>
    {
    }
}
