// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.Client.DesignMode;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.VisualStudio.TestPlatform.Client.UnitTests.DesignMode;

[TestClass]
public class DesignModeTestHostLauncherFactoryTests
{
    [TestMethod]
    public void DesignModeTestHostFactoryShouldReturnNonDebugLauncherIfDebuggingDisabled()
    {
        var mockDesignModeClient = new Mock<IDesignModeClient>();
        var testRunRequestPayload = new TestRunRequestPayload { DebuggingEnabled = false };
        var launcher = DesignModeTestHostLauncherFactory.GetCustomHostLauncherForTestRun(mockDesignModeClient.Object, testRunRequestPayload.DebuggingEnabled);

        Assert.IsFalse(launcher.IsDebug, "Factory must not return debug launcher if debugging is disabled.");
    }

    [TestMethod]
    public void DesignModeTestHostFactoryShouldReturnDebugLauncherIfDebuggingEnabled()
    {
        var mockDesignModeClient = new Mock<IDesignModeClient>();
        var testRunRequestPayload = new TestRunRequestPayload { DebuggingEnabled = true };
        var launcher = DesignModeTestHostLauncherFactory.GetCustomHostLauncherForTestRun(mockDesignModeClient.Object, testRunRequestPayload.DebuggingEnabled);

        Assert.IsTrue(launcher.IsDebug, "Factory must return non-debug launcher if debugging is enabled.");
    }
}
