using Serilog;
using Serilog.Core;
using System.IO;
using Topshelf;

namespace ServiceTopShelf
{

    public class ServiceConfigurationHelper
    {
        public WindowsServiceBase ServiceType { get; set; }
        public IServiceDependencies ServiceDependencies { get; }

        public Logger Logger { get; set; }

        public ServiceConfigurationHelper(WindowsServiceBase windowsService)
        {
            ServiceType = windowsService;
        }

        public ServiceConfigurationHelper(WindowsServiceBase windowsService, IServiceDependencies serviceDependencies)
        {
            if (windowsService == null)
                throw new System.ArgumentNullException(nameof(windowsService));

            if (serviceDependencies == null)
                throw new System.ArgumentNullException(nameof(serviceDependencies));

            ServiceType = windowsService;
            ServiceDependencies = serviceDependencies;

            Logger = ConfigureLogger(serviceDependencies);

            Logger.Information("Constructor initialised.");
        }

        public void Configure()
        {
            Logger.Information("Configuring the base service [Start]");

            HostFactory.Run(x =>
            {
                x.Service<WindowsServiceBase>(sc =>
                {
                    sc.ConstructUsing(() => ServiceType);

                    // the start and stop methods for the service
                    sc.WhenStarted(s => s.Start(this.Logger));
                    sc.WhenStopped(s => s.Stop());

                    // optional pause/continue methods if used
                    sc.WhenPaused(s => s.Pause());
                    sc.WhenContinued(s => s.Continue());

                    // optional, when shutdown is supported
                    sc.WhenShutdown(s => s.Shutdown());

                    x.RunAsLocalSystem();
                    x.StartAutomatically();

                    Logger.Information("Configuring the base service [End]");
                });
            });

        }

        public virtual Logger ConfigureLogger(IServiceDependencies serviceDependencies)
        {

            var logger  = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File(
                                Path.Combine(serviceDependencies.LoggingConfiguration?.LogFolder, 
                                serviceDependencies.LoggingConfiguration?.LogFile), 
                                rollingInterval: RollingInterval.Day
                              )
                .CreateLogger();

            return logger;
        }
    }
}
