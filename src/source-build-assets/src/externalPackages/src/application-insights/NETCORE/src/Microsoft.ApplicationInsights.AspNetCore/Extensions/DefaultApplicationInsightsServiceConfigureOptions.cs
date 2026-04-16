namespace Microsoft.AspNetCore.Hosting
{
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using Microsoft.ApplicationInsights.AspNetCore.Extensions;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// <see cref="IConfigureOptions&lt;ApplicationInsightsServiceOptions&gt;"/> implementation that reads options from 'appsettings.json',
    /// environment variables and sets developer mode based on debugger state.
    /// </summary>
    internal class DefaultApplicationInsightsServiceConfigureOptions : IConfigureOptions<ApplicationInsightsServiceOptions>
    {
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IConfiguration userConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultApplicationInsightsServiceConfigureOptions"/> class.
        /// </summary>
        /// <param name="hostingEnvironment"><see cref="IHostingEnvironment"/> to use for retreiving ContentRootPath.</param>
        /// <param name="configuration"><see cref="IConfiguration"/>  from an application.</param>
        public DefaultApplicationInsightsServiceConfigureOptions(IHostingEnvironment hostingEnvironment, IConfiguration configuration = null)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.userConfiguration = configuration;
        }

        /// <inheritdoc />
        public void Configure(ApplicationInsightsServiceOptions options)
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(this.hostingEnvironment.ContentRootPath ?? Directory.GetCurrentDirectory());
            if (this.userConfiguration != null)
            {
                configBuilder.AddConfiguration(this.userConfiguration);
            }

            configBuilder.AddJsonFile("appsettings.json", true)
                         .AddJsonFile(string.Format(CultureInfo.InvariantCulture, "appsettings.{0}.json", this.hostingEnvironment.EnvironmentName), true)
                         .AddEnvironmentVariables();

            ApplicationInsightsExtensions.AddTelemetryConfiguration(configBuilder.Build(), options);

            if (Debugger.IsAttached)
            {
                options.DeveloperMode = true;
            }
        }
    }
}
