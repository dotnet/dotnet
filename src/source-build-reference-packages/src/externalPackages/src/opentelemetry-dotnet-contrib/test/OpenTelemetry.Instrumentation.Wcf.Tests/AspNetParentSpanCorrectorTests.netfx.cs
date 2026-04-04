// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NETFRAMEWORK
using System.Collections.Specialized;
using System.Diagnostics;
using System.Reflection;
using OpenTelemetry.Instrumentation.Wcf.Implementation;
using OpenTelemetry.Trace;
using Xunit;

namespace OpenTelemetry.Instrumentation.Wcf.Tests;

[Collection("WCF")]
public class AspNetParentSpanCorrectorTests
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void IncomingRequestHeadersAreOverwrittenWithAspNetParent(bool headersAreReadOnly)
    {
        var testSource = new ActivitySource("TestSource");
        using var provider = Sdk.CreateTracerProviderBuilder()
            .AddSource("TestSource")
            .AddWcfInstrumentation()
            .Build();

        var reflectedValues = typeof(AspNetParentSpanCorrector).GetField("ReflectedValues", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
        Assert.False(reflectedValues == null, "The reflection-based bind to OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule failed. The AspNet telemetry has likely changed, and this assembly needs to be updated to match");

        using (var aspNetActivity = testSource.StartActivity("AspNetActivity")!)
        {
            var context = new FakeHttpContext();

            var isReadOnlyProperty = context.Request.Headers.GetType().GetProperty("IsReadOnly", BindingFlags.NonPublic | BindingFlags.Instance);
            isReadOnlyProperty.SetValue(context.Request.Headers, headersAreReadOnly);

            var method = typeof(AspNetParentSpanCorrector).GetMethod("OnRequestStarted", BindingFlags.Static | BindingFlags.NonPublic);
            method.Invoke(null, [aspNetActivity, context]);

            var headerVal = context.Request.Headers["traceparent"];
            Assert.Contains(aspNetActivity.TraceId.ToString(), headerVal);
            Assert.Contains(aspNetActivity.SpanId.ToString(), headerVal);
            Assert.Equal(headersAreReadOnly, isReadOnlyProperty.GetValue(context.Request.Headers));
        }

        WcfInstrumentationActivitySource.Options = null;
    }

    private class FakeHttpContext
    {
        public FakeRequest Request { get; } = new();
    }

    private class FakeRequest
    {
        public NameValueCollection Headers { get; } = [];
    }
}
#endif
