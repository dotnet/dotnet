// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestPlatform.Common;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework.Utilities;
using Microsoft.VisualStudio.TestPlatform.Common.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPlatform.Common.UnitTests.ExtensionFramework;

[TestClass]
public class TestPluginManagerTests
{
    [TestMethod]
    public void GetTestExtensionTypeShouldReturnExtensionType()
    {
        var type = TestPluginManager.GetTestExtensionType(typeof(TestPluginManagerTests).AssemblyQualifiedName!);

        Assert.AreEqual(typeof(TestPluginManagerTests), type);
    }

    [TestMethod]
    public void GetTestExtensionTypeShouldThrowIfTypeNotFound()
    {
        Assert.ThrowsException<TypeLoadException>(() => TestPluginManager.GetTestExtensionType("randomassemblyname.random"));
    }

    [TestMethod]
    public void CreateTestExtensionShouldCreateExtensionTypeInstance()
    {
        var instance = TestPluginManager.CreateTestExtension<ITestDiscoverer>(typeof(DummyTestDiscoverer));

        Assert.IsNotNull(instance);
    }

    [TestMethod]
    public void CreateTestExtensionShouldThrowIfInstanceCannotBeCreated()
    {
        Assert.ThrowsException<MissingMethodException>(() => TestPluginManager.CreateTestExtension<ITestLogger>(typeof(AbstractDummyLogger)));
    }

    [TestMethod]
    public void InstanceShouldReturnTestPluginManagerInstance()
    {
        var instance = TestPluginManager.Instance;

        Assert.IsNotNull(instance);
        Assert.IsTrue(instance is TestPluginManager);
    }

    [TestMethod]
    public void InstanceShouldReturnCachedTestPluginManagerInstance()
    {
        var instance = TestPluginManager.Instance;

        Assert.AreEqual(instance, TestPluginManager.Instance);
    }

    [TestMethod]
    public void GetTestExtensionsShouldReturnTestDiscovererExtensions()
    {
        TestPluginCacheHelper.SetupMockExtensions(typeof(TestPluginManagerTests));

        TestPluginManager.GetSpecificTestExtensions<TestDiscovererPluginInformation, ITestDiscoverer, ITestDiscovererCapabilities, TestDiscovererMetadata>(
            TestPlatformConstants.TestAdapterEndsWithPattern,
            out var unfilteredTestExtensions,
            out var testExtensions);

        Assert.IsNotNull(unfilteredTestExtensions);
        Assert.IsNotNull(testExtensions);
        Assert.IsTrue(testExtensions.Any());
    }

    [TestMethod]
    public void GetTestExtensionsShouldDiscoverExtensionsOnlyOnce()
    {
        var discoveryCount = 0;
        TestPluginCacheHelper.SetupMockExtensions(typeof(TestPluginManagerTests), () => discoveryCount++);

        TestPluginManager.GetSpecificTestExtensions<TestDiscovererPluginInformation, ITestDiscoverer, ITestDiscovererCapabilities, TestDiscovererMetadata>(
            TestPlatformConstants.TestAdapterEndsWithPattern,
            out var unfilteredTestExtensions,
            out var testExtensions);

        // Call this again to verify that discovery is not called again.
        TestPluginManager.GetSpecificTestExtensions<TestDiscovererPluginInformation, ITestDiscoverer, ITestDiscovererCapabilities, TestDiscovererMetadata>(
            TestPlatformConstants.TestAdapterEndsWithPattern,
            out unfilteredTestExtensions,
            out testExtensions);

        Assert.IsNotNull(testExtensions);
        Assert.IsTrue(testExtensions.Any());

        Assert.AreEqual(2, discoveryCount);
    }

    [TestMethod]
    public void GetTestExtensionsForAnExtensionAssemblyShouldReturnExtensionsInThatAssembly()
    {
        TestPluginManager.GetTestExtensions<TestDiscovererPluginInformation, ITestDiscoverer, ITestDiscovererCapabilities, TestDiscovererMetadata>(
                typeof(TestPluginManagerTests).Assembly.Location,
                out _,
                out var testExtensions);

        Assert.IsNotNull(testExtensions);
        Assert.IsTrue(testExtensions.Any());
    }

    #region Implementations

    private abstract class AbstractDummyLogger : ITestLogger
    {
        public void Initialize(TestLoggerEvents events, string testRunDirectory)
        {
            throw new NotImplementedException();
        }
    }

    private class DummyTestDiscoverer : ITestDiscoverer, ITestExecutor
    {
        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public void DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext, IMessageLogger logger, ITestCaseDiscoverySink discoverySink)
        {
            throw new NotImplementedException();
        }

        public void RunTests(IEnumerable<string>? sources, IRunContext? runContext, IFrameworkHandle? frameworkHandle)
        {
            throw new NotImplementedException();
        }

        public void RunTests(IEnumerable<TestCase>? tests, IRunContext? runContext, IFrameworkHandle? frameworkHandle)
        {
            throw new NotImplementedException();
        }
    }

    #endregion
}
