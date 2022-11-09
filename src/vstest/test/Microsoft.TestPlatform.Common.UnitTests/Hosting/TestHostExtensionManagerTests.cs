// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestPlatform.Common.Hosting;
using Microsoft.VisualStudio.TestPlatform.Common.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPlatform.Common.UnitTests.Logging;

[TestClass]
public class TestHostExtensionManagerTests
{
    [TestInitialize]
    public void Initialize()
    {
        TestPluginCacheHelper.SetupMockExtensions(typeof(TestHostExtensionManagerTests));
    }

    [TestMethod]
    public void CreateShouldThrowExceptionIfMessageLoggerIsNull()
    {
        Assert.ThrowsException<ArgumentNullException>(() =>
        {
            var testLoggerExtensionManager = TestRuntimeExtensionManager.Create(null!);
        });
    }

    [TestMethod]
    public void CreateShouldReturnInstanceOfTestLoggerExtensionManager()
    {
        try
        {
            var testLoggerExtensionManager = TestRuntimeExtensionManager.Create(TestSessionMessageLogger.Instance);
            Assert.IsNotNull(testLoggerExtensionManager);
            Assert.IsInstanceOfType(testLoggerExtensionManager, typeof(TestRuntimeExtensionManager));
        }
        finally
        {
            TestSessionMessageLogger.Instance = null;
        }
    }
}
