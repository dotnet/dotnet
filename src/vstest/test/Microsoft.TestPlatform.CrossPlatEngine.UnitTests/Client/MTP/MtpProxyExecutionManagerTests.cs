// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Testing.Platform.ServerMode.Client;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client.MTP;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.UnitTests.Client.MTP;

/// <summary>
/// Tests that drive <see cref="MtpProxyExecutionManager"/> against a fake MTP server.
/// </summary>
/// <remarks>
/// Not parallelized: these tests swap the process-wide <see cref="MtpServerClientFactory.Launch"/>
/// seam, so running them alongside another class that does the same would let one class's fake leak
/// into the other's run.
/// </remarks>
[TestClass]
[DoNotParallelize]
public class MtpProxyExecutionManagerTests
{
    private const string Source = @"C:\tests\MtpApp.dll";

    private Func<string, MtpServerClientOptions, IMtpServerClient>? _originalLaunch;
    private FakeMtpServerClient _client = null!;
    private Mock<IInternalTestRunEventsHandler> _eventHandler = null!;

    [TestInitialize]
    public void Initialize()
    {
        _originalLaunch = MtpServerClientFactory.Launch;
        _client = new FakeMtpServerClient();
        MtpServerClientFactory.Launch = (_, _) => _client;
        _eventHandler = new Mock<IInternalTestRunEventsHandler>();
    }

    [TestCleanup]
    public void Cleanup()
        => MtpServerClientFactory.Launch = _originalLaunch!;

    private static TestCase TestCaseWithUid(string uid)
    {
        var testCase = new TestCase("My.Tests.MyTest", new Uri(MtpTestNodeConverter.DefaultExecutorUri), Source);
        testCase.SetPropertyValue(MtpTestNodeConverter.MtpUidProperty, uid);
        return testCase;
    }

    private static TestCase TestCaseWithoutUid()
        => new("My.Tests.MyTest", new Uri(MtpTestNodeConverter.DefaultExecutorUri), Source);

    private static TestRunCriteria CriteriaFor(params TestCase[] tests)
        => new(tests, 1);

    /// <summary>
    /// The server matches a run filter on node uid alone, so the uid stored at discovery is what
    /// must be sent - not the display name or the fully qualified name.
    /// </summary>
    [TestMethod]
    public void StartTestRunSendsTheMtpNodeUidAsTheRunFilter()
    {
        using var manager = new MtpProxyExecutionManager();
        manager.StartTestRun(CriteriaFor(TestCaseWithUid("node-uid-1")), _eventHandler.Object);

        Assert.IsNotNull(_client.RunFilterUids);
        Assert.AreEqual("node-uid-1", _client.RunFilterUids.Single());
    }

    /// <summary>
    /// A TestCase with no MTP uid cannot be addressed: the server would match nothing and the run
    /// would report success having executed zero of the selected tests. The manager must surface
    /// that as an error instead of silently running nothing.
    /// </summary>
    [TestMethod]
    public void StartTestRunFailsLoudlyWhenATestCarriesNoMtpUid()
    {
        using var manager = new MtpProxyExecutionManager();
        manager.StartTestRun(CriteriaFor(TestCaseWithoutUid()), _eventHandler.Object);

        Assert.IsNull(_client.RunFilterUids, "No run may be requested when the selection cannot be expressed.");
        _eventHandler.Verify(
            h => h.HandleLogMessage(ObjectModel.Logging.TestMessageLevel.Error, It.IsAny<string>()),
            Times.AtLeastOnce);
    }

    /// <summary>
    /// The whole source is aborted rather than silently running the addressable subset: reporting a
    /// partial run as if it were the run the user asked for is the same class of bug this fix exists
    /// to remove.
    /// </summary>
    [TestMethod]
    public void StartTestRunFailsLoudlyWhenOnlySomeTestsCarryAnMtpUid()
    {
        using var manager = new MtpProxyExecutionManager();
        manager.StartTestRun(CriteriaFor(TestCaseWithUid("node-uid-1"), TestCaseWithoutUid()), _eventHandler.Object);

        Assert.IsNull(
            _client.RunFilterUids,
            "A selection that cannot be fully expressed must not be partially run.");
        _eventHandler.Verify(
            h => h.HandleLogMessage(ObjectModel.Logging.TestMessageLevel.Error, It.IsAny<string>()),
            Times.AtLeastOnce);
    }

    [TestMethod]
    public void StartTestRunRunsEveryTestWhenNoSpecificTestsAreSelected()
    {
        using var manager = new MtpProxyExecutionManager();
        manager.StartTestRun(new TestRunCriteria([Source], 1), _eventHandler.Object);

        Assert.IsNull(_client.RunFilterUids, "An unfiltered run must not send a uid filter at all.");
        Assert.IsTrue(_client.ExitCalled);
    }

    [TestMethod]
    public void StartTestRunAsksTheServerToExitAndDisposesTheClient()
    {
        using var manager = new MtpProxyExecutionManager();
        manager.StartTestRun(CriteriaFor(TestCaseWithUid("node-uid-1")), _eventHandler.Object);

        Assert.IsTrue(_client.ExitCalled);
        Assert.IsTrue(_client.Disposed);
    }

    /// <summary>
    /// Exit runs in a finally block, so a run that fails part-way through still shuts the test
    /// application down rather than leaking the process.
    /// </summary>
    [TestMethod]
    public void StartTestRunExitsWhenTheRunFails()
    {
        _client.ThrowFromRequest = new InvalidOperationException("server blew up");

        using var manager = new MtpProxyExecutionManager();
        manager.StartTestRun(CriteriaFor(TestCaseWithUid("node-uid-1")), _eventHandler.Object);

        Assert.IsTrue(_client.ExitCalled);
        Assert.IsTrue(_client.Disposed);
    }

    /// <summary>
    /// Cancelling a run cancels the token the in-flight request is riding on. Exit must not be tied
    /// to that token, or a cancelled run would skip the shutdown handshake entirely.
    /// </summary>
    [TestMethod]
    public void StartTestRunStillExitsWhenTheRunIsCancelled()
    {
        _client.ThrowFromRequest = new OperationCanceledException();

        using var manager = new MtpProxyExecutionManager();
        manager.StartTestRun(CriteriaFor(TestCaseWithUid("node-uid-1")), _eventHandler.Object);

        Assert.IsTrue(_client.ExitCalled, "A cancelled run must still shut the test application down.");
        Assert.IsFalse(
            _client.ExitToken.IsCancellationRequested,
            "Exit must not be driven by the cancelled run token, or it would be skipped.");
        Assert.IsTrue(_client.Disposed, "The launched test application must never outlive a cancelled run.");
    }
}
