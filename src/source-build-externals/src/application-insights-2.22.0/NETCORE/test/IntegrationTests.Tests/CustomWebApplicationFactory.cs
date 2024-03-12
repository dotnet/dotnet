using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationTests.Tests
{
    public class CustomWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup> where TStartup : class
    {
        internal ConcurrentBag<ITelemetry> sentItems = new ConcurrentBag<ITelemetry>();


        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {

            builder.ConfigureServices(services =>
            {
                services.AddLogging(loggingBuilder =>
                loggingBuilder.AddFilter<ApplicationInsightsLoggerProvider>("Microsoft.AspNetCore.DataProtection.KeyManagement.XmlKeyManager", LogLevel.None));

                services.AddSingleton<ITelemetryChannel>(new StubChannel()
                {
                    OnSend = (item) => this.sentItems.Add(item)
                });
                var aiOptions = new ApplicationInsightsServiceOptions();
                aiOptions.AddAutoCollectedMetricExtractor = false;
                aiOptions.EnableAdaptiveSampling = false;
                aiOptions.InstrumentationKey = "ikey";
                services.AddApplicationInsightsTelemetry(aiOptions);
            });
        }
    }
}
