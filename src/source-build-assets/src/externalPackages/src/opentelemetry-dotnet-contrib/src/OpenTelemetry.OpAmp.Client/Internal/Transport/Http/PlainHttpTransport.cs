// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NETFRAMEWORK
using System.Net.Http;
#endif

using Google.Protobuf;
using OpenTelemetry.Internal;
using OpenTelemetry.OpAmp.Client.Internal.Utils;
using OpenTelemetry.OpAmp.Client.Settings;

namespace OpenTelemetry.OpAmp.Client.Internal.Transport.Http;

internal sealed class PlainHttpTransport : IOpAmpTransport, IDisposable
{
    private const string HeaderContentType = "Content-Type";
    private const string HeaderOpAmpInstanceUUID = "OpAMP-Instance-UID";

    private readonly Uri uri;
    private readonly HttpClient httpClient;
    private readonly FrameProcessor processor;
    private readonly OpAmpClientSettings settings;

    public PlainHttpTransport(OpAmpClientSettings settings, FrameProcessor processor)
    {
        Guard.ThrowIfNull(settings, nameof(settings));
        Guard.ThrowIfNull(processor, nameof(processor));

        this.uri = settings.ServerUrl;
        this.processor = processor;
        this.httpClient = settings.HttpClientFactory();
        this.settings = settings;
    }

    public async Task SendAsync<T>(T message, CancellationToken token)
        where T : IMessage<T>
    {
        var content = message.ToByteArray();

        using var byteContent = new ByteArrayContent(content);
        byteContent.Headers.Add(HeaderContentType, "application/x-protobuf");
        byteContent.Headers.Add(HeaderOpAmpInstanceUUID, this.settings.InstanceUid.ToString());

        using var request = new HttpRequestMessage(HttpMethod.Post, this.uri)
        {
            Content = byteContent,
        };

        // ResponseHeadersRead prevents HttpClient from buffering the entire response body
        // before we can enforce the transport size limit.
        using var response = await this.httpClient
            .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, token)
            .ConfigureAwait(false);

        response.EnsureSuccessStatusCode();

        var responseMessage = await HttpClientHelpers.GetResponseBodyAsByteArrayAsync(
            TransportConstants.MaxMessageSize,
            response,
            token).ConfigureAwait(false);

        OpAmpClientEventSource.Log.HttpResponseBytesReceived(responseMessage.Length);

        this.processor.OnServerFrame(responseMessage.AsSequence());
    }

    public void Dispose() => this.httpClient?.Dispose();
}
