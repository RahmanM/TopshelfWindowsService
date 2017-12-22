using Serilog;
using Serilog.Core;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceTopShelf.DI
{
    public static class ConfigureDependency
    {
        public static Container Configure()
        {
            var container = new Container();
            container.Register<IDatabaseHelper, DatabaseHelper>();
            container.Register<IInvoiceManager, InvoiceManager>();
            container.Verify();
            return container;
        }
    }
}
