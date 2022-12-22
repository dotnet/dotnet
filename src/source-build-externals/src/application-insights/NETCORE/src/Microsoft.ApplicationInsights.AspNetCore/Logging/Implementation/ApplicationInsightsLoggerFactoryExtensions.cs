namespace Microsoft.Extensions.Logging
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.ApplicationInsights;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Extension methods for <see cref="ILoggerFactory"/> that allow adding Application Insights logger.
    /// </summary>
    [SuppressMessage("Microsoft.Usage", "SA1614:ElementParameterDocumentationMustHaveText", Justification = "Obsolete class")]
    [SuppressMessage("Microsoft.Usage", "SA1615:ElementReturnValueMustBeDocumented", Justification = "Obsolete class")]
    public static class ApplicationInsightsLoggerFactoryExtensions
    {
        /// <summary>
        /// Adds an ApplicationInsights logger that is enabled for <see cref="LogLevel.Warning"/> or higher.
        /// </summary>
        /// <param name="factory">Used to configure the logging system and create instances of ILogger from the registered ILoggerProviders.</param>
        /// <param name="serviceProvider">The instance of <see cref="IServiceProvider"/> to use for service resolution.</param>
        [Obsolete("ApplicationInsightsLoggerProvider is now enabled by default when enabling ApplicationInsights monitoring using UseApplicationInsights extension method on IWebHostBuilder or AddApplicationInsightsTelemetry extension method on IServiceCollection. From 2.7.0-beta3 onwards, calling this method will result in double logging and filters applied will not get applied. If interested in using just logging provider, then please use Microsoft.Extensions.Logging.ApplicationInsightsLoggingBuilderExtensions.AddApplicationInsights from Microsoft.Extensions.Logging.ApplicationInsights package. Read more https://aka.ms/ApplicationInsightsILoggerFaq")]
        public static ILoggerFactory AddApplicationInsights(this ILoggerFactory factory, IServiceProvider serviceProvider)
        {
            return factory.AddApplicationInsights(serviceProvider, LogLevel.Warning);
        }

        /// <summary>
        /// Adds an ApplicationInsights logger that is enabled for <see cref="LogLevel"/>s of minLevel or higher.
        /// </summary>
        /// <param name="factory">Used to configure the logging system and create instances of ILogger from the registered ILoggerProviders.</param>
        /// <param name="serviceProvider">The instance of <see cref="IServiceProvider"/> to use for service resolution.</param>
        /// <param name="minLevel">The minimum <see cref="LogLevel"/> to be logged.</param>
        [Obsolete("ApplicationInsightsLoggerProvider is now enabled by default when enabling ApplicationInsights monitoring using UseApplicationInsights extension method on IWebHostBuilder or AddApplicationInsightsTelemetry extension method on IServiceCollection. From 2.7.0-beta3 onwards, calling this method will result in double logging and filters applied will not get applied. If interested in using just logging provider, then please use Microsoft.Extensions.Logging.ApplicationInsightsLoggingBuilderExtensions.AddApplicationInsights from Microsoft.Extensions.Logging.ApplicationInsights package. Read more https://aka.ms/ApplicationInsightsILoggerFaq")]
        public static ILoggerFactory AddApplicationInsights(
            this ILoggerFactory factory,
            IServiceProvider serviceProvider,
            LogLevel minLevel)
        {
            factory.AddApplicationInsights(serviceProvider, (category, logLevel) => logLevel >= minLevel);
            return factory;
        }

        /// <summary>
        /// Adds an ApplicationInsights logger that is enabled as defined by the filter function.
        /// </summary>
        /// <param name="factory">Used to configure the logging system and create instances of ILogger from the registered ILoggerProviders.</param>
        /// <param name="serviceProvider">The instance of <see cref="IServiceProvider"/> to use for service resolution.</param>
        /// <param name="filter"></param>
        [Obsolete("ApplicationInsightsLoggerProvider is now enabled by default when enabling ApplicationInsights monitoring using UseApplicationInsights extension method on IWebHostBuilder or AddApplicationInsightsTelemetry extension method on IServiceCollection. From 2.7.0-beta3 onwards, calling this method will result in double logging and filters applied will not get applied. If interested in using just logging provider, then please use Microsoft.Extensions.Logging.ApplicationInsightsLoggingBuilderExtensions.AddApplicationInsights from Microsoft.Extensions.Logging.ApplicationInsights package. Read more https://aka.ms/ApplicationInsightsILoggerFaq")]
        public static ILoggerFactory AddApplicationInsights(
            this ILoggerFactory factory,
            IServiceProvider serviceProvider,
            Func<string, LogLevel, bool> filter)
        {
            return factory.AddApplicationInsights(serviceProvider, filter, null);
        }

        /// <summary>
        /// Adds an ApplicationInsights logger that is enabled as defined by the filter function.
        /// </summary>
        /// <param name="factory">Used to configure the logging system and create instances of ILogger from the registered ILoggerProviders.</param>
        /// <param name="serviceProvider">The instance of <see cref="IServiceProvider"/> to use for service resolution.</param>
        /// <param name="filter">Filter action.</param>
        /// <param name="loggerAddedCallback">The callback that gets executed when another ApplicationInsights logger is added.</param>
        [Obsolete("ApplicationInsightsLoggerProvider is now enabled by default when enabling ApplicationInsights monitoring using UseApplicationInsights extension method on IWebHostBuilder or AddApplicationInsightsTelemetry extension method on IServiceCollection. From 2.7.0-beta3 onwards, calling this method will result in double logging and filters applied will not get applied. If interested in using just logging provider, then please use Microsoft.Extensions.Logging.ApplicationInsightsLoggingBuilderExtensions.AddApplicationInsights from Microsoft.Extensions.Logging.ApplicationInsights package. Read more https://aka.ms/ApplicationInsightsILoggerFaq")]
        [SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope", Justification = "Obsolete method.")]
        public static ILoggerFactory AddApplicationInsights(
            this ILoggerFactory factory,
            IServiceProvider serviceProvider,
            Func<string, LogLevel, bool> filter,
            Action loggerAddedCallback)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            var client = serviceProvider.GetService<TelemetryClient>();
            var debugLoggerControl = serviceProvider.GetService<Microsoft.ApplicationInsights.AspNetCore.Logging.ApplicationInsightsLoggerEvents>();
            var options = serviceProvider.GetService<IOptions<Microsoft.ApplicationInsights.AspNetCore.Logging.ApplicationInsightsLoggerOptions>>();

            if (options == null)
            {
                options = Options.Create(new Microsoft.ApplicationInsights.AspNetCore.Logging.ApplicationInsightsLoggerOptions());
            }

            if (debugLoggerControl != null)
            {
                debugLoggerControl.OnLoggerAdded();

                if (loggerAddedCallback != null)
                {
                    debugLoggerControl.LoggerAdded += loggerAddedCallback;
                }
            }

            factory.AddProvider(new Microsoft.ApplicationInsights.AspNetCore.Logging.ApplicationInsightsLoggerProvider(client, filter, options));
            return factory;
        }
    }
}