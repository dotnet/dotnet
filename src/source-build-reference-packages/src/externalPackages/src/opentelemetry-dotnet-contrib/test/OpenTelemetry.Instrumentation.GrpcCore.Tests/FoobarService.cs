// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace OpenTelemetry.Instrumentation.GrpcCore.Tests;

/// <summary>
/// Test implementation of foobar.
/// </summary>
internal class FoobarService : Foobar.FoobarBase
{
    /// <summary>
    /// Default traceparent header value with the sampling bit on.
    /// </summary>
    internal const string DefaultTraceparentWithSampling = "00-4bf92f3577b34da6a3ce929d0e0e4736-00f067aa0ba902b7-01";

    /// <summary>
    /// The request header fail with status code.
    /// </summary>
    internal const string RequestHeaderFailWithStatusCode = "failurestatuscode";

    /// <summary>
    /// The request header error description.
    /// </summary>
    internal const string RequestHeaderErrorDescription = "failuredescription";

    /// <summary>
    /// The default parent from a traceparent header.
    /// </summary>
    internal static readonly ActivityContext DefaultParentFromTraceparentHeader = ActivityContext.Parse(DefaultTraceparentWithSampling, null);

    /// <summary>
    /// The default request message.
    /// </summary>
    internal static readonly FoobarRequest DefaultRequestMessage = new() { Message = "foo" };

    /// <summary>
    /// The default request message size.
    /// </summary>
    internal static readonly int DefaultRequestMessageSize = DefaultRequestMessage.CalculateSize();

    /// <summary>
    /// The default response message.
    /// </summary>
    internal static readonly FoobarResponse DefaultResponseMessage = new() { Message = "bar" };

    /// <summary>
    /// The default request message size.
    /// </summary>
    internal static readonly int DefaultResponseMessageSize = DefaultResponseMessage.CalculateSize();

    /// <summary>
    /// Starts the specified service.
    /// </summary>
    /// <param name="serverInterceptor">The server interceptor.</param>
    /// <returns>A tuple.</returns>
    public static DisposableServer Start(Interceptor? serverInterceptor = null)
    {
        // Disable SO_REUSEPORT to prevent https://github.com/grpc/grpc/issues/10755
        var serviceDefinition = Foobar.BindService(new FoobarService());
        if (serverInterceptor != null)
        {
            serviceDefinition = serviceDefinition.Intercept(serverInterceptor);
        }

        var server = new Server
        {
            Ports = { { "localhost", ServerPort.PickUnused, ServerCredentials.Insecure } },
            Services = { serviceDefinition },
        };

        server.Start();
        var serverUriString = new Uri("dns:localhost:" + server.Ports.Single().BoundPort).ToString();

        return new DisposableServer(server, serverUriString);
    }

    /// <summary>
    /// Builds the default RPC client.
    /// </summary>
    /// <param name="target">The target.</param>
    /// <param name="clientTracingInterceptor">The client tracing interceptor, if any.</param>
    /// <param name="additionalMetadata">The additional metadata, if any.</param>
    /// <returns>
    /// The gRPC client.
    /// </returns>
    public static Foobar.FoobarClient ConstructRpcClient(
        string target,
        ClientTracingInterceptor? clientTracingInterceptor = null,
        IEnumerable<Metadata.Entry>? additionalMetadata = null)
    {
        var channel = new Channel(target, ChannelCredentials.Insecure);
        var callInvoker = channel.CreateCallInvoker();

        if (clientTracingInterceptor != null)
        {
            callInvoker = callInvoker.Intercept(clientTracingInterceptor);
        }

        // The metadata injector comes first
        if (additionalMetadata != null)
        {
            callInvoker = callInvoker.Intercept(
                metadata =>
                {
                    foreach (var m in additionalMetadata)
                    {
                        metadata.Add(m);
                    }

                    return metadata;
                });
        }

        return new Foobar.FoobarClient(callInvoker);
    }

    /// <summary>
    /// Makes a unary asynchronous request.
    /// </summary>
    /// <param name="client">The client.</param>
    /// <param name="additionalMetadata">The additional metadata.</param>
    /// <returns>A Task.</returns>
    public static async Task MakeUnaryAsyncRequest(Foobar.FoobarClient client, Metadata? additionalMetadata)
    {
        using var call = client.UnaryAsync(DefaultRequestMessage, headers: additionalMetadata);
        _ = await call.ResponseAsync.ConfigureAwait(false);
    }

    /// <summary>
    /// Makes a client streaming request.
    /// </summary>
    /// <param name="client">The client.</param>
    /// <param name="additionalMetadata">The additional metadata.</param>
    /// <returns>A Task.</returns>
    public static async Task MakeClientStreamingRequest(Foobar.FoobarClient client, Metadata? additionalMetadata)
    {
        using var call = client.ClientStreaming(headers: additionalMetadata);
        await call.RequestStream.WriteAsync(DefaultRequestMessage).ConfigureAwait(false);
        await call.RequestStream.CompleteAsync().ConfigureAwait(false);
        _ = await call.ResponseAsync.ConfigureAwait(false);
    }

    /// <summary>
    /// Makes a server streaming request.
    /// </summary>
    /// <param name="client">The client.</param>
    /// <param name="additionalMetadata">The additional metadata.</param>
    /// <returns>A Task.</returns>
    public static async Task MakeServerStreamingRequest(Foobar.FoobarClient client, Metadata? additionalMetadata)
    {
        using var call = client.ServerStreaming(DefaultRequestMessage, headers: additionalMetadata);
        while (await call.ResponseStream.MoveNext().ConfigureAwait(false))
        {
        }
    }

    /// <summary>
    /// Makes a duplex streaming request.
    /// </summary>
    /// <param name="client">The client.</param>
    /// <param name="additionalMetadata">The additional metadata.</param>
    /// <returns>A Task.</returns>
    public static async Task MakeDuplexStreamingRequest(Foobar.FoobarClient client, Metadata? additionalMetadata)
    {
        using var call = client.DuplexStreaming(headers: additionalMetadata);
        await call.RequestStream.WriteAsync(DefaultRequestMessage).ConfigureAwait(false);
        await call.RequestStream.CompleteAsync().ConfigureAwait(false);

        while (await call.ResponseStream.MoveNext().ConfigureAwait(false))
        {
        }
    }

    /// <inheritdoc/>
    public async override Task<FoobarResponse> Unary(FoobarRequest request, ServerCallContext context)
    {
        this.CheckForFailure(context);

        // Yield to avoid race-condition where tests for disposal before the
        // request completes fail as the call is completed before Dispose() completes.
        await Task.Yield();

        return DefaultResponseMessage;
    }

    /// <inheritdoc/>
    public override async Task<FoobarResponse> ClientStreaming(IAsyncStreamReader<FoobarRequest> requestStream, ServerCallContext context)
    {
        this.CheckForFailure(context);

        while (await requestStream.MoveNext().ConfigureAwait(false))
        {
        }

        return DefaultResponseMessage;
    }

    /// <inheritdoc/>
    public override async Task ServerStreaming(FoobarRequest request, IServerStreamWriter<FoobarResponse> responseStream, ServerCallContext context)
    {
        this.CheckForFailure(context);

        await responseStream.WriteAsync(DefaultResponseMessage).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async Task DuplexStreaming(IAsyncStreamReader<FoobarRequest> requestStream, IServerStreamWriter<FoobarResponse> responseStream, ServerCallContext context)
    {
        this.CheckForFailure(context);

        while (await requestStream.MoveNext().ConfigureAwait(false))
        {
        }

        await responseStream.WriteAsync(DefaultResponseMessage).ConfigureAwait(false);
    }

    /// <summary>
    /// Throws if we see some by-convention request metadata.
    /// </summary>
    /// <param name="context">The context.</param>
    private void CheckForFailure(ServerCallContext context)
    {
        var failureStatusCodeString = context.RequestHeaders.GetValue(RequestHeaderFailWithStatusCode);
        var failureDescription = context.RequestHeaders.GetValue(RequestHeaderErrorDescription);
        if (failureStatusCodeString != null)
        {
#if NET
            var statusCode = Enum.Parse<StatusCode>(failureStatusCodeString);
#else
            var statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), failureStatusCodeString);
#endif
            throw new RpcException(new Status(statusCode, failureDescription ?? string.Empty));
        }
    }

    /// <summary>
    /// Wraps server shutdown with an IDisposable pattern.
    /// </summary>
    /// <seealso cref="IDisposable" />
    internal sealed class DisposableServer : IDisposable
    {
        /// <summary>
        /// The server.
        /// </summary>
        private readonly Server server;

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableServer" /> class.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="uriString">The URI string.</param>
        public DisposableServer(Server server, string uriString)
        {
            this.server = server;
            this.UriString = uriString;
        }

        /// <summary>
        /// Gets the URI string.
        /// </summary>
        public string UriString { get; }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.server.ShutdownAsync().Wait();
        }
    }
}
