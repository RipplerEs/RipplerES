using RipplerES.CommandHandler;

namespace RipplerAccountTest.AccountAggregate
{
    public class Withdraw : IAggregateCommand<Account>
    {
        public Withdraw(double amount)
        {
            Amount = amount;
        }

        public double Amount { get; }
    }
}