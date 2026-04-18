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
    /// <param name="fullServiceName">Full name of the service.</param>
    /// <param name="recordMessageEvents">if set to <c>true</c> [record message events].</param>
    /// <param name="recordException">If set to <c>true</c> [record exception].</param>
    protected RpcScope(string? fullServiceName, bool recordMessageEvents, bool recordException)
    {
        this.FullServiceName = fullServiceName?.TrimStart('/') ?? "unknownservice/unknownmethod";
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

        this.AddMessageEvent(typeof(TRequest).Name, (request as IMessage)!, request: true);
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

        this.AddMessageEvent(typeof(TResponse).Name, (response as IMessage)!, request: false);
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

        // assign some reasonable defaults
        var rpcService = this.FullServiceName;
        var rpcMethod = this.FullServiceName;

        // split the full service name by the slash
        var parts = this.FullServiceName.Split('/');
        if (parts.Length == 2)
        {
            rpcService = parts[0];
            rpcMethod = parts[1];
        }

        this.activity.SetTag(SemanticConventions.AttributeRpcSystem, "grpc");
        this.activity.SetTag(SemanticConventions.AttributeRpcService, rpcService);
        this.activity.SetTag(SemanticConventions.AttributeRpcMethod, rpcMethod);
    }

    /// <summary>
    /// Stops the activity.
    /// </summary>
    /// <param name="statusCode">The status code.</param>
    /// <param name="markAsCompleted">If set to <c>true</c> [mark as completed].</param>
    private void StopActivity(int statusCode, bool markAsCompleted = true)
    {
        if (markAsCompleted && !this.TryMarkAsCompleted())
        {
            return;
        }

        this.activity!.SetTag(SemanticConventions.AttributeRpcGrpcStatusCode, statusCode);
        this.activity.Stop();
    }

    /// <summary>
    /// Stops the activity.
    /// </summary>
    /// <param name="exception">The exception.</param>
    private void StopActivity(Exception exception)
    {
        if (!this.TryMarkAsCompleted())
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

        if (!string.IsNullOrEmpty(description))
        {
            this.activity!.SetStatus(ActivityStatusCode.Error, description);
        }

        if (this.activity!.IsAllDataRequested && this.recordException)
        {
            this.activity.AddException(exception);
        }

        this.StopActivity((int)grpcStatusCode, markAsCompleted: false);
    }

    /// <summary>
    /// Tries to mark <see cref="RpcScope{TRequest, TResponse}"/> as completed.
    /// </summary>
    /// <returns>Returns <c>true</c> if marked as completed successfully.</returns>
    private bool TryMarkAsCompleted()
    {
        return Interlocked.CompareExchange(ref this.complete, 1, 0) == 0;
    }

    /// <summary>
    /// Adds a message event.
    /// </summary>
    /// <param name="eventName">Name of the event.</param>
    /// <param name="message">The message.</param>
    /// <param name="request">if true this is a request message.</param>
    private void AddMessageEvent(string eventName, IMessage message, bool request)
    {
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

        this.activity!.AddEvent(new ActivityEvent(eventName, default, attributes));
    }
}
