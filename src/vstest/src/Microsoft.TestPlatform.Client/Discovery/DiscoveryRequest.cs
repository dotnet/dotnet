// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework.Utilities;
using Microsoft.VisualStudio.TestPlatform.Common.Telemetry;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.Utilities;

namespace Microsoft.VisualStudio.TestPlatform.Client.Discovery;

/// <summary>
/// The discovery request.
/// </summary>
public sealed class DiscoveryRequest : IDiscoveryRequest, ITestDiscoveryEventsHandler2
{
    private readonly IDataSerializer _dataSerializer;

    /// <summary>
    /// Initializes a new instance of the <see cref="DiscoveryRequest"/> class.
    /// </summary>
    /// <param name="requestData">The Request Data instance providing services and data for discovery</param>
    /// <param name="criteria">Discovery criterion.</param>
    /// <param name="discoveryManager">Discovery manager instance.</param>
    internal DiscoveryRequest(IRequestData requestData, DiscoveryCriteria criteria, IProxyDiscoveryManager discoveryManager, ITestLoggerManager loggerManager)
        : this(requestData, criteria, discoveryManager, loggerManager, JsonDataSerializer.Instance)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DiscoveryRequest"/> class.
    /// </summary>
    /// <param name="requestData">The Request Data instance providing services and data for discovery</param>
    /// <param name="criteria">Discovery criterion.</param>
    /// <param name="discoveryManager">Discovery manager instance.</param>
    /// <param name="dataSerializer">Data Serializer</param>
    internal DiscoveryRequest(
        IRequestData requestData,
        DiscoveryCriteria criteria,
        IProxyDiscoveryManager discoveryManager,
        ITestLoggerManager loggerManager,
        IDataSerializer dataSerializer)
    {
        RequestData = requestData;
        DiscoveryCriteria = criteria;
        DiscoveryManager = discoveryManager;
        LoggerManager = loggerManager;
        _dataSerializer = dataSerializer;
    }

    /// <summary>
    /// Start the discovery request
    /// </summary>
    public void DiscoverAsync()
    {
        EqtTrace.Verbose("DiscoveryRequest.DiscoverAsync: Starting.");

        lock (_syncObject)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(DiscoveryRequest));
            }

            // Reset the discovery completion event
            _discoveryCompleted.Reset();

            DiscoveryInProgress = true;
            try
            {
                _discoveryStartTime = DateTime.UtcNow;

                // Collecting Data Point Number of sources sent for discovery
                RequestData.MetricsCollection.Add(TelemetryDataConstants.NumberOfSourcesSentForDiscovery, DiscoveryCriteria.Sources.Count());

                // Invoke OnDiscoveryStart event
                var discoveryStartEvent = new DiscoveryStartEventArgs(DiscoveryCriteria);
                LoggerManager.HandleDiscoveryStart(discoveryStartEvent);
                OnDiscoveryStart.SafeInvoke(this, discoveryStartEvent, "DiscoveryRequest.DiscoveryStart");

                DiscoveryManager.DiscoverTests(DiscoveryCriteria, this);
            }
            catch
            {
                DiscoveryInProgress = false;
                throw;
            }
        }

        EqtTrace.Info("DiscoveryRequest.DiscoverAsync: Started.");
    }

    /// <summary>
    /// Aborts the test discovery.
    /// </summary>
    public void Abort()
    {
        EqtTrace.Verbose("DiscoveryRequest.Abort: Aborting.");

        lock (_syncObject)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(DiscoveryRequest));
            }

            if (DiscoveryInProgress)
            {
                // TODO: COMPAT: This should not check the default protocol config, that is vstest.console maximum protocol version, not the
                // version we negotiated with testhost. Instead this should be handled by the proxy discovery manager, based on the protocol
                // version it has.
                // If testhost has old version, we should use old cancel logic
                // to be consistent and not create regression issues
                if (Constants.DefaultProtocolConfig.Version < Constants.MinimumProtocolVersionWithCancelDiscoveryEventHandlerSupport)
                {
                    DiscoveryManager.Abort();
                }
                else
                {
                    DiscoveryManager.Abort(this);
                }
            }
            else
            {
                EqtTrace.Info("DiscoveryRequest.Abort: No operation to abort.");
                return;
            }
        }

        EqtTrace.Info("DiscoveryRequest.Abort: Aborted.");
    }

    /// <summary>
    /// Wait for discovery completion
    /// </summary>
    /// <param name="timeout"> The timeout. </param>
    bool IRequest.WaitForCompletion(int timeout)
    {
        EqtTrace.Verbose("DiscoveryRequest.WaitForCompletion: Waiting with timeout {0}.", timeout);

        if (_isDisposed)
        {
            throw new ObjectDisposedException("DiscoveryRequest");
        }

        // This method is not synchronized as it can lead to dead-lock
        // (the discoveryCompletionEvent cannot be raised unless that lock is released)
        return _discoveryCompleted == null || _discoveryCompleted.WaitOne(timeout);
    }

    /// <summary>
    /// Raised when the test discovery starts.
    /// </summary>
    public event EventHandler<DiscoveryStartEventArgs>? OnDiscoveryStart;

    /// <summary>
    /// Raised when the test discovery completes.
    /// </summary>
    public event EventHandler<DiscoveryCompleteEventArgs>? OnDiscoveryComplete;

    /// <summary>
    /// Raised when the message is received.
    /// </summary>
    /// <remarks>TestRunMessageEventArgs should be renamed to more generic</remarks>
    public event EventHandler<TestRunMessageEventArgs>? OnDiscoveryMessage;

    /// <summary>
    /// Raised when new tests are discovered in this discovery request.
    /// </summary>
    public event EventHandler<DiscoveredTestsEventArgs>? OnDiscoveredTests;

    /// <summary>
    ///  Raised when a discovery event related message is received from host
    ///  This is required if one wants to re-direct the message over the process boundary without any processing overhead
    ///  All the discovery events should come as raw messages as well as proper serialized events like OnDiscoveredTests
    /// </summary>
    public event EventHandler<string>? OnRawMessageReceived;

    /// <summary>
    /// Specifies the discovery criterion
    /// </summary>
    public DiscoveryCriteria DiscoveryCriteria
    {
        get;
        private set;
    }

    /// <summary>
    /// Get the status for the discovery
    /// Returns true if discovery is in progress
    /// </summary>
    internal bool DiscoveryInProgress { get; private set; }

    /// <summary>
    /// Parent discovery manager
    /// </summary>
    internal IProxyDiscoveryManager DiscoveryManager { get; }

    /// <summary>
    /// Logger manager.
    /// </summary>
    internal ITestLoggerManager LoggerManager { get; }

    #region ITestDiscoveryEventsHandler2 Methods

    /// <inheritdoc/>
    public void HandleDiscoveryComplete(DiscoveryCompleteEventArgs discoveryCompleteEventArgs, IEnumerable<TestCase>? lastChunk)
    {
        EqtTrace.Verbose("DiscoveryRequest.HandleDiscoveryComplete: Begin processing discovery complete notification. Aborted: {0}, TotalTests: {1}", discoveryCompleteEventArgs.IsAborted, discoveryCompleteEventArgs.TotalCount);

        lock (_syncObject)
        {
            if (_isDisposed)
            {
                EqtTrace.Warning("DiscoveryRequest.HandleDiscoveryComplete: Ignoring as the object is disposed.");
                return;
            }

            // If discovery event is already raised, ignore current one.
            if (_discoveryCompleted.WaitOne(0))
            {
                EqtTrace.Verbose("DiscoveryRequest.HandleDiscoveryComplete: Ignoring duplicate DiscoveryComplete.");
                return;
            }

            // Close the discovery session and terminate any test host processes. This operation should never
            // throw.
            DiscoveryManager?.Close();

            try
            {
                // Raise onDiscoveredTests event if there are some tests in the last chunk.
                // (We don't want to send the tests in the discovery complete event so that programming on top of
                // RS client is easier i.e. user does not have to listen on discovery complete event.)
                if (lastChunk != null && lastChunk.Any())
                {
                    var discoveredTestsEvent = new DiscoveredTestsEventArgs(lastChunk);
                    LoggerManager.HandleDiscoveredTests(discoveredTestsEvent);
                    OnDiscoveredTests.SafeInvoke(this, discoveredTestsEvent, "DiscoveryRequest.HandleDiscoveryComplete");
                }

                // Add extensions discovered by vstest.console.
                //
                // TODO(copoiena): Writing telemetry twice is less than ideal.
                // We first write telemetry data in the _requestData variable in the ParallelRunEventsHandler
                // and then we write again here. We should refactor this code and write only once.
                discoveryCompleteEventArgs.DiscoveredExtensions = TestExtensions.CreateMergedDictionary(
                    discoveryCompleteEventArgs.DiscoveredExtensions,
                    TestPluginCache.Instance.TestExtensions?.GetCachedExtensions());

                if (RequestData.IsTelemetryOptedIn)
                {
                    TestExtensions.AddExtensionTelemetry(
                        discoveryCompleteEventArgs.Metrics!,
                        discoveryCompleteEventArgs.DiscoveredExtensions);
                }

                LoggerManager.HandleDiscoveryComplete(discoveryCompleteEventArgs);
                OnDiscoveryComplete.SafeInvoke(this, discoveryCompleteEventArgs, "DiscoveryRequest.HandleDiscoveryComplete");
            }
            finally
            {
                // Notify the waiting handle that discovery is complete
                if (_discoveryCompleted != null)
                {
                    _discoveryCompleted.Set();
                    EqtTrace.Verbose("DiscoveryRequest.HandleDiscoveryComplete: Notified the discovery complete event.");
                }
                else
                {
                    EqtTrace.Warning("DiscoveryRequest.HandleDiscoveryComplete: Discovery request was disposed.");
                }

                DiscoveryInProgress = false;
                var discoveryFinalTimeTaken = DateTime.UtcNow - _discoveryStartTime;

                // Fill in the Metrics From Test Host Process
                var metrics = discoveryCompleteEventArgs.Metrics;
                if (metrics != null && metrics.Count != 0)
                {
                    foreach (var metric in metrics)
                    {
                        RequestData.MetricsCollection.Add(metric.Key, metric.Value);
                    }
                }

                // Collecting Total Time Taken
                RequestData.MetricsCollection.Add(
                    TelemetryDataConstants.TimeTakenInSecForDiscovery, discoveryFinalTimeTaken.TotalSeconds);
            }
        }

        EqtTrace.Info("DiscoveryRequest.HandleDiscoveryComplete: Finished processing discovery complete notification.");
    }

    /// <inheritdoc/>
    public void HandleDiscoveredTests(IEnumerable<TestCase>? discoveredTestCases)
    {
        EqtTrace.Verbose("DiscoveryRequest.HandleDiscoveredTests: Starting.");

        lock (_syncObject)
        {
            if (_isDisposed)
            {
                EqtTrace.Warning("DiscoveryRequest.HandleDiscoveredTests: Ignoring as the object is disposed.");
                return;
            }

            var discoveredTestsEvent = new DiscoveredTestsEventArgs(discoveredTestCases);
            LoggerManager.HandleDiscoveredTests(discoveredTestsEvent);
            OnDiscoveredTests.SafeInvoke(this, discoveredTestsEvent, "DiscoveryRequest.OnDiscoveredTests");
        }

        EqtTrace.Info("DiscoveryRequest.HandleDiscoveredTests: Completed.");
    }

    /// <summary>
    /// Dispatch TestRunMessage event to listeners.
    /// </summary>
    /// <param name="level">Output level of the message being sent.</param>
    /// <param name="message">Actual contents of the message</param>
    public void HandleLogMessage(TestMessageLevel level, string? message)
    {
        EqtTrace.Verbose("DiscoveryRequest.HandleLogMessage: Starting.");

        lock (_syncObject)
        {
            if (_isDisposed)
            {
                EqtTrace.Warning("DiscoveryRequest.HandleLogMessage: Ignoring as the object is disposed.");
                return;
            }

            var testRunMessageEvent = new TestRunMessageEventArgs(level, message!);
            LoggerManager.HandleDiscoveryMessage(testRunMessageEvent);
            OnDiscoveryMessage.SafeInvoke(this, testRunMessageEvent, "DiscoveryRequest.OnTestMessageRecieved");
        }

        EqtTrace.Info("DiscoveryRequest.HandleLogMessage: Completed.");
    }

    /// <summary>
    /// Handle Raw message directly from the host
    /// </summary>
    /// <param name="rawMessage">Raw message.</param>
    public void HandleRawMessage(string rawMessage)
    {
        // Note: Deserialize rawMessage only if required.

        var message = LoggerManager.LoggersInitialized || RequestData.IsTelemetryOptedIn
            ? _dataSerializer.DeserializeMessage(rawMessage)
            : null;

        if (MessageType.DiscoveryComplete.Equals(message?.MessageType))
        {
            var discoveryCompletePayload = _dataSerializer.DeserializePayload<DiscoveryCompletePayload>(message);
            rawMessage = UpdateRawMessageWithTelemetryInfo(discoveryCompletePayload, message) ?? rawMessage;
            HandleLoggerManagerDiscoveryComplete(discoveryCompletePayload);
        }

        OnRawMessageReceived?.SafeInvoke(this, rawMessage, "DiscoveryRequest.RawMessageReceived");
    }

    /// <summary>
    /// Handles LoggerManager's DiscoveryComplete.
    /// </summary>
    /// <param name="discoveryCompletePayload">Discovery complete payload.</param>
    private void HandleLoggerManagerDiscoveryComplete(DiscoveryCompletePayload? discoveryCompletePayload)
    {
        if (LoggerManager.LoggersInitialized && discoveryCompletePayload != null)
        {
            // Send last chunk to logger manager.
            if (discoveryCompletePayload.LastDiscoveredTests != null)
            {
                var discoveredTestsEventArgs = new DiscoveredTestsEventArgs(discoveryCompletePayload.LastDiscoveredTests);
                LoggerManager.HandleDiscoveredTests(discoveredTestsEventArgs);
            }

            // Send discovery complete to logger manager.
            var discoveryCompleteEventArgs = new DiscoveryCompleteEventArgs(discoveryCompletePayload.TotalTests, discoveryCompletePayload.IsAborted);
            discoveryCompleteEventArgs.Metrics = discoveryCompletePayload.Metrics;
            LoggerManager.HandleDiscoveryComplete(discoveryCompleteEventArgs);
        }
    }

    /// <summary>
    /// Update raw message with telemetry info.
    /// </summary>
    /// <param name="discoveryCompletePayload">Discovery complete payload.</param>
    /// <param name="message">Message.</param>
    /// <returns>Updated rawMessage.</returns>
    private string? UpdateRawMessageWithTelemetryInfo(DiscoveryCompletePayload? discoveryCompletePayload, Message? message)
    {
        var rawMessage = default(string);

        if (!RequestData.IsTelemetryOptedIn)
        {
            return rawMessage;
        }

        if (discoveryCompletePayload != null)
        {
            if (discoveryCompletePayload.Metrics == null)
            {
                discoveryCompletePayload.Metrics = RequestData.MetricsCollection.Metrics;
            }
            else
            {
                foreach (var kvp in RequestData.MetricsCollection.Metrics)
                {
                    discoveryCompletePayload.Metrics[kvp.Key] = kvp.Value;
                }
            }

            var discoveryFinalTimeTakenForDesignMode = DateTime.UtcNow - _discoveryStartTime;

            // Collecting Total Time Taken
            discoveryCompletePayload.Metrics[TelemetryDataConstants.TimeTakenInSecForDiscovery] = discoveryFinalTimeTakenForDesignMode.TotalSeconds;

            // Add extensions discovered by vstest.console.
            //
            // TODO(copoiena):
            // Doing extension merging here is incorrect because we can end up not merging the
            // cached extensions for the current process (i.e. vstest.console) and hence have
            // an incomplete list of discovered extensions. This can happen because this method
            // is called only if telemetry is opted in (see: HandleRawMessage). We should handle
            // this merge a level above in order to be consistent, but that means we'd have to
            // deserialize all raw messages no matter if telemetry is opted in or not and that
            // would probably mean a performance hit.
            discoveryCompletePayload.DiscoveredExtensions = TestExtensions.CreateMergedDictionary(
                discoveryCompletePayload.DiscoveredExtensions,
                TestPluginCache.Instance.TestExtensions?.GetCachedExtensions());

            // Write extensions to telemetry data.
            TestExtensions.AddExtensionTelemetry(
                discoveryCompletePayload.Metrics,
                discoveryCompletePayload.DiscoveredExtensions);
        }

        if (message is VersionedMessage message1)
        {
            var version = message1.Version;

            rawMessage = _dataSerializer.SerializePayload(
                MessageType.DiscoveryComplete,
                discoveryCompletePayload,
                version);
        }
        else
        {
            rawMessage = _dataSerializer.SerializePayload(
                MessageType.DiscoveryComplete,
                discoveryCompletePayload);
        }

        return rawMessage;
    }

    #endregion

    #region IDisposable implementation

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or
    /// resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        EqtTrace.Verbose("DiscoveryRequest.Dispose: Starting.");

        lock (_syncObject)
        {
            if (!_isDisposed)
            {
                if (_discoveryCompleted != null)
                {
                    _discoveryCompleted.Dispose();
                }

                // Indicate that object has been disposed
                _discoveryCompleted = null!;
                _isDisposed = true;
            }
        }

        EqtTrace.Info("DiscoveryRequest.Dispose: Completed.");
    }

    #endregion
    /// <summary>
    /// Request Data
    /// </summary>
    internal IRequestData RequestData;

    /// <summary>
    /// If this request has been disposed.
    /// </summary>
    private bool _isDisposed;

    /// <summary>
    /// It get set when current discovery request is completed.
    /// </summary>
    private ManualResetEvent _discoveryCompleted = new(false);

    /// <summary>
    /// Sync object for various operations
    /// </summary>
    private readonly object _syncObject = new();

    /// <summary>
    /// Discovery Start Time
    /// </summary>
    private DateTime _discoveryStartTime;

}
