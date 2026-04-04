// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Extensions.Enrichment;

namespace Examples.Enrichment;

internal sealed class MyTraceEnricher : TraceEnricher
{
    private readonly IMyService myService;

    public MyTraceEnricher(IMyService myService)
    {
        this.myService = myService;
    }

    public override void Enrich(in TraceEnrichmentBag bag)
    {
        var (service, status) = this.myService.MyDailyStatus();

        bag.Add(service, status);
    }
}
