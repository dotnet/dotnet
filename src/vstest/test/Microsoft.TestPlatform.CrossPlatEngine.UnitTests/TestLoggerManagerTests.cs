// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestPlatform.Common.Exceptions;
using Microsoft.VisualStudio.TestPlatform.Common.Logging;
using Microsoft.VisualStudio.TestPlatform.Common.Telemetry;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using ObjectModel = Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace TestPlatform.CrossPlatEngine.UnitTests;

/// <summary>
/// Tests the behaviors of the TestLoggerManager class.
/// </summary>
[TestClass]
public class TestLoggerManagerTests
{
    private static int s_counter;
    private static readonly EventWaitHandle WaitHandle = new AutoResetEvent(false);
    private readonly string _loggerUri = "testlogger://logger";

    [TestInitialize]
    public void Initialize()
    {
        TestPluginCacheHelper.SetupMockExtensions(
            new string[] { typeof(TestLoggerManagerTests).GetTypeInfo().Assembly.Location },
            () => { });
    }

    [TestCleanup]
    public void TestCleanup()
    {
        TestPluginCacheHelper.ResetExtensionsCache();
    }

    [TestMethod]
    public void TryGetUriFromFriendlyNameShouldReturnUriIfLoggerIsAdded()
    {
        var testLoggerManager = new DummyTestLoggerManager();
        testLoggerManager.InitializeLoggerByUri(new Uri(_loggerUri), new());
        testLoggerManager.TryGetUriFromFriendlyName("TestLoggerExtension", out var uri);
        Assert.AreEqual(uri?.ToString(), new Uri(_loggerUri).ToString());
    }

    [TestMethod]
    public void TryGetUriFromFriendlyNameShouldNotReturnUriIfLoggerIsNotAdded()
    {
        var testLoggerManager = new DummyTestLoggerManager();
        testLoggerManager.TryGetUriFromFriendlyName("TestLoggerExtension1", out var uri);
        Assert.IsNull(uri);
    }

    [TestMethod]
    public void GetResultsDirectoryShouldReturnNullIfRunSettingsIsNull()
    {
        var result = TestLoggerManager.GetResultsDirectory(null);
        Assert.IsNull(result);
    }

    [TestMethod]
    public void GetResultsDirectoryShouldReadResultsDirectoryFromSettingsIfSpecified()
    {
        string runSettingsXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
    <RunSettings>
      <RunConfiguration>
        <MaxCpuCount>0</MaxCpuCount>
        <ResultsDirectory>DummyTestResultsFolder</ResultsDirectory>
        <TargetPlatform> x64 </TargetPlatform>
        <TargetFrameworkVersion> Framework45 </TargetFrameworkVersion>
      </RunConfiguration>
    </RunSettings> ";

        var result = TestLoggerManager.GetResultsDirectory(runSettingsXml);
        Assert.AreEqual(0, string.Compare("DummyTestResultsFolder", result));
    }

    [TestMethod]
    public void GetResultsDirectoryShouldReturnDefaultPathIfResultsDirectoryIsNotProvidedInRunSettings()
    {
        string runSettingsXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
    <RunSettings>
      <RunConfiguration>
        <MaxCpuCount>0</MaxCpuCount>
        <TargetPlatform> x64 </TargetPlatform>
        <TargetFrameworkVersion> Framework45 </TargetFrameworkVersion>
      </RunConfiguration>
    </RunSettings> ";

        var result = TestLoggerManager.GetResultsDirectory(runSettingsXml);

        Assert.AreEqual(0, string.Compare(Constants.DefaultResultsDirectory, result));
    }

    [TestMethod]
    public void GetTargetFrameworkShouldReturnFrameworkProvidedInRunSettings()
    {
        string runSettingsXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
                                        <RunSettings>
                                          <RunConfiguration>
                                            <MaxCpuCount>0</MaxCpuCount>
                                            <TargetPlatform> x64 </TargetPlatform>
                                            <TargetFrameworkVersion> Framework45 </TargetFrameworkVersion>
                                          </RunConfiguration>
                                        </RunSettings> ";

        var framework = TestLoggerManager.GetTargetFramework(runSettingsXml);

        Assert.AreEqual(".NETFramework,Version=v4.5", framework?.Name);
    }

    [TestMethod]
    public void HandleTestRunMessageShouldInvokeTestRunMessageHandlerOfLoggers()
    {
        s_counter = 0;
        WaitHandle.Reset();
        var testLoggerManager = new DummyTestLoggerManager();
        testLoggerManager.InitializeLoggerByUri(new Uri(_loggerUri), new());
        testLoggerManager.EnableLogging();

        testLoggerManager.HandleTestRunMessage(new TestRunMessageEventArgs(TestMessageLevel.Informational, "TestRunMessage"));

        WaitHandle.WaitOne();
        Assert.AreEqual(1, s_counter);
    }

    [TestMethod]
    public void HandleTestRunMessageShouldNotInvokeTestRunMessageHandlerOfLoggersIfDisposed()
    {
        s_counter = 0;
        WaitHandle.Reset();
        var testLoggerManager = new DummyTestLoggerManager();
        testLoggerManager.InitializeLoggerByUri(new Uri(_loggerUri), new());
        testLoggerManager.EnableLogging();

        testLoggerManager.Dispose();
        testLoggerManager.HandleTestRunMessage(new TestRunMessageEventArgs(TestMessageLevel.Informational, "TestRunMessage"));

        Assert.AreEqual(0, s_counter);
    }

    [TestMethod]
    public void HandleTestRunCompleteShouldInvokeTestRunCompleteHandlerOfLoggers()
    {
        s_counter = 0;
        WaitHandle.Reset();
        // setup TestLogger
        var testLoggerManager = new DummyTestLoggerManager();
        testLoggerManager.InitializeLoggerByUri(new Uri(_loggerUri), new());
        testLoggerManager.EnableLogging();

        testLoggerManager.HandleTestRunComplete(new TestRunCompleteEventArgs(null, false, false, null, null, null, new TimeSpan()));

        WaitHandle.WaitOne();
        Assert.AreEqual(1, s_counter);
    }

    [TestMethod]
    public void HandleTestRunCompleteShouldNotInvokeTestRunCompleteHandlerOfLoggersIfDisposed()
    {
        s_counter = 0;
        WaitHandle.Reset();
        // setup TestLogger
        var testLoggerManager = new DummyTestLoggerManager();
        testLoggerManager.InitializeLoggerByUri(new Uri(_loggerUri), new());
        testLoggerManager.EnableLogging();

        testLoggerManager.Dispose();
        testLoggerManager.HandleTestRunComplete(new TestRunCompleteEventArgs(null, false, false, null, null, null, new TimeSpan()));

        Assert.AreEqual(0, s_counter);
    }

    [TestMethod]
    public void HandleTestRunCompleteShouldDisposeLoggerManager()
    {
        s_counter = 0;
        WaitHandle.Reset();
        // setup TestLogger
        var testLoggerManager = new DummyTestLoggerManager();
        testLoggerManager.InitializeLoggerByUri(new Uri(_loggerUri), new());
        testLoggerManager.EnableLogging();

        testLoggerManager.HandleTestRunComplete(new TestRunCompleteEventArgs(null, false, false, null, null, null, new TimeSpan()));
        testLoggerManager.HandleTestRunComplete(new TestRunCompleteEventArgs(null, false, false, null, null, null, new TimeSpan())); // count should not increase because of second call.

        Assert.AreEqual(1, s_counter);
    }

    [TestMethod]
    public void HandleTestRunStatsChangeShouldInvokeTestRunChangedHandlerOfLoggers()
    {
        s_counter = 0;
        WaitHandle.Reset();
        // setup TestLogger
        var testLoggerManager = new DummyTestLoggerManager();
        testLoggerManager.InitializeLoggerByUri(new Uri(_loggerUri), new());
        testLoggerManager.EnableLogging();

        testLoggerManager.HandleTestRunStatsChange(
            new TestRunChangedEventArgs(
                null,
                new List<ObjectModel.TestResult>()
                {
                    new ObjectModel.TestResult(
                        new TestCase(
                            "This is a string.",
                            new Uri("some://uri"),
                            "This is a string."))
                },
                null));

        WaitHandle.WaitOne();
        Assert.AreEqual(1, s_counter);
    }

    [TestMethod]
    public void HandleTestRunStatsChangeShouldNotInvokeTestRunChangedHandlerOfLoggersIfDisposed()
    {
        s_counter = 0;
        WaitHandle.Reset();
        // setup TestLogger
        var testLoggerManager = new DummyTestLoggerManager();
        testLoggerManager.InitializeLoggerByUri(new Uri(_loggerUri), new());
        testLoggerManager.EnableLogging();

        testLoggerManager.Dispose();
        testLoggerManager.HandleTestRunStatsChange(
            new TestRunChangedEventArgs(
                null,
                new List<ObjectModel.TestResult>()
                {
                    new ObjectModel.TestResult(
                        new TestCase(
                            "This is a string.",
                            new Uri("some://uri"),
                            "This is a string."))
                },
                null));

        Assert.AreEqual(0, s_counter);
    }

    [TestMethod]
    public void AddLoggerShouldNotThrowExceptionIfUriIsNull()
    {
        var testLoggerManager = new DummyTestLoggerManager();
        Assert.ThrowsException<ArgumentNullException>(
            () => testLoggerManager.InitializeLoggerByUri(null!, null));
    }

    [TestMethod]
    public void AddLoggerShouldNotThrowExceptionIfUriIsNonExistent()
    {
        var testLoggerManager = new DummyTestLoggerManager();
        Assert.IsFalse(testLoggerManager.InitializeLoggerByUri(new Uri("logger://NotALogger"), null));
    }

    [TestMethod]
    public void AddLoggerShouldAddDefaultLoggerParameterForTestLoggerWithParameters()
    {
        ValidLoggerWithParameters.Reset();
        var mockRequestData = new Mock<IRequestData>();
        var mockMetricsCollection = new Mock<IMetricsCollection>();
        mockRequestData.Setup(rd => rd.MetricsCollection).Returns(mockMetricsCollection.Object);

        var codeBase = typeof(TestLoggerManagerTests).GetTypeInfo().Assembly.Location;

        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <RunConfiguration>
                    <MaxCpuCount>0</MaxCpuCount>
                    <TargetPlatform> x64 </TargetPlatform>
                    <TargetFrameworkVersion> Framework45 </TargetFrameworkVersion>
                  </RunConfiguration>
                  <LoggerRunSettings>
                    <Loggers>
                      <Logger friendlyName=""TestLoggerExtension"" />
                      <Logger uri=""test-logger-with-parameter://logger"" enabled=""true""></Logger>
                    </Loggers>
                  </LoggerRunSettings>
                </RunSettings>";

        var testLoggerManager = new DummyTestLoggerManager(mockRequestData.Object);
        testLoggerManager.Initialize(settingsXml);

        Assert.IsNotNull(ValidLoggerWithParameters.Parameters, "parameters not getting passed");
        Assert.IsTrue(
            ValidLoggerWithParameters.Parameters.ContainsKey(DefaultLoggerParameterNames.TestRunDirectory),
            $"{DefaultLoggerParameterNames.TestRunDirectory} not added to parameters");
        Assert.IsFalse(
            string.IsNullOrWhiteSpace(
                ValidLoggerWithParameters.Parameters[DefaultLoggerParameterNames.TestRunDirectory]),
            $"parameter {DefaultLoggerParameterNames.TestRunDirectory} should not be null, empty or whitespace");
    }

    [TestMethod]
    public void DisposeShouldNotThrowExceptionIfCalledMultipleTimes()
    {
        // Dispose the logger manager multiple times and verify that no exception is thrown.
        var testLoggerManager = new DummyTestLoggerManager();
        testLoggerManager.Dispose();
        testLoggerManager.Dispose();
    }

    [TestMethod]
    public void AddLoggerShouldThrowObjectDisposedExceptionAfterDisposedIsCalled()
    {
        var testLoggerManager = new DummyTestLoggerManager();
        testLoggerManager.Dispose();

        Assert.ThrowsException<ObjectDisposedException>(
            () => testLoggerManager.InitializeLoggerByUri(new Uri("some://uri"), null));
    }


    [TestMethod]
    public void EnableLoggingShouldThrowObjectDisposedExceptionAfterDisposedIsCalled()
    {
        var testLoggerManager = new DummyTestLoggerManager();
        testLoggerManager.Dispose();
        Assert.ThrowsException<ObjectDisposedException>(
            () => testLoggerManager.EnableLogging());
    }

    [TestMethod]
    public void LoggerInitialzeShouldCollectLoggersForTelemetry()
    {
        var mockRequestData = new Mock<IRequestData>();
        var mockMetricsCollection = new Mock<IMetricsCollection>();
        mockRequestData.Setup(rd => rd.MetricsCollection).Returns(mockMetricsCollection.Object);

        var testLoggerManager = new DummyTestLoggerManager(mockRequestData.Object);
        testLoggerManager.InitializeLoggerByUri(new Uri(_loggerUri), new());

        // Act.
        testLoggerManager.Initialize(null);

        // Verify
        mockMetricsCollection.Verify(
            rd => rd.Add(TelemetryDataConstants.LoggerUsed, "TestPlatform.CrossPlatEngine.UnitTests.TestLoggerManagerTests+ValidLogger"));
    }

    /// <summary>
    /// DiscoveryStart event handler of loggers should be called only if discovery events are registered.
    /// </summary>
    [TestMethod]
    public void HandleDiscoveryStartShouldInvokeDiscoveryStartHandlerOfLoggers()
    {
        s_counter = 0;
        WaitHandle.Reset();

        DiscoveryCriteria discoveryCriteria = new() { TestCaseFilter = "Name=Test1" };
        DiscoveryStartEventArgs discoveryStartEventArgs = new(discoveryCriteria);

        // setup TestLogger
        var testLoggerManager = new DummyTestLoggerManager();
        testLoggerManager.InitializeLoggerByUri(new Uri(_loggerUri), new());
        testLoggerManager.EnableLogging();

        testLoggerManager.HandleDiscoveryStart(discoveryStartEventArgs);

        // Assertions when discovery events registered
        WaitHandle.WaitOne();
        Assert.AreEqual(1, s_counter);
    }

    /// <summary>
    /// DiscoveryStart event handler of loggers should be called only if discovery events are registered.
    /// </summary>
    [TestMethod]
    public void HandleDiscoveryStartShouldNotInvokeDiscoveryStartHandlerOfLoggersIfDisposed()
    {
        s_counter = 0;
        WaitHandle.Reset();

        DiscoveryCriteria discoveryCriteria = new() { TestCaseFilter = "Name=Test1" };
        DiscoveryStartEventArgs discoveryStartEventArgs = new(discoveryCriteria);

        // setup TestLogger
        var testLoggerManager = new DummyTestLoggerManager();
        testLoggerManager.InitializeLoggerByUri(new Uri(_loggerUri), new());
        testLoggerManager.EnableLogging();

        testLoggerManager.Dispose();
        testLoggerManager.HandleDiscoveryStart(discoveryStartEventArgs);

        // Assertions when discovery events registered
        Assert.AreEqual(0, s_counter);
    }

    /// <summary>
    /// DiscoveredTests event handler of loggers should be called only if discovery events are registered.
    /// </summary>
    [TestMethod]
    public void HandleDiscoveredTestsShouldInvokeDiscoveredTestsHandlerOfLoggers()
    {
        s_counter = 0;
        WaitHandle.Reset();

        List<TestCase> testCases = new() { new TestCase("This is a string.", new Uri("some://uri"), "DummySourceFileName") };
        DiscoveredTestsEventArgs discoveredTestsEventArgs = new(testCases);

        // setup TestLogger
        var testLoggerManager = new DummyTestLoggerManager();
        testLoggerManager.InitializeLoggerByUri(new Uri(_loggerUri), new());
        testLoggerManager.EnableLogging();

        testLoggerManager.HandleDiscoveredTests(discoveredTestsEventArgs);

        // Assertions when discovery events registered
        WaitHandle.WaitOne();
        Assert.AreEqual(1, s_counter);
    }

    [TestMethod]
    public void HandleDiscoveredTestsShouldNotInvokeDiscoveredTestsHandlerOfLoggersIfDisposed()
    {
        s_counter = 0;
        WaitHandle.Reset();

        List<TestCase> testCases = new() { new TestCase("This is a string.", new Uri("some://uri"), "DummySourceFileName") };
        DiscoveredTestsEventArgs discoveredTestsEventArgs = new(testCases);

        // setup TestLogger
        var testLoggerManager = new DummyTestLoggerManager();
        testLoggerManager.InitializeLoggerByUri(new Uri(_loggerUri), new());
        testLoggerManager.EnableLogging();

        testLoggerManager.Dispose();
        testLoggerManager.HandleDiscoveredTests(discoveredTestsEventArgs);

        // Assertions when discovery events registered
        Assert.AreEqual(0, s_counter);
    }

    /// <summary>
    /// TestRunStart event handler of loggers should be called only if test run events are registered.
    /// </summary>
    [TestMethod]
    public void HandleTestRunStartShouldInvokeTestRunStartHandlerOfLoggers()
    {
        s_counter = 0;
        WaitHandle.Reset();

        TestRunCriteria testRunCriteria = new(new List<string> { @"x:dummy\foo.dll" }, 10, false, string.Empty, TimeSpan.MaxValue, null, "Name=Test1", null);
        TestRunStartEventArgs testRunStartEventArgs = new(testRunCriteria);

        // setup TestLogger
        var testLoggerManager = new DummyTestLoggerManager();
        testLoggerManager.InitializeLoggerByUri(new Uri(_loggerUri), new());
        testLoggerManager.EnableLogging();

        testLoggerManager.HandleTestRunStart(testRunStartEventArgs);

        // Assertions when test run events registered
        WaitHandle.WaitOne();
        Assert.AreEqual(1, s_counter);
    }

    /// <summary>
    /// TestRunStart event handler of loggers should be called only if test run events are registered.
    /// </summary>
    [TestMethod]
    public void HandleTestRunStartShouldNotInvokeTestRunStartHandlerOfLoggersIfDisposed()
    {
        s_counter = 0;
        WaitHandle.Reset();

        TestRunCriteria testRunCriteria = new(new List<string> { @"x:dummy\foo.dll" }, 10, false, string.Empty, TimeSpan.MaxValue, null, "Name=Test1", null);
        TestRunStartEventArgs testRunStartEventArgs = new(testRunCriteria);

        // setup TestLogger
        var testLoggerManager = new DummyTestLoggerManager();
        testLoggerManager.InitializeLoggerByUri(new Uri(_loggerUri), new());
        testLoggerManager.EnableLogging();

        testLoggerManager.Dispose();
        testLoggerManager.HandleTestRunStart(testRunStartEventArgs);

        // Assertions when test run events registered
        Assert.AreEqual(0, s_counter);
    }

    /// <summary>
    /// DiscoveryComplete event handler of loggers should be called only if discovery events are registered.
    /// </summary>
    [TestMethod]
    public void HandleDiscoveryCompleteShouldInvokeDiscoveryCompleteHandlerOfLoggers()
    {
        s_counter = 0;
        WaitHandle.Reset();

        DiscoveryCompleteEventArgs discoveryCompleteEventArgs = new(2, false);

        // setup TestLogger
        var testLoggerManager = new DummyTestLoggerManager();
        testLoggerManager.InitializeLoggerByUri(new Uri(_loggerUri), new());
        testLoggerManager.EnableLogging();

        testLoggerManager.HandleDiscoveryComplete(discoveryCompleteEventArgs);

        // Assertions when discovery events registered
        WaitHandle.WaitOne();
        Assert.AreEqual(1, s_counter);
    }

    /// <summary>
    /// DiscoveryComplete event handler of loggers should be called only if discovery events are registered.
    /// </summary>
    [TestMethod]
    public void HandleDiscoveryCompleteShouldNotInvokeDiscoveryCompleteHandlerOfLoggersIfDisposed()
    {
        s_counter = 0;
        WaitHandle.Reset();

        var discoveryCompleteEventArgs = new DiscoveryCompleteEventArgs(2, false);

        // setup TestLogger
        var testLoggerManager = new DummyTestLoggerManager();
        testLoggerManager.InitializeLoggerByUri(new Uri(_loggerUri), new());
        testLoggerManager.EnableLogging();

        testLoggerManager.Dispose();
        testLoggerManager.HandleDiscoveryComplete(discoveryCompleteEventArgs);

        // Assertions when discovery events registered
        Assert.AreEqual(0, s_counter);
    }

    [TestMethod]
    public void HandleDiscoveryCompleteShouldDisposeLoggerManager()
    {
        s_counter = 0;
        WaitHandle.Reset();
        // setup TestLogger
        var testLoggerManager = new DummyTestLoggerManager();
        testLoggerManager.InitializeLoggerByUri(new Uri(_loggerUri), new());
        testLoggerManager.EnableLogging();

        var discoveryCompleteEventArgs = new DiscoveryCompleteEventArgs(2, false);
        testLoggerManager.HandleDiscoveryComplete(discoveryCompleteEventArgs);
        testLoggerManager.HandleDiscoveryComplete(discoveryCompleteEventArgs); // count should not increase because of second call.

        Assert.AreEqual(1, s_counter);
    }

    /// <summary>
    /// DiscoveryMessage event handler of loggers should be called only if discovery events are registered.
    /// </summary>
    [TestMethod]
    public void HandleDiscoveryMessageShouldInvokeDiscoveryMessageHandlerOfLoggers()
    {
        s_counter = 0;
        WaitHandle.Reset();

        string message = "This is the test message";
        TestRunMessageEventArgs testRunMessageEventArgs = new(TestMessageLevel.Informational, message);

        // setup TestLogger
        var testLoggerManager = new DummyTestLoggerManager();
        testLoggerManager.InitializeLoggerByUri(new Uri(_loggerUri), new());
        testLoggerManager.EnableLogging();

        testLoggerManager.HandleDiscoveryMessage(testRunMessageEventArgs);

        // Assertions when discovery events registered
        WaitHandle.WaitOne();
        Assert.AreEqual(1, s_counter);
    }

    /// <summary>
    /// DiscoveryMessage event handler of loggers should be called only if discovery events are registered.
    /// </summary>
    [TestMethod]
    public void HandleDiscoveryMessageShouldNotInvokeDiscoveryMessageHandlerOfLoggersIfDisposed()
    {
        s_counter = 0;
        WaitHandle.Reset();

        string message = "This is the test message";
        TestRunMessageEventArgs testRunMessageEventArgs = new(TestMessageLevel.Informational, message);

        // setup TestLogger
        var testLoggerManager = new DummyTestLoggerManager();
        testLoggerManager.InitializeLoggerByUri(new Uri(_loggerUri), new());
        testLoggerManager.EnableLogging();

        testLoggerManager.Dispose();
        testLoggerManager.HandleDiscoveryMessage(testRunMessageEventArgs);

        Assert.AreEqual(0, s_counter);
    }

    [TestMethod]
    public void InitializeShouldInitializeLoggerFromFriendlyNameWhenOnlyFriendlyNamePresent()
    {
        ValidLoggerWithParameters.Reset();
        var mockRequestData = new Mock<IRequestData>();
        var mockMetricsCollection = new Mock<IMetricsCollection>();
        mockRequestData.Setup(rd => rd.MetricsCollection).Returns(mockMetricsCollection.Object);

        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <LoggerRunSettings>
                    <Loggers>
                      <Logger friendlyName=""TestLoggerWithParameterExtension""></Logger>
                    </Loggers>
                  </LoggerRunSettings>
                </RunSettings>";

        var testLoggerManager = new DummyTestLoggerManager(mockRequestData.Object);
        testLoggerManager.Initialize(settingsXml);

        Assert.AreEqual(1, ValidLoggerWithParameters.Counter);
        mockMetricsCollection.Verify(
            rd => rd.Add(TelemetryDataConstants.LoggerUsed, "TestPlatform.CrossPlatEngine.UnitTests.TestLoggerManagerTests+ValidLoggerWithParameters"));
    }

    [TestMethod]
    public void InitializeShouldInitializeLoggerFromUriWhenOnlyUriPresent()
    {
        ValidLoggerWithParameters.Reset();
        var mockRequestData = new Mock<IRequestData>();
        var mockMetricsCollection = new Mock<IMetricsCollection>();
        mockRequestData.Setup(rd => rd.MetricsCollection).Returns(mockMetricsCollection.Object);

        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <LoggerRunSettings>
                    <Loggers>
                      <Logger uri=""test-logger-with-parameter://logger""></Logger>
                    </Loggers>
                  </LoggerRunSettings>
                </RunSettings>";

        var testLoggerManager = new DummyTestLoggerManager(mockRequestData.Object);
        testLoggerManager.Initialize(settingsXml);

        Assert.AreEqual(1, ValidLoggerWithParameters.Counter);
        mockMetricsCollection.Verify(
            rd => rd.Add(TelemetryDataConstants.LoggerUsed, "TestPlatform.CrossPlatEngine.UnitTests.TestLoggerManagerTests+ValidLoggerWithParameters"));
    }

    [TestMethod]
    public void InitializeShouldInitializeLoggerFromAssemblyNameWhenAssemblyNameAndCodeBasePresent()
    {
        ValidLoggerWithParameters.Reset();
        var mockRequestData = new Mock<IRequestData>();
        var mockMetricsCollection = new Mock<IMetricsCollection>();
        mockRequestData.Setup(rd => rd.MetricsCollection).Returns(mockMetricsCollection.Object);

        var assemblyQualifiedName = typeof(ValidLoggerWithParameters).AssemblyQualifiedName;
        var codeBase = typeof(TestLoggerManagerTests).GetTypeInfo().Assembly.Location;

        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <LoggerRunSettings>
                    <Loggers>
                      <Logger assemblyQualifiedName=""" + assemblyQualifiedName + @""" codeBase=""" + codeBase + @"""></Logger>
                    </Loggers>
                  </LoggerRunSettings>
                </RunSettings>";

        var testLoggerManager = new DummyTestLoggerManager(mockRequestData.Object);
        testLoggerManager.Initialize(settingsXml);

        Assert.AreEqual(1, ValidLoggerWithParameters.Counter);
        mockMetricsCollection.Verify(
            rd => rd.Add(TelemetryDataConstants.LoggerUsed, "TestPlatform.CrossPlatEngine.UnitTests.TestLoggerManagerTests+ValidLoggerWithParameters"));
    }

    [TestMethod]
    public void InitializeShouldNotInitializeLoggersWhenOnlyAssemblyNameIsPresent()
    {
        ValidLoggerWithParameters.Reset();
        var mockRequestData = new Mock<IRequestData>();
        var mockMetricsCollection = new Mock<IMetricsCollection>();
        mockRequestData.Setup(rd => rd.MetricsCollection).Returns(mockMetricsCollection.Object);

        var assemblyQualifiedName = typeof(ValidLoggerWithParameters).AssemblyQualifiedName;

        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <LoggerRunSettings>
                    <Loggers>
                      <Logger assemblyQualifiedName=""" + assemblyQualifiedName + @"""></Logger>
                    </Loggers>
                  </LoggerRunSettings>
                </RunSettings>";

        var testLoggerManager = new DummyTestLoggerManager(mockRequestData.Object);

        Assert.ThrowsException<InvalidLoggerException>(() => testLoggerManager.Initialize(settingsXml));
        Assert.AreEqual(0, ValidLoggerWithParameters.Counter);
    }

    [TestMethod]
    public void InitializeShouldNotInitializeLoggersFromAssemblyNameWhenInterfaceDoesNotMatch()
    {
        ValidLoggerWithParameters.Reset();
        var mockRequestData = new Mock<IRequestData>();
        var mockMetricsCollection = new Mock<IMetricsCollection>();
        mockRequestData.Setup(rd => rd.MetricsCollection).Returns(mockMetricsCollection.Object);

        var assemblyQualifiedName = typeof(InvalidLogger).AssemblyQualifiedName;
        var codeBase = typeof(TestLoggerManagerTests).GetTypeInfo().Assembly.Location;

        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <LoggerRunSettings>
                    <Loggers>
                      <Logger assemblyQualifiedName=""" + assemblyQualifiedName + @""" codeBase=""" + codeBase + @"""></Logger>
                    </Loggers>
                  </LoggerRunSettings>
                </RunSettings>";

        var testLoggerManager = new DummyTestLoggerManager(mockRequestData.Object);

        Assert.ThrowsException<InvalidLoggerException>(() => testLoggerManager.Initialize(settingsXml));
        Assert.AreEqual(0, InvalidLogger.Counter);
    }

    [TestMethod]
    public void InitializeShouldNotInitializeLoggersWhenAssemblyNameInvalid()
    {
        ValidLoggerWithParameters.Reset();
        var mockRequestData = new Mock<IRequestData>();
        var mockMetricsCollection = new Mock<IMetricsCollection>();
        mockRequestData.Setup(rd => rd.MetricsCollection).Returns(mockMetricsCollection.Object);

        var assemblyQualifiedName = "invalid";

        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <LoggerRunSettings>
                    <Loggers>
                      <Logger assemblyQualifiedName=""" + assemblyQualifiedName + @"""></Logger>
                    </Loggers>
                  </LoggerRunSettings>
                </RunSettings>";

        var testLoggerManager = new DummyTestLoggerManager(mockRequestData.Object);
        Assert.ThrowsException<InvalidLoggerException>(() => testLoggerManager.Initialize(settingsXml));
        Assert.AreEqual(0, ValidLoggerWithParameters.Counter);
    }

    [TestMethod]
    public void InitializeShouldNotInitializeLoggersWhenCodeBaseInvalid()
    {
        ValidLoggerWithParameters.Reset();
        var mockRequestData = new Mock<IRequestData>();
        var mockMetricsCollection = new Mock<IMetricsCollection>();
        mockRequestData.Setup(rd => rd.MetricsCollection).Returns(mockMetricsCollection.Object);

        var assemblyQualifiedName = typeof(InvalidLogger).AssemblyQualifiedName;
        var codeBase = typeof(TestLoggerManagerTests).GetTypeInfo().Assembly.Location;

        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <LoggerRunSettings>
                    <Loggers>
                      <Logger assemblyQualifiedName=""" + assemblyQualifiedName + @""" codeBase=""" + codeBase + @"""></Logger>
                    </Loggers>
                  </LoggerRunSettings>
                </RunSettings>";

        var testLoggerManager = new DummyTestLoggerManager(mockRequestData.Object);

        Assert.ThrowsException<InvalidLoggerException>(() => testLoggerManager.Initialize(settingsXml));
        Assert.AreEqual(0, ValidLoggerWithParameters.Counter);
    }

    [TestMethod]
    public void InitializeShouldInitializeLoggerOnceWhenMultipleLoggersWithSameAssemblyNamePresent()
    {
        // Duplicate loggers should be ignored
        ValidLoggerWithParameters.Reset();
        var mockRequestData = new Mock<IRequestData>();
        var mockMetricsCollection = new Mock<IMetricsCollection>();
        mockRequestData.Setup(rd => rd.MetricsCollection).Returns(mockMetricsCollection.Object);

        var assemblyQualifiedName = typeof(ValidLoggerWithParameters).AssemblyQualifiedName;
        var codeBase = typeof(TestLoggerManagerTests).GetTypeInfo().Assembly.Location;

        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <LoggerRunSettings>
                    <Loggers>
                      <Logger assemblyQualifiedName=""" + assemblyQualifiedName + @""" codeBase=""" + codeBase + @"""></Logger>
                      <Logger assemblyQualifiedName=""" + assemblyQualifiedName + @""" codeBase=""" + codeBase + @"""></Logger>
                    </Loggers>
                  </LoggerRunSettings>
                </RunSettings>";

        var testLoggerManager = new DummyTestLoggerManager(mockRequestData.Object);
        testLoggerManager.Initialize(settingsXml);

        Assert.AreEqual(1, ValidLoggerWithParameters.Counter);
        mockMetricsCollection.Verify(
            rd => rd.Add(TelemetryDataConstants.LoggerUsed, "TestPlatform.CrossPlatEngine.UnitTests.TestLoggerManagerTests+ValidLoggerWithParameters"));
    }

    [TestMethod]
    public void InitializeShouldInitializeLoggerOnce()
    {
        // Duplicate loggers should be ignored
        ValidLoggerWithParameters.Reset();
        var mockRequestData = new Mock<IRequestData>();
        var mockMetricsCollection = new Mock<IMetricsCollection>();
        mockRequestData.Setup(rd => rd.MetricsCollection).Returns(mockMetricsCollection.Object);

        var assemblyQualifiedName = typeof(ValidLoggerWithParameters).AssemblyQualifiedName;
        var codeBase = typeof(TestLoggerManagerTests).GetTypeInfo().Assembly.Location;

        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <LoggerRunSettings>
                    <Loggers>
                      <Logger friendlyName=""TestLoggerWithParameterExtension""></Logger>
                      <Logger uri=""test-logger-with-parameter://logger""></Logger>
                      <Logger assemblyQualifiedName=""" + assemblyQualifiedName + @""" codeBase=""" + codeBase + @"""></Logger>
                      <Logger assemblyQualifiedName=""" + assemblyQualifiedName + @""" codeBase=""" + codeBase + @"""></Logger>
                    </Loggers>
                  </LoggerRunSettings>
                </RunSettings>";

        var testLoggerManager = new DummyTestLoggerManager(mockRequestData.Object);
        testLoggerManager.Initialize(settingsXml);

        Assert.AreEqual(1, ValidLoggerWithParameters.Counter);
        mockMetricsCollection.Verify(
            rd => rd.Add(TelemetryDataConstants.LoggerUsed, "TestPlatform.CrossPlatEngine.UnitTests.TestLoggerManagerTests+ValidLoggerWithParameters"));
    }

    [TestMethod]
    public void InitializeShouldNotConsiderLoggerAsInitializedWhenInitializationErrorOccurs()
    {
        ValidLoggerWithParameters.Reset();
        var mockRequestData = new Mock<IRequestData>();
        var mockMetricsCollection = new Mock<IMetricsCollection>();
        mockRequestData.Setup(rd => rd.MetricsCollection).Returns(mockMetricsCollection.Object);

        var assemblyQualifiedName = typeof(LoggerWithInitializationError).AssemblyQualifiedName;
        var codeBase = typeof(TestLoggerManagerTests).GetTypeInfo().Assembly.Location;

        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <LoggerRunSettings>
                    <Loggers>
                      <Logger assemblyQualifiedName=""" + assemblyQualifiedName + @""" codeBase=""" + codeBase + @"""></Logger>
                    </Loggers>
                  </LoggerRunSettings>
                </RunSettings>";

        var testLoggerManager = new DummyTestLoggerManager(mockRequestData.Object);

        Assert.ThrowsException<InvalidLoggerException>(() => testLoggerManager.Initialize(settingsXml));
    }

    [TestMethod]
    public void InitializeShouldThrowWhenLoggerManagerAlreadyDisposed()
    {
        ValidLoggerWithParameters.Reset();
        var mockRequestData = new Mock<IRequestData>();
        var mockMetricsCollection = new Mock<IMetricsCollection>();
        mockRequestData.Setup(rd => rd.MetricsCollection).Returns(mockMetricsCollection.Object);

        var assemblyQualifiedName = typeof(ValidLoggerWithParameters).AssemblyQualifiedName;
        var codeBase = typeof(TestLoggerManagerTests).GetTypeInfo().Assembly.Location;

        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <LoggerRunSettings>
                    <Loggers>
                      <Logger assemblyQualifiedName=""" + assemblyQualifiedName + @""" codeBase=""" + codeBase + @"""></Logger>
                    </Loggers>
                  </LoggerRunSettings>
                </RunSettings>";

        var testLoggerManager = new DummyTestLoggerManager(mockRequestData.Object);
        testLoggerManager.Dispose();
        Assert.ThrowsException<ObjectDisposedException>(() => testLoggerManager.Initialize(settingsXml));
    }

    [TestMethod]
    public void InitializeShouldInitilaizeMultipleLoggersIfPresent()
    {
        ValidLoggerWithParameters.Reset();
        var mockRequestData = new Mock<IRequestData>();
        var mockMetricsCollection = new Mock<IMetricsCollection>();
        mockRequestData.Setup(rd => rd.MetricsCollection).Returns(mockMetricsCollection.Object);

        var assemblyQualifiedName = typeof(ValidLoggerWithParameters).AssemblyQualifiedName;
        var codeBase = typeof(TestLoggerManagerTests).GetTypeInfo().Assembly.Location;

        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <LoggerRunSettings>
                    <Loggers>
                      <Logger friendlyName=""TestLoggerExtension"" />
                      <Logger uri=""testlogger://logger2""></Logger>
                      <Logger assemblyQualifiedName=""" + assemblyQualifiedName + @""" codeBase=""" + codeBase + @"""></Logger>
                    </Loggers>
                  </LoggerRunSettings>
                </RunSettings>";

        var testLoggerManager = new DummyTestLoggerManager(mockRequestData.Object);
        testLoggerManager.Initialize(settingsXml);

        Assert.AreEqual(1, ValidLoggerWithParameters.Counter);
        mockMetricsCollection.Verify(
            rd => rd.Add(TelemetryDataConstants.LoggerUsed, "TestPlatform.CrossPlatEngine.UnitTests.TestLoggerManagerTests+ValidLogger,TestPlatform.CrossPlatEngine.UnitTests.TestLoggerManagerTests+ValidLogger2,TestPlatform.CrossPlatEngine.UnitTests.TestLoggerManagerTests+ValidLoggerWithParameters"));
    }

    [TestMethod]
    public void AreLoggersInitializedShouldReturnTrueWhenLoggersInitialized()
    {
        ValidLoggerWithParameters.Reset();
        var mockRequestData = new Mock<IRequestData>();
        var mockMetricsCollection = new Mock<IMetricsCollection>();
        mockRequestData.Setup(rd => rd.MetricsCollection).Returns(mockMetricsCollection.Object);

        var assemblyQualifiedName = typeof(ValidLoggerWithParameters).AssemblyQualifiedName;
        var codeBase = typeof(TestLoggerManagerTests).GetTypeInfo().Assembly.Location;

        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <LoggerRunSettings>
                    <Loggers>
                      <Logger friendlyName=""TestLoggerExtension"" />
                      <Logger uri=""testlogger://logger2""></Logger>
                      <Logger assemblyQualifiedName=""" + assemblyQualifiedName + @""" codeBase=""" + codeBase + @"""></Logger>
                    </Loggers>
                  </LoggerRunSettings>
                </RunSettings>";

        var testLoggerManager = new DummyTestLoggerManager(mockRequestData.Object);
        testLoggerManager.Initialize(settingsXml);

        Assert.IsTrue(testLoggerManager.LoggersInitialized);
    }

    [TestMethod]
    public void AreLoggersInitializedShouldReturnFalseWhenLoggersNotInitialized()
    {
        ValidLoggerWithParameters.Reset();
        var mockRequestData = new Mock<IRequestData>();
        var mockMetricsCollection = new Mock<IMetricsCollection>();
        mockRequestData.Setup(rd => rd.MetricsCollection).Returns(mockMetricsCollection.Object);

        var assemblyQualifiedName = typeof(ValidLoggerWithParameters).AssemblyQualifiedName;
        var codeBase = typeof(TestLoggerManagerTests).GetTypeInfo().Assembly.Location;

        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <LoggerRunSettings>
                    <Loggers>
                      <Logger friendlyName=""TestLoggerExtension"" />
                      <Logger uri=""testlogger://logger2""></Logger>
                      <Logger assemblyQualifiedName=""" + assemblyQualifiedName + @""" codeBase=""" + codeBase + @"""></Logger>
                    </Loggers>
                  </LoggerRunSettings>
                </RunSettings>";

        var testLoggerManager = new DummyTestLoggerManager(mockRequestData.Object);

        Assert.IsFalse(testLoggerManager.LoggersInitialized);
    }

    [TestMethod]
    public void AreLoggersInitializedShouldReturnFalseWhenNoLoggersPresent()
    {
        ValidLoggerWithParameters.Reset();
        var mockRequestData = new Mock<IRequestData>();
        var mockMetricsCollection = new Mock<IMetricsCollection>();
        mockRequestData.Setup(rd => rd.MetricsCollection).Returns(mockMetricsCollection.Object);

        var assemblyQualifiedName = typeof(ValidLoggerWithParameters).AssemblyQualifiedName;
        var codeBase = typeof(TestLoggerManagerTests).GetTypeInfo().Assembly.Location;

        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <LoggerRunSettings>
                    <Loggers>
                    </Loggers>
                  </LoggerRunSettings>
                </RunSettings>";

        var testLoggerManager = new DummyTestLoggerManager(mockRequestData.Object);
        testLoggerManager.Initialize(settingsXml);

        Assert.IsFalse(testLoggerManager.LoggersInitialized);
    }

    [TestMethod]
    public void InitializeShouldPassConfigurationElementAsParameters()
    {
        ValidLoggerWithParameters.Reset();
        var mockRequestData = new Mock<IRequestData>();
        var mockMetricsCollection = new Mock<IMetricsCollection>();
        mockRequestData.Setup(rd => rd.MetricsCollection).Returns(mockMetricsCollection.Object);

        var assemblyQualifiedName = typeof(ValidLoggerWithParameters).AssemblyQualifiedName;
        var codeBase = typeof(TestLoggerManagerTests).GetTypeInfo().Assembly.Location;

        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <LoggerRunSettings>
                    <Loggers>
                      <Logger friendlyName=""TestLoggerExtension"" />
                      <Logger uri=""testlogger://logger2""></Logger>
                      <Logger assemblyQualifiedName=""" + assemblyQualifiedName + @""" codeBase=""" + codeBase + @""">
                        <Configuration>
                          <Key1>Value1</Key1>
                          <Key2>Value2</Key2>
                        </Configuration>
                      </Logger>
                    </Loggers>
                  </LoggerRunSettings>
                </RunSettings>";

        var testLoggerManager = new DummyTestLoggerManager(mockRequestData.Object);
        testLoggerManager.Initialize(settingsXml);

        Assert.AreEqual(1, ValidLoggerWithParameters.Counter);
        Assert.AreEqual(4, ValidLoggerWithParameters.Parameters!.Count); // Two additional because of testRunDirectory and targetFramework
        Assert.AreEqual("Value1", ValidLoggerWithParameters.Parameters["Key1"]);
        Assert.AreEqual("Value2", ValidLoggerWithParameters.Parameters["Key2"]);

        mockMetricsCollection.Verify(
            rd => rd.Add(TelemetryDataConstants.LoggerUsed, "TestPlatform.CrossPlatEngine.UnitTests.TestLoggerManagerTests+ValidLogger,TestPlatform.CrossPlatEngine.UnitTests.TestLoggerManagerTests+ValidLogger2,TestPlatform.CrossPlatEngine.UnitTests.TestLoggerManagerTests+ValidLoggerWithParameters"));
    }

    [TestMethod]
    public void InitializeShouldSkipEmptyConfigurationValueInParameters()
    {
        ValidLoggerWithParameters.Reset();
        var mockRequestData = new Mock<IRequestData>();
        var mockMetricsCollection = new Mock<IMetricsCollection>();
        mockRequestData.Setup(rd => rd.MetricsCollection).Returns(mockMetricsCollection.Object);

        var assemblyQualifiedName = typeof(ValidLoggerWithParameters).AssemblyQualifiedName;
        var codeBase = typeof(TestLoggerManagerTests).GetTypeInfo().Assembly.Location;

        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <LoggerRunSettings>
                    <Loggers>
                      <Logger friendlyName=""TestLoggerExtension"" />
                      <Logger uri=""testlogger://logger2""></Logger>
                      <Logger assemblyQualifiedName=""" + assemblyQualifiedName + @""" codeBase=""" + codeBase + @""">
                        <Configuration>
                          <Key1> </Key1>
                          <Key2>Value2</Key2>
                        </Configuration>
                      </Logger>
                    </Loggers>
                  </LoggerRunSettings>
                </RunSettings>";

        var testLoggerManager = new DummyTestLoggerManager(mockRequestData.Object);
        testLoggerManager.Initialize(settingsXml);

        Assert.AreEqual(1, ValidLoggerWithParameters.Counter);
        Assert.AreEqual(3, ValidLoggerWithParameters.Parameters!.Count); // Two additional because of testRunDirectory and targetFramework
        Assert.IsFalse(ValidLoggerWithParameters.Parameters.TryGetValue("Key1", out var key1Value));
        Assert.AreEqual("Value2", ValidLoggerWithParameters.Parameters["Key2"]);

        mockMetricsCollection.Verify(
            rd => rd.Add(TelemetryDataConstants.LoggerUsed, "TestPlatform.CrossPlatEngine.UnitTests.TestLoggerManagerTests+ValidLogger,TestPlatform.CrossPlatEngine.UnitTests.TestLoggerManagerTests+ValidLogger2,TestPlatform.CrossPlatEngine.UnitTests.TestLoggerManagerTests+ValidLoggerWithParameters"));
    }

    [TestMethod]
    public void InitializeShouldUseLastValueInParametersForDuplicateConfigurationValue()
    {
        ValidLoggerWithParameters.Reset();
        var mockRequestData = new Mock<IRequestData>();
        var mockMetricsCollection = new Mock<IMetricsCollection>();
        mockRequestData.Setup(rd => rd.MetricsCollection).Returns(mockMetricsCollection.Object);

        var assemblyQualifiedName = typeof(ValidLoggerWithParameters).AssemblyQualifiedName;
        var codeBase = typeof(TestLoggerManagerTests).GetTypeInfo().Assembly.Location;

        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <LoggerRunSettings>
                    <Loggers>
                      <Logger friendlyName=""TestLoggerExtension"" />
                      <Logger uri=""testlogger://logger2""></Logger>
                      <Logger assemblyQualifiedName=""" + assemblyQualifiedName + @""" codeBase=""" + codeBase + @""">
                        <Configuration>
                          <Key1>Value1</Key1>
                          <Key2>Value2</Key2>
                          <Key1>Value3</Key1>
                        </Configuration>
                      </Logger>
                    </Loggers>
                  </LoggerRunSettings>
                </RunSettings>";

        var testLoggerManager = new DummyTestLoggerManager(mockRequestData.Object);
        testLoggerManager.Initialize(settingsXml);

        Assert.AreEqual(1, ValidLoggerWithParameters.Counter);
        Assert.AreEqual(4, ValidLoggerWithParameters.Parameters!.Count); // Two additional because of testRunDirectory and targetFramework
        Assert.AreEqual("Value3", ValidLoggerWithParameters.Parameters["Key1"]);
        Assert.AreEqual("Value2", ValidLoggerWithParameters.Parameters["Key2"]);

        mockMetricsCollection.Verify(
            rd => rd.Add(TelemetryDataConstants.LoggerUsed, "TestPlatform.CrossPlatEngine.UnitTests.TestLoggerManagerTests+ValidLogger,TestPlatform.CrossPlatEngine.UnitTests.TestLoggerManagerTests+ValidLogger2,TestPlatform.CrossPlatEngine.UnitTests.TestLoggerManagerTests+ValidLoggerWithParameters"));
    }

    [TestMethod]
    public void InitializeShouldNotInitializeDisabledLoggers()
    {
        ValidLoggerWithParameters.Reset();
        var mockRequestData = new Mock<IRequestData>();
        var mockMetricsCollection = new Mock<IMetricsCollection>();
        mockRequestData.Setup(rd => rd.MetricsCollection).Returns(mockMetricsCollection.Object);

        var assemblyQualifiedName = typeof(ValidLoggerWithParameters).AssemblyQualifiedName;
        var codeBase = typeof(TestLoggerManagerTests).GetTypeInfo().Assembly.Location;

        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <LoggerRunSettings>
                    <Loggers>
                      <Logger friendlyName=""TestLoggerExtension"" />
                      <Logger uri=""testlogger://logger2"" enabled=""true""></Logger>
                      <Logger assemblyQualifiedName=""" + assemblyQualifiedName + @""" codeBase=""" + codeBase + @""" enabled=""false""></Logger>
                    </Loggers>
                  </LoggerRunSettings>
                </RunSettings>";

        var testLoggerManager = new DummyTestLoggerManager(mockRequestData.Object);
        testLoggerManager.Initialize(settingsXml);

        Assert.AreEqual(0, ValidLoggerWithParameters.Counter);
        mockMetricsCollection.Verify(
            rd => rd.Add(TelemetryDataConstants.LoggerUsed, "TestPlatform.CrossPlatEngine.UnitTests.TestLoggerManagerTests+ValidLogger,TestPlatform.CrossPlatEngine.UnitTests.TestLoggerManagerTests+ValidLogger2"));
    }

    [TestMethod]
    public void InitializeShouldInitializeFromAssemblyNameIfAllAttributesPresent()
    {
        ValidLoggerWithParameters.Reset();
        var mockRequestData = new Mock<IRequestData>();
        var mockMetricsCollection = new Mock<IMetricsCollection>();
        mockRequestData.Setup(rd => rd.MetricsCollection).Returns(mockMetricsCollection.Object);

        var assemblyQualifiedName = typeof(ValidLoggerWithParameters).AssemblyQualifiedName;
        var codeBase = typeof(TestLoggerManagerTests).GetTypeInfo().Assembly.Location;

        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <LoggerRunSettings>
                    <Loggers>
                      <Logger friendlyName=""TestLoggerExtension"" />
                      <Logger uri=""testlogger://logger2"" enabled=""true""></Logger>
                      <Logger friendlyName=""invalidName"" uri=""invalid://invalidUri"" assemblyQualifiedName=""" + assemblyQualifiedName + @""" codeBase=""" + codeBase + @""">
                        <Configuration>
                          <Key1>Value1</Key1>
                          <Key2>Value2</Key2>
                        </Configuration>
                      </Logger>
                    </Loggers>
                  </LoggerRunSettings>
                </RunSettings>";

        var testLoggerManager = new DummyTestLoggerManager(mockRequestData.Object);
        testLoggerManager.Initialize(settingsXml);

        Assert.AreEqual(1, ValidLoggerWithParameters.Counter);
        Assert.AreEqual(4, ValidLoggerWithParameters.Parameters!.Count); // Two additional because of testRunDirectory and targetFramework
        Assert.AreEqual("Value1", ValidLoggerWithParameters.Parameters["Key1"]);
        Assert.AreEqual("Value2", ValidLoggerWithParameters.Parameters["Key2"]);
        mockMetricsCollection.Verify(
            rd => rd.Add(TelemetryDataConstants.LoggerUsed, "TestPlatform.CrossPlatEngine.UnitTests.TestLoggerManagerTests+ValidLogger,TestPlatform.CrossPlatEngine.UnitTests.TestLoggerManagerTests+ValidLogger2,TestPlatform.CrossPlatEngine.UnitTests.TestLoggerManagerTests+ValidLoggerWithParameters"));
    }

    [TestMethod]
    public void InitializeShouldInitializeFromUriIfUriAndNamePresent()
    {
        ValidLoggerWithParameters.Reset();
        var mockRequestData = new Mock<IRequestData>();
        var mockMetricsCollection = new Mock<IMetricsCollection>();
        mockRequestData.Setup(rd => rd.MetricsCollection).Returns(mockMetricsCollection.Object);

        var codeBase = typeof(TestLoggerManagerTests).GetTypeInfo().Assembly.Location;

        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <LoggerRunSettings>
                    <Loggers>
                      <Logger friendlyName=""TestLoggerExtension"" />
                      <Logger uri=""testlogger://logger2"" enabled=""true""></Logger>
                      <Logger friendlyName=""invalidName"" uri=""test-logger-with-parameter://logger"" codeBase=""" + codeBase + @""">
                        <Configuration>
                          <Key1>Value1</Key1>
                          <Key2>Value2</Key2>
                        </Configuration>
                      </Logger>
                    </Loggers>
                  </LoggerRunSettings>
                </RunSettings>";

        var testLoggerManager = new DummyTestLoggerManager(mockRequestData.Object);
        testLoggerManager.Initialize(settingsXml);

        Assert.AreEqual(1, ValidLoggerWithParameters.Counter);
        Assert.AreEqual(4, ValidLoggerWithParameters.Parameters!.Count); // Two additional because of testRunDirectory and targetFramework
        Assert.AreEqual("Value1", ValidLoggerWithParameters.Parameters["Key1"]);
        Assert.AreEqual("Value2", ValidLoggerWithParameters.Parameters["Key2"]);
        mockMetricsCollection.Verify(
            rd => rd.Add(TelemetryDataConstants.LoggerUsed, "TestPlatform.CrossPlatEngine.UnitTests.TestLoggerManagerTests+ValidLogger,TestPlatform.CrossPlatEngine.UnitTests.TestLoggerManagerTests+ValidLogger2,TestPlatform.CrossPlatEngine.UnitTests.TestLoggerManagerTests+ValidLoggerWithParameters"));
    }

    [TestMethod]
    public void InitializeShouldInitializeFromUriIfUnableToFromAssemblyName()
    {
        ValidLoggerWithParameters.Reset();
        var mockRequestData = new Mock<IRequestData>();
        var mockMetricsCollection = new Mock<IMetricsCollection>();
        mockRequestData.Setup(rd => rd.MetricsCollection).Returns(mockMetricsCollection.Object);

        var codeBase = typeof(TestLoggerManagerTests).GetTypeInfo().Assembly.Location;

        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <LoggerRunSettings>
                    <Loggers>
                      <Logger friendlyName=""TestLoggerExtension"" />
                      <Logger uri=""testlogger://logger2"" enabled=""true""></Logger>
                      <Logger friendlyName=""invalidName"" uri=""test-logger-with-parameter://logger"" assemblyQualifiedName=""invalidAssembly"" codeBase=""" + codeBase + @""">
                        <Configuration>
                          <Key1>Value1</Key1>
                          <Key2>Value2</Key2>
                        </Configuration>
                      </Logger>
                    </Loggers>
                  </LoggerRunSettings>
                </RunSettings>";

        var testLoggerManager = new DummyTestLoggerManager(mockRequestData.Object);
        testLoggerManager.Initialize(settingsXml);

        Assert.AreEqual(1, ValidLoggerWithParameters.Counter);
        Assert.AreEqual(4, ValidLoggerWithParameters.Parameters!.Count); // Two additional because of testRunDirectory and targetFramework
        Assert.AreEqual("Value1", ValidLoggerWithParameters.Parameters["Key1"]);
        Assert.AreEqual("Value2", ValidLoggerWithParameters.Parameters["Key2"]);
        mockMetricsCollection.Verify(
            rd => rd.Add(TelemetryDataConstants.LoggerUsed, "TestPlatform.CrossPlatEngine.UnitTests.TestLoggerManagerTests+ValidLogger,TestPlatform.CrossPlatEngine.UnitTests.TestLoggerManagerTests+ValidLogger2,TestPlatform.CrossPlatEngine.UnitTests.TestLoggerManagerTests+ValidLoggerWithParameters"));
    }

    [TestMethod]
    public void InitializeShouldInitializeFromNameIfUnableToFromUri()
    {
        ValidLoggerWithParameters.Reset();
        var mockRequestData = new Mock<IRequestData>();
        var mockMetricsCollection = new Mock<IMetricsCollection>();
        mockRequestData.Setup(rd => rd.MetricsCollection).Returns(mockMetricsCollection.Object);

        var codeBase = typeof(TestLoggerManagerTests).GetTypeInfo().Assembly.Location;

        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <LoggerRunSettings>
                    <Loggers>
                      <Logger friendlyName=""TestLoggerExtension"" />
                      <Logger uri=""testlogger://logger2"" enabled=""true""></Logger>
                      <Logger friendlyName=""TestLoggerWithParameterExtension"" uri=""invalid://invalid"" assemblyQualifiedName=""invalidAssembly"" codeBase=""" + codeBase + @""">
                        <Configuration>
                          <Key1>Value1</Key1>
                          <Key2>Value2</Key2>
                        </Configuration>
                      </Logger>
                    </Loggers>
                  </LoggerRunSettings>
                </RunSettings>";

        var testLoggerManager = new DummyTestLoggerManager(mockRequestData.Object);
        testLoggerManager.Initialize(settingsXml);

        Assert.AreEqual(1, ValidLoggerWithParameters.Counter);
        Assert.AreEqual(4, ValidLoggerWithParameters.Parameters!.Count); // Two additional because of testRunDirectory and targetFramework
        Assert.AreEqual("Value1", ValidLoggerWithParameters.Parameters["Key1"]);
        Assert.AreEqual("Value2", ValidLoggerWithParameters.Parameters["Key2"]);
        mockMetricsCollection.Verify(
            rd => rd.Add(TelemetryDataConstants.LoggerUsed, "TestPlatform.CrossPlatEngine.UnitTests.TestLoggerManagerTests+ValidLogger,TestPlatform.CrossPlatEngine.UnitTests.TestLoggerManagerTests+ValidLogger2,TestPlatform.CrossPlatEngine.UnitTests.TestLoggerManagerTests+ValidLoggerWithParameters"));
    }

    [TestMethod]
    public void InitializeShouldInitializeLoggersWithTestRunDirectoryIfPresentInRunSettings()
    {
        ValidLoggerWithParameters.Reset();
        var mockRequestData = new Mock<IRequestData>();
        var mockMetricsCollection = new Mock<IMetricsCollection>();
        mockRequestData.Setup(rd => rd.MetricsCollection).Returns(mockMetricsCollection.Object);

        var codeBase = typeof(TestLoggerManagerTests).GetTypeInfo().Assembly.Location;

        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <RunConfiguration>
                    <MaxCpuCount>0</MaxCpuCount>
                    <ResultsDirectory>DummyTestResultsFolder</ResultsDirectory>
                    <TargetPlatform> x64 </TargetPlatform>
                    <TargetFrameworkVersion> Framework45 </TargetFrameworkVersion>
                  </RunConfiguration>
                  <LoggerRunSettings>
                    <Loggers>
                      <Logger friendlyName=""TestLoggerExtension"" />
                      <Logger uri=""testlogger://logger2"" enabled=""true""></Logger>
                      <Logger friendlyName=""TestLoggerWithParameterExtension"" uri=""invalid://invalid"" assemblyQualifiedName=""invalidAssembly"" codeBase=""" + codeBase + @""">
                        <Configuration>
                          <Key1>Value1</Key1>
                          <Key2>Value2</Key2>
                        </Configuration>
                      </Logger>
                    </Loggers>
                  </LoggerRunSettings>
                </RunSettings>";

        var testLoggerManager = new DummyTestLoggerManager(mockRequestData.Object);
        testLoggerManager.Initialize(settingsXml);

        Assert.AreEqual(1, ValidLoggerWithParameters.Counter);
        Assert.AreEqual(4, ValidLoggerWithParameters.Parameters!.Count); // Two additional because of testRunDirectory and targetFramework
        Assert.AreEqual("Value1", ValidLoggerWithParameters.Parameters["Key1"]);
        Assert.AreEqual("DummyTestResultsFolder", ValidLoggerWithParameters.Parameters["testRunDirectory"]);
        Assert.AreEqual("Value2", ValidLoggerWithParameters.Parameters["Key2"]);
        mockMetricsCollection.Verify(
            rd => rd.Add(TelemetryDataConstants.LoggerUsed, "TestPlatform.CrossPlatEngine.UnitTests.TestLoggerManagerTests+ValidLogger,TestPlatform.CrossPlatEngine.UnitTests.TestLoggerManagerTests+ValidLogger2,TestPlatform.CrossPlatEngine.UnitTests.TestLoggerManagerTests+ValidLoggerWithParameters"));
    }

    [TestMethod]
    public void InitializeShouldInitializeLoggersWithDefaultTestRunDirectoryIfNotPresentInRunSettings()
    {
        ValidLoggerWithParameters.Reset();
        var mockRequestData = new Mock<IRequestData>();
        var mockMetricsCollection = new Mock<IMetricsCollection>();
        mockRequestData.Setup(rd => rd.MetricsCollection).Returns(mockMetricsCollection.Object);

        var codeBase = typeof(TestLoggerManagerTests).GetTypeInfo().Assembly.Location;

        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <RunConfiguration>
                    <MaxCpuCount>0</MaxCpuCount>
                    <TargetPlatform> x64 </TargetPlatform>
                    <TargetFrameworkVersion> Framework45 </TargetFrameworkVersion>
                  </RunConfiguration>
                  <LoggerRunSettings>
                    <Loggers>
                      <Logger friendlyName=""TestLoggerExtension"" />
                      <Logger uri=""testlogger://logger2"" enabled=""true""></Logger>
                      <Logger friendlyName=""TestLoggerWithParameterExtension"" uri=""invalid://invalid"" assemblyQualifiedName=""invalidAssembly"" codeBase=""" + codeBase + @""">
                        <Configuration>
                          <Key1>Value1</Key1>
                          <Key2>Value2</Key2>
                        </Configuration>
                      </Logger>
                    </Loggers>
                  </LoggerRunSettings>
                </RunSettings>";

        var testLoggerManager = new DummyTestLoggerManager(mockRequestData.Object);
        testLoggerManager.Initialize(settingsXml);

        Assert.AreEqual(1, ValidLoggerWithParameters.Counter);
        Assert.AreEqual(4, ValidLoggerWithParameters.Parameters!.Count); // Two additional because of testRunDirectory and targetFramework
        Assert.AreEqual("Value1", ValidLoggerWithParameters.Parameters["Key1"]);
        Assert.AreEqual(Constants.DefaultResultsDirectory, ValidLoggerWithParameters.Parameters["testRunDirectory"]);
        Assert.AreEqual("Value2", ValidLoggerWithParameters.Parameters["Key2"]);
        mockMetricsCollection.Verify(
            rd => rd.Add(TelemetryDataConstants.LoggerUsed, "TestPlatform.CrossPlatEngine.UnitTests.TestLoggerManagerTests+ValidLogger,TestPlatform.CrossPlatEngine.UnitTests.TestLoggerManagerTests+ValidLogger2,TestPlatform.CrossPlatEngine.UnitTests.TestLoggerManagerTests+ValidLoggerWithParameters"));
    }

    [TestMethod]
    public void InitializeShouldNotInitializeIfUnableToFromName()
    {
        ValidLoggerWithParameters.Reset();
        var mockRequestData = new Mock<IRequestData>();
        var mockMetricsCollection = new Mock<IMetricsCollection>();
        mockRequestData.Setup(rd => rd.MetricsCollection).Returns(mockMetricsCollection.Object);

        var codeBase = typeof(TestLoggerManagerTests).GetTypeInfo().Assembly.Location;

        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <LoggerRunSettings>
                    <Loggers>
                      <Logger friendlyName=""TestLoggerExtension"" />
                      <Logger uri=""testlogger://logger2"" enabled=""true""></Logger>
                      <Logger friendlyName=""invalid"" uri=""invalid://invalid"" assemblyQualifiedName=""invalidAssembly"" codeBase=""" + codeBase + @""">
                        <Configuration>
                          <Key1>Value1</Key1>
                          <Key2>Value2</Key2>
                        </Configuration>
                      </Logger>
                    </Loggers>
                  </LoggerRunSettings>
                </RunSettings>";

        var testLoggerManager = new DummyTestLoggerManager(mockRequestData.Object);

        Assert.ThrowsException<InvalidLoggerException>(() => testLoggerManager.Initialize(settingsXml));
        Assert.AreEqual(0, ValidLoggerWithParameters.Counter);
    }

    [TestMethod]
    public void InitializeShouldNotInitializeAnyLoggerIfNoLoggerPresent()
    {
        ValidLoggerWithParameters.Reset();
        var mockRequestData = new Mock<IRequestData>();
        var mockMetricsCollection = new Mock<IMetricsCollection>();
        mockRequestData.Setup(rd => rd.MetricsCollection).Returns(mockMetricsCollection.Object);

        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                </RunSettings>";

        var testLoggerManager = new DummyTestLoggerManager(mockRequestData.Object);
        testLoggerManager.Initialize(settingsXml);

        Assert.AreEqual(0, ValidLoggerWithParameters.Counter);
        mockMetricsCollection.Verify(
            rd => rd.Add(TelemetryDataConstants.LoggerUsed, ""));
    }

    [TestMethod]
    public void InitializeShouldNotInitializeAnyLoggerIfEmptyLoggerRunSettings()
    {
        ValidLoggerWithParameters.Reset();
        var mockRequestData = new Mock<IRequestData>();
        var mockMetricsCollection = new Mock<IMetricsCollection>();
        mockRequestData.Setup(rd => rd.MetricsCollection).Returns(mockMetricsCollection.Object);

        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <LoggerRunSettings>
                  </LoggerRunSettings>
                </RunSettings>";

        var testLoggerManager = new DummyTestLoggerManager(mockRequestData.Object);
        testLoggerManager.Initialize(settingsXml);

        Assert.AreEqual(0, ValidLoggerWithParameters.Counter);
        mockMetricsCollection.Verify(
            rd => rd.Add(TelemetryDataConstants.LoggerUsed, ""));
    }

    [TestMethod]
    public void InitializeShouldNotThrowWhenLoggersNotPresentInRunSettings()
    {
        ValidLoggerWithParameters.Reset();
        var mockRequestData = new Mock<IRequestData>();
        var mockMetricsCollection = new Mock<IMetricsCollection>();
        mockRequestData.Setup(rd => rd.MetricsCollection).Returns(mockMetricsCollection.Object);

        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                </RunSettings>";

        var testLoggerManager = new DummyTestLoggerManager(mockRequestData.Object);
        testLoggerManager.Initialize(settingsXml);

        Assert.AreEqual(0, ValidLoggerWithParameters.Counter);
        mockMetricsCollection.Verify(
            rd => rd.Add(TelemetryDataConstants.LoggerUsed, ""));
    }

    [TestMethod]
    public void InitializeShouldNotInitializeAnyLoggerIfEmptyLoggersNode()
    {
        ValidLoggerWithParameters.Reset();
        var mockRequestData = new Mock<IRequestData>();
        var mockMetricsCollection = new Mock<IMetricsCollection>();
        mockRequestData.Setup(rd => rd.MetricsCollection).Returns(mockMetricsCollection.Object);

        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <LoggerRunSettings>
                    <Loggers>
                    </Loggers>
                  </LoggerRunSettings>
                </RunSettings>";

        var testLoggerManager = new DummyTestLoggerManager(mockRequestData.Object);
        testLoggerManager.Initialize(settingsXml);

        Assert.AreEqual(0, ValidLoggerWithParameters.Counter);
        mockMetricsCollection.Verify(
            rd => rd.Add(TelemetryDataConstants.LoggerUsed, ""));
    }

    [ExtensionUri("testlogger://logger")]
    [FriendlyName("TestLoggerExtension")]
    private class ValidLogger : ITestLogger
    {
        public void Initialize(TestLoggerEvents events, string testRunDirectory)
        {
            events.TestRunMessage += TestMessageHandler;
            events.TestRunComplete += Events_TestRunComplete;
            events.TestResult += Events_TestResult;
            events.TestRunStart += TestRunStartHandler;

            events.DiscoveryStart += DiscoveryStartHandler;
            events.DiscoveryMessage += DiscoveryMessageHandler;
            events.DiscoveredTests += DiscoveredTestsHandler;
            events.DiscoveryComplete += DiscoveryCompleteHandler;
        }

        private void Events_TestResult(object? sender, TestResultEventArgs e)
        {
            s_counter++;
            WaitHandle.Set();
        }

        private void Events_TestRunComplete(object? sender, TestRunCompleteEventArgs e)
        {
            s_counter++;
            WaitHandle.Set();

        }

        private void TestMessageHandler(object? sender, TestRunMessageEventArgs e)
        {
            if (e.Message.Equals("TestRunMessage"))
            {
                s_counter++;
                WaitHandle.Set();

            }
        }

        private void TestRunStartHandler(object? sender, TestRunStartEventArgs e)
        {
            s_counter++;
            WaitHandle.Set();
        }

        private void DiscoveryMessageHandler(object? sender, TestRunMessageEventArgs e)
        {
            s_counter++;
            WaitHandle.Set();
        }

        private void DiscoveryStartHandler(object? sender, DiscoveryStartEventArgs e)
        {
            s_counter++;
            WaitHandle.Set();
        }

        private void DiscoveredTestsHandler(object? sender, DiscoveredTestsEventArgs e)
        {
            s_counter++;
            WaitHandle.Set();
        }

        private void DiscoveryCompleteHandler(object? sender, DiscoveryCompleteEventArgs e)
        {
            s_counter++;
            WaitHandle.Set();
        }
    }

    [ExtensionUri("testlogger://logger2")]
    [FriendlyName("TestLoggerExtension2")]
    private class ValidLogger2 : ITestLogger
    {
        public void Initialize(TestLoggerEvents events, string testRunDirectory)
        {
        }

    }

    [ExtensionUri("testlogger://invalidLogger")]
    [FriendlyName("InvalidTestLoggerExtension")]
    private class InvalidLogger
    {
        public static int Counter;

        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Usage is unclear so keeping as non-static")]
        public void Initialize(TestLoggerEvents _, string _2)
        {
            Counter++;
        }

    }

    [ExtensionUri("testlogger://loggerWithError")]
    [FriendlyName("ErroredTestLoggerExtension")]
    private class LoggerWithInitializationError : ITestLogger
    {
        public void Initialize(TestLoggerEvents events, string testRunDirectory)
        {
            throw new Exception();
        }
    }

    [ExtensionUri("test-logger-with-parameter://logger")]
    [FriendlyName("TestLoggerWithParameterExtension")]
    private class ValidLoggerWithParameters : ITestLoggerWithParameters
    {
        public static Dictionary<string, string?>? Parameters;
        public static int Counter;

        public void Initialize(TestLoggerEvents events, string testRunDirectory)
        {
            Counter += 2;
        }

        public void Initialize(TestLoggerEvents events, Dictionary<string, string?> parameters)
        {
            Counter++;
            Parameters = parameters;
        }

        public static void Reset()
        {
            Counter = 0;
            Parameters = null;
        }
    }

    internal class DummyTestLoggerManager : TestLoggerManager
    {
        public DummyTestLoggerManager()
            : base(null!, TestSessionMessageLogger.Instance, new InternalTestLoggerEvents(TestSessionMessageLogger.Instance))
        {

        }

        public DummyTestLoggerManager(IRequestData requestData)
            : base(requestData, TestSessionMessageLogger.Instance, new InternalTestLoggerEvents(TestSessionMessageLogger.Instance))
        {

        }
    }
}
