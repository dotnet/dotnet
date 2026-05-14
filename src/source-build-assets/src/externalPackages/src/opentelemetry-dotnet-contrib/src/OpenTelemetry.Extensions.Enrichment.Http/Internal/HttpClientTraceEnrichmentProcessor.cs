// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
#if NETFRAMEWORK
using System.Net;
#endif

namespace OpenTelemetry.Extensions.Enrichment.Http;

internal sealed class HttpClientTraceEnrichmentProcessor
{
    private readonly HttpClientTraceEnricher[] enrichers;

    public HttpClientTraceEnrichmentProcessor(IEnumerable<HttpClientTraceEnricher> enrichers)
    {
        this.enrichers = [.. enrichers];
    }

#if NETFRAMEWORK
    public void EnrichWithRequest(Activity activity, HttpWebRequest request)
#else
    public void EnrichWithRequest(Activity activity, HttpRequestMessage request)
#endif
    {
        var propertyBag = new TraceEnrichmentBag(activity);

        foreach (var enricher in this.enrichers)
        {
            enricher.EnrichWithRequest(in propertyBag, request);
        }
    }

#if NETFRAMEWORK
    public void EnrichWithResponse(Activity activity, HttpWebResponse response)
#else
    public void EnrichWithResponse(Activity activity, HttpResponseMessage response)
#endif
    {
        var propertyBag = new TraceEnrichmentBag(activity);

        foreach (var enricher in this.enrichers)
        {
            enricher.EnrichWithResponse(in propertyBag, response);
        }
    }

    public void EnrichWithException(Activity activity, Exception exception)
    {
        var propertyBag = new TraceEnrichmentBag(activity);

        foreach (var enricher in this.enrichers)
        {
            enricher.EnrichWithException(in propertyBag, exception);
        }
    }
}
