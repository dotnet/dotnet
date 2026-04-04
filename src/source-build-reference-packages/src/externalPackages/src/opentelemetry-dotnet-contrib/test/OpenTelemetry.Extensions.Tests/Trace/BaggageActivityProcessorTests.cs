// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using OpenTelemetry.Trace;
using Xunit;

namespace OpenTelemetry.Extensions.Tests.Trace;

public class BaggageActivityProcessorTests
{
    [Fact]
    public void BaggageActivityProcessor_CanAddAllowAllBaggageKeysPredicate()
    {
        var sourceName = GetTestMethodName();

        using var provider = Sdk.CreateTracerProviderBuilder()
            .AddBaggageActivityProcessor(BaggageActivityProcessor.AllowAllBaggageKeys)
            .AddSource(sourceName)
            .Build();

        Baggage.SetBaggage("key", "value");
        Baggage.SetBaggage("other_key", "other_value");

        using var source = new ActivitySource(sourceName);
        using var activity = source.StartActivity("name", ActivityKind.Server);
        Assert.NotNull(activity);
        activity.Stop();

        Assert.Contains(activity.Tags, kv => kv.Key == "key" && kv.Value == "value");
        Assert.Contains(activity.Tags, kv => kv.Key == "other_key" && kv.Value == "other_value");
    }

    [Fact]
    public void BaggageActivityProcessor_CanUseCustomPredicate()
    {
        var sourceName = GetTestMethodName();

        using var provider = Sdk.CreateTracerProviderBuilder()
            .AddBaggageActivityProcessor((baggageKey) => baggageKey.StartsWith("key", StringComparison.Ordinal))
            .AddSource(sourceName)
            .Build();

        Baggage.SetBaggage("key", "value");
        Baggage.SetBaggage("other_key", "other_value");

        using var source = new ActivitySource(sourceName);
        using var activity = source.StartActivity("name", ActivityKind.Client);
        Assert.NotNull(activity);
        activity.Stop();

        Assert.Contains(activity.Tags, kv => kv.Key == "key" && kv.Value == "value");
        Assert.DoesNotContain(activity.Tags, kv => kv.Key == "other_key" && kv.Value == "other_value");
    }

    [Fact]
    public void BaggageActivityProcessor_CanUseRegex()
    {
        var sourceName = GetTestMethodName();

        var regex = new Regex("^mykey", RegexOptions.Compiled);
        using var provider = Sdk.CreateTracerProviderBuilder()
            .AddBaggageActivityProcessor(regex.IsMatch)
            .AddSource(sourceName)
            .Build();

        Baggage.SetBaggage("mykey", "value");
        Baggage.SetBaggage("other_key", "other_value");

        using var source = new ActivitySource(sourceName);
        using var activity = source.StartActivity("name", ActivityKind.Client);
        Assert.NotNull(activity);
        activity.Stop();

        Assert.Contains(activity.Tags, kv => kv.Key == "mykey" && kv.Value == "value");
        Assert.DoesNotContain(activity.Tags, kv => kv.Key == "other_key" && kv.Value == "other_value");
    }

    [Fact]
    public void BaggageActivityProcessor_PredicateThrows_DoesNothing()
    {
        var sourceName = GetTestMethodName();

        using var provider = Sdk.CreateTracerProviderBuilder()
            .AddBaggageActivityProcessor(_ => throw new Exception("Predicate throws an exception."))
            .AddSource(sourceName)
            .Build();

        Baggage.SetBaggage("key", "value");

        using var source = new ActivitySource(sourceName);
        using var activity = source.StartActivity("name", ActivityKind.Server);
        Assert.NotNull(activity);
        activity.Stop();

        Assert.DoesNotContain(activity.Tags, kv => kv.Key == "key" && kv.Value == "value");
    }

    [Fact]
    public void BaggageActivityProcessor_PredicateThrows_OnlyDropsEntriesThatThrow()
    {
        var sourceName = GetTestMethodName();

        // First call to predicate should not throw, second call should.
        using var provider = Sdk.CreateTracerProviderBuilder()
            .AddBaggageActivityProcessor(key =>
            {
                return key == "key" ? throw new Exception("Predicate throws an exception.") : true;
            })
            .AddSource(sourceName)
            .Build();

        Baggage.SetBaggage("key", "value");
        Baggage.SetBaggage("other_key", "other_value");
        Baggage.SetBaggage("another_key", "another_value");

        using var source = new ActivitySource(sourceName);
        using var activity = source.StartActivity("name", ActivityKind.Server);
        Assert.NotNull(activity);
        activity.Stop();

        // Only keys that do not throw should be added.
        Assert.DoesNotContain(activity.Tags, kv => kv.Key == "key" && kv.Value == "value");
        Assert.Contains(activity.Tags, kv => kv.Key == "other_key" && kv.Value == "other_value");
        Assert.Contains(activity.Tags, kv => kv.Key == "another_key" && kv.Value == "another_value");
    }

    private static string GetTestMethodName([CallerMemberName] string callingMethodName = "")
    {
        return callingMethodName;
    }
}
