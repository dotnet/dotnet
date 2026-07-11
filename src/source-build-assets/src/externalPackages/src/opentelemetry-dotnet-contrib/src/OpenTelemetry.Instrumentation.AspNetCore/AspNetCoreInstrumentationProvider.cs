// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Microsoft.Extensions.Options;

namespace OpenTelemetry.Instrumentation.AspNetCore;

/// <summary>
/// Holds at most one <see cref="AspNetCoreInstrumentation"/> instance per named-options name,
/// preventing duplicate DiagnosticSource subscriptions when
/// <see cref="Trace.AspNetCoreInstrumentationTracerProviderBuilderExtensions.AddAspNetCoreInstrumentation(Trace.TracerProviderBuilder)"/>
/// is called multiple times with the same name (e.g., by a distro and user code).
/// </summary>
internal sealed class AspNetCoreInstrumentationProvider
{
    private readonly IOptionsMonitor<AspNetCoreTraceInstrumentationOptions> optionsMonitor;
    private readonly Dictionary<string, AspNetCoreInstrumentation> instances = new(StringComparer.Ordinal);

    public AspNetCoreInstrumentationProvider(
        IOptionsMonitor<AspNetCoreTraceInstrumentationOptions> optionsMonitor)
    {
        this.optionsMonitor = optionsMonitor;
    }

    /// <summary>
    /// Returns the existing <see cref="AspNetCoreInstrumentation"/> for <paramref name="name"/>,
    /// creating and subscribing a new one on first access.
    /// </summary>
    /// <param name="name">The named-options name used when retrieving options.</param>
    /// <returns>The <see cref="AspNetCoreInstrumentation"/> instance for the given name.</returns>
    public AspNetCoreInstrumentation GetOrCreate(string name)
    {
        lock (this.instances)
        {
            if (!this.instances.TryGetValue(name, out var instance))
            {
                var options = this.optionsMonitor.Get(name);
                this.instances[name] = instance = new(new(options));
            }

            return instance;
        }
    }
}
