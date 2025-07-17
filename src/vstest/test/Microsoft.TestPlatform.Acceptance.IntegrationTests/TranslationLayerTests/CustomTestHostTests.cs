// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

using FluentAssertions;

using Microsoft.TestPlatform.VsTestConsole.TranslationLayer.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests.TranslationLayerTests;

/// <summary>
/// The Run Tests using VsTestConsoleWrapper API's
/// </summary>
[TestClass]
public class CustomTestHostTests : AcceptanceTestBase
{
    private IVsTestConsoleWrapper? _vstestConsoleWrapper;

    [TestCleanup]
    public void Cleanup()
    {
        _vstestConsoleWrapper?.EndSession();
    }

    [TestMethod]
    [TestCategory("Windows-Review")]
    [RunnerCompatibilityDataSource(BeforeFeature = Features.ATTACH_DEBUGGER_FLOW)]
    // This does not work with testhosts that are earlier than when the feature was introduced,
    // when latest runner is used, because the latest runner does not downgrade the messages when
    // older testhost launcher is used.
    // [TestHostCompatibilityDataSource(BeforeFeature = Features.ATTACH_DEBUGGER_FLOW)]
    public void RunTestsWithCustomTestHostLauncherLaunchesTheProcessUsingTheProvidedLauncher(RunnerInfo runnerInfo)
    {
        // Pins the existing functionality.

        // Arrange
        SetTestEnvironment(_testEnvironment, runnerInfo);

        _vstestConsoleWrapper = GetVsTestConsoleWrapper();
        var runEventHandler = new RunEventHandler();

        // Act
        var customTestHostLauncher = new TestHostLauncherV1();
        _vstestConsoleWrapper.RunTestsWithCustomTestHost(GetTestDlls("MSTestProject1.dll", "MSTestProject2.dll"), GetDefaultRunSettings(), runEventHandler, customTestHostLauncher);

        // Assert
        EnsureTestsRunWithoutErrors(runEventHandler, passed: 2, failed: 2, skipped: 2);

        // Ensure we tried to launch testhost process.
        customTestHostLauncher.Should().BeAssignableTo<ITestHostLauncher>();
        customTestHostLauncher.LaunchProcessProcessId.Should().NotBeNull("we should launch some real process and save the pid of it");
    }

    [TestMethod]
    [TestCategory("Windows-Review")]
    // [RunnerCompatibilityDataSource(BeforeFeature = Features.ATTACH_DEBUGGER_FLOW)]
    [TestHostCompatibilityDataSource(DEFAULT_RUNNER_NETFX, DEFAULT_RUNNER_NETCORE, "LegacyStable", BeforeFeature = Features.ATTACH_DEBUGGER_FLOW, DebugVSTestConsole = true)]
    [Ignore("This is not working for any testhost prior 16.7.0 where the change was introduced. The launch testhost flow was replaced with AttachDebugger in runner, and the new callback to AttachDebugger happens in testhost."
        + "But any testhost prior 16.7.0 where the change was introduced does not know to call back AttachDebugger, and the call never happens.")]
    // You can confirm that the functionality broke between runner and testhost, past this point by using newer runners, against older testhosts.
    // [TestPlatformCompatibilityDataSource(AfterRunnerFeature = Features.ATTACH_DEBUGGER_FLOW, BeforeTestHostFeature = Features.ATTACH_DEBUGGER_FLOW)]
    public void RunTestsWithCustomTestHostLauncherLaunchesTheProcessUsingTheProvidedLauncherWhenITestHostLauncher2IsProvided(RunnerInfo runnerInfo)
    {
        // Ensures compatibility with testhost and runners that were created before 16.3.0. It makes sure that even if user provides
        // an implementation of the ITestHostLauncher2 interface, then testhost expecting ITestHostLauncher still works correctly.

        // Arrange
        SetTestEnvironment(_testEnvironment, runnerInfo);
        _vstestConsoleWrapper = GetVsTestConsoleWrapper();
        var runEventHandler = new RunEventHandler();

        // Act
        var customTestHostLauncher = new TestHostLauncherV2();
        _vstestConsoleWrapper.RunTestsWithCustomTestHost(GetTestDlls("MSTestProject1.dll", "MSTestProject2.dll"), GetDefaultRunSettings(), runEventHandler, customTestHostLauncher);

        // Assert
        EnsureTestsRunWithoutErrors(runEventHandler, passed: 2, failed: 2, skipped: 2);

        customTestHostLauncher.Should().BeAssignableTo<ITestHostLauncher2>();
        customTestHostLauncher.LaunchProcessProcessId.Should().NotBeNull("we should launch some real process and save the pid of it");
        customTestHostLauncher.AttachDebuggerProcessId.Should().BeNull("we should not be asked to attach to a debugger, that flow is not used when vstest.console does not support it yet, even when it is given ITestHostLauncher2");
    }

    [TestMethod]
    [TestCategory("Windows-Review")]
    [RunnerCompatibilityDataSource(AfterFeature = Features.ATTACH_DEBUGGER_FLOW)]
    // [TestHostCompatibilityDataSource(AfterFeature = Features.ATTACH_DEBUGGER_FLOW)]
    public void RunTestsWithCustomTestHostLauncherAttachesToDebuggerUsingTheProvidedLauncher(RunnerInfo runnerInfo)
    {

        // Arrange
        SetTestEnvironment(_testEnvironment, runnerInfo);
        _vstestConsoleWrapper = GetVsTestConsoleWrapper();
        var runEventHandler = new RunEventHandler();

        // Act
        var customTestHostLauncher = new TestHostLauncherV2();
        _vstestConsoleWrapper.RunTestsWithCustomTestHost(GetTestDlls("MSTestProject1.dll", "MSTestProject2.dll"), GetDefaultRunSettings(), runEventHandler, customTestHostLauncher);

        // Assert
        EnsureTestsRunWithoutErrors(runEventHandler, passed: 2, failed: 2, skipped: 2);

        customTestHostLauncher.Should().BeAssignableTo<ITestHostLauncher2>();
        customTestHostLauncher.AttachDebuggerProcessId.Should().NotBeNull("we should be asked to attach a debugger to some process and save the pid of the process");
        customTestHostLauncher.LaunchProcessProcessId.Should().BeNull("we should not be asked to launch some real process, that flow is not used when vstest.console supports it and is given ITestHostLauncher2");
    }

    [TestMethod]
    [TestCategory("Windows-Review")]
    [Ignore("This is not working. The compatibility code only checks the protocol version (in handler), which is dictated by testhost. "
        + "It sees 6 but does not realize that the provided CustomTesthostLauncher is not supporting the new feature, it ends up calling back to EditoAttachDebugger" +
        "in translation layer, and that just silently skips the call.")]
    [RunnerCompatibilityDataSource(AfterFeature = Features.ATTACH_DEBUGGER_FLOW)]
    [TestHostCompatibilityDataSource(AfterFeature = Features.ATTACH_DEBUGGER_FLOW)]
    public void RunTestsWithCustomTestHostLauncherUsesLaunchWhenGivenAnOutdatedITestHostLauncher(RunnerInfo runnerInfo)
    {
        // Arrange
        SetTestEnvironment(_testEnvironment, runnerInfo);
        _vstestConsoleWrapper = GetVsTestConsoleWrapper();
        var runEventHandler = new RunEventHandler();

        // Act
        var customTestHostLauncher = new TestHostLauncherV1();
        _vstestConsoleWrapper.RunTestsWithCustomTestHost(GetTestDlls("MSTestProject1.dll", "MSTestProject2.dll"), GetDefaultRunSettings(), runEventHandler, customTestHostLauncher);

        // Assert
        EnsureTestsRunWithoutErrors(runEventHandler, passed: 2, failed: 2, skipped: 2);

        customTestHostLauncher.Should().NotBeAssignableTo<ITestHostLauncher2>();
        customTestHostLauncher.LaunchProcessProcessId.Should().NotBeNull("we should launch some real process and save the pid of it");
    }

    [TestMethod]
    [TestCategory("Windows-Review")]
    [TestCategory("Feature")]
    // "Just row" used here because mstest does not cooperate with older versions of vstest.console correctly, so we test with just the latest version available..
    [RunnerCompatibilityDataSource(AfterFeature = Features.MULTI_TFM, JustRow = 0)]
    public void RunAllTestsWithMixedTFMsWillProvideAdditionalInformationToTheDebugger(RunnerInfo runnerInfo)
    {
        // Arrange
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var vstestConsoleWrapper = GetVsTestConsoleWrapper();
        var runEventHandler = new RunEventHandler();
        var netFrameworkDll = GetTestDllForFramework("MSTestProject1.dll", DEFAULT_HOST_NETFX);
        var netDll = GetTestDllForFramework("MSTestProject1.dll", DEFAULT_HOST_NETCORE);
        var testHostLauncher = new TestHostLauncherV3();

        // Act
        // We have no preference around what TFM is used. It will be autodetected.
        var runsettingsXml = "<RunSettings><RunConfiguration></RunConfiguration></RunSettings>";
        vstestConsoleWrapper.RunTestsWithCustomTestHost(new[] { netFrameworkDll, netDll }, runsettingsXml, runEventHandler, testHostLauncher);

        // Assert
        if (runEventHandler.Errors.Any())
        {
            var tempPath = TempDirectory.Path;
            var files = System.IO.Directory.GetFiles(tempPath, "*.txt").ToList();
            if (files.Count == 0)
            {
                throw new InvalidOperationException($"No error files found in {tempPath}. {string.Join("\n", Directory.GetFiles(tempPath))}");
            }

            var allText = new StringBuilder();
            foreach (var file in files)
            {
#pragma warning disable CA1305 // Specify IFormatProvider
                allText.AppendLine($"Error file: {file}");
                allText.AppendLine(File.ReadAllText(file));
                allText.AppendLine();
#pragma warning restore CA1305 // Specify IFormatProvider
            }
            throw new InvalidOperationException($"Logs: {allText}");
        }

        runEventHandler.Errors.Should().BeEmpty();
        testHostLauncher.AttachDebuggerInfos.Should().HaveCount(2);
        var targetFrameworks = testHostLauncher.AttachDebuggerInfos.Select(i => i.TargetFramework).ToList();
        targetFrameworks.Should().OnlyContain(tfm => tfm.StartsWith(".NETFramework") || tfm.StartsWith(".NET "));

        runEventHandler.TestResults.Should().HaveCount(6, "we run all tests from both assemblies");
    }

    [TestMethod]
    [TestCategory("Windows-Review")]
    [TestCategory("BackwardCompatibilityWithRunner")]
    // "Just row" used here because mstest does not cooperate with older versions of vstest.console correctly, so we test with just the single version available
    // before the multi tfm feature.
    [RunnerCompatibilityDataSource(BeforeFeature = Features.MULTI_TFM, JustRow = 0)]
    public void RunAllTestsCallsBackToTestHostLauncherV3EvenWhenRunnerDoesNotSupportMultiTfmOrTheNewAttachDebugger2MessageYet(RunnerInfo runnerInfo)
    {
        // Arrange
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var vstestConsoleWrapper = GetVsTestConsoleWrapper();
        var runEventHandler = new RunEventHandler();
        var netFrameworkDll = GetTestDllForFramework("MSTestProject1.dll", DEFAULT_HOST_NETFX);
        var netDll = GetTestDllForFramework("MSTestProject1.dll", DEFAULT_HOST_NETCORE);
        var testHostLauncher = new TestHostLauncherV3();

        // Act
        // We have no preference around what TFM is used. It will be autodetected.
        var runsettingsXml = "<RunSettings><RunConfiguration></RunConfiguration></RunSettings>";
        vstestConsoleWrapper.RunTestsWithCustomTestHost(new[] { netFrameworkDll, netDll }, runsettingsXml, runEventHandler, testHostLauncher);

        // Assert
        runEventHandler.Errors.Should().BeEmpty();
        testHostLauncher.AttachDebuggerInfos.Should().HaveCount(1);
        var pid = testHostLauncher.AttachDebuggerInfos.Select(i => i.ProcessId).Single();
        pid.Should().NotBe(0);

        runEventHandler.TestResults.Should().HaveCount(3, "we run all tests from just one of the assemblies, because the runner does not support multi tfm");
    }

    private static void EnsureTestsRunWithoutErrors(RunEventHandler runEventHandler, int passed, int failed, int skipped)
    {
        runEventHandler.Errors.Should().BeEmpty();
        runEventHandler.TestResults.Should().HaveCount(passed + failed + skipped);
        runEventHandler.TestResults.Count(t => t.Outcome == TestOutcome.Passed).Should().Be(passed);
        runEventHandler.TestResults.Count(t => t.Outcome == TestOutcome.Failed).Should().Be(failed);
        runEventHandler.TestResults.Count(t => t.Outcome == TestOutcome.Skipped).Should().Be(skipped);
    }

    /// <summary>
    /// The custom test host launcher implementing ITestHostLauncher.
    /// </summary>
    private class TestHostLauncherV1 : ITestHostLauncher
    {
        public int? LaunchProcessProcessId { get; private set; }

        /// <inheritdoc />
        public bool IsDebug => true;

        /// <inheritdoc />
        public int LaunchTestHost(TestProcessStartInfo defaultTestHostStartInfo)
        {
            return LaunchTestHost(defaultTestHostStartInfo, CancellationToken.None);
        }

        /// <inheritdoc />
        public int LaunchTestHost(TestProcessStartInfo defaultTestHostStartInfo, CancellationToken cancellationToken)
        {
            var processInfo = new ProcessStartInfo(
                defaultTestHostStartInfo.FileName!,
                defaultTestHostStartInfo.Arguments!)
            {
                WorkingDirectory = defaultTestHostStartInfo.WorkingDirectory
            };
            processInfo.UseShellExecute = false;

            var process = new Process { StartInfo = processInfo };
            process.Start();

            LaunchProcessProcessId = process?.Id;
            return LaunchProcessProcessId ?? -1;
        }
    }

    /// <summary>
    /// The custom test host launcher implementing ITestHostLauncher2, and through that also ITestHostLauncher.
    /// </summary>
    private class TestHostLauncherV2 : TestHostLauncherV1, ITestHostLauncher2
    {

        public int? AttachDebuggerProcessId { get; private set; }

        public bool AttachDebuggerToProcess(int pid) => AttachDebuggerToProcess(pid, CancellationToken.None);

        public bool AttachDebuggerToProcess(int pid, CancellationToken cancellationToken)
        {
            AttachDebuggerProcessId = pid;
            return true;
        }
    }

#pragma warning disable CS0618 // Type or member is obsolete
    private class TestHostLauncherV3 : ITestHostLauncher3
    {
        public bool IsDebug => true;

        public List<AttachDebuggerInfo> AttachDebuggerInfos { get; } = new();

        public bool AttachDebuggerToProcess(AttachDebuggerInfo attachDebuggerInfo, CancellationToken cancellationToken)
        {
            AttachDebuggerInfos.Add(attachDebuggerInfo);

            return true;
        }

        public bool AttachDebuggerToProcess(int pid)
        {
            return AttachDebuggerToProcess(new AttachDebuggerInfo
            {
                ProcessId = pid,
                TargetFramework = null,
            }, CancellationToken.None);
        }

        public bool AttachDebuggerToProcess(int pid, CancellationToken cancellationToken)
        {
            return AttachDebuggerToProcess(new AttachDebuggerInfo
            {
                ProcessId = pid,
                TargetFramework = null,
            }, CancellationToken.None);
        }

        public int LaunchTestHost(TestProcessStartInfo defaultTestHostStartInfo)
        {
            return -1;
        }

        public int LaunchTestHost(TestProcessStartInfo defaultTestHostStartInfo, CancellationToken cancellationToken)
        {
            return -1;
        }
    }
}
#pragma warning restore CS0618 // Type or member is obsolete

