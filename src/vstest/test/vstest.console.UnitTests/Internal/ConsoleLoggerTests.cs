// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;

using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.VisualStudio.TestPlatform.CommandLine.Internal;
using Microsoft.VisualStudio.TestPlatform.CommandLine.UnitTests.Processors;
using Microsoft.VisualStudio.TestPlatform.Common.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.TestPlatform.TestUtilities;

using Moq;

using vstest.console.Internal;

using CommandLineResources = Microsoft.VisualStudio.TestPlatform.CommandLine.Resources.Resources;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.UnitTests.Internal;

[TestClass]
public class ConsoleLoggerTests
{
    private readonly Mock<IRequestData> _mockRequestData;
    private readonly Mock<IMetricsCollection> _mockMetricsCollection;
    private readonly Mock<IOutput> _mockOutput;
    private readonly ConsoleLogger _consoleLogger;
    private readonly Mock<IProgressIndicator> _mockProgressIndicator;
    private readonly Mock<IFeatureFlag> _mockFeatureFlag;

    private const string PassedTestIndicator = "  Passed ";
    private const string FailedTestIndicator = "  Failed ";
    private const string SkippedTestIndicator = "  Skipped ";

    public ConsoleLoggerTests()
    {
        _mockRequestData = new Mock<IRequestData>();
        _mockMetricsCollection = new Mock<IMetricsCollection>();
        _mockFeatureFlag = new Mock<IFeatureFlag>();
        _mockFeatureFlag.Setup(x => x.IsSet(It.IsAny<string>())).Returns(false);
        _mockRequestData.Setup(rd => rd.MetricsCollection).Returns(_mockMetricsCollection.Object);

        _mockOutput = new Mock<IOutput>();
        _mockProgressIndicator = new Mock<IProgressIndicator>();
        _consoleLogger = new ConsoleLogger(_mockOutput.Object, _mockProgressIndicator.Object, _mockFeatureFlag.Object);

        RunTestsArgumentProcessorTests.SetupMockExtensions();
    }

    [TestMethod]
    public void InitializeShouldThrowExceptionIfEventsIsNull()
    {
        Assert.ThrowsException<ArgumentNullException>(() => _consoleLogger.Initialize(null!, string.Empty));
    }

    [TestMethod]
    public void InitializeShouldNotThrowExceptionIfEventsIsNotNull()
    {
        _consoleLogger.Initialize(new Mock<TestLoggerEvents>().Object, string.Empty);
    }

    [TestMethod]
    public void InitializeWithParametersShouldThrowExceptionIfEventsIsNull()
    {
        var parameters = new Dictionary<string, string?>
        {
            { "param1", "value" },
        };

        Assert.ThrowsException<ArgumentNullException>(() => _consoleLogger.Initialize(null!, parameters));
    }

    [TestMethod]
    public void InitializeWithParametersShouldThrowExceptionIfParametersIsEmpty()
    {
        Assert.ThrowsException<ArgumentException>(() => _consoleLogger.Initialize(new Mock<TestLoggerEvents>().Object, new Dictionary<string, string?>()));
    }

    [TestMethod]
    public void InitializeWithParametersShouldThrowExceptionIfParametersIsNull()
    {
        Assert.ThrowsException<ArgumentNullException>(() => _consoleLogger.Initialize(new Mock<TestLoggerEvents>().Object, (Dictionary<string, string?>)null!));
    }

    [TestMethod]
    public void InitializeWithParametersShouldSetVerbosityLevel()
    {
        var parameters = new Dictionary<string, string?>
        {
            { "verbosity", "minimal" },
            { DefaultLoggerParameterNames.TargetFramework , "net462"}
        };
        _consoleLogger.Initialize(new Mock<TestLoggerEvents>().Object, parameters);

        Assert.AreEqual(ConsoleLogger.Verbosity.Minimal, _consoleLogger.VerbosityLevel);
    }

    [TestMethod]
    public void InitializeWithParametersShouldDefaultToNormalVerbosityLevelForInvalidVerbosity()
    {
        var parameters = new Dictionary<string, string?>
        {
            { "verbosity", "" },
        };

        _consoleLogger.Initialize(new Mock<TestLoggerEvents>().Object, parameters);

#if NETFRAMEWORK
        Assert.AreEqual(ConsoleLogger.Verbosity.Normal, _consoleLogger.VerbosityLevel);
#else
        Assert.AreEqual(ConsoleLogger.Verbosity.Minimal, _consoleLogger.VerbosityLevel);
#endif
    }

    [TestMethod]
    public void InitializeWithParametersShouldSetPrefixValue()
    {
        var parameters = new Dictionary<string, string?>
        {
            { "prefix", "true" },
        };
        Assert.IsFalse(ConsoleLogger.AppendPrefix);

        _consoleLogger.Initialize(new Mock<TestLoggerEvents>().Object, parameters);

        Assert.IsTrue(ConsoleLogger.AppendPrefix);
        ConsoleLogger.AppendPrefix = false;
    }

    [TestMethod]
    public void InitializeWithParametersShouldSetNoProgress()
    {
        var parameters = new Dictionary<string, string?>();

        Assert.IsFalse(ConsoleLogger.EnableProgress);

        parameters.Add("progress", "true");
        _consoleLogger.Initialize(new Mock<TestLoggerEvents>().Object, parameters);

        Assert.IsTrue(ConsoleLogger.EnableProgress);

        ConsoleLogger.EnableProgress = false;
    }

    [TestMethod]
    public void TestMessageHandlerShouldThrowExceptionIfEventArgsIsNull()
    {
        var loggerEvents = new InternalTestLoggerEvents(TestSessionMessageLogger.Instance);
        loggerEvents.EnableEvents();

        Assert.ThrowsException<ArgumentNullException>(() => loggerEvents.RaiseTestRunMessage(default!));
    }

    [TestMethod]
    public void TestMessageHandlerShouldWriteToConsoleWhenTestRunMessageIsRaised()
    {
        var count = 0;
        _mockOutput.Setup(o => o.WriteLine(It.IsAny<string>(), It.IsAny<OutputLevel>())).Callback<string, OutputLevel>(
            (s, o) => count++);

        SetupForTestMessageHandler(out var loggerEvents);

        loggerEvents.RaiseTestRunMessage(new TestRunMessageEventArgs(TestMessageLevel.Informational, "Informational123"));
        loggerEvents.RaiseTestRunMessage(new TestRunMessageEventArgs(TestMessageLevel.Error, "Error123"));
        loggerEvents.RaiseTestRunMessage(new TestRunMessageEventArgs(TestMessageLevel.Warning, "Warning123"));
        loggerEvents.WaitForEventCompletion();

        // Added this for synchronization
        SpinWait.SpinUntil(() => count == 3, 300);

        AssertsForTestMessageHandler();
        _mockProgressIndicator.Verify(pi => pi.Pause(), Times.Exactly(3));
        _mockProgressIndicator.Verify(pi => pi.Start(), Times.Exactly(3));
    }

    [TestMethod]
    public void TestMessageHandlerShouldWriteToConsoleWhenTestDiscoveryMessageIsRaised()
    {
        var count = 0;
        _mockOutput.Setup(o => o.WriteLine(It.IsAny<string>(), It.IsAny<OutputLevel>())).Callback<string, OutputLevel>(
            (s, o) => count++);

        SetupForTestMessageHandler(out var loggerEvents);

        loggerEvents.RaiseDiscoveryMessage(new TestRunMessageEventArgs(TestMessageLevel.Informational, "Informational123"));
        loggerEvents.RaiseDiscoveryMessage(new TestRunMessageEventArgs(TestMessageLevel.Error, "Error123"));
        loggerEvents.RaiseDiscoveryMessage(new TestRunMessageEventArgs(TestMessageLevel.Warning, "Warning123"));
        loggerEvents.WaitForEventCompletion();

        // Added this for synchronization
        SpinWait.SpinUntil(() => count == 3, 300);

        AssertsForTestMessageHandler();
        _mockProgressIndicator.Verify(pi => pi.Pause(), Times.Exactly(3));
        _mockProgressIndicator.Verify(pi => pi.Start(), Times.Exactly(3));
    }

    private void AssertsForTestMessageHandler()
    {
        _mockOutput.Verify(o => o.WriteLine("Informational123", OutputLevel.Information), Times.Once());
        _mockOutput.Verify(o => o.WriteLine("Warning123", OutputLevel.Warning), Times.Once());
        _mockOutput.Verify(o => o.WriteLine("Error123", OutputLevel.Error), Times.Once());
    }

    private void SetupForTestMessageHandler(out InternalTestLoggerEvents loggerEvents)
    {
        loggerEvents = new InternalTestLoggerEvents(TestSessionMessageLogger.Instance);
        loggerEvents.EnableEvents();
        var parameters = new Dictionary<string, string?>
        {
            { "verbosity", "normal" }
        };
        _consoleLogger.Initialize(loggerEvents, parameters);
    }

    [TestMethod]
    public void TestResultHandlerShouldThowExceptionIfEventArgsIsNull()
    {
        var loggerEvents = new InternalTestLoggerEvents(TestSessionMessageLogger.Instance);
        loggerEvents.EnableEvents();

        Assert.ThrowsException<ArgumentNullException>(() => loggerEvents.RaiseTestResult(default!));
    }

    [TestMethod]
    public void TestResultHandlerShouldShowStdOutMessagesBannerIfStdOutIsNotEmpty()
    {
        var count = 0;
        _mockOutput.Setup(o => o.WriteLine(It.IsAny<string>(), It.IsAny<OutputLevel>())).Callback<string, OutputLevel>(
            (s, o) => count++);

        var loggerEvents = new InternalTestLoggerEvents(TestSessionMessageLogger.Instance);
        loggerEvents.EnableEvents();
        var parameters = new Dictionary<string, string?>
        {
            { "verbosity", "normal" }
        };
        _consoleLogger.Initialize(loggerEvents, parameters);

        string message = "Dummy message";
        var testcase = new TestCase("TestName", new Uri("some://uri"), "TestSource");
        TestResultMessage testResultMessage = new(TestResultMessage.StandardOutCategory, message);
        var testresult = new ObjectModel.TestResult(testcase)
        {
            Outcome = TestOutcome.Failed
        };
        testresult.Messages.Add(testResultMessage);

        loggerEvents.RaiseTestResult(new TestResultEventArgs(testresult));
        loggerEvents.WaitForEventCompletion();

        // Added this for synchronization
        SpinWait.SpinUntil(() => count == 2, 300);

        _mockOutput.Verify(o => o.WriteLine("  Standard Output Messages:", OutputLevel.Information), Times.Once());
        _mockOutput.Verify(o => o.WriteLine(" " + message, OutputLevel.Information), Times.Once());
    }

    [TestMethod]
    public void NormalVerbosityShowNotStdOutMessagesForPassedTests()
    {
        // Setup
        var loggerEvents = new InternalTestLoggerEvents(TestSessionMessageLogger.Instance);
        loggerEvents.EnableEvents();
        var parameters = new Dictionary<string, string?>
        {
            { "verbosity", "normal" }
        };

        _consoleLogger.Initialize(loggerEvents, parameters);
        var testcase = new TestCase("TestName", new Uri("some://uri"), "TestSource");
        string message = "Dummy message";
        TestResultMessage testResultMessage = new(TestResultMessage.StandardOutCategory, message);

        var testresult = new ObjectModel.TestResult(testcase);
        testresult.Outcome = TestOutcome.Passed;
        testresult.Messages.Add(testResultMessage);

        // Raise an event on mock object
        loggerEvents.RaiseTestResult(new TestResultEventArgs(testresult));
        loggerEvents.WaitForEventCompletion();

        // Verify
        _mockOutput.Verify(o => o.WriteLine(CommandLineResources.StdOutMessagesBanner, OutputLevel.Information), Times.Never());
        _mockOutput.Verify(o => o.WriteLine(" " + message, OutputLevel.Information), Times.Never());
    }

    [TestMethod]
    public void DetailedVerbosityShowStdOutMessagesForPassedTests()
    {
        // Setup
        var loggerEvents = new InternalTestLoggerEvents(TestSessionMessageLogger.Instance);
        loggerEvents.EnableEvents();
        var parameters = new Dictionary<string, string?>
        {
            { "verbosity", "detailed" }
        };

        _consoleLogger.Initialize(loggerEvents, parameters);
        var testcase = new TestCase("TestName", new Uri("some://uri"), "TestSource");
        string message = "Dummy message";

        TestResultMessage testResultMessage = new(TestResultMessage.StandardOutCategory, message);
        var testresult = new ObjectModel.TestResult(testcase)
        {
            Outcome = TestOutcome.Passed
        };

        testresult.Messages.Add(testResultMessage);

        // Act. Raise an event on mock object
        loggerEvents.RaiseTestResult(new TestResultEventArgs(testresult));
        loggerEvents.WaitForEventCompletion();

        // Verify
        _mockOutput.Verify(o => o.WriteLine("  Standard Output Messages:", OutputLevel.Information), Times.Once());
        _mockOutput.Verify(o => o.WriteLine(" " + message, OutputLevel.Information), Times.Once());
    }

    [TestMethod]
    public void TestRunErrorMessageShowShouldTestRunFailed()
    {
        // Setup
        var loggerEvents = new InternalTestLoggerEvents(TestSessionMessageLogger.Instance);
        loggerEvents.EnableEvents();
        var parameters = new Dictionary<string, string?>
        {
            { "verbosity", "detailed" }
        };

        _consoleLogger.Initialize(loggerEvents, parameters);
        string message = "Adapter Error";

        // Act. Raise an event on mock object
        loggerEvents.RaiseTestRunMessage(new TestRunMessageEventArgs(TestMessageLevel.Error, message));
        loggerEvents.RaiseTestRunComplete(new TestRunCompleteEventArgs(new Mock<ITestRunStatistics>().Object, false, false, null, new Collection<AttachmentSet>(), new Collection<InvokedDataCollector>(), TimeSpan.FromSeconds(1)));
        loggerEvents.WaitForEventCompletion();

        // Verify
        _mockOutput.Verify(o => o.WriteLine(CommandLineResources.TestRunFailed, OutputLevel.Error), Times.Once());
        _mockOutput.Verify(o => o.WriteLine(message, OutputLevel.Error), Times.Once());
    }

    [TestMethod]
    public void InQuietModeTestErrorMessageShouldShowTestRunFailed()
    {
        // Setup
        var loggerEvents = new InternalTestLoggerEvents(TestSessionMessageLogger.Instance);
        loggerEvents.EnableEvents();
        var parameters = new Dictionary<string, string?>
        {
            { "verbosity", "quiet" },
            { DefaultLoggerParameterNames.TargetFramework , "abc" }
        };

        _consoleLogger.Initialize(loggerEvents, parameters);
        string message = "Adapter Error";

        // Act. Raise an event on mock object
        loggerEvents.RaiseTestRunMessage(new TestRunMessageEventArgs(TestMessageLevel.Error, message));
        loggerEvents.RaiseTestRunComplete(new TestRunCompleteEventArgs(new Mock<ITestRunStatistics>().Object, false, false, null, new Collection<AttachmentSet>(), new Collection<InvokedDataCollector>(), TimeSpan.FromSeconds(1)));
        loggerEvents.WaitForEventCompletion();

        // Verify
        _mockOutput.Verify(o => o.WriteLine(message, OutputLevel.Error), Times.Once());
    }

    [TestMethod]
    public void InQuietModeTestWarningMessageShouldNotShow()
    {
        // Setup
        var loggerEvents = new InternalTestLoggerEvents(TestSessionMessageLogger.Instance);
        loggerEvents.EnableEvents();
        var parameters = new Dictionary<string, string?>
        {
            { "verbosity", "quiet" },
            { DefaultLoggerParameterNames.TargetFramework , "abc" }
        };

        _consoleLogger.Initialize(loggerEvents, parameters);
        string message = "Adapter Warning";

        // Act. Raise an event on mock object
        loggerEvents.RaiseTestRunMessage(new TestRunMessageEventArgs(TestMessageLevel.Warning, message));
        loggerEvents.RaiseTestRunComplete(new TestRunCompleteEventArgs(new Mock<ITestRunStatistics>().Object, false, false, null, new Collection<AttachmentSet>(), new Collection<InvokedDataCollector>(), TimeSpan.FromSeconds(1)));
        loggerEvents.WaitForEventCompletion();

        // Verify
        _mockOutput.Verify(o => o.WriteLine(message, OutputLevel.Warning), Times.Never());
    }

    [TestMethod]
    public void InNormalModeTestWarningAndErrorMessagesShouldShow()
    {
        // Setup
        var loggerEvents = new InternalTestLoggerEvents(TestSessionMessageLogger.Instance);
        loggerEvents.EnableEvents();
        var parameters = new Dictionary<string, string?>
        {
            { "verbosity", "normal" }
        };

        _consoleLogger.Initialize(loggerEvents, parameters);
        string message = "Adapter Warning";
        string errorMessage = "Adapter Error";

        // Act. Raise an event on mock object
        loggerEvents.RaiseTestRunMessage(new TestRunMessageEventArgs(TestMessageLevel.Warning, message));
        loggerEvents.RaiseTestRunMessage(new TestRunMessageEventArgs(TestMessageLevel.Error, errorMessage));
        loggerEvents.RaiseTestRunComplete(new TestRunCompleteEventArgs(new Mock<ITestRunStatistics>().Object, false, false, null, new Collection<AttachmentSet>(), new Collection<InvokedDataCollector>(), TimeSpan.FromSeconds(1)));
        loggerEvents.WaitForEventCompletion();

        // Verify
        _mockOutput.Verify(o => o.WriteLine(CommandLineResources.TestRunFailed, OutputLevel.Error), Times.Once());
        _mockOutput.Verify(o => o.WriteLine(message, OutputLevel.Warning), Times.Once());
        _mockOutput.Verify(o => o.WriteLine(errorMessage, OutputLevel.Error), Times.Once());
    }

    [TestMethod]
    public void TestResultHandlerShouldNotShowStdOutMessagesBannerIfStdOutIsEmpty()
    {
        var loggerEvents = new InternalTestLoggerEvents(TestSessionMessageLogger.Instance);
        loggerEvents.EnableEvents();
        var parameters = new Dictionary<string, string?>
        {
            { "verbosity", "normal" }
        };
        _consoleLogger.Initialize(loggerEvents, parameters);

        var testcase = new TestCase("TestName", new Uri("some://uri"), "TestSource");
        TestResultMessage testResultMessage = new(TestResultMessage.StandardOutCategory, null);
        var testresult = new ObjectModel.TestResult(testcase)
        {
            Outcome = TestOutcome.Failed
        };
        testresult.Messages.Add(testResultMessage);

        loggerEvents.RaiseTestResult(new TestResultEventArgs(testresult));
        loggerEvents.WaitForEventCompletion();

        _mockOutput.Verify(o => o.WriteLine(CommandLineResources.StdOutMessagesBanner, OutputLevel.Information), Times.Never());
    }

    [TestMethod]
    public void TestResultHandlerShouldShowStdErrMessagesBannerIfStdErrIsNotEmpty()
    {
        var loggerEvents = new InternalTestLoggerEvents(TestSessionMessageLogger.Instance);
        loggerEvents.EnableEvents();
        var parameters = new Dictionary<string, string?>
        {
            { "verbosity", "normal" }
        };
        _consoleLogger.Initialize(loggerEvents, parameters);

        var testcase = new TestCase("TestName", new Uri("some://uri"), "TestSource");
        string message = "Dummy message";
        TestResultMessage testResultMessage = new(TestResultMessage.StandardErrorCategory, message);
        var testresult = new ObjectModel.TestResult(testcase)
        {
            Outcome = TestOutcome.Failed
        };
        testresult.Messages.Add(testResultMessage);

        loggerEvents.RaiseTestResult(new TestResultEventArgs(testresult));
        loggerEvents.WaitForEventCompletion();

        _mockOutput.Verify(o => o.WriteLine("  Standard Error Messages:", OutputLevel.Information), Times.Once());
        _mockOutput.Verify(o => o.WriteLine(" " + message, OutputLevel.Information), Times.Once());
    }

    [TestMethod]
    public void TestResultHandlerShouldNotShowStdErrMessagesBannerIfStdErrIsEmpty()
    {
        var loggerEvents = new InternalTestLoggerEvents(TestSessionMessageLogger.Instance);
        loggerEvents.EnableEvents();
        var parameters = new Dictionary<string, string?>
        {
            { "verbosity", "normal" }
        };
        _consoleLogger.Initialize(loggerEvents, parameters);

        var testcase = new TestCase("TestName", new Uri("some://uri"), "TestSource");
        TestResultMessage testResultMessage = new(TestResultMessage.StandardErrorCategory, null);
        var testresult = new ObjectModel.TestResult(testcase)
        {
            Outcome = TestOutcome.Failed
        };
        testresult.Messages.Add(testResultMessage);

        loggerEvents.RaiseTestResult(new TestResultEventArgs(testresult));
        loggerEvents.WaitForEventCompletion();

        _mockOutput.Verify(o => o.WriteLine(CommandLineResources.StdErrMessagesBanner, OutputLevel.Information), Times.Never());
    }

    [TestMethod]
    public void TestResultHandlerShouldShowAdditionalInfoBannerIfAdditionalInfoIsNotEmpty()
    {
        var loggerEvents = new InternalTestLoggerEvents(TestSessionMessageLogger.Instance);
        loggerEvents.EnableEvents();
        var parameters = new Dictionary<string, string?>
        {
            { "verbosity", "normal" }
        };
        _consoleLogger.Initialize(loggerEvents, parameters);

        var testcase = new TestCase("TestName", new Uri("some://uri"), "TestSource");
        string message = "Dummy message";
        TestResultMessage testResultMessage = new(TestResultMessage.AdditionalInfoCategory, message);
        var testresult = new ObjectModel.TestResult(testcase)
        {
            Outcome = TestOutcome.Failed
        };
        testresult.Messages.Add(testResultMessage);

        loggerEvents.RaiseTestResult(new TestResultEventArgs(testresult));
        loggerEvents.WaitForEventCompletion();

        _mockOutput.Verify(o => o.WriteLine("  Additional Information Messages:", OutputLevel.Information), Times.Once());
        _mockOutput.Verify(o => o.WriteLine(" " + message, OutputLevel.Information), Times.Once());
    }

    [TestMethod]
    public void TestResultHandlerShouldNotShowAdditionalInfoBannerIfAdditionalInfoIsEmpty()
    {
        var loggerEvents = new InternalTestLoggerEvents(TestSessionMessageLogger.Instance);
        loggerEvents.EnableEvents();
        var parameters = new Dictionary<string, string?>
        {
            { "verbosity", "normal" }
        };
        _consoleLogger.Initialize(loggerEvents, parameters);

        var testcase = new TestCase("TestName", new Uri("some://uri"), "TestSource");

        TestResultMessage testResultMessage = new(TestResultMessage.AdditionalInfoCategory, null);

        var testresult = new ObjectModel.TestResult(testcase)
        {
            Outcome = TestOutcome.Failed
        };
        testresult.Messages.Add(testResultMessage);

        loggerEvents.RaiseTestResult(new TestResultEventArgs(testresult));
        loggerEvents.WaitForEventCompletion();

        _mockOutput.Verify(o => o.WriteLine(CommandLineResources.AddnlInfoMessagesBanner, OutputLevel.Information), Times.Never());
    }

    [TestMethod]
    public void TestResultHandlerShouldShowPassedTestsForNormalVebosity()
    {
        var loggerEvents = new InternalTestLoggerEvents(TestSessionMessageLogger.Instance);
        loggerEvents.EnableEvents();
        var parameters = new Dictionary<string, string?>
        {
            { "verbosity", "normal" }
        };
        _consoleLogger.Initialize(loggerEvents, parameters);

        foreach (var testResult in GetTestResultsObject())
        {
            loggerEvents.RaiseTestResult(new TestResultEventArgs(testResult));
        }
        loggerEvents.WaitForEventCompletion();

        _mockOutput.Verify(o => o.Write(PassedTestIndicator, OutputLevel.Information), Times.Once);
        _mockOutput.Verify(o => o.WriteLine("TestName [1 h 2 m]", OutputLevel.Information), Times.Once);
        _mockOutput.Verify(o => o.Write(FailedTestIndicator, OutputLevel.Information), Times.Once);
        _mockOutput.Verify(o => o.WriteLine("TestName [4 m 5 s]", OutputLevel.Information), Times.Once());
        _mockOutput.Verify(o => o.Write(SkippedTestIndicator, OutputLevel.Information), Times.Exactly(3));
        _mockOutput.Verify(o => o.WriteLine("TestName", OutputLevel.Information), Times.Exactly(3));
        _mockProgressIndicator.Verify(pi => pi.Pause(), Times.Exactly(5));
        _mockProgressIndicator.Verify(pi => pi.Start(), Times.Exactly(5));
    }

    [DataRow(".NETFramework,version=v4.6.2", "(net462)", "quiet")]
    [DataRow(".NETFramework,version=v4.6.2", "(net462)", "minimal")]
    [DataRow(null, null, "quiet")]
    [DataRow(null, null, "minimal")]
    [TestMethod]
    public void TestResultHandlerShouldShowFailedTestsAndPassedTestsForQuietVerbosity(string framework, string expectedFramework, string verbosityLevel)
    {
        var loggerEvents = new InternalTestLoggerEvents(TestSessionMessageLogger.Instance);
        loggerEvents.EnableEvents();
        var parameters = new Dictionary<string, string?>
        {
            { "verbosity", verbosityLevel },
            { DefaultLoggerParameterNames.TargetFramework , framework}
        };
        _consoleLogger.Initialize(loggerEvents, parameters);

        foreach (var testResult in GetTestResultsObject())
        {
            loggerEvents.RaiseTestResult(new TestResultEventArgs(testResult));
        }

        foreach (var testResult in GetPassedTestResultsObject())
        {
            loggerEvents.RaiseTestResult(new TestResultEventArgs(testResult));
        }

        loggerEvents.RaiseTestRunComplete(new TestRunCompleteEventArgs(new Mock<ITestRunStatistics>().Object, false, false, null, new Collection<AttachmentSet>(), new Collection<InvokedDataCollector>(), TimeSpan.FromSeconds(1)));
        loggerEvents.WaitForEventCompletion();

        _mockOutput.Verify(o => o.Write(string.Format(CultureInfo.CurrentCulture, CommandLineResources.TestRunSummary,
            new[] { (CommandLineResources.PassedTestIndicator + "!").PadRight(8),
            0.ToString(CultureInfo.InvariantCulture).PadLeft(5),
            1.ToString(CultureInfo.InvariantCulture).PadLeft(5),
            1.ToString(CultureInfo.InvariantCulture).PadLeft(5),
            2.ToString(CultureInfo.InvariantCulture).PadLeft(5),
            "1 m 2 s"}), OutputLevel.Information), Times.Once);

        _mockOutput.Verify(o => o.WriteLine(string.Format(CultureInfo.CurrentCulture, CommandLineResources.TestRunSummaryAssemblyAndFramework,
            "TestSourcePassed",
            expectedFramework), OutputLevel.Information), Times.Once);

        _mockOutput.Verify(o => o.Write(string.Format(CultureInfo.CurrentCulture, CommandLineResources.TestRunSummary,
            new[] { (CommandLineResources.FailedTestIndicator + "!").PadRight(8),
            1.ToString(CultureInfo.InvariantCulture).PadLeft(5),
            1.ToString(CultureInfo.InvariantCulture).PadLeft(5),
            1.ToString(CultureInfo.InvariantCulture).PadLeft(5),
            3.ToString(CultureInfo.InvariantCulture).PadLeft(5),
            "1 h 2 m" }), OutputLevel.Information), Times.Once);

        _mockOutput.Verify(o => o.WriteLine(string.Format(CultureInfo.CurrentCulture, CommandLineResources.TestRunSummaryAssemblyAndFramework,
            "TestSource",
            expectedFramework), OutputLevel.Information), Times.Once);
    }

    [TestMethod]
    [DataRow("normal")]
    [DataRow("detailed")]
    public void TestResultHandlerShouldNotShowformattedFailedTestsAndPassedTestsForOtherThanQuietVerbosity(string verbosityLevel)
    {
        var loggerEvents = new InternalTestLoggerEvents(TestSessionMessageLogger.Instance);
        loggerEvents.EnableEvents();
        var parameters = new Dictionary<string, string?>
        {
            { "verbosity", verbosityLevel },
            { DefaultLoggerParameterNames.TargetFramework , "net462"}
        };
        _consoleLogger.Initialize(loggerEvents, parameters);

        foreach (var testResult in GetTestResultsObject())
        {
            loggerEvents.RaiseTestResult(new TestResultEventArgs(testResult));
        }

        foreach (var testResult in GetPassedTestResultsObject())
        {
            loggerEvents.RaiseTestResult(new TestResultEventArgs(testResult));
        }

        loggerEvents.RaiseTestRunComplete(new TestRunCompleteEventArgs(new Mock<ITestRunStatistics>().Object, false, false, null, new Collection<AttachmentSet>(), new Collection<InvokedDataCollector>(), TimeSpan.FromSeconds(1)));
        loggerEvents.WaitForEventCompletion();

        _mockOutput.Verify(o => o.WriteLine(string.Format(CultureInfo.CurrentCulture, CommandLineResources.TestRunSummary, new object[] { CommandLineResources.PassedTestIndicator, 2, 1, 0, 1, "1 m 2 s", "TestSourcePassed", "(net462)" }), OutputLevel.Information), Times.Never);
        _mockOutput.Verify(o => o.WriteLine(string.Format(CultureInfo.CurrentCulture, CommandLineResources.TestRunSummary, new object[] { CommandLineResources.FailedTestIndicator, 5, 1, 1, 1, "1 h 6 m", "TestSource", "(net462)" }), OutputLevel.Information), Times.Never);
    }

    [TestMethod]
    public void TestResultHandlerShouldNotShowNotStdOutMsgOfPassedTestIfVerbosityIsNormal()
    {
        var loggerEvents = new InternalTestLoggerEvents(TestSessionMessageLogger.Instance);
        loggerEvents.EnableEvents();
        var parameters = new Dictionary<string, string?>
        {
            { "verbosity", "normal" }
        };
        _consoleLogger.Initialize(loggerEvents, parameters);

        var testcase = new TestCase("TestName", new Uri("some://uri"), "TestSource");
        string message = "Dummy message";
        TestResultMessage testResultMessage = new(TestResultMessage.StandardOutCategory, message);
        var testresult = new ObjectModel.TestResult(testcase)
        {
            Outcome = TestOutcome.Passed
        };
        testresult.Messages.Add(testResultMessage);

        loggerEvents.RaiseTestResult(new TestResultEventArgs(testresult));
        loggerEvents.WaitForEventCompletion();

        _mockOutput.Verify(o => o.WriteLine("", OutputLevel.Information), Times.Never());
        _mockOutput.Verify(o => o.WriteLine(" " + message, OutputLevel.Information), Times.Never());
    }

    [TestMethod]
    public void TestResultHandlerShouldNotShowDbgTrcMsg()
    {
        var loggerEvents = new InternalTestLoggerEvents(TestSessionMessageLogger.Instance);
        loggerEvents.EnableEvents();
        var parameters = new Dictionary<string, string?>
        {
            { "verbosity", "normal" }
        };
        _consoleLogger.Initialize(loggerEvents, parameters);

        var testcase = new TestCase("TestName", new Uri("some://uri"), "TestSource");
        string message = "Dummy message";
        TestResultMessage testResultMessage = new(TestResultMessage.DebugTraceCategory, message);
        var testresult = new ObjectModel.TestResult(testcase)
        {
            Outcome = TestOutcome.Passed
        };
        testresult.Messages.Add(testResultMessage);

        loggerEvents.RaiseTestResult(new TestResultEventArgs(testresult));
        loggerEvents.WaitForEventCompletion();

        _mockOutput.Verify(o => o.WriteLine(CommandLineResources.DbgTrcMessagesBanner, OutputLevel.Information), Times.Never());
        _mockOutput.Verify(o => o.WriteLine(" " + message, OutputLevel.Information), Times.Never());
    }

    [TestMethod]
    public void TestResultHandlerShouldWriteToConsoleButSkipPassedTestsForMinimalVerbosity()
    {
        var loggerEvents = new InternalTestLoggerEvents(TestSessionMessageLogger.Instance);
        loggerEvents.EnableEvents();
        var parameters = new Dictionary<string, string?>
        {
            { "verbosity", "minimal" },
            { DefaultLoggerParameterNames.TargetFramework , "net462"}
        };
        _consoleLogger.Initialize(loggerEvents, parameters);

        foreach (var testResult in GetTestResultsObject())
        {
            loggerEvents.RaiseTestResult(new TestResultEventArgs(testResult));
        }
        loggerEvents.WaitForEventCompletion();

        _mockOutput.Verify(o => o.Write(PassedTestIndicator, OutputLevel.Information), Times.Never);
        _mockOutput.Verify(o => o.WriteLine("TestName [1 h 2 m]", OutputLevel.Information), Times.Never);
        _mockOutput.Verify(o => o.Write(FailedTestIndicator, OutputLevel.Information), Times.Once);
        _mockOutput.Verify(o => o.WriteLine("TestName [4 m 5 s]", OutputLevel.Information), Times.Once());
        _mockOutput.Verify(o => o.Write(SkippedTestIndicator, OutputLevel.Information), Times.Exactly(3));
        _mockOutput.Verify(o => o.WriteLine("TestName", OutputLevel.Information), Times.Exactly(3));
    }

    [TestMethod]
    public void TestResultHandlerShouldWriteToNoTestResultForQuietVerbosity()
    {
        var loggerEvents = new InternalTestLoggerEvents(TestSessionMessageLogger.Instance);
        loggerEvents.EnableEvents();
        var parameters = new Dictionary<string, string?>
        {
            { "verbosity", "quiet" },
            { DefaultLoggerParameterNames.TargetFramework , "net462"}
        };
        _consoleLogger.Initialize(loggerEvents, parameters);

        foreach (var testResult in GetTestResultsObject())
        {
            loggerEvents.RaiseTestResult(new TestResultEventArgs(testResult));
        }
        loggerEvents.WaitForEventCompletion();

        _mockOutput.Verify(o => o.WriteLine(string.Format(CultureInfo.CurrentCulture, CommandLineResources.PassedTestIndicator, "TestName [1 h 2 m]"), OutputLevel.Information), Times.Never);
        _mockOutput.Verify(o => o.WriteLine(string.Format(CultureInfo.CurrentCulture, CommandLineResources.FailedTestIndicator, "TestName [4 m 5 s]"), OutputLevel.Information), Times.Never);
        _mockOutput.Verify(o => o.WriteLine(string.Format(CultureInfo.CurrentCulture, CommandLineResources.SkippedTestIndicator, "TestName"), OutputLevel.Warning), Times.Never);
        _mockOutput.Verify(o => o.WriteLine(string.Format(CultureInfo.CurrentCulture, CommandLineResources.NotRunTestIndicator, "TestName"), OutputLevel.Information), Times.Never);
    }

    [DataRow("[1 h 2 m]", new int[5] { 0, 1, 2, 3, 78 })]
    [DataRow("[4 m 3 s]", new int[5] { 0, 0, 4, 3, 78 })]
    [DataRow("[3 s]", new int[5] { 0, 0, 0, 3, 78 })]
    [DataRow("[78 ms]", new int[5] { 0, 0, 0, 0, 78 })]
    [DataRow("[1 h]", new int[5] { 0, 1, 0, 5, 78 })]
    [DataRow("[5 m]", new int[5] { 0, 0, 5, 0, 78 })]
    [DataRow("[4 s]", new int[5] { 0, 0, 0, 4, 0 })]
    [DataTestMethod]
    public void TestResultHandlerForTestResultWithDurationShouldPrintDurationInfo(string expectedDuration, int[] timeSpanArgs)
    {
        var loggerEvents = new InternalTestLoggerEvents(TestSessionMessageLogger.Instance);
        loggerEvents.EnableEvents();
        var parameters = new Dictionary<string, string?>
        {
            { "verbosity", "normal" }
        };
        _consoleLogger.Initialize(loggerEvents, parameters);
        var testResultWithHrMinSecMs = new ObjectModel.TestResult(new TestCase("DymmyNamespace.DummyClass.TestName", new Uri("some://uri"), "TestSource") { DisplayName = "TestName" })
        {
            Outcome = TestOutcome.Passed,
            Duration = new TimeSpan(timeSpanArgs[0], timeSpanArgs[1], timeSpanArgs[2], timeSpanArgs[3], timeSpanArgs[4])
        };

        loggerEvents.RaiseTestResult(new TestResultEventArgs(testResultWithHrMinSecMs));
        loggerEvents.WaitForEventCompletion();

        _mockOutput.Verify(o => o.Write(PassedTestIndicator, OutputLevel.Information), Times.Once());
        _mockOutput.Verify(o => o.WriteLine("TestName " + expectedDuration, OutputLevel.Information), Times.Once());
    }

    [DataTestMethod]
    public void TestResultHandlerForTestResultWithDurationLessThanOneMsShouldPrintDurationInfo()
    {
        var loggerEvents = new InternalTestLoggerEvents(TestSessionMessageLogger.Instance);
        loggerEvents.EnableEvents();
        var parameters = new Dictionary<string, string?>
        {
            { "verbosity", "normal" }
        };
        _consoleLogger.Initialize(loggerEvents, parameters);
        var testResultWithHrMinSecMs = new ObjectModel.TestResult(new TestCase("DymmyNamespace.DummyClass.TestName", new Uri("some://uri"), "TestSource") { DisplayName = "TestName" })
        {
            Outcome = TestOutcome.Passed,
            Duration = TimeSpan.FromTicks(50)
        };

        loggerEvents.RaiseTestResult(new TestResultEventArgs(testResultWithHrMinSecMs));
        loggerEvents.WaitForEventCompletion();

        _mockOutput.Verify(o => o.Write(PassedTestIndicator, OutputLevel.Information), Times.Once());
        _mockOutput.Verify(o => o.WriteLine("TestName [< 1 ms]", OutputLevel.Information), Times.Once());
    }

    [TestMethod]
    public void TestRunCompleteHandlerCorrectlySplitPathsForSourceName()
    {
        // Arrange
        var loggerEvents = new InternalTestLoggerEvents(TestSessionMessageLogger.Instance);
        loggerEvents.EnableEvents();
        var parameters = new Dictionary<string, string?>
        {
            { "verbosity", "minimal" }
        };
        _consoleLogger.Initialize(loggerEvents, parameters);

        // Linux-like path
        loggerEvents.RaiseTestResult(new(new(new("FQN1", new Uri("some://uri"), "/home/MyApp1/Tests/MyApp1.Tests/MyApp1.Tests.dll"))));
        // Double forward slashes path
        loggerEvents.RaiseTestResult(new(new(new("FQN2", new Uri("some://uri"), "/home//MyApp2//Tests//MyApp2.Tests//MyApp2.Tests.dll"))));
        // Backslashes path
        loggerEvents.RaiseTestResult(new(new(new("FQN3", new Uri("some://uri"), @"C:\MyApp3/Tests/MyApp3.Tests\MyApp3.Tests.dll"))));
        // Multiple Backslashes path
        loggerEvents.RaiseTestResult(new(new(new("FQN4", new Uri("some://uri"), "C:\\\\MyApp4\\\\Tests\\\\MyApp4.Tests\\\\MyApp4.Tests.dll"))));
        // Mix backslashes and forward slashes path
        loggerEvents.RaiseTestResult(new(new(new("FQN5", new Uri("some://uri"), "C:\\MyApp5/Tests\\\\MyApp5.Tests///MyApp5.Tests.dll"))));
        // UNC path
        loggerEvents.RaiseTestResult(new(new(new("FQN6", new Uri("some://uri"), @"\\MyApp6\Tests\MyApp6.Tests\MyApp6.Tests.dll"))));

        // Act
        loggerEvents.CompleteTestRun(null, false, false, null, null, null, new TimeSpan(1, 0, 0, 0));

        // Assert
        VerifyCall("MyApp1.Tests.dll");
        VerifyCall("MyApp2.Tests.dll");
        // On Linux and MAC we don't support backslash for path so source name will contain backslashes.
        VerifyCall(OSUtils.IsWindows ? "MyApp3.Tests.dll" : "MyApp3.Tests\\MyApp3.Tests.dll");
        // On Linux and MAC we don't support backslash for path so source name will contain backslashes.
        VerifyCall(OSUtils.IsWindows ? "MyApp4.Tests.dll" : @"C:\\MyApp4\\Tests\\MyApp4.Tests\\MyApp4.Tests.dll");
        VerifyCall("MyApp5.Tests.dll");
        VerifyCall(OSUtils.IsWindows ? "MyApp6.Tests.dll" : @"\\MyApp6\Tests\MyApp6.Tests\MyApp6.Tests.dll");

        // Local functions
        void VerifyCall(string testName)
            => _mockOutput.Verify(
                o => o.WriteLine(
                    string.Format(CultureInfo.CurrentCulture, CommandLineResources.TestRunSummaryAssemblyAndFramework, testName, ""),
                    OutputLevel.Information),
                Times.Once());
    }

    [TestMethod]
    public void TestRunCompleteHandlerShouldWriteToConsoleIfTestsPass()
    {
        var loggerEvents = new InternalTestLoggerEvents(TestSessionMessageLogger.Instance);
        loggerEvents.EnableEvents();
        var parameters = new Dictionary<string, string?>
        {
            { "verbosity", "normal" }
        };
        _consoleLogger.Initialize(loggerEvents, parameters);

        foreach (var testResult in GetTestResultObject(TestOutcome.Passed))
        {
            loggerEvents.RaiseTestResult(new TestResultEventArgs(testResult));
        }
        loggerEvents.CompleteTestRun(null, false, false, null, null, null, new TimeSpan(1, 0, 0, 0));

        _mockOutput.Verify(o => o.WriteLine(string.Format(CultureInfo.CurrentCulture, CommandLineResources.TestRunSummaryTotalTests, 1), OutputLevel.Information), Times.Once());
        _mockOutput.Verify(o => o.WriteLine(string.Format(CultureInfo.CurrentCulture, CommandLineResources.TestRunSummaryPassedTests, 1), OutputLevel.Information), Times.Once());
        _mockOutput.Verify(o => o.WriteLine(string.Format(CultureInfo.CurrentCulture, CommandLineResources.TestRunSummaryFailedTests, 0), OutputLevel.Information), Times.Never());
        _mockOutput.Verify(o => o.WriteLine(string.Format(CultureInfo.CurrentCulture, CommandLineResources.TestRunSummarySkippedTests, 0), OutputLevel.Information), Times.Never());
        _mockOutput.Verify(o => o.WriteLine(CommandLineResources.TestRunSuccessful, OutputLevel.Information), Times.Once());
        _mockProgressIndicator.Verify(pi => pi.Stop(), Times.Once);
    }

    [TestMethod]
    public void TestRunCompleteHandlerShouldWriteToConsoleIfTestsFail()
    {
        var loggerEvents = new InternalTestLoggerEvents(TestSessionMessageLogger.Instance);
        loggerEvents.EnableEvents();
        var parameters = new Dictionary<string, string?>
        {
            { "verbosity", "normal" }
        };
        _consoleLogger.Initialize(loggerEvents, parameters);

        foreach (var testResult in GetTestResultObject(TestOutcome.Failed))
        {
            loggerEvents.RaiseTestResult(new TestResultEventArgs(testResult));
        }
        loggerEvents.CompleteTestRun(null, false, false, null, null, null, new TimeSpan(1, 0, 0, 0));

        _mockOutput.Verify(o => o.WriteLine(string.Format(CultureInfo.CurrentCulture, CommandLineResources.TestRunSummaryTotalTests, 1), OutputLevel.Information), Times.Once());
        _mockOutput.Verify(o => o.WriteLine(string.Format(CultureInfo.CurrentCulture, CommandLineResources.TestRunSummaryFailedTests, 1), OutputLevel.Information), Times.Once());
        _mockOutput.Verify(o => o.WriteLine(string.Format(CultureInfo.CurrentCulture, CommandLineResources.TestRunSummaryPassedTests, 0), OutputLevel.Information), Times.Never());
        _mockOutput.Verify(o => o.WriteLine(string.Format(CultureInfo.CurrentCulture, CommandLineResources.TestRunSummarySkippedTests, 0), OutputLevel.Information), Times.Never());
        _mockOutput.Verify(o => o.WriteLine(CommandLineResources.TestRunFailed, OutputLevel.Error), Times.Once());
    }

    [TestMethod]
    public void TestRunCompleteHandlerShouldWriteToConsoleIfTestsCanceled()
    {
        var loggerEvents = new InternalTestLoggerEvents(TestSessionMessageLogger.Instance);
        loggerEvents.EnableEvents();
        var parameters = new Dictionary<string, string?>
        {
            { "verbosity", "normal" }
        };
        _consoleLogger.Initialize(loggerEvents, parameters);

        foreach (var testResult in GetTestResultObject(TestOutcome.Failed))
        {
            loggerEvents.RaiseTestResult(new TestResultEventArgs(testResult));
        }
        loggerEvents.CompleteTestRun(null, true, false, null, null, null, new TimeSpan(1, 0, 0, 0));

        _mockOutput.Verify(o => o.WriteLine(string.Format(CultureInfo.CurrentCulture, CommandLineResources.TestRunSummaryForCanceledOrAbortedRun, Array.Empty<string>()), OutputLevel.Information), Times.Once());
        _mockOutput.Verify(o => o.WriteLine(string.Format(CultureInfo.CurrentCulture, CommandLineResources.TestRunSummaryFailedTests, 1), OutputLevel.Information), Times.Once());
        _mockOutput.Verify(o => o.WriteLine(string.Format(CultureInfo.CurrentCulture, CommandLineResources.TestRunSummaryPassedTests, 0), OutputLevel.Information), Times.Never());
        _mockOutput.Verify(o => o.WriteLine(string.Format(CultureInfo.CurrentCulture, CommandLineResources.TestRunSummarySkippedTests, 0), OutputLevel.Information), Times.Never());
        _mockOutput.Verify(o => o.WriteLine(CommandLineResources.TestRunCanceled, OutputLevel.Error), Times.Once());
    }

    [TestMethod]
    public void TestRunCompleteHandlerShouldWriteToConsoleIfTestsCanceledWithoutRunningAnyTest()
    {
        var loggerEvents = new InternalTestLoggerEvents(TestSessionMessageLogger.Instance);
        loggerEvents.EnableEvents();
        var parameters = new Dictionary<string, string?>
        {
            { "verbosity", "normal" }
        };
        _consoleLogger.Initialize(loggerEvents, parameters);

        loggerEvents.CompleteTestRun(null, true, false, null, null, null, new TimeSpan(1, 0, 0, 0));

        _mockOutput.Verify(o => o.WriteLine(CommandLineResources.TestRunCanceled, OutputLevel.Error), Times.Once());
    }

    [TestMethod]
    public void TestRunCompleteHandlerShouldWriteToConsoleIfTestsAborted()
    {
        var loggerEvents = new InternalTestLoggerEvents(TestSessionMessageLogger.Instance);
        loggerEvents.EnableEvents();
        var parameters = new Dictionary<string, string?>
        {
            { "verbosity", "normal" }
        };

        _consoleLogger.Initialize(loggerEvents, parameters);

        foreach (var testResult in GetTestResultObject(TestOutcome.Failed))
        {
            loggerEvents.RaiseTestResult(new TestResultEventArgs(testResult));
        }
        loggerEvents.CompleteTestRun(null, false, true, null, null, null, new TimeSpan(1, 0, 0, 0));

        _mockOutput.Verify(o => o.WriteLine(string.Format(CultureInfo.CurrentCulture, CommandLineResources.TestRunSummaryForCanceledOrAbortedRun, Array.Empty<string>()), OutputLevel.Information), Times.Once());
        _mockOutput.Verify(o => o.WriteLine(CommandLineResources.TestRunAborted, OutputLevel.Error), Times.Once());
    }

    [TestMethod]
    public void TestRunCompleteHandlerShouldWriteToConsoleIfTestsAbortedWithoutRunningAnyTest()
    {
        var loggerEvents = new InternalTestLoggerEvents(TestSessionMessageLogger.Instance);
        loggerEvents.EnableEvents();
        var parameters = new Dictionary<string, string?>
        {
            { "verbosity", "normal" }
        };
        _consoleLogger.Initialize(loggerEvents, parameters);

        loggerEvents.CompleteTestRun(null, false, true, null, null, null, new TimeSpan(1, 0, 0, 0));

        _mockOutput.Verify(o => o.WriteLine(CommandLineResources.TestRunAborted, OutputLevel.Error), Times.Once());
    }

    [TestMethod]
    public void TestRunStartHandlerShouldWriteNumberOfTestSourcesDiscoveredOnConsole()
    {
        var loggerEvents = new InternalTestLoggerEvents(TestSessionMessageLogger.Instance);
        loggerEvents.EnableEvents();

        var fileHelper = new Mock<IFileHelper>();
        CommandLineOptions.Reset();
        CommandLineOptions.Instance.FileHelper = fileHelper.Object;
        CommandLineOptions.Instance.FilePatternParser = new FilePatternParser(new Mock<Matcher>().Object, fileHelper.Object);
        string testFilePath = Path.Combine(Path.GetTempPath(), "DmmyTestFile.dll");
        fileHelper.Setup(fh => fh.Exists(testFilePath)).Returns(true);

        CommandLineOptions.Instance.AddSource(testFilePath);

        var parameters = new Dictionary<string, string?>
        {
            { "verbosity", "normal" }
        };
        _consoleLogger.Initialize(loggerEvents, parameters);

        var testRunStartEventArgs = new TestRunStartEventArgs(new TestRunCriteria(new List<string> { testFilePath }, 1));
        loggerEvents.RaiseTestRunStart(testRunStartEventArgs);
        loggerEvents.WaitForEventCompletion();

        _mockOutput.Verify(o => o.WriteLine(string.Format(CultureInfo.CurrentCulture, CommandLineResources.TestSourcesDiscovered, CommandLineOptions.Instance.Sources.Count()), OutputLevel.Information), Times.Once());
    }

    [TestMethod]
    public void TestRunStartHandlerShouldWriteTestSourcesDiscoveredOnConsoleIfVerbosityDetailed()
    {
        var loggerEvents = new InternalTestLoggerEvents(TestSessionMessageLogger.Instance);
        loggerEvents.EnableEvents();

        var fileHelper = new Mock<IFileHelper>();
        CommandLineOptions.Reset();
        CommandLineOptions.Instance.FileHelper = fileHelper.Object;
        CommandLineOptions.Instance.FilePatternParser = new FilePatternParser(new Mock<Matcher>().Object, fileHelper.Object);
        var temp = Path.GetTempPath();
        string testFilePath = Path.Combine(temp, "DummyTestFile.dll");
        fileHelper.Setup(fh => fh.Exists(testFilePath)).Returns(true);
        string testFilePath2 = Path.Combine(temp, "DummyTestFile2.dll");
        fileHelper.Setup(fh => fh.Exists(testFilePath2)).Returns(true);

        CommandLineOptions.Instance.AddSource(testFilePath);
        CommandLineOptions.Instance.AddSource(testFilePath2);

        var parameters = new Dictionary<string, string?>
        {
            { "verbosity", "detailed" }
        };
        _consoleLogger.Initialize(loggerEvents, parameters);

        var testRunStartEventArgs = new TestRunStartEventArgs(new TestRunCriteria(new List<string> { testFilePath }, 1));
        loggerEvents.RaiseTestRunStart(testRunStartEventArgs);
        loggerEvents.WaitForEventCompletion();

        _mockOutput.Verify(o => o.WriteLine(string.Format(CultureInfo.CurrentCulture, CommandLineResources.TestSourcesDiscovered, CommandLineOptions.Instance.Sources.Count()), OutputLevel.Information), Times.Once());
        _mockOutput.Verify(o => o.WriteLine(testFilePath, OutputLevel.Information), Times.Once);
        _mockOutput.Verify(o => o.WriteLine(testFilePath, OutputLevel.Information), Times.Once);
    }

    [TestMethod]
    public void TestRunStartHandlerShouldNotWriteTestSourcesDiscoveredOnConsoleIfVerbosityNotDetailed()
    {
        var loggerEvents = new InternalTestLoggerEvents(TestSessionMessageLogger.Instance);
        loggerEvents.EnableEvents();

        var fileHelper = new Mock<IFileHelper>();
        CommandLineOptions.Reset();
        CommandLineOptions.Instance.FileHelper = fileHelper.Object;
        CommandLineOptions.Instance.FilePatternParser = new FilePatternParser(new Mock<Matcher>().Object, fileHelper.Object);
        var temp = Path.GetTempPath();
        string testFilePath = Path.Combine(temp, "DummyTestFile.dll");
        fileHelper.Setup(fh => fh.Exists(testFilePath)).Returns(true);
        string testFilePath2 = Path.Combine(temp, "DummyTestFile2.dll");
        fileHelper.Setup(fh => fh.Exists(testFilePath2)).Returns(true);

        CommandLineOptions.Instance.AddSource(testFilePath);
        CommandLineOptions.Instance.AddSource(testFilePath2);

        var parameters = new Dictionary<string, string?>
        {
            { "verbosity", "normal" }
        };
        _consoleLogger.Initialize(loggerEvents, parameters);

        var testRunStartEventArgs = new TestRunStartEventArgs(new TestRunCriteria(new List<string> { testFilePath }, 1));
        loggerEvents.RaiseTestRunStart(testRunStartEventArgs);
        loggerEvents.WaitForEventCompletion();

        _mockOutput.Verify(o => o.WriteLine(string.Format(CultureInfo.CurrentCulture, CommandLineResources.TestSourcesDiscovered, CommandLineOptions.Instance.Sources.Count()), OutputLevel.Information), Times.Once());
        _mockOutput.Verify(o => o.WriteLine(testFilePath, OutputLevel.Information), Times.Never);
        _mockOutput.Verify(o => o.WriteLine(testFilePath2, OutputLevel.Information), Times.Never);
    }

    [TestMethod]
    public void PrintTimeHandlerShouldPrintElapsedTimeOnConsole()
    {
        var loggerEvents = new InternalTestLoggerEvents(TestSessionMessageLogger.Instance);
        loggerEvents.EnableEvents();
        var parameters = new Dictionary<string, string?>
        {
            { "verbosity", "normal" }
        };
        _consoleLogger.Initialize(loggerEvents, parameters);

        foreach (var testResult in GetTestResultObject(TestOutcome.Passed))
        {
            loggerEvents.RaiseTestResult(new TestResultEventArgs(testResult));
        }
        loggerEvents.CompleteTestRun(null, false, false, null, null, null, new TimeSpan(1, 0, 0, 0));
        loggerEvents.CompleteTestRun(null, false, false, null, null, null, new TimeSpan(0, 1, 0, 0));
        loggerEvents.CompleteTestRun(null, false, false, null, null, null, new TimeSpan(0, 0, 1, 0));
        loggerEvents.CompleteTestRun(null, false, false, null, null, null, new TimeSpan(0, 0, 0, 1));

        // Verify PrintTimeSpan with different formats
        _mockOutput.Verify(o => o.WriteLine(string.Format(CultureInfo.CurrentCulture, CommandLineResources.ExecutionTimeFormatString, 1, CommandLineResources.Days), OutputLevel.Information), Times.Once());
        _mockOutput.Verify(o => o.WriteLine(string.Format(CultureInfo.CurrentCulture, CommandLineResources.ExecutionTimeFormatString, 1, CommandLineResources.Hours), OutputLevel.Information), Times.Once());
        _mockOutput.Verify(o => o.WriteLine(string.Format(CultureInfo.CurrentCulture, CommandLineResources.ExecutionTimeFormatString, 1, CommandLineResources.Minutes), OutputLevel.Information), Times.Once());
        _mockOutput.Verify(o => o.WriteLine(string.Format(CultureInfo.CurrentCulture, CommandLineResources.ExecutionTimeFormatString, 1, CommandLineResources.Seconds), OutputLevel.Information), Times.Once());
    }

    [TestMethod]
    public void DisplayFullInformationShouldWriteErrorMessageAndStackTraceToConsole()
    {
        var loggerEvents = new InternalTestLoggerEvents(TestSessionMessageLogger.Instance);
        loggerEvents.EnableEvents();
        var parameters = new Dictionary<string, string?>
        {
            { "verbosity", "normal" }
        };
        _consoleLogger.Initialize(loggerEvents, parameters);

        var testresults = GetTestResultObject(TestOutcome.Failed);
        testresults[0].ErrorMessage = "ErrorMessage";
        testresults[0].ErrorStackTrace = "ErrorStackTrace";
        foreach (var testResult in testresults)
        {
            loggerEvents.RaiseTestResult(new TestResultEventArgs(testResult));
        }
        loggerEvents.WaitForEventCompletion();

        _mockOutput.Verify(o => o.WriteLine(string.Format(CultureInfo.CurrentCulture, "{0}", "   ErrorMessage"), OutputLevel.Information), Times.Once());
        _mockOutput.Verify(o => o.WriteLine(string.Format(CultureInfo.CurrentCulture, "{0}", "  ErrorStackTrace"), OutputLevel.Information), Times.Once());
        _mockOutput.Verify(o => o.WriteLine("  Error Message:", OutputLevel.Information), Times.Once());
        _mockOutput.Verify(o => o.WriteLine("  Stack Trace:", OutputLevel.Information), Times.Once());
    }

    [TestMethod]
    public void DisplayFullInformationShouldWriteStdMessageWithNewLine()
    {
        var loggerEvents = new InternalTestLoggerEvents(TestSessionMessageLogger.Instance);
        loggerEvents.EnableEvents();
        var parameters = new Dictionary<string, string?>
        {
            { "verbosity", "detailed" }
        };
        _consoleLogger.Initialize(loggerEvents, parameters);

        var testresults = GetTestResultObject(TestOutcome.Passed);
        testresults[0].Messages.Add(new TestResultMessage(TestResultMessage.StandardOutCategory, "Hello"));

        foreach (var testResult in testresults)
        {
            loggerEvents.RaiseTestResult(new TestResultEventArgs(testResult));
        }
        loggerEvents.WaitForEventCompletion();

        _mockOutput.Verify(o => o.Write(PassedTestIndicator, OutputLevel.Information), Times.Once());
        _mockOutput.Verify(o => o.WriteLine("TestName", OutputLevel.Information), Times.Once());
        _mockOutput.Verify(o => o.WriteLine(" Hello", OutputLevel.Information), Times.Once());
        _mockOutput.Verify(o => o.WriteLine(string.Empty, OutputLevel.Information), Times.AtLeastOnce);
    }

    [TestMethod]
    public void GetTestMessagesShouldWriteMessageAndStackTraceToConsole()
    {
        var loggerEvents = new InternalTestLoggerEvents(TestSessionMessageLogger.Instance);
        loggerEvents.EnableEvents();
        var parameters = new Dictionary<string, string?>
        {
            { "verbosity", "normal" }
        };
        _consoleLogger.Initialize(loggerEvents, parameters);

        var testresults = GetTestResultObject(TestOutcome.Failed);
        testresults[0].Messages.Add(new TestResultMessage(TestResultMessage.StandardOutCategory, "StandardOutCategory"));
        testresults[0].Messages.Add(new TestResultMessage(TestResultMessage.StandardErrorCategory, "StandardErrorCategory"));
        testresults[0].Messages.Add(new TestResultMessage(TestResultMessage.AdditionalInfoCategory, "AdditionalInfoCategory"));
        testresults[0].Messages.Add(new TestResultMessage(TestResultMessage.AdditionalInfoCategory, "AnotherAdditionalInfoCategory"));

        foreach (var testResult in testresults)
        {
            loggerEvents.RaiseTestResult(new TestResultEventArgs(testResult));
        }
        loggerEvents.WaitForEventCompletion();

        _mockOutput.Verify(o => o.WriteLine("  Standard Output Messages:", OutputLevel.Information), Times.Once());
        _mockOutput.Verify(o => o.WriteLine(" StandardOutCategory", OutputLevel.Information), Times.Once());

        _mockOutput.Verify(o => o.WriteLine("  Standard Error Messages:", OutputLevel.Information), Times.Once());
        _mockOutput.Verify(o => o.WriteLine(" StandardErrorCategory", OutputLevel.Information), Times.Once());

        _mockOutput.Verify(o => o.WriteLine("  Additional Information Messages:", OutputLevel.Information), Times.Once());
        _mockOutput.Verify(o => o.WriteLine(" AdditionalInfoCategory AnotherAdditionalInfoCategory", OutputLevel.Information), Times.Once());
    }

    [DataRow("quiet")]
    [DataRow("Normal")]
    [DataRow("minimal")]
    [DataRow("detailed")]
    [TestMethod]
    public void AttachmentInformationShouldBeWrittenToConsoleIfAttachmentsArePresent(string verbosityLevel)
    {
        var loggerEvents = new InternalTestLoggerEvents(TestSessionMessageLogger.Instance);
        loggerEvents.EnableEvents();
        var parameters = new Dictionary<string, string?>
        {
            { "verbosity", verbosityLevel }
        };
        _consoleLogger.Initialize(loggerEvents, parameters);

        var attachmentSet = new AttachmentSet(new Uri("test://uri"), "myattachmentset");
        var uriDataAttachment = new UriDataAttachment(new Uri("file://server/filename.ext"), "description");
        attachmentSet.Attachments.Add(uriDataAttachment);
        var uriDataAttachment1 = new UriDataAttachment(new Uri("file://server/filename1.ext"), "description");
        attachmentSet.Attachments.Add(uriDataAttachment1);
        var attachmentSetList = new List<AttachmentSet>
        {
            attachmentSet
        };
        loggerEvents.CompleteTestRun(null, false, false, null, new Collection<AttachmentSet>(attachmentSetList), new Collection<InvokedDataCollector>(), new TimeSpan(1, 0, 0, 0));

        _mockOutput.Verify(o => o.WriteLine(string.Format(CultureInfo.CurrentCulture, CommandLineResources.AttachmentOutputFormat, uriDataAttachment.Uri.LocalPath), OutputLevel.Information), Times.Once());
        _mockOutput.Verify(o => o.WriteLine(string.Format(CultureInfo.CurrentCulture, CommandLineResources.AttachmentOutputFormat, uriDataAttachment1.Uri.LocalPath), OutputLevel.Information), Times.Once());
    }

    [TestMethod]
    public void ResultsInHeirarchichalOrderShouldReportCorrectCount()
    {
        var loggerEvents = new InternalTestLoggerEvents(TestSessionMessageLogger.Instance);
        loggerEvents.EnableEvents();
        var parameters = new Dictionary<string, string?>
        {
            { "verbosity", "normal" }
        };
        _consoleLogger.Initialize(loggerEvents, parameters);

        TestCase testCase1 = CreateTestCase("TestCase1");
        TestCase testCase2 = CreateTestCase("TestCase2");
        TestCase testCase3 = CreateTestCase("TestCase3");

        Guid parentExecutionId = Guid.NewGuid();
        TestProperty parentExecIdProperty = TestProperty.Register("ParentExecId", "ParentExecId", typeof(Guid), TestPropertyAttributes.Hidden, typeof(ObjectModel.TestResult));
        TestProperty executionIdProperty = TestProperty.Register("ExecutionId", "ExecutionId", typeof(Guid), TestPropertyAttributes.Hidden, typeof(ObjectModel.TestResult));
        TestProperty testTypeProperty = TestProperty.Register("TestType", "TestType", typeof(Guid), TestPropertyAttributes.Hidden, typeof(ObjectModel.TestResult));

        var result1 = new ObjectModel.TestResult(testCase1) { Outcome = TestOutcome.Failed };
        result1.SetPropertyValue(executionIdProperty, parentExecutionId);

        var result2 = new ObjectModel.TestResult(testCase2) { Outcome = TestOutcome.Passed };
        result2.SetPropertyValue(executionIdProperty, Guid.NewGuid());
        result2.SetPropertyValue(parentExecIdProperty, parentExecutionId);

        var result3 = new ObjectModel.TestResult(testCase3) { Outcome = TestOutcome.Failed };
        result3.SetPropertyValue(executionIdProperty, Guid.NewGuid());
        result3.SetPropertyValue(parentExecIdProperty, parentExecutionId);

        loggerEvents.RaiseTestResult(new TestResultEventArgs(result1));
        loggerEvents.RaiseTestResult(new TestResultEventArgs(result2));
        loggerEvents.RaiseTestResult(new TestResultEventArgs(result3));

        loggerEvents.CompleteTestRun(null, false, false, null, null, null, new TimeSpan(1, 0, 0, 0));

        _mockOutput.Verify(o => o.WriteLine(string.Format(CultureInfo.CurrentCulture, CommandLineResources.TestRunSummaryFailedTests, 1), OutputLevel.Information), Times.Once());
        _mockOutput.Verify(o => o.WriteLine(string.Format(CultureInfo.CurrentCulture, CommandLineResources.TestRunSummaryPassedTests, 1), OutputLevel.Information), Times.Once());
        _mockOutput.Verify(o => o.WriteLine(string.Format(CultureInfo.CurrentCulture, CommandLineResources.TestRunSummaryTotalTests, 2), OutputLevel.Information), Times.Once());
    }

    private static TestCase CreateTestCase(string testCaseName)
    {
        return new TestCase(testCaseName, new Uri("some://uri"), "DummySourceFileName");
    }

    private static List<ObjectModel.TestResult> GetTestResultsObject()
    {
        var testcase = new TestCase("DymmyNamespace.DummyClass.TestName", new Uri("some://uri"), "TestSource")
        {
            DisplayName = "TestName"
        };

        var duration = new TimeSpan(1, 2, 3);
        var testresult = new ObjectModel.TestResult(testcase)
        {
            Outcome = TestOutcome.Passed,
            Duration = duration,
            StartTime = DateTime.Now - duration,
            EndTime = DateTime.Now
        };

        var duration1 = new TimeSpan(0, 0, 4, 5, 60);
        var testresult1 = new ObjectModel.TestResult(testcase)
        {
            Outcome = TestOutcome.Failed,
            Duration = duration1,
            StartTime = DateTime.Now - duration1,
            EndTime = DateTime.Now
        };

        var testresult2 = new ObjectModel.TestResult(testcase)
        {
            Outcome = TestOutcome.None
        };

        var testresult3 = new ObjectModel.TestResult(testcase)
        {
            Outcome = TestOutcome.NotFound
        };

        var testresult4 = new ObjectModel.TestResult(testcase)
        {
            Outcome = TestOutcome.Skipped
        };

        var testresultList = new List<ObjectModel.TestResult> { testresult, testresult1, testresult2, testresult3, testresult4 };

        return testresultList;
    }

    private static List<ObjectModel.TestResult> GetPassedTestResultsObject()
    {
        var testcase = new TestCase("DymmyNamespace.DummyClass.TestName", new Uri("some://uri"), "TestSourcePassed")
        {
            DisplayName = "TestName"
        };

        var duration = new TimeSpan(0, 0, 1, 2, 3);
        var testresult = new ObjectModel.TestResult(testcase)
        {
            Outcome = TestOutcome.Passed,
            Duration = duration,
            StartTime = DateTime.Now - duration,
            EndTime = DateTime.Now
        };

        var testresult1 = new ObjectModel.TestResult(testcase)
        {
            Outcome = TestOutcome.Skipped
        };

        var testresultList = new List<ObjectModel.TestResult> { testresult, testresult1 };

        return testresultList;
    }


    private static List<ObjectModel.TestResult> GetTestResultObject(TestOutcome outcome)
    {
        var testcase = new TestCase("TestName", new Uri("some://uri"), "TestSource");
        var testresult = new ObjectModel.TestResult(testcase)
        {
            Outcome = outcome
        };
        var testresultList = new List<ObjectModel.TestResult> { testresult };
        return testresultList;
    }
}
