// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

using Microsoft.VisualStudio.TestPlatform.Common;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Tracing.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using vstest.console.UnitTests.TestDoubles;

using CommandLineResources = Microsoft.VisualStudio.TestPlatform.CommandLine.Resources.Resources;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.UnitTests;

[TestClass]
// These tests construct real Executors and share fixed temp-file names, so they must not run in parallel.
[DoNotParallelize]
public class ExecutorUnitTests
{
    private readonly CommandLineOptions _commandLineOptions = new();
    private readonly RunSettingsManager _runSettingsManager = new();
    private readonly Mock<ITestPlatformEventSource> _mockTestPlatformEventSource;

    public ExecutorUnitTests()
    {
        _mockTestPlatformEventSource = new Mock<ITestPlatformEventSource>();
    }

    /// <summary>
    /// Executor should Print splash screen first
    /// </summary>
    [TestMethod]
    public void ExecutorPrintsSplashScreenTest()
    {
        var mockOutput = new MockOutput();
        var exitCode = new Executor(mockOutput, _mockTestPlatformEventSource.Object, new ProcessHelper(), new PlatformEnvironment()).Execute("/badArgument");
        var assemblyVersion = typeof(Executor).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion;

        Assert.AreEqual(1, exitCode, "Exit code must be One for bad arguments");

        // Verify that messages exist
        Assert.IsNotEmpty(mockOutput.Messages, "Executor must print at least copyright info");
        Assert.IsNotNull(mockOutput.Messages.First().Message, "First Printed Message cannot be null or empty");

        Assert.Contains(CommandLineResources.MicrosoftCommandLineTitle.Split(['{'], 2)[0],
            mockOutput.Messages.First().Message!);

        var suffixIndex = assemblyVersion.IndexOf("-");
        var version = suffixIndex == -1 ? assemblyVersion : assemblyVersion.Substring(0, suffixIndex);
        Assert.Contains(version,
            mockOutput.Messages.First().Message!);
    }

    [TestMethod]
    public void ExecutorShouldNotPrintsSplashScreenIfNoLogoPassed()
    {
        var mockOutput = new MockOutput();
        var exitCode = new Executor(mockOutput, _mockTestPlatformEventSource.Object, new ProcessHelper(), new PlatformEnvironment()).Execute("--nologo");

        Assert.AreEqual(1, exitCode, "Exit code must be One for bad arguments");

        // Verify that messages exist
        Assert.HasCount(1, mockOutput.Messages);

        // Check the part of message before the actual version because that is variable.
        Assert.DoesNotContain(
            CommandLineResources.MicrosoftCommandLineTitle.Split(['{'], 2)[0],
            mockOutput.Messages.First()
                .Message!);
    }

    [TestMethod]
    public void ExecutorShouldSanitizeNoLogoInput()
    {
        var mockOutput = new MockOutput();
        var exitCode = new Executor(mockOutput, _mockTestPlatformEventSource.Object, new ProcessHelper(), new PlatformEnvironment()).Execute("--nologo");

        Assert.AreEqual(1, exitCode, "Exit code must be One when no arguments are provided.");

        Assert.Contains(message => message.Message!.Contains(CommandLineResources.NoArgumentsProvided), mockOutput.Messages);
    }

    /// <summary>
    /// Executor should Print Error message and Help contents when no arguments are provided.
    /// </summary>
    [TestMethod]
    public void ExecutorEmptyArgsPrintsErrorAndHelpMessage()
    {
        var mockOutput = new MockOutput();
        var exitCode = new Executor(mockOutput, _mockTestPlatformEventSource.Object, new ProcessHelper(), new PlatformEnvironment()).Execute(null);

        Assert.AreEqual(1, exitCode, "Exit code must be One when no arguments are provided.");

        Assert.Contains(message => message.Message!.Contains(CommandLineResources.NoArgumentsProvided), mockOutput.Messages);
    }

    [TestMethod]
    public void ExecutorWithInvalidArgsShouldPrintErrorMessage()
    {
        var mockOutput = new MockOutput();
        string badArg = "/badArgument";
        var exitCode = new Executor(mockOutput, _mockTestPlatformEventSource.Object, new ProcessHelper(), new PlatformEnvironment()).Execute(badArg);

        Assert.AreEqual(1, exitCode, "Exit code must be One when no arguments are provided.");

        Assert.Contains(message => message.Message!.Contains(string.Format(CultureInfo.CurrentCulture, CommandLineResources.InvalidArgument, badArg)), mockOutput.Messages);
    }

    [TestMethod]
    public void ExecutorWithInvalidArgsShouldPrintHowToUseHelpOption()
    {
        var mockOutput = new MockOutput();
        string badArg = "--invalidArg";
        var exitCode = new Executor(mockOutput, _mockTestPlatformEventSource.Object, new ProcessHelper(), new PlatformEnvironment()).Execute(badArg);

        Assert.AreEqual(1, exitCode, "Exit code must be One when no arguments are provided.");

        Assert.Contains(message => message.Message!.Contains(string.Format(CultureInfo.CurrentCulture, CommandLineResources.InvalidArgument, badArg)), mockOutput.Messages);
    }

    [TestMethod]
    public void ExecutorWithInvalidArgsAndValueShouldPrintErrorMessage()
    {
        var mockOutput = new MockOutput();
        string badArg = "--invalidArg:xyz";
        var exitCode = new Executor(mockOutput, _mockTestPlatformEventSource.Object, new ProcessHelper(), new PlatformEnvironment()).Execute(badArg);

        Assert.AreEqual(1, exitCode, "Exit code must be One when no arguments are provided.");

        Assert.Contains(message => message.Message!.Contains(string.Format(CultureInfo.CurrentCulture, CommandLineResources.InvalidArgument, badArg)), mockOutput.Messages);
    }

    /// <summary>
    /// Executor should set default runsettings value even there is no processor
    /// </summary>
    [TestMethod]
    public void ExecuteShouldInitializeDefaultRunsettings()
    {
        var mockOutput = new MockOutput();
        _ = new Executor(mockOutput, _mockTestPlatformEventSource.Object, new ProcessHelper(), new PlatformEnvironment(), _runSettingsManager).Execute(null);
        RunConfiguration runConfiguration = XmlRunSettingsUtilities.GetRunConfigurationNode(_runSettingsManager.ActiveRunSettings.SettingsXml);
        Assert.AreEqual(Constants.DefaultResultsDirectory, runConfiguration.ResultsDirectory);
        Assert.AreEqual(Framework.DefaultFramework.ToString(), runConfiguration.TargetFramework!.ToString());
        Assert.AreEqual(Constants.DefaultPlatform, runConfiguration.TargetPlatform);
    }

    [TestMethod]
    public void ExecuteShouldInstrumentVsTestConsoleStart()
    {
        var mockOutput = new MockOutput();
        var exitCode = new Executor(mockOutput, _mockTestPlatformEventSource.Object, new ProcessHelper(), new PlatformEnvironment()).Execute(It.IsAny<string[]>());

        _mockTestPlatformEventSource.Verify(x => x.VsTestConsoleStart(), Times.Once);
    }

    [TestMethod]
    public void ExecuteShouldInstrumentVsTestConsoleStop()
    {
        var mockOutput = new MockOutput();
        var exitCode = new Executor(mockOutput, _mockTestPlatformEventSource.Object, new ProcessHelper(), new PlatformEnvironment()).Execute(It.IsAny<string[]>());

        _mockTestPlatformEventSource.Verify(x => x.VsTestConsoleStop(), Times.Once);
    }

    [TestMethod]
    public void ExecuteShouldExitWithErrorOnResponseFileException()
    {
        string[] args = ["@FileDoesNotExist.rsp"];
        var mockOutput = new MockOutput();

        var exitCode = new Executor(mockOutput, _mockTestPlatformEventSource.Object, new ProcessHelper(), new PlatformEnvironment()).Execute(args);

        var errorMessageCount = mockOutput.Messages.Count(msg => msg.Level == OutputLevel.Error && msg.Message!.Contains(
            string.Format(CultureInfo.CurrentCulture, CommandLineResources.OpenResponseFileError, args[0].Substring(1))));
        Assert.AreEqual(1, errorMessageCount, "Response File Exception should display error.");
        Assert.AreEqual(1, exitCode, "Response File Exception execution should exit with error.");
    }

    [TestMethod]
    [Ignore("TODO: gets stuck when running with other tests in ExecutorUnitTests")]
    [DataRow("--ShowDeprecateDotnetVSTestMessage")]
    [DataRow("--showdeprecateDotnetvsTestMessage")]
    public void ExecutorShouldPrintDotnetVSTestDeprecationMessage(string commandLine)
    {
        var mockOutput = new MockOutput();
        Mock<IProcessHelper> processHelper = new();
        processHelper.Setup(x => x.GetCurrentProcessArchitecture()).Returns(PlatformArchitecture.X64);
        processHelper.Setup(x => x.GetCurrentProcessId()).Returns(0);
        Mock<IEnvironment> environment = new();
        environment.Setup(x => x.Architecture).Returns(PlatformArchitecture.X64);

        new Executor(mockOutput, _mockTestPlatformEventSource.Object, processHelper.Object, environment.Object).Execute(commandLine);

        Assert.HasCount(5, mockOutput.Messages);
        Assert.AreEqual(OutputLevel.Warning, mockOutput.Messages[2].Level);
        Assert.AreEqual("The dotnet vstest command is superseded by dotnet test, which can now be used to run assemblies. See https://aka.ms/dotnet-test.", mockOutput.Messages[2].Message);
    }

    [TestMethod]
    public void ExecuteShouldNotThrowSettingsExceptionButLogOutput()
    {
        var runSettingsFile = Path.Combine(Path.GetTempPath(), "ExecutorShouldShowRightErrorMessage.runsettings");

        try
        {
            if (File.Exists(runSettingsFile))
            {
                File.Delete(runSettingsFile);
            }

            var fileContents = @"<RunSettings>
                                    <LoggerRunSettings>
                                        <Loggers>
                                            <Logger invalidName=""trx"" />
                                        </Loggers>
                                    </LoggerRunSettings>
                                </RunSettings>";

            File.WriteAllText(runSettingsFile, fileContents);

            var testSourceDllPath = Path.GetTempFileName();
            string[] args = [testSourceDllPath, "/settings:" + runSettingsFile];
            var mockOutput = new MockOutput();

            var exitCode = new Executor(mockOutput, _mockTestPlatformEventSource.Object, new ProcessHelper(), new PlatformEnvironment()).Execute(args);

            var result = mockOutput.Messages.Any(o => o.Level == OutputLevel.Error && o.Message!.Contains("Invalid settings 'Logger'. Unexpected XmlAttribute: 'invalidName'."));
            Assert.IsTrue(result, "expecting error message : Invalid settings 'Logger'.Unexpected XmlAttribute: 'invalidName'.");
        }
        finally
        {
            File.Delete(runSettingsFile);
        }
    }

    [TestMethod]
    public void ExecuteShouldReturnNonZeroExitCodeIfSettingsException()
    {
        var runSettingsFile = Path.Combine(Path.GetTempPath(), "ExecutorShouldShowRightErrorMessage.runsettings");

        try
        {
            if (File.Exists(runSettingsFile))
            {
                File.Delete(runSettingsFile);
            }

            var fileContents = @"<RunSettings>
                                    <LoggerRunSettings>
                                        <Loggers>
                                            <Logger invalidName=""trx"" />
                                        </Loggers>
                                    </LoggerRunSettings>
                                </RunSettings>";

            File.WriteAllText(runSettingsFile, fileContents);

            string[] args = ["/settings:" + runSettingsFile];
            var mockOutput = new MockOutput();

            var exitCode = new Executor(mockOutput, _mockTestPlatformEventSource.Object, new ProcessHelper(), new PlatformEnvironment()).Execute(args);

            Assert.AreEqual(1, exitCode, "Exit code should be one because it throws exception");
        }
        finally
        {
            File.Delete(runSettingsFile);
        }
    }

    [TestMethod]
    public void ExecutorShouldShowRightErrorMessage()
    {
        var runSettingsFile = Path.Combine(Path.GetTempPath(), "ExecutorShouldShowRightErrorMessage.runsettings");

        try
        {
            if (File.Exists(runSettingsFile))
            {
                File.Delete(runSettingsFile);
            }

            var fileContents = @"<RunSettings>
                                    <RunConfiguration>
                                        <TargetPlatform>Invalid</TargetPlatform>
                                    </RunConfiguration>
                                </RunSettings>";

            File.WriteAllText(runSettingsFile, fileContents);

            string[] args = ["/settings:" + runSettingsFile];
            var mockOutput = new MockOutput();

            var exitCode = new Executor(mockOutput, _mockTestPlatformEventSource.Object, new ProcessHelper(), new PlatformEnvironment()).Execute(args);

            var result = mockOutput.Messages.Any(o => o.Level == OutputLevel.Error && o.Message!.Contains("Invalid setting 'RunConfiguration'. Invalid value 'Invalid' specified for 'TargetPlatform'."));
            Assert.AreEqual(1, exitCode, "Exit code should be one because it throws exception");
            Assert.IsTrue(result, "expecting error message : Invalid setting 'RunConfiguration'. Invalid value 'Invalid' specified for 'TargetPlatform'.");
        }
        finally
        {
            File.Delete(runSettingsFile);
        }
    }

    [TestMethod]
    [TestCategory("Windows")]
    public void ExecutorShouldPrintWarningIfRunningEmulatedOnARM64()
    {
        var mockOutput = new MockOutput();
        Mock<IProcessHelper> processHelper = new();
        processHelper.Setup(x => x.GetCurrentProcessArchitecture()).Returns(PlatformArchitecture.X64);
        processHelper.Setup(x => x.GetCurrentProcessId()).Returns(0);
        processHelper.Setup(x => x.GetCurrentProcessFileName()).Returns(@"X:\vstest.console.exe");
        Mock<IEnvironment> environment = new();
        environment.Setup(x => x.Architecture).Returns(PlatformArchitecture.ARM64);

        var exitCode = new Executor(mockOutput, _mockTestPlatformEventSource.Object, processHelper.Object, environment.Object).Execute();
        var assemblyVersion = typeof(Executor).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion;

        Assert.HasCount(4, mockOutput.Messages);
        Assert.AreEqual("vstest.console.exe is running in emulated mode as x64. For better performance, please consider using the native runner vstest.console.arm64.exe.",
            mockOutput.Messages[1].Message);
        Assert.AreEqual(OutputLevel.Warning,
            mockOutput.Messages[1].Level);
    }

    [TestMethod]
    public void ExecutorShouldPrintRunnerArchitecture()
    {
        var mockOutput = new MockOutput();
        Mock<IProcessHelper> processHelper = new();
        processHelper.Setup(x => x.GetCurrentProcessArchitecture()).Returns(PlatformArchitecture.X64);
        processHelper.Setup(x => x.GetCurrentProcessId()).Returns(0);
        Mock<IEnvironment> environment = new();
        environment.Setup(x => x.Architecture).Returns(PlatformArchitecture.X64);

        var exitCode = new Executor(mockOutput, _mockTestPlatformEventSource.Object, processHelper.Object, environment.Object).Execute();
        var assemblyVersion = typeof(Executor).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion;

        Assert.HasCount(3, mockOutput.Messages);
        Assert.MatchesRegex(@"VSTest version .* \(x64\)", mockOutput.Messages[0].Message!);
        Assert.DoesNotContain(message => message.Message!.Contains("vstest.console.exe is running in emulated mode"), mockOutput.Messages);
    }

    [TestMethod]
    public void MarkingTestRunFailedOnInjectedAggregatorIsObservedByExecutorExitCode()
    {
        // The exit code produced at the end of Executor.Execute is OR-ed with the outcome of the
        // TestRunResultAggregator (Executor.cs: exitCode |= (Outcome == Passed) ? 0 : 1). This test
        // proves the reader (Executor) observes the SAME aggregator instance it was constructed with,
        // and that separate aggregator instances are isolated from one another (no shared state).
        //
        // "--help" is a zero-baseline path: HelpArgumentProcessor runs first and returns Abort, which
        // does not set the exit bit (only Fail does), so the aggregator's outcome is the sole
        // contributor to the final exit code. That makes the two outcomes below decisively distinct.

        // Writer: mark a failure on the injected aggregator.
        var injectedAggregator = new DummyTestRunResultAggregator();
        injectedAggregator.MarkTestRunFailed();

        // Reader observes the write through the injected instance: Failed outcome sets the exit bit.
        var exitCodeWithInjected = new Executor(
            new MockOutput(),
            _mockTestPlatformEventSource.Object,
            new ProcessHelper(),
            new PlatformEnvironment(),
            _runSettingsManager,
            RunSettingsHelper.Instance,
            _commandLineOptions,
            injectedAggregator).Execute("--help");

        Assert.AreEqual(1, exitCodeWithInjected, "Executor must observe the injected aggregator's Failed outcome.");

        // Negative control: an Executor bound to a separate, default aggregator (still Passed) yields a
        // zero exit for the same args, and the write above did not leak onto this other instance.
        var defaultAggregator = new TestRunResultAggregator();
        var exitCodeWithDefault = new Executor(
            new MockOutput(),
            _mockTestPlatformEventSource.Object,
            new ProcessHelper(),
            new PlatformEnvironment(),
            _runSettingsManager,
            RunSettingsHelper.Instance,
            _commandLineOptions,
            defaultAggregator).Execute("--help");

        Assert.AreEqual(0, exitCodeWithDefault, "A separate default aggregator is still Passed, so its Executor must not set the failure bit.");
        Assert.AreEqual(TestOutcome.Passed, defaultAggregator.Outcome, "Marking the injected aggregator failed must not leak onto other aggregator instances.");
    }

    private class MockOutput : IOutput
    {
        public List<OutputMessage> Messages { get; set; } = new List<OutputMessage>();

        public void Write(string? message, OutputLevel level)
        {
            Messages.Add(new OutputMessage() { Message = message, Level = level });
        }

        public void WriteLine(string? message, OutputLevel level)
        {
            Messages.Add(new OutputMessage() { Message = message, Level = level });
        }
    }

    private class OutputMessage
    {
        public string? Message { get; set; } = "";
        public OutputLevel Level { get; set; }
    }
}
