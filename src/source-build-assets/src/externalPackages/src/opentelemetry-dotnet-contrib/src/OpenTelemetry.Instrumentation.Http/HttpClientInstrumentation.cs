// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Instrumentation.Http.Implementation;

namespace OpenTelemetry.Instrumentation.Http;

/// <summary>
/// HttpClient instrumentation.
/// </summary>
internal sealed class HttpClientInstrumentation : IDisposable
{
    private static readonly HashSet<string> ExcludedDiagnosticSourceEventsNet7OrGreater =
    [
        "System.Net.Http.Request",
        "System.Net.Http.Response",
        "System.Net.Http.HttpRequestOut"
    ];

    private static readonly HashSet<string> ExcludedDiagnosticSourceEvents =
    [
        "System.Net.Http.Request",
        "System.Net.Http.Response"
    ];

    private readonly DiagnosticSourceSubscriber diagnosticSourceSubscriber;

    private readonly Func<string, object?, object?, bool> isEnabled = static (eventName, _, _)
        => !ExcludedDiagnosticSourceEvents.Contains(eventName);

    private readonly Func<string, object?, object?, bool> isEnabledNet7OrGreater = static (eventName, _, _)
        => !ExcludedDiagnosticSourceEventsNet7OrGreater.Contains(eventName);

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpClientInstrumentation"/> class.
    /// </summary>
    /// <param name="options">Configuration options for HTTP client instrumentation.</param>
    public HttpClientInstrumentation(HttpClientTraceInstrumentationOptions options)
    {
        // For .NET 7+ activity will be created using ActivitySource.
        // https://github.com/dotnet/runtime/blob/main/src/libraries/System.Net.Http/src/System/Net/Http/DiagnosticsHandler.cs
        // However, in case when activity creation returns null (due to sampling)
        // the framework will fall back to creating an activity anyway due to active diagnostic source listener.
        // To prevent this, isEnabled is implemented which will return false always
        // so that the sampler's decision is respected.
        this.diagnosticSourceSubscriber = new(
            new HttpHandlerDiagnosticListener(options),
            HttpHandlerDiagnosticListener.IsNet7OrGreater ? this.isEnabledNet7OrGreater : this.isEnabled,
            HttpInstrumentationEventSource.Log.UnknownErrorProcessingEvent);

        this.diagnosticSourceSubscriber.Subscribe();
    }

    /// <inheritdoc/>
    public void Dispose()
        => this.diagnosticSourceSubscriber?.Dispose();
}
