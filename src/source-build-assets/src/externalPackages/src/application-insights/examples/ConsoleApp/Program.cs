using Azure.Identity;

using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse;
using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;
using Microsoft.ApplicationInsights.WorkerService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleAppWithApplicationInsights
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create the DI container.
            IServiceCollection services = new ServiceCollection();

            // Add or configure channel
            services.AddSingleton<ITelemetryChannel>(new ServerTelemetryChannel() { StorageFolder = @"C:\temp\aisdkstorage" });

            // Add custom TelemetryInitializer
            services.AddSingleton<ITelemetryInitializer, MyCustomTelemetryInitializer>();

            // Configure TelemetryConfiguration
            services.Configure<TelemetryConfiguration>(config =>
            {
                // Optionally configure AAD
                //var credential = new DefaultAzureCredential();
                //config.SetAzureTokenCredential(credential);
            });

            // Being a regular console app, there is no appsettings.json or configuration providers enabled by default.
            // Hence connection string must be specified here.
            services.AddApplicationInsightsTelemetryWorkerService((ApplicationInsightsServiceOptions options) => options.ConnectionString = "InstrumentationKey=00000000-0000-0000-0000-000000000000");

            // Add custom TelemetryProcessor
            services.AddApplicationInsightsTelemetryProcessor<MyCustomTelemetryProcessor>();

            // Example on Configuring TelemetryModules.
            // [SuppressMessage("Microsoft.Security", "CS002:SecretInNextLine", Justification="Not a real api key, this is example code.")]
            services.ConfigureTelemetryModule<QuickPulseTelemetryModule>((module, opt) => module.AuthenticationApiKey = "put_actual_authentication_key_here");

            // Build ServiceProvider.
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            // Obtain logger instance from DI.
            ILogger<Program> logger = serviceProvider.GetRequiredService<ILogger<Program>>();

            // Obtain TelemetryClient instance from DI, for additional manual tracking or to flush.
            var telemetryClient = serviceProvider.GetRequiredService<TelemetryClient>();

            var res = new HttpClient().GetAsync("https://bing.com").Result.StatusCode; // this dependency will be captured by Application Insights.
            logger.LogWarning("Response from bing is:" + res); // this will be captured by Application Insights.

            telemetryClient.TrackEvent("sampleevent");

            // Explicitly call Flush() followed by sleep is required in Console Apps.
            // This is to ensure that even if application terminates, telemetry is sent to the back-end.
            telemetryClient.Flush();
            Task.Delay(500000).Wait();
        }
    }

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
