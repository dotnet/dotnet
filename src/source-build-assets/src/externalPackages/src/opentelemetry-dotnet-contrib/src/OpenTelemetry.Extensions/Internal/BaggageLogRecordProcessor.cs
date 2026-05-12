// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Logs;

namespace OpenTelemetry.Extensions.Internal;

internal sealed class BaggageLogRecordProcessor : BaseProcessor<LogRecord>
{
    private readonly Predicate<string> baggageKeyPredicate;

    public BaggageLogRecordProcessor(Predicate<string> baggageKeyPredicate)
    {
        this.baggageKeyPredicate = baggageKeyPredicate ?? throw new ArgumentNullException(nameof(baggageKeyPredicate));
    }

    public static Predicate<string> AllowAllBaggageKeys => (_) => true;

    public override void OnEnd(LogRecord data)
    {
        var baggage = Baggage.Current;

        if (data != null && baggage.Count > 0)
        {
            var capacity = (data.Attributes?.Count ?? 0) + baggage.Count;
            var attributes = new List<KeyValuePair<string, object?>>(capacity);

            foreach (var entry in baggage)
            {
                if (this.baggageKeyPredicate(entry.Key))
                {
                    attributes.Add(new(entry.Key, entry.Value));
                }
            }

            if (data.Attributes != null)
            {
                attributes.AddRange(data.Attributes);
            }

            data.Attributes = attributes;
        }

        base.OnEnd(data!);
    }
}
