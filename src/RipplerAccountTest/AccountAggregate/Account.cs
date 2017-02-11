
#region Aliases

using System.Globalization;
using RipplerES.CommandHandler;
using Result  = RipplerES.CommandHandler.IAggregateCommandResult<RipplerAccountTest.AccountAggregate.Account>;
#endregion

namespace RipplerAccountTest.AccountAggregate
{
    public class Account: AggregateBase<Account>, ISnapshotable
    {
        public double _balance = 0;

        public Result Execute(Deposit command)
        {
            return Success(new Deposited(amount: command.Amount));
        }

        public void Apply(Deposited @event)
        {
            _balance += @event.Amount;
        }

        public Result Execute(Withdraw command)
        {
            if(_balance - command.Amount > 0)
                return Success(new Withdrawn(amount: command.Amount ));

            return Error(new InsufficientFunds());
        }

        public void Apply(Withdrawn @event)
        {
            _balance -= @event.Amount;
        }


        #region Snapshotable
        public string TakeSnapshot()
        {
            return _balance.ToString(CultureInfo.InvariantCulture);
        }

        public void RestoreFromSnapshot(string snapshot)
        {
            if (!string.IsNullOrWhiteSpace(snapshot))
                _balance = double.Parse(snapshot);
        }
        #endregion
    }
}
