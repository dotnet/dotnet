namespace Microsoft.Extensions.DependencyInjection
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.ApplicationInsights.AspNetCore;
    using Microsoft.ApplicationInsights.AspNetCore.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.AspNetCore.Extensions;
    using Microsoft.ApplicationInsights.AspNetCore.TelemetryInitializers;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.EventCounterCollector;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/> that allow adding Application Insights services to application.
    /// </summary>
    public static partial class ApplicationInsightsExtensions
    {
#pragma warning disable CS1591 // Missing XML comment.
        [SuppressMessage(category: "StyleCop Documentation Rules", checkId: "SA1600:ElementsMustBeDocumented", Justification = "Obsolete method.")]
        [Obsolete("This middleware is no longer needed. Enable Request monitoring using services.AddApplicationInsights")]
        public static IApplicationBuilder UseApplicationInsightsRequestTelemetry(this IApplicationBuilder app)
        {
            return app;
        }

        [SuppressMessage(category: "StyleCop Documentation Rules", checkId: "SA1600:ElementsMustBeDocumented", Justification = "Obsolete method.")]
        [Obsolete("This middleware is no longer needed to track exceptions as they are automatically tracked by RequestTrackingTelemetryModule")]
        public static IApplicationBuilder UseApplicationInsightsExceptionTelemetry(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionTrackingMiddleware>();
        }
#pragma warning restore CS1591 // Missing XML comment.

        /// <summary>
        /// Adds Application Insights services into service collection.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> instance.</param>
        /// <param name="instrumentationKey">Instrumentation key to use for telemetry.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        [Obsolete("InstrumentationKey based global ingestion is being deprecated. Use the AddApplicationInsightsTelemetry() overload which accepts Action<ApplicationInsightsServiceOptions> and set ApplicationInsightsServiceOptions.ConnectionString. See https://github.com/microsoft/ApplicationInsights-dotnet/issues/2560 for more details.")]
        public static IServiceCollection AddApplicationInsightsTelemetry(
            this IServiceCollection services,
            string instrumentationKey)
        {
            services.AddApplicationInsightsTelemetry(options => options.InstrumentationKey = instrumentationKey);
            return services;
        }

        /// <summary>
        /// Adds Application Insights services into service collection.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> instance.</param>
        /// <param name="configuration">Configuration to use for sending telemetry.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddApplicationInsightsTelemetry(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddApplicationInsightsTelemetry(options => AddTelemetryConfiguration(configuration, options));
            return services;
        }

        /// <summary>
        /// Adds Application Insights services into service collection.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> instance.</param>
        /// <param name="options">The action used to configure the options.</param>
        /// <returns>
        /// The <see cref="IServiceCollection"/>.
        /// </returns>
        public static IServiceCollection AddApplicationInsightsTelemetry(
            this IServiceCollection services,
            Action<ApplicationInsightsServiceOptions> options)
        {
            services.AddApplicationInsightsTelemetry();
            services.Configure(options);
            return services;
        }

        /// <summary>
        /// Adds Application Insights services into service collection.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> instance.</param>
        /// <param name="options">The options instance used to configure with.</param>
        /// <returns>
        /// The <see cref="IServiceCollection"/>.
        /// </returns>
        public static IServiceCollection AddApplicationInsightsTelemetry(
            this IServiceCollection services,
            ApplicationInsightsServiceOptions options)
        {
            services.AddApplicationInsightsTelemetry();
            services.Configure((ApplicationInsightsServiceOptions o) => options.CopyPropertiesTo(o));
            return services;
        }

        /// <summary>
        /// Adds Application Insights services into service collection.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> instance.</param>
        /// <returns>
        /// The <see cref="IServiceCollection"/>.
        /// </returns>
        public static IServiceCollection AddApplicationInsightsTelemetry(this IServiceCollection services)
        {
            try
            {
                if (!IsApplicationInsightsAdded(services))
                {
                    services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
                    AddAspNetCoreWebTelemetryInitializers(services);
                    AddCommonInitializers(services);

                    // Request Tracking.
                    services.AddSingleton<ITelemetryModule, RequestTrackingTelemetryModule>();

                    services.ConfigureTelemetryModule<RequestTrackingTelemetryModule>((module, options) =>
                    {
                        if (options.EnableRequestTrackingTelemetryModule)
                        {
                            module.CollectionOptions = options.RequestCollectionOptions;
                        }
                    });

                    AddCommonTelemetryModules(services);
                    AddTelemetryChannel(services);

                    services.TryAddSingleton<IConfigureOptions<ApplicationInsightsServiceOptions>,
                            DefaultApplicationInsightsServiceConfigureOptions>();

                    AddTelemetryConfigAndClient(services);
                    AddDefaultApplicationIdProvider(services);

                    // Using startup filter instead of starting DiagnosticListeners directly because
                    // AspNetCoreHostingDiagnosticListener injects TelemetryClient that injects TelemetryConfiguration
                    // that requires IOptions infrastructure to run and initialize
                    services.AddSingleton<IStartupFilter, ApplicationInsightsStartupFilter>();
                    services.AddSingleton<IJavaScriptSnippet, JavaScriptSnippet>();

                    // Add 'JavaScriptSnippet' "Service" for backwards compatibility. To remove in favour of 'IJavaScriptSnippet'.
                    services.AddSingleton<JavaScriptSnippet>();

                    // NetStandard2.0 has a package reference to Microsoft.Extensions.Logging.ApplicationInsights, and
                    // enables ApplicationInsightsLoggerProvider by default.
                    AddApplicationInsightsLoggerProvider(services);
                }

                return services;
            }
            catch (Exception e)
            {
                AspNetCoreEventSource.Instance.LogError(e.ToInvariantString());
                return services;
            }
        }

        private static void AddAspNetCoreWebTelemetryInitializers(IServiceCollection services)
        {
            services.AddSingleton<ITelemetryInitializer, AzureAppServiceRoleNameFromHostNameHeaderInitializer>();
            services.AddSingleton<ITelemetryInitializer, ClientIpHeaderTelemetryInitializer>();
            services.AddSingleton<ITelemetryInitializer, OperationNameTelemetryInitializer>();
            services.AddSingleton<ITelemetryInitializer, SyntheticTelemetryInitializer>();
            services.AddSingleton<ITelemetryInitializer, WebSessionTelemetryInitializer>();
            services.AddSingleton<ITelemetryInitializer, WebUserTelemetryInitializer>();
            services.AddSingleton<ITelemetryInitializer, AspNetCoreEnvironmentTelemetryInitializer>();
        }
    }
}
