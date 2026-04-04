// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using Microsoft.Extensions.Configuration;

#if NETFRAMEWORK
using System.Net.Http;

#endif
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry.Instrumentation.Http.Implementation;
using OpenTelemetry.Metrics;
using OpenTelemetry.Tests;
using OpenTelemetry.Trace;
using Xunit;
using Xunit.Abstractions;

namespace OpenTelemetry.Instrumentation.Http.Tests;

public partial class HttpClientTests : IDisposable
{
    private readonly ITestOutputHelper output;
    private readonly IDisposable serverLifeTime;
    private readonly string host;
    private readonly int port;
    private readonly string url;

    public HttpClientTests(ITestOutputHelper output)
    {
        this.output = output;

        this.serverLifeTime = TestHttpServer.RunServer(
            ctx =>
            {
                var traceparent = ctx.Request.Headers["traceparent"];
                var custom_traceparent = ctx.Request.Headers["custom_traceparent"];
                var contextRequired = ctx.Request.Headers["contextRequired"];
                var responseCode = ctx.Request.Headers["responseCode"];
                if ((contextRequired == null || bool.Parse(contextRequired)) && string.IsNullOrWhiteSpace(traceparent) && string.IsNullOrWhiteSpace(custom_traceparent))
                {
                    ctx.Response.StatusCode = 500;
                    ctx.Response.StatusDescription = "Missing trace context";
                }
                else if (ctx.Request.Url != null && ctx.Request.Url.PathAndQuery.Contains("500"))
                {
                    ctx.Response.StatusCode = 500;
                }
                else if (ctx.Request.Url != null && ctx.Request.Url.PathAndQuery.Contains("redirect"))
                {
                    ctx.Response.RedirectLocation = "/";
                    ctx.Response.StatusCode = 302;
                }
                else if (responseCode != null)
                {
                    ctx.Response.StatusCode = int.Parse(responseCode);
                }
                else if (ctx.Request.Url != null && ctx.Request.Url.PathAndQuery.Contains("slow"))
                {
                    Thread.Sleep(10000);
                    ctx.Response.RedirectLocation = "/";
                    ctx.Response.StatusCode = 200;
                }
                else
                {
                    ctx.Response.StatusCode = 200;
                }

                ctx.Response.OutputStream.Close();
            },
            out var host,
            out var port);

        this.host = host;
        this.port = port;
        this.url = $"http://{host}:{port}/";

        this.output.WriteLine($"HttpServer started: {this.url}");
    }

    [Fact]
    public void AddHttpClientInstrumentation_NamedOptions()
    {
        var defaultExporterOptionsConfigureOptionsInvocations = 0;
        var namedExporterOptionsConfigureOptionsInvocations = 0;

        using var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .ConfigureServices(services =>
            {
                services.Configure<HttpClientTraceInstrumentationOptions>(_ => defaultExporterOptionsConfigureOptionsInvocations++);

                services.Configure<HttpClientTraceInstrumentationOptions>("Instrumentation2", _ => namedExporterOptionsConfigureOptionsInvocations++);
            })
            .AddHttpClientInstrumentation()
            .AddHttpClientInstrumentation("Instrumentation2", configureHttpClientTraceInstrumentationOptions: null)
            .Build();

        Assert.Equal(1, defaultExporterOptionsConfigureOptionsInvocations);
        Assert.Equal(1, namedExporterOptionsConfigureOptionsInvocations);
    }

    [Fact]
    public void AddHttpClientInstrumentation_BadArgs()
    {
        TracerProviderBuilder? builder = null;
        Assert.Throws<ArgumentNullException>(() => builder!.AddHttpClientInstrumentation());
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task InjectsHeadersAsync(bool shouldEnrich)
    {
        var exportedItems = new List<Activity>();

        using var request = new HttpRequestMessage
        {
            RequestUri = new Uri(this.url),
            Method = new HttpMethod("GET"),
        };

        using var parent = new Activity("parent")
            .SetIdFormat(ActivityIdFormat.W3C)
            .Start();
        parent.TraceStateString = "k1=v1,k2=v2";
        parent.ActivityTraceFlags = ActivityTraceFlags.Recorded;

        using (Sdk.CreateTracerProviderBuilder()
            .AddHttpClientInstrumentation(o =>
            {
                if (shouldEnrich)
                {
                    o.EnrichWithHttpWebRequest = (activity, _) =>
                    {
                        activity.SetTag("enrichedWithHttpWebRequest", "yes");
                    };

                    o.EnrichWithHttpWebResponse = (activity, _) =>
                    {
                        activity.SetTag("enrichedWithHttpWebResponse", "yes");
                    };

                    o.EnrichWithHttpRequestMessage = (activity, _) =>
                    {
                        activity.SetTag("enrichedWithHttpRequestMessage", "yes");
                    };

                    o.EnrichWithHttpResponseMessage = (activity, _) =>
                    {
                        activity.SetTag("enrichedWithHttpResponseMessage", "yes");
                    };
                }
            })
            .AddInMemoryExporter(exportedItems)
            .Build())
        {
            using var c = new HttpClient();
            await c.SendAsync(request);
        }

        Assert.Single(exportedItems);
        var activity = exportedItems[0];

        Assert.Equal(ActivityKind.Client, activity.Kind);
        Assert.Equal(parent.TraceId, activity.Context.TraceId);
        Assert.Equal(parent.SpanId, activity.ParentSpanId);
        Assert.NotEqual(parent.SpanId, activity.Context.SpanId);
        Assert.NotEqual(default, activity.Context.SpanId);

#if NETFRAMEWORK
        // Note: On .NET Framework a HttpWebRequest is created and enriched
        // not the HttpRequestMessage passed to HttpClient.
        Assert.Empty(request.Headers);
#else
        Assert.True(request.Headers.TryGetValues("traceparent", out var traceparents));
        Assert.True(request.Headers.TryGetValues("tracestate", out var tracestates));
        Assert.Single(traceparents);
        Assert.Single(tracestates);

        Assert.Equal($"00-{activity.Context.TraceId}-{activity.Context.SpanId}-01", traceparents.Single());
        Assert.Equal("k1=v1,k2=v2", tracestates.Single());
#endif

#if NETFRAMEWORK
        if (shouldEnrich)
        {
            Assert.Equal("yes", activity.Tags.FirstOrDefault(tag => tag.Key == "enrichedWithHttpWebRequest").Value);
            Assert.Equal("yes", activity.Tags.FirstOrDefault(tag => tag.Key == "enrichedWithHttpWebResponse").Value);
        }
        else
        {
            Assert.DoesNotContain(activity.Tags, tag => tag.Key == "enrichedWithHttpWebRequest");
            Assert.DoesNotContain(activity.Tags, tag => tag.Key == "enrichedWithHttpWebResponse");
        }

        Assert.DoesNotContain(activity.Tags, tag => tag.Key == "enrichedWithHttpRequestMessage");
        Assert.DoesNotContain(activity.Tags, tag => tag.Key == "enrichedWithHttpResponseMessage");
#else
        Assert.DoesNotContain(activity.Tags, tag => tag.Key == "enrichedWithHttpWebRequest");
        Assert.DoesNotContain(activity.Tags, tag => tag.Key == "enrichedWithHttpWebResponse");

        if (shouldEnrich)
        {
            Assert.Equal("yes", activity.Tags.FirstOrDefault(tag => tag.Key == "enrichedWithHttpRequestMessage").Value);
            Assert.Equal("yes", activity.Tags.FirstOrDefault(tag => tag.Key == "enrichedWithHttpResponseMessage").Value);
        }
        else
        {
            Assert.DoesNotContain(activity.Tags, tag => tag.Key == "enrichedWithHttpRequestMessage");
            Assert.DoesNotContain(activity.Tags, tag => tag.Key == "enrichedWithHttpResponseMessage");
        }
#endif
    }

    [Fact]
    public async Task InjectsHeadersAsync_CustomFormat()
    {
        var propagator = new CustomTextMapPropagator();
        propagator.InjectValues.Add("custom_traceParent", context => $"00/{context.ActivityContext.TraceId}/{context.ActivityContext.SpanId}/01");
        propagator.InjectValues.Add("custom_traceState", context => Activity.Current?.TraceStateString ?? string.Empty);

        var exportedItems = new List<Activity>();

        using var request = new HttpRequestMessage();
        request.RequestUri = new Uri(this.url);
        request.Method = new HttpMethod("GET");

        using var parent = new Activity("parent")
            .SetIdFormat(ActivityIdFormat.W3C)
            .Start();
        parent.TraceStateString = "k1=v1,k2=v2";
        parent.ActivityTraceFlags = ActivityTraceFlags.Recorded;

        Sdk.SetDefaultTextMapPropagator(propagator);

        using (Sdk.CreateTracerProviderBuilder()
            .AddHttpClientInstrumentation()
            .AddInMemoryExporter(exportedItems)
            .Build())
        {
            using var c = new HttpClient();
            await c.SendAsync(request);
        }

        Assert.Single(exportedItems);
        var activity = exportedItems[0];

        Assert.Equal(ActivityKind.Client, activity.Kind);
        Assert.Equal(parent.TraceId, activity.Context.TraceId);
        Assert.Equal(parent.SpanId, activity.ParentSpanId);
        Assert.NotEqual(parent.SpanId, activity.Context.SpanId);
        Assert.NotEqual(default, activity.Context.SpanId);

#if NETFRAMEWORK
        // Note: On .NET Framework a HttpWebRequest is created and enriched
        // not the HttpRequestMessage passed to HttpClient.
        Assert.Empty(request.Headers);
#else
        Assert.True(request.Headers.TryGetValues("custom_traceParent", out var traceParents));
        Assert.True(request.Headers.TryGetValues("custom_traceState", out var traceStates));
        Assert.Single(traceParents);
        Assert.Single(traceStates);

        Assert.Equal($"00/{activity.Context.TraceId}/{activity.Context.SpanId}/01", traceParents.Single());
        Assert.Equal("k1=v1,k2=v2", traceStates.Single());
#endif

        Sdk.SetDefaultTextMapPropagator(new CompositeTextMapPropagator(
        [
            new TraceContextPropagator(),
            new BaggagePropagator(),
        ]));
    }

    [Fact(Skip = "https://github.com/open-telemetry/opentelemetry-dotnet-contrib/issues/1727")]
    public async Task RespectsSuppress()
    {
        try
        {
            var propagator = new CustomTextMapPropagator();
            propagator.InjectValues.Add("custom_traceParent", context => $"00/{context.ActivityContext.TraceId}/{context.ActivityContext.SpanId}/01");
            propagator.InjectValues.Add("custom_traceState", context => Activity.Current?.TraceStateString ?? string.Empty);

            var exportedItems = new List<Activity>();

            using var request = new HttpRequestMessage();
            request.RequestUri = new Uri(this.url);
            request.Method = new HttpMethod("GET");

            using var parent = new Activity("parent")
                .SetIdFormat(ActivityIdFormat.W3C)
                .Start();
            parent.TraceStateString = "k1=v1,k2=v2";
            parent.ActivityTraceFlags = ActivityTraceFlags.Recorded;

            Sdk.SetDefaultTextMapPropagator(propagator);

            using (Sdk.CreateTracerProviderBuilder()
                .AddHttpClientInstrumentation()
                .AddInMemoryExporter(exportedItems)
                .Build())
            {
                using var c = new HttpClient();
                using (SuppressInstrumentationScope.Begin())
                {
                    await c.SendAsync(request);
                }
            }

            // If suppressed, activity is not emitted and
            // propagation is also not performed.
            Assert.Empty(exportedItems);
            Assert.False(request.Headers.Contains("custom_traceParent"));
            Assert.False(request.Headers.Contains("custom_traceState"));
        }
        finally
        {
            Sdk.SetDefaultTextMapPropagator(new CompositeTextMapPropagator(
            [
                new TraceContextPropagator(),
                new BaggagePropagator(),
            ]));
        }
    }

    [Fact]
    public async Task ExportsSpansCreatedForRetries()
    {
        var exportedItems = new List<Activity>();
        using var request = new HttpRequestMessage
        {
            RequestUri = new Uri(this.url),
            Method = new HttpMethod("GET"),
        };

        using var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddHttpClientInstrumentation()
            .AddInMemoryExporter(exportedItems)
            .Build();

        var maxRetries = 3;
        using var clientHandler = new HttpClientHandler();
        using var retryHandler = new RepeatHandler(clientHandler, maxRetries);
        using var httpClient = new HttpClient(retryHandler);
        await httpClient.SendAsync(request);

        // number of exported spans should be 3(maxRetries)
        Assert.Equal(maxRetries, exportedItems.Count);

        var spanid1 = exportedItems[0].SpanId;
        var spanid2 = exportedItems[1].SpanId;
        var spanid3 = exportedItems[2].SpanId;

        // Validate span ids are different
        Assert.NotEqual(spanid1, spanid2);
        Assert.NotEqual(spanid3, spanid1);
        Assert.NotEqual(spanid2, spanid3);
    }

    [Theory]
    [InlineData("CONNECT", "CONNECT", null)]
    [InlineData("DELETE", "DELETE", null)]
    [InlineData("GET", "GET", null)]
    [InlineData("PUT", "PUT", null)]
    [InlineData("HEAD", "HEAD", null)]
    [InlineData("OPTIONS", "OPTIONS", null)]
    [InlineData("PATCH", "PATCH", null)]
    [InlineData("POST", "POST", null)]
    [InlineData("TRACE", "TRACE", null)]
    [InlineData("Delete", "DELETE", "Delete")]
#if NETFRAMEWORK
    [InlineData("Connect", "CONNECT", null)]// HTTP Client converts Connect to its canonical form (Connect). Expected original method is null.
    [InlineData("Get", "GET", null)] // HTTP Client converts Get to its canonical form (GET). Expected original method is null.
    [InlineData("Put", "PUT", null)] // HTTP Client converts Put to its canonical form (PUT). Expected original method is null.
    [InlineData("Head", "HEAD", null)] // HTTP Client converts Head to its canonical form (HEAD). Expected original method is null.
    [InlineData("Post", "POST", null)] // HTTP Client converts Post to its canonical form (POST). Expected original method is null.
#else
    [InlineData("Connect", "CONNECT", "Connect")]
    [InlineData("Get", "GET", "Get")]
    [InlineData("Put", "PUT", "Put")]
    [InlineData("Head", "HEAD", "Head")]
    [InlineData("Post", "POST", "Post")]
#endif
    [InlineData("Options", "OPTIONS", "Options")]
    [InlineData("Patch", "PATCH", "Patch")]
    [InlineData("Trace", "TRACE", "Trace")]
    [InlineData("CUSTOM", "_OTHER", "CUSTOM")]
    public async Task HttpRequestMethodIsSetOnActivityAsPerSpec(string originalMethod, string expectedMethod, string? expectedOriginalMethod)
    {
        var exportedItems = new List<Activity>();
        using var request = new HttpRequestMessage
        {
            RequestUri = new Uri(this.url),
            Method = new HttpMethod(originalMethod),
        };

        using var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddHttpClientInstrumentation()
            .AddInMemoryExporter(exportedItems)
            .Build();

        using var httpClient = new HttpClient();

        try
        {
            await httpClient.SendAsync(request);
        }
        catch
        {
            // ignore error.
        }

        Assert.Single(exportedItems);

        var activity = exportedItems[0];

        if (originalMethod.Equals(expectedMethod, StringComparison.OrdinalIgnoreCase))
        {
            Assert.Equal(expectedMethod, activity.DisplayName);
        }
        else
        {
            Assert.Equal("HTTP", activity.DisplayName);
        }

        Assert.Equal(expectedMethod, activity.GetTagValue(SemanticConventions.AttributeHttpRequestMethod));

#if NET9_0_OR_GREATER
        if (expectedOriginalMethod is not null and not "CUSTOM")
        {
            // HACK: THIS IS A HACK TO MAKE THE TEST PASS.
            // TODO: THIS CAN BE REMOVED AFTER RUNTIME PATCHES NET9.
            // Currently Runtime is not following the OTel Spec for Http Spans: https://github.com/open-telemetry/semantic-conventions/blob/main/docs/http/http-spans.md#http-client
            // Currently "http.request.method_original" is not being set as expected.
            // Tracking issue: https://github.com/dotnet/runtime/issues/109847
            expectedOriginalMethod = null;
        }
#endif
        Assert.Equal(expectedOriginalMethod, activity.GetTagValue(SemanticConventions.AttributeHttpRequestMethodOriginal));
    }

    [Theory]
    [InlineData("CONNECT", "CONNECT")]
    [InlineData("DELETE", "DELETE")]
    [InlineData("GET", "GET")]
    [InlineData("PUT", "PUT")]
    [InlineData("HEAD", "HEAD")]
    [InlineData("OPTIONS", "OPTIONS")]
    [InlineData("PATCH", "PATCH")]
    [InlineData("Get", "GET")]
    [InlineData("POST", "POST")]
    [InlineData("TRACE", "TRACE")]
    [InlineData("Trace", "TRACE")]
    [InlineData("CUSTOM", "_OTHER")]
    public async Task HttpRequestMethodIsSetonRequestDurationMetricAsPerSpec(string originalMethod, string expectedMethod)
    {
        var metricItems = new List<Metric>();
        using var request = new HttpRequestMessage
        {
            RequestUri = new Uri(this.url),
            Method = new HttpMethod(originalMethod),
        };

        using var meterProvider = Sdk.CreateMeterProviderBuilder()
            .AddHttpClientInstrumentation()
            .AddInMemoryExporter(metricItems)
            .Build();

        using var httpClient = new HttpClient();

        try
        {
            await httpClient.SendAsync(request);
        }
        catch
        {
            // ignore error.
        }

        meterProvider.Dispose();

        var metric = metricItems.FirstOrDefault(m => m.Name == "http.client.request.duration");

        Assert.NotNull(metric);

        var metricPoints = new List<MetricPoint>();
        foreach (var p in metric.GetMetricPoints())
        {
            metricPoints.Add(p);
        }

        Assert.Single(metricPoints);
        var mp = metricPoints[0];

        // Inspect Metric Attributes
        var attributes = new Dictionary<string, object?>();
        foreach (var tag in mp.Tags)
        {
            attributes[tag.Key] = tag.Value;
        }

        Assert.Contains(attributes, kvp => kvp.Key == SemanticConventions.AttributeHttpRequestMethod && kvp.Value?.ToString() == expectedMethod);

        Assert.DoesNotContain(attributes, t => t.Key == SemanticConventions.AttributeHttpRequestMethodOriginal);
    }

    [Fact]
    public async Task RedirectTest()
    {
        var exportedItems = new List<Activity>();
        using (Sdk.CreateTracerProviderBuilder()
            .AddHttpClientInstrumentation()
            .AddInMemoryExporter(exportedItems)
            .Build())
        {
            using var c = new HttpClient();
            await c.GetAsync(new Uri($"{this.url}redirect"));
        }

#if NETFRAMEWORK
        // Note: HttpWebRequest automatically handles redirects and reuses
        // the same instance which is patched reflectively. There isn't a
        // good way to produce two spans when redirecting that we have
        // found. For now, this is not supported.

        Assert.Single(exportedItems);
        Assert.Contains(exportedItems[0].TagObjects, t => t.Key == "http.response.status_code" && (int?)t.Value == 200);
#else
        Assert.Equal(2, exportedItems.Count);
        Assert.Contains(exportedItems[0].TagObjects, t => t.Key == "http.response.status_code" && (int?)t.Value == 302);
        Assert.Contains(exportedItems[1].TagObjects, t => t.Key == "http.response.status_code" && (int?)t.Value == 200);
#endif
    }

    [Fact]
    public async Task RequestNotCollectedWhenInstrumentationFilterApplied()
    {
        var exportedItems = new List<Activity>();

        var httpWebRequestFilterApplied = false;
        var httpRequestMessageFilterApplied = false;

        using (Sdk.CreateTracerProviderBuilder()
            .AddHttpClientInstrumentation(
                opt =>
                {
                    opt.FilterHttpWebRequest = req =>
                    {
                        httpWebRequestFilterApplied = true;
                        return !req.RequestUri.OriginalString.Contains(this.url);
                    };
                    opt.FilterHttpRequestMessage = req =>
                    {
                        httpRequestMessageFilterApplied = true;
                        return req.RequestUri != null && !req.RequestUri.OriginalString.Contains(this.url);
                    };
                })
            .AddInMemoryExporter(exportedItems)
            .Build())
        {
            using var c = new HttpClient();
            await c.GetAsync(new Uri(this.url));
        }

#if NETFRAMEWORK
        Assert.True(httpWebRequestFilterApplied);
        Assert.False(httpRequestMessageFilterApplied);
#else
        Assert.False(httpWebRequestFilterApplied);
        Assert.True(httpRequestMessageFilterApplied);
#endif

        Assert.Empty(exportedItems);
    }

    [Fact]
    public async Task RequestNotCollectedWhenInstrumentationFilterThrowsException()
    {
        var exportedItems = new List<Activity>();

        using (Sdk.CreateTracerProviderBuilder()
            .AddHttpClientInstrumentation(
                opt =>
                {
                    opt.FilterHttpWebRequest = _ => throw new Exception("From InstrumentationFilter");
                    opt.FilterHttpRequestMessage = _ => throw new Exception("From InstrumentationFilter");
                })
            .AddInMemoryExporter(exportedItems)
            .Build())
        {
            using var c = new HttpClient();
            using var inMemoryEventListener = new InMemoryEventListener(HttpInstrumentationEventSource.Log);
            await c.GetAsync(new Uri(this.url));
            Assert.Single(inMemoryEventListener.Events, e => e.EventId == 4);
        }

        Assert.Empty(exportedItems);
    }

    [Fact]
    public async Task ReportsExceptionEventForNetworkFailuresWithGetAsync()
    {
        var exportedItems = new List<Activity>();
        var exceptionThrown = false;

        using var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddHttpClientInstrumentation(o => o.RecordException = true)
            .AddInMemoryExporter(exportedItems)
            .Build();

        using var c = new HttpClient();
        try
        {
            await c.GetAsync(new Uri("https://sdlfaldfjalkdfjlkajdflkajlsdjf.sdlkjafsdjfalfadslkf.com/"));
        }
        catch
        {
            exceptionThrown = true;
        }

        // Exception is thrown and collected as event
        Assert.True(exceptionThrown);

#if NET10_0_OR_GREATER
        // .NET 10 reports its own activity, but does not set the status description
        Assert.Contains(exportedItems[0].Events, evt => evt.Name.Equals("exception"));
#else
        Assert.Single(exportedItems[0].Events, evt => evt.Name.Equals("exception"));
#endif
    }

    [Fact]
    public async Task DoesNotReportExceptionEventOnErrorResponseWithGetAsync()
    {
        var exportedItems = new List<Activity>();
        var exceptionThrown = false;

        using var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddHttpClientInstrumentation(o => o.RecordException = true)
            .AddInMemoryExporter(exportedItems)
            .Build();

        using var c = new HttpClient();
        try
        {
            await c.GetAsync(new Uri($"{this.url}500"));
        }
        catch
        {
            exceptionThrown = true;
        }

        // Exception is not thrown and not collected as event
        Assert.False(exceptionThrown);
        Assert.Empty(exportedItems[0].Events);
    }

    [Fact]
    public async Task DoesNotReportExceptionEventOnErrorResponseWithGetStringAsync()
    {
        var exportedItems = new List<Activity>();
        var exceptionThrown = false;
        using var request = new HttpRequestMessage
        {
            RequestUri = new Uri($"{this.url}500"),
            Method = new HttpMethod("GET"),
        };

        using var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddHttpClientInstrumentation(o => o.RecordException = true)
            .AddInMemoryExporter(exportedItems)
            .Build();

        using var c = new HttpClient();
        try
        {
            await c.GetStringAsync(new Uri($"{this.url}500"));
        }
        catch
        {
            exceptionThrown = true;
        }

        // Exception is thrown and not collected as event
        Assert.True(exceptionThrown);
        Assert.Empty(exportedItems[0].Events);
    }

    [Theory]
    [InlineData("?a", "?a", false)]
    [InlineData("?a=bdjdjh", "?a=Redacted", false)]
    [InlineData("?a=b&", "?a=Redacted&", false)]
    [InlineData("?c=b&", "?c=Redacted&", false)]
    [InlineData("?c=a", "?c=Redacted", false)]
    [InlineData("?a=b&c", "?a=Redacted&c", false)]
    [InlineData("?a=b&c=1123456&", "?a=Redacted&c=Redacted&", false)]
    [InlineData("?a=b&c=1&a1", "?a=Redacted&c=Redacted&a1", false)]
    [InlineData("?a=ghgjgj&c=1deedd&a1=", "?a=Redacted&c=Redacted&a1=Redacted", false)]
    [InlineData("?a=b&c=11&a1=&", "?a=Redacted&c=Redacted&a1=Redacted&", false)]
    [InlineData("?c&c&c&", "?c&c&c&", false)]
    [InlineData("?a&a&a&a", "?a&a&a&a", false)]
    [InlineData("?&&&&&&&", "?&&&&&&&", false)]
    [InlineData("?c", "?c", false)]
    [InlineData("?a", "?a", true)]
    [InlineData("?a=bdfdfdf", "?a=bdfdfdf", true)]
    [InlineData("?a=b&", "?a=b&", true)]
    [InlineData("?c=b&", "?c=b&", true)]
    [InlineData("?c=a", "?c=a", true)]
    [InlineData("?a=b&c", "?a=b&c", true)]
    [InlineData("?a=b&c=111111&", "?a=b&c=111111&", true)]
    [InlineData("?a=b&c=1&a1", "?a=b&c=1&a1", true)]
    [InlineData("?a=b&c=1&a1=", "?a=b&c=1&a1=", true)]
    [InlineData("?a=b123&c=11&a1=&", "?a=b123&c=11&a1=&", true)]
    [InlineData("?c&c&c&", "?c&c&c&", true)]
    [InlineData("?a&a&a&a", "?a&a&a&a", true)]
    [InlineData("?&&&&&&&", "?&&&&&&&", true)]
    [InlineData("?c", "?c", true)]
    [InlineData("?c=%26&", "?c=Redacted&", false)]
    public async Task ValidateUrlQueryRedaction(string urlQuery, string expectedUrlQuery, bool disableQueryRedaction)
    {
        var exportedItems = new List<Activity>();

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?> { ["OTEL_DOTNET_EXPERIMENTAL_HTTPCLIENT_DISABLE_URL_QUERY_REDACTION"] = disableQueryRedaction.ToString() })
            .Build();

        // Arrange
        using var traceprovider = Sdk.CreateTracerProviderBuilder()
            .ConfigureServices(services => services.AddSingleton<IConfiguration>(configuration))
            .AddHttpClientInstrumentation()
            .AddInMemoryExporter(exportedItems)
            .Build();

        using var c = new HttpClient();
        try
        {
            await c.GetStringAsync(new Uri($"{this.url}path{urlQuery}"));
        }
        catch
        {
        }

        Assert.Single(exportedItems);
        var activity = exportedItems[0];

        var expectedUrl = $"{this.url}path{expectedUrlQuery}";

#if NET9_0_OR_GREATER
        // In .NET 9+ URIs are redacted by default. We could disable it with the
        // System.Net.Http.DisableUriRedaction=true AppContext switch, but as that
        // is process-wide it affects other tests. Instead, we adjust our expectations
        // here. For more information see: https://github.com/dotnet/docs/issues/42792
        expectedUrl = $"{this.url}path?*";
#endif

        Assert.Equal(expectedUrl, activity.GetTagValue(SemanticConventions.AttributeUrlFull));
    }

    [Theory]
    [InlineData(true, true)]
    [InlineData(true, false)]
    [InlineData(false, true)]
    [InlineData(false, false)]
    public async Task CustomPropagatorCalled(bool sample, bool createParentActivity)
    {
#if NETFRAMEWORK
        ActivityContext parentContext = default;
#endif
        ActivityContext contextFromPropagator = default;

        var propagator = new CustomTextMapPropagator
        {
            Injected = context => contextFromPropagator = context.ActivityContext,
        };
        propagator.InjectValues.Add("custom_traceParent", context => $"00/{context.ActivityContext.TraceId}/{context.ActivityContext.SpanId}/01");
        propagator.InjectValues.Add("custom_traceState", context => Activity.Current?.TraceStateString ?? string.Empty);

        var exportedItems = new List<Activity>();

        using (var tracerProvider = Sdk.CreateTracerProviderBuilder()
           .AddHttpClientInstrumentation()
           .AddInMemoryExporter(exportedItems)
           .SetSampler(sample ? new ParentBasedSampler(new AlwaysOnSampler()) : new AlwaysOffSampler())
           .Build())
        {
            var previousDefaultTextMapPropagator = Propagators.DefaultTextMapPropagator;
            Sdk.SetDefaultTextMapPropagator(propagator);

            Activity? parent = null;
            if (createParentActivity)
            {
                parent = new Activity("parent")
                    .SetIdFormat(ActivityIdFormat.W3C)
                    .Start();

                parent.TraceStateString = "k1=v1,k2=v2";
                parent.ActivityTraceFlags = ActivityTraceFlags.Recorded;

#if NETFRAMEWORK
                parentContext = parent.Context;
#endif
            }

            using var request = new HttpRequestMessage();
            request.RequestUri = new Uri(this.url);
            request.Method = new HttpMethod("GET");

            using var c = new HttpClient();
            await c.SendAsync(request);

            parent?.Stop();

            Sdk.SetDefaultTextMapPropagator(previousDefaultTextMapPropagator);
        }

        if (!sample)
        {
            Assert.Empty(exportedItems);
        }
        else
        {
            Assert.Single(exportedItems);
        }

        // Make sure custom propagator was called.
        Assert.True(contextFromPropagator != default);
        if (sample)
        {
            Assert.Equal(contextFromPropagator, exportedItems[0].Context);
        }

#if NETFRAMEWORK
        if (!sample && createParentActivity)
        {
            Assert.Equal(parentContext.TraceId, contextFromPropagator.TraceId);
            Assert.Equal(parentContext.SpanId, contextFromPropagator.SpanId);
        }
#endif
    }

    [Theory]
    [InlineData(ActivityStatusCode.Unset)]
    [InlineData(ActivityStatusCode.Ok)]
    [InlineData(ActivityStatusCode.Error)]
    public async Task ResponseEnrichmentCanChangeActivityStatus(ActivityStatusCode activityStatus)
    {
        bool httpWebResponseEnrichmentApplied = false;
        bool httpResponseMessageEnrichmentApplied = false;

        var exportedItems = new List<Activity>();

        using var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddInMemoryExporter(exportedItems)
            .AddHttpClientInstrumentation(
                options =>
                {
                    options.EnrichWithHttpWebResponse = (activity, _) =>
                    {
                        httpWebResponseEnrichmentApplied = true;
                        activity.SetStatus(activityStatus);
                    };
                    options.EnrichWithHttpResponseMessage = (activity, _) =>
                    {
                        httpResponseMessageEnrichmentApplied = true;
                        activity.SetStatus(activityStatus);
                    };
                })
            .Build();
        {
            using var c = new HttpClient();
            await c.GetAsync(new Uri(this.url));
        }

#if NETFRAMEWORK
        Assert.True(httpWebResponseEnrichmentApplied);
        Assert.False(httpResponseMessageEnrichmentApplied);
#else
        Assert.False(httpWebResponseEnrichmentApplied);
        Assert.True(httpResponseMessageEnrichmentApplied);
#endif

        var item = Assert.Single(exportedItems);
        Assert.Equal(activityStatus, item.Status);
    }

    public void Dispose()
    {
        this.serverLifeTime?.Dispose();
        this.output.WriteLine($"HttpServer stopped: {this.url}");
        Activity.Current = null;
        GC.SuppressFinalize(this);
    }
}
