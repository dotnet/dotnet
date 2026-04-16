// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using Microsoft.ServiceFabric.Services.Remoting.V2;
using Microsoft.ServiceFabric.Services.Remoting.V2.Runtime;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry.Internal;
using OpenTelemetry.Trace;

namespace OpenTelemetry.Instrumentation.ServiceFabricRemoting;

/// <summary>
/// Provides an implementation of <see cref="IServiceRemotingMessageHandler"/> that can dispatch
/// messages to an actor service and to the actors hosted in the service.
/// </summary>
public sealed class ServiceRemotingMessageDispatcherAdapter : IServiceRemotingMessageHandler, IDisposable
{
    private static readonly TextMapPropagator Propagator = Propagators.DefaultTextMapPropagator;

    private readonly IServiceRemotingMessageHandler innerDispatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceRemotingMessageDispatcherAdapter"/> class.
    /// </summary>
    /// <param name="dispatcher">The IServiceRemotingMessageHandler to wrap.</param>
    public ServiceRemotingMessageDispatcherAdapter(IServiceRemotingMessageHandler dispatcher)
    {
        Guard.ThrowIfNull(dispatcher);

        this.innerDispatcher = dispatcher;
    }

    /// <summary>
    /// Gets a factory for creating the remoting message bodies.
    /// </summary>
    /// <returns>A factory for creating the remoting message bodies.</returns>
    public IServiceRemotingMessageBodyFactory GetRemotingMessageBodyFactory()
    {
        return this.innerDispatcher.GetRemotingMessageBodyFactory();
    }

    /// <summary>
    /// Handles a one way message from the client.
    /// </summary>
    /// <param name="requestMessage">The request message.</param>
    public void HandleOneWayMessage(IServiceRemotingRequestMessage requestMessage)
    {
        this.innerDispatcher.HandleOneWayMessage(requestMessage);
    }

    /// <summary>
    /// Dispatches the messages received from the client to the actor service methods or the actor methods.
    /// This can be used by user where they know interfaceId and MethodId for the method to dispatch to.
    /// </summary>
    /// <param name="requestContext">Request context that allows getting the callback channel if required.</param>
    /// <param name="requestMessage">Remoting message.</param>
    /// <returns>The response for the received request.</returns>
    public async Task<IServiceRemotingResponseMessage> HandleRequestResponseAsync(IServiceRemotingRequestContext requestContext, IServiceRemotingRequestMessage requestMessage)
    {
        Guard.ThrowIfNull(requestMessage);

        if (ServiceFabricRemotingActivitySource.Options?.Filter?.Invoke(requestMessage) == false)
        {
            // If we filter out the request we don't need to process anything related to the activity
            return await this.innerDispatcher.HandleRequestResponseAsync(requestContext, requestMessage).ConfigureAwait(false);
        }
        else
        {
            IServiceRemotingRequestMessageHeader requestMessageHeader = requestMessage.GetHeader();
            Guard.ThrowIfNull(requestMessageHeader, "requestMessage.GetHeader()");

            // Extract the PropagationContext of the upstream parent from the message headers.
            PropagationContext parentContext = Propagator.Extract(default, requestMessageHeader, ServiceFabricRemotingUtils.ExtractTraceContextFromRequestMessageHeader);
            Baggage.Current = parentContext.Baggage;

            string activityName = requestMessageHeader?.MethodName ?? ServiceFabricRemotingActivitySource.IncomingRequestActivityName;

            using (Activity? activity = ServiceFabricRemotingActivitySource.ActivitySource.StartActivity(activityName, ActivityKind.Server, parentContext.ActivityContext))
            {
                try
                {
                    IServiceRemotingResponseMessage responseMessage = await this.innerDispatcher.HandleRequestResponseAsync(requestContext, requestMessage).ConfigureAwait(false);

                    return responseMessage;
                }
                catch (Exception ex)
                {
                    if (activity != null)
                    {
                        activity.SetStatus(ActivityStatusCode.Error);

                        if (ServiceFabricRemotingActivitySource.Options?.AddExceptionAtServer == true)
                        {
                            activity.RecordException(ex);
                        }
                    }

                    throw;
                }
            }
        }
    }

    /// <summary>
    ///  Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        if (this.innerDispatcher is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}
