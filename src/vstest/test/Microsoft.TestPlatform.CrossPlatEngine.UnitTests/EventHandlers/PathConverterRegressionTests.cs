// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.UnitTests.EventHandlers;

/// <summary>
/// Regression tests for PathConverter — path conversion for UWP deployment.
/// </summary>
[TestClass]
[TestCategory("Windows")]
public class PathConverterRegressionTests
{
    private readonly PathConverter _pathConverter;

    public PathConverterRegressionTests()
    {
        // Set up path converter: original (remote) path and deployment (local) path
        _pathConverter = new PathConverter(
            @"C:\Remote\TestProject",
            @"C:\Local\Deployed",
            new FileHelper());
    }

    // Regression test for #3367 — PathConverter does not convert uris
    [TestMethod]
    public void UpdatePath_Receive_ShouldReplaceOriginalWithDeployment()
    {
        string path = @"C:\Remote\TestProject\bin\test.dll";
        string result = _pathConverter.UpdatePath(path, PathConversionDirection.Receive);

        Assert.IsTrue(result.StartsWith(@"C:\Local\Deployed\", StringComparison.Ordinal),
            $"Expected deployment path but got: {result}");
    }

    // Regression test for #3367
    [TestMethod]
    public void UpdatePath_Send_ShouldReplaceDeploymentWithOriginal()
    {
        string path = @"C:\Local\Deployed\bin\test.dll";
        string result = _pathConverter.UpdatePath(path, PathConversionDirection.Send);

        Assert.IsTrue(result.StartsWith(@"C:\Remote\TestProject\", StringComparison.Ordinal),
            $"Expected original path but got: {result}");
    }

    // Regression test for #3367
    [TestMethod]
    public void UpdatePath_NullInput_ShouldReturnNull()
    {
        string? result = _pathConverter.UpdatePath(null, PathConversionDirection.Receive);
        Assert.IsNull(result);
    }

    // Regression test for #3367
    [TestMethod]
    public void UpdatePath_PathWithoutMatchingPrefix_ShouldReturnUnchanged()
    {
        string path = @"D:\Other\Path\test.dll";
        string result = _pathConverter.UpdatePath(path, PathConversionDirection.Receive);

        Assert.AreEqual(path, result);
    }

    // Regression test for #3367
    [TestMethod]
    public void UpdateTestCase_ShouldUpdateSourceAndCodeFilePath()
    {
        var testCase = new TestCase(
            "TestNamespace.TestClass.TestMethod",
            new Uri("executor://testexecutor"),
            @"C:\Remote\TestProject\bin\test.dll")
        {
            CodeFilePath = @"C:\Remote\TestProject\src\TestClass.cs"
        };

        _pathConverter.UpdateTestCase(testCase, PathConversionDirection.Receive);

        Assert.IsTrue(testCase.Source.StartsWith(@"C:\Local\Deployed\", StringComparison.Ordinal));
        Assert.IsTrue(testCase.CodeFilePath!.StartsWith(@"C:\Local\Deployed\", StringComparison.Ordinal));
    }

    // Regression test for #3367
    [TestMethod]
    public void UpdateTestCases_ShouldUpdateAllTestCases()
    {
        var testCases = new List<TestCase>
        {
            new("Test1", new Uri("executor://test"), @"C:\Remote\TestProject\test1.dll"),
            new("Test2", new Uri("executor://test"), @"C:\Remote\TestProject\test2.dll"),
        };

        _pathConverter.UpdateTestCases(testCases, PathConversionDirection.Receive);

        foreach (var tc in testCases)
        {
            Assert.IsTrue(tc.Source.StartsWith(@"C:\Local\Deployed\", StringComparison.Ordinal));
        }
    }

    // Regression test for #3367
    [TestMethod]
    public void UpdatePaths_ShouldUpdateAllPaths()
    {
        var paths = new List<string>
        {
            @"C:\Remote\TestProject\a.dll",
            @"C:\Remote\TestProject\b.dll",
        };

        var result = _pathConverter.UpdatePaths(paths, PathConversionDirection.Receive).ToList();

        Assert.HasCount(2, result);        foreach (var p in result)
        {
            Assert.IsTrue(p.StartsWith(@"C:\Local\Deployed\", StringComparison.Ordinal));
        }
    }
}
