// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using RouteTests.TestApplication;
using Xunit;

namespace RouteTests;

[Collection("AspNetCore")]
public class RoutingTests : IClassFixture<RoutingTestFixture>
{
    private const string HttpStatusCode = "http.response.status_code";
    private const string HttpMethod = "http.request.method";
    private const string HttpRoute = "http.route";

    private readonly RoutingTestFixture fixture;

    public RoutingTests(RoutingTestFixture fixture)
    {
        this.fixture = fixture;
    }

    public static IEnumerable<object[]> TestData => RoutingTestCases.GetTestCases();

    [Theory]
    [MemberData(nameof(TestData))]
    public async Task TestHttpRoute_Traces(TestCase testCase)
    {
        List<Activity> exportedActivities = [];

        using var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddAspNetCoreInstrumentation()
            .AddInMemoryExporter(exportedActivities)
            .Build()!;

        await this.fixture.MakeRequest(testCase.TestApplicationScenario, testCase.Path);

        Action flushAction = () => tracerProvider.ForceFlush();
        var result = await TryWaitUntilAny(exportedActivities, flushAction);
        Assert.True(result, "No activities were collected");

        var activity = Assert.Single(exportedActivities);

        GetTagsFromActivity(activity, out var activityHttpStatusCode, out var activityHttpMethod, out var activityHttpRoute);

        Assert.Equal(testCase.ExpectedStatusCode, activityHttpStatusCode);
        Assert.Equal(testCase.HttpMethod, activityHttpMethod);

        // TODO: The CurrentHttpRoute property will go away. They only serve to capture status quo.
        // If CurrentHttpRoute is null, then that means we already conform to the correct behavior.
        var expectedHttpRoute = testCase.CurrentHttpRoute ?? testCase.ExpectedHttpRoute;
        Assert.Equal(expectedHttpRoute, activityHttpRoute);

        // Activity.DisplayName should be a combination of http.method + http.route attributes, see:
        // https://github.com/open-telemetry/semantic-conventions/blob/main/docs/http/http-spans.md#name
        var expectedActivityDisplayName = string.IsNullOrEmpty(expectedHttpRoute)
            ? testCase.HttpMethod
            : $"{testCase.HttpMethod} {expectedHttpRoute}";

        Assert.Equal(expectedActivityDisplayName, activity.DisplayName);

        var testResult = new ActivityRoutingTestResult(testCase)
        {
            IdealHttpRoute = testCase.ExpectedHttpRoute,
            ActivityDisplayName = activity.DisplayName,
            ActivityHttpRoute = activityHttpRoute,
            RouteInfo = RouteInfo.Current,
        };

        this.fixture.AddActivityTestResult(testResult);
    }

    [Theory]
    [MemberData(nameof(TestData))]
    public async Task TestHttpRoute_Metrics(TestCase testCase)
    {
        List<Metric> exportedMetrics = [];

        using var meterProvider = Sdk.CreateMeterProviderBuilder()
            .AddAspNetCoreInstrumentation()
            .AddInMemoryExporter(exportedMetrics)
            .Build()!;

        await this.fixture.MakeRequest(testCase.TestApplicationScenario, testCase.Path);

        var filter = new Func<Metric, bool>(x =>
            x.Name is "http.server.request.duration" or "http.server.duration");
        var flushAction = new Action(() => meterProvider.ForceFlush());
        var result = await TryWaitUntilAny(exportedMetrics, flushAction, filter);
        Assert.True(result, "No metrics were collected");

        var durationMetric = exportedMetrics.Single(filter);
        var metricPoints = new List<MetricPoint>();
        foreach (var mp in durationMetric.GetMetricPoints())
        {
            metricPoints.Add(mp);
        }

        var metricPoint = Assert.Single(metricPoints);

        GetTagsFromMetricPoint(metricPoint, out var metricHttpStatusCode, out var metricHttpMethod, out var metricHttpRoute);

        Assert.Equal(testCase.ExpectedStatusCode, metricHttpStatusCode);
        Assert.Equal(testCase.HttpMethod, metricHttpMethod);

        // TODO: The CurrentHttpRoute property will go away. They only serve to capture status quo.
        // If CurrentHttpRoute is null, then that means we already conform to the correct behavior.
        var expectedHttpRoute = testCase.CurrentHttpRoute ?? testCase.ExpectedHttpRoute;
        var expectedMetricRoute = testCase.ExpectedMetricRoute ?? expectedHttpRoute;
        Assert.Equal(expectedMetricRoute, metricHttpRoute);

        var testResult = new MetricRoutingTestResult(testCase)
        {
            IdealHttpRoute = testCase.ExpectedHttpRoute,
            MetricHttpRoute = metricHttpRoute,
            RouteInfo = RouteInfo.Current,
        };

        this.fixture.AddMetricsTestResult(testResult);
    }

    private static async Task<bool> TryWaitUntilAny<T>(ICollection<T> collection, Action flush, Func<T, bool>? filter = null)
    {
        // flush and see if data is immediately available
        flush();

        for (var i = 0; i < 10; i++)
        {
            if (collection.Count > 0)
            {
                // nothing to filter
                if (filter == null)
                {
                    return true;
                }

                foreach (var item in collection)
                {
                    // filter is defined and succeeds finding elements
                    if (filter(item))
                    {
                        return true;
                    }
                }
            }

            // reflush and try again
            flush();

            // Add time to process the flush
            await Task.Delay(TimeSpan.FromSeconds(1.5));
        }

        return false;
    }

    private static void GetTagsFromActivity(Activity activity, out int httpStatusCode, out string httpMethod, out string? httpRoute)
    {
        var expectedStatusCodeKey = HttpStatusCode;
        var expectedHttpMethodKey = HttpMethod;
        httpStatusCode = Convert.ToInt32(activity.GetTagItem(expectedStatusCodeKey));
        httpMethod = (activity.GetTagItem(expectedHttpMethodKey) as string)!;
        httpRoute = activity.GetTagItem(HttpRoute) as string ?? string.Empty;
    }

    private static void GetTagsFromMetricPoint(MetricPoint metricPoint, out int httpStatusCode, out string httpMethod, out string? httpRoute)
    {
        var expectedStatusCodeKey = HttpStatusCode;
        var expectedHttpMethodKey = HttpMethod;

        httpStatusCode = 0;
        httpMethod = string.Empty;
        httpRoute = string.Empty;

        foreach (var tag in metricPoint.Tags)
        {
            if (tag.Key.Equals(expectedStatusCodeKey))
            {
                httpStatusCode = Convert.ToInt32(tag.Value);
            }
            else if (tag.Key.Equals(expectedHttpMethodKey))
            {
                httpMethod = (tag.Value as string)!;
            }
            else if (tag.Key.Equals(HttpRoute))
            {
                httpRoute = tag.Value as string;
            }
        }
    }
}
