// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry.Contrib.Instrumentation.Remoting.Implementation;
using OpenTelemetry.Trace;

namespace OpenTelemetry.Instrumentation.Remoting.Implementation;

/// <summary>
/// A dynamic remoting sink that intercepts all calls leaving or entering an AppDomain, see
/// <see cref="RemotingInstrumentation"/> constructor: RegisterDynamicProperty
/// code.
/// </summary>
/// <remarks>
/// See https://docs.microsoft.com/previous-versions/dotnet/netframework-4.0/tdzwhfy3(v=vs.100) for
/// information about remoting sinks and https://docs.microsoft.com/dotnet/api/system.runtime.remoting.contexts.context.registerdynamicproperty
/// for RegisterDynamicProperty behavior.
/// </remarks>
internal sealed class TelemetryDynamicSink : IDynamicMessageSink
{
    private const string AttributeRpcSystemNameValue = "dotnet.remoting";
    private readonly ActivitySource remotingActivitySource;
    private readonly string activityInName;
    private readonly string activityOutName;
    private readonly string savedAspnetActivityPropertyName;
    private readonly ConcurrentDictionary<string, string> serviceNameCache;

    private readonly TelemetryDynamicSinkProvider optionsProvider;

    public TelemetryDynamicSink(
        TelemetryDynamicSinkProvider optionsProvider,
        ActivitySource activitySource,
        ConcurrentDictionary<string, string> serviceNameCache)
    {
        this.optionsProvider = optionsProvider;
        this.remotingActivitySource = activitySource;
        this.activityOutName = activitySource.Name + ".RequestOut";
        this.activityInName = activitySource.Name + ".RequestIn";
        this.savedAspnetActivityPropertyName = activitySource.Name + ".SavedAspnetActivity";
        this.serviceNameCache = serviceNameCache;
    }

    public void ProcessMessageStart(IMessage reqMsg, bool bCliSide, bool bAsync)
    {
        var options = this.optionsProvider.Options;

        try
        {
            if (options.Filter?.Invoke(reqMsg) == false)
            {
                return;
            }
        }
        catch (Exception e)
        {
            RemotingInstrumentationEventSource.Log.MessageFilterException(e);
            return;
        }

        try
        {
            IMethodMessage? methodMsg = reqMsg as IMethodMessage;
            ActivityTagsCollection? tags = methodMsg != null
                ? this.BuildSamplingTags(methodMsg)
                : null;

            // Are we executing on client?
            if (bCliSide)
            {
                // The context to inject will either be from the Remoting activity that we will start,
                // or, if the Remoting activity doesn't get sampled, could also be taken from Activity.Current,
                // if it is available.
                ActivityContext contextToInject = default;

                // Start new outgoing activity
                var activity = this.remotingActivitySource.StartActivity(
                    this.activityOutName,
                    ActivityKind.Client,
                    default(ActivityContext),
                    tags);

                if (activity != null)
                {
                    this.SetPostCreationAttributes(activity, methodMsg);

                    contextToInject = activity.Context;
                }
                else if (Activity.Current != null)
                {
                    contextToInject = Activity.Current.Context;
                }

                var callContext = (LogicalCallContext)reqMsg.Properties["__CallContext"];

                options.Propagator.Inject(new PropagationContext(contextToInject, Baggage.Current), callContext, InjectActivityProperties);
            }
            else
            {
                // We are on server, need to start new or attach to an existing incoming activity.

                var callContext = (LogicalCallContext)reqMsg.Properties["__CallContext"];
                var activityParentContext = options.Propagator.Extract(default, callContext, ExtractActivityProperties);

                Activity? ourActivity = null;

                // Do we already have an incoming activity?
                // Existing activity might be started by an instrumentation layer higher up the
                // stack. E.g. if we are using http as a transport for remoting, and we have
                // instrumented incoming http request, then that instrumentation happens before
                // our "ProcessMessageStart".
                var parentActivity = Activity.Current;

                if (parentActivity == null)
                {
                    // We don't have an existing incoming activity. Start a brand new one ourselves using the extracted context.

                    ourActivity = this.remotingActivitySource.StartActivity(
                        this.activityInName,
                        ActivityKind.Server,
                        activityParentContext.ActivityContext,
                        tags);
                }
                else if (parentActivity.TraceId == activityParentContext.ActivityContext.TraceId)
                {
                    // Existing parent activity belongs to the same trace as our remoting context.
                    // This likely means that we are using http as a transport for Remoting and have
                    // instrumented both the client and the server with http instrumentation, resulting
                    // in a call stack like below:

                    // |-------------Client--------------|--------------------------Server----------------------------|
                    // | RemotingClient -> HttpClient -> | -> ASP.NET Instrumentation -> RemotingServer (this method) |

                    // The context was propagated between HttpClient and ASP.NET Instrumentation.
                    // We let this context take over and simply create our Remoting activity as a child of the ASP.NET one.

                    ourActivity = this.remotingActivitySource.StartActivity(
                        this.activityInName,
                        ActivityKind.Server,
                        default(ActivityContext),
                        tags);
                }
                else if (activityParentContext.ActivityContext.IsValid())
                {
                    // We have a valid Remoting context but the existing parent activity has started a different
                    // trace. This could happen in the instrumentation chain set up like below:

                    // |------Client-------|--------------------------Server----------------------------|
                    // | RemotingClient -> | -> ASP.NET Instrumentation -> RemotingServer (this method) |

                    // In this scenario ASP.NET Instrumentation was unable to extract the "correct" context.
                    // We will create our Remoting activity as a "sibling" of the ASP.NET one to maintain
                    // the context. ASP.NET activity is saved as a custom property so that we can restore it later
                    // (see ProcessMessageFinish) to give ASP.NET Instrumentation a chance to stop it.

                    ourActivity = this.remotingActivitySource.StartActivity(
                        this.activityInName,
                        ActivityKind.Server,
                        activityParentContext.ActivityContext,
                        tags);
                    ourActivity?.SetCustomProperty(this.savedAspnetActivityPropertyName, parentActivity);
                }
                else
                {
                    // Got here if we don't have a valid Remoting context. This is possible if the client
                    // didn't have Remoting instrumentation and so didn't initialize the "__CallContext".
                    // We still have a parent activity from another instrumentation, so we keep it as
                    // Current (better than creating a new "root" Remoting activity).
                }

                if (ourActivity != null)
                {
                    this.SetPostCreationAttributes(ourActivity, methodMsg);
                }
            }
        }
        catch (Exception e)
        {
            RemotingInstrumentationEventSource.Log.DynamicSinkException(e);
        }
    }

    public void ProcessMessageFinish(IMessage replyMsg, bool bCliSide, bool bAsync)
    {
        // This will be called:
        // 1) On the server after we are done processing the call and are returning back to client
        //    => Current activity should be the "ActivityIn", we need to stop it
        //
        // 2) On the client, when the server call returns
        //    => Current activity should be the "ActivityOut", we need to stop it as well

        try
        {
            var options = this.optionsProvider.Options;

            var activity = Activity.Current;
            if (activity == null)
            {
                return;
            }

            bool validClientActivity = bCliSide && activity.OperationName == this.activityOutName;
            bool validServerActivity = !bCliSide && activity.OperationName == this.activityInName;
            if (validClientActivity || validServerActivity)
            {
                if (replyMsg is IMethodReturnMessage returnMsg)
                {
                    if (returnMsg.Exception == null)
                    {
                        // Default to "Unset" status as per spec:
                        // https://github.com/open-telemetry/opentelemetry-specification/blob/fc2e1a9ef6472ea8ccbc71d9bff748211a3f5001/specification/trace/api.md#status
                        activity.SetStatus(ActivityStatusCode.Unset);
                    }
                    else
                    {
                        activity.SetStatus(ActivityStatusCode.Error);
                        activity.SetTag(SemanticConventions.AttributeErrorType, returnMsg.Exception.GetType().FullName);
                    }

                    // Call enrich before stopping
                    try
                    {
                        options.EnrichWithMethodReturnMessage?.Invoke(activity, returnMsg);
                    }
                    catch (Exception ex)
                    {
                        RemotingInstrumentationEventSource.Log.MethodReturnMessageEnrichmentException(ex);
                    }
                }

                activity.Stop();

                // Did ProcessMessageStart discard activity created by the ASP.NET Instrumentation?
                // If yes, we need to restore it here, so that it can be stopped by the instrumentation later.
                if (validServerActivity && activity.GetCustomProperty(this.savedAspnetActivityPropertyName) is Activity otherActivity)
                {
                    Activity.Current = otherActivity;
                }
            }
        }
        catch (Exception e)
        {
            RemotingInstrumentationEventSource.Log.DynamicSinkException(e);
        }
    }

    private static bool TryParseUri(string? uri, out string? host, out int? port)
    {
        host = null;
        port = null;

        if (string.IsNullOrEmpty(uri))
        {
            return false;
        }

        if (!Uri.TryCreate(uri, UriKind.Absolute, out Uri? parsedUri))
        {
            return false;
        }

        if (string.IsNullOrEmpty(parsedUri.Host))
        {
            return false;
        }

        host = parsedUri.Host;

        if (!parsedUri.IsDefaultPort)
        {
            port = parsedUri.Port;
        }

        return true;
    }

    private static void InjectActivityProperties(LogicalCallContext ctx, string key, string value) => ctx.SetData(key, value);

    private static IEnumerable<string> ExtractActivityProperties(LogicalCallContext ctx, string key)
    {
        var data = ctx.GetData(key);
        return data != null ? [(string)data] : [];
    }

    private ActivityTagsCollection BuildSamplingTags(IMethodMessage msg)
    {
        var fullyQualifiedMethod = this.GetFullyQualifiedMethod(msg);

        var tags = new ActivityTagsCollection
        {
            { SemanticConventions.AttributeRpcSystemName, AttributeRpcSystemNameValue },
            { SemanticConventions.AttributeRpcMethod, fullyQualifiedMethod },
        };

        if (TryParseUri(msg.Uri, out string? host, out int? port))
        {
            tags.Add(SemanticConventions.AttributeServerAddress, host);
            if (port.HasValue)
            {
                tags.Add(SemanticConventions.AttributeServerPort, port.Value);
            }
        }

        return tags;
    }

    private string GetFullyQualifiedMethod(IMethodMessage message)
    {
        string serviceName = this.GetServiceName(message.TypeName);
        string methodName = message.MethodName;
        return $"{serviceName}/{methodName}";
    }

    private string GetServiceName(string typeName)
    {
        // typeName will be a full .NET type name as a string "SharedLib.IHelloServer, SharedLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
        return this.serviceNameCache.GetOrAdd(typeName, assemblyQualifiedTypeName =>
        {
            int index = assemblyQualifiedTypeName.IndexOf(",", StringComparison.OrdinalIgnoreCase);
            if (index >= 0)
            {
                // Trim to just the type's full name
                return assemblyQualifiedTypeName.Substring(0, index);
            }

            return assemblyQualifiedTypeName;
        });
    }

    private void SetPostCreationAttributes(Activity activity, IMethodMessage? msg)
    {
        if (!activity.IsAllDataRequested || msg == null)
        {
            return;
        }

        var options = this.optionsProvider.Options;

        string fullyQualifiedMethod = this.GetFullyQualifiedMethod(msg);

        activity.DisplayName = fullyQualifiedMethod;

        try
        {
            options.EnrichWithMethodMessage?.Invoke(activity, msg);
        }
        catch (Exception ex)
        {
            RemotingInstrumentationEventSource.Log.MethodMessageEnrichmentException(ex);
        }
    }
}
