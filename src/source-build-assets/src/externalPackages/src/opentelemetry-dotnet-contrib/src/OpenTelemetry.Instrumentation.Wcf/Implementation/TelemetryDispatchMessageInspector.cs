// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NETFRAMEWORK
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry.Internal;
using OpenTelemetry.Trace;

namespace OpenTelemetry.Instrumentation.Wcf.Implementation;

/// <summary>
/// An <see cref="IDispatchMessageInspector"/> implementation which adds telemetry to incoming requests.
/// </summary>
internal class TelemetryDispatchMessageInspector : IDispatchMessageInspector
{
    private readonly IDictionary<string, ActionMetadata> actionMappings;

    internal TelemetryDispatchMessageInspector(IDictionary<string, ActionMetadata> actionMappings)
    {
        Guard.ThrowIfNull(actionMappings);

        this.actionMappings = actionMappings;
    }

    /// <inheritdoc/>
    public object? AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
    {
        Guard.ThrowIfNull(request);
        Guard.ThrowIfNull(channel);

        WcfInstrumentationOptions options;

        try
        {
            if (WcfInstrumentationActivitySource.Options is not { } wcfOptions ||
                wcfOptions.IncomingRequestFilter?.Invoke(request) == false)
            {
                WcfInstrumentationEventSource.Log.RequestIsFilteredOut();
                return null;
            }

            options = wcfOptions;
        }
        catch (Exception ex)
        {
            WcfInstrumentationEventSource.Log.RequestFilterException(ex);
            return null;
        }

        var textMapPropagator = Propagators.DefaultTextMapPropagator;
        var ctx = textMapPropagator.Extract(default, request, WcfInstrumentationActivitySource.MessageHeaderValuesGetter);

        var action = request.Headers.Action ?? string.Empty;
        if (!this.actionMappings.TryGetValue(action, out var actionMetadata))
        {
            actionMetadata = new ActionMetadata(
                contractName: null,
                operationName: action);
        }

        // Add RPC and network tags at span creation time so that they are available for sampling decisions.
        // See https://github.com/open-telemetry/semantic-conventions/blob/v1.42.0/docs/rpc/rpc-spans.md.
        var activitySource = WcfInstrumentationActivitySource.Get(options);
        var activity = activitySource.StartActivity(
            WcfInstrumentationActivitySource.IncomingRequestActivityName,
            ActivityKind.Server,
            ctx.ActivityContext,
            CreateActivityTags(options, actionMetadata, request, channel));

        if (activity != null)
        {
            if (!string.IsNullOrEmpty(action))
            {
                activity.DisplayName = action;
            }

            if (activity.IsAllDataRequested)
            {
                if (options.SetSoapMessageVersion)
                {
                    activity.SetTag(WcfInstrumentationConstants.AttributeSoapMessageVersion, request.Version.ToString());
                }

                if (options?.Enrich is { } enrich)
                {
                    try
                    {
                        enrich(activity, WcfEnrichEventNames.AfterReceiveRequest, request);
                    }
                    catch (Exception ex)
                    {
                        WcfInstrumentationEventSource.Log.EnrichmentException(ex);
                    }
                }
            }

            if (textMapPropagator is not TraceContextPropagator)
            {
                Baggage.Current = ctx.Baggage;
            }
        }

        if (options?.RecordException == true)
        {
            OperationContext.Current?.Extensions.Add(new WcfOperationContext(activity));
        }

        return activity;
    }

    /// <inheritdoc/>
    public void BeforeSendReply(ref Message reply, object? correlationState)
    {
        if (correlationState is Activity activity)
        {
            if (activity.IsAllDataRequested && reply != null)
            {
                var options = WcfInstrumentationActivitySource.Options;

                if (reply.IsFault)
                {
                    activity.SetStatus(ActivityStatusCode.Error);

                    if (options?.EmitNewRpcAttributes == true)
                    {
                        string? statusCode = null;
                        if (TryGetHttpResponseMessageProperty(OperationContext.Current?.OutgoingMessageProperties, out var response) ||
                            TryGetHttpResponseMessageProperty(reply.Properties, out response))
                        {
                            statusCode = ((int)response.StatusCode).ToString(CultureInfo.InvariantCulture);
                            activity.SetTag(SemanticConventions.AttributeRpcResponseStatusCode, statusCode);
                        }

                        // error.type is conditionally required when the operation has failed.
                        // See https://github.com/open-telemetry/semantic-conventions/blob/v1.42.0/docs/rpc/rpc-spans.md
                        activity.SetTag(SemanticConventions.AttributeErrorType, statusCode ?? WcfInstrumentationConstants.ErrorTypeOther);
                    }
                }

                activity.SetTag(WcfInstrumentationConstants.AttributeSoapReplyAction, reply.Headers.Action);

                if (options?.Enrich is { } enrich)
                {
                    try
                    {
                        enrich(activity, WcfEnrichEventNames.BeforeSendReply, reply);
                    }
                    catch (Exception ex)
                    {
                        WcfInstrumentationEventSource.Log.EnrichmentException(ex);
                    }
                }
            }

            activity.Stop();

            if (Propagators.DefaultTextMapPropagator is not TraceContextPropagator)
            {
                Baggage.Current = default;
            }
        }
    }

    private static bool TryGetHttpResponseMessageProperty(MessageProperties? properties, [NotNullWhen(true)] out HttpResponseMessageProperty? response)
    {
        response = null;
        if (properties?.TryGetValue(HttpResponseMessageProperty.Name, out var property) == true &&
            property is HttpResponseMessageProperty httpResponse)
        {
            response = httpResponse;
            return true;
        }

        return false;
    }

    private static List<KeyValuePair<string, object?>> CreateActivityTags(
        WcfInstrumentationOptions options,
        ActionMetadata actionMetadata,
        Message request,
        IClientChannel channel)
    {
        var tags = new List<KeyValuePair<string, object?>>();

        if (options.EmitOldRpcAttributes)
        {
            tags.Add(new(SemanticConventions.AttributeRpcSystem, WcfInstrumentationConstants.WcfSystemValue));
            tags.Add(new(SemanticConventions.AttributeRpcService, actionMetadata.ContractName));

            if (!options.EmitNewRpcAttributes)
            {
                tags.Add(new(SemanticConventions.AttributeRpcMethod, actionMetadata.OperationName));
            }
        }

        if (options.EmitNewRpcAttributes)
        {
            tags.Add(new(SemanticConventions.AttributeRpcMethod, WcfInstrumentationConstants.GetRpcMethod(actionMetadata.ContractName, actionMetadata.OperationName)));
            tags.Add(new(SemanticConventions.AttributeRpcSystemName, WcfInstrumentationConstants.WcfSystemValue));
        }

        var localAddressUri = channel.LocalAddress?.Uri;
        if (localAddressUri != null)
        {
            if (options.EmitOldRpcAttributes)
            {
                tags.Add(new(SemanticConventions.AttributeNetHostName, localAddressUri.Host));
                tags.Add(new(SemanticConventions.AttributeNetHostPort, localAddressUri.Port));
            }

            if (options.EmitNewRpcAttributes)
            {
                tags.Add(new(SemanticConventions.AttributeServerAddress, localAddressUri.Host));
                tags.Add(new(SemanticConventions.AttributeServerPort, localAddressUri.Port));
            }

            tags.Add(new(WcfInstrumentationConstants.AttributeWcfChannelScheme, localAddressUri.Scheme));
            tags.Add(new(WcfInstrumentationConstants.AttributeWcfChannelPath, localAddressUri.LocalPath));
        }

        // On a server span, network.peer.* describes the remote client peer rather than the local
        // listen address, so it is derived from the transport's remote endpoint when available.
        if (options.EmitNewRpcAttributes &&
            request.Properties.TryGetValue(RemoteEndpointMessageProperty.Name, out var property) &&
            property is RemoteEndpointMessageProperty remoteEndpoint &&
            !string.IsNullOrEmpty(remoteEndpoint.Address))
        {
            tags.Add(new(SemanticConventions.AttributeNetworkPeerAddress, remoteEndpoint.Address));
            tags.Add(new(SemanticConventions.AttributeNetworkPeerPort, remoteEndpoint.Port));
        }

        return tags;
    }
}

#endif
