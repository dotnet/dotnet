namespace Microsoft.ApplicationInsights.AspNetCore.Logging
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

#pragma warning disable CS0618 // ApplicationInsightsLoggerOptions is obsolete. This will not be fixed because this class is also obsolete.
    /// <summary>
    /// <see cref="ILoggerProvider"/> implementation that creates returns instances of <see cref="ApplicationInsightsLogger"/>.
    /// </summary>
    /// <remarks>
    /// THIS CLASS IS OBSOLETE.
    /// For NETSTANDARD2.0 and NET461 We take dependency on Microsoft.Extensions.Logging.ApplicationInsights which has ApplicationInsightsProvider having the same ProviderAlias and don't want to clash with this ProviderAlias.
    /// </remarks>
    [SuppressMessage("Documentation Rules", "SA1614:ElementParameterDocumentationMustHaveText", Justification = "This class is obsolete and will not be completely documented.")]
    [Obsolete] 
    internal class ApplicationInsightsLoggerProvider : ILoggerProvider
    {
        private readonly TelemetryClient telemetryClient;
        private readonly Func<string, LogLevel, bool> filter;
        private readonly ApplicationInsightsLoggerOptions options;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationInsightsLoggerProvider"/> class.
        /// </summary>
        /// <param name="telemetryClient"></param>
        /// <param name="options"></param>
        /// <param name="filter"></param>
        public ApplicationInsightsLoggerProvider(TelemetryClient telemetryClient, Func<string, LogLevel, bool> filter, IOptions<ApplicationInsightsLoggerOptions> options)
        {
            this.telemetryClient = telemetryClient;
            this.filter = filter;
            this.options = options.Value;
        }

        /// <inheritdoc />
        public ILogger CreateLogger(string categoryName)
        {
            return new ApplicationInsightsLogger(categoryName, this.telemetryClient, this.filter, this.options);
        }

        /// <inheritdoc />
        public void Dispose()
        {
        }
    }
#pragma warning restore CS0618
}