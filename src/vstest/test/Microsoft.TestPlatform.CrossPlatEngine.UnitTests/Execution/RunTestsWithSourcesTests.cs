// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestPlatform.Common;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework.Utilities;
using Microsoft.VisualStudio.TestPlatform.Common.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Adapter;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Execution;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine.ClientProtocol;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace TestPlatform.CrossPlatEngine.UnitTests.Execution;

[TestClass]
public class RunTestsWithSourcesTests
{
    private readonly TestExecutionContext _testExecutionContext;
    private readonly Mock<IInternalTestRunEventsHandler> _mockTestRunEventsHandler;
    private TestableRunTestsWithSources? _runTestsInstance;
    private readonly Mock<IRequestData> _mockRequestData;
    private readonly Mock<IMetricsCollection> _mockMetricsCollection;

    internal const string RunTestsWithSourcesTestsExecutorUri = "executor://RunTestWithSourcesDiscoverer/";

    public RunTestsWithSourcesTests()
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
            testCaseFilter: null,
            filterOptions: null);
        _mockTestRunEventsHandler = new Mock<IInternalTestRunEventsHandler>();
        _mockMetricsCollection = new Mock<IMetricsCollection>();
        _mockRequestData = new Mock<IRequestData>();
        _mockRequestData.Setup(rd => rd.MetricsCollection).Returns(_mockMetricsCollection.Object);

        TestPluginCacheHelper.SetupMockExtensions(
            [typeof(RunTestsWithSourcesTests).Assembly.Location],
            () => { });

        TestPluginCache.Instance.DiscoverTestExtensions<TestExecutorPluginInformation, ITestExecutor>(TestPlatformConstants.TestAdapterEndsWithPattern);
        TestPluginCache.Instance.DiscoverTestExtensions<TestDiscovererPluginInformation, ITestDiscoverer>(TestPlatformConstants.TestAdapterEndsWithPattern);
    }

    [TestCleanup]
    public void TestCleanup()
    {
        RunTestWithSourcesExecutor.RunTestsWithSourcesCallback = null;
        TestPluginCacheHelper.ResetExtensionsCache();
    }

    [TestMethod]
    public void BeforeRaisingTestRunCompleteShouldWarnIfNoTestsAreRun()
    {
        var adapterSourceMap = new Dictionary<string, IEnumerable<string>>
        {
            { "a", new List<string> { "a", "aa" } },
            { "b", new List<string> { "b", "ab" } }
        };

        var executorUriVsSourceList = new Dictionary<Tuple<Uri, string>, IEnumerable<string>>
        {
            { new Tuple<Uri, string>(new Uri("e://d/"), "A.dll"), new List<string> { "s1.dll " } }
        };

        _runTestsInstance = new TestableRunTestsWithSources(
            adapterSourceMap,
            null,
            _testExecutionContext,
            null,
            _mockTestRunEventsHandler.Object,
            executorUriVsSourceList,
            _mockRequestData.Object);

        _runTestsInstance.CallBeforeRaisingTestRunComplete(false);

        var messageFormat =
            "No test is available in {0}. Make sure that test discoverer & executors are registered and platform & framework version settings are appropriate and try again.";
        var message = string.Format(CultureInfo.CurrentCulture, messageFormat, "a aa b ab");
        _mockTestRunEventsHandler.Verify(treh => treh.HandleLogMessage(TestMessageLevel.Warning, message),
            Times.Once);
    }

    [TestMethod]
    public void GetExecutorUriExtensionMapShouldReturnEmptyOnInvalidSources()
    {
        var adapterSourceMap = new Dictionary<string, IEnumerable<string>>
        {
            { "a", new List<string> { "a", "aa" } }
        };

        _runTestsInstance = new TestableRunTestsWithSources(
            adapterSourceMap,
            null,
            _testExecutionContext,
            null,
            _mockTestRunEventsHandler.Object,
            _mockRequestData.Object);

        var executorUris = _runTestsInstance.CallGetExecutorUriExtensionMap(new Mock<IFrameworkHandle>().Object, new RunContext());

        Assert.IsNotNull(executorUris);
        Assert.AreEqual(0, executorUris.Count());
    }

    [TestMethod]
    public void GetExecutorUriExtensionMapShouldReturnDefaultExecutorUrisForTheDiscoverersDefined()
    {
        var assemblyLocation = typeof(RunTestsWithSourcesTests).Assembly.Location;

        var adapterSourceMap = new Dictionary<string, IEnumerable<string>>
        {
            { "a", new List<string> { "a", "aa" } },
            { assemblyLocation, new List<string> { assemblyLocation } }
        };

        _runTestsInstance = new TestableRunTestsWithSources(
            adapterSourceMap,
            null,
            _testExecutionContext,
            null,
            _mockTestRunEventsHandler.Object,
            _mockRequestData.Object);

        var executorUris = _runTestsInstance.CallGetExecutorUriExtensionMap(
            new Mock<IFrameworkHandle>().Object, new RunContext());

        Assert.IsNotNull(executorUris);
        CollectionAssert.Contains(executorUris.ToArray(),
            new Tuple<Uri, string>(new Uri("executor://RunTestWithSourcesDiscoverer"), assemblyLocation));
    }

    [TestMethod]
    public void InvokeExecutorShouldInvokeTestExecutorWithTheSources()
    {
        var adapterSourceMap = new Dictionary<string, IEnumerable<string>>
        {
            { "a", new List<string> { "a", "aa" } },
            { "b", new List<string> { "b", "ab" } }
        };

        var executorUriVsSourceList = new Dictionary<Tuple<Uri, string>, IEnumerable<string>>();
        var executorUriExtensionTuple = new Tuple<Uri, string>(new Uri("e://d/"), "A.dll");
        executorUriVsSourceList.Add(executorUriExtensionTuple, new List<string> { "s1.dll " });

        _runTestsInstance = new TestableRunTestsWithSources(
            adapterSourceMap,
            null,
            _testExecutionContext,
            null,
            _mockTestRunEventsHandler.Object,
            executorUriVsSourceList,
            _mockRequestData.Object);

        var testExecutor = new RunTestWithSourcesExecutor();
        var extension = new LazyExtension<ITestExecutor, ITestExecutorCapabilities>(testExecutor, new TestExecutorMetadata("e://d/"));
        IEnumerable<string>? receivedSources = null;
        RunTestWithSourcesExecutor.RunTestsWithSourcesCallback = (sources, rc, fh) => receivedSources = sources;

        _runTestsInstance.CallInvokeExecutor(extension, executorUriExtensionTuple, null, null);

        Assert.IsNotNull(receivedSources);
        CollectionAssert.AreEqual(new List<string> { "s1.dll " }, receivedSources.ToList());
    }

    [TestMethod]
    public void RunTestsShouldRunTestsForTheSourcesSpecified()
    {
        var assemblyLocation = typeof(RunTestsWithSourcesTests).Assembly.Location;

        var adapterSourceMap = new Dictionary<string, IEnumerable<string>>
        {
            { "a", new List<string> { "a", "aa" } },
            { assemblyLocation, new List<string> { assemblyLocation } }
        };

        _runTestsInstance = new TestableRunTestsWithSources(
            adapterSourceMap,
            null,
            _testExecutionContext,
            null,
            _mockTestRunEventsHandler.Object,
            _mockRequestData.Object);

        bool isExecutorCalled = false;
        RunTestWithSourcesExecutor.RunTestsWithSourcesCallback = (s, rc, fh) => isExecutorCalled = true;

        _runTestsInstance.RunTests();

        Assert.IsTrue(isExecutorCalled);
        _mockTestRunEventsHandler.Verify(
            treh => treh.HandleTestRunComplete(It.IsAny<TestRunCompleteEventArgs>(),
                It.IsAny<TestRunChangedEventArgs>(),
                It.IsAny<ICollection<AttachmentSet>>(),
                It.IsAny<ICollection<string>>()), Times.Once);
    }

    [TestMethod]
    public void RunTestsShouldLogWarningOnNoTestsAvailableInAssembly()
    {
        string? testCaseFilter = null;
        SetupForNoTestsAvailable(testCaseFilter, out var sourcesString);

        _runTestsInstance.RunTests();

        var expectedMessage =
            $"No test is available in {sourcesString}. Make sure that test discoverer & executors are registered and platform & framework version settings are appropriate and try again.";
        _mockTestRunEventsHandler.Verify(treh => treh.HandleLogMessage(TestMessageLevel.Warning, expectedMessage), Times.Once);
    }

    [TestMethod]
    public void RunTestsShouldLogWarningOnNoTestsAvailableInAssemblyWithTestCaseFilter()
    {
        var testCaseFilter = "Name~TestMethod1";
        SetupForNoTestsAvailable(testCaseFilter, out var sourcesString);

        _runTestsInstance.RunTests();

        var expectedMessage =
            $"No test matches the given testcase filter `{testCaseFilter}` in {sourcesString}";
        _mockTestRunEventsHandler.Verify(treh => treh.HandleLogMessage(TestMessageLevel.Warning, expectedMessage), Times.Once);
    }

    [TestMethod]
    public void RunTestsShouldLogWarningOnNoTestsAvailableInAssemblyWithLongTestCaseFilter()
    {
        var veryLengthyTestCaseFilter = "FullyQualifiedName=TestPlatform.CrossPlatEngine" +
                                        ".UnitTests.Execution.RunTestsWithSourcesTests." +
                                        "RunTestsShouldLogWarningOnNoTestsAvailableInAssemblyWithLongTestCaseFilter" +
                                        "WithVeryLengthTestCaseNameeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee";
        SetupForNoTestsAvailable(veryLengthyTestCaseFilter, out var sourcesString);

        _runTestsInstance.RunTests();

        var expectedTestCaseFilter = veryLengthyTestCaseFilter.Substring(0, 256) + "...";

        var expectedMessage =
            $"No test matches the given testcase filter `{expectedTestCaseFilter}` in {sourcesString}";
        _mockTestRunEventsHandler.Verify(treh => treh.HandleLogMessage(TestMessageLevel.Warning, expectedMessage), Times.Once);
    }

    [TestMethod]
    public void SendSessionStartShouldCallSessionStartWithCorrectTestSources()
    {
        var adapterSourceMap = new Dictionary<string, IEnumerable<string>>
        {
            { "a", new List<string> { "1.dll", "2.dll" } }
        };
        var mockTestCaseEventsHandler = new Mock<ITestCaseEventsHandler>();

        _runTestsInstance = new TestableRunTestsWithSources(
            adapterSourceMap,
            null,
            _testExecutionContext,
            mockTestCaseEventsHandler.Object,
            _mockTestRunEventsHandler.Object,
            _mockRequestData.Object);

        _runTestsInstance.CallSendSessionStart();

        mockTestCaseEventsHandler.Verify(x => x.SendSessionStart(It.Is<IDictionary<string, object?>>(
            y => y.ContainsKey("TestSources")
                 && ((IEnumerable<string>)y["TestSources"]!).Contains("1.dll")
                 && ((IEnumerable<string>)y["TestSources"]!).Contains("2.dll")
        )));
    }

    [TestMethod]
    public void SendSessionEndShouldCallSessionEnd()
    {
        var adapterSourceMap = new Dictionary<string, IEnumerable<string>>
        {
            { "a", new List<string> { "1.dll", "2.dll" } }
        };
        var mockTestCaseEventsHandler = new Mock<ITestCaseEventsHandler>();

        _runTestsInstance = new TestableRunTestsWithSources(
            adapterSourceMap,
            null,
            _testExecutionContext,
            mockTestCaseEventsHandler.Object,
            _mockTestRunEventsHandler.Object,
            _mockRequestData.Object);

        _runTestsInstance.CallSendSessionEnd();

        mockTestCaseEventsHandler.Verify(x => x.SendSessionEnd());
    }

    [MemberNotNull(nameof(_runTestsInstance))]
    private void SetupForNoTestsAvailable(string? testCaseFilter, out string sourcesString)
    {
        var testAssemblyLocation = typeof(TestCase).Assembly.Location;

        var adapterAssemblyLocation = typeof(RunTestsWithSourcesTests).Assembly.Location;

        var adapterSourceMap = new Dictionary<string, IEnumerable<string>>();

        var sources = new[] { testAssemblyLocation, "a" };
        sourcesString = string.Join(" ", sources);

        adapterSourceMap.Add(adapterAssemblyLocation, sources);

        _testExecutionContext.TestCaseFilter = testCaseFilter;

        _runTestsInstance = new TestableRunTestsWithSources(
            adapterSourceMap,
            null,
            _testExecutionContext,
            null,
            _mockTestRunEventsHandler.Object,
            _mockRequestData.Object);
    }

    #region Testable Implementations

    private class TestableRunTestsWithSources : RunTestsWithSources
    {
        public TestableRunTestsWithSources(Dictionary<string, IEnumerable<string>> adapterSourceMap, string? runSettings,
            TestExecutionContext testExecutionContext, ITestCaseEventsHandler? testCaseEventsHandler, IInternalTestRunEventsHandler testRunEventsHandler,
            IRequestData requestData)
            : base(requestData, adapterSourceMap, null, runSettings, testExecutionContext, testCaseEventsHandler, testRunEventsHandler)
        {
        }

        internal TestableRunTestsWithSources(Dictionary<string, IEnumerable<string>> adapterSourceMap, string? runSettings,
            TestExecutionContext testExecutionContext,
            ITestCaseEventsHandler? testCaseEventsHandler, IInternalTestRunEventsHandler testRunEventsHandler, Dictionary<Tuple<Uri, string>,
            IEnumerable<string>> executorUriVsSourceList, IRequestData requestData)
            : base(requestData, adapterSourceMap, null, runSettings, testExecutionContext, testCaseEventsHandler, testRunEventsHandler,
                  executorUriVsSourceList)
        {
        }

        public void CallBeforeRaisingTestRunComplete(bool exceptionsHitDuringRunTests)
        {
            BeforeRaisingTestRunComplete(exceptionsHitDuringRunTests);
        }

        public IEnumerable<Tuple<Uri, string>> CallGetExecutorUriExtensionMap(
            IFrameworkHandle testExecutorFrameworkHandle, RunContext runContext)
        {
            return GetExecutorUriExtensionMap(testExecutorFrameworkHandle, runContext);
        }

        public void CallSendSessionStart()
        {
            SendSessionStart();
        }

        public void CallSendSessionEnd()
        {
            SendSessionEnd();
        }

        public void CallInvokeExecutor(LazyExtension<ITestExecutor, ITestExecutorCapabilities> executor,
            Tuple<Uri, string> executorUriExtensionTuple, RunContext? runContext, IFrameworkHandle? frameworkHandle)
        {
            InvokeExecutor(executor, executorUriExtensionTuple, runContext, frameworkHandle);
        }
    }

    [FileExtension(".dll")]
    [FileExtension(".exe")]
    [DefaultExecutorUri(RunTestsWithSourcesTestsExecutorUri)]
    private class RunTestWithSourcesDiscoverer : ITestDiscoverer
    {
        public void DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext, IMessageLogger logger,
            ITestCaseDiscoverySink discoverySink)
        {
            throw new NotImplementedException();
        }
    }

    [ExtensionUri(RunTestsWithSourcesTestsExecutorUri)]
    internal class RunTestWithSourcesExecutor : ITestExecutor
    {
        public static Action<IEnumerable<string>?, IRunContext?, IFrameworkHandle?>? RunTestsWithSourcesCallback { get; set; }
        public static Action<IEnumerable<TestCase>?, IRunContext?, IFrameworkHandle?>? RunTestsWithTestsCallback { get; set; }

        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public void RunTests(IEnumerable<string>? sources, IRunContext? runContext, IFrameworkHandle? frameworkHandle)
        {
            RunTestsWithSourcesCallback?.Invoke(sources, runContext, frameworkHandle);
        }

        public void RunTests(IEnumerable<TestCase>? tests, IRunContext? runContext, IFrameworkHandle? frameworkHandle)
        {
            RunTestsWithTestsCallback?.Invoke(tests, runContext, frameworkHandle);
        }
    }

    #endregion
}
