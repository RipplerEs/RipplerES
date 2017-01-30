using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace RipplerES.EFCoreRepository
{
    public class SqlServerInstaller
    {
        public void Install(IServiceCollection collection)
        {
            collection.AddTransient<EventContext>();
            collection.AddTransient<EfCoreEventRepository>();
        }
    }
}
