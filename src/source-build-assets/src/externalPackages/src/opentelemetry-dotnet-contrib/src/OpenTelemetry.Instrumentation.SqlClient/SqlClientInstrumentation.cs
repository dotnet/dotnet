// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Instrumentation.SqlClient.Implementation;

namespace OpenTelemetry.Instrumentation.SqlClient;

/// <summary>
/// SqlClient instrumentation.
/// </summary>
internal sealed class SqlClientInstrumentation : IDisposable
{
    internal const string SqlClientDiagnosticListenerName = "SqlClientDiagnosticListener";

    internal static readonly SqlClientInstrumentation Instance = new();

    internal readonly InstrumentationHandleManager HandleManager = new();

#if !NETFRAMEWORK
    private static readonly HashSet<string> DiagnosticSourceEvents =
    [
        "System.Data.SqlClient.WriteCommandBefore",
        "Microsoft.Data.SqlClient.WriteCommandBefore",
        "System.Data.SqlClient.WriteCommandAfter",
        "Microsoft.Data.SqlClient.WriteCommandAfter",
        "System.Data.SqlClient.WriteCommandError",
        "Microsoft.Data.SqlClient.WriteCommandError"
    ];
#endif

    private readonly Lock tracingOptionsSync = new();
    private readonly List<SqlClientTraceInstrumentationOptions> activeTracingOptions = [];
#if NETFRAMEWORK
    private readonly SqlEventSourceListener sqlEventSourceListener;
#else
    private readonly Func<string, object?, object?, bool> isEnabled = static (eventName, _, _)
        => DiagnosticSourceEvents.Contains(eventName);

    private readonly DiagnosticSourceSubscriber diagnosticSourceSubscriber;
#endif
    private SqlClientTraceInstrumentationOptions tracingOptions = CreateTracingOptionsSnapshot([]);

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlClientInstrumentation"/> class.
    /// </summary>
    private SqlClientInstrumentation()
    {
#if NETFRAMEWORK
        this.sqlEventSourceListener = new SqlEventSourceListener();
#else
        this.diagnosticSourceSubscriber = new DiagnosticSourceSubscriber(
           name => new SqlClientDiagnosticListener(name),
           listener => listener.Name == SqlClientDiagnosticListenerName,
           this.isEnabled,
           SqlClientInstrumentationEventSource.Log.UnknownErrorProcessingEvent);
        this.diagnosticSourceSubscriber.Subscribe();
#endif
    }

    void IDisposable.Dispose() =>
#if NETFRAMEWORK
        this.sqlEventSourceListener?.Dispose();
#else
        this.diagnosticSourceSubscriber?.Dispose();
#endif

    internal SqlClientTraceInstrumentationOptions GetTracingOptions() => Volatile.Read(ref this.tracingOptions);

    internal IDisposable AddTracingHandle(SqlClientTraceInstrumentationOptions options)
    {
        lock (this.tracingOptionsSync)
        {
            this.activeTracingOptions.Add(options);
            Volatile.Write(ref this.tracingOptions, CreateTracingOptionsSnapshot(this.activeTracingOptions));
        }

        return new TracingHandle(this, this.HandleManager.AddTracingHandle(), options);
    }

    private static SqlClientTraceInstrumentationOptions CreateTracingOptionsSnapshot(
        List<SqlClientTraceInstrumentationOptions> activeTracingOptions)
    {
        var snapshot = new SqlClientTraceInstrumentationOptions();

#if !NETFRAMEWORK
        snapshot.RecordException = false;
        snapshot.SetDbQueryParameters = false;
#endif
#if NET
        snapshot.EnableTraceContextPropagation = false;
#endif

        if (activeTracingOptions.Count == 0)
        {
            return snapshot;
        }

        var firstActiveTracingOption = activeTracingOptions[0];

#if !NETFRAMEWORK
        var filters = new List<Func<object, bool>>();
        var enrichWithSqlCommand = firstActiveTracingOption.EnrichWithSqlCommand;

        snapshot.RecordException = firstActiveTracingOption.RecordException;
        snapshot.SetDbQueryParameters = firstActiveTracingOption.SetDbQueryParameters;
#endif
#if NET
        snapshot.EnableTraceContextPropagation = firstActiveTracingOption.EnableTraceContextPropagation;
#endif

        for (var i = 0; i < activeTracingOptions.Count; i++)
        {
            var options = activeTracingOptions[i];

#if !NETFRAMEWORK
            if (options.Filter != null)
            {
                filters.Add(options.Filter);
            }

            if (!Equals(enrichWithSqlCommand, options.EnrichWithSqlCommand))
            {
                enrichWithSqlCommand = null;
            }

            snapshot.RecordException &= options.RecordException;
            snapshot.SetDbQueryParameters &= options.SetDbQueryParameters;
#endif
#if NET
            snapshot.EnableTraceContextPropagation &= options.EnableTraceContextPropagation;
#endif
        }

#if !NETFRAMEWORK
        switch (filters.Count)
        {
            case 0:
                snapshot.Filter = null;
                break;

            case 1:
                snapshot.Filter = filters[0];
                break;

            default:
                snapshot.Filter = command =>
                {
                    foreach (var filter in filters)
                    {
                        if (!filter(command))
                        {
                            return false;
                        }
                    }

                    return true;
                };
                break;
        }

        snapshot.EnrichWithSqlCommand = enrichWithSqlCommand;
#endif

        return snapshot;
    }

    private void RemoveTracingHandle(SqlClientTraceInstrumentationOptions options)
    {
        lock (this.tracingOptionsSync)
        {
            _ = this.activeTracingOptions.Remove(options);
            Volatile.Write(ref this.tracingOptions, CreateTracingOptionsSnapshot(this.activeTracingOptions));
        }
    }

    private sealed class TracingHandle : IDisposable
    {
        private readonly SqlClientInstrumentation instrumentation;
        private readonly IDisposable handle;
        private readonly SqlClientTraceInstrumentationOptions options;
        private bool disposed;

        public TracingHandle(
            SqlClientInstrumentation instrumentation,
            IDisposable handle,
            SqlClientTraceInstrumentationOptions options)
        {
            this.instrumentation = instrumentation;
            this.handle = handle;
            this.options = options;
        }

        public void Dispose()
        {
            if (!this.disposed)
            {
                this.instrumentation.RemoveTracingHandle(this.options);
                this.handle.Dispose();
                this.disposed = true;
            }
        }
    }
}
