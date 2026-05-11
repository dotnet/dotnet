// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NETFRAMEWORK
using System.Net.Http;
#endif
using System.Text;
using System.Text.Json;
#if !NETFRAMEWORK
using System.Text.Json.Serialization.Metadata;
#endif

namespace OpenTelemetry.Resources.AWS;

/// <summary>
/// Class for resource detector utils.
/// </summary>
internal static class ResourceDetectorUtils
{
#if NETFRAMEWORK
    private static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web);
#endif

    internal static string SendOutRequest(
        string url,
        HttpMethod method,
        KeyValuePair<string, string>? header,
        HttpClientHandler? handler = null,
        CancellationToken cancellationToken = default)
    {
        using var httpRequestMessage = new HttpRequestMessage();

        httpRequestMessage.RequestUri = new Uri(url);
        httpRequestMessage.Method = method;

        if (header is { } headerValue)
        {
            httpRequestMessage.Headers.Add(headerValue.Key, headerValue.Value);
        }

#pragma warning disable CA2000 // Dispose objects before losing scope
        var httpClient = handler == null ? new HttpClient() : new HttpClient(handler);
#pragma warning restore CA2000 // Dispose objects before losing scope

#if NET
        using var response = httpClient.Send(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
#else
#pragma warning disable CA2025 // Do not pass 'IDisposable' instances into unawaited tasks
        using var response = httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false).GetAwaiter().GetResult();
#pragma warning restore CA2025 // Do not pass 'IDisposable' instances into unawaited tasks
#endif

        response.EnsureSuccessStatusCode();

        return HttpClientHelpers.GetResponseBodyAsString(response, cancellationToken) ?? string.Empty;
    }

#if NETFRAMEWORK
    internal static T? DeserializeFromFile<T>(string filePath)
    {
        using var stream = GetStream(filePath);
        return JsonSerializer.Deserialize<T>(stream, JsonSerializerOptions);
    }

    internal static T? DeserializeFromString<T>(string json) =>
        JsonSerializer.Deserialize<T>(json, JsonSerializerOptions);
#else
    internal static T? DeserializeFromFile<T>(string filePath, JsonTypeInfo<T> jsonTypeInfo)
    {
        using var stream = GetStream(filePath);
        return JsonSerializer.Deserialize(stream, jsonTypeInfo);
    }

    internal static T? DeserializeFromString<T>(string json, JsonTypeInfo<T> jsonTypeInfo) =>
        JsonSerializer.Deserialize(json, jsonTypeInfo);
#endif

    internal static Stream GetStream(string filePath) =>
        new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

    internal static StreamReader GetStreamReader(string filePath)
    {
        var fileStream = GetStream(filePath);
#if NET
        return new StreamReader(fileStream, Encoding.UTF8, leaveOpen: false);
#else
        return new StreamReader(fileStream, Encoding.UTF8);
#endif
    }
}
