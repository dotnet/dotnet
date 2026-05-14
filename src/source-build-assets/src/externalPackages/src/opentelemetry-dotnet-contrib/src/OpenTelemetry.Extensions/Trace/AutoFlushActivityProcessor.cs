// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;

namespace OpenTelemetry.Trace;

/// <summary>
/// Activity processor that flushes its containing <see cref="TracerProvider"/> if an ended
/// Activity matches a predicate.
/// </summary>
/// <remarks>
/// Add this processor *after* exporter related Activity processors.
/// </remarks>
internal sealed class AutoFlushActivityProcessor : BaseProcessor<Activity>
{
    private readonly Func<Activity, bool> predicate;
    private readonly int timeoutMilliseconds;

    private TracerProvider? tracerProvider;
    private bool canForceFlush = true;

    /// <summary>
    /// Initializes a new instance of the <see cref="AutoFlushActivityProcessor"/> class.
    /// </summary>
    /// <param name="predicate">Predicate that should return <c>true</c> to initiate a flush.</param>
    /// <param name="timeoutMilliseconds">Timeout (in milliseconds) to use for flushing. Specify <see cref="Timeout.Infinite"/>
    /// to wait indefinitely.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the <c>timeoutMilliseconds</c> is smaller than -1.
    /// </exception>
    /// <remarks>
    /// It's assumed that the predicate is defined as a lambda expression which is executed quite fast and
    /// doesn't contain more complex code. The predicate must not create new Activity instances,
    /// otherwise the behavior is undefined. Any exception thrown by the predicate will be swallowed and logged.
    /// In case of an exception the predicate is treated as false which means flush will not be applied.
    /// </remarks>
    internal AutoFlushActivityProcessor(Func<Activity, bool> predicate, int timeoutMilliseconds)
    {
        this.predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
#if NET
        ArgumentOutOfRangeException.ThrowIfLessThan(timeoutMilliseconds, Timeout.Infinite);
#else
        if (timeoutMilliseconds < Timeout.Infinite)
        {
            throw new ArgumentOutOfRangeException(nameof(timeoutMilliseconds));
        }
#endif

        this.timeoutMilliseconds = timeoutMilliseconds;
    }

    /// <inheritdoc/>
    public override void OnEnd(Activity data)
    {
        if (!this.canForceFlush)
        {
            return;
        }

        if (this.tracerProvider == null && this.ParentProvider != null)
        {
            this.tracerProvider = this.ParentProvider as TracerProvider;
            this.canForceFlush = this.tracerProvider != null;
            if (!this.canForceFlush)
            {
                return;
            }
        }
        else if (this.ParentProvider == null)
        {
            return;
        }

        var shouldFlush = this.RunPredicate(data);
        if (shouldFlush)
        {
            this.tracerProvider!.ForceFlush(this.timeoutMilliseconds);
        }
    }

    private bool RunPredicate(Activity data)
    {
        var shouldFlush = false;
        try
        {
            shouldFlush = this.predicate(data);
        }
#pragma warning disable CA1031 // Do not catch general exception types
        catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
        {
            OpenTelemetryExtensionsEventSource.Log.LogProcessorException($"Flushing predicate threw an exception. Flush of {typeof(TracerProvider)} was skipped.", ex);
        }

        return shouldFlush;
    }
}
