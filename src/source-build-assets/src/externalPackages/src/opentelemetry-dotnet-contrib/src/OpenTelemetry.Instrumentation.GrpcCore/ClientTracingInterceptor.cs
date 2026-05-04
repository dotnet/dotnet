// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using Grpc.Core;
using Grpc.Core.Interceptors;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Instrumentation.GrpcCore;

/// <summary>
/// A client interceptor that starts and stops an Activity for each outbound RPC.
/// </summary>
/// <seealso cref="Interceptor" />
public class ClientTracingInterceptor : Interceptor
{
    /// <summary>
    /// The options.
    /// </summary>
    private readonly ClientTracingInterceptorOptions options;

    /// <summary>
    /// Initializes a new instance of the <see cref="ClientTracingInterceptor"/> class.
    /// </summary>
    /// <param name="options">The options.</param>
    public ClientTracingInterceptor(ClientTracingInterceptorOptions options)
    {
        Guard.ThrowIfNull(options);

        this.options = options;
    }

    /// <inheritdoc/>
    public override TResponse BlockingUnaryCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        BlockingUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        Guard.ThrowIfNull(context);
        Guard.ThrowIfNull(continuation);

        ClientRpcScope<TRequest, TResponse>? rpcScope = null;

        try
        {
            rpcScope = new ClientRpcScope<TRequest, TResponse>(context, this.options);
            rpcScope.RecordRequest(request);
            var response = continuation(request, rpcScope.Context);
            rpcScope.RecordResponse(response);
            rpcScope.Complete();
            return response;
        }
        catch (Exception e)
        {
            rpcScope?.CompleteWithException(e);
            throw;
        }
        finally
        {
            rpcScope?.RestoreParentActivity();
            rpcScope?.Dispose();
        }
    }

    /// <inheritdoc/>
    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        Guard.ThrowIfNull(context);
        Guard.ThrowIfNull(continuation);

        ClientRpcScope<TRequest, TResponse>? rpcScope = null;

        try
        {
#pragma warning  disable CA2000
            rpcScope = new ClientRpcScope<TRequest, TResponse>(context, this.options);
#pragma warning restore CA2000
            rpcScope.RecordRequest(request);
            var responseContinuation = continuation(request, rpcScope.Context);
            var responseAsync = responseContinuation.ResponseAsync.ContinueWith(
                responseTask =>
                {
                    try
                    {
                        var response = responseTask.Result;
                        rpcScope.RecordResponse(response);
                        rpcScope.Complete();
                        return response;
                    }
                    catch (AggregateException ex)
                    {
                        if (ex.InnerException != null)
                        {
                            rpcScope.CompleteWithException(ex.InnerException);
                            throw ex.InnerException;
                        }

                        rpcScope.CompleteWithException(ex);
                        throw;
                    }
                },
                TaskScheduler.Current);

            return new AsyncUnaryCall<TResponse>(
                responseAsync,
                responseContinuation.ResponseHeadersAsync,
                responseContinuation.GetStatus,
                responseContinuation.GetTrailers,
                responseContinuation.WithBestEffortDispose(rpcScope));
        }
        catch (Exception e)
        {
            rpcScope?.CompleteWithException(e);
            throw;
        }
        finally
        {
            rpcScope?.RestoreParentActivity();
        }
    }

    /// <inheritdoc/>
    public override AsyncClientStreamingCall<TRequest, TResponse> AsyncClientStreamingCall<TRequest, TResponse>(
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncClientStreamingCallContinuation<TRequest, TResponse> continuation)
    {
        Guard.ThrowIfNull(context);
        Guard.ThrowIfNull(continuation);

        ClientRpcScope<TRequest, TResponse>? rpcScope = null;

        try
        {
#pragma warning disable CA2000
            rpcScope = new ClientRpcScope<TRequest, TResponse>(context, this.options);
#pragma warning restore CA2000
            var responseContinuation = continuation(rpcScope.Context);
            var clientRequestStreamProxy = new ClientStreamWriterProxy<TRequest>(
                responseContinuation.RequestStream,
                rpcScope.RecordRequest,
                onException: rpcScope.CompleteWithException);

            var responseAsync = responseContinuation.ResponseAsync.ContinueWith(
                responseTask =>
                {
                    try
                    {
                        var response = responseTask.Result;
                        rpcScope.RecordResponse(response);
                        rpcScope.Complete();
                        return response;
                    }
                    catch (AggregateException ex)
                    {
                        if (ex.InnerException != null)
                        {
                            rpcScope.CompleteWithException(ex.InnerException);
                            throw ex.InnerException;
                        }

                        rpcScope.CompleteWithException(ex);
                        throw;
                    }
                },
                TaskScheduler.Current);

            return new AsyncClientStreamingCall<TRequest, TResponse>(
                clientRequestStreamProxy,
                responseAsync,
                responseContinuation.ResponseHeadersAsync,
                responseContinuation.GetStatus,
                responseContinuation.GetTrailers,
                responseContinuation.WithBestEffortDispose(rpcScope));
        }
        catch (Exception e)
        {
            rpcScope?.CompleteWithException(e);
            throw;
        }
        finally
        {
            rpcScope?.RestoreParentActivity();
        }
    }

    /// <inheritdoc/>
    public override AsyncServerStreamingCall<TResponse> AsyncServerStreamingCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncServerStreamingCallContinuation<TRequest, TResponse> continuation)
    {
        Guard.ThrowIfNull(context);
        Guard.ThrowIfNull(continuation);

        ClientRpcScope<TRequest, TResponse>? rpcScope = null;

        try
        {
#pragma warning disable CA2000
            rpcScope = new ClientRpcScope<TRequest, TResponse>(context, this.options);
#pragma warning restore CA2000
            rpcScope.RecordRequest(request);
            var responseContinuation = continuation(request, rpcScope.Context);

            var responseStreamProxy = new AsyncStreamReaderProxy<TResponse>(
                responseContinuation.ResponseStream,
                rpcScope.RecordResponse,
                rpcScope.Complete,
                rpcScope.CompleteWithException);

            return new AsyncServerStreamingCall<TResponse>(
                responseStreamProxy,
                responseContinuation.ResponseHeadersAsync,
                responseContinuation.GetStatus,
                responseContinuation.GetTrailers,
                responseContinuation.WithBestEffortDispose(rpcScope));
        }
        catch (Exception e)
        {
            rpcScope?.CompleteWithException(e);
            throw;
        }
        finally
        {
            rpcScope?.RestoreParentActivity();
        }
    }

    /// <inheritdoc/>
    public override AsyncDuplexStreamingCall<TRequest, TResponse> AsyncDuplexStreamingCall<TRequest, TResponse>(
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncDuplexStreamingCallContinuation<TRequest, TResponse> continuation)
    {
        Guard.ThrowIfNull(context);
        Guard.ThrowIfNull(continuation);

        ClientRpcScope<TRequest, TResponse>? rpcScope = null;

        try
        {
#pragma warning disable CA2000
            rpcScope = new ClientRpcScope<TRequest, TResponse>(context, this.options);
#pragma warning restore CA2000
            var responseContinuation = continuation(rpcScope.Context);

            var requestStreamProxy = new ClientStreamWriterProxy<TRequest>(
                responseContinuation.RequestStream,
                rpcScope.RecordRequest,
                onException: rpcScope.CompleteWithException);

            var responseStreamProxy = new AsyncStreamReaderProxy<TResponse>(
                responseContinuation.ResponseStream,
                rpcScope.RecordResponse,
                rpcScope.Complete,
                rpcScope.CompleteWithException);

            return new AsyncDuplexStreamingCall<TRequest, TResponse>(
                requestStreamProxy,
                responseStreamProxy,
                responseContinuation.ResponseHeadersAsync,
                responseContinuation.GetStatus,
                responseContinuation.GetTrailers,
                responseContinuation.WithBestEffortDispose(rpcScope));
        }
        catch (Exception e)
        {
            rpcScope?.CompleteWithException(e);
            throw;
        }
        finally
        {
            rpcScope?.RestoreParentActivity();
        }
    }

    /// <summary>
    /// A class to help track the lifetime of a client-side RPC.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    private sealed class ClientRpcScope<TRequest, TResponse> : RpcScope<TRequest, TResponse>
        where TRequest : class
        where TResponse : class
    {
        /// <summary>
        /// The metadata setter action.
        /// </summary>
        private static readonly Action<Metadata?, string, string> MetadataSetter = (metadata, key, value) => { metadata?.Add(new Metadata.Entry(key, value)); };

        /// <summary>
        /// The context.
        /// </summary>
        private readonly ClientInterceptorContext<TRequest, TResponse> context;

        /// <summary>
        /// The parent activity.
        /// </summary>
        private readonly Activity? parentActivity;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientRpcScope{TRequest, TResponse}" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="options">The options.</param>
        public ClientRpcScope(ClientInterceptorContext<TRequest, TResponse> context, ClientTracingInterceptorOptions options)
            : base(context.Method?.FullName, options.RecordMessageEvents, options.RecordException)
        {
            this.context = context;

            // Capture the current activity.
            this.parentActivity = Activity.Current;

            // Short-circuit if nobody is listening
            if (!GrpcCoreInstrumentation.ActivitySource.HasListeners())
            {
                return;
            }

            // We want to start an activity but don't activate it.
            // After calling StartActivity, Activity.Current will be the new Activity.
            // This scope is created synchronously before the RPC invocation starts and so this new Activity will overwrite
            // the callers current Activity which isn't what we want. We need to restore the original immediately after doing this.
            // If this call happened after some kind of async context await then a restore wouldn't be necessary.
            // gRPC Core just doesn't have the hooks to do this as far as I can tell.
            var rpcActivity = GrpcCoreInstrumentation.ActivitySource.StartActivity(
                this.FullServiceName,
                ActivityKind.Client,
                this.parentActivity == default ? default : this.parentActivity.Context,
                tags: options.AdditionalTags);

            if (rpcActivity == null)
            {
                return;
            }

            var callOptions = context.Options;

            // Do NOT mutate incoming call headers, make a new copy.
            // Retry mechanisms that may sit above this interceptor rely on an original set of call headers.
            var metadata = new Metadata();
            if (callOptions.Headers != null)
            {
                for (var i = 0; i < callOptions.Headers.Count; i++)
                {
                    metadata.Add(callOptions.Headers[i]);
                }
            }

            // replace the CallOptions
            callOptions = callOptions.WithHeaders(metadata);

            this.SetActivity(rpcActivity);
            options.Propagator.Inject(new PropagationContext(rpcActivity.Context, Baggage.Current), callOptions.Headers, MetadataSetter);
            this.context = new ClientInterceptorContext<TRequest, TResponse>(context.Method!, context.Host, callOptions);
        }

        /// <summary>
        /// Gets the context.
        /// </summary>
        public ClientInterceptorContext<TRequest, TResponse> Context => this.context;

        /// <summary>
        /// Restores the parent activity.
        /// </summary>
        public void RestoreParentActivity()
        {
            Activity.Current = this.parentActivity;
        }
    }
}
