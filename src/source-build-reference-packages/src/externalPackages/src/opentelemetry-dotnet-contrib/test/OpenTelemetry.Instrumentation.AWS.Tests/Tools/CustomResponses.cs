// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Net;
using Amazon.Runtime;
using Amazon.Runtime.Internal;
#if NETFRAMEWORK
using Amazon.Runtime.Internal.Transform;
#endif
using Amazon.Util;

namespace OpenTelemetry.Instrumentation.AWS.Tests.Tools;

internal static class CustomResponses
{
#if NETFRAMEWORK
    public static void SetResponse(
        AmazonServiceClient client, string? content, string requestId, bool isOK)
    {
        var response = Create(content, requestId, isOK);
        SetResponse(client, response);
    }

    public static void SetResponse(
        AmazonServiceClient client,
        Func<MockHttpRequest, HttpWebResponse?> responseCreator)
    {
        var pipeline = client
            .GetType()
            .GetProperty("RuntimePipeline", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)?
            .GetValue(client, null)
            as RuntimePipeline;

        var requestFactory = new MockHttpRequestFactory { ResponseCreator = responseCreator };
        var httpHandler = new HttpHandler<Stream>(requestFactory, client);
        pipeline?.ReplaceHandler<HttpHandler<Stream>>(httpHandler);
    }

    private static Func<MockHttpRequest, HttpWebResponse?> Create(
        string? content, string requestId, bool isOK)
    {
        var status = isOK ? HttpStatusCode.OK : HttpStatusCode.NotFound;

        return (request) =>
        {
            var headers = new Dictionary<string, string>(StringComparer.Ordinal);
            if (!string.IsNullOrEmpty(requestId))
            {
                headers.Add(HeaderKeys.RequestIdHeader, requestId);
            }

            var response = MockWebResponse.Create(status, headers, content);

            return isOK ? response : throw new HttpErrorResponseException(new HttpWebRequestResponseData(response));
        };
    }
#else
    public static void SetResponse(AmazonServiceClient client, string? content, string requestId, bool isOK)
    {
        var response = Create(content, requestId, isOK);
        SetResponse(client, response);
    }

    public static void SetResponse(AmazonServiceClient client, Func<MockHttpRequest, HttpResponseMessage> responseCreator)
    {
        var pipeline = client
                .GetType()
                .GetProperty("RuntimePipeline", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)?
                .GetValue(client, null)
            as RuntimePipeline;

        var requestFactory = new MockHttpRequestFactory { ResponseCreator = responseCreator };
        var httpHandler = new HttpHandler<HttpContent>(requestFactory, client);
        pipeline?.ReplaceHandler<HttpHandler<HttpContent>>(httpHandler);
    }

    private static Func<MockHttpRequest, HttpResponseMessage> Create(
        string? content, string requestId, bool isOK)
    {
        var status = isOK ? HttpStatusCode.OK : HttpStatusCode.NotFound;

        return (request) =>
        {
            var headers = new Dictionary<string, string>(StringComparer.Ordinal);
            if (!string.IsNullOrEmpty(requestId))
            {
                headers.Add(HeaderKeys.RequestIdHeader, requestId);
            }

            var response = MockWebResponse.Create(status, headers, content);

            return isOK
                ? response
                : throw new HttpErrorResponseException(CustomWebResponse.GenerateWebResponse(response));
        };
    }
#endif
}
