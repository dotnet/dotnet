// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.Remoting.Contexts;
using Microsoft.Extensions.Options;
using OpenTelemetry.Trace;

namespace OpenTelemetry.Instrumentation.Remoting.Implementation;

/// <summary>
/// A <see cref="IContributeDynamicSink"/> implementation that returns an instance of
/// <see cref="TelemetryDynamicSink"/> responsible for instrumenting remoting calls.
/// </summary>
internal sealed class TelemetryDynamicSinkProvider : IDynamicProperty, IContributeDynamicSink, IDisposable
{
    internal const string ActivitySourceName = "OpenTelemetry.Instrumentation.Remoting";
    internal const string DynamicPropertyName = "TelemetryDynamicSinkProvider";
    internal static readonly Version SemanticConventionsVersion = new(1, 40, 0);

    private readonly ActivitySource activitySource = ActivitySourceFactory.Create<TelemetryDynamicSinkProvider>(SemanticConventionsVersion);
    private readonly ConcurrentDictionary<string, string> serviceNameCache = new();
    private readonly IDisposable? optionsChangeRegistration;

    private volatile RemotingInstrumentationOptions options;

    public TelemetryDynamicSinkProvider(IOptionsMonitor<RemotingInstrumentationOptions> options)
    {
        this.options = options.CurrentValue;
        this.optionsChangeRegistration = options.OnChange((updatedOptions, _) => this.options = updatedOptions);
    }

    /// <inheritdoc />
    public string Name => DynamicPropertyName;

    internal RemotingInstrumentationOptions Options => this.options;

    /// <summary>
    /// Creates and returns a <see cref="TelemetryDynamicSink"/> to be used for instrumentation.
    /// </summary>
    /// <returns>A new instance of <see cref="TelemetryDynamicSink"/>.</returns>
    public IDynamicMessageSink GetDynamicSink() => new TelemetryDynamicSink(this, this.activitySource, this.serviceNameCache);

    /// <inheritdoc />
    public void Dispose()
    {
        this.optionsChangeRegistration?.Dispose();
        this.activitySource.Dispose();
    }
}
