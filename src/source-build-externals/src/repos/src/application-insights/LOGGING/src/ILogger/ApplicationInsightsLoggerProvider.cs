// -----------------------------------------------------------------------
// <copyright file="ApplicationInsightsLoggerProvider.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. 
// All rights reserved.  2013
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Extensions.Logging.ApplicationInsights
{
    using System;
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Implementation;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Application insights logger provider.
    /// </summary>
    /// <seealso cref="ILoggerProvider" />
    /// <seealso cref="ISupportExternalScope" />
    [ProviderAlias("ApplicationInsights")]
    public class ApplicationInsightsLoggerProvider : ILoggerProvider, ISupportExternalScope
    {
        /// <summary>
        /// The application insights logger options.
        /// </summary>
        private readonly ApplicationInsightsLoggerOptions applicationInsightsLoggerOptions;

        /// <summary>
        /// The telemetry client to be used to log messages to Application Insights.
        /// </summary>
        private readonly TelemetryClient telemetryClient;

        /// <summary>
        /// The external scope provider to allow setting scope data in messages.
        /// </summary>
        private IExternalScopeProvider externalScopeProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationInsightsLoggerProvider"/> class.
        /// </summary>
        /// <param name="telemetryConfigurationOptions">The telemetry configuration options..</param>
        /// <param name="applicationInsightsLoggerOptions">The application insights logger options.</param>
        /// <exception cref="System.ArgumentNullException">
        /// telemetryConfiguration
        /// or
        /// loggingFilter
        /// or
        /// applicationInsightsLoggerOptions.
        /// </exception>
        public ApplicationInsightsLoggerProvider(
            IOptions<TelemetryConfiguration> telemetryConfigurationOptions,
            IOptions<ApplicationInsightsLoggerOptions> applicationInsightsLoggerOptions)
        {
            if (telemetryConfigurationOptions?.Value == null)
            {
                throw new ArgumentNullException(nameof(telemetryConfigurationOptions));
            }

            this.applicationInsightsLoggerOptions = applicationInsightsLoggerOptions?.Value ?? throw new ArgumentNullException(nameof(applicationInsightsLoggerOptions));

            this.telemetryClient = new TelemetryClient(telemetryConfigurationOptions.Value);
            this.telemetryClient.Context.GetInternalContext().SdkVersion = SdkVersionUtils.GetSdkVersion("il:");
        }

        /// <summary>
        /// Creates a new <see cref="ILogger" /> instance.
        /// </summary>
        /// <param name="categoryName">The category name for messages produced by the logger.</param>
        /// <returns>An <see cref="ILogger"/> instance to be used for logging.</returns>
        public ILogger CreateLogger(string categoryName)
        {
            return new ApplicationInsightsLogger(
                    categoryName,
                    this.telemetryClient,
                    this.applicationInsightsLoggerOptions)
            {
                ExternalScopeProvider = this.externalScopeProvider,
            };
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Sets the scope provider. This method also updates all the existing logger to also use the new ScopeProvider.
        /// </summary>
        /// <param name="externalScopeProvider">The external scope provider.</param>
        public void SetScopeProvider(IExternalScopeProvider externalScopeProvider)
        {
            this.externalScopeProvider = externalScopeProvider;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources. 
        /// </summary>
        /// <param name="releasedManagedResources">Release managed resources.</param>
        protected virtual void Dispose(bool releasedManagedResources)
        {
            if (releasedManagedResources && this.applicationInsightsLoggerOptions.FlushOnDispose)
            {
                this.telemetryClient.Flush();

                // With the ServerTelemetryChannel, Flush pushes buffered telemetry to the Transmitter,
                // but it doesn't guarantee that all events have been transmitted to the endpoint.
                // TODO: Should we sleep here? Should that be controlled by options?
            }
        }
    }
}
