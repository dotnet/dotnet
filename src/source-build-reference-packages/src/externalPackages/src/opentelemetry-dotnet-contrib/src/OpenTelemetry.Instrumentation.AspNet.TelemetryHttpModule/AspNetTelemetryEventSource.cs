// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.Diagnostics.Tracing;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Instrumentation.AspNet;

/// <summary>
/// ETW EventSource tracing class.
/// </summary>
[EventSource(Name = "OpenTelemetry-Instrumentation-AspNet-Telemetry", Guid = "1de158cc-f7ce-4293-bd19-2358c93c8186")]
internal sealed class AspNetTelemetryEventSource : EventSource
{
    /// <summary>
    /// Instance of the AspNetTelemetryEventSource class.
    /// </summary>
    public static readonly AspNetTelemetryEventSource Log = new();

    [NonEvent]
    public void ActivityStarted(Activity activity)
    {
        if (this.IsEnabled(EventLevel.Verbose, EventKeywords.All))
        {
            this.ActivityStarted(activity.Id);
        }
    }

    [NonEvent]
    public void ActivityStopped(Activity activity)
    {
        if (this.IsEnabled(EventLevel.Verbose, EventKeywords.All))
        {
            this.ActivityStopped(activity.Id);
        }
    }

    [NonEvent]
    public void ActivityRestored(Activity activity)
    {
        if (this.IsEnabled(EventLevel.Informational, EventKeywords.All))
        {
            this.ActivityRestored(activity.Id);
        }
    }

    [NonEvent]
    public void ActivityException(Activity? activity, Exception ex)
    {
        if (this.IsEnabled(EventLevel.Error, EventKeywords.All))
        {
            this.ActivityException(activity?.Id, ex.ToInvariantString());
        }
    }

    [NonEvent]
    public void CallbackException(Activity? activity, string eventName, Exception ex)
    {
        if (this.IsEnabled(EventLevel.Error, EventKeywords.All))
        {
            this.CallbackException(activity?.Id, eventName, ex.ToInvariantString());
        }
    }

    [Event(1, Message = "Callback='{0}'", Level = EventLevel.Verbose)]
    public void TraceCallback(string callback)
    {
        this.WriteEvent(1, callback);
    }

    [Event(2, Message = "Activity started, Id='{0}'", Level = EventLevel.Verbose)]
    public void ActivityStarted(string? id)
    {
        this.WriteEvent(2, id);
    }

    [Event(3, Message = "Activity stopped, Id='{0}'", Level = EventLevel.Verbose)]
    public void ActivityStopped(string? id)
    {
        this.WriteEvent(3, id);
    }

    [Event(4, Message = "Activity restored, Id='{0}'", Level = EventLevel.Informational)]
    public void ActivityRestored(string? id)
    {
        this.WriteEvent(4, id);
    }

    [Event(5, Message = "Failed to invoke OnExecuteRequestStep, Error='{0}'", Level = EventLevel.Error)]
    public void OnExecuteRequestStepInvocationError(string error)
    {
        this.WriteEvent(5, error);
    }

    [Event(6, Message = "Activity exception, Id='{0}': {1}", Level = EventLevel.Error)]
    public void ActivityException(string? id, string ex)
    {
        this.WriteEvent(6, id, ex);
    }

    [Event(7, Message = "Callback exception, Id='{0}', Name='{1}': {2}", Level = EventLevel.Error)]
    public void CallbackException(string? id, string eventName, string ex)
    {
        this.WriteEvent(7, id, eventName, ex);
    }
}
