// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if !NETFRAMEWORK

using Microsoft.Extensions.Options;

namespace OpenTelemetry.Instrumentation.Http;

/// <summary>
/// Holds at most one <see cref="HttpClientInstrumentation"/> instance per named-options name,
/// preventing duplicate DiagnosticSource subscriptions when
/// <see cref="Trace.HttpClientInstrumentationTracerProviderBuilderExtensions.AddHttpClientInstrumentation(Trace.TracerProviderBuilder)"/>
/// is called multiple times with the same name (e.g., by a distro and user code).
/// </summary>
internal sealed class HttpClientInstrumentationProvider
{
    private readonly IOptionsMonitor<HttpClientTraceInstrumentationOptions> optionsMonitor;
    private readonly Dictionary<string, HttpClientInstrumentation> instances = new(StringComparer.Ordinal);

    public HttpClientInstrumentationProvider(
        IOptionsMonitor<HttpClientTraceInstrumentationOptions> optionsMonitor)
    {
        this.optionsMonitor = optionsMonitor;
    }

    /// <summary>
    /// Returns the existing <see cref="HttpClientInstrumentation"/> for <paramref name="name"/>,
    /// creating and subscribing a new one on first access.
    /// </summary>
    /// <param name="name">The named-options name used when retrieving options.</param>
    /// <returns>The <see cref="HttpClientInstrumentation"/> instance for the given name.</returns>
    public HttpClientInstrumentation GetOrCreate(string name)
    {
        lock (this.instances)
        {
            if (!this.instances.TryGetValue(name, out var instance))
            {
                var options = this.optionsMonitor.Get(name);
                this.instances[name] = instance = new(options);
            }

            return instance;
        }
    }
}
#endif
