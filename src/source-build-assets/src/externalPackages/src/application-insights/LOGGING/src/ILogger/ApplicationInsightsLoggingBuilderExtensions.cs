// -----------------------------------------------------------------------
// <copyright file="ApplicationInsightsLoggingBuilderExtensions.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. 
// All rights reserved.  2013
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Extensions.Logging
{
    using System;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Logging.ApplicationInsights;

    /// <summary>
    /// Extensions methods to add and configure application insights logger.
    /// </summary>
    public static class ApplicationInsightsLoggingBuilderExtensions
    {
        /// <summary>
        /// Adds an ApplicationInsights logger named 'ApplicationInsights' to the factory.
        /// </summary>
        /// <param name="builder">The <see cref="ILoggingBuilder"/> to use.</param>
        /// <returns>Logging builder with Application Insights added to it.</returns>
        public static ILoggingBuilder AddApplicationInsights(this ILoggingBuilder builder)
        {
            return builder.AddApplicationInsights((applicationInsightsOptions) => { });
        }

        /// <summary>
        /// Adds an ApplicationInsights logger named 'ApplicationInsights' to the factory.
        /// </summary>
        /// <param name="builder">The <see cref="ILoggingBuilder"/> to use.</param>
        /// <param name="instrumentationKey">Application insights instrumentation key.</param>
        /// <returns>Logging builder with Application Insights added to it.</returns>
        [Obsolete("InstrumentationKey based global ingestion is being deprecated. Use the AddApplicationInsights() overload which accepts Action<TelemetryConfiguration> and set TelemetryConfiguration.ConnectionString. See https://github.com/microsoft/ApplicationInsights-dotnet/issues/2560 for more details.")]
        public static ILoggingBuilder AddApplicationInsights(this ILoggingBuilder builder, string instrumentationKey)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder.AddApplicationInsights(
                (telemetryConfiguration) => telemetryConfiguration.InstrumentationKey = instrumentationKey,
                (applicationInsightsOptions) => { });
        }

        /// <summary>
        /// Adds an ApplicationInsights logger named 'ApplicationInsights' to the factory.
        /// </summary>
        /// <param name="builder">The <see cref="ILoggingBuilder"/> to use.</param>
        /// <param name="instrumentationKey">Application insights instrumentation key.</param>
        /// <param name="configureApplicationInsightsLoggerOptions">Action to configure ApplicationInsights logger.</param>
        /// <returns>Logging builder with Application Insights added to it.</returns>
        [Obsolete("InstrumentationKey based global ingestion is being deprecated. Use the AddApplicationInsights() overload which accepts Action<TelemetryConfiguration> and set TelemetryConfiguration.ConnectionString. See https://github.com/microsoft/ApplicationInsights-dotnet/issues/2560 for more details.")]
        public static ILoggingBuilder AddApplicationInsights(
            this ILoggingBuilder builder,
            string instrumentationKey,
            Action<ApplicationInsightsLoggerOptions> configureApplicationInsightsLoggerOptions)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder.AddApplicationInsights(
                (telemetryConfiguration) => telemetryConfiguration.InstrumentationKey = instrumentationKey,
                configureApplicationInsightsLoggerOptions);
        }

        /// <summary>
        /// Adds an ApplicationInsights logger named 'ApplicationInsights' to the factory.
        /// </summary>
        /// <param name="builder">The <see cref="ILoggingBuilder"/> to use.</param>
        /// <param name="configureApplicationInsightsLoggerOptions">Action to configure ApplicationInsights logger.</param>
        public static ILoggingBuilder AddApplicationInsights(
            this ILoggingBuilder builder,
            Action<ApplicationInsightsLoggerOptions> configureApplicationInsightsLoggerOptions)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder.AddApplicationInsights(
                (telemetryConfiguration) => { },
                configureApplicationInsightsLoggerOptions);
        }

        /// <summary>
        /// Adds an ApplicationInsights logger named 'ApplicationInsights' to the factory.
        /// </summary>
        /// <param name="builder">The <see cref="ILoggingBuilder"/> to use.</param>
        /// <param name="configureTelemetryConfiguration">Action to configure telemetry configuration.</param>
        /// <param name="configureApplicationInsightsLoggerOptions">Action to configure ApplicationInsights logger.</param>
        public static ILoggingBuilder AddApplicationInsights(
            this ILoggingBuilder builder,
            Action<TelemetryConfiguration> configureTelemetryConfiguration,
            Action<ApplicationInsightsLoggerOptions> configureApplicationInsightsLoggerOptions)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (configureTelemetryConfiguration == null)
            {
                throw new ArgumentNullException(nameof(configureTelemetryConfiguration));
            }

            if (configureApplicationInsightsLoggerOptions == null)
            {
                throw new ArgumentNullException(nameof(configureApplicationInsightsLoggerOptions));
            }

            builder.Services.Configure<TelemetryConfiguration>(configureTelemetryConfiguration);

            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, ApplicationInsightsLoggerProvider>());
            builder.Services.Configure(configureApplicationInsightsLoggerOptions);

            return builder;
        }
    }
}
