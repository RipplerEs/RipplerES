using RipplerES.CommandHandler;

namespace RipplerAccountTest.AccountAggregate
{
    public class Deposited : IAggregateEvent<Account>
    {
        public Deposited(double amount)
        {
            Amount = amount;
        }

        public double Amount { get; }
    }
}