using RipplerES.CommandHandler;

namespace RipplerAccountTest.AccountAggregate
{
    [FriendlyName("Set Name")]
    public class AccountFriendlyNameSet : IAggregateEvent<Account>
    {
        public string Name { get; }

        public AccountFriendlyNameSet(string name)
        {
            Name = name;
        }
    }
}