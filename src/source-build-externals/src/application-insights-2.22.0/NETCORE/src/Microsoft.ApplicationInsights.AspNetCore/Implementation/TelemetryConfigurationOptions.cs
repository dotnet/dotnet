namespace Microsoft.Extensions.DependencyInjection
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Microsoft.ApplicationInsights.AspNetCore.Extensions;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// The <see cref="IOptions{TelemetryConfiguration}"/> implementation that create new <see cref="TelemetryConfiguration"/> every time when called".
    /// </summary>
    internal class TelemetryConfigurationOptions : IOptions<TelemetryConfiguration>
    {
        private static readonly object LockObject = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="TelemetryConfigurationOptions"/> class.
        /// </summary>
        /// <param name="configureOptions">Options to be configured.</param>
        /// <param name="applicationInsightsServiceOptions">User defined serviceOptions.</param>
        public TelemetryConfigurationOptions(IEnumerable<IConfigureOptions<TelemetryConfiguration>> configureOptions, IOptions<ApplicationInsightsServiceOptions> applicationInsightsServiceOptions)
        {
            this.Value = TelemetryConfiguration.CreateDefault();

            var configureOptionsArray = configureOptions.ToArray();
            foreach (var c in configureOptionsArray)
            {
                c.Configure(this.Value);
            }

            if (applicationInsightsServiceOptions.Value.EnableActiveTelemetryConfigurationSetup)
            {
                lock (LockObject)
                {
                    // workaround for Microsoft/ApplicationInsights-dotnet#613
                    // as we expect some customers to use TelemetryConfiguration.Active together with dependency injection
                    // we make sure it has been set up, it must be done only once even if there are multiple Web Hosts in the process
                    if (!IsActiveConfigured(this.Value.InstrumentationKey))
                    {
                        foreach (var c in configureOptionsArray)
                        {
#pragma warning disable CS0618 // This must be maintained for backwards compatibility.
                            c.Configure(TelemetryConfiguration.Active);
#pragma warning restore CS0618
                        }
                    }
                }
            }
        }

        /// <inheritdoc />
        public TelemetryConfiguration Value { get; }

        /// <summary>
        /// Determines if TelemetryConfiguration.Active needs to be configured.
        /// </summary>
        /// <param name="instrumentationKey">Instrumentation key.</param>
        /// <returns>True is TelemertryConfiguration.Active was previously configured.</returns>
        private static bool IsActiveConfigured(string instrumentationKey)
        {
#pragma warning disable CS0618 // This must be maintained for backwards compatibility.
            var active = TelemetryConfiguration.Active;
#pragma warning restore CS0618
            if (string.IsNullOrEmpty(active.InstrumentationKey) && !string.IsNullOrEmpty(instrumentationKey))
            {
                return false;
            }

            if (active.TelemetryInitializers.Count <= 1)
            {
                return false;
            }

            return true;
        }
    }
}