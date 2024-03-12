namespace Microsoft.AspNetCore.Hosting
{
    using System;

    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Extension methods for <see cref="IWebHostBuilder"/> that allow adding Application Insights services to application.
    /// </summary>
    public static class ApplicationInsightsWebHostBuilderExtensions
    {
        /// <summary>
        /// Configures <see cref="IWebHostBuilder"/> to use Application Insights services.
        /// </summary>
        /// <param name="webHostBuilder">The <see cref="IWebHostBuilder"/> instance.</param>
        /// <returns>The <see cref="IWebHostBuilder"/>.</returns>
        [Obsolete("This method is deprecated in favor of AddApplicationInsightsTelemetry() extension method on IServiceCollection.")]
        public static IWebHostBuilder UseApplicationInsights(this IWebHostBuilder webHostBuilder)
        {
            if (webHostBuilder == null)
            {
                throw new ArgumentNullException(nameof(webHostBuilder));
            }

            webHostBuilder.ConfigureServices(collection =>
            {
                collection.AddApplicationInsightsTelemetry();
            });

            return webHostBuilder;
        }

        /// <summary>
        /// Configures <see cref="IWebHostBuilder"/> to use Application Insights services.
        /// </summary>
        /// <param name="webHostBuilder">The <see cref="IWebHostBuilder"/> instance.</param>
        /// <param name="instrumentationKey">Instrumentation key to use for telemetry.</param>
        /// <returns>The <see cref="IWebHostBuilder"/>.</returns>
        [Obsolete("This method is deprecated in favor of AddApplicationInsightsTelemetry(string instrumentationKey) extension method on IServiceCollection.")]
        public static IWebHostBuilder UseApplicationInsights(this IWebHostBuilder webHostBuilder, string instrumentationKey)
        {
            if (webHostBuilder == null)
            {
                throw new ArgumentNullException(nameof(webHostBuilder));
            }

            webHostBuilder.ConfigureServices(collection => collection.AddApplicationInsightsTelemetry(instrumentationKey));
            return webHostBuilder;
        }
    }
}
