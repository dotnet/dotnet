// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;

namespace OpenTelemetry.Extensions.Enrichment;

/// <summary>
/// Interface for implementing enrichers for traces.
/// </summary>
public abstract class TraceEnricher
{
    protected TraceEnricher()
    {
    }

    /// <summary>
    /// Enrich trace with additional tags when an <see cref="Activity"/> is stopped.
    /// </summary>
    /// <param name="bag"><see cref="TraceEnrichmentBag"/> object to be used to add the required tags to enrich the traces.</param>
    public abstract void Enrich(in TraceEnrichmentBag bag);

    /// <summary>
    /// Enrich trace with additional tags when an <see cref="Activity"/> is started.
    /// </summary>
    /// <param name="bag"><see cref="TraceEnrichmentBag"/> object to be used to add the required tags to enrich the traces.</param>
    public virtual void EnrichOnActivityStart(in TraceEnrichmentBag bag)
    {
        // default implementation: noop
    }
}
