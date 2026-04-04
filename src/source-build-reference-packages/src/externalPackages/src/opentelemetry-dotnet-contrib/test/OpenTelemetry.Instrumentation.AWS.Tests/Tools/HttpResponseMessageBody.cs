// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Amazon.Runtime.Internal.Transform;

namespace OpenTelemetry.Instrumentation.AWS.Tests.Tools;

internal class HttpResponseMessageBody : IHttpResponseBody
{
    private readonly HttpClient? httpClient;
    private readonly HttpResponseMessage response;
    private readonly bool disposeClient;
    private bool disposed;

    public HttpResponseMessageBody(HttpResponseMessage response, HttpClient? httpClient, bool disposeClient)
    {
        this.httpClient = httpClient;
        this.response = response;
        this.disposeClient = disposeClient;
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    Stream IHttpResponseBody.OpenResponse()
    {
#if NET
        ObjectDisposedException.ThrowIf(this.disposed, this);
#else
        if (this.disposed)
        {
            throw new ObjectDisposedException("HttpWebResponseBody");
        }
#endif

        return this.response.Content.ReadAsStreamAsync().Result;
    }

    Task<Stream> IHttpResponseBody.OpenResponseAsync()
    {
#if NET
        ObjectDisposedException.ThrowIf(this.disposed, this);
#else
        if (this.disposed)
        {
            throw new ObjectDisposedException("HttpWebResponseBody");
        }
#endif

        if (this.response.Content != null)
        {
            return this.response.Content.ReadAsStreamAsync();
        }
        else
        {
            var ms = new MemoryStream();
            return Task.FromResult<Stream>(ms);
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (this.disposed)
        {
            return;
        }

        if (disposing)
        {
            this.response?.Dispose();

            if (this.httpClient != null && this.disposeClient)
            {
                this.httpClient.Dispose();
            }

            this.disposed = true;
        }
    }
}
