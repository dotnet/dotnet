// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;
using Microsoft.VisualStudio.TestPlatform.Common.SettingsProvider;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;

using Moq;

namespace Microsoft.TestPlatform.TestUtilities;

public static class TestPluginCacheHelper
{
    public static TestableTestPluginCache SetupMockAdditionalPathExtensions(Type callingTest)
    {
        return SetupMockAdditionalPathExtensions(
            new string[] { callingTest.Assembly.Location });
    }

    public static TestableTestPluginCache SetupMockAdditionalPathExtensions(string[] extensions)
    {
        var mockFileHelper = new Mock<IFileHelper>();
        var testPluginCache = new TestableTestPluginCache();

        TestPluginCache.Instance = testPluginCache;

        // Stub the default extensions folder.
        mockFileHelper.Setup(fh => fh.DirectoryExists(It.IsAny<string>())).Returns(false);

        TestPluginCache.Instance.UpdateExtensions(extensions, true);

        return testPluginCache;
    }

    public static void SetupMockExtensions(Type callingTest, Mock<IFileHelper>? mockFileHelper = null)
    {
        SetupMockExtensions(callingTest, () => { }, mockFileHelper);
    }

    public static void SetupMockExtensions(Type callingTest, Action callback, Mock<IFileHelper>? mockFileHelper = null)
    {
        SetupMockExtensions(new[] { callingTest.Assembly.Location }, callback, mockFileHelper);
    }

    public static void SetupMockExtensions(string[] extensions, Action callback, Mock<IFileHelper>? mockFileHelper = null)
    {
        // Setup mocks.
        mockFileHelper ??= new Mock<IFileHelper>();

        mockFileHelper.Setup(fh => fh.DirectoryExists(It.IsAny<string>())).Returns(true);

        var testableTestPluginCache = new TestableTestPluginCache(extensions.ToList());
        testableTestPluginCache.Action = callback;

        // Setup the testable instance.
        TestPluginCache.Instance = testableTestPluginCache;
    }

    public static void ResetExtensionsCache()
    {
        TestPluginCache.Instance = null;
        SettingsProviderExtensionManager.Destroy();
    }
}
