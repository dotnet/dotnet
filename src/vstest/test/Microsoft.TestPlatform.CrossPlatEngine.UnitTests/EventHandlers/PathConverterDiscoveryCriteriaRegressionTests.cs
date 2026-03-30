// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.UnitTests.EventHandlers;

/// <summary>
/// Regression tests for PathConverter DiscoveryCriteria and TestRunCriteria handling.
/// </summary>
[TestClass]
[TestCategory("Windows")]
public class PathConverterDiscoveryCriteriaRegressionTests
{
    private readonly PathConverter _pathConverter;

    public PathConverterDiscoveryCriteriaRegressionTests()
    {
        _pathConverter = new PathConverter(
            @"C:\Remote\TestDir",
            @"C:\Local\DeployDir",
            new FileHelper());
    }

    // Regression test for #3367 — PathConverter does not convert uris
    [TestMethod]
    public void UpdateDiscoveryCriteria_ShouldUpdateSourcePaths()
    {
        var criteria = new DiscoveryCriteria(
            new[] { @"C:\Remote\TestDir\test.dll" },
            frequencyOfDiscoveredTestsEvent: 100,
            testSettings: null)
        {
            Package = @"C:\Remote\TestDir\app.msix"
        };

        _pathConverter.UpdateDiscoveryCriteria(criteria, PathConversionDirection.Receive);

        Assert.IsTrue(criteria.Package!.StartsWith(@"C:\Local\DeployDir\", StringComparison.Ordinal));
    }

    // Regression test for #3367
    [TestMethod]
    public void UpdateTestRunCriteriaWithSources_ShouldUpdatePackage()
    {
        var sourceMap = new Dictionary<string, IEnumerable<string>>
        {
            ["adapter1"] = new[] { @"C:\Remote\TestDir\test1.dll" }
        };

        var criteria = new CommunicationUtilities.ObjectModel.TestRunCriteriaWithSources(
            sourceMap,
            @"C:\Remote\TestDir\app.msix",
            runSettings: null!,
            testExecutionContext: null!);

        var result = _pathConverter.UpdateTestRunCriteriaWithSources(criteria, PathConversionDirection.Receive);

        Assert.IsTrue(result.Package!.StartsWith(@"C:\Local\DeployDir\", StringComparison.Ordinal));
    }

    // Regression test for #3367
    [TestMethod]
    public void UpdateTestRunCriteriaWithTests_ShouldUpdateTestCasePaths()
    {
        var tests = new List<TestCase>
        {
            new("Test1", new Uri("executor://test"), @"C:\Remote\TestDir\test.dll")
        };

        var criteria = new CommunicationUtilities.ObjectModel.TestRunCriteriaWithTests(
            tests,
            package: @"C:\Remote\TestDir\app.msix",
            runSettings: null!,
            testExecutionContext: null!);

        var result = _pathConverter.UpdateTestRunCriteriaWithTests(criteria, PathConversionDirection.Receive);

        Assert.IsTrue(result.Package!.StartsWith(@"C:\Local\DeployDir\", StringComparison.Ordinal));
    }
}
