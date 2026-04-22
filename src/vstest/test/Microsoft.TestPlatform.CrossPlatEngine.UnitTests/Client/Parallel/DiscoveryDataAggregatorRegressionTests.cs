// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client.Parallel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.UnitTests.Client.Parallel;

/// <summary>
/// Regression tests for DiscoveryDataAggregator source status tracking.
/// </summary>
[TestClass]
public class DiscoveryDataAggregatorRegressionTests
{
    // Regression test for #3381 — Change serializer settings to not send empty values
    // DiscoveryDataAggregator tracks which sources were discovered, partially discovered, or skipped.
    [TestMethod]
    public void MarkSourcesWithStatus_ShouldTrackNotDiscoveredSources()
    {
        var aggregator = new DiscoveryDataAggregator();
        var sources = new List<string> { "test1.dll", "test2.dll" };

        aggregator.MarkSourcesWithStatus(sources, DiscoveryStatus.NotDiscovered);

        var result = aggregator.GetSourcesWithStatus(DiscoveryStatus.NotDiscovered);
        Assert.HasCount(2, result);
        CollectionAssert.Contains(result, "test1.dll");
        CollectionAssert.Contains(result, "test2.dll");
    }

    // Regression test for #3381
    [TestMethod]
    public void MarkSourcesWithStatus_ShouldUpgradeStatus()
    {
        var aggregator = new DiscoveryDataAggregator();
        aggregator.MarkSourcesWithStatus(new[] { "test1.dll" }, DiscoveryStatus.NotDiscovered);
        aggregator.MarkSourcesWithStatus(new[] { "test1.dll" }, DiscoveryStatus.FullyDiscovered);

        var notDiscovered = aggregator.GetSourcesWithStatus(DiscoveryStatus.NotDiscovered);
        var fullyDiscovered = aggregator.GetSourcesWithStatus(DiscoveryStatus.FullyDiscovered);

        Assert.IsEmpty(notDiscovered);
        Assert.HasCount(1, fullyDiscovered);
        Assert.AreEqual("test1.dll", fullyDiscovered[0]);
    }

    // Regression test for #3381
    [TestMethod]
    public void GetSourcesWithStatus_EmptyAggregator_ShouldReturnEmptyList()
    {
        var aggregator = new DiscoveryDataAggregator();

        var result = aggregator.GetSourcesWithStatus(DiscoveryStatus.NotDiscovered);

        Assert.IsEmpty(result);
    }

    // Regression test for #3381
    [TestMethod]
    public void MarkSourcesWithStatus_NullSources_ShouldNotThrow()
    {
        var aggregator = new DiscoveryDataAggregator();

        // Should not throw
        aggregator.MarkSourcesWithStatus(null, DiscoveryStatus.NotDiscovered);
    }

    // Regression test for #3381
    [TestMethod]
    public void MarkSourcesWithStatus_NullEntriesInSources_ShouldBeIgnored()
    {
        var aggregator = new DiscoveryDataAggregator();
        var sources = new List<string?> { "test1.dll", null, "test2.dll" };

        aggregator.MarkSourcesWithStatus(sources, DiscoveryStatus.NotDiscovered);

        var result = aggregator.GetSourcesWithStatus(DiscoveryStatus.NotDiscovered);
        Assert.HasCount(2, result);
    }

    // Regression test for #3381
    [TestMethod]
    public void MarkSourcesWithStatus_MixedStatuses_ShouldFilterCorrectly()
    {
        var aggregator = new DiscoveryDataAggregator();
        aggregator.MarkSourcesWithStatus(new[] { "a.dll", "b.dll", "c.dll" }, DiscoveryStatus.NotDiscovered);
        aggregator.MarkSourcesWithStatus(new[] { "a.dll" }, DiscoveryStatus.FullyDiscovered);
        aggregator.MarkSourcesWithStatus(new[] { "b.dll" }, DiscoveryStatus.PartiallyDiscovered);

        Assert.HasCount(1, aggregator.GetSourcesWithStatus(DiscoveryStatus.FullyDiscovered));
        Assert.HasCount(1, aggregator.GetSourcesWithStatus(DiscoveryStatus.PartiallyDiscovered));
        Assert.HasCount(1, aggregator.GetSourcesWithStatus(DiscoveryStatus.NotDiscovered));
    }

    // Regression test for #3381
    [TestMethod]
    public void TryAggregateIsMessageSent_FirstCall_ShouldReturnTrue()
    {
        var aggregator = new DiscoveryDataAggregator();

        bool first = aggregator.TryAggregateIsMessageSent();
        bool second = aggregator.TryAggregateIsMessageSent();

        Assert.IsTrue(first, "First call should return true (first to send).");
        Assert.IsFalse(second, "Second call should return false (already sent).");
    }

    // Regression test for #3381
    [TestMethod]
    public void MarkSourcesWithStatus_AfterMessageSent_ShouldSkipUpdate()
    {
        var aggregator = new DiscoveryDataAggregator();
        aggregator.MarkSourcesWithStatus(new[] { "test.dll" }, DiscoveryStatus.NotDiscovered);

        // Mark message as sent
        aggregator.TryAggregateIsMessageSent();

        // This should be skipped since message was already sent
        aggregator.MarkSourcesWithStatus(new[] { "test.dll" }, DiscoveryStatus.FullyDiscovered);

        // Status should still be NotDiscovered since update was skipped
        var result = aggregator.GetSourcesWithStatus(DiscoveryStatus.NotDiscovered);
        Assert.HasCount(1, result);
    }
}
