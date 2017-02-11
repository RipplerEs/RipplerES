using RipplerES.CommandHandler;

namespace RipplerAccountTest.AccountAggregate
{
    public class Withdrawn : IAggregateEvent<Account>
    {
        public double Amount { get; }

        public Withdrawn(double amount)
        {
            Amount = amount;
        }
    }
}