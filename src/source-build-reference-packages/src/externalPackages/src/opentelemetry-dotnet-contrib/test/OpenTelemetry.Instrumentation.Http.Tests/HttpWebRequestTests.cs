// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.Net;
using System.Text.Json;
using OpenTelemetry.Tests;
using OpenTelemetry.Trace;
using Xunit;

#if !NETFRAMEWORK
#pragma warning disable SYSLIB0014 // Type or member is obsolete
#endif

namespace OpenTelemetry.Instrumentation.Http.Tests;

[Collection("Http")]
public partial class HttpWebRequestTests
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public static IEnumerable<object[]> TestData => HttpTestData.ReadTestCases();

    [Theory]
    [MemberData(nameof(TestData))]
    public void HttpOutCallsAreCollectedSuccessfully(HttpOutTestCase tc)
    {
        using var serverLifeTime = TestHttpServer.RunServer(
            ctx =>
            {
                ctx.Response.StatusCode = tc.ResponseCode == 0 ? 200 : tc.ResponseCode;
                ctx.Response.OutputStream.Close();
            },
            out var host,
            out var port);

        var enrichWithHttpWebRequestCalled = false;
        var enrichWithHttpWebResponseCalled = false;
        var enrichWithHttpRequestMessageCalled = false;
        var enrichWithHttpResponseMessageCalled = false;
        var enrichWithExceptionCalled = false;

        var exportedItems = new List<Activity>();
        using var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddInMemoryExporter(exportedItems)
            .AddHttpClientInstrumentation(options =>
            {
                options.EnrichWithHttpWebRequest = (_, _) => { enrichWithHttpWebRequestCalled = true; };
                options.EnrichWithHttpWebResponse = (_, _) => { enrichWithHttpWebResponseCalled = true; };
                options.EnrichWithHttpRequestMessage = (_, _) => { enrichWithHttpRequestMessageCalled = true; };
                options.EnrichWithHttpResponseMessage = (_, _) => { enrichWithHttpResponseMessageCalled = true; };
                options.EnrichWithException = (_, _) => { enrichWithExceptionCalled = true; };
                options.RecordException = tc.RecordException ?? false;
            })
            .Build();

        tc.Url = HttpTestData.NormalizeValues(tc.Url, host, port);

        try
        {
            var request = (HttpWebRequest)WebRequest.Create(new Uri(tc.Url));

            request.Method = tc.Method;

            if (tc.Headers != null)
            {
                foreach (var header in tc.Headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            request.ContentLength = 0;

            using var response = (HttpWebResponse)request.GetResponse();

            using var streamReader = new StreamReader(response.GetResponseStream());
            streamReader.ReadToEnd();
        }
        catch (Exception)
        {
            // test case can intentionally send request that will result in exception
            tc.ResponseExpected = false;
        }

        Assert.Single(exportedItems);
        var activity = exportedItems[0];
        ValidateHttpWebRequestActivity(activity);
        Assert.Equal(tc.SpanName, activity.DisplayName);

        tc.SpanAttributes = tc.SpanAttributes.ToDictionary(
            x => x.Key,
            x =>
            {
                return x.Key == "network.protocol.version" ? "1.1" : HttpTestData.NormalizeValues(x.Value, host, port);
            });

        foreach (var tag in activity.TagObjects)
        {
            var tagValue = tag.Value?.ToString();

            if (!tc.SpanAttributes.TryGetValue(tag.Key, out var value))
            {
                if (tag.Key == SemanticConventions.AttributeErrorType)
                {
                    // TODO: Add validation for error.type in test cases.
                    continue;
                }

                Assert.Fail($"Tag {tag.Key} was not found in test data.");
            }

#if NET9_0_OR_GREATER
            // TODO: NEED TO REVIEW THE SPEC
            // NET9 does not record the URL Fragment Identifier.
            if (value.EndsWith("#fragment", StringComparison.Ordinal))
            {
                // remove fragment from expected value
                value = value.Substring(0, value.Length - "#fragment".Length);
            }
#endif

            Assert.Equal(value, tagValue);
        }

#if NETFRAMEWORK
        Assert.True(enrichWithHttpWebRequestCalled);
        Assert.False(enrichWithHttpRequestMessageCalled);
        if (tc.ResponseExpected)
        {
            Assert.True(enrichWithHttpWebResponseCalled);
            Assert.False(enrichWithHttpResponseMessageCalled);
        }
#else
        Assert.False(enrichWithHttpWebRequestCalled);
        Assert.True(enrichWithHttpRequestMessageCalled);
        if (tc.ResponseExpected)
        {
            Assert.False(enrichWithHttpWebResponseCalled);
            Assert.True(enrichWithHttpResponseMessageCalled);
        }
#endif

        if (tc.RecordException.HasValue && tc.RecordException.Value)
        {
#if NET10_0_OR_GREATER
            // .NET 10 reports its own activity, but does not set the status description
            Assert.Contains(activity.Events, evt => evt.Name.Equals("exception"));
#else
            Assert.Single(activity.Events, evt => evt.Name.Equals("exception"));
#endif
            Assert.True(enrichWithExceptionCalled);
        }
    }

    [Fact]
    public void DebugIndividualTest()
    {
        var input = JsonSerializer.Deserialize<HttpOutTestCase>(
        @"
             {
                ""name"": ""Http version attribute populated"",
                ""method"": ""GET"",
                ""url"": ""http://{host}:{port}/"",
                ""responseCode"": 200,
                ""spanName"": ""GET"",
                ""spanStatus"": ""UNSET"",
                ""spanKind"": ""Client"",
                ""setHttpFlavor"": true,
                ""spanAttributes"": {
                  ""url.scheme"": ""http"",
                  ""http.request.method"": ""GET"",
                  ""server.address"": ""{host}"",
                  ""server.port"": ""{port}"",
                  ""network.protocol.version"": ""1.1"",
                  ""http.response.status_code"": ""200"",
                  ""url.full"": ""http://{host}:{port}/""
                }
              }
            ",
        JsonSerializerOptions);

        Assert.NotNull(input);
        this.HttpOutCallsAreCollectedSuccessfully(input);
    }

    private static void ValidateHttpWebRequestActivity(Activity activityToValidate)
    {
        Assert.Equal(ActivityKind.Client, activityToValidate.Kind);
    }
}
