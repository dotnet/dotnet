// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NETFRAMEWORK
using System.Diagnostics;
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

        try
        {
            if (WcfInstrumentationActivitySource.Options == null || WcfInstrumentationActivitySource.Options.IncomingRequestFilter?.Invoke(request) == false)
            {
                WcfInstrumentationEventSource.Log.RequestIsFilteredOut();
                return null;
            }
        }
        catch (Exception ex)
        {
            WcfInstrumentationEventSource.Log.RequestFilterException(ex);
            return null;
        }

        var textMapPropagator = Propagators.DefaultTextMapPropagator;
        var ctx = textMapPropagator.Extract(default, request, WcfInstrumentationActivitySource.MessageHeaderValuesGetter);

        var activity = WcfInstrumentationActivitySource.ActivitySource.StartActivity(
            WcfInstrumentationActivitySource.IncomingRequestActivityName,
            ActivityKind.Server,
            ctx.ActivityContext);

        if (activity != null)
        {
            string action;
            if (!string.IsNullOrEmpty(request.Headers.Action))
            {
                action = request.Headers.Action;
                activity.DisplayName = action;
            }
            else
            {
                action = string.Empty;
            }

            if (activity.IsAllDataRequested)
            {
                activity.SetTag(SemanticConventions.AttributeRpcSystem, WcfInstrumentationConstants.WcfSystemValue);

                if (!this.actionMappings.TryGetValue(action, out var actionMetadata))
                {
                    actionMetadata = new ActionMetadata(
                        contractName: null,
                        operationName: action);
                }

                activity.SetTag(SemanticConventions.AttributeRpcService, actionMetadata.ContractName);
                activity.SetTag(SemanticConventions.AttributeRpcMethod, actionMetadata.OperationName);

                if (WcfInstrumentationActivitySource.Options.SetSoapMessageVersion)
                {
                    activity.SetTag(WcfInstrumentationConstants.AttributeSoapMessageVersion, request.Version.ToString());
                }

                var localAddressUri = channel.LocalAddress?.Uri;
                if (localAddressUri != null)
                {
                    activity.SetTag(SemanticConventions.AttributeNetHostName, localAddressUri.Host);
                    activity.SetTag(SemanticConventions.AttributeNetHostPort, localAddressUri.Port);
                    activity.SetTag(WcfInstrumentationConstants.AttributeWcfChannelScheme, localAddressUri.Scheme);
                    activity.SetTag(WcfInstrumentationConstants.AttributeWcfChannelPath, localAddressUri.LocalPath);
                }

                try
                {
                    WcfInstrumentationActivitySource.Options.Enrich?.Invoke(activity, WcfEnrichEventNames.AfterReceiveRequest, request);
                }
                catch (Exception ex)
                {
                    WcfInstrumentationEventSource.Log.EnrichmentException(ex);
                }
            }

            if (WcfInstrumentationActivitySource.Options.RecordException)
            {
                OperationContext.Current?.Extensions.Add(new WcfOperationContext(activity));
            }

            if (textMapPropagator is not TraceContextPropagator)
            {
                Baggage.Current = ctx.Baggage;
            }
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
                if (reply.IsFault)
                {
                    activity.SetStatus(ActivityStatusCode.Error);
                }

                activity.SetTag(WcfInstrumentationConstants.AttributeSoapReplyAction, reply.Headers.Action);
                try
                {
                    WcfInstrumentationActivitySource.Options!.Enrich?.Invoke(activity, WcfEnrichEventNames.BeforeSendReply, reply);
                }
                catch (Exception ex)
                {
                    WcfInstrumentationEventSource.Log.EnrichmentException(ex);
                }
            }

            activity.Stop();

            if (Propagators.DefaultTextMapPropagator is not TraceContextPropagator)
            {
                Baggage.Current = default;
            }
        }
    }
}

#endif
