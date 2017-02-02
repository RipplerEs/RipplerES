namespace RipplerES.CommandHandler
{
    public class CommandSuccessResult<T> : ICommandResult<T>
    {
        public int NewVewaionNumber {  get;  }

        public CommandSuccessResult(int newVewaionNumber)
        {
            NewVewaionNumber = newVewaionNumber;
        }
    }
}