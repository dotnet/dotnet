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

    internal static async Task<string> SendOutRequestAsync(
        string url,
        HttpMethod method,
        KeyValuePair<string, string>? header,
        HttpClientHandler? handler = null)
    {
        using (var httpRequestMessage = new HttpRequestMessage())
        {
            httpRequestMessage.RequestUri = new Uri(url);
            httpRequestMessage.Method = method;
            if (header.HasValue)
            {
                httpRequestMessage.Headers.Add(header.Value.Key, header.Value.Value);
            }

#pragma warning disable CA2000 // Dispose objects before losing scope
            var httpClient = handler == null ? new HttpClient() : new HttpClient(handler);
#pragma warning restore CA2000 // Dispose objects before losing scope
            using (var response = await httpClient.SendAsync(httpRequestMessage).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
        }
    }

#if NETFRAMEWORK
    internal static T? DeserializeFromFile<T>(string filePath)
    {
        using (var stream = GetStream(filePath))
        {
            return JsonSerializer.Deserialize<T>(stream, JsonSerializerOptions);
        }
    }

    internal static T? DeserializeFromString<T>(string json)
    {
        return JsonSerializer.Deserialize<T>(json, JsonSerializerOptions);
    }
#else
    internal static T? DeserializeFromFile<T>(string filePath, JsonTypeInfo<T> jsonTypeInfo)
    {
        using (var stream = GetStream(filePath))
        {
            return JsonSerializer.Deserialize(stream, jsonTypeInfo);
        }
    }

    internal static T? DeserializeFromString<T>(string json, JsonTypeInfo<T> jsonTypeInfo)
    {
        return JsonSerializer.Deserialize(json, jsonTypeInfo);
    }
#endif

    internal static Stream GetStream(string filePath)
    {
        return new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
    }

    internal static StreamReader GetStreamReader(string filePath)
    {
        var fileStream = GetStream(filePath);
        var streamReader = new StreamReader(fileStream, Encoding.UTF8);
        return streamReader;
    }
}
