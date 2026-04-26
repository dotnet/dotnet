// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Trace;

/// <summary>
/// Extension methods to simplify registering of auto flush Activity processor.
/// </summary>
public static class TracerProviderBuilderExtensions
{
    /// <summary>
    /// Adds auto flush Activity processor.
    /// </summary>
    /// <param name="builder"><see cref="TracerProviderBuilder"/> being configured.</param>
    /// <param name="predicate">Predicate that should return <c>true</c> to initiate a flush.</param>
    /// <param name="timeoutMilliseconds">Timeout (in milliseconds) to use for flushing. Specify <see cref="Timeout.Infinite"/>
    /// to wait indefinitely.
    /// </param>
    /// <exception cref="ArgumentNullException">Thrown when the <c>builder</c> is null.</exception>
    /// <returns>The instance of <see cref="TracerProviderBuilder"/> to chain the calls.</returns>
    /// <remarks>
    /// Add this processor *after* exporter related Activity processors.
    /// It's assumed that the predicate is defined as a lambda expression which is executed quite fast and
    /// doesn't contain more complex code. The predicate must not create new Activity instances,
    /// otherwise the behavior is undefined. Any exception thrown by the predicate will be swallowed and logged.
    /// In case of an exception the predicate is treated as false which means flush will not be applied.
    /// </remarks>
    public static TracerProviderBuilder AddAutoFlushActivityProcessor(
        this TracerProviderBuilder builder,
        Func<Activity, bool> predicate,
        int timeoutMilliseconds = 10000)
    {
#if NET
        ArgumentNullException.ThrowIfNull(builder);
#else
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }
#endif

#pragma warning disable CA2000 // Dispose objects before losing scope
        return builder.AddProcessor(new AutoFlushActivityProcessor(predicate, timeoutMilliseconds));
#pragma warning restore CA2000 // Dispose objects before losing scope
    }

    /// <summary>
    /// Adds the <see cref="BaggageActivityProcessor"/> to the <see cref="TracerProviderBuilder"/>.
    /// </summary>
    /// <param name="builder"><see cref="TracerProviderBuilder"/> to add the <see cref="BaggageActivityProcessor"/> to.</param>
    /// <param name="baggageKeyPredicate">Predicate to determine which baggage keys should be added to the activity.</param>
    /// <returns>The instance of <see cref="TracerProviderBuilder"/> to chain the calls.</returns>
    public static TracerProviderBuilder AddBaggageActivityProcessor(
        this TracerProviderBuilder builder,
        Predicate<string> baggageKeyPredicate)
    {
        Guard.ThrowIfNull(builder);
        Guard.ThrowIfNull(baggageKeyPredicate);

        return builder.AddProcessor(b => new BaggageActivityProcessor(baggageKey =>
        {
            try
            {
                return baggageKeyPredicate(baggageKey);
            }
            catch (Exception exception)
            {
                OpenTelemetryExtensionsEventSource.Log.BaggageKeyActivityPredicateException(baggageKey, exception.Message);
                return false;
            }
        }));
    }
}
