// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Extensions.Enrichment.Tests;

internal class MyTraceEnricher2 : TraceEnricher
{
    public const string Key = nameof(MyTraceEnricher2);

    public int TimesCalled { get; private set; }

    public override void Enrich(in TraceEnrichmentBag bag)
    {
        bag.Add(Key, ++this.TimesCalled);
    }
}
