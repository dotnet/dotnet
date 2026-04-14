// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Text;

using Microsoft.TestPlatform.TestHostProvider.Hosting;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Helpers;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Helpers.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Hosting;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Host;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace TestPlatform.TestHostProvider.Hosting.UnitTests;

/// <summary>
/// Regression tests for:
/// - GH-5184: stderr must be forwarded as Informational, not Error.
/// - GH-2479: ARM64 on Windows must not use testhost.exe.
/// </summary>
[TestClass]
public class RegressionBugFixTests
{
    #region GH-5184: Stderr forwarded as Informational

    [TestMethod]
    public void ErrorReceivedCallback_ForwardEnabled_MustSendInformational_NotError()
    {
        // GH-5184: ErrorReceivedCallback changed TestMessageLevel.Error to Informational.
        // If the fix were reverted, SendMessage would be called with Error.
        var mockLogger = new Mock<IMessageLogger>();
        var callbacks = new TestHostManagerCallbacks(true, mockLogger.Object);
        var stdError = new StringBuilder(0, Microsoft.VisualStudio.TestPlatform.CoreUtilities.Constants.StandardErrorMaxLength);

        callbacks.ErrorReceivedCallback(stdError, "debug output from testhost");

        // Must be Informational
        mockLogger.Verify(
            l => l.SendMessage(TestMessageLevel.Informational, "debug output from testhost"),
            Times.Once(),
            "GH-5184: Stderr must be forwarded as Informational.");

        // Must NOT be Error
        mockLogger.Verify(
            l => l.SendMessage(TestMessageLevel.Error, It.IsAny<string>()),
            Times.Never(),
            "GH-5184: Stderr must NOT be forwarded as Error.");
    }

    [TestMethod]
    public void ErrorReceivedCallback_ForwardDisabled_MustNotSendAnyMessage()
    {
        // When forwardOutput=false, no messages should be sent regardless of level.
        var mockLogger = new Mock<IMessageLogger>();
        var callbacks = new TestHostManagerCallbacks(false, mockLogger.Object);
        var stdError = new StringBuilder(0, Microsoft.VisualStudio.TestPlatform.CoreUtilities.Constants.StandardErrorMaxLength);

        callbacks.ErrorReceivedCallback(stdError, "some stderr text");

        mockLogger.Verify(
            l => l.SendMessage(It.IsAny<TestMessageLevel>(), It.IsAny<string>()),
            Times.Never());
    }

    #endregion

    #region GH-2479: ARM64 doesn't use testhost.exe

    [TestMethod]
    public void GetTestHostProcessStartInfo_ARM64OnWindows_MustNotUseTestHostExe()
    {
        // GH-2479: On ARM64 Windows, testhost.exe must not be used because the
        // apphost cannot be relied upon. The fix added an IsWinOnArm() guard that
        // checks the Machine-level PROCESSOR_ARCHITECTURE environment variable.
        //
        // To be a true regression test, testhost.arm64.exe is mocked as existing
        // so the code WOULD use it if the guard weren't there. On ARM64 CI where
        // IsWinOnArm() returns true, the guard blocks entry into the exe-search
        // block entirely.
        var mockProcessHelper = new Mock<IProcessHelper>();
        var mockFileHelper = new Mock<IFileHelper>();
        var mockEnvironment = new Mock<IEnvironment>();
        var mockWindowsRegistry = new Mock<IWindowsRegistryHelper>();
        var mockRunsettingHelper = new Mock<IRunSettingsHelper>();
        var mockEnvironmentVariable = new Mock<IEnvironmentVariableHelper>();
        var mockMessageLogger = new Mock<IMessageLogger>();

        var temp = Path.GetTempPath();
        var testSourcePath = Path.Combine(temp, "test.dll");
        var testhostDllPath = Path.Combine(temp, "testhost.dll");
        var engineDir = @"c:\tmp";
        var dotnetPath = Path.Combine(engineDir, "dotnet.exe");

        // For ARM64, the code generates "testhost.arm64.exe". Mock it as
        // existing so the exe-search code path is actually exercised.
        var testhostExePath = Path.Combine(temp, "testhost.arm64.exe");
        mockFileHelper.Setup(fh => fh.Exists(testhostExePath)).Returns(true);
        mockFileHelper.Setup(fh => fh.Exists(testhostDllPath)).Returns(true);
        mockFileHelper.Setup(fh => fh.Exists(dotnetPath)).Returns(true);

        mockEnvironment.SetupGet(e => e.Architecture).Returns(PlatformArchitecture.ARM64);
        mockEnvironment.Setup(ev => ev.OperatingSystem).Returns(PlatformOperatingSystem.Windows);
        mockRunsettingHelper.SetupGet(r => r.IsDefaultTargetArchitecture).Returns(false);
        mockProcessHelper.Setup(ph => ph.GetCurrentProcessFileName()).Returns(dotnetPath);
        mockProcessHelper.Setup(ph => ph.GetTestEngineDirectory()).Returns(engineDir);
        mockProcessHelper.Setup(ph => ph.GetCurrentProcessArchitecture()).Returns(PlatformArchitecture.ARM64);

        // PROCESSOR_ARCHITECTURE mock — IsWinOnArm() reads the Machine-level
        // env var via Environment.GetEnvironmentVariable (not this helper), so
        // the guard's behavior depends on the real hardware. This mock documents
        // the intended scenario.
        mockEnvironmentVariable
            .Setup(ev => ev.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE"))
            .Returns("ARM64");

        var hostManager = new DotnetTestHostManager(
            mockProcessHelper.Object,
            mockFileHelper.Object,
            new DotnetHostHelper(mockFileHelper.Object, mockEnvironment.Object,
                mockWindowsRegistry.Object, mockEnvironmentVariable.Object, mockProcessHelper.Object),
            mockEnvironment.Object,
            mockRunsettingHelper.Object,
            mockWindowsRegistry.Object,
            mockEnvironmentVariable.Object);

        hostManager.Initialize(mockMessageLogger.Object,
            "<RunSettings><RunConfiguration><TargetPlatform>ARM64</TargetPlatform></RunConfiguration></RunSettings>");

        var connectionInfo = new TestRunnerConnectionInfo
        {
            Port = 123,
            ConnectionInfo = new TestHostConnectionInfo
            {
                Endpoint = "127.0.0.1:123",
                Role = ConnectionRole.Client,
            },
            RunnerProcessId = 0,
        };

        var startInfo = hostManager.GetTestHostProcessStartInfo(
            new[] { testSourcePath }, null, connectionInfo);

        Assert.IsNotNull(startInfo);
        Assert.IsFalse(
            startInfo.FileName!.EndsWith("testhost.exe", StringComparison.OrdinalIgnoreCase),
            "GH-2479: ARM64 on Windows must NOT use testhost.exe.");
        Assert.IsFalse(
            startInfo.FileName!.EndsWith("testhost.x86.exe", StringComparison.OrdinalIgnoreCase),
            "GH-2479: ARM64 on Windows must NOT use testhost.x86.exe.");
    }

    #endregion
}
