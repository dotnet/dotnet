// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.Common.Telemetry;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client.Parallel;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Host;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace TestPlatform.CrossPlatEngine.UnitTests.Client;

[TestClass]
public class ParallelProxyExecutionManagerTests
{
    private static readonly int Timeout3Seconds = 3 * 1000; // In milliseconds

    private readonly List<Mock<IProxyExecutionManager>> _usedMockManagers;
    private readonly Func<TestRuntimeProviderInfo, TestRunCriteria, IProxyExecutionManager> _createMockManager;
    private readonly Mock<IInternalTestRunEventsHandler> _mockEventHandler;

    private readonly List<string> _sources;
    private readonly List<string> _processedSources;
    private readonly TestRunCriteria _testRunCriteriaWith2Sources;
    private readonly List<TestRuntimeProviderInfo> _runtimeProviders;
    private readonly List<TestCase> _testCases;
    private readonly List<TestCase> _processedTestCases;
    private readonly TestRunCriteria _testRunCriteriaWithTestsFrom3Dlls;

    private int _createMockManagerCalled;
    private readonly ManualResetEventSlim _executionCompleted;
    private readonly Queue<Mock<IProxyExecutionManager>> _preCreatedMockManagers;
    private readonly Mock<IRequestData> _mockRequestData;

    public ParallelProxyExecutionManagerTests()
    {
        _executionCompleted = new ManualResetEventSlim(false);
        _preCreatedMockManagers = new Queue<Mock<IProxyExecutionManager>>(
            new List<Mock<IProxyExecutionManager>>
            {
                new(),
                new(),
                new(),
                new(),
            });
        _usedMockManagers = new List<Mock<IProxyExecutionManager>>();
        _createMockManager = (_, _2) =>
        {
            _createMockManagerCalled++;
            var manager = _preCreatedMockManagers.Dequeue();
            _usedMockManagers.Add(manager);
            return manager.Object;
        };
        _mockEventHandler = new Mock<IInternalTestRunEventsHandler>();

        // Configure sources
        _sources = new List<string>() { "1.dll", "2.dll" };
        _processedSources = new List<string>();
        _testRunCriteriaWith2Sources = new TestRunCriteria(_sources, 100, false, string.Empty, TimeSpan.MaxValue, null, "Name~Test", new FilterOptions() { FilterRegEx = @"^[^\s\(]+" });
        _runtimeProviders = new List<TestRuntimeProviderInfo> {
            new(typeof(ITestRuntimeProvider), false, "<RunSettings></RunSettings>", new List<SourceDetail>
            {
                new() { Source = "1.dll", Architecture = Architecture.X86, Framework = Framework.DefaultFramework },
                new() { Source = "2.dll", Architecture = Architecture.X86, Framework = Framework.DefaultFramework },
                // For testcases on the bottom.
                new() { Source = "3.dll", Architecture = Architecture.X86, Framework = Framework.DefaultFramework },
            })
        };

        // Configure testcases
        _testCases = CreateTestCases();
        _processedTestCases = new List<TestCase>();
        _testRunCriteriaWithTestsFrom3Dlls = new TestRunCriteria(_testCases, 100);
        _mockRequestData = new Mock<IRequestData>();
        _mockRequestData.Setup(rd => rd.MetricsCollection).Returns(new NoOpMetricsCollection());
        _mockRequestData.Setup(rd => rd.ProtocolConfig).Returns(new ProtocolConfig());
    }

    [TestMethod]
    public void NoManagersArePreCreatedUntilThereIsWorkForThem()
    {
        InvokeAndVerifyInitialize(3);
    }

    [TestMethod]
    public void NoManagersArePreCreatedUntilThereIsWorkForThemButSkipDefaultAdaptersValueFalseIsKept()
    {
        InvokeAndVerifyInitialize(3, skipDefaultAdapters: false);
    }

    [TestMethod]
    public void NoManagersArePreCreatedUntilThereIsWorkForThemButSkipDefaultAdaptersValueTrueIsKept()
    {
        InvokeAndVerifyInitialize(3, skipDefaultAdapters: true);
    }

    [TestMethod]
    public void AbortShouldCallAllConcurrentManagersOnce()
    {
        var parallelExecutionManager = new ParallelProxyExecutionManager(_mockRequestData.Object, _createMockManager, parallelLevel: 1000, _runtimeProviders);

        // Starting parallel run will create 2 proxy managers, which we will then promptly abort.
        parallelExecutionManager.StartTestRun(_testRunCriteriaWith2Sources, new Mock<IInternalTestRunEventsHandler>().Object);
        parallelExecutionManager.Abort(It.IsAny<IInternalTestRunEventsHandler>());

        Assert.AreEqual(2, _usedMockManagers.Count, "Number of Concurrent Managers created should be equal to the amount of dlls that run");
        _usedMockManagers.ForEach(em => em.Verify(m => m.Abort(It.IsAny<IInternalTestRunEventsHandler>()), Times.Once));
    }

    [TestMethod]
    public void CancelShouldCallAllConcurrentManagersOnce()
    {
        var parallelExecutionManager = new ParallelProxyExecutionManager(_mockRequestData.Object, _createMockManager, 4, _runtimeProviders);

        // Starting parallel run will create 2 proxy managers, which we will then promptly cancel.
        parallelExecutionManager.StartTestRun(_testRunCriteriaWith2Sources, new Mock<IInternalTestRunEventsHandler>().Object);
        parallelExecutionManager.Cancel(It.IsAny<IInternalTestRunEventsHandler>());

        Assert.AreEqual(2, _usedMockManagers.Count, "Number of Concurrent Managers created should be equal to the amount of dlls that run");
        _usedMockManagers.ForEach(em => em.Verify(m => m.Cancel(It.IsAny<IInternalTestRunEventsHandler>()), Times.Once));
    }

    [TestMethod]
    public void StartTestRunShouldProcessAllSources()
    {
        var parallelExecutionManager = SetupExecutionManager(_createMockManager, 2);

        parallelExecutionManager.StartTestRun(_testRunCriteriaWith2Sources, _mockEventHandler.Object);

        Assert.IsTrue(_executionCompleted.Wait(Timeout3Seconds), "Test run not completed.");
        Assert.AreEqual(_sources.Count, _processedSources.Count, "All Sources must be processed.");
        AssertMissingAndDuplicateSources(_processedSources);
    }



    [TestMethod]
    public void StartTestRunShouldProcessAllTestCases()
    {
        var parallelExecutionManager = SetupExecutionManager(_createMockManager, 3, setupTestCases: true);

        parallelExecutionManager.StartTestRun(_testRunCriteriaWithTestsFrom3Dlls, _mockEventHandler.Object);

        Assert.IsTrue(_executionCompleted.Wait(Timeout3Seconds), "Test run not completed.");
        Assert.AreEqual(_testCases.Count, _processedTestCases.Count, "All Tests must be processed.");
        AssertMissingAndDuplicateTestCases(_testCases, _processedTestCases);
    }

    [TestMethod]
    public void StartTestRunWithSourcesShouldNotSendCompleteUntilAllSourcesAreProcessed()
    {
        var parallelExecutionManager = SetupExecutionManager(_createMockManager, 2);

        Task.Run(() => parallelExecutionManager.StartTestRun(_testRunCriteriaWith2Sources, _mockEventHandler.Object));

        Assert.IsTrue(_executionCompleted.Wait(Timeout3Seconds), "Test run not completed.");
        Assert.AreEqual(_sources.Count, _processedSources.Count, "All Sources must be processed.");
        AssertMissingAndDuplicateSources(_processedSources);
    }

    [TestMethod]
    public void HandlePartialRunCompleteShouldCreateNewProxyExecutionManagerIfDataCollectionEnabled()
    {
        var completeArgs = new TestRunCompleteEventArgs(null, isCanceled: false, isAborted: false, null, null, null, TimeSpan.Zero);
        var mockTestHostManager = new Mock<ITestRuntimeProvider>();
        var mockRequestSender = new Mock<ITestRequestSender>();
        var mockDataCollectionManager = new Mock<IProxyDataCollectionManager>();
        var parallelExecutionManager = SetupExecutionManager(_createMockManager, 2, setupTestCases: true);

        // Trigger discover tests, this will create a manager by calling the _createMockManager func
        // which dequeues it to _usedMockManagers.
        parallelExecutionManager.StartTestRun(_testRunCriteriaWith2Sources, _mockEventHandler.Object);
        var completedManager = _usedMockManagers[0];

        // act
        // Tell the manager that completedManager finished work, and that it should progress to next work
        parallelExecutionManager.HandlePartialRunComplete(completedManager.Object, completeArgs, null!, null!, null!);

        // assert
        // We created 2 managers 1 for the original work and another one
        // when we called HandlePartialDiscoveryComplete and it moved on to the next piece of work.
        Assert.AreEqual(2, _createMockManagerCalled);
    }

    [TestMethod]
    public void HandlePartialRunCompleteShouldCreateNewProxyExecutionManagerIfIsAbortedIsTrue()
    {
        var completeArgs = new TestRunCompleteEventArgs(null, true, true, null, null, null, TimeSpan.Zero);
        var mockTestHostManager = new Mock<ITestRuntimeProvider>();
        var mockRequestSender = new Mock<ITestRequestSender>();
        var parallelExecutionManager = SetupExecutionManager(_createMockManager, 2, setupTestCases: true);

        // Trigger discover tests, this will create a manager by calling the _createMockManager func
        // which dequeues it to _usedMockManagers.
        parallelExecutionManager.StartTestRun(_testRunCriteriaWith2Sources, _mockEventHandler.Object);
        var completedManager = _usedMockManagers[0];

        // act
        // Tell the manager that completedManager finished work, and that it should progress to next work
        parallelExecutionManager.HandlePartialRunComplete(completedManager.Object, completeArgs, null, null, null);

        // assert
        // We created 2 managers 1 for the original work and another one
        // when we called HandlePartialDiscoveryComplete and it moved on to the next piece of work.
        Assert.AreEqual(2, _createMockManagerCalled);
    }

    [TestMethod]
    public void StartTestRunWithTestsShouldNotSendCompleteUntilAllTestsAreProcessed()
    {
        var parallelExecutionManager = SetupExecutionManager(_createMockManager, 3, setupTestCases: true);

        var task = Task.Run(() => parallelExecutionManager.StartTestRun(_testRunCriteriaWithTestsFrom3Dlls, _mockEventHandler.Object));

        bool executionCompleted = _executionCompleted.Wait(Timeout3Seconds);

        if (task.IsCompleted)
        {
            // Receive any exception if some happened
            // Don't await if not completed, to avoid hanging the test.
            task.GetAwaiter().GetResult();
        }

        Assert.IsTrue(executionCompleted, "Test run not completed.");
        Assert.AreEqual(_testCases.Count, _processedTestCases.Count, "All Tests must be processed.");
        AssertMissingAndDuplicateTestCases(_testCases, _processedTestCases);
    }

    [TestMethod]
    public void StartTestRunShouldNotProcessAllSourcesOnExecutionCancelsForAnySource()
    {
        var executionManagerMock = new Mock<IProxyExecutionManager>();
        var parallelExecutionManager = new ParallelProxyExecutionManager(_mockRequestData.Object, (_, _2) => executionManagerMock.Object, 1, _runtimeProviders);
        _preCreatedMockManagers.Enqueue(executionManagerMock);
        SetupMockManagers(_processedSources, isCanceled: true, isAborted: false);
        SetupHandleTestRunComplete(_executionCompleted);

        Task.Run(() => parallelExecutionManager.StartTestRun(_testRunCriteriaWith2Sources, _mockEventHandler.Object));

        Assert.IsTrue(_executionCompleted.Wait(Timeout3Seconds), "Test run not completed.");
        Assert.AreEqual(1, _processedSources.Count, "Abort should stop all sources execution.");
    }

    [TestMethod]
    public void StartTestRunShouldNotProcessAllSourcesOnExecutionAborted()
    {
        var executionManagerMock = new Mock<IProxyExecutionManager>();
        var parallelExecutionManager = new ParallelProxyExecutionManager(_mockRequestData.Object, (_, _2) => executionManagerMock.Object, 1, _runtimeProviders);
        _preCreatedMockManagers.Enqueue(executionManagerMock);
        SetupMockManagers(_processedSources, isCanceled: false, isAborted: false);
        SetupHandleTestRunComplete(_executionCompleted);

        parallelExecutionManager.Abort(It.IsAny<IInternalTestRunEventsHandler>());
        Task.Run(() => parallelExecutionManager.StartTestRun(_testRunCriteriaWith2Sources, _mockEventHandler.Object));

        Assert.IsTrue(_executionCompleted.Wait(Timeout3Seconds), "Test run not completed.");
        Assert.AreEqual(1, _processedSources.Count, "Abort should stop all sources execution.");
    }

    [TestMethod]
    public void StartTestRunShouldProcessAllSourcesOnExecutionAbortsForAnySource()
    {
        var executionManagerMock = new Mock<IProxyExecutionManager>();
        var parallelExecutionManager = new ParallelProxyExecutionManager(_mockRequestData.Object, (_, _2) => executionManagerMock.Object, 1, _runtimeProviders);
        _preCreatedMockManagers.Enqueue(executionManagerMock);
        SetupMockManagers(_processedSources, isCanceled: false, isAborted: true);
        SetupHandleTestRunComplete(_executionCompleted);

        Task.Run(() => parallelExecutionManager.StartTestRun(_testRunCriteriaWith2Sources, _mockEventHandler.Object));

        Assert.IsTrue(_executionCompleted.Wait(Timeout3Seconds), "Test run not completed.");

        // Even though we start the test run for two sources, because of the current setup where
        // we initialize a proxy if no more slots are available, we end up with abort notice being
        // sent only to the running manager. This leaves the initialized manager in limbo and the
        // assert will fail because of this.
        Assert.AreEqual(1, _processedSources.Count, "Abort should stop all sources execution.");
    }

    [TestMethod]
    public void StartTestRunShouldProcessAllSourceIfOneDiscoveryManagerIsStarved()
    {
        // Ensure that second discovery manager never starts. Expect 10 total tests.
        var parallelExecutionManager = SetupExecutionManager(_createMockManager, 2);
        var mockManagers = _preCreatedMockManagers.ToArray();
        mockManagers[1].Reset();
        mockManagers[1].Setup(em => em.StartTestRun(It.IsAny<TestRunCriteria>(), It.IsAny<IInternalTestRunEventsHandler>()))
            .Throws<NotImplementedException>();

        Task.Run(() => parallelExecutionManager.StartTestRun(_testRunCriteriaWith2Sources, _mockEventHandler.Object));

        // Processed sources should be 1 since the 2nd source is never discovered
        Assert.IsTrue(_executionCompleted.Wait(Timeout3Seconds), "Test run not completed.");
        Assert.AreEqual(1, _processedSources.Count, "All Sources must be processed.");
    }

    [TestMethod]
    public void StartTestRunShouldCatchExceptionAndHandleLogMessageOfError()
    {
        var parallelExecutionManager = SetupExecutionManager(_createMockManager, 2);
        var mockManagers = _preCreatedMockManagers.ToArray();
        mockManagers[1].Reset();
        mockManagers[1].Setup(em => em.StartTestRun(It.IsAny<TestRunCriteria>(), It.IsAny<IInternalTestRunEventsHandler>()))
            .Throws<NotImplementedException>();

        Task.Run(() => parallelExecutionManager.StartTestRun(_testRunCriteriaWith2Sources, _mockEventHandler.Object));

        Assert.IsTrue(_executionCompleted.Wait(Timeout3Seconds), "Test run not completed.");
        _mockEventHandler.Verify(s => s.HandleLogMessage(TestMessageLevel.Error, It.IsAny<string>()), Times.Once);
    }

    [TestMethod]
    public void StartTestRunShouldCatchExceptionAndHandleRawMessageOfTestMessage()
    {
        var parallelExecutionManager = SetupExecutionManager(_createMockManager, 2);
        var mockManagers = _preCreatedMockManagers.ToArray();
        mockManagers[1].Reset();
        mockManagers[1].Setup(em => em.StartTestRun(It.IsAny<TestRunCriteria>(), It.IsAny<IInternalTestRunEventsHandler>()))
            .Throws<NotImplementedException>();

        Task.Run(() => parallelExecutionManager.StartTestRun(_testRunCriteriaWith2Sources, _mockEventHandler.Object));

        Assert.IsTrue(_executionCompleted.Wait(Timeout3Seconds), "Test run not completed.");
        _mockEventHandler.Verify(s => s.HandleRawMessage(It.Is<string>(str => str.Contains(MessageType.TestMessage))));
    }

    [TestMethod]
    public void StartTestRunShouldAggregateRunData()
    {
        var parallelExecutionManager = new ParallelProxyExecutionManager(_mockRequestData.Object, _createMockManager, 2, _runtimeProviders);
        var syncObject = new object();

        foreach (var manager in _preCreatedMockManagers)
        {
            manager.Setup(m => m.StartTestRun(It.IsAny<TestRunCriteria>(), It.IsAny<IInternalTestRunEventsHandler>())).
                Callback<TestRunCriteria, IInternalTestRunEventsHandler>(
                    (criteria, handler) =>
                    {
                        lock (syncObject)
                        {
                            _processedSources.AddRange(criteria.Sources!);
                        }

                        Task.Delay(100).Wait();
                        var stats = new Dictionary<TestOutcome, long>
                        {
                            { TestOutcome.Passed, 3 },
                            { TestOutcome.Failed, 2 }
                        };
                        var runAttachments = new Collection<AttachmentSet>
                        {
                            new(new Uri("hello://x/"), "Hello")
                        };
                        var executorUris = new List<string>() { "hello1" };
                        bool isCanceled = false;
                        bool isAborted = false;
                        TimeSpan timespan = TimeSpan.FromMilliseconds(100);

                        if (string.Equals(criteria.Sources?.FirstOrDefault(), "2.dll"))
                        {
                            isCanceled = true;
                            isAborted = true;
                            timespan = TimeSpan.FromMilliseconds(200);
                        }

                        var completeArgs = new TestRunCompleteEventArgs(new
                            TestRunStatistics(5, stats), isCanceled, isAborted, null, runAttachments, new Collection<InvokedDataCollector>(), timespan);
                        handler.HandleTestRunComplete(completeArgs, null, runAttachments, executorUris);
                    });
        }

        Exception? assertException = null;
        _mockEventHandler.Setup(m => m.HandleTestRunComplete(
            It.IsAny<TestRunCompleteEventArgs>(),
            It.IsAny<TestRunChangedEventArgs>(),
            It.IsAny<ICollection<AttachmentSet>>(),
            It.IsAny<ICollection<string>>())).Callback
            <TestRunCompleteEventArgs, TestRunChangedEventArgs, ICollection<AttachmentSet>, ICollection<string>>(
                (completeArgs, runChangedArgs, runAttachments, executorUris) =>
                {
                    try
                    {
                        Assert.AreEqual(TimeSpan.FromMilliseconds(200), completeArgs.ElapsedTimeInRunningTests,
                            "Time should be max of all");
                        Assert.AreEqual(2, completeArgs.AttachmentSets.Count,
                            "All Complete Arg attachments should return");
                        Assert.AreEqual(2, runAttachments.Count, "All RunContextAttachments should return");

                        Assert.IsTrue(completeArgs.IsAborted, "Aborted value must be OR of all values");
                        Assert.IsTrue(completeArgs.IsCanceled, "Canceled value must be OR of all values");

                        Assert.AreEqual(10, completeArgs.TestRunStatistics!.ExecutedTests,
                            "Stats must be aggregated properly");

                        Assert.AreEqual(6, completeArgs.TestRunStatistics.Stats![TestOutcome.Passed],
                            "Stats must be aggregated properly");
                        Assert.AreEqual(4, completeArgs.TestRunStatistics.Stats[TestOutcome.Failed],
                            "Stats must be aggregated properly");
                    }
                    catch (Exception ex)
                    {
                        assertException = ex;
                    }
                    finally
                    {
                        _executionCompleted.Set();
                    }
                });

        Task.Run(() => parallelExecutionManager.StartTestRun(_testRunCriteriaWith2Sources, _mockEventHandler.Object));

        // If you are debugging this, maybe it is good idea to set this timeout higher.
        Assert.IsTrue(_executionCompleted.Wait(Timeout3Seconds), "Test run not completed.");

        Assert.IsNull(assertException, assertException?.ToString());
        Assert.AreEqual(_sources.Count, _processedSources.Count, "All Sources must be processed.");
        AssertMissingAndDuplicateSources(_processedSources);
    }

    private ParallelProxyExecutionManager SetupExecutionManager(Func<TestRuntimeProviderInfo, TestRunCriteria, IProxyExecutionManager> proxyManagerFunc, int parallelLevel)
    {
        return SetupExecutionManager(proxyManagerFunc, parallelLevel, false);
    }

    private ParallelProxyExecutionManager SetupExecutionManager(Func<TestRuntimeProviderInfo, TestRunCriteria, IProxyExecutionManager> proxyManagerFunc, int parallelLevel, bool setupTestCases)
    {
        var parallelExecutionManager = new ParallelProxyExecutionManager(_mockRequestData.Object, proxyManagerFunc, parallelLevel, _runtimeProviders);

        if (setupTestCases)
        {
            SetupMockManagersForTestCase(_processedTestCases, _testRunCriteriaWithTestsFrom3Dlls);
        }
        else
        {
            SetupMockManagers(_processedSources);
        }

        SetupHandleTestRunComplete(_executionCompleted);
        return parallelExecutionManager;
    }

    private void SetupHandleTestRunComplete(ManualResetEventSlim completeEvent)
    {
        _mockEventHandler.Setup(mh => mh.HandleTestRunComplete(It.IsAny<TestRunCompleteEventArgs>(),
                It.IsAny<TestRunChangedEventArgs>(),
                It.IsAny<ICollection<AttachmentSet>>(),
                It.IsAny<ICollection<string>>()))
            .Callback<TestRunCompleteEventArgs, TestRunChangedEventArgs, ICollection<AttachmentSet>, ICollection<string>>(
                (testRunCompleteArgs, testRunChangedEventArgs, attachmentSets, executorUris) => completeEvent.Set());
    }

    private void AssertMissingAndDuplicateSources(List<string> processedSources)
    {
        foreach (var source in _sources)
        {
            var matchFound = false;

            foreach (var processedSrc in processedSources)
            {
                if (processedSrc.Equals(source))
                {
                    if (matchFound)
                    {
                        Assert.Fail("Concurrreny issue detected: Source['{0}'] got processed twice", processedSrc);
                    }

                    matchFound = true;
                }
            }

            Assert.IsTrue(matchFound, "Concurrency issue detected: Source['{0}'] did NOT get processed at all", source);
        }
    }

    private static TestRunCompleteEventArgs CreateTestRunCompleteArgs(bool isCanceled = false, bool isAborted = false)
    {
        return new TestRunCompleteEventArgs(
            new TestRunStatistics(new Dictionary<TestOutcome, long>()),
            isCanceled,
            isAborted,
            null,
            null,
            null,
            TimeSpan.FromMilliseconds(1));
    }

    private static void AssertMissingAndDuplicateTestCases(List<TestCase> tests, List<TestCase> processedTestCases)
    {
        foreach (var test in tests)
        {
            bool matchFound = false;

            foreach (var processedTest in processedTestCases)
            {
                if (processedTest.FullyQualifiedName.Equals(test.FullyQualifiedName))
                {
                    if (matchFound)
                        Assert.Fail("Concurrency issue detected: Test['{0}'] got processed twice", test.FullyQualifiedName);
                    matchFound = true;
                }
            }

            Assert.IsTrue(matchFound, "Concurrency issue detected: Test['{0}'] did NOT get processed at all",
                test.FullyQualifiedName);
        }
    }

    private void SetupMockManagersForTestCase(List<TestCase> processedTestCases, TestRunCriteria testRunCriteria)
    {
        var syncObject = new object();
        foreach (var manager in _preCreatedMockManagers)
        {
            manager.Setup(m => m.StartTestRun(It.IsAny<TestRunCriteria>(), It.IsAny<IInternalTestRunEventsHandler>())).
                Callback<TestRunCriteria, IInternalTestRunEventsHandler>(
                    (criteria, handler) =>
                    {
                        lock (syncObject)
                        {
                            processedTestCases.AddRange(criteria.Tests!);
                        }

                        Task.Delay(100).Wait();

                        // Duplicated testRunCriteria should match the actual one.
                        Assert.AreEqual(testRunCriteria, criteria, "Mismatch in testRunCriteria");
                        handler.HandleTestRunComplete(CreateTestRunCompleteArgs(), null, null, null);
                    });
        }
    }

    private static List<TestCase> CreateTestCases()
    {
        TestCase tc1 = new("dll1.class1.test1", new Uri("hello://x/"), "1.dll");
        TestCase tc21 = new("dll2.class21.test21", new Uri("hello://x/"), "2.dll");
        TestCase tc22 = new("dll2.class21.test22", new Uri("hello://x/"), "2.dll");
        TestCase tc31 = new("dll3.class31.test31", new Uri("hello://x/"), "3.dll");
        TestCase tc32 = new("dll3.class31.test32", new Uri("hello://x/"), "3.dll");

        var tests = new List<TestCase>() { tc1, tc21, tc22, tc31, tc32 };
        return tests;
    }

    private void SetupMockManagers(List<string> processedSources, bool isCanceled = false, bool isAborted = false)
    {
        var syncObject = new object();
        foreach (var manager in _preCreatedMockManagers)
        {
            manager.Setup(m => m.StartTestRun(It.IsAny<TestRunCriteria>(), It.IsAny<IInternalTestRunEventsHandler>())).
                Callback<TestRunCriteria, IInternalTestRunEventsHandler>(
                    (criteria, handler) =>
                    {
                        lock (syncObject)
                        {
                            processedSources.AddRange(criteria.Sources!);
                        }
                        Task.Delay(100).Wait();

                        handler.HandleTestRunComplete(CreateTestRunCompleteArgs(isCanceled, isAborted), null, null, null);
                    });
        }
    }

    private void InvokeAndVerifyInitialize(int parallelLevel, bool skipDefaultAdapters = false)
    {
        var parallelExecutionManager = new ParallelProxyExecutionManager(_mockRequestData.Object, _createMockManager, parallelLevel, _runtimeProviders);

        parallelExecutionManager.Initialize(skipDefaultAdapters);

        Assert.AreEqual(0, _usedMockManagers.Count, $"No concurrent managers should be pre-created, until there is work for them");
        _usedMockManagers.ForEach(em => em.Verify(m => m.Initialize(skipDefaultAdapters), Times.Once));
    }
}
