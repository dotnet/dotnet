// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if !NET
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Instrumentation.AspNetCore.Implementation;
#endif
using OpenTelemetry.Internal;

namespace OpenTelemetry.Metrics;

/// <summary>
/// Extension methods to simplify registering of ASP.NET Core request instrumentation.
/// </summary>
public static class AspNetCoreInstrumentationMeterProviderBuilderExtensions
{
    /// <summary>
    /// Enables the incoming requests automatic data collection for ASP.NET Core.
    /// </summary>
    /// <param name="builder"><see cref="MeterProviderBuilder"/> being configured.</param>
    /// <returns>The instance of <see cref="MeterProviderBuilder"/> to chain the calls.</returns>
    public static MeterProviderBuilder AddAspNetCoreInstrumentation(
        this MeterProviderBuilder builder)
    {
        Guard.ThrowIfNull(builder);

#if NET
        return builder.ConfigureMeters();
#else
        // Note: Warm-up the status code and method mapping.
        _ = TelemetryHelper.BoxedStatusCodes;
        _ = TelemetryHelper.RequestDataHelper;

        builder.AddMeter(HttpInMetricsListener.InstrumentationName);

#pragma warning disable CA2000
        builder.AddInstrumentation(new AspNetCoreMetrics());
#pragma warning restore CA2000

        return builder;
#endif
    }

    internal static MeterProviderBuilder ConfigureMeters(this MeterProviderBuilder builder)
    {
        // There is no cost to listen for meters that aren't used. For example, listening for Kestrel meter in an app that doesn't use Kestrel is fine.
        // Listen for all built-in ASP.NET Core meters so metrics automatically light up depending on what an app does.
        var builtInAspNetCoreMeters = new[]
        {
            "Microsoft.AspNetCore.Hosting",
            "Microsoft.AspNetCore.Server.Kestrel",
            "Microsoft.AspNetCore.Http.Connections",
            "Microsoft.AspNetCore.Routing",
            "Microsoft.AspNetCore.Diagnostics",
            "Microsoft.AspNetCore.RateLimiting",
            "Microsoft.AspNetCore.Components",
            "Microsoft.AspNetCore.Components.Server.Circuits",
            "Microsoft.AspNetCore.Components.Lifecycle",
            "Microsoft.AspNetCore.Authorization",
            "Microsoft.AspNetCore.Authentication",
            "Microsoft.AspNetCore.Identity",
            "Microsoft.AspNetCore.MemoryPool",
        };

        return builder.AddMeter(builtInAspNetCoreMeters);
    }
}
