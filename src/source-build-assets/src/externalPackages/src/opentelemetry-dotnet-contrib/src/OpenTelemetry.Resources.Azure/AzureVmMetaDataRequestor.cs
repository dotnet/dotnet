// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Text.Json;

namespace OpenTelemetry.Resources.Azure;

internal static class AzureVmMetaDataRequestor
{
    private static readonly Uri AzureVmMetadataEndpointUri = new("http://169.254.169.254/metadata/instance/compute?api-version=2021-12-13&format=json");

    public static Func<AzureVmMetadataResponse?> GetAzureVmMetaDataResponse { get; internal set; } = GetAzureVmMetaDataResponseDefault;

    public static AzureVmMetadataResponse? GetAzureVmMetaDataResponseDefault()
    {
        var timeout = TimeSpan.FromSeconds(2);

        using var cts = new CancellationTokenSource(timeout);
        using var httpClient = new HttpClient() { Timeout = timeout };

        return GetAzureVmMetaData(httpClient, cts.Token);
    }

    public static AzureVmMetadataResponse? GetAzureVmMetaData(
        HttpClient client,
        CancellationToken cancellationToken)
    {
        using var httpRequestMessage = new HttpRequestMessage();

        httpRequestMessage.RequestUri = AzureVmMetadataEndpointUri;
        httpRequestMessage.Method = HttpMethod.Get;
        httpRequestMessage.Headers.Add("Metadata", "True");

#if NET
        using var response = client.Send(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
#else
#pragma warning disable CA2025 // Do not pass 'IDisposable' instances into unawaited tasks
        using var response = client.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false).GetAwaiter().GetResult();
#pragma warning restore CA2025 // Do not pass 'IDisposable' instances into unawaited tasks
#endif

        response.EnsureSuccessStatusCode();

        var result = HttpClientHelpers.GetResponseBodyAsString(response, cancellationToken);

        if (!string.IsNullOrEmpty(result))
        {
#if NET
            return JsonSerializer.Deserialize(result, SourceGenerationContext.Default.AzureVmMetadataResponse);
#else
            return JsonSerializer.Deserialize<AzureVmMetadataResponse>(result!);
#endif
        }

        return null;
    }
}
