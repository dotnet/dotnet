// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.ObjectModel.UnitTests;

/// <summary>
/// Regression tests for DiscoveryCriteria source management.
/// </summary>
[TestClass]
public class DiscoveryCriteriaRegressionTests
{
    // Regression test for #3381 — Change serializer settings to not send empty values
    // DiscoveryCriteria was extended to track source discovery status.

    [TestMethod]
    public void Constructor_WithSources_ShouldPopulateAdapterSourceMap()
    {
        var sources = new[] { "test1.dll", "test2.dll" };
        var criteria = new DiscoveryCriteria(sources, frequencyOfDiscoveredTestsEvent: 100, testSettings: null);

        Assert.IsNotNull(criteria.AdapterSourceMap);
        Assert.IsNotEmpty(criteria.AdapterSourceMap);
    }

    [TestMethod]
    public void Sources_ShouldReturnAllConfiguredSources()
    {
        var sources = new[] { "test1.dll", "test2.dll", "test3.dll" };
        var criteria = new DiscoveryCriteria(sources, frequencyOfDiscoveredTestsEvent: 100, testSettings: null);

        var actualSources = criteria.Sources.ToList();
        Assert.HasCount(3, actualSources);
    }

    [TestMethod]
    public void Package_ShouldBeSettableAndGettable()
    {
        var criteria = new DiscoveryCriteria(
            new[] { "test.dll" },
            frequencyOfDiscoveredTestsEvent: 100,
            testSettings: null)
        {
            Package = @"C:\Path\app.msix"
        };

        Assert.AreEqual(@"C:\Path\app.msix", criteria.Package);
    }

    // Regression test for #3381
    [TestMethod]
    public void Constructor_WithRunSettings_ShouldStoreRunSettings()
    {
        var runSettings = "<RunSettings><RunConfiguration /></RunSettings>";
        var criteria = new DiscoveryCriteria(
            new[] { "test.dll" },
            frequencyOfDiscoveredTestsEvent: 50,
            testSettings: runSettings);

        Assert.AreEqual(runSettings, criteria.RunSettings);
    }
}
