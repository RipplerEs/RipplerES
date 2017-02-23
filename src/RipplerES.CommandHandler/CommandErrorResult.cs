namespace RipplerES.CommandHandler
{
    public class CommandErrorResult<T> : ICommandResult<T>
    {
        public IAggregateError<T> Error { get; }
        public CommandErrorResult(IAggregateError<T> error)
        {
            Error = error;
        }
    }
}