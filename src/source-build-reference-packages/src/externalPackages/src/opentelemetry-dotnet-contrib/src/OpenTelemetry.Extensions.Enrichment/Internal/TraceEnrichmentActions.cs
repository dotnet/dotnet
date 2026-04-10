// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Extensions.Enrichment;

#pragma warning disable CA1812 // Class is instantiated through dependency injection
internal sealed class TraceEnrichmentActions : TraceEnricher
#pragma warning restore CA1812 // Class is instantiated through dependency injection
{
    private readonly Action<TraceEnrichmentBag>[] actions;

    public TraceEnrichmentActions(IEnumerable<Action<TraceEnrichmentBag>> actions)
    {
        this.actions = actions.ToArray();
    }

    public override void Enrich(in TraceEnrichmentBag bag)
    {
        for (var i = 0; i < this.actions.Length; i++)
        {
            this.actions[i].Invoke(bag);
        }
    }
}
