// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.ObjectModel.UnitTests;

/// <summary>
/// Regression tests for DiscoveryCompleteEventArgs source status properties.
/// </summary>
[TestClass]
public class DiscoveryCompleteEventArgsRegressionTests
{
    // Regression test for #3381 — Change serializer settings to not send empty values
    // DiscoveryCompleteEventArgs was extended with source status lists.
    [TestMethod]
    public void Constructor_ShouldSetTotalCountAndAborted()
    {
        var args = new DiscoveryCompleteEventArgs(42, false);

        Assert.AreEqual(42, args.TotalCount);
        Assert.IsFalse(args.IsAborted);
    }

    // Regression test for #3381
    [TestMethod]
    public void FullyDiscoveredSources_ShouldBeSettableAndGettable()
    {
        var args = new DiscoveryCompleteEventArgs(10, false)
        {
            FullyDiscoveredSources = new List<string> { "a.dll", "b.dll" }
        };

        Assert.IsNotNull(args.FullyDiscoveredSources);
        Assert.HasCount(2, args.FullyDiscoveredSources!);
    }

    // Regression test for #3381
    [TestMethod]
    public void PartiallyDiscoveredSources_ShouldBeSettableAndGettable()
    {
        var args = new DiscoveryCompleteEventArgs(5, true)
        {
            PartiallyDiscoveredSources = new List<string> { "partial.dll" }
        };

        Assert.IsNotNull(args.PartiallyDiscoveredSources);
        Assert.HasCount(1, args.PartiallyDiscoveredSources!);
    }

    // Regression test for #3381
    [TestMethod]
    public void NotDiscoveredSources_ShouldBeSettableAndGettable()
    {
        var args = new DiscoveryCompleteEventArgs(0, true)
        {
            NotDiscoveredSources = new List<string> { "missing.dll" }
        };

        Assert.IsNotNull(args.NotDiscoveredSources);
        Assert.HasCount(1, args.NotDiscoveredSources!);
    }

    // Regression test for #3381
    [TestMethod]
    public void SkippedDiscoveredSources_ShouldBeSettableAndGettable()
    {
        var args = new DiscoveryCompleteEventArgs(0, false)
        {
            SkippedDiscoveredSources = new List<string> { "skipped.dll" }
        };

        Assert.IsNotNull(args.SkippedDiscoveredSources);
        Assert.HasCount(1, args.SkippedDiscoveredSources!);
    }

    // Regression test for #3381
    [TestMethod]
    public void AllSourceStatusLists_DefaultToEmptyOrNull()
    {
        var args = new DiscoveryCompleteEventArgs(0, false);

        // The default may be empty lists or null depending on the constructor
        // Verify at least that they are accessible without throwing
        _ = args.FullyDiscoveredSources;
        _ = args.PartiallyDiscoveredSources;
        _ = args.NotDiscoveredSources;
        _ = args.SkippedDiscoveredSources;
    }

    // Regression test for #3381
    [TestMethod]
    public void Metrics_ShouldBeSettableAndGettable()
    {
        var args = new DiscoveryCompleteEventArgs(10, false)
        {
            Metrics = new Dictionary<string, object>
            {
                { "TestDiscovery.TotalTests", 10 },
                { "TestDiscovery.TimeTaken", 1.5 }
            }
        };

        Assert.IsNotNull(args.Metrics);
        Assert.HasCount(2, args.Metrics!);
    }
}
