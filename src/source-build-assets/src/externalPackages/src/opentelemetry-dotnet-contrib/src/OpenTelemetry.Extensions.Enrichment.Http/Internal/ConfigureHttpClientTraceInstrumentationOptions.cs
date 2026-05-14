// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Microsoft.Extensions.Options;
using OpenTelemetry.Instrumentation.Http;

namespace OpenTelemetry.Extensions.Enrichment.Http;

#pragma warning disable CA1812 // Class is instantiated through dependency injection
internal sealed class ConfigureHttpClientTraceInstrumentationOptions : IConfigureOptions<HttpClientTraceInstrumentationOptions>
#pragma warning restore CA1812 // Class is instantiated through dependency injection
{
    private readonly HttpClientTraceEnrichmentProcessor processor;

    public ConfigureHttpClientTraceInstrumentationOptions(HttpClientTraceEnrichmentProcessor processor)
    {
        this.processor = processor;
    }

    public void Configure(HttpClientTraceInstrumentationOptions options)
    {
#if NETFRAMEWORK
        options.EnrichWithHttpWebRequest += this.processor.EnrichWithRequest;
        options.EnrichWithHttpWebResponse += this.processor.EnrichWithResponse;
#else
        options.EnrichWithHttpRequestMessage += this.processor.EnrichWithRequest;
        options.EnrichWithHttpResponseMessage += this.processor.EnrichWithResponse;
#endif

        options.EnrichWithException += this.processor.EnrichWithException;
    }
}
