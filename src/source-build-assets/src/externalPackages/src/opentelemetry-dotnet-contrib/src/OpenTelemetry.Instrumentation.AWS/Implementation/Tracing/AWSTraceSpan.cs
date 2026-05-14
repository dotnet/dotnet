// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using Amazon.Runtime.Telemetry;
using Amazon.Runtime.Telemetry.Tracing;

namespace OpenTelemetry.Instrumentation.AWS.Implementation.Tracing;

internal sealed class AWSTraceSpan : TraceSpan
{
    private readonly Activity? activity;

    public AWSTraceSpan(Activity? activity)
    {
        this.activity = activity;
        this.Name = activity?.DisplayName;
    }

    public override void EmitEvent(string name, Attributes? attributes = null)
    {
        if (this.activity == null || !this.activity.IsAllDataRequested)
        {
            return;
        }

        var tags = attributes != null ? new ActivityTagsCollection(attributes.AllAttributes) : null;
        this.activity.AddEvent(new ActivityEvent(name, tags: tags));
    }

    public override void SetAttribute(string key, object value)
    {
        if (this.activity == null || !this.activity.IsAllDataRequested)
        {
            return;
        }

        this.activity.SetTag(key, value);
    }

    public override void SetStatus(SpanStatus status)
    {
        if (this.activity == null || !this.activity.IsAllDataRequested)
        {
            return;
        }

        var activityStatus = ConvertToActivityStatusCode(status);
        this.activity.SetStatus(activityStatus);
    }

    public override void RecordException(Exception exception, Attributes? attributes = null)
    {
        if (this.activity == null || !this.activity.IsAllDataRequested)
        {
            return;
        }

        var tags = attributes != null ? new TagList(attributes.AllAttributes.ToArray()) : default;

        this.activity.AddException(exception, tags);
        this.activity.SetStatus(ActivityStatusCode.Error, exception.Message);
    }

    public override void End()
    {
        this.activity?.Stop();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            this.End();
            this.activity?.Dispose();
        }

        base.Dispose(disposing);
    }

    private static ActivityStatusCode ConvertToActivityStatusCode(SpanStatus status)
    {
        return status switch
        {
            SpanStatus.OK => ActivityStatusCode.Ok,
            SpanStatus.ERROR => ActivityStatusCode.Error,
            SpanStatus.UNSET => ActivityStatusCode.Unset,
            _ => ActivityStatusCode.Unset,
        };
    }
}
