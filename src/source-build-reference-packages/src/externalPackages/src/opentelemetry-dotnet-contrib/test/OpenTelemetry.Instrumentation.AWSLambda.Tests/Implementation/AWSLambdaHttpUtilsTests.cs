// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.ApplicationLoadBalancerEvents;
using OpenTelemetry.AWS;
using OpenTelemetry.Instrumentation.AWSLambda.Implementation;
using OpenTelemetry.Trace;
using Xunit;

namespace OpenTelemetry.Instrumentation.AWSLambda.Tests.Implementation;

[Collection("TracerProviderDependent")]
public class AWSLambdaHttpUtilsTests
{
    private readonly AWSSemanticConventions latestSemanticConventions =
        new AWSSemanticConventions(SemanticConventionVersion.Latest);

    [Fact]
    public void GetHttpTags_APIGatewayProxyRequest_ReturnsCorrectTags()
    {
        var request = new APIGatewayProxyRequest
        {
            MultiValueHeaders = new Dictionary<string, IList<string>>
            {
                { "X-Forwarded-Proto", new List<string> { "https" } },
                { "Host", new List<string> { "localhost:1234" } },
            },
            MultiValueQueryStringParameters = new Dictionary<string, IList<string>>
            {
#pragma warning disable CA1861 // Avoid constant arrays as arguments
                { "q1", ["value1"] },
#pragma warning restore CA1861 // Avoid constant arrays as arguments
            },
            RequestContext = new APIGatewayProxyRequest.ProxyRequestContext
            {
                HttpMethod = "GET",
                Path = "/path/test",
            },
        };

        var actualTags = AWSLambdaHttpUtils.GetHttpTags(this.latestSemanticConventions, request);

        var expectedTags = new Dictionary<string, object>
        {
            { ExpectedSemanticConventions.AttributeHttpScheme, "https" },
            { ExpectedSemanticConventions.AttributeUrlPath, "/path/test" },
            { ExpectedSemanticConventions.AttributeUrlQuery, "?q1=value1" },
            { ExpectedSemanticConventions.AttributeNetHostName, "localhost" },
            { ExpectedSemanticConventions.AttributeNetHostPort, 1234 },
            { ExpectedSemanticConventions.AttributeHttpMethod, "GET" },
        };

        AssertTags(expectedTags, actualTags);
    }

    [Fact]
    public void GetHttpTags_ApplicationLoadBalancerRequest_ReturnsCorrectTags()
    {
        var request = new ApplicationLoadBalancerRequest
        {
            Headers = new Dictionary<string, string>
            {
                { "X-Forwarded-Proto",  "https" },
                { "Host", "localhost:1234" },
            },
            QueryStringParameters = new Dictionary<string, string>
            {
                { "q1",  "value1" },
            },
            HttpMethod = "GET",
            Path = "/path/test",
        };

        var actualTags = AWSLambdaHttpUtils.GetHttpTags(this.latestSemanticConventions, request);

        var expectedTags = new Dictionary<string, object>
        {
            { ExpectedSemanticConventions.AttributeHttpScheme, "https" },
            { ExpectedSemanticConventions.AttributeUrlPath, "/path/test" },
            { ExpectedSemanticConventions.AttributeUrlQuery, "?q1=value1" },
            { ExpectedSemanticConventions.AttributeNetHostName, "localhost" },
            { ExpectedSemanticConventions.AttributeNetHostPort, 1234 },
            { ExpectedSemanticConventions.AttributeHttpMethod, "GET" },
        };

        AssertTags(expectedTags, actualTags);
    }

    [Fact]
    public void GetHttpTags_ApplicationLoadBalancerRequestWithMultiValue_ReturnsCorrectTags()
    {
        var request = new ApplicationLoadBalancerRequest
        {
            MultiValueHeaders = new Dictionary<string, IList<string>>
            {
                { "X-Forwarded-Proto", new List<string> { "https" } },
                { "Host", new List<string> { "localhost:1234" } },
            },
            MultiValueQueryStringParameters = new Dictionary<string, IList<string>>
            {
#pragma warning disable CA1861 // Avoid constant arrays as arguments
                { "q1", ["value1"] },
#pragma warning restore CA1861 // Avoid constant arrays as arguments
            },
            HttpMethod = "GET",
            Path = "/path/test",
        };

        var actualTags = AWSLambdaHttpUtils.GetHttpTags(this.latestSemanticConventions, request);

        var expectedTags = new Dictionary<string, object>
        {
            { ExpectedSemanticConventions.AttributeHttpScheme, "https" },
            { ExpectedSemanticConventions.AttributeUrlPath, "/path/test" },
            { ExpectedSemanticConventions.AttributeUrlQuery, "?q1=value1" },
            { ExpectedSemanticConventions.AttributeNetHostName, "localhost" },
            { ExpectedSemanticConventions.AttributeNetHostPort, 1234 },
            { ExpectedSemanticConventions.AttributeHttpMethod, "GET" },
        };

        AssertTags(expectedTags, actualTags);
    }

    [Fact]
    public void GetHttpTags_ApplicationLoadBalancerRequestWithMultiValueHeader_UsesLastValue()
    {
        var request = new ApplicationLoadBalancerRequest
        {
            MultiValueHeaders = new Dictionary<string, IList<string>>
            {
                { "X-Forwarded-Proto", new List<string> { "https", "http" } },
                { "Host", new List<string> { "localhost:1234", "myhost:432" } },
            },
        };

        var actualTags = AWSLambdaHttpUtils.GetHttpTags(this.latestSemanticConventions, request);

        var expectedTags = new Dictionary<string, object>
        {
            { ExpectedSemanticConventions.AttributeUrlPath, string.Empty },
            { ExpectedSemanticConventions.AttributeUrlQuery, string.Empty },
            { ExpectedSemanticConventions.AttributeHttpScheme, "http" },
            { ExpectedSemanticConventions.AttributeNetHostName, "myhost" },
            { ExpectedSemanticConventions.AttributeNetHostPort, 432 },
        };

        AssertTags(expectedTags, actualTags);
    }

    [Fact]
    public void SetHttpTagsFromResult_ApplicationLoadBalancerResponse_SetsCorrectTags()
    {
        var response = new ApplicationLoadBalancerResponse
        {
            StatusCode = 200,
        };

        using var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddSource("TestActivitySource")
            .Build();

        using var testActivitySource = new ActivitySource("TestActivitySource");
        using var activity = testActivitySource.StartActivity("TestActivity");

        AWSLambdaHttpUtils.SetHttpTagsFromResult(this.latestSemanticConventions, activity, response);

        var expectedTags = new Dictionary<string, object>
        {
            { ExpectedSemanticConventions.AttributeHttpStatusCode, 200 },
        };

        var actualTags = activity?.TagObjects
            .Select(kvp => new KeyValuePair<string, object>(kvp.Key, kvp.Value ?? new object()));

        AssertTags(expectedTags, actualTags);
    }

    [Theory]
    [InlineData(null, "")]
#pragma warning disable CA1861 // Avoid constant arrays as arguments
    [InlineData("", "?name=")]
    [InlineData("value1", "?name=value1")]
    [InlineData("value$a", "?name=value%24a")]
    [InlineData("value 1", "?name=value+1")]
#pragma warning restore CA1861 // Avoid constant arrays as arguments
    public void GetQueryString_ApplicationLoadBalancerRequest_CorrectQueryString(string? value, string expectedQueryString)
    {
        var request = new ApplicationLoadBalancerRequest();
        if (value != null)
        {
            request.QueryStringParameters = new Dictionary<string, string>
            {
                { "name", value },
            };
        }

        var queryString = AWSLambdaHttpUtils.GetQueryString(request);

        Assert.Equal(expectedQueryString, queryString);
    }

    [Theory]
    [InlineData(null, "")]
#pragma warning disable CA1861 // Avoid constant arrays as arguments
    [InlineData(new string[] { }, "")]
    [InlineData(new[] { "value1" }, "?name=value1")]
    [InlineData(new[] { "value$a" }, "?name=value%24a")]
    [InlineData(new[] { "value 1" }, "?name=value+1")]
    [InlineData(new[] { "value1", "value2" }, "?name=value1&name=value2")]
#pragma warning restore CA1861 // Avoid constant arrays as arguments
    public void GetQueryString_ApplicationLoadBalancerRequestMultiValue_CorrectQueryString(IList<string>? values, string expectedQueryString)
    {
        var request = new ApplicationLoadBalancerRequest();
        if (values != null)
        {
            request.MultiValueQueryStringParameters = new Dictionary<string, IList<string>>
            {
                { "name", values },
            };
        }

        var queryString = AWSLambdaHttpUtils.GetQueryString(request);

        Assert.Equal(expectedQueryString, queryString);
    }

    [Fact]
    public void GetHttpTags_APIGatewayProxyRequestWithEmptyContext_ReturnsTagsFromRequest()
    {
        var request = new APIGatewayProxyRequest
        {
            MultiValueQueryStringParameters = new Dictionary<string, IList<string>>
            {
#pragma warning disable CA1861 // Avoid constant arrays as arguments
                { "q1", ["value1"] },
#pragma warning restore CA1861 // Avoid constant arrays as arguments
            },
            HttpMethod = "POST",
            Path = "/path/test",
        };

        var actualTags = AWSLambdaHttpUtils.GetHttpTags(this.latestSemanticConventions, request);

        var expectedTags = new Dictionary<string, object>
        {
            { ExpectedSemanticConventions.AttributeHttpMethod, "POST" },
            { ExpectedSemanticConventions.AttributeUrlPath, "/path/test" },
            { ExpectedSemanticConventions.AttributeUrlQuery, "?q1=value1" },
        };

        AssertTags(expectedTags, actualTags);
    }

    [Fact]
    public void GetHttpTags_APIGatewayProxyRequestWithMultiValueHeader_UsesLastValue()
    {
        var request = new APIGatewayProxyRequest
        {
            MultiValueHeaders = new Dictionary<string, IList<string>>
            {
                { "X-Forwarded-Proto", new List<string> { "https", "http" } },
                { "Host", new List<string> { "localhost:1234", "myhost:432" } },
            },
        };

        var actualTags = AWSLambdaHttpUtils.GetHttpTags(this.latestSemanticConventions, request);

        var expectedTags = new Dictionary<string, object>
        {
            { ExpectedSemanticConventions.AttributeUrlPath, string.Empty },
            { ExpectedSemanticConventions.AttributeUrlQuery, string.Empty },
            { ExpectedSemanticConventions.AttributeHttpScheme, "http" },
            { ExpectedSemanticConventions.AttributeNetHostName, "myhost" },
            { ExpectedSemanticConventions.AttributeNetHostPort, 432 },
        };

        AssertTags(expectedTags, actualTags);
    }

    [Fact]
    public void GetHttpTags_APIGatewayHttpApiV2ProxyRequest_ReturnsCorrectTags()
    {
        var request = new APIGatewayHttpApiV2ProxyRequest
        {
            Headers = new Dictionary<string, string>
            {
                { "X-Forwarded-Proto",  "https" },
                { "Host", "localhost:1234" },
            },
            RawPath = "/path/test",
            RawQueryString = "q1=value1",
            RequestContext = new APIGatewayHttpApiV2ProxyRequest.ProxyRequestContext
            {
                Http = new APIGatewayHttpApiV2ProxyRequest.HttpDescription
                {
                    Method = "GET",
                },
            },
        };

        var actualTags = AWSLambdaHttpUtils.GetHttpTags(this.latestSemanticConventions, request);

        var expectedTags = new Dictionary<string, object>
        {
            { ExpectedSemanticConventions.AttributeHttpScheme, "https" },
            { ExpectedSemanticConventions.AttributeUrlPath, "/path/test" },
            { ExpectedSemanticConventions.AttributeUrlQuery, "?q1=value1" },
            { ExpectedSemanticConventions.AttributeNetHostName, "localhost" },
            { ExpectedSemanticConventions.AttributeNetHostPort, 1234 },
            { ExpectedSemanticConventions.AttributeHttpMethod, "GET" },
        };

        AssertTags(expectedTags, actualTags);
    }

    [Fact]
    public void GetHttpTags_APIGatewayHttpApiV2ProxyRequestWithMultiValueHeader_UsesLastValue()
    {
        var request = new APIGatewayHttpApiV2ProxyRequest
        {
            Headers = new Dictionary<string, string>
            {
                { "X-Forwarded-Proto", "https,http" },
                { "Host", "localhost:1234,myhost:432" },
            },
        };

        var actualTags = AWSLambdaHttpUtils.GetHttpTags(this.latestSemanticConventions, request);

        var expectedTags = new Dictionary<string, object>
        {
            { ExpectedSemanticConventions.AttributeUrlPath, string.Empty },
            { ExpectedSemanticConventions.AttributeUrlQuery, string.Empty },
            { ExpectedSemanticConventions.AttributeHttpScheme, "http" },
            { ExpectedSemanticConventions.AttributeNetHostName, "myhost" },
            { ExpectedSemanticConventions.AttributeNetHostPort, 432 },
        };

        AssertTags(expectedTags, actualTags);
    }

    [Fact]
    public void SetHttpTagsFromResult_APIGatewayProxyResponse_SetsCorrectTags()
    {
        var response = new APIGatewayProxyResponse
        {
            StatusCode = 200,
        };

        using var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddSource("TestActivitySource")
            .Build();

        using var testActivitySource = new ActivitySource("TestActivitySource");
        using var activity = testActivitySource.StartActivity("TestActivity");

        AWSLambdaHttpUtils.SetHttpTagsFromResult(this.latestSemanticConventions, activity, response);

        var expectedTags = new Dictionary<string, object>
        {
            { ExpectedSemanticConventions.AttributeHttpStatusCode, 200 },
        };

        Assert.NotNull(activity);

        var actualTags = activity.TagObjects
            .Select(kvp => new KeyValuePair<string, object>(kvp.Key, kvp.Value!));
        AssertTags(expectedTags, actualTags);
    }

    [Fact]
    public void SetHttpTagsFromResult_APIGatewayHttpApiV2ProxyResponse_SetsCorrectTags()
    {
        var response = new APIGatewayHttpApiV2ProxyResponse
        {
            StatusCode = 200,
        };

        using var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddSource("TestActivitySource")
            .Build();

        using var testActivitySource = new ActivitySource("TestActivitySource");
        using var activity = testActivitySource.StartActivity("TestActivity");

        AWSLambdaHttpUtils.SetHttpTagsFromResult(this.latestSemanticConventions, activity, response);

        var expectedTags = new Dictionary<string, object>
        {
            { ExpectedSemanticConventions.AttributeHttpStatusCode, 200 },
        };

        var actualTags = activity?.TagObjects
            .Select(kvp => new KeyValuePair<string, object>(kvp.Key, kvp.Value ?? new object()));

        AssertTags(expectedTags, actualTags);
    }

    [Theory]
    [InlineData(null, null, null, null)]
    [InlineData("", "", "", null)]
    [InlineData(null, "localhost:4321", "localhost", 4321)]
    [InlineData(null, "localhost:1a", "localhost", null)]
    [InlineData(null, "localhost", "localhost", null)]
    [InlineData("http", "localhost", "localhost", 80)]
    [InlineData("https", "localhost", "localhost", 443)]
    public void GetHostAndPort_HostHeader_ReturnsCorrectHostAndPort(string? httpSchema, string? hostHeader, string? expectedHost, int? expectedPort)
    {
        (var host, var port) = AWSLambdaHttpUtils.GetHostAndPort(httpSchema, hostHeader);

        Assert.Equal(expectedHost, host);
        Assert.Equal(expectedPort, port);
    }

    [Theory]
    [InlineData(null, "")]
#pragma warning disable CA1861 // Avoid constant arrays as arguments
    [InlineData(new string[] { }, "")]
    [InlineData(new[] { "value1" }, "?name=value1")]
    [InlineData(new[] { "value$a" }, "?name=value%24a")]
    [InlineData(new[] { "value 1" }, "?name=value+1")]
    [InlineData(new[] { "value1", "value2" }, "?name=value1&name=value2")]
#pragma warning restore CA1861 // Avoid constant arrays as arguments
    public void GetQueryString_APIGatewayProxyRequest_CorrectQueryString(IList<string>? values, string expectedQueryString)
    {
        var request = new APIGatewayProxyRequest();
        if (values != null)
        {
            request.MultiValueQueryStringParameters = new Dictionary<string, IList<string>>
            {
                { "name", values },
            };
        }

        var queryString = AWSLambdaHttpUtils.GetQueryString(request);

        Assert.Equal(expectedQueryString, queryString);
    }

    [Theory]
    [InlineData(null, "")]
    [InlineData("", "")]
    [InlineData("name=value1", "?name=value1")]
    [InlineData("sdckj9_+", "?sdckj9_+")]
    public void GetQueryString_APIGatewayHttpApiV2ProxyRequest_CorrectQueryString(string? rawQueryString, string expectedQueryString)
    {
        var request = new APIGatewayHttpApiV2ProxyRequest
        {
            RawQueryString = rawQueryString,
        };

        var queryString = AWSLambdaHttpUtils.GetQueryString(request);

        Assert.Equal(expectedQueryString, queryString);
    }

    private static void AssertTags<TActualValue>(Dictionary<string, object> expectedTags, IEnumerable<KeyValuePair<string, TActualValue>>? actualTags)
        where TActualValue : class
    {
        Assert.NotNull(actualTags);
        Assert.Equal(expectedTags.Count, actualTags.Count());
        foreach (var tag in expectedTags)
        {
            Assert.Contains(new KeyValuePair<string, TActualValue>(tag.Key, (TActualValue)tag.Value), actualTags);
        }
    }

    private static class ExpectedSemanticConventions
    {
        public const string AttributeHttpScheme = "url.scheme";
        public const string AttributeUrlPath = "url.path";
        public const string AttributeUrlQuery = "url.query";
        public const string AttributeNetHostName = "server.address";
        public const string AttributeNetHostPort = "server.port";
        public const string AttributeHttpMethod = "http.request.method";
        public const string AttributeHttpStatusCode = "http.response.status_code";
    }
}
