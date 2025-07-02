
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();

                    // Application Insights

                    // Add custom TelemetryInitializer
                    services.AddSingleton<ITelemetryInitializer, MyCustomTelemetryInitializer>();

                    // Add custom TelemetryProcessor
                    services.AddApplicationInsightsTelemetryProcessor<MyCustomTelemetryProcessor>();

                    // Example on Configuring TelemetryModules.
                    // [SuppressMessage("Microsoft.Security", "CS002:SecretInNextLine", Justification="Not a real api key, this is example code.")]
                    services.ConfigureTelemetryModule<QuickPulseTelemetryModule>((mod, opt) => mod.AuthenticationApiKey = "put_actual_authentication_key_here");

                    // instrumentation key is read automatically from appsettings.json
                    services.AddApplicationInsightsTelemetryWorkerService();
                });

        internal class MyCustomTelemetryInitializer : ITelemetryInitializer
        {
            public void Initialize(ITelemetry telemetry)
            {
                // Replace with actual properties.
                (telemetry as ISupportProperties).Properties["MyCustomKey"] = "MyCustomValue";
            }
        }

        internal class MyCustomTelemetryProcessor : ITelemetryProcessor
        {
            ITelemetryProcessor next;

            public MyCustomTelemetryProcessor(ITelemetryProcessor next)
            {
                this.next = next;
            }

            public void Process(ITelemetry item)
            {
                // Example processor - not filtering out anything.
                // This should be replaced with actual logic.
                this.next.Process(item);
            }
        }
    }
}
