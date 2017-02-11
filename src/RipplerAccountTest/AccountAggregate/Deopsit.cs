using RipplerES.CommandHandler;

namespace RipplerAccountTest.AccountAggregate
{
    public class Deposit : IAggregateCommand<Account>
    {
        public Deposit(double amount)
        {
            Amount = amount;
        }

        public double Amount { get; }
    }
}