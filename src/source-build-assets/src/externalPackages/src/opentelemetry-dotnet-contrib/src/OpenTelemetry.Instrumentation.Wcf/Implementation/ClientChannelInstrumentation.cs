// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.ServiceModel.Channels;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Instrumentation.Wcf.Implementation;

internal static class ClientChannelInstrumentation
{
    public static RequestTelemetryState BeforeSendRequest(Message request, Uri? remoteChannelAddress)
    {
        if (!ShouldInstrumentRequest(request))
        {
            return new RequestTelemetryState { SuppressionScope = SuppressDownstreamInstrumentation() };
        }

        var activity = WcfInstrumentationActivitySource.ActivitySource.StartActivity(
            WcfInstrumentationActivitySource.OutgoingRequestActivityName,
            ActivityKind.Client);
        var suppressionScope = SuppressDownstreamInstrumentation();

        if (activity != null)
        {
            var action = string.Empty;
            if (!string.IsNullOrEmpty(request.Headers.Action))
            {
                action = request.Headers.Action;
                activity.DisplayName = action;
            }

            Propagators.DefaultTextMapPropagator.Inject(
                new PropagationContext(activity.Context, Baggage.Current),
                request,
                WcfInstrumentationActivitySource.MessageHeaderValueSetter);

            if (activity.IsAllDataRequested)
            {
                activity.SetTag(WcfInstrumentationConstants.RpcSystemTag, WcfInstrumentationConstants.WcfSystemValue);

                var actionMetadata = GetActionMetadata(request, action);
                activity.SetTag(WcfInstrumentationConstants.RpcServiceTag, actionMetadata.ContractName);
                activity.SetTag(WcfInstrumentationConstants.RpcMethodTag, actionMetadata.OperationName);

                if (WcfInstrumentationActivitySource.Options!.SetSoapMessageVersion)
                {
                    activity.SetTag(WcfInstrumentationConstants.SoapMessageVersionTag, request.Version.ToString());
                }

                var remoteAddressUri = request.Headers.To ?? remoteChannelAddress;
                if (remoteAddressUri != null)
                {
                    activity.SetTag(WcfInstrumentationConstants.NetPeerNameTag, remoteAddressUri.Host);
                    activity.SetTag(WcfInstrumentationConstants.NetPeerPortTag, remoteAddressUri.Port);
                    activity.SetTag(WcfInstrumentationConstants.WcfChannelSchemeTag, remoteAddressUri.Scheme);
                    activity.SetTag(WcfInstrumentationConstants.WcfChannelPathTag, remoteAddressUri.LocalPath);
                }

                if (request.Properties.Via != null)
                {
                    activity.SetTag(WcfInstrumentationConstants.SoapViaTag, request.Properties.Via.ToString());
                }

                try
                {
                    WcfInstrumentationActivitySource.Options.Enrich?.Invoke(activity, WcfEnrichEventNames.BeforeSendRequest, request);
                }
                catch (Exception ex)
                {
                    WcfInstrumentationEventSource.Log.EnrichmentException(ex);
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
            if (activity.IsAllDataRequested)
            {
                if (reply == null || reply.IsFault)
                {
                    activity.SetStatus(ActivityStatusCode.Error);

                    if (WcfInstrumentationActivitySource.Options!.RecordException && exception != null)
                    {
                        activity.AddException(exception);
                    }
                }

                if (reply != null)
                {
                    activity.SetTag(WcfInstrumentationConstants.SoapReplyActionTag, reply.Headers.Action);
                    try
                    {
                        WcfInstrumentationActivitySource.Options!.Enrich?.Invoke(activity, WcfEnrichEventNames.AfterReceiveReply, reply);
                    }
                    catch (Exception ex)
                    {
                        WcfInstrumentationEventSource.Log.EnrichmentException(ex);
                    }
                }
            }

            activity.Stop();
        }
    }

    private static IDisposable? SuppressDownstreamInstrumentation()
    {
        return WcfInstrumentationActivitySource.Options?.SuppressDownstreamInstrumentation ?? false
            ? SuppressInstrumentationScope.Begin()
            : null;
    }

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

    private static bool ShouldInstrumentRequest(Message request)
    {
        try
        {
            if (WcfInstrumentationActivitySource.Options == null || WcfInstrumentationActivitySource.Options.OutgoingRequestFilter?.Invoke(request) == false)
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
