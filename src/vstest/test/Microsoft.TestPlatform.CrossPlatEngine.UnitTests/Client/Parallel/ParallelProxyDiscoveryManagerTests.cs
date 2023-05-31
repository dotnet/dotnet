// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.Common.Telemetry;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client.Parallel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Host;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace TestPlatform.CrossPlatEngine.UnitTests.Client;

[TestClass]
public class ParallelProxyDiscoveryManagerTests
{
    private const int Timeout3Seconds = 3 * 1000;
    private readonly Queue<Mock<IProxyDiscoveryManager>> _preCreatedMockManagers;
    private readonly List<Mock<IProxyDiscoveryManager>> _usedMockManagers;
    private readonly Func<TestRuntimeProviderInfo, DiscoveryCriteria, IProxyDiscoveryManager> _createMockManager;
    private readonly Mock<ITestDiscoveryEventsHandler2> _mockEventHandler;
    private readonly List<string> _sources = new() { "1.dll", "2.dll" };
    private readonly DiscoveryCriteria _discoveryCriteriaWith2Sources;
    private readonly List<TestRuntimeProviderInfo> _runtimeProviders;
    private int _createMockManagerCalled;
    private readonly List<string> _processedSources;
    private readonly ManualResetEventSlim _discoveryCompleted;
    private readonly Mock<IRequestData> _mockRequestData;
    private readonly DiscoveryDataAggregator _dataAggregator;

    public ParallelProxyDiscoveryManagerTests()
    {
        _processedSources = new List<string>();
        _preCreatedMockManagers = new Queue<Mock<IProxyDiscoveryManager>>(
            new List<Mock<IProxyDiscoveryManager>>{
                // have at least as many of them as you have test dlls
                // they will be dequeued when we "create" a non-parallel
                // manager. The setup adds callback for handler to complete
                // the discovery.
                new Mock<IProxyDiscoveryManager>(),
                new Mock<IProxyDiscoveryManager>(),
                new Mock<IProxyDiscoveryManager>(),
                new Mock<IProxyDiscoveryManager>(),
                new Mock<IProxyDiscoveryManager>(),
            });
        _usedMockManagers = new List<Mock<IProxyDiscoveryManager>>();
        _createMockManager = (_, _2) =>
        {
            // We create the manager at the last possible
            // moment now, not when we create the parallel proxy manager class
            // so rather than creating the class here, and adding the mock setup
            // that allows the tests to complete. We instead pre-create a bunch of managers
            // and then grab and setup the ones we need, and only assert on the used ones.
            _createMockManagerCalled++;
            var manager = _preCreatedMockManagers.Dequeue();
            _usedMockManagers.Add(manager);
            return manager.Object;
        };
        _mockEventHandler = new Mock<ITestDiscoveryEventsHandler2>();
        _discoveryCriteriaWith2Sources = new DiscoveryCriteria(_sources, 100, null);
        _runtimeProviders = new List<TestRuntimeProviderInfo> {
            new TestRuntimeProviderInfo(typeof(ITestRuntimeProvider), false, "<RunSettings></RunSettings>", new List<SourceDetail>
            {
                new SourceDetail{ Source = _sources[0], Architecture = Architecture.X86, Framework = Framework.DefaultFramework },
                new SourceDetail{ Source = _sources[1], Architecture = Architecture.X86, Framework = Framework.DefaultFramework }
            })
        };

        // This event is Set by callback from _mockEventHandler in SetupDiscoveryManager
        _discoveryCompleted = new ManualResetEventSlim(false);
        _mockRequestData = new Mock<IRequestData>();
        _mockRequestData.Setup(rd => rd.MetricsCollection).Returns(new NoOpMetricsCollection());
        _mockRequestData.Setup(rd => rd.ProtocolConfig).Returns(new ProtocolConfig());
        _dataAggregator = new();
    }

    [TestMethod]
    public void CreatingAndInitializingProxyExecutionManagerDoesNothingUntilThereIsActualWorkToDo()
    {
        InvokeAndVerifyInitialize(3);
    }

    [TestMethod]
    public void CreatingAndInitializingProxyExecutionManagerDoesNothingUntilThereIsActualWorkToDoButItKeepsSkipDefaultAdaptersValueFalse()
    {
        InvokeAndVerifyInitialize(3, skipDefaultAdapters: false);
    }


    [TestMethod]
    public void CreatingAndInitializingProxyExecutionManagerDoesNothingUntilThereIsActualWorkToDoButItKeepsSkipDefaultAdaptersValueTrue()
    {
        InvokeAndVerifyInitialize(3, skipDefaultAdapters: true);
    }

    [TestMethod]
    public void AbortShouldCallAllConcurrentManagersOnce()
    {
        var parallelDiscoveryManager = new ParallelProxyDiscoveryManager(_mockRequestData.Object, _createMockManager, dataAggregator: new(), parallelLevel: 1000, _runtimeProviders);

        // Starting parallel discovery will create 2 proxy managers, which we will then promptly abort.
        parallelDiscoveryManager.DiscoverTests(_discoveryCriteriaWith2Sources, new Mock<ITestDiscoveryEventsHandler2>().Object);

        parallelDiscoveryManager.Abort();

        Assert.AreEqual(2, _usedMockManagers.Count, "Number of Concurrent Managers created should be equal to the number of sources that should run");
        _usedMockManagers.ForEach(dm => dm.Verify(m => m.Abort(), Times.Once));
    }

    [TestMethod]
    public void DiscoverTestsShouldProcessAllSources()
    {
        // Testcase filter should be passed to all parallel discovery criteria.
        _discoveryCriteriaWith2Sources.TestCaseFilter = "Name~Test";
        var parallelDiscoveryManager = SetupDiscoveryManager(_createMockManager, 2, false);

        var task = Task.Run(() => parallelDiscoveryManager.DiscoverTests(_discoveryCriteriaWith2Sources, _mockEventHandler.Object));
        var discoveryCompleted = _discoveryCompleted.Wait(Timeout3Seconds);

        if (task.IsCompleted)
        {
            // If the work is done, either there is output,
            // or an exception that we want to "receive" to
            // fail our test.
            task.GetAwaiter().GetResult();
        }
        else
        {
            // We don't want to await the result because we
            // completed or timed out on the event above.
        }

        Assert.IsTrue(discoveryCompleted, "Test discovery not completed.");
        Assert.AreEqual(_sources.Count, _processedSources.Count, "All Sources must be processed.");
        AssertMissingAndDuplicateSources(_processedSources);
    }

    [TestMethod]
    public void HandlePartialDiscoveryCompleteShouldReturnTrueIfDiscoveryWasAbortedBeforeBeingStartedWithEventHandler()
    {
        var parallelDiscoveryManager = new ParallelProxyDiscoveryManager(_mockRequestData.Object, _createMockManager, dataAggregator: new(), parallelLevel: 1, new List<TestRuntimeProviderInfo>());
        var proxyDiscovermanager = new ProxyDiscoveryManager(_mockRequestData.Object, new Mock<ITestRequestSender>().Object, new Mock<ITestRuntimeProvider>().Object);

        parallelDiscoveryManager.Abort(_mockEventHandler.Object);
        bool isPartialDiscoveryComplete = parallelDiscoveryManager.HandlePartialDiscoveryComplete(proxyDiscovermanager, 20, new List<TestCase>(), isAborted: false);

        Assert.IsTrue(isPartialDiscoveryComplete);
    }

    [TestMethod]
    public void HandlePartialDiscoveryCompleteShouldReturnTrueIfDiscoveryWasAbortedAfterBeingStartedWithEventHandler()
    {
        var discoveryManagerMock = new Mock<IProxyDiscoveryManager>();
        _preCreatedMockManagers.Enqueue(discoveryManagerMock);
        var parallelDiscoveryManager = SetupDiscoveryManager((_, _2) => discoveryManagerMock.Object, 1, true);
        var proxyDiscovermanager = new ProxyDiscoveryManager(_mockRequestData.Object, new Mock<ITestRequestSender>().Object, new Mock<ITestRuntimeProvider>().Object);

        parallelDiscoveryManager.DiscoverTests(_discoveryCriteriaWith2Sources, _mockEventHandler.Object);
        parallelDiscoveryManager.Abort(_mockEventHandler.Object);
        bool isPartialDiscoveryComplete = parallelDiscoveryManager.HandlePartialDiscoveryComplete(proxyDiscovermanager, 20, new List<TestCase>(), isAborted: false);

        Assert.IsTrue(isPartialDiscoveryComplete);
    }

    [TestMethod]
    public void HandlePartialDiscoveryCompleteShouldReturnTrueIfDiscoveryWasAbortedBeforeBeingStarted()
    {
        var parallelDiscoveryManager = new ParallelProxyDiscoveryManager(_mockRequestData.Object, _createMockManager, dataAggregator: new(), parallelLevel: 1, new List<TestRuntimeProviderInfo>());
        var proxyDiscovermanager = new ProxyDiscoveryManager(_mockRequestData.Object, new Mock<ITestRequestSender>().Object, new Mock<ITestRuntimeProvider>().Object);

        parallelDiscoveryManager.Abort();
        bool isPartialDiscoveryComplete = parallelDiscoveryManager.HandlePartialDiscoveryComplete(proxyDiscovermanager, 20, new List<TestCase>(), isAborted: false);

        Assert.IsTrue(isPartialDiscoveryComplete);
    }

    [TestMethod]
    public void HandlePartialDiscoveryCompleteShouldReturnTrueIfDiscoveryWasAbortedAfterBeingStarted()
    {
        var discoveryManagerMock = new Mock<IProxyDiscoveryManager>();
        _preCreatedMockManagers.Enqueue(discoveryManagerMock);
        var parallelDiscoveryManager = SetupDiscoveryManager((_, _2) => discoveryManagerMock.Object, 1, true);
        var proxyDiscovermanager = new ProxyDiscoveryManager(_mockRequestData.Object, new Mock<ITestRequestSender>().Object, new Mock<ITestRuntimeProvider>().Object);

        parallelDiscoveryManager.DiscoverTests(_discoveryCriteriaWith2Sources, _mockEventHandler.Object);
        parallelDiscoveryManager.Abort();
        bool isPartialDiscoveryComplete = parallelDiscoveryManager.HandlePartialDiscoveryComplete(proxyDiscovermanager, 20, new List<TestCase>(), isAborted: false);

        Assert.IsTrue(isPartialDiscoveryComplete);
    }

    [TestMethod]
    public void DiscoveryTestsShouldStopDiscoveryIfAbortionWasRequested()
    {
        // Since the hosts are aborted, total aggregated tests sent across will be -1
        var discoveryManagerMock = new Mock<IProxyDiscoveryManager>();
        _preCreatedMockManagers.Enqueue(discoveryManagerMock);
        var parallelDiscoveryManager = SetupDiscoveryManager((_, _2) => discoveryManagerMock.Object, 1, true);

        Task.Run(() =>
        {
            parallelDiscoveryManager.DiscoverTests(_discoveryCriteriaWith2Sources, _mockEventHandler.Object);
            parallelDiscoveryManager.Abort();
        });

        Assert.IsTrue(_discoveryCompleted.Wait(Timeout3Seconds), "Test discovery not completed.");
        Assert.AreEqual(1, _processedSources.Count, "One source should be processed.");
    }

    [TestMethod]
    public void DiscoveryTestsShouldStopDiscoveryIfAbortionWithEventHandlerWasRequested()
    {
        // Since the hosts are aborted, total aggregated tests sent across will be -1
        var discoveryManagerMock = new Mock<IProxyDiscoveryManager>();
        _preCreatedMockManagers.Enqueue(discoveryManagerMock);
        var parallelDiscoveryManager = SetupDiscoveryManager((_, _2) => discoveryManagerMock.Object, 1, true);

        Task.Run(() =>
        {
            parallelDiscoveryManager.DiscoverTests(_discoveryCriteriaWith2Sources, _mockEventHandler.Object);
            parallelDiscoveryManager.Abort(_mockEventHandler.Object);
        });

        Assert.IsTrue(_discoveryCompleted.Wait(Timeout3Seconds), "Test discovery not completed.");
        Assert.AreEqual(1, _processedSources.Count, "One source should be processed.");
    }

    [TestMethod]
    public void DiscoveryTestsShouldProcessAllSourceIfOneDiscoveryManagerIsStarved()
    {
        // Ensure that second discovery manager never starts. Expect 10 total tests.
        // Override DiscoveryComplete since overall aborted should be true
        var parallelDiscoveryManager = SetupDiscoveryManager(_createMockManager, 2, false);
        var secondMockManager = _preCreatedMockManagers.ToArray()[1];
        secondMockManager.Reset();
        secondMockManager.Setup(dm => dm.DiscoverTests(It.IsAny<DiscoveryCriteria>(), It.IsAny<ITestDiscoveryEventsHandler2>()))
            .Throws<NotImplementedException>();
        _mockEventHandler.Setup(mh => mh.HandleDiscoveryComplete(It.IsAny<DiscoveryCompleteEventArgs>(), null))
            .Callback<DiscoveryCompleteEventArgs, IEnumerable<TestCase>>((t, l) => _discoveryCompleted.Set());

        Task.Run(() => parallelDiscoveryManager.DiscoverTests(_discoveryCriteriaWith2Sources, _mockEventHandler.Object));

        // Processed sources should be 1 since the 2nd source is never discovered
        Assert.IsTrue(_discoveryCompleted.Wait(Timeout3Seconds), "Test discovery not completed.");
        Assert.AreEqual(1, _processedSources.Count, "All Sources must be processed.");
    }

    [TestMethod]
    public void DiscoveryTestsShouldCatchExceptionAndHandleLogMessageOfError()
    {
        // Ensure that second discovery manager never starts. Expect 10 total tests.
        // Override DiscoveryComplete since overall aborted should be true
        var parallelDiscoveryManager = SetupDiscoveryManager(_createMockManager, 2, false);
        var secondMockManager = _preCreatedMockManagers.ToArray()[1];
        secondMockManager.Reset();
        secondMockManager.Setup(dm => dm.DiscoverTests(It.IsAny<DiscoveryCriteria>(), It.IsAny<ITestDiscoveryEventsHandler2>()))
            .Throws<NotImplementedException>();
        _mockEventHandler.Setup(mh => mh.HandleDiscoveryComplete(It.IsAny<DiscoveryCompleteEventArgs>(), null))
            .Callback<DiscoveryCompleteEventArgs, IEnumerable<TestCase>>((t, l) => _discoveryCompleted.Set());

        Task.Run(() => parallelDiscoveryManager.DiscoverTests(_discoveryCriteriaWith2Sources, _mockEventHandler.Object));

        // Processed sources should be 1 since the 2nd source is never discovered
        Assert.IsTrue(_discoveryCompleted.Wait(Timeout3Seconds), "Test discovery not completed.");
        _mockEventHandler.Verify(s => s.HandleLogMessage(TestMessageLevel.Error, It.IsAny<string>()), Times.Once);
    }

    [TestMethod]
    public void DiscoveryTestsShouldCatchExceptionAndHandleRawMessageOfTestMessage()
    {
        // Ensure that second discovery manager never starts. Expect 10 total tests.
        // Override DiscoveryComplete since overall aborted should be true
        var parallelDiscoveryManager = SetupDiscoveryManager(_createMockManager, 2, false);
        var secondMockManager = _preCreatedMockManagers.ToArray()[1];
        secondMockManager.Reset();
        secondMockManager.Setup(dm => dm.DiscoverTests(It.IsAny<DiscoveryCriteria>(), It.IsAny<ITestDiscoveryEventsHandler2>()))
            .Throws<NotImplementedException>();
        _mockEventHandler.Setup(mh => mh.HandleDiscoveryComplete(It.IsAny<DiscoveryCompleteEventArgs>(), null))
            .Callback<DiscoveryCompleteEventArgs, IEnumerable<TestCase>>((t, l) => _discoveryCompleted.Set());

        Task.Run(() => parallelDiscoveryManager.DiscoverTests(_discoveryCriteriaWith2Sources, _mockEventHandler.Object));

        // Processed sources should be 1 since the 2nd source is never discovered
        Assert.IsTrue(_discoveryCompleted.Wait(Timeout3Seconds), "Test discovery not completed.");
        _mockEventHandler.Verify(s => s.HandleRawMessage(It.Is<string>(str => str.Contains(MessageType.TestMessage))));
    }

    [TestMethod]
    public void HandlePartialDiscoveryCompleteShouldCreateANewProxyDiscoveryManagerIfIsAbortedIsTrue()
    {
        var parallelDiscoveryManager = new ParallelProxyDiscoveryManager(_mockRequestData.Object, _createMockManager, dataAggregator: new(), parallelLevel: 1, _runtimeProviders);

        // Trigger discover tests, this will create a manager by calling the _createMockManager func
        // which dequeues it to _usedMockManagers.
        parallelDiscoveryManager.DiscoverTests(_discoveryCriteriaWith2Sources, _mockEventHandler.Object);
        var completedManager = _usedMockManagers[0];

        // act
        // Tell the manager that completedManager finished work, and that it should progress to next work
        parallelDiscoveryManager.HandlePartialDiscoveryComplete(completedManager.Object, 20, new List<TestCase>(), isAborted: true);

        // assert
        // We created 2 managers 1 for the original work and another one
        // when we called HandlePartialDiscoveryComplete and it moved on to the next piece of work.
        Assert.AreEqual(2, _createMockManagerCalled);
    }

    [TestMethod]
    public void DiscoveryTestsWithCompletionMarksAllSourcesAsFullyDiscovered()
    {
        _discoveryCriteriaWith2Sources.TestCaseFilter = "Name~Test";
        var parallelDiscoveryManager = SetupDiscoveryManager(_createMockManager, 2, false);

        Task.Run(() => parallelDiscoveryManager.DiscoverTests(_discoveryCriteriaWith2Sources, _mockEventHandler.Object));

        Assert.IsTrue(_discoveryCompleted.Wait(Timeout3Seconds), "Test discovery not completed.");
        Assert.AreEqual(_sources.Count, _processedSources.Count, "All Sources must be processed.");
        CollectionAssert.AreEquivalent(_sources, _dataAggregator.GetSourcesWithStatus(DiscoveryStatus.FullyDiscovered));
        Assert.AreEqual(0, _dataAggregator.GetSourcesWithStatus(DiscoveryStatus.PartiallyDiscovered).Count);
        Assert.AreEqual(0, _dataAggregator.GetSourcesWithStatus(DiscoveryStatus.NotDiscovered).Count);
    }

    private ParallelProxyDiscoveryManager SetupDiscoveryManager(Func<TestRuntimeProviderInfo, DiscoveryCriteria, IProxyDiscoveryManager> getProxyManager, int parallelLevel, bool abortDiscovery)
    {
        var parallelDiscoveryManager = new ParallelProxyDiscoveryManager(_mockRequestData.Object, getProxyManager, dataAggregator: new(), parallelLevel, _runtimeProviders);
        SetupDiscoveryTests(_processedSources, abortDiscovery);

        // Setup a complete handler for parallel discovery manager
        _mockEventHandler.Setup(mh => mh.HandleDiscoveryComplete(It.IsAny<DiscoveryCompleteEventArgs>(), null))
            .Callback<DiscoveryCompleteEventArgs, IEnumerable<TestCase>>(
                (discoveryCompleteEventArgs, lastChunk) => _discoveryCompleted.Set());

        return parallelDiscoveryManager;
    }

    private void SetupDiscoveryTests(List<string> processedSources, bool isAbort)
    {
        var syncObject = new object();
        // This setups callbacks for the handler the we pass through.
        // We pick up those managers in the _createMockManager func,
        // and return them.
        foreach (var manager in _preCreatedMockManagers.ToArray())
        {
            manager.Setup(m => m.DiscoverTests(It.IsAny<DiscoveryCriteria>(), It.IsAny<ITestDiscoveryEventsHandler2>())).
                Callback<DiscoveryCriteria, ITestDiscoveryEventsHandler2>(
                    (criteria, handler) =>
                    {
                        lock (syncObject)
                        {
                            processedSources.AddRange(criteria.Sources);
                        }

                        _dataAggregator.MarkSourcesWithStatus(criteria.Sources, DiscoveryStatus.FullyDiscovered);

                        Task.Delay(100).Wait();

                        Assert.AreEqual(_discoveryCriteriaWith2Sources.TestCaseFilter, criteria.TestCaseFilter);
                        handler.HandleDiscoveryComplete(isAbort ? new DiscoveryCompleteEventArgs(-1, isAbort) : new DiscoveryCompleteEventArgs(10, isAbort), null);
                    });
        }
    }

    private void AssertMissingAndDuplicateSources(List<string> processedSources)
    {
        foreach (var source in _sources)
        {
            bool matchFound = false;

            foreach (var processedSrc in processedSources)
            {
                if (processedSrc.Equals(source))
                {
                    if (matchFound)
                    {
                        Assert.Fail("Concurrency issue detected: Source['{0}'] got processed twice", processedSrc);
                    }

                    matchFound = true;
                }
            }

            Assert.IsTrue(matchFound, "Concurrency issue detected: Source['{0}'] did NOT get processed at all", source);
        }
    }

    private void InvokeAndVerifyInitialize(int maxParallelLevel, bool skipDefaultAdapters = false)
    {
        var parallelDiscoveryManager = new ParallelProxyDiscoveryManager(_mockRequestData.Object, _createMockManager, dataAggregator: new(), maxParallelLevel, new List<TestRuntimeProviderInfo>());

        // Action
        parallelDiscoveryManager.Initialize(skipDefaultAdapters);

        // Verify
        Assert.AreEqual(0, _usedMockManagers.Count, $"No managers are pre-created until there is work for them.");
        _usedMockManagers.ForEach(dm => dm.Verify(m => m.Initialize(skipDefaultAdapters), Times.Once));
    }
}
