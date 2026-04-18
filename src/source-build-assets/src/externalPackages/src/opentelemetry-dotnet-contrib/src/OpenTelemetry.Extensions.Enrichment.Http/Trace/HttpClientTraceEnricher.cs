// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NETFRAMEWORK
using System.Net;
#endif

namespace OpenTelemetry.Extensions.Enrichment.Http;

/// <summary>
/// Class for implementing enrichers of outgoing requests for traces.
/// </summary>
public abstract class HttpClientTraceEnricher
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HttpClientTraceEnricher"/> class.
    /// </summary>
    protected HttpClientTraceEnricher()
    {
    }

#if NETFRAMEWORK
    /// <summary>
    /// Enriches a trace with additional tags.
    /// </summary>
    /// <param name="bag"><see cref="TraceEnrichmentBag"/> object to be used to add tags to enrich the traces.</param>
    /// <param name="request"><see cref="HttpWebRequest"/> object from which additional information can be extracted to enrich the trace.</param>
    public virtual void EnrichWithRequest(in TraceEnrichmentBag bag, HttpWebRequest request)
#else
    /// <summary>
    /// Enriches a trace with additional tags.
    /// </summary>
    /// <param name="bag"><see cref="TraceEnrichmentBag"/> object to be used to add tags to enrich the traces.</param>
    /// <param name="request"><see cref="HttpRequestMessage"/> object from which additional information can be extracted to enrich the trace.</param>
    public virtual void EnrichWithRequest(in TraceEnrichmentBag bag, HttpRequestMessage request)
#endif
    {
    }

#if NETFRAMEWORK
    /// <summary>
    /// Enriches a trace with additional tags.
    /// </summary>
    /// <param name="bag"><see cref="TraceEnrichmentBag"/> object to be used to add tags to enrich the traces.</param>
    /// <param name="response"><see cref="HttpWebResponse"/> object from which additional information can be extracted to enrich the trace.</param>
    public virtual void EnrichWithResponse(in TraceEnrichmentBag bag, HttpWebResponse response)
#else
    /// <summary>
    /// Enriches a trace with additional tags.
    /// </summary>
    /// <param name="bag"><see cref="TraceEnrichmentBag"/> object to be used to add tags to enrich the traces.</param>
    /// <param name="response"><see cref="HttpResponseMessage"/> object from which additional information can be extracted to enrich the trace.</param>
    public virtual void EnrichWithResponse(in TraceEnrichmentBag bag, HttpResponseMessage response)
#endif
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
