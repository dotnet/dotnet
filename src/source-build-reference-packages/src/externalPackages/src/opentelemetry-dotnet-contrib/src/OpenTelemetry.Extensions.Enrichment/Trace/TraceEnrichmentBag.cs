// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;

namespace OpenTelemetry.Extensions.Enrichment;

#pragma warning disable CA1815 // Override equals and operator equals on value types
public readonly struct TraceEnrichmentBag
#pragma warning restore CA1815 // Override equals and operator equals on value types
{
    private readonly Activity activity;

    public TraceEnrichmentBag(Activity activity)
    {
        this.activity = activity;
    }

    public void Add(string key, object? value)
    {
        this.activity.AddTag(key, value);
    }
}
