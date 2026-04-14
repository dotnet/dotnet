// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Instrumentation;

internal sealed class InstrumentationHandleManager
{
    private int metricHandles;
    private int tracingHandles;

    /// <summary>
    /// Gets the current count of active metric handles.
    /// </summary>
    public int MetricHandles => this.metricHandles;

    /// <summary>
    /// Gets the current count of active tracing handles.
    /// </summary>
    public int TracingHandles => this.tracingHandles;

    /// <summary>
    /// Adds a metric handle and returns an IDisposable to decrement the count when disposed.
    /// </summary>
    /// <returns>An IDisposable object.</returns>
    public IDisposable AddMetricHandle() => new MetricHandle(this);

    /// <summary>
    /// Adds a tracing handle and returns an IDisposable to decrement the count when disposed.
    /// </summary>
    /// <returns>An IDisposable object.</returns>
    public IDisposable AddTracingHandle() => new TracingHandle(this);

    private sealed class MetricHandle : IDisposable
    {
        private readonly InstrumentationHandleManager manager;
        private bool disposed;

        public MetricHandle(InstrumentationHandleManager manager)
        {
            this.manager = manager;
            Interlocked.Increment(ref this.manager.metricHandles);
        }

        public void Dispose()
        {
            if (!this.disposed)
            {
                Interlocked.Decrement(ref this.manager.metricHandles);
                this.disposed = true;
            }
        }
    }

    private sealed class TracingHandle : IDisposable
    {
        private readonly InstrumentationHandleManager manager;
        private bool disposed;

        public TracingHandle(InstrumentationHandleManager manager)
        {
            this.manager = manager;
            Interlocked.Increment(ref this.manager.tracingHandles);
        }

        public void Dispose()
        {
            if (!this.disposed)
            {
                Interlocked.Decrement(ref this.manager.tracingHandles);
                this.disposed = true;
            }
        }
    }
}
