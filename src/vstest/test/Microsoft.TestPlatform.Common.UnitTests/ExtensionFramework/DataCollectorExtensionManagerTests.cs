// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;
using Microsoft.VisualStudio.TestPlatform.Common.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.Common.UnitTests.ExtensionFramework;

[TestClass]
public class DataCollectorExtensionManagerTests
{
    [TestInitialize]
    public void Initialize()
    {
        TestPluginCacheHelper.SetupMockExtensions(typeof(DataCollectorExtensionManagerTests));
    }

    [TestMethod]
    public void CreateShouldThrowExceptionIfMessageLoggerIsNull()
    {
        Assert.ThrowsException<ArgumentNullException>(() =>
        {
            var dataCollectionExtensionManager = DataCollectorExtensionManager.Create(null!);
        });
    }

    [TestMethod]
    public void CreateShouldReturnInstanceOfDataCollectorExtensionManager()
    {
        try
        {
            var dataCollectorExtensionManager = DataCollectorExtensionManager.Create(TestSessionMessageLogger.Instance);
            Assert.IsNotNull(dataCollectorExtensionManager);
            Assert.IsInstanceOfType(dataCollectorExtensionManager, typeof(DataCollectorExtensionManager));
        }
        finally
        {
            TestSessionMessageLogger.Instance = null;
        }
    }
}
