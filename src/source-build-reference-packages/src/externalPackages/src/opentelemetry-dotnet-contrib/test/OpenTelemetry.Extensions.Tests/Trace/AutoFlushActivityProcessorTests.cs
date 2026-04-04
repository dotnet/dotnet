// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.Runtime.CompilerServices;
using OpenTelemetry.Tests;
using OpenTelemetry.Trace;
using Xunit;

namespace OpenTelemetry.Extensions.Tests.Trace;

public class AutoFlushActivityProcessorTests
{
    [Fact]
    public void AutoFlushActivityProcessor_FlushAfterLocalServerSideRootSpans_EndMatchingSpan_Flush()
    {
        var activityProcessor = new TestActivityProcessor();
        var sourceName = GetTestMethodName();

        using var provider = Sdk.CreateTracerProviderBuilder()
            .AddProcessor(activityProcessor)
            .AddAutoFlushActivityProcessor(a => a.Parent == null && (a.Kind == ActivityKind.Server || a.Kind == ActivityKind.Consumer), 5000)
            .AddSource(sourceName)
            .Build();

        using var source = new ActivitySource(sourceName);
        using var activity = source.StartActivity("name", ActivityKind.Server);
        Assert.NotNull(activity);
        activity.Stop();

        Assert.True(activityProcessor.ForceFlushCalled);
    }

    [Fact]
    public void AutoFlushActivityProcessor_FlushAfterLocalServerSideRootSpans_EndNonMatchingSpan_DoesNothing()
    {
        var activityProcessor = new TestActivityProcessor();
        var sourceName = GetTestMethodName();

        using var provider = Sdk.CreateTracerProviderBuilder()
            .AddProcessor(activityProcessor)
            .AddAutoFlushActivityProcessor(a => a.Parent == null && (a.Kind == ActivityKind.Server || a.Kind == ActivityKind.Consumer))
            .AddSource(sourceName)
            .Build();

        using var source = new ActivitySource(sourceName);
        using var activity = source.StartActivity("name", ActivityKind.Client);
        Assert.NotNull(activity);
        activity.Stop();

        Assert.False(activityProcessor.ForceFlushCalled);
    }

    [Fact]
    public void AutoFlushActivityProcessor_PredicateThrows_DoesNothing()
    {
        var activityProcessor = new TestActivityProcessor();
        var sourceName = GetTestMethodName();

        using var provider = Sdk.CreateTracerProviderBuilder()
            .AddProcessor(activityProcessor)
            .AddAutoFlushActivityProcessor(_ => throw new Exception("Predicate throws an exception."))
            .AddSource(sourceName)
            .Build();

        using var source = new ActivitySource(sourceName);
        using var activity = source.StartActivity("name", ActivityKind.Server);
        Assert.NotNull(activity);
        activity.Stop();

        Assert.False(activityProcessor.ForceFlushCalled);
    }

    private static string GetTestMethodName([CallerMemberName] string callingMethodName = "")
    {
        return callingMethodName;
    }
}
