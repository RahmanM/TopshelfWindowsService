using ServiceTopShelf.DI;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceTopShelf
{
    /// <summary>
    /// This is the windows service
    /// </summary>
    class InvoiceWindowsService : WindowsServiceBase
    {

        private CancellationTokenSource cancellationTokenSource;
        private List<Task> Tasks { get; set; }

        public override bool OnContinue()
        {
            BaseLogger.Information("OnContinue");
            return true;
        }

        public override bool OnPause()
        {
            BaseLogger.Information("OnPause");
            return true;
        }

        public override void OnShutdown()
        {
            BaseLogger.Information("OnShutdown");
            cancellationTokenSource.Cancel();
        }

        public override void OnStart()
        {
            // Will be called the first time windows service is started!
            Tasks = new List<Task>();
        
            // Configure the DI and dependencies and intitialize the Manager
            Console.WriteLine("OnStart" + DateTime.Now);
            var container = ConfigureDependency.Configure();

            var invoiceManager = container.GetInstance<IInvoiceManager>();
            invoiceManager.ServiceLogger = this.BaseLogger;

            cancellationTokenSource = new CancellationTokenSource();
            Tasks.Add(invoiceManager.ProcessInvoices(cancellationTokenSource.Token));

        }     

        public override void OnStop()
        {
            BaseLogger.Information("OnStop");
            cancellationTokenSource.Cancel();
        }
    }
   
}
