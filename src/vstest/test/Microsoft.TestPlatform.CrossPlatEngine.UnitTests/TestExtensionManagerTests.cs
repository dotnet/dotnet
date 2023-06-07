// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPlatform.CrossPlatEngine.UnitTests;

[TestClass]
public class TestExtensionManagerTests
{
    private readonly ITestExtensionManager _testExtensionManager;

    public TestExtensionManagerTests()
    {
        _testExtensionManager = new TestExtensionManager();

        // Reset the singleton
        TestPluginCache.Instance = null;
    }

    [TestCleanup]
    public void TestCleanup()
    {
        TestPluginCache.Instance = null;
    }

    [TestMethod]
    public void UseAdditionalExtensionsShouldUpdateAdditionalExtensionsInCache()
    {
        var extensions = new List<string> { typeof(TestExtensionManagerTests).Assembly.Location };

        _testExtensionManager.UseAdditionalExtensions(extensions, true);

        CollectionAssert.AreEquivalent(extensions, TestPluginCache.Instance.GetExtensionPaths(string.Empty));
    }

    [TestMethod]
    public void ClearExtensionsShouldClearExtensionsInCache()
    {
        var extensions = new List<string> { @"Foo.dll" };
        _testExtensionManager.UseAdditionalExtensions(extensions, false);

        _testExtensionManager.ClearExtensions();

        Assert.AreEqual(0, TestPluginCache.Instance.GetExtensionPaths(string.Empty).Count);
    }
}
