// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NETFRAMEWORK
using System.Net.Http;
#endif

namespace OpenTelemetry.Exporter.OneCollector;

internal interface IHttpClient
{
    HttpResponseMessage Send(
        HttpRequestMessage request,
        HttpCompletionOption completionOption,
        CancellationToken cancellationToken);
}

internal sealed class HttpClientWrapper : IHttpClient
{
    private readonly HttpClient httpClient;
#if NET
    private readonly bool synchronousSendSupportedByCurrentPlatform;
#endif

    public HttpClientWrapper(HttpClient httpClient)
    {
        this.httpClient = httpClient;

#if NET
        // See: https://github.com/dotnet/runtime/blob/280f2a0c60ce0378b8db49adc0eecc463d00fe5d/src/libraries/System.Net.Http/src/System/Net/Http/HttpClientHandler.AnyMobile.cs#L767
        this.synchronousSendSupportedByCurrentPlatform = !OperatingSystem.IsAndroid()
            && !OperatingSystem.IsIOS()
            && !OperatingSystem.IsTvOS()
            && !OperatingSystem.IsBrowser();
#endif
    }

    public HttpResponseMessage Send(
        HttpRequestMessage request,
        HttpCompletionOption completionOption,
        CancellationToken cancellationToken)
    {
#if NET
        return this.synchronousSendSupportedByCurrentPlatform
            ? this.httpClient.Send(request, completionOption, cancellationToken)
            : this.httpClient.SendAsync(request, completionOption, cancellationToken).GetAwaiter().GetResult();
#else
        return this.httpClient.SendAsync(request, completionOption, cancellationToken).GetAwaiter().GetResult();
#endif
    }
}
