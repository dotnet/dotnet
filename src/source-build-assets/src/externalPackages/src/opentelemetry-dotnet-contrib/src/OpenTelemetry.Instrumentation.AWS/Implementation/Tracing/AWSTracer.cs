// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using Amazon.Runtime.Telemetry;
using Amazon.Runtime.Telemetry.Tracing;

namespace OpenTelemetry.Instrumentation.AWS.Implementation.Tracing;

internal sealed class AWSTracer : Tracer
{
    private readonly ActivitySource activitySource;

    /// <summary>
    /// Initializes a new instance of the <see cref="AWSTracer"/> class.
    /// </summary>
    /// <param name="activitySource">The ActivitySource used for creating and tracking the activities.</param>
    public AWSTracer(ActivitySource activitySource)
    {
        this.activitySource = activitySource ?? throw new ArgumentNullException(nameof(activitySource));
    }

    public override TraceSpan CreateSpan(
        string name,
        Attributes? initialAttributes = null,
        SpanKind spanKind = SpanKind.INTERNAL,
        SpanContext? parentContext = null)
    {
        var tags = initialAttributes != null ? new ActivityTagsCollection(initialAttributes.AllAttributes) : null;
        var activityKind = ConvertToActivityKind(spanKind);

        var parentActivityContext = ConvertToActivityContext(parentContext);
        var activity = this.activitySource.StartActivity(name, activityKind, parentActivityContext, tags);

        return new AWSTraceSpan(activity);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            this.activitySource.Dispose();
        }

        base.Dispose(disposing);
    }

    private static ActivityContext ConvertToActivityContext(SpanContext? parentContext)
    {
        if (parentContext == null)
        {
            return default;
        }

        var traceId = ActivityTraceId.CreateFromString(parentContext.TraceId.AsSpan());
        var spanId = ActivitySpanId.CreateFromString(parentContext.SpanId.AsSpan());

        return new ActivityContext(traceId, spanId, default, null, parentContext.IsRemote);
    }

    private static ActivityKind ConvertToActivityKind(SpanKind spanKind)
    {
        return spanKind switch
        {
            SpanKind.CLIENT => ActivityKind.Client,
            SpanKind.SERVER => ActivityKind.Server,
            SpanKind.PRODUCER => ActivityKind.Producer,
            SpanKind.CONSUMER => ActivityKind.Consumer,
            SpanKind.INTERNAL => ActivityKind.Internal,
            _ => ActivityKind.Internal,
        };
    }
}
