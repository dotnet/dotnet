// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;

namespace OpenTelemetry.Trace;

/// <summary>
/// Activity processor that adds <see cref="Baggage"/> fields to every new activity.
/// </summary>
public sealed class BaggageActivityProcessor : BaseProcessor<Activity>
{
    private readonly Predicate<string> baggageKeyPredicate;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaggageActivityProcessor"/> class.
    /// </summary>
    /// <param name="baggageKeyPredicate">Predicate to determine which baggage keys should be added to the activity.</param>
    internal BaggageActivityProcessor(Predicate<string> baggageKeyPredicate)
    {
        this.baggageKeyPredicate = baggageKeyPredicate ?? throw new ArgumentNullException(nameof(baggageKeyPredicate));
    }

    /// <summary>
    /// Gets a baggage key predicate that returns <c>true</c> for all baggage keys.
    /// </summary>
    public static Predicate<string> AllowAllBaggageKeys => (_) => true;

    /// <inheritdoc />
    public override void OnStart(Activity data)
    {
        foreach (var entry in Baggage.Current)
        {
            if (this.baggageKeyPredicate(entry.Key))
            {
                data?.SetTag(entry.Key, entry.Value);
            }
        }
    }
}
