// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.Reflection;
using System.Web;
using System.Web.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry.Instrumentation.AspNet.Implementation;
using OpenTelemetry.Tests;
using OpenTelemetry.Trace;
using Xunit;

namespace OpenTelemetry.Instrumentation.AspNet.Tests;

public class HttpInListenerTests
{
    public enum QueryRedactionDisableBehavior
    {
#pragma warning disable SA1602 // Enumeration items should be documented
        DisableViaEnvVar,
        DisableViaIConfiguration,
#pragma warning restore SA1602 // Enumeration items should be documented
    }

    [Theory]
    [InlineData("http://localhost/", "http", "/", null, null, "localhost", 80, "GET", "GET", null, 0, null, "GET", "GET")]
    [InlineData("http://localhost/?foo=bar&baz=test", "http", "/", "foo=bar&baz=test", QueryRedactionDisableBehavior.DisableViaEnvVar, "localhost", 80, "POST", "POST", null, 0, null, "POST", "POST", true)]
    [InlineData("http://localhost/?foo=bar&baz=test", "http", "/", "foo=bar&baz=test", QueryRedactionDisableBehavior.DisableViaIConfiguration, "localhost", 80, "POST", "POST", null, 0, null, "POST", "POST", true)]
    [InlineData("http://localhost/?foo=bar&baz=test", "http", "/", "foo=Redacted&baz=Redacted", null, "localhost", 80, "POST", "POST", null, 0, null, "POST", "POST", true)]
    [InlineData("https://localhost/", "https", "/", null, null, "localhost", 443, "NonStandard", "_OTHER", "NonStandard", 0, null, "HTTP", "HTTP")]
    [InlineData("https://user:pass@localhost/", "https", "/", null, null, "localhost", 443, "GeT", "GET", "GeT", 0, null, "GET", "GET")] // Test URL sanitization
    [InlineData("http://localhost:443/", "http", "/", null, null, "localhost", 443, "GET", "GET", null, 0, null, "GET", "GET")] // Test http over 443
    [InlineData("https://localhost:80/", "https", "/", null, null, "localhost", 80, "GET", "GET", null, 0, null, "GET", "GET")] // Test https over 80
    [InlineData("https://localhost:80/Home/Index.htm?q1=v1&q2=v2#FragmentName", "https", "/Home/Index.htm", "q1=v1&q2=v2", QueryRedactionDisableBehavior.DisableViaEnvVar, "localhost", 80, "GET", "GET", null, 0, null, "GET", "GET")] // Test complex URL
    [InlineData("https://localhost:80/Home/Index.htm?q1=v1&q2=v2#FragmentName", "https", "/Home/Index.htm", "q1=Redacted&q2=Redacted", null, "localhost", 80, "GET", "GET", null, 0, null, "GET", "GET")] // Test complex URL
    [InlineData("https://user:password@localhost:80/Home/Index.htm?q1=v1&q2=v2#FragmentName", "https", "/Home/Index.htm", "q1=v1&q2=v2", QueryRedactionDisableBehavior.DisableViaIConfiguration, "localhost", 80, "GET", "GET", null, 0, null, "GET", "GET")] // Test complex URL sanitization
    [InlineData("http://localhost:80/", "http", "/", null, null, "localhost", 80, "GET", "GET", null, 1, "{controller}/{action}/{id}", "GET", "GET Home/Index")]
    [InlineData("http://localhost:80/Home", "http", "/Home", null, null, "localhost", 80, "GET", "GET", null, 1, "{controller}/{action}/{id}", "GET", "GET Home/Index")]
    [InlineData("http://localhost:80/Weather/Today/10", "http", "/Weather/Today/10", null, null, "localhost", 80, "GET", "GET", null, 1, "{controller}/{action}/{id}", "GET", "GET Weather/Today/{id}")]
    [InlineData("https://localhost:443/about_attr_route/10", "https", "/about_attr_route/10", null, null, "localhost", 443, "HEAD", "HEAD", null, 2, "about_attr_route/{customerId}", "HEAD", "HEAD about_attr_route/{customerId}")]
    [InlineData("http://localhost:1880/api/WeatherForecast", "http", "/api/WeatherForecast", null, null, "localhost", 1880, "GET", "GET", null, 3, "api/{controller}/{id}", "GET", "GET api/WeatherForecast")]
    [InlineData("https://localhost:1843/subroute/10", "https", "/subroute/10", null, null, "localhost", 1843, "GET", "GET", null, 4, "subroute/{customerId}", "GET", "GET subroute/{customerId}")]
    [InlineData("https://localhost:1843/subroute/10", "https", "/subroute/10", null, null, "localhost", 1843, "GET", "GET", null, 5, "subroute/{customerId}", "GET", "GET subroute/{customerId}")]
    [InlineData("http://localhost/api/value", "http", "/api/value", null, null, "localhost", 80, "GET", "GET", null, 0, null, "GET", "GET", false, "/api/value")] // Request will be filtered
    [InlineData("http://localhost/api/value", "http", "/api/value", null, null, "localhost", 80, "GET", "GET", null, 0, null, "GET", "GET", false, "{ThrowException}")] // Filter user code will throw an exception
    [InlineData("http://localhost/", "http", "/", null, null, "localhost", 80, "GET", "GET", null, 0, null, "GET", "GET", false, null, true, "System.InvalidOperationException")] // Test RecordException option
    public void AspNetRequestsAreCollectedSuccessfully(
        string url,
        string expectedUrlScheme,
        string expectedUrlPath,
        string? expectedUrlQuery,
        QueryRedactionDisableBehavior? disableQueryRedaction,
        string expectedHost,
        int expectedPort,
        string requestMethod,
        string expectedRequestMethod,
        string? expectedOriginalRequestMethod,
        int routeType,
        string? routeTemplate,
        string expectedNameAfterStart,
        string expectedNameAfterStop,
        bool setStatusToErrorInEnrich = false,
        string? filter = null,
        bool recordException = false,
        string? expectedErrorType = null)
    {
        using (EnvironmentVariableScope.Create("OTEL_DOTNET_EXPERIMENTAL_ASPNET_DISABLE_URL_QUERY_REDACTION", disableQueryRedaction == QueryRedactionDisableBehavior.DisableViaEnvVar ? "true" : null))
        {
            HttpContext.Current = RouteTestHelper.BuildHttpContext(url, routeType, routeTemplate, requestMethod);

            typeof(HttpRequest).GetField("_wr", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(HttpContext.Current.Request, new TestHttpWorkerRequest());

            var exportedItems = new List<Activity>(16);

            Sdk.SetDefaultTextMapPropagator(new TraceContextPropagator());
            using (Sdk.CreateTracerProviderBuilder()
                .ConfigureServices(services =>
                {
                    if (disableQueryRedaction == QueryRedactionDisableBehavior.DisableViaIConfiguration)
                    {
                        var config = new ConfigurationBuilder()
                            .AddInMemoryCollection(new Dictionary<string, string?>()
                            {
                                ["OTEL_DOTNET_EXPERIMENTAL_ASPNET_DISABLE_URL_QUERY_REDACTION"] = "true",
                            })
                            .Build();

                        services.AddSingleton<IConfiguration>(config);
                    }
                })
                .AddAspNetInstrumentation((options) =>
                {
                    options.Filter = httpContext =>
                    {
                        Assert.True(Activity.Current!.IsAllDataRequested);
                        if (string.IsNullOrEmpty(filter))
                        {
                            return true;
                        }

                        if (filter == "{ThrowException}")
                        {
                            throw new InvalidOperationException();
                        }

                        return httpContext.Request.Path != filter;
                    };

                    options.EnrichWithHttpRequest = (activity, request) =>
                    {
                        Assert.NotNull(activity);
                        Assert.NotNull(request);

                        Assert.True(activity.IsAllDataRequested);
                    };

                    options.EnrichWithHttpResponse = (activity, response) =>
                    {
                        Assert.NotNull(activity);
                        Assert.NotNull(response);

                        Assert.True(activity.IsAllDataRequested);

                        if (setStatusToErrorInEnrich)
                        {
                            activity.SetStatus(ActivityStatusCode.Error);
                        }
                    };

                    options.EnrichWithException = (activity, exception) =>
                    {
                        Assert.NotNull(activity);
                        Assert.NotNull(exception);

                        Assert.True(activity.IsAllDataRequested);
                    };

                    options.RecordException = recordException;
                })
                .AddInMemoryExporter(exportedItems)
                .SetSampler(
                    new TestSampler(SamplingDecision.RecordAndSample)
                    {
                        ExpectedUrlPath = expectedUrlPath,
                    })
                .Build())
            {
                using var inMemoryEventListener = new InMemoryEventListener(AspNetInstrumentationEventSource.Log);

                var activity = ActivityHelper.StartAspNetActivity(Propagators.DefaultTextMapPropagator, new HttpContextWrapper(HttpContext.Current), TelemetryHttpModule.Options.OnRequestStartedCallback);

                if (filter == "{ThrowException}")
                {
                    Assert.Single(inMemoryEventListener.Events, e => e.EventId == 2);
                }

                Assert.Equal(expectedNameAfterStart, Activity.Current!.DisplayName);

                if (recordException)
                {
                    ActivityHelper.WriteActivityException(activity, new HttpContextWrapper(HttpContext.Current), new InvalidOperationException(), TelemetryHttpModule.Options.OnExceptionCallback);
                }

                ActivityHelper.StopAspNetActivity(Propagators.DefaultTextMapPropagator, activity, new HttpContextWrapper(HttpContext.Current), TelemetryHttpModule.Options.OnRequestStoppedCallback);
            }

            if (HttpContext.Current.Request.Path == filter || filter == "{ThrowException}")
            {
                Assert.Empty(exportedItems);
                return;
            }

            Assert.Single(exportedItems);

            var span = exportedItems[0];

            Assert.NotEqual(TimeSpan.Zero, span.Duration);

            Assert.Equal(expectedNameAfterStop, span.DisplayName);
            Assert.Equal(ActivityKind.Server, span.Kind);
            Assert.True(span.Duration != TimeSpan.Zero);

            Assert.Equal(200, span.GetTagValue("http.response.status_code"));

            Assert.Equal(expectedHost, span.GetTagValue("server.address"));
            Assert.Equal(expectedPort, span.GetTagValue("server.port"));

            Assert.Equal(expectedRequestMethod, span.GetTagValue("http.request.method"));
            Assert.Equal(expectedOriginalRequestMethod, span.GetTagValue("http.request.method_original"));
            Assert.Equal("FakeHTTP/123", span.GetTagValue("network.protocol.version"));

            Assert.Equal(expectedUrlPath, span.GetTagValue("url.path"));
            Assert.Equal(expectedUrlQuery, span.GetTagValue("url.query"));
            Assert.Equal(expectedUrlScheme, span.GetTagValue("url.scheme"));
            Assert.Equal("Custom User Agent v1.2.3", span.GetTagValue("user_agent.original"));
            Assert.Equal(expectedErrorType, span.GetTagValue("error.type"));

            if (recordException)
            {
                var status = span.Status;
                Assert.Equal(ActivityStatusCode.Error, span.Status);
                Assert.Equal("Operation is not valid due to the current state of the object.", span.StatusDescription);
            }
            else if (setStatusToErrorInEnrich)
            {
                // This validates that users can override the
                // status in Enrich.
                Assert.Equal(ActivityStatusCode.Error, span.Status);

                // Instrumentation is not expected to set status description
                // as the reason can be inferred from SemanticConventions.AttributeHttpStatusCode
                Assert.True(string.IsNullOrEmpty(span.StatusDescription));
            }
            else
            {
                Assert.Equal(ActivityStatusCode.Unset, span.Status);

                // Instrumentation is not expected to set status description
                // as the reason can be inferred from SemanticConventions.AttributeHttpStatusCode
                Assert.True(string.IsNullOrEmpty(span.StatusDescription));
            }
        }
    }

    [Theory]
    [InlineData(SamplingDecision.Drop)]
    [InlineData(SamplingDecision.RecordOnly)]
    [InlineData(SamplingDecision.RecordAndSample)]
    public void ExtractContextIrrespectiveOfSamplingDecision(SamplingDecision samplingDecision)
    {
        HttpContext.Current = new HttpContext(
            new HttpRequest(string.Empty, "http://localhost/", string.Empty)
            {
                RequestContext = new RequestContext()
                {
                    RouteData = new RouteData(),
                },
            },
            new HttpResponse(new StringWriter()));

        var isPropagatorCalled = false;
        var propagator = new TestTextMapPropagator
        {
            Extracted = () => isPropagatorCalled = true,
        };

        var activityProcessor = new TestActivityProcessor();
        Sdk.SetDefaultTextMapPropagator(propagator);
        using (var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .SetSampler(new TestSampler(samplingDecision))
            .AddAspNetInstrumentation()
            .AddProcessor(activityProcessor).Build())
        {
            var activity = ActivityHelper.StartAspNetActivity(Propagators.DefaultTextMapPropagator, new HttpContextWrapper(HttpContext.Current), TelemetryHttpModule.Options.OnRequestStartedCallback);
            ActivityHelper.StopAspNetActivity(Propagators.DefaultTextMapPropagator, activity, new HttpContextWrapper(HttpContext.Current), TelemetryHttpModule.Options.OnRequestStoppedCallback);
        }

        Assert.True(isPropagatorCalled);
    }

    [Fact]
    public void ExtractContextIrrespectiveOfTheFilterApplied()
    {
        HttpContext.Current = new HttpContext(
            new HttpRequest(string.Empty, "http://localhost/", string.Empty)
            {
                RequestContext = new RequestContext()
                {
                    RouteData = new RouteData(),
                },
            },
            new HttpResponse(new StringWriter()));

        var isPropagatorCalled = false;
        var propagator = new TestTextMapPropagator
        {
            Extracted = () => isPropagatorCalled = true,
        };

        var isFilterCalled = false;
        var activityProcessor = new TestActivityProcessor();
        Sdk.SetDefaultTextMapPropagator(propagator);
        using (var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddAspNetInstrumentation(options =>
            {
                options.Filter = context =>
                {
                    isFilterCalled = true;
                    return false;
                };
            })
            .AddProcessor(activityProcessor).Build())
        {
            var activity = ActivityHelper.StartAspNetActivity(Propagators.DefaultTextMapPropagator, new HttpContextWrapper(HttpContext.Current), TelemetryHttpModule.Options.OnRequestStartedCallback);
            ActivityHelper.StopAspNetActivity(Propagators.DefaultTextMapPropagator, activity, new HttpContextWrapper(HttpContext.Current), TelemetryHttpModule.Options.OnRequestStoppedCallback);
        }

        Assert.True(isFilterCalled);
        Assert.True(isPropagatorCalled);
    }

    private class TestSampler(SamplingDecision samplingDecision) : Sampler
    {
        private readonly SamplingDecision samplingDecision = samplingDecision;

        public string? ExpectedUrlPath { get; set; }

        public override SamplingResult ShouldSample(in SamplingParameters samplingParameters)
        {
            if (!string.IsNullOrEmpty(this.ExpectedUrlPath))
            {
                Assert.NotNull(samplingParameters.Tags);
                Assert.Contains(samplingParameters.Tags, t => t.Key == "url.path" && (t.Value as string) == this.ExpectedUrlPath);
            }

            return new SamplingResult(this.samplingDecision);
        }
    }
}
