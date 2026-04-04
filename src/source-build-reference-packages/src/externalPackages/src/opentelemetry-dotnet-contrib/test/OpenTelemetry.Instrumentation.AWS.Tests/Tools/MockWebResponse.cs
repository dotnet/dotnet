// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Net;
using System.Reflection;

namespace OpenTelemetry.Instrumentation.AWS.Tests.Tools;

internal class MockWebResponse
{
#if NETFRAMEWORK
    public static HttpWebResponse? CreateFromResource(string resourceName)
    {
        var rawResponse = Utils.GetResourceText(resourceName);
        var response = ParseRawReponse(rawResponse);
        var statusCode = ParseStatusCode(response.StatusLine);
        return Create(statusCode, response.Headers, response.Body);
    }

    public static HttpWebResponse? Create(HttpStatusCode statusCode, IDictionary<string, string>? headers, string? body = null)
    {
        var type = typeof(HttpWebResponse);
        var assembly = Assembly.GetAssembly(type);

        if (assembly?.CreateInstance("System.Net.HttpWebResponse") is not HttpWebResponse obj)
        {
            return null;
        }

        var webHeaders = new WebHeaderCollection();
        if (headers != null)
        {
            foreach (var header in headers)
            {
                webHeaders.Add(header.Key, header.Value);
            }
        }

        body ??= string.Empty;
        var responseBodyStream = Utils.CreateStreamFromString(body);

        var statusFieldInfo = type.GetField(
            "m_StatusCode",
            BindingFlags.NonPublic | BindingFlags.Instance);
        var headersFieldInfo = type.GetField(
            "m_HttpResponseHeaders",
            BindingFlags.NonPublic | BindingFlags.Instance);
        var streamFieldInfo = type.GetField(
            "m_ConnectStream",
            BindingFlags.NonPublic | BindingFlags.Instance);
        var contentLengthFieldInfo = type.GetField(
            "m_ContentLength",
            BindingFlags.NonPublic | BindingFlags.Instance);

        statusFieldInfo?.SetValue(obj, statusCode);
        headersFieldInfo?.SetValue(obj, webHeaders);
        streamFieldInfo?.SetValue(obj, responseBodyStream);
        contentLengthFieldInfo?.SetValue(obj, responseBodyStream.Length);

        return obj;
    }

#else
    public static HttpResponseMessage CreateFromResource(string resourceName)
    {
        var rawResponse = Utils.GetResourceText(resourceName);

        var response = ParseRawReponse(rawResponse);
        var statusCode = ParseStatusCode(response.StatusLine);

        return Create(statusCode, response.Headers, response.Body);
    }

    public static HttpResponseMessage Create(HttpStatusCode statusCode, IDictionary<string, string>? headers, string? body = null)
    {
        HttpResponseMessage? httpResponseMessage = null;
        var type = typeof(HttpResponseMessage);
        var assembly = Assembly.GetAssembly(type);
        if (assembly != null)
        {
            httpResponseMessage = assembly.CreateInstance("System.Net.Http.HttpResponseMessage") as HttpResponseMessage;
        }

        httpResponseMessage ??= new HttpResponseMessage(statusCode);

        var webHeaders = new WebHeaderCollection();
        if (headers != null)
        {
            foreach (var header in headers)
            {
                webHeaders.Add(header.Key, header.Value);
                httpResponseMessage?.Headers.Add(header.Key, header.Value);
            }
        }

        httpResponseMessage!.StatusCode = statusCode;
        var dummyJson = "{\"key1\":\"value1\"}";
        httpResponseMessage.Content = new StringContent(body ?? dummyJson); // Content should be in Json format else we get exception from downstream unmarshalling
        return httpResponseMessage;
    }

#endif
    public static HttpResponse ParseRawReponse(string rawResponse)
    {
        var response = new HttpResponse { StatusLine = rawResponse };

        var responseLines = rawResponse.Split('\n');

        if (responseLines.Length == 0)
        {
            throw new ArgumentException(
                "The resource does not contain a valid HTTP response.",
                nameof(rawResponse));
        }

        response.StatusLine = responseLines[0];
        var currentLine = responseLines[0];
        _ = ParseStatusCode(currentLine);

        int lineIndex;
        if (responseLines.Length > 1)
        {
            for (lineIndex = 1; lineIndex < responseLines.Length; lineIndex++)
            {
                currentLine = responseLines[lineIndex];
                if (string.IsNullOrEmpty(currentLine.Trim()))
                {
                    currentLine = responseLines[lineIndex - 1];
                    break;
                }

#pragma warning disable CA1865 // Use char overload
                var index = currentLine.IndexOf(":", StringComparison.Ordinal);
#pragma warning restore CA1865 // Use char overload
                if (index != -1)
                {
                    var headerKey = currentLine.Substring(0, index);
                    var headerValue = currentLine.Substring(index + 1);
                    response.Headers?.Add(headerKey.Trim(), headerValue.Trim());
                }
            }
        }

        var startOfBody = rawResponse.IndexOf(currentLine, StringComparison.Ordinal) + currentLine.Length;
        response.Body = rawResponse.Substring(startOfBody).Trim();
        return response;
    }

    private static HttpStatusCode ParseStatusCode(string? statusLine)
    {
        try
        {
            var statusCode = statusLine?.Split(' ')[1] ?? string.Empty;
#if NET
            return Enum.Parse<HttpStatusCode>(statusCode);
#else
            return (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), statusCode);
#endif
        }
        catch (Exception exception)
        {
            throw new ArgumentException("Invalid HTTP status line.", exception);
        }
    }

    internal class HttpResponse
    {
        public HttpResponse()
        {
            this.Headers = new Dictionary<string, string>();
        }

        public string? StatusLine { get; set; }

        public IDictionary<string, string>? Headers { get; private set; }

        public string? Body { get; set; }
    }
}
