// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using Google.Protobuf;
using Grpc.Core;
using OpenTelemetry.Internal;
using OpenTelemetry.Trace;
using StatusCode = Grpc.Core.StatusCode;

namespace OpenTelemetry.Instrumentation.GrpcCore;

/// <summary>
/// A class to help track the lifetime of an RPC.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
internal abstract class RpcScope<TRequest, TResponse> : IDisposable
    where TRequest : class
    where TResponse : class
{
    /// <summary>
    /// The host.
    /// </summary>
    private readonly string? host;

    /// <summary>
    /// The record message events flag.
    /// </summary>
    private readonly bool recordMessageEvents;

    /// <summary>
    /// The record exception as ActivityEvent flag.
    /// </summary>
    private readonly bool recordException;

    /// <summary>
    /// The RPC activity.
    /// </summary>
    private Activity? activity;

    /// <summary>
    /// The complete flag.
    /// </summary>
    private long complete;

    /// <summary>
    /// The request message counter.
    /// </summary>
    private int requestMessageCounter;

    /// <summary>
    /// The response counter.
    /// </summary>
    private int responseMessageCounter;

    /// <summary>
    /// Initializes a new instance of the <see cref="RpcScope{TRequest, TResponse}" /> class.
    /// </summary>
    /// <param name="host">The host that the current invocation will be dispatched to.</param>
    /// <param name="fullServiceName">Full name of the service.</param>
    /// <param name="recordMessageEvents">if set to <c>true</c> [record message events].</param>
    /// <param name="recordException">If set to <c>true</c> [record exception].</param>
    protected RpcScope(
        string? host,
        string? fullServiceName,
        bool recordMessageEvents,
        bool recordException)
    {
        this.host = host;
        this.FullServiceName = fullServiceName?.Trim('/') ?? "unknownservice/unknownmethod";
        this.recordMessageEvents = recordMessageEvents;
        this.recordException = recordException;
    }

    /// <summary>
    /// Gets the full name of the service.
    /// </summary>
    protected string FullServiceName { get; }

    /// <summary>
    /// Records a request message.
    /// </summary>
    /// <param name="request">The request.</param>
    public void RecordRequest(TRequest request)
    {
        Guard.ThrowIfNull(request);

        this.requestMessageCounter++;

        if (this.activity == null || !this.activity.IsAllDataRequested || !this.recordMessageEvents)
        {
            return;
        }

        if (request is IMessage message)
        {
            this.AddMessageEvent(typeof(TRequest).Name, message, request: true);
        }
    }

    /// <summary>
    /// Records a response message.
    /// </summary>
    /// <param name="response">The response.</param>
    public void RecordResponse(TResponse response)
    {
        Guard.ThrowIfNull(response);

        this.responseMessageCounter++;

        if (this.activity == null || !this.activity.IsAllDataRequested || !this.recordMessageEvents)
        {
            return;
        }

        if (response is IMessage message)
        {
            this.AddMessageEvent(typeof(TResponse).Name, message, request: false);
        }
    }

    /// <summary>
    /// Completes the RPC.
    /// </summary>
    public void Complete()
    {
        if (this.activity == null)
        {
            return;
        }

        // The overall Span status should remain unset however the grpc status code attribute is required
        this.StopActivity((int)StatusCode.OK);
    }

    /// <summary>
    /// Records a failed RPC.
    /// </summary>
    /// <param name="exception">The exception.</param>
    public void CompleteWithException(Exception exception)
    {
        Guard.ThrowIfNull(exception);

        if (this.activity == null)
        {
            return;
        }

        this.StopActivity(exception);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (this.activity == null)
        {
            return;
        }

        // If not already completed this will mark the Activity as cancelled.
        this.StopActivity((int)StatusCode.Cancelled);
    }

    /// <summary>
    /// Sets the activity for this RPC scope. Should only be called once.
    /// </summary>
    /// <param name="activity">The activity.</param>
    protected void SetActivity(Activity? activity)
    {
        this.activity = activity;

        if (this.activity == null || !this.activity.IsAllDataRequested)
        {
            return;
        }

        GrpcTagHelper.SetGrpcSystemName(this.activity);
        GrpcTagHelper.SetGrpcMethodAndDisplayNameFromActivity(this.activity, this.FullServiceName);

        if (this.host is { Length: > 0 } host)
        {
            TrySetServerAttributes(this.activity, host);
        }
    }

    private static void TrySetServerAttributes(Activity activity, string host)
    {
        // Add a placeholder for the scheme so the parse succeeds. The value is not important.
        if (!Uri.TryCreate($"http://{host}", UriKind.Absolute, out var uri))
        {
            return;
        }

        activity.SetTag(SemanticConventions.AttributeServerAddress, uri.Host);

        if (!uri.IsDefaultPort)
        {
            activity.SetTag(SemanticConventions.AttributeServerPort, uri.Port);
        }
    }

    /// <summary>
    /// Stops the activity.
    /// </summary>
    /// <param name="statusCode">The status code.</param>
    /// <param name="markAsCompleted">If set to <c>true</c> [mark as completed].</param>
    /// <param name="statusDescription">The status description to set when the span is marked as failed, if any.</param>
    private void StopActivity(int statusCode, bool markAsCompleted = true, string? statusDescription = null)
    {
        if ((markAsCompleted && !this.TryMarkAsCompleted()) || this.activity is null)
        {
            return;
        }

        var grpcStatusName = GrpcTagHelper.GetGrpcStatusCodeName(statusCode);
        this.activity.SetTag(SemanticConventions.AttributeRpcResponseStatusCode, grpcStatusName);

        // Resolve the span status from the gRPC status code using the rules for the span kind:
        // client spans treat every non-OK status as an error, whereas server spans only treat a
        // subset of status codes as errors.
        // See https://github.com/open-telemetry/semantic-conventions/blob/v1.42.0/docs/rpc/grpc.md
        var spanStatus = this.activity.Kind == ActivityKind.Client
            ? GrpcTagHelper.ResolveSpanStatusForGrpcStatusCodeOnClient(statusCode)
            : GrpcTagHelper.ResolveSpanStatusForGrpcStatusCodeOnServer(statusCode);

        if (spanStatus == ActivityStatusCode.Error)
        {
            this.activity.SetStatus(spanStatus, statusDescription);

            // error.type is conditionally required when the operation failed; for gRPC it is set to
            // the status code name.
            this.activity.SetTag(SemanticConventions.AttributeErrorType, grpcStatusName);
        }

        this.activity.Stop();
    }

    /// <summary>
    /// Stops the activity.
    /// </summary>
    /// <param name="exception">The exception.</param>
    private void StopActivity(Exception exception)
    {
        if (!this.TryMarkAsCompleted() || this.activity is null)
        {
            return;
        }

        var grpcStatusCode = StatusCode.Unknown;
        var description = exception.Message;

        if (exception is RpcException rpcException)
        {
            grpcStatusCode = rpcException.StatusCode;
            description = rpcException.Message;
        }

        if (this.activity.IsAllDataRequested && this.recordException)
        {
            this.activity.AddException(exception);
        }

        // Defer to StopActivity to apply the span-kind specific status rules. The status description
        // is only used when the resolved span status is an error so that, for example, server spans
        // do not report an error status for status codes the conventions consider successful.
        this.StopActivity((int)grpcStatusCode, markAsCompleted: false, statusDescription: description);
    }

    /// <summary>
    /// Tries to mark <see cref="RpcScope{TRequest, TResponse}"/> as completed.
    /// </summary>
    /// <returns>Returns <c>true</c> if marked as completed successfully.</returns>
    private bool TryMarkAsCompleted()
        => Interlocked.CompareExchange(ref this.complete, 1, 0) == 0;

    /// <summary>
    /// Adds a message event.
    /// </summary>
    /// <param name="eventName">Name of the event.</param>
    /// <param name="message">The message.</param>
    /// <param name="request">if true this is a request message.</param>
    private void AddMessageEvent(string eventName, IMessage? message, bool request)
    {
        if (message == null || this.activity is null)
        {
            return;
        }

        var messageSize = message.CalculateSize();

        var attributes = new ActivityTagsCollection(
        [
            new("name", "message"),
            new(SemanticConventions.AttributeMessageType, request ? "SENT" : "RECEIVED"),
            new(SemanticConventions.AttributeMessageId, request ? this.requestMessageCounter : this.responseMessageCounter),

            // TODO how to get the real compressed or uncompressed sizes
            new(SemanticConventions.AttributeMessageCompressedSize, messageSize),
            new(SemanticConventions.AttributeMessageUncompressedSize, messageSize),
        ]);

        this.activity.AddEvent(new ActivityEvent(eventName, default, attributes));
    }
}
