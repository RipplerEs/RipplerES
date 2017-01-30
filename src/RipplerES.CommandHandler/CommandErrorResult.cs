namespace RipplerES.CommandHandler
{
    public struct CommandErrorResult<T> : ICommandResult<T>
    {
        public AggregateErrorResult<T> Error { get; }
        public CommandErrorResult(AggregateErrorResult<T> error)
        {
            Error = error;
        }
    }
}