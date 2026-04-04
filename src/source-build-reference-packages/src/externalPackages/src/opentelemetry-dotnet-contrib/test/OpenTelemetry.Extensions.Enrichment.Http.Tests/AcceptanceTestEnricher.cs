// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NETFRAMEWORK
using System.Net;
#endif

namespace OpenTelemetry.Extensions.Enrichment.Http.Tests;

internal sealed class AcceptanceTestEnricher : HttpClientTraceEnricher
{
    public override void EnrichWithRequest(
        in TraceEnrichmentBag bag,
#if NET
        HttpRequestMessage request) =>
        bag.Add(
            "accept.request.method",
            request.Method.Method);
#else
        HttpWebRequest request) =>
        bag.Add(
            "accept.request.method",
            request.Method);
#endif

    public override void EnrichWithResponse(
        in TraceEnrichmentBag bag,
#if NET
        HttpResponseMessage response) =>
#else
        HttpWebResponse response) =>
#endif
        bag.Add(
            "accept.response.status",
            (int)response.StatusCode);

    public override void EnrichWithException(
        in TraceEnrichmentBag bag,
        Exception exception) =>
        bag.Add(
            "accept.exception",
            exception.GetType().Name);
}
