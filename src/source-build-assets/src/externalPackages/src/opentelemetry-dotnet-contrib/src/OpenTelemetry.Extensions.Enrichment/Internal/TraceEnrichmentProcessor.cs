// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;

namespace OpenTelemetry.Extensions.Enrichment;

#pragma warning disable CA1812 // Class is instantiated through dependency injection
internal sealed class TraceEnrichmentProcessor : BaseProcessor<Activity>
#pragma warning restore CA1812 // Class is instantiated through dependency injection
{
    private readonly TraceEnricher[] traceEnrichers;

    public TraceEnrichmentProcessor(IEnumerable<TraceEnricher> traceEnrichers)
    {
        this.traceEnrichers = [.. traceEnrichers];
    }

    public override void OnStart(Activity activity)
    {
        var bag = new TraceEnrichmentBag(activity);

        foreach (var enricher in this.traceEnrichers)
        {
            try
            {
                enricher.EnrichOnActivityStart(bag);
            }
            catch (Exception ex)
            {
                EnrichmentEventSource.Log.TraceEnricherException(nameof(this.OnStart), enricher, ex);
            }
        }
    }

    public override void OnEnd(Activity activity)
    {
        var bag = new TraceEnrichmentBag(activity);

        foreach (var enricher in this.traceEnrichers)
        {
            try
            {
                enricher.Enrich(bag);
            }
            catch (Exception ex)
            {
                EnrichmentEventSource.Log.TraceEnricherException(nameof(this.OnEnd), enricher, ex);
            }
        }
    }
}
