// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Microsoft.Extensions.Options;
using OpenTelemetry.Instrumentation.AspNetCore;

namespace OpenTelemetry.Extensions.Enrichment.AspNetCore;

#pragma warning disable CA1812 // Class is instantiated through dependency injection
internal sealed class ConfigureAspNetCoreTraceInstrumentationOptions : IConfigureOptions<AspNetCoreTraceInstrumentationOptions>
#pragma warning restore CA1812 // Class is instantiated through dependency injection
{
    private readonly AspNetCoreTraceEnrichmentProcessor processor;

    public ConfigureAspNetCoreTraceInstrumentationOptions(AspNetCoreTraceEnrichmentProcessor processor)
    {
        this.processor = processor;
    }

    public void Configure(AspNetCoreTraceInstrumentationOptions options)
    {
        options.EnrichWithHttpRequest += this.processor.EnrichWithHttpRequest;

        options.EnrichWithHttpResponse += this.processor.EnrichWithHttpResponse;

        options.EnrichWithException += this.processor.EnrichWithException;
    }
}
