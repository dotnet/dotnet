// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Amazon.Runtime.Telemetry.Metrics;

namespace OpenTelemetry.Instrumentation.AWS.Implementation.Metrics;

internal sealed class AWSMeter : Meter
{
    private readonly System.Diagnostics.Metrics.Meter meter;
    private int disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="AWSMeter"/> class.
    /// </summary>
    /// <param name="meter">The Meter used for creating and tracking the Instruments.</param>
    public AWSMeter(System.Diagnostics.Metrics.Meter meter)
    {
        this.meter = meter ?? throw new ArgumentNullException(nameof(meter));
    }

    public override UpDownCounter<T> CreateUpDownCounter<T>(
        string name,
        string? units = null,
        string? description = null)
        where T : struct
        => new AWSUpDownCounter<T>(this.meter, name, this.IsDisposed, units, description);

    public override MonotonicCounter<T> CreateMonotonicCounter<T>(
        string name,
        string? units = null,
        string? description = null)
        where T : struct
        => new AWSMonotonicCounter<T>(this.meter, name, this.IsDisposed, units, description);

    public override Histogram<T> CreateHistogram<T>(
        string name,
        string? units = null,
        string? description = null)
        where T : struct
        => new AWSHistogram<T>(this.meter, name, this.IsDisposed, units, description);

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Interlocked.Exchange(ref this.disposed, 1);
        }

        base.Dispose(disposing);
    }

    private bool IsDisposed()
        => Volatile.Read(ref this.disposed) != 0;
}
