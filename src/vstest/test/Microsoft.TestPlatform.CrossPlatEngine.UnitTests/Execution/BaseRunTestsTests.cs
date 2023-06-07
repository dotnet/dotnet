// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework.Utilities;
using Microsoft.VisualStudio.TestPlatform.Common.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Common.Telemetry;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Tracing.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Adapter;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Execution;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine.ClientProtocol;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OMTestResult = Microsoft.VisualStudio.TestPlatform.ObjectModel.TestResult;

namespace TestPlatform.CrossPlatEngine.UnitTests.Execution;

[TestClass]
public class BaseRunTestsTests
{
    private const string BaseRunTestsExecutorUri = "executor://BaseRunTestsExecutor/";
    private const string BadBaseRunTestsExecutorUri = "executor://BadBaseRunTestsExecutor/";

    private readonly TestExecutionContext _testExecutionContext;
    private readonly Mock<IInternalTestRunEventsHandler> _mockTestRunEventsHandler;
    private readonly Mock<ITestPlatformEventSource> _mockTestPlatformEventSource;
    private readonly Mock<IRequestData> _mockRequestData;
    private readonly Mock<IMetricsCollection> _mockMetricsCollection;
    private readonly Mock<IDataSerializer> _mockDataSerializer;

    private Mock<IThread> _mockThread;
    private TestableBaseRunTests _runTestsInstance;
    private TestRunChangedEventArgs? _receivedRunStatusArgs;
    private TestRunCompleteEventArgs? _receivedRunCompleteArgs;
    private ICollection<AttachmentSet>? _receivedattachments;
    private ICollection<string>? _receivedExecutorUris;
    private TestCase? _inProgressTestCase;

    public BaseRunTestsTests()
    {
        _testExecutionContext = new TestExecutionContext(
            frequencyOfRunStatsChangeEvent: 100,
            runStatsChangeEventTimeout: TimeSpan.MaxValue,
            inIsolation: false,
            keepAlive: false,
            isDataCollectionEnabled: false,
            areTestCaseLevelEventsRequired: false,
            hasTestRun: false,
            isDebug: false,
            testCaseFilter: string.Empty,
            filterOptions: null);
        _mockTestRunEventsHandler = new Mock<IInternalTestRunEventsHandler>();

        _mockTestPlatformEventSource = new Mock<ITestPlatformEventSource>();

        _mockRequestData = new Mock<IRequestData>();
        _mockMetricsCollection = new Mock<IMetricsCollection>();
        _mockThread = new Mock<IThread>();
        _mockDataSerializer = new Mock<IDataSerializer>();
        _mockRequestData.Setup(rd => rd.MetricsCollection).Returns(_mockMetricsCollection.Object);

        _runTestsInstance = new TestableBaseRunTests(
            _mockRequestData.Object,
            null,
            null,
            _testExecutionContext,
            null,
            _mockTestRunEventsHandler.Object,
            _mockTestPlatformEventSource.Object,
            null,
            new PlatformThread(),
            _mockDataSerializer.Object);

        TestPluginCacheHelper.SetupMockExtensions(new string[] { typeof(BaseRunTestsTests).Assembly.Location }, () => { });
    }

    [TestCleanup]
    public void Cleanup()
    {
        TestExecutorExtensionManager.Destroy();
        TestPluginCacheHelper.ResetExtensionsCache();
    }

    #region Constructor tests

    [TestMethod]
    public void ConstructorShouldInitializeRunContext()
    {
        var runContext = _runTestsInstance.GetRunContext;
        Assert.IsNotNull(runContext);
        Assert.IsFalse(runContext.KeepAlive);
        Assert.IsFalse(runContext.InIsolation);
        Assert.IsFalse(runContext.IsDataCollectionEnabled);
        Assert.IsFalse(runContext.IsBeingDebugged);
    }

    [TestMethod]
    public void ConstructorShouldInitializeFrameworkHandle()
    {
        var frameworkHandle = _runTestsInstance.GetFrameworkHandle;
        Assert.IsNotNull(frameworkHandle);
    }

    [TestMethod]
    public void ConstructorShouldInitializeExecutorUrisThatRanTests()
    {
        var executorUris = _runTestsInstance.GetExecutorUrisThatRanTests;
        Assert.IsNotNull(executorUris);
    }

    #endregion

    #region RunTests tests

    [TestMethod]
    public void RunTestsShouldRaiseTestRunCompleteWithAbortedAsTrueOnException()
    {
        TestRunCompleteEventArgs? receivedCompleteArgs = null;

        // Setup mocks.
        _runTestsInstance.GetExecutorUriExtensionMapCallback = (fh, rc) => throw new NotImplementedException();
        _mockTestRunEventsHandler.Setup(
                treh =>
                    treh.HandleTestRunComplete(
                        It.IsAny<TestRunCompleteEventArgs>(),
                        It.IsAny<TestRunChangedEventArgs>(),
                        It.IsAny<ICollection<AttachmentSet>>(),
                        It.IsAny<ICollection<string>>()))
            .Callback(
                (
                    TestRunCompleteEventArgs complete,
                    TestRunChangedEventArgs stats,
                    ICollection<AttachmentSet> attachments,
                    ICollection<string> executorUris) => receivedCompleteArgs = complete);

        _runTestsInstance.RunTests();

        Assert.IsNotNull(receivedCompleteArgs);
        Assert.IsTrue(receivedCompleteArgs.IsAborted);
    }

    [TestMethod]
    public void RunTestsShouldNotThrowIfExceptionIsAFileNotFoundException()
    {
        TestRunCompleteEventArgs? receivedCompleteArgs = null;

        // Setup mocks.
        _runTestsInstance.GetExecutorUriExtensionMapCallback = (fh, rc) => throw new FileNotFoundException();
        _mockTestRunEventsHandler.Setup(
                treh =>
                    treh.HandleTestRunComplete(
                        It.IsAny<TestRunCompleteEventArgs>(),
                        It.IsAny<TestRunChangedEventArgs>(),
                        It.IsAny<ICollection<AttachmentSet>>(),
                        It.IsAny<ICollection<string>>()))
            .Callback(
                (
                    TestRunCompleteEventArgs complete,
                    TestRunChangedEventArgs stats,
                    ICollection<AttachmentSet> attachments,
                    ICollection<string> executorUris) => receivedCompleteArgs = complete);

        // This should not throw.
        _runTestsInstance.RunTests();

        Assert.IsNotNull(receivedCompleteArgs);
        Assert.IsTrue(receivedCompleteArgs.IsAborted);
    }

    [TestMethod]
    public void RunTestsShouldNotThrowIfExceptionIsAnArgumentException()
    {
        TestRunCompleteEventArgs? receivedCompleteArgs = null;

        // Setup mocks.
        _runTestsInstance.GetExecutorUriExtensionMapCallback = (fh, rc) => throw new ArgumentException();
        _mockTestRunEventsHandler.Setup(
                treh =>
                    treh.HandleTestRunComplete(
                        It.IsAny<TestRunCompleteEventArgs>(),
                        It.IsAny<TestRunChangedEventArgs>(),
                        It.IsAny<ICollection<AttachmentSet>>(),
                        It.IsAny<ICollection<string>>()))
            .Callback(
                (
                    TestRunCompleteEventArgs complete,
                    TestRunChangedEventArgs stats,
                    ICollection<AttachmentSet> attachments,
                    ICollection<string> executorUris) => receivedCompleteArgs = complete);

        // This should not throw.
        _runTestsInstance.RunTests();

        Assert.IsNotNull(receivedCompleteArgs);
        Assert.IsTrue(receivedCompleteArgs.IsAborted);
    }

    [TestMethod]
    public void RunTestsShouldAbortIfExecutorUriExtensionMapIsNull()
    {
        TestRunCompleteEventArgs? receivedCompleteArgs = null;

        // Setup mocks.
        _runTestsInstance.GetExecutorUriExtensionMapCallback = (fh, rc) => null;
        _mockTestRunEventsHandler.Setup(
                treh =>
                    treh.HandleTestRunComplete(
                        It.IsAny<TestRunCompleteEventArgs>(),
                        It.IsAny<TestRunChangedEventArgs>(),
                        It.IsAny<ICollection<AttachmentSet>>(),
                        It.IsAny<ICollection<string>>()))
            .Callback(
                (
                    TestRunCompleteEventArgs complete,
                    TestRunChangedEventArgs stats,
                    ICollection<AttachmentSet> attachments,
                    ICollection<string> executorUris) => receivedCompleteArgs = complete);

        // This should not throw.
        _runTestsInstance.RunTests();

        Assert.IsNotNull(receivedCompleteArgs);
        Assert.IsTrue(receivedCompleteArgs.IsAborted);
    }

    [TestMethod]
    public void RunTestsShouldInvokeTheTestExecutorIfAdapterAssemblyIsKnown()
    {
        var assemblyLocation = typeof(BaseRunTestsTests).Assembly.Location;
        var executorUriExtensionMap = new List<Tuple<Uri, string>>
        {
            new Tuple<Uri, string>(new Uri(BaseRunTestsExecutorUri), assemblyLocation)
        };
        LazyExtension<ITestExecutor, ITestExecutorCapabilities>? receivedExecutor = null;

        // Setup mocks.
        _runTestsInstance.GetExecutorUriExtensionMapCallback = (fh, rc) => executorUriExtensionMap;
        _runTestsInstance.InvokeExecutorCallback =
            (executor, executorUriExtensionTuple, runContext, frameworkHandle) => receivedExecutor = executor;

        _runTestsInstance.RunTests();

        Assert.IsNotNull(receivedExecutor);
        Assert.AreEqual(BaseRunTestsExecutorUri, receivedExecutor.Metadata.ExtensionUri);
    }

    [TestMethod]
    public void RunTestsShouldInvokeTheTestExecutorIfAdapterAssemblyIsUnknown()
    {
        var executorUriExtensionMap = new List<Tuple<Uri, string>>
        {
            new Tuple<Uri, string>(new Uri(BaseRunTestsExecutorUri), Constants.UnspecifiedAdapterPath)
        };
        LazyExtension<ITestExecutor, ITestExecutorCapabilities>? receivedExecutor = null;

        // Setup mocks.
        _runTestsInstance.GetExecutorUriExtensionMapCallback = (fh, rc) => executorUriExtensionMap;
        _runTestsInstance.InvokeExecutorCallback =
            (executor, executorUriExtensionTuple, runContext, frameworkHandle) => receivedExecutor = executor;

        _runTestsInstance.RunTests();

        Assert.IsNotNull(receivedExecutor);
        Assert.AreEqual(BaseRunTestsExecutorUri, receivedExecutor.Metadata.ExtensionUri);
    }

    [TestMethod]
    public void RunTestsShouldInstrumentExecutionStart()
    {
        _runTestsInstance.RunTests();

        _mockTestPlatformEventSource.Verify(x => x.ExecutionStart(), Times.Once);
    }

    [TestMethod]
    public void RunTestsShouldInstrumentExecutionStop()
    {
        SetupExecutorUriMock();

        _runTestsInstance.RunTests();

        _mockTestPlatformEventSource.Verify(x => x.ExecutionStop(It.IsAny<long>()), Times.Once);
    }

    [TestMethod]
    public void RunTestsShouldInstrumentAdapterExecutionStart()
    {
        SetupExecutorUriMock();

        _runTestsInstance.RunTests();

        _mockTestPlatformEventSource.Verify(x => x.AdapterExecutionStart(It.IsAny<string>()), Times.AtLeastOnce);
    }

    [TestMethod]
    public void RunTestsShouldInstrumentAdapterExecutionStop()
    {
        SetupExecutorUriMock();

        _runTestsInstance.RunTests();

        _mockTestPlatformEventSource.Verify(x => x.AdapterExecutionStop(It.IsAny<long>()), Times.AtLeastOnce);
    }

    [TestMethod]
    public void RunTestsShouldReportAWarningIfExecutorUriIsNotDefinedInExtensionAssembly()
    {
        var assemblyLocation = typeof(BaseRunTestsTests).Assembly.Location;
        var executorUriExtensionMap = new List<Tuple<Uri, string>>
        {
            new Tuple<Uri, string>(new Uri("executor://nonexistent/"), assemblyLocation)
        };
        LazyExtension<ITestExecutor, ITestExecutorCapabilities>? receivedExecutor = null;

        // Setup mocks.
        _runTestsInstance.GetExecutorUriExtensionMapCallback = (fh, rc) => executorUriExtensionMap;
        _runTestsInstance.InvokeExecutorCallback =
            (executor, executorUriExtensionTuple, runContext, frameworkHandle) => receivedExecutor = executor;

        _runTestsInstance.RunTests();

        var expectedWarningMessageFormat =
            "Could not find test executor with URI '{0}'.  Make sure that the test executor is installed and supports .net runtime version {1}.";

        // var runtimeVersion = string.Concat(PlatformServices.Default.Runtime.RuntimeType, " ",
        //            PlatformServices.Default.Runtime.RuntimeVersion);
        var runtimeVersion = " ";

        var expectedWarningMessage = string.Format(
            CultureInfo.InvariantCulture,
            expectedWarningMessageFormat,
            "executor://nonexistent/",
            runtimeVersion);
        _mockTestRunEventsHandler.Verify(
            treh => treh.HandleLogMessage(TestMessageLevel.Warning, expectedWarningMessage), Times.Once);

        // Should not have been called.
        Assert.IsNull(receivedExecutor);
    }

    [TestMethod]
    public void RunTestsShouldNotAddExecutorUriToExecutorUriListIfNoTestsAreRun()
    {
        var assemblyLocation = typeof(BaseRunTestsTests).Assembly.Location;
        var executorUriExtensionMap = new List<Tuple<Uri, string>>
        {
            new Tuple<Uri, string>(new Uri(BaseRunTestsExecutorUri), assemblyLocation)
        };

        // Setup mocks.
        _runTestsInstance.GetExecutorUriExtensionMapCallback = (fh, rc) => executorUriExtensionMap;

        _runTestsInstance.RunTests();

        Assert.AreEqual(0, _runTestsInstance.GetExecutorUrisThatRanTests.Count);
    }

    [TestMethod]
    public void RunTestsShouldAddExecutorUriToExecutorUriListIfExecutorHasRunTests()
    {
        var assemblyLocation = typeof(BaseRunTestsTests).Assembly.Location;
        var executorUriExtensionMap = new List<Tuple<Uri, string>>
        {
            new Tuple<Uri, string>(new Uri(BaseRunTestsExecutorUri), assemblyLocation)
        };

        // Setup mocks.
        _runTestsInstance.GetExecutorUriExtensionMapCallback = (fh, rc) => executorUriExtensionMap;
        _runTestsInstance.InvokeExecutorCallback =
            (executor, executorUriExtensionTuple, runContext, frameworkHandle) =>
            {
                var testCase = new TestCase("x.y.z", new Uri("uri://dummy"), "x.dll");
                var testResult = new OMTestResult(testCase);
                _runTestsInstance.GetTestRunCache.OnNewTestResult(testResult);
            };

        _runTestsInstance.RunTests();

        var expectedUris = new string[] { BaseRunTestsExecutorUri.ToLower(CultureInfo.InvariantCulture) };
        CollectionAssert.AreEqual(expectedUris, _runTestsInstance.GetExecutorUrisThatRanTests.ToArray());
    }

    [TestMethod]
    public void RunTestsShouldReportWarningIfExecutorThrowsAnException()
    {
        var assemblyLocation = typeof(BaseRunTestsTests).Assembly.Location;
        var executorUriExtensionMap = new List<Tuple<Uri, string>>
        {
            new Tuple<Uri, string>(new Uri(BaseRunTestsExecutorUri), assemblyLocation)
        };

        // Setup mocks.
        _runTestsInstance.GetExecutorUriExtensionMapCallback = (fh, rc) => executorUriExtensionMap;
        _runTestsInstance.InvokeExecutorCallback =
            (executor, executorUriExtensionTuple, runContext, frameworkHandle) => throw new ArgumentException("Test influenced.");

        _runTestsInstance.RunTests();

        var messageFormat = "An exception occurred while invoking executor '{0}': {1}";
        var message = string.Format(CultureInfo.InvariantCulture, messageFormat, BaseRunTestsExecutorUri.ToLower(CultureInfo.InvariantCulture), "Test influenced.");
        _mockTestRunEventsHandler.Verify(
            treh => treh.HandleLogMessage(TestMessageLevel.Error, It.Is<string>(s => s.StartsWith(message))),
            Times.Once);

        // Also validate that a test run complete is called.
        _mockTestRunEventsHandler.Verify(
            treh => treh.HandleTestRunComplete(
                It.IsAny<TestRunCompleteEventArgs>(),
                It.IsAny<TestRunChangedEventArgs>(),
                It.IsAny<ICollection<AttachmentSet>>(),
                It.IsAny<ICollection<string>>()), Times.Once);
    }

    [TestMethod]
    public void RunTestsShouldNotFailOtherExecutorsRunIfOneExecutorThrowsAnException()
    {
        var assemblyLocation = typeof(BaseRunTestsTests).Assembly.Location;
        var executorUriExtensionMap = new List<Tuple<Uri, string>>
        {
            new Tuple<Uri, string>(new Uri(BadBaseRunTestsExecutorUri), assemblyLocation),
            new Tuple<Uri, string>(new Uri(BaseRunTestsExecutorUri), assemblyLocation)
        };

        // Setup mocks.
        _runTestsInstance.GetExecutorUriExtensionMapCallback = (fh, rc) => executorUriExtensionMap;
        _runTestsInstance.InvokeExecutorCallback =
            (executor, executorUriExtensionTuple, runContext, frameworkHandle) =>
            {
                if (string.Equals(BadBaseRunTestsExecutorUri, executor.Metadata.ExtensionUri))
                {
                    throw new Exception();
                }
                else
                {
                    var testCase = new TestCase("x.y.z", new Uri("uri://dummy"), "x.dll");
                    var testResult = new OMTestResult(testCase);
                    _runTestsInstance.GetTestRunCache.OnNewTestResult(testResult);
                }
            };

        _runTestsInstance.RunTests();

        var expectedUris = new string[] { BaseRunTestsExecutorUri.ToLower(CultureInfo.InvariantCulture) };
        CollectionAssert.AreEqual(expectedUris, _runTestsInstance.GetExecutorUrisThatRanTests.ToArray());
    }

    [TestMethod]
    public void RunTestsShouldIterateThroughAllExecutors()
    {
        var assemblyLocation = typeof(BaseRunTestsTests).Assembly.Location;
        var executorUriExtensionMap = new List<Tuple<Uri, string>>
        {
            new Tuple<Uri, string>(new Uri(BadBaseRunTestsExecutorUri), assemblyLocation),
            new Tuple<Uri, string>(new Uri(BaseRunTestsExecutorUri), assemblyLocation)
        };

        // Setup mocks.
        _runTestsInstance.GetExecutorUriExtensionMapCallback = (fh, rc) => executorUriExtensionMap;
        _runTestsInstance.InvokeExecutorCallback =
            (executor, executorUriExtensionTuple, runContext, frameworkHandle) =>
            {
                var testCase = new TestCase("x.y.z", new Uri("uri://dummy"), "x.dll");
                var testResult = new OMTestResult(testCase);
                _runTestsInstance.GetTestRunCache.OnNewTestResult(testResult);
            };

        _runTestsInstance.RunTests();

        var expectedUris = new string[] { BadBaseRunTestsExecutorUri.ToLower(CultureInfo.InvariantCulture), BaseRunTestsExecutorUri.ToLower(CultureInfo.InvariantCulture) };
        CollectionAssert.AreEqual(expectedUris, _runTestsInstance.GetExecutorUrisThatRanTests.ToArray());
    }

    [TestMethod]
    public void RunTestsShouldRaiseTestRunComplete()
    {
        SetUpTestRunEvents();

        // Act.
        _runTestsInstance.RunTests();

        // Test run complete assertions.
        Assert.IsNotNull(_receivedRunCompleteArgs);
        Assert.IsNull(_receivedRunCompleteArgs.Error);
        Assert.IsFalse(_receivedRunCompleteArgs.IsAborted);
        Assert.AreEqual(_runTestsInstance.GetTestRunCache.TestRunStatistics.ExecutedTests, _receivedRunCompleteArgs.TestRunStatistics!.ExecutedTests);

        // Test run changed event assertions
        Assert.IsNotNull(_receivedRunStatusArgs);
        Assert.AreEqual(_runTestsInstance.GetTestRunCache.TestRunStatistics.ExecutedTests, _receivedRunStatusArgs.TestRunStatistics!.ExecutedTests);
        Assert.IsNotNull(_receivedRunStatusArgs.NewTestResults);
        Assert.IsTrue(_receivedRunStatusArgs.NewTestResults.Any());
        Assert.IsTrue(_receivedRunStatusArgs.ActiveTests == null || !_receivedRunStatusArgs.ActiveTests.Any());

        // Attachments
        Assert.IsNotNull(_receivedattachments);

        // Executor Uris
        var expectedUris = new string[] { BadBaseRunTestsExecutorUri.ToLower(CultureInfo.InvariantCulture), BaseRunTestsExecutorUri.ToLower(CultureInfo.InvariantCulture) };
        CollectionAssert.AreEqual(expectedUris, _receivedExecutorUris!.ToArray());
    }

    [TestMethod]
    public void RunTestsShouldNotCloneTestCaseAndTestResultsObjectForNonPackageSource()
    {
        SetUpTestRunEvents();

        _runTestsInstance.RunTests();

        _mockDataSerializer.Verify(d => d.Clone(It.IsAny<TestCase>()), Times.Never);
        _mockDataSerializer.Verify(d => d.Clone(It.IsAny<OMTestResult>()), Times.Never);
    }

    [TestMethod]
    public void RunTestsShouldUpdateTestResultsTestCaseSourceWithPackageIfTestSourceIsPackage()
    {
        const string package = @"C:\Projects\UnitTestApp1\AppPackages\UnitTestApp1\UnitTestApp1_1.0.0.0_Win32_Debug_Test\UnitTestApp1_1.0.0.0_Win32_Debug.appx";
        SetUpTestRunEvents(package);

        // Act.
        _runTestsInstance.RunTests();

        // Test run changed event assertions
        Assert.IsNotNull(_receivedRunStatusArgs?.NewTestResults);
        Assert.IsTrue(_receivedRunStatusArgs.NewTestResults.Any());

        // verify TC.Source is updated with package
        foreach (var tr in _receivedRunStatusArgs.NewTestResults)
        {
            Assert.AreEqual(tr.TestCase.Source, package);
        }
    }

    [TestMethod]
    public void RunTestsShouldUpdateActiveTestCasesSourceWithPackageIfTestSourceIsPackage()
    {
        const string package = @"C:\Porjects\UnitTestApp3\Debug\UnitTestApp3\UnitTestApp3.build.appxrecipe";
        _mockDataSerializer.Setup(d => d.Clone(It.IsAny<TestCase>()))
            .Returns<TestCase>(t => JsonDataSerializer.Instance.Clone(t));
        SetUpTestRunEvents(package, setupHandleTestRunComplete: false);

        // Act.
        _runTestsInstance.RunTests();

        Assert.IsNotNull(_receivedRunStatusArgs?.ActiveTests);
        Assert.AreEqual(1, _receivedRunStatusArgs.ActiveTests.Count());

        foreach (var tc in _receivedRunStatusArgs.ActiveTests)
        {
            Assert.AreEqual(tc.Source, package);
        }
    }

    [TestMethod]
    public void RunTestsShouldCloneTheActiveTestCaseObjectsIfTestSourceIsPackage()
    {
        const string package = @"C:\Porjects\UnitTestApp3\Debug\UnitTestApp3\UnitTestApp3.build.appxrecipe";

        SetUpTestRunEvents(package, setupHandleTestRunComplete: false);

        // Act.
        _runTestsInstance.RunTests();

        Assert.IsNotNull(_receivedRunStatusArgs?.ActiveTests);
        Assert.AreEqual(1, _receivedRunStatusArgs.ActiveTests.Count());

        _mockDataSerializer.Verify(d => d.Clone(It.IsAny<TestCase>()), Times.Exactly(2));
    }

    [TestMethod]
    public void RunTestsShouldCloneTheTestResultsObjectsIfTestSourceIsPackage()
    {
        const string package = @"C:\Porjects\UnitTestApp3\Debug\UnitTestApp3\UnitTestApp3.build.appxrecipe";

        SetUpTestRunEvents(package, setupHandleTestRunComplete: false);

        // Act.
        _runTestsInstance.RunTests();

        Assert.IsNotNull(_receivedRunStatusArgs?.NewTestResults);
        Assert.AreEqual(1, _receivedRunStatusArgs.ActiveTests!.Count());

        _mockDataSerializer.Verify(d => d.Clone(It.IsAny<OMTestResult>()), Times.Exactly(2));
    }

    [TestMethod]
    public void RunTestsShouldNotifyItsImplementersOfAnyExceptionThrownByTheExecutors()
    {
        bool? isExceptionThrown = null;
        var assemblyLocation = typeof(BaseRunTestsTests).Assembly.Location;
        var executorUriExtensionMap = new List<Tuple<Uri, string>>
        {
            new Tuple<Uri, string>(new Uri(BaseRunTestsExecutorUri), assemblyLocation)
        };

        // Setup mocks.
        _runTestsInstance.GetExecutorUriExtensionMapCallback = (fh, rc) => executorUriExtensionMap;
        _runTestsInstance.InvokeExecutorCallback =
            (executor, executorUriExtensionTuple, runContext, frameworkHandle) => throw new Exception();
        _runTestsInstance.BeforeRaisingTestRunCompleteCallback = (isEx) => isExceptionThrown = isEx;

        _runTestsInstance.RunTests();

        Assert.IsTrue(isExceptionThrown.HasValue && isExceptionThrown.Value);
    }

    [TestMethod]
    public void RunTestsShouldReportLogMessagesFromExecutors()
    {
        var assemblyLocation = typeof(BaseRunTestsTests).Assembly.Location;
        var executorUriExtensionMap = new List<Tuple<Uri, string>>
        {
            new Tuple<Uri, string>(new Uri(BaseRunTestsExecutorUri), assemblyLocation)
        };
        _runTestsInstance.GetExecutorUriExtensionMapCallback = (fh, rc) => executorUriExtensionMap;
        _runTestsInstance.InvokeExecutorCallback =
            (executor, executorUriTuple, runcontext, frameworkHandle) => frameworkHandle.SendMessage(TestMessageLevel.Error, "DummyMessage");

        _runTestsInstance.RunTests();

        _mockTestRunEventsHandler.Verify(re => re.HandleLogMessage(TestMessageLevel.Error, "DummyMessage"));
    }

    [TestMethod]
    public void RunTestsShouldCreateStaThreadIfExecutionThreadApartmentStateIsSta()
    {
        SetupForExecutionThreadApartmentStateTests(PlatformApartmentState.STA);
        _runTestsInstance.RunTests();
        _mockThread.Verify(t => t.Run(It.IsAny<Action>(), PlatformApartmentState.STA, true));
    }

    [TestMethod]
    public void RunTestsShouldSendMetricsOnTestRunComplete()
    {
        TestRunCompleteEventArgs? receivedRunCompleteArgs = null;
        var mockMetricsCollector = new Mock<IMetricsCollection>();

        var dict = new Dictionary<string, object>
        {
            { "DummyMessage", "DummyValue" }
        };

        // Setup mocks.
        mockMetricsCollector.Setup(mc => mc.Metrics).Returns(dict);
        _mockRequestData.Setup(rd => rd.MetricsCollection).Returns(mockMetricsCollector.Object);

        _mockTestRunEventsHandler.Setup(
                treh =>
                    treh.HandleTestRunComplete(
                        It.IsAny<TestRunCompleteEventArgs>(),
                        It.IsAny<TestRunChangedEventArgs>(),
                        It.IsAny<ICollection<AttachmentSet>>(),
                        It.IsAny<ICollection<string>>()))
            .Callback(
                (
                    TestRunCompleteEventArgs complete,
                    TestRunChangedEventArgs stats,
                    ICollection<AttachmentSet> attachments,
                    ICollection<string> executorUris) => receivedRunCompleteArgs = complete);

        // Act.
        _runTestsInstance.RunTests();

        // Assert.
        Assert.IsNotNull(receivedRunCompleteArgs?.Metrics);
        Assert.IsTrue(receivedRunCompleteArgs.Metrics.Any());
        Assert.IsTrue(receivedRunCompleteArgs.Metrics.ContainsKey("DummyMessage"));
    }

    [TestMethod]
    public void RunTestsShouldCollectMetrics()
    {
        var mockMetricsCollector = new Mock<IMetricsCollection>();
        var dict = new Dictionary<string, object>
        {
            { "DummyMessage", "DummyValue" }
        };
        var assemblyLocation = typeof(BaseRunTestsTests).Assembly.Location;
        var executorUriExtensionMap = new List<Tuple<Uri, string>>
        {
            new Tuple<Uri, string>(new Uri(BadBaseRunTestsExecutorUri), assemblyLocation),
            new Tuple<Uri, string>(new Uri(BaseRunTestsExecutorUri), assemblyLocation)
        };

        // Setup mocks.
        _runTestsInstance.GetExecutorUriExtensionMapCallback = (fh, rc) => executorUriExtensionMap;
        _runTestsInstance.InvokeExecutorCallback =
            (executor, executorUriExtensionTuple, runContext, frameworkHandle) =>
            {
                var testCase = new TestCase("x.y.z", new Uri("uri://dummy"), "x.dll");
                var testResult = new OMTestResult(testCase);
                _runTestsInstance.GetTestRunCache.OnNewTestResult(testResult);
            };
        mockMetricsCollector.Setup(mc => mc.Metrics).Returns(dict);
        _mockRequestData.Setup(rd => rd.MetricsCollection).Returns(mockMetricsCollector.Object);

        // Act.
        _runTestsInstance.RunTests();

        // Verify.
        mockMetricsCollector.Verify(rd => rd.Add(TelemetryDataConstants.TotalTestsRun, It.IsAny<object>()), Times.Once);
        mockMetricsCollector.Verify(rd => rd.Add(TelemetryDataConstants.RunState, It.IsAny<string>()), Times.Once);
        mockMetricsCollector.Verify(rd => rd.Add(TelemetryDataConstants.NumberOfAdapterUsedToRunTests, It.IsAny<object>()), Times.Once);
        mockMetricsCollector.Verify(rd => rd.Add(TelemetryDataConstants.NumberOfAdapterDiscoveredDuringExecution, It.IsAny<object>()), Times.Once);
        mockMetricsCollector.Verify(rd => rd.Add(TelemetryDataConstants.TimeTakenByAllAdaptersInSec, It.IsAny<object>()), Times.Once);
        mockMetricsCollector.Verify(rd => rd.Add(string.Concat(TelemetryDataConstants.TimeTakenToRunTestsByAnAdapter, ".", new Uri(BadBaseRunTestsExecutorUri)), It.IsAny<object>()), Times.Once);
        mockMetricsCollector.Verify(rd => rd.Add(string.Concat(TelemetryDataConstants.TotalTestsRanByAdapter, ".", new Uri(BadBaseRunTestsExecutorUri)), It.IsAny<object>()), Times.Once);
    }

    [TestMethod]
    public void RunTestsShouldNotCreateThreadIfExecutionThreadApartmentStateIsMta()
    {
        SetupForExecutionThreadApartmentStateTests(PlatformApartmentState.MTA);
        _runTestsInstance.RunTests();

        _mockThread.Verify(t => t.Run(It.IsAny<Action>(), PlatformApartmentState.STA, true), Times.Never);
    }

    [TestMethod]
    public void RunTestsShouldRunTestsInMtaThreadWhenRunningInStaThreadFails()
    {
        SetupForExecutionThreadApartmentStateTests(PlatformApartmentState.STA);
        _mockThread.Setup(
            mt => mt.Run(It.IsAny<Action>(), PlatformApartmentState.STA, It.IsAny<bool>())).Throws<ThreadApartmentStateNotSupportedException>();
        bool isInvokeExecutorCalled = false;
        _runTestsInstance.InvokeExecutorCallback =
            (executor, executorUriTuple, runcontext, frameworkHandle) => isInvokeExecutorCalled = true;
        _runTestsInstance.RunTests();

        Assert.IsTrue(isInvokeExecutorCalled, "InvokeExecutor() should be called when STA thread creation fails.");
        _mockThread.Verify(t => t.Run(It.IsAny<Action>(), PlatformApartmentState.STA, true), Times.Once);
    }

    [TestMethod]
    public void CancelShouldCreateStaThreadIfExecutionThreadApartmentStateIsSta()
    {
        SetupForExecutionThreadApartmentStateTests(PlatformApartmentState.STA);
        _mockThread.Setup(mt => mt.Run(It.IsAny<Action>(), PlatformApartmentState.STA, It.IsAny<bool>()))
            .Callback<Action, PlatformApartmentState, bool>((action, start, waitForCompletion) =>
            {
                if (waitForCompletion)
                {
                    // Callback for RunTests().
                    _runTestsInstance.Cancel();
                }
            });

        _runTestsInstance.RunTests();
        _mockThread.Verify(
            t => t.Run(It.IsAny<Action>(), PlatformApartmentState.STA, It.IsAny<bool>()),
            Times.Exactly(2),
            "Both RunTests() and Cancel() should create STA thread.");
    }

    #endregion

    #region Private Methods

    private void SetupExecutorUriMock()
    {
        var executorUriExtensionMap = new List<Tuple<Uri, string>>
        {
            new Tuple<Uri, string>(new Uri(BaseRunTestsExecutorUri), Constants.UnspecifiedAdapterPath)
        };
        LazyExtension<ITestExecutor, ITestExecutorCapabilities>? receivedExecutor = null;

        // Setup mocks.
        _runTestsInstance.GetExecutorUriExtensionMapCallback = (fh, rc) => executorUriExtensionMap;
        _runTestsInstance.InvokeExecutorCallback =
            (executor, executorUriExtensionTuple, runContext, frameworkHandle) => receivedExecutor = executor;
    }

    private void SetupForExecutionThreadApartmentStateTests(PlatformApartmentState apartmentState)
    {
        _mockThread = new Mock<IThread>();

        _runTestsInstance = new TestableBaseRunTests(
            _mockRequestData.Object,
            null,
            $@"<RunSettings>
                  <RunConfiguration>
                     <ExecutionThreadApartmentState>{apartmentState}</ExecutionThreadApartmentState>
                   </RunConfiguration>
                </RunSettings>",
            _testExecutionContext,
            null,
            _mockTestRunEventsHandler.Object,
            _mockTestPlatformEventSource.Object,
            null,
            _mockThread.Object,
            _mockDataSerializer.Object);

        TestPluginCacheHelper.SetupMockExtensions(new string[] { typeof(BaseRunTestsTests).Assembly.Location }, () => { });
        var assemblyLocation = typeof(BaseRunTestsTests).Assembly.Location;
        var executorUriExtensionMap = new List<Tuple<Uri, string>>
        {
            new Tuple<Uri, string>(new Uri(BaseRunTestsExecutorUri), assemblyLocation)
        };
        _runTestsInstance.GetExecutorUriExtensionMapCallback = (fh, rc) => executorUriExtensionMap;
    }

    [MemberNotNull(nameof(_runTestsInstance))]
    private void SetUpTestRunEvents(string? package = null, bool setupHandleTestRunComplete = true)
    {
        if (setupHandleTestRunComplete)
        {
            SetupHandleTestRunComplete();
        }
        else
        {
            SetupHandleTestRunStatsChange();
        }

        SetupDataSerializer();

        _runTestsInstance = _runTestsInstance = new TestableBaseRunTests(
            _mockRequestData.Object,
            package,
            null,
            _testExecutionContext,
            null,
            _mockTestRunEventsHandler.Object,
            _mockTestPlatformEventSource.Object,
            null,
            new PlatformThread(),
            _mockDataSerializer.Object);

        var assemblyLocation = typeof(BaseRunTestsTests).Assembly.Location;
        var executorUriExtensionMap = new List<Tuple<Uri, string>>
        {
            new Tuple<Uri, string>(new Uri(BadBaseRunTestsExecutorUri), assemblyLocation),
            new Tuple<Uri, string>(new Uri(BaseRunTestsExecutorUri), assemblyLocation)
        };

        // Setup mocks.
        SetupExecutorCallback(executorUriExtensionMap);
    }

    private void SetupExecutorCallback(List<Tuple<Uri, string>> executorUriExtensionMap)
    {
        _runTestsInstance.GetExecutorUriExtensionMapCallback = (fh, rc) => executorUriExtensionMap;
        _runTestsInstance.InvokeExecutorCallback =
            (executor, executorUriExtensionTuple, runContext, frameworkHandle) =>
            {
                var testCase = new TestCase("x.y.z", new Uri("uri://dummy"), "x.dll");
                var testResult = new OMTestResult(testCase);
                _inProgressTestCase = new TestCase("x.y.z2", new Uri("uri://dummy"), "x.dll");

                _runTestsInstance.GetTestRunCache.OnTestStarted(_inProgressTestCase);
                _runTestsInstance.GetTestRunCache.OnNewTestResult(testResult);
            };
    }

    private void SetupDataSerializer()
    {
        _mockDataSerializer.Setup(d => d.Clone(It.IsAny<TestCase>()))
            .Returns<TestCase>(t => JsonDataSerializer.Instance.Clone(t));

        _mockDataSerializer.Setup(d => d.Clone(It.IsAny<OMTestResult>()))
            .Returns<OMTestResult>(t => JsonDataSerializer.Instance.Clone(t));
    }

    private void SetupHandleTestRunStatsChange()
    {
        _testExecutionContext.FrequencyOfRunStatsChangeEvent = 2;
        _mockTestRunEventsHandler
            .Setup(treh => treh.HandleTestRunStatsChange(It.IsAny<TestRunChangedEventArgs>()))
            .Callback((TestRunChangedEventArgs stats) => _receivedRunStatusArgs = stats);
    }

    private void SetupHandleTestRunComplete()
    {
        _mockTestRunEventsHandler.Setup(
                treh =>
                    treh.HandleTestRunComplete(
                        It.IsAny<TestRunCompleteEventArgs>(),
                        It.IsAny<TestRunChangedEventArgs>(),
                        It.IsAny<ICollection<AttachmentSet>>(),
                        It.IsAny<ICollection<string>>()))
            .Callback(
                (
                    TestRunCompleteEventArgs complete,
                    TestRunChangedEventArgs stats,
                    ICollection<AttachmentSet> attachments,
                    ICollection<string> executorUris) =>
                {
                    _receivedRunCompleteArgs = complete;
                    _receivedRunStatusArgs = stats;
                    _receivedattachments = attachments;
                    _receivedExecutorUris = executorUris;
                });
    }

    #endregion

    #region Testable Implementation

    private class TestableBaseRunTests : BaseRunTests
    {
        public TestableBaseRunTests(
            IRequestData requestData,
            string? package,
            string? runSettings,
            TestExecutionContext testExecutionContext,
            ITestCaseEventsHandler? testCaseEventsHandler,
            IInternalTestRunEventsHandler testRunEventsHandler,
            ITestPlatformEventSource testPlatformEventSource,
            ITestEventsPublisher? testEventsPublisher,
            IThread platformThread,
            IDataSerializer dataSerializer)
            : base(
                requestData,
                package,
                runSettings,
                testExecutionContext,
                testCaseEventsHandler,
                testRunEventsHandler,
                testPlatformEventSource,
                testEventsPublisher,
                platformThread,
                dataSerializer)
        {
            _testCaseEventsHandler = testCaseEventsHandler;
        }

        private readonly ITestCaseEventsHandler? _testCaseEventsHandler;

        public Action<bool>? BeforeRaisingTestRunCompleteCallback { get; set; }

        public Func<IFrameworkHandle, RunContext, IEnumerable<Tuple<Uri, string>>?>? GetExecutorUriExtensionMapCallback { get; set; }

        public Action<LazyExtension<ITestExecutor, ITestExecutorCapabilities>, Tuple<Uri, string>, RunContext, IFrameworkHandle>? InvokeExecutorCallback { get; set; }

        /// <summary>
        /// Gets the run settings.
        /// </summary>
        public string? GetRunSettings => RunSettings;

        /// <summary>
        /// Gets the test execution context.
        /// </summary>
        public TestExecutionContext GetTestExecutionContext => TestExecutionContext;

        /// <summary>
        /// Gets the test run events handler.
        /// </summary>
        public IInternalTestRunEventsHandler GetTestRunEventsHandler => TestRunEventsHandler;

        /// <summary>
        /// Gets the test run cache.
        /// </summary>
        public ITestRunCache GetTestRunCache => TestRunCache;

        public bool GetIsCancellationRequested => IsCancellationRequested;

        public RunContext GetRunContext => RunContext;

        public FrameworkHandle GetFrameworkHandle => FrameworkHandle;

        public ICollection<string> GetExecutorUrisThatRanTests => ExecutorUrisThatRanTests;

        protected override void BeforeRaisingTestRunComplete(bool exceptionsHitDuringRunTests)
        {
            BeforeRaisingTestRunCompleteCallback?.Invoke(exceptionsHitDuringRunTests);
        }

        protected override IEnumerable<Tuple<Uri, string>>? GetExecutorUriExtensionMap(IFrameworkHandle testExecutorFrameworkHandle, RunContext runContext)
        {
            return GetExecutorUriExtensionMapCallback?.Invoke(testExecutorFrameworkHandle, runContext);
        }

        protected override void InvokeExecutor(LazyExtension<ITestExecutor, ITestExecutorCapabilities> executor, Tuple<Uri, string> executorUriExtensionTuple, RunContext runContext, IFrameworkHandle frameworkHandle)
        {
            InvokeExecutorCallback?.Invoke(executor, executorUriExtensionTuple, runContext, frameworkHandle);
        }

        protected override void SendSessionEnd()
        {
            _testCaseEventsHandler?.SendSessionEnd();
        }

        protected override void SendSessionStart()
        {
            _testCaseEventsHandler?.SendSessionStart(new Dictionary<string, object?> { { "TestSources", new List<string>() { "1.dll" } } });
        }

        protected override bool ShouldAttachDebuggerToTestHost(
            LazyExtension<ITestExecutor, ITestExecutorCapabilities> executor,
            Tuple<Uri, string> executorUri,
            RunContext runContext)
        {
            return false;
        }
    }

    [ExtensionUri(BaseRunTestsExecutorUri)]
    private class TestExecutor : ITestExecutor
    {
        public void Cancel()
        {
        }

        public void RunTests(IEnumerable<string>? sources, IRunContext? runContext, IFrameworkHandle? frameworkHandle)
        {
        }

        public void RunTests(IEnumerable<TestCase>? tests, IRunContext? runContext, IFrameworkHandle? frameworkHandle)
        {
        }
    }

    [ExtensionUri(BadBaseRunTestsExecutorUri)]
    private class BadTestExecutor : ITestExecutor
    {
        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public void RunTests(IEnumerable<string>? sources, IRunContext? runContext, IFrameworkHandle? frameworkHandle)
        {
            throw new NotImplementedException();
        }

        public void RunTests(IEnumerable<TestCase>? tests, IRunContext? runContext, IFrameworkHandle? frameworkHandle)
        {
            throw new NotImplementedException();
        }
    }

    #endregion

}
