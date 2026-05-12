// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Microsoft.AspNetCore.Http;

namespace OpenTelemetry.Extensions.Enrichment.AspNetCore;

/// <summary>
/// Class for implementing enrichers of incoming requests (ASP.NET Core) for traces.
/// </summary>
public abstract class AspNetCoreTraceEnricher
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AspNetCoreTraceEnricher"/> class.
    /// </summary>
    protected AspNetCoreTraceEnricher()
    {
    }

    /// <summary>
    /// Enriches a trace with additional tags.
    /// </summary>
    /// <param name="bag"><see cref="TraceEnrichmentBag"/> object to be used to add tags to enrich the traces.</param>
    /// <param name="request"><see cref="HttpRequest"/> object from which additional information can be extracted to enrich the trace.</param>
    public virtual void EnrichWithHttpRequest(in TraceEnrichmentBag bag, HttpRequest request)
    {
    }

    /// <summary>
    /// Enriches a trace with additional tags.
    /// </summary>
    /// <param name="bag"><see cref="TraceEnrichmentBag"/> object to be used to add tags to enrich the traces.</param>
    /// <param name="response"><see cref="HttpResponse"/> object from which additional information can be extracted to enrich the trace.</param>
    public virtual void EnrichWithHttpResponse(in TraceEnrichmentBag bag, HttpResponse response)
    {
    }

    /// <summary>
    /// Enriches a trace with additional tags.
    /// </summary>
    /// <param name="bag"><see cref="TraceEnrichmentBag"/> object to be used to add tags to enrich the traces.</param>
    /// <param name="exception"><see cref="Exception"/> object from which additional information can be extracted to enrich the trace.</param>
    public virtual void EnrichWithException(in TraceEnrichmentBag bag, Exception exception)
    {
    }
}
