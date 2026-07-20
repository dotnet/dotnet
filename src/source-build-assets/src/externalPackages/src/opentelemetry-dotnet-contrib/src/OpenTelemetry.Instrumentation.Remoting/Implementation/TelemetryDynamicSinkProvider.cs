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
    internal const int MaxCachedServiceNames = 1024;
    internal static readonly Version SemanticConventionsVersion = new(1, 40, 0);

    private readonly ActivitySource activitySource = ActivitySourceFactory.Create<TelemetryDynamicSinkProvider>(SemanticConventionsVersion);
    private readonly ConcurrentDictionary<string, string> serviceNameCache = new();
    private readonly IDisposable? optionsChangeRegistration;
    private int approximateServiceNameCacheCount;

    private volatile RemotingInstrumentationOptions options;

    public TelemetryDynamicSinkProvider(IOptionsMonitor<RemotingInstrumentationOptions> options)
    {
        this.options = options.CurrentValue;
        this.optionsChangeRegistration = options.OnChange((updatedOptions, _) => this.options = updatedOptions);
    }

    /// <inheritdoc />
    public string Name => DynamicPropertyName;

    internal RemotingInstrumentationOptions Options => this.options;

    internal int CachedServiceNameCount => Volatile.Read(ref this.approximateServiceNameCacheCount);

    /// <summary>
    /// Creates and returns a <see cref="TelemetryDynamicSink"/> to be used for instrumentation.
    /// </summary>
    /// <returns>A new instance of <see cref="TelemetryDynamicSink"/>.</returns>
    public IDynamicMessageSink GetDynamicSink() => new TelemetryDynamicSink(this, this.activitySource);

    /// <inheritdoc />
    public void Dispose()
    {
        this.optionsChangeRegistration?.Dispose();
        this.activitySource.Dispose();
    }

    internal static string ExtractServiceName(string assemblyQualifiedTypeName)
    {
        var index = FindAssemblySeparatorIndex(assemblyQualifiedTypeName);
        return index >= 0 ? assemblyQualifiedTypeName.Substring(0, index) : assemblyQualifiedTypeName;
    }

    internal string GetServiceName(string typeName)
    {
        if (this.serviceNameCache.TryGetValue(typeName, out var serviceName))
        {
            return serviceName;
        }

        serviceName = ExtractServiceName(typeName);

        if (Volatile.Read(ref this.approximateServiceNameCacheCount) >= MaxCachedServiceNames)
        {
            return serviceName;
        }

        if (this.serviceNameCache.TryAdd(typeName, serviceName))
        {
            Interlocked.Increment(ref this.approximateServiceNameCacheCount);
            return serviceName;
        }

        return this.serviceNameCache.TryGetValue(typeName, out var existing) ? existing : serviceName;
    }

    private static int FindAssemblySeparatorIndex(string assemblyQualifiedTypeName)
    {
        var depth = 0;

        for (var i = 0; i < assemblyQualifiedTypeName.Length; i++)
        {
            var c = assemblyQualifiedTypeName[i];

            if (c == '[')
            {
                depth++;
            }
            else if (c == ']')
            {
                if (depth > 0)
                {
                    depth--;
                }
            }
            else if (c == ',' && depth == 0)
            {
                return i;
            }
        }

        return -1;
    }
}
