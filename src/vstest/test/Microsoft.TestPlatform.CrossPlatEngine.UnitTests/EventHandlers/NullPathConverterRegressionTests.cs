// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.UnitTests.EventHandlers;

/// <summary>
/// Regression tests for NullPathConverter — should pass through all inputs unchanged.
/// </summary>
[TestClass]
public class NullPathConverterRegressionTests
{
    // Regression test for #3367 — PathConverter does not convert uris
    // NullPathConverter is used when no path conversion is needed (no deployment scenario).
    [TestMethod]
    public void UpdatePath_ShouldReturnInputUnchanged()
    {
        IPathConverter converter = NullPathConverter.Instance;

        string path = @"C:\Some\Path\test.dll";
        string? result = converter.UpdatePath(path, PathConversionDirection.Receive);

        Assert.AreEqual(path, result);
    }

    // Regression test for #3367
    [TestMethod]
    public void UpdatePath_Null_ShouldReturnNull()
    {
        IPathConverter converter = NullPathConverter.Instance;

        string? result = converter.UpdatePath(null, PathConversionDirection.Send);

        Assert.IsNull(result);
    }

    // Regression test for #3367
    [TestMethod]
    public void UpdateTestCase_ShouldNotModifyTestCase()
    {
        IPathConverter converter = NullPathConverter.Instance;

        var testCase = new TestCase("Test1", new Uri("executor://test"), @"C:\Path\test.dll")
        {
            CodeFilePath = @"C:\Path\TestClass.cs"
        };

        string originalSource = testCase.Source;
        string? originalCodeFilePath = testCase.CodeFilePath;

        converter.UpdateTestCase(testCase, PathConversionDirection.Receive);

        Assert.AreEqual(originalSource, testCase.Source);
        Assert.AreEqual(originalCodeFilePath, testCase.CodeFilePath);
    }

    // Regression test for #3367
    [TestMethod]
    public void Instance_ShouldReturnSameInstance()
    {
        var instance1 = NullPathConverter.Instance;
        var instance2 = NullPathConverter.Instance;

        Assert.AreSame(instance1, instance2, "NullPathConverter should be a singleton.");
    }
}
