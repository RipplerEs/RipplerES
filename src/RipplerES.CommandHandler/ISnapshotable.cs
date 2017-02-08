using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RipplerES.CommandHandler
{
    public interface ISnapshotable
    {
        string TakeSnapshot();
        void RestoreFromSnapshot(string snapshot);
    }
}
