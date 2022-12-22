// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading;

using Microsoft.VisualStudio.TestPlatform.Client.DesignMode;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.VisualStudio.TestPlatform.Client.UnitTests.DesignMode;

[TestClass]
public class DesignModeTestHostLauncherTests
{
    [TestMethod]
    public void DesignModeTestHostLauncherLaunchTestHostShouldCallDesignModeClientToLaunchCustomHost()
    {
        var mockDesignModeClient = new Mock<IDesignModeClient>();
        var launcher = new DesignModeTestHostLauncher(mockDesignModeClient.Object);
        Assert.IsFalse(launcher.IsDebug, "Default launcher must not implement debug launcher interface.");

        var testProcessStartInfo = new TestProcessStartInfo();

        launcher.LaunchTestHost(testProcessStartInfo);

        mockDesignModeClient.Verify(md => md.LaunchCustomHost(testProcessStartInfo, It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public void DesignModeDebugTestHostLauncherLaunchTestHostShouldCallDesignModeClientToLaunchCustomHost()
    {
        var mockDesignModeClient = new Mock<IDesignModeClient>();
        var launcher = new DesignModeDebugTestHostLauncher(mockDesignModeClient.Object);
        Assert.IsTrue(launcher.IsDebug, "Debug launcher must implement debug launcher interface.");

        var testProcessStartInfo = new TestProcessStartInfo();

        launcher.LaunchTestHost(testProcessStartInfo);

        mockDesignModeClient.Verify(md => md.LaunchCustomHost(testProcessStartInfo, It.IsAny<CancellationToken>()), Times.Once);
    }
}
