// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NETFRAMEWORK
using System.Net;
#endif
using Amazon.Runtime;

namespace OpenTelemetry.Instrumentation.AWS.Tests.Tools;

#if NETFRAMEWORK
internal class MockHttpRequestFactory : IHttpRequestFactory<Stream>
{
    public Action? GetResponseAction { get; set; }

    public Func<MockHttpRequest, HttpWebResponse?>? ResponseCreator { get; set; }

    public MockHttpRequest? LastCreatedRequest { get; private set; }

    public IHttpRequest<Stream> CreateHttpRequest(Uri requestUri)
    {
        this.LastCreatedRequest = new MockHttpRequest(requestUri, this.GetResponseAction, this.ResponseCreator);
        return this.LastCreatedRequest;
    }

    public void Dispose()
    {
    }
}
#else
internal class MockHttpRequestFactory : IHttpRequestFactory<HttpContent>
{
    public Action? GetResponseAction { get; set; }

    public MockHttpRequest? LastCreatedRequest { get; private set; }

    public Func<MockHttpRequest, HttpResponseMessage>? ResponseCreator { get; set; }

    public IHttpRequest<HttpContent> CreateHttpRequest(Uri requestUri)
    {
        this.LastCreatedRequest = new MockHttpRequest(requestUri, this.GetResponseAction, this.ResponseCreator);
        return this.LastCreatedRequest;
    }

    public void Dispose()
    {
    }
}
#endif
