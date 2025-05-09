namespace Microsoft.Extensions.DependencyInjection.Test
{
    using System;

    using Microsoft.ApplicationInsights.Extensibility;

    using Microsoft.Extensions.Options;

    internal static class IServiceProviderExtensions
    {
        public static TelemetryConfiguration GetTelemetryConfiguration(this IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<IOptions<TelemetryConfiguration>>().Value;
        }
    }
}