// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using Grpc.Core;
using Grpc.Core.Interceptors;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Instrumentation.GrpcCore;

/// <summary>
/// A service interceptor that starts and stops an Activity for each inbound RPC.
/// </summary>
/// <seealso cref="Interceptor" />
public class ServerTracingInterceptor : Interceptor
{
    /// <summary>
    /// The options.
    /// </summary>
    private readonly ServerTracingInterceptorOptions options;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServerTracingInterceptor"/> class.
    /// </summary>
    /// <param name="options">The options.</param>
    public ServerTracingInterceptor(ServerTracingInterceptorOptions options)
    {
        Guard.ThrowIfNull(options);

        this.options = options;
    }

    /// <inheritdoc/>
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        Guard.ThrowIfNull(context);
        Guard.ThrowIfNull(continuation);

        using var rpcScope = new ServerRpcScope<TRequest, TResponse>(context, this.options);

        try
        {
            rpcScope.RecordRequest(request);
            var response = await continuation(request, context).ConfigureAwait(false);
            rpcScope.RecordResponse(response);
            rpcScope.Complete();
            return response;
        }
        catch (Exception e)
        {
            rpcScope.CompleteWithException(e);
            throw;
        }
    }

    /// <inheritdoc/>
    public override async Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(
        IAsyncStreamReader<TRequest> requestStream,
        ServerCallContext context,
        ClientStreamingServerMethod<TRequest, TResponse> continuation)
    {
        Guard.ThrowIfNull(context);
        Guard.ThrowIfNull(continuation);

        using var rpcScope = new ServerRpcScope<TRequest, TResponse>(context, this.options);

        try
        {
            var requestStreamReaderProxy = new AsyncStreamReaderProxy<TRequest>(
                requestStream,
                rpcScope.RecordRequest);

            var response = await continuation(requestStreamReaderProxy, context).ConfigureAwait(false);
            rpcScope.RecordResponse(response);
            rpcScope.Complete();
            return response;
        }
        catch (Exception e)
        {
            rpcScope.CompleteWithException(e);
            throw;
        }
    }

    /// <inheritdoc/>
    public override async Task ServerStreamingServerHandler<TRequest, TResponse>(
        TRequest request,
        IServerStreamWriter<TResponse> responseStream,
        ServerCallContext context,
        ServerStreamingServerMethod<TRequest, TResponse> continuation)
    {
        Guard.ThrowIfNull(context);
        Guard.ThrowIfNull(continuation);

        using var rpcScope = new ServerRpcScope<TRequest, TResponse>(context, this.options);

        try
        {
            rpcScope.RecordRequest(request);

            var responseStreamProxy = new ServerStreamWriterProxy<TResponse>(
                responseStream,
                rpcScope.RecordResponse);

            await continuation(request, responseStreamProxy, context).ConfigureAwait(false);
            rpcScope.Complete();
        }
        catch (Exception e)
        {
            rpcScope.CompleteWithException(e);
            throw;
        }
    }

    /// <inheritdoc/>
    public override async Task DuplexStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, DuplexStreamingServerMethod<TRequest, TResponse> continuation)
    {
        Guard.ThrowIfNull(context);
        Guard.ThrowIfNull(continuation);

        using var rpcScope = new ServerRpcScope<TRequest, TResponse>(context, this.options);

        try
        {
            var requestStreamReaderProxy = new AsyncStreamReaderProxy<TRequest>(
                requestStream,
                rpcScope.RecordRequest);

            var responseStreamProxy = new ServerStreamWriterProxy<TResponse>(
                responseStream,
                rpcScope.RecordResponse);

            await continuation(requestStreamReaderProxy, responseStreamProxy, context).ConfigureAwait(false);
            rpcScope.Complete();
        }
        catch (Exception e)
        {
            rpcScope.CompleteWithException(e);
            throw;
        }
    }

    /// <summary>
    /// A class to help track the lifetime of a service-side RPC.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    private class ServerRpcScope<TRequest, TResponse> : RpcScope<TRequest, TResponse>
        where TRequest : class
        where TResponse : class
    {
        /// <summary>
        /// The metadata setter action.
        /// </summary>
        private static readonly Func<Metadata, string, IEnumerable<string>> MetadataGetter = (metadata, key) =>
        {
            for (var i = 0; i < metadata.Count; i++)
            {
                var entry = metadata[i];
                if (string.Equals(entry.Key, key, StringComparison.OrdinalIgnoreCase))
                {
                    return [entry.Value];
                }
            }

            return [];
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerRpcScope{TRequest, TResponse}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="options">The options.</param>
        public ServerRpcScope(ServerCallContext context, ServerTracingInterceptorOptions options)
            : base(context.Method, options.RecordMessageEvents, options.RecordException)
        {
            if (!GrpcCoreInstrumentation.ActivitySource.HasListeners())
            {
                return;
            }

            var currentContext = Activity.Current?.Context;

            // Extract the SpanContext, if any from the headers
            var metadata = context.RequestHeaders;
            if (metadata != null)
            {
                var propagationContext = options.Propagator.Extract(new PropagationContext(currentContext ?? default, Baggage.Current), metadata, MetadataGetter);
                if (propagationContext.ActivityContext.IsValid())
                {
                    currentContext = propagationContext.ActivityContext;
                }

                if (propagationContext.Baggage != default)
                {
                    Baggage.Current = propagationContext.Baggage;
                }
            }

            var activity = GrpcCoreInstrumentation.ActivitySource.StartActivity(
                this.FullServiceName,
                ActivityKind.Server,
                currentContext ?? default,
                tags: options.AdditionalTags);

            this.SetActivity(activity);
        }
    }
}
