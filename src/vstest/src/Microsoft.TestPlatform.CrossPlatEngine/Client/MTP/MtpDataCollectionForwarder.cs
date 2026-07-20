// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.EventHandlers;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client.MTP;

/// <summary>
/// Forwards per-test-case "started"/"ended" notifications from a Microsoft.Testing.Platform (MTP)
/// application to the out-of-process datacollector, over the same socket sub-channel that testhost
/// uses in the classic run path.
///
/// <para>
/// In the classic path testhost owns this connection: the datacollector opens a socket, testhost
/// dials in via <see cref="DataCollectionTestCaseEventSender"/> and pushes
/// <c>TestCaseStart</c>/<c>TestCaseEnd</c> events so collectors such as Blame can track which test is
/// currently running. Under MTP there is no testhost — vstest.console drives the MTP application
/// directly and is the only party that observes per-test-case state — so nobody connects to that
/// socket and the datacollector blocks for its full connection timeout (~90s) before giving up, and
/// collectors never learn which test crashed.
/// </para>
///
/// <para>
/// This class closes that gap. It reuses the very same <see cref="DataCollectionTestCaseEventSender"/>,
/// <see cref="ProxyOutOfProcDataCollectionManager"/> and <see cref="TestCaseEventsHandler"/> publisher
/// that testhost uses, but is driven by MTP node updates instead of the adapter: as the run progresses
/// it raises test-case start/end, test-result and session-end notifications through the publisher, which
/// the out-of-proc manager relays to the datacollector.
/// </para>
/// </summary>
internal sealed class MtpDataCollectionForwarder : IDisposable
{
    private readonly DataCollectionTestCaseEventSender _sender;
    private readonly object _syncObject = new();
    private readonly HashSet<Guid> _startedTests = new();

    // The classic publisher used by testhost. ProxyOutOfProcDataCollectionManager subscribes to it in
    // its constructor and forwards the events to the datacollector through _sender; we raise them by
    // calling the publisher's Send* methods as MTP node updates arrive.
    private readonly TestCaseEventsHandler _publisher;
    private readonly ProxyOutOfProcDataCollectionManager _outOfProcManager;

    private bool _connected;
    private bool _disposed;

    public MtpDataCollectionForwarder()
    {
        _sender = DataCollectionTestCaseEventSender.Create();
        _publisher = new TestCaseEventsHandler();
        _outOfProcManager = new ProxyOutOfProcDataCollectionManager(_sender, _publisher);
    }

    /// <summary>
    /// Connects to the datacollector's test-case event socket on the given port. Returns
    /// <see langword="true"/> on success. On failure the run continues without event forwarding
    /// (data collectors that need per-test-case events, e.g. Blame, will not function, but the run
    /// itself is not aborted).
    /// </summary>
    public bool Connect(int port)
    {
        try
        {
            _sender.InitializeCommunication(port);
            var timeout = EnvironmentHelper.GetConnectionTimeout();
            _connected = _sender.WaitForRequestSenderConnection(timeout * 1000);
            if (!_connected)
            {
                EqtTrace.Error("MtpDataCollectionForwarder.Connect: timed out connecting to the datacollector on port {0}.", port);
            }
        }
        catch (Exception ex)
        {
            EqtTrace.Error("MtpDataCollectionForwarder.Connect: failed to connect to the datacollector on port {0}: {1}", port, ex);
            _connected = false;
        }

        return _connected;
    }

    /// <summary>
    /// Notifies the datacollector that a test case has started. Idempotent per test id.
    /// </summary>
    public void NotifyTestCaseStart(TestCase testCase)
    {
        if (!_connected)
        {
            return;
        }

        lock (_syncObject)
        {
            if (!_startedTests.Add(testCase.Id))
            {
                return;
            }
        }

        SafeInvoke(() => _publisher.SendTestCaseStart(testCase));
    }

    /// <summary>
    /// Notifies the datacollector that a test case has ended. Ensures a matching start was sent first,
    /// so collectors that key end events off a prior start (e.g. Blame) never see an orphan end.
    /// </summary>
    public void NotifyTestCaseEnd(TestResult result)
    {
        if (!_connected)
        {
            return;
        }

        NotifyTestCaseStart(result.TestCase);
        SafeInvoke(() => _publisher.SendTestCaseEnd(result.TestCase, result.Outcome));

        // Give the out-of-proc manager the result so any attachments produced by TestCaseEnd are
        // merged into it, matching the classic path.
        SafeInvoke(() => _publisher.SendTestResult(result));
    }

    /// <summary>
    /// Notifies the datacollector that the session has ended, releasing its wait on the test-case
    /// event channel so it can finalize without hitting the connection timeout.
    /// </summary>
    public void NotifySessionEnd()
    {
        if (!_connected)
        {
            return;
        }

        SafeInvoke(() => _publisher.SendSessionEnd());
    }

    private void SafeInvoke(Action action)
    {
        try
        {
            action();
        }
        catch (Exception ex)
        {
            // A failure on the datacollector sub-channel must not take the whole run down. Stop
            // forwarding and let the run finish; the datacollector's own timeout is the backstop.
            EqtTrace.Error("MtpDataCollectionForwarder: error forwarding a test-case event, disabling forwarding: {0}", ex);
            _connected = false;
        }
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;

        try
        {
            _sender.Close();
        }
        catch (Exception ex)
        {
            EqtTrace.Warning("MtpDataCollectionForwarder.Dispose: error closing the sender: {0}", ex);
        }
    }
}
