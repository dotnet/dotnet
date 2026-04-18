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

using Microsoft.TestPlatform.Library.IntegrationTests.TranslationLayerTests.EventHandler;
using Microsoft.TestPlatform.TestUtilities;
using Microsoft.TestPlatform.VsTestConsole.TranslationLayer.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.Library.IntegrationTests.TranslationLayerTests;

/// <summary>
/// The Run Tests using VsTestConsoleWrapper API's
/// </summary>
[TestClass]
public class CustomTestHostLauncherTests : AcceptanceTestBase
{
    private IVsTestConsoleWrapper? _vstestConsoleWrapper;

    [TestCleanup]
    public void Cleanup()
    {
        _vstestConsoleWrapper?.EndSession();
    }

    [TestMethod]
    [TestCategory("Windows-Review")]
    [WrapperCompatibilityDataSource()]
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
    [TestCategory("Feature")]
    [WrapperCompatibilityDataSource]
    public void RunAllTestsWithMixedTFMsWillProvideAdditionalInformationToTheDebugger(RunnerInfo runnerInfo)
    {
        // Arrange
        SetTestEnvironment(_testEnvironment, runnerInfo);

        _vstestConsoleWrapper = GetVsTestConsoleWrapper();
        var runEventHandler = new RunEventHandler();
        var netFrameworkDll = GetTestDllForFramework("MSTestProject1.dll", HOST_NETFX);
        var netDll = GetTestDllForFramework("MSTestProject1.dll", HOST_NET);
        var testHostLauncher = new TestHostLauncherV3();

        // Act
        // We have no preference around what TFM is used. It will be autodetected.
        var runsettingsXml = "<RunSettings><RunConfiguration></RunConfiguration></RunSettings>";
        _vstestConsoleWrapper.RunTestsWithCustomTestHost(new[] { netFrameworkDll, netDll }, runsettingsXml, runEventHandler, testHostLauncher);

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

