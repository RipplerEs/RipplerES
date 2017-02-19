using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RipplerES.CommandHandler;

namespace RipplerAccountTest.AccountAggregate
{
    public class SetAccountFriendlyName : IAggregateCommand<Account>
    {
        public string Name { get; }

        public SetAccountFriendlyName(string name)
        {
            Name = name;
        }
    }
}
