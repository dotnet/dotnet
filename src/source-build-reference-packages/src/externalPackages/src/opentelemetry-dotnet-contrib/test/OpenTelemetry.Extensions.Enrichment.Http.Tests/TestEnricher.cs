// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NETFRAMEWORK
using System.Net;
#endif

namespace OpenTelemetry.Extensions.Enrichment.Http.Tests;

internal sealed class TestEnricher : HttpClientTraceEnricher
{
    public int RequestCount { get; private set; }

    public int ResponseCount { get; private set; }

    public int ExceptionCount { get; private set; }

#if NET
    public override void EnrichWithRequest(in TraceEnrichmentBag bag, HttpRequestMessage request)
    {
        this.RequestCount++;
        bag.Add("test.request", "ok");
    }

    public override void EnrichWithResponse(in TraceEnrichmentBag bag, HttpResponseMessage response)
    {
        this.ResponseCount++;
        bag.Add("test.response", (int)response.StatusCode);
    }
#else
    public override void EnrichWithRequest(in TraceEnrichmentBag bag, HttpWebRequest request)
    {
        this.RequestCount++;
        bag.Add("test.request", "ok");
    }

    public override void EnrichWithResponse(in TraceEnrichmentBag bag, HttpWebResponse response)
    {
        this.ResponseCount++;
        bag.Add("test.response", (int)response.StatusCode);
    }
#endif

    public override void EnrichWithException(in TraceEnrichmentBag bag, Exception exception)
    {
        this.ExceptionCount++;
        bag.Add("test.exception", exception.GetType().Name);
    }
}
