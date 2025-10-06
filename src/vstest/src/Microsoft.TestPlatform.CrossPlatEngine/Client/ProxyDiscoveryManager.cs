// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.Common;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client.Parallel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Host;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client;

/// <summary>
/// Orchestrates discovery operations for the engine communicating with the client.
/// </summary>
public class ProxyDiscoveryManager : IProxyDiscoveryManager, IBaseProxy, ITestDiscoveryEventsHandler2
{
    private readonly TestSessionInfo? _testSessionInfo;
    private readonly Func<string, ProxyDiscoveryManager, ProxyOperationManager>? _proxyOperationManagerCreator;
    private readonly DiscoveryDataAggregator _discoveryDataAggregator;
    private readonly IFileHelper _fileHelper;
    private readonly IDataSerializer _dataSerializer;

    private ITestRuntimeProvider? _testHostManager;
    private bool _isCommunicationEstablished;
    private ProxyOperationManager? _proxyOperationManager;
    private ITestDiscoveryEventsHandler2? _baseTestDiscoveryEventsHandler;
    private bool _skipDefaultAdapters;
    private string? _previousSource;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProxyDiscoveryManager"/> class.
    /// </summary>
    ///
    /// <param name="testSessionInfo">The test session info.</param>
    /// <param name="proxyOperationManagerCreator">The proxy operation manager creator.</param>
    public ProxyDiscoveryManager(
        TestSessionInfo testSessionInfo,
        Func<string, ProxyDiscoveryManager, ProxyOperationManager> proxyOperationManagerCreator)
        : this(testSessionInfo, proxyOperationManagerCreator, new())
    {
    }

    internal ProxyDiscoveryManager(
        TestSessionInfo testSessionInfo,
        Func<string, ProxyDiscoveryManager, ProxyOperationManager> proxyOperationManagerCreator,
        DiscoveryDataAggregator discoveryDataAggregator)
    {
        // Filling in test session info and proxy information.
        _testSessionInfo = testSessionInfo;
        _proxyOperationManagerCreator = proxyOperationManagerCreator;
        _discoveryDataAggregator = discoveryDataAggregator;
        _dataSerializer = JsonDataSerializer.Instance;
        _fileHelper = new FileHelper();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProxyDiscoveryManager"/> class.
    /// </summary>
    ///
    /// <param name="requestData">
    /// The request data for providing discovery services and data.
    /// </param>
    /// <param name="testRequestSender">Test request sender instance.</param>
    /// <param name="testHostManager">Test host manager instance.</param>
    public ProxyDiscoveryManager(
        IRequestData requestData,
        ITestRequestSender testRequestSender,
        ITestRuntimeProvider testHostManager)
        : this(requestData, testRequestSender, testHostManager, null, null, null)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProxyDiscoveryManager"/> class.
    /// </summary>
    ///
    /// <remarks>
    /// Constructor with dependency injection. Used for unit testing.
    /// </remarks>
    ///
    /// <param name="requestData">
    /// The request data for providing discovery services and data.
    /// </param>
    /// <param name="requestSender">The request sender.</param>
    /// <param name="testHostManager">Test host manager instance.</param>
    /// <param name="testhostManagerFramework">Framework of the manager.</param>
    /// <param name="discoveryDataAggregator">Aggregator of discovery data.</param>
    /// <param name="dataSerializer">The data serializer.</param>
    /// <param name="fileHelper">The file helper.</param>
    internal ProxyDiscoveryManager(
        IRequestData requestData,
        ITestRequestSender requestSender,
        ITestRuntimeProvider testHostManager,
        Framework? testhostManagerFramework,
        DiscoveryDataAggregator? discoveryDataAggregator = null,
        IDataSerializer? dataSerializer = null,
        IFileHelper? fileHelper = null)
    {
        _testHostManager = testHostManager;
        _discoveryDataAggregator = discoveryDataAggregator ?? new();
        _dataSerializer = dataSerializer ?? JsonDataSerializer.Instance;
        _fileHelper = fileHelper ?? new FileHelper();

        // Create a new proxy operation manager.
        _proxyOperationManager = new ProxyOperationManager(requestData, requestSender, testHostManager, testhostManagerFramework, this);
    }

    #region IProxyDiscoveryManager implementation.

    public void Initialize(bool skipDefaultAdapters)
    {
        _skipDefaultAdapters = skipDefaultAdapters;
    }

    /// <inheritdoc/>
    public void InitializeDiscovery(DiscoveryCriteria discoveryCriteria, ITestDiscoveryEventsHandler2 eventHandler, bool skipDefaultAdapters)
    {
        // Multiple method calls will iterate over this sources collection so we want to ensure
        // it's built once.
        var discoverySources = discoveryCriteria.Sources.ToArray();

        if (_proxyOperationManager == null)
        {
            TPDebug.Assert(_proxyOperationManagerCreator is not null, "_proxyOperationManagerCreator is null");
            // Passing only first because that is how the testhost pool is keyed.
            _proxyOperationManager = _proxyOperationManagerCreator(discoverySources[0], this);
            _testHostManager = _proxyOperationManager.TestHostManager;
        }

        _baseTestDiscoveryEventsHandler = eventHandler;

        // All sources are already marked as not discovered by ParallelProxyDiscoveryManager or
        // DiscoveryManager but it's possible to have this manager called directly so we still need
        // to ensure correct initial state.
        _discoveryDataAggregator.MarkSourcesWithStatus(discoverySources, DiscoveryStatus.NotDiscovered);

        try
        {
            _isCommunicationEstablished = _proxyOperationManager.SetupChannel(discoverySources, discoveryCriteria.RunSettings);

            if (_isCommunicationEstablished)
            {
                InitializeExtensions(discoverySources, skipDefaultAdapters);
            }
        }
        catch (Exception exception)
        {
            HandleException(exception);
        }
    }

    public void DiscoverTests(DiscoveryCriteria discoveryCriteria, ITestDiscoveryEventsHandler2 eventHandler)
    {
        // Multiple method calls will iterate over this sources collection so we want to ensure
        // it's built once.
        var discoverySources = discoveryCriteria.Sources.ToArray();

        try
        {
            if (!_isCommunicationEstablished)
            {
                InitializeDiscovery(discoveryCriteria, eventHandler, _skipDefaultAdapters);
            }

            TPDebug.Assert(_proxyOperationManager is not null, "ProxyOperationManager is null.");

            if (_isCommunicationEstablished)
            {
                discoveryCriteria.UpdateDiscoveryCriteria(_testHostManager!);

                // Consider the first source as the previous source so that if we are discovering a source
                // with no tests, we will always consider the source as fully discovered when reaching the
                // discovery complete event.
                _previousSource = discoverySources[0];

                _proxyOperationManager.RequestSender.DiscoverTests(discoveryCriteria, this);
            }
        }
        catch (Exception exception)
        {
            HandleException(exception);
        }
    }

    private void HandleException(Exception exception)
    {
        // If requested abort and the code below was just sending data, we will get communication exception because we try to write the channel that is already closed.
        // In such case don't report the exception because user cannot do anything about it.
        if (!(_proxyOperationManager != null && _proxyOperationManager.CancellationTokenSource.IsCancellationRequested && exception is CommunicationException))
        {
            EqtTrace.Error("ProxyDiscoveryManager.DiscoverTests: Failed to discover tests: {0}", exception);

            // Log to vs ide test output
            var testMessagePayload = new TestMessagePayload { MessageLevel = TestMessageLevel.Error, Message = exception.ToString() };
            var rawMessage = _dataSerializer.SerializePayload(MessageType.TestMessage, testMessagePayload);
            HandleRawMessage(rawMessage);

            // Log to vstest.console
            HandleLogMessage(TestMessageLevel.Error, exception.ToString());
        }

        // Send a discovery complete to caller. Similar logic is also used in ParallelProxyDiscoveryManager.DiscoverTestsOnConcurrentManager
        // Aborted is `true`: in case of parallel discovery (or non shared host), an aborted message ensures another discovery manager
        // created to replace the current one. This will help if the current discovery manager is aborted due to irreparable error
        // and the test host is lost as well.
        var discoveryCompletePayload = new DiscoveryCompletePayload
        {
            IsAborted = true,
            LastDiscoveredTests = null,
            TotalTests = -1
        };
        HandleRawMessage(_dataSerializer.SerializePayload(MessageType.DiscoveryComplete, discoveryCompletePayload));
        var discoveryCompleteEventsArgs = new DiscoveryCompleteEventArgs(-1, true);

        HandleDiscoveryComplete(discoveryCompleteEventsArgs, new List<TestCase>());
    }

    /// <inheritdoc/>
    public void Abort()
    {
        // Do nothing if the proxy is not initialized yet.
        if (_proxyOperationManager == null)
        {
            return;
        }

        // Cancel fast, try to stop testhost deployment/launch
        _proxyOperationManager.CancellationTokenSource.Cancel();
        Close();
    }

    // <inheritdoc/>
    public void Abort(ITestDiscoveryEventsHandler2 eventHandler)
    {
        // Do nothing if the proxy is not initialized yet.
        if (_proxyOperationManager is null)
        {
            return;
        }

        _baseTestDiscoveryEventsHandler ??= eventHandler;

        if (_isCommunicationEstablished)
        {
            _proxyOperationManager.RequestSender.SendDiscoveryAbort();
        }

        // It is important to ensure we cancel the token and close the channel in case of an abort.
        // If we don't then we will wait until the completion of this discovery. In case of a
        // parallel discovery that means we will wait until the source with the less number of tests
        // is discovered before notifying the caller with the aborted DiscoveryCompleteEventArgs.
        Abort();
    }

    /// <inheritdoc/>
    public void Close()
    {
        // Do nothing if the proxy is not initialized yet.
        if (_proxyOperationManager == null)
        {
            return;
        }

        // When no test session is being used, we don't share the testhost
        // between test discovery and test run. The testhost is closed upon
        // successfully completing the operation it was spawned for.
        //
        // In contrast, the new workflow (using test sessions) means we should keep
        // the testhost alive until explicitly closed by the test session owner, but
        // only if the testhost is part of a test session (i.e. the proxy operation manager
        // id is valid), since there is the distinct possibility of test session criteria
        // changing between spawn and discovery/run, causing a new proxy operation manager
        // to be spawned on demand instead of dequeuing an incompatible proxy from the pool.
        if (_testSessionInfo == null || _proxyOperationManager.Id < 0)
        {
            _proxyOperationManager.Close();
            return;
        }

        TestSessionPool.Instance.ReturnProxy(_testSessionInfo, _proxyOperationManager.Id);
    }

    /// <inheritdoc/>
    public void HandleDiscoveryComplete(DiscoveryCompleteEventArgs discoveryCompleteEventArgs, IEnumerable<TestCase>? lastChunk)
    {
        // Currently, TestRequestSender always passes null for lastChunk in case of an aborted
        // discovery but we are not making this assumption here to ease potential future
        // evolution.
        // When discovery is complete and not aborted, we can simply mark all given test cases and
        // the latest discovered source as fully discovered. Otherwise we still want to process
        // the last chunk as if it was a normal discovery notification.
        if (discoveryCompleteEventArgs.IsAborted)
        {
            _previousSource = _discoveryDataAggregator.MarkSourcesBasedOnDiscoveredTestCases(_previousSource, lastChunk);
        }
        else
        {
            _discoveryDataAggregator.MarkSourcesWithStatus(lastChunk?.Select(x => x.Source), DiscoveryStatus.FullyDiscovered);
            _discoveryDataAggregator.MarkSourcesWithStatus(new[] { _previousSource }, DiscoveryStatus.FullyDiscovered);
            _previousSource = null;
        }
        _baseTestDiscoveryEventsHandler?.HandleDiscoveryComplete(discoveryCompleteEventArgs, lastChunk);
    }

    /// <inheritdoc/>
    public void HandleDiscoveredTests(IEnumerable<TestCase>? discoveredTestCases)
    {
        _previousSource = _discoveryDataAggregator.MarkSourcesBasedOnDiscoveredTestCases(_previousSource, discoveredTestCases);
        _baseTestDiscoveryEventsHandler?.HandleDiscoveredTests(discoveredTestCases);
    }

    /// <inheritdoc/>
    public void HandleRawMessage(string rawMessage)
    {
        var message = _dataSerializer.DeserializeMessage(rawMessage);
        if (string.Equals(message.MessageType, MessageType.DiscoveryComplete))
        {
            Close();
        }

        _baseTestDiscoveryEventsHandler?.HandleRawMessage(rawMessage);
    }

    /// <inheritdoc/>
    public void HandleLogMessage(TestMessageLevel level, string? message)
    {
        _baseTestDiscoveryEventsHandler?.HandleLogMessage(level, message);
    }

    #endregion

    #region IBaseProxy implementation.
    /// <inheritdoc/>
    public virtual TestProcessStartInfo UpdateTestProcessStartInfo(TestProcessStartInfo testProcessStartInfo)
    {
        // Update Telemetry Opt in status because by default in Test Host Telemetry is opted out
        var telemetryOptedIn = _proxyOperationManager?.RequestData?.IsTelemetryOptedIn == true ? "true" : "false";
        testProcessStartInfo.Arguments += $" --telemetryoptedin {telemetryOptedIn}";
        return testProcessStartInfo;
    }
    #endregion

    private void InitializeExtensions(IEnumerable<string> sources, bool skipDefaultAdapters)
    {
        var extensions = TestPluginCache.Instance.GetExtensionPaths(TestPlatformConstants.TestAdapterEndsWithPattern, skipDefaultAdapters);

        // Filter out non existing extensions
        var nonExistingExtensions = extensions.Where(extension => !_fileHelper.Exists(extension));
        if (nonExistingExtensions.Any())
        {
            LogMessage(TestMessageLevel.Warning, string.Format(CultureInfo.CurrentCulture, Resources.Resources.NonExistingExtensions, string.Join(",", nonExistingExtensions)));
        }

        var sourceList = sources.ToList();
        var platformExtensions = _testHostManager?.GetTestPlatformExtensions(sourceList, extensions.Except(nonExistingExtensions));

        // Only send this if needed.
        if (platformExtensions is not null && platformExtensions.Any())
        {
            _proxyOperationManager?.RequestSender.InitializeDiscovery(platformExtensions);
        }
    }

    private void LogMessage(TestMessageLevel testMessageLevel, string message)
    {
        // Log to translation layer.
        var testMessagePayload = new TestMessagePayload { MessageLevel = testMessageLevel, Message = message };
        var rawMessage = _dataSerializer.SerializePayload(MessageType.TestMessage, testMessagePayload);
        HandleRawMessage(rawMessage);

        // Log to vstest.console layer.
        HandleLogMessage(testMessageLevel, message);
    }
}
