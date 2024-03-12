
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)] // this is an XUnit api setting
namespace Microsoft.Extensions.DependencyInjection.Test
{
    using System;
    using System.Diagnostics;
    using System.IO;

    using Microsoft.ApplicationInsights.AspNetCore.Extensions;
    using Microsoft.ApplicationInsights.AspNetCore.Tests;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Hosting.Internal;
    using Microsoft.Extensions.Configuration;

    public abstract class BaseTestClass
    {
        internal const string TestInstrumentationKey = "11111111-2222-3333-4444-555555555555";
        internal const string TestConnectionString = "InstrumentationKey=11111111-2222-3333-4444-555555555555;IngestionEndpoint=http://127.0.0.1";
        internal const string InstrumentationKeyInAppSettings = "33333333-2222-3333-4444-555555555555";
        internal const string InstrumentationKeyFromConfig = "ApplicationInsights:InstrumentationKey";
        internal const string InstrumentationKeyEnvironmentVariable = "APPINSIGHTS_INSTRUMENTATIONKEY";
        internal const string ConnectionStringEnvironmentVariable = "APPLICATIONINSIGHTS_CONNECTION_STRING";
        internal const string TestEndPointEnvironmentVariable = "APPINSIGHTS_ENDPOINTADDRESS";
        internal const string DeveloperModeEnvironmentVariable = "APPINSIGHTS_DEVELOPER_MODE";
        internal const string TestEndPoint = "http://127.0.0.1/v2/track";

        public static ServiceCollection CreateServicesAndAddApplicationinsightsTelemetry(string jsonPath, string channelEndPointAddress, Action<ApplicationInsightsServiceOptions> serviceOptions = null, bool addChannel = true, bool useDefaultConfig = true)
        {
            var services = GetServiceCollectionWithContextAccessor();
            if (addChannel)
            {
                services.AddSingleton<ITelemetryChannel>(new InMemoryChannel());
            }

            IConfigurationRoot config = null;

            if (jsonPath != null)
            {
                var jsonFullPath = Path.Combine(Directory.GetCurrentDirectory(), jsonPath);
                Console.WriteLine("json:" + jsonFullPath);
                Trace.WriteLine("json:" + jsonFullPath);
                try
                {
                    config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(jsonFullPath).Build();
                }
                catch (Exception)
                {
                    throw new Exception("Unable to build with json:" + jsonFullPath);
                }
            }
            else if (channelEndPointAddress != null)
            {
                config = new ConfigurationBuilder().AddApplicationInsightsSettings(endpointAddress: channelEndPointAddress).Build();
            }
            else
            {
                config = new ConfigurationBuilder().Build();
            }


            if (useDefaultConfig)
            {
                services.AddSingleton<IConfiguration>(config);
                services.AddApplicationInsightsTelemetry();
            }
            else
            {
                services.AddApplicationInsightsTelemetry(config);
            }

            if (serviceOptions != null)
            {
                services.Configure(serviceOptions);
            }
            return services;
        }

        public static ServiceCollection GetServiceCollectionWithContextAccessor()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IHostingEnvironment>(EnvironmentHelper.GetIHostingEnvironment());
            services.AddSingleton<DiagnosticListener>(new DiagnosticListener("TestListener"));
            return services;
        }
    }
}
