// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;
using Microsoft.Owin.Hosting;
using OpenTelemetry.Instrumentation.Owin.Implementation;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Owin;
using Xunit;

namespace OpenTelemetry.Instrumentation.Owin.Tests;

public class DiagnosticsMiddlewareTests : IDisposable
{
    private readonly Uri serviceBaseUri;
    private readonly IDisposable? listener;
    private readonly EventWaitHandle requestCompleteHandle = new(false, EventResetMode.AutoReset);

    public DiagnosticsMiddlewareTests()
    {
        var random = new Random();
        var retryCount = 5;
        do
        {
            this.serviceBaseUri = new Uri($"http://localhost:{random.Next(2000, 5000)}/");
            try
            {
                this.listener = WebApp.Start(
                    this.serviceBaseUri.ToString(),
                    appBuilder =>
                    {
                        appBuilder.Use((context, next) =>
                        {
                            try
                            {
                                return next();
                            }
                            finally
                            {
                                this.requestCompleteHandle?.Set();
                            }
                        });

                        appBuilder.UseOpenTelemetry();

                        appBuilder.Use((context, next) =>
                        {
                            if (context.Request.Path == new PathString("/exception"))
                            {
                                context.Response.StatusCode = 500;
                                throw new InvalidOperationException("Unhandled exception requested by caller.");
                            }

                            return next();
                        });

                        var config = new HttpConfiguration();
                        config.Routes.MapHttpRoute(
                            name: "DefaultApi",
                            routeTemplate: "api/{controller}/{id}",
                            defaults: new { id = RouteParameter.Optional });

                        appBuilder.UseWebApi(config);
                    });
                break;
            }
            catch
            {
                this.listener?.Dispose();
                this.listener = null;
                retryCount--;
            }
        }
        while (retryCount > 0);

        if (this.listener == null)
        {
            throw new InvalidOperationException("HttpListener could not be started.");
        }
    }

    public void Dispose()
    {
        this.listener?.Dispose();
        this.requestCompleteHandle?.Dispose();
    }

    [Theory]
    [InlineData(true, false, false)]
    [InlineData(true, false, true)]
    [InlineData(true, false, false, true)]
    [InlineData(true, false, false, true, true)]
    [InlineData(true, false, false, false, false, true)]
    [InlineData(true, false, false, false, false, true, true)]
    [InlineData(true, true, false)]
    [InlineData(true, true, true)]
    [InlineData(true, true, false, true)]
    [InlineData(true, true, false, true, true)]
    [InlineData(true, true, false, false, false, true)]
    [InlineData(true, true, false, false, false, true, true)]
    [InlineData(false, true, false)]
    [InlineData(false, true, true)]
    [InlineData(false, true, false, true)]
    [InlineData(false, true, false, true, true)]
    [InlineData(false, true, false, false, false, true)]
    [InlineData(false, true, false, false, false, true, true)]
    [InlineData(false, false)]
    public async Task OutgoingRequestInstrumentationTest(
        bool instrumentTraces,
        bool instrumentMetrics,
        bool filter = false,
        bool enrich = false,
        bool enrichmentException = false,
        bool generateRemoteException = false,
        bool recordException = false)
    {
        List<Activity> stoppedActivities = [];
        List<Metric> exportedMetrics = [];

        var builder = Sdk.CreateTracerProviderBuilder()
            .AddInMemoryExporter(stoppedActivities);
        var meterBuilder = Sdk.CreateMeterProviderBuilder()
            .AddInMemoryExporter(exportedMetrics);

        if (instrumentTraces)
        {
            builder
                .AddOwinInstrumentation(options =>
                {
                    if (enrich)
                    {
                        options.Enrich = enrichmentException
                            ? (_, _, _, _) => throw new Exception("Error while enriching activity")
                            : (activity, eventName, _, _) =>
                            {
                                switch (eventName)
                                {
                                    case OwinEnrichEventType.BeginRequest:
                                        activity.SetTag("client.beginrequest", nameof(OwinEnrichEventType.BeginRequest));
                                        break;
                                    case OwinEnrichEventType.EndRequest:
                                        activity.SetTag("client.endrequest", nameof(OwinEnrichEventType.EndRequest));
                                        break;
                                    default:
                                        break;
                                }
                            };
                    }

                    options.Filter = _ => !filter;
                    options.RecordException = recordException;
                });
        }

        if (instrumentMetrics)
        {
            meterBuilder.AddOwinInstrumentation();
        }

        using var tracerProvider = builder.Build();
        using var meterProvider = meterBuilder.Build();

        using var client = new HttpClient();

        var requestUri = generateRemoteException
            ? new Uri($"{this.serviceBaseUri}exception")
            : new Uri($"{this.serviceBaseUri}api/test");

        this.requestCompleteHandle.Reset();

        using var response = await client.GetAsync(requestUri);

        /* Note: This code will continue executing as soon as the response
        is available but Owin could still be working. We need to wait until
        Owin has finished to inspect the activity status. */

        Assert.True(this.requestCompleteHandle.WaitOne(3000));

        if (instrumentTraces)
        {
            if (!filter)
            {
                Assert.NotEmpty(stoppedActivities);
                Assert.Single(stoppedActivities);

                var activity = stoppedActivities[0];
                Assert.Equal(OwinInstrumentationActivitySource.IncomingRequestActivityName, activity.OperationName);

                Assert.Equal(requestUri.Host, activity.TagObjects.FirstOrDefault(t => t.Key == SemanticConventions.AttributeServerAddress).Value);
                Assert.Equal(requestUri.Port, activity.TagObjects.FirstOrDefault(t => t.Key == SemanticConventions.AttributeServerPort).Value);
                Assert.Equal("GET", activity.TagObjects.FirstOrDefault(t => t.Key == SemanticConventions.AttributeHttpRequestMethod).Value);
                Assert.Equal(requestUri.AbsolutePath, activity.TagObjects.FirstOrDefault(t => t.Key == SemanticConventions.AttributeUrlPath).Value);
                Assert.Equal(generateRemoteException ? 500 : 200, activity.TagObjects.FirstOrDefault(t => t.Key == SemanticConventions.AttributeHttpResponseStatusCode).Value);

                if (generateRemoteException)
                {
                    Assert.Equal(ActivityStatusCode.Error, activity.Status);

                    if (recordException)
                    {
                        Assert.Contains(activity.Events, ae => ae.Name == SemanticConventions.AttributeExceptionEventName);
                    }
                }
                else
                {
                    Assert.Equal(ActivityStatusCode.Unset, activity.Status);
                }

                if (enrich && !enrichmentException)
                {
                    Assert.Equal(nameof(OwinEnrichEventType.BeginRequest), activity.TagObjects.Single(t => t.Key == "client.beginrequest").Value);
                    Assert.Equal(nameof(OwinEnrichEventType.EndRequest), activity.TagObjects.Single(t => t.Key == "client.endrequest").Value);
                }
            }
            else
            {
                Assert.Empty(stoppedActivities);
            }
        }
        else
        {
            Assert.Empty(stoppedActivities);
        }

        if (instrumentMetrics)
        {
            meterProvider.Dispose();
            var metric = exportedMetrics[0];
            var metricPoints = this.GetMetricPoints(metric);
            var metricPoint = Assert.Single(metricPoints);

            Assert.Equal(OwinInstrumentationMetrics.MeterName, metric.MeterName);
            Assert.Equal("http.server.request.duration", metric.Name);
            Assert.Equal(MetricType.Histogram, metric.MetricType);
            Assert.Equal(1, metricPoint.GetHistogramCount());
            Assert.Equal(3, metricPoint.Tags.Count);

            foreach (var tag in metricPoint.Tags)
            {
                switch (tag.Key)
                {
                    case SemanticConventions.AttributeHttpRequestMethod:
                        Assert.Equal("GET", tag.Value);
                        break;
                    case SemanticConventions.AttributeHttpScheme:
                        Assert.Equal(requestUri.Scheme, tag.Value);
                        break;
                    case SemanticConventions.AttributeHttpResponseStatusCode:
                        Assert.Equal(generateRemoteException ? 500 : 200, tag.Value);
                        break;
                    default:
                        break;
                }
            }
        }
        else
        {
            Assert.Empty(exportedMetrics);
        }

        if (instrumentMetrics && instrumentTraces && !filter)
        {
            var metric = Assert.Single(exportedMetrics);
            var activity = Assert.Single(stoppedActivities);
            var metricPoints = this.GetMetricPoints(metric);
            var metricPoint = Assert.Single(metricPoints);

            // metric value and span duration should match
            // https://github.com/open-telemetry/semantic-conventions/blob/main/docs/http/http-metrics.md#metric-httpserverrequestduration
            Assert.Equal(activity.Duration.TotalSeconds, metricPoint.GetHistogramSum());
        }
    }

    [Theory]
    [InlineData("path?a=b&c=d", "path?a=Redacted&c=Redacted", false, false)]
    [InlineData("path?a=b&c=d", "path?a=b&c=d", true, false)]
    [InlineData("path?a=b&c=d", "path?a=b&c=d", false, true)]
    public async Task QueryParametersAreRedacted(
        string actualPath,
        string expectedPath,
        bool disableQueryRedactionUsingEnvVar,
        bool disableQueryRedactionUsingConfiguration)
    {
        using (EnvironmentVariableScope.Create("OTEL_DOTNET_EXPERIMENTAL_OWIN_DISABLE_URL_QUERY_REDACTION", disableQueryRedactionUsingEnvVar ? "true" : null))
        {
            List<Activity> stoppedActivities = [];

            var builder = Sdk.CreateTracerProviderBuilder()
                .ConfigureServices(services =>
                {
                    if (disableQueryRedactionUsingConfiguration)
                    {
                        var config = new ConfigurationBuilder()
                            .AddInMemoryCollection(new Dictionary<string, string?>()
                            {
                                ["OTEL_DOTNET_EXPERIMENTAL_OWIN_DISABLE_URL_QUERY_REDACTION"] = "true",
                            })
                            .Build();

                        services.AddSingleton<IConfiguration>(config);
                    }
                })
                .AddInMemoryExporter(stoppedActivities)
                .AddOwinInstrumentation()
                .Build();

            using var client = new HttpClient();

            var requestUri = new Uri($"{this.serviceBaseUri}{actualPath}");
            var expectedRequestUri = new Uri($"{this.serviceBaseUri}{expectedPath}");

            this.requestCompleteHandle.Reset();

            try
            {
                using var response = await client.GetAsync(requestUri);
            }
            catch
            {
            }

            /* Note: This code will continue executing as soon as the response
            is available but Owin could still be working. We need to wait until
            Owin has finished to inspect the activity status. */

            Assert.True(this.requestCompleteHandle.WaitOne(3000));

            Assert.NotEmpty(stoppedActivities);
            Assert.Single(stoppedActivities);

            var activity = stoppedActivities[0];
            Assert.Equal(OwinInstrumentationActivitySource.IncomingRequestActivityName, activity.OperationName);

            Assert.Equal(requestUri.Host, activity.TagObjects.FirstOrDefault(t => t.Key == SemanticConventions.AttributeServerAddress).Value);
            Assert.Equal(requestUri.Port, activity.TagObjects.FirstOrDefault(t => t.Key == SemanticConventions.AttributeServerPort).Value);
            Assert.Equal("GET", activity.TagObjects.FirstOrDefault(t => t.Key == SemanticConventions.AttributeHttpRequestMethod).Value);
            Assert.Equal(expectedRequestUri.AbsolutePath, activity.TagObjects.FirstOrDefault(t => t.Key == SemanticConventions.AttributeUrlPath).Value);
            Assert.Equal(expectedRequestUri.Query.TrimStart('?'), activity.TagObjects.FirstOrDefault(t => t.Key == SemanticConventions.AttributeUrlQuery).Value);
        }
    }

    private List<MetricPoint> GetMetricPoints(Metric metric)
    {
        List<MetricPoint> metricPoints = [];

        foreach (var metricPoint in metric.GetMetricPoints())
        {
            metricPoints.Add(metricPoint);
        }

        return metricPoints;
    }
}
