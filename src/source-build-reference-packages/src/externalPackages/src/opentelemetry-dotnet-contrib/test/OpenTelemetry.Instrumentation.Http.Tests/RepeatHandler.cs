// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NETFRAMEWORK
using System.Net.Http;
#endif

namespace OpenTelemetry.Tests;

internal class RepeatHandler : DelegatingHandler
{
    private readonly int maxRetries;

    public RepeatHandler(HttpMessageHandler innerHandler, int maxRetries)
        : base(innerHandler)
    {
        this.maxRetries = maxRetries;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        HttpResponseMessage? response = null;
        for (var i = 0; i < this.maxRetries; i++)
        {
            response?.Dispose();

            try
            {
                response = await base.SendAsync(request, cancellationToken);
            }
            catch
            {
            }
        }

        return response!;
    }
}
