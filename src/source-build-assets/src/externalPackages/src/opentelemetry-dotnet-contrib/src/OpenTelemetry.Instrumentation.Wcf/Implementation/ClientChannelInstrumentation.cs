// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry.Internal;
using OpenTelemetry.Trace;

namespace OpenTelemetry.Instrumentation.Wcf.Implementation;

internal static class ClientChannelInstrumentation
{
    public static RequestTelemetryState BeforeSendRequest(Message request, Uri? remoteChannelAddress)
    {
        if (!ShouldInstrumentRequest(request, out var options))
        {
            return new() { SuppressionScope = SuppressDownstreamInstrumentation(options) };
        }

        var action = request.Headers.Action ?? string.Empty;
        var actionMetadata = GetActionMetadata(request, action);
        var remoteAddressUri = request.Headers.To ?? remoteChannelAddress;

        // Add RPC and network tags at span creation time so that they are available for sampling decisions.
        // See https://github.com/open-telemetry/semantic-conventions/blob/v1.42.0/docs/rpc/rpc-spans.md.
        var activitySource = WcfInstrumentationActivitySource.Get(options);
        var activity = activitySource.StartActivity(
            WcfInstrumentationActivitySource.OutgoingRequestActivityName,
            ActivityKind.Client,
            parentContext: default,
            tags: CreateActivityTags(options, actionMetadata, remoteAddressUri));

        var suppressionScope = SuppressDownstreamInstrumentation(options);

        if (activity != null)
        {
            if (!string.IsNullOrEmpty(action))
            {
                activity.DisplayName = action;
            }

            Propagators.DefaultTextMapPropagator.Inject(
                new PropagationContext(activity.Context, Baggage.Current),
                request,
                WcfInstrumentationActivitySource.MessageHeaderValueSetter);

            if (activity.IsAllDataRequested)
            {
                if (options?.SetSoapMessageVersion == true)
                {
                    activity.SetTag(WcfInstrumentationConstants.AttributeSoapMessageVersion, request.Version.ToString());
                }

                if (request.Properties.Via != null)
                {
                    activity.SetTag(WcfInstrumentationConstants.AttributeSoapVia, request.Properties.Via.ToString());
                }

                if (options?.Enrich is { } enrich)
                {
                    try
                    {
                        enrich(activity, WcfEnrichEventNames.BeforeSendRequest, request);
                    }
                    catch (Exception ex)
                    {
                        WcfInstrumentationEventSource.Log.EnrichmentException(ex);
                    }
                }
            }
        }

        return new RequestTelemetryState
        {
            SuppressionScope = suppressionScope,
            Activity = activity,
        };
    }

    public static void AfterRequestCompleted(Message? reply, RequestTelemetryState? state, Exception? exception = null)
    {
        Guard.ThrowIfNull(state);
        state.SuppressionScope?.Dispose();

        if (state.Activity is Activity activity)
        {
            var options = WcfInstrumentationActivitySource.Options;

            if (activity.IsAllDataRequested)
            {
                if (exception is FaultException fault && options?.EmitNewRpcAttributes is true)
                {
                    activity.SetTag(SemanticConventions.AttributeRpcResponseStatusCode, fault.Code.Name);
                }

                if (reply == null || reply.IsFault)
                {
                    activity.SetStatus(ActivityStatusCode.Error);

                    if (options?.EmitNewRpcAttributes is true)
                    {
                        // error.type is conditionally required when the operation has failed.
                        // See https://github.com/open-telemetry/semantic-conventions/blob/v1.42.0/docs/rpc/rpc-spans.md
                        var errorType = (exception as FaultException)?.Code.Name
                            ?? exception?.GetType().FullName
                            ?? WcfInstrumentationConstants.ErrorTypeOther;
                        activity.SetTag(SemanticConventions.AttributeErrorType, errorType);
                    }

                    if (options?.RecordException == true && exception != null)
                    {
                        activity.AddException(exception);
                    }
                }

                if (reply != null)
                {
                    activity.SetTag(WcfInstrumentationConstants.AttributeSoapReplyAction, reply.Headers.Action);

                    if (options?.Enrich is { } enrich)
                    {
                        try
                        {
                            enrich(activity, WcfEnrichEventNames.AfterReceiveReply, reply);
                        }
                        catch (Exception ex)
                        {
                            WcfInstrumentationEventSource.Log.EnrichmentException(ex);
                        }
                    }
                }
            }

            activity.Stop();
        }
    }

    private static List<KeyValuePair<string, object?>> CreateActivityTags(
        WcfInstrumentationOptions? options,
        ActionMetadata actionMetadata,
        Uri? remoteAddressUri)
    {
        var tags = new List<KeyValuePair<string, object?>>();

        if (options?.EmitOldRpcAttributes is true)
        {
            tags.Add(new(SemanticConventions.AttributeRpcSystem, WcfInstrumentationConstants.WcfSystemValue));
            tags.Add(new(SemanticConventions.AttributeRpcService, actionMetadata.ContractName));

            if (options.EmitNewRpcAttributes is not true)
            {
                tags.Add(new(SemanticConventions.AttributeRpcMethod, actionMetadata.OperationName));
            }
        }

        if (options?.EmitNewRpcAttributes is true)
        {
            tags.Add(new(SemanticConventions.AttributeRpcMethod, WcfInstrumentationConstants.GetRpcMethod(actionMetadata.ContractName, actionMetadata.OperationName)));
            tags.Add(new(SemanticConventions.AttributeRpcSystemName, WcfInstrumentationConstants.WcfSystemValue));
        }

        if (remoteAddressUri != null)
        {
            if (options?.EmitOldRpcAttributes is true)
            {
                tags.Add(new(SemanticConventions.AttributeNetPeerName, remoteAddressUri.Host));
                tags.Add(new(SemanticConventions.AttributeNetPeerPort, remoteAddressUri.Port));
            }

            if (options?.EmitNewRpcAttributes is true)
            {
                tags.Add(new(SemanticConventions.AttributeServerAddress, remoteAddressUri.Host));
                tags.Add(new(SemanticConventions.AttributeServerPort, remoteAddressUri.Port));

                if (remoteAddressUri.Host is { Length: > 0 } host)
                {
                    var uriHostNameType = Uri.CheckHostName(host);

                    if (uriHostNameType is UriHostNameType.IPv4 or UriHostNameType.IPv6)
                    {
                        tags.Add(new(SemanticConventions.AttributeNetworkPeerAddress, host));
                        tags.Add(new(SemanticConventions.AttributeNetworkPeerPort, remoteAddressUri.Port));
                    }
                }
            }

            tags.Add(new(WcfInstrumentationConstants.AttributeWcfChannelScheme, remoteAddressUri.Scheme));
            tags.Add(new(WcfInstrumentationConstants.AttributeWcfChannelPath, remoteAddressUri.LocalPath));
        }

        return tags;
    }

    private static IDisposable? SuppressDownstreamInstrumentation(WcfInstrumentationOptions? options) =>
        options?.SuppressDownstreamInstrumentation == true
            ? SuppressInstrumentationScope.Begin()
            : null;

    private static ActionMetadata GetActionMetadata(Message request, string action)
    {
        ActionMetadata? actionMetadata = null;
        if (request.Properties.TryGetValue(TelemetryContextMessageProperty.Name, out var telemetryContextProperty))
        {
            var actionMappings = (telemetryContextProperty as TelemetryContextMessageProperty)?.ActionMappings;
            if (actionMappings != null && actionMappings.TryGetValue(action, out var metadata))
            {
                actionMetadata = metadata;
            }
        }

        return actionMetadata ?? new ActionMetadata(
            contractName: null,
            operationName: action);
    }

    private static bool ShouldInstrumentRequest(Message request, out WcfInstrumentationOptions? options)
    {
        options = WcfInstrumentationActivitySource.Options;

        try
        {
            if (options == null || options.OutgoingRequestFilter?.Invoke(request) == false)
            {
                WcfInstrumentationEventSource.Log.RequestIsFilteredOut();
                return false;
            }
        }
        catch (Exception ex)
        {
            WcfInstrumentationEventSource.Log.RequestFilterException(ex);
            return false;
        }

        return true;
    }
}
