namespace Microsoft.ApplicationInsights.WorkerService
{
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// <see cref="IConfigureOptions&lt;ApplicationInsightsServiceOptions&gt;"/> implementation that reads options from provided IConfiguration.
    /// </summary>
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "This class is instantiated by Dependency Injection.")]
    internal class DefaultApplicationInsightsServiceConfigureOptions : IConfigureOptions<ApplicationInsightsServiceOptions>
    {
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultApplicationInsightsServiceConfigureOptions"/> class.
        /// </summary>
        /// <param name="configuration"><see cref="IConfiguration"/> from which configuration for ApplicationInsights can be retrieved.</param>
        public DefaultApplicationInsightsServiceConfigureOptions(IConfiguration configuration = null)
        {
            this.configuration = configuration;
        }

        /// <inheritdoc />
        public void Configure(ApplicationInsightsServiceOptions options)
        {
            if (this.configuration != null)
            {
                ApplicationInsightsExtensions.AddTelemetryConfiguration(this.configuration, options);
            }

            if (Debugger.IsAttached)
            {
                options.DeveloperMode = true;
            }
        }
    }
}
