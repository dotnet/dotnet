// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NETFRAMEWORK
using System.Net;
#endif
using System.Diagnostics;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry.Extensions.AWS.Trace;
using Xunit;

namespace OpenTelemetry.Extensions.AWS.Tests;

public class AWSXRayPropagatorTests
{
    private const string AWSXRayTraceHeaderKey = "X-Amzn-Trace-Id";
    private const string TraceId = "5759e988bd862e3fe1be46a994272793";
    private const string ParentId = "53995c3f42cd8ad8";

    private static readonly string[] Empty = [];

    private static readonly Func<IDictionary<string, string>, string, IEnumerable<string>> Getter = (headers, name) =>
    {
        return headers.TryGetValue(name, out var value) ? [value] : Empty;
    };

    private static readonly Action<IDictionary<string, string>, string, string> Setter = (carrier, name, value) =>
    {
        carrier[name] = value;
    };

#if NETFRAMEWORK
    private static readonly Action<HttpWebRequest, string, string> HeaderValueSetter = (request, name, value) => request.Headers.Add(name, value);
#endif

    private readonly AWSXRayPropagator awsXRayPropagator = new();

#if !NETFRAMEWORK
    private static Action<HttpRequestMessage, string, string> HeaderValueSetter => (request, name, value) =>
    {
        request.Headers.Remove(name);
        request.Headers.Add(name, value);
    };
#endif

    [Fact]
    public void TestInjectTraceHeader()
    {
        var carrier = new Dictionary<string, string>();
        var traceId = ActivityTraceId.CreateFromString(TraceId.AsSpan());
        var parentId = ActivitySpanId.CreateFromString(ParentId.AsSpan());
        var traceFlags = ActivityTraceFlags.Recorded;
        var activityContext = new ActivityContext(traceId, parentId, traceFlags);
        this.awsXRayPropagator.Inject(new PropagationContext(activityContext, default), carrier, Setter);

        Assert.True(carrier.ContainsKey(AWSXRayTraceHeaderKey));
        Assert.Equal("Root=1-5759e988-bd862e3fe1be46a994272793;Parent=53995c3f42cd8ad8;Sampled=1", carrier[AWSXRayTraceHeaderKey]);
    }

    [Fact]
    public void TestInjectTraceHeaderNotSampled()
    {
        var carrier = new Dictionary<string, string>();
        var traceId = ActivityTraceId.CreateFromString(TraceId.AsSpan());
        var parentId = ActivitySpanId.CreateFromString(ParentId.AsSpan());
        var traceFlags = ActivityTraceFlags.None;
        var activityContext = new ActivityContext(traceId, parentId, traceFlags);
        this.awsXRayPropagator.Inject(new PropagationContext(activityContext, default), carrier, Setter);

        Assert.True(carrier.ContainsKey(AWSXRayTraceHeaderKey));
        Assert.Equal("Root=1-5759e988-bd862e3fe1be46a994272793;Parent=53995c3f42cd8ad8;Sampled=0", carrier[AWSXRayTraceHeaderKey]);
    }

    [Fact]
    public void TestInjectTraceHeaderAlreadyExists()
    {
        var traceIdHeader = "Root=1-00000-00000000000000000;Parent=123456789;Sampled=0";

#if !NETFRAMEWORK
        var carrier = new HttpRequestMessage();
#else
        var carrier = (HttpWebRequest)WebRequest.Create(new Uri("http://www.google.com/"));
#endif
        carrier.Headers.Add(AWSXRayTraceHeaderKey, traceIdHeader);
        var traceId = ActivityTraceId.CreateFromString(TraceId.AsSpan());
        var parentId = ActivitySpanId.CreateFromString(ParentId.AsSpan());
        var traceFlags = ActivityTraceFlags.None;
        var activityContext = new ActivityContext(traceId, parentId, traceFlags);
        this.awsXRayPropagator.Inject(new PropagationContext(activityContext, default), carrier, HeaderValueSetter);

#if !NETFRAMEWORK
        Assert.True(carrier.Headers.Contains(AWSXRayTraceHeaderKey));
        Assert.Equal(traceIdHeader, carrier.Headers.GetValues(AWSXRayTraceHeaderKey).FirstOrDefault());
#else
        Assert.Equal(traceIdHeader, carrier.Headers.Get(AWSXRayTraceHeaderKey));
#endif
    }

    [Fact]
    public void TestInjectTraceHeaderAlreadyExistsButNotHttpRequestMessage()
    {
        var traceIdHeader = "Root=1-00000-00000000000000000;Parent=123456789;Sampled=0";
        var carrier = new Dictionary<string, string>()
        {
            { AWSXRayTraceHeaderKey, traceIdHeader },
        };
        var traceId = ActivityTraceId.CreateFromString(TraceId.AsSpan());
        var parentId = ActivitySpanId.CreateFromString(ParentId.AsSpan());
        var traceFlags = ActivityTraceFlags.None;
        var activityContext = new ActivityContext(traceId, parentId, traceFlags);
        this.awsXRayPropagator.Inject(new PropagationContext(activityContext, default), carrier, Setter);

        Assert.True(carrier.ContainsKey(AWSXRayTraceHeaderKey));
        Assert.Equal("Root=1-5759e988-bd862e3fe1be46a994272793;Parent=53995c3f42cd8ad8;Sampled=0", carrier[AWSXRayTraceHeaderKey]);
    }

    [Fact]
    public void TestExtractTraceHeader()
    {
        var carrier = new Dictionary<string, string>()
        {
            { AWSXRayTraceHeaderKey, "Root=1-5759e988-bd862e3fe1be46a994272793;Parent=53995c3f42cd8ad8;Sampled=1" },
        };
        var traceId = ActivityTraceId.CreateFromString(TraceId.AsSpan());
        var parentId = ActivitySpanId.CreateFromString(ParentId.AsSpan());
        var traceFlags = ActivityTraceFlags.Recorded;
        var activityContext = new ActivityContext(traceId, parentId, traceFlags, isRemote: true);

        Assert.Equal(new PropagationContext(activityContext, default), this.awsXRayPropagator.Extract(default, carrier, Getter));
    }

    [Fact]
    public void TestExtractTraceHeaderNotSampled()
    {
        var carrier = new Dictionary<string, string>()
        {
            { AWSXRayTraceHeaderKey, "Root=1-5759e988-bd862e3fe1be46a994272793;Parent=53995c3f42cd8ad8;Sampled=0" },
        };
        var traceId = ActivityTraceId.CreateFromString(TraceId.AsSpan());
        var parentId = ActivitySpanId.CreateFromString(ParentId.AsSpan());
        var traceFlags = ActivityTraceFlags.None;
        var activityContext = new ActivityContext(traceId, parentId, traceFlags, isRemote: true);

        Assert.Equal(new PropagationContext(activityContext, default), this.awsXRayPropagator.Extract(default, carrier, Getter));
    }

    [Fact]
    public void TestExtractTraceHeaderDifferentOrder()
    {
        var carrier = new Dictionary<string, string>()
        {
            { AWSXRayTraceHeaderKey, "Sampled=1;Parent=53995c3f42cd8ad8;Root=1-5759e988-bd862e3fe1be46a994272793" },
        };
        var traceId = ActivityTraceId.CreateFromString(TraceId.AsSpan());
        var parentId = ActivitySpanId.CreateFromString(ParentId.AsSpan());
        var traceFlags = ActivityTraceFlags.Recorded;
        var activityContext = new ActivityContext(traceId, parentId, traceFlags, isRemote: true);

        Assert.Equal(new PropagationContext(activityContext, default), this.awsXRayPropagator.Extract(default, carrier, Getter));
    }

    [Fact]
    public void TestExtractTraceHeaderWithEmptyValue()
    {
        var carrier = new Dictionary<string, string>()
        {
            { AWSXRayTraceHeaderKey, string.Empty },
        };

        Assert.Equal(default, this.awsXRayPropagator.Extract(default, carrier, Getter));
    }

    [Fact]
    public void TestExtractTraceHeaderWithoutParentId()
    {
        var carrier = new Dictionary<string, string>()
        {
            { AWSXRayTraceHeaderKey, "Root=1-5759e988-bd862e3fe1be46a994272793;Sampled=1" },
        };

        Assert.Equal(default, this.awsXRayPropagator.Extract(default, carrier, Getter));
    }

    [Fact]
    public void TestExtractTraceHeaderWithoutSampleDecision()
    {
        var carrier = new Dictionary<string, string>()
        {
            { AWSXRayTraceHeaderKey, "Root=1-5759e988-bd862e3fe1be46a994272793;Parent=53995c3f42cd8ad8" },
        };

        Assert.Equal(default, this.awsXRayPropagator.Extract(default, carrier, Getter));
    }

    [Fact]
    public void TestExtractTraceHeaderWithInvalidTraceId()
    {
        var carrier = new Dictionary<string, string>()
        {
            { AWSXRayTraceHeaderKey, "Root=15759e988bd862e3fe1be46a994272793;Parent=53995c3f42cd8ad8;Sampled=1" },
        };

        Assert.Equal(default, this.awsXRayPropagator.Extract(default, carrier, Getter));
    }

    [Fact]
    public void TestExtractTraceHeaderWithInvalidParentId()
    {
        var carrier = new Dictionary<string, string>()
        {
            { AWSXRayTraceHeaderKey, "Root=1-5759e988-bd862e3fe1be46a994272793;Parent=123;Sampled=1" },
        };

        Assert.Equal(default, this.awsXRayPropagator.Extract(default, carrier, Getter));
    }

    [Fact]
    public void TestExtractTraceHeaderWithInvalidSampleDecision()
    {
        var carrier = new Dictionary<string, string>()
        {
            { AWSXRayTraceHeaderKey, "Root=1-5759e988-bd862e3fe1be46a994272793;Parent=53995c3f42cd8ad8;Sampled=" },
        };

        Assert.Equal(default, this.awsXRayPropagator.Extract(default, carrier, Getter));
    }

    [Fact]
    public void TestExtractTraceHeaderWithInvalidSampleDecisionValue()
    {
        var carrier = new Dictionary<string, string>()
        {
            { AWSXRayTraceHeaderKey, "Root=1-5759e988-bd862e3fe1be46a994272793;Parent=53995c3f42cd8ad8;Sampled=3" },
        };

        Assert.Equal(default, this.awsXRayPropagator.Extract(default, carrier, Getter));
    }

    [Fact]
    public void TestExtractTraceHeaderWithInvalidSampleDecisionLength()
    {
        var carrier = new Dictionary<string, string>()
        {
            { AWSXRayTraceHeaderKey, "Root=1-5759e988-bd862e3fe1be46a994272793;Parent=53995c3f42cd8ad8;Sampled=123" },
        };

        Assert.Equal(default, this.awsXRayPropagator.Extract(default, carrier, Getter));
    }

    [Fact]
    public void TestExtractTraceHeaderWithAdditionalField()
    {
        var carrier = new Dictionary<string, string>()
        {
            { AWSXRayTraceHeaderKey, "Root=1-5759e988-bd862e3fe1be46a994272793;Parent=53995c3f42cd8ad8;Sampled=1;Foo=Bar" },
        };
        var traceId = ActivityTraceId.CreateFromString(TraceId.AsSpan());
        var parentId = ActivitySpanId.CreateFromString(ParentId.AsSpan());
        var traceFlags = ActivityTraceFlags.Recorded;
        var activityContext = new ActivityContext(traceId, parentId, traceFlags, isRemote: true);

        Assert.Equal(new PropagationContext(activityContext, default), this.awsXRayPropagator.Extract(default, carrier, Getter));
    }
}
