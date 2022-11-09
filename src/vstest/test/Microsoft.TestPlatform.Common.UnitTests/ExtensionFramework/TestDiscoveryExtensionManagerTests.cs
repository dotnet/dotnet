// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPlatform.Common.UnitTests.ExtensionFramework;

[TestClass]
public class TestDiscoveryExtensionManagerTests
{
    [TestCleanup]
    public void TestCleanup()
    {
        TestDiscoveryExtensionManager.Destroy();
    }

    [TestMethod]
    public void CreateShouldDiscoverDiscovererExtensions()
    {
        TestPluginCacheHelper.SetupMockExtensions(typeof(TestDiscoveryExtensionManagerTests));

        var extensionManager = TestDiscoveryExtensionManager.Create();

        Assert.IsNotNull(extensionManager.Discoverers);
        Assert.IsTrue(extensionManager.Discoverers.Any());
    }

    [TestMethod]
    public void CreateShouldCacheDiscoveredExtensions()
    {
        TestPluginCacheHelper.SetupMockExtensions(typeof(TestDiscoveryExtensionManagerTests), () => { });

        var extensionManager = TestDiscoveryExtensionManager.Create();
        TestDiscoveryExtensionManager.Create();

        Assert.IsNotNull(extensionManager.Discoverers);
        Assert.IsTrue(extensionManager.Discoverers.Any());
    }

    [TestMethod]
    public void GetDiscoveryExtensionManagerShouldReturnADiscoveryManagerWithExtensions()
    {
        var extensionManager =
            TestDiscoveryExtensionManager.GetDiscoveryExtensionManager(
                typeof(TestDiscoveryExtensionManagerTests).GetTypeInfo().Assembly.Location);

        Assert.IsNotNull(extensionManager.Discoverers);
        Assert.IsTrue(extensionManager.Discoverers.Any());
    }

    #region LoadAndInitialize tests

    [TestMethod]
    public void LoadAndInitializeShouldInitializeAllExtensions()
    {
        TestPluginCacheHelper.SetupMockExtensions(typeof(TestDiscoveryExtensionManagerTests));

        TestDiscoveryExtensionManager.LoadAndInitializeAllExtensions(false);

        var allDiscoverers = TestDiscoveryExtensionManager.Create().Discoverers;

        foreach (var discoverer in allDiscoverers)
        {
            Assert.IsTrue(discoverer.IsExtensionCreated);
        }
    }

    #endregion
}

[TestClass]
public class TestDiscovererMetadataTests
{
    [TestMethod]
    public void TestDiscovererMetadataCtorDoesNotThrowWhenFileExtensionsIsNull()
    {
        var metadata = new TestDiscovererMetadata(null, null);

        Assert.IsNull(metadata.FileExtension);
        Assert.IsFalse(metadata.IsDirectoryBased);
    }

    [TestMethod]
    public void TestDiscovererMetadataCtorDoesNotThrowWhenFileExtensionsIsEmpty()
    {
        var metadata = new TestDiscovererMetadata(new List<string>(), null);

        Assert.IsNull(metadata.FileExtension);
        Assert.IsFalse(metadata.IsDirectoryBased);
    }

    [TestMethod]
    public void TestDiscovererMetadataCtorDoesNotThrowWhenDefaultUriIsNull()
    {
        var metadata = new TestDiscovererMetadata(new List<string>(), null);

        Assert.IsNull(metadata.DefaultExecutorUri);
        Assert.IsFalse(metadata.IsDirectoryBased);
    }

    [TestMethod]
    public void TestDiscovererMetadataCtorDoesNotThrowWhenDefaultUriIsEmpty()
    {
        var metadata = new TestDiscovererMetadata(new List<string>(), " ");

        Assert.IsNull(metadata.DefaultExecutorUri);
        Assert.IsFalse(metadata.IsDirectoryBased);
    }

    [TestMethod]
    public void TestDiscovererMetadataCtorSetsFileExtensions()
    {
        var extensions = new List<string> { "csv", "dll" };
        var metadata = new TestDiscovererMetadata(extensions, null);

        CollectionAssert.AreEqual(extensions, metadata.FileExtension!.ToList());
        Assert.IsFalse(metadata.IsDirectoryBased);
    }

    [TestMethod]
    public void TestDiscovererMetadataCtorSetsDefaultUri()
    {
        var metadata = new TestDiscovererMetadata(null, "executor://helloworld");

        Assert.AreEqual("executor://helloworld/", metadata.DefaultExecutorUri!.AbsoluteUri);
        Assert.IsFalse(metadata.IsDirectoryBased);
    }

    [TestMethod]
    public void TestDiscovererMetadataCtorSetsAssemblyType()
    {
        var metadata = new TestDiscovererMetadata(null, "executor://helloworld", AssemblyType.Native);

        Assert.AreEqual(AssemblyType.Native, metadata.AssemblyType);
        Assert.IsFalse(metadata.IsDirectoryBased);
    }

    [TestMethod]
    public void TestDiscovererMetadataCtorSetsIsDirectoryBased()
    {
        var metadata = new TestDiscovererMetadata(null, "executor://helloworld", isDirectoryBased: true);

        Assert.IsTrue(metadata.IsDirectoryBased);
    }
}
