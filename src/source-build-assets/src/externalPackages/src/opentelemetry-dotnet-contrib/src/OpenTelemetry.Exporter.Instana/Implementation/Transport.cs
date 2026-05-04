// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Concurrent;
using System.Globalization;
using System.Net;
#if NETFRAMEWORK
using System.Net.Http;
#endif
using System.Net.Http.Headers;

namespace OpenTelemetry.Exporter.Instana.Implementation;

internal static class Transport
{
    private const int MultiSpanBufferSize = 4096000;
    private const int MultiSpanBufferLimit = 4070000;
    private static readonly MediaTypeHeaderValue MEDIAHEADER = new("application/json");
    private static readonly byte[] TracesBuffer = new byte[MultiSpanBufferSize];
    private static bool isConfigured;
    private static int backendTimeout;
    private static string configuredEndpoint = string.Empty;
    private static string configuredAgentKey = string.Empty;
    private static string bundleUrl = string.Empty;

    static Transport()
    {
        Configure();
    }

    internal static bool IsAvailable => isConfigured && Client != null;

    internal static InstanaHttpClient? Client { get; set; }

    internal static async Task SendSpansAsync(ConcurrentQueue<InstanaSpan> spanQueue)
    {
        try
        {
            using var sendBuffer = new MemoryStream(TracesBuffer);
            using var writer = new StreamWriter(sendBuffer);
            await writer.WriteAsync("{\"spans\":[").ConfigureAwait(false);
            var first = true;

            // peek instead of dequeue, because we don't yet know whether the next span
            // fits within our MULTI_SPAN_BUFFER_LIMIT
            while (spanQueue.TryPeek(out var span) && sendBuffer.Position < MultiSpanBufferLimit)
            {
                if (!first)
                {
                    await writer.WriteAsync(",").ConfigureAwait(false);
                }

                await InstanaSpanSerializer.SerializeToStreamWriterAsync(span, writer).ConfigureAwait(false);
                await writer.FlushAsync().ConfigureAwait(false);

                first = false;

                // Now we can dequeue. Note, this means we'll be giving up/losing
                // this span if we fail to send for any reason.
                spanQueue.TryDequeue(out _);
            }

            await writer.WriteAsync("]}").ConfigureAwait(false);
            await writer.FlushAsync().ConfigureAwait(false);

            var length = sendBuffer.Position;
            sendBuffer.Position = 0;
            sendBuffer.SetLength(length);

            HttpContent content = new StreamContent(sendBuffer, (int)length);
            content.Headers.ContentType = MEDIAHEADER;
            content.Headers.Add("X-INSTANA-TIME", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString(CultureInfo.InvariantCulture));

            using var httpMsg = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(bundleUrl),
            };
            httpMsg.Content = content;
            if (Client != null)
            {
                await Client.SendAsync(httpMsg).ConfigureAwait(false);
            }
        }
        catch (Exception e)
        {
            InstanaExporterEventSource.Log.FailedExport(e);
        }
    }

    private static void Configure()
    {
        if (isConfigured)
        {
            return;
        }

        if (string.IsNullOrEmpty(configuredEndpoint))
        {
            configuredEndpoint = Environment.GetEnvironmentVariable(InstanaExporterConstants.ENVVAR_INSTANA_ENDPOINT_URL);
        }

        if (string.IsNullOrEmpty(configuredEndpoint))
        {
            return;
        }

        bundleUrl = configuredEndpoint + "/bundle";

        if (string.IsNullOrEmpty(configuredAgentKey))
        {
            configuredAgentKey = Environment.GetEnvironmentVariable(InstanaExporterConstants.ENVVAR_INSTANA_AGENT_KEY);
        }

        if (string.IsNullOrEmpty(configuredAgentKey))
        {
            return;
        }

        if (backendTimeout == 0)
        {
            if (!int.TryParse(Environment.GetEnvironmentVariable(InstanaExporterConstants.ENVVAR_INSTANA_TIMEOUT), out backendTimeout))
            {
                backendTimeout = InstanaExporterConstants.BACKEND_DEFAULT_TIMEOUT;
            }
        }

        ConfigureBackendClient();
        isConfigured = true;
    }

    private static void ConfigureBackendClient()
    {
        if (Client != null)
        {
            return;
        }

#pragma warning disable CA2000
        var configuredHandler = new HttpClientHandler();
#pragma warning restore CA2000
        var proxy = Environment.GetEnvironmentVariable(InstanaExporterConstants.ENVVAR_INSTANA_ENDPOINT_PROXY);
        if (Uri.TryCreate(proxy, UriKind.Absolute, out var proxyAddress))
        {
            configuredHandler.Proxy = new WebProxy(proxyAddress, true);
            configuredHandler.UseProxy = true;
#pragma warning disable SA1130 // Use lambda syntax
            configuredHandler.ServerCertificateCustomValidationCallback = delegate { return true; };
#pragma warning restore SA1130 // Use lambda syntax
        }

#pragma warning disable CA5400
        Client = new InstanaHttpClient(backendTimeout, configuredHandler);
#pragma warning restore CA5400

        Client.DefaultRequestHeaders.Add("X-INSTANA-KEY", configuredAgentKey);
    }
}

#pragma warning disable SA1402 // File may only contain a single type
internal class InstanaHttpClient : HttpClient
#pragma warning restore SA1402 // File may only contain a single type
{
    public InstanaHttpClient(int timeout)
        : base()
    {
        this.Timeout = TimeSpan.FromMilliseconds(timeout);
        this.DefaultRequestHeaders.Add("X-INSTANA-NOTRACE", "1");
    }

    public InstanaHttpClient(int timeout, HttpClientHandler handler)
        : base(handler)
    {
        this.Timeout = TimeSpan.FromMilliseconds(timeout);
        this.DefaultRequestHeaders.Add("X-INSTANA-NOTRACE", "1");
    }
}
