// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client.Parallel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.UnitTests.Client.Parallel;

/// <summary>
/// Regression tests for DiscoveryDataAggregator test case source tracking.
/// </summary>
[TestClass]
public class DiscoveryDataAggregatorSourceTrackingRegressionTests
{
    // Regression test for #3381 — Discovery source status tracking
    [TestMethod]
    public void MarkSourcesBasedOnDiscoveredTestCases_ShouldMarkAsPartiallyDiscovered()
    {
        var aggregator = new DiscoveryDataAggregator();

        // First mark as not discovered
        aggregator.MarkSourcesWithStatus(new[] { "test1.dll", "test2.dll" }, DiscoveryStatus.NotDiscovered);

        // Simulate discovering test cases from test1.dll
        var testCases = new List<TestCase>
        {
            new("Test.Method1", new Uri("executor://test"), "test1.dll"),
            new("Test.Method2", new Uri("executor://test"), "test1.dll"),
        };

        string? previousSource = null;
        _ = aggregator.MarkSourcesBasedOnDiscoveredTestCases(previousSource, testCases);

        // test1.dll should now be partially discovered (not fully since discovery hasn't completed)
        var partialSources = aggregator.GetSourcesWithStatus(DiscoveryStatus.PartiallyDiscovered);
        Assert.Contains("test1.dll", partialSources);
    }

    // Regression test for #3381
    [TestMethod]
    public void MarkSourcesBasedOnDiscoveredTestCases_NullTestCases_ShouldNotThrow()
    {
        var aggregator = new DiscoveryDataAggregator();

        // Should not throw
        aggregator.MarkSourcesBasedOnDiscoveredTestCases(null, null);
    }

    // Regression test for #3381
    [TestMethod]
    public void MarkSourcesBasedOnDiscoveredTestCases_EmptyTestCases_ShouldReturnPreviousSource()
    {
        var aggregator = new DiscoveryDataAggregator();
        aggregator.MarkSourcesWithStatus(new[] { "test1.dll" }, DiscoveryStatus.NotDiscovered);

        string? result = aggregator.MarkSourcesBasedOnDiscoveredTestCases("test1.dll", new List<TestCase>());

        // Should return the previous source
        Assert.AreEqual("test1.dll", result);
    }

    // Regression test for #3381
    [TestMethod]
    public void Aggregate_TotalTests_ShouldAccumulateCount()
    {
        var aggregator = new DiscoveryDataAggregator();

        aggregator.Aggregate(new DiscoveryCompleteEventArgs(10, false));
        aggregator.Aggregate(new DiscoveryCompleteEventArgs(15, false));

        Assert.AreEqual(25, aggregator.TotalTests);
        Assert.IsFalse(aggregator.IsAborted);
    }

    // Regression test for #3381
    [TestMethod]
    public void Aggregate_WhenAnyAborted_ShouldBeAborted()
    {
        var aggregator = new DiscoveryDataAggregator();

        aggregator.Aggregate(new DiscoveryCompleteEventArgs(10, false));
        aggregator.Aggregate(new DiscoveryCompleteEventArgs(5, true));

        Assert.IsTrue(aggregator.IsAborted);
    }
}
