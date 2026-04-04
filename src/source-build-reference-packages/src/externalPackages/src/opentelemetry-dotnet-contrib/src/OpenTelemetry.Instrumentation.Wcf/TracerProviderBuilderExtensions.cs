// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Instrumentation.Wcf;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Trace;

/// <summary>
/// Extension methods to simplify registering of dependency instrumentation.
/// </summary>
public static class TracerProviderBuilderExtensions
{
    /// <summary>
    /// Enables the outgoing requests automatic data collection for WCF.
    /// </summary>
    /// <param name="builder"><see cref="TracerProviderBuilderExtensions"/> being configured.</param>
    /// <returns>The instance of <see cref="TracerProviderBuilderExtensions"/> to chain the calls.</returns>
    public static TracerProviderBuilder AddWcfInstrumentation(this TracerProviderBuilder builder) =>
        AddWcfInstrumentation(builder, configure: null);

    /// <summary>
    /// Enables the outgoing requests automatic data collection for WCF.
    /// </summary>
    /// <param name="builder"><see cref="TracerProviderBuilderExtensions"/> being configured.</param>
    /// <param name="configure">Wcf configuration options.</param>
    /// <returns>The instance of <see cref="TracerProviderBuilderExtensions"/> to chain the calls.</returns>
    public static TracerProviderBuilder AddWcfInstrumentation(this TracerProviderBuilder builder, Action<WcfInstrumentationOptions>? configure)
    {
        Guard.ThrowIfNull(builder);

        if (WcfInstrumentationActivitySource.Options != null)
        {
            throw new NotSupportedException("WCF instrumentation has already been registered and doesn't support multiple registrations.");
        }

        var options = new WcfInstrumentationOptions();
        configure?.Invoke(options);

        WcfInstrumentationActivitySource.Options = options;

#if NETFRAMEWORK
        Instrumentation.Wcf.Implementation.AspNetParentSpanCorrector.Register();
#endif

        return builder.AddSource(WcfInstrumentationActivitySource.ActivitySourceName);
    }
}
