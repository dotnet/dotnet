// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Microsoft.AspNetCore.Http;

namespace OpenTelemetry.Extensions.Enrichment.AspNetCore.Tests;

internal sealed class MyAspNetCoreTraceEnricher2 : AspNetCoreTraceEnricher
{
    public const string Key = nameof(MyAspNetCoreTraceEnricher2);

    public int RequestCalls { get; private set; }

    public int ResponseCalls { get; private set; }

    public int ExceptionCalls { get; private set; }

    public override void EnrichWithHttpRequest(
        in TraceEnrichmentBag enrichmentBag,
        HttpRequest request) => enrichmentBag.Add(Key + ".request", ++this.RequestCalls);

    public override void EnrichWithHttpResponse(
        in TraceEnrichmentBag enrichmentBag,
        HttpResponse response) => enrichmentBag.Add(Key + ".response", ++this.ResponseCalls);

    public override void EnrichWithException(
        in TraceEnrichmentBag enrichmentBag,
        Exception exception) => enrichmentBag.Add(Key + ".exception", ++this.ExceptionCalls);
}
