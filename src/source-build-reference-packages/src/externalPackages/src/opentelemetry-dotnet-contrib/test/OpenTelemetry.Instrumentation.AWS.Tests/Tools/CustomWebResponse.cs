// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Net;
using System.Net.Http.Headers;
using Amazon.Runtime.Internal.Transform;

namespace OpenTelemetry.Instrumentation.AWS.Tests.Tools;

internal class CustomWebResponse : IWebResponseData
{
    private readonly HttpResponseMessageBody response;
    private string[]? headerNames;
    private Dictionary<string, string?>? headers;
    private HashSet<string>? headerNamesSet;

    public CustomWebResponse(HttpResponseMessage response)
        : this(response, null, false)
    {
    }

    public CustomWebResponse(HttpResponseMessage responseMsg, HttpClient? httpClient, bool disposeClient)
    {
        this.response = new HttpResponseMessageBody(responseMsg, httpClient, disposeClient);

        this.StatusCode = responseMsg.StatusCode;
        this.IsSuccessStatusCode = responseMsg.IsSuccessStatusCode;
        this.ContentLength = responseMsg.Content.Headers.ContentLength ?? 0;
        this.CopyHeaderValues(responseMsg);
    }

    public HttpStatusCode StatusCode { get; private set; }

    public bool IsSuccessStatusCode { get; private set; }

    public string? ContentType { get; private set; }

    public long ContentLength { get; private set; }

    public IHttpResponseBody ResponseBody => this.response;

    public static IWebResponseData GenerateWebResponse(HttpResponseMessage response)
    {
        return new CustomWebResponse(response);
    }

    public string? GetHeaderValue(string headerName)
    {
        return this.headers != null && this.headers.TryGetValue(headerName, out var headerValue)
            ? headerValue
            : string.Empty;
    }

    public bool IsHeaderPresent(string headerName)
    {
        return this.headerNamesSet != null && this.headerNamesSet.Contains(headerName);
    }

    public string[]? GetHeaderNames()
    {
        return this.headerNames;
    }

    private void CopyHeaderValues(HttpResponseMessage response)
    {
        List<string> headerNames = [];
        this.headers = new Dictionary<string, string?>(10, StringComparer.OrdinalIgnoreCase);

        foreach (var kvp in response.Headers)
        {
            headerNames.Add(kvp.Key);
            var headerValue = this.GetFirstHeaderValue(response.Headers, kvp.Key);
            this.headers.Add(kvp.Key, headerValue);
        }

        if (response.Content != null)
        {
            foreach (var kvp in response.Content.Headers)
            {
                if (!headerNames.Contains(kvp.Key))
                {
                    headerNames.Add(kvp.Key);
                    var headerValue = this.GetFirstHeaderValue(response.Content.Headers, kvp.Key);
                    this.headers.Add(kvp.Key, headerValue);
                }
            }
        }

        this.headerNames = [.. headerNames];
        this.headerNamesSet = new HashSet<string>(this.headerNames, StringComparer.OrdinalIgnoreCase);
    }

    private string? GetFirstHeaderValue(HttpHeaders headers, string key)
    {
        return headers.TryGetValues(key, out var headerValues) ? headerValues.FirstOrDefault() : string.Empty;
    }
}
