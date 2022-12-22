// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestPlatform.Common;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework.Utilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace TestPlatform.Common.UnitTests.ExtensionFramework;

[TestClass]
public class TestPluginCacheTests
{
    private readonly Mock<IFileHelper> _mockFileHelper;

    private readonly TestableTestPluginCache _testablePluginCache;

    public TestPluginCacheTests()
    {
        // Reset the singleton.
        TestPluginCache.Instance = null;
        _mockFileHelper = new Mock<IFileHelper>();
        _testablePluginCache = new TestableTestPluginCache(new List<string> { typeof(TestPluginCacheTests).GetTypeInfo().Assembly.Location });

        _mockFileHelper.Setup(fh => fh.DirectoryExists(It.IsAny<string>())).Returns(true);
    }

    #region Properties tests

    [TestMethod]
    public void InstanceShouldNotReturnANull()
    {
        Assert.IsNotNull(TestPluginCache.Instance);
    }

    [TestMethod]
    public void TestExtensionsShouldBeNullByDefault()
    {
        Assert.IsNull(TestPluginCache.Instance.TestExtensions);
    }

    #endregion

    #region UpdateAdditionalExtensions tests

    [TestMethod]
    public void UpdateAdditionalExtensionsShouldNotThrowIfExtensionPathIsNull()
    {
        TestPluginCache.Instance.UpdateExtensions(null, true);
        Assert.IsFalse(TestPluginCache.Instance.GetExtensionPaths(string.Empty).Any());
    }

    [TestMethod]
    public void UpdateAdditionalExtensionsShouldNotThrowIfExtensionPathIsEmpty()
    {
        TestPluginCache.Instance.UpdateExtensions(new List<string>(), true);
        Assert.IsFalse(TestPluginCache.Instance.GetExtensionPaths(string.Empty).Any());
    }

    [TestMethod]
    public void UpdateAdditionalExtensionsShouldUpdateAdditionalExtensions()
    {
        var additionalExtensions = new List<string> { typeof(TestPluginCacheTests).GetTypeInfo().Assembly.Location };
        TestPluginCache.Instance.UpdateExtensions(additionalExtensions, false);
        var updatedExtensions = TestPluginCache.Instance.GetExtensionPaths(string.Empty);

        Assert.IsNotNull(updatedExtensions);
        CollectionAssert.AreEqual(additionalExtensions, updatedExtensions.ToList());
    }

    [TestMethod]
    public void UpdateAdditionalExtensionsShouldOnlyAddUniqueExtensionPaths()
    {
        var additionalExtensions = new List<string>
        {
            typeof(TestPluginCacheTests).GetTypeInfo().Assembly.Location,
            typeof(TestPluginCacheTests).GetTypeInfo().Assembly.Location
        };
        TestPluginCache.Instance.UpdateExtensions(additionalExtensions, false);
        var updatedExtensions = TestPluginCache.Instance.GetExtensionPaths(string.Empty);

        Assert.IsNotNull(updatedExtensions);
        Assert.AreEqual(1, updatedExtensions.Count);
        CollectionAssert.AreEqual(new List<string> { additionalExtensions.First() }, updatedExtensions);
    }

    [TestMethod]
    public void UpdateAdditionalExtensionsShouldUpdatePathsThatDoNotExist()
    {
        var additionalExtensions = new List<string> { "foo.dll" };
        TestPluginCache.Instance.UpdateExtensions(additionalExtensions, false);
        var updatedExtensions = TestPluginCache.Instance.GetExtensionPaths(string.Empty);

        Assert.IsNotNull(updatedExtensions);
        Assert.AreEqual(1, updatedExtensions.Count);
    }

    [TestMethod]
    public void UpdateAdditionalExtensionsShouldUpdateUnfilteredExtensionsListWhenSkipFilteringIsTrue()
    {
        var additionalExtensions = new List<string> { "foo.dll" };
        TestPluginCache.Instance.UpdateExtensions(additionalExtensions, true);
        var updatedExtensions = TestPluginCache.Instance.GetExtensionPaths("testadapter.dll");

        // Since the extension is unfiltered, above filter criteria doesn't filter it
        Assert.IsNotNull(updatedExtensions);
        Assert.AreEqual(1, updatedExtensions.Count);
    }

    [Ignore]
    [TestMethod]
    public void UpdateAdditionalExtensionsShouldResetExtensionsDiscoveredFlag()
    {
    }

    #endregion

    #region ClearExtensions

    [TestMethod]
    public void ClearExtensionsShouldClearPathToExtensions()
    {
        TestPluginCache.Instance.UpdateExtensions(new List<string> { @"oldExtension.dll" }, false);

        TestPluginCache.Instance.ClearExtensions();

        Assert.AreEqual(0, TestPluginCache.Instance.GetExtensionPaths(string.Empty).Count);
    }

    #endregion

    #region GetExtensionPaths

    [TestMethod]
    public void GetExtensionPathsShouldConsolidateAllExtensions()
    {
        var expectedExtensions = new[] { "filter.dll", "unfilter.dll" }.Select(Path.GetFullPath).ToList();
        expectedExtensions.Add("default.dll");
        TestPluginCache.Instance.UpdateExtensions(new[] { @"filter.dll" }, false);
        TestPluginCache.Instance.UpdateExtensions(new[] { @"unfilter.dll" }, true);
        TestPluginCache.Instance.DefaultExtensionPaths = new[] { "default.dll" };

        var extensions = TestPluginCache.Instance.GetExtensionPaths("filter.dll");

        CollectionAssert.AreEquivalent(expectedExtensions, extensions);
    }

    [TestMethod]
    public void GetExtensionPathsShouldFilterFilterableExtensions()
    {
        var expectedExtensions = new[] { "filter.dll", "unfilter.dll" }.Select(Path.GetFullPath).ToList();
        expectedExtensions.Add("default.dll");
        TestPluginCache.Instance.UpdateExtensions(new[] { @"filter.dll", @"other.dll" }, false);
        TestPluginCache.Instance.UpdateExtensions(new[] { @"unfilter.dll" }, true);
        TestPluginCache.Instance.DefaultExtensionPaths = new[] { "default.dll" };

        var extensions = TestPluginCache.Instance.GetExtensionPaths("filter.dll");

        CollectionAssert.AreEquivalent(expectedExtensions, extensions);
    }

    [TestMethod]
    public void GetExtensionPathsShouldNotFilterIfEndsWithPatternIsNullOrEmpty()
    {
        var expectedExtensions = new[] { "filter.dll", "other.dll", "unfilter.dll" }.Select(Path.GetFullPath).ToList();
        expectedExtensions.Add("default.dll");
        TestPluginCache.Instance.UpdateExtensions(new[] { @"filter.dll", @"other.dll" }, false);
        TestPluginCache.Instance.UpdateExtensions(new[] { @"unfilter.dll" }, true);
        TestPluginCache.Instance.DefaultExtensionPaths = new[] { "default.dll" };

        var extensions = TestPluginCache.Instance.GetExtensionPaths(string.Empty);

        CollectionAssert.AreEquivalent(expectedExtensions, extensions);
    }

    [TestMethod]
    public void GetExtensionPathsShouldSkipDefaultExtensionsIfSetTrue()
    {
        var expectedExtensions = new[] { "filter.dll", "unfilter.dll" }.Select(Path.GetFullPath).ToList();
        InvokeGetExtensionPaths(expectedExtensions, true);
    }

    [TestMethod]
    public void GetExtensionPathsShouldNotSkipDefaultExtensionsIfSetFalse()
    {
        var expectedExtensions = new[] { "filter.dll", "unfilter.dll" }.Select(Path.GetFullPath).ToList();
        expectedExtensions.Add("default.dll");
        InvokeGetExtensionPaths(expectedExtensions, false);
    }

    #endregion

    #region GetDefaultResolutionPaths tests

    [TestMethod]
    public void GetDefaultResolutionPathsShouldReturnCurrentDirectoryByDefault()
    {
        var currentDirectory = Path.GetDirectoryName(typeof(TestPluginCache).GetTypeInfo().Assembly.Location);
        var expectedDirectories = new List<string> { currentDirectory! };

        var resolutionPaths = TestPluginCache.Instance.GetDefaultResolutionPaths();

        Assert.IsNotNull(resolutionPaths);
        CollectionAssert.AreEqual(expectedDirectories, resolutionPaths.ToList());
    }

    [TestMethod]
    public void GetDefaultResolutionPathsShouldReturnAdditionalExtensionPathsDirectories()
    {
        var currentDirectory = Path.GetDirectoryName(typeof(TestPluginCache).GetTypeInfo().Assembly.Location)!;
        var candidateDirectory = Directory.GetParent(currentDirectory)!.FullName;
        var extensionPaths = new List<string> { Path.Combine(candidateDirectory, "foo.dll") };

        // Setup mocks.
        var mockFileHelper = new Mock<IFileHelper>();
        mockFileHelper.Setup(fh => fh.DirectoryExists(It.IsAny<string>())).Returns(false);
        var testableTestPluginCache = new TestableTestPluginCache();

        TestPluginCache.Instance = testableTestPluginCache;

        TestPluginCache.Instance.UpdateExtensions(extensionPaths, true);
        var resolutionPaths = TestPluginCache.Instance.GetDefaultResolutionPaths();

        var expectedExtensions = new List<string> { candidateDirectory, currentDirectory };

        Assert.IsNotNull(resolutionPaths);
        CollectionAssert.AreEqual(expectedExtensions, resolutionPaths.ToList());
    }

    [TestMethod]
    public void GetDefaultResolutionPathsShouldReturnDirectoryFromDefaultExtensionsPath()
    {
        // Setup the testable instance.
        TestPluginCache.Instance = _testablePluginCache;

        var defaultExtensionsFile = typeof(TestPluginCache).GetTypeInfo().Assembly.Location;
        _testablePluginCache.DefaultExtensionPaths = new List<string>() { defaultExtensionsFile };

        var resolutionPaths = TestPluginCache.Instance.GetDefaultResolutionPaths();

        Assert.IsNotNull(resolutionPaths);
        Assert.IsTrue(resolutionPaths.Contains(Path.GetDirectoryName(defaultExtensionsFile)!));
    }

    #endregion

    #region GetResolutionPaths tests

    [TestMethod]
    public void GetResolutionPathsShouldThrowIfExtensionAssemblyIsNull()
    {
        Assert.ThrowsException<ArgumentNullException>(() => TestPluginCache.GetResolutionPaths(null!));
    }

    [TestMethod]
    public void GetResolutionPathsShouldReturnExtensionAssemblyDirectoryAndTpCommonDirectory()
    {
        var temp = Path.GetTempPath();
        var resolutionPaths = TestPluginCache.GetResolutionPaths($@"{temp}{Path.DirectorySeparatorChar}Idonotexist.dll").Select(p => p.Replace("/", "\\")).ToList();

        var tpCommonDirectory = Path.GetDirectoryName(typeof(TestPluginCache).GetTypeInfo().Assembly.Location)!;
        var expectedPaths = new List<string> { temp, tpCommonDirectory }.ConvertAll(p => p.Replace("/", "\\").TrimEnd('\\'));

        CollectionAssert.AreEqual(expectedPaths, resolutionPaths, $"Collection {string.Join(", ", resolutionPaths)}, is not equal to the expected collection {string.Join(", ", expectedPaths)}.");
    }

    [TestMethod]
    public void GetResolutionPathsShouldNotHaveDuplicatePathsIfExtensionIsInSameDirectory()
    {
        var tpCommonlocation = typeof(TestPluginCache).GetTypeInfo().Assembly.Location;

        var resolutionPaths = TestPluginCache.GetResolutionPaths(tpCommonlocation);

        var expectedPaths = new List<string> { Path.GetDirectoryName(tpCommonlocation)! };

        CollectionAssert.AreEqual(expectedPaths, resolutionPaths.ToList());
    }

    #endregion

    #region GetTestExtensions tests

    [TestMethod]
    public void GetTestExtensionsShouldReturnExtensionsInAssembly()
    {
        TestPluginCacheHelper.SetupMockAdditionalPathExtensions(typeof(TestPluginCacheTests));

        TestPluginCache.Instance.GetTestExtensions<TestDiscovererPluginInformation, ITestDiscoverer>(typeof(TestPluginCacheTests).GetTypeInfo().Assembly.Location);

        Assert.IsNotNull(TestPluginCache.Instance.TestExtensions);
        Assert.IsTrue(TestPluginCache.Instance.TestExtensions.TestDiscoverers!.Count > 0);
    }

    [TestMethod]
    public void GetTestExtensionsShouldAddTestExtensionsDiscoveredToCache()
    {
        var extensionAssembly = typeof(TestPluginCacheTests).GetTypeInfo().Assembly.Location;

        var testDiscovererPluginInfos = _testablePluginCache.GetTestExtensions<TestDiscovererPluginInformation, ITestDiscoverer>(extensionAssembly);

        Assert.IsNotNull(_testablePluginCache.TestExtensions);
        CollectionAssert.AreEqual(
            _testablePluginCache.TestExtensions.TestDiscoverers!.Keys,
            testDiscovererPluginInfos.Keys);
    }

    [TestMethod]
    public void GetTestExtensionsShouldGetTestExtensionsFromCache()
    {
        var extensionAssembly = typeof(TestPluginCacheTests).GetTypeInfo().Assembly.Location;
        var testDiscovererPluginInfos = _testablePluginCache.GetTestExtensions<TestDiscovererPluginInformation, ITestDiscoverer>(extensionAssembly);
        Assert.IsFalse(testDiscovererPluginInfos.ContainsKey("td"));

        // Set the cache.
        _testablePluginCache.TestExtensions!.TestDiscoverers!.Add("td", new TestDiscovererPluginInformation(typeof(TestPluginCacheTests)));

        testDiscovererPluginInfos = _testablePluginCache.GetTestExtensions<TestDiscovererPluginInformation, ITestDiscoverer>(extensionAssembly);
        Assert.IsTrue(testDiscovererPluginInfos.ContainsKey("td"));
    }

    [Ignore]
    [TestMethod]
    public void GetTestExtensionsShouldShouldThrowIfDiscovererThrows()
    {
        //TODO : make ITestDiscoverer interface and then mock it in order to make this test case pass.

        var extensionAssembly = typeof(TestPluginCacheTests).GetTypeInfo().Assembly.Location;
        Assert.ThrowsException<Exception>(() => _testablePluginCache.GetTestExtensions<TestDiscovererPluginInformation, ITestDiscoverer>(extensionAssembly));
    }

    #endregion

    #region DiscoverTestExtensions tests

    [TestMethod]
    public void DiscoverTestExtensionsShouldDiscoverExtensionsFromExtensionsFolder()
    {
        TestPluginCacheHelper.SetupMockAdditionalPathExtensions(typeof(TestPluginCacheTests));

        TestPluginCache.Instance.DiscoverTestExtensions<TestDiscovererPluginInformation, ITestDiscoverer>(TestPlatformConstants.TestAdapterEndsWithPattern);

        Assert.IsNotNull(TestPluginCache.Instance.TestExtensions);

        // Validate the discoverers to be absolutely certain.
        Assert.IsTrue(TestPluginCache.Instance.TestExtensions.TestDiscoverers!.Count > 0);
    }

    [TestMethod]
    public void DiscoverTestExtensionsShouldSetCachedBoolToTrue()
    {
        TestPluginCacheHelper.SetupMockAdditionalPathExtensions(typeof(TestPluginCacheTests));

        TestPluginCache.Instance.DiscoverTestExtensions<TestDiscovererPluginInformation, ITestDiscoverer>(TestPlatformConstants.TestAdapterEndsWithPattern);

        Assert.IsTrue(TestPluginCache.Instance.TestExtensions!.AreTestDiscoverersCached);
        Assert.IsTrue(TestPluginCache.Instance.TestExtensions.AreTestExtensionsCached<TestDiscovererPluginInformation>());
    }

    #endregion

    private static void InvokeGetExtensionPaths(List<string> expectedExtensions, bool skipDefaultExtensions)
    {
        TestPluginCache.Instance.UpdateExtensions(new[] { @"filter.dll", @"other.dll" }, false);
        TestPluginCache.Instance.UpdateExtensions(new[] { @"unfilter.dll" }, true);
        TestPluginCache.Instance.DefaultExtensionPaths = new[] { "default.dll" };

        var extensions = TestPluginCache.Instance.GetExtensionPaths("filter.dll", skipDefaultExtensions);

        CollectionAssert.AreEquivalent(expectedExtensions, extensions);
    }
}
